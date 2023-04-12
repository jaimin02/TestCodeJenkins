
Partial Class frmProjectGroupWorkspaceMatrix
    Inherits System.Web.UI.Page


#Region "Variable Declaration "

    Private objCommon As New clsCommon
    Private objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

    Private Choice As WS_Lambda.DataObjOpenSaveModeEnum
    Private MasterEntry As WS_Lambda.MasterEntriesEnum
    Private estr_retu As String = ""

    Private Const VS_dtProjectGroupWorkspaceMatrix As String = "dtProjectGroupWorkspaceMatrix"

#End Region

#Region "Page Load "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Page.Title = ":: Project Group Project Matrix  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
        Page.Title = ":: Scope Of Service  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
        If Not IsPostBack Then
            Me.GenCall()
        End If

    End Sub

#End Region

#Region "GENCALL "

    Private Function GenCall() As Boolean
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            'CType(Me.Master.FindControl("lblHeading"), Label).Text = "Project Group Project Matrix"
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Scope Of Service"

            If Not Me.GenCall_Data() Then
                Exit Function
            End If

            If Not Me.fillProjectGroup() Then
                Exit Function
            End If

            Me.fillCheckBoxListWorkspace()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Function

#End Region

#Region "GENCALL_DATA "

    Private Function GenCall_Data() As Boolean
        Dim dsProGrpWrkspceMtx As DataSet = Nothing
        Dim eStr As String = ""

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Not Me.objhelpDb.GetProjectgroupWorkspaceMatrix("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                                                         dsProGrpWrkspceMtx, eStr) Then
                Me.ShowErrorMessage("", estr_retu)
                Exit Function
            End If

            If dsProGrpWrkspceMtx Is Nothing Then
                Throw New Exception(estr_retu)
            End If

            Me.ViewState(VS_dtProjectGroupWorkspaceMatrix) = dsProGrpWrkspceMtx.Tables(0)

            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage("", estr_retu)
        End Try
    End Function

#End Region

#Region "Fill ProjectGroup "

    Private Function fillProjectGroup() As Boolean
        Dim ds_ProjectGroup As New DataSet
        Dim wStr As String = ""
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Not objCommon.GetScopeValueWithCondition(wStr) Then
                Exit Function
            End If
            wStr += " And cstatusindi <> 'D'"

            If Not Me.objhelpDb.GetProjectgroupMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                    ds_ProjectGroup, estr_retu) Then
                Me.ShowErrorMessage("", estr_retu)
                Exit Function
            End If

            Me.ddlProjectGroup.DataSource = Nothing
            Me.ddlProjectGroup.DataSource = ds_ProjectGroup
            Me.ddlProjectGroup.DataTextField = "vProjectGroupDesc"
            Me.ddlProjectGroup.DataValueField = "nProjectGroupNo"
            Me.ddlProjectGroup.DataBind()
            Me.ddlProjectGroup.Items.Insert(0, New ListItem("Please Select Project Group !", "0"))
            Me.ddlProjectGroup.SelectedIndex = 0

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Function

#End Region

#Region "fillCheckBoxListWorkspace"

    Private Sub fillCheckBoxListWorkspace()
        Dim ds_Workspace As New DataSet
        Dim wStr As String = ""
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Not objCommon.GetScopeValueWithCondition(wStr) Then
                Exit Sub
            End If
            wStr += " And cstatusindi <> 'D'"

            If Not Me.objhelpDb.getworkspacemst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                ds_Workspace, estr_retu) Then

                Me.ShowErrorMessage("", estr_retu)
                Exit Sub

            End If

            Me.chklstWorkspace.Items.Clear()

            For Each dr As DataRow In ds_Workspace.Tables(0).Rows
                Me.chklstWorkspace.Items.Add(New ListItem(dr("vWorkspaceDesc") + " (" + dr("vRequestId") + ")", dr("vWorkspaceId")))
            Next

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

#End Region

#Region "DropDownList Event "

    Protected Sub ddlProjectGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProjectGroup.SelectedIndexChanged
        Dim ds As New DataSet
        Dim wStr As String = ""
        Dim lst As ListItem
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Me.ResetAll()

            If Me.ddlProjectGroup.SelectedValue = 0 Then
                Exit Sub
            End If

            wStr = "nProjectGroupNo = " + ddlProjectGroup.SelectedValue.ToString + " And cStatusIndi <> 'D'"

            If Not Me.objhelpDb.GetProjectgroupWorkspaceMatrix(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                    ds, estr_retu) Then
                Me.ShowErrorMessage("", estr_retu)
                Exit Sub
            End If

            For index As Integer = 0 To ds.Tables(0).Rows.Count - 1

                lst = Me.chklstWorkspace.Items.FindByValue(ds.Tables(0).Rows(index)("vWorkSpaceId").ToString)

                If lst.Value = ds.Tables(0).Rows(index)("vWorkspaceId").ToString Then
                    Me.chklstWorkspace.Items.FindByValue(lst.Value).Selected = True
                End If

            Next

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

