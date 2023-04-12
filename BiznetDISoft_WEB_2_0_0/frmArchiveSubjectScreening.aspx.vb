
Partial Class frmArchiveSubjectScreening
    Inherits System.Web.UI.Page
#Region "Variable Declaration "

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private Const dt_MedEx As String = "Dt_MedEx"
    Private Const VSDtSubMedExMst As String = "Dt_SubMedExMst"
    Private Const VS_Controls As String = "VsControls"
    Private Const VS_dsSubMedex As String = "ds_SubMedex"
    Private Const VS_Choice As String = "Choice"
    Private VS_ScreenDate As String = "ScreenDate"
    Private Const VS_dtMedEx_Fill As String = "dtMedEx_Fill"
    Private Const VS_MedexGroups As String = "MedexGroups"

    Private Const VS_DtQC As String = "dtQC"

    Private Const Val_AN As String = "AN" '"Alph+anumeric"
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
    Private Gender As String = ""


#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'If Not Me.IsPostBack Then
        'BtnSave.Attributes.Add("onclick", "return closewindow();")

        Me.lblUserName.Text = Session(S_FirstName).ToString.Trim() + " " + Session(S_LastName).ToString.Trim()
        'Me.lblTime.Text = Session(S_Time).ToString
        Me.lblTime.Text = Convert.ToString(CDate(Session(S_Time)).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)

        If Not GenCall() Then
            Exit Sub
        End If

        If Page.IsPostBack Then
            btnContinueWorking_Click(Nothing, Nothing)
            Exit Sub
        End If
    End Sub

#End Region

#Region "GenCall "

    Private Function GenCall() As Boolean
      
        If Not IsNothing(Me.Request.QueryString("SchemaId")) AndAlso _
         Me.Request.QueryString("SchemaId").ToString.Trim() <> "" Then

            Me.HSchema.Value = Me.Request.QueryString("SchemaId").ToString.Trim()
        End If

        If Not IsNothing(Me.Request.QueryString("ScreenDate")) AndAlso _
        Me.Request.QueryString("ScreenDate").ToString.Trim() <> "" Then

            Me.HScreenDate.Value = Me.Request.QueryString("ScreenDate").ToString.Trim()
        End If




        If Not IsNothing(Me.Request.QueryString("SubjectId")) AndAlso _
            Me.Request.QueryString("SubjectId").ToString.Trim() <> "" Then

            Me.HSubjectId.Value = Me.Request.QueryString("SubjectId").ToString.Trim()
        End If
        If Not IsNothing(Me.Request.QueryString("ScrHdrNo")) AndAlso _
         Me.Request.QueryString("ScrHdrNo").ToString.Trim() <> "" Then

            Me.HScrNo.Value = Me.Request.QueryString("ScrHdrNo").ToString.Trim()
        End If

        If Me.HSubjectId.Value.Trim() = "" Then
            Me.rblScreeningDate.Items.Clear()
        End If


        If Not GenCall_Data() Then
            Exit Function
        End If

        If Not GenCall_ShowUI() Then
            Exit Function
        End If
        Return True

    End Function

#End Region

