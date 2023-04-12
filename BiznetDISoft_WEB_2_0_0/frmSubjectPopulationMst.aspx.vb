Imports System.Web.Services
Imports Newtonsoft.Json
Imports Microsoft
Imports Microsoft.Office.Interop
Imports System.IO
Imports System.Drawing

Partial Class frmSubjectPopulationMst
Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION "

    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()
    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtSubjectPopulationMst As String = "DtSubjectPopulationMst"
    Private Const VS_PopulationId As String = "PopulationId"
    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_PopulationId As Integer = 1
    Private Const GVC_Active As Integer = 3
    Private Const GVC_Remark As Integer = 4
    Private Const GVC_Edit As Integer = 5

#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            GenCall()
        End If
        If gvPopulation.Rows.Count > 0 Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvPopulation", "UIgvPopulation(); ", True)
        End If
    End Sub
#End Region

#Region "GenCall"
    Private Function GenCall() As Boolean
        Dim ds As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try
            Choice = Me.Request.QueryString("Mode")
            Me.ViewState(VS_Choice) = Choice
            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_PopulationId) = Me.Request.QueryString("Value").ToString
            End If

            If Not GenCall_Data(ds) Then
                Exit Function
            End If

            Me.ViewState(VS_DtSubjectPopulationMst) = ds.Tables("SubjectPopulationMst")

            If Not GenCall_ShowUI() Then
                Exit Function
            End If
            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HidePopulationDetails", "HidePopulationDetails(); ", True)
            End If
            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "........GenCall")

        Finally

        End Try

    End Function
#End Region

#Region "GenCall_Data"
    Private Function GenCall_Data(ByRef ds_DWR_Retu As DataSet) As Boolean

        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Val As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing

        Try

            Val = Me.ViewState(VS_PopulationId)
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "nPopulationId=" + Val.ToString
            End If
            If Not objHelp.getSubjectPopulationMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
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
            Me.ShowErrorMessage(ex.Message, "..........GenCallData")
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_ShowUI"
    Private Function GenCall_ShowUI() As Boolean
        Dim dt_SubjectPopulationMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Try
            Page.Title = " :: Subject Population Master ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Master.FindControl("lblHeading"), Label).Text = "Population Master"

            Page.Title = ":: Population Master   :: " + System.Configuration.ConfigurationManager.AppSettings("Client")


            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Population Master"

            dt_SubjectPopulationMst = Me.ViewState(VS_DtSubjectPopulationMst)

            If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.txtPopulationName.Text = dt_SubjectPopulationMst.Rows(0).Item("vPopulationName")
                Me.ChkActive.Checked = (dt_SubjectPopulationMst.Rows(0).Item("cActiveFlag").ToString.ToUpper.Trim() = "Y")
                btnSave.Text = "Update"
                btnSave.ToolTip = "Update"
            End If

            GenCall_ShowUI = True


            If Not FillGridPopulation() Then
                Exit Function
            End If


        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_ShowUI")
        Finally
        End Try

    End Function

#End Region

#Region "FillGridPopulation"

    Private Function FillGridPopulation() As Boolean
        Dim ds_SubjectPopulationMst As New Data.DataSet
        Dim dv_SubjectPopulationMst As New DataView
        Dim estr As String = String.Empty
        Try

            If Not objHelp.getSubjectPopulationMst("cStatusIndi <> 'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_SubjectPopulationMst, estr) Then
                Return False
            End If
            dv_SubjectPopulationMst = ds_SubjectPopulationMst.Tables(0).Copy.DefaultView()
            dv_SubjectPopulationMst.Sort = "nPopulationId"

            Me.gvPopulation.DataSource = dv_SubjectPopulationMst
            Me.gvPopulation.DataBind()

            If gvPopulation.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvPopulation", "UIgvPopulation(); ", True)
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "........FillGrid")
            Return False
        End Try
    End Function
#End Region

#Region "Reset Page"
    Private Sub ResetPage()
        Me.txtPopulationName.Text = ""
        Me.txtremark.Text = ""
        'Response.Redirect("frmSubjectPopulationMst.aspx?mode=1")
    End Sub
#End Region

