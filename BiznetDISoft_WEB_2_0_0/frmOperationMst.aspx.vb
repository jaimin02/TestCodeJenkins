
Partial Class frmOperationMst
    Inherits System.Web.UI.Page

#Region "Variable Declaration "

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtOpMst As String = "DtOpMst"
    Private Const VS_OpCode As String = "OperationCode"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_Code As Integer = 1
    Private Const GVC_Name As Integer = 2
    Private Const GVC_Path As Integer = 3
    Private Const GVC_Parent As Integer = 4
    Private Const GVC_SeqNo As Integer = 5
    Private Const GVC_Edit As Integer = 6

#End Region

#Region "Load Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.rbtnlstApplicationType.Items(0).Selected = True
            GenCall()
        End If
        Try
            If GV_Operation.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIGV_Operation", "UIGV_Operation(); ", True)
            End If
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "GenCall "

    Private Function GenCall(Optional ByVal mode As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add) As Boolean
        Dim ds As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try


            Choice = Me.Request.QueryString("Mode")
            Me.ViewState(VS_Choice) = Choice   'To use it while saving

            If mode <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_Choice) = mode
            End If

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_OpCode) = Me.Request.QueryString("Value").ToString
            End If

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtOpMst) = ds.Tables("operationmst")   ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If
            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideSponsorDetails", "HideSponsorDetails(); ", True)
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

            Val = Me.ViewState(VS_OpCode) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vOperationCode=" + Val.ToString
            End If
            wStr += " And cStatusIndi <> 'D'"

            If Not objHelp.getOperationMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
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
            Me.ShowErrorMessage(ex.Message, "..GenCall_Data")

        Finally

        End Try
    End Function

#End Region

#Region "GenCall_showUI "

    Private Function GenCall_ShowUI() As Boolean
        Dim dt_OpMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try
            Page.Title = ":: Operation Master :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Operation Master"

            dt_OpMst = Me.ViewState(VS_DtOpMst)

            Choice = Me.ViewState("Choice")

            If Not FillDropDown() Then
                Exit Function
            End If

            If Not FillGrid() Then
                Exit Function
            End If

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                Me.txtOpName.Text = dt_OpMst.Rows(0).Item("vOperationName")
                Me.TxtOpPath.Text = dt_OpMst.Rows(0).Item("vOperationPath")
                If Not dt_OpMst.Rows(0).Item("vParentOperationCode") = "-999" Then
                    Me.DDLParentOp.SelectedValue = dt_OpMst.Rows(0).Item("vParentOperationCode")
                End If
                Me.TxtSeq.Value = dt_OpMst.Rows(0).Item("iSeqNo")
                Me.BtnSave.Text = "Update"
                Me.BtnSave.ToolTip = "Update"

            End If

            'Me.BtnSave.Attributes.Add("OnClick", "return Validation();")
            'Me.TxtSeq.Attributes.Add("onblur", "return Numeric();")

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_ShowUI")
        Finally
        End Try
    End Function

#End Region

#Region "FillDropDown"

    Private Function FillDropDown() As Boolean
        Dim ds_PerentOp As New Data.DataSet
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Try



            wstr = "cStatusIndi <> 'D'"
            If Me.rbtnlstApplicationType.SelectedValue = "0" Then
                wstr += " And cOperationType = '" & GeneralModule.OpType_BizNetWeb & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "1" Then
                wstr += " And cOperationType = '" & GeneralModule.OpType_BizNetDesktop & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "2" Then
                wstr += " And cOperationType = '" & GeneralModule.OpType_BizNetLIMS & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "3" Then
                wstr += " And cOperationType = '" & GeneralModule.OpType_Biolyte & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "4" Then
                wstr += " And cOperationType = '" & GeneralModule.OpType_PharmacyManagement & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "5" Then
                wstr += " And cOperationType = '" & GeneralModule.OpType_MedicalImaging & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "6" Then
                wstr += " And cOperationType = '" & GeneralModule.OpType_SDTM & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "7" Then
                wstr += " And cOperationType = '" & GeneralModule.OpType_OIMS & "'"
            End If

            If Not objHelp.getOperationMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_PerentOp, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From OperationMst", estr)
                Return False
            End If

            If ds_PerentOp.Tables(0).Rows.Count < 1 Then
                ObjCommon.ShowAlert("No Record Found For DDLParentOp", Me.Page)
                Return True
            End If

            Me.DDLParentOp.DataSource = ds_PerentOp
            Me.DDLParentOp.DataValueField = "vOperationCode"
            Me.DDLParentOp.DataTextField = "vOperationName"
            Me.DDLParentOp.DataBind()
            Me.DDLParentOp.Items.Insert(0, New ListItem("Select Parent", 0))
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillDropDown")
            Return False
        End Try
    End Function

