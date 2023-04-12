Imports System.Web.Services
Imports Newtonsoft.Json
Imports Microsoft.Win32
Imports Microsoft
Imports Microsoft.Office.Interop
Imports System.IO
Imports System.Drawing

Partial Class frmDrugMst
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION "

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtDrugMst As String = "DtDrugMst"
    Private Const VS_DrugCode As String = "DrugCode"

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_Code As Integer = 1
    Private Const GVC_DrugName As Integer = 2
    Private Const GVC_WashOutPeriod As Integer = 3
    Private Const GVC_Housing As Integer = 4
    Private Const GVC_DrugSynonyms As Integer = 5
    Private Const GVC_FoodEffect As Integer = 6
    Private Const GVC_ActiveFlag As Integer = 7
    Private Const GVC_Edit As Integer = 8

#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            If Not IsPostBack Then

                GenCall()

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
            Choice = Me.Request.QueryString("Mode")
            Me.ViewState(VS_Choice) = Choice   'To use it while saving

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_DrugCode) = Me.Request.QueryString("Value").ToString
            End If

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtDrugMst) = ds.Tables("DrugMst")   ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If
            If Not IsPostBack Then
                ' ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideDrugDetails", "HideDrugDetails(); ", True)
                If (Choice = 1) Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideDrugDetails", "HideDrugDetails(); ", True)
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "HideDrugDetails", " ", True)
                End If
            End If
            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...Gencall")

        Finally

        End Try

    End Function
#End Region

#Region "GenCall_Data "
    Private Function GenCall_Data(ByRef ds_DWR_Retu As DataSet) As Boolean

        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Val As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing

        Try


            Val = Me.ViewState(VS_DrugCode) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vDrugCode=" + Val.ToString
            End If


            If Not objHelp.getdrugmst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
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

            ds_DWR_Retu = ds
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....Gencall_Data")

        Finally

        End Try
    End Function
#End Region

#Region "GenCall_showUI "
    Private Function GenCall_ShowUI() As Boolean
        Dim dt_OpMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try
            Page.Title = " :: Drug Master ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Drug Master"

            dt_OpMst = Me.ViewState(VS_DtDrugMst)

            Choice = Me.ViewState("Choice")

            If Not FillGrid() Then
                Exit Function
            End If

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.txtDrugName.Text = ""
                Me.TxtWashOutPeriod.Text = ""
                Me.TxtHousing.Text = ""
                Me.txtSynonyms.Text = ""
                Me.txtFood.Text = ""

                Me.txtstrength.Text = ""
                Me.txtformulation.Text = ""
                Me.txtrelease.Text = ""

                If Not dt_OpMst.Rows(0).Item("vDrugName") Is System.DBNull.Value Then
                    Me.txtDrugName.Text = dt_OpMst.Rows(0).Item("vDrugName")
                End If

                If Not dt_OpMst.Rows(0).Item("vWashOutPeriod") Is System.DBNull.Value Then
                    Me.TxtWashOutPeriod.Text = dt_OpMst.Rows(0).Item("vWashOutPeriod")
                End If

                If Not dt_OpMst.Rows(0).Item("vHousing") Is System.DBNull.Value Then
                    Me.TxtHousing.Text = dt_OpMst.Rows(0).Item("vHousing")
                End If

                If Not dt_OpMst.Rows(0).Item("vDrugSynonyms") Is System.DBNull.Value Then
                    Me.txtSynonyms.Text = dt_OpMst.Rows(0).Item("vDrugSynonyms")
                End If

                If Not dt_OpMst.Rows(0).Item("vFoodEffect") Is System.DBNull.Value Then
                    Me.txtFood.Text = dt_OpMst.Rows(0).Item("vFoodEffect")
                End If

                If dt_OpMst.Rows(0).Item("cActiveFlag") = GeneralModule.ActiveFlag_Yes Then
                    Me.chkActive.Checked = True
                Else
                    Me.chkActive.Checked = False
                End If

                If Not dt_OpMst.Rows(0).Item("vstrength") Is System.DBNull.Value Then
                    Me.txtstrength.Text = dt_OpMst.Rows(0).Item("vstrength")
                End If

                If Not dt_OpMst.Rows(0).Item("vformulation") Is System.DBNull.Value Then
                    Me.txtformulation.Text = dt_OpMst.Rows(0).Item("vformulation")
                End If

                If Not dt_OpMst.Rows(0).Item("vrelease") Is System.DBNull.Value Then
                    Me.txtrelease.Text = dt_OpMst.Rows(0).Item("vrelease")
                End If

                Me.txtRemark.Text = ""
                Me.BtnSave.Text = "Update"
                Me.BtnSave.ToolTip = "UPdate"

            End If

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....GencallShow_UI")
        Finally
        End Try
    End Function
