
Partial Class frmActivitySelectionforBunch
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION"
    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()
    Dim eStr As String
#End Region

#Region "Page Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not GenCall() Then
                Exit Sub
            End If
        End If
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
            ShowErrorMessage(ex.Message, "......GenCall")
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_ShowUI"
    Private Function GenCall_ShowUI() As Boolean
        Dim sender As New Object
        Dim e As New EventArgs

        Try
            Page.Title = " :: Activity Bunch Enrollment ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Master.FindControl("lblHeading"), Label).Text = "Activity Bunch Enrollment"

            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)
            If String.IsNullOrEmpty(Request.QueryString("WorkSpaceId")) AndAlso String.IsNullOrEmpty(Request.QueryString("SubjectId")) Then
                btnSetProject_Click(sender, e)
            Else
                Dim WStr As String = String.Empty
                Dim ds As New DataSet
                Me.HProjectId.Value = Me.Request.QueryString("WorkSpaceId").ToString()
                Me.HSubject.Value = Me.Request.QueryString("SubjectId").ToString()
                Me.txtproject.Text = Me.objHelp.GetProjectName(Me.HProjectId.Value.Trim(), Me.Session(S_UserID))
                Me.txtproject.Enabled = False
                Me.txtSubject.Text = Me.objHelp.GetSubjectNo(Me.HProjectId.Value.Trim(), Me.HSubject.Value.Trim())
                Me.txtSubject.Enabled = False
                Me.AutoCompleteExtender2.ContextKey = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "'"

                'Check User Rights For logged UserId, WorkspaceId and SubjectId
                WStr = " vWorkspaceId='" + Me.HProjectId.Value.Trim() + "' And vSubjectId='" + Me.HSubject.Value.Trim() + "' And iUserId=" + Me.Session(S_UserID) + ""
                If Not Me.objHelp.GetViewWorkSpaceUserNodeDetail(WStr, ds, eStr) Then
                    ShowErrorMessage(eStr, ".....GenCall_ShowUI")
                    Return False
                End If

                btnExit.Visible = False
                btnExitView.Visible = True

                If ds.Tables(0).Rows.Count = 0 Then
                    btnSave.Visible = False
                    objcommon.ShowAlert("User have No Activity Rights.", Me.Page)
                    Return False
                Else
                    btnSave.Visible = True
                End If

                btnSetSubject_Click(sender, e)
            End If

            GenCall_ShowUI = True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....GenCall_ShowUI")
        Finally
            'ds_Workspace.Dispose()
        End Try

    End Function
#End Region

#Region "Fill Visit"
    Public Function FillVisitDropDown() As String
        Dim pHProjectId As String = Me.HProjectId.Value.Trim()
        Dim pSubjectId As String = Me.HSubject.Value.Trim()
        Dim ds_Visit As New DataSet

        Try
            ds_Visit = Me.objHelp.GetVisitList(pHProjectId, pSubjectId)
            ChkBoxLstActivity.Items.Clear()
            If ds_Visit.Tables(0).Rows.Count > 0 Then
                For Index = 0 To ds_Visit.Tables(0).Rows.Count - 1 Step 1
                    ChkBoxLstActivity.Items.Add(New ListItem(Convert.ToString(ds_Visit.Tables(0).Rows(Index).Item("vNodeDisplayName")).Trim(), Convert.ToString(ds_Visit.Tables(0).Rows(Index).Item("iNodeId")).Trim()))
                Next
                LblActivity.Text = "Activity :"
                LblActivity.Visible = True
                PnlActivity.Visible = True
                ChkBoxLstActivity.Visible = True
                Return True
            Else
                objcommon.ShowAlert("No Pending Visit Found!", Me.Page)
                LblActivity.Text = ""
                LblActivity.Visible = False
                PnlActivity.Visible = False
                ChkBoxLstActivity.Visible = False
                Return False
            End If

        Catch ex As Exception
            ShowErrorMessage("Problem While Filling Activity. ", ex.Message)
            Return False
        End Try
    End Function
#End Region

