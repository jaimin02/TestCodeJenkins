Imports Newtonsoft.Json

Partial Class frmsubjectassignment
    Inherits System.Web.UI.Page

#Region " variable declaration "

    Shared objcommon As New clsCommon
    Dim objhelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Dim objlambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()
    Shared objHelpdb As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()

    Private Const vs_choice As String = "choice"
    Private Const dsWorkspaceSubMst As String = "dsWorkspaceSubjectMst"
    Private Const vs_workspacesubjectmst As String = "dtworkspacesubjectmst"
    Private Const vs_workspacesubjectid As String = "vworkspacesubjectid"
    Private Const vs_gvdsworkspacesubjectmst As String = "gvdsworkspacesubjectmst"
    Private Const vs_gvmysubjectno As String = "gvmysubjectno"
    Private Const vs_subjectid As String = "vsubjectid"
    Private Const vs_vmysubjectno As String = "vmysubjectno"
    Private Const vs_Index As String = "Index"
    Private Const vs_type As String = "type"
    Private Const vs_isscrdate As String = "isscrdate"
    Private Const vs_srno As String = "isrno"
    Private Const vs_reportingdate As String = "dreportingdate"
    Private Const gvc_srno As Integer = 0
    Private Const gvcell_subjectid As Integer = 1
    Private Const gvcell_subjectno As Integer = 2
    Private Const gvcell_txtsubjectno As Integer = 3
    Private Const gvcell_subject As Integer = 4
    Private Const gvcell_initials As Integer = 5
    Private Const gvcell_reportingdatetime As Integer = 6
    Private Const gvcell_iScrDays As Integer = 7
    Private Const gvcell_screendate As Integer = 8
    Private Const gvcell_save As Integer = 9
    Private Const gvcell_extra As Integer = 10
    Private Const gvcell_crejected As Integer = 11
    Private Const gvcell_vmysubjectno As Integer = 12
    Private Const gvc_code As Integer = 13
    Private Const gvc_scrdate As Integer = 14
    Private Const gvc_medexscreenhdrno As Integer = 15
    Private Const gvc_Audit As Integer = 17
    Private Const gvc_dReportingDate_actual As Integer = 18

    Private Const GVCAudit_iAsnNo As Integer = 0
    Private Const GVCAudit_vInitials As Integer = 1
    Private Const GVCAudit_vSubjectID As Integer = 2
    Private Const GVCAudit_vMySubjectNo As Integer = 3
    Private Const GVCAudit_dReportingDate As Integer = 4
    Private Const GVCAudit_vModifyBy As Integer = 5
    Private Const GVCAudit_dModifyOn As Integer = 6
    Private Const GVCAudit_vRemarks As Integer = 7
    Private Const GVCAudit_nWorkspaceSubjectHistoryId As Integer = 8
    Private Const GVCAudit_vWorkspaceSubjectId As Integer = 9
    Private Const GVCAudit_iTranN As Integer = 10


    Private isfirst As Boolean = False
    'private isscrdate as boolean = false
    Private isreplace As Boolean = False
    Private estr_retu As String
    Private VS_SelectediMySubjectNo As String = "VS_SelectediMySubjectNo"

    Dim reportingdate As String = String.Empty
    Dim replaceoffset As String = String.Empty

#End Region

#Region "page load"
    Protected Sub page_load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            gencall()
            'Me.txtproject.Text = Me.Session(S_ProjectName).ToString()
            Me.ViewState(vs_isscrdate) = False
        End If

    End Sub
#End Region

