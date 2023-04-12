Imports System.Data
Imports System.Web.UI.WebControls
Imports System.Diagnostics
Imports System.IO

Partial Class FrmForceCtmMedexInfoHdrDtlunlockForm
    Inherits System.Web.UI.Page

#Region "Variable Declartion"
    Dim ObjCommon As New clsCommon
    Dim ObjHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Dim ObjLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
    Private objHelpDbTable As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()

    Private Const Vs_DataEntryRecords As String = "View_DataEntryControl"
    Private Const Vs_Records As String = "DataentryControl"
    Private Const Vs_DataentryControlHistory As String = "DataEntryControlHistory"
#End Region
#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                If Not GenCall() Then
                    Me.ShowErrorMessage("Error While GenCall()...", "")
                End If
            End If

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Page_Load()...", "")
        End Try

    End Sub
#End Region
#Region "GenCall"
    Private Function GenCall() As Boolean
        Try
            If Not GenCall_Data() Then
                Me.ShowErrorMessage("Error While GenCall_Data()....", "")
            End If

            If Not GenCall_ShowUI() Then
                Me.ShowErrorMessage("Error While GenCall_ShowUI()...", "")
            End If
        Catch ex As Exception

        End Try
        Return True
    End Function

    Private Function GenCall_Data() As Boolean
        Dim Ds_Project As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim Ds_Records As New DataSet
        Try
            wStr = "1=1"
            Me.ViewState(Vs_DataEntryRecords) = Nothing
            If Not ObjHelp.GetData("View_DataEntryControl", "*", wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, Ds_Project, eStr) Then
                Me.ShowErrorMessage(eStr + "", "Error While GetData()...")
            End If
            If Not Ds_Project Is Nothing AndAlso Ds_Project.Tables(0).Rows.Count > 0 Then
                Me.ViewState(Vs_DataEntryRecords) = Ds_Project
            End If

            wStr = "1=2"
            If Not ObjHelp.GetData("DataEntryControl", "*", wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Me.ViewState(Vs_Records), eStr) Then
                Me.ShowErrorMessage(eStr.ToString(), "")
            End If

            wStr = "1=2"
            If Not ObjHelp.GetData("DataEntryControlHistory", "*", wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Me.ViewState(Vs_DataentryControlHistory), eStr) Then
                Me.ShowErrorMessage("Error While GetData()..... ", "")
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message + "Error While GenCall_Data()", "")
        End Try
        Return True
    End Function

    Private Function GenCall_ShowUI() As Boolean
        Try

            Page.Title = ":: ForceUnlockForm :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Force Unlock Operation"

            If Not FillDDlProject() Then
                Me.ShowErrorMessage("Error While FillDDLProject()", "")
            End If
        Catch ex As Exception
            Me.ShowErrorMessage("Error while GenCall_ShowUI()...", "")
        End Try
        Return True
    End Function
