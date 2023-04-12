
Partial Class frmCTMMeddraBrowse
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objCommon As New clsCommon
    Private objHelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()

    Private Const Vs_TableName As String = "TableName"
    Private Const Vs_DisplayFields As String = "DisplayFields"
    Private Vs_Meddra As String = "MedDra"

    Private DtSearch As New DataTable
    Private Const Vs_DtSearch As String = "DtSearch"
    'Private Const GVC_expandable As Integer = 0
    Private Const GVC_CheckBoxConditionField As Integer = 0
    Private Const GVC_CheckBoxDisplayField As Integer = 1
    Private Const GVC_MeddraTerm As Integer = 2
    Private Const GVC_Operator As Integer = 3
    Private Const GVC_SearchText As Integer = 4
    Dim DisplayFields As String = String.Empty
    Dim columnstring As String = "llt_name,pt_name"
#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack() Then



            If Not Me.Gencall_ShowUI() Then
                Exit Sub
            End If

        End If
    End Sub

#End Region

#Region "HideMenu"

    Private Sub HideMenu()
        Dim tmpMenu As Menu
        tmpMenu = CType(Me.Master.FindControl("menu1"), Menu)

        If Not tmpMenu Is Nothing Then
            tmpMenu.Visible = False
        End If
    End Sub

#End Region

#Region "Gencall_ShowUI()"

    Private Function Gencall_ShowUI() As Boolean
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim query As String = String.Empty
        Dim dsMedExMst As New DataSet
        Try
            Page.Title = " ::  Global Dictionary  ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            If Not Me.Request.QueryString("MedExCode") Is Nothing AndAlso _
                        Convert.ToString(Me.Request.QueryString("MedExCode")).Trim() <> "" Then


                query = "select * from MedExWorkSpaceDtl inner join REFERENCETABLEDEFINITIONS" + _
                        " On (MedExWorkSpaceDtl.vRefTable = ReferenceTableDefinitions.nRefMasterNo and ReferenceTableDefinitions .cStatusIndi <>'D')" + _
                        "where vMedExCode = '" + CType(Me.Request.QueryString("MedExCode"), String).Trim() + "'" + _
                        " And nMedexWorkspaceDtlno=" + Me.Request.QueryString("MedExWorkSpaceDtlNo").ToString.Trim()


                dsMedExMst = objHelpDb.GetResultSet(query, "ComboGlobal")



                If dsMedExMst.Tables(0) Is Nothing OrElse dsMedExMst.Tables(0).Rows.Count < 1 Then
                    Me.objCommon.ShowAlert("Attribute Details Not found", Me.Page)
                    Exit Function
                End If

                Me.ViewState(Vs_TableName) = dsMedExMst.Tables(0).Rows(0)("vRefTableName").ToString()

                AddColumnsSearchDtl(dsMedExMst.Tables(0).Rows(0)("vRefColumn").ToString.Split(","))
                Me.ViewState(Vs_DtSearch) = DtSearch

                Me.GVWSearchBuilder.DataSource = DtSearch
                Me.GVWSearchBuilder.DataBind()
            End If

            If Not Me.FillComboMeddra() Then
                Exit Function
            End If



            If Not Me.Request.QueryString("CRFTERM") = Nothing Then
                Dim sender As Object = Nothing
                Dim e As System.EventArgs = Nothing
                Me.Medracode.Value = 1
                Me.checkuncheck()
                Me.Btnback.Visible = True
                BtnGo_Click(sender, e)
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("Error While Loading Page. ", eStr)
            Return False
        End Try

    End Function

#End Region

