Imports System.Web.Services
Imports Newtonsoft.Json

Partial Class frmGenerateDosingLabels
    Inherits System.Web.UI.Page
#Region "VARIABLE DECLARATION "

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DosingDetail As String = "DosingDetail"
    Private Const VS_Type As String = "Type"

    Private Const GVC_Check As Integer = 0
    Private Const GVCell_DosingDetailNo As Integer = 1
    Private Const GVCell_BarCode As Integer = 2
    Private Const GVCell_ProjectNo As Integer = 3
    Private Const GVCell_ProtocolNo As Integer = 4
    Private Const GVCell_Drug As Integer = 5
    Private Const GVCell_Period As Integer = 6
    Private Const GVCell_SubNo As Integer = 7
    Private Const GVC_SubId As Integer = 8
    Private Const GVC_SubName As Integer = 9
    Private Const GVCell_Type As Integer = 10
    Private Const GVCell_randomizationcode As Integer = 11
    Private Const GVCell_Day As Integer = 12
    Private Const GVCell_Dose As Integer = 13
    Private Const GVCell_DosageForm As Integer = 14
    Private Const GVCell_Storage As Integer = 15
    Private Const GVCell_Streangth As Integer = 16
    Private Const GVCell_ExpDate As Integer = 17
    Private Const GVCell_Instruction As Integer = 18
    Private Const GVCell_Assign As Integer = 19
    Private Const GVCell_DosedOn As Integer = 20
    Private Const GVCell_Replaced As Integer = 21
    Private Const GVCell_PIName As Integer = 22
    Private Const GVCell_RouteAdmin As Integer = 23
    Private Const GVCell_ReplaceFlag As Integer = 24


    Private Const GVAudit_DosingDetailNo As Integer = 0

    Private Const GVAudit_BarCode As Integer = 1
    Private Const GVAudit_ProjectNo As Integer = 2
    Private Const GVAudit_ProtocolNo As Integer = 3
    Private Const GVAudit_Drug As Integer = 4
    Private Const GVAudit_Period As Integer = 5
    Private Const GVAudit_SubNo As Integer = 6
    Private Const GVAudit_SubId As Integer = 7
    Private Const GVAudit_SubName As Integer = 8
    Private Const GVAudit_Type As Integer = 9
    Private Const GVAudit_randomizationcode As Integer = 10
    Private Const GVAudit_Day As Integer = 12
    Private Const GVAudit_Dose As Integer = 12
    Private Const GVAudit_DosageForm As Integer = 13
    Private Const GVAudit_Storage As Integer = 14
    Private Const GVAudit_Streangth As Integer = 15
    Private Const GVAudit_ExpDate As Integer = 16
    Private Const GVGVAudit_Modifyby As Integer = 17
    Private Const GVAudit_ModifyOn As Integer = 18

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Dim eStr_Retu As String
#End Region

#Region "Load Event"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Me.Session("S_Temp") = "N"
            Gencall_showUI()
        End If

    End Sub
#End Region

#Region "Gencall_ShowUI"

    Private Function Gencall_showUI() As Boolean

        Dim Sender As New Object
        Dim e As EventArgs
        Try
            Page.Title = " :: Generate Dosing Labels ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Generate Dosing Labels"
            Me.BtnDelete1.Visible = False
            Me.BtnDelete2.Visible = False


            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtproject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(Sender, e)
            End If
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)
            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function

#End Region

