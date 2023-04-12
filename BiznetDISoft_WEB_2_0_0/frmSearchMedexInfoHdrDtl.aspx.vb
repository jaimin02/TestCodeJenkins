Partial Class frmSearchMedexInfoHdrDtl
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()
    Private VS_DtSearch As String = "Dt_Search"
    Private VS_DtResult As String = "Dt_Result"
    Private VS_DtView As String = "Dt_View"
    Private VS_QueryStr As String = "QueryStr"

    Private GVSC_SrNo As Integer = 0
    Private GVSC_SearchCondition As Integer = 1

    Private GVRC_SrNo As Integer = 0
    Private GVRC_WorkSpaceId As Integer = 1
    Private GVRC_WorkSpaceDesc As Integer = 2
    Private GVRC_ProjectDetails As Integer = 3

#End Region

#Region "Page Load Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CType(Master.FindControl("lblHeading"), Label).Text = "Search AttributeWise"
        If Not IsPostBack Then
            ShowErrorMessage("", "")
            GenCall_ShowUI()
        End If
    End Sub

#End Region
    
#Region "GenCall_ShowUI"
    Private Function GenCall_ShowUI() As Boolean

        If Not CreateTable() Then
            Me.objcommon.ShowAlert("Error While Creating Table", Me.Page())
            Return False
        End If

        Me.BtnSearch.Visible = False
        Page.Title = ":: Search AttributeWise  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
        Return True

    End Function
#End Region

#Region "CreateTable"

    Private Function CreateTable() As Boolean
        Dim dc As New DataColumn
        Dim dtNew As New DataTable

        Try

            dc = New DataColumn
            dc.ColumnName = "SrNo"
            dc.DataType = GetType(Integer)
            dtNew.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "SearchCondition"
            dc.DataType = GetType(String)
            dtNew.Columns.Add(dc)

            dtNew.AcceptChanges()
            Me.ViewState(VS_DtSearch) = dtNew
            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function

#End Region

#Region "Button Click Event"

    Protected Sub BtnAnd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAnd.Click

        AssignValues_Search("AND")
        BindGVSearch()
        Reset()

    End Sub

    Protected Sub BtnOr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnOr.Click

        AssignValues_Search("OR")
        BindGVSearch()
        Reset()

    End Sub

    Protected Sub BtnEnd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnEnd.Click

        AssignValues_Search("END")
        BindGVSearch()
        Reset()
        Me.BtnSearch.Visible = True

    End Sub

    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click

        Reset()
        Me.BtnSearch.Visible = False
        Me.GVSearch.DataSource = Nothing
        Me.GVSearch.DataBind()
        Me.GVResult.DataSource = Nothing
        Me.GVResult.DataBind()
        Me.ViewState(VS_DtResult) = Nothing
        CType(Me.ViewState(VS_DtSearch), DataTable).Clear()

    End Sub

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click
        Me.Response.Redirect("frmmainpage.aspx")
    End Sub

    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSearch.Click
        Dim ds_Result As New DataSet
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim Wstr_Scope As String = String.Empty
        Dim index As Integer
        Dim dv_Result As New DataView
        Dim dt_Result As New DataTable
        Dim QStr As String = String.Empty
        Try
            ShowErrorMessage("", "")

            For index = 0 To Me.GVSearch.Rows.Count - 1

                If (index = Me.GVSearch.Rows.Count - 1 And _
                        Me.GVSearch.Rows(index).Cells(GVSC_SearchCondition).Text.ToUpper.Contains(") AND")) Then

                    Wstr += Me.GVSearch.Rows(index).Cells(GVSC_SearchCondition).Text.Substring(0, Me.GVSearch.Rows(index).Cells(GVSC_SearchCondition).Text.ToUpper.LastIndexOf("AND") - 1).Trim()
                    Exit For

                ElseIf (index = Me.GVSearch.Rows.Count - 1 And _
                        Me.GVSearch.Rows(index).Cells(GVSC_SearchCondition).Text.ToUpper.Contains(") OR")) Then

                    Wstr += Me.GVSearch.Rows(index).Cells(GVSC_SearchCondition).Text.Substring(0, Me.GVSearch.Rows(index).Cells(GVSC_SearchCondition).Text.ToUpper.LastIndexOf("OR") - 1).Trim()
                    Exit For

                End If
                If (index = 0) Then
                    Wstr += Me.GVSearch.Rows(index).Cells(GVSC_SearchCondition).Text.Trim().Replace("&lt;", "<").Replace("&gt;", ">")
                Else
                    Wstr = Wstr + Me.GVSearch.Rows(index).Cells(GVSC_SearchCondition).Text.Trim().Replace("&lt;", "<").Replace("&gt;", ">")
                End If
            Next

            'For ScopeManagement on 21-Jan-2009
            'To Get Where condition of ScopeVales( Project Type )
            If Not objcommon.GetScopeValueWithCondition(Wstr_Scope) Then
                Exit Sub
            End If

            Wstr += "And " + Wstr_Scope

            If Not Me.objHelp.View_MedExInfoHdrDtl_Edit(Wstr, "vWorkSpaceId,vWorkSpaceDesc,vActivityId", ds_Result, estr) Then

                Me.objcommon.ShowAlert("Error While Getting Result Data :" + estr, Me.Page())
                Exit Sub

            End If

            If ds_Result.Tables(0).Rows.Count < 1 Then
                Me.objcommon.ShowAlert("No Record Found", Me.Page)
                Exit Sub
            End If

            For Each dr As DataRow In ds_Result.Tables(0).Rows
                QStr += dr("vActivityId").ToString() + ","
            Next dr
            QStr = QStr.Substring(0, QStr.LastIndexOf(","))
            Me.ViewState(VS_QueryStr) = QStr

            dv_Result = ds_Result.Tables(0).DefaultView.ToTable(True, "vWorkSpaceId,vWorkSpaceDesc".Split(",")).DefaultView
            dt_Result = dv_Result.ToTable()

            Me.ViewState(VS_DtResult) = dt_Result
            BindGVResult()

            CType(Me.ViewState(VS_DtSearch), DataTable).Clear()

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...BtnSearch_Click")
        End Try

    End Sub

