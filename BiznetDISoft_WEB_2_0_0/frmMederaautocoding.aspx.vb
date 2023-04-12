Imports System.Linq
Imports System.IO
Imports System.Drawing
Partial Class frmMederaautocoding
    Inherits System.Web.UI.Page

#Region "Variable Declaration"
    Private objCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()


    Private Const VS_CRFTermCode As String = "CRFTermCode"

    Private Const GVCSub_Select As Integer = 0
    Private Const GVCSub_vProjectNo As Integer = 1
    Private Const GVCSub_vSubjectId As Integer = 2
    Private Const GVCSub_vMySubjectNo As Integer = 3
    Private Const GVCSub_vRandomizationNo As Integer = 4
    Private Const GVCSub_iRepeatNo As Integer = 5
    Private Const GVCSub_CRFTerm As Integer = 6
    Private Const GVCSub_vMedExResult As Integer = 7
    Private Const GVCSub_vRefTableRemark As Integer = 8
    Private Const GVCSub_vModifyBy As Integer = 9
    Private Const GVCSub_dModifyOn As Integer = 10
    Private Const GVCSub_nCRFSubDtlNo As Integer = 11
    Private Const GVCSub_vProjectTypeCOde As Integer = 12

    Private Const VS_ExportGrid As String = "ExportGrid"
    Private Const Vs_dsReviewerlevel As String = "dsActivitystatusReviewerlevel"
    Private Const Sub_Specific_Activity As Integer = 1
    Private Const Generic_Activity As Integer = 2
    'Private Const Data_Entry_Pending As Integer = 1
    'Private Const Data_Entry_Continue As Integer = 2
    Private Const First_Review_Pending As Integer = 1
    Private Const Second_Review_Pending As Integer = 2
    Private Const Final_Review_Pending As Integer = 3
    Private Const Reviewed_Locked As Integer = 4
    Dim iPeriod As String = String.Empty
    Dim strSubjectId As String = String.Empty
    Dim strParentID As String = String.Empty
    Dim strActivityId As String = String.Empty
    Dim cSubjectWiseFlag As String = String.Empty
    Dim iProjectType As String = 0
    Dim cDataStatus As String = String.Empty
    Dim iWorkflowStageId As String = String.Empty
    Dim cDictionaryType As String = String.Empty
    Dim CRFTerm As String = String.Empty
    Dim cCodingStatus As String = String.Empty
    Dim cActStatus As String = String.Empty

#End Region

#Region "Form Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtproject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(sender, e)
            End If
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)
            CType(Me.Master.FindControl("lblMandatory"), Label).Visible = True
            CType(Me.Master.FindControl("lblHeading"), Label).Text = " Medical Coding "
            Page.Title = ":: Medical Coding :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
        End If
    End Sub
#End Region

