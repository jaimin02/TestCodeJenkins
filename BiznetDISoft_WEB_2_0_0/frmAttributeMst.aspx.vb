Imports Newtonsoft.Json

Partial Class frmAttributeMst
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objcommon As New clsCommon
    Private objhelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()


    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtMedexMst As String = "DtMedExMst"
    'Private Const VS_DtBlankMedExMst As String = "DtBlankMedExMst"
    Private Const VS_MedexCode As String = "MedexCode"
    Private Const VS_DtMedExFormulaMst As String = "DtMedExFormulaMst"

    Private Const VS_DtReferenceTableDefinitions_M As String = "DtReferenceTableDefinitions_M"
    Private Const VS_DtReferenceTableDefinitions_D As String = "DtReferenceTableDefinitions_D"

    Private eStr As String = String.Empty

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

    Private Const Type_Combo As String = "ComboBox"
    Private Const Type_Redio As String = "Radio"
    Private Const Type_Check As String = "CheckBox"
    Private Const Type_ComboGlobalDictionary As String = "ComboGlobalDictionary"
    Private Const Type_Formula As String = "Formula"
    Private Const Type_TextBox As String = "TextBox"
    Private Const Type_TextArea As String = "TextArea"
    Private Const Type_File As String = "File"
    Private Const Type_Date As String = "DateTime"
    Private Const Type_Time As String = "Time"
    Private Const Type_AsyncDateTime As String = "AsyncDateTime"
    Private Const Type_AsyncTime As String = "AsyncTime"
    Private Const Type_Import As String = "Import"
    Private Const Type_Label As String = "Label"
    Private Const Type_CrfTerm As String = "CrfTerm"
    Private IsGlobalDictionary As Boolean = False

    Private Const VS_CurrentPage As String = "PageNo"
    Private Const VS_TotalRowCount As String = "TotalRowCount"
    Private Const VS_PagerStartPage As String = "PagerStartPage"
    Private Const PAGESIZE As Integer = 500

#End Region

#Region "Page Load "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.SetPaging()

        If Not IsPostBack Then
            GenCall()
            Me.txtDecimal.Text = ""
        End If
    End Sub

#End Region

#Region "GenCall "

    Private Function GenCall() As Boolean
        Dim ds As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try

            Choice = Me.Request.QueryString("Mode")
            Me.ViewState(VS_Choice) = Choice   'To use it while saving

            'If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
            '    Me.ViewState(VS_MedexCode) = Me.Request.QueryString("Value").ToString
            'End If

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtMedexMst) = ds.Tables("VIEW_MedExMst")   ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If
            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideAttributeDetails", "HideAttributeDetails(); ", True)
            End If
            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall")
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_Data "

    Private Function GenCall_Data(ByRef ds_DWR_Retu As DataSet) As Boolean

        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Val As String = String.Empty
        'Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing
        Dim ds_MedExFormulaMst As New DataSet
        Dim ds_MedExCrossChecks As New DataSet

        Try



            'Val = Me.ViewState(VS_MedexCode) 'Value of where condition
            ' Choice = Me.ViewState(VS_Choice)

            wStr = "1=2"
            'If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
            '    wStr = "vMedExCode=" + Val.ToString
            'End If

            If Not objhelp.GetMedExMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If

            If ds Is Nothing Then
                Throw New Exception(eStr_Retu)
            End If

            'If ds.Tables(0).Rows.Count <= 0 And _
            ' Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
            '    Throw New Exception("No Records Found for Selected Operation")
            'End If

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
            Me.ShowErrorMessage(ex.Message, "....GenCall_Data")
        End Try

    End Function

#End Region

#Region "GenCall_showUI "

    Private Function GenCall_ShowUI() As Boolean
        Dim dt_OpMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Attribute Master"
            Page.Title = ":: Attribute Master :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            'setTabIndex()

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

            Me.ddlOperator.Attributes.Add("onchange", "SetOperator();")
            Me.ddlNumbers.Attributes.Add("onchange", "SetNumber();")
            '***********************************

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_ShowUI")
        Finally
        End Try
    End Function

#End Region

    '#Region "Set Tab Index"

    '    Private Sub setTabIndex()

    '        Me.txtmedexdesc.TabIndex = 0
    '        Me.ddlmedexgroup.TabIndex = 1
    '        Me.ddlMedExSubGroupDesc.TabIndex = 2
    '        Me.ddlMedExType.TabIndex = 3
    '        Me.txtMedExValue.TabIndex = 4
    '        Me.ddlValidation.TabIndex = 5
    '        Me.txtDefaultValue.TabIndex = 6
    '        Me.ddlUOMDesc.TabIndex = 7
    '        Me.txtCDISCValues.TabIndex = 8
    '        Me.txtLowRange.TabIndex = 9
    '        Me.txtHighRange.TabIndex = 10
    '        Me.txtlength.TabIndex = 11
    '        Me.txtAlerton.TabIndex = 12
    '        Me.txtAlertMsg.TabIndex = 13
    '        Me.ddlRefTable.TabIndex = 14
    '        Me.ddlRefColumn.TabIndex = 15
    '        Me.chkActive.TabIndex = 16
    '        Me.chkNotNull.TabIndex = 17
    '        Me.txtOtherValues.TabIndex = 18
    '        Me.BtnSave.TabIndex = 19
    '        Me.btnupdate.TabIndex = 20
    '        Me.btncancel.TabIndex = 21
    '        Me.BtnExit.TabIndex = 22
    '        Me.txtMedex.TabIndex = 23
    '        Me.gvmedex.TabIndex = 24
    '        Me.txtmedexdesc.Focus()

    '    End Sub

    '#End Region

