
Partial Class frmExpense
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION "

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtGetOtherExpHdr As String = "OtherExpHdr"
    Private Const VS_DtGetOtherExpDtl As String = "OtherExpDtl"
    Private Const VS_DtExpenseGV As String = "Expense" ' gvwExpense

    Private amt As Decimal = 0

    Private Const GVC_HdrNo As Integer = 0
    Private Const GVC_DtlNo As Integer = 1
    Private Const GVC_FromDate As Integer = 2
    Private Const GVC_ToDate As Integer = 3
    Private Const GVC_SiteNo As Integer = 4
    Private Const GVC_Site As Integer = 5
    Private Const GVC_ExpenseTypeNo As Integer = 6
    Private Const GVC_ExpenseType As Integer = 7
    Private Const GVC_ExpenseAmount As Integer = 8
    Private Const GVC_Remarks As Integer = 9
    Private Const GVC_RefDetail As Integer = 10
    Private Const GVC_AttachmentFullName As Integer = 11

    Private Const GVCExpAdd_FromDate As Integer = 0
    Private Const GVCExpAdd_ToDate As Integer = 1
    Private Const GVCExpAdd_TotalExpAmt As Integer = 2
    Private Const GVCExpAdd_ApprovalFlag As Integer = 3

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

            Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add

            Me.ViewState(VS_Choice) = Choice   'To use it while saving

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

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
    Private Function GenCall_Data(ByRef ds_DWR_Retu As DataSet) As Boolean

        Dim eStr_Retu As String = ""
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Choice = Me.ViewState(VS_Choice)

            If Not objHelp.GetOtherExpHdr("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                                  ds, eStr_Retu) Then

                Response.Write(eStr_Retu)
                Exit Function

            End If

            Me.ViewState(VS_DtGetOtherExpHdr) = ds.Tables(0)
            ds = Nothing

            If Not objHelp.GetOtherExpDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                                  ds, eStr_Retu) Then

                Response.Write(eStr_Retu)
                Exit Function

            End If

            Me.ViewState(VS_DtGetOtherExpDtl) = ds.Tables(0)

            ds_DWR_Retu = ds
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")

        Finally

        End Try
    End Function
#End Region

#Region "GenCall_showUI "

    Private Function GenCall_ShowUI() As Boolean
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try
            Page.Title = " :: Expense ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Expense Detail"
            Choice = Me.ViewState(VS_Choice)

            Me.txtExpenseFromDate.Text = Today.Now.Date.ToString("dd-MMM-yyyy")
            Me.txtExpenseToDate.Text = Today.Now.Date.ToString("dd-MMM-yyyy")

            If Not FillGridgvwExpenseAdded() Then
                Return False
            End If

            Me.tblExpense.Visible = False
            Me.btnSave.Visible = False

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
            End If

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function

#End Region

