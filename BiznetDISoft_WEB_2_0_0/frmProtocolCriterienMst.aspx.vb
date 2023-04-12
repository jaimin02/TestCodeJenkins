
Partial Class frmProtocolCriterienMst
    Inherits System.Web.UI.Page

    Dim ObjCommon As New clsCommon
    Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private Const VS_Choice As String = "Choice"
    Private Const VS_SAVE As String = "Save"
    Private Const VS_DtProtocolCriterienMst As String = "DtProtocolCriterienMst"
    Private Const VS_ProtocolCriterienID As String = "ProtocolCriterienID"
#Region "Load Event"
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
                Me.ViewState(VS_ProtocolCriterienID) = Me.Request.QueryString("Value").ToString
            End If

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtProtocolCriterienMst) = ds.Tables("ProtocolCriterienMst")   ' adding blank DataTable in viewstate

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
    Private Function GenCall_Data(ByRef ds_Retu As DataSet) As Boolean

        Dim wStr As String = ""
        Dim eStr_Retu As String = ""
        Dim Val As String = ""
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing
        'Dim objFFR As New FFReporting.FFReporting

        Try

            Val = Me.ViewState(VS_ProtocolCriterienID) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vProtocolCriterienID=" + Val.ToString
            End If


            If Not objHelp.getProtocolCriterienMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
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

            ds_Retu = ds
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
            Page.Title = ":: Protocol Criteria Master  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "ProtocolCriterien Master"

            dt_OpMst = Me.ViewState(VS_DtProtocolCriterienMst)

            Choice = Me.ViewState("Choice")

            'If Me.ViewState(VS_SAVE) Is Nothing Then 'For Save
            '    Me.ViewState(VS_SAVE) = Me.Request.QueryString("Save")
            'End If
            'If Me.ViewState(VS_SAVE) = "Y" Then
            '    ObjCommon.ShowAlert("Record Saved SuccessFully", Me.Page)
            '    Me.ViewState(VS_SAVE) = "N"
            'End If '***********************************************

            If Not FillGrid() Then
                Exit Function
            End If

            If Not FillDropDown() Then
                Exit Function
            End If

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                'Me.txtUserTypeName.Text = dt_OpMst.Rows(0).Item("vUserTypeName")
                Me.txtCriterianDesc.Text = dt_OpMst.Rows(0).Item("vProtocolCriterienDescription")
                Me.chkActive.Checked = IIf(dt_OpMst.Rows(0).Item("cActiveFlag").ToString.ToUpper = "Y", True, False)
                Me.DDLCriterienType.SelectedValue = dt_OpMst.Rows(0).Item("cProtocolCriterienType")
            End If
            Me.BtnSave.Attributes.Add("OnClick", "return Validation();")

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function
#End Region

#Region "FillDropDown"
    Private Function FillDropDown() As Boolean
        Try
            Me.DDLCriterienType.Items.Add(New ListItem("Inclusion List", "I"))
            Me.DDLCriterienType.Items.Add(New ListItem("Exclusion List", "E"))
            Me.DDLCriterienType.Items.Add(New ListItem("Predose", "D"))
            Me.DDLCriterienType.Items.Add(New ListItem("Postdose", "P"))
            Me.DDLCriterienType.Items.Insert(0, New ListItem("Select Criterien type", ""))
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
#End Region

#Region "FillGrid"
    Private Function FillGrid() As Boolean
        Dim ds_View As New Data.DataSet
        Dim estr As String = ""
        Try


            'objHelp.getOperationMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_PerentOp, estr)
            If Not objHelp.getProtocolCriterienMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_View, estr) Then
                Return False
            End If

            Me.GV_Criterien.DataSource = ds_View
            Me.GV_Criterien.DataBind()

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region "Button Click"
    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim ds_Save As New DataSet
        Dim dt_Save As New DataTable
        Dim estr As String = ""
        Try
            AssignValues()
            ds_Save = New DataSet
            dt_Save = CType(Me.ViewState(VS_DtProtocolCriterienMst), DataTable)
            dt_Save.TableName = "ProtocolCriterienMst"
            ds_Save.Tables.Add(dt_Save)
            If Not objLambda.Save_InsertProtocolCriterienMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_ProtocolCriterienMst, ds_Save, Me.Session(S_UserID), estr) Then
                ObjCommon.ShowAlert("Error While Saving ProtocolCriterienMst", Me.Page)
                Exit Sub
            Else
                ObjCommon.ShowAlert("Record Saved SuccessFully", Me.Page)
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
#End Region

#Region "Assign values"
    Private Sub AssignValues()
        Dim dr As DataRow
        Dim dt_User As New DataTable
        dt_User = CType(Me.ViewState(VS_DtProtocolCriterienMst), DataTable)
        dt_User.Clear()
        dr = dt_User.NewRow()
        'vProtocolCriterienID,vProtocolCriterienDescription,cProtocolCriterienType,cActiveFlag,iModifyBy,dModifyon,cStatusIndi
        dr("vProtocolCriterienID") = "0"
        dr("vProtocolCriterienDescription") = Me.txtCriterianDesc.Text.Trim()
        dr("cProtocolCriterienType") = Me.DDLCriterienType.SelectedValue
        dr("cActiveFlag") = IIf(Me.chkActive.Checked, "Y", "N")
        dr("cStatusIndi") = "N"
        dr("iModifyBy") = Me.Session(S_UserID)
        dt_User.Rows.Add(dr)
        Me.ViewState(VS_DtProtocolCriterienMst) = dt_User
    End Sub
#End Region

#Region "Reset Page"
    Private Sub ResetPage()
        Me.txtCriterianDesc.Text = ""
        Me.DDLCriterienType.SelectedIndex = 0
        Me.chkActive.Checked = False
        Me.ViewState(VS_DtProtocolCriterienMst) = Nothing
        Me.GenCall()
        'Me.Response.Redirect("frmProtocolCriterienMst.aspx?mode=1")
        'Me.Response.Redirect("frmUserTypeMst.aspx?mode=1&Save=Y")
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