#End Region

#Region "FillGrid"

    Private Function FillGrid() As Boolean
        Dim ds_View As New Data.DataSet
        Dim estr As String = String.Empty
        Dim wStr As String = String.Empty
        Try



            wStr = "cStatusIndi <> 'D'"
            If Me.rbtnlstApplicationType.SelectedValue = "0" Then
                wStr += " And cOperationType = '" & GeneralModule.OpType_BizNetWeb & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "1" Then
                wStr += " And cOperationType = '" & GeneralModule.OpType_BizNetDesktop & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "2" Then
                wStr += " And cOperationType = '" & GeneralModule.OpType_BizNetLIMS & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "3" Then
                wStr += " And cOperationType = '" & GeneralModule.OpType_Biolyte & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "4" Then
                wStr += " And cOperationType = '" & GeneralModule.OpType_PharmacyManagement & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "5" Then
                wStr += " And cOperationType = '" & GeneralModule.OpType_MedicalImaging & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "6" Then
                wStr += " And cOperationType = '" & GeneralModule.OpType_SDTM & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "7" Then
                wStr += " And cOperationType = '" & GeneralModule.OpType_OIMS & "'"
            End If

            If Not objHelp.getOperationMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_View, estr) Then
                Return False
            End If
            ds_View.Tables(0).DefaultView.Sort = "vOperationName"
            Me.GV_Operation.DataSource = ds_View.Tables(0).DefaultView.ToTable
            Me.GV_Operation.DataBind()
            If GV_Operation.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIGV_Operation", "UIGV_Operation(); ", True)
            End If
            'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "CreateGridHeader", "Validation();", True)
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "CreateGridHeader", "CreateGridHeader();", True)

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillGrid")
            Return False
        End Try
    End Function

#End Region

#Region "Button Click"

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim ds_Save As New DataSet
        Dim dt_Save As New DataTable
        Dim estr As String = String.Empty
        Dim message As String = String.Empty
        Try


            If Not AssignValues() Then
                Exit Sub
            End If

            ds_Save = New DataSet
            dt_Save = CType(Me.ViewState(VS_DtOpMst), DataTable)
            dt_Save.TableName = "operationmst"
            ds_Save.Tables.Add(dt_Save)

            If Not objLambda.Save_InsertOperationMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_OperationMst, ds_Save, Me.Session(S_UserID), estr) Then
                ObjCommon.ShowAlert("Error While Saving OperationMst", Me.Page)
                Exit Sub

            End If

            message = IIf(Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "Record Saved Successfully", "Record  Updated Successfully")


            ObjCommon.ShowAlert(message, Me.Page)
            ResetPage()
            FillGrid()




        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...BtnSave_Click")
        End Try
    End Sub

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        ResetPage()
        Me.Response.Redirect("frmOperationMst.aspx?mode=1")
    End Sub

#End Region

