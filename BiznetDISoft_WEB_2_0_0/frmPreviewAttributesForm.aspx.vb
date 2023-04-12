Imports System.Text
Imports System.IO
Imports AjaxControlToolkit



Partial Class frmPreviewAttributesForm
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

    Private Const VS_dtMedEx_Fill As String = "dt_MedEx_Fill"
    'Private Const VS_Controls As String = "VsControls"
    Private Const VS_MedexGroups As String = "MedexGroups"
    Private Const VS_dtid As String = " "

    Private Is_ComboGlobalDictionary As Boolean = False
    Private Is_FormulaEnabled As Boolean = False


#End Region

#Region "Page Load"

    'Add by shivani pandya
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Try
            ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "assignCSS", "  assignCSS();", True)
        Catch ex As Exception
            Throw New Exception("Error while Page_Init()")
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.HFWorkspaceId.Value = String.Empty
            Me.HFActivityId.Value = String.Empty
            Me.HFTemplateId.Value = String.Empty
            Me.HFWorkspaceHdrScreeningNo.Value = String.Empty

            If Not IsNothing(Me.Request.QueryString("WorkSpaceId")) Then
                Me.HFWorkspaceId.Value = Me.Request.QueryString("WorkSpaceId").ToString
            End If
            If Not IsNothing(Me.Request.QueryString("WorkspaceScreeningHdrNo")) Then
                Me.HFWorkspaceHdrScreeningNo.Value = Me.Request.QueryString("WorkspaceScreeningHdrNo").ToString
            End If
            If Not IsNothing(Me.Request.QueryString("ScreeningTemplateHdrno")) Then
                Me.HFScreeningTemplateHdrNo.Value = Me.Request.QueryString("ScreeningTemplateHdrno").ToString
            End If
            If Not IsNothing(Me.Request.QueryString("ActivityId")) Then
                Me.HFActivityId.Value = Me.Request.QueryString("ActivityId").ToString
            End If
            If Not IsNothing(Me.Request.QueryString("TemplateId")) Then
                Me.HFTemplateId.Value = Me.Request.QueryString("TemplateId").ToString
            End If
            If Not IsNothing(Me.Request.QueryString("iNodeId")) Then
                Me.HFNodeId.Value = Me.Request.QueryString("iNodeId").ToString
            End If

            Me.lblUserName.Text = Session(S_FirstName).ToString.Trim() + " " + Session(S_LastName).ToString.Trim()
            Me.lblTime.Text = Session(S_Time).ToString
            'Me.lblTime.Text = Convert.ToString(CDate(Session(S_Time)).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)


            If Not GenCall() Then
                Exit Sub
            End If

            If Page.IsPostBack Then
                btnContinueWorking_Click(Nothing, Nothing)
                Exit Sub
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
        End Try
    End Sub

#End Region

#Region "Fill Drop Dwon"
    Private Function fillLabels() As Boolean
        Dim ds As New DataSet
        Dim dv_MedexTemplate As New DataView
        Dim estr_retu As String = String.Empty
        Dim Wstr As String = String.Empty
        Try


            dv_MedexTemplate = Nothing
            dv_MedexTemplate = New DataView()

            Me.lblTemplateName.Text = ""

            Me.lblProject.Text = ""

            If Me.HFWorkspaceId.Value.ToString.Trim() <> "" Then

                Wstr = "vWorkSpaceId='" + Me.HFWorkspaceId.Value.Trim() + "' And vActivityId='" + Me.HFActivityId.Value.Trim() + "'"
                If Not objHelp.GetViewWorkSpaceNodeDetail(Wstr, ds, estr_retu) Then
                    Exit Function
                End If

                If ds.Tables(0).Rows.Count > 0 Then
                    Me.lblProject.Text = "Project: " + ds.Tables(0).Rows(0)("vWorkSpaceDesc")
                    Me.lblTemplateName.Text = "Activty Name: " + ds.Tables(0).Rows(0)("vActivityName")

                End If

            ElseIf Me.HFTemplateId.Value.ToString.Trim() <> "" Then

                Wstr = "vMedExTemplateId='" + Me.HFTemplateId.Value.Trim() + "'"

                If Not objHelp.GetMedExTemplateDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr_retu) Then
                    Exit Function
                End If

                dv_MedexTemplate = Nothing
                dv_MedexTemplate = New DataView()
                dv_MedexTemplate = ds.Tables(0).DefaultView.ToTable(True, "vTemplateName,vMedExTemplateId".Split(",")).DefaultView

                If ds.Tables(0).Rows.Count > 0 Then
                    Me.lblTemplateName.Text = "Template Name: " + dv_MedexTemplate.ToTable().Rows(0)("vTemplateName")
                End If


            End If

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, " .....fillLabels")
            Return False
        End Try
    End Function

#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean
        Try
            If Not fillLabels() Then
                Return False
            End If

            If Not GenCall_Data() Then
                Return False
            End If

            If Not GenCall_ShowUI() Then
                Return False
            End If

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....GenCall")
            Return False
        End Try


    End Function

#End Region

