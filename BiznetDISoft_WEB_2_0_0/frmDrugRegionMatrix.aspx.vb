Partial Class frmDrugRegionMatrix
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()


    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtDrugRegionMatrix As String = "DtDrugRegionMatrix"
    Private Const VS_DtBlankDrugRegionMatrix As String = "DtBlankDrugRegionMatrix"
    Private Const VS_DrugCode As String = "DrugCode"

    Private Const GVC_DrugRegionCode As Integer = 1
    Private Const GVC_DrugCode As Integer = 2
    Private Const GVC_DrugName As Integer = 3
    Private Const GVC_RegionCode As Integer = 4
    Private Const GVC_RegionName As Integer = 5
    Private Const GVC_RLDDetails As Integer = 6
    Private Const GVC_Manufacturer As Integer = 7
    Private Const GVC_AvtiveFlag As Integer = 8
    Private Const GVC_Edit As Integer = 9
    Private Const GVC_Delete As Integer = 10

#End Region

#Region "Load Event"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                GenCall()
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
            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_DrugCode) = Me.Request.QueryString("Value").ToString
            End If

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtBlankDrugRegionMatrix) = ds.Tables("DrugRegionMatrix")   ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If

            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall")

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



            Val = Me.ViewState(VS_DrugCode) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vDrugRegionCode=" + Val.ToString
            End If


            If Not objHelp.GetDrugRegionMatrix(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
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
            Me.ShowErrorMessage(ex.Message, ".....GenCall_Data")

        Finally

        End Try
    End Function
#End Region

#Region " GenCall_showUI() "
    Private Function GenCall_ShowUI() As Boolean
        Dim dt_OpMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try


            Page.Title = " :: Drug Region Matrix ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Drug Region Detail"

            dt_OpMst = Me.ViewState(VS_DtDrugRegionMatrix)

            Choice = Me.ViewState(VS_Choice)

            If Not FillDropDown() Then
                Exit Function
            End If

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
        Dim dv_Drug As New DataView
        Dim dv_region As New DataView
        Try
            objHelp.getdrugmst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_Drug, estr)
            objHelp.getregionmst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_region, estr)

            dv_Drug = ds_Drug.Tables(0).DefaultView
            dv_Drug.Sort = "vDrugName"
            dv_region = ds_region.Tables(0).DefaultView
            dv_region.Sort = "vRegionName"

            Me.SlcDrugEdit.DataSource = dv_Drug
            Me.SlcDrugEdit.DataValueField = "vDrugCode"
            Me.SlcDrugEdit.DataTextField = "vDrugName"
            Me.SlcDrugEdit.DataBind()
            Me.SlcDrugEdit.Items.Insert(0, New ListItem("select Drug", ""))

            Me.SlcRegionEdit.DataSource = dv_region
            Me.SlcRegionEdit.DataValueField = "vRegionCode"
            Me.SlcRegionEdit.DataTextField = "vRegionName"
            Me.SlcRegionEdit.DataBind()
            Me.SlcRegionEdit.Items.Insert(0, New ListItem("select Region", ""))

            Me.SlcDrug.DataSource = dv_Drug
            Me.SlcDrug.DataValueField = "vDrugCode"
            Me.SlcDrug.DataTextField = "vDrugName"
            Me.SlcDrug.DataBind()
            Me.SlcDrug.Items.Insert(0, New ListItem("select Drug", ""))

            Me.SlcRegion.DataSource = dv_region
            Me.SlcRegion.DataValueField = "vRegionCode"
            Me.SlcRegion.DataTextField = "vRegionName"
            Me.SlcRegion.DataBind()
            Me.SlcRegion.Items.Insert(0, New ListItem("select Region", ""))


            Me.SlcDrug.Disabled = False
            Me.SlcRegion.Disabled = False
            'Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....FillDropDown")
        Finally

        End Try
    End Function

#End Region

