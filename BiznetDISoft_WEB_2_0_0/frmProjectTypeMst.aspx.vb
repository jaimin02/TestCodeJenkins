Imports Newtonsoft.Json
Imports Microsoft
Imports Microsoft.Office.Interop
Imports System.Drawing


Partial Class frmProjectTypeMst
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATIONS"

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private Const VS_Choice As String = "Choice"
    Private Const VS_SAVE As String = "Save"
    Private Const VS_DtProjectTypeMst As String = "DtProjectTypeMst"
    Private Const VS_ProjectTypeCode As String = "ProjectTypeCode"
    Private Const VS_ProjectSubTypeCode As String = "ProjectSubTypeCode"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_ProjectTypeCode As Integer = 1
    Private Const GVC_ProjectTypeName As Integer = 2
    Private Const GVC_Edit As Integer = 3
    Private Const GVC_ProjectSubTypeCode As Integer = 5


#End Region

#Region "Page Load "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            GenCall()
        End If

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
                Me.ViewState(VS_ProjectTypeCode) = Me.Request.QueryString("Value").ToString
                Me.ViewState(VS_ProjectSubTypeCode) = Me.Request.QueryString("subcode").ToString
            End If

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtProjectTypeMst) = ds.Tables("view_ProjectSubTypeMst")   ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If
            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideSponsorDetails", "HideSponsorDetails(); ", True)
            End If
            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...Gencall")

        Finally

        End Try

    End Function

#End Region

#Region " GenCall_Data "

    Private Function GenCall_Data(ByRef ds_Retu As DataSet) As Boolean

        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Val As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing
        'Dim objFFR As New FFReporting.FFReporting
        Dim SubCode As String = String.Empty
        Try

            Val = Me.ViewState(VS_ProjectTypeCode) 'Value of where condition
            SubCode = IIf(Me.ViewState(VS_ProjectSubTypeCode) Is "", "0000", Me.ViewState(VS_ProjectSubTypeCode))
            Choice = Me.ViewState(VS_Choice)
            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vProjectTypeCode=" + Val.ToString
                If SubCode <> "0000" Then
                    wStr += " and vProjectSubTypeCode =" + SubCode
                End If
            End If


            If Not objHelp.GetviewProjectSubTypeMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
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

            ds_Retu = ds
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_Data")

        Finally

        End Try
    End Function

#End Region

#Region " GenCall_showUI() "

    Private Function GenCall_ShowUI() As Boolean
        Dim dt_ProTypeMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try
            Page.Title = ":: Project Type Master  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            Me.trRemarks.Attributes.Add("display", "none")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Project Type Master"

            dt_ProTypeMst = Me.ViewState(VS_DtProjectTypeMst)

            Choice = Me.ViewState("Choice")

            If Not FillDropDown() Then
                Exit Function
            End If

            If Not FillGrid() Then
                Exit Function
            End If



            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                Me.hdnProjectSubTypeCode.Value = Convert.ToString(dt_ProTypeMst.Rows(0).Item("vProjectSubTypeCode"))
                Try
                    Me.ddlProjectType.SelectedValue = Convert.ToString(dt_ProTypeMst.Rows(0).Item("vProjectTypeCode"))
                Catch ex As Exception
                End Try
                Me.txtProjectSubType.Text = Convert.ToString(dt_ProTypeMst.Rows(0).Item("vProjectSubTypeName"))
                Me.txtPSuffix.Text = Convert.ToString(dt_ProTypeMst.Rows(0).Item("vProjectTypeSuffix"))
                Me.BtnSave.Text = "Update"
                Me.BtnSave.ToolTip = "Update"
                ddlProjectType.Enabled = False
                Me.trRemarks.Visible = True
            End If

            'Me.BtnSave.Attributes.Add("OnClick", "return Validation();")

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_showUI")
        Finally
        End Try
    End Function

#End Region

#Region "FillGrid"

    Private Function FillGrid() As Boolean
        Dim ds_View As New Data.DataSet
        Dim estr As String = String.Empty
        Dim Wstr_Scope As String = String.Empty
        Try


            'To Get Where condition of ScopeVales( Project Type )
            If Not ObjCommon.GetScopeValueWithCondition(Wstr_Scope) Then
                Return False
            End If

            If Not objHelp.GetviewProjectSubTypeMst(Wstr_Scope, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_View, estr) Then
                Return False
            End If
            ds_View.Tables(0).DefaultView.Sort = "vProjectTypeName"
            Me.GV_ProjectType.DataSource = ds_View.Tables(0).DefaultView.ToTable()
            Me.GV_ProjectType.DataBind()
            If GV_ProjectType.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "FillProjectType", "FillProjectType(); ", True)   ''Added by ketan
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..FillGrid")
            Return False
        End Try
    End Function

#End Region


