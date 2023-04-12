Partial Class frmMedExMst
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objcommon As New clsCommon
    Private objhelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()

    Private eStr As String = ""

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtMedexMst As String = "DtMedExMst"
    Private Const VS_DtBlankMedExMst As String = "DtBlankMedExMst"
    Private Const VS_MedexCode As String = "MedexCode"
    Private Const VS_DtMedExFormulaMst As String = "DtMedExFormulaMst"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_MedexCode As Integer = 1
    Private Const GVC_MedExDesc As Integer = 2
    Private Const GVC_MedExGroupDesc As Integer = 3
    Private Const GVC_MedExSubGroupDesc As Integer = 4
    Private Const GVC_MedExType As Integer = 5
    Private Const GVC_MedExValue As Integer = 6
    Private Const GVC_Default As Integer = 7
    Private Const GVC_UOM As Integer = 8
    Private Const GVC_HighRange As Integer = 9
    Private Const GVC_LowRange As Integer = 10
    Private Const GVC_ValidationType As Integer = 11
    Private Const GVC_Active As Integer = 12
    Private Const GVC_RefTable As Integer = 13
    Private Const GVC_RefColumn As Integer = 14

    Private Const Type_Combo As Integer = 3
    Private Const Type_Redio As Integer = 4
    Private Const Type_Check As Integer = 5
    Private Const Type_ComboGlobalDictionary As Integer = 10
    Private Const Type_Formula As Integer = 11
    Private IsGlobalDictionary As Boolean = False

#End Region

#Region "Page Load "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Not IsPostBack Then
                GenCall()
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr)
        End Try
    End Sub
#End Region

#Region "GenCall "

    Private Function GenCall() As Boolean
        Dim ds As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try

            Choice = Me.Request.QueryString("Mode")
            Me.ViewState(VS_Choice) = Choice   'To use it while saving

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_MedexCode) = Me.Request.QueryString("Value").ToString
            End If

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtMedexMst) = ds.Tables("VIEW_MedExMst")   ' adding blank DataTable in viewstate

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

#Region "GenCall_Data "

    Private Function GenCall_Data(ByRef ds_DWR_Retu As DataSet) As Boolean

        Dim wStr As String = ""
        Dim eStr_Retu As String = ""
        Dim Val As String = ""
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing
        Dim ds_MedExFormulaMst As New DataSet

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Val = Me.ViewState(VS_MedexCode) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vMedExCode=" + Val.ToString
            End If


            If Not objhelp.GetMedExMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If

            If ds Is Nothing Then
                Throw New Exception(eStr_Retu)
            End If

            If ds.Tables(0).Rows.Count <= 0 And _
             Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Throw New Exception("No Records Found for Selected Operation")
            End If

            ds_DWR_Retu = ds

            'Added on 02-Jan-2010 for MedExFormulaMst by Chandresh Vanker
            If Me.ViewState(VS_DtMedExFormulaMst) Is Nothing Then

                If Not Me.objhelp.Get_MedexFormulaMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_MedExFormulaMst, eStr) Then
                    Throw New Exception(eStr)
                End If
                If ds_MedExFormulaMst Is Nothing Then
                    Throw New Exception(eStr)
                End If
                Me.ViewState(VS_DtMedExFormulaMst) = ds_MedExFormulaMst.Tables(0)

            End If

            '***************************************

            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")

        Finally

        End Try
    End Function

#End Region

#Region "GenCall_showUI "

    Private Function GenCall_ShowUI() As Boolean
        Dim dt_OpMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Try
            Page.Title = ":: Attribute Master :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Attribute Master"

            dt_OpMst = Me.ViewState(VS_DtMedexMst)

            Choice = Me.ViewState(VS_Choice)

            fillRefTable()

            If Not FillDropdown() Then
                Exit Function
            End If

            BindGrid()

            Me.ddlRefTable.Enabled = False
            Me.ddlRefColumn.Enabled = False

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

            End If

            'Added for Formula MedEx on 01-Jan-2010 by Chandresh Vanker
            If Not FillMedExListBox() Then
                Exit Function
            End If
            'Me.ddlOperator.Attributes.Add("onselectedindexchanged", "SetOperator();")
            Me.ddlOperator.Attributes.Add("onchange", "SetOperator();")
            Me.ddlNumbers.Attributes.Add("onchange", "SetNumber();")
            '***********************************

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function

#End Region