#End Region

#Region "FillGrid"

    Private Function FillGrid(Optional ByVal Wstr As String = "") As Boolean
        Dim ds_View As New Data.DataSet
        Dim estr As String = String.Empty

        Try

            If Wstr = "" Then
                If Not objHelp.getdrugmst("cStatusIndi <> 'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_View, estr) Then
                    Return False
                End If
            Else
                If Not objHelp.getdrugmst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_View, estr) Then
                    Return False
                End If
            End If
            ds_View.Tables(0).DefaultView.Sort = "vDrugName"
            Me.GV_Drug.DataSource = ds_View.Tables(0).DefaultView.ToTable()
            'Me.GV_Drug.DataBind()

            If GV_Drug.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "gvDrugMst", "gvDrugMst(); ", True)   ''Added by ketan
            End If

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillGrid")
            Return False
        End Try
    End Function

#End Region

#Region "Button Click"
    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim ds_Save As New DataSet
        Dim dt_Save As New DataTable
        Dim estr As String = String.Empty
        Dim message As String = String.Empty
        Try

            If Not AssignValues() Then
                Exit Sub
            End If

            ds_Save = New DataSet
            dt_Save = CType(Me.ViewState(VS_DtDrugMst), DataTable)
            dt_Save.TableName = "DrugMst"
            ds_Save.Tables.Add(dt_Save)

            If Not objLambda.Save_InsertDrugMst(Me.ViewState(VS_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_DrugMst, ds_Save, Me.Session(S_UserID), estr) Then

                ObjCommon.ShowAlert("Error While Saving DrugMst", Me.Page)
                Exit Sub

            End If
            message = IIf(Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "Drug Details Saved Successfully !", "Drug Details Updated Successfully !")
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Alert", "ShowAlert('" + message + "')", True)
            ResetPage()

        Catch exThreading As System.Threading.ThreadAbortException

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....BtnSave_Click")

        End Try
    End Sub

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click

        Me.Response.Redirect("frmMainPage.aspx")

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        ResetPage()
        Me.Response.Redirect("frmDrugMst.aspx?mode=1")

    End Sub

    'Protected Sub btnViewAllRec_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnViewAllRec.Click
    '    FillGrid()
    'End Sub

    'Protected Sub btnSearchDrug_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchDrug.Click
    '    FillGrid("vDrugCode='" & Me.HDrugCode.Value.Trim() & "'")
    'End Sub

#End Region

#Region "Assign values"

    Private Function AssignValues() As Boolean
        Dim dr As DataRow
        Dim dt_User As New DataTable
        Dim ds_Check As New DataSet
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Try



            dt_User = CType(Me.ViewState(VS_DtDrugMst), DataTable)

            'For Validating Duplicate Data

            wStr = "cStatusIndi <> 'D' And  Upper(vDrugName) ='" & Me.txtDrugName.Text.Trim().ToUpper() & "'"
            If Me.Request.QueryString("mode") = 2 Then
                wStr += " And vDrugCode <> '" + Me.Request.QueryString("Value").ToString + "'"
            End If

            If Not objHelp.getdrugmst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Check, eStr) Then

                Me.ShowErrorMessage("Error While Getting Data from DrugMst", eStr)
                Return False

            End If

            If ds_Check.Tables(0).Rows.Count > 0 Then
                ObjCommon.ShowAlert("Drug Name Already Exists !", Me.Page)

                FillGrid()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "gvDrugMst", "fnGetDataforDrugMaster()", True)   ''Added by ketan
                Return False
            End If
            '****************************************************

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                dt_User.Clear()
                dr = dt_User.NewRow()
                'vDrugCode,vDrugName,vWashOutPeriod,vHousing,iModifyBy,dModifyOn,cStatusIndi
                dr("vDrugCode") = "0"
                dr("vDrugName") = Me.txtDrugName.Text.Trim()
                dr("vWashOutPeriod") = Me.TxtWashOutPeriod.Text.Trim()
                dr("vHousing") = Me.TxtHousing.Text.Trim()
                dr("vDrugSynonyms") = Me.txtSynonyms.Text.Trim()
                dr("vFoodEffect") = Me.txtFood.Text.Trim()
                dr("cActiveFlag") = IIf(Me.chkActive.Checked = True, "Y", "N")
                dr("iCreatedBy") = Me.Session(S_UserID)
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("vstrength") = Me.txtstrength.Text.Trim()
                dr("vformulation") = Me.txtformulation.Text.Trim()
                dr("vrelease") = Me.txtrelease.Text.Trim()
                dr("vRemark") = txtRemark.Text.Trim()
                dt_User.Rows.Add(dr)

            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

                For Each dr In dt_User.Rows
                    dr("vDrugName") = Me.txtDrugName.Text.Trim()
                    dr("vWashOutPeriod") = Me.TxtWashOutPeriod.Text.Trim()
                    dr("vHousing") = Me.TxtHousing.Text.Trim()
                    dr("vDrugSynonyms") = Me.txtSynonyms.Text.Trim()
                    dr("vFoodEffect") = Me.txtFood.Text.Trim()
                    dr("cActiveFlag") = IIf(Me.chkActive.Checked = True, "Y", "N")
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "E"
                    dr("vstrength") = Me.txtstrength.Text.Trim()
                    dr("vformulation") = Me.txtformulation.Text.Trim()
                    dr("vrelease") = Me.txtrelease.Text.Trim()
                    dr("vRemark") = txtRemark.Text.Trim()
                    dr.AcceptChanges()
                Next dr

                dt_User.AcceptChanges()

            End If

            Me.ViewState(VS_DtDrugMst) = dt_User
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......AssignValues")
            Return False
        End Try

    End Function

