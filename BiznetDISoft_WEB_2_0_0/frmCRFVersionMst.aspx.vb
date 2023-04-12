
Partial Class frmCRFVersionMst
    Inherits System.Web.UI.Page
#Region "VARIABLE DECLARATION"

    Private ObjCommon As New clsCommon
    Private ObjHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private ObjLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()


    Private GVC_CRFVersion_VersionNo As Integer = 0
    Private GVC_CRFVersion_cFreezeStatus As Integer = 1
    Private GVC_CRFVersion_vRemark As Integer = 2
    Private GVC_CRFVersion_dVersiondate As Integer = 3
    Private GVC_CRFVersion_vFirstName As Integer = 4
    Private GVC_CRFVersion_dModifyOn As Integer = 5

#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                If Not GenCall_ShowUI() Then
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....Page_Load")
        End Try

    End Sub

#End Region

#Region "GenCall_showUI()"

    Private Function GenCall_ShowUI() As Boolean

        Dim sender As New Object
        Dim e As EventArgs


        Try
            Page.Title = " :: CRF Version Master ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")

            'CType(Me.Master.FindControl("lblHeading"), Label).Text = "CRF Version Control"

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "CRF Version Master"

            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtProject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(sender, e)
                GenCall_ShowUI = True
                Exit Function
            End If




            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)

            GenCall_ShowUI = True


        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_ShowUI")
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

#Region "Fill Grid"
    Private Function FillGrid() As Boolean
        Dim estr_retu As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim ds_CRFVersionMst As Data.DataSet = Nothing

        Try
            Wstr = Me.HProjectId.Value + "##" + "0"
            If Not ObjHelp.Proc_cdc_dbo_CRFVersionMst_CT(Wstr, ds_CRFVersionMst, estr_retu) Then
                Throw New Exception("Error While Getting Data From cdc.dbo_CRFVersionMst_CT")
            End If
            If Not ds_CRFVersionMst Is Nothing Then
                For i As Integer = 0 To ds_CRFVersionMst.Tables(0).Rows.Count - 1
                    If ds_CRFVersionMst.Tables(0).Rows(i)("cFreezeStatus") = "F" Then
                        ds_CRFVersionMst.Tables(0).Rows(i)("cFreezeStatus") = "Freeze"
                    Else
                        ds_CRFVersionMst.Tables(0).Rows(i)("cFreezeStatus") = "UnFreeze"
                    End If
                Next
                Me.GV_CRFVersion.DataSource = ds_CRFVersionMst
                Me.GV_CRFVersion.DataBind()
            End If
            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.ToString(), "....FillGrid")
            Return False
        End Try

    End Function
#End Region

#Region "Freeze/UnFreeze"

    Public Function FreezeUnFreeze() As Boolean
        Dim ds_CRFStatus As Data.DataSet = Nothing
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty

        Try
            wstr = " vWorkSpaceId='" + Me.HProjectId.Value + "'"
            If Not ObjHelp.GetData("CRFVersionMst", "*", wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CRFStatus, estr) Then
                Throw New Exception(estr.ToString())
            End If
            If ds_CRFStatus.Tables(0).Rows.Count > 0 Then
                If ds_CRFStatus.Tables(0).Rows(ds_CRFStatus.Tables(0).Rows.Count - 1).Item("cFreezeStatus").ToString = "U" Then
                    Me.RblFreeze.Items(0).Selected = True
                    Me.RblFreeze.Items(0).Enabled = True
                    Me.RblFreeze.Items(1).Selected = False
                    Me.RblFreeze.Items(1).Enabled = False
                    Me.txtRemark.Text = ""
                    Return True
                Else
                    Me.RblFreeze.Items(1).Enabled = True
                    Me.RblFreeze.Items(1).Selected = True
                    Me.RblFreeze.Items(0).Selected = False
                    Me.RblFreeze.Items(0).Enabled = False
                    Me.txtRemark.Text = ""
                    Return True
                End If
            End If
            Me.RblFreeze.Items(1).Selected = False
            Me.RblFreeze.Items(1).Enabled = False
            Me.RblFreeze.Items(0).Enabled = True
            Me.RblFreeze.Items(0).Selected = True
            Me.txtRemark.Text = ""


            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "....FreezeUnFreeze")
            Return False
        End Try
    End Function

#End Region

#Region "Button Events"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Try

            If Not FreezeUnFreeze() Then
                Throw New Exception()
            End If
            If Not FillGrid() Then
                Throw New Exception("Error While Filling Grid")
            End If
            ''ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "getData", "getData();", True)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString(), "....btnSetProject_Click")
        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        'Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable

        Dim ds_CRFStruct As Data.DataSet = Nothing
        Dim estr As String = String.Empty

        Try

            If Not objHelp.GetData("CRFVersionMst", "*", "", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_CRFStruct, estr) Then
                Throw New Exception("Error While Getting Blank Structure")
            End If
            Dim dr = ds_CRFStruct.Tables(0).NewRow


            ds_CRFStruct.Tables(0).Rows.Add(dr)
            ds_CRFStruct.Tables(0).Rows(0)("vWorkSpaceId") = Me.HProjectId.Value
            ds_CRFStruct.Tables(0).Rows(0)("vRemark") = Me.txtRemark.Text
            ds_CRFStruct.Tables(0).Rows(0)("iModifyBy") = Me.Session(S_UserID)
            ds_CRFStruct.AcceptChanges()

            If RblFreeze.SelectedValue = "U" Then
                If Not ObjLambda.insert_CRFVersionMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_CRFStruct, Session(S_UserID), estr) Then
                    Throw New Exception("Errror While UnFreezing Structure Data In CRFVersinMst")
                End If
            Else
                If Not ObjLambda.insert_CRFVersionMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_CRFStruct, Session(S_UserID), estr) Then
                    Throw New Exception("Error While Freezing Structure Data In CRFVersionMst")
                End If
            End If

            If Not FillGrid() Then
                Throw New Exception("Error While Filling Grid")
            End If
            If Not FreezeUnFreeze() Then
                Throw New Exception
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "....btnSave_Click")
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Response.Redirect("frmCRFVersionMst.aspx")
    End Sub