#Region "Fill Functions"

    Private Sub fillRefTable()
        Dim ds_Tables As New DataSet
        Dim estr As String = ""
        Dim dv_Tables As New DataView
        Dim wstr As String = ""

        If Me.IsGlobalDictionary = False Then
            wstr = "cRefTableType ='M'"
        ElseIf IsGlobalDictionary = True Then
            wstr = "cRefTableType ='D'"
        End If

        If Not objhelp.GetReferenceTableDefinitions(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Tables, estr) Then
            Me.objcommon.ShowAlert(estr, Me.Page())
            Exit Sub
        End If

        ds_Tables.Tables(0).DefaultView.Sort = "vRefTableName"
        Me.ddlRefTable.DataSource = ds_Tables.Tables(0).DefaultView.ToTable()
        Me.ddlRefTable.DataTextField = "vRefTableName"
        Me.ddlRefTable.DataValueField = "nRefMasterNo"
        Me.ddlRefTable.DataBind()
        Me.ddlRefTable.Items.Insert(0, New ListItem("Select ::", ""))

    End Sub

    Private Function FillDropdown() As Boolean
        Dim eStr_Retu As String = ""
        Dim Ds_FillDrop As New DataSet
        Dim Ds_FillSubGroup As New DataSet
        Dim Ds_FillUOMMst As New DataSet
        Dim dv_MedExGroup As New DataView
        Dim dv_MedExSubGroup As New DataView
        Dim dv_UOMMst As New DataView
        Dim Wstr_Scope As String = ""
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            'To Get Where condition of ScopeVales( Project Type )
            If Not objcommon.GetScopeValueWithCondition(Wstr_Scope) Then
                Exit Function
            End If

            Wstr_Scope += " And cActiveFlag<>'N' and cStatusIndi<>'D'"

            If objhelp.GetMedExGroupMst(Wstr_Scope, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                Ds_FillDrop, eStr_Retu) Then

                dv_MedExGroup = Ds_FillDrop.Tables(0).DefaultView
                dv_MedExGroup.Sort = "vMedExGroupDesc"
                Me.ddlmedexgroup.DataSource = dv_MedExGroup
                Me.ddlmedexgroup.DataValueField = "vMedExGroupCode"
                Me.ddlmedexgroup.DataTextField = "vMedExGroupDesc"
                Me.ddlmedexgroup.DataBind()

            End If

            If objhelp.GetMedExSubGroupMst("cStatusIndi<>'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_FillSubGroup, eStr_Retu) Then

                dv_MedExSubGroup = Ds_FillSubGroup.Tables(0).DefaultView
                dv_MedExSubGroup.Sort = "vMedExSubGroupDesc"
                Me.ddlMedExSubGroupDesc.DataSource = dv_MedExSubGroup
                Me.ddlMedExSubGroupDesc.DataValueField = "vMedExSubGroupCode"
                Me.ddlMedExSubGroupDesc.DataTextField = "vMedExSubGroupDesc"
                Me.ddlMedExSubGroupDesc.DataBind()

            End If

            If objhelp.GetUOMMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, Ds_FillUOMMst, eStr_Retu) Then

                dv_UOMMst = Ds_FillUOMMst.Tables(0).DefaultView
                dv_UOMMst.Sort = "vUOMDesc"
                Me.ddlUOMDesc.DataSource = dv_UOMMst
                Me.ddlUOMDesc.DataValueField = "vUOMDesc"
                Me.ddlUOMDesc.DataTextField = "vUOMDesc"
                Me.ddlUOMDesc.DataBind()
                Me.ddlUOMDesc.Items.Insert(0, New ListItem("Select UOM", 0))

            End If


            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr)
            Return False
        End Try

    End Function

    'Added for Formula MedEx on 01-Jan-2010 by Chandresh Vanker
    Private Function FillMedExListBox() As Boolean
        Dim ds_MedEx As New DataSet
        Dim wStr As String = ""
        Dim eStr As String = ""

        Try
            wStr = "cStatusIndi <> 'D'"
            If Not Me.objhelp.GetMedExMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_MedEx, eStr) Then
                Throw New Exception(eStr)
            End If

            Me.lstMedEx.DataSource = ds_MedEx
            Me.lstMedEx.DataTextField = ds_MedEx.Tables(0).Columns("vMedExDesc").ColumnName.Trim()
            Me.lstMedEx.DataValueField = ds_MedEx.Tables(0).Columns("vMedExCode").ColumnName.Trim()
            Me.lstMedEx.DataBind()

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("Error While Filling Attribute ListBox. ", ex.Message)
            Return False
        End Try

    End Function
    '*********************

#End Region

