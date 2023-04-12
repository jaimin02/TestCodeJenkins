Imports System.Data
Partial Class frmAddTemplateMst
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION "

    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()

    Private objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()

    Private Const VS_Choice As String = "Choice"
    'Private Const VS_TemplateId As String = "vTemplateId"
    Private Const VS_TemplateMst As String = "dtTemplateMst"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_TemplateName As Integer = 1
    Private Const GVC_ProjectTypeName As Integer = 2
    Private Const GVC_TemplateTypeName As Integer = 3
    Private Const GVC_Modifyon As Integer = 4
    Private Const GVC_Status As Integer = 5
    Private Const GVC_Add As Integer = 6
    Private Const GVC_Copy As Integer = 7
    Private Const GVC_Edit As Integer = 8
    Private Const GVC_Id As Integer = 9

#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            GenCall()
            Exit Sub
        End If
    End Sub

    Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Gvtemplate.Rows.Count > 0 Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIGvtemplate", "UIGvtemplate(); ", True)
        End If
    End Sub
#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean
        Dim dt_TemplateMst As DataTable = Nothing
        'Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try


            'Choice = Me.Request.QueryString("mode").ToString
            Me.ViewState(VS_Choice) = Me.Request.QueryString("mode").ToString   'To use it while saving

            'If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

            '    Me.ViewState(VS_TemplateId) = Me.Request.QueryString("Value").ToString

            'End If

            ''''Check for Valid User'''
            If Not GenCall_Data() Then ' For Data Retrieval
                Exit Function
            End If

            'Me.ViewState(VS_TemplateMst) = dt_TemplateMst ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI() Then 'For Displaying Data 
                Exit Function
            End If
            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideTemplateDetails", "HideTemplateDetails(); ", True)
            End If
            GenCall = True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...GenCall")
            GenCall = False
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_Data"

    Private Function GenCall_Data() As Boolean

        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        'Dim eStr_Retu As String = ""
        Dim ds As DataSet = Nothing
        'Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Try


            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vTemplateId=" + Me.Request.QueryString("Value").ToString  'Value of where condition
            End If

            If Not objHelp.getTemplateMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr) Then
                Exit Function
            End If

            If ds Is Nothing Then
                Throw New Exception(eStr)
            End If

            If ds.Tables(0).Rows.Count <= 0 And _
               Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                Throw New Exception("No Records Found for Selected role")

            End If

            ' dt_Dwr_Retu = ds.Tables(0)
            Me.ViewState(VS_TemplateMst) = ds.Tables(0)
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_Data")
        Finally

        End Try
    End Function

#End Region

#Region "GenCall_ShowUI"
    Private Function GenCall_ShowUI() As Boolean
        Dim dt_TemplateMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try
            Page.Title = " :: Template Structure Management ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            Choice = Me.ViewState(VS_Choice)
            dt_TemplateMst = Me.ViewState(VS_TemplateMst)
            CType(Master.FindControl("lblHeading"), Label).Text = "Template Structure Management"

            If Not FillDropDown() Then
                Return False
            End If

            BindGrid()

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.txtTemplateName.Text = ConvertDbNullToDbTypeDefaultValue(dt_TemplateMst.Rows(0)("vTemplateDesc"), dt_TemplateMst.Rows(0)("vTemplateDesc").GetType)
                Me.ddlTemplateType.SelectedValue = ConvertDbNullToDbTypeDefaultValue(dt_TemplateMst.Rows(0)("vTemplateTypeCode"), dt_TemplateMst.Rows(0)("vTemplateTypeCode").GetType)
                Me.ddlProjectType.SelectedValue = ConvertDbNullToDbTypeDefaultValue(dt_TemplateMst.Rows(0)("vProjectTypeCode"), dt_TemplateMst.Rows(0)("vProjectTypeCode").GetType)
                Me.btnSave.Text = "Update"
                Me.btnSave.ToolTip = "Update"
            End If

            'Me.btnSave.Attributes.Add("OnClick", "return Validation();")

            GenCall_ShowUI = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_ShowUI")
            GenCall_ShowUI = False
        End Try
    End Function
#End Region

#Region "Reset Page"

    Private Sub ResetPage()
        Me.txtTemplateName.Text = ""
        Me.ddlTemplateType.SelectedIndex = 0
        'Response.Redirect("frmAddTemplateMst.aspx?mode=1&choice=1")
    End Sub

#End Region

