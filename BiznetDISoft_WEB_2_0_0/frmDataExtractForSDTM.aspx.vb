Imports Newtonsoft.Json
Imports System.Drawing

Partial Class frmDataExtractForSDTM
    Inherits System.Web.UI.Page

#Region "Variable Declaration "
    Private objCommon As New clsCommon
    Private objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

#End Region

#Region "Page Event"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            Me.Page.Title = " :: SDTM Data Export ::" + System.Configuration.ConfigurationManager.AppSettings("Client")
            If Not Page.IsPostBack Then
                If Not GenCall_Data() Then
                    Throw New Exception("Error While calling GenCall_Data()")
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "GenCall Data"
    Private Function GenCall_Data() As Boolean
        Try
            Page.Title = ":: CDASH/SDTM Data Export  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Master.FindControl("lblHeading"), Label).Text = "CDASH/SDTM Data Export"

            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID) & ""
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage("", ex.Message)
            Return False
        End Try
    End Function
#End Region

#Region "GridView Event"

#Region "SDTM"

    Protected Sub gvwSDTMData_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)

    End Sub

    Protected Sub gvwSDTMData_RowCommand(sender As Object, e As GridViewCommandEventArgs)

    End Sub

    Protected Sub gvwSDTMData_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub gvwSDTMData_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

#End Region

#Region "cDASH"

    Protected Sub gvwCDASH_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)

    End Sub

    Protected Sub gvwCDASH_RowCommand(sender As Object, e As GridViewCommandEventArgs)

    End Sub

    Protected Sub gvwCDASH_RowDataBound(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub gvwCDASH_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

#End Region

#Region "OTHER'"
    Protected Sub gvwOther_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)

    End Sub

    Protected Sub gvwOther_RowCommand(sender As Object, e As GridViewCommandEventArgs)

    End Sub

    Protected Sub gvwOther_RowCreated(sender As Object, e As GridViewRowEventArgs)

    End Sub

    Protected Sub gvwOther_RowDataBound(sender As Object, e As GridViewRowEventArgs)



    End Sub

#End Region

#End Region

