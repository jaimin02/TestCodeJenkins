Imports System.Collections
Imports System.Collections.Generic

Partial Class frmSubjectSelectionCriteria
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION "
    Private Const VS_Choice As String = "Choice"
    Private Const GVCell_SrNo As Integer = 0
    Private Const GVCell_SubjectId As Integer = 1
    Private Const GVCell_SubjectName As Integer = 3
    Private Const GVCell_Selected As Integer = 4

    Private Const GVIncExCell_ProtocolWorkspaceCriterienId As Integer = 1

    Dim ObjCommon As New clsCommon
    Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
    Dim eStr_Retu As String


#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Not GenCall() Then
                Exit Sub
            End If
        End If
    End Sub

#Region " GENCALL "

    Private Function GenCall() As Boolean
        Dim dt_DrugScanMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Me.txtDate.Text = DateTime.Today.ToString("dd-MMM-yyyy")

        Try
            Choice = CType("1", WS_Lambda.DataObjOpenSaveModeEnum)
            Me.ViewState("Choice") = Choice   'To use it while saving
            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState("vCaseRecordId") = Me.Request.QueryString("Value").ToString
            End If

            FillProjectPeriodDropDown()
            FillPeriodDropDown(HProjectId.Value)
            GetSetCRFHeader()
            FillGridView()

            GenCall = True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function

#Region "GenCall_ShowUI"

    Private Function GetSetCRFHeader() As Boolean


        Page.Title = ":: Subject Selection Criteriea :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

        If Not Session(S_CaseRecordId) Is Nothing And _
            Not Session(S_ProjectId) Is Nothing And _
            Not Session(S_PeriodId) Is Nothing Then

            Dim dsSetProject As New DataSet

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
            whereCondition += " AND vPeriod = '" + ddlPeriod.SelectedValue.Trim.ToString + "'"
            whereCondition += " AND vLocationCode = " + Session(S_LocationCode)
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
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Function

    Private Function FillPeriodDropDown(ByVal workSpaceId As String) As Boolean
        Dim dsPeriod As New DataSet()
        Try
            If objHelp.FillDropDown("ActivityMst", "vActivityId", "vActivityName", _
                " cStatusIndi <> 'D' And vActivityId in (SELECT DISTINCT vPeriod FROM CRFHdr WHERE vWorkspaceId='" & _
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

    Private Sub FillGridView()

        Dim wStr As String = ""
        Dim dsSubjectCriterien As New DataSet
        If Session(S_CaseRecordId) Is Nothing Then
            wStr = "0000000000"
        Else
            wStr = " vCaseRecordId ='" + Session(S_CaseRecordId).ToString() + "'"
        End If
        wStr += " AND cRejected='N' "

        Try
            If Not objHelp.GetCRFSubSelectionDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                 dsSubjectCriterien, eStr_Retu) Then
                Throw New Exception(eStr_Retu)
            Else
                gvwSubjectSelection.DataSource = dsSubjectCriterien
                gvwSubjectSelection.DataBind()
            End If

        Catch ex As Exception
            ObjCommon.ShowAlert(ex.Message.ToString(), Me)
        End Try

    End Sub

    Private Sub DisableControls()
        txtProject.ReadOnly = True
        ddlPeriod.Enabled = False
    End Sub

    Private Sub EnableControls()
        txtProject.ReadOnly = False
        ddlPeriod.Enabled = True
    End Sub

    Private Sub FillInclusionGrid()
        Dim dsInclusion As New DataSet
        Dim parameterString As String = ""

        parameterString = HProjectId.Value.Trim + "##" + Me.lblSubjectId.Text.Trim + "##" + "I"

        Try
            dsInclusion = objHelp.ProcedureExecute("Proc_SubjectSelectionCriterien", parameterString)
           
            gvwInclusion.DataSource = dsInclusion
            gvwInclusion.DataBind()

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

    Private Sub FillExclusionGrid()
        Dim dsExclusion As New DataSet
        Dim parameterString As String = ""

        parameterString = HProjectId.Value.Trim + "##" + Me.lblSubjectId.Text.Trim + "##" + "E"

        Try
            dsExclusion = objHelp.ProcedureExecute("Proc_SubjectSelectionCriterien", parameterString)
            
            gvwExclusion.DataSource = dsExclusion
            gvwExclusion.DataBind()

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

    Private Sub GetGeneralRemarks()

        Dim dsGenRemarks As New DataSet
        Dim wStr As String = ""

        wStr += " vCaseRecordId = '" + Session(S_CaseRecordId).ToString() + "'"
        wStr += " AND vSubjectId='" + Me.lblSubjectId.Text.Trim + "'"
        If Not objHelp.GetCRFSubSelectionDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                         dsGenRemarks, eStr_Retu) Then
            ShowErrorMessage(eStr_Retu, "")
        Else
            If dsGenRemarks.Tables(0).Rows.Count > 0 Then
                Me.txtGenRemark.Text = dsGenRemarks.Tables(0).Rows(0)("vRemark").ToString()
                If Not dsGenRemarks.Tables(0).Rows(0)("cSelectedFlag") Is DBNull.Value Then
                    rdoLstResult.SelectedValue = dsGenRemarks.Tables(0).Rows(0)("cSelectedFlag").ToString()
                End If

            End If
        End If


    End Sub

