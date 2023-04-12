
Partial Class frmprojectcopy
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objcommon As New clsCommon
    Private objhelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()

#End Region

#Region "Page Load "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                GenCall()
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....Page_Load")
        End Try
    End Sub

#End Region

#Region "GenCall "

    Private Function GenCall() As Boolean
        Try
            If Not GenCall_ShowUI() Then
                Return False
            End If

            GenCall = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....GenCall")
            Return False
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_showUI "

    Private Function GenCall_ShowUI() As Boolean
        Try

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Copy Project Structure"
            Page.Title = ":: Copy Project Structure :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID) + " and cWorkspaceType ='P' "
            Me.AutoCompleteExtender2.ContextKey = "iUserId = " & Me.Session(S_UserID) + " and cWorkspaceType ='P' "
            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....GenCall_ShowUI")
            Return False
        End Try
    End Function

#End Region

#Region "Error Handler"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objcommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub

#End Region

#Region "Buttons event "

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnexit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btncopyproject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncopyproject.Click
        Dim ds_copyproject As New DataSet
        Dim dt_copyproject As New DataTable
        Dim dr_copyproject As DataRow
        Dim ErrorMsg As String = String.Empty
        Dim eStr As String = String.Empty
        Try

            dt_copyproject.TableName = "Proc_CopyProject"
            dt_copyproject.Columns.Add("vFromWorkspaceId")
            dt_copyproject.Columns.Add("vToWorkspaceId")
            dt_copyproject.Columns.Add("iModifyBy")
            dt_copyproject.Columns.Add("IsCopyEditCheck")
            dt_copyproject.Columns.Add("OUTPUT")
            dt_copyproject.AcceptChanges()


            dr_copyproject = dt_copyproject.NewRow()
            dr_copyproject("vFromWorkspaceId") = Me.HfromProjectId.Value.ToString()
            dr_copyproject("vToWorkspaceId") = Me.HtoProjectId.Value.ToString()
            dr_copyproject("iModifyBy") = Me.Session(S_UserID)
            If rblCopyEdit.Checked = True Then
                dr_copyproject("IsCopyEditCheck") = "YES"
            Else
                dr_copyproject("IsCopyEditCheck") = "NO"
            End If

            dr_copyproject("Output") = ""

            dt_copyproject.Rows.Add(dr_copyproject)
            dt_copyproject.AcceptChanges()
            ds_copyproject.Tables.Add(dt_copyproject)
            ds_copyproject.AcceptChanges()

            If Not Me.objLambda.Proc_CopyProject(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_copyproject, ErrorMsg, eStr) Then
                Throw New Exception("Error While Copy project structure...." & eStr.ToString())
                Exit Sub
            Else
                If Not ErrorMsg.ToUpper = "SUCCESS" Then
                    Throw New Exception(ErrorMsg)
                    Exit Sub
                End If
            End If
            Me.objcommon.ShowAlert("Project Structure Copied Successfully", Me.Page)
            Me.txtfromProject.Text = ""
            Me.txttoproject.Text = ""
            Me.HfromProjectId.Value = ""
            Me.HtoProjectId.Value = ""
            Me.rblCopyEdit.Checked = Nothing
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....btncopyproject_Click")
        End Try
    End Sub


    Protected Sub btnGetEditChecks_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetEditChecks.Click
        Dim ds_EditChecks As DataSet
        Dim vWorkSpaceid As String
        Try
            vWorkSpaceid = HfromProjectId.Value
            hdnIsEditCheck.Value = "NO"

            ds_EditChecks = objhelp.GetResultSet("select *  from [MedExWorkspaceEditChecks] Where vWorkspaceId = '" + vWorkSpaceid + "' AND cStatusIndi <> 'D' ", "MedExWorkspaceEditChecks")
            If Not ds_EditChecks Is Nothing AndAlso ds_EditChecks.Tables(0).Rows.Count > 1 Then
                hdnIsEditCheck.Value = "YES"
            End If
            btnTemplateValidation_Click(Nothing, Nothing)
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub btnTemplateValidation_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTemplateValidation.Click
        Dim ds_DATA As DataSet
        Dim vFromWorkSpaceid As String
        Dim vTOWorkSpaceid As String
        Dim eStr As String
        Dim validation As String = "FALSE"
        Try
            vFromWorkSpaceid = Convert.ToString(HfromProjectId.Value)
            vTOWorkSpaceid = Convert.ToString(Me.HtoProjectId.Value)


            If Not objhelp.Proc_ValidationForCopyEditChecks(vFromWorkSpaceid, vTOWorkSpaceid, ds_DATA, eStr) Then
                Throw New Exception(eStr)
            End If
            If Not ds_DATA Is Nothing AndAlso ds_DATA.Tables(0).Rows.Count > 0 Then
                validation = Convert.ToString(ds_DATA.Tables(0).Rows(0)(0))
            End If

            Me.hdnValidationCount.Value = validation

        Catch ex As Exception

        End Try

    End Sub

#End Region

End Class