#Region "Button CLick"
    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Try
            If Not FillDropDownList() Then
                Me.ShowErrorMessage("Error Wihle Fill Domain", "")
                Exit Sub
            End If
            gvwCDASH.DataSource = Nothing
            gvwCDASH.DataBind()
            gvwExport.DataSource = Nothing
            gvwExport.DataBind()
            gvwSDTMData.DataSource = Nothing
            gvwSDTMData.DataBind()
            gvwOther.DataSource = Nothing
            gvwOther.DataBind()
            fldSDTM.Attributes.Add("style", "display:none")
            fldcDesc.Attributes.Add("style", "display:none")
            fldOther.Attributes.Add("style", "display:none")



        Catch ex As Exception
            Me.ShowErrorMessage("Error Wihle set Project", ex.Message)
        End Try
    End Sub

    Protected Sub btnView_Click(sender As Object, e As EventArgs) Handles btnView.Click
        Try
            If Not FillGrid() Then
                Me.ShowErrorMessage("Error Wihle Fill Data", "")
                Exit Sub
            End If
        Catch ex As Exception
            Me.ShowErrorMessage("Error Wihle Data Extract", ex.Message)
        End Try

    End Sub

    Protected Sub btnClose_click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Response.Redirect("frmmainpage.aspx")
    End Sub

    Protected Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        Response.Redirect("~/frmDataExtractForSDTM.aspx")


    End Sub

    Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        ddlExportType.SelectedIndex = 0
        Me.MPExport.Show()

    End Sub

    Protected Sub btnDownLoad_Click(sender As Object, e As EventArgs) Handles btnDownLoad.Click
        Dim ds_ExTractDataSDTM As DataSet = Nothing
        Dim wStr As String = ""
        Dim fileName As String = String.Empty
        Dim strMessage As New StringBuilder
        Try
            wStr = HProjectId.Value + "##" + ddlDomain.SelectedValue + "##" + Convert.ToString(ddlExportType.SelectedValue) + "##" + hdnRequiredColumn.Value
            ds_ExTractDataSDTM = Me.objhelpDb.ProcedureExecute("dbo.Proc_GetSDTMTabulationDataForExcel", wStr)

            If Not ds_ExTractDataSDTM Is Nothing AndAlso ds_ExTractDataSDTM.Tables.Count > 0 AndAlso ds_ExTractDataSDTM.Tables(0).Rows.Count > 0 Then
                If ddlExportType.SelectedIndex <> 1 Then
                    For i As Integer = 0 To ds_ExTractDataSDTM.Tables(0).Rows.Count - 1
                        ds_ExTractDataSDTM.Tables(0).Rows(i)("QORIG") = txtQRIG.Text
                        ds_ExTractDataSDTM.Tables(0).Rows(i)("QEVAL") = txtQEVAL.Text
                    Next
                End If
            End If




            gvwExport.DataSource = ds_ExTractDataSDTM
            gvwExport.DataBind()

            gvwExport.HeaderRow.BackColor = Color.White
            gvwExport.FooterRow.BackColor = Color.White
            For Each cell As TableCell In gvwExport.HeaderRow.Cells
                cell.BackColor = gvwExport.HeaderStyle.BackColor
            Next
            For Each row As GridViewRow In gvwExport.Rows
                row.BackColor = Color.White
                row.Height = 20
                For Each cell As TableCell In row.Cells
                    If row.RowIndex Mod 2 = 0 Then
                        cell.BackColor = gvwExport.AlternatingRowStyle.BackColor
                        cell.ForeColor = gvwExport.RowStyle.ForeColor
                    Else
                        cell.BackColor = gvwExport.RowStyle.BackColor
                        cell.ForeColor = gvwExport.RowStyle.ForeColor
                    End If
                    cell.CssClass = "textmode"
                Next
            Next

            'style to format numbers to string
            Dim style As String = "<style> .textmode { } </style>"
            Response.Write(style)



            Dim info As String = String.Empty
            Dim gridviewHtml As String = String.Empty
            If ddlExportType.SelectedIndex = 1 Then
                fileName = " SDTM_" + "" + Convert.ToString(ddlDomain.SelectedItem) + "" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"
            Else
                fileName = " SUPP_" + "" + Convert.ToString(ddlDomain.SelectedItem) + "" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"
            End If



            Dim stringWriter As New System.IO.StringWriter()

            Dim writer As New HtmlTextWriter(stringWriter)
            gvwExport.RenderControl(writer)
            gridviewHtml = stringWriter.ToString()
            Dim colspan As Integer = ds_ExTractDataSDTM.Tables(0).Columns.Count

            strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=" + colspan.ToString() + "><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
            strMessage.Append("</font></strong><center></b></td>")
            strMessage.Append("</tr>")





            strMessage.Append("<td colspan=" + colspan.ToString() + "><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            If ddlExportType.SelectedIndex = 1 Then
                strMessage.Append("SDTM Data")
            ElseIf ddlExportType.SelectedIndex = 2 Then
                strMessage.Append("CDASH Data")
            ElseIf ddlExportType.SelectedIndex = 3 Then
                strMessage.Append("OTHER Data")
            End If

            strMessage.Append("</font></strong><center></b></td>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td colspan=" + Convert.ToString(colspan - 3) + " align = ""Right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")


            gridviewHtml = strMessage.ToString() + gridviewHtml
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
            Context.Response.AddHeader("content-Disposition", "attachment; filename=" + fileName)
            Context.Response.AddHeader("Content-Length", gridviewHtml.Length)

            Context.Response.Write(gridviewHtml)
            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)

        Catch ex As Exception

        End Try


    End Sub


#End Region

#Region "Error Handler"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub

#End Region

