Public Enum ValidationType1
    Validation_NotNullInteger = 1
    Validation_NotNullNumeric = 2
    Validation_Integer = 3
    Validation_Numeric = 4
    Validation_Alphabet = 5
    Validation_Alphanumeric = 6
End Enum

Partial Class frmMedExDtls
    Inherits System.Web.UI.Page

    Dim objCommon As New clsCommon
    Dim objHelpDBTable As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()
    Private Const dt_MedEx As String = "Dt_MedEx"
    Private Const VSDtSubMedExMst As String = "Dt_SubMedExMst"
    Private Const VS_Controls As String = "VsControls"
    Private Const VS_dsSubMedex As String = "ds_SubMedex"

    Private Const Val_AN As String = "AN" '"Alphanumeric"
    Private Const Val_AL As String = "AL" '"Alphabate"
    Private Const Val_NU As String = "NU" '"Numeric"
    Private Const Val_IN As String = "IN" '"Integer"
    Private Const Val_NNI As String = "NNI" '"NotNull Integer"
    Private Const Val_NNU As String = "NNU" '"NotNull Numeric"

    Dim arrylst As New ArrayList
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            BtnSave.Attributes.Add("onclick", "return closewindow();")
            'fillMedExGroup()
            fillSubject()
            GenCall()
        End If
    End Sub

#Region "Fill Drop Dwon"
    'Private Function fillWorkspace() As Boolean
    '    Dim ds As New DataSet
    '    Dim estr_retu As String = ""
    '    Dim Wstr As String = ""

    '    If Not objHelpDBTable.getworkspacemst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds, estr_retu) Then
    '        Exit Function
    '    End If

    '    Me.DDLWorkspace.DataSource = ds
    '    Me.DDLWorkspace.DataTextField = "vWorkSpaceDesc"
    '    Me.DDLWorkspace.DataValueField = "vWorkSpaceId"

    '    Me.DDLWorkspace.DataBind()


    '    If Not Me.Request.QueryString("Workspace") Is Nothing Then
    '        Me.DDLWorkspace.SelectedValue = Me.Request.QueryString("Workspace").Trim()
    '    End If

    '    Return True
    'End Function

    Private Function fillTabControl() As Boolean

    End Function

    Private Function fillMedExGroup() As Boolean
        Dim ds As New DataSet
        Dim estr_retu As String = ""
        Dim Wstr As String = "vWorkSpaceId='0000000000'"
        Dim DistParm(1) As String
        'If Not objHelpDBTable.GetMedExGroupMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds, estr_retu) Then
        'Exit Function
        'End If
        If Not Me.objHelpDBTable.GetViewMedExWorkSpaceDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr_retu) Then
            Exit Function
        End If

        DistParm = "vMedExGroupDesc,vMedExGroupCode".Split(",")

        Me.DDLMedexGroup.DataSource = ds.Tables(0).DefaultView.ToTable(True, DistParm)
        Me.DDLMedexGroup.DataTextField = "vMedExGroupDesc"
        Me.DDLMedexGroup.DataValueField = "vMedExGroupCode"

        Me.DDLMedexGroup.DataBind()


        If Not Me.Request.QueryString("MedexGroup") Is Nothing Then
            Me.DDLMedexGroup.SelectedValue = Me.Request.QueryString("MedexGroup").Trim()
        End If

        Return True

    End Function

    Private Function fillSubject() As Boolean
        Dim ds As New DataSet
        Dim estr_retu As String = ""
        Dim Wstr As String = ""

        Wstr = "cRejectionFlag <> 'Y'"

        If Not objHelpDBTable.GetView_SubjectMaster(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds, estr_retu) Then
            Exit Function
        End If

        Me.ddlSubject.DataSource = ds
        Me.ddlSubject.DataTextField = "FullName"
        Me.ddlSubject.DataValueField = "vSubjectID"
        Me.ddlSubject.DataBind()

        Return True

    End Function


#End Region

#Region "DDL Selected Index Change"

    Protected Sub DDLMedexGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLMedexGroup.SelectedIndexChanged
        Me.Response.Redirect("frmMedexDtls.aspx?MedexGroup=" & Me.DDLMedexGroup.SelectedValue)
    End Sub