#Region "Fill Dropown"
    Private Function FillDropDown() As Boolean
        Dim ds_ProjectrTypeMst As DataSet
        Try
            ds_ProjectrTypeMst = objHelp.ProcedureExecute("dbo.Proc_getProjectType", "")

            If Not ds_ProjectrTypeMst Is Nothing AndAlso ds_ProjectrTypeMst.Tables(0).Rows.Count > 0 Then
                ddlProjectType.DataSource = ds_ProjectrTypeMst
                Me.ddlProjectType.DataValueField = "vProjectTypeCode"
                Me.ddlProjectType.DataTextField = "vProjectTypeName"
                ddlProjectType.DataBind()
                ddlProjectType.Items.Insert(0, New ListItem("Select ProjectType", ""))
            End If

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
#End Region


#Region "Button Click"

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim ds_Save As New DataSet
        Dim dt_Save As New DataTable
        Dim estr As String = String.Empty
        Dim ProjectTypeCode As String = String.Empty
        Dim wStr As String = String.Empty
        Dim dr As DataRow
        Dim message As String = String.Empty
        Try


            If Not AssignValues() Then
                Exit Sub
            End If

            ds_Save = New DataSet
            dt_Save = CType(Me.ViewState(VS_DtProjectTypeMst), DataTable)
            dt_Save.TableName = "ProjectTypeMst"
            ds_Save.Tables.Add(dt_Save)

            If Not objLambda.Save_InsertProjectTypeMst(Me.ViewState(VS_Choice), ds_Save, Me.Session(S_UserID), ProjectTypeCode, estr) Then
                ObjCommon.ShowAlert("Error While Saving ProjectTypeMst", Me.Page)
                Exit Sub

            End If

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                wStr = "nScopeNo in(" + Me.Session(S_ScopeNo).ToString() + "," + Scope_SAdmin.ToString() + ")"
                If Not objHelp.GetScopeMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Save, estr) Then
                    ObjCommon.ShowAlert("Error While Getting Data From ScopeMst", Me.Page)
                    Exit Sub
                End If

                'ProjectTypeCode += "000"
                'ProjectTypeCode = Right(ProjectTypeCode, 4)
                'ProjectTypeCode = StrReverse(ProjectTypeCode)
                For Each dr In ds_Save.Tables(0).Rows

                    dr("vTableValues") = dr("vTableValues") + "," + ProjectTypeCode

                Next
                ds_Save.Tables(0).AcceptChanges()

                If Not objLambda.Save_ScopeMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                            WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_ScopeMst, ds_Save, _
                            Me.Session(S_UserID), estr) Then
                    ObjCommon.ShowAlert("Error While Updating ScopeMst", Me.Page)
                    Exit Sub
                End If

            End If

            Me.Session(S_ScopeValue) = Me.Session(S_ScopeValue).ToString.Trim() + ",'" + ProjectTypeCode + "'"

            ProjectTypeCode = ""
            message = IIf(Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "Project Type Saved Successfully !", "Project Type Updated Successfully !")
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Alert", "ShowAlert('" + message + "')", True)

            ResetPage()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...BtnSave_Click")
        End Try
    End Sub

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click

        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.ResetPage()
        Me.Response.Redirect("frmProjectTypeMst.aspx?mode=1")
    End Sub

    Protected Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        Dim wStr As String = String.Empty
        Dim ds As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim isReportComplete As Boolean = False

        Dim dtClientMstHistory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable
        Dim filename As String
        Dim strMessage As New StringBuilder

        Try
            Dim array() As String = hdnClientCode.Value.ToString().Split("+")

            wStr = Convert.ToString(array(0)) + "##" + Convert.ToString(array(1)) + "##" + Convert.ToString(array(2)) + "##"

            ds = objHelp.ProcedureExecute("dbo.Proc_GetProjectSubTypeMstAuditTrail", wStr)

            If Not dtClientMstHistory Is Nothing Then
                dtClientMstHistory.Columns.Add("SrNo")
                dtClientMstHistory.Columns.Add("ProjectTypeName")
                dtClientMstHistory.Columns.Add("ProjectSubTypeName")
                dtClientMstHistory.Columns.Add("ProjectTypeSuffix")
                dtClientMstHistory.Columns.Add("Remark")
                dtClientMstHistory.Columns.Add("ModifyBy")
                dtClientMstHistory.Columns.Add("ModifyOn")
            End If
            dt = ds.Tables(0)
            Dim dv As New DataView(dt)

            For Each dr As DataRow In dv.ToTable.Rows

                drAuditTrail = dtClientMstHistory.NewRow()

                drAuditTrail("SrNo") = i
                drAuditTrail("ProjectTypeName") = dr("vProjectTypeName").ToString()
                drAuditTrail("ProjectSubTypeName") = dr("vProjectSubTypeName").ToString()
                drAuditTrail("ProjectTypeSuffix") = dr("vProjectTypeSuffix").ToString()
                drAuditTrail("Remark") = dr("vRemark").ToString()
                drAuditTrail("ModifyBy") = dr("ModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("ModifyOn"))
                dtClientMstHistory.Rows.Add(drAuditTrail)
                dtClientMstHistory.AcceptChanges()
                i += 1
            Next

            gvExport.DataSource = dtClientMstHistory
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

                filename = "Audit Trail_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

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
                strMessage.Append("Project Group Master-AuditTrail")
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

                filename = "Audit"
                filename = filename & ".xls"

                drAuditTrail = dtClientMstHistory.NewRow()

                drAuditTrail("SrNo") = ""
                drAuditTrail("ProjectTypeName") = ""
                drAuditTrail("ProjectSubTypeName") = ""
                drAuditTrail("ProjectTypeSuffix") = ""
                drAuditTrail("Remark") = ""
                drAuditTrail("ModifyBy") = ""
                drAuditTrail("ModifyOn") = ""
                dtClientMstHistory.Rows.Add(drAuditTrail)
                dtClientMstHistory.TableName = "AuditTrail"
                dtClientMstHistory.AcceptChanges()

                gvExport.DataSource = dtClientMstHistory
                gvExport.DataBind()

                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(StringWriter)
                gvExport.RenderControl(writer)
                gridviewHtml = StringWriter.ToString()


                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Project Group Master-AuditTrail")
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
            End If


        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        'Tell the compiler that the control is rendered
        'explicitly by overriding the VerifyRenderingInServerForm event.
    End Sub

#End Region

#Region "Assign values"
    Private Function AssignValues() As Boolean
        Dim dr As DataRow
        Dim dt_Project As New DataTable
        Dim ds_Check As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim Wstr_Scope As String = String.Empty
        Try

            dt_Project = CType(Me.ViewState(VS_DtProjectTypeMst), DataTable)

            'To Get Where condition of ScopeVales( Project Type )
            If Not ObjCommon.GetScopeValueWithCondition(Wstr_Scope) Then
                Exit Function
            End If

            wStr = Wstr_Scope & " And vProjectTypeCode='" + _
                   Convert.ToString(Me.ddlProjectType.SelectedValue) + "'  And view_ProjectSubTypeMst.vProjectSubTypeName='" + Me.txtProjectSubType.Text.Trim() + "'  "

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                wStr += " And vProjectTypeCode <> '" + dt_Project.Rows(0).Item("vProjectTypeCode").ToString().Trim() + "'"
            End If

            If Not objHelp.GetviewProjectSubTypeMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Check, eStr) Then
                Me.ShowErrorMessage("Error While Getting Data From ViewProjectTypeMst", eStr)
                Return False
            End If

            If ds_Check.Tables(0).Rows.Count > 0 Then
                ObjCommon.ShowAlert("Project Type Name Already Exists !", Me.Page)
                If GV_ProjectType.Rows.Count > 0 Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "FillProjectType", "FillProjectType(); ", True)   ''Added by ketan
                End If
                Return False
            End If


            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                dt_Project.Clear()
                ' dt_Project.Columns.Add("vProjectSubTypeName")
                ' dt_Project.Columns.Add("vProjectSubTypeCode")
                dr = dt_Project.NewRow()
                dr("vProjectTypeCode") = ddlProjectType.SelectedValue
                dr("vProjectTypeName") = Convert.ToString(Me.ddlProjectType.SelectedItem)
                dr("vProjectSubTypeCode") = Me.hdnProjectSubTypeCode.Value()
                dr("vProjectSubTypeName") = Me.txtProjectSubType.Text.Trim()
                dr("vProjectTypeSuffix") = Me.txtPSuffix.Text.Trim()
                dr("vRemark") = ""
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dt_Project.Rows.Add(dr)
            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                For Each dr In dt_Project.Rows
                    dr("vProjectTypeName") = Convert.ToString(Me.ddlProjectType.SelectedItem)
                    dr("vProjectSubTypeCode") = IIf(Me.ViewState(VS_ProjectSubTypeCode) Is "", "0000", Me.ViewState(VS_ProjectSubTypeCode))
                    dr("vProjectSubTypeName") = Me.txtProjectSubType.Text.Trim()
                    dr("vProjectTypeSuffix") = Me.txtPSuffix.Text.Trim()
                    dr("vRemark") = Me.txtRemark.Text.Trim()
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "E"
                    ddlProjectType.Enabled = True
                Next
            End If

            Me.ViewState(VS_DtProjectTypeMst) = dt_Project
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...AssignValues")
            Return False
        End Try
    End Function
