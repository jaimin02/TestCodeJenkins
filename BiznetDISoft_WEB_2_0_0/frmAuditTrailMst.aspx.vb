
Partial Class frmAuditTrailMst
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()

    Private Const VS_WorkspaceId As String = "vWorkSpaceId"
    Private Const VS_NId As String = "NodeId"

#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            GenCall()

            Exit Sub
        End If
    End Sub
#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean
        Dim dt_DocAudit As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Dim WorkSpaceId As String = ""
        Dim NodeId As String
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Choice = CType("1", WS_Lambda.DataObjOpenSaveModeEnum)
            Me.ViewState("Choice") = Choice   'To use it while saving
            WorkSpaceId = Me.Request.QueryString("WorkSpaceId")
            NodeId = Me.Request.QueryString("NodeId")
            Me.ViewState(VS_WorkspaceId) = WorkSpaceId
            Me.ViewState(VS_NId) = NodeId

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState("VS_WorkspaceId") = Me.Request.QueryString("Value").ToString
            End If

            ''''Check for Valid User''''''''''''''
            If Not GenCall_Data(Choice, dt_DocAudit) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState("dtDocAudit") = dt_DocAudit ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI(Choice, dt_DocAudit) Then 'For Displaying Data 
                Exit Function
            End If

            GenCall = True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_Data"
    Private Function GenCall_Data(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                            ByRef dt_Dist_Retu As DataTable) As Boolean

        Dim eStr As String = ""
        Dim wStr As String = ""
        Dim Wstr_WS As String = ""
        Dim ds_DocAudit As DataSet = Nothing
        'Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Wstr_WS = "vWorkspaceid = '" + Me.ViewState(VS_WorkspaceId).ToString.Trim() + "' And iNodeId = " + Me.ViewState(VS_NId).ToString.Trim()

            If Not objHelp.GetViewProjectActivityAtrributes(Wstr_WS, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_DocAudit, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_DocAudit Is Nothing Then
                Throw New Exception(eStr)
            End If

            If ds_DocAudit.Tables(0).Rows.Count <= 0 And _
               Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Throw New Exception("No Records Found for Selected role")
            End If

            dt_Dist_Retu = ds_DocAudit.Tables(0)
            GenCall_Data = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally

        End Try
    End Function
#End Region

#Region "GenCall_ShowUI"
    Private Function GenCall_ShowUI(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                                      ByVal dt_Dist As DataTable) As Boolean
        Dim dt_DocAudit As DataTable = Nothing
        Try
            Page.Title = " :: Audit Trail ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Master.FindControl("lblMandatory"), Label).Text = ""
            CType(Master.FindControl("lblHeading"), Label).Text = "Audit Trial"

            FillAudittrialMst()

            GenCall_ShowUI = True

        Catch ex As Exception
            GenCall_ShowUI = False
        End Try

    End Function
#End Region

#Region "Fill AudittrialMst"
    Private Function FillAudittrialMst() As Boolean
        Dim wStr As String = ""
        Dim eStr As String = ""
        Dim Wstr_WS As String = ""
        Dim ds_DocAudit As New Data.DataSet
        Dim WorkSpaceId As String = ""
        Dim NodeId As String = ""
        Dim dc_AuditTrailMst As New DataColumn
        'Dim dr_Audit As DataRow
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            WorkSpaceId = Me.Request.QueryString("WorkSpaceId")
            NodeId = Me.Request.QueryString("NodeId")
            Me.ViewState(VS_WorkspaceId) = WorkSpaceId
            Me.ViewState(VS_NId) = NodeId

            Wstr_WS = "vWorkspaceid = '" + Me.ViewState(VS_WorkspaceId).ToString.Trim() + "' And iNodeId = " + Me.ViewState(VS_NId).ToString.Trim()
            If Not objHelp.GetViewProjectActivityAtrributes(Wstr_WS, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_DocAudit, eStr) Then
                Throw New Exception(eStr)
            End If
            Me.lblProjectNo.Text = ds_DocAudit.Tables(0).Rows(0).Item("vProjectNo")
            Me.lblProjectName.Text = ds_DocAudit.Tables(0).Rows(0).Item("vWorkSpaceDesc")
            Me.lblActivityName.Text = ds_DocAudit.Tables(0).Rows(0).Item("vNodeDisplayName")

            dc_AuditTrailMst = New DataColumn("dModifyOn_IST", System.Type.GetType("System.String"))
            ds_DocAudit.Tables(0).Columns.Add("dModifyOn_IST")
            ds_DocAudit.AcceptChanges()
            For Each dr_Audit In ds_DocAudit.Tables(0).Rows
                dr_Audit("dModifyOn_IST") = Convert.ToString(dr_Audit("dModifyOn") + strServerOffset)
            Next
            ds_DocAudit.AcceptChanges()



            Me.gvdeptstage.DataSource = ds_DocAudit
            Me.gvdeptstage.DataBind()
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function
#End Region

#Region "Button Click"

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Dim DMS As String = ""
        Me.Response.Redirect(Me.Request.QueryString("Page2").Trim() & ".aspx?WorkSpaceId=" & Me.Request.QueryString("WorkSpaceId").Trim() & "&Page=" & Me.Request.QueryString("Page").Trim() & "&Type=" & Me.Request.QueryString("Type") & IIf(DMS.Trim() = "", "", "&DMS=" & DMS))

    End Sub

#End Region

#Region "Grid Event"

    Protected Sub gvdeptstage_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Dim hlnk As HyperLink = CType(e.Row.FindControl("hlnkDocument"), HyperLink)

        If e.Row.RowType = DataControlRowType.DataRow Then

            If Not IsNothing(hlnk) Then

                hlnk.Text = Path.GetFileName(hlnk.Text)

            End If
        End If
    End Sub

    Protected Sub gvdeptstage_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        gvdeptstage.PageIndex = e.NewPageIndex
        If Not FillAudittrialMst() Then
            Exit Sub
        End If
    End Sub

#End Region

#Region "Reset Page"
    Private Sub ResetPage()
        Me.ViewState(VS_WorkspaceId) = ""
        Me.ViewState(VS_NId) = ""
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
    
End Class