#Region "Helper Functions/Subs"
    Protected Sub GvwDtl_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim index As Int32
        Dim IndexDelete As Int32
        Dim DTDELETE As New DataTable

        Try
            index = CInt(e.CommandArgument)
            DTDELETE = CType(Me.Session("DeleteRecords"), DataTable)

            For IndexDelete = 0 To DTDELETE.Rows.Count - 1
                If Convert.ToString(DTDELETE.Rows(IndexDelete).Item("vActivity")).Trim = Replace(GvwDtl.Rows(index).Cells(1).Text, "&nbsp;", " ").Trim Then
                    Dim iBunchNo As String = String.Empty
                    Dim iNodeId As String = String.Empty

                    iNodeId = (DTDELETE.Rows(IndexDelete).Item("iNodeId")).ToString().Trim()
                    iBunchNo = (DTDELETE.Rows(IndexDelete).Item("iBunchId")).ToString().Trim()

                    If Not Me.objHelp.CRFBunch_Validation(Me.HProjectId.Value.Trim(), Me.HSubject.Value.Trim(), iBunchNo, iNodeId, eStr) Then
                        objcommon.ShowAlert(eStr, Me.Page)
                        Exit Sub
                    Else
                        DTDELETE.Rows(IndexDelete).Item("cStatusIndi") = "D"
                        DTDELETE.Rows(IndexDelete).Item("DataMode") = "D"
                        DTDELETE.AcceptChanges()
                        Exit For
                    End If
                End If
            Next IndexDelete

            Dim ds_Save As New DataSet
            Dim dr_Delete = DTDELETE.[Select]("DataMode='D'")
            If dr_Delete.Length > 0 Then
                DTDELETE = dr_Delete.CopyToDataTable()
            End If
            Dim dtCopy As DataTable = DTDELETE.Copy()
            dtCopy.TableName = "CRFBunchHdrTable"
            ds_Save.Tables.Add(dtCopy)

            If Not Me.objLambda.Save_CRFBunchData(ds_Save, Me.Session(S_UserID), eStr) Then
                objcommon.ShowAlert("Error While Deleting Records", Me.Page)
                Exit Sub
            Else
                objcommon.ShowAlert("Deleted Successfully.", Me.Page)
                Me.ResetPage()
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".......GvwDtl_RowCommand")
        End Try
    End Sub

    Protected Sub GvwDtl_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("ImbADelete"), ImageButton).CommandName = "ADelete"
            CType(e.Row.FindControl("ImbADelete"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("ImbADelete"), ImageButton).Attributes.Add("OnClick", "return show_confirm()")
        End If
    End Sub

    Protected Sub GvwDtl_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs)

    End Sub

    Private Sub MakeTemplate()
        Dim ds_GetTemplateDetails As DataSet
        Dim IndexForTemplate As Integer

        Dim ds_Visit As DataSet = Me.objHelp.GetVisitList(Me.HProjectId.Value.Trim(), Me.HSubject.Value.Trim())
        If ds_Visit.Tables(0).Rows.Count > 0 Then
            PlaceMedEx.Controls.Add(New LiteralControl("<table id='tblMain' runat='server' style=""width: 100%; height: 150px; margin-top: 15px; border:1px Navy solid"">"))
            Dim Finalstring As String = String.Empty
            Dim FianlMedExCd As String = String.Empty
            Dim FianlMedExDesc As String = String.Empty
            Dim Id As Integer = 1
            ds_GetTemplateDetails = Me.objHelp.GetTemplateDetails(Me.HProjectId.Value.Trim())
            If ds_GetTemplateDetails.Tables(0).Rows.Count > 0 Then
                For IndexForTemplate = 0 To ds_GetTemplateDetails.Tables(0).Rows.Count - 1
                    Dim Rdvalue As String = String.Empty
                    Dim MedExCode As String = String.Empty
                    Dim MedExDesc As String = String.Empty
                    Dim Questring As String = String.Empty
                    Dim Radiostring As String = String.Empty
                    PlaceMedEx.Controls.Add(New LiteralControl("<tr style=""border-bottom: 1px navy solid;"">"))

                    MedExCode = ds_GetTemplateDetails.Tables(0).Rows(IndexForTemplate)("vMedExCode").ToString()
                    FianlMedExCd += MedExCode + ","
                    MedExDesc = ds_GetTemplateDetails.Tables(0).Rows(IndexForTemplate)("vMedExDesc").ToString()
                    FianlMedExDesc += MedExDesc + ","

                    Questring = "<td style='text-align:left;border-right: 1px navy solid;'>"
                    Questring += "<label style='padding-left: 5px;' class='LabelDisplay'>" + MedExDesc + "</label>"
                    Questring += "</td>"

                    Rdvalue = ds_GetTemplateDetails.Tables(0).Rows(IndexForTemplate)("vMedExValues").ToString()
                    ' Split string based on comma
                    Dim values As String() = Rdvalue.Split(New Char() {","c})
                    Dim word As String
                    For Each word In values
                        Radiostring += "<span><input type='radio' id='TempId_" + Id.ToString() + "' class='RemoveSelection' runat='server' onclick='SetValue(" + Id.ToString() + ")' name='Template_" + Id.ToString() + "' value='" + word + "' /><label>" + word + "</label></span>"
                    Next
                    Finalstring = Questring + "<td>" + Radiostring + "</td>"
                    Id += 1
                    PlaceMedEx.Controls.Add(New LiteralControl(Finalstring))
                    PlaceMedEx.Controls.Add(New LiteralControl("</tr>"))
                Next IndexForTemplate
            End If

            hdnMedExCode.Value = FianlMedExCd
            hdnMedExDesc.Value = FianlMedExDesc

            PlaceMedEx.Controls.Add(New LiteralControl("</table>"))
        Else
            PlaceMedEx.Controls.Clear()
        End If
    End Sub
