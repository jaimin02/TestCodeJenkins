Imports Microsoft.Win32
Imports System.Web.Services
Imports Newtonsoft.Json
Imports Microsoft
Imports Microsoft.Office.Interop
Imports System.IO
Imports System.Drawing

Partial Class frmMedExGroupMst
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objcommon As New clsCommon
    Private objhelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtMedexGroupMst As String = "DtMedExGroupMst"
    Private Const VS_DtBlankMedExGroupMst As String = "DtBlankMedExGroupMst"
    Private Const VS_MedexGroupCode As String = "MedexGroupCode"

    Private eStr As String = ""

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_MedexGroupCode As Integer = 1
    Private Const GVC_MedExGroupDesc As Integer = 2
    Private Const GVC_CDISCValue As Integer = 3
    Private Const GVC_OtherValue As Integer = 4

#End Region

#Region "Page Load "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try


            If Not IsPostBack Then
                GenCall()
            End If

            Me.AutoCompleteExtenderMedexGroup.ContextKey = " vProjectTypeCode in (" + Me.Session(S_ScopeValue) + ")"

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...Page_Load")
        End Try

        Try
            If gvmedexgrp.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvmedexgrp", "UIgvmedexgrp(); ", True)
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
            Choice = Me.Request.QueryString("Mode")
            Me.ViewState(VS_Choice) = Choice   'To use it while saving

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_MedexGroupCode) = Me.Request.QueryString("Value").ToString
            End If

            Me.ViewState(VS_DtMedexGroupMst) = ds.Tables("MedExGroupMst")   ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If
            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideAttributeGroupDetails", "HideAttributeGroupDetails(); ", True)
            End If
            GenCall = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall")
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



            Val = Me.ViewState(VS_MedexGroupCode) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vMedExGroupCode=" + Val.ToString
            End If

            If Not objhelp.GetMedExGroupMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
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
            Me.ShowErrorMessage(ex.Message, "...GenCall_Data")

        Finally

        End Try
    End Function

#End Region

#Region "GenCall_showUI "

    Private Function GenCall_ShowUI() As Boolean
        Dim dt_OpMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Try
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Attribute Group Master"

            BindGrid()

            Page.Title = ":: Attribute Group Master :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            If Not FillDropDown() Then
                Exit Function
            End If

            dt_OpMst = Me.ViewState(VS_DtMedexGroupMst)

            Choice = Me.ViewState(VS_Choice)

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_ShowUI")
        Finally
        End Try
    End Function

#End Region

#Region "FillDropDown"

    Private Function FillDropDown() As Boolean
        Dim ds_ProjectType As New Data.DataSet
        Dim dv_ProjectType As New Data.DataView
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty
        Try


            'To Get Where condition of ScopeVales( Project Type )
            If Not objcommon.GetScopeValueWithCondition(Wstr) Then
                Return False
            End If

            If Not objhelp.GetviewProjectTypeMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                ds_ProjectType, estr) Then
                Return False
            End If

            dv_ProjectType = ds_ProjectType.Tables(0).DefaultView.ToTable(True, "vProjectTypeCode,vProjectTypeName".Split(",")).DefaultView
            dv_ProjectType.Sort = "vProjectTypeName"
            Me.ddlProjectType.DataSource = dv_ProjectType
            Me.ddlProjectType.DataValueField = "vProjectTypeCode"
            Me.ddlProjectType.DataTextField = "vProjectTypeName"

            Me.ddlProjectType.DataBind()
            Me.ddlProjectType.Items.Insert(0, New ListItem("Select Project Type", ""))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillDropDown")
            Return False
        End Try
    End Function

#End Region

