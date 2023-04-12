Imports Microsoft.Win32
Imports System.Web.Services
Imports Newtonsoft.Json
Imports Microsoft
Imports Microsoft.Office.Interop
Imports System.IO
Imports System.Drawing

Partial Class frmMedExSubGroupMst
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objcommon As New clsCommon
    Private objhelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtMedexSubGroupMst As String = "DtMedExSubGroupMst"
    Private Const VS_DtBlankMedExSubGroupMst As String = "DtBlankMedExSubGroupMst"
    Private Const VS_MedexSubGroupCode As String = "MedexSubGroupCode"

    Private eStr As String = ""

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_MedexSubGroupCode As Integer = 1

#End Region

#Region "Page Load "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                GenCall()
            End If
            Me.AutoCompleteExtenderForSubGroup.ContextKey = " nScopeNo in (" + Me.Session(S_ScopeNo) + ")"
        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr)
        End Try

        Try
            If gvwMedExSubGroupMst.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvwMedExSubGroupMst", "UIgvwMedExSubGroupMst(); ", True)
            End If
            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideAttributeSubGroupDetails", "HideAttributeSubGroupDetails(); ", True)
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
                Me.ViewState(VS_MedexSubGroupCode) = Me.Request.QueryString("Value").ToString
            End If

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtMedexSubGroupMst) = ds.Tables(0)   ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If

            'If Not IsPostBack Then
            '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideSponsorDetails", "HideSponsorDetails(); ", True)
            'End If
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

            Val = Me.ViewState(VS_MedexSubGroupCode) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vMedExSubGroupCode=" + Val.ToString
            End If


            If Not objhelp.GetMedExSubGroupMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                Me.ShowErrorMessage(eStr_Retu, "")
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

            Page.Title = ":: Attribute Sub Group Master :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Attribute Sub Group Master"

            If Not Fillgrid() Then
                Exit Function
            End If


            dt_OpMst = Me.ViewState(VS_DtMedexSubGroupMst)

            Choice = Me.ViewState(VS_Choice)

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

            End If

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_ShowUI")
        Finally
        End Try
    End Function

#End Region

#Region "Fill Grid"

    Private Function Fillgrid() As Boolean
        Dim eStr_Retu As String = String.Empty
        Dim Ds_Grid As New DataSet
        Dim dvMedExGroup As New DataView
        Dim wstr As String = String.Empty
        Try


            wstr = "cStatusIndi<>'D' "

            If Me.HSubGroupId.Value.Trim.Length() > 0 Then
                wstr += " And vMedExSubGroupCode = '" & Me.HSubGroupId.Value.Trim.ToString() & "' And  nScopeNo in (" + Me.Session(S_ScopeNo) + ")"
            End If

            If objhelp.GetMedExSubGroupMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_Grid, eStr_Retu) Then
                dvMedExGroup = Ds_Grid.Tables(0).Copy.DefaultView
                dvMedExGroup.Sort = "vMedExSubGroupDesc"
                gvwMedExSubGroupMst.DataSource = dvMedExGroup
                gvwMedExSubGroupMst.DataBind()

                If gvwMedExSubGroupMst.Rows.Count > 0 Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvwMedExSubGroupMst", "UIgvwMedExSubGroupMst(); ", True)
                End If

            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...Fillgrid")
        End Try

    End Function

#End Region