#End Region

#Region "AssignValues"

    Private Sub AssignValues_Search(ByVal Criteria As String)
        Dim dr As DataRow
        Dim dt_Search As New DataTable
        Try

            ShowErrorMessage("", "")
            dt_Search = CType(Me.ViewState(VS_DtSearch), DataTable)
            dr = dt_Search.NewRow()
            dr("SrNo") = dt_Search.Rows.Count + 1
            dr("SearchCondition") = " (vMedexCode='" & Me.hMedexCode.Value.Trim() & "' And vDefaultValue " & _
            Me.rblstOption.SelectedItem.Value.ToString().Trim() & " '" & _
            Me.txtMedexValue.Text.Trim() & "') " & IIf(Criteria = "END", "", Criteria) & " "
            'dr("SearchCondition") = " (vMedexCode='" & Me.hMedexCode.Value.Trim() & "' And vDefaultValue " & _
            '                        Me.rblstOption.SelectedItem.Value.ToString().Trim() & " '" & Me.txtMedexValue.Text.Trim() & "')"

            dt_Search.Rows.Add(dr)
            dt_Search.AcceptChanges()
            Me.ViewState(VS_DtSearch) = dt_Search

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...AssignValues_Search")
        End Try

    End Sub

#End Region

#Region "Bind Grids"

    Private Sub BindGVSearch()
        Dim dt_Search As New DataTable
        Try
            ShowErrorMessage("", "")

            dt_Search = CType(Me.ViewState(VS_DtSearch), DataTable)
            dt_Search.DefaultView.Sort = "SrNo"
            Me.GVSearch.DataSource = dt_Search.DefaultView.ToTable()
            Me.GVSearch.DataBind()

            If Me.GVSearch.Rows.Count < 1 Then
                Me.GVSearch.DataSource = Nothing
                Me.GVSearch.DataBind()
                Me.GVResult.DataSource = Nothing
                Me.GVResult.DataBind()
                Me.BtnSearch.Visible = False
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...dt_Search")
        End Try
    End Sub

    Private Sub BindGVResult()
        Dim dt_Result As New DataTable
        Try
            ShowErrorMessage("", "")

            dt_Result = CType(Me.ViewState(VS_DtResult), DataTable)
            dt_Result.DefaultView.Sort = "vWorkSpaceDesc"
            Me.GVResult.DataSource = dt_Result.DefaultView.ToTable()
            Me.GVResult.DataBind()

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...dt_Result")
        End Try
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

#Region "Grid Events"

    Protected Sub GVSearch_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVSearch.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            CType(e.Row.FindControl("ImgBtnDelete"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("ImgBtnDelete"), ImageButton).CommandName = "DELETE"

        End If

    End Sub

    Protected Sub GVSearch_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GVSearch.RowCommand
        Dim dt_Search As New DataTable
        Dim index As Integer = CType(e.CommandArgument, Integer)

        If e.CommandName.ToUpper.Trim() = "DELETE" Then

            dt_Search = CType(Me.ViewState(VS_DtSearch), DataTable)

            For Each dr As DataRow In dt_Search.Rows

                If dr("SrNo") = Me.GVSearch.Rows(index).Cells(GVSC_SrNo).Text.Trim() Then
                    dt_Search.Rows.Remove(dr)
                    Exit For
                End If

            Next

            dt_Search.AcceptChanges()
            Me.ViewState(VS_DtSearch) = dt_Search

            BindGVSearch()

        End If

    End Sub

    Protected Sub GVSearch_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs)

    End Sub

    Protected Sub GVResult_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVResult.RowCreated

        e.Row.Cells(GVRC_WorkSpaceId).Visible = False

    End Sub

    Protected Sub GVResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVResult.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            e.Row.Cells(GVRC_SrNo).Text = e.Row.RowIndex + (Me.GVResult.PageIndex * Me.GVResult.PageSize) + 1

            CType(e.Row.FindControl("lnkProjectDetails"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkProjectDetails"), LinkButton).CommandName = "ProjectDetails"

        End If

    End Sub

    Protected Sub GVResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GVResult.RowCommand
        Dim Index As Integer = CType(e.CommandArgument, Integer)
        Dim Str As String = ""

        If e.CommandName.ToUpper.Trim() = "PROJECTDETAILS" Then

            Me.Session.Add(S_ActivityIds, Me.ViewState(VS_QueryStr))
            Str = Me.GVResult.Rows(Index).Cells(GVRC_WorkSpaceId).Text.ToString()
            Me.Response.Redirect("frmDocumentManagementProject.aspx?WorkspaceId=" + Str)

        End If

    End Sub

#End Region

    Public Sub Reset()
        Me.txtMedex.Text = ""
        Me.txtMedexValue.Text = ""
        Me.hMedexCode.Value = ""
    End Sub
End Class