#Region "Button Events"

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Response.Redirect("frmMainpage.aspx")
    End Sub

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds As New DataSet
        Dim ds_save As New DataSet
        Dim dt_save As New DataSet
        Dim eStr_Retu As String = String.Empty
        Dim message As String = String.Empty
        Try



            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                If Not AssignValues("Edit") Then
                    Exit Sub
                End If

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                Me.GenCall_Data(ds)
                Me.ViewState(VS_DtMedexGroupMst) = ds.Tables("MedExGroupMst")   ' adding blank DataTable in viewstate

                If Not AssignValues("Add") Then
                    Exit Sub
                End If

            End If

            ds_save = New DataSet
            ds_save.Tables.Add(CType(Me.ViewState(VS_DtMedexGroupMst), Data.DataTable).Copy())
            ds_save.Tables(0).TableName = "MedExGroupMst"


            If Not objLambda.Save_MedExGroupMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_MedExGroupMst, ds_save, Me.Session(S_UserID), eStr_Retu) Then

                objcommon.ShowAlert("Error While Saving Attribute Group Master", Me.Page)
                Exit Sub

            End If
            message = IIf(Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "Attribute Group Saved Successfully !", "Attribute Group Updated Successfully !")

            objcommon.ShowAlert(message, Me.Page)
            Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
            Me.BtnExit.Visible = True
            Me.btncancel.Visible = False
            Resetpage()
            BindGrid()

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".......BtnSave_Click")
        End Try
    End Sub

    Protected Sub btnupdate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        '    Dim Ds_Editsave As New DataSet
        '    Dim eStr As String = String.Empty

        '    Try

        '        
        '        If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

        '            If Not AssignValues("Edit") Then
        '                Exit Sub
        '            End If

        '        End If

        '        Ds_Editsave = New DataSet
        '        Ds_Editsave.Tables.Add(CType(Me.ViewState(VS_DtMedexGroupMst), Data.DataTable).Copy())
        '        Ds_Editsave.Tables(0).TableName = "MedExGroupMst"

        '        If Not objLambda.Save_MedExGroupMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_MedExGroupMst, Ds_Editsave, Me.Session(S_UserID), eStr) Then

        '            objcommon.ShowAlert("Error While Saving Attribute Group Master", Me.Page)
        '            Exit Sub

        '        End If

        '        objcommon.ShowAlert("Attribute Group Updated Successfully", Me.Page)
        '        Resetpage()
        '        BindGrid()
        '        Me.BtnSave.Visible = True
        '        Me.BtnExit.Visible = True
        '        Me.btnupdate.Visible = False
        '        Me.btncancel.Visible = False
        '        Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add

        '    Catch ex As Exception
        '        ShowErrorMessage(ex.Message, "...btnupdate_Click")
        '    End Try

    End Sub

    Protected Sub btncancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Resetpage()
        'Me.BtnSave.Visible = True
        Me.BtnExit.Visible = True
        'Me.btnupdate.Visible = False
        Me.btncancel.Visible = False
        Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
        'Me.Response.Redirect("frmMedExGroupMst.aspx?mode=1")
    End Sub

#End Region

