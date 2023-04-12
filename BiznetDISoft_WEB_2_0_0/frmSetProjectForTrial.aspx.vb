
Imports System.Collections.Generic

Partial Class frmSetProjectForTrial
    Inherits System.Web.UI.Page

#Region " VARIABLE DECLARATION "

    Dim ObjCommon As New clsCommon
    Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
    Private Const VS_Choice As String = "Choice"
    Dim flag As String = String.Empty

#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Not Page.IsPostBack Then
                If Not Gencall_showUI() Then
                    Throw New Exception("Error While Gencall_showUI")
                End If
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....Page_Load")
        Finally
        End Try

    End Sub

#End Region

#Region "GenCall_showUI"

    Private Function Gencall_showUI() As Boolean
        Dim sender As Object
        Dim e As EventArgs

        Try
            Page.Title = ":: Set/Unset Project :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Set/Unset Project"

            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtProject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnProject_Click(sender, e)
            End If
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)

            'Me.trPeriod.Style.Add("display", "")
            'If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
            '    Me.trPeriod.Style.Add("display", "none")
            'End If

            Gencall_showUI = True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....Gencall_showUI")
        End Try

    End Function
#End Region

#Region "BUTTON CLICK"
    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Try

            If Me.txtProject.Text.Split("]").Length = 2 Then
                flag = "SET"
                If Me.txtProject.Text <> "" Then
                    If Not Session(S_ProjectId) Is Nothing AndAlso Session(S_ProjectName) Is Nothing Then
                        Session.Remove(S_ProjectId)
                        Session.Remove(S_ProjectName)
                        Session.Remove(S_PeriodId)
                    End If
                    Session.Add(S_ProjectId, Me.HProjectId.Value.Trim)
                    Session.Add(S_ProjectName, Me.txtProject.Text.Trim)
                    'Session.Add(S_PeriodId, Me.ddlPeriod.SelectedValue.Trim)
                    If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                        Session.Add(S_PeriodId, "1")
                    End If
                    If Not AssignValues(flag) Then
                        Me.ShowErrorMessage("", "Error While AssignValues()...")
                    Else
                        ObjCommon.ShowAlert("Project Set Successfully.", Me.Page)
                    End If
                Else
                    ObjCommon.ShowAlert("Select Project.", Me.Page)
                End If
            Else
                ObjCommon.ShowAlert("Select Proper Project to Set.", Me.Page)
                Me.txtProject.Text = ""
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "Error while btnOk_Click()....")
        End Try
    End Sub

    Protected Sub btnProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProject.Click
        Try


            Me.btnOk.Enabled = True
            'CType(Me.Master.FindControl("ScriptManager1"), ScriptManager).SetFocus(ddlPeriod)

            'If Not FillddlPeriod() Then
            '    Me.ShowErrorMessage("Error While Getting Periods", "")
            '    Exit Sub
            'End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnProject_Click")
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            If Not Session(S_ProjectId) Is Nothing AndAlso Session(S_ProjectName) Is Nothing Then
                Session.Remove(S_ProjectId)
                Session.Remove(S_ProjectName)
                Session.Remove(S_PeriodId)
            End If

            Me.txtProject.Text = ""
            Me.HProjectId.Value = ""
            'Me.ddlPeriod.Items.Clear()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....btnCancel_Click")
        End Try
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Try
            Me.Response.Redirect("frmMainPage.aspx")
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......btnExit_Click")
        End Try
    End Sub

    Protected Sub btnUnSet_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUnSet.Click

        Dim Ds_UnSetProject As New DataSet
        Dim Dt_UnsetProject As New DataTable

        Try
            flag = "UNSET"
            If txtProject.Text <> "" Then
                If Not Session(S_UserID) Is Nothing AndAlso Not Session(S_ProjectName) Is Nothing Then
                    If Not AssignValues(flag) Then
                        Me.ShowErrorMessage("", "....Error While AssignValues()..")
                    Else
                        ObjCommon.ShowAlert("Project Unset Successfully.", Me.Page)
                    End If
                End If
            Else
                ObjCommon.ShowAlert("Select Project", Me.Page)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....Error While btnUnSet_Click")
        End Try
    End Sub
#End Region

