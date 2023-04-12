Imports Newtonsoft.Json
Imports System.Web.Services
Imports System.Drawing

Partial Class frmPrinterDtl
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private objlambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()

    Private Const VS_PrinterNo As String = "nPrinterID"
    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtPrinterDtl As String = "DtPrinterDtl"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_PrinterId As Integer = 1
    Private Const GVC_PrinterName As Integer = 2
    Private Const GVC_LocationName As Integer = 3
    Private Const GVC_Remarks As Integer = 4
    Private Const GVC_cStatusIndi As Integer = 5
    Private Const GVC_Edit As Integer = 6
    Private Const GVC_Delete As Integer = 7


    Private Shared Gv_index As Integer = 0
    Private Shared index As Integer = 0
    Private rPage As RepoPage


    Private Shared Status As String = String.Empty


#End Region

#Region "Form Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                GenCall()
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
            Choice = Me.Request.QueryString("mode")
            Me.ViewState(VS_Choice) = Choice   'To use it while saving

            Me.ViewState(VS_PrinterNo) = 0

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_PrinterNo) = Me.Request.QueryString("value").ToString
            End If

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtPrinterDtl) = ds.Tables("DocumentPrinterDtl")   ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If
            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HidePrinterDetail", "HidePrinterDetail(); ", True)
            End If
            GenCall = True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...GenCall", ex)
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

            Val = Me.ViewState(VS_PrinterNo) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "nPrinterId=" + Val.ToString
            End If

            If Not objHelp.GetDocumentPrinterDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
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
            Me.ShowErrorMessage(ex.Message, "...GenCallData", ex)

        Finally
        End Try
    End Function
#End Region

#Region "GenCall_ShowUI"
    Private Function GenCall_ShowUI() As Boolean
        Dim dt_PrinterDtl As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try
            Page.Title = ":: Printer Detail :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Printer Detail"

            dt_PrinterDtl = Me.ViewState(VS_DtPrinterDtl)

            Choice = Me.ViewState(VS_Choice)

            If Not FillGrid() Then
                Exit Function
            End If

            If Not FillLocation() Then
                Exit Function
            End If

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.DdlListLocation.SelectedValue = dt_PrinterDtl.Rows(0).Item("vLocationCode")
                Me.TxtPrinter.Text = dt_PrinterDtl.Rows(0).Item("vPrinterName")
            End If
            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "........GenCall_ShowUI", ex)
        End Try
    End Function

#End Region

#Region "Fill Location Drop Down List"

    Public Function FillLocation() As Boolean
        Dim ds_Location As New DataSet
        Dim Wstr As String = String.Empty
        Dim eStr_Retu As String = String.Empty

        Try
            Wstr = "cLocationType='L' and cStatusIndi <>'D' "

            If Not objHelp.getLocationMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Location, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If

            If ds_location.Tables(0).Rows.Count > 0 Then
                Me.DdlListLocation.DataSource = ds_Location
                Me.DdlListLocation.DataTextField = "vLocationName"
                Me.DdlListLocation.DataValueField = "vLocationCode"
                Me.DdlListLocation.DataBind()
            End If
            Me.DdlListLocation.Items.Insert(0, New ListItem("Please Select Location", ""))
            Return True
        Catch ex As Exception
            Me.objcommon.ShowAlert(ex.Message.ToString, Me.Page)
            Exit Function
        End Try
    End Function

#End Region