#Region "Assign Values"

    Private Function AssignValues(ByVal type As String) As Boolean
        Dim dr As DataRow
        Dim dt_User As New DataTable
        Dim ds_Save As New DataSet
        Dim estr_Retu As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim ds_Check As New DataSet
        Dim ds_Valid As New DataSet
        Try



            dt_User = CType(Me.ViewState(VS_DtMedexGroupMst), DataTable)

            Wstr = "cStatusIndi <> 'D' And vMedExGroupDesc ='" & Me.txtmedexdesc.Text.Trim() & "' And vProjectTypeCode In (" & Me.Session(S_ScopeValue) & ") And vProjectTypeCode = '" & Me.ddlProjectType.SelectedValue.ToString() & "' "

            If type.ToUpper = "EDIT" Then
                Wstr += " And vMedExGroupCode <> '" & dt_User.Rows(0).Item("vMedExGroupCode").Trim() & "'"
            End If

            'For validating Duplication
            If Not objhelp.GetMedExGroupMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                ds_Check, estr_Retu) Then

                Me.ShowErrorMessage("Error While Getting Data From Attribute Group Master", estr_Retu)
                Return False

            End If

            If ds_Check.Tables(0).Rows.Count > 0 Then

                objcommon.ShowAlert("Attribute Group Name Already Exists !", Me.Page)

                If gvmedexgrp.Rows.Count > 0 Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvmedexgrp", "UIgvmedexgrp(); ", True)
                End If

                Exit Function

                Return False

            End If
            '***********************************************

            If type.ToUpper = "ADD" Then

                dt_User.Clear()
                dr = dt_User.NewRow()
                dr("vMedExGroupCode") = "0"
                dr("vMedExGroupDesc") = Me.txtmedexdesc.Text.Trim
                dr("vProjectTypeCode") = Me.ddlProjectType.SelectedValue.Trim()
                dr("cActiveFlag") = GeneralModule.ActiveFlag_Yes 'IIf(Me.chkactive.Checked = True, "Y", "N")\\Changed By Mihir 9-9-2008
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dr("vCDISCValue") = Me.txtCDISCValue.Text.Trim()
                dr("vOtherValues") = Me.txtOtherValue.Text.Trim()
                dr("vRemarks") = Me.txtRemark.Text.Trim()
                dt_User.Rows.Add(dr)

            ElseIf type.ToUpper = "EDIT" Then

                For Each dr In dt_User.Rows

                    'dr("vMedExCode") = "0"
                    dr("vMedExGroupDesc") = Me.txtmedexdesc.Text.Trim
                    dr("vProjectTypeCode") = Me.ddlProjectType.SelectedValue.Trim()
                    dr("cActiveFlag") = GeneralModule.ActiveFlag_Yes 'IIf(Me.chkactive.Checked = True, "Y", "N") \\Changed By Mihir 9-9-2008
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "E"
                    dr("vCDISCValue") = Me.txtCDISCValue.Text.Trim()
                    dr("vOtherValues") = Me.txtOtherValue.Text.Trim()
                    dr("vRemarks") = Me.txtRemark.Text.Trim()
                    dr.AcceptChanges()

                Next dr

            ElseIf type.ToUpper = "DELETE" Then

                For Each dr In dt_User.Rows

                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cActiveFlag") = GeneralModule.ActiveFlag_No
                    dr("cStatusIndi") = "D"
                    dr.AcceptChanges()

                Next dr

                dt_User.TableName = "MedExGroupMst"
                ds_Save.Tables.Add(dt_User.Copy())

                If Not objLambda.Save_MedExGroupMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_MedExGroupMst, ds_Save, Me.Session(S_UserID), estr_Retu) Then

                    objcommon.ShowAlert("Error While Deleteing from Attribute Group", Me.Page)
                    Exit Function

                End If

                objcommon.ShowAlert("Attribute Group Deleted Successfully !", Me.Page)
                BindGrid()
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add

            End If

            Me.ViewState(VS_DtMedexGroupMst) = dt_User

            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....AssignValues")
            Return False
        End Try
    End Function

#End Region

#Region "Reset Page"

    Private Sub Resetpage()
        Me.txtmedexdesc.Text = ""
        Me.chkactive.Checked = False
        Me.txtCDISCValue.Text = ""
        Me.txtOtherValue.Text = ""
        Me.ddlProjectType.SelectedIndex = 0
        Me.txtRemark.Text = ""
        Me.BtnSave.Text = "Save"
        Me.BtnSave.ToolTip = "Save"
    End Sub

#End Region

#Region "GridViewEvents"

    Protected Sub gvmedexgrp_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)

        gvmedexgrp.PageIndex = e.NewPageIndex
        BindGrid()

    End Sub

    Protected Sub gvmedexgrp_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim index As Integer = e.CommandArgument
        Dim ds As New DataSet
        Dim eStr As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim dt As New DataTable
        Try



            If e.CommandName.ToUpper() = "MYEDIT" Or e.CommandName.ToUpper() = "MYDELETE" Then

                Wstr = "vMedExGroupCode='" & Me.gvmedexgrp.Rows(index).Cells(GVC_MedexGroupCode).Text & "'"
                If Not objhelp.GetMedExGroupMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr) Then
                    Response.Write(eStr)
                    Exit Sub
                End If

                Me.ViewState(VS_DtMedexGroupMst) = ds.Tables(0)
                dt = Me.ViewState(VS_DtMedexGroupMst)

            End If

            If e.CommandName.ToUpper = "MYEDIT" Then

                Dim dr As DataRow = dt.Rows(0)
                Me.ddlProjectType.SelectedValue = dr("vProjectTypeCode")
                Me.txtmedexdesc.Text = dr("vMedExGroupDesc").ToString
                Me.chkactive.Checked = IIf(dr("cActiveFlag") = GeneralModule.ActiveFlag_Yes, True, False)
                Me.txtCDISCValue.Text = dr("vCDISCValue").ToString
                Me.txtOtherValue.Text = dr("vOtherValues").ToString
                Me.ViewState(VS_DtMedexGroupMst) = dt
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                Me.BtnSave.Text = "Update"
                Me.BtnSave.ToolTip = "Update"
                Me.BtnExit.Visible = False
                'Me.btnupdate.Visible = True
                Me.btncancel.Visible = True

            ElseIf e.CommandName.ToUpper = "MYDELETE" Then

                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                If Not AssignValues("Delete") Then
                    Exit Sub
                End If

            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....gvmedexgrp_RowCommand")
        End Try
    End Sub

    Protected Sub gvmedexgrp_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.DataRow Then

            CType(e.Row.FindControl("ImbEdit"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("ImbEdit"), ImageButton).CommandName = "MYEDIT"
            CType(e.Row.FindControl("ImbDelete"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("ImbDelete"), ImageButton).CommandName = "MYDELETE"
            e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (gvmedexgrp.PageSize * gvmedexgrp.PageIndex) + 1
            CType(e.Row.FindControl("lnkAudit"), ImageButton).Attributes.Add("vMedexGroupCode", e.Row.Cells(GVC_MedexGroupCode).Text)
        End If

        If e.Row.RowType = DataControlRowType.DataRow Or _
            e.Row.RowType = DataControlRowType.Header Or _
            e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVC_MedexGroupCode).Visible = False

        End If

    End Sub

    Protected Sub gvmedexgrp_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs)

    End Sub

    Protected Sub gvmedexgrp_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs)
        e.Cancel = True
    End Sub

