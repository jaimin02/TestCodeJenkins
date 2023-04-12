Imports System.Text
Imports System.IO

Partial Class frmMedExInfoHdrDtl
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

    Private Const dt_MedEx As String = "Dt_MedEx"
    Private Const VS_dtMedEx_Fill As String = "dt_MedEx_Fill"
    Private Const VS_DtMedExInfoHdr As String = "DtMedExInfoHdr"
    Private Const VS_DtMedExInfoDtl As String = "DtMedExInfoDtl"
    Private Const VS_Controls As String = "VsControls"
    Private Const VS_dsSubMedex As String = "ds_SubMedex"
    Private Const VS_Choice As String = "Choice"
    Private Const VS_MedExInfoHdrNO As String = "nMedExInfoHdrNO"
    Private Const VS_MedexGroups As String = "MedexGroups"
    Private Const VS_SetDefault As String = "SetDefault"

    Private arrylst As New ArrayList

    Private Const VS_DtQC As String = "dtQC"

    Private Const GVCQC_nMedexInfoHdrQcNo As Integer = 0
    Private Const GVCQC_vWorkSpaceDesc As Integer = 1
    Private Const GVCQC_vNodeDisplayName As Integer = 2
    Private Const GVCQC_SubjectId As Integer = 3
    Private Const GVCQC_SrNo As Integer = 4
    Private Const GVCQC_Subject As Integer = 5
    Private Const GVCQC_QCComment As Integer = 6
    Private Const GVCQC_QCFlag As Integer = 7
    Private Const GVCQC_QCBy As Integer = 8
    Private Const GVCQC_QCDate As Integer = 9
    Private Const GVCQC_Response As Integer = 10
    Private Const GVCQC_ResponseGivenBy As Integer = 11
    Private Const GVCQC_ResponseGivenOn As Integer = 12
    Private Const GVCQC_LnkResponse As Integer = 13


#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.HFWorkspaceId.Value = Me.Request.QueryString("WorkSpaceId").ToString
        Me.HFActivityId.Value = Me.Request.QueryString("ActivityId").ToString
        Me.HFNodeId.Value = Me.Request.QueryString("NodeId").ToString
        Me.HFPeriodId.Value = Me.Request.QueryString("PeriodId").ToString
        Me.HFSubjectId.Value = Me.Request.QueryString("SubjectId").ToString
        Me.HFMySubjectNo.Value = Me.Request.QueryString("MySubjectNo").ToString
        Me.HType.Value = " "

        Me.lblUserName.Text = Session(S_FirstName).ToString.Trim() + " " + Session(S_LastName).ToString.Trim()
        Me.lblTime.Text = Session(S_Time).ToString
        'Me.lblTime.Text = Convert.ToString(CDate(Session(S_Time)).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)

        If Not IsNothing(Me.Request.QueryString("Type")) Then
            Me.HType.Value = Me.Request.QueryString("Type").ToString
        End If


        fillWorkspace()
        GenCall()

        'Added in 05-Jun-2009
        If Me.HFSubjectId.Value.Trim() = "0000" Or Me.HFSubjectId.Value.Trim() = "" Then
            Me.ddlSubject.Visible = False
        End If
        '*******************

        If Page.IsPostBack Then
            btnContinueWorking_Click(Nothing, Nothing)
            Exit Sub
        End If
    End Sub

#End Region

#Region "Fill Drop Dwon"
    Private Function fillWorkspace() As Boolean
        Dim ds As New DataSet
        Dim estr_retu As String = ""
        Dim Wstr As String = ""

        Wstr = "vWorkSpaceId in (" + Me.HFWorkspaceId.Value.Trim() + ")"

        If Not objHelp.getworkspacemst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr_retu) Then
            Exit Function
        End If

        Me.DDLWorkspace.DataSource = ds
        Me.DDLWorkspace.DataTextField = "vWorkSpaceDesc"
        Me.DDLWorkspace.DataValueField = "vWorkSpaceId"
        Me.DDLWorkspace.DataBind()

        'Me.DDLWorkspace.Items.Insert(0, New ListItem("Select Project", ""))
        Me.DDLWorkspace.SelectedValue = Me.HFWorkspaceId.Value.Trim()

        Return True
    End Function

    Private Function fillTabControl() As Boolean

    End Function

    Private Function fillSubject() As Boolean
        Dim ds As New DataSet
        Dim estr_retu As String = ""
        Dim SubjectId As String = ""
        Dim Wstr As String = " vWorkspaceId='" & Me.HFWorkspaceId.Value.Trim() & _
                            "' and iPeriod=" & Me.Request.QueryString("PeriodId").Trim() '& " and cRejectionFlag <> 'Y'"

        If Not objHelp.GetViewWorkspaceSubjectMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr_retu) Then
            Exit Function
        End If
        SubjectId = Me.HFSubjectId.Value.Trim()

        Me.lblSubjectID.Text += SubjectId

        Me.ddlSubject.DataSource = ds
        Me.ddlSubject.DataTextField = "FullName"
        Me.ddlSubject.DataValueField = "vSubjectID"
        Me.ddlSubject.DataBind()

        'Added in 05-Jun-2009
        If SubjectId.Trim() <> "" And SubjectId.Trim() <> "0000" Then
            Me.ddlSubject.SelectedValue = SubjectId.Trim()
        End If
        '******************************

        'Added in 03-Aug-2009
        If Me.HFMySubjectNo.Value.Trim() <> "" Then
            Me.lblMySubjectNo.Text = Me.lblMySubjectNo.Text + Me.HFMySubjectNo.Value.Trim()
        End If
        '******************************


        Me.ddlSubject.Enabled = False
        Return True

    End Function

