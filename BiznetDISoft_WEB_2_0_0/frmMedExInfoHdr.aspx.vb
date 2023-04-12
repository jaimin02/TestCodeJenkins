Imports System.Collections.Generic

Partial Class frmMedExInfoHdr
    Inherits System.Web.UI.Page

#Region " VARIABLE DECLARATION "

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtMedExInfoHdr As String = "DtMedExInfoHdr"
    Private Const VS_CaseRecordId As String = "vCaseRecordId"

    Dim ObjCommon As New clsCommon
    Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()


    Dim eStr_Retu As String

#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            GenCall()
        End If
    End Sub
#Region " GenCall() "
    Private Function GenCall() As Boolean

        Dim dtMedExInfoHdr_Retu As New DataTable()
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum 'Query String to decide Add or Edit Mode

        Try
            Choice = CType("1", WS_Lambda.DataObjOpenSaveModeEnum)

            Me.ViewState(VS_Choice) = Choice

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_CaseRecordId) = Request.QueryString("Value").Trim.ToString()
            End If

            If Not GenCall_Data(Choice, dtMedExInfoHdr_Retu) Then
                Exit Function
            End If

            Me.ViewState(VS_DtMedExInfoHdr) = dtMedExInfoHdr_Retu
        Catch ex As Exception

        End Try

        txtDate.Text = System.DateTime.Today.Date.ToString("dd-MMM-yyyy")
        FillDropDown() 'Fill Values in Project Drop Down List
        SetLocationLabel()
        CheckMedExInfoHdrExists()
    End Function

#End Region

