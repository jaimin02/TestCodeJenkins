
Partial Class frmSlotDateandLocation
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION"

    Dim objCommon As New clsCommon
    Dim objHelpDBTable As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

    Dim eStr_Retu As String

    Dim workspaceId As String = String.Empty
    Dim nodeId As String = String.Empty

    Private Const VS_QSLocation As String = "QueryStringLocation"


#End Region

#Region "Load Event"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Page.Title = "::  Slot Date & Location  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
        CType(Me.Master.FindControl("lblMandatory"), Label).Text = ""
        CType(Me.Master.FindControl("lblHeading"), Label).Text = "Slot Date and Location"

        If Not Page.IsPostBack Then
            If Not String.IsNullOrEmpty(Me.Request.QueryString("workspaceid")) Then
                ViewState("workspaceId") = Me.Request.QueryString("workspaceid")
            End If

            If Not String.IsNullOrEmpty(Me.Request.QueryString("nodeId")) Then
                ViewState("nodeId") = Me.Request.QueryString("nodeId")
            End If

            FillDropDown()
            If Not String.IsNullOrEmpty(Me.Request.QueryString("Act")) Then
                lblActivity.Text = Me.Request.QueryString("Act").Trim
            End If

            If Not IsNothing(Me.Request.QueryString("Res")) AndAlso _
                Not String.IsNullOrEmpty(Me.Request.QueryString("Res")) Then

                Me.ddlResource.SelectedValue = Me.Request.QueryString("Res").Trim
            End If

            If Not IsNothing(Me.Request.QueryString("Loc")) AndAlso _
                Not String.IsNullOrEmpty(Me.Request.QueryString("Loc")) Then

                Me.ddlLocation.SelectedValue = Me.Request.QueryString("Loc").Trim

            End If


            'Changed the below If conditions on 06-Mar-2009  -----By Chandresh Vanker

            If Not String.IsNullOrEmpty(Me.Request.QueryString("Start").Trim) Then
                Me.txtSchStartDt.Text = DateTime.Parse(Me.Request.QueryString("Start").Trim).ToString("dd-MMM-yyyy")
            End If

            If Not String.IsNullOrEmpty(Me.Request.QueryString("End").Trim) Then
                Me.txtSchEndDt.Text = DateTime.Parse(Me.Request.QueryString("End").Trim).ToString("dd-MMM-yyyy")
            End If

            If (String.IsNullOrEmpty(Me.Request.QueryString("Start").Trim) And String.IsNullOrEmpty(Me.Request.QueryString("End").Trim)) Then
                btnViewReport_Click(sender, e)
            End If

            If Not Me.Request.QueryString("mode") Is Nothing Then
                If Not (String.IsNullOrEmpty(Me.Request.QueryString("Start").Trim) And _
                    String.IsNullOrEmpty(Me.Request.QueryString("End").Trim)) And _
                     Me.Request.QueryString("mode").Trim.ToUpper = "VIEW" Then
                    Me.ViewState(VS_QSLocation) = Request.QueryString("LOC").ToString.Trim
                    Me.txtSchStartDt.Text = DateTime.Parse(Me.Request.QueryString("Start").Trim).ToString("dd-MMM-yyyy")
                    Me.txtSchEndDt.Text = DateTime.Parse(Me.Request.QueryString("End").Trim).ToString("dd-MMM-yyyy")
                    tblTopTable.Visible = False
                    CType(Me.Master.FindControl("Menu1"), Menu).Visible = False
                    Me.btnClose.Visible = True
                    btnViewReport_Click(sender, e)
                End If
            End If
        End If
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "getData", "getData();", True)
    End Sub
#End Region

