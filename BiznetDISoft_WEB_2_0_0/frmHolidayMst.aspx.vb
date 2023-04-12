
Imports System.Collections.Generic

Partial Class frmHolidayMst
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()


    Private Const VS_Choice As String = "Choice"
    Private Const VS_dtHoliday As String = "dtHoliday"
    Private Const VS_HolidayNo As String = "HolidayNo"
    Private VS_ReportString As String = "ReportString"
    Private Const VS_dtViewGrid As String = "ViewGrid"
    Private Const VS_EditSaved As String = "EditSaved"

    Private eStr_Retu As String = ""

    Private Const GVView_HolidayNo As Integer = 0
    Private Const GVView_LocationCode As Integer = 1
    Private Const GVView_HolidayDate As Integer = 2
    Private Const GVView_HolidayType As Integer = 3
    Private Const GVView_HolidayDescription As Integer = 4
    Private Const GVView_ActiveFlag As Integer = 5
    Private Const GVView_ModifyBy As Integer = 6
    Private Const GVView_ModifyOn As Integer = 7
    Private Const GVView_StatusIndi As Integer = 8
    Private Const GVView_LocationName As Integer = 9
    Private Const GVView_Edit As Integer = 10

    Private Const GVCell_HolidayNo As Integer = 0
    Private Const GVCell_LocationCode As Integer = 1
    Private Const GVCell_Location As Integer = 2
    Private Const GVCell_Date As Integer = 3
    Private Const GVCell_HolidayName As Integer = 4
    Private Const GVCell_Day As Integer = 5
    Private Const GVCell_HolidayType As Integer = 6

    Private Const HoliFlag As String = "H"
    Private Const WeeklyFlag As String = "W"

#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Not GenCall() Then
                Exit Sub
            End If
        End If

    End Sub

#End Region

#Region "GenCall "
    Private Function GenCall() As Boolean
        Dim dsDrugRegionAnalytesRpt As New DataSet
        Dim wStr As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try

            Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add

            If Not IsNothing(Me.Request.QueryString("mode")) Then
                Choice = Me.Request.QueryString("mode").ToString
            End If

            Me.ViewState(VS_Choice) = Choice   'To use it while saving

            If Not GenCall_Data() Then ' For Data Retrieval
                Exit Function
            End If

            If Not GenCall_ShowUI() Then 'For Displaying Data 
                Exit Function
            End If

            GenCall = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....GenCall")
            Return False
        End Try
    End Function
#End Region

#Region "GenCall_Data "

    Private Function GenCall_Data() As Boolean
        Dim dsHoliday As New DataSet
        Dim unqConstraint As UniqueConstraint = Nothing
        Dim wStr As String = String.Empty
        Dim Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum
        Try
            Me .ShowErrorMessage ("","")

            Choice_1 = Me.ViewState(VS_Choice)

            If Choice_1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "iHolidayNo=" + Me.Request.QueryString("Value").ToString 'Value of where condition
            End If

            wStr += " And cActiveFlag <> 'N' And cStatusIndi <> 'D'"

            If Not objHelp.GetViewHolidayMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    dsHoliday, eStr_Retu) Then

                Throw New Exception(eStr_Retu)
                Return False

            End If

            If Not IsNothing(Me.Request.QueryString("Value")) Then
                Me.ViewState(VS_HolidayNo) = Me.Request.QueryString("Value").ToString
            End If

            unqConstraint = New UniqueConstraint("UC1", New DataColumn() {dsHoliday.Tables(0).Columns("vLocationCode"), dsHoliday.Tables(0).Columns("dHolidayDate")})
            dsHoliday.Tables(0).Constraints.Add(unqConstraint)
            dsHoliday.AcceptChanges()
            Me.ViewState(VS_dtHoliday) = dsHoliday.Tables(0)

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_Data")
            Return False
        End Try
    End Function

#End Region

