Imports System.Web.Services
Imports Newtonsoft.Json

Partial Class frmActivityOperationMatrix
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtActivityOpMtx As String = "DtActivityOpMtx"
    Private Const VS_ActivityOpCode As String = "ActivityOpCode"
    Dim VS_ForExport As String = "ExportExcel"

    Private Const GRDVCell_ActivityRoleId As Integer = 1
    Private Const GRDVCell_ActivityId As Integer = 2
    Private Const GRDVCell_UserTypeCode As Integer = 4
    Private Const GRDVCell_DeptCode As Integer = 6
    Private Const GRDVCell_LocationCode As Integer = 8
    Private Const GRDVCell_OperationCode As Integer = 10

    Private Const GV_ActOperation_SrNo As Integer = 0
    Private Const GV_ActOperation_Delete As Integer = 12

    Dim ds_Activity As New DataSet
    Dim ds_UserName As New DataSet
    Dim ds_RoleOp As New Data.DataSet


    Private Const VS_CurrentPage As String = "PageNo"
    Private Const VS_TotalRowCount As String = "TotalRowCount"
    Private Const VS_PagerStartPage As String = "PagerStartPage"
    Private Const PAGESIZE As Integer = 1000
    Private Const VS_Grid As String = "Gridtable"


#End Region

#Region "Load Event"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.SetPaging()

            If Not IsPostBack Then
                GenCall()
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...Page_Load")
        End Try
    End Sub

#End Region

#Region "GenCall"
    Private Function GenCall() As Boolean
        Dim ds As New DataSet
        'Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try


            'Choice = Me.Request.QueryString("Mode")
            Me.ViewState(VS_Choice) = Me.Request.QueryString("Mode")   'To use it while saving

            If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_ActivityOpCode) = Me.Request.QueryString("Value").ToString
            End If

            If Not GenCall_Data(ds) Then 'For Data Retrieval
                Return False
            End If

            Me.ViewState(VS_DtActivityOpMtx) = ds.Tables("ActivityOperationMatrix")   ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Return False
            End If
            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideSponsorDetails", "HideSponsorDetails(); ", True)
            End If
            GenCall = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall")
            Return False
        End Try
    End Function
#End Region

