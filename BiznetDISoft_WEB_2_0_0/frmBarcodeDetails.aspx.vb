
Partial Class frmBarcodeDetails

    Inherits System.Web.UI.Page


#Region "Variable declaration"

    Dim ObjCommon As New clsCommon
    Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    ' Dim objLambda As New WS_Lambda.WS_Lambda

    Private Const VS_DsViewSampleTypeDtl As String = "DsViewSampleTypeDtl"

    ',,,,,,,
    Private Const gvw_nSampleTypeDetailNo As Integer = 0
    Private Const gvw_vSampleBarCode As Integer = 1
    Private Const gvw_vSampleTypeDesc As Integer = 2
    Private Const gvw_vLocationName As Integer = 3
    Private Const gvw_vWorkspacedesc As Integer = 4
    Private Const gvw_vSubjectID As Integer = 5
    'Private Const FullName As Integer = 0
    Private Const gvw_vNodeDisplayName As Integer = 6
    Private Const gvw_dCollectionDateTime As Integer = 7
    Private Const gvw_CollectedBy As Integer = 8


    Private Const VS_sampletypedtl As String = "ds_sampletypedtl"
    
#End Region

#Region "PageLoad"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            If Not IsPostBack Then


                GenCall()

            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub
#End Region

#Region "GenCall"
    Private Function GenCall() As Boolean
        Dim ds As New DataSet
        'Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            'Choice = "1"
            'Me.ViewState(VS_Choice) = Choice   'To use it while saving

            

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If

            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function
#End Region



#Region "GenCall_ShowUI"
    Private Function GenCall_ShowUI() As Boolean
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try
            Page.Title = " :: Barcode Details ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Barcode Details"


            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function
#End Region

#Region "fillDropdown"

    Private Function FillddlActivity() As Boolean
        Dim ds_Activity As New Data.DataSet
        Dim dt_Activity As New Data.DataTable
        Dim dv_Activity As New Data.DataView
        Dim estr As String = ""
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            ds_Activity = Nothing
            If Not ObjHelp.Proc_ProjectNodeCommandButtonRights(Me.HProjectId.Value.Trim(), Me.Session(S_UserID), _
                                    "", "", ds_Activity, estr) Then

                Me.ObjCommon.ShowAlert("Error While Getting Data from Proc_ProjectNodeCommandButtonRights:" + estr, Me.Page)
                Return False
            End If

            dv_Activity = ds_Activity.Tables(0).DefaultView
            dv_Activity.RowFilter = "iPeriod = " + Me.ddlPeriod.SelectedItem.Value.Trim()
            dt_Activity = dv_Activity.ToTable()

            'dt_Activity = ds_Activity.Tables(0).DefaultView.ToTable(True, "iNodeId,vNodeDisplayName".Split(","))

            Me.ddlActivity.DataSource = dt_Activity.DefaultView.ToTable(True, "iNodeId,vNodeDisplayName".Split(","))
            Me.ddlActivity.DataValueField = "iNodeId"
            Me.ddlActivity.DataTextField = "vNodeDisplayName"
            Me.ddlActivity.DataBind()
            Me.ddlActivity.Items.Insert(0, New ListItem("--Select Activity--", 0))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function

    Private Function FillPeriod() As Boolean
        Dim ds_Period As New DataSet
        Dim eStr_Retu As String = ""


        If Not objHelp.FillDropDown("WorkspaceNodeDetail", "iPeriod", "iPeriod", "vworkspaceid='" + Me.HProjectId.Value.Trim + "'", ds_Period, eStr_Retu) Then
            ObjCommon.ShowAlert("Error while getting data from WorkspaceNodeDetail", Me)
            Return False
            Exit Function
        End If

        Me.ddlPeriod.DataSource = ds_Period
        Me.ddlPeriod.DataTextField = "iPeriod"
        Me.ddlPeriod.DataValueField = "iPeriod"
        Me.ddlPeriod.DataBind()

        Me.ddlPeriod.Items.Insert(0, New ListItem("Select Period", 0))
        Return True
    End Function

    Private Function fillgrid()
        Dim ds_sampletypedtl As New DataSet
        Dim wstr As String = ""
        Dim eStr_Retu As String = ""



        Try
                        wstr = "vWorkspaceId = '" + Me.HProjectId.Value + "' And cStatusIndi <> 'D'"
            wstr += " And iNodeId = " + Me.ddlActivity.SelectedItem.Value.Trim() + " And iPeriod = " + _
                       Me.ddlPeriod.SelectedItem.Value.Trim()


            If Not objHelp.View_SampleTypeDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_sampletypedtl, eStr_Retu) Then
                ObjCommon.ShowAlert("error while getting data from view_SAmpletypedetail", Me)
                Return False
            End If
            gvwBarcodeDtl.DataSource = ds_sampletypedtl
            gvwBarcodeDtl.DataBind()
            Me.ViewState(VS_sampletypedtl) = ds_sampletypedtl


            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function

#End Region

#Region "Selected Index change"
    Protected Sub ddlActivity_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlActivity.SelectedIndexChanged
        If Not fillgrid() Then
            ObjCommon.ShowAlert("error while filling Grid", Me)

        End If


    End Sub

    Protected Sub ddlPeriod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPeriod.SelectedIndexChanged

        If Not FillddlActivity() Then
            Exit Sub
        End If


    End Sub

    Protected Sub gvwBarcodeDtl_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvwBarcodeDtl.PageIndexChanging
        gvwBarcodeDtl.PageIndex = e.NewPageIndex
        gvwBarcodeDtl.DataSource = CType(Me.ViewState(VS_sampletypedtl), DataSet)
        gvwBarcodeDtl.DataBind()

        'If Not fillgrid() Then
        'ObjCommon.ShowAlert("error while filling Grid", Me)

        'End If

    End Sub
#End Region

#Region "Button EVents"
    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Not FillPeriod() Then
                Exit Sub
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
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
