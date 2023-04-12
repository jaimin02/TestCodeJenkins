
Partial Class frmAutoSchedule
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION "

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()


    Private Const VS_Choice As String = "Choice"
    Private Const VS_dtAutoSchedule As String = "dtAutoSchedule"

    Private Const GVCell_ActivityId As Integer = 1
    Private Const GVCell_Activity As Integer = 2
    Private Const GVCell_CanStartAfter As Integer = 3
    Private Const GVCell_StartDate As Integer = 4
    Private Const GVCell_EndDate As Integer = 5
    Private Const GVCell_Days As Integer = 6
    Private Const GVCell_NodeId As Integer = 10
    Private Const GVCell_cPeriodSpecefic As Integer = 11
    Private Const GVCell_MileStone As Integer = 12

    Private eStr_Retu As String

#End Region

#Region "PAGE LOAD"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            GenCall()

        End If

    End Sub

#End Region

#Region "GenCall"
    Private Function GenCall() As Boolean
        Dim ds As New DataSet
        Try


            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_dtAutoSchedule) = ds.Tables(0)   ' adding blank DataTable in viewstate

            If Not Gencall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If

            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall")

        Finally

        End Try

    End Function
#End Region

#Region "GenCall_Data "
    Private Function GenCall_Data(ByRef ds_AutoSchedule As DataSet) As Boolean
        Dim wStr As String = String.Empty
        Dim ds As DataSet = Nothing

        Try


            wStr = "1=2"

            If Not objHelp.getWorkspaceNodeAttrHistory(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                                    ds, eStr_Retu) Then

                ObjCommon.ShowAlert(eStr_Retu, Me)
                Exit Function

            End If

            If ds Is Nothing Then
                Throw New Exception(eStr_Retu)
            End If

            ds_AutoSchedule = ds
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "........GenCall_Data")

        Finally

        End Try
    End Function
#End Region

#Region "Gencall_ShowUI"

    Private Function Gencall_ShowUI() As Boolean

        Dim scopeno As String = String.Empty
        Dim PMScopeNo As String = Scope_SAll.ToString()
        Dim Sender As New Object
        Dim e As EventArgs

        Try
            Page.Title = " :: Auto Schedule ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Master.FindControl("lblHeading"), Label).Text = "Auto Schedule"
            If Not FillProjectDropDown() Then
                Return False
            End If

            If Not IsNothing(Me.Request.QueryString("Saved")) AndAlso _
                Me.Request.QueryString("Saved").ToUpper = "YES" Then

                ObjCommon.ShowAlert("Activity slotting details saved successfully !", Me)

            End If
            '==added on 25-nov-2011 by Mrunal
            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtProject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(Sender, e)
                Gencall_ShowUI = True
            End If
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)

            '' === For give all rights to pmadmin ==
            scopeno = Me.Session(S_ScopeNo).ToString()

            If (scopeno = PMScopeNo) Then
                Me.AutoCompleteExtender1.ServiceMethod = "GetMyProjectCompletionListwithworkspacedesc"
            Else
                Me.AutoCompleteExtender1.ServiceMethod = "GetMyProjectCompletionList"
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....Gencall_ShowUI")
        End Try

    End Function

#End Region

