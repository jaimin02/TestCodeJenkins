Imports System.IO
Imports System.Drawing

Partial Class frmScreeningDCFReport
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_nDCFNo As Integer = 1
    Private Const GVC_ProjectNo As Integer = 2
    Private Const GVC_GroupName As Integer = 3
    Private Const GVC_SubjectId As Integer = 4
    Private Const GVC_ScreenDate As Integer = 5
    Private Const GVC_Attribute As Integer = 6
    Private Const GVC_Discrepancy As Integer = 7
    Private Const GVC_DcfType As Integer = 8
    Private Const GVC_Query As Integer = 9
    Private Const GVC_ModifyValue As Integer = 10
    Private Const GVC_ModifyRemarks As Integer = 11
    Private Const GVC_ModifyDate As Integer = 12
    Private Const GVC_ModifyBy As Integer = 13
    Private Const GVC_DCFRemarks As Integer = 14
    Private Const GVC_DCFCreatedby As Integer = 15
    Private Const GVC_DCFCreatedDate As Integer = 16
    Private Const GVC_Status As Integer = 17
    Private Const GVC_DataEntryBy As Integer = 18
    Private Const GVC_DCFUpdatedOn As Integer = 19
    Private Const GVC_Edit As Integer = 20
    Private Const GVC_GroupCode As Integer = 21
    Private Const GVC_Resolved As Integer = 22
    Private Const GVC_DCFby As Integer = 23
    Private Const GVC_DCFWORKFLOW As Integer = 24
    Private Const GVC_DCFUserType As Integer = 25
    Private Const GVC_SCrenDate As Integer = 26



    Private Const GVCPS_SrNo As Integer = 0
    Private Const GVCPS_nDCFNo As Integer = 1
    Private Const GVCps_ProjectNo As Integer = 2
    Private Const GVCps_GroupName As Integer = 3
    Private Const GVCps_SubjectId As Integer = 4
    Private Const GVCps_ScreenDate As Integer = 5
    Private Const GVCps_Attribute As Integer = 6
    Private Const GVCps_Discrepancy As Integer = 7
    Private Const GVCps_DcfType As Integer = 8
    Private Const GVCps_Query As Integer = 9
    Private Const GVCps_ModifyValue As Integer = 10
    Private Const GVCps_ModifyRemarks As Integer = 11
    Private Const GVCps_ModifyDate As Integer = 12
    Private Const GVCps_ModifyBy As Integer = 13
    Private Const GVCps_DCFRemarks As Integer = 14
    Private Const GVCps_DCFCreatedby As Integer = 15
    Private Const GVCps_DCFCreatedDate As Integer = 16
    Private Const GVCps_Status As Integer = 17
    Private Const GVCps_DataEntryBy As Integer = 18
    Private Const GVCps_DCFUpdatedOn As Integer = 19
    Private Const GVCps_Edit As Integer = 20
    Private Const GVCps_GroupCode As Integer = 21
    Private Const GVCps_Resolved As Integer = 22
    Private Const GVCPs_DCFby As Integer = 23    Private Const GVCPs_DCFWORKFLOW As Integer = 24    Private Const GVCPs_DCFUserType As Integer = 25
    Private Const GVCPs_SCreendatenotDisplay As Integer = 26

#End Region

#Region "Page Load "
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                GenCall()
            End If
            Me.Session(S_ScrProfileIndex) = hdnSelectedIndex.Value
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....Page_Load")
        End Try
    End Sub
#End Region

#Region "GenCall"
    Private Function GenCall() As Boolean
        Try
            If Not GenCall_ShowUI() Then
                Exit Function
            End If

            GenCall = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....GenCall")
        Finally
        End Try
    End Function
#End Region

#Region "GenCall_showUI "
    Private Function GenCall_ShowUI() As Boolean
        Try
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Screening DCF Report"
            Page.Title = ":: Screening DCF Report :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            'Me.AutoCompleteExtender1.ContextKey = Me.Session(S_LocationCode)
            'Vineet'
            If System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge").Contains(Session(S_LocationCode)) = True Then
                Me.AutoCompleteExtender1.ContextKey = Me.Session(S_LocationCode)
            Else
                Me.AutoCompleteExtender1.ContextKey = System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge")
                Me.AutoCompleteExtender1.ServiceMethod = "GetSubjectCompletionList_NotRejectedDataMerg"
            End If
                Me.AutoCompleteExtender2.ContextKey = "iUserId = " & Me.Session(S_UserID)
                If Not FillGroup() Then
                    Me.ShowErrorMessage("", ".....Fill Group")
                End If
                GenCall_ShowUI = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....GenCall_ShowUI")
        Finally
        End Try
    End Function