#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean

        fillSubject()

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
        Dim ds_SubMedExMst As New DataSet
        Dim ds_SubMedex As New DataSet
        Dim estr_retu As String = ""
        Dim strQuery As String = ""
        Dim Wstr As String = ""

        Dim ds_MedExInfoHdrDtl As New DataSet

        Try

            'Here We Are not Using Import Type of Medex 

            Wstr = "cActiveFlag<>'N' and vWorkSpaceId='" & Me.HFWorkspaceId.Value.Trim() & _
                    "' and vActivityId='" & Me.HFActivityId.Value & "' and vSubjectId='" & _
                    Me.HFSubjectId.Value.Trim() & "' and iPeriod=" & Me.HFPeriodId.Value.Trim() & _
                    " And iNodeId=" & Me.HFNodeId.Value.Trim() & " And vMedExType <> 'IMPORT'" & _
                    " And iMySubjectNo=" & Me.HFMySubjectNo.Value.Trim() & " Order by iSeqNo"

            If Not Me.objHelp.View_MedExInfoHdrDtl_Edit(Wstr, "*", ds_MedExInfoHdrDtl, estr_retu) Then
                Exit Function
            End If

            Me.ViewState(VS_dtMedEx_Fill) = ds_MedExInfoHdrDtl.Tables(0)

            Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add

            'Added for QC Comments
            If Not IsNothing(Me.Request.QueryString("mode")) AndAlso Me.Request.QueryString("mode").ToString.Trim() = 4 Then
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View
            End If
            '***********************

            Me.ViewState(VS_SetDefault) = "YES"

            'Added for QC Comments on 13-Jun-2009
            If ds_MedExInfoHdrDtl.Tables(0).Rows.Count > 0 Then
                Me.HFMedexInfoDtlTranNo.Value = ds_MedExInfoHdrDtl.Tables(0).Rows(0).Item("itranNo")
                Me.HFMedexHdrNo.Value = ds_MedExInfoHdrDtl.Tables(0).Rows(0).Item("nMedExInfoHdrNo")
                Me.ViewState(VS_SetDefault) = "NO"
            End If
            '*******************************

            If ds_MedExInfoHdrDtl.Tables(0).Rows.Count <= 0 Then

                Wstr = "cActiveFlag<>'N' and vWorkSpaceId='" & Me.HFWorkspaceId.Value.Trim() & "' and vActivityId='" & _
                        Me.HFActivityId.Value & "' And vMedExType <> 'IMPORT' Order by iSeqNo"

                If Not Me.objHelp.GetViewMedExWorkSpaceDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr_retu) Then
                    Exit Function
                End If
                Me.ViewState(VS_dtMedEx_Fill) = ds.Tables(0)
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
            End If

            ds = Nothing
            ds = New DataSet

            If Not objHelp.GetMedExInfoHdr("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr_retu) Then
                Response.Write(estr_retu)
                Exit Function
            End If

            If ds Is Nothing Then
                Throw New Exception(estr_retu)
            End If

            Me.ViewState(VS_DtMedExInfoHdr) = ds.Tables("MedExInfoHdr")   ' adding blank DataTable in viewstate

            ds = Nothing
            ds = New DataSet
            If Not objHelp.GetMedExInfoDtl("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr_retu) Then
                Response.Write(estr_retu)
                Exit Function
            End If

            Me.ViewState(VS_DtMedExInfoDtl) = ds.Tables(0)

            If ds Is Nothing Then
                Throw New Exception(estr_retu)
            End If

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
        Dim dt_heading As New DataTable
        Dim dv_MedexGroup As New DataView
        Dim dt_MedexGroup As New DataTable
        Dim i As Integer = 0
        Dim CntSubGroup As Integer = 0
        Dim PrevSubGroupCodes As String = ""
        Dim Wstr As String = ""
        Dim ds_Heading As New DataSet
        Dim estr As String = ""
        Dim ds_EmailAlert As New DataSet
        Dim Copyright As String = String.Empty
        Dim clientname As String = String.Empty

        Try
            Copyright = System.Configuration.ConfigurationManager.AppSettings("CopyRight")

            Page.Title = " :: Subject Attributes Detail :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            Me.CompanyName.Value = Copyright

            Me.BtnSave.OnClientClick = " Validation();"
            PlaceMedEx.Controls.Clear()
            'dv_MedexGroup = CType(Me.ViewState(dt_MedEx), DataTable).DefaultView
            'For Heading As per Sugested By Nikur Sir on 28-Nov-2008

            Wstr = "vWorkspaceId='" & Me.HFWorkspaceId.Value.Trim() & "' and vActivityId='" & _
                   Me.HFActivityId.Value.Trim() & "' And iNodeId=" & Me.HFNodeId.Value.Trim()

            If Not Me.objHelp.GetViewWorkSpaceNodeDetail(Wstr, ds_Heading, estr) Then
                Me.objCommon.ShowAlert("Error while getting Header information.", Page)
                Exit Function
            End If
            dt_heading = ds_Heading.Tables(0)

            Me.lblHeader.Text = "Project: " + dt_heading.Rows(0).Item("vWorkSpaceDesc").ToString.Trim() + _
                                "<br />Activity: " + dt_heading.Rows(0).Item("vNodeDisplayName").ToString.Trim() + _
                                "<br />Period: " + Me.HFPeriodId.Value.Trim()

            dt_heading = Nothing
            '*************************************************

            If CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Rows.Count < 1 Then
                Me.BtnSave.Visible = False
                If Not IsPostBack Then
                    Me.objCommon.ShowAlert("No Attribute is Attached with This Activity. So, please Attach Attribute to this Activity.", Me.Page)
                End If
                Exit Function
            End If

            dv_MedexGroup = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).DefaultView
            StrGroup(0) = "vMedExGroupCode"
            StrGroup(1) = "vMedExGroupDesc"
            dt_MedexGroup = dv_MedexGroup.ToTable(True, StrGroup)
            PlaceMedEx.Controls.Add(New LiteralControl("<br />")) '</Tr>
            PlaceMedEx.Controls.Add(New LiteralControl("<Table  style=""width: 890px"">"))
            '; border: solid 1px gray
            'For Tab Buttons
            For Each drGroup In dt_MedexGroup.Rows
                If StrGroupCodes <> "" Then
                    StrGroupCodes += ",Div" + drGroup("vMedExGroupCode")
                    StrGroupDesc += "," + drGroup("vMedExGroupDesc")
                Else
                    StrGroupCodes = "Div" + drGroup("vMedExGroupCode")
                    StrGroupDesc = "" + drGroup("vMedExGroupDesc")
                End If

            Next

            Me.ViewState(VS_MedexGroups) = StrGroupCodes.Trim()

            PlaceMedEx.Controls.Add(New LiteralControl("<Tr ALIGN=LEFT>"))

            PlaceMedEx.Controls.Add(New LiteralControl("<Td white-space: nowrap; vertical-align:top"">"))
            Me.GetButtons(StrGroupDesc, StrGroupCodes)

            'Added on 27-Jul-2009
            Me.BtnNext.OnClientClick = "return Next('" & StrGroupCodes & "');"
            Me.BtnPrevious.OnClientClick = "return Previous('" & StrGroupCodes & "');"
            '*******************************

            PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
            PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
            '**********************************

            PlaceMedEx.Controls.Add(New LiteralControl("<Tr ALIGN=LEFT >"))
            PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""white-space: nowrap; vertical-align:top"">"))

            For Each drGroup In dt_MedexGroup.Rows

                dt = New DataTable
                dt_MedExMst = New DataTable
                'dt = Me.ViewState(dt_MedEx)
                dt = Me.ViewState(VS_dtMedEx_Fill)
                dv = New DataView
                dv = dt.DefaultView
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

                PlaceMedEx.Controls.Add(New LiteralControl("<Table width=""100%"" style=""border: solid 1px gray"">")) ' border=""1""
                For Each dr In dt_MedExMst.Rows

                    If PrevSubGroupCodes = "" Or PrevSubGroupCodes <> dr("vMedExSubGroupCode") Then
                        PlaceMedEx.Controls.Add(New LiteralControl("<tr>"))
                        PlaceMedEx.Controls.Add(New LiteralControl("<td>"))
                        PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
                        PlaceMedEx.Controls.Add(New LiteralControl("</td>"))
                        PlaceMedEx.Controls.Add(New LiteralControl("</tr>"))
                        PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""100%"" ALIGN=LEFT style=""BACKGROUND-COLOR: #ffcc66"">"))
                        PlaceMedEx.Controls.Add(New LiteralControl("<Td colspan=""2"" style=""white-space: nowrap; vertical-align:middle"">"))
                        PlaceMedEx.Controls.Add(Getlable(dr("vMedExSubGroupDesc").ToString.Trim(), dr("vMedExSubGroupCode") + CntSubGroup.ToString.Trim()))
                        CntSubGroup += 1
                        PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                        PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                    End If

                    PlaceMedEx.Controls.Add(New LiteralControl("<Tr ALIGN=LEFT>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<Td style="" width:450px; vertical-align:middle"" ALIGN=LEFT>"))

                    PrevSubGroupCodes = dr("vMedExSubGroupCode")
                    PlaceMedEx.Controls.Add(GetlableWithHistory(dr("vMedExDesc") & ": ", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode")))
                    PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""white-space: nowrap; vertical-align:middle"" ALIGN=LEFT >"))


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
                    If Not dr("vUOM") Is System.DBNull.Value AndAlso dr("vUOM").ToString.Trim() <> "" Then
                        PlaceMedEx.Controls.Add(Getlable(dr("vUOM"), "UOM" + dr("vMedExCode")))
                    End If
                    '*********************************

                    'End If
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

            'Added for QC comments on 23-May-2009
            Me.BtnSave.Visible = True

            Me.BtnQCSaveSend.Visible = False

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                Me.BtnSave.Visible = False
                Me.BtnQCSaveSend.Visible = True
            End If

            'If Not IsNothing(Me.Request.QueryString("QC")) AndAlso Me.Request.QueryString("QC") = "Y" Then
            If Not fillQCGrid() Then
                Exit Function
            End If

            'End If

            '************************************

            dt_MedExMst = New DataTable
            dt_MedExMst = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable)


            'Added on 09-July
            Wstr = "nEmailAlertId =" + Email_QCOFMedexINfoHdr.ToString() + " And cStatusIndi <> 'D'"
            If Not Me.objHelp.GetEmailAlertMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_EmailAlert, estr) Then

                Me.objCommon.ShowAlert("Error While Getting Data From EmailAlert : " + estr, Me.Page)
                Exit Function

            End If

            If ds_EmailAlert.Tables(0).Rows.Count > 0 Then

                Me.txtToEmailId.Text = ds_EmailAlert.Tables(0).Rows(0)("vToUserEmailId").ToString()
                Me.txtCCEmailId.Text = ds_EmailAlert.Tables(0).Rows(0)("vCCUserEmailId").ToString()

            End If
            '******************************

            ''This is for Showing Save button or Print Button
            'For Each dr In dt_MedExMst.Rows

            '    Me.BtnSave.Visible = True
            '    Me.btnPrint.Visible = False

            '    If dr("vMedExCode") = GeneralModule.Medex_FilePath Or dr("vMedExCode") = GeneralModule.Medex_ScanedFile Then
            '        Me.BtnSave.Visible = False
            '        Me.btnPrint.Visible = True
            '        Exit For
            '    End If

            'Next
            ''*******************************

            Return True
        Catch ex As Exception
            'Added for QC Comments
            ShowErrorMessage(ex.Message.ToString, "")
        End Try
    End Function

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
    Private Function Getlable(ByVal vlabelName As String, ByVal Id As String, Optional ByVal vFieldType As String = "") As Label
        Dim lab As Label
        lab = New Label
        lab.ID = "Lab" & Id
        lab.Text = vlabelName.Trim()
        lab.SkinID = "lblDisplay"

        If vFieldType.ToUpper.Trim() = "IMPORT" Then
            lab.Visible = False
        End If

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
        Try
            StrGroupCode_arry = Id.Split(",")
            StrGroupDesc_arry = BtnName.Split(",")
            For index = 0 To StrGroupCode_arry.Length - 1
                Btn = New Button
                Btn.ID = "Btn" & StrGroupCode_arry(index)
                Btn.Text = " " & StrGroupDesc_arry(index).Trim() & " "
                Btn.ForeColor = Drawing.Color.Brown
                Btn.Font.Bold = True
                Btn.Style.Add("BACKGROUND-IMAGE", "url(images/btn.jpg)")
                Btn.Style.Add("background-repeat", "no-repeat")
                Btn.Style.Add("background-color", "#F2B040")

                If index = 0 Then
                    Btn.Style.Add("color", "navy")

                    'Added on 13-Oct-2009
                ElseIf index Mod 12 = 0 Then

                    PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                    PlaceMedEx.Controls.Add(New LiteralControl("<Tr ALIGN=LEFT >"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""white-space: nowrap; vertical-align:top"">"))

                End If
                '********************************************

                Btn.BorderStyle = BorderStyle.None

                'this is to add event on perticuler Dynamic controls
                'AddHandler Btn.Click, AddressOf Me.BtnAdd_Click
                '***************************************************

                'Btn.CssClass = "TABButton"
                Btn.OnClientClick = "return DisplayDiv('" & StrGroupCode_arry(index) & "','" & Id & "');"
                'GetButtons = Btn
                PlaceMedEx.Controls.Add(Btn)
            Next
        Catch ex As Exception

        End Try
    End Function
    '~/images/tab1.bmp
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
        Dim Hf As HiddenField

        Select Case vFieldType.ToUpper.Trim()
            Case "TEXTBOX"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
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
                    txt.Attributes.Add("onblur", "ValidateTextbox(" & GeneralModule.Validation_Alphanumeric & ",this,'Please Enter AlphaNumeric Value'," & _
                                                                    HighRange & "," & LowRange & ");")

                ElseIf Validation = GeneralModule.Val_NU Then
                    txt.Attributes.Add("onblur", "ValidateTextbox(" & GeneralModule.Validation_Numeric & ",this,'Please Enter Numeric or Blank Value'," & _
                                                                    HighRange & "," & LowRange & ");")

                ElseIf Validation = GeneralModule.Val_IN Then
                    txt.Attributes.Add("onblur", "ValidateTextbox(" & GeneralModule.Validation_Integer & ",this,'Please Enter Integer or Blank Value'," & _
                                                                    HighRange & "," & LowRange & ");")

                ElseIf Validation = GeneralModule.Val_AL Then
                    txt.Attributes.Add("onblur", "ValidateTextbox('" & GeneralModule.Validation_Alphabet & "',this,'Please Enter Alphabets only'," & _
                                                                    HighRange & "," & LowRange & ");")

                ElseIf Validation = GeneralModule.Val_NNI Then
                    txt.Attributes.Add("onblur", "ValidateTextbox('" & GeneralModule.Validation_NotNullInteger & "',this,'Please Enter Integer Value'," & _
                                                                    HighRange & "," & LowRange & ");")

                ElseIf Validation = GeneralModule.Val_NNU Then
                    txt.Attributes.Add("onblur", "ValidateTextbox('" & GeneralModule.Validation_NotNullNumeric & "',this,'Please Enter Numeric Value'," & _
                                                                    HighRange & "," & LowRange & ");")

                End If

                'Added on 23-Sep-2009
                If Id = GeneralModule.Medex_Oral_Body_Temperature_F Then

                    txt.Attributes.Add("onblur", "F2C('" & GeneralModule.Medex_Oral_Body_Temperature_F.Trim() & "','" & GeneralModule.Medex_Oral_Body_Temperature_C.Trim() & "');")

                    'ElseIf Id = GeneralModule.Medex_Oral_Body_Temperature_C Then

                    '    txt.Attributes.Add("onblur", "C2F('" & GeneralModule.Medex_Oral_Body_Temperature_C.Trim() & "','" & GeneralModule.Medex_Oral_Body_Temperature_F.Trim() & "');")

                End If
                '************************************

                'End If
                '************************************

                'Added for QC Comments on 22-May-2009
                If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    txt.Enabled = False
                End If
                '***********************

                GetObject = txt
                'PlaceMedEx.Controls.Add(txt)

            Case "COMBOBOX"
                Dim Arrvalue() As String = Nothing
                Dim i As Integer
                Dim ds_ddl As New DataSet
                Dim estr As String = ""

                ddl = New DropDownList
                ddl.ID = Id
                ddl.CssClass = "dropDownList"

                If RefTable.Trim() <> "" Then

                    If Not Me.objHelp.GetFieldsOfTable(RefTable.Trim(), RefColumn.Trim(), "", ds_ddl, estr) Then
                        Me.objCommon.ShowAlert(estr, Me.Page())
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
                        ddl.Items.Add(New ListItem(Arrvalue(i).Trim(), Arrvalue(i).Trim()))
                    Next

                    If Not dtValues = "" Then
                        ddl.SelectedValue = dtValues.Trim()
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
                Dim estr As String = ""

                RBL = New RadioButtonList
                RBL.ID = Id
                RBL.EnableViewState = True
                'RBL.CssClass = "textBox"

                If RefTable.Trim() <> "" Then

                    If Not Me.objHelp.GetFieldsOfTable(RefTable.Trim(), RefColumn.Trim(), "", ds_Radio, estr) Then
                        Me.objCommon.ShowAlert(estr, Me.Page())
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
                        RBL.Items.Add(New ListItem(Arrvalue(i).Trim(), Arrvalue(i).Trim().ToUpper()))
                    Next
                    If Not dtValues = "" Then
                        'RBL.ClearSelection()
                        RBL.SelectedValue = dtValues.Trim().ToUpper()
                    End If

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
                If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    RBL.Enabled = False
                End If
                '***********************

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
                'CBL.ValidationGroup = Id.Trim()
                'RBL.CssClass = "textBox"

                If RefTable.Trim() <> "" Then

                    If Not Me.objHelp.GetFieldsOfTable(RefTable.Trim(), RefColumn.Trim(), "", ds_Check, estr) Then
                        Me.objCommon.ShowAlert(estr, Me.Page())
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
                        CBL.Items.Add(New ListItem(Arrvalue(i).Trim(), Arrvalue(i).Trim()))
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
                FileBro.CssClass = "textBox"
                'FileBro.fi = dtValues
                'txt.MaxLength = length 

                GetObject = FileBro

            Case "TEXTAREA"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                txt.Text = dtValues
                txt.TextMode = TextBoxMode.MultiLine
                txt.Width = 275
                txt.Height = 70

                'Added for QC Comments on 22-May-2009
                If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    txt.Enabled = False
                End If
                '***********************

                GetObject = txt

            Case "DATETIME"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                txt.Text = IIf(dtValues = "" And Me.ViewState(VS_SetDefault) = "YES", System.DateTime.Now.ToString("dd-MMM-yyyy"), dtValues)

                If dtValues = "" AndAlso Me.HFSubjectId.Value.Trim() = "0000" Then
                    txt.Text = ""
                End If

                txt.Attributes.Add("onblur", "DateConvert(this.value,this)")

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
                txt.CssClass = "textBox"

                'Commented as per the requirement of CPMA dept on 06-Oct-2009
                'txt.Text = IIf(dtValues = "" And Me.ViewState(VS_SetDefault) = "YES", DateTime.Now.ToString("HH:mm:ss"), dtValues)
                txt.Text = dtValues

                'If dtValues = "" AndAlso Me.HFSubjectId.Value.Trim() = "0000" Then
                '    txt.Text = ""
                'End If
                '****************************************************

                txt.Attributes.Add("onblur", "TimeConvert(this.value,this)")

                'Added for QC Comments on 22-May-2009
                If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    txt.Enabled = False
                End If
                '***********************

                GetObject = txt
            Case "IMPORT"
                Hf = New HiddenField
                Hf.ID = Id
                Hf.Value = dtValues.Trim()

                GetObject = Hf
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

#Region "GetRF"
    Private Function GetRF(ByVal vFieldType As String, ByVal Id As String, ByVal vMedExValues As String) As Object
        Dim RF As RequiredFieldValidator

        RF = New RequiredFieldValidator
        RF.ID = "RF" & Id
        RF.ControlToValidate = Id
        RF.ErrorMessage = "Please Enter the Value"
        RF.SkinID = "ErrorMsg"
        GetRF = RF

    End Function

    Private Function GetREV(ByVal vFieldType As String, ByVal Id As String, ByVal vMedExValues As String, ByVal ValidationType As String) As Object
        Dim REV As RegularExpressionValidator
        REV = New RegularExpressionValidator
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
        Dim estr As String = ""
        Dim dt_top As New DataTable
        Dim SubjectId As String = ""
        Dim RedirectStr As String = ""
        Dim WorkspaceId As String = ""
        Dim ActivityId As String = ""
        Dim NodeId As String = ""
        Dim PeriodId As String = ""
        Dim MySubjectNo As String = ""
        Dim DsSave As New DataSet

        Dim Dir As DirectoryInfo
        Dim Flinfo As FileInfo
        Dim TranNo_Retu As String = ""
        Dim FolderPath As String = ""


        Dim objCollection As ControlCollection
        Dim objControl As Control
        Dim ObjId As String = ""

        Try

            SubjectId = Me.HFSubjectId.Value.Trim()
            WorkspaceId = Me.HFWorkspaceId.Value.Trim()
            ActivityId = Me.HFActivityId.Value.Trim()
            NodeId = Me.HFNodeId.Value.Trim()
            PeriodId = Me.HFPeriodId.Value.Trim()
            MySubjectNo = Me.HFMySubjectNo.Value.Trim()

            If Not AssignValues(SubjectId, WorkspaceId, ActivityId, NodeId, PeriodId, MySubjectNo, DsSave) Then
                Me.objCommon.ShowAlert("Error While Assigning Data.", Me.Page)
                Exit Sub
            End If

            ds = DsSave

            If Not Me.objLambda.Save_MedExInfoHdrDtl(Me.ViewState(VS_Choice), ds, False, Me.Session(S_UserID), TranNo_Retu, estr) Then
                Me.objCommon.ShowAlert(estr, Me.Page)
                Exit Sub
            Else

                objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection) 'Me.PlaceMedEx.Controls 
                For Each objControl In objCollection
                    If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.FileUpload" Then
                        Dim filename As String = ""

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
                            FolderPath += WorkspaceId + "/" + NodeId + "/" + SubjectId + "/" + TranNo_Retu + "/"

                            Dir = New DirectoryInfo(FolderPath)
                            If Not Dir.Exists() Then
                                Dir.Create()
                            End If

                            Flinfo = New FileInfo(filename.Trim())
                            Flinfo.CopyTo(FolderPath + Flinfo.Name)

                        ElseIf Request.Files(objControl.ID).FileName <> "" Then
                            FolderPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail"))
                            FolderPath += WorkspaceId + "/" + NodeId + "/" + SubjectId + "/" + TranNo_Retu + "/"

                            Dir = New DirectoryInfo(FolderPath)
                            If Not Dir.Exists() Then
                                Dir.Create()
                            End If

                            filename = FolderPath + Path.GetFileName(Request.Files(objControl.ID).FileName.ToString.Trim())
                            Request.Files(objControl.ID).SaveAs(filename)

                        End If

                    End If
                Next objControl

            End If

            Me.objCommon.ShowAlert("Attributes Saved Successfully", Me.Page)
            'GenCall()

            Me.Session.Remove("PlaceMedEx")
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "CloseWin", "closewindow();", True)


        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
        End Try
    End Sub

#Region "AssignValues"
    Private Function AssignValues(ByVal SubjectId As String, ByVal WorkspaceId As String, _
                                ByVal ActivityId As String, ByVal NodeId As String, _
                                ByVal PeriodId As String, _
                                ByVal MySubjectNo As String, ByRef DsSave As DataSet) As Boolean
        Dim DsMedExInfoHdr As New DataSet
        Dim DtMedExInfoHdr As New DataTable
        Dim DtMedExInfoDtl As New DataTable
        'Dim DsSave As New DataSet
        Dim objCollection As ControlCollection
        Dim objControl As Control
        Dim ObjId As String = ""
        Dim dr As DataRow
        Dim TranNo As Integer = 0
        Dim Wstr As String = ""
        Dim estr As String = ""
        Dim dsNodeInfo As New DataSet
        Try

            Wstr = "vWorkSpaceId='" & WorkspaceId & "' and iNodeId='" & NodeId & "'" & _
                    " and vActivityId='" & ActivityId & "'"

            If Not Me.objHelp.getWorkSpaceNodeDetail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                            dsNodeInfo, estr) Then
                Me.objCommon.ShowAlert("Error while getting NodeIndex", Me)
                Return False
            End If

            objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection) 'Me.PlaceMedEx.Controls 

            DtMedExInfoHdr = CType(Me.ViewState(VS_DtMedExInfoHdr), DataTable)
            DtMedExInfoDtl = CType(Me.ViewState(VS_DtMedExInfoDtl), DataTable)

            If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                If Not Me.objHelp.GetMedExInfoHdr("nMedExInfoHdrNo=" & CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Rows(0)("nMedExInfoHdrNo"), _
                                                    WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                            DsMedExInfoHdr, estr) Then
                    Exit Function
                End If
                DtMedExInfoHdr = DsMedExInfoHdr.Tables(0)

            Else

                DtMedExInfoHdr.Clear()
                dr = DtMedExInfoHdr.NewRow
                'nMedExInfoHdrNo, vWorkSpaceId, dStartDate, vPeriod, iModifyBy, dModifyOn
                dr("nMedExInfoHdrNo") = 1
                If DtMedExInfoHdr.Rows.Count > 0 Then
                    dr("nMedExInfoHdrNo") = DtMedExInfoHdr.Rows(0)("nMedExInfoHdrNo")
                End If

                dr("vWorkSpaceId") = WorkspaceId
                dr("dStartDate") = System.DateTime.Now
                dr("iPeriod") = PeriodId
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                'dr.AcceptChanges()
                DtMedExInfoHdr.Rows.Add(dr)
                DtMedExInfoHdr.TableName = "MedExInfoHdr"
                DtMedExInfoHdr.AcceptChanges()
            End If

            DtMedExInfoDtl.Clear()
            'For Detail Table
            For Each objControl In objCollection
                'nMedexInfoDtlNo, nMedexInfoHdrNo, iNodeId, iNodeIndex, vActivityId, iTranNo, vSubjectId, 
                'iMySubjectNo, vMedexCode, vMedexValue, dModifyon, iModifyBy, cStatusIndi
                If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.TextBox" Then
                    TranNo += 1
                    If objControl.ID.ToString.Contains("txt") Then
                        ObjId = objControl.ID.ToString.Replace("txt", "")
                    Else
                        ObjId = objControl.ID.ToString.Trim()
                    End If
                    dr = DtMedExInfoDtl.NewRow
                    dr("nMedexInfoDtlNo") = TranNo
                    dr("nMedExInfoHdrNo") = DtMedExInfoHdr.Rows(0)("nMedExInfoHdrNo")
                    dr("iNodeId") = NodeId
                    dr("iNodeIndex") = dsNodeInfo.Tables(0).Rows(0).Item("iNodeIndex") '1
                    dr("vActivityId") = ActivityId
                    dr("iTranNo") = TranNo
                    dr("vSubjectId") = SubjectId
                    dr("iMySubjectNo") = MySubjectNo
                    dr("vMedExCode") = ObjId 'CType(objControl, TextBox).ID
                    dr("vMedexValue") = Request.Form(objControl.ID) 'CType(objControl, TextBox).Text 'CType(Me.FindControl(obj.GetId), TextBox).Text
                    'dr("dModifyon") = 1
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "N"
                    DtMedExInfoDtl.Rows.Add(dr)
                    DtMedExInfoDtl.AcceptChanges()

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.DropDownList" Then
                    TranNo += 1
                    ObjId = objControl.ID.ToString.Trim()
                    dr = DtMedExInfoDtl.NewRow

                    dr("nMedexInfoDtlNo") = TranNo
                    dr("nMedExInfoHdrNo") = DtMedExInfoHdr.Rows(0)("nMedExInfoHdrNo")
                    dr("iNodeId") = NodeId
                    dr("iNodeIndex") = dsNodeInfo.Tables(0).Rows(0).Item("iNodeIndex") '1
                    dr("vActivityId") = ActivityId
                    dr("iTranNo") = TranNo
                    dr("vSubjectId") = SubjectId
                    dr("iMySubjectNo") = MySubjectNo
                    dr("vMedExCode") = ObjId 'CType(objControl, TextBox).ID
                    dr("vMedexValue") = Request.Form(objControl.ID) 'CType(objControl, TextBox).Text 'CType(Me.FindControl(obj.GetId), TextBox).Text
                    'dr("dModifyon") = 1
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "N"
                    DtMedExInfoDtl.Rows.Add(dr)
                    DtMedExInfoDtl.AcceptChanges()

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.FileUpload" Then
                    Dim filename As String = ""

                    TranNo += 1
                    If objControl.ID.ToString.Contains("FU") Then
                        ObjId = objControl.ID.ToString.Replace("FU", "")
                    Else
                        ObjId = objControl.ID.ToString.Trim()
                    End If


                    If Request.Files(objControl.ID).FileName = "" And _
                        Not IsNothing(CType(FindControl("hlnk" + ObjId), HyperLink)) AndAlso _
                        CType(FindControl("hlnk" + ObjId), HyperLink).NavigateUrl <> "" Then

                        filename = CType(FindControl("hlnk" + ObjId), HyperLink).NavigateUrl

                    ElseIf Request.Files(objControl.ID).FileName <> "" Then

                        filename = "~/" + System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail") + _
                                    WorkspaceId + "/" + NodeId + "/" + SubjectId + "/" + Path.GetFileName(Request.Files(objControl.ID).FileName.ToString.Trim())
                    End If

                    'If ObjId = GeneralModule.Medex_ScanedFile Then
                    '    filename = "~/" + System.Configuration.ConfigurationManager.AppSettings("FolderForSubjectDetail") + _
                    '                WorkspaceId + "/" + NodeId + "/" + SubjectId + "/"
                    'End If

                    'filename = Request.Files(objControl.ID).FileName.ToString.Trim()

                    dr = DtMedExInfoDtl.NewRow

                    dr("nMedexInfoDtlNo") = TranNo
                    dr("nMedExInfoHdrNo") = DtMedExInfoHdr.Rows(0)("nMedExInfoHdrNo")
                    dr("iNodeId") = NodeId
                    dr("iNodeIndex") = dsNodeInfo.Tables(0).Rows(0).Item("iNodeIndex") '1
                    dr("vActivityId") = ActivityId
                    dr("iTranNo") = TranNo
                    dr("vSubjectId") = SubjectId
                    dr("iMySubjectNo") = MySubjectNo
                    dr("vMedExCode") = ObjId 'CType(objControl, TextBox).ID
                    dr("vMedexValue") = filename 'Request.Form(objControl.ID)
                    'dr("dModifyon") = 1
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "N"
                    DtMedExInfoDtl.Rows.Add(dr)
                    DtMedExInfoDtl.AcceptChanges()

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.RadioButtonList" Then
                    TranNo += 1
                    ObjId = objControl.ID.ToString.Trim()
                    dr = DtMedExInfoDtl.NewRow

                    dr("nMedexInfoDtlNo") = TranNo
                    dr("nMedExInfoHdrNo") = DtMedExInfoHdr.Rows(0)("nMedExInfoHdrNo")
                    dr("iNodeId") = NodeId
                    dr("iNodeIndex") = dsNodeInfo.Tables(0).Rows(0).Item("iNodeIndex") '1
                    dr("vActivityId") = ActivityId
                    dr("iTranNo") = TranNo
                    dr("vSubjectId") = SubjectId
                    dr("iMySubjectNo") = MySubjectNo
                    dr("vMedExCode") = ObjId 'CType(objControl, TextBox).ID

                    'Added on 17-Sep-2009 by chandresh vanker for saving even blank values in result
                    Dim rbl As RadioButtonList = CType(FindControl(objControl.ID), RadioButtonList)
                    Dim StrChk As String = ""

                    For index As Integer = 0 To rbl.Items.Count - 1
                        StrChk += IIf(rbl.Items(index).Selected, rbl.Items(index).Value.ToString.Trim() + ",", "")
                    Next index

                    If StrChk.Trim() <> "" Then
                        StrChk = StrChk.Substring(0, StrChk.LastIndexOf(","))
                    End If

                    dr("vMedexValue") = StrChk
                    '*******************************************
                    'dr("vMedexValue") = Request.Form(objControl.ID) 'CType(objControl, TextBox).Text 'CType(Me.FindControl(obj.GetId), TextBox).Text
                    'dr("dModifyon") = 1
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "N"
                    DtMedExInfoDtl.Rows.Add(dr)
                    DtMedExInfoDtl.AcceptChanges()

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.CheckBoxList" Then
                    TranNo += 1
                    ObjId = objControl.ID.ToString.Trim()
                    dr = DtMedExInfoDtl.NewRow

                    dr("nMedexInfoDtlNo") = TranNo
                    dr("nMedExInfoHdrNo") = DtMedExInfoHdr.Rows(0)("nMedExInfoHdrNo")
                    dr("iNodeId") = NodeId
                    dr("iNodeIndex") = dsNodeInfo.Tables(0).Rows(0).Item("iNodeIndex") '1
                    dr("vActivityId") = ActivityId
                    dr("iTranNo") = TranNo
                    dr("vSubjectId") = SubjectId
                    dr("iMySubjectNo") = MySubjectNo
                    Dim chkl As CheckBoxList = CType(FindControl(objControl.ID), CheckBoxList)
                    Dim StrChk As String = ""

                    For index As Integer = 0 To chkl.Items.Count - 1
                        StrChk += IIf(chkl.Items(index).Selected, chkl.Items(index).Value.ToString.Trim() + ",", "")
                    Next index

                    If StrChk.Trim() <> "" Then
                        StrChk = StrChk.Substring(0, StrChk.LastIndexOf(","))
                    End If


                    dr("vMedExCode") = ObjId 'CType(objControl, TextBox).ID
                    dr("vMedexValue") = StrChk 'Request.Form(objControl.ID) 'CType(objControl, TextBox).Text 'CType(Me.FindControl(obj.GetId), TextBox).Text
                    'dr("dModifyon") = 1
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "N"
                    DtMedExInfoDtl.Rows.Add(dr)
                    DtMedExInfoDtl.AcceptChanges()

                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.HiddenField" Then
                    TranNo += 1
                    ObjId = objControl.ID.ToString.Trim()
                    dr = DtMedExInfoDtl.NewRow

                    dr("nMedexInfoDtlNo") = TranNo
                    dr("nMedExInfoHdrNo") = DtMedExInfoHdr.Rows(0)("nMedExInfoHdrNo")
                    dr("iNodeId") = NodeId
                    dr("iNodeIndex") = dsNodeInfo.Tables(0).Rows(0).Item("iNodeIndex") '1
                    dr("vActivityId") = ActivityId
                    dr("iTranNo") = TranNo
                    dr("vSubjectId") = SubjectId
                    dr("iMySubjectNo") = MySubjectNo
                    dr("vMedExCode") = ObjId 'CType(objControl, TextBox).ID

                    '******Code added by Chandresh Vanker on 28-March-2009*************
                    '******Adding Header & footer to the document**********************

                    Dim ds_WorkSpaceNodeHistory As New DataSet
                    Dim filename As String = ""
                    Dim versionNo As String = ""
                    Wstr = "vWorkSpaceId = '" + Me.DDLWorkspace.SelectedItem.Value.Trim() + "' And iNodeId=" + NodeId.Trim() + " And iStageId =" & GeneralModule.Stage_Authorized

                    If Not objHelp.GetViewWorkSpaceNodeHistory(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                            ds_WorkSpaceNodeHistory, estr) Then
                        Me.ShowErrorMessage("Error While Getting Data From View_WorkSpaceNodeHistory", estr)
                    End If

                    If Not ds_WorkSpaceNodeHistory.Tables(0).Rows.Count < 1 Then

                        filename = ds_WorkSpaceNodeHistory.Tables(0).Rows(0).Item("vDocPath").ToString()
                        versionNo = ds_WorkSpaceNodeHistory.Tables(0).Rows(0).Item("vUSerVersion").ToString()
                    End If

                    '*****************************************

                    If ObjId = GeneralModule.Medex_FilePath.Trim() Then
                        dr("vMedexValue") = filename  'File Name from WorkspaceNodeHistory
                    ElseIf ObjId = GeneralModule.Medex_DownloadedBy.Trim() Then
                        dr("vMedexValue") = Me.Session(S_UserID)
                    ElseIf ObjId = GeneralModule.Medex_VersionNo.Trim() Then
                        dr("vMedexValue") = versionNo 'Version No from WorkspaceNodeHistory
                    ElseIf ObjId = GeneralModule.Medex_ReleaseDate.Trim() Then
                        dr("vMedexValue") = Date.Now.Date.ToString("dd-MMM-yyyy") 'Version No from WorkspaceNodeHistory
                    Else
                        dr("vMedexValue") = Request.Form(objControl.ID) 'CType(objControl, TextBox).Text 'CType(Me.FindControl(obj.GetId), TextBox).Text
                    End If


                    'dr("dModifyon") = 1
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "N"
                    DtMedExInfoDtl.Rows.Add(dr)
                    DtMedExInfoDtl.AcceptChanges()


                End If

            Next objControl
            '****************************************

            DtMedExInfoDtl.TableName = "MedExInfoDtl"
            DtMedExInfoDtl.AcceptChanges()

            DsSave = Nothing
            DsSave = New DataSet
            DsSave.Tables.Add(DtMedExInfoHdr.Copy())
            DsSave.Tables.Add(DtMedExInfoDtl.Copy())
            DsSave.AcceptChanges()
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function