#Region "FillDropDown"
    Private Sub FillDropDown()
        Dim ds_DropDown As DataSet = Nothing
        Dim Wstr As String = String.Empty
        'Fill Location Drop Down
        ds_DropDown = New DataSet
        If objHelpDBTable.FillDropDown("LocationMst", "vLocationCode", "vLocationName", "", ds_DropDown, eStr_Retu) Then
            ddlLocation.DataSource = ds_DropDown.Tables(0)
            ddlLocation.DataValueField = "vLocationCode"
            ddlLocation.DataTextField = "vLocationName"
            ddlLocation.Items.Insert(0, New ListItem("--- Select Location ---", "0"))
            ddlLocation.DataBind()
            ddlLocation.Items.Insert(0, New ListItem("--- Select Location ---", "0"))
        Else
            objCommon.ShowAlert(eStr_Retu, Me)
        End If

        'Fill Resource Drop Down
        ds_DropDown = New DataSet

        If ddlLocation.SelectedIndex >= 0 Then
            Wstr = " vLocationCode = " & ddlLocation.SelectedValue.Trim
        Else
            Wstr = ""
        End If

        If objHelpDBTable.FillDropDown("ResourceMst", "vResourceCode", "vResourceName", Wstr, ds_DropDown, eStr_Retu) Then
            ddlResource.DataSource = ds_DropDown.Tables(0)

            ddlResource.DataValueField = "vResourceCode"
            ddlResource.DataTextField = "vResourceName"
            ddlResource.Items.Insert(0, New ListItem(" --- Select Resource ---", "0"))
            ddlResource.DataBind()
            ddlResource.Items.Insert(0, New ListItem(" --- Select Resource ---", "0"))
        Else
            objCommon.ShowAlert(eStr_Retu, Me)
        End If


    End Sub
#End Region

#Region "Drop Down Events"

    Protected Sub ddlLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlLocation.SelectedIndexChanged
        Dim ds_DropDown As New DataSet

        If ddlLocation.SelectedIndex >= 0 Then
            If objHelpDBTable.FillDropDown("ResourceMst", "vResourceCode", "vResourceName", " vLocationCode = " & ddlLocation.SelectedValue.Trim, ds_DropDown, eStr_Retu) Then
                ddlResource.DataSource = ds_DropDown.Tables(0)
                ddlResource.DataValueField = "vResourceCode"
                ddlResource.DataTextField = "vResourceName"
                ddlResource.DataBind()
            Else
                objCommon.ShowAlert(eStr_Retu, Me)
            End If
        End If

    End Sub

#End Region

#Region "BUTTON EVENTS"
    Protected Sub btnViewReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnViewReport.Click
        GetSlottingResult()
        Reset()
    End Sub

    Protected Sub BtnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnBack.Click
        Dim DMS As String = String.Empty
        DMS = IIf(Convert.ToString(Me.Request.QueryString("DMS")) Is Nothing, "", Convert.ToString(Me.Request.QueryString("DMS")))

        Me.Response.Redirect(Me.Request.QueryString("Page2").Trim() & ".aspx?WorkSpaceId=" & Me.Request.QueryString("WorkSpaceId").Trim() & _
                            "&Page=" & Me.Request.QueryString("Page").Trim() & "&Type=" & Me.Request.QueryString("Type") & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))

    End Sub
#End Region

