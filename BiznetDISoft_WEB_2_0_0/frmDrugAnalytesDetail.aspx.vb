
Partial Class frmDrugAnalyseDetail
    Inherits System.Web.UI.Page

#Region " VARIABLE DECLARATION "

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()


    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtDrugAnalytesMatrix As String = "DtDrugAnalytesMatrix"
    Private Const VS_DtBlankDrugAnalytesMatrix As String = "DtBlankDrugAnalytesMatrix"
    Private Const VS_DrugCode As String = "DrugCode"

    Private Const GVC_DrugRegionCode As Integer = 1
    Private Const GVC_DrugCode As Integer = 2
    Private Const GVC_DrugName As Integer = 3
    Private Const GVC_analytes As Integer = 4
    Private Const GVC_RLDDetails As Integer = 6
    Private Const GVC_Manufacturer As Integer = 7
    Private Const GVC_Edit As Integer = 8
    Private Const GVC_Delete As Integer = 9

#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                GenCall()
            End If
        Catch ex As Exception

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
                Me.ViewState(VS_DrugCode) = Me.Request.QueryString("Value").ToString
            End If

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtBlankDrugAnalytesMatrix) = ds.Tables("DrugAnalytesMatrix")   ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If

            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall")

        Finally

        End Try

    End Function
#End Region