#End Region

#End Region

#Region "Button Click"
    'Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click
    '    Me.Session.Remove("PlaceMedEx")
    '    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "CloseWin", "window.close();", True)
    '    'Me.Response.Redirect("frmMainpage.aspx")
    'End Sub

#End Region

#Region "Selected Index Change"

    Protected Sub DDLWorkspace_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLWorkspace.SelectedIndexChanged
        Me.Response.Redirect("frmSubjectScreening.aspx?mode=1&Workspace=" & DDLWorkspace.SelectedValue.Trim())
    End Sub

#End Region

#Region "Link Button Events"

    'Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
    '    Me.Session.Remove("PlaceMedEx")
    '    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "CloseWin", "window.close();", True)
    'End Sub

    'Protected Sub lnkAttendance_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkAttendance.Click
    '    Response.Redirect("frmWorkspaceSubjectMst.aspx?mode=1&workspaceid=" + _
    '                        Me.HFWorkspaceId.Value.Trim() & _
    '                        "&Page2=frmMedExInfoHdrDtl&Type=" & Me.Request.QueryString("Type") & _
    '                        "&PeriodId=" & Me.HFPeriodId.Value.Trim())

    'End Sub

#End Region

    '*******************************************************************************************

    'Added for QA Comments on 13-Jun-2009