#Region "GenCall_ShowUI "

    Private Function GenCall_ShowUI() As Boolean

        Dim Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum
        Try

            Choice_1 = Me.ViewState(VS_Choice)

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Holiday Master"

            Page.Title = " :: Holiday Master ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")

            Me.chkSelectAllLocation.Attributes.Add("onclick", "CheckBoxListSelection(" + Me.chklstLocation.ClientID + ",this);")

            Me.btnAddHoliday.Attributes.Add("OnClick", "return Validation();")

            If Not FillLocationCheckboxList() Then
                ObjCommon.ShowAlert("Error Filling Location Checkbox List", Me)
                Return False
            End If

            If Not FillYearDropDown() Then
                ObjCommon.ShowAlert("Error Filling Year Drop Down List", Me)
                Return False
            End If

            Me.ddlLocation.Visible = False
            Me.chklstLocation.Visible = True
            Me.chkSelectAllLocation.Visible = True

            If Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                For Each dr As DataRow In CType(Me.ViewState(VS_dtHoliday), DataTable).Rows

                    SetHolidayEdit(dr)

                Next dr

                Me.rdoWeeklyOff.Disabled = True
                Me.divUpdateCancel.Visible = True
                Me.btnAddHoliday.Visible = False
                Me.ddlLocation.Visible = True
                Me.chklstLocation.Visible = False
                Me.chkSelectAllLocation.Visible = False
                Me.btnClear.Visible = False

            End If

            If Me.rdoHoliday.Checked Then

                Me.divHolidayOption.Style.Add("display", "block")
                Me.divWeeklyOffOption.Style.Add("display", "none")

            ElseIf Me.rdoWeeklyOff.Checked Then

                Me.divHolidayOption.Style.Add("display", "none")
                Me.divWeeklyOffOption.Style.Add("display", "block")

            End If

            Return True

        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region

#Region "CHECKBOX EVENTS "

    Protected Sub chkSelectAllLocation_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSelectAllLocation.CheckedChanged

        If Me.chkSelectAllLocation.Checked = False Then

            Me.chklstLocation.ClearSelection()

        ElseIf Me.chkSelectAllLocation.Checked = True Then

            If Me.chklstLocation.Items.Count > 0 Then
                Me.chklstLocation.ClearSelection()

                For Each lstItem As ListItem In Me.chklstLocation.Items
                    lstItem.Selected = Me.chkSelectAllLocation.Checked
                Next lstItem

            End If

        End If
    End Sub

#End Region

