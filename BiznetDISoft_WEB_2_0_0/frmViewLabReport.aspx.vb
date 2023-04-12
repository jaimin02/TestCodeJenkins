
Partial Class frmViewLabReport
    Inherits System.Web.UI.Page
#Region "Variable Declaration"
    Private ObjCommon As New clsCommon
    Private Const VS_Choice As String = "Choice"
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Dim GVC_SrNo As Integer = 0
    Dim GVC_Normalflag As Integer = 6
    Dim GVC_Abnormalflag As Integer = 7
    Dim GVC_ClinicallySignflag As Integer = 8

#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Page.Title = ":: View Lab Report  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
        If Not Me.IsPostBack Then
            GenCall()
        End If
    End Sub

#Region "GenCall "

    Private Function GenCall() As Boolean

        If Not IsNothing(Me.Request.QueryString("SubjectId")) AndAlso _
            Me.Request.QueryString("SubjectId").ToString.Trim() <> "" Then

            Me.HSubjectId.Value = Me.Request.QueryString("SubjectId").ToString.Trim()

        End If

        If Me.HSubjectId.Value.Trim() = "" Then
            Me.rblScreeningDate.Items.Clear()
        End If

        If Not GenCall_Data() Then
            Exit Function
        End If

      

        Return True

    End Function

#End Region

#Region "GenCall_Data"
    Private Function GenCall_Data() As Boolean
        Dim ds As New DataSet
        Dim ds_LabReport As New DataSet
        Dim ds_SubMedExMst As New DataSet
        Dim ds_SubMedex As New DataSet
        Dim ds_SubDetail As New DataSet
        Dim estr_retu As String = ""
        Dim strQuery As String = ""
        Dim Wstr As String = ""

        Try

            If Not IsNothing(Me.Request.QueryString("mode")) Then
                Me.ViewState(VS_Choice) = CType(Me.Request.QueryString("mode"), WS_Lambda.DataObjOpenSaveModeEnum)
            End If
            If Not IsNothing(Me.Request.QueryString("SubjectId")) AndAlso _
                         (Me.Request.QueryString("SubjectId").ToString.Trim() <> "") Then

                Wstr = "vSubjectId='" & Me.HSubjectId.Value.Trim() & "'"
                If Not Me.objHelp.GetView_SubjectMaster(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                    ds_SubDetail, estr_retu) Then
                    Exit Function
                End If

                Me.txtSubject.Text = ds_SubDetail.Tables(0).Rows(0).Item("FullName").ToString.Trim() + " (" + Me.HSubjectId.Value.Trim() + ")"

                If Me.rblScreeningDate.Items.Count <= 0 Then
                    fillScreeningDates()
                End If



            End If

            If Me.rblScreeningDate.Items.Count > 0 AndAlso Me.rblScreeningDate.SelectedValue.ToUpper.Trim() <> "N" Then 'For Old

                Wstr = "'vSubjectId='" & Me.HSubjectId.Value.Trim() & "' and dScreenDate='" & CDate(Me.rblScreeningDate.SelectedValue.Trim()).ToString("dd-MMM-yyyy") & "'"

                If Not Me.objHelp.View_SubjectLabReportDetail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                    ds_LabReport, estr_retu) Then
                    Exit Function
                End If
                If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View And _
                  ds_LabReport.Tables(0).Rows.Count <= 0 AndAlso Me.HSubjectId.Value.Trim() <> "" Then

                    Me.ObjCommon.ShowAlert("No Screening Has Been Done For This Subject", Me.Page())

                    Exit Function
                End If
            End If
        Catch ex As Exception
            Me.ObjCommon.ShowAlert(ex.Message, Me.Page)
        End Try


    End Function
#End Region