#Region "FillGrid"
    Private Function FillGrid() As Boolean
        Dim ds_Detail As New Data.DataSet
        Dim estr As String = String.Empty
        Try


            If Not objHelp.GetViewDrugRegionMatrix(" cActiveflag<>'N' and vDrugCode='" & _
                                Me.SlcDrugEdit.Value.Trim() & "' and vRegionCode='" & _
                                Me.SlcRegionEdit.Value.Trim() & "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_Detail, estr) Then

                Return False

            End If
            ds_Detail.Tables(0).DefaultView.Sort = "vDrugName"
            Me.GV_Detail.DataSource = ds_Detail.Tables(0).DefaultView.ToTable
            Me.GV_Detail.DataBind()

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillGrid")
            Return False
        End Try
    End Function

#End Region

#Region "Button Click"

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim ds As New DataSet
        Dim ds_Save As New DataSet
        Dim dt_Save As New DataTable
        Dim estr As String = String.Empty
        Dim message As String = String.Empty
        Try



            If Me.SlcDrug.Disabled = False Then
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
            End If

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                If Not AssignValues("Edit") Then
                    Exit Sub
                End If
            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.GenCall_Data(ds)
                Me.ViewState(VS_DtDrugRegionMatrix) = ds.Tables("DrugRegionMatrix")   ' adding blank DataTable in viewstate
                If Not AssignValues("Add") Then
                    Exit Sub
                End If

            End If

            ds_Save = New DataSet
            dt_Save = CType(Me.ViewState(VS_DtDrugRegionMatrix), DataTable).Copy()
            dt_Save.TableName = "DrugRegionMatrix"
            ds_Save.Tables.Add(dt_Save.Copy())

            If Not objLambda.Save_DrugRegionMatrix(Me.ViewState(VS_Choice), ds_Save, Me.Session(S_UserID), estr) Then

                ObjCommon.ShowAlert("Error While Saving DrugRegionMatrix", Me.Page)
                Exit Sub

            End If
            message = IIf(Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, " Drug Region Information Saved SuccessFully !", " Drug Region Information Updated Successfully !")

            ObjCommon.ShowAlert(message, Me.Page)
            ResetPage()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...BtnSave")
        End Try
    End Sub

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.ResetPage()
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click

        Me.TabContainer1.ActiveTabIndex = Me.TabContainer1.ActiveTabIndex + 1
        Me.SlcDrug.Value = Me.SlcDrugEdit.Value.Trim()
        Me.SlcRegion.Value = Me.SlcRegionEdit.Value.Trim()
        Me.SlcDrug.Disabled = False
        Me.SlcRegion.Disabled = False
    End Sub

    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGo.Click

        If Me.SlcDrugEdit.SelectedIndex = 0 Then
            Me.ObjCommon.ShowAlert("Please Select Drug", Me)
            Exit Sub
        ElseIf Me.SlcRegionEdit.SelectedIndex = 0 Then
            Me.ObjCommon.ShowAlert("Please Select Region", Me)
            Exit Sub
        End If

        If Not FillGrid() Then
            Me.ObjCommon.ShowAlert("Error while filling grid", Me)

        End If
        Me.GV_Detail.Visible = True
    End Sub

#End Region

#Region "Assign values"

    Private Function AssignValues(ByVal type As String) As Boolean
        Dim dr As DataRow
        Dim dt_User As New DataTable
        Dim ds_Save As New DataSet
        Dim ds_Check As New DataSet
        Dim estr As String = String.Empty
        Try

            If Not objHelp.GetDrugRegionMatrix("cStatusIndi <> '" + Status_Delete + "' And cActiveFlag <> 'N' And vDrugCode='" & _
                        Me.SlcDrug.Value.Trim() & "' And vRegionCode='" & Me.SlcRegion.Value.Trim() & "'", _
                        WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Check, estr) Then

                Me.ShowErrorMessage("Error While Getting Data from DrugMst", estr)
                Return False

            End If

            If (type.ToUpper = "ADD" And ds_Check.Tables(0).Rows.Count > 0) Or _
                (type.ToUpper = "EDIT" And ds_Check.Tables(0).Rows.Count > 1) Then

                ObjCommon.ShowAlert("Only One Information Can Be Saved For Same Drug And Same Region.", Me.Page)
                Return False

            End If

            If type.ToUpper = "ADD" Then

                dt_User = CType(Me.ViewState(VS_DtBlankDrugRegionMatrix), DataTable)
                dt_User.Clear()
                dr = dt_User.NewRow()
                'vDrugRegionCode,vDrugCode,vRegionCode,vRLDDetails,vManufacturer,cActiveFlag,iModifyBy,dModifyOn,cStatusIndi
                dr("vDrugRegionCode") = "0"
                dr("vDrugCode") = Me.SlcDrug.Value.Trim()
                dr("vRegionCode") = Me.SlcRegion.Value.Trim()
                dr("vRLDDetails") = Me.TxtRLD.Value.Trim()
                dr("vManufacturer") = Me.txtMfg.Value.Trim()
                dr("cActiveFlag") = GeneralModule.ActiveFlag_Yes 'IIf(Me.ChkActive.Checked = True, "Y", "N")
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dt_User.Rows.Add(dr)

            ElseIf type.ToUpper = "EDIT" Then

                dt_User = CType(Me.ViewState(VS_DtDrugRegionMatrix), DataTable)
                For Each dr In dt_User.Rows

                    dr("vRLDDetails") = Me.TxtRLD.Value.Trim()
                    dr("vManufacturer") = Me.txtMfg.Value.Trim()
                    dr("cActiveFlag") = GeneralModule.ActiveFlag_Yes 'IIf(Me.ChkActive.Checked = True, "Y", "N") Changed By Vishal 09-Sep-2008
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "E"
                    dr.AcceptChanges()

                Next dr

            ElseIf type.ToUpper = "DELETE" Then

                dt_User = CType(Me.ViewState(VS_DtDrugRegionMatrix), DataTable)
                For Each dr In dt_User.Rows

                    dr("vRLDDetails") = Me.TxtRLD.Value.Trim()
                    dr("vManufacturer") = Me.txtMfg.Value.Trim()
                    dr("cActiveFlag") = GeneralModule.ActiveFlag_No
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "D"
                    dr.AcceptChanges()

                Next dr

                dt_User.TableName = "DrugRegionMatrix"
                ds_Save.Tables.Add(dt_User.Copy())

                If Not objLambda.Save_DrugRegionMatrix(Me.ViewState(VS_Choice), ds_Save, Me.Session(S_UserID), estr) Then
                    ObjCommon.ShowAlert("Error While Deleteing from DrugRegionMatrix", Me.Page)
                    Exit Function
                End If

                ObjCommon.ShowAlert("Record Deleted SuccessFully", Me.Page)
                FillGrid()
                Me.SlcDrugEdit.SelectedIndex = 0
                Me.SlcRegionEdit.SelectedIndex = 0


            End If
            dt_User.AcceptChanges()
            Me.ViewState(VS_DtDrugRegionMatrix) = dt_User

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...Assign Values")
            Return False
        End Try
    End Function

#End Region

#Region "Reset Page"

    Private Sub ResetPage()
        Me.SlcDrug.SelectedIndex = 0
        Me.SlcRegion.SelectedIndex = 0
        Me.TxtRLD.Value = ""
        Me.txtMfg.Value = ""
        Me.ChkActive.Checked = False
        Me.ViewState(VS_DtDrugRegionMatrix) = Nothing
        Me.SlcDrug.Disabled = False
        Me.SlcRegion.Disabled = False
        Me.BtnSave.Text = "Save"
        Me.BtnSave.ToolTip = "Save"
        Me.GV_Detail.Visible = False

        If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
            Me.GenCall()
        ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
            FillGrid()
            Me.TabContainer1.ActiveTabIndex = Me.TabContainer1.ActiveTabIndex - 1
        End If

    End Sub

#End Region

#Region "Grid Events"

    Protected Sub GV_Detail_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV_Detail.RowCreated
        Try

            If e.Row.RowType = DataControlRowType.DataRow Or _
                  e.Row.RowType = DataControlRowType.Header Or _
                  e.Row.RowType = DataControlRowType.Footer Then

                e.Row.Cells(GVC_DrugRegionCode).Visible = False
                e.Row.Cells(GVC_DrugCode).Visible = False
                e.Row.Cells(GVC_RegionCode).Visible = False

            End If

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GV_Detail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV_Detail.RowDataBound

        Try
            'If Not e.Row.RowType = DataControlRowType.Pager Then
            '    e.Row.Cells(GVC_DrugRegionCode).Visible = False
            '    e.Row.Cells(GVC_DrugCode).Visible = False
            '    e.Row.Cells(GVC_RegionCode).Visible = False

            If e.Row.RowType = DataControlRowType.DataRow Then

                CType(e.Row.FindControl("ImbEdit"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImbEdit"), ImageButton).CommandName = "Edit"
                CType(e.Row.FindControl("ImbDelete"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImbDelete"), ImageButton).CommandName = "Delete"
                CType(e.Row.FindControl("LblSrNo"), Label).Text = e.Row.RowIndex + (Me.GV_Detail.PageSize * Me.GV_Detail.PageIndex) + 1

            End If
            'End If
        Catch ex As Exception
        Finally
        End Try

    End Sub

    Protected Sub GV_Detail_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GV_Detail.RowCommand
        Dim index As Integer = e.CommandArgument
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim ds As New DataSet
        Dim dt As New DataTable

        wstr = "vDrugRegionCode='" & Me.GV_Detail.Rows(index).Cells(GVC_DrugRegionCode).Text & "'"
        If Not objHelp.GetDrugRegionMatrix(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr) Then
            Response.Write(estr)
            Exit Sub
        End If

        Me.ViewState(VS_DtDrugRegionMatrix) = ds.Tables(0)
        dt = Me.ViewState(VS_DtDrugRegionMatrix)

        Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit

        If e.CommandName.ToUpper = "EDIT" Then

            Dim dr As DataRow = dt.Rows(0)
            Me.SlcDrug.Value = dr("vDrugCode")
            Me.SlcRegion.Value = dr("vRegionCode")
            Me.txtMfg.Value = dr("vManufacturer")
            Me.TxtRLD.Value = dr("vRLDDetails")
            Me.ChkActive.Checked = IIf(dr("cActiveFlag") = GeneralModule.ActiveFlag_Yes, True, False)
            Me.SlcDrug.Disabled = True
            Me.SlcRegion.Disabled = True

            Me.ViewState(VS_DtDrugRegionMatrix) = dt

            Me.TabContainer1.ActiveTabIndex = Me.TabContainer1.ActiveTabIndex + 1
            Me.BtnSave.Text = "Update"
            Me.BtnSave.ToolTip = "Update"
        ElseIf e.CommandName.ToUpper = "DELETE" Then

            AssignValues("Delete")

        End If
    End Sub

    Protected Sub GV_Detail_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GV_Detail.RowDeleting

    End Sub

    Protected Sub GV_Detail_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GV_Detail.RowEditing
        e.Cancel = True
    End Sub

    Protected Sub GV_Detail_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GV_Detail.PageIndexChanging
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