#Region "For Slotting"
    Private Sub GetSlottingResult()
        Dim month As String = String.Empty
        Dim year As String = String.Empty
        Dim locationCode As String = ddlLocation.SelectedValue.Trim
        Dim resourceCode As String = ddlResource.SelectedValue.Trim
        Dim firstDate As String = String.Empty
        Dim lastDate As String = String.Empty
        Dim d As DateTime
        Dim WorkSpaceId As String = String.Empty

        If (Request.QueryString("WorkSpaceId") <> "") Then
            WorkSpaceId = Request.QueryString("WorkSpaceId")
        End If

        Dim dtStart As Date
        Dim dtEnd As Date

        Dim dv_SlotData As New DataView

        If DateTime.TryParse(txtSchStartDt.Text.Trim, dtStart) And _
               DateTime.TryParse(txtSchEndDt.Text.Trim, dtEnd) Then
            month = Convert.ToDateTime(txtSchStartDt.Text.Trim).Month
            year = Convert.ToDateTime(txtSchStartDt.Text.Trim).Year
            firstDate = dtStart.ToString("dd-MMM-yyyy")
            lastDate = dtEnd.ToString("dd-MMM-yyyy")
        Else
            month = DateTime.Now.Month
            year = DateTime.Now.Year
            firstDate = year & "-" & month & "-" & "01"
            firstDate = DateTime.Parse(firstDate).ToString("dd-MMM-yyyy")
            lastDate = year & "-" & month & "-" & Date.DaysInMonth(year, month)
            lastDate = DateTime.Parse(lastDate).ToString("dd-MMM-yyyy")
        End If



        d = DateTime.Parse(year & "-" & month & "-" & "01")
        cldSlotData.VisibleDate = d


        Dim ds_SlotData As New DataSet

        If objHelpDBTable.GetSlotCalendar(locationCode, resourceCode, firstDate, lastDate, WorkSpaceId, ds_SlotData, eStr_Retu) Then

            dv_SlotData = ds_SlotData.Tables(0).DefaultView()
            dv_SlotData.RowFilter = "nMilestone in (" & GeneralModule.MileStone_Monitoring.ToString.Trim() & "," & _
                           GeneralModule.MileStone_Monitoring_Scheduling.ToString.Trim() & ")"

            ViewState("dtSlotCalendar") = dv_SlotData.ToTable()
        Else
            objCommon.ShowAlert(eStr_Retu, Me)
        End If
    End Sub

    Protected Sub cldSlotData_DayRender(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DayRenderEventArgs) Handles cldSlotData.DayRender
        Dim strHoliday As String = String.Empty
        If e.Day.IsOtherMonth Then
            e.Day.IsSelectable = False
            e.Cell.Enabled = False
        End If



        e.Cell.Height = 100
        e.Cell.Width = 175
        e.Cell.Text = ""
        e.Cell.BorderStyle = BorderStyle.Solid
        e.Cell.BorderWidth = 1
        'e.Cell.CssClass = "calendarCell"
        e.Cell.HorizontalAlign = HorizontalAlign.Center
        e.Cell.VerticalAlign = VerticalAlign.Middle
        e.Cell.Text = e.Day.Date.Day.ToString
        e.Cell.Font.Size = 7


        Dim ltrDate As New Literal
        ltrDate.Text = "<table width='100%' style='padding:2px;text-align:center;'><tr><td style='width:100%;' align='center' nowrap='nowrap'>" & e.Day.Date.Day.ToString & "</td></tr>" & _
                                    "</table>"
        If e.Day.Date.DayOfWeek = DayOfWeek.Sunday Then
            ltrDate.Text = "<table width='100%' style='color:red;font-family:verdana;font-size:10pt;padding:2px;text-align:center;'><tr><td style='width:100%;' align='center' nowrap='nowrap'>" & e.Day.Date.Day.ToString & "</td></tr>" & _
                                    "</table>"
            e.Cell.Controls.Add(ltrDate)
            Exit Sub
        End If

        If Not Me.ViewState(VS_QSLocation) Is Nothing Then
            strHoliday = GetDayIsHoliday(e.Day.Date, Me.ViewState(VS_QSLocation).ToString.Trim)
        Else
            strHoliday = GetDayIsHoliday(e.Day.Date, Me.ddlLocation.SelectedValue.Trim)
        End If

        If Not String.IsNullOrEmpty(strHoliday) Then
            ltrDate.Text = "<table width='100%' style='color:red;font-family:verdana;font-size:10pt;padding:2px;text-align:center;'><tr><td style='width:100%;' align='center' nowrap='nowrap'>" & _
            e.Day.Date.Day.ToString & " <br/> " & strHoliday & "</td></tr>" & _
                                    "</table>"
            e.Cell.Controls.Add(ltrDate)
            Exit Sub
        End If

        e.Cell.Controls.Add(ltrDate)

        If Not ViewState("dtSlotCalendar") Is Nothing Then
            Dim index As Integer = 0
            Dim dtbl As DataTable = CType(ViewState("dtSlotCalendar"), DataTable)
            Dim strFilter As String = ""
            Dim strDiv As String = ""

            If dtbl.Rows.Count > 0 Then
                strFilter = "vschedulestartdate <= '" & e.Day.Date.ToString("dd-MMM-yyyy") & "'"
                strFilter += " And vscheduleenddate >= '" & e.Day.Date.ToString("dd-MMM-yyyy") & "'"
                Dim drSlotting() As DataRow = Nothing
                drSlotting = dtbl.Select(strFilter)

                For index = 0 To drSlotting.Length - 1
                    'strDiv = "<div id='" + drSlotting(index)("vWorkSpaceId").ToString + "' style=""display:none;text-align:left;padding:6px;"" class=""DIVSTYLE2"" onblur=""this.style.display='none';"" >" & _
                    strDiv = "<div id='" + drSlotting(index)("vWorkSpaceId").ToString + drSlotting(index)("vActivityId").ToString + "' style=""display:none;text-align:left;padding:6px;"" class=""DIVSTYLE2"" onblur=""this.style.display='none';"" >" & _
                    "<table cellspacing=""3px"" cellpadding=""6px""><tr><td>Project No.</td><td>" & drSlotting(index)("vProjectNo").ToString & "</td></tr>" & _
                     "<tr><td>Department</td><td>" & drSlotting(index)("vDeptName").ToString & "</td></tr>" & _
                     "<tr><td>Client Name</td><td>" & drSlotting(index)("vClientName").ToString & "</td></tr>" & _
                     "<tr><td>Drug</td><td>" & drSlotting(index)("vDrugName").ToString & "</td></tr>" & _
                     "<tr><td>Activity</td><td>" & drSlotting(index)("vActivityName").ToString & "</td></tr>" & _
                     "<tr><td>No. of Subjects</td><td>" & drSlotting(index)("iNoOfSubjects").ToString & "</td></tr>" & _
                     "<tr><td>" & drSlotting(index)("vUOM").ToString & " </td><td>" & drSlotting(index)("nResourceCapacity").ToString & "</td></tr>" & _
                     "</table></div>"


                    'If DateTime.Parse(dtbl.Rows(index)("vschedulestartdate")).ToString("dd-MMMM-yyyy").Equals(e.Day.Date.ToString("dd-MMMM-yyyy")) Then

                    If DateTime.Parse(drSlotting(index)("vschedulestartdate")).ToString("dd-MMMM-yyyy") <= e.Day.Date.ToString("dd-MMMM-yyyy") And _
                    DateTime.Parse(drSlotting(index)("vscheduleenddate")).ToString("dd-MMMM-yyyy") >= e.Day.Date.ToString("dd-MMMM-yyyy") Then
                        If True Then
                            Dim literal As New Literal
                            Dim lnkBtn As New LinkButton
                            Dim divLiteral As New Literal
                            Dim linkText As String = ""

                            linkText = drSlotting(index)("vWorkSpaceId").ToString
                            If Not drSlotting(index)("vProjectNo") Is Nothing Then
                                If Not String.IsNullOrEmpty(drSlotting(index)("vProjectNo").ToString) Then
                                    linkText = drSlotting(index)("vProjectNo").ToString
                                End If
                            End If


                            lnkBtn.Text = drSlotting(index)("vActivityName").ToString & " ( " & drSlotting(index)("vProjectNo").ToString & " )"
                            lnkBtn.Attributes.Add("font-size", "7pt")
                            lnkBtn.Attributes.Add("href", "")

                            lnkBtn.OnClientClick = " return ShowDiv(event,'" + drSlotting(index)("vWorkSpaceId").ToString + drSlotting(index)("vActivityId").ToString + "');"

                            If drSlotting(index)("cCompletedFlag").ToString.ToUpper = "C" Then
                                lnkBtn.Attributes.Add("style", "color:#063")
                            Else
                                lnkBtn.Attributes.Add("style", "color:#C30")
                            End If
                            If e.Cell.Enabled Then
                                divLiteral.Text = "<div style='height:5px;'>&nbsp;</div>"
                                e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#EEEEEE")
                                literal.Text = strDiv
                                e.Cell.Controls.Add(literal)
                                e.Cell.Controls.Add(lnkBtn)
                                e.Cell.Controls.Add(divLiteral)
                            End If


                        End If
                    End If
                Next

            End If
        End If


    End Sub

    Protected Sub cldSlotData_VisibleMonthChanged(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MonthChangedEventArgs) Handles cldSlotData.VisibleMonthChanged

        Dim locationCode As String = ddlLocation.SelectedValue.Trim
        Dim resourceCode As String = ddlResource.SelectedValue.Trim
        Dim firstDate As String = String.Empty
        Dim lastDate As String = String.Empty
        Dim WorkSpaceId As String = String.Empty
        Dim newDate As DateTime = e.NewDate
        Dim ds_SlotData As New DataSet
        Dim dv_SlotData As New DataView
        If String.IsNullOrEmpty(txtSchStartDt.Text) Or _
            String.IsNullOrEmpty(txtSchEndDt.Text) Then
            Exit Sub
        End If

        firstDate = DateTime.Parse(txtSchStartDt.Text).ToString("dd-MMM-yyyy")
        lastDate = DateTime.Parse(txtSchEndDt.Text).ToString("dd-MMM-yyyy")

        If (Request.QueryString("workspaceid") <> "") Then
            workspaceId = Request.QueryString("workspaceid").ToString.Trim()
        End If
        If newDate >= DateTime.Parse(txtSchStartDt.Text) And _
            newDate <= DateTime.Parse(txtSchEndDt.Text) Then

            If objHelpDBTable.GetSlotCalendar(locationCode, resourceCode, firstDate, lastDate, workspaceId, ds_SlotData, eStr_Retu) Then
                dv_SlotData = ds_SlotData.Tables(0).DefaultView()
                dv_SlotData.RowFilter = "nMilestone in (" & GeneralModule.MileStone_Monitoring.ToString.Trim() & "," & _
                               GeneralModule.MileStone_Monitoring_Scheduling.ToString.Trim() & ")"

                ViewState("dtSlotCalendar") = dv_SlotData.ToTable()
            Else
                objCommon.ShowAlert(eStr_Retu, Me)
            End If
        End If
    End Sub


