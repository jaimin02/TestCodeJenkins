Partial Class frmDocTypeTemplateMst
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objcommon As New clsCommon
    Private objhelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()
    Private ObjPath As New ClsFolderPath

    Private eStr As String = ""

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtDocTypeTemplateMst As String = "DtDocTypeTemplateMst"
    Private Const VS_DtBlankDocTypeTemplateMst As String = "DtBlankDocTypeTemplateMst"
    Private Const VS_DtEditDocTypeTemplateMst As String = "DtEditDocTypeTemplateMst"
    Private Const VS_DtAddDocTypeTemplateMst As String = "DtAddDocTypeTemplateMst"
    Private Const VS_DocTemplateId As String = "DocTemplateId"
    Private Const VS_DocEditTemplateId As String = "DocEditTemplateId"
    Private Const VS_TNo As String = "TranId"
    Private Const Vs_VersionNo As String = "VersionNo"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_DocTemplateId As Integer = 1
    Private Const GVC_TemplateName As Integer = 2
    Private Const GVC_VersionNo As Integer = 3
    Private Const GVC_MedExGroupDesc As Integer = 4
    Private Const GVC_User As Integer = 5
    Private Const GVC_Date As Integer = 6

#End Region
    
#Region "Page Load "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                GenCall()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideDocTypeTempDetails", "HideDocTypeTempDetails(); ", True)
            End If
            If gvDocTypeTemplate.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvDocTypeTemplate", "UIgvDocTypeTemplate(); ", True)
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

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Choice = Me.Request.QueryString("Mode")
            Me.ViewState(VS_Choice) = Choice   'To use it while saving
            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_DocTemplateId) = Me.Request.QueryString("Value").ToString
            End If
            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If
            Me.ViewState(VS_DtDocTypeTemplateMst) = ds.Tables("VIEW_DocTypeTemplateMst")   ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If
            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideSponsorDetails", "HideSponsorDetails(); ", True)
            End If
            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_Data "

    Private Function GenCall_Data(ByRef ds_DWR_Retu As DataSet) As Boolean

        Dim wStr As String = ""
        Dim eStr_Retu As String = ""
        Dim Val As String = ""
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Val = Me.ViewState(GVC_DocTemplateId) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vDocTemplateId=" + Val.ToString
            End If

            If Not objhelp.GetDocTypeTemplateMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
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
            Me.ShowErrorMessage(ex.Message, "")

        Finally

        End Try
    End Function

#End Region

#Region "GenCall_showUI "

    Private Function GenCall_ShowUI() As Boolean
        Dim dt_OpMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Try

            Page.Title = " ::  Document Type TemplateMaster ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Doc Type Template Master"

            dt_OpMst = Me.ViewState(VS_DtDocTypeTemplateMst)

            Choice = Me.ViewState(VS_Choice)

            Me.BtnGo.Attributes.Add("OnClick", "return Validation();")
            FillDropdown()
            BindGrid()

            If Not FillDropdown() Then
                Exit Function
            End If

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

            End If

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function

#End Region

