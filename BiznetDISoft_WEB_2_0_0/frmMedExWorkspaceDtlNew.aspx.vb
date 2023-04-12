Imports Newtonsoft.Json
Partial Class frmMedExWorkspaceDtlNew
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objcommon As New clsCommon
    Private objhelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()


    Private eStr As String = String.Empty
    Private StrValidation As String = String.Empty
    Private strActivityId As String
    Private ForCrfVersionControl As Integer = 0

    Private Const VS_CrfVersionDetail As String = "CrfVersionDetail"
    'Private Const VS_Choice As String = "Choice"
    Private Const VS_DtMedExWorkspaceMst As String = "DtMedExWorkspaceMst"
    'Private Const VS_DtBlankMedExWorkspaceMst As String = "DtBlankMedExWorkspaceMst"
    Private Const VS_MedExWorkspaceId As String = "MedExWorkspaceId"
    Private Const Vs_MedExWorkspaceHdrNo As String = "MedExWorkspaceHdrNo"
    Private Const Vs_MedExWorkspaceHdr As String = "DtMedExWorkspaceHdr"
    'Private Const Vs_DtBlankMedExWorkspaceHdr As String = "DtBlankMedExWorkspaceHdr"
    Private Const VS_MedExEditTemplate As String = "DtMedExEditTemplate"
    Private Const VS_MedExEditWorkspace As String = "DtMedExEditWorkspaceHdr"
    'Private Const VS_DtBlankMedExWorkspaceDtl As String = "DtBlankMedExWorkspaceDtl"
    Private Const Vs_UOM As String = "UOMMst"
    Private Const Vs_Status As String = "Status"
    ' Private Const Vs_MedexCode As String = "Medexcode"
    Private Const Vs_Dictionary As String = "DictionaryName"

    Private Const VS_DtMedExMst As String = "MedExMst" 'ddlMedEx
    Private Const VS_DtMedExWorkSpaceDtl As String = "MedExWorkSpaceDtl" 'gvwMedEx

    Private Const AddToGrid As String = "AddToGrid"
    Private Const AddToDatabase As String = "AddToDatabase"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_MedExWorkSpaceHdrNo As Integer = 1
    Private Const GVC_WorkSpaceID As Integer = 2
    Private Const GVC_ProjectNo As Integer = 3
    Private Const GVC_ProjectName As Integer = 4
    Private Const GVC_ActivityID As Integer = 5
    Private Const GVC_ActivityName As Integer = 6
    Private Const GVC_MedExWorkSpaceEdit As Integer = 7
    Private Const GVC_MedExWorkSpaceDetach As Integer = 8
    Private Const GVC_MedExWorkSpacePreview As Integer = 9
    Private Const GVC_iNodeId As Integer = 10
    Private Const GVC_Reorder As Integer = 11

    Private Const gvc_porjectscreening_detach As Integer = 6
    Private Const gvc_porjectscreening_order As Integer = 8


    Private Const GVC_MedExTemplateId As Integer = 0
    Private Const GVC_TemplateName As Integer = 2

    Private Const GvwMedEx_Select As Integer = 0
    Private Const GvwMedEx_Details As Integer = 1
    Private Const GvwMedEx_MedExWorkSpaceDtlNo As Integer = 2
    Private Const GvwMedEx_MedExWorkSpaceHdrNo As Integer = 3
    Private Const GvwMedEx_MedExCode As Integer = 4
    Private Const GvwMedEx_MedExDesc As Integer = 5
    Private Const GvwMedEx_MedExType As Integer = 6
    Private Const GvwMedEx_MedExValue As Integer = 7
    Private Const GvwMedEx_DefaultValue As Integer = 8
    Private Const GvwMedEx_Alertonvalue As Integer = 9
    Private Const GvwMedEx_AlertMessage As Integer = 10
    Private Const GvwMedEx_LowRang As Integer = 11
    Private Const GvwMedEx_HighRange As Integer = 12
    Private Const GvwMedEx_ActiveFlag As Integer = 13
    Private Const GvwMedEx_SeqNo As Integer = 14
    Private Const GvwMedEx_vMedExGroupCode As Integer = 15
    Private Const GvwMedEx_vmedexgroupDesc As Integer = 16
    Private Const GvwMedEx_vMedexGroupCDISCValue As Integer = 17
    Private Const GvwMedEx_vmedexGroupOtherValue As Integer = 18
    Private Const GvwMedEx_vMedExSubGroupCode As Integer = 19
    Private Const GvwMedEx_vmedexsubGroupDesc As Integer = 20
    Private Const GvwMedEx_vMedexSubGroupCDISCValue As Integer = 21
    Private Const GvwMedEx_vmedexsubGroupOtherValue As Integer = 22
    Private Const GvwMedEx_vUOM As Integer = 23
    Private Const GvwMedEx_vValidationType As Integer = 24
    Private Const GvwMedEx_Length As Integer = 25
    Private Const GvwMedEx_cAlertType As Integer = 26
    Private Const GvwMedEx_cRefType As Integer = 27
    Private Const GvwMedEx_vRefTable As Integer = 28
    Private Const GvwMedEx_vRefColumn As Integer = 29
    Private Const GvwMedEx_vRefFilePath As Integer = 30
    Private Const GvwMedEx_vCDISCValues As Integer = 31
    Private Const GvwMedEx_vOtherValues As Integer = 32
    Private Const GvwMedEx_MeddraDropdown As Integer = 33
    'Private Const GvwMedEx_Delete As Integer = 33
    'Private Const GvwMedEx_AlertMessage As Integer = 33
    Private Const GvwMedEx_vUOMHdn As Integer = 34
    Private Const GvwMedEx_vValidationTypeHdn As Integer = 35
    Private Const GvwMedEx_cAlertTypeHdn As Integer = 36
    Private Const GvMedex_vMedexFormula As Integer = 37
    Private Const GvMedex_iDecimal As Integer = 38
    Private Const GvMedex_MedExTypeHdn As Integer = 40


    Private Const GVPrSpScr_nWorkSpaceScreeningHdrNo As Integer = 1
    Private Const GVPrSpScr_vWorkspaceID As Integer = 2
    Private Const GVPrSpScr_vProjectNo As Integer = 3
    Private Const GVPrSpScr_vActivityName As Integer = 4

    Private Const Gv_GeneralScr_nScreeningTemplateHdrNo As Integer = 1
    Private Const Gv_GeneralScr_vWorkspaceID As Integer = 2
    Private Const Gv_GeneralScr_vProjectNo As Integer = 3
    Private Const Gv_GeneralScr_vActivityName As Integer = 4
    Private Const Gv_GeneralScr_Edit As Integer = 6
    Private Const Gv_GeneralScr_Versionno As Integer = 5
    Private Const Gv_GeneralScr_Deatch As Integer = 7
    Private Const Gv_GeneralScr_Order As Integer = 9
    Private Const Gv_GeneralScr_Status As Integer = 10
    Private Const Gv_GeneralScr_StatusForText As Integer = 11


    Private Const GV_ProjectSpScr_Select As Integer = 0
    Private Const GV_ProjectSpScr_Details As Integer = 1
    Private Const GV_ProjectSpScr_nWorkSpaceScreeningDtlNo As Integer = 2
    Private Const GV_ProjectSpScr_nWorkSpaceScreeningHdrNo As Integer = 3
    Private Const GV_ProjectSpScr_MedExCode As Integer = 4
    Private Const GV_ProjectSpScr_MedExDesc As Integer = 5
    Private Const GV_ProjectSpScr_MedExType As Integer = 6
    Private Const GV_ProjectSpScr_MedExValue As Integer = 7
    Private Const GV_ProjectSpScr_DefaultValue As Integer = 8
    Private Const GV_ProjectSpScr_Alertonvalue As Integer = 9
    Private Const GV_ProjectSpScr_AlertMessage As Integer = 10
    Private Const GV_ProjectSpScr_LowRang As Integer = 11
    Private Const GV_ProjectSpScr_HighRange As Integer = 12
    Private Const GV_ProjectSpScr_ActiveFlag As Integer = 13
    Private Const GV_ProjectSpScr_SeqNo As Integer = 14
    Private Const GV_ProjectSpScr_vMedExGroupCode As Integer = 15
    Private Const GV_ProjectSpScr_vmedexgroupDesc As Integer = 16
    Private Const GV_ProjectSpScr_vMedexGroupCDISCValue As Integer = 17
    Private Const GV_ProjectSpScr_vmedexGroupOtherValue As Integer = 18
    Private Const GV_ProjectSpScr_vMedExSubGroupCode As Integer = 19
    Private Const GV_ProjectSpScr_vmedexsubGroupDesc As Integer = 20
    Private Const GV_ProjectSpScr_vMedexSubGroupCDISCValue As Integer = 21
    Private Const GV_ProjectSpScr_vmedexsubGroupOtherValue As Integer = 22
    Private Const GV_ProjectSpScr_vUOM As Integer = 23
    Private Const GV_ProjectSpScr_vValidationType As Integer = 24
    Private Const GV_ProjectSpScr_Length As Integer = 25
    Private Const GV_ProjectSpScr_cAlertType As Integer = 26
    Private Const GV_ProjectSpScr_cRefType As Integer = 27
    Private Const GV_ProjectSpScr_vRefTable As Integer = 28
    Private Const GV_ProjectSpScr_vRefColumn As Integer = 29
    Private Const GV_ProjectSpScr_vRefFilePath As Integer = 30
    Private Const GV_ProjectSpScr_vCDISCValues As Integer = 31
    Private Const GV_ProjectSpScr_vOtherValues As Integer = 32
    'Private Const GV_ProjectSpScr_AlertMessage As Integer = 32
    'Private Const GV_ProjectSpScr_Delete As Integer = 32
    Private Const GV_ProjectSpScr_vUOMHdn As Integer = 33
    Private Const GV_ProjectSpScr_vValidationTypeHdn As Integer = 34
    Private Const GV_ProjectSpScr_cAlertTypeHdn As Integer = 35
    Private Const Gv_ProjectSprScr_vMedexFormula As Integer = 36
    Private Const Gv_ProjectSprScr_iDecimalNo As Integer = 37

    Private Const GvGenScr_Medex_Select As Integer = 0
    Private Const GvGenScr_Medex_Details As Integer = 1
    Private Const GvGenScr_Medex_nScreeningTemplateDtlNo As Integer = 2
    Private Const GvGenScr_Medex_nScreeningTemplateHdrNo As Integer = 3
    Private Const GvGenScr_Medex_MedExCode As Integer = 4
    Private Const GvGenScr_Medex_MedExDesc As Integer = 5
    Private Const GvGenScr_Medex_MedExType As Integer = 6
    Private Const GvGenScr_Medex_MedExValue As Integer = 7
    Private Const GvGenScr_Medex_DefaultValue As Integer = 8
    Private Const GvGenScr_Medex_Alertonvalue As Integer = 9
    Private Const GvGenScr_Medex_AlertMessage As Integer = 10
    Private Const GvGenScr_Medex_LowRang As Integer = 11
    Private Const GvGenScr_Medex_HighRange As Integer = 12
    Private Const GvGenScr_Medex_ActiveFlag As Integer = 13
    Private Const GvGenScr_Medex_SeqNo As Integer = 14
    Private Const GvGenScr_Medex_vMedExGroupCode As Integer = 15
    Private Const GvGenScr_Medex_vmedexgroupDesc As Integer = 16
    Private Const GvGenScr_Medex_vMedexGroupCDISCValue As Integer = 17
    Private Const GvGenScr_Medex_vmedexGroupOtherValue As Integer = 18
    Private Const GvGenScr_Medex_vMedExSubGroupCode As Integer = 19
    Private Const GvGenScr_Medex_vmedexsubGroupDesc As Integer = 20
    Private Const GvGenScr_Medex_vMedexSubGroupCDISCValue As Integer = 21
    Private Const GvGenScr_Medex_vmedexsubGroupOtherValue As Integer = 22
    Private Const GvGenScr_Medex_vUOM As Integer = 23
    Private Const GvGenScr_Medex_vValidationType As Integer = 24
    Private Const GvGenScr_Medex_Length As Integer = 25
    Private Const GvGenScr_Medex_cAlertType As Integer = 26
    Private Const GvGenScr_Medex_cRefType As Integer = 27
    Private Const GvGenScr_Medex_vRefTable As Integer = 28
    Private Const GvGenScr_Medex_vRefColumn As Integer = 29
    Private Const GvGenScr_Medex_vRefFilePath As Integer = 30
    Private Const GvGenScr_Medex_vCDISCValues As Integer = 31
    Private Const GvGenScr_Medex_vOtherValues As Integer = 32
    'Private Const GvGenScr_Medex_Delete As Integer = 32
    'Private Const GvGenScr_Medex_AlertMessage As Integer = 32
    Private Const GvGenScr_Medex_vUOMHdn As Integer = 33
    Private Const GvGenScr_Medex_vValidationTypeHdn As Integer = 34
    Private Const GvGenScr_Medex_cAlertTypeHdn As Integer = 35
    Private Const GvGenScr_Medex_vMedexFormula As Integer = 36
    Private Const GvGenScr_Medex_iDecimalNo As Integer = 37



    Private GenericScreeningIsFreeze As Boolean

#End Region

#Region "Page Load "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                GenCall()
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....Page_Load")
        End Try
    End Sub
#End Region

#Region "GenCall "
    Private Function GenCall() As Boolean
        Dim ds As New DataSet

        Try

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If

            GenCall = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....GenCall")
        Finally
        End Try
    End Function
#End Region

#Region "GenCall_Data "

    Private Function GenCall_Data(ByRef ds_DWR_Retu As DataSet) As Boolean
        Try

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_Data")

        Finally

        End Try
    End Function

#End Region

#Region "GenCall_showUI "

    Private Function GenCall_ShowUI() As Boolean
        Try



            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Project Attribute Management"
            Page.Title = ":: Project Attribute Management :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID) & " And cWorkspaceType='P'"

            If Not FillDropdown() Then
                Exit Function
            End If
            If Not FillScreeningGroup() Then
                Exit Function
            End If

            RBLProjecttype_SelectedIndexChanged(Nothing, Nothing)

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....GenCall_ShowUI")
        Finally
        End Try
    End Function

#End Region

#Region "Fill Functions"

#Region "Fill DropDown"

    Private Function FillDropdown() As Boolean
        Dim eStr_Retu As String = String.Empty
        Dim Dv_FillTemplate As New DataView
        Dim Ds_FillTemplate As New DataSet
        Dim wstr As String = String.Empty
        Try
            If Not objcommon.GetScopeValueWithCondition(wstr) Then
                Return False
            End If

            If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                wstr += " And vMedExType <> 'IMPORT'"
            End If

            If objhelp.GetMedExTemplateDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                    Ds_FillTemplate, eStr_Retu) Then

                Dv_FillTemplate = Ds_FillTemplate.Tables(0).DefaultView
                Dv_FillTemplate.Sort = "vTemplateName"
                Me.ddlTemplate.DataSource = Dv_FillTemplate.ToTable(True, "vMedExTemplateId,vTemplateNameWithID".Split(","))
                Me.ddlTemplate.DataValueField = "vMedExTemplateId"
                Me.ddlTemplate.DataTextField = "vTemplateNameWithID"
                Me.ddlTemplate.DataBind()
                Me.ddlTemplate.Items.Insert(0, New ListItem("Select Template", "0"))

            End If
            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "......FillDropdown")
        End Try
    End Function

#End Region

#Region "FillDropDown For MedEx"

    Private Function FillddlMedexGroup() As Boolean
        Dim ds_MedexGroup As New DataSet
        Dim estr As String = ""
        Dim wstr As String = ""
        Try
            If Not objcommon.GetScopeValueWithCondition(wstr) Then
                Return False
            End If

            wstr += " And cStatusIndi <> 'D' order by vMedExGroupDesc"
            If Not objhelp.GetMedExMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                            ds_MedexGroup, estr) Then
                Me.objcommon.ShowAlert("Error While Getting Data from Attribute GroupMst:" + estr, Me.Page)
                Return False
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function

    Private Function FillUOM() As Boolean
        Dim Ds_FillUOMMst As New Data.DataSet
        Dim wstr As String = ""
        Dim estr As String = ""

        Try
            If Not objhelp.GetUOMMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, Ds_FillUOMMst, estr) Then
                Throw New Exception(estr)
            End If
            Me.ViewState(Vs_UOM) = Ds_FillUOMMst.Tables(0)
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....FillUOM")
            Return False
        End Try
    End Function

    Private Function FillScreeningGroup() As Boolean
        Dim ds_MedexGroup As New DataSet
        Dim estr As String = ""
        Dim wstr As String = ""
        Try
            If Not objcommon.GetScopeValueWithCondition(wstr) Then
                Return False
            End If

            Me.ddlScreeningGroup.Items.Clear()

            If RBLProjecttype.SelectedIndex = 0 Then
                wstr = HProjectId.Value + "##" + Session(S_LocationCode)
            ElseIf RBLProjecttype.SelectedIndex = 1 Then
                wstr = "0000000000" + "##" + Session(S_LocationCode)
            End If


            ds_MedexGroup = Me.objhelp.ProcedureExecute("dbo.Proc_UniqueGetScreeningGroup", wstr)

            If Not ds_MedexGroup Is Nothing AndAlso ds_MedexGroup.Tables(0).Rows.Count > 0 Then
                Me.ddlScreeningGroup.DataSource = ds_MedexGroup.Tables(0).DefaultView.ToTable(True, "vMedExGroupCode,vMedExGroupDesc".Split(","))
                Me.ddlScreeningGroup.DataValueField = "vMedExGroupCode"
                Me.ddlScreeningGroup.DataTextField = "vMedExGroupDesc"
                Me.ddlScreeningGroup.DataBind()
                Me.ddlScreeningGroup.Items.Insert(0, New ListItem("Select Screening Group", 0))
                Me.ddlScreeningGroup.Items.Insert(ds_MedexGroup.Tables(0).Rows.Count + 1, New ListItem("Add New Group", "XXX"))
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function

#End Region

#Region "FillGrid For MedEx"

    Private Function FillGridgvwMedEx(ByVal vWorkspaceId As String, ByVal iNodeId As String) As Boolean
        Dim ds_gvwMedEx As New Data.DataSet
        Dim dt_gvwMedEx As New DataTable
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Try

            wstr = "vWorkspaceId='" + vWorkspaceId.ToString.Trim() + "' And iNodeId=" + iNodeId.ToString.Trim() + " And cStatusIndi <> 'D' And cActiveFlag <> 'N' "
            If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                wstr += " And vMedExType <> 'IMPORT'"
            End If

            wstr += " Order by iSeqNo,nMedExWorkSpaceHdrNo"
            If Not objhelp.GetViewMedExWorkSpaceDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_gvwMedEx, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From View_MedExWorkSpaceDtl", estr)
                Return False
            End If

            Me.ViewState(VS_DtMedExWorkSpaceDtl) = ds_gvwMedEx.Tables(0)
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".........FillGridgvwMedEx")
            Return False
        End Try
    End Function

    Private Function FillGridgvwMedExForProjectSpScr(ByVal nWorkSpaceScreeningHdrNo As Integer) As Boolean
        Dim ds_gvwMedEx As New Data.DataSet
        Dim dt_gvwMedEx As New DataTable
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Try

            wstr = "nWorkSpaceScreeningHdrNo = " + nWorkSpaceScreeningHdrNo.ToString() + " And cStatusIndi <> 'D' And cActiveFlag <> 'N' AND vMedExGroupCode = '" + ddlScreeningGroup.SelectedValue + "'"

            If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                wstr += " And vMedExType <> 'IMPORT'"
            End If

            wstr += " Order by iSeqNo,nWorkSpaceScreeningHdrNo"
            If Not objhelp.View_WorkspaceScreeningHdrDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_gvwMedEx, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From View_MedExWorkSpaceDtl", estr)
                Return False
            End If

            Me.ViewState(VS_DtMedExWorkSpaceDtl) = ds_gvwMedEx.Tables(0)

            If Not ds_gvwMedEx.Tables(0).Rows.Count >= 1 Then
                Return True
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".........FillGridgvwMedExForProjectSpScr")
            Return False
        End Try
    End Function

    Private Function FillGridgvwMedExForGenScr(ByVal nScreeningTemplateHdrNo As Integer) As Boolean
        Dim ds_gvwMedEx As New Data.DataSet
        Dim dt_gvwMedEx As New DataTable
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Try
            wstr = "nScreeningTemplateHdrNo = " + nScreeningTemplateHdrNo.ToString() + " And cStatusIndi <> 'D' And cActiveFlag <> 'N' "

            If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                wstr += " And vMedExType <> 'IMPORT'"
            End If

            If Me.ddlScreeningGroup.SelectedIndex > 0 Then
                wstr += " AND vMedExGroupCode = '" + ddlScreeningGroup.SelectedValue + "'"
            End If

            wstr += " Order by iSeqNo,nScreeningTemplateHdrNo"

            If Not objhelp.View_GeneralScreeningHdrDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_gvwMedEx, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From View_MedExWorkSpaceDtl", estr)
                Return False
            End If
            Me.ViewState(VS_DtMedExWorkSpaceDtl) = ds_gvwMedEx.Tables(0)

            If Not ds_gvwMedEx.Tables(0).Rows.Count >= 1 Then
                Return True
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".........FillGridgvwMedExForProjectSpScr")
            Return False
        End Try
    End Function

#End Region

#Region "Fill For MedEx Sequence"

    Private Function FillMedExDesc(ByVal vWorkspaceId As String, ByVal iNodeId As String) As Boolean
        Dim ds_gvwMedEx As New Data.DataSet
        Dim dt_gvwMedEx As New DataTable
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Try
            wstr = "vWorkspaceId='" + vWorkspaceId.ToString.Trim() + "' And iNodeId=" + iNodeId.ToString.Trim() + " And cStatusIndi <> 'D' And cActiveFlag <> 'N' "
            If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                wstr += " And vMedExType <> 'IMPORT'"
            End If

            wstr += " Order by iSeqNo,nMedExWorkSpaceHdrNo"
            If Not objhelp.GetViewMedExWorkSpaceDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_gvwMedEx, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From View_MedExWorkSpaceDtl", estr)
                Return False
            End If


            Me.ViewState(VS_DtMedExWorkSpaceDtl) = ds_gvwMedEx.Tables(0)

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".........FillMedExDesc")
            Return False
        End Try
    End Function

    Private Function FillGenScrMedexDesc(ByVal nScreeningTemplateHdrNo As Integer) As Boolean
        Dim ds_gvwMedEx As New Data.DataSet
        Dim dt_gvwMedEx As New DataTable
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Try
            wstr = "nScreeningTemplateHdrNo = " + nScreeningTemplateHdrNo.ToString() + " And cStatusIndi <> 'D' And cActiveFlag <> 'N' "
            If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                wstr += " And vMedExType <> 'IMPORT'"
            End If
            If ddlScreeningGroup.SelectedIndex > 0 Then
                wstr += " And vMedExGroupCode = " + ddlScreeningGroup.SelectedItem.Value.Trim
            End If
            wstr += " Order by iSeqNo,nScreeningTemplateHdrNo"
            If Not objhelp.View_GeneralScreeningHdrDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_gvwMedEx, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From View_MedExWorkSpaceDtl", estr)
                Return False
            End If
            Me.ViewState(VS_DtMedExWorkSpaceDtl) = ds_gvwMedEx.Tables(0)
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".........FillGenScrMedexDesc")
            Return False
        End Try
    End Function

    Private Function FillMedExDescForProjectSpScr(ByVal nWorkSpaceScreeningHdrNo As Integer) As Boolean
        Dim ds_gvwMedEx As New Data.DataSet
        Dim dt_gvwMedEx As New DataTable
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Try

            wstr = "nWorkSpaceScreeningHdrNo = " + nWorkSpaceScreeningHdrNo.ToString() + " And cStatusIndi <> 'D' And cActiveFlag <> 'N' "

            If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                wstr += " And vMedExType <> 'IMPORT'"
            End If

            wstr += " Order by iSeqNo,nWorkSpaceScreeningHdrNo"

            If Not objhelp.View_WorkspaceScreeningHdrDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_gvwMedEx, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From View_MedExWorkSpaceDtl", estr)
                Return False
            End If


            Me.ViewState(VS_DtMedExWorkSpaceDtl) = ds_gvwMedEx.Tables(0)


            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".........FillMedExDescForProjectSpScr")
            Return False
        End Try
    End Function

#End Region

#Region "Fill Dictionary"

    Private Function fillRefTable(ByRef ds_Tables As DataSet) As Boolean
        Dim wstr As String = String.Empty
        wstr = "cRefTableType ='D' And cActiveFlag ='Y' AND cStatusIndi <> 'D' "
        If Not objhelp.GetReferenceTableDefinitions(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Tables, eStr) Then
            Me.objcommon.ShowAlert(eStr, Me.Page())
            Exit Function
        End If
        Return True

    End Function

#End Region

#Region "FillEditedgrid"
    Private Function FillEditedgrid(ByVal dt As DataTable) As Boolean
        Dim dvTemp As New DataView
        dvTemp = dt.DefaultView
        dvTemp.RowFilter = "cStatusIndi <> 'D' And cActiveFlag <> 'N'"

        If Me.RBLProjecttype.SelectedValue = 1 Then
            dvTemp.Sort = "iSeqNo,nMedExWorkSpaceHdrNo"
            dt = dvTemp.ToTable()
        ElseIf Me.RBLProjecttype.SelectedValue = 2 Then
            dvTemp.Sort = "iSeqNo,nWorkSpaceScreeningHdrNo"
            dt = dvTemp.ToTable()
        Else
            dvTemp.Sort = "iSeqNo,nScreeningTemplateHdrNo"
            dt = dvTemp.ToTable()
        End If
        Return True

    End Function