#Region "GenCall_Data "
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
                wStr = "vDrugAnalytesCode=" + Val.ToString
            End If

            If Not objHelp.GetDrugAnalytesMatrix(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
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
            Me.ShowErrorMessage(ex.Message, "....GenCall_Data")

        Finally

        End Try
    End Function
#End Region

#Region "GenCall_showUI "
    Private Function GenCall_ShowUI() As Boolean
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Try


            Page.Title = " :: Drug Analytes Detail ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Drug Analytes Detail"

            Choice = Me.ViewState(VS_Choice)

            If Not FillDropDown() Then
                Exit Function
            End If

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_ShowUI")
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
        Try


            objHelp.getdrugmst("cActiveFlag<>'N' And cStatusIndi<>'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_Drug, estr)

            dv_DrugName = ds_Drug.Tables(0).DefaultView
            dv_DrugName.Sort = "vDrugName"
            Me.SlcDrugEdit.DataSource = dv_DrugName
            Me.SlcDrugEdit.DataValueField = "vDrugCode"
            Me.SlcDrugEdit.DataTextField = "vDrugName"
            Me.SlcDrugEdit.DataBind()
            Me.SlcDrugEdit.Items.Insert(0, New ListItem("select Drug", ""))

            Me.SlcDrug.DataSource = dv_DrugName
            Me.SlcDrug.DataValueField = "vDrugCode"
            Me.SlcDrug.DataTextField = "vDrugName"
            Me.SlcDrug.DataBind()
            Me.SlcDrug.Items.Insert(0, New ListItem("select Drug", ""))

            Me.SlcDrug.Enabled = True

            'Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillDropDown")
        Finally
        End Try
    End Function

#End Region

#Region "FillGrid"

    Private Function FillGrid() As Boolean
        Dim ds_Detail As New Data.DataSet
        Dim estr As String = String.Empty
        Try


            If Not objHelp.GetViewDrugAnalytesMatrix("cStatusIndi <> '" + Status_Delete + "' and vDrugCode='" & Me.SlcDrugEdit.SelectedItem.Value.Trim() & "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Detail, estr) Then
                Return False
            End If

            Me.GV_Detail.DataSource = ds_Detail
            Me.GV_Detail.DataBind()
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillGrid")
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

            If Me.SlcDrug.Enabled = True Then
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
            End If

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                AssignValues("Edit")

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                Me.GenCall_Data(ds)
                Me.ViewState(VS_DtDrugAnalytesMatrix) = ds.Tables("DrugAnalytesMatrix")   ' adding blank DataTable in viewstate
                AssignValues("Add")

            End If

            ds_Save = New DataSet
            dt_Save = CType(Me.ViewState(VS_DtDrugAnalytesMatrix), DataTable)
            dt_Save.TableName = "DrugAnalytesMatrix"
            ds_Save.Tables.Add(dt_Save)

            If Not objLambda.Save_DrugAnalytesMatrix(Me.ViewState(VS_Choice), ds_Save, Me.Session(S_UserID), estr) Then

                ObjCommon.ShowAlert("Error While Saving DrugAnalytesMatrix", Me.Page)
                Exit Sub

            End If
            message = IIf(Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, " Drug Analytes Details Saved SuccessFully !", " Drug Analytes Details Updated Successfully !")
            ObjCommon.ShowAlert(message, Me.Page)
            ResetPage()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....BtnSave_Click")
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.ResetPage()
    End Sub

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click

        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click

        Me.TabContainer1.ActiveTabIndex = Me.TabContainer1.ActiveTabIndex + 1
        Me.SlcDrug.SelectedItem.Value = Me.SlcDrugEdit.SelectedItem.Value.Trim()
        Me.SlcDrug.Enabled = True
       
    End Sub

    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGo.Click

        If Me.SlcDrugEdit.SelectedIndex = 0 Then
            Me.ObjCommon.ShowAlert("Please Select Drug", Me)
        End If

        If Not FillGrid() Then
            Me.ObjCommon.ShowAlert("Error while filling grid", Me)
        End If

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

                dt_User = CType(Me.ViewState(VS_DtBlankDrugAnalytesMatrix), DataTable)
                dt_User.Clear()
                dr = dt_User.NewRow()
                'vDrugRegionCode,vDrugCode,vRegionCode,vRLDDetails,vManufacturer,cActiveFlag,iModifyBy,dModifyOn,cStatusIndi
                dr("vDrugAnalytesCode") = "0000000000"
                dr("vDrugCode") = Me.SlcDrug.SelectedItem.Value.Trim()
                dr("vAnalytes") = Me.Txtanalytes.Value.Trim()
                dr("vLinearityRange") = Me.TxtLinearrange.Value.Trim()
                dr("vAnalysisMethod") = Me.txtAnalysismethod.Value.Trim()
                dr("vComments") = Me.TxtComments.Value.Trim()
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dt_User.Rows.Add(dr)

            ElseIf type.ToUpper = "EDIT" Then

                dt_User = CType(Me.ViewState(VS_DtDrugAnalytesMatrix), DataTable)
                For Each dr In dt_User.Rows

                    dr("vAnalytes") = Me.Txtanalytes.Value.Trim()
                    dr("vLinearityRange") = Me.TxtLinearrange.Value.Trim()
                    dr("vAnalysisMethod") = Me.txtAnalysismethod.Value.Trim()
                    dr("vComments") = Me.TxtComments.Value.Trim()
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "E"
                    dr.AcceptChanges()
                Next dr

                'ElseIf type.ToUpper = "DELETE" Then

                '    dt_User = CType(Me.ViewState(VS_DtDrugAnalytesMatrix), DataTable)
                '    For Each dr In dt_User.Rows
                '        dr("iModifyBy") = Me.Session(S_UserID)
                '        dr("cStatusIndi") = "D"
                '        dr.AcceptChanges()
                '    Next dr

                '    dt_User.TableName = "DrugAnalytesMatrix"
                '    ds_Save.Tables.Add(dt_User.Copy())

                '    If Not objLambda.Save_DrugAnalytesMatrix(Me.ViewState(VS_Choice), ds_Save, Me.Session(S_UserID), estr) Then

                '        ObjCommon.ShowAlert("Error While Deleteing from DrugAnalytesMatrix ", Me.Page)
                '        Exit Sub

                '    End If

                '    ObjCommon.ShowAlert("Record Deleted SuccessFully", Me.Page)
                '    FillGrid()

            End If

            dt_User.AcceptChanges()
            Me.ViewState(VS_DtDrugAnalytesMatrix) = dt_User

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....AssignValues")
        End Try
    End Sub

#End Region

#Region "Grid Events"

   

    Protected Sub GV_Detail_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV_Detail.RowCreated

        e.Row.Cells(GVC_DrugRegionCode).Visible = False
        e.Row.Cells(GVC_DrugCode).Visible = False
        e.Row.Cells(GVC_analytes).Visible = True

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
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim ds_Save As New DataSet
        Dim dr As DataRow

        wstr = "vDrugAnalytesCode='" & Me.GV_Detail.Rows(index).Cells(GVC_DrugRegionCode).Text & "' And vDrugCode='" & _
                Me.GV_Detail.Rows(index).Cells(GVC_DrugCode).Text & "'"

        If Not objHelp.GetDrugAnalytesMatrix(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr) Then
            Me.ObjCommon.ShowAlert(estr, Me.Page())
            Exit Sub
        End If

        Me.ViewState(VS_DtDrugAnalytesMatrix) = ds.Tables(0)
        dt = Me.ViewState(VS_DtDrugAnalytesMatrix)

        If e.CommandName.ToUpper = "EDIT" Then

            dr = dt.Rows(0)
            'Me.SlcDrug.SelectedItem.Value = dr("vDrugCode")
            Me.Txtanalytes.Value = dr("vAnalytes")
            Me.TxtLinearrange.Value = dr("vLinearityRange")
            Me.txtAnalysismethod.Value = dr("vAnalysisMethod")
            Me.TxtComments.Value = dr("vComments")
            Me.SlcDrug.Enabled = False
            Me.ViewState(VS_DtDrugAnalytesMatrix) = dt

            Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit

            Me.TabContainer1.ActiveTabIndex = Me.TabContainer1.ActiveTabIndex + 1
            Me.BtnSave.Text = "Update"
            Me.BtnSave.ToolTip = "Update"

        ElseIf e.CommandName.ToUpper = "DELETE" Then


            Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit


            For Each dr In dt.Rows
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "D"
                dr.AcceptChanges()
            Next dr

            dt.TableName = "DrugAnalytesMatrix"
            ds_Save.Tables.Add(dt.Copy())

            If Not objLambda.Save_DrugAnalytesMatrix(Me.ViewState(VS_Choice), ds_Save, Me.Session(S_UserID), estr) Then

                ObjCommon.ShowAlert("Error While Deleteing from DrugAnalytesMatrix ", Me.Page)
                Exit Sub

            End If

            ObjCommon.ShowAlert("Record Deleted SuccessFully", Me.Page)
            FillGrid()

        End If

    End Sub

    Protected Sub GV_Detail_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GV_Detail.RowDeleting

    End Sub

    Protected Sub GV_Detail_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GV_Detail.RowEditing
        e.Cancel = True
    End Sub

#End Region

#Region "Selected IndexChanged"

    Protected Sub SlcDrugEdit_SelectedIndexChanged1(ByVal sender As Object, ByVal e As System.EventArgs) Handles SlcDrugEdit.SelectedIndexChanged
        Me.SlcDrug.SelectedIndex = Me.SlcDrugEdit.SelectedIndex
        If Not FillGrid() Then
            Me.ObjCommon.ShowAlert("Error while filling grid", Me)
        End If
        Me.GV_Detail.Visible = True
    End Sub

#End Region

#Region "Reset Page"

    Private Sub ResetPage()
        Me.SlcDrug.SelectedIndex = -1
        Me.Txtanalytes.Value = ""
        Me.TxtLinearrange.Value = ""
        Me.txtAnalysismethod.Value = ""
        Me.TxtComments.Value = ""
        Me.ViewState(VS_DtDrugAnalytesMatrix) = Nothing
        Me.SlcDrug.Enabled = True
        Me.BtnSave.Text = "Save"
        Me.BtnSave.ToolTip = "Save"
        Me.GV_Detail.Visible = False
        Me.SlcDrugEdit.SelectedIndex = 0

        If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
            Me.GenCall()
        ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
            FillGrid()
            Me.TabContainer1.ActiveTabIndex = Me.TabContainer1.ActiveTabIndex - 1
        End If

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