#Region "Fill Functions"

    Private Sub fillRefTable()
        Dim ds_Tables As New DataSet
        Dim estr As String = String.Empty
        Dim dv_Tables As New DataView
        Dim wstr As String = String.Empty

        If IsGlobalDictionary = False Then

            If Me.ViewState(VS_DtReferenceTableDefinitions_M) Is Nothing Then

                wstr = "cRefTableType ='M'"
                If Not objhelp.GetReferenceTableDefinitions(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Tables, estr) Then
                    Me.objcommon.ShowAlert(estr, Me.Page())
                    Exit Sub
                End If
                Me.ViewState(VS_DtReferenceTableDefinitions_M) = ds_Tables.Tables(0)

            End If

            ds_Tables = Nothing
            ds_Tables = New DataSet
            ds_Tables.Tables.Add(CType(Me.ViewState(VS_DtReferenceTableDefinitions_M), DataTable).Copy())
            ds_Tables.AcceptChanges()

        ElseIf IsGlobalDictionary = True Then

            If Me.ViewState(VS_DtReferenceTableDefinitions_D) Is Nothing Then

                wstr = "cRefTableType ='D'"
                If Not objhelp.GetReferenceTableDefinitions(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Tables, estr) Then
                    Me.objcommon.ShowAlert(estr, Me.Page())
                    Exit Sub
                End If
                Me.ViewState(VS_DtReferenceTableDefinitions_D) = ds_Tables.Tables(0)

            End If

            ds_Tables = Nothing
            ds_Tables = New DataSet
            ds_Tables.Tables.Add(CType(Me.ViewState(VS_DtReferenceTableDefinitions_D), DataTable).Copy())
            ds_Tables.AcceptChanges()

        End If

        ds_Tables.Tables(0).DefaultView.Sort = "vRefTableName"
        Me.ddlRefTable.DataSource = ds_Tables.Tables(0).DefaultView.ToTable()
        Me.ddlRefTable.DataTextField = "vRefTableName"
        Me.ddlRefTable.DataValueField = "nRefMasterNo"
        Me.ddlRefTable.DataBind()
        Me.ddlRefTable.Items.Insert(0, New ListItem("Select ::", ""))

    End Sub

    Private Function FillDropdown() As Boolean
        Dim eStr_Retu As String = String.Empty
        Dim Ds_FillDrop As New DataSet
        Dim Ds_FillSubGroup As New DataSet
        Dim Ds_FillUOMMst As New DataSet
        Dim dv_MedExGroup As New DataView
        Dim dv_MedExSubGroup As New DataView
        Dim dv_UOMMst As New DataView
        Dim Wstr_Scope As String = String.Empty
        Dim dv_CoreofVariable As New DataView
        Dim dv_RoleofVariable As New DataView
        Dim Ds_FillRoleofVariable As New DataSet
        Dim Ds_FillCoreofVariable As New DataSet
        Try



            'To Get Where condition of ScopeVales( Project Type )
            If Not objcommon.GetScopeValueWithCondition(Wstr_Scope) Then
                Exit Function
            End If

            'ScriptManager.RegisterClientScriptBlock(Page, Me.GetType(), "this", Wstr_Scope, True)

            Wstr_Scope += " And cActiveFlag<>'N' and cStatusIndi<>'D'"

            If objhelp.GetMedExGroupMst(Wstr_Scope, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                Ds_FillDrop, eStr_Retu) Then

                dv_MedExGroup = Ds_FillDrop.Tables(0).DefaultView
                dv_MedExGroup.Sort = "vMedExGroupDesc"
                Me.ddlmedexgroup.DataSource = dv_MedExGroup
                Me.ddlmedexgroup.DataValueField = "vMedExGroupCode"
                Me.ddlmedexgroup.DataTextField = "vMedExGroupDesc"
                Me.ddlmedexgroup.DataBind()
                Me.ddlmedexgroup.Items.Insert(0, New ListItem(" Select Attribute Group  ", ""))

            End If

            If objhelp.GetMedExSubGroupMst("cStatusIndi<>'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_FillSubGroup, eStr_Retu) Then

                dv_MedExSubGroup = Ds_FillSubGroup.Tables(0).DefaultView
                dv_MedExSubGroup.Sort = "vMedExSubGroupDesc"
                Me.ddlMedExSubGroupDesc.DataSource = dv_MedExSubGroup
                Me.ddlMedExSubGroupDesc.DataValueField = "vMedExSubGroupCode"
                Me.ddlMedExSubGroupDesc.DataTextField = "vMedExSubGroupDesc"
                Me.ddlMedExSubGroupDesc.DataBind()
                Me.ddlMedExSubGroupDesc.Items.Insert(0, New ListItem(" Select Attribute SubGroup ", ""))

            End If

            If objhelp.GetUOMMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, Ds_FillUOMMst, eStr_Retu) Then

                dv_UOMMst = Ds_FillUOMMst.Tables(0).DefaultView
                dv_UOMMst.Sort = "vUOMDesc"
                Me.ddlUOMDesc.DataSource = dv_UOMMst
                Me.ddlUOMDesc.DataValueField = "vUOMDesc"
                Me.ddlUOMDesc.DataTextField = "vUOMDesc"
                Me.ddlUOMDesc.DataBind()
                Me.ddlUOMDesc.Items.Insert(0, New ListItem(" Select UOM ", 0))


            End If

            If objhelp.GetSDTMRoleDtl("cStatusIndi<>'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_FillRoleofVariable, eStr_Retu) Then
                dv_RoleofVariable = Ds_FillRoleofVariable.Tables(0).DefaultView
                dv_RoleofVariable.Sort = "vRoleDescription"
                Me.ddlRoleofVariable.DataSource = dv_RoleofVariable
                Me.ddlRoleofVariable.DataValueField = "nRoleNo"
                Me.ddlRoleofVariable.DataTextField = "vRoleDescription"
                Me.ddlRoleofVariable.DataBind()
                Me.ddlRoleofVariable.Items.Insert(0, New ListItem(" Select Role of Variable ", 0))
            End If

            If objhelp.GetSDTMCoreDtl("cStatusIndi<>'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_FillCoreofVariable, eStr_Retu) Then
                dv_CoreofVariable = Ds_FillCoreofVariable.Tables(0).DefaultView
                dv_CoreofVariable.Sort = "vCoreDescription"
                Me.ddlCoreofVariable.DataSource = dv_CoreofVariable
                Me.ddlCoreofVariable.DataValueField = "nCoreNo"
                Me.ddlCoreofVariable.DataTextField = "vCoreDescription"
                Me.ddlCoreofVariable.DataBind()
                Me.ddlCoreofVariable.Items.Insert(0, New ListItem(" Select Core of Variable ", 0))
            End If

            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...FillDropdown")
            Return False
        End Try

    End Function

    'Added for Formula MedEx on 01-Jan-2010 by Chandresh Vanker
    Private Function FillMedExListBox() As Boolean
        Dim ds_MedEx As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try

            wStr = "cStatusIndi <> 'D' and vMedExType not in('Import','File','AsyncDateTime','AsyncTime','Time','ComboGlobalDictionary','Label','checkbox','crfterm') ORder by dModifyon Desc"
            If Not Me.objhelp.GetMedExMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_MedEx, eStr) Then
                Throw New Exception(eStr)
            End If

            ds_MedEx.Tables(0).Columns.Add("FullMedExDescription")

            For Each dr As DataRow In ds_MedEx.Tables(0).Rows
                dr("FullMedExDescription") = dr("vMedExDesc").ToString.Trim() + _
                                            " - " + dr("vMedExSubGroupDesc").ToString.Trim() + _
                                            " - " + dr("vMedExGroupDesc").ToString.Trim() + _
                                            " - " + dr("vMedexType").ToString.Trim().ToUpper() + _
                                            "  @@" + dr("vMedExCode").ToString.Trim().ToUpper()
            Next
            ds_MedEx.AcceptChanges()

            Me.lstMedEx.DataSource = ds_MedEx
            Me.lstMedEx.DataTextField = ds_MedEx.Tables(0).Columns("FullMedExDescription").ColumnName.Trim()

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
    Protected Sub ddlMedExType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMedExType.SelectedIndexChanged
        Me.ddlRefColumn.Visible = True
        Me.ddlRefTable.Enabled = False
        Me.ddlRefColumn.Enabled = False
        Me.chkActive.Enabled = False
        Me.chkNotNull.Enabled = False

        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvmedexRefresh", "UIgvmedexRefresh(); ", True)

        'Added condition for "ComboGlobalDictionary" by Chandresh Vanker on 16-Dec-2009.
        If Me.ddlMedExType.SelectedValue = Type_ComboGlobalDictionary Then

            Me.ddlRefTable.Enabled = True
            Me.ddlRefColumn.Enabled = True
            Me.txtlength.Enabled = False
            Me.txtLowRange.Enabled = False
            Me.txtHighRange.Enabled = False
            Me.ddlValidation.Enabled = False
            Me.txtMedExValue.Enabled = False
            Me.txtDefaultValue.Enabled = False
            Me.ddlUOMDesc.Enabled = False
            Me.txtAlertMsg.Enabled = False
            Me.txtAlerton.Enabled = False


            Me.pnlCheckBoxListRefColumn.Visible = False

            If Me.ddlMedExType.SelectedValue = Type_ComboGlobalDictionary Then
                Me.ddlRefColumn.Visible = False
                IsGlobalDictionary = True
                fillRefTable()
                Exit Sub
            End If

            IsGlobalDictionary = False
            fillRefTable()
        ElseIf Me.ddlMedExType.SelectedItem.Value = Type_Combo Or Me.ddlMedExType.SelectedValue = Type_Redio Or _
                Me.ddlMedExType.SelectedValue = Type_Check Then
            Me.txtlength.Enabled = False
            Me.txtLowRange.Enabled = False
            Me.txtHighRange.Enabled = False
            Me.ddlValidation.Enabled = False
            Me.ddlRefColumn.Visible = False
            Me.ddlRefTable.Enabled = False
            Me.ddlRefColumn.Enabled = False
            Me.ddlRefColumn.Visible = False
            Me.ddlRefTable.Enabled = False
            Me.ddlRefColumn.Enabled = False

        ElseIf ddlMedExType.SelectedValue = Type_Formula Then
            Me.txtlength.Enabled = True
            Me.txtLowRange.Enabled = True
            Me.txtHighRange.Enabled = True
            Me.ddlValidation.Enabled = True
            Me.ddlRefColumn.Visible = False
            Me.ddlRefTable.Enabled = False
            Me.ddlRefColumn.Enabled = False
            Me.ddlRefColumn.Visible = False
            Me.ddlRefTable.Enabled = False
            Me.ddlRefColumn.Enabled = False
            Me.txtAlertMsg.Enabled = True
            Me.txtAlerton.Enabled = True
            Me.txtCDISCValues.Enabled = True


        ElseIf Me.ddlMedExType.SelectedValue = Type_File Or Me.ddlMedExType.SelectedValue = Type_Import Then
            Me.txtlength.Enabled = False
            Me.txtLowRange.Enabled = False
            Me.txtHighRange.Enabled = False
            Me.ddlValidation.Enabled = True
            Me.txtCDISCValues.Enabled = True
            Me.ddlUOMDesc.Enabled = False
            Me.txtDefaultValue.Enabled = False
            Me.txtAlertMsg.Enabled = False
            Me.txtAlerton.Enabled = False
            Me.txtMedExValue.Enabled = False
            Me.ddlRefColumn.Visible = False
            Me.ddlRefTable.Enabled = False
            Me.ddlRefColumn.Enabled = False




        ElseIf Me.ddlMedExType.SelectedItem.Value = Type_Date Or Me.ddlMedExType.SelectedItem.Value = Type_Time Or _
         Me.ddlMedExType.SelectedItem.Value = Type_AsyncDateTime Or Me.ddlMedExType.SelectedItem.Value = Type_AsyncTime Then
            Me.txtlength.Enabled = False
            Me.txtLowRange.Enabled = False
            Me.txtHighRange.Enabled = False
            Me.ddlValidation.Enabled = False
            Me.txtCDISCValues.Enabled = True
            Me.txtAlertMsg.Enabled = False
            Me.txtAlerton.Enabled = False
            Me.txtDefaultValue.Enabled = False
            Me.txtMedExValue.Enabled = False
            Me.ddlUOMDesc.Enabled = True
            Me.ddlRefColumn.Visible = False
            Me.ddlRefTable.Enabled = False
            Me.ddlRefColumn.Enabled = False

        ElseIf Me.ddlMedExType.SelectedItem.Value = Type_Label Then
            Me.txtlength.Enabled = False
            Me.txtLowRange.Enabled = False
            Me.txtHighRange.Enabled = False
            Me.ddlValidation.Enabled = False
            Me.txtCDISCValues.Enabled = True
            Me.txtAlertMsg.Enabled = False
            Me.txtAlerton.Enabled = False
            Me.txtDefaultValue.Enabled = True
            Me.txtMedExValue.Enabled = True
            Me.ddlUOMDesc.Enabled = False
            Me.ddlRefColumn.Visible = False
            Me.ddlRefTable.Enabled = False
            Me.ddlRefColumn.Enabled = False

        Else


            Me.txtlength.Enabled = True
            Me.txtLowRange.Enabled = True
            Me.txtHighRange.Enabled = True
            Me.ddlValidation.Enabled = True
            Me.txtCDISCValues.Enabled = True
            Me.ddlUOMDesc.Enabled = True
            Me.txtDefaultValue.Enabled = True
            Me.txtAlertMsg.Enabled = True
            Me.txtAlerton.Enabled = True
            Me.ddlRefColumn.Visible = False
            Me.ddlRefTable.Enabled = False
            Me.ddlRefColumn.Enabled = False

        End If

        '***********************************

        'Added for Formula MedEx on 01-Jan-2010 by Chandresh Vanker
        Me.trFormula.Style.Add("display", "none")
        If Me.ddlMedExType.SelectedItem.Value = Type_Formula Then
            Me.trFormula.Style.Add("display", "''")
        End If
        If Me.ddlMedExType.SelectedItem.Value = Type_TextBox Or Me.ddlMedExType.SelectedItem.Value = Type_TextArea Or Me.ddlMedExType.SelectedItem.Value = Type_File Or _
                                Me.ddlMedExType.SelectedItem.Value = Type_Date Or Me.ddlMedExType.SelectedItem.Value = Type_AsyncDateTime Or Me.ddlMedExType.SelectedItem.Value = Type_AsyncTime Or _
                                 Me.ddlMedExType.SelectedItem.Value = Type_Label Or Me.ddlMedExType.SelectedItem.Value = Type_Import Or Me.ddlMedExType.SelectedItem.Value = Type_Formula Or _
                                 Me.ddlMedExType.SelectedItem.Value = Type_Combo Or Me.ddlMedExType.SelectedItem.Value = Type_Check Or Me.ddlMedExType.SelectedItem.Value = Type_Redio Or _
                                 Me.ddlMedExType.SelectedItem.Value = Type_Time Or Me.ddlMedExType.SelectedItem.Value = Type_CrfTerm Then

            Me.pnlCheckBoxListRefColumn.Visible = False
            Me.ddlRefTable.SelectedIndex = 0
            Me.ddlRefColumn.SelectedIndex = -1

        End If


    End Sub

    Protected Sub ddlRefTable_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds_Columns As New DataSet
        Dim estr As String = String.Empty

        If Not Me.objhelp.GetColumnNames(Me.ddlRefTable.SelectedItem.Text.Trim(), ds_Columns, estr) Then

            Me.objcommon.ShowAlert(estr, Me.Page())
            Exit Sub

        End If

        'ds_Columns.Tables(0).DefaultView.Sort = "ColumnName"

        Me.ddlRefColumn.Visible = True
        If Me.ddlMedExType.SelectedValue = Type_ComboGlobalDictionary Then
            Me.pnlCheckBoxListRefColumn.Visible = True
            Me.chkRefColumn.Items.Clear()
            Me.chkRefColumn.DataSource = ds_Columns.Tables(0)
            Me.chkRefColumn.DataTextField = "ColumnName"
            Me.chkRefColumn.DataValueField = "ColumnName"
            Me.chkRefColumn.DataBind()
            Me.ddlRefColumn.Visible = False

            Me.ddlRefColumnNew.DataSource = ds_Columns.Tables(0).DefaultView.ToTable()
            Me.ddlRefColumnNew.DataTextField = "ColumnName"
            Me.ddlRefColumnNew.DataValueField = "ColumnName"
            Me.ddlRefColumnNew.DataBind()


            Me.ddltemp.DataSource = ds_Columns.Tables(0).DefaultView.ToTable()
            Me.ddltemp.DataTextField = "ColumnName"
            Me.ddltemp.DataValueField = "ColumnName"
            Me.ddltemp.DataBind()


            'Me.ddlRefColumnNew.Items.Insert(0, New ListItem("Select ::", ""))
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fnApplyRefColName", "fnApplyRefColName(); ", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvmedexRefresh", "UIgvmedexRefresh(); ", True)
            Exit Sub
        End If

        Me.ddlRefColumn.DataSource = ds_Columns.Tables(0).DefaultView.ToTable()
        Me.ddlRefColumn.DataTextField = "ColumnName"
        Me.ddlRefColumn.DataValueField = "ColumnName"
        Me.ddlRefColumn.DataBind()
        Me.ddlRefColumn.Items.Insert(0, New ListItem("Select ::", ""))




        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvmedexRefresh", "UIgvmedexRefresh(); ", True)
    End Sub