#Region "Fill DropDown"

    Private Function FillDropdown() As Boolean
        Dim eStr_Retu As String = ""
        Dim Ds_FillDocType As New DataSet
        Dim Ds_FillLocation As New DataSet
        Dim Ds_FillDept As New DataSet
        Dim Dv_FillDocType As New DataView
        Dim Dv_FillLocation As New DataView
        Dim Dv_FillDept As New DataView
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If objhelp.GetDoctypeMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, Ds_FillDocType, eStr_Retu) Then
                Dv_FillDocType = Ds_FillDocType.Tables(0).DefaultView
                Dv_FillDocType.Sort = "vDocTypeName"

                Me.ddlDocType.DataSource = Dv_FillDocType
                Me.ddlDocType.DataValueField = "vDocTypeCode"
                Me.ddlDocType.DataTextField = "vDocTypeName"
                Me.ddlDocType.DataBind()
                Me.ddlDocType.Items.Insert(0, "--Select Document Type--")
            End If

            If objhelp.getLocationMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, Ds_FillLocation, eStr_Retu) Then
                Dv_FillLocation = Ds_FillLocation.Tables(0).DefaultView
                Dv_FillLocation.Sort = "vLocationName"

                Me.ddlLocation.DataSource = Dv_FillLocation
                Me.ddlLocation.DataValueField = "vLocationCode"
                Me.ddlLocation.DataTextField = "vLocationName"
                Me.ddlLocation.DataBind()
                Me.ddlLocation.Items.Insert(0, "--Select Location--")
            End If

            If objhelp.GetFieldsOfTable("deptmst", "*", "", Ds_FillDept, eStr_Retu) Then
                Dv_FillDept = Ds_FillDept.Tables(0).DefaultView()
                Dv_FillDept.Sort = "vDeptName"

                Me.ddlDepartment.DataSource = Dv_FillDept
                Me.ddlDepartment.DataValueField = "vDeptCode"
                Me.ddlDepartment.DataTextField = "vDeptName"
                Me.ddlDepartment.DataBind()
                Me.ddlDepartment.Items.Insert(0, "--Select Department--")
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr)
        End Try

    End Function

#End Region

#Region "Button Events"

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.btnadd.Enabled = True
        Dim wStr As String = ""
        Dim estr_Retu As String = ""
        Dim filepath As String = ""
        Dim DocId As String = ""
        Dim ds As New DataSet
        Dim ds_save As New DataSet
        Dim dt_save As New DataSet
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                AssignValues("Edit")
            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.GenCall_Data(ds)
                Me.ViewState(VS_DtDocTypeTemplateMst) = ds.Tables("VIEW_DocTypeTemplateMst")   ' adding blank DataTable in viewstate
                AssignValues("Add")
            End If

            ds_save = New DataSet
            ds_save.Tables.Add(CType(Me.ViewState(VS_DtDocTypeTemplateMst), Data.DataTable).Copy())
            ds_save.Tables(0).TableName = "VIEW_DocTypeTemplateMst"

            If Not objLambda.Save_DocTypeTemplateMst(Me.ViewState(VS_Choice), ds_save, Me.Session(S_UserID), estr_Retu, DocId) Then

                objcommon.ShowAlert("Error While Saving Doc Type Template Mst", Me.Page)
                Exit Sub

            End If

            objcommon.ShowAlert("Doc Type Template Saved SuccessFully !", Me.Page)
            SaveFile(DocId)
            BindGrid()

        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr)
        End Try
    End Sub

    Protected Sub btnadd_Click1(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim eStr As String = ""
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Show", "ShowElement('" + CType(sender, Button).ClientID + "','" + divAdd.ClientID + "');", True)
            'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "ShowPnl", "ShowElement('" + CType(sender, Button).ClientID + "','" + pnldivAdd.ClientID + "');", True)

        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr)
        End Try
    End Sub

    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Resetpage()
    End Sub

    Protected Sub btnExit_Click1(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

#End Region

#Region "Assign Values"

    Private Sub AssignValues(ByVal type As String)
        Dim dr As DataRow
        Dim eStr As String = ""
        Dim dt_User As New DataTable
        Dim ds_Save As New DataSet
        Dim estr_Retu As String = ""
        Dim Filename As String = ""
        Dim DocId As String = ""
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If type.ToUpper = "ADD" Then

                dt_User = CType(Me.ViewState(VS_DtDocTypeTemplateMst), DataTable)
                dt_User.Clear()
                dr = dt_User.NewRow()
                dr("vDocTemplateId") = "00000"
                dr("vDocTypeCode") = Me.ddlDocType.SelectedItem.Value.Trim
                dr("vVersionNo") = "" 'Me.txtdivVersionNo.Text.Trim
                dr("vDocTemplatePath") = ""
                dr("vDocTemplateName") = Me.txtDocTemplateName.Text
                dr("vLocationId") = Me.ddlLocation.SelectedItem.Value
                dr("vDeptId") = Me.ddlDepartment.SelectedItem.Value
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dt_User.Rows.Add(dr)

            ElseIf type.ToUpper = "EDIT" Then

                dt_User = CType(Me.ViewState(VS_DtDocTypeTemplateMst), DataTable)

                For Each dr In dt_User.Rows
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "E"
                    dr.AcceptChanges()
                Next

            ElseIf type.ToUpper = "DELETE" Then
                dt_User = CType(Me.ViewState(VS_DtDocTypeTemplateMst), DataTable)

                For Each dr In dt_User.Rows
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "D"
                    dr.AcceptChanges()
                Next

                dt_User.TableName = "VIEW_DocTypeTemplateMst"
                ds_Save.Tables.Add(dt_User.Copy())
            End If
            Me.ViewState(VS_DtDocTypeTemplateMst) = dt_User

        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr)

        End Try
    End Sub