#End Region

#Region "GenCall"
    Private Function GenCall() As Boolean

        If Not GenCall_Data() Then
            Exit Function
        End If

        If Not GenCall_ShowUI() Then
            Exit Function
        End If

        Return True

    End Function
#End Region

#Region "GenCall_Data()"
    Private Function GenCall_Data() As Boolean
        Dim ds As New DataSet
        Dim ds_SubMedExMst As New DataSet
        Dim ds_SubMedex As New DataSet
        Dim estr_retu As String = ""
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Dim strQuery As String = ""
        Dim Wstr As String = ""

        Try
            'strQuery = "Select * from View_MedExMst"
            Wstr = "cActiveFlag<>'N'" ' and vMedExGroupCode='" & Me.DDLMedexGroup.SelectedValue.Trim() & "'"
            If Not objHelpDBTable.GetMedExMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr_retu) Then
                Exit Function
            End If

            If Not objHelpDBTable.GetMedExScreeningDtl("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubMedex, estr_retu) Then
                Exit Function
            End If

            If Not objHelpDBTable.GetMedExScreeningHdr("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubMedExMst, estr_retu) Then
                Exit Function
            End If

            If ds_SubMedex Is Nothing Then
                Throw New Exception(estr_retu)
            End If

            Me.ViewState(VS_dsSubMedex) = ds_SubMedex 'Blank Data Structer for Saveing
            Me.ViewState(VSDtSubMedExMst) = ds_SubMedExMst

            If ds Is Nothing Then
                Throw New Exception(estr_retu)
            End If

            If ds.Tables(0).Rows.Count <= 0 And _
                Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Throw New Exception("No Records Found")
            End If

            Me.ViewState(Dt_MedEx) = ds.Tables(0)

            Return True
        Catch ex As Exception
            'ShowErrorMessage(ex.Message.ToString, estr_retu)
        End Try
    End Function
#End Region

#Region "GenCall_ShowUI()"
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
        Try
            Page.Title = ":: Medical Examination for WorkSpace :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            dv_MedexGroup = CType(Me.ViewState(dt_MedEx), DataTable).DefaultView
            StrGroup(0) = "vMedExGroupCode"
            StrGroup(1) = "vMedExGroupDesc"
            dt_MedexGroup = dv_MedexGroup.ToTable(True, StrGroup)
            PlaceMedEx.Controls.Add(New LiteralControl("</br>")) '</Tr>
            PlaceMedEx.Controls.Add(New LiteralControl("<Table  border=""1"" style=""background-color:#e9f2fa"">"))

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

            PlaceMedEx.Controls.Add(New LiteralControl("<Tr ALIGN=LEFT>"))
            PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""white-space: nowrap; vertical-align:middle"">"))

            Me.GetButtons(StrGroupDesc, StrGroupCodes)

            PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
            PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
            '**********************************

            PlaceMedEx.Controls.Add(New LiteralControl("<Tr ALIGN=LEFT >"))
            PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""white-space: nowrap; vertical-align:middle"">"))
            'Me.GetButtons(drGroup("vMedExGroupDesc").ToString.Trim(), drGroup("vMedExGroupCode").ToString.Trim())
            For Each drGroup In dt_MedexGroup.Rows

                dt = New DataTable
                dt_MedExMst = New DataTable
                dt = Me.ViewState(dt_MedEx)
                dv = New DataView
                dv = dt.DefaultView
                dv.RowFilter = "vMedExGroupCode='" & drGroup("vMedExGroupCode").ToString.Trim() & "'"
                dt_MedExMst = dv.ToTable()
                'PlaceMedEx.Controls.Add(New LiteralControl("<b>" & drGroup("vMedExGroupDesc").ToString.Trim() & ":</b>"))
                If i = 0 Then
                    PlaceMedEx.Controls.Add(New LiteralControl("<Div id='Div" & drGroup("vMedExGroupCode").ToString.Trim() & "' style=""display:block"">"))
                Else
                    PlaceMedEx.Controls.Add(New LiteralControl("<Div id='Div" & drGroup("vMedExGroupCode").ToString.Trim() & "' style=""display:none"">"))
                End If
                PlaceMedEx.Controls.Add(New LiteralControl("<Table>")) ' border=""1""
                For Each dr In dt_MedExMst.Rows
                    PlaceMedEx.Controls.Add(New LiteralControl("<Tr ALIGN=LEFT>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""white-space: nowrap; vertical-align:middle"">"))
                    PlaceMedEx.Controls.Add(Getlable(dr("vMedExDesc") & ": ", dr("vMedExCode")))
                    PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""white-space: nowrap; vertical-align:middle"">"))

                    If Not (dr("vValidationType1") Is System.DBNull.Value) Then
                        If dr("vValidationType1") <> "" And dr("vValidationType1") <> "NA" Then
                            StrValidation = dr("vValidationType1").ToString.Trim().Split(",")
                            objelement = GetObject(dr("vMedExType"), dr("vMedExCode"), dr("vMedExValues"), "", StrValidation(0).ToString.Trim(), StrValidation(1).ToString.Trim())
                        Else
                            objelement = GetObject(dr("vMedExType"), dr("vMedExCode"), dr("vMedExValues"), "", "")
                        End If
                    Else
                        objelement = GetObject(dr("vMedExType"), dr("vMedExCode"), dr("vMedExValues"), "", "")
                    End If

                    PlaceMedEx.Controls.Add(objelement)
                    PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
                Next
                i += 1
                PlaceMedEx.Controls.Add(New LiteralControl("</Table>")) '</Tr>
                PlaceMedEx.Controls.Add(New LiteralControl("</Div>")) '</Tr>
            Next
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
#End Region

#Region "GetButtons"
    Private Function GetButtons(ByVal BtnName As String, ByVal Id As String) As Boolean
        Dim Btn As Button
        Dim i As Integer
        Dim StrGroupCode_arry() As String
        Dim StrGroupDesc_arry() As String
        Try
            StrGroupCode_arry = Id.Split(",")
            StrGroupDesc_arry = BtnName.Split(",")
            For i = 0 To StrGroupCode_arry.Length - 1
                Btn = New Button
                Btn.ID = "Btn" & StrGroupCode_arry(i)
                Btn.Text = StrGroupDesc_arry(i).Trim()
                Btn.CssClass = "TABButton"
                Btn.OnClientClick = "return DisplayDiv('" & StrGroupCode_arry(i) & "','" & Id & "');"
                'GetButtons = Btn
                PlaceMedEx.Controls.Add(Btn)
            Next
        Catch ex As Exception

        End Try
    End Function
#End Region

#Region "GetObject"
    Private Function GetObject(ByVal vFieldType As String, ByVal Id As String, ByVal vMedExValues As String, ByVal dtValues As String, Optional ByVal Validation As String = "", Optional ByVal length As String = "") As Object
        Dim txt As TextBox
        Dim ddl As DropDownList
        Dim FileBro As FileUpload

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

                If Validation <> "" And Validation <> "NA" Then
                    If Validation = Val_AN Then
                        txt.Attributes.Add("onblur", "ValidateTextbox(" & ValidationType1.Validation_Alphanumeric & ",this,'Please Enter AlphaNumeric Value');")
                        'txt.Attributes.Add("onblur", "return checkVal(this.value," & txt.ClientID & ",'" & _
                        '                            ValidationType1.Validation_Alphanumeric & "');")
                    ElseIf Validation = Val_NU Then
                        txt.Attributes.Add("onblur", "ValidateTextbox(" & ValidationType1.Validation_Numeric & ",this,'Please Enter Numeric or Blank Value');")

                    ElseIf Validation = Val_IN Then
                        txt.Attributes.Add("onblur", "ValidateTextbox(" & ValidationType1.Validation_Integer & ",this,'Please Enter Integer or Blank Value');")

                        'txt.Attributes.Add("onblur", "return checkVal(this.value," & txt.ClientID & ",'" & _
                        '                            ValidationType1.Validation_Integer & "');")
                    ElseIf Validation = Val_AL Then
                        txt.Attributes.Add("onblur", "ValidateTextbox('" & ValidationType1.Validation_Alphabet & "',this,'Please Enter Alphabets only');")

                        'txt.Attributes.Add("onblur", "return checkVal(this.value," & txt.ClientID & ",'" & _
                        '                            ValidationType1.Validation_Alphabate & "');")
                    ElseIf Validation = Val_NNI Then
                        txt.Attributes.Add("onblur", "ValidateTextbox('" & ValidationType1.Validation_NotNullInteger & "',this,'Please Enter Integer Value');")
                        'txt.Attributes.Add("onblur", "return checkVal(this.value," & txt.ClientID & ",'" & _
                        '                            ValidationType1.Validation_NotNullInteger & "');")
                    ElseIf Validation = Val_NNU Then
                        txt.Attributes.Add("onblur", "ValidateTextbox('" & ValidationType1.Validation_NotNullNumeric & "',this,'Please Enter Numeric Value');")
                        'txt.Attributes.Add("onblur", "return checkVal(this.value," & txt.ClientID & ",'" & _
                        '                            ValidationType1.Validation_NotNullNumeric & "');")
                    End If

                End If
                '************************************
                GetObject = txt
            Case "COMBOBOX"
                ddl = New DropDownList
                ddl.ID = Id
                ddl.CssClass = "dropDownList"
                Dim Arrvalue() As String = Nothing
                Dim i As Integer
                Arrvalue = Split(vMedExValues, ",")
                For i = 0 To Arrvalue.Length - 1
                    ddl.Items.Add(New ListItem(Arrvalue(i), Arrvalue(i)))
                Next
                If Not dtValues = "" Then
                    ddl.SelectedItem.Text = dtValues
                End If
                GetObject = ddl
            Case "FILE"
                FileBro = New FileUpload
                FileBro.ID = Id
                FileBro.EnableViewState = True
                FileBro.CssClass = "textBox"
                'FileBro. = dtValues
                'txt.MaxLength = length

                GetObject = FileBro

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

    Private Function GetREV(ByVal vFieldType As String, ByVal Id As String, ByVal vMedExValues As String, ByVal ValidationType1 As String) As Object
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
        Dim TranNo As Integer
        Dim Is_Transaction As Boolean = False
        Try

            If Not AssignValues() Then
                Me.objCommon.ShowAlert("Error While Assigning Data.", Me.Page)
                Exit Sub
            End If

            ds = Me.ViewState(VS_dsSubMedex)
            'dt_top = ds.Tables("MedExScreeningDtl").Rows(0).Table
            'ds.Tables("MedExScreeningDtl").Rows(0).Delete()
            'ds.Tables("MedExScreeningDtl").AcceptChanges()
            'dt_top.TableName = "Top_MedExScreeningDtl"
            'ds.Tables.Add(dt_top.Copy)

            If Not Me.objLambda.Save_MedExScreeningDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds, Me.Session(S_UserID), TranNo, Is_Transaction, estr) Then
                Me.objCommon.ShowAlert(estr, Me.Page)
                Exit Sub
            Else
                Me.objCommon.ShowAlert("Records Save Successfully", Me.Page)
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
        End Try
    End Sub

    Private Function AssignValues() As Boolean
        Dim index As Integer = 0
        Dim StrValue As String = ""
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim str_retn As String = ""
        Dim objCollection As ControlCollection
        Dim objControl As Control
        'Dim Dt_WSSub As New DataTable
        Dim dr As DataRow
        Dim dt_Save As New DataTable
        Dim dt_SubMedExMst As New DataTable
        Dim TranNo As Integer = 0

        Try

            dt_Save = CType(Me.ViewState(VS_dsSubMedex), DataSet).Tables(0)
            dt_SubMedExMst = CType(Me.ViewState(VSDtSubMedExMst), DataSet).Tables(0)

            objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection) 'Me.PlaceMedEx.Controls 

            'For Master table
            dr = dt_SubMedExMst.NewRow()
            dr("vSubjectId") = Me.ddlSubject.SelectedValue.Trim()
            dr("iMedExScreenId") = -999
            dt_SubMedExMst.Rows.Add(dr)
            dt_SubMedExMst.AcceptChanges()
            '****************************************

            'For Detail Table
            For Each objControl In objCollection
                If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.TextBox" Then
                    TranNo -= 1
                    dr = dt_Save.NewRow()
                    dr("iMedExScreenId") = -999
                    dr("vMedExCode") = objControl.ID 'CType(objControl, TextBox).ID
                    dr("vMedExResult") = Request.Form(objControl.ID) 'CType(objControl, TextBox).Text 'CType(Me.FindControl(obj.GetId), TextBox).Text
                    dr("iTranno") = TranNo
                    dt_Save.Rows.Add(dr)
                    dt_Save.AcceptChanges()
                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.DropDownList" Then
                    TranNo -= 1
                    dr = dt_Save.NewRow()
                    dr("iMedExScreenId") = -999
                    dr("vMedExCode") = objControl.ID 'CType(objControl, DropDownList).ID
                    dr("vMedExResult") = Request.Form(objControl.ID) 'CType(objControl, DropDownList).SelectedValue.Trim()
                    dr("iTranno") = TranNo
                    dt_Save.Rows.Add(dr)
                    dt_Save.AcceptChanges()
                ElseIf objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.FileUpload" Then
                    TranNo -= 1
                    dr = dt_Save.NewRow()
                    dr("iMedExScreenId") = -999
                    dr("vMedExCode") = objControl.ID 'CType(objControl, DropDownList).ID
                    dr("vMedExResult") = Request.Form(objControl.ID) 'CType(objControl, DropDownList).SelectedValue.Trim()
                    dr("iTranno") = TranNo
                    dt_Save.Rows.Add(dr)
                    dt_Save.AcceptChanges()
                End If

            Next
            '****************************************
            dt_SubMedExMst.TableName = "MedExScreeningHdr"
            dt_Save.TableName = "MedExScreeningDtl"
            ds.Tables.Add(dt_SubMedExMst.Copy)
            ds.Tables.Add(dt_Save.Copy)

            Me.ViewState(VS_dsSubMedex) = ds
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
            Return False
        End Try

    End Function

    Private Function CreateTable(ByRef Dt_WSSub As DataTable) As Boolean
        'Dim Dt_WSSub As New DataTable
        Dim dc As DataColumn
        Try

            dc = New DataColumn
            dc.ColumnName = "vMedexCode"
            dc.DataType = GetType(String)
            Dt_WSSub.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "vMedexCode"
            dc.DataType = GetType(String)
            Dt_WSSub.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "vMedexCode"
            dc.DataType = GetType(String)
            Dt_WSSub.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "vMedExValue"
            dc.DataType = GetType(String)
            Dt_WSSub.Columns.Add(dc)

            Dt_WSSub.AcceptChanges()
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
            Return False
        End Try
    End Function
#End Region

#Region "Button Click"
    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click
        Me.Response.Redirect("frmMainpage.aspx")
    End Sub
#End Region

    'Protected Sub DDLWorkspace_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDLWorkspace.SelectedIndexChanged
    '    fillMedExGroup()
    'End Sub

    'Private Function GetValues(ByVal obj As ClsMedExDtls) As String
    '    Dim str_retn As String = ""
    '    Dim objCollection As ControlCollection
    '    Dim objControl As Control

    '    objCollection = CType(Me.Session("PlaceMedEx"), ControlCollection)

    '    For Each objControl In objCollection
    '        If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.TextBox" Then
    '            str_retn = CType(objControl, TextBox).Text 'CType(Me.FindControl(obj.GetId), TextBox).Text
    '        End If
    '    Next

    '    Return str_retn
    'End Function

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click
        Dim ds As DataSet
        If Not AssignValues() Then
            Me.objCommon.ShowAlert("Error While Assigning Data.", Me.Page)
            Exit Sub
        Else
            ds = Me.ViewState(VS_dsSubMedex)
            Me.GridView1.DataSource = ds
            Me.GridView1.DataBind()
        End If
    End Sub

End Class
