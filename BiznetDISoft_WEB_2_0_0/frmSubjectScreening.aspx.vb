
Partial Class frmSubjectScreening
    Inherits System.Web.UI.Page

#Region "Variable Declaration "

    Shared ObjCommon As New clsCommon
    Shared objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()


    Private Const dt_MedEx As String = "Dt_MedEx"
    Private Const VSDtSubMedExMst As String = "Dt_SubMedExMst"
    Private Const VS_Controls As String = "VsControls"
    Private Const VS_dsSubMedex As String = "ds_SubMedex"
    Private Const VS_Choice As String = "Choice"
    Private Const VS_dtMedEx_Fill As String = "dtMedEx_Fill"
    Private Const VS_MedexGroups As String = "MedexGroups"
    Private Const VS_ContinueMode As String = "ContinueMode"
    Private Const VS_ProjectNo As String = "ProjectNo"
    Private Const VS_SubjectID As String = "SubjectID"

    Private Const VS_DtQC As String = "dtQC"

    Private Const Val_AN As String = "AN" '"Alphanumeric"
    Private Const Val_AL As String = "AL" '"Alphabate"
    Private Const Val_NU As String = "NU" '"Numeric"
    Private Const Val_IN As String = "IN" '"Integer"
    Private Const Val_NNI As String = "NNI" '"NotNull Integer"
    Private Const Val_NNU As String = "NNU" '"NotNull Numeric"

    Private Const Valid_XRayMonth As Integer = 6

    Private arrylst As New ArrayList

    Private Const GVCQC_MedExScreeningHdrQCNo As Integer = 0
    Private Const GVCQC_SubjectId As Integer = 1
    Private Const GVCQC_SrNo As Integer = 2
    Private Const GVCQC_Subject As Integer = 3
    Private Const GVCQC_QCComment As Integer = 4
    Private Const GVCQC_QCFlag As Integer = 5
    Private Const GVCQC_QCBy As Integer = 6
    Private Const GVCQC_QCDate As Integer = 7
    Private Const GVCQC_Response As Integer = 8
    Private Const GVCQC_ResponseGivenBy As Integer = 9
    Private Const GVCQC_ResponseGivenOn As Integer = 10
    Private Const GVCQC_LnkResponse As Integer = 11

    Private Const GVAudit_SrNO As Integer = 0
    Private Const GVAudit_ScreenDate As Integer = 1
    Private Const GVAudit_Remark As Integer = 2
    Private Const GVAudit_ModifyBy As Integer = 3
    Private Const GVAudit_Modifyon As Integer = 4
    Private Const GVAudit_MedExScreeningHdrNo As Integer = 5

    Private isScreenDoneToday As Boolean = False
    Private Gender As String = String.Empty
    Private Is_FormulaEnabled As Boolean = False
    Private Is_ComboGlobalDictionary As Boolean = False
    Private isDate As String = String.Empty
    Dim ContinueMode As Integer = 0

#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not GenCall() Then
            Exit Sub
        End If

        '==== added by Megha on 18-5-2012 for view mode of Report
        If Not IsPostBack() Then

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                If (Not Me.Request.QueryString("Type") Is Nothing) Then
                    Me.txtSubject.Visible = False
                    Me.BtnQC.Style.Add("display", "none")
                    Me.lblSuject.Style.Add("display", "none")
                    Me.txtremark.Style.Add("display", "none")
                    Me.lblRemarks.Style.Add("display", "none")
                    Me.rblScreeningDate.Items(0).Selected = True
                    Me.rblScreeningDate_SelectedIndexChanged(sender, e)
                End If
            End If
        Else
            btnContinueWorking_Click(Nothing, Nothing)
            Exit Sub
        End If
        '=====================================

    End Sub

#End Region

#Region "GenCall "

    Private Function GenCall() As Boolean
        Dim sender As Object = Nothing
        Dim e As EventArgs = Nothing

        Try
            If Not IsNothing(Me.Request.QueryString("SubjectId")) AndAlso _
                Me.Request.QueryString("SubjectId").ToString.Trim() <> "" Then
                Me.HSubjectId.Value = Me.Request.QueryString("SubjectId").ToString.Trim()
            End If

            If Not IsNothing(Me.Request.QueryString("ScrHdrNo")) AndAlso _
             Me.Request.QueryString("ScrHdrNo").ToString.Trim() <> "" Then
                Me.HScrNo.Value = Me.Request.QueryString("ScrHdrNo").ToString.Trim()
            End If
            If Not GenCall_Data() Then
                GenCall = False
                Exit Function
            End If

            If Not GenCall_ShowUI() Then
                GenCall = False
                Exit Function
            End If

            Me.BtnQCSaveSend.Visible = False

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                Me.BtnQCSaveSend.Visible = True
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall")
            Return False
        End Try
    End Function

#End Region