#End Region

    Private Function FillMedExListBox(ByVal vWorkspaceId As String, ByVal nMedExWorkSpaceHdrNo As String, ByVal nMedExWorkSpaceDtlNo As String) As Boolean
        Dim ds_gvwMedEx As New Data.DataSet
        Dim dt_gvwMedEx As New DataTable
        Dim dv As DataView
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim txt As String = String.Empty
        Dim txt_arr() As String
        Dim strFormula As String = String.Empty
        Try

            Me.lstMedEx.DataSource = Nothing
            Me.txtFormula.Text = ""

            If Me.RBLProjecttype.SelectedValue.ToString() = "1" Then

                wstr = "vWorkspaceId='" + vWorkspaceId.ToString.Trim() + "' And nMedExWorkSpaceHdrNo=" + nMedExWorkSpaceHdrNo.ToString.Trim() + " And cStatusIndi <> 'D' And cActiveFlag <> 'N' "

                If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                    wstr += " And vMedExType <> 'IMPORT'"
                End If

                wstr += " Order by iSeqNo,nMedExWorkSpaceHdrNo"

                '--------- Modify on 02-July-2009-----------------------
                If Not objhelp.GetViewMedExWorkSpaceDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_gvwMedEx, estr) Then
                    Me.ShowErrorMessage("Error While Getting Data From View_MedExWorkSpaceDtl", estr)
                    Return False
                End If

            ElseIf Me.RBLProjecttype.SelectedValue.ToString() = "0000000000" Then

                wstr = "nScreeningTemplateHdrNo = " + nMedExWorkSpaceHdrNo.ToString() + " And cStatusIndi <> 'D' And cActiveFlag <> 'N' "

                If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                    wstr += " And vMedExType <> 'IMPORT'"
                End If

                wstr += " Order by iSeqNo,nScreeningTemplateHdrNo"

                '--------- Modify on 02-July-2009-----------------------
                If Not objhelp.View_GeneralScreeningHdrDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_gvwMedEx, estr) Then
                    Me.ShowErrorMessage("Error While Getting Data From View_MedExWorkSpaceDtl", estr)
                    Return False
                End If
            ElseIf Me.RBLProjecttype.SelectedValue.ToString() = "2" Then

                wstr = "nWorkSpaceScreeningHdrNo = " + nMedExWorkSpaceHdrNo.ToString() + " And cStatusIndi <> 'D' And cActiveFlag <> 'N' "

                If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                    wstr += " And vMedExType <> 'IMPORT'"
                End If

                wstr += " Order by iSeqNo,nWorkSpaceScreeningHdrNo"

                '--------- Modify on 02-July-2009-----------------------
                If Not objhelp.View_WorkspaceScreeningHdrDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_gvwMedEx, estr) Then
                    Me.ShowErrorMessage("Error While Getting Data From View_MedExWorkSpaceDtl", estr)
                    Return False
                End If
            End If
            dv = ds_gvwMedEx.Tables(0).DefaultView
            dt_gvwMedEx = dv.ToTable()
            dt_gvwMedEx.Columns.Add("FullMedExDescription")

            For Each dr As DataRow In dt_gvwMedEx.Rows
                dr("FullMedExDescription") = dr("vMedExDesc").ToString.Trim() + _
                                            " - " + dr("vMedExSubGroupDesc").ToString.Trim() + _
                                            " - " + dr("vMedExGroupDesc").ToString.Trim() + _
                                            " - " + dr("vMedexType").ToString.Trim().ToUpper() + _
                                            "  @@" + dr("vMedExCode").ToString.Trim().ToUpper()
            Next
            dt_gvwMedEx.AcceptChanges()

            Me.lstMedEx.DataSource = dt_gvwMedEx
            Me.lstMedEx.DataTextField = dt_gvwMedEx.Columns("FullMedExDescription").ColumnName.Trim()

            Me.lstMedEx.DataValueField = dt_gvwMedEx.Columns("vMedExCode").ColumnName.Trim()
            Me.lstMedEx.DataBind()
            dv = ds_gvwMedEx.Tables(0).DefaultView
            Me.HFFormulaNo.Value = nMedExWorkSpaceDtlNo
            If Me.RBLProjecttype.SelectedValue = 1 Then

                dv.RowFilter = " nMedExWorkSpaceDtlNo = " + nMedExWorkSpaceDtlNo
            ElseIf Me.RBLProjecttype.SelectedValue = "0000000000" Then

                dv.RowFilter = " nScreeningTemplateDtlNo = " + nMedExWorkSpaceDtlNo
            ElseIf Me.RBLProjecttype.SelectedValue.ToString() = "2" Then

                dv.RowFilter = " nWorkspaceScreeningDtlNo = " + nMedExWorkSpaceDtlNo
            End If

            If dv.ToTable().Rows.Count = 0 Then
                Return True
            End If

            txt = dv.ToTable().Rows(0)("vMedExFormula").ToString()
            Me.HFFormula.Value = txt
            Me.HFFormula_Final.Value = txt
            txt_arr = txt.Split("?")

            For i = 0 To txt_arr.Length - 1
                If txt_arr(i).Length = 5 Then
                    txt = txt.Replace(txt_arr(i).Trim(), Me.lstMedEx.Items.FindByValue(txt_arr(i).Trim()).Text.Trim())
                    strFormula += Me.lstMedEx.Items.FindByValue(txt_arr(i).Trim()).Text.Trim()
                Else
                    strFormula += txt_arr(i)
                End If

            Next i

            txt = txt.Replace("?", "")
            Me.txtFormula.Text = strFormula
            Me.txtDecimal.Text = dv.ToTable().Rows(0)("iDecimalNo").ToString()

            If Me.Hdn_FreezeStatus.Value = "F" Or Me.Hdn_ProjectLock.Value = "L" Then
                btnSaveFormula.Visible = False
            Else
                btnSaveFormula.Visible = True
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("Error While Filling Attribute ListBox on project specific. ", ex.Message)
            Return False
        End Try
    End Function

#End Region

#Region "BindGrid"
    Private Function BindGrid() As Boolean
        Dim Ds_WorkspaceGrid As New DataSet
        Dim eStr_Retu As String = String.Empty
        Dim wstrProject As String = String.Empty
        Try
            wstrProject = " vProjectTypeCode in (" + Me.Session(S_ScopeValue).ToString() + ")"
            If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                wstrProject += " And vMedExType <> 'IMPORT'"
            End If
            If Me.RBLProjecttype.SelectedValue = 1 Then
                wstrProject += " And vWorkspaceid='" + Me.HProjectId.Value.Trim + "'"
                If Not objhelp.GetVIEWMedExWorkspaceHdr(wstrProject + " And cActiveFlag <> 'N' And cStatusindi <> 'D' Order by vWorkspaceDesc,ActivityDisplayName", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_WorkspaceGrid, eStr_Retu) Then
                    Throw New Exception("Error while Getting Project Structure...")
                End If
                gvmedexworkspadce.DataSource = Nothing
                gvmedexworkspadce.DataBind()

                gvmedexworkspadce.DataSource = Ds_WorkspaceGrid.Tables(0).DefaultView.ToTable(True, "nMedExWorkspaceHdrNo,vWorkspaceId,vWorkspaceDesc,vActivityId,ActivityDisplayName,vProjectNo,iNodeId".Split(","))
                gvmedexworkspadce.DataBind()
                If Me.Hdn_ProjectLock.Value = "L" Then
                    gvmedexworkspadce.Columns(GVC_MedExWorkSpaceDetach).Visible = False
                    gvmedexworkspadce.Columns(GVC_Reorder).Visible = False
                Else
                    gvmedexworkspadce.Columns(GVC_MedExWorkSpaceDetach).Visible = True
                    gvmedexworkspadce.Columns(GVC_Reorder).Visible = True
                End If

                Me.PanelProjectSpecific.Style.Add("display", "")
                Me.pnlgvmedexworkspadce.Style.Add("display", "")
                Me.tblmedexworkspadce.Style.Add("display", "")
                Me.tblGeneralScr.Style.Add("display", "none")
                Me.tblProjectSpcScreening.Style.Add("display", "none")
                Me.Collpase1.ClientState = True
                Me.Collpase1.Collapsed = True
                Me.lblprojecthdr.Text = "Project Details : " + Me.txtproject.Text.Trim()
                Return True
            ElseIf Me.RBLProjecttype.SelectedValue = "0000000000" Then
                wstrProject = " vWorkspaceid='0000000000' And cActiveFlag <> 'N' And cStatusindi <> 'D' And vLocationCode like'%" + Session(S_LocationCode) + "%'"
                If Not objhelp.View_GeneralScreeningHdrDtl(wstrProject, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_WorkspaceGrid, eStr_Retu) Then
                    Throw New Exception("Error while Getting Project Structure...")
                End If
                Ds_WorkspaceGrid.Tables(0).DefaultView.Sort = "nScreeningTemplateHdrNo ASC"
                ViewState("Gv_GeneralScr") = Ds_WorkspaceGrid.Tables(0).DefaultView.ToTable(True, "nScreeningTemplateHdrNo,vWorkspaceId,vProjectNo,vActivityName,vScreeningTemplateVersionName,cFreezeStatus".Split(","))
                Me.Gv_GeneralScr.DataSource = Ds_WorkspaceGrid.Tables(0).DefaultView.ToTable(True, "nScreeningTemplateHdrNo,vWorkspaceId,vProjectNo,vActivityName,vScreeningTemplateVersionName,cFreezeStatus".Split(","))
                Me.Gv_GeneralScr.DataBind()
                Me.PanelProjectSpecific.Style.Add("display", "")
                Me.pnlgvmedexworkspadce.Style.Add("display", "")
                Me.tblmedexworkspadce.Style.Add("display", "none")
                Me.tblGeneralScr.Style.Add("display", "")
                Me.tblProjectSpcScreening.Style.Add("display", "none")
                Me.Collpase1.ClientState = True
                Me.Collpase1.Collapsed = True
                Me.CollapsiblePanelExtender1.ClientState = False
                Me.CollapsiblePanelExtender1.Collapsed = False
                Me.lblprojecthdr.Text = "Generic Screening"
                For Each dr As DataRow In Ds_WorkspaceGrid.Tables(0).DefaultView.ToTable(True, "nScreeningTemplateHdrNo,vWorkspaceId,vProjectNo,vActivityName,vScreeningTemplateVersionName,cFreezeStatus".Split(",")).Rows
                    hdnVersion.Value = "Version " + Convert.ToString((Convert.ToInt64(Convert.ToString(dr("vScreeningTemplateVersionName")).Replace("Version", "").Trim()) + 1))
                Next
                Return True


            ElseIf Me.RBLProjecttype.SelectedValue = 2 Then

                Me.gvmedexworkspadce.DataSource = Nothing
                Me.gvmedexworkspadce.DataBind()

                wstrProject += " And vWorkspaceid='" + Me.HProjectId.Value.Trim + "' And cStatusIndi<>'D' And vMedExType <> 'IMPORT'"
                If Not Me.objhelp.View_WorkspaceScreeningHdrDtl(wstrProject, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_WorkspaceGrid, eStr_Retu) Then
                    Throw New Exception("Error while getting Project Specific Screening Structure...." & eStr_Retu.ToString())
                End If
                If Not Ds_WorkspaceGrid.Tables(0).Rows.Count <= 0 Then
                    Me.GVProjectSpcScreening.DataSource = Ds_WorkspaceGrid.Tables(0).DefaultView.ToTable(True, "nWorkSpaceScreeningHdrNo,vWorkspaceId,vProjectNo,vActivityName".Split(","))
                    Me.GVProjectSpcScreening.DataBind()
                    Me.PanelProjectSpecific.Style.Add("display", "")
                    Me.pnlgvmedexworkspadce.Style.Add("display", "")
                    Me.tblmedexworkspadce.Style.Add("display", "none")
                    Me.tblGeneralScr.Style.Add("display", "none")
                    Me.tblProjectSpcScreening.Style.Add("display", "")
                    Me.Collpase1.ClientState = True
                    Me.Collpase1.Collapsed = True
                    Me.lblprojecthdr.Text = "Project Details : " + Me.txtproject.Text.Trim()
                    If Me.Hdn_ProjectLock.Value = "L" Then
                        GVProjectSpcScreening.Columns(gvc_porjectscreening_detach).Visible = False
                        GVProjectSpcScreening.Columns(gvc_porjectscreening_order).Visible = False
                    Else
                        GVProjectSpcScreening.Columns(gvc_porjectscreening_detach).Visible = True
                        GVProjectSpcScreening.Columns(gvc_porjectscreening_order).Visible = True

                    End If

                    Return True
                End If
                Me.GVProjectSpcScreening.DataSource = Nothing
                Me.GVProjectSpcScreening.EmptyDataText = "No Template Is Attached With This Project"
                Me.GVProjectSpcScreening.DataBind()
                Me.PanelProjectSpecific.Style.Add("display", "")
                Me.pnlgvmedexworkspadce.Style.Add("display", "")
                Me.tblmedexworkspadce.Style.Add("display", "none")
                Me.tblGeneralScr.Style.Add("display", "none")
                Me.tblProjectSpcScreening.Style.Add("display", "")
                Me.Collpase1.ClientState = True
                Me.Collpase1.Collapsed = True
                Me.lblprojecthdr.Text = "Project Details : " + Me.txtproject.Text.Trim()
                Return True
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, ".. Error in BindGrid()")
            Return False
        End Try

    End Function
#End Region

#Region "Assign Functions"