#Region "Assign Function"

    Private Function AssignUpdatedValues() As Boolean
        Dim dr As DataRow
        Dim dt_Population As New DataTable
        Dim ds_Check As New DataSet
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Try

            dt_Population = Me.ViewState(VS_DtSubjectPopulationMst)
            wStr = "cStatusIndi <> 'D' And  upper(vPopulationName ) ='" & Me.txtPopulationName.Text.Trim.Replace("'", "''").ToUpper() & "'"
            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                wStr += " And nPopulationId <> '" + Me.ViewState(VS_PopulationId).ToString() + "'"
            End If

            If Not objHelp.getSubjectPopulationMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_Check, eStr) Then

                Me.ShowErrorMessage("Error While Getting Data From SubjectPopulationMst", eStr)
                Exit Function

            End If
            
            If ds_Check.Tables(0).Rows.Count > 0 Then

                objcommon.ShowAlert("Population Name Already Exists !", Me)
                If gvPopulation.Rows.Count > 0 Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvPopulation", "UIgvPopulation(); ", True)
                End If
                Exit Function

            End If

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                ds_Check.Clear()
                dr = ds_Check.Tables(0).NewRow
                dr("vPopulationName") = Me.txtPopulationName.Text.Trim
                dr("cActiveFlag") = IIf(Me.ChkActive.Checked, "Y", "N")
                dr("vRemarks") = Me.txtremark.Text.Trim
                dr("iModifyBy") = Session(S_UserID)
                ds_Check.Tables(0).Rows.Add(dr)

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                ds_Check.Clear()
                dr = ds_Check.Tables(0).NewRow
                dr("nPopulationId") = Me.ViewState(VS_PopulationId).ToString()
                dr("vPopulationName") = Me.txtPopulationName.Text.Trim
                dr("cActiveFlag") = IIf(Me.ChkActive.Checked, "Y", "N")
                dr("vRemarks") = Me.txtremark.Text.Trim
                dr("iModifyBy") = Session(S_UserID)
                dr("cStatusIndi") = "E"
                ds_Check.Tables(0).Rows.Add(dr)
                ds_Check.AcceptChanges()

            End If

            Me.ViewState(VS_DtSubjectPopulationMst) = ds_Check.Tables(0)
            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...AssignUpdatedValues")
            Return False
        End Try
    End Function
#End Region

#Region "Button Event"

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ResetPage()
        Me.Response.Redirect("frmSubjectPopulationMst.aspx?mode=1")
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnClose.Click
        Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_SubjectPopulationMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty

        Dim dtSubjectPopulationMstHistory As New DataTable
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

            wStr = "nPopulationId= '" + hdnPopulationId.Value + "' Order by nPopulationIdHistoryNo DESC"
            If Not objHelp.GetView_SubjectPopulationMstHistory(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubjectPopulationMst, estr) Then
                Throw New Exception(estr)
            End If

            If Not dtSubjectPopulationMstHistory Is Nothing Then
                dtSubjectPopulationMstHistory.Columns.Add("Sr. No")
                dtSubjectPopulationMstHistory.Columns.Add("Population Name")
                dtSubjectPopulationMstHistory.Columns.Add("Active")
                dtSubjectPopulationMstHistory.Columns.Add("Remarks")
                dtSubjectPopulationMstHistory.Columns.Add("ModifyBy")
                dtSubjectPopulationMstHistory.Columns.Add("ModifyOn")
            End If

            dtSubjectPopulationMstHistory.AcceptChanges()
            dt = ds_SubjectPopulationMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtSubjectPopulationMstHistory.NewRow()
                drAuditTrail("Sr. No") = i
                drAuditTrail("Population Name") = dr("vPopulationName").ToString()
                drAuditTrail("Active") = dr("cActiveFlag").ToString()
                drAuditTrail("Remarks") = dr("vRemarks").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("dModifyOn"))
                dtSubjectPopulationMstHistory.Rows.Add(drAuditTrail)
                dtSubjectPopulationMstHistory.AcceptChanges()
                i += 1
            Next

            gvExport.DataSource = dtSubjectPopulationMstHistory
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

                filename = "Audit Trail_" + hdnPopulationId.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

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





                strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Subject Population Master-AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td Align=""Right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")


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

                filename = "Audit Trail_" + hdnPopulationId.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"


                drAuditTrail = dtSubjectPopulationMstHistory.NewRow()
                drAuditTrail("Sr. No") = ""
                drAuditTrail("User Type") = ""
                drAuditTrail("Remark") = ""
                drAuditTrail("User Status") = ""
                drAuditTrail("UserName") = ""
                drAuditTrail("ModifyBy") = ""
                drAuditTrail("ModifyOn") = ""
                dtSubjectPopulationMstHistory.Rows.Add(drAuditTrail)
                dtSubjectPopulationMstHistory.AcceptChanges()
                gvExport.DataSource = dtSubjectPopulationMstHistory
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

                strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Subject Population Master-AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td align=""right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")


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

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim ds_SubjectPopulationMst As New DataSet
        Dim dt_SubjectPopulationMst As New DataTable
        Dim estr As String = String.Empty
        Dim message As String = String.Empty

        Try

            If Not AssignUpdatedValues() Then
                Exit Sub
            End If

            ds_SubjectPopulationMst = New DataSet
            dt_SubjectPopulationMst = (CType(Me.ViewState(VS_DtSubjectPopulationMst), Data.DataTable).Copy())
            dt_SubjectPopulationMst.TableName = "SubjectPopulationMst"
            ds_SubjectPopulationMst.Tables.Add(dt_SubjectPopulationMst)
            If Not objLambda.Save_SubjectPopulationMst(Me.ViewState(VS_Choice), ds_SubjectPopulationMst, Me.Session(S_UserID), estr) Then
                objcommon.ShowAlert("Error While Saving SubjectPopulationMst", Me.Page)
                Exit Sub
            End If
            ResetPage()
            message = IIf(Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "Population Details Saved Successfully !", "Population Details Updated Successfully !")
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Alert", "ShowAlert('" + message + "')", True)
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".......btnSave_Click")

        End Try
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    End Sub
#End Region

