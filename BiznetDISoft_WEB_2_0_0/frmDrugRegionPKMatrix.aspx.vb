Partial Class frmDrugRegionPKMatrix
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtDrugRegionPKMatrix As String = "DtDrugRegionPKMatrix"
    Private Const VS_DtBlankDrugRegionPKMatrix As String = "DtBlankDrugRegionPKMatrix"
    Private Const VS_DrugRegionPKCode As String = "vDrugRegionPKCode"

    Private Const GVC_DrugAnalytesCode As Integer = 1
    Private Const GVC_DrugRegionCode As Integer = 2
    Private Const GVC_DrugCode As Integer = 3
    Private Const GVC_DrugName As Integer = 4
    Private Const GVC_RegionCode As Integer = 5
    Private Const GVC_RegionName As Integer = 6
    Private Const GVC_Analytes As Integer = 7
    Private Const GVC_RLD As Integer = 8
    Private Const GVC_MetNo As Integer = 9
    Private Const GVC_MCode As Integer = 10
    Private Const GVC_MValue As Integer = 11
    Private Const GVC_MMaxValue As Integer = 12
    Private Const GVC_MHalfValue As Integer = 13
    Private Const GVC_AvtiveFlag As Integer = 14
    Private Const GVC_Edit As Integer = 15
    Private Const GVC_Delete As Integer = 16

#End Region

#Region "Page Load "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                GenCall()
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

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
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
            Me.ShowErrorMessage(ex.Message, ".......GenCall")

        Finally

        End Try

    End Function
#End Region

#Region " GenCall_Data "
    Private Function GenCall_Data(ByRef ds_DWR_Retu As DataSet) As Boolean

        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Val As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing

        Try


            Val = Me.ViewState(VS_DrugRegionPKCode) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vDrugAnalytesCode=" + Val.ToString
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
                Throw New Exception("No Records Found For Selected Operation")
            End If

            ds_DWR_Retu = ds
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_Data")

        Finally

        End Try
    End Function
#End Region

#Region "GenCall_showUI "
    Private Function GenCall_ShowUI() As Boolean
        Dim dt_DrugRegionPKMatrix As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try


            Page.Title = " :: Drug Region PK Matrix ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Drug Region Analytes PK Detail"

            dt_DrugRegionPKMatrix = Me.ViewState(VS_DtDrugRegionPKMatrix)

            Choice = Me.ViewState(VS_Choice)

            If Not FillDropDown() Then
                Exit Function
            End If

            Me.DivAdd.Visible = False
            Me.BtnAdd.Enabled = False

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....GenCall_ShowUI")
        Finally
        End Try
    End Function
#End Region

