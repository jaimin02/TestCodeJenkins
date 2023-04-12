
Partial Class frmrptScreeningAuditTrail
    Inherits System.Web.UI.Page

#Region "Variable Declaration "

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private Const dt_MedEx As String = "Dt_MedEx"
    Private Const VS_Controls As String = "VsControls"
    Private Const VS_Choice As String = "Choice"
    Private Const VS_MedexGroups As String = "MedexGroups"
    Private Const VS_dtMedEx_Fill As String = "dtMedEx_Fill"


    Private arrylst As New ArrayList

    Private Const GVC_MedExScreeningHdrQCNo As Integer = 0
    Private Const GVC_SubjectId As Integer = 1
    Private Const GVC_SrNo As Integer = 2
    Private Const GVC_Subject As Integer = 3
    Private Const GVC_QCComment As Integer = 4
    Private Const GVC_QCFlag As Integer = 5
    Private Const GVC_QCBy As Integer = 6
    Private Const GVC_QCDate As Integer = 7
    Private Const GVC_Response As Integer = 8
    Private Const GVC_ResponseGivenBy As Integer = 9
    Private Const GVC_ResponseGivenOn As Integer = 10
    Private Const GVC_LnkResponse As Integer = 11


#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'If Not Me.IsPostBack Then
        'BtnSave.Attributes.Add("onclick", "return closewindow();")

        fillWorkspace()
        GenCall()
        'End If


       
    End Sub

#End Region

#Region "GenCall "

    Private Function GenCall() As Boolean

        If Not IsNothing(Me.Request.QueryString("SubjectId")) AndAlso _
            Me.Request.QueryString("SubjectId").ToString.Trim() <> "" Then

            Me.HSubjectId.Value = Me.Request.QueryString("SubjectId").ToString.Trim()

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
        Dim ds_SubDetail As New DataSet
        Dim estr_retu As String = ""
        Dim strQuery As String = ""
        Dim Wstr As String = ""

        Try

            Wstr = "cActiveFlag<>'N' and vWorkSpaceId='0000000000' and vActivityId='" & GeneralModule.Act_Screening & "' and vSubjectId='" & _
                    IIf(Me.HSubjectId.Value.Trim() = "", "0000", Me.HSubjectId.Value.Trim()) & "' And vMedExType <> 'IMPORT'"
            If Not Me.objHelp.View_LastMedExScreeningHdrDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                ds_MedExSubjectHdrDtl, estr_retu) Then
                Exit Function
            End If

            If Not IsNothing(Me.Request.QueryString("SubjectId")) AndAlso _
                        (Me.Request.QueryString("SubjectId").ToString.Trim() <> "") Then

                Wstr = "vSubjectId='" & Me.HSubjectId.Value.Trim() & "'"
                If Not Me.objHelp.GetView_SubjectMaster(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                    ds_SubDetail, estr_retu) Then
                    Exit Function
                End If

                Me.txtSubject.Text = ds_SubDetail.Tables(0).Rows(0).Item("FullName").ToString.Trim() + " (" + Me.HSubjectId.Value.Trim() + ")"

            End If

            If ds_MedExSubjectHdrDtl.Tables(0).Rows.Count <= 0 AndAlso Me.HSubjectId.Value.Trim() <> "" Then

                Me.ObjCommon.ShowAlert("No Screening Has Been Done For This Subject", Me.Page())
                Exit Function

            End If

            Me.ViewState(VS_dtMedEx_Fill) = ds_MedExSubjectHdrDtl.Tables(0)

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

        Dim XRayDateAfterMonths As DateTime
        Dim Dt_XRay As New DataTable
        Dim Dv_XRay As New DataView

        Dim dsUser As New DataSet
        Dim wstr As String = ""
        Dim estr As String = ""
        Try

            'CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            Page.Title = ":: Subject Screening Audit Trail Report  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            PlaceMedEx.Controls.Clear()


            If CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).Rows.Count < 1 Then
                Exit Function
            End If

            dv_MedexGroup = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable).DefaultView
            StrGroup(0) = "vMedExGroupCode"
            StrGroup(1) = "vMedExGroupDesc"
            dt_MedexGroup = dv_MedexGroup.ToTable(True, StrGroup)
            PlaceMedEx.Controls.Add(New LiteralControl("</br>")) '</Tr>
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

#End Region

#Region "Fill Drop Dwon"

    Private Function fillWorkspace() As Boolean
        Dim ds As New DataSet
        Dim estr_retu As String = ""
        Dim Wstr As String = "vWorkSpaceId in (select Distinct vWorkspaceId from View_MedExWorkSpaceDtl where vActivityId='0003')"
        'Dim Wstr As String = "vWorkSpaceId in (select vWorkspaceId from View_MedExWorkSpaceDtl)" ' where vActivityId='0003')"

        If Not objHelp.getworkspacemst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr_retu) Then
            Exit Function
        End If

        Me.DDLWorkspace.DataSource = ds
        Me.DDLWorkspace.DataTextField = "vWorkSpaceDesc"
        Me.DDLWorkspace.DataValueField = "vWorkSpaceId"

        Me.DDLWorkspace.DataBind()
        Me.DDLWorkspace.Items.Insert(0, New ListItem("Select Project", ""))

        If Not Me.Request.QueryString("Workspace") Is Nothing Then
            Me.DDLWorkspace.SelectedValue = Me.Request.QueryString("Workspace").Trim()
        End If

        Return True
    End Function