#End Region

#Region "BindGrid"

    Private Sub BindGrid()
        Dim dsMedExGroup As New DataSet
        Dim dvMedExGroup As New DataView
        Dim eStr As String = String.Empty
        'Dim Wstr_Scope As String = ""
        Dim wstr As String = String.Empty
        Try

            'To Get Where condition of ScopeVales( Project Type )
            If Not objcommon.GetScopeValueWithCondition(wstr) Then
                Exit Sub
            End If

            wstr += " And cStatusIndi<>'D'"

            If Me.HMedexGroupId.Value.Trim.Length() > 0 Then
                wstr += " And vMedExGroupCode = '" & Me.HMedexGroupId.Value.Trim.ToString() & "'"
            End If

            wstr += " order by vMedExGroupDesc"

            If objhelp.View_MedexGroupMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsMedExGroup, eStr) Then

                gvmedexgrp.ShowFooter = False
                dvMedExGroup = dsMedExGroup.Tables(0).Copy.DefaultView
                dvMedExGroup.Sort = "vMedExGroupDesc"
                gvmedexgrp.DataSource = dvMedExGroup
                gvmedexgrp.DataBind()
                If gvmedexgrp.Rows.Count > 0 Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvmedexgrp", "UIgvmedexgrp(); ", True)
                End If
                gvmedexgrp.Dispose()

            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...BindGrid")
        End Try
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

    Protected Sub btnSetMedexGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetMedexGroup.Click
        BindGrid()
    End Sub

    Protected Sub BtnViewAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnViewAll.Click
        Me.HMedexGroupId.Value = String.Empty
        Me.txtMedexGroup.Text = String.Empty
        BindGrid()
    End Sub

