
Partial Class frmAttributeSearch
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()


    Private Const VS_WorkspaceId As String = "workspaceId"
    Private Const VS_MyProject As String = "Myproject"
    Private Const VS_Choice As String = "Choice"
    Private Const VS_WsMedExNodeDtl As String = "Myproject"

    '' added by vikas shah
    ' added on: 15th feb 2012
    'description:0010 = DMS this hardcode variable is declared as told by deepak bhai to fill only DMS projects
    ' value of this variable might be changed when it will be released at client side.
    Private Const VS_ProjectTypeCode As String = "0010"
    Private MedExCode As New ArrayList
    Private Const QuatationWId As String = "0000002892"
    Private Const SynopsisWId As String = "0000002897"
    Private Const VS_user As String = "synopsis"
    Private Const VS_temp As String = "Quatation"
    Dim j As Integer = 0
    Dim k As Integer = 0
    Dim dt_transpose As New DataTable
    Dim dt As DataTable = New DataTable
    Dim ds_ForSynopsis As New DataSet


#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Dim estr As String = Nothing
        Dim ds As New DataSet
        Dim FromDate As String = String.Empty
        Dim ToDate As String = String.Empty
        Try
            If Not IsPostBack Then
                CType(Me.Master.FindControl("lblHeading"), Label).Text = "Project Submission"
                If Not GenCall_ShowUI() Then
                    Exit Sub
                End If

                FillDocType()
                FillClient()
                FillMolecule()
                FillProjectType()
                FillLocation()

                Me.ddlReportFor.Items.Clear()
                'Me.ddlReportFor.Items.Add("-- Select --")
                Me.ddlReportFor.Items.Add("Last Month")
                Me.ddlReportFor.Items.Add("Current Month")
                Me.ddlReportFor.Items.Add("Last Quater")
                Me.ddlReportFor.Items.Add("Current Calendar Year")
                Me.ddlReportFor.Items.Add("Last Calendar Year")
                Me.ddlReportFor.Items.Add("Current Financial Year")
                Me.ddlReportFor.Items.Add("Last Financial Year")

                ddlReportFor.SelectedValue = "Current Calendar Year"
                FromDate = "01" & "-" & "Jan" & "-" & DateTime.Now.Year.ToString()
                'LastdayOfPpreviousMonth = DateTime.DaysInMonth(DateTime.Now.AddMonths(-1).Year.ToString(), DateTime.Now.AddMonths(-1).Month)
                ToDate = "31" & "-" & "Dec" & "-" & DateTime.Now.Year.ToString()
                txtFromDate.Text = FromDate
                txtToDate.Text = ToDate

            End If



        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

#End Region

#Region "GenCall"
    Private Function GenCall() As Boolean
        Dim ds As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Choice = Me.Request.QueryString("Mode")
            Me.ViewState(VS_Choice) = Choice   'To use it while saving
            'Me.Session(S_ScopeNo)

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                'Me.ViewState(VS_WorkspaceId) = Me.Request.QueryString("workspaceid").ToString
                'Me.ViewState(VS_WorkspaceId) = Me.HWorkspaceId.Value.Trim()
                'txtproject.Text = Me.Request.QueryString("workspaceid").ToString
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally

        End Try

    End Function
#End Region

#Region "GenCall_Data "

    Private Function GenCall_Data() As Boolean
        Dim ds_Projects As New DataSet
        Dim wStr As String = ""
        Dim eStr As String = ""
        Dim Val As String = ""
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds_ClientMst As DataSet = Nothing
        'Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""


            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        Finally

        End Try
    End Function

#End Region