#End Region

#Region "Button Events"

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds As New DataSet
        Dim ds_save As New DataSet
        Dim dt_save As New DataSet
        Dim eStr_Retu As String = String.Empty
        Dim message As String = String.Empty
        Try

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                AssignValues("Edit")

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                If Me.ddlMedExType.SelectedItem.Value.Trim = "Formula" Then
                    btnSaveFormula_Click(sender, e)
                End If
                Me.GenCall_Data(ds)
                Me.ViewState(VS_DtMedexMst) = ds.Tables("VIEW_MedExMst")   ' adding blank DataTable in viewstate
                AssignValues("Add")

            End If


            ds_save = New DataSet
            ds_save.Tables.Add(CType(Me.ViewState(VS_DtMedexMst), Data.DataTable).Copy())
            ds_save.Tables(0).TableName = "VIEW_MedExMst"

            'Added for Formula MedEx on 02-Jan-2010 by Chandresh Vanker

            'If Me.ddlMedExType.SelectedItem.Value.Trim = "Formula" Then
            If CType(Me.ViewState(VS_DtMedExFormulaMst), Data.DataTable).Rows.Count > 0 Then
                ds_save.Tables.Add(CType(Me.ViewState(VS_DtMedExFormulaMst), Data.DataTable).Copy())
                ds_save.Tables(ds_save.Tables.Count - 1).TableName = "MedExFormulaMst"
                Me.ViewState(VS_DtMedExFormulaMst) = Nothing
                'Else
                '    objcommon.ShowAlert("Please Create Formula", Me.Page)
                '    Exit Sub
                'End If
            End If
            '***************************

            If Not objLambda.Save_MedExMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_MedExMst, ds_save, Me.Session(S_UserID), eStr_Retu) Then

                objcommon.ShowAlert("Error While Saving Attribute Master", Me.Page)
                Exit Sub

            End If
            message = IIf(Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "Attribute Details Saved Successfully !", "Attribute Group Details Updated Successfully !")

            objcommon.ShowAlert(message, Me.Page)

            Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
            Me.btncancel.Visible = True
            Me.BtnExit.Visible = True
            Me.ddlMedExType.Enabled = True
            'Resetpage()
            BindGrid()

            If Not FillMedExListBox() Then
                Exit Sub
            End If

            'Me.Page_Load(sender, e)
            GenCall()
            Resetpage()
            'GenCall_Data()
            'If Not FillDropdown() Then
            '    Exit Sub
            'End If
            Me.txtDecimal.Text = ""
            Me.HFdecimalText.Value = ""
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...BtnSave_Click")
        End Try

    End Sub

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    'Protected Sub btnupdate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    'Dim Ds_Editsave As New DataSet
    'Dim eStr As String = String.Empty

    'Try

    '    If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
    '        AssignValues("Edit")
    '    End If

    '    Ds_Editsave = New DataSet
    '    Ds_Editsave.Tables.Add(CType(Me.ViewState(VS_DtMedexMst), Data.DataTable).Copy())
    '    Ds_Editsave.Tables(0).TableName = "VIEW_MedExMst"

    '    'Added for Formula MedEx on 02-Jan-2010 by Chandresh Vanker
    '    If CType(Me.ViewState(VS_DtMedExFormulaMst), Data.DataTable).Rows.Count > 0 Then
    '        Ds_Editsave.Tables.Add(CType(Me.ViewState(VS_DtMedExFormulaMst), Data.DataTable).Copy())
    '        Ds_Editsave.Tables(Ds_Editsave.Tables.Count - 1).TableName = "MedExFormulaMst"
    '    End If
    '    '***************************

    '    If Not objLambda.Save_MedExMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_MedExMst, Ds_Editsave, Me.Session(S_UserID), eStr) Then
    '        objcommon.ShowAlert("Error While Saving Attribute Master", Me.Page)
    '        Exit Sub
    '    End If

    '    Resetpage()
    '    BindGrid(Me.hMedexCode.Value.ToString())
    '    gvmedex.PageIndex = 0
    '    Me.BtnSave.Visible = True
    '    Me.BtnExit.Visible = True
    '    Me.btnupdate.Visible = False
    '    Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
    '    objcommon.ShowAlert("Attribute Updated Successfully", Me.Page)
    '    Me.ddlMedExType.Enabled = True

    'Catch ex As Exception
    '    ShowErrorMessage(ex.Message, "....btnupdate_Click")
    'End Try

    'End Sub

    Protected Sub btncancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Resetpage()
        'Me.BtnSave.Visible = True
        Me.BtnExit.Visible = True
        'Me.btnupdate.Visible = False
        Me.ddlMedExType.Enabled = True
        Me.ddlRefColumn.Visible = True
        Me.txtDecimal.Text = ""
        Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvmedexRefresh", "UIgvmedexRefresh(); ", True)

    End Sub

    Protected Sub btnSetMedex_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        BindGrid(Me.hMedexCode.Value.Trim())
    End Sub

    Protected Sub btnSaveFormula_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dt_MedExFormula As New DataTable



        Try

            'If (Me.txtDecimal.Text = "") Then
            '    objcommon.ShowAlert("Enter the Decimal no ", Me.Page)

            '    Exit Sub
            'End If




            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                If HFdecimalText.Value = "" Then
                    Me.HFdecimalText.Value = Me.txtDecimal.Text.ToString.Trim()
                End If
                AssignValuesMedExFormulaMst("Edit")

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                If HFdecimalText.Value = "" Then
                    Me.HFdecimalText.Value = Me.txtDecimal.Text.ToString.Trim()
                End If
                dt_MedExFormula = CType(Me.ViewState(VS_DtMedexMst), DataTable).Copy()
                dt_MedExFormula.Clear()
                Me.ViewState(VS_DtMedexMst) = Nothing
                Me.ViewState(VS_DtMedexMst) = dt_MedExFormula
                AssignValuesMedExFormulaMst("Add")

            End If

            objcommon.ShowAlert("Formula Saved Sucessfully !", Me.Page)

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Saving Formula. ", ex.Message)
        End Try

        'Me.txtDecimal.Text = ""
    End Sub

