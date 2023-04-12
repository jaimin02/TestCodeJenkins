
Partial Class frmSpeciallityMst
    Inherits System.Web.UI.Page
    Dim objcommon As New clsCommon
    Dim objhelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Dim objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()

    Dim eStr As String = ""
    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtSpecMaster As String = "DtSpecMaster"
    Private Const VS_DtBlankSpecMaster As String = "DtBlankSpecMaster "
    Private Const VS_SpecNo As String = "SpecNo"
    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_SpecNo As Integer = 1
    Private Const GVC_SpecDesc As Integer = 2
    Private Const GVC_SpecDtl As Integer = 3
    Private Const GVC_Edit As Integer = 4
    Private Const GVC_Delete As Integer = 5
    Private Const VS_WhereCondition As String = "wStr_CountryMst"
#Region " Load Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

        
            If Not IsPostBack Then

                GenCall()

            End If


        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr)
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
                Me.ViewState(VS_SpecNo) = Me.Request.QueryString("Value").ToString
            End If
            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If
            Me.ViewState(VS_DtSpecMaster) = ds.Tables("specialityMst")   ' adding blank DataTable in viewstate

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

        Try

            Val = Me.ViewState(VS_SpecNo) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "nSpecialityNo=" + Val.ToString
            End If


            If Not objhelp.GetSpecialityMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                Me.ShowErrorMessage(eStr_Retu, "")
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
        Dim dt_OpMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Try

            Page.Title = ":: Speciallity Master :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            Fillgrid()
            'FillDropDownList()

            'Me.BtnSave.OnClientClick = "return Validation();"
            'Me.btnupdate.OnClientClick = "return Validation();"
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Speciality Master"

            dt_OpMst = Me.ViewState(VS_DtSpecMaster)

            Choice = Me.ViewState(VS_Choice)

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
            End If

            GenCall_ShowUI = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function
#End Region
#Region "Fill Grid"
    Private Function Fillgrid() As Boolean
        Dim eStr_Retu As String = ""
        Dim Ds_Grid As New DataSet
        Dim wStr_SpecNo As String = ""

        Try
            If Not Me.ViewState(VS_WhereCondition) Is Nothing Then
                wStr_SpecNo = Convert.ToString(Me.ViewState(VS_WhereCondition))
            Else
                wStr_SpecNo = "cStatusIndi<>'D' order by nSpecialityNo"
            End If

            If objhelp.GetFieldsOfTable("SpecialityMst", "*", wStr_SpecNo, Ds_Grid, eStr_Retu) Then
                gvwSpecialityMst.DataSource = Ds_Grid
                gvwSpecialityMst.DataBind()
            End If

            Return True
        Catch ex As Exception
            Return False
            ShowErrorMessage(ex.Message, eStr)
        End Try
    End Function
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