#End Region

#Region "DDL Selected Index Change"

    Protected Sub DDLMedexGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLMedexGroup.SelectedIndexChanged
        Me.Response.Redirect("frmMedexDtls.aspx?MedexGroup=" & Me.DDLMedexGroup.SelectedValue)
    End Sub

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
                Btn.Style.Add("background-color", "rgb(174,77,2)")

                If index = 0 Then
                    Btn.Style.Add("color", "navy")
                End If

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

                txt.Enabled = False

                GetObject = txt

            Case "COMBOBOX"
                Dim Arrvalue() As String = Nothing
                Dim i As Integer
                Dim ds_ddl As New DataSet
                Dim estr As String = ""

                ddl = New DropDownList
                ddl.ID = Id
                ddl.CssClass = "dropDownList"

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

                ddl.Enabled = False

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

                End If

                RBL.RepeatDirection = RepeatDirection.Horizontal
                RBL.RepeatColumns = 3

                RBL.Enabled = False

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

                CBL.Enabled = False
                
                GetObject = CBL

            Case "FILE"
                FileBro = New FileUpload
                FileBro.ID = "FU" + Id
                FileBro.EnableViewState = True
                FileBro.CssClass = "textBox"

                FileBro.Enabled = False
                
                'FileBro. = dtValues
                'txt.MaxLength = length 

                GetObject = FileBro

            Case "TEXTAREA"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                txt.Text = dtValues
                txt.TextMode = TextBoxMode.MultiLine
                txt.Width = 262

                txt.Enabled = False

                GetObject = txt

            Case "DATETIME"
                Dim eStr As String = ""
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                txt.Text = IIf(dtValues = "", System.DateTime.Now.ToString("dd-MMM-yyyy"), dtValues)

                txt.Enabled = False
                
                GetObject = txt

            Case "TIME"
                txt = New TextBox
                txt.ID = Id
                txt.EnableViewState = True
                txt.CssClass = "textBox"
                txt.Text = IIf(dtValues = "", DateTime.Now.ToString("HH:mm:ss"), dtValues)

                txt.Enabled = False
                
                GetObject = txt

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

#Region "Button Click"

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click
        Me.Session.Remove("PlaceMedEx")

        If (Not IsNothing(Me.Request.QueryString("SubjectId")) AndAlso _
            Me.Request.QueryString("SubjectId").ToString.Trim() <> "") Then

            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "", "closewindow()", True)
            Exit Sub

        End If

        Me.Response.Redirect("frmMainpage.aspx")
    End Sub

    Protected Sub btnSubject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubject.Click
        Dim wStr As String = ""
        Dim ds_EmailAlert As New DataSet
        Dim eStr As String = ""
        Try

            GenCall()

        Catch ex As Exception
            Me.ObjCommon.ShowAlert(ex.Message, Me.Page)
        End Try

    End Sub

#End Region

#Region "fillGrid"

    Private Function fillGrid(ByRef MedexGroupCode As String) As Boolean
        Dim ds_AuditTrail As New DataSet
        Dim Wstr As String = ""
        Dim estr As String = ""

        Try

            'CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            Wstr = "vSubjectId='" & Me.HSubjectId.Value.Trim() & "' And vMedexCode='" & Me.hfMedexCode.Value.Trim() & "'"
            If Not Me.objHelp.View_MedExScreeningHdrDtlAuditTrail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                             ds_AuditTrail, estr) Then

                Exit Function
            End If

            Me.lblMedexDescription.Text = ds_AuditTrail.Tables(0).Rows(0).Item("vMedExDesc").ToString.Trim()

            Me.GVHistoryDtl.DataSource = ds_AuditTrail
            Me.GVHistoryDtl.DataBind()

            MedexGroupCode = ds_AuditTrail.Tables(0).Rows(0).Item("vMedExGroupCode").ToString.Trim()

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False

        End Try

    End Function

#End Region

#Region "Selected Index Change"

    Protected Sub DDLWorkspace_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLWorkspace.SelectedIndexChanged
        Me.Response.Redirect("frmSubjectScreening.aspx?mode=1&Workspace=" & DDLWorkspace.SelectedValue.Trim())
    End Sub

#End Region

#Region "GVHistoryDtl Grid Events"

    Protected Sub GVHistoryDtl_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

    End Sub

    Protected Sub GVHistoryDtl_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

    End Sub

#End Region

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

  


End Class
