
Partial Class frmProtocolWorkSpaceDetails
    Inherits System.Web.UI.Page

    Dim ObjCommon As New clsCommon
    Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private Const VS_Choice As String = "Choice"
    Private Const VS_SAVE As String = "Save"
    Private Const VS_DtProtocolWorkSpaceDetails As String = "DtProtocolWorkSpaceDetails"
    Private Const VS_WorkspaceId As String = "WorkspaceId"
#Region "Page Load "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                CType(Me.Master.FindControl("lblHeading"), Label).Text = "Protocol Workspace Detail"
                Me.Divgeneral.Disabled = True
            End If
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "GenCall "
    Private Function GenCall() As Boolean
        Dim ds As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Choice = Me.Request.QueryString("Mode")
            Me.ViewState(VS_Choice) = Choice   'To use it while saving
            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_WorkspaceId) = Me.Request.QueryString("Value").ToString
            End If

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtProtocolWorkSpaceDetails) = ds.Tables("ProtocolWorkSpaceDetails")   ' adding blank DataTable in viewstate

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

#Region "GenCall_Data "

    Private Function GenCall_Data(ByRef ds_Retu As DataSet) As Boolean

        Dim wStr As String = ""
        Dim eStr_Retu As String = ""
        Dim Val As String = ""
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing
        'Dim objFFR As New FFReporting.FFReporting

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Val = Me.ViewState(VS_WorkspaceId) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vWorkspaceId=" + Val.ToString
            End If


            If Not objHelp.getProtocolWorkSpaceDetails(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If

            If ds Is Nothing Then
                Throw New Exception(eStr_Retu)
            End If

            If ds.Tables(0).Rows.Count <= 0 And _
             Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Throw New Exception("No Records Found for Selected Operation")
            End If

            ds_Retu = ds
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")

        Finally

        End Try
    End Function
#End Region

#Region " GenCall_showUI() "
    Private Function GenCall_ShowUI() As Boolean
        Dim ProtocolWorkSpaceDetails As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try
            Page.Title = ":: Protocol Project Details   :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Protocol Workspace Detail"

            ProtocolWorkSpaceDetails = Me.ViewState(VS_DtProtocolWorkSpaceDetails)

            Choice = Me.ViewState("Choice")

            'If Me.ViewState(VS_SAVE) Is Nothing Then 'For Save
            '    Me.ViewState(VS_SAVE) = Me.Request.QueryString("Save")
            'End If
            'If Me.ViewState(VS_SAVE) = "Y" Then
            '    ObjCommon.ShowAlert("Record Saved SuccessFully", Me.Page)
            '    Me.ViewState(VS_SAVE) = "N"
            'End If '***********************************************
            If Not FillDropDown() Then
                Exit Function
            End If
            If Not FillGrid() Then
                Exit Function
            End If
            

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                'Me.txtUserTypeName.Text = dt_OpMst.Rows(0).Item("vUserTypeName")
                'Me.txtCriterianDesc.Text = dt_OpMst.Rows(0).Item("vProtocolCriterienDescription")
                'Me.chkActive.Checked = IIf(dt_OpMst.Rows(0).Item("cActiveFlag").ToString.ToUpper = "Y", True, False)
                'Me.DDLCriterienType.SelectedValue = dt_OpMst.Rows(0).Item("cProtocolCriterienType")
            End If
            Me.BtnSave.Attributes.Add("OnClick", "return Validation();")

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function
#End Region

#Region "FillDropDown"
    Private Function FillDropDown() As Boolean
        Dim Ds_Project As New DataSet
        Dim Ds_Detail As New DataSet
        Dim estr As String = ""
        Try
            Me.objHelp.GetFieldsOfTable("WorkSpaceProtocolDetail " & _
                " inner join drugmst on WorkSpaceProtocolDetail.vDrugCode=drugmst.vDrugCode" & _
                "", " * ", "vWorkSpaceId='" & Me.HWorkspaceId.Value.Trim() & "'", Ds_Detail, estr)
            'Me.objHelp.FillDropDown("WorkspaceMst", "vWorkSpaceId", "vWorkSpaceDesc", "", Ds_Project, estr)

            Me.txtDrug.Text = Ds_Detail.Tables(0).Rows(0).Item("vDrugName")
            Me.txtTestProduct.Text = Ds_Detail.Tables(0).Rows(0).Item("vTPGenericName")
            Me.txtRefProduct.Text = Ds_Detail.Tables(0).Rows(0).Item("vBrandName")
            Me.txtStrength.Text = Ds_Detail.Tables(0).Rows(0).Item("vStrength")
            Me.txtFormulation.Text = Ds_Detail.Tables(0).Rows(0).Item("vFormulationType")
            Me.txtDrug.Enabled = False
            Me.txtTestProduct.Enabled = False
            Me.txtRefProduct.Enabled = False
            Me.txtStrength.Enabled = False
            Me.txtFormulation.Enabled = False
            'Me.DDLProject.DataSource = Ds_Project
            'Me.DDLProject.DataTextField = "vWorkSpaceDesc"
            'Me.DDLProject.DataValueField = "vWorkSpaceId"
            'Me.DDLProject.DataBind()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
#End Region

#Region "FillGrid"
    Private Function FillGrid() As Boolean
        Dim ds_View As New Data.DataSet
        Dim dv_View As New Data.DataView
        Dim ds_View2 As New Data.DataSet
        Dim estr As String = ""
        Try
            'objHelp.getOperationMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_PerentOp, estr)
            If Not objHelp.getProtocolCriterienMst("cActiveflag <> 'N'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_View, estr) Then
                Return False
            End If
            dv_View = ds_View.Tables(0).DefaultView

            'For Pre-Dose Grid
            dv_View.RowFilter = "cProtocolCriterienType='D'"
            Me.GV_Pre.DataSource = dv_View
            Me.GV_Pre.DataBind()

            'For Post-Dose Grid
            dv_View.RowFilter = "cProtocolCriterienType='P'"
            Me.GV_Post.DataSource = dv_View
            Me.GV_Post.DataBind()

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

#End Region

#Region "Button Click"

    Protected Sub BtnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGo.Click
        Me.Divgeneral.Disabled = False
        GenCall()
    End Sub

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim ds_Save As New DataSet
        Dim dt_Save As New DataTable
        Dim estr As String = ""
        Try
            AssignValues()
            ds_Save = New DataSet
            dt_Save = CType(Me.ViewState(VS_DtProtocolWorkSpaceDetails), DataTable)
            dt_Save.TableName = "ProtocolWorkSpaceDetails"
            ds_Save.Tables.Add(dt_Save)
            If Not objLambda.Save_InsertProtocolWorkSpaceDetails(Me.ViewState(VS_Choice), ds_Save, Me.Session(S_UserID), estr) Then
                ObjCommon.ShowAlert("Error While Saving ProtocolWorkSpaceDetails", Me.Page)
                Exit Sub
            Else
                ObjCommon.ShowAlert("Record Saved SuccessFully", Me.Page)
                ResetPage()
            End If


        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        ResetPage()
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

#End Region

#Region "Assign values"
    Private Sub AssignValues()
        Dim dr As DataRow
        Dim dt_ProtocolWorkSpaceDetails As New DataTable
        Dim i As Integer
        Try

            dt_ProtocolWorkSpaceDetails = CType(Me.ViewState(VS_DtProtocolWorkSpaceDetails), DataTable)
            dt_ProtocolWorkSpaceDetails.Clear()
            dr = dt_ProtocolWorkSpaceDetails.NewRow()
            'vWorkspaceId,iTranNo,nWaterQty,nAdditinalWater,vWaterUOM,nHousing,nWashoutMax,nWashoutMin,vHousingUOM,
            'vPreDoseRestriction1, vPreDoseRestrivtion2, vPreDoseRestriction3, vPreDoseRestriction4, vPostDoseRestriction1, vPostDoseRestriction2, vPostDoseRestriction3, vPostDoseRestriction4, 
            'nNoofSamples, nSampleQty, nAdditionalSampleQty, nSampleStartHours, nSampleIntervalHrs, iModifyBy, dModifyOn, cStatusIndi
            dr("vWorkspaceId") = Me.HWorkspaceId.Value.Trim()
            dr("iTranNo") = "0"
            dr("nWaterQty") = Me.txtWaterMin.Text.Trim()
            dr("nAdditinalWater") = Me.txtWaterMax.Text.Trim()
            dr("vWaterUOM") = "ml."
            dr("nHousing") = Me.txtHousing.Text.Trim()
            dr("nWashoutMax") = Me.txtWoshOutMax.Text.Trim()
            dr("nWashoutMin") = Me.txtWoshOutMin.Text.Trim()
            dr("vHousingUOM") = "Hrs."
            For i = 0 To Me.GV_Pre.Rows.Count - 1
                Dim strPreCol As String
                strPreCol = "vPreDoseRestriction" + (i + 1).ToString.Trim()
                dr(strPreCol) = CType(Me.GV_Pre.Rows(i).FindControl("txtPre"), TextBox).Text.Trim()
            Next
            For i = 0 To Me.GV_Post.Rows.Count - 1
                Dim strPostCol As String
                strPostCol = "vPostDoseRestriction" + (i + 1).ToString.Trim()
                dr(strPostCol) = CType(Me.GV_Post.Rows(i).FindControl("txtPost"), TextBox).Text.Trim()
            Next
            dr("nNoofSamples") = Me.txtsampleNo.Text.Trim()
            dr("nSampleQty") = Me.txtSampleQty.Text.Trim()
            dr("nAdditionalSampleQty") = Me.txtAdditionalSampleQty.Text.Trim()
            dr("nSampleStartHours") = Me.txtSampleStarts.Text.Trim()
            dr("nSampleIntervalHrs") = Me.txtSampleInterval.Text.Trim()
            dr("cStatusIndi") = "N"
            dr("iModifyBy") = Me.Session(S_UserID)
            dt_ProtocolWorkSpaceDetails.Rows.Add(dr)
            Me.ViewState(VS_DtProtocolWorkSpaceDetails) = dt_ProtocolWorkSpaceDetails
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub
#End Region

#Region "Reset Page"
    Private Sub ResetPage()
        'Me.txt = ""
        'Me.DDLCriterienType.SelectedIndex = 0
        'Me.chkActive.Checked = False

        Me.ViewState(VS_DtProtocolWorkSpaceDetails) = Nothing
        Me.Divgeneral.Disabled = True
        'Me.Response.Redirect("frmProtocolWorkSpaceDetails.aspx?mode=1")
        'Me.Response.Redirect("frmUserTypeMst.aspx?mode=1&Save=Y")
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

#Region "Grid Event"
    Protected Sub GV_Pre_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV_Pre.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("LblPreSrNo"), Label).Text = e.Row.RowIndex + 1
        End If
    End Sub

    Protected Sub GV_Post_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GV_Post.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("LblPostSrNo"), Label).Text = e.Row.RowIndex + 1
        End If
    End Sub
#End Region

End Class
