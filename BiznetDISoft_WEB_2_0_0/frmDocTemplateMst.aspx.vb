
Partial Class frmDocTemplateMst
    Inherits System.Web.UI.Page

#Region "Varible Declaration"

    Dim ObjCommon As New clsCommon
    Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private Const VS_Choice As String = "Choice"
    Private Const VS_SAVE As String = "Save"
    Private Const VS_DtDocTemplateMst As String = "DtDocTemplateMst"
    Private Const VS_DocTemplateId As String = "DocTemplateId"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_DocTemplateId As Integer = 1
    Private Const GVC_DocTemplateName As Integer = 2
    Private Const GVC_DocTemplateDesc As Integer = 3
    Private Const GVC_Modifyon As Integer = 4
    Private Const GVC_Edit As Integer = 5

#End Region

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
                Me.ViewState(VS_DocTemplateId) = Me.Request.QueryString("Value").ToString
            End If

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtDocTemplateMst) = ds.Tables("DocTemplateMst")   ' adding blank DataTable in viewstate

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

            Val = Me.ViewState(VS_DocTemplateId) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vDocTemplateId=" + Val.ToString
            End If


            If Not objHelp.getDocTemplateMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
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
            Page.Title = " :: Document Template Master ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "DocTemplate Master"

            dt_OpMst = Me.ViewState(VS_DtDocTemplateMst)

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
            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                If dt_OpMst.Rows(0).Item("vDocTemplateName") Is System.DBNull.Value Then
                    Me.txtDocTemplateName.Text = ""
                Else
                    Me.txtDocTemplateName.Text = dt_OpMst.Rows(0).Item("vDocTemplateName")
                End If
            End If
            Me.BtnSave.Attributes.Add("OnClick", "return Validation();")

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function
#End Region

#Region "FillGrid"
    Private Function FillGrid() As Boolean
        Dim ds_View As New Data.DataSet
        Dim estr As String = ""
        Try

            If Not objHelp.getDoctemplatemst("cStatusIndi <> 'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_View, estr) Then
                Return False
            End If

            Me.gvdoctemplatemst.DataSource = ds_View
            Me.gvdoctemplatemst.DataBind()

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region "Button Click"
    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim ds_Save As New DataSet
        Dim dt_Save As New DataTable
        Dim estr As String = ""
        Try
            If Not AssignValues() Then
                Exit Sub
            End If
            ds_Save = New DataSet
            dt_Save = CType(Me.ViewState(VS_DtDocTemplateMst), DataTable)
            dt_Save.TableName = "DocTemplateMst"
            ds_Save.Tables.Add(dt_Save)
            If Not objLambda.Save_InsertDocTemplateMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_DocTemplateMst, ds_Save, Me.Session(S_UserID), estr) Then
                ObjCommon.ShowAlert("Error While Saving DocTemplateMst", Me.Page)
                Exit Sub
            Else
                ObjCommon.ShowAlert("Record Saved SuccessFully !", Me.Page)
                ResetPage()
            End If


        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub
    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.ResetPage()
    End Sub
#End Region

#Region "Assign values"
    Private Function AssignValues() As Boolean
        Dim dr As DataRow
        Dim dt_User As New DataTable
        Dim ds_Check As New DataSet
        Dim eStr As String = ""
        Try

            dt_User = CType(Me.ViewState(VS_DtDocTemplateMst), DataTable)

            'If Not objHelp.getDoctemplatemst("cStatusIndi <> 'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Check, eStr) Then
            '    Me.ShowErrorMessage("Error While Getting Data From DocTemplateMst", eStr)
            '    Return False
            'End If

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                'For Each dr In ds_Check.Tables(0).Rows
                '    If dr("vDocTemplateName").ToString().Trim() = Me.txtDocTemplateName.Text.Trim() Then
                '        ObjCommon.ShowAlert("DocTemplate Name Already Exists", Me.Page)
                '        Return False
                '    End If
                'Next

                dt_User.Clear()
                dr = dt_User.NewRow()
                'vDocTemplateId,vDocTemplateName,vDocTemplateDesc,iModifyBy,dModifyOn,cStatusIndi
                dr("vDocTemplateId") = "0"
                dr("vDocTemplateName") = Me.txtDocTemplateName.Text.Trim()
                'dr("vDocTemplateDesc") = Me.txtDescription.Text.Trim()
                dr("cStatusIndi") = "N"
                dr("iModifyBy") = Me.Session(S_UserID)
                dt_User.Rows.Add(dr)
            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                'For Each dr In ds_Check.Tables(0).Rows

                '    If dr("vDocTemplateId").ToString().Trim() <> dt_User.Rows(0).Item("vDocTemplateId").ToString().Trim() _
                '                        And dr("vDocTemplateName").ToString().Trim() = Me.txtDocTemplateName.Text.Trim() Then
                '        ObjCommon.ShowAlert("DocTemplate Name Already Exists", Me.Page)
                '        Return False
                '    End If
                'Next

                For Each dr In dt_User.Rows
                    dr("vDocTemplateName") = Me.txtDocTemplateName.Text.Trim()
                    'dr("vDocTemplateDesc") = Me.txtDescription.Text.Trim()
                    dr("cStatusIndi") = "E"
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr.AcceptChanges()
                Next
                dt_User.AcceptChanges()
            End If

            Me.ViewState(VS_DtDocTemplateMst) = dt_User
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function
#End Region

#Region "Reset Page"
    Private Sub ResetPage()
        Me.txtDocTemplateName.Text = ""
        'Me.txtDescription.Text = ""
        Me.ViewState(VS_DtDocTemplateMst) = Nothing
        Me.Response.Redirect("frmDocTemplateMst.aspx?mode=1")
        'Me.Response.Redirect("frmDocTemplateMst.aspx?mode=1&Save=Y")
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

#Region "Grid Events"

    Protected Sub gvdoctemplatemst_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvdoctemplatemst.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + 1
            CType(e.Row.FindControl("lnkEdit"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkEdit"), LinkButton).CommandName = "Edit"
        End If
    End Sub

    Protected Sub gvdoctemplatemst_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvdoctemplatemst.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or _
                e.Row.RowType = DataControlRowType.Header Or _
                e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(GVC_DocTemplateId).Visible = False
        End If
    End Sub

    Protected Sub gvdoctemplatemst_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvdoctemplatemst.RowCommand
        Dim i As Integer = e.CommandArgument
        If e.CommandName.ToUpper = "EDIT" Then
            Me.Response.Redirect("frmDocTemplateMst.aspx?mode=2&value=" & Me.gvdoctemplatemst.Rows(i).Cells(GVC_DocTemplateId).Text.Trim())
        End If
    End Sub

    Protected Sub gvdoctemplatemst_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvdoctemplatemst.PageIndexChanging
        gvdoctemplatemst.PageIndex = e.NewPageIndex
        If Not FillGrid() Then
            Exit Sub
        End If
    End Sub

#End Region

End Class
