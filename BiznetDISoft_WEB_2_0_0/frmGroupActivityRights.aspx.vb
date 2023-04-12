Imports System.Web.Services
Imports Newtonsoft.Json
Partial Class frmGroupActivityRights
    Inherits System.Web.UI.Page


#Region "Variable Declaration"

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private Const VS_Choice As String = "Choice"
    Dim VS_ForExport As String = "ExportExcel"

    Private Const GRDVCell_nActivityRoleId As Integer = 0
    Private Const GRDVCell_vActivityGroupId As Integer = 1
    Private Const GRDVCell_UserTypeCode As Integer = 3

    Private Const GV_ActOperation_Delete As Integer = 7

#End Region

#Region "Load Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                AutoCompleteExtender1.ContextKey = "isnull(vParentWorkspaceId,'') = ''"
                GenCall()
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...Page_Load")
        End Try
    End Sub

#End Region

#Region "GenCall"
    Private Function GenCall() As Boolean

        Try
            If Not GenCall_Data() Then 'For Data Retrieval
                Return False
            End If

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Return False
            End If

            GenCall = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall")
            Return False
        End Try
    End Function
#End Region

#Region "GenCall_Data "
    Private Function GenCall_Data() As Boolean
        Dim ds As DataSet
        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Val As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try

            ds = Me.objHelp.GetResultSet("Select * from  ScreeningGroupUserMatrix Where 1=2 ", "ScreeningGroupUserMatrix")
            Me.ViewState("ActivityRoleMatrix") = ds

            GenCall_Data = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_Data")
            Return False
        Finally
            ds = Nothing
        End Try
    End Function
#End Region

#Region "GenCall_showUI"
    Private Function GenCall_ShowUI() As Boolean

        Try
            Page.Title = " :: Screening Group Rights ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Screening Group Operation Rights"

            'Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID) & " And cWorkspaceType='P'"



            If Not FillDropDown() Then
                Return False
            End If

            If Not FillGrid() Then
                Return False
            End If

            GenCall_ShowUI = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_showUI")
            Return False
        Finally
        End Try
    End Function
#End Region