#Region "Selected Index Change"

    Protected Sub ddlMedExType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        'Added by Chandresh Vanker on 26-March-2009.......work assigned by Naimesh Bhai.....

        Me.ddlRefColumn.Visible = True

        Me.ddlRefTable.Enabled = False
        Me.ddlRefColumn.Enabled = False

        'Added condition for "ComboGlobalDictionary" by Chandresh Vanker on 16-Dec-2009 as per instruction of Nikur Sir.
        If Me.ddlMedExType.SelectedItem.Value = Type_Combo Or Me.ddlMedExType.SelectedValue = Type_Redio Or _
                Me.ddlMedExType.SelectedValue = Type_Check Or Me.ddlMedExType.SelectedValue = Type_ComboGlobalDictionary Then

            Me.ddlRefTable.Enabled = True
            Me.ddlRefColumn.Enabled = True

            Me.pnlCheckBoxListRefColumn.Visible = False
            If Me.ddlMedExType.SelectedValue = Type_ComboGlobalDictionary Then
                'Me.pnlCheckBoxListRefColumn.Visible = True
                Me.ddlRefColumn.Visible = False
                'added on 
                IsGlobalDictionary = True
                fillRefTable()
                Exit Sub
            End If

            IsGlobalDictionary = False
            fillRefTable()
        End If

        '***********************************

        'Added for Formula MedEx on 01-Jan-2010 by Chandresh Vanker
        Me.trFormula.Style.Add("display", "none")
        If Me.ddlMedExType.SelectedItem.Value = Type_Formula Then
            Me.trFormula.Style.Add("display", "block")
        End If
        '*****************************
    End Sub

    Protected Sub ddlRefTable_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds_Columns As New DataSet
        Dim estr As String = ""

        If Not Me.objhelp.GetColumnNames(Me.ddlRefTable.SelectedItem.Text.Trim(), ds_Columns, estr) Then

            Me.objcommon.ShowAlert(estr, Me.Page())
            Exit Sub

        End If

        ds_Columns.Tables(0).DefaultView.Sort = "ColumnName"

        Me.ddlRefColumn.Visible = True
        If Me.ddlMedExType.SelectedValue = Type_ComboGlobalDictionary Then
            Me.pnlCheckBoxListRefColumn.Visible = True
            Me.chkRefColumn.Items.Clear()
            Me.chkRefColumn.DataSource = ds_Columns.Tables(0).DefaultView.ToTable()
            Me.chkRefColumn.DataTextField = "ColumnName"
            Me.chkRefColumn.DataValueField = "ColumnName"
            Me.chkRefColumn.DataBind()
            Me.ddlRefColumn.Visible = False
            Exit Sub
        End If

        Me.ddlRefColumn.DataSource = ds_Columns.Tables(0).DefaultView.ToTable()
        Me.ddlRefColumn.DataTextField = "ColumnName"
        Me.ddlRefColumn.DataValueField = "ColumnName"
        Me.ddlRefColumn.DataBind()

    End Sub

#End Region

#Region "Button Events"

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds As New DataSet
        Dim ds_save As New DataSet
        Dim dt_save As New DataSet
        Dim eStr_Retu As String = ""
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                AssignValues("Edit")

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                Me.GenCall_Data(ds)
                Me.ViewState(VS_DtMedexMst) = ds.Tables("VIEW_MedExMst")   ' adding blank DataTable in viewstate
                AssignValues("Add")

            End If


            ds_save = New DataSet
            ds_save.Tables.Add(CType(Me.ViewState(VS_DtMedexMst), Data.DataTable).Copy())
            ds_save.Tables(0).TableName = "VIEW_MedExMst"

            'Added for Formula MedEx on 02-Jan-2010 by Chandresh Vanker
            ds_save.Tables.Add(CType(Me.ViewState(VS_DtMedExFormulaMst), Data.DataTable).Copy())
            ds_save.Tables(1).TableName = "MedExFormulaMst"
            '***************************

            If Not objLambda.Save_MedExMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_MedExMst, ds_save, Me.Session(S_UserID), eStr_Retu) Then

                objcommon.ShowAlert("Error While Saving Attribute Master", Me.Page)
                Exit Sub

            End If

            objcommon.ShowAlert("Attribute Saved Successfully", Me.Page)
            Resetpage()
            BindGrid()

        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr)
        End Try

    End Sub

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Resetpage()
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btnupdate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Ds_Editsave As New DataSet
        Dim eStr As String = ""

        Try

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                AssignValues("Edit")
            End If

            Ds_Editsave = New DataSet
            Ds_Editsave.Tables.Add(CType(Me.ViewState(VS_DtMedexMst), Data.DataTable).Copy())
            Ds_Editsave.Tables(0).TableName = "VIEW_MedExMst"

            'Added for Formula MedEx on 02-Jan-2010 by Chandresh Vanker
            Ds_Editsave.Tables.Add(CType(Me.ViewState(VS_DtMedExFormulaMst), Data.DataTable).Copy())
            Ds_Editsave.Tables(1).TableName = "MedExFormulaMst"
            '***************************

            If Not objLambda.Save_MedExMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_MedExMst, Ds_Editsave, Me.Session(S_UserID), eStr) Then
                objcommon.ShowAlert("Error While Saving Attribute Master", Me.Page)
                Exit Sub
            End If

            Resetpage()
            BindGrid()
            gvmedex.PageIndex = 0
            Me.BtnSave.Visible = True
            Me.BtnExit.Visible = True
            Me.btnupdate.Visible = False
            'Me.btncancel.Visible = False
            Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
            objcommon.ShowAlert("Attribute Updated Successfully", Me.Page)

        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr)

        End Try
    End Sub

    Protected Sub btncancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Resetpage()
        Me.BtnSave.Visible = True
        Me.BtnExit.Visible = True
        Me.btnupdate.Visible = False
        'Me.btncancel.Visible = False
        Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
    End Sub

    Protected Sub btnSetMedex_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        BindGrid(Me.hMedexCode.Value.Trim())
    End Sub

    'Added for Formula MedEx on 01-Jan-2010 by Chandresh Vanker

    Protected Sub btnSaveFormula_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dt_MedExFormula As New DataTable

        Try
            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                AssignValuesMedExFormulaMst("Edit")

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                dt_MedExFormula = CType(Me.ViewState(VS_DtMedexMst), DataTable).Copy()
                dt_MedExFormula.Clear()
                Me.ViewState(VS_DtMedexMst) = Nothing
                Me.ViewState(VS_DtMedexMst) = dt_MedExFormula
                AssignValuesMedExFormulaMst("Add")

            End If

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Saving Formula. ", ex.Message)
        End Try

    End Sub

    '*****************************