#Region "GenCall_Data "

    Private Function GenCall_Data() As Boolean
        Dim ds As New DataSet
        Dim ds_SubMedExMst As New DataSet
        Dim ds_SubMedex As New DataSet
        Dim estr_retu As String = String.Empty
        Dim strQuery As String = String.Empty
        Dim Wstr As String = String.Empty

        Dim ds_MedExInfoHdrDtl As New DataSet

        Try

            ds = Nothing
            ds = New DataSet

            If Me.HFWorkspaceId.Value.ToString <> "" Then

                If Me.HFWorkspaceHdrScreeningNo.Value.ToString <> "" Then
                    Wstr = "cActiveFlag<>'N' and vWorkSpaceId='" & Me.HFWorkspaceId.Value.Trim() & "' and nWorkspaceScreeningHdrNo =" & Me.HFWorkspaceHdrScreeningNo.Value.Trim()
                    If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                        Wstr += " And vMedExType <> 'IMPORT'"
                    End If
                    Wstr += " Order by iSeqNo"
                    If Not Me.objHelp.View_WorkspaceScreeningHdrDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr_retu) Then
                        Throw New Exception("Error While Getting Project specific Screening Template Detail ")
                    End If
                ElseIf Me.HFScreeningTemplateHdrNo.Value.ToString <> "" Then
                    Wstr = "cActiveFlag<>'N' and vWorkSpaceId='" & Me.HFWorkspaceId.Value.Trim() & "' and nScreeningTemplateHdrNo =" & Me.HFScreeningTemplateHdrNo.Value.Trim() & _
                    " And cStatusIndi <>'" & Status_Delete & "'"

                    If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                        Wstr += " And vMedExType <> 'IMPORT'"
                    End If

                    Wstr += " Order by iSeqNo"

                    If Not Me.objHelp.View_GeneralScreeningHdrDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr_retu) Then
                        Throw New Exception("Error While Getting Project specific Screening Template Detail ")
                    End If
                Else

                    Wstr = "cActiveFlag<>'N' and vWorkSpaceId='" & Me.HFWorkspaceId.Value.Trim() & "' and vActivityId='" & _
                            Me.HFActivityId.Value & "' And iNodeId= " & Me.HFNodeId.Value.Trim()

                    If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                        Wstr += " And vMedExType <> 'IMPORT'"
                    End If

                    Wstr += " Order by iSeqNo"

                    If Not Me.objHelp.GetViewMedExWorkSpaceDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr_retu) Then
                        Throw New Exception("Error While Getting Template Detail ")
                    End If

                End If

                Me.ViewState(VS_dtMedEx_Fill) = ds.Tables(0)

            ElseIf Me.HFTemplateId.Value.ToString.Trim() <> "" Then

                Wstr = "cActiveFlag<>'N' and vMedExTemplateId='" + Me.HFTemplateId.Value.Trim() + "'"

                If Me.Session(S_ScopeNo) <> Scope_ClinicalTrial Then
                    Wstr += " And vMedExType <> 'IMPORT'"
                End If

                Wstr += " Order by iSeqNo"

                If Not objHelp.GetMedExTemplateDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr_retu) Then
                    Throw New Exception("Error While Getting Template Detail ")
                End If

                Me.ViewState(VS_dtMedEx_Fill) = ds.Tables(0)

            End If
            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message.ToString, estr_retu)
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
        Dim dt_heading As New DataTable
        Dim dv_MedexGroup As New DataView
        Dim dt_MedexGroup As New DataTable
        Dim i As Integer = 0
        Dim CntSubGroup As Integer = 0
        Dim PrevSubGroupCodes As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim ds_Heading As New DataSet
        Dim estr As String = String.Empty
        Dim ds_EmailAlert As New DataSet
        Dim objimage As Object
        Dim Copyright As String = String.Empty
        Dim clientname As String = String.Empty


        Try
            Copyright = System.Configuration.ConfigurationManager.AppSettings("CopyRight")

            Page.Title = " :: Subject Medical Examination :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            Me.CompanyName.Value = Copyright



            PlaceMedEx.Controls.Clear()

            If CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Rows.Count < 1 Then

                If Not IsPostBack Then
                    Me.objCommon.ShowAlert("No Attribute Is Attached With This Activity. So, please Attach Attribute To This Activity.", Me.Page)
                End If
                Exit Function

            End If

            dv_MedexGroup = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).DefaultView
            If dv_MedexGroup.ToTable.Columns.Contains("vWorkSpaceDesc") Then
                Me.lblProject.Text = "Project: " + dv_MedexGroup.ToTable.Rows(0)("vWorkSpaceDesc")
                Me.lblTemplateName.Text = "Activty Name: " + dv_MedexGroup.ToTable.Rows(0)("vActivityName")
            End If
            StrGroup(0) = "vMedExGroupCode"
            StrGroup(1) = "vMedExGroupDesc"
            dt_MedexGroup = dv_MedexGroup.ToTable(True, StrGroup)
            If dt_MedexGroup.Rows.Count > 1 Then
                Me.BtnNext.Visible = True
                Me.BtnPrevious.Visible = True
            End If
            'PlaceMedEx.Controls.Add(New LiteralControl("<br />")) '</Tr>
            'PlaceMedEx.Controls.Add(New LiteralControl("<Table  style=""width: 890px"">"))
            PlaceMedEx.Controls.Add(New LiteralControl("<Table  style=""width: 980px"">"))
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

            'PlaceMedEx.Controls.Add(New LiteralControl("<Td white-space: nowrap; vertical-align:top"">"))
            PlaceMedEx.Controls.Add(New LiteralControl("<Td   style="" width:980px; vertical-align:top"">"))
            Me.GetButtons(StrGroupDesc, StrGroupCodes)
            'Me.GetDropDownList(StrGroupDesc, StrGroupCodes)

            'Added on 27-Jul-2009
            If Not (Request.QueryString("iNodeId")) Is Nothing Then
                Me.BtnNext.OnClientClick = "return Next('" & StrGroupCodes & "');"
                Me.BtnPrevious.OnClientClick = "return Previous('" & StrGroupCodes & "');"
                Me.BtnNext.Style.Add("display", "")
                Me.BtnPrevious.Style.Add("display", "")
            Else
                Me.BtnNext.Style.Add("display", "none")
                Me.BtnPrevious.Style.Add("display", "none")
            End If
            '*******************************

            PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
            PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
            '**********************************

            '**********For user's readibility************************
            PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""100%"" height=""2px"" ALIGN=LEFT style=""BACKGROUND-COLOR: #FFFFFF"">"))
            PlaceMedEx.Controls.Add(New LiteralControl("<Td style="" vertical-align:middle"">"))
            PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
            PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
            '*************************

            PlaceMedEx.Controls.Add(New LiteralControl("<Tr ALIGN=LEFT >"))
            PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""vertical-align:top"">"))

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
                    PlaceMedEx.Controls.Add(New LiteralControl("<Div id='Div" & drGroup("vMedExGroupCode").ToString.Trim() & "' style=""display:block""" + " >"))
                    'Added on 27-Jul-2009
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "CurrentTab", "currTab='Div" & drGroup("vMedExGroupCode").ToString.Trim() & "';", True)
                    '***********************
                Else
                    PlaceMedEx.Controls.Add(New LiteralControl("<Div id='Div" & drGroup("vMedExGroupCode").ToString.Trim() & "' style=""display:none""" + " >")) 'Added for QC Comments on 22-May-2009
                End If

                'PlaceMedEx.Controls.Add(New LiteralControl("<Table width=""100%"" style=""border: solid 1px gray"">")) ' border=""1""
                PlaceMedEx.Controls.Add(New LiteralControl("<Table width=""970px"" style=""border: solid 1px gray"">")) 'Added on 30-01-2010 to fix the size of display
                For Each dr In dt_MedExMst.Rows

                    If PrevSubGroupCodes = "" Or PrevSubGroupCodes <> dr("vMedExSubGroupCode") Then
                        'PlaceMedEx.Controls.Add(New LiteralControl("<tr>"))
                        'PlaceMedEx.Controls.Add(New LiteralControl("<td>"))
                        'PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
                        'PlaceMedEx.Controls.Add(New LiteralControl("</td>"))
                        'PlaceMedEx.Controls.Add(New LiteralControl("</tr>"))
                        If Convert.ToString(dr("vMedExSubGroupCode")).Trim() <> "0000" Then 'Added for default SubGroup

                            PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""100%"" ALIGN=LEFT style=""BACKGROUND-COLOR: #58BDF7"">")) 'ffcc66
                            PlaceMedEx.Controls.Add(New LiteralControl("<Td colspan=""3"" style=""vertical-align:middle"">"))
                            PlaceMedEx.Controls.Add(Getlable(dr("vMedExSubGroupDesc").ToString.Trim(), dr("vMedExSubGroupCode") + CntSubGroup.ToString.Trim()))
                            CntSubGroup += 1
                            PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                            PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                        End If

                    End If

                    PlaceMedEx.Controls.Add(New LiteralControl("<Tr ALIGN=LEFT>"))

                    'If Convert.ToString(dr("vMedExType")).Trim.ToUpper() = "DATETIME" Then
                    '    PlaceMedEx.Controls.Add()
                    'End If

                    If Convert.ToString(dr("vMedExType")).Trim.ToUpper() <> "LABEL" Then
                        PlaceMedEx.Controls.Add(New LiteralControl("<Td style="" width:500px; vertical-align:middle"" ALIGN=LEFT>"))

                        PrevSubGroupCodes = dr("vMedExSubGroupCode")
                        PlaceMedEx.Controls.Add(GetlableWithHistory(dr("vMedExDesc") & ": ", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), dr("cIsVisibleFront")))
                        PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                        PlaceMedEx.Controls.Add(New LiteralControl("<Td style="" vertical-align:middle"" ALIGN=LEFT>"))
                    Else
                        PlaceMedEx.Controls.Add(New LiteralControl("<Td colspan=""2"" style="" vertical-align:middle"" ALIGN=LEFT>"))
                        PrevSubGroupCodes = dr("vMedExSubGroupCode")
                        'PlaceMedEx.Controls.Add(GetlableWithHistory(dr("vMedExDesc") & ": ", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode")))
                    End If

                    'PlaceMedEx.Controls.Add(New LiteralControl("<Td style="" width:500px; vertical-align:middle"" ALIGN=LEFT>"))

                    'PrevSubGroupCodes = dr("vMedExSubGroupCode")
                    'PlaceMedEx.Controls.Add(GetlableWithHistory(dr("vMedExDesc") & ": ", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode")))
                    'PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                    'PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""white-space: nowrap; vertical-align:middle"" ALIGN=LEFT>"))

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

                    If dr("vMedExType").ToString.ToUpper.Trim() = "FILE" AndAlso dr("vDefaultValue").ToString.Trim() <> "" Then
                        Dim HLnkFile As New HyperLink

                        HLnkFile.ID = "hlnk" + dr("vMedExCode").ToString.Trim()
                        HLnkFile.NavigateUrl = dr("vDefaultValue").ToString.Trim()
                        HLnkFile.Text = Path.GetFileName(dr("vDefaultValue").ToString.Trim())
                        HLnkFile.Target = "_blank"
                        PlaceMedEx.Controls.Add(HLnkFile)
                    End If
                    'for calendar image
                    If dr("vMedExType").ToString.ToUpper.Trim() = "DATETIME" Then
                        objimage = GetDateImage(dr("vMedExCode"), objelement)
                    End If

                    'For Print UOM 
                    If Not dr("vUOM") Is System.DBNull.Value AndAlso dr("vUOM").ToString.Trim() <> "" AndAlso dr("vUOM").ToString.Trim() <> "0" Then
                        PlaceMedEx.Controls.Add(Getlable(dr("vUOM"), "UOM" + dr("vMedExCode"), dr("vMedExType")))
                    End If
                    '*********************************

                    If Is_ComboGlobalDictionary Then
                        PlaceMedEx.Controls.Add(GetBrowseButton("Browse", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode")))
                        Is_ComboGlobalDictionary = False
                    End If

                    If Is_FormulaEnabled Then
                        PlaceMedEx.Controls.Add(GetAutoCalculateButton("Calc", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExCode"), dr("vMedExFormula")))
                        Is_FormulaEnabled = False
                    End If

                    PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                    'PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""white-space: nowrap; vertical-align:middle"" ALIGN=LEFT>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""white-space: nowrap "" ALIGN=""LEFT"">"))

                    PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                    '**********For User's readibility**************
                    PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""100%"" height=""2px"" ALIGN=LEFT style=""BACKGROUND-COLOR: #FFFFFF"">"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<Td colspan=""3"" style=""white-space: nowrap; vertical-align:middle"">"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                    PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""100%"" height=""1px"" ALIGN=LEFT style=""BACKGROUND-COLOR: #1560a1"">")) '#FFC300
                    PlaceMedEx.Controls.Add(New LiteralControl("<Td colspan=""3"" style=""white-space: nowrap; vertical-align:middle"">"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                    PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""100%"" height=""2px"" ALIGN=LEFT style=""BACKGROUND-COLOR: #FFFFFF"">"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<Td colspan=""3"" style=""white-space: nowrap; vertical-align:middle"">"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
                    '****************************************

                Next dr

                i += 1
                PlaceMedEx.Controls.Add(New LiteralControl("</Table>")) '</Tr>
                PlaceMedEx.Controls.Add(New LiteralControl("</Div>")) '</Tr>

            Next drGroup

            PlaceMedEx.Controls.Add(New LiteralControl("</Td>")) '</Tr>
            PlaceMedEx.Controls.Add(New LiteralControl("</Tr>")) '</Tr>
            PlaceMedEx.Controls.Add(New LiteralControl("</Table>"))
            Me.Session("PlaceMedEx") = PlaceMedEx.Controls

            dt_MedExMst = New DataTable
            dt_MedExMst = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable)

            Return True
        Catch ex As Exception
            'Added for QC Comments
            ShowErrorMessage(ex.Message.ToString, "...GenCall_ShowUI")
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
        lab.SkinID = "LabelDisplay" '' Added By Prayag For Micro Symbol Change to m
        lab.ForeColor = System.Drawing.Color.FromName("Navy")
        lab.Attributes.Add("Style", "font-size: 13px;font-weight: bold")

        If vFieldType.ToUpper.Trim() = "IMPORT" Then
            lab.Visible = False
        End If



        Getlable = lab
    End Function

    Private Function GetlableWithHistory(ByVal vlabelName As String, ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal MedExCode As String, ByVal cIsVisibleFront As String) As Label
        'Dim lnk As LinkButton
        'lnk = New LinkButton
        Dim lnk As Label
        lnk = New Label
        lnk.ID = "Lnk" & MedExGroupCode + MedExSubGroupCode + MedExCode
        lnk.Text = vlabelName.Trim()
        'lnk.SkinID = "lblDisplay"
        lnk.CssClass = "LabelText"

        If cIsVisibleFront = "N" Then
            lnk.ForeColor = System.Drawing.ColorTranslator.FromHtml("#de934a")
        End If

        'lnk.OnClientClick = "return HistoryDivShowHide('S','" + MedExCode + "');"

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

                If StrGroupDesc_arry(index).Trim().Length > 30 Then
                    Btn.Text = " " & StrGroupDesc_arry(index).Substring(0, 30).Trim() & "... "
                End If

                Btn.ToolTip = StrGroupDesc_arry(index).Trim()

                'Btn.Width = "230"
                'Btn.Font.Bold = True

                'Btn.Style.Add("BACKGROUND-IMAGE", "url(images/btn.jpg)")
                'Btn.Style.Add("BACKGROUND-IMAGE", "url(images/btn.gif)")
                'Btn.Style.Add("background-repeat", "no-repeat")
                'Btn.Style.Add("background-color", "#F2B040")

                'Btn.ForeColor = Drawing.Color.Brown
                'Btn.Style.Add("color", "#FFCC33")
                'Btn.CssClass = "~/App_Themes/StyleBlue/StyleBlue.css/buttonForTabActive"
                Btn.CssClass = "buttonForTabActive"

                If index = 0 Then
                    'Btn.Style.Add("color", "navy")
                    'Btn.Style.Add("color", "#FFffff")
                    Btn.CssClass = "buttonForTabInActive"

                    'Added on 13-Oct-2009
                    'ElseIf index Mod 12 = 0 Then
                ElseIf index Mod 5 = 0 Then
                    'PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                    'PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                    'PlaceMedEx.Controls.Add(New LiteralControl("<Tr ALIGN=LEFT >"))
                    'PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""white-space: nowrap; vertical-align:top"">"))

                End If
                '********************************************

                'Btn.BorderStyle = BorderStyle.None

                'this is to add event on perticuler Dynamic controls
                'AddHandler Btn.Click, AddressOf Me.BtnAdd_Click
                '***************************************************

                'Btn.CssClass = "TABButton"
                Btn.OnClientClick = "return DisplayDiv('" & StrGroupCode_arry(index) & "','" & Id & "');"
                'If Request.QueryString("iNodeId") = Nothing Then
                '    Btn.Visible = False
                'Else
                '    Btn.Visible = True
                'End If


                'GetButtons = Btn
                PlaceMedEx.Controls.Add(Btn)
                PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
                PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
            Next
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GetButtons")

        End Try
    End Function


     Private Function GetDropDownList(ByVal BtnName As String, ByVal Id As String) As Boolean
        Dim Btn As Button
        Dim index As Integer
        Dim StrGroupCode_arry() As String
        Dim StrGroupDesc_arry() As String
        Dim dtMedExMst As New DataTable
        Dim dvMedExMst As New DataView
        ddlGroup.Items.Clear()
        Try
            StrGroupCode_arry = Id.Split(",")
            StrGroupDesc_arry = BtnName.Split(",")
            Dim ddl As DropDownList
            ddl = New DropDownList

            ddlGroup.Items.Add(New ListItem("--- Select Group ---", "Div00000", True))
            For index = 0 To StrGroupCode_arry.Length - 1
                If (index = 1) Then
                    ddlGroup.Items.Add(New ListItem(StrGroupDesc_arry(index), StrGroupCode_arry(index), True))
                    ddlGroup.Attributes.Add("onchange", "return DisplayDivFromDDL('" & StrGroupCode_arry(index) & "','" & Id & "');")
                Else
                    ddlGroup.Items.Add(New ListItem(StrGroupDesc_arry(index), StrGroupCode_arry(index)))
                    ddlGroup.Attributes.Add("onchange", "return DisplayDivFromDDL('" & "this.id" & "','" & Id & "');")
                End If


                ddlGroup.CssClass = "dropDownList"
                If (StrGroupCode_arry(index).Replace("Div", "") = Request.QueryString("Group")) Then
                    ddlGroup.SelectedValue = StrGroupCode_arry(index)
                End If

            Next

            PlaceMedEx.Controls.Add(ddlGroup)
            ddlGroup.SelectedIndex = 1
            ddlGroup.Style.Add("display", "")
            'ddlGroup.Style.Add("margin-left", "-151px;")
            If Request.QueryString("iNodeId") <> Nothing Then
                ddlGroup.Visible = False
            Else
                ddlGroup.Visible = True
            End If


            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GetButtons")
            Return False
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
        Dim Hf As HiddenField
        Dim lbl As Label



        Select Case vFieldType.ToUpper.Trim()
            Case "TEXTBOX"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                txt.Text = dtValues
                txt.Attributes.Add("title", dtValues)
                'For Validation Only ****************
                If length <> "" And length <> "0" Then
                    'txt.MaxLength = length
                End If

                HighRange = IIf(HighRange = "", "0", HighRange)
                LowRange = IIf(LowRange = "", "0", LowRange)

                Dim checktype As String = ""
                Dim msg As String = ""

                If Validation = "" Or Validation = "NA" Then
                    'txt.Attributes.Add("onblur", "ValidateTextbox(0,this,'No Validation'," & HighRange & "," & LowRange & ");")
                    checktype = "0"
                    msg = "No Validation"
                ElseIf Validation = GeneralModule.Val_AN Then
                    'txt.Attributes.Add("onblur", "ValidateTextbox(" & GeneralModule.Validation_Alphanumeric & ",this,'Please Enter AlphaNumeric Value'," & _
                    '                                                HighRange & "," & LowRange & ");")
                    checktype = GeneralModule.Validation_Alphanumeric.ToString()
                    msg = "Please Enter AlphaNumeric Value"
                ElseIf Validation = GeneralModule.Val_NU Then
                    'txt.Attributes.Add("onblur", "ValidateTextbox(" & GeneralModule.Validation_Numeric & ",this,'Please Enter Numeric or Blank Value'," & _
                    '                                               HighRange & "," & LowRange & ");")
                    checktype = GeneralModule.Validation_Numeric.ToString()
                    msg = "Please Enter Numeric or Blank Value"
                ElseIf Validation = GeneralModule.Val_IN Then
                    'txt.Attributes.Add("onblur", "ValidateTextbox(" & GeneralModule.Validation_Integer & ",this,'Please Enter Integer or Blank Value'," & _
                    '                                                HighRange & "," & LowRange & ");")
                    checktype = GeneralModule.Validation_Integer.ToString()
                    msg = "Please Enter Integer or Blank Value"
                ElseIf Validation = GeneralModule.Val_AL Then
                    'txt.Attributes.Add("onblur", "ValidateTextbox('" & GeneralModule.Validation_Alphabet & "',this,'Please Enter Alphabets only'," & _
                    '                                                HighRange & "," & LowRange & ");")
                    checktype = GeneralModule.Validation_Alphabet.ToString()
                    msg = "Please Enter Alphabets only"
                ElseIf Validation = GeneralModule.Val_NNI Then
                    'txt.Attributes.Add("onblur", "ValidateTextbox('" & GeneralModule.Validation_NotNullInteger & "',this,'Please Enter Integer Value'," & _
                    '                                                HighRange & "," & LowRange & ");")
                    checktype = GeneralModule.Validation_NotNullInteger.ToString()
                    msg = "Please Enter Integer Value"
                ElseIf Validation = GeneralModule.Val_NNU Then
                    'txt.Attributes.Add("onblur", "ValidateTextbox('" & GeneralModule.Validation_NotNullNumeric & "',this,'Please Enter Numeric Value'," & _
                    '                                                HighRange & "," & LowRange & ");")
                    checktype = GeneralModule.Validation_NotNullNumeric.ToString()
                    msg = "Please Enter Numeric Value"
                End If

                If AlertonValue.Trim() <> "" OrElse length.Trim() <> "" Then
                    checktype = "0"
                End If

                txt.Attributes.Add("onblur", "ValidateTextbox('" & checktype & "',this,'" & msg & "'," & _
                                           HighRange & "," & LowRange & ",'" & AlertonValue & "','" & AlertMsg & "','" & length & "');")


                'Added on 23-Sep-2009
                If Id = GeneralModule.Medex_Oral_Body_Temperature_F Then

                    txt.Attributes.Add("onblur", "F2C('" & GeneralModule.Medex_Oral_Body_Temperature_F.Trim() & "','" & GeneralModule.Medex_Oral_Body_Temperature_C.Trim() & "');")

                    'ElseIf Id = GeneralModule.Medex_Oral_Body_Temperature_C Then

                    '    txt.Attributes.Add("onblur", "C2F('" & GeneralModule.Medex_Oral_Body_Temperature_C.Trim() & "','" & GeneralModule.Medex_Oral_Body_Temperature_F.Trim() & "');")

                End If
                '************************************

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
                        ddl.Items(i).Attributes.Add("title", Arrvalue(i).Trim())
                    Next

                    If Not dtValues = "" Then
                        ddl.SelectedValue = dtValues.Trim()
                        ddl.Attributes.Add("title", dtValues)
                    End If

                End If

                'Adde by Naimesh Dave on 28-Nov-2008 for Alert on Value as suggested by Nikur Sir
                If AlertonValue.Trim() <> "" Then
                    ddl.Attributes.Add("onchange", "ddlAlerton('" & ddl.ClientID & "','" & AlertonValue & "','" & AlertMsg & "');")
                End If
                '*********************************

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

                RBL.RepeatDirection = System.Web.UI.WebControls.RepeatDirection.Horizontal
                RBL.RepeatColumns = 3

                'Adde by Naimesh Dave on 28-Nov-2008 for Alert on Value as suggested by Nikur Sir
                If AlertonValue.Trim() <> "" Then
                    RBL.Attributes.Add("onclick", "Alerton('" & RBL.ClientID & "','" & AlertonValue & "','" & AlertMsg & "');")
                    'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "AlertOn", _
                    '                        "Alerton('" & RBL.ClientID & "','" & AlertonValue & "','" & AlertMsg & "');", True)
                End If
                '*********************************

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

                CBL.RepeatDirection = System.Web.UI.WebControls.RepeatDirection.Horizontal
                CBL.RepeatColumns = 3

                If AlertonValue.Trim() <> "" Then
                    CBL.Attributes.Add("onclick", "AlertonCheckBox(this,'" & AlertonValue & "','" & AlertMsg & "');")
                End If
                ''" & CBL.ClientID & "'
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

                txt.Attributes.Add("title", dtValues)

                GetObject = txt

            Case "DATETIME"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                txt.Attributes.Add("onchange", "DateValidationForCTM(this.value,this,'Y')")
                GetObject = txt

            Case "TIME"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                txt.Attributes.Add("title", dtValues)
                'Commented as per the requirement of CPMA dept on 06-Oct-2009
                'txt.Text = IIf(dtValues = "" And Me.ViewState(VS_SetDefault) = "YES", DateTime.Now.ToString("HH:mm:ss"), dtValues)
                txt.Text = dtValues

                'If dtValues = "" AndAlso Me.HFSubjectId.Value.Trim() = "0000" Then
                '    txt.Text = ""
                'End If
                '****************************************************

                txt.Attributes.Add("onblur", "TimeConvert(this.value,this)")

                GetObject = txt
            Case "ASYNCDATETIME"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                txt.Text = System.DateTime.Now.ToString("DD-MMM-YYYY")
                txt.Attributes.Add("title", System.DateTime.Now.ToString("DD-MMM-YYYY"))

                'txt.Attributes.Add("onblur", "DateConvert(this.value,this)")
                txt.Attributes.Add("onblur", "DateValidationForCTM(this.value,this,'Y')")

                GetObject = txt

            Case "ASYNCTIME"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                txt.Attributes.Add("title", dtValues)
                'Commented as per the requirement of CPMA dept on 06-Oct-2009
                'txt.Text = IIf(dtValues = "" And Me.ViewState(VS_SetDefault) = "YES", DateTime.Now.ToString("HH:mm:ss"), dtValues)
                txt.Text = dtValues

                'If dtValues = "" AndAlso Me.HFSubjectId.Value.Trim() = "0000" Then
                '    txt.Text = ""
                'End If
                '****************************************************

                txt.Attributes.Add("onblur", "TimeConvert(this.value,this)")

                GetObject = txt
            Case "IMPORT"
                Hf = New HiddenField
                Hf.ID = Id
                Hf.Value = dtValues.Trim()

                GetObject = Hf

            Case "COMBOGLOBALDICTIONARY"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                txt.Text = dtValues
                txt.Attributes.Add("title", dtValues)
                Is_ComboGlobalDictionary = True
                GetObject = txt

            Case "FORMULA"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                txt.Text = dtValues
                txt.Attributes.Add("title", dtValues)

                HighRange = IIf(HighRange = "", "0", HighRange)
                LowRange = IIf(LowRange = "", "0", LowRange)

                Dim checktype As String = ""
                Dim msg As String = ""

                If Validation = "" Or Validation = "NA" Then
                    checktype = "0"
                    msg = "No Validation"
                ElseIf Validation = GeneralModule.Val_AN Then
                    checktype = GeneralModule.Validation_Alphanumeric.ToString()
                    msg = "Please Enter AlphaNumeric Value"
                ElseIf Validation = GeneralModule.Val_NU Then
                    checktype = GeneralModule.Validation_Numeric.ToString()
                    msg = "Please Enter Numeric or Blank Value"
                ElseIf Validation = GeneralModule.Val_IN Then
                    checktype = GeneralModule.Validation_Integer.ToString()
                    msg = "Please Enter Integer or Blank Value"
                ElseIf Validation = GeneralModule.Val_AL Then
                    checktype = GeneralModule.Validation_Alphabet.ToString()
                    msg = "Please Enter Alphabets only"
                ElseIf Validation = GeneralModule.Val_NNI Then
                    checktype = GeneralModule.Validation_NotNullInteger.ToString()
                    msg = "Please Enter Integer Value"
                ElseIf Validation = GeneralModule.Val_NNU Then
                    checktype = GeneralModule.Validation_NotNullNumeric.ToString()
                    msg = "Please Enter Numeric Value"
                End If

                If AlertonValue.Trim() <> "" OrElse length.Trim() <> "" Then
                    checktype = "0"
                End If

                txt.Attributes.Add("onblur", "ValidateTextbox('" & checktype & "',this,'" & msg & "'," & _
                                           HighRange & "," & LowRange & ",'" & AlertonValue & "','" & AlertMsg & "','" & length & "');")


                'Added on 23-Sep-2009
                If Id = GeneralModule.Medex_Oral_Body_Temperature_F Then

                    txt.Attributes.Add("onblur", "F2C('" & GeneralModule.Medex_Oral_Body_Temperature_F.Trim() & "','" & GeneralModule.Medex_Oral_Body_Temperature_C.Trim() & "');")

                    'ElseIf Id = GeneralModule.Medex_Oral_Body_Temperature_C Then

                    '    txt.Attributes.Add("onblur", "C2F('" & GeneralModule.Medex_Oral_Body_Temperature_C.Trim() & "','" & GeneralModule.Medex_Oral_Body_Temperature_F.Trim() & "');")

                End If
                '************************************

                Is_FormulaEnabled = True
                GetObject = txt

            Case "LABEL"

                lbl = New Label
                lbl.ID = "lbl" + Id
                lbl.EnableViewState = True
                lbl.CssClass = "Label"
                'lbl.Width = "500"
                'lbl.Attributes.Add("word-wrap", "break-word")
                lbl.Text = vMedExValues.Trim()
                lbl.Attributes.Add("title", vMedExValues.Trim())
                GetObject = lbl
            Case "CRFTERM"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                txt.Text = dtValues


                GetObject = txt

            Case "STANDARDDATE"
                Dim ddlDate As New DropDownList
                Dim ddlMonth As New DropDownList
                Dim ddlYear As New DropDownList
                Dim str As String = String.Empty
                Dim estr As String = String.Empty
                Dim dsDate As New DataSet

                ddlDate.ID = Id + "_1"
                ddlDate.CssClass = "dropDownList"
                ddlDate.Width = 80
                ddlMonth.ID = Id + "_2"
                ddlMonth.CssClass = "dropDownList"
                ddlMonth.Width = 80
                ddlYear.ID = Id + "_3"
                ddlYear.CssClass = "dropDownList"
                ddlYear.Width = 80

                ddlYear.Attributes.Add("StandardDate", "Y")
                ddlMonth.Attributes.Add("StandardDate", "Y")
                ddlDate.Attributes.Add("StandardDate", "Y")
               

                If Not objHelp.GetDatesMonthsAndYears("PROC_GetDatesMonthsAndYears", dsDate, estr) Then
                    Throw New Exception("Error While Getting Dates,Months and Years.")
                End If

                ddlDate.DataSource = dsDate.Tables(0)
                ddlDate.DataTextField = "Dates"
                ddlDate.DataValueField = "Dates"
                ddlDate.DataBind()
                ddlDate.Items.Insert(0, New ListItem("DD", ""))
                ddlDate.Items.Insert(1, New ListItem("UK", "00"))

                ddlMonth.DataSource = dsDate.Tables(1)
                ddlMonth.DataTextField = "Months"
                ddlMonth.DataValueField = "Val"
                ddlMonth.DataBind()
                ddlMonth.Items.Insert(0, New ListItem("MMM", ""))
                ddlMonth.Items.Insert(1, New ListItem("UNK", "00"))

                ddlYear.DataSource = dsDate.Tables(2)
                ddlYear.DataTextField = "Years"
                ddlYear.DataValueField = "Years"
                ddlYear.DataBind()
                ddlYear.Items.Insert(0, New ListItem("YYYY", ""))
                ddlYear.Items.Insert(1, New ListItem("UKUK", "0000"))



                If Not dtValues = "" Then
                    ddlDate.SelectedValue = Split(dtValues.ToString, "-")(0)
                    ddlDate.Attributes.Add("title", Split(dtValues.ToString, "-")(0))
                    ddlMonth.SelectedValue = Split(dtValues.ToString, "-")(1)
                    ddlMonth.Attributes.Add("title", Split(dtValues.ToString, "-")(1))
                    ddlYear.SelectedValue = Split(dtValues.ToString, "-")(2)
                    ddlYear.Attributes.Add("title", Split(dtValues.ToString, "-")(2))
                End If

                ddlDate.Attributes.Add("onchange", "CheckStandardDate(this,'" & ddlDate.ClientID & "','" & ddlMonth.ClientID & "','" & ddlYear.ClientID & "');")
                ddlMonth.Attributes.Add("onchange", "CheckStandardDate(this,'" & ddlDate.ClientID & "','" & ddlMonth.ClientID & "','" & ddlYear.ClientID & "');")
                ddlYear.Attributes.Add("onchange", "CheckStandardDate(this,'" & ddlDate.ClientID & "','" & ddlMonth.ClientID & "','" & ddlYear.ClientID & "');")


                PlaceMedEx.Controls.Add(ddlDate)
                PlaceMedEx.Controls.Add(ddlMonth)
                
                GetObject = ddlYear

            Case "STANDARDDATETIME"
                Dim ddlDate As New DropDownList
                Dim ddlMonth As New DropDownList
                Dim ddlYear As New DropDownList
                Dim ddlHH As New DropDownList
                Dim ddlMM As New DropDownList
                Dim str As String = String.Empty
                Dim estr As String = String.Empty
                Dim dsDate As New DataSet

                ddlDate.ID = Id + "_1"
                ddlDate.CssClass = "dropDownList"
                ddlDate.Width = 80
                ddlMonth.ID = Id + "_2"
                ddlMonth.CssClass = "dropDownList"
                ddlMonth.Width = 80
                ddlYear.ID = Id + "_3"
                ddlYear.CssClass = "dropDownList"
                ddlYear.Width = 80

                ddlHH.CssClass = "dropDownList"
                ddlHH.Width = 80
                ddlMM.CssClass = "dropDownList"
                ddlMM.Width = 80


                ddlYear.Attributes.Add("StandardDate", "Y")
                ddlMonth.Attributes.Add("StandardDate", "Y")
                ddlDate.Attributes.Add("StandardDate", "Y")
                ddlHH.Attributes.Add("StandardDate", "Y")
                ddlMM.Attributes.Add("StandardDate", "Y")

                If Not objHelp.GetDatesMonthsAndYears("PROC_GetDatesMonthsYearsHouresAndTime", dsDate, estr) Then
                    Throw New Exception("Error While Getting Dates,Months,Years, Hours And Minutes.")
                End If

                ddlDate.DataSource = dsDate.Tables(0)
                ddlDate.DataTextField = "Dates"
                ddlDate.DataValueField = "Dates"
                ddlDate.DataBind()
                ddlDate.Items.Insert(0, New ListItem("DD", ""))
                ddlDate.Items.Insert(1, New ListItem("UK", "00"))

                ddlMonth.DataSource = dsDate.Tables(1)
                ddlMonth.DataTextField = "Months"
                ddlMonth.DataValueField = "Val"
                ddlMonth.DataBind()
                ddlMonth.Items.Insert(0, New ListItem("MMM", ""))
                ddlMonth.Items.Insert(1, New ListItem("UNK", "00"))

                ddlYear.DataSource = dsDate.Tables(2)
                ddlYear.DataTextField = "Years"
                ddlYear.DataValueField = "Years"
                ddlYear.DataBind()
                ddlYear.Items.Insert(0, New ListItem("YYYY", ""))
                ddlYear.Items.Insert(1, New ListItem("UKUK", "0000"))


                ddlHH.DataSource = dsDate.Tables(3)
                ddlHH.DataTextField = "HH"
                ddlHH.DataValueField = "HH"
                ddlHH.DataBind()
                ddlHH.Items.Insert(0, New ListItem("HH", ""))

                ddlMM.DataSource = dsDate.Tables(4)
                ddlMM.DataTextField = "MM"
                ddlMM.DataValueField = "MM"
                ddlMM.DataBind()
                ddlMM.Items.Insert(0, New ListItem("MM", ""))


                If Not dtValues = "" Then
                    ddlDate.SelectedValue = Split(dtValues.ToString, "-")(0)
                    ddlDate.Attributes.Add("title", Split(dtValues.ToString, "-")(0))
                    ddlMonth.SelectedValue = Split(dtValues.ToString, "-")(1)
                    ddlMonth.Attributes.Add("title", Split(dtValues.ToString, "-")(1))
                    ddlYear.SelectedValue = Split(dtValues.ToString, "-")(2)
                    ddlYear.Attributes.Add("title", Split(dtValues.ToString, "-")(2))

                    If Split(dtValues.ToString, "-").Length > 3 Then
                        If Split(dtValues.ToString, "-")(3) <> "" Then
                            ddlHH.SelectedValue = Split(dtValues.ToString, "-")(3)
                        End If
                        If Split(dtValues.ToString, "-").Length > 4 Then
                            If Split(dtValues.ToString, "-")(4) <> "" Then
                                ddlMM.SelectedValue = Split(dtValues.ToString, "-")(4)
                            End If
                        End If

                    End If

                End If

                ddlDate.Attributes.Add("onchange", "CheckStandardDate(this,'" & ddlDate.ClientID & "','" & ddlMonth.ClientID & "','" & ddlYear.ClientID & "');")
                ddlMonth.Attributes.Add("onchange", "CheckStandardDate(this,'" & ddlDate.ClientID & "','" & ddlMonth.ClientID & "','" & ddlYear.ClientID & "');")
                ddlYear.Attributes.Add("onchange", "CheckStandardDate(this,'" & ddlDate.ClientID & "','" & ddlMonth.ClientID & "','" & ddlYear.ClientID & "');")


                PlaceMedEx.Controls.Add(ddlDate)
                PlaceMedEx.Controls.Add(ddlMonth)
                PlaceMedEx.Controls.Add(ddlYear)
                PlaceMedEx.Controls.Add(ddlHH)


                GetObject = ddlMM

            Case Else
                Return Nothing
        End Select
    End Function

#End Region
    'Added By Debashis Sahoo for calendar image(07-01-2013)
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
    '============================================
#Region "Show Error Message"
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.FindControl("lblErrorMsg"), Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblErrorMsg"), Label).Text = exMessage + "<BR> " + eStr
        objcommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
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

    Private Function GetBrowseButton(ByVal vButtonName As String, ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal MedExCode As String) As Button
        Dim btn As Button
        btn = New Button
        btn.ID = "btnBrowse" & MedExGroupCode + MedExSubGroupCode + MedExCode
        btn.Text = vButtonName.Trim()
        btn.CssClass = "button"
        btn.Style.Add("display", "block")
        btn.OnClientClick = "return MeddraBrowser('" + MedExCode + "');"
        GetBrowseButton = btn
    End Function

    Private Function GetAutoCalculateButton(ByVal vButtonName As String, ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal MedExCode As String, ByVal MedExFormula As String) As Button
        Dim btn As Button
        btn = New Button
        btn.ID = "btnAutoCalculate" & MedExGroupCode + MedExSubGroupCode + MedExCode
        btn.Text = vButtonName.Trim()
        btn.CssClass = "button"
        btn.OnClientClick = "return MedExFormula('" + MedExCode + "','" + MedExFormula + "');"
        GetAutoCalculateButton = btn
    End Function

    Protected Sub btnContinueWorking_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContinueWorking.Click
        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "ResetSessionTimer", "btnContinueWorking_Click();", True)
    End Sub

End Class