#Region "FillComboMeddra"

    Private Function FillComboMeddra() As Boolean
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds As New DataSet
        Try

            'wStr = "cActiveFlag <> 'N' And cStatusIndi <> 'D'"
            wStr = "cStatusIndi <> 'D'"
            If Not Me.Request.QueryString("MedExCode") Is Nothing Then

                If Convert.ToString(Me.Request.QueryString("MedExCode")).Trim() <> "" Then
                    wStr += " And cRefTableType = 'D'"
                End If

            End If

            If Not Me.objHelpDb.GetReferenceTableDefinitions(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds.Tables(0) Is Nothing Then
                Me.objCommon.ShowAlert("No MedDRA Found", Me.Page)
                Exit Function
            End If

            Me.ddlMedDRA.DataValueField = ds.Tables(0).Columns("vRefTableName").ToString()
            Me.ddlMedDRA.DataTextField = ds.Tables(0).Columns("vRefTableRemark").ToString()
            Me.ddlMedDRA.DataSource = ds.Tables(0)
            Me.ddlMedDRA.DataBind()

            Me.ddlMedDRA.Items.Insert(0, "--Select MedDRA--")

            Me.ddlMedDRA.SelectedIndex = -1

            Me.ddlMedDRA.Enabled = True
            If Convert.ToString(Me.ViewState(Vs_TableName)).Trim() <> "" Then
                Me.ddlMedDRA.Items.FindByValue(Convert.ToString(Me.ViewState(Vs_TableName)).Trim()).Selected = True
                Me.ddlMedDRA.Enabled = False
            End If



            Return True

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Binding Combo. ", eStr)
            Return False
        End Try
    End Function

#End Region

#Region "AddColumnsSearchDtl"

    Private Sub AddColumnsSearchDtl(ByVal rows() As String) 'As DataTable
        Dim FieldName As DataColumn = New DataColumn("FieldName")
        Dim AliasName As DataColumn = New DataColumn("AliasName")
        Dim dr As DataRow

        Try

            FieldName.DataType = System.Type.GetType("System.String")
            DtSearch.Columns.Add(FieldName)

            AliasName.DataType = System.Type.GetType("System.String")
            DtSearch.Columns.Add(AliasName)

            DtSearch.AcceptChanges()

            For index As Integer = 0 To rows.Length - 1

                dr = DtSearch.NewRow()
                dr("AliasName") = rows(index).Trim() 'rows(index).Substring(0, rows(index).LastIndexOf("_")).ToUpper()
                dr("FieldName") = rows(index).Trim()
                DtSearch.Rows.Add(dr)

            Next

            DtSearch.AcceptChanges()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....AddColumnsSearchDtl")
        End Try
    End Sub

#End Region

#Region "Error Handler"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub

#End Region

#Region "Grid Meddra Events"

    Protected Sub GVWMeddra_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVWMeddra.RowDataBound
        Dim rindex As Integer = e.Row.RowIndex
        Dim Param As String = String.Empty
        Dim displaycolumn(10) As String
        Dim strCellValue As String = ""
        displaycolumn = DisplayFields.Split(",")

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ds As DataSet = CType(Me.ViewState(Vs_Meddra), DataSet)

            If Not CType(e.Row.FindControl("lnkbtnSelect"), LinkButton) Is Nothing Then

                For index As Integer = 1 To e.Row.Cells.Count - 1
                    If Not Param.Trim() = "" Then
                        Param += "##"
                    End If
                    'Param += displaycolumn(index - 1).ToString.Trim() + "=" + e.Row.Cells(index).Text.Trim()
                    Try
                        Param += displaycolumn(index - 1).ToString.Trim() + "=" + ds.Tables(0).Rows(e.Row.RowIndex)(index - 1).ToString()
                    Catch ex As Exception

                    End Try

                Next index
                'Commented and added by Parth Modi on dated 02th-September-2014
                'Reason: Special chaarcter like "(,),-,+" replace with _
                'Param = Param.Replace("(", "_")
                'Param = Param.Replace(")", "_")
                'Param = Param.Replace("-", "_")
                'Param = Param.Replace("+", "_")
                'END IF
                'Commented and added by Parth Modi on dated 09th-June-2014
                'Reason: String that contains ' not performing any action on Select link button :: Issue generate by Dr Dhiraj on 07th-June-2014
                ' CType(e.Row.FindControl("lnkbtnSelect"), LinkButton).Attributes.Add("onclick", "SetMeddraValues('" + Param + "');")


                CType(e.Row.FindControl("lnkbtnSelect"), LinkButton).Attributes.Add("onclick", "SetMeddraValues('" + Param.ToString.Replace("'", "\'") + "');")
                'END
            End If
            'For DataRowIndex As Integer = 0 To e.Row.Cells.Count - 1
            '    strCellValue = e.Row.Cells(DataRowIndex).Text
            '    If strCellValue.Length > 20 Then
            '        e.Row.Cells(DataRowIndex).Attributes.Add("title", strCellValue)
            '        e.Row.Cells(DataRowIndex).Text = strCellValue.Substring(0, 20) + "..."
            '    End If
            'Next DataRowIndex
        End If

    End Sub

#End Region

#Region "Button Go"

    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGo.Click
        Dim wStr As String = String.Empty

        Dim SearchCondition As String = String.Empty
        Dim ds As New DataSet

        Try

            Me.BuildSearchCondition(DisplayFields, wStr)

            'If sender Is Nothing Then
            '    DisplayFields += "llt_name,pt_name,soc_name,Primary_soc_fg"
            '    wStr += "llt_name = '" + Me.Request.QueryString("CRFTERM").ToString.Trim() + "'"
            'End If
            SearchCondition = "Select " + DisplayFields + " From " + Me.ddlMedDRA.SelectedItem.Value.Trim() + " Where "
            SearchCondition += wStr

            ds = Me.objHelpDb.GetResultSet(SearchCondition, Me.ddlMedDRA.SelectedItem.Value.Trim())

            Me.ViewState(Vs_DisplayFields) = DisplayFields
            Me.ViewState(Vs_Meddra) = ds

            Me.GVWMeddra.DataSource = ds
            Me.GVWMeddra.DataBind()
            If ds.Tables(0).Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "bindgGVWMeddra", "bindgGVWMeddra();", True)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....BtnGo_Click")
        End Try
    End Sub