#End Region

#Region "Assign Values"

    Private Sub AssignValues(ByVal type As String)
        Dim dr As DataRow
        Dim eStr As String = String.Empty
        Dim dt_User As New DataTable
        Dim ds_Save As New DataSet
        Dim estr_Retu As String = String.Empty
        Try

            If type.ToUpper = "ADD" Then

                dt_User = CType(Me.ViewState(VS_DtMedexMst), DataTable)
                dt_User.Clear()
                dr = dt_User.NewRow()
                dr("vMedExCode") = "0"
                dr("vMedExDesc") = Me.txtmedexdesc.Text.Trim
                dr("vMedExGroupCode") = Me.ddlmedexgroup.SelectedItem.Value
                dr("vMedExSubGroupCode") = Me.ddlMedExSubGroupDesc.SelectedItem.Value

                If ddlMedExType.SelectedItem.Value = "n" Then
                    objcommon.ShowAlert("Please Select Attribute Type !", Me.Page)
                    Exit Sub
                End If

                If ddlAttributeCategory.SelectedItem.Value = "n" Then
                    objcommon.ShowAlert("Please Select Atleast One Attribute Category !", Me.Page)
                    Exit Sub
                End If

                dr("vMedExType") = Me.ddlMedExType.SelectedItem.Value.Trim
                dr("cAttributeCategory") = Me.ddlAttributeCategory.SelectedItem.Value.Trim

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
                dr("cAlertType") = IIf(Me.chkNotNull.Checked = True, "Y", "N")
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

                    dr("vRefColumn") = Me.ddlRefColumn.SelectedValue.Trim()

                    If Me.ddlMedExType.SelectedValue = Type_ComboGlobalDictionary Then
                        Dim strVal As String = ""
                        For index As Integer = 0 To Me.chkRefColumn.Items.Count - 1
                            If Me.chkRefColumn.Items(index).Selected = True Then
                                strVal += Me.chkRefColumn.Items(index).Text.Trim() + ","
                            End If
                        Next
                        strVal = strVal.Substring(0, strVal.LastIndexOf(","))
                        dr("vRefColumn") = strVal
                    End If

                End If
                '*************************************
                'Added New fields by Chandresh Vanker on 16-Dec-2009.
                dr("vCDISCValues") = Me.txtCDISCValues.Text.Trim()
                dr("vOtherValues") = Me.txtOtherValues.Text.Trim()
                '*************************************

                dr("nRoleNo") = Me.ddlRoleofVariable.SelectedValue.Trim()
                dr("nCoreNo") = Me.ddlCoreofVariable.SelectedValue.Trim()

                dt_User.Rows.Add(dr)

            ElseIf type.ToUpper = "EDIT" Then

                dt_User = CType(Me.ViewState(VS_DtMedexMst), DataTable)

                For Each dr In dt_User.Rows

                    dr("vMedExDesc") = Me.txtmedexdesc.Text.Trim
                    dr("vMedExGroupCode") = Me.ddlmedexgroup.SelectedItem.Value
                    dr("vMedExSubGroupCode") = Me.ddlMedExSubGroupDesc.SelectedItem.Value
                    ' dr("vMedExType") = Me.ddlMedExType.SelectedItem.Value.Trim

                    If ddlAttributeCategory.SelectedItem.Value = "n" Then
                        objcommon.ShowAlert("Please Select Atleast One Attribute Category", Me.Page)
                        Exit Sub
                    Else
                        dr("cAttributeCategory") = Me.ddlAttributeCategory.SelectedItem.Value.Trim
                    End If

                    dr("vMedExValues") = Me.txtMedExValue.Text.Trim
                    dr("vDefaultValue") = Me.txtDefaultValue.Text.Trim
                    dr("vUOM") = IIf(Me.ddlUOMDesc.SelectedIndex > 0, Me.ddlUOMDesc.SelectedItem.Value, "")
                    dr("vLowRange") = Me.txtLowRange.Text.Trim
                    dr("vHighRange") = Me.txtHighRange.Text.Trim
                    dr("vValidationType") = Me.ddlValidation.SelectedItem.Value.Trim + "," + Me.txtlength.Text.Trim
                    dr("cActiveFlag") = GeneralModule.ActiveFlag_Yes 'IIf(Me.chkactive.Checked = True, "Y", "N") 'Changed By Vishal 09-Sep-2008
                    dr("cAlertType") = IIf(Me.chkNotNull.Checked = True, "Y", "N")
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
                        dr("vRefColumn") = Me.ddlRefColumn.SelectedValue.Trim()

                        If Me.ddlMedExType.SelectedValue = Type_ComboGlobalDictionary Then
                            Dim strVal As String = ""
                            For index As Integer = 0 To Me.chkRefColumn.Items.Count - 1
                                If Me.chkRefColumn.Items(index).Selected = True Then
                                    strVal += Me.chkRefColumn.Items(index).Text.Trim() + ","
                                End If
                            Next
                            strVal = strVal.Substring(0, strVal.LastIndexOf(","))
                            dr("vRefColumn") = strVal
                        End If

                    End If
                    '*************************************
                    'Added New fields by Chandresh Vanker on 16-Dec-2009.
                    dr("vCDISCValues") = Me.txtCDISCValues.Text.Trim()
                    dr("vOtherValues") = Me.txtOtherValues.Text.Trim()
                    '*************************************

                    dr("nRoleNo") = Me.ddlRoleofVariable.SelectedValue.Trim()
                    dr("nCoreNo") = Me.ddlCoreofVariable.SelectedValue.Trim()

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

                objcommon.ShowAlert("Attribute Deleted Successfully !", Me.Page)
                BindGrid()
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add

            End If
            Me.ViewState(VS_DtMedexMst) = dt_User

            If gvmedex.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvmedexRefresh", "UIgvmedexRefresh(); ", True)
            End If

            Exit Sub
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "..AssignValues")
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
                dr("vMedExCode") = Me.hMedexCode.Value.Trim()
                dr("vMedexFormula") = Me.HFFormula.Value.Trim()
                dr("vMedexCodeForFormula") = Me.HFMedexCodeUsedForFormula.Value.Trim()
                dr("iDecimalNo") = Me.HFdecimalText.Value.ToString.Trim()
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dt_MedExFormula.Rows.Add(dr)
                dt_MedExFormula.AcceptChanges()

            ElseIf type.ToUpper = "EDIT" Then
                'Me.lblShowInEdit.Attributes.Add("Display", "")
                For Each dr In dt_MedExFormula.Rows
                    dr("vMedexFormula") = Me.HFFormula.Value.Trim()
                    dr("vMedexCodeForFormula") = Me.HFMedexCodeUsedForFormula.Value.Trim()
                    dr("iDecimalNo") = Me.HFdecimalText.Value.ToString.Trim()
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