#Region "gencall"
    Private Function gencall() As Boolean

        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim dt_workspacesubjectmst As DataTable = Nothing
        Dim ds_workspacesubjectmst As New DataSet
        Dim choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try
            choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add

            Me.ViewState(vs_choice) = choice   'to use it while saving
            If choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(vs_workspacesubjectid) = Me.Request.QueryString("value").ToString
            End If

            ''''check for valid user''''''''''''''
            If Not gencall_data(choice, dt_workspacesubjectmst) Then ' for data retrieval
                Return False
            End If

            Me.ViewState(vs_workspacesubjectmst) = dt_workspacesubjectmst ' adding blank datatable in viewstate
            If Not gencall_showui(choice, dt_workspacesubjectmst) Then 'for displaying data 
                Return False
            End If

            Return True
        Catch ex As Exception
            showerrormessage(ex.Message, "...gencall")
            Return False
        Finally
        End Try
    End Function
#End Region

#Region "gencall_data"
    Private Function gencall_data(ByVal choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                            ByRef dt_dist_retu As DataTable) As Boolean
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim ds_workspacesubjectmst As DataSet = Nothing
        Dim objhelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()

        Try
            wstr = "1=2"
            If choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wstr = "vworkspacesubjectid=" + Me.ViewState(vs_workspacesubjectid).ToString() 'value of where condition
            End If

            If Not objhelp.GetWorkspaceSubjectMaster(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_workspacesubjectmst, estr) Then
                Throw New Exception(estr)
            End If

            If ds_workspacesubjectmst Is Nothing Then
                Throw New Exception(estr)
            End If

            If ds_workspacesubjectmst.Tables(0).Rows.Count <= 0 And _
               choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Throw New Exception("no records found for selected role")
            End If
            dt_dist_retu = ds_workspacesubjectmst.Tables(0)
            gencall_data = True

        Catch ex As Exception
            Me.showerrormessage(ex.Message, "...gencall_data")
            Return False
        Finally
        End Try
    End Function
#End Region

#Region "gencall_show_ui"

    Private Function gencall_showui(ByVal choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, ByVal dt_workspacesubjectmst As DataTable) As Boolean
        Dim workspaceid As String = String.Empty
        Dim estr As String = String.Empty
        Dim dsworkspace As New DataSet
        Dim sender As New Object
        Dim e As New EventArgs
        Try

            Page.Title = ":: Subject Assignment  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Not IsNothing(Me.Request.QueryString("workspaceid")) Then
                workspaceid = Me.Request.QueryString("workspaceid").Trim()
            End If

            CType(Master.FindControl("lblheading"), Label).Text = "Subject Assignment"

            If workspaceid.Trim() <> "" Then
                If Not Me.objhelp.getworkspacemst("vworkspaceid='" & workspaceid.Trim() & "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                            dsworkspace, estr) Then
                    Me.showerrormessage(estr, "")
                    gencall_showui = False
                End If

                Me.txtproject.Text = dsworkspace.Tables(0).Rows(0).Item("vworkspacedesc")
                Me.HProjectId.Value = workspaceid.Trim()
                fillvalues()
            End If

            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtproject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnsetproject_click(sender, e)
                gencall_showui = True
                Exit Function
            End If
            '==added on 11-Nov-2011 by Mrunal Parekh to show project according to user
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)
            '========
            gencall_showui = True

        Catch ex As Exception
            Me.showerrormessage(ex.Message, "...gencall_showui")
            Return False
        End Try
    End Function

#End Region

#Region "reset page"
    Private Sub resetpage()

        Me.ViewState(vs_gvdsworkspacesubjectmst) = Nothing
        Me.ViewState(vs_gvmysubjectno) = Nothing
        Me.ViewState(vs_srno) = Nothing
        Me.ViewState(vs_workspacesubjectid) = Nothing
        Me.ViewState(vs_workspacesubjectmst) = Nothing

        Me.gvwWorkspaceSubjectMst.DataSource = Nothing
        Me.ddlSubject.DataSource = Nothing

        gencall()
        'response.redirect("frmworkspacesubjectmst.aspx?mode=1")
    End Sub
#End Region

#Region "error handler"
    Private Sub showerrormessage(ByVal exmessage As String, ByVal estr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exmessage + "<br> " + estr
        objcommon.WriteError(Server, Request, Session, exmessage + "<br> " + estr)
    End Sub
    Private Sub showerrormessage(ByVal exmessage As String, ByVal estr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exmessage + "<br> " + estr
        objcommon.WriteError(Server, Request, Session, ex, exmessage + "<br> " + estr)
    End Sub
#End Region

#Region "save"

    Protected Sub save(ByVal subjectid As String, ByVal type As String)
        Dim dt As New DataTable
        Dim ds_stagemat As DataSet
        Dim ds_grid As New DataSet
        Dim ds_workspacesubjectmst As New DataSet
        Dim estr As String = String.Empty
        Dim ds_type As New Data.DataSet
        Dim objopws As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()
        Dim ds_medexscreen As New Data.DataSet

        Try
            If Not assignupdatedvalues(subjectid, WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, type) Then
                fillvalues()
                Exit Sub
            End If

            ds_stagemat = New DataSet
            ds_stagemat.Tables.Add(CType(Me.ViewState(vs_workspacesubjectmst), Data.DataTable).Copy())
            ds_stagemat.Tables(0).TableName = "view_workspacesubjectmst"   ' new values on the form to be updated

            If Not objopws.Save_InsertWorkspaceSubjectMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_WorkspaceSubjectMst, ds_stagemat, Me.Session(S_UserID), estr) Then
                objcommon.ShowAlert("error while assigning", Me.Page)
                Exit Sub
            End If
            'divSearch.Attributes.Add("style", "display:none")
            Me.MPEdate.Hide()
            objcommon.ShowAlert("subject assigned successfully !", Me)
            fillgridview()
            fillvalues()

        Catch ex As System.Threading.ThreadAbortException
            showerrormessage(ex.Message, estr)
        Catch ex As Exception
            showerrormessage(ex.Message, estr)
        End Try
    End Sub

    Private Function assignupdatedvalues(ByVal subjectid As String, ByVal choice As Integer, ByVal type As String) As Boolean
        Dim estr As String = String.Empty
        Dim dtold As New DataTable
        Dim dssubjectno As New DataSet
        Dim subjectno As Integer = 0
        Dim dr As DataRow
        Dim canstartafterdetails As String = String.Empty
        Dim vmysubjectno As String = String.Empty
        Dim dssubjectmst As New DataSet
        Dim dvold As New DataView
        Dim wstr As String = String.Empty
        Dim workspaceid As String = String.Empty
        Dim SelectedSubjectNo As String = String.Empty
        workspaceid = Me.HProjectId.Value.Trim()

        Try
            vmysubjectno = CType(ViewState(vs_vmysubjectno), String)
            If ddlSubject.SelectedIndex > 0 Then
                isreplace = True
            End If

            wstr += " vworkspaceid = '" & workspaceid & "' And cStatusIndi <>'" + Status_Delete + "'"
            If Not Me.objhelp.GetWorkspaceSubjectMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
            dssubjectmst, estr) Then
                Me.objcommon.ShowAlert(estr, Me.Page())
                Return False
            End If
            dtold = dssubjectmst.Tables(0)

            If isreplace = False Then 'to avoid when subject is replaced 
                If Not validatesubjectno(dssubjectmst.Tables(0), vmysubjectno) Then

                    'divSearch.Attributes.Add("style", "display:none")
                    Me.MPEdate.Hide()
                    objcommon.ShowAlert("entered mysubjectno is already assigned to another subject", Me)
                    ViewState(vs_vmysubjectno) = Nothing
                    vmysubjectno = ""
                    fillgridview()
                    Return False
                End If
            End If

            dvold = dssubjectmst.Tables(0).DefaultView
            dvold.RowFilter = "vsubjectid='" + subjectid.Trim() + "'"
            dtold = dvold.ToTable.Copy()

            If type.ToUpper = "SAVE" Then

                If HParentWorkSpaceId.Value <> "" And (HIsTestSite.Value = "N" Or HIsTestSite.Value = "") Then
                    'If Not Me.objhelp.GetFieldsOfTable("workspacesubjectmst", "isnull(max(left(imysubjectno,4)),0) as maxsubno", _
                    '                                       "vParentWorkspaceId = '" + HParentWorkSpaceId.Value + "' and left(imysubjectno,4) < 2000 ", _
                    '                                       dssubjectno, estr) Then
                    If Not Me.objhelp.GetFieldsOfTable("View_WorkSpaceSubjectMstForiMySubjectNo", "isnull(max(left(imysubjectno,4)),0) as maxsubno", _
                                   "vParentWorkspaceId = '" + HParentWorkSpaceId.Value + "' and left(imysubjectno,4) < 2000  and cIsTestSite<>'Y' ", _
                                   dssubjectno, estr) Then
                        Return False
                    End If
                Else
                    If Not Me.objhelp.GetFieldsOfTable("workspacesubjectmst", "isnull(max(left(imysubjectno,4)),0) as maxsubno", _
                                                           "vworkspaceid = '" + workspaceid + "' and left(imysubjectno,4) < 2000 ", _
                                                           dssubjectno, estr) Then
                        Return False
                    End If
                End If



                If dssubjectno.Tables(0).Rows(0)("maxsubno") = 0 Then
                    subjectno = 1001
                Else
                    subjectno = dssubjectno.Tables(0).Rows(0)("maxsubno") + 1
                End If

                'Added By Dharmesh H.Salla on 20-May-2011''
            ElseIf type.ToUpper() = "REJECT" Then
                'If Not Me.objhelp.GetFieldsOfTable("workspacesubjectmst", "isnull(max(imysubjectno),1001) as maxsubno", _
                '                "vworkspaceid = '" + workspaceid + "' and imysubjectno < 2000 ", _
                '                dssubjectno, estr) Then
                '    Return False
                'End If                                       'comment By Mani


                'If dssubjectno.Tables(0).Rows(0)("maxsubno") = 0 Then
                '    subjectno = 1001                                               'Comment above two lines if assignment has done only for one subject etc
                ''Added By Dharmesh H.Salla on 18-May-2011'

                SelectedSubjectNo = ViewState(VS_SelectediMySubjectNo)
                If SelectedSubjectNo = 0 Then
                    Me.objcommon.ShowAlert("You Can not Replace Subject which is Not assigned", Me.Page)
                    Exit Function
                End If


                If SelectedSubjectNo.Length() > 4 Then
                    Dim MaxLength As Char = SelectedSubjectNo.Chars(SelectedSubjectNo.Length - 1)
                    Dim MaxIncrement As Integer = Convert.ToInt32(MaxLength.ToString()) + 1
                    SelectedSubjectNo = SelectedSubjectNo.ToString().Remove(SelectedSubjectNo.ToString().Length - 1)
                    subjectno = SelectedSubjectNo & MaxIncrement
                ElseIf SelectedSubjectNo.Length = 4 Then
                    subjectno = SelectedSubjectNo & "1"
                End If
                ' subjectno = dssubjectno.Tables(0).Rows(0)("maxsubno") + 1
                'End If           'comment by Mani 

                ''''' Addition By Dharmesh Completed
            ElseIf type.ToUpper = "EXTRA" Then
                If Not Me.objhelp.GetFieldsOfTable("workspacesubjectmst", "isnull(min(imysubjectno),0) as minsubno", _
                        "vworkspaceid = '" + workspaceid + "' and imysubjectno < 0 ", _
                        dssubjectno, estr) Then
                    Return False
                End If

                If dssubjectno.Tables(0).Rows(0)("minsubno") = 0 Then
                    subjectno = -1
                Else
                    subjectno = dssubjectno.Tables(0).Rows(0)("minsubno") - 1
                End If
            End If

            If choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                For Each dr In dtold.Rows
                    dr("vworkspaceid") = workspaceid
                    dr("imysubjectno") = subjectno
                    dr("vmysubjectno") = vmysubjectno

                    If Not IsNothing(Me.ViewState(vs_gvmysubjectno)) AndAlso Me.ViewState(vs_gvmysubjectno) <> 0 Then
                        If Not Me.objhelp.GetFieldsOfTable("workspacesubjectmst", "isnull(max(imysubjectno),0) as maxsubno", _
                                                               "vworkspaceid = '" + workspaceid + "' and imysubjectno < 2000 ", _
                                                               dssubjectno, estr) Then
                            Return False
                        End If
                        'Commented By Dharmesh '
                        'dr("imysubjectno") = dssubjectno.Tables(0).Rows(0)("maxsubno") + 1
                        ''' ****************************************************************
                        'dr("imysubjectno") = ctype(me.viewstate(vs_gvmysubjectno), integer) + 1
                        dr("vmysubjectno") = Me.txtMySubNo.Text.Trim
                    End If
                    dr("nmedexscreeninghdrno") = Me.rblScreeningDate.SelectedValue
                    dr("imodifyby") = Me.Session(S_UserID)
                    dr.AcceptChanges()
                Next
                dtold.AcceptChanges()
            End If

            Me.ViewState(vs_workspacesubjectmst) = dtold
            Return True
        Catch ex As Exception
            showerrormessage(ex.Message, "...assignupdatedvalues")
            Return False
        End Try
    End Function

    Private Function assignvalues(Optional ByVal rejected As Boolean = False, Optional ByVal mode As String = "add") As Boolean
        Dim dstemp As New DataSet
        Dim dttemp As New DataTable
        Dim dsattendence As New DataSet
        Dim dr As DataRow = Nothing
        Dim dvtemp As New DataView
        Dim wstr As String = String.Empty
        Dim workspaceid As String = String.Empty
        workspaceid = Me.HProjectId.Value.Trim()

        Try
            wstr = "vworkspaceid='" & workspaceid & "'"
            If Not objhelp.GetWorkspaceSubjectMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dstemp, estr_retu) Then
                Throw New Exception(estr_retu)
                Exit Function
            End If
            dttemp = dstemp.Tables(0)
            dttemp.TableName = "view_workspacesubjectmst"

            If Not validatesubjectno(dttemp, Me.txtMySubNo.Text) Then
                'divSearch.Attributes.Add("style", "display:none")
                Me.MPEdate.Hide()
                objcommon.ShowAlert("entered mysubjectno is already assigned to another subject", Me)
                ViewState(vs_vmysubjectno) = Nothing
                fillgridview()
                Return False
            End If
            dvtemp = dttemp.DefaultView
            dvtemp.RowFilter = "vworkspacesubjectid = " + ViewState(gvc_code)
            dttemp = dvtemp.ToTable.Copy
            dstemp = New DataSet
            dstemp.Tables.Add(dttemp)

            If rejected Then
                For Each dr In dttemp.Rows
                    dr("crejectionflag") = "Y"
                    dr("nreasonno") = Me.ddlReject.SelectedValue
                    dr("imodifyby") = Me.Session(S_UserID)
                    dr.AcceptChanges()
                Next
                dttemp.AcceptChanges()
            Else
                dr("crejectionflag") = "N"
                dr("nreasonno") = ""
                'dr("imysubjectno") = 1
                dr("imodifyby") = Me.Session(S_UserID)
                dttemp.AcceptChanges()
            End If

            If Not objlambda.Save_InsertWorkspaceSubjectMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete, _
                                                            WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_WorkspaceSubjectMst, _
                                                            dstemp, Me.Session(S_UserID), estr_retu) Then
                Throw New Exception(estr_retu)
                resetpage()
            End If

            If rejected Then
                Me.txtSubjectRemark.Text = ""
                divRejectSubject.Attributes.Add("style", "display:none")
            End If
            Me.MPEdate.Hide()
            ' Me.divSearch.Attributes.Add("style", "display:none")
            objcommon.ShowAlert("subject is replaced successfully !", Me)
            Return True

        Catch ex As Exception
            Me.showerrormessage(ex.Message, "...assignvalues")
            Return False
        End Try

    End Function

#End Region

#Region "button event"

    Protected Sub btnclose_click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.ddlSubject.DataSource = Nothing
        Me.ddlSubject.DataBind()
        Me.ddlSubject.ClearSelection()
        divRejectSubject.Attributes.Add("style", "display:none")
        modalrejectsubject.Hide()
    End Sub

    Protected Sub btnsave_click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim estr As String = String.Empty
        Dim isvalidsubject As Boolean = True 'added so that control goes directly to fillgridview() if assignvalues() is false 
        Try
            If Not fillscreeningdates(Me.ddlSubject.SelectedValue.ToString.Split("##")(0)) Then
                objcommon.ShowAlert("Error while getting screening dates", Me)
                Exit Sub
            End If
            divRejectSubject.Attributes.Add("style", "display:none")
            Me.MPEdate.Show()

            'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "DivSearchShowHide", "DivSearchShowHide('S');", True)
            'modalrejectsubject.Hide()

        Catch ex As Exception
            Me.showerrormessage(ex.Message, "...btnsave_click")
        End Try
    End Sub

    Protected Sub btncancel_click(ByVal sender As Object, ByVal e As System.EventArgs)
        resetpage()
    End Sub

    Protected Sub btnclosenew_click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim workspaceid As String = String.Empty
        workspaceid = Me.Request.QueryString("workspaceid").Trim()
        Me.Response.Redirect(Me.Request.QueryString("page2") & ".aspx?type=" & Me.Request.QueryString("type") & "&workspaceid=" & workspaceid.Trim() & _
                          "&page=" & Me.Request.QueryString("page"))
    End Sub

    Protected Sub btnsetproject_click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_Check As New DataSet
        Dim dv_Check As New DataView
        Dim ds_Subjects As New DataSet
        Dim ds_CreatedBy As New DataSet
        Dim dv_CreatedBy As New DataView

        Try
            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D'"

            If Not Me.objhelp.GetCRFLockDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_Check, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds_Check Is Nothing Then
                dv_Check = ds_Check.Tables(0).DefaultView
                dv_Check.Sort = "iTranNo desc"
                If dv_Check.ToTable().Rows.Count > 0 Then
                    If Convert.ToString(dv_Check.ToTable().Rows(0)("cLockFlag")).Trim.ToUpper() = "L" Then
                        Me.objcommon.ShowAlert("Site is Locked.", Me.Page)
                        Me.txtproject.Text = ""
                        Me.HProjectId.Value = ""
                        Exit Sub
                    End If
                End If
            End If

            If Not Me.fillvalues() Then
                Exit Sub
            End If
            Me.GetParentWorkSpaceId()
        Catch ex As Exception
            Me.showerrormessage("Error While Getting CRF Lock Details ", ex.Message + "...btnsetproject_click")
        End Try
    End Sub

    Protected Sub btncancel_click1(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("frmsubjectassignment.aspx?mode=1")
    End Sub

    Protected Sub btnexit_click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Response.Redirect("frmmainpage.aspx")
    End Sub

#End Region

#Region "grid event"

#Region "gvwworkspacesubjectmst"

    Protected Sub gvwworkspacesubjectmst_pageindexchanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        gvwWorkspaceSubjectMst.PageIndex = e.NewPageIndex
        fillgridview()
    End Sub

    Protected Sub gvwworkspacesubjectmst_rowcommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim i As Integer = e.CommandArgument
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim ds_workspacesubmst As New DataSet

        Try
            Me.ViewState(vs_gvmysubjectno) = Me.gvwWorkspaceSubjectMst.Rows(i).Cells(gvcell_subjectno).Text.Trim()
            Me.ViewState(vs_reportingdate) = Convert.ToString(Me.gvwWorkspaceSubjectMst.Rows(i).Cells(gvc_dReportingDate_actual).Text.Trim)

            If e.CommandName.ToUpper = "SAVE" Or e.CommandName.ToUpper = "EXTRA" Then
                If (Convert.ToString(CType(Me.gvwWorkspaceSubjectMst.Rows(i).FindControl("txtmysubjectno"), TextBox).Text.Trim()) = "" Or _
                    Convert.ToString(CType(Me.gvwWorkspaceSubjectMst.Rows(i).FindControl("txtmysubjectno"), TextBox).Text.Trim()) = "0") Then
                    objcommon.ShowAlert("Please Enter MySubjectNo", Me)
                    Exit Sub
                End If

                If e.CommandName.ToUpper = "SAVE" Then
                    If Not IsNumeric(CType(Me.gvwWorkspaceSubjectMst.Rows(i).FindControl("txtmysubjectno"), TextBox).Text.Trim()) Then
                        objcommon.ShowAlert("Non-Numeric MysubjectNo is not allowed while saving Subject", Me)
                        Exit Sub
                    End If
                    Me.txtScreenDays.Text = Me.gvwWorkspaceSubjectMst.Rows(i).Cells(gvcell_iScrDays).Text
                End If

                If e.CommandName.ToUpper = "EXTRA" Then

                    If IsNumeric(CType(Me.gvwWorkspaceSubjectMst.Rows(i).FindControl("txtmysubjectno"), TextBox).Text.Trim()) Then
                        objcommon.ShowAlert("Numeric MysubjectNo is not allowed while saving 'Extra Subject'", Me)
                        Exit Sub
                    End If
                    Me.txtScreenDays.Text = Me.gvwWorkspaceSubjectMst.Rows(i).Cells(gvcell_iScrDays).Text
                End If
                '===
                'added on 21-apr-10
                If CType(Me.gvwWorkspaceSubjectMst.Rows(i).FindControl("txtmysubjectno"), TextBox).Text.Trim = "" Then
                    objcommon.ShowAlert("please enter my subject no", Me)
                    CType(Me.gvwWorkspaceSubjectMst.Rows(i).FindControl("txtmysubjectno"), TextBox).Focus()
                    Exit Sub
                End If
                ViewState(vs_vmysubjectno) = CType(Me.gvwWorkspaceSubjectMst.Rows(i).FindControl("txtmysubjectno"), TextBox).Text.Trim()
                CType(Me.gvwWorkspaceSubjectMst.Rows(i).FindControl("txtmysubjectno"), TextBox).Enabled = False
                '======
                If i > 0 Then
                    If Me.gvwWorkspaceSubjectMst.Rows(i - 1).Cells(gvcell_subjectno).Text = "0" Then
                        Me.objcommon.ShowAlert("can not jump subjects.", Me.Page())
                        fillvalues()
                        Exit Sub
                    End If
                End If
                If Not fillscreeningdates(Me.gvwWorkspaceSubjectMst.Rows(i).Cells(gvcell_subjectid).Text) Then
                    'Me.objcommon.ShowAlert("error while getting screening dates", Me)
                    Exit Sub
                End If

                Me.MPEdate.Show()

                ' ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "DivSearchShowHide", "DivSearchShowHide('S');", True)
                ViewState(vs_subjectid) = Me.gvwWorkspaceSubjectMst.Rows(i).Cells(gvcell_subjectid).Text
                ViewState(vs_type) = e.CommandName.ToUpper

            ElseIf e.CommandName.ToUpper = "REJECT" Then
                If Not CheckInDosingDetail(Left(Me.gvwWorkspaceSubjectMst.Rows(i).Cells(gvcell_subjectno).Text.ToString(), 4)) Then
                    Me.objcommon.ShowAlert("Subject is Dosed.You cannot REPLACE Subject.", Me.Page)
                    Exit Sub
                End If
                ViewState(gvc_code) = Me.gvwWorkspaceSubjectMst.Rows(i).Cells(gvc_code).Text.Trim()
                Me.lblSubjectName.Text = Me.gvwWorkspaceSubjectMst.Rows(i).Cells(gvcell_txtsubjectno).Text.Trim()
                Me.lblSubjectName.Font.Bold = True

                ''Added By Dharmesh H.Salla on 18-May-2011
                ViewState(VS_SelectediMySubjectNo) = Me.gvwWorkspaceSubjectMst.Rows(i).Cells(gvcell_subjectno).Text.Trim()
                Me.txtScreenDays.Text = Me.gvwWorkspaceSubjectMst.Rows(i).Cells(gvcell_iScrDays).Text
                If ViewState(VS_SelectediMySubjectNo) = 0 Then
                    Me.objcommon.ShowAlert("You can Not Replace Subject which is not assigned ", Me.Page)
                    Exit Sub
                End If

                fillrejectdropdown()
                Me.trAddRejectSubject.Visible = True

                If Not Me.fillsubject() Then
                    Exit Sub
                End If
                ViewState(vs_type) = e.CommandName.ToUpper
                'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "DivRejectShowHide", "DivRejectShowHide('S');", True)
                'divRejectSubject.Style.Add("background-color", "#CEE3ED")
                modalrejectsubject.Show()

            ElseIf e.CommandName.ToUpper = "SCRDATE" Then
                ViewState(vs_subjectid) = Me.gvwWorkspaceSubjectMst.Rows(i).Cells(gvcell_subjectid).Text
                ViewState(vs_isscrdate) = True
                If Not fillscreeningdates(Me.gvwWorkspaceSubjectMst.Rows(i).Cells(gvcell_subjectid).Text) Then
                    Me.objcommon.ShowAlert("error while getting screening dates", Me)
                    Exit Sub
                End If
                Me.MPEdate.Show()

                'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "DivSearchShowHide", "DivSearchShowHide('S');", True)
            End If

            If e.CommandName.ToUpper = "EDIT" Then
                If Not CheckInDosingDetail(Left(Me.gvwWorkspaceSubjectMst.Rows(i).Cells(gvcell_subjectno).Text.ToString(), 4)) Then
                    Me.objcommon.ShowAlert(" Subject is Dosed.You cannot edit Subject No.", Me.Page)
                    Exit Sub
                End If
                CType(gvwWorkspaceSubjectMst.Rows(i).FindControl("imgsave"), ImageButton).Visible = True
                CType(gvwWorkspaceSubjectMst.Rows(i).FindControl("imgcancel"), ImageButton).Visible = True
                CType(gvwWorkspaceSubjectMst.Rows(i).FindControl("imgedit"), ImageButton).Visible = False
                CType(gvwWorkspaceSubjectMst.Rows(i).FindControl("txtmysubjectno"), TextBox).Enabled = True

            ElseIf e.CommandName.ToUpper = "UPDATE" Then
                Try
                    If CType(Me.gvwWorkspaceSubjectMst.Rows(i).FindControl("txtmysubjectno"), TextBox).Text.Trim = "" Then
                        objcommon.ShowAlert("please enter my subject no", Me)
                        CType(Me.gvwWorkspaceSubjectMst.Rows(i).FindControl("txtmysubjectno"), TextBox).Focus()
                        Exit Sub
                    End If

                    wstr = "vWorkspaceId = '" & Me.HProjectId.Value & "' And vMySubjectNo = '" & CType(Me.gvwWorkspaceSubjectMst.Rows(i).FindControl("txtmysubjectno"), TextBox).Text.Trim & "' and vWorkspaceSubjectId <> '" & Me.gvwWorkspaceSubjectMst.Rows(i).Cells(gvc_code).Text.Trim() & "' and cStatusindi <>'D'"
                    If Not objhelp.GetViewWorkspaceSubjectMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                              ds_workspacesubmst, estr) Then

                        Throw New Exception(estr)
                    End If

                    If ds_workspacesubmst.Tables(0).Rows.Count > 0 Then
                        objcommon.ShowAlert("Entered MySubjectNo is already assigned to other Subject", Me)
                        CType(gvwWorkspaceSubjectMst.Rows(i).FindControl("txtmysubjectno"), TextBox).Enabled = False
                        fillgridview()
                        Exit Sub
                    End If

                    ViewState(vs_Index) = i
                    Me.mpeDialog.Show()
                Catch ex As Exception
                    Me.showerrormessage(ex.Message, "...gvwworkspacesubjectmst_rowcommand UPDATE")
                End Try

            ElseIf e.CommandName.ToUpper = "CANCEL" Then
                CType(gvwWorkspaceSubjectMst.Rows(i).FindControl("imgsave"), ImageButton).Visible = False
                CType(gvwWorkspaceSubjectMst.Rows(i).FindControl("imgcancel"), ImageButton).Visible = False
                CType(gvwWorkspaceSubjectMst.Rows(i).FindControl("imgedit"), ImageButton).Visible = True
                fillgridview()
                CType(gvwWorkspaceSubjectMst.Rows(i).FindControl("txtmysubjectno"), TextBox).Enabled = False
            End If

            If e.CommandName.ToUpper = "AUDIT" Then
                GVAudit.DataSource = Nothing
                GVAudit.DataBind()
                ViewState(gvc_code) = Me.gvwWorkspaceSubjectMst.Rows(i).Cells(gvc_code).Text.Trim()
                If Not ShowAudit() Then
                    Exit Sub
                End If
                Me.MpeAudit.Show()
            End If

        Catch ex As Exception
            Me.showerrormessage(ex.Message, "...gvwworkspacesubjectmst_rowcommand")
        End Try
    End Sub

    Protected Sub gvwworkspacesubjectmst_rowcreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Or _
                      e.Row.RowType = DataControlRowType.Header Or _
                      e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(gvc_code).Visible = False
            e.Row.Cells(gvcell_vmysubjectno).Visible = False
            e.Row.Cells(gvc_medexscreenhdrno).Visible = False

        End If
    End Sub

    Protected Sub gvwworkspacesubjectmst_rowdatabound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Try
            Dim eStr As String = String.Empty
            Dim ds As New DataSet
            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(gvc_dReportingDate_actual).Visible = False
            End If

            If e.Row.RowType = DataControlRowType.DataRow Then



                If e.Row.Cells(gvcell_subjectno).Text.Trim() < 0 Then
                    CType(e.Row.FindControl("imgBtnReject"), ImageButton).Visible = False
                End If
                If e.Row.Cells(gvcell_subjectno).Text.Trim() <> 0 Then
                    CType(e.Row.FindControl("imgSave1"), ImageButton).Visible = False
                End If

                e.Row.Cells(gvc_srno).Text = e.Row.RowIndex + (Me.gvwWorkspaceSubjectMst.PageSize * Me.gvwWorkspaceSubjectMst.PageIndex) + 1

                CType(e.Row.FindControl("imgSave1"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("imgSave1"), ImageButton).CommandName = "save"
                e.Row.Cells(gvcell_save).HorizontalAlign = HorizontalAlign.Center
                e.Row.Cells(gvcell_save).VerticalAlign = HorizontalAlign.Center

                CType(e.Row.FindControl("imgExtra"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("imgExtra"), ImageButton).CommandName = "extra"
                e.Row.Cells(gvcell_extra).HorizontalAlign = HorizontalAlign.Center
                e.Row.Cells(gvcell_extra).VerticalAlign = HorizontalAlign.Center

                CType(e.Row.FindControl("imgBtnReject"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("imgBtnReject"), ImageButton).CommandName = "reject"
                'If e.Row.Cells(gvcell_vmysubjectno).Text.Trim() = "0" Then
                'CType(e.Row.FindControl("imgBtnReject"), ImageButton).OnClientClick = "return fnValidateReject( '" + e.Row.Cells(gvcell_subjectid).Text.Trim() + "')"
                'Else
                CType(e.Row.FindControl("imgBtnReject"), ImageButton).OnClientClick = "return fnValidateReject( '" + e.Row.Cells(gvcell_vmysubjectno).Text.Trim() + "')"
                'End If


                e.Row.Cells(gvcell_crejected).HorizontalAlign = HorizontalAlign.Center
                e.Row.Cells(gvcell_crejected).VerticalAlign = HorizontalAlign.Center

                CType(e.Row.FindControl("imgScrDate"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("imgScrDate"), ImageButton).CommandName = "scrdate"
                e.Row.Cells(gvc_scrdate).HorizontalAlign = HorizontalAlign.Center
                e.Row.Cells(gvc_scrdate).VerticalAlign = HorizontalAlign.Center

                CType(e.Row.FindControl("imgedit"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("imgedit"), ImageButton).CommandName = "edit"

                CType(e.Row.FindControl("imgsave"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("imgsave"), ImageButton).CommandName = "update"

                CType(e.Row.FindControl("imgcancel"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("imgcancel"), ImageButton).CommandName = "cancel"

                CType(e.Row.FindControl("ImgAudit"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImgAudit"), ImageButton).CommandName = "AUDIT"
                e.Row.Cells(gvc_Audit).HorizontalAlign = HorizontalAlign.Center
                e.Row.Cells(gvc_Audit).VerticalAlign = HorizontalAlign.Center


                If Not Convert.ToString(e.Row.Cells(gvcell_reportingdatetime).Text).Trim = "" Then
                    reportingdate = Replace(e.Row.Cells(gvcell_reportingdatetime).Text.Trim(), "&nbsp;", "")
                    If reportingdate = "" Then
                        e.Row.Cells(gvcell_reportingdatetime).Text = ""
                    Else
                        '=======================  Added By Jeet Patel on 28-April-2015 to show canada time for canada subject in Grid View ===================================
                        If (e.Row.Cells(gvcell_subjectid).Text Like "CA*") Then
                            If Not objhelp.Proc_ActualAuditTrailTime(CType(Replace(e.Row.Cells(gvcell_reportingdatetime).Text.Trim(), "&nbsp;", ""), Date).ToString("dd-MMM-yyyy HH:mm").Trim() + "##" + "0042", ds, eStr) Then  '0042 is for passing Location code of canada
                                Throw New Exception(eStr)
                            End If
                            e.Row.Cells(gvcell_reportingdatetime).Text = CDate(ds.Tables(0).Rows(0).Item("ActualTIME")).ToString("dd-MMM-yyyy HH:mm") + " EST (" + ds.Tables(0).Rows(0).Item("TimeZoneOffSet").ToString + " GMT)"
                        Else
                            e.Row.Cells(gvcell_reportingdatetime).Text = CType(Replace(e.Row.Cells(gvcell_reportingdatetime).Text.Trim(), "&nbsp;", ""), Date).ToString("dd-MMM-yyyy HH:mm").Trim() + strServerOffset
                        End If
                        '======================================================================================================================================================
                        'replaceoffset = reportingdate.Substring(reportingdate.Length - 7, 7).ToString.Trim()
                        'e.Row.Cells(gvcell_reportingdatetime).Text = CDate(Replace(reportingdate, replaceoffset, "")).ToString("dd-MMM-yyyy HH:mm:ss tt") + strServerOffset '& " (" & reportingdate.Substring(reportingdate.Length - 7, 7).ToString.Trim() & " GMT)"
                        ' replaceoffset = reportingdate.Substring(reportingdate.Length - 7, 7).ToString.Trim()
                        'e.Row.Cells(gvcell_reportingdatetime).Text = CDate(Replace(reportingdate, replaceoffset, "")).ToString("dd-MMM-yyyy HH:mm") & " (" & reportingdate.Substring(reportingdate.Length - 7, 7).ToString.Trim() & " GMT)"
                    End If
                End If


                'CType(e.Row.FindControl("imgScrDate"), ImageButton).Visible = False
                If e.Row.Cells(gvcell_vmysubjectno).Text.Trim() = "&nbsp;" Then
                    e.Row.Cells(gvcell_vmysubjectno).Text = ""
                End If

                If e.Row.Cells(gvc_medexscreenhdrno).Text.Trim() = "&nbsp;" Then
                    e.Row.Cells(gvc_medexscreenhdrno).Text = ""
                End If

                CType(e.Row.FindControl("txtmysubjectno"), TextBox).Text = e.Row.Cells(gvcell_vmysubjectno).Text.Trim()

                If e.Row.Cells(gvcell_subjectno).Text <> "0" Then
                    CType(e.Row.FindControl("imgExtra"), ImageButton).Visible = False
                    CType(e.Row.FindControl("imgExtra"), ImageButton).Visible = False
                    'added on 21-apr-10
                    CType(e.Row.FindControl("txtmysubjectno"), TextBox).Enabled = False

                    CType(e.Row.FindControl("txtmysubjectno"), TextBox).BorderColor = Drawing.Color.DarkGray
                    CType(e.Row.FindControl("txtmysubjectno"), TextBox).ForeColor = Drawing.Color.Black

                    isfirst = True
                    If e.Row.Cells(gvcell_crejected).Text.ToUpper <> "y" AndAlso _
                       e.Row.Cells(gvc_medexscreenhdrno).Text = "" Then
                        CType(e.Row.FindControl("imgScrDate"), ImageButton).Visible = True
                    End If
                End If
            End If
        Catch ex As Exception
            Me.showerrormessage(ex.Message, "...gvwworkspacesubjectmst_rowdatabound")
        End Try
    End Sub

    Protected Sub gvwWorkspaceSubjectMst_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvwWorkspaceSubjectMst.RowCancelingEdit

    End Sub

    Protected Sub gvwWorkspaceSubjectMst_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvwWorkspaceSubjectMst.RowEditing
        e.Cancel = True
    End Sub

    Protected Sub gvwWorkspaceSubjectMst_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvwWorkspaceSubjectMst.RowUpdating

    End Sub

#End Region

#Region "GVAudit"

    Protected Sub GVAudit_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVAudit.RowCreated

        If e.Row.RowType = DataControlRowType.DataRow Or _
                      e.Row.RowType = DataControlRowType.Header Or _
                      e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVCAudit_iAsnNo).Visible = False
            e.Row.Cells(GVCAudit_iTranN).Visible = False
            e.Row.Cells(GVCAudit_nWorkspaceSubjectHistoryId).Visible = False
            e.Row.Cells(GVCAudit_vWorkspaceSubjectId).Visible = False
        End If

    End Sub

    Protected Sub GVAudit_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVAudit.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                'e.Row.Cells(GVCAudit_dModifyOn).Text = Replace(e.Row.Cells(GVCAudit_dModifyOn).Text.Trim(), "&nbsp;", "")

                If Not Convert.ToString(e.Row.Cells(GVCAudit_dReportingDate).Text).Trim = "" Then
                    reportingdate = Replace(e.Row.Cells(GVCAudit_dReportingDate).Text.ToString.Trim(), "&nbsp;", "")
                    replaceoffset = reportingdate.Substring(reportingdate.Length - 7, 7).ToString.Trim()
                    e.Row.Cells(GVCAudit_dReportingDate).Text = CDate(Replace(reportingdate, replaceoffset, "")).ToString("dd-MMM-yyyy HH:mm") & " (" & reportingdate.Substring(reportingdate.Length - 7, 7).ToString.Trim() & " GMT)"

                    'e.Row.Cells(GVCAudit_dReportingDate).Text = CDate(Replace(reportingdate, replaceoffset, "")).ToString("dd-MMM-yyyy HH:mm:ss tt") + strServerOffset '& " (" & reportingdate.Substring(reportingdate.Length - 7, 7).ToString.Trim() & " GMT)"
                End If

                If Not Convert.ToString(e.Row.Cells(GVCAudit_dModifyOn).Text).Trim = "" Then
                    e.Row.Cells(GVCAudit_dModifyOn).Text = CType(Replace(e.Row.Cells(GVCAudit_dModifyOn).Text.Trim(), "&nbsp;", ""), Date).ToString("dd-MMM-yyyy HH:mm").Trim() + strServerOffset
                End If
            End If

        Catch ex As Exception
            Me.showerrormessage(ex.Message, "...GVAudit_RowDataBound")
        End Try
    End Sub

#End Region

#End Region

#Region "fill functions"

    Private Function fillscreeningdates(ByVal subjectid As String) As Boolean
        Dim ds_audittrail As New DataSet
        Dim dv_audittrail As New DataView
        Dim ds_WorkspaceSubjectMst As New DataSet
        Dim dvReportingtime As New DataView
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim reportingdate As String = String.Empty
        Dim reportingdate1 As Date
        Dim Validdate As Date
        Dim index As Integer = 0
        Dim ds_ProjectSp As New DataSet
        Dim Message As String = String.Empty
        Dim screenDates As String = String.Empty
        Try
            Me.rblScreeningDate.Items.Clear()

            '=========
            ds_WorkspaceSubjectMst = CType(ViewState(vs_gvdsworkspacesubjectmst), DataSet)
            dvReportingtime = ds_WorkspaceSubjectMst.Tables(0).DefaultView
            dvReportingtime.RowFilter = "vSubjectId = '" & subjectid.Trim & "'"
            reportingdate = dvReportingtime.ToTable.Rows(0)("dReportingDate").ToString
            reportingdate1 = CDate(reportingdate).ToString("dd-MMM-yyyy")

            If (Me.txtScreenDays.Text <> "") AndAlso (IsNumeric(Me.txtScreenDays.Text)) Then
                Validdate = reportingdate1.AddDays(-Convert.ToDouble(Me.txtScreenDays.Text))
            Else
                objcommon.ShowAlert("Entered Screening validation days are not in correct format", Me)
                Return False
            End If
            If Not objhelp.GetWorkspaceScreeningHdr("vWorkspaceId='" & Me.HParentWorkSpaceId.Value.ToString() & "' And cStatusindi<>'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ProjectSp, estr) Then
                Throw New Exception("Error while checking for Project Specific Screening Structure")
            End If
            If ds_ProjectSp.Tables(0).Rows.Count > 0 Then
                '                If Not objhelp.GetData("MedexWorkspaceScreeningHdr", "top 1 *", " vWorkspaceId='" + Me.HParentWorkSpaceId.Value.Trim() + "' And vSubjectId='" + subjectid.ToString.Trim() + "' order by dScreenDate desc", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_audittrail, estr) Then
                If Not objhelp.GetData("MedexWorkspaceScreeningHdr", "*", " vWorkspaceId='" + Me.HParentWorkSpaceId.Value.Trim() + "' And vSubjectId='" + subjectid.ToString.Trim() + "' order by dScreenDate desc", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_audittrail, estr) Then
                End If
                If ds_audittrail.Tables(0).Rows.Count > 0 Then
                    wstr = "vsubjectid='" & subjectid.Trim & "' And cast(convert(varchar(11),dscreendate,113) as smalldatetime)>= cast(convert(varchar(11),cast('" _
                                          & Validdate.ToString & "' as datetime),113)as smalldatetime) And cast(convert(varchar(11),dscreendate,113)as smalldatetime)<=cast(convert(varchar(11),cast('" & CDate(reportingdate1).ToString & "'as datetime),113)as smalldatetime)" & _
                                          " AND vWorkspaceId='" + Me.HParentWorkSpaceId.Value.Trim() + "'" 'And nMedExScreeningHdrNo=" + ds_audittrail.Tables(0).Rows(0).Item("nMedexScreeningHdrNo").ToString() + ""
                Else
                    Me.objcommon.ShowAlert("No project specific screening found", Me.Page)
                    Me.MPEdate.Hide()
                    fillgridview()
                    Return False
                End If
                Message = "No project specific screening found"
            Else

                wstr = "vsubjectid='" & subjectid.Trim & "' And cast(convert(varchar(11),dscreendate,113) as smalldatetime)>= cast(convert(varchar(11),cast('" _
                       & Validdate.ToString & "' as datetime),113)as smalldatetime) And cast(convert(varchar(11),dscreendate,113)as smalldatetime)<=cast(convert(varchar(11),cast('" & CDate(reportingdate1).ToString & "'as datetime),113)as smalldatetime)" & _
                       " AND nMedExScreeningHdrNo not in(SELECT nMedExScreeningHdrNo FROM MedExWorkSpaceScreeningHdr WHERE vSubjectId='" + subjectid.Trim + "')"
                Message = "no screening found"
            End If

            If Not Me.objhelp.View_GetEligibleScreeningHdrDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                             ds_audittrail, estr) Then
                Return False
            End If

            If ds_audittrail.Tables(0).Rows.Count <= 0 Then
                Me.objcommon.ShowAlert(Message, Me.Page)
                Me.MPEdate.Hide()
                fillgridview()
                Return False
            End If

            dv_audittrail = ds_audittrail.Tables(0).DefaultView()

            ' Change By Jeet Patel on 20-Jun-2015 
            'dv_audittrail.Sort = "dScreenDate,cReviewType desc"
            dv_audittrail.Sort = "cReviewType desc,nMedExScreeningHdrNo desc"
            '=======================================================

            Me.rblScreeningDate.Items.Clear()
            For Each dr As DataRow In dv_audittrail.ToTable().Rows
                index += 1
                If Not screenDates.Contains(dr("dscreendate")) Then
                    If screenDates <> "" Then
                        screenDates = screenDates + "," + dr("dscreendate")
                    Else
                        screenDates = dr("dscreendate")
                    End If
                    If dr("cIsEligible").ToString.ToUpper = "Y" AndAlso dr("cReviewType").ToString.ToUpper = "P" Then
                        Me.rblScreeningDate.Items.Add(New ListItem(CDate(dr("dscreendate")).GetDateTimeFormats()(6).Trim(), dr("nMedExScreeningHdrNo")))
                        'If Me.rblScreeningDate.Items.Count > 0 Then
                        If rblScreeningDate.SelectedIndex < 0 Then
                            Me.rblScreeningDate.Items(Me.rblScreeningDate.Items.Count - 1).Selected = True
                        End If
                    ElseIf dr("cIsEligible").ToString.ToUpper = "Y" AndAlso dr("cReviewType").ToString.ToUpper = "E" Then
                        Me.rblScreeningDate.Items.Add(New ListItem(CDate(dr("dscreendate")).GetDateTimeFormats()(6).Trim(), dr("nMedExScreeningHdrNo"), False))
                    ElseIf dr("cIsEligible").ToString.ToUpper <> "Y" Then
                        Me.rblScreeningDate.Items.Add(New ListItem(CDate(dr("dscreendate")).GetDateTimeFormats()(6).Trim(), dr("nMedExScreeningHdrNo"), False))
                    End If
                End If
            Next dr

            If rblScreeningDate.SelectedIndex < 0 Then
                btnOK.Visible = False
            Else
                btnOK.Visible = True
            End If

            Return True
        Catch ex As Exception
            Me.showerrormessage(ex.Message, "...fillscreeningdates")
            Return False
        End Try
    End Function

    Private Function fillvalues() As Boolean
        Try
            fillgridview()
            Return True

        Catch ex As Exception
            showerrormessage(ex.Message, "...fillvalues")
            Return False
        End Try
    End Function

    Private Sub fillgridview()
        Dim dsworkspacesubjectmst As New DataSet
        Dim estr As String = String.Empty
        Dim qstr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim workspaceid As String = String.Empty

        Try
            workspaceid = Me.HProjectId.Value.Trim()
            'SDNidhi -------------------------
            'qstr = "select distinct * from view_workspacesubjectmst where vworkspaceid='" & workspaceid & "' and iperiod=1 " & " and crejectionflag <> 'y'"
            '---------------------------------
            wstr = " vworkspaceid='" & workspaceid & "' and iperiod=1 " & " and crejectionflag <> 'y'"
            'added order by on 19-mar-2010
            wstr += " and cstatusindi <> 'D' order by cast(dreportingdate as datetimeoffset)"
            '*********************
            Me.objhelp.GetViewWorkspaceSubjectMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                dsworkspacesubjectmst, estr)

            gvwWorkspaceSubjectMst.DataSource = dsworkspacesubjectmst
            gvwWorkspaceSubjectMst.DataBind()
            Me.ViewState(vs_gvdsworkspacesubjectmst) = dsworkspacesubjectmst

        Catch ex As Exception
            Me.showerrormessage(ex.Message, "...fillgridview")
        End Try
    End Sub

    Private Sub fillrejectdropdown()
        Dim dsreject As New DataSet

        If Not objhelp.GetReasonMst("vactivityid='" + GeneralModule.Act_Attendance + "' and cstatusindi<>'d'", _
            WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsreject, estr_retu) Then

            'If Not objhelp.GetViewWorkspaceSubjectMst("vWorkSpaceId='" + Me.HProjectId.Value.Trim() + "' and iMySubjectNo<0 and cstatusindi<>'d'", _
            '    WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsreject, estr_retu) Then
            objcommon.ShowAlert("reason drop down could not be loaded", Me)
            Exit Sub
        End If

        Me.ddlReject.DataSource = dsreject
        Me.ddlReject.DataValueField = "nreasonno"
        Me.ddlReject.DataTextField = "vreasondesc"
        Me.ddlReject.DataBind()
        Me.ddlReject.Items.Insert(0, New ListItem("select reason", "0"))

    End Sub

    Private Function fillsubject() As Boolean
        Dim dvsubject As New DataView
        Dim dtsubject As New DataTable
        Dim dssubject As New DataSet
        Try
            ddlSubject.Items.Clear()
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            dssubject = Me.ViewState(vs_gvdsworkspacesubjectmst)

            dvsubject = dssubject.Tables(0).DefaultView
            dvsubject.RowFilter = "imysubjectno <= 0"
            dvsubject.Sort = "imysubjectno desc"
            dtsubject = dvsubject.ToTable()
            'dtsubject.Columns.Add("Valuefield", GetType(String))
            For Each dr_row In dtsubject.Rows
                Dim list As New ListItem
                list.Value = dr_row("vSubjectId")
                list.Text = dr_row("fullnamewithno")
                Me.ddlSubject.Items.Add(list)
            Next
            'Me.ddlSubject.DataSource = dtsubject
            'Me.ddlSubject.DataValueField = "ValueField"
            'Me.ddlSubject.DataTextField = "fullnamewithno"
            'Me.ddlSubject.DataBind()
            Me.ddlSubject.Items.Insert(0, New ListItem("Select subject", "0"))

            Return True
        Catch ex As Exception
            showerrormessage(ex.Message, "...fillsubject")
            Return False
        End Try

    End Function

    Private Function validatesubjectno(ByVal dtold As DataTable, ByVal subjectno As String) As Boolean
        For Each dr As Data.DataRow In dtold.Rows
            If subjectno.ToUpper.Trim <> "" AndAlso subjectno.ToUpper.Trim = dr("vmysubjectno").ToString.ToUpper.Trim Then
                Return False
            End If
        Next
        Return True

    End Function

    Private Function assignscreening() As Boolean
        Dim subjectid As String = String.Empty
        Dim type As String = String.Empty
        Dim ds_stagemat As DataSet
        Dim estr As String = String.Empty
        Dim isvalidsubject As Boolean = True
        Dim isscrdate As Boolean
        Dim vmysubjectno As String = String.Empty
        Dim dssubjectmst As New DataSet
        Dim dtold As New DataTable
        Dim dvold As New DataView
        Dim dr As DataRow = Nothing
        Dim wstr As String = String.Empty
        Dim workspaceid As String = String.Empty
        Dim ds_PrevSubAssignmentDtl As New DataSet

        Try
            workspaceid = Me.HProjectId.Value.Trim()
            isscrdate = CType(ViewState(vs_isscrdate), Boolean)
            ''Added by Aaditya on 02-Jun-2015 for new validation that previous project subject should not remain to assign then next project can proceed
            ds_PrevSubAssignmentDtl = objhelp.ProcedureExecute("dbo.Proc_GetPreviousSubjectAssignmentDetail", workspaceid)

            If Not IsNothing(ds_PrevSubAssignmentDtl) AndAlso ds_PrevSubAssignmentDtl.Tables(1).Rows.Count > 0 Then
                objcommon.ShowAlert("Previous Project " + ds_PrevSubAssignmentDtl.Tables(1).Rows(0)("vProjectNo").ToString.Trim() + " Assignment is Pending. First Assign/Reject Them And Then Proceed.", Me.Page)
                Return False
            End If
            ''Ended by Aaditya
            If Me.ddlSubject.SelectedIndex > 0 AndAlso isscrdate = False Then
                'Me.divSearch.Attributes.Add("style", "dispaly:none")
                Me.MPEdate.Hide()

                If Not assignvalues(True) Then
                    isvalidsubject = False
                End If

                If Not Me.ddlSubject.SelectedIndex = 0 And isvalidsubject = True Then
                    If Not Me.assignupdatedvalues(Me.ddlSubject.SelectedItem.Value.Trim(), WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, Me.ViewState(vs_type).ToString()) Then
                        Return False
                    End If
                    ds_stagemat = New DataSet
                    ds_stagemat.Tables.Add(CType(Me.ViewState(vs_workspacesubjectmst), Data.DataTable).Copy())
                    ds_stagemat.Tables(0).TableName = "view_workspacesubjectmst"   ' new values on the form to be updated

                    If Not objlambda.Save_InsertWorkspaceSubjectMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_WorkspaceSubjectMst, ds_stagemat, Me.Session(S_UserID), estr) Then
                        objcommon.ShowAlert("error while saving :" + estr, Me.Page)
                        Return False
                    End If
                    isreplace = False
                End If
                Me.ddlSubject.ClearSelection()
                fillgridview()

                Me.ViewState(vs_gvmysubjectno) = Nothing
                Return False

            ElseIf isscrdate = True Then
                subjectid = CType(ViewState(vs_subjectid), String)
                '=============
                wstr += "vworkspaceid='" & workspaceid & "' and vsubjectid = '" + subjectid + "'"

                If Not Me.objhelp.GetWorkspaceSubjectMaster(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                dssubjectmst, estr) Then
                    Me.objcommon.ShowAlert(estr, Me.Page())
                    Return False
                End If
                dtold = dssubjectmst.Tables(0)

                For Each dr In dtold.Rows
                    dr("vworkspaceid") = workspaceid
                    dr("nmedexscreeninghdrno") = Me.rblScreeningDate.SelectedValue
                    dr("imodifyby") = Me.Session(S_UserID)
                    dr.AcceptChanges()
                Next
                dtold.AcceptChanges()
                '================

                ds_stagemat = New DataSet
                ds_stagemat.Tables.Add(dtold.Copy)
                ds_stagemat.Tables(0).TableName = "view_workspacesubjectmst"   ' new values on the form to be updated

                If Not objlambda.Save_InsertWorkspaceSubjectMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_WorkspaceSubjectMst, ds_stagemat, Me.Session(S_UserID), estr) Then
                    objcommon.ShowAlert("error while saving :" + estr, Me.Page)
                    Return False
                End If
                fillgridview()

                'divSearch.Attributes.Add("style", "display:none")
                Me.MPEdate.Hide()

                Me.objcommon.ShowAlert("screening date assigned successfully !", Me)
                Me.ViewState(vs_gvmysubjectno) = Nothing
                'isscrdate = false
                ViewState(vs_isscrdate) = False
                fillgridview()
                Return False
            End If

            subjectid = CType(ViewState(vs_subjectid), String)
            type = CType(ViewState(vs_type), String)
            save(subjectid, type)

            Return True
        Catch ex As Exception
            showerrormessage("error while saving workspacesubjectmst", "...assignscreening")
            Return False
        End Try
    End Function

#End Region

    'protected sub rblscreeningdate_selectedindexchanged1(byval sender as object, byval e as system.eventargs)
    '    dim subjectid as string = string.empty
    '    dim type as string = string.empty
    '    dim ds_stagemat as dataset
    '    dim estr as string = ""
    '    dim isvalidsubject as boolean = true
    '    dim isscrdate as boolean
    '    dim vmysubjectno as string = string.empty
    '    dim dssubjectmst as new dataset
    '    dim dtold as new datatable
    '    dim dvold as new dataview
    '    dim dr as datarow = nothing
    '    dim wstr as string = ""
    '    dim workspaceid as string = ""

    '    try
    '        workspaceid = me.hprojectid.value.trim()

    '        isscrdate = ctype(viewstate(vs_isscrdate), boolean)

    '        if me.ddlsubject.selectedindex > 0 andalso isscrdate = false then

    '            me.divsearch.attributes.add("style", "dispaly:none")

    '            isreplace = true

    '            if not assignvalues(true) then
    '                isvalidsubject = false
    '            end if

    '            if not me.ddlsubject.selectedindex = 0 and isvalidsubject = true then
    '                if not me.assignupdatedvalues(me.ddlsubject.selecteditem.value.trim(), ws_lambda.dataobjopensavemodeenum.dataobjopenmode_edit, "save") then
    '                    exit sub
    '                end if
    '                ds_stagemat = new dataset
    '                ds_stagemat.tables.add(ctype(me.viewstate(vs_workspacesubjectmst), data.datatable).copy())
    '                ds_stagemat.tables(0).tablename = "view_workspacesubjectmst"   ' new values on the form to be updated

    '                if not objlambda.save_insertworkspacesubjectmst(ws_lambda.dataobjopensavemodeenum.dataobjopenmode_edit, ws_lambda.masterentriesenum.masterentriesenum_workspacesubjectmst, ds_stagemat, me.session(s_userid), estr) then
    '                    objcommon.showalert("error while saving :" + estr, me.page)
    '                    exit sub
    '                end if

    '            end if
    '            fillgridview()

    '            me.viewstate(vs_gvmysubjectno) = nothing

    '            exit sub


    '        elseif isscrdate = true then

    '            'if not assignvalues(true) then
    '            '    isvalidsubject = false
    '            'end if

    '            subjectid = ctype(viewstate(vs_subjectid), string)

    '            '=============
    '            wstr += "vworkspaceid='" & workspaceid & "' and vsubjectid = '" + subjectid + "'"

    '            if not me.objhelp.getworkspacesubjectmaster(wstr, ws_helpdbtable.dataretrievalmodeenum.datatable_withwherecondition, _
    '            dssubjectmst, estr) then
    '                me.objcommon.showalert(estr, me.page())
    '                exit sub
    '            end if
    '            dtold = dssubjectmst.tables(0)

    '            for each dr in dtold.rows
    '                dr("vworkspaceid") = workspaceid
    '                dr("nmedexscreeninghdrno") = me.rblscreeningdate.selectedvalue
    '                dr("imodifyby") = me.session(s_userid)
    '                dr.acceptchanges()
    '            next
    '            dtold.acceptchanges()
    '            '================

    '            ds_stagemat = new dataset
    '            ds_stagemat.tables.add(dtold.copy)
    '            ds_stagemat.tables(0).tablename = "view_workspacesubjectmst"   ' new values on the form to be updated

    '            if not objlambda.save_insertworkspacesubjectmst(ws_lambda.dataobjopensavemodeenum.dataobjopenmode_edit, ws_lambda.masterentriesenum.masterentriesenum_workspacesubjectmst, ds_stagemat, me.session(s_userid), estr) then
    '                objcommon.showalert("error while saving :" + estr, me.page)
    '                exit sub
    '            end if


    '            fillgridview()

    '            divsearch.attributes.add("style", "display:none")

    '            me.objcommon.showalert("screening date assigned successfully", me)

    '            me.viewstate(vs_gvmysubjectno) = nothing
    '            'isscrdate = false
    '            viewstate(vs_isscrdate) = false

    '            exit sub



    '        end if

    '        subjectid = ctype(viewstate(vs_subjectid), string)
    '        type = ctype(viewstate(vs_type), string)
    '        save(subjectid, type)

    '    catch ex as exception
    '        showerrormessage("error while saving workspacesubjectmst", "")
    '    end try

    'end sub

#Region "Button Event"

    Protected Sub btnok_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk2.Click
        If Not assignscreening() Then
            Exit Sub
        End If
    End Sub

    Protected Sub btnscreenclose_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnScreenClose.Click
        'divSearch.Attributes.Add("style", "display:none")
        MPEdate.Hide()
        fillgridview()
    End Sub

    Protected Sub btnDivOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDivOK.Click
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_workspacesubmst As New DataSet
        Dim dt_workspacesubmst As New DataTable
        Dim dv_workspacesubmst As New DataView
        Dim MySubNo As String = String.Empty
        Dim index As Integer = CType(ViewState(vs_Index), Integer)

        Try
            wStr = " vworkspacesubjectid = '" & Me.gvwWorkspaceSubjectMst.Rows(index).Cells(gvc_code).Text.Trim() & "'"

            If Not objhelp.GetViewWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                      ds_workspacesubmst, eStr) Then
                Throw New Exception(eStr)
            End If

            For Each dr As DataRow In ds_workspacesubmst.Tables(0).Rows
                dr("vmysubjectno") = CType(Me.gvwWorkspaceSubjectMst.Rows(index).FindControl("txtmysubjectno"), TextBox).Text.Trim
                dr("vRemarks") = Me.txtRemark.Text.Trim()
                dr("iModifyBy") = Me.Session(S_UserID)
            Next
            ds_workspacesubmst.Tables(0).AcceptChanges()

            If Not objlambda.Save_InsertWorkspaceSubjectMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                                            WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_WorkspaceSubjectMst, _
                                            ds_workspacesubmst, Me.Session(S_UserID), estr_retu) Then
                Throw New Exception(estr_retu)
                resetpage()
            End If
            Me.txtRemark.Text = ""
            Me.mpeDialog.Hide()
            fillgridview()
            CType(gvwWorkspaceSubjectMst.Rows(index).FindControl("txtmysubjectno"), TextBox).Enabled = False

        Catch ex As Exception
            Me.showerrormessage(ex.Message, "...btnDivOK_Click")
        End Try
    End Sub

    Protected Sub btnDivCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDivCancel.Click
        Me.txtRemark.Text = ""
        mpeDialog.Hide()
        fillgridview()
    End Sub

#End Region

#Region "Functions"

    Private Function ShowAudit() As Boolean
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_WorkspaceSubjectMstHistory As New DataSet

        Try
            wStr = " vWorkspaceSubjectId = '" & Me.ViewState(gvc_code).ToString & "' order by iTranNo"
            If Not objhelp.View_WorkSpaceSubjectMstHistory(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                            ds_WorkspaceSubjectMstHistory, eStr) Then

                Throw New Exception(eStr)
            End If

            If ds_WorkspaceSubjectMstHistory.Tables(0).Rows.Count > 0 Then
                GVAudit.DataSource = ds_WorkspaceSubjectMstHistory
                GVAudit.DataBind()

            ElseIf ds_WorkspaceSubjectMstHistory.Tables(0).Rows.Count <= 0 Then
                objcommon.ShowAlert("No Audit Trail present", Me)
                Return False
            End If

            Return True
        Catch ex As Exception
            showerrormessage(ex.Message, "...ShowAudit")
            Return False
        End Try

    End Function

    Public Function CheckInDosingDetail(ByVal iMySubjectNo As String) As Boolean
        Dim Wstr As String = String.Empty
        Dim Estr As String = String.Empty
        Dim Ds_DosingDetails As New DataSet

        Try
            Wstr = "vWorkSpaceId='" & HProjectId.Value.ToString() & "' and iMySubjectNo='" & iMySubjectNo & "' and dDosedOn is NOT NULL and cStatusIndi<>'D' and iPeriod=1"
            If Not objhelp.View_DosingDetail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_DosingDetails, Estr) Then
                Me.objcommon.ShowAlert("Error While Getting Data From View_DosingDetail", Me.Page)
                Return False
            End If
            If Ds_DosingDetails.Tables(0).Rows.Count > 0 Then
                '' ****************************Dosing  Done ***************************''
                Return False
                Exit Function
            End If
            ' *********************************Dosing Not Done **************************'
            Return True

        Catch ex As Exception
            Me.showerrormessage(ex.Message, "...CheckInDosingDetail")
            Return False
        End Try
    End Function

#End Region

#Region "GetParentWorkSpaceId"
    Private Function GetParentWorkSpaceId() As Boolean
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds As New DataSet
        wStr = "vWorkspaceId='" + HProjectId.Value + "'"
        If Not objhelp.GetData("view_workspaceprotocoldetail", "vParentWorkspaceId,cIsTestSite", wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr_retu) Then
            Throw New Exception(estr_retu)
        End If
        If ds.Tables(0).Rows.Count > 0 And IsDBNull(ds.Tables(0).Rows(0).Item("vParentWorkspaceId")) = False Then
            HParentWorkSpaceId.Value = ds.Tables(0).Rows(0).Item("vParentWorkspaceId")

            If IsDBNull(ds.Tables(0).Rows(0).Item("cIsTestSite")) = False Then
                HIsTestSite.Value = ds.Tables(0).Rows(0).Item("cIsTestSite")
            Else
                HIsTestSite.Value = ""
            End If

        Else
            HParentWorkSpaceId.Value = Me.HProjectId.Value

        End If
    End Function
#End Region

#Region "WEb Method"
    <Web.Services.WebMethod()> _
    Public Shared Function SubjectScreeningDCFValidation(ByVal MedExScreeningHdrNo As String) As String
        Dim wStr As String
        Dim ds_Validation As DataSet

        Try
            wStr = MedExScreeningHdrNo
            ds_Validation = objHelpdb.ProcedureExecute("Proc_ScreeningAssignmentValidation", wStr)

            If Not ds_Validation Is Nothing AndAlso ds_Validation.Tables(0).Rows.Count Then
                Return JsonConvert.SerializeObject(ds_Validation.Tables(0).Rows(0)(0))
            End If
            Return JsonConvert.SerializeObject("False")
        Catch ex As Exception
            Return JsonConvert.SerializeObject("False")
        End Try

    End Function

#End Region



End Class