#End Region

#Region "Helper Sub & Function"

    Private Sub BuildSearchCondition(ByRef DisplayFields As String, ByRef wStr As String)
        Dim index As Integer = 0

        For index = 0 To Me.GVWSearchBuilder.Rows.Count - 1
            Dim chk As CheckBox
            Dim hf As HiddenField
            Dim ddl As DropDownList
            Dim txt As TextBox

            chk = CType(Me.GVWSearchBuilder.Rows(index).Cells(GVC_CheckBoxConditionField).FindControl("chkMedraName"), CheckBox)

            If chk.Checked Then

                hf = CType(Me.GVWSearchBuilder.Rows(index).Cells(GVC_CheckBoxConditionField).FindControl("hdnvField"), HiddenField)
                ddl = CType(Me.GVWSearchBuilder.Rows(index).Cells(GVC_CheckBoxConditionField).FindControl("ddlSearchCondition"), DropDownList)
                txt = CType(Me.GVWSearchBuilder.Rows(index).Cells(GVC_SearchText).FindControl("txtSearchValueText"), TextBox)

                If Not hf.Value Is Nothing OrElse Convert.ToString(hf.Value).Trim() <> "" Then
                    If Not wStr.Trim() = "" Then
                        wStr += " And "
                    End If
                    wStr += " " + Convert.ToString(hf.Value).Trim()
                    wStr += " " + ddl.SelectedItem.Text.Trim() + " "
                    wStr += "'"
                    If ddl.SelectedItem.Text.Trim.ToUpper() = "LIKE" Then
                        wStr += "%" + txt.Text.Trim().Replace("'", "''") + "%"
                    Else
                        wStr += txt.Text.Trim().Replace("'", "''")
                    End If
                    wStr += "'"
                End If

            End If

            Dim chkDisplay As CheckBox
            Dim hfDisplay As HiddenField

            chkDisplay = CType(Me.GVWSearchBuilder.Rows(index).Cells(GVC_CheckBoxDisplayField).FindControl("chkDisplayField"), CheckBox)
            hfDisplay = CType(Me.GVWSearchBuilder.Rows(index).Cells(GVC_CheckBoxConditionField).FindControl("hdnvField"), HiddenField)
            If chkDisplay.Checked Then
                If DisplayFields.Trim() <> "" Then
                    DisplayFields += ","
                End If
                DisplayFields += hfDisplay.Value.Trim()
            End If
        Next


    End Sub