#Region "GenCall_Data "
    Private Function GenCall_Data(ByRef ds_DWR_Retu As DataSet) As Boolean
        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Val As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try


            Val = Me.ViewState(VS_ActivityOpCode) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vActivityRoleId=" + Val.ToString
            End If

            If Not objHelp.GetActivityOperationMatrix(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_DWR_Retu, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Return False
            End If

            If ds_DWR_Retu Is Nothing Then
                Throw New Exception(eStr_Retu)
            End If

            If ds_DWR_Retu.Tables(0).Rows.Count <= 0 And _
             Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Throw New Exception("No Records Found for Selected Operation")
            End If
            ds_DWR_Retu.Tables(0).AcceptChanges()

            GenCall_Data = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_Data")
            Return False
        End Try
    End Function
#End Region

#Region "GenCall_showUI"
    Private Function GenCall_ShowUI() As Boolean

        Try
            Page.Title = " :: Activity Operation Matrix ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "UserType Activity Operation Rights"

            If Not FillDropDown() Then
                Return False
            End If

            If Not FillGrid() Then
                Return False
            End If

            If Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then 'for View and Edit Mode
                Me.BtnSave.Text = "Update"
                Me.BtnSave.ToolTip = "Update"
            End If

            GenCall_ShowUI = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_showUI")
            Return False
        Finally
        End Try
    End Function
#End Region

#Region "FillDropDown"

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

            'To Get Where condition of ScopeVales( Project Type )
            If Not ObjCommon.GetScopeValueWithCondition(Wstr) Then
                Return False
            End If

            If Not objHelp.GetActivityCodeDetails(ds_Act, Wstr, estr) Then
                Return False
            End If

            If Not objHelp.getOperationMst("cStatusIndi <> '" + Status_Delete + "' And vOperationCode < '0020' and vOperationCode > '0001'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Op, estr) Then
                Return False
            End If

            If Not objHelp.getUserTypeMst("cStatusIndi <> '" + Status_Delete + "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_UserType, estr) Then
                Return False
            End If

            If Not objHelp.getLocationMst("cStatusIndi <> '" + Status_Delete + "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Location, estr) Then
                Return False
            End If

            If Not objHelp.GetDeptMst("cStatusIndi <> '" + Status_Delete + "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Dept, estr) Then
                Return False
            End If

            dv_Act = ds_Act.Tables(0).DefaultView
            dv_Act.Sort = "vActivityName"

            Me.ChklstActivity.DataSource = dv_Act.ToTable()
            Me.ChklstActivity.DataValueField = "vActivityId"
            Me.ChklstActivity.DataTextField = "vActivityName"
            Me.ChklstActivity.DataBind()

            Me.chklstOperation.DataSource = ds_Op.Tables(0)
            Me.chklstOperation.DataValueField = "vOperationCode"
            Me.chklstOperation.DataTextField = "vOperationName"
            Me.chklstOperation.DataBind()

            Me.chklstUserType.DataSource = ds_UserType
            Me.chklstUserType.DataValueField = "vUserTypeCode"
            Me.chklstUserType.DataTextField = "vUserTypeName"
            Me.chklstUserType.DataBind()

            Me.chklstLocation.DataSource = ds_Location
            Me.chklstLocation.DataValueField = "vLocationCode"
            Me.chklstLocation.DataTextField = "vLocationName"
            Me.chklstLocation.DataBind()

            Me.chklstDept.DataSource = ds_Dept
            Me.chklstDept.DataValueField = "vDeptCode"
            Me.chklstDept.DataTextField = "vDeptName"
            Me.chklstDept.DataBind()

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillDropDown")
            Return False
        End Try
    End Function

#End Region

#Region "FillGrid"

    Private Function FillGrid() As Boolean
        Dim dv_RoleOp As New DataView
        Dim dt_ActName As New DataTable
        Dim dt_UserName As New DataTable
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim ActivityName As String = String.Empty
        Dim UserName As String = String.Empty
        Dim Parameters As String = String.Empty
        Dim ds_RoleOpCount As New DataSet

        Try

            'To Get Where condition of ScopeVales( Project Type )
            If Not ObjCommon.GetScopeValueWithCondition(Wstr) Then
                Return False
            End If

            If Me.ViewState(VS_PagerStartPage) Is Nothing OrElse Me.ViewState(VS_CurrentPage) Is Nothing Then
                Me.ViewState(VS_PagerStartPage) = "1"
                Me.ViewState(VS_CurrentPage) = "1"
            End If

            Parameters = "0" + "##" + "0"
            If objHelp.Proc_GetActivityOperationMatrix(Parameters, ds_RoleOpCount, estr) Then
                Me.ViewState(VS_TotalRowCount) = ds_RoleOpCount.Tables(0).Rows(0)("TotalRow")
            End If


            Parameters = Convert.ToString(Me.ViewState(VS_CurrentPage)).Trim() + "##" + PAGESIZE.ToString()
            If objHelp.Proc_GetActivityOperationMatrix(Parameters, ds_RoleOp, estr) Then
                '================(29-05-2012)by: vikas shah(to get distinct activity name and usertypename) ===================

                dt_ActName = ds_RoleOp.Tables(0).DefaultView.ToTable(True, "vActivityName")
                DdlActivityName.DataSource = dt_ActName
                DdlActivityName.DataTextField = "vActivityName"
                DdlActivityName.DataBind()

                DdlActivityName.Items.Insert(0, "Select Activity")
                dt_UserName = ds_RoleOp.Tables(0).DefaultView.ToTable(True, "vUserTypeName")
                DdlUserType.DataSource = dt_UserName
                DdlUserType.DataTextField = "vUserTypeName"
                DdlUserType.DataBind()
                DdlUserType.Items.Insert(0, "Select UserType")

                Me.GV_ActOperation.DataSource = ds_RoleOp
                Me.GV_ActOperation.DataBind()
                Me.ViewState(VS_Grid) = ds_RoleOp
                'If GV_ActOperation.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIGV_ActOperation", "UIGV_ActOperation(); ", True)
            End If
            '==============================================================================================================

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillGrid")
            Return False
        End Try
    End Function

#End Region

#Region "Button Click"

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim ds_Save As New DataSet
        Dim dt_Save As New DataTable
        Dim estr As String = String.Empty
        Dim selected As Boolean = False

        Try

            If Not AssignValues() Then
                Exit Sub
            End If

            ds_Save = New DataSet
            dt_Save = CType(Me.ViewState(VS_DtActivityOpMtx), DataTable)
            dt_Save.TableName = "ActivityOperationMatrix"
            ds_Save.Tables.Add(dt_Save)

            If ds_Save.Tables(0).Rows.Count > 0 Then
                If Not objLambda.Save_InsertActivityOperationMatrix(Me.ViewState(VS_Choice), ds_Save, Me.Session(S_UserID), estr) Then
                    ObjCommon.ShowAlert("Error while saving activityOperationMatrix", Me.Page)
                    Exit Sub
                End If
            End If

            ObjCommon.ShowAlert("Record Saved SuccessFully !", Me.Page)
            ResetPage()
            ' btnCancel_Click(sender, e)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...BtnSave_Click")
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.ResetPage()
    End Sub

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click
        Dim ActivityName As String = Nothing
        Dim wstr As String = Nothing
        Dim UserType As String = Nothing
        Dim estr As String = Nothing
        Me.ViewState(VS_Grid) = Nothing
        Try

            If (DdlActivityName.SelectedIndex <> 0) Then
                If (DdlUserType.SelectedIndex <> 0) Then
                    ActivityName = DdlActivityName.SelectedItem.Text
                    UserType = DdlUserType.SelectedItem.Text
                    wstr += "cStatusIndi <> '" + Status_Delete + "' And vActivityName='" + ActivityName + "' And vUserTypeName='" + UserType + "'"
                    If Not objHelp.View_ActivityOperationMatrix(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Activity, estr) Then
                        Me.ShowErrorMessage("Error while getting data from View_ActivityOperationMatrix", estr)
                        Exit Sub
                    End If
                    Me.GV_ActOperation.DataSource = ds_Activity
                    Me.GV_ActOperation.DataBind()
                    Me.GV_ForExportToExcel.DataSource = ds_Activity
                    Me.GV_ForExportToExcel.DataBind()
                    Me.ViewState(VS_Grid) = ds_Activity
                Else
                    ActivityName = DdlActivityName.SelectedItem.Text
                    wstr += "cStatusIndi <> '" + Status_Delete + "' And vActivityName='" + ActivityName + "'"
                    If Not objHelp.View_ActivityOperationMatrix(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Activity, estr) Then
                        Me.ShowErrorMessage("Error while getting data from View_ActivityOperationMatrix", estr)
                        Exit Sub
                    End If
                    Me.GV_ActOperation.DataSource = ds_Activity
                    Me.GV_ActOperation.DataBind()
                    Me.GV_ForExportToExcel.DataSource = ds_Activity
                    Me.GV_ForExportToExcel.DataBind()
                    Me.ViewState(VS_Grid) = ds_Activity
                End If
            Else
                If DdlUserType.SelectedIndex <> 0 Then
                    UserType = DdlUserType.SelectedItem.Text
                    wstr += "cStatusIndi <> '" + Status_Delete + "' And vUserTypeName='" + UserType + "'"
                    If Not objHelp.View_ActivityOperationMatrix(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_UserName, estr) Then
                        Me.ShowErrorMessage("Error while getting data from View_ActivityOperationMatrix", estr)
                        Exit Sub
                    End If
                    Me.GV_ActOperation.DataSource = ds_UserName
                    Me.GV_ActOperation.DataBind()
                    Me.GV_ForExportToExcel.DataSource = ds_UserName
                    Me.GV_ForExportToExcel.DataBind()
                    Me.ViewState(VS_Grid) = ds_UserName
                End If
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIGV_ActOperation", "UIGV_ActOperation(); ", True)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnGo_Click")
        End Try
    End Sub


    Protected Sub btnImportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportToExcel.Click
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim ds_RoleOp As New Data.DataSet

        Dim ds_Field As New DataSet
        Dim fileName As String = String.Empty
        Dim dt_Final As New DataTable
        Dim ds_grid As New DataSet
        Dim dt_grid As New DataTable
        'dt_grid = GV_ForExportToExcel.DataSource
        'ds_grid.Tables.Add(dt_grid)

        'dt_grid = ViewState("VS_Grid")

        ds_grid = CType(ViewState(VS_Grid), DataSet)

        'Try

        '    If Not ObjCommon.GetScopeValueWithCondition(Wstr) Then
        '        Return
        '    End If

        '    Wstr += " AND cStatusIndi <> 'D'"
        '    If Not objHelp.View_ActivityOperationMatrix(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_RoleOp, estr) Then
        '        Return
        '    End If
        '    strReturn = JsonConvert.SerializeObject(ds_RoleOp)


        'Catch ex As Exception
        '    Return
        'End Try

        Try
            fileName = "ProjectStatusDetail-" + Date.Today.ToString("dd-MMM-yyyy") + ".xls"


            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)

            Context.Response.Write(ConvertDsuserTO(ds_grid))

            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)
        Catch ex As Exception

        End Try

    End Sub
    Private Function ConvertDsuserTO(ByVal ds As DataSet) As String
        Dim iCol, i, j As Integer
        Dim dsConvert As New DataSet
        Dim strMessage As New StringBuilder
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim ds_RoleOp As New Data.DataSet
        Dim Dt_Actopt As New DataTable

        strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")

        strMessage.Append("<tr>")
        strMessage.Append("<td colspan=""5""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
        strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
        strMessage.Append("</font></strong><center></b></td>")
        strMessage.Append("</tr>")
        strMessage.Append("<tr><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""3"" face=""Verdana""><b>" + Date.Today.ToString("dd-MMM-yyyy") + "</b></font></td><td></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")
        'strMessage.Append("</table>")

        dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vActivityName,vUserTypeName,vDeptName,vLocationName,vOperationName".Split(",")).DefaultView.ToTable())
        dsConvert.AcceptChanges()
        dsConvert.Tables(0).Columns(0).ColumnName = "Activity Name"
        dsConvert.Tables(0).Columns(1).ColumnName = "UserType Name"
        dsConvert.Tables(0).Columns(2).ColumnName = "Dept Name"
        dsConvert.Tables(0).Columns(3).ColumnName = "Location Name"
        dsConvert.Tables(0).Columns(4).ColumnName = "Operation Name"

        dsConvert.AcceptChanges()
        For iCol = 0 To dsConvert.Tables(0).Columns.Count - 1

            strMessage.Append("<td bgcolor=""#1560a1""><strong><font color=""#FFFFFF"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append(Convert.ToString(dsConvert.Tables(0).Columns(iCol)).Trim())
            strMessage.Append("</font></strong></td>")

        Next

        strMessage.Append("</tr>")

        strMessage.Append("<tr>")
        strMessage.Append("</tr>")

        For j = 0 To dsConvert.Tables(0).Rows.Count - 1
            strMessage.Append("<tr>")
            For i = 0 To dsConvert.Tables(0).Columns.Count - 1

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
    End Function

    'Protected Sub btnImportToExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportToExcel.Click
    '    Try

    '        If GV_ActOperation.Rows.Count <> 0 Then
    '            Dim stringWriter As New System.IO.StringWriter()
    '            Dim writer As New HtmlTextWriter(stringWriter)
    '            GV_ForExportToExcel.RenderControl(writer)
    '            Dim gridViewhtml As String = stringWriter.ToString()
    '            Dim fileName As String = "ProjectStatusDetail" & ".xls"
    '            Context.Response.Buffer = True
    '            Context.Response.ClearContent()
    '            Context.Response.ClearHeaders()
    '            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
    '            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)
    '            Context.Response.Write(gridViewhtml.Substring(5).Substring(0, gridViewhtml.Substring(5).Length - 6))
    '            GV_ForExportToExcel.DataSource = Nothing
    '            Context.Response.Flush()
    '            Context.Response.End()
    '            File.Delete(fileName)
    '        End If
    '    Catch ex As Exception
    '        Me.ShowErrorMessage(ex.Message, "...btnImportToExcel_Click")
    '    End Try
    'End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Don't remove this function
        ' This Function is mendetory when you are going to export your grid to excel.
        ' NOTE :: And Click event of button must be in postback trigger. (Page must be loaded)
    End Sub

#End Region

#Region "Assign values"
    Private Function AssignValues() As Boolean
        Dim dr As DataRow
        Dim dt_ActivityOpMtx As New DataTable
        Dim Act As Integer
        Dim UT As Integer
        Dim Dept As Integer
        Dim Loc As Integer
        Dim Op As Integer
        Dim ds_Check As New DataSet
        Dim dv_Check As New DataView
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ActivityExists As String = String.Empty
        Try

            dt_ActivityOpMtx = CType(Me.ViewState(VS_DtActivityOpMtx), DataTable)
            dt_ActivityOpMtx.Clear()

            For Act = 0 To Me.ChklstActivity.Items.Count - 1

                If Me.ChklstActivity.Items(Act).Selected = True Then
                    wStr = "vActivityId = '" + Me.ChklstActivity.Items(Act).Value.Trim() + "' And cStatusIndi <> '" + Status_Delete + "'"
                    If Not objHelp.GetActivityOperationMatrix(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition _
                                , ds_Check, eStr) Then
                        Me.ShowErrorMessage("Error While Getting ActivityOperationMatrix", eStr)
                        Return False
                    End If

                    For UT = 0 To Me.chklstUserType.Items.Count - 1
                        If Me.chklstUserType.Items(UT).Selected = True Then
                            For Dept = 0 To Me.chklstDept.Items.Count - 1
                                If Me.chklstDept.Items(Dept).Selected = True Then
                                    For Loc = 0 To Me.chklstLocation.Items.Count - 1
                                        If Me.chklstLocation.Items(Loc).Selected = True Then
                                            For Op = 0 To Me.chklstOperation.Items.Count - 1
                                                If Me.chklstOperation.Items(Op).Selected = True Then
                                                    If ds_Check.Tables(0).Rows.Count > 0 Then
                                                        dv_Check = ds_Check.Tables(0).DefaultView
                                                        If dv_Check.ToTable().Rows.Count > 0 Then
                                                            dv_Check.RowFilter = "vUserTypeCode = '" + Me.chklstUserType.Items(UT).Value.Trim() + "' And " + _
                                                                                "vDeptCode = '" + Me.chklstDept.Items(Dept).Value.Trim() + "' And " + _
                                                                                "vLocationCode = '" + Me.chklstLocation.Items(Loc).Value.Trim() + "' And " + _
                                                                                "vOperationCode = '" + Me.chklstOperation.Items(Op).Value.Trim() + "'"

                                                            If dv_Check.ToTable().Rows.Count > 0 Then

                                                                ActivityExists += "User = '" + Me.chklstUserType.Items(UT).Text.Trim() + _
                                                                                "', Department = '" + Me.chklstDept.Items(Dept).Text.Trim() + _
                                                                                "', Location = '" + Me.chklstLocation.Items(Loc).Text.Trim() + _
                                                                                " And Operation = '" + Me.chklstOperation.Items(Op).Text.Trim() + "'\n"
                                                                Continue For
                                                            End If
                                                        End If
                                                    End If

                                                    dr = dt_ActivityOpMtx.NewRow()
                                                    dr("vActivityRoleId") = Act.ToString.Trim() + UT.ToString.Trim() + Dept.ToString.Trim() + Loc.ToString.Trim() + Op.ToString.Trim()
                                                    dr("vActivityId") = Me.ChklstActivity.Items(Act).Value.Trim() 'Me.DDLActivity.SelectedValue.Trim()
                                                    dr("vUserTypeCode") = Me.chklstUserType.Items(UT).Value.Trim()
                                                    dr("vDeptCode") = Me.chklstDept.Items(Dept).Value.Trim()
                                                    dr("vLocationCode") = Me.chklstLocation.Items(Loc).Value.Trim()
                                                    dr("vOperationCode") = Me.chklstOperation.Items(Op).Value.Trim()
                                                    dr("cStatusIndi") = Status_New
                                                    dr("iModifyBy") = Me.Session(S_UserID)
                                                    dt_ActivityOpMtx.Rows.Add(dr)
                                                End If
                                            Next Op
                                        End If
                                    Next Loc
                                End If
                            Next Dept
                        End If
                    Next UT
                End If
            Next Act

            Me.ViewState(VS_DtActivityOpMtx) = dt_ActivityOpMtx
            If ActivityExists <> "" Then
                ObjCommon.ShowAlert("Record Already Exists For \n" + ActivityExists, Me.Page)
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...AssignValues")
            Return False
        End Try

    End Function
#End Region

#Region "Reset Page"

    Private Sub ResetPage()
        Try
            Me.ViewState(VS_DtActivityOpMtx) = Nothing
            Me.ViewState(VS_ActivityOpCode) = Nothing
            Me.ChklstActivity.Items.Clear()
            Me.chklstUserType.Items.Clear()
            Me.chklstDept.Items.Clear()
            Me.chklstLocation.Items.Clear()
            Me.chklstOperation.Items.Clear()

            If Not Me.GenCall() Then
                Exit Sub
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...ResetPage")
        End Try
    End Sub

#End Region

#Region "Grid Event"

    Protected Sub GV_ActOperation_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV_ActOperation.RowDataBound
        Try
            Me.SetPaging()
            If e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.Footer Or e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(GRDVCell_ActivityRoleId).Visible = False
                e.Row.Cells(GRDVCell_ActivityId).Visible = False
                e.Row.Cells(GRDVCell_DeptCode).Visible = False
                e.Row.Cells(GRDVCell_LocationCode).Visible = False
                e.Row.Cells(GRDVCell_OperationCode).Visible = False
                e.Row.Cells(GRDVCell_UserTypeCode).Visible = False

                If e.Row.RowType = DataControlRowType.DataRow Then
                    CType(e.Row.FindControl("ImgDelete"), ImageButton).CommandArgument = e.Row.RowIndex
                    CType(e.Row.FindControl("ImgDelete"), ImageButton).CommandName = "Delete"
                    e.Row.Cells(GV_ActOperation_Delete).HorizontalAlign = HorizontalAlign.Center
                    e.Row.Cells(GV_ActOperation_SrNo).Text = e.Row.RowIndex + (Me.GV_ActOperation.PageSize * Me.GV_ActOperation.PageIndex) + 1
                    e.Row.Cells(GV_ActOperation_SrNo).HorizontalAlign = HorizontalAlign.Center
                End If
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GV_ActOperation_RowDataBound")
        End Try
    End Sub

    Protected Sub GV_ActOperation_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Try
            GV_ActOperation.PageIndex = e.NewPageIndex

            If DdlActivityName.SelectedIndex <> 0 Then
                FillGridBaseOnDdl()
            End If
            If DdlUserType.SelectedIndex <> 0 Then
                FillGridBaseOnDdl()
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GV_ActOperation_PageIndexChanging")
        End Try
    End Sub

    Protected Sub GV_ActOperation_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GV_ActOperation.RowCommand
        Dim index As Integer
        Dim Ds_Save As New DataSet
        Dim dt_ActivityOpMtx As New DataTable
        Dim Dr_ActOp As DataRow
        Dim Dv_ActOp As New DataView
        Dim estr As String = String.Empty

        Try

            If e.CommandName.ToUpper = "DELETE" Then
                index = CType(e.CommandArgument, Integer)

                dt_ActivityOpMtx.Columns.Add("vActivityRoleId")
                dt_ActivityOpMtx.Columns.Add("vActivityId")
                dt_ActivityOpMtx.Columns.Add("vUserTypeCode")
                dt_ActivityOpMtx.Columns.Add("vDeptCode")
                dt_ActivityOpMtx.Columns.Add("vLocationCode")
                dt_ActivityOpMtx.Columns.Add("vOperationCode")
                dt_ActivityOpMtx.Columns.Add("cStatusIndi")
                dt_ActivityOpMtx.Columns.Add("iModifyBy")
                dt_ActivityOpMtx.AcceptChanges()
                Dr_ActOp = dt_ActivityOpMtx.NewRow()
                Dr_ActOp("vActivityRoleId") = Me.GV_ActOperation.Rows(index).Cells(GRDVCell_ActivityRoleId).Text
                Dr_ActOp("vActivityId") = Me.GV_ActOperation.Rows(index).Cells(GRDVCell_ActivityId).Text
                Dr_ActOp("vUserTypeCode") = Me.GV_ActOperation.Rows(index).Cells(GRDVCell_UserTypeCode).Text
                Dr_ActOp("vDeptCode") = Me.GV_ActOperation.Rows(index).Cells(GRDVCell_DeptCode).Text
                Dr_ActOp("vLocationCode") = Me.GV_ActOperation.Rows(index).Cells(GRDVCell_LocationCode).Text
                Dr_ActOp("vOperationCode") = Me.GV_ActOperation.Rows(index).Cells(GRDVCell_OperationCode).Text
                Dr_ActOp("cStatusIndi") = Status_Delete
                Dr_ActOp("iModifyBy") = Me.Session(S_UserID)
                dt_ActivityOpMtx.Rows.Add(Dr_ActOp)

                Ds_Save = New DataSet
                dt_ActivityOpMtx.TableName = "ActivityOperationMatrix"
                Ds_Save.Tables.Add(dt_ActivityOpMtx.Copy())

                If Ds_Save.Tables(0).Rows.Count > 0 Then
                    If Not objLambda.Save_InsertActivityOperationMatrix(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, Ds_Save, Me.Session(S_UserID), estr) Then
                        ObjCommon.ShowAlert("Error while deleting activityOperationMatrix", Me.Page)
                        Exit Sub
                    End If
                End If

                GV_ActOperation.Rows(index).Visible = False
                ObjCommon.ShowAlert("Record Deleted Successfully !", Me.Page)
                If Not FillGrid() Then
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GV_ActOperation_RowCommand")
        End Try
    End Sub

    Protected Sub GV_ActOperation_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GV_ActOperation.Sorting
        Try
            If ViewState("sortExpression") Is Nothing Then
                ViewState("sortExpression") = e.SortExpression
            End If

            If ViewState("sortDirection") Is Nothing Then
                ViewState("sortDirection") = "ASC"
            Else
                If ViewState("sortExpression").ToString <> e.SortExpression Then
                    ViewState("sortExpression") = e.SortExpression
                    ViewState("sortDirection") = "ASC"
                Else
                    If ViewState("sortDirection").ToString.Contains("ASC") Then
                        ViewState("sortDirection") = "DESC"
                    Else
                        ViewState("sortDirection") = "ASC"
                    End If
                End If
            End If

            If Not FillGrid() Then
                Exit Sub
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GV_ActOperation_Sorting")
        End Try
    End Sub

    Protected Sub GV_ActOperation_RowDeleting(sender As Object, e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GV_ActOperation.RowDeleting

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


#Region "Helper Functions/Subs"
    Private Sub SetPaging()
        Dim totalPages As Integer = 0
        Dim startIndex As Integer = 1
        Dim count As Integer = 1
        Try

            Me.phTopPager.Controls.Clear()
            Me.phBottomPager.Controls.Clear()

            If Not Me.ViewState(VS_TotalRowCount) Is Nothing AndAlso Integer.Parse(Me.ViewState(VS_TotalRowCount)) > 0 Then
                totalPages = Math.Ceiling(Me.ViewState(VS_TotalRowCount) / PAGESIZE)
            End If
            startIndex = Me.ViewState(VS_PagerStartPage)
            If Me.ViewState(VS_PagerStartPage) Is Nothing Then
                startIndex = 1
                Me.ViewState(VS_PagerStartPage) = 1
            End If

            If totalPages > 1 Then

                For index As Integer = startIndex To totalPages
                    Me.phTopPager.Visible = True
                    Me.phBottomPager.Visible = True
                    Dim lnkButton As New LinkButton()
                    If startIndex > 1 And count = 1 Then
                        'This is for first Page <<
                        lnkButton = Me.AddLnkButton("BtnFirstPage", "BtnFirstPage", 1.ToString, "<<")

                        'This is for ellipse ...
                        lnkButton = Me.AddLnkButton("BtnEllipsePrev", "BtnEllipsePrev", (index - 1).ToString(), "...")
                    End If

                    'This is for Numeric Buttons
                    If index = Me.ViewState(VS_CurrentPage) Then
                        lnkButton = Me.AddLnkButton(index.ToString, "BtnPager", index, index, False)
                    Else
                        lnkButton = Me.AddLnkButton(index.ToString, "BtnPager", index, index)
                    End If

                    If count = 10 Then
                        If index < totalPages Then
                            'This is for ellipses ...
                            Me.AddLnkButton("BtnEllipseNext", "BtnEllipseNext", (index + 1).ToString, "...")

                            'This is for Last Page >>
                            Me.AddLnkButton("BtnLastPage", "BtnLastPage", totalPages.ToString, ">>")
                        End If
                        Exit For
                    End If

                    count = count + 1
                Next
            Else
                Me.phTopPager.Visible = False
                Me.phBottomPager.Visible = False
            End If
            'End If
        Catch ex As Exception

        End Try
    End Sub

    Private Function AddLnkButton(ByVal ID_1 As String, ByVal CommandName_1 As String, _
                                  ByVal CommandArg_1 As String, ByVal Text_1 As String, _
                                  Optional ByVal IsEnablePostBack As Boolean = True) As LinkButton
        Dim lnkButton As New LinkButton
        Dim lnkButtonBottom As New LinkButton
        Dim ltr As Literal
        Dim ltrBottom As Literal
        lnkButton = New LinkButton()
        ltr = New Literal()
        lnkButtonBottom = New LinkButton()
        ltrBottom = New Literal()
        lnkButton.ID = "Top" + ID_1
        lnkButton.CommandName = CommandName_1
        lnkButton.CommandArgument = CommandArg_1
        lnkButton.Text = Text_1
        lnkButton.CssClass = "PagerLinks"
        AddHandler lnkButton.Click, AddressOf PagerButton_Click
        If Not IsEnablePostBack Then
            lnkButton.OnClientClick = "return false;"
            lnkButton.Font.Underline = False
        End If

        lnkButtonBottom.ID = "Bottom" + ID_1
        lnkButtonBottom.CommandName = CommandName_1
        lnkButtonBottom.CommandArgument = CommandArg_1
        lnkButtonBottom.Text = Text_1
        lnkButtonBottom.CssClass = "PagerLinks"
        AddHandler lnkButtonBottom.Click, AddressOf PagerButton_Click
        If Not IsEnablePostBack Then
            lnkButtonBottom.OnClientClick = "return false;"
            lnkButtonBottom.Font.Underline = False
        End If

        Me.phTopPager.Controls.Add(lnkButton)
        Me.phBottomPager.Controls.Add(lnkButtonBottom)
        ltr.Text = "&nbsp;"
        ltrBottom.Text = "&nbsp;"
        Me.phTopPager.Controls.Add(ltr)
        Me.phBottomPager.Controls.Add(ltrBottom)
        Return lnkButton
    End Function

    Protected Sub PagerButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lnkButton As LinkButton
        Dim totalPages As Integer = 1
        totalPages = Me.GetTotalPages()
        lnkButton = CType(sender, LinkButton)
        Me.ViewState(VS_CurrentPage) = lnkButton.CommandArgument

        If lnkButton.CommandName.ToUpper = "BtnEllipseNext".ToUpper.ToString Then
            Me.ViewState(VS_PagerStartPage) = lnkButton.CommandArgument
            If (Integer.Parse(totalPages) - Integer.Parse(lnkButton.CommandArgument)) < 9 Then
                Me.ViewState(VS_PagerStartPage) = (Integer.Parse(totalPages) - 9)
            End If
        ElseIf lnkButton.CommandName.ToUpper = "BtnEllipsePrev".ToUpper.ToString Or _
                lnkButton.CommandName.ToUpper = "BtnLastPage".ToUpper.ToString Then
            Me.ViewState(VS_PagerStartPage) = Integer.Parse(lnkButton.CommandArgument) - 9
            If (Integer.Parse(lnkButton.CommandArgument) - 10) < 1 Then
                Me.ViewState(VS_PagerStartPage) = 1
            End If
        ElseIf lnkButton.CommandName.ToUpper = "BtnFirstPage".ToUpper.ToString Then
            Me.ViewState(VS_PagerStartPage) = 1
        End If

        If Not FillGrid() Then
            Exit Sub
        End If
    End Sub

    Private Function GetTotalPages() As Integer
        GetTotalPages = 1
        If Not Me.ViewState(VS_TotalRowCount) Is Nothing Then
            GetTotalPages = Me.ViewState(VS_TotalRowCount)
        End If

        If GetTotalPages > PAGESIZE Then
            GetTotalPages = Math.Ceiling(Double.Parse(Me.ViewState(VS_TotalRowCount)) / PAGESIZE)
        End If
    End Function
#End Region

    '=============created by:vikas shah( implemented this for fill grid based on selected dropdown value)(29-05-2012)==

    Private Function FillGridBaseOnDdl() As Boolean
        Dim wstr As String = String.Empty
        Dim ActivityName As String = String.Empty
        Dim UserName As String = String.Empty
        Dim ds_RoleOp As New DataSet
        Dim estr As String = String.Empty

        Try

            If (DdlUserType.SelectedIndex <> 0) Then
                UserName = DdlUserType.SelectedItem.Text
                wstr += "cStatusIndi <> '" + Status_Delete + "' And vUserTypeName='" + UserName + "'"
                If Not objHelp.View_ActivityOperationMatrix(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_UserName, estr) Then
                    Me.ShowErrorMessage("Error while retriving data from View_ActivityOperationMatrix", estr)
                    Return False
                End If
                Me.GV_ActOperation.DataSource = ds_UserName
                Me.GV_ActOperation.DataBind()

            Else
                If (DdlActivityName.SelectedIndex <> 0) Then
                    ActivityName = DdlActivityName.SelectedItem.Text
                    wstr += "cStatusIndi <> '" + Status_Delete + "' And vActivityName='" + ActivityName + "'"
                    If Not objHelp.View_ActivityOperationMatrix(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Activity, estr) Then
                        Me.ShowErrorMessage("Error while retriving data from View_ActivityOperationMatrix", estr)
                        Return False
                    End If
                    Me.GV_ActOperation.DataSource = ds_Activity
                    Me.GV_ActOperation.DataBind()

                End If
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillGridBaseOnDdl")
            Return False
        End Try
    End Function

    <WebMethod> _
    Public Shared Function View_ActivityOperationMatrix() As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim ds_RoleOp As New Data.DataSet

        Try

            If Not ObjCommon.GetScopeValueWithCondition(Wstr) Then
                Return False
            End If

            Wstr += " AND cStatusIndi <> 'D'"
            If Not objHelp.View_ActivityOperationMatrix(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_RoleOp, estr) Then
                Return False
            End If
            strReturn = JsonConvert.SerializeObject(ds_RoleOp)

            Return strReturn
        Catch ex As Exception
            Return strReturn
        End Try


    End Function


    <WebMethod> _
    Public Shared Function Delete_ActivityOperationMatrix(ByVal id As String) As String
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim ds_RoleOp As New Data.DataSet

        Dim Dr_ActOp As DataRow
        Dim Dt_Actopt As New DataTable

        Try

            If Not ObjCommon.GetScopeValueWithCondition(Wstr) Then
                Return False
            End If

            Wstr += " AND cStatusIndi <> 'D' AND vActivityRoleId = '" + id + "' "
            If Not objHelp.View_ActivityOperationMatrix(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_RoleOp, estr) Then
                Return False
            End If
            Dt_Actopt.Columns.Add("vActivityRoleId")
            Dt_Actopt.Columns.Add("cStatusIndi")
            Dt_Actopt.Columns.Add("iModifyBy")
            Dt_Actopt.AcceptChanges()
            Dr_ActOp = Dt_Actopt.NewRow()
            Dr_ActOp("vActivityRoleId") = id
            Dr_ActOp("cStatusIndi") = Status_Delete
            Dr_ActOp("iModifyBy") = HttpContext.Current.Session(S_UserID)
            Dt_Actopt.Rows.Add(Dr_ActOp)
            Dt_Actopt.TableName = "ActivityOperationMatrix"
            ds_RoleOp.Tables.Add(Dt_Actopt.Copy())
            If ds_RoleOp.Tables(0).Rows.Count > 0 Then
                If Not objLambda.Save_InsertActivityOperationMatrix(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_RoleOp, HttpContext.Current.Session(S_UserID), estr) Then
                    Return False
                End If
            End If
            If Not ObjCommon.GetScopeValueWithCondition(Wstr) Then
                Return False
            End If
            Wstr += " AND cStatusIndi <> 'D'"
            If Not objHelp.View_ActivityOperationMatrix(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_RoleOp, estr) Then
                Return False
            End If
            strReturn = JsonConvert.SerializeObject(ds_RoleOp)

            Return strReturn
        Catch ex As Exception
            Return strReturn
        End Try


    End Function



End Class

