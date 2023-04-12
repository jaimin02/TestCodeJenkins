
Partial Class frmDrugAnalytesPkDetail
    Inherits System.Web.UI.Page
    Dim ObjCommon As New clsCommon
    Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtDrugRegionPKMatrix As String = "DtDrugRegionPKMatrix"
    Private Const VS_DtBlankDrugRegionPKMatrix As String = "DtBlankDrugRegionPKMatrix"
    Private Const VS_DrugRegionPKCode As String = "vDrugRegionPKCode"
    Private Const GVC_DrugRegionPKCode As Integer = 1
    Private Const GVC_DrugRegionCode As Integer = 2
    Private Const GVC_DrugCode As Integer = 3
    Private Const GVC_DrugName As Integer = 4
    Private Const GVC_RegionCode As Integer = 5
    Private Const GVC_RegionName As Integer = 6
    Private Const GVC_RLD As Integer = 7
    Private Const GVC_MetNo As Integer = 8
    Private Const GVC_MCode As Integer = 9
    Private Const GVC_MValue As Integer = 10
    Private Const GVC_MMaxValue As Integer = 11
    Private Const GVC_MHalfValue As Integer = 12
    Private Const GVC_AvtiveFlag As Integer = 13
    Private Const GVC_Edit As Integer = 14
    Private Const GVC_Delete As Integer = 15

#Region "Load Event"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                GenCall()
                Me.DivAdd.Visible = False
                Me.BtnAdd.Enabled = False
                'Me.TxtWashOutPeriod.Attributes.Add("onblur", "Numeric('Washout');")
                'Me.TxtHousing.Attributes.Add("onblur", "Numeric('Housing');")
            End If
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region " GenCall() "
    Private Function GenCall() As Boolean
        Dim ds As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try
            Choice = Me.Request.QueryString("Mode")
            Me.ViewState(VS_Choice) = Choice   'To use it while saving
            If (Not Me.Request.QueryString("Mode") Is Nothing) And (Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add) Then
                Me.ViewState(VS_DrugRegionPKCode) = Me.Request.QueryString("Value").ToString
            End If

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtBlankDrugRegionPKMatrix) = ds.Tables("DrugRegionPKMatrix")   ' adding blank DataTable in viewstate

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

#Region " GenCall_Data "
    Private Function GenCall_Data(ByRef ds_DWR_Retu As DataSet) As Boolean

        Dim wStr As String = ""
        Dim eStr_Retu As String = ""
        Dim Val As String = ""
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing
        'Dim objFFR As New FFReporting.FFReporting

        Try

            Val = Me.ViewState(VS_DrugRegionPKCode) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vDrugRegionPKCode=" + Val.ToString
            End If


            If Not objHelp.GetDrugRegionPKMatrix(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
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
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")

        Finally

        End Try
    End Function
#End Region

#Region " GenCall_showUI() "
    Private Function GenCall_ShowUI() As Boolean
        Dim dt_DrugRegionPKMatrix As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try
            Page.Title = " :: Drug Analyst ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Drug Region PK Detail"

            dt_DrugRegionPKMatrix = Me.ViewState(VS_DtDrugRegionPKMatrix)

            Choice = Me.ViewState(VS_Choice)

            'If Me.Request.QueryString("Save") = "Y" Then
            '    ObjCommon.ShowAlert("Record Saved SuccessFully", Me.Page)
            'End If
            If Not FillDropDown() Then
                Exit Function
            End If

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                'Me.txtDrugName.Text = dt_OpMst.Rows(0).Item("vDrugName")
                'Me.TxtWashOutPeriod.Text = dt_OpMst.Rows(0).Item("vWashOutPeriod")
                'Me.TxtHousing.text = dt_OpMst.Rows(0).Item("vHousing")
                'Me.txtSynonyms.Text = dt_OpMst.Rows(0).Item("vDrugSynonyms")
                'Me.txtFood.Text = dt_OpMst.Rows(0).Item("vFoodEffect")
                'If dt_OpMst.Rows(0).Item("cActiveFlag") = GeneralModule.ActiveFlag_Yes Then
                '    Me.ChkActive.Checked = True
                'Else
                '    Me.ChkActive.Checked = False
                'End If
            End If
            'Me.BtnSave.Attributes.Add("OnClick", "return Validation();")

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function
#End Region

#Region "FillDropDown"
    Private Function FillDropDown() As Boolean
        Dim ds_Drug As New Data.DataSet
        Dim ds_region As New Data.DataSet
        Dim estr As String = ""
        Try
            objHelp.getdrugmst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_Drug, estr)
            objHelp.getregionmst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_region, estr)

            Me.SlcDrug.DataSource = ds_Drug
            Me.SlcDrug.DataValueField = "vDrugCode"
            Me.SlcDrug.DataTextField = "vDrugName"
            Me.SlcDrug.DataBind()
            Me.SlcDrug.Items.Insert(0, New ListItem("select Drug", ""))

            Me.SlcRegion.DataSource = ds_region
            Me.SlcRegion.DataValueField = "vRegionCode"
            Me.SlcRegion.DataTextField = "vRegionName"
            Me.SlcRegion.DataBind()
            Me.SlcRegion.Items.Insert(0, New ListItem("select Region", ""))

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Function
#End Region