#Region "Button Events"

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click
        Me.Response.Redirect("frmMainpage.aspx", False)
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
                Me.ViewState(VS_DtMedexSubGroupMst) = ds.Tables(0)   ' adding blank DataTable in viewstate

                If Not AssignValues("Add") Then
                    Exit Sub
                End If

            End If


            ds_save = New DataSet
            ds_save.Tables.Add(CType(Me.ViewState(VS_DtMedexSubGroupMst), Data.DataTable).Copy())
            ds_save.Tables(0).TableName = "MedExSubGroupMst"


            If Not objLambda.Save_MedExSubGroupMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_MedExSubGroupMst, ds_save, Me.Session(S_UserID), eStr_Retu) Then

                objcommon.ShowAlert("Error While Saving Attribute Sub Group Master", Me.Page)
                Exit Sub

            End If
            message = IIf(Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "Attribute Sub Group Saved Successfully !", "Attribute Sub Group Updated Successfully !")

            objcommon.ShowAlert(message, Me.Page)
            Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
            Me.btncancel.Visible = False
            Me.BtnExit.Visible = True
            Resetpage()
            Fillgrid()

        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr)
        End Try
    End Sub

    Protected Sub btnupdate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Dim Ds_Editsave As New DataSet
        'Dim eStr As String = String.Empty

        'Try
        '    

        '    If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

        '        If Not AssignValues("Edit") Then
        '            Exit Sub
        '        End If

        '    End If

        '    Ds_Editsave = New DataSet
        '    Ds_Editsave.Tables.Add(CType(Me.ViewState(VS_DtMedexSubGroupMst), Data.DataTable).Copy())
        '    Ds_Editsave.Tables(0).TableName = "MedExSubGroupMst"

        '    If Not objLambda.Save_MedExSubGroupMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_MedExSubGroupMst, _
        '                                            Ds_Editsave, Me.Session(S_UserID), eStr) Then

        '        objcommon.ShowAlert("Error While Saving Attribute Sub Group Master", Me.Page)
        '        Exit Sub

        '    End If

        '    objcommon.ShowAlert("Attribute Sub Group Updated Successfully", Me.Page)
        '    Resetpage()
        '    Fillgrid()
        '    Me.BtnSave.Visible = True
        '    Me.BtnExit.Visible = True
        '    Me.btnupdate.Visible = False
        '    Me.btncancel.Visible = False
        '    Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add

        'Catch ex As Exception
        '    ShowErrorMessage(ex.Message, eStr)

        'End Try
    End Sub

    Protected Sub btncancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Resetpage()
        Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
        'Me.BtnSave.Visible = True
        Me.BtnExit.Visible = True
        'Me.btnupdate.Visible = False
        Me.btncancel.Visible = False
    End Sub

#End Region

#Region "Assign Values"

    Private Function AssignValues(ByVal type As String) As Boolean
        Dim dr As DataRow
        Dim dt_User As New DataTable
        Dim ds_Save As New DataSet
        Dim estr_Retu As String = String.Empty
        Dim ds_Check As New DataSet
        Dim wStr As String = String.Empty
        Try


            dt_User = CType(Me.ViewState(VS_DtMedexSubGroupMst), DataTable)

            'For validating Duplication
            wStr = "cStatusIndi<>'D' And vMedExSubGroupDesc='" & Me.txtMedExSubGropuDesc.Text.Trim() & "' AND  vDisplayName='" & Me.txtDisplayName.Text.Trim() & "'  "

            If type.ToUpper = "EDIT" Then
                wStr += " And vMedExSubGroupCode <> '" + Me.ViewState(VS_MedexSubGroupCode).ToString() + "'"
            End If

            If Not objhelp.GetMedExSubGroupMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                ds_Check, estr_Retu) Then

                Me.ShowErrorMessage("Error While Getting Data From Attribute Sub Group Master", estr_Retu)
                Return False

            End If

            If ((type.ToUpper = "ADD" Or type.ToUpper = "EDIT") And ds_Check.Tables(0).Rows.Count > 0) Then

                objcommon.ShowAlert("Attribute Sub Group Name Already Exists !", Me.Page)

                If gvwMedExSubGroupMst.Rows.Count > 0 Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvwMedExSubGroupMst", "UIgvwMedExSubGroupMst(); ", True)
                End If
                Exit Function

                Return False

            End If
            '*************************************

            If type.ToUpper = "ADD" Then

                dt_User.Clear()
                dr = dt_User.NewRow()
                dr("vMedExSubGroupCode") = "0"
                dr("vMedExSubGroupDesc") = Me.txtMedExSubGropuDesc.Text.Trim()
                dr("cActiveFlag") = GeneralModule.ActiveFlag_Yes
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dr("vCDISCValue") = Me.txtCDISCValue.Text.Trim()
                dr("vOtherValues") = Me.txtOtherValues.Text.Trim()
                dr("vDisplayName") = txtDisplayName.Text.Trim()
                dr("vRemark") = txtRemark.Text.Trim()
                dt_User.Rows.Add(dr)

            ElseIf type.ToUpper = "EDIT" Then

                For Each dr In dt_User.Rows

                    'dr("vMedExCode") = "0"
                    dr("vMedExSubGroupDesc") = Me.txtMedExSubGropuDesc.Text.Trim()
                    dr("cActiveFlag") = GeneralModule.ActiveFlag_Yes
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "E"
                    dr("vCDISCValue") = Me.txtCDISCValue.Text.Trim()
                    dr("vOtherValues") = Me.txtOtherValues.Text.Trim()
                    dr("vDisplayName") = txtDisplayName.Text.Trim()
                    dr("vRemark") = txtRemark.Text.Trim()
                    dr.AcceptChanges()

                Next dr

            ElseIf type.ToUpper = "DELETE" Then

                For Each dr In dt_User.Rows

                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "D"
                    dr.AcceptChanges()

                Next dr

                dt_User.TableName = "MedExSubGroupMst"
                ds_Save.Tables.Add(dt_User.Copy())

                If Not objLambda.Save_MedExSubGroupMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_MedExSubGroupMst, _
                                                        ds_Save, Me.Session(S_UserID), estr_Retu) Then

                    objcommon.ShowAlert("Error While Deleteing from Attribute Sub Group Master", Me.Page)
                    Exit Function

                End If

                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
                objcommon.ShowAlert("Attribute Sub Group Deleted Successfully", Me.Page)
                Fillgrid()

            End If

            Me.ViewState(VS_DtMedexSubGroupMst) = dt_User
            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....AssignValues")
            Return False
        End Try
    End Function