#End Region 'End GenCall_ShowUI

#End Region 'End GenCall

#Region " Grid View Events "

    'FOR SUBJECT SELECTION GRID
    Protected Sub gvwSubjectSelection_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwSubjectSelection.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVCell_SrNo).Text = e.Row.RowIndex + 1
            If e.Row.Cells(GVCell_Selected).Text = "Y" Then
                e.Row.Cells(GVCell_Selected).Text = "Yes"
            ElseIf e.Row.Cells(GVCell_Selected).Text = "N" Then
                e.Row.Cells(GVCell_Selected).Text = "No"
            End If

            If e.Row.Cells(GVCell_Selected).Text <> "Yes" And _
              e.Row.Cells(GVCell_Selected).Text <> "No" Then
                Dim lnkBtnEdit As LinkButton = CType(e.Row.FindControl("lnkBtnSubSelEdit"), LinkButton)
                Dim lnkBtnAdd As LinkButton = CType(e.Row.FindControl("lnkBtnSubSelAdd"), LinkButton)
                If (Not lnkBtnEdit Is Nothing) And (Not lnkBtnAdd Is Nothing) Then
                    lnkBtnEdit.Visible = False
                    lnkBtnAdd.Visible = True
                End If
            Else
                Dim lnkBtnEdit As LinkButton = CType(e.Row.FindControl("lnkBtnSubSelEdit"), LinkButton)
                Dim lnkBtnAdd As LinkButton = CType(e.Row.FindControl("lnkBtnSubSelAdd"), LinkButton)
                If (Not lnkBtnEdit Is Nothing) And (Not lnkBtnAdd Is Nothing) Then
                    lnkBtnEdit.Visible = True
                    lnkBtnAdd.Visible = False
                End If
            End If



        End If

        If e.Row.RowType = DataControlRowType.DataRow Or _
            e.Row.RowType = DataControlRowType.Header Or _
            e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVCell_SubjectId).Visible = False

        End If
    End Sub

    Protected Sub gvwSubjectSelection_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvwSubjectSelection.RowEditing
        Dim SubjectId As String = gvwSubjectSelection.Rows(e.NewEditIndex).Cells(GVCell_SubjectId).Text
        Dim SubjectName As String = gvwSubjectSelection.Rows(e.NewEditIndex).Cells(2).Text

        divIncExlGrid.Visible = True
        lblSubjectId.Text = SubjectId
        Me.lblFullName.Text = SubjectName
        Me.FillInclusionGrid()
        Me.FillExclusionGrid()
        Me.GetGeneralRemarks()

    End Sub


    'FOR INCLUSION CRITERIEN GRID
    Protected Sub gvwInclusion_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwInclusion.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVCell_SrNo).Text = e.Row.RowIndex + 1
            e.Row.Cells(GVIncExCell_ProtocolWorkspaceCriterienId).Visible = False

        End If
        If e.Row.RowType = DataControlRowType.Header Or _
            e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(GVIncExCell_ProtocolWorkspaceCriterienId).Visible = False
        End If
    End Sub

    'FOR EXCLUSION CRITERIEN GRID
    Protected Sub gvwExclusion_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwExclusion.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVCell_SrNo).Text = e.Row.RowIndex + 1
            e.Row.Cells(GVIncExCell_ProtocolWorkspaceCriterienId).Visible = False
        End If
        If e.Row.RowType = DataControlRowType.Header Or _
            e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(GVIncExCell_ProtocolWorkspaceCriterienId).Visible = False
        End If
    End Sub