#End Region

#Region "Reset Page"
    Private Sub ResetPage()
        Me.txtProjectSubType.Text = ""
        Me.ViewState(VS_DtProjectTypeMst) = Nothing
        Me.hdnProjectSubTypeCode.Value = ""
        'Me.Response.Redirect("frmProjectTypeMst.aspx?mode=1")

    End Sub
#End Region

#Region "Grid Events"

    Protected Sub GV_ProjectType_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim Index As Integer = e.CommandArgument
        If e.CommandName.ToUpper = "EDIT" Then
            Me.hdnProjectSubTypeCode.Value = Me.GV_ProjectType.Rows(Index).Cells(GVC_ProjectSubTypeCode).Text.Trim()
            Me.Response.Redirect("frmProjectTypeMst.aspx?mode=2&value=" & Me.GV_ProjectType.Rows(Index).Cells(GVC_ProjectTypeCode).Text.Trim() & "&subcode=" & Me.GV_ProjectType.Rows(Index).Cells(GVC_ProjectSubTypeCode).Text.Trim())
        End If
        If e.CommandName.Contains("ExportToExcel") Then
            hdnClientCode.Value = e.CommandName.Replace("ExportToExcel", "")
            btnExportToExcel_Click(Nothing, Nothing)
        End If
    End Sub

    Protected Sub GV_ProjectType_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If Not e.Row.RowType = DataControlRowType.Pager Then
            e.Row.Cells(GVC_ProjectTypeCode).Visible = False
            e.Row.Cells(GVC_ProjectSubTypeCode).Visible = False

            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (Me.GV_ProjectType.PageIndex * Me.GV_ProjectType.PageSize) + 1
                CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandName = "Edit"
                'CType(e.Row.FindControl("imgExport"), ImageButton).CommandArgument = e.Row.RowIndex
                'CType(e.Row.FindControl("imgExport"), ImageButton).CommandName = "ExportToExcel"
                'CType(e.Row.FindControl("lnkAudit"), ImageButton).CommandArgument = e.Row.RowIndex
                'CType(e.Row.FindControl("lnkAudit"), ImageButton).CommandName = "Audit"
            End If
        End If
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