#Region "Web Method"

    <WebMethod> _
    Public Shared Function AuditTrail(ByVal vMedExGroupCode As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_MedExGroupMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim dtMedExGroupMstHistory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable

        Dim vTableName As String = String.Empty
        Dim vIdName As String = String.Empty
        Dim AuditFieldName As String = String.Empty
        Dim AuditFieldValue As String = String.Empty
        Dim vOtherTableName As String = String.Empty
        Dim vOtherIdName As String = String.Empty

        Try

            vTableName = "MedExGroupMstHistory"
            vIdName = "vProjectTypeCode"
            AuditFieldName = "vMedExGroupCode"
            AuditFieldValue = vMedExGroupCode
            vOtherTableName = "ProjectTypeMst"
            vOtherIdName = "vProjectTypeCode"


            Dim Param As String = ""


            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName

            If Not objHelp.Proc_GetAuditTrail(wStr, ds_MedExGroupMst, Param) Then
                Throw New Exception(estr)
            End If

            If Not dtMedExGroupMstHistory Is Nothing Then
                dtMedExGroupMstHistory.Columns.Add("SrNo")
                dtMedExGroupMstHistory.Columns.Add("MedExGroupDesc")
                dtMedExGroupMstHistory.Columns.Add("CDISCValue")
                dtMedExGroupMstHistory.Columns.Add("OtherValues")
                dtMedExGroupMstHistory.Columns.Add("ProjectTypeName")
                dtMedExGroupMstHistory.Columns.Add("Remark")
                dtMedExGroupMstHistory.Columns.Add("ModifyBy")
                dtMedExGroupMstHistory.Columns.Add("ModifyOn")
            End If

            dt = ds_MedExGroupMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtMedExGroupMstHistory.NewRow()
                drAuditTrail("SrNo") = i
                drAuditTrail("MedExGroupDesc") = dr("vMedExGroupDesc").ToString()
                drAuditTrail("CDISCValue") = dr("vCDISCValue").ToString()
                drAuditTrail("OtherValues") = dr("vOtherValues").ToString()
                drAuditTrail("ProjectTypeName") = dr("vProjectTypeName").ToString()
                drAuditTrail("Remark") = dr("vRemarks").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("ModifyOn"))
                dtMedExGroupMstHistory.Rows.Add(drAuditTrail)
                dtMedExGroupMstHistory.AcceptChanges()
                i += 1
            Next
            strReturn = JsonConvert.SerializeObject(dtMedExGroupMstHistory)
            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Protected Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_MedExGroupMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty

        Dim dtMedExGroupMstHistory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable


        Dim filename As String = String.Empty
        Dim vTableName As String = String.Empty
        Dim vIdName As String = String.Empty
        Dim AuditFieldName As String = String.Empty
        Dim AuditFieldValue As String = String.Empty
        Dim vOtherTableName As String = String.Empty
        Dim vOtherIdName As String = String.Empty
        Dim strMessage As New StringBuilder

        Try

            vTableName = "MedExGroupMstHistory"
            vIdName = "vProjectTypeCode"
            AuditFieldName = "vMedExGroupCode"
            AuditFieldValue = hdnMedExGroupCode.Value
            vOtherTableName = "ProjectTypeMst"
            vOtherIdName = "vProjectTypeCode"


            Dim Param As String = ""

            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName

            If Not objHelp.Proc_GetAuditTrail(wStr, ds_MedExGroupMst, Param) Then
                Throw New Exception(estr)
            End If

            If Not dtMedExGroupMstHistory Is Nothing Then
                dtMedExGroupMstHistory.Columns.Add("Sr. No")
                dtMedExGroupMstHistory.Columns.Add("Attribute Group Desc")
                dtMedExGroupMstHistory.Columns.Add("Variable Name")
                dtMedExGroupMstHistory.Columns.Add("Other Value")
                dtMedExGroupMstHistory.Columns.Add("Project Type Name")
                dtMedExGroupMstHistory.Columns.Add("Remarks")
                dtMedExGroupMstHistory.Columns.Add("ModifyBy")
                dtMedExGroupMstHistory.Columns.Add("ModifyOn")
            End If

            dtMedExGroupMstHistory.AcceptChanges()
            dt = ds_MedExGroupMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtMedExGroupMstHistory.NewRow()
                drAuditTrail("Sr. No") = i
                drAuditTrail("Attribute Group Desc") = dr("vMedExGroupDesc").ToString()
                drAuditTrail("Variable Name") = dr("vCDISCValue").ToString()
                drAuditTrail("Other Value") = dr("vOtherValues").ToString()
                drAuditTrail("Project Type Name") = dr("vProjectTypeName").ToString()
                drAuditTrail("Remarks") = dr("vRemarks").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("ModifyOn"))
                dtMedExGroupMstHistory.Rows.Add(drAuditTrail)
                dtMedExGroupMstHistory.AcceptChanges()
                i += 1
            Next

            gvExport.DataSource = dtMedExGroupMstHistory
            gvExport.DataBind()
            If gvExport.Rows.Count > 0 Then

                gvExport.HeaderRow.BackColor = Color.White
                gvExport.FooterRow.BackColor = Color.White
                For Each cell As TableCell In gvExport.HeaderRow.Cells
                    cell.BackColor = gvExport.HeaderStyle.BackColor
                Next
                For Each row As GridViewRow In gvExport.Rows
                    row.BackColor = Color.White
                    row.Height = 20
                    For Each cell As TableCell In row.Cells
                        If row.RowIndex Mod 2 = 0 Then
                            cell.BackColor = gvExport.AlternatingRowStyle.BackColor
                            cell.ForeColor = gvExport.RowStyle.ForeColor
                        Else
                            cell.BackColor = gvExport.RowStyle.BackColor
                            cell.ForeColor = gvExport.RowStyle.ForeColor
                        End If
                        cell.CssClass = "textmode"
                    Next
                Next

                'style to format numbers to string
                Dim style As String = "<style> .textmode { } </style>"
                Response.Write(style)



                Dim info As String = String.Empty
                Dim gridviewHtml As String = String.Empty

                filename = "Audit Trail_" + hdnMedExGroupCode.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                Dim stringWriter As New System.IO.StringWriter()

                Dim writer As New HtmlTextWriter(stringWriter)
                gvExport.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()

                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""8""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")





                strMessage.Append("<td colspan=""8""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Attribute Group Master-AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td></td><td></td><td Align=""Right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")


                gridviewHtml = strMessage.ToString() + gridviewHtml
                Context.Response.Buffer = True
                Context.Response.ClearContent()
                Context.Response.ClearHeaders()

                Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
                Context.Response.AddHeader("content-Disposition", "attachment; filename=" + filename)
                Context.Response.AddHeader("Content-Length", gridviewHtml.Length)

                Context.Response.Write(gridviewHtml)
                Context.Response.Flush()
                Context.Response.End()

                System.IO.File.Delete(filename)
            Else

                Dim info As String = String.Empty
                Dim gridviewHtml As String = String.Empty

                filename = "Audit Trail_" + hdnMedExGroupCode.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"


                drAuditTrail = dtMedExGroupMstHistory.NewRow()

                drAuditTrail("Sr. No") = ""
                drAuditTrail("Attribute Group Desc") = ""
                drAuditTrail("Variable Name") = ""
                drAuditTrail("Other Value") = ""
                drAuditTrail("Project Type Name") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("ModifyBy") = ""
                drAuditTrail("ModifyOn") = ""
                dtMedExGroupMstHistory.Rows.Add(drAuditTrail)
                dtMedExGroupMstHistory.AcceptChanges()
                gvExport.DataSource = dtMedExGroupMstHistory
                gvExport.DataBind()


                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(stringWriter)
                gvExport.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()


                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""8""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<td colspan=""8""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Attribute Group Master-AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td></td><td></td><td align=""right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")


                gridviewHtml = strMessage.ToString() + gridviewHtml
                Context.Response.Buffer = True
                Context.Response.ClearContent()
                Context.Response.ClearHeaders()

                Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
                Context.Response.AddHeader("content-Disposition", "attachment; filename=" + filename)
                Context.Response.AddHeader("Content-Length", gridviewHtml.Length)

                Context.Response.Write(gridviewHtml)
                Context.Response.Flush()
                Context.Response.End()

                System.IO.File.Delete(filename)
                Exit Sub
            End If
        Catch ex As Exception
            Me.objcommon.ShowAlert("Error While Exporting Data To Excel : " + estr, Me.Page)

            Exit Sub
        End Try
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    End Sub