#Region "FillGrid"
    Private Function FillGrid() As Boolean
        Dim ds_Printer As New Data.DataSet
        Dim estr As String = String.Empty
        Dim wStr As String = String.Empty

        Try
            If Not objHelp.View_GetDocumentPrinterDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, _
                        ds_Printer, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From View_GetDocumentPrinterDtl", estr)
                Return False
            End If

            If ds_Printer.Tables(0).Rows.Count < 1 Then
                objcommon.ShowAlert("No Record Found", Me.Page)
                Return True
            End If
            Me.GV_Printer.DataSource = ds_Printer.Tables(0)
            Me.GV_Printer.DataBind()

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...Fill Grid", ex)
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
            dt_Save = CType(Me.ViewState(VS_DtPrinterDtl), DataTable)
            dt_Save.TableName = "DocumentPrinterDtl"
            ds_Save.Tables.Add(dt_Save)

            If Not objlambda.Save_PrinterDtl(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_PrinterDtl, _
                    ds_Save, Me.Session(S_UserID), estr) Then
                objcommon.ShowAlert("Error While Saving UserMst", Me.Page)
                Exit Sub
            End If

            message = IIf(Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "Printer Details Saved Successfully!", "Printer Details Updated Successfully!")
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Alert", "ShowAlert('" + message + "')", True)

            ResetPage()
            If Not GenCall() Then
                objcommon.ShowAlert("Error while getting DocumentPrinterDtl", Me.Page())
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...BtnSave_Click", ex)
        End Try
    End Sub

    Protected Sub btnRemarksUpdate_Click(sender As Object, e As EventArgs)
        Dim ds_Delete As New DataSet
        Dim estr As String = ""
        Dim dr As DataRow
        Dim dt_Printer As New DataTable
        Dim wStr As String = String.Empty

        index = Me.GV_Printer.Rows(Gv_index).Cells(GVC_PrinterId).Text.Trim()

        Try
            wStr = "nPrinterId=" & index
            If Not objHelp.GetDocumentPrinterDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    ds_Delete, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From ReasonMst", estr)
                Exit Sub
            End If

            dt_Printer = ds_Delete.Tables(0)
            For Each dr In dt_Printer.Rows
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("vRemarks") = Me.txtRemarks_delete.Text.Trim()
                dr("cStatusIndi") = "D"
                dr.AcceptChanges()
            Next
            dt_Printer.AcceptChanges()
            dt_Printer.TableName = "DocumentPrinterDtl"
            ds_Delete = Nothing
            ds_Delete = New DataSet
            ds_Delete.Tables.Add(dt_Printer.Copy())
            If Not objlambda.Save_PrinterDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_PrinterDtl, _
                    ds_Delete, Me.Session(S_UserID), estr) Then
                objcommon.ShowAlert("Error While Deleting DocumentPrinterDtl", Me.Page)
                Exit Sub
            End If
            ResetPage()
            objcommon.ShowAlert("Printer Deleted SuccessFully!", Me.Page)
            txtRemarks_delete.Text = Nothing

            If Not Me.GenCall() Then
                Exit Sub
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnRemarksUpdate_Click", ex)
        End Try
    End Sub

    Protected Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        'Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable
        Dim wStr As String = String.Empty
        Dim ds_Printer As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim dtDocumentPrinterDtlHistory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable
        Dim filename As String = String.Empty
        Dim strMessage As New StringBuilder

        Try
            wStr = " nPrinterId = '" + hdnPrinterId.Value + "' Order by nPrinterDtlId DESC"
            If Not objHelp.View_GetPrinterAuditTrail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Printer, estr) Then
                Throw New Exception(estr)
            End If

            If Not dtDocumentPrinterDtlHistory Is Nothing Then
                dtDocumentPrinterDtlHistory.Columns.Add("SrNo")
                dtDocumentPrinterDtlHistory.Columns.Add("Printer Name")
                dtDocumentPrinterDtlHistory.Columns.Add("Location Name")
                dtDocumentPrinterDtlHistory.Columns.Add("Remarks")
                dtDocumentPrinterDtlHistory.Columns.Add("ModifyBy")
                dtDocumentPrinterDtlHistory.Columns.Add("ModifyOn")
            End If
            dtDocumentPrinterDtlHistory.AcceptChanges()
            dt = ds_Printer.Tables(0)
            Dim dv As New DataView(dt)

            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtDocumentPrinterDtlHistory.NewRow()
                drAuditTrail("SrNo") = i
                drAuditTrail("Printer Name") = dr("vPrinterName").ToString()
                drAuditTrail("Location Name") = dr("vLocationName").ToString()
                drAuditTrail("Remarks") = dr("vRemarks").ToString()
                drAuditTrail("ModifyBy") = dr("iModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("dModifyOn"))
                dtDocumentPrinterDtlHistory.Rows.Add(drAuditTrail)
                dtDocumentPrinterDtlHistory.AcceptChanges()
                i += 1
            Next
            gvExport.DataSource = dtDocumentPrinterDtlHistory
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

                filename = "Audit Trail_" + hdnPrinterId.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

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
                strMessage.Append("Printer - AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td align = ""right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")

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

                filename = "Audit Trail_" + hdnPrinterId.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                drAuditTrail = dtDocumentPrinterDtlHistory.NewRow()

                drAuditTrail("SrNo") = ""
                drAuditTrail("Reason Desc") = ""
                drAuditTrail("ActivityName") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("ModifyBy") = ""
                drAuditTrail("ModifyOn") = ""
                dtDocumentPrinterDtlHistory.Rows.Add(drAuditTrail)
                dtDocumentPrinterDtlHistory.AcceptChanges()
                gvExport.DataSource = dtDocumentPrinterDtlHistory
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
                strMessage.Append("Printer-AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td align = ""right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")


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

    Protected Sub btnExportToExcelGrid_Click(sender As Object, e As EventArgs) Handles btnExportToExcelGrid.Click
        'Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable
        Dim wStr As String = String.Empty
        Dim ds_Printer As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim dtDocumentPrinterDtl As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable
        Dim filename As String = String.Empty
        Dim strMessage As New StringBuilder

        Try
            If Not objHelp.View_GetDocumentPrinterDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, _
                        ds_Printer, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From View_GetDocumentPrinterDtl", estr)
                Exit Sub
            End If

            If Not dtDocumentPrinterDtl Is Nothing Then
                dtDocumentPrinterDtl.Columns.Add("Sr. No")
                dtDocumentPrinterDtl.Columns.Add("Printer Name")
                dtDocumentPrinterDtl.Columns.Add("Location Name")
                dtDocumentPrinterDtl.Columns.Add("Remarks")
                dtDocumentPrinterDtl.Columns.Add("Modify By")
                dtDocumentPrinterDtl.Columns.Add("Modify On")
            End If

            dtDocumentPrinterDtl.AcceptChanges()
            dt = ds_Printer.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtDocumentPrinterDtl.NewRow()
                drAuditTrail("Sr. No") = i
                drAuditTrail("Printer Name") = dr("vPrinterName").ToString()
                drAuditTrail("Location Name") = dr("vLocationName").ToString()
                drAuditTrail("Remarks") = dr("vRemarks").ToString()
                drAuditTrail("Modify By") = dr("iModifyBy").ToString()
                drAuditTrail("Modify On") = Convert.ToString(dr("dModifyOn"))
                dtDocumentPrinterDtl.Rows.Add(drAuditTrail)
                dtDocumentPrinterDtl.AcceptChanges()
                i += 1
            Next

            gvExportToExcel.DataSource = dtDocumentPrinterDtl
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

                filename = "Printer Audit Trail" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

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
                strMessage.Append("Printer Detail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td align = ""right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")


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

                filename = "Printer Audit Trail" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                drAuditTrail = dtDocumentPrinterDtl.NewRow()
                drAuditTrail("Sr. No") = ""
                drAuditTrail("Printer Name") = ""
                drAuditTrail("Location Name") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("Modify By") = ""
                drAuditTrail("Modify On") = ""
                dtDocumentPrinterDtl.Rows.Add(drAuditTrail)
                dtDocumentPrinterDtl.AcceptChanges()
                gvExportToExcel.DataSource = dtDocumentPrinterDtl
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
                strMessage.Append("Project Group Master")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td align = ""right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")

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

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ResetPage()
        Me.Response.Redirect("frmPrinterDtl.aspx?mode=1")
    End Sub

#End Region

#Region "Assign values"

    Private Function AssignValues() As Boolean
        Dim dr As DataRow
        Dim dt_PrinterDtl As New DataTable
        Dim ds_Check As New DataSet
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Try

            wstr = "cStatusIndi <> 'D' And vLocationCode='" & Me.DdlListLocation.SelectedValue.ToString().Trim() & "' And vPrinterName ='" & Me.TxtPrinter.Text.Trim() & "'"

            If Not objHelp.GetDocumentPrinterDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Check, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From DocumentPrinterDtl", estr)
                Exit Function
            End If

            If ds_Check.Tables(0).Rows.Count > 0 Then
                objcommon.ShowAlert("Printer Already Exists for Selected Location!", Me.Page)
                Exit Function
            End If
            
            dt_PrinterDtl = CType(Me.ViewState(VS_DtPrinterDtl), DataTable)

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                dt_PrinterDtl.Clear()
                dr = dt_PrinterDtl.NewRow()
                dr("nPrinterId") = 0
                dr("vLocationCode") = Convert.ToString(Me.DdlListLocation.SelectedValue).Trim()
                dr("vPrinterName") = Me.TxtPrinter.Text.Trim()
                dr("vRemarks") = Me.txtRemarks.Text.Trim()
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dt_PrinterDtl.Rows.Add(dr)

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                For Each dr In dt_PrinterDtl.Rows
                    dr("vLocationCode") = Convert.ToString(Me.DdlListLocation.SelectedValue).Trim()
                    dr("vPrinterName") = Me.TxtPrinter.Text.Trim()
                    dr("vRemarks") = Me.txtRemarks.Text.Trim()
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "E"
                    dr.AcceptChanges()
                Next
                dt_PrinterDtl.AcceptChanges()
            End If

            Me.ViewState(VS_DtPrinterDtl) = dt_PrinterDtl
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...AssignValue()", ex)
            Return False
        End Try
    End Function

#End Region

#Region "Reset Page"

    Private Sub ResetPage()
        Try
            Me.DdlListLocation.SelectedIndex = -1
            Me.TxtPrinter.Text = ""
            Me.ViewState(VS_DtPrinterDtl) = Nothing
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...ResetPage()", ex)
        End Try
    End Sub

#End Region

#Region "Grid Event"

    Protected Sub GV_Printer_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GV_Printer.PageIndexChanging
        Try
            GV_Printer.PageIndex = e.NewPageIndex
            If Not FillGrid() Then
                Exit Sub
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GV_Printer_PageIndexChanging", ex)
        End Try
    End Sub

    Protected Sub GV_Printer_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Try
            Gv_index = e.CommandArgument
            Dim wStr As String = String.Empty
            Dim Ds As New DataSet
            Dim eStr As String = String.Empty
            Dim Dr As DataRow

            index = Me.GV_Printer.Rows(Gv_index).Cells(GVC_PrinterId).Text.Trim()
            If e.CommandName.ToUpper = "EDIT" Then

                wStr = "nPrinterId=" & index
                If Not objHelp.GetDocumentPrinterDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds, eStr) Then
                    objcommon.ShowAlert(eStr, Me.Page())
                End If

                If Ds.Tables(0).Rows.Count > 0 Then
                    Dr = Ds.Tables(0).Rows(0)
                    Me.DdlListLocation.SelectedValue = Dr.Item("vLocationCode")
                    Me.TxtPrinter.Text = Dr.Item("vPrinterName")
                    Me.ViewState(VS_PrinterNo) = Dr.Item("nPrinterId")
                    Me.BtnSave.Text = "Update"
                    Me.BtnSave.ToolTip = "Update"
                End If

                Me.ViewState(VS_DtPrinterDtl) = Ds.Tables(0)
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit

            ElseIf e.CommandName.ToUpper = "DELETE" Then
                Status = "DELETE"

                btnRemarksUpdate.Text = "Delete"
                btnRemarksUpdate.ToolTip = "Delete"
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete

                Gv_index = e.CommandArgument
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GV_Printer_RowCommand", ex)
        End Try
    End Sub

    Protected Sub GV_Printer_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Try

            e.Row.Cells(GVC_PrinterId).Visible = False
            e.Row.Cells(GVC_cStatusIndi).Visible = False


            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + 1 + (Me.GV_Printer.PageSize * Me.GV_Printer.PageIndex)
                CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandName = "Edit"
                CType(e.Row.FindControl("lnkDelete"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("lnkDelete"), ImageButton).CommandName = "Delete"
                CType(e.Row.FindControl("lnkAudit"), ImageButton).Attributes.Add("nPrinterId", e.Row.Cells(GVC_PrinterId).Text.Trim)

                If e.Row.Cells(GVC_cStatusIndi).Text = "D" Then
                    e.Row.BackColor = Drawing.Color.FromArgb(255, 189, 121)

                    CType(e.Row.FindControl("lnkDelete"), ImageButton).Enabled = False
                    CType(e.Row.FindControl("lnkEdit"), ImageButton).Enabled = False

                End If
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GV_Reason_RowDataBound", ex)
        End Try
    End Sub

    Protected Sub GV_Printer_PreRender(sender As Object, e As EventArgs) Handles GV_Printer.PreRender

    End Sub

    Protected Sub GV_Printer_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GV_Printer.RowDeleting

    End Sub

    Protected Sub GV_Printer_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GV_Printer.RowEditing
        e.Cancel = True
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
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

#Region "Audit Trail"

    <WebMethod> _
    Public Shared Function AuditTrail(ByVal nPrinterId As String) As String
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_Printer As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim dtDocumentPrinterDtlHistory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable

        Try
            wStr = " nPrinterId = '" + nPrinterId + "' Order by nPrinterDtlId DESC"
            If Not objHelp.View_GetPrinterAuditTrail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Printer, estr) Then
                Throw New Exception(estr)
            End If

            If Not dtDocumentPrinterDtlHistory Is Nothing Then
                dtDocumentPrinterDtlHistory.Columns.Add("SrNo")
                dtDocumentPrinterDtlHistory.Columns.Add("Printer")
                dtDocumentPrinterDtlHistory.Columns.Add("Location")
                dtDocumentPrinterDtlHistory.Columns.Add("Remarks")
                dtDocumentPrinterDtlHistory.Columns.Add("ModifyBy")
                dtDocumentPrinterDtlHistory.Columns.Add("ModifyOn")
            End If
            dt = ds_Printer.Tables(0)
            Dim dv As New DataView(dt)

            For Each dr As DataRow In dv.ToTable.Rows

                drAuditTrail = dtDocumentPrinterDtlHistory.NewRow()

                drAuditTrail("SrNo") = i
                drAuditTrail("Printer") = dr("vPrinterName").ToString()
                drAuditTrail("Location") = dr("vLocationName").ToString()
                drAuditTrail("Remarks") = dr("vRemarks").ToString()
                drAuditTrail("ModifyBy") = dr("iModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("dModifyOn"))
                dtDocumentPrinterDtlHistory.Rows.Add(drAuditTrail)
                dtDocumentPrinterDtlHistory.AcceptChanges()
                i += 1
            Next
            strReturn = JsonConvert.SerializeObject(dtDocumentPrinterDtlHistory)
            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

#End Region

End Class
