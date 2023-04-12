Imports Microsoft.Win32
Imports System.Web.Services
Imports Newtonsoft.Json
Imports Microsoft
Imports Microsoft.Office.Interop
Imports System.IO
Imports System.Drawing

Partial Class frmStageMst
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtStageMst As String = "dtstageMst"
    Private Const VS_StageId As String = "iStageId"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_Stagedesc As Integer = 1
    Private Const GVC_SeqNo As Integer = 2
    Private Const GVC_Modifyon As Integer = 3
    Private Const GVC_Edit As Integer = 4
    Private Const GVC_Id As Integer = 5

#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CType(Master.FindControl("lblHeading"), Label).Text = "Stage Master"
        If Not IsPostBack Then
            GenCall()
            Exit Sub
        End If
        Try
            If gvstage.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvstage", "UIgvstage(); ", True)
            End If
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean
        Dim dt_stageMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try
            Choice = Me.Request.QueryString("mode").ToString
            Me.ViewState(VS_Choice) = Choice   'To use it while saving
            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_StageId) = Me.Request.QueryString("Value").ToString
            End If
            ''''Check for Valid User''''''''''''''
            If Not GenCall_Data(Choice, dt_stageMst) Then ' For Data Retrieval
                Exit Function
            End If
            Me.ViewState(VS_DtStageMst) = dt_stageMst ' adding blank DataTable in viewstate
            If Not GenCall_ShowUI(Choice, dt_stageMst) Then 'For Displaying Data 
                Exit Function
            End If
            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideStageDetails", "HideStageDetails(); ", True)
            End If
            GenCall = True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....GenCall")
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_Data"

    Private Function GenCall_Data(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                            ByRef dt_Dist_Retu As DataTable) As Boolean

        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_stage As DataSet = Nothing
        Try

            If Choice_1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "iStageId=" + Me.ViewState(VS_StageId).ToString() 'Value of where condition
            End If

            If Not objHelp.GetStageMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_stage, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_stage Is Nothing Then
                Throw New Exception(eStr)
            End If

            If ds_stage.Tables(0).Rows.Count <= 0 And _
               Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                Throw New Exception("No Records Found For Selected role")

            End If
            dt_Dist_Retu = ds_stage.Tables(0)
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....GenCallData")
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_ShowUI"

    Private Function GenCall_ShowUI(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                                      ByVal dt_stageMst As DataTable) As Boolean
        Try
            Page.Title = ":: Stage Master :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            If Not FillStageMst() Then
                Exit Function
            End If

            If Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.txtName.Text = ConvertDbNullToDbTypeDefaultValue(dt_stageMst.Rows(0)("vStageDesc"), dt_stageMst.Rows(0)("vStageDesc").GetType)
                Me.ddlseqno.SelectedValue = ConvertDbNullToDbTypeDefaultValue(dt_stageMst.Rows(0)("iSequenceNo"), dt_stageMst.Rows(0)("iSequenceNo").GetType)
                btnSave.Text = "Update"
                btnSave.ToolTip = "Update"

            End If

            'Me.btnSave.Attributes.Add("OnClick", "return Validation();")
            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCallShowUI")
        Finally
        End Try
    End Function

#End Region

#Region "FillGrid"

    Private Function FillStageMst() As Boolean
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_StageMst As New DataSet
        Dim dv_view As New Data.DataView
        Try
            wStr = " cStatusIndi <> 'D'"
            If Not objHelp.GetStageMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_StageMst, eStr) Then
                Throw New Exception(eStr)
                Return False
            End If

            If ds_StageMst.Tables(0).Rows.Count < 1 Then
                objcommon.ShowAlert("No Record Found", Me.Page)
                Return True
            End If
            dv_view = ds_StageMst.Tables(0).Copy.DefaultView
            dv_view.Sort = "  vStageDesc "
            Me.gvstage.DataSource = dv_view
            Me.gvstage.DataBind()
            Me.ddlseqno.Items.Insert(0, New ListItem(" Select Sequence No", ""))
            If gvstage.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvstage", "UIgvstage(); ", True)
            End If
            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "..FillStageMst")
            Return False
        End Try

    End Function

    Private Sub BindGrid(ByVal grdName As String)
        Dim dsTemp As New DataSet
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty

        wStr = " cStatusIndi <> 'D'"
        If grdName = "gvstage" Then
            If objHelp.GetStageMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsTemp, eStr) Then
                gvstage.ShowFooter = False
                gvstage.DataSource = dsTemp
                gvstage.DataBind()
               
                dsTemp.Dispose()
            End If
        End If
    End Sub

#End Region

#Region "Reset Page"
    Private Sub ResetPage()
        Me.txtName.Text = ""
        Me.ddlseqno.SelectedIndex = 0
        Me.txtRemark.Text = ""
        Me.ViewState(VS_Choice) = Nothing
        Me.ViewState(VS_DtStageMst) = Nothing
        Me.ViewState(VS_StageId) = Nothing
        
    End Sub
#End Region

#Region "Assign Values"

    Private Function AssignUpdatedValues() As Boolean
        Dim dtOld As New DataTable
        Dim dr As DataRow
        Dim ds_Check As New DataSet
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Try

            'For validating Duplication
            wstr = "cStatusIndi <> 'D' And vStageDesc='" & Me.txtName.Text.Trim() & "'"

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                wstr += " And iStageId <> " + Me.ViewState(VS_StageId)
            End If

            If Not objHelp.GetStageMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_Check, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From StageMst", estr)
                Return False
            End If

            If ds_Check.Tables(0).Rows.Count > 0 Then
                objcommon.ShowAlert("Stage Name Already Exists !", Me.Page)
                Return False
            End If
            '***********************************

            dtOld = Me.ViewState(VS_DtStageMst)
            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                dtOld.Clear()
                dr = dtOld.NewRow
                dr("iStageId") = "0"
                dr("vStageDesc") = Me.txtName.Text.Trim
                dr("iSequenceNo") = Me.ddlseqno.SelectedValue.Trim
                dr("vRemark") = Me.txtRemark.Text.Trim
                dr("iModifyBy") = Session(S_UserID)
                dtOld.Rows.Add(dr)
            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                For Each dr In dtOld.Rows
                    dr("vStageDesc") = Me.txtName.Text.Trim
                    dr("iSequenceNo") = Me.ddlseqno.SelectedValue.Trim
                    dr("vRemark") = Me.txtRemark.Text.Trim
                    dr("iModifyBy") = Session(S_UserID)
                    dr("cStatusIndi") = "E"
                    dr.AcceptChanges()
                Next
                dtOld.AcceptChanges()
            End If
            Me.ViewState(VS_DtStageMst) = dtOld
            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...AssignUpdatedValues")
            Return False
        End Try
    End Function

#End Region

#Region "Button Events"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim Dt As New DataTable
        Dim Ds_stagesv As DataSet
        Dim Ds_stagemstgrid As New DataSet
        Dim eStr As String = String.Empty
        Dim objOPws As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()
        Dim message As String = String.Empty
        Try

            If Not AssignUpdatedValues() Then
                objcommon.ShowAlert("Error While Assign Values", Me.Page)
                Exit Sub
            End If

            Ds_stagesv = New DataSet
            Ds_stagesv.Tables.Add(CType(Me.ViewState(VS_DtStageMst), Data.DataTable).Copy())
            Ds_stagesv.Tables(0).TableName = "stageMst"   ' New Values on the form to be updated

            If Not objOPws.Save_StageMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_StageMst, Ds_stagesv, "1", eStr) Then
                objcommon.ShowAlert("Error While Saving StageMst", Me.Page)

                Exit Sub
            End If
            message = IIf(Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "Stage Details Saved Successfully !", "Stage Details Updated Successfully !")
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Alert", "ShowAlert('" + message + "')", True)


           
            ResetPage()
            
        Catch ex As Exception
            ShowErrorMessage(ex.Message, " ..... btnSave_Click")
        Finally
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ResetPage()
        Response.Redirect("frmStageMst.aspx?mode=1")
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Response.Redirect("frmMainPage.aspx")
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

#Region "GridView Events"

    Protected Sub gvstage_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or _
              e.Row.RowType = DataControlRowType.Header Or _
              e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(GVC_Id).Visible = False
        End If
    End Sub

    Protected Sub gvstage_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        gvstage.PageIndex = e.NewPageIndex
        'BindGrid("gvstage")
        FillStageMst()
    End Sub

    Protected Sub gvstage_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (Me.gvstage.PageSize * Me.gvstage.PageIndex) + 1
            CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandName = "Edit"
            CType(e.Row.FindControl("lnkAudit"), ImageButton).Attributes.Add("iStageId", e.Row.Cells(GVC_Id).Text)
        End If
    End Sub

    Protected Sub gvstage_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim i As Integer = e.CommandArgument
        If e.CommandName.ToUpper = "EDIT" Then
            Me.Response.Redirect("frmStageMst.aspx?mode=2&value=" & Me.gvstage.Rows(i).Cells(GVC_Id).Text.Trim())
        End If
    End Sub

#End Region

#Region "Web Method"

    <WebMethod> _
    Public Shared Function AuditTrail(ByVal iStageId As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_StageMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim dtStageMstHistory As New DataTable
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

            vTableName = "StageMstHistory"
            vIdName = ""
            AuditFieldName = "iStageId"
            AuditFieldValue = iStageId
            vOtherTableName = ""
            vOtherIdName = ""


            Dim Param As String = ""


            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName

            If Not objHelp.Proc_GetAuditTrail(wStr, ds_StageMst, Param) Then
                Throw New Exception(estr)
            End If

            If Not dtStageMstHistory Is Nothing Then
                dtStageMstHistory.Columns.Add("SrNo")
                dtStageMstHistory.Columns.Add("StageDesc")
                dtStageMstHistory.Columns.Add("SequenceNo")
                dtStageMstHistory.Columns.Add("Remark")
                dtStageMstHistory.Columns.Add("ModifyBy")
                dtStageMstHistory.Columns.Add("ModifyOn")
            End If

            dt = ds_StageMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtStageMstHistory.NewRow()
                drAuditTrail("SrNo") = i
                drAuditTrail("StageDesc") = dr("vStageDesc").ToString()
                drAuditTrail("SequenceNo") = dr("iSequenceNo").ToString()
                drAuditTrail("Remark") = dr("vRemark").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("ModifyOn"))
                dtStageMstHistory.Rows.Add(drAuditTrail)
                dtStageMstHistory.AcceptChanges()
                i += 1
            Next
            strReturn = JsonConvert.SerializeObject(dtStageMstHistory)
            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Protected Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_StageMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty

        Dim dtStageMstHistory As New DataTable
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

            vTableName = "StageMstHistory"
            vIdName = ""
            AuditFieldName = "iStageId"
            AuditFieldValue = hdnStageId.Value
            vOtherTableName = ""
            vOtherIdName = ""


            Dim Param As String = ""

            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName

            If Not objHelp.Proc_GetAuditTrail(wStr, ds_StageMst, Param) Then
                Throw New Exception(estr)
            End If

            If Not dtStageMstHistory Is Nothing Then
                dtStageMstHistory.Columns.Add("Sr. No")
                dtStageMstHistory.Columns.Add("Stage Name")
                dtStageMstHistory.Columns.Add("Sequence No")
                dtStageMstHistory.Columns.Add("Remarks")
                dtStageMstHistory.Columns.Add("ModifyBy")
                dtStageMstHistory.Columns.Add("ModifyOn")
            End If

            dtStageMstHistory.AcceptChanges()
            dt = ds_StageMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtStageMstHistory.NewRow()
                drAuditTrail("Sr. No") = i
                drAuditTrail("Stage Name") = dr("vStageDesc").ToString()
                drAuditTrail("Sequence No") = dr("iSequenceNo").ToString()
                drAuditTrail("Remarks") = dr("vRemark").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("ModifyOn"))
                dtStageMstHistory.Rows.Add(drAuditTrail)
                dtStageMstHistory.AcceptChanges()
                i += 1
            Next

            gvExport.DataSource = dtStageMstHistory
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

                filename = "Audit Trail_" + hdnStageId.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

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
                strMessage.Append("Stage Master-AuditTrail")
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

                filename = "Audit Trail_" + hdnStageId.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"


                drAuditTrail = dtStageMstHistory.NewRow()

                drAuditTrail("Sr. No") = i
                drAuditTrail("Stage Name") = ""
                drAuditTrail("Sequence No") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("ModifyBy") = ""
                drAuditTrail("ModifyOn") = ""
                dtStageMstHistory.Rows.Add(drAuditTrail)
                dtStageMstHistory.AcceptChanges()
                gvExport.DataSource = dtStageMstHistory
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
                strMessage.Append("Stage Master-AuditTrail")
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

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    End Sub

#End Region

    Protected Sub btnExportToExcelGrid_Click(sender As Object, e As EventArgs) Handles btnExportToExcelGrid.Click
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_StageMst As New DataSet
        Dim estr As String = String.Empty
        Dim dtStageMstHistory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable
        Dim StageId As String = ""
        Dim filename As String = String.Empty
        Dim strMessage As New StringBuilder

        Try
            wStr = "" + "##"
            ds_StageMst = objHelp.ProcedureExecute("Proc_StageMst ", wStr)
            wStr = String.Empty

            If Not dtStageMstHistory Is Nothing Then
                dtStageMstHistory.Columns.Add("Sr. No")
                dtStageMstHistory.Columns.Add("Stage Name")
                dtStageMstHistory.Columns.Add("Sequence No")
                dtStageMstHistory.Columns.Add("Remarks")
                dtStageMstHistory.Columns.Add("ModifyBy")
                dtStageMstHistory.Columns.Add("ModifyOn")
            End If

            dtStageMstHistory.AcceptChanges()
            dt = ds_StageMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtStageMstHistory.NewRow()
                drAuditTrail("Sr. No") = i
                drAuditTrail("Stage Name") = dr("vStageDesc").ToString()
                drAuditTrail("Sequence No") = dr("iSequenceNo").ToString()
                drAuditTrail("Remarks") = dr("vRemark").ToString()
                drAuditTrail("ModifyBy") = dr("iModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("dModifyOn"))
                dtStageMstHistory.Rows.Add(drAuditTrail)
                dtStageMstHistory.AcceptChanges()
                i += 1
            Next

            gvExportToExcel.DataSource = dtStageMstHistory
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

                filename = "Stage Master" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                Dim stringWriter As New System.IO.StringWriter()

                Dim writer As New HtmlTextWriter(stringWriter)
                gvExportToExcel.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()

                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Stage Master-AuditTrail")
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

                filename = "Stage Master" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"


                drAuditTrail = dtStageMstHistory.NewRow()

                drAuditTrail("Sr. No") = i
                drAuditTrail("Stage Name") = ""
                drAuditTrail("Sequence No") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("ModifyBy") = ""
                drAuditTrail("ModifyOn") = ""
                dtStageMstHistory.Rows.Add(drAuditTrail)
                dtStageMstHistory.AcceptChanges()
                gvExportToExcel.DataSource = dtStageMstHistory
                gvExportToExcel.DataBind()


                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(stringWriter)
                gvExportToExcel.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()


                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Stage Master-AuditTrail")
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
End Class