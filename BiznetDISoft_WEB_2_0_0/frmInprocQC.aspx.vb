
Partial Class frmInprocQC
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private eStr_Retu As String = ""

    Private Const GVCell_SubjectNo As Integer = 0
    Private Const GVCell_SubjectId As Integer = 1
    Private Const GVCell_SubjectName As Integer = 2
    Private Const GVCell_NoOfComm As Integer = 3
    Private Const GVCell_NoOfResp As Integer = 4
    Private Const GVCell_QC As Integer = 5
    Private Const GVCell_iMySubjectNo As Integer = 6

#End Region

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            GenCall()
        End If
    End Sub
#End Region

#Region "GENCALL"
    Private Function GenCall() As Boolean

        Dim dt_WorkspaceSubjectmst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try



            Choice = Me.Request.QueryString("Mode")

            If Not GenCall_ShowUI(Choice, dt_WorkspaceSubjectmst) Then 'For Displaying Data 
                Exit Function
            End If
            GenCall = True


        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....GenCall")
        Finally
        End Try
    End Function
#End Region

#Region "GENCALL_SHOW_UI"

    Private Function GenCall_ShowUI(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, ByVal dt_WorkspaceSubjectmst As DataTable) As Boolean
        Dim dsMedexInfo As New DataSet
        Dim WorkspaceId As String = String.Empty
        Dim ActId As String = String.Empty
        Dim Act As String = String.Empty
        Dim Period As String = String.Empty
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty
        Page.Title = " :: InProcess Quality Check ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
        If Not IsNothing(Me.Request.QueryString("workspaceid")) Then

            Me.HFWorkspaceId.Value = Me.Request.QueryString("workspaceid").Trim()
            Me.HFActivityId.Value = Me.Request.QueryString("ActId").Trim()
            Me.HFNodeId.Value = Me.Request.QueryString("NodeId").ToString
            Act = Me.Request.QueryString("Act").Trim()
            Me.HFPeriodId.Value = Me.Request.QueryString("Period").Trim()

        End If

        If Not FillGridView(dsMedexInfo) Then
            GenCall_ShowUI = False
            Exit Function
        End If

        Me.lblHeader.Text = "No Subjects Found For " + "Activity: " + Act + _
                                                    "<br />And Period: " + Me.HFPeriodId.Value.Trim()

        If dsMedexInfo.Tables(0).Rows.Count > 0 Then

            Me.lblHeader.Text = "Project: " + dsMedexInfo.Tables(0).Rows(0).Item("vWorkSpaceDesc").ToString.Trim() + _
                                                    "<br />Activity: " + Act + _
                                                    "<br />Period: " + Me.HFPeriodId.Value.Trim()

        End If

        GenCall_ShowUI = True

    End Function

#End Region

#Region "FillGridView"

    Private Function FillGridView(ByRef ds_Subject As DataSet) As Boolean
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty
        Try

            ' Changed by Pratiksha To have subjects whoes imysubjectno > 0
            'Wstr = "cActiveFlag<>'N' and vWorkSpaceId='" & Me.HFWorkspaceId.Value.Trim() & _
            '                    "' And iNodeId=" & Me.HFNodeId.Value.Trim() & " and vActivityId='" & _
            '                    Me.HFActivityId.Value.Trim() & "' and iPeriod=" & _
            '                    Me.HFPeriodId.Value.Trim() & " Order by iMySubjectNo"

            'Wstr = "cActiveFlag<>'N' and vWorkSpaceId='" & Me.HFWorkspaceId.Value.Trim() & _
            '                    "' And iNodeId=" & Me.HFNodeId.Value.Trim() & " and vActivityId='" & _
            '                    Me.HFActivityId.Value.Trim() & "' and iPeriod=" & _
            '                    Me.HFPeriodId.Value.Trim() & " And iMySubjectNo > 0 Order by iMySubjectNo"
            ''--Pratiksha

            'If Not Me.objHelp.View_CRFHdrDtlSubDtl_Edit(Wstr, "iMySubjectNo,vSubjectId,vSubjectName,vModifyBy,dStartDate,vWorkSpaceDesc", _
            '                                ds_Subject, estr) Then

            '    Me.ShowErrorMessage(estr, "")
            '    FillGridView = False
            '    Exit Function

            'End If

            Wstr = "vWorkSpaceId='" + Me.HFWorkspaceId.Value.Trim() + "' and iPeriod=" + Me.HFPeriodId.Value.Trim()


            'Wstr += " and cStatusindi <>'D' and cRejectionflag <>'Y' order by iMySubjectNo,dReportingDate"
            Wstr += " and cStatusindi <>'D' order by iMySubjectNo,dReportingDate"
            '*********************

            If Not objHelp.GetWorkspaceSubjectMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                            ds_Subject, estr) Then

                Return False

            End If



            If ds_Subject.Tables(0).Rows.Count > 0 Then

                Me.gvSubjectQC.DataSource = ds_Subject
                Me.gvSubjectQC.DataBind()

            End If

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region

