
Partial Class frmExpenseTypeMst
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtExpenseTypeMst As String = "DtExpenseTypeMst"
    Private Const VS_ExpenseTypeId As String = "ExpenseTypeId"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_ExpenseTypeId As Integer = 1
    Private Const GVC_ExpenseType As Integer = 2
    Private Const GVC_Edit As Integer = 3
    Private Const GVC_dELETE As Integer = 4
    Dim gvw_exptype As String = String.Empty

#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            If Not GenCall() Then
                Exit Sub
            End If
        End If

    End Sub

#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean

        Dim eStr As String = ""
        Dim wStr As String = ""
        Dim ds_ExpenseTypeMst As New DataSet
        Dim dt_ExpenseTypeMst As New DataTable
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Not Me.Request.QueryString("mode") Is Nothing Then
                Choice = Convert.ToString(Me.Request.QueryString("mode")).Trim()
            End If

            Me.ViewState(VS_Choice) = Choice   'To use it while saving

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_ExpenseTypeId) = Me.Request.QueryString("Value").ToString
            End If

            ''''Check for Valid User''''''''''''''
            If Not GenCall_Data(Choice, dt_ExpenseTypeMst) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtExpenseTypeMst) = dt_ExpenseTypeMst ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI(Choice, dt_ExpenseTypeMst) Then 'For Displaying Data 
                Exit Function
            End If

            GenCall = True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function
#End Region

#Region "GenCall_Data"

    Private Function GenCall_Data(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                            ByRef dt_Dist_Retu As DataTable) As Boolean

        Dim eStr As String = ""
        Dim wStr As String = ""
        Dim ds_ExpenseTypeMst As DataSet = Nothing

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Choice_1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "nExpTypeId= '" + Me.ViewState(VS_ExpenseTypeId).ToString() + "'"
            End If

            wStr += " And cStatusIndi <> 'D'"

            If Not objHelp.getExpenseTypeMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ExpenseTypeMst, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_ExpenseTypeMst Is Nothing Then
                Throw New Exception(eStr)
            End If

            If ds_ExpenseTypeMst.Tables(0).Rows.Count <= 0 And _
               Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                Throw New Exception("No Records Found for Selected role")

            End If

            dt_Dist_Retu = ds_ExpenseTypeMst.Tables(0)
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_ShowUI"

    Private Function GenCall_ShowUI(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                                      ByVal dt_ExpenseTypeMst As DataTable) As Boolean

        CType(Master.FindControl("lblHeading"), Label).Text = "Expense Type Master"
        Page.Title = " :: Expense Type Master ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
        If Not FillGridExpenseTypeMst() Then
            Exit Function
        End If

        If Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

            Me.TxtExpenseType.Text = ConvertDbNullToDbTypeDefaultValue(dt_ExpenseTypeMst.Rows(0)("vExpType"), dt_ExpenseTypeMst.Rows(0)("vExpType").GetType)
            'Me.txtlocationname.Text = ConvertDbNullToDbTypeDefaultValue(dt_LocationMst.Rows(0)("vLocationName"), dt_LocationMst.Rows(0)("vLocationName").GetType)
            'Me.txtInitial.Text = ConvertDbNullToDbTypeDefaultValue(dt_LocationMst.Rows(0)("vLocationInitiate"), dt_LocationMst.Rows(0)("vLocationInitiate").GetType)
            'Me.txtcountrycode.Text = ConvertDbNullToDbTypeDefaultValue(dt_LocationMst.Rows(0)("vRemark"), dt_LocationMst.Rows(0)("vRemark").GetType)

        End If

        GenCall_ShowUI = True

    End Function

#End Region

#Region "FillGridExpenseTypeMst"

    Private Function FillGridExpenseTypeMst() As Boolean
        Dim wStr As String = ""
        Dim eStr As String = ""
        Dim ds_ExpenseTypeMst As New DataSet
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            wStr = "cStatusIndi <> 'D'"

            If Not objHelp.getExpenseTypeMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ExpenseTypeMst, eStr) Then
                Throw New Exception(eStr)
            End If

            Me.GVW_ShowExpense.DataSource = ds_ExpenseTypeMst
            Me.GVW_ShowExpense.DataBind()

            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
            Return False
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

#Region "Reset Page"

    Private Sub ResetPage()
        Me.TxtExpenseType.Text = ""
        Response.Redirect("frmExpenseTypeMst.aspx?mode=1")
    End Sub

#End Region

#Region "Save"

    Private Function AssignUpdatedValues() As Boolean
        Dim dtOld As New DataTable
        Dim dr As DataRow
        Dim ds_Check As New DataSet
        Dim eStr As String = ""
        Dim wStr As String = ""
        Try
            dtOld = Me.ViewState(VS_DtExpenseTypeMst)

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "vExpType = '" + Me.TxtExpenseType.Text.Trim() + "'"
            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                wStr = "vExpType = '" + Me.TxtExpenseType.Text.Trim() + "' And nExpTypeId='" + Me.ViewState(VS_ExpenseTypeId) + "'"
            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete Then
                wStr = "nExpTypeId='" + Me.ViewState(VS_ExpenseTypeId) + "'"
            End If
            wStr += " And cStatusIndi <> 'D'"

            If Not objHelp.getExpenseTypeMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Check, eStr) Then

                Me.ShowErrorMessage("Error While Getting Data From Expense Type Mst", eStr)
                Return False

            End If

           


            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                dtOld.Clear()
                dr = dtOld.NewRow
                dr("nExpTypeId") = 0
                dr("vExpType") = Me.TxtExpenseType.Text.Trim
                dr("cStatusIndi") = "N"
                dr("iModifyBy") = Me.Session(S_UserID)
                dtOld.Rows.Add(dr)
                dtOld.AcceptChanges()

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                For Each dr In dtOld.Rows

                    dr("vExpType") = Me.TxtExpenseType.Text.Trim
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "E"
                    dr.AcceptChanges()

                Next dr

                dtOld.AcceptChanges()

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete Then

                For Each dr In dtOld.Rows
                    dr("cStatusIndi") = "D"
                    dr("iModifyBy") = Me.Session(S_UserID)
                Next dr
                dtOld.AcceptChanges()

            End If

            Me.ViewState(VS_DtExpenseTypeMst) = dtOld
            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

    Protected Sub LnkBtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkBtnSave.Click
        Dim Dt As New DataTable
        Dim Ds_ExpenseTypeMst As DataSet
        Dim ds_LocGrid As New DataSet
        Dim eStr As String = ""

        Try

            If Not AssignUpdatedValues() Then
                Exit Sub
            End If

            Ds_ExpenseTypeMst = New DataSet
            Ds_ExpenseTypeMst.Tables.Add(CType(Me.ViewState(VS_DtExpenseTypeMst), Data.DataTable).Copy())
            Ds_ExpenseTypeMst.Tables(0).TableName = "ExpenseTypeMst"   ' New Values on the form to be updated

            'If Not objLambda.Save_ExpenseTypeMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MastersEntriesEnum_ExpenseTypeMaster, Ds_ExpenseTypeMst, Me.Session(S_UserID), eStr) Then

            '    ShowErrorMessage(eStr, "")
            '    Exit Sub

            'End If

            Me.objcommon.ShowAlert("Record Saved Successfully", Me)
            If Not FillGridExpenseTypeMst() Then
                ShowErrorMessage(eStr, "")
                Exit Sub
            End If
            ResetPage()

        Catch ThreaEx As Threading.ThreadAbortException

        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr)
        End Try
    End Sub

    Protected Sub LnkBtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LnkBtnCancel.Click
        TxtExpenseType.Text = String.Empty
    End Sub

#Region "Grid Events"
 
   
    Protected Sub GVW_ShowExpense_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GVW_ShowExpense.PageIndexChanging
        GVW_ShowExpense.PageIndex = e.NewPageIndex
        If Not FillGridExpenseTypeMst() Then
            Exit Sub
        End If
    End Sub

    Protected Sub GVW_ShowExpense_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GVW_ShowExpense.RowCommand
        Dim Index As Integer = e.CommandArgument
        Dim dS_ExpenseTypeMst = New DataSet
        Dim eStr As String = ""
        Dim wStr As String = String.Empty
        Try

        
            'Dim GV_Index As Integer = 0
            If e.CommandName.ToUpper = "EDIT" Then
                'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "EditExpenseType", "EdiToTxtBox", True)
                gvw_exptype = Me.GVW_ShowExpense.Rows(Index).Cells(GVC_ExpenseTypeId).Text.Trim()
                Me.Response.Redirect("frmExpenseTypeMst.aspx?mode=2&Value=" & gvw_exptype)
            ElseIf e.CommandName.ToUpper = "DELETE" Then


                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete

                gvw_exptype = Me.GVW_ShowExpense.Rows(Index).Cells(GVC_ExpenseTypeId).Text.Trim()
                Me.ViewState(VS_ExpenseTypeId) = gvw_exptype
                wStr = "nExpTypeId= '" + gvw_exptype + "'"
                wStr += " And cStatusIndi <> 'D'"

                If Not objHelp.getExpenseTypeMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dS_ExpenseTypeMst, eStr) Then
                    Exit Sub
                End If

                Me.ViewState(VS_DtExpenseTypeMst) = dS_ExpenseTypeMst.Tables(0)
                If Not AssignUpdatedValues() Then
                    Throw New Exception(eStr)
                    Exit Sub
                End If
                dS_ExpenseTypeMst = New DataSet
                dS_ExpenseTypeMst.Tables.Add(CType(Me.ViewState(VS_DtExpenseTypeMst), Data.DataTable).Copy())
                dS_ExpenseTypeMst.Tables(0).TableName = "ExpenseTypeMst"   ' New Values on the form to be updated

                'If Not objLambda.Save_ExpenseTypeMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MastersEntriesEnum_ExpenseTypeMaster, dS_ExpenseTypeMst, Me.Session(S_UserID), eStr) Then

                '    ShowErrorMessage(eStr, "")
                '    Exit Sub

                'End If

                Me.objcommon.ShowAlert("Record Deleted Successfully", Me)
                If Not FillGridExpenseTypeMst() Then
                    ShowErrorMessage(eStr, "")
                    Exit Sub
                End If
                ResetPage()
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub GVW_ShowExpense_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVW_ShowExpense.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or _
            e.Row.RowType = DataControlRowType.Header Or _
            e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(GVC_ExpenseTypeId).Visible = False
        End If
    End Sub

    Protected Sub GVW_ShowExpense_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVW_ShowExpense.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (GVW_ShowExpense.PageSize * GVW_ShowExpense.PageIndex) + 1
            CType(e.Row.FindControl("ImgEdit"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("ImgEdit"), ImageButton).CommandName = "Edit"
            CType(e.Row.FindControl("ImgDelete"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("ImgDelete"), ImageButton).CommandName = "Delete"

        End If
    End Sub

#End Region

    
    Protected Sub GVW_ShowExpense_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GVW_ShowExpense.RowDeleting

    End Sub
End Class
