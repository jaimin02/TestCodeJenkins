Imports System.Collections.Generic
Imports System.Text
Partial Class frmDeviationReports
    Inherits System.Web.UI.Page
#Region "VARIABLE DECLARATION "

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private Const VS_DataDefinationReport As String = "VS_Deviationreport"
    Private Const VS_DataDefinationfinal As String = "VS_Deviationreport"

    Private rPage As RepoPage


#End Region

#Region "Page Load "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not GenCallShowUI() Then
                Exit Sub
            End If
            Exit Sub
        End If

        If Me.Session("PlaceSearchOptions") Is Nothing Then
            Me.CreateSearchControls()
        End If
    End Sub
    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Me.Session.Remove("PlaceSearchOptions")
    End Sub

#End Region

#Region "GenCallShowUI "

    Private Function GenCallShowUI() As Boolean

        Try
            Page.Title = " :: Deviation Reports ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Deviation Reports"

            GenCallShowUI = True

            If Not FillActivityGroup() Then
                Exit Function
            End If

        Catch ex As Exception
            GenCallShowUI = False
        End Try

    End Function

#End Region

#Region "Generate Report "

    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        If Not FillGrid() Then
            Exit Sub
        End If
    End Sub

#End Region

#Region "Set Project"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click

        Try
            If Not FillChkLstColumns() Then
                Exit Sub
            End If
            Me.PlaceSearchOptions.Controls.Clear()
            Me.gvwActivityGrid.DataSource = Nothing
            Me.gvwActivityGrid.DataBind()
            Me.btnSearch.Visible = False
            Me.btnExportGrid.Visible = False


        Catch ex As Exception
            Me.ShowErrorMessage("Error While Getting CRF Lock Details ", ex.Message)
        End Try
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

#Region "Fill Grid"
    Private Function FillGrid() As Boolean


        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim Columns As String = String.Empty
        Dim PerameterView As String = String.Empty
        Dim Subjects As String = String.Empty
        Dim ds_grid As New DataSet
        Dim dv_grid As New DataView

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            gvwActivityGrid.DataSource = Nothing

            For index As Integer = 0 To Me.chklstColumns.Items.Count - 1
                If Me.chklstColumns.Items(index).Selected Then
                    If Columns.Trim() <> "" OrElse Columns <> String.Empty Then
                        Columns += ","
                    End If
                    Columns += "[" + Me.chklstColumns.Items(index).Value.Trim() + "]"
                End If
            Next



            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'"
            PerameterView = "view_CRFDataDefinationReport1"

            Me.objHelp.Timeout = 300000
            If Not Me.objHelp.GetFieldsOfTable(PerameterView, Columns, wStr, ds_grid, eStr) Then
                Throw New Exception(eStr)
            End If

            If (ds_grid.Tables(0).Rows.Count < 1) Then

                Me.ObjCommon.ShowAlert("Records not Found", Me.Page)
            End If



            For index As Integer = 0 To ds_grid.Tables(0).Columns.Count - 1
                ds_grid.Tables(0).Columns(index).ColumnName = Replace(ds_grid.Tables(0).Columns(index).ColumnName, " ", "").Trim()
            Next index

            'Dim dt As New String

            If DdlDeviationField1.SelectedIndex > 1 AndAlso DdlDeviationField2.SelectedIndex > 1 Then

                ds_grid.Tables(0).Columns.Add("Deviation", GetType(String))
                ds_grid.Tables(0).Columns.Add("days", GetType(String))
                ds_grid.Tables(0).Columns.Add("Hours", GetType(String))
                ds_grid.Tables(0).Columns.Add("Mintues", GetType(String))

                For Each dr As DataRow In ds_grid.Tables(0).Rows
                    'dr("Deviation") = (If(Not dr(DdlDeviationField2.SelectedItem.Text).ToString = String.Empty, cint(dr(DdlDeviationField2.SelectedItem.Text).ToString), 0)) - (If(Not dr(DdlDeviationField2.SelectedItem.Text).ToString = String.Empty, CInt(dr(DdlDeviationField1.SelectedItem.Text).ToString), 0))
                    ' dr("Deviation") = (If(Not dr(DdlDeviationField2.SelectedItem.Text).ToString = String.Empty, Math.Round(CType(dr(DdlDeviationField2.SelectedItem.Text).ToString, Double), 2), 0)) - (If(Not dr(DdlDeviationField2.SelectedItem.Text).ToString = String.Empty, Math.Round(CType(dr(DdlDeviationField2.SelectedItem.Text).ToString, Double), 2), 0))
                    '  Dim dateDifference As New DateDifference(dr(DdlDeviationField1.SelectedItem.Text).ToString, dr(DdlDeviationField2.SelectedItem.Text).ToString)
                    ' dr("days") = dateDifference.ToStringDays()
                    dr("days")=DateDiff(DateInterval.Day, dr(DdlDeviationField1.SelectedItem.Text), dr(DdlDeviationField2.SelectedItem.Text))
                    '  dr("Hours") = dateDifference.ToStringHours()
                    dr("Hours")=DateDiff(DateInterval.Hour, dr(DdlDeviationField1.SelectedItem.Text), dr(DdlDeviationField2.SelectedItem.Text))
                    'dr("Mintues") = dateDifference.ToStringMintues()
                    dr("Mintues") = DateDiff(DateInterval.Minute, dr(DdlDeviationField1.SelectedItem.Text), dr(DdlDeviationField2.SelectedItem.Text))



                Next
            End If


            Me.ViewState(VS_DataDefinationReport) = ds_grid.Tables(0)


            dv_grid = ds_grid.Tables(0).DefaultView

            gvwActivityGrid.DataSource = Nothing
            gvwActivityGrid.DataBind()


            If Not BindGrid() Then
                Throw New Exception()
            End If


            Me.CreateSearchControls()

            Return True

        Catch ex As Exception
            ShowErrorMessage("Error While Filling Grid. ", ex.Message)
            Return False
        End Try

    End Function