#End Region

#Region "Reset Page"
    Private Sub Resetpage()
        Me.ddlmedexgroup.SelectedIndex = 0
        Me.txtmedexdesc.Text = ""
        Me.ddlMedExType.SelectedIndex = 0
        Me.ddlMedExSubGroupDesc.SelectedIndex = 0
        Me.txtMedExValue.Text = ""
        Me.ddlUOMDesc.SelectedIndex = 0
        Me.txtLowRange.Text = ""
        Me.txtHighRange.Text = ""
        Me.ddlValidation.SelectedIndex = 0
        Me.txtDefaultValue.Text = ""
        Me.txtlength.Text = ""
        Me.txtAlerton.Text = ""
        Me.txtAlertMsg.Text = ""
        Me.ddlRefColumn.SelectedIndex = -1
        Me.ddlRefTable.SelectedIndex = -1
        'Added new fields on 16-Dec-2009 by Chandresh Vanker
        Me.txtCDISCValues.Text = ""
        Me.txtOtherValues.Text = ""
        Me.txtFormula.Text = ""
        Me.HFFormula.Value = ""
        Me.HFMedexCodeUsedForFormula.Value = ""
        Me.pnlCheckBoxListRefColumn.Visible = False
        Me.BtnSave.Text = "Save"
        Me.BtnSave.ToolTip = "Save"
        ' added by Mani Kumar To Give specific Validation
        Me.txtLowRange.Enabled = True
        Me.txtHighRange.Enabled = True
        Me.txtlength.Enabled = True
        Me.ddlValidation.Enabled = True
        Me.trFormula.Style.Add("display", "none")
        Me.txtMedex.Text = ""
        Me.txtMedExValue.Enabled = True
        Me.txtCDISCValues.Enabled = True
        Me.ddlRefColumn.Enabled = False
        Me.ddlRefTable.Enabled = False
        Me.txtAlertMsg.Enabled = True
        Me.txtAlerton.Enabled = True
        Me.txtDefaultValue.Enabled = True
        Me.ddlUOMDesc.Enabled = True
        Me.ddlAttributeCategory.SelectedIndex = -1
        Me.ddlRoleofVariable.SelectedIndex = 0
        Me.ddlCoreofVariable.SelectedIndex = 0
    End Sub
#End Region