#Region " Gridview Events"
    Protected Sub gvwSpecialityMst_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        gvwSpecialityMst.PageIndex = e.NewPageIndex
        Fillgrid()
    End Sub

    Protected Sub gvwSpecialityMst_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Dim DrugName As String = ""
        Dim eStr As String = ""
        Try

            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.FindControl("ImbEdit"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImbEdit"), ImageButton).CommandName = "MYEDIT"
                CType(e.Row.FindControl("ImbDelete"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImbDelete"), ImageButton).CommandName = "MYDELETE"
                e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (gvwSpecialityMst.PageSize * gvwSpecialityMst.PageIndex) + 1
                e.Row.Cells(GVC_SpecNo).Visible = False
                'e.Row.Cells(GVC_Delete).Visible = False
            End If

            If e.Row.RowType = DataControlRowType.Header Then

                e.Row.Cells(GVC_SpecNo).Visible = False
                'e.Row.Cells(GVC_Delete).Visible = False

            End If

            If e.Row.RowType = DataControlRowType.Footer Then

                e.Row.Cells(GVC_SpecNo).Visible = False
                'e.Row.Cells(GVC_Delete).Visible = False

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)

        End Try
    End Sub

    Protected Sub gvwSpecialityMst_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim index As Integer = e.CommandArgument
        Dim ds As New DataSet
        Dim eStr As String = ""
        Dim Wstr As String = ""
        Dim dt As New DataTable
        Dim Formulation As String = ""
        Dim UOM As String = ""
        Dim item As New ListItem
        Dim itm As New ListItem
        Try
            If e.CommandName.ToUpper() = "MYEDIT" Or e.CommandName.ToUpper() = "MYDELETE" Then
                Wstr = "nspecialityNo='" & Me.gvwSpecialityMst.Rows(index).Cells(GVC_SpecNo).Text & "'"
                If Not objhelp.GetSpecialityMaster(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr) Then
                    Response.Write(eStr)
                    Exit Sub
                End If
                Me.ViewState(VS_DtSpecMaster) = ds.Tables(0)
                dt = Me.ViewState(VS_DtSpecMaster)

            End If
            If e.CommandName.ToUpper = "MYEDIT" Then
                Dim dr As DataRow = dt.Rows(0)

                Me.txtSpecDesc.Text = dr("vSpecialityDesc").ToString
                Me.txtSpecDtl.Text = dr("vSpecialityDetails").ToString

                gvwSpecialityMst.Rows(e.CommandArgument).BackColor = Drawing.ColorTranslator.FromHtml("#ABABAB")

                Me.ViewState(VS_DtSpecMaster) = dt
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit

                Me.BtnSave.Visible = False
                Me.BtnExit.Visible = False
                Me.btnupdate.Visible = True
                Me.btncancel.Visible = True

            ElseIf e.CommandName.ToUpper = "MYDELETE" Then
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                AssignValues("Delete")
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr)
        End Try
        
    End Sub
#End Region

#Region " Button Events "


    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)


        Dim ds As New DataSet
        Dim ds_save As New DataSet
        Dim dt_save As New DataSet
        Dim eStr_Retu As String = ""
        Dim ds_CountryCheck As New DataSet
        Dim dt_CountryMSt As New DataTable
        Dim str_nCountryNo As String = ""

        'Dim wStr_CountryCode As String = "vCountryCode= '" + Me.txtCountryCode.Text.ToUpper() + "'" + " AND cStatusIndi <> 'C'"
        Try
            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                AssignValues("Edit")

                dt_CountryMSt = CType(Me.ViewState(VS_DtSpecMaster), DataTable)
                str_nCountryNo = Convert.ToString(dt_CountryMSt.Rows(0)("nSpecialityNo"))
                'If Not Me.ValidateReportTypeExists(str_nCountryNo) Then
                '    objcommon.ShowAlert("Country Code Already Exists !!!", Me)
                '    Exit Sub
                'End If

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.GenCall_Data(ds)
                Me.ViewState(VS_DtSpecMaster) = ds.Tables("SpecialityMst")   ' adding blank DataTable in viewstate
                'If Not Me.ValidateReportTypeExists() Then
                '    objcommon.ShowAlert("Country Code Already Exists !!!", Me)
                '    Exit Sub
                'End If
                AssignValues("Add")

            End If
            ds_save = New DataSet
            ds_save.Tables.Add(CType(Me.ViewState(VS_DtSpecMaster), Data.DataTable).Copy())
            ds_save.Tables(0).TableName = "SpecialityMst"

            If Not objLambda.Save_SpecialityMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_SpecialityMst, ds_save, Me.Session(S_UserID), eStr_Retu) Then
                objcommon.ShowAlert("Error While Saving Speciality Mst", Me.Page)
                Exit Sub
            Else
                objcommon.ShowAlert("Speciality Saved SuccessFully", Me.Page)
                Resetpage()
                Fillgrid()
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr)
        End Try
    End Sub

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Resetpage()
        Me.Response.Redirect("frmMainpage.aspx")
    End Sub

    Protected Sub btnupdate_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim Ds_Editsave As New DataSet
        Dim eStr As String = ""
        Dim dt_SpecMSt As New DataTable
        Dim str_nspecNo As String = ""
        Dim eStr_Retu As String = ""

        Try
            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                dt_SpecMSt = CType(Me.ViewState(VS_DtSpecMaster), DataTable)
                str_nspecNo = Convert.ToString(dt_SpecMSt.Rows(0)("nSpecialityNo"))
                'If Not Me.ValidateReportTypeExists(str_nCountryNo) Then
                '    objcommon.ShowAlert("Country Code Already Exists !!!", Me)
                '    Exit Sub
                'End If
                AssignValues("Edit")
            End If
            Ds_Editsave = New DataSet
            Ds_Editsave.Tables.Add(CType(Me.ViewState(VS_DtSpecMaster), Data.DataTable).Copy())
            Ds_Editsave.Tables(0).TableName = "SpecialityMst"

            If Not objLambda.Save_SpecialityMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_SpecialityMst, Ds_Editsave, Me.Session(S_UserID), eStr) Then
                objcommon.ShowAlert("Error While Saving in Speciality Master", Me.Page)
                Exit Sub
            Else
                objcommon.ShowAlert("Speciality Updated SuccessFully", Me.Page)
                Resetpage()
                Fillgrid()
                Me.BtnSave.Visible = True
                Me.BtnExit.Visible = True
                Me.btnupdate.Visible = False
                Me.btncancel.Visible = False
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr)

        End Try

    End Sub

    Protected Sub btncancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Resetpage()
        Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
        If Not Me.Fillgrid() Then
            objcommon.ShowAlert("Error Filling Grid ", Me)
        End If
        Me.BtnSave.Visible = True
        Me.BtnExit.Visible = True
        Me.btnupdate.Visible = False
        Me.btncancel.Visible = False

    End Sub