#End Region

#Region "BindGrid"

    Private Sub BindGrid()
        Dim dsDocTemplate As New DataSet
        Dim eStr As String = ""

        If objhelp.GetDocTypeTemplateMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, dsDocTemplate, eStr) Then
            gvDocTypeTemplate.ShowFooter = False
            gvDocTypeTemplate.DataSource = dsDocTemplate
            gvDocTypeTemplate.DataBind()
            'If gvDocTypeTemplate.Rows.Count > 0 Then
            '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvDocTypeTemplate", "UIgvDocTypeTemplate(); ", True)
            'End If
            gvDocTypeTemplate.Dispose()
        End If

    End Sub

#End Region

#Region "Grid View Event"

    Protected Sub lnlEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim eStr As String = ""
        Dim gridViewRow As GridViewRow = CType(CType(sender, ImageButton).Parent.Parent, GridViewRow)
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Me.ViewState(VS_DocEditTemplateId) = gridViewRow.Cells(GVC_DocTemplateId).Text.Trim
            Me.lblEditVersion.Text = Val(gridViewRow.Cells(GVC_VersionNo).Text.Trim) + 1
            Me.txtEditTemplateName.Text = gridViewRow.Cells(GVC_TemplateName).Text.Trim
            Me.HdfFileName.Value = CType(gridViewRow.FindControl("lnkFolderPath"), HyperLink).Text

            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Show", "ShowElement('" + CType(sender, ImageButton).ClientID + "','" + DivEdit.ClientID + "');", True)
            'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "ShowPnl", "ShowElement('" + CType(sender, LinkButton).ClientID + "','" + pnlEdit.ClientID + "');", True)

        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr)
        End Try
    End Sub

    Protected Sub gvDocTypeTemplate_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)

    End Sub

    Protected Sub gvDocTypeTemplate_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        Dim lblsrNo As Label
        Dim hlnk As HyperLink

        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandName = "EDIT"

        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVC_DocTemplateId).Visible = False
        ElseIf e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(GVC_DocTemplateId).Visible = False
        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(GVC_DocTemplateId).Visible = False
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then

            lblsrNo = CType(e.Row.FindControl("lblSrNo"), Label)
            lblsrNo.Text = e.Row.RowIndex + 1
            hlnk = CType(e.Row.FindControl("lnkFolderPath"), HyperLink)
            hlnk.Text = Path.GetFileName(hlnk.Text)
        End If
    End Sub

    Protected Sub gvDocTypeTemplate_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs)

    End Sub

    Protected Sub gvDocTypeTemplate_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs)
        e.Cancel = True
    End Sub

    Protected Sub gvDocTypeTemplate_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        gvDocTypeTemplate.PageIndex = e.NewPageIndex
        BindGrid()
    End Sub

#End Region