#Region "GenCall_showUI "

    Private Function GenCall_ShowUI() As Boolean
        Dim sender As New Object
        Dim e As New EventArgs
        Try
            Page.Title = " :: Attribute Search ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblMandatory"), Label).Visible = False
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Global Document Repository Report"

            'added by Deepak Sing h on 2-Mar-10 to show the set Project on page load
            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                GenCall_ShowUI = True
                Exit Function
            End If

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region "Grid Events"

    Protected Sub gvQuatation_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvQuatation.RowDataBound
        Dim HdrStr As String = String.Empty
        Dim MedExDesc As String = String.Empty
        Dim Dt_MedExInfoHdrDtl As DataTable = Nothing
        Dim Dv_MedExInfoHdrDtl As DataView = Nothing
        Dim Dt_MedEx As DataTable = Nothing
        Dim dr As DataRow = Nothing
        Dim ObjElement As Object = Nothing
        Dim StrValidation() As String = Nothing
        Try
            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Or e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(13).Style.Add("display", "none")
                'e.Row.Cells(3).Visible = False
                e.Row.Cells(5).Visible = False
                e.Row.Cells(11).Visible = False
            End If

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            If e.Row.RowType = DataControlRowType.Header Then
                For HdrIndex As Integer = 0 To e.Row.Cells.Count - 1
                    If e.Row.Cells(HdrIndex).Text.Contains("#") Then
                        HdrStr = e.Row.Cells(HdrIndex).Text.Trim()
                        MedExCode.Insert(HdrIndex, HdrStr.Substring(HdrStr.LastIndexOf("#") + 1).Trim())
                        e.Row.Cells(HdrIndex).Text = HdrStr.Substring(0, HdrStr.IndexOf("#"))
                    Else
                        MedExCode.Insert(HdrIndex, HdrIndex.ToString())
                    End If
                Next HdrIndex
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub


    Protected Sub gvProject_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvProject.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Or e.Row.RowType = DataControlRowType.Header Then
                'e.Row.Attributes.Add("attribute", "document.getElementByID(e.row.Cells(2)).style.display = 'none'")
                e.Row.Cells(11).Style.Add("display", "none")
            End If

            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.FindControl("ImgEdit"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImgEdit"), ImageButton).CommandName = "MYEDIT"
                e.Row.Attributes.Add("onclick", "document.getElementById('HIDDENFIELD').value = e.row.rowIndex;")

                CType(e.Row.FindControl("ImgDetails"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImgDetails"), ImageButton).CommandName = "MYDETAILS"
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

#End Region

#Region "Fill Dropdowns"

    'code is added by vikas shah to fill the doctype based on projecttypecode="0010"

    Private Function FillDocType() As Boolean
        Dim ds_Project As New DataSet
        Dim dv_Project As New DataView
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Try
            wstr = "iUserId=" + Me.Session(S_UserID) + " AND vProjectTypecode ='" + VS_ProjectTypeCode + "'"
            If Not objHelp.View_MyProjects(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Project, estr) Then
                Throw New Exception(estr)
            End If

            If ds_Project.Tables(0).Rows.Count > 0 Then
                ds_Project.Tables(0).Columns.Add("vProjectName", GetType(String))
                ds_Project.AcceptChanges()
                For Each dr As DataRow In ds_Project.Tables(0).Rows
                    dr("vProjectName") = dr("vWorkSpaceDesc") + " - " + "(" + dr("vProjectNo") + ")"
                Next
                ds_Project.AcceptChanges()


                dv_Project = ds_Project.Tables(0).DefaultView
                dv_Project.Sort = "vProjectName"

                ddlDocType.DataSource = dv_Project
                ddlDocType.DataValueField = "vWorkspaceId"
                ddlDocType.DataTextField = "vProjectName"
                ddlDocType.DataBind()

                ddlDocType.Items.Insert(0, New ListItem("All Document Type"))

                'ADDED TOOLTIP
                For iddlRegion As Integer = 0 To ddlDocType.Items.Count - 1
                    ddlDocType.Items(iddlRegion).Attributes.Add("title", ddlDocType.Items(iddlRegion).Text)
                Next
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function

    Private Function FillClient() As Boolean
        Dim ds_Client As New DataSet
        Dim dv_Client As New DataView
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Try
            wstr = "cStatusIndi <> 'D'"
            If Not objHelp.getclientmst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Client, estr) Then
                Throw New Exception(estr)
            End If

            If ds_Client.Tables(0).Rows.Count > 0 Then
                dv_Client = ds_Client.Tables(0).DefaultView
                dv_Client.Sort = "vClientName"

                chkclient.DataSource = dv_Client
                chkclient.DataValueField = "vClientCode"
                chkclient.DataTextField = "vClientName"
                chkclient.DataBind()
            Else
                chkclient.DataSource = Nothing
                chkclient.DataBind()
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function FillLocation() As Boolean
        Dim ds_Location As New DataSet
        Dim dv_Location As New DataView
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Try
            wstr = "cLocationType = 'L'"
            If Not objHelp.getLocationMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Location, estr) Then
                Throw New Exception(estr)
            End If

            If ds_Location.Tables(0).Rows.Count > 0 Then
                dv_Location = ds_Location.Tables(0).DefaultView
                dv_Location.Sort = "vLocationName"

                chklocation.DataSource = dv_Location
                chklocation.DataValueField = "vLocationCode"
                chklocation.DataTextField = "vLocationName"
                chklocation.DataBind()
            Else
                chklocation.DataSource = Nothing
                chklocation.DataBind()
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function FillMolecule() As Boolean
        Dim ds_Molecule As New DataSet
        Dim dv_Molecule As New DataView
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Try
            wstr = "cStatusIndi <> 'D'"
            If Not objHelp.MoleculeMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Molecule, estr) Then
                Throw New Exception(estr)
            End If

            If ds_Molecule.Tables(0).Rows.Count > 0 Then
                dv_Molecule = ds_Molecule.Tables(0).DefaultView
                dv_Molecule.Sort = "vMoleculeName"

                chkmolecule.DataSource = dv_Molecule
                chkmolecule.DataValueField = "vMoleculeCode"
                chkmolecule.DataTextField = "vMoleculeName"
                chkmolecule.DataBind()
            Else
                chkmolecule.DataSource = Nothing
                chkmolecule.DataBind()
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function FillProjectType() As Boolean
        Dim ds_ProjectType As New DataSet
        Dim dv_ProjectType As New DataView
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Try
            wstr = "vProjectTypeCode In(" + Me.Session(S_ScopeValue) + ") AND cStatusIndi <> 'D'"
            If Not objHelp.getprojectTypeMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ProjectType, estr) Then
                Throw New Exception(estr)
            End If

            If ds_ProjectType.Tables(0).Rows.Count > 0 Then
                dv_ProjectType = ds_ProjectType.Tables(0).DefaultView
                dv_ProjectType.Sort = "vProjectTypeName"

                chkprojecttype.DataSource = dv_ProjectType
                chkprojecttype.DataValueField = "vProjectTypeCode"
                chkprojecttype.DataTextField = "vProjectTypeName"
                chkprojecttype.DataBind()
            Else
                chkprojecttype.DataSource = Nothing
                chkprojecttype.DataBind()
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function
#End Region

