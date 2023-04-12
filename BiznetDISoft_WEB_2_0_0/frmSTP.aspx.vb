
Partial Class frmSTP
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATIONS"

    Private objCommon As New clsCommon
    Private ObjLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()
    Private ObjHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()

    Private VS_dtSTP As String = "dtSTP"
    Private VS_dtView_STP As String = "dtView_STP"
    Private VS_Choice As String = "Choice"

    Private GVC_STPNo As Integer = 0
    Private GVC_StateNo As Integer = 1
    Private GVC_CityNo As Integer = 2
    Private GVC_InvestigatorName As Integer = 3
    Private GVC_Email As Integer = 4
    Private GVC_MobNo As Integer = 5
    Private GVC_SiteName As Integer = 6
    Private GVC_SiteAddr1 As Integer = 7
    Private GVC_SiteAddr2 As Integer = 8
    Private GVC_SiteAddr3 As Integer = 9
    Private GVC_TelePhoneNo As Integer = 10
    Private GVC_FaxNo As Integer = 11
    Private GVC_ActiveFlag As Integer = 12
    Private GVC_edit As Integer = 13

#End Region
    
#Region " Page Load "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            GenCall()
            Me.btnAdd.Attributes.Add("onclientclick", "return CheckCityVal();")
        End If
    End Sub
#End Region