#Region "fillQCGrid"

    Private Function fillQCGrid() As Boolean
        Dim Ds_QCGrid As New DataSet
        Dim Wstr As String = ""
        Dim estr As String = ""
        Try
            ' "' and iMedexInfoDtlTranNo=" & Me.HFMedexInfoDtlTranNo.Value & _
            Wstr = "vSubjectId='" & Me.HFSubjectId.Value.Trim() & _
                    "' and nMedExInfoHdrNo=" & Me.HFMedexHdrNo.Value & _
                    " and iNodeId=" & Me.HFNodeId.Value


            Me.BtnQC.Visible = False

            If Me.HFMedexHdrNo.Value.Trim() <> "" And Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View And _
                (Not IsNothing(Me.Request.QueryString("QC")) AndAlso Me.Request.QueryString("QC") = "Y") Then

                Me.BtnQC.Visible = True

            End If

            If Not Me.objHelp.View_MedexInfoHdrQc(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        Ds_QCGrid, estr) Then
                Me.ShowErrorMessage(estr, estr)
                Return False
            End If

            Me.GVQCDtl.DataSource = Ds_QCGrid
            Me.GVQCDtl.DataBind()

            Me.BtnQC.Visible = True

            If Ds_QCGrid.Tables(0).Rows.Count <= 0 Then

                Me.BtnQC.Visible = False

                If Not IsNothing(Me.Request.QueryString("QC")) AndAlso Me.Request.QueryString("QC") = "Y" Then
                    Me.BtnQC.Visible = True
                End If

            End If

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False

        End Try

    End Function

