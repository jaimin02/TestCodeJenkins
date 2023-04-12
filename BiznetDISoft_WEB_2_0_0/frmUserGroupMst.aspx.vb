Imports System.Web.Services
Imports Newtonsoft.Json
Imports Microsoft
Imports Microsoft.Office.Interop
Imports System.IO
Imports System.Drawing

Partial Class frmUserGroupMst
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtUserGroupMst As String = "DtUserGroupMst"
    Private Const VS_UserGroupCode As String = "iUserGroupCode"

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()


    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_UserGroupCode As Integer = 1
    Private Const GVC_UserGroupName As Integer = 2
    Private Const GVC_LocationCode As Integer = 3
    Private Const GVC_ProjectTypeCode As Integer = 4
    Private Const GVC_Remark As Integer = 5
    'iUserGroupCode,vUserGroupName,vLocationCode,vProjectTypeCode,vRemark,iModifyBy,dModifyOn,cStatusIndi

#End Region

#Region "Form Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                GenCall()
            End If
        Catch ex As Exception

        End Try
        Try
            If GV_UserGroupMst.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIGV_UserGroupMst", "UIGV_UserGroupMst(); ", True)   ''Added by ketan
            End If
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean
        Dim ds As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try
            Choice = Me.Request.QueryString("Mode")
            Me.ViewState(VS_Choice) = Choice   'To use it while saving
            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_UserGroupCode) = Me.Request.QueryString("Value").ToString
            End If

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtUserGroupMst) = ds.Tables("UserGroupMst")   ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If
            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideSponsorDetails", "HideSponsorDetails(); ", True)
            End If
            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..GenCall")
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_Data"

    Private Function GenCall_Data(ByRef ds_Retu As DataSet) As Boolean

        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Val As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing

        Try

            Val = Me.ViewState(VS_UserGroupCode) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "iUserGroupCode=" + Val.ToString
            End If

            If Not objHelp.getuserGroupMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
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

            ds_Retu = ds
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..GenCall_Data")
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_ShowUI"

    Private Function GenCall_ShowUI() As Boolean
        Dim dt_OpMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try

            Page.Title = ":: User Group Master  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")


            CType(Me.Master.FindControl("lblHeading"), Label).Text = "User Group Master"

            dt_OpMst = Me.ViewState(VS_DtUserGroupMst)

            Choice = Me.ViewState("Choice")

            If Not FillDropDown() Then
                Exit Function
            End If

            If Not FillGrid() Then
                Exit Function
            End If

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.txtUsrGroupName.Text = dt_OpMst.Rows(0).Item("vUserGroupName")
                Me.DDLLocation.SelectedValue = dt_OpMst.Rows(0).Item("vLocationCode")
                Me.DDLProjType.SelectedValue = dt_OpMst.Rows(0).Item("vProjectTypeCode")
                'Me.TxtRemark.Value = dt_OpMst.Rows(0).Item("vRemark")
                Me.BtnSave.Text = "Update"
                Me.BtnSave.ToolTip = "Update"
            End If

            'Me.BtnSave.Attributes.Add("OnClick", "return Validation();")

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
        Dim ds_Location As New Data.DataSet
        Dim dv_ProjectType As New DataView
        Dim dv_Location As New DataView
        Dim estr As String = String.Empty
        Dim wStr As String = String.Empty
        Try
            wStr = "cStatusIndi <> 'D'"
            If Not Me.objHelp.getLocationMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        ds_Location, estr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data From ProjectTypeMst : " + estr, Me.Page)
                Exit Function
            End If

            dv_Location = ds_Location.Tables(0).DefaultView.ToTable(True, "vLocationCode,vLocationName".Split(",")).DefaultView()
            dv_Location.Sort = "vLocationName"
            Me.DDLLocation.DataSource = dv_Location.ToTable()
            Me.DDLLocation.DataValueField = "vLocationCode"
            Me.DDLLocation.DataTextField = "vLocationName"
            Me.DDLLocation.DataBind()
            Me.DDLLocation.Items.Insert(0, New ListItem("Select Location", 0))

            If Not Me.objHelp.getprojectTypeMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        ds_ProjectType, estr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data From ProjectTypeMst : " + estr, Me.Page)
                Exit Function
            End If

            dv_ProjectType = ds_ProjectType.Tables(0).DefaultView.ToTable(True, "vProjectTypeCode,vProjectTypeName".Split(",")).DefaultView()
            dv_ProjectType.Sort = "vProjectTypeName"
            Me.DDLProjType.DataSource = dv_ProjectType.ToTable()
            Me.DDLProjType.DataValueField = "vProjectTypeCode"
            Me.DDLProjType.DataTextField = "vProjectTypeName"
            Me.DDLProjType.DataBind()
            Me.DDLProjType.Items.Insert(0, New ListItem("Select Project Type", 0))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..FillDropDown")
            Return False
        End Try
    End Function