#End Region
#Region "Fill Function"
    Private Function FillDDlProject() As Boolean
        Dim Ds_Project As New DataSet
        Dim dv As DataView
        Try
            Me.ddlProject.Items.Clear()
            Ds_Project = CType(Me.ViewState(Vs_DataEntryRecords), DataSet)
            If Not Ds_Project Is Nothing AndAlso Ds_Project.Tables(0).Rows.Count > 0 Then
                dv = Ds_Project.Tables(0).DefaultView
                Me.ddlProject.DataSource = dv.ToTable(True, "vWorkspaceId", "vProjectno")
                Me.ddlProject.DataValueField = "vWorkspaceId"
                Me.ddlProject.DataTextField = "vProjectNo"
                Me.ddlProject.DataBind()
                Me.ddlProject.Items.Insert(0, New ListItem("--Select Project--", 0))
            Else
                Me.ddlProject.DataSource = Nothing
                Me.ddlProject.DataBind()
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message + "", "Error while FillDDlProject()....")
        End Try
        Return True
    End Function

    Private Function FillDDLPeriod() As Boolean
        Dim ds_Period As New DataSet
        Dim dv As DataView

        Try
            Me.ddlPeriod.Items.Clear()
            ds_Period = CType(Me.ViewState(Vs_DataEntryRecords), DataSet)
            If Not ds_Period Is Nothing AndAlso ds_Period.Tables(0).Rows.Count > 0 Then
                dv = ds_Period.Tables(0).DefaultView
                dv.RowFilter = "vWorkSpaceId= '" + Me.ddlProject.SelectedValue.ToString().Trim() + "'"
                Me.ddlPeriod.DataSource = dv.ToTable(True, "iPeriod")
                Me.ddlPeriod.DataValueField = "iPeriod"
                Me.ddlPeriod.DataTextField = "iPeriod"
                Me.ddlPeriod.DataBind()
                Me.ddlPeriod.Items.Insert(0, New ListItem("--Select Period--", 0))
            Else
                Me.ddlPeriod.DataSource = Nothing
                Me.ddlPeriod.DataBind()
                Return False
            End If
        Catch ex As Exception
            Me.ShowErrorMessage("Error While FillDDLPeriod()... ", "")
        End Try
        Return True
    End Function

    Private Function FillDDLSubject() As Boolean
        Dim ds_Subject As New DataSet
        Dim dv As New DataView
        Try
            Me.ddlSubjects.Items.Clear()
            ds_Subject = CType(Me.ViewState(Vs_DataEntryRecords), DataSet)
            If Not ds_Subject Is Nothing AndAlso ds_Subject.Tables(0).Rows.Count > 0 Then
                dv = ds_Subject.Tables(0).DefaultView
                dv.RowFilter = "vWorkSpaceId= '" + Me.ddlProject.SelectedValue.ToString().Trim() + "' And iPeriod='" + Me.ddlPeriod.SelectedValue.ToString().Trim() + "'"
                Me.ddlSubjects.DataSource = dv.ToTable(True, "vSubjectId", "MySubjectNo")
                Me.ddlSubjects.DataValueField = "vSubjectId"
                Me.ddlSubjects.DataTextField = "MySubjectNo"
                Me.ddlSubjects.DataBind()
                Me.ddlSubjects.Items.Insert(0, New ListItem("--Select Subject--", 0))
            Else
                Me.ddlSubjects.DataSource = Nothing
                Me.ddlSubjects.DataBind()
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message + "", "Error While FillDDLSubject()...")
            Return False
        End Try
        Return True
    End Function

    Private Function FillDDLActivity() As Boolean
        Dim Ds_Activity As New DataSet
        Dim Dv As New DataView
        Try
            Me.ddlActivities.Items.Clear()
            Ds_Activity = CType(Me.ViewState(Vs_DataEntryRecords), DataSet)
            If Not Ds_Activity Is Nothing AndAlso Ds_Activity.Tables(0).Rows.Count > 0 Then
                Dv = Ds_Activity.Tables(0).DefaultView
                Dv.RowFilter = "vWorkSpaceId= '" + Me.ddlProject.SelectedValue.ToString().Trim() + "' And iPeriod='" + Me.ddlPeriod.SelectedValue.ToString().Trim() + "' And vSubjectId= '" + Me.ddlSubjects.SelectedValue.ToString().Trim() + "'"
                Me.ddlActivities.DataSource = Dv.ToTable(True, "inodeId", "vNodeDisplayName")
                Me.ddlActivities.DataValueField = "iNodeId"
                Me.ddlActivities.DataTextField = "vNodeDisplayName"
                Me.ddlActivities.DataBind()
                Me.ddlActivities.Items.Insert(0, New ListItem("--Select Activity--", 0))
            Else
                Me.ddlActivities.DataSource = Nothing
                Me.ddlActivities.DataBind()
                Return False
            End If
        Catch ex As Exception
            Me.ShowErrorMessage("......Error while FillDDLActivity()", "")
            Return False
        End Try
        Return True
    End Function
#End Region
#Region "Drop Down Event"
    Protected Sub ddlProject_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlProject.SelectedIndexChanged
        Try
            If Not ddlProject.SelectedIndex.ToString().Trim() = "0" Then
                If Not FillDDLPeriod() Then
                    Me.ShowErrorMessage("Error while FillDDlPeriod()", "")
                End If
            Else
                ObjCommon.ShowAlert("Select Project", Me.Page)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage("Error While ddlProject_SelectedIndexChanged().. ", "")
            Exit Sub
        End Try
    End Sub

    Protected Sub ddlPeriod_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPeriod.SelectedIndexChanged
        Try
            If Not FillDDLSubject() Then
                Me.ShowErrorMessage("Error While ddlPeriod_SelectedIndexChanged", "........FillDDLSubject()")
            End If
        Catch ex As Exception
            Me.ShowErrorMessage("Error While ddlPeriod_SelectedIndexChanged().....", "")
            Exit Sub
        End Try
    End Sub


    Protected Sub ddlSubjects_SelectedIndexChanged1(sender As Object, e As EventArgs) Handles ddlSubjects.SelectedIndexChanged
        Try
            If Not FillDDLActivity() Then
                Me.ShowErrorMessage("FillDDLActivity()", "")
            End If
        Catch ex As Exception
            Me.ShowErrorMessage("Error While ddlSubjects_SelectedIndexChanged1()..", "")
        End Try
    End Sub
#End Region