#End Region

#Region "ResetAll"

    Private Function ResetAll() As Boolean
        Dim index As Integer = 0
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            For index = 0 To Me.chklstWorkspace.Items.Count - 1

                If Me.chklstWorkspace.Items(index).Selected Then
                    Me.chklstWorkspace.Items(index).Selected = False
                End If

            Next index

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Function

#End Region

#Region "Save Button "

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim ds_save As New DataSet
        Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
        MasterEntry = WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_ProjectGroupWorkspaceMatrix

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Not AssignValues() Then
                Exit Sub
            End If

            ds_save.Tables.Add(CType(Me.ViewState(VS_dtProjectGroupWorkspaceMatrix), DataTable).Copy)
            ds_save.Tables(0).TableName = "PROJECTGROUPWORKSPACEMATRIX"

            If Not Me.objLambda.Save_ProjectGroupWorkspaceMatrix(Choice, MasterEntry, ds_save, Me.Session(S_UserID).ToString, estr_retu) Then

                Me.objCommon.ShowAlert("Error occured while saving in ProjectGroupWorkspaceMatrix", Me)
                Exit Sub

            End If

            objCommon.ShowAlert("Record Saved Successfully !", Me)

            Me.GenCall_Data()
            Me.ddlProjectGroup.SelectedIndex = 0
            Me.ResetAll()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Private Function AssignValues() As Boolean
        Dim cnt As Integer = 0
        Dim CountNode As New ListItem
        
        Dim ds_ProjGrpWrkspceMtx As New DataSet
        Dim drow As DataRow
        Dim Find As Boolean = False

        Dim ds_NewProjGrpWrkspceMtx As New DataSet
        Dim wStr As String = ""
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            wStr = "nProjectGroupNo= " + Me.ddlProjectGroup.SelectedValue.ToString '+ " and cstatusindi <> 'D'"

            If Not Me.objhelpDb.GetProjectgroupWorkspaceMatrix(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                        ds_ProjGrpWrkspceMtx, estr_retu) Then
                Me.ShowErrorMessage("", estr_retu)
                Exit Function
            End If

            For Each dr_old As DataRow In ds_ProjGrpWrkspceMtx.Tables(0).Rows
                dr_old("cStatusIndi") = "D"
                dr_old.AcceptChanges()
            Next dr_old

            ds_ProjGrpWrkspceMtx.Tables(0).AcceptChanges()

            For Index As Integer = 0 To Me.chklstWorkspace.Items.Count - 1
                If Me.chklstWorkspace.Items(Index).Selected Then

                    CountNode = Me.chklstWorkspace.Items(Index)
                    Find = False

                    For cnt = 0 To ds_ProjGrpWrkspceMtx.Tables(0).Rows.Count - 1

                        If ds_ProjGrpWrkspceMtx.Tables(0).Rows(cnt)("vWorkspaceId") = CountNode.Value.ToString Then

                            ds_ProjGrpWrkspceMtx.Tables(0).Rows(cnt)("cStatusIndi") = "E"
                            ds_ProjGrpWrkspceMtx.Tables(0).Rows(cnt)("iModifyBy") = Me.Session(S_UserID).ToString
                            ds_ProjGrpWrkspceMtx.Tables(0).Rows(cnt)("dModifyOn") = Now.Date()

                            Find = True
                            Exit For
                        End If

                    Next cnt

                    If Find = False Then
                        'nProjectGroupWorkspaceMatrixNo,nProjectGroupNo,vWorkspaceId,iModifyBy,dModifyOn,cStatusIndi

                        drow = ds_ProjGrpWrkspceMtx.Tables(0).NewRow()
                        drow("nProjectGroupWorkspaceMatrixNo") = -1
                        drow("nProjectGroupNo") = ddlProjectGroup.SelectedValue
                        drow("vWorkspaceId") = CountNode.Value
                        drow("cStatusIndi") = "N"
                        drow("iModifyBy") = Me.Session(S_UserID).ToString
                        drow("dModifyOn") = Now.Date()
                        ds_ProjGrpWrkspceMtx.Tables(0).Rows.Add(drow)
                        ds_ProjGrpWrkspceMtx.Tables(0).AcceptChanges()

                    End If

                End If

            Next Index

            ds_ProjGrpWrkspceMtx.AcceptChanges()
            ds_NewProjGrpWrkspceMtx = ds_ProjGrpWrkspceMtx.Copy()

            Me.ViewState(VS_dtProjectGroupWorkspaceMatrix) = ds_NewProjGrpWrkspceMtx.Tables(0)
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, estr_retu)
        End Try
    End Function

#End Region

#Region "Button Events"

    Protected Sub btnexit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnexit.Click
        Response.Redirect("frmmainpage.aspx")
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