#End Region

#Region "FillGrid"

    Private Function FillGrid() As Boolean
        Dim ds_UserGroupMst As New Data.DataSet
        Dim estr As String = String.Empty
        Dim wStr As String = String.Empty
        Try
            wStr = "cStatusIndi <> 'D'"
            If Not objHelp.getuserGroupMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_UserGroupMst, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From UserGroupMst", estr)
                Return False
            End If

            If ds_UserGroupMst.Tables(0).Rows.Count < 1 Then
                ObjCommon.ShowAlert("No Record Found", Me.Page)
                Return True
            End If
            ds_UserGroupMst.Tables(0).DefaultView.Sort = "vUserGroupName"
            Me.GV_UserGroupMst.DataSource = ds_UserGroupMst.Tables(0).DefaultView.ToTable
            Me.GV_UserGroupMst.DataBind()
            If GV_UserGroupMst.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIGV_UserGroupMst", "UIGV_UserGroupMst(); ", True)   ''Added by ketan
            End If
            Return True

        Catch ex As Exception
            If GV_UserGroupMst.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIGV_UserGroupMst", "UIGV_UserGroupMst(); ", True)
            End If


            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..FillGrid")
            Return False
        End Try
    End Function

#End Region

#Region "Button Events"

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
            dt_Save = CType(Me.ViewState(VS_DtUserGroupMst), DataTable)
            dt_Save.TableName = "UserGroupMst"
            ds_Save.Tables.Add(dt_Save)

            If Not objLambda.Save_InsertuserGroupMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_userGroupMst, ds_Save, Me.Session(S_UserID), estr) Then
                ObjCommon.ShowAlert("Error While Saving UserGroupMst", Me.Page)
                Exit Sub
            End If

            message = IIf(Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "User Group Details Saved Successfully !", "User Group Details Updated Successfully !")
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Alert", "ShowAlert('" + message + "')", True)


        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..BtnSave_Click")
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.ResetPage()
        Me.Response.Redirect("frmUserGroupMst.aspx?mode=1")
    End Sub

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

#End Region

#Region "Assign values"

    Private Function AssignValues() As Boolean
        Dim dr As DataRow
        Dim dt_User As New DataTable
        Dim ds_Check As New DataSet
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Try

            dt_User = CType(Me.ViewState(VS_DtUserGroupMst), DataTable)

            'For validating Duplication
            wStr = "cStatusIndi <> 'D' And vUserGroupName='" & Me.txtUsrGroupName.Text.Trim() & "'"

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                wStr += " And iUserGroupCode <> " + Me.ViewState(VS_UserGroupCode)
            End If

            If Not objHelp.getuserGroupMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_Check, eStr) Then
                Me.ShowErrorMessage("Error While Getting Data From UserGroupMst", eStr)
                Exit Function
            End If

            If ds_Check.Tables(0).Rows.Count > 0 Then

                ObjCommon.ShowAlert("User Group Name Already Exists !", Me.Page)
                Return False

            End If
            '***********************************

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                dt_User.Clear()
                dr = dt_User.NewRow()
                'vOperationName,vOperationPath,vParentOperationCode,iSeqNo,iModifyBy
                'dr("iUserGroupCode") = "0"
                dr("vUserGroupName") = Me.txtUsrGroupName.Text
                dr("vLocationCode") = Me.DDLLocation.SelectedValue
                dr("vProjectTypeCode") = Me.DDLProjType.SelectedValue
                dr("vRemark") = Me.TxtRemark.Value
                dr("iModifyBy") = Me.Session(S_UserID)
                dt_User.Rows.Add(dr)

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                dt_User.Rows(0).Item("vUserGroupName") = Me.txtUsrGroupName.Text
                dt_User.Rows(0).Item("vLocationCode") = Me.DDLLocation.SelectedValue
                dt_User.Rows(0).Item("vProjectTypeCode") = Me.DDLProjType.SelectedValue
                dt_User.Rows(0).Item("vRemark") = Me.TxtRemark.Value
                dt_User.Rows(0).Item("iModifyBy") = Me.Session(S_UserID)
                dt_User.Rows(0).Item("cStatusIndi") = "E"
                dt_User.AcceptChanges()

            End If

            Me.ViewState(VS_DtUserGroupMst) = dt_User
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...AssignValues")
            Return False
        End Try
    End Function

