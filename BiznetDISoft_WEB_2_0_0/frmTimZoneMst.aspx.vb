Imports Microsoft.Win32
Imports System.Web.Services
Imports Newtonsoft.Json
Imports System.Drawing

Partial Class frmTimZoneMst
    Inherits System.Web.UI.Page
#Region "Variable Declaration"

    Private objCommon As New clsCommon
    Private objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DTimeZoneMst As String = "DtLocationMst"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_vTimeZoneName As Integer = 1
    Private Const GVC_vTimeZoneOffset As Integer = 2
    Private Const GVC_cLocationType As Integer = 3
    Private Const GVC_startDate As Integer = 4
    Private Const GVC_endDate As Integer = 5
    Private Const GVC_startDateEST As Integer = 6
    Private Const GVC_endDateEST As Integer = 7
    Private Const GVC_nTimeZoneMstNo As Integer = 8
    Private Const GVC_status As Integer = 9

    Dim TimeZoneCol As Collection

    Private Const ValueName As String = "Display"
    Private Const MasterKey As String = "SOFTWARE\Microsoft\Windows NT\CurrentVersion\Time Zones"

    Private Shared Gv_index As Integer = 0

    Private Shared index As Integer = 0
    Private Shared nTimeZoneMstNo As String = String.Empty
    Private Shared Status As String = String.Empty

#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'calanderToDate.startDate = Now.Today()
        If Not IsPostBack Then
            GenCall()
            Exit Sub

        End If
    End Sub

#End Region

#Region "GenCall"
    Private Function GenCall() As Boolean

        Try

            Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
            If Not GenCall_ShowUI() Then
                Exit Function
            End If
            If Not IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideTimeZoneDetails", "HideTimeZoneDetails(); ", True)
            End If
            GenCall = True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...GenCall")
        Finally
        End Try
    End Function
#End Region

#Region "GenCall_ShowUI"

    Private Function GenCall_ShowUI() As Boolean


        Try
            CType(Master.FindControl("lblHeading"), Label).Text = "Time Zone Master"

            Page.Title = ":: Time Zone Master :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            If Not FillTimezone() Then
                objCommon.ShowAlert("Error While Getting TimeZone From Registry", Me.Page)
                Return False
            End If

            If Not FillCountry() Then
                objCommon.ShowAlert("Error While Getting Country List", Me.Page)
                Return False
            End If
            If Not FillGrid() Then
                objCommon.ShowAlert("Error While Binding Grid", Me.Page)
                Return False
            End If

            'If Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

            '    Me.txtlocationname.Text = ConvertDbNullToDbTypeDefaultValue(dt_LocationMst.Rows(0)("vLocationName"), dt_LocationMst.Rows(0)("vLocationName").GetType)
            '    Me.txtInitial.Text = ConvertDbNullToDbTypeDefaultValue(dt_LocationMst.Rows(0)("vLocationInitiate"), dt_LocationMst.Rows(0)("vLocationInitiate").GetType)
            '    Me.txtcountrycode.Text = ConvertDbNullToDbTypeDefaultValue(dt_LocationMst.Rows(0)("vRemark"), dt_LocationMst.Rows(0)("vRemark").GetType)
            '    If ConvertDbNullToDbTypeDefaultValue(dt_LocationMst.Rows(0)("cLocationType"), dt_LocationMst.Rows(0)("cLocationType").GetType) = "L" Then
            '        Me.RbIsLocation.Checked = True
            '    Else
            '        Me.RbIsNotLocation.Checked = True
            '    End If
            '    Me.ddlTimezone.SelectedValue = ConvertDbNullToDbTypeDefaultValue(dt_LocationMst.Rows(0)("vTimeZoneName"), dt_LocationMst.Rows(0)("vTimeZoneName").GetType)
            '    btnSave.Text = "Update"
            '    btnSave.ToolTip = "Update"
            'End If

            GenCall_ShowUI = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_ShowUI")
            Return False
        End Try
    End Function

#End Region

