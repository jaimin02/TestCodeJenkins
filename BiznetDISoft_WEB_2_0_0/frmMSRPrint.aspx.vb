Imports System.Drawing
Imports System.Collections.Generic
Imports System.Collections
Imports System.Text
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports Winnovative
Imports System.Drawing.Imaging
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.DataVisualization.Charting

Partial Class frmMSRPrint
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION"

    Private objCommon As New clsCommon
    Private objHelpDbTable As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private rPage As RepoPage

    Private VS_SubjectInfo As String = "SubejctInfo"

#End Region

    Protected Sub btnGeneratePDF_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGeneratePDF.Click
        Dim htmlcontent As String = String.Empty
        Dim headercontent As String = String.Empty
        Dim downloadbytes As Byte()
        Dim d1 As Document
        Dim data As String = String.Empty
        Dim watermarkTextFont As System.Drawing.Font
        Dim watermarkTextElement As TextElement
        Dim stylesheetarraylist As New ArrayList
        Dim strProfileStatus As String = String.Empty
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_Check As New DataSet
        Try

            Dim pdfconverter As HtmlToPdfConverter = New HtmlToPdfConverter()
            pdfconverter.PdfDocumentOptions.AvoidImageBreak = True
            pdfconverter.PdfDocumentOptions.AvoidTextBreak = True

            'pdfconverter.TruncateOutOfBoundsText = True
            pdfconverter.PdfDocumentOptions.StretchToFit = True
            pdfconverter.LicenseKey = "dfvo+uv66OPj6vrr6PTq+unr9Ovo9OPj4+P66g=="
            pdfconverter.PdfDocumentOptions.TopMargin = 15
            pdfconverter.PdfDocumentOptions.BottomMargin = 30
            pdfconverter.PdfDocumentOptions.LeftMargin = 20

            pdfconverter.PdfHeaderOptions.HeaderHeight = 120
            'pdfconverter.PdfHeaderOptions.HeaderTextFontName = "verdana"
            'pdfconverter.PdfHeaderOptions.HeaderTextFontStyle = FontStyle.Bold
            'pdfconverter.PdfHeaderOptions.HeaderTextFontSize = 50
            'pdfconverter.PdfDocumentOptions.OptimizePdfPageBreaks = True
            'pdfconverter.PdfHeaderOptions.DrawHeaderLine = True
            'pdfconverter.PdfFooterOptions.DrawFooterLine = True
            'pdfconverter.PdfFooterOptions.FooterTextFontName = "verdana"
            'pdfconverter.PdfFooterOptions.PageNumberTextFontName = "verdana"
            'pdfconverter.PdfFooterOptions.PageNumberTextFontSize = 7

            pdfconverter.PdfDocumentOptions.JpegCompressionEnabled = False
            pdfconverter.PdfDocumentOptions.JpegCompressionLevel = 0
            pdfconverter.PdfDocumentOptions.ShowHeader = True
            pdfconverter.PdfDocumentOptions.ShowFooter = True
            'pdfconverter.PdfFooterOptions.ShowPageNumber = False
            'pdfconverter.PdfFooterOptions.PageNumberYLocation = 30
            'pdfconverter.PdfFooterOptions.PageNumberTextFontStyle = FontStyle.Bold

            ''pdfconverter.PdfFooterOptions.PageNumberingFormatString = "[Authenticated By: " + CType(ViewState(VS_AuthenticatedBy), String) _
            '' + "]  [Authenticated On: " + CType(ViewState(VS_AuthenticatedOn), String) + "]                  "

            pdfconverter.PdfFooterOptions.PageNumberingStartIndex = 0

            Dim Path As String = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.ToString.LastIndexOf("/"))
            ImgLogo.Src = Path + ImgLogo.Src.ToString.Replace("~", "")

            headercontent = Regex.Replace(Me.hfHeaderText.Value.ToString(), "<IMG[^>]+", "<IMG id=ImgLogo1 alt=" + ImgLogo.Src.ToString + " align=right src=" + ImgLogo.Src.ToString + " /", RegexOptions.IgnoreCase)
            Me.hdn.Value = Regex.Replace(Me.hdn.Value, "disabled=""true""", "").Replace("disabled", "")
            htmlcontent = Me.hdn.Value.ToString()

            Dim Header1 As New HtmlToPdfElement(headercontent, Nothing)
            pdfconverter.PdfHeaderOptions.AddElement(Header1)
            'Dim htmlHeader As New Winnovative.WnvHtmlConvert.HtmlToPdfArea(18.0, 0.0, headercontent, Nothing) '21.0, 0.0,
            'pdfconverter.PdfHeaderOptions.AddHtmlToPdfArea(htmlHeader)
            pdfconverter.PdfFooterOptions.PageNumberingStartIndex = 0
            pdfconverter.PdfFooterOptions.AddElement(New LineElement(0, 0, pdfconverter.PdfDocumentOptions.PdfPageSize.Width - pdfconverter.PdfDocumentOptions.LeftMargin - pdfconverter.PdfDocumentOptions.RightMargin, 0))
            Dim footerText As New TextElement(0, 15, "*This is an Electronically authenticated report.                                                                                                         Page &p; of &P; pages                   ", New Font(New FontFamily("Tahoma"), 8.5, GraphicsUnit.Point))
            footerText.TextAlign = HorizontalTextAlign.Right
            footerText.ForeColor = Color.Black
            footerText.EmbedSysFont = True
            pdfconverter.PdfFooterOptions.AddElement(footerText)

            'pdfconverter.PdfFooterOptions.FooterTextFontName = "Tahoma"
            'pdfconverter.PdfFooterOptions.FooterText = "       *This is an Electronically authenticated report.                                                                                                         Page &p; of &P; pages                   "

            btnGeneratePDF.Enabled = False
            d1 = pdfconverter.ConvertHtmlToPdfDocumentObject(htmlcontent, String.Empty)

            If Me.hfWaterMark.Value.ToString <> "0" Then
                watermarkTextFont = New System.Drawing.Font("HelveticaBold", 75, FontStyle.Bold, GraphicsUnit.Point)
                'watermarkTextFont = d1.AddFont(Winnovative.WnvHtmlConvert.PdfDocument.StdFontBaseFamily.HelveticaBold)
                'watermarkTextFont.Size = 75
                watermarkTextElement = New TextElement(100, 400, strProfileStatus + " Draft Copy", watermarkTextFont)
                watermarkTextElement.ForeColor = System.Drawing.Color.Blue
                watermarkTextElement.Opacity = 10
                watermarkTextElement.TextAngle = 45
                For Each pdfPage In d1.Pages
                    pdfPage.AddElement(watermarkTextElement)
                Next
            End If

            ' d1 = pdfconverter.GetPdfDocumentObjectFromHtmlString(htmlcontent)
            downloadbytes = d1.Save()

            Dim response As System.Web.HttpResponse = System.Web.HttpContext.Current.Response
            response.Clear()
            response.ContentType = "application/pdf"
            response.AddHeader("content-disposition", "attachment; filename=pdf1.pdf; size=" & downloadbytes.Length.ToString())
            response.Flush()
            response.BinaryWrite(downloadbytes)
            response.Flush()
            response.End()

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....btnGeneratePDF_Click")
        End Try

    End Sub

    Protected Sub btnExportSubjectInfo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExporttoExcel.Click
        Dim ds_Field As New DataSet
        Dim fileName As String = String.Empty
        Dim dt_Final As New DataTable
        Dim wStr As String = String.Empty
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()
        Dim objDataLogic As New clsDataLogic

        Try
            If Me.rblLog.SelectedValue = "GenericData" Then
                wStr = "select nMedExScreeningHdrNo,vSubjectId,vMySubjectNo,dScreenDate,vWorkSpaceId,cIsEligible,vInitials,TemplateID,cSex from  View_GenericScreeningMSRLog "
                wStr += " where vWorkspaceid='" + HProjectId.Value.ToString.Trim() + "'"
                ds_Field = objHelp.GetResultSet(wStr, "TempTable")
            End If
            If Me.rblLog.SelectedValue = "Data" Then
                wStr = " select nMedExScreeningHdrNo,vSubjectId,dScreenDate,vWorkSpaceId,cIsEligible,vInitials,cSex,TemplateID from View_ProjectSpecificScreeningLog "
                wStr += " where vWorkspaceid='" + HProjectId.Value.ToString.Trim() + "'"
                ds_Field = objHelp.GetResultSet(wStr, "TempTable")
            End If
            dt_Final = ds_Field.Tables(0).DefaultView.ToTable

            fileName = "Msr Report-" + Date.Today.ToString("dd-MMM-yyyy") + ".xls"
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()
            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)
            Context.Response.Write(ConvertDsuserTO(ds_Field))
            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....btnExportToExcel")
        End Try
    End Sub

    Private Function ConvertDsuserTO(ByVal ds As DataSet) As String
        Dim strMessage As New StringBuilder
        Dim i As Integer
        Dim iCol As Integer
        Dim j As Integer
        Dim dsConvert As New DataSet
        Dim Temp As String = String.Empty

        Try
            strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")
            strMessage.Append("<tr>")
            If rblLog.SelectedValue = "GenericData" Then
                strMessage.Append("<td colspan=""5""><center><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            End If
            If rblLog.SelectedValue = "Data" Then
                strMessage.Append("<td colspan=""4""><center><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            End If
            strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
            strMessage.Append("</font></strong><center></td>")
            strMessage.Append("</tr>")
            If rblLog.SelectedValue = "Data" Then
                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""4"" align=""Left""><Strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Project Specific-Screening MSR LOG")
                strMessage.Append("</font></strong><center></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<td colspan=""4"" align=""Left""><Strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Project No:" + Me.txtProject.Text.ToString.Trim().Split("]")(0).Split("[")(1))
                strMessage.Append("</font></strong><center></td>")
                strMessage.Append("<tr><td colspan=""4""></td></tr>")
                strMessage.Append("<tr>")
            End If
            If rblLog.SelectedValue = "GenericData" Then
                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""5"" align=""Left""><Strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Generic Screening MSR LOG")
                strMessage.Append("</font></strong><center></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<td colspan=""5"" align=""Left""><Strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Project No:" + Me.txtProject.Text.ToString.Trim().Split("]")(0).Split("[")(1))
                strMessage.Append("</font></strong><center></td>")
                strMessage.Append("<tr><td colspan=""5""></td></tr>")
                strMessage.Append("<tr>")
            End If
            If rblLog.SelectedValue = "Data" Then
                dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vSubjectId,vInitials,dScreenDate,cIsEligible".Split(",")).DefaultView.ToTable())
                dsConvert.AcceptChanges()
                dsConvert.Tables(0).Columns(0).ColumnName = "Subject Id"
                dsConvert.Tables(0).Columns(1).ColumnName = "Initials"
                dsConvert.Tables(0).Columns(2).ColumnName = "ScreenDate"
                dsConvert.Tables(0).Columns(3).ColumnName = "IsEligible"
                dsConvert.AcceptChanges()
            End If
            If Me.rblLog.SelectedValue = "GenericData" Then
                dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vSubjectId,vMySubjectNo,vInitials,dScreenDate,cIsEligible".Split(",")).DefaultView.ToTable())
                dsConvert.AcceptChanges()
                dsConvert.Tables(0).Columns(0).ColumnName = "Subject Id"
                dsConvert.Tables(0).Columns(1).ColumnName = "MySubejctNo"
                dsConvert.Tables(0).Columns(2).ColumnName = "Initials"
                dsConvert.Tables(0).Columns(3).ColumnName = "ScreenDate"
                dsConvert.Tables(0).Columns(4).ColumnName = "IsEligible"
                dsConvert.AcceptChanges()
            End If

            For iCol = 0 To dsConvert.Tables(0).Columns.Count - 1
                strMessage.Append("<td align=""left""><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(Convert.ToString(dsConvert.Tables(0).Columns(iCol)).Trim())
                strMessage.Append("</font></strong></td>")
                'End If
            Next
            strMessage.Append("</tr>")

            For j = 0 To dsConvert.Tables(0).Rows.Count - 1
                strMessage.Append("<tr>")
                For i = 0 To dsConvert.Tables(0).Columns.Count - 1
                    ''
                    strMessage.Append("<td align=""left""><font color=""#000000"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(Convert.ToString(dsConvert.Tables(0).Rows(j).Item(i)).Trim())
                    strMessage.Append("</font></td>")
                Next
                strMessage.Append("</tr>")
            Next
            strMessage.Append("</table>")
            Return strMessage.ToString
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....ConvertDsuserTO")
            Return ""
        End Try
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CType(Me.Master.FindControl("lblHeading"), Label).Text = "Screening Log"
        Page.Title = ":: MSR Print :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
    End Sub

#Region "Error Handler"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), System.Web.UI.WebControls.Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), System.Web.UI.WebControls.Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub

#End Region

End Class