#Region "FillGrid"
    Private Function FillGrid() As Boolean
        Dim ds_Detail As New Data.DataSet
        Dim ds_Code As New Data.DataSet
        Dim estr As String = ""
        Try
            If Not objHelp.GetViewDrugRegionPKMatrix(" DrugRegionPKActive<>'N' and vDrugCode='" & Me.SlcDrug.Value.Trim() & _
                                "' and vRegionCode='" & Me.SlcRegion.Value.Trim() & "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Detail, estr) Then
                Return False
            End If
            Me.GV_Detail.DataSource = ds_Detail
            Me.GV_Detail.DataBind()
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region "Button Click"
    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim ds As New DataSet
        Dim ds_Save As New DataSet
        Dim dt_Save As New DataTable
        Dim estr As String = ""
        Try

            If Me.SlcDrug.Disabled = False Then
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
            End If

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                AssignValues("Edit")
            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.GenCall_Data(ds)
                Me.ViewState(VS_DtDrugRegionPKMatrix) = ds.Tables("DrugRegionPKMatrix")   ' adding blank DataTable in viewstate
                AssignValues("Add")
            End If

            ds_Save = New DataSet
            dt_Save = CType(Me.ViewState(VS_DtDrugRegionPKMatrix), DataTable)
            dt_Save.TableName = "DrugRegionPKMatrix"
            ds_Save.Tables.Add(dt_Save)
            If Not objLambda.Save_DrugRegionPKMatrix(Me.ViewState(VS_Choice), ds_Save, Me.Session(S_UserID), estr) Then
                ObjCommon.ShowAlert("Error While Saving DrugRegionPKMatrix", Me.Page)
                Exit Sub
            Else
                If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                    ObjCommon.ShowAlert("Record Edited SuccessFully", Me.Page)
                ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                    ObjCommon.ShowAlert("Record Saved SuccessFully", Me.Page)
                End If

                'Me.Response.Write("Record Saved SuccessFully")
                ResetPage()
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub
    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click
        ResetPage()
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGo.Click
        If Me.SlcDrug.SelectedIndex = 0 Then
            Me.ObjCommon.ShowAlert("Please Select Drug", Me)
            Exit Sub
        ElseIf Me.SlcRegion.SelectedIndex = 0 Then
            Me.ObjCommon.ShowAlert("Please Select Region", Me)
            Exit Sub
        End If
        Me.BtnAdd.Enabled = True
        If Not FillGrid() Then
            Me.ObjCommon.ShowAlert("Error while filling grid", Me)
        End If
    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click
        'Me.TabContainer1.ActiveTabIndex = Me.TabContainer1.ActiveTabIndex + 1
        Dim ds_Code As New Data.DataSet
        Dim estr As String = ""
        If Not objHelp.GetDrugRegionMatrix(" cActiveFlag<>'N' and vDrugCode='" & Me.SlcDrug.Value.Trim() & _
                    "' and vRegionCode='" & Me.SlcRegion.Value.Trim() & "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Code, estr) Then
            Me.ObjCommon.ShowAlert("Error while Getting DrugRegionCode", Me)
        End If
        If ds_Code.Tables(0).Rows.Count > 0 Then
            Me.HdfDrugRegion.Value = ds_Code.Tables(0).Rows(0).Item("vDrugRegionCode")
        End If
        Me.DivAdd.Visible = True
        Me.SlcDrug.Disabled = False
        Me.SlcRegion.Disabled = False

    End Sub
#End Region

#Region "Assign values"
    Private Sub AssignValues(ByVal type As String)
        Dim dr As DataRow
        Dim dt_User As New DataTable
        Dim ds_Save As New DataSet
        Dim estr As String = ""
        Try

            If type.ToUpper = "ADD" Then
                dt_User = CType(Me.ViewState(VS_DtBlankDrugRegionPKMatrix), DataTable)
                dt_User.Clear()
                dr = dt_User.NewRow()
                'vDrugRegionPKCode, vDrugRegionCode, iMetNo, vMCode, vMValue, vMMaxValue, vMHalfValue, cActiveFlag, iModifyBy, dModifyOn, cStatusIndi
                dr("vDrugRegionPKCode") = "0"
                dr("vDrugRegionCode") = Me.HdfDrugRegion.Value
                'dr("iMetNo") = Me.SlcRegion.Value.Trim()
                dr("vMCode") = Me.txtCode.Text.Trim()
                dr("vMValue") = Me.txtValue.Text.Trim()
                dr("vMMaxValue") = Me.txtMaxValue.Text.Trim()
                dr("vMHalfValue") = Me.txtHalfValue.Text.Trim()
                dr("cActiveFlag") = IIf(Me.ChkActive.Checked = True, "Y", "N")
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dt_User.Rows.Add(dr)
            ElseIf type.ToUpper = "EDIT" Then
                dt_User = CType(Me.ViewState(VS_DtDrugRegionPKMatrix), DataTable)
                For Each dr In dt_User.Rows
                    'dr("vDrugRegionPKCode") = "0"
                    'dr("vDrugRegionCode") = ""
                    'dr("iMetNo") = Me.SlcRegion.Value.Trim()
                    dr("vMCode") = Me.txtCode.Text.Trim()
                    dr("vMValue") = Me.txtValue.Text.Trim()
                    dr("vMMaxValue") = Me.txtMaxValue.Text.Trim()
                    dr("vMHalfValue") = Me.txtHalfValue.Text.Trim()
                    dr("cActiveFlag") = IIf(Me.ChkActive.Checked = True, "Y", "N")
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "E"
                    dr.AcceptChanges()
                Next
            ElseIf type.ToUpper = "DELETE" Then
                dt_User = CType(Me.ViewState(VS_DtDrugRegionPKMatrix), DataTable)
                For Each dr In dt_User.Rows
                    'dr("vDrugRegionPKCode") = "0"
                    'dr("vDrugRegionCode") = ""
                    'dr("iMetNo") = Me.SlcRegion.Value.Trim()
                    dr("vMCode") = Me.txtCode.Text.Trim()
                    dr("vMValue") = Me.txtValue.Text.Trim()
                    dr("vMMaxValue") = Me.txtMaxValue.Text.Trim()
                    dr("vMHalfValue") = Me.txtHalfValue.Text.Trim()
                    dr("cActiveFlag") = GeneralModule.ActiveFlag_No
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "E"
                    dr.AcceptChanges()
                Next
                dt_User.TableName = "DrugRegionPKMatrix"
                ds_Save.Tables.Add(dt_User.Copy())
                If Not objLambda.Save_DrugRegionPKMatrix(Me.ViewState(VS_Choice), ds_Save, Me.Session(S_UserID), estr) Then
                    ObjCommon.ShowAlert("Error While Deleteing from DrugRegionPKMatrix", Me.Page)
                    Exit Sub
                Else
                    ObjCommon.ShowAlert("Record Deleted SuccessFully", Me.Page)
                    FillGrid()
                    'Me.Response.Write("Record Saved SuccessFully")
                End If
            End If
            dt_User.AcceptChanges()
            Me.ViewState(VS_DtDrugRegionPKMatrix) = dt_User

        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Reset Page"
    Private Sub ResetPage()
        'Me.SlcDrug.SelectedIndex = 0
        'Me.SlcRegion.SelectedIndex = 0
        Me.txtCode.Text = ""
        Me.txtValue.Text = ""
        Me.txtMaxValue.Text = ""
        Me.txtHalfValue.Text = ""
        Me.ChkActive.Checked = False
        Me.ViewState(VS_DtDrugRegionPKMatrix) = Nothing
        Me.SlcDrug.Disabled = False
        Me.SlcRegion.Disabled = False
        Me.DivAdd.Visible = False
        'Me.BtnAdd.Enabled = False
        FillGrid()
        'If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
        '    Me.GenCall()
        '    'ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
        '    '    Me.DivAdd.Visible = False
        '    '    'Me.TabContainer1.ActiveTabIndex = Me.TabContainer1.ActiveTabIndex - 1
        'End If
        'Me.Response.Redirect("frmDrugRegionPKMatrix.aspx?mode=1&Save=Y")
        'Me.Response.Redirect("frmDrugRegionPKMatrix.aspx?mode=1")
    End Sub
#End Region

#Region "Error Handler"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        ObjCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        ObjCommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub
#End Region

#Region "Grid Event"

    Protected Sub GV_Detail_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV_Detail.RowCreated
        e.Row.Cells(GVC_DrugRegionPKCode).Visible = False
        e.Row.Cells(GVC_DrugRegionCode).Visible = False
        e.Row.Cells(GVC_DrugCode).Visible = False
        e.Row.Cells(GVC_RegionCode).Visible = False
        e.Row.Cells(GVC_MetNo).Visible = False
    End Sub

    Protected Sub GV_Detail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV_Detail.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("ImbEdit"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("ImbEdit"), ImageButton).CommandName = "Edit"
            CType(e.Row.FindControl("ImbDelete"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("ImbDelete"), ImageButton).CommandName = "Delete"

            CType(e.Row.FindControl("LblSrNo"), Label).Text = e.Row.RowIndex + 1

        End If
    End Sub

    Protected Sub GV_Detail_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GV_Detail.RowCommand
        Dim index As Integer = e.CommandArgument
        Dim wstr As String = ""
        Dim estr As String = ""
        Dim ds As New DataSet
        Dim dt As New DataTable

        wstr = "vDrugRegionPKCode='" & Me.GV_Detail.Rows(index).Cells(GVC_DrugRegionPKCode).Text & "'"
        If Not objHelp.GetDrugRegionPKMatrix(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr) Then
            Response.Write(estr)
            Exit Sub
        End If

        Me.ViewState(VS_DtDrugRegionPKMatrix) = ds.Tables(0)
        dt = Me.ViewState(VS_DtDrugRegionPKMatrix)
        If e.CommandName.ToUpper = "EDIT" Then

            Dim dr As DataRow = dt.Rows(0)
            'Me.SlcDrug.Value = dr("vDrugCode")
            'Me.SlcRegion.Value = dr("vRegionCode")
            Me.txtCode.Text = dr("vMCode")
            Me.txtValue.Text = dr("vMValue")
            Me.txtMaxValue.Text = dr("vMMaxValue")
            Me.txtHalfValue.Text = dr("vMHalfValue")
            Me.ChkActive.Checked = IIf(dr("cActiveFlag") = GeneralModule.ActiveFlag_Yes, True, False)
            Me.SlcDrug.Disabled = True
            Me.SlcRegion.Disabled = True

            Me.ViewState(VS_DtDrugRegionPKMatrix) = dt

            Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
            Me.DivAdd.Visible = True
            'Me.TabContainer1.ActiveTabIndex = Me.TabContainer1.ActiveTabIndex + 1
        ElseIf e.CommandName.ToUpper = "DELETE" Then
            Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
            AssignValues("Delete")
        End If
    End Sub

    Protected Sub GV_Detail_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GV_Detail.RowDeleting

    End Sub

    Protected Sub GV_Detail_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GV_Detail.RowEditing
        e.Cancel = True
    End Sub
#End Region

    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        ResetPage()
    End Sub
End Class