#Region "Grid Event"

    Protected Sub gvSubjectQC_PreRender(sender As Object, e As EventArgs) Handles gvSubjectQC.PreRender
        If gvSubjectQC.Rows.Count > 0 Or Not gvSubjectQC.DataSource Is Nothing Then
            gvSubjectQC.UseAccessibleHeader = True
            gvSubjectQC.HeaderRow.TableSection = TableRowSection.TableHeader
        End If
    End Sub


    Protected Sub gvSubjectQC_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSubjectQC.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or _
            e.Row.RowType = DataControlRowType.Footer Or _
            e.Row.RowType = DataControlRowType.Header Then



            ' Added by Pratiksha
            e.Row.Cells(GVCell_NoOfComm).Visible = False
            e.Row.Cells(GVCell_NoOfResp).Visible = False
            'Pratiksha

            e.Row.Cells(GVCell_iMySubjectNo).Visible = False

            If Not IsNothing(Me.Request.QueryString("QC")) AndAlso Me.Request.QueryString("QC") = "Y" Then

                'e.Row.Cells(GVCell_Date).Visible = True
                'e.Row.Cells(GVCell_User).Visible = True

            End If

        End If

    End Sub

    Protected Sub gvSubjectQC_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSubjectQC.RowDataBound
        Dim ds As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim dv As New DataView

        If e.Row.RowType = DataControlRowType.Header Then

            e.Row.Cells(GVCell_QC).Text = "View"
            'Added on 03-Oct-2009
            If Not IsNothing(Me.Request.QueryString("QC")) AndAlso Me.Request.QueryString("QC") = "Y" Then

                e.Row.Cells(GVCell_QC).Text = "QC/QA"

            End If

        End If

        If e.Row.RowType = DataControlRowType.DataRow Then

            CType(e.Row.FindControl("LnkQC"), LinkButton).Text = "View"
            CType(e.Row.FindControl("LnkQC"), LinkButton).CommandName = "VIEW"
            CType(e.Row.FindControl("LnkQC"), LinkButton).CommandArgument = e.Row.RowIndex

            'Added on 03-Oct-2009
            If Not IsNothing(Me.Request.QueryString("QC")) AndAlso Me.Request.QueryString("QC") = "Y" Then

                CType(e.Row.FindControl("LnkQC"), LinkButton).Text = "QC"
                CType(e.Row.FindControl("LnkQC"), LinkButton).CommandName = "QC"
                CType(e.Row.FindControl("LnkQC"), LinkButton).CommandArgument = e.Row.RowIndex

            End If


            '***********************************
            wStr = "vWorkSpaceId = '" + Me.HFWorkspaceId.Value.Trim() + "'"
            wStr += " And iNodeId = " + Me.HFNodeId.Value.Trim() + ""
            wStr += " And vSubjectId = '" + e.Row.Cells(GVCell_SubjectId).Text.Trim() + "'"
            wStr += " And cStatusIndi <> 'D'"

            If Not objHelp.View_MedexInfoHdrQc(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr) Then
                ObjCommon.ShowAlert("Error While Getting Data from MedExInfoHdr", Me.Page)
                Exit Sub
            End If

            If ds.Tables(0).Rows.Count > 0 And (Not IsNothing(Me.Request.QueryString("QC")) AndAlso Me.Request.QueryString("QC") = "Y") Then
                'For Comments
                e.Row.BackColor = Drawing.Color.DarkOrange
                e.Row.Cells(GVCell_NoOfComm).Text = ds.Tables(0).Rows.Count

                'For Response
                dv = Nothing
                dv = New DataView
                dv = ds.Tables(0).Copy().DefaultView
                dv.RowFilter = "vResponse is NOT NULL"
                e.Row.Cells(GVCell_NoOfResp).Text = dv.ToTable().Rows.Count
            ElseIf ds.Tables(0).Rows.Count > 0 And (Not IsNothing(Me.Request.QueryString("QC")) AndAlso Me.Request.QueryString("QC") = "Y") Then

                e.Row.Cells(GVCell_NoOfComm).Text = "0"
                e.Row.Cells(GVCell_NoOfResp).Text = "0"


            End If



        End If

    End Sub

    Protected Sub gvSubjectQC_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSubjectQC.RowCommand
        Dim index As Integer = e.CommandArgument
        Dim RedirectStr As String

        If e.CommandName.ToUpper = "QC" Then

            RedirectStr = "window.open(""" + "frmMedExInfoHdrDtl.aspx?mode=4&QC=Y&workspaceid=" & Me.HFWorkspaceId.Value.Trim() & _
                                "&ActivityId=" & Me.HFActivityId.Value.Trim() & "&NodeId=" & Me.HFNodeId.Value.Trim() & _
                                "&PeriodId=" & Me.HFPeriodId.Value.Trim() & "&SubjectId=" & Me.gvSubjectQC.Rows(index).Cells(GVCell_SubjectId).Text.Trim() & _
                                "&MySubjectNo=" & Me.gvSubjectQC.Rows(index).Cells(GVCell_iMySubjectNo).Text.Trim() + """)"
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", RedirectStr, True)

        ElseIf e.CommandName.ToUpper = "VIEW" Then

            RedirectStr = "window.open(""" + "frmCTMMedExInfoHdrDtl.aspx?mode=4&Type=BA-BE&workspaceid=" & Me.HFWorkspaceId.Value.Trim() & _
                                "&ActivityId=" & Me.HFActivityId.Value.Trim() & "&NodeId=" & Me.HFNodeId.Value.Trim() & _
                                "&PeriodId=" & Me.HFPeriodId.Value.Trim() & "&SubjectId=" & Me.gvSubjectQC.Rows(index).Cells(GVCell_SubjectId).Text.Trim() & _
                                "&MySubjectNo=" & Me.gvSubjectQC.Rows(index).Cells(GVCell_iMySubjectNo).Text.Trim() + """)"

            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", RedirectStr, True)


        End If

    End Sub


    Protected Sub gvSubjectQC_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSubjectQC.PageIndexChanging

        Dim DsFill As New DataSet
        Me.gvSubjectQC.PageIndex = e.NewPageIndex
        FillGridView(DsFill)

    End Sub

#End Region

#Region "Button Event"
    Protected Sub BtnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnBack.Click
        Dim WorkspaceId As String = String.Empty

        WorkspaceId = Me.HFWorkspaceId.Value.Trim()
        Me.Response.Redirect(Me.Request.QueryString("Page2") & ".aspx?Type=" & Me.Request.QueryString("Type") & "&WorkSpaceId=" & WorkspaceId.Trim() & _
                            "&page=" & Me.Request.QueryString("Page"))
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