#Region "Show UI"
    Private Sub FillDropDown()

        Dim dsProject As New DataSet()

        Try
            Page.Title = ":: MedExInfoHdr :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            Me.objHelp.FillDropDown("workspacemst", "vWorkspaceId", "vWorkspaceDesc", "", dsProject, eStr_Retu)
            If dsProject.Tables(0).Rows.Count > 0 Then
                Me.HProjectId.Value = dsProject.Tables(0).Rows(0)("vWorkspaceId")
                Me.txtProject.Text = dsProject.Tables(0).Rows(0)("vWorkspaceDesc")
            End If
            '    'ddlProject.DataValueField = "vWorkspaceId"
            '    'ddlProject.DataTextField = "vWorkspaceDesc"
            '    'ddlProject.DataBind()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try


    End Sub

    Private Sub SetLocationLabel()
        Dim dsUserMst As New DataSet

        If Not objHelp.GetViewUserMst(" iUserId = " & Session(S_UserID), WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsUserMst, eStr_Retu) Then
            ShowErrorMessage(eStr_Retu, "")
        Else
            lblLocation.Text = dsUserMst.Tables(0).Rows(0)("vLocationName").ToString
            ViewState("LocationCode") = dsUserMst.Tables(0).Rows(0)("vLocationcode").ToString
        End If
    End Sub

    Private Sub CheckMedExInfoHdrExists()
        Dim projectId As String = ""
        Dim perioId As String = ""
        Dim dsSetProject As New DataSet()
        Dim whereCondition As String = ""

        Try
            If HProjectId.Value.Trim.Length > 0 And ddlPeriod.SelectedIndex >= 0 Then

                'whereCondition = " vWorkspaceId = '" + ddlProject.SelectedItem.Value.Trim + "'"
                whereCondition = " vWorkspaceId = '" + HProjectId.Value.Trim + "'"
                whereCondition += " AND vPeriod = '" + ddlPeriod.SelectedValue.Trim + "'"
                whereCondition += " AND vLocationCode = '" + ViewState("LocationCode") + "'"

                If objHelp.GetMedExInfoHdr(whereCondition, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsSetProject, eStr_Retu) Then
                    If dsSetProject.Tables(0).Rows.Count > 0 Then
                        txtDate.Text = DateTime.Parse(dsSetProject.Tables(0).Rows(0)("dStartDate").ToString).ToString("dd-MMM-yyyy")
                        Me.ViewState("CaseRecordId") = dsSetProject.Tables(0).Rows(0)("vCaseRecordId").ToString
                        txtDate.Enabled = False
                        img2.Disabled = True

                    Else
                        txtDate.Text = Today.ToString("dd-MMM-yyyy")
                        txtDate.Enabled = True
                        img2.Disabled = False
                    End If

                    'SetSessionValues()

                End If
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

    Private Function GenCall_Data(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                           ByRef dt_MedExInfoHdr_Retu As DataTable) As Boolean

        Dim eStr As String = ""
        Dim wStr As String = ""
        Dim ds_MedExInfoHdr As DataSet = Nothing

        Try
            If Choice_1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vCaseRecordId=" + Me.ViewState(VS_CaseRecordId).ToString().Trim
            End If


            If Not objHelp.GetMedExInfoHdr(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                        ds_MedExInfoHdr, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_MedExInfoHdr Is Nothing Then
                Throw New Exception("No Records Found for Selected role")
            End If

            dt_MedExInfoHdr_Retu = ds_MedExInfoHdr.Tables(0)
            GenCall_Data = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try

        Return True
    End Function

#Region "PAGE METHOD FOR AUTOCOMPLETE EXTENDER"

    <System.Web.Services.WebMethod()> _
        <System.Web.Script.Services.ScriptMethod()> _
        Public Shared Function GetCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        If count = 0 Then
            count = 10
        End If

        Dim ds As New DataSet()
        Dim estr As String = ""
        Dim result As Boolean = False
        Dim dr As DataRow = Nothing

        Dim objcommon As New clsCommon
        Dim objHlp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()

        result = objHlp.getworkspacemst(" vWorkspaceDesc" + " Like '%" + prefixText + "%'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr)

        Dim items As List(Of String) = New List(Of String)(count)

        For index As Integer = 0 To ds.Tables(0).Rows.Count - 1
            dr = ds.Tables(0).Rows(index)
            items.Add("'" + dr.Item("vWorkspaceId").ToString + "#" + dr.Item("vWorkspaceDesc").ToString)
        Next
        Return items.ToArray()
    End Function

#End Region

#End Region

#Region "BUTTON OK CLICK"

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Dim projectId As String = ""
        Dim perioId As String = ""
        Dim dsSetProject As New DataSet()
        Dim whereCondition As String = ""

        Dim Dt As New DataTable
        Dim Ds_MedExInfoHdr As DataSet
        Dim RetuVal As String = ""

        Dim eStr As String = ""

        Try
            If HProjectId.Value.Trim.Length > 0 And ddlPeriod.SelectedIndex >= 0 Then

                'whereCondition = " vWorkspaceId = '" + ddlProject.SelectedItem.Value.Trim + "'"
                whereCondition = " vWorkspaceId = '" + HProjectId.Value.Trim + "'"
                whereCondition += " AND vPeriod = '" + ddlPeriod.SelectedValue.Trim + "'"
                whereCondition += " AND vLocationCode = '" + ViewState("LocationCode") + "'"

                If objHelp.GetMedExInfoHdr(whereCondition, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsSetProject, eStr_Retu) Then
                    If dsSetProject.Tables(0).Rows.Count > 0 Then
                        txtDate.Text = DateTime.Parse(dsSetProject.Tables(0).Rows(0)("dStartDate").ToString).ToString("dd-MMM-yyyy")
                        Me.ViewState("CaseRecordId") = dsSetProject.Tables(0).Rows(0)("vCaseRecordId").ToString
                        SetSessionValues()
                    Else
                        AssignValues()
                        Ds_MedExInfoHdr = New DataSet()
                        Ds_MedExInfoHdr.Tables.Add(CType(Me.ViewState(VS_DtMedExInfoHdr), DataTable))
                        Ds_MedExInfoHdr.Tables(0).TableName = "MedExInfoHdr"

                        If Not objLambda.Save_InsertMedExInfoHdr(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, Ds_MedExInfoHdr, "1", RetuVal) Then
                            ShowErrorMessage(eStr, "")
                        Else
                            CType(Me.ViewState(VS_DtMedExInfoHdr), DataTable).Clear()
                            CheckMedExInfoHdrExists()
                            ObjCommon.ShowAlert("Project set successfully....", Me)
                            FillGV_Subject()
                        End If
                    End If
                End If
                'SetSessionValues()
            End If
        Catch ex As Exception
        End Try
    End Sub

#End Region

    Private Sub FillGV_Subject()
        Dim ds_Subject As New DataSet
        ds_Subject = Me.objHelp.GetResultSet("", "SubjectDetail")
    End Sub
#Region " Assign Values "
    Private Function AssignValues() As Boolean
        Dim dtOld As New DataTable
        Dim dr As DataRow
        Try
            dtOld = Me.ViewState(VS_DtMedExInfoHdr)
            dr = dtOld.NewRow
            dr("nCaseRecordHdrId") = 1 '"0000000001"
            dr("vWorkspaceId") = Me.HProjectId.Value.Trim 'Me.ddlProject.SelectedValue.Trim
            dr("vLocationCode") = Me.ViewState("LocationCode").ToString.Trim
            dr("vPeriod") = Me.ddlPeriod.SelectedValue.ToString.Trim
            dr("dStartDate") = Me.txtDate.Text.Trim
            dr("iModifyBy") = Session(S_UserID)
            dtOld.Rows.Add(dr)
            Me.ViewState(VS_DtMedExInfoHdr) = dtOld
            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function
#End Region

#Region "Drop Down List Events"

    Protected Sub ddlProject_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles ddlProject.SelectedIndexChanged
        CheckMedExInfoHdrExists()
    End Sub

    Protected Sub ddlPeriod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPeriod.SelectedIndexChanged
        CheckMedExInfoHdrExists()
        CType(Me.Master.FindControl("ScriptManager1"), ScriptManager).SetFocus(txtDate)
    End Sub

#End Region

    Private Sub SetSessionValues()
        'Session(S_ProjectId) = ddlProject.SelectedItem.Value.Trim
        Session(S_ProjectId) = HProjectId.Value.Trim
        Session(S_PeriodId) = ddlPeriod.SelectedValue.Trim
        Session(S_CaseRecordId) = Me.ViewState("CaseRecordId")
    End Sub


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

    Protected Sub btnProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProject.Click
        CheckMedExInfoHdrExists()
        CType(Me.Master.FindControl("ScriptManager1"), ScriptManager).SetFocus(ddlPeriod)
    End Sub

End Class
