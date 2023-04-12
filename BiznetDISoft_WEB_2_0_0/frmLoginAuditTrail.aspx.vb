
Partial Class frmLoginAuditTrail
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

#Region "PageLoad Event"
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
#End Region

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
            Page.Title = ":: Login Audit Trial :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("Menu1"), Menu).Visible = False
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "User Login Audit Trail"
            Me.rdoMonth.Checked = True
            Me.FillUserLoginAuditTrailGrid()

        Catch ex As Exception
            Throw ex
        End Try

    End Sub
#End Region

#Region " FILL FUNCTIONS "

    Private Sub FillUserLoginAuditTrailGrid()
        Dim wStr_UserLoginAuditTrailGrid As String = String.Empty
        Dim dsUserLoginAuditTrail As New DataSet
        Dim dvUserLoginAuditTrail As New DataView
        Dim dtUserLoginAuditTrail As New DataTable

        Dim dc As DataColumn
        Try
            wStr_UserLoginAuditTrailGrid = Me.GetWhereCondition()
            'wStr_UserLoginAuditTrailGrid = "nUserId=" + Me.ViewState(VS_UserID) + " ORDER BY dInOutDateTime"
            If Not objHelp.View_UserLoginAuditTrail(wStr_UserLoginAuditTrailGrid, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                       dsUserLoginAuditTrail, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            End If
            dc = New DataColumn("dInOutDateTime_IST", System.Type.GetType("System.String"))
            dsUserLoginAuditTrail.Tables(0).Columns.Add(dc)
            For Each dr_AuditDate In dsUserLoginAuditTrail.Tables(0).Rows

                dr_AuditDate("dInOutDateTime_IST") = Convert.ToString(CDate(dr_AuditDate("dInOutDateTime")).ToString("dd-MMM-yyyy hh:mm") + strServerOffset)
                'dr_AuditDate("dInOutDateTime_IST") = Convert.ToString(dr_AuditDate("dInOutDateTime") + strServerOffset)
            Next
            dsUserLoginAuditTrail.AcceptChanges()
            dvUserLoginAuditTrail = dsUserLoginAuditTrail.Tables(0).DefaultView
            dtUserLoginAuditTrail = dvUserLoginAuditTrail.ToTable(True, "nUserLoginHistoryNo,iUserId,vLoginName,vFirstName,vLastName,cLOFlag,vLOFlag,dInOutDateTime_IST".Split(","))
            
        
            dtUserLoginAuditTrail.AcceptChanges()
            Me.gvwLoginAuditTrail.DataSource = dtUserLoginAuditTrail





            'Me.gvwLoginAuditTrail.DataSource = dsUserLoginAuditTrail.Tables(0)
            Me.gvwLoginAuditTrail.DataBind()

            If gvwLoginAuditTrail.Rows.Count <= 0 Then
                Me.lblMessage.Text = "No Login Details Found !!!"
            Else
                Me.lblMessage.Text = ""
                Me.gvwLoginAuditTrail.FooterRow.Visible = False
            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Function GetWhereCondition() As String
        GetWhereCondition = String.Empty
        Dim svrDateTime As String = String.Empty

        Try
            svrDateTime = objHelp.GetServerDateTime() 

            If Me.rdoMonth.Checked Then 'This is for one month
                GetWhereCondition = "dInOutDateTime >= '" + DateTime.Parse(svrDateTime).AddMonths(-1).ToString("dd-MMM-yyyy") + "'"
                GetWhereCondition += " AND dInOutDateTime <='" + DateTime.Parse(svrDateTime).AddDays(1).ToString("dd-MMM-yyyy") + "'"
            ElseIf Me.rdoQuarter.Checked Then
                GetWhereCondition = "dInOutDateTime >= '" + DateTime.Parse(svrDateTime).AddMonths(-3).ToString("dd-MMM-yyyy") + "'"
                GetWhereCondition += " AND dInOutDateTime <='" + DateTime.Parse(svrDateTime).AddDays(1).ToString("dd-MMM-yyyy") + "'"
            ElseIf Me.rdoYear.Checked Then
                GetWhereCondition = "dInOutDateTime >= '" + DateTime.Parse(svrDateTime).AddYears(-1).ToString("dd-MMM-yyyy") + "'"
                GetWhereCondition += " AND dInOutDateTime <='" + DateTime.Parse(svrDateTime).AddDays(1).ToString("dd-MMM-yyyy") + "'"
            End If


            If GetWhereCondition.Trim.Length > 0 Then
                GetWhereCondition += " AND "
            End If
            GetWhereCondition += " iUserID=" + Me.ViewState(VS_UserID) + " ORDER BY nUserLoginHistoryNo"

        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region " GRID VIEW EVENTS "

    Protected Sub gvwLoginAuditTrail_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvwLoginAuditTrail.PageIndexChanging
        gvwLoginAuditTrail.PageIndex = e.NewPageIndex
        Me.FillUserLoginAuditTrailGrid()

    End Sub


    Protected Sub gvwPasswordHistoryAuditTrail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwLoginAuditTrail.RowDataBound
        e.Row.Cells(GVC_UserID).Visible = False

        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (gvwLoginAuditTrail.PageSize * gvwLoginAuditTrail.PageIndex) + 1
        End If
    End Sub

#End Region

#Region " RADIO BUTTON EVENTS "
    Protected Sub rdoMonth_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoMonth.CheckedChanged, rdoQuarter.CheckedChanged, rdoYear.CheckedChanged
        Try
            Me.FillUserLoginAuditTrailGrid()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
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