#Region "Fill FillDropdown"

    Private Function FillTimezone() As Boolean
        Dim Timezonesubkey As Array
        Dim regKey As Microsoft.Win32.RegistryKey
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
                'timezonevalue = regKey.OpenSubKey(Timezonesubkey(i), False).GetValue("Std").ToString

                'ddlTimezone.Items.Add(New ListItem(timezonevalue, timezonevalue))
                ddlTimezone.Items.Add(New ListItem(timezoneview, timezoneview))
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
            ddlTimezone.DataTextField = "Value"
            ddlTimezone.DataBind()
            ddlTimezone.Items.Insert(0, "Select TimeZone")

        Catch ex As Exception
            Me.ShowErrorMessage("Error while Sorting dropdownlist", ".....sortListItems")
        End Try
    End Sub

    Private Function FillCountry() As Boolean
        Dim Wstr As String = String.Empty
        Dim ds_Country As New DataSet
        Dim dt_SortCountry As New DataTable
        Dim eStr As String = String.Empty
        Try

            Wstr = "cStatusIndi <> 'D'"
            If Not objhelpDb.getLocationMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Country, eStr) Then
                Me.ShowErrorMessage("", eStr)
                Exit Function
            End If
            If Not ds_Country Is Nothing Then
                If ds_Country.Tables(0).Rows.Count > 0 Then
                    ds_Country.Tables(0).DefaultView.ToTable(True, "vLocationName")
                    dt_SortCountry = ds_Country.Tables(0).DefaultView.ToTable()
                    dt_SortCountry.DefaultView.Sort = "vLocationName"
                    Me.ddlLocation.DataSource = dt_SortCountry.DefaultView.ToTable()
                    Me.ddlLocation.DataValueField = "vLocationName"
                    Me.ddlLocation.DataTextField = "vLocationName"
                    Me.ddlLocation.DataBind()
                    Me.ddlLocation.Items.Insert(0, "Select Location")
                End If
            End If

            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...FillDropdown")
            Return False
        End Try
    End Function

    Private Function FillGrid() As Boolean
        Dim Wstr As String = String.Empty
        Dim ds_Timezone As New DataSet
        'Dim dt_SortCountry As New DataTable
        Dim eStr As String = String.Empty
        Try
            Me.gvTimeZone.DataSource = Nothing
            Me.gvTimeZone.DataBind()

            If Not objhelpDb.GetTimeZoneMaster(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_Timezone, eStr) Then
                Me.ShowErrorMessage("", eStr)
                Exit Function
            End If
            If Not ds_Timezone Is Nothing Then
                If ds_Timezone.Tables(0).Rows.Count > 0 Then
                    Me.gvTimeZone.DataSource = ds_Timezone.Tables(0)
                    Me.gvTimeZone.DataBind()
                    If gvTimeZone.Rows.Count > 0 Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvTimeZone", "UIgvTimeZone(); ", True)
                    End If

                End If
            End If

            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...FillDropdown")
            Return False
        End Try
    End Function


#End Region

#Region "Reset Page"

    Private Sub ResetPage()
        Me.ddlTimezone.SelectedIndex = 0
        Me.ddlLocation.SelectedIndex = 0
        'Me.txtoffset.Text = ""
        Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
        Me.btnSave.Text = "Save"
        Me.btnSave.ToolTip = "Save"
        Me.txtStartDate.Text = ""
        Me.txtStarttime.Text = ""
        Me.txtEndDate.Text = ""
        Me.txtEndTime.Text = ""
    End Sub

#End Region