#Region "Assign Values"

    Private Sub AssignValues(ByVal type As String)
        Dim dt_User As New DataTable
        Dim ds_Save As New DataSet
        Dim estr_Retu As String = String.Empty
        Dim ds As New DataSet
        Dim dt_save As New DataSet
        Dim ds_Workspace As New DataSet
        Dim ds_Template As New DataSet
        Dim ds_MedExWorkspaceHdr As New DataSet
        Dim ds_MedExWorkspaceHdrEdit As New DataSet
        Dim cnt As Integer = 0
        Dim wStr As String = String.Empty
        Dim dt_MedExWorkspace As DataTable
        Dim drinner1 As DataRow
        Dim dt As DataTable
        Dim dt_Template As DataTable
        Dim drinner As DataRow
        Dim dt_MedExWorkspaceHdr As DataTable
        Dim drinner2 As DataRow
        Try
            If type.ToUpper = "ADD" Then

                If Not objhelp.GetMedExWorkSpaceHdr("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_MedExWorkspaceHdr, estr_Retu) Then 'get Blank Table From MedExWorkspaceHdr
                    Exit Sub
                    'Me.ViewState(Vs_DtBlankMedExWorkspaceHdr) = ds_MedExWorkspaceHdr.Tables(0)
                End If

                dt_MedExWorkspace = ds_MedExWorkspaceHdr.Tables(0)
                drinner1 = dt_MedExWorkspace.NewRow()

                drinner1("nMedExWorkSpaceHdrNo") = "0"
                strActivityId = Me.ddlActivity.SelectedItem.Value.Trim
                drinner1("vActivityId") = strActivityId.Substring(0, strActivityId.LastIndexOf("#"))
                drinner1("iNodeId") = strActivityId.Substring(strActivityId.LastIndexOf("#") + 1)
                drinner1("vWorkspaceId") = Me.HProjectId.Value
                drinner1("iModifyBy") = Session(S_UserID)
                dt_MedExWorkspace.Rows.Add(drinner1)
                Me.ViewState(Vs_MedExWorkspaceHdr) = dt_MedExWorkspace

                wStr = "vMedExTemplateId ='" + Me.ddlTemplate.SelectedItem.Value + "' And cActiveFlag<> 'N' order by iSeqNo,nMedExTemplateDtlNo"

                If Not objhelp.GetMedExTemplateDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Template, estr_Retu) Then
                    Response.Write(estr_Retu)
                    Exit Sub
                End If

                If Not objhelp.GetMedExWorkSpaceDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_Workspace, estr_Retu) Then
                    'Me.ViewState(VS_DtBlankMedExWorkspaceMst) = ds_Workspace.Tables(0)
                    Exit Sub
                End If

                dt = ds_Workspace.Tables(0)
                dt_Template = ds_Template.Tables(0)

                For cnt = 0 To dt_Template.Rows.Count - 1

                    drinner = dt.NewRow()
                    drinner("iSeqNo") = dt_Template.Rows(cnt)("iSeqNo")
                    drinner("vMedExCode") = dt_Template.Rows(cnt)("vMedExCode")
                    drinner("vMedExDesc") = dt_Template.Rows(cnt)("vMedExDesc")
                    drinner("vMedExDefaultValue") = dt_Template.Rows(cnt)("vDefaultValue")
                    drinner("nMedExWorkSpaceDtlNo") = dt_Template.Rows(cnt)("vMedExCode")
                    drinner("nMedExWorkSpaceHdrNo") = dt_Template.Rows(cnt)("vMedExCode")
                    drinner("vHighRange") = dt_Template.Rows(cnt)("vHighRange")
                    drinner("vLowRange") = dt_Template.Rows(cnt)("vLowRange")
                    drinner("vWarningOnValue") = dt_Template.Rows(cnt)("vAlertonvalue")
                    drinner("vWarningMsg") = dt_Template.Rows(cnt)("vAlertMessage")
                    drinner("cActiveFlag") = GeneralModule.ActiveFlag_Yes
                    drinner("iModifyBy") = Session(S_UserID)
                    drinner("vMedExGroupCode") = dt_Template.Rows(cnt)("vMedExGroupCode")
                    drinner("vmedexgroupDesc") = dt_Template.Rows(cnt)("vmedexgroupDesc")
                    drinner("vMedexGroupCDISCValue") = dt_Template.Rows(cnt)("vMedexGroupCDISCValue")
                    drinner("vmedexGroupOtherValue") = dt_Template.Rows(cnt)("vmedexGroupOtherValue")
                    drinner("vMedExSubGroupCode") = dt_Template.Rows(cnt)("vMedExSubGroupCode")
                    drinner("vmedexsubGroupDesc") = dt_Template.Rows(cnt)("vmedexsubGroupDesc")
                    drinner("vMedexSubGroupCDISCValue") = dt_Template.Rows(cnt)("vMedexSubGroupCDISCValue")
                    drinner("vmedexsubGroupOtherValue") = dt_Template.Rows(cnt)("vmedexsubGroupOtherValue")
                    drinner("vMedExType") = dt_Template.Rows(cnt)("vMedExType")
                    drinner("vMedExValues") = dt_Template.Rows(cnt)("vMedExValues")
                    drinner("vUOM") = dt_Template.Rows(cnt)("vUOM")
                    drinner("vValidationType") = dt_Template.Rows(cnt)("vValidationType")
                    drinner("cWarningType") = dt_Template.Rows(cnt)("cAlertType")
                    drinner("cRefType") = dt_Template.Rows(cnt)("cRefType")
                    drinner("vRefTable") = dt_Template.Rows(cnt)("vRefTable")
                    drinner("vRefColumn") = dt_Template.Rows(cnt)("vRefColumn")
                    drinner("vRefFilePath") = dt_Template.Rows(cnt)("vRefFilePath")
                    drinner("vCDISCValues") = dt_Template.Rows(cnt)("vCDISCValues")
                    drinner("vOtherValues") = dt_Template.Rows(cnt)("vOtherValues")
                    drinner("vMedexFormula") = dt_Template.Rows(cnt)("vMedexFormula")
                    dt.Rows.Add(drinner)
                    dt.AcceptChanges()

                Next cnt

                Me.ViewState(VS_DtMedExWorkspaceMst) = dt

            ElseIf type.ToUpper = "EDIT" Then

                If objhelp.GetMedExWorkSpaceHdr("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_MedExWorkspaceHdrEdit, estr_Retu) Then 'get Blank Table From MedExWorkspaceHdr
                    Exit Sub
                End If
                ' Me.ViewState(VS_Choice) = 1
                dt_MedExWorkspaceHdr = ds_MedExWorkspaceHdrEdit.Tables(0)
                drinner2 = dt_MedExWorkspaceHdr.NewRow()
                drinner2("nMedExWorkSpaceHdrNo") = "0"
                strActivityId = Me.ddlActivity.SelectedItem.Value.Trim
                drinner2("vActivityId") = strActivityId.Substring(0, strActivityId.LastIndexOf("#"))
                drinner2("iNodeId") = strActivityId.Substring(strActivityId.LastIndexOf("#") + 1)
                drinner2("vWorkspaceId") = Me.HProjectId.Value
                drinner2("iModifyBy") = Session(S_UserID)
                dt_MedExWorkspaceHdr.Rows.Add(drinner2)
                Me.ViewState(Vs_MedExWorkspaceHdr) = dt_MedExWorkspaceHdr

                wStr = "vMedExTemplateId ='" + Me.ddlTemplate.SelectedItem.Value + "'"
                If Not objhelp.GetMedExTemplateDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Template, estr_Retu) Then
                    Response.Write(estr_Retu)
                    Exit Sub
                End If

                If objhelp.GetMedExWorkSpaceDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_Workspace, estr_Retu) Then
                    Exit Sub
                    ' Me.ViewState(VS_DtBlankMedExWorkspaceMst) = ds_Workspace.Tables(0)
                End If

                dt = ds_Workspace.Tables(0)
                dt_Template = ds_Template.Tables(0)

                For cnt = 0 To dt_Template.Rows.Count - 1

                    drinner = dt.NewRow()
                    drinner("iSeqNo") = dt_Template.Rows(cnt)("iSeqNo")
                    drinner("vMedExCode") = dt_Template.Rows(cnt)("vMedExCode")
                    drinner("vMedExDesc") = dt_Template.Rows(cnt)("vMedExDesc")
                    drinner("vMedExDefaultValue") = dt_Template.Rows(cnt)("vDefaultValue")
                    drinner("nMedExWorkSpaceDtlNo") = dt_Template.Rows(cnt)("vMedExCode")
                    drinner("nMedExWorkSpaceHdrNo") = dt_Template.Rows(cnt)("vMedExCode")
                    drinner("vHighRange") = dt_Template.Rows(cnt)("vHighRange")
                    drinner("vLowRange") = dt_Template.Rows(cnt)("vLowRange")
                    drinner("vWarningOnValue") = dt_Template.Rows(cnt)("vAlertonvalue")
                    drinner("vWarningMsg") = dt_Template.Rows(cnt)("vAlertMessage")
                    drinner("cActiveFlag") = GeneralModule.ActiveFlag_No
                    drinner("iModifyBy") = Session(S_UserID)
                    drinner("vMedExGroupCode") = dt_Template.Rows(cnt)("vMedExGroupCode")
                    drinner("vmedexgroupDesc") = dt_Template.Rows(cnt)("vmedexgroupDesc")
                    drinner("vMedexGroupCDISCValue") = dt_Template.Rows(cnt)("vMedexGroupCDISCValue")
                    drinner("vmedexGroupOtherValue") = dt_Template.Rows(cnt)("vmedexGroupOtherValue")
                    drinner("vMedExSubGroupCode") = dt_Template.Rows(cnt)("vMedExSubGroupCode")
                    drinner("vmedexsubGroupDesc") = dt_Template.Rows(cnt)("vmedexsubGroupDesc")
                    drinner("vMedexSubGroupCDISCValue") = dt_Template.Rows(cnt)("vMedexSubGroupCDISCValue")
                    drinner("vmedexsubGroupOtherValue") = dt_Template.Rows(cnt)("vmedexsubGroupOtherValue")
                    drinner("vMedExType") = dt_Template.Rows(cnt)("vMedExType")
                    drinner("vMedExValues") = dt_Template.Rows(cnt)("vMedExValues")
                    drinner("vUOM") = dt_Template.Rows(cnt)("vUOM")
                    drinner("vValidationType") = dt_Template.Rows(cnt)("vValidationType")
                    drinner("cAlertType") = dt_Template.Rows(cnt)("cAlertType")
                    drinner("cRefType") = dt_Template.Rows(cnt)("vAlertMessage")
                    drinner("vRefTable") = dt_Template.Rows(cnt)("vRefTable")
                    drinner("vRefColumn") = dt_Template.Rows(cnt)("vRefColumn")
                    drinner("vRefFilePath") = dt_Template.Rows(cnt)("vRefFilePath")
                    drinner("vCDISCValues") = dt_Template.Rows(cnt)("vCDISCValues")
                    drinner("vOtherValues") = dt_Template.Rows(cnt)("vOtherValues")
                    dt.Rows.Add(drinner)
                    dt.AcceptChanges()

                Next cnt

                Me.ViewState(VS_DtMedExWorkspaceMst) = dt

            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....AssignValues")
        End Try
    End Sub

    Private Function AssignValuesForProjectSpecificScreening(ByVal type As String) As Boolean

        Dim estr_Retu As String = String.Empty
        Dim ds_WorkspaceScreeningDtl As New DataSet
        Dim ds_Template As New DataSet
        Dim ds_WorkspaceScreeningHdr As New DataSet
        Dim cnt As Integer = 0
        Dim wStr As String = String.Empty
        Dim drWorkspaceScreeningHdr As DataRow
        Dim drWorkspaceScreeningDtl As DataRow
        Dim ds_WorkspaceScreeningHdrDtl As New DataSet
        Try



            If type.ToUpper = "ADD" Then

                If Not objhelp.GetWorkspaceScreeningHdr("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_WorkspaceScreeningHdr, estr_Retu) Then 'get Blank Table From MedExWorkspaceHdr
                    objcommon.ShowAlert("Error while getting Screening TemplateHDR.... AssignValuesForProjectSpecificScreening", Me.Page)
                End If


                drWorkspaceScreeningHdr = ds_WorkspaceScreeningHdr.Tables(0).NewRow()

                drWorkspaceScreeningHdr("nWorkSpaceScreeningHdrNo") = "0"
                drWorkspaceScreeningHdr("vWorkspaceId") = Me.HProjectId.Value
                drWorkspaceScreeningHdr("iModifyBy") = Session(S_UserID)
                ds_WorkspaceScreeningHdr.Tables(0).Rows.Add(drWorkspaceScreeningHdr)
                wStr = "vMedExTemplateId ='" + Me.ddlTemplate.SelectedItem.Value + "' And cActiveFlag<> 'N' order by iSeqNo,nMedExTemplateDtlNo"

                If Not objhelp.GetMedExTemplateDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Template, estr_Retu) Then
                    objcommon.ShowAlert("Error while getting Attribute Template.... AssignValuesForProjectSpecificScreening", Me.Page)
                    Exit Function
                End If

                If Not objhelp.GetWorkspaceScreeningDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_WorkspaceScreeningDtl, estr_Retu) Then
                    objcommon.ShowAlert("Error while getting Screening TemplateDtL.... AssignValuesForProjectSpecificScreening", Me.Page)
                End If
                For cnt = 0 To ds_Template.Tables(0).Rows.Count - 1

                    drWorkspaceScreeningDtl = ds_WorkspaceScreeningDtl.Tables(0).NewRow()
                    drWorkspaceScreeningDtl("nWorkSpaceScreeningDtlNo") = 0
                    drWorkspaceScreeningDtl("iSeqNo") = ds_Template.Tables(0).Rows(cnt)("iSeqNo")
                    drWorkspaceScreeningDtl("vMedExCode") = ds_Template.Tables(0).Rows(cnt)("vMedExCode")
                    drWorkspaceScreeningDtl("vMedExDesc") = ds_Template.Tables(0).Rows(cnt)("vMedExDesc")
                    drWorkspaceScreeningDtl("vMedExDefaultValue") = ds_Template.Tables(0).Rows(cnt)("vDefaultValue")
                    drWorkspaceScreeningDtl("vHighRange") = ds_Template.Tables(0).Rows(cnt)("vHighRange")
                    drWorkspaceScreeningDtl("vLowRange") = ds_Template.Tables(0).Rows(cnt)("vLowRange")
                    drWorkspaceScreeningDtl("vWarningOnValue") = ds_Template.Tables(0).Rows(cnt)("vAlertonvalue")
                    drWorkspaceScreeningDtl("vWarningMsg") = ds_Template.Tables(0).Rows(cnt)("vAlertMessage")
                    drWorkspaceScreeningDtl("cActiveFlag") = GeneralModule.ActiveFlag_Yes
                    drWorkspaceScreeningDtl("iModifyBy") = Session(S_UserID)
                    drWorkspaceScreeningDtl("vMedExGroupCode") = ds_Template.Tables(0).Rows(cnt)("vMedExGroupCode")
                    drWorkspaceScreeningDtl("vmedexgroupDesc") = ds_Template.Tables(0).Rows(cnt)("vmedexgroupDesc")
                    drWorkspaceScreeningDtl("vMedexGroupCDISCValue") = ds_Template.Tables(0).Rows(cnt)("vMedexGroupCDISCValue")
                    drWorkspaceScreeningDtl("vmedexGroupOtherValue") = ds_Template.Tables(0).Rows(cnt)("vmedexGroupOtherValue")
                    drWorkspaceScreeningDtl("vMedExSubGroupCode") = ds_Template.Tables(0).Rows(cnt)("vMedExSubGroupCode")
                    drWorkspaceScreeningDtl("vmedexsubGroupDesc") = ds_Template.Tables(0).Rows(cnt)("vmedexsubGroupDesc")
                    drWorkspaceScreeningDtl("vMedexSubGroupCDISCValue") = ds_Template.Tables(0).Rows(cnt)("vMedexSubGroupCDISCValue")
                    drWorkspaceScreeningDtl("vmedexsubGroupOtherValue") = ds_Template.Tables(0).Rows(cnt)("vmedexsubGroupOtherValue")
                    drWorkspaceScreeningDtl("vMedExType") = ds_Template.Tables(0).Rows(cnt)("vMedExType")
                    drWorkspaceScreeningDtl("vMedExValues") = ds_Template.Tables(0).Rows(cnt)("vMedExValues")
                    drWorkspaceScreeningDtl("vUOM") = ds_Template.Tables(0).Rows(cnt)("vUOM")
                    drWorkspaceScreeningDtl("vValidationType") = ds_Template.Tables(0).Rows(cnt)("vValidationType")
                    drWorkspaceScreeningDtl("cWarningType") = ds_Template.Tables(0).Rows(cnt)("cAlertType")
                    drWorkspaceScreeningDtl("cRefType") = ds_Template.Tables(0).Rows(cnt)("cRefType")
                    drWorkspaceScreeningDtl("vRefTable") = ds_Template.Tables(0).Rows(cnt)("vRefTable")
                    drWorkspaceScreeningDtl("vRefColumn") = ds_Template.Tables(0).Rows(cnt)("vRefColumn")
                    drWorkspaceScreeningDtl("vRefFilePath") = ds_Template.Tables(0).Rows(cnt)("vRefFilePath")
                    drWorkspaceScreeningDtl("vCDISCValues") = ds_Template.Tables(0).Rows(cnt)("vCDISCValues")
                    drWorkspaceScreeningDtl("vOtherValues") = ds_Template.Tables(0).Rows(cnt)("vOtherValues")
                    drWorkspaceScreeningDtl("vMedexFormula") = ds_Template.Tables(0).Rows(cnt)("vMedexFormula")
                    ds_WorkspaceScreeningDtl.Tables(0).Rows.Add(drWorkspaceScreeningDtl)
                    'ds_WorkspaceScreeningDtl .Tables(0).Rows.Add()
                    ds_WorkspaceScreeningDtl.Tables(0).AcceptChanges()
                Next cnt

                ds_WorkspaceScreeningHdrDtl.Tables.Add(ds_WorkspaceScreeningHdr.Tables(0).Copy())
                ds_WorkspaceScreeningHdrDtl.Tables(0).TableName = "WorkspaceScreeningHdr"
                ds_WorkspaceScreeningHdrDtl.Tables.Add(ds_WorkspaceScreeningDtl.Tables(0).Copy())
                ds_WorkspaceScreeningHdrDtl.Tables(1).TableName = "WorkspaceScreeningDtl"
                If Not Me.objLambda.Insert_WorkSpaceScreeningHdrDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_WorkspaceScreeningHdrDtl, estr_Retu) Then
                    Throw New Exception("Error while saving Project specific screening structure")
                End If
                Me.objcommon.ShowAlert("Project specific screening structure Attached successfully", Me.Page)
                If Not BindGrid() Then
                    Throw New Exception("Error in BindGrid()...")
                End If

            End If

            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....AssignValuesForProjectspecific")
            Return False
        End Try
    End Function

    Private Function AssignValuesForGenericScreening(ByVal type As String) As Boolean

        Dim estr_Retu As String = String.Empty
        Dim ds_ScreeningTemplateDtl As New DataSet
        Dim ds_Template As New DataSet
        Dim ds_ScreeningTemplateHdr As New DataSet
        Dim cnt As Integer = 0
        Dim wStr As String = String.Empty
        Dim drScreeningTemplateHdr As DataRow
        Dim drScreeningTemplateDtl As DataRow
        Dim ds_ScreeningTemplateHdrDtl As New DataSet
        Try
            If type.ToUpper = "ADD" Then

                If Not objhelp.GetScreeningTemplateHdr("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_ScreeningTemplateHdr, estr_Retu) Then 'get Blank Table From MedExWorkspaceHdr
                    objcommon.ShowAlert("Error while getting Screening TemplateHDR.... AssignValuesForProjectSpecificScreening", Me.Page)
                End If

                drScreeningTemplateHdr = ds_ScreeningTemplateHdr.Tables(0).NewRow()
                drScreeningTemplateHdr("nScreeningTemplateHdrNo") = "0"
                drScreeningTemplateHdr("iModifyBy") = Session(S_UserID)
                drScreeningTemplateHdr("vLocationcode") = Session(S_LocationCode)
                ds_ScreeningTemplateHdr.Tables(0).Rows.Add(drScreeningTemplateHdr)
                wStr = "vMedExTemplateId ='" + Me.ddlTemplate.SelectedItem.Value + "' And cActiveFlag<> 'N' order by iSeqNo,nMedExTemplateDtlNo"

                If Not objhelp.GetMedExTemplateDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Template, estr_Retu) Then
                    objcommon.ShowAlert("Error while getting Attribute Template.... AssignValuesForProjectSpecificScreening", Me.Page)
                    Exit Function
                End If

                If Not objhelp.GetScreeningTemplateDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_ScreeningTemplateDtl, estr_Retu) Then
                    objcommon.ShowAlert("Error while getting Screening TemplateDtL.... AssignValuesForProjectSpecificScreening", Me.Page)
                End If
                For cnt = 0 To ds_Template.Tables(0).Rows.Count - 1

                    drScreeningTemplateDtl = ds_ScreeningTemplateDtl.Tables(0).NewRow()
                    drScreeningTemplateDtl("nScreeningTemplateDtlNo") = 0
                    drScreeningTemplateDtl("iSeqNo") = ds_Template.Tables(0).Rows(cnt)("iSeqNo")
                    drScreeningTemplateDtl("vMedExCode") = ds_Template.Tables(0).Rows(cnt)("vMedExCode")
                    drScreeningTemplateDtl("vMedExDesc") = ds_Template.Tables(0).Rows(cnt)("vMedExDesc")
                    drScreeningTemplateDtl("vMedExDefaultValue") = ds_Template.Tables(0).Rows(cnt)("vDefaultValue")
                    drScreeningTemplateDtl("vHighRange") = ds_Template.Tables(0).Rows(cnt)("vHighRange")
                    drScreeningTemplateDtl("vLowRange") = ds_Template.Tables(0).Rows(cnt)("vLowRange")
                    drScreeningTemplateDtl("vWarningOnValue") = ds_Template.Tables(0).Rows(cnt)("vAlertonvalue")
                    drScreeningTemplateDtl("vWarningMsg") = ds_Template.Tables(0).Rows(cnt)("vAlertMessage")
                    drScreeningTemplateDtl("cActiveFlag") = GeneralModule.ActiveFlag_Yes
                    drScreeningTemplateDtl("iModifyBy") = Session(S_UserID)
                    drScreeningTemplateDtl("vMedExGroupCode") = ds_Template.Tables(0).Rows(cnt)("vMedExGroupCode")
                    drScreeningTemplateDtl("vmedexgroupDesc") = ds_Template.Tables(0).Rows(cnt)("vmedexgroupDesc")
                    drScreeningTemplateDtl("vMedexGroupCDISCValue") = ds_Template.Tables(0).Rows(cnt)("vMedexGroupCDISCValue")
                    drScreeningTemplateDtl("vmedexGroupOtherValue") = ds_Template.Tables(0).Rows(cnt)("vmedexGroupOtherValue")
                    drScreeningTemplateDtl("vMedExSubGroupCode") = ds_Template.Tables(0).Rows(cnt)("vMedExSubGroupCode")
                    drScreeningTemplateDtl("vmedexsubGroupDesc") = ds_Template.Tables(0).Rows(cnt)("vmedexsubGroupDesc")
                    drScreeningTemplateDtl("vMedexSubGroupCDISCValue") = ds_Template.Tables(0).Rows(cnt)("vMedexSubGroupCDISCValue")
                    drScreeningTemplateDtl("vmedexsubGroupOtherValue") = ds_Template.Tables(0).Rows(cnt)("vmedexsubGroupOtherValue")
                    drScreeningTemplateDtl("vMedExType") = ds_Template.Tables(0).Rows(cnt)("vMedExType")
                    drScreeningTemplateDtl("vMedExValues") = ds_Template.Tables(0).Rows(cnt)("vMedExValues")
                    drScreeningTemplateDtl("vUOM") = ds_Template.Tables(0).Rows(cnt)("vUOM")
                    drScreeningTemplateDtl("vValidationType") = ds_Template.Tables(0).Rows(cnt)("vValidationType")
                    drScreeningTemplateDtl("cWarningType") = ds_Template.Tables(0).Rows(cnt)("cAlertType")
                    drScreeningTemplateDtl("cRefType") = ds_Template.Tables(0).Rows(cnt)("cRefType")
                    drScreeningTemplateDtl("vRefTable") = ds_Template.Tables(0).Rows(cnt)("vRefTable")
                    drScreeningTemplateDtl("vRefColumn") = ds_Template.Tables(0).Rows(cnt)("vRefColumn")
                    drScreeningTemplateDtl("vRefFilePath") = ds_Template.Tables(0).Rows(cnt)("vRefFilePath")
                    drScreeningTemplateDtl("vCDISCValues") = ds_Template.Tables(0).Rows(cnt)("vCDISCValues")
                    drScreeningTemplateDtl("vOtherValues") = ds_Template.Tables(0).Rows(cnt)("vOtherValues")
                    drScreeningTemplateDtl("vMedexFormula") = ds_Template.Tables(0).Rows(cnt)("vMedexFormula")
                    ds_ScreeningTemplateDtl.Tables(0).Rows.Add(drScreeningTemplateDtl)
                    'ds_ScreeningTemplateDtl .Tables(0).Rows.Add()
                    ds_ScreeningTemplateDtl.Tables(0).AcceptChanges()
                Next cnt

                ds_ScreeningTemplateHdrDtl.Tables.Add(ds_ScreeningTemplateHdr.Tables(0).Copy())
                ds_ScreeningTemplateHdrDtl.Tables(0).TableName = "ScreeningTemplateHdr"
                ds_ScreeningTemplateHdrDtl.Tables.Add(ds_ScreeningTemplateDtl.Tables(0).Copy())
                ds_ScreeningTemplateHdrDtl.Tables(1).TableName = "ScreeningTemplateDtl"
                If Not Me.objLambda.Insert_ScreeningTemplateHdrDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_ScreeningTemplateHdrDtl, estr_Retu) Then
                    Throw New Exception("Error while saving Project specific screening structure")
                End If
                Me.objcommon.ShowAlert("Generic screening structure Attached successfully", Me.Page)
                If Not BindGrid() Then
                    Throw New Exception("Error in BindGrid()...")
                End If

            End If

            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....AssignValuesForProjectspecific")
            Return False
        End Try
    End Function

#End Region