#End Region

    Protected Sub btnExportToExcelGrid_Click(sender As Object, e As EventArgs) Handles btnExportToExcelGrid.Click
        'Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable
        Dim wStr As String = String.Empty
        Dim ds_MedExGroupMst As New DataSet
        Dim estr As String = String.Empty
        Dim dtMedExGroupMstHistory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable
        Dim MedExGroupCode As String = String.Empty
        Dim filename As String = String.Empty
        Dim strMessage As New StringBuilder

        Try
            ds_MedExGroupMst = objHelp.ProcedureExecute("Proc_MedExGroupMaster ", wStr)
            If Not dtMedExGroupMstHistory Is Nothing Then
                dtMedExGroupMstHistory.Columns.Add("Sr. No")
                dtMedExGroupMstHistory.Columns.Add("Attribute Group Desc")
                dtMedExGroupMstHistory.Columns.Add("Variable Name")
                dtMedExGroupMstHistory.Columns.Add("Other Value")
                dtMedExGroupMstHistory.Columns.Add("Project Type Name")
                dtMedExGroupMstHistory.Columns.Add("Remarks")
                dtMedExGroupMstHistory.Columns.Add("ModifyBy")
                dtMedExGroupMstHistory.Columns.Add("ModifyOn")
            End If

            dtMedExGroupMstHistory.AcceptChanges()
            dt = ds_MedExGroupMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtMedExGroupMstHistory.NewRow()
                drAuditTrail("Sr. No") = i
                drAuditTrail("Attribute Group Desc") = dr("vMedExGroupDesc").ToString()
                drAuditTrail("Variable Name") = dr("vCDISCValue").ToString()
                drAuditTrail("Other Value") = dr("vOtherValues").ToString()
                drAuditTrail("Project Type Name") = dr("vProjectTypeName").ToString()
                drAuditTrail("Remarks") = dr("vRemarks").ToString()
                drAuditTrail("ModifyBy") = dr("iModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("dModifyOn"))
                dtMedExGroupMstHistory.Rows.Add(drAuditTrail)
                dtMedExGroupMstHistory.AcceptChanges()
                i += 1
            Next

            gvExportToExcel.DataSource = dtMedExGroupMstHistory
            gvExportToExcel.DataBind()
            If gvExportToExcel.Rows.Count > 0 Then

                gvExportToExcel.HeaderRow.BackColor = Color.White
                gvExportToExcel.FooterRow.BackColor = Color.White
                For Each cell As TableCell In gvExportToExcel.HeaderRow.Cells
                    cell.BackColor = gvExportToExcel.HeaderStyle.BackColor
                Next
                For Each row As GridViewRow In gvExportToExcel.Rows
                    row.BackColor = Color.White
                    row.Height = 20
                    For Each cell As TableCell In row.Cells
                        If row.RowIndex Mod 2 = 0 Then
                            cell.BackColor = gvExportToExcel.AlternatingRowStyle.BackColor
                            cell.ForeColor = gvExportToExcel.RowStyle.ForeColor
                        Else
                            cell.BackColor = gvExportToExcel.RowStyle.BackColor
                            cell.ForeColor = gvExportToExcel.RowStyle.ForeColor
                        End If
                        cell.CssClass = "textmode"
                    Next
                Next

                'style to format numbers to string
                Dim style As String = "<style> .textmode { } </style>"
                Response.Write(style)



                Dim info As String = String.Empty
                Dim gridviewHtml As String = String.Empty

                filename = "AttributeGroup Master" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                Dim stringWriter As New System.IO.StringWriter()

                Dim writer As New HtmlTextWriter(stringWriter)
                gvExportToExcel.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()
                gridviewHtml = gridviewHtml.Replace("display: none", "display: block")


                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""8""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<td colspan=""8""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Attribute Group Master")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font><td></td><td></td></td><td></td><td></td><td Align=""Right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")


                gridviewHtml = strMessage.ToString() + gridviewHtml
                Context.Response.Buffer = True
                Context.Response.ClearContent()
                Context.Response.ClearHeaders()

                Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
                Context.Response.AddHeader("content-Disposition", "attachment; filename=" + filename)
                Context.Response.AddHeader("Content-Length", gridviewHtml.Length)

                Context.Response.Write(gridviewHtml)
                Context.Response.Flush()
                Context.Response.End()

                System.IO.File.Delete(filename)
            Else

                Dim info As String = String.Empty
                Dim gridviewHtml As String = String.Empty

                filename = "AttributeGroup Master" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                drAuditTrail = dtMedExGroupMstHistory.NewRow()

                drAuditTrail("Sr. No") = ""
                drAuditTrail("Attribute Group Desc") = ""
                drAuditTrail("Variable Name") = ""
                drAuditTrail("Other Value") = ""
                drAuditTrail("Project Type Name") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("ModifyBy") = ""
                drAuditTrail("ModifyOn") = ""
                dtMedExGroupMstHistory.Rows.Add(drAuditTrail)
                dtMedExGroupMstHistory.AcceptChanges()
                gvExportToExcel.DataSource = dtMedExGroupMstHistory
                gvExportToExcel.DataBind()

                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(stringWriter)
                gvExportToExcel.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()

                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""8""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<td colspan=""8""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Attribute Group Master")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td></td><td></td><td align=""right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")

                gridviewHtml = strMessage.ToString() + gridviewHtml
                Context.Response.Buffer = True
                Context.Response.ClearContent()
                Context.Response.ClearHeaders()

                Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
                Context.Response.AddHeader("content-Disposition", "attachment; filename=" + filename)
                Context.Response.AddHeader("Content-Length", gridviewHtml.Length)

                Context.Response.Write(gridviewHtml)
                Context.Response.Flush()
                Context.Response.End()

                System.IO.File.Delete(filename)
                Exit Sub
            End If
        Catch ex As Exception
            Me.objcommon.ShowAlert("Error While Exporting Data To Excel : " + estr, Me.Page)

            Exit Sub
        End Try
    End Sub
End Class