#End Region

#Region "Assign Values"

    Private Sub AssignValues(ByVal type As String)
        Dim dr As DataRow
        Dim eStr As String = ""
        Dim dt_User As New DataTable
        Dim ds_Save As New DataSet
        Dim estr_Retu As String = ""
        Try

            If type.ToUpper = "ADD" Then

                dt_User = CType(Me.ViewState(VS_DtMedexMst), DataTable)
                dt_User.Clear()
                dr = dt_User.NewRow()
                dr("vMedExCode") = "0"
                dr("vMedExDesc") = Me.txtmedexdesc.Text.Trim
                dr("vMedExGroupCode") = Me.ddlmedexgroup.SelectedItem.Value
                dr("vMedExSubGroupCode") = Me.ddlMedExSubGroupDesc.SelectedItem.Value

                If ddlMedExType.SelectedItem.Value = 0 Then
                    objcommon.ShowAlert("Please Select Atleast One Attribute Type", Me.Page)
                    Exit Sub
                End If
                dr("vMedExType") = Me.ddlMedExType.SelectedItem.Text.Trim

                dr("vMedExValues") = Me.txtMedExValue.Text.Trim
                dr("vDefaultValue") = Me.txtDefaultValue.Text.Trim

                dr("vUOM") = IIf(Me.ddlUOMDesc.SelectedIndex > 0, Me.ddlUOMDesc.SelectedItem.Value, "")
                dr("vLowRange") = Me.txtLowRange.Text.Trim
                dr("vHighRange") = Me.txtHighRange.Text.Trim

                If Me.txtlength.Text = "" Then
                    Me.txtlength.Text = 0
                End If

                dr("vValidationType") = Me.ddlValidation.SelectedItem.Value.Trim + "," + Me.txtlength.Text.Trim
                dr("cActiveFlag") = GeneralModule.ActiveFlag_Yes 'IIf(Me.chkactive.Checked = True, "Y", "N") ' Changed by Vishal 09-Sep-2008
                'Added By Naimesh Dave on 28-Nov-2008 as suggested by Nikur Sir
                dr("vAlertonvalue") = Me.txtAlerton.Text.Trim()
                dr("vAlertMessage") = Me.txtAlertMsg.Text.Trim()
                '********************************
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"

                'Added by Chandresh on 26-March-2009.........assigned by Naimesh Bhai
                'Added condition of "ComboGlobalDictionary" on 16-Dec-2009 as per instruction of Nikur Sir.
                If Me.ddlMedExType.SelectedItem.Value = Type_Combo Or Me.ddlMedExType.SelectedValue = Type_Redio Or _
                    Me.ddlMedExType.SelectedValue = Type_Check Or Me.ddlMedExType.SelectedValue = Type_ComboGlobalDictionary Then

                    dr("vRefTable") = Me.ddlRefTable.SelectedValue.Trim()

                    If Me.ddlMedExType.SelectedValue = Type_ComboGlobalDictionary Then
                        Dim strVal As String = ""
                        For index As Integer = 0 To Me.chkRefColumn.Items.Count - 1
                            If Me.chkRefColumn.Items(index).Selected = True Then
                                strVal += Me.chkRefColumn.Items(index).Text.Trim() + ","
                            End If
                        Next
                        strVal = strVal.Substring(0, strVal.LastIndexOf(","))
                        dr("vRefColumn") = strVal
                    Else
                        dr("vRefColumn") = Me.ddlRefColumn.SelectedValue.Trim()
                    End If

                End If
                '*************************************
                'Added New fields by Chandresh Vanker on 16-Dec-2009 as per instruction of Nikur Sir.
                dr("vCDISCValues") = Me.txtCDISCValues.Text.Trim()
                dr("vOtherValues") = Me.txtOtherValues.Text.Trim()
                '*************************************
                dt_User.Rows.Add(dr)

            ElseIf type.ToUpper = "EDIT" Then
                dt_User = CType(Me.ViewState(VS_DtMedexMst), DataTable)

                For Each dr In dt_User.Rows

                    dr("vMedExDesc") = Me.txtmedexdesc.Text.Trim
                    dr("vMedExGroupCode") = Me.ddlmedexgroup.SelectedItem.Value
                    dr("vMedExSubGroupCode") = Me.ddlMedExSubGroupDesc.SelectedItem.Value
                    dr("vMedExType") = Me.ddlMedExType.SelectedItem.Text.Trim
                    dr("vMedExValues") = Me.txtMedExValue.Text.Trim
                    dr("vDefaultValue") = Me.txtDefaultValue.Text.Trim
                    dr("vUOM") = IIf(Me.ddlUOMDesc.SelectedIndex > 0, Me.ddlUOMDesc.SelectedItem.Value, "")
                    dr("vLowRange") = Me.txtLowRange.Text.Trim
                    dr("vHighRange") = Me.txtHighRange.Text.Trim
                    dr("vValidationType") = Me.ddlValidation.SelectedItem.Value.Trim + "," + Me.txtlength.Text.Trim
                    dr("cActiveFlag") = GeneralModule.ActiveFlag_Yes 'IIf(Me.chkactive.Checked = True, "Y", "N") 'Changed By Vishal 09-Sep-2008
                    'Added By Naimesh Dave on 28-Nov-2008 as suggested by Nikur Sir
                    dr("vAlertonvalue") = Me.txtAlerton.Text.Trim()
                    dr("vAlertMessage") = Me.txtAlertMsg.Text.Trim()
                    '**************************************
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "E"

                    'Added by Chandresh on 26-March-2009.........assigned by Naimesh Bhai
                    'Added condition of "ComboGlobalDictionary" on 16-Dec-2009 as per instruction of Nikur Sir.
                    If Me.ddlMedExType.SelectedItem.Value = Type_Combo Or Me.ddlMedExType.SelectedValue = Type_Redio Or _
                         Me.ddlMedExType.SelectedValue = Type_Check Or Me.ddlMedExType.SelectedValue = Type_ComboGlobalDictionary Then

                        dr("vRefTable") = Me.ddlRefTable.SelectedValue.Trim()
                        If Me.ddlMedExType.SelectedValue = Type_ComboGlobalDictionary Then
                            Dim strVal As String = ""
                            For index As Integer = 0 To Me.chkRefColumn.Items.Count - 1
                                If Me.chkRefColumn.Items(index).Selected = True Then
                                    strVal += Me.chkRefColumn.Items(index).Text.Trim() + ","
                                End If
                            Next
                            strVal = strVal.Substring(0, strVal.LastIndexOf(","))
                            dr("vRefColumn") = strVal
                        Else
                            dr("vRefColumn") = Me.ddlRefColumn.SelectedValue.Trim()
                        End If

                    End If
                    '*************************************
                    'Added New fields by Chandresh Vanker on 16-Dec-2009 as per instruction of Nikur Sir.
                    dr("vCDISCValues") = Me.txtCDISCValues.Text.Trim()
                    dr("vOtherValues") = Me.txtOtherValues.Text.Trim()
                    '*************************************
                    dr.AcceptChanges()
                Next dr

            ElseIf type.ToUpper = "DELETE" Then

                dt_User = CType(Me.ViewState(VS_DtMedexMst), DataTable)

                For Each dr In dt_User.Rows

                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "D"
                    dr.AcceptChanges()

                Next dr

                dt_User.TableName = "VIEW_MedExMst"
                ds_Save.Tables.Add(dt_User.Copy())

                'Added for Formula MedEx on 02-Jan-2010 by Chandresh Vanker
                ds_Save.Tables.Add(CType(Me.ViewState(VS_DtMedExFormulaMst), DataTable).Copy())
                ds_Save.Tables(1).TableName = "MedExFormulaMst"
                '******************************************

                If Not objLambda.Save_MedExMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_MedExMst, ds_Save, Me.Session(S_UserID), estr_Retu) Then

                    objcommon.ShowAlert("Error While Deleteing from Attribute Master", Me.Page)
                    Exit Sub

                End If

                objcommon.ShowAlert("Attribute Deleted Successfully", Me.Page)
                BindGrid()
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add

            End If

            Me.ViewState(VS_DtMedexMst) = dt_User

        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr)
        End Try
    End Sub

    'Added for Formula MedEx on 02-Jan-2010 by Chandresh Vanker
    Private Sub AssignValuesMedExFormulaMst(ByVal type As String)
        Dim dt_MedExFormula As New DataTable
        Dim dr As DataRow

        Try
            dt_MedExFormula = CType(Me.ViewState(VS_DtMedExFormulaMst), DataTable).Copy()

            If type.ToUpper = "ADD" Then

                dt_MedExFormula.Clear()
                dr = dt_MedExFormula.NewRow()
                'vMedExCode,vMedexFormula,iModifyBy,dModifyOn,cStatusIndi
                dr("vMedExCode") = Me.hMedexCode.Value.Trim()
                dr("vMedexFormula") = Me.HFFormula.Value.Trim()
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dt_MedExFormula.Rows.Add(dr)
                dt_MedExFormula.AcceptChanges()

            ElseIf type.ToUpper = "EDIT" Then

                For Each dr In dt_MedExFormula.Rows
                    dr("vMedexFormula") = Me.HFFormula.Value.Trim()
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "E"
                Next
                dt_MedExFormula.AcceptChanges()
            
            ElseIf type.ToUpper = "DELETE" Then

                For Each dr In dt_MedExFormula.Rows
                    dr("cStatusIndi") = "D"
                Next
                dt_MedExFormula.AcceptChanges()
            
            End If
            
            Me.ViewState(VS_DtMedExFormulaMst) = Nothing
            Me.ViewState(VS_DtMedExFormulaMst) = dt_MedExFormula.Copy()

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Saving Formula. ", ex.Message)
        End Try
    End Sub
    '**************************