#Region "GRIDVIEW EVENTS "

    Protected Sub gvwHolidays_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Dim tempDate As Date = DateTime.MinValue

        If e.Row.RowType = DataControlRowType.DataRow Then

            If e.Row.Cells(GVCell_Date).Text.ToString.Length > 8 Then

                If DateTime.TryParse(e.Row.Cells(GVCell_Date).Text.Trim, tempDate) Then

                    e.Row.Cells(GVCell_Day).Text = [Enum].GetName(GetType(DayOfWeek), tempDate.DayOfWeek).ToString.Trim

                End If

            End If

            If e.Row.Cells(GVCell_HolidayType).Text.ToUpper.Trim() = WeeklyFlag Then

                CType(e.Row.FindControl("imgBtnEdit"), ImageButton).Visible = False

            End If

        End If

        If e.Row.RowType = DataControlRowType.DataRow Or _
            e.Row.RowType = DataControlRowType.Footer Or _
            e.Row.RowType = DataControlRowType.Header Then

            e.Row.Cells(GVCell_LocationCode).Visible = False
            e.Row.Cells(GVCell_HolidayType).Visible = False

        End If

    End Sub

    Protected Sub gvwHolidays_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Dim dtv As New DataView
        Dim dtHoliday As New DataTable
        Dim whereCondition As String = "iHolidayNo =" + e.CommandArgument
        Dim isDeleted As Boolean = False

        If e.CommandName.ToUpper.Trim() = "MYEDIT" Then

            Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit

            Me.ViewState(VS_HolidayNo) = e.CommandArgument.ToString.Trim
            Me.ViewState(VS_Choice) = Choice

            If Not Me.ViewState(VS_dtHoliday) Is Nothing Then

                dtHoliday = Me.ViewState(VS_dtHoliday)

                If dtHoliday.Rows.Count <= 0 Then
                    ObjCommon.ShowAlert("No Data To Edit", Me)
                    Exit Sub
                End If

                dtv = dtHoliday.DefaultView()
                dtv.RowFilter = whereCondition
                Me.SetHolidayEdit(dtv.ToTable.Rows(0))
                Me.divUpdateCancel.Visible = True
                Me.btnAddHoliday.Visible = False
                Me.rdoHoliday.Checked = True
                Me.rdoWeeklyOff.Checked = False
                Me.ddlLocation.Visible = True
                Me.chklstLocation.Visible = False
                Me.chkSelectAllLocation.Visible = False
                Me.btnClear.Visible = False

                Me.ViewState(VS_EditSaved) = "N"

            End If

        ElseIf e.CommandName.ToUpper.Trim() = "MYDELETE" Then

            Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete

            Me.ViewState(VS_HolidayNo) = e.CommandArgument.ToString.Trim
            Me.ViewState(VS_Choice) = Choice

            If Not Me.ViewState(VS_dtHoliday) Is Nothing Then
                dtHoliday = Me.ViewState(VS_dtHoliday)

                If dtHoliday.Rows.Count <= 0 Then
                    ObjCommon.ShowAlert("No Data To Edit", Me)
                    Exit Sub
                End If

                For index As Integer = dtHoliday.Rows.Count - 1 To 0 Step -1

                    If Not isDeleted Then

                        dtHoliday.Rows(index)("iHolidayNo") = Integer.Parse(dtHoliday.Rows(index)("iHolidayNo")) - 1
                        dtHoliday.Rows(index).AcceptChanges()
                        dtHoliday.AcceptChanges()

                    End If

                    If dtHoliday.Rows(index)("iHolidayNo") = e.CommandArgument Then

                        dtHoliday.Rows.RemoveAt(index)
                        dtHoliday.AcceptChanges()
                        isDeleted = True
                        Exit For

                    End If

                Next index

                dtHoliday.AcceptChanges()
                Me.ViewState(VS_dtHoliday) = dtHoliday
                Me.FillHolidayGrid()
                isDeleted = False

            End If

        End If

    End Sub

    Protected Sub gvwHolidays_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)

        Me.gvwHolidays.PageIndex = e.NewPageIndex
        Me.FillHolidayGrid()

    End Sub

#End Region