#Region "Fill DropDown"

    Private Function FillDropDown() As Boolean
        Dim estr As String = String.Empty
        Dim ds_ProjectType As New Data.DataSet
        Dim ds_type As New Data.DataSet
        Dim Wstr_Scope As String = String.Empty
        Dim dv_TemplateType As New DataView
        Dim dv_ProjectType As New DataView
        Try


            If Not objHelp.getTemplateTypeMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_type, estr) Then

                ShowErrorMessage("Error while getting TemplateTypeMst.", estr)
                Return False

            End If

            dv_TemplateType = ds_type.Tables(0).DefaultView
            dv_TemplateType.Sort = "vTemplateTypeName"
            Me.ddlTemplateType.DataSource = dv_TemplateType
            Me.ddlTemplateType.DataValueField = "vTemplateTypeCode"
            Me.ddlTemplateType.DataTextField = "vTemplateTypeName"
            Me.ddlTemplateType.DataBind()
            Me.ddlTemplateType.Items.Insert(0, New ListItem("Select Template Type", ""))


            'To Get Where condition of ScopeVales( Project Type )
            If Not objcommon.GetScopeValueWithCondition(Wstr_Scope) Then
                Return False
            End If

            If Not objHelp.GetviewProjectTypeMst(Wstr_Scope, _
                            WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ProjectType, estr) Then

                ShowErrorMessage("Error while getting ProjectTypeMst", estr)
                Return False

            End If

            dv_ProjectType = ds_ProjectType.Tables(0).DefaultView
            dv_ProjectType.Sort = "vProjectTypeName"
            Me.ddlProjectType.DataSource = dv_ProjectType
            Me.ddlProjectType.DataValueField = "vProjectTypeCode"
            Me.ddlProjectType.DataTextField = "vProjectTypeName"
            Me.ddlProjectType.DataBind()
            Me.ddlProjectType.Items.Insert(0, New ListItem("Select Project Type", ""))

            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, " ...FillDropDown")
        End Try
    End Function

#End Region

#Region "BindGrid"

    Private Sub BindGrid()
        Dim dsTemplate As New DataSet
        Dim eStr As String = String.Empty
        Dim Wstr_Scope As String = String.Empty

        'To Get Where condition of ScopeVales( Project Type )
        Try
            If Not objcommon.GetScopeValueWithCondition(Wstr_Scope) Then
                Exit Sub
            End If
            If Wstr_Scope.Length <= 0 Then
                Wstr_Scope = "1 = 1"
            End If
            Wstr_Scope += " Order By vTemplateDesc"
            If objHelp.GetViewTemplateMst(Wstr_Scope, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsTemplate, eStr) Then
                Gvtemplate.ShowFooter = False
                Gvtemplate.DataSource = dsTemplate
                Gvtemplate.DataBind()
                If Gvtemplate.Rows.Count > 0 Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIGvtemplate", "UIGvtemplate(); ", True)
                End If
                dsTemplate.Dispose()
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, " ...BindGrid")
        End Try


    End Sub
#End Region

#Region "AssignUpdatedValues"

    Private Function AssignUpdatedValues() As Boolean
        Dim dtOld As New DataTable
        Dim dr As DataRow
        Try


            dtOld = Me.ViewState(VS_TemplateMst)
            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                If Not ValidataTemplate("ADD", Me.txtTemplateName.Text.Trim) Then
                    Return False
                End If

                dtOld.Clear()
                dr = dtOld.NewRow
                dr("vTemplateId") = Me.ddlTemplateType.SelectedValue.Trim
                dr("vTemplateDesc") = Me.txtTemplateName.Text.Trim
                dr("vTemplateTypeCode") = Me.ddlTemplateType.SelectedValue.Trim
                dr("vProjectTypeCode") = Me.ddlProjectType.SelectedValue.Trim
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("vRemark") = "None"
                dtOld.Rows.Add(dr)

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                If Not ValidataTemplate("EDIT", Me.txtTemplateName.Text.Trim) Then
                    Return False
                End If

                dr = dtOld.Rows(0)
                dr("vTemplateDesc") = Me.txtTemplateName.Text.Trim
                dr("vTemplateTypeCode") = Me.ddlTemplateType.SelectedValue.Trim
                dr("vProjectTypeCode") = Me.ddlProjectType.SelectedValue.Trim
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("vRemark") = "None"
                dr.AcceptChanges()
                dtOld.AcceptChanges()

            End If

            Me.ViewState(VS_TemplateMst) = dtOld
            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, " ....AssignUpdateValues")
            Return False
        End Try

    End Function