#Region "Fill Crf Drop Down List"

    Public Function FillddlDictionary() As Boolean
        Dim Estr As String = String.Empty
        Dim dt_CRF As DataTable = Nothing
        Dim dt_CrfTermMed As DataTable = Nothing
        Try
            dt_CRF = CType(Me.ViewState(VS_CRFTermCode), DataTable).Copy()
            dt_CrfTermMed = dt_CRF.DefaultView.ToTable
            If (ddlCodingStatus.SelectedIndex > 0) Then
                If (ddlCodingStatus.SelectedIndex = 1) Then
                    dt_CrfTermMed.DefaultView().RowFilter = "vMedExType = 'ComboGlobalDictionary'" + "AND CONVERT(Isnull(vMedExResult,''), System.String) <> ''"
                Else
                    dt_CrfTermMed.DefaultView().RowFilter = "vMedExType = 'ComboGlobalDictionary'" + "AND CONVERT(Isnull(vMedExResult,''), System.String) = ''"
                End If
            End If

            dt_CRF = dt_CrfTermMed.DefaultView().ToTable(True, "cDictionaryType")
            dt_CRF.DefaultView.RowFilter = "cDictionaryType is not null"
            Me.trDictionary.Visible = True

            Me.ddlDictionary.DataSource = dt_CRF.DefaultView.ToTable
            Me.ddlDictionary.DataValueField = "cDictionaryType"
            Me.ddlDictionary.DataTextField = "cDictionaryType"
            Me.ddlDictionary.DataBind()
            Me.ddlDictionary.Items.Insert(0, "All")
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("Error While Fill ddlDictionary", Estr)
            Return False
        End Try
    End Function

    Public Function FillAllCRFData() As Boolean
        Dim Estr As String = String.Empty
        Dim Param As String = String.Empty
        Dim Ds_crfterm As DataSet = Nothing
        Dim dt_medera As DataTable = Nothing
        Dim dt_Medra As DataTable = Nothing
        Dim dt_CrfTermMed As DataTable = Nothing
        Dim cVersionStatus As Char

        Try
            cVersionStatus = getCRFVersion()
            If cVersionStatus = "U" Then
                objCommon.ShowAlert("Project Structure Is Not Freeze,To Do Data Entry Freeze It ", Me.Page)
                Exit Function
            End If
            Param = Me.HProjectId.Value.ToString() + "##" + cCodingStatus
            If Not objHelp.Proc_CRFTermCode(Param, Ds_crfterm, Estr) Then
                Return False
            End If
            If Not Ds_crfterm.Tables(0).Rows.Count > 0 Then
                objCommon.ShowAlert("No Data For Selected Project", Me.Page)
                'Me.lblCodeStatus.Style.Add("display", "none")
                'Me.tdCodeStatus.Style.Add("display", "none")
                'Me.trDictionary.Visible = False
                'Me.trForSiteWise.Visible = False
                'Me.trCRFTerm.Visible = False
                ddlDictionary.Items.Clear()
                MedExDropDown.Items.Clear()
                ddlPeriods.Items.Clear()
                ddlActStatus.Items.Clear()
                Me.tdHRUpper.Style.Add("display", "none")
                Me.tdHRLower.Style.Add("display", "none")
                Me.fsetSubject.Style.Add("display", "none")
                Me.fsetActivity.Style.Add("display", "none")
                Me.tvActivity.Nodes.Clear()
                Me.tvSubject.Nodes.Clear()
                Me.tvSubject.Nodes.Clear()
                Me.ViewState(VS_CRFTermCode) = Nothing
                'Me.ddlActStatus.Style.Add("width", "100%")
                Exit Function
            End If
            Me.ddlActStatus.Style.Add("width", "62%")
            Me.lblCodeStatus.Style.Add("display", "")
            Me.tdCodeStatus.Style.Add("display", "")
            dt_medera = Ds_crfterm.Tables(0)
            dt_medera.DefaultView().RowFilter = "CRFTerm is not null"
            Me.ViewState(VS_CRFTermCode) = dt_medera.DefaultView.ToTable
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("Error While Filling AllCRFData", Estr)
            Return False
        End Try
    End Function

    Public Function FillCRFDropDown() As Boolean
        Dim Estr As String = String.Empty
        Dim dt_Medra As DataTable = Nothing
        Dim dt_CrfTermMed As DataTable = Nothing
        Dim dt_CodingStatus As DataTable = Nothing
        Try
            dt_CodingStatus = CType(Me.ViewState(VS_CRFTermCode), DataTable).Copy()
            If (ddlCodingStatus.SelectedIndex > 0) Then
                If (ddlCodingStatus.SelectedIndex = 1) Then
                    dt_CodingStatus.DefaultView().RowFilter = "vMedExType = 'ComboGlobalDictionary'" + "AND CONVERT(Isnull(vMedExResult,''), System.String) <> ''"
                Else
                    dt_CodingStatus.DefaultView().RowFilter = "vMedExType = 'ComboGlobalDictionary'" + "AND CONVERT(Isnull(vMedExResult,''), System.String) = ''"
                End If
            End If

            dt_CrfTermMed = dt_CodingStatus.DefaultView.ToTable
            If (ddlDictionary.SelectedIndex > 0) Then
                Dim strDictionary As String = Convert.ToString(ddlDictionary.Text)
                If (strDictionary <> "") Then
                    dt_CrfTermMed.DefaultView().RowFilter = "cDictionaryType =" + "'" + strDictionary + "'"
                End If
            End If

            Me.MedExDropDown.DataSource = dt_CrfTermMed.DefaultView().ToTable(True, "CRFTerm")
            Me.MedExDropDown.DataTextField = "CRFTerm"
            Me.MedExDropDown.DataValueField = "CRFTerm"
            Me.MedExDropDown.DataBind()
            Me.MedExDropDown.Items.Insert(0, New ListItem("All", 0))
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("Error While Filling Dropdown", Estr)
            Return False
        End Try
    End Function

    Private Function FillddlPeriods(ByRef eStr As String) As Boolean
        Dim ds_Periods As New DataSet
        Dim wStr As String = String.Empty
        Dim Periods As Integer = 1

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            Me.ddlPeriods.Items.Clear()

            wStr = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "' and cStatusIndi<>'D'"

            If Not objHelp.GetWorkspaceProtocolDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        ds_Periods, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_Periods Is Nothing Then
                Throw New Exception("Problem while getting data")
            End If

            If Not ds_Periods.Tables(0).Rows(0)("iNoOfPeriods") Is Nothing AndAlso Convert.ToString(ds_Periods.Tables(0).Rows(0)("iNoOfPeriods")).Trim() <> "" Then

                Periods = ds_Periods.Tables(0).Rows(0)("iNoOfPeriods")
                For count As Integer = 0 To Periods - 1
                    Me.ddlPeriods.Items.Add((count + 1).ToString)
                Next count

            End If
            Me.ddlPeriods.Items.Insert(0, "All")
            Return True

        Catch ex As Exception
            ShowErrorMessage("Problem While Filling Periods. ", ex.Message)
            eStr = ex.Message
            Return False
        Finally
            ds_Periods.Dispose()
        End Try

    End Function

    Private Function FillddlActStatusColor() As Boolean
        Try
            Dim str As String = String.Empty
            If (ddlActStatus.Items.Count > 0) Then
                'Me.ddlActStatus.sel
                'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ChangeColor", "ChangeColor();", True)
                Me.ddlActStatus.Items.FindByValue("0").Attributes.Add("style", "font-weight:bold")
                'Me.ddlActStatus.Items.FindByValue("1").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Red))
                'Me.ddlActStatus.Items.FindByValue("2").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Orange))
                Me.ddlActStatus.Items.FindByValue("1").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Blue))

                If ddlActStatus.Items.Count = 5 Then
                    Me.ddlActStatus.Items.FindByValue("2").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Purple))
                    Me.ddlActStatus.Items.FindByValue("3").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Green))
                    Me.ddlActStatus.Items.FindByValue("4").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Gray))
                ElseIf ddlActStatus.Items.Count = 4 Then
                    Me.ddlActStatus.Items.FindByValue("2").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Purple))
                    Me.ddlActStatus.Items.FindByValue("3").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Green))
                ElseIf ddlActStatus.Items.Count = 3 Then
                    Me.ddlActStatus.Items.FindByValue("2").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Purple))
                End If
            End If
            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".......FillddlActStatusColor")
            Return False
        End Try
    End Function

    Private Function FillddlActStatus() As Boolean
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim ds As New DataSet
        Dim SeqNo As Integer
        Try

            Me.ddlActStatus.Items.Clear()
            dt.Columns.Add(New DataColumn("iSeqNo", GetType(Integer)))
            dt.Columns.Add(New DataColumn("vDesc", GetType(String)))
            dt.AcceptChanges()

            dr = dt.NewRow()
            dr("iSeqNo") = "0"
            dr("vDesc") = "All"
            dt.Rows.InsertAt(dr, 0)
            dt.AcceptChanges()

            'dr = dt.NewRow()
            'dr("iSeqNo") = "1"
            'dr("vDesc") = "Data Entry Pending"
            'dt.Rows.InsertAt(dr, 1)
            'dt.AcceptChanges()

            'dr = dt.NewRow()
            'dr("iSeqNo") = "2"
            'dr("vDesc") = "Data Entry Continue"
            'dt.Rows.InsertAt(dr, 2)
            'dt.AcceptChanges()

            dr = dt.NewRow()
            dr("iSeqNo") = "1"
            dr("vDesc") = "Ready For Review"
            dt.Rows.InsertAt(dr, 3)
            dt.AcceptChanges()

            dt.DefaultView.Sort = "iSeqNo Desc"
            ds = CType(Me.Session(Vs_dsReviewerlevel), DataSet)
            If ds.Tables(0).Rows.Count > 0 Then
                SeqNo = Convert.ToInt16(dt.DefaultView.ToTable.Rows(0)("iSeqNo")) + 1
                For Each dr_Reports As DataRow In ds.Tables(0).Rows
                    dr = dt.NewRow
                    dr("iSeqNo") = SeqNo
                    dr("vDesc") = Convert.ToString(dr_Reports("Reviewer"))
                    dt.Rows.Add(dr)
                    dt.AcceptChanges()
                    SeqNo = SeqNo + 1
                Next
            End If
            dt.DefaultView.Sort = "iSeqNo Asc"
            Me.ddlActStatus.DataSource = dt
            Me.ddlActStatus.DataTextField = "vDesc"
            Me.ddlActStatus.DataValueField = "iSeqNo"
            Me.ddlActStatus.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ChangeColor", "ChangeColor();", True)

            Me.ddlActStatus.Items.FindByValue("0").Attributes.Add("style", "font-weight:bold")
            'Me.ddlActStatus.Items.FindByValue("1").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Red))
            'Me.ddlActStatus.Items.FindByValue("2").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Orange))
            Me.ddlActStatus.Items.FindByValue("1").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Blue))

            If ddlActStatus.Items.Count = 5 Then
                Me.ddlActStatus.Items.FindByValue("2").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Purple))
                Me.ddlActStatus.Items.FindByValue("3").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Green))
                Me.ddlActStatus.Items.FindByValue("4").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Gray))
            ElseIf ddlActStatus.Items.Count = 4 Then
                Me.ddlActStatus.Items.FindByValue("2").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Purple))
                Me.ddlActStatus.Items.FindByValue("3").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Green))
            ElseIf ddlActStatus.Items.Count = 3 Then
                Me.ddlActStatus.Items.FindByValue("2").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Purple))
            End If
            Me.ddlActStatus.SelectedIndex = 0
            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".......FillddlActStatus")
            Return False
        End Try
    End Function

#End Region