#Region "Grid Events"
    'Protected Sub gvTimeZone_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTimeZone.PageIndexChanging
    '    gvTimeZone.PageIndex = e.NewPageIndex
    '    If Not FillGrid() Then
    '        objCommon.ShowAlert("Error while binding", Me.Page)
    '        Exit Sub
    '    End If
    'End Sub

    Protected Sub gvTimeZone_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTimeZone.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or _
                e.Row.RowType = DataControlRowType.Header Or _
                e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(GVC_nTimeZoneMstNo).Style.Add("display", "none")
            e.Row.Cells(GVC_status).Visible = False


        End If
    End Sub

    Protected Sub gvTimeZone_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTimeZone.RowCommand
        Try
            Gv_index = e.CommandArgument
            Dim wStr As String = ""
            Dim ds_Timezone As DataSet = New Data.DataSet
            Dim eStr As String = String.Empty
            Dim Dr As DataRow

            index = Me.gvTimeZone.Rows(Gv_index).Cells(GVC_nTimeZoneMstNo).Text.Trim()
            nTimeZoneMstNo = index
            'wStr = "nTimeZoneMstNo = " & index

            'If Not objhelpDb.GetTimeZoneMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Timezone, eStr) Then
            '    Me.ShowErrorMessage("", eStr)
            '    Exit Sub
            'End If


            If e.CommandName.ToUpper = "DELETE" Then
                Status = "Delete"

                'Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete
                'For Each Dr In ds_Timezone.Tables(0).Rows
                '    Dr("iModifyBy") = Session(S_UserID)
                '    Dr("cStatusIndi") = "D"
                '    Dr.AcceptChanges()
                'Next Dr

                'ds_Timezone.Tables(0).AcceptChanges()

                'If Not objLambda.Save_TimeZoneMst(Me.ViewState(VS_Choice), ds_Timezone, Me.Session(S_UserID), eStr) Then
                '    objCommon.ShowAlert("Error While Saving TimeZonemst", Me.Page)
                '    Exit Sub
                'End If

                If Not FillGrid() Then
                    Me.ShowErrorMessage("Error While Binding", "gvTimeZone_RowCommand")
                    Exit Sub
                End If
                'ResetPage()
            End If

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Getting TimeZone details...", "gvTimeZone_RowCommand")

        End Try
    End Sub

    Protected Sub gvTimeZone_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTimeZone.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then


            e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (gvTimeZone.PageSize * gvTimeZone.PageIndex) + 1

            CType(e.Row.FindControl("lnkDelete"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkDelete"), ImageButton).CommandName = "DELETE"
            CType(e.Row.FindControl("lnkAudit"), ImageButton).Attributes.Add("nTimeZoneMstNo", e.Row.Cells(GVC_nTimeZoneMstNo).Text.Trim)
            If e.Row.Cells(GVC_status).Text = "D" Then
                e.Row.BackColor = Drawing.Color.FromArgb(255, 189, 121)
                CType(e.Row.FindControl("lnkDelete"), ImageButton).Enabled = False
            End If
        End If
    End Sub

    Protected Sub gvTimeZone_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvTimeZone.RowEditing
        e.Cancel = True
    End Sub

    Protected Sub gvTimeZone_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvTimeZone.RowDeleting

    End Sub

#End Region

#Region "Assignvalues"

    Private Function AssignUpdatedValues(ByRef ds_Save As DataSet) As Boolean
        Dim dtOld As New DataTable
        Dim dr As DataRow
        Dim ds_Timezone, ds_PreviousTimeZone, ds_Country As New DataSet
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim startDate As New DateTime
        Dim EndDate As New DateTime
        Dim startDateEST As New DateTime
        Dim EndDateEST As New DateTime
        Dim strOffset As String
        Dim TimeZoneName As String
        Dim vTimeZone As String

        Try


            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                ds_Timezone = Me.ViewState(VS_DTimeZoneMst)

            Else
                wStr = "1=2"

                If Not objhelpDb.GetTimeZoneMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Timezone, eStr) Then
                    Me.ShowErrorMessage("", eStr)
                    Exit Function
                End If

            End If
            'startDate = Me.txtStartDate.Text.Trim() + " " + Me.txtStarttime.Text.Trim()        'Commented By Rahul Shah on 19-March-2015
            'EndDate = Me.txtEndDate.Text.Trim() + " " + Me.txtEndTime.Text.Trim()

            wStr = "cStatusIndi <> 'D'"

            If Not objhelpDb.GetTimeZoneMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_PreviousTimeZone, eStr) Then
                Me.ShowErrorMessage("", eStr)
                Exit Function
            End If

            startDateEST = Me.txtStartDate.Text.Trim() + " " + Me.txtStarttime.Text.Trim()
            EndDateEST = Me.txtEndDate.Text.Trim() + " " + Me.txtEndTime.Text.Trim()

            TimeZoneName = Me.ddlTimezone.SelectedValue.Trim()
            strOffset = TimeZoneName.Substring(4, 6)

            'If ds_PreviousTimeZone.Tables(0).Rows.Count <> 1 Then
            '    startDate = ds_PreviousTimeZone.Tables(0).Rows(ds_PreviousTimeZone.Tables(0).Rows.Count - 1).Item("dDaylightEnd").ToString
            'Else
            '    startDate = objCommon.GetDayLightSavingTime(Me.txtStartDate.Text.Trim() + " " + Me.txtStarttime.Text.Trim(), strOffset)
            'End If
            startDate = objCommon.GetDayLightSavingTime(Me.txtStartDate.Text.Trim() + " " + Me.txtStarttime.Text.Trim(), strOffset)
            EndDate = objCommon.GetDayLightSavingTime(Me.txtEndDate.Text.Trim() + " " + Me.txtEndTime.Text.Trim(), strOffset)


            wStr = "cStatusIndi <> 'D'"
            wStr += " AND vLocationName='" + Me.ddlLocation.SelectedItem.ToString + "'"
            If Not objhelpDb.getLocationMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Country, eStr) Then
                Me.ShowErrorMessage("", eStr)
                Exit Function
            End If
            vTimeZone = ds_Country.Tables(0).Rows(ds_Country.Tables(0).Rows.Count - 1).Item("vTimeZoneName").ToString

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                dr = ds_Timezone.Tables(0).NewRow()
                dr("vTimeZoneName") = vTimeZone
                dr("vLocationName") = Me.ddlLocation.SelectedValue.Trim()
                dr("vTimeZoneOffset") = TimeZoneName.Substring(4, 6)
                'dr("vTimeZoneOffset") = Me.txtoffset.Text.Trim()       'Commented By Rahul on Shah 19-March-2015
                dr("iModifyBy") = Session(S_UserID)
                dr("cStatusIndi") = "N"
                dr("dDaylightStart") = startDate
                dr("dDaylightEnd") = EndDate
                dr("dDaylightStartEST") = startDateEST      'Added By Rahul Shah on 19-March-2015
                dr("dDaylightEndEST") = EndDateEST
                dr("vRemarks") = Me.txtRemarks_delete.Text.Trim() 'Added by simki
                ds_Timezone.Tables(0).Rows.Add(dr)



            End If


            ds_Save = ds_Timezone

            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...AssignUpdates")
            Return False
        End Try


    End Function

