Imports Microsoft.Win32
Imports System.Web.Services
Imports Newtonsoft.Json
Imports Microsoft
Imports Microsoft.Office.Interop
Imports System.IO
Imports System.Drawing

Partial Class frmDeptStageMAtrix
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION "

    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtDeptStageMatrix As String = "DtDeptStageMatrix"
    Private Const VS_DeptStageCode As String = "vDeptStageCode"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_DeptName As Integer = 1
    Private Const GVC_StageDesc As Integer = 2
    Private Const GVC_Modifyon As Integer = 3
    Private Const GVC_Edit As Integer = 4
    Private Const GVC_Code As Integer = 5

#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            GenCall()
            Exit Sub
        End If
    End Sub
#End Region

#Region "GenCall"
    Private Function GenCall() As Boolean
        Dim dt_deptstageMatrix As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try


            Choice = Me.Request.QueryString("mode").ToString
            Me.ViewState(VS_Choice) = Choice   'To use it while saving

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_DeptStageCode) = Me.Request.QueryString("Value").ToString
            End If

            ''''Check for Valid User''''''''''''''
            If Not GenCall_Data(Choice, dt_deptstageMatrix) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtDeptStageMatrix) = dt_deptstageMatrix ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI(Choice, Me.ViewState(VS_DtDeptStageMatrix)) Then 'For Displaying Data 
                Exit Function
            End If
            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideDeptStageDetails", "HideDeptStageDetails(); ", True)
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
                            ByRef dt_Dist_Retu As DataTable) As Boolean

        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_stagemat As DataSet = Nothing
        'Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable
        Try

            If Choice_1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vDeptStageCode=" + Me.ViewState(VS_DeptStageCode).ToString.Trim() 'Value of where condition
            End If

            If Not objHelp.GetDeptStageMatrix(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_stagemat, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_stagemat Is Nothing Then
                Throw New Exception(eStr)
            End If

            If ds_stagemat.Tables(0).Rows.Count <= 0 And _
               Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                Throw New Exception("No Records Found for Selected role")

            End If

            dt_Dist_Retu = ds_stagemat.Tables(0)
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCallData")
        Finally
        End Try
    End Function
#End Region

#Region "GenCall_ShowUI"
    Private Function GenCall_ShowUI(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                                      ByVal dt_deptstageMat As DataTable) As Boolean
        Try
            Page.Title = " :: Department Stage Matrix ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Master.FindControl("lblHeading"), Label).Text = "Department Stage Matrix"

            If Not FillDropdown() Then
                Return False
            End If

            BindGrid()

            If Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                Me.ddlStages.SelectedValue = ConvertDbNullToDbTypeDefaultValue(dt_deptstageMat.Rows(0)("iStageId"), dt_deptstageMat.Rows(0)("iStageId").GetType)
                Me.ddldept.SelectedValue = ConvertDbNullToDbTypeDefaultValue(dt_deptstageMat.Rows(0)("vDeptCode"), dt_deptstageMat.Rows(0)("vDeptCode").GetType)
                Me.txtRemark.Text = ""
                Me.btnSave.Text = "Update"
                Me.btnSave.ToolTip = "Update"

            End If

            GenCall_ShowUI = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCallShowUI")
        Finally
        End Try

    End Function
#End Region

#Region "Fill Dropdown"
    Private Function FillDropdown() As Boolean
        Dim ds_dept As New DataSet
        Dim ds_stage As New DataSet
        Dim dv_dept As New DataView
        Dim dv_stage As New DataView
        Dim estr As String = String.Empty
        Dim wStr As String = String.Empty
        Try



            wStr = "cStatusIndi <> 'D'"
            If Not Me.objHelp.GetDeptMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_dept, estr) Then
                Me.objcommon.ShowAlert("Error While Getting Data From DeptMst : " + estr, Me.Page)
                Exit Function
            End If

            dv_dept = ds_dept.Tables(0).DefaultView.ToTable(True, "vDeptCode,vDeptName".Split(",")).DefaultView
            dv_dept.Sort = "vDeptName"
            Me.ddldept.DataSource = dv_dept.ToTable()
            Me.ddldept.DataValueField = "vDeptCode"
            Me.ddldept.DataTextField = "vDeptName"
            Me.ddldept.DataBind()
            Me.ddldept.Items.Insert(0, New ListItem("Select Department", ""))

            If Not Me.objHelp.GetStageMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_stage, estr) Then
                Me.objcommon.ShowAlert("Error While Getting Data From StageMst : " + estr, Me.Page)
                Return False
            End If

            dv_stage = ds_stage.Tables(0).DefaultView.ToTable(True, "iStageId,vStageDesc".Split(",")).DefaultView
            dv_stage.Sort = "vStageDesc"

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                Me.Cblstages.Visible = True
                Me.Cblstages.DataSource = dv_stage.ToTable()
                Me.Cblstages.DataValueField = "iStageId"
                Me.Cblstages.DataTextField = "vStageDesc"
                Me.Cblstages.DataBind()
                Me.ddlStages.Visible = False
                Me.btnSave.OnClientClick = "return Validation();"
            Else

                Me.ddlStages.Visible = True
                Me.ddlStages.DataSource = dv_stage.ToTable()
                Me.ddlStages.DataValueField = "iStageId"
                Me.ddlStages.DataTextField = "vStageDesc"
                Me.ddlStages.DataBind()
                Me.Cblstages.Visible = False
                Me.dvChkListStageMatrix.Visible = False
                Me.btnSave.OnClientClick = "return ValidationForEdit();"
            End If

            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....FillDropdown")
            Return False
        End Try
    End Function

#End Region

#Region "BindGrid"

    Private Sub BindGrid()
        Dim dsdeptstage As New DataSet
        Dim eStr As String = String.Empty

        If objHelp.GetViewdeptstagematrix("cStatusIndi <> 'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsdeptstage, eStr) Then
            dsdeptstage.Tables(0).DefaultView.Sort = "vDeptName"
            gvdeptstage.ShowFooter = False
            gvdeptstage.DataSource = dsdeptstage.Tables(0).DefaultView.ToTable()
            gvdeptstage.DataBind()
            dsdeptstage.Dispose()
        End If
        If gvdeptstage.Rows.Count > 0 Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvdeptstage", "UIgvdeptstage(); ", True)
        End If
    End Sub

#End Region

#Region "Reset Page"
    Private Sub ResetPage()
        Me.ddldept.SelectedIndex = 0
        Me.txtRemark.Text = ""
        'Response.Redirect("frmDeptStageMatrix.aspx?mode=1&choice=1")
    End Sub
#End Region

#Region "Save"

    Private Function AssignUpdatedValues() As Boolean
        Dim dtOld As New DataTable
        Dim dr As DataRow
        Dim index As Integer = 0
        Dim ds_Check As New DataSet
        Dim eStr As String = String.Empty
        Dim flag As Boolean = False
        Dim DeptStageCode As String = String.Empty

        Try

            dtOld = Me.ViewState(VS_DtDeptStageMatrix)

            If Not objHelp.GetViewdeptstagematrix("cStatusIndi <> 'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Check, eStr) Then
                Me.ShowErrorMessage("Error While Getting Data From ViewDeptStageMatrix", eStr)
                Return False
            End If

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                If Cblstages.SelectedItem.Text.Length = 0 Then
                    Me.objcommon.ShowAlert("Please Check atleast one stage", Me)
                    Exit Function
                End If

                dtOld.Clear()
                For index = 0 To Cblstages.Items.Count - 1
                    If Cblstages.Items(index).Selected Then
                        flag = False
                        For Each dr In ds_Check.Tables(0).Rows

                            If dr("vDeptCode").ToString().Trim() = Me.ddldept.SelectedItem.Value.Trim() And _
                                dr("vStageDesc").ToString().Trim() = Me.Cblstages.Items(index).Text.Trim() Then
                                flag = True
                                Exit For

                            End If
                        Next dr

                        If flag = True Then
                            objcommon.ShowAlert("Stage Name Already Exists With Selected Dept !", Me.Page)
                            Return False
                        ElseIf flag Then
                            Continue For
                        End If

                        dr = dtOld.NewRow
                        dr("iStageId") = Cblstages.Items(index).Value.Trim()
                        dr("vDeptCode") = Me.ddldept.SelectedValue.Trim()
                        dr("vRemark") = Me.txtRemark.Text.Trim()
                        For ind As Integer = 0 To ((4 - index.ToString.Length) - 1)
                            DeptStageCode += "0"
                        Next ind

                        dr("vDeptStageCode") = DeptStageCode & index.ToString
                        dr("iModifyBy") = Session(S_UserID)
                        dtOld.Rows.Add(dr)

                    End If

                Next index
            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                For Each dr In ds_Check.Tables(0).Rows

                    If dr("vDeptCode").ToString().Trim() = Me.ddldept.SelectedItem.Value.Trim() And _
                        dr("iStageId").ToString().Trim() = Me.ddlStages.SelectedItem.Value.Trim() And _
                        dr("vDeptStageCode").ToString().Trim() <> dtOld.Rows(0).Item("vDeptStageCode").ToString().Trim() Then

                        objcommon.ShowAlert("Stage Name Already Exists With Selected Dept !", Me.Page)
                        Return False

                    End If

                Next dr

                For Each dr In dtOld.Rows

                    dr("iStageId") = Me.ddlStages.SelectedValue.Trim()
                    dr("vDeptCode") = Me.ddldept.SelectedValue.Trim()
                    dr("vRemark") = Me.txtRemark.Text.Trim()
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "E"
                    dr.AcceptChanges()

                Next dr

                dtOld.AcceptChanges()

            End If

            Me.ViewState(VS_DtDeptStageMatrix) = dtOld
            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...AssignUpdatedValues")
            Return False
        End Try
    End Function

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim Dt As New DataTable
        Dim Ds_stagemat As DataSet
        Dim Ds_deptstagematgrid As New DataSet
        Dim eStr As String = String.Empty
        Dim objOPws As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()
        Dim message As String = String.Empty
        Try


            If Not AssignUpdatedValues() Then
                Exit Sub
            End If

            Ds_stagemat = New DataSet
            Ds_stagemat.Tables.Add(CType(Me.ViewState(VS_DtDeptStageMatrix), Data.DataTable).Copy())
            Ds_stagemat.Tables(0).TableName = "deptstageMatrix"   ' New Values on the form to be updated

            If Not objOPws.Save_DeptStageMatrix(Me.ViewState(VS_Choice), Ds_stagemat, Me.Session(S_UserID), eStr) Then


                objcommon.ShowAlert("Error While Saving DepStagetMst", Me.Page)
                Exit Sub

            End If
            message = IIf(Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "Department Stages Added Successfully !", "Department Stage Updated Successfully !")
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Alert", "ShowAlert('" + message + "')", True)


            ResetPage()

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....btnSave_Click")
        End Try
    End Sub

#End Region

#Region "Cancel & Close"

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ResetPage()
        Response.Redirect("frmDeptStageMatrix.aspx?mode=1")
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Response.Redirect("frmMainPage.aspx")
    End Sub

#End Region

#Region "Grid Event "

    Protected Sub gvdeptstage_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)



    End Sub

    Protected Sub gvdeptstage_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)

        gvdeptstage.PageIndex = e.NewPageIndex
        BindGrid()

    End Sub

    Protected Sub gvdeptstage_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim index As Integer = e.CommandArgument

        If e.CommandName.ToUpper = "EDIT" Then

            Me.Response.Redirect("frmDeptStageMAtrix.aspx?mode=2&value=" & Me.gvdeptstage.Rows(index).Cells(GVC_Code).Text.Trim())

        End If

    End Sub

    Protected Sub gvdeptstage_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If Not e.Row.RowType = DataControlRowType.Pager Then
            e.Row.Cells(GVC_Code).Visible = False
        End If


        If e.Row.RowType = DataControlRowType.DataRow Then

            e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (Me.gvdeptstage.PageSize * Me.gvdeptstage.PageIndex) + 1
            CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandName = "Edit"
            CType(e.Row.FindControl("lnkAudit"), ImageButton).Attributes.Add("vDeptStageCode", e.Row.Cells(GVC_Code).Text)

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
    Public Shared Function AuditTrail(ByVal vDeptStageCode As String) As String
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim Str As String = String.Empty
        Dim ds_DeptStageMatrix As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable
        Dim dtDeptStageMatrixHistory As New DataTable

        Try

            Str = Convert.ToString(vDeptStageCode)
            ds_DeptStageMatrix = objHelp.ProcedureExecute("Proc_GetDeptStageAuditTrail", Str)

            If Not dtDeptStageMatrixHistory Is Nothing Then
                dtDeptStageMatrixHistory.Columns.Add("SrNo")
                dtDeptStageMatrixHistory.Columns.Add("DeptName")
                dtDeptStageMatrixHistory.Columns.Add("StageDesc")
                dtDeptStageMatrixHistory.Columns.Add("Remark")
                dtDeptStageMatrixHistory.Columns.Add("ModifyBy")
                dtDeptStageMatrixHistory.Columns.Add("ModifyOn")
            End If

            dt = ds_DeptStageMatrix.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtDeptStageMatrixHistory.NewRow()
                drAuditTrail("SrNo") = i
                drAuditTrail("DeptName") = dr("vDeptName").ToString()
                drAuditTrail("StageDesc") = dr("vStageDesc").ToString()
                drAuditTrail("Remark") = dr("vRemark").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("dModifyOn"))
                dtDeptStageMatrixHistory.Rows.Add(drAuditTrail)
                dtDeptStageMatrixHistory.AcceptChanges()
                i += 1
            Next
            strReturn = JsonConvert.SerializeObject(dtDeptStageMatrixHistory)
            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Protected Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_DeptStageMatrix As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty

        Dim dtDeptStageMatrixHistory As New DataTable
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

            If Not dtDeptStageMatrixHistory Is Nothing Then
                dtDeptStageMatrixHistory.Columns.Add("Sr. No")
                dtDeptStageMatrixHistory.Columns.Add("Department Name")
                dtDeptStageMatrixHistory.Columns.Add("Stage Desc")
                dtDeptStageMatrixHistory.Columns.Add("Remarks")
                dtDeptStageMatrixHistory.Columns.Add("Modify By")
                dtDeptStageMatrixHistory.Columns.Add("Modify On")
            End If

            dtDeptStageMatrixHistory.AcceptChanges()

            wStr = hdnDeptStageCode.Value + "##"
            ds_DeptStageMatrix = objHelp.ProcedureExecute("Proc_GetDeptStageAuditTrail", wStr)

            If Not ds_DeptStageMatrix.Tables(0) Is Nothing Then

                dt = ds_DeptStageMatrix.Tables(0)
                Dim dv As New DataView(dt)
                For Each dr As DataRow In dv.ToTable.Rows
                    drAuditTrail = dtDeptStageMatrixHistory.NewRow()
                    drAuditTrail("Sr. No") = i
                    drAuditTrail("Department Name") = dr("vDeptName").ToString()
                    drAuditTrail("Stage Desc") = dr("vStageDesc").ToString()
                    drAuditTrail("Remarks") = dr("vRemark").ToString()
                    drAuditTrail("Modify By") = dr("vModifyBy").ToString()
                    drAuditTrail("Modify On") = Convert.ToString(dr("dModifyOn"))
                    dtDeptStageMatrixHistory.Rows.Add(drAuditTrail)
                    dtDeptStageMatrixHistory.AcceptChanges()
                    i += 1
                Next

                gvExport.DataSource = dtDeptStageMatrixHistory
                gvExport.DataBind()
            End If


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

                filename = "Audit Trail_" + hdnDeptStageCode.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

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
                strMessage.Append("Department Stage Matrix Master-AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td Align=""Right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")


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

                filename = "Audit Trail_" + hdnDeptStageCode.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"


                drAuditTrail = dtDeptStageMatrixHistory.NewRow()

                drAuditTrail("Sr. No") = i
                drAuditTrail("Department Name") = ""
                drAuditTrail("Stage Desc") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("Modify By") = ""
                drAuditTrail("Modify On") = ""
                dtDeptStageMatrixHistory.Rows.Add(drAuditTrail)
                dtDeptStageMatrixHistory.AcceptChanges()
                gvExport.DataSource = dtDeptStageMatrixHistory
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
                strMessage.Append("Department Stage Matrix Master-AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td align=""right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")


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
        Dim ds_DeptStageMatrix As New DataSet
        Dim estr As String = String.Empty
        Dim dtDeptStageMatrixHistory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable
        Dim vDeptStageCode As String = String.Empty
        Dim filename As String = String.Empty
        Dim strMessage As New StringBuilder

        Try
            wStr = "" + "##"
            ds_DeptStageMatrix = objHelp.ProcedureExecute("Proc_DeptStageMatrix ", wStr)
            wStr = String.Empty
            If Not dtDeptStageMatrixHistory Is Nothing Then
                dtDeptStageMatrixHistory.Columns.Add("Sr. No")
                dtDeptStageMatrixHistory.Columns.Add("Department Name")
                dtDeptStageMatrixHistory.Columns.Add("Stage Desc")
                dtDeptStageMatrixHistory.Columns.Add("Remarks")
                dtDeptStageMatrixHistory.Columns.Add("Modify By")
                dtDeptStageMatrixHistory.Columns.Add("Modify On")
            End If

            dtDeptStageMatrixHistory.AcceptChanges()
            dt = ds_DeptStageMatrix.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtDeptStageMatrixHistory.NewRow()
                drAuditTrail("Sr. No") = i
                drAuditTrail("Department Name") = dr("vDeptName").ToString()
                drAuditTrail("Stage Desc") = dr("vStageDesc").ToString()
                drAuditTrail("Remarks") = dr("vRemark").ToString()
                drAuditTrail("Modify By") = dr("iModifyBy").ToString()
                drAuditTrail("Modify On") = Convert.ToString(dr("dModifyOn"))
                dtDeptStageMatrixHistory.Rows.Add(drAuditTrail)
                dtDeptStageMatrixHistory.AcceptChanges()
                i += 1
            Next

            gvExportToExcel.DataSource = dtDeptStageMatrixHistory
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
                        cell.CssClass = " "
                    Next
                Next

                'style to format numbers to string
                Dim style As String = "<style> .textmode { } </style>"
                Response.Write(style)

                Dim info As String = String.Empty
                Dim gridviewHtml As String = String.Empty

                filename = "DepartmentStage Matrix" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

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
                strMessage.Append("Department Stage Matrix")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td Align=""Right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")


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

                filename = "DepartmentStage Matrix" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                drAuditTrail = dtDeptStageMatrixHistory.NewRow()
                drAuditTrail("Sr. No") = i
                drAuditTrail("Department Name") = ""
                drAuditTrail("Stage Desc") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("Modify By") = ""
                drAuditTrail("Modify On") = ""
                dtDeptStageMatrixHistory.Rows.Add(drAuditTrail)
                dtDeptStageMatrixHistory.AcceptChanges()
                gvExportToExcel.DataSource = dtDeptStageMatrixHistory
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
                strMessage.Append("Department Stage Matrix")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td align=""right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")

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
