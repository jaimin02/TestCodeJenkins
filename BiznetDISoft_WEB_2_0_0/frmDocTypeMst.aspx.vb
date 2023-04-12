
Partial Class frmAddDocTypeMst
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtDocTypeMst As String = "dtDocTypeMst"
    Private Const VS_DocTypeCode As String = "vDocTypeCode"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_Name As Integer = 1
    Private Const GVC_Remark As Integer = 2
    Private Const GVC_Indicator As Integer = 3
    Private Const GVC_ReviwedDays As Integer = 4
    Private Const GVC_Modifyon As Integer = 5
    Private Const GVC_Edit As Integer = 6
    Private Const GVC_Code As Integer = 7

#End Region
    
#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            GenCall()
            Exit Sub
        End If
    End Sub

#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean
        Dim dt_DoctypeMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Choice = Me.Request.QueryString("mode").ToString
            Me.ViewState(VS_Choice) = Choice   'To use it while saving

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_DocTypeCode) = Me.Request.QueryString("Value").ToString
            End If

            ''''Check for Valid User''''''''''''''
            If Not GenCall_Data(Choice, dt_DoctypeMst) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtDocTypeMst) = dt_DoctypeMst ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI(Choice, dt_DoctypeMst) Then 'For Displaying Data 
                Exit Function
            End If
            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideDocTypeDetails", "HideDocTypeDetails(); ", True)
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
        Dim ds_DocTypeMst As DataSet = Nothing
        'Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Choice_1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vDocTypeCode=" + Me.ViewState(VS_DocTypeCode).ToString() 'Value of where condition
            End If

            If Not objHelp.GetDoctypeMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_DocTypeMst, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_DocTypeMst Is Nothing Then
                Throw New Exception(eStr)
            End If

            If ds_DocTypeMst.Tables(0).Rows.Count <= 0 And _
               Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                Throw New Exception("No Records Found for Selected role")

            End If

            dt_Dist_Retu = ds_DocTypeMst.Tables(0)
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_ShowUI"

    Private Function GenCall_ShowUI(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                                      ByVal dt_Dist As DataTable) As Boolean
        Dim dt_DocTypeMst As DataTable = Nothing
        Page.Title = " ::  Document Type Master ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
        CType(Master.FindControl("lblHeading"), Label).Text = "Document Type Master"

        BindGrid()

        dt_DocTypeMst = Me.ViewState(VS_DtDocTypeMst)

        If Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
            Me.txtDocTypeName.Text = ConvertDbNullToDbTypeDefaultValue(dt_DocTypeMst.Rows(0)("vDocTypeName"), dt_DocTypeMst.Rows(0)("vDocTypeName").GetType)
            Me.txtremark.Text = ConvertDbNullToDbTypeDefaultValue(dt_DocTypeMst.Rows(0)("vRemark"), dt_DocTypeMst.Rows(0)("vRemark").GetType)
            Me.ddlIndicator.SelectedValue = ConvertDbNullToDbTypeDefaultValue(dt_DocTypeMst.Rows(0)("cDocTypeIndi"), dt_DocTypeMst.Rows(0)("cDocTypeIndi").GetType)
            Me.txtReviewedDays.Text = ConvertDbNullToDbTypeDefaultValue(dt_DocTypeMst.Rows(0)("iReviewWithinDays"), dt_DocTypeMst.Rows(0)("iReviewWithinDays").GetType)
        End If

        Me.btnSave.Attributes.Add("OnClick", "return Validation();")
        Me.txtReviewedDays.Attributes.Add("onblur", "return Numeric();")
        GenCall_ShowUI = True

    End Function

#End Region

#Region "Reset Page"
    Private Sub ResetPage()
        Me.txtDocTypeName.Text = ""
        Me.txtremark.Text = ""
        Me.txtReviewedDays.Text = ""
        Me.ddlIndicator.SelectedIndex = -1
        Me.Response.Redirect("frmDocTypeMst.aspx?mode=1")
    End Sub
#End Region

#Region "AssignUpdatedValues"

    Private Function AssignUpdatedValues() As Boolean
        Dim dtOld As New DataTable
        Dim dr As DataRow
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            dtOld = Me.ViewState(VS_DtDocTypeMst)

           

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                dr = dtOld.NewRow
                dr(VS_DocTypeCode) = "0000"
                dr("vDocTypeName") = Me.txtDocTypeName.Text.Trim

                If Me.ddlIndicator.SelectedIndex = 0 Then
                    objcommon.ShowAlert("Please Select Atleast One Indicator !", Me)
                    Exit Function
                End If

                dr("cDocTypeIndi") = Me.ddlIndicator.SelectedItem.Value
                dr("iReviewWithinDays") = Me.txtReviewedDays.Text.Trim
                dr("vRemark") = Me.txtremark.Text.Trim
                dr("iModifyBy") = Session(S_UserID)
                dtOld.Rows.Add(dr)

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                For Each dr In dtOld.Rows
                    dr("vDocTypeName") = Me.txtDocTypeName.Text.Trim

                    If Me.ddlIndicator.SelectedIndex = 0 Then
                        objcommon.ShowAlert("Please Select Atleast One Indicator !", Me)
                        Exit Function
                    End If

                    dr("cDocTypeIndi") = Me.ddlIndicator.SelectedItem.Value
                    dr("iReviewWithinDays") = Me.txtReviewedDays.Text.Trim
                    dr("vRemark") = Me.txtremark.Text.Trim
                    dr("iModifyBy") = Session(S_UserID)
                    dr("cStatusIndi") = "E"
                    dr.AcceptChanges()
                Next
                dtOld.AcceptChanges()

            End If

            Me.ViewState(VS_DtDocTypeMst) = dtOld

            Return True
        Catch Threadx As Threading.ThreadAbortException
            'ShowErrorMessage(ex.Message, "")

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region "Button Events"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim Dt As New DataTable
        Dim Ds_doctypemst As DataSet
        Dim ds_doctypegrid As New DataSet
        Dim eStr As String = ""
        Dim objOPws As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            AssignUpdatedValues()
            Ds_doctypemst = New DataSet
            Ds_doctypemst.Tables.Add(CType(Me.ViewState(VS_DtDocTypeMst), Data.DataTable).Copy())
            Ds_doctypemst.Tables(0).TableName = "view_doctemplateMatrix"   ' New Values on the form to be updated

            If Not objOPws.Save_DoctypeMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_DoctypeMst, Ds_doctypemst, Me.Session(S_UserID), eStr) Then
                ShowErrorMessage(eStr, "")
                Exit Sub
            End If

            objcommon.ShowAlert("Doc Type Saved Successfully !", Me)
            BindGrid()
            ResetPage()

        Catch Threadx As Threading.ThreadAbortException
        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr)
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ResetPage()
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Response.Redirect("frmMainPage.aspx")
    End Sub

#End Region

#Region "Grid Event"

    Protected Sub gvdoctypemst_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.DataRow Or _
        e.Row.RowType = DataControlRowType.Header Or _
        e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVC_Code).Visible = False

        End If

    End Sub

    Protected Sub gvdoctypemst_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        gvdoctypemst.PageIndex = e.NewPageIndex
        BindGrid()
    End Sub

    Private Sub BindGrid()
        Dim dsDocType As New DataSet
        Dim eStr As String = ""

        If objHelp.GetDoctypeMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, dsDocType, eStr) Then
            gvdoctypemst.ShowFooter = False
            gvdoctypemst.DataSource = dsDocType
            gvdoctypemst.DataBind()

            If gvdoctypemst.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvdoctypemst", "UIgvdoctypemst(); ", True)
            End If



            dsDocType.Dispose()
        End If

    End Sub

    Protected Sub gvdoctypemst_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.DataRow Then

            e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (Me.gvdoctypemst.PageIndex * Me.gvdoctypemst.PageSize) + 1

            If e.Row.Cells(GVC_Indicator).Text = "T" Then
                e.Row.Cells(GVC_Indicator).Text = "Template"
            ElseIf e.Row.Cells(GVC_Indicator).Text = "S" Then
                e.Row.Cells(GVC_Indicator).Text = "Sample"
            End If


            CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandName = "Edit"

        End If

    End Sub

    Protected Sub gvdoctypemst_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim index As Integer = e.CommandArgument
        If e.CommandName.ToUpper = "EDIT" Then
            Me.Response.Redirect("frmDocTypeMst.aspx?mode=2&value=" & Me.gvdoctypemst.Rows(index).Cells(GVC_Code).Text.Trim())
        End If
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