#Region "GenCall_Data "

    Private Function GenCall_Data() As Boolean
        Dim ds As New DataSet
        Dim ds_MedExSubjectHdrDtl As New DataSet
        Dim ds_SubMedExMst As New DataSet
        Dim ds_SubMedex As New DataSet
        Dim ds_SubDetail As New DataSet
        Dim estr_retu As String = ""
        Dim strQuery As String = ""
        Dim Wstr As String = ""
        Dim vWorkspaceId As String = Nothing
        Dim vsubjectId As String = Nothing
        Dim dscreendate As String
        Dim vschema As String = Nothing
        Try

            'If Not IsPostBack Then
            Dim sender As Object = Nothing
            Dim e As System.EventArgs = Nothing
            txtSubject.Text = Request.QueryString("SubjectId")
            btnSubject_Click(sender, e)
            txtSubject.Enabled = False

            'End If
            HfUserName.Value = Me.Session(S_UserName)
            'If Not IsNothing(Me.Request.QueryString("mode")) Then
            '    Me.ViewState(VS_Choice) = CType(Me.Request.QueryString("mode"), WS_Lambda.DataObjOpenSaveModeEnum)
            'End If



            'If Not IsNothing(Me.Request.QueryString("SubjectId")) AndAlso _
            '            (Me.Request.QueryString("SubjectId").ToString.Trim() <> "") Then

            '    Wstr = "vSubjectId='" & Me.HSubjectId.Value.Trim() & "'"
            '    If Not Me.objHelp.GetView_SubjectMaster(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
            '                                                        ds_SubDetail, estr_retu) Then
            '        Exit Function
            '    End If

            '    Me.txtSubject.Text = ds_SubDetail.Tables(0).Rows(0).Item("FullName").ToString.Trim() + " (" + Me.HSubjectId.Value.Trim() + ")"

            '    If Me.rblScreeningDate.Items.Count <= 0 Then
            '        fillScreeningDates()
            '    End If

            'End If

            If Me.rblScreeningDate.Items.Count > 0 AndAlso Me.rblScreeningDate.SelectedValue.ToUpper.Trim() <> "N" Then 'For Old

                'vWorkspaceId = GeneralModule.Pro_Screening
                vsubjectId = HSubjectId.Value
                vschema = HSchema.Value
                dscreendate = CDate(Me.rblScreeningDate.SelectedValue.Trim()).ToString("dd-MMM-yyyy")

                If Not Me.objHelp.proc_medexscreeninghdrdtl(vsubjectId, dscreendate, vschema, ds_MedExSubjectHdrDtl, estr_retu) Then
                    Exit Function
                End If

                'Me.BtnQC.Visible = True
                If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View And _
                    ds_MedExSubjectHdrDtl.Tables(0).Rows.Count <= 0 AndAlso Me.HSubjectId.Value.Trim() <> "" Then

                    Me.ObjCommon.ShowAlert("No Screening Has Been Done For This Subject", Me.Page())
                    Me.BtnQC.Visible = False
                    Exit Function
                End If

                Me.ViewState(VS_dtMedEx_Fill) = ds_MedExSubjectHdrDtl.Tables(0)
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit

                'ElseIf Me.rblScreeningDate.Items.Count > 0 AndAlso Me.rblScreeningDate.SelectedValue.ToUpper.Trim() = "N" Then 'For New

                '    Wstr = "cActiveFlag<>'N' and vWorkSpaceId='" & Me.Request.QueryString("Workspace") & "' And vMedExType <> 'IMPORT'" & _
                '            " Order by iSeqNo"

                '    If Not Me.objHelp.GetViewMedExWorkSpaceDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr_retu) Then
                '        Exit Function
                '    End If
                '    Me.ViewState(VS_dtMedEx_Fill) = ds.Tables(0)
                '    Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add

            End If

            'Added for QC Comments
            Me.LinkButton1.Visible = True
            'If Not IsNothing(Me.Request.QueryString("mode")) AndAlso _
            '    Me.Request.QueryString("mode").ToString.Trim() = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

            '    Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View
            'End If

            If Not IsNothing(Me.Request.QueryString("SubjectId")) AndAlso _
                                 (Me.Request.QueryString("SubjectId").ToString.Trim() <> "") Then
                Me.LinkButton1.Visible = False
            End If

            ds = New DataSet

            If Not objHelp.GetMedExScreeningDtl("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubMedex, estr_retu) Then
                Exit Function
            End If

            If Not objHelp.GetMedExScreeningHdr("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubMedExMst, estr_retu) Then
                Exit Function
            End If

            If ds_SubMedex Is Nothing Then
                Throw New Exception(estr_retu)
            End If

            Me.ViewState(VS_dsSubMedex) = ds_SubMedex 'Blank Data Structer for Saveing
            Me.ViewState(VSDtSubMedExMst) = ds_SubMedExMst

            Return True

        Catch ex As Exception
            'ShowErrorMessage(ex.Message.ToString, estr_retu)
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
        Dim StrGroupCodes As String = ""
        Dim StrGroupDesc As String = ""

        Dim dv_MedexGroup As New DataView
        Dim dt_MedexGroup As New DataTable
        Dim i As Integer = 0

        Dim PrevSubGroupCodes As String = ""
        Dim CntSubGroup As Integer = 0
        Dim CountForFemale As Integer = 0


        'Dim XRayDateAfterMonths As DateTime
        'Dim Dv_XRay As New DataView

        Dim dsUser As New DataSet
        Dim wstr As String = ""
        Dim estr As String = ""
        Dim Copyright As String = String.Empty
        Dim clientname As String = String.Empty
        Try
            Copyright = System.Configuration.ConfigurationManager.AppSettings("CopyRight")

            Page.Title = " :: Subject Medical Examination :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            Me.CompanyName.Value = Copyright


            'CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            'Me.lblXRayMsg.Text = ""

            Me.AutoCompleteExtender1.ContextKey = Session(S_LocationCode)

            PlaceMedEx.Controls.Clear()
            '=========added on 13-nov-2009--=====

            ShowHideControls("S")
            Me.divAudit.Style.Add("display", "none")
            Me.btnDeleteScreenNo.Style.Add("display", "none")

            If Me.rblScreeningDate.SelectedIndex <= -1 Or IsNothing(Me.ViewState(VS_dtMedEx_Fill)) Then

                ShowHideControls("H")
                Return True

            End If

            If CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Rows.Count < 1 Then

                ShowHideControls("H")

                If Not IsPostBack Then
                    Me.ObjCommon.ShowAlert("No Attribute is Attached with Screening. So, please Attach Attribute to Screening.", Me.Page)
                End If
                Exit Function

            End If

            dv_MedexGroup = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).DefaultView
            StrGroup(0) = "vMedExGroupCode"
            StrGroup(1) = "vMedExGroupDesc"
            dt_MedexGroup = dv_MedexGroup.ToTable(True, StrGroup)
            'PlaceMedEx.Controls.Add(New LiteralControl("</br>")) '</Tr>
            PlaceMedEx.Controls.Add(New LiteralControl("<Table  width=""1300"">"))

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
            'PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""BACKGROUND-IMAGE: url(images/tab1.bmp); BACKGROUND-REPEAT: no-repeat; white-space: nowrap; vertical-align:middle"">"))
            PlaceMedEx.Controls.Add(New LiteralControl("<Td width=""1200"" white-space: nowrap;>"))
            Me.GetButtons(StrGroupDesc, StrGroupCodes)

            'Added on 27-Jul-2009
            Me.BtnNext.OnClientClick = "return Next('" & StrGroupCodes & "');"
            Me.BtnPrevious.OnClientClick = "return Previous('" & StrGroupCodes & "');"
            '*******************************

            PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
            PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
            '**********************************

            PlaceMedEx.Controls.Add(New LiteralControl("<Tr ALIGN=LEFT >"))
            PlaceMedEx.Controls.Add(New LiteralControl("<Td>"))
            'Me.GetButtons(drGroup("vMedExGroupDesc").ToString.Trim(), drGroup("vMedExGroupCode").ToString.Trim())
            For Each drGroup In dt_MedexGroup.Rows

                dt = New DataTable
                dt_MedExMst = New DataTable
                dt = Me.ViewState(VS_dtMedEx_Fill)
                dv = New DataView
                dv = dt.DefaultView

                If drGroup.Item("vMedExGroupCode") = "00037" Then
                    CountForFemale += 1

                End If


                dv.RowFilter = "vMedExGroupCode='" & drGroup("vMedExGroupCode").ToString.Trim() & "'"
                dt_MedExMst = dv.ToTable()
                'PlaceMedEx.Controls.Add(New LiteralControl("<b>" & drGroup("vMedExGroupDesc").ToString.Trim() & ":</b>"))
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
                PlaceMedEx.Controls.Add(New LiteralControl("<Table width=""890"" style=""border: solid 1px gray"">")) ' border=""1""
                For Each dr In dt_MedExMst.Rows

                    ''Added By Dharmesh H.Salla on 29-Apr-2011
                    If Not CountForFemale = 0 Then
                        HfMaleCount.Value = dt_MedExMst.Rows.Count
                        CountForFemale = 0
                    End If
                    '''''''''''''''

                    If PrevSubGroupCodes = "" Or PrevSubGroupCodes <> dr("vMedExSubGroupCode") Then
                        PlaceMedEx.Controls.Add(New LiteralControl("<tr>"))
                        PlaceMedEx.Controls.Add(New LiteralControl("<td>"))
                        PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
                        PlaceMedEx.Controls.Add(New LiteralControl("</td>"))
                        PlaceMedEx.Controls.Add(New LiteralControl("</tr>"))
                        PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""600"" ALIGN=LEFT style=""BACKGROUND-COLOR: #ffcc66"">"))
                        PlaceMedEx.Controls.Add(New LiteralControl("<Td colspan=""2"" style=""white-space: nowrap; vertical-align:middle"">"))
                        PlaceMedEx.Controls.Add(Getlable(dr("vMedExSubGroupDesc").ToString.Trim(), dr("vMedExSubGroupCode") + CntSubGroup.ToString.Trim()))
                        CntSubGroup += 1
                        PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                        PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                    End If

                    PlaceMedEx.Controls.Add(New LiteralControl("<Tr ALIGN=LEFT>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""white-space: nowrap; vertical-align:middle"">"))

                    PrevSubGroupCodes = dr("vMedExSubGroupCode")
                    PlaceMedEx.Controls.Add(GetlableWithHistory(dr("vMedExDesc") & ": ", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode")))
                    PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""white-space: nowrap; vertical-align:middle"">"))


                    'Else
                    '    '********************************

                    If Not (dr("vValidationType") Is System.DBNull.Value) Then

                        If dr("vValidationType") <> "" And dr("vValidationType") <> "NA" Then
                            StrValidation = dr("vValidationType").ToString.Trim().Split(",")

                            If StrValidation.Length > 1 Then
                                objelement = GetObject(dr("vMedExType"), dr("vMedExCode"), dr("vMedExValues"), IIf(dr("vDefaultValue") Is System.DBNull.Value, "", dr("vDefaultValue")), _
                                                        StrValidation(0).ToString.Trim(), StrValidation(1).ToString.Trim(), IIf(dr("vAlertonvalue") Is System.DBNull.Value, "", dr("vAlertonvalue")), _
                                                        IIf(dr("vAlertMessage") Is System.DBNull.Value, "", dr("vAlertMessage")), IIf(dr("vHighRange") Is System.DBNull.Value, "0", dr("vHighRange")), _
                                                        IIf(dr("vLowRange") Is System.DBNull.Value, "0", dr("vLowRange")), _
                                                        IIf(dr("vRefTable") Is System.DBNull.Value, "", dr("vRefTable")), _
                                                        IIf(dr("vRefColumn") Is System.DBNull.Value, "", dr("vRefColumn")))
                            Else

                                objelement = GetObject(dr("vMedExType"), dr("vMedExCode"), dr("vMedExValues"), IIf(dr("vDefaultValue") Is System.DBNull.Value, "", dr("vDefaultValue")), _
                                                        StrValidation(0).ToString.Trim(), "", IIf(dr("vAlertonvalue") Is System.DBNull.Value, "", dr("vAlertonvalue")), _
                                                        IIf(dr("vAlertMessage") Is System.DBNull.Value, "", dr("vAlertMessage")), IIf(dr("vHighRange") Is System.DBNull.Value, "0", dr("vHighRange")), _
                                                        IIf(dr("vLowRange") Is System.DBNull.Value, "0", dr("vLowRange")), _
                                                        IIf(dr("vRefTable") Is System.DBNull.Value, "", dr("vRefTable")), _
                                                        IIf(dr("vRefColumn") Is System.DBNull.Value, "", dr("vRefColumn")))
                            End If

                        Else
                            objelement = GetObject(dr("vMedExType"), dr("vMedExCode"), dr("vMedExValues"), IIf(dr("vDefaultValue") Is System.DBNull.Value, "", dr("vDefaultValue")), _
                                                     "", "", IIf(dr("vAlertonvalue") Is System.DBNull.Value, "", dr("vAlertonvalue")), _
                                                        IIf(dr("vAlertMessage") Is System.DBNull.Value, "", dr("vAlertMessage")), IIf(dr("vHighRange") Is System.DBNull.Value, "0", dr("vHighRange")), _
                                                        IIf(dr("vLowRange") Is System.DBNull.Value, "0", dr("vLowRange")), _
                                                        IIf(dr("vRefTable") Is System.DBNull.Value, "", dr("vRefTable")), _
                                                        IIf(dr("vRefColumn") Is System.DBNull.Value, "", dr("vRefColumn")))
                        End If

                    Else
                        objelement = GetObject(dr("vMedExType"), dr("vMedExCode"), dr("vMedExValues"), IIf(dr("vDefaultValue") Is System.DBNull.Value, "", dr("vDefaultValue")), _
                                                    "", "", IIf(dr("vAlertonvalue") Is System.DBNull.Value, "", dr("vAlertonvalue")), _
                                                        IIf(dr("vAlertMessage") Is System.DBNull.Value, "", dr("vAlertMessage")), IIf(dr("vHighRange") Is System.DBNull.Value, "0", dr("vHighRange")), _
                                                        IIf(dr("vLowRange") Is System.DBNull.Value, "0", dr("vLowRange")), _
                                                        IIf(dr("vRefTable") Is System.DBNull.Value, "", dr("vRefTable")), _
                                                        IIf(dr("vRefColumn") Is System.DBNull.Value, "", dr("vRefColumn")))
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
            Me.Session("PlaceMedEx") = PlaceMedEx.Controls
            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message.ToString, "")
        End Try
    End Function

    Private Function ShowHideControls(ByVal Type As String) As Boolean

        If Type.ToUpper.Trim() = "H" Then

            Me.BtnQC.Visible = False
            Me.BtnPrevious.Visible = False
            Me.BtnNext.Visible = False
            'Me.BtnSave.Visible = False
            'Me.txtremark.Visible = False
            'Me.lblRemarks.Visible = False

        ElseIf Type.ToUpper.Trim() = "S" Then

            'Me.BtnQC.Visible = True
            Me.BtnPrevious.Visible = True
            Me.BtnNext.Visible = True
            'Me.BtnSave.Visible = True
            'Me.txtremark.Visible = True
            'Me.lblRemarks.Visible = True

        End If

        Return True

    End Function

#End Region

#Region "Fill Drop Dwon"

    'Private Function fillWorkspace() As Boolean
    '    Dim ds As New DataSet
    '    Dim estr_retu As String = ""
    '    Dim Wstr As String = "vWorkSpaceId in (select Distinct vWorkspaceId from View_MedExWorkSpaceDtl where vActivityId='0003')"

    '    If Not objHelp.getworkspacemst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr_retu) Then
    '        Exit Function
    '    End If

    '    Me.DDLWorkspace.DataSource = ds
    '    Me.DDLWorkspace.DataTextField = "vWorkSpaceDesc"
    '    Me.DDLWorkspace.DataValueField = "vWorkSpaceId"

    '    Me.DDLWorkspace.DataBind()
    '    Me.DDLWorkspace.Items.Insert(0, New ListItem("Select Project", ""))

    '    If Not Me.Request.QueryString("Workspace") Is Nothing Then
    '        Me.DDLWorkspace.SelectedValue = Me.Request.QueryString("Workspace").Trim()
    '    End If

    '    Return True
    'End Function

#End Region

#Region "DDL Selected Index Change"

    'Protected Sub DDLMedexGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLMedexGroup.SelectedIndexChanged
    '    Me.Response.Redirect("frmMedexDtls.aspx?MedexGroup=" & Me.DDLMedexGroup.SelectedValue)
    'End Sub

#End Region

#Region "GetLiteral"

    Private Function GetLiteral(ByVal text As String) As Literal
        Dim Lit As Literal
        Lit = New Literal
        Lit.Text = text
        GetLiteral = Lit
    End Function

#End Region

#Region "Getlable"

    Private Function Getlable(ByVal vlabelName As String, ByVal Id As String) As Label
        Dim lab As Label
        lab = New Label
        lab.ID = "Lab" & Id
        lab.Text = vlabelName.Trim()
        lab.SkinID = "lblDisplay"
        Getlable = lab
    End Function

    Private Function GetlableWithHistory(ByVal vlabelName As String, ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal MedExCode As String) As LinkButton
        Dim lnk As LinkButton
        lnk = New LinkButton
        lnk.ID = "Lnk" & MedExGroupCode + MedExSubGroupCode + MedExCode
        lnk.Text = vlabelName.Trim()
        lnk.SkinID = "lblDisplay"

        lnk.OnClientClick = "return HistoryDivShowHide('S','" + MedExCode + "');"

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
                'Btn.ForeColor = Drawing.Color.Brown
                Btn.Style.Add("color", "#FFffff")

                Btn.Font.Bold = True
                Btn.Style.Add("BACKGROUND-IMAGE", "url(images/btn.jpg)")
                Btn.Style.Add("background-repeat", "no-repeat")
                Btn.Style.Add("background-color", "rgb(174,77,2)")


                If index = 0 Then
                    'Btn.Style.Add("color", "navy")
                    Btn.Style.Add("color", "#FFC300")
                End If

                Btn.BorderStyle = BorderStyle.None

                'this is to add event on perticuler Dynamic controls
                'AddHandler Btn.Click, AddressOf Me.BtnAdd_Click
                '***************************************************

                'Btn.CssClass = "TABButton"
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
                'GetButtons = Btn
                PlaceMedEx.Controls.Add(Btn)
            Next
        Catch ex As Exception

        End Try
    End Function

#End Region

#Region "GetObject"

    Private Function GetObject(ByVal vFieldType As String, ByVal Id As String, ByVal vMedExValues As String, _
                                ByVal dtValues As String, Optional ByVal Validation As String = "", _
                                Optional ByVal length As String = "", Optional ByVal AlertonValue As String = "", _
                                Optional ByVal AlertMsg As String = "", _
                                Optional ByVal HighRange As String = "0", _
                                Optional ByVal LowRange As String = "0", _
                                Optional ByVal RefTable As String = "", _
                                Optional ByVal RefColumn As String = "") As Object
        Dim txt As TextBox
        Dim ddl As DropDownList
        Dim FileBro As FileUpload
        Dim RBL As RadioButtonList
        Dim CBL As CheckBoxList

        Dim ds_SubjectMst As New DataSet
        Dim wStr As String = ""
        Dim birthDate As Date

        Select Case vFieldType.ToUpper.Trim()
            Case "TEXTBOX"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                txt.CssClass = "Required textBox"
                txt.Text = dtValues
                'For Validation Only ****************
                If length <> "" And length <> "0" Then
                    txt.MaxLength = length
                End If

                HighRange = IIf(HighRange = "", "0", HighRange)
                LowRange = IIf(LowRange = "", "0", LowRange)

                If Validation = "" Or Validation = "NA" Then
                    txt.Attributes.Add("onblur", "ValidateTextbox(0,this,'No Validation'," & HighRange & "," & LowRange & ");")

                ElseIf Validation = GeneralModule.Val_AN Then
                    txt.Attributes.Add("onblur", "ValidateTextbox(" & GeneralModule.Validation_Alphanumeric & ",this,'Please Enter AlphaNumeric Value');")

                ElseIf Validation = Val_NU Then
                    txt.Attributes.Add("onblur", "ValidateTextbox(" & GeneralModule.Validation_Numeric & ",this,'Please Enter Numeric or Blank Value');")

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
                'If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                txt.Enabled = False
                'End If
                '***********************


                If Id = GeneralModule.Medex_Weight Then

                    txt.Attributes.Add("onblur", "FillBMIValue('" & GeneralModule.Medex_Height.Trim() & "','" & GeneralModule.Medex_Weight.Trim() & "','" & GeneralModule.Medex_BMI.Trim() & "');")

                ElseIf Id = GeneralModule.Medex_Temperature_F Then

                    txt.Attributes.Add("onblur", "F2C('" & GeneralModule.Medex_Temperature_F.Trim() & "','" & GeneralModule.Medex_Temperature_C.Trim() & "'," & HighRange & "," & LowRange & ");")

                ElseIf Id = GeneralModule.Medex_Temperature_C Then

                    txt.Attributes.Add("ReadOnly", "true")
                    txt.Attributes.Add("onblur", "C2F('" & GeneralModule.Medex_Temperature_C.Trim() & "','" & GeneralModule.Medex_Temperature_F.Trim() & "');")

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
                Dim estr As String = ""

                ddl = New DropDownList
                ddl.ID = Id
                ddl.CssClass = "dropDownList"
                ddl.CssClass = "Required dropDownList"

                If RefTable.Trim() <> "" And RefColumn.Trim() <> "" Then

                    If Not Me.objHelp.GetFieldsOfTable(RefTable.Trim(), RefColumn.Trim(), "", ds_ddl, estr) Then
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
                    End If

                End If
                'Adde by Naimesh Dave on 28-Nov-2008 for Alert on Value as suggested by Nikur Sir
                If AlertonValue.Trim() <> "" Then
                    ddl.Attributes.Add("onchange", "ddlAlerton('" & ddl.ClientID & "','" & AlertonValue & "','" & AlertMsg & "');")
                End If
                '*********************************

                'Added for QC Comments on 22-May-2009
                'If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                ddl.Enabled = False
                'End If
                '***********************

                GetObject = ddl
                'PlaceMedEx.Controls.Add(ddl)

            Case "RADIO"
                Dim Arrvalue() As String = Nothing
                Dim i As Integer
                Dim ds_Radio As New DataSet
                Dim estr As String = ""

                RBL = New RadioButtonList
                RBL.ID = Id
                RBL.EnableViewState = True
                'RBL.CssClass = "textBox"
                RBL.CssClass = "Required"
                If RefTable.Trim() <> "" And RefColumn.Trim() <> "" Then

                    If Not Me.objHelp.GetFieldsOfTable(RefTable.Trim(), RefColumn.Trim(), "", ds_Radio, estr) Then
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
                            If Not Me.objHelp.GetSubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
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

                If Convert.ToString(RBL.SelectedValue).Trim.ToUpper() = "MALE" Then

                    CType(Me.PlaceMedEx.FindControl(Me.hfMedExGroupCode.Value.Trim()), Button).Text = MedExGroupDescForMale
                    CType(Me.PlaceMedEx.FindControl(Me.hfMedExGroupCode.Value.Trim()), Button).Attributes.Add("disabled", "true")

                ElseIf Convert.ToString(RBL.SelectedValue).Trim.ToUpper() = "FEMALE" Then

                    CType(Me.PlaceMedEx.FindControl(Me.hfMedExGroupCode.Value.Trim()), Button).Text = MedExGroupDescForFemale
                    CType(Me.PlaceMedEx.FindControl(Me.hfMedExGroupCode.Value.Trim()), Button).Attributes.Remove("disabled")

                End If

                RBL.RepeatDirection = RepeatDirection.Horizontal
                RBL.RepeatColumns = 3

                'Adde by Naimesh Dave on 28-Nov-2008 for Alert on Value as suggested by Nikur Sir
                If AlertonValue.Trim() <> "" Then
                    RBL.Attributes.Add("onclick", "Alerton('" & RBL.ClientID & "','" & AlertonValue & "','" & AlertMsg & "');")
                    'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "AlertOn", _
                    '                        "Alerton('" & RBL.ClientID & "','" & AlertonValue & "','" & AlertMsg & "');", True)
                End If
                '*********************************

                'Added for QC Comments on 22-May-2009
                'If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                RBL.Enabled = False
                'End If
                '***********************

                If RBL.ID = MedExCodeForSex Then
                    RBL.Attributes.Add("onclick", "CheckOnlyForFemale('" & RBL.ClientID & "');")
                    Me.hfMedExCodeForSex.Value = MedExCodeForSex

                ElseIf RBL.ID = Medex_ClinicallyFit Then
                    RBL.Attributes.Add("onclick", "JustAlert('" & HfUserName.Value & "','" & Medex_Physician & "');")

                ElseIf RBL.ID = Medex_SubjectFoundEligible Then
                    'RBL.Attributes.Add("onclick", "SetUserName('" & HfUserName.Value & "','" & Medex_Eligibilitydeclaredby & "');")
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
                Dim estr As String = ""

                CBL = New CheckBoxList
                CBL.ID = Id
                CBL.EnableViewState = True
                'RBL.CssClass = "textBox"
                CBL.CssClass = "Required"
                If RefTable.Trim() <> "" And RefColumn.Trim() <> "" Then

                    If Not Me.objHelp.GetFieldsOfTable(RefTable.Trim(), RefColumn.Trim(), "", ds_Check, estr) Then
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
                'If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                CBL.Enabled = False
                'End If
                '***********************

                GetObject = CBL

            Case "FILE"
                FileBro = New FileUpload
                FileBro.ID = "FU" + Id
                FileBro.EnableViewState = True
                FileBro.CssClass = "textBox"
                FileBro.CssClass = "Required textBox"

                'Added for QC Comments on 22-May-2009
                If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    FileBro.Enabled = False
                End If
                '***********************

                'FileBro. = dtValues
                'txt.MaxLength = length 

                GetObject = FileBro

            Case "TEXTAREA"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                txt.CssClass = "Required textBox"
                txt.Text = dtValues
                txt.TextMode = TextBoxMode.MultiLine
                txt.Width = 462

                'Added for QC Comments on 22-May-2009
                'If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                txt.Enabled = False
                'End If
                '***********************

                If Id = GeneralModule.Medex_PIComments Then

                    txt.Attributes.Add("onchange", "SetPICommentsgivenon('" & GeneralModule.Medex_PICommentsgivenon.Trim() & "','" & HfUserName.Value & "','" & Medex_PI_Co_I_Designate & "');")

                End If

                GetObject = txt

            Case "DATETIME"
                Dim eStr As String = ""
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                txt.CssClass = "Required textBox"

                'Commented as per the requirement of Screeining on 06-Oct-2009
                'txt.Text = IIf(dtValues = "", System.DateTime.Now.ToString("dd-MMM-yyyy"), dtValues)
                txt.Text = dtValues
                '**********************************

                If Id = GeneralModule.Medex_DateOfBirth Then
                    '***************************************
                    If dtValues = "" Then

                        wStr = "vSubjectId = '" + Me.HSubjectId.Value.Trim() + "' And cStatusIndi <> 'D'"
                        If Not Me.objHelp.GetSubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
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
                    txt.Attributes.Add("onblur", "SetAge('" & GeneralModule.Medex_DateOfBirth.Trim() & "','" & GeneralModule.Medex_Age.Trim() & "','" & System.DateTime.Now.ToString("dd-MMM-yyyy") & "');")

                ElseIf Id = GeneralModule.Medex_Eligibilitydeclaredon Then

                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = GeneralModule.Medex_PICommentsgivenon Then

                    txt.Attributes.Add("ReadOnly", "true")

                Else

                    txt.Attributes.Add("onblur", "DateConvert(this.value,this)")
                End If

                'Added for QC Comments on 22-May-2009
                'If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                txt.Enabled = False
                'End If
                '***********************

                GetObject = txt

            Case "TIME"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                txt.CssClass = "Required textBox"

                'Commented as per the requirement of Screeining on 06-Oct-2009
                'txt.Text = IIf(dtValues = "", DateTime.Now.ToString("HH:mm:ss"), dtValues)
                txt.Text = dtValues
                '*******************************

                txt.Attributes.Add("onblur", "TimeConvert(this.value,this)")

                'Added for QC Comments on 22-May-2009
                'If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                txt.Enabled = False
                'End If
                '***********************

                GetObject = txt
                '-----------------------------
            Case "ASYNCDATETIME"
                Dim eStr As String = ""
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                txt.CssClass = "Required textBox"

                'Commented as per the requirement of Screeining on 06-Oct-2009
                'txt.Text = IIf(dtValues = "", System.DateTime.Now.ToString("dd-MMM-yyyy"), dtValues)
                txt.Text = dtValues
                '**********************************

                If Id = GeneralModule.Medex_DateOfBirth Then
                    '***************************************
                    If dtValues = "" Then

                        wStr = "vSubjectId = '" + Me.HSubjectId.Value.Trim() + "' And cStatusIndi <> 'D'"
                        If Not Me.objHelp.GetSubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
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
                    txt.Attributes.Add("onblur", "SetAge('" & GeneralModule.Medex_DateOfBirth.Trim() & "','" & GeneralModule.Medex_Age.Trim() & "','" & System.DateTime.Now.ToString("dd-MMM-yyyy") & "');")

                ElseIf Id = GeneralModule.Medex_Eligibilitydeclaredon Then

                    txt.Attributes.Add("ReadOnly", "true")

                ElseIf Id = GeneralModule.Medex_PICommentsgivenon Then

                    txt.Attributes.Add("ReadOnly", "true")

                Else

                    txt.Attributes.Add("onblur", "DateConvert(this.value,this)")
                End If

                'Added for QC Comments on 22-May-2009
                'If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                txt.Enabled = False
                'End If
                '***********************

                GetObject = txt

            Case "ASYNCTIME"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                txt.CssClass = "Required textBox"

                'Commented as per the requirement of Screeining on 06-Oct-2009
                'txt.Text = IIf(dtValues = "", DateTime.Now.ToString("HH:mm:ss"), dtValues)
                txt.Text = dtValues
                '*******************************

                txt.Attributes.Add("onblur", "TimeConvert(this.value,this)")

                'Added for QC Comments on 22-May-2009
                'If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                txt.Enabled = False
                'End If
                '***********************

                GetObject = txt
                '--------------------
            Case Else
                Return Nothing
        End Select
    End Function

#End Region

#Region "GetDateImage"

    Public Function GetDateImage(ByVal vMedExCode As Integer) As Object
        Dim imgTo As New System.Web.UI.HtmlControls.HtmlImage()
        imgTo.ID = "img" & CStr(vMedExCode)
        imgTo.Src = "../images/calendar.gif"
        imgTo.Align = "absMiddle"
        Return imgTo
    End Function

#End Region

#Region "Show Error Message"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        '   CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
    End Sub

#End Region

#Region "fillGrid"

    Private Function fillGrid(ByRef MedexGroupCode As String) As Boolean
        Dim ds_AuditTrail As New DataSet
        Dim dv_AuditTrail As New DataView
        Dim Wstr As String = ""
        Dim estr As String = ""
        Dim dc_dModifyOn As DataColumn
        Dim dc_screeningDate As DataColumn


        Try

            'CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            'Wstr = "vSubjectId='" & Me.HSubjectId.Value.Trim() & "' And vMedexCode='" & Me.hfMedexCode.Value.Trim() & "' And " & _
            '        " dScreenDate='" & Me.rblScreeningDate.SelectedValue & "'"

            If Not Me.objHelp.proc_MedexScreeningHdrDtlAuditTrail(HSubjectId.Value, HScrNo.Value, Me.hfMedexCode.Value.Trim(), HSchema.Value, ds_AuditTrail, estr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data From Proc_MedExScreeningHdrDtlAuditTrail : " + estr, Me.Page)
                Exit Function
            End If

            Me.lblMedexDescription.Text = ds_AuditTrail.Tables(0).Rows(0).Item("vMedExDesc").ToString.Trim()
            dc_dModifyOn = New DataColumn("dModifyOn_IST", System.Type.GetType("System.String"))
            dc_screeningDate = New DataColumn("dScreenDate_IST", System.Type.GetType("System.String"))
            ds_AuditTrail.Tables(0).Columns.Add("dModifyOn_IST")
            ds_AuditTrail.AcceptChanges()
            ds_AuditTrail.Tables(0).Columns.Add("dScreenDate_IST")
            For Each dr_dModifyOn In ds_AuditTrail.Tables(0).Rows
                dr_dModifyOn("dModifyOn_IST") = Convert.ToString(dr_dModifyOn("dModifyOn") + strServerOffset)
                dr_dModifyOn("dScreenDate_IST") = Convert.ToString(dr_dModifyOn("dScreenDate") + strServerOffset)
            Next
            ds_AuditTrail.AcceptChanges()


            dv_AuditTrail = ds_AuditTrail.Tables(0).DefaultView()
            dv_AuditTrail.Sort = "iTranNo desc"

            Me.GVHistoryDtl.DataSource = dv_AuditTrail.ToTable()
            Me.GVHistoryDtl.DataBind()

            MedexGroupCode = ds_AuditTrail.Tables(0).Rows(0).Item("vMedExGroupCode").ToString.Trim()

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False

        End Try

    End Function
#End Region

    '#Region "GetRF"

    '    Private Function GetRF(ByVal vFieldType As String, ByVal Id As String, ByVal vMedExValues As String) As Object
    '        Dim RF As RequiredFieldValidator

    '        RF = New RequiredFieldValidator
    '        RF.ID = "RF" & Id
    '        RF.ControlToValidate = Id
    '        RF.ErrorMessage = "Please Enter the Value"
    '        RF.SkinID = "ErrorMsg"
    '        GetRF = RF

    '    End Function

    '    Private Function GetREV(ByVal vFieldType As String, ByVal Id As String, ByVal vMedExValues As String, ByVal ValidationType As String) As Object
    '        Dim REV As RegularExpressionValidator
    '        REV = New RegularExpressionValidator
    '        REV.ID = "REV" & Id
    '        REV.ControlToValidate = Id
    '        REV.ErrorMessage = "Please Enter the Value"
    '        REV.SkinID = "ErrorMsg"
    '        GetREV = REV

    '    End Function

    '#End Region

#Region "Button Click"

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click
        Dim nMedExScreenNo As Integer
        Dim estr As String = ""
        Dim ds As New DataSet
        nMedExScreenNo = 1
        If Not Me.hMedExNo.Value = "" AndAlso rblScreeningDate.SelectedValue.ToString.ToString.Trim() <> "N" Then
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

        Me.Response.Redirect("frmMainpage.aspx")
    End Sub

    Protected Sub btnSubject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubject.Click
        Try

            PlaceMedEx.Controls.Clear()

            ShowHideControls("H")
            fillScreeningDates()

        Catch ex As Exception
            Me.ObjCommon.ShowAlert(ex.Message, Me.Page)
        End Try

    End Sub

    Protected Sub btnHistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHistory.Click
        Dim MedexGroupCode As String = ""

        If Me.rblScreeningDate.Items.Count > 0 AndAlso Me.rblScreeningDate.SelectedValue.ToUpper.Trim() = "N" Then 'For New
            Me.ObjCommon.ShowAlert("No Audit Trail Available For New Screening", Me.Page())
        End If

        If Not fillGrid(MedexGroupCode) Then
            Exit Sub
        End If

        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv", "HistoryDivShowHide('SN','','Div" & MedexGroupCode & _
                                               "','" & Me.ViewState(VS_MedexGroups) & "');", True)
    End Sub

    Private Function fillScreeningDates() As Boolean
        Dim ds_AuditTrail As New DataSet
        Dim dv_AuditTrail As New DataView
        Dim Wstr As String = ""
        Dim estr As String = ""
        Dim vSubjectId As String = Nothing
        Dim vMedexScreeningHdrNo As String
        Dim vSchema As String
        Dim sender As Object = Nothing
        Dim e As EventArgs = Nothing
        Try
            vSubjectId = HSubjectId.Value
            vMedexScreeningHdrNo = HScrNo.Value
            vSchema = HSchema.Value

            If Not Me.objHelp.proc_MedexScreeningHdrDtlAuditTrail(vSubjectId, vMedexScreeningHdrNo, "", vSchema, ds_AuditTrail, estr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data From proc_MedExScreeningHdrDtlAuditTrail : " + estr, Me.Page)
                Exit Function
            End If

            dv_AuditTrail = ds_AuditTrail.Tables(0).DefaultView().ToTable(True, "dScreenDate,nMedExScreeningHdrNo,vReviewBy".Split(",")).DefaultView()
            dv_AuditTrail.Sort = "dScreenDate desc"

            Me.rblScreeningDate.Items.Clear()
            For Each dr As DataRow In dv_AuditTrail.ToTable().Rows

                If Not Me.objHelp.ChkLockedScreenDate(dr("nMedExScreeningHdrNo")) AndAlso Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    Me.rblScreeningDate.Items.Add(New ListItem(CDate(dr("dScreenDate")).ToString("dd-MMM-yyyy") + _
                                                                IIf(dr("vReviewBy").ToString.Trim() = "", "", "(Reviewed By: " + dr("vReviewBy").ToString() + ")") + "(Locked)", _
                                                                dr("dScreenDate"), False))


                Else

                    Me.rblScreeningDate.Items.Add(New ListItem(CDate(dr("dScreenDate")).ToString("dd-MMM-yyyy") + _
                                                                    IIf(dr("vReviewBy").ToString.Trim() = "", "", "(Reviewed By: " + dr("vReviewBy").ToString() + ")"), _
                                                                    dr("dScreenDate")))

                End If

                rblScreeningDate.SelectedIndex = 0
                rblScreeningDate.Items(0).Enabled = False
            Next dr
            'rblScreeningDate_SelectedIndexChanged(sender, e)
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False

        End Try

    End Function

    Protected Sub btnRmkHistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRmkHistory.Click
        Dim dsRmkHistory As New DataSet
        Dim dvRmkHistory As New DataView
        Dim Wstr As String = ""
        Dim estr As String = ""
        Try

            Wstr = "  vSubjectID = '" + Me.HSubjectId.Value + "' And cast(convert(varchar(11),dScreenDate,113) as smalldatetime)= cast(convert(varchar(11),cast('" & Me.rblScreeningDate.SelectedValue.Trim() & "' as datetime),113)as smalldatetime) "
            'Wstr = " dScreenDate='" & CDate(Me.rblScreeningDate.SelectedValue.Trim()).ToString("dd-MMM-yyyy") & "' Order By iTranNo desc "

            If Not objHelp.View_MedExScreeningHdrHistory(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                              dsRmkHistory, estr) Then
                Throw New Exception(estr)
            End If

            If dsRmkHistory.Tables(0).Rows.Count > 0 Then

                Me.GVAuditFnlRmk.DataSource = dsRmkHistory
                Me.GVAuditFnlRmk.DataBind()
                Me.MPEAction.Show()
                'Me.divAudit.Style.Add("display", "block")
                'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "SetCenter", "SetCenter('divAudit');", True)
            End If

        Catch ex As Exception
            Me.ShowErrorMessage("", ex.Message)
        End Try

    End Sub

#End Region

#Region "fillQCGrid"

    Private Function fillQCGrid() As Boolean
        Dim Ds_QCGrid As New DataSet
        Dim Wstr As String = ""
        Dim estr As String = ""
        Try

            If Me.rblScreeningDate.Items.Count <= 0 Then
                Return True
            End If

            'CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            Wstr = "vSubjectId='" & Me.HSubjectId.Value.Trim() & "' And " & _
                   " replace(convert(varchar(11),dScreenDate,113),' ','-')='" & CDate(Me.rblScreeningDate.SelectedValue.Trim()).ToString("dd-MMM-yyyy") & "' and cIsSourceDocComment='N'"

            If Not Me.objHelp.View_MedexScreeningHdrQc(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        Ds_QCGrid, estr) Then
                Me.ShowErrorMessage(estr, estr)
                Return False
            End If

            Me.GVQCDtl.DataSource = Ds_QCGrid
            Me.GVQCDtl.DataBind()

            'Me.BtnQC.Visible = True

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

#Region "Selected Index Change"

    'Protected Sub DDLWorkspace_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLWorkspace.SelectedIndexChanged
    '    Me.Response.Redirect("frmSubjectScreening.aspx?mode=1&Workspace=" & DDLWorkspace.SelectedValue.Trim())
    'End Sub

    Protected Sub rblScreeningDate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblScreeningDate.SelectedIndexChanged
        Dim wStr As String = ""
        Dim ds_EmailAlert As New DataSet
        Dim eStr As String = ""
        Dim ds_AuditTrail As New DataSet
        Dim dv_AuditTrail As New DataView
        Dim drsub As String = ""
        Dim TodayDate As String = ""
        Dim UserName As String = ""


        Dim ScreeningDate As String = Nothing

        Try

            '*********added on 14-11-09 by Deepak Singh***************************
        
            ViewState(VS_ScreenDate) = Me.rblScreeningDate.SelectedItem.Text

            If Not Me.objHelp.proc_MedexScreeningHdrDtlAuditTrail(HSubjectId.Value, HScrNo.Value, "", HSchema.Value, ds_AuditTrail, eStr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data From Proc_MedExScreeningHdrDtlAuditTrail : " + eStr, Me.Page)
                Exit Sub
            End If

            dv_AuditTrail = ds_AuditTrail.Tables(0).DefaultView().ToTable(True, "dScreenDate,vReviewBy,nMedExScreeningHdrNo".Split(",")).DefaultView()
            dv_AuditTrail.Sort = "dScreenDate desc"



            'Me.BtnSave.OnClientClick = "return Validation('ADD');"

            'If rblScreeningDate.SelectedValue.ToUpper.Trim() <> "N" Then
            '    Me.BtnSave.OnClientClick = "return Validation('EDIT');"
            'End If

            GenCall()

            If Not fillQCGrid() Then
                Exit Sub
            End If

            'Added on 06-July
            wStr = "nEmailAlertId =" + Email_QCOFSCREENING.ToString() + " And cStatusIndi <> 'D'"
            If Not Me.objHelp.GetEmailAlertMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
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
#End Region

    Protected Sub GVAuditFnlRmk_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVAuditFnlRmk.RowDataBound
        e.Row.Cells(0).Visible = False
        e.Row.Cells(5).Visible = False

    End Sub

    Protected Sub btnContinueWorking_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContinueWorking.Click
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ResetSessionTimer", "btnContinueWorking_Click();", True)
    End Sub

End Class
