
Partial Class frmCRFReviewHistory
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_WorkFlowStageId As Integer = 1
    Private Const GVC_StatusChangedBy As Integer = 2
    Private Const GVC_StatusChangedOn As Integer = 3

#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Page.Title = " :: CRF Review History ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
        If Not Me.IsPostBack() Then

            If Not Me.FillGrid() Then
                Exit Sub
            End If

        End If

    End Sub

#End Region

#Region "Fill Grid"

    Private Function FillGrid() As Boolean
        Dim ds As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim dc As New DataColumn
        Dim bf As BoundField
        Dim Ds_ActulTime As New DataSet


        Try
            wStr = "nCRFDtlNo = " + Convert.ToString(Me.Request.QueryString("CRFDtlNo")).Trim() + " Order by iTranNo"
            If Not Me.objHelp.View_CRFWorkFlowDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                  ds, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds.Tables(0) Is Nothing Then
                If ds.Tables(0).Rows.Count > 0 Then
                    Me.lblActivity.Text = "Activity : " + Convert.ToString(ds.Tables(0).Rows(0)("vNodeDisplayName")).Trim()
                End If
            End If

            'dc = New DataColumn("ActualTIME", System.Type.GetType("System.toString"))
            'ds.Tables(0).Columns.Add(dc)
            'ds.AcceptChanges()

            If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then

                ds.Tables(0).Columns.Add("ActualTime", Type.GetType("System.String"))
                For Each dr In ds.Tables(0).Rows
                    dr("ActualTime") = Convert.ToString(CDate(dr("dModifyOn")).ToString("dd-MMM-yyyy HH:mm"))
                    If Not objHelp.Proc_ActualAuditTrailTime(CDate(dr("dModifyOn")).ToString("dd-MMM-yyyy HH:mm") + "##" + Session(S_LocationCode), Ds_ActulTime, eStr) Then '' Added by Dipen shah on 3-dec-2014
                        Throw New Exception(eStr)
                    End If
                    dr("ActualTime") = CDate(Ds_ActulTime.Tables(0).Rows(0).Item("ActualTIME")).ToString("dd-MMM-yyyy HH:mm") + " (" + Ds_ActulTime.Tables(0).Rows(0).Item("TimeZoneOffSet").ToString + " GMT)"
                Next
                bf = New BoundField
                bf.DataField = ds.Tables(0).Columns("ActualTIME").ToString()
                bf.HeaderText = Session(S_TimeZoneName)
                GVWReviewHistory.Columns.Add(bf)
            Else

                objCommon.ShowAlert("No records found.", Me.Page)
                Exit Function
            End If

            ds.Tables(0).Columns.Add(dc)
            ds.AcceptChanges()

            Me.GVWReviewHistory.DataSource = ds
            Me.GVWReviewHistory.DataBind()
            Me.lblCount.Text = "No. Of Records Found : " + Me.GVWReviewHistory.Rows.Count.ToString()
            Return True

        Catch ex As Exception
            objCommon.ShowAlert("Error While Filling Grid. " + ex.Message, Me.Page)
            Return False
        End Try

    End Function

#End Region

#Region "Grid Events"

    Protected Sub GVWReviewHistory_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVWReviewHistory.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            If Not Convert.ToString(e.Row.Cells(GVC_StatusChangedOn).Text).Trim = "" Then
                e.Row.Cells(GVC_StatusChangedOn).Text = e.Row.Cells(GVC_StatusChangedOn).Text.ToString + " " + strServerOffset
            End If

            ''commented by nipun khant for dynamic reviews
            'If Convert.ToString(e.Row.Cells(GVC_WorkFlowStageId).Text).Trim() = WorkFlowStageId_DataEntry Then
            '    e.Row.Cells(GVC_WorkFlowStageId).Text = "Data Entry"
            'ElseIf Convert.ToString(e.Row.Cells(GVC_WorkFlowStageId).Text).Trim() = WorkFlowStageId_FirstReview Then
            '    e.Row.Cells(GVC_WorkFlowStageId).Text = "First Review"
            'ElseIf Convert.ToString(e.Row.Cells(GVC_WorkFlowStageId).Text).Trim() = WorkFlowStageId_SecondReview Then
            '    e.Row.Cells(GVC_WorkFlowStageId).Text = "Second Review"
            'ElseIf Convert.ToString(e.Row.Cells(GVC_WorkFlowStageId).Text).Trim() = WorkFlowStageId_FinalReviewAndLock Then
            '    e.Row.Cells(GVC_WorkFlowStageId).Text = "Final Review"
            'End If
        End If
    End Sub

#End Region

End Class