#End Region
    '********************************************

#Region "QC Div Save"

    'Added for QA Comments on 13-Jun-2009
#Region "AssignValues_MedExInfoHdrQC"
    Private Function AssignValues_MedExInfoHdrQC(ByVal SubjectId As String, ByVal WorkspaceId As String, _
                                ByVal ActivityId As String, ByVal NodeId As String, _
                                ByVal PeriodId As String, _
                                ByVal MySubjectNo As String, ByRef DsSave As DataSet) As Boolean
        Dim DtMedExInfoHdr As New DataTable
        Dim ObjId As String = ""
        Dim dr As DataRow
        Dim TranNo As Integer = 0
        Dim Wstr As String = ""
        Dim estr As String = ""
        Dim ds_MEdexInfroHdrQC As New DataSet
        Dim dtMEdexInfroHdrQC As New DataTable
        Dim MedexInfoDtlTranNo As String
        Dim MedExInfoHdrNo As String
        Try
            MedexInfoDtlTranNo = Me.HFMedexInfoDtlTranNo.Value.Trim()
            MedExInfoHdrNo = Me.HFMedexHdrNo.Value.Trim()

            DtMedExInfoHdr = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable)

            If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

                If Not Me.objHelp.GetMedexInfoHdrQC(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                                                  ds_MEdexInfroHdrQC, estr) Then
                    Me.objCommon.ShowAlert(estr, Me.Page)
                    Return False
                End If

                dtMEdexInfroHdrQC = ds_MEdexInfroHdrQC.Tables(0)
                dr = dtMEdexInfroHdrQC.NewRow
                dr("nMedExInfoHdrQCNo") = 1
                dr("nMedExInfoHdrNo") = MedExInfoHdrNo
                dr("iNodeId") = NodeId
                dr("vSubjectId") = SubjectId
                dr("iMedexInfoDtlTranNo") = MedexInfoDtlTranNo
                dr("iTranNo") = 1
                dr("vQCComment") = Me.txtQCRemarks.Value.Trim()
                dr("cQCFlag") = Me.RBLQCFlag.SelectedValue.Trim()
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dr("iQCGivenBy") = Me.Session(S_UserID)
                dr("dQCGivenOn") = System.DateTime.Today.Now.ToString("dd-MMM-yyyy hh:mm tt")
                dtMEdexInfroHdrQC.Rows.Add(dr)

            Else 'For Response

                dtMEdexInfroHdrQC = Me.ViewState(VS_DtQC)

                For Each dr In dtMEdexInfroHdrQC.Rows
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "E"
                    dr("vResponse") = Me.txtQCRemarks.Value.Trim()
                    dr("iResponseGivenBy") = Me.Session(S_UserID)
                    dr("dResponseGivenOn") = System.DateTime.Today.Now.ToString("dd-MMM-yyyy hh:mm tt")

                    dr.AcceptChanges()
                Next dr

            End If

            dtMEdexInfroHdrQC.AcceptChanges()

            dtMEdexInfroHdrQC.TableName = "MedExInfoHdrQC"
            dtMEdexInfroHdrQC.AcceptChanges()

            DsSave.Tables.Add(dtMEdexInfroHdrQC.Copy())
            DsSave.AcceptChanges()
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function