#End Region

#Region "FillChkLstColumns"

    Private Function FillChkLstColumns() As Boolean
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim Pameter As String = String.Empty
        Dim ds_Columns As New DataSet

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Pameter = "view_CRFDataDefinationReport1"
            wStr = " SysColumns.Name not in ('vWorkSpaceId')"



            If Not objHelp.GetColumnNamesWithWhereCondition(Pameter, wStr, ds_Columns, eStr) Then
                Throw New Exception(eStr)
            End If

            For index As Integer = 0 To ds_Columns.Tables(0).Rows.Count - 1
                ds_Columns.Tables(0).Rows(index)(0) = Replace(ds_Columns.Tables(0).Rows(index)(0), " ", "").Trim()
            Next index

            Me.chklstColumns.DataSource = ds_Columns.Tables(0)
            Me.chklstColumns.DataTextField = "ColumnName"
            Me.chklstColumns.DataValueField = "ColumnName"

            Me.chklstColumns.DataBind()

            Me.DdlDeviationField1.DataSource = ds_Columns.Tables(0)
            Me.DdlDeviationField1.DataTextField = "ColumnName"
            Me.DdlDeviationField1.DataValueField = "ColumnName"
            Me.DdlDeviationField1.DataBind()
            Me.DdlDeviationField1.Items.Insert(0, "--Select Deviation Field --")

            Me.DdlDeviationField2.DataSource = ds_Columns.Tables(0)
            Me.DdlDeviationField2.DataTextField = "ColumnName"
            Me.DdlDeviationField2.DataValueField = "ColumnName"
            Me.DdlDeviationField2.DataBind()
            Me.DdlDeviationField2.Items.Insert(0, "--Select Deviation Field --")

            Return True

        Catch ex As Exception
            ShowErrorMessage("Error While Filling Fields. ", ex.Message)
            Return False
        End Try

    End Function

#End Region