#End Region

#Region "SAVE"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim ds_WorkspaceNodeAttrHistory As New DataSet
        Dim ds_EndDate As New DataSet
        Dim dt_workspaceNodeAttrHistory As New DataTable
        Dim endDate As Date = Nothing
        Dim dt As New DataTable

        'Get date value to compare with Schedule Start Date
        Try

            If objHelpDBTable.GetProcedure_ReturnValue("Proc_ValidationCanStartAfterActivity", ViewState("workspaceId"), ViewState("nodeId"), ds_EndDate, eStr_Retu) Then
                dt = ds_EndDate.Tables(0)
                Dim objEndDate As Object
                objEndDate = dt.Compute("Max(EndDate)", "")
                'endDate = DateTime.Parse(ds_EndDate.Tables(0).Rows(0)("EndDate").ToString)
                'Check if any of Start Date or End date in Null
                For Each dr As DataRow In dt.Rows
                    If Date.Parse(dr("EndDate")) < DateTime.Parse("01-01-1900") Then
                        objCommon.ShowAlert("Please Enter Scheduled Start Date and Scheduled End Date ", Me)
                        Exit Sub
                    End If
                Next

                'Check max endDate is smaller than current Date
                If Not String.IsNullOrEmpty(objEndDate.ToString) Then
                    If Date.Parse(objEndDate.ToString) > DateTime.Parse(Me.txtSchStartDt.Text.Trim) Then
                        objCommon.ShowAlert("Invalid Schedule Start Date", Me)
                        Exit Sub
                    End If
                End If
            Else

            End If

            If objHelpDBTable.getWorkspaceNodeAttrHistory("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_WorkspaceNodeAttrHistory, eStr_Retu) Then
                dt_workspaceNodeAttrHistory = ds_WorkspaceNodeAttrHistory.Tables(0)
                ViewState("dtWorkspaceNodeAttrHistory") = dt_workspaceNodeAttrHistory
                ds_WorkspaceNodeAttrHistory.Tables.Clear()
                ds_WorkspaceNodeAttrHistory.Dispose()
                AssignValues()
            Else
                objCommon.ShowAlert(eStr_Retu, Me)
                Exit Sub
            End If

            ds_WorkspaceNodeAttrHistory = New DataSet
            ds_WorkspaceNodeAttrHistory.Tables.Add(ViewState("dtWorkspaceNodeAttrHistory"))

            If objLambda.Save_InsertDataForWorkspaceNodeAttrHistory(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                                                   ds_WorkspaceNodeAttrHistory, Session(S_UserID), _
                                                   eStr_Retu) Then
                objCommon.ShowAlert("Activity slotting details saved successfully", Me)
            Else
                objCommon.ShowAlert(eStr_Retu, Me)
            End If
            Reset()
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".......btnSave_Click")
        End Try


    End Sub

    Private Sub AssignValues()
        Dim dt_workspaceNodeAttrHistory As DataTable
        Dim dr As DataRow
        dt_workspaceNodeAttrHistory = ViewState("dtWorkspaceNodeAttrHistory")

        Try

            dr = dt_workspaceNodeAttrHistory.NewRow
            dr("vWorkSpaceId") = ViewState("workspaceId")
            dr("iNodeId") = ViewState("nodeId")
            dr("iTranNo") = 0
            dr("iAttrId") = "1"
            dr("vAttrValue") = Me.txtSchStartDt.Text.Trim
            dr("iStageId") = GeneralModule.Stage_Created
            dr("vRemark") = ""
            dr("iModifyBy") = Session(S_UserID)

            dt_workspaceNodeAttrHistory.Rows.Add(dr)


            dr = dt_workspaceNodeAttrHistory.NewRow
            dr("vWorkSpaceId") = ViewState("workspaceId")
            dr("iNodeId") = ViewState("nodeId")
            dr("iTranNo") = 0
            dr("iAttrId") = "2"
            dr("vAttrValue") = Me.txtSchEndDt.Text.Trim
            dr("iStageId") = GeneralModule.Stage_Created
            dr("vRemark") = ""
            dr("iModifyBy") = Session(S_UserID)
            dt_workspaceNodeAttrHistory.Rows.Add(dr)

            dr = dt_workspaceNodeAttrHistory.NewRow
            dr("vWorkSpaceId") = ViewState("workspaceId")
            dr("iNodeId") = ViewState("nodeId")
            dr("iTranNo") = 0
            dr("iAttrId") = "5"
            dr("vAttrValue") = ddlResource.SelectedValue.Trim
            dr("iStageId") = GeneralModule.Stage_Created
            dr("vRemark") = ""
            dr("iModifyBy") = Session(S_UserID)
            dt_workspaceNodeAttrHistory.Rows.Add(dr)

            ViewState("dtWorkspaceNodeAttrHistory") = dt_workspaceNodeAttrHistory
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "........AssignValues")
        End Try
    End Sub