#End Region

#Region "DropDownList Events"

    Protected Sub ddlMedDRA_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMedDRA.SelectedIndexChanged
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds As New DataSet
        Dim Columns As String = String.Empty
        Dim index As Integer = 0
        Try
            wStr = "Select * From " + Me.ddlMedDRA.SelectedItem.Value.Trim()
            ds = Me.objHelpDb.GetResultSet(wStr, Me.ddlMedDRA.SelectedItem.Value.Trim())


            For index = 0 To ds.Tables(0).Columns.Count - 1
                If Columns.Trim() <> "" Then
                    Columns += ","
                End If
                Columns += ds.Tables(0).Columns(index).ColumnName.Trim()
            Next

            AddColumnsSearchDtl(Columns.Split(","))
            Me.ViewState(Vs_DtSearch) = DtSearch

            Me.GVWSearchBuilder.DataSource = DtSearch
            Me.GVWSearchBuilder.DataBind()
            Me.Gridshow.Attributes.Add("style", "display :'';")

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "bindgGVWMeddra", "bindgGVWMeddra();", True)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....ddlMedDRA_SelectedIndexChanged")
        End Try
    End Sub

#End Region

    Private Sub checkuncheck()
        Dim hiddenval As HiddenField
        Dim checkboxsearch As CheckBox
        Dim checkboxDisplay As CheckBox
        Dim txtsearch As TextBox
        Dim comparedropdown As DropDownList
        For index = 0 To Me.GVWSearchBuilder.Rows.Count - 1
            hiddenval = CType(Me.GVWSearchBuilder.Rows(index).Cells(GVC_CheckBoxConditionField).FindControl("hdnvField"), HiddenField)
            checkboxDisplay = CType(Me.GVWSearchBuilder.Rows(index).Cells(GVC_CheckBoxDisplayField).FindControl("chkDisplayField"), CheckBox)
            checkboxsearch = CType(Me.GVWSearchBuilder.Rows(index).Cells(GVC_CheckBoxConditionField).FindControl("chkMedraName"), CheckBox)
            txtsearch = CType(Me.GVWSearchBuilder.Rows(index).Cells(GVC_SearchText).FindControl("txtSearchValueText"), TextBox)
            comparedropdown = CType(Me.GVWSearchBuilder.Rows(index).Cells(GVC_CheckBoxConditionField).FindControl("ddlSearchCondition"), DropDownList)
            If Me.ddlMedDRA.SelectedItem.Text.Trim() = "Drug Dictionary" Then 'Added By Maitri on 06-Jun-2015

                If hiddenval.Value = "vDrugName" Then
                    txtsearch.Text = Me.Request.QueryString("CRFTERM").ToString.Trim()
                    checkboxsearch.Checked = True
                    checkboxDisplay.Checked = True
                    comparedropdown.SelectedIndex = 1
                End If
                'If hiddenval.Value = "nDrugNo" Then
                '    checkboxDisplay.Checked = True
                'End If
                If hiddenval.Value = "vSource" Then
                    checkboxsearch.Visible = False
                    checkboxDisplay.Checked = True
                    checkboxDisplay.Enabled = False
                End If
                checkboxDisplay.Checked = True
            Else
                If hiddenval.Value = "vMeddraVersion" Then
                    checkboxsearch.Visible = False
                    checkboxDisplay.Checked = True
                    checkboxDisplay.Enabled = False
                End If
                If columnstring.Contains(hiddenval.Value) Then
                    If hiddenval.Value = "llt_name" Then
                        checkboxsearch.Checked = True
                        txtsearch.Text = Me.Request.QueryString("CRFTERM").ToString.Trim()
                        comparedropdown.SelectedIndex = 1
                    End If
                    'checkboxDisplay.Checked = True
                End If
                checkboxDisplay.Checked = True
                End If
        Next
    End Sub

End Class