#Region "Button Event"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim ds_CrfVersionDetail As New DataSet
        Dim ds_CrfVersionDetail1 As New DataSet
        Dim VersionNo As Integer = 0
        Dim VersionDate As Date
        Dim wstr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim dt_PiUser As New DataTable
        Dim ds_PiUser As New DataSet
        Dim Userid As Integer = 0
        Dim wStr2 As String = String.Empty
        Dim ds_WorkSpaceMst As New DataSet

        '============================CRFversionControl======
        ResetPage1()
        Me.btnSave.Attributes.Add("style", " ")
        Me.BtnDelete1.Attributes.Add("style", " ")
        Me.BtnDelete2.Attributes.Add("style", " ")
        Me.divnote.Style.Add("Display", "none;")
        If CheckProjectStatus() Then
            Me.ObjCommon.ShowAlert("Project is Locked. You Can not generate IP Labels. ", Me.Page())
            Me.btnSave.Attributes.Add("style", "display:none;")
            Me.BtnDelete1.Attributes.Add("style", "display:none;")
            Me.BtnDelete2.Attributes.Add("style", "display:none;")
        End If
        Me.VersionDtl.Attributes.Add("style", " ")
        Me.VersionDtl.Attributes.Add("style", "display:none;")
        wstr = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "'"
        If Not objHelp.GetData("View_CRFVersionForDataEntryControl", "*", wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CrfVersionDetail1, eStr_Retu) Then
            Throw New Exception(eStr_Retu)
        End If
        If ds_CrfVersionDetail1.Tables(0).Rows.Count > 0 Then
            If IsDBNull(ds_CrfVersionDetail1.Tables(0).Rows(0)("vParentWorkspaceId")) = False Then
                HParentProject.Value = ds_CrfVersionDetail1.Tables(0).Rows(0)("vParentWorkspaceId")
                If IsDBNull(ds_CrfVersionDetail1.Tables(0).Rows(0)("cIsTestSite")) = False Then
                    HIsTestSite.Value = ds_CrfVersionDetail1.Tables(0).Rows(0)("cIsTestSite")
                Else
                    HIsTestSite.Value = ""
                End If
            Else
                HParentProject.Value = ""
            End If
            Dim dt As DataTable = ds_CrfVersionDetail1.Tables.Item(0)
            Dim dv As New DataView(dt)
            dv.RowFilter = "nVersionNo is not null"
            If dv.ToTable.Rows.Count > 0 Then
                ds_CrfVersionDetail.Tables.Add(dv.ToTable("0"))
                ds_CrfVersionDetail.AcceptChanges()
                '============SSSSS====================================================
                'If row.Length Then
                If ds_CrfVersionDetail.Tables(0).Rows.Count > 0 Then
                    VersionNo = ds_CrfVersionDetail.Tables(0).Rows(0)("nVersionNo")
                    VersionDate = ds_CrfVersionDetail.Tables(0).Rows(0)("dVersiondate").ToString
                    Me.VersionNo.Text = VersionNo.ToString
                    Me.VersionDate.Text = VersionDate.ToString("dd-MMM-yyyy")
                    Me.VersionDtl.Attributes.Add("style", " ")
                    If ds_CrfVersionDetail.Tables(0).Rows(0)("cFreezeStatus").ToString.Trim.ToUpper = "U" Then
                        ImageLockUnlock.Attributes.Add("src", "images/UnFreeze.jpg")
                        ObjCommon.ShowAlert("Project Is In Unfreeze State, First Freeze It Then Proceed", Me.Page)
                        Exit Sub
                    ElseIf ds_CrfVersionDetail.Tables(0).Rows(0)("cFreezeStatus").ToString.Trim.ToUpper = "F" Then
                        ImageLockUnlock.Attributes.Add("src", "images/Freeze.jpg")
                    End If
                Else
                    Me.VersionDtl.Attributes.Add("style", "display:none;")
                End If
            End If
        End If

        trSubjectRange.Style.Add("display", "''")
        wStr2 = "Select ISNULL(workspacemst.vLocationCode,'') AS vLocationCode,ISNULL(workspacemst.vPIName,0) AS vPIName From WorkSpaceMst WITH(NOLOCK) Where vWorkSpaceId = " & Me.HProjectId.Value.Trim() & " And cStatusIndi <> 'D'"

        ds_WorkSpaceMst = objHelp.GetResultSet(wStr2, "RandomizationDetail")

        If Not ds_WorkSpaceMst Is Nothing AndAlso ds_WorkSpaceMst.Tables(0).Rows.Count > 0 Then
            Me.ddlPIName.Items.Clear()
            If Not objHelp.getuserMst("vUserTypeCode=0024 AND cStatusIndi <> 'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_PiUser, eStr_Retu) Then
                Me.ShowErrorMessage("", "Error While getUserMst")
            End If

            If Not ds_PiUser Is Nothing AndAlso ds_PiUser.Tables(0).Rows.Count > 0 Then
                dt_PiUser = ds_PiUser.Tables(0)
                dt_PiUser.Columns.Add("FullName", GetType(String), "vFirstName + ' ' + vLastName")
                Me.ddlPIName.DataSource = dt_PiUser
                Me.ddlPIName.DataValueField = "iUserid"
                Me.ddlPIName.DataTextField = "FullName"
                Me.ddlPIName.DataBind()
                Me.ddlPIName.Items.Insert(0, New ListItem("Select Principal Investigator", ""))

                Userid = CType(ds_WorkSpaceMst.Tables(0).Rows(0).Item("vPIName").ToString.Trim(), Integer)
                ddlPIName.SelectedIndex = ddlPIName.Items.IndexOf(ddlPIName.Items.FindByValue(Userid))
            End If
        End If
        Me.btnSave.Visible = True
        FillPeriodDropDown()
        setcheckboxvalue()
        FillDrugDetails()
        FillProductTypeDetails()

        If HIsTestSite.Value = "N" Or HIsTestSite.Value = "" Then
            CheckLastSubjectForDosingDetail()
        End If

        Me.gvwDosingDetail.DataSource = Nothing
        Me.gvwDosingDetail.DataBind()
        Me.BtnDelete1.Visible = False
        Me.BtnDelete2.Visible = False
        Me.HstarteiMySubjectNo.Value = ""
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ResetPage()
        Me.txtproject.Text = ""
        Me.HProjectId.Value = ""
        Me.ddlPeriod.DataSource = Nothing
        Me.ddlPeriod.DataBind()
        Me.ddlPeriod.Items.Clear()
        Me.ddlProductType.DataSource = Nothing
        Me.ddlProductType.DataBind()
        Me.ddlProductType.Items.Clear()
        Me.gvwDosingDetail.DataSource = Nothing
        Me.gvwDosingDetail.DataBind()
        Me.BtnDelete1.Visible = False
        Me.BtnDelete2.Visible = False
        Me.HIsTestSite.Value = ""
        Me.txtDrugNameForPrint.Text = ""
        Me.ddlPIName.DataSource = Nothing
        Me.ddlPIName.DataBind()
        Me.ddlPIName.Items.Clear()
        Me.ddlDrug.DataSource = Nothing
        Me.ddlDrug.DataBind()
        Me.ddlDrug.Items.Clear()
        Me.ddlBatchno.DataSource = Nothing
        Me.ddlBatchno.DataBind()
        Me.ddlBatchno.Items.Clear()
        Me.divnote.Style.Add("Display", "none;")
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim ds_Save As New DataSet
        Dim BarCodes_Retu As String = String.Empty
        Dim BarCodes As String = String.Empty
        Dim Drug As String = String.Empty
        Dim Project As String = String.Empty
        Dim Subject As String = String.Empty
        Dim Dose As String = String.Empty
        Dim Test As String = String.Empty

        If Not AssignValues(ds_Save) Then
            Exit Sub
        End If

        If Not Me.objLambda.Save_DosingDetail(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                                        ds_Save, Me.Session(S_UserID), BarCodes_Retu, eStr_Retu) Then

            Me.ObjCommon.ShowAlert("Error While Saving Data In Dosing Detail", Me.Page())
            Exit Sub

        End If

        If Not FillGrid() Then
            Me.ObjCommon.ShowAlert("Error While Filling Grid", Me.Page())
            Exit Sub
        End If
        Me.ObjCommon.ShowAlert("Record Saved Successfully", Me.Page())


        Me.BtnDelete1.Visible = True
        Me.BtnDelete2.Visible = True

        If Me.gvwDosingDetail.Rows.Count <= 0 Then
            Me.BtnDelete1.Visible = False
            Me.BtnDelete2.Visible = False

        End If

        ResetPage1()

    End Sub

    Protected Sub BtnPrint1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnPrint1.Click

        If Not Print() Then
            Me.ObjCommon.ShowAlert("Error While Printing", Me.Page())
            Exit Sub
        End If

    End Sub

    Protected Sub BtnPrint2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnPrint2.Click

        If Not Print() Then
            Me.ObjCommon.ShowAlert("Error While Printing", Me.Page())
            Exit Sub
        End If

    End Sub

    Protected Sub btnReplaceOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReplaceOK.Click

        Dim ds_DosingDetail As New DataSet
        Dim BarCodes_Retu As String = String.Empty

        Try
            If Not Me.objHelp.GetDosingDetail("vDosingBarCode='" & Me.lblLabelId.Text.Trim() & "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                    ds_DosingDetail, eStr_Retu) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data From Dosing Detail", Me.Page())
                Exit Sub
            End If




            For Each dr_Dose As DataRow In ds_DosingDetail.Tables(0).Rows

                dr_Dose("iMySubjectNo") = Me.txtreplaceCode.Text.Trim()
                dr_Dose("iModifyBy") = Me.Session(S_UserID)
                dr_Dose("cStatusindi") = "E"
                dr_Dose("dModifyOn") = CDate(ObjCommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName)).DateTime).GetDateTimeFormats()(23)          '' Added by Rahul Rupareliya

            Next dr_Dose
            ds_DosingDetail.Tables(0).AcceptChanges()

            If Not Me.objLambda.Save_DosingDetail(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                                           ds_DosingDetail, Me.Session(S_UserID), BarCodes_Retu, eStr_Retu) Then

                Me.ObjCommon.ShowAlert("Error While Saving Data In DosingDetail", Me.Page())
                Exit Sub

            End If

            If Not FillGrid() Then
                Me.ObjCommon.ShowAlert("Error While Filling Grid", Me.Page())
                Exit Sub
            End If

            Me.divReplacement.Visible = False
            Me.pnlReplace.Visible = False

            Me.txtreplaceCode.Text = ""
            Me.lblLabelId.Text = ""

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....btnReplaceOK_Click")

        End Try

    End Sub

    Protected Sub btnReplaceCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReplaceCancel.Click
        Me.divReplacement.Visible = False
        Me.pnlReplace.Visible = False

        Me.txtreplaceCode.Text = ""
        Me.lblLabelId.Text = ""
    End Sub

    Protected Sub btnCloseNew_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub BtnDelete1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnDelete1.Click

        Me.TxtRemarks.Text = ""
        Me.MpeDialogRemarks.Show()

    End Sub

    Protected Sub BtnSaveRemarks_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSaveRemarks.Click

        Dim wstr As String = String.Empty
        Dim BarCodes_Retu As String = String.Empty
        Dim ds_DelDosingDtl As New DataSet
        Dim LabelCollected As String = String.Empty
        Dim ds_SaveDosingdetail As New DataSet
        Dim dv_Dosingdetail As DataView
        Dim strSubjectNo As String = String.Empty
        Dim str As String = String.Empty
        Dim ds_DosedIpLabel As New DataSet
        Dim DayNo As String = String.Empty

        wstr = "iPeriod=" & Me.ddlPeriod.SelectedValue.Trim() & "AND vWorkSpaceId='" & Me.HProjectId.Value & "' and cstatusindi <>'D' "

        If Not Me.objHelp.GetDosingDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                ds_DelDosingDtl, eStr_Retu) Then
            Me.ObjCommon.ShowAlert("Error While Getting Data From Dosing Detail", Me.Page())
            Exit Sub
        End If


        For index As Integer = 0 To Me.gvwDosingDetail.Rows.Count - 1

            If CType(Me.gvwDosingDetail.Rows(index).FindControl("chkSelectBarCode"), CheckBox).Checked = True Then

                For Each dr As DataRow In ds_DelDosingDtl.Tables(0).Rows
                    If dr("nDosingDetailNo").ToString.ToUpper.Trim = Me.gvwDosingDetail.Rows(index).Cells(GVCell_DosingDetailNo).Text.ToUpper.Trim() Then
                        dr("cStatusindi") = "D"
                        dr("iModifyBy") = Me.Session(S_UserID)
                        dr("vRemarks") = Me.TxtRemarks.Text.Trim()
                        dr("dModifyOn") = DateTime.Now()          '' Added by Rahul Rupareliya
                        strSubjectNo += IIf(strSubjectNo = "", "", ",") + dr("iMySubjectNo").ToString.Trim()
                        DayNo += IIf(DayNo = "", "", ",") + Me.gvwDosingDetail.Rows(index).Cells(GVCell_Day).Text.ToUpper.Trim()
                    End If
                    dr.AcceptChanges()
                    ds_DelDosingDtl.Tables(0).AcceptChanges()
                Next

            End If
        Next index

        dv_Dosingdetail = ds_DelDosingDtl.Tables(0).DefaultView
        dv_Dosingdetail.RowFilter = "cStatusindi ='D'"

        ds_SaveDosingdetail.Tables.Add(dv_Dosingdetail.ToTable())

        strSubjectNo = "," + strSubjectNo + ","
        DayNo = "," + DayNo + ","

        str = Me.HProjectId.Value.ToString.Trim + "##" + CType(Me.ddlPeriod.SelectedValue, Integer).ToString.Trim() + "##" + Act_IPAdmin.ToString.Trim() + "##" + strSubjectNo.ToString.Trim() + "##" + DayNo.ToString.Trim()

        ds_DosedIpLabel = objHelp.ProcedureExecute("dbo.Proc_GetDosedDataForGeneratedIPLabel", str)

        If Not IsNothing(ds_DosedIpLabel) AndAlso CType(ds_DosedIpLabel.Tables(0).Rows(0).Item("Cnt").ToString.Trim(), Integer) > 0 Then
            ObjCommon.ShowAlert("Dosing Already Done For Selected Subjects. You Can't Delete Them!", Me.Page)
            Exit Sub
        End If

        If ds_SaveDosingdetail.Tables.Count > 0 AndAlso ds_SaveDosingdetail.Tables(0).Rows.Count > 0 Then

            If Not Me.objLambda.Save_DosingDetail(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete, _
                                            ds_SaveDosingdetail, Me.Session(S_UserID), BarCodes_Retu, eStr_Retu) Then

                Me.ObjCommon.ShowAlert("Error While Saving Data In Dosing Detail", Me.Page())
                Exit Sub

            End If


            If Not FillGrid() Then
                Me.ObjCommon.ShowAlert("Error While Filling Grid", Me.Page())
                Exit Sub
            End If
        End If

        ObjCommon.ShowAlert("IP Label Deleted Successfully", Me.Page)
        ResetPage1()

    End Sub

    Protected Sub BtnAuditTrail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAuditTrail.Click

        Dim dsViewDosingDetail As New DataSet
        Dim dvViewDosingDetail As New DataView
        Dim WorkspaceId As String = String.Empty
        Dim wstr As String = String.Empty
        Dim dc_Audit As DataColumn
        Dim estr As String = String.Empty
        Dim ds As New DataSet

        Try
            WorkspaceId = Me.HProjectId.Value.Trim()

            Me.GVAudit.DataSource = Nothing
            Me.GVAudit.DataBind()

            wstr = "iPeriod=" & Me.ddlPeriod.SelectedValue.Trim() & "AND vWorkSpaceId='" & WorkspaceId & "' and cstatusindi = 'D' order by dmodifyon desc"

            If Not objHelp.View_DosingDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                         dsViewDosingDetail, eStr_Retu) Then

                ShowErrorMessage(eStr_Retu, "")
                Exit Sub
            End If

            If dsViewDosingDetail.Tables(0).Rows.Count > 0 Then
                dc_Audit = New DataColumn("dExpiryDate_IST", System.Type.GetType("System.String"))
                dc_Audit = New DataColumn("ActualTIME", System.Type.GetType("System.String"))
                dsViewDosingDetail.Tables(0).Columns.Add("dExpiryDate_IST")
                dsViewDosingDetail.Tables(0).Columns.Add("ActualTIME")
                dsViewDosingDetail.AcceptChanges()

                For Each dr_Audit In dsViewDosingDetail.Tables(0).Rows
                    dr_Audit("dExpiryDate_IST") = Convert.ToString(CDate(dr_Audit("dExpiryDate")).ToString("dd-MMM-yyyy"))

                    If Not objHelp.Proc_ActualAuditTrailTime(CDate(dr_Audit("dModifyOn")).ToString("dd-MMM-yyyy HH:mm") + "##" + Session(S_LocationCode), ds, estr) Then
                        Throw New Exception(estr)
                    End If
                    dr_Audit("ActualTIME") = CDate(ds.Tables(0).Rows(0).Item("ActualTIME")).ToString("dd-MMM-yyyy HH:mm") + " (" + ds.Tables(0).Rows(0).Item("TimeZoneOffSet").ToString + " GMT)"

                Next
                dsViewDosingDetail.AcceptChanges()

                If Convert.ToString(Me.Session("S_Temp")).Trim() = "N" Then
                    Dim bf As BoundField = New BoundField
                    Dim dc As DataColumn
                    dc = New DataColumn(ds.Tables(0).Columns("ActualTIME").ColumnName)
                    bf.DataField = dc.ColumnName
                    bf.HeaderText = String.Empty
                    bf.HeaderText = Session(S_TimeZoneName)
                    GVAudit.Columns.Add(bf)
                    Me.Session("S_Temp") = "Y"
                End If

                Me.GVAudit.DataSource = dsViewDosingDetail
                Me.GVAudit.DataBind()
                Me.MpeAudit.Show()
            Else
                ObjCommon.ShowAlert("No Audit Trail Available", Me)

            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....BtnAuditTrail_Click")

        End Try
    End Sub

    Protected Sub BtnDelete2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnDelete2.Click
        Me.TxtRemarks.Text = ""
        Me.MpeDialogRemarks.Show()
    End Sub

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Dim dtMultodose As DataTable = New DataTable()
        Dim dr As DataRow = Nothing
        Dim i As Integer

        dtMultodose.Columns.Add(New DataColumn("Days"))
        dtMultodose.Columns.Add(New DataColumn("Doses"))

        For i = 0 To Me.grdMultidose.Rows.Count - 1
            dr = dtMultodose.NewRow()
            dr("Days") = Me.grdMultidose.Rows(i).Cells(0).Text.Trim
            dr("Doses") = CType(Me.grdMultidose.Rows(i).FindControl("txtMultidose"), TextBox).Text
            dtMultodose.Rows.Add(dr)
        Next
        ViewState("dt_Multodose") = dtMultodose.Copy()
    End Sub

    Protected Sub btnCancelDeletedLabels_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelDeletedLabels.Click
        Me.txtdays.Text = ""
        Me.txtdoses.Text = ""
        Me.chkMultidose.Enabled = True
        Me.chkMultidose.Checked = False
        Me.chkDeletebarcode.Enabled = True
        Me.chkDeletebarcode.Checked = False
        Me.txtBlanks.Enabled = True
    End Sub

    Protected Sub btnCancelMultidose_Click(sender As Object, e As EventArgs) Handles btnCancelMultidose.Click
        Me.txtTotaldays.Text = ""
        Me.txtTotaldoses.Text = ""
        Me.grdMultidose.DataSource = Nothing
        Me.grdMultidose.DataBind()
        Me.chkMultidose.Enabled = True
        Me.chkMultidose.Checked = False
        Me.chkDeletebarcode.Enabled = True
        Me.chkDeletebarcode.Checked = False
        Me.txtBlanks.Enabled = True
    End Sub

