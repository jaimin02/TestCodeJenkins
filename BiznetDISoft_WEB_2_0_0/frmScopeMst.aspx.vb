
Partial Class frmScopeMst
    Inherits System.Web.UI.Page

#Region "Varriable Declaration"

    Private objCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()


    Private Choice As WS_Lambda.DataObjOpenSaveModeEnum
    Private MasterEntry As WS_Lambda.MasterEntriesEnum
    Private estr_Retu As String = ""

    Private Const Vs_Choice As String = "Choice"
    Private Const Vs_dsScopeMst As String = "dsScopeMst"

#End Region

#Region "Page_Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not GenCall() Then
                Exit Sub
            End If
        End If
    End Sub

#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean
        Dim ds_ScopeMst As New DataSet
        Dim Wstr As String = "1=2"
        Try

            If Not GenCall_Data(ds_ScopeMst, Wstr) Then
                GenCall = False
                Exit Function
            End If

            Me.ViewState(Vs_dsScopeMst) = ds_ScopeMst

            If Not Gencall_ShowUI() Then
                GenCall = False
                Exit Function
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..Gencall")
        End Try
    End Function

#End Region

#Region "GenCall_Data"

    Private Function GenCall_Data(ByRef ds_ScopeMst As DataSet, ByVal Wstr As String) As Boolean
        Try
            If Not Me.objHelp.GetScopeMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ScopeMst, estr_Retu) Then
                Me.ShowErrorMessage("", estr_Retu)
                GenCall_Data = False
                Exit Function
            End If

            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_Data")
        End Try
    End Function

#End Region

#Region "GenCall_ShowUI"

    Private Function Gencall_ShowUI() As Boolean
        Try
            Page.Title = ":: Scope Master  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblheading"), Label).Text = "Scope Master"
            If Not Me.FillProjectTypeList() Then
                Exit Function
            End If
            Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
            Me.ViewState(Vs_Choice) = Choice
            Gencall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_ShowUI")
        End Try
    End Function

#End Region