#Region "GridViewEvent"

    Protected Sub gvmedex_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvmedex.RowDataBound
        Me.SetPaging()

        If e.Row.RowType = DataControlRowType.DataRow Then

            CType(e.Row.FindControl("ImbEdit"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("ImbEdit"), ImageButton).CommandName = "MYEDIT"

            CType(e.Row.FindControl("ImbDelete"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("ImbDelete"), ImageButton).CommandName = "MYDELETE"

            'e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (gvmedex.PageSize * gvmedex.PageIndex) + 1

        End If

        If e.Row.RowType = DataControlRowType.DataRow Or _
            e.Row.RowType = DataControlRowType.Header Or _
            e.Row.RowType = DataControlRowType.Footer Then



            e.Row.Cells(GVC_MedexCode).Visible = False
            e.Row.Cells(GVC_MedExType).Visible = False
            e.Row.Cells(GVC_ValidationType).Visible = False
            e.Row.Cells(GVC_Active).Visible = False
            e.Row.Cells(GVC_RefColumn).Visible = False
            e.Row.Cells(GVC_RefTable).Visible = False

        End If
        'If e.Row.RowType = DataControlRowType.Pager Then
        '    e.Row.Cells(GVC_MedexCode).Visible = False
        '    e.Row.Cells(GVC_MedExType).Visible = False
        '    e.Row.Cells(GVC_ValidationType).Visible = False
        '    e.Row.Cells(GVC_Active).Visible = False
        '    e.Row.Cells(GVC_RefColumn).Visible = False
        '    e.Row.Cells(GVC_RefTable).Visible = False
        'End If


    End Sub

    Protected Sub gvmedex_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvmedex.PageIndexChanging

        gvmedex.PageIndex = e.NewPageIndex
        BindGrid()

    End Sub

    Protected Sub gvmedex_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvmedex.RowCommand
        Dim index As Integer = e.CommandArgument
        Dim ds As New DataSet
        Dim eStr As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim dt As New DataTable
        Dim Status As String = String.Empty
        Dim itm As New ListItem
        Dim strstring As String = String.Empty
        Dim strMainstring As String = String.Empty
        Dim item As New ListItem
        Dim stritemstring As String = String.Empty
        Dim subgroup As String = String.Empty
        Dim itemgroup As New ListItem
        Dim strsubstring As String = String.Empty
        Dim strUOM As String = String.Empty
        Dim StrCoreNo As String = String.Empty
        Dim StrRoleNo As String = String.Empty
        Dim StrAttributeCategory As String = String.Empty
        Dim itemUOM As New ListItem
        Dim itmRoleofVairable As New ListItem
        Dim itmCoreofVairable As New ListItem
        Dim itemAttributeCategory As New ListItem
        Dim itm1 As New ListItem
        Dim Desc As New ListItem
        Dim medExDesc As String = String.Empty
        Dim dr As DataRow
        Dim ds_MedExFormulaMst As New DataSet
        Dim dt_MedExFormulaMst As New DataTable

        Dim ds_MedExCrossChecks As New DataSet
        Dim dt_MedExCrossChecks As New DataTable

        Dim txt As String = String.Empty
        Dim txt_arr() As String
        Dim i As Integer = 0

        Try



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
                '****************************

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

                StrCoreNo = dr("nCoreNo")
                itmCoreofVairable = ddlCoreofVariable.Items.FindByValue(StrCoreNo)
                Me.ddlCoreofVariable.SelectedIndex = ddlCoreofVariable.Items.IndexOf(itmCoreofVairable)

                StrRoleNo = dr("nRoleNo")
                itmRoleofVairable = ddlRoleofVariable.Items.FindByValue(StrRoleNo)
                Me.ddlRoleofVariable.SelectedIndex = ddlRoleofVariable.Items.IndexOf(itmRoleofVairable)

                StrAttributeCategory = dr("cAttributeCategory")
                itemAttributeCategory = ddlAttributeCategory.Items.FindByValue(StrAttributeCategory)
                Me.ddlAttributeCategory.SelectedIndex = ddlAttributeCategory.Items.IndexOf(itemAttributeCategory)


                Me.txtLowRange.Text = dr("vLowRange").ToString
                Me.txtHighRange.Text = dr("vHighRange").ToString
                strstring = dr("vValidationType").ToString
                If strstring.IndexOf(",") > 0 Then
                    Dim tees As Array
                    tees = strstring.Split(",")

                    If strstring.Substring(0, 2).ToString.ToUpper = "NU" And tees.Length > 2 Then
                        strMainstring = tees(0).ToString()
                        strsubstring = tees(1).ToString() + "," + tees(2).ToString()
                        'strsubstring = tees(1).ToString()
                    Else
                        strMainstring = strstring.Substring(0, strstring.LastIndexOf(","))
                        strsubstring = strstring.Substring(strstring.LastIndexOf(",") + 1)
                    End If
                End If
                Me.txtlength.Text = strsubstring
                stritemstring = strMainstring
                item = ddlValidation.Items.FindByValue(stritemstring)
                Me.ddlValidation.SelectedIndex = Me.ddlValidation.Items.IndexOf(item)

                Me.chkActive.Checked = IIf(dr("cActiveFlag") = GeneralModule.ActiveFlag_Yes, True, False)
                Me.chkNotNull.Checked = IIf(Convert.ToString(dr("cAlertType")).Trim() = GeneralModule.ActiveFlag_Yes, True, False)
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

                    If Me.ddlMedExType.SelectedValue = Type_ComboGlobalDictionary Or Me.ddlMedExType.SelectedValue = Type_Combo Then
                        If Me.ddlMedExType.SelectedValue = Type_ComboGlobalDictionary Then
                            IsGlobalDictionary = True
                        End If
                        Me.fillRefTable()
                    End If

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
                '===================================================================
                VisibleFalseOrTrue()

                '===================================================================

                Me.ViewState(VS_DtMedexMst) = dt
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                'Me.BtnSave.Visible = False
                Me.BtnExit.Visible = False
                'Me.btnupdate.Visible = True
                Me.btncancel.Visible = True
                Me.BtnSave.Text = "Update"
                Me.BtnSave.ToolTip = "Update"
                Me.ddlMedExType.Enabled = False
                BindGrid(Me.hMedexCode.Value.ToString())

                'Added for Formula MedEx on 02-Jan-2010 by Chandresh Vanker
                Me.trFormula.Style.Add("display", "none")
                If Me.ddlMedExType.SelectedItem.Value = Type_Formula Then
                    Me.trFormula.Style.Add("display", "")
                    Me.lblShowInEdit.Style.Add("display", "")
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
            ShowErrorMessage(ex.Message, "....gvmedex_RowCommand")
        End Try
    End Sub

    Protected Sub gvmedex_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvmedex.RowDeleting
    End Sub

#End Region

#Region "BindGrid"

    Private Sub BindGrid(Optional ByVal MedexCode As String = "")
        ''Change By Vivek Patel
        Try
            Dim dsMedEx As New DataSet
            Dim eStr As String = String.Empty
            Dim Wstr_Scope As String = String.Empty
            Dim Parameters As String = String.Empty
            Dim ds_MedExMst As New DataSet

            If Not objcommon.GetScopeValueWithCondition(Wstr_Scope) Then
                Exit Sub
            End If
            Wstr_Scope = Wstr_Scope.Replace(" vProjectTypeCode in (", String.Empty)
            Wstr_Scope = Wstr_Scope.Replace("','", ",")
            Wstr_Scope = Wstr_Scope.Replace(")", String.Empty)
            Wstr_Scope = Wstr_Scope.Replace("'", String.Empty)

            'MedexCode = Wstr_Scope
            If (Wstr_Scope.Substring(Wstr_Scope.Length - 1, 1) = ",") Then
                Wstr_Scope = Wstr_Scope.Substring(0, Wstr_Scope.Length - 1)
            End If

            If Me.ViewState(VS_PagerStartPage) Is Nothing OrElse Me.ViewState(VS_CurrentPage) Is Nothing Then
                Me.ViewState(VS_PagerStartPage) = "1"
                Me.ViewState(VS_CurrentPage) = "1"
            End If

            Parameters = Wstr_Scope + "##" + "0" + "##" + "0" + "##" + MedexCode
            If objhelp.Proc_GetMedExMst(Parameters, ds_MedExMst, eStr) Then
                If Not ds_MedExMst Is Nothing Then
                    If ds_MedExMst.Tables(0).Rows.Count > 0 Then
                        Me.ViewState(VS_TotalRowCount) = ds_MedExMst.Tables(0).Rows(0)("TotalRow")
                    End If
                End If
            End If

            Parameters = Wstr_Scope + "##" + Convert.ToString(Me.ViewState(VS_CurrentPage)).Trim() + "##" + PAGESIZE.ToString() + "##" + MedexCode
            If objhelp.Proc_GetMedExMst(Parameters, dsMedEx, eStr) Then
                gvmedex.ShowFooter = False
                dsMedEx.Tables(0).DefaultView.Sort = "vMedexDesc"

                If (dsMedEx.Tables(0).Rows.Count = 0) Then
                    Me.txtDecimal.Text = ""
                Else
                    Me.txtDecimal.Text = dsMedEx.Tables(0).Rows(0)("iDecimalNo").ToString
                End If

                gvmedex.DataSource = dsMedEx
                gvmedex.DataBind()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvmedex", "UIgvmedexRefresh(); ", True)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        

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

    Protected Sub BtnViewAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnViewAll.Click
        BindGrid()
        Me.txtMedex.Text = ""
        Me.hMedexCode.Value = ""
    End Sub


#Region "WebMethod for GetScopeValue"
    <Services.WebMethod()> _
    Public Shared Function GetScopeValue() As String
        Dim Str As String = String.Empty
        Dim objcommon As New clsCommon
        Try
            If Not objcommon.GetScopeValueWithCondition(Str) Then
                Throw New Exception()
            End If

        Catch ex As Exception
        End Try
        Return Str.ToString()
    End Function



    <Services.WebMethod()> _
    Public Shared Function GetMedExMstData(ByVal vMedexCode As String) As String
        Dim objcommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()

        Dim wStr As String = "cStatusIndi<>'D'"
        Dim Wstr_Scope As String = String.Empty
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim ds_MedExMst As DataSet = New DataSet
        Dim dt_MedExMst As DataTable = New DataTable
        Dim MedexCode As String = vMedexCode
        Dim i As Integer = 1
        Try
            If MedexCode.Trim() <> "" Then
                wStr += " And vMedexCode='" + MedexCode + "'"
            End If
            If Not objcommon.GetScopeValueWithCondition(Wstr_Scope) Then
                Return "false"
                Exit Function
            End If
            wStr += " And " + Wstr_Scope
            If Not objHelp.GetMedExMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_MedExMst, estr) Then
                Return "false"
                Exit Function
            End If
            If ds_MedExMst.Tables.Count > 0 Then
                If ds_MedExMst.Tables(0).Rows.Count > 0 Then
                    strReturn = JsonConvert.SerializeObject(ds_MedExMst.Tables(0))
                    Return strReturn
                End If
            End If
            Return "false"
        Catch ex As Exception
            Throw New Exception("Error while Insert data")
            Return "false"
        End Try

    End Function

    <Services.WebMethod()> _
    Public Shared Function EditData(arr As List(Of String)) As String
        Dim objcommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()

        Dim wStr As String = "cStatusIndi<>'D'"
        Dim Wstr_Scope As String = String.Empty
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim ds_MedExMst As DataSet = New DataSet
        Dim dt_MedExMst As DataTable = New DataTable
        Dim MedexCode As String = 0
        Dim i As Integer = 1
        Try
            If MedexCode.Trim() <> "" Then
                wStr += " And vMedexCode='" + MedexCode + "'"
            End If
            If Not objcommon.GetScopeValueWithCondition(Wstr_Scope) Then
                Return "false"
                Exit Function
            End If
            wStr += " And " + Wstr_Scope
            If Not objHelp.GetMedExMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_MedExMst, estr) Then
                Return "false"
                Exit Function
            End If
            If ds_MedExMst.Tables.Count > 0 Then
                If ds_MedExMst.Tables(0).Rows.Count > 0 Then
                    strReturn = JsonConvert.SerializeObject(ds_MedExMst.Tables(0))
                    Return strReturn
                End If
            End If
            Return "false"
        Catch ex As Exception
            Throw New Exception("Error while Insert data")
            Return "false"
        End Try

    End Function