#End Region

#Region "AssignValues"  'To Save data in to dosing detail

    Private Function AssignValues(ByRef ds_Save As DataSet) As Boolean
        Dim ds_Randomization As New DataSet
        Dim ds_DosingDetail As New DataSet
        Dim dt_ProductType As New DataTable
        Dim dt_Multidose As New DataTable
        Dim ds_GetDeletedLabesCounts As New DataSet
        Dim Wstr As String = String.Empty
        Dim Wstr1 As String = String.Empty
        Dim StrQry As String = String.Empty
        Dim dr As DataRow
        Dim dr_Dose As DataRow
        Dim indexRow As Integer = 0
        Dim DosePerDay As Integer = 0
        Dim indexNo As Integer = 0
        Dim TotalDoseNo As Integer = 0
        Dim Days As String = String.Empty
        Dim Dose As String = String.Empty
        Dim Wstr2 As String = String.Empty
        Dim StrProjectsForDeletion As String = String.Empty
        Dim ds_WorkSpaceId As New DataSet
        Dim strWorkSpaceId As String = String.Empty

        Try
            If HParentProject.Value <> "" And (HIsTestSite.Value = "" Or HIsTestSite.Value = "N") Then
                Wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "' And iPeriod=" & Me.ddlPeriod.SelectedValue.Trim() + _
                "And (iMySubjectNo between " + txtSubjectInitial.Text + " and " + txtSubjectLast.Text + ")"

                Wstr1 = "vWorkspaceId='" & Me.HParentProject.Value.Trim() & "' And iPeriod=" & Me.ddlPeriod.SelectedValue.Trim() + _
                "And (iMySubjectNo between " + txtSubjectInitial.Text + " and " + txtSubjectLast.Text + ")"

                If Me.chkGenerateLabel.Checked = True Then
                    Wstr = Wstr.Substring(0, Wstr.LastIndexOf(")"))
                    Wstr += "or ImySubjectNo=0 )"

                    Wstr1 = Wstr1.Substring(0, Wstr1.LastIndexOf(")"))
                    Wstr1 += "or ImySubjectNo=0 )"
                End If
            Else
                Wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "' And iPeriod=" & Me.ddlPeriod.SelectedValue.Trim() + _
                "And (iMySubjectNo between " + txtSubjectInitial.Text + " and " + txtSubjectLast.Text + ")"

                Wstr1 = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "' And iPeriod=" & Me.ddlPeriod.SelectedValue.Trim() + _
               "And (iMySubjectNo between " + txtSubjectInitial.Text + " and " + txtSubjectLast.Text + ")"


            End If

            Wstr += " and vProductType = '" + Me.ddlProductType.SelectedItem.Text.ToString.Trim & "'"
            Wstr1 += " and vProductType = '" + Me.ddlProductType.SelectedItem.Text.ToString.Trim & "'"

            Wstr += " And cStatusIndi <> 'D'"
            Wstr1 += " And cStatusIndi <> 'D'"

            StrProjectsForDeletion = HProjectId.Value.Trim()

            ds_WorkSpaceId = Me.objHelp.ProcedureExecute("Proc_GetLabelForDelete", StrProjectsForDeletion)

            For i As Integer = 0 To ds_WorkSpaceId.Tables(0).Rows.Count - 1
                strWorkSpaceId += "'" & ds_WorkSpaceId.Tables(0).Rows(i)("vWorkSpaceId").ToString() & "'"
                strWorkSpaceId += If((i < ds_WorkSpaceId.Tables(0).Rows.Count - 1), ",", String.Empty)
            Next

            Wstr2 = "vWorkspaceId IN (" & strWorkSpaceId.ToString.Trim() & ") And iPeriod=" & Me.ddlPeriod.SelectedValue.Trim() + _
                "And (iMySubjectNo between " + txtSubjectInitial.Text + " and " + txtSubjectLast.Text + ") and vProductType = '" + Me.ddlProductType.SelectedItem.Text.ToString.Trim & "' And cStatusIndi <> 'D'"

            If Me.chkDeletebarcode.Checked = True Then
                StrQry = Wstr2 + " And iDayNo = '" + Me.txtdays.Text.ToString.Trim + "'"
            ElseIf Me.chkMultidose.Checked = True Then
                StrQry = Wstr2
            Else
                StrQry = Wstr2 + " And iDayNo = 1"
            End If
            If Not Me.objHelp.GetDosingDetail(StrQry, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                    ds_GetDeletedLabesCounts, eStr_Retu) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data From DosingDetail", Me.Page())
                Exit Function
            End If
            If Not IsNothing(ds_GetDeletedLabesCounts.Tables(0)) AndAlso ds_GetDeletedLabesCounts.Tables(0).Rows.Count > 0 Then
                If Me.chkDeletebarcode.Checked = True Then
                    ObjCommon.ShowAlert("Barcode labels already exist for day  " + Me.txtdays.Text.ToString.Trim + ". First Delete and generate again!", Me)
                Else
                    ObjCommon.ShowAlert("Barcode labels already exist. First Delete labels and generate again!", Me)
                End If
                Exit Function
            End If

            If Not Me.objHelp.GetRandomizationDetail(Wstr1, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                    ds_Randomization, eStr_Retu) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data From Randomization Detail", Me.Page())
                Exit Function
            End If

            If ds_Randomization.Tables(0).Rows.Count <= 0 Then
                Me.ObjCommon.ShowAlert("Randomization Details for the Entered Subject Range is Not Found.Please Upload CSV File for the same.", Me)
                Return False
            End If

            If Not Me.objHelp.GetDosingDetail("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                                                    ds_DosingDetail, eStr_Retu) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data From DosingDetail", Me.Page())
                Exit Function
            End If

            dt_Multidose = CType(ViewState("dt_Multodose"), DataTable)

            If Me.chkMultidose.Checked = True Then
                For i As Integer = 0 To dt_Multidose.Rows.Count - 2
                    Days = CType(dt_Multidose.Rows(i)("Days").ToString.Trim, Integer)
                    DosePerDay = CType(dt_Multidose.Rows(i)("Doses").ToString.Trim, Integer)

                    For Each dr In ds_Randomization.Tables(0).Rows
                        indexNo = 0

                        indexRow += 1
                        For indexDose As Integer = 1 To DosePerDay
                            indexNo = indexNo + 1

                            dr_Dose = ds_DosingDetail.Tables(0).NewRow
                            dr_Dose("nDosingDetailNo") = Val(indexRow.ToString.Trim() & Days.ToString.Trim() & indexDose.ToString.Trim())
                            dr_Dose("vDosingBarCode") = indexRow.ToString.Trim() & Days.ToString.Trim() & indexDose.ToString.Trim()
                            If HIsTestSite.Value = "" Or Me.HIsTestSite.Value = "N" Then
                                If HParentProject.Value <> "" Then
                                    dr_Dose("vParentWorkspaceId") = HParentProject.Value
                                Else
                                    dr_Dose("vParentWorkspaceId") = HProjectId.Value
                                End If
                            End If
                            dr_Dose("vWorkspaceId") = HProjectId.Value
                            dr_Dose("vGenericName") = Convert.ToString(Me.txtDrugNameForPrint.Text)
                            dr_Dose("nProductNo") = Me.ddlDrug.SelectedValue.ToString()  ''Added by ketan on 11-july-2016 for PMS
                            dr_Dose("nProductTypeID") = Me.ddlProductType.SelectedValue.Trim 'added by ketan
                            dr_Dose("vProductType") = Me.ddlProductType.SelectedItem.Text.ToString.Trim ' Added By Mrunal Parekh on 28-Dec-2011 for change in double blinded study
                            dr_Dose("iPeriod") = dr("iPeriod")
                            dr_Dose("iMySubjectNo") = dr("iMySubjectNo")
                            dr_Dose("vdosageform") = Me.txtDosage.Text.Trim()
                            dr_Dose("vStrength") = Me.txtstrength.Text.Trim()
                            dr_Dose("vProtocolNo") = Me.txtprotocol.Text.Trim()
                            dr_Dose("dExpiryDate") = CType(Me.txtExpiryDate.Text.Trim(), Date).ToString("dd-MMM-yyyy")
                            dr_Dose("vInstruction") = Me.txtInstruction.Text.Trim()
                            dr_Dose("vStoragecondition") = Me.txtcondition.Text.Trim()
                            dr_Dose("iDayNo") = Days
                            dr_Dose("iDoseNo") = indexNo
                            dr_Dose("iModifyBy") = Me.Session(S_UserID)
                            dr_Dose("cStatusindi") = "N"
                            dr_Dose("vRandomizationcode") = dr("vRandomizationcode")
                            dr_Dose("vPIName") = Me.ddlPIName.SelectedItem.Text.ToString.Trim()
                            dr_Dose("vRouteAdmin") = Me.txtRouteAdmin.Text.ToString.Trim()
                            dr_Dose("vBatchNo") = Me.ddlBatchno.SelectedItem.Text.ToString.Trim()
                            dr_Dose("nBatchNo") = Me.ddlBatchno.SelectedValue.Trim()
                            dr_Dose("iDispenseQtyPerSubject") = Me.txtDispenseQtyPerSubject.Text.ToString.Trim()

                            dr_Dose("dModifyOn") = CDate(ObjCommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName)).DateTime).GetDateTimeFormats()(23)
                            ds_DosingDetail.Tables(0).Rows.Add(dr_Dose)

                        Next indexDose

                        ds_DosingDetail.Tables(0).AcceptChanges()
                    Next dr
                Next i
            ElseIf Me.chkDeletebarcode.Checked = True Then
                Days = CType(Me.txtdays.Text.ToString.Trim, Integer)
                DosePerDay = CType(Me.txtdoses.Text.ToString.Trim, Integer)

                For Each dr In ds_Randomization.Tables(0).Rows
                    indexNo = 0

                    indexRow += 1
                    For indexDose As Integer = 1 To DosePerDay
                        indexNo = indexNo + 1

                        dr_Dose = ds_DosingDetail.Tables(0).NewRow
                        dr_Dose("nDosingDetailNo") = Val(indexRow.ToString.Trim() & Days.ToString.Trim() & indexDose.ToString.Trim())
                        dr_Dose("vDosingBarCode") = indexRow.ToString.Trim() & Days.ToString.Trim() & indexDose.ToString.Trim()
                        If HIsTestSite.Value = "" Or Me.HIsTestSite.Value = "N" Then
                            If HParentProject.Value <> "" Then
                                dr_Dose("vParentWorkspaceId") = HParentProject.Value
                            Else
                                dr_Dose("vParentWorkspaceId") = HProjectId.Value
                            End If
                        End If
                        dr_Dose("vWorkspaceId") = HProjectId.Value
                        dr_Dose("vGenericName") = Convert.ToString(Me.txtDrugNameForPrint.Text)
                        dr_Dose("nProductNo") = Me.ddlDrug.SelectedValue.ToString()
                        dr_Dose("nProductTypeID") = Me.ddlProductType.SelectedValue.Trim()
                        dr_Dose("vProductType") = Me.ddlProductType.SelectedItem.Text.ToString.Trim()
                        dr_Dose("iPeriod") = dr("iPeriod")
                        dr_Dose("iMySubjectNo") = dr("iMySubjectNo")
                        dr_Dose("vdosageform") = Me.txtDosage.Text.Trim()
                        dr_Dose("vStrength") = Me.txtstrength.Text.Trim()
                        dr_Dose("vProtocolNo") = Me.txtprotocol.Text.Trim()
                        dr_Dose("dExpiryDate") = CType(Me.txtExpiryDate.Text.Trim(), Date).ToString("dd-MMM-yyyy")
                        dr_Dose("vInstruction") = Me.txtInstruction.Text.Trim()
                        dr_Dose("vStoragecondition") = Me.txtcondition.Text.Trim()
                        dr_Dose("iDayNo") = Days
                        dr_Dose("iDoseNo") = indexNo
                        dr_Dose("iModifyBy") = Me.Session(S_UserID)
                        dr_Dose("cStatusindi") = "N"
                        dr_Dose("vRandomizationcode") = dr("vRandomizationcode") 'Added By Mrunal Parekh 28-Dec-2011 for randomizationcode
                        dr_Dose("vPIName") = Me.ddlPIName.SelectedItem.Text.ToString.Trim()
                        dr_Dose("vRouteAdmin") = Me.txtRouteAdmin.Text.ToString.Trim()
                        dr_Dose("vBatchNo") = Me.ddlBatchno.SelectedItem.Text.ToString.Trim()
                        dr_Dose("nBatchNo") = Me.ddlBatchno.SelectedValue.Trim()  ''Added by ketan
                        dr_Dose("iDispenseQtyPerSubject") = Me.txtDispenseQtyPerSubject.Text.ToString.Trim() ''Added by ketan
                        dr_Dose("dModifyOn") = CDate(ObjCommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName)).DateTime).GetDateTimeFormats()(23)          '' Added by Rahul Rupareliya
                        ds_DosingDetail.Tables(0).Rows.Add(dr_Dose)

                    Next indexDose

                    ds_DosingDetail.Tables(0).AcceptChanges()
                Next dr
            Else
                Days = 1
                DosePerDay = 1

                For Each dr In ds_Randomization.Tables(0).Rows
                    ''DosePerDay = Convert.ToInt32(Dose.Trim()) / Convert.ToInt32(Days.Trim())

                    indexNo = 0

                    indexRow += 1
                    For indexDay As Integer = 1 To Convert.ToInt32(Days.Trim())
                        For indexDose As Integer = 1 To DosePerDay
                            indexNo = indexNo + 1

                            dr_Dose = ds_DosingDetail.Tables(0).NewRow
                            dr_Dose("nDosingDetailNo") = Val(indexRow.ToString.Trim() & indexDay.ToString.Trim() & indexDose.ToString.Trim())
                            dr_Dose("vDosingBarCode") = indexRow.ToString.Trim() & indexDay.ToString.Trim() & indexDose.ToString.Trim()
                            If HIsTestSite.Value = "" Or Me.HIsTestSite.Value = "N" Then
                                If HParentProject.Value <> "" Then
                                    dr_Dose("vParentWorkspaceId") = HParentProject.Value
                                Else
                                    dr_Dose("vParentWorkspaceId") = HProjectId.Value
                                End If
                            End If
                            dr_Dose("vWorkspaceId") = HProjectId.Value
                            dr_Dose("vGenericName") = Convert.ToString(Me.txtDrugNameForPrint.Text)
                            dr_Dose("nProductNo") = Me.ddlDrug.SelectedValue.ToString() ''Added by ketan
                            dr_Dose("nProductTypeID") = Me.ddlProductType.SelectedValue.Trim 'added by ketan
                            dr_Dose("vProductType") = Me.ddlProductType.SelectedItem.Text.ToString.Trim ' Added By Mrunal Parekh on 28-Dec-2011 for change in double blinded study
                            dr_Dose("iPeriod") = dr("iPeriod")
                            dr_Dose("iMySubjectNo") = dr("iMySubjectNo")
                            dr_Dose("vdosageform") = Me.txtDosage.Text.Trim()
                            dr_Dose("vStrength") = Me.txtstrength.Text.Trim()
                            dr_Dose("vProtocolNo") = Me.txtprotocol.Text.Trim()
                            dr_Dose("dExpiryDate") = CType(Me.txtExpiryDate.Text.Trim(), Date).ToString("dd-MMM-yyyy")
                            dr_Dose("vInstruction") = Me.txtInstruction.Text.Trim()
                            dr_Dose("vStoragecondition") = Me.txtcondition.Text.Trim()

                            dr_Dose("iDayNo") = indexDay
                            dr_Dose("iDoseNo") = indexNo

                            dr_Dose("iModifyBy") = Me.Session(S_UserID)
                            dr_Dose("cStatusindi") = "N"
                            dr_Dose("vRandomizationcode") = dr("vRandomizationcode")
                            dr_Dose("vPIName") = Me.ddlPIName.SelectedItem.Text.ToString.Trim()
                            dr_Dose("vRouteAdmin") = Me.txtRouteAdmin.Text.ToString.Trim()
                            dr_Dose("vBatchNo") = Me.ddlBatchno.SelectedItem.Text.ToString.Trim()
                            dr_Dose("nBatchNo") = Me.ddlBatchno.SelectedValue.Trim()
                            dr_Dose("iDispenseQtyPerSubject") = Me.txtDispenseQtyPerSubject.Text.ToString.Trim()

                            dr_Dose("dModifyOn") = CDate(ObjCommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName)).DateTime).GetDateTimeFormats()(23)          '' Added by Rahul Rupareliya
                            ds_DosingDetail.Tables(0).Rows.Add(dr_Dose)

                        Next indexDose

                    Next indexDay
                    ds_DosingDetail.Tables(0).AcceptChanges()
                Next dr
            End If

            If Me.txtBlanks.Text > 0 Then
                If Me.chkMultidose.Checked = True Then
                    For i As Integer = 0 To dt_Multidose.Rows.Count - 2
                        Days = CType(dt_Multidose.Rows(i)("Days").ToString.Trim, Integer)
                        DosePerDay = CType(dt_Multidose.Rows(i)("Doses").ToString.Trim, Integer)

                        For cntBl As Integer = 0 To Me.txtBlanks.Text - 1

                            indexNo = 0
                            indexRow += 1

                            For indexDose As Integer = 1 To DosePerDay
                                indexNo = indexNo + 1

                                dr_Dose = ds_DosingDetail.Tables(0).NewRow
                                dr_Dose("nDosingDetailNo") = ds_DosingDetail.Tables(0).Compute("Max(nDosingDetailNo) + 1 ", "1=1").ToString.Trim()
                                dr_Dose("vDosingBarCode") = ds_DosingDetail.Tables(0).Compute("Max(nDosingDetailNo) + 1 ", "1=1").ToString.Trim()
                                dr_Dose("vWorkspaceId") = Me.HProjectId.Value.Trim()
                                dr_Dose("vGenericName") = Convert.ToString(Me.txtDrugNameForPrint.Text)
                                dr_Dose("nProductNo") = Me.ddlDrug.SelectedValue.ToString()
                                dr_Dose("nProductTypeID") = Me.ddlProductType.SelectedValue.Trim()
                                dr_Dose("vProductType") = Me.ddlProductType.SelectedItem.Text.ToString.Trim()
                                dr_Dose("iPeriod") = Me.ddlPeriod.SelectedValue.Trim()
                                dr_Dose("vdosageform") = Me.txtDosage.Text.Trim()
                                dr_Dose("vStrength") = Me.txtstrength.Text.Trim()
                                dr_Dose("vProtocolNo") = Me.txtprotocol.Text.Trim()
                                dr_Dose("dExpiryDate") = CType(Me.txtExpiryDate.Text.Trim(), Date).ToString("dd-MMM-yyyy")
                                dr_Dose("vInstruction") = Me.txtInstruction.Text.Trim()
                                dr_Dose("vStoragecondition") = Me.txtcondition.Text.Trim()

                                dr_Dose("iDayNo") = Days
                                dr_Dose("iDoseNo") = indexNo

                                dr_Dose("iModifyBy") = Me.Session(S_UserID)
                                dr_Dose("cStatusindi") = "N"

                                If HIsTestSite.Value = "" Or Me.HIsTestSite.Value = "N" Then

                                    If HParentProject.Value <> "" Then
                                        dr_Dose("vParentWorkspaceId") = HParentProject.Value
                                    Else
                                        dr_Dose("vParentWorkspaceId") = HProjectId.Value
                                    End If

                                End If
                                dr_Dose("vPIName") = Me.ddlPIName.SelectedItem.Text.ToString.Trim()
                                dr_Dose("vRouteAdmin") = Me.txtRouteAdmin.Text.ToString.Trim()
                                dr_Dose("vBatchNo") = Me.ddlBatchno.SelectedItem.Text.ToString.Trim()
                                dr_Dose("nBatchNo") = Convert.ToDecimal(Me.ddlBatchno.SelectedValue.ToString.Trim())   ''Added by ketan
                                dr_Dose("iDispenseQtyPerSubject") = Me.txtDispenseQtyPerSubject.Text.ToString.Trim() ''Added by ketan

                                dr_Dose("dModifyOn") = CDate(ObjCommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName)).DateTime).GetDateTimeFormats()(23)          '' Added by Rahul Rupareliya
                                ds_DosingDetail.Tables(0).Rows.Add(dr_Dose)

                            Next indexDose

                            ds_DosingDetail.Tables(0).AcceptChanges()

                        Next cntBl
                    Next i
                ElseIf Me.chkDeletebarcode.Checked = True Then
                    Days = CType(Me.txtdays.Text.ToString.Trim, Integer)
                    DosePerDay = CType(Me.txtdoses.Text.ToString.Trim, Integer)

                    For cntBl As Integer = 0 To Me.txtBlanks.Text - 1

                        indexNo = 0
                        indexRow += 1

                        For indexDose As Integer = 1 To DosePerDay
                            indexNo = indexNo + 1

                            dr_Dose = ds_DosingDetail.Tables(0).NewRow
                            dr_Dose("nDosingDetailNo") = ds_DosingDetail.Tables(0).Compute("Max(nDosingDetailNo) + 1 ", "1=1").ToString.Trim()
                            dr_Dose("vDosingBarCode") = ds_DosingDetail.Tables(0).Compute("Max(nDosingDetailNo) + 1 ", "1=1").ToString.Trim()
                            dr_Dose("vWorkspaceId") = Me.HProjectId.Value.Trim()
                            dr_Dose("vGenericName") = Convert.ToString(Me.txtDrugNameForPrint.Text)
                            dr_Dose("nProductNo") = Me.ddlDrug.SelectedValue.ToString() ''Added by ketan
                            dr_Dose("nProductTypeID") = Me.ddlProductType.SelectedValue.Trim 'added by ketan
                            dr_Dose("vProductType") = Me.ddlProductType.SelectedItem.Text.ToString.Trim ' Added By Mrunal Parekh on 28-Dec-2011 for change in double blinded study
                            dr_Dose("iPeriod") = Me.ddlPeriod.SelectedValue.Trim()
                            dr_Dose("vdosageform") = Me.txtDosage.Text.Trim()
                            dr_Dose("vStrength") = Me.txtstrength.Text.Trim()
                            dr_Dose("vProtocolNo") = Me.txtprotocol.Text.Trim()
                            dr_Dose("dExpiryDate") = CType(Me.txtExpiryDate.Text.Trim(), Date).ToString("dd-MMM-yyyy")
                            dr_Dose("vInstruction") = Me.txtInstruction.Text.Trim()
                            dr_Dose("vStoragecondition") = Me.txtcondition.Text.Trim()

                            dr_Dose("iDayNo") = Days
                            dr_Dose("iDoseNo") = indexNo

                            dr_Dose("iModifyBy") = Me.Session(S_UserID)
                            dr_Dose("cStatusindi") = "N"

                            If HIsTestSite.Value = "" Or Me.HIsTestSite.Value = "N" Then

                                If HParentProject.Value <> "" Then
                                    dr_Dose("vParentWorkspaceId") = HParentProject.Value
                                Else
                                    dr_Dose("vParentWorkspaceId") = HProjectId.Value
                                End If

                            End If
                            dr_Dose("vPIName") = Me.ddlPIName.SelectedItem.Text.ToString.Trim()
                            dr_Dose("vRouteAdmin") = Me.txtRouteAdmin.Text.ToString.Trim()
                            dr_Dose("vBatchNo") = Me.ddlBatchno.SelectedItem.Text.ToString.Trim()
                            dr_Dose("nBatchNo") = Convert.ToDecimal(Me.ddlBatchno.SelectedValue.ToString.Trim())  ''Added by ketan
                            dr_Dose("iDispenseQtyPerSubject") = Me.txtDispenseQtyPerSubject.Text.ToString.Trim() ''Added by ketan

                            dr_Dose("dModifyOn") = CDate(ObjCommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName)).DateTime).GetDateTimeFormats()(23)          '' Added by Rahul Rupareliya
                            ds_DosingDetail.Tables(0).Rows.Add(dr_Dose)

                        Next indexDose

                        ds_DosingDetail.Tables(0).AcceptChanges()

                    Next cntBl
                Else
                    Days = 1
                    DosePerDay = 1

                    For cntBl As Integer = 0 To Me.txtBlanks.Text - 1

                        indexNo = 0
                        indexRow += 1
                        For indexDay As Integer = 1 To Convert.ToInt32(Days.Trim())

                            For indexDose As Integer = 1 To DosePerDay
                                indexNo = indexNo + 1

                                dr_Dose = ds_DosingDetail.Tables(0).NewRow
                                dr_Dose("nDosingDetailNo") = ds_DosingDetail.Tables(0).Compute("Max(nDosingDetailNo) + 1 ", "1=1").ToString.Trim()
                                dr_Dose("vDosingBarCode") = ds_DosingDetail.Tables(0).Compute("Max(nDosingDetailNo) + 1 ", "1=1").ToString.Trim()
                                dr_Dose("vWorkspaceId") = Me.HProjectId.Value.Trim()
                                dr_Dose("vGenericName") = Convert.ToString(Me.txtDrugNameForPrint.Text)
                                dr_Dose("nProductNo") = Me.ddlDrug.SelectedValue.ToString() ''Added by ketan
                                dr_Dose("nProductTypeID") = Me.ddlProductType.SelectedValue.Trim 'added by ketan
                                dr_Dose("vProductType") = Me.ddlProductType.SelectedItem.Text.ToString.Trim ' Added By Mrunal Parekh on 28-Dec-2011 for change in double blinded study
                                dr_Dose("iPeriod") = Me.ddlPeriod.SelectedValue.Trim()
                                dr_Dose("vdosageform") = Me.txtDosage.Text.Trim()
                                dr_Dose("vStrength") = Me.txtstrength.Text.Trim()
                                dr_Dose("vProtocolNo") = Me.txtprotocol.Text.Trim()
                                dr_Dose("dExpiryDate") = CType(Me.txtExpiryDate.Text.Trim(), Date).ToString("dd-MMM-yyyy")
                                dr_Dose("vInstruction") = Me.txtInstruction.Text.Trim()
                                dr_Dose("vStoragecondition") = Me.txtcondition.Text.Trim()

                                dr_Dose("iDayNo") = indexDay
                                dr_Dose("iDoseNo") = indexNo

                                dr_Dose("iModifyBy") = Me.Session(S_UserID)
                                dr_Dose("cStatusindi") = "N"

                                If HIsTestSite.Value = "" Or Me.HIsTestSite.Value = "N" Then
                                    If HParentProject.Value <> "" Then
                                        dr_Dose("vParentWorkspaceId") = HParentProject.Value
                                    Else
                                        dr_Dose("vParentWorkspaceId") = HProjectId.Value
                                    End If
                                End If
                                dr_Dose("vPIName") = Me.ddlPIName.SelectedItem.Text.ToString.Trim()
                                dr_Dose("vRouteAdmin") = Me.txtRouteAdmin.Text.ToString.Trim()
                                dr_Dose("vBatchNo") = Me.ddlBatchno.SelectedItem.Text.ToString.Trim()
                                dr_Dose("nBatchNo") = Convert.ToDecimal(Me.ddlBatchno.SelectedValue.ToString.Trim())  ''Added by ketan
                                dr_Dose("iDispenseQtyPerSubject") = Me.txtDispenseQtyPerSubject.Text.ToString.Trim() ''Added by ketan

                                dr_Dose("dModifyOn") = CDate(ObjCommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName)).DateTime).GetDateTimeFormats()(23)          '' Added by Rahul Rupareliya
                                ds_DosingDetail.Tables(0).Rows.Add(dr_Dose)

                            Next indexDose

                        Next indexDay
                        ds_DosingDetail.Tables(0).AcceptChanges()

                    Next cntBl
                End If
            End If
            ds_Save = ds_DosingDetail

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..AssignValues")
            Return False
        End Try
    End Function