#Region "Dropdown Events"

    Protected Sub MedExDropDown_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim dt_CRF As DataTable = Nothing
        Try
            If rbtCoding.SelectedValue = "General" Then
                If Not FILLGRIDVIEWCRF() Then
                    objCommon.ShowAlert("Error While Assigning Data To Grid", Me.Page)
                    Exit Sub
                End If
            End If
            If rbtCoding.SelectedValue = "SiteWise" Then
                If Not FillddlActStatusColor() Then
                    objCommon.ShowAlert("Error While Fill ddlActStatusColor", Me.Page)
                    Exit Sub
                End If
            End If
            ResetPage()
        Catch ex As Exception
            Me.ShowErrorMessage("Error in Dropdown Selected Index Change", "MedExDropDown_SelectedIndexChanged")
        End Try

    End Sub

    Protected Sub ddlDictionary_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.trDictionary.Visible = True
        If Not FillCRFDropDown() Then
            objCommon.ShowAlert("Error While Filling Dropdown", Me.Page)
            Exit Sub
        End If
        If rbtCoding.SelectedValue = "General" Then
            If Not FILLGRIDVIEWCRF() Then
                objCommon.ShowAlert("Error While Assigning Data To Grid", Me.Page)
                Exit Sub
            End If
        End If
        If rbtCoding.SelectedValue = "SiteWise" Then
            If Not FillddlActStatusColor() Then
                objCommon.ShowAlert("Error While Fill ddlActStatusColor", Me.Page)
                Exit Sub
            End If
        End If
    End Sub

    Protected Sub ddlCodingStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCodingStatus.SelectedIndexChanged
        Dim dt_CRF As DataTable = Nothing
        Try
            If IsNothing(CType(Me.ViewState(VS_CRFTermCode), DataTable)) Then
                objCommon.ShowAlert("No Data For Selected Project", Me.Page)
                Exit Sub
            End If
            If Not FillddlDictionary() Then
                objCommon.ShowAlert("Error While Fill ddlDictionary", Me.Page)
                Exit Try
            End If
            If Not FillCRFDropDown() Then
                objCommon.ShowAlert("Error While Filling Dropdown", Me.Page)
                Exit Sub
            End If
            If rbtCoding.SelectedValue = "General" Then
                If Not FILLGRIDVIEWCRF() Then
                    objCommon.ShowAlert("Error While Assigning Data To Grid", Me.Page)
                    Exit Sub
                End If
            End If

            If rbtCoding.SelectedValue = "SiteWise" Then
                If Not FillddlActStatusColor() Then
                    objCommon.ShowAlert("Error While Fill ddlActStatusColor", Me.Page)
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            Me.ShowErrorMessage("Error in Dropdown Selected Index Change", "ddlCodingStatus_SelectedIndexChanged")
        End Try

    End Sub

    Protected Sub ddlPeriods_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPeriods.SelectedIndexChanged
        Try
            If Me.ddlActType.SelectedIndex = 0 Then
                Me.tdHRUpper.Style.Add("display", "none")
                Me.tdHRLower.Style.Add("display", "none")
                Me.fsetSubject.Style.Add("display", "none")
                Me.fsetActivity.Style.Add("display", "none")
                Me.tvActivity.Nodes.Clear()
                Me.tvSubject.Nodes.Clear()
                Me.tvSubject.Nodes.Clear()
                Me.tvActivity.Nodes.Clear()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ChangeColor", "ChangeColor();", True)
                If Not FillddlActStatus() Then
                    objCommon.ShowAlert("Error While Fill ddlActStatus", Me.Page)
                    Exit Sub
                End If
                Exit Sub
            End If
            ddlActType_SelectedIndexChanged(sender, e)
            'If Not FillddlActStatus() Then
            '    objCommon.ShowAlert("Error While Fill ddlActStatus", Me.Page)
            '    Exit Sub
            'End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".............ddlPeriods_SelectedIndexChanged")
        End Try
    End Sub

    Protected Sub ddlActType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlActType.SelectedIndexChanged
        Dim eStr As String = String.Empty
        Try
            If txtproject.Text.Trim() = "" Then
                objCommon.ShowAlert("First Select Project !", Me.Page)
                ddlActType.SelectedIndex = 0
            End If
            If Not Me.HProjectId.Value.Trim.ToString = "" Then
                Me.tvSubject.Nodes.Clear()
                Me.tvActivity.Nodes.Clear()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ChangeColor", "ChangeColor();", True)

                Me.tdHRUpper.Style.Add("display", "none")
                Me.tdHRLower.Style.Add("display", "none")
                Me.fsetSubject.Style.Add("display", "none")
                Me.fsetActivity.Style.Add("display", "none")
                Me.tvActivity.Nodes.Clear()
                Me.tvSubject.Nodes.Clear()
                If Me.ddlActType.SelectedIndex <> 0 Then

                    If Not BindSubjectTree(eStr) Then
                        Throw New Exception(eStr)
                    End If
                    If Not BindActivityTree(eStr) Then
                        Throw New Exception(eStr)
                    End If
                    Me.tdHRUpper.Style.Add("display", "")
                    Me.tdHRLower.Style.Add("display", "")

                    If Me.ddlActType.SelectedIndex = 1 Then
                        Me.fsetSubject.Style.Add("display", "block")
                    End If
                    Me.fsetActivity.Style.Add("display", "block")
                End If

                If Not FillddlActStatus() Then
                    objCommon.ShowAlert("Error While Fill ddlActStatus", Me.Page)
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".............ddlType_SelectedIndexChanged")
        End Try
    End Sub

    Protected Sub ddlActStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlActStatus.SelectedIndexChanged
        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ChangeColor", "ChangeColor();", True)

        Me.ddlActStatus.Items.FindByValue("0").Attributes.Add("style", "font-weight:bold")
        'Me.ddlActStatus.Items.FindByValue("1").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Red))
        'Me.ddlActStatus.Items.FindByValue("2").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Orange))
        Me.ddlActStatus.Items.FindByValue("1").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Blue))

        If ddlActStatus.Items.Count = 5 Then
            Me.ddlActStatus.Items.FindByValue("2").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Purple))
            Me.ddlActStatus.Items.FindByValue("3").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Green))
            Me.ddlActStatus.Items.FindByValue("4").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Gray))
        ElseIf ddlActStatus.Items.Count = 4 Then
            Me.ddlActStatus.Items.FindByValue("2").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Purple))
            Me.ddlActStatus.Items.FindByValue("3").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Green))
        ElseIf ddlActStatus.Items.Count = 3 Then
            Me.ddlActStatus.Items.FindByValue("2").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Purple))
        End If
    End Sub

    Protected Function BindSubjectTree(ByRef eStr As String) As Boolean
        Dim strqry As String = String.Empty
        Dim whrCon As String = String.Empty
        Dim dsSubject As DataSet = New DataSet
        Dim dtSubject As New DataTable
        Dim period As String = "1"

        If Me.ddlPeriods.SelectedValue <> "All" Then
            period = Me.ddlPeriods.SelectedValue
        End If
        Try
            whrCon = " vWorkspaceId='" + Me.HProjectId.Value + "'" _
                    + " and  iPeriod=" + CInt(period).ToString + "  AND cStatusIndi <> 'D' Order by iMySubjectNo"
            Me.tdHRUpper.Style.Add("display", "none")
            Me.tdHRLower.Style.Add("display", "none")
            Me.fsetSubject.Style.Add("display", "none")
            Me.tvSubject.Style.Add("Height", "0px")
            If Me.ddlActType.SelectedValue = Sub_Specific_Activity Then
                If Not objHelp.GetWorkspaceSubjectMst(whrCon, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsSubject, eStr) Then
                    Throw New Exception(eStr)
                End If
                If Not dsSubject Is Nothing Then
                    If dsSubject.Tables(0).Rows.Count > 0 Then
                        dtSubject = dsSubject.Tables(0)
                        Dim nodeAll As New TreeNode()
                        nodeAll.Text = "All Subject\ScreenNo*"
                        nodeAll.Value = "All Subject\ScreenNo"

                        If (Request.QueryString("ProjectName") <> "" And Request.QueryString("SubjectId") = "") Then
                            nodeAll.Checked = True
                        End If
                        Me.tvSubject.Nodes.Add(nodeAll)
                        For index = 0 To dtSubject.Rows.Count - 1
                            Dim nodeSubject As New TreeNode()
                            nodeSubject.Text = dtSubject.Rows(index).Item("vMySubjectNo").ToString()
                            If dtSubject.Rows(index).Item("cRejectionFlag").ToString() = "Y" Then
                                nodeSubject.Text = "<font color = red>" + dtSubject.Rows(index).Item("vMySubjectNo").ToString() + "</font>"
                            End If
                            nodeSubject.ToolTip = dtSubject.Rows(index).Item("vMySubjectNo").ToString() + "|" + dtSubject.Rows(index).Item("vSubjectId").ToString()
                            nodeSubject.Value = dtSubject.Rows(index).Item("vSubjectId").ToString()
                            nodeSubject.SelectAction = TreeNodeSelectAction.None
                            If (Request.QueryString("ProjectName") <> "" And Request.QueryString("SubjectId") <> "" And Request.QueryString("SubjectId") = dtSubject.Rows(index).Item("vSubjectId").ToString()) Then
                                nodeSubject.Checked = True
                            ElseIf (Request.QueryString("ProjectName") <> "" And Request.QueryString("SubjectId") = "") Then
                                nodeSubject.Checked = True
                            End If
                            nodeSubject.ChildNodes.Add(nodeSubject)

                            Me.tvSubject.Nodes(0).ChildNodes.Add(nodeSubject)
                        Next
                        Me.tvSubject.Nodes(0).ExpandAll()
                        Me.tvSubject.Nodes(0).SelectAction = TreeNodeSelectAction.None
                        Me.tdHRUpper.Style.Add("display", "")
                        Me.tdHRLower.Style.Add("display", "")
                        Me.fsetSubject.Style.Add("display", "block")
                        Me.tvSubject.Style.Add("Height", "100px")
                    Else
                        objCommon.ShowAlert("No Subject Found For This Project!", Me.Page)
                    End If
                End If

            End If
            Return True
        Catch ex As Exception
            eStr = ex.Message
            Return False
        Finally
            dsSubject.Dispose()
            dtSubject.Dispose()
        End Try
    End Function

    Protected Function BindActivityTree(ByRef eStr As String) As Boolean
        Dim strqry As String = String.Empty
        Dim iPeriod As String = String.Empty
        Dim whrCon As String = String.Empty
        Dim dtActivity As New DataTable
        Dim dsActivity As DataSet = New DataSet
        Dim dvActivity As DataView
        Dim dvChild As DataView
        Dim Subject_Specific As String = String.Empty
        Dim ActNodeAll As New TreeNode()
        Try
            iPeriod = "," + Me.ddlPeriods.SelectedValue.ToString() + ","
            If Me.ddlPeriods.SelectedValue = "All" Then
                iPeriod = ""
            End If
            Subject_Specific = "Y"
            If Me.ddlActType.SelectedValue = Generic_Activity Then
                Subject_Specific = "N"
            End If

            'Hiren Rami
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                If Not objHelp.Proc_GetActivityTreeCRFTerm(Me.HProjectId.Value, iPeriod, Subject_Specific, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, dsActivity, eStr) Then
                    Throw New Exception(eStr)
                End If
            Else
                If Not objHelp.Proc_GetActivityTreeCRFTerm(Me.HProjectId.Value, iPeriod, Subject_Specific, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, dsActivity, eStr) Then
                    Throw New Exception(eStr)
                End If
            End If
            'Proc_GetActivityTreeCRFTerm_BABE
            dtActivity = dsActivity.Tables(0)
            dvActivity = New DataView(dtActivity)
            dvActivity.RowFilter = "TreeLevel=0"
            ActNodeAll.Text = "All Activity*"
            ActNodeAll.Value = "All Activity"
            Me.tvActivity.Nodes.Add(ActNodeAll)
            If (Request.QueryString("ProjectName") <> "") Then
                ActNodeAll.Checked = True
            End If
            For ParentNode = 0 To dvActivity.Count - 1
                Dim nodeActivity As New TreeNode()
                nodeActivity.Text = dvActivity(ParentNode).Item("Name").ToString()
                nodeActivity.ToolTip = dvActivity(ParentNode).Item("Name").ToString()
                nodeActivity.Value = dvActivity(ParentNode).Item("Id").ToString()
                If (Request.QueryString("ProjectName") <> "") Then
                    nodeActivity.Checked = True
                End If
                nodeActivity.SelectAction = TreeNodeSelectAction.None
                nodeActivity.ChildNodes.Add(nodeActivity)
                dvChild = New DataView(dtActivity)
                dvChild.RowFilter = "Treelevel=1 AND ParentId=" + dvActivity(ParentNode).Item("Id").ToString()
                dvChild.Sort = "iNodeNo"
                For ChildNode = 0 To dvChild.Count - 1
                    Dim nodeChild As New TreeNode()
                    nodeChild.Text = dvChild(ChildNode)("Name").ToString()
                    nodeChild.ToolTip = dvChild(ChildNode)("Name").ToString()
                    nodeChild.Value = dvChild(ChildNode)("Id").ToString()
                    If (Request.QueryString("ProjectName") <> "") Then
                        nodeChild.Checked = True
                    End If
                    nodeChild.SelectAction = TreeNodeSelectAction.None
                    nodeActivity.ChildNodes.Add(nodeChild)
                Next
                Me.tvActivity.Nodes(0).ChildNodes.Add(nodeActivity)
            Next
            Me.tvActivity.Nodes(0).Expand()
            Me.tvActivity.Nodes(0).SelectAction = TreeNodeSelectAction.None
            Return True

        Catch ex As Exception
            eStr = ex.Message
            Return False
        Finally
            dsActivity.Dispose()
            dtActivity.Dispose()
        End Try
    End Function

