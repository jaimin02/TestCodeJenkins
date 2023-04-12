
Partial Class frmClientContactMatrix
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION "

    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()

    Private Const VS_Choice As String = "Choice"
    Private Const VS_ClientCode As String = "vClientCode"
    Private Const VS_dtClientMst As String = "dtClientMst"
    Private Const VS_ClientContactCode As String = "vClientContactCode"
    Private Const VS_dtClientContact As String = "dtClientContact"
    Private Const VS_DtGrid As String = "dtGrid"

    Private Const GVCol_SrNo As Integer = 0
    Private Const GVCol_ClientContactCode As Integer = 1
    Private Const GVCol_ClientCode As Integer = 2
    Private Const GVCol_ClientName As Integer = 3
    'Private Const GVCol_ClientRemark As Integer = 4
    Private Const GVCol_ContactName As Integer = 4
    Private Const GVCol_Designation As Integer = 5
    Private Const GVCol_Address1 As Integer = 6
    Private Const GVCol_Address2 As Integer = 7
    Private Const GVCol_Address3 As Integer = 8
    Private Const GVCol_TelephoneNo As Integer = 9
    Private Const GVCol_ExtNo As Integer = 10
    Private Const GVCol_EmailId As Integer = 11
    Private Const GVCol_ModifyOn As Integer = 12
    Private Const GVCol_Active As Integer = 14

#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            GenCall()

        End If

    End Sub
#End Region