#End Region

#Region "Reset Page"

    Private Sub ResetPage()
        Me.txtcondition.Text = ""
        Me.txtDosage.Text = ""
        Me.txtExpiryDate.Text = ""
        Me.txtInstruction.Text = ""
        Me.txtprotocol.Text = ""
        Me.txtstrength.Text = ""
        Me.txtDispenseQtyPerSubject.Text = ""
        Me.HstarteiMySubjectNo.Value = ""
        Me.txtSubjectInitial.Text = ""
        Me.txtSubjectLast.Text = ""
        Me.chkMultidose.Checked = False
        Me.txtDispenseQtyPerSubject.Text = ""
        If Me.ddlDrug.SelectedIndex > 0 Then
            Me.ddlDrug.SelectedIndex = 0
        End If
        If Me.ddlBatchno.SelectedIndex > 0 Then
            Me.ddlBatchno.SelectedIndex = 0
        End If
    End Sub

    Private Sub ResetPage1()
        Me.txtcondition.Text = ""
        Me.txtExpiryDate.Text = ""
        Me.txtInstruction.Text = ""
        Me.txtprotocol.Text = ""
        Me.HstarteiMySubjectNo.Value = ""
        Me.txtSubjectInitial.Text = ""
        Me.txtSubjectLast.Text = ""
        Me.txtDispenseQtyPerSubject.Text = ""
        Me.chkMultidose.Checked = False
        Me.chkDeletebarcode.Checked = False
        Me.txtBlanks.Text = 0
        If Me.ddlProductType.SelectedIndex > 0 Then
            Me.ddlProductType.SelectedIndex = 0
        End If
        Me.txtRouteAdmin.Text = ""
    End Sub