#End Region

#Region "Reset Page"

    Private Sub Resetpage()
        Me.txtMedExSubGropuDesc.Text = ""
        Me.chkactive.Checked = True
        Me.txtCDISCValue.Text = ""
        Me.txtOtherValues.Text = ""
        Me.txtDisplayName.Text = ""
        Me.txtRemark.Text = ""
        Me.BtnSave.Text = "Save"
        Me.BtnSave.ToolTip = "Save"
    End Sub

#End Region

#Region "GridViewEvents"

    Protected Sub gvmedexgrp_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)

        gvwMedExSubGroupMst.PageIndex = e.NewPageIndex
        Fillgrid()

    End Sub

    Protected Sub gvmedexgrp_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim index As Integer = e.CommandArgument
        Dim ds As New DataSet
        Dim eStr As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim dt As New DataTable
        Try

            If e.CommandName.ToUpper() = "EDIT" Or e.CommandName.ToUpper() = "DELETE" Then

                Me.ViewState(VS_MedexSubGroupCode) = Me.gvwMedExSubGroupMst.Rows(index).Cells(GVC_MedexSubGroupCode).Text.ToString()

                Wstr = "vMedExSubGroupCode='" & Me.gvwMedExSubGroupMst.Rows(index).Cells(GVC_MedexSubGroupCode).Text & "'"
                If Not objhelp.GetMedExSubGroupMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr) Then
                    Response.Write(eStr)
                    Exit Sub
                End If
                Me.ViewState(VS_DtMedexSubGroupMst) = ds.Tables(0)
                dt = Me.ViewState(VS_DtMedexSubGroupMst)

            End If

            If e.CommandName.ToUpper = "EDIT" Then

                Dim dr As DataRow = dt.Rows(0)
                Me.txtMedExSubGropuDesc.Text = dr("vMedExSubGroupDesc").ToString
                Me.chkactive.Checked = IIf(dr("cActiveFlag") = GeneralModule.ActiveFlag_Yes, True, False)
                Me.txtCDISCValue.Text = dr("vCDISCValue").ToString
                Me.txtOtherValues.Text = dr("vOtherValues").ToString()
                Me.txtDisplayName.Text = dr("vDisplayName").ToString()
                Me.ViewState(VS_DtMedexSubGroupMst) = dt
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                'Me.BtnSave.Visible = True
                Me.BtnExit.Visible = False
                'Me.btnupdate.Visible = True
                Me.btncancel.Visible = True
                Me.BtnSave.Text = "Update"
                Me.BtnSave.ToolTip = "Update"

            ElseIf e.CommandName.ToUpper = "DELETE" Then

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
            CType(e.Row.FindControl("ImbEdit"), ImageButton).CommandName = "EDIT"
            CType(e.Row.FindControl("ImbDelete"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("ImbDelete"), ImageButton).CommandName = "DELETE"
            e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (gvwMedExSubGroupMst.PageSize * gvwMedExSubGroupMst.PageIndex) + 1
            CType(e.Row.FindControl("lnkAudit"), ImageButton).Attributes.Add("vMedExSubGroupCode", e.Row.Cells(GVC_MedexSubGroupCode).Text)

        End If

        If e.Row.RowType = DataControlRowType.DataRow Or _
            e.Row.RowType = DataControlRowType.Header Or _
            e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVC_MedexSubGroupCode).Visible = False

        End If

    End Sub

    Protected Sub gvmedexgrp_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs)

    End Sub

    Protected Sub gvmedexgrp_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs)
        e.Cancel = True
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

    Protected Sub btnSetSubGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetSubGroup.Click
        Fillgrid()
    End Sub

    Protected Sub BtnViewAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnViewAll.Click
        Me.txtSubGroup.Text = ""
        Me.HSubGroupId.Value = ""
        Fillgrid()
    End Sub