#End Region

#Region "VisibleFalseOrTrue"

    Private Function VisibleFalseOrTrue() As Boolean
        Me.chkActive.Enabled = False
        Me.chkNotNull.Enabled = False
        If Me.ddlMedExType.SelectedItem.Value = Type_Combo Or Me.ddlMedExType.SelectedValue = Type_Redio Or _
                Me.ddlMedExType.SelectedValue = Type_Check Then
            Me.txtlength.Enabled = False
            Me.txtLowRange.Enabled = False
            Me.txtHighRange.Enabled = False
            Me.ddlValidation.Enabled = False
            Me.ddlRefColumn.Visible = False
            Me.ddlRefTable.Enabled = False
            Me.ddlRefColumn.Enabled = False
            Me.ddlRefColumn.Visible = False
            Me.ddlRefTable.Enabled = False
            Me.ddlRefColumn.Enabled = False

        ElseIf ddlMedExType.SelectedValue = Type_Formula Then
            Me.txtlength.Enabled = True
            Me.txtLowRange.Enabled = True
            Me.txtHighRange.Enabled = True
            Me.ddlValidation.Enabled = True
            Me.ddlRefColumn.Visible = False
            Me.ddlRefTable.Enabled = False
            Me.ddlRefColumn.Enabled = False
            Me.ddlRefColumn.Visible = False
            Me.ddlRefTable.Enabled = False
            Me.ddlRefColumn.Enabled = False
            Me.txtAlertMsg.Enabled = True
            Me.txtAlerton.Enabled = True
            Me.txtCDISCValues.Enabled = True

        ElseIf Me.ddlMedExType.SelectedValue = Type_File Or Me.ddlMedExType.SelectedValue = Type_Import Then
            Me.txtlength.Enabled = False
            Me.txtLowRange.Enabled = False
            Me.txtHighRange.Enabled = False
            Me.ddlValidation.Enabled = False
            Me.txtCDISCValues.Enabled = True
            Me.ddlUOMDesc.Enabled = False
            Me.txtDefaultValue.Enabled = False
            Me.txtAlertMsg.Enabled = False
            Me.txtAlerton.Enabled = False
            Me.txtMedExValue.Enabled = False
            Me.ddlRefColumn.Visible = False
            Me.ddlRefTable.Enabled = False
            Me.ddlRefColumn.Enabled = False
        ElseIf Me.ddlMedExType.SelectedItem.Value = Type_Date Or Me.ddlMedExType.SelectedItem.Value = Type_Time Or _
        Me.ddlMedExType.SelectedItem.Value = Type_AsyncDateTime Or Me.ddlMedExType.SelectedItem.Value = Type_AsyncTime Then
            Me.txtlength.Enabled = False
            Me.txtLowRange.Enabled = False
            Me.txtHighRange.Enabled = False
            Me.ddlValidation.Enabled = False
            Me.txtCDISCValues.Enabled = True
            Me.txtAlertMsg.Enabled = False
            Me.txtAlerton.Enabled = False
            Me.txtDefaultValue.Enabled = False
            Me.txtMedExValue.Enabled = False
            Me.ddlUOMDesc.Enabled = True
            Me.ddlRefColumn.Visible = False
            Me.ddlRefTable.Enabled = False
            Me.ddlRefColumn.Enabled = False

        ElseIf Me.ddlMedExType.SelectedItem.Value = Type_Label Then
            Me.txtlength.Enabled = False
            Me.txtLowRange.Enabled = False
            Me.txtHighRange.Enabled = False
            Me.ddlValidation.Enabled = False
            Me.txtCDISCValues.Enabled = True
            Me.txtAlertMsg.Enabled = False
            Me.txtAlerton.Enabled = False
            Me.txtDefaultValue.Enabled = True
            Me.txtMedExValue.Enabled = True
            Me.ddlUOMDesc.Enabled = False
            Me.ddlRefColumn.Visible = False
            Me.ddlRefTable.Enabled = False
            Me.ddlRefColumn.Enabled = False

        End If
    End Function

#End Region

    Protected Sub BtnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnUpdate.Click

        'Dim index As Integer = Me.hdnStatus.Value
        Dim ds As New DataSet
        Dim eStr As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim dt As New DataTable
        Dim Status As String = String.Empty
        Dim itm As New ListItem
        Dim strstring As String = String.Empty
        Dim strMainstring As String = String.Empty
        Dim item As New ListItem
        Dim stritemstring As String = String.Empty
        Dim subgroup As String = String.Empty
        Dim itemgroup As New ListItem
        Dim strsubstring As String = String.Empty
        Dim strUOM As String = String.Empty
        Dim strnRoleNo As String = String.Empty
        Dim strNCoreNo As String = String.Empty
        Dim itemUOM As New ListItem
        Dim itemRollofVariable As New ListItem
        Dim itemCoreofVariable As New ListItem
        Dim itm1 As New ListItem
        Dim Desc As New ListItem
        Dim medExDesc As String = String.Empty
        Dim dr As DataRow
        Dim ds_MedExFormulaMst As New DataSet
        Dim dt_MedExFormulaMst As New DataTable

        Dim ds_MedExCrossChecks As New DataSet
        Dim dt_MedExCrossChecks As New DataTable

        Dim txt As String = String.Empty
        Dim txt_arr() As String
        Dim i As Integer = 0

        Dim OpeartionStatus As String = Me.hdnStatus.Value
        Try

            If OpeartionStatus = "MYEDIT" Or OpeartionStatus = "MYDELETE" Then

                Wstr = "vMedExCode='" & Me.hdnMedexCode.Value & "'"

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
                '****************************

            End If

            If OpeartionStatus = "MYEDIT" Then

                dr = dt.Rows(0)
                Me.txtmedexdesc.Text = dr("vMedExDesc").ToString
                subgroup = dr("vMedExSubGroupCode")
                itemgroup = ddlMedExSubGroupDesc.Items.FindByValue(subgroup)
                Me.ddlMedExSubGroupDesc.SelectedIndex = ddlMedExSubGroupDesc.Items.IndexOf(itemgroup)

                Me.txtMedExValue.Text = dr("vMedExValues").ToString

                Status = dr("vMedExType").ToString
                itm1 = ddlMedExType.Items.FindByText(Status)
                Me.ddlMedExType.SelectedIndex = Me.ddlMedExType.Items.IndexOf(itm1)

                Status = dr("cAttributeCategory").ToString
                itm1 = ddlAttributeCategory.Items.FindByValue(Status)
                Me.ddlAttributeCategory.SelectedIndex = Me.ddlAttributeCategory.Items.IndexOf(itm1)

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
                If strstring.IndexOf(",") > 0 Then
                    Dim tees As Array
                    tees = strstring.Split(",")

                    If strstring.Substring(0, 2).ToString.ToUpper = "NU" And tees.Length > 2 Then
                        strMainstring = tees(0).ToString()
                        strsubstring = tees(1).ToString() + "," + tees(2).ToString()
                        'strsubstring = tees(1).ToString()
                    Else
                        strMainstring = strstring.Substring(0, strstring.LastIndexOf(","))
                        strsubstring = strstring.Substring(strstring.LastIndexOf(",") + 1)
                    End If
                End If
                Me.txtlength.Text = strsubstring
                stritemstring = strMainstring
                item = ddlValidation.Items.FindByValue(stritemstring)
                Me.ddlValidation.SelectedIndex = Me.ddlValidation.Items.IndexOf(item)

                

                Me.chkActive.Checked = IIf(dr("cActiveFlag") = GeneralModule.ActiveFlag_Yes, True, False)
                Me.chkNotNull.Checked = IIf(Convert.ToString(dr("cAlertType")).Trim() = GeneralModule.ActiveFlag_Yes, True, False)
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

                    If Me.ddlMedExType.SelectedValue = Type_ComboGlobalDictionary Or Me.ddlMedExType.SelectedValue = Type_Combo Then
                        If Me.ddlMedExType.SelectedValue = Type_ComboGlobalDictionary Then
                            IsGlobalDictionary = True
                        End If
                        Me.fillRefTable()
                    End If

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

                strnRoleNo = dr("nRoleNo")
                itemRollofVariable = ddlRoleofVariable.Items.FindByValue(strnRoleNo)
                Me.ddlRoleofVariable.SelectedIndex = ddlRoleofVariable.Items.IndexOf(itemRollofVariable)

                strNCoreNo = dr("nCoreNo")
                itemCoreofVariable = ddlCoreofVariable.Items.FindByValue(strNCoreNo)
                Me.ddlCoreofVariable.SelectedIndex = ddlCoreofVariable.Items.IndexOf(itemCoreofVariable)

                '===================================================================
                VisibleFalseOrTrue()

                '===================================================================

                Me.ViewState(VS_DtMedexMst) = dt
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                'Me.BtnSave.Visible = False
                Me.BtnExit.Visible = False
                'Me.btnupdate.Visible = True
                Me.btncancel.Visible = True
                Me.BtnSave.Text = "Update"
                Me.BtnSave.ToolTip = "Update"
                Me.ddlMedExType.Enabled = False
                BindGrid(Me.hMedexCode.Value.ToString())

                'Added for Formula MedEx on 02-Jan-2010 by Chandresh Vanker
                Me.trFormula.Style.Add("display", "none")
                If Me.ddlMedExType.SelectedItem.Value = Type_Formula Then
                    Me.trFormula.Style.Add("display", "")
                    Me.lblShowInEdit.Style.Add("display", "")
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

            ElseIf OpeartionStatus = "MYDELETE" Then

                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit

                'Added for Formula MedEx on 02-Jan-2010 by Chandresh Vanker
                AssignValuesMedExFormulaMst("Delete")
                '***********************

                AssignValues("Delete")

            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....gvmedex_RowCommand")
        End Try

    End Sub