#Region "Save And Delete Functions"
    Private Function SaveRecords() As Boolean
        Dim Ds_Save As New DataSet
        Dim Ds As New DataSet
        Dim Dv_Save As DataView
        Dim Dt_Save As New DataTable
        Dim eStr As String = String.Empty
        Try
            Ds_Save = CType(Me.ViewState(Vs_DataEntryRecords), DataSet)
            Dv_Save = Ds_Save.Tables(0).DefaultView
            Dv_Save.RowFilter = "vWorkSpaceId= '" + Me.ddlProject.SelectedValue.ToString().Trim() + "' And iPeriod='" + Me.ddlPeriod.SelectedValue.ToString().Trim() + "' And vSubjectId= '" + Me.ddlSubjects.SelectedValue.ToString().Trim() + "'"
            Dt_Save = Dv_Save.ToTable()

            Ds = CType(Me.ViewState(Vs_DataentryControlHistory), DataSet)
            Dim Dr As DataRow = Ds.Tables(0).NewRow()
            Dr("vWorkspaceId") = Me.ddlProject.SelectedValue.ToString.Trim()
            Dr("iNodeId") = Me.ddlActivities.SelectedValue.ToString.Trim()
            Dr("vSubjectId") = Me.ddlSubjects.SelectedValue.ToString.Trim()
            Dr("ilockBy") = Dt_Save.Rows(0)("LockBy").ToString.Trim()
            Dr("iModifyby") = Session(S_UserID).ToString.Trim()
            Dr("cStatusIndi") = "N"
            Ds.Tables(0).Rows.Add(Dr)
            Ds.Tables(0).AcceptChanges()
            If Not Ds Is Nothing AndAlso Ds.Tables(0).Rows.Count > 0 Then
                If Not ObjLambda.Insert_DataEntryControlHistory(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, Ds, eStr) Then
                    Me.ShowErrorMessage("Error While Insert_DataEntryControlHistory().... ", "")
                End If
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message + "Error While SaveRecords()....", "")
        End Try
        Return True
    End Function

    Private Function DeleteRecords() As Boolean
        Dim Ds_Records As New DataSet
        Dim eStr As String = String.Empty
        Dim Dr_Records As DataRow
        Dim Dt_Records As New DataTable
        Dim Ds As New DataSet
        Dim Return_Val As Char
        Try
            Ds = CType(Me.ViewState(Vs_DataEntryRecords), DataSet)
            Dim dv As DataView = Ds.Tables(0).DefaultView
            dv.RowFilter = "vWorkSpaceId= '" + Me.ddlProject.SelectedValue.ToString().Trim() + "' And iPeriod='" + Me.ddlPeriod.SelectedValue.ToString().Trim() + "' And vSubjectId= '" + Me.ddlSubjects.SelectedValue.ToString().Trim() + "'"
            Dt_Records = dv.ToTable()

            Ds_Records = CType(Me.ViewState(Vs_Records), DataSet)
            Dr_Records = Ds_Records.Tables(0).NewRow()
            Dr_Records("vWorkspaceId") = Me.ddlProject.SelectedValue.ToString().Trim()
            Dr_Records("iNodeId") = Me.ddlActivities.SelectedValue.ToString().Trim()
            Dr_Records("vSubjectId") = Me.ddlSubjects.SelectedValue.ToString().Trim()
            Dr_Records("iWorkFlowStageId") = Dt_Records.Rows(0)("iWorkFlowStageId")
            Ds_Records.Tables(0).Rows.Add(Dr_Records)
            Ds_Records.Tables(0).AcceptChanges()

            If Not Ds_Records Is Nothing AndAlso Ds_Records.Tables(0).Rows.Count > 0 Then
                If Not ObjLambda.insert_DataEntryControl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete, Ds_Records, eStr, Return_Val) Then
                    Me.ShowErrorMessage(eStr + "", "Error while insert_DataEntryControl() ")
                End If
                ObjCommon.ShowAlert("Page/Activity unlock successfully.", Me.Page)
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message + "Error while DeleteRecords()...", "")
            Return False
        End Try
        Return True
    End Function
#End Region
#Region "Button Events"
    Protected Sub btnUnlock_Click(sender As Object, e As EventArgs) Handles btnUnlock.Click
        Try
            If Not SaveRecords() Then
                Me.ShowErrorMessage("Error While SaveRecords()....", "")
                Exit Sub
            End If
            If Not DeleteRecords() Then
                Me.ShowErrorMessage("Error While DeleteRecords.....", "")
                Exit Sub
            End If
            Me.ddlProject.Items.Clear()
            Me.ddlPeriod.Items.Clear()
            Me.ddlSubjects.Items.Clear()
            Me.ddlActivities.Items.Clear()
            GenCall()

        Catch ex As Exception
            Me.ShowErrorMessage("Error While btnUnlock_Click", "")
        End Try
    End Sub

    Protected Sub BtnCancel_click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Try
            Me.ddlActivities.Items.Clear()
            Me.ddlSubjects.Items.Clear()
            Me.ddlProject.Items.Clear()
            ddlPeriod.Items.Clear()
            GenCall()
        Catch ex As Exception
            Me.ShowErrorMessage("Error While BtnCancel_click()....", "")
        End Try
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

