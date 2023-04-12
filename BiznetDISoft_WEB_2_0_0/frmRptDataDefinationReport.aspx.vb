
Partial Class frmRptDataDefinationReport
    Inherits System.Web.UI.Page
#Region "VARIABLE DECLARATION "

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private Const VS_DataDefinationReport As String = "VS_DataDefinationRepor"
    Private Const VS_DataDefinationfinal As String = "VS_DataDefinationFinal"
    Private Const gvwActivityGrid_SrNo As Integer = 0

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
        Dim sender As New Object
        Dim e As EventArgs
        Try
            Page.Title = ":: Data Definition Report :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Data Definition Report"

            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtproject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(sender, e)
            End If

            Me.AutoCompleteExtender1.ContextKey = "cWorkspaceType = 'P'"
            GenCallShowUI = True
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
            PerameterView = "View_CRFDataDefinationReport"

            Me.objHelp.Timeout = 300000
            If Not Me.objHelp.GetFieldsOfTable(PerameterView, Columns, wStr, ds_grid, eStr) Then
                Throw New Exception(eStr)
            End If

            If (ds_grid.Tables(0).Rows.Count < 1) Then

                Me.ObjCommon.ShowAlert("Records Not Found", Me.Page)
            End If



            For index As Integer = 0 To ds_grid.Tables(0).Columns.Count - 1
                ds_grid.Tables(0).Columns(index).ColumnName = Replace(ds_grid.Tables(0).Columns(index).ColumnName, " ", "").Trim()
            Next index

            Me.ViewState(VS_DataDefinationReport) = ds_grid.Tables(0)

            dv_Grid = ds_grid.Tables(0).DefaultView

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


            Pameter = "view_CRFDataDefinationReport"
            wStr = " SysColumns.Name not in ('vWorkSpaceId')"



            If Not objHelp.GetColumnNamesWithWhereCondition(Pameter, wStr, ds_Columns, eStr) Then
                Throw New Exception(eStr)
            End If

            Me.chklstColumns.DataSource = ds_Columns.Tables(0)
            Me.chklstColumns.DataTextField = "ColumnName"
            Me.chklstColumns.DataValueField = "ColumnName"
            Me.chklstColumns.DataBind()

            Return True

        Catch ex As Exception
            ShowErrorMessage("Error While Filling Fields. ", ex.Message)
            Return False
        End Try

    End Function

#End Region

#Region "Export To Excel Logic & Events"

    Protected Sub btnExportGrid_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim fileName As String = String.Empty
        Dim ds As New DataSet
        Try

            If Me.gvwActivityGrid.Rows.Count < 1 Then
                Me.ObjCommon.ShowAlert("No Data To Export", Me.Page)
                Exit Sub
            End If
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            fileName = "DataDefinitionReport"
            fileName = fileName & ".xls"
            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel") '"application/TEXT") 
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)

            ds.Tables.Add(CType(Me.ViewState(VS_DataDefinationReport), DataTable))
            ds.AcceptChanges()

            Context.Response.Write(ConvertDsTO(ds))

            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...btnExportGrid_Click")
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
            ShowErrorMessage(ex.Message, "...ConvertDsTO")
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
        
        Try
            
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

            PlaceSearchOptions.Controls.Add(New LiteralControl("  </br>  <Table border=""1"" style=""width: 950px;margin-left:255px;"">"))
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


            gvwActivityGrid.DataSource = Nothing

            dt = CType(Me.ViewState(VS_DataDefinationReport), DataTable)

            gvwActivityGrid.DataSource = dt
            gvwActivityGrid.DataBind()

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIGV_RptDefination", "UIGV_RptDefination(); ", True)

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

    Protected Sub gvwActivityGrid_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwActivityGrid.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(gvwActivityGrid_SrNo).Text = e.Row.RowIndex + (Me.gvwActivityGrid.PageSize * Me.gvwActivityGrid.PageIndex) + 1
                e.Row.Cells(gvwActivityGrid_SrNo).HorizontalAlign = HorizontalAlign.Center
            End If
        Catch ex As Exception

        End Try
    End Sub






    Private Function BindGridWithSearch() As Boolean
        Dim objCollection As ControlCollection
        Dim objControl As Control
        Dim ObjId As String = String.Empty
        Dim wStr As String = String.Empty
        Dim dt As New DataTable
        Dim dv As New DataView
        Dim wStrAll As String = String.Empty

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
                            wStrAll += "[" + ObjId.Trim() + "] = '" + wStr + "'"
                        End If

                    End If

                End If

            Next objControl

            dt = CType(Me.ViewState(VS_DataDefinationReport), DataTable)
            dv = dt.DefaultView
            dv.RowFilter = wStrAll
            dt = CType(dv.ToTable, DataTable)


            ViewState(VS_DataDefinationReport) = dt

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
End Class

