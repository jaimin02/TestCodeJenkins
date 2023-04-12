
Partial Class frmViewSubjectDetail
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATIONS"

    Dim objCommon As New clsCommon
    Dim objHelpDBTable As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

    Dim ds_SubjectDet As DataSet
    Dim ds_SubComments As DataSet
    Dim eStr_Retu As String = ""
    Dim workspaceId As String = "0000000001"
    Dim nodeId As String = "1"
    Dim opCode As String = "0006"
    Dim workspaceSubjectDocDetailId As String = ""
    Dim ds_DocType As DataSet
    Dim DBRetMode As New WS_HelpDbTable.DataRetrievalModeEnum


#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CType(Me.Master.FindControl("lblHeading"), Label).Text = "View Subject Details"
        CType(Me.Master.FindControl("lblMandatory"), Label).Text = ""
        Page.Title = ":: View Subject Details   :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

        If Not IsPostBack Then
            FillDocTypeDropDown(ddlDocType)
            BindGrid(ddlDocType.SelectedValue.Trim.ToString)
            SetLabels()
        End If

    End Sub


#Region "GRIDVIEW EVENTS"

    Protected Sub gvwViewSubjectDetail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwViewSubjectDetail.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblsrNo As Label
            lblsrNo = CType(e.Row.FindControl("lblSrNo"), Label)
            lblsrNo.Text = e.Row.RowIndex + 1

            Dim hlnk As HyperLink
            hlnk = CType(e.Row.FindControl("hlnkDoc"), HyperLink)
            hlnk.Text = Path.GetFileName(hlnk.Text)


        End If
    End Sub

    Protected Sub gvwViewSubjectDetail_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        e.Row.Cells(1).Visible = False


        
    End Sub

    Protected Sub gvwViewSubjectDetail_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        workspaceSubjectDocDetailId = gvwViewSubjectDetail.Rows(0).Cells(1).Text.Trim
        Me.ViewState("WorkspaceSubjectDocDetailId") = gvwViewSubjectDetail.Rows(0).Cells(1).Text.Trim
        divComments.Visible = True

        BindCommentGrid()
    End Sub



    Protected Sub GVSubjectComments_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lblSrNo"), Label).Text = e.Row.RowIndex + 1
        End If
    End Sub
#End Region

#Region "DATA RETRIEVEL"

    Private Sub BindGrid(ByVal docTypeCode As String)
        ds_SubjectDet = New DataSet
        Dim dv_SubjectDet As DataView = Nothing
        Dim dt_SubjectDet As New DataTable
        Try
            If objHelpDBTable.GetView_SubjectDocDetails("", ds_SubjectDet, eStr_Retu) Then

                If String.IsNullOrEmpty(docTypeCode) Then
                    gvwViewSubjectDetail.DataSource = ds_SubjectDet
                Else
                    dt_SubjectDet = ds_SubjectDet.Tables(0)
                    dv_SubjectDet = New DataView(dt_SubjectDet)
                    dv_SubjectDet.RowFilter = "vDocTypeCode ='" & docTypeCode & "'"
                    gvwViewSubjectDetail.DataSource = dv_SubjectDet.ToTable 'ds_SubjectDet.Tables(0).Select("vDocTypeCode = " & docTypeCode)
                End If


                gvwViewSubjectDetail.DataBind()
            Else
                objCommon.ShowAlert("Cannot fetch Data", Me)
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Private Sub BindCommentGrid()
        ds_SubComments = New DataSet
        Try
            If objHelpDBTable.GetSubjectComments("", ds_SubComments, eStr_Retu) Then
                GVSubjectComments.DataSource = ds_SubComments
                GVSubjectComments.DataBind()
            Else
                objCommon.ShowAlert("Cannot display comments, Error", Me)
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub FillDocTypeDropDown(ByRef ddl As DropDownList)
        ds_DocType = New DataSet

        If objHelpDBTable.FillDropDown("DocTypeMst", "vDocTypeCode", "vDocTypeName", "", ds_DocType, eStr_Retu) Then

            ddl.Items.Clear()
            ddl.DataSource = ds_DocType
            ddl.DataValueField = ds_DocType.Tables(0).Columns(0).ColumnName
            ddl.DataTextField = ds_DocType.Tables(0).Columns(1).ColumnName
            ddl.DataBind()
        End If
    End Sub

    Private Sub SetLabels()
        Dim dsTemp As New DataSet
        'FILL DROPDOWN FUNCTION IS USED TO DISPLAY DATA IN LABEL
        'PLEASE DONT USE IT
        If objHelpDBTable.FillDropDown("View_getWorkspaceDetailForHdr", "vProjectNo", "vProjectNo", " vworkspaceid = " & Me.workspaceId, dsTemp, eStr_Retu) Then
            If dsTemp.Tables(0).Rows.Count > 0 Then
                lblProjectNo.Text = dsTemp.Tables(0).Rows(0)(0).ToString
            End If
        End If

        dsTemp.Dispose()
        dsTemp = New DataSet
        If objHelpDBTable.FillDropDown("workspacenodedetail", "vNodeDisplayName", "vNodeName", " iNodeId = " & Me.nodeId, dsTemp, eStr_Retu) Then
            If dsTemp.Tables(0).Rows.Count > 0 Then
                lblActivity.Text = dsTemp.Tables(0).Rows(0)(0).ToString
            End If
        End If
    End Sub

   