#End Region

#Region "Fill Function"

    Private Function FillPeriodDropDown() As Boolean
        Dim dsPeriod As New DataSet
        Dim WorkspaceId As String = String.Empty
        Dim ds_WorkSpaceNodeDetail As New DataSet
        Dim View_WorkSpaceNodeDetail As New DataView
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty

        Try

            wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "' "
            ds_WorkSpaceNodeDetail = Nothing
            If Not objHelp.GetViewWorkSpaceNodeDetail(wstr, ds_WorkSpaceNodeDetail, estr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data from WorkSpaceModeDetail:" + estr, Me.Page)
                Return False
            End If

            View_WorkSpaceNodeDetail = ds_WorkSpaceNodeDetail.Tables(0).DefaultView
            View_WorkSpaceNodeDetail.Sort = "iPeriod"

            Me.ddlPeriod.DataSource = ds_WorkSpaceNodeDetail.Tables(0).DefaultView.ToTable(True, "iPeriod".ToString())
            Me.ddlPeriod.DataValueField = "iPeriod"
            Me.ddlPeriod.DataTextField = "iPeriod"
            Me.ddlPeriod.DataBind()
            Me.ddlPeriod.Items.Insert(0, New ListItem("--Select Period--", 0))

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".FillPeriodDropDown")
            Return False
        End Try
    End Function
    Private Function FillProductTypeDetails() As Boolean
        Dim WorkspaceId As String = String.Empty
        Dim DsWorkspace As New DataSet
        Dim wStr As String = String.Empty
        Dim dsProductType As New DataSet

        Dim ProductType As String = String.Empty
        Dim Drug As String = String.Empty
        Try
            If Me.ddlProductType.SelectedValue.ToString.Trim() = "" Then
                ProductType = "0"
                Drug = "0"
            End If
            wStr = Me.HProjectId.Value.ToString.Trim + "##" + ProductType + "##" + Drug
            dsProductType = Me.objHelp.ProcedureExecute("Proc_GetDrugDtlPMS", wStr)

            If Not IsNothing(dsProductType) AndAlso dsProductType.Tables(0).Rows.Count > 0 Then
                Me.ddlProductType.DataSource = dsProductType.Tables(0)
                Me.ddlProductType.DataTextField = "vProductType"
                Me.ddlProductType.DataValueField = "nProductTypeID"
                Me.ddlProductType.DataBind()
                Me.ddlProductType.Items.Insert(0, New ListItem("--Select Product Type--", 0))
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillDrugDetails")
            Return False
        End Try
    End Function

    Private Function FillDrugDetails() As Boolean
        Dim WorkspaceId As String = String.Empty
        Dim DsWorkspace As New DataSet
        Dim wStr As String = String.Empty
        Dim dsDrug As New DataSet

        Dim ProductType As String = String.Empty
        Dim Drug As String = String.Empty

        Try
            If Me.ddlProductType.SelectedValue.ToString.Trim() = "" Then
                ProductType = "0"
            Else
                ProductType = Me.ddlProductType.SelectedValue.ToString.Trim()

            End If
            If Me.ddlDrug.SelectedValue.ToString.Trim() = "" Then
                Drug = "0"
            Else
                Drug = Me.ddlDrug.SelectedValue.ToString.Trim()
            End If

            wStr = Me.HProjectId.Value.Trim()
            wStr = Me.HProjectId.Value.ToString.Trim + "##" + ProductType + "##" + Drug

            dsDrug = Me.objHelp.ProcedureExecute("Proc_GetDrugDtlPMS", wStr)

            If Not IsNothing(dsDrug) AndAlso dsDrug.Tables(1).Rows.Count > 0 Then
                Me.ddlDrug.DataSource = dsDrug.Tables(1)
                Me.ddlDrug.DataTextField = "vProductName"
                Me.ddlDrug.DataValueField = "nProductNo"
                Me.ddlDrug.DataBind()
                Me.ddlDrug.Items.Insert(0, New ListItem("--Select Drug--", 0))
                Me.txtstrength.Text = dsDrug.Tables(1).Rows(0).Item("vProductStrength")
                Me.txtDosage.Text = dsDrug.Tables(1).Rows(0).Item("vProductForm")
            Else
                Me.ddlDrug.Items.Clear()
                Me.txtstrength.Text = ""
                Me.txtDosage.Text = ""
                Me.ddlBatchno.Items.Clear()
            End If
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillDrugDetails")
            Return False
        End Try
    End Function

    Private Function FillBatchNo() As Boolean
        Dim WorkspaceId As String = String.Empty
        Dim DsWorkspace As New DataSet
        Dim wStr As String = String.Empty
        Dim dsDrug As New DataSet
        Dim ProductType As String = String.Empty
        Dim Drug As String = String.Empty
        Try
            If Me.ddlProductType.SelectedValue.ToString.Trim() = "" Then
                ProductType = "0"
            Else
                ProductType = Me.ddlProductType.SelectedValue.ToString.Trim()
            End If
            If Me.ddlDrug.SelectedValue.ToString.Trim() = "" Then
                Drug = "0"
            Else
                Drug = Me.ddlDrug.SelectedValue.ToString.Trim()
                If Drug <> "0" Then
                    If Convert.ToString(ddlDrug.SelectedItem.Text).Length < 60 Then
                        txtDrugNameForPrint.Text = Convert.ToString(ddlDrug.SelectedItem.Text).Substring(0, Convert.ToString(ddlDrug.SelectedItem.Text).Length)
                    Else
                        txtDrugNameForPrint.Text = Convert.ToString(ddlDrug.SelectedItem.Text).Substring(0, 60)
                    End If
                Else
                    txtDrugNameForPrint.Text = ""
                End If

            End If

            wStr = Me.HProjectId.Value.Trim()
            wStr = Me.HProjectId.Value.ToString.Trim + "##" + ProductType + "##" + Drug

            dsDrug = Me.objHelp.ProcedureExecute("Proc_GetDrugDtlPMS", wStr)


            If Not IsNothing(dsDrug) AndAlso dsDrug.Tables(2).Rows.Count > 0 Then
                Me.ddlBatchno.DataSource = dsDrug.Tables(2)
                Me.ddlBatchno.DataTextField = "vBatchLotNo"
                Me.ddlBatchno.DataValueField = "nStudyProductBatchNo"
                Me.ddlBatchno.DataBind()
                Me.ddlBatchno.Items.Insert(0, New ListItem("--Select Batch No--", 0))
            Else
                ddlBatchno.Items.Clear()
            End If

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillDrugDetails")
            Return False
        End Try
    End Function

    Private Function FillGrid() As Boolean
        Dim dsPeriod As New DataSet
        Dim dvPeriod As New DataView
        Dim WorkspaceId As String = String.Empty
        Dim wstr As String = String.Empty

        Try
            WorkspaceId = Me.HProjectId.Value.Trim()

            Me.gvwDosingDetail.DataSource = Nothing
            Me.gvwDosingDetail.DataBind()
            Me.BtnDelete1.Visible = False
            Me.BtnDelete2.Visible = False
            Me.BtnPrint1.Visible = False
            Me.BtnPrint2.Visible = False

            wstr = "iPeriod=" & Me.ddlPeriod.SelectedValue.Trim() & " AND vWorkSpaceId='" & WorkspaceId & "' and cstatusindi <>'D' "

            If Not objHelp.View_DosingDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                         dsPeriod, eStr_Retu) Then

                ShowErrorMessage(eStr_Retu, "")
                Exit Function
            End If

            If dsPeriod.Tables(0).Rows.Count > 0 Then
                dvPeriod = dsPeriod.Tables(0).DefaultView()
                dvPeriod.Sort = "nDosingDetailNo"
                Me.gvwDosingDetail.DataSource = dvPeriod.ToTable() '.DefaultView.ToTable("WorkSpaceNodeDetail", True, paramArry(0))
                Me.gvwDosingDetail.DataBind()

                Me.ViewState(VS_DosingDetail) = dvPeriod.ToTable()

                Me.BtnDelete1.Visible = True
                Me.BtnDelete2.Visible = True
                Me.divnote.Style.Add("Display", "")
                Me.PanelProjectSpecific.Style.Add("display", "")
                Me.pnlgvmedexworkspadce.Style.Add("display", "")
            Else
                Me.PanelProjectSpecific.Style.Add("display", "none")
                Me.pnlgvmedexworkspadce.Style.Add("display", "none")
            End If

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillGrid")
            Return False
        End Try
    End Function

    Private Function Print() As Boolean
        Dim BarCodes As String = String.Empty
        Dim Drug As String = String.Empty
        Dim Project As String = String.Empty
        Dim Subject As String = String.Empty
        Dim Dose As String = String.Empty
        Dim Test As String = String.Empty

        For index As Integer = 0 To Me.gvwDosingDetail.Rows.Count - 1

            If CType(Me.gvwDosingDetail.Rows(index).FindControl("chkSelectBarCode"), CheckBox).Checked = True Then

                BarCodes += IIf(BarCodes.Trim() = "", "", ",*") + _
                            IIf(Me.gvwDosingDetail.Rows(index).Cells(GVCell_BarCode).Text.ToString.Trim() = "", "-", Me.gvwDosingDetail.Rows(index).Cells(GVCell_BarCode).Text.ToString.Trim())

                Drug += IIf(Drug.Trim() = "", "", ",*") + _
                        IIf(Me.gvwDosingDetail.Rows(index).Cells(GVCell_Drug).Text.ToString.Trim() = "", "-", Me.gvwDosingDetail.Rows(index).Cells(GVCell_Drug).Text.ToString.Trim())

                Project += IIf(Project.Trim() = "", "", ",*") + Me.gvwDosingDetail.Rows(index).Cells(GVCell_ProjectNo).Text.ToString.Trim() + " (" + _
                          Me.gvwDosingDetail.Rows(index).Cells(GVCell_Period).Text.ToString.Trim() + ")"

                Subject += IIf(Subject.Trim() = "", "", ",*") + _
                        IIf(Me.gvwDosingDetail.Rows(index).Cells(GVCell_Replaced).Text.ToString.Trim() = "", "-", Me.gvwDosingDetail.Rows(index).Cells(GVCell_Replaced).Text.ToString.Trim())

                Dose += IIf(Dose.Trim() = "", "", ",*") + Me.gvwDosingDetail.Rows(index).Cells(GVCell_Dose).Text.ToString.Trim() + "-" + _
                        Me.gvwDosingDetail.Rows(index).Cells(GVCell_DosageForm).Text.ToString.Trim()

                If chkdoubleblinded.Checked = False Then
                    Test += IIf(Test.Trim() = "", "", ",*") + _
                            IIf(Me.gvwDosingDetail.Rows(index).Cells(GVCell_Type).Text.ToString.Trim() = "", "-", Me.gvwDosingDetail.Rows(index).Cells(GVCell_Type).Text.ToString.Trim())
                Else
                    Test += IIf(Test.Trim() = "", "", ",*") + _
                           IIf(Me.gvwDosingDetail.Rows(index).Cells(GVCell_randomizationcode).Text.ToString.Trim() = "", "-", Me.gvwDosingDetail.Rows(index).Cells(GVCell_randomizationcode).Text.ToString.Trim())
                End If

            End If

        Next index

        ScriptManager.RegisterStartupScript(Me.Page(), Me.GetType(), "PrintBarCode", _
            "printLbl('" & BarCodes & "','" & Drug & "','" & Project & "','" & Subject & "','" & Dose & "','" & Test & "');", True)
        Return True

    End Function

    Private Function FillProductType() As Boolean
        Dim wStr As String = String.Empty
        Dim ds_ProductType As New DataSet

        Try
            If HParentProject.Value <> "" Then

                wStr = "select distinct vProductType from RandomizationDetail where vWorkspaceId = '" + Me.HParentProject.Value + _
                   "' and iPeriod = " + Me.ddlPeriod.SelectedValue.Trim + " and cstatusindi <>'D'"
            Else
                wStr = "select distinct vProductType from RandomizationDetail where vWorkspaceId = '" + Me.HProjectId.Value + _
                   "' and iPeriod = " + Me.ddlPeriod.SelectedValue.Trim + " and cstatusindi <>'D'"
            End If

            ds_ProductType = objHelp.GetResultSet(wStr, "RandomizationDetail")

            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message.ToString, "Error While Getting Product Type")
            Return False
        End Try

    End Function

    Private Function FillAuditGrid() As Boolean
        Dim dsViewDosingDetail As New DataSet
        Dim dvViewDosingDetail As New DataView
        Dim WorkspaceId As String = String.Empty
        Dim wstr As String = String.Empty

        Try
            WorkspaceId = Me.HProjectId.Value.Trim()

            Me.GVAudit.DataSource = Nothing
            Me.GVAudit.DataBind()

            wstr = "iPeriod=" & Me.ddlPeriod.SelectedValue.Trim() & "AND vWorkSpaceId='" & WorkspaceId & "' and cstatusindi = 'D' "

            If Not objHelp.View_DosingDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                         dsViewDosingDetail, eStr_Retu) Then

                ShowErrorMessage(eStr_Retu, "")
                Exit Function
            End If

            If dsViewDosingDetail.Tables(0).Rows.Count > 0 Then
                dvViewDosingDetail = dsViewDosingDetail.Tables(0).DefaultView()
                dvViewDosingDetail.Sort = "nDosingDetailNo"
                Me.GVAudit.DataSource = dvViewDosingDetail.ToTable() '.DefaultView.ToTable("WorkSpaceNodeDetail", True, paramArry(0))
                Me.GVAudit.DataBind()

            End If


            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillAuditGrid")
            Return False
        End Try
    End Function

    Private Function setcheckboxvalue() As Boolean
        Dim dsrandomcode As New DataSet
        Dim WorkspaceId As String = String.Empty
        Dim Wstr As String = String.Empty


        Try
            If Not Me.Request.QueryString("mode") = Nothing Then
                ViewState("Type") = Convert.ToString(Me.Request.QueryString("mode")).Trim()
            End If
            If HParentProject.Value <> "" Then
                WorkspaceId = Me.HParentProject.Value.Trim()
            Else
                WorkspaceId = Me.HProjectId.Value.Trim()
            End If

            Wstr = "vWorkSpaceId='" & WorkspaceId & "' and cstatusindi <>'D' and vRandomizationcode <> ''"

            If Not objHelp.GetRandomizationDetail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                  dsrandomcode, eStr_Retu) Then
                ShowErrorMessage(eStr_Retu, "")
                Exit Function
            End If

            Me.chkdoubleblinded.Enabled = False
            Me.chkdoubleblinded.Checked = False
            Me.txtBlanks.Enabled = True
            Me.ddlProductType.Enabled = True

            If ViewState("Type") = "1" Then
                If Not dsrandomcode.Tables(0).Rows.Count > 0 Then
                    Me.ObjCommon.ShowAlert("You Can Not Generate Labels Because Project Is Not Double Blinded Study.You Can Only Print Labels", Me.Page)
                    Me.btnSave.Visible = False
                    Return False
                    Exit Function
                Else
                    Me.chkdoubleblinded.Checked = True
                    Me.txtBlanks.Enabled = False
                    Me.trBlankBarcodes.Attributes.Add("style", "display:none;")
                    Me.txtBlanks.Text = 0
                    If HParentProject.Value <> "" Then
                        Me.trGenerateBlankIpLabel.Attributes.Add("style", "")
                    End If
                End If
            Else
                If dsrandomcode.Tables(0).Rows.Count > 0 Then
                    Me.ObjCommon.ShowAlert("You Can Not Generate Labels Because Project Is Double Blinded Study.You Can Only Print Labels", Me.Page)
                    Me.chkdoubleblinded.Checked = True
                    Me.btnSave.Visible = False
                    Me.ddlProductType.Enabled = False
                    Me.txtBlanks.Enabled = False
                    Me.txtBlanks.Text = 0
                    Return False
                    Exit Function
                End If
            End If

            Return True
        Catch ex As Exception

        End Try
    End Function

    Private Function checkLastSubject() As Boolean
        Dim wStr As String = String.Empty
        Dim wStr1 As String = String.Empty
        Dim eStr As String = String.Empty
        Dim dsDosing As New DataSet
        Dim dsAssignment As New DataSet

        wStr = HParentProject.Value + "##" + HProjectId.Value + "##" + Me.ddlPeriod.SelectedValue.Trim()

        dsDosing = Me.objHelp.ProcedureExecute("Proc_ExtraDosedSubjectwithoutAssignment", wStr)
        HstarteiMySubjectNo.Value = ""
        If dsDosing.Tables(0).Rows.Count > 0 Then
            For i As Integer = 0 To dsDosing.Tables(0).Rows.Count - 1
                HstarteiMySubjectNo.Value += dsDosing.Tables(0).Rows(i).Item("iMySubjectNo").ToString() + ","
            Next
            HstarteiMySubjectNo.Value = HstarteiMySubjectNo.Value.Substring(0, HstarteiMySubjectNo.Value.LastIndexOf(","))
        End If
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