#Region "Fill Grid"

    Private Function FillGvQuatation(ByVal WsId As String) As Boolean
        Dim Clients As String = String.Empty
        Dim Molecules As String = String.Empty
        Dim ProjectTypes As String = String.Empty
        Dim Locations As String = String.Empty
        Dim FromDate As String = String.Empty
        Dim ToDate As String = String.Empty
        Dim ds_Result As DataSet = Nothing
        Dim estr As String = String.Empty
        Dim dt As DataTable = New DataTable
        Dim ds As DataTable = New DataTable
        Dim errorstr As String = String.Empty
        Dim ds_details As DataSet = Nothing

        Try
            Clients = GenerateClientString()
            Molecules = GenerateMoleculeString()
            ProjectTypes = GenerateProjectTypeString()
            Locations = GenerateLocationString()
            FromDate = txtFromDate.Text.Trim
            ToDate = txtToDate.Text.Trim

            If Not objHelp.Proc_GetDetailsForGlobalDoc(WsId, Clients, Molecules, Locations, ProjectTypes, ds_Result, estr) Then
                Throw New Exception(estr)
            End If

            If Not objHelp.Proc_GetDetailsForGlobalDoc_temp(WsId, FromDate, ToDate, ds_details, errorstr) Then

                Throw New Exception(errorstr)

            End If

            If Not ds_Result Is Nothing AndAlso ds_Result.Tables(0).Rows.Count > 0 Then
                gvQuatation.DataSource = ds_Result
                gvQuatation.DataBind()
            Else
                gvQuatation.DataSource = Nothing
                gvQuatation.DataBind()
            End If


            If Not ds_details Is Nothing AndAlso ds_details.Tables(0).Rows.Count > 0 Then
                dt = ds_details.Tables(0)
                transposedataset(dt, WsId)
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function FillGvProject(ByVal WsId As String) As Boolean
        Dim Clients As String = String.Empty
        Dim Molecules As String = String.Empty
        Dim ProjectTypes As String = String.Empty
        Dim Locations As String = String.Empty
        Dim FromDate As String = String.Empty
        Dim ToDate As String = String.Empty
        Dim ds_Result As DataSet = Nothing
        Dim estr As String = String.Empty
        Dim ds_MedexDetail As DataSet = Nothing
        Dim Newestr As String = String.Empty

        Try
            Clients = GenerateClientString()
            Molecules = GenerateMoleculeString()
            ProjectTypes = GenerateProjectTypeString()
            Locations = GenerateLocationString()
            FromDate = txtFromDate.Text.Trim
            ToDate = txtToDate.Text.Trim

            If Not objHelp.Proc_GetDetailsForProjectSynopsis(WsId, Clients, Molecules, Locations, ProjectTypes, ds_Result, estr) Then
                Throw New Exception(estr)
            End If

            If Not objHelp.Proc_GetDetailsForGlobalDoc_temp(WsId, FromDate, ToDate, ds_MedexDetail, Newestr) Then
                Throw New Exception(Newestr)
            End If

            If Not ds_Result Is Nothing AndAlso ds_Result.Tables(0).Rows.Count > 0 Then
                gvProject.DataSource = ds_Result
                gvProject.DataBind()
            Else
                gvProject.DataSource = Nothing
                gvProject.DataBind()
            End If

            If Not ds_MedexDetail Is Nothing AndAlso ds_MedexDetail.Tables(0).Rows.Count > 0 Then
                dt = ds_MedexDetail.Tables(0)
                transposedataset(dt, WsId)
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    '' added by vikas shah
    ' added on: 17th March 2012
    'description: this function is used to convert datatable in particular format as per requirement and export that into excel

    Private Function transposedataset(ByVal ds_synopsis As DataTable, ByVal Ws_Synopsis As String) As Boolean
        Dim s As Integer = 0
        Dim dr As DataRow
        Dim i As Integer = 0
        Dim dc As New DataColumn
        'Dim dp As New DataTable
        Dim b As Integer = 0
        Try
            dt = ds_synopsis
            Dim dt_distinct As DataTable = (dt.DefaultView.ToTable(True, "vMedExDesc"))
            dt_transpose.Columns.Add(dc)

            For i = 0 To dt_distinct.Rows.Count - 1

                For Me.j = k To dt.Rows.Count - 1

                    If (dt_distinct.Rows(i)("vMedExDesc").ToString.Equals(dt.Rows(j)("vMedExDesc").ToString)) Then
                        dr = dt_transpose.NewRow
                        dt_transpose.Rows.Add(dr)
                        dt_transpose.Rows(b)(i) = dt.Rows(j)("vMedExResult")
                        k = k + 1
                        b = b + 1
                    Else
                        Dim dc_newcolumn As New DataColumn
                        dt_transpose.Columns.Add(dc_newcolumn)
                        k = k + 1
                        b = 0
                        Exit For
                    End If
                Next
            Next

            For i = 0 To dt_distinct.Rows.Count - 1
                dt_transpose.Columns(i).ColumnName = dt_distinct.Rows(i).Item(0)
                dt_transpose.AcceptChanges()
                s = i

            Next
            s = s + 1

            'this code is to remove some extra columns at last in datatable when workspace id= Quatation id
            If Ws_Synopsis.Equals(QuatationWId) Then
                For temp As Integer = s To dt_transpose.Columns.Count - 1
                    dt_transpose.Columns.RemoveAt(s)
                    dt_transpose.AcceptChanges()
                Next
                ViewState(VS_temp) = dt_transpose.Copy()
            End If

            If Ws_Synopsis.Equals(SynopsisWId) Then
                ds_ForSynopsis.Tables.Add(dt_transpose)
                ViewState(VS_user) = ds_ForSynopsis.Copy()
            End If
            k = 0
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function
#End Region