#End Region

#Region "Button CLick Event"
    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Try
            Me.txtProject.Text = ""
            Me.txtSubject.Text = ""
            Me.txtFromDate.Text = ""
            Me.txtToDate.Text = ""
            Me.ddlGroup.Items.Clear()
            chkSelectAllSubjects.Checked = False
            'Me.ddlDCFType.Items.Clear()
            Me.ddlGeneratedBy.Items.Clear()
            Me.ddlResolvedBy.Items.Clear()
            chklstSubjects.Items.Clear()
            'ddlResolvedBy.SelectedIndex = 0
            ddlDCFType.SelectedIndex = 0
            'ddlGeneratedBy.SelectedIndex = 0

            HProjectId.Value = ""
            HSubjectId.Value = ""
            gvScreeningSubjectRepprt.DataSource = Nothing
            gvScreeningSubjectRepprt.DataBind()
            gvScreeningProjectSpecific.DataSource = Nothing
            gvScreeningProjectSpecific.DataBind()
            chkRereview.Checked = False
            upScreeningSubjectReport.Update()
            btnExportToExcel.Visible = False
            gvExport.DataSource = Nothing
            gvExport.DataBind()

        Catch ex As Exception
            Throw New Exception("Error while btnCancel_Click")
        End Try
    End Sub

    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        HProjectId.Value = ""
        HSubjectId.Value = ""
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btnGo_Click(Senser As Object, e As EventArgs) Handles btnGo.Click
        Try
            If Not FillGrid() Then
                Exit Sub
            End If
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub btnExportToExcel_Click(Senser As Object, e As EventArgs) Handles btnExportToExcel.Click
        Dim ds_ExpotrToExcel As DataSet
        Dim filename As String
        Dim strMessage As New StringBuilder
        Try
            Dim style As String = "<style> .textmode { } </style>"
            Response.Write(style)
            Dim info As String = String.Empty
            Dim gridviewHtml As String = String.Empty

            filename = "Screening DCF Report.Xls"

            Dim stringWriter As New System.IO.StringWriter()

            Dim writer As New HtmlTextWriter(stringWriter)
            gvExport.RenderControl(writer)
            gridviewHtml = stringWriter.ToString()

            strMessage.Append("<table width=""100%""  cellpadding=""0"" cellspacing=""0"">")

            strMessage.Append("<tr>")
            strMessage.Append("<td colspan=""17""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
            strMessage.Append("</font></strong><center></b></td>")
            strMessage.Append("</tr>")


            strMessage.Append("<td colspan=""17""><center><strong><b><font color=""#000099"" size=""3.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
            strMessage.Append("Screening DCF Report")
            strMessage.Append("</font></strong><center></b></td>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr><td><font color=""#000099"" size=""2"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""2"" face=""Verdana""><b>" + DateTime.Now.ToString("dd-MMM-yyyy") + "</b></font></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td Align=""Right""><font color=""#000099"" size=""2"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""2"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")


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

        Catch ex As Exception

        End Try

    End Sub

    Protected Sub btnSubject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubject.Click
        Try
            If Not FillDropDown() Then
                Me.ShowErrorMessage("Error While Fill DropDown", "")
            End If

            If Not FillGroup() Then
                Me.ShowErrorMessage("Error While Fill Group", "")
            End If

        Catch ex As Exception
            Me.ShowErrorMessage("Error While btnSetProject", "")
        End Try

    End Sub

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Try
            If Not FillDropDown() Then
                Me.ShowErrorMessage("Error While Fill DropDown", "")
            End If

            If Not FillGroup() Then
                Me.ShowErrorMessage("Error While Fill DropDown", "")
            End If

            If Not BindSubjectTree() Then
                Me.ShowErrorMessage("Error While Fill Subject", "")
            End If


        Catch ex As Exception
            Me.ShowErrorMessage("Error While btnSetProject", "")
        End Try

    End Sub

    Protected Sub Btn_Resolve_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn_Resolve.Click
        Dim nDcfNo As Integer = Me.HFnDCFNo.Value.Trim.ToString()
        'Dim objLambda = New WS_Lambda.WS_Lambda
        'Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()
        Dim eStr As String = Nothing
        Dim wStr As String = Nothing
        Dim ds_DCF As DataSet = Nothing

        Try

            wStr = "SELECT * FROM ScreeningDCfMst  WHERE nScreeningDCFNo = " + Convert.ToString(nDcfNo)
            ds_DCF = objHelp.GetResultSet(wStr, "ScreeningDCFMst")

            For Each dr As DataRow In ds_DCF.Tables(0).Rows
                dr("cDCFStatus") = Discrepancy_Resolved
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("iStatusChangedBy") = Me.Session(S_UserID)
            Next dr
            ds_DCF.AcceptChanges()

            If Not ds_DCF Is Nothing AndAlso ds_DCF.Tables(0).Rows.Count > 0 Then
                If Not objLambda.Save_ScreeningDCFMST(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_DCF, eStr) Then
                    Me.ShowErrorMessage("Error While Save in ScreeningDCFMST", "")
                End If

                Me.objCommon.ShowAlert("Discrepancy Resolved Successfully.", Me.Page)
            End If

            Me.HFnDCFNo.Value = ""

            If Not FillGrid() Then
                Throw New Exception
            End If
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "Radio button Event"
    Protected Sub rblScreeningType_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            If rblScreeningType.SelectedItem.Value = "1" Then
                txtFromDate.Text = ""
                txtToDate.Text = ""
                ddlDCFType.SelectedIndex = 0
                chkSelectAllSubjects.Visible = False
                txtSubject.Visible = True
                pnlSubjects.Visible = False
                lblSubject.Text = "Subject Name*:"
                rwSubject.Visible = True
                rwProject.Visible = False
                gvScreeningSubjectRepprt.DataSource = Nothing
                gvScreeningSubjectRepprt.DataBind()
                gvScreeningProjectSpecific.DataSource = Nothing
                gvScreeningProjectSpecific.DataBind()
                txtProject.Text = ""
                HSubjectId.Value = ""
                HProjectId.Value = ""
                txtSubject.Text = ""
                btnExportToExcel.Visible = False
                upScreeningDCF.Update()
                upScreeningSubjectReport.Update()
                'gvScreeningProjectSpecific.Visible = False
                'gvScreeningSubjectRepprt.Visible = True
            ElseIf rblScreeningType.SelectedItem.Value = "2" Then
                txtFromDate.Text = ""
                txtToDate.Text = ""
                ddlDCFType.SelectedIndex = 0
                chkSelectAllSubjects.Visible = True
                pnlSubjects.Visible = True
                txtSubject.Visible = False
                txtProject.Text = ""
                HProjectId.Value = ""
                lblSubject.Text = "Subject Name :"
                rwProject.Visible = True
                txtSubject.Text = ""
                HSubjectId.Value = ""
                HProjectId.Value = ""
                chkSelectAllSubjects.Checked = False
                chklstSubjects.Items.Clear()
                gvScreeningSubjectRepprt.DataSource = Nothing
                gvScreeningSubjectRepprt.DataBind()
                gvScreeningProjectSpecific.DataSource = Nothing
                gvScreeningProjectSpecific.DataBind()
                ddlGroup.Items.Clear()
                ddlGeneratedBy.Items.Clear()
                ddlResolvedBy.Items.Clear()
                btnExportToExcel.Visible = False
                upScreeningDCF.Update()
                upScreeningSubjectReport.Update()
                'gvScreeningProjectSpecific.Visible = True
                'gvScreeningSubjectRepprt.Visible = False
            ElseIf rblScreeningType.SelectedItem.Value = "3" Then
                txtFromDate.Text = ""
                txtToDate.Text = ""
                ddlDCFType.SelectedIndex = 0
                ddlGroup.Items.Clear()
                ddlGeneratedBy.Items.Clear()
                ddlResolvedBy.Items.Clear()
                txtProject.Text = ""
                chklstSubjects.Items.Clear()
                HProjectId.Value = ""
                chkSelectAllSubjects.Checked = False
                txtSubject.Visible = False
                chkSelectAllSubjects.Visible = True
                pnlSubjects.Visible = True
                lblSubject.Text = "Subject Name :"
                rwProject.Visible = True
                txtSubject.Text = ""
                txtProject.Text = ""
                HSubjectId.Value = ""
                HProjectId.Value = ""
                gvScreeningSubjectRepprt.DataSource = Nothing
                gvScreeningSubjectRepprt.DataBind()
                gvScreeningProjectSpecific.DataSource = Nothing
                gvScreeningProjectSpecific.DataBind()
                btnExportToExcel.Visible = False
                upScreeningDCF.Update()
                upScreeningSubjectReport.Update()
            End If

            If Not FillGroup() Then
                ShowErrorMessage("", "Error While Fill Group")
                Exit Sub
            End If
        Catch ex As Exception
        End Try
    End Sub
#End Region

#Region "Fill Function"
    Public Function FillGrid() As Boolean
        Dim wStr As String
        Dim ds_ScreeningReport As DataSet
        Dim ReReviewed As String
        Dim Subjects As String = ""

        Try
            gvScreeningProjectSpecific.DataSource = Nothing
            gvScreeningProjectSpecific.DataBind()
            gvScreeningSubjectRepprt.DataSource = Nothing
            gvScreeningSubjectRepprt.DataBind()
            upScreeningSubjectReport.Update()
            upScreeningDCF.Update()

            If chkRereview.Checked = True Then
                ReReviewed = "Y"
            Else
                ReReviewed = " "
            End If

            For index As Integer = 0 To Me.chklstSubjects.Items.Count - 1
                If Me.chklstSubjects.Items(index).Selected Then
                    If Subjects.Trim() <> "" OrElse Subjects <> String.Empty Then
                        If Me.chklstSubjects.Items(index).Value.Trim() <> "" Then
                            Subjects += ","
                        End If
                    End If
                    If Me.chklstSubjects.Items(index).Value.Trim() <> "" Then
                        Subjects += "" + Me.chklstSubjects.Items(index).Value.Trim() + ""
                    End If
                End If
            Next


            If rblScreeningType.SelectedValue = "1" Then

                wStr = Me.HSubjectId.Value + "##" + txtFromDate.Text + "##" + txtToDate.Text + "##" + ddlGroup.SelectedValue + "##" + ddlDCFType.SelectedValue + "##" + ddlGeneratedBy.SelectedValue + "##" + ddlResolvedBy.SelectedValue + "##" + IIf(Me.HProjectId.Value = "", "00000000", Me.HProjectId.Value) + "##" + ReReviewed
                ds_ScreeningReport = objHelp.ProcedureExecute("Proc_ScreeningDCFReport", wStr)

                If Not ds_ScreeningReport Is Nothing AndAlso ds_ScreeningReport.Tables(0).Rows.Count Then
                    gvScreeningSubjectRepprt.DataSource = ds_ScreeningReport.Tables(0)
                    gvScreeningSubjectRepprt.DataBind()
                    gvExport.DataSource = ds_ScreeningReport.Tables(0)
                    gvExport.DataBind()
                    upScreeningDCF.Update()
                    upScreeningSubjectReport.Update()
                    btnExportToExcel.Visible = True
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "DispalyDCFReport", "DispalyDCFReport(ctl00_CPHLAMBDA_imgfldgen,'tblEntryData');", True)
                Else
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "alert11", "msgalert('No Record Found.');", True)
                End If
            ElseIf (rblScreeningType.SelectedValue = "2") Then

                wStr = Subjects + "##" + txtFromDate.Text + "##" + txtToDate.Text + "##" + ddlGroup.SelectedValue + "##" + ddlDCFType.SelectedValue + "##" + ddlGeneratedBy.SelectedValue + "##" + ddlResolvedBy.SelectedValue + "##" + IIf(Me.HProjectId.Value = "", "00000000", Me.HProjectId.Value) + "##" + ReReviewed
                ds_ScreeningReport = objHelp.ProcedureExecute("Proc_ProjectScreeningDCFReport", wStr)

                If Not ds_ScreeningReport Is Nothing AndAlso ds_ScreeningReport.Tables(0).Rows.Count Then
                    gvScreeningProjectSpecific.DataSource = ds_ScreeningReport.Tables(0)
                    gvScreeningProjectSpecific.DataBind()
                    gvExport.DataSource = ds_ScreeningReport.Tables(0)
                    gvExport.DataBind()
                    upScreeningDCF.Update()
                    upScreeningSubjectReport.Update()
                    btnExportToExcel.Visible = True
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "DispalyDCFReport", "DispalyDCFReport(ctl00_CPHLAMBDA_imgfldgen,'tblEntryData');", True)
                Else
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "alert1", "msgalert('No Record Found.');", True)
                End If

            ElseIf (rblScreeningType.SelectedValue = "3") Then
                wStr = Subjects + "##" + txtFromDate.Text + "##" + txtToDate.Text + "##" + ddlGroup.SelectedValue + "##" + ddlDCFType.SelectedValue + "##" + ddlGeneratedBy.SelectedValue + "##" + ddlResolvedBy.SelectedValue + "##" + IIf(Me.HProjectId.Value = "", "00000000", Me.HProjectId.Value) + "##" + ReReviewed
                ds_ScreeningReport = objHelp.ProcedureExecute("Proc_ProjectSpecificDCFReport", wStr)

                If Not ds_ScreeningReport Is Nothing AndAlso ds_ScreeningReport.Tables(0).Rows.Count Then
                    gvScreeningProjectSpecific.DataSource = ds_ScreeningReport.Tables(0)
                    gvScreeningProjectSpecific.DataBind()
                    gvExport.DataSource = ds_ScreeningReport.Tables(0)
                    gvExport.DataBind()
                    upScreeningDCF.Update()
                    upScreeningSubjectReport.Update()
                    btnExportToExcel.Visible = True
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "DispalyDCFReport", "DispalyDCFReport(ctl00_CPHLAMBDA_imgfldgen,'tblEntryData');", True)
                Else
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "alert1", "msgalert('No Record Found.');", True)
                End If


            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("Error While Filling Grid. ", ex.Message)
            Return False
        End Try
    End Function

    Public Function FillGroup() As Boolean
        Dim ds_Group As DataSet
        Try
            ddlGroup.DataSource = Nothing
            ddlGroup.DataBind()
            ddlGroup.Items.Insert(0, New ListItem("Select Group", "0"))
            upScreeningDCF.Update()
            upScreeningSubjectReport.Update()

            If rblScreeningType.SelectedValue = "1" Then
                ds_Group = Me.objHelp.ProcedureExecute("dbo.Proc_GetGroupFetch", "0000000000")
            ElseIf rblScreeningType.SelectedValue = "2" Then
                ds_Group = Me.objHelp.ProcedureExecute("dbo.Proc_GetGroupFetch", HProjectId.Value)
            ElseIf rblScreeningType.SelectedValue = "3" Then
                ds_Group = Me.objHelp.ProcedureExecute("dbo.Proc_GetGroupFetch", "0000000000")
            End If
            If Not ds_Group Is Nothing AndAlso ds_Group.Tables(0).Rows.Count > 0 Then
                ddlGroup.Items.Clear()
                ddlGroup.DataSource = ds_Group.Tables(0)
                ddlGroup.DataValueField = "vMedExGroupCode"
                ddlGroup.DataTextField = "vmedexgroupDesc"
                ddlGroup.DataBind()
                ddlGroup.Items.Insert(0, New ListItem("Select Group", "0"))
                upScreeningDCF.Update()
                upScreeningSubjectReport.Update()
            End If
            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....Fill Group")
            Return False
        End Try
    End Function

    Public Function FillDropDown() As Boolean
        Dim ds_Screening As DataSet
        Dim wStr As String = ""
        Try

            If rblScreeningType.SelectedValue = "1" Then
                wStr = HSubjectId.Value + "##" + "" + "##" + "0000000000" + "##" + "N"
                ds_Screening = Me.objHelp.ProcedureExecute("dbo.Proc_ScreeningDCFBYANDOn", wStr)

                
            ElseIf rblScreeningType.SelectedValue = "2" Then
                wStr = IIf(HSubjectId.Value = "", "--", HSubjectId.Value) + "##" + "--" + "##" + HProjectId.Value + "" + "##" + "Y"
                ds_Screening = Me.objHelp.ProcedureExecute("dbo.Proc_ScreeningDCFBYANDOn", wStr)
            ElseIf rblScreeningType.SelectedValue = "3" Then
                wStr = IIf(HSubjectId.Value = "", "--", HSubjectId.Value) + "##" + "--" + "##" + HProjectId.Value + "" + "##" + "N"
                ds_Screening = Me.objHelp.ProcedureExecute("dbo.Proc_ScreeningDCFBYANDOn", wStr)
            End If

            If Not ds_Screening Is Nothing AndAlso ds_Screening.Tables(0).Rows.Count > 0 Then

                ddlGeneratedBy.DataSource = ds_Screening.Tables(0).DefaultView().ToTable(True, ("iCreatedby,CreatedBy").Split(",")).DefaultView()
                ddlGeneratedBy.DataValueField = "iCreatedby"
                ddlGeneratedBy.DataTextField = "CreatedBy"
                ddlGeneratedBy.DataBind()
                ddlGeneratedBy.Items.Insert(0, New ListItem("Select Generated by", "0"))


                ddlResolvedBy.DataSource = ds_Screening.Tables(0).DefaultView().ToTable(True, ("iAnsweredby,AnsweredBy").Split(",")).DefaultView()
                ddlResolvedBy.DataValueField = "iAnsweredby"
                ddlResolvedBy.DataTextField = "AnsweredBy"
                ddlResolvedBy.DataBind()
                ddlResolvedBy.Items.Insert(0, New ListItem("Select Resolved by", "0"))

                upScreeningDCF.Update()
                upScreeningSubjectReport.Update()
            End If
            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....Fill DropDown")
            Return False
        End Try

    End Function

    Protected Function BindSubjectTree() As Boolean
        Dim eStr As String = String.Empty
        Dim ds_Subjects As New DataSet
        Dim wStr As String = String.Empty
        Dim lItems As ListItem

        Try

            If rblScreeningType.SelectedItem.Value = "3" Then
                wStr = " vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D'  "
                wStr += " order by iMySubjectNo"

                If Not objHelp.GetViewWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                        ds_Subjects, eStr) Then
                    Throw New Exception(eStr)
                End If

            Else
                wStr = HProjectId.Value + "##"
                ds_Subjects = objHelp.ProcedureExecute("Proc_ProjectSpecificSCreening", wStr)


            End If

            Me.chklstSubjects.Items.Clear()

            For Each dr As DataRow In ds_Subjects.Tables(0).Rows
                lItems = New ListItem
                lItems.Text = Convert.ToString(dr("vSubjectId")).Trim()
                lItems.Value = dr("vSubjectId")
                Me.chklstSubjects.Items.Add(lItems)
            Next

            Return True
        Catch ex As Exception

            Return False
        Finally
        End Try
    End Function