#Region "Grid Events"

    Protected Sub gvwDosingDetail_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwDosingDetail.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Or _
        e.Row.RowType = DataControlRowType.Header Then

            e.Row.Cells(GVCell_DosingDetailNo).Visible = False
            e.Row.Cells(GVCell_DosedOn).Style.Add("display", "none")
            e.Row.Cells(GVCell_Replaced).Style.Add("display", "none")
            e.Row.Cells(GVCell_ReplaceFlag).Style.Add("display", "none")
            If chkdoubleblinded.Checked = False Then
                e.Row.Cells(GVCell_randomizationcode).Style.Add("display", "none")
            End If

            If chkdoubleblinded.Checked = True And ViewState("Type") <> "1" Then
                e.Row.Cells(GVCell_Drug).Style.Add("display", "none")
                e.Row.Cells(GVCell_Type).Style.Add("display", "none")
                e.Row.Cells(GVCell_randomizationcode).Style.Add("display", "")
                e.Row.Cells(GVCell_DosageForm).Style.Add("display", "none")
                e.Row.Cells(GVCell_Storage).Style.Add("display", "none")
                e.Row.Cells(GVCell_Streangth).Style.Add("display", "none")
                e.Row.Cells(GVCell_ExpDate).Style.Add("display", "none")
                e.Row.Cells(GVCell_Instruction).Style.Add("display", "none")
            End If
        End If

    End Sub
    Protected Sub gvwDosingDetail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwDosingDetail.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            If e.Row.Cells(GVCell_DosedOn).Text.Trim() = "&nbsp;" Then
                e.Row.Cells(GVCell_DosedOn).Text = ""
            End If

            If e.Row.Cells(GVCell_ReplaceFlag).Text.Trim() = "Y" Then
                e.Row.BackColor = Drawing.Color.Red
                e.Row.Enabled = False
            End If

        End If

    End Sub

    Protected Sub gvwDosingDetail_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvwDosingDetail.PageIndexChanging

        gvwDosingDetail.PageIndex = e.NewPageIndex
        FillGrid()

    End Sub

    Protected Sub gvwDosingDetail_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvwDosingDetail.RowCommand
        Dim index As Integer = CType(e.CommandArgument, Integer)

        If e.CommandName.ToUpper.Trim() = "ASSIGN" Then
            Me.divReplacement.Visible = True
            Me.pnlReplace.Visible = True

            Me.lblLabelId.Text = ""
            Me.lblLabelId.Text = Me.gvwDosingDetail.Rows(index).Cells(GVCell_BarCode).Text.Trim()

            Me.txtAssignRemark.Text = ""

            Me.txtreplaceCode.Focus()

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + _
                                                     Me.divReplacement.ClientID.ToString.Trim() + "');", True)
        End If

    End Sub

    Protected Sub GVAudit_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVAudit.RowCreated

        If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Or _
            e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(GVAudit_DosingDetailNo).Visible = False
            e.Row.Cells(GVAudit_randomizationcode).Style.Add("display", "none")

            If chkdoubleblinded.Checked = True Then
                e.Row.Cells(GVAudit_Drug).Style.Add("display", "none")
                e.Row.Cells(GVAudit_Type).Style.Add("display", "none")
                e.Row.Cells(GVAudit_randomizationcode).Style.Add("display", "")
                e.Row.Cells(GVAudit_DosageForm).Style.Add("display", "none")
                e.Row.Cells(GVAudit_Storage).Style.Add("display", "none")
                e.Row.Cells(GVAudit_Streangth).Style.Add("display", "none")
                e.Row.Cells(GVAudit_ExpDate).Style.Add("display", "none")

            End If
        End If
    End Sub

    Protected Sub GVAudit_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVAudit.RowDataBound

        Dim ModifyOn As DateTime

        If e.Row.RowType = DataControlRowType.DataRow Then
            If Not e.Row.Cells(GVAudit_ModifyOn).Text.Trim() = "" Then
                ModifyOn = CType(e.Row.Cells(GVAudit_ModifyOn).Text.Trim, Date)
                e.Row.Cells(GVAudit_ModifyOn).Text = ModifyOn.ToString("dd-MMM-yyyy HH:mm") + strServerOffset
            End If
        End If

    End Sub

    Protected Sub grdMultidose_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdMultidose.RowDataBound
        Dim i As Integer = 0
        Dim d As Decimal = 0
        Dim str As String = String.Empty
        Dim b As Boolean = True
        Dim dt As DataTable = Nothing

        Try
            Select Case e.Row.RowType
                Case DataControlRowType.DataRow
                    For Each cell As DataControlFieldCell In e.Row.Cells
                        If e.Row.Cells(0).Text = "Total" Then
                            cell.CssClass = "center"
                            cell.Font.Bold = True
                            cell.Enabled = False
                        Else
                            cell.CssClass = "center"
                        End If
                    Next cell
            End Select
        Catch ex As Exception
            ShowErrorMessage(ex.Message.ToString, "Error While Multidose Data Bind.")
        End Try
    End Sub

