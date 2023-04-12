Imports Newtonsoft.Json

Partial Class frmSubjectRejectionDetail
    Inherits System.Web.UI.Page

#Region " VARIABLE DECLARATION "

    Dim ObjCommon As New clsCommon
    Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private objhelpDb As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtSubjectRejectionDetail As String = "DtSubjectRejectionDetail"
    Private Const VS_SubjectRejectionNo As String = "SubjectRejectionNo"

#End Region

#Region "Load Event"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                GenCall()
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

            Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add

            If Not IsNothing(Me.Request.QueryString("Mode")) AndAlso Me.Request.QueryString("Mode") <> "" Then
                Choice = Me.Request.QueryString("Mode")
            End If

            Me.ViewState(VS_Choice) = Choice   'To use it while saving

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_SubjectRejectionNo) = Me.Request.QueryString("SubjectId").ToString
            End If



            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtSubjectRejectionDetail) = ds.Tables("SubjectRejectionDetail")   ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If

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

            Val = Me.ViewState(VS_SubjectRejectionNo) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "nSubjectRejectionNo=" + Val.ToString
            End If
            wStr += " And cStatusIndi <> 'D'"

            If Not objHelp.GetSubjectRejectionDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
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

            ds_DWR_Retu = ds
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..........GenCall_Data")

        Finally

        End Try
    End Function
#End Region

#Region "GenCall_showUI "
    Private Function GenCall_ShowUI() As Boolean
        Dim dt_SubjectRejectionDetail As DataTable = Nothing
        Dim ds_SubjectRejectionDetail As New DataSet
        Dim ds_subjectRejectionShow As New DataSet
        Dim dt_SubjectRejectionShow As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Try
            If System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge").Contains(Session(S_LocationCode)) = True Then
                Me.AutoCompleteExtender1.ContextKey = Me.Session(S_LocationCode)
                Me.AutoCompleteExtender1.ServiceMethod = "GetSubjectCompletionList_NotRejected_BlockPeriod"
            Else
                Me.AutoCompleteExtender1.ContextKey = System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge")
                Me.AutoCompleteExtender1.ServiceMethod = "GetSubjectCompletionList_NotRejected_BlockPeriodDataMerg"
            End If

            If System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge").Contains(Session(S_LocationCode)) = True Then
                Me.AutoCompleteExtender2.ContextKey = Me.Session(S_LocationCode)
                Me.AutoCompleteExtender2.ServiceMethod = "GetSubjectCompletionList_NotRejected_BlockPeriod"
            Else
                Me.AutoCompleteExtender2.ContextKey = System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge")
                Me.AutoCompleteExtender2.ServiceMethod = "GetSubjectCompletionList_NotRejected_BlockPeriodDataMerg"
            End If




            Fillgrid(True)

            Page.Title = ":: Subject Rejection   :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Subject Rejection Detail"

            dt_SubjectRejectionDetail = Me.ViewState(VS_DtSubjectRejectionDetail)

            Me.lblRejectedBY.Visible = False
            Me.lblRejectedByUser.Text = ""

            If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                Wstr = "vSubjectId='" & Me.HSubjectId.Value.Trim() & "' And "
                Wstr += " cStatusIndi <> 'D'"

                If Not objHelp.View_SubjectRejectionDetail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                    ds_SubjectRejectionDetail, estr) Then
                    Return False
                End If

                dt_SubjectRejectionDetail = ds_SubjectRejectionDetail.Tables(0)

                Me.txtRemark.Text = dt_SubjectRejectionDetail.Rows(0).Item("vRemark")
                'Me.rblReject.SelectedValue = dt_SubjectRejectionDetail.Rows(0).Item("cRejectionFlag")
                Me.lblRejectedBY.Visible = True
                Me.lblRejectedByUser.Text = dt_SubjectRejectionDetail.Rows(0).Item("vModifyBy")
                Me.btnSave.Text = "Update"
                Me.btnSave.ToolTip = "Update"
            End If

            ' Me.btnSave.Attributes.Add("onclick", "return Validation();")

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_ShowUI")
            GenCall_ShowUI = False
        Finally
        End Try
    End Function
#End Region