#Region "Web Methods"
    <Web.Services.WebMethod()> _
    Public Shared Function AuditTrailForProjectSubType(ByVal ProjectTypeCode As String, ByVal ProjectSubTypeCode As String, ByVal iUserId As Integer) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim ds As DataSet
        Dim eStr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim wStr As String
        Try
            wStr = iUserId.ToString() + "##" + ProjectTypeCode + "##" + ProjectSubTypeCode + "##"

            ds = objHelp.ProcedureExecute("dbo.Proc_GetProjectSubTypeMstAuditTrail", wStr)

            Dim column As DataColumn
            column = New DataColumn()
            Dim drAuditTrail As DataRow
            column.DataType = System.Type.GetType("System.Int32")
            column.ColumnName = "SrNo"
            ds.Tables(0).Columns.Add(column)
            ds.AcceptChanges()

            Dim dtTempAuditTrail As New DataTable

            If Not dtTempAuditTrail Is Nothing Then
                dtTempAuditTrail.Columns.Add("SrNo", GetType(String))
                dtTempAuditTrail.Columns.Add("vProjectTypeName", GetType(String))
                dtTempAuditTrail.Columns.Add("vProjectSubTypeName", GetType(String))
                dtTempAuditTrail.Columns.Add("vProjectTypeSuffix", GetType(String))
                dtTempAuditTrail.Columns.Add("vRemark", GetType(String))
                dtTempAuditTrail.Columns.Add("ModifyBy", GetType(String))
                dtTempAuditTrail.Columns.Add("Modifyon", GetType(String))
            End If

            dtTempAuditTrail.AcceptChanges()

            Dim i1 As Integer
            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                i1 = i + 1
                drAuditTrail = dtTempAuditTrail.NewRow()
                drAuditTrail("SrNo") = i1
                drAuditTrail("vProjectTypeName") = Convert.ToString(ds.Tables(0).Rows(i)("vProjectTypeName"))
                drAuditTrail("vProjectSubTypeName") = Convert.ToString(ds.Tables(0).Rows(i)("vProjectSubTypeName"))
                drAuditTrail("vProjectTypeSuffix") = Convert.ToString(ds.Tables(0).Rows(i)("vProjectTypeSuffix"))
                drAuditTrail("vRemark") = Convert.ToString(ds.Tables(0).Rows(i)("vRemark").ToString())
                drAuditTrail("ModifyBy") = Convert.ToString(ds.Tables(0).Rows(i)("ModifyBy").ToString())
                drAuditTrail("Modifyon") = Convert.ToString(ds.Tables(0).Rows(i)("Modifyon"))
                dtTempAuditTrail.Rows.Add(drAuditTrail)
                dtTempAuditTrail.AcceptChanges()
            Next i


            Return JsonConvert.SerializeObject(dtTempAuditTrail)

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

    End Function

#End Region


End Class