#End Region

#Region "Text box change event"
    Protected Sub txtreplaceCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtreplaceCode.TextChanged
        Dim wStr As String = String.Empty
        Dim ds As New DataSet
        Try
            wStr = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "' And "
            wStr += " iPeriod=" & Me.ddlPeriod.SelectedValue.Trim() & " and "
            wStr += " vSubjectID = '" + Me.txtreplaceCode.Text.Trim + "'"
            wStr += " And cRejectionFlag <> 'Y'"

            If Not objHelp.GetViewWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                Me.ShowErrorMessage("Error While Getting Data From ViewWorkSpaceSubjectMst", eStr_Retu)
                Exit Sub
            End If

            If ds.Tables(0).Rows.Count <= 0 Then

                Me.ObjCommon.ShowAlert("Subject Is Not Valid For This Project.", Me.Page())
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + _
                                                       Me.divReplacement.ClientID.ToString.Trim() + "');", True)
                Exit Sub

            End If

            If ds.Tables(0).Rows(0)("iMySubjectNo").ToString().Trim() = "" Then

                Me.ObjCommon.ShowAlert("Subject No. Is Not Assigned To This Subject.", Me.Page())
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + _
                                                       Me.divReplacement.ClientID.ToString.Trim() + "');", True)
                Exit Sub

            End If


            Me.txtreplaceCode.Text = ds.Tables(0).Rows(0)("iMySubjectNo").ToString()

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + _
                                                        Me.divReplacement.ClientID.ToString.Trim() + "');", True)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....txtreplaceCode_TextChanged")
        End Try
    End Sub

    Protected Sub txtTotaldoses_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTotaldoses.TextChanged

        If Me.txtTotaldoses.Text.ToString.Trim() <> "" Then
            If CType(IIf(Me.txtTotaldays.Text.ToString.Trim() = "", 0, Me.txtTotaldays.Text.ToString.Trim()), Integer) > CType(IIf(Me.txtTotaldoses.Text.ToString.Trim() = "", 0, Me.txtTotaldoses.Text.ToString.Trim()), Integer) Then
                ObjCommon.ShowAlert("Total number of doses should be equal to or greater than the total number of days.", Me.Page)
                Me.txtTotaldoses.Text = ""
                mpeMultidose.Show()
                Exit Sub
            End If

            SetInitialRow()
            mpeMultidose.Show()
        Else
            btnCancelMultidose_Click(sender, e)
        End If

    End Sub
#End Region

#Region "Checkbox check event"
    Private Sub chkMultidose_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkMultidose.CheckedChanged
        If Me.chkMultidose.Checked = True Then
            Me.txtTotaldays.Text = ""
            Me.txtTotaldoses.Text = ""
            Me.chkDeletebarcode.Enabled = False
            Me.grdMultidose.DataSource = Nothing
            Me.grdMultidose.DataBind()
            mpeMultidose.Show()
        Else
            Me.chkMultidose.Enabled = True
            Me.chkDeletebarcode.Enabled = True
        End If
    End Sub

    Protected Sub chkDeletebarcode_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDeletebarcode.CheckedChanged
        If Me.chkDeletebarcode.Checked = True Then
            Me.txtBlanks.Enabled = False
            Me.chkMultidose.Enabled = False
            Me.txtdays.Text = ""
            Me.txtdoses.Text = ""
            mpeDeletedLabels.Show()
        Else
            Me.txtBlanks.Enabled = True
            Me.chkMultidose.Enabled = True
            Me.chkDeletebarcode.Enabled = True
        End If
    End Sub

    Private Function CheckLastSubjectForDosingDetail() As Boolean
        Dim wStr As String = String.Empty
        Dim dsAssignment As New DataSet

        If Me.HParentProject.Value.Trim() = HProjectId.Value.Trim() Then
            wStr = "vParentWorkspaceId='" + Me.HParentProject.Value.Trim() + "' and cStatusindi<>'D' and vWorkSpaceId <='" + HProjectId.Value + "'And  left(imysubjectno,4) < 2000 AND cIsTestSite<>'Y'"
        Else
            wStr = "vParentWorkspaceId='" + Me.HParentProject.Value.Trim() + "' and cStatusindi<>'D' and vWorkSpaceId <'" + HProjectId.Value + "'And  left(imysubjectno,4) < 2000 AND cIsTestSite<>'Y'"
        End If

        If Not objHelp.GetData("View_WorkSpaceSubjectMstForiMySubjectNo", "isnull(max(left(imysubjectno,4)),0) AS SubjectNo", wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsAssignment, eStr_Retu) Then
            Throw New Exception(eStr_Retu)
        End If

        If Me.HParentProject.Value.Trim() = HProjectId.Value.Trim() Then
            Me.txtSubjectInitial.Text = 1001
        Else
            If dsAssignment.Tables(0).Rows(0)("SubjectNo") = 0 Then
                Me.txtSubjectInitial.Text = 1001
            Else
                Me.txtSubjectInitial.Text = dsAssignment.Tables(0).Rows(0).Item("SubjectNo").ToString + 1
            End If
        End If
    End Function

#End Region