#End Region

#Region "Validate Template"

    Private Function ValidataTemplate(ByVal Type As String, ByVal TemplateDesc As String) As Boolean
        Dim ds_Valid As New DataSet
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty

        'For validating Duplication 
        Wstr = "vTemplateDesc='" & TemplateDesc & "'"


        If Me.Request.QueryString("mode") = 2 Then
            Wstr += " And vTemplateID <> '" + Me.Request.QueryString("Value").ToString + "'"
        End If
        Me.objHelp.getTemplateMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        ds_Valid, estr)

        If ((Type.ToUpper = "ADD" Or Type.ToUpper = "COPY") And ds_Valid.Tables(0).Rows.Count > 0) Or _
            (Type.ToUpper = "EDIT" And ds_Valid.Tables(0).Rows.Count >= 1) Then

            objcommon.ShowAlert("Template Name Is Already Exist For Selected Project Type !", Me.Page)
            Return False

        End If
        '**************************
        Return True

    End Function

#End Region

#Region "Button Events"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim Dt As New DataTable
        Dim Ds As DataSet
        Dim Ds_Grid As New DataSet
        Dim eStr As String = String.Empty
        Dim ds_type As New Data.DataSet
        Dim message As String = String.Empty

        Try

            If Not AssignUpdatedValues() Then
                Exit Sub
            End If

            Ds = New DataSet
            Ds.Tables.Add(CType(Me.ViewState(VS_TemplateMst), Data.DataTable).Copy())
            Ds.Tables(0).TableName = "templatemst"   ' New Values on the form to be updated

            If Not objLambda.Save_TemplateMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_TemplateMst, Ds, "1", eStr) Then
                objcommon.ShowAlert("Error While Saveing Templatemst", Me.Page)
                Exit Sub
            End If

            'objcommon.ShowAlert("Record Saved Successfully", Me)
            message = IIf(Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "Template Saved Successfully !", "Template Updated Successfully !")
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Alert", "ShowAlert('" + message + "')", True)
            ResetPage()
            BindGrid()

        Catch ex As System.Threading.ThreadAbortException
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...btnSave_Click")
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ResetPage()
        Me.Response.Redirect("frmAddTemplateMst.aspx?mode=1")
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Response.Redirect("frmMainPage.aspx")
    End Sub

#End Region