#Region "Helper Function"

    '' This Function concatinate clients, molecules, projecttypes and locations
    Private Function GenerateClientString() As String
        Dim clients As String = String.Empty
        Try
            If chkAnyClient.Checked = True Then
                Return clients
                'For Each lst As ListItem In chkclient.Items
                '    clients += "'" + lst.Text.Trim() + "',"
                'Next
            Else
                For Each lst As ListItem In chkclient.Items
                    If lst.Selected = True Then
                        clients += lst.Text.Trim() + ","
                    End If
                Next
            End If

            If clients <> String.Empty Then
                clients = clients.Remove(clients.Length - 1)
            End If

            Return clients
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message + " (Error in TransposeDataset function...)", "")
            Return False
        End Try
    End Function

    Private Function GenerateMoleculeString() As String
        Dim molecule As String = String.Empty
        Try
            If chkAnyMolecule.Checked = True Then
                Return molecule
                'For Each lst As ListItem In chkmolecule.Items
                '    molecule += "'" + lst.Text.Trim() + "',"
                'Next
            Else
                For Each lst As ListItem In chkmolecule.Items
                    If lst.Selected = True Then
                        molecule += lst.Text.Trim() + ","
                    End If
                Next
            End If

            If molecule <> String.Empty Then
                molecule = molecule.Remove(molecule.Length - 1)
            End If

            Return molecule
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function GenerateProjectTypeString() As String
        Dim ProjectType As String = String.Empty
        Try
            If chkAnyProjectType.Checked = True Then
                Return ProjectType
                'For Each lst As ListItem In chkprojecttype.Items
                '    ProjectType += "'" + lst.Text.Trim() + "',"
                'Next
            Else
                For Each lst As ListItem In chkprojecttype.Items
                    If lst.Selected = True Then
                        ProjectType += lst.Text.Trim() + ","
                    End If
                Next
            End If

            If ProjectType <> String.Empty Then
                ProjectType = ProjectType.Remove(ProjectType.Length - 1)
            End If

            Return ProjectType
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function GenerateLocationString() As String
        Dim location As String = String.Empty
        Try
            If chkAnyLocation.Checked = True Then
                Return location
                'For Each lst As ListItem In chklocation.Items
                '    location += "'" + lst.Text.Trim() + "',"
                'Next
            Else
                For Each lst As ListItem In chklocation.Items
                    If lst.Selected = True Then
                        location += lst.Text.Trim() + ","
                    End If
                Next
            End If

            If location <> String.Empty Then
                Return location.Remove(location.Length - 1)
            End If

            Return location
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region "Search Functions"