#Region "Dynamic Row generation"
    Public Sub SetInitialRow()
        Dim dt As DataTable = New DataTable()
        Dim dr As DataRow = Nothing
        Dim i As Integer
        Dim Dose As Integer = 0
        Dim TotDose As Integer = 0

        dt.Columns.Add(New DataColumn("Days"))
        dt.Columns.Add(New DataColumn("Doses"))

        Dose = (CType(Me.txtTotaldoses.Text.Trim, Integer) / CType(Me.txtTotaldays.Text.Trim, Integer))
        If CType(Me.txtTotaldoses.Text.Trim, Integer) Mod CType(Me.txtTotaldays.Text.Trim, Integer) = 0 Then
            For i = 0 To CType(Me.txtTotaldays.Text.Trim, Integer)
                dr = dt.NewRow()
                If i = CType(Me.txtTotaldays.Text.Trim, Integer) Then
                    dr("Days") = "Total"
                    dr("Doses") = TotDose
                Else
                    dr("Days") = i + 1
                    dr("Doses") = Dose
                    TotDose = TotDose + Dose
                End If
                dt.Rows.Add(dr)
            Next
        Else
            For i = 0 To CType(Me.txtTotaldays.Text.Trim, Integer)
                dr = dt.NewRow()
                If i = CType(Me.txtTotaldays.Text.Trim, Integer) Then
                    dr("Days") = "Total"
                    dr("Doses") = TotDose
                Else
                    dr("Days") = i + 1
                    dr("Doses") = String.Empty
                    TotDose = CType(Me.txtTotaldoses.Text.Trim, Integer)
                End If
                dt.Rows.Add(dr)
            Next
        End If

        grdMultidose.DataSource = dt
        grdMultidose.DataBind()
    End Sub
#End Region

#Region "Project Status Check"
    Protected Function CheckProjectStatus() As Boolean

        Dim wstr As String = String.Empty
        Dim ds_CheckStatus As DataSet = New DataSet
        Dim eStr As String = String.Empty
        Dim dv_Check As DataView

        wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "'"

        If Not Me.objHelp.GetCRFLockDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_CheckStatus, eStr) Then
            Throw New Exception(eStr)
        End If
        If Not ds_CheckStatus Is Nothing Then

            dv_Check = ds_CheckStatus.Tables(0).DefaultView
            dv_Check.Sort = "iTranNo desc"

            If dv_Check.ToTable().Rows.Count > 0 Then

                If Convert.ToString(dv_Check.ToTable().Rows(0)("cLockFlag")).Trim.ToUpper() = "L" Then
                    Me.ObjCommon.ShowAlert("Project is Locked. You Can not generate IP Labels.", Me.Page)
                    Return True
                End If
            End If
        End If

        Return False
    End Function
#End Region

#Region "Drop down chhange event"

    Protected Sub ddlPeriod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPeriod.SelectedIndexChanged

        If Me.ddlPeriod.SelectedIndex > 0 Then

            If Not FillGrid() Then
                Me.ObjCommon.ShowAlert("Error While Filling Grid", Me.Page())
                Exit Sub
            End If


            Me.BtnDelete1.Visible = True
            Me.BtnDelete2.Visible = True


            If Me.btnSave.Visible = False Then
                Me.BtnDelete1.Visible = False
                Me.BtnDelete2.Visible = False
            End If

            If Not Me.chkdoubleblinded.Checked = True Then
                If Not FillProductType() Then
                    ObjCommon.ShowAlert("Unable to get ProductType", Me)
                End If
            End If


            If Me.gvwDosingDetail.Rows.Count <= 0 Then
                Me.BtnDelete1.Visible = False
                Me.BtnDelete2.Visible = False

            End If
        End If

    End Sub

    Protected Sub ddlProductType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProductType.SelectedIndexChanged
        Dim Str As String = String.Empty
        Dim ds_DosingLabelData As New DataSet

        If Me.HIsTestSite.Value = "" Or Me.HIsTestSite.Value = "N" Then
            CheckLastSubjectForDosingDetail()
        End If

        Str = Me.HProjectId.Value.ToString.Trim + "##" + CType(Me.ddlPeriod.SelectedValue - 1, Integer).ToString.Trim + "##" + Me.ddlProductType.SelectedItem.Text.ToString.ToString.Trim()

        ds_DosingLabelData = objHelp.ProcedureExecute("Proc_GetProductTypeWiseDosingLabelsDetail", Str)

        FillDrugDetails()
        FillBatchNo()

        If Not IsNothing(ds_DosingLabelData) AndAlso ds_DosingLabelData.Tables(0).Rows.Count > 0 Then
            ddlDrug.SelectedIndex = ddlDrug.Items.IndexOf(ddlDrug.Items.FindByValue(Convert.ToString(ds_DosingLabelData.Tables(0).Rows(0)("nProductNo")).Trim()))
            ddlBatchno.SelectedIndex = ddlBatchno.Items.IndexOf(ddlBatchno.Items.FindByValue(Convert.ToString(ds_DosingLabelData.Tables(0).Rows(0)("nBatchNo")).Trim()))
            Me.txtstrength.Text = ds_DosingLabelData.Tables(0).Rows(0)("vStrength").ToString.Trim()
            Me.txtDosage.Text = ds_DosingLabelData.Tables(0).Rows(0)("vdosageform").ToString.Trim()
            Me.txtprotocol.Text = ds_DosingLabelData.Tables(0).Rows(0)("vProtocolNo").ToString.Trim()
            Me.txtExpiryDate.Text = ds_DosingLabelData.Tables(0).Rows(0)("dExpiryDate").ToString.Trim()
            Me.txtInstruction.Text = ds_DosingLabelData.Tables(0).Rows(0)("vInstruction").ToString.Trim()
            Me.txtcondition.Text = ds_DosingLabelData.Tables(0).Rows(0)("vStoragecondition").ToString.Trim()
            Me.txtRouteAdmin.Text = ds_DosingLabelData.Tables(0).Rows(0)("vRouteAdmin").ToString.Trim()
            Me.txtDispenseQtyPerSubject.Text = ds_DosingLabelData.Tables(0).Rows(0)("iDispenseQtyPerSubject").ToString.Trim()
        Else
            Me.txtstrength.Text = ""
            Me.txtDosage.Text = ""
            Me.txtprotocol.Text = ""
            Me.txtExpiryDate.Text = ""
            Me.txtInstruction.Text = ""
            Me.txtcondition.Text = ""
            Me.txtRouteAdmin.Text = ""
            Me.txtDispenseQtyPerSubject.Text = ""
        End If

    End Sub

    Protected Sub ddlDrug_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDrug.SelectedIndexChanged
        FillBatchNo()
    End Sub

#End Region


    Private Function FillGridForPMSData() As Boolean
        Dim dsPeriod As New DataSet
        Dim dvPeriod As New DataView
        Dim WorkspaceId As String = String.Empty
        Dim wstr As String = String.Empty

        Try
            WorkspaceId = Me.HProjectId.Value.Trim()
            Me.gvwDosingDetail.DataSource = Nothing
            Me.gvwDosingDetail.DataBind()
            Me.BtnDelete1.Visible = False
            Me.BtnDelete2.Visible = False
            Me.BtnPrint1.Visible = False
            Me.BtnPrint2.Visible = False

            wstr = "iPeriod=" & Me.ddlPeriod.SelectedValue.Trim() & " AND vWorkSpaceId='" & WorkspaceId & "'  AND nProductTypeID =" & Me.ddlProductType.SelectedValue.Trim() & " AND nProductNo= " & Me.ddlDrug.SelectedValue.Trim() & " AND nBatchNo=" & Me.ddlBatchno.SelectedValue.Trim() & " AND cstatusindi <>'D' "

            If Not objHelp.View_DosingDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                         dsPeriod, eStr_Retu) Then
                ShowErrorMessage(eStr_Retu, "")
                Exit Function
            End If


            If dsPeriod.Tables(0).Rows.Count > 0 Then
                dvPeriod = dsPeriod.Tables(0).DefaultView()
                dvPeriod.Sort = "nDosingDetailNo"
                Me.gvwDosingDetail.DataSource = dvPeriod.ToTable()
                Me.gvwDosingDetail.DataBind()
                Me.ViewState(VS_DosingDetail) = dvPeriod.ToTable()
                Me.BtnDelete1.Visible = True
                Me.BtnDelete2.Visible = True
                Me.divnote.Style.Add("Display", "")
                Me.PanelProjectSpecific.Style.Add("display", "")
                Me.pnlgvmedexworkspadce.Style.Add("display", "")
            Else
                Me.PanelProjectSpecific.Style.Add("display", "none")
                Me.pnlgvmedexworkspadce.Style.Add("display", "none")
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....FillGrid")
            Return False
        End Try
    End Function

    Protected Sub ddlBatchno_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlBatchno.SelectedIndexChanged

        Dim WorkspaceId As String = String.Empty
        Dim DsWorkspace As New DataSet
        Dim wStr As String = String.Empty
        Dim dsDrug As New DataSet

        Dim ProductType As String = String.Empty
        Dim Drug As String = String.Empty

        Try
            If Me.ddlProductType.SelectedValue.ToString.Trim() = "" Then
                ProductType = "0"
            Else
                ProductType = Me.ddlProductType.SelectedValue.ToString.Trim()
            End If
            If Me.ddlDrug.SelectedValue.ToString.Trim() = "" Then
                Drug = "0"
            Else
                Drug = Me.ddlDrug.SelectedValue.ToString.Trim()
            End If

            wStr = Me.HProjectId.Value.ToString.Trim + "##" + ProductType + "##" + Drug + "##" + Convert.ToString(Me.ddlBatchno.SelectedValue).Trim()

            dsDrug = Me.objHelp.ProcedureExecute("[dbo].[Proc_GetProductDtl]", wStr)


            If Not IsNothing(dsDrug) AndAlso dsDrug.Tables(0).Rows.Count > 0 Then
                Me.txtstrength.Text = Convert.ToString(dsDrug.Tables(0).Rows(0)("vProductStrength")).Trim()
                Me.txtExpiryDate.Text = Convert.ToString(dsDrug.Tables(0).Rows(0)("dExpDate")).Trim()
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillDrugDetails")

        End Try
    End Sub

#Region "Web Method"
    <WebMethod> _
    Public Shared Function GetRandomizationDetailCount(ByVal vWorkSpaceId As String, ByVal iPeriod As String, ByVal vProductType As String, ByVal iMySubjectNoStart As String, ByVal iMySubjectNoEnd As String, ByVal nProductNo As String, ByVal nBatchNo As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
        Dim ds_RandomizationDetail As DataSet = New DataSet
        Dim dt_RandomizationDetail As DataTable = New DataTable
        Dim wStr As String = String.Empty
        Dim estr As String = String.Empty

        Try

            'wStr = Convert.ToString(vWorkSpaceId) + "##" + Convert.ToInt32(iPeriod) + "##" + Convert.ToString(vProductType) + "##" + Convert.ToInt32(iMySubjectNoStart) + "##" + Convert.ToInt32(iMySubjectNoEnd)
            wStr = Convert.ToString(vWorkSpaceId) + "##" + Convert.ToString(iPeriod) + "##" + Convert.ToString(vProductType) + "##" + Convert.ToString(iMySubjectNoStart) + "##" + Convert.ToString(iMySubjectNoEnd) + "##" + Convert.ToString(nProductNo) + "##" + Convert.ToString(nBatchNo)

            ds_RandomizationDetail = objHelp.ProcedureExecute("dbo.Proc_GetRandomizationDetailCount", wStr)


            Return JsonConvert.SerializeObject(ds_RandomizationDetail)

        Catch ex As Exception
            Throw New Exception("Error while Insert data")
            Return "0"
        End Try

    End Function

#End Region
End Class

