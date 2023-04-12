Imports System.Web.Services
Imports Newtonsoft.Json
Imports System.Drawing

Partial Class frmUOMmst
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtUOMMst As String = "DtUOMMst"
    Private Const VS_DtGvwUOMMst As String = "DtGvwUOMMst"
    Private Const VS_UOMCode As String = "UOMCode"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_UOMCode As Integer = 1
    Private Const GVC_UOMClass As Integer = 2
    Private Const GVC_UOMDesc As Integer = 3
    Private Const GVC_Edit As Integer = 5
    Private Const GVC_Delete As Integer = 6
    Private Const GVC_cStatusIndi As Integer = 4


    Private Shared index As Integer = 0
    Private Shared Gv_index As Integer = 0
    Private Shared Status As String = String.Empty


#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                GenCall()
                Exit Sub
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvwUOM", "UIgvwUOM(); ", True)
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "GenCall"
    Private Function GenCall() As Boolean
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim dt_UOMMst As DataTable = Nothing
        Dim ds_UOMMst As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try


            Choice = Me.Request.QueryString("mode").ToString
            Me.ViewState(VS_Choice) = Choice   'To use it while saving

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_UOMCode) = Me.Request.QueryString("Value").ToString
            End If

            ''''Check for Valid User''''''''''''''
            If Not GenCall_Data(Choice, dt_UOMMst) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtUOMMst) = dt_UOMMst ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI(Choice, dt_UOMMst) Then 'For Displaying Data 
                Exit Function
            End If
            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideUOMDetails", "HideUOMDetails(); ", True)
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
                            ByRef dt_UOM_Retu As DataTable) As Boolean

        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_UOMMst As DataSet = Nothing
        Try


            If Choice_1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vUOMCode= '" + Me.ViewState(VS_UOMCode).ToString() + "'"
            End If

            wStr += " And cStatusIndi <> 'D'"

            If Not objHelp.GetUOMMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_UOMMst, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_UOMMst Is Nothing Then
                Throw New Exception(eStr)
            End If

            dt_UOM_Retu = ds_UOMMst.Tables(0)
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall")
        Finally
        End Try
    End Function
#End Region

#Region "GenCall_ShowUI"
    Private Function GenCall_ShowUI(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                            ByVal dt_UOMMst As DataTable) As Boolean

        Try
            CType(Master.FindControl("lblHeading"), Label).Text = "UOM Master"

            Page.Title = ":: UOM Master  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            If Not FillGridUOM() Then
                Return False
            End If

            If Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.txtUOMDesc.Text = ConvertDbNullToDbTypeDefaultValue(dt_UOMMst.Rows(0)("vUOMDesc"), dt_UOMMst.Rows(0)("vUOMDesc").GetType)
                'Me.ddlUOMClass.Items.FindByText(ConvertDbNullToDbTypeDefaultValue(dt_UOMMst.Rows(0)("vUOMClass"), dt_UOMMst.Rows(0)("vUOMClass").GetType)).Selected = True
                Me.txtUOMClass.Text = ConvertDbNullToDbTypeDefaultValue(dt_UOMMst.Rows(0)("vUOMClass"), dt_UOMMst.Rows(0)("vUOMClass").GetType)
                Me.btnSave.Text = "Update"
                Me.btnSave.ToolTip = "Update"
            End If

            GenCall_ShowUI = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_ShowUI")
        End Try
    End Function
#End Region

    '#Region "Fill DropDownUOMClass"
    '    Private Function FillDropDownUOMClass() As Boolean
    '        Dim wStr As String = String.Empty
    '        Dim eStr As String = String.Empty
    '        Dim ds_UOMMst As New DataSet
    '        Dim dv_UOMMst As New DataView
    '        Try
    '            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

    '            wStr = "cStatusIndi <> 'D'"

    '            If Not objHelp.GetUOMMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                        ds_UOMMst, eStr) Then
    '                Throw New Exception(eStr)
    '            End If

    '            dv_UOMMst = ds_UOMMst.Tables(0).DefaultView.ToTable(True, "vUOMClass").DefaultView
    '            dv_UOMMst.Sort = "vUOMClass"
    '            Me.ddlUOMClass.DataSource = dv_UOMMst
    '            Me.ddlUOMClass.DataTextField = "vUOMClass"
    '            'Me.ddlUOMClass.DataValueField = "vUOMCode"
    '            Me.ddlUOMClass.DataBind()
    '            Me.ddlUOMClass.Items.Insert(0, New ListItem("Select UOM Class", ""))

    '            Return True
    '        Catch ex As Exception
    '            ShowErrorMessage(ex.Message, "... FillDropDownUOMClass")
    '            Return False
    '        End Try

    '    End Function
    '#End Region

#Region "Fill GridUOM"
    Private Function FillGridUOM() As Boolean
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_UOMMst As New DataSet
        Try

            wStr = ""

            If Not objHelp.GetUOMMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, _
                                        ds_UOMMst, eStr) Then
                Throw New Exception(eStr)
            End If



            ds_UOMMst.Tables(0).DefaultView.Sort = "vUOMClass"
            Me.gvwUOM.DataSource = ds_UOMMst.Tables(0).DefaultView.ToTable()
            Me.gvwUOM.DataBind()

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvwUOM", "UIgvwUOM(); ", True)
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...FillGridUOM")
            Return False
        End Try
        Return True
    End Function
#End Region

#Region "Reset Page"
    Private Sub ResetPage()

        Me.txtUOMDesc.Text = ""
        Me.txtUOMClass.Text = ""
        Me.ViewState(VS_DtGvwUOMMst) = Nothing
        Me.ViewState(VS_DtUOMMst) = Nothing
        Me.ViewState(VS_UOMCode) = Nothing


    End Sub
#End Region

#Region "AssignValues"

    Private Function AssignValues() As Boolean
        Dim dtUOM As New DataTable
        Dim dr As DataRow
        Dim ds_Check As New DataSet
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Try
            dtUOM = Me.ViewState(VS_DtUOMMst)


            wStr = "cStatusIndi <> 'D' And vUOMClass='" & Me.txtUOMClass.Text.Trim & "'"

            If Me.Request.QueryString("mode").ToString = 2 Then
                wStr += " And vUOMCode <> '" + Me.Request.QueryString("Value").ToString + "'"
            End If

            If Not objHelp.GetUOMMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Check, eStr) Then
                Me.ShowErrorMessage("Error While Getting Data From UOMMst", eStr)
                Return False
            End If

            If ds_Check.Tables(0).Rows.Count > 0 Then
                objcommon.ShowAlert("UOM  Already Exists !", Me.Page)
                Return False
            End If

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                dtUOM.Clear()
                dr = dtUOM.NewRow
                'vUOMCode,vUOMClass,vUOMDesc,iModifyBy,dModifyOn,cStatusIndi
                dr("vUOMCode") = 0
                'dr("vUOMClass") = Me.ddlUOMClass.SelectedItem.Text.Trim
                dr("vUOMClass") = Me.txtUOMClass.Text.Trim
                dr("vRemarks") = Me.txtRemarks.Text.Trim
                dr("vUOMDesc") = Me.txtUOMDesc.Text.Trim
                dr("iCreatedBy") = Me.Session(S_UserID)
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dtUOM.Rows.Add(dr)

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                For Each dr In dtUOM.Rows

                    dr("vUOMCode") = Me.ViewState(VS_UOMCode).ToString()
                    'dr("vUOMClass") = Me.ddlUOMClass.SelectedItem.Text.Trim
                    dr("vUOMClass") = Me.txtUOMClass.Text.Trim
                    dr("vUOMDesc") = Me.txtUOMDesc.Text.Trim
                    dr("vRemarks") = Me.txtRemarks.Text.Trim
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "E"
                    dr.AcceptChanges()

                Next dr

                dtUOM.AcceptChanges()

            End If

            Me.ViewState(VS_DtUOMMst) = dtUOM
            Return True

        Catch ex As Exception
            Return True
        End Try
    End Function

#End Region

#Region "Button Events"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim Ds_UOM As DataSet
        Dim eStr As String = String.Empty
        Dim message As String = String.Empty
        Try


            If Not AssignValues() Then
                Exit Sub
            End If

            Ds_UOM = New DataSet
            Ds_UOM.Tables.Add(CType(Me.ViewState(VS_DtUOMMst), Data.DataTable).Copy())
            Ds_UOM.Tables(0).TableName = "UOMMst"   ' New Values on the form to be updated

            If Not objLambda.Save_UOMMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_UOMMst, Ds_UOM, Me.Session(S_UserID), eStr) Then


                Me.objcommon.ShowAlert("Error While Saving UOMMst", Me.Page)
                Exit Sub

            End If
            message = IIf(Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "UOM Details Saved Successfully !", "UOM Details Updated Successfully !")
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Alert", "ShowAlert('" + message + "')", True)
            ResetPage()
        Catch ThreadEx As Threading.ThreadAbortException

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...btnSave_Click")
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.ResetPage()
        Me.Response.Redirect("frmUOMmst.aspx?mode=1")

    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Response.Redirect("frmMainPage.aspx")
    End Sub

#End Region

#Region "BindGrid"
    Private Sub BindGrid()
        Dim dsUOM As New DataSet
        Dim eStr As String = String.Empty

        If objHelp.GetUOMMst("cStatusIndi <> 'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsUOM, eStr) Then

            gvwUOM.ShowFooter = False
            gvwUOM.DataSource = dsUOM
            gvwUOM.DataBind()
            dsUOM.Dispose()

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

#Region "Grid Events"

    Protected Sub gvwUOM_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.gvwUOM.PageIndex = e.NewPageIndex
        FillGridUOM()
    End Sub

    Protected Sub gvwUOM_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Try


            Dim Index As Integer = e.CommandArgument
            Dim dsUOM As New DataSet
            Dim wStr As String = String.Empty
            Dim eStr As String = String.Empty

            If e.CommandName.ToUpper = "EDIT" Then
                Me.Response.Redirect("frmUOMMst.aspx?mode=2&value=" & Me.gvwUOM.Rows(Index).Cells(GVC_UOMCode).Text.Trim())

            ElseIf e.CommandName.ToUpper = "DELETE" Then

                Status = "DELETE"

                btnRemarksUpdate.Text = "Delete"
                btnRemarksUpdate.ToolTip = "Delete"
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                Gv_index = e.CommandArgument
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvwUOM", "UIgvwUOM(); ", True)

        Catch ex As Exception

        End Try

    End Sub

    Protected Sub gvwUOM_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or _
                      e.Row.RowType = DataControlRowType.Header Or _
                      e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVC_UOMCode).Visible = False
            e.Row.Cells(GVC_cStatusIndi).Visible = False

            If e.Row.Cells(GVC_cStatusIndi).Text = "D" Then
                e.Row.BackColor = Drawing.Color.FromArgb(255, 189, 121)

                CType(e.Row.FindControl("lnkDelete"), ImageButton).Enabled = False
                CType(e.Row.FindControl("lnkEdit"), ImageButton).Enabled = False

            End If
        End If

    End Sub

    Protected Sub gvwUOM_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then

            e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (gvwUOM.PageSize * gvwUOM.PageIndex) + 1

            CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandName = "Edit"

            CType(e.Row.FindControl("lnkDelete"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkDelete"), ImageButton).CommandName = "Delete"
            CType(e.Row.FindControl("lnkAudit"), ImageButton).Attributes.Add("vUOMCode", e.Row.Cells(GVC_UOMCode).Text.Trim)

            If e.Row.Cells(GVC_cStatusIndi).Text = "D" Then
                e.Row.BackColor = Drawing.Color.FromArgb(255, 189, 121)

                CType(e.Row.FindControl("lnkDelete"), ImageButton).Enabled = False
                CType(e.Row.FindControl("lnkEdit"), ImageButton).Enabled = False

            End If

        End If
    End Sub
#End Region

    Protected Sub gvwUOM_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvwUOM.RowDeleting

    End Sub

    Protected Sub gvwUOM_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvwUOM.RowEditing
        e.Cancel = True
    End Sub

#Region "Web Method"

    <WebMethod>
    Public Shared Function AuditTrail(ByVal vUomCode As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_UomMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty

        Dim dtUomMstHistory As New DataTable
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
            vTableName = "UomMstHistory"
            vIdName = ""
            AuditFieldName = "vUOMCode"
            AuditFieldValue = vUomCode
            vOtherTableName = ""
            vOtherIdName = ""


            Dim Param As String = ""


            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName
            If Not objHelp.Proc_GetAuditTrail(wStr, ds_UomMst, Param) Then
                Throw New Exception(estr)
            End If

            If Not dtUomMstHistory Is Nothing Then
                dtUomMstHistory.Columns.Add("SrNo")
                dtUomMstHistory.Columns.Add("UOMClass")
                dtUomMstHistory.Columns.Add("UOMDesc")
                dtUomMstHistory.Columns.Add("Remarks")
                dtUomMstHistory.Columns.Add("ModifyBy")
                dtUomMstHistory.Columns.Add("ModifyOn")
            End If
            dt = ds_UomMst.Tables(0)
            Dim dv As New DataView(dt)

            For Each dr As DataRow In dv.ToTable.Rows

                drAuditTrail = dtUomMstHistory.NewRow()

                drAuditTrail("SrNo") = i
                drAuditTrail("UOMClass") = dr("vUOMClass").ToString()
                drAuditTrail("UOMDesc") = dr("vUOMDesc").ToString()
                drAuditTrail("Remarks") = dr("vRemarks").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("ModifyOn"))
                dtUomMstHistory.Rows.Add(drAuditTrail)
                dtUomMstHistory.AcceptChanges()
                i += 1
            Next
            strReturn = JsonConvert.SerializeObject(dtUomMstHistory)
            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function



    Protected Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_UomMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty

        Dim dtUomMstHistory As New DataTable
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
            vTableName = "UomMstHistory"
            vIdName = ""
            AuditFieldName = "vUOMCode"
            AuditFieldValue = hdnUOMCode.Value
            vOtherTableName = ""
            vOtherIdName = ""


            Dim Param As String = ""


            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName
            If Not objHelp.Proc_GetAuditTrail(wStr, ds_UomMst, Param) Then
                Throw New Exception(estr)
            End If

            If Not dtUomMstHistory Is Nothing Then
                dtUomMstHistory.Columns.Add("SrNo")
                dtUomMstHistory.Columns.Add("UOMClass")
                dtUomMstHistory.Columns.Add("UOMDesc")
                dtUomMstHistory.Columns.Add("Remarks")
                dtUomMstHistory.Columns.Add("ModifyBy")
                dtUomMstHistory.Columns.Add("ModifyOn")
            End If
            dt = ds_UomMst.Tables(0)
            Dim dv As New DataView(dt)

            For Each dr As DataRow In dv.ToTable.Rows

                drAuditTrail = dtUomMstHistory.NewRow()

                drAuditTrail("SrNo") = i
                drAuditTrail("UOMClass") = dr("vUOMClass").ToString()
                drAuditTrail("UOMDesc") = dr("vUOMDesc").ToString()
                drAuditTrail("Remarks") = dr("vRemarks").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("ModifyOn"))
                dtUomMstHistory.Rows.Add(drAuditTrail)
                dtUomMstHistory.AcceptChanges()
                i += 1
            Next



            gvExport.DataSource = dtUomMstHistory
            gvExport.DataBind()

            If gvExport.Rows.Count >= 0 Then


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

                'style to format numbers to string
                Dim style As String = "<style> .textmode { } </style>"
                Response.Write(style)



                Dim info As String = String.Empty
                Dim gridviewHtml As String = String.Empty

                filename = "Audit Trail_" + hdnUOMCode.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

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
                strMessage.Append("UOM Master-AuditTrail")
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
            Else
                Dim info As String = String.Empty
                Dim gridviewHtml As String = String.Empty

                filename = "Audit Trail_" + hdnUOMCode.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"


                drAuditTrail = dtUomMstHistory.NewRow()

                drAuditTrail("SrNo") = ""
                drAuditTrail("UOMClass") = ""
                drAuditTrail("UOMDesc") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("ModifyBy") = ""
                drAuditTrail("ModifyOn") = ""
                dtUomMstHistory.Rows.Add(drAuditTrail)
                dtUomMstHistory.AcceptChanges()

                gvExport.DataSource = dtUomMstHistory
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
                strMessage.Append("UOM Master-AuditTrail")
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
            Me.objcommon.ShowAlert("Error While Exporting Data To Excel : " + estr, Me.Page)

            Exit Sub
        End Try
    End Sub


    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    End Sub
#End Region

    Protected Sub btnRemarksUpdate_Click(sender As Object, e As EventArgs)
        Dim ds_Delete As New DataSet
        Dim estr As String = ""
        Dim dr As DataRow
        Dim dt_UOM As DataTable
        Dim wStr As String = String.Empty
        ' Dim ds As DataSet
        index = Me.gvwUOM.Rows(Gv_index).Cells(GVC_UOMCode).Text.Trim()
        Try
            wStr = "vUOMCode=" & index
            If Not objHelp.GetUOMMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    ds_Delete, estr) Then

                Me.ShowErrorMessage("Error While Getting Data From ReasonMst", estr)
                Exit Sub

            End If
            dt_UOM = ds_Delete.Tables(0)
            For Each dr In dt_UOM.Rows
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("vRemarks") = Me.txtRemarks_delete.Text.Trim()
                dr("cStatusIndi") = "D"
                dr.AcceptChanges()
            Next
            dt_UOM.AcceptChanges()
            dt_UOM.TableName = "UOMMst"

            ds_Delete = Nothing
            ds_Delete = New DataSet
            ds_Delete.Tables.Add(dt_UOM.Copy())
            If Not objLambda.Save_UOMMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_UOMMst, _
                    ds_Delete, Me.Session(S_UserID), estr) Then
                objcommon.ShowAlert("Error While Deleting UOMMst", Me.Page)
                Exit Sub
            End If

            ResetPage()
            objcommon.ShowAlert("Record Deleted SuccessFully !", Me.Page)
            txtRemarks_delete.Text = Nothing

            If Not Me.GenCall() Then
                Exit Sub
            End If


        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")

        End Try
    End Sub



End Class