#End Region

#Region "Grid Events"

    Protected Sub GV_CRFVersion_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GV_CRFVersion.RowCommand
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim ds_CDC_CRFVersion As Data.DataSet = Nothing
        Dim dc_Audit As DataColumn
        Try
            If e.CommandName = "Audit" Then
                wstr = Me.HProjectId.Value + "##" + "1"
                If Not ObjHelp.Proc_cdc_dbo_CRFVersionMst_CT(wstr, ds_CDC_CRFVersion, estr) Then
                    Throw New Exception("Error While Geting Whole Detail Of CRF Version")
                End If

                dc_Audit = New DataColumn("dModifyOn_IST", System.Type.GetType("System.String"))
                dc_Audit = New DataColumn("dVersiondate_IST", System.Type.GetType("System.String"))
                ds_CDC_CRFVersion.Tables(0).Columns.Add("dModifyOn_IST")
                ds_CDC_CRFVersion.AcceptChanges()
                ds_CDC_CRFVersion.Tables(0).Columns.Add("dVersiondate_IST")
                ds_CDC_CRFVersion.AcceptChanges()
                For Each dr_Audit In ds_CDC_CRFVersion.Tables(0).Rows
                    dr_Audit("dModifyOn_IST") = Convert.ToString(CDate(dr_Audit("dModifyOn")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)


                    dr_Audit("dVersiondate_IST") = Convert.ToString(CDate(dr_Audit("dVersiondate")).ToString("dd-MMM-yyyy"))
                Next
                ds_CDC_CRFVersion.AcceptChanges()

                For i As Integer = 0 To ds_CDC_CRFVersion.Tables(0).Rows.Count - 1
                    If ds_CDC_CRFVersion.Tables(0).Rows(i)("cFreezeStatus") = "F" Then
                        ds_CDC_CRFVersion.Tables(0).Rows(i)("cFreezeStatus") = "Freeze"
                    Else
                        ds_CDC_CRFVersion.Tables(0).Rows(i)("cFreezeStatus") = "UnFreeze"
                    End If
                Next
                GV_Audit.DataSource = ds_CDC_CRFVersion
                GV_Audit.DataBind()
                MPEID.Show()
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString(), "....GV_CRFVersion_RowCommand")
        End Try
    End Sub

    Protected Sub GV_CRFVersion_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV_CRFVersion.RowDataBound
        Try
            

            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.FindControl("AuditTrail"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("AuditTrail"), ImageButton).CommandName = "Audit"
                If Not Convert.ToString(e.Row.Cells(GVC_CRFVersion_dModifyOn).Text).Trim = "" Then
                    'e.Row.Cells(GVC_CRFVersion_dModifyOn).Text = e.Row.Cells(GVC_CRFVersion_dModifyOn).Text + strServerOffset
                    e.Row.Cells(GVC_CRFVersion_dModifyOn).Text = Convert.ToString(CDate(e.Row.Cells(GVC_CRFVersion_dModifyOn).Text).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)

                End If
                If Not Convert.ToString(e.Row.Cells(GVC_CRFVersion_dVersiondate).Text).Trim = "" Then
                    'e.Row.Cells(GVC_CRFVersion_dVersiondate).Text = e.Row.Cells(GVC_CRFVersion_dVersiondate).Text + strServerOffset
                    e.Row.Cells(GVC_CRFVersion_dVersiondate).Text = Convert.ToString(CDate(e.Row.Cells(GVC_CRFVersion_dVersiondate).Text).ToString("dd-MMM-yyyy"))
                End If
                If hndLockStatus.Value.Trim() = "Lock" Then
                    Me.txtRemark.Attributes.Add("Disabled", "Disabled")
                    Me.btnSave.Attributes.Add("Disabled", "Disabled")
                End If
                If hndLockStatus.Value.Trim() = "UnLock" Then
                    Me.txtRemark.Attributes.Remove("Disabled")
                    Me.btnSave.Attributes.Remove("Disabled")
                End If
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GVAuditTrail_RowDataBound")
        End Try
    End Sub
#End Region


    'Add by shivani pandya for Project lock
#Region "LockImpact"
    <Services.WebMethod()> _
    Public Shared Function LockImpact(ByVal WorkspaceID As String) As String
        Dim ds_Check As DataSet = Nothing
        Dim dtProjectStatus As New DataTable
        Dim ProjectStatus As String = String.Empty
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim iTran As String = String.Empty
        Dim objCommon As New clsCommon

        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()

        wStr = "vWorkspaceId = '" + WorkspaceID.ToString() + "' And cStatusIndi <> 'D'"

        If Not objHelp.GetCRFLockDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_Check, eStr) Then
            Throw New Exception(eStr)
        End If

        If ds_Check.Tables(0).Rows.Count > 0 Then

            dtProjectStatus = ds_Check.Tables(0)
            iTran = dtProjectStatus.Compute("Max(iTranNo)", String.Empty)
            dtProjectStatus.DefaultView.RowFilter = "iTranNo =" + iTran.ToString()

            If dtProjectStatus.DefaultView.ToTable.Rows.Count > 0 Then
                ProjectStatus = dtProjectStatus.DefaultView.ToTable().Rows(0)("cLockFlag").ToString()
            End If
        Else
            ProjectStatus = "U"
        End If

        Return ProjectStatus
    End Function
#End Region
End Class