#Region "FillDropdown"

    Private Function FillddlPeriod() As Boolean
        Dim ds_WorkSpaceNodeDetail As New Data.DataSet
        Dim View_WorkSpaceNodeDetail As New DataView
        Dim estr As String = String.Empty
        Dim wstr As String = "1=1"

        Try
           
            wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "' "
            ds_WorkSpaceNodeDetail = Nothing

            If Not objHelp.GetViewWorkSpaceNodeDetail(wstr, ds_WorkSpaceNodeDetail, estr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data From WorkSpaceModeDetail:" + estr, Me.Page)
                Return False
            End If

            View_WorkSpaceNodeDetail = ds_WorkSpaceNodeDetail.Tables(0).DefaultView
            View_WorkSpaceNodeDetail.Sort = "iPeriod"

            'Me.ddlPeriod.DataSource = ds_WorkSpaceNodeDetail.Tables(0).DefaultView.ToTable(True, "iPeriod".ToString())
            'Me.ddlPeriod.DataValueField = "iPeriod"
            'Me.ddlPeriod.DataTextField = "iPeriod"
            'Me.ddlPeriod.DataBind()
            'Me.ddlPeriod.Items.Insert(0, New ListItem("--Select Period--", 0))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillddlPeriod")
            Return False
        End Try
    End Function

#End Region
#Region "AssignValues" '' Added By dipen Shah on 22 sept 2014
    Private Function AssignValues(ByVal flag As String) As Boolean
        Dim Ds As New DataSet
        Dim Ds_Field As New DataSet
        Dim dr As DataRow
        Dim dt_SetProject As DataTable = Nothing
        Dim Choise As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim WStr As String
        Dim Ds_Unset As New DataSet
        Try

            If flag.ToUpper = "SET" Then
                WStr = "select * from SetProjectMatrix where vWorkspaceId = '" + Me.HProjectId.Value.ToString() + "'And iUserId=" + Me.Session(S_UserID) + "And cStatusIndi='N'"
                Ds_Field = objHelp.GetResultSet(WStr, "SetProjectMatrix")
                If Ds_Field.Tables(0).Rows.Count > 0 Then
                    ObjCommon.ShowAlert("This Project is already set.", Me.Page)
                Else
                    WStr = "1=2"
                    If Not objHelp.GetSetProjectMatrix(WStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds, eStr_Retu) Then
                        Response.Write(eStr_Retu)
                        Exit Function
                    End If
                    dt_SetProject = Ds.Tables(0)
                    dr = dt_SetProject.NewRow()
                    dr("vWorkSpaceID") = Me.HProjectId.Value.ToString()
                    dr("iUserId") = Me.Session(S_UserID)
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dt_SetProject.Rows.Add(dr)
                    dt_SetProject.AcceptChanges()
                    Ds.Tables.Add("dt_SetProject")
                    Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
                    If Not objLambda.Save_SetProjectMatrix(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MastersEntriesEnum_SetProjectMatrix, Ds, Me.Session(S_UserID), eStr_Retu) Then
                        Me.ShowErrorMessage(eStr_Retu, "........Save_SetProjectMatrix()")
                        Return False
                    End If
                End If
                Return True
            ElseIf flag.ToUpper = "UNSET" Then
                WStr = "1=2"
                If Not objHelp.GetSetProjectMatrix(WStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds, eStr_Retu) Then
                    Me.ShowErrorMessage(eStr_Retu, ".......GetSetProjectMatrix()")
                    Return False
                End If
                dt_SetProject = Ds.Tables(0)
                dr = dt_SetProject.NewRow()
                dr("vWorkspaceId") = Me.HProjectId.Value.ToString()
                dr("iUserId") = Me.Session(S_UserID)
                dr("iModifyby") = Me.Session(S_UserID)
                dr("cStatusIndi") = "D"
                dt_SetProject.Rows.Add(dr)
                Ds.Tables.Add("dt_SetProject")
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
                If Not objLambda.Save_SetProjectMatrix(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MastersEntriesEnum_SetProjectMatrix, Ds, Me.Session(S_UserID), eStr_Retu) Then
                    Me.ShowErrorMessage(eStr_Retu, ".....Error While Save_SetProjectMatrix()")
                    Return False
                End If
                Session.Remove(S_ProjectName)
                Session.Remove(S_ProjectId)
                Me.txtProject.Text = ""
                Me.HProjectId.Value = ""
                'Me.ddlPeriod.Items.Clear()
                Return True
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "Error While AssignValues(UNSET)....")
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

  
End Class