#Region "FillCheckBoxList"

    Protected Function FillProjectTypeList() As Boolean
        Dim Ds_ProjectType As New DataSet
        Try

            If Not Me.objHelp.getprojectTypeMst("cstatusindi <> 'C'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                           Ds_ProjectType, estr_Retu) Then
                Me.ShowErrorMessage("Error While Getting Data From ProjectTypeMst", estr_Retu)
                Return False
            End If

            If Ds_ProjectType.Tables(0).Rows.Count < 1 Then
                objCommon.ShowAlert("No Record Found For ChkScopeValues", Me.Page)
                Return True
            End If

            Me.ChkScopeValues.DataTextField = "vProjectTypeName"
            Me.ChkScopeValues.DataValueField = "vProjectTypeCode"
            Me.ChkScopeValues.DataSource = Ds_ProjectType.Tables(0)
            Me.ChkScopeValues.DataBind()
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillProjectTypeList")
        End Try
    End Function

#End Region

#Region "FillDropDownList"

    Protected Function FillScopeName() As Boolean
        Dim ds_ScopeName As New DataSet
        Try

            If Not GenCall_Data(ds_ScopeName, "cstatusindi <> 'C'") Then
                objCommon.ShowAlert("Error While Getting Data From ScopeMst", Me.Page)
                Return False
            End If

            If ds_ScopeName.Tables(0).Rows.Count < 1 Then
                objCommon.ShowAlert("No Record Found For ddlScope", Me.Page)
                Return True
            End If

            Me.ddlScope.DataTextField = "vScopeName"
            Me.ddlScope.DataValueField = "nScopeNo"
            Me.ddlScope.DataSource = ds_ScopeName
            Me.ddlScope.DataBind()
            Me.ddlScope.Items.Insert(0, "--Select Scope Name--")

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillScopeName")
        End Try
    End Function

#End Region

#Region "DropDownList Events"

    Protected Sub ddlScope_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlScope.SelectedIndexChanged
        Dim ds_ScopeValues As New DataSet
        Dim ProjectNos As String = String.Empty
        Dim ProjectArr() As String
        Dim Wstr As String = String.Empty
        Try

            Me.ChkScopeValues.ClearSelection()

            If Me.ddlScope.SelectedIndex <= 0 Then
                Return
            End If

            Wstr = "nScopeNo=" + Me.ddlScope.SelectedValue.ToString + " and cstatusindi <> 'C'"

            If Not Me.GenCall_Data(ds_ScopeValues, Wstr) Then
                Exit Sub
            End If

            If Not ds_ScopeValues Is Nothing AndAlso ds_ScopeValues.Tables(0).Rows.Count > 0 Then
                ProjectNos = ds_ScopeValues.Tables(0).Rows(0)("vTableValues").ToString
            End If

            ProjectArr = ProjectNos.Split(",")
            For i As Integer = 0 To ProjectArr.Length - 1

                For Each l As ListItem In ChkScopeValues.Items
                    If ProjectArr(i) = l.Value.ToString Then
                        l.Selected = True
                        Exit For
                    End If
                Next l
            Next i

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..ddlScope_SelectedIndexChanged")
        End Try
    End Sub

#End Region

#Region "Button Events"

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim ds_save As New DataSet
        Dim SelectedProject As String = String.Empty
        Dim message As String = String.Empty
        Try
            SelectedProject = GetSelectedValuesInString(ChkScopeValues, False)

            SelectedProject = SelectedProject.Replace("(", "")
            SelectedProject = SelectedProject.Replace(")", "")

            If Me.ViewState(Vs_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                If Me.ddlScope.SelectedIndex = 0 Then
                    Me.objCommon.ShowAlert("Please select scope name !", Me)
                    Exit Sub
                End If

            ElseIf Me.ViewState(Vs_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                If Me.txtScopeName.Text = "" Then
                    Me.objCommon.ShowAlert("Please enter scope name !", Me)
                    Exit Sub
                End If

            End If

            If SelectedProject = "" Then
                Me.objCommon.ShowAlert("Please select at least one Project !", Me)
                Exit Sub
            End If

            MasterEntry = WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_ScopeMst
            ds_save = Me.AssignValues()

            If Not Me.objLambda.Save_ScopeMst(Me.ViewState(Vs_Choice), MasterEntry, ds_save, Me.Session(S_UserID).ToString, estr_Retu) Then
                Me.objCommon.ShowAlert("Error occured while saving in scope master", Me)
                Exit Sub
            End If
            message = IIf(Me.ViewState(Vs_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "Scope Details Saved Successfully !", "Scope Details Updated Successfully !")
            'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Alert", "ShowAlert('" + message + "')", True)
            Me.objCommon.ShowAlert(message, Me.Page)
            Me.ResetPage()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...BtnSave_Click")
        End Try
    End Sub

    Protected Sub BtnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnEdit.Click
        Try

            Me.ChkScopeValues.ClearSelection()

            If Me.BtnEdit.Text.ToUpper = "EDIT" Then

                Me.BtnEdit.Text = "Cancel"
                Me.BtnEdit.ToolTip = "Cancel"
                Me.BtnSave.Text = "Update"
                Me.BtnSave.ToolTip = "Update"
                Me.Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                Me.ViewState(Vs_Choice) = Choice
                Me.txtScopeName.Visible = False
                Me.ddlScope.Visible = True
                If Not Me.FillScopeName() Then
                    Exit Sub
                End If
                Exit Sub
            End If

            Me.BtnEdit.Text = "Edit"
            Me.BtnEdit.ToolTip = "Edit"
            Me.BtnSave.Text = "Save"
            Me.BtnSave.ToolTip = "Save"
            Me.Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
            Me.ViewState(Vs_Choice) = Choice
            Me.txtScopeName.Visible = True
            Me.ddlScope.Visible = False

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....BtnEdit_Click")
        End Try
    End Sub

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click
        Response.Redirect("frmmainpage.aspx")
    End Sub

#End Region

#Region "AssignValues"

    Private Function AssignValues() As DataSet
        Dim Ds_SaveScopeMst As New DataSet
        Dim Ds_Save As New DataSet
        Dim ds As New DataSet
        Dim ds_old As New DataSet
        Dim dr As DataRow
        Dim SelectedProject As String = String.Empty

        Dim ds_Check As New DataSet
        Dim estr As String = String.Empty
        Try

            SelectedProject = GetSelectedValuesInString(ChkScopeValues, False)
            SelectedProject = SelectedProject.Replace("(", "")
            SelectedProject = SelectedProject.Replace(")", "")


            'For validating Duplication
            If Not objHelp.GetScopeMst("cStatusIndi <> 'D' And vScopeName='" & Me.txtScopeName.Text.Trim() & "'", _
                                WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Check, estr) Then
                Me.ShowErrorMessage("Error While Getting Data From ScopeMst", estr)
                Return Nothing
                Exit Function
            End If

            If (Me.ViewState(Vs_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add And ds_Check.Tables(0).Rows.Count > 0) Or _
                (Me.ViewState(Vs_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit And ds_Check.Tables(0).Rows.Count > 1) Then

                objCommon.ShowAlert("Scope Name Already Exists !", Me.Page)
                Return Nothing
                Exit Function

            End If
            '***********************************


            If Me.ViewState(Vs_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                If Not GenCall_Data(Ds_Save, "nScopeNo=" + Me.ddlScope.SelectedValue.ToString + " and cstatusindi <> 'C'") Then
                    Return Nothing
                    Exit Function
                End If

                For Each dr In Ds_Save.Tables(0).Rows
                    dr("cStatusIndi") = "D"
                Next dr
                Ds_Save.Tables(0).AcceptChanges()

                dr = Ds_Save.Tables(0).NewRow()
                dr("nScopeNo") = Me.ddlScope.SelectedValue.ToString
                dr("vScopeName") = Me.ddlScope.SelectedItem.Text.ToString
                dr("vTableName") = "ProjectTypeMst"
                dr("vTableValues") = SelectedProject
                dr("iModifyBy") = Me.Session(S_UserID).ToString.Trim
                dr("cStatusIndi") = "E"

                Ds_Save.Tables(0).Rows.Add(dr)
                Ds_Save.Tables(0).AcceptChanges()

                ds = Ds_Save

            ElseIf Me.ViewState(Vs_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                Ds_SaveScopeMst = CType(Me.ViewState(Vs_dsScopeMst), DataSet)

                dr = Ds_SaveScopeMst.Tables(0).NewRow()

                dr("nScopeNo") = -1
                dr("vScopeName") = Me.txtScopeName.Text.ToString
                dr("vTableName") = "ProjectTypeMst"
                dr("vTableValues") = SelectedProject
                dr("iModifyBy") = Me.Session(S_UserID).ToString.Trim
                dr("cStatusIndi") = "N"
                Ds_SaveScopeMst.Tables(0).Rows.Add(dr)
                Ds_SaveScopeMst.AcceptChanges()
                ds = Ds_SaveScopeMst
            End If

            Return ds

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...AssignValues")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Error Handler"
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub
#End Region

#Region "ResetPage"
    Protected Sub ResetPage()
        Me.txtScopeName.Text = ""
        Me.ddlScope.SelectedIndex = -1
        Me.ChkScopeValues.ClearSelection()
        Me.txtScopeName.Visible = True
        Me.ddlScope.Visible = False
        Me.BtnEdit.Text = "Edit"
        Me.BtnEdit.ToolTip = "Edit"
        Me.BtnSave.Text = "Save"
        Me.BtnSave.ToolTip = "Save"
    End Sub
#End Region

End Class