#End Region

#Region "Helper Functions"

    Public Sub DisplayBlankGrid(ByRef ds As DataSet, ByVal gridView As GridView, ByVal strMessage As String)
        Dim ColumnCount As Integer

        ds.Tables(0).Rows.Add(ds.Tables(0).NewRow())
        gridView.DataSource = ds
        gridView.DataBind()

        ColumnCount = gridView.Columns.Count
        gridView.Rows(0).Cells.Clear()
        gridView.Rows(0).Cells.Add(New TableCell())
        gridView.Rows(0).Cells(0).ColumnSpan = ColumnCount

        gridView.Rows(0).Cells(0).HorizontalAlign = HorizontalAlign.Center
        gridView.Rows(0).Cells(0).ForeColor = Drawing.ColorTranslator.FromHtml("#000080")
        gridView.Rows(0).Cells(0).Text = strMessage

    End Sub

#End Region

#Region "Button Events"

    Protected Sub btnsearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsearch.Click
        Try
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "onUpdating", "onUpdating(sender,e)", True)
            If ddlDocType.SelectedIndex = 0 Then
                FillGvProject(SynopsisWId)
                FillGvQuatation(QuatationWId)
                trQuatation.Style.Add("Display", "")
                trProject.Style.Add("Display", "")
            Else
                If ddlDocType.SelectedValue = QuatationWId Then
                    gvProject.DataSource = Nothing
                    gvProject.DataBind()
                    FillGvQuatation(QuatationWId)
                    trQuatation.Style.Add("Display", "")
                ElseIf ddlDocType.SelectedValue = SynopsisWId Then
                    gvQuatation.DataSource = Nothing
                    gvQuatation.DataBind()
                    FillGvProject(SynopsisWId)
                    trProject.Style.Add("Display", "")
                End If
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "fixheader", "fixheader()", True)
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "onUpdated", "onUpdated()", True)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Try
            Me.Response.Redirect("frmMainPage.aspx?mode=1")
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub ImgExport_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgExport.Click
        Try
            If gvQuatation.Rows.Count <> 0 Then
                'ObjCommon.ShowAlert("Data Not Available.", Me.Page)
                'Exit Sub

                GridView3.DataSource = ViewState(VS_temp)
                GridView3.DataBind()

                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(stringWriter)

                GridView3.RenderControl(writer)
                Dim gridViewhtml As String = stringWriter.ToString()

                Dim fileName As String = "Quatation" & ".xls"

                Context.Response.Buffer = True
                Context.Response.ClearContent()
                Context.Response.ClearHeaders()

                Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
                Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)

                Context.Response.Write(gridViewhtml.Substring(5).Substring(0, gridViewhtml.Substring(5).Length - 6))
                GridView3.DataSource = Nothing

                Context.Response.Flush()
                Context.Response.End()
                File.Delete(fileName)

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub ImgExoprtProject_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgExoprtProject.Click
        Try
            'gridview 
            If gvProject.Rows.Count <> 0 Then
                '    ObjCommon.ShowAlert("Data Not Available.", Me.Page)
                '    Exit Sub
                'End If
                Dim ds_Export As New DataSet
                'Dim dt_Temptable As New DataTable
                ds_Export = ViewState(VS_user)

                'ds_Export.Tables.Add(dt_Temptable)
                GridView4.DataSource = ds_Export
                GridView4.DataBind()

                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(stringWriter)

                Me.GridView4.RenderControl(writer)
                Dim gridViewhtml As String = stringWriter.ToString()

                Dim fileName As String = "Project Synopsis" & ".xls"

                Context.Response.Buffer = True
                Context.Response.ClearContent()
                Context.Response.ClearHeaders()

                Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
                Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)

                Context.Response.Write(gridViewhtml.Substring(5).Substring(0, gridViewhtml.Substring(5).Length - 6))
                GridView3.DataSource = Nothing
                Context.Response.Flush()
                Context.Response.End()

                File.Delete(fileName)

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        ' Don't remove this function
        ' This Function is mendetory when you are going to export your grid to excel.
        ' NOTE :: And Click event of button must be in postback trigger. (Page must be loaded)
    End Sub

    Protected Sub ddlReportFor_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlReportFor.SelectedIndexChanged
        Dim LastMonth As String = String.Empty
        Dim LastdayOfPpreviousMonth As String = String.Empty
        Dim FromDate As String = String.Empty
        Dim ToDate As String = String.Empty

        If ddlReportFor.SelectedItem.ToString() = "-- Select --" Then
            txtFromDate.Text = ""
            txtToDate.Text = ""
        ElseIf ddlReportFor.SelectedItem.ToString() = "Last Month" Then
            FromDate = "01" & "-" & MonthName(DateTime.Now.AddMonths(-1).Month, True) & "-" & DateTime.Now.AddMonths(-1).Year.ToString()
            LastdayOfPpreviousMonth = DateTime.DaysInMonth(DateTime.Now.AddMonths(-1).Year.ToString(), DateTime.Now.AddMonths(-1).Month)
            ToDate = LastdayOfPpreviousMonth & "-" & MonthName(DateTime.Now.AddMonths(-1).Month, True) & "-" & DateTime.Now.AddMonths(-1).Year.ToString()
            txtFromDate.Text = FromDate
            txtToDate.Text = ToDate
        ElseIf ddlReportFor.SelectedItem.ToString() = "Current Month" Then
            FromDate = "01" & "-" & MonthName(DateTime.Now.Month, True) & "-" & DateTime.Now.Year.ToString()
            LastdayOfPpreviousMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)
            ToDate = LastdayOfPpreviousMonth & "-" & MonthName(DateTime.Now.Month, True) & "-" & DateTime.Now.Year
            txtFromDate.Text = FromDate
            txtToDate.Text = ToDate
        ElseIf ddlReportFor.SelectedItem.ToString() = "Last Quater" Then
            FromDate = "01" & "-" & MonthName(DateTime.Now.AddMonths(-2).Month, True) & "-" & DateTime.Now.AddMonths(-2).Year.ToString()
            LastdayOfPpreviousMonth = DateTime.DaysInMonth(DateTime.Now.AddMonths(0).Year.ToString(), DateTime.Now.AddMonths(0).Month)
            ToDate = LastdayOfPpreviousMonth & "-" & MonthName(DateTime.Now.AddMonths(0).Month, True) & "-" & DateTime.Now.AddMonths(0).Year.ToString()
            txtFromDate.Text = FromDate
            txtToDate.Text = ToDate
        ElseIf ddlReportFor.SelectedItem.ToString() = "Current Calendar Year" Then
            FromDate = "01" & "-" & "Jan" & "-" & DateTime.Now.Year.ToString()
            'LastdayOfPpreviousMonth = DateTime.DaysInMonth(DateTime.Now.AddMonths(-1).Year.ToString(), DateTime.Now.AddMonths(-1).Month)
            ToDate = "31" & "-" & "Dec" & "-" & DateTime.Now.Year.ToString()
            txtFromDate.Text = FromDate
            txtToDate.Text = ToDate
        ElseIf ddlReportFor.SelectedItem.ToString() = "Last Calendar Year" Then
            FromDate = "01" & "-" & "Jan" & "-" & DateTime.Now.AddYears(-1).Year.ToString()
            'LastdayOfPpreviousMonth = DateTime.DaysInMonth(DateTime.Now.AddMonths(-1).Year.ToString(), DateTime.Now.AddMonths(-1).Month)
            ToDate = "31" & "-" & "Dec" & "-" & DateTime.Now.AddYears(-1).Year.ToString()
            txtFromDate.Text = FromDate
            txtToDate.Text = ToDate
        ElseIf ddlReportFor.SelectedItem.ToString() = "Current Financial Year" Then
            FromDate = "01" & "-" & "Apr" & "-" & DateTime.Now.Year.ToString()
            ToDate = "31" & "-" & "Mar" & "-" & DateTime.Now.AddYears(1).Year.ToString()
            txtFromDate.Text = FromDate
            txtToDate.Text = ToDate
        ElseIf ddlReportFor.SelectedItem.ToString() = "Last Financial Year" Then
            FromDate = "01" & "-" & "Apr" & "-" & DateTime.Now.AddYears(-1).Year.ToString()
            ToDate = "31" & "-" & "Mar" & "-" & DateTime.Now.Year.ToString()
            txtFromDate.Text = FromDate
            txtToDate.Text = ToDate
        End If
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

End Class