#Region "Grid Event"

    Protected Sub Gvtemplate_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        'If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Or _
        '    e.Row.RowType = DataControlRowType.Header Then

        '    e.Row.Cells(GVC_Id).Visible = False
        '    e.Row.Cells(GVC_Status).Visible = False
        '    e.Row.Cells(GVC_Modifyon).Visible = False
        'End If

    End Sub

    Protected Sub Gvtemplate_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)

        Gvtemplate.PageIndex = e.NewPageIndex
        BindGrid()

    End Sub

    Protected Sub Gvtemplate_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)


        If Not e.Row.RowType = DataControlRowType.Pager Then
            e.Row.Cells(GVC_Id).Visible = False
            e.Row.Cells(GVC_Status).Visible = False
            e.Row.Cells(GVC_Modifyon).Visible = False

            If e.Row.RowType = DataControlRowType.DataRow Then

                e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (Me.Gvtemplate.PageIndex * Me.Gvtemplate.PageSize) + 1
                CType(e.Row.FindControl("ImgCopy"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImgCopy"), ImageButton).CommandName = "Copy"

                CType(e.Row.FindControl("ImgSave"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImgSave"), ImageButton).CommandName = "Save"

                CType(e.Row.FindControl("ImgEdit"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImgEdit"), ImageButton).CommandName = "Edit"

                CType(e.Row.FindControl("ImgCancel"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImgCancel"), ImageButton).CommandName = "Cancel"

                CType(e.Row.FindControl("ImgCopy"), ImageButton).Visible = True
                CType(e.Row.FindControl("ImgSave"), ImageButton).Visible = False
                CType(e.Row.FindControl("ImgCancel"), ImageButton).Visible = False

                CType(e.Row.FindControl("txtNewName"), TextBox).Visible = False

            End If
        End If
    End Sub

    Protected Sub Gvtemplate_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim index As Integer = e.CommandArgument
        Dim estr As String = String.Empty
        Dim UserId As String = Me.Session(S_UserID)
        Dim dtNew As New DataTable
        Dim dsNew As New DataSet
        Dim dr As DataRow

        If e.CommandName.ToUpper = "COPY" Then

            CType(Me.Gvtemplate.Rows(index).FindControl("ImgCopy"), ImageButton).Visible = False
            CType(Me.Gvtemplate.Rows(index).FindControl("ImgSave"), ImageButton).Visible = True
            CType(Me.Gvtemplate.Rows(index).FindControl("ImgCancel"), ImageButton).Visible = True
            CType(Me.Gvtemplate.Rows(index).FindControl("txtNewName"), TextBox).Visible = True
            CType(Me.Gvtemplate.Rows(index).FindControl("txtNewName"), TextBox).Text = ""

        ElseIf e.CommandName.ToUpper = "SAVE" Then

            If CType(Me.Gvtemplate.Rows(index).FindControl("txtNewName"), TextBox).Text.Trim = "" Then
                Me.objcommon.ShowAlert("Please Enter Name of Template", Me.Page)
                Exit Sub
            End If

            If Not ValidataTemplate("COPY", CType(Me.Gvtemplate.Rows(index).FindControl("txtNewName"), TextBox).Text.Trim) Then
                Exit Sub
            End If

            createtable(dtNew)
            dtNew.Clear()
            dr = dtNew.NewRow()
            dr("vTemplateId") = Me.Gvtemplate.Rows(index).Cells(GVC_Id).Text.Trim()
            dr("vTemplateDesc") = CType(Me.Gvtemplate.Rows(index).FindControl("txtNewName"), TextBox).Text.Trim()
            dr("iModifyBy") = Me.Session(S_UserID)
            dtNew.Rows.Add(dr)
            dtNew.AcceptChanges()
            dsNew.Tables.Add(dtNew.Copy)

            If Not Me.objLambda.Proc_Copy_Template(dsNew, estr) Then

                Me.objcommon.ShowAlert("Error While Copying", Me.Page)
                Exit Sub

            End If

            ResetPage()
            BindGrid()
            CType(Me.Gvtemplate.Rows(index).FindControl("ImgCopy"), ImageButton).Visible = True
            CType(Me.Gvtemplate.Rows(index).FindControl("ImgSave"), ImageButton).Visible = False
            CType(Me.Gvtemplate.Rows(index).FindControl("ImgCancel"), ImageButton).Visible = False

            CType(Me.Gvtemplate.Rows(index).FindControl("txtNewName"), TextBox).Text = ""
            CType(Me.Gvtemplate.Rows(index).FindControl("txtNewName"), TextBox).Visible = False




        ElseIf e.CommandName.ToUpper = "CANCEL" Then

            CType(Me.Gvtemplate.Rows(index).FindControl("ImgCopy"), ImageButton).Visible = True
            CType(Me.Gvtemplate.Rows(index).FindControl("ImgSave"), ImageButton).Visible = False
            CType(Me.Gvtemplate.Rows(index).FindControl("ImgCancel"), ImageButton).Visible = False

            CType(Me.Gvtemplate.Rows(index).FindControl("txtNewName"), TextBox).Text = ""
            CType(Me.Gvtemplate.Rows(index).FindControl("txtNewName"), TextBox).Visible = False

        ElseIf e.CommandName.ToUpper = "EDIT" Then

            Me.Response.Redirect("frmAddTemplateMst.aspx?mode=2&value=" & Me.Gvtemplate.Rows(index).Cells(GVC_Id).Text.Trim())

        End If

    End Sub

    Protected Sub Gvtemplate_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs)
        e.Cancel = True
    End Sub

    Protected Sub Gvtemplate_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs)
        e.Cancel = True
    End Sub

    Protected Sub Gvtemplate_PreRender(sender As Object, e As EventArgs) Handles Gvtemplate.PreRender
        If Gvtemplate.Rows.Count > 0 Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIGvtemplate", "UIGvtemplate(); ", True)
        End If
    End Sub

#End Region

#Region "createtable"
    Private Function createtable(ByRef dtNew As DataTable) As Boolean
        'Dim dt As New DataTable
        Dim dc As New DataColumn
        Try

            dc = New DataColumn
            dc.ColumnName = "vTemplateId"
            dc.DataType = GetType(String)
            dtNew.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "vTemplateDesc"
            dc.DataType = GetType(String)
            dtNew.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "iModifyBy"
            dc.DataType = GetType(String)
            dtNew.Columns.Add(dc)

            dtNew.AcceptChanges()
            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....createtable")
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

 



End Class