#End Region

#Region "Reset Page"
    Private Sub Resetpage()
        Me.ddlmedexgroup.SelectedIndex = -1
        Me.txtmedexdesc.Text = ""
        Me.ddlMedExType.SelectedIndex = -1
        Me.ddlMedExSubGroupDesc.SelectedIndex = -1
        Me.txtMedExValue.Text = ""
        Me.ddlUOMDesc.SelectedIndex = -1
        Me.txtLowRange.Text = ""
        Me.txtHighRange.Text = ""
        Me.ddlValidation.SelectedIndex = -1
        Me.txtDefaultValue.Text = ""
        Me.txtlength.Text = ""
        Me.txtAlerton.Text = ""
        Me.txtAlertMsg.Text = ""
        Me.ddlRefColumn.SelectedIndex = -1
        Me.ddlRefTable.SelectedIndex = -1
        Me.ddlRefColumn.Enabled = False
        Me.ddlRefTable.Enabled = False
        'Me.chkactive.Checked = False
        'Added new fields on 16-Dec-2009 by Chandresh Vanker
        Me.txtCDISCValues.Text = ""
        Me.txtOtherValues.Text = ""
        Me.txtFormula.Text = ""
        Me.HFFormula.Value = ""
        Me.pnlCheckBoxListRefColumn.Visible = False
    End Sub