#Region "FillDropDowns"

    Private Function FillDropDownddlSite() As Boolean
        Dim ds_Site As New Data.DataSet
        Dim estr As String = ""
        Dim wstr As String = ""
        Dim dv_Site As New DataView
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            Me.ddlSite.Items.Clear()

            wstr = "(cast(dReportDate as Datetime) >='" + txtExpenseFromDate.Text.ToString() + "'"
            wstr += " AND cast(dReportDate as Datetime) <='" + txtExpenseToDate.Text.ToString() + "')"
            wstr += " AND iUserId=" & Me.Session(S_UserID)

            If Not objHelp.GetViewUserWiseDWR(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Site, estr) Then

                Me.ObjCommon.ShowAlert("Error while Getting Data from ViewUserWiseDWR", Me.Page())
                Return False

            End If

            Me.tblExpense.Visible = True

            If Not ds_Site.Tables(0).Rows.Count > 0 Then
                ObjCommon.ShowAlert("No Record Found For Site", Me.Page)
                Me.tblExpense.Visible = False
                Return True
            End If

            dv_Site = ds_Site.Tables(0).DefaultView.ToTable(True, "nVisitedSTPNo,vSiteName".Split(",")).DefaultView
            dv_Site.Sort = "vSiteName"
            Me.ddlSite.DataSource = dv_Site
            Me.ddlSite.DataValueField = "nVisitedSTPNo"
            Me.ddlSite.DataTextField = "vSiteName"
            Me.ddlSite.DataBind()
            Me.ddlSite.Items.Insert(0, New ListItem("--Select Site--", 0))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function FillDropDownddlExpType() As Boolean
        Dim ds_ExpType As New Data.DataSet
        Dim estr As String = ""
        Dim dv_ExpType As New DataView
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Me.ddlExpType.Items.Clear()

            If Not objHelp.GetOtherExpMst("cActiveFlag <> 'N'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                          ds_ExpType, estr) Then

                Me.ObjCommon.ShowAlert("Error while Getting Data from OtherExpMst", Me.Page())
                Return False

            End If

            If Not ds_ExpType.Tables(0).Rows.Count > 0 Then
                ObjCommon.ShowAlert("No Record Found For Expense Type", Me.Page)
                Return True
            End If

            dv_ExpType = ds_ExpType.Tables(0).DefaultView
            dv_ExpType.Sort = "vOtherExpName"
            Me.ddlExpType.DataSource = dv_ExpType
            Me.ddlExpType.DataValueField = "nOtherExpMstNo"
            Me.ddlExpType.DataTextField = "vOtherExpName"
            Me.ddlExpType.DataBind()
            Me.ddlExpType.Items.Insert(0, New ListItem("--Select Expense Type--", 0))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function
#End Region

#Region "FillGrid"

    Private Function FillGrid() As Boolean
        Dim ds_Expense As New Data.DataSet
        Dim estr As String = ""
        Dim wstr As String = ""
        Try
            wstr = "(dFromDate = '" + txtExpenseFromDate.Text.ToString() + "'"
            wstr += " And dToDate = '" + txtExpenseToDate.Text.ToString() + "')"
            wstr += " AND iUserId=" & Me.Session(S_UserID)
            wstr += " And cActiveFlag = 'Y'"

            If Not objHelp.GetViewUserWiseExpense(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                 ds_Expense, estr) Then

                Me.ObjCommon.ShowAlert("Error while Getting Data from ViewUserWiseExpense", Me.Page())
                Return False

            End If

            Me.gvwExpense.DataSource = ds_Expense.Tables(0)
            Me.gvwExpense.DataBind()

            Me.ViewState(VS_DtExpenseGV) = ds_Expense.Tables(0)

            Me.btnSave.Visible = False
            Me.gvwExpense.Visible = False

            If Me.gvwExpense.Rows.Count > 0 Then

                Me.btnSave.Visible = True
                Me.gvwExpense.Visible = True

            End If

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function FillGridgvwExpenseAdded() As Boolean
        Dim ds_ExpenseAdded As New Data.DataSet
        Dim estr As String = ""
        Dim wstr As String = ""
        Try

            wstr = "(month(dFromDate) = month(getdate())-1 or "
            wstr += "(month(dToDate) = month(getdate()) and dToDate <= getdate())) AND iUserId = " & Me.Session(S_UserID)

            If Not objHelp.GetOtherExpHdr(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                          ds_ExpenseAdded, estr) Then

                Me.ObjCommon.ShowAlert("Error while Getting Data from OtherExpHdr", Me.Page())
                Return False

            End If

            Me.gvwExpenseAdded.DataSource = ds_ExpenseAdded.Tables(0)
            Me.gvwExpenseAdded.DataBind()

            Me.gvwExpenseAdded.Visible = False
            Me.btnSave.Visible = False

            If Me.gvwExpenseAdded.Rows.Count > 0 Then
                Me.gvwExpenseAdded.Visible = True
                Me.btnSave.Visible = True
            End If

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region

#Region "Button Events"

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds_Expense As New Data.DataSet
        Dim estr As String = ""
        Dim wstr As String = ""
        
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            '********Checking is Added on 07-March-2009*****************
            '********Checking if From Date or To Date is Already exists in Database**********

            wstr = "(((dFromDate >= '" + txtExpenseFromDate.Text.ToString() + "'"
            wstr += " And dFromDate <= '" + txtExpenseToDate.Text.ToString() + "')"
            wstr += " AND dFromDate <> '" + txtExpenseFromDate.Text.ToString() + "')"
            wstr += " or ((dToDate >= '" + txtExpenseFromDate.Text.ToString() + "'"
            wstr += " And dToDate <= '" + txtExpenseToDate.Text.ToString() + "')"
            wstr += " AND dToDate <> '" + txtExpenseToDate.Text.ToString() + "'))"
            wstr += " AND iUserId=" & Me.Session(S_UserID)
            wstr += " And cActiveFlag = 'Y'"

            If Not objHelp.GetViewUserWiseExpense(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                  ds_Expense, estr) Then

                Me.ObjCommon.ShowAlert("Error while Getting Data from ViewUserWiseExpense", Me.Page())
                Exit Sub

            End If

            If ds_Expense.Tables(0).Rows.Count > 0 Then

                Me.ObjCommon.ShowAlert("One of the date from selected range, is already exists", Me.Page())
                Me.resetpage()
                Exit Sub

            End If

            '*******************End Checking***************************

            If Not FillDropDownddlSite() Then
                Exit Sub
            End If

            If Not FillDropDownddlExpType() Then
                Exit Sub
            End If

            If Not FillGrid() Then
                Exit Sub
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Dim dt_DtExpense As New DataTable
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Me.ddlSite.SelectedIndex = 0 Then
                Exit Sub
            End If

            If Me.ddlExpType.SelectedIndex = 0 Then
                Exit Sub
            End If

            AssignValues()
            dt_DtExpense = CType(Me.ViewState(VS_DtExpenseGV), DataTable)

            Me.gvwExpense.DataSource = dt_DtExpense
            Me.gvwExpense.DataBind()

            Me.btnSave.Visible = False
            Me.gvwExpense.Visible = False

            If Me.gvwExpense.Rows.Count > 0 Then
                Me.btnSave.Visible = True
                Me.gvwExpense.Visible = True
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds_save As New DataSet
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim dt_DtOtherExpHdr As New DataTable
        Dim dt_DtOtherExpDtl As New DataTable
        Dim dt_DtExpense As New DataTable
        Dim estr_Retu As String = ""
        Dim wstr As String = ""
        Dim TotalExp As Decimal
        Try
            If Me.gvwExpense.Rows.Count > 0 Then

                dt_DtExpense = CType(Me.ViewState(VS_DtExpenseGV), DataTable)
                dt_DtOtherExpHdr = Me.ViewState(VS_DtGetOtherExpHdr)
                dt_DtOtherExpDtl = Me.ViewState(VS_DtGetOtherExpDtl)

                If dt_DtOtherExpHdr.Rows.Count < 1 Then

                    ObjCommon.ShowAlert("No New Records To Save", Me.Page)
                    Exit Sub

                End If

                TotalExp = dt_DtExpense.Compute("Sum(iExpAmt)", "1=1")

                For Each dr In dt_DtOtherExpHdr.Rows

                    dr("iTotalExpAmt") = TotalExp

                Next dr

                dt_DtOtherExpHdr.AcceptChanges()

                ds_save = New DataSet
                ds_save.Tables.Add(dt_DtOtherExpHdr.Copy())
                ds_save.Tables(0).TableName = "OtherExpHdr"

                ds_save.Tables.Add(dt_DtOtherExpDtl.Copy())
                ds_save.Tables(1).TableName = "OtherExpDtl"

                If Not objLambda.Save_OtherExpHdr(Me.ViewState(VS_Choice), ds_save, Me.Session(S_UserID), estr_Retu) Then

                    ObjCommon.ShowAlert("Error While Saving Expense Details", Me.Page)
                    Exit Sub

                End If

                ObjCommon.ShowAlert("Expense Details Saved SuccessFully", Me.Page)
                Me.resetpage()

            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.resetpage()
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

#End Region

#Region "Assign Values"

    Private Sub AssignValues()
        Dim dr As DataRow
        Dim dt_DtExpense As New DataTable

        Dim dt_DtOtherExpHdr As New DataTable
        Dim dt_DtOtherExpDtl As New DataTable

        Dim estr_Retu As String = ""
        Dim wstr As String = ""
        Dim totalAmount As Double = 0
        Dim IndexExpense As Integer = 0
        Dim strPath As String = ""
        Dim di As DirectoryInfo
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            strPath = System.Web.HttpContext.Current.Server.MapPath("DocMgmtLambda/Expence/" + _
                     Me.txtExpenseFromDate.Text.Substring(3, 3) + "/" + _
                     Me.ddlSite.SelectedItem.Value.ToString() + "/" + _
                     Me.Session(S_UserID) + "/")


            dt_DtExpense = CType(Me.ViewState(VS_DtExpenseGV), DataTable)
            dt_DtOtherExpHdr = CType(Me.ViewState(VS_DtGetOtherExpHdr), DataTable)
            dt_DtOtherExpDtl = CType(Me.ViewState(VS_DtGetOtherExpDtl), DataTable)

            'Adding datarow to dt_DtExpense data table for Grid

            dr = dt_DtExpense.NewRow()
            dr("nOtherExpHdrNo") = 0

            dr("nOtherExpDtlNo") = dt_DtOtherExpDtl.Rows.Count - 999

            If dt_DtOtherExpDtl.Rows.Count > 1 Then

                dr("nOtherExpDtlNo") = dt_DtOtherExpDtl.Compute("Max(nOtherExpDtlNo)", "1=1") + 1

            End If

            dr("dFromDate") = Me.txtExpenseFromDate.Text.ToString()
            dr("dToDate") = Me.txtExpenseToDate.Text.ToString()
            dr("nSTPNo") = Me.ddlSite.SelectedItem.Value.ToString()
            dr("vSiteName") = Me.ddlSite.SelectedItem.Text.ToString()
            dr("nOtherExpMstNo") = Me.ddlExpType.SelectedItem.Value.ToString()
            dr("vOtherExpName") = Me.ddlExpType.SelectedItem.Text.ToString()
            dr("iExpAmt") = Me.txtExpAmount.Text.ToString()
            dr("vRemarks") = Me.txtRemarks.Text.ToString()
            dr("vRefDetail") = Me.txtReferenceDetail.Text.ToString()

            If Me.FlAttachment.FileName.Trim() <> "" Then

                dr("vAttachment") = "DocMgmtLambda/Expence/" + Me.txtExpenseFromDate.Text.Substring(3, 3) + "/" + _
                                     Me.ddlSite.SelectedItem.Value.ToString() + "/" + Me.Session(S_UserID) + "/" + _
                                     Path.GetFileName(Me.FlAttachment.PostedFile.FileName)

            End If

            dt_DtExpense.Rows.Add(dr)
            dt_DtExpense.AcceptChanges()

            Me.ViewState(VS_DtExpenseGV) = dt_DtExpense

            'Adding datarow to dt_DtOtherExpHdr data table

            dt_DtExpense = CType(Me.ViewState(VS_DtExpenseGV), DataTable)

            For IndexExpense = 0 To dt_DtExpense.Rows.Count - 1

                totalAmount = totalAmount + dt_DtExpense.Rows(IndexExpense).Item("iExpAmt").ToString()

            Next IndexExpense

            If dt_DtOtherExpHdr.Rows.Count < 1 Then

                dr = dt_DtOtherExpHdr.NewRow()
                'nOtherExpHdrNo,iUserId,dDate,iTotalExpAmt,vRemarks,iApprovalUserId,cApprovalFlag,dApprovalDate,cActiveFlag,
                'iModifyBY, dModifyOn, cReplicaFlag

                dr("nOtherExpHdrNo") = 0
                dr("iUserId") = Me.Session(S_UserID)
                dr("iTotalExpAmt") = totalAmount
                dr("dFromDate") = Me.txtExpenseFromDate.Text.ToString()
                dr("dToDate") = Me.txtExpenseToDate.Text.ToString()
                'dr("vRemarks") = dt_DtExpense.Rows(IndexExpense)'Item("Remarks").ToString()
                'dr("iApprovalUserId") = Me.Session(S_UserID)
                'dr("cApprovalFlag") = ""
                'dr("dApprovalDate") = ""
                dr("cActiveFlag") = "Y"
                dr("iModifyBY") = Me.Session(S_UserID)
                dr("dModifyOn") = Today.Date.Now.ToString("dd-MMM-yy")
                dr("cReplicaFlag") = "N"
                dt_DtOtherExpHdr.Rows.Add(dr)

            Else

                For Each dr In dt_DtOtherExpHdr.Rows
                    dr("iTotalExpAmt") = totalAmount
                Next dr

            End If
            dt_DtOtherExpHdr.AcceptChanges()

            'Adding datarow to dt_DtOtherExpDtl data table
            'nOtherExpDtlNo, nOtherExpHdrNo, iSrNo, nSTPNo, nOtherExpMstNo, iExpAmt, vRemarks, vRefDetail, 
            'vAttachment, iApprovalUserId, cApprovalFlag, dApprovalDate, cActiveFlag, iModifyBY, dModifyOn, cReplicaFlag

            dr = dt_DtOtherExpDtl.NewRow()
            dr("nOtherExpHdrNo") = 0

            dr("nOtherExpDtlNo") = dt_DtOtherExpDtl.Rows.Count - 999

            If dt_DtOtherExpDtl.Rows.Count > 1 Then

                dr("nOtherExpDtlNo") = dt_DtOtherExpDtl.Compute("Max(nOtherExpDtlNo)", "1=1") + 1

            End If

            dr("iSrNo") = "1" 'Temporary passed 1
            dr("nSTPNo") = Me.ddlSite.SelectedValue.Trim()
            dr("nOtherExpMstNo") = Me.ddlExpType.SelectedValue.Trim()
            dr("iExpAmt") = Me.txtExpAmount.Text.Trim()
            dr("vRemarks") = Me.txtRemarks.Text.Trim()
            dr("vRefDetail") = Me.txtReferenceDetail.Text.Trim()

            If Not Me.FlAttachment.FileName = "" Then
                'If Not Me.FlAttachment.PostedFile.FileName = "" Then
                dr("vAttachment") = "DocMgmtLambda/Expence/" + Me.txtExpenseFromDate.Text.Substring(3, 3) + "/" + _
                             Me.ddlSite.SelectedItem.Value.ToString() + "/" + Me.Session(S_UserID) + "/" + _
                             Path.GetFileName(Me.FlAttachment.PostedFile.FileName)
            End If

            'dr("iApprovalUserId") = ""
            'dr("cApprovalFlag") = ""
            'dr("dApprovalDate") = ""
            dr("cActiveFlag") = "Y"
            dr("iModifyBY") = Me.Session(S_UserID)
            dr("dModifyOn") = Today.Date.Now.ToString("dd-MMM-yy")
            dr("cReplicaFlag") = "N"
            dt_DtOtherExpDtl.Rows.Add(dr)
            dt_DtOtherExpDtl.AcceptChanges()

            Me.ViewState(VS_DtGetOtherExpHdr) = dt_DtOtherExpHdr
            Me.ViewState(VS_DtGetOtherExpDtl) = dt_DtOtherExpDtl

            'File Creation**************

            If Me.FlAttachment.FileName.Trim() <> "" Then

                di = New DirectoryInfo(strPath)

                If Not di.Exists() Then
                    Directory.CreateDirectory(strPath)
                End If

                Me.FlAttachment.SaveAs(strPath + Path.GetFileName(Me.FlAttachment.PostedFile.FileName))

            End If

            'End **********************

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

#End Region

#Region "ResetPage"

    Protected Sub resetpage()

        Me.ViewState(VS_Choice) = Nothing
        Me.ViewState(VS_DtExpenseGV) = Nothing
        Me.ViewState(VS_DtGetOtherExpHdr) = Nothing
        Me.ViewState(VS_DtGetOtherExpDtl) = Nothing

        Me.txtExpenseFromDate.Text = ""
        Me.txtExpenseToDate.Text = ""
        Me.txtExpAmount.Text = ""
        Me.txtReferenceDetail.Text = ""
        Me.txtRemarks.Text = ""

        Me.ddlSite.DataSource = Nothing
        Me.ddlSite.DataBind()
        Me.ddlExpType.DataSource = Nothing
        Me.ddlExpType.DataBind()
        Me.gvwExpense.DataSource = Nothing
        Me.gvwExpense.DataBind()

        GenCall()
    End Sub

#End Region

#Region "Grid Events"

    Protected Sub gvwExpense_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        e.Row.Cells(GVC_HdrNo).Visible = False
        e.Row.Cells(GVC_DtlNo).Visible = False
        e.Row.Cells(GVC_SiteNo).Visible = False
        e.Row.Cells(GVC_ExpenseTypeNo).Visible = False

    End Sub

    Protected Sub gvwExpense_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvwExpense.RowDeleting
        Dim dtExpenseGV As DataTable
        Dim dtOtherExpDtl As DataTable
        Dim dtOtherExpHdr As DataTable
        Dim dr As DataRow
        Dim dsOtherExpHdr As New DataSet
        Dim dsOtherExpDtl As New DataSet
        Dim dsOtherExpDelete As New DataSet
        Dim Estr As String = ""
        Dim Wstr As String = ""
        Dim index As Integer
        Dim DeleteHdr As Boolean = False
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            'Deleting From All Tables
            dtOtherExpHdr = CType(Me.ViewState(VS_DtGetOtherExpHdr), DataTable).Copy()
            dtOtherExpDtl = CType(Me.ViewState(VS_DtGetOtherExpDtl), DataTable).Copy()
            dtExpenseGV = CType(Me.ViewState(VS_DtExpenseGV), DataTable)

            'For deleting Existing Data

            If Me.gvwExpense.Rows(e.RowIndex).Cells(GVC_HdrNo).Text.Trim() > 0 AndAlso Me.gvwExpense.Rows(e.RowIndex).Cells(GVC_DtlNo).Text.Trim() > 0 Then

                Wstr = "nOtherExpHdrNo=" & Me.gvwExpense.Rows(e.RowIndex).Cells(GVC_HdrNo).Text.Trim()
                If Not Me.objHelp.GetOtherExpHdr(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                dsOtherExpHdr, Estr) Then
                    Me.ObjCommon.ShowAlert("Error while Getting Data from OtherExpHdr", Me.Page())
                    Exit Sub
                End If

                If Not Me.objHelp.GetOtherExpDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                dsOtherExpDtl, Estr) Then
                    Me.ObjCommon.ShowAlert("Error while Getting Data from OtherExpDtl", Me.Page())
                    Exit Sub
                End If

                dsOtherExpDtl.Tables(0).DefaultView.RowFilter = "nOtherExpDtlNo=" & Me.gvwExpense.Rows(e.RowIndex).Cells(GVC_DtlNo).Text.Trim()
                dsOtherExpDelete.Tables.Add(dsOtherExpDtl.Tables(0).DefaultView.ToTable().Copy())

                'To Substract deleted Expense

                For Each dr In dsOtherExpHdr.Tables(0).Rows
                    dr("iTotalExpAmt") = dr("iTotalExpAmt") - dsOtherExpDtl.Tables(0).DefaultView.ToTable().Rows(0).Item("iExpAmt")
                Next dr

                dsOtherExpHdr.Tables(0).AcceptChanges()
                '***********************

                If Me.objLambda.Save_OtherExpDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete, dsOtherExpDelete, _
                                                Me.Session(S_UserID), Estr) Then

                    If Me.objLambda.Save_OtherExpHdr(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, dsOtherExpHdr, _
                                                         Me.Session(S_UserID), Estr) Then

                        Me.ObjCommon.ShowAlert("Expense Successfuly Deleted", Me.Page())

                    End If

                End If

            Else ' If Newly Added then

                '**********************
                'Deleteing from DWRDetail Data table dtDWRDetail
                For index = 0 To dtOtherExpDtl.Rows.Count - 1
                    If Me.gvwExpense.Rows(e.RowIndex).Cells(GVC_DtlNo).Text.Trim() = dtOtherExpDtl.Rows(index).Item("nOtherExpDtlNo").ToString.Trim() Then

                        'To Substract deleted Expense
                        For Each dr In dtOtherExpHdr.Rows
                            dr("iTotalExpAmt") = dr("iTotalExpAmt") - dtOtherExpDtl.Rows(index).Item("iExpAmt")
                        Next
                        dtOtherExpHdr.AcceptChanges()
                        '***********************

                        dtOtherExpDtl.Rows(index).Delete()
                        dtOtherExpDtl.AcceptChanges()
                        Exit For

                    End If

                Next index

                'Deleteing from DWRDetail Data table dtDWRHdr
                If dtOtherExpDtl.Rows.Count < 1 Then

                    dtOtherExpHdr.Rows(0).Delete()
                    dtOtherExpHdr.AcceptChanges()

                End If

                Me.ViewState(VS_DtGetOtherExpHdr) = dtOtherExpHdr
                Me.ViewState(VS_DtGetOtherExpDtl) = dtOtherExpDtl

            End If


            'Deleteing from Grid Data table dtDWRDetailGV
            dtExpenseGV.Rows(e.RowIndex).Delete()
            dtExpenseGV.AcceptChanges()

            Me.ViewState(VS_DtExpenseGV) = dtExpenseGV

            Me.gvwExpense.DataSource = dtExpenseGV
            Me.gvwExpense.DataBind()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
            dtOtherExpHdr = Nothing
            dtOtherExpDtl = Nothing
            dtExpenseGV = Nothing
        End Try
    End Sub

    Protected Sub gvwExpense_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwExpense.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            CType(e.Row.FindControl("hlnkFile"), HyperLink).NavigateUrl = CType(e.Row.FindControl("hlnkFile"), HyperLink).Text.Trim()
            CType(e.Row.FindControl("hlnkFile"), HyperLink).Text = Path.GetFileName(CType(e.Row.FindControl("hlnkFile"), HyperLink).Text.Trim())
            amt += Val(e.Row.Cells(GVC_ExpenseAmount).Text.Trim())

        End If

        If e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(GVC_ExpenseAmount).Text = amt

        End If
    End Sub

    Protected Sub gvwExpenseAdded_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwExpenseAdded.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or _
                e.Row.RowType = DataControlRowType.Header Or _
                e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVCExpAdd_ApprovalFlag).Visible = False

        End If
    End Sub

    Protected Sub gvwExpenseAdded_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        gvwExpenseAdded.PageIndex = e.NewPageIndex
        If Not FillGridgvwExpenseAdded() Then
            Exit Sub
        End If
    End Sub

    Protected Sub gvwExpenseAdded_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwExpenseAdded.RowDataBound
        Dim index As Integer = e.Row.RowIndex

        If e.Row.RowType = DataControlRowType.DataRow Then

            CType(e.Row.FindControl("lnkbtnEdit"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkbtnEdit"), LinkButton).CommandName = "EDIT"

            If e.Row.Cells(GVCExpAdd_ApprovalFlag).Text.Trim = "A" Or e.Row.Cells(GVCExpAdd_ApprovalFlag).Text.Trim = "R" Then
                CType(e.Row.FindControl("lnkbtnEdit"), LinkButton).Enabled = False
            End If

        End If
    End Sub

    Protected Sub gvwExpenseAdded_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvwExpenseAdded.RowCommand
        Dim index As Integer = e.CommandArgument

        If e.CommandName.ToUpper = "EDIT" Then

            Me.txtExpenseFromDate.Text = Me.gvwExpenseAdded.Rows(index).Cells(GVCExpAdd_FromDate).Text.Trim()
            Me.txtExpenseToDate.Text = Me.gvwExpenseAdded.Rows(index).Cells(GVCExpAdd_ToDate).Text.Trim()
            btnSearch_Click(sender, e)

        End If
    End Sub

    Protected Sub gvwExpenseAdded_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvwExpenseAdded.RowEditing
        'Event is generated only for handling the "Edit"
        e.Cancel = True
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