#End Region

#Region "Fill FILLGRIDVIEWCRF"
    Private Function FILLGRIDVIEWCRF() As Boolean
        Dim ds_crf As DataSet = Nothing
        Dim dt_CRf As DataTable = Nothing
        Dim sender As Object = Nothing

        Dim dt_Coding As DataTable = Nothing
        Dim dt_Dictionary As DataTable = Nothing
        Dim dt_CRFTerm As DataTable = Nothing

        Dim e As System.EventArgs = Nothing
        Try

            If IsNothing(CType(Me.ViewState(VS_CRFTermCode), DataTable)) Then
                objCommon.ShowAlert("No Data For Selected Project", Me.Page)
                Exit Function
            End If

            Me.trDictionary.Visible = True
            If rbtCoding.SelectedValue = "General" Then

                dt_Coding = CType(Me.ViewState(VS_CRFTermCode), DataTable).Copy()
                If (ddlCodingStatus.SelectedIndex > 0) Then
                    If (ddlCodingStatus.SelectedIndex = 1) Then
                        dt_Coding.DefaultView().RowFilter = "vMedExType = 'ComboGlobalDictionary'" + "AND CONVERT(Isnull(vMedExResult,''), System.String) <> ''"
                    Else
                        dt_Coding.DefaultView().RowFilter = "vMedExType = 'ComboGlobalDictionary'" + "AND CONVERT(Isnull(vMedExResult,''), System.String) = ''"
                    End If
                End If

                dt_Dictionary = dt_Coding.DefaultView.ToTable
                If (ddlDictionary.SelectedIndex > 0) Then
                    Dim strDictionary As String = Convert.ToString(ddlDictionary.Text)
                    If (strDictionary <> "") Then
                        dt_Dictionary.DefaultView().RowFilter = "vMedExType = 'ComboGlobalDictionary'" + "AND cDictionaryType =" + "'" + strDictionary + "'"
                    End If
                End If

                dt_CRFTerm = dt_Dictionary.DefaultView.ToTable
                If (MedExDropDown.SelectedIndex > 0) Then
                    Dim strMedExDropDown As String = Convert.ToString(MedExDropDown.Text)
                    If (strMedExDropDown <> "") Then
                        dt_CRFTerm.DefaultView().RowFilter = "vMedExType = 'ComboGlobalDictionary'" + "AND CRFTerm = '" + Me.MedExDropDown.SelectedValue.ToString.Replace("'", "''") + "'"
                    End If
                End If

                If (ddlCodingStatus.SelectedIndex <= 0 And ddlDictionary.SelectedIndex <= 0 And MedExDropDown.SelectedIndex <= 0) Then
                    dt_CRFTerm.DefaultView().RowFilter = "vMedExType = 'ComboGlobalDictionary'"
                End If
                dt_CRf = dt_CRFTerm.DefaultView.ToTable

                dt_CRf.DefaultView.Sort = "dModifyOn"
                Me.ViewState(VS_ExportGrid) = dt_CRf.DefaultView.ToTable
                Me.gvwCRF.DataSource = dt_CRf.DefaultView.ToTable
                Me.gvwCRF.DataBind()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvwCRF", "UIgvwCRF(); ", True)
                If dt_CRf.DefaultView.ToTable.Rows.Count <> 0 Then
                    Me.btnExport.Visible = True
                Else
                    Me.btnExport.Visible = False
                End If
                'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Display", "Display(" + Me.imgMedCodeDetail.ClientID + ",'divMedCodeDetail');", True)
                Return True
            End If
        Catch ex As Exception
            Me.ShowErrorMessage("Error While Binding Grid", "....FILLGRIDVIEWCRF")
            Return False
        End Try
    End Function

    Private Function FILLGRIDVIEWCRF1(ByRef eStr As String) As Boolean
        Dim dsMaster As New DataSet
        Dim ds_reviewer As New DataSet
        Dim dv As DataView
        Dim dt_view As DataTable = Nothing

        Dim strSubjectId As String = String.Empty
        Dim strParentID As String = String.Empty
        Dim strActivityId As String = String.Empty
        Try

            If Me.ddlActType.SelectedValue = Sub_Specific_Activity Then
                'If tvSubject.Nodes(0).Checked = False Then
                For index = 0 To tvSubject.Nodes(0).ChildNodes.Count - 1
                    If tvSubject.Nodes(0).ChildNodes(index).Checked = True Then
                        strSubjectId = strSubjectId + tvSubject.Nodes(0).ChildNodes(index).Value + ","
                    End If
                Next
                If strSubjectId <> "" Then
                    strSubjectId.Remove(strSubjectId.Length - 1)
                    strSubjectId = "'," + strSubjectId + "'"
                End If
                'End If
            End If

            'If tvActivity.Nodes(0).Checked = False Then
            For index = 0 To tvActivity.Nodes(0).ChildNodes.Count - 1
                If tvActivity.Nodes(0).ChildNodes(index).Checked = True Then
                    strParentID = strParentID + tvActivity.Nodes(0).ChildNodes(index).Value + ","
                End If
            Next
            'End If
            If strParentID <> "" Then
                strParentID.Remove(strParentID.Length - 1)
                strParentID = "'," + strParentID + "'"
            End If

            'If tvActivity.Nodes(0).Checked = False Then
            For iParent = 0 To tvActivity.Nodes(0).ChildNodes.Count - 1
                For iChild = 0 To tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes.Count - 1
                    If tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes(iChild).Checked = True Then
                        strActivityId = strActivityId + tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes(iChild).Value + ","
                    End If
                Next
            Next
            'End If

            If strActivityId <> "" Then
                strActivityId.Remove(strActivityId.Length - 1)
                strActivityId = "'," + strActivityId + "'"
            End If

            cSubjectWiseFlag = "Y"
            If Me.ddlActType.SelectedValue = Generic_Activity Then
                cSubjectWiseFlag = "N"
                strSubjectId = ""
            End If
            iPeriod = "," + Me.ddlPeriods.SelectedValue.ToString() + ","
            If Me.ddlPeriods.SelectedValue = "All" Then
                iPeriod = ""
            End If
            'If Me.ddlActStatus.SelectedValue = Data_Entry_Pending Then
            '    cDataStatus = ",0,"
            '    iWorkflowStageId = ",0,"
            'ElseIf ddlActStatus.SelectedValue = Data_Entry_Continue Then
            '    cDataStatus = ",B,"
            '    iWorkflowStageId = ",0,"
            If ddlActStatus.SelectedValue = First_Review_Pending Then
                cDataStatus = ",D,"
                iWorkflowStageId = ",0,"
            Else
                ds_reviewer = CType(Me.Session(Vs_dsReviewerlevel), DataSet)
                dv = ds_reviewer.Tables(0).Copy.DefaultView
                dv.RowFilter = "Reviewer ='" + ddlActStatus.SelectedItem.Text.Trim() + "'"
                If dv.ToTable.Rows.Count > 0 Then
                    If dv.ToTable.Rows(0)("vStatus") = "L" Then
                        cDataStatus = ",F,"
                    Else
                        cDataStatus = ",E,"
                    End If
                    iWorkflowStageId = "," + Convert.ToString(dv.ToTable.Rows(0)("iReviewWorkflowStageId")) + ","
                End If
            End If
            iProjectType = 1
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                iProjectType = 2
            End If
            objHelp.Timeout = -1

            Dim strDictionary As String = String.Empty
            If Me.ddlDictionary.SelectedValue = "All" Then
                For Each item As ListItem In ddlDictionary.Items
                    If (item.Value <> "All") Then
                        strDictionary = strDictionary + Convert.ToString(item.Value) + ","
                    End If
                Next

                If strDictionary <> "" Then
                    strDictionary = strDictionary.Remove(strDictionary.Length - 1)
                    strDictionary = "''" + strDictionary + "''"
                End If
            Else
                strDictionary = ddlDictionary.SelectedValue
            End If

            cDictionaryType = strDictionary
            CRFTerm = MedExDropDown.SelectedValue
            cCodingStatus = ddlCodingStatus.SelectedValue
            'If ddlActStatus.SelectedIndex = "1" Then
            '    cActStatus = "DEP"
            'ElseIf ddlActStatus.SelectedIndex = "2" Then
            '    cActStatus = "DEC"
            If ddlActStatus.SelectedIndex = "0" Then
                cActStatus = "0"
            ElseIf ddlActStatus.SelectedIndex = "1" Then
                cActStatus = "FRP"
            ElseIf ddlActStatus.SelectedIndex = "2" Then
                cActStatus = "SRP"
            ElseIf ddlActStatus.SelectedIndex = "3" Then
                cActStatus = "FnlRP"
            ElseIf ddlActStatus.SelectedIndex = "4" Then
                cActStatus = "Locked"
            End If
            If GetLegends() Then
            End If
            If Not objHelp.Proc_getcrfactivitystatusformedicalcoding(Me.HProjectId.Value, iPeriod, strSubjectId, strParentID, strActivityId, cSubjectWiseFlag, iProjectType, cDataStatus, iWorkflowStageId, cDictionaryType, CRFTerm, cCodingStatus, cActStatus, dsMaster, eStr) Then
                Me.objCommon.ShowAlert("Error in Getting Data", Me)
                Return False
            End If

            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                dsMaster.Tables(0).Columns("vSubjectId").ColumnName = "vSubjectId1"
                dsMaster.Tables(0).Columns("vMySubjectNo").ColumnName = "vMySubjectNo1"

                dsMaster.Tables(0).Columns("vSubjectId1").ColumnName = "vMySubjectNo"
                dsMaster.Tables(0).Columns("vMySubjectNo1").ColumnName = "vSubjectId"
            End If

            dt_view = dsMaster.Tables(0)
            Me.gvwCRF.DataSource = dt_view
            Me.gvwCRF.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvwCRF", "UIgvwCRF(); ", True)
            If dt_view.DefaultView.ToTable.Rows.Count <> 0 Then
                Me.btnExport.Visible = True
            Else
                Me.btnExport.Visible = False
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Display", "Display(" + Me.imgMedCodeDetail.ClientID + ",'divMedCodeDetail');", True)
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("Error While Binding Grid", "....FILLGRIDVIEWCRF1")
            Return False
        End Try
    End Function