#End Region

#Region "Button Events"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim ds_Save As New DataSet
        Dim eStr As String = String.Empty
        Try
            If Not AssignUpdatedValues(ds_Save) Then
                objCommon.ShowAlert("Error While Assigning values", Me.Page)
                Exit Sub
            End If

            If Not objLambda.Save_TimeZoneMst(Me.ViewState(VS_Choice), ds_Save, Me.Session(S_UserID), eStr) Then
                objCommon.ShowAlert("Error While Saving TimeZonemst", Me.Page)
                Exit Sub
            End If

            If Not FillGrid() Then
                Me.ShowErrorMessage("Error While Binding", "btn_Save")
                Exit Sub
            End If
            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.objCommon.ShowAlert("Record Saved Sucessfully", Me.Page)
            Else
                Me.objCommon.ShowAlert("Record Updated Sucessfully", Me.Page)
            End If

            ResetPage()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "Error While Saving")
        End Try
    End Sub

    Protected Sub btnclose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnclose.Click
        Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Response.Redirect("frmTimZoneMst.aspx")
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

    Protected Sub btnRemarksUpdate_Click(sender As Object, e As EventArgs)
        Dim ds_Delete As New DataSet
        Dim estr As String = ""
        Dim dr As DataRow
        Dim dt_TimeZone As DataTable
        Dim wStr As String = String.Empty

        index = hdnTimeZoneNo.Value
        Try
            wStr = "nTimeZoneMstNo=" & index
            If Not objhelpDb.GetTimeZoneMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    ds_Delete, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From TimeZoneMaster", estr)
                Exit Sub
            End If
            dt_TimeZone = ds_Delete.Tables(0)
            For Each dr In dt_TimeZone.Rows
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("vRemarks") = Me.txtRemarks_delete.Text.Trim()
                dr("cStatusIndi") = "D"
                dr.AcceptChanges()
            Next
            dt_TimeZone.AcceptChanges()
            dt_TimeZone.TableName = "TimeZoneMst"
            ds_Delete = Nothing
            ds_Delete = New DataSet
            ds_Delete.Tables.Add(dt_TimeZone.Copy())

            If Not objLambda.Save_TimeZoneMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_Delete, Me.Session(S_UserID), estr) Then
                objCommon.ShowAlert("Error While InActivate TimeZoneMst!", Me.Page)
                Exit Sub
            End If
            ResetPage()
            objCommon.ShowAlert("Record Deleted Successfully!", Me.Page)
            txtRemarks_delete.Text = Nothing

            If Not FillGrid() Then
                Me.ShowErrorMessage("Error While Binding", "gvTimeZone_RowCommand")
                Exit Sub
            End If
            'If Not Me.GenCall() Then
            '    Exit Sub
            'End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")

        End Try
    End Sub