#End Region
#Region "Assign Values"
    Private Sub AssignValues(ByVal type As String)
        Dim dr As DataRow
        Dim dt_User As New DataTable
        Dim ds_Save As New DataSet
        Dim estr_Retu As String = ""
        Try
            If type.ToUpper = "ADD" Then
                dt_User = CType(Me.ViewState(VS_DtSpecMaster), DataTable)
                dt_User.Clear()
                dr = dt_User.NewRow()
                dr("nSpecialityNo") = "0"
                dr("vSpecialityDesc") = Me.txtSpecDesc.Text.Trim
                dr("vSpecialityDetails") = Me.txtSpecDtl.Text.Trim
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dt_User.Rows.Add(dr)
            ElseIf type.ToUpper = "EDIT" Then
                dt_User = CType(Me.ViewState(VS_DtSpecMaster), DataTable)
                For Each dr In dt_User.Rows
                    dr("vSpecialityDesc") = Me.txtSpecDesc.Text.Trim
                    dr("vSpecialityDetails") = Me.txtSpecDtl.Text.Trim
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "E"
                    dr.AcceptChanges()
                Next
            ElseIf type.ToUpper = "DELETE" Then
                dt_User = CType(Me.ViewState(VS_DtSpecMaster), DataTable)
                For Each dr In dt_User.Rows
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "D"
                    dr.AcceptChanges()
                Next
                dt_User.TableName = "SpecialityMst"
                ds_Save.Tables.Add(dt_User.Copy())
                If Not objLambda.Save_SpecialityMaster(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_SpecialityMst, ds_Save, Me.Session(S_UserID), estr_Retu) Then
                    objcommon.ShowAlert("Error While Deleteing from Speciality Master", Me.Page)
                    Exit Sub
                Else
                    Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
                    objcommon.ShowAlert("Speciality Deleted SuccessFully", Me.Page)
                    Fillgrid()
                End If
            End If
            Me.ViewState(VS_DtSpecMaster) = dt_User
        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr)
        End Try
    End Sub
#End Region
#Region "Reset Page"
    Private Sub Resetpage()

        Me.txtSpecDesc.Text = ""
        Me.txtSpecDtl.Text = ""

    End Sub
#End Region


End Class
