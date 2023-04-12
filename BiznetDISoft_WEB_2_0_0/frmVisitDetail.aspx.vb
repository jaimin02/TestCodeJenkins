
Partial Class frmVisitDetail
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION "

    Private Const VS_Choice As String = "Choice"
    Private Const VS_WorkspaceSubjectMst As String = "dtWorkspaceSubjectMst"

    Private Const VS_WorkspaceId As String = "vWorkspaceId"
    Private Const VS_WorkspaceName As String = "vWorkspaceDesc"
    Private Const VS_SubjectId As String = "vSubjectId"
    Private Const VS_MySubjectNo As String = "iMySubjectNo"
    Private Const VS_Initial As String = "vInitial"
    Private Const VS_RandomizationNo As String = "vRandomizationNo"

    Private Const GVC_Period As Integer = 0
    Private Const GVC_Visit As Integer = 1
    Private Const GVC_cPresent As Integer = 2
    Private Const GVC_Present As Integer = 3

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()


    Dim eStr_Retu As String

#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            GenCall()
        End If
    End Sub

#End Region

#Region "GENCALL"

    Private Function GenCall() As Boolean

        Dim eStr As String = ""
        Dim wStr As String = ""
        Dim dt_WorkspaceSubjectmst As DataTable = Nothing
        Dim ds_WorkspaceSubjectmst As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Not IsNothing(Me.Request.QueryString("WorkspaceId")) AndAlso Me.Request.QueryString("WorkspaceId").Trim() <> "" Then
                Me.ViewState(VS_WorkspaceId) = Me.Request.QueryString("WorkspaceId").Trim()
            End If

            If Not IsNothing(Me.Request.QueryString("SubjectId")) AndAlso Me.Request.QueryString("SubjectId").Trim() <> "" Then
                Me.ViewState(VS_SubjectId) = Me.Request.QueryString("SubjectId").Trim()
            End If

            If Not IsNothing(Me.Request.QueryString("MySubjectNo")) AndAlso Me.Request.QueryString("MySubjectNo").Trim() <> "" Then
                Me.ViewState(VS_MySubjectNo) = Me.Request.QueryString("MySubjectNo").Trim()
            End If

            If Not IsNothing(Me.Request.QueryString("RandomizationNo")) AndAlso Me.Request.QueryString("RandomizationNo").Trim() <> "" Then
                Me.ViewState(VS_RandomizationNo) = Me.Request.QueryString("RandomizationNo").Trim()
            End If

            ''''Check for Valid User''''''''''''''
            If Not GenCall_Data(Choice, dt_WorkspaceSubjectmst) Then ' For Data Retrieval
                Exit Function
            End If
            Me.ViewState(VS_WorkspaceSubjectMst) = dt_WorkspaceSubjectmst ' adding blank DataTable in viewstate
            If Not GenCall_ShowUI(Choice, dt_WorkspaceSubjectmst) Then 'For Displaying Data 
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
        Dim ds_WorkspaceSubjectMst As DataSet = Nothing
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            'If Choice_1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
            wStr = "1=2"
            'Else
            '    wStr = "vWorkspaceSubjectId=" + Me.ViewState(VS_WorkspaceSubjectId).ToString() 'Value of where condition
            'End If

            If Not objHelp.GetWorkspaceSubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_WorkspaceSubjectMst, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_WorkspaceSubjectMst Is Nothing Then
                Throw New Exception(eStr)
            End If
            'If ds_WorkspaceSubjectMst.Tables(0).Rows.Count <= 0 And _
            '   Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

            '    Throw New Exception("No Records Found for Selected role")

            'End If
            dt_Dist_Retu = ds_WorkspaceSubjectMst.Tables(0)
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function

#End Region

#Region "GENCALL_SHOW_UI"

    Private Function GenCall_ShowUI(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, ByVal dt_WorkspaceSubjectmst As DataTable) As Boolean
      
        CType(Master.FindControl("lblHeading"), Label).Text = "Visit Detail"
        Page.Title = ":: Visit Detail   :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
        If Not FillGridView() Then
            GenCall_ShowUI = False
        End If

        If Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
        End If

        GenCall_ShowUI = True
    End Function

#End Region

#Region "FillGridView"

    Private Function FillGridView() As Boolean
        Dim WorkspaceId As String = ""
        Dim SubjectId As String = ""
        Dim Wstr As String = ""
        Dim dsWorkspace As New DataSet
        Dim dsSubject As New DataSet
        Dim dsWorkspaceSubject As New DataSet
        Dim dvWorkspaceSubject As New DataView
        Dim dtWorkspace As New DataTable
        Try

            If Not IsNothing(Me.Request.QueryString("WorkspaceId")) Then
                WorkspaceId = Me.Request.QueryString("WorkspaceId").Trim()
            End If

            If Not IsNothing(Me.Request.QueryString("SubjectId")) Then
                SubjectId = Me.Request.QueryString("SubjectId").Trim()
            End If

            If WorkspaceId.Trim() <> "" And SubjectId.Trim() <> "" Then

                Wstr = "vWorkspaceId='" & WorkspaceId.Trim() & "' And vSubjectId='" & SubjectId.Trim() & "'"
                Wstr += " And cRejectionFlag <> 'Y'"

                If Not Me.objHelp.GetViewWorkspaceSubjectMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                            dsWorkspaceSubject, eStr_Retu) Then
                    Me.ShowErrorMessage(eStr_Retu, "")
                    FillGridView = False
                    Exit Function
                End If

            End If

            Wstr = "vWorkspaceId='" & WorkspaceId.Trim() & "'"
            If Not Me.objHelp.GetViewWorkSpaceNodeDetail(Wstr, dsWorkspace, eStr_Retu) Then
                Me.ShowErrorMessage(eStr_Retu, "")
                FillGridView = False
                Exit Function
            End If

            dtWorkspace = dsWorkspace.Tables(0).Copy().DefaultView.ToTable(True, "iPeriod")

            'For Checking whether subject is present or not
            dtWorkspace.Columns.Add("cPresent", GetType(String))
            dtWorkspace.Columns.Add("vPresentOn", GetType(String))
            dtWorkspace.AcceptChanges()

            For Each dr As DataRow In dtWorkspace.Rows

                dr("cPresent") = "N"
                dr("vPresentOn") = ""

                If dsWorkspaceSubject.Tables(0).Rows.Count > 0 Then

                    dvWorkspaceSubject = Nothing
                    dvWorkspaceSubject = New DataView

                    dvWorkspaceSubject = dsWorkspaceSubject.Tables(0).Copy().DefaultView
                    dvWorkspaceSubject.RowFilter = "iPeriod=" & dr("iPeriod")

                    If dvWorkspaceSubject.ToTable.Rows.Count > 0 Then
                        dr("cPresent") = "Y"
                        dr("vPresentOn") = dvWorkspaceSubject.ToTable.Rows(0).Item("dReportingDate")
                    End If

                End If

            Next dr
            dtWorkspace.AcceptChanges()
            '************************************************************

            Wstr = "vSubjectId='" & SubjectId.Trim() & "' And cRejectionFlag <> 'Y'"
            If Not Me.objHelp.GetView_SubjectMaster(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        dsSubject, eStr_Retu) Then
                Me.ShowErrorMessage(eStr_Retu, "")
                FillGridView = False
                Exit Function

            End If

            Me.ViewState(VS_WorkspaceName) = dsWorkspace.Tables(0).Rows(0).Item("vWorkSpaceDesc")

            Me.lblSubject.Text = "<br/>" & "Project: " & dsWorkspace.Tables(0).Rows(0).Item("vWorkSpaceDesc") & "<br/>" & _
                                 "Subject: " & dsSubject.Tables(0).Rows(0).Item("FullName") & "<br/>" & _
                                 "Subject Id: " & dsSubject.Tables(0).Rows(0).Item("vSubjectId") & "<br/>" & _
                                 "Initial: " & dsSubject.Tables(0).Rows(0).Item("vInitials") & "<br/>" & "<br/>"

            Me.ViewState(VS_Initial) = dsSubject.Tables(0).Rows(0).Item("vInitials").ToString.Trim()

            Me.GVVisits.DataSource = dtWorkspace
            Me.GVVisits.DataBind()

            FillGridView = True
        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region

#Region "Reset Page"
    Private Sub ResetPage()
        Me.lblSubject.Text = ""
        FillGridView()
        'Response.Redirect("frmWorkspaceSubjectMst.aspx?mode=1")
    End Sub
#End Region

#Region "Grid Events"

    Protected Sub GVVisits_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVVisits.RowCreated

        e.Row.Cells(GVC_cPresent).Visible = False
        e.Row.Cells(GVC_Period).Visible = False

    End Sub

    Protected Sub GVVisits_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVVisits.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            CType(e.Row.FindControl("lnkPresent"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkPresent"), LinkButton).CommandName = "Present"

            CType(e.Row.FindControl("lnkView"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkView"), LinkButton).CommandName = "View"


            'Added on 10-Jul-2009
            CType(e.Row.FindControl("lblVisit"), Label).Text = "Visit " + CType(e.Row.FindControl("lblVisit"), Label).Text
            CType(e.Row.FindControl("lnkPresent"), LinkButton).Text = "Visit " + CType(e.Row.FindControl("lnkPresent"), LinkButton).Text

            '*******************************************

            CType(e.Row.FindControl("lnkPresent"), LinkButton).Visible = True
            CType(e.Row.FindControl("lblVisit"), Label).Visible = False
            CType(e.Row.FindControl("lnkView"), LinkButton).Visible = False

            If e.Row.Cells(GVC_cPresent).Text.ToUpper.Trim() = "Y" Then
                CType(e.Row.FindControl("lnkPresent"), LinkButton).Visible = False
                CType(e.Row.FindControl("lblVisit"), Label).Visible = True
                CType(e.Row.FindControl("lnkView"), LinkButton).Visible = True
            End If

            'Added on 10-Jul-2009
            If e.Row.RowIndex > 0 Then

                If Me.GVVisits.Rows(e.Row.RowIndex - 1).Cells(GVC_cPresent).Text.ToUpper.Trim() <> "Y" Then
                    CType(e.Row.FindControl("lnkPresent"), LinkButton).Visible = False
                    CType(e.Row.FindControl("lblVisit"), Label).Visible = True
                    CType(e.Row.FindControl("lnkView"), LinkButton).Visible = False
                End If

            End If
            '*******************************

        End If

    End Sub

    Protected Sub GVVisits_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GVVisits.RowCommand
        Dim Index As Integer = CType(e.CommandArgument, Integer)
        Dim RedirectStr As String = ""
        Try

            If e.CommandName.ToUpper.Trim() = "PRESENT" Then

                If Not Me.AssignValues(Me.GVVisits.Rows(Index).Cells(GVC_Period).Text.Trim()) Then
                    Exit Sub
                End If
                If Not Me.FillGridView() Then
                    Exit Sub
                End If

            ElseIf e.CommandName.ToUpper.Trim() = "VIEW" Then

                RedirectStr = "window.open(""" + "frmProjectDetailMst.aspx?WorkSpaceId=" & Me.Request.QueryString("WorkspaceId").Trim() & _
                              "&Page=frmVisitDetail&Type=OPERATIONAL&Period=" & Me.GVVisits.Rows(Index).Cells(GVC_Period).Text & """)"

                'Added on 17-Sep-2009 by Chandresh Vanker For CTM Flow
                If Not IsNothing(Me.ViewState(VS_SubjectId)) AndAlso Me.ViewState(VS_SubjectId) <> "" Then
                    RedirectStr = "window.open(""" + "frmProjectDetailMst.aspx?WorkSpaceId=" & Me.Request.QueryString("WorkspaceId").Trim() & _
                                  "&Page=frmVisitDetail&Type=OPERATIONAL&Period=" & Me.GVVisits.Rows(Index).Cells(GVC_Period).Text & _
                                  "&SubjectId=" & Me.ViewState(VS_SubjectId) & "&MySubjectNo=" & Me.ViewState(VS_MySubjectNo) & """)"
                End If
                '**************************************
                ScriptManager.RegisterClientScriptBlock(Me.Page(), Me.GetType(), "OpenWindow", RedirectStr, True)

            End If

        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "AssignValues"

    Private Function AssignValues(ByVal Period As String) As Boolean
        Dim estr As String = ""
        Dim dr As DataRow
        Dim dsWorkspaceSubjectMst As New DataSet
        Try

            dsWorkspaceSubjectMst.Tables.Add(CType(Me.ViewState(VS_WorkspaceSubjectMst), DataTable).Copy())

            dr = dsWorkspaceSubjectMst.Tables(0).NewRow()

            'vWorkspaceSubjectId,vWorkspaceId,iMySubjectNo,vSubjectId,vInitials,iPeriod,dReportingDate,
            'cRejectionFlag,nReasonNo,iModifyBy,dModifyOn,cStatusIndi

            dr("vWorkspaceSubjectId") = 0
            dr("vWorkspaceid") = Me.ViewState(VS_WorkspaceId)
            dr("iMySubjectNo") = 0
            If Not IsNothing(Me.ViewState(VS_MySubjectNo)) Then
                dr("iMySubjectNo") = Me.ViewState(VS_MySubjectNo)
            End If
            'Added by Chandresh Vanker on 17-Sep-2009
            dr("vRandomizationNo") = 0
            If Not IsNothing(Me.ViewState(VS_RandomizationNo)) Then
                dr("vRandomizationNo") = Me.ViewState(VS_RandomizationNo)
            End If
            '***************************
            dr("vSubjectId") = Me.ViewState(VS_SubjectId)
            dr("vInitials") = Me.ViewState(VS_Initial)
            dr("iPeriod") = Period
            dr("dReportingDate") = Now.Date.ToString()
            dr("cRejectionFlag") = "N"
            'dr("nReasonNo") = ""
            dr("iModifyBy") = Me.Session(S_UserID)

            dsWorkspaceSubjectMst.Tables(0).Rows.Add(dr)
            dsWorkspaceSubjectMst.Tables(0).TableName = "VIEW_WORKSPACESUBJECTMST"
            dsWorkspaceSubjectMst.AcceptChanges()

            If Not Me.objLambda.Save_InsertWorkspaceSubjectMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                                WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_WorkspaceSubjectMst, dsWorkspaceSubjectMst, _
                                Me.Session(S_UserID), estr) Then
                ObjCommon.ShowAlert("Error while Assigning", Me.Page)
                Exit Function
            End If

            ObjCommon.ShowAlert("Subject Assigned Successfully.", Me)

            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

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

#Region "Button Events"

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If Not IsNothing(Me.Request.QueryString("Page2")) AndAlso Me.Request.QueryString("Page2").Trim() <> "" Then
            Me.Response.Redirect(Me.Request.QueryString("page2") + ".aspx?WorkspaceId=" + Me.Request.QueryString("WorkspaceId"))
        End If

        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub lnkUnScheculed_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkUnScheculed.Click
        Me.Response.Redirect("frmEditWorkspaceNodeDetail.aspx?SubjectId=" + Me.Request.QueryString("SubjectId").Trim() + _
                             "&WorkspaceId=" + Me.Request.QueryString("WorkspaceId").Trim() + _
                             "&WorkspaceName=" + Me.ViewState(VS_WorkspaceName).ToString.Trim())

    End Sub

#End Region

End Class
