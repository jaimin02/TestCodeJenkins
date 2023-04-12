
Partial Class frmUserMstHistoryAuditTrail
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
            Page.Title = ":: User Master Audit Trail  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            CType(Me.Master.FindControl("Menu1"), Menu).Visible = False
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "User Master Audit Trail"

            Me.FillUserMstHistoryAuditTrailGrid()

        Catch ex As Exception
            Throw ex
        End Try

    End Sub
#End Region

#Region " FILL FUNCTIONS "

    Private Sub FillUserMstHistoryAuditTrailGrid()
        Dim wStr_UserMstHistoryAuditTrailGrid As String = String.Empty
        Dim dsUserMstHistoryAuditTrail As New DataSet
        Dim dc_UserMstHistoryAuditTrail As New DataColumn
        Try
            wStr_UserMstHistoryAuditTrailGrid = "iUserId=" + Me.ViewState(VS_UserID) + " ORDER BY dModifyOn"
            If Not objHelp.View_UserMstHistoryAuditTrail(wStr_UserMstHistoryAuditTrailGrid, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                       dsUserMstHistoryAuditTrail, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If
            dc_UserMstHistoryAuditTrail = New DataColumn("dModifyOn_IST", System.Type.GetType("System.String"))
            dsUserMstHistoryAuditTrail.Tables(0).Columns.Add("dModifyOn_IST")
            dsUserMstHistoryAuditTrail.AcceptChanges()
            For Each dr_UserMstHistory In dsUserMstHistoryAuditTrail.Tables(0).Rows
                dr_UserMstHistory("dModifyOn_IST") = Convert.ToString(dr_UserMstHistory("dModifyOn") + strServerOffset)
            Next
            dsUserMstHistoryAuditTrail.AcceptChanges()



            Me.gvwUserMstHistoryAuditTrail.DataSource = dsUserMstHistoryAuditTrail.Tables(0)
            Me.gvwUserMstHistoryAuditTrail.DataBind()
            Me.gvwUserMstHistoryAuditTrail.FooterRow.Visible = False

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

#End Region

#Region " GRID VIEW EVENTS "

    Protected Sub gvwUserMstHistoryAuditTrail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwUserMstHistoryAuditTrail.RowDataBound
        e.Row.Cells(GVC_UserID).Visible = False

        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (gvwUserMstHistoryAuditTrail.PageSize * gvwUserMstHistoryAuditTrail.PageIndex) + 1
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