#End Region

#Region "Reset Page"

    Private Sub ResetPage()
        Me.DDLLocation.SelectedIndex = 0
        Me.DDLProjType.SelectedIndex = 0
        Me.txtUsrGroupName.Text = ""
        Me.TxtRemark.Value = ""
        Me.ViewState(VS_DtUserGroupMst) = Nothing

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

    Protected Sub GV_UserGroupMst_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        'If e.Row.RowType = DataControlRowType.DataRow Or _
        'e.Row.RowType = DataControlRowType.Footer Or _
        'e.Row.RowType = DataControlRowType.Header Then
        '    e.Row.Cells(GVC_UserGroupCode).Visible = False
        '    e.Row.Cells(GVC_LocationCode).Visible = False
        '    e.Row.Cells(GVC_ProjectTypeCode).Visible = False
        'End If
    End Sub

    Protected Sub GV_UserGroupMst_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If Not e.Row.RowType = DataControlRowType.Pager Then
            e.Row.Cells(GVC_UserGroupCode).Visible = False
            e.Row.Cells(GVC_LocationCode).Visible = False
            e.Row.Cells(GVC_ProjectTypeCode).Visible = False

            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + 1
                CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandName = "Edit"
                CType(e.Row.FindControl("lnkAudit"), ImageButton).Attributes.Add("iUserGroupCode", e.Row.Cells(GVC_UserGroupCode).Text)
            End If
        End If
    End Sub

    Protected Sub GV_UserGroupMst_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim i As Integer = e.CommandArgument

        If e.CommandName.ToUpper = "EDIT" Then
            Me.Response.Redirect("frmUserGroupMst.aspx?mode=2&value=" & Me.GV_UserGroupMst.Rows(i).Cells(GVC_UserGroupCode).Text.Trim())
        End If
    End Sub

    Protected Sub GV_UserGroupMst_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        GV_UserGroupMst.PageIndex = e.NewPageIndex
        If Not FillGrid() Then
            Exit Sub
        End If
    End Sub