#End Region

#Region "Reset Page"

    Private Sub ResetPage()
        Me.txtDrugName.Text = ""
        Me.TxtWashOutPeriod.Text = ""
        Me.TxtHousing.Text = ""
        Me.txtSynonyms.Text = ""
        Me.txtFood.Text = ""
        Me.chkActive.Checked = False
        Me.txtRemark.Text = ""
        Me.txtstrength.Text = ""
        Me.txtformulation.Text = ""
        Me.txtrelease.Text = ""
        Me.ViewState(VS_DtDrugMst) = Nothing

    End Sub

#End Region

#Region "Grid Event"

    Protected Sub GV_Drug_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim index As Integer = e.CommandArgument

        If e.CommandName.ToUpper = "EDIT" Then
            Me.Response.Redirect("frmDrugMst.aspx?mode=2&value=" & Me.GV_Drug.Rows(index).Cells(GVC_Code).Text.Trim())
        End If

    End Sub

    Protected Sub GV_Drug_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        'If e.Row.RowType = DataControlRowType.DataRow Or _
        '              e.Row.RowType = DataControlRowType.Header Or _
        '              e.Row.RowType = DataControlRowType.Footer Then

        '    e.Row.Cells(GVC_Code).Visible = False
        '    e.Row.Cells(GVC_ActiveFlag).Visible = False
        'End If
    End Sub

    Protected Sub GV_Drug_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        If Not e.Row.RowType = DataControlRowType.Pager Then

            e.Row.Cells(GVC_Code).Visible = False
            e.Row.Cells(GVC_ActiveFlag).Visible = False

            If e.Row.RowType = DataControlRowType.DataRow Then

                e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (Me.GV_Drug.PageSize * Me.GV_Drug.PageIndex) + 1
                CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandName = "Edit"
                CType(e.Row.FindControl("lnkAudit"), ImageButton).Attributes.Add("vDrugCode", e.Row.Cells(GVC_Code).Text)

            End If
        End If
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