#Region "Web Method"

    <WebMethod> _
    Public Shared Function AuditTrail(ByVal nTimeZoneMstNo As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_TimeZoneMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty

        Dim dtTimeZoneMstHistrory As New DataTable
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
            vTableName = "TimeZoneMstHistory"
            vIdName = ""
            AuditFieldName = "nTimeZoneMstNo"
            AuditFieldValue = nTimeZoneMstNo
            vOtherTableName = ""
            vOtherIdName = ""


            Dim Param As String = ""


            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName
            If Not objHelp.Proc_GetAuditTrail(wStr, ds_TimeZoneMst, Param) Then
                Throw New Exception(estr)
            End If

            If Not dtTimeZoneMstHistrory Is Nothing Then
                dtTimeZoneMstHistrory.Columns.Add("SrNo")
                dtTimeZoneMstHistrory.Columns.Add("TimeZoneName")
                dtTimeZoneMstHistrory.Columns.Add("LocationName")
                dtTimeZoneMstHistrory.Columns.Add("Remarks")
                dtTimeZoneMstHistrory.Columns.Add("ModifyBy")
                dtTimeZoneMstHistrory.Columns.Add("ModifyOn")
            End If
            dt = ds_TimeZoneMst.Tables(0)
            Dim dv As New DataView(dt)

            For Each dr As DataRow In dv.ToTable.Rows

                drAuditTrail = dtTimeZoneMstHistrory.NewRow()

                drAuditTrail("SrNo") = i
                drAuditTrail("TimeZoneName") = dr("vTimeZoneName").ToString()
                drAuditTrail("LocationName") = dr("vLocationName").ToString()
                drAuditTrail("Remarks") = dr("vRemarks").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("ModifyOn"))
                dtTimeZoneMstHistrory.Rows.Add(drAuditTrail)
                dtTimeZoneMstHistrory.AcceptChanges()
                i += 1
            Next
            strReturn = JsonConvert.SerializeObject(dtTimeZoneMstHistrory)
            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function



    Protected Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_TimeZoneMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty

        Dim dtTimeZoneMstHistrory As New DataTable
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
            vTableName = "TimeZoneMstHistory"
            vIdName = ""
            AuditFieldName = "nTimeZoneMstNo"
            AuditFieldValue = hdnTimeZoneNo.Value
            vOtherTableName = ""
            vOtherIdName = ""


            Dim Param As String = ""


            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName


            If Not objHelp.Proc_GetAuditTrail(wStr, ds_TimeZoneMst, Param) Then
                Throw New Exception(estr)
            End If

            If Not dtTimeZoneMstHistrory Is Nothing Then
                dtTimeZoneMstHistrory.Columns.Add("SrNo")
                dtTimeZoneMstHistrory.Columns.Add("TimeZoneName")
                dtTimeZoneMstHistrory.Columns.Add("LocationName")
                dtTimeZoneMstHistrory.Columns.Add("Remarks")
                dtTimeZoneMstHistrory.Columns.Add("ModifyBy")
                dtTimeZoneMstHistrory.Columns.Add("ModifyOn")
            End If
            dt = ds_TimeZoneMst.Tables(0)
            Dim dv As New DataView(dt)

            For Each dr As DataRow In dv.ToTable.Rows

                drAuditTrail = dtTimeZoneMstHistrory.NewRow()

                drAuditTrail("SrNo") = i
                drAuditTrail("TimeZoneName") = dr("vTimeZoneName").ToString()
                drAuditTrail("LocationName") = dr("vLocationName").ToString()
                drAuditTrail("Remarks") = dr("vRemarks").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("dModifyOn"))
                dtTimeZoneMstHistrory.Rows.Add(drAuditTrail)
                dtTimeZoneMstHistrory.AcceptChanges()
                i += 1
            Next


            gvExport.DataSource = dtTimeZoneMstHistrory
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

                filename = "Audit Trail_" + hdnTimeZoneNo.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

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
                strMessage.Append("TimeZone Master-AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")

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

                filename = "Audit Trail_" + hdnTimeZoneNo.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"


                drAuditTrail = dtTimeZoneMstHistrory.NewRow()

                drAuditTrail("SrNo") = ""
                drAuditTrail("TimeZoneName") = ""
                drAuditTrail("LocationName") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("ModifyBy") = ""
                drAuditTrail("ModifyOn") = ""
                dtTimeZoneMstHistrory.Rows.Add(drAuditTrail)
                dtTimeZoneMstHistrory.AcceptChanges()

                gvExport.DataSource = dtTimeZoneMstHistrory
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
                strMessage.Append("TimeZone Master-AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_UserNameWithProfile) + "</b></font></td></tr>")


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
            Me.objCommon.ShowAlert("Error While Exporting Data To Excel : " + estr, Me.Page)

            Exit Sub
        End Try
    End Sub


    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    End Sub
#End Region
End Class