#End Region
#Region "Web Method"

    <WebMethod> _
    Public Shared Function AuditTrail(ByVal VUserGroupCode As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_UserGroupMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty

        Dim dtUserGroupMstHistory As New DataTable
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
            vTableName = "UserGroupMstHistory"
            vIdName = ""
            AuditFieldName = "iUserGroupCode"
            AuditFieldValue = VUserGroupCode
            vOtherTableName = ""
            vOtherIdName = ""


            Dim Param As String = ""


            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName



            If Not objHelp.Proc_GetAuditTrail(wStr, ds_UserGroupMst, Param) Then
                Throw New Exception(estr)
            End If

            If Not dtUserGroupMstHistory Is Nothing Then
                dtUserGroupMstHistory.Columns.Add("SrNo")
                dtUserGroupMstHistory.Columns.Add("UserGroupName")
                dtUserGroupMstHistory.Columns.Add("Remarks")
                dtUserGroupMstHistory.Columns.Add("ModifyBy")
                dtUserGroupMstHistory.Columns.Add("ModifyOn")
            End If
            dt = ds_UserGroupMst.Tables(0)
            Dim dv As New DataView(dt)

            For Each dr As DataRow In dv.ToTable.Rows

                drAuditTrail = dtUserGroupMstHistory.NewRow()

                drAuditTrail("SrNo") = i
                drAuditTrail("UserGroupName") = dr("vUserGroupName").ToString()
                drAuditTrail("Remarks") = dr("vRemark").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("ModifyOn"))
                dtUserGroupMstHistory.Rows.Add(drAuditTrail)
                dtUserGroupMstHistory.AcceptChanges()
                i += 1
            Next
            strReturn = JsonConvert.SerializeObject(dtUserGroupMstHistory)
            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function



    Protected Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_UserGroupMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty

        Dim dtUserGroupMstHistory As New DataTable
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
            vTableName = "UserGroupMstHistory"
            vIdName = ""
            AuditFieldName = "iUserGroupCode"
            AuditFieldValue = hdnUsergroupCode.Value
            vOtherTableName = ""
            vOtherIdName = ""


            Dim Param As String = ""


            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName

            If Not objHelp.Proc_GetAuditTrail(wStr, ds_UserGroupMst, Param) Then
                Throw New Exception(estr)
            End If

            If Not dtUserGroupMstHistory Is Nothing Then
                dtUserGroupMstHistory.Columns.Add("SrNo")
                dtUserGroupMstHistory.Columns.Add("UserGroupName")
                dtUserGroupMstHistory.Columns.Add("Remarks")
                dtUserGroupMstHistory.Columns.Add("ModifyBy")
                dtUserGroupMstHistory.Columns.Add("ModifyOn")
            End If
            dt = ds_UserGroupMst.Tables(0)
            Dim dv As New DataView(dt)

            For Each dr As DataRow In dv.ToTable.Rows

                drAuditTrail = dtUserGroupMstHistory.NewRow()

                drAuditTrail("SrNo") = i
                drAuditTrail("UserGroupName") = dr("vUserGroupName").ToString()
                drAuditTrail("Remarks") = dr("vRemark").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("ModifyOn"))
                dtUserGroupMstHistory.Rows.Add(drAuditTrail)
                dtUserGroupMstHistory.AcceptChanges()
                i += 1
            Next
            gvExport.DataSource = dtUserGroupMstHistory
            gvExport.DataBind()

            If gvExport.Rows.Count > 0 Then

                gvExport.HeaderRow.BackColor = Color.White
                For Each cell As TableCell In gvExport.HeaderRow.Cells
                    cell.BackColor = gvExport.HeaderStyle.BackColor
                    cell.Height = 20
                Next
                For Each row As GridViewRow In gvExport.Rows
                    row.BackColor = Color.White
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


                Dim style As String = "<style> .textmode { } </style>"
                Response.Write(style)



                Dim info As String = String.Empty
                Dim gridviewHtml As String = String.Empty

                filename = "Audit Trail_" + hdnUsergroupCode.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                Dim stringWriter As New System.IO.StringWriter()

                Dim writer As New HtmlTextWriter(stringWriter)
                gvExport.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()

                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("UserGroup Master-AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")


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

                filename = "Audit Trail_" + hdnUsergroupCode.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"


                drAuditTrail = dtUserGroupMstHistory.NewRow()

                drAuditTrail("SrNo") = ""
                drAuditTrail("UserGroupName") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("ModifyBy") = ""
                drAuditTrail("ModifyOn") = ""
                dtUserGroupMstHistory.Rows.Add(drAuditTrail)
                dtUserGroupMstHistory.AcceptChanges()

                gvExport.DataSource = dtUserGroupMstHistory
                gvExport.DataBind()


                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(stringWriter)
                gvExport.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()


                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("UserGroup Master-AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")


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
            Me.ObjCommon.ShowAlert("Error While Exporting Data To Excel : " + estr, Me.Page)

            Exit Sub
        End Try
    End Sub


    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    End Sub
#End Region

End Class