#End Region

#Region "ERROR HANDLER"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub
#End Region

    Private Function GetDayIsHoliday(ByVal checkDate As DateTime, ByVal locationCode As String) As String

        Dim dsHoliday As New DataSet
        Dim whereCondition As String = "CONVERT(datetime,CONVERT(varchar,dHolidayDate,106)) = '" + checkDate.ToString("dd-MMM-yyyy") + "' "
        whereCondition += " AND vLocationCode='" + locationCode + "'"

        If Not objHelpDBTable.GetFieldsOfTable("View_HolidayMst", " * ", whereCondition, dsHoliday, eStr_Retu) Then
            objCommon.ShowAlert(eStr_Retu, Me)
            Return Nothing
        End If

        If dsHoliday.Tables(0).Rows.Count > 0 Then
            Return dsHoliday.Tables(0).Rows(0)("vHolidayDescription").ToString
        End If

        Return Nothing

    End Function


    Public Sub Reset()
        Me.txtSchStartDt.Text = ""
        Me.txtSchEndDt.Text = ""
        Me.ddlLocation.SelectedIndex = -1
        Me.ddlResource.SelectedIndex = 0
    End Sub

    'Add by shivani pandya for Project lock
#Region "LockImpact"
    <Services.WebMethod()> _
    Public Shared Function LockImpact(ByVal WorkspaceID As String) As String
        Dim ds_Check As DataSet = Nothing
        Dim dtProjectStatus As New DataTable
        Dim ProjectStatus As String = String.Empty
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim iTran As String = String.Empty
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()

        wStr = "vWorkspaceId = '" + WorkspaceID.ToString() + "' And cStatusIndi <> 'D'"

        If Not objHelp.GetCRFLockDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_Check, eStr) Then
            Throw New Exception(eStr)
        End If

        If ds_Check.Tables(0).Rows.Count > 0 Then

            dtProjectStatus = ds_Check.Tables(0)
            iTran = dtProjectStatus.Compute("Max(iTranNo)", String.Empty)
            dtProjectStatus.DefaultView.RowFilter = "iTranNo =" + iTran.ToString()

            If dtProjectStatus.DefaultView.ToTable.Rows.Count > 0 Then
                ProjectStatus = dtProjectStatus.DefaultView.ToTable().Rows(0)("cLockFlag").ToString()
            End If
        Else
            ProjectStatus = "U"
        End If

        Return ProjectStatus
    End Function
#End Region

End Class