#Region "Web Method"

    <WebMethod> _
    Public Shared Function AuditTrail(ByVal vMedExSubGroupCode As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_MedExSubGroupMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim dtMedExSubGroupMstHistory As New DataTable
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

            vTableName = "MedExSubGroupMstHistory"
            vIdName = ""
            AuditFieldName = "vMedExSubGroupCode"
            AuditFieldValue = vMedExSubGroupCode
            vOtherTableName = ""
            vOtherIdName = ""


            Dim Param As String = ""


            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName

            If Not objHelp.Proc_GetAuditTrail(wStr, ds_MedExSubGroupMst, Param) Then
                Throw New Exception(estr)
            End If

            If Not dtMedExSubGroupMstHistory Is Nothing Then
                dtMedExSubGroupMstHistory.Columns.Add("SrNo")
                dtMedExSubGroupMstHistory.Columns.Add("MedExSubGroupDesc")
                dtMedExSubGroupMstHistory.Columns.Add("CDISCValue")
                dtMedExSubGroupMstHistory.Columns.Add("OtherValues")
                dtMedExSubGroupMstHistory.Columns.Add("DisplayName")
                dtMedExSubGroupMstHistory.Columns.Add("Remark")
                dtMedExSubGroupMstHistory.Columns.Add("ModifyBy")
                dtMedExSubGroupMstHistory.Columns.Add("ModifyOn")
            End If

            dt = ds_MedExSubGroupMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtMedExSubGroupMstHistory.NewRow()
                drAuditTrail("SrNo") = i
                drAuditTrail("MedExSubGroupDesc") = dr("vMedExSubGroupDesc").ToString()
                drAuditTrail("CDISCValue") = dr("vCDISCValue").ToString()
                drAuditTrail("OtherValues") = dr("vOtherValues").ToString()
                drAuditTrail("DisplayName") = dr("vDisplayName").ToString()
                drAuditTrail("Remark") = dr("vRemark").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("ModifyOn"))
                dtMedExSubGroupMstHistory.Rows.Add(drAuditTrail)
                dtMedExSubGroupMstHistory.AcceptChanges()
                i += 1
            Next
            strReturn = JsonConvert.SerializeObject(dtMedExSubGroupMstHistory)
            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Protected Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_MedExSubGroupMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty

        Dim dtMedExSubGroupMstHistory As New DataTable
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

            vTableName = "MedExSubGroupMstHistory"
            vIdName = ""
            AuditFieldName = "vMedExSubGroupCode"
            AuditFieldValue = hdnMedExSubGroupCode.Value
            vOtherTableName = ""
            vOtherIdName = ""


            Dim Param As String = ""

            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName

            If Not objHelp.Proc_GetAuditTrail(wStr, ds_MedExSubGroupMst, Param) Then
                Throw New Exception(estr)
            End If

            If Not dtMedExSubGroupMstHistory Is Nothing Then
                dtMedExSubGroupMstHistory.Columns.Add("Sr. No")
                dtMedExSubGroupMstHistory.Columns.Add("Attribute Sub Group Desc")
                dtMedExSubGroupMstHistory.Columns.Add("Variable Name")
                dtMedExSubGroupMstHistory.Columns.Add("Other Value")
                dtMedExSubGroupMstHistory.Columns.Add("Display Name")
                dtMedExSubGroupMstHistory.Columns.Add("Remarks")
                dtMedExSubGroupMstHistory.Columns.Add("Modify By")
                dtMedExSubGroupMstHistory.Columns.Add("Modify On")
            End If

            dtMedExSubGroupMstHistory.AcceptChanges()
            dt = ds_MedExSubGroupMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtMedExSubGroupMstHistory.NewRow()
                drAuditTrail("Sr. No") = i
                drAuditTrail("Attribute Sub Group Desc") = dr("vMedExSubGroupDesc").ToString()
                drAuditTrail("Variable Name") = dr("vCDISCValue").ToString()
                drAuditTrail("Other Value") = dr("vOtherValues").ToString()
                drAuditTrail("Display Name") = dr("vDisplayName").ToString()
                drAuditTrail("Remarks") = dr("vRemark").ToString()
                drAuditTrail("Modify By") = dr("vModifyBy").ToString()
                drAuditTrail("Modify On") = Convert.ToString(dr("ModifyOn"))
                dtMedExSubGroupMstHistory.Rows.Add(drAuditTrail)
                dtMedExSubGroupMstHistory.AcceptChanges()
                i += 1
            Next

            gvExport.DataSource = dtMedExSubGroupMstHistory
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

                filename = "Audit Trail_" + hdnMedExSubGroupCode.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                Dim stringWriter As New System.IO.StringWriter()

                Dim writer As New HtmlTextWriter(stringWriter)
                gvExport.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()

                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""7""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")





                strMessage.Append("<td colspan=""7""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Attribute Sub Group Master-AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td></td><td Align=""Right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")


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

                filename = "Audit Trail_" + hdnMedExSubGroupCode.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"


                drAuditTrail = dtMedExSubGroupMstHistory.NewRow()

                drAuditTrail("Sr. No") = ""
                drAuditTrail("Attribute Sub Group Desc") = ""
                drAuditTrail("Variable Name") = ""
                drAuditTrail("Other Value") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("Modify By") = ""
                drAuditTrail("Modify On") = ""
                dtMedExSubGroupMstHistory.Rows.Add(drAuditTrail)
                dtMedExSubGroupMstHistory.AcceptChanges()
                gvExport.DataSource = dtMedExSubGroupMstHistory
                gvExport.DataBind()


                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(stringWriter)
                gvExport.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()


                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""7""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<td colspan=""7""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Attribute Sub Group Master-AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td></td><td align=""right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")


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
        Dim ds_MedSubGroupMst As New DataSet
        Dim estr As String = String.Empty
        Dim dtMedSubGroupMstHistory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable
        Dim MedSubGroupCode As String = String.Empty
        Dim filename As String = String.Empty
        Dim strMessage As New StringBuilder

        Try
            wStr = "" + "##"
            ds_MedSubGroupMst = objHelp.ProcedureExecute("Proc_MedExSubGroupMst ", wStr)
            wStr = String.Empty

            If Not dtMedSubGroupMstHistory Is Nothing Then
                dtMedSubGroupMstHistory.Columns.Add("Sr. No")
                dtMedSubGroupMstHistory.Columns.Add("Attribute Sub Group Desc")
                dtMedSubGroupMstHistory.Columns.Add("Variable Name")
                dtMedSubGroupMstHistory.Columns.Add("Other Value")
                dtMedSubGroupMstHistory.Columns.Add("Remarks")
                dtMedSubGroupMstHistory.Columns.Add("Modify By")
                dtMedSubGroupMstHistory.Columns.Add("Modify On")
            End If

            dtMedSubGroupMstHistory.AcceptChanges()
            dt = ds_MedSubGroupMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtMedSubGroupMstHistory.NewRow()
                drAuditTrail("Sr. No") = i
                drAuditTrail("Attribute Sub Group Desc") = dr("vMedExSubGroupDesc").ToString()
                drAuditTrail("Variable Name") = dr("vCDISCValue").ToString()
                drAuditTrail("Other Value") = dr("vOtherValues").ToString()
                'drAuditTrail("Display Name") = dr("vDisplayName").ToString()
                drAuditTrail("Remarks") = dr("vRemark").ToString()
                drAuditTrail("Modify By") = dr("iModifyBy").ToString()
                drAuditTrail("Modify On") = Convert.ToString(dr("dModifyOn"))
                dtMedSubGroupMstHistory.Rows.Add(drAuditTrail)
                dtMedSubGroupMstHistory.AcceptChanges()
                i += 1
            Next

            gvExportToExcel.DataSource = dtMedSubGroupMstHistory
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

                filename = "AttributeSubGroup Master" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                Dim stringWriter As New System.IO.StringWriter()

                Dim writer As New HtmlTextWriter(stringWriter)
                gvExportToExcel.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()

                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""7""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<td colspan=""7""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Attribute Sub Group Master")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td></td><td Align=""Right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")

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

                filename = "AttributeSubGroup Master" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                drAuditTrail = dtMedSubGroupMstHistory.NewRow()

                drAuditTrail("Sr. No") = i
                drAuditTrail("Attribute Sub Group Desc") = ""
                drAuditTrail("Variable Name") = ""
                drAuditTrail("Other Value") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("Modify By") = ""
                drAuditTrail("Modify On") = ""
                dtMedSubGroupMstHistory.Rows.Add(drAuditTrail)
                dtMedSubGroupMstHistory.AcceptChanges()
                gvExportToExcel.DataSource = dtMedSubGroupMstHistory
                gvExportToExcel.DataBind()

                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(stringWriter)
                gvExportToExcel.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()

                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""7""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<td colspan=""7""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Attribute Sub Group Master")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td></td><td align=""right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")

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