#Region "Export To Excel Logic & Events"

    Protected Sub btnExportGrid_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim fileName As String = ""
        Dim ds As New DataSet
        Try

            If Me.gvwActivityGrid.Rows.Count < 1 Then
                Me.ObjCommon.ShowAlert("No data to Export", Me.Page)
                Exit Sub
            End If
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            fileName = "Deviation Reports"
            fileName = fileName & ".xls"
            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel") '"application/TEXT") 
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)

            ds.Tables.Add(CType(Me.ViewState(VS_DataDefinationfinal), DataTable))
            ds.AcceptChanges()

            Context.Response.Write(ConvertDsTO(ds))

            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
        End Try
    End Sub



    Private Function ConvertDsTO(ByVal ds As DataSet) As String
        Dim strMessage As New StringBuilder
        Dim i As Integer
        Dim iCol As Integer
        Dim j As Integer

        Try

            strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")

            strMessage.Append("<tr>")
            strMessage.Append("<td><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append("Project/Site")
            strMessage.Append("</font></strong></td>")
            strMessage.Append("<td colspan=""4""><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append(Me.txtproject.Text.Trim())
            strMessage.Append("</font></strong></td>")
            strMessage.Append("</tr>")



            strMessage.Append("<tr>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")

            For iCol = 0 To ds.Tables(0).Columns.Count - 1
                ' ds.Tables(0).Columns(iCol).ColumnName = Me.gvwActivityGrid.Columns(iCol).HeaderText.Trim()

                'If ds.Tables(0).Columns(iCol).ToString <> "TranNo" Then
                strMessage.Append("<td><strong><font color=""#000099"" size=""3"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(ds.Tables(0).Columns(iCol).ToString)
                strMessage.Append("</font></strong></td>")
                ' End If
            Next
            strMessage.Append("</tr>")

            strMessage.Append("<tr>")
            strMessage.Append("</tr>")

            For j = 0 To ds.Tables(0).Rows.Count - 1
                strMessage.Append("<tr>")
                For i = 0 To ds.Tables(0).Columns.Count - 1
                    '  If ds.Tables(0).Columns(i).ToString <> "TranNo" Then
                    If IsDBNull(ds.Tables(0).Rows(j).Item(i)) = False Then
                        If (CType(IIf(IsDBNull(ds.Tables(0).Rows(j).Item(i)) = True, "", ds.Tables(0).Rows(j).Item(i)), String) = "N") Then
                            strMessage.Append("<td><strong><font color=""green"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                        Else
                            strMessage.Append("<td><strong><font color=""#000000"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                        End If
                    Else
                        strMessage.Append("<td><strong><font color=""red"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    End If
                    strMessage.Append(ds.Tables(0).Rows(j).Item(i))
                    strMessage.Append("</font></strong></td>")
                    ' End If
                Next
                strMessage.Append("</tr>")
            Next
            strMessage.Append("</table>")

            Return strMessage.ToString

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
            Return ""
        End Try
    End Function

#End Region

#Region " Searching part"

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not BindGridWithSearch() Then
            Exit Sub

        End If
    End Sub


    Private Sub CreateSearchControls()
        Dim dv_CRFDtl As DataView
        Dim dt_CRFDtl As New DataTable
        Dim eStr As String = String.Empty
        Dim index As Integer = 0
        Dim strHdr As String = String.Empty
        Dim ColumnName As String = String.Empty
        Dim count As Integer = 0
        Dim indexes() As Integer

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Not Me.ViewState(VS_DataDefinationReport) Is Nothing Then
                dt_CRFDtl = CType(Me.ViewState(VS_DataDefinationReport), DataTable)
            Else
                Exit Sub
            End If

            PlaceSearchOptions.Controls.Clear()

            'If Me.rbtnlstGridOptions.Items(0).Selected Then

            '    PlaceSearchOptions.Controls.Add(New LiteralControl("<Table border=""1"" id=""test"" class=""display"" style=""width: 950px"">"))
            '    PlaceSearchOptions.Controls.Add(New LiteralControl("<thead>"))
            '    PlaceSearchOptions.Controls.Add(New LiteralControl("<tr>"))

            '    For index = 0 To Me.chklstColumns.Items.Count - 1

            '        If Me.chklstColumns.Items(index).Selected Then

            '            strHdr = dt_CRFDtl.Columns(Me.chklstColumns.Items(index).Value.Trim.Replace(" ", "")).ColumnName.Trim()
            '            PlaceSearchOptions.Controls.Add(New LiteralControl("<th style=""width: 950px"">" + strHdr + "</th>"))

            '            ReDim Preserve indexes(count)
            '            indexes(count) = index
            '            count += 1

            '        End If

            '    Next

            '    PlaceSearchOptions.Controls.Add(New LiteralControl("</tr>"))
            '    PlaceSearchOptions.Controls.Add(New LiteralControl("</thead>"))
            '    PlaceSearchOptions.Controls.Add(New LiteralControl("<tbody>"))

            '    For index = 0 To dt_CRFDtl.Rows.Count - 1

            '        PlaceSearchOptions.Controls.Add(New LiteralControl("<Tr class=""gradeA"" ALIGN=CENTER>"))

            '        For innerindex As Integer = 0 To indexes.Length - 1

            '            ColumnName = Me.chklstColumns.Items(indexes(innerindex)).Text.Trim.Replace(" ", "")

            '            PlaceSearchOptions.Controls.Add(New LiteralControl("<Td ALIGN=LEFT style=""width: 950px"">" + _
            '                                            IIf(Convert.ToString(dt_CRFDtl.Rows(index)(ColumnName)).Trim() = "", "&nbsp", _
            '                                                dt_CRFDtl.Rows(index)(ColumnName).ToString())))
            '            PlaceSearchOptions.Controls.Add(New LiteralControl("</Td>"))

            '        Next

            '        PlaceSearchOptions.Controls.Add(New LiteralControl("</Tr>"))

            '    Next index

            '    PlaceSearchOptions.Controls.Add(New LiteralControl("</tbody>"))
            '    PlaceSearchOptions.Controls.Add(New LiteralControl("</Table>"))

            'Else

            '**************For dynamic drop down filters***************

            PlaceSearchOptions.Controls.Add(New LiteralControl("<Table border=""1"" style=""width: 950px"">"))
            PlaceSearchOptions.Controls.Add(New LiteralControl("<tr>"))
            PlaceSearchOptions.Controls.Add(New LiteralControl("<td style=""width: 950px"">"))

            For index = 0 To dt_CRFDtl.Columns.Count - 1

                Dim ddl As New DropDownList
                ddl.ID = dt_CRFDtl.Columns(index).ColumnName

                dv_CRFDtl = dt_CRFDtl.DefaultView.ToTable(True, dt_CRFDtl.Columns(index).ColumnName.Split(",")).DefaultView
                ddl.DataSource = dv_CRFDtl.ToTable()
                ddl.DataTextField = dt_CRFDtl.Columns(index).ColumnName
                ddl.DataValueField = dt_CRFDtl.Columns(index).ColumnName
                ddl.DataBind()
                ddl.Items.Insert(0, "Select " + dt_CRFDtl.Columns(index).ColumnName.Trim())
                ddl.SelectedIndex = 0
                ddl.Width = 150

                For cnt As Integer = 0 To ddl.Items.Count - 1
                    ddl.Items(cnt).Attributes.Add("title", ddl.Items(cnt).Text)
                Next cnt

                PlaceSearchOptions.Controls.Add(ddl)
                PlaceSearchOptions.Controls.Add(New LiteralControl("&nbsp"))

            Next index

            PlaceSearchOptions.Controls.Add(New LiteralControl("</td>"))
            PlaceSearchOptions.Controls.Add(New LiteralControl("</tr>"))
            PlaceSearchOptions.Controls.Add(New LiteralControl("</Table>"))

            '*****************************************

            'End If

            Me.Session("PlaceSearchOptions") = PlaceSearchOptions.Controls

        Catch ex As Exception
            ShowErrorMessage("Error While Creating Filters. ", ex.Message)
        End Try
    End Sub


    Private Function BindGrid() As Boolean
        Dim dt As New DataTable

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            gvwActivityGrid.DataSource = Nothing

            dt = CType(Me.ViewState(VS_DataDefinationReport), DataTable)

            gvwActivityGrid.DataSource = dt
            gvwActivityGrid.DataBind()

            Me.btnSearch.Visible = False
            Me.btnExportGrid.Visible = False
            If dt.Rows.Count > 0 Then
                Me.btnSearch.Visible = True
                Me.btnExportGrid.Visible = True
            End If

            Return True

        Catch ex As Exception
            ShowErrorMessage("Error While Binding Grid. ", ex.Message)
            Return False
        End Try

    End Function


    Protected Sub gvwActivityGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvwActivityGrid.PageIndexChanging

        gvwActivityGrid.PageIndex = e.NewPageIndex
        If Not BindGridWithSearch() Then
            Exit Sub
        End If


    End Sub






    Private Function BindGridWithSearch() As Boolean
        Dim objCollection As ControlCollection
        Dim objControl As Control
        Dim ObjId As String = ""
        Dim wStr As String = ""
        Dim dt As New DataTable
        Dim dv As New DataView
        Dim wStrAll As String = ""

        Try

            objCollection = CType(Me.Session("PlaceSearchOptions"), ControlCollection)

            For Each objControl In objCollection

                If objControl.GetType.ToString.Trim() = "System.Web.UI.WebControls.DropDownList" Then

                    ObjId = objControl.ID.ToString.Trim()

                    wStr = CType(objControl, DropDownList).SelectedItem.Text

                    If Not wStr Is Nothing Then

                        If wStr.ToUpper() <> "" And CType(objControl, DropDownList).SelectedIndex <> 0 Then
                            If wStrAll.Trim() <> "" Then
                                wStrAll += " And "
                            End If
                            wStrAll += ObjId.Trim() + " = '" + wStr + "'"
                        End If

                    End If

                End If

            Next objControl

            dt = CType(Me.ViewState(VS_DataDefinationReport), DataTable)
            dv = dt.DefaultView
            dv.RowFilter = wStrAll
            ViewState(VS_DataDefinationfinal) = dt

        Catch ex As Exception
            ShowErrorMessage("Error While Searching Data. ", ex.Message)
        Finally
            'Me.ViewState(VS_DtGrid) = dv.ToTable()
            If Not Me.BindGrid() Then
                Throw New Exception
            End If
            ' Me.CreateSearchControls() 'edited by vishal 26-04-2011
        End Try
    End Function
#End Region

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")

    End Sub

#Region " Fill Activity "
    Private Function FillActivity() As Boolean
        Dim wstr As String = ""
        Dim Ds_FillActivity As New DataSet
        Dim eStr_Retu As String = ""
        Try
            wstr = "vactivitygroupid='" + Me.ddlActivityGroup.SelectedValue.Trim()
            wstr += "'and   cstatusindi<>'D' "

            If Not objHelp.getActivityMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_FillActivity, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If
            If Ds_FillActivity.Tables(0).Rows.Count <= 0 Then
                ObjCommon.ShowAlert("No Record Found", Me)
                Me.ddlActivity.Items.Clear()
                Me.ddlActivity.Items.Insert(0, New ListItem("Select Activity", 0))
                Return True
                Exit Function
            End If

            Me.ddlActivity.DataSource = Ds_FillActivity
            Me.ddlActivity.DataTextField = "vActivityName"
            Me.ddlActivity.DataValueField = "vActivityId"
            Me.ddlActivity.DataBind()

            Me.ddlActivity.Items.Insert(0, New ListItem("Select Activity", 0))
            'SSSS Me.trvwStructure.Nodes.Clear()

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region " Fill Activity Group "
    Private Function FillActivityGroup() As Boolean
        Dim wstr As String = "1=1"
        Dim Ds_FillActivityGroup As New DataSet
        Dim eStr_Retu As String = ""
        Try



            If Not objHelp.getActivityGroupMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, Ds_FillActivityGroup, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If
            If Ds_FillActivityGroup.Tables(0).Rows.Count <= 0 Then
                ObjCommon.ShowAlert("No Record Found", Me)
                Me.ddlActivityGroup.Items.Clear()
                Me.ddlActivityGroup.Items.Insert(0, New ListItem("Select Activity Group", 0))
                Return True
                Exit Function
            End If

            Me.ddlActivityGroup.DataSource = Ds_FillActivityGroup
            Me.ddlActivityGroup.DataTextField = "vActivityGroupName"
            Me.ddlActivityGroup.DataValueField = "vActivityGroupId"
            Me.ddlActivityGroup.DataBind()

            Me.ddlActivityGroup.Items.Insert(0, New ListItem("Select Activity Group", 0))
            ' Me.trvwStructure.Nodes.Clear()

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

    'Public Class DateDifference
    '    ''' <summary>
    '    ''' defining Number of days in month; index 0=> january and 11=> December
    '    ''' february contain either 28 or 29 days, that's why here value is -1
    '    ''' which wil be calculate later.
    '    ''' </summary>
    '    Private monthDay As Integer() = New Integer(11) {31, -1, 31, 30, 31, 30, _
    '     31, 31, 30, 31, 30, 31}

    '    ''' <summary>
    '    ''' contain from date
    '    ''' </summary>
    '    Private fromDate As DateTime

    '    ''' <summary>
    '    ''' contain To Date
    '    ''' </summary>
    '    Private toDate As DateTime

    '    ''' <summary>
    '    ''' this three variable for output representation..
    '    ''' </summary>
    '    Private year As Integer
    '    Private month As Integer
    '    Private day As Integer
    '    Private hour As Integer

    '    Private mintue As Integer


    '    Public Sub New(ByVal d1 As DateTime, ByVal d2 As DateTime)
    '        Dim increment As Integer

    '        'If d1 > d2 Then
    '        '    Me.fromDate = d2
    '        '    Me.toDate = d1
    '        'Else
    '        Me.fromDate = d1
    '        Me.toDate = d2
    '        '          (DATEDIFF(HOUR,IMP.DateIMP,Vital.DateVital),0)),'') as IMP_Lunch_HRs,
    '        '(ISNULL((ISNULL(DATEDIFF(MINUTE,IMP.DateIMP,Vital.DateVital),0)),'')- 240) IMP_Lunch_Minute

    '        'Dim abs As New DateTime
    '        '(DateInterval.Minute, Me.fromDate, Me.toDate)


    '        'End If

    '        ''' 
    '        ''' Day Calculation
    '        ''' 
    '        increment = 0

    '        If Me.fromDate.Day > Me.toDate.Day Then

    '            increment = Me.monthDay(Me.fromDate.Month - 1)
    '        End If
    '        ''' if it is february month
    '        ''' if it's to day is less then from day
    '        If increment = -1 Then
    '            If DateTime.IsLeapYear(Me.fromDate.Year) Then
    '                ' leap year february contain 29 days
    '                increment = 29
    '            Else
    '                increment = 28
    '            End If
    '        End If
    '        If increment <> 0 Then
    '            day = (Me.toDate.Day + increment) - Me.fromDate.Day
    '            increment = 1
    '        Else
    '            day = Me.toDate.Day - Me.fromDate.Day
    '        End If

    '        '''
    '        '''month calculation
    '        '''
    '        If (Me.fromDate.Month + increment) > Me.toDate.Month Then
    '            Me.month = (Me.toDate.Month + 12) - (Me.fromDate.Month + increment)
    '            increment = 1
    '        Else
    '            Me.month = (Me.toDate.Month) - (Me.fromDate.Month + increment)
    '            increment = 0
    '        End If

    '        '''
    '        ''' year calculation
    '        '''

    '        Me.year = Me.toDate.Year - (Me.fromDate.Year + increment)

    '        Me.hour = Me.toDate.Hour - (Me.fromDate.Hour)
    '        Me.mintue = Me.toDate.Minute - (Me.fromDate.Minute)


    '    End Sub

    '    Public Function ToStringDays() As String
    '        'return base.ToString();
    '        'Return Me.year & " Year(s), " & Me.month & " month(s), " & Me.day & " day(s), " & Me.hours & " hours(s), " & Me.second & " mint(s) "

    '        Return Me.day
    '    End Function
    '    Public Function ToStringHours() As String
    '        'return base.ToString();
    '        'Return Me.year & " Year(s), " & Me.month & " month(s), " & Me.day & " day(s), " & Me.hours & " hours(s), " & Me.second & " mint(s) "

    '        Return Me.hour
    '    End Function
    '    Public Function ToStringMintues() As String
    '        'return base.ToString();
    '        'Return Me.year & " Year(s), " & Me.month & " month(s), " & Me.day & " day(s), " & Me.hours & " hours(s), " & Me.second & " mint(s) "

    '        Return Me.mintue
    '    End Function

    '    Public ReadOnly Property Years() As Integer
    '        Get
    '            Return Me.year
    '        End Get
    '    End Property

    '    Public ReadOnly Property Months() As Integer
    '        Get
    '            Return Me.month
    '        End Get
    '    End Property

    '    Public ReadOnly Property Days() As Integer
    '        Get
    '            Return Me.day
    '        End Get
    '    End Property

    '    Public ReadOnly Property hours() As Integer
    '        Get
    '            Return Me.hour

    '        End Get
    '    End Property
    '    Public ReadOnly Property mintues() As Integer
    '        Get
    '            Return Me.mintue

    '        End Get
    '    End Property
    'end class


    Protected Sub ddlActivityGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlActivityGroup.SelectedIndexChanged
        If Not FillActivity() Then
            Exit Sub
        End If
    End Sub

#Region "Fill attributeFromActivity"

    Private Function FillAttribute() As Boolean
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim Pameter As String = String.Empty
        Dim ds_Columns As New DataSet

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            Me.chklstColumns.Items.Clear()

            ' Me.chklstColumns.DataSource = Nothing
            ' Me.chklstColumns.DataBind()

            ' Pameter = "view_CRFDataDefinationReport1"
            wStr = "vActivityId= '" + Me.ddlActivity.SelectedValue.Trim()
            wStr += "' and cstatusindi <> 'D' "




            If Not objHelp.GetMedExTemplateDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Columns, eStr) Then
                Throw New Exception(eStr)
            End If

            For index As Integer = 0 To ds_Columns.Tables(0).Rows.Count - 1
                ds_Columns.Tables(0).Rows(index)(0) = Replace(ds_Columns.Tables(0).Rows(index)(0), " ", "").Trim()
            Next index


            Me.chklstColumns.DataSource = ds_Columns.Tables(0)
            Me.chklstColumns.DataTextField = "vMedexDesc"
            Me.chklstColumns.DataValueField = "vMedexCode"
            Me.chklstColumns.DataBind()

            Me.DdlDeviationField1.DataSource = ds_Columns.Tables(0)
            Me.DdlDeviationField1.DataTextField = "vMedexDesc"
            Me.DdlDeviationField1.DataValueField = "vMedexCode"
            Me.DdlDeviationField1.DataBind()
            Me.DdlDeviationField1.Items.Insert(0, "--Select Deviation Field --")

            Me.DdlDeviationField2.DataSource = ds_Columns.Tables(0)
            Me.DdlDeviationField2.DataTextField = "vMedexDesc"
            Me.DdlDeviationField2.DataValueField = "vMedexCode"
            Me.DdlDeviationField2.DataBind()
            Me.DdlDeviationField2.Items.Insert(0, "--Select Deviation Field --")




            Return True

        Catch ex As Exception
            ShowErrorMessage("Error While Filling Fields. ", ex.Message)
            Return False
        End Try

    End Function

#End Region

    Protected Sub ddlActivity_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlActivity.SelectedIndexChanged
        Me.chklstColumns.Items.Clear()
        If Not FillAttribute() Then
            Exit Sub

        End If


    End Sub

    Protected Sub Btnswap_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btnswap.Click
        Dim Columns As String = String.Empty
        ' For index As Integer = 0 To Me.chklstColumns.Items.Count - 1
        'Me.ChklistForDe.Items.Add(Me.chklstColumns.SelectedItem)
        'Next
        'Me.chklstColumns.Items.Clear()

        For i As Integer = 0 To chklstColumns.Items.Count - 1

            If chklstColumns.Items(i).Selected = True Then
                ChklistForDeviation.Items.Add(chklstColumns.Items(i))
                'Me.ChklistForDe.SelectedValue = False

                '  Dim li As ListItem = chklstColumns.Items(i)
                'chklstColumns.Items.Remove(li)

            End If

        Next

        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Uncheck", "uncheckCheckbox();", True)

    End Sub
End Class