#End Region
    '********************************************

    Protected Sub BtnQCSaveSend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnQCSaveSend.Click
        Dim estr As String = ""
        Dim SubjectId As String = ""
        Dim RedirectStr As String = ""
        Dim WorkspaceId As String = ""
        Dim ActivityId As String = ""
        Dim NodeId As String = ""
        Dim PeriodId As String = ""
        Dim MySubjectNo As String = ""
        Dim DsSave As New DataSet

        Dim TranNo_Retu As String = ""
        Dim FolderPath As String = ""

        Dim QCMsg As String = ""

        Dim ds_QC As New DataSet
        Dim fromEmailId As String = ""
        Dim toEmailId As String = ""
        Dim password As String = ""
        Dim ccEmailId As String = ""
        Dim SubjectLine As String = ""
        Dim wStr As String = ""
        Dim ds_EmailAlert As New DataSet

        Try
            If Me.lblResponse.Text = "" And CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                Me.objCommon.ShowAlert("Please Click Response Button Of Particular QA Comments Against Which You Want To Response", Me.Page)

                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowHide", "QCDivShowHide('ST');", True)

                Exit Sub
            End If

            SubjectId = Me.HFSubjectId.Value.Trim()
            WorkspaceId = Me.HFWorkspaceId.Value.Trim()
            ActivityId = Me.HFActivityId.Value.Trim()
            NodeId = Me.HFNodeId.Value.Trim()
            PeriodId = Me.HFPeriodId.Value.Trim()
            MySubjectNo = Me.HFMySubjectNo.Value.Trim()

            'Added for QC Comments

            If Not AssignValues_MedExInfoHdrQC(SubjectId, WorkspaceId, ActivityId, NodeId, PeriodId, MySubjectNo, ds_QC) Then
                Me.objCommon.ShowAlert("Error While Assigning Data.", Me.Page)
                Exit Sub
            End If

            If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                If Not Me.objLambda.Save_MedExInfoHdrQC(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_QC, Me.Session(S_UserID), estr) Then

                    Me.objCommon.ShowAlert(estr, Me.Page)
                    Exit Sub

                End If

                QCMsg = "Inproc. QA On " + Me.lblHeader.Text.Trim() + _
                        "<br/><br/>Subject: " + Me.ddlSubject.SelectedItem.Text.Trim() + _
                        "<br/><br/>QA: " + Me.RBLQCFlag.SelectedItem.Text.Trim() + _
                        "<br/><br/>QA Comments: " + Me.txtQCRemarks.Value + _
                        "<br/><br/>Comments Given By : " + Me.Session(S_FirstName).ToString.Trim() + " " + Me.Session(S_LastName).ToString.Trim()

                fromEmailId = ConfigurationSettings.AppSettings("Username")
                password = ConfigurationSettings.AppSettings("Password")


                wStr = "nEmailAlertId =" + Email_QCOFMedexINfoHdr.ToString() + " And cStatusIndi <> 'D'"
                If Not Me.objHelp.GetEmailAlertMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                    ds_EmailAlert, estr) Then

                    Me.objCommon.ShowAlert("Error While Getting Data From EmailAlert : " + estr, Me.Page)
                    Exit Sub

                End If

                If ds_EmailAlert.Tables(0).Rows.Count > 0 Then

                    toEmailId = Me.txtToEmailId.Text.Trim() 'ds_EmailAlert.Tables(0).Rows(0)("vToUserEmailId").ToString()
                    ccEmailId = Me.txtCCEmailId.Text.Trim() 'ds_EmailAlert.Tables(0).Rows(0)("vCCUserEmailId").ToString()
                    SubjectLine = ds_EmailAlert.Tables(0).Rows(0)("vSubjectLine").ToString()

                    Dim sn As New SendMail(Server, Request, Session, SubjectLine, QCMsg, fromEmailId, password, toEmailId, ccEmailId)
                    sn.Send(Server, Response, Session, , fromEmailId, password)
                    sn = Nothing
                    Me.objCommon.ShowAlert("QA Comments Saved Successfully", Me.Page)

                Else
                    Me.objCommon.ShowAlert("QA Comments Saved Successfully But Email Is Not Sent Due To Some Reason", Me.Page)
                End If

            Else 'For Response

                If Not Me.objLambda.Save_MedExInfoHdrQC(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                        ds_QC, Me.Session(S_UserID), estr) Then

                    Me.objCommon.ShowAlert(estr, Me.Page)
                    Exit Sub

                End If

                Me.objCommon.ShowAlert("Response Saved Successfully", Me.Page)

            End If

            Me.txtQCRemarks.Value = ""
            Me.lblResponse.Text = ""
            Me.RBLQCFlag.SelectedValue = "F"

            Me.txtToEmailId.Text = ""
            Me.txtCCEmailId.Text = ""

            Me.Session.Remove("PlaceMedEx")

            If Not IsNothing(Me.Request.QueryString("QC")) AndAlso Me.Request.QueryString("QC") = "Y" Then
                If Not fillQCGrid() Then
                    Exit Sub
                End If

            End If

            '***********************
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub BtnQCSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnQCSave.Click
        Dim estr As String = ""
        Dim SubjectId As String = ""
        Dim RedirectStr As String = ""
        Dim WorkspaceId As String = ""
        Dim ActivityId As String = ""
        Dim NodeId As String = ""
        Dim PeriodId As String = ""
        Dim MySubjectNo As String = ""
        Dim DsSave As New DataSet

        Dim TranNo_Retu As String = ""
        Dim FolderPath As String = ""

        Dim QCMsg As String = ""

        Dim ds_QC As New DataSet

        Try
            If Me.lblResponse.Text = "" And CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                Me.objCommon.ShowAlert("Please Click Response Button Of Particular QA Comments Against Which You Want To Response", Me.Page)

                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowHide", "QCDivShowHide('ST');", True)

                Exit Sub
            End If

            SubjectId = Me.HFSubjectId.Value.Trim()
            WorkspaceId = Me.HFWorkspaceId.Value.Trim()
            ActivityId = Me.HFActivityId.Value.Trim()
            NodeId = Me.HFNodeId.Value.Trim()
            PeriodId = Me.HFPeriodId.Value.Trim()
            MySubjectNo = Me.HFMySubjectNo.Value.Trim()

            'Added for QC Comments

            If Not AssignValues_MedExInfoHdrQC(SubjectId, WorkspaceId, ActivityId, NodeId, PeriodId, MySubjectNo, ds_QC) Then
                Me.objCommon.ShowAlert("Error While Assigning Data.", Me.Page)
                Exit Sub
            End If

            If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                If Not Me.objLambda.Save_MedExInfoHdrQC(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_QC, Me.Session(S_UserID), estr) Then

                    Me.objCommon.ShowAlert(estr, Me.Page)
                    Exit Sub

                End If

                Me.objCommon.ShowAlert("QA Comments Saved Successfully", Me.Page)

            Else 'For Response

                If Not Me.objLambda.Save_MedExInfoHdrQC(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                        ds_QC, Me.Session(S_UserID), estr) Then

                    Me.objCommon.ShowAlert(estr, Me.Page)
                    Exit Sub

                End If

                Me.objCommon.ShowAlert("Response Saved Successfully", Me.Page)

            End If

            Me.txtQCRemarks.Value = ""
            Me.lblResponse.Text = ""
            Me.RBLQCFlag.SelectedValue = "F"

            Me.Session.Remove("PlaceMedEx")

            If Not IsNothing(Me.Request.QueryString("QC")) AndAlso Me.Request.QueryString("QC") = "Y" Then
                If Not fillQCGrid() Then
                    Exit Sub
                End If

            End If

            '***********************
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "GVQCDtl Grid Events"
    Protected Sub GVQCDtl_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVQCDtl.RowCreated
        e.Row.Cells(GVCQC_nMedexInfoHdrQcNo).Visible = False
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
        Dim Wstr As String = ""
        Dim estr As String = ""
        Dim ds_MEdexInfroHdrQC As New DataSet

        If e.CommandName.ToUpper.Trim() = "RESPONSE" Then

            Me.lblResponse.Text = "Response To: " + Me.GVQCDtl.Rows(index).Cells(GVCQC_QCComment).Text.Trim()

            Wstr = "nMedexInfoHdrQcNo=" + Me.GVQCDtl.Rows(index).Cells(GVCQC_nMedexInfoHdrQcNo).Text.Trim()

            If Not Me.objHelp.GetMedexInfoHdrQC(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    ds_MEdexInfroHdrQC, estr) Then

                Me.objCommon.ShowAlert(estr, Me.Page)
                Exit Sub

            End If

            Me.ViewState(VS_DtQC) = ds_MEdexInfroHdrQC.Tables(0)

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowHide", "QCDivShowHide('ST');", True)

        End If

    End Sub