#End Region

#Region "Button Events"
    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Try
            Me.AutoCompleteExtender2.ContextKey = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "'"
            Me.GvwDtl.DataSource = Nothing
            Me.GvwDtl.DataBind()
            ChkBoxLstActivity.Items.Clear()
            Me.txtSubject.Text = ""
            Me.HSubject.Value = ""
            Me.ResetHiddenControls()
            Me.MakeTemplate()
            Me.Session("DeleteRecords") = Nothing
        Catch ex As Exception
            Me.ShowErrorMessage("", ex.Message)
        Finally
            'ds_Check.Dispose()
        End Try
    End Sub

    Protected Sub btnSetSubject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetSubject.Click
        Dim ds_GetselectedActivity As DataSet
        Dim ds_CheckRights As New DataSet

        Try
            Dim WStr As String = String.Empty

            'Check User Rights For logged UserId, WorkspaceId and SubjectId
            WStr = " vWorkspaceId='" + Me.HProjectId.Value.Trim() + "' And vSubjectId='" + Me.HSubject.Value.Trim() + "' And iUserId=" + Me.Session(S_UserID) + ""
            If Not Me.objHelp.GetViewWorkSpaceUserNodeDetail(WStr, ds_CheckRights, eStr) Then
                Me.ShowErrorMessage(eStr, ".....btnSetSubject_Click")
                Exit Sub
            End If

            btnExit.Visible = True
            btnExitView.Visible = False

            If ds_CheckRights.Tables(0).Rows.Count = 0 Then
                btnSave.Visible = False
                objcommon.ShowAlert("User have No Activity Rights.", Me.Page)
                Exit Sub
            Else
                btnSave.Visible = True
            End If

            ResetHiddenControls()
            FillVisitDropDown()
            HParentWorkSpaceId.Value = Me.objHelp.GetParentWorkspaceId(Me.HProjectId.Value.Trim())
            MakeTemplate()

            ds_GetselectedActivity = Me.objHelp.GetCRFBunchRecord(Me.HProjectId.Value.Trim(), Me.HSubject.Value.Trim())
            If ds_GetselectedActivity.Tables(0).Rows.Count > 0 Then
                Dim ActTable As DataTable = ds_GetselectedActivity.Tables(0)
                Session("DeleteRecords") = ActTable
                GvwDtl.DataSource = ActTable
                GvwDtl.Visible = True
                GvwDtl.DataBind()
            Else
                GvwDtl.Visible = False
                GvwDtl.DataSource = Nothing
                Me.Session("DeleteRecords") = Nothing
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...........btnSetProject_Click")
        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim ds_Save As New DataSet
        Dim dt_SaveDtlinfo As New DataTable
        Dim dt_SaveSubInfo As New DataTable
        Dim ds_SubSave As New DataSet
        Dim dr_SubDtl As DataRow
        Dim TblLen As Integer
        Dim MedExCd As String = String.Empty
        Dim MedExDesc As String = String.Empty

        Try
            ds_Save = New DataSet
            Dim dr As DataRow
            Dim IndexForActivity As Integer

            Dim IBunchId As String = String.Empty
            IBunchId = Me.objHelp.GetBunchId(Me.HProjectId.Value.Trim(), Me.HSubject.Value.Trim())

            Dim ds_CRFHdrtbl As New DataSet
            If Not objHelp.GetCRFBunchMst("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CRFHdrtbl, eStr) Then
                objcommon.ShowAlert("Error While Getting Blank Structure", Me.Page)
                Exit Sub
            Else
                dt_SaveDtlinfo = ds_CRFHdrtbl.Tables(0)
            End If

            For IndexForActivity = 0 To ChkBoxLstActivity.Items.Count - 1
                If ChkBoxLstActivity.Items.Item(IndexForActivity).Selected = True Then
                    dr = dt_SaveDtlinfo.NewRow()
                    dr.Item("nCRFBunchHdrNo") = 0
                    dr.Item("vWorkSpaceId") = Me.HProjectId.Value.Trim()
                    dr.Item("vParentWorkspaceId") = Me.HParentWorkSpaceId.Value.Trim()
                    dr.Item("vSubjectId") = Me.HSubject.Value.Trim()
                    dr.Item("iBunchId") = IBunchId.Trim()
                    dr.Item("iNodeId") = Convert.ToString(ChkBoxLstActivity.Items(IndexForActivity).Value).Trim()
                    dr.Item("vActivityId") = objHelp.GetActivityId(Me.HProjectId.Value.Trim(), Convert.ToString(ChkBoxLstActivity.Items(IndexForActivity).Value).Trim())
                    dr.Item("vActivity") = Convert.ToString(ChkBoxLstActivity.Items(IndexForActivity).Text).Trim()
                    dr.Item("iModifyBy") = Me.Session(S_UserID)
                    dr.Item("cStatusIndi") = "N"
                    dr.Item("DataMode") = "S"
                    dt_SaveDtlinfo.Rows.Add(dr)
                End If
            Next IndexForActivity

            Dim dtCopy As DataTable = dt_SaveDtlinfo.Copy()
            dtCopy.TableName = "CRFBunchHdrTable"
            ds_Save.Tables.Add(dtCopy)

            If Not Me.objLambda.Save_CRFBunchData(ds_Save, Me.Session(S_UserID), eStr) Then
                objcommon.ShowAlert("Error While Saving Bunch Records", Me.Page)
                Exit Sub
            End If

            MedExCd = hdnMedExCode.Value
            MedExCd = MedExCd.TrimEnd(","c) 'For Remove Last Comma
            MedExDesc = hdnMedExDesc.Value
            MedExDesc = MedExDesc.TrimEnd(","c) 'For Remove Last Comma
            Dim MedExCdList As String() = MedExCd.Split(",")
            Dim MedExDescList As String() = MedExDesc.Split(",")
            TblLen = MedExCdList.Length

            Dim ds_CRFSubtbl As New DataSet
            If Not objHelp.GetCRFBunchSubMst("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CRFSubtbl, eStr) Then
                Throw New Exception("Error While Getting Blank Structure")
            Else
                dt_SaveSubInfo = ds_CRFSubtbl.Tables(0)
            End If

            For IndexForActivity = 0 To dt_SaveDtlinfo.Rows.Count - 1
                For TblLen = 0 To MedExCdList.Length - 1
                    dr_SubDtl = dt_SaveSubInfo.NewRow()
                    dr_SubDtl.Item("nCRFBunchSubDtlNo") = 0
                    dr_SubDtl.Item("vWorkSpaceId") = Me.HProjectId.Value.Trim()
                    dr_SubDtl.Item("vSubjectId") = Me.HSubject.Value.Trim()
                    dr_SubDtl.Item("iNodeId") = dt_SaveDtlinfo.Rows(IndexForActivity)("iNodeId").ToString()
                    dr_SubDtl.Item("vMedExCode") = MedExCdList(TblLen)
                    dr_SubDtl.Item("vMedExDesc") = MedExDescList(TblLen)
                    If TblLen = 0 Then
                        dr_SubDtl.Item("vMedExResult") = hdnAns1.Value
                    ElseIf TblLen = 1 Then
                        dr_SubDtl.Item("vMedExResult") = hdnAns2.Value
                    ElseIf TblLen = 2 Then
                        dr_SubDtl.Item("vMedExResult") = hdnAns3.Value
                    ElseIf TblLen = 3 Then
                        dr_SubDtl.Item("vMedExResult") = hdnAns4.Value
                    ElseIf TblLen = 4 Then
                        dr_SubDtl.Item("vMedExResult") = hdnAns5.Value
                    End If
                    dr_SubDtl.Item("iModifyBy") = Me.Session(S_UserID)
                    dr_SubDtl.Item("cStatusIndi") = "N"
                    dt_SaveSubInfo.Rows.Add(dr_SubDtl)
                Next TblLen
            Next IndexForActivity

            Dim dt_Sub As DataTable = dt_SaveSubInfo.Copy()
            dt_Sub.TableName = "CRFBunchSubDtlTable"
            ds_SubSave.Tables.Add(dt_Sub)

            If Not Me.objLambda.Save_CRFSubBunchData(ds_SubSave, Me.Session(S_UserID), eStr) Then
                objcommon.ShowAlert("Error While Saving Bunch Sub Records", Me.Page)
                Exit Sub
            Else
                objcommon.ShowAlert("Activity Bunch Saved Successfully.", Me.Page)
                Me.ResetPage()
                dt_SaveDtlinfo.Rows.Clear()
                dt_SaveSubInfo.Rows.Clear()
            End If
        Catch ex As Exception
            Me.ResetPage()
            Me.ShowErrorMessage(ex.Message, ".........btnSave_Click")
            Me.ResetPage()
            dt_SaveDtlinfo.Rows.Clear()
            dt_SaveSubInfo.Rows.Clear()
        End Try
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Response.Redirect("frmmainPage.aspx", False)
    End Sub
#End Region

#Region "Error Handler"
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objcommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objcommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub
#End Region

#Region "Reset Page/Controls"
    Private Sub ResetPage()
        Me.GvwDtl.DataSource = Nothing
        Me.GvwDtl.DataBind()
        ChkBoxLstActivity.Items.Clear()
        Me.txtproject.Text = ""
        Me.HProjectId.Value = ""
        Me.HParentWorkSpaceId.Value = ""
        Me.txtSubject.Text = ""
        Me.HSubject.Value = ""
        ResetHiddenControls()
        Me.LblActivity.Text = ""
        Me.LblActivity.Visible = False
        Me.PnlActivity.Visible = False
        ChkBoxLstActivity.Visible = False
        PlaceMedEx.Controls.Clear()
        Me.Session("DeleteRecords") = Nothing
        GenCall()
    End Sub

    Private Sub ResetHiddenControls()
        Me.hdnAns1.Value = ""
        Me.hdnAns2.Value = ""
        Me.hdnAns3.Value = ""
        Me.hdnAns4.Value = ""
        Me.hdnAns5.Value = ""
    End Sub
#End Region

End Class