#Region "Fill Functions"

    Private Function FillProjectDropDown() As Boolean
        Dim dsProject As New DataSet()
        Try
            Me.FillDropDown()
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillProjectDropDown")
            Return False
        End Try
    End Function

    Private Sub FillAutoScheduleGridView()
        Dim dsProjActDtl As New DataSet
        Dim dtProjActDtl As New DataTable

        Try

            If Not objHelp.GetProc_AutoScheduleDetails(HProjectId.Value.Trim, Session(S_UserID), True, _
                                                    dsProjActDtl, eStr_Retu) Then


                ObjCommon.ShowAlert(eStr_Retu, Me)
                Exit Sub

            End If
            dtProjActDtl = dsProjActDtl.Tables(0)
            Me.gvwAutoSchedule.DataSource = dtProjActDtl
            Me.gvwAutoSchedule.DataBind()

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....FillAutoScheduleGridView")
        End Try
    End Sub

    Private Sub FillDropDown()
        Dim ds_DropDown As DataSet = Nothing
        Dim Wstr As String = String.Empty
        Dim dv_DropDown As New DataView

        Try

            'Fill Location Drop Down
            ds_DropDown = New DataSet

            If Not objHelp.FillDropDown("LocationMst", "vLocationCode", "vLocationName", "cLocationType = 'L'", ds_DropDown, eStr_Retu) Then
                ObjCommon.ShowAlert(eStr_Retu, Me)
                Exit Sub
            End If

            dv_DropDown = ds_DropDown.Tables(0).DefaultView
            dv_DropDown.Sort = "vLocationName"
            ddlLocation.DataSource = dv_DropDown
            ddlLocation.DataValueField = "vLocationCode"
            ddlLocation.DataTextField = "vLocationName"
            ddlLocation.DataBind()
            ddlLocation.Items.Insert(0, New ListItem(" --- Select Location ---", "0"))

            'Fill Resource Drop Down
            ds_DropDown = New DataSet

            Wstr = ""
            If ddlLocation.SelectedIndex > 0 Then
                Wstr = " vLocationCode = " & ddlLocation.SelectedValue.Trim

                If Not objHelp.FillDropDown("ResourceMst", "vResourceCode", "vResourceName", Wstr, ds_DropDown, eStr_Retu) Then

                    ObjCommon.ShowAlert(eStr_Retu, Me)
                    Exit Sub

                End If

                dv_DropDown = New DataView
                dv_DropDown = ds_DropDown.Tables(0).DefaultView
                dv_DropDown.Sort = "vResourceName"
                ddlResource.DataSource = dv_DropDown
                ddlResource.DataValueField = "vResourceCode"
                ddlResource.DataTextField = "vResourceName"
                ddlResource.DataBind()
            End If

            ddlResource.Items.Insert(0, New ListItem(" --- Select Resource ---", "0"))

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....FillDropDown")
        End Try

    End Sub

#End Region

#Region "DROPDOWN LIST EVENTS"

    Protected Sub ddlLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlLocation.SelectedIndexChanged
        Dim ds_DropDown As New DataSet
        Dim dv_DropDown As New DataView
        Try


            If ddlLocation.SelectedIndex >= 0 Then

                If Not objHelp.FillDropDown("ResourceMst", "vResourceCode", "vResourceName", " vLocationCode = " & ddlLocation.SelectedValue.Trim, ds_DropDown, eStr_Retu) Then

                    ObjCommon.ShowAlert(eStr_Retu, Me)
                    Exit Sub

                End If

                dv_DropDown = ds_DropDown.Tables(0).DefaultView
                dv_DropDown.Sort = "vResourceName"
                ddlResource.DataSource = dv_DropDown
                ddlResource.DataValueField = "vResourceCode"
                ddlResource.DataTextField = "vResourceName"
                ddlResource.DataBind()
                ddlResource.Items.Insert(0, New ListItem(" --- Select Resource ---", "0"))

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......ddlLocation_SelectedIndexChanged")
        End Try
    End Sub

#End Region