#End Region

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click

        'Dim ds As New DataSet
        'Dim estr As String = ""
        'Dim dt_top As New DataTable
        'Dim SubjectId As String = ""
        'Dim RedirectStr As String = ""
        'Dim WorkspaceId As String = ""
        'Dim ActivityId As String = ""
        'Dim NodeId As String = ""
        'Dim PeriodId As String = ""
        'Dim MySubjectNo As String = ""
        'Dim DsSave As New DataSet

        'Dim TranNo_Retu As String = ""
        'Dim FolderPath As String = ""


        'Dim ObjId As String = ""
        'Dim wStr As String = ""

        'Dim ds_WorkSpaceNodeHistory As New DataSet
        'Dim NoOfCopies As Integer
        'Dim iCopy As Integer
        'Dim TranNos As String = ""

        'Try

        '    'Val = Me.ViewState(VS_WorkSpaceSubjectId) 'Value of where condition
        '    SubjectId = Me.HFSubjectId.Value.Trim()
        '    WorkspaceId = Me.HFWorkspaceId.Value.Trim()
        '    ActivityId = Me.HFActivityId.Value.Trim()
        '    NodeId = Me.HFNodeId.Value.Trim()
        '    PeriodId = Me.HFPeriodId.Value.Trim()
        '    MySubjectNo = Me.HFMySubjectNo.Value.Trim()


        '    NoOfCopies = CType(FindControl(GeneralModule.Medex_NoOfCopies), TextBox).Text

        '    For iCopy = 1 To NoOfCopies
        '        If Not AssignValues(SubjectId, WorkspaceId, ActivityId, NodeId, PeriodId, MySubjectNo, DsSave) Then
        '            Me.objCommon.ShowAlert("Error While Assigning Data.", Me.Page)
        '            Exit Sub
        '        End If

        '        ds = DsSave

        '        If Not Me.objLambda.Save_MedExInfoHdrDtl(Me.ViewState(VS_Choice), ds, False, Me.Session(S_UserID), TranNo_Retu, estr) Then

        '            Me.objCommon.ShowAlert(estr, Me.Page)
        '            Exit Sub

        '        End If
        '        TranNos += IIf(TranNos.Trim() = "", "", ",") & TranNo_Retu

        '    Next iCopy


        '    Me.Response.Redirect("WordDoc.aspx?WorkSpaceId=" & Me.DDLWorkspace.SelectedValue.Trim() & "&TranNo=" & _
        '                TranNos & "&ActivityId=" & Me.HFActivityId.Value.Trim() & "&SubjectId=" & _
        '                Me.HFSubjectId.Value.Trim() & "&NodeId=" & Me.HFNodeId.Value.Trim() & _
        '                "&PeriodId=" & Me.HFPeriodId.Value.Trim())


        'Catch ex As Exception
        '    'addheaderfooter(headerText, footerText, fileNameDest)
        '    Me.ShowErrorMessage(ex.ToString, "")
        'End Try
    End Sub


    Protected Sub btnHistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHistory.Click
        Dim MedexGroupCode As String = ""

        If Not fillGrid(MedexGroupCode) Then
            Exit Sub
        End If

        'ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv", "DisplayDiv('Div" & MedexGroupCode & _
        '                                       "','" & Me.ViewState(VS_MedexGroups) & "');", True)

        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ShowDiv", "HistoryDivShowHide('SN','','Div" & MedexGroupCode & _
                                               "','" & Me.ViewState(VS_MedexGroups) & "');", True)
    End Sub

#Region "fillGrid"

    Private Function fillGrid(ByRef MedexGroupCode As String) As Boolean
        Dim ds_AuditTrail As New DataSet
        Dim dv_AuditTrail As New DataView
        Dim Wstr As String = ""
        Dim estr As String = ""

        Try

            'CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Me.HFMedexHdrNo.Value.Trim() = "" Then
                Me.objCommon.ShowAlert("No History Available", Me.Page())
                Exit Function
            End If

            Wstr = "vSubjectId='" & Me.HFSubjectId.Value.Trim() & "' And vMedexCode='" & Me.hfMedexCode.Value.Trim() & "' And " & _
                    " nMedExInfoHdrNo=" & Me.HFMedexHdrNo.Value.Trim() & " And iNodeId=" & Me.HFNodeId.Value.Trim()

            If Not Me.objHelp.View_MedExInfoHdrDtlAuditTrail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                             ds_AuditTrail, estr) Then

                Exit Function
            End If

            Me.lblMedexDescription.Text = ds_AuditTrail.Tables(0).Rows(0).Item("vMedExDesc").ToString.Trim()

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

    Protected Sub btnContinueWorking_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContinueWorking.Click
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ResetSessionTimer", "btnContinueWorking_Click();", True)
    End Sub

End Class