#Region "BUTTON EVENTS "

    Protected Sub btnAddHoliday_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddHoliday.Click
        If Me.chklstLocation.SelectedIndex() < 0 Then
            ObjCommon.ShowAlert("Please Select Location(s) To Add", Me.Page())
            Exit Sub
        End If
        Dim dtHoliday As New DataTable
        Dim dr As DataRow
        Dim strLocation As String = String.Empty
        Try



            If Not Me.ViewState(VS_dtHoliday) Is Nothing Then

                dtHoliday = CType(Me.ViewState(VS_dtHoliday), DataTable)

                For Each lstItem As ListItem In Me.chklstLocation.Items

                    If lstItem.Selected Then
                        dr = dtHoliday.NewRow()
                        strLocation = lstItem.Text
                        dr("vLocationCode") = lstItem.Value.Trim
                        dr("vLocationName") = lstItem.Text.Trim
                        dr("dHolidayDate") = Me.txtDate.Text

                        If Me.rdoHoliday.Checked Then
                            dr("cHolidayType") = HoliFlag
                        ElseIf Me.rdoWeeklyOff.Checked Then
                            dr("cHolidayType") = WeeklyFlag
                        End If

                        dr("vHolidayDescription") = Me.txtHolidayDescription.Text.Trim
                        dr("cActiveFlag") = IIf(Me.chkIsActive.Checked, GeneralModule.ActiveFlag_Yes, GeneralModule.ActiveFlag_No)
                        dr("iModifyBy") = Me.Session(S_UserID)
                        dr("cStatusIndi") = "N"

                        dr("iHolidayNo") = IIf(dtHoliday.Rows.Count > 0, dtHoliday.Rows.Count + 1, 1)
                        dtHoliday.Rows.Add(dr)
                        dtHoliday.AcceptChanges()

                    End If

                Next lstItem

                Me.ViewState(VS_dtHoliday) = dtHoliday
                FillHolidayGrid()
                ResetForm()

            End If

        Catch ex As System.Data.ConstraintException
            Me.ObjCommon.ShowAlert("Date:" + Me.txtDate.Text + " Already Exists for " + strLocation, Me)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr_Retu)
        End Try
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Dim strHolidayNo As String = Me.ViewState(VS_HolidayNo)
        Dim dtHoliday As New DataTable
        Dim dsHoliday As New DataSet
        Try

           
            If Me.ViewState(VS_dtHoliday) Is Nothing Then
                Exit Sub
            End If

            dtHoliday = CType(Me.ViewState(VS_dtHoliday), DataTable)

            For Each dr As DataRow In dtHoliday.Rows

                If dr("iHolidayNo") = strHolidayNo Then

                    If Me.rdoHoliday.Checked Then

                        dr("cHolidayType") = HoliFlag
                        dr("vLocationCode") = Me.ddlLocation.SelectedValue
                        dr("vLocationName") = Me.ddlLocation.SelectedItem.Text.Trim
                        dr("vHolidayDescription") = Me.txtHolidayDescription.Text.Trim
                        dr("dHolidayDate") = Me.txtDate.Text.Trim

                        If Me.chkIsActive.Checked Then
                            dr("cActiveFlag") = GeneralModule.ActiveFlag_Yes
                        Else
                            dr("cActiveFlag") = GeneralModule.ActiveFlag_No
                        End If

                    End If

                    dr("cStatusIndi") = "E"

                End If
                dr.AcceptChanges()

            Next dr

            dtHoliday.AcceptChanges()
            Me.ViewState(VS_dtHoliday) = dtHoliday
            Me.divUpdateCancel.Visible = False
            Me.btnAddHoliday.Visible = True

            Me.ddlLocation.Visible = False
            Me.chklstLocation.Visible = True
            Me.chkSelectAllLocation.Visible = True
            Me.btnClear.Visible = True

            If IsNothing(Me.ViewState(VS_EditSaved)) AndAlso _
                CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                dsHoliday.Tables.Add(dtHoliday)

                If Not Me.objLambda.Save_InsertHolidayMst(Me.ViewState(VS_Choice), dsHoliday, Me.Session(S_UserID), eStr_Retu) Then

                    ObjCommon.ShowAlert(eStr_Retu, Me)
                    Exit Sub

                End If

                Me.rdoWeeklyOff.Disabled = False
                Me.ViewState(VS_HolidayNo) = ""
                dtHoliday.Clear()
                Me.ViewState(VS_dtHoliday) = dtHoliday

                Me.ResetForm()
                ObjCommon.ShowAlert("Holiday Information Updated Successfully", Me)

            ElseIf (Not IsNothing(Me.ViewState(VS_EditSaved)) AndAlso Me.ViewState(VS_EditSaved) = "N") And CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                Me.FillHolidayGrid()
                Me.ResetForm()
                ObjCommon.ShowAlert("Holiday Information Updated Successfully", Me)

            End If

            Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....btnUpdate_Click")
        End Try

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        ResetForm()
        FillHolidayGrid()
        Me.divUpdateCancel.Visible = False
        Me.btnAddHoliday.Visible = True

    End Sub

    Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click
        If Me.chklstLocation.SelectedIndex() < 0 Then
            ObjCommon.ShowAlert("Please Select Location(s) To Add", Me.Page())
            Exit Sub
        End If
        Dim month As Integer = 8
        Dim year As Integer = Integer.Parse(Me.ddlYear.SelectedValue.Trim)
        Dim dictWeeklyOff As Dictionary(Of String, String) = Me.GetSundaysForMonthAndYear(month, year)
        Dim dtHoliday As New DataTable
        Dim dr As DataRow

        If Not Me.ViewState(VS_dtHoliday) Is Nothing Then
            dtHoliday = CType(Me.ViewState(VS_dtHoliday), DataTable)
        End If

        Try

            Me.divHolidayOption.Style.Add("display", "none")
            Me.divWeeklyOffOption.Style.Add("display", "block")
            For Each lstItem As ListItem In Me.chklstLocation.Items

                If lstItem.Selected Then

                    For Each kvp As KeyValuePair(Of String, String) In dictWeeklyOff

                        dr = dtHoliday.NewRow
                        dr("iHolidayNo") = IIf(dtHoliday.Rows.Count > 0, dtHoliday.Rows.Count + 1, 1)
                        dr("vLocationCode") = lstItem.Value.Trim
                        dr("vLocationName") = lstItem.Text.Trim
                        dr("dHolidayDate") = kvp.Key
                        dr("vHolidayDescription") = kvp.Value

                        If Me.rdoHoliday.Checked Then
                            dr("cHolidayType") = HoliFlag
                        ElseIf Me.rdoWeeklyOff.Checked Then
                            dr("cHolidayType") = WeeklyFlag
                        End If

                        dr("cActiveFlag") = GeneralModule.ActiveFlag_Yes
                        dr("iModifyBy") = Me.Session(S_UserID)
                        dr("dModifyOn") = DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt")
                        dr("cStatusIndi") = "N"
                        dtHoliday.Rows.Add(dr)
                        dtHoliday.AcceptChanges()

                    Next kvp

                End If

            Next lstItem

            Me.ViewState(VS_dtHoliday) = dtHoliday
            Me.FillHolidayGrid()

            Me.pnlView.Visible = False

            Me.divHolidayOption.Style.Add("display", "none")
            Me.divWeeklyOffOption.Style.Add("display", "block")

        Catch ex As Exception
            Me.ObjCommon.ShowAlert(ex.Message, Me.Page())
        End Try


    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim dtHoliday As New DataTable
        Dim dsHoliday As New DataSet

        If Not Me.ViewState(VS_dtHoliday) Is Nothing Then
            dtHoliday = CType(Me.ViewState(VS_dtHoliday), DataTable)

            If dtHoliday.Rows.Count <= 0 Then
                ObjCommon.ShowAlert("No Data To Save", Me)
                Exit Sub
            End If

            dsHoliday.Tables.Add(dtHoliday)

            If Not Me.objLambda.Save_InsertHolidayMst(Me.ViewState(VS_Choice), dsHoliday, Me.Session(S_UserID), eStr_Retu) Then

                ObjCommon.ShowAlert(eStr_Retu, Me)
                Exit Sub

            End If

            ResetForm()

            If Not Me.ViewState(VS_dtHoliday) Is Nothing Then

                dtHoliday.Rows.Clear()
                Me.ViewState(VS_dtHoliday) = dtHoliday
                Me.FillHolidayGrid()

            End If

            ObjCommon.ShowAlert("Holidays Saved Successfully !", Me)

        End If
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Dim dtHoliday As New DataTable

        If Not Me.ViewState(VS_dtHoliday) Is Nothing Then

            dtHoliday = CType(Me.ViewState(VS_dtHoliday), DataTable)
            dtHoliday.Rows.Clear()
            Me.ViewState(VS_dtHoliday) = dtHoliday

        End If

        Me.FillHolidayGrid()

    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