#Region "GenCall_Data "

    Private Function GenCall_Data() As Boolean
        Dim ds As New DataSet
        Dim ds_MedExSubjectHdrDtl As New DataSet
        Dim ds_SubMedExMst As New DataSet
        Dim ds_SubMedex As New DataSet
        Dim ds_SubDetail As New DataSet
        Dim estr_retu As String = String.Empty
        Dim strQuery As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim format As String = "dd-MMM-yyyy"
        Dim datetime As New DateTime

        Try

            HfUserName.Value = Me.Session(S_UserName)
            If Not IsNothing(Me.Request.QueryString("mode")) Then
                Me.ViewState(VS_Choice) = CType(Me.Request.QueryString("mode"), WS_Lambda.DataObjOpenSaveModeEnum)
            End If

            If Not IsNothing(Me.Request.QueryString("SubjectId")) AndAlso _
                        (Me.Request.QueryString("SubjectId").ToString.Trim() <> "") Then
                Wstr = "vSubjectId='" & Me.HSubjectId.Value.Trim() & "'"
                If Not objHelp.GetView_SubjectMaster(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                    ds_SubDetail, estr_retu) Then
                    Return False
                End If

                Me.txtSubject.Text = ds_SubDetail.Tables(0).Rows(0).Item("FullName").ToString.Trim() + " (" + Me.HSubjectId.Value.Trim() + ")"
                If Me.rblScreeningDate.Items.Count <= 0 Then
                    fillScreeningDates()
                End If
            End If

            If Me.rblScreeningDate.Items.Count > 0 AndAlso Me.rblScreeningDate.SelectedValue.ToUpper.Trim() <> "N" AndAlso _
                                        Me.rblScreeningDate.SelectedIndex <> -1 Then 'For Old

                'Wstr = "cActiveFlag<>'N' and vWorkSpaceId='" + GeneralModule.Pro_Screening + "' and vActivityId='" & GeneralModule.Act_Screening & "' and vSubjectId='" & _

                If Not Me.rblScreeningDate.SelectedValue.Trim() = "" Then
                    
                    Wstr = "cActiveFlag<>'N' and vSubjectId='" & _
                   IIf(Me.HSubjectId.Value.Trim() = "", "0000", Me.HSubjectId.Value.Trim()) & "' And vMedExType <> 'IMPORT' And " & _
                  "  replace(convert(varchar(11),dScreenDate,113),' ','-')='" & CDate(Me.rblScreeningDate.SelectedValue.Trim()).Date.ToString("dd-MMM-yyyy") & "'" & _
                   " Order by vWorkSpaceId,iSeqNo"



                    If Not objHelp.GetViewScreeningTemplateHdrDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                   ds_MedExSubjectHdrDtl, estr_retu) Then
                        Return False
                    End If
                    Me.BtnQC.Visible = True
                    If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View And _
                        ds_MedExSubjectHdrDtl.Tables(0).Rows.Count <= 0 AndAlso Me.HSubjectId.Value.Trim() <> "" Then
                        Me.ObjCommon.ShowAlert("No Screening Has Been Done For This Subject", Me.Page())
                        Me.BtnQC.Visible = False
                        Return False
                    End If

                    Me.ViewState(VS_dtMedEx_Fill) = ds_MedExSubjectHdrDtl.Tables(0)
                    Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit

                    If Not SetControls(CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Copy()) Then
                        Me.ShowErrorMessage("Error in SetControls", "...GenCall_Data")
                        Return False
                    End If
                End If

                '----------------- PSS -------------------------




            ElseIf Me.rblScreeningDate.Items.Count > 0 AndAlso Me.rblScreeningDate.SelectedValue.ToUpper.Trim() = "N" Then 'For New

                If chkScreeningType.SelectedItem Is Nothing Then
                    Me.ObjCommon.ShowAlert("Please Select Screening Type.", Me.Page)
                    Me.rblScreeningDate.ClearSelection()
                    chkScreeningType.Enabled = True
                    Return False
                End If

                If chkScreeningType.Items(1).Selected Then
                    If HProjectId.Value.ToString = "" Or txtproject.Text.Trim = "" Then
                        Me.ObjCommon.ShowAlert("Please Select Project for Project Specific Screening.", Me.Page)
                        chkScreeningType.Enabled = True
                        Me.rblScreeningDate.ClearSelection()
                        Return False
                    Else
                        'Wstr = "vWorkSpaceId='" + HProjectId.Value.ToString + "' AND vSubjectID='" + HSubjectId.Value.ToString.Trim() + "' AND cStatusIndi <>'" + Status_Delete + "'"
                        'If Not objHelp.GetMedExWorkSpaceScreeningHdr(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr_retu) Then
                        '    Return False
                        'End If
                        'If ds.Tables(0).Rows.Count > 0 Then
                        '    Me.ObjCommon.ShowAlert("Screening Of This Subject Is Already Saved For This Project.", Me.Page)
                        '    Return False
                        'End If

                        Wstr = "vWorkSpaceId='" + HProjectId.Value.ToString + "' AND cStatusIndi <>'" + Status_Delete + "'"
                        If Not objHelp.GetWorkspaceScreeningHdr(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr_retu) Then
                            Return False
                        End If
                        If ds.Tables(0).Rows.Count <= 0 Then
                            Me.ObjCommon.ShowAlert("No Template Is Attached For This Project. Please Attach Template For This Project.", Me.Page)
                            Return False
                        End If
                    End If
                End If

                Wstr = "cActiveFlag <>'" + ActiveFlag_No + "' AND cStatusIndi <>'" + Status_Delete + "' AND cStatusIndi_Dtl <> '" + Status_Delete + "'  And vMedExType <> 'IMPORT'"
                If chkScreeningType.Items(0).Selected Then
                    Wstr += " AND vWorkSpaceId='" & Me.Request.QueryString("Workspace") & "'" & _
                        " AND vUserLocationCode like'%" + Session(S_LocationCode) + "%'" 'GETDATE()> dFromDate AND GETDATE()<dToDate"
                End If

                If chkScreeningType.Items(1).Selected Then
                    Wstr += IIf(chkScreeningType.Items(0).Selected, " OR ", " AND ") + " vWorkSpaceId = " + HProjectId.Value.ToString
                End If
                Wstr += " Order by vWorkSpaceId,iSeqNo"
                '---------------------------------------------------------------------------

                If Not objHelp.GetViewScreeningTemplateDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr_retu) Then
                    GenCall_Data = False
                    Exit Function
                End If
                If ds.Tables(0).Rows.Count <= 0 Then
                    ObjCommon.ShowAlert("No default template available for this location.", Me.Page)
                    rblScreeningDate.SelectedIndex = -1
                    Return False
                End If

                Me.ViewState(VS_dtMedEx_Fill) = ds.Tables(0)
                'If Me.ViewState(VS_ContinueMode) <> 1 Then
                '    Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
                'End If


            End If

            'Added for QC Comments
            Me.LinkButton1.Visible = True
            If Not IsNothing(Me.Request.QueryString("mode")) AndAlso _
                Me.Request.QueryString("mode").ToString.Trim() = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View
            End If

            If Not IsNothing(Me.Request.QueryString("SubjectId")) AndAlso _
                                 (Me.Request.QueryString("SubjectId").ToString.Trim() <> "") Then
                Me.LinkButton1.Visible = False
            End If

            ds = New DataSet
            If Not objHelp.GetMedExScreeningDtl("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubMedex, estr_retu) Then
                Return False
            End If

            If Not objHelp.GetMedExScreeningHdr("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubMedExMst, estr_retu) Then
                Return False
            End If

            If ds_SubMedex Is Nothing Then
                Throw New Exception(estr_retu)
            End If

            Me.ViewState(VS_dsSubMedex) = ds_SubMedex 'Blank Data Structer for Saveing
            Me.ViewState(VSDtSubMedExMst) = ds_SubMedExMst
            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message.ToString, estr_retu + "...GenCall_Data")
            Return False
        End Try
    End Function

#End Region

#Region "GenCall_ShowUI "

    Private Function GenCall_ShowUI() As Boolean
        Dim dt As New DataTable
        Dim dv As New DataView
        Dim dt_MedExMst As New DataTable
        Dim dr As DataRow
        Dim drGroup As DataRow
        Dim objelement As Object
        Dim StrValidation() As String
        Dim StrGroup(1) As String
        Dim StrGroupCodes As String = String.Empty
        Dim StrGroupDesc As String = String.Empty
        Dim dv_MedexGroup As New DataView
        Dim dt_MedexGroup As New DataTable
        Dim i As Integer = 0
        Dim PrevSubGroupCodes As String = String.Empty
        Dim CntSubGroup As Integer = 0
        Dim CountForFemale As Integer = 0
        Dim sender As Object = Nothing
        Dim e As EventArgs = Nothing
        Dim dsUser As New DataSet
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim objimage As Object
        Dim Copyright As String = String.Empty
        Dim clientname As String = String.Empty

        Try
            Copyright = System.Configuration.ConfigurationManager.AppSettings("CopyRight")

            Page.Title = " :: Subject Screening :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            Me.CompanyName.Value = Copyright


            Me.lblUserName.Text = Session(S_FirstName).ToString.Trim() + " " + Session(S_LastName).ToString.Trim()
            'Me.lblTime.Text = Session(S_Time).ToString '+ " IST (+5.5 GMT)"
            Me.lblTime.Text = Convert.ToString(CDate(Session(S_Time)).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)

            Me.AutoCompleteExtender1.ContextKey = Session(S_LocationCode)
            Me.AutoCompleteExtenderWorkSpace.ContextKey = "iUserId = " & Me.Session(S_UserID)
            PlaceMedEx.Controls.Clear()
            '=========added on 13-nov-2009--=====

            ShowHideControls("S")
            Me.divAudit.Style.Add("display", "none")
            Me.btnDeleteScreenNo.Style.Add("display", "none")
            Me.btnRmkHistory.Style.Add("display", "none")
            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                Me.btnRmkHistory.Style.Add("display", "block")

            ElseIf Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                If Me.rblScreeningDate.SelectedIndex > 0 Then
                    Me.btnRmkHistory.Style.Add("display", "block")
                End If
            End If

            If Me.rblScreeningDate.SelectedIndex <= -1 Or IsNothing(Me.ViewState(VS_dtMedEx_Fill)) Then
                ShowHideControls("H")
                Return True
            End If


            If CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Rows.Count < 1 Then
                ShowHideControls("H")
                If Not IsPostBack Then
                    Me.ObjCommon.ShowAlert("No Attribute is Attached with Screening. So, please Attach Attribute to Screening.", Me.Page)
                End If
                Return False
            End If

            dv_MedexGroup = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).DefaultView
            StrGroup(0) = "vMedExGroupCode"
            StrGroup(1) = "vMedExGroupDesc"
            dt_MedexGroup = dv_MedexGroup.ToTable(True, StrGroup)
            PlaceMedEx.Controls.Add(New LiteralControl("<Table  width=""100%"">"))

            'For Tab Buttons
            For Each drGroup In dt_MedexGroup.Rows
                If StrGroupCodes <> "" Then
                    StrGroupCodes += ",Div" + drGroup("vMedExGroupCode")
                    StrGroupDesc += "," + drGroup("vMedExGroupDesc")
                Else
                    StrGroupCodes = "Div" + drGroup("vMedExGroupCode")
                    StrGroupDesc = "" + drGroup("vMedExGroupDesc")
                End If
            Next drGroup

            Me.ViewState(VS_MedexGroups) = StrGroupCodes.Trim()

            PlaceMedEx.Controls.Add(New LiteralControl("<Tr ALIGN=LEFT>"))
            PlaceMedEx.Controls.Add(New LiteralControl("<Td width=""100%"" white-space: nowrap;>"))
            Me.GetButtons(StrGroupDesc, StrGroupCodes)

            'Added on 27-Jul-2009
            Me.BtnNext.OnClientClick = "return Next('" & StrGroupCodes & "');"
            Me.BtnPrevious.OnClientClick = "return Previous('" & StrGroupCodes & "');"
            '*******************************
            '====================================================

            PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
            PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
            '**********************************

            PlaceMedEx.Controls.Add(New LiteralControl("<Tr ALIGN=LEFT >"))
            PlaceMedEx.Controls.Add(New LiteralControl("<Td>"))

            For Each drGroup In dt_MedexGroup.Rows
                dt = New DataTable
                dt_MedExMst = New DataTable
                dt = Me.ViewState(VS_dtMedEx_Fill)
                dv = New DataView
                dv = dt.DefaultView

                If drGroup.Item("vMedExGroupCode") = "00037" Then           'Problem has come from this End
                    CountForFemale += 1
                End If

                dv.RowFilter = "vMedExGroupCode='" & drGroup("vMedExGroupCode").ToString.Trim() & "'"
                dt_MedExMst = dv.ToTable()

                If i = 0 Then
                    PlaceMedEx.Controls.Add(New LiteralControl("<Div id='Div" & drGroup("vMedExGroupCode").ToString.Trim() & "' style=""display:block""" + _
                    IIf(CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View, _
                        " disabled = ""true"" >", " >"))) 'Added for QC Comments on 22-May-2009
                    'Added on 27-Jul-2009
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "CurrentTab", "currTab='Div" & drGroup("vMedExGroupCode").ToString.Trim() & "';", True)
                    '***********************
                Else
                    PlaceMedEx.Controls.Add(New LiteralControl("<Div id='Div" & drGroup("vMedExGroupCode").ToString.Trim() & "' style=""display:none""" + _
                    IIf(CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View, _
                        " disabled = ""true"" >", " >"))) 'Added for QC Comments on 22-May-2009
                End If
                PlaceMedEx.Controls.Add(New LiteralControl("<Table width=""100%"" cellspacing=""5px"" style=""border: solid 1px gray"">")) ' border=""1""

                For Each dr In dt_MedExMst.Rows
                    ''Added By Dharmesh H.Salla on 29-Apr-2011
                    If Not CountForFemale = 0 Then
                        HfMaleCount.Value = dt_MedExMst.Rows.Count
                        CountForFemale = 0
                    End If
                    '''''''''''''''
                    If PrevSubGroupCodes = "" Or PrevSubGroupCodes <> dr("vMedExSubGroupCode") Then
                        If dr("vMedExSubGroupCode").ToString <> "0000" Then
                            PlaceMedEx.Controls.Add(New LiteralControl("<tr>"))
                            PlaceMedEx.Controls.Add(New LiteralControl("<td>"))
                            PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
                            PlaceMedEx.Controls.Add(New LiteralControl("</td>"))
                            PlaceMedEx.Controls.Add(New LiteralControl("</tr>"))
                            PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""99%"" ALIGN=LEFT style=""BACKGROUND-COLOR: #ffcc66"">"))
                            PlaceMedEx.Controls.Add(New LiteralControl("<Td colspan=""2"" style=""vertical-align:middle"">"))
                            PlaceMedEx.Controls.Add(Getlable(dr("vMedExSubGroupDesc").ToString.Trim(), dr("vMedExSubGroupCode") + CntSubGroup.ToString.Trim()))
                            CntSubGroup += 1
                            PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                            PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
                        End If
                    End If
                    If dr("vMedExType").ToString.ToUpper.Trim() = "LABEL" Then
                        PlaceMedEx.Controls.Add(New LiteralControl("<Tr ALIGN=LEFT width=""99%"">"))
                        PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""vertical-align:middle;;"" colspan=""2"">"))
                        PrevSubGroupCodes = dr("vMedExSubGroupCode")
                        'PlaceMedEx.Controls.Add(GetlableWithHistory(dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), dr("cScreeningType")))
                        'PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                    Else
                        PlaceMedEx.Controls.Add(New LiteralControl("<Tr ALIGN=LEFT width=""99%"">"))
                        PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""vertical-align:middle;width:50%;"">"))
                        PrevSubGroupCodes = dr("vMedExSubGroupCode")
                        PlaceMedEx.Controls.Add(GetlableWithHistory(dr("vMedExDesc") & ": ", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), dr("cScreeningType")))
                        PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                        PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""vertical-align:middle;width:50%;"">"))
                        'Else
                        '    '********************************
                    End If

                    If Not (dr("vValidationType") Is System.DBNull.Value) Then

                        If dr("vValidationType") <> "" And dr("vValidationType") <> "NA" Then
                            StrValidation = dr("vValidationType").ToString.Trim().Split(",")
                            HFNumericScale.Value = 0
                            If StrValidation.Length > 2 Then
                                HFNumericScale.Value = StrValidation(2).ToString()
                            End If

                            If StrValidation.Length > 1 Then
                                objelement = GetObject(dr("vMedExType"), dr("vMedExCode"), dr("vMedExValues"), IIf(dr("vDefaultValue") Is System.DBNull.Value, "", dr("vDefaultValue")), dr("cScreeningType"), _
                                                        StrValidation(0).ToString.Trim(), StrValidation(1).ToString.Trim(), IIf(dr("vAlertonvalue") Is System.DBNull.Value, "", dr("vAlertonvalue")), _
                                                        IIf(dr("vAlertMessage") Is System.DBNull.Value, "", dr("vAlertMessage")), IIf(dr("vHighRange") Is System.DBNull.Value, "0", dr("vHighRange")), _
                                                        IIf(dr("vLowRange") Is System.DBNull.Value, "0", dr("vLowRange")), _
                                                        IIf(dr("vRefTable") Is System.DBNull.Value, "", dr("vRefTable")), _
                                                        IIf(dr("vRefColumn") Is System.DBNull.Value, "", dr("vRefColumn")), HFNumericScale.Value)
                            Else
                                objelement = GetObject(dr("vMedExType"), dr("vMedExCode"), dr("vMedExValues"), IIf(dr("vDefaultValue") Is System.DBNull.Value, "", dr("vDefaultValue")), dr("cScreeningType"), _
                                                        StrValidation(0).ToString.Trim(), "", IIf(dr("vAlertonvalue") Is System.DBNull.Value, "", dr("vAlertonvalue")), _
                                                        IIf(dr("vAlertMessage") Is System.DBNull.Value, "", dr("vAlertMessage")), IIf(dr("vHighRange") Is System.DBNull.Value, "0", dr("vHighRange")), _
                                                        IIf(dr("vLowRange") Is System.DBNull.Value, "0", dr("vLowRange")), _
                                                        IIf(dr("vRefTable") Is System.DBNull.Value, "", dr("vRefTable")), _
                                                        IIf(dr("vRefColumn") Is System.DBNull.Value, "", dr("vRefColumn")), HFNumericScale.Value)
                            End If

                        Else
                            objelement = GetObject(dr("vMedExType"), dr("vMedExCode"), dr("vMedExValues"), IIf(dr("vDefaultValue") Is System.DBNull.Value, "", dr("vDefaultValue")), dr("cScreeningType"), _
                                                     "", "", IIf(dr("vAlertonvalue") Is System.DBNull.Value, "", dr("vAlertonvalue")), _
                                                        IIf(dr("vAlertMessage") Is System.DBNull.Value, "", dr("vAlertMessage")), IIf(dr("vHighRange") Is System.DBNull.Value, "0", dr("vHighRange")), _
                                                        IIf(dr("vLowRange") Is System.DBNull.Value, "0", dr("vLowRange")), _
                                                        IIf(dr("vRefTable") Is System.DBNull.Value, "", dr("vRefTable")), _
                                                        IIf(dr("vRefColumn") Is System.DBNull.Value, "", dr("vRefColumn")), HFNumericScale.Value)
                        End If
                    Else
                        objelement = GetObject(dr("vMedExType"), dr("vMedExCode"), dr("vMedExValues"), IIf(dr("vDefaultValue") Is System.DBNull.Value, "", dr("vDefaultValue")), dr("cScreeningType"), _
                                                    "", "", IIf(dr("vAlertonvalue") Is System.DBNull.Value, "", dr("vAlertonvalue")), _
                                                        IIf(dr("vAlertMessage") Is System.DBNull.Value, "", dr("vAlertMessage")), IIf(dr("vHighRange") Is System.DBNull.Value, "0", dr("vHighRange")), _
                                                        IIf(dr("vLowRange") Is System.DBNull.Value, "0", dr("vLowRange")), _
                                                        IIf(dr("vRefTable") Is System.DBNull.Value, "", dr("vRefTable")), _
                                                        IIf(dr("vRefColumn") Is System.DBNull.Value, "", dr("vRefColumn")), HFNumericScale.Value)
                    End If
                    PlaceMedEx.Controls.Add(objelement)

                    ''For File Type*************
                    If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                        If dr("vMedExType").ToString.ToUpper.Trim() = "FILE" AndAlso dr("vDefaultValue").ToString.Trim() <> "" Then
                            Dim HLnkFile As New HyperLink

                            HLnkFile.ID = "hlnk" + dr("vMedExCode").ToString.Trim()
                            HLnkFile.NavigateUrl = dr("vDefaultValue").ToString.Trim()
                            HLnkFile.Text = Path.GetFileName(dr("vDefaultValue").ToString.Trim())
                            HLnkFile.Target = "_blank"
                            PlaceMedEx.Controls.Add(HLnkFile)
                        End If
                    End If

                    'For Print UOM 
                    If Not dr("vUOM") Is System.DBNull.Value AndAlso dr("vUOM") <> "" Then
                        PlaceMedEx.Controls.Add(Getlable(dr("vUOM"), "UOM" + dr("vMedExCode")))
                    End If
                    '********************************
                    'for calendar image
                    If dr("vMedExType").ToString.ToUpper.Trim() = "DATETIME" Then
                        objimage = GetDateImage(dr("vMedExCode"), objelement)
                    End If

                    If Is_FormulaEnabled Then
                        If Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = WorkFlowStageId_DataEntry Or _
                                    Convert.ToString(Me.Session(S_WorkFlowStageId)).Trim() = WorkFlowStageId_FirstReview Then
                            PlaceMedEx.Controls.Add(GetAutoCalculateButton("Calc", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), dr("vMedExFormula"), dr("vMedexType")))
                            Is_FormulaEnabled = False
                        End If
                    End If


                    PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
                Next dr
                i += 1
                PlaceMedEx.Controls.Add(New LiteralControl("</Table>")) '</Tr>
                PlaceMedEx.Controls.Add(New LiteralControl("</Div>")) '</Tr>
            Next drGroup

            PlaceMedEx.Controls.Add(New LiteralControl("</Td>")) '</Tr>
            PlaceMedEx.Controls.Add(New LiteralControl("</Tr>")) '</Tr>
            PlaceMedEx.Controls.Add(New LiteralControl("</Table>"))
            'Added for QC comments on 23-May-2009
            Me.BtnSave.Visible = True
            Me.btnContinueSave.Visible = True

            Me.Session("PlaceMedEx") = PlaceMedEx.Controls

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                Me.BtnSave.Visible = False
                Me.btnContinueSave.Visible = False
            End If
            '************************************

            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message.ToString, "...GenCall_ShowUI")
            Return False
        End Try
    End Function

    Private Function ShowHideControls(ByVal Type As String) As Boolean

        If Type.ToUpper.Trim() = "H" Then
            Me.BtnQC.Visible = False
            Me.BtnPrevious.Visible = False
            Me.BtnNext.Visible = False
            Me.BtnSave.Visible = False
            Me.btnContinueSave.Visible = False
            Me.txtremark.Visible = False
            Me.lblRemarks.Visible = False
        ElseIf Type.ToUpper.Trim() = "S" Then
            Me.BtnQC.Visible = True
            Me.BtnPrevious.Visible = True
            Me.BtnNext.Visible = True
            Me.BtnSave.Visible = True
            Me.btnContinueSave.Visible = True
            Me.txtremark.Visible = True
            Me.lblRemarks.Visible = True
        End If

        Return True
    End Function

    Private Function SetControls(ByVal dt_screening As DataTable) As Boolean
        Dim dv As New DataView
        Try

            chkScreeningType.ClearSelection()

            ' Set control based on selection
            dt_screening.DefaultView.RowFilter = "cScreeningType= 'D'"
            If dt_screening.DefaultView.ToTable.Rows.Count > 0 Then
                chkScreeningType.Items(0).Selected = True
            End If
            dt_screening.DefaultView.RowFilter = ""
            dt_screening.DefaultView.RowFilter = "cScreeningType= 'P'"
            If dt_screening.DefaultView.ToTable.Rows.Count > 0 And Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                chkScreeningType.Items(1).Selected = True
                tr_WorkSpace.Style("display") = "block"
                HProjectId.Value = dt_screening.DefaultView.ToTable.Rows(0)("vWorkSpaceId").ToString
                txtproject.Text = "[" + dt_screening.DefaultView.ToTable.Rows(0)("vProjectNo").ToString + "] " + dt_screening.DefaultView.ToTable.Rows(0)("vRequestId").ToString
                dt_screening.DefaultView.RowFilter = ""
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...SetControls")
            Return False
        End Try
    End Function

#End Region

#Region "GetLiteral"

    Private Function GetLiteral(ByVal text As String) As Literal
        Dim Lit As New Literal
        Lit.Text = text
        GetLiteral = Lit
    End Function

#End Region

#Region "Getlable"

    Private Function Getlable(ByVal vlabelName As String, ByVal Id As String) As Label
        Dim lab As New Label
        lab.ID = "Lab" & Id
        lab.Text = vlabelName.Trim()
        lab.SkinID = "lblDisplay"
        Getlable = lab
    End Function

    Private Function GetlableWithHistory(ByVal vlabelName As String, ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal MedExCode As String, ByVal ScreeningType As String) As LinkButton
        Dim lnk As New LinkButton
        lnk.ID = "Lnk" & MedExGroupCode + MedExSubGroupCode + MedExCode
        lnk.Text = vlabelName.Trim()
        lnk.SkinID = "lblDisplay"
        lnk.OnClientClick = "return HistoryDivShowHide('S','" + MedExCode + "','','','" + ScreeningType + "');"
        GetlableWithHistory = lnk
    End Function

#End Region

#Region "GetButtons"

    Private Function GetButtons(ByVal BtnName As String, ByVal Id As String) As Boolean
        Dim Btn As Button
        Dim index As Integer
        Dim StrGroupCode_arry() As String
        Dim StrGroupDesc_arry() As String
        Dim dtMedExMst As New DataTable
        Dim dvMedExMst As New DataView

        Try
            StrGroupCode_arry = Id.Split(",")
            StrGroupDesc_arry = BtnName.Split(",")
            For index = 0 To StrGroupCode_arry.Length - 1
                Btn = New Button
                Btn.ID = "Btn" & StrGroupCode_arry(index)
                Btn.Text = " " & StrGroupDesc_arry(index).Trim() & " "
                Btn.Style.Add("color", "#FFffff")
                Btn.Font.Bold = True
                Btn.Style.Add("BACKGROUND-IMAGE", "url(images/btn.jpg)")
                Btn.Style.Add("background-repeat", "no-repeat")
                Btn.Style.Add("background-color", "rgb(174,77,2)")
                If index = 0 Then
                    Btn.Style.Add("color", "#FFC300")
                End If
                Btn.BorderStyle = BorderStyle.None

                'this is to add event on perticuler Dynamic controls
                'AddHandler Btn.Click, AddressOf Me.BtnAdd_Click
                '***************************************************
                Btn.OnClientClick = "return DisplayDiv('" & StrGroupCode_arry(index) & "','" & Id & "');"

                If Btn.Text.ToUpper() = MedExGroupDescForFemale.ToUpper() Then
                    Me.hfMedExGroupCode.Value = Btn.ID.Trim()
                    dtMedExMst = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable)
                    dvMedExMst = dtMedExMst.DefaultView
                    dvMedExMst.RowFilter = "vMedExCode = '" + MedExCodeForSex + "'"

                    If dvMedExMst.ToTable().Rows.Count > 0 Then
                        If Convert.ToString(dvMedExMst.ToTable().Rows(0)("vDefaultValue")).Trim.ToUpper() = MedExResultForMale.ToUpper() Then
                            Btn.Text = MedExGroupDescForMale
                            Btn.Attributes.Add("disabled", "true")
                        End If
                    End If
                End If
                PlaceMedEx.Controls.Add(Btn)
            Next
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GetButtons")
            Return False
        End Try
    End Function

#End Region

#Region "GetObject"

    Private Function GetObject(ByVal vFieldType As String, ByVal Id As String, ByVal vMedExValues As String, _
                                ByVal dtValues As String, ByVal ScreeningType As String, Optional ByVal Validation As String = "", _
                                Optional ByVal length As String = "", Optional ByVal AlertonValue As String = "", _
                                Optional ByVal AlertMsg As String = "", _
                                Optional ByVal HighRange As String = "0", _
                                Optional ByVal LowRange As String = "0", _
                                Optional ByVal RefTable As String = "", _
                                Optional ByVal RefColumn As String = "", _
                                Optional ByVal NumericScale As String = "0") As Object
        Dim txt As TextBox
        Dim ddl As DropDownList
        Dim FileBro As FileUpload
        Dim RBL As RadioButtonList
        Dim CBL As CheckBoxList
        Dim lbl As Label
        Dim ds_SubjectMst As New DataSet
        Dim wStr As String = String.Empty
        Dim birthDate As Date

        Select Case vFieldType.ToUpper.Trim()
            Case "TEXTBOX"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                'txt.CssClass = "Required"
                txt.CssClass = ScreeningType + " Required textBox"
                txt.Text = dtValues
                'For Validation Only ****************
                If length <> "" And length <> "0" Then
                    'txt.MaxLength = length
                    If Validation = "NU" Then
                        txt.MaxLength = length + 1
                    Else
                        txt.MaxLength = length
                    End If
                End If

                HighRange = IIf(HighRange = String.Empty, "0", HighRange)
                LowRange = IIf(LowRange = "", "0", LowRange)

                If Validation = "" Or Validation = "NA" Then
                    txt.Attributes.Add("onblur", "ValidateTextbox(0,this,'No Validation'," & HighRange & "," & LowRange & ");")

                ElseIf Validation = GeneralModule.Val_AN Then
                    txt.Attributes.Add("onblur", "ValidateTextbox(" & GeneralModule.Validation_Alphanumeric & ",this,'Please Enter AlphaNumeric Value');")

                ElseIf Validation = Val_NU Then
                    'txt.Attributes.Add("onblur", "ValidateTextbox(" & GeneralModule.Validation_Numeric & ",this,'Please Enter Numeric or Blank Value');")
                    txt.Attributes.Add("onblur", "ValidateTextboxNumeric(" & GeneralModule.Validation_Numeric & ",this,'Please Enter Numeric or Blank Value','" & HighRange & "','" & LowRange & "','" & Validation & "' , '" & length & "' , '" & NumericScale & "');")

                ElseIf Validation = Val_IN Then
                    txt.Attributes.Add("onblur", "ValidateTextbox(" & GeneralModule.Validation_Integer & ",this,'Please Enter Integer or Blank Value');")

                ElseIf Validation = Val_AL Then
                    txt.Attributes.Add("onblur", "ValidateTextbox('" & GeneralModule.Validation_Alphabet & "',this,'Please Enter Alphabets only');")

                ElseIf Validation = Val_NNI Then
                    txt.Attributes.Add("onblur", "ValidateTextbox('" & GeneralModule.Validation_NotNullInteger & "',this,'Please Enter Integer Value');")

                ElseIf Validation = Val_NNU Then
                    txt.Attributes.Add("onblur", "ValidateTextbox('" & GeneralModule.Validation_NotNullNumeric & "',this,'Please Enter Numeric Value');")
                End If

                'Added for QC Comments on 22-May-2009
                If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    txt.Enabled = False
                End If
                '***********************
                If Id = GeneralModule.Medex_Weight Then
                    txt.Attributes.Add("onblur", "FillBMIValue('" & GeneralModule.Medex_Height.Trim() & "','" & GeneralModule.Medex_Weight.Trim() & "','" & GeneralModule.Medex_BMI.Trim() & "');")

                ElseIf Id = GeneralModule.Medex_Temperature_F Then
                    txt.Attributes.Add("onblur", "F2C('" & GeneralModule.Medex_Temperature_F.Trim() & "','" & GeneralModule.Medex_Temperature_C.Trim() & "'," & HighRange & "," & LowRange & ");")
                    Me.HFFerenhitToCelcius.Value = HighRange + "##" + LowRange

                ElseIf Id = GeneralModule.Medex_Temperature_C Then
                    'txt.Attributes.Add("ReadOnly", "true")
                    txt.Attributes.Add("onblur", "C2F('" & GeneralModule.Medex_Temperature_C.Trim() & "','" & GeneralModule.Medex_Temperature_F.Trim() & "'," & HighRange & "," & LowRange & ");")

                ElseIf Id = GeneralModule.Medex_Eligibilitydeclaredby Then
                    txt.Attributes.Add("ReadOnly", "true")
                    txt.Attributes.Add("onchange", "SetEligibilitydeclaredon('" & GeneralModule.Medex_Eligibilitydeclaredby.Trim() & "','" & GeneralModule.Medex_Eligibilitydeclaredon.Trim() & "');")

                ElseIf Id = GeneralModule.Medex_RecordedBy_BMI Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_PI_Co_I_Designate Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_Physician Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_RecordedBy Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_RecordedBy_ECG Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_RecordedBy_Xray Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_RecordedBy_Lab Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_Eligibilitydeclaredon Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_PICommentsgivenon Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_BMI Then
                    txt.Attributes.Add("ReadOnly", "true")

                End If
                '************************************
                GetObject = txt

            Case "COMBOBOX"
                Dim Arrvalue() As String = Nothing
                Dim i As Integer
                Dim ds_ddl As New DataSet
                Dim estr As String = String.Empty

                ddl = New DropDownList
                ddl.ID = Id
                'ddl.CssClass = "dropDownList"
                ddl.CssClass = ScreeningType + " Required dropDownList"

                If RefTable.Trim() <> "" And RefColumn.Trim() <> "" Then

                    If Not objHelp.GetFieldsOfTable(RefTable.Trim(), RefColumn.Trim(), "", ds_ddl, estr) Then
                        Me.ObjCommon.ShowAlert(estr, Me.Page())
                        Return Nothing
                    End If
                    ds_ddl.Tables(0).DefaultView.Sort = RefColumn
                    ddl.DataSource = ds_ddl.Tables(0).DefaultView.ToTable()
                    ddl.DataTextField = RefColumn
                    ddl.DataValueField = RefColumn
                    ddl.DataBind()

                Else
                    Arrvalue = Split(vMedExValues, ",")
                    For i = 0 To Arrvalue.Length - 1
                        ddl.Items.Add(New ListItem(Arrvalue(i), Arrvalue(i)))
                    Next
                    If Not dtValues = "" Then
                        ddl.SelectedItem.Text = dtValues
                        ddl.SelectedItem.Value = dtValues
                    End If

                End If
                'Adde by Naimesh Dave on 28-Nov-2008 for Alert on Value as suggested by Nikur Sir
                If AlertonValue.Trim() <> "" Then
                    ddl.Attributes.Add("onchange", "ddlAlerton('" & ddl.ClientID & "','" & AlertonValue & "','" & AlertMsg & "');")
                End If
                '*********************************

                'Added for QC Comments on 22-May-2009
                If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    ddl.Enabled = False
                End If
                '***********************

                GetObject = ddl
                'PlaceMedEx.Controls.Add(ddl)

            Case "RADIO"
                Dim Arrvalue() As String = Nothing
                Dim i As Integer
                Dim ds_Radio As New DataSet
                Dim estr As String = String.Empty

                RBL = New RadioButtonList
                RBL.ID = Id
                'RBL.AutoPostBack = True
                RBL.EnableViewState = True
                'RBL.CssClass = "textBox"
                RBL.CssClass = ScreeningType + " Required"
                If RefTable.Trim() <> "" And RefColumn.Trim() <> "" Then

                    If Not objHelp.GetFieldsOfTable(RefTable.Trim(), RefColumn.Trim(), "", ds_Radio, estr) Then
                        Me.ObjCommon.ShowAlert(estr, Me.Page())
                        Return Nothing
                    End If

                    ds_Radio.Tables(0).DefaultView.Sort = RefColumn
                    RBL.DataSource = ds_Radio.Tables(0).DefaultView.ToTable()
                    RBL.DataTextField = RefColumn
                    RBL.DataValueField = RefColumn
                    RBL.DataBind()

                Else
                    Arrvalue = Split(vMedExValues, ",")
                    For i = 0 To Arrvalue.Length - 1
                        RBL.Items.Add(New ListItem(Arrvalue(i), Arrvalue(i)))
                    Next

                    If Not dtValues = "" Then
                        RBL.SelectedValue = dtValues
                    End If
                    If Id = MedExCodeForSex Then
                        If dtValues = "" Then

                            wStr = "vSubjectId = '" + Me.HSubjectId.Value.Trim() + "' And cStatusIndi <> 'D'"
                            If Not objHelp.GetSubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                            ds_SubjectMst, estr) Then
                                Me.ObjCommon.ShowAlert("Error While Setting Birth Date: " + estr, Me.Page)
                            End If

                            If ds_SubjectMst.Tables(0).Rows.Count > 0 AndAlso _
                              (Not ds_SubjectMst.Tables(0).Rows(0)("cSex") Is System.DBNull.Value) AndAlso _
                                      ds_SubjectMst.Tables(0).Rows(0)("cSex").ToString.Trim() <> "" Then

                                If ds_SubjectMst.Tables(0).Rows(0)("cSex").ToString.ToUpper.Trim() = "M" Then
                                    Gender = "Male"
                                ElseIf ds_SubjectMst.Tables(0).Rows(0)("cSex").ToString.ToUpper.Trim() = "F" Then
                                    Gender = "Female"
                                End If
                                RBL.SelectedValue = Gender
                            End If
                        End If
                    End If
                End If

                If Convert.ToString(RBL.SelectedValue).Trim.ToUpper() = "MALE" And Me.hfMedExGroupCode.Value.Trim() <> "" Then
                    CType(Me.PlaceMedEx.FindControl(Me.hfMedExGroupCode.Value.Trim()), Button).Text = MedExGroupDescForMale
                    CType(Me.PlaceMedEx.FindControl(Me.hfMedExGroupCode.Value.Trim()), Button).Attributes.Add("disabled", "true")
                ElseIf Convert.ToString(RBL.SelectedValue).Trim.ToUpper() = "FEMALE" And Me.hfMedExGroupCode.Value.Trim() <> "" Then
                    CType(Me.PlaceMedEx.FindControl(Me.hfMedExGroupCode.Value.Trim()), Button).Text = MedExGroupDescForFemale
                    CType(Me.PlaceMedEx.FindControl(Me.hfMedExGroupCode.Value.Trim()), Button).Attributes.Remove("disabled")
                End If

                RBL.RepeatDirection = RepeatDirection.Horizontal
                RBL.RepeatColumns = 4

                'Adde by Naimesh Dave on 28-Nov-2008 for Alert on Value as suggested by Nikur Sir
                If AlertonValue.Trim() <> "" Then
                    RBL.Attributes.Add("onclick", "Alerton('" & RBL.ClientID & "','" & AlertonValue & "','" & AlertMsg & "');")
                    'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "AlertOn", _
                    '                        "Alerton('" & RBL.ClientID & "','" & AlertonValue & "','" & AlertMsg & "');", True)
                End If
                '*********************************

                'Added for QC Comments on 22-May-2009
                If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    RBL.Enabled = False
                End If
                '***********************

                If RBL.ID = MedExCodeForSex Then
                    RBL.Attributes.Add("onclick", "CheckOnlyForFemale('" & RBL.ClientID & "');")
                    Me.hfMedExCodeForSex.Value = MedExCodeForSex

                ElseIf RBL.ID = Medex_ClinicallyFit Then
                    RBL.Attributes.Add("onclick", "JustAlert('" & HfUserName.Value & "','" & Medex_Physician & "');")

                ElseIf RBL.ID = Medex_SubjectFoundEligible Then
                    RBL.Attributes.Add("onclick", "SetEligibilitydeclaredon('" & GeneralModule.Medex_Eligibilitydeclaredby.Trim() & "','" & GeneralModule.Medex_Eligibilitydeclaredon.Trim() & "','" & HfUserName.Value & "');")

                ElseIf RBL.ID = Medex_RecreationlDrug Then
                    RBL.Attributes.Add("onclick", "SetUserName('" & HfUserName.Value & "','" & GeneralModule.Medex_RecordedBy.Trim & "');")

                ElseIf RBL.ID = Medex_Clinically_ECG Then
                    RBL.Attributes.Add("onclick", "SetUserName('" & HfUserName.Value & "','" & GeneralModule.Medex_RecordedBy_ECG.Trim & "');")

                ElseIf RBL.ID = Medex_Clinically_Lab Then
                    RBL.Attributes.Add("onclick", "SetUserName('" & HfUserName.Value & "','" & GeneralModule.Medex_RecordedBy_Lab.Trim & "');")

                ElseIf RBL.ID = Medex_Consent_SCr Then
                    RBL.Attributes.Add("onclick", "SetUserName('" & HfUserName.Value & "','" & GeneralModule.Medex_RecordedBy_BMI.Trim & "');")

                ElseIf RBL.ID = Medex_Clinically_Xray Then
                    RBL.Attributes.Add("onclick", "SetUserName('" & HfUserName.Value & "','" & GeneralModule.Medex_RecordedBy_Xray.Trim & "');")

                End If
                GetObject = RBL

            Case "CHECKBOX"
                Dim Arrvalue() As String = Nothing
                Dim Defvalue() As String = Nothing
                Dim i As Integer
                Dim ds_Check As New DataSet
                Dim estr As String = String.Empty

                CBL = New CheckBoxList
                CBL.ID = Id
                CBL.EnableViewState = True
                CBL.CssClass = ScreeningType + " Required"
                If RefTable.Trim() <> "" And RefColumn.Trim() <> "" Then

                    If Not objHelp.GetFieldsOfTable(RefTable.Trim(), RefColumn.Trim(), "", ds_Check, estr) Then
                        Me.ObjCommon.ShowAlert(estr, Me.Page())
                        Return Nothing
                    End If

                    ds_Check.Tables(0).DefaultView.Sort = RefColumn
                    CBL.DataSource = ds_Check.Tables(0).DefaultView.ToTable()
                    CBL.DataTextField = RefColumn
                    CBL.DataValueField = RefColumn
                    CBL.DataBind()
                Else
                    Arrvalue = Split(vMedExValues, ",")
                    For i = 0 To Arrvalue.Length - 1
                        CBL.Items.Add(New ListItem(Arrvalue(i), Arrvalue(i)))
                    Next
                End If

                CBL.ClearSelection()
                If Not dtValues = "" Then
                    Defvalue = Split(dtValues, ",")
                    For i = 0 To Defvalue.Length - 1
                        For Each item As ListItem In CBL.Items
                            If item.Value.Trim().ToUpper() = Defvalue(i).Trim().ToUpper() Then
                                item.Selected = True
                                Exit For
                            End If
                        Next item
                    Next i
                End If

                CBL.RepeatDirection = RepeatDirection.Horizontal
                CBL.RepeatColumns = 3

                'Added for QC Comments on 22-May-2009
                If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    CBL.Enabled = False
                End If
                '***********************
                GetObject = CBL

            Case "FILE"
                FileBro = New FileUpload
                FileBro.ID = "FU" + Id
                FileBro.EnableViewState = True
                FileBro.CssClass = ScreeningType + " Required textBox"

                'Added for QC Comments on 22-May-2009
                If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    FileBro.Enabled = False
                End If
                '***********************
                GetObject = FileBro

            Case "TEXTAREA"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = ScreeningType + " Required textBox"
                txt.Text = dtValues
                txt.TextMode = TextBoxMode.MultiLine
                txt.Width = 462

                'Added for QC Comments on 22-May-2009
                If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    txt.Enabled = False
                    txt.Rows = Math.Round(txt.Text.ToString.Length / 20)
                End If
                '***********************

                If Id = GeneralModule.Medex_PIComments Then
                    Dim dt_profiles As New DataTable
                    Dim dv_profiles As DataView
                    'added by Megha on 28-6-2012
                    Me.lblSignername.Text = Session(S_FirstName).ToString.Trim() + " " + Session(S_LastName).ToString.Trim()
                    dt_profiles = CType(Me.Session(S_Profiles), DataTable)
                    dv_profiles = dt_profiles.DefaultView
                    dv_profiles.RowFilter = "vUserTypeCode = '" + Me.Session(S_UserType).ToString() + "'"
                    Me.lblSignerDesignation.Text = dv_profiles.ToTable.Rows(0)("vUserTypeName").ToString()
                    txt.Attributes.Add("onChange", "Authentication();")
                    Me.HMedex_Medex_PI_Co_I_Designate.Value = Medex_PI_Co_I_Designate
                    Me.HMedex_Medex_PICommentsgivenon.Value = GeneralModule.Medex_PICommentsgivenon.Trim()
                    ''End If
                    ''========================
                    ''RBL.Style.Add("onclick", "SetEligibilitydeclaredon_1(Eligibilitydeclaredby,Eligibilitydeclaredon,Username);")
                End If

                GetObject = txt

            Case "DATETIME"
                Dim eStr As String = String.Empty
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                'txt.CssClass = "textBox"
                txt.CssClass = ScreeningType + " Required textBox"

                'Commented as per the requirement of Screeining on 06-Oct-2009
                'txt.Text = IIf(dtValues = "", System.DateTime.Now.ToString("dd-MMM-yyyy"), dtValues)
                txt.Text = dtValues
                '**********************************

                If Id = GeneralModule.Medex_DateOfBirth Then
                    '***************************************
                    If dtValues = "" Then
                        wStr = "vSubjectId = '" + Me.HSubjectId.Value.Trim() + "' And cStatusIndi <> 'D'"
                        If Not objHelp.GetSubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        ds_SubjectMst, eStr) Then
                            Me.ObjCommon.ShowAlert("Error While Setting Birth Date: " + eStr, Me.Page)
                        End If

                        If ds_SubjectMst.Tables(0).Rows.Count > 0 Then
                            If Not ds_SubjectMst.Tables(0).Rows(0)("dBirthDate") Is System.DBNull.Value AndAlso _
                            ds_SubjectMst.Tables(0).Rows(0)("dBirthDate").ToString.Trim() <> "" Then
                                birthDate = ds_SubjectMst.Tables(0).Rows(0)("dBirthDate")
                                txt.Text = birthDate.ToString("dd-MMM-yyyy")
                            End If
                        End If
                    End If
                    '****************************************
                    'SDNidhi
                    'txt.Attributes.Add("onblur", "SetAge('" & GeneralModule.Medex_DateOfBirth.Trim() & "','" & GeneralModule.Medex_Age.Trim() & "','" & System.DateTime.Now.ToString("dd-MMM-yyyy") & "');")
                    txt.Attributes.Add("onblur", "SetAge('" & GeneralModule.Medex_DateOfBirth.Trim() & "','" & GeneralModule.Medex_Age.Trim() & "','" & CDate(ObjCommon.GetCurDatetime(Me.Session(S_TimeZoneName))).Date.ToString("dd-MMM-yyyy") & "');")

                ElseIf Id = GeneralModule.Medex_Eligibilitydeclaredon Then
                    txt.Attributes.Add("ReadOnly", "true")
                ElseIf Id = GeneralModule.Medex_PICommentsgivenon Then
                    txt.Attributes.Add("ReadOnly", "true")
                Else
                    'txt.Attributes.Add("OnChange", "DateConvert(this.value,this)")
                    txt.Attributes.Add("OnChange", "DateConvertForScreening(this.value,this)")
                End If

                'Added for QC Comments on 22-May-2009
                If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    txt.Enabled = False
                End If
                '***********************
                GetObject = txt

            Case "TIME"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = ScreeningType + " Required textBox"

                'Commented as per the requirement of Screeining on 06-Oct-2009
                'txt.Text = IIf(dtValues = "", DateTime.Now.ToString("HH:mm:ss"), dtValues)
                txt.Text = dtValues
                '*******************************
                txt.Attributes.Add("onblur", "TimeConvert(this.value,this)")

                'Added for QC Comments on 22-May-2009
                If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    txt.Enabled = False
                End If
                '***********************

                GetObject = txt
                '-----------------------------
            Case "ASYNCDATETIME"
                Dim eStr As String = String.Empty
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = ScreeningType + " Required textBox"

                'Commented as per the requirement of Screeining on 06-Oct-2009
                'txt.Text = IIf(dtValues = "", System.DateTime.Now.ToString("dd-MMM-yyyy"), dtValues)
                txt.Text = dtValues
                '**********************************

                If Id = GeneralModule.Medex_DateOfBirth Then
                    '***************************************
                    If dtValues = "" Then

                        wStr = "vSubjectId = '" + Me.HSubjectId.Value.Trim() + "' And cStatusIndi <> 'D'"
                        If Not objHelp.GetSubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        ds_SubjectMst, eStr) Then
                            Me.ObjCommon.ShowAlert("Error While Setting Birth Date: " + eStr, Me.Page)
                        End If

                        If ds_SubjectMst.Tables(0).Rows.Count > 0 Then
                            If Not ds_SubjectMst.Tables(0).Rows(0)("dBirthDate") Is System.DBNull.Value AndAlso _
                            ds_SubjectMst.Tables(0).Rows(0)("dBirthDate").ToString.Trim() <> "" Then

                                birthDate = ds_SubjectMst.Tables(0).Rows(0)("dBirthDate")
                                txt.Text = birthDate.ToString("dd-MMM-yyyy")
                            End If
                        End If

                    End If
                    '****************************************
                    txt.Attributes.Add("onblur", "SetAge('" & GeneralModule.Medex_DateOfBirth.Trim() & "','" & GeneralModule.Medex_Age.Trim() & "','" & CDate(ObjCommon.GetCurDatetime(Me.Session(S_TimeZoneName))).Date.ToString("dd-MMM-yyyy") & "');")

                ElseIf Id = GeneralModule.Medex_Eligibilitydeclaredon Then
                    txt.Attributes.Add("ReadOnly", "true")
                ElseIf Id = GeneralModule.Medex_PICommentsgivenon Then
                    txt.Attributes.Add("ReadOnly", "true")
                Else
                    ' txt.Attributes.Add("onblur", "DateConvert(this.value,this)")
                    txt.Attributes.Add("onblur", "DateConvertForScreening(this.value,this)")
                End If

                'Added for QC Comments on 22-May-2009
                If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    txt.Enabled = False
                End If
                '***********************
                GetObject = txt

            Case "ASYNCTIME"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = ScreeningType + " Required textBox"

                'Commented as per the requirement of Screeining on 06-Oct-2009
                'txt.Text = IIf(dtValues = "", DateTime.Now.ToString("HH:mm:ss"), dtValues)
                txt.Text = dtValues
                '*******************************
                txt.Attributes.Add("onblur", "TimeConvert(this.value,this)")

                'Added for QC Comments on 22-May-2009
                If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    txt.Enabled = False
                End If
                '***********************
                GetObject = txt
                '--------------------
            Case "LABEL"
                lbl = New Label
                lbl.ID = "lbl" + Id
                lbl.EnableViewState = True
                lbl.CssClass = ScreeningType + " Label"
                lbl.Style.Add("word-wrap", "break-word")
                lbl.Style.Add("white-space", "none")
                'lbl.Style.Add("list-style-type", "square")
                lbl.Text = vMedExValues.Trim()
                lbl.ToolTip = dtValues.Trim()
                GetObject = lbl

            Case "CRFTERM"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = ScreeningType + " textBox"
                txt.Text = dtValues
                GetObject = txt

            Case "FORMULA"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                'txt.CssClass = "textBox"
                'If IsNotNull.ToUpper() = "Y" Then
                txt.CssClass = ScreeningType + " Required textBox"
                'End If
                txt.Text = dtValues
                txt.Attributes.Add("title", dtValues)

                HighRange = IIf(HighRange = "", "0", HighRange)
                LowRange = IIf(LowRange = "", "0", LowRange)

                Dim checktype As String = ""
                Dim msg As String = ""
                If Validation = "" Or Validation = "NA" Then
                    txt.Attributes.Add("onblur", "ValidateTextbox(0,this,'No Validation'," & HighRange & "," & LowRange & ");")

                ElseIf Validation = GeneralModule.Val_AN Then
                    txt.Attributes.Add("onblur", "ValidateTextbox(" & GeneralModule.Validation_Alphanumeric & ",this,'Please Enter AlphaNumeric Value');")

                ElseIf Validation = Val_NU Then
                    'txt.Attributes.Add("onblur", "ValidateTextbox(" & GeneralModule.Validation_Numeric & ",this,'Please Enter Numeric or Blank Value');")
                    txt.Attributes.Add("onblur", "ValidateTextboxNumeric(" & GeneralModule.Validation_Numeric & ",this,'Please Enter Numeric or Blank Value','" & HighRange & "','" & LowRange & "','" & Validation & "' , '" & length & "' , '" & NumericScale & "');")

                ElseIf Validation = Val_IN Then
                    txt.Attributes.Add("onblur", "ValidateTextbox(" & GeneralModule.Validation_Integer & ",this,'Please Enter Integer or Blank Value');")

                ElseIf Validation = Val_AL Then
                    txt.Attributes.Add("onblur", "ValidateTextbox('" & GeneralModule.Validation_Alphabet & "',this,'Please Enter Alphabets only');")

                ElseIf Validation = Val_NNI Then
                    txt.Attributes.Add("onblur", "ValidateTextbox('" & GeneralModule.Validation_NotNullInteger & "',this,'Please Enter Integer Value');")

                ElseIf Validation = Val_NNU Then
                    txt.Attributes.Add("onblur", "ValidateTextbox('" & GeneralModule.Validation_NotNullNumeric & "',this,'Please Enter Numeric Value');")
                End If
                'Added for QC Comments on 22-May-2009
                If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    txt.Enabled = False
                End If
                '***********************
                If Id = GeneralModule.Medex_Weight Then
                    txt.Attributes.Add("onblur", "FillBMIValue('" & GeneralModule.Medex_Height.Trim() & "','" & GeneralModule.Medex_Weight.Trim() & "','" & GeneralModule.Medex_BMI.Trim() & "');")

                ElseIf Id = GeneralModule.Medex_Temperature_F Then
                    txt.Attributes.Add("onblur", "F2C('" & GeneralModule.Medex_Temperature_F.Trim() & "','" & GeneralModule.Medex_Temperature_C.Trim() & "'," & HighRange & "," & LowRange & ");")
                    Me.HFFerenhitToCelcius.Value = HighRange + "##" + LowRange

                ElseIf Id = GeneralModule.Medex_Temperature_C Then
                    'txt.Attributes.Add("ReadOnly", "true")
                    txt.Attributes.Add("onblur", "C2F('" & GeneralModule.Medex_Temperature_C.Trim() & "','" & GeneralModule.Medex_Temperature_F.Trim() & "'," & HighRange & "," & LowRange & ");")

                ElseIf Id = GeneralModule.Medex_Eligibilitydeclaredby Then
                    txt.Attributes.Add("ReadOnly", "true")
                    txt.Attributes.Add("onchange", "SetEligibilitydeclaredon('" & GeneralModule.Medex_Eligibilitydeclaredby.Trim() & "','" & GeneralModule.Medex_Eligibilitydeclaredon.Trim() & "');")

                ElseIf Id = GeneralModule.Medex_RecordedBy_BMI Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_PI_Co_I_Designate Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_Physician Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_RecordedBy Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_RecordedBy_ECG Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_RecordedBy_Xray Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_RecordedBy_Lab Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_Eligibilitydeclaredon Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_PICommentsgivenon Then
                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = Medex_BMI Then
                    txt.Attributes.Add("ReadOnly", "true")

                End If
                'txt.Attributes.Add("onfocus", "SetValue(this.id,this.value);")

                Is_FormulaEnabled = True
                GetObject = txt
            Case Else
                Return Nothing
        End Select
    End Function

#End Region

#Region "GetDateImage"

    Public Function GetDateImage(ByVal vMedExCode As Integer, ByVal objelement As Object) As Object
        Dim imgTo As New System.Web.UI.HtmlControls.HtmlImage()
        imgTo.ID = "img" & CStr(vMedExCode)
        Dim CalendarExtender As AjaxControlToolkit.CalendarExtender = New AjaxControlToolkit.CalendarExtender()
        CalendarExtender.TargetControlID = objelement.id
        CalendarExtender.PopupButtonID = imgTo.ID
        CalendarExtender.Format = "dd-MMM-yyyy"

        PlaceMedEx.Controls.Add(CalendarExtender)
        Return imgTo
    End Function

#End Region

#Region "Show Error Message"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        Me.lblerrormsg.text = exMessage + "<BR> " + eStr
    End Sub

#End Region

#Region "GetRF"

    Private Function GetRF(ByVal vFieldType As String, ByVal Id As String, ByVal vMedExValues As String) As Object
        Dim RF As New RequiredFieldValidator
        RF.ID = "RF" & Id
        RF.ControlToValidate = Id
        RF.ErrorMessage = "Please Enter the Value"
        RF.SkinID = "ErrorMsg"
        GetRF = RF
    End Function

    Private Function GetREV(ByVal vFieldType As String, ByVal Id As String, ByVal vMedExValues As String, ByVal ValidationType As String) As Object
        Dim REV As New RegularExpressionValidator
        REV.ID = "REV" & Id
        REV.ControlToValidate = Id
        REV.ErrorMessage = "Please Enter the Value"
        REV.SkinID = "ErrorMsg"
        GetREV = REV
    End Function

#End Region

#Region "Save"

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim ds As New DataSet
        Dim estr As String = String.Empty
        Dim dt_top As New DataTable
        Dim Dir As DirectoryInfo
        Dim Flinfo As FileInfo
        Dim TranNo_Retu As String = String.Empty
        Dim FolderPath As String = String.Empty
        Dim objCollection As ControlCollection
        Dim objControl As Control
        Dim ObjId As String = String.Empty
        Dim filename As String = String.Empty
        Dim SubjectId As String = String.Empty
        Dim Is_Transaction As Boolean = False
        Dim param As String = String.Empty
        Dim nMedExScreenNo As Integer
        Dim StrRetValue() As String

        Try
            '==added on 12-jan-2010 by deepak to disable btnexit when system is saving data
            BtnExit.Enabled = False
            '=======
            If Not AssignValues() Then
                Me.ObjCommon.ShowAlert("Error While Assigning Data.", Me.Page)
                Exit Sub
            End If

            ds = Me.ViewState(VS_dsSubMedex)
            If Not Me.objLambda.Save_MedExScreeningTempDtl(Me.ViewState(VS_Choice), ds, Me.Session(S_UserID), TranNo_Retu, Is_Transaction, estr) Then
                Me.ObjCommon.ShowAlert(estr, Me.Page)
                Me.btnSubject_Click(sender, e)
                Exit Sub
            End If

            objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection) 'Me.PlaceMedEx.Controls 
            For Each objControl In objCollection
                filename = String.Empty
                If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.FileUpload" Then
                    If objControl.ID.ToString.Contains("FU") Then
                        ObjId = objControl.ID.ToString.Replace("FU", "")
                    Else
                        ObjId = objControl.ID.ToString.Trim()
                    End If

                    If Request.Files(objControl.ID).FileName = "" And _
                         Not IsNothing(CType(FindControl("hlnk" + ObjId), HyperLink)) AndAlso _
                         CType(FindControl("hlnk" + ObjId), HyperLink).NavigateUrl <> "" Then

                        filename = Server.MapPath(CType(FindControl("hlnk" + ObjId), HyperLink).NavigateUrl)

                        FolderPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail"))
                        FolderPath += GeneralModule.Pro_Screening + "/" + Me.HSubjectId.Value + "/" + TranNo_Retu + "/"

                        Dir = New DirectoryInfo(FolderPath)
                        If Not Dir.Exists() Then
                            Dir.Create()
                        End If
                        Flinfo = New FileInfo(filename.Trim())
                        Flinfo.CopyTo(FolderPath + Flinfo.Name)

                    ElseIf Request.Files(objControl.ID).FileName <> "" Then
                        FolderPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail"))
                        FolderPath += GeneralModule.Pro_Screening + "/" + Me.HSubjectId.Value + "/" + TranNo_Retu + "/"

                        Dir = New DirectoryInfo(FolderPath)
                        If Not Dir.Exists() Then
                            Dir.Create()
                        End If
                        filename = FolderPath + Path.GetFileName(Request.Files(objControl.ID).FileName.ToString.Trim())
                        Request.Files(objControl.ID).SaveAs(filename)
                    End If
                End If
            Next objControl

            BtnExit.Enabled = True
            Me.ObjCommon.ShowAlert("Records Save Successfully", Me.Page)

            Me.txtSubject.Text = ""
            Me.txtremark.Text = ""
            Me.lblRcnt.Text = ""
            Me.chkScreeningType.ClearSelection()
            'Me.chkScreeningType.Items(0).Selected = True
            HProjectId.Value = ""
            txtproject.Text = ""


            nMedExScreenNo = 1
            If Not rblScreeningDate.SelectedValue.ToString.ToString.Trim() = "N" Then
                StrRetValue = TranNo_Retu.Split("/")
                nMedExScreenNo = StrRetValue(0)
            End If

            If Not objHelp.DeleteScreenigTmpTable(nMedExScreenNo, Me.HSubjectId.Value, ds, estr) Then
                Throw New Exception
            End If
            'If ContinueMode = 1 Then
            '    Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
            'End If
            'If ContinueMode <> 1 Then
            Me.rblScreeningDate.Items.Clear()
            Me.HProjectId.Value = ""
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "", "ShowHideproject()", True)
            Me.chkScreeningType.ClearSelection()
            Me.HSubjectId.Value = ""
            Me.hMedExNo.Value = ""
            GenCall()
            'End If

            If Not IsNothing(Me.Request.QueryString("SubjectId")) AndAlso _
            Me.Request.QueryString("SubjectId").ToString.Trim() <> "" Then
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "", "closewindow()", True)
                Exit Sub
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "BtnSave_Click")
        End Try
    End Sub

    Private Function AssignValues() As Boolean
        Dim index As Integer = 0
        Dim StrValue As String = String.Empty
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim str_retn As String = String.Empty
        Dim objCollection As ControlCollection
        Dim objControl As Control
        Dim dr As DataRow
        Dim dt_Save As New DataTable
        Dim dt_SubMedExMst As New DataTable
        Dim ds_SubMedExMst As New DataSet
        Dim ds_SubjectScreeningTemplate As New DataSet
        Dim TranNo As Integer = 0
        Dim estr As String = String.Empty
        Dim ObjId As String = String.Empty
        Dim cnt As Integer
        Dim ds_MedExScreeningHdrHidtory As New DataSet
        Dim wStr As String = String.Empty
        Dim dt_MedExWorkSpaceScreeningHdr As New DataTable
        Dim dt_MedExWorkSpaceScreeningDtl As New DataTable
        Dim dt_MedExScreeningDtl As New DataTable
        Dim ds_EditMedExWorkSpaceScrDtl As New DataSet
        Dim ds_Temp As New DataSet
        Dim MedExScreeningHdrNo As String = String.Empty

        Try
            ' GenCall()
            dt_Save = CType(Me.ViewState(VS_dsSubMedex), DataSet).Tables(0)
            dt_MedExScreeningDtl = CType(Me.ViewState(VS_dsSubMedex), DataSet).Tables(0).Clone()
            dt_SubMedExMst = CType(Me.ViewState(VSDtSubMedExMst), DataSet).Tables(0)
            objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection) 'Me.PlaceMedEx.Controls 

            'For Master table
            If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                If Not objHelp.GetMedExScreeningHdr("nMedExScreeningHdrNo=" & CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Rows(0)("nMedExScreeningHdrNo"), _
                                                                  WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                          ds_SubMedExMst, estr) Then
                    Return False
                End If
                dt_SubMedExMst = ds_SubMedExMst.Tables(0)
            Else


                dr = dt_SubMedExMst.NewRow()
                dr("vSubjectId") = Me.HSubjectId.Value 'Me.ddlSubject.SelectedValue.Trim()
                dr("vWorkspaceId") = GeneralModule.Pro_Screening
                dr("nMedExScreeningHdrNo") = 1
                If dt_SubMedExMst.Rows.Count > 0 Then
                    dr("nMedExScreeningHdrNo") = dt_SubMedExMst.Rows(0)("nMedExScreeningHdrNo")
                End If
                dr("dScreenDate") = CDate(ObjCommon.GetCurDatetimeWithOffSet(Me.Session(S_TimeZoneName)).DateTime)
                dr("iModifyBy") = Me.Session(S_UserID)
                dt_SubMedExMst.Rows.Add(dr)
                dt_SubMedExMst.AcceptChanges()
                '********************** Nidhi **********************************
                If Not objHelp.GetSubjectScreeningTemplate("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                                                           ds_SubjectScreeningTemplate, estr) Then
                    Return False
                End If

                dr = ds_SubjectScreeningTemplate.Tables(0).NewRow()
                dr("nScreeningTemplateHdrNo") = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Rows(0)("nScreeningTemplateHdrNo")
                dr("iModifyBy") = Me.Session(S_UserID)
                ds_SubjectScreeningTemplate.Tables(0).Rows.Add(dr)
                ds_SubjectScreeningTemplate.Tables(0).AcceptChanges()
                ds.Tables.Add(ds_SubjectScreeningTemplate.Tables(0).Copy)
                '********************************************************
            End If

            'Added by Chandresh Vanker on 16-Sep-2009 For saving Remarks in MedExScreeningHdrHistory table

            If Not objHelp.GetMedExScreeningHdrHistory(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                                                                      ds_MedExScreeningHdrHidtory, estr) Then
                Return False
            End If
            dr = ds_MedExScreeningHdrHidtory.Tables(0).NewRow()
            dr("nMedExScreeningHdrHistoryNo") = 1
            dr("nMedExScreeningHdrNo") = 1
            dr("iTranno") = 1
            dr("vRemark") = Me.txtremark.Text.Trim()
            dr("iModifyBy") = Me.Session(S_UserID)
            dr("cStatusIndi") = "N"
            ds_MedExScreeningHdrHidtory.Tables(0).Rows.Add(dr)
            ds_MedExScreeningHdrHidtory.Tables(0).AcceptChanges()
            '*****************************************************************

            '****************************************

            cnt = 0
            'PSS----------------
            dt_Save.Columns.Add("ScreeningType")
            dt_Save.AcceptChanges()
            '-------------------
            'For Detail Table
            For Each objControl In objCollection
                cnt += 1
                If Not objControl.ID Is Nothing Then
                    If objControl.ID.ToString.Trim() = "11173" Then
                        TranNo = 0
                    End If
                End If

                If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.TextBox" Then
                    TranNo = 0
                    dr = dt_Save.NewRow()
                    dr("nMedExScreeningDtlNo") = cnt
                    dr("nMedExScreeningHdrNo") = dt_SubMedExMst.Rows(0)("nMedExScreeningHdrNo")
                    If objControl.ID.ToString.Contains("txt") Then
                        ObjId = objControl.ID.ToString.Replace("txt", "")
                    Else
                        ObjId = objControl.ID.ToString.Trim()
                    End If
                    dr("vMedExCode") = ObjId 'CType(objControl, TextBox).ID
                    dr("vMedExResult") = Request.Form(objControl.ID) 'CType(objControl, TextBox).Text 'CType(Me.FindControl(obj.GetId), TextBox).Text
                    dr("iTranno") = TranNo
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("ScreeningType") = CType(objControl, TextBox).CssClass.ToString.Split(" ")(0)
                    dt_Save.Rows.Add(dr)
                    dt_Save.AcceptChanges()

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.DropDownList" Then
                    TranNo = 0
                    dr = dt_Save.NewRow()
                    dr("nMedExScreeningDtlNo") = cnt
                    dr("nMedExScreeningHdrNo") = dt_SubMedExMst.Rows(0)("nMedExScreeningHdrNo")
                    dr("vMedExCode") = objControl.ID 'CType(objControl, DropDownList).ID
                    dr("vMedExResult") = Request.Form(objControl.ID) 'CType(objControl, DropDownList).SelectedValue.Trim()
                    dr("iTranno") = TranNo
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("ScreeningType") = CType(objControl, DropDownList).CssClass.ToString.Split(" ")(0)
                    dt_Save.Rows.Add(dr)
                    dt_Save.AcceptChanges()

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.FileUpload" Then
                    Dim filename As String = String.Empty
                    If objControl.ID.ToString.Contains("FU") Then
                        ObjId = objControl.ID.ToString.Replace("FU", "")
                    Else
                        ObjId = objControl.ID.ToString.Trim()
                    End If
                    TranNo = 0
                    If Request.Files(objControl.ID).FileName = "" And _
                        Not IsNothing(CType(FindControl("hlnk" + ObjId), HyperLink)) AndAlso _
                        CType(FindControl("hlnk" + ObjId), HyperLink).NavigateUrl <> "" Then

                        filename = CType(FindControl("hlnk" + ObjId), HyperLink).NavigateUrl

                    ElseIf Request.Files(objControl.ID).FileName <> "" Then
                        filename = "~/" + System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail") + _
                                    GeneralModule.Pro_Screening + "/" + Me.HSubjectId.Value + "/" + Path.GetFileName(Request.Files(objControl.ID).FileName.ToString.Trim())
                    End If

                    dr = dt_Save.NewRow()
                    dr("nMedExScreeningDtlNo") = cnt
                    dr("nMedExScreeningHdrNo") = dt_SubMedExMst.Rows(0)("nMedExScreeningHdrNo")
                    dr("vMedExCode") = ObjId
                    dr("vMedExResult") = filename
                    dr("iTranno") = TranNo
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("ScreeningType") = CType(objControl, FileUpload).CssClass.ToString.Split(" ")(0)
                    dt_Save.Rows.Add(dr)
                    dt_Save.AcceptChanges()

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.RadioButtonList" Then
                    TranNo = 0
                    dr = dt_Save.NewRow()
                    dr("nMedExScreeningDtlNo") = cnt
                    dr("nMedExScreeningHdrNo") = dt_SubMedExMst.Rows(0)("nMedExScreeningHdrNo")
                    dr("vMedExCode") = objControl.ID 'CType(objControl, DropDownList).ID

                    'Added on 17-Sep-2009 by chandresh vanker for saving even blank values in result
                    Dim rbl As RadioButtonList = CType(FindControl(objControl.ID), RadioButtonList)
                    Dim StrChk As String = String.Empty

                    For index = 0 To rbl.Items.Count - 1
                        StrChk += IIf(rbl.Items(index).Selected, rbl.Items(index).Value.ToString.Trim() + ",", "")
                    Next index

                    If StrChk.Trim() <> "" Then
                        StrChk = StrChk.Substring(0, StrChk.LastIndexOf(","))
                    End If

                    dr("vMedExResult") = StrChk
                    '*******************************************
                    dr("iTranno") = TranNo
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("ScreeningType") = CType(objControl, RadioButtonList).CssClass.ToString.Split(" ")(0)
                    dt_Save.Rows.Add(dr)
                    dt_Save.AcceptChanges()


                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.CheckBoxList" Then
                    TranNo = 0
                    dr = dt_Save.NewRow()
                    dr("nMedExScreeningDtlNo") = cnt
                    dr("nMedExScreeningHdrNo") = dt_SubMedExMst.Rows(0)("nMedExScreeningHdrNo")
                    dr("vMedExCode") = objControl.ID 'CType(objControl, DropDownList).ID
                    Dim chkl As CheckBoxList = CType(FindControl(objControl.ID), CheckBoxList)
                    Dim StrChk As String = String.Empty

                    For index = 0 To chkl.Items.Count - 1
                        StrChk += IIf(chkl.Items(index).Selected, chkl.Items(index).Value.ToString.Trim() + ",", "")
                    Next index

                    If StrChk.Trim() <> "" Then
                        StrChk = StrChk.Substring(0, StrChk.LastIndexOf(","))
                    End If
                    dr("vMedExResult") = StrChk 'Request.Form(objControl.ID) 
                    dr("iTranno") = TranNo
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("ScreeningType") = CType(objControl, CheckBoxList).CssClass.ToString.Split(" ")(0)
                    dt_Save.Rows.Add(dr)
                    dt_Save.AcceptChanges()

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.Label" Then
                    If objControl.ID.ToString.Contains("lbl") Then
                        TranNo = 0
                        dr = dt_Save.NewRow()
                        dr("nMedExScreeningDtlNo") = cnt
                        dr("nMedExScreeningHdrNo") = dt_SubMedExMst.Rows(0)("nMedExScreeningHdrNo")
                        ObjId = objControl.ID.ToString.Replace("lbl", "")
                        dr("vMedExCode") = ObjId
                        dr("vMedExResult") = CType(objControl, Label).Text 'Request.Form(objControl.ID) 'CType(objControl, Label).Text 'CType(Me.FindControl(obj.GetId), Label).Text
                        dr("iTranno") = TranNo
                        dr("iModifyBy") = Me.Session(S_UserID)
                        dr("ScreeningType") = CType(objControl, Label).CssClass.ToString.Split(" ")(0)
                        dt_Save.Rows.Add(dr)
                        dt_Save.AcceptChanges()
                    End If
                End If

            Next objControl
            '****************************************

            dt_SubMedExMst.TableName = "MedExScreeningHdr"
            ds.Tables.Add(dt_SubMedExMst.Copy)


            dt_Save.DefaultView.RowFilter = "ScreeningType='D'"
            dt_MedExScreeningDtl = dt_Save.DefaultView.ToTable
            If dt_MedExScreeningDtl.Rows.Count > 0 Then

                dt_MedExScreeningDtl.Columns.Remove("ScreeningType")
                dt_MedExScreeningDtl.TableName = "MedExScreeningDtl"
                dt_MedExScreeningDtl.AcceptChanges()
                ds.Tables.Add(dt_MedExScreeningDtl.Copy)
            End If

            dt_Save.DefaultView.RowFilter = ""
            dt_Save.DefaultView.RowFilter = "ScreeningType='P'"
            dt_MedExWorkSpaceScreeningDtl = dt_Save.DefaultView.ToTable
            If dt_MedExWorkSpaceScreeningDtl.Rows.Count > 0 Then

                dt_MedExWorkSpaceScreeningDtl.Columns.Remove("ScreeningType")
                dt_MedExWorkSpaceScreeningDtl.Columns("nMedExScreeningHdrNo").ColumnName = "nMedExWorkSpaceScreeningHdrNo"
                dt_MedExWorkSpaceScreeningDtl.Columns("nMedExScreeningDtlNo").ColumnName = "nMedExWorkSpaceScreeningDtlNo"
                dt_MedExWorkSpaceScreeningDtl.TableName = "MedExWorkSpaceScreeningDtl"

                'if there is data in MedExWorkSpaceScreeningDtl then it will add row in MedExWorkSpaceScreeningHdr
                If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                    dt_MedExWorkSpaceScreeningHdr = dt_SubMedExMst.Copy()
                    dt_MedExWorkSpaceScreeningHdr.Rows(0)("vWorkSpaceId") = HProjectId.Value.ToString
                    dt_MedExWorkSpaceScreeningHdr.Columns.Add("nMedWorkSpaceExScreeningHdrNo")
                    dt_MedExWorkSpaceScreeningHdr.TableName = "MedExWorkSpaceScreeningHdr"
                    dt_MedExWorkSpaceScreeningHdr.AcceptChanges()
                    ds.Tables.Add(dt_MedExWorkSpaceScreeningHdr.Copy)

                ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                    If Not objHelp.GetViewScreeningTemplateHdrDtl("nMedExScreeningHdrNo=" & CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Rows(0)("nMedExScreeningHdrNo").ToString + " AND vWorkSpaceId <> '0000000000'", _
                                                                 WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                         ds_EditMedExWorkSpaceScrDtl, estr) Then
                        Return False
                    End If

                    If Not objHelp.GetMedExWorkSpaceScreeningHdr("nMedExScreeningHdrNo=" & CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Rows(0)("nMedExScreeningHdrNo"), _
                                                                 WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                         ds_Temp, estr) Then
                        Return False
                    End If
                    MedExScreeningHdrNo = ds_Temp.Tables(0).Rows(0)("nMedWorkSpaceExScreeningHdrNo").ToString


                    ds_Temp = New DataSet
                    ds_Temp.Tables.Add(dt_MedExWorkSpaceScreeningDtl.Copy().Clone())

                    For index = 0 To dt_MedExWorkSpaceScreeningDtl.Rows.Count - 1
                        ds_EditMedExWorkSpaceScrDtl.Tables(0).DefaultView.RowFilter = "vMedExCode='" + dt_MedExWorkSpaceScreeningDtl.Rows(index)("vMedExCode").ToString + "'"
                        cnt = ds_EditMedExWorkSpaceScrDtl.Tables(0).DefaultView.ToTable.Rows.Count
                        If (cnt <= 0) Or (cnt > 0 And dt_MedExWorkSpaceScreeningDtl.Rows(index)("vMedExResult").ToString.Trim.ToUpper <> ds_EditMedExWorkSpaceScrDtl.Tables(0).DefaultView.ToTable.Rows(0)("vDefaultValue").ToString.Trim.ToUpper) Then
                            dt_MedExWorkSpaceScreeningDtl.Rows(index)("nMedExWorkSpaceScreeningDtlNo") = 0
                            dt_MedExWorkSpaceScreeningDtl.Rows(index)("nMedExWorkSpaceScreeningHdrNo") = MedExScreeningHdrNo
                            ds_Temp.Tables(0).ImportRow(dt_MedExWorkSpaceScreeningDtl.Rows(index))
                        End If
                    Next
                    ds_Temp.AcceptChanges()

                    dt_MedExWorkSpaceScreeningDtl.Clear()
                    dt_MedExWorkSpaceScreeningDtl = ds_Temp.Tables(0).Copy()
                End If
            End If
            dt_MedExWorkSpaceScreeningDtl.AcceptChanges()
            ds.Tables.Add(dt_MedExWorkSpaceScreeningDtl.Copy)

            'Added by Chandresh Vanker on 16-Sep-2009 For saving Remarks in MedExScreeningHdrHistory table
            ds_MedExScreeningHdrHidtory.Tables(0).TableName = "MedExScreeningHdrHistory"
            ds.Tables.Add(ds_MedExScreeningHdrHidtory.Tables(0).Copy)
            '********************************************
            Me.ViewState(VS_dsSubMedex) = ds

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "... AssignValue")
            Return False
        End Try

    End Function

#End Region

#Region "Button Click"

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click
        Dim nMedExScreenNo As Integer
        Dim estr As String = String.Empty
        Dim ds As New DataSet

        nMedExScreenNo = 1
        If Not Me.hMedExNo.Value = String.Empty AndAlso rblScreeningDate.SelectedValue.ToString.ToString.Trim() <> "N" Then
            nMedExScreenNo = Me.hMedExNo.Value
        End If

        If Not objHelp.DeleteScreenigTmpTable(nMedExScreenNo, Me.HSubjectId.Value, ds, estr) Then
            Throw New Exception
        End If
        Me.Session.Remove("PlaceMedEx")
        Dim choice As New WS_Lambda.DataObjOpenSaveModeEnum
        choice = Me.ViewState(VS_Choice)

        If (Not IsNothing(Me.Request.QueryString("SubjectId")) AndAlso _
            Me.Request.QueryString("SubjectId").ToString.Trim() <> "") Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "", "closewindow()", True)
            Exit Sub
        End If

        Me.Response.Redirect("frmMainpage.aspx", False)
    End Sub

    Protected Sub btnSubject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubject.Click
        Try
            PlaceMedEx.Controls.Clear()
            ShowHideControls("H")
            If Not fillScreeningDates() Then
                Throw New Exception("Error While Fill Screening Dates")
            End If
            Me.ViewState(VS_ContinueMode) = Nothing
        Catch ex As Exception
            Me.ObjCommon.ShowAlert(ex.Message, Me.Page)
        End Try

    End Sub

    Protected Sub btnHistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHistory.Click
        Dim MedexGroupCode As String = String.Empty

        If Me.rblScreeningDate.Items.Count > 0 AndAlso Me.rblScreeningDate.SelectedValue.ToUpper.Trim() = "N" Then 'For New
            Me.ObjCommon.ShowAlert("No Audit Trail Available For New Screening", Me.Page())
        End If

        If Not fillGrid(MedexGroupCode) Then
            Exit Sub
        End If
        Me.MPEDeviation.Show()
    End Sub

    Protected Sub btnRmkHistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRmkHistory.Click
        Dim dsRmkHistory As New DataSet
        Dim dvRmkHistory As New DataView
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty

        Try
            Wstr = "  vSubjectID = '" + Me.HSubjectId.Value + "' And cast(convert(varchar(11),dScreenDate,113) as smalldatetime)= cast(convert(varchar(11),cast('" & Me.rblScreeningDate.SelectedValue.Trim() & "' as datetime),113)as smalldatetime) "
            If Not objHelp.View_MedExScreeningHdrHistory(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                              dsRmkHistory, estr) Then
                Throw New Exception(estr)
            End If

            If dsRmkHistory.Tables(0).Rows.Count > 0 Then
                Me.GVAuditFnlRmk.DataSource = dsRmkHistory
                Me.GVAuditFnlRmk.DataBind()
                Me.divAudit.Style.Add("display", "block")
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "SetCenter", "SetCenter('divAudit');", True)
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnRmkHistory_Click")
        End Try
    End Sub

    Protected Sub btnDeleteScreenNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteScreenNo.Click
        Dim nMedExScreenNo As Integer
        Dim estr As String = String.Empty
        Dim ds As New DataSet

        nMedExScreenNo = 1
        If Not Me.hMedExNo.Value = "" AndAlso rblScreeningDate.SelectedValue.ToString.ToString.Trim() <> "N" Then
            If Not rblScreeningDate.SelectedValue.ToString.ToString.Trim() = "" Then
                nMedExScreenNo = Me.hMedExNo.Value
            End If
        End If

        If Not objHelp.DeleteScreenigTmpTable(nMedExScreenNo, Me.HSubjectId.Value, ds, estr) Then
            Throw New Exception
        End If
        Me.hMedExNo.Value = ""
        Me.Response.Redirect("frmMainpage.aspx", False)

    End Sub

    Protected Sub btnAuthenticate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAuthenticate.Click
        If Not Auntheticate() Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Pwd_AuthenticationFail", "Pwd_AuthenticationFail();", True)
            Exit Sub
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SetPICommentsgivenon", "SetPICommentsgivenon('" & Me.HMedex_Medex_PICommentsgivenon.Value & "','" & HfUserName.Value & "','" & Me.HMedex_Medex_PI_Co_I_Designate.Value & "');", True)
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        Me.btnDeleteScreenNo_Click(sender, e)
        Me.Response.Redirect("frmMainPage.aspx", False)
    End Sub

#Region "QC Div"

    Protected Sub BtnQCSaveSend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnQCSaveSend.Click
        Dim estr As String = String.Empty
        Dim ds_QC As New DataSet
        Dim QCMsg As String = String.Empty
        Dim fromEmailId As String = String.Empty
        Dim toEmailId As String = String.Empty
        Dim password As String = String.Empty
        Dim ccEmailId As String = String.Empty
        Dim SubjectLine As String = String.Empty
        Dim wStr As String = String.Empty

        Try
            If Me.lblResponse.Text = "" And CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                Me.ObjCommon.ShowAlert("Please Click Response Button Of Particular QC Comments Against Which You Want To Response", Me.Page)
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowHide", "QCDivShowHide('ST');", True)
                Exit Sub
            End If

            If Not AssignValues_MedExScreeningHdrQC(ds_QC) Then
                Me.ObjCommon.ShowAlert("Error While Assigning Data.", Me.Page)
                Exit Sub
            End If

            'Added for QC Comments
            If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                If Not Me.objLambda.Save_MedexScreeningHdrQC(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_QC, Me.Session(S_UserID), estr) Then
                    Me.ObjCommon.ShowAlert(estr, Me.Page)
                    Exit Sub
                End If

                'If Not fillQCGrid() Then
                '    Exit Sub
                'End If

                QCMsg = "QC On Screening of " + Me.txtSubject.Text.Trim() + " <br/><br/>QC : " + Me.RBLQCFlag.SelectedItem.Text.Trim() + _
                        "<br/><br/>QC Comments: " + Me.txtQCRemarks.Value + "<br/><br/>" + _
                        "Comments Given By : " + Me.Session(S_FirstName).ToString.Trim() + " " + Me.Session(S_LastName).ToString.Trim()

                fromEmailId = ConfigurationSettings.AppSettings("Username")
                password = ConfigurationSettings.AppSettings("Password")


                toEmailId = Me.txtToEmailId.Text.Trim() 'ds_EmailAlert.Tables(0).Rows(0)("vToUserEmailId").ToString()
                ccEmailId = Me.txtCCEmailId.Text.Trim() 'ds_EmailAlert.Tables(0).Rows(0)("vCCUserEmailId").ToString()
                SubjectLine = "QC On Screening " + HSubjectId.Value.Trim() 'ds_EmailAlert.Tables(0).Rows(0)("vSubjectLine").ToString()

                Dim sn As New SendMail(Server, Request, Session, SubjectLine, QCMsg, fromEmailId, password, toEmailId, ccEmailId)
                'sn.Send(Server, Response, Session, , fromEmailId, password)

                'Changed on 26-Aug-2009
                If Not sn.Send(Server, Response, Session, , fromEmailId, password) Then
                    Me.ObjCommon.ShowAlert("QC Comments Saved Successfully But Email Is Not Sent Due To Some Reason", Me.Page)
                    Exit Sub
                End If
                '****************************************************
                sn = Nothing
                Me.ObjCommon.ShowAlert("QC Comments Saved Successfully", Me.Page)

            Else 'For Response
                If Not Me.objLambda.Save_MedexScreeningHdrQC(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_QC, Me.Session(S_UserID), estr) Then
                    Me.ObjCommon.ShowAlert(estr, Me.Page)
                    Exit Sub
                End If
                Me.ObjCommon.ShowAlert("Response Saved Successfully", Me.Page)

            End If

            If Not fillQCGrid() Then
                Exit Sub
            End If

            Me.txtQCRemarks.Value = ""
            Me.lblResponse.Text = ""
            Me.RBLQCFlag.SelectedValue = "F"
            Me.txtToEmailId.Text = ""
            Me.txtCCEmailId.Text = ""
            Me.Session.Remove("PlaceMedEx")
            rblScreeningDate_SelectedIndexChanged(sender, e) 'Call Screen Date Select
            '***********************

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

    Protected Sub BtnQCSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnQCSave.Click
        Dim estr As String = String.Empty
        Dim ds_QC As New DataSet
        Dim QCMsg As String = String.Empty
        Dim wStr As String = String.Empty

        Try
            If Me.lblResponse.Text = "" And CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                Me.ObjCommon.ShowAlert("Please Click Response Button Of Particular QC Comments Against Which You Want To Response", Me.Page)
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowHide", "QCDivShowHide('ST');", True)
                Exit Sub
            End If

            If Not AssignValues_MedExScreeningHdrQC(ds_QC) Then
                Me.ObjCommon.ShowAlert("Error While Assigning Data.", Me.Page)
                Exit Sub
            End If

            'Added for QC Comments
            If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                If Not Me.objLambda.Save_MedexScreeningHdrQC(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_QC, Me.Session(S_UserID), estr) Then
                    Me.ObjCommon.ShowAlert(estr, Me.Page)
                    Exit Sub
                End If
                Me.ObjCommon.ShowAlert("QC Comments Saved Successfully", Me.Page)

            Else 'For Response
                If Not Me.objLambda.Save_MedexScreeningHdrQC(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_QC, Me.Session(S_UserID), estr) Then
                    Me.ObjCommon.ShowAlert(estr, Me.Page)
                    Exit Sub
                End If
                Me.ObjCommon.ShowAlert("Response Saved Successfully", Me.Page)
            End If

            If Not fillQCGrid() Then
                Exit Sub
            End If
            Me.txtQCRemarks.Value = ""
            Me.lblResponse.Text = ""
            Me.RBLQCFlag.SelectedValue = "F"
            Me.Session.Remove("PlaceMedEx")

            rblScreeningDate_SelectedIndexChanged(sender, e) 'Call Screen Date Select
            '***********************

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...BtnQCSave_Click")
        End Try

    End Sub

#End Region

#End Region

#Region "Grid Events"

#Region "GVQCDtl Grid Events"

    Protected Sub GVQCDtl_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVQCDtl.RowCreated
        e.Row.Cells(GVCQC_MedExScreeningHdrQCNo).Visible = False
        e.Row.Cells(GVCQC_SubjectId).Visible = False
        e.Row.Cells(GVCQC_QCFlag).Visible = False
        e.Row.Cells(GVCQC_LnkResponse).Visible = True

        If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
            e.Row.Cells(GVCQC_LnkResponse).Visible = False
        End If
    End Sub

    Protected Sub GVQCDtl_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVQCDtl.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lnkResponse"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkResponse"), LinkButton).CommandName = "Response"
            CType(e.Row.FindControl("lnkResponse"), LinkButton).OnClientClick = "return QCDivShowHide('ST');"
        End If

    End Sub

    Protected Sub GVQCDtl_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GVQCDtl.RowCommand
        Dim index As Integer = CType(e.CommandArgument, Integer)
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim ds_MEdexInfroHdrQC As New DataSet

        If e.CommandName.ToUpper.Trim() = "RESPONSE" Then
            Me.lblResponse.Text = "Response To: " + Me.GVQCDtl.Rows(index).Cells(GVCQC_QCComment).Text.Trim()

            Wstr = "nMedExScreeningHdrQCNo=" + Me.GVQCDtl.Rows(index).Cells(GVCQC_MedExScreeningHdrQCNo).Text.Trim()
            If Not objHelp.GetMedexScreeningHdrQC(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    ds_MEdexInfroHdrQC, estr) Then
                Me.ObjCommon.ShowAlert(estr, Me.Page)
                Exit Sub
            End If

            Me.ViewState(VS_DtQC) = ds_MEdexInfroHdrQC.Tables(0)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowHide", "QCDivShowHide('ST');", True)
        End If
    End Sub
#End Region

#Region "GVAuditFnlRmk"

    Protected Sub GVAuditFnlRmk_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVAuditFnlRmk.RowCreated
        e.Row.Cells(GVAudit_MedExScreeningHdrNo).Visible = False
    End Sub

    Protected Sub GVAuditFnlRmk_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVAuditFnlRmk.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVAudit_SrNO).Text = e.Row.RowIndex + 1 + (Me.GVAuditFnlRmk.PageSize * Me.GVAuditFnlRmk.PageIndex)
        End If
    End Sub

#End Region

#End Region

#Region "Selected Index Change"

    Protected Sub rblScreeningDate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblScreeningDate.SelectedIndexChanged
        Dim wStr As String = String.Empty
        Dim ds_EmailAlert As New DataSet
        Dim eStr As String = String.Empty
        Dim ds_AuditTrail As New DataSet
        Dim dv_AuditTrail As New DataView
        Dim drsub As String = String.Empty
        Dim TodayDate As String = String.Empty
        Dim UserName As String = String.Empty

        Try
            'Added for QC comments on 22-May-2009
            Me.PlaceMedEx.Controls.Clear()
            Me.BtnSave.Visible = True
            Me.btnContinueSave.Visible = True
            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                Me.BtnSave.Visible = False
                Me.btnContinueSave.Visible = False
            End If

            '*********added on 14-11-09 by Deepak Singh***************************
            If ContinueMode = 1 Then
                wStr = "vSubjectId='" & Me.ViewState(VS_SubjectID).ToString & "'"
            Else
                wStr = "vSubjectId='" & Me.HSubjectId.Value.Trim() & "'"
            End If

            If Not objHelp.GetData("View_fillScreeningDate", "*", wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                             ds_AuditTrail, eStr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data From View_MedExScreeningHdrDtlAuditTrail : " + eStr, Me.Page)
                Exit Sub
            End If

            dv_AuditTrail = ds_AuditTrail.Tables(0).DefaultView() '.ToTable(True, "dScreenDate,vReviewBy,nMedExScreeningHdrNo".Split(",")).DefaultView()
            dv_AuditTrail.Sort = "dScreenDate desc"

            If rblScreeningDate.SelectedValue.ToString.ToString.Trim() = "N" Then

                For Each dr As DataRow In dv_AuditTrail.ToTable().Rows

                    drsub = CType(dr("dScreenDate").ToString.Trim(), Date).ToString("dd-MMM-yyyy").Trim()
                    TodayDate = CDate(ObjCommon.GetCurDatetime(Me.Session(S_TimeZoneName))).Date.ToString("dd-MMM-yyyy").Trim()
                    isScreenDoneToday = False
                    If drsub = TodayDate Then
                        Me.ObjCommon.ShowAlert("Screening For the Subject is done Today only,You can Edit that But cannot add a new Screening for Today's Date.", Me.Page)
                        isScreenDoneToday = True
                        Me.PlaceMedEx.Controls.Clear()
                        Me.ShowHideControls("H")
                        Exit Sub
                    End If
                Next
                chkScreeningType.Enabled = True
            End If
            '================================================================
            Me.BtnSave.OnClientClick = "return Validation('ADD');"
            If rblScreeningDate.SelectedValue.ToUpper.Trim() <> "N" Then
                If rblScreeningDate.SelectedValue.ToUpper.Trim() <> "" Then
                    Me.BtnSave.OnClientClick = "return Validation('EDIT');"
                    Me.btnContinueSave.OnClientClick = "return Validation('EDIT');"
                End If
            End If

            If Me.ViewState(VS_ContinueMode) = 1 Then
                Me.btnContinueSave.OnClientClick = "return Validation('EDIT');"
            Else
                Me.btnContinueSave.OnClientClick = "return Validation('NA');"
            End If

            GenCall()
            If Not rblScreeningDate.SelectedValue.ToString.ToString.Trim() = "N" And rblScreeningDate.SelectedValue.ToString.ToString.Trim() <> "" Then
                If Not fillQCGrid() Then
                    Exit Sub
                End If
                If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                    chkScreeningType.Enabled = False
                End If
            Else
                chkScreeningType.Enabled = True
            End If

            'Added on 06-July
            wStr = "nEmailAlertId =" + Email_QCOFSCREENING.ToString() + " And cStatusIndi <> 'D'"
            If Not objHelp.GetEmailAlertMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_EmailAlert, eStr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data From EmailAlert : " + eStr, Me.Page)
                Exit Sub
            End If

            If ds_EmailAlert.Tables(0).Rows.Count > 0 Then
                Me.txtToEmailId.Text = ds_EmailAlert.Tables(0).Rows(0)("vToUserEmailId").ToString()
                Me.txtCCEmailId.Text = ds_EmailAlert.Tables(0).Rows(0)("vCCUserEmailId").ToString()
            End If
            '******************************

            Me.divAudit.Style.Add("display", "none")

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub chkScreeningType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkScreeningType.SelectedIndexChanged
        Try

            rblScreeningDate_SelectedIndexChanged(sender, e)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "chkScreeningType_SelectedIndexChanged")
        End Try
    End Sub

#End Region

#Region "DeleteScreeningtmpTable"
    <Web.Services.WebMethod()> _
    Public Shared Function DeleteScreeningtmpTable(ByVal nMedExScreenNo As String, _
                                       ByVal SubjectID As String) As Boolean
        Dim estr As String = String.Empty
        Dim ds As New DataSet

        Try
            If Not objHelp.DeleteScreenigTmpTable(nMedExScreenNo, SubjectID, ds, estr) Then
                Throw New Exception
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region

#Region "Functions"

#Region "fillGrid"

    Private Function fillGrid(ByRef MedexGroupCode As String) As Boolean
        Dim ds_AuditTrail As New DataSet
        Dim dv_AuditTrail As New DataView
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim dc_AuditTrailMst As DataColumn

        Try
            Wstr = "vSubjectId='" & Me.HSubjectId.Value.Trim() & "' And vMedexCode='" & Me.hfMedexCode.Value.Trim().Split("###")(0).ToString & "' And " & _
                    " dScreenDate='" & Me.rblScreeningDate.SelectedValue & "'"

            If Me.hfMedexCode.Value.Trim().Split("###")(3).ToString = "D" Then
                If Not objHelp.View_MedExScreeningHdrDtlAuditTrail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                 ds_AuditTrail, estr) Then
                    Return False
                End If
            Else
                If Not objHelp.GetData("View_MedExScreeningHdrDtlAuditTrail_ProjectSpecific", "*", Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                 ds_AuditTrail, estr) Then
                    Return False
                End If
            End If

            Me.lblMedexDescription.Text = ds_AuditTrail.Tables(0).Rows(0).Item("vMedExDesc").ToString.Trim()
            dc_AuditTrailMst = New DataColumn("dModifyOn_IST", System.Type.GetType("System.String"))
            ds_AuditTrail.Tables(0).Columns.Add("dModifyOn_IST")
            ds_AuditTrail.AcceptChanges()
            For Each dr_Audit In ds_AuditTrail.Tables(0).Rows
                dr_Audit("dModifyOn_IST") = CDate(dr_Audit("dModifyOn")).ToString("dd-MMM-yyyy HH:mm").Trim() + strServerOffset
            Next
            ds_AuditTrail.AcceptChanges()
            dv_AuditTrail = ds_AuditTrail.Tables(0).DefaultView()
            dv_AuditTrail.Sort = "iTranNo desc"

            Me.GVHistoryDtl.DataSource = dv_AuditTrail.ToTable()
            Me.GVHistoryDtl.DataBind()
            MedexGroupCode = ds_AuditTrail.Tables(0).Rows(0).Item("vMedExGroupCode").ToString.Trim()

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...fillGrid")
            Return False
        End Try

    End Function

#End Region

#Region "fillQCGrid"

    Private Function fillQCGrid() As Boolean
        Dim Ds_QCGrid As New DataSet
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty

        Try
            If Me.rblScreeningDate.Items.Count <= 0 Then
                Return True
            End If

            Wstr = "vSubjectId='" & Me.HSubjectId.Value.Trim() & "' And " & _
                   " replace(convert(varchar(11),dScreenDate,113),' ','-')='" & CDate(Me.rblScreeningDate.SelectedValue.Trim()).ToString("dd-MMM-yyyy") & "' and cIsSourceDocComment='N'"

            If Not objHelp.View_MedexScreeningHdrQc(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        Ds_QCGrid, estr) Then
                Me.ShowErrorMessage(estr, estr)
                Return False
            End If
            Me.GVQCDtl.DataSource = Ds_QCGrid
            Me.GVQCDtl.DataBind()
            Me.BtnQC.Visible = True

            If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View And _
                Ds_QCGrid.Tables(0).Rows.Count <= 0 Then
                Me.BtnQC.Visible = False
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function

#End Region

#Region "AssignValues_MedExScreeningHdrQC"

    Private Function AssignValues_MedExScreeningHdrQC(ByRef DsSave As DataSet) As Boolean
        Dim DtMedExScreeningHdr As New DataTable
        Dim ObjId As String = String.Empty
        Dim dr As DataRow
        Dim TranNo As Integer = 0
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim ds_MEdexScreeningHdrQC As New DataSet
        Dim dtMEdexScreeningHdrQC As New DataTable

        Try
            DtMedExScreeningHdr = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable)

            If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                If Not objHelp.GetMedexScreeningHdrQC(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_MEdexScreeningHdrQC, estr) Then
                    Me.ObjCommon.ShowAlert(estr, Me.Page)
                    Return False
                End If

                dtMEdexScreeningHdrQC = ds_MEdexScreeningHdrQC.Tables(0)
                dr = dtMEdexScreeningHdrQC.NewRow
                dr("nMedExScreeningHdrQCNo") = 1
                dr("nMedExScreeningHdrNo") = DtMedExScreeningHdr.Rows(0).Item("nMedExScreeningHdrNo")
                dr("iTranNo") = 1
                dr("vQCComment") = Me.txtQCRemarks.Value.Trim()
                dr("cQCFlag") = Me.RBLQCFlag.SelectedValue.Trim()
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dr("iQCGivenBy") = Me.Session(S_UserID)


                'dr("dQCGivenOn") = System.DateTime.Today.Now.ToString("dd-MMM-yyyy hh:mm:ss")

                dr("dQCGivenOn") = CDate(ObjCommon.GetCurDatetime(Me.Session(S_TimeZoneName)))
                dtMEdexScreeningHdrQC.Rows.Add(dr)

            Else 'For Response
                dtMEdexScreeningHdrQC = Me.ViewState(VS_DtQC)
                For Each dr In dtMEdexScreeningHdrQC.Rows
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "E"
                    dr("vResponse") = Me.txtQCRemarks.Value.Trim()
                    dr("iResponseGivenBy") = Me.Session(S_UserID)
                    'dr("dResponseGivenOn") = System.DateTime.Today.Now.ToString("dd-MMM-yyyy hh:mm:ss")
                    dr("dResponseGivenOn") = CDate(ObjCommon.GetCurDatetime(Me.Session(S_TimeZoneName)))
                    dr.AcceptChanges()
                Next dr
            End If

            dtMEdexScreeningHdrQC.AcceptChanges()
            dtMEdexScreeningHdrQC.TableName = "MedExScreeningHdrQC"
            dtMEdexScreeningHdrQC.AcceptChanges()
            DsSave.Tables.Add(dtMEdexScreeningHdrQC.Copy())
            DsSave.AcceptChanges()

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function

#End Region

    Private Function fillScreeningDates() As Boolean
        Dim ds_AuditTrail As New DataSet
        Dim ds_MedExTrail As New DataSet
        Dim dv_AuditTrail As New DataView
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty

        Try
            'added on 15-Nov-2010
            Me.chkScreeningType.Enabled = True
            Me.chkScreeningType.ClearSelection()
            Me.chkScreeningType.Items(0).Selected = True
            'tr_WorkSpace.Style("display") = "none"
            HProjectId.Value = ""
            txtproject.Text = ""
            If ContinueMode = 1 Then
                Wstr = "vSubjectId='" & Me.ViewState(VS_SubjectID).ToString & "'"
            Else
                Wstr = "vSubjectId='" & Me.HSubjectId.Value.Trim() & "'"
            End If

            If Not Me.HScrNo.Value.Trim = "" Then
                Wstr = "vSubjectId='" & Me.HSubjectId.Value.Trim() & "' and nMedexScreeninghdrNo = " & Me.HScrNo.Value.Trim()
            End If

            If Not objHelp.GetData("View_fillScreeningDate", "*", Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                             ds_AuditTrail, estr) Then
                Return False
            End If

            dv_AuditTrail = ds_AuditTrail.Tables(0).DefaultView() '.ToTable(True, "dScreenDate,nMedExScreeningHdrNo,vReviewBy".Split(",")).DefaultView()
            dv_AuditTrail.Sort = "dScreenDate desc"

            Me.rblScreeningDate.Items.Clear()
            For Each dr As DataRow In dv_AuditTrail.ToTable().Rows
                If Not objHelp.ChkLockedScreenDate(dr("nMedExScreeningHdrNo")) AndAlso Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    Me.rblScreeningDate.Items.Add(New ListItem(Format(CDate(dr("dScreenDate").Date), "dd-MMM-yyyy") + _
                                                                IIf(dr("vReviewBy").ToString.Trim() = "", "", "(Reviewed By: " + dr("vReviewBy").ToString() + ")") + "(Locked)", _
                                                               dr("dScreenDate").ToString, False))
                Else
                    Me.rblScreeningDate.Items.Add(New ListItem(Format(CDate(dr("dScreenDate").Date), "dd-MMM-yyyy") + _
                                                                    IIf(dr("vReviewBy").ToString.Trim() = "", "", "(Reviewed By: " + dr("vReviewBy").ToString() + ")"), _
                                                                    dr("dScreenDate").ToString))
                End If
            Next dr

            'Added for QC Comments
            If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                Me.rblScreeningDate.Items.Insert(0, New ListItem("New Screening", "N"))
            End If
            '***********************

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...fillScreeningDates")
            Return False
        End Try
    End Function

    Private Function Auntheticate() As Boolean
        Dim pwd As String = String.Empty
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        pwd = Me.txtPassword.Text
        pwd = objHelp.EncryptPassword(pwd)

        If Me.Session(S_Password).ToString <> pwd.ToString() Then
            ObjCommon.ShowAlert("Password Authentication Fails.", Me.Page)
            Me.txtPassword.Focus()
            Return False
        End If
        Return True
    End Function

    Protected Function Save_ScreenigTmpTable(ByVal dv As DataView, ByRef UserName As String) As Boolean
        Dim eStr As String = String.Empty
        Dim ds_ScreenigTmpTable As New DataSet
        Dim dr_ScreenigTmpTable As DataRow
        Dim dt_ScreenigTmpTable As New DataTable
        Dim ds_ScreenigTmpTableFinal As New DataSet
        Dim drsub As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim estr_retu As String = String.Empty
        Dim UserId As Integer
        Dim ds_UserName As New DataSet
        Dim ds_UserTypeName As New DataSet
        Try
            Wstr = "vSubjectID='" + Me.HSubjectId.Value + "'"
            If Not hMedExNo.Value = "" Then

                If Not objHelp.DeleteScreenigTmpTable(Me.hMedExNo.Value, Me.HSubjectId.Value, ds_ScreenigTmpTable, eStr) Then
                    Return False
                End If
            End If
            Me.hMedExNo.Value = "1"
            If Not rblScreeningDate.SelectedValue.ToString.ToString.Trim() = "N" Then
                dv.RowFilter = "dScreenDate='" + rblScreeningDate.SelectedValue + "'"
                Wstr += " and nMedExScreenNo=" + dv(0)("nMedExScreeningHdrNo").ToString()
                Me.hMedExNo.Value = dv(0)("nMedExScreeningHdrNo").ToString()

            Else
                Wstr += " and nMedExScreenNo=1"
            End If

            If Not objHelp.GetScreenigTmpTable(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                    ds_ScreenigTmpTable, estr_retu) Then
                Return False
            End If
            If ds_ScreenigTmpTable.Tables(0).Rows.Count > 0 Then
                Me.hMedExNo.Value = ""
                UserId = ds_ScreenigTmpTable.Tables(0).Rows(0)("iUserID")
                objHelp.getuserMst("iUserID=" + UserId.ToString(), WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_UserName, eStr)
                UserName = ds_UserName.Tables(0).Rows(0)("vUserName")

                objHelp.getUserTypeMst("vUserTypeCode=" + ds_ScreenigTmpTable.Tables(0).Rows(0)("vUserTypeName").ToString(), WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_UserTypeName, eStr)
                UserName += " ,Profile: " + ds_UserTypeName.Tables(0).Rows(0)("vUserTypeName").ToString()
                Throw New Exception
            End If
            dt_ScreenigTmpTable = ds_ScreenigTmpTable.Tables(0)

            dr_ScreenigTmpTable = dt_ScreenigTmpTable.NewRow()
            dr_ScreenigTmpTable("nMedExScreenNo") = 1
            If Not rblScreeningDate.SelectedValue.ToString.ToString.Trim() = "N" Then
                dr_ScreenigTmpTable("nMedExScreenNo") = dv(0)("nMedExScreeningHdrNo")
            End If

            dr_ScreenigTmpTable("vSubjectID") = Me.HSubjectId.Value
            dr_ScreenigTmpTable("iUserID") = Me.Session(S_UserID)
            dr_ScreenigTmpTable("vUserTypeName") = Me.Session(S_UserType)
            dr_ScreenigTmpTable("dModifyOn") = DateTime.Now()
            dt_ScreenigTmpTable.Rows.Add(dr_ScreenigTmpTable)
            dt_ScreenigTmpTable.AcceptChanges()
            ds_ScreenigTmpTableFinal.Tables.Add(dt_ScreenigTmpTable.Copy)
            If Not Me.objLambda.Save_ScreenigTmpTable(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_ScreenigTmpTableFinal, Me.Session(S_UserID), eStr) Then
                Throw New Exception
            End If

        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

#End Region

#Region "GetAutoCalculateButton"
    Private Function GetAutoCalculateButton(ByVal vButtonName As String, ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal MedExCode As String, ByVal MedExFormula As String, ByVal vMedexType As String) As Button
        Dim btn As Button
        btn = New Button
        btn.ID = "btnAutoCalculate" & MedExGroupCode + MedExSubGroupCode + MedExCode
        btn.Text = vButtonName.Trim()
        btn.CssClass = "button"
        btn.OnClientClick = "return MedExFormula('" + MedExCode + "','" + MedExFormula + "');"
        GetAutoCalculateButton = btn
    End Function
#End Region


#Region "Formula Related Functions"
    Private Function GetControlValue(ByRef result As String, ByVal ControlId As String, ByRef objControl_Retu As Control, ByRef objControlType As String) As Boolean
        Dim objCollection As ControlCollection
        Dim objControl As Control
        Dim ObjId As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim dat As New Date
        Dim dt As DataTable = Nothing

        Try

            objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection)

            For Each objControl In objCollection

                If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.TextBox" Then
                    If objControl.ID.ToString.Contains("txt") Then
                        ObjId = objControl.ID.ToString.Replace("txt", "")
                    Else
                        ObjId = objControl.ID.ToString.Trim()
                    End If
                    If ObjId = ControlId.Trim.ToUpper() Then
                        If objControl.ID.ToString.Contains("txt") Then
                            ObjId = objControl.ID.ToString.Replace("txt", "")
                        Else
                            ObjId = objControl.ID.ToString.Trim()
                        End If
                        result = Request.Form(objControl.ID)
                        'If Date.TryParse(result, dat) Then
                        '    isDate = "True"
                        'End If
                        dt = CType(ViewState(VS_dtMedEx_Fill), DataTable)
                        dt.DefaultView().RowFilter = "vMedexCode='" & objControl.ID.ToString.Trim() & "'"
                        dt.DefaultView.ToTable()
                        objControlType = dt.DefaultView.ToTable().Rows(0).Item("vMedexType")
                        objControl_Retu = objControl
                        Return True
                    End If

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.DropDownList" Then
                    ObjId = objControl.ID.ToString.Trim()
                    If ObjId = ControlId.Trim.ToUpper() Then
                        result = Request.Form(objControl.ID)
                        objControl_Retu = objControl
                        Return True
                    End If

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.RadioButtonList" Then
                    ObjId = objControl.ID.ToString.Trim()
                    If ObjId = ControlId.Trim.ToUpper() Then
                        Dim rbl As RadioButtonList = CType(FindControl(objControl.ID), RadioButtonList)
                        Dim StrChk As String = String.Empty

                        For index As Integer = 0 To rbl.Items.Count - 1
                            StrChk += IIf(rbl.Items(index).Selected, rbl.Items(index).Value.ToString.Trim() + ",", "")
                        Next index

                        If StrChk.Trim() <> "" Then
                            StrChk = StrChk.Substring(0, StrChk.LastIndexOf(","))
                        End If
                        result = StrChk

                        'If Convert.ToString(Me.HFRadioButtonValue.Value).Trim() = "NULL" Then
                        '    result = ""
                        'End If
                        'Me.HFRadioButtonValue.Value = ""
                        objControl_Retu = objControl
                        Return True
                    End If

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.CheckBoxList" Then
                    ObjId = objControl.ID.ToString.Trim()
                    If ObjId = ControlId.Trim.ToUpper() Then
                        Dim chkl As CheckBoxList = CType(FindControl(objControl.ID), CheckBoxList)
                        Dim StrChk As String = String.Empty

                        For index As Integer = 0 To chkl.Items.Count - 1
                            StrChk += IIf(chkl.Items(index).Selected, chkl.Items(index).Value.ToString.Trim() + ",", "")
                        Next index

                        If StrChk.Trim() <> "" Then
                            StrChk = StrChk.Substring(0, StrChk.LastIndexOf(","))
                        End If
                        result = StrChk
                        objControl_Retu = objControl
                        Return True
                    End If
                End If

            Next objControl
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GetControlValue")
            Return False
        End Try
    End Function


#End Region

#Region "btnAutoCalculateForFormula"
    Protected Sub btnAutoCalculate_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim formula As String = String.Empty
        Dim MedExes() As String
        Dim index As Integer = 0
        Dim result As String = String.Empty
        Dim objControl As New Control
        Dim evaluator As New Evaluator
        Dim FinalResult As Double = 0
        Dim objControlType As String = String.Empty

        Try

            formula = Me.HFMedExFormula.Value.Trim()
            MedExes = formula.Split("?")
            formula = formula.Replace("?", "")

            For index = 0 To MedExes.Length - 1
                If MedExes(index).Length = 5 Then


                    If Not Me.GetControlValue(result, MedExes(index).ToString.Trim(), objControl, objControlType) Then
                        Throw New Exception("Error while Get Control Value")
                    End If
                    formula = formula.Replace(MedExes(index).Trim(), result)
                End If
            Next index
            If objControlType = "DateTime" Then
                ' ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "DiffAge", "DiffAge(' & formula & ", True)
                FinalResult = evaluator.GetDateDiff(formula)
            Else
                FinalResult = evaluator.Eval(formula)
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "SetResult", "SetFormulaResult('" + FinalResult.ToString() + "');", True)

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Auto Calculation. ", ex.Message)
        End Try
    End Sub
#End Region

    Protected Sub btnContinueSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContinueSave.Click
        Dim Subjectdetails As String = String.Empty
        Dim ScreeningDate As String = String.Empty

        Try

            If rblScreeningDate.SelectedValue.ToString.ToString.Trim() = "N" Then

                ScreeningDate = Nothing
            Else
                ScreeningDate = rblScreeningDate.SelectedValue.ToString.ToString.Trim()
            End If
            ContinueMode = 1
            Me.ViewState(VS_ContinueMode) = ContinueMode
            Subjectdetails = Me.txtSubject.Text.ToString
            Me.ViewState(VS_ProjectNo) = Me.HProjectId.Value
            Me.ViewState(VS_SubjectID) = Me.HSubjectId.Value
            BtnSave_Click(sender, e)
            btnSubject_Click(sender, e)
            Me.txtSubject.Text = Subjectdetails
            Me.HSubjectId.Value = Me.ViewState(VS_SubjectID).ToString.Trim()
            Me.HProjectId.Value = Me.ViewState(VS_ProjectNo).ToString.Trim()

            If ScreeningDate Is Nothing Then
                rblScreeningDate.SelectedValue = rblScreeningDate.Items(1).Value
            Else
                rblScreeningDate.SelectedValue = ScreeningDate
            End If
            rblScreeningDate_SelectedIndexChanged(sender, e)
        Catch ex As Exception

        End Try


    End Sub

    Protected Sub btnContinueWorking_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContinueWorking.Click
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ResetSessionTimer", "btnContinueWorking_Click();", True)
    End Sub

End Class