#End Region

#Region " BUTTON EVENTS "

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Me.FillPeriodDropDown(HProjectId.Value.Trim)
        Me.GetSetCRFHeader()
        Me.FillGridView()
        CType(Me.Master.FindControl("ScriptManager1"), ScriptManager).SetFocus(Me.ddlPeriod)
    End Sub

    Protected Sub btnCloseDiv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseDiv.Click
        Me.divIncExlGrid.Visible = False
    End Sub

    Protected Sub btnSaveInExlDtl_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveInExlDtl.Click

        Dim dsIncEx As New DataSet
        Dim dtIncEx As New DataTable
        Dim dsSubSelectionDtl As New DataSet
        Dim dtSubSelectionDtl As New DataTable()
        Dim dummyTranNo As Int16 = 0
        Dim dr As DataRow

        If Not objHelp.GetCRFProtocolCriterienDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                     dsIncEx, eStr_Retu) Then
            Throw New Exception(eStr_Retu)
        Else
            dtIncEx = dsIncEx.Tables(0)
        End If

        If Not objHelp.GetCRFSubSelectionDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                        dsSubSelectionDtl, eStr_Retu) Then
            Throw New Exception(eStr_Retu)
        Else

        End If

        'Assign values for CRFSubSelectionDtl
        dr = dsSubSelectionDtl.Tables(0).NewRow
        dr("vCaseRecordId") = Session(S_CaseRecordId).ToString()
        dr("vSubjectId") = lblSubjectId.Text.Trim().ToString()
        dr("cSelectedFlag") = rdoLstResult.SelectedValue.Trim.ToString()
        dr("iMySubjectNo") = 0
        dr("vRemark") = Me.txtGenRemark.Text.Trim.ToString()
        dr("iModifyBy") = Me.Session(S_UserID).ToString()

        dsSubSelectionDtl.Tables(0).Rows.Add(dr)
        dsSubSelectionDtl.AcceptChanges()
        dtSubSelectionDtl.TableName = "View_CRFSubSelectionDtl"
        dtSubSelectionDtl = dsSubSelectionDtl.Tables(0).Copy()


        'SAVING LOGIC FOR INCLUSION CRITERIA
        For Each gvr As GridViewRow In gvwInclusion.Rows
            dr = dtIncEx.NewRow()
            dr("vCaseRecordId") = Session(S_CaseRecordId).ToString()
            dr("vSubjectId") = lblSubjectId.Text.Trim().ToString()
            dr("vProtocolWorkspaceCriterienId") = gvr.Cells(1).Text.ToString()
            dr("iTranNo") = dummyTranNo
            dr("dResultDateTime") = DateTime.Now
            dr("vResultRemark") = CType(gvr.FindControl("txtRemark"), TextBox).Text

            Dim rdoLstInclusionResult As RadioButtonList = gvr.FindControl("rdoLstInclusionResult")
            For Each lstItem As ListItem In rdoLstInclusionResult.Items
                If lstItem.Selected Then
                    dr("cResultFlag") = lstItem.Value
                End If
            Next

            dr("iModifyBy") = Session(S_UserID)
            dtIncEx.Rows.Add(dr)
            dummyTranNo = dummyTranNo + 1
        Next

        'SAVING LOGIC FOR EXCLUSION CRITERIA
        For Each gvr As GridViewRow In gvwExclusion.Rows
            dr = dtIncEx.NewRow()
            dr("vCaseRecordId") = Session(S_CaseRecordId).ToString()
            dr("vSubjectId") = lblSubjectId.Text.Trim().ToString()
            dr("vProtocolWorkspaceCriterienId") = gvr.Cells(1).Text.ToString()
            dr("iTranNo") = dummyTranNo
            dr("dResultDateTime") = DateTime.Now
            dr("vResultRemark") = CType(gvr.FindControl("txtRemark"), TextBox).Text

            Dim rdoLstInclusionResult As RadioButtonList = gvr.FindControl("rdoLstInclusionResult")
            For Each lstItem As ListItem In rdoLstInclusionResult.Items
                If lstItem.Selected Then
                    dr("cResultFlag") = lstItem.Value
                End If
            Next

            dr("iModifyBy") = Session(S_UserID)
            dtIncEx.Rows.Add(dr)
            dummyTranNo = dummyTranNo + 1
        Next

        dsIncEx.Tables.Add(dtSubSelectionDtl)


        'If Not objLambda.Save_InsertCRFProtocolCriterienDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
        '                    dsIncEx, "1", eStr_Retu) Then
        '    ObjCommon.ShowAlert(eStr_Retu, Me)
        '    ShowErrorMessage(eStr_Retu, "")
        'Else
        '    Me.divIncExlGrid.Visible = False
        '    Me.FillGridView()
        '    ObjCommon.ShowAlert("Selection Criterien Information Saved Successfully", Me)


        'End If
        

    End Sub

    Protected Sub lnkBtnSubSelAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dsSubSel As New DataSet 'For Inclusion grid
        Dim wStr As String = "" 'For Where condition
        Dim dsSubSelExc As New DataSet 'For Exclusion grid
        Dim gridViewRow As GridViewRow = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow)
        Dim SubjectId As String = gridViewRow.Cells(GVCell_SubjectId).Text
        Dim SubjectName As String = gridViewRow.Cells(GVCell_SubjectName).Text

        divIncExlGrid.Visible = True
        Me.lblSubjectId.Text = SubjectId
        Me.lblFullName.Text = SubjectName

        'Fill Inclusion Grid for Add Mode
        wStr = "vWorkspaceId='" + HProjectId.Value.Trim + "'"
        wStr += " AND cIncExFlag = 'I'"

        If Not objHelp.GetFieldsOfTable("ProtocolWorkspaceCriterienDtls", "vProtocolWorkspaceCriterienId, vCriterienDescription, cIncExFlag, '' as vResultRemark, 'A' as cResultFlag ", _
                    wStr, dsSubSel, eStr_Retu) Then
            Throw New Exception(eStr_Retu)
        Else
            gvwInclusion.DataSource = dsSubSel
            gvwInclusion.DataBind()
        End If


        'Fill Exclusion Grid for Add Mode
        wStr = "vWorkspaceId='" + HProjectId.Value.Trim + "'"
        wStr += " AND cIncExFlag = 'E'"

        If Not objHelp.GetFieldsOfTable("ProtocolWorkspaceCriterienDtls", "vProtocolWorkspaceCriterienId, vCriterienDescription, cIncExFlag, '' as vResultRemark, 'A' as cResultFlag ", _
                    wStr, dsSubSelExc, eStr_Retu) Then
            Throw New Exception(eStr_Retu)
        Else
            gvwExclusion.DataSource = dsSubSelExc
            gvwExclusion.DataBind()
        End If

        Me.txtGenRemark.Text = ""
        Me.rdoLstResult.Items(1).Selected = True


    End Sub

#End Region

#Region " DROP DOWN LIST EVENTS "

    Protected Sub ddlPeriod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPeriod.SelectedIndexChanged
        Me.GetSetCRFHeader()
        Me.FillGridView()
        CType(Me.Master.FindControl("ScriptManager1"), ScriptManager).SetFocus(Me.ddlPeriod)
    End Sub

#End Region

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

        Dim ObjCommon As New clsCommon
        Dim objHlp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()

        result = objHlp.GetFieldsOfTable("WorkspaceMst", " vWorkspaceID, vWorkspaceDesc  ", " vWorkspaceDesc" + " Like '%" + prefixText + "%'", ds, estr)

        Dim items As List(Of String) = New List(Of String)(count)

        For index As Integer = 0 To ds.Tables(0).Rows.Count - 1
            dr = ds.Tables(0).Rows(index)
            items.Add("'" + dr.Item("vWorkspaceId").ToString + "#" + dr.Item("vWorkspaceDesc").ToString)
        Next
        Return items.ToArray()
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