#Region "Fill Grid"
    Private Function FillGrid() As Boolean
        Dim ds_LabReport As New Data.DataSet
       
        Dim estr As String = ""
        Dim wStr As String = ""
        Try
            If Me.rblScreeningDate.Items.Count > 0 AndAlso Me.rblScreeningDate.SelectedValue.ToUpper.Trim() <> "N" Then 'For Old

                wStr = " vSubjectId='" & Me.HSubjectId.Value.Trim() & "' and replace(convert(varchar(11),dScreenDate,113),' ','-')= '" & CDate(Me.rblScreeningDate.SelectedValue.Trim()).ToString("dd-MMM-yyyy") & "'"

                If Not Me.objHelp.View_SubjectLabReportDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                    ds_LabReport, estr) Then
                    Exit Function
                End If
            End If
           
            If ds_LabReport.Tables(0).Rows.Count < 1 Then
                ObjCommon.ShowAlert("No Record Found", Me.Page)
                Return True
            End If
            
            Me.GV_LabReport.DataSource = ds_LabReport
            Me.GV_LabReport.DataBind()



            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

   
#End Region

#Region "Button Click"

    Protected Sub btnSubject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubject.Click
        Try

            fillScreeningDates()

        Catch ex As Exception
            Me.ObjCommon.ShowAlert(ex.Message, Me.Page)
        End Try

    End Sub


    Private Function fillScreeningDates() As Boolean
        Dim ds_AuditTrail As New DataSet
        Dim dv_AuditTrail As New DataView
        Dim Wstr As String = ""
        Dim estr As String = ""

        Try
            Wstr = "vSubjectId='" & Me.HSubjectId.Value.Trim() & "'"
            If Not Me.objHelp.View_MedExScreeningHdrDtlAuditTrail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                             ds_AuditTrail, estr) Then

                Exit Function
            End If

            dv_AuditTrail = ds_AuditTrail.Tables(0).DefaultView().ToTable(True, "dScreenDate,vReviewBy".Split(",")).DefaultView()
            dv_AuditTrail.Sort = "dScreenDate desc"

            Me.rblScreeningDate.Items.Clear()
            For Each dr As DataRow In dv_AuditTrail.ToTable().Rows
                Me.rblScreeningDate.Items.Add(New ListItem(CDate(dr("dScreenDate")).ToString("dd-MMM-yyyy") + _
                                            IIf(dr("vReviewBy").ToString.Trim() = "", "", "(Reviewed By: " + dr("vReviewBy").ToString() + ")"), _
                                            dr("dScreenDate")))
            Next dr

            'Added for QC Comments

            '***********************

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False

        End Try

    End Function
#End Region

#Region "Other Events"

    Protected Sub GV_LabReport_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GV_LabReport.PageIndexChanging
        GV_LabReport.PageIndex = e.NewPageIndex
        FillGrid()
    End Sub

    Protected Sub rblScreeningDate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblScreeningDate.SelectedIndexChanged
        FillGrid()
    End Sub

    'added by Deepak after discussing with Naimesh Bhai to change N with NO and Y with Yes

    Protected Sub GV_LabReport_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV_LabReport.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + 1 + (Me.GV_LabReport.PageSize * Me.GV_LabReport.PageIndex)
            If e.Row.Cells(GVC_Normalflag).Text.ToUpper.Trim() = "N" Then
                e.Row.Cells(GVC_Normalflag).Text = "NO"
            ElseIf e.Row.Cells(GVC_Normalflag).Text.ToUpper.Trim() = "Y" Then
                e.Row.Cells(GVC_Normalflag).Text = "YES"
            End If

            If e.Row.Cells(GVC_Abnormalflag).Text.ToUpper.Trim() = "N" Then
                e.Row.Cells(GVC_Abnormalflag).Text = "NO"
            ElseIf e.Row.Cells(GVC_Abnormalflag).Text.ToUpper.Trim() = "Y" Then
                e.Row.Cells(GVC_Abnormalflag).Text = "YES"
            End If

            If e.Row.Cells(GVC_ClinicallySignflag).Text.ToUpper.Trim() = "N" Then
                e.Row.Cells(GVC_ClinicallySignflag).Text = "NO"
            ElseIf e.Row.Cells(GVC_ClinicallySignflag).Text.ToUpper.Trim() = "Y" Then
                e.Row.Cells(GVC_ClinicallySignflag).Text = "YES"
            End If

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