#Region "GenCall "

    Private Function GenCall() As Boolean
        Dim ds_Stp As DataSet = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add

            If Not IsNothing(Me.Request.QueryString("Mode")) Then
                Choice = CType(Me.Request.QueryString("Mode").ToString, WS_Lambda.DataObjOpenSaveModeEnum)
            End If

            Me.ViewState(VS_Choice) = Choice   'To use it while saving

            ''''Check for Valid User''''''''''''''

            If Not GenCall_Data(ds_Stp) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_dtSTP) = ds_Stp.Tables(0) ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If

            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "", ex)
        Finally
            ds_Stp = Nothing
        End Try

    End Function

#End Region

#Region "GenCall_Data "
    Private Function GenCall_Data(ByRef ds_STP_Retu As DataSet) As Boolean

        Dim wStr As String = ""
        Dim eStr_Retu As String = ""
        Dim Val As String = ""
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Dim ds As DataSet = Nothing

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Choice = Me.ViewState(VS_Choice)
            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            End If

            If Not ObjHelp.GetViewSTPWithScope(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds, eStr_Retu) Then
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


            ds_STP_Retu = ds

            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "", ex)

        Finally
            ds = Nothing
        End Try
    End Function
#End Region

#Region "GenCall_showUI "
    Private Function GenCall_ShowUI() As Boolean
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Try
            Page.Title = "::  Standard Coverage  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Standard Coverage"

            Choice = Me.ViewState(VS_Choice)

            fillState()

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

            End If

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "", ex)

        Finally
        End Try
    End Function

#End Region

#Region "Add Site Information"

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Dim dtSTP As New DataTable
        Dim drPlace As DataRow
        Dim dsSTP As New DataSet
        Dim dsPlace As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Dim estr As String = ""
        Try
            Choice = Me.ViewState(VS_Choice)
            dtSTP = CType(Me.ViewState(VS_dtSTP), DataTable)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                drPlace = dtSTP.NewRow
                drPlace("nSTPNo") = dtSTP.Rows.Count + 1
                drPlace("vWorkspaceId") = Me.HProjectId.Value.Trim()
                drPlace("iSerialNo") = dtSTP.Rows.Count + 1
                drPlace("vInvestigatorName") = Me.txtInvestigator.Text
                drPlace("vMobNo") = Me.txtMobile.Text
                drPlace("vEmail") = Me.txtEmail.Text
                drPlace("nCityNo") = Me.ddlPlaceCity.SelectedValue.Trim()
                drPlace("vSiteName") = Me.txtSiteName.Text.Trim()
                drPlace("vSiteAddr1") = Me.txtSiteAddr1.Text.Trim()
                drPlace("vSiteAddr2") = Me.txtSiteAddr2.Text.Trim()
                drPlace("vSiteAddr3") = Me.txtSiteAddr3.Text.Trim()
                drPlace("vTelePhoneNo") = Me.TxtTelePhone.Text.Trim()
                drPlace("vFaxNo") = Me.TxtFax.Text.Trim()
                drPlace("cActiveFlag") = IIf(Me.ChkActive.Checked, "Y", "N")
                drPlace("iModifyBY") = Me.Session(S_UserID)

                dtSTP.Rows.Add(drPlace)
                dtSTP.AcceptChanges()

            ElseIf Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                For Each drPlace In dtSTP.Rows
                    drPlace("vInvestigatorName") = Me.txtInvestigator.Text
                    drPlace("vMobNo") = Me.txtMobile.Text
                    drPlace("vEmail") = Me.txtEmail.Text
                    drPlace("nCityNo") = Me.ddlPlaceCity.SelectedValue.Trim()
                    drPlace("vSiteName") = Me.txtSiteName.Text.Trim()
                    drPlace("vSiteAddr1") = Me.txtSiteAddr1.Text.Trim()
                    drPlace("vSiteAddr2") = Me.txtSiteAddr2.Text.Trim()
                    drPlace("vSiteAddr3") = Me.txtSiteAddr3.Text.Trim()
                    drPlace("vTelePhoneNo") = Me.TxtTelePhone.Text.Trim()
                    drPlace("vFaxNo") = Me.TxtFax.Text.Trim()
                    drPlace("cActiveFlag") = IIf(Me.ChkActive.Checked, "Y", "N")
                    drPlace("iModifyBY") = Me.Session(S_UserID)
                    drPlace.AcceptChanges()
                Next drPlace

            End If

            Me.ViewState(VS_dtSTP) = dtSTP
            dtSTP.AcceptChanges()
            dsSTP.Tables.Add(dtSTP.Copy())

            If Not ObjLambda.Save_InsertSTP(Me.ViewState(VS_Choice), dsSTP, Me.Session(S_UserID), estr) Then
                Me.ShowErrorMessage("", estr)
                Exit Sub
            End If

            objCommon.ShowAlert("Site Added Sucessfully.", Me)
            ResetPage()

            If Not FillGrid() Then
                Me.objCommon.ShowAlert("Error while filling Grid", Me.Page)
                Exit Sub
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "", ex)
        Finally
            dtSTP = Nothing
        End Try
    End Sub

    Private Function ChkSTP() As Boolean

        Dim dtCHK As New DataTable
        Dim dvCHK As DataView
        Dim placeDesc As String = ""
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            'Checking for Duplication

            dvCHK = CType(Me.ViewState(VS_dtView_STP), DataTable).DefaultView
            dvCHK.RowFilter = "cActiveflag='Y' and nCityNo=" & Me.ddlPlaceCity.SelectedValue.Trim()
            dtCHK = dvCHK.ToTable()

            If Not IsNothing(dtCHK) Then

                If dtCHK.Rows.Count > 0 Then
                    objCommon.ShowAlert("This Site is Already Submitted please select Another Site !!!", Me.Page)
                    Me.ResetPage()
                    Return False
                    Exit Function
                End If

            End If
            Return True
            '***************
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "", ex)
        End Try
    End Function

#End Region

#Region "ResetPage"
    Private Sub ResetPage()
        Dim ds_Stp As DataSet = Nothing
        Me.txtInvestigator.Text = ""
        Me.txtMobile.Text = ""
        Me.txtEmail.Text = ""
        Me.TxtFax.Text = ""
        Me.txtSiteAddr1.Text = ""
        Me.txtSiteAddr2.Text = ""
        Me.txtSiteAddr3.Text = ""
        Me.txtSiteName.Text = ""
        Me.TxtTelePhone.Text = ""
        Me.ddlFromstate.SelectedIndex = 0
        If Me.ddlPlaceCity.Items.Count > 0 Then
            Me.ddlPlaceCity.SelectedIndex = 0
        End If
        Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
    End Sub
#End Region

#Region "GridView Events"

    Protected Sub GVSiteDesc_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVSiteDesc.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or _
        e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(GVC_STPNo).Visible = False
            e.Row.Cells(GVC_StateNo).Visible = False
            e.Row.Cells(GVC_CityNo).Visible = False
        End If
    End Sub

    Protected Sub GVSiteDesc_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        GVSiteDesc.PageIndex = e.NewPageIndex
        If Not FillGrid() Then
            Exit Sub
        End If
    End Sub

    Protected Sub GVSiteDesc_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVSiteDesc.RowDataBound
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.FindControl("lnkEdit"), LinkButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("lnkEdit"), LinkButton).CommandName = "EDIT"

            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ex.Message)
        End Try
    End Sub

    Protected Sub GVSiteDesc_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim index As Integer = CInt(e.CommandArgument)
        Dim STPNo As String = ""
        Try
            If CType(Me.GVSiteDesc.Rows(index).FindControl("lnkEdit"), LinkButton).CommandName.ToUpper = "EDIT" Then
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                STPNo = Me.GVSiteDesc.Rows(index).Cells(GVC_STPNo).Text.Trim()
                If Not FillControls(STPNo) Then
                    Me.objCommon.ShowAlert("Error while filling Editable Detail.", Me.Page)
                    Exit Sub
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GVSiteDesc_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs)
        e.Cancel = True
    End Sub

#End Region

#Region "Button Click"
    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ResetPage()
    End Sub

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Not FillGrid() Then
                Me.objCommon.ShowAlert("Error while filling Grid", Me.Page)
                Exit Sub
            End If

        Catch ex As Exception
            Me.ShowErrorMessage("", ex.Message)
        End Try
    End Sub
#End Region

#Region "Selected Index Changed"

    Protected Sub ddlFromstate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        fillCity()
    End Sub

#End Region

#Region "Fill Functions"

    Private Function FillGrid() As Boolean
        Dim wStr As String = ""
        Dim eStr As String = ""
        Dim dsPlace As DataSet = Nothing
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            wStr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "'"

            If Not Me.ObjHelp.GetViewSTPWithScope(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsPlace, eStr) Then
                Me.ShowErrorMessage("", eStr)
                Exit Function
            End If

            GVSiteDesc.DataSource = dsPlace
            GVSiteDesc.DataBind()
            Me.ViewState(VS_dtView_STP) = dsPlace.Tables(0)
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage("", ex.Message)
            Return False
        End Try
    End Function

    Private Function FillControls(ByVal STPNo As String) As Boolean
        Dim wStr As String = ""
        Dim eStr As String = ""
        Dim dsPlace As New DataSet
        Dim drPlace As DataRow
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            wStr = "nSTPNo=" & STPNo.Trim()
            If Not Me.ObjHelp.GetViewSTPWithScope(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsPlace, eStr) Then
                Me.ShowErrorMessage("", eStr)
                Exit Function
            End If

            For Each drPlace In dsPlace.Tables(0).Rows
                Me.HProjectId.Value = drPlace("vWorkspaceId")
                Me.txtproject.Text = drPlace("vWorkSpaceDesc")
                Me.ddlFromstate.SelectedValue = drPlace("nStateNo")
                fillCity()
                Me.ddlPlaceCity.SelectedValue = drPlace("nCityNo")
                Me.txtInvestigator.Text = drPlace("vInvestigatorName")
                Me.txtMobile.Text = drPlace("vMobNo")
                Me.txtEmail.Text = drPlace("vEmail")
                Me.txtSiteName.Text = drPlace("vSiteName")
                Me.txtSiteAddr1.Text = drPlace("vSiteAddr1")
                Me.txtSiteAddr2.Text = drPlace("vSiteAddr2")
                Me.txtSiteAddr3.Text = drPlace("vSiteAddr3")
                Me.TxtTelePhone.Text = drPlace("vTelePhoneNo")
                Me.TxtFax.Text = drPlace("vFaxNo")
                Me.ChkActive.Checked = IIf(drPlace("cActiveFlag") = "Y", True, False)
            Next drPlace

            Me.ViewState(VS_dtSTP) = dsPlace.Tables(0)
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage("", ex.Message)
            Return False
        End Try
    End Function

    Private Sub fillState()
        Dim wStr As String = ""
        Dim eStr As String = ""
        Dim dsState As DataSet = Nothing
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Not Me.ObjHelp.GetStateMSt(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, dsState, eStr) Then
                Me.ShowErrorMessage("", eStr)
                Exit Sub
            End If

            dsState.Tables(0).DefaultView.Sort = "vStateName"
            Me.ddlFromstate.DataSource = dsState.Tables(0).DefaultView
            Me.ddlFromstate.DataTextField = "vStateName"
            Me.ddlFromstate.DataValueField = "nStateNo"
            Me.ddlFromstate.DataBind()
            Me.ddlFromstate.Items.Insert(0, New ListItem("Select State", 0))

        Catch ex As Exception
            Me.ShowErrorMessage("", ex.Message)

        End Try
    End Sub

    Private Sub fillCity()
        Dim wStr As String = ""
        Dim eStr As String = ""
        Dim ds_PlaceMst As New DataSet
        Dim dv_PlaceMst As New DataView

        If Me.ddlFromstate.SelectedIndex = 0 Then
            objCommon.ShowAlert("Please select from state", Me)
            Exit Sub
        End If

        wStr = "nStateNo = " + Me.ddlFromstate.SelectedValue.Trim()

        Me.ObjHelp.GetPlaceMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_PlaceMst, eStr)

        dv_PlaceMst = ds_PlaceMst.Tables(0).DefaultView.ToTable(True, "nCityNo,vCityName".Split(",")).DefaultView
        dv_PlaceMst.Sort = "vCityName"
        Me.ddlPlaceCity.DataSource = dv_PlaceMst
        Me.ddlPlaceCity.DataTextField = "vCityName"
        Me.ddlPlaceCity.DataValueField = "nCityNo"
        Me.ddlPlaceCity.DataBind()
        Me.ddlPlaceCity.Items.Insert(0, New ListItem("Select City", 0))
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

End Class