#End Region

#Region "Grid Events"

    Protected Sub gvwCRF_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwCRF.RowCreated
        e.Row.Cells(GVCSub_nCRFSubDtlNo).Style.Add("display", "none")
        e.Row.Cells(GVCSub_vRandomizationNo).Style.Add("display", "none")
        e.Row.Cells(GVCSub_vProjectTypeCOde).Style.Add("display", "none")
        e.Row.Cells(GVCSub_vSubjectId).Style.Add("display", "none")
        e.Row.Cells(GVCSub_vMySubjectNo).Style.Add("display", "none")
        If Me.MedExDropDown.SelectedIndex = 0 Then
            e.Row.Cells(GVCSub_Select).Style.Add("display", "none")
        End If
        If hndLockStatus.Value.Trim() = "Lock" Then
            'CType(e.Row.FindControl("ChkMove"), CheckBox).Attributes.Add("Disabled", "Disabled")
            e.Row.Cells(GVCSub_Select).Style.Add("display", "none")
        End If
    End Sub

    Protected Sub gvwCRF_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwCRF.RowDataBound
        Dim dt_medra As DataTable = Nothing
        dt_medra = CType(Me.ViewState(VS_CRFTermCode), DataTable).Copy()

        If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
            e.Row.Cells(GVCSub_vSubjectId).Style.Add("display", "")
            e.Row.Cells(GVCSub_vRandomizationNo).Style.Add("display", "")
        Else
            e.Row.Cells(GVCSub_vSubjectId).Style.Add("display", "")
            e.Row.Cells(GVCSub_vMySubjectNo).Style.Add("display", "")
        End If

        'If rbtCoding.SelectedValue = "General" Then
        '    If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
        '        e.Row.Cells(GVCSub_vMySubjectNo).Style.Add("display", "")
        '    Else
        '        e.Row.Cells(GVCSub_vMySubjectNo).Style.Add("display", "")
        '        'e.Row.Cells(GVCSub_vSubjectId).Style.Add("display", "")
        '    End If
        'Else
        '    'e.Row.Cells(GVCSub_vSubjectId).Style.Add("display", "")
        '    e.Row.Cells(GVCSub_vMySubjectNo).Style.Add("display", "")
        '    e.Row.Cells(GVCSub_vRandomizationNo).Style.Add("display", "")
        'End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            If Not e.Row.Cells.Item(GVCSub_dModifyOn).Text.ToString = "" Then
                e.Row.Cells.Item(GVCSub_dModifyOn).Text = CType(Replace(e.Row.Cells(GVCSub_dModifyOn).Text.Trim(), "&nbsp;", ""), Date).ToString("dd-MMM-yyyy HH:mm ").Trim() + strServerOffset.ToString.Trim()
                ' e.Row.Cells.Item(GVCSub_dModifyOn).Text.ToString(+strServerOffset.ToString.Replace("IST ", ""))               
            End If
        End If
    End Sub

#End Region