#Region "AddDiv Button Events"

    Protected Sub btnDivAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim wStr As String = ""
        Dim estr_Retu As String = ""
        Dim filepath As String = ""
        Dim ds As New DataSet
        Dim Ds_AddSave As New DataSet
        Dim dt_AddSave As New DataTable
        Dim Ds_AddDivSave As New DataSet
        Dim AdFilename As String = ""
        Dim DocumentTemplate As String = ""
        Me.ViewState(VS_Choice) = 2

        If Not Upload("NotCreate") Then 'To create Folder and Upload File
            Exit Sub
        End If
        'If Not AssignValues("Add") Then
        '    Exit Sub
        'End If
        Try
            wStr = "vDocTypeCode='" & Me.ddlDocType.SelectedItem.Value & "' and vLocationId='" + Me.ddlLocation.SelectedItem.Value & "'and vDeptId='" + Me.ddlDepartment.SelectedItem.Value & "'"
            If objhelp.GetDocTypeTemplateMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_AddSave, estr_Retu) Then

                dt_AddSave = Ds_AddSave.Tables(0)
            End If
            Dim dr As DataRow = dt_AddSave.Rows(0)
            dr("vVersionNo") = Me.txtdivVersionNo.Text.Trim
            dr("vDocTemplatePath") = Me.txtdivTemplateName.Text.Trim
            dr("vDocTemplateName") = Me.txtdivTemplateName.Text.Trim

            If Not (AdFlUpload.PostedFile Is Nothing) Then
                Dim strLongFilePath As String = AdFlUpload.PostedFile.FileName
                Dim intFileNameLength As Integer = InStr(1, StrReverse(strLongFilePath), "\")
                Dim strFileName As String = Mid(strLongFilePath, (Len(strLongFilePath) - intFileNameLength) + 2)
                dr("vDocTemplatePath") = AdFlUpload.PostedFile.FileName
            Else
                objcommon.ShowAlert("pls upload One File", Me.Page)
                Exit Sub
            End If
            NewTranNo("Doc")
            Dim c As String = System.IO.Path.GetFileName(AdFlUpload.PostedFile.FileName)
            'Save uploaded file to server at C:\ServerFolder\
            AdFlUpload.PostedFile.SaveAs(Server.MapPath("\Web_Lambda\DocumentTypeTemplate\" & c))
            dr.AcceptChanges()
            dt_AddSave.AcceptChanges()
            Me.ViewState(VS_DtAddDocTypeTemplateMst) = dt_AddSave

            Ds_AddDivSave = New DataSet
            Ds_AddDivSave.Tables.Add(CType(Me.ViewState(VS_DtAddDocTypeTemplateMst), Data.DataTable).Copy())
            Ds_AddDivSave.Tables(0).TableName = "VIEW_DocTypeTemplateMst"

            'If Not objLambda.Save_DocTypeTemplateMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_DocTypeTemplateMst, Ds_AddDivSave, Me.Session(S_UserID), estr_Retu) Then
            '    objcommon.ShowAlert("Error While Add in Doc Type Template Master", Me.Page)
            '    Exit Sub
            'Else
            '    objcommon.ShowAlert("Doc Type Template Added SuccessFully", Me.Page)
            '    BindGrid()
            'End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr)
        End Try
    End Sub

    Protected Sub btnDivclose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    End Sub

#End Region

#Region "EditDiv Button Events"

    Protected Sub btnDivupdate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Wstr As String = ""
        Dim eStr_Retu As String = ""
        Dim Ds_EditDiv As New DataSet
        Dim Dt_EditDiv As New DataTable
        Dim ds_Updatesave As New DataSet
        Dim DocId As String = ""
        Me.ViewState(VS_Choice) = 2
        Try
            Wstr = "vDocTemplateId='" & Me.ViewState(VS_DocEditTemplateId) & "'"
            If objhelp.GetDocTypeTemplateMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_EditDiv, eStr_Retu) Then
                Dt_EditDiv = Ds_EditDiv.Tables(0)
            End If
            Dim dr As DataRow = Dt_EditDiv.Rows(0)
            'If Not (AdUpload2.PostedFile Is Nothing) Then
            '    dr("vDocTemplatePath") = Me.AdUpload2.PostedFile.FileName
            'Else
            '    objcommon.ShowAlert("pls upload One File", Me.Page)
            '    Exit Sub
            'End If

            DocId = Me.ViewState(VS_DocEditTemplateId)
            Me.ViewState(VS_DocTemplateId) = DocId
            Me.ViewState(VS_TNo) = Dt_EditDiv.Rows(Dt_EditDiv.Rows.Count - 1).Item("iTranNo") + 1

            Upload("EDITDOC")
            dr("vDocTemplateName") = Me.txtEditTemplateName.Text.Trim()
            dr("vDocTemplatePath") = Me.HdfFileName.Value.Trim()
            dr.AcceptChanges()
            Dt_EditDiv.AcceptChanges()

            Me.ViewState(VS_DtEditDocTypeTemplateMst) = Dt_EditDiv

            ds_Updatesave = New DataSet
            ds_Updatesave.Tables.Add(CType(Me.ViewState(VS_DtEditDocTypeTemplateMst), Data.DataTable).Copy())
            ds_Updatesave.Tables(0).TableName = "VIEW_DocTypeTemplateMst"
            If Not objLambda.Save_DocTypeTemplateMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Updatesave, Me.Session(S_UserID), eStr_Retu, DocId) Then
                objcommon.ShowAlert("Error While Editing in Doc Type Template Master", Me.Page)
                Exit Sub
            Else
                objcommon.ShowAlert("Doc Type Template Updated SuccessFully !", Me.Page)
                BindGrid()
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
                Me.btnadd.Enabled = False
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr)
        End Try

    End Sub

    Protected Sub btnEditclose_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub

#End Region

#Region "Button Events"


#End Region

#Region "Reset Page"
    Private Sub Resetpage()
        Me.ddlDepartment.SelectedIndex = -1
        Me.ddlDocType.SelectedIndex = -1
        Me.ddlLocation.SelectedIndex = -1
        Me.txtDocTemplateName.Text = ""
    End Sub
#End Region

#Region "New TranNo"

    Private Function NewTranNo(ByVal type As String) As Boolean
        Dim DocTempId As String = ""
        Dim VsId As String = ""
        Dim TNo As String = ""
        Dim Ds_Tran As New DataSet
        Dim estr As String = ""
        Dim Wstr As String = ""
        Try
            DocTempId = Me.ViewState(VS_DocTemplateId)
            VsId = Me.txtdivVersionNo.Text.Trim 'Me.ViewState(VS_NId)

            Me.ViewState(VS_TNo) = Nothing
            If type.ToUpper = "DOC" Or type.ToUpper = "TEMP" Then
                Wstr = "vDocTemplateId='" & DocTempId + "' and vVersionNo= " & VsId
            ElseIf type.ToUpper = "COM" Then
                Wstr = "vDocTemplateId='" & DocTempId + "' and vVersionNo= " & VsId
            End If
            If Not objhelp.GetFieldsOfTable("DocTypeTemplateMst", "isNull(Max(iTranNo+1),1) as TranNo", Wstr, Ds_Tran, estr) Then
                Return False
            End If
            TNo = Ds_Tran.Tables(0).Rows(0).Item("TranNo")

            Me.ViewState(VS_TNo) = TNo
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region