#Region "Assign Values MedEx"

    Private Sub AssignValuesMedEx(ByVal mode As String)
        Dim dr As DataRow = Nothing
        Dim drMedEx As DataRow = Nothing
        Dim drMedExWorkSpace As DataRow = Nothing
        Dim dt_MedEx As New DataTable
        Dim dt_DtMedExDtl As New DataTable
        Dim ds_save As New DataSet
        Dim estr_Retu As String = String.Empty
        Dim wstr As String = String.Empty
        Dim dvAdd As New DataView
        Dim dvDelete As New DataView
        Dim dvEdit As New DataView
        Dim dsTemp As New DataSet
        Dim dt_ViewMedExWorkSpaceDtl As New DataTable
        Dim dt_MedExWorkSpaceDtl As New DataTable
        Dim Index As Integer
        Dim dsDelete As New DataSet
        Dim dtTemp As DataTable
        Dim MaxSeqNo As Integer = 0
        Dim ds_SaveForFormula As New DataSet
        'Dim sender As Object
        'Dim e As EventArgs
        Dim WsubSTR As String = String.Empty
        Dim ds_CrfSubDtl As DataSet = Nothing
        Dim Ds_crfterm As DataSet = Nothing
        Dim ds_WorkspaceDtlEdit As New DataSet
        Try
            If mode = AddToGrid Then

                dt_MedEx = CType(Me.ViewState(VS_DtMedExMst), DataTable)
                dt_DtMedExDtl = CType(Me.ViewState(VS_DtMedExWorkSpaceDtl), DataTable)

                If Me.Request.QueryString("mode") = 1 Then
                    For Each dr In dt_MedEx.Rows
                    Next dr
                    MaxSeqNo = 0
                    If dt_DtMedExDtl.Rows.Count > 0 Then
                        MaxSeqNo = dt_DtMedExDtl.Compute("Max(iSeqNo)", "1=1")
                    End If
                    '--------Checking for duplicate MedEx Entry
                    For Each dr In dt_DtMedExDtl.Rows
                    Next dr
                    '----------------Change On 01-July-2009------------------------

                    dr = dt_DtMedExDtl.NewRow()
                    dr("nMedExWorkSpaceDtlNo") = 0 - dt_DtMedExDtl.Rows.Count - 1
                    dr("nMedExWorkSpaceHdrNo") = Me.ViewState(Vs_MedExWorkspaceHdrNo).ToString()
                    dr("iSeqNo") = MaxSeqNo + 1
                    dr("vMedExCode") = drMedEx("vMedExCode")
                    dr("vMedExDesc") = drMedEx("vMedExDesc")
                    dr("vMedExType") = drMedEx("vMedExType")
                    dr("vMedExValues") = drMedEx("vMedExValues")
                    dr("vDefaultValue") = drMedEx("vDefaultValue")
                    dr("vLowRange") = drMedEx("vLowRange")
                    dr("vHighRange") = drMedEx("vHighRange")
                    dr("vAlertonvalue") = drMedEx("vAlertonvalue")
                    dr("vAlertMessage") = drMedEx("vAlertMessage")
                    dr("cActiveFlag") = drMedEx("cActiveFlag")
                    dr("iModifyBy") = Me.Session(S_UserID).ToString()
                    dr("cStatusIndi") = "N"
                    dr("vMedExGroupCode") = drMedEx("vMedExGroupCode")
                    dr("vmedexgroupDesc") = drMedEx("vmedexgroupDesc")
                    dr("vMedexGroupCDISCValue") = drMedEx("vMedexGroupCDISCValue")
                    dr("vmedexGroupOtherValue") = drMedEx("vmedexGroupOtherValue")
                    dr("vMedExSubGroupCode") = drMedEx("vMedExSubGroupCode")
                    dr("vmedexsubGroupDesc") = drMedEx("vmedexsubGroupDesc")
                    dr("vMedexSubGroupCDISCValue") = drMedEx("vMedexSubGroupCDISCValue")
                    dr("vmedexsubGroupOtherValue") = drMedEx("vmedexsubGroupOtherValue")
                    dr("vMedExType") = drMedEx("vMedExType")
                    dr("vMedExValues") = drMedEx("vMedExValues")
                    dr("vUOM") = drMedEx("vUOM")
                    dr("vValidationType") = drMedEx("vValidationType")
                    dr("cAlertType") = drMedEx("cAlertType")
                    dr("cRefType") = drMedEx("cRefType")
                    dr("vRefTable") = drMedEx("vRefTable")
                    dr("vRefColumn") = drMedEx("vRefColumn")
                    dr("vRefFilePath") = drMedEx("vRefFilePath")
                    dr("vCDISCValues") = drMedEx("vCDISCValues")
                    dr("vOtherValues") = drMedEx("vOtherValues")
                    dr("vMedexFormula") = drMedEx("vMedexFormula")
                    dr("iDecimalNo") = drMedEx("iDecimalNo")
                    dt_DtMedExDtl.Rows.Add(dr)
                    dt_DtMedExDtl.AcceptChanges()

                End If
                Me.ViewState(VS_DtMedExWorkSpaceDtl) = dt_DtMedExDtl

            ElseIf mode = AddToDatabase Then

                If Not objhelp.GetMedExWorkSpaceDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                               ds_WorkspaceDtlEdit, estr_Retu) Then 'get Blank Table From MedExWorkspaceHdr

                    Throw New Exception(estr_Retu)
                End If

                dt_MedExWorkSpaceDtl = ds_WorkspaceDtlEdit.Tables(0)
                dt_MedExWorkSpaceDtl.Rows.Clear()

                dsTemp.Tables.Add(CType(dt_MedExWorkSpaceDtl, DataTable).Copy())
                dvAdd = dsTemp.Tables(0).DefaultView

                '============Added By Mani======================================
                ds_SaveForFormula.Tables.Add(CType(dt_MedExWorkSpaceDtl, DataTable).Copy())
                ds_SaveForFormula.Tables(0).TableName = "MedExWorkSpaceDtl"


                If Not MedexUsedForFormula(ds_SaveForFormula) Then
                    Exit Sub
                End If
                '======================================================
                '-----For Deleted Row---------------------------------
                ds_save = Nothing
                dt_MedExWorkSpaceDtl.Rows.Clear()

                dsDelete.Tables.Add(CType(Me.ViewState(VS_DtMedExWorkSpaceDtl), DataTable).Copy())
                dvDelete = dsDelete.Tables(0).DefaultView
                dvDelete.RowFilter = "cStatusIndi = 'D'"
                dtTemp = dvDelete.ToTable()

                For Each drMedEx In dtTemp.Rows
                    dr = dt_MedExWorkSpaceDtl.NewRow
                    dr("nMedExWorkSpaceDtlNo") = drMedEx("nMedExWorkSpaceDtlNo")
                    dr("nMedExWorkSpaceHdrNo") = drMedEx("nMedExWorkSpaceHdrNo")
                    dr("iSeqNo") = drMedEx("iSeqNo")
                    dr("vMedExCode") = drMedEx("vMedExCode")
                    dr("vMedExDesc") = drMedEx("vMedExDesc")
                    dr("vMedExDefaultValue") = drMedEx("vDefaultValue")
                    dr("vLowRange") = drMedEx("vLowRange")
                    dr("vHighRange") = drMedEx("vHighRange")
                    dr("vWarningOnValue") = drMedEx("vAlertonvalue")
                    dr("vWarningMsg") = drMedEx("vAlertMessage")
                    dr("cActiveFlag") = drMedEx("cActiveFlag")
                    dr("iModifyBy") = Me.Session(S_UserID).ToString()
                    dr("cStatusIndi") = "D"
                    dt_MedExWorkSpaceDtl.Rows.Add(dr)
                    dt_MedExWorkSpaceDtl.AcceptChanges()
                Next drMedEx

                ds_save = New DataSet
                ds_save.Tables.Add(dt_MedExWorkSpaceDtl.Copy())
                ds_save.Tables(0).TableName = "MedExWorkSpaceDtl"


                If ds_save.Tables(0).Rows.Count > 0 Then

                    If Not objLambda.Save_MedExWorkspaceDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                            WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_MedExWorkspaceDtl, ds_save, Me.Session(S_UserID), estr_Retu) Then
                        objcommon.ShowAlert("Error While Saving Attribute Details In Attribute WorkspaceDtl" + estr_Retu, Me.Page)
                        Exit Sub
                    End If

                End If

                '-------For Add New Row----------
                dvAdd.RowFilter = "nMedExWorkSpaceDtlNo < 0"

                ds_save = New DataSet
                ds_save.Tables.Add(dvAdd.ToTable().Copy())
                ds_save.Tables(0).TableName = "MedExWorkSpaceDtl"

                If ds_save.Tables(0).Rows.Count > 0 Then

                    If Not objLambda.Save_MedExWorkspaceDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                            WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_MedExWorkspaceDtl, ds_save, Me.Session(S_UserID), estr_Retu) Then
                        objcommon.ShowAlert("Error While Saving Attribute Details In AttributeWorkspaceDtl" + estr_Retu, Me.Page)
                        Exit Sub
                    End If

                End If

                '-------For EDIT Row----------
                ds_save = Nothing

                dvEdit = dsTemp.Tables(0).DefaultView
                dvEdit.RowFilter = "nMedExWorkSpaceDtlNo > 0"

                ds_save = New DataSet
                ds_save.Tables.Add(dvEdit.ToTable().Copy())
                ds_save.Tables(0).TableName = "MedExWorkSpaceDtl"


                '===========for Deleting Previously Coded Meddra when Dictionary Changes==================
                If Me.hdndictionaryindic.Value <> "" Then
                    Dim Param As String = String.Empty
                    Dim dt_final As DataTable = Nothing
                    Dim Strcrfsubdtl As String = String.Empty
                    Dim dt_Worktbl As DataTable = Nothing
                    Param = Me.HProjectId.Value.ToString()
                    If Not objhelp.Proc_CRFTermCode(Param, Ds_crfterm, eStr) Then
                        Exit Sub
                    End If

                    If Ds_crfterm.Tables(0).Rows.Count > 0 Then
                        Ds_crfterm.Tables(0).DefaultView.RowFilter = "vMedExType = 'ComboGlobalDictionary' AND vRefTableRemark = '" + Me.ViewState(Vs_Dictionary).ToString + "'"
                        dt_Worktbl = Ds_crfterm.Tables(0).DefaultView.ToTable
                        'dt_Worktbl.DefaultView.RowFilter = " vWorkspaceId = '" + Me.HProjectId.Value.ToString() + "'"
                        If dt_Worktbl.Rows.Count > 0 Then
                            For count As Integer = 0 To dt_Worktbl.Rows.Count - 1
                                Strcrfsubdtl += dt_Worktbl.Rows(count).Item("nCRFSubDtlNo").ToString() + ","
                            Next

                            If Strcrfsubdtl <> "" Then
                                Strcrfsubdtl = Strcrfsubdtl.Substring(0, Strcrfsubdtl.LastIndexOf(","))
                            End If
                            WsubSTR = "SELECT * FROM  CRFSubDtl WHERE nCRFSubDtlNo IN (" + Strcrfsubdtl + ") "

                            ds_CrfSubDtl = objhelp.GetResultSet(WsubSTR, "CRFSubDtl")

                            If ds_CrfSubDtl.Tables(0).Rows.Count > 0 Then
                                For count As Integer = 0 To ds_CrfSubDtl.Tables(0).Rows.Count - 1
                                    ds_CrfSubDtl.Tables(0).Rows(count)("vMedExResult") = ""
                                    ds_CrfSubDtl.Tables(0).Rows(count)("vModificationRemark") = "Changed Dictionary"
                                    ds_CrfSubDtl.Tables(0).Rows(count)("iModifyBy") = Me.Session(S_UserID)
                                    ds_CrfSubDtl.Tables(0).Rows(count)("dModifyOn") = DateTime.Now()  ''Added by Rahul Rupareliya For Audit Trail Changes
                                Next
                                ds_CrfSubDtl.Tables(0).AcceptChanges()
                                If Not Me.objLambda.Save_CRFSubDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                                                  ds_CrfSubDtl, Me.Session(S_UserID), eStr) Then
                                    objcommon.ShowAlert("Error While Deleting Previous Coded Meddra Terms", Me.Page)
                                    Exit Sub
                                End If
                            End If
                        End If
                    End If
                End If
                '=================================Added By Debashis================================================

                If ds_save.Tables(0).Rows.Count > 0 Then

                    If Not objLambda.Save_MedExWorkspaceDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                            WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_MedExWorkspaceDtl, ds_save, Me.Session(S_UserID), estr_Retu) Then
                        objcommon.ShowAlert("Error While Saving Attribute Details In AttributeWorkspaceDtl" + estr_Retu, Me.Page)
                        Exit Sub
                    End If

                End If

                objcommon.ShowAlert("Attribute Details Saved SuccessFully", Me.Page)
                ' btnClose_Click(sender, e)
                Me.ViewState(VS_DtMedExWorkSpaceDtl) = Nothing
                Me.ViewState(VS_DtMedExMst) = Nothing

            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...........AssignValuesMedEx")
        End Try
    End Sub

    Private Function AssignValuesMedExForProjectSpScr(ByVal mode As String) As Boolean
        Dim dr As DataRow = Nothing
        Dim drMedEx As DataRow = Nothing
        Dim drMedExWorkSpace As DataRow = Nothing
        Dim dt_MedEx As New DataTable
        Dim dt_DtMedExDtl As New DataTable
        Dim ds_save As New DataSet
        Dim estr_Retu As String = String.Empty
        Dim wstr As String = String.Empty
        Dim dvAdd As New DataView
        Dim dvDelete As New DataView
        Dim dvEdit As New DataView
        Dim dsTemp As New DataSet
        Dim ds_WorkspaceDtl As New DataSet
        Dim ds_WorkspaceHdr As New DataSet
        Dim dt_ViewMedExWorkSpaceDtl As New DataTable
        Dim dt_MedExWorkSpaceDtl As New DataTable
        Dim ds_WorkspaceDtlEdit As New DataSet
        Dim Index As Integer
        Dim dsDelete As New DataSet
        Dim dtTemp As DataTable
        Dim MaxSeqNo As Integer = 0
        Dim ds_SaveForFormula As New DataSet

        Try

            If Not objhelp.GetWorkspaceScreeningHdr("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_WorkspaceHdr, estr_Retu) Then
                Throw New Exception(estr_Retu)
            End If

            If Not objhelp.GetWorkspaceScreeningDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                               ds_WorkspaceDtl, estr_Retu) Then 'get Blank Table From MedExWorkspaceHdr

                Throw New Exception(estr_Retu)
            End If

            If mode = AddToGrid Then

                dt_MedEx = CType(Me.ViewState(VS_DtMedExMst), DataTable)
                dt_DtMedExDtl = CType(Me.ViewState(VS_DtMedExWorkSpaceDtl), DataTable)

                If Me.Request.QueryString("mode") = 1 Then
                    MaxSeqNo = 0
                    If dt_DtMedExDtl.Rows.Count > 0 Then
                        MaxSeqNo = dt_DtMedExDtl.Compute("Max(iSeqNo)", "1=1")
                    End If
                    '--------Checking for duplicate MedEx Entry
                    For Each dr In dt_DtMedExDtl.Rows

                    Next dr
                    '----------------Change On 01-July-2009------------------------

                    dr = dt_DtMedExDtl.NewRow()
                    dr("nWorkspaceScreeningDtlNo") = 0 - dt_DtMedExDtl.Rows.Count - 1
                    dr("nWorkSpaceScreeningHdrNo") = Me.ViewState(Vs_MedExWorkspaceHdrNo).ToString()
                    dr("iSeqNo") = MaxSeqNo + 1
                    dr("vMedExCode") = drMedEx("vMedExCode")
                    dr("vMedExDesc") = drMedEx("vMedExDesc")
                    dr("vMedExType") = drMedEx("vMedExType")
                    dr("vMedExValues") = drMedEx("vMedExValues")
                    dr("vDefaultValue") = drMedEx("vDefaultValue")
                    dr("vLowRange") = drMedEx("vLowRange")
                    dr("vHighRange") = drMedEx("vHighRange")
                    dr("vAlertonvalue") = drMedEx("vAlertonvalue")
                    dr("vAlertMessage") = drMedEx("vAlertMessage")
                    dr("cActiveFlag") = drMedEx("cActiveFlag")
                    dr("iModifyBy") = Me.Session(S_UserID).ToString()
                    dr("cStatusIndi") = "N"
                    dr("vMedExGroupCode") = drMedEx("vMedExGroupCode")
                    dr("vmedexgroupDesc") = drMedEx("vmedexgroupDesc")
                    dr("vMedexGroupCDISCValue") = drMedEx("vMedexGroupCDISCValue")
                    dr("vmedexGroupOtherValue") = drMedEx("vmedexGroupOtherValue")
                    dr("vMedExSubGroupCode") = drMedEx("vMedExSubGroupCode")
                    dr("vmedexsubGroupDesc") = drMedEx("vmedexsubGroupDesc")
                    dr("vMedexSubGroupCDISCValue") = drMedEx("vMedexSubGroupCDISCValue")
                    dr("vmedexsubGroupOtherValue") = drMedEx("vmedexsubGroupOtherValue")
                    dr("vMedExType") = drMedEx("vMedExType")
                    dr("vMedExValues") = drMedEx("vMedExValues")
                    dr("vUOM") = drMedEx("vUOM")
                    dr("vValidationType") = drMedEx("vValidationType")
                    dr("cAlertType") = drMedEx("cAlertType")
                    dr("cRefType") = drMedEx("cRefType")
                    dr("vRefTable") = drMedEx("vRefTable")
                    dr("vRefColumn") = drMedEx("vRefColumn")
                    dr("vRefFilePath") = drMedEx("vRefFilePath")
                    dr("vCDISCValues") = drMedEx("vCDISCValues")
                    dr("vOtherValues") = drMedEx("vOtherValues")
                    dr("vMedexFormula") = drMedEx("vMedexFormula")
                    dr("iDecimalNo") = drMedEx("iDecimalNo")
                    dt_DtMedExDtl.Rows.Add(dr)
                    dt_DtMedExDtl.AcceptChanges()

                End If
                Me.ViewState(VS_DtMedExWorkSpaceDtl) = dt_DtMedExDtl

            ElseIf mode = AddToDatabase Then

                If Not objhelp.GetWorkspaceScreeningDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                               ds_WorkspaceDtlEdit, estr_Retu) Then

                    Throw New Exception(estr_Retu)
                End If
                dt_MedExWorkSpaceDtl = ds_WorkspaceDtlEdit.Tables(0)
                dt_MedExWorkSpaceDtl.Rows.Clear()

                dsTemp.Tables.Add(CType(dt_MedExWorkSpaceDtl, DataTable).Copy())
                dvAdd = dsTemp.Tables(0).DefaultView

                '============Added By Mani======================================

                ds_SaveForFormula.Tables.Add(CType(dt_MedExWorkSpaceDtl, DataTable).Copy())
                ds_SaveForFormula.Tables(0).TableName = "MedExWorkSpaceDtl"


                If Not MedexUsedForFormula(ds_SaveForFormula) Then
                    Exit Function
                End If
                '======================================================

                '-----For Deleted Row---------------------------------
                ds_save = Nothing
                dt_MedExWorkSpaceDtl.Rows.Clear()

                dsDelete.Tables.Add(CType(Me.ViewState(VS_DtMedExWorkSpaceDtl), DataTable).Copy())
                dvDelete = dsDelete.Tables(0).DefaultView
                dvDelete.RowFilter = "cStatusIndi = 'D'"
                dtTemp = dvDelete.ToTable()
                For Each drMedEx In dtTemp.Rows
                    dr = dt_MedExWorkSpaceDtl.NewRow()
                    dr("nWorkspaceScreeningDtlNo") = drMedEx("nWorkspaceScreeningDtlNo")
                    dr("nWorkSpaceScreeningHdrNo") = drMedEx("nWorkSpaceScreeningHdrNo")
                    dr("iSeqNo") = drMedEx("iSeqNo")
                    dr("vMedExCode") = drMedEx("vMedExCode")
                    dr("vMedExDesc") = drMedEx("vMedExDesc")
                    dr("vMedExDefaultValue") = drMedEx("vDefaultValue")
                    dr("vLowRange") = drMedEx("vLowRange")
                    dr("vHighRange") = drMedEx("vHighRange")
                    dr("vWarningOnValue") = drMedEx("vAlertonvalue")
                    dr("vWarningMsg") = drMedEx("vAlertMessage")
                    dr("cActiveFlag") = drMedEx("cActiveFlag")
                    dr("iModifyBy") = Me.Session(S_UserID).ToString()
                    dt_MedExWorkSpaceDtl.Rows.Add(dr)
                    dt_MedExWorkSpaceDtl.AcceptChanges()
                Next drMedEx
                ds_save = New DataSet
                If dtTemp.Rows.Count = CType(Me.ViewState(VS_DtMedExWorkSpaceDtl), DataTable).Rows.Count Then
                    If Me.objhelp.GetWorkspaceScreeningHdr(" nWorkSpaceScreeningHdrNo=" + dt_MedExWorkSpaceDtl.Rows(0).Item("nWorkSpaceScreeningHdrNo").ToString(), WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_WorkspaceHdr, eStr) Then
                        ds_WorkspaceHdr.Tables(0).Rows(0).Item("iModifyBy") = Me.Session(S_UserID).ToString()
                        ds_WorkspaceHdr.AcceptChanges()
                    Else
                        Throw New Exception(eStr.ToString())
                    End If

                    ds_save.Tables.Add(ds_WorkspaceHdr.Tables(0).Copy())
                    ds_save.Tables(0).TableName = "WorkspaceScreeningHdr"

                End If
                If ds_save.Tables.Contains("WorkspaceScreeningHdr") Then
                    ds_save.Tables.Add(dt_MedExWorkSpaceDtl.Copy)
                    ds_save.Tables(1).TableName = "WorkspaceScreeningDtl"
                Else
                    ds_save.Tables.Add(dt_MedExWorkSpaceDtl.Copy)
                    ds_save.Tables(0).TableName = "WorkspaceScreeningDtl"
                End If
                If ds_save.Tables(0).Rows.Count > 0 Then

                    If Not objLambda.Insert_WorkSpaceScreeningHdrDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete, ds_save, estr_Retu) Then
                        Throw New Exception("Error While Saving Attribute Details In Project Specific Screening" + estr_Retu)
                    End If

                End If

                ''-------For Add New Row----------
                dvAdd.RowFilter = "nWorkspaceScreeningDtlNo < 0"
                ds_save = New DataSet
                ds_save.Tables.Add(dvAdd.ToTable().Copy())
                ds_save.AcceptChanges()
                ds_save.Tables(0).TableName = "WorkspaceScreeningDtl"

                If ds_save.Tables(0).Rows.Count > 0 Then

                    If Not objLambda.Insert_WorkSpaceScreeningHdrDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_save, estr_Retu) Then
                        Throw New Exception("Error While Saving Attribute Details In Project Specific Screening" + estr_Retu)
                    End If

                End If

                '-------For EDIT Row----------
                ds_save = Nothing
                dvEdit = dsTemp.Tables(0).DefaultView
                dvEdit.RowFilter = "nWorkspaceScreeningDtlNo > 0"
                ds_save = New DataSet
                ds_save.Tables.Add(dvEdit.ToTable().Copy())
                ds_save.Tables(0).TableName = "WorkspaceScreeningDtl"
                If ds_save.Tables(0).Rows.Count > 0 Then
                    If Not objLambda.Insert_WorkSpaceScreeningHdrDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_save, estr_Retu) Then
                        Throw New Exception("Error While Saving Attribute Details In Project Specific Screening" + estr_Retu)
                    End If
                End If
                objcommon.ShowAlert("Attribute Details Saved SuccessFully", Me.Page)
                'btnCancel_Click(sender, e)
                If Not Me.Resetpage() Then
                    Me.ShowErrorMessage("Error in Reseting Page", "")
                End If
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...........AssignValuesMedExForProjectSpScr")
            Return False
        End Try
    End Function

    Private Function AssignValuesMedExForGenricScr(ByVal mode As String) As Boolean
        Dim dr As DataRow = Nothing
        Dim drMedEx As DataRow = Nothing
        Dim drMedExWorkSpace As DataRow = Nothing
        Dim dt_MedEx As New DataTable
        Dim dt_DtMedExDtl As New DataTable
        Dim ds_save As New DataSet
        Dim estr_Retu As String = String.Empty
        Dim wstr As String = String.Empty
        Dim ds_WorkspaceDtlEdit As New DataSet
        Dim dvAdd As New DataView
        Dim dvDelete As New DataView
        Dim dvEdit As New DataView
        Dim dsTemp As New DataSet
        Dim ds_WorkspaceDtl As New DataSet
        Dim ds_WorkspaceHdr As New DataSet

        Dim dt_ViewMedExWorkSpaceDtl As New DataTable
        Dim dt_MedExWorkSpaceDtl As New DataTable
        Dim Index As Integer
        Dim dsDelete As New DataSet
        Dim dtTemp As DataTable
        Dim MaxSeqNo As Integer = 0
        Dim ds_SaveForFormula As New DataSet
        Try


            If Not objhelp.GetScreeningTemplateHdr("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_WorkspaceHdr, estr_Retu) Then
                Throw New Exception(estr_Retu)
            End If

            If Not objhelp.GetScreeningTemplateDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                               ds_WorkspaceDtl, estr_Retu) Then 'get Blank Table From MedExWorkspaceHdr

                Throw New Exception(estr_Retu)
            End If

            If mode = AddToGrid Then

                dt_MedEx = CType(Me.ViewState(VS_DtMedExMst), DataTable)
                dt_DtMedExDtl = CType(Me.ViewState(VS_DtMedExWorkSpaceDtl), DataTable)

                If Me.Request.QueryString("mode") = 1 Then

                    MaxSeqNo = 0
                    If dt_DtMedExDtl.Rows.Count > 0 Then
                        MaxSeqNo = dt_DtMedExDtl.Compute("Max(iSeqNo)", "1=1")
                    End If
                    '--------Checking for duplicate MedEx Entry
                    '----------------Change On 01-July-2009------------------------

                    dr = dt_DtMedExDtl.NewRow()
                    dr("nScreeningTemplateDtlNo") = 0 - dt_DtMedExDtl.Rows.Count - 1
                    dr("nScreeningTemplateHdrNo") = Me.ViewState(Vs_MedExWorkspaceHdrNo).ToString()
                    dr("iSeqNo") = MaxSeqNo + 1
                    dr("vMedExCode") = drMedEx("vMedExCode")
                    dr("vMedExDesc") = drMedEx("vMedExDesc")
                    dr("vMedExType") = drMedEx("vMedExType")
                    dr("vMedExValues") = drMedEx("vMedExValues")
                    dr("vDefaultValue") = drMedEx("vDefaultValue")
                    dr("vLowRange") = drMedEx("vLowRange")
                    dr("vHighRange") = drMedEx("vHighRange")
                    dr("vAlertonvalue") = drMedEx("vAlertonvalue")
                    dr("vAlertMessage") = drMedEx("vAlertMessage")
                    dr("cActiveFlag") = drMedEx("cActiveFlag")
                    dr("iModifyBy") = Me.Session(S_UserID).ToString()
                    dr("cStatusIndi") = "N"
                    dr("vMedExGroupCode") = drMedEx("vMedExGroupCode")
                    dr("vmedexgroupDesc") = drMedEx("vmedexgroupDesc")
                    dr("vMedexGroupCDISCValue") = drMedEx("vMedexGroupCDISCValue")
                    dr("vmedexGroupOtherValue") = drMedEx("vmedexGroupOtherValue")
                    dr("vMedExSubGroupCode") = drMedEx("vMedExSubGroupCode")
                    dr("vmedexsubGroupDesc") = drMedEx("vmedexsubGroupDesc")
                    dr("vMedexSubGroupCDISCValue") = drMedEx("vMedexSubGroupCDISCValue")
                    dr("vmedexsubGroupOtherValue") = drMedEx("vmedexsubGroupOtherValue")
                    dr("vMedExType") = drMedEx("vMedExType")
                    dr("vMedExValues") = drMedEx("vMedExValues")
                    dr("vUOM") = drMedEx("vUOM")
                    dr("vValidationType") = drMedEx("vValidationType")
                    dr("cAlertType") = drMedEx("cAlertType")
                    dr("cRefType") = drMedEx("cRefType")
                    dr("vRefTable") = drMedEx("vRefTable")
                    dr("vRefColumn") = drMedEx("vRefColumn")
                    dr("vRefFilePath") = drMedEx("vRefFilePath")
                    dr("vCDISCValues") = drMedEx("vCDISCValues")
                    dr("vOtherValues") = drMedEx("vOtherValues")
                    dr("vMedexFormula") = drMedEx("vMedexFormula")
                    dr("iDecimalNo") = drMedEx("iDecimalNo")
                    dt_DtMedExDtl.Rows.Add(dr)
                    dt_DtMedExDtl.AcceptChanges()

                End If
                Me.ViewState(VS_DtMedExWorkSpaceDtl) = dt_DtMedExDtl

            ElseIf mode = AddToDatabase Then

                If Not objhelp.GetScreeningTemplateDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                                ds_WorkspaceDtlEdit, estr_Retu) Then

                    Throw New Exception(estr_Retu)
                End If

                dt_MedExWorkSpaceDtl = ds_WorkspaceDtlEdit.Tables(0)
                dt_MedExWorkSpaceDtl.Rows.Clear()

                dsTemp.Tables.Add(CType(dt_MedExWorkSpaceDtl, DataTable).Copy())
                dvAdd = dsTemp.Tables(0).DefaultView
                '============Added By Mani======================================

                ds_SaveForFormula.Tables.Add(CType(dt_MedExWorkSpaceDtl, DataTable).Copy())
                ds_SaveForFormula.Tables(0).TableName = "MedExWorkSpaceDtl"

                If Not MedexUsedForFormula(ds_SaveForFormula) Then
                    Exit Function
                End If

                '-----For Deleted Row---------------------------------
                ds_save = Nothing
                dt_MedExWorkSpaceDtl.Rows.Clear()

                dsDelete.Tables.Add(CType(Me.ViewState(VS_DtMedExWorkSpaceDtl), DataTable).Copy())
                dvDelete = dsDelete.Tables(0).DefaultView
                dvDelete.RowFilter = "cStatusIndi = 'D'"
                dtTemp = dvDelete.ToTable()

                For Each drMedEx In dtTemp.Rows
                    dr = dt_MedExWorkSpaceDtl.NewRow()
                    dr("nScreeningTemplateDtlNo") = drMedEx("nScreeningTemplateDtlNo")
                    dr("nScreeningTemplateHdrNo") = drMedEx("nScreeningTemplateHdrNo")
                    dr("iSeqNo") = drMedEx("iSeqNo")
                    dr("vMedExCode") = drMedEx("vMedExCode")
                    dr("vMedExDesc") = drMedEx("vMedExDesc")
                    dr("vMedExDefaultValue") = drMedEx("vDefaultValue")
                    dr("vLowRange") = drMedEx("vLowRange")
                    dr("vHighRange") = drMedEx("vHighRange")
                    dr("vWarningOnValue") = drMedEx("vAlertonvalue")
                    dr("vWarningMsg") = drMedEx("vAlertMessage")
                    dr("cActiveFlag") = drMedEx("cActiveFlag")
                    dr("iModifyBy") = Me.Session(S_UserID).ToString()
                    dt_MedExWorkSpaceDtl.Rows.Add(dr)
                    dt_MedExWorkSpaceDtl.AcceptChanges()
                Next drMedEx

                ds_save = New DataSet
                ds_save.Tables.Add(dt_MedExWorkSpaceDtl.Copy)
                ds_save.Tables(0).TableName = "ScreeningTemplateDtl"
                If ds_save.Tables(0).Rows.Count > 0 Then

                    If Not objLambda.Insert_ScreeningTemplateHdrDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete, ds_save, estr_Retu) Then
                        Throw New Exception("Error While Saving Attribute Details In Project Specific Screening" + estr_Retu)
                    End If

                End If

                ''-------For Add New Row----------
                dvAdd.RowFilter = "nScreeningTemplateDtlNo < 0"
                ds_save = New DataSet
                ds_save.Tables.Add(dvAdd.ToTable().Copy())
                ds_save.AcceptChanges()
                ds_save.Tables(0).TableName = "ScreeningTemplateDtl"

                If ds_save.Tables(0).Rows.Count > 0 Then

                    If Not objLambda.Insert_ScreeningTemplateHdrDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_save, estr_Retu) Then
                        Throw New Exception("Error While Saving Attribute Details In Project Specific Screening" + estr_Retu)
                    End If

                End If

                '-------For EDIT Row----------
                ds_save = Nothing
                dvEdit = dsTemp.Tables(0).DefaultView
                dvEdit.RowFilter = "nScreeningTemplateDtlNo > 0"
                ds_save = New DataSet
                ds_save.Tables.Add(dvEdit.ToTable().Copy())
                ds_save.Tables(0).TableName = "ScreeningTemplateDtl"
                If ds_save.Tables(0).Rows.Count > 0 Then
                    If Not objLambda.Insert_ScreeningTemplateHdrDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_save, estr_Retu) Then
                        Throw New Exception("Error While Saving Attribute Details In Project Specific Screening" + estr_Retu)
                    End If
                End If
                objcommon.ShowAlert("Attribute Details Saved SuccessFully", Me.Page)
                'btnCancel_Click(sender, e)
                If Not Me.Resetpage() Then
                    Me.ShowErrorMessage("Error in Reseting Page", "")
                End If
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...........AssignValuesMedExForProjectSpScr")
            Return False
        End Try
    End Function

#End Region

#End Region