#Region "Button Events"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim eStr As String = String.Empty
        Try
            If Not FillAllCRFData() Then
                objCommon.ShowAlert("Error While Filling AllCRFData", Me.Page)
                Exit Sub
            End If

            If Not FillddlDictionary() Then
                objCommon.ShowAlert("Error While Fill ddlDictionary", Me.Page)
                Exit Try
            End If

            If Not FillCRFDropDown() Then
                objCommon.ShowAlert("Error While Filling Dropdown", Me.Page)
                Exit Sub
            End If

            If rbtCoding.SelectedValue = "General" Then
                If Not FILLGRIDVIEWCRF() Then
                    objCommon.ShowAlert("Error While Assigning Data To Grid", Me.Page)
                    Exit Sub
                End If
            End If
            If rbtCoding.SelectedValue = "SiteWise" Then
                Me.tdHRUpper.Style.Add("display", "none")
                Me.tdHRLower.Style.Add("display", "none")
                Me.fsetSubject.Style.Add("display", "none")
                Me.fsetActivity.Style.Add("display", "none")
                Me.tvActivity.Nodes.Clear()
                Me.tvSubject.Nodes.Clear()
                Me.tvSubject.Nodes.Clear()
                Me.ddlActType.SelectedIndex = 0
                Me.tvActivity.Nodes.Clear()

                If Not FillddlPeriods(eStr) Then
                    Throw New Exception(eStr)
                    Exit Sub
                End If

                If Not GetLegends() Then
                    Exit Sub
                End If

                If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                    Me.ddlActType.SelectedValue = 1
                    ddlActType_SelectedIndexChanged(sender, e)
                End If

                'If Not GetLegends() Then
                '    Exit Sub
                'End If
                'If Not FillddlActStatus() Then
                '    objCommon.ShowAlert("Error While Fill ddlActStatus", Me.Page)
                '    Exit Sub
                'End If
            End If
        Catch ex As Exception
            Me.ShowErrorMessage("Error In btnSetProject_Click", "btnSetProject_Click")
        End Try
    End Sub

    Private Function GetLegends() As Boolean
        Dim ds As New DataSet
        Dim estr As String = ""
        Try
            If Not Me.objHelp.Proc_GetLegends(Me.HProjectId.Value.ToString(), ds, estr) Then
                Throw New Exception("Error While Gettin Proc_getLegends" + estr)
            End If
            If ds Is Nothing Or ds.Tables.Count = 0 Then
                Return False
            End If
            Me.Session(Vs_dsReviewerlevel) = ds.Copy()
            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....GetLegends")
            Return False
        End Try
    End Function

    Protected Sub btnBrowse_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        Dim dt_CRf As DataTable = Nothing
        Dim medexcode As String = String.Empty
        Dim medexdtlno As String = String.Empty
        Dim dt_medera As DataTable = Nothing
        Dim dt_Dictionary As DataTable = Nothing
        Dim DT_final As DataTable = Nothing
        Try
            dt_CRf = CType(Me.ViewState(VS_CRFTermCode), DataTable).Copy()

            If Me.MedExDropDown.SelectedIndex <> 0 Then
                If Me.ddlDictionary.SelectedIndex <> 0 And Me.ddlDictionary.Visible = True Then
                    dt_CRf.DefaultView().RowFilter = "CRFTerm = '" + Me.MedExDropDown.SelectedValue.ToString.Replace("'", "''") + "'" + "AND vMedExType = 'ComboGlobalDictionary'" + " AND cDictionaryType = '" + Me.ddlDictionary.SelectedValue.ToString + "'"
                Else
                    dt_CRf.DefaultView().RowFilter = "CRFTerm = '" + Me.MedExDropDown.SelectedValue.ToString.Replace("'", "''") + "'" + "AND vMedExType = 'ComboGlobalDictionary'"
                End If
            Else
                dt_CRf.DefaultView().RowFilter = "vMEdExType='ComboGlobalDictionary'"
            End If
            dt_medera = dt_CRf.DefaultView.ToTable.Copy
            'dt_Dictionary = dt_CRf.DefaultView.ToTable.DefaultView().ToTable(True, "vRefTableRemark")
            'dt_Dictionary.DefaultView().RowFilter = "vRefTableRemark is not null"
            DT_final = dt_CRf.DefaultView().ToTable(True, "vRefTableRemark")
            If DT_final.Rows.Count > 1 Then
                objCommon.ShowAlert("Please Select Only One Dictionary For A Project", Me.Page)
                Exit Sub
            End If

            medexcode = dt_medera.Rows(0).Item("vMedExCode")
            medexdtlno = dt_medera.Rows(0).Item("nMedexWorkspaceDtlno")
            '-----------------------
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "mederaBrowser", "javascript:MeddraBrowser('" + medexcode + "','" + medexdtlno + "');", True)
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvwCRF", "UIgvwCRF(); ", True)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "btnBrowse_Click")
        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim Strcrfsubdtl As String = String.Empty
        Dim chk As CheckBox
        Dim Ds_crfterm As DataSet = Nothing
        Dim Wstr As String = String.Empty
        Dim ds_CrfSubDtl As DataSet
        Dim eStr As String = String.Empty
        Dim param As String = String.Empty
        Dim dt_medera As DataTable = Nothing
        Try
            param = Me.HProjectId.Value.ToString() + "##" + cCodingStatus

            For index As Integer = 0 To gvwCRF.Rows.Count - 1


                chk = CType(Me.gvwCRF.Rows(index).Cells(GVCSub_Select).FindControl("ChkMove"), CheckBox)
                If chk.Checked Then
                    Strcrfsubdtl += Me.gvwCRF.Rows(index).Cells(GVCSub_nCRFSubDtlNo).Text.ToString.Trim() + ","
                End If
            Next
            If Strcrfsubdtl <> "" Then
                Strcrfsubdtl = Strcrfsubdtl.Substring(0, Strcrfsubdtl.LastIndexOf(","))
            End If
            Wstr = "SELECT * FROM  CRFSubDtl WHERE nCRFSubDtlNo IN (" + Strcrfsubdtl + ") "
            ds_CrfSubDtl = objHelp.GetResultSet(Wstr, "CRFSubDtl")

            If Not AssignUpdatedValues(ds_CrfSubDtl) Then
                objCommon.ShowAlert("Error While Assigning Data", Me.Page)
                Exit Sub
            End If

            If Not Me.objLambda.Save_CRFSubDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                        ds_CrfSubDtl, Me.Session(S_UserID), eStr) Then
                objCommon.ShowAlert("Error While Saving", Me.Page)
                Exit Sub
            End If

            If Not objHelp.Proc_CRFTermCode(param, Ds_crfterm, eStr) Then
                objCommon.ShowAlert("Error While Getting Data ", Me.Page)
                Exit Sub
            End If
            dt_medera = Ds_crfterm.Tables(0)
            dt_medera.DefaultView().RowFilter = "CRFTerm is not null"

            Me.ViewState(VS_CRFTermCode) = dt_medera.DefaultView.ToTable

            ResetPage()
            Me.objCommon.ShowAlert("CrfTerm Coded Sucessfully", Me.Page)
            If rbtCoding.SelectedValue = "General" Then
                If Not FILLGRIDVIEWCRF() Then
                    objCommon.ShowAlert("Error Filling Grid", Me.Page)
                    Exit Sub
                End If
            Else
                If Not FILLGRIDVIEWCRF1(eStr) Then
                    objCommon.ShowAlert("Error Filling Grid For Coding Site Wise", Me.Page)
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "btnSave_Click")
        End Try
    End Sub

    Private Function AssignUpdatedValues(ByRef ds_CrfSubDtl As DataSet) As Boolean
        Dim dt_assign As New DataTable
        Try
            dt_assign = ds_CrfSubDtl.Tables(0)


            For index As Integer = 0 To dt_assign.Rows.Count - 1
                dt_assign.Rows(index)("vMedExResult") = Me.txtcode.Text.Trim()
                dt_assign.Rows(index)("vModificationRemark") = "Generated From Medical Coding"
                dt_assign.Rows(index)("iModifyBy") = Me.Session(S_UserID)
                dt_assign.Rows(index)("dModifyOn") = DateTime.Now()
            Next
            dt_assign.AcceptChanges()
            ds_CrfSubDtl = dt_assign.DataSet
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "AssignUpdatedValues")
            Return False
        End Try
    End Function

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim ds_reviewer As New DataSet
        Dim dv As DataView
        'Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable
        Dim ds_MedicalSiteWise As New DataSet
        Dim estr As String = String.Empty
        Dim dtMedicalCoding As New DataTable
        Dim drMedical As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable
        Dim filename As String = String.Empty
        Dim strMessage As New StringBuilder
        Try
            If rbtCoding.SelectedValue = "SiteWise" Then
                cSubjectWiseFlag = "Y"
                If Me.ddlActType.SelectedValue = Generic_Activity Then
                    cSubjectWiseFlag = "N"
                    strSubjectId = ""
                End If
                iPeriod = "," + Me.ddlPeriods.SelectedValue.ToString() + ","
                If Me.ddlPeriods.SelectedValue = "All" Then
                    iPeriod = ""
                End If
                'If Me.ddlActStatus.SelectedValue = Data_Entry_Pending Then
                '    cDataStatus = ",0,"
                '    iWorkflowStageId = ",0,"
                'ElseIf ddlActStatus.SelectedValue = Data_Entry_Continue Then
                '    cDataStatus = ",B,"
                '    iWorkflowStageId = ",0,"
                If ddlActStatus.SelectedValue = First_Review_Pending Then
                    cDataStatus = ",D,"
                    iWorkflowStageId = ",0,"
                Else
                    ds_reviewer = CType(Me.Session(Vs_dsReviewerlevel), DataSet)
                    dv = ds_reviewer.Tables(0).Copy.DefaultView
                    dv.RowFilter = "Reviewer ='" + ddlActStatus.SelectedItem.Text.Trim() + "'"
                    If dv.ToTable.Rows.Count > 0 Then
                        If dv.ToTable.Rows(0)("vStatus") = "L" Then
                            cDataStatus = ",F,"
                        Else
                            cDataStatus = ",E,"
                        End If
                        iWorkflowStageId = "," + Convert.ToString(dv.ToTable.Rows(0)("iReviewWorkflowStageId")) + ","
                    End If
                End If
                iProjectType = 1
                If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                    iProjectType = 2
                End If
                objHelp.Timeout = -1

                Dim strDictionary As String = String.Empty
                If Me.ddlDictionary.SelectedValue = "All" Then
                    For Each item As ListItem In ddlDictionary.Items
                        If (item.Value <> "All") Then
                            strDictionary = strDictionary + Convert.ToString(item.Value) + ","
                        End If
                    Next

                    If strDictionary <> "" Then
                        strDictionary = strDictionary.Remove(strDictionary.Length - 1)
                        strDictionary = "''" + strDictionary + "''"
                    End If
                Else
                    strDictionary = ddlDictionary.SelectedValue
                End If

                cDictionaryType = strDictionary
                CRFTerm = MedExDropDown.SelectedValue
                cCodingStatus = ddlCodingStatus.SelectedValue
                'If ddlActStatus.SelectedIndex = "1" Then
                '    cActStatus = "DEP"
                'ElseIf ddlActStatus.SelectedIndex = "2" Then
                '    cActStatus = "DEC"
                If ddlActStatus.SelectedIndex = "0" Then
                    cActStatus = "0"
                ElseIf ddlActStatus.SelectedIndex = "1" Then
                    cActStatus = "FRP"
                ElseIf ddlActStatus.SelectedIndex = "2" Then
                    cActStatus = "SRP"
                ElseIf ddlActStatus.SelectedIndex = "3" Then
                    cActStatus = "FnlRP"
                ElseIf ddlActStatus.SelectedIndex = "4" Then
                    cActStatus = "Locked"
                End If
                'iPeriod = "," + Me.ddlPeriods.SelectedValue.ToString() + ","
                'If Me.ddlPeriods.SelectedValue = "All" Then
                '    iPeriod = ""
                'End If

                'If Me.ddlActStatus.SelectedValue = Sub_Specific_Activity Then
                '    If tvSubject.Nodes(0).Checked = False Then
                '        For index = 0 To tvSubject.Nodes(0).ChildNodes.Count - 1
                '            If tvSubject.Nodes(0).ChildNodes(index).Checked = True Then
                '                strSubjectId = strSubjectId + tvSubject.Nodes(0).ChildNodes(index).Value + ","
                '            End If
                '        Next
                '        If strSubjectId <> "" Then
                '            strSubjectId.Remove(strSubjectId.Length - 1)
                '            strSubjectId = "'," + strSubjectId + "'"
                '        End If
                '    End If
                'End If
                'If tvActivity.Nodes(0).Checked = False Then
                '    For index = 0 To tvActivity.Nodes(0).ChildNodes.Count - 1
                '        If tvActivity.Nodes(0).ChildNodes(index).Checked = True Then
                '            strParentID = strParentID + tvActivity.Nodes(0).ChildNodes(index).Value + ","
                '        End If
                '    Next
                'End If
                'If strParentID <> "" Then
                '    strParentID.Remove(strParentID.Length - 1)
                '    strParentID = "'," + strParentID + "'"
                'End If
                'If tvActivity.Nodes(0).Checked = False Then
                '    For iParent = 0 To tvActivity.Nodes(0).ChildNodes.Count - 1
                '        For iChild = 0 To tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes.Count - 1
                '            If tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes(iChild).Checked = True Then
                '                strActivityId = strActivityId + tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes(iChild).Value + ","
                '            End If
                '        Next
                '    Next
                'End If
                'If strActivityId <> "" Then
                '    strActivityId.Remove(strActivityId.Length - 1)
                '    strActivityId = "'," + strActivityId + "'"
                'End If

                If Me.ddlActType.SelectedValue = Sub_Specific_Activity Then
                    For index = 0 To tvSubject.Nodes(0).ChildNodes.Count - 1
                        If tvSubject.Nodes(0).ChildNodes(index).Checked = True Then
                            strSubjectId = strSubjectId + tvSubject.Nodes(0).ChildNodes(index).Value + ","
                        End If
                    Next
                    If strSubjectId <> "" Then
                        strSubjectId.Remove(strSubjectId.Length - 1)
                        strSubjectId = "'," + strSubjectId + "'"
                    End If
                End If

                For index = 0 To tvActivity.Nodes(0).ChildNodes.Count - 1
                    If tvActivity.Nodes(0).ChildNodes(index).Checked = True Then
                        strParentID = strParentID + tvActivity.Nodes(0).ChildNodes(index).Value + ","
                    End If
                Next

                If strParentID <> "" Then
                    strParentID.Remove(strParentID.Length - 1)
                    strParentID = "'," + strParentID + "'"
                End If

                For iParent = 0 To tvActivity.Nodes(0).ChildNodes.Count - 1
                    For iChild = 0 To tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes.Count - 1
                        If tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes(iChild).Checked = True Then
                            strActivityId = strActivityId + tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes(iChild).Value + ","
                        End If
                    Next
                Next

                If strActivityId <> "" Then
                    strActivityId.Remove(strActivityId.Length - 1)
                    strActivityId = "'," + strActivityId + "'"
                End If

                If Not objHelp.Proc_getcrfactivitystatusformedicalcoding(Me.HProjectId.Value, iPeriod, strSubjectId, strParentID, strActivityId, cSubjectWiseFlag, iProjectType, cDataStatus, iWorkflowStageId, cDictionaryType, CRFTerm, cCodingStatus, cActStatus, ds_MedicalSiteWise, estr) Then
                    Me.objCommon.ShowAlert("Error in Getting Data", Me)
                    Exit Sub
                End If
            End If

            If Not dtMedicalCoding Is Nothing Then
                dtMedicalCoding.Columns.Add("Sr. No")
                dtMedicalCoding.Columns.Add("Project No")
                dtMedicalCoding.Columns.Add("Screening No")

                If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                    dtMedicalCoding.Columns.Add("Randomization No")
                Else
                    dtMedicalCoding.Columns.Add("Subject ID")
                End If
                'If rbtCoding.SelectedValue = "SiteWise" Then
                '    dtMedicalCoding.Columns.Add("Randomization No")
                'End If
                dtMedicalCoding.Columns.Add("Repeat No")
                dtMedicalCoding.Columns.Add("CRF Term")
                dtMedicalCoding.Columns.Add("Dictionary Code Term")
                dtMedicalCoding.Columns.Add("Dictionary")
                dtMedicalCoding.Columns.Add("ModifyBy")
                dtMedicalCoding.Columns.Add("ModifyOn")
                'dtMedicalCoding.Columns.Add("Project Type Code")
            End If
            dtMedicalCoding.AcceptChanges()
            If rbtCoding.SelectedValue = "General" Then
                dt = CType(Me.ViewState(VS_ExportGrid), DataTable).Copy()
            Else
                dt = ds_MedicalSiteWise.Tables(0)
            End If
            Dim dvMedical As New DataView(dt)
            For Each dr As DataRow In dvMedical.ToTable.Rows
                drMedical = dtMedicalCoding.NewRow()
                drMedical("Sr. No") = i
                drMedical("Project No") = dr("vProjectNo").ToString()
                drMedical("Screening No") = dr("vSubjectId").ToString()

                If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                    drMedical("Randomization No") = dr("vRandomizationNo").ToString()
                Else
                    drMedical("Subject ID") = dr("vMySubjectNo").ToString()
                End If

                'If rbtCoding.SelectedValue = "SiteWise" Then
                '    drMedical("Randomization No") = dr("vRandomizationNo").ToString()
                'End If
                drMedical("Repeat No") = dr("RepetitionNo").ToString()
                drMedical("CRF Term") = dr("CRFTerm").ToString()
                drMedical("Dictionary Code Term") = dr("vMedExResult").ToString()
                drMedical("Dictionary") = dr("vRefTableRemark").ToString()
                drMedical("ModifyBy") = dr("vModifyBy").ToString()
                drMedical("ModifyOn") = Convert.ToString(dr("dModifyOn"))
                'drMedical("Project Type Code") = dr("vProjectTypeCode").ToString()
                dtMedicalCoding.Rows.Add(drMedical)
                dtMedicalCoding.AcceptChanges()
                i += 1
            Next
            gvExportToExcel.DataSource = dtMedicalCoding
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
                        End If
                        cell.CssClass = "textmode"
                    Next
                Next
                Dim style As String = "<style> .textmode { } </style>"
                Response.Write(style)
                Dim info As String = String.Empty
                Dim gridviewHtml As String = String.Empty
                filename = "Medical Coding" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"
                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(stringWriter)
                gvExportToExcel.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()
                If rbtCoding.SelectedValue = "General" Then
                    strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")
                    strMessage.Append("<tr>")
                    strMessage.Append("<td colspan=""10""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                    strMessage.Append("</font></strong><center></b></td>")
                    strMessage.Append("</tr>")
                    strMessage.Append("<td colspan=""10""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append("General Medical Coding")
                    strMessage.Append("</font></strong><center></b></td>")
                    strMessage.Append("</tr>")
                    strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td Align=""Right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")
                Else
                    strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")
                    strMessage.Append("<tr>")
                    strMessage.Append("<td colspan=""11""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                    strMessage.Append("</font></strong><center></b></td>")
                    strMessage.Append("</tr>")
                    strMessage.Append("<td colspan=""11""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append("SiteWise Medical Coding")
                    strMessage.Append("</font></strong><center></b></td>")
                    strMessage.Append("</tr>")
                    strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td Align=""Right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")
                End If
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
                filename = "Medical Coding" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"
                drMedical = dtMedicalCoding.NewRow()
                drMedical("Sr. No") = i
                drMedical("Project No") = ""
                drMedical("Screening No") = ""
                If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                    drMedical("Randomization No") = ""
                Else
                    drMedical("Subject ID") = ""
                End If
                drMedical("Repeat No") = ""
                drMedical("CRF Term") = ""
                drMedical("Dictionary Code Term") = ""
                drMedical("Dictionary") = ""
                drMedical("ModifyBy") = ""
                drMedical("ModifyOn") = ""
                drMedical("Project Type Code") = ""
                dtMedicalCoding.Rows.Add(drMedical)
                dtMedicalCoding.AcceptChanges()
                gvExportToExcel.DataSource = dtMedicalCoding
                gvExportToExcel.DataBind()
                Dim stringWriter As New System.IO.StringWriter()
                Dim writer As New HtmlTextWriter(stringWriter)
                gvExportToExcel.RenderControl(writer)
                gridviewHtml = stringWriter.ToString()
                If rbtCoding.SelectedValue = "General" Then
                    strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")
                    strMessage.Append("<tr>")
                    strMessage.Append("<td colspan=""10""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                    strMessage.Append("</font></strong><center></b></td>")
                    strMessage.Append("</tr>")
                    strMessage.Append("<td colspan=""10""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append("General Medical Coding")
                    strMessage.Append("</font></strong><center></b></td>")
                    strMessage.Append("</tr>")
                    strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td Align=""Right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")
                Else
                    strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")
                    strMessage.Append("<tr>")
                    strMessage.Append("<td colspan=""11""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                    strMessage.Append("</font></strong><center></b></td>")
                    strMessage.Append("</tr>")
                    strMessage.Append("<td colspan=""11""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append("SiteWise Medical Coding")
                    strMessage.Append("</font></strong><center></b></td>")
                    strMessage.Append("</tr>")
                    strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Today.ToString("dd-MMM-yyyy") + "&nbsp;" + DateTime.Now.ToString("HH:mm") + "</b></font></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td Align=""Right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")
                End If
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
        Catch ex2 As System.Threading.ThreadAbortException
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "btnExport_Click")
        End Try
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    End Sub

    Protected Sub btnclose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnclose.Click, btnclose.Click
        Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect("frmmederaautocoding.aspx")
    End Sub

    Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click
        Dim iPeriod As String = String.Empty
        Dim ds As DataSet = New DataSet
        Dim eStr As String = ""
        Dim cSubjectWiseFlag As String = String.Empty
        Dim chCnt As Integer = 0
        Try
            iPeriod = "," + Me.ddlPeriods.SelectedValue.ToString() + ","
            If Me.ddlPeriods.SelectedValue = "All" Then
                iPeriod = ""
            End If

            If (ddlActStatus.Items.Count > 0) Then
                Me.ddlActStatus.Items.FindByValue("0").Attributes.Add("style", "font-weight:bold")
                Me.ddlActStatus.Items.FindByValue("1").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Blue))
                If ddlActStatus.Items.Count = 5 Then
                    Me.ddlActStatus.Items.FindByValue("2").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Purple))
                    Me.ddlActStatus.Items.FindByValue("3").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Green))
                    Me.ddlActStatus.Items.FindByValue("4").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Gray))
                ElseIf ddlActStatus.Items.Count = 4 Then
                    Me.ddlActStatus.Items.FindByValue("2").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Purple))
                    Me.ddlActStatus.Items.FindByValue("3").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Green))
                ElseIf ddlActStatus.Items.Count = 3 Then
                    Me.ddlActStatus.Items.FindByValue("2").Attributes.Add("style", "color:" + Drawing.ColorTranslator.ToHtml(Drawing.Color.Purple))
                End If
            End If

            If Not GetLegends() Then
                Exit Sub
            End If

            'If Me.ddlActType.SelectedValue = Sub_Specific_Activity Then
            '    'If tvSubject.Nodes(0).Checked = False Then
            '    For index = 0 To tvSubject.Nodes(0).ChildNodes.Count - 1
            '        If tvSubject.Nodes(0).ChildNodes(index).Checked = True Then
            '            strSubjectId = strSubjectId + tvSubject.Nodes(0).ChildNodes(index).Value + ","
            '        End If
            '    Next
            '    If strSubjectId <> "" Then
            '        strSubjectId.Remove(strSubjectId.Length - 1)
            '        strSubjectId = "'," + strSubjectId + "'"
            '    End If
            '    'End If
            'End If

            ''If tvActivity.Nodes(0).Checked = False Then
            'For index = 0 To tvActivity.Nodes(0).ChildNodes.Count - 1
            '    If tvActivity.Nodes(0).ChildNodes(index).Checked = True Then
            '        strParentID = strParentID + tvActivity.Nodes(0).ChildNodes(index).Value + ","
            '    End If
            'Next
            ''End If
            'If strParentID <> "" Then
            '    strParentID.Remove(strParentID.Length - 1)
            '    strParentID = "'," + strParentID + "'"
            'End If

            ''If tvActivity.Nodes(0).Checked = False Then
            'For iParent = 0 To tvActivity.Nodes(0).ChildNodes.Count - 1
            '    For iChild = 0 To tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes.Count - 1
            '        If tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes(iChild).Checked = True Then
            '            strActivityId = strActivityId + tvActivity.Nodes(0).ChildNodes(iParent).ChildNodes(iChild).Value + ","
            '        End If
            '    Next
            'Next
            ''End If

            'If strActivityId <> "" Then
            '    strActivityId.Remove(strActivityId.Length - 1)
            '    strActivityId = "'," + strActivityId + "'"
            'End If

            If Not FILLGRIDVIEWCRF1(eStr) Then
                Throw New Exception(eStr)
                Exit Sub
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "..............btnGo_Click")
        Finally
            ds.Dispose()
        End Try
    End Sub