#Region "BUTTON CLICK EVENTS"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        FillAutoScheduleGridView()
    End Sub

    Protected Sub btnView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnView.Click
        Dim startDate As DateTime = DateTime.MaxValue
        Dim endDate As DateTime = DateTime.MinValue
        Dim q As String = String.Empty

        'objHelp

        If Not Me.GetStartDateAndEndDate(startDate, endDate) Then
            ObjCommon.ShowAlert("Cannot Get Start Date And End Date", Me)
        End If

        If DateTime.MaxValue = startDate Then
            startDate = DateAdd(DateInterval.Month, -2, DateTime.Now)
        End If
        If DateTime.MinValue = endDate Then
            endDate = DateAdd(DateInterval.Year, 2, DateTime.Now)
        End If

        Dim mode As String = "View"

        q = "window.open(""" + "frmSlotDateandLocation.aspx?Start=" + startDate.ToString("dd-MMM-yyyy") + "&End=" + endDate.ToString("dd-MMM-yyyy") + "&mode=" + mode + "&Loc=" + Me.ddlLocation.SelectedValue.Trim + """)"
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", q, True)

    End Sub

    Protected Sub btnSchedule_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSchedule.Click

        Dim txtStartDate As TextBox
        Dim txtEndDate As TextBox
        Dim endDate As String = String.Empty
        Dim validErrStr As String = String.Empty
        Dim dys As Integer
        Try
            If Not Me.ValidateStartDates(validErrStr) Then
                ObjCommon.ShowAlert(validErrStr, Me)
                Exit Sub
            End If

            If Not Me.ValidateParentDate(validErrStr) Then
                ObjCommon.ShowAlert(validErrStr, Me)
                Exit Sub
            End If

            If Me.ddlLocation.SelectedIndex <= 0 Then
                ObjCommon.ShowAlert("Please Select A Location", Me)
                Exit Sub
            End If

            For Each gvr As GridViewRow In gvwAutoSchedule.Rows
                txtStartDate = CType(gvr.FindControl("txtStartDate"), TextBox)
                txtEndDate = CType(gvr.FindControl("txtEndDate"), TextBox)

                If Not txtStartDate Is Nothing Then

                    If txtStartDate.Text.Length > 8 Then

                        dys = CType(gvr.FindControl("txtCompletionDays"), TextBox).Text

                        endDate = AddDays(txtStartDate.Text, Integer.Parse(dys))
                        endDate = CheckSunday(DateTime.Parse(txtStartDate.Text), DateTime.Parse(endDate))
                        txtEndDate.Text = endDate

                    End If

                    If txtStartDate.Text.Length <= 0 And _
                       txtEndDate.Text.Length <= 0 Then

                        ScheduleDates(gvr.RowIndex)

                    End If
                End If

            Next gvr
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....btnSchedule_Click")
        End Try


    End Sub

    Protected Sub btnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApply.Click
        Dim chkSelectRow As CheckBox = Nothing

        If ddlLocation.SelectedIndex <= 0 Then
            ObjCommon.ShowAlert("Please Select Location.", Me)
            Exit Sub
        ElseIf ddlResource.SelectedIndex <= 0 Then
            ObjCommon.ShowAlert("Please Select Resource.", Me)
            Exit Sub
        End If

        For Each gvr As GridViewRow In gvwAutoSchedule.Rows

            If ddlLocation.SelectedIndex > 0 Then

                CType(gvr.FindControl("lblLocation"), Label).Text = ddlLocation.SelectedItem.Text
                CType(gvr.FindControl("HLocationId"), HiddenField).Value = ddlLocation.SelectedValue

            End If

            If ddlResource.SelectedIndex > 0 Then

                CType(gvr.FindControl("lblResource"), Label).Text = ddlResource.SelectedItem.Text
                CType(gvr.FindControl("HResourceId"), HiddenField).Value = ddlResource.SelectedValue

            End If

        Next gvr

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim dsWorkspaceNodeAttrHistory As New DataSet
        Dim dtWorkspaceNodeAttrHistory As New DataTable
        Dim resourceId As String = String.Empty
        Try



            If Not AssignValues() Then

                ObjCommon.ShowAlert("Error While AssingValues", Me)
                Exit Sub

            End If

            dsWorkspaceNodeAttrHistory.Tables.Add(CType(Me.ViewState(VS_dtAutoSchedule), DataTable).Copy())

            If Not objLambda.Save_InsertDataForWorkspaceNodeAttrHistory(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                                                 dsWorkspaceNodeAttrHistory, Session(S_UserID), eStr_Retu) Then

                ObjCommon.ShowAlert(eStr_Retu, Me)
                Exit Sub

            End If

            ObjCommon.ShowAlert("Activity Slotting Details Saved Successfully !", Me)
            Me.resetPage()
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....btnSave_Click")
        End Try

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'Me.resetPage()

        Me.ddlLocation.DataSource = Nothing
        Me.ddlResource.DataSource = Nothing
        Me.txtProject.Text = ""
        Me.HProjectId.Value = ""
        Me.ViewState(VS_dtAutoSchedule) = Nothing
        Me.Response.Redirect("frmAutoSchedule.aspx?Saved=NO")


    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

#End Region

#Region "ResetPage"

    Protected Sub resetPage()

        Me.ddlLocation.DataSource = Nothing
        Me.ddlResource.DataSource = Nothing

        Me.txtProject.Text = ""

        Me.HProjectId.Value = ""
        Me.ViewState(VS_dtAutoSchedule) = Nothing
        Me.Response.Redirect("frmAutoSchedule.aspx?Saved=Yes")

    End Sub

#End Region

#Region "GRID EVENTS "

    Protected Sub gvwAutoSchedule_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwAutoSchedule.RowDataBound
        Dim strClick As String = String.Empty
        Dim activityName As String = e.Row.Cells(GVCell_Activity).Text
        Dim lblLocation As Label = Nothing
        Dim lblResource As Label = Nothing
        Dim lblCanStartAfter As Label

        e.Row.Cells(GVCell_NodeId).Visible = False
        e.Row.Cells(GVCell_cPeriodSpecefic).Visible = False
        e.Row.Cells(GVCell_MileStone).Visible = False

        If e.Row.RowType = DataControlRowType.DataRow Then

            activityName = activityName.Replace("*", "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;")
            e.Row.Cells(GVCell_Activity).Text = activityName

            lblCanStartAfter = CType(e.Row.FindControl("lblCanStartAfter"), Label)
            If Not lblCanStartAfter Is Nothing Then

                If lblCanStartAfter.Text = "0000" Or _
                String.IsNullOrEmpty(lblCanStartAfter.Text) Then

                    e.Row.BackColor = Drawing.ColorTranslator.FromHtml("#FFF1AF")

                End If

            End If

            'CType(e.Row.FindControl("txtStartDate"), TextBox).Enabled = False
            'CType(e.Row.FindControl("txtEndDate"), TextBox).Enabled = False

            strClick = "popUpCalendar(this," + CType(e.Row.FindControl("txtStartDate"), TextBox).ClientID + ",'dd-mmm-yyyy');"
            CType(e.Row.FindControl("imgStartDate"), HtmlImage).Attributes.Add("onclick", strClick)

            CType(e.Row.FindControl("txtStartDate"), TextBox).Attributes.Add("name", "StartDate")
            CType(e.Row.FindControl("txtStartDate"), TextBox).Enabled = False
            strClick = "popUpCalendar(this," + CType(e.Row.FindControl("txtEndDate"), TextBox).ClientID + ",'dd-mmm-yyyy');"
            CType(e.Row.FindControl("imgEndDate"), HtmlImage).Attributes.Add("onclick", strClick)
            CType(e.Row.FindControl("txtEndDate"), TextBox).Enabled = False

            CType(e.Row.FindControl("btnReCalculate"), ImageButton).CommandArgument = e.Row.RowIndex

            lblLocation = CType(e.Row.FindControl("lblLocation"), Label)

            If Not String.IsNullOrEmpty(lblLocation.Text) Then

                If lblLocation.Text.IndexOf(",") >= 0 Then

                    lblLocation.Text = lblLocation.Text.Split(",".ToCharArray)(0)

                End If

            End If

        End If
    End Sub

    Protected Sub gvwAutoSchedule_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvwAutoSchedule.RowCommand

        Dim txtStartDate As TextBox = gvwAutoSchedule.Rows(e.CommandArgument).FindControl("txtStartDate")
        Dim txtEndDate As TextBox = gvwAutoSchedule.Rows(e.CommandArgument).FindControl("txtEndDate")
        Dim endDate As String
        Dim dys As Integer = CType(gvwAutoSchedule.Rows(e.CommandArgument).FindControl("txtCompletionDays"), TextBox).Text

        If txtStartDate.Text.Length = 0 Then
            ObjCommon.ShowAlert("Please Select Start Date", Me.Page)
            Exit Sub
        End If

        If e.CommandName.ToUpper = "RECALCULATE" Then 'This is for recalculating from current row

            txtStartDate = gvwAutoSchedule.Rows(e.CommandArgument).FindControl("txtStartDate")
            txtEndDate = gvwAutoSchedule.Rows(e.CommandArgument).FindControl("txtEndDate")
            dys = CType(gvwAutoSchedule.Rows(e.CommandArgument).FindControl("txtCompletionDays"), TextBox).Text
            endDate = AddDays(txtStartDate.Text, Integer.Parse(dys))
            endDate = CheckSunday(DateTime.Parse(txtStartDate.Text), DateTime.Parse(endDate))
            txtEndDate.Text = endDate

            'STEP 1 Validating Start Date and End Date for current row
            If txtStartDate.Text.Length <= 0 Then

                ObjCommon.ShowAlert("Start Day Cannot Be Blank For " + _
                        gvwAutoSchedule.Rows(e.CommandArgument).Cells(GVCell_Activity).Text.Replace("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", ""), Me)
                Exit Sub

            ElseIf txtEndDate.Text.Length <= 0 Then

                ObjCommon.ShowAlert("End Day Cannot Be Blank For " + _
                        gvwAutoSchedule.Rows(e.CommandArgument).Cells(GVCell_Activity).Text.Replace("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", ""), Me)
                Exit Sub

            ElseIf CheckDayIsSunday(txtStartDate.Text) Then

                ObjCommon.ShowAlert("Start Day Cannot Be Sunday For " + _
                        gvwAutoSchedule.Rows(e.CommandArgument).Cells(GVCell_Activity).Text.Replace("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", ""), Me)
                Exit Sub

            ElseIf CheckDayIsSunday(txtEndDate.Text) Then

                ObjCommon.ShowAlert("End Day Cannot Be Sunday For " + _
                        gvwAutoSchedule.Rows(e.CommandArgument).Cells(GVCell_Activity).Text.Replace("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", ""), Me)
                Exit Sub

            End If


            UpdateDependentActivites(gvwAutoSchedule.Rows(e.CommandArgument).Cells(GVCell_ActivityId).Text.Trim, e.CommandArgument)

        End If

    End Sub

#End Region

#Region "HELPER FUNCTIONS "

    Private Sub ScheduleDates(ByVal rowIndex As Integer)

        Dim gvr As GridViewRow = gvwAutoSchedule.Rows(rowIndex)
        Dim txtStartDate As TextBox = gvr.FindControl("txtStartDate")
        Dim txtEndDate As TextBox = gvr.FindControl("txtEndDate")
        Dim endDate As String = String.Empty
        Dim errString As String = String.Empty
        Dim dys As Integer
        If Not txtStartDate Is Nothing Then

            If txtStartDate.Text.Length < 8 And _
                CType(gvr.FindControl("lblCanStartAfter"), Label).Text = "0000" Then

                errString = "Please Enter Start Date for " + gvr.Cells(GVCell_Activity).Text
                ObjCommon.ShowAlert(errString, Me)
                Exit Sub

            Else

                dys = CType(gvr.FindControl("txtCompletionDays"), TextBox).Text
                txtStartDate.Text = GetStartDate(CType(gvr.FindControl("lblCanStartAfter"), Label).Text.Trim)
                endDate = AddDays(txtStartDate.Text, Integer.Parse(dys))
                endDate = CheckSunday(DateTime.Parse(txtStartDate.Text), DateTime.Parse(endDate))
                txtEndDate.Text = endDate
            End If

        End If

    End Sub

    Private Function GetStartDate(ByVal CanStartAfter As String) As String
        Dim startDate As String = String.Empty
        Dim endDate As String = String.Empty
        Dim txtEndDate As TextBox = Nothing
        Dim txtStartDate As TextBox = Nothing
        Dim arrCanStartAfter() As String
        Dim index As Integer = 0
        Dim dys As Integer
        Dim tmpDate As DateTime
        Try

           
            If String.IsNullOrEmpty(CanStartAfter) Then
                ObjCommon.ShowAlert("Can Start After Cannot Be Blank", Me)
                Return Today.ToString("dd-MMM-yyyy")
            End If

            arrCanStartAfter = CanStartAfter.Split(",")

            For Each strCanStartAfter As String In arrCanStartAfter
                index = 0

                For Each gvr As GridViewRow In gvwAutoSchedule.Rows

                    index = index + 1

                    txtStartDate = CType(gvr.FindControl("txtStartDate"), TextBox)
                    txtEndDate = CType(gvr.FindControl("txtEndDate"), TextBox)

                    If gvr.Cells(GVCell_ActivityId).Text = strCanStartAfter Then

                        If txtStartDate.Text.Trim.Length > 8 Then

                            If txtEndDate.Text.Trim.Length < 8 Then

                                dys = CType(gvr.FindControl("txtCompletionDays"), TextBox).Text
                                endDate = AddDays(txtStartDate.Text, Integer.Parse(dys))
                                endDate = CheckSunday(DateTime.Parse(txtStartDate.Text), DateTime.Parse(endDate))
                                txtEndDate.Text = endDate

                            End If

                            If startDate.Length > 8 Then

                                If DateTime.Parse(startDate) < DateTime.Parse(txtEndDate.Text) Then

                                    startDate = DateTime.Parse(txtEndDate.Text).AddDays(1).ToString("dd-MMM-yyyy")

                                End If

                            Else

                                startDate = DateTime.Parse(txtEndDate.Text).AddDays(1).ToString("dd-MMM-yyyy")

                            End If

                        Else

                            ScheduleDates(gvr.RowIndex)

                        End If

                    End If

                Next gvr

            Next strCanStartAfter

            tmpDate = DateTime.MinValue

            If startDate.ToString.Length > 7 Then

                If DateTime.TryParse(startDate.ToString, tmpDate) Then

                    Return GetCheckedOffDay(startDate).ToString("dd-MMM-yyyy")

                End If

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GetStartDate")
        End Try


    End Function

    Private Function AddDays(ByVal startDate As String, ByVal DaysToAdd As Integer) As String
        Dim endDate As DateTime

        If (String.IsNullOrEmpty(startDate) Or startDate.Length < 8) Then
            endDate = DateTime.Now
        Else
            endDate = DateTime.Parse(startDate)
            endDate = endDate.AddDays(DaysToAdd - 1)
        End If

        Return endDate.ToString("dd-MMM-yyyy")
    End Function

    ''' <summary>
    ''' This Function Checks if there is a sunday between
    ''' StartDate and EndDate, if there is a sunday then EndDate is incremented by 1 Day
    ''' </summary>
    ''' <param name="startDate">Start Date as DateTime</param>
    ''' <param name="endDate">End Date as DateTime</param>
    ''' <returns>Returns EndDate as string in Format "dd-MMM-yyyy"</returns>
    ''' <remarks></remarks>
    Private Function CheckSunday(ByVal startDate As DateTime, ByVal endDate As DateTime) As String

        Dim newEndDate As String = String.Empty
        Dim tempDate As DateTime = startDate
        Dim IsSunday As Boolean = False
        Dim IsHoliday As Boolean = False
        Dim daysCount As Integer = 0

        For index As Integer = 0 To DateDiff(DateInterval.Day, startDate, endDate)


            tempDate = GetCheckedOffDay(tempDate).AddDays(1)


        Next index

        If IsSunday And IsHoliday Then

            Return tempDate.ToString("dd-MMM-yyyy")

        Else

            tempDate = tempDate.AddDays(-1)
            Return tempDate.ToString("dd-MMM-yyyy")

        End If

    End Function
    'Check date is Not Sunday or Holiday

    Private Function CheckDayIsSunday(ByVal startDate As DateTime) As Boolean

        Dim IsSunday As Boolean = False

        If startDate.DayOfWeek = DayOfWeek.Sunday Then
            IsSunday = True
        End If
        Return IsSunday

    End Function

    Private Function ValidateStartDates(ByRef ErrString As String) As Boolean
        Dim IsDatesValid As Boolean = True
        Dim startDate As String = String.Empty

        For Each gvr As GridViewRow In gvwAutoSchedule.Rows

            If CType(gvr.FindControl("lblCanStartAfter"), Label).Text = "0000" Then

                startDate = CType(gvr.FindControl("txtStartDate"), TextBox).Text

                If startDate.Length <= 0 Then

                    ErrString = "Start Day Cannot Be Blank For " + gvr.Cells(GVCell_Activity).Text.Replace("&nbsp;", "")
                    Return False

                ElseIf startDate.Length > 0 And startDate.Length < 8 Then

                    ErrString = "Invalid Start Date"
                    Return False

                ElseIf Me.CheckDayIsSunday(DateTime.Parse(startDate)) Then

                    ErrString = "Start Date Cannot Be Sunday For " + gvr.Cells(GVCell_Activity).Text.Replace("&nbsp;", "")
                    Return False

                End If

            End If

        Next gvr

        Return IsDatesValid
    End Function

    Private Function ValidateParentDate(ByRef ErrorString As String) As Boolean

        ValidateParentDate = True
        Dim rowCount As Integer = 0

        For Each gvr As GridViewRow In gvwAutoSchedule.Rows
            If CType(gvr.FindControl("lblCanStartAfter"), Label).Text = "0000" Then 'If Can Start After is 0000 continue
                Continue For
            Else
                Dim arrCanStartAfter() As String = CType(gvr.FindControl("lblCanStartAfter"), Label).Text.Split(",")
                For Each strCanStartAfter As String In arrCanStartAfter

                    rowCount = 0

                    For Each gvrInner As GridViewRow In gvwAutoSchedule.Rows

                        rowCount = rowCount + 1
                        'Here Check if Can Start After is There in Activity Id
                        If (strCanStartAfter = gvrInner.Cells(GVCell_ActivityId).Text) Then

                            ValidateParentDate = True
                            Exit For

                        ElseIf (Me.CheckValidDate(CType(gvr.FindControl("txtStartDate"), TextBox).Text)) Then

                            ValidateParentDate = True
                            Exit For

                        Else

                            If rowCount = gvwAutoSchedule.Rows.Count Then
                                ValidateParentDate = False
                                ErrorString = "Please Enter Start Date For " & gvr.Cells(GVCell_Activity).Text.Trim.Replace("&nbsp;", "")
                            End If

                        End If

                    Next gvrInner

                    If ValidateParentDate = False Then

                        Exit For

                    End If

                Next strCanStartAfter

            End If

            If ValidateParentDate = False Then

                Return False

            End If

        Next gvr

        Return True

    End Function

    ''' <summary>
    ''' This Sub is used to update activities when,
    ''' User updates automatically scheduled activities
    ''' </summary>
    ''' <param name="ActivityId">ActivityId of Activity that is updated</param>
    ''' <param name="rowIndex">GridView row index in which StartDate or End Date is Updated</param>
    ''' <remarks></remarks>
    Private Sub UpdateDependentActivites(ByVal ActivityId As String, ByVal rowIndex As Integer)

        Dim lblCanStartAfter As Label = Nothing
        Dim txtStartDate As TextBox = Nothing
        Dim txtEndDate As TextBox = Nothing
        Dim endDate As String = String.Empty
        Dim startDate As String = String.Empty
        Dim arrCanStartAfter() As String
        Dim dys As Integer
        For Each gvr As GridViewRow In gvwAutoSchedule.Rows

            lblCanStartAfter = CType(gvr.FindControl("lblCanStartAfter"), Label)
            If lblCanStartAfter.Text.Contains(ActivityId) Then
                txtStartDate = gvr.FindControl("txtStartDate")
                txtEndDate = gvr.FindControl("txtEndDate")
                startDate = CType(gvwAutoSchedule.Rows(rowIndex).FindControl("txtEndDate"), TextBox).Text.Trim

                arrCanStartAfter = lblCanStartAfter.Text.Split(",")

                For Each strCanStartAfter As String In arrCanStartAfter

                    For Each innergvr As GridViewRow In gvwAutoSchedule.Rows

                        If strCanStartAfter = innergvr.Cells(GVCell_ActivityId).Text.Trim Then

                            If DateTime.Parse(startDate) < DateTime.Parse(CType(innergvr.FindControl("txtEndDate"), TextBox).Text) Then

                                startDate = CType(innergvr.FindControl("txtEndDate"), TextBox).Text.Trim

                            End If

                        End If

                    Next innergvr

                Next strCanStartAfter

                txtStartDate.Text = GetCheckedOffDay(DateTime.Parse(startDate).AddDays(1)).ToString("dd-MMM-yyyy")

                dys = CType(gvr.FindControl("txtCompletionDays"), TextBox).Text
                endDate = AddDays(txtStartDate.Text, Integer.Parse(dys))
                endDate = CheckSunday(DateTime.Parse(txtStartDate.Text), DateTime.Parse(endDate))
                txtEndDate.Text = endDate
                Me.UpdateDependentActivites(gvr.Cells(GVCell_ActivityId).Text, gvr.RowIndex)

            End If

        Next gvr

    End Sub

    'This Function CHecks for Continues Holidays or Sunday and finally return Next Date
    Private Function GetCheckedOffDay(ByVal chdate As DateTime) As DateTime

        If CheckDayIsHoliday(chdate) Then

            Return GetCheckedOffDay(chdate.AddDays(1))

        End If

        If CheckDayIsSunday(chdate) Then

            Return GetCheckedOffDay(chdate.AddDays(1))

        End If

        Return chdate
    End Function

    Private Function GetStartDateAndEndDate(ByRef startDate As DateTime, ByRef endDate As DateTime) As Boolean
        Dim txtStartDate As TextBox = Nothing
        Dim txtEndDate As TextBox = Nothing
        Try



            For Each gvr As GridViewRow In gvwAutoSchedule.Rows

                txtStartDate = gvr.FindControl("txtStartDate")
                txtEndDate = gvr.FindControl("txtEndDate")

                If DateTime.Parse(txtStartDate.Text) < startDate Then

                    startDate = DateTime.Parse(txtStartDate.Text)

                End If

                If DateTime.Parse(txtEndDate.Text) < startDate Then

                    startDate = DateTime.Parse(txtEndDate.Text)

                End If

                If DateTime.Parse(txtStartDate.Text) > endDate Then

                    endDate = DateTime.Parse(txtStartDate.Text)

                End If

                If DateTime.Parse(txtEndDate.Text) > endDate Then

                    endDate = DateTime.Parse(txtEndDate.Text)

                End If
            Next gvr

            Return True
        Catch ex As Exception
            Return False
            ShowErrorMessage(ex.Message, ".......GetStartDateAndEndDate")
        End Try
    End Function

    Private Function CheckDayIsHoliday(ByVal checkDate As DateTime) As Boolean
        Dim dsHoliday As New DataSet
        Dim whereCondition As String = "CONVERT(datetime,CONVERT(varchar,dHolidayDate,106)) = '" + checkDate.ToString("dd-MMM-yyyy") + "' "

        whereCondition += " AND vLocationCode='" + Me.ddlLocation.SelectedValue.Trim + "'"

        If Not objHelp.GetViewHolidayMst(whereCondition, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsHoliday, eStr_Retu) Then

            ObjCommon.ShowAlert(eStr_Retu, Me)
            Return False

        End If

        If dsHoliday.Tables(0).Rows.Count > 0 Then

            Return True

        End If

    End Function

    Private Function CheckValidDate(ByVal dt As String)
        Dim tempDT As Date = DateTime.MinValue

        If Not DateTime.TryParse(dt, tempDT) Then

            Return False

        End If

        Return True
    End Function

#End Region

#Region "AssignValues "

    Private Function AssignValues() As Boolean
        Dim dtWorkspaceNodeAttrHistory As New DataTable
        Dim dr As DataRow
        Dim resourceId As String = String.Empty
        Try
            dtWorkspaceNodeAttrHistory = Me.ViewState(VS_dtAutoSchedule)

            If gvwAutoSchedule.Rows.Count <= 0 Then

                ObjCommon.ShowAlert("No Scheduling Data to Save", Me)
                Exit Function

            End If

            For Each gvr As GridViewRow In gvwAutoSchedule.Rows

                dr = dtWorkspaceNodeAttrHistory.NewRow

                dr("vWorkspaceId") = Me.HProjectId.Value.Trim
                dr("iNodeId") = gvr.Cells(GVCell_NodeId).Text.Trim
                dr("iTranNo") = 0
                dr("iAttrId") = "1"
                dr("vAttrValue") = CType(gvr.FindControl("txtStartDate"), TextBox).Text.Trim
                dr("iStageId") = GeneralModule.Stage_Created
                dr("vRemark") = "Demo"
                dr("iModifyBy") = Session(S_UserID)
                dtWorkspaceNodeAttrHistory.Rows.Add(dr)

                dr = dtWorkspaceNodeAttrHistory.NewRow
                dr("vWorkSpaceId") = Me.HProjectId.Value.Trim
                dr("iNodeId") = gvr.Cells(GVCell_NodeId).Text.Trim
                dr("iTranNo") = 0
                dr("iAttrId") = "2"
                dr("vAttrValue") = CType(gvr.FindControl("txtEndDate"), TextBox).Text.Trim
                dr("iStageId") = GeneralModule.Stage_Created
                dr("vRemark") = "Demo"
                dr("iModifyBy") = Session(S_UserID)
                dtWorkspaceNodeAttrHistory.Rows.Add(dr)


                dr = dtWorkspaceNodeAttrHistory.NewRow
                resourceId = CType(gvr.FindControl("HResourceId"), HiddenField).Value.Trim
                dr("vWorkSpaceId") = Me.HProjectId.Value.Trim
                dr("iNodeId") = gvr.Cells(GVCell_NodeId).Text.Trim
                dr("iTranNo") = 0
                dr("iAttrId") = "5"

                If Not String.IsNullOrEmpty(resourceId) Then
                    dr("vAttrValue") = resourceId
                Else
                    dr("vAttrValue") = "0"
                End If

                dr("iStageId") = GeneralModule.Stage_Created
                dr("vRemark") = "Demo"
                dr("iModifyBy") = Session(S_UserID)
                dtWorkspaceNodeAttrHistory.Rows.Add(dr)

                dtWorkspaceNodeAttrHistory.AcceptChanges()

            Next gvr

            Me.ViewState(VS_dtAutoSchedule) = dtWorkspaceNodeAttrHistory

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....AssignValues")

            Return False
        End Try

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


End Class