#Region "Button Events"

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim ds As New DataSet
        Dim ds_save As New DataSet
        Dim dt_save As New DataSet
        Dim ds_MedExWorkspaceHdr As New DataSet
        Dim ds_MedExWorkspaceDtl As New DataSet
        Dim eStr_Retu As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_Check As New DataSet
        Dim strActivityNode As String
        Dim strActivity As String
        Dim ds_CrfVersionDetail As DataTable = Nothing
        Dim ds_Template As DataSet = Nothing
        Try

            ds_CrfVersionDetail = CType(ViewState(VS_CrfVersionDetail), DataTable)

            If Not ds_CrfVersionDetail Is Nothing AndAlso ds_CrfVersionDetail.Rows.Count > 0 Then
                If ds_CrfVersionDetail.Rows(0)("cFreezeStatus").ToString.Trim.ToUpper = "F" Then
                    objcommon.ShowAlert("This Project Is Freezed. Kindly UnFreeze Project TO change Project Structure", Me.Page)
                    Exit Sub
                End If
            End If


            If Me.Request.QueryString("mode") = 1 Then
                'If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                If RBLProjecttype.SelectedValue = 2 Then

                    If Not Me.objhelp.GetWorkspaceScreeningHdr(" vWorkspaceId='" & Me.HProjectId.Value.Trim() & "' And cStatusIndi<>'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                        Throw New Exception("Error while Checking values of Projectspecific Screening..")
                    Else
                        If ds.Tables(0).Rows.Count > 0 Then
                            Me.objcommon.ShowAlert("Template Is All Ready Attached With This Project", Me.Page)
                            Exit Sub
                        End If
                    End If
                    ds = Nothing
                    If Not AssignValuesForProjectSpecificScreening("Add") Then
                        Throw New Exception("Error while Assignning values of Projectspecific Screening..")
                    End If
                    Exit Sub
                End If

                If RBLProjecttype.SelectedValue = "0000000000" Then

                    If Not Me.objhelp.GetScreeningTemplateHdr(" vWorkspaceId='0000000000' And cStatusIndi<>'D' And vLocationcode like '%" & Session(S_LocationCode) & "%'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                        Throw New Exception("Error while Checking values of Projectspecific Screening..")
                    Else
                        If ds.Tables(0).Rows.Count > 0 Then
                            Me.objcommon.ShowAlert("Template Is All Ready Attached With Generic Screening", Me.Page)
                            Exit Sub
                        End If
                    End If
                    ds = Nothing
                    If Not AssignValuesForGenericScreening("Add") Then
                        Throw New Exception("Error while Assignning values of Projectspecific Screening..")
                    End If
                    Exit Sub
                End If

                If RBLProjecttype.SelectedValue = 1 Then
                    wStr = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "'"
                    strActivityId = Me.ddlActivity.SelectedItem.Value.Trim
                    strActivity = strActivityId.Substring(0, strActivityId.LastIndexOf("#"))
                    strActivityNode = strActivityId.Substring(strActivityId.LastIndexOf("#") + 1)
                    wStr += " And vActivityId = '" + strActivity + "' and cStatusIndi <> 'D' And iNodeId = " + strActivityNode
                    If Not objhelp.GetMedExWorkSpaceHdr(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                        ds_Check, eStr) Then
                        objcommon.ShowAlert("Error While Getting Existed Data From MedExWorkSpaceHdr", Me.Page)
                        Exit Sub
                    End If
                    If ds_Check.Tables(0).Rows.Count > 0 Then
                        objcommon.ShowAlert("Template Of attribute Is Already Attached", Me.Page)
                        Exit Sub
                    End If
                    ' Me.GenCall_Data(ds)
                    AssignValues("Add")
                    ds_MedExWorkspaceHdr = New DataSet
                    ds_MedExWorkspaceHdr.Tables.Add(CType(Me.ViewState(Vs_MedExWorkspaceHdr), Data.DataTable).Copy())
                    ds_MedExWorkspaceHdr.Tables(0).TableName = "MedExWorkspaceHdr"
                    If Not Me.ViewState(VS_DtMedExWorkspaceMst) Is Nothing Then
                        ds_MedExWorkspaceHdr.Tables.Add(CType(Me.ViewState(VS_DtMedExWorkspaceMst), Data.DataTable).Copy())
                        ds_MedExWorkspaceHdr.Tables(1).TableName = "MedExWorkspaceDtl"
                    End If
                    If Not objLambda.Save_MedExWorkspaceDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_MedExWorkspaceDtl, ds_MedExWorkspaceHdr, Me.Session(S_UserID), eStr_Retu) Then
                        objcommon.ShowAlert("Error While Saving Workspace Mst", Me.Page)
                        Exit Sub
                    End If
                    objcommon.ShowAlert("Template Attached To Activity Successfully", Me.Page)
                End If

            End If


            Me.ddlActivity.SelectedIndex = -1
            Me.ddlTemplate.SelectedIndex = -1

            If Not BindGrid() Then
                Throw New Exception("Error in BindGrid()...")
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....BtnSave_Click")
        End Try
    End Sub

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim eStr_Retu As String = String.Empty
        Dim Ds_FillActivity As New DataSet
        Dim wstr As String = String.Empty
        Dim ds_CrfVersionDetail As DataSet = Nothing
        Dim VersionNo As Integer = 0
        Dim VersionDate As Date
        Dim ds_Check As New DataSet
        Dim dv_Check As New DataView
        Try
            Me.Hdn_ProjectLock.Value = ""
            If Me.RBLProjecttype.SelectedValue = 1 Or Me.RBLProjecttype.SelectedValue = 2 Then
                wstr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D'"

                If Not Me.objhelp.GetCRFLockDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                    ds_Check, eStr) Then
                    Throw New Exception(eStr)
                End If
                If Not ds_Check Is Nothing Then

                    dv_Check = ds_Check.Tables(0).DefaultView
                    dv_Check.Sort = "iTranNo desc"
                    ' edited by vishal for lock/unlock site
                    If dv_Check.ToTable().Rows.Count > 0 Then

                        If Convert.ToString(dv_Check.ToTable().Rows(0)("cLockFlag")).Trim.ToUpper() = "L" Then
                            Me.objcommon.ShowAlert("Project is Locked.", Me.Page)
                            Me.Hdn_ProjectLock.Value = "L"
                        End If
                    End If
                End If
            End If

            If Me.RBLProjecttype.SelectedValue = 1 Then

                '====== CRFVersion Control==================================
                Hdn_FreezeStatus.Value = ""
                wstr = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "'"

                If Not objhelp.GetData("View_CRFVersionForDataEntryControl", "*", wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CrfVersionDetail, eStr_Retu) Then
                    Throw New Exception(eStr_Retu)
                End If

                If ds_CrfVersionDetail.Tables(0).Rows.Count > 0 Then
                    Me.ViewState(VS_CrfVersionDetail) = ds_CrfVersionDetail.Tables(0)
                    Hdn_FreezeStatus.Value = ds_CrfVersionDetail.Tables(0).Rows(0)("cFreezeStatus").ToString.ToUpper.Trim
                    VersionNo = ds_CrfVersionDetail.Tables(0).Rows(0)("nVersionNo")
                    VersionDate = ds_CrfVersionDetail.Tables(0).Rows(0)("dVersiondate").ToString
                    Me.VersionNo.Text = VersionNo.ToString
                    Me.VersionDate.Text = VersionDate.ToString("dd-MMM-yyyy")
                    Me.VersionDtl.Attributes.Add("style", "display:;")
                    If ds_CrfVersionDetail.Tables(0).Rows(0)("cFreezeStatus").ToString.Trim.ToUpper = "U" Then
                        ImageLockUnlock.Attributes.Add("src", "images/UnFreeze.jpg")
                    End If
                Else
                    Me.VersionDtl.Attributes.Add("style", "display:none;")
                End If
                '==========================================================
            End If
            If Me.RBLProjecttype.SelectedValue = "0000000000" Then
                If objhelp.getActivityMst("cStatusIndi <> 'D' order by vActivityName", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_FillActivity, eStr_Retu) Then
                    Me.ddlActivity.DataSource = Ds_FillActivity.Tables(0)
                    Me.ddlActivity.DataValueField = "vActivityId"
                    Me.ddlActivity.DataTextField = "vActivityName"
                    Me.ddlActivity.DataBind()
                End If
            Else
                If objhelp.GetViewWorkSpaceNodeDetail(" vWorkspaceId = '" & Me.HProjectId.Value.Trim() & _
                                                        "' And cStatusIndi <> 'D' And (vActivityId is Not NULL or vActivityId<>'') order by vActivityName", _
                                                        Ds_FillActivity, eStr_Retu) Then
                    Me.ddlActivity.DataSource = Ds_FillActivity.Tables(0).DefaultView.ToTable(True, "ActivityDisplayId,ActivityDisplayName".Split(","))
                    Me.ddlActivity.DataValueField = "ActivityDisplayId"
                    Me.ddlActivity.DataTextField = "ActivityDisplayName"
                    Me.ddlActivity.DataBind()
                End If
            End If
            Me.ddlActivity.Items.Insert(0, New ListItem("Select Activity", 0))
            Me.PanelProjectSpecific.Style.Add("display", "none")
            Me.pnlgvmedexworkspadce.Style.Add("display", "none")
            Me.PanelGridHeader.Style.Add("display", "none")
            'Me.pnlMedEx.Style.Add("display", "none")
            If Not BindGrid() Then
                Throw New Exception("Error in BindGrid()...")
            End If
            If Not FillScreeningGroup() Then
                Throw New Exception("Error in FillScreening()...")
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....btnSetProject_Click")
        End Try
    End Sub


    Protected Sub btnedit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Wstr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim ds_EditTemplate As New DataSet
        Dim ds_EditWorkspaceHdr As New DataSet
        Dim ds_EditTemplateWorkspace As New DataSet
        Dim dt_EditTemplate As New DataTable
        Dim dt_EditWorkspaceDtl As New DataTable
        Dim ds_DeleteWorkspaceDtl As New DataSet
        Dim ds_WorkSpaceDtl As New DataSet
        Dim ds As New DataSet
        Dim drinner As DataRow
        Dim gridViewRow As GridViewRow = CType(CType(sender, Button).Parent.Parent, GridViewRow)

        'Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit

        Wstr = "vMedExTemplateId='" & gridViewRow.Cells(GVC_MedExTemplateId).Text & "'"
        If objhelp.GetMedExTemplateDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_EditTemplate, eStr_Retu) Then
            Me.ViewState(VS_MedExEditTemplate) = ds_EditTemplate.Tables(0)
        End If

        Wstr = "nMedExWorkSpaceHdrNo='" & Me.ViewState(Vs_MedExWorkspaceHdrNo) & "'"
        If objhelp.GetMedExWorkSpaceDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_EditWorkspaceHdr, eStr_Retu) Then
            Me.ViewState(VS_MedExEditWorkspace) = ds_EditWorkspaceHdr.Tables(0)
        End If

        Wstr = "nMedExWorkSpaceHdrNo='" & Me.ViewState(Vs_MedExWorkspaceHdrNo) & "'"
        If Not objhelp.GetMedExWorkspaceDtlDelete(Me.ViewState(Vs_MedExWorkspaceHdrNo), ds_DeleteWorkspaceDtl, eStr_Retu) Then
            'this function is used to delete perticular MedExWorkspaceHdrNo
            objcommon.ShowAlert("Attribute WoekspaceHdrNo Is Not Available", Me.Page)
        End If


        dt_EditTemplate = Me.ViewState(VS_MedExEditTemplate)

        If Not objhelp.GetMedExWorkSpaceDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_WorkSpaceDtl, eStr_Retu) Then
            Throw New Exception(eStr_Retu)
        End If

        dt_EditWorkspaceDtl = ds_WorkSpaceDtl.Tables(0)
        'dt_EditWorkspaceDtl = Me.ViewState(VS_DtBlankMedExWorkspaceDtl)

        For Each dr As DataRow In dt_EditTemplate.Rows
            drinner = dt_EditWorkspaceDtl.NewRow
            drinner("nMedExWorkspaceDtlNo") = dr("vMedExCode")
            drinner("nMedExWorkspaceHdrNo") = Me.ViewState(Vs_MedExWorkspaceHdrNo)
            drinner("vMedExCode") = dr("vMedExCode")
            drinner("vMedExDesc") = dr("vMedExDesc")
            drinner("vMedExDefaultValue") = dr("vDefaultValue")
            drinner("vLowRange") = dr("vLowRange")
            drinner("vHighRange") = dr("vHighRange")
            drinner("cActiveFlag") = dr("cActiveFlag")
            drinner("imodifyBy") = Session(S_UserID)
            dt_EditWorkspaceDtl.Rows.Add(drinner)
            dt_EditWorkspaceDtl.AcceptChanges()
        Next dr

        Me.ViewState("EditTemplate") = dt_EditWorkspaceDtl
        ds_EditTemplateWorkspace = New DataSet
        ds_EditTemplateWorkspace.Tables.Add(CType(Me.ViewState("EditTemplate"), Data.DataTable).Copy())
        ds_EditTemplateWorkspace.Tables(0).TableName = "MedExWorkspaceHdr"

        If Not objLambda.Save_MedExWorkspaceDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_MedExWorkspaceDtl, ds_EditTemplateWorkspace, Me.Session(S_UserID), eStr_Retu) Then
            objcommon.ShowAlert("Error While Edit Template In MedExWorkspaceHdr", Me.Page)
            Exit Sub

        End If

        objcommon.ShowAlert("Attribute Template Edited SuccessFully", Me.Page)
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Response.Redirect("frmMedExWorkspaceDtl.aspx?mode=1")
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Response.Redirect("frmMainPage.aspx", False)
    End Sub

    Protected Sub btnAddMedEx_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds_gvwMedEx As New DataSet
        Dim dt_DtMedExDtl As New DataTable
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Try
            If Me.RBLProjecttype.SelectedValue = 1 Then
                AssignValuesMedEx(AddToGrid)
                dt_DtMedExDtl = CType(Me.ViewState(VS_DtMedExWorkSpaceDtl), DataTable)
                If Not FillEditedgrid(dt_DtMedExDtl) Then
                    Exit Sub
                End If
                Me.CollapsiblePanelExtender1.ClientState = True
                Me.CollapsiblePanelExtender1.Collapsed = True
                'Me.pnlMedEx.Style.Add("display", "")
                Me.PanelGridHeader.Style.Add("display", "")
            ElseIf Me.RBLProjecttype.SelectedValue = 2 Then
                If Not AssignValuesMedExForProjectSpScr(AddToGrid) Then
                    'Throw New Exception("Error in AssignValuesForProjectSpecificScreening")
                    Exit Sub
                End If
                dt_DtMedExDtl = CType(Me.ViewState(VS_DtMedExWorkSpaceDtl), DataTable)
                If Not FillEditedgrid(dt_DtMedExDtl) Then
                    Exit Sub
                End If
                Me.CollapsiblePanelExtender1.ClientState = True
                Me.CollapsiblePanelExtender1.Collapsed = True
                'Me.pnlMedEx.Style.Add("display", "")
                Me.PanelGridHeader.Style.Add("display", "")
            Else
                If Not AssignValuesMedExForGenricScr(AddToGrid) Then
                    Exit Sub
                End If
                dt_DtMedExDtl = CType(Me.ViewState(VS_DtMedExWorkSpaceDtl), DataTable)
                If Not FillEditedgrid(dt_DtMedExDtl) Then
                    Exit Sub
                End If
                Me.CollapsiblePanelExtender1.ClientState = True
                Me.CollapsiblePanelExtender1.Collapsed = True
                Me.PanelGridHeader.Style.Add("display", "")
            End If

            Me.ViewState(Vs_Status) = "Active"

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..btnAddMedEx_Click")
        End Try
    End Sub


    Protected Sub btnSaveSequence_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveSequence.Click
        Dim ds_Field As New DataSet
        Dim JSONString As String = String.Empty
        Dim DvMedExWorkSpaceDtl As DataView = Nothing
        Dim estr As String = String.Empty
        Dim dsConvert As New DataSet
        Dim dr As DataRow = Nothing
        Try

            If Me.hdnMedexList.Value <> "" Then
                JSONString = Me.hdnMedexList.Value
                ds_Field.Tables.Add(JsonConvert.DeserializeObject(JSONString, GetType(System.Data.DataTable)))

                If ds_Field.Tables(0).Rows.Count > 0 Then
                    If Me.RBLProjecttype.SelectedValue = 1 Then

                        If Not objhelp.GetMedExWorkSpaceDtl("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, dsConvert, estr) Then
                            Throw New Exception(estr)
                        End If

                        For Index = 0 To ds_Field.Tables(0).Rows.Count - 1
                            dr = dsConvert.Tables(0).NewRow()
                            dr("nMedExWorkSpaceDtlNo") = ds_Field.Tables(0).Rows(Index)("nMedExWorkSpaceDtlNo")
                            dr("nMedExWorkSpaceHdrNo") = ds_Field.Tables(0).Rows(Index)("nMedExWorkSpaceHdrNo")
                            dr("iSeqNo") = ds_Field.Tables(0).Rows(Index)("iSeqNo")
                            dr("vMedExCode") = ds_Field.Tables(0).Rows(Index)("vMedExCode")
                            dr("vMedExDesc") = ds_Field.Tables(0).Rows(Index)("vMedExDesc")
                            dr("vMedExDefaultValue") = ds_Field.Tables(0).Rows(Index)("vDefaultValue")
                            dr("vLowRange") = ds_Field.Tables(0).Rows(Index)("vLowRange")
                            dr("vHighRange") = ds_Field.Tables(0).Rows(Index)("vHighRange")
                            dr("vWarningOnValue") = ds_Field.Tables(0).Rows(Index)("vAlertonvalue")
                            dr("vWarningMsg") = ds_Field.Tables(0).Rows(Index)("vAlertMessage")
                            dr("cActiveFlag") = ds_Field.Tables(0).Rows(Index)("cActiveFlag")
                            dr("iModifyBy") = Me.Session(S_UserID).ToString()
                            dr("cStatusIndi") = ds_Field.Tables(0).Rows(Index)("cStatusIndi")
                            dr("vMedExGroupCode") = ds_Field.Tables(0).Rows(Index)("vMedExGroupCode")
                            dr("vmedexgroupDesc") = ds_Field.Tables(0).Rows(Index)("vmedexgroupDesc")
                            dr("vMedexGroupCDISCValue") = ds_Field.Tables(0).Rows(Index)("vMedexGroupCDISCValue")
                            dr("vmedexGroupOtherValue") = ds_Field.Tables(0).Rows(Index)("vmedexGroupOtherValue")
                            dr("vMedExSubGroupCode") = ds_Field.Tables(0).Rows(Index)("vMedExSubGroupCode")
                            dr("vmedexsubGroupDesc") = ds_Field.Tables(0).Rows(Index)("vmedexsubGroupDesc")
                            dr("vMedexSubGroupCDISCValue") = ds_Field.Tables(0).Rows(Index)("vMedexSubGroupCDISCValue")
                            dr("vmedexsubGroupOtherValue") = ds_Field.Tables(0).Rows(Index)("vmedexsubGroupOtherValue")
                            dr("vMedExType") = ds_Field.Tables(0).Rows(Index)("vMedExType")
                            dr("vMedExValues") = ds_Field.Tables(0).Rows(Index)("vMedExValues")
                            dr("vUOM") = ds_Field.Tables(0).Rows(Index)("vUOM")
                            dr("vValidationType") = ds_Field.Tables(0).Rows(Index)("vValidationType")
                            dr("cWarningType") = ds_Field.Tables(0).Rows(Index)("cAlertType")
                            dr("cRefType") = ds_Field.Tables(0).Rows(Index)("cRefType")
                            dr("vRefTable") = ds_Field.Tables(0).Rows(Index)("vRefTable")
                            dr("vRefColumn") = ds_Field.Tables(0).Rows(Index)("vRefColumn")
                            dr("vRefFilePath") = ds_Field.Tables(0).Rows(Index)("vRefFilePath")
                            dr("vCDISCValues") = ds_Field.Tables(0).Rows(Index)("vCDISCValues")
                            dr("vOtherValues") = ds_Field.Tables(0).Rows(Index)("vOtherValues")
                            dr("vMedexFormula") = ds_Field.Tables(0).Rows(Index)("vMedexFormula")

                            dsConvert.Tables(0).Rows.Add(dr)
                            dsConvert.Tables(0).AcceptChanges()
                        Next Index

                        dsConvert.Tables(0).TableName = "MedExWorkSpaceDtl"
                        If Not objLambda.Save_MedExWorkspaceDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                            WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_MedExWorkspaceDtl, dsConvert, Me.Session(S_UserID), estr) Then
                            objcommon.ShowAlert("Error While Saving Attribute Details In AttributeWorkspaceDtl" + estr, Me.Page)
                            Exit Sub
                        End If
                    ElseIf Me.RBLProjecttype.SelectedValue = 2 Then
                        'AssignValuesMedExForProjectSpScr(AddToDatabase)

                        If Not objhelp.GetWorkspaceScreeningDtl("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                              dsConvert, estr) Then
                            Throw New Exception(estr)
                            Exit Sub
                        End If

                        For Index = 0 To ds_Field.Tables(0).Rows.Count - 1
                            dr = dsConvert.Tables(0).NewRow()
                            dr("nWorkspaceScreeningDtlNo") = ds_Field.Tables(0).Rows(Index)("nWorkspaceScreeningDtlNo")
                            dr("nWorkSpaceScreeningHdrNo") = ds_Field.Tables(0).Rows(Index)("nWorkSpaceScreeningHdrNo")
                            dr("iSeqNo") = ds_Field.Tables(0).Rows(Index)("iSeqNo")
                            dr("vMedExCode") = ds_Field.Tables(0).Rows(Index)("vMedExCode")
                            dr("vMedExDesc") = ds_Field.Tables(0).Rows(Index)("vMedExDesc")
                            dr("vMedExDefaultValue") = ds_Field.Tables(0).Rows(Index)("vDefaultValue")
                            dr("vLowRange") = ds_Field.Tables(0).Rows(Index)("vLowRange")
                            dr("vHighRange") = ds_Field.Tables(0).Rows(Index)("vHighRange")
                            dr("vWarningOnValue") = ds_Field.Tables(0).Rows(Index)("vAlertonvalue")
                            dr("vWarningMsg") = ds_Field.Tables(0).Rows(Index)("vAlertMessage")
                            dr("cActiveFlag") = ds_Field.Tables(0).Rows(Index)("cActiveFlag")
                            dr("iModifyBy") = Me.Session(S_UserID).ToString()
                            dr("cStatusIndi") = ds_Field.Tables(0).Rows(Index)("cStatusIndi")
                            dr("vMedExGroupCode") = ds_Field.Tables(0).Rows(Index)("vMedExGroupCode")
                            dr("vmedexgroupDesc") = ds_Field.Tables(0).Rows(Index)("vmedexgroupDesc")
                            dr("vMedexGroupCDISCValue") = ds_Field.Tables(0).Rows(Index)("vMedexGroupCDISCValue")
                            dr("vmedexGroupOtherValue") = ds_Field.Tables(0).Rows(Index)("vmedexGroupOtherValue")
                            dr("vMedExSubGroupCode") = ds_Field.Tables(0).Rows(Index)("vMedExSubGroupCode")
                            dr("vmedexsubGroupDesc") = ds_Field.Tables(0).Rows(Index)("vmedexsubGroupDesc")
                            dr("vMedexSubGroupCDISCValue") = ds_Field.Tables(0).Rows(Index)("vMedexSubGroupCDISCValue")
                            dr("vmedexsubGroupOtherValue") = ds_Field.Tables(0).Rows(Index)("vmedexsubGroupOtherValue")
                            dr("vMedExType") = ds_Field.Tables(0).Rows(Index)("vMedExType")
                            dr("vMedExValues") = ds_Field.Tables(0).Rows(Index)("vMedExValues")
                            dr("vUOM") = ds_Field.Tables(0).Rows(Index)("vUOM")
                            dr("vValidationType") = ds_Field.Tables(0).Rows(Index)("vValidationType")
                            dr("cWarningType") = ds_Field.Tables(0).Rows(Index)("cAlertType")
                            dr("cRefType") = ds_Field.Tables(0).Rows(Index)("cRefType")
                            dr("vRefTable") = ds_Field.Tables(0).Rows(Index)("vRefTable")
                            dr("vRefColumn") = ds_Field.Tables(0).Rows(Index)("vRefColumn")
                            dr("vRefFilePath") = ds_Field.Tables(0).Rows(Index)("vRefFilePath")
                            dr("vCDISCValues") = ds_Field.Tables(0).Rows(Index)("vCDISCValues")
                            dr("vOtherValues") = ds_Field.Tables(0).Rows(Index)("vOtherValues")
                            dr("vMedexFormula") = ds_Field.Tables(0).Rows(Index)("vMedexFormula")

                            dsConvert.Tables(0).Rows.Add(dr)
                            dsConvert.Tables(0).AcceptChanges()
                        Next Index


                        dsConvert.Tables(0).TableName = "WorkspaceScreeningDtl"

                        If dsConvert.Tables(0).Rows.Count > 0 Then
                            If Not objLambda.Insert_WorkSpaceScreeningHdrDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, dsConvert, estr) Then
                                Throw New Exception("Error While Saving Attribute Details In Project Specific Screening" + estr)
                            End If
                        End If

                    Else
                        'AssignValuesMedExForGenricScr(AddToDatabase)
                        If Not objhelp.GetScreeningTemplateDtl("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                              dsConvert, estr) Then
                            Throw New Exception(estr)
                            Exit Sub
                        End If

                        For Index = 0 To ds_Field.Tables(0).Rows.Count - 1
                            dr = dsConvert.Tables(0).NewRow()
                            dr("nScreeningTemplateDtlNo") = ds_Field.Tables(0).Rows(Index)("nScreeningTemplateDtlNo")
                            dr("nScreeningTemplateHdrNo") = ds_Field.Tables(0).Rows(Index)("nScreeningTemplateHdrNo")
                            dr("iSeqNo") = ds_Field.Tables(0).Rows(Index)("iSeqNo")
                            dr("vMedExCode") = ds_Field.Tables(0).Rows(Index)("vMedExCode")
                            dr("vMedExDesc") = ds_Field.Tables(0).Rows(Index)("vMedExDesc")
                            dr("vMedExDefaultValue") = ds_Field.Tables(0).Rows(Index)("vDefaultValue")
                            dr("vLowRange") = ds_Field.Tables(0).Rows(Index)("vLowRange")
                            dr("vHighRange") = ds_Field.Tables(0).Rows(Index)("vHighRange")
                            dr("vWarningOnValue") = ds_Field.Tables(0).Rows(Index)("vAlertonvalue")
                            dr("vWarningMsg") = ds_Field.Tables(0).Rows(Index)("vAlertMessage")
                            dr("cActiveFlag") = ds_Field.Tables(0).Rows(Index)("cActiveFlag")
                            dr("iModifyBy") = Me.Session(S_UserID)
                            dr("cStatusIndi") = ds_Field.Tables(0).Rows(Index)("cStatusIndi")
                            dr("vMedExGroupCode") = ds_Field.Tables(0).Rows(Index)("vMedExGroupCode")
                            dr("vmedexgroupDesc") = ds_Field.Tables(0).Rows(Index)("vmedexgroupDesc")
                            dr("vMedexGroupCDISCValue") = ds_Field.Tables(0).Rows(Index)("vMedexGroupCDISCValue")
                            dr("vmedexGroupOtherValue") = ds_Field.Tables(0).Rows(Index)("vmedexGroupOtherValue")
                            dr("vMedExSubGroupCode") = ds_Field.Tables(0).Rows(Index)("vMedExSubGroupCode")
                            dr("vmedexsubGroupDesc") = ds_Field.Tables(0).Rows(Index)("vmedexsubGroupDesc")
                            dr("vMedexSubGroupCDISCValue") = ds_Field.Tables(0).Rows(Index)("vMedexSubGroupCDISCValue")
                            dr("vmedexsubGroupOtherValue") = ds_Field.Tables(0).Rows(Index)("vmedexsubGroupOtherValue")
                            dr("vMedExType") = ds_Field.Tables(0).Rows(Index)("vMedExType")
                            dr("vMedExValues") = ds_Field.Tables(0).Rows(Index)("vMedExValues")
                            dr("vUOM") = ds_Field.Tables(0).Rows(Index)("vUOM")
                            dr("vValidationType") = ds_Field.Tables(0).Rows(Index)("vValidationType")
                            dr("cWarningType") = ds_Field.Tables(0).Rows(Index)("cAlertType")
                            dr("cRefType") = ds_Field.Tables(0).Rows(Index)("cRefType")
                            dr("vRefTable") = ds_Field.Tables(0).Rows(Index)("vRefTable")
                            dr("vRefColumn") = ds_Field.Tables(0).Rows(Index)("vRefColumn")
                            dr("vRefFilePath") = ds_Field.Tables(0).Rows(Index)("vRefFilePath")
                            dr("vCDISCValues") = ds_Field.Tables(0).Rows(Index)("vCDISCValues")
                            dr("vOtherValues") = ds_Field.Tables(0).Rows(Index)("vOtherValues")
                            dr("vMedexFormula") = ds_Field.Tables(0).Rows(Index)("vMedexFormula")

                            dsConvert.Tables(0).Rows.Add(dr)
                            dsConvert.Tables(0).AcceptChanges()
                        Next Index
                        dsConvert.Tables(0).TableName = "ScreeningTemplateDtl"

                        If Not objLambda.Insert_ScreeningTemplateHdrDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, dsConvert, estr) Then
                            Throw New Exception("Error While Saving Attribute Details In Project Specific Screening" + estr)
                        End If
                    End If
                End If

                objcommon.ShowAlert("Attribute Sequence Saved SuccessFully", Me.Page)

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......btnSaveSequence_Click")
        End Try
    End Sub


    Protected Sub btnSaveFormula_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim DtMedExWorkSpaceDtl As DataTable = Nothing
        Dim dt_DtMedExDtl As New DataTable
        Dim dtMedexFormula As DataTable = Nothing
        Dim dv As DataView
        Dim nMedExWorkSpaceDtlNo As String = String.Empty
        Dim foundRow As DataRow()
        Try

            dt_DtMedExDtl = CType(Me.ViewState(VS_DtMedExWorkSpaceDtl), DataTable).Copy
            DtMedExWorkSpaceDtl = dt_DtMedExDtl.Copy
            nMedExWorkSpaceDtlNo = Me.HFFormulaNo.Value.ToString()
            dv = dt_DtMedExDtl.Copy.DefaultView
            If Not dt_DtMedExDtl Is Nothing Then
                If dt_DtMedExDtl.Rows.Count > 0 Then
                    If Me.RBLProjecttype.SelectedValue = 1 Then

                        dv.RowFilter = " vMedexType='Formula' AND nMedExWorkSpaceDtlNo = " + nMedExWorkSpaceDtlNo
                        foundRow = DtMedExWorkSpaceDtl.Select("vMedexType='Formula' AND nMedExWorkSpaceDtlNo = " + nMedExWorkSpaceDtlNo)
                        DtMedExWorkSpaceDtl.Rows.Remove(foundRow(0))
                    ElseIf Me.RBLProjecttype.SelectedValue = "0000000000" Then

                        dv.RowFilter = " vMedexType='Formula' AND nScreeningTemplateDtlNo = " + nMedExWorkSpaceDtlNo
                        foundRow = DtMedExWorkSpaceDtl.Select("vMedexType='Formula' AND nScreeningTemplateDtlNo = " + nMedExWorkSpaceDtlNo)
                        DtMedExWorkSpaceDtl.Rows.Remove(foundRow(0))
                    ElseIf Me.RBLProjecttype.SelectedValue.ToString() = "2" Then

                        dv.RowFilter = " vMedexType='Formula' AND nWorkspaceScreeningDtlNo = " + nMedExWorkSpaceDtlNo
                        foundRow = DtMedExWorkSpaceDtl.Select("vMedexType='Formula' AND nWorkspaceScreeningDtlNo = " + nMedExWorkSpaceDtlNo)
                        DtMedExWorkSpaceDtl.Rows.Remove(foundRow(0))

                    End If

                    dtMedexFormula = dv.ToTable()

                    For Each dr As DataRow In dtMedexFormula.Rows
                        dr("vMedexFormula") = Me.HFFormula.Value.ToString()
                        dr("iDecimalNo") = Me.txtDecimal.Text.ToString()
                    Next

                    dtMedexFormula.AcceptChanges()
                    DtMedExWorkSpaceDtl.AcceptChanges()

                    DtMedExWorkSpaceDtl.Merge(dtMedexFormula)
                    DtMedExWorkSpaceDtl.AcceptChanges()

                    Me.ViewState(VS_DtMedExWorkSpaceDtl) = DtMedExWorkSpaceDtl

                    If Not FillEditedgrid(DtMedExWorkSpaceDtl.Copy()) Then
                        Exit Sub
                    End If

                    objcommon.ShowAlert("Formula saved successfully.", Me.Page)
                    If HFdecimalText.Value = "" Then
                        Me.HFdecimalText.Value = Me.txtDecimal.Text.ToString.Trim()
                    End If
                End If
            End If
            Exit Try
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".......btndeleteMedex")
        End Try

    End Sub

    Protected Sub btnCopyScreening_Click(sender As Object, e As EventArgs) Handles btnCopyScreening.Click
        Try
            Dim dt_GeneralScreenig As DataTable = CType(ViewState("Gv_GeneralScr"), DataTable)
            For Each dr As DataRow In dt_GeneralScreenig.Rows
                txtVersion.Text = "Version " + Convert.ToString((Convert.ToInt64(Convert.ToString(dr("vScreeningTemplateVersionName")).Replace("Version", "")) + 1))
            Next
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".......btnCopyScreening")
        End Try
        txtCreatedDate.Text = DateTime.Parse(DateTime.Now).ToString("dd-MMM-yyyy")
        upModalRemarks.Update()
        ModalRemarks.Show()

    End Sub

    Protected Sub btnSaveCopyScreening_Click(sender As Object, e As EventArgs) Handles btnSaveCopyScreening.Click
        Dim wStr As String = "1 = 2"
        Dim ds_ScreeningCopy As DataSet
        Dim nScreeningTemplateHdrNo As Integer
        Try
            upModalRemarks.Update()
            ds_ScreeningCopy = objhelp.GetResultSet("select * from View_CopyScreeningTemplate where  cStatusIndi <> 'D' AND 1= 2 ", "View_CopyScreeningTemplate")

            Dim ds_GeneralScreening As DataTable = CType(ViewState("Gv_GeneralScr"), DataTable)

            For Each dr As DataRow In ds_GeneralScreening.Rows
                nScreeningTemplateHdrNo = dr("nScreeningTemplateHdrNo")
            Next

            If Not ds_ScreeningCopy Is Nothing Then
                Dim dr As DataRow = ds_ScreeningCopy.Tables(0).NewRow()
                dr("nScreeningTemplateHdrNo") = nScreeningTemplateHdrNo
                dr("VersionName") = Convert.ToString(txtVersion.Text)
                dr("cFreezeStatus") = "U"
                dr("dCreatedDate") = Convert.ToString(txtCreatedDate.Text)
                dr("vRemarks") = Convert.ToString(txtRemarks.Text)
                dr("cStatusIndi") = "N"
                dr("iModifyBy") = Session(S_UserID)
                ds_ScreeningCopy.Tables(0).Rows.Add(dr)
                ds_ScreeningCopy.AcceptChanges()

            End If

            If Not objLambda.Save_CopyScreeningTemplate(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_ScreeningCopy, Session(S_UserID), eStr) Then
                objcommon.ShowAlert("Error While Copy Screening Template", Me.Page)
                Exit Sub
            Else
            End If
            If eStr = "" Then
                objcommon.ShowAlert("Copy Screening Successfully", Me.Page)
            End If
            BindGrid()
            UpdatePanel1.Update()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".......btnSaveCopyScreening")
        End Try


    End Sub

    Protected Sub btnFreezeSave_Click(sender As Object, e As EventArgs) Handles btnFreezeSave.Click
        Dim wStr As String = "1 = 2"
        Dim ScreeningTemplateVersionMst As DataSet
        Dim nScreeningTemplateHdrNo As Integer
        Try
            upFreeModal.Update()
            ScreeningTemplateVersionMst = objhelp.GetResultSet("select * from ScreeningTemplateVersionMst where  cStatusIndi <> 'D' AND  nScreeningTemplateHdrNo = " + txtTemplateheaderno.Text, "View_CopyScreeningTemplate")


            If Not ScreeningTemplateVersionMst Is Nothing Then
                For Each dr As DataRow In ScreeningTemplateVersionMst.Tables(0).Rows
                    dr("cFreezeStatus") = "F"
                    dr("dCreatedDate") = txtEffectiveDate.Text
                    dr("vRemarks") = Convert.ToString(txtRemarksFreeze.Text)
                    dr("iModifyBy") = Session(S_UserID)

                    ScreeningTemplateVersionMst.AcceptChanges()
                Next
            End If

            If Not objLambda.Save_ScreeningTemplateVersionMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ScreeningTemplateVersionMst, Session(S_UserID), eStr) Then
                objcommon.ShowAlert("Error While Copy Screening Template", Me.Page)
                Exit Sub
            Else
            End If

            If eStr = "" Then
                objcommon.ShowAlert("Data Save Successuly", Me.Page)
            End If
            BindGrid()
            UpdatePanel1.Update()


        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".......btnSaveCopyScreening")
        End Try

    End Sub