#End Region

#Region "Reset Page"
    Private Sub ResetPage()

    End Sub
#End Region

#Region "Reset Page"
    Private Function getCRFVersion() As Char
        Dim ds_CrfVersionDetail As DataSet = Nothing
        Dim wstr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Try

            ''====== CRFVersion Control==================================

            wstr = "vWorkSpaceId = '" + Me.HProjectId.Value.ToString() + "'"
            If Not objHelp.GetData("View_CRFVersionForDataEntryControl", "*", wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CrfVersionDetail, eStr_Retu) Then
                Throw New Exception("Error While Getting Status For CrfVersion")
            End If

            If Not ds_CrfVersionDetail Is Nothing AndAlso ds_CrfVersionDetail.Tables(0).Rows.Count > 0 Then
                If ds_CrfVersionDetail.Tables(0).Rows(0)("cFreezeStatus").ToString.Trim.ToUpper = "U" Then
                    getCRFVersion = "U" 'U=UnFreeze
                    Exit Function
                End If
                getCRFVersion = "F" 'F=Freeze
            Else
                getCRFVersion = "N" 'N=Not Applicable(For This Project There is No Data Entry In CRFVersionTable)
            End If
            '==========================================================
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString(), "...getCRFVersion")
            Return ""
        End Try

    End Function

#End Region

#Region "Error Handler"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
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
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()

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