#Region "Fill Operation"
    Public Function FillDropDownList() As Boolean
        Dim wStr As String = ""
        Dim ds_Domain As DataSet
        Try
            wStr = HProjectId.Value
            ds_Domain = Me.objhelpDb.ProcedureExecute("dbo.Proc_getDomainName", wStr)

            If Not ds_Domain Is Nothing AndAlso ds_Domain.Tables(0).Rows.Count > 0 Then
                ddlDomain.DataSource = ds_Domain
                Me.ddlDomain.DataValueField = "vMedExGroupCode"
                Me.ddlDomain.DataTextField = "vMedExGroupDesc"
                ddlDomain.DataBind()
            End If
            ddlDomain.Items.Insert(0, "Select Domain")

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("Error Wihle Fill Domain", ex.Message)
            Return False
        End Try
    End Function

    Public Function FillGrid() As Boolean
        Dim ds_ExTractDataSDTM As DataSet = Nothing
        Dim ds_ExTractDataCDash As DataSet = Nothing
        Dim ds_ExTractDataOther As DataSet = Nothing
        Dim wStr As String = ""
        Dim sdtm As Boolean = False
        Dim cDash As Boolean = False
        Dim Other As Boolean = False

        Try

            gvwSDTMData.DataSource = Nothing
            gvwSDTMData.DataBind()
            gvwCDASH.DataSource = Nothing
            gvwCDASH.DataBind()
            gvwOther.DataSource = Nothing
            gvwOther.DataBind()


            wStr = HProjectId.Value + "##" + ddlDomain.SelectedValue + "##" + "S"
            ds_ExTractDataSDTM = Me.objhelpDb.ProcedureExecute("dbo.Proc_GetSDTMTabulationData", wStr)

            wStr = HProjectId.Value + "##" + ddlDomain.SelectedValue + "##" + "C"
            ds_ExTractDataCDash = Me.objhelpDb.ProcedureExecute("dbo.Proc_GetSDTMTabulationData", wStr)


            wStr = HProjectId.Value + "##" + ddlDomain.SelectedValue + "##" + "O"
            ds_ExTractDataOther = Me.objhelpDb.ProcedureExecute("dbo.Proc_GetSDTMTabulationData", wStr)

            If Not ds_ExTractDataSDTM Is Nothing AndAlso ds_ExTractDataSDTM.Tables.Count > 0 AndAlso ds_ExTractDataSDTM.Tables(0).Rows.Count > 0 Then
                fldSDTM.Style.Remove("display")
                fldSDTM.Attributes.Add("style", "width: 85%; margin: auto; margin-top: 20px;")
                gvwSDTMData.DataSource = ds_ExTractDataSDTM
                gvwSDTMData.DataBind()
                sdtm = True

            End If

            If Not ds_ExTractDataCDash Is Nothing AndAlso ds_ExTractDataCDash.Tables.Count > 0 AndAlso ds_ExTractDataCDash.Tables(0).Rows.Count > 0 Then
                fldcDesc.Style.Remove("display")
                fldcDesc.Attributes.Add("style", "width: 85%; margin: auto; margin-top: 20px;")
                gvwCDASH.DataSource = ds_ExTractDataCDash
                gvwCDASH.DataBind()
                cDash = True
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "displayProjectInfo11", "ClosedisplayProjectInfo();", True)
            End If

            If Not ds_ExTractDataOther Is Nothing AndAlso ds_ExTractDataOther.Tables.Count > 0 AndAlso ds_ExTractDataOther.Tables(0).Rows.Count > 0 Then
                fldOther.Attributes.Add("style", "width: 85%; margin: auto; margin-top: 20px;")
                gvwOther.DataSource = ds_ExTractDataOther
                gvwOther.DataBind()
                Other = True
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "displayProjectInfo11", "ClosedisplayProjectInfo()", True)
            End If

            If sdtm = False And cDash = False And Other = False Then
                Me.objCommon.ShowAlert("Data Not Found!", Me.Page)
            End If

            Return True
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UISDTMDATA", "UISDTMDATA(); ", True)
        Catch ex As Exception
            Me.ShowErrorMessage("Error Wihle Fill Data", ex.Message)
            Return False
        End Try
    End Function

#End Region

#Region "Dropdown Event"

    'Protected Sub ddlExportType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlExportType.SelectedIndexChanged
    '    Dim ds_Export As DataSet
    '    Dim wStr As String = ""

    '    Try
    '        wStr = HProjectId.Value + "##" + ddlDomain.SelectedValue + "##" + ddlExportType.SelectedValue
    '        ds_Export = Me.objhelpDb.ProcedureExecute("dbo.Proc_AttributeList", wStr)

    '        If Not ds_Export Is Nothing AndAlso ds_Export.Tables(0).Rows.Count > 0 Then
    '        End If

    '        MPExport.Show()
    '    Catch ex As Exception

    '    End Try
    'End Sub

#End Region

#Region "WEB METHOD"
    <Web.Services.WebMethod()> _
    Public Shared Function GETAttributeData(ByVal vWorkSpaceId As String, ByVal vMedExGroupCode As String, ByVal cAttributeType As String) As String
        Dim objCommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim ds_Attribute As DataSet
        Dim wstr = Convert.ToString(vWorkSpaceId) + "##" + vMedExGroupCode + "##" + cAttributeType

        ds_Attribute = objhelpDb.ProcedureExecute("dbo.Proc_AttributeList", wstr)
        If Not ds_Attribute Is Nothing AndAlso ds_Attribute.Tables(0).Rows.Count > 0 Then

            Return JsonConvert.SerializeObject(ds_Attribute.Tables(0))

        End If
        Return JsonConvert.SerializeObject("")
    End Function
#End Region


#Region "Miscellaneous"
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    End Sub
#End Region


End Class