#End Region

#Region "Radio Button List "

    Protected Sub RBLProjecttype_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RBLProjecttype.SelectedIndexChanged
        Dim eStr_Retu As String = String.Empty
        Dim Ds_FillActivity As New DataSet

        Try
            Me.btnCopyScreening.Visible = False
            Me.ViewState(VS_DtMedExWorkSpaceDtl) = Nothing
            Hdn_FreezeStatus.Value = ""
            Me.Hdn_ProjectLock.Value = ""
            Me.ViewState(VS_CrfVersionDetail) = Nothing
            If Not Resetpage() Then
                Throw New Exception("Error in Reseting Page")
            End If

            Me.PanelProjectSpecific.Style.Add("display", "none")
            Me.pnlgvmedexworkspadce.Style.Add("display", "none")
            Me.tblmedexworkspadce.Style.Add("display", "none")
            Me.tblGeneralScr.Style.Add("display", "none")
            Me.tblProjectSpcScreening.Style.Add("display", "none")
            'Me.pnlMedEx.Style.Add("display", "none")
            Me.PanelGridHeader.Style.Add("display", "none")

            If Me.RBLProjecttype.SelectedValue = "0000000000" Then
                Me.txtproject.Text = "Default Project"
                Me.txtproject.Enabled = False
                Me.btnCopyScreening.Visible = True
                Me.trScreeningGroup.Visible = True
                If Not BindGrid() Then
                    Throw New Exception("Error in BindGrid()...")
                End If
            ElseIf Me.RBLProjecttype.SelectedValue = 1 Then
                Me.ddlActivity.Attributes.Add("style", "")
                Me.tdActivity.Attributes.Add("Style", "text-align: right;")
                Me.trScreeningGroup.Visible = False
            ElseIf Me.RBLProjecttype.SelectedValue = 2 Then
                Me.trScreeningGroup.Visible = True
            End If

            If Not FillScreeningGroup() Then
                Me.ShowErrorMessage("Error in Fill Screening Group...", "")
            End If
        Catch ex As Exception
            Me.ShowErrorMessage("Error in Selected Index Change...", ex.ToString)
        End Try
    End Sub

#End Region

#Region "Grid Events"