#Region "GenCall"
    Private Function GenCall() As Boolean
        Dim dt_ClientContact As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try
            
            Choice = Me.Request.QueryString("mode").ToString
            Me.ViewState(VS_Choice) = Choice   'To use it while saving

            If Not IsNothing(Me.Request.QueryString("ClientCode")) Then
                Me.ViewState(VS_ClientCode) = Me.Request.QueryString("ClientCode").Trim()
            End If


            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_ClientContactCode) = Me.Request.QueryString("Value").ToString
            End If

            ''''Check for Valid User''''''''''''''
            If Not GenCall_Data(Choice, dt_ClientContact) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_dtClientContact) = dt_ClientContact ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI(Choice, dt_ClientContact) Then 'For Displaying Data 
                Exit Function
            End If

            GenCall = True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...GenCall")
        Finally
        End Try
    End Function
#End Region

#Region "GenCall_Data"
    Private Function GenCall_Data(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                            ByRef dt_Dist_Retu As DataTable) As Boolean

        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_ClientMst As DataSet = Nothing
        'Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable
        Try
            
            If Choice_1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vClientContactCode=" + Me.ViewState(VS_ClientContactCode).ToString() 'Value of where condition
            End If

            If Not objHelp.getClientContactMatrix(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ClientMst, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_ClientMst Is Nothing Then
                Throw New Exception(eStr)
            End If

            If ds_ClientMst.Tables(0).Rows.Count <= 0 And _
               Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                Throw New Exception("No Records Found for Selected role")

            End If

            dt_Dist_Retu = ds_ClientMst.Tables(0)
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..GenCall_Data")
        Finally

        End Try
    End Function
#End Region

#Region "GenCall_ShowUI"
    Private Function GenCall_ShowUI(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                                      ByVal dt_ClientMst As DataTable) As Boolean

        Try


            Page.Title = " :: Client Contact ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Master.FindControl("lblHeading"), Label).Text = "Client Contact Matrix"

            If Not BindGrid() Then
                Return False
            End If
            If Not FillDropdown() Then
                Return False
            End If

            If Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                Me.DDLClient.SelectedValue = ConvertDbNullToDbTypeDefaultValue(dt_ClientMst.Rows(0)("vClientCode"), dt_ClientMst.Rows(0)("vClientCode").GetType)
                Me.DDLClient.Enabled = False
                Me.txtContactPerson.Text = ConvertDbNullToDbTypeDefaultValue(dt_ClientMst.Rows(0)("vContactName"), dt_ClientMst.Rows(0)("vRemark").GetType)
                Me.txtAddr1.Text = ConvertDbNullToDbTypeDefaultValue(dt_ClientMst.Rows(0)("vAddress1"), dt_ClientMst.Rows(0)("vRemark").GetType)
                Me.txtAddr2.Text = ConvertDbNullToDbTypeDefaultValue(dt_ClientMst.Rows(0)("vAddress2"), dt_ClientMst.Rows(0)("vRemark").GetType)
                Me.txtAddr3.Text = ConvertDbNullToDbTypeDefaultValue(dt_ClientMst.Rows(0)("vAddress3"), dt_ClientMst.Rows(0)("vRemark").GetType)
                Me.txtDesignation.Text = ConvertDbNullToDbTypeDefaultValue(dt_ClientMst.Rows(0)("vDesignation"), dt_ClientMst.Rows(0)("vRemark").GetType)
                Me.txtTelNo.Text = ConvertDbNullToDbTypeDefaultValue(dt_ClientMst.Rows(0)("vTelephoneNo"), dt_ClientMst.Rows(0)("vRemark").GetType)
                Me.txtExtNo.Text = ConvertDbNullToDbTypeDefaultValue(dt_ClientMst.Rows(0)("vExtNo"), dt_ClientMst.Rows(0)("vRemark").GetType)
                Me.txtEmailId.Text = ConvertDbNullToDbTypeDefaultValue(dt_ClientMst.Rows(0)("vEmailId"), dt_ClientMst.Rows(0)("vRemark").GetType)
                Me.txtFaxNo.Text = ConvertDbNullToDbTypeDefaultValue(dt_ClientMst.Rows(0)("vFaxNo"), dt_ClientMst.Rows(0)("vRemark").GetType)
                Me.ChkActive.Checked = IIf(dt_ClientMst.Rows(0)("cActiveFlag") = GeneralModule.ActiveFlag_Yes, True, False)

                Me.BtnAdd.Text = "Update"
                Me.BtnAdd.ToolTip = "Update"
            End If

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_ShowUI")
        Finally
        End Try
    End Function
#End Region

#Region "Fill DropDown and Grid"

    Private Function FillDropdown() As Boolean
        Dim ds As New DataSet
        Dim estr As String = String.Empty
        Dim dv_Client As New DataView
        Try

            If Me.DDLClient.Items.Count <= 0 Then

                If Not Me.objHelp.getclientmst("cStatusIndi <> 'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr) Then
                    Return False
                End If

                dv_Client = ds.Tables(0).DefaultView
                dv_Client.Sort = "vClientName" '"dModifyOn desc"
                Me.DDLClient.DataSource = dv_Client
                Me.DDLClient.DataTextField = "vClientName"
                Me.DDLClient.DataValueField = "vClientCode"
                Me.DDLClient.DataBind()
                Me.DDLClient.Items.Insert(0, New ListItem("Select Client", 0))

                If Not IsNothing(Me.ViewState(VS_ClientCode)) Then
                    Me.DDLClient.SelectedValue = Me.ViewState(VS_ClientCode).ToString.Trim()
                    Me.DDLClient.Enabled = False
                End If

            End If

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillDropdown")
            Return False
        End Try
    End Function

    Private Function BindGrid() As Boolean
        Dim dsclient As New DataSet
        Dim errStr As String = String.Empty
        Dim dv_Client As New DataView
        Dim Wstr As String = String.Empty
        Try

            If Not IsNothing(Me.ViewState(VS_ClientCode)) Then

                Wstr = " vClientCode='" & Me.ViewState(VS_ClientCode).ToString.Trim() & "' And cStatusIndi <> 'D'"



                If Not objHelp.GetViewClientContactMatrix(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsclient, errStr) Then

                    Me.objcommon.ShowAlert("Error While Binding Grid:" & errStr, Me.Page())
                    Return False

                End If

                dv_Client = dsclient.Tables(0).DefaultView
                dv_Client.Sort = "vClientName,vContactName"
                gvclient.DataSource = dv_Client
                gvclient.DataBind()
                dsclient.Dispose()
                Me.ViewState(VS_DtGrid) = dsclient.Tables(0)

            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...BindGrid")
            Return False
        End Try
    End Function

#End Region

#Region "DropDown Event"

    Protected Sub DDLClient_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Me.ViewState(VS_ClientCode) = Me.DDLClient.SelectedValue.Trim()
        BindGrid()

    End Sub

#End Region

#Region "Assign Values"

    Private Function AssignValue() As Boolean

        Dim Ds_ClientContact As New DataSet
        Dim dr As DataRow
        Dim Dt_ClientContact As New DataTable
        Dim dt_Grid As New DataTable
        Dim dv_ClientContact As New DataView
        Dim Dt_Delete As New DataTable
        Dim Ds_Delete As New DataSet
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim ds_Valid As New DataSet

        Try



            Dt_ClientContact = CType(Me.ViewState(VS_dtClientContact), DataTable)

            'For Validation of Duplication
            wstr = "vClientCode='" & Me.DDLClient.SelectedValue.Trim() & "' And vContactName='" & _
                Me.txtContactPerson.Text.Trim & "' And vAddress1='" & Me.txtAddr1.Text.Trim() & _
                "' And vAddress2='" & Me.txtAddr2.Text.Trim() & "' And vAddress3='" & Me.txtAddr3.Text.Trim() & _
                "' And vDesignation='" & Me.txtDesignation.Text.Trim() & "' And vEmailId='" & Me.txtEmailId.Text.Trim() & "'"

            wstr += " And vDesignation='" & Me.txtDesignation.Text.Trim() & "' And vTelephoneNo='" & Me.txtTelNo.Text.Trim() & _
                    "' And vFaxNo='" & Me.txtFaxNo.Text.Trim() & "' And vExtNo='" & Me.txtExtNo.Text.Trim() & "'"

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                wstr += " And vClientContactCode <> '" + Me.ViewState(VS_ClientContactCode).ToString() + "'"
            End If

            If Not Me.objHelp.getClientContactMatrix(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                            ds_Valid, estr) Then
                objcommon.ShowAlert("Error While Getting Data From ClientContactMatrix : " + estr, Me)
                Return False
            End If

            If ds_Valid.Tables(0).Rows.Count > 0 Then

                objcommon.ShowAlert("Client Contact Already Exist !", Me)
                Return False

            End If
            '************************

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                Dt_ClientContact.Clear()
                dr = Dt_ClientContact.NewRow
                'vClientContactCode,vClientCode,vContactName,vAddress1,vAddress2,vAddress3,vDesignation,vTelephoneNo,
                'vExtNo, vEmailId, vRemark, cActiveFlag, iModifyBy, dModifyOn, cStatusIndi
                dr("vClientContactCode") = "0000"
                dr("vClientCode") = Me.DDLClient.SelectedValue.Trim()
                dr("vContactName") = Me.txtContactPerson.Text.Trim
                dr("vAddress1") = Me.txtAddr1.Text.Trim
                dr("vAddress2") = Me.txtAddr2.Text.Trim
                dr("vAddress3") = Me.txtAddr3.Text.Trim
                dr("vDesignation") = Me.txtDesignation.Text.Trim
                dr("vTelephoneNo") = Me.txtTelNo.Text.Trim
                dr("vExtNo") = Me.txtExtNo.Text.Trim
                dr("vFaxNo") = Me.txtFaxNo.Text.Trim
                dr("vEmailId") = Me.txtEmailId.Text.Trim
                dr("vRemark") = "N/A"
                dr("cActiveFlag") = IIf(Me.ChkActive.Checked = True, "Y", "N")
                dr("cStatusIndi") = "N"
                dr("iModifyBy") = Session(S_UserID)
                Dt_ClientContact.Rows.Add(dr)
                Dt_ClientContact.AcceptChanges()

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                dr = Dt_ClientContact.Rows(0)
                dr("vClientCode") = Me.DDLClient.SelectedValue.Trim()
                dr("vContactName") = Me.txtContactPerson.Text.Trim
                dr("vAddress1") = Me.txtAddr1.Text.Trim
                dr("vAddress2") = Me.txtAddr2.Text.Trim
                dr("vAddress3") = Me.txtAddr3.Text.Trim
                dr("vDesignation") = Me.txtDesignation.Text.Trim
                dr("vTelephoneNo") = Me.txtTelNo.Text.Trim
                dr("vExtNo") = Me.txtExtNo.Text.Trim
                dr("vFaxNo") = Me.txtFaxNo.Text.Trim
                dr("vEmailId") = Me.txtEmailId.Text.Trim
                dr("vRemark") = "N/A"
                dr("cActiveFlag") = IIf(Me.ChkActive.Checked = True, "Y", "N")
                dr("cStatusIndi") = "E"
                dr("iModifyBy") = Session(S_UserID)
                dr.AcceptChanges()
                Dt_ClientContact.AcceptChanges()

            End If

            Me.ViewState(VS_dtClientContact) = Dt_ClientContact
            Me.ViewState(VS_DtGrid) = dt_Grid

            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...AssignValue")
            Return False
        End Try
    End Function

#End Region

#Region "Button Events"

    Protected Sub BtnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAdd.Click
        Dim Dt As New DataTable
        Dim Ds_clientmst As DataSet
        Dim Ds_clientmstgrid As New DataSet
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_ClientContact As DataSet = Nothing
        Dim message As String = String.Empty

        'Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable
        Try



            If Not AssignValue() Then
                Exit Sub
            End If

            Ds_clientmst = New DataSet
            Ds_clientmst.Tables.Add(CType(Me.ViewState(VS_dtClientContact), Data.DataTable).Copy())
            Ds_clientmst.Tables(0).TableName = "clientContactMatrix"   ' New Values on the form to be updated
            If Not objLambda.Save_InsertClientContactMatrix(Me.ViewState(VS_Choice), Ds_clientmst, Me.Session(S_UserID), eStr) Then

                'ShowErrorMessage(eStr, "Error While Saving ClientContactMatrix")
                objcommon.ShowAlert("Error While Saving ClientContactMatrix !", Me.Page)
                Exit Sub

            End If
            message = IIf(Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "Client Contact Details Saved Successfully !", " Client Contact Details Updated Successfully !")

          
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Alert", "ShowAlert('" + message + "')", True)
           

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....BtnAdd_Click")
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        'ResetPage()
        Me.Response.Redirect("frmClientContactMatrix.aspx?mode=1")
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Dim RedirectStr As String = ""

        RedirectStr = "frmMainPage.aspx"
        If Not IsNothing(Me.ViewState(VS_ClientCode)) Then
            RedirectStr = "frmClientMst.aspx?mode=1"
        End If
        Response.Redirect(RedirectStr)

    End Sub

#End Region

#Region "Reset Page"
    Private Sub ResetPage()
        Dim RedirectStr As String = ""
        Me.txtContactPerson.Text = ""
        Me.txtDesignation.Text = ""
        Me.txtAddr1.Text = ""
        Me.txtAddr2.Text = ""
        Me.txtAddr3.Text = ""
        Me.txtEmailId.Text = ""
        Me.txtExtNo.Text = ""
        Me.txtFaxNo.Text = ""
        Me.txtTelNo.Text = ""
        Me.ChkActive.Checked = True

        If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
            GenCall()
        Else
            RedirectStr = "frmClientContactMatrix.aspx?mode=1"
            If Not IsNothing(Me.ViewState(VS_ClientCode)) Then
                RedirectStr = "frmClientContactMatrix.aspx?mode=1&ClientCode=" & Me.ViewState(VS_ClientCode).ToString.Trim()
            End If
            Response.Redirect(RedirectStr)
        End If

    End Sub
#End Region

#Region "Grid Event"

    Protected Sub gvclient_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        e.Row.Cells(GVCol_ClientContactCode).Visible = False
        e.Row.Cells(GVCol_ClientCode).Visible = False

    End Sub

    Protected Sub gvclient_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.DataRow Then

            e.Row.Cells(GVCol_SrNo).Text = e.Row.RowIndex + 1

            e.Row.Cells(GVCol_Active).Text = IIf(e.Row.Cells(GVCol_Active).Text.ToUpper.Trim() = "Y", "YES", "NO")

            CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandName = "Edit"

            CType(e.Row.FindControl("ImbADelete"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("ImbADelete"), ImageButton).CommandName = "Delete"

        End If

    End Sub

    Protected Sub gvclient_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim index As Integer = e.CommandArgument
        Dim RedirectStr As String = ""
        If e.CommandName.ToUpper = "DELETE" Then
            'AssignValue("DELETE", index)
        ElseIf e.CommandName.ToUpper = "EDIT" Then
            RedirectStr = "frmClientContactMatrix.aspx?mode=2&value=" & Me.gvclient.Rows(index).Cells(GVCol_ClientContactCode).Text.Trim()
            If Not IsNothing(Me.ViewState(VS_ClientCode)) Then
                RedirectStr = "frmClientContactMatrix.aspx?mode=2&value=" & Me.gvclient.Rows(index).Cells(GVCol_ClientContactCode).Text.Trim() & _
                                  "&ClientCode=" & Me.ViewState(VS_ClientCode).ToString.Trim()
            End If
            Response.Redirect(RedirectStr)
        End If
    End Sub

    Protected Sub gvclient_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs)

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
End Class