#End Region

#Region "GridViewEvent"

    Protected Sub gvmedex_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then

            CType(e.Row.FindControl("ImbEdit"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("ImbEdit"), ImageButton).CommandName = "MYEDIT"

            CType(e.Row.FindControl("ImbDelete"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("ImbDelete"), ImageButton).CommandName = "MYDELETE"

            e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (gvmedex.PageSize * gvmedex.PageIndex) + 1

        End If

        If e.Row.RowType = DataControlRowType.DataRow Or _
            e.Row.RowType = DataControlRowType.Header Or _
            e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVC_MedexCode).Visible = False
            e.Row.Cells(GVC_MedExType).Visible = False
            e.Row.Cells(GVC_ValidationType).Visible = False
            e.Row.Cells(GVC_Active).Visible = False

        End If

    End Sub

    Protected Sub gvmedex_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)

        gvmedex.PageIndex = e.NewPageIndex
        BindGrid()

    End Sub

    Protected Sub gvmedex_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim index As Integer = e.CommandArgument
        Dim ds As New DataSet
        Dim eStr As String = ""
        Dim Wstr As String = ""
        Dim dt As New DataTable
        Dim Status As String = ""
        Dim itm As New ListItem
        Dim strstring As String = ""
        Dim strMainstring As String = ""
        Dim item As New ListItem
        Dim stritemstring As String = ""
        Dim subgroup As String = ""
        Dim itemgroup As New ListItem
        Dim strsubstring As String = ""
        Dim strUOM As String = ""
        Dim itemUOM As New ListItem
        Dim itm1 As New ListItem
        Dim Desc As New ListItem
        Dim medExDesc As String = ""
        Dim dr As DataRow
        Dim ds_MedExFormulaMst As New DataSet
        Dim dt_MedExFormulaMst As New DataTable

        Dim txt As String = ""
        Dim txt_arr() As String
        Dim i As Integer = 0

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If e.CommandName.ToUpper() = "MYEDIT" Or e.CommandName.ToUpper() = "MYDELETE" Then

                Wstr = "vMedExCode='" & Me.gvmedex.Rows(index).Cells(GVC_MedexCode).Text & "'"

                If Not objhelp.GetMedExMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr) Then
                    ShowErrorMessage(eStr, eStr)
                    Exit Sub
                End If

                Me.ViewState(VS_DtMedexMst) = ds.Tables(0)
                dt = Me.ViewState(VS_DtMedexMst)

                'Added for Formula MedEx on 02-Jan-2010 by Chandresh Vanker
                If Not objhelp.Get_MedexFormulaMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_MedExFormulaMst, eStr) Then
                    ShowErrorMessage(eStr, eStr)
                    Exit Sub
                End If

                Me.ViewState(VS_DtMedExFormulaMst) = ds_MedExFormulaMst.Tables(0)
                dt_MedExFormulaMst = Me.ViewState(VS_DtMedExFormulaMst)
                '****************************8

            End If

            If e.CommandName.ToUpper = "MYEDIT" Then

                dr = dt.Rows(0)
                Me.txtmedexdesc.Text = dr("vMedExDesc").ToString
                subgroup = dr("vMedExSubGroupCode")
                itemgroup = ddlMedExSubGroupDesc.Items.FindByValue(subgroup)
                Me.ddlMedExSubGroupDesc.SelectedIndex = ddlMedExSubGroupDesc.Items.IndexOf(itemgroup)

                Me.txtMedExValue.Text = dr("vMedExValues").ToString

                Status = dr("vMedExType").ToString
                itm1 = ddlMedExType.Items.FindByText(Status)
                Me.ddlMedExType.SelectedIndex = Me.ddlMedExType.Items.IndexOf(itm1)

                medExDesc = dr("vMedExGroupCode").ToString
                Desc = ddlmedexgroup.Items.FindByValue(medExDesc)
                Me.ddlmedexgroup.SelectedIndex = Me.ddlmedexgroup.Items.IndexOf(Desc)
                Me.txtDefaultValue.Text = dr("vDefaultValue").ToString

                strUOM = dr("vUOM")
                itemUOM = ddlUOMDesc.Items.FindByValue(strUOM)
                Me.ddlUOMDesc.SelectedIndex = ddlUOMDesc.Items.IndexOf(itemUOM)

                Me.txtLowRange.Text = dr("vLowRange").ToString
                Me.txtHighRange.Text = dr("vHighRange").ToString
                strstring = dr("vValidationType").ToString
                If strstring.IndexOf(",") > -1 Then
                    strMainstring = strstring.Substring(0, strstring.LastIndexOf(","))
                    strsubstring = strstring.Substring(strstring.LastIndexOf(",") + 1)
                End If
                Me.txtlength.Text = strsubstring
                stritemstring = strMainstring
                item = ddlValidation.Items.FindByText(stritemstring)

                Me.ddlValidation.SelectedIndex = Me.ddlValidation.Items.IndexOf(item)
                Me.chkActive.Checked = IIf(dr("cActiveFlag") = GeneralModule.ActiveFlag_Yes, True, False)
                'Added By Naimesh Dave on 28-Nov-2008 as suggested by Nikur Sir
                Me.txtAlerton.Text = dr("vAlertonvalue").ToString.Trim()
                Me.txtAlertMsg.Text = dr("vAlertMessage").ToString.Trim()
                '******************************************************

                'Added new fields on 16-Dec-2009 by Chandresh Vanker
                Me.txtCDISCValues.Text = dr("vCDISCValues").ToString.Trim()
                Me.txtOtherValues.Text = dr("vOtherValues").ToString.Trim()
                '*******************************************

                'Added condition of "ComboGlobalDictionary" by Chandresh Vanker on 16-Dec-2009
                If Me.ddlMedExType.SelectedItem.Value = Type_Combo Or Me.ddlMedExType.SelectedValue = Type_Redio Or _
                       Me.ddlMedExType.SelectedValue = Type_Check Or Me.ddlMedExType.SelectedValue = Type_ComboGlobalDictionary Then

                    Me.ddlRefColumn.Visible = True
                    Me.ddlRefTable.Enabled = True
                    Me.ddlRefColumn.Enabled = True
                    Me.pnlCheckBoxListRefColumn.Visible = False

                    If Not dr("vRefTable") Is System.DBNull.Value AndAlso dr("vRefColumn").ToString.Trim() <> "" Then
                        Me.ddlRefTable.SelectedValue = dr("vRefTable").ToString.Trim()
                        Me.ddlRefTable_SelectedIndexChanged(sender, e)
                    End If

                    If Not dr("vRefColumn") Is System.DBNull.Value AndAlso dr("vRefColumn").ToString.Trim() <> "" Then

                        If Me.ddlMedExType.SelectedValue = Type_ComboGlobalDictionary Then
                            Me.ddlRefColumn.Visible = False
                            Me.pnlCheckBoxListRefColumn.Visible = True

                            Dim strVal As String = dr("vRefColumn").ToString.Trim()
                            Dim str() As String = strVal.Split(",")
                            For counter As Integer = 0 To str.Length - 1
                                Me.chkRefColumn.Items.FindByText(str(counter).Trim).Selected = True
                            Next

                        Else
                            Me.ddlRefColumn.Visible = True
                            Me.ddlRefColumn.SelectedValue = dr("vRefColumn").ToString.Trim()
                        End If

                    End If

                End If

                Me.ViewState(VS_DtMedexMst) = dt
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                Me.BtnSave.Visible = False
                Me.BtnExit.Visible = False
                Me.btnupdate.Visible = True
                Me.btncancel.Visible = True
                BindGrid()
                'gvmedex.PageIndex = 0

                'Added for Formula MedEx on 02-Jan-2010 by Chandresh Vanker
                Me.trFormula.Style.Add("display", "none")
                If Me.ddlMedExType.SelectedItem.Value = Type_Formula Then
                    Me.trFormula.Style.Add("display", "block")
                    Me.HFFormula.Value = dt_MedExFormulaMst.Rows(0)("vMedExFormula").ToString()

                    txt = dt_MedExFormulaMst.Rows(0)("vMedExFormula").ToString()
                    txt_arr = txt.Split("?")

                    For i = 0 To txt_arr.Length - 1
                        If txt_arr(i).Length = 5 Then
                            txt = txt.Replace(txt_arr(i).Trim(), Me.lstMedEx.Items.FindByValue(txt_arr(i).Trim()).Text.Trim())
                        End If
                    Next i

                    txt = txt.Replace("?", "")
                    Me.txtFormula.Text = txt

                End If
                '**************************

            ElseIf e.CommandName.ToUpper = "MYDELETE" Then

                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit

                'Added for Formula MedEx on 02-Jan-2010 by Chandresh Vanker
                AssignValuesMedExFormulaMst("Delete")
                '***********************

                AssignValues("Delete")

            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr)
        End Try
    End Sub

    Protected Sub gvmedex_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs)
    End Sub

#End Region

#Region "BindGrid"

    Private Sub BindGrid(Optional ByVal MedexCode As String = "")
        Dim dsMedEx As New DataSet
        Dim eStr As String = ""
        Dim whereCondition As String = "cStatusIndi<>'D'"
        Dim Wstr_Scope As String = ""

        If MedexCode.Trim() <> "" Then
            whereCondition += " And vMedexCode='" + MedexCode + "'"
        End If

        'To Get Where condition of ScopeVales( Project Type )
        If Not objcommon.GetScopeValueWithCondition(Wstr_Scope) Then
            Exit Sub
        End If

        whereCondition += " And " + Wstr_Scope

        If objhelp.GetMedExMst(whereCondition, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsMedEx, eStr) Then

            gvmedex.ShowFooter = False
            dsMedEx.Tables(0).DefaultView.Sort = "vMedexDesc"
            gvmedex.DataSource = dsMedEx.Tables(0).DefaultView.ToTable()
            gvmedex.DataBind()
            gvmedex.Dispose()

        End If

    End Sub

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
    
End Class