#Region "gvmedexworkspace Event"

    Protected Sub gvmedexworkspadce_PageIndexChanging1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        If Not gvmedexworkspadce.PageIndex = 0 Then
            gvmedexworkspadce.PageIndex = e.NewPageIndex
            If Not BindGrid() Then
                Throw New Exception("Error in BindGrid()...")
            End If
        Else
            gvmedexworkspadce.PageIndex = e.NewPageIndex
            If Not BindGrid() Then
                Throw New Exception("Error in BindGrid()...")
            End If
        End If

    End Sub

    Protected Sub gvmedexworkspadce_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)

        Dim index As Integer = e.CommandArgument
        Dim Wstr As String = String.Empty
        Dim ds_Hdr As New DataSet
        Dim eStr_Retu As String = String.Empty
        Dim ds_DeleteTemplateWorkspace As New DataSet
        Dim ds_WorkspaceHdrDtl As New DataSet
        Dim dt_DtMedExDtl As DataTable = Nothing
        Dim dt_DeleteTemplateWorkspace As DataTable
        Dim JsStr As String = String.Empty
        Try
            If Not Me.ViewState(Vs_Status) Is Nothing Then
                objcommon.ShowAlert("Please save or cancel the previous changes", Me.Page)
                Exit Sub
            End If
            If e.CommandName.ToUpper = "DETTACH" Then
                If Not ShowHideControls() Then
                    Exit Sub
                End If
                'edit
                Me.ViewState(Vs_MedExWorkspaceHdrNo) = Me.gvmedexworkspadce.Rows(index).Cells(GVC_MedExWorkSpaceHdrNo).Text.Trim()
                Wstr = "nMedExWorkSpaceHdrNo='" & Me.gvmedexworkspadce.Rows(index).Cells(GVC_MedExWorkSpaceHdrNo).Text.Trim() & "'"
                Wstr += " And cStatusIndi <> 'D'"

                If Not objhelp.GetMedExWorkSpaceHdr(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_DeleteTemplateWorkspace, eStr_Retu) Then
                    objcommon.ShowAlert("Error While Getting data from attribute WorkspaceHdr", Me.Page)
                    Exit Sub
                End If
                dt_DeleteTemplateWorkspace = ds_DeleteTemplateWorkspace.Tables(0)
                ds_WorkspaceHdrDtl.Tables.Add(dt_DeleteTemplateWorkspace.Copy())
                ds_WorkspaceHdrDtl.Tables(0).TableName = "MedExWorkSpaceHdr"
                ds_DeleteTemplateWorkspace = Nothing
                dt_DeleteTemplateWorkspace = Nothing

                Wstr += " And cActiveFlag <> 'N'"

                If Not objhelp.GetMedExWorkSpaceDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_DeleteTemplateWorkspace, eStr_Retu) Then
                    objcommon.ShowAlert("Error While Getting data from attribute WorkspaceDtl", Me.Page)
                    Exit Sub
                End If

                dt_DeleteTemplateWorkspace = ds_DeleteTemplateWorkspace.Tables(0)
                ds_WorkspaceHdrDtl.Tables.Add(dt_DeleteTemplateWorkspace.Copy())
                ds_WorkspaceHdrDtl.Tables(1).TableName = "MedExWorkSpaceDtl"

                If Not objLambda.Save_MedExWorkspaceDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete, WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_MedExWorkspaceDtl, ds_WorkspaceHdrDtl, Me.Session(S_UserID), eStr_Retu) Then
                    objcommon.ShowAlert("Error While Dettaching Template In attribute WorkspaceHdr", Me.Page)
                    Exit Sub
                End If

                objcommon.ShowAlert("Attribute Template Dettached SuccessFully", Me.Page)

                If Not BindGrid() Then
                    Throw New Exception("Error in BindGrid()...")
                    Exit Sub
                End If
            End If

            If e.CommandName.ToUpper = "EDIT" Then
                Me.CollapsiblePanelExtender1.ClientState = True
                Me.CollapsiblePanelExtender1.Collapsed = True
                Me.PanelGridHeader.Style.Add("display", "")
            End If
            If e.CommandName.ToUpper = "REORDER" Then

                Me.lblMedexCount.Text = ""
                'edit
                Me.ViewState(Vs_MedExWorkspaceHdrNo) = Me.gvmedexworkspadce.Rows(index).Cells(GVC_MedExWorkSpaceHdrNo).Text.Trim()
                If Hdn_FreezeStatus.Value = "F" Then
                    objcommon.ShowAlert("This Project Is Freezed.Kindly UnFreeze This To Change Project Structure", Me.Page)
                    Exit Sub
                Else
                    If Not ShowHideControls() Then
                        Exit Sub
                    End If
                    If Not FillMedExDesc(Me.gvmedexworkspadce.Rows(index).Cells(GVC_WorkSpaceID).Text, Me.gvmedexworkspadce.Rows(index).Cells(GVC_iNodeId).Text) Then
                        Exit Sub
                    End If

                    dt_DtMedExDtl = CType(Me.ViewState(VS_DtMedExWorkSpaceDtl), DataTable).Copy

                    If Not dt_DtMedExDtl.Rows.Count > 0 Then
                        objcommon.ShowAlert("No Attributes Found For The Selected Activity", Me.Page)
                        Exit Sub
                    End If

                    For noOfrows As Integer = 0 To dt_DtMedExDtl.Rows.Count - 1
                        Dim li As New HtmlGenericControl("li")
                        Dim div As New HtmlGenericControl("div")
                        li.Attributes.Add("class", "ui-state-default")
                        'li.Style.Add("cursor", "move")
                        li.Style.Add("cursor", "pointer")
                        li.Style.Add("text-align", "left")
                        li.Style.Add("margin-bottom", "1%")
                        li.Style.Add("margin-left", "2%")
                        li.Style.Add("height", "40px")
                        li.Style.Add("border-radius", "7px 7px 7px 7px")
                        div.Attributes.Add("class", "innerdiv")
                        div.Style.Add("font-size", "0.9em")
                        div.Style.Add("font-weight", "bold")
                        div.Style.Add("margin-left", "5%")
                        div.Style.Add("line-height", "3.2em")
                        div.Style.Add("height", "35px")
                        div.Style.Add("overflow", "hidden")
                        div.InnerText = dt_DtMedExDtl.Rows(noOfrows)("vMedExDesc")
                        li.ID = dt_DtMedExDtl.Rows(noOfrows)("vMedExCode")
                        li.Attributes.Add("title", dt_DtMedExDtl.Rows(noOfrows)("vMedExDesc") + " (" + dt_DtMedExDtl.Rows(noOfrows)("vmedexsubGroupDesc") + ") " + "[" + dt_DtMedExDtl.Rows(noOfrows)("vmedexgroupDesc") + "] ")
                        li.Controls.Add(div)
                        li.Attributes.Add("class", "allmed")
                        Me.SeqMedex.Controls.Add(li)

                    Next

                    Me.SeqMedex.Attributes.Add("class", "six")
                    Me.lblMedexCount.Text = "Total No Of Attributes : " + dt_DtMedExDtl.Rows.Count.ToString()
                    Me.hdnMedexList.Value = JsonConvert.SerializeObject(dt_DtMedExDtl)
                    If Me.Hdn_FreezeStatus.Value = "F" Or Me.Hdn_ProjectLock.Value = "L" Then
                        btnSequence.Visible = False
                    Else
                        btnSequence.Visible = True
                    End If

                    Me.MPEMedexSequence.Show()
                End If


            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".......gvmedexworkspadce_RowCommand")
        End Try
    End Sub

    Protected Sub gvmedexworkspadce_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvmedexworkspadce.RowDataBound
        Dim lblsrNo As Label

        Try
            If e.Row.RowType = DataControlRowType.DataRow Or _
                e.Row.RowType = DataControlRowType.Header Or _
                e.Row.RowType = DataControlRowType.Footer Then

                e.Row.Cells(GVC_MedExWorkSpaceHdrNo).Visible = False
                e.Row.Cells(GVC_iNodeId).Visible = False

            End If

            If e.Row.RowType = DataControlRowType.DataRow Then

                CType(e.Row.FindControl("ImbEdit"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImbEdit"), ImageButton).CommandName = "EDIT"
                CType(e.Row.FindControl("ImbEdit"), ImageButton).Attributes.Add("OnClick", "return GetSubGroupName('" & e.Row.Cells(GvwMedEx_MedExWorkSpaceDtlNo).Text.Trim() & "' , '" & Session(S_ScopeNo) & "' , '" & e.Row.Cells(GVC_iNodeId).Text.Trim() & "','" & e.Row.Cells(GVC_ActivityName).Text.Trim() & "'  );")

                lblsrNo = CType(e.Row.FindControl("lblSrNo"), Label)
                lblsrNo.Text = e.Row.RowIndex + (Me.gvmedexworkspadce.PageIndex * Me.gvmedexworkspadce.PageSize) + 1

                CType(e.Row.FindControl("ImbDelete"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImbDelete"), ImageButton).CommandName = "DETTACH"

                CType(e.Row.FindControl("ImbPreview"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImbPreview"), ImageButton).CommandName = "PREVIEW"
                If (e.Row.Cells(GVC_WorkSpaceID).Text.ToString() = "") Then
                    'gvmedexworkspadce.PageIndex = 2
                    'gvmedexworkspadce_PageIndexChanging1(Nothing, Nothing)
                    CType(e.Row.FindControl("ImbPreview"), ImageButton).Attributes.Add("OnClick", "return PreviewTemplate('frmPreviewattributesForm.aspx?WorkspaceId=" & e.Row.Cells(GVC_WorkSpaceID).Text.ToString() & "&ActivityId=" & e.Row.Cells(GVC_ActivityID).Text.ToString() & "&iNodeId=" & e.Row.Cells(GVC_iNodeId).Text.ToString() & "');")
                End If
                CType(e.Row.FindControl("ImbPreview"), ImageButton).Attributes.Add("OnClick", "return PreviewTemplate('frmPreviewattributesForm.aspx?WorkspaceId=" & e.Row.Cells(GVC_WorkSpaceID).Text.ToString() & "&ActivityId=" & e.Row.Cells(GVC_ActivityID).Text.ToString() & "&iNodeId=" & e.Row.Cells(GVC_iNodeId).Text.ToString() & "');")

                CType(e.Row.FindControl("ImbReorder"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImbReorder"), ImageButton).CommandName = "REORDER"

            End If

        Catch ex As Exception
            Me.ShowErrorMessage("Error ", "")
        End Try

    End Sub

    Protected Sub gvmedexworkspadce_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs)
        Me.CollapsiblePanelExtender1.ClientState = True
        Me.CollapsiblePanelExtender1.Collapsed = True
        Me.PanelGridHeader.Style.Add("display", "")
    End Sub

    Protected Sub gvmedexworkspadce_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.Footer Or _
                   e.Row.RowType = DataControlRowType.DataRow Or _
                   e.Row.RowType = DataControlRowType.Header Then

            e.Row.Cells(GVC_WorkSpaceID).Visible = False
            e.Row.Cells(GVC_ProjectName).Visible = False
            e.Row.Cells(GVC_ProjectNo).Visible = False
            e.Row.Cells(GVC_ActivityID).Visible = False

        End If
    End Sub

#End Region

#Region "Project Specific Screening Grid Events"

    Protected Sub GVProjectSpcScreening_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GVProjectSpcScreening.RowCommand
        Dim index As Integer = e.CommandArgument
        Dim Wstr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim ds_DeleteTemplateWorkspace As New DataSet
        Dim ds_WorkspaceScreeningHdrDtl As New DataSet
        Dim JsStr As String = String.Empty
        Dim dt_DtMedExDtl As New DataTable
        Try

            If Not Me.ViewState(Vs_Status) Is Nothing Then
                objcommon.ShowAlert("Please save or cancel the previous changes", Me.Page)
                Exit Sub
            End If

            If e.CommandName.ToUpper = "DETTACH" Then

                If Not ShowHideControls() Then
                    Exit Sub
                End If

                Wstr = "vWorkspaceID='" & Me.GVProjectSpcScreening.Rows(index).Cells(GVPrSpScr_vWorkspaceID).Text.Trim() & "'"
                Wstr += " And cStatusIndi <> 'D'"
                If Not objhelp.GetWorkspaceScreeningHdr(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_DeleteTemplateWorkspace, eStr_Retu) Then
                    objcommon.ShowAlert("Error While Getting data from attribute WorkspaceHdr", Me.Page)
                    Exit Sub
                End If
                ds_WorkspaceScreeningHdrDtl.Tables.Add(ds_DeleteTemplateWorkspace.Tables(0).Copy())
                ds_WorkspaceScreeningHdrDtl.Tables(0).TableName = "WORKSPACESCREENINGHDR"
                ds_DeleteTemplateWorkspace = Nothing
                Wstr = "nWorkSpaceScreeningHdrNo='" & Me.GVProjectSpcScreening.Rows(index).Cells(GVPrSpScr_nWorkSpaceScreeningHdrNo).Text.Trim() & "'"
                Wstr += " And cStatusIndi <> 'D' And cActiveFlag <> 'N'"
                'Wstr += " And cActiveFlag <> 'N'"
                If Not objhelp.GetWorkspaceScreeningDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_DeleteTemplateWorkspace, eStr_Retu) Then
                    objcommon.ShowAlert("Error While Getting data from attribute WorkspaceDtl", Me.Page)
                    Exit Sub
                End If
                ds_WorkspaceScreeningHdrDtl.Tables.Add(ds_DeleteTemplateWorkspace.Tables(0).Copy())
                ds_WorkspaceScreeningHdrDtl.Tables(1).TableName = "WORKSPACESCREENINGDTL"
                If Not objLambda.Insert_WorkSpaceScreeningHdrDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete, ds_WorkspaceScreeningHdrDtl, eStr_Retu) Then
                    objcommon.ShowAlert("Error While Dettaching Template In attribute WorkspaceHdr", Me.Page)
                    Exit Sub
                End If

                objcommon.ShowAlert("Attribute Template Dettached SuccessFully", Me.Page)
                If Not BindGrid() Then
                    Throw New Exception("Error in BindGrid()...")
                End If
            End If
            If e.CommandName.ToUpper = "EDIT" Then
                If Not FillddlMedexGroup() Then
                    Exit Sub
                End If
                If Me.Hdn_ProjectLock.Value = "L" Then
                End If

                If Not FillUOM() Then
                    Exit Sub
                End If
                Me.ViewState(Vs_MedExWorkspaceHdrNo) = Me.GVProjectSpcScreening.Rows(index).Cells(GVPrSpScr_nWorkSpaceScreeningHdrNo).Text.Trim()
                If Not FillGridgvwMedExForProjectSpScr(CInt(Me.GVProjectSpcScreening.Rows(index).Cells(GVPrSpScr_nWorkSpaceScreeningHdrNo).Text.Trim())) Then
                    Exit Sub
                End If
                Me.CollapsiblePanelExtender1.ClientState = True
                Me.CollapsiblePanelExtender1.Collapsed = True
                Me.PanelGridHeader.Style.Add("display", "")
            End If

            If e.CommandName.ToUpper = "REORDER" Then
                Me.lblMedexCount.Text = ""

                If Not ShowHideControls() Then
                    Exit Sub
                End If

                If Not FillMedExDescForProjectSpScr(CInt(Me.GVProjectSpcScreening.Rows(index).Cells(GVPrSpScr_nWorkSpaceScreeningHdrNo).Text.Trim())) Then
                    Exit Sub
                End If
                dt_DtMedExDtl = CType(Me.ViewState(VS_DtMedExWorkSpaceDtl), DataTable).Copy

                If Not dt_DtMedExDtl.Rows.Count > 0 Then
                    objcommon.ShowAlert("No Attributes Found For The Selected Activity", Me.Page)
                    Exit Sub
                End If

                For noOfrows As Integer = 0 To dt_DtMedExDtl.Rows.Count - 1
                    Dim li As New HtmlGenericControl("li")
                    Dim div As New HtmlGenericControl("div")
                    li.Attributes.Add("class", "ui-state-default")
                    'li.Style.Add("cursor", "move")
                    li.Style.Add("cursor", "pointer")
                    li.Style.Add("text-align", "left")
                    li.Style.Add("margin-bottom", "1%")
                    li.Style.Add("margin-left", "2%")
                    li.Style.Add("height", "40px")
                    li.Style.Add("border-radius", "7px 7px 7px 7px")
                    div.Attributes.Add("class", "innerdiv")
                    div.Style.Add("font-size", "0.9em")
                    div.Style.Add("font-weight", "bold")
                    div.Style.Add("margin-left", "5%")
                    div.Style.Add("line-height", "3.2em")
                    div.Style.Add("height", "35px")
                    div.Style.Add("overflow", "hidden")
                    div.InnerText = dt_DtMedExDtl.Rows(noOfrows)("vMedExDesc")
                    li.ID = dt_DtMedExDtl.Rows(noOfrows)("vMedExCode")
                    li.Attributes.Add("title", dt_DtMedExDtl.Rows(noOfrows)("vMedExDesc") + " (" + dt_DtMedExDtl.Rows(noOfrows)("vmedexsubGroupDesc") + ") " + "[" + dt_DtMedExDtl.Rows(noOfrows)("vmedexgroupDesc") + "] ")
                    li.Controls.Add(div)
                    li.Attributes.Add("class", "allmed")
                    Me.SeqMedex.Controls.Add(li)

                Next

                Me.SeqMedex.Attributes.Add("class", "six")
                Me.lblMedexCount.Text = "Total No Of Attributes : " + dt_DtMedExDtl.Rows.Count.ToString()
                Me.hdnMedexList.Value = JsonConvert.SerializeObject(dt_DtMedExDtl)

                If Me.Hdn_FreezeStatus.Value = "F" Or Me.Hdn_ProjectLock.Value = "L" Then
                    btnSequence.Visible = False
                Else
                    btnSequence.Visible = True
                End If

                Me.MPEMedexSequence.Show()

            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "........GVProjectSpcScreening_RowCommand")
        Finally
        End Try

    End Sub

    Protected Sub GVProjectSpcScreening_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVProjectSpcScreening.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Or _
                e.Row.RowType = DataControlRowType.Header Or _
                e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(GVPrSpScr_nWorkSpaceScreeningHdrNo).Visible = False
                e.Row.Cells(GVPrSpScr_vWorkspaceID).Visible = False
                e.Row.Cells(GVPrSpScr_vProjectNo).Visible = False

            End If

            If e.Row.RowType = DataControlRowType.DataRow Then

                CType(e.Row.FindControl("ImbEdit"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImbEdit"), ImageButton).CommandName = "EDIT"
                CType(e.Row.FindControl("ImbEdit"), ImageButton).Attributes.Add("OnClick", "return GetSubGroupName_SpecificScreening('" & e.Row.Cells(GVPrSpScr_nWorkSpaceScreeningHdrNo).Text.Trim() & "' , '" & e.Row.Cells(GVPrSpScr_vWorkspaceID).Text.Trim() & "', '" & Session(S_ScopeNo) & "' , '" & e.Row.Cells(GVPrSpScr_vActivityName).Text.Trim() & "'  );")

                CType(e.Row.FindControl("ImbDelete"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImbDelete"), ImageButton).CommandName = "DETTACH"

                CType(e.Row.FindControl("ImbPreview"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImbPreview"), ImageButton).CommandName = "PREVIEW"

                CType(e.Row.FindControl("lblSrNo"), Label).Text = e.Row.RowIndex + 1
                CType(e.Row.FindControl("ImbPreview"), ImageButton).Attributes.Add("OnClick", "return PreviewTemplate('frmPreviewattributesForm.aspx?WorkspaceId=" & e.Row.Cells(GVPrSpScr_vWorkspaceID).Text.ToString() & "&WorkspaceScreeningHdrNo=" & e.Row.Cells(GVPrSpScr_nWorkSpaceScreeningHdrNo).Text.ToString() & "');")

                CType(e.Row.FindControl("ImbReorder"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImbReorder"), ImageButton).CommandName = "REORDER"


            End If
        Catch ex As Exception
            Me.ShowErrorMessage("Error in  GVProjectSpcScreening_RowDataBound", "")
        End Try
    End Sub

    Protected Sub GVProjectSpcScreening_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GVProjectSpcScreening.RowEditing
        e.Cancel = True
    End Sub

#End Region

#Region "Genereal Screening Grid Events"

    Protected Sub Gv_GeneralScr_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Gv_GeneralScr.RowCommand
        Dim index As Integer = Convert.ToInt64(e.CommandArgument)
        Dim Wstr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim ds_DeleteTemplateWorkspace As New DataSet
        Dim ds_WorkspaceScreeningHdrDtl As New DataSet
        Dim JsStr As String = String.Empty
        Dim dt_DtMedExDtl As New DataTable
        Dim Status As String
        Dim row As GridViewRow
        Try
            If Not e.CommandName.ToUpper = "FREEZE" Then
                row = Gv_GeneralScr.Rows(index)
                Status = TryCast(row.FindControl("imgFreezerStatus"), LinkButton).Text
            End If

            If Not Me.ViewState(Vs_Status) Is Nothing Then
                objcommon.ShowAlert("Please save or cancel the previous changes", Me.Page)
                Exit Sub
            End If

            If e.CommandName.ToUpper = "DETTACH" Then

                If Not ShowHideControls() Then
                    Exit Sub
                End If

                Wstr = "vWorkspaceID='" & Me.Gv_GeneralScr.Rows(index).Cells(Gv_GeneralScr_vWorkspaceID).Text.Trim() & "'"
                Wstr += " And cStatusIndi <> 'D' And nScreeningTemplateHdrNo=" & Me.Gv_GeneralScr.Rows(index).Cells(Gv_GeneralScr_nScreeningTemplateHdrNo).Text.Trim()
                If Not objhelp.GetScreeningTemplateHdr(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_DeleteTemplateWorkspace, eStr_Retu) Then
                    objcommon.ShowAlert("Error While Getting data from attribute Screening TemplateHdr", Me.Page)
                    Exit Sub
                End If
                ds_WorkspaceScreeningHdrDtl.Tables.Add(ds_DeleteTemplateWorkspace.Tables(0).Copy())
                ds_WorkspaceScreeningHdrDtl.Tables(0).TableName = "SCREENINGTEMPLATEHDR"
                ds_DeleteTemplateWorkspace = Nothing
                Wstr = "nScreeningTemplateHdrNo='" & Me.Gv_GeneralScr.Rows(index).Cells(Gv_GeneralScr_nScreeningTemplateHdrNo).Text.Trim() & "'"
                Wstr += " And cStatusIndi <> 'D'"
                Wstr += " And cActiveFlag <> 'N'"
                If Not objhelp.GetScreeningTemplateDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_DeleteTemplateWorkspace, eStr_Retu) Then
                    objcommon.ShowAlert("Error While Getting data from attribute WorkspaceDtl", Me.Page)
                    Exit Sub
                End If
                ds_WorkspaceScreeningHdrDtl.Tables.Add(ds_DeleteTemplateWorkspace.Tables(0).Copy())
                ds_WorkspaceScreeningHdrDtl.Tables(1).TableName = "SCREENINGTEMPLATEDTL"
                If Not objLambda.Insert_ScreeningTemplateHdrDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete, ds_WorkspaceScreeningHdrDtl, eStr_Retu) Then
                    objcommon.ShowAlert("Error While Dettaching Template In attribute WorkspaceHdr", Me.Page)
                    Exit Sub
                End If
                objcommon.ShowAlert("Attribute Template Dettached SuccessFully", Me.Page)
                Me.RBLProjecttype.SelectedValue = "0000000000"
                If Not BindGrid() Then
                    Throw New Exception("Error in BindGrid()...")
                End If
            End If

            If e.CommandName.ToUpper = "REORDER" Then

                Me.lblMedexCount.Text = ""

                If Not ShowHideControls() Then
                    Exit Sub
                End If

                If Not FillGenScrMedexDesc(CInt(Me.Gv_GeneralScr.Rows(index).Cells(Gv_GeneralScr_nScreeningTemplateHdrNo).Text.Trim())) Then
                    Throw New Exception()
                End If

                dt_DtMedExDtl = CType(Me.ViewState(VS_DtMedExWorkSpaceDtl), DataTable).Copy

                If Not dt_DtMedExDtl.Rows.Count > 0 Then
                    objcommon.ShowAlert("No Attributes Found For The Selected Activity", Me.Page)
                    Exit Sub
                End If

                For noOfrows As Integer = 0 To dt_DtMedExDtl.Rows.Count - 1
                    Dim li As New HtmlGenericControl("li")
                    Dim div As New HtmlGenericControl("div")
                    li.Attributes.Add("class", "ui-state-default")
                    'li.Style.Add("cursor", "move")
                    li.Style.Add("cursor", "pointer")
                    li.Style.Add("text-align", "left")
                    li.Style.Add("margin-bottom", "1%")
                    li.Style.Add("margin-left", "2%")
                    li.Style.Add("height", "40px")
                    li.Style.Add("border-radius", "7px 7px 7px 7px")
                    div.Attributes.Add("class", "innerdiv")
                    div.Style.Add("font-size", "0.9em")
                    div.Style.Add("font-weight", "bold")
                    div.Style.Add("margin-left", "5%")
                    div.Style.Add("line-height", "3.2em")
                    div.Style.Add("height", "35px")
                    div.Style.Add("overflow", "hidden")
                    div.InnerText = dt_DtMedExDtl.Rows(noOfrows)("vMedExDesc")
                    li.ID = dt_DtMedExDtl.Rows(noOfrows)("vMedExCode")
                    li.Attributes.Add("title", dt_DtMedExDtl.Rows(noOfrows)("vMedExDesc") + " (" + dt_DtMedExDtl.Rows(noOfrows)("vmedexsubGroupDesc") + ") " + "[" + dt_DtMedExDtl.Rows(noOfrows)("vmedexgroupDesc") + "] ")
                    li.Controls.Add(div)
                    li.Attributes.Add("class", "allmed")
                    Me.SeqMedex.Controls.Add(li)

                Next

                Me.SeqMedex.Attributes.Add("class", "six")
                Me.lblMedexCount.Text = "Total No Of Attributes : " + dt_DtMedExDtl.Rows.Count.ToString()
                Me.hdnMedexList.Value = JsonConvert.SerializeObject(dt_DtMedExDtl)

                If Me.Hdn_FreezeStatus.Value = "F" Or Me.Hdn_ProjectLock.Value = "L" Then
                    btnSequence.Visible = False
                Else
                    btnSequence.Visible = True
                End If

                Me.MPEMedexSequence.Show()

            ElseIf e.CommandName.ToUpper = "FREEZE" Then
                Me.mpfreeze.Show()
                txtTemplateheaderno.Text = e.CommandArgument
            ElseIf e.CommandName.ToUpper = "EDIT" And Status = "Freeze" Then
            End If
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub Gv_GeneralScr_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Gv_GeneralScr.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Or _
                e.Row.RowType = DataControlRowType.Header Or _
                e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(Gv_GeneralScr_nScreeningTemplateHdrNo).Visible = False
                e.Row.Cells(Gv_GeneralScr_vWorkspaceID).Visible = False
                e.Row.Cells(Gv_GeneralScr_vProjectNo).Visible = False
                e.Row.Cells(Gv_GeneralScr_vProjectNo).Visible = False
                e.Row.Cells(Gv_GeneralScr_StatusForText).Visible = False

            End If

            If e.Row.RowType = DataControlRowType.DataRow Then

                CType(e.Row.FindControl("ImbEdit"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImbEdit"), ImageButton).CommandName = "EDIT"
                CType(e.Row.FindControl("ImbEdit"), ImageButton).Attributes.Add("OnClick", "return GetSubGroupName_GenericScreening('" & e.Row.Cells(Gv_GeneralScr_nScreeningTemplateHdrNo).Text.Trim() & "' , '" & Session(S_ScopeNo) & "' ,'" & e.Row.Cells(Gv_GeneralScr_StatusForText).Text & "'   );")

                CType(e.Row.FindControl("ImbDelete"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImbDelete"), ImageButton).CommandName = "DETTACH"

                CType(e.Row.FindControl("ImbPreview"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImbPreview"), ImageButton).CommandName = "PREVIEW"

                CType(e.Row.FindControl("lblSrNo"), Label).Text = e.Row.RowIndex + 1
                CType(e.Row.FindControl("ImbPreview"), ImageButton).Attributes.Add("OnClick", "return PreviewTemplate('frmPreviewattributesForm.aspx?WorkspaceId=" & e.Row.Cells(Gv_GeneralScr_vWorkspaceID).Text.ToString() & "&ScreeningTemplateHdrno=" & e.Row.Cells(Gv_GeneralScr_nScreeningTemplateHdrNo).Text.ToString() & "');")

                CType(e.Row.FindControl("ImbReorder"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImbReorder"), ImageButton).CommandName = "REORDER"

                CType(e.Row.FindControl("ImbReorder"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImbReorder"), ImageButton).CommandName = "REORDER"


                CType(e.Row.FindControl("imgFreezerStatus"), LinkButton).CommandArgument = e.Row.Cells(Gv_GeneralScr_nScreeningTemplateHdrNo).Text.ToString()
                CType(e.Row.FindControl("imgFreezerStatus"), LinkButton).CommandName = "Freeze"


                CType(e.Row.FindControl("imgAuditTrail"), ImageButton).Attributes.Add("OnClick", "Audittrail(" & e.Row.Cells(Gv_GeneralScr_nScreeningTemplateHdrNo).Text.ToString() & ")")
                CType(e.Row.FindControl("imgAuditTrail"), ImageButton).CommandName = "Auditrail"
                CType(e.Row.FindControl("imgAuditTrail"), ImageButton).CommandArgument = e.Row.Cells(Gv_GeneralScr_nScreeningTemplateHdrNo).Text

                If e.Row.Cells(Gv_GeneralScr_StatusForText).Text = "Freeze" Then
                    'e.Row.Cells(Gv_GeneralScr_Edit).Enabled = False
                    e.Row.Cells(Gv_GeneralScr_Deatch).Enabled = False
                    e.Row.Cells(Gv_GeneralScr_Order).Enabled = False
                    e.Row.Cells(Gv_GeneralScr_Status).Enabled = False
                End If

            End If
        Catch ex As Exception
            Me.ShowErrorMessage("Error in  GVProjectSpcScreening_RowDataBound", "")
        End Try
    End Sub

    Protected Sub Gv_GeneralScr_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles Gv_GeneralScr.RowEditing
        e.Cancel = True
    End Sub

#End Region


#End Region

#Region "FindusedMedexForFormula"

    Protected Function MedexUsedForFormula(ByVal ds_save As DataSet) As Boolean
        Dim ds_MedexGroup As New DataSet
        Dim vMedexcode As String = String.Empty
        Dim wstr As String = String.Empty
        Try
            wstr = "cStatusIndi <> 'D' order by vMedExGroupDesc"

            If Not objhelp.GetMedExMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                            ds_MedexGroup, eStr) Then
                Me.objcommon.ShowAlert("Error While Getting Data from Attribute GroupMst:" + eStr, Me.Page)
                Exit Function
            End If
            For i As Integer = 0 To ds_save.Tables(0).Rows.Count - 1
                vMedexcode += "'" + ds_save.Tables(0).Rows(i).Item("vMedexCode").ToString.Trim + "',"
            Next
            If Not vMedexcode = "" Then
                vMedexcode = vMedexcode.Substring(0, vMedexcode.LastIndexOf(","))
            End If

            Dim dv_co As New DataView
            dv_co = ds_MedexGroup.Tables(0).DefaultView
            dv_co.RowFilter = "vMedexCode in(" + vMedexcode + ") and vMedexType='Formula' and vMedexCodeForFormula is Not Null"
            Dim dt_filterMedxCode As DataTable = dv_co.ToTable

            Dim ds_FilterMedex As New DataSet
            ds_FilterMedex.Tables.Add(dt_filterMedxCode)
            ds_FilterMedex.AcceptChanges()
            Dim vMedexCodeUsed As String = String.Empty

            For i As Integer = 0 To ds_FilterMedex.Tables(0).Rows.Count - 1
                vMedexCodeUsed += ds_FilterMedex.Tables(0).Rows(i).Item("vMedexCodeForFormula")
            Next
            If Not vMedexCodeUsed = "" Then
                vMedexCodeUsed = vMedexCodeUsed.Substring(0, vMedexCodeUsed.LastIndexOf(","))
            End If
            If vMedexCodeUsed <> "" Then
                Dim arrvMedxCode() As String
                arrvMedxCode = vMedexCodeUsed.Split(",")
                Dim Arr As New ArrayList

                For nDx As Integer = 0 To arrvMedxCode.Length - 1
                    If Not Arr.Contains(arrvMedxCode(nDx)) Then
                        Arr.Add(arrvMedxCode(nDx))
                    End If
                Next


                Dim vMedex As String = String.Empty
                Dim ArrMedex As New ArrayList
                Dim dataview As New DataView
                dataview = ds_save.Tables(0).DefaultView

                For Index1 As Integer = 0 To Arr.Count - 1
                    dataview.RowFilter = "vMedexCode='" + Arr(Index1) + "'"
                    If dataview.ToTable.Rows.Count = 0 Then
                        vMedex += "'" + Arr(Index1) + "',"
                    End If
                Next

                Dim medexstring As String = String.Empty
                Dim ds_require As New DataSet
                If Not vMedex = "" Then
                    vMedex = vMedex.Substring(0, vMedex.LastIndexOf(","))
                    Dim dv_New As New DataView
                    dv_New = ds_MedexGroup.Tables(0).DefaultView
                    dv_New.RowFilter = "vMedexCode in(" + vMedex + ")"
                    Dim dt_filter As DataTable = dv_New.ToTable
                    ds_require.Tables.Add(dt_filter)
                    ds_require.AcceptChanges()

                    If ds_require.Tables(0).Rows.Count > 0 Then
                        For index1 As Integer = 0 To ds_require.Tables(0).Rows.Count - 1
                            medexstring += ds_require.Tables(0).Rows(index1).Item("MedExDescWithSubGroup").ToString + " and "
                        Next
                    End If
                    medexstring = medexstring.Substring(0, medexstring.LastIndexOf("and"))
                    Me.objcommon.ShowAlert("Please Add Attribute For Related Formula", Me.Page())
                    'MPEditMedex.Show()
                    Me.CollapsiblePanelExtender1.ClientState = True
                    Me.CollapsiblePanelExtender1.Collapsed = True
                    'Me.pnlMedEx.Style.Add("display", "")
                    Me.PanelGridHeader.Style.Add("display", "")
                    Return False
                End If
            End If
            '================================================================================================
            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
            eStr = ex.Message
            Return False
        End Try
    End Function
#End Region

#Region "Resetpage"

    Private Function Resetpage() As Boolean
        Try
            Me.ddlActivity.SelectedIndex = -1
            Me.ddlTemplate.SelectedIndex = -1
            Me.txtproject.Text = ""
            Me.txtproject.Enabled = True
            Me.tdActivity.Attributes.Add("Style", "display:none")
            Me.ddlActivity.Attributes.Add("Style", "display:none")
            'Me.HProjectId.Value = ""
            Me.gvmedexworkspadce.DataSource = Nothing
            Me.gvmedexworkspadce.DataBind()
            Me.GVProjectSpcScreening.EmptyDataText = Nothing
            Me.GVProjectSpcScreening.DataSource = Nothing
            Me.GVProjectSpcScreening.DataBind()
            Me.Gv_GeneralScr.DataSource = Nothing
            Me.Gv_GeneralScr.DataBind()
            'Me.GvGenScr_Medex.DataSource = Nothing
            'Me.GvGenScr_Medex.DataBind()
            'Me.GV_ProjectSpScr.DataSource = Nothing
            'Me.GV_ProjectSpScr.DataBind()
            'Me.gvwMedEx.DataSource = Nothing
            'Me.gvwMedEx.DataBind()
            Me.VersionDtl.Attributes.Add("style", "display:none;")
            Me.ViewState(Vs_Status) = Nothing
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("Error while Reset Page..." & ex.ToString, "")
            Return False
        End Try
    End Function

#End Region

#Region "ShowHide Controls"

    Private Function ShowHideControls() As Boolean
        Try
            Me.PanelGridHeader.Style.Add("display", "none")
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("Error while ShowHide Controls..." & ex.ToString, "")
            Return False
        End Try
    End Function
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
#Region "Helper Function"

    Private Function CheckProjectSpecificScreeningStart() As Boolean
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim ds As New DataSet
        Try
            wstr = "vWorkspaceId='" + Me.HProjectId.Value.ToString + "' and cStatusIndi <> 'D'"
            If Not Me.objhelp.GetMedExWorkSpaceScreeningHdr(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr) Then
                Me.objcommon.ShowAlert("Error While Getting Data from MedExWorkSpaceScreeningHdr:" + estr, Me.Page)
                Return False
            End If
            If ds.Tables(0).Rows.Count > 0 Then
                Return False
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "........CheckProjectSpecificScreeningStart")
            Return False
        End Try
    End Function

#End Region


#Region "Web Methods"
    <Web.Services.WebMethod()> _
    Public Shared Function AuditTrail(ByVal TemplateTd As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim ds As DataSet
        Dim eStr As String = String.Empty
        Dim strReturn As String = String.Empty
        Try
            ds = objHelp.ProcedureExecute("Proc_ScreeningTemplateVersionMstAudittrail", TemplateTd)
            If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                Dim column As DataColumn
                column = New DataColumn()
                column.DataType = System.Type.GetType("System.Int32")
                column.ColumnName = "Sr.No"
                ds.Tables(0).Columns.Add(column)
                ds.AcceptChanges()
                strReturn = JsonConvert.SerializeObject(ds.Tables(0))
            End If

            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function GetSubGroupName(ByVal vWorkspaceId As String, ByVal Scope As String, ByVal iNodeId As Integer) As String
        Dim ObjCommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim ds_gvwMedEx As DataSet = New DataSet()
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty
        Try
            wstr = "vWorkspaceId='" + vWorkspaceId.ToString.Trim() + "' And iNodeId=" + iNodeId.ToString.Trim() + " And cStatusIndi <> 'D' And cActiveFlag <> 'N' "

            If Scope <> Scope_ClinicalTrial Then
                wstr += " And vMedExType <> 'IMPORT'"
            End If

            If Not objhelpDb.GetViewMedExWorkSpaceDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_gvwMedEx, estr) Then
                Return "Error While Data Fetching."
            End If
            Return JsonConvert.SerializeObject(ds_gvwMedEx.Tables(0).DefaultView.ToTable(True, "vMedexSubGroupCode", "vMedExSubGroupDesc", "vWorkspaceId", "iNodeId"))
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function GetSubGroupName_SpecificScreening(ByVal nWorkSpaceScreeningHdrNo As String, ByVal vMedExGroupCode As String, ByVal Scope As String) As String
        Dim ObjCommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim ds_gvwMedEx As DataSet = New DataSet()
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty
        Try
            wstr = "nWorkSpaceScreeningHdrNo=" + nWorkSpaceScreeningHdrNo.ToString.Trim() + " And vMedExGroupCode='" + vMedExGroupCode.ToString.Trim() + "' And cStatusIndi <> 'D' And cActiveFlag <> 'N' "

            If Scope <> Scope_ClinicalTrial Then
                wstr += " And vMedExType <> 'IMPORT'"
            End If

            If Not objhelpDb.View_WorkspaceScreeningHdrDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_gvwMedEx, estr) Then
                Return "Error While Data Fetching."
            End If
            Return JsonConvert.SerializeObject(ds_gvwMedEx.Tables(0).DefaultView.ToTable(True, "vMedexSubGroupCode", "vMedExSubGroupDesc", "vWorkspaceId", "nWorkSpaceScreeningHdrNo", "vMedExGroupCode"))
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function GetSubGroupName_GenericScreening(ByVal nScreeningTemplateHdrNo As String, ByVal vMedExGroupCode As String, ByVal Scope As String) As String
        Dim ObjCommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim ds_gvwMedEx As DataSet = New DataSet()
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty
        Try

            wstr = "nScreeningTemplateHdrNo=" + nScreeningTemplateHdrNo.ToString.Trim() + " And cStatusIndi <> 'D' And cActiveFlag <> 'N' "
            If Convert.ToString(vMedExGroupCode) <> "0" Then
                wstr += " And vMedExGroupCode='" + vMedExGroupCode.ToString.Trim() + "'"
            End If

            If Scope <> Scope_ClinicalTrial Then
                wstr += " And vMedExType <> 'IMPORT'"
            End If

            If Not objhelpDb.View_GeneralScreeningHdrDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_gvwMedEx, estr) Then
                Return estr
            End If
            Return JsonConvert.SerializeObject(ds_gvwMedEx.Tables(0).DefaultView.ToTable(True, "vMedexSubGroupCode", "vMedExSubGroupDesc", "vWorkspaceId", "nScreeningTemplateHdrNo"))
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function GetMedExGroupData(ByVal Scope As String) As String
        Dim ObjCommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim ds_MedexGroup As New DataSet
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Try
            If Not objcommon.GetScopeValueWithCondition(wstr) Then
                Return False
            End If
            wstr += " And cStatusIndi <> 'D' order by vMedExGroupDesc "
            If Not objhelpDb.GetMedExMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                            ds_MedexGroup, estr) Then

            End If
            Return JsonConvert.SerializeObject(ds_MedexGroup.Tables(0).DefaultView.ToTable(True, "vMedExGroupCode", "vMedExGroupDesc"))
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function GetAttributeGroupWise(ByVal GroupId As String, ByVal vMedExSubGroupCode As String) As String

        Dim ObjCommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim ds_MedexMst As New DataSet
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty

        Try
            wstr = " vMedexGroupCode='" + Convert.ToString(GroupId) + "'  AND vMedExSubGroupCode='" + vMedExSubGroupCode + "'  "
            wstr += " And cStatusIndi <> 'D' and cActiveFlag <> 'N' order by vMedExDesc"
            If Not objhelpDb.GetMedExMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_MedexMst, estr) Then
                Return estr
            End If

            Return JsonConvert.SerializeObject(ds_MedexMst.Tables(0))
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function GetUOM(ByVal Scope As String) As String
        Dim ObjCommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim Ds_FillUOMMst As New DataSet
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty

        Try
            If Not objhelpDb.GetUOMMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, Ds_FillUOMMst, estr) Then
            End If
            Return JsonConvert.SerializeObject(Ds_FillUOMMst.Tables(0).DefaultView.ToTable(True, "vUOMDesc"))

        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function GetMedDraTable(ByVal Scope As String) As String
        Dim objcommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
        Dim ds_Tables As New DataSet
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty

        Try
            wstr = "cRefTableType ='D' And cActiveFlag ='Y' AND cStatusIndi <> 'D' "
            If Not objhelpDb.GetReferenceTableDefinitions(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Tables, estr) Then

                Exit Function
            End If
            Return JsonConvert.SerializeObject(ds_Tables.Tables(0).DefaultView.ToTable(True, "vRefTableName", "nRefMasterNo"))

        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function GetViewMedExWorkSpaceDtl(ByVal vWorkspaceId As String, ByVal iNodeId As String, Scope As String, ByVal vMedExSubGroupCode As String) As String
        Dim objcommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
        Dim ds_MedExWorkSpaceDtl As DataSet = New DataSet()
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty
        Try
            wstr = "vWorkspaceId='" + vWorkspaceId.ToString.Trim() + "' And iNodeId=" + iNodeId.ToString.Trim() + " And vMedExSubGroupCode=" + vMedExSubGroupCode.ToString.Trim() + " And cStatusIndi <> 'D' And cActiveFlag <> 'N' "

            If Scope <> Scope_ClinicalTrial Then
                wstr += " And vMedExType <> 'IMPORT'"
            End If
            wstr += " Order By nMedExWorkSpaceHdrNo ,iSeqNo"

            If Not objhelpDb.GetViewMedExWorkSpaceDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_MedExWorkSpaceDtl, estr) Then
                Return "Error While Data Fetching."
            End If
            Return JsonConvert.SerializeObject(ds_MedExWorkSpaceDtl.Tables(0))
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function GetView_WorkspaceScreeningHdrDtl(ByVal nWorkspaceScreeningHdrNo As String, ByVal vMedExGroupCode As String, Scope As String, ByVal vMedExSubGroupCode As String) As String
        Dim objcommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
        Dim ds_gvwMedEx As DataSet = New DataSet()
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty
        Try
            wstr = "nWorkspaceScreeningHdrNo=" + nWorkspaceScreeningHdrNo.ToString.Trim() + " And vMedExGroupCode='" + vMedExGroupCode.ToString.Trim() + "' AND vMedExSubGroupCode = '" + vMedExSubGroupCode + "'  And cStatusIndi <> 'D' And cActiveFlag <> 'N' "

            If Scope <> Scope_ClinicalTrial Then
                wstr += " And vMedExType <> 'IMPORT'"
            End If

            wstr += " Order by iSeqNo,nWorkspaceScreeningHdrNo "

            If Not objhelpDb.View_WorkspaceScreeningHdrDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_gvwMedEx, estr) Then
                Return "Error While Data Fetching."
            End If

            Return JsonConvert.SerializeObject(ds_gvwMedEx.Tables(0))
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function GetView_GeneralScreeningHdrDtl(ByVal nScreeningTemplateHdrNo As String, ByVal vMedExGroupCode As String, Scope As String, ByVal vMedExSubGroupCode As String) As String
        Dim objcommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
        Dim ds_gvwMedEx As DataSet = New DataSet()
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty

        Try

            wstr = "nScreeningTemplateHdrNo=" + nScreeningTemplateHdrNo.ToString.Trim() + " And vMedExSubGroupCode = '" + vMedExSubGroupCode + "' AND cStatusIndi <> 'D' And cActiveFlag <> 'N' "
            If Convert.ToString(vMedExGroupCode) <> "0" Then
                wstr += " And vMedExGroupCode='" + vMedExGroupCode.ToString.Trim() + "'"
            End If

            If Scope <> Scope_ClinicalTrial Then
                wstr += " And vMedExType <> 'IMPORT'"
            End If

            If Not objhelpDb.View_GeneralScreeningHdrDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_gvwMedEx, estr) Then
                Return estr
            End If

            Return JsonConvert.SerializeObject(ds_gvwMedEx.Tables(0))
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function



    <Web.Services.WebMethod()> _
    Public Shared Function Save_MedExWorkSpaceDtl(ByVal objMedExWorkSpaceDtl As Object()) As String
        Dim objcommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty

        Dim ds_MedExWorkSpaceDtl As DataSet = New DataSet()
        Dim ds_save As DataSet = New DataSet()
        Dim temp_dt As DataTable = New DataTable()
        Dim temp As DataTable = New DataTable()
        Dim userid = objMedExWorkSpaceDtl(0)("iModifyBy").ToString()
        Try
            wstr = "vWorkspaceId='" + objMedExWorkSpaceDtl(0)("vWorkspaceId").ToString().Trim() + "' And iNodeId=" + objMedExWorkSpaceDtl(0)("iNodeId").ToString().Trim() + " And cStatusIndi <> 'D' And cActiveFlag <> 'N' "
            If objMedExWorkSpaceDtl(0)("Scope").ToString().Trim() <> Scope_ClinicalTrial Then
                wstr += " And vMedExType <> 'IMPORT'"
            End If
            wstr += " Order By nMedExWorkSpaceHdrNo ,iSeqNo"
            wstr = "nMedExWorkSpaceHdrNo = " + objMedExWorkSpaceDtl(0)("nMedExWorkSpaceHdrNo").ToString()
            If Not objhelpDb.GetMedExWorkSpaceDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_MedExWorkSpaceDtl, estr) Then
                Return "Error While Data Fetching."
            End If
            temp_dt = ds_MedExWorkSpaceDtl.Tables(0)
            For Each dr As DataRow In ds_MedExWorkSpaceDtl.Tables(0).Rows
                For i As Integer = 0 To objMedExWorkSpaceDtl.Count - 1
                    If (Convert.ToString(dr("nMedExWorkSpaceDtlNo")) = objMedExWorkSpaceDtl(i)("nMedExWorkSpaceDtlNo").ToString()) Then
                        dr.Item("vMedExDesc") = objMedExWorkSpaceDtl(i)("vMedExDesc").ToString()
                        dr.Item("vMedExType") = objMedExWorkSpaceDtl(i)("vMedExType").ToString()
                        dr.Item("vMedExValues") = objMedExWorkSpaceDtl(i)("vMedExValues").ToString()
                        dr.Item("vMedExDefaultValue") = objMedExWorkSpaceDtl(i)("vDefaultValue").ToString()
                        dr.Item("vWarningOnValue") = objMedExWorkSpaceDtl(i)("vAlertonvalue").ToString()
                        dr.Item("vWarningMsg") = objMedExWorkSpaceDtl(i)("vAlertMessage").ToString()
                        dr.Item("vLowRange") = objMedExWorkSpaceDtl(i)("vLowRange").ToString()
                        dr.Item("vHighRange") = objMedExWorkSpaceDtl(i)("vHighRange").ToString()
                        dr.Item("vUOM") = objMedExWorkSpaceDtl(i)("vUOM").ToString()
                        dr.Item("vValidationType") = objMedExWorkSpaceDtl(i)("vValidationType").ToString() + "," + objMedExWorkSpaceDtl(i)("iLength").ToString()
                        dr.Item("cWarningType") = objMedExWorkSpaceDtl(i)("cAlertType").ToString()
                        dr.Item("vCDISCValues") = objMedExWorkSpaceDtl(i)("vCDISCValues").ToString()
                        dr.Item("vOtherValues") = objMedExWorkSpaceDtl(i)("vOtherValues").ToString()
                        'dr.Item("vMedExCode") = objMedExWorkSpaceDtl(i)("vMedExCode").ToString()
                        dr.Item("iModifyBy") = objMedExWorkSpaceDtl(i)("iModifyBy").ToString()
                        dr.Item("vRefTable") = objMedExWorkSpaceDtl(i)("vRefTable").ToString()

                        Exit For
                    End If
                Next
            Next

            ds_MedExWorkSpaceDtl.Tables(0).AcceptChanges()
            temp_dt = ds_MedExWorkSpaceDtl.Tables(0).Copy()
            temp_dt.TableName = "MedExWorkSpaceDtl"
            ds_save.Tables.Add(temp_dt)

            If ds_save.Tables(0).Rows.Count > 0 Then
                If Not objLambda.Save_MedExWorkspaceDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                        WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_MedExWorkspaceDtl, ds_save, userid, estr) Then
                    Return "Error While Data Saving."
                    Exit Function
                End If

            End If

            Return "True"
        Catch ex As Exception
            Return "Error While Saving Attribute Details In MedexTemplateDtl"
        End Try
    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function Save_WorkspaceScreeningDtl(ByVal objWorkspaceScreeningDtl As Object()) As String
        Dim objcommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim ds_WorkspaceScreeningDtl As DataSet = New DataSet()
        Dim ds_save As DataSet = New DataSet()
        Dim temp_dt As DataTable = New DataTable()
        Dim temp As DataTable = New DataTable()
        Dim userid = objWorkspaceScreeningDtl(0)("iModifyBy").ToString()

        Try

            wstr = "nWorkspaceScreeningHdrNo=" + objWorkspaceScreeningDtl(0)("nWorkspaceScreeningHdrNo").ToString() + " And vMedExGroupCode='" + objWorkspaceScreeningDtl(0)("vMedExGroupCode").ToString() + "' And cStatusIndi <> 'D' And cActiveFlag <> 'N' "
            If Not objhelpDb.GetWorkspaceScreeningDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_WorkspaceScreeningDtl, estr) Then
                Return "Error While Data Fetching."
            End If

            For Each dr As DataRow In ds_WorkspaceScreeningDtl.Tables(0).Rows
                For i As Integer = 0 To objWorkspaceScreeningDtl.Count - 1
                    If (Convert.ToString(dr("nWorkSpaceScreeningDtlNo")) = objWorkspaceScreeningDtl(i)("nWorkspaceScreeningDtlNo").ToString()) Then
                        dr.Item("vMedExDesc") = objWorkspaceScreeningDtl(i)("vMedExDesc").ToString()
                        'dr.Item("vMedExType") = objMedExWorkSpaceDtl(i)("vMedExType").ToString()
                        dr.Item("vMedExValues") = objWorkspaceScreeningDtl(i)("vMedExValues").ToString()
                        dr.Item("vMedExDefaultValue") = objWorkspaceScreeningDtl(i)("vDefaultValue").ToString()
                        dr.Item("vWarningOnValue") = objWorkspaceScreeningDtl(i)("vAlertonvalue").ToString()
                        dr.Item("vWarningMsg") = objWorkspaceScreeningDtl(i)("vAlertMessage").ToString()
                        dr.Item("vLowRange") = objWorkspaceScreeningDtl(i)("vLowRange").ToString()
                        dr.Item("vHighRange") = objWorkspaceScreeningDtl(i)("vHighRange").ToString()
                        dr.Item("vUOM") = objWorkspaceScreeningDtl(i)("vUOM").ToString()
                        dr.Item("vValidationType") = objWorkspaceScreeningDtl(i)("vValidationType").ToString() + "," + objWorkspaceScreeningDtl(i)("iLength").ToString()
                        dr.Item("cWarningType") = objWorkspaceScreeningDtl(i)("cAlertType").ToString()
                        dr.Item("vCDISCValues") = objWorkspaceScreeningDtl(i)("vCDISCValues").ToString()
                        dr.Item("vOtherValues") = objWorkspaceScreeningDtl(i)("vOtherValues").ToString()
                        'dr.Item("vMedExCode") = objWorkspaceScreeningDtl(i)("vMedExCode").ToString()
                        dr.Item("iModifyBy") = objWorkspaceScreeningDtl(i)("iModifyBy").ToString()
                        dr.Item("vRefTable") = objWorkspaceScreeningDtl(i)("vRefTable").ToString()
                        Exit For
                    End If
                Next
            Next

            ds_WorkspaceScreeningDtl.Tables(0).AcceptChanges()

            If ds_WorkspaceScreeningDtl.Tables(0).Rows.Count > 0 Then
                If Not objLambda.Insert_WorkSpaceScreeningHdrDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_WorkspaceScreeningDtl, estr) Then
                    Throw New Exception("Error While Saving Attribute Details In Project Specific Screening" + estr)
                End If

            End If

            Return "True"
        Catch ex As Exception
            Return "Error While Saving Attribute Details In MedexTemplateDtl"
        End Try
    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function Save_ScreeningTemplateDtl(ByVal objScreeningTemplateDtl As Object()) As String
        Dim objcommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty

        Dim ds_ScreeningTemplateDtl As DataSet = New DataSet()
        Dim ds_save As DataSet = New DataSet()
        Dim temp_dt As DataTable = New DataTable()
        Dim temp As DataTable = New DataTable()
        Dim userid = objScreeningTemplateDtl(0)("iModifyBy").ToString()

        Try

            wstr = "nScreeningTemplateHdrNo=" + objScreeningTemplateDtl(0)("nScreeningTemplateHdrNo").ToString() + " And vMedExSubGroupCode = '" + objScreeningTemplateDtl(0)("vMedExSubGroupCode").ToString() + "' AND cStatusIndi <> 'D' And cActiveFlag <> 'N' "
            If Convert.ToString(objScreeningTemplateDtl(0)("vMedExGroupCode")) <> "0" Then
                wstr += " And vMedExGroupCode='" + Convert.ToString(objScreeningTemplateDtl(0)("vMedExGroupCode")) + "'"
            End If

            If objScreeningTemplateDtl(0)("Scope").ToString() <> Scope_ClinicalTrial Then
                wstr += " And vMedExType <> 'IMPORT'"
            End If

            If Not objhelpDb.GetScreeningTemplateDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ScreeningTemplateDtl, estr) Then
                Return "Error While Data Fetching."
            End If


            For Each dr As DataRow In ds_ScreeningTemplateDtl.Tables(0).Rows
                For i As Integer = 0 To objScreeningTemplateDtl.Count - 1
                    If (Convert.ToString(dr("nScreeningTemplateDtlNo")) = objScreeningTemplateDtl(i)("nScreeningTemplateDtlNo").ToString()) Then
                        dr.Item("vMedExDesc") = objScreeningTemplateDtl(i)("vMedExDesc").ToString()
                        dr.Item("vMedExValues") = objScreeningTemplateDtl(i)("vMedExValues").ToString()
                        dr.Item("vMedExDefaultValue") = objScreeningTemplateDtl(i)("vDefaultValue").ToString()
                        dr.Item("vWarningOnValue") = objScreeningTemplateDtl(i)("vAlertonvalue").ToString()
                        dr.Item("vWarningMsg") = objScreeningTemplateDtl(i)("vAlertMessage").ToString()
                        dr.Item("vLowRange") = objScreeningTemplateDtl(i)("vLowRange").ToString()
                        dr.Item("vHighRange") = objScreeningTemplateDtl(i)("vHighRange").ToString()
                        dr.Item("vUOM") = objScreeningTemplateDtl(i)("vUOM").ToString()
                        dr.Item("vValidationType") = objScreeningTemplateDtl(i)("vValidationType").ToString() + "," + objScreeningTemplateDtl(i)("iLength").ToString()
                        dr.Item("cWarningType") = objScreeningTemplateDtl(i)("cAlertType").ToString()
                        dr.Item("vCDISCValues") = objScreeningTemplateDtl(i)("vCDISCValues").ToString()
                        dr.Item("vOtherValues") = objScreeningTemplateDtl(i)("vOtherValues").ToString()
                        dr.Item("iModifyBy") = objScreeningTemplateDtl(i)("iModifyBy").ToString()
                        Exit For
                    End If
                Next
            Next

            ds_ScreeningTemplateDtl.Tables(0).AcceptChanges()

            If ds_ScreeningTemplateDtl.Tables(0).Rows.Count > 0 Then
                If Not objLambda.Insert_ScreeningTemplateHdrDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_ScreeningTemplateDtl, estr) Then
                    Throw New Exception("Error While Saving Attribute Details In Project Specific Screening" + estr)
                End If

            End If

            Return "True"
        Catch ex As Exception
            Return "Error While Saving Attribute Details In MedexTemplateDtl"
        End Try
    End Function



    <Web.Services.WebMethod()> _
    Public Shared Function SaveAttributeInMedExWorkSpaceDtl(ByVal MedExCode As String, ByVal nMedExWorkSpaceHdrNo As String, ByVal UserId As Integer, ByVal nMedExWorkSpaceDtlNo As String, ByVal vMedExGroupCode As String, _
                                                             ByVal BulkAttribute As String) As String
        Dim objcommon As New clsCommon
        Dim objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()
        Dim ds_Save As New DataSet
        Dim estr As String = String.Empty
        Dim dt As New DataTable

        Try
            dt.Columns.Add("vMedExCode")
            dt.Columns.Add("nMedExWorkSpaceHdrNo")
            dt.Columns.Add("nMedExWorkSpaceDtlNo")
            dt.Columns.Add("iModifyBy")
            dt.Columns.Add("DATAMODE")
            dt.AcceptChanges()
            ds_Save.Tables.Add(dt)
            ds_Save.AcceptChanges()

            For Each MedExCodeTemp In BulkAttribute.Split(",")
                Dim dr As DataRow = ds_Save.Tables(0).NewRow
                dr("vMedExCode") = MedExCodeTemp
                dr("nMedExWorkSpaceHdrNo") = nMedExWorkSpaceHdrNo
                dr("iModifyBy") = UserId
                dr("nMedExWorkSpaceDtlNo") = ""
                dr("DATAMODE") = 2
                ds_Save.Tables(0).Rows.Add(dr)
            Next
            ds_Save.Tables(0).TableName = "Insert_AttributeMedExWorkSpaceDtl"
            ds_Save.Tables(0).AcceptChanges()

            If ds_Save.Tables(0).Rows.Count > 0 Then
                If Not objLambda.TableInsert(ds_Save, UserId, estr) Then
                    Return "False"
                End If
            End If
            Return "True"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function SaveAttributeInWorkspaceScreeningDtl(ByVal MedExCode As String, ByVal nWorkSpaceScreeningHdrNo As String, ByVal UserId As Integer, ByVal nWorkSpaceScreeningDtlNo As String, ByVal vMedExGroupCode As String, _
                                                                 ByVal BulkAttribute As String) As String
        Dim objcommon As New clsCommon
        Dim objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()
        Dim ds_Save As New DataSet
        Dim estr As String = String.Empty
        Dim dt As New DataTable

        Try
            dt.Columns.Add("vMedExCode")
            dt.Columns.Add("nWorkSpaceScreeningHdrNo")
            dt.Columns.Add("nWorkSpaceScreeningDtlNo")
            dt.Columns.Add("iModifyBy")
            dt.Columns.Add("DATAMODE")
            dt.AcceptChanges()
            ds_Save.Tables.Add(dt)
            ds_Save.AcceptChanges()

            For Each MedExCodeTemp In BulkAttribute.Split(",")
                Dim dr As DataRow = ds_Save.Tables(0).NewRow
                dr("vMedExCode") = MedExCodeTemp
                dr("nWorkSpaceScreeningHdrNo") = nWorkSpaceScreeningHdrNo
                dr("iModifyBy") = UserId
                dr("nWorkSpaceScreeningDtlNo") = ""
                dr("DATAMODE") = 2
                ds_Save.Tables(0).Rows.Add(dr)
            Next
            ds_Save.Tables(0).TableName = "Insert_AttributeInWorkspaceScreeningDtl"
            ds_Save.Tables(0).AcceptChanges()

            If ds_Save.Tables(0).Rows.Count > 0 Then
                If Not objLambda.TableInsert(ds_Save, UserId, estr) Then
                    Return "False"
                End If
            End If
            Return "True"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function SaveAttributeInScreeningTemplateDtl(ByVal MedExCode As String, ByVal nScreeningTemplateHdrNo As String, ByVal UserId As Integer, ByVal vMedExGroupCode As String, _
                                                                ByVal BulkAttribute As String) As String
        Dim objcommon As New clsCommon
        Dim objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()
        Dim ds_Save As New DataSet
        Dim estr As String = String.Empty
        Dim dt As New DataTable

        Try
            dt.Columns.Add("vMedExCode")
            dt.Columns.Add("nScreeningTemplateHdrNo")
            dt.Columns.Add("nScreeningTemplateDtlNo")
            dt.Columns.Add("iModifyBy")
            dt.Columns.Add("DATAMODE")
            dt.AcceptChanges()
            ds_Save.Tables.Add(dt)
            ds_Save.AcceptChanges()

            For Each MedExCodeTemp In BulkAttribute.Split(",")
                Dim dr As DataRow = ds_Save.Tables(0).NewRow
                dr("vMedExCode") = MedExCodeTemp
                dr("nScreeningTemplateHdrNo") = nScreeningTemplateHdrNo
                dr("iModifyBy") = UserId
                dr("nScreeningTemplateDtlNo") = ""
                dr("DATAMODE") = 2
                ds_Save.Tables(0).Rows.Add(dr)
            Next
            ds_Save.Tables(0).TableName = "Insert_AttributeInScreeningTemplateDtl"
            ds_Save.Tables(0).AcceptChanges()

            If ds_Save.Tables(0).Rows.Count > 0 Then
                If Not objLambda.TableInsert(ds_Save, UserId, estr) Then
                    Return "False"
                End If
            End If
            Return "True"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function



    <Web.Services.WebMethod()> _
    Public Shared Function DeleteAttributeInMedExWorkSpaceDtl(ByVal nMedExWorkSpaceHdrNo As String, ByVal UserId As Integer, ByVal nMedExWorkSpaceDtlNo As String) As String
        Dim objcommon As New clsCommon
        Dim objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
        Dim ds_MedExWorkSpaceDtl As New DataSet
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim dt As New DataTable
        Try

            wstr = "nMedExWorkSpaceDtlNo IN( " + nMedExWorkSpaceDtlNo + ")"
            If Not objhelpDb.GetMedExWorkSpaceDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_MedExWorkSpaceDtl, estr) Then
                Return "Error While Data Fetching."
            End If

            For Each dr As DataRow In ds_MedExWorkSpaceDtl.Tables(0).Rows
                dr.Item("cStatusIndi") = "D"
                dr.Item("iModifyBy") = UserId
            Next
            If ds_MedExWorkSpaceDtl.Tables(0).Rows.Count > 0 Then
                If Not objLambda.Save_MedExWorkspaceDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                        WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_MedExWorkspaceDtl, ds_MedExWorkSpaceDtl, UserId, estr) Then
                    Return "Error While Data Saving."
                    Exit Function
                End If

            End If

            Return "True"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function DeleteAttributeInWorkspaceScreeningDtl(ByVal nWorkSpaceScreeningHdrNo As String, ByVal UserId As Integer, ByVal nWorkSpaceScreeningDtlNo As String) As String
        Dim objcommon As New clsCommon
        Dim objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
        Dim ds_WorkspaceScreeningDtl As New DataSet
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim dt As New DataTable
        Try

            wstr = "nWorkSpaceScreeningDtlNo IN( " + nWorkSpaceScreeningDtlNo + ")"
            If Not objhelpDb.GetWorkspaceScreeningDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_WorkspaceScreeningDtl, estr) Then
                Return "Error While Data Fetching."
            End If

            For Each dr As DataRow In ds_WorkspaceScreeningDtl.Tables(0).Rows
                dr.Item("cStatusIndi") = "D"
                dr.Item("iModifyBy") = UserId
            Next

            If ds_WorkspaceScreeningDtl.Tables(0).Rows.Count > 0 Then
                If Not objLambda.Insert_WorkSpaceScreeningHdrDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete, ds_WorkspaceScreeningDtl, estr) Then
                    Throw New Exception("Error While Saving Attribute Details In Project Specific Screening" + estr)
                End If
            End If


            Return "True"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function DeleteAttributeInScreeningTemplateDtl(ByVal nScreeningTemplateHdrNo As String, ByVal UserId As Integer, ByVal nScreeningTemplateDtlNo As String) As String
        Dim objcommon As New clsCommon
        Dim objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
        Dim ds_ScreeningTemplateDtl As New DataSet
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim dt As New DataTable
        Try

            wstr = "nScreeningTemplateDtlNo IN( " + nScreeningTemplateDtlNo + ")"

            If Not objhelpDb.GetScreeningTemplateDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ScreeningTemplateDtl, estr) Then
                Return "Error While Data Fetching."
            End If

            For Each dr As DataRow In ds_ScreeningTemplateDtl.Tables(0).Rows
                dr.Item("cStatusIndi") = "D"
                dr.Item("iModifyBy") = UserId
            Next

            If ds_ScreeningTemplateDtl.Tables(0).Rows.Count > 0 Then
                If Not objLambda.Insert_ScreeningTemplateHdrDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete, ds_ScreeningTemplateDtl, estr) Then
                    Throw New Exception("Error While Saving Attribute Details In Project Specific Screening" + estr)
                End If
            End If

            Return "True"
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function

#End Region

End Class