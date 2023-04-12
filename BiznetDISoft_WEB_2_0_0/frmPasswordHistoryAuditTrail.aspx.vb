
Partial Class frmPasswordHistoryAuditTrail
    Inherits System.Web.UI.Page

#Region " VARIABLE DECLARATION "

    Private objCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objPVNET As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()
    Private eStr_Retu As String

    Private Const VS_UserID As String = "iUserID"

    Private Const GVC_UserID As Integer = 0
    Private Const GVC_SrNo As Integer = 1

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then

            If Not Me.Request.QueryString("Value") Is Nothing AndAlso _
                                    Me.Request.QueryString("Value").ToString.Length > 0 Then

                Me.ViewState(VS_UserID) = Me.Request.QueryString("Value").ToString.Trim

            End If

            If Not GenCall() Then
                Exit Sub
            End If
        End If
    End Sub

#Region " GENCALL "

    Private Function GenCall() As Boolean
        GenCall = True

        Try

            GenCall_Data()
            GenCall_ShowUI()

        Catch ex As Exception
            GenCall = False
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Function

    Private Sub GenCall_Data()

    End Sub

    Private Sub GenCall_ShowUI()
        Try
            Page.Title = ":: Password Change Audit Trail :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("Menu1"), Menu).Visible = False
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Password Change Audit Trail"

            Me.FillPasswordHistoryAuditTrailGrid()

        Catch ex As Exception
            Throw ex
        End Try

    End Sub
#End Region

#Region " FILL FUNCTIONS "

    Private Sub FillPasswordHistoryAuditTrailGrid()
        Dim wStr_PasswordHistoryAuditTrailGrid As String = String.Empty
        Dim dsPasswordHistoryAuditTrail As New DataSet
        Dim dv_PasswordHistoryAuditTrail As New DataView
        Dim dt_PasswordHistoryAuditTrail As New DataTable
        Dim dc_DateWithIST As New DataColumn
        Try
            wStr_PasswordHistoryAuditTrailGrid = "iUserId=" + Me.ViewState(VS_UserID) + " ORDER BY dChangedDate"
            If Not objHelp.View_PasswordHistoryAuditTrail(wStr_PasswordHistoryAuditTrailGrid, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                       dsPasswordHistoryAuditTrail, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If
            dc_DateWithIST = New DataColumn("dChangedDate_IST", System.Type.GetType("System.String"))

            dsPasswordHistoryAuditTrail.Tables(0).Columns.Add(dc_DateWithIST)

            For Each dr_AuditDate In dsPasswordHistoryAuditTrail.Tables(0).Rows
                dr_AuditDate("dChangedDate_IST") = Convert.ToString(dr_AuditDate("dChangedDate") + strServerOffset)
            Next
            dsPasswordHistoryAuditTrail.AcceptChanges()

            'dv_PasswordHistoryAuditTrail = dsPasswordHistoryAuditTrail.Tables(0).DefaultView
            'dt_PasswordHistoryAuditTrail = dv_PasswordHistoryAuditTrail.ToTable()




            Me.gvwPasswordHistoryAuditTrail.DataSource = dsPasswordHistoryAuditTrail.Tables(0)
            Me.gvwPasswordHistoryAuditTrail.DataBind()

            If Me.gvwPasswordHistoryAuditTrail.Rows.Count > 0 Then
                Me.gvwPasswordHistoryAuditTrail.FooterRow.Visible = False
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region " GRID VIEW EVENTS "

    Protected Sub gvwPasswordHistoryAuditTrail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwPasswordHistoryAuditTrail.RowDataBound
        e.Row.Cells(GVC_UserID).Visible = False

        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (gvwPasswordHistoryAuditTrail.PageSize * gvwPasswordHistoryAuditTrail.PageIndex) + 1
        End If
    End Sub

#End Region

#Region " ERROR MESSAGE "

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub

#End Region

End Class