#End Region


    Protected Sub ddlDocType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        BindGrid(CType(sender, DropDownList).SelectedValue.Trim.ToString)
    End Sub


    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ObjCommon As New clsCommon
        Dim objLmd As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
        Dim ds_InsertWorkspaceSubjectComment As New DataSet
        Dim dt As New DataTable
        Dim eStr As String = ""

        Try
            divComments.Visible = False
            AssignValues()

            dt = Me.ViewState("dtWorkspaceSubjectComment")
            ds_InsertWorkspaceSubjectComment = New DataSet
            ds_InsertWorkspaceSubjectComment.Tables.Add(dt)
            ds_InsertWorkspaceSubjectComment.Tables(0).TableName = "WorkspaceSubjectComment"

            If objLambda.Save_InsertworkspaceSubjectComment(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                            ds_InsertWorkspaceSubjectComment, "", Me.Session("UserCode"), eStr) Then
            Else
                Me.ShowErrorMessage("", eStr)
                Exit Sub
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "", ex)
        End Try
    End Sub


    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        divComments.Visible = False
    End Sub


    Private Sub AssignValues()
        Dim ObjCommon As New clsCommon
        Dim objHelpDBTable As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim dt_WorkspaceSubjectComment As New DataTable
        Dim ds_WorkspaceSubjectComment As New DataSet
        Dim dr As DataRow
        Dim dt As New DataTable
        Dim index As Integer = 0

        Try
            If objHelpDBTable.getWorkspaceSubjectComment("", DBRetMode.DataTable_Empty, ds_WorkspaceSubjectComment, eStr_Retu) Then
                dt_WorkspaceSubjectComment = ds_WorkspaceSubjectComment.Tables(0)

                dr = dt_WorkspaceSubjectComment.NewRow
                dr("vWorkSpaceSubjectCommentId") = ""
                dr("vWorkSpaceSubjectDocDetailID") = Me.ViewState("WorkspaceSubjectDocDetailId")
                dr("vComment") = txtComments.Text.Trim
                dr("iModifyBy") = Me.Session("UserID").ToString

                dt_WorkspaceSubjectComment.Rows.Add(dr)

                dt = dt_WorkspaceSubjectComment
                ds_WorkspaceSubjectComment.Tables.Clear()
                ds_WorkspaceSubjectComment.Dispose()
                Me.ViewState("dtWorkspaceSubjectComment") = dt



            Else
                objCommon.ShowAlert("Cannot Assign Values", Me)
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
        End Try
    End Sub






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

   
    Protected Sub BtnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Response.Redirect(Me.Request.QueryString("Page2").Trim() & ".aspx?WorkSpaceId=" & Me.Request.QueryString("WorkSpaceId").Trim() & "&Page=" & Me.Request.QueryString("Page").Trim())
    End Sub
End Class