#End Region

#Region " Helper Functions and Methods "

    Private Function FillLocationCheckboxList() As Boolean
        Dim dsLocation As New DataSet
        Try
            If Not objHelp.getLocationMst("cStatusIndi<>'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        dsLocation, eStr_Retu) Then

                Throw New Exception(eStr_Retu)
                Return False

            End If

            Me.chklstLocation.DataSource = dsLocation.Tables(0)
            Me.chklstLocation.DataTextField = "vLocationName"
            Me.chklstLocation.DataValueField = "vLocationCode"
            Me.chklstLocation.DataBind()

            Me.ddlLocation.DataSource = dsLocation.Tables(0)
            Me.ddlLocation.DataTextField = "vLocationName"
            Me.ddlLocation.DataValueField = "vLocationCode"
            Me.ddlLocation.DataBind()

            For Each lstItem As ListItem In chklstLocation.Items

                lstItem.Attributes.Add("onclick", "SetCheckUncheckAll(document.getElementById('" + chklstLocation.ClientID + _
                                                    "'), document.getElementById('" + Me.chkSelectAllLocation.ClientID + "'));")

            Next lstItem

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillLocationCheckboxList")
            Return False
        End Try
        Return True
    End Function

    Private Sub FillHolidayGrid()

        Dim dt As New DataTable

        Me.gvwHolidays.DataSource = Nothing
        Me.gvwHolidays.DataBind()

        If Not Me.ViewState(VS_dtHoliday) Is Nothing Then
            dt = CType(Me.ViewState(VS_dtHoliday), DataTable)
            Me.gvwHolidays.DataSource = dt
            Me.gvwHolidays.DataBind()

            Me.btnSave.Visible = False

            'If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
            If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Or _
                (Not IsNothing(Me.ViewState(VS_EditSaved)) AndAlso Me.ViewState(VS_EditSaved) = "N") Then
                'End If
                If dt.Rows.Count > 0 Then
                    Me.btnSave.Visible = True
                End If
            End If
        End If

    End Sub

    Private Sub ResetForm()

        Me.rdoWeeklyOff.Checked = False
        Me.rdoHoliday.Checked = True
        Me.chkSelectAllLocation.Checked = False
        Me.chklstLocation.ClearSelection()
        Me.txtDate.Text = ""
        Me.txtHolidayDescription.Text = ""
        Me.chkIsActive.Checked = True

    End Sub

    Private Sub SetHolidayEdit(ByVal dr As DataRow)

        Me.txtDate.Text = DateTime.Parse(dr("dHolidayDate").ToString).ToString("dd-MMM-yy")
        Me.txtHolidayDescription.Text = dr("vHolidayDescription").ToString.Trim

        If dr("cHolidayType").ToString.ToUpper.Trim() = HoliFlag Then

            Me.rdoHoliday.Checked = True
            Me.rdoWeeklyOff.Checked = False
        ElseIf dr("cHolidayType").ToString.ToUpper.Trim() = WeeklyFlag Then

            Me.rdoHoliday.Checked = False
            Me.rdoWeeklyOff.Checked = True
        End If

        If dr("cActiveFlag") = GeneralModule.ActiveFlag_Yes Then
            Me.chkIsActive.Checked = True
        ElseIf dr("cActiveFlag") = GeneralModule.ActiveFlag_No Then
            Me.chkIsActive.Checked = False
        End If

        If Not dr("vLocationCode") Is Nothing Then
            Me.ddlLocation.SelectedValue = dr("vLocationCode")
        End If

        Me.divHolidayOption.Style.Add("display", "block")
        Me.divWeeklyOffOption.Style.Add("display", "none")
        Me.rdoHoliday.Attributes.Add("checked", "checked")

    End Sub

    Private Function FillYearDropDown() As Boolean
        Dim currentYear As Integer
        Try


            currentYear = DateTime.Now.Year

            Me.ddlYear.Items.Add(currentYear - 4)
            Me.ddlYear.Items.Add(currentYear - 3)
            Me.ddlYear.Items.Add(currentYear - 2)
            Me.ddlYear.Items.Add(currentYear - 1)
            Me.ddlYear.Items.Add(currentYear)
            Me.ddlYear.Items.Add(currentYear + 1)
            Me.ddlYear.Items.Add(currentYear + 2)
            Me.ddlYear.Items.Add(currentYear + 3)
            Me.ddlYear.Items.Add(currentYear + 4)
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillYearDropDown")
            Return False
        End Try

    End Function

    Private Function GetSundaysForMonthAndYear(ByVal month As Integer, ByVal year As Integer) As Dictionary(Of String, String)

        Dim dictList As System.Collections.Generic.Dictionary(Of String, String) = New System.Collections.Generic.Dictionary(Of String, String)()



        For index As Integer = 1 To 12
            Dim firstDate As DateTime = New DateTime(year, index, 1)
            Dim lastDate As DateTime = New DateTime(year, index, DateTime.DaysInMonth(year, index))
            Dim tempDate As DateTime = firstDate
            Dim week As Integer = 1

            Do While (tempDate <= lastDate)

                If tempDate.DayOfWeek = DayOfWeek.Sunday Then
                    dictList.Add(tempDate.ToString("dd-MMM-yy"), "Sunday")
                End If

                If tempDate.DayOfWeek = DayOfWeek.Saturday Then

                    For Each lstItem As ListItem In Me.chkLstSaturday.Items

                        If (lstItem.Selected = True) And (lstItem.Value = week) Then

                            dictList.Add(tempDate.ToString("dd-MMM-yy"), "Saturday")
                            If tempDate <> firstDate Then
                            End If

                        End If

                    Next lstItem
                    week += 1
                End If
                tempDate = tempDate.AddDays(1)
            Loop

        Next
        Return dictList
    End Function

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

#Region "For View "

    Protected Sub btnView_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            Me.txtFromDate.Text = Now.Date.ToString("dd-MMM-yy")
            Me.txtToDate.Text = Now.Date.ToString("dd-MMM-yy")
            Me.pnlView.Visible = True
            Me.gvwView.DataSource = Nothing
            Me.gvwView.DataBind()

            If Me.rdoHoliday.Checked Then

                Me.divHolidayOption.Style.Add("display", "block")
                Me.divWeeklyOffOption.Style.Add("display", "none")

            ElseIf Me.rdoWeeklyOff.Checked Then

                Me.divHolidayOption.Style.Add("display", "none")
                Me.divWeeklyOffOption.Style.Add("display", "block")

            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....btnView_Click")
        End Try
    End Sub

    Protected Sub btnDivView_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds_View As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Try

            Me.ViewState(VS_dtViewGrid) = Nothing
            Me.gvwView.DataSource = Nothing
            Me.gvwView.DataBind()


            wStr = "vLocationCode in('"

            If Me.chklstLocation.Visible = True Then

                For Each lstItem As ListItem In Me.chklstLocation.Items

                    If lstItem.Selected Then
                        wStr += lstItem.Value.Trim + "','"
                    End If

                Next lstItem
            Else
                wStr += Me.ddlLocation.SelectedValue.Trim()
            End If

            wStr += "')"
            wStr += " And (dHolidayDate between'" + Me.txtFromDate.Text + "' And '" + Me.txtToDate.Text + "')"
            wStr += " And cHolidayType = '"
            If Me.rdoHoliday.Checked Then
                wStr += Me.rdoHoliday.Value.Trim
            Else
                wStr += Me.rdoWeeklyOff.Value.Trim
            End If
            wStr += "' And cActiveFlag <> 'N'"

            If Not objHelp.GetViewHolidayMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    ds_View, eStr) Then

                Me.ShowErrorMessage("Error While Getting Data From View_HolidayMst", eStr)
                Exit Sub

            End If

            If ds_View.Tables(0).Rows.Count < 1 Then
                ObjCommon.ShowAlert("No Record Found", Me.Page)
                Exit Sub
            End If

            Me.ViewState(VS_dtViewGrid) = ds_View.Tables(0)
            Me.gvwView.DataSource = ds_View.Tables(0)
            Me.gvwView.DataBind()

            If Me.rdoHoliday.Checked Then

                Me.divHolidayOption.Style.Add("display", "block")
                Me.divWeeklyOffOption.Style.Add("display", "none")

            ElseIf Me.rdoWeeklyOff.Checked Then

                Me.divHolidayOption.Style.Add("display", "none")
                Me.divWeeklyOffOption.Style.Add("display", "block")

            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....btnDivView_Click")
        End Try
    End Sub

    Protected Sub btnDivClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDivClose.Click
        Me.pnlView.Visible = False
        Me.ViewState(VS_dtViewGrid) = Nothing
        Me.gvwView.DataSource = Nothing
        Me.gvwView.DataBind()
        Me.txtFromDate.Text = ""
        Me.txtToDate.Text = ""
        If Me.rdoHoliday.Checked Then

            Me.divHolidayOption.Style.Add("display", "block")
            Me.divWeeklyOffOption.Style.Add("display", "none")

        ElseIf Me.rdoWeeklyOff.Checked Then

            Me.divHolidayOption.Style.Add("display", "none")
            Me.divWeeklyOffOption.Style.Add("display", "block")

        End If
    End Sub

    Protected Sub gvwView_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.DataRow Or _
            e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVView_HolidayNo).Visible = False
            e.Row.Cells(GVView_LocationCode).Visible = False
            e.Row.Cells(GVView_HolidayType).Visible = False
            e.Row.Cells(GVView_ActiveFlag).Visible = False
            e.Row.Cells(GVView_ModifyBy).Visible = False
            e.Row.Cells(GVView_ModifyOn).Visible = False
            e.Row.Cells(GVView_StatusIndi).Visible = False

            If Me.rdoWeeklyOff.Checked = True Then
                e.Row.Cells(GVView_Edit).Visible = False
            End If

        End If

    End Sub

    Protected Sub gvwView_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.gvwView.PageIndex = e.NewPageIndex
        Me.FillViewGrid()
    End Sub

    Protected Sub gvwView_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)

        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Dim dvHoliday As New DataView
        Dim dtHoliday As New DataTable
        Dim dsHoliday As New DataSet
        Dim whereCondition As String = "iHolidayNo =" + e.CommandArgument
        Dim isDeleted As Boolean = False

        If e.CommandName.ToUpper.Trim() = "MYEDIT" Then

            Me.Response.Redirect("frmHolidayMst.aspx?mode=2&Value=" & e.CommandArgument)

        ElseIf e.CommandName.ToUpper.Trim() = "MYDELETE" Then
            Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete

            dvHoliday = CType(Me.ViewState(VS_dtViewGrid), DataTable).DefaultView()
            dvHoliday.RowFilter = whereCondition
            dtHoliday = dvHoliday.ToTable

            dtHoliday.Rows(0).Item("cActiveFlag") = GeneralModule.ActiveFlag_No
            dtHoliday.AcceptChanges()

            dsHoliday.Tables.Add(dtHoliday.Copy())

            If Not Me.objLambda.Save_InsertHolidayMst(Choice, dsHoliday, Me.Session(S_UserID), eStr_Retu) Then
                ObjCommon.ShowAlert(eStr_Retu, Me)
                Exit Sub
            End If

            Me.ObjCommon.ShowAlert("Record Deleted Successfully.", Me.Page())

        End If
    End Sub

    Private Sub FillViewGrid()
        Dim dt As New DataTable
        Try

            Me.gvwView.DataSource = Nothing
            Me.gvwView.DataBind()

            If Not Me.ViewState(VS_dtViewGrid) Is Nothing Then
                dt = CType(Me.ViewState(VS_dtViewGrid), DataTable)
                Me.gvwView.DataSource = dt
                Me.gvwView.DataBind()
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillViewGrid")
        End Try
    End Sub

#End Region

End Class