#Region "Helper Functions/Subs"
    Private Sub SetPaging()
        Dim totalPages As Integer = 0
        Dim startIndex As Integer = 1
        Dim count As Integer = 1
        Try

            Me.phTopPager.Controls.Clear()
            Me.phBottomPager.Controls.Clear()

            If Not Me.ViewState(VS_TotalRowCount) Is Nothing AndAlso Integer.Parse(Me.ViewState(VS_TotalRowCount)) > 0 Then
                totalPages = Math.Ceiling(Me.ViewState(VS_TotalRowCount) / PAGESIZE)
            End If
            startIndex = Me.ViewState(VS_PagerStartPage)
            If Me.ViewState(VS_PagerStartPage) Is Nothing Then
                startIndex = 1
                Me.ViewState(VS_PagerStartPage) = 1
            End If

            If totalPages > 1 Then

                For index As Integer = startIndex To totalPages
                    Me.phTopPager.Visible = True
                    Me.phBottomPager.Visible = True
                    Dim lnkButton As New LinkButton()
                    If startIndex > 1 And count = 1 Then
                        'This is for first Page <<
                        lnkButton = Me.AddLnkButton("BtnFirstPage", "BtnFirstPage", 1.ToString, "<<")

                        'This is for ellipse ...
                        lnkButton = Me.AddLnkButton("BtnEllipsePrev", "BtnEllipsePrev", (index - 1).ToString(), "...")
                    End If

                    'This is for Numeric Buttons
                    If index = Me.ViewState(VS_CurrentPage) Then
                        lnkButton = Me.AddLnkButton(index.ToString, "BtnPager", index, index, False)
                    Else
                        lnkButton = Me.AddLnkButton(index.ToString, "BtnPager", index, index)
                    End If

                    If count = 10 Then
                        If index < totalPages Then
                            'This is for ellipses ...
                            Me.AddLnkButton("BtnEllipseNext", "BtnEllipseNext", (index + 1).ToString, "...")

                            'This is for Last Page >>
                            Me.AddLnkButton("BtnLastPage", "BtnLastPage", totalPages.ToString, ">>")
                        End If
                        Exit For
                    End If

                    count = count + 1
                Next
            Else
                Me.phTopPager.Visible = False
                Me.phBottomPager.Visible = False
            End If
            'End If
        Catch ex As Exception

        End Try
    End Sub

    Private Function AddLnkButton(ByVal ID_1 As String, ByVal CommandName_1 As String, _
                                  ByVal CommandArg_1 As String, ByVal Text_1 As String, _
                                  Optional ByVal IsEnablePostBack As Boolean = True) As LinkButton
        Dim lnkButton As New LinkButton
        Dim lnkButtonBottom As New LinkButton
        Dim ltr As Literal
        Dim ltrBottom As Literal
        lnkButton = New LinkButton()
        ltr = New Literal()
        lnkButtonBottom = New LinkButton()
        ltrBottom = New Literal()
        lnkButton.ID = "Top" + ID_1
        lnkButton.CommandName = CommandName_1
        lnkButton.CommandArgument = CommandArg_1
        lnkButton.Text = Text_1
        lnkButton.CssClass = "PagerLinks"
        AddHandler lnkButton.Click, AddressOf PagerButton_Click
        If Not IsEnablePostBack Then
            lnkButton.OnClientClick = "return false;"
            lnkButton.Font.Underline = False
        End If

        lnkButtonBottom.ID = "Bottom" + ID_1
        lnkButtonBottom.CommandName = CommandName_1
        lnkButtonBottom.CommandArgument = CommandArg_1
        lnkButtonBottom.Text = Text_1
        lnkButtonBottom.CssClass = "PagerLinks"
        AddHandler lnkButtonBottom.Click, AddressOf PagerButton_Click
        If Not IsEnablePostBack Then
            lnkButtonBottom.OnClientClick = "return false;"
            lnkButtonBottom.Font.Underline = False
        End If

        Me.phTopPager.Controls.Add(lnkButton)
        Me.phBottomPager.Controls.Add(lnkButtonBottom)
        ltr.Text = "&nbsp;"
        ltrBottom.Text = "&nbsp;"
        Me.phTopPager.Controls.Add(ltr)
        Me.phBottomPager.Controls.Add(ltrBottom)
        Return lnkButton
    End Function

    Protected Sub PagerButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lnkButton As LinkButton
        Dim totalPages As Integer = 1
        totalPages = Me.GetTotalPages()
        lnkButton = CType(sender, LinkButton)
        Me.ViewState(VS_CurrentPage) = lnkButton.CommandArgument

        If lnkButton.CommandName.ToUpper = "BtnEllipseNext".ToUpper.ToString Then
            Me.ViewState(VS_PagerStartPage) = lnkButton.CommandArgument
            If (Integer.Parse(totalPages) - Integer.Parse(lnkButton.CommandArgument)) < 9 Then
                Me.ViewState(VS_PagerStartPage) = (Integer.Parse(totalPages) - 9)
            End If
        ElseIf lnkButton.CommandName.ToUpper = "BtnEllipsePrev".ToUpper.ToString Or _
                lnkButton.CommandName.ToUpper = "BtnLastPage".ToUpper.ToString Then
            Me.ViewState(VS_PagerStartPage) = Integer.Parse(lnkButton.CommandArgument) - 9
            If (Integer.Parse(lnkButton.CommandArgument) - 10) < 1 Then
                Me.ViewState(VS_PagerStartPage) = 1
            End If
        ElseIf lnkButton.CommandName.ToUpper = "BtnFirstPage".ToUpper.ToString Then
            Me.ViewState(VS_PagerStartPage) = 1
        End If

        BindGrid()

        'If Not Me.BindGrid() Then
        '    Exit Sub
        'End If

    End Sub

    Private Function GetTotalPages() As Integer
        GetTotalPages = 1
        If Not Me.ViewState(VS_TotalRowCount) Is Nothing Then
            GetTotalPages = Me.ViewState(VS_TotalRowCount)
        End If

        If GetTotalPages > PAGESIZE Then
            GetTotalPages = Math.Ceiling(Double.Parse(Me.ViewState(VS_TotalRowCount)) / PAGESIZE)
        End If
    End Function
#End Region

End Class