#Region "FillData"

    Private Function FillData() As Boolean
        Dim ds_SubjectRejectionDetail As New Data.DataSet
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim cRejectionFlag As String = String.Empty
        Try


            Wstr = "vSubjectId='" & Me.HSubjectId.Value.Trim() & "' And "
            Wstr += " cStatusIndi <> 'D'"

            If Not objHelp.View_SubjectRejectionDetail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_SubjectRejectionDetail, estr) Then
                Return False
            End If

            Me.lblRejectedBY.Visible = False
            Me.lblRejectedByUser.Text = ""

            If ds_SubjectRejectionDetail.Tables(0).Rows.Count > 0 Then
                cRejectionFlag = ds_SubjectRejectionDetail.Tables(0).Rows(0).Item("cRejectionFlag")

                Me.txtRemark.Text = ds_SubjectRejectionDetail.Tables(0).Rows(0).Item("vRemark")
                'Me.rblReject.SelectedValue = ds_SubjectRejectionDetail.Tables(0).Rows(0).Item("cRejectionFlag")
                Me.lblRejectedBY.Visible = True
                Me.lblRejectedByUser.Text = ds_SubjectRejectionDetail.Tables(0).Rows(0).Item("vModifyBy")
                Me.btnSave.Text = "Update"
                Me.btnSave.ToolTip = "Update"
                Me.chkreject.Checked = IIf(cRejectionFlag = "Y", True, False)
                Me.txtfromDate.Text = Convert.ToString(ds_SubjectRejectionDetail.Tables(0).Rows(0).Item("dblockfrom"))
                Me.txttoDate.Text = Convert.ToString(ds_SubjectRejectionDetail.Tables(0).Rows(0).Item("dblockto"))

            Else
                Me.txtRemark.Text = ""
                Me.chkreject.Checked = True
                Me.txtfromDate.Text = ""
                Me.txttoDate.Text = ""
            End If




            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..FillData")
            Return False
        End Try
    End Function

    Private Function ConvertDsuserTO(ByVal ds As DataSet) As String
        Dim strMessage As New StringBuilder
        Dim i As Integer
        Dim iCol As Integer
        Dim j As Integer
        Dim dsConvert As New DataSet


        Try
            strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")
            strMessage.Append("<tr>")
            Select Case ViewState("IdOfBtn")
                Case "btnExport"
                    strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                    strMessage.Append("</font></strong><center></b></td>")
                    strMessage.Append("</tr>")
                    strMessage.Append("<tr>")
                    strMessage.Append("</tr>")
                    strMessage.Append("<tr>")
                    strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""2.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append("Subject Rejection Report")
                    strMessage.Append("</font></strong><center></b></td>")
                    strMessage.Append("</tr>")
                    strMessage.Append("<tr><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""3"" face=""Verdana""><b>" + DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt") + "</b></font></td><td></td><td></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")
                Case "btnexportAudit"
                    strMessage.Append("<td colspan=""9""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                    strMessage.Append("</font></strong><center></b></td>")
                    strMessage.Append("</tr>")
                    strMessage.Append("<tr>")
                    strMessage.Append("</tr>")
                    strMessage.Append("<tr>")
                    strMessage.Append("<td colspan=""9""><center><strong><b><font color=""#000099"" size=""2.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append("Subject Rejection Audit Trail")
                    strMessage.Append("</font></strong><center></b></td>")
                    strMessage.Append("</tr>")
                    strMessage.Append("<tr><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""3"" face=""Verdana""><b>" + DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt") + "</b></font></td><td></td><td></td><td></td>><td></td><td></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")
            End Select
            strMessage.Append("<tr>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")

            If ViewState("IdOfBtn") = "btnExport" Then

                dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vSubjectId,FullName,dBirthDate,dModifyOn,vModifyBy,vRemark".Split(",")).DefaultView.ToTable())
                dsConvert.AcceptChanges()
                dsConvert.Tables(0).Columns(0).ColumnName = "Subject ID"
                dsConvert.Tables(0).Columns(1).ColumnName = "Subject Name"
                dsConvert.Tables(0).Columns(2).ColumnName = "D.O.B"
                dsConvert.Tables(0).Columns(3).ColumnName = "Rejection On"
                dsConvert.Tables(0).Columns(4).ColumnName = "Rejection By"
                dsConvert.Tables(0).Columns(5).ColumnName = "Remarks"

                dsConvert.AcceptChanges()
            ElseIf ViewState("IdOfBtn") = "btnexportAudit" Then
                dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "SrNo,vSubjectId,FullName,cRejectionFlag,vRemark,dBlockfrom,dBlockto,ModifyBy,dModifyOn".Split(",")).DefaultView.ToTable())
                dsConvert.AcceptChanges()
                dsConvert.Tables(0).Columns(0).ColumnName = "SrNo"
                dsConvert.Tables(0).Columns(1).ColumnName = "SubjectId"
                dsConvert.Tables(0).Columns(2).ColumnName = "SubjectName"
                dsConvert.Tables(0).Columns(3).ColumnName = "Permanent Block"
                dsConvert.Tables(0).Columns(4).ColumnName = "Remarks"
                dsConvert.Tables(0).Columns(5).ColumnName = "FromDate"
                dsConvert.Tables(0).Columns(6).ColumnName = "ToDate"
                dsConvert.Tables(0).Columns(7).ColumnName = "Modify By"
                dsConvert.Tables(0).Columns(8).ColumnName = "Modify On"

                dsConvert.AcceptChanges()
            End If

            For iCol = 0 To dsConvert.Tables(0).Columns.Count - 1
                strMessage.Append("<td bgcolor=""#1560a1""><strong><font color=""#FFFFFF"" size=""2.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(Convert.ToString(dsConvert.Tables(0).Columns(iCol)).Trim())
                strMessage.Append("</font></strong></td>")

            Next

            strMessage.Append("</tr>")

            strMessage.Append("<tr>")
            strMessage.Append("</tr>")

            For j = 0 To dsConvert.Tables(0).Rows.Count - 1
                strMessage.Append("<tr>")
                For i = 0 To dsConvert.Tables(0).Columns.Count - 1
                    ''
                    If j Mod 2 = 0 Then
                        strMessage.Append("<td align=""left"" bgcolor=""84c8e6""><font color=""#191970"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")

                        strMessage.Append(Convert.ToString(dsConvert.Tables(0).Rows(j).Item(i)).Trim())
                        strMessage.Append("</font></td>")
                    Else
                        strMessage.Append("<td align=""left"" bgcolor=""#""><font color=""#191970"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")

                        strMessage.Append(Convert.ToString(dsConvert.Tables(0).Rows(j).Item(i)).Trim())
                        strMessage.Append("</font></td>")
                    End If
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

#End Region

#Region "Button Click"

    Protected Sub btnSubject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubject.Click
        If Not Me.FillData() Then
            ObjCommon.ShowAlert("Error While Filling SubjectRejectionDetail", Me.Page)
            Exit Sub
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim ds_SubjectRejectionDetail As New DataSet
        Dim dt_SubjectRejectionDetail As New DataTable
        Dim dt_SubjectMaster As New DataTable
        Dim estr As String = String.Empty
        Try


            If Not AssignValues(dt_SubjectMaster) Then
                Exit Sub
            End If

            ds_SubjectRejectionDetail = New DataSet
            dt_SubjectRejectionDetail = CType(Me.ViewState(VS_DtSubjectRejectionDetail), DataTable)
            dt_SubjectRejectionDetail.TableName = "SubjectRejectionDetail"
            ds_SubjectRejectionDetail.Tables.Add(dt_SubjectRejectionDetail.Copy())

            ds_SubjectRejectionDetail.Tables.Add(dt_SubjectMaster.Copy())


            If Not objLambda.Save_SubjectRejectionDetail(Me.ViewState(VS_Choice), ds_SubjectRejectionDetail, Me.Session(S_UserID), estr) Then

                ObjCommon.ShowAlert("Error While Saving SubjectRejectionDetail", Me.Page)
                Exit Sub

            End If

            ObjCommon.ShowAlert("Record Saved SuccessFully", Me.Page)
            ResetPage()
            Me.btnSave.Text = "Save"
            Me.btnSave.ToolTip = "Save"

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnSave_Click")
        End Try

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        ResetPage()
    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click

        Dim ds_Field As New DataSet
        Dim fileName As String = String.Empty
        Dim dt_Final As New DataTable

        Dim Wstr As String = ""
        Dim estr As String = ""
        Try

            Wstr = "(cRejectionFlag = 'Y' Or CAST(GETDATE() As DATE)  Between Cast(isnull(dBlockfrom,'') As Date) And Cast(isnull(dBlockto,'') As Date))" '' or added by prayag
            objHelp.View_SubjectRejectionDetail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Field, estr)


            fileName = "Subject Rejection Report" + " " + Date.Today.ToString("dd-MMM-yyyy") + ".xls"

            ViewState("IdOfBtn") = "btnExport"

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

        End Try

    End Sub

    Protected Sub btnexportAudit_Click(sender As Object, e As EventArgs) Handles btnexportAudit.Click

        Dim ds_Field As New DataSet
        Dim fileName As String = String.Empty
        Dim dt_Final As New DataTable
        Try
            dt_Final = (JsonConvert.DeserializeObject(hdnExportAuditdata.Value, GetType(DataTable)))

            ds_Field.Tables.Add(dt_Final)

            fileName = "Subject Rejection Audit Trail" + " " + Date.Today.ToString("dd-MMM-yyyy") + ".xls"

            ViewState("IdOfBtn") = "btnexportAudit"


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

        End Try

    End Sub
#End Region

#Region "Assign Values "
    Private Function AssignValues(ByRef dt_SubjectMaster As DataTable) As Boolean
        Dim dr As DataRow
        Dim dt_SubjectRejectionDetail As New DataTable
        Dim Ds_SubjectMaster As New DataSet
        Dim eStr As String = String.Empty
        Dim cRejectionFlag As String = IIf(chkreject.Checked = True, "Y", "N")
        Try



            dt_SubjectRejectionDetail = CType(Me.ViewState(VS_DtSubjectRejectionDetail), DataTable)

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                dt_SubjectRejectionDetail.Clear()
                dr = dt_SubjectRejectionDetail.NewRow()
                dr("nSubjectRejectionNo") = 0
                dr("vSubjectId") = Me.HSubjectId.Value.Trim
                dr("itranNo") = 0
                ''dr("cRejectionFlag") = Me.rblReject.SelectedValue.Trim
                dr("cRejectionFlag") = cRejectionFlag.ToString()
                dr("vRemark") = Me.txtRemark.Text.Trim
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("dBlockfrom") = IIf(txtfromDate.Text = "", DBNull.Value, txtfromDate.Text)
                dr("dBlockto") = IIf(txttoDate.Text = "", DBNull.Value, txttoDate.Text)
                dt_SubjectRejectionDetail.Rows.Add(dr)

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                For Each dr In dt_SubjectRejectionDetail.Rows
                    dr("vSubjectId") = Me.HSubjectId.Value.Trim
                    ''dr("cRejectionFlag") = Me.rblReject.SelectedValue.Trim
                    dr("cRejectionFlag") = cRejectionFlag.ToString()
                    dr("vRemark") = Me.txtRemark.Text.Trim
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "E"
                    dr("dBlockfrom") = IIf(txtfromDate.Text = "", DBNull.Value, txtfromDate.Text)
                    dr("dBlockto") = IIf(txttoDate.Text = "", DBNull.Value, txttoDate.Text)
                    dr.AcceptChanges()
                Next
                dt_SubjectRejectionDetail.AcceptChanges()
            End If

            Me.ViewState(VS_DtSubjectRejectionDetail) = dt_SubjectRejectionDetail

            'Added on 29-Jul-2009
            If Not Me.objHelp.GetView_SubjectMaster("vSubjectId='" & Me.HSubjectId.Value.Trim & "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        Ds_SubjectMaster, eStr) Then

                Me.ObjCommon.ShowAlert("Error While Getting Data From SubjectMaster", Me.Page())
                Return False

            End If

            dt_SubjectMaster = Ds_SubjectMaster.Tables(0).Copy()
            For Each dr In dt_SubjectMaster.Rows
                '' dr("cRejectionFlag") = Me.rblReject.SelectedValue.Trim
                dr("cRejectionFlag") = cRejectionFlag.ToString()
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "E"
                dr("dBlockfrom") = IIf(txtfromDate.Text = "", DBNull.Value, txtfromDate.Text)
                dr("dBlockto") = IIf(txttoDate.Text = "", DBNull.Value, txttoDate.Text)
                dr.AcceptChanges()
            Next
            dt_SubjectMaster.AcceptChanges()
            '**********************************************************

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....AssignValues")
            Return False
        End Try

    End Function
#End Region

#Region "ResetPage"

    Private Sub ResetPage()
        Me.txtSubject.Text = ""
        Me.txtRemark.Text = ""
        Me.HSubjectId.Value = ""
        Me.txtfromDate.Text = ""
        Me.txttoDate.Text = ""
        'Me.rblReject.SelectedValue = ""
        Me.chkreject.Checked = False
        Me.lblRejectedBY.Visible = False
        Me.lblRejectedByUser.Text = ""
        Me.ViewState(VS_DtSubjectRejectionDetail) = Nothing
        Me.ViewState(VS_SubjectRejectionNo) = Nothing
        Me.ViewState(VS_Choice) = Nothing
        Me.GenCall()

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

    Protected Sub Gv_SubjectRejection_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles Gv_SubjectRejection.PageIndexChanging
        Gv_SubjectRejection.PageIndex = e.NewPageIndex
        If Not Fillgrid(False) Then
            Exit Sub
        End If
    End Sub

    Protected Sub Gv_SubjectRejection_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Gv_SubjectRejection.RowCommand
        If e.CommandName.ToUpper().Trim() = "LAB REPORT" Then 'added by vishal
            Dim newWin1 As String = String.Empty
            Try
                newWin1 = "window.open(""" + "frmReportReview.aspx?mode=4" + "&SubjectId=" + Gv_SubjectRejection.Rows(e.CommandArgument).Cells(0).Text + """)"
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", newWin1, True)

                'i7
            Catch
            Finally
            End Try
        End If



        If e.CommandName.ToUpper().Trim() = "DETAILS PIF" Then
            Dim newWin2 As String = String.Empty
            Try
                newWin2 = "window.open(""" + "frmSubjectPIFMst_New.aspx?mode=4" + "&SubjectId=" + Gv_SubjectRejection.Rows(e.CommandArgument).Cells(0).Text + """)"
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", newWin2, True)

            Catch
            Finally
            End Try

        End If
        If e.CommandName.ToUpper().Trim() = "DETAILS MSR" Then
            Dim newWin3 As String = String.Empty
            Dim WorkspaceId As String = String.Empty
            Dim Wstr As String = String.Empty
            Dim estr As String = String.Empty
            Dim latestscreeningdate As String = String.Empty
            Dim ds_ScreeningData As New DataSet
            Dim dv_ScreeningData As New DataView
            Try
                ''Commented and added by Aaditya link with new screening page
                WorkspaceId = "0000000000"
                Wstr = "vWorkspaceid= '" + Convert.ToString(WorkspaceId) + " ' AND vSubjectId ='" & Gv_SubjectRejection.Rows(e.CommandArgument).Cells(0).Text.Trim() & "'"

                If Not objHelp.GetData("View_fillScreeningDate", "*", Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                             ds_ScreeningData, estr) Then
                    Me.ObjCommon.ShowAlert("Error while getting Subject Screening data.", Me.Page)
                    Exit Sub
                End If

                If Not ds_ScreeningData Is Nothing AndAlso ds_ScreeningData.Tables(0).Rows.Count > 0 Then
                    dv_ScreeningData = ds_ScreeningData.Tables(0).DefaultView
                    dv_ScreeningData.Sort = "dScreenDate desc"
                    latestscreeningdate = Convert.ToString(dv_ScreeningData.ToTable.Rows(0)(2))
                End If
                ''newWin3 = "window.open(""" + "frmSubjectScreening_New.aspx?mode=4" + "&SubjectId=" + Gv_SubjectRejection.Rows(e.CommandArgument).Cells(0).Text + """)"
                newWin3 = "window.open(""" & "frmSubjectScreening_New.aspx?mode=4&Workspace=" + Convert.ToString(WorkspaceId) + "&SubId=" & _
                                       Gv_SubjectRejection.Rows(e.CommandArgument).Cells(0).Text.Trim() & "&ScrDt=" & latestscreeningdate & "&Attendance=true"")"

                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", newWin3, True)
                ''Ended by Aaditya
            Catch
            Finally
            End Try
        End If

        If e.CommandName.ToUpper().Trim() = "AUDIT" Then
            Dim newWin2 As String = String.Empty
            Try
                HSubjectIdAudit.Value = Gv_SubjectRejection.Rows(e.CommandArgument).Cells(0).Text.Trim()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "GetSubjectAudit", "GetSubjectAudit(); ", True)
            Catch
            Finally
            End Try

        End If
    End Sub

    Protected Sub Gv_SubjectRejection_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Gv_SubjectRejection.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'added by vishal
            CType(e.Row.FindControl("ImgLabRpt"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("LnkPIF"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("LnkMSR"), ImageButton).CommandArgument = e.Row.RowIndex
        End If


        'e.Row.Cells(9).Visible = False
        'e.Row.Cells(10).Visible = False
        'e.Row.Cells(11).Visible = False
        'e.Row.Cells(12).Visible = False
        'e.Row.Cells(14).Visible = False
        'e.Row.Cells(15).Visible = False
        ''e.Row.Cells(16).Visible = False
        ''e.Row.Cells(17).Visible = False
        ''e.Row.Cells(18).Visible = False
        ''e.Row.Cells(19).Visible = False
        ''e.Row.Cells(20).Visible = False
        ''e.Row.Cells(21).Visible = False
        ''e.Row.Cells(22).Visible = False
        ''e.Row.Cells(23).Visible = False
        ''e.Row.Cells(24).Visible = False




    End Sub

    Private Function Fillgrid(ByVal SearchedResult As Boolean) As Boolean
        Dim dt_SubjectRejectionDetail As DataTable = Nothing
        Dim ds_SubjectRejectionDetail As New DataSet
        Dim ds_subjectRejectionShow As New DataSet
        Dim dt_SubjectRejectionShow As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim Wstr As String = ""
        Dim estr As String = ""
        Try
            Wstr = "(cRejectionFlag = 'Y' Or CAST(GETDATE() As DATE)  Between Cast(isnull(dBlockfrom,'') As Date) And Cast(isnull(dBlockto,'') As Date))" '' or added by prayag
            objHelp.View_SubjectRejectionDetail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_subjectRejectionShow, estr)


            Me.Gv_SubjectRejection.DataSource = ds_subjectRejectionShow.Tables(0)

            Me.Gv_SubjectRejection.DataBind()

        Catch
        Finally
        End Try


    End Function

    <Web.Services.WebMethod()> _
    Public Shared Function GetSubjectAudit(ByVal SubjectId As String) As String
        ' added by prayag
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim ds As DataSet
        Dim eStr As String = String.Empty
        Dim Parameter As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim dtTempAuditTrail As New DataTable

        Try

            Parameter = SubjectId.ToString()

            ds = objHelp.ProcedureExecute("dbo.GetRejectedSubjectAudit", Parameter.ToString)


            If Not dtTempAuditTrail Is Nothing Then
                dtTempAuditTrail.Columns.Add("SrNo", GetType(String))
                dtTempAuditTrail.Columns.Add("FullName", GetType(String))
                dtTempAuditTrail.Columns.Add("vSubjectId", GetType(String))
                dtTempAuditTrail.Columns.Add("cRejectionFlag", GetType(String))
                dtTempAuditTrail.Columns.Add("vRemark", GetType(String))
                dtTempAuditTrail.Columns.Add("dBlockfrom", GetType(String))
                dtTempAuditTrail.Columns.Add("dBlockto", GetType(String))
                dtTempAuditTrail.Columns.Add("ModifyBy", GetType(String))
                dtTempAuditTrail.Columns.Add("dModifyOn", GetType(String))

            End If

            dtTempAuditTrail.AcceptChanges()

            Dim drAuditTrail As DataRow

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                drAuditTrail = dtTempAuditTrail.NewRow()
                drAuditTrail("SrNo") = Convert.ToString(ds.Tables(0).Rows(i)("SrNo"))
                drAuditTrail("vSubjectId") = Convert.ToString(ds.Tables(0).Rows(i)("vSubjectId").ToString())
                drAuditTrail("FullName") = Convert.ToString(ds.Tables(0).Rows(i)("FullName").ToString())
                drAuditTrail("cRejectionFlag") = Convert.ToString(ds.Tables(0).Rows(i)("cRejectionFlag").ToString())
                drAuditTrail("vRemark") = Convert.ToString(ds.Tables(0).Rows(i)("vRemark"))
                drAuditTrail("dBlockfrom") = Convert.ToString(ds.Tables(0).Rows(i)("dBlockfrom"))
                drAuditTrail("dBlockto") = Convert.ToString(ds.Tables(0).Rows(i)("dBlockto"))
                drAuditTrail("ModifyBy") = Convert.ToString(ds.Tables(0).Rows(i)("ModifyBy"))
                drAuditTrail("dModifyOn") = Convert.ToString(ds.Tables(0).Rows(i)("dModifyOn"))
                dtTempAuditTrail.Rows.Add(drAuditTrail)
                dtTempAuditTrail.AcceptChanges()
            Next i

            strReturn = JsonConvert.SerializeObject(dtTempAuditTrail)

            Return strReturn

        Catch ex As Exception
            Return ex.Message
            Return False
        End Try
        Return True
    End Function
End Class