#Region "UpLoad File"

    Private Sub SaveFile(ByVal DocId As String)
        Dim Ds_Tran As New DataSet
        Dim Ds_Save As New DataSet
        Dim Dt_Save As New DataTable
        Dim dr As DataRow
        Dim estr As String = ""
        If Not Me.objhelp.GetDocTypeTemplateMst("vDocTemplateId='" & DocId & "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_Tran, estr) Then
            Me.objcommon.ShowAlert("", Me.Page)
            Exit Sub
        End If
        Me.ViewState(VS_DocTemplateId) = DocId
        Me.ViewState(VS_TNo) = Ds_Tran.Tables(0).Rows(Ds_Tran.Tables(0).Rows.Count - 1).Item("iTranNo")

        Upload("DOC")

        Dt_Save = CType(Me.ViewState(VS_DtDocTypeTemplateMst), DataTable)

        For Each dr In Dt_Save.Rows
            dr("vDocTemplateId") = DocId
            dr("vDocTemplatePath") = Me.HdfFileName.Value
            dr("iTranNo") = Me.ViewState(VS_TNo)
            dr("vVersionNo") = Ds_Tran.Tables(0).Rows(Ds_Tran.Tables(0).Rows.Count - 1).Item("vVersionNo")
            dr.AcceptChanges()
        Next

        Dt_Save.AcceptChanges()
        Ds_Save.Tables.Add(Dt_Save.Copy())

        If Not objLambda.Save_DocTypeTemplateMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, Ds_Save, Me.Session(S_UserID), estr, DocId) Then
            objcommon.ShowAlert("Error While Saving Doc Type Template Mst", Me.Page)
            Exit Sub
        End If
    End Sub

    Private Function Upload(ByVal Type As String) As Boolean
        Dim DocPath As String = "" 'Me.Session("Path")
        Dim DocValidFile As String = ""
        Dim NewFilePath As String = ""
        DocValidFile = System.Configuration.ConfigurationManager.AppSettings("Validity")
        Dim bl As Boolean = False
        Try

            If Not IsNothing(Me.Session("FileName")) Then
                Me.Session("FileName") = Nothing
            End If

            'If Not NewTranNo("DOC") Then
            '    objcommon.ShowAlert("Error Occured while Getting Tran No., Try Again... ", Me)
            '    Return False
            'End If

            If Not CreateFolder("DOC") Then
                objcommon.ShowAlert("Error Occured while Creating Folder, Try Again... ", Me)
                'Exit Function
                Return False
            End If

            DocPath = Me.HdfBaseFolder.Value.Trim() + Me.HdfFolder.Value.Trim() + "/"
            If Type = "EDITDOC" Then
                If Not IsNothing(Me.AdUpload2.PostedFile) Then
                    Me.AdUpload2.PostedFile.SaveAs(Server.MapPath(DocPath) & Path.GetFileName(Me.AdUpload2.PostedFile.FileName))
                    Me.HdfFileName.Value = DocPath & Path.GetFileName(Me.AdUpload2.PostedFile.FileName)
                End If
            Else
                Me.flDocument.PostedFile.SaveAs(Server.MapPath(DocPath) & Path.GetFileName(Me.flDocument.PostedFile.FileName))
                Me.HdfFileName.Value = DocPath & Path.GetFileName(Me.flDocument.PostedFile.FileName)
            End If

            Return True

        Catch ex As Exception
            objcommon.ShowAlert("Error Occured while Uploading File, Try Again... ", Me)
            Return False
        End Try
    End Function
#End Region

#Region "Create New Folder"
    Private Function CreateFolder(ByVal type As String) As Boolean
        Dim DocTempId As String = ""
        Dim DCId As String = ""
        Dim VsId As String = ""
        Dim NId As String = ""
        Dim TNo As String = ""
        Dim ActId As String = ""
        Dim DocTemplateDetail As String = ""
        Dim dir As DirectoryInfo
        Dim Ds_Tran As New DataSet
        Dim estr As String = ""
        Dim DocId As String = ""
        'Dim tranNo As String = ""
        Try


            'DocTempId = 1
            'TNo = Ds_Tran.Tables(0).Rows(Ds_Tran.Tables(0).Rows.Count).Item("iTranNo")

            'VsId = Me.txtdivVersionNo.Text.Trim 'Me.ViewState(VS_VersionNo)
            Me.HdfBaseFolder.Value = "DocTypeTemplateDetail/"
            DocTemplateDetail = "DocTypeTemplateDetail/"
            DocTempId = Me.ViewState(VS_DocTemplateId)
            TNo = Me.ViewState(VS_TNo)

            'For New TranNo

            Me.HdfFolder.Value = ""
            Me.HdfFolder.Value = DocTempId & "/" & TNo

            dir = New DirectoryInfo(ObjPath.Param_FolderPath(DocTemplateDetail & DocTempId & "/" & TNo))
            If Not dir.Exists() Then
                dir.Create()
            End If
            Return True
        Catch ex As Exception
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