#Region "Assign values"

    Private Function AssignValues() As Boolean
        Dim dr As DataRow
        Dim dt_User As New DataTable
        Dim ds_Check As New DataSet
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim choice As String = String.Empty
        Try



            dt_User = CType(Me.ViewState(VS_DtOpMst), DataTable)
            choice = Me.ViewState(VS_Choice)
            'For validating Duplication
            wstr = "cStatusIndi <> 'D' And vOperationName='" & Me.txtOpName.Text.Trim() & "'"
            If Me.rbtnlstApplicationType.SelectedValue = "0" Then
                wstr += " And cOperationType = '" & GeneralModule.OpType_BizNetWeb & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "1" Then
                wstr += " And cOperationType = '" & GeneralModule.OpType_BizNetDesktop & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "2" Then
                wstr += " And cOperationType = '" & GeneralModule.OpType_BizNetLIMS & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "3" Then
                wstr += " And cOperationType = '" & GeneralModule.OpType_Biolyte & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "4" Then
                wstr += " And cOperationType = '" & GeneralModule.OpType_PharmacyManagement & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "5" Then
                wstr += " And cOperationType = '" & GeneralModule.OpType_MedicalImaging & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "6" Then
                wstr += " And cOperationType = '" & GeneralModule.OpType_SDTM & "'"
            ElseIf Me.rbtnlstApplicationType.SelectedValue = "7" Then
                wstr += " And cOperationType = '" & GeneralModule.OpType_OIMS & "'"
            End If


            If choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                wstr += " And vOperationCode <> '" + Me.ViewState(VS_OpCode) + "'"
            End If

            If Not objHelp.getOperationMst(wstr, _
                                WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Check, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From OperaionMst", estr)
                If GV_Operation.Rows.Count > 0 Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIGV_Operation", "UIGV_Operation(); ", True)
                End If

                Exit Function
                Return False
            End If

            'If (Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add And ds_Check.Tables(0).Rows.Count > 0) Or _
            '    (Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit And ds_Check.Tables(0).Rows.Count > 0) Then
            If ds_Check.Tables(0).Rows.Count > 0 Then
                ObjCommon.ShowAlert("Operation Name Already Exists", Me.Page)
                Return False

            End If
            '***********************************

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                dt_User.Clear()
                dr = dt_User.NewRow()
                'vOperationName,vOperationPath,vParentOperationCode,iSeqNo,iModifyBy
                dr("vOperationCode") = "0"
                dr("vOperationName") = Me.txtOpName.Text.Trim()
                dr("vOperationPath") = Me.TxtOpPath.Text.Trim()
                dr("vParentOperationCode") = IIf(Me.DDLParentOp.SelectedIndex = 0, "-999", Me.DDLParentOp.SelectedValue.Trim())
                dr("iSeqNo") = Me.TxtSeq.Value.Trim()

                If Me.rbtnlstApplicationType.SelectedValue = "0" Then
                    dr("cOperationType") = GeneralModule.OpType_BizNetWeb
                ElseIf Me.rbtnlstApplicationType.SelectedValue = "1" Then
                    dr("cOperationType") = GeneralModule.OpType_BizNetDesktop
                ElseIf Me.rbtnlstApplicationType.SelectedValue = "2" Then
                    dr("cOperationType") = GeneralModule.OpType_BizNetLIMS
                ElseIf Me.rbtnlstApplicationType.SelectedValue = "3" Then
                    dr("cOperationType") = GeneralModule.OpType_Biolyte
                ElseIf Me.rbtnlstApplicationType.SelectedValue = "4" Then
                    dr("cOperationType") = GeneralModule.OpType_PharmacyManagement
                ElseIf Me.rbtnlstApplicationType.SelectedValue = "5" Then
                    dr("cOperationType") = GeneralModule.OpType_MedicalImaging
                ElseIf Me.rbtnlstApplicationType.SelectedValue = "6" Then
                    dr("cOperationType") = GeneralModule.OpType_SDTM
                ElseIf Me.rbtnlstApplicationType.SelectedValue = "7" Then
                    dr("cOperationType") = GeneralModule.OpType_OIMS
                End If

                dr("iModifyBy") = Me.Session(S_UserID)
                dt_User.Rows.Add(dr)

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                For Each dr In dt_User.Rows

                    dr("vOperationName") = Me.txtOpName.Text.Trim()
                    dr("vOperationPath") = Me.TxtOpPath.Text.Trim()
                    'dr("vParentOperationCode") = IIf(Me.DDLParentOp.SelectedIndex = 0, System.DBNull.Value, Me.DDLParentOp.SelectedValue.Trim())
                    dr("vParentOperationCode") = IIf(Me.DDLParentOp.SelectedIndex = 0, "-999", Me.DDLParentOp.SelectedValue.Trim())
                    dr("iSeqNo") = Me.TxtSeq.Value.Trim()

                    If Me.rbtnlstApplicationType.SelectedValue = "0" Then
                        dr("cOperationType") = GeneralModule.OpType_BizNetWeb
                    ElseIf Me.rbtnlstApplicationType.SelectedValue = "1" Then
                        dr("cOperationType") = GeneralModule.OpType_BizNetDesktop
                    ElseIf Me.rbtnlstApplicationType.SelectedValue = "2" Then
                        dr("cOperationType") = GeneralModule.OpType_BizNetLIMS
                    ElseIf Me.rbtnlstApplicationType.SelectedValue = "3" Then
                        dr("cOperationType") = GeneralModule.OpType_Biolyte
                    ElseIf Me.rbtnlstApplicationType.SelectedValue = "4" Then
                        dr("cOperationType") = GeneralModule.OpType_PharmacyManagement
                    ElseIf Me.rbtnlstApplicationType.SelectedValue = "5" Then
                        dr("cOperationType") = GeneralModule.OpType_MedicalImaging
                    ElseIf Me.rbtnlstApplicationType.SelectedValue = "6" Then
                        dr("cOperationType") = GeneralModule.OpType_SDTM
                    ElseIf Me.rbtnlstApplicationType.SelectedValue = "7" Then
                        dr("cOperationType") = GeneralModule.OpType_OIMS
                    End If

                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "E"
                    dr.AcceptChanges()

                Next dr
                dt_User.AcceptChanges()

            End If

            Me.ViewState(VS_DtOpMst) = dt_User
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...AssignValues")
            Return False
        End Try
    End Function

#End Region

#Region "Reset Page"

    Private Sub ResetPage()
        Me.DDLParentOp.SelectedIndex = 0
        Me.txtOpName.Text = String.Empty
        Me.TxtOpPath.Text = String.Empty
        Me.TxtSeq.Value = String.Empty
        Me.GV_Operation.PageIndex = 0
        Me.ViewState(VS_DtOpMst) = Nothing
        Me.BtnSave.Text = "Save"
        Me.BtnSave.ToolTip = "Save"
        'Me.Response.Redirect("frmOperationMst.aspx?mode=1&Save=Y")
        'Me.Response.Redirect("frmOperationMst.aspx?mode=1")
    End Sub

#End Region

#Region "Grid Event"

    Protected Sub GV_Operation_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or _
          e.Row.RowType = DataControlRowType.Header Or _
          e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(GVC_Code).Visible = False
        End If


    End Sub

    Protected Sub GV_Operation_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim index As Integer = e.CommandArgument

        If e.CommandName.ToUpper = "EDIT" Then
            Me.ViewState(VS_OpCode) = Me.GV_Operation.Rows(index).Cells(GVC_Code).Text.Trim()
            GenCall(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit)
        End If



        'If e.CommandName.ToUpper = "EDIT" Then
        '    Me.Response.Redirect("frmOperationMst.aspx?mode=2&value=" & Me.GV_Operation.Rows(index).Cells(GVC_Code).Text.Trim())
        'End If

    End Sub

    Protected Sub GV_Operation_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        'If e.Row.RowType = DataControlRowType.Pager Then
        '    e.Row.Cells(GVC_Code).Visible = False
        'End If
        If e.Row.RowType = DataControlRowType.DataRow Then

            e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (GV_Operation.PageSize * GV_Operation.PageIndex) + 1
            CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandName = "Edit"

        End If

    End Sub

    Protected Sub GV_Operation_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        GV_Operation.PageIndex = e.NewPageIndex
        FillGrid()
    End Sub

    Protected Sub GV_Operation_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs)
        e.Cancel = True
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

    Protected Sub rbtnlstApplicationType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnlstApplicationType.SelectedIndexChanged
        Me.GenCall(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add)
    End Sub

End Class