#Region "Grid Event"

    Protected Sub gvPopulation_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        gvPopulation.PageIndex = e.NewPageIndex
        If Not FillGridPopulation() Then
            Exit Sub
        End If
    End Sub

    Protected Sub gvPopulation_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        If e.CommandName.ToUpper = "EDIT" Then
            Me.Response.Redirect("frmSubjectPopulationMst.aspx?mode=2&value=" & Me.gvPopulation.Rows(e.CommandArgument).Cells(GVC_PopulationId).Text.Trim())
        End If
    End Sub

    Protected Sub gvPopulation_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        Dim strCellValue As String = ""
        If Not e.Row.RowType = DataControlRowType.Pager Then
            e.Row.Cells(GVC_PopulationId).Visible = False
            If e.Row.RowType = DataControlRowType.DataRow Then

                e.Row.Cells(GVC_Active).Text = IIf(e.Row.Cells(GVC_Active).Text = "Y", "Yes", "No")
                e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (gvPopulation.PageSize * gvPopulation.PageIndex) + 1
                CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandName = "Edit"
                CType(e.Row.FindControl("lnkAudit"), ImageButton).Attributes.Add("nPopulationId", e.Row.Cells(GVC_PopulationId).Text)


            End If
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

#Region "Web Method"
    <WebMethod> _
    Public Shared Function AuditTrail(ByVal nPopulationId As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_SubjectPopulationMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim dtSubjectPopulationMstHistory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable
        Try

            wStr = "nPopulationId = '" + nPopulationId + "' Order by nPopulationIdHistoryNo DESC"
            If Not objHelp.GetView_SubjectPopulationMstHistory(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubjectPopulationMst, estr) Then
                Throw New Exception(estr)
            End If

            If Not dtSubjectPopulationMstHistory Is Nothing Then
                dtSubjectPopulationMstHistory.Columns.Add("SrNo")
                dtSubjectPopulationMstHistory.Columns.Add("PopulationName")
                dtSubjectPopulationMstHistory.Columns.Add("Active")
                dtSubjectPopulationMstHistory.Columns.Add("Remarks")
                dtSubjectPopulationMstHistory.Columns.Add("ModifyBy")
                dtSubjectPopulationMstHistory.Columns.Add("ModifyOn")
            End If
            dt = ds_SubjectPopulationMst.Tables(0)
            Dim dv As New DataView(dt)

            For Each dr As DataRow In dv.ToTable.Rows

                drAuditTrail = dtSubjectPopulationMstHistory.NewRow()

                drAuditTrail("SrNo") = i
                drAuditTrail("PopulationName") = dr("vPopulationName").ToString()
                drAuditTrail("Active") = dr("cActiveFlag").ToString()
                drAuditTrail("Remarks") = dr("vRemarks").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("dModifyOn"))
                dtSubjectPopulationMstHistory.Rows.Add(drAuditTrail)
                dtSubjectPopulationMstHistory.AcceptChanges()
                i += 1
            Next
            strReturn = JsonConvert.SerializeObject(dtSubjectPopulationMstHistory)
            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
#End Region

End Class