#Region "FillFunction"

    Private Function FillDropDown() As Boolean
        Dim ds_Act As New Data.DataSet
        Dim dv_Act As New DataView
        Dim ds_Op As New Data.DataSet
        Dim ds_Location As New Data.DataSet
        Dim ds_Dept As New Data.DataSet
        Dim dv_Op As New DataView
        Dim ds_UserType As New Data.DataSet
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty
        Try


            'ds_Act = Me.objHelp.GetResultSet("Select Distinct vMedExGroupCode,vmedexgroupDesc  from ScreeningTemplateDtl  where cStatusIndi<>'D' ", "ScreeningTemplateDtl")


            'ds_Act = Me.objHelp.ProcedureExecute("dbo.Proc_GetGroupFetch", Wstr)


            If Not objHelp.getUserTypeMst("cStatusIndi <> '" + Status_Delete + "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_UserType, estr) Then
                Return False
            End If


            'dv_Act = ds_Act.Tables(0).DefaultView
            'dv_Act.Sort = "vmedexgroupDesc"

            'Me.ChklstActivity.DataSource = dv_Act.ToTable()
            'Me.ChklstActivity.DataValueField = "vMedExGroupCode"
            'Me.ChklstActivity.DataTextField = "vmedexgroupDesc"
            'Me.ChklstActivity.DataBind()

            rbtnDefault_SelectedIndexChanged(Nothing, Nothing)

            Me.chklstUserType.DataSource = ds_UserType
            Me.chklstUserType.DataValueField = "vUserTypeCode"
            Me.chklstUserType.DataTextField = "vUserTypeName"
            Me.chklstUserType.DataBind()


            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillDropDown")
            Return False
        Finally
            ds_Act = Nothing
        End Try
    End Function

    Private Function FillGrid() As Boolean

        Dim DsGrid As DataSet = Nothing
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty

        Try

            If rbtnDefault.SelectedIndex = 0 Then
                Wstr = Session(S_UserID).ToString() + "##" + "0000000000"
            Else
                If Me.HProjectId.Value = "" Then
                    Me.GV_ActOperation.DataSource = Nothing
                    Me.GV_ActOperation.DataBind()
                    Return False
                    Exit Function
                Else
                    Wstr = Session(S_UserID).ToString() + "##" + Me.HProjectId.Value
                End If

            End If
            DsGrid = objHelp.ProcedureExecute("dbo.GetScreeningGroupUserMatrix", Wstr.ToString)
            Me.GV_ActOperation.DataSource = Nothing
            Me.ViewState("ActivityGrid") = DsGrid
            If Not DsGrid Is Nothing AndAlso DsGrid.Tables(0).Rows.Count > 0 Then
                Me.GV_ActOperation.DataSource = DsGrid
                Me.GV_ActOperation.DataBind()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideEntryData", "displayCRFInfo(" + Me.imgfldgen.ClientID + ",'tblEntryData');", True)
            Else
                ObjCommon.ShowAlert("No Data Found!", Me.Page)
            End If
            upgrid.Update()


            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillGrid")
            Return False
        Finally
            DsGrid = Nothing
        End Try
    End Function

    Private Function AssignValues() As Boolean
        Dim dr As DataRow
        Dim dS_ActivityOpMtx As New DataSet
        Dim Act As Integer
        Dim UT As Integer
        Dim ds_Check As New DataSet
        Dim dv_Check As New DataView
        Dim wStr As String = String.Empty
        Dim ActivityExists As String = String.Empty
        Try

            dS_ActivityOpMtx = CType(Me.ViewState("ActivityRoleMatrix"), DataSet).Copy()
            dS_ActivityOpMtx.Tables(0).Clear()

            For Act = 0 To Me.ChklstActivity.Items.Count - 1
                If Me.ChklstActivity.Items(Act).Selected = True Then

                    ds_Check = CType(Me.ViewState("ActivityGrid"), DataSet).Copy()

                    For UT = 0 To Me.chklstUserType.Items.Count - 1
                        If Me.chklstUserType.Items(UT).Selected = True Then

                            If ds_Check.Tables(0).Rows.Count > 0 Then
                                dv_Check = ds_Check.Tables(0).DefaultView
                                If dv_Check.ToTable().Rows.Count > 0 Then
                                    dv_Check.RowFilter = "vMedExGroupCode = '" + Me.ChklstActivity.Items(Act).Value.Trim() + "' And " + _
                                                         "vUserTypeCode = '" + Me.chklstUserType.Items(UT).Value.Trim() + "' And " + _
                                                          "vWorkSpaceid = '" + IIf(Me.HProjectId.Value.Trim() <> "", HProjectId.Value.Trim(), "0000000000") + "'"

                                    If dv_Check.ToTable().Rows.Count > 0 Then

                                        ActivityExists += "Activity Group= '" + Me.ChklstActivity.Items(Act).Text.Trim() + _
                                                        "', User Type = '" + Me.chklstUserType.Items(UT).Text.Trim() + "'\n"
                                        Continue For
                                    End If
                                End If
                            End If

                            dr = dS_ActivityOpMtx.Tables(0).NewRow()
                            dr("vMedExGroupCode") = Me.ChklstActivity.Items(Act).Value.Trim()
                            dr("vUserTypeCode") = Me.chklstUserType.Items(UT).Value.Trim()
                            dr("cStatusIndi") = Status_New
                            dr("iModifyBy") = Me.Session(S_UserID)
                            If rbtnDefault.SelectedIndex = 0 Then
                                dr("vWorkSpaceId") = "0000000000"
                            Else
                                dr("vWorkSpaceId") = HProjectId.Value
                            End If
                            dS_ActivityOpMtx.Tables(0).Rows.Add(dr)
                            dS_ActivityOpMtx.Tables(0).AcceptChanges()
                        End If

                    Next UT
                End If
            Next Act

            Me.ViewState("ActivityRoleMatrix") = dS_ActivityOpMtx.Tables(0)
            If ActivityExists <> "" Then
                ObjCommon.ShowAlert("Record Already Exists For \n" + ActivityExists, Me.Page)
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...AssignValues")
            Return False
        Finally
            dS_ActivityOpMtx = Nothing
            ds_Check = Nothing
            dv_Check = Nothing
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
                    strMessage.Append("<td colspan=""5""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                    strMessage.Append("</font></strong><center></b></td>")
                    strMessage.Append("</tr>")
                    strMessage.Append("<tr>")
                    strMessage.Append("</tr>")
                    strMessage.Append("<tr>")
                    strMessage.Append("<td colspan=""5""><center><strong><b><font color=""#000099"" size=""2.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    If rbtnDefault.SelectedIndex = 0 Then
                        strMessage.Append("Generic Screening Attribute Group Report ")
                    ElseIf rbtnDefault.SelectedIndex = 1 Then
                        strMessage.Append("Project Specific Screening Attribute Group Report ")
                    End If

                    strMessage.Append("</font></strong><center></b></td>")
                    strMessage.Append("</tr>")
                    strMessage.Append("<tr><td><font color=""#000099"" size=""2.5"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""3"" face=""Verdana""><b>" + Date.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")

                Case "btnexportAudit"
                    strMessage.Append("<td colspan=""5""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                    strMessage.Append("</font></strong><center></b></td>")
                    strMessage.Append("</tr>")
                    strMessage.Append("<tr>")
                    strMessage.Append("</tr>")
                    strMessage.Append("<tr>")
                    strMessage.Append("<td colspan=""5""><center><strong><b><font color=""#000099"" size=""2.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append("InActivated Information")
                    strMessage.Append("</font></strong><center></b></td>")
                    strMessage.Append("</tr>")
                    strMessage.Append("<tr><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""3"" face=""Verdana""><b>" + Date.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")
            End Select
            strMessage.Append("<tr>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")

            If ViewState("IdOfBtn") = "btnExport" Then

                dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vmedexgroupDesc,vUserTypeName,ProjectName,vUserName,dModifyOn".Split(",")).DefaultView.ToTable())
                dsConvert.AcceptChanges()
                dsConvert.Tables(0).Columns(0).ColumnName = "Screening Group Name"
                dsConvert.Tables(0).Columns(1).ColumnName = "User Type Name"
                dsConvert.Tables(0).Columns(2).ColumnName = "Project Name"
                dsConvert.Tables(0).Columns(3).ColumnName = "ModifiedBy"
                dsConvert.Tables(0).Columns(4).ColumnName = "ModifiedOn"

                dsConvert.AcceptChanges()
            ElseIf ViewState("IdOfBtn") = "btnexportAudit" Then
                dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vmedexgroupDesc,vUserTypeName,vRemarks,vUserName,dModifyOn".Split(",")).DefaultView.ToTable())
                dsConvert.AcceptChanges()
                dsConvert.Tables(0).Columns(0).ColumnName = "Screening Group Name"
                dsConvert.Tables(0).Columns(1).ColumnName = "User Type Name"
                dsConvert.Tables(0).Columns(2).ColumnName = "Remarks"
                dsConvert.Tables(0).Columns(3).ColumnName = "ModifiedBy"
                dsConvert.Tables(0).Columns(4).ColumnName = "ModifiedOn"

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

#Region "Grid Event"

    Protected Sub GV_ActOperation_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV_ActOperation.RowDataBound
        Try

            If e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.Footer Or e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(GRDVCell_nActivityRoleId).Visible = False
                e.Row.Cells(GRDVCell_vActivityGroupId).Visible = False
                e.Row.Cells(GRDVCell_UserTypeCode).Visible = False
                e.Row.Cells(9).Visible = False
                If e.Row.RowType = DataControlRowType.DataRow Then
                    CType(e.Row.FindControl("ImgDelete"), ImageButton).CommandArgument = e.Row.RowIndex
                    CType(e.Row.FindControl("ImgDelete"), ImageButton).CommandName = "Delete"
                    e.Row.Cells(GV_ActOperation_Delete).HorizontalAlign = HorizontalAlign.Center
                End If
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GV_ActOperation_RowDataBound")
        End Try
    End Sub

    Protected Sub GV_ActOperation_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GV_ActOperation.RowCommand
        Try
            If e.CommandName.ToUpper = "DELETE" Then

                Me.HfGridIndex.Value = CType(e.CommandArgument, Integer)
                mdlRemarks.Show()

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GV_ActOperation_RowCommand")
        Finally

        End Try
    End Sub

    Protected Sub GV_ActOperation_RowDeleting(sender As Object, e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GV_ActOperation.RowDeleting
    End Sub

#End Region

#Region "Button Event"

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim ds_Save As New DataSet
        Dim estr As String = String.Empty
        Dim selected As Boolean = False
        Try

            If Not AssignValues() Then
                Exit Sub
            End If

            ds_Save = New DataSet

            ds_Save.Tables.Add(CType(Me.ViewState("ActivityRoleMatrix"), DataTable).Copy())
            ds_Save.Tables(0).TableName = "Insert_ScreeningGroupUserMatrix"
            ds_Save.Tables(0).AcceptChanges()

            If ds_Save.Tables(0).Rows.Count > 0 Then

                If Not objLambda.TableInsert(ds_Save, Me.Session(S_UserID), estr) Then
                    Throw New Exception("Error while saving ActivityRoleMatrix")
                Else
                    ObjCommon.ShowAlert("Record Saved Successfully !", Me.Page)

                End If
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...BtnSave_Click")
        Finally
            ds_Save = Nothing
            ResetPage()
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.ResetPage()
    End Sub

    Protected Sub btnSaveRemarks_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveRemarks.Click

        Dim index As Integer
        Dim dS_ActivityOpMtx As New DataSet
        Dim Dr_ActOp As DataRow
        Dim estr As String = String.Empty

        Try

            dS_ActivityOpMtx = CType(Me.ViewState("ActivityRoleMatrix"), DataSet)
            dS_ActivityOpMtx.Tables(0).Clear()


            index = CType(Me.HfGridIndex.Value, Integer)

            Dr_ActOp = dS_ActivityOpMtx.Tables(0).NewRow()

            Dr_ActOp("vMedExGroupCode") = Me.GV_ActOperation.Rows(index).Cells(GRDVCell_vActivityGroupId).Text
            Dr_ActOp("vUserTypeCode") = Me.GV_ActOperation.Rows(index).Cells(GRDVCell_UserTypeCode).Text
            Dr_ActOp("vRemarks") = Me.txtRemarks.Text()
            Dr_ActOp("cStatusIndi") = Status_Delete
            Dr_ActOp("iModifyBy") = Me.Session(S_UserID)
            If rbtnDefault.SelectedIndex = 0 Then
                Dr_ActOp("vWorkSpaceId") = "0000000000"
            Else
                Dr_ActOp("vWorkSpaceId") = Me.GV_ActOperation.Rows(index).Cells(9).Text
            End If

            dS_ActivityOpMtx.Tables(0).Rows.Add(Dr_ActOp)
            dS_ActivityOpMtx.Tables(0).TableName = "Insert_ScreeningGroupUserMatrix"
            dS_ActivityOpMtx.Tables(0).AcceptChanges()



            If dS_ActivityOpMtx.Tables(0).Rows.Count > 0 Then
                If Not objLambda.TableInsert(dS_ActivityOpMtx, Me.Session(S_UserID), estr) Then
                    Throw New Exception("Error while saving ActivityRoleMatrix")
                Else
                    ObjCommon.ShowAlert("Record InActivated Successfully !", Me.Page)
                End If
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GV_ActOperation_RowCommand")
        Finally
            Me.HfGridIndex.Value = ""
            dS_ActivityOpMtx = Nothing
            ResetPage()
        End Try

    End Sub

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click

        Dim ds_Field As New DataSet
        Dim fileName As String = String.Empty
        Dim dt_Final As New DataTable

        Try
            ds_Field = CType(ViewState("ActivityGrid"), DataSet).Copy()

            fileName = "Screening Group Report" + " " + Date.Today.ToString("dd-MMM-yyyy") + ".xls"

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
            ds_Field.Tables(0).TableName = "InactivatedReportData"
            fileName = "InActivated Information" + " " + Date.Today.ToString("dd-MMM-yyyy") + ".xls"

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

    Protected Sub btnSetProject_Click(sender As Object, e As EventArgs)
        rbtnDefault_SelectedIndexChanged(Nothing, Nothing)
    End Sub

#End Region

#Region "Radio Button Event"

    Protected Sub rbtnDefault_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ds_Activity As DataSet

        Try
            ChklstActivity.Style.Add("display", "")
            If rbtnDefault.SelectedIndex = 1 Then
                trProjectSpecific.Visible = True
                If HProjectId.Value <> "" Then
                    ds_Activity = Me.objHelp.ProcedureExecute("dbo.Proc_GetGroupFetch", HProjectId.Value)
                End If
                ChklstActivity.Style.Add("display", "")
                If Not ds_Activity Is Nothing AndAlso ds_Activity.Tables(0).Rows.Count > 0 Then

                    ChklstActivity.Items.Clear()
                    ChklstActivity.Style.Add("display", "")
                    Me.ChklstActivity.DataSource = ds_Activity.Tables(0)
                    Me.ChklstActivity.DataValueField = "vMedExGroupCode"
                    Me.ChklstActivity.DataTextField = "vmedexgroupDesc"
                    Me.ChklstActivity.DataBind()
                Else
                    ChklstActivity.Items.Clear()
                    ChklstActivity.Items.Add("No Data Found")
                    ChklstActivity.Style.Add("display", "none")

                End If
                upgrid.Update()
            Else
                trProjectSpecific.Visible = False
                txtProject.Text = ""
                HProjectId.Value = ""
                ds_Activity = Me.objHelp.ProcedureExecute("dbo.Proc_GetGroupFetch", "0000000000")
                If Not ds_Activity Is Nothing AndAlso ds_Activity.Tables(0).Rows.Count > 0 Then
                    Me.ChklstActivity.DataSource = ds_Activity.Tables(0)
                    Me.ChklstActivity.DataValueField = "vMedExGroupCode"
                    Me.ChklstActivity.DataTextField = "vmedexgroupDesc"
                    Me.ChklstActivity.DataBind()
                Else
                    Me.ChklstActivity.DataSource = Nothing
                    Me.ChklstActivity.DataBind()
                End If
                upgrid.Update()
            End If
            FillGrid()
        Catch ex As Exception

        End Try

    End Sub

#End Region

#Region "Web Method"
    <WebMethod> _
    Public Shared Function AuditTrail(ByVal iUserid As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_Clientmst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty

        Dim dtClientMstHistory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable
        Try

            wStr = Status_Delete + "##" + iUserid.ToString()
            ds_Clientmst = objHelp.ProcedureExecute("dbo.GetScreeningGroupUserMatrixHistory", wStr.ToString)

            If Not dtClientMstHistory Is Nothing Then

                dtClientMstHistory.Columns.Add("vmedexgroupDesc")
                dtClientMstHistory.Columns.Add("vUserTypeName")
                dtClientMstHistory.Columns.Add("vRemarks")
                dtClientMstHistory.Columns.Add("vUserName")
                dtClientMstHistory.Columns.Add("dModifyOn")
            End If
            dt = ds_Clientmst.Tables(0)
            Dim dv As New DataView(dt)

            For Each dr As DataRow In dv.ToTable.Rows

                drAuditTrail = dtClientMstHistory.NewRow()


                drAuditTrail("vmedexgroupDesc") = dr("vmedexgroupDesc").ToString()
                drAuditTrail("vUserTypeName") = dr("vUserTypeName").ToString()
                drAuditTrail("vRemarks") = dr("vRemarks").ToString()
                drAuditTrail("vUserName") = dr("vUserName").ToString()
                drAuditTrail("dModifyOn") = Convert.ToString(dr("dModifyOn"))
                dtClientMstHistory.Rows.Add(drAuditTrail)
                dtClientMstHistory.AcceptChanges()
                i += 1
            Next
            strReturn = JsonConvert.SerializeObject(dtClientMstHistory)

            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
#End Region

#Region "Reset Page"
    Private Sub ResetPage()
        Try
            Me.ChklstActivity.Items.Clear()
            Me.chklstUserType.Items.Clear()
            Me.ViewState("ActivityRoleMatrix") = Nothing
            Me.ViewState("ActivityGrid") = Nothing
            If Not Me.GenCall() Then
                Exit Sub
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...ResetPage")
        End Try
    End Sub
#End Region

#Region "Error Handler"
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        ObjCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub
#End Region

End Class