#End Region

#Region "GridView Event"
    Protected Sub gvScreeningProjectSpecific_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvScreeningProjectSpecific.RowCreated

        If e.Row.RowType = DataControlRowType.DataRow Or _
                   e.Row.RowType = DataControlRowType.Header Or _
                   e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVCPS_nDCFNo).Visible = False
            e.Row.Cells(GVCps_GroupCode).Visible = False
            e.Row.Cells(GVCPs_DCFby).Visible = False
            e.Row.Cells(GVCPs_DCFWORKFLOW).Visible = False
            e.Row.Cells(GVCPs_DCFUserType).Visible = False
            e.Row.Cells(GVCPs_SCreendatenotDisplay).Visible = False

        End If
    End Sub

    Protected Sub gvExport_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvExport.RowCreated

        If e.Row.RowType = DataControlRowType.DataRow Or _
                   e.Row.RowType = DataControlRowType.Header Or _
                   e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(1).Visible = False
        End If
    End Sub


    Protected Sub gvScreeningProjectSpecific_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvScreeningProjectSpecific.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Text = e.Row.RowIndex + 1

            CType(e.Row.FindControl("lnkEditForProjectSpecific"), ImageButton).CommandName = "EDIT"
            CType(e.Row.FindControl("lnkEditForProjectSpecific"), ImageButton).CommandArgument = e.Row.RowIndex
            Dim profile As String = CType(Me.Master.FindControl("ddlProfile"), DropDownList).SelectedValue

            Dim RedirectStr As String = "frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000&Set=true&ScrDt=" & Date.Parse(Convert.ToString(e.Row.Cells(GVCPs_SCreendatenotDisplay).Text).ToString()).ToString("M/d/yyyy h:mm:ss tt") & _
                                        "&SubId=" & e.Row.Cells(GVC_SubjectId).Text.Trim() & _
                                        "&Group=" & e.Row.Cells(GVC_GroupCode).Text.Trim() & _
                                        "&Profile=" & profile

            CType(e.Row.FindControl("lnkEditForProjectSpecific"), ImageButton).OnClientClick = "return OpenWindow('" + RedirectStr + "');"

            e.Row.Cells(GVC_Resolved).FindControl("lnkResolve").Visible = False
            If e.Row.Cells(GVC_DcfType).Text.ToString().ToUpper() = "MANUAL" AndAlso e.Row.Cells(GVC_Status).Text.ToString().ToUpper() <> "RESOLVED" Then
                If Convert.ToString(e.Row.Cells(GVC_DCFWORKFLOW).Text) = Convert.ToString(Session(S_WorkFlowStageId)) Then
                    e.Row.Cells(GVC_Resolved).FindControl("lnkResolve").Visible = True
                    CType(e.Row.Cells(GVC_Resolved).FindControl("lnkResolve"), LinkButton).OnClientClick = "return ShowConfirmation(" & e.Row.Cells(GVC_nDCFNo).Text.Trim.ToString() & ");"
                    'ElseIf System.Configuration.ConfigurationManager.AppSettings("ScreeningIndependentReviewer").Contains(Session(S_UserType)) Then
                    '    e.Row.Cells(GVC_Resolved).FindControl("lnkResolve").Visible = True
                    '    CType(e.Row.Cells(GVC_Resolved).FindControl("lnkResolve"), LinkButton).OnClientClick = "return ShowConfirmation(" & e.Row.Cells(GVC_nDCFNo).Text.Trim.ToString() & ");"
                End If
            End If



        End If

    End Sub

    Protected Sub gvScreeningSubjectRepprt_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvScreeningSubjectRepprt.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or _
                   e.Row.RowType = DataControlRowType.Header Or _
                   e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVC_nDCFNo).Visible = False
            e.Row.Cells(GVC_GroupCode).Visible = False
            e.Row.Cells(GVC_DCFby).Visible = False
            e.Row.Cells(GVC_DCFWORKFLOW).Visible = False
            e.Row.Cells(GVC_DCFUserType).Visible = False
            e.Row.Cells(GVC_SCrenDate).Visible = False

        End If
    End Sub

    Protected Sub gvScreeningSubjectRepprt_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvScreeningSubjectRepprt.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + 1
            Dim profile As String = CType(Me.Master.FindControl("ddlProfile"), DropDownList).SelectedValue
            CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandName = "EDIT"
            CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandArgument = e.Row.RowIndex

            Dim RedirectStr As String = "frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000&Set=true&ScrDt=" & Date.Parse(Convert.ToString(e.Row.Cells(GVC_SCrenDate).Text).ToString()).ToString("M/d/yyyy h:mm:ss tt") & _
                                        "&SubId=" & e.Row.Cells(GVC_SubjectId).Text.Trim() & _
                                        "&Group=" & e.Row.Cells(GVC_GroupCode).Text.Trim() & _
                                        "&Profile=" & profile

            CType(e.Row.FindControl("lnkEdit"), ImageButton).OnClientClick = "return OpenWindow('" + RedirectStr + "');"

            e.Row.Cells(GVC_Resolved).FindControl("lnkResolve").Visible = False
            If e.Row.RowType = DataControlRowType.DataRow Then
                If e.Row.Cells(GVC_DcfType).Text.ToString().ToUpper() = "MANUAL" AndAlso e.Row.Cells(GVC_Status).Text.ToString().ToUpper() <> "RESOLVED" Then

                    If Convert.ToString(e.Row.Cells(GVC_DCFWORKFLOW).Text) = Convert.ToString(Session(S_WorkFlowStageId)) Then
                        CType(e.Row.Cells(GVC_Resolved).FindControl("lnkResolve"), LinkButton).OnClientClick = "return ShowConfirmation(" & e.Row.Cells(GVC_nDCFNo).Text.Trim.ToString() & ");"
                        e.Row.Cells(GVC_Resolved).FindControl("lnkResolve").Visible = True
                        'ElseIf System.Configuration.ConfigurationManager.AppSettings("ScreeningIndependentReviewer").Contains(Session(S_UserType)) Then
                        '    CType(e.Row.Cells(GVC_Resolved).FindControl("lnkResolve"), LinkButton).OnClientClick = "return ShowConfirmation(" & e.Row.Cells(GVC_nDCFNo).Text.Trim.ToString() & ");"
                        '    e.Row.Cells(GVC_Resolved).FindControl("lnkResolve").Visible = True
                    End If

                End If
            End If



        End If

    End Sub
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

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    End Sub

End Class