#Region "Web Method"

    <WebMethod> _
    Public Shared Function AuditTrail(ByVal vDrugCode As String) As String
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_DrugMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim dtDrugMstHistory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable

        Dim vTableName As String = String.Empty
        Dim vIdName As String = String.Empty
        Dim AuditFieldName As String = String.Empty
        Dim AuditFieldValue As String = String.Empty
        Dim vOtherTableName As String = String.Empty
        Dim vOtherIdName As String = String.Empty

        Try

            vTableName = "DrugMstHistory"
            vIdName = ""
            AuditFieldName = "vDrugCode"
            AuditFieldValue = vDrugCode
            vOtherTableName = ""
            vOtherIdName = ""


            Dim Param As String = ""


            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName

            If Not objHelp.Proc_GetAuditTrail(wStr, ds_DrugMst, Param) Then
                Throw New Exception(estr)
            End If

            If Not dtDrugMstHistory Is Nothing Then
                dtDrugMstHistory.Columns.Add("SrNo")
                dtDrugMstHistory.Columns.Add("DrugName")
                dtDrugMstHistory.Columns.Add("WashOutPeriod")
                dtDrugMstHistory.Columns.Add("Housing")
                dtDrugMstHistory.Columns.Add("DrugSynonyms")
                dtDrugMstHistory.Columns.Add("FoodEffect")
                dtDrugMstHistory.Columns.Add("strength")
                dtDrugMstHistory.Columns.Add("formulation")
                dtDrugMstHistory.Columns.Add("release")
                dtDrugMstHistory.Columns.Add("Remarks")
                dtDrugMstHistory.Columns.Add("ModifyBy")
                dtDrugMstHistory.Columns.Add("ModifyOn")
            End If

            dt = ds_DrugMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtDrugMstHistory.NewRow()
                drAuditTrail("SrNo") = i
                drAuditTrail("DrugName") = dr("vDrugName").ToString()
                drAuditTrail("WashOutPeriod") = dr("vWashOutPeriod").ToString()
                drAuditTrail("Housing") = dr("vHousing").ToString()
                drAuditTrail("DrugSynonyms") = dr("vDrugSynonyms").ToString()
                drAuditTrail("FoodEffect") = dr("vFoodEffect").ToString()
                drAuditTrail("strength") = dr("vstrength").ToString()
                drAuditTrail("formulation") = dr("vformulation").ToString()
                drAuditTrail("release") = dr("vrelease").ToString()
                drAuditTrail("Remarks") = dr("vRemark").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("ModifyOn"))
                dtDrugMstHistory.Rows.Add(drAuditTrail)
                dtDrugMstHistory.AcceptChanges()
                i += 1
            Next
            strReturn = JsonConvert.SerializeObject(dtDrugMstHistory)
            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Protected Sub btnExportToExcel_Click(sender As Object, e As EventArgs) Handles btnExportToExcel.Click
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_DrugMst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty

        Dim dtDrugMstHistory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable


        Dim filename As String = String.Empty
        Dim vTableName As String = String.Empty
        Dim vIdName As String = String.Empty
        Dim AuditFieldName As String = String.Empty
        Dim AuditFieldValue As String = String.Empty
        Dim vOtherTableName As String = String.Empty
        Dim vOtherIdName As String = String.Empty
        Dim strMessage As New StringBuilder

        Try

            vTableName = "DrugMstHistory"
            vIdName = ""
            AuditFieldName = "vDrugCode"
            AuditFieldValue = hdnDrugCode.Value
            vOtherTableName = ""
            vOtherIdName = ""


            Dim Param As String = ""

            wStr = vTableName + "##" + vIdName + "##" + AuditFieldName + "##" + AuditFieldValue + "##" + vOtherTableName + "##" + vOtherIdName

            If Not objHelp.Proc_GetAuditTrail(wStr, ds_DrugMst, Param) Then
                Throw New Exception(estr)
            End If

            If Not dtDrugMstHistory Is Nothing Then
                dtDrugMstHistory.Columns.Add("Sr. No")
                dtDrugMstHistory.Columns.Add("Drug Name")
                dtDrugMstHistory.Columns.Add("Wash Out Period")
                dtDrugMstHistory.Columns.Add("Housing")
                dtDrugMstHistory.Columns.Add("Drug Synonyms")
                dtDrugMstHistory.Columns.Add("Food Effect")
                dtDrugMstHistory.Columns.Add("Strength")
                dtDrugMstHistory.Columns.Add("Formulation")
                dtDrugMstHistory.Columns.Add("Release")
                dtDrugMstHistory.Columns.Add("Remarks")
                dtDrugMstHistory.Columns.Add("ModifyBy")
                dtDrugMstHistory.Columns.Add("ModifyOn")
            End If

            dtDrugMstHistory.AcceptChanges()
            dt = ds_DrugMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtDrugMstHistory.NewRow()
                drAuditTrail("Sr. No") = i
                drAuditTrail("Drug Name") = dr("vDrugName").ToString()
                drAuditTrail("Wash Out Period") = dr("vWashOutPeriod").ToString()
                drAuditTrail("Housing") = dr("vHousing").ToString()
                drAuditTrail("Drug Synonyms") = dr("vDrugSynonyms").ToString()
                drAuditTrail("Food Effect") = dr("vFoodEffect").ToString()
                drAuditTrail("Strength") = dr("vstrength").ToString()
                drAuditTrail("Formulation") = dr("vformulation").ToString()
                drAuditTrail("Release") = dr("vrelease").ToString()
                drAuditTrail("Remarks") = dr("vRemark").ToString()
                drAuditTrail("ModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("ModifyOn"))
                dtDrugMstHistory.Rows.Add(drAuditTrail)
                dtDrugMstHistory.AcceptChanges()
                i += 1
            Next

            gvExport.DataSource = dtDrugMstHistory
            gvExport.DataBind()
            If gvExport.Rows.Count > 0 Then

                gvExport.HeaderRow.BackColor = Color.White
                gvExport.FooterRow.BackColor = Color.White
                For Each cell As TableCell In gvExport.HeaderRow.Cells
                    cell.BackColor = gvExport.HeaderStyle.BackColor
                Next
                For Each row As GridViewRow In gvExport.Rows
                    row.BackColor = Color.White
                    row.Height = 20
                    For Each cell As TableCell In row.Cells
                        If row.RowIndex Mod 2 = 0 Then
                            cell.BackColor = gvExport.AlternatingRowStyle.BackColor
                            cell.ForeColor = gvExport.RowStyle.ForeColor
                        Else
                            cell.BackColor = gvExport.RowStyle.BackColor
                            cell.ForeColor = gvExport.RowStyle.ForeColor
                        End If
                        cell.CssClass = "textmode"
                    Next
                Next

                'style to format numbers to string
                Dim style As String = "<style> .textmode { } </style>"
                Response.Write(style)



                Dim info As String = String.Empty
                Dim gridviewHtml As String = String.Empty

                filename = "Audit Trail_" + hdnDrugCode.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                Dim stringWriter As New System.IO.StringWriter()

                Dim writer As New HtmlTextWriter(stringWriter)
                gvExport.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()

                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""12""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")





                strMessage.Append("<td colspan=""12""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Drug Master-AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td Align=""Right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")


                gridviewHtml = strMessage.ToString() + gridviewHtml
                Context.Response.Buffer = True
                Context.Response.ClearContent()
                Context.Response.ClearHeaders()

                Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
                Context.Response.AddHeader("content-Disposition", "attachment; filename=" + filename)
                Context.Response.AddHeader("Content-Length", gridviewHtml.Length)

                Context.Response.Write(gridviewHtml)
                Context.Response.Flush()
                Context.Response.End()

                System.IO.File.Delete(filename)
            Else

                Dim info As String = String.Empty
                Dim gridviewHtml As String = String.Empty

                filename = "Audit Trail_" + hdnDrugCode.Value + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"


                drAuditTrail = dtDrugMstHistory.NewRow()

                drAuditTrail("Sr. No") = i
                drAuditTrail("Drug Name") = ""
                drAuditTrail("Wash Out Period") = ""
                drAuditTrail("Housing") = ""
                drAuditTrail("Drug Synonyms") = ""
                drAuditTrail("Food Effect") = ""
                drAuditTrail("Strength") = ""
                drAuditTrail("Formulation") = ""
                drAuditTrail("Release") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("ModifyBy") = ""
                drAuditTrail("ModifyOn") = ""
                dtDrugMstHistory.Rows.Add(drAuditTrail)
                dtDrugMstHistory.AcceptChanges()
                gvExport.DataSource = dtDrugMstHistory
                gvExport.DataBind()


                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(stringWriter)
                gvExport.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()


                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""12""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<td colspan=""12""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("ProjectGroup Master-AuditTrail")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td align=""right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")


                gridviewHtml = strMessage.ToString() + gridviewHtml
                Context.Response.Buffer = True
                Context.Response.ClearContent()
                Context.Response.ClearHeaders()

                Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
                Context.Response.AddHeader("content-Disposition", "attachment; filename=" + filename)
                Context.Response.AddHeader("Content-Length", gridviewHtml.Length)

                Context.Response.Write(gridviewHtml)
                Context.Response.Flush()
                Context.Response.End()

                System.IO.File.Delete(filename)
                Exit Sub
            End If
        Catch ex As Exception
            Me.ObjCommon.ShowAlert("Error While Exporting Data To Excel : " + estr, Me.Page)

            Exit Sub
        End Try
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    End Sub

#End Region

    Protected Sub btnExportToExcelGrid_Click(sender As Object, e As EventArgs) Handles btnExportToExcelGrid.Click
        'Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable
        Dim wStr As String = String.Empty
        Dim ds_DrugMst As New DataSet
        Dim estr As String = String.Empty
        Dim dtDrugMstHistory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable
        Dim DrugCode As String = String.Empty
        Dim filename As String = String.Empty
        Dim strMessage As New StringBuilder

        Try
            wStr = "" + "##"
            ds_DrugMst = objHelp.ProcedureExecute("Proc_DrugMaster ", wStr)
            wStr = String.Empty
            If Not dtDrugMstHistory Is Nothing Then
                dtDrugMstHistory.Columns.Add("Sr. No")
                dtDrugMstHistory.Columns.Add("Drug Name")
                dtDrugMstHistory.Columns.Add("Wash Out Period")
                dtDrugMstHistory.Columns.Add("Housing")
                dtDrugMstHistory.Columns.Add("Drug Synonyms")
                dtDrugMstHistory.Columns.Add("Food Effect")
                dtDrugMstHistory.Columns.Add("Strength")
                dtDrugMstHistory.Columns.Add("Formulation")
                dtDrugMstHistory.Columns.Add("Release")
                dtDrugMstHistory.Columns.Add("Remarks")
                dtDrugMstHistory.Columns.Add("ModifyBy")
                dtDrugMstHistory.Columns.Add("ModifyOn")
            End If

            dtDrugMstHistory.AcceptChanges()
            dt = ds_DrugMst.Tables(0)
            Dim dv As New DataView(dt)
            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtDrugMstHistory.NewRow()
                drAuditTrail("Sr. No") = i
                drAuditTrail("Drug Name") = dr("vDrugName").ToString()
                drAuditTrail("Wash Out Period") = dr("vWashOutPeriod").ToString()
                drAuditTrail("Housing") = dr("vHousing").ToString()
                drAuditTrail("Drug Synonyms") = dr("vDrugSynonyms").ToString()
                drAuditTrail("Food Effect") = dr("vFoodEffect").ToString()
                drAuditTrail("Strength") = dr("vstrength").ToString()
                drAuditTrail("Formulation") = dr("vformulation").ToString()
                drAuditTrail("Release") = dr("vrelease").ToString()
                drAuditTrail("Remarks") = dr("vRemark").ToString()
                drAuditTrail("ModifyBy") = dr("iModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("dModifyOn"))
                dtDrugMstHistory.Rows.Add(drAuditTrail)
                dtDrugMstHistory.AcceptChanges()
                i += 1
            Next

            gvExportToExcel.DataSource = dtDrugMstHistory
            gvExportToExcel.DataBind()
            If gvExportToExcel.Rows.Count > 0 Then

                gvExportToExcel.HeaderRow.BackColor = Color.White
                gvExportToExcel.FooterRow.BackColor = Color.White
                For Each cell As TableCell In gvExportToExcel.HeaderRow.Cells
                    cell.BackColor = gvExportToExcel.HeaderStyle.BackColor
                Next
                For Each row As GridViewRow In gvExportToExcel.Rows
                    row.BackColor = Color.White
                    row.Height = 20
                    For Each cell As TableCell In row.Cells
                        If row.RowIndex Mod 2 = 0 Then
                            cell.BackColor = gvExportToExcel.AlternatingRowStyle.BackColor
                            cell.ForeColor = gvExportToExcel.RowStyle.ForeColor
                        Else
                            cell.BackColor = gvExportToExcel.RowStyle.BackColor
                            cell.ForeColor = gvExportToExcel.RowStyle.ForeColor
                        End If
                        cell.CssClass = "textmode"
                    Next
                Next

                'style to format numbers to string
                Dim style As String = "<style> .textmode { } </style>"
                Response.Write(style)

                Dim info As String = String.Empty
                Dim gridviewHtml As String = String.Empty

                filename = "Drug Master" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                Dim stringWriter As New System.IO.StringWriter()

                Dim writer As New HtmlTextWriter(stringWriter)
                gvExportToExcel.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()

                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""12""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<td colspan=""12""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Drug Master")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td Align=""Right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")


                gridviewHtml = strMessage.ToString() + gridviewHtml
                Context.Response.Buffer = True
                Context.Response.ClearContent()
                Context.Response.ClearHeaders()

                Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
                Context.Response.AddHeader("content-Disposition", "attachment; filename=" + filename)
                Context.Response.AddHeader("Content-Length", gridviewHtml.Length)

                Context.Response.Write(gridviewHtml)
                Context.Response.Flush()
                Context.Response.End()

                System.IO.File.Delete(filename)
            Else

                Dim info As String = String.Empty
                Dim gridviewHtml As String = String.Empty

                filename = "Drug Master" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"

                drAuditTrail = dtDrugMstHistory.NewRow()
                drAuditTrail("Sr. No") = i
                drAuditTrail("Drug Name") = ""
                drAuditTrail("Wash Out Period") = ""
                drAuditTrail("Housing") = ""
                drAuditTrail("Drug Synonyms") = ""
                drAuditTrail("Food Effect") = ""
                drAuditTrail("Strength") = ""
                drAuditTrail("Formulation") = ""
                drAuditTrail("Release") = ""
                drAuditTrail("Remarks") = ""
                drAuditTrail("ModifyBy") = ""
                drAuditTrail("ModifyOn") = ""
                dtDrugMstHistory.Rows.Add(drAuditTrail)
                dtDrugMstHistory.AcceptChanges()
                gvExportToExcel.DataSource = dtDrugMstHistory
                gvExportToExcel.DataBind()

                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(stringWriter)
                gvExportToExcel.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()

                strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

                strMessage.Append("<tr>")
                strMessage.Append("<td colspan=""12""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")

                strMessage.Append("<td colspan=""12""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append("Drug Master")
                strMessage.Append("</font></strong><center></b></td>")
                strMessage.Append("</tr>")
                strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td align=""right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")

                gridviewHtml = strMessage.ToString() + gridviewHtml
                Context.Response.Buffer = True
                Context.Response.ClearContent()
                Context.Response.ClearHeaders()

                Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
                Context.Response.AddHeader("content-Disposition", "attachment; filename=" + filename)
                Context.Response.AddHeader("Content-Length", gridviewHtml.Length)

                Context.Response.Write(gridviewHtml)
                Context.Response.Flush()
                Context.Response.End()

                System.IO.File.Delete(filename)
                Exit Sub
            End If
        Catch ex As Exception
            Me.ObjCommon.ShowAlert("Error While Exporting Data To Excel : " + estr, Me.Page)

            Exit Sub
        End Try
    End Sub
    <WebMethod> _
    Public Shared Function View_Drug() As String
        Dim objCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim wStr As String = ""
        Dim ds_Drug As New DataSet
        Dim eStr_Retu As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim dt As New DataTable
        Dim ds_Drugsfinal As New DataSet

        Try
            If wStr = "" Then
                If Not objHelp.getdrugmst("cStatusIndi <> 'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Drug, eStr_Retu) Then
                    Return False
                End If
            Else
                If Not objHelp.getdrugmst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Drug, eStr_Retu) Then
                    Return False
                End If
            End If

            dt = ds_Drug.Tables(0)
            Dim dv_Drugs As New DataView(dt)
            dv_Drugs.Sort = "vDrugName ASC"

            ds_Drugsfinal.Tables.Add(dv_Drugs.ToTable())

            strReturn = JsonConvert.SerializeObject(ds_Drugsfinal)
            Return strReturn




        Catch ex As Exception

            Return strReturn
        End Try


    End Function

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click

        Me.Response.Redirect("frmDrugMst.aspx?mode=2&value=" & Me.hdnEditedId.Value)

    End Sub

End Class
