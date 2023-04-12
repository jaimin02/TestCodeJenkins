Imports Microsoft.Win32
Imports System.Web.Services
Imports Newtonsoft.Json
Imports Microsoft
Imports Microsoft.Office.Interop
Imports System.IO
Imports System.Drawing

Partial Class frmAddLocationMst
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtLocationMst As String = "DtLocationMst"
    Private Const VS_LocationCode As String = "vLocationCode"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_LocationName As Integer = 1
    Private Const GVC_LocationInitial As Integer = 2
    Private Const GVC_cLocationType As Integer = 3
    Private Const GVC_Remark As Integer = 4
    Private Const GVC_vTimeZoneName As Integer = 5
    Private Const GVC_Modifyon As Integer = 6
    Private Const GVC_Edit As Integer = 7
    Private Const GVC_Code As Integer = 8
    Dim TimeZoneCol As Collection
    Private Const ValueName As String = "Display"
    Private Const MasterKey As String = "SOFTWARE\Microsoft\Windows NT\CurrentVersion\Time Zones"
#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            GenCall()
            Exit Sub
        End If
        If gvlocation.Rows.Count > 0 Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvlocation", "UIgvlocation(); ", True)
        End If
    End Sub
#End Region

#Region "GenCall"
    Private Function GenCall() As Boolean
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim dt_LocationMst As DataTable = Nothing
        Dim ds_Locationmst As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try


            Choice = Me.Request.QueryString("mode").ToString
            Me.ViewState(VS_Choice) = Choice   'To use it while saving
            Page.ClientScript.RegisterHiddenField(VS_Choice, Me.ViewState(VS_Choice))

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_LocationCode) = Me.Request.QueryString("Value").ToString
                Page.ClientScript.RegisterHiddenField(VS_LocationCode, Me.ViewState(VS_LocationCode))
            End If

            ''''Check for Valid User''''''''''''''
            If Not GenCall_Data(Choice, dt_LocationMst) Then ' For Data Retrieval
                Exit Function
            End If

            ' adding blank DataTable in viewstate
            Me.ViewState(VS_DtLocationMst) = dt_LocationMst

            'For Displaying Data
            If Not GenCall_ShowUI(Choice, dt_LocationMst) Then
                Exit Function
            End If
            Me.txtlocationname.Attributes.Add("onblur", "return CheckLocationInitial();")
            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideLocationDetails", "HideLocationDetails(); ", True)
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
        Dim ds_Locationmst As DataSet = Nothing
        'Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable
        Try
            
            If Choice_1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vLocationCode= '" + Me.ViewState(VS_LocationCode).ToString() + "'"
            End If

            wStr += " And cStatusIndi <> 'D'"

            If Not objHelp.getLocationMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Locationmst, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_Locationmst Is Nothing Then
                Throw New Exception(eStr)
            End If

            If ds_Locationmst.Tables(0).Rows.Count <= 0 And _
               Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Throw New Exception("No Records Found for Selected role")
            End If

            dt_Dist_Retu = ds_Locationmst.Tables(0)
            GenCall_Data = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_Data")
        Finally
        End Try
    End Function
#End Region

#Region "GenCall_ShowUI"
    Private Function GenCall_ShowUI(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                                      ByVal dt_LocationMst As DataTable) As Boolean


        Try
            CType(Master.FindControl("lblHeading"), Label).Text = "Location Master"

            Page.Title = ":: Location Master :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            If Not FillGridLocation() Then
                Exit Function
            End If

            If Not FillDropdown() Then
                objcommon.ShowAlert("Error While Getting TimeZone From Registry", Me.Page)
                Return False
            End If



            If Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                Me.txtlocationname.Text = ConvertDbNullToDbTypeDefaultValue(dt_LocationMst.Rows(0)("vLocationName"), dt_LocationMst.Rows(0)("vLocationName").GetType)
                Me.txtInitial.Text = ConvertDbNullToDbTypeDefaultValue(dt_LocationMst.Rows(0)("vLocationInitiate"), dt_LocationMst.Rows(0)("vLocationInitiate").GetType)
                Me.txtcountrycode.Text = ConvertDbNullToDbTypeDefaultValue(dt_LocationMst.Rows(0)("vCountryCode"), dt_LocationMst.Rows(0)("vCountryCode").GetType)
                If ConvertDbNullToDbTypeDefaultValue(dt_LocationMst.Rows(0)("cLocationType"), dt_LocationMst.Rows(0)("cLocationType").GetType) = "L" Then
                    Me.RbIsLocation.Checked = True
                Else
                    Me.RbIsNotLocation.Checked = True
                End If
                Me.ddlTimezone.SelectedValue = ConvertDbNullToDbTypeDefaultValue(dt_LocationMst.Rows(0)("vTimeZoneName"), dt_LocationMst.Rows(0)("vTimeZoneName").GetType)
                btnSave.Text = "Update"
                btnSave.ToolTip = "Update"
            End If

            GenCall_ShowUI = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_ShowUI")
            Return False
        End Try
    End Function
#End Region

#Region "Fill GridLocation"
    Private Function FillGridLocation() As Boolean
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_LocationMst As New DataSet
        Dim dv_view As New Data.DataView
        Try


            If Not objHelp.getLocationMst("cStatusIndi <> 'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_LocationMst, eStr) Then
                Throw New Exception(eStr)
            End If
            dv_view = ds_LocationMst.Tables(0).Copy.DefaultView
            dv_view.Sort = "vLocationName "
            Me.gvlocation.DataSource = dv_view
            Me.gvlocation.DataBind()
            If gvlocation.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvlocation", "UIgvlocation(); ", True)
            End If
            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...FillGridLocation")
            Return False
        End Try
    End Function
#End Region

#Region "Fill FillDropdown"
    Private Function FillDropdown() As Boolean
        Dim Timezonesubkey As Array
        Dim regKey As Microsoft.Win32.RegistryKey
        Dim timezonevalue As String
        Dim timezoneview As String
        Dim i As Integer
        Dim TimeZoneArray As ArrayList

        Try
            regKey = My.Computer.Registry.LocalMachine.OpenSubKey("Software\Microsoft\Windows NT\CurrentVersion\Time Zones", False)

            Timezonesubkey = regKey.GetSubKeyNames
            TimeZoneArray = New ArrayList
            For i = 0 To Timezonesubkey.Length - 1
                timezoneview = regKey.OpenSubKey(Timezonesubkey(i), False).GetValue("Display").ToString

                TimeZoneArray.Add(timezoneview)
                timezonevalue = regKey.OpenSubKey(Timezonesubkey(i), False).GetValue("Std").ToString

                ddlTimezone.Items.Add(New ListItem(timezoneview, timezonevalue))
            Next
            TimeZoneArray.Sort()
            sortListItems(ddlTimezone)


            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...FillDropdown")
            Return False
        End Try
    End Function


    Private Sub sortListItems(ByVal ddlTimezone As Object)
        Dim li As ListItem
        Dim sl As SortedList = New SortedList
        Try
            For Each li In ddlTimezone.Items
                sl.Add(li.Text, li.Value)
            Next

            ' Move sorted items back to List again
            ddlTimezone.DataSource = sl
            ddlTimezone.DataValueField = "Value"
            ddlTimezone.DataTextField = "Key"
            ddlTimezone.DataBind()
            ddlTimezone.Items.Insert(0, "Select TimeZone")

        Catch ex As Exception
            Me.ShowErrorMessage("Error while Sorting dropdownlist", ".....sortListItems")
        End Try
            End Sub
#End Region

#Region "Reset Page"
    Private Sub ResetPage()
        Me.txtlocationname.Text = ""
        Me.txtInitial.Text = ""
        Me.txtcountrycode.Text = ""
        Me.txtRemark.Text = ""
        Me.ViewState(VS_DtLocationMst) = Nothing
        Me.ViewState(VS_Choice) = Nothing
        Me.ViewState(VS_LocationCode) = Nothing
    End Sub
#End Region

#Region "Save"
    Private Function AssignUpdatedValues() As Boolean
        Dim dtOld As New DataTable
        Dim dr As DataRow
        Dim ds_Check As New DataSet
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Try
            dtOld = Me.ViewState(VS_DtLocationMst)

            wStr = "cStatusIndi <> 'D' And vLocationName ='" & Me.txtlocationname.Text.Trim() & "'"
            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                wStr += " And vLocationCode <> '" + dtOld.Rows(0).Item("vLocationCode") + "'"
            End If

            If Not objHelp.getLocationMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Check, eStr) Then
                Me.ShowErrorMessage("Error While Getting Data From LocationMst", eStr)
                Return False
            End If

            If ds_Check.Tables(0).Rows.Count > 0 Then
                objcommon.ShowAlert("Location Name Already Exists !", Me.Page)
                Return False
            End If

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                dtOld.Clear()
                dr = dtOld.NewRow
                dr("vLocationName") = Me.txtlocationname.Text.Trim
                dr("vLocationInitiate") = Me.txtInitial.Text.Trim
                dr("vCountryCode") = Me.txtcountrycode.Text.Trim
                dr("vLocationCode") = "0000"
                dr("cLocationType") = "S"
                dr("vTimeZoneName") = Me.ddlTimezone.SelectedValue
                If Me.RbIsLocation.Checked = True Then
                    dr("cLocationType") = "L"
                End If
                dr("vRemark") = Me.txtRemark.Text.Trim
                dr("iCreatedBy") = Session(S_UserID)
                dr("iModifyBy") = Session(S_UserID)
                dtOld.Rows.Add(dr)

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                For Each dr In dtOld.Rows
                    dr("vLocationName") = Me.txtlocationname.Text.Trim
                    dr("vLocationInitiate") = Me.txtInitial.Text.Trim
                    dr("vCountryCode") = Me.txtcountrycode.Text.Trim.ToUpper.ToString
                    dr("iModifyBy") = Session(S_UserID)
                    dr("cStatusIndi") = "E"
                    dr("cLocationType") = "S"
                    dr("vTimeZoneName") = Me.ddlTimezone.SelectedValue
                    If Me.RbIsLocation.Checked = True Then
                        dr("cLocationType") = "L"
                    End If
                    dr("vRemark") = Me.txtRemark.Text.Trim
                    dr.AcceptChanges()
                Next dr
                dtOld.AcceptChanges()
            End If

            Me.ViewState(VS_DtLocationMst) = dtOld
            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...AssignUpdates")
            Return False
        End Try
    End Function

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim Dt As New DataTable
        Dim Ds_stagemat As DataSet
        Dim ds_LocGrid As New DataSet
        Dim eStr As String = String.Empty
        Dim objOPws As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()
        Dim message As String = String.Empty
        Try

            If Not AssignUpdatedValues() Then
                Exit Sub
            End If

            Ds_stagemat = New DataSet
            Ds_stagemat.Tables.Add(CType(Me.ViewState(VS_DtLocationMst), Data.DataTable).Copy())
            Ds_stagemat.Tables(0).TableName = "locationMst"   ' New Values on the form to be updated

            If Not objOPws.Save_LocationMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_LocationMst, Ds_stagemat, "1", eStr) Then
                objcommon.ShowAlert("Error While Saving LocationMst !", Me.Page)
                Exit Sub
            End If
            message = IIf(Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "Location Saved Successfully !", "Location  Updated Successfully !")
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Alert", "ShowAlert('" + message + "')", True)
            ResetPage()

        Catch ThreaEx As Threading.ThreadAbortException
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....btnSave_Click")
        End Try
    End Sub
#End Region

#Region "Cancel"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ResetPage()
        Response.Redirect("frmLocationMst.aspx?mode=1")
    End Sub
#End Region

#Region "Close"
    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnclose.Click, btnclose.Click
        Response.Redirect("frmMainPage.aspx")
    End Sub
#End Region

#Region "Export To Excel"

    Protected Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        'Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable
        Dim wStr As String = String.Empty
        Dim ds_LocationMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty

        Dim dtLocationMstHistory As New DataTable
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

            vTableName = "LocationMstHistory"
            vIdName = ""
            AuditFieldName = "vLocationCode"
            AuditFieldValue = hdnLocationCode.Value
            vOtherTableName = ""
            vOtherIdName = ""


            Dim Param As String = ""

            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName

            If Not objHelp.Proc_GetAuditTrail(wStr, ds_LocationMst, Param) Then
                Throw New Exception(estr)
            End If

            If Not dtLocationMstHistory Is Nothing Then
                dtLocationMstHistory.Columns.Add("Sr. No")
                dtLocationMstHistory.Columns.Add("Location Name")
                dtLocationMstHistory.Columns.Add("Initial")
                dtLocationMstHistory.Columns.Add("Type")
                dtLocationMstHistory.Columns.Add("Country Code")
                dtLocationMstHistory.Columns.Add("Time Zone Name")
                dtLocationMstHistory.Columns.Add("Remarks")
                dtLocationMstHistory.Columns.Add("Modify By")
                dtLocationMstHistory.Columns.Add("Modify On")
            End If

            dtLocationMstHistory.AcceptChanges()
            dt = ds_LocationMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtLocationMstHistory.NewRow()
                drAuditTrail("Sr. No") = i
                drAuditTrail("Location Name") = dr("vLocationname").ToString()
                drAuditTrail("Initial") = dr("vLocationInitiate").ToString()
                'drAuditTrail("Type") = dr("cLocationType").ToString()
                If dr("cLocationType").ToString() = "L" Then
                    drAuditTrail("Type") = "Location"
                ElseIf dr("cLocationType").ToString() = "S" Then
                    drAuditTrail("Type") = "Site"
                End If
                drAuditTrail("Country Code") = dr("vCountryCode").ToString()
                drAuditTrail("Time Zone Name") = dr("vTimeZoneName").ToString()
                drAuditTrail("Remarks") = dr("vRemark").ToString()
                drAuditTrail("Modify By") = dr("vModifyBy").ToString()
                drAuditTrail("Modify On") = Convert.ToString(dr("ModifyOn"))
                dtLocationMstHistory.Rows.Add(drAuditTrail)
                dtLocationMstHistory.AcceptChanges()
                i += 1
            Next

            gvExport.DataSource = dtLocationMstHistory
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

                filename = "Audit Trail_" + hdnLocationCode.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                Dim stringWriter As New System.IO.StringWriter()

                Dim writer As New HtmlTextWriter(stringWriter)
                gvExport.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()

                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""9""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")





                strMessage.Append("<td colspan=""9""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Location Master-AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td></td><td></td><td></td><td Align=""Right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")

                gvExport.Visible = True
                gridviewHtml = strMessage.ToString() + gridviewHtml

                gridviewHtml = gridviewHtml.Replace("display: none", "display: inline")

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

                filename = "Audit Trail_" + hdnLocationCode.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"


                drAuditTrail = dtLocationMstHistory.NewRow()

                drAuditTrail("Sr. No") = ""
                drAuditTrail("Location Name") = ""
                drAuditTrail("Initial") = ""
                drAuditTrail("Type") = ""
                drAuditTrail("Country Code") = ""
                drAuditTrail("Time Zone Name") = ""
                drAuditTrail("Remark") = ""
                drAuditTrail("Modify By") = ""
                drAuditTrail("Modify On") = ""
                dtLocationMstHistory.Rows.Add(drAuditTrail)
                dtLocationMstHistory.AcceptChanges()
                gvExport.DataSource = dtLocationMstHistory
                gvExport.DataBind()


                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(stringWriter)
                gvExport.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()


                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""9""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<td colspan=""9""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Location Master-AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td></td><td></td><td></td><td align=""right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")


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

    Protected Sub btnExportToExcelGrid_Click(sender As Object, e As EventArgs) Handles btnExportToExcelGrid.Click
        'Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable
        Dim wStr As String = String.Empty
        Dim ds_LocationMst As New DataSet
        Dim estr As String = String.Empty
        Dim dtLocationMstHistory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable
        Dim LocationCode As String = ""
        Dim filename As String = String.Empty
        Dim strMessage As New StringBuilder

        Try
            wStr = "" + "##"
            ds_LocationMst = objHelp.ProcedureExecute("Proc_LocationMaster ", wStr)
            wStr = String.Empty
            If Not dtLocationMstHistory Is Nothing Then
                dtLocationMstHistory.Columns.Add("Sr. No")
                dtLocationMstHistory.Columns.Add("Location Name")
                dtLocationMstHistory.Columns.Add("Initial")
                dtLocationMstHistory.Columns.Add("Type")
                dtLocationMstHistory.Columns.Add("Country Code")
                dtLocationMstHistory.Columns.Add("Time Zone Name")
                dtLocationMstHistory.Columns.Add("Remarks")
                dtLocationMstHistory.Columns.Add("Modify By")
                dtLocationMstHistory.Columns.Add("Modify On")
            End If

            dtLocationMstHistory.AcceptChanges()
            dt = ds_LocationMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtLocationMstHistory.NewRow()
                drAuditTrail("Sr. No") = i
                drAuditTrail("Location Name") = dr("vLocationname").ToString()
                drAuditTrail("Initial") = dr("vLocationInitiate").ToString()
                drAuditTrail("Type") = dr("cLocationType").ToString()
                drAuditTrail("Country Code") = dr("vCountryCode").ToString()
                drAuditTrail("Time Zone Name") = dr("vTimeZoneName").ToString()
                drAuditTrail("Remarks") = dr("vRemark").ToString()
                drAuditTrail("Modify By") = dr("iModifyBy").ToString()
                drAuditTrail("Modify On") = Convert.ToString(dr("dModifyOn"))
                dtLocationMstHistory.Rows.Add(drAuditTrail)
                dtLocationMstHistory.AcceptChanges()
                i += 1
            Next

            gvExport.DataSource = dtLocationMstHistory
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

                filename = "Location Master" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                Dim stringWriter As New System.IO.StringWriter()

                Dim writer As New HtmlTextWriter(stringWriter)
                gvExportToExcel.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()

                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""9""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")





                strMessage.Append("<td colspan=""9""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Location Master")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td></td><td></td><td></td><td Align=""Right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")


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

                filename = "Location Master" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"


                drAuditTrail = dtLocationMstHistory.NewRow()

                drAuditTrail("Sr. No") = ""
                drAuditTrail("Location Name") = ""
                drAuditTrail("Initial") = ""
                drAuditTrail("Type") = ""
                drAuditTrail("Country Code") = ""
                drAuditTrail("Time Zone Name") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("Modify By") = ""
                drAuditTrail("Modify On") = ""
                dtLocationMstHistory.Rows.Add(drAuditTrail)
                dtLocationMstHistory.AcceptChanges()
                gvExportToExcel.DataSource = dtLocationMstHistory
                gvExportToExcel.DataBind()


                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(stringWriter)
                gvExportToExcel.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()


                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""9""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<td colspan=""9""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Location Master")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td></td><td></td><td></td><td align=""right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")


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

#End Region

#Region "BindGrid"
    Private Sub BindGrid()
        Dim dsLocation As New DataSet
        Dim eStr As String = String.Empty


        If objHelp.getLocationMst("cStatusIndi <> 'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsLocation, eStr) Then
            gvlocation.ShowFooter = False
            gvlocation.DataSource = dsLocation
            gvlocation.DataBind()
            dsLocation.Dispose()
        End If
    End Sub
#End Region

#Region "Grid Event"



    Protected Sub gvlocation_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)

        gvlocation.PageIndex = e.NewPageIndex
        FillGridLocation()
    End Sub

    Protected Sub gvlocation_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim Index As Integer = e.CommandArgument
        If e.CommandName.ToUpper = "EDIT" Then
            Me.Response.Redirect("frmLocationMst.aspx?mode=2&value=" & Me.gvlocation.Rows(Index).Cells(GVC_Code).Text.Trim())
        End If
    End Sub

    Protected Sub gvlocation_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        If Not e.Row.RowType = DataControlRowType.Pager Then
            e.Row.Cells(GVC_Code).Visible = False
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (gvlocation.PageSize * gvlocation.PageIndex) + 1
            CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandName = "Edit"
            If e.Row.Cells(GVC_cLocationType).Text = "L" Then
                e.Row.Cells(GVC_cLocationType).Text = "Location"
            Else
                e.Row.Cells(GVC_cLocationType).Text = "Site"
            End If
            CType(e.Row.FindControl("lnkAudit"), ImageButton).Attributes.Add("vLocationCode", e.Row.Cells(GVC_Code).Text)
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

#Region "CheckDuplicateLocationInitial"
    <Services.WebMethod()> _
    Public Shared Function CheckDuplicateLocationInitial(ByVal Choice As String, _
                                                         ByVal vInitial As String, _
                                                         ByVal vLocation As String, _
                                                         ByVal vLocationCode As String) As String
        Dim Ds_Location As DataSet = Nothing
        Dim Dv_Location As DataView = Nothing
        Dim Dv_Initial As DataView = Nothing
        Dim ErrorMsg As String = String.Empty
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()

        wStr = "cStatusIndi <> 'D' "

        If vInitial <> "0" And vLocation <> "0" Then
            wStr += " And (vLocationName = '" & vLocation.ToString() & "' Or vLocationInitiate = '" & vInitial.ToString() & "') "
        ElseIf vInitial <> "0" And vLocation = "0" Then
            wStr += " And vLocationInitiate = '" & vInitial.ToString() & "' "
        ElseIf vInitial = "0" And vLocation <> "0" Then
            wStr += " And vLocationName = '" & vLocation.ToString() & "'"
        End If

        If Choice <> "1" Then
            wStr = " And vLocationCode <> '" & vLocationCode.ToString() & "' "
        End If

        If Not objHelp.getLocationMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_Location, eStr) Then
            Return eStr.ToString()
        End If

        Dv_Location = Ds_Location.Tables(0).DefaultView
        Dv_Initial = Ds_Location.Tables(0).DefaultView

        Dv_Location.RowFilter = "vLocationName = '" & vLocation.ToString() & "'"

        If Dv_Location.ToTable().Rows.Count > 0 Then
            ErrorMsg = "Location"
        End If

        Dv_Initial.RowFilter = " vLocationInitiate = '" & vInitial.ToString() & "'"
        If Dv_Initial.ToTable().Rows.Count > 0 Then
            ErrorMsg = "Initial"
        End If

        Return ErrorMsg
    End Function
#End Region

#Region "Web Method"
    <WebMethod> _
    Public Shared Function AuditTrail(ByVal vLocationCode As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_LocationMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim dtLocationMstHistory As New DataTable
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

            vTableName = "LocationMstHistory"
            vIdName = ""
            AuditFieldName = "vLocationCode"
            AuditFieldValue = vLocationCode
            vOtherTableName = ""
            vOtherIdName = ""


            Dim Param As String = ""


            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName

            If Not objHelp.Proc_GetAuditTrail(wStr, ds_LocationMst, Param) Then
                Throw New Exception(estr)
            End If

            If Not dtLocationMstHistory Is Nothing Then
                dtLocationMstHistory.Columns.Add("SrNo")
                dtLocationMstHistory.Columns.Add("LocationName")
                dtLocationMstHistory.Columns.Add("LocationInitiate")
                dtLocationMstHistory.Columns.Add("LocationType")
                dtLocationMstHistory.Columns.Add("vCountryCode")
                dtLocationMstHistory.Columns.Add("TimeZoneName")
                dtLocationMstHistory.Columns.Add("Remark")
                dtLocationMstHistory.Columns.Add("ModifyBy")
                dtLocationMstHistory.Columns.Add("ModifyOn")
            End If

            dt = ds_LocationMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtLocationMstHistory.NewRow()
                drAuditTrail("SrNo") = i
                drAuditTrail("LocationName") = dr("vLocationname").ToString()
                drAuditTrail("LocationInitiate") = dr("vLocationInitiate").ToString()
                'drAuditTrail("LocationType") = dr("cLocationType").ToString()
                If dr("cLocationType").ToString() = "L" Then
                    drAuditTrail("LocationType") = "Location"
                ElseIf dr("cLocationType").ToString() = "S" Then
                    drAuditTrail("LocationType") = "Site"
                End If
                drAuditTrail("vCountryCode") = dr("vCountryCode").ToString()
                drAuditTrail("TimeZoneName") = dr("vTimeZoneName").ToString()
                drAuditTrail("Remark") = dr("vRemark").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("ModifyOn"))
                dtLocationMstHistory.Rows.Add(drAuditTrail)
                dtLocationMstHistory.AcceptChanges()
                i += 1
            Next
            strReturn = JsonConvert.SerializeObject(dtLocationMstHistory)
            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function
#End Region

    Protected Sub ddlTimezone_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTimezone.Load

    End Sub

End Class