#Region "FillDropDown"

    Private Function FillDropDown() As Boolean
        Dim ds_Drug As New Data.DataSet
        Dim ds_region As New Data.DataSet
        Dim estr As String = String.Empty
        Dim dv_DrugName As New DataView
        Dim dv_RegionName As New DataView
        Try



            objHelp.getdrugmst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_Drug, estr)
            objHelp.getregionmst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_region, estr)

            dv_DrugName = ds_Drug.Tables(0).DefaultView
            dv_DrugName.Sort = "vDrugName"
            Me.SlcDrug.DataSource = dv_DrugName
            Me.SlcDrug.DataValueField = "vDrugCode"
            Me.SlcDrug.DataTextField = "vDrugName"
            Me.SlcDrug.DataBind()
            Me.SlcDrug.Items.Insert(0, New ListItem("--Select Drug--", ""))

            dv_RegionName = ds_region.Tables(0).DefaultView
            dv_RegionName.Sort = "vRegionName"
            Me.SlcRegion.DataSource = dv_RegionName
            Me.SlcRegion.DataValueField = "vRegionCode"
            Me.SlcRegion.DataTextField = "vRegionName"
            Me.SlcRegion.DataBind()
            Me.SlcRegion.Items.Insert(0, New ListItem("--select Region--", ""))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillDropDown")
            Return False
        End Try
    End Function

    Private Function FillAnalytesCombo() As Boolean
        Dim ds_Detail As New Data.DataSet
        Dim ds_Code As New Data.DataSet
        Dim estr As String = String.Empty
        Dim dv_Analytes As New DataView
        Try



            If Not objHelp.GetDrugAnalytesMatrix("vDrugCode='" & Me.SlcDrug.SelectedValue.Trim() & "' And cStatusIndi<>'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Detail, estr) Then
                Return False
            End If

            dv_Analytes = ds_Detail.Tables(0).DefaultView.ToTable(True, "vDrugAnalytesCode,vAnalytes".Split(",")).DefaultView
            dv_Analytes.Sort = "vAnalytes"
            ddlAnalytes.DataSource = dv_Analytes
            ddlAnalytes.DataValueField = "vDrugAnalytesCode"
            ddlAnalytes.DataTextField = "vAnalytes"
            ddlAnalytes.DataBind()
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillAnalytesCombo")
            Return False
        End Try
    End Function

#End Region

#Region "FillGrid"

    Private Function FillGrid() As Boolean
        Dim ds_Detail As New Data.DataSet
        Dim ds_Code As New Data.DataSet
        Dim estr As String = String.Empty
        Dim whereCondition As String = String.Empty
        Try

            
            whereCondition = " DrugREgionPKActive<>'N'"
            whereCondition += " and vDrugCode='" & Me.SlcDrug.SelectedValue.Trim & "'"
            whereCondition += " and vRegionCode='" & Me.SlcRegion.SelectedValue.Trim() & "'"
            whereCondition += " and vDrugAnalytesCode='" & Me.ddlAnalytes.Items(Me.ddlAnalytes.SelectedIndex).Value.Trim() & "'"

            If Not objHelp.GetViewDrugRegionPKMatrix(whereCondition, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Detail, estr) Then
                Return False
            End If

            Me.GV_Detail.DataSource = ds_Detail
            Me.GV_Detail.DataBind()
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillGrid")
            Return False
        End Try
    End Function

#End Region

#Region "Drop Down Events "

    Protected Sub SlcRegion_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Me.SlcDrug.SelectedIndex > 0 Then
            FillAnalytesCombo()
        End If

    End Sub

    Protected Sub SlcDrug_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Me.SlcDrug.SelectedIndex > 0 Then
            FillAnalytesCombo()
        End If

    End Sub

#End Region

#Region "Button Click"

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim ds As New DataSet
        Dim ds_Save As New DataSet
        Dim dt_Save As New DataTable
        Dim estr As String = String.Empty
        Try



            If Me.SlcDrug.Enabled = True Then
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

            End If

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                ObjCommon.ShowAlert("Drug Analytes PK information Edited SuccessFully !", Me.Page)
            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                ObjCommon.ShowAlert("Drug Analytes PK Information Saved SuccessFully !", Me.Page)
            End If

            ResetPage()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...BtnSave_Click")
        End Try
    End Sub

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGo.Click

        If Me.ddlAnalytes.Items.Count <= 0 Or Me.ddlAnalytes.SelectedIndex < 0 Then

            Me.ObjCommon.ShowAlert("No Analytes, Please Enter Analytes !", Me)
            Exit Sub

        End If

        Me.BtnAdd.Enabled = True

        ResetPage()

    End Sub

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click
        Dim ds_Code As New Data.DataSet
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty

        Wstr = " cActiveFlag<>'N' and vDrugCode='" & Me.SlcDrug.SelectedValue.Trim() & "' and vRegionCode='" & Me.SlcRegion.SelectedValue.Trim() & "'"

        If Not objHelp.GetDrugRegionMatrix(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Code, estr) Then
            Me.ObjCommon.ShowAlert("Error While Getting DrugRegionCode", Me)
        End If

        If ds_Code.Tables(0).Rows.Count <= 0 Then
            ObjCommon.ShowAlert("DrugRegion Record Does Not Exist !", Me)
            Exit Sub
        End If

        Me.HdfDrugRegion.Value = ds_Code.Tables(0).Rows(0).Item("vDrugRegionCode")

        Me.DivAdd.Visible = True
        Me.SlcDrug.Enabled = True
        Me.SlcRegion.Enabled = True

    End Sub

    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        ResetPage()
    End Sub

#End Region

#Region "Assign values"
    Private Sub AssignValues(ByVal type As String)
        Dim dr As DataRow
        Dim dt_User As New DataTable
        Dim ds_Save As New DataSet
        Dim estr As String = String.Empty
        Try

            If type.ToUpper = "ADD" Then

                dt_User = CType(Me.ViewState(VS_DtBlankDrugRegionPKMatrix), DataTable)
                dt_User.Clear()
                dr = dt_User.NewRow()
                'vDrugRegionPKCode, vDrugRegionCode, iMetNo, vMCode, vMValue, vMMaxValue, vMHalfValue, cActiveFlag, iModifyBy, dModifyOn, cStatusIndi
                dr("vDrugAnalytesCode") = Me.ddlAnalytes.Value
                dr("vDrugRegionCode") = Me.HdfDrugRegion.Value
                dr("vDrugCode") = Me.SlcDrug.SelectedValue.Trim
                dr("vMCode") = Me.txtCode.Text.Trim()
                dr("vMValue") = Me.txtValue.Text.Trim()
                dr("vMMaxValue") = Me.txtMaxValue.Text.Trim()
                dr("vMHalfValue") = Me.txtHalfValue.Text.Trim()
                dr("cActiveFlag") = GeneralModule.ActiveFlag_Yes 'IIf(Me.ChkActive.Checked = True, "Y", "N") 'Changed by Vishal 09-Sep-2008
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dt_User.Rows.Add(dr)

            ElseIf type.ToUpper = "EDIT" Then
                dt_User = CType(Me.ViewState(VS_DtDrugRegionPKMatrix), DataTable)
                For Each dr In dt_User.Rows

                    dr("vDrugCode") = Me.SlcDrug.SelectedValue.Trim
                    dr("vMCode") = Me.txtCode.Text.Trim()
                    dr("vMValue") = Me.txtValue.Text.Trim()
                    dr("vMMaxValue") = Me.txtMaxValue.Text.Trim()
                    dr("vMHalfValue") = Me.txtHalfValue.Text.Trim()
                    dr("cActiveFlag") = GeneralModule.ActiveFlag_Yes 'IIf(Me.ChkActive.Checked = True, "Y", "N") 'Changed by Vishal 09-Sep-2008
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "E"
                    dr.AcceptChanges()

                Next dr

            ElseIf type.ToUpper = "DELETE" Then
                dt_User = CType(Me.ViewState(VS_DtDrugRegionPKMatrix), DataTable)
                For Each dr In dt_User.Rows

                    dr("vDrugCode") = Me.SlcDrug.SelectedValue.Trim
                    dr("vMCode") = Me.txtCode.Text.Trim()
                    dr("vMValue") = Me.txtValue.Text.Trim()
                    dr("vMMaxValue") = Me.txtMaxValue.Text.Trim()
                    dr("vMHalfValue") = Me.txtHalfValue.Text.Trim()
                    dr("cActiveFlag") = GeneralModule.ActiveFlag_No
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "C"
                    dr.AcceptChanges()

                Next dr

                dt_User.TableName = "DrugRegionPKMatrix"
                ds_Save.Tables.Add(dt_User.Copy())

                If Not objLambda.Save_DrugRegionPKMatrix(Me.ViewState(VS_Choice), ds_Save, Me.Session(S_UserID), estr) Then

                    ObjCommon.ShowAlert("Error While Deleteing From DrugRegionPKMatrix", Me.Page)
                    Exit Sub

                End If

                ObjCommon.ShowAlert("Drug Region PK Information Deleted SuccessFully", Me.Page)
                FillGrid()

            End If

            dt_User.AcceptChanges()
            Me.ViewState(VS_DtDrugRegionPKMatrix) = dt_User

        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Reset Page"

    Private Sub ResetPage()

        Me.txtCode.Text = ""
        Me.txtValue.Text = ""
        Me.txtMaxValue.Text = ""
        Me.txtHalfValue.Text = ""
        Me.ChkActive.Checked = True
        Me.ViewState(VS_DtDrugRegionPKMatrix) = Nothing
        Me.SlcDrug.Enabled = True
        Me.SlcRegion.Enabled = True
        Me.DivAdd.Visible = False
        If Not FillGrid() Then
            Exit Sub
        End If
    End Sub

#End Region

#Region "Grid Event"

    Protected Sub GV_Detail_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV_Detail.RowCreated

        If e.Row.RowType = DataControlRowType.DataRow Or _
                      e.Row.RowType = DataControlRowType.Header Or _
                      e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVC_DrugAnalytesCode).Visible = False
            e.Row.Cells(GVC_DrugRegionCode).Visible = False
            e.Row.Cells(GVC_DrugCode).Visible = False
            e.Row.Cells(GVC_RegionCode).Visible = False
            e.Row.Cells(GVC_MetNo).Visible = False
            e.Row.Cells(GVC_MCode).Visible = False

        End If
        

    End Sub

    Protected Sub GV_Detail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV_Detail.RowDataBound

        If e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(GVC_MHalfValue).Text = "T<sub>1/2</sub>Value"
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then

            CType(e.Row.FindControl("ImbEdit"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("ImbEdit"), ImageButton).CommandName = "Edit"
            CType(e.Row.FindControl("ImbDelete"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("ImbDelete"), ImageButton).CommandName = "Delete"

            CType(e.Row.FindControl("LblSrNo"), Label).Text = e.Row.RowIndex + (Me.GV_Detail.PageSize * Me.GV_Detail.PageIndex) + 1

        End If
    End Sub

    Protected Sub GV_Detail_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GV_Detail.RowCommand
        Dim index As Integer = e.CommandArgument
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim ds As New DataSet
        Dim dt As New DataTable

        wstr = "vDrugAnalytesCode='" & Me.GV_Detail.Rows(index).Cells(GVC_DrugAnalytesCode).Text & "'" & _
                " AND vDrugRegionCode='" & Me.GV_Detail.Rows(index).Cells(GVC_DrugRegionCode).Text & "'" & _
                " AND iMetNo=" & Me.GV_Detail.Rows(index).Cells(GVC_MetNo).Text


        If Not objHelp.GetDrugRegionPKMatrix(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr) Then
            Response.Write(estr)
            Exit Sub
        End If

        Me.ViewState(VS_DtDrugRegionPKMatrix) = ds.Tables(0)
        dt = Me.ViewState(VS_DtDrugRegionPKMatrix)

        Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit

        If e.CommandName.ToUpper = "EDIT" Then

            Dim dr As DataRow = dt.Rows(0)
            Me.txtCode.Text = dr("vMCode")
            Me.txtValue.Text = dr("vMValue")
            Me.txtMaxValue.Text = dr("vMMaxValue")
            Me.txtHalfValue.Text = dr("vMHalfValue")
            Me.ChkActive.Checked = IIf(dr("cActiveFlag") = GeneralModule.ActiveFlag_Yes, True, False)
            Me.SlcDrug.Enabled = False
            Me.SlcRegion.Enabled = False

            Me.ViewState(VS_DtDrugRegionPKMatrix) = dt

            Me.DivAdd.Visible = True

        ElseIf e.CommandName.ToUpper = "DELETE" Then

            AssignValues("Delete")

        End If
    End Sub

    Protected Sub GV_Detail_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GV_Detail.RowDeleting

    End Sub

    Protected Sub GV_Detail_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GV_Detail.RowEditing
        e.Cancel = True
    End Sub

    Protected Sub GV_Detail_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)

        Me.GV_Detail.PageIndex = e.NewPageIndex
        FillGrid()

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

End Class
