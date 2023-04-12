
Partial Class frmDrugScanReport
    Inherits System.Web.UI.Page

#Region " VARIABLE DECLARATION "

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtCRFAttendence As String = "DtCRFAttendence"
    Private Const VS_CaseRecordId As String = "vCaseRecordId"
    Private Const VS_SubjectId As String = "vSubjectId"
    Private Const VS_TranNo As String = "iTranNo"
    Private Const VS_SrNo As String = "iSrNo"
    Private Const VS_Date As String = "SavedDate"
    Private Const GVCell_SrNo As Integer = 0
    Private Const GVCell_SubjectId As Integer = 1
    Private Const GVCell_ReportingTime As Integer = 3
    Private Const GVCell_Positive As Integer = 4
    Private Const GVCell_Rejected As Integer = 6


    Dim ObjCommon As New clsCommon
    Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()


    Dim eStr_Retu As String

#End Region
#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Page.Title = " :: Drug Scan Report ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
        CType(Master.FindControl("lblHeading"), Label).Text = "Drug Scan Master"

        If Not IsPostBack Then
            GenCall()
            FillProjectPeriodDropDown()
            FillPeriodDropDown(HProjectId.Value)
            GetSetCRFHeader()
            GriViewEvents()
        End If
    End Sub
#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean
        Dim dt_DrugScanMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Me.txtDrugScanDate.Text = DateTime.Today.ToString("dd-MMM-yyyy")
        Me.imgDate.Disabled = True
        Try
            Choice = CType("1", WS_Lambda.DataObjOpenSaveModeEnum)
            Me.ViewState("Choice") = Choice   'To use it while saving
            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState("vCaseRecordId") = Me.Request.QueryString("Value").ToString
            End If
            ''''Check for Valid User''''''''''''''
            If Not GenCall_Data(Choice, dt_DrugScanMst) Then ' For Data Retrieval
                Exit Function
            End If
            Me.ViewState("dtDrugScanMst") = dt_DrugScanMst ' adding blank DataTable in viewstate
            'If Not GenCall_ShowUI(Choice, dt_DrugScanMst) Then 'For Displaying Data 
            '    Exit Function
            'End If
            GenCall = True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function

#Region "GenCall_Data"
    Private Function GenCall_Data(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                            ByRef dt_Dist_Retu As DataTable) As Boolean

        Dim eStr As String = ""
        Dim wStr As String = ""
        Dim ds_ClientMst As DataSet = Nothing
        'Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable
        Try

            If Choice_1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vCaseRecordId=" + Me.ViewState("vCaseRecordId").ToString() 'Value of where condition
            End If
            If Not objHelp.getclientmst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ClientMst, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_ClientMst Is Nothing Then
                Throw New Exception(eStr)
            End If
            If ds_ClientMst.Tables(0).Rows.Count <= 0 And _
               Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                Throw New Exception("No Records Found for Selected role")

            End If
            dt_Dist_Retu = ds_ClientMst.Tables(0)
            GenCall_Data = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function
#End Region

#Region "GenCall_ShowUI"

    Private Function GetSetCRFHeader() As Boolean

        Dim dsSetProject As New DataSet

        If Not Session(S_CaseRecordId) Is Nothing And _
            Not Session(S_ProjectId) Is Nothing And _
            Not Session(S_PeriodId) Is Nothing Then

            If Not objHelp.GetFieldsOfTable("WorkspaceMst", " * ", "vWorkspaceId='" + Session(S_ProjectId) + "'", _
                dsSetProject, eStr_Retu) Then
                ObjCommon.ShowAlert("Problem getting Project Information", Me)
            End If
            Me.txtProject.Text = dsSetProject.Tables(0).Rows(0)("vWorkspaceDesc")


            HProjectId.Value = Session(S_ProjectId).ToString
            ddlPeriod.SelectedValue = Session(S_PeriodId).ToString

            DisableControls()
            Return True
        Else
            Dim dsCRFHdr As New DataSet
            Dim whereCondition As String = ""
            'whereCondition = " vWorkspaceId = '" + ddlProject.SelectedValue.ToString.Trim + "'"
            whereCondition = " vWorkspaceId = '" + IIf(String.IsNullOrEmpty(HProjectId.Value), "0000000001", HProjectId.Value) + "'"
            whereCondition += " AND iPeriod = " + ddlPeriod.SelectedValue.Trim.ToString
            whereCondition += " AND vLocationCode = '" + Session(S_LocationCode) + "'"
            If objHelp.GetFieldsOfTable("CRFHdr", " vCaseRecordId ", whereCondition, dsCRFHdr, eStr_Retu) Then
                If dsCRFHdr.Tables(0).Rows.Count > 0 Then
                    Session(S_CaseRecordId) = dsCRFHdr.Tables(0).Rows(0).Item(0).ToString
                Else
                    Session(S_CaseRecordId) = "0000000000"
                End If
            End If
        End If

        EnableControls()
        Return True

    End Function

    Private Function FillProjectPeriodDropDown() As Boolean
        Dim dsDrugProject As New DataSet
        Dim eStr_Retu As String = ""

        Try
            Me.objHelp.FillDropDown("workspacemst", "vWorkspaceId", "vWorkspaceDesc", " vWorkspaceId in (SELECT DISTINCT vWorkspaceId FROM CRFHdr)", dsDrugProject, eStr_Retu)
            HProjectId.Value = dsDrugProject.Tables(0).Rows(0)(0).ToString
            txtProject.Text = dsDrugProject.Tables(0).Rows(0)(1).ToString
            'ddlProject.DataSource = dsDrugProject
            'ddlProject.DataValueField = "vWorkspaceId"
            'ddlProject.DataTextField = "vWorkspaceDesc"
            'ddlProject.DataBind()
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Function

    Private Function FillPeriodDropDown(ByVal workSpaceId As String) As Boolean
        Dim dsPeriod As New DataSet()
        Try
            If objHelp.FillDropDown("ActivityMst", "vActivityId", "vActivityName", _
                " vActivityId in (SELECT DISTINCT vPeriod FROM CRFHdr WHERE vWorkspaceId='" & _
                    IIf(String.IsNullOrEmpty(workSpaceId), "0000000001", workSpaceId) & "'" + ")", _
                dsPeriod, eStr_Retu) Then
                If dsPeriod.Tables(0).Rows.Count > 0 Then
                    ddlPeriod.DataSource = dsPeriod
                    ddlPeriod.DataValueField = "vActivityId"
                    ddlPeriod.DataTextField = "vActivityName"
                    ddlPeriod.DataBind()
                End If
            End If
        Catch ex As Exception
            ShowErrorMessage("Error Binding Period Drop Down - " + ex.Message, "")
        End Try
        FillPeriodDropDown = True
    End Function

#End Region

#End Region

#Region "GridView Events"
    Private Function GriViewEvents() As Boolean
        Dim eStr_retu As String = ""
        Dim ds As New DataSet
        If Not objHelp.GetCRFDrugScanReport(" vCaseRecordId=" + Session(S_CaseRecordId).ToString() + " AND cRejected='N'" + " ", _
        WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_retu) Then
        Else
            gvwSubjectDrugScan.DataSource = ds
            gvwSubjectDrugScan.DataBind()
            GetOperationRemark()
        End If
    End Function
#End Region

#Region "Reset Page"
    Private Sub ResetPage()
        'Me.txtClientName.Text = ""
        'Me.txtremark.Text = ""
        'Response.Redirect("frmClientMst.aspx?mode=1&choice=1")
    End Sub
#End Region

#Region "Save"

    Private Function AssignUpdatedValues() As Boolean
        Dim dtOld As New DataTable
        Dim dr As DataRow
        Try
            dtOld = Me.ViewState("dtDrugScanMst")
            dr = dtOld.NewRow
            dr("vCaseRecordId") = "0000000000"
            dr("vSubjectId") = "0000000000"
            dr("vWorkspaceId") = HProjectId.Value 'Me.ddlProject.SelectedItem.Value.Trim
            dr("vActivityId") = Me.ddlPeriod.SelectedItem.Value.Trim
            'dr("dstartdate")= 
            dr("vTestedFor") = Me.txtTestedFor.Text.Trim
            'dr("vPositiveTestCode")= 
            'dr("vRepeatAnalysisDetails")=
            'dr("cFinalResult")=
            dr("iModifyBy") = Session(S_UserID)
            dtOld.Rows.Add(dr)
            Me.ViewState("dtDrugScanMst") = dtOld
            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles btnSave.Click

        Dim Dt As New DataTable
        Dim Ds_clientmst As DataSet
        Dim Ds_clientmstgrid As New DataSet
        Dim eStr As String = ""
        'Dim objOPws As New WS_Lambda.WS_Lambda

        Try
            AssignUpdatedValues()
            Ds_clientmst = New DataSet
            Ds_clientmst.Tables.Add(CType(Me.ViewState("dtDrugScanMst"), Data.DataTable).Copy())
            Ds_clientmst.Tables(0).TableName = "CRFDrugScanReport"   ' New Values on the form to be updated

            'If Not objLambda.save_CRFDrugScanReport(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_clientmst, Ds_clientmst, "1", eStr) Then
            '    ShowErrorMessage(eStr, "")
            '    Exit Sub
            'Else
            '    ObjCommon.ShowAlert("Drug Report Saved Successfully", Me)
            '    ResetPage()
            '    If Not objHelp.GetCRFDrugScanReport("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, Ds_clientmstgrid, eStr) Then
            '        ' objcommon.ShowAlert("Error while FillGrid", Me.Page)
            '    End If
            '    Me.gvwSubjectDrugScan.DataSource = Ds_clientmstgrid
            '    Me.gvwSubjectDrugScan.DataBind()
            '    'ResetPage()
            'End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr)
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

#Region "DrugRemarks"

    Private Function AssignValuesDrugRemarks() As Boolean
        Dim OperationCode As String = "0062"

        Dim dsRemarks As New DataSet
        Dim dtRemarks As New DataTable
        Dim drRemarks As DataRow

        If objHelp.GetCRFOperationRemarkDtl("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                dsRemarks, eStr_Retu) Then
            dtRemarks = dsRemarks.Tables(0)
            drRemarks = dtRemarks.NewRow
            drRemarks("vCaseRecordId") = Me.Session(S_CaseRecordId)
            drRemarks("vOperationCode") = "0062"
            drRemarks("iTranNo") = "2"
            drRemarks("vRemark") = txtAllRemarks.Text.Trim
            drRemarks("iModifyBy") = Session(S_UserID)

            dtRemarks.Rows.Add(drRemarks)
        Else
            ShowErrorMessage(eStr_Retu, "")
        End If

        'If objLambda.Save_InsertCRFOperationRemarkDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
        '        dsRemarks, "1", eStr_Retu) Then
        '    ObjCommon.ShowAlert("Drug Scan Comment saved successfully.".Trim, Me)
        'Else
        '    ShowErrorMessage(eStr_Retu, "")
        'End If


    End Function

    Private Sub GetOperationRemark()
        Dim wStr As String = ""
        Dim dsTemp As New DataSet
        If Session(S_CaseRecordId) Is Nothing Then
            wStr = " vCaseRecordId = '0000000000'"
        Else
            wStr = " vCaseRecordId = '" + Session(S_CaseRecordId).ToString() + "'"
        End If
        wStr += " AND vOperationCode='0062'"

        If objHelp.GetCRFOperationRemarkDtl(wStr, _
                                    WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsTemp, eStr_Retu) Then
            If dsTemp.Tables(0).Rows.Count > 0 Then
                txtAllRemarks.Text = dsTemp.Tables(0).Rows(0)("vRemark")
            Else
                txtAllRemarks.Text = ""
            End If

        End If
    End Sub

#End Region

#Region "Cancel"
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles btnCancel.Click
        ResetPage()
    End Sub
#End Region

#Region "Drop Down List Events"

    Protected Sub ddlProject_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles ddlProject.SelectedIndexChanged
        FillPeriodDropDown(HProjectId.Value)
        GetSetCRFHeader()
        Me.GriViewEvents()
    End Sub

    Protected Sub ddlPeriod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPeriod.SelectedIndexChanged
        GetSetCRFHeader()
        Me.GriViewEvents()
    End Sub

#End Region

    '#Region "PAGE METHOD FOR AUTOCOMPLETE EXTENDER"

    '    <System.Web.Services.WebMethod()> _
    '        <System.Web.Script.Services.ScriptMethod()> _
    '        Public Shared Function GetCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
    '        If count = 0 Then
    '            count = 10
    '        End If

    '        Dim ds As New DataSet()
    '        Dim estr As String = ""
    '        Dim result As Boolean = False
    '        Dim dr As DataRow = Nothing

    '        Dim objHlp As New WS_HelpDbTable.WS_HelpDbTable

    '        result = objHlp.GetFieldsOfTable("WorkspaceMst", " vWorkspaceID, vWorkspaceDesc  ", " vWorkspaceDesc" + " Like '%" + prefixText + "%'", ds, estr)

    '        Dim items As List(Of String) = New List(Of String)(count)

    '        For index As Integer = 0 To ds.Tables(0).Rows.Count - 1
    '            dr = ds.Tables(0).Rows(index)
    '            items.Add("'" + dr.Item("vWorkspaceId").ToString + "#" + dr.Item("vWorkspaceDesc").ToString)
    '        Next
    '        Return items.ToArray()
    '    End Function

    '#End Region

#Region " BUTTON EVENTS "

    Protected Sub btnProjectSelect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        FillPeriodDropDown(HProjectId.Value)
        GetSetCRFHeader()
        gvwSubjectDrugScan.EditIndex = -1
        Me.GriViewEvents()
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles btndiv.Click
        'divdrugscan.Visible = False
    End Sub

    Protected Sub btnanalysis_Click(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles btnanalysis.Click
        'divdrugscan.Visible = True
    End Sub

    Protected Sub btnRemarks_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        AssignValuesDrugRemarks()
    End Sub

#End Region



    Private Sub DisableControls()
        txtProject.ReadOnly = True
        ddlPeriod.Enabled = False
    End Sub

    Private Sub EnableControls()
        txtProject.ReadOnly = False
        ddlPeriod.Enabled = True
    End Sub

    Private Sub AssignValues()

        'Dim dsDrugScan As New DataSet
        'Dim dtDrugScan As New DataTable


        'If objHelp.GetCRFDrugScanReport("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
        '        dsDrugScan, eStr_Retu) Then
        '    dtDrugScan = dsDrugScan.Tables(0)

        '    dr = dtDrugScan.NewRow()

        '    dr("vCaseRecordId") = Session(S_CaseRecordId).ToString
        '    dr("dStartDate") = Me.txtDrugScanDate.Text


        'End If

    End Sub


#Region "GRID VIEW EVENTS"

    Protected Sub gvclient_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

    End Sub

    Protected Sub gvclient_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
    End Sub

    Protected Sub gvwSubjectDrugScan_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVCell_SrNo).Text = e.Row.RowIndex + 1
            'If Not String.IsNullOrEmpty(CType(e.Row.FindControl("lblStartTime"), Label).Text) Then
            '    CType(e.Row.FindControl("lblStartTime"), Label).Text = DateTime.Parse(CType(e.Row.FindControl("lblStartTime"), Label).Text).ToString("dd-MMM-yyyy HH:mm")
            'End If
        End If
    End Sub

    Protected Sub gvwSubjectDrugScan_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs)

        Dim cbl As CheckBoxList
        Dim arr() As String
        Dim positive As String
        Dim finalResult As String


        gvwSubjectDrugScan.EditIndex = e.NewEditIndex
        positive = CType(gvwSubjectDrugScan.Rows(gvwSubjectDrugScan.EditIndex).FindControl("lblPositive"), Label).Text
        finalResult = CType(gvwSubjectDrugScan.Rows(gvwSubjectDrugScan.EditIndex).FindControl("lblFinalResult"), Label).Text

        Me.GriViewEvents()



        cbl = CType(gvwSubjectDrugScan.Rows(gvwSubjectDrugScan.EditIndex).FindControl("cblPositive"), CheckBoxList)
        arr = txtTestedFor.Text.Trim.Split(",".ToCharArray())
        cbl.Items.Clear()
        cbl.DataSource = arr
        cbl.DataBind()


        arr = IIf(positive.Length > 0, positive.Split(",".ToCharArray), Nothing)
        If Not arr Is Nothing Then
            For Each lstItem As ListItem In cbl.Items
                For Each strPositive As String In arr
                    If lstItem.Value = strPositive Then
                        lstItem.Selected = True
                        Exit For
                    End If
                Next
            Next
        End If

        If finalResult.Length > 0 Then
            CType(gvwSubjectDrugScan.Rows(gvwSubjectDrugScan.EditIndex).FindControl("ddlFinalResult"), DropDownList).SelectedValue = finalResult.Trim
        End If

        Dim txt As TextBox = CType(gvwSubjectDrugScan.Rows(gvwSubjectDrugScan.EditIndex).FindControl("txtStartTime"), TextBox)
        CType(Me.Master.FindControl("ScriptManager1"), ScriptManager).SetFocus(txt)

    End Sub

    Protected Sub gvwSubjectDrugScan_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs)
        gvwSubjectDrugScan.EditIndex = -1
        Me.GriViewEvents()
    End Sub

    Protected Sub gvwSubjectDrugScan_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs)

        Dim gvr As GridViewRow = gvwSubjectDrugScan.Rows(e.RowIndex)
        Dim cbl As CheckBoxList
        Dim positive As String
        Dim dsDrugScan As New DataSet
        Dim dtDrugScan As New DataTable
        Dim dr As DataRow

        Try


            If objHelp.GetCRFDrugScanReport("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                    dsDrugScan, eStr_Retu) Then
                dtDrugScan = dsDrugScan.Tables(0)

                dr = dtDrugScan.NewRow()
                dr("vSubjectId") = CType(gvr.Cells(GVCell_SubjectId).Controls(1), Label).Text
                dr("vCaseRecordId") = Session(S_CaseRecordId).ToString
                dr("dStartDate") = Me.txtDrugScanDate.Text + " " + CType(gvr.FindControl("txtStartTime"), TextBox).Text + ":00"
                cbl = CType(gvr.FindControl("cblPositive"), CheckBoxList)
                positive = ""
                For Each lstItem As ListItem In cbl.Items
                    If lstItem.Selected Then
                        positive += lstItem.Value + ","
                    End If
                Next
                If positive.Length > 0 Then
                    positive = positive.Substring(0, positive.Length - 1)
                End If

                dr("vPositiveTestCodes") = positive
                dr("vTestedFor") = Me.txtTestedFor.Text.Trim
                dr("vRepeatAnalysisDetails") = CType(gvr.FindControl("txtRepeatDetails"), TextBox).Text
                dr("cFinalResult") = CType(gvr.FindControl("ddlFinalResult"), DropDownList).SelectedValue
                dr("cTestTypeIndi") = "D"
                dr("iModifyby") = Session(S_UserID)

                dtDrugScan.Rows.Add(dr)
                dtDrugScan.AcceptChanges()
            End If

            'If Not objLambda.save_CRFDrugScanReport(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_CRFDrugScanReport, _
            '                             dsDrugScan, Session(S_UserID), eStr_Retu) Then

            '    ObjCommon.ShowAlert(eStr_Retu, Me)
            'Else
            '    ObjCommon.ShowAlert("Subject Drug Scan Report Added Successfully...", Me)
            '    gvwSubjectDrugScan.EditIndex = -1
            '    Me.GriViewEvents()
            'End If

        Catch ex As Exception

        End Try

    End Sub

#End Region
End Class
