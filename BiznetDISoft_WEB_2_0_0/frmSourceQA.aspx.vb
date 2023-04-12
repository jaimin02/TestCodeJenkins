
Partial Class frmSourceQA
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()


    Private Const VS_Choice As String = "Choice"
    Private eStr_Retu As String = ""
    Private Const GVCell_SrNo As Integer = 0
    Private Const GVCell_SubjectNo As Integer = 1
    Private Const GVCell_SubjectId As Integer = 2
    Private Const GVCell_Initials As Integer = 3
    Private Const GVCell_SourceDoc As Integer = 4
    Private Const GVCell_LabReport As Integer = 5
    Private Const GVCell_status As Integer = 6
    Private Const GVCell_Comments As Integer = 7
    Private Const GVCell_LOCK As Integer = 8
    Private Const GVCell_MedexScreeningHdrNo As Integer = 9
    Private Const GVCell_cRejectionflag As Integer = 10
    Private Const GVCell_TotalComment As Integer = 11
    Private Const GVCell_Name As Integer = 11




    Private Const GVCQC_SrNo As Integer = 0
    Private Const GVCQC_SubjectMasterQCNo As Integer = 1
    Private Const GVCQC_SubjectId As Integer = 2
    Private Const GVCQC_Subject As Integer = 3
    Private Const GVCQC_QCComment As Integer = 4
    Private Const GVCQC_QCFlag As Integer = 5
    Private Const GVCQC_QCBy As Integer = 6
    Private Const GVCQC_QCDate As Integer = 7
    Private Const GVCQC_Response As Integer = 8
    Private Const GVCQC_ResponseGivenBy As Integer = 9
    Private Const GVCQC_ResponseGivenOn As Integer = 10
    Private Const GVCQC_LnkResponse As Integer = 11
    Private Const GVCQC_Delete As Integer = 12


    Private Const VS_dtMedEx_Fill As String = "dtMedEx_Fill"
    Private Const VS_DtQC As String = "dtQC"
    Private Const VS_SubjectName As String = "SubjectName"
    Private Const VS_WSSDDetailId As String = "vWorkSpaceSubjectDocDetailId"
    Private Const VS_dsSourceDocDtl As String = "SourceDocDtl"
    Private Const VS_dsScreeningLockDtl As String = "ScreeningLockDtl"
    Private Const VS_dsWorkspaceSubjectMst As String = "ds_WorkspaceSubjectMst"
    Private Const VS_dsSubDtl As String = "ds_SubDtl"
    Private Const VS_dsMaxScreeningLockDtl As String = "ds_MaxScreeningLockDtl"
    Private Const VS_SubjectId As String = "SubjectId"
    Private Const VS_MedexScreeningHdrNo As String = "MedexScreeningHdrNo"
    Private Const str_Activityid As String = "Activityid"
    Private Const VS_dsComment As String = "ds_comment"
    Private Const QAonPIFActID As String = "1186"


    Private Str_SubjectId As String = String.Empty

    Private Str_OperationPath As String = String.Empty


#End Region

#Region "Page_Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            GenCall()
        End If
    End Sub
#End Region

#Region "GENCALL"
    Private Function GenCall() As Boolean

        Dim dt_WorkspaceSubjectmst As DataTable = Nothing
        Dim ds As DataSet = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Choice = Me.Request.QueryString("Mode")

            Me.ViewState(VS_Choice) = Choice  'To use it while comment saving

            If Not GenCall_Data(Choice, ds) Then ' For Data Retrieval
                Exit Function
            End If
            If Not GenCall_ShowUI(Choice, dt_WorkspaceSubjectmst) Then 'For Displaying Data 
                Exit Function
            End If
            GenCall = True


        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function
#End Region

#Region " GenCall_Data "

    Private Function GenCall_Data(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                            ByRef dt_Dist_Retu As DataSet) As Boolean

        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_SourceDocDtl As New DataSet
        Dim ds_ScreeningLockDtl As New DataSet

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Not objHelp.GetSourceDocDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, _
                                    ds_SourceDocDtl, eStr) Then

                Throw New Exception(eStr)
            End If

            ViewState(VS_dsSourceDocDtl) = ds_SourceDocDtl

            If Not objHelp.GetScreeningLockDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                                                ds_ScreeningLockDtl, eStr) Then

                Throw New Exception(eStr)
            End If

            ViewState(VS_dsScreeningLockDtl) = ds_ScreeningLockDtl
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            GenCall_Data = False
        Finally
        End Try
    End Function

#End Region

#Region "HideMenu"

    Private Sub HideMenu()
        Dim tmpMenu As Menu
        Dim tmpddlprofile As DropDownList

        tmpMenu = CType(Me.Master.FindControl("menu1"), Menu)
        tmpddlprofile = CType(Me.Master.FindControl("ddlProfile"), DropDownList)

        If Not tmpMenu Is Nothing Then
            tmpMenu.Visible = False
        End If

        If Not tmpddlprofile Is Nothing Then
            tmpddlprofile.Enabled = False
        End If

    End Sub

#End Region

#Region "GENCALL_SHOW_UI"

    Private Function GenCall_ShowUI(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, ByVal dt_WorkspaceSubjectmst As DataTable) As Boolean
        Dim dsMedexInfo As New DataSet
        Dim WorkspaceId As String = ""
        Dim dvSourceDoc As New DataView
        Dim dtSourceDoc As New DataTable
        Dim ActId As String = ""
        Dim Act As String = ""
        Dim Period As String = ""
        Dim estr As String = ""
        Dim Wstr As String = ""
        Dim Usertype() As String

        lblActivityName.Text = ""
        Page.Title = ":: Source QA  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
        If Not IsNothing(Me.Request.QueryString("workspaceid")) Then

            Me.HFWorkspaceId.Value = Me.Request.QueryString("workspaceid").Trim()
            Me.HFActivityId.Value = Me.Request.QueryString("vActivityId").Trim()
            Me.HFNodeId.Value = Me.Request.QueryString("NodeID").Trim()
            Me.lblActivityName.Text = "Activity : " + Me.Request.QueryString("Act").Trim()
            ViewState(str_Activityid) = HFActivityId.Value
        End If

        dtSourceDoc = CType(ViewState(VS_dsSourceDocDtl), DataSet).Tables(0)
        dvSourceDoc = dtSourceDoc.DefaultView
        dvSourceDoc.RowFilter = "vActivityID = '" + Me.HFActivityId.Value.Trim + "'"


        'Added by Vikas For Screening Details in view mode If schemaId is present
        '=============================================================================================
        If Not Me.Request.QueryString("SchemaId") Is Nothing Then
            Me.HFPath.Value = "frmArchiveSubjectScreening.aspx?"
        Else
            If Not dvSourceDoc.ToTable.Rows.Count <= 0 Then
                Me.HFPath.Value = dvSourceDoc.ToTable.Rows(0)("vOperationPath")
                Usertype = dvSourceDoc.ToTable.Rows(0)("vCommentUserType").ToString.Split(",")

                For index As Integer = 0 To Usertype.Length - 1
                    If Usertype(index) = Me.Session(S_UserType) Then
                        Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View
                    End If
                Next
            End If
        End If
        ' ================================================================================================

        HideMenu()

        If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
            Imglockall.Enabled = False
            ImgUnlockall.Enabled = False
        End If
        ' added by Megha on 15-May-2012 for view mode and type=Report
        If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
            If (Not Me.Request.QueryString("WorkSpaceId") Is Nothing) AndAlso (Not Me.Request.QueryString("Type") Is Nothing) Then
                Me.Imglockall.Visible = False
                Me.ImgUnlockall.Visible = False
                Me.lblLockall.Visible = False
                Me.lblUnlockall.Visible = False
                Me.BtnBack.Visible = False
            End If
        End If
        ' ========================
        If ViewState(str_Activityid) = QAonPIFActID Then
            Me.Imglockall.Visible = False
            Me.lblLockall.Visible = False
            Me.ImgUnlockall.Visible = False
            Me.lblUnlockall.Visible = False
        End If


        If Not FillGridView() Then
            GenCall_ShowUI = False
            Exit Function
        End If

        GenCall_ShowUI = True

    End Function

#End Region

#Region "FillGridView"

    Private Function FillGridView() As Boolean
        Dim wstr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_WorkspaceSubjectMst As DataSet = Nothing
        Dim ds_MaxScreeningLockDtl As DataSet = Nothing
        Dim Ds_comment As DataSet = Nothing
        Dim subjectscreeningno As String = String.Empty



        Try

            ViewState(VS_dsMaxScreeningLockDtl) = Nothing

            wstr = "vWorkspaceid= '" + Me.HFWorkspaceId.Value.ToString.Trim + "'  and cStatusIndi<>'D'"

            If Not objHelp.View_MaxScreeningLockDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                 ds_MaxScreeningLockDtl, eStr) Then

                Throw New Exception(eStr)
            End If

            If ds_MaxScreeningLockDtl.Tables(0).Rows.Count > 0 Then

                ViewState(VS_dsMaxScreeningLockDtl) = ds_MaxScreeningLockDtl
            End If

            wstr = "vWorkspaceid= '" + Me.HFWorkspaceId.Value.ToString.Trim + "' And iPeriod =1  And cStatusIndi<>'D'  And iMysubjectNo >0  order By dReportingDate"

            If Not objHelp.GetViewWorkspaceSubjectMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                 ds_WorkspaceSubjectMst, eStr) Then

                Throw New Exception(eStr)
            End If

            ViewState(VS_dsWorkspaceSubjectMst) = ds_WorkspaceSubjectMst

            For i As Integer = 0 To ds_WorkspaceSubjectMst.Tables(0).Rows.Count - 1
                If Not ds_WorkspaceSubjectMst.Tables(0).Rows(i).Item("nMedExScreeningHdrNo").ToString = "" Then
                    subjectscreeningno += "'" + ds_WorkspaceSubjectMst.Tables(0).Rows(i).Item("nMedExScreeningHdrNo").ToString + "',"
                End If
            Next
            If subjectscreeningno.Trim() <> "" Then


                subjectscreeningno = subjectscreeningno.Substring(0, subjectscreeningno.Length - 1)

                wstr = "nMedexScreeningHdrNo in (" + subjectscreeningno.ToString() + ")"
                If Not Me.objHelp.View_MedexScreeningHdrQc(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                       Ds_comment, eStr) Then
                    Me.ShowErrorMessage(eStr, eStr)
                    Return False
                    Exit Function
                End If





                Me.ViewState(VS_dsComment) = Ds_comment
                '----------------
                If Not ds_WorkspaceSubjectMst Is Nothing Then
                    If ds_WorkspaceSubjectMst.Tables(0).Rows.Count > 0 Then
                        Me.gvSubjectQC.DataSource = ds_WorkspaceSubjectMst
                        Me.gvSubjectQC.DataBind()
                    Else
                        Me.gvSubjectQC.EmptyDataText = "No data found"
                        Me.gvSubjectQC.DataBind()
                    End If

                End If

                '----------------
            Else
                Me.gvSubjectQC.EmptyDataText = "No data found"
                Me.gvSubjectQC.DataBind()
            End If
            If ds_WorkspaceSubjectMst.Tables(0).Rows.Count > 0 Then
                Me.HFProjectNo.Value = ds_WorkspaceSubjectMst.Tables(0).Rows(0)("vProjectNo")
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("Error while Binding Grid", ex.Message)
            Return False
        End Try

    End Function
#End Region

#Region "Click and SelectedIndexChanged Event"

    Private Function chkSampleID(ByVal SubjectID As String) As Boolean
        Dim wstr As String = String.Empty
        Dim ds_SampleDetail As New DataSet
        Dim eStr As String = String.Empty
        Dim dv_Sampldetail As New DataView

        Try
            wstr = "vWorkSpaceid = '" + Me.HFWorkspaceId.Value + "' and iNodeId = " + Me.HFNodeId.Value + _
                   " and vSubjectId = '" + SubjectID + "'"

            If Not objHelp.GetSampleDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                   ds_SampleDetail, eStr) Then

                Throw New Exception(eStr)

            End If

            If ds_SampleDetail.Tables(0).Rows.Count <= 0 Then
                Me.ObjCommon.ShowAlert("No Sample is present for selected Subject in selected Project and Activity", Me)
                Return False
            ElseIf ds_SampleDetail.Tables(0).Rows.Count > 0 Then
                dv_Sampldetail = ds_SampleDetail.Tables(0).DefaultView
                dv_Sampldetail.RowFilter = " cApprovalFlag = 'Y'"
                If dv_Sampldetail.ToTable.Rows.Count <= 0 Then
                    Me.ObjCommon.ShowAlert("Sample is in process in Lab", Me)
                    Return False
                End If
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage("", ex.Message)
            Return False
        End Try


    End Function

    Protected Sub Imglockall_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Imglockall.Click

        If Not LockUnlockScreening("LOCK") Then
            ObjCommon.ShowAlert("Error while saving details", Me)
        End If
        Me.ObjCommon.ShowAlert("Source data of project Locked successfully", Me)
        FillGridView()


    End Sub

    Protected Sub ImgUnlockall_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgUnlockall.Click

        If Not LockUnlockScreening("UNLOCK") Then
            ObjCommon.ShowAlert("Error while saving details", Me)
        End If
        Me.ObjCommon.ShowAlert("Source data of project Unlocked successfully", Me)
        FillGridView()


    End Sub

    Private Function LockUnlockScreening(ByVal Type As String, Optional ByVal IslockAll As Boolean = True) As Boolean
        Dim ds_ScreeningLockDtl As New DataSet
        Dim ds_WorkspaceSubjectMst As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim dr_ScrLockDtl As DataRow
        Try
            If IslockAll = False Then
                ds_WorkspaceSubjectMst = CType(ViewState(VS_dsSubDtl), DataSet)
                ds_ScreeningLockDtl = CType(ViewState(VS_dsScreeningLockDtl), DataSet)
            ElseIf IslockAll = True Then

                ds_ScreeningLockDtl = CType(ViewState(VS_dsScreeningLockDtl), DataSet)
                ds_WorkspaceSubjectMst = CType(ViewState(VS_dsWorkspaceSubjectMst), DataSet)
                'dr_ScrLockDtl = ds_ScreeningLockDtl.Tables(0).NewRow
            End If


            For index As Integer = 0 To ds_WorkspaceSubjectMst.Tables(0).Rows.Count - 1
                dr_ScrLockDtl = ds_ScreeningLockDtl.Tables(0).NewRow
                dr_ScrLockDtl("nScreeningLockDtlNo") = 0
                dr_ScrLockDtl("vWorkspaceID") = Me.HFWorkspaceId.Value
                dr_ScrLockDtl("vSubjectId") = ds_WorkspaceSubjectMst.Tables(0).Rows(index)("vSubjectID")
                dr_ScrLockDtl("iTranNo") = 0
                dr_ScrLockDtl("nMedexScreeningHdrNo") = ds_WorkspaceSubjectMst.Tables(0).Rows(index)("nMedexScreeningHdrNo")
                dr_ScrLockDtl("iLockedBy") = Me.Session(S_UserID)
                dr_ScrLockDtl("cLockUnlockFlag") = "U"
                If Type.ToUpper.Trim() = "LOCK" Then
                    dr_ScrLockDtl("cLockUnlockFlag") = "L"
                End If
                dr_ScrLockDtl("iModifyBy") = Me.Session(S_UserID)
                dr_ScrLockDtl("cStatusindi") = "N"
                ds_ScreeningLockDtl.Tables(0).Rows.Add(dr_ScrLockDtl)
                ds_ScreeningLockDtl.AcceptChanges()

            Next

            If Not objLambda.Save_ScreeeningLockDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                   ds_ScreeningLockDtl, Me.Session(S_UserID), eStr) Then

                Throw New Exception(eStr)

            End If

            If IslockAll = False Then
                FillGridView()
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try



    End Function

#End Region

#Region "GridView event"

    Protected Sub gvSubjectQC_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvSubjectQC.RowCommand
        Dim index As Integer = CType(e.CommandArgument, Integer)
        Dim RedirectStr As String = ""
        Dim wStr As String = ""
        Dim dv_subdtl As DataView
        Dim ds_subdtl As New DataSet


        ViewState(VS_MedexScreeningHdrNo) = Convert.ToString(Me.gvSubjectQC.Rows(index).Cells(GVCell_MedexScreeningHdrNo).Text.Trim())

        If Convert.ToString(Me.gvSubjectQC.Rows(index).Cells(GVCell_MedexScreeningHdrNo).Text.Trim()) = "&nbsp;" Then
            ViewState(VS_MedexScreeningHdrNo) = ""
        End If


        If Me.ViewState(VS_MedexScreeningHdrNo) Is Nothing Or Convert.ToString(Me.ViewState(VS_MedexScreeningHdrNo)).Trim = "" Then
            ObjCommon.ShowAlert("No Screening Date have been assigned. Please assign Screening Date from Subject Assignment", Me)
            Exit Sub
        End If

        If e.CommandName.ToUpper = "DOC" Then
            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                'added by megha
                If (Not Me.Request.QueryString("workspaceid") Is Nothing) AndAlso (Not Me.Request.QueryString("type") Is Nothing) Then
                    If Not Me.Request.QueryString("schemaid") Is Nothing Then

                        If (Me.HFPath.Value.Contains("frmSubjectScreening_New.aspx")) Then
                            RedirectStr = "window.open(""" + Me.HFPath.Value + "&workspace=" & Me.HFWorkspaceId.Value.Trim() & _
                                      "&scrhdrno=" & Me.gvSubjectQC.Rows(index).Cells(GVCell_MedexScreeningHdrNo).Text.Trim() & _
                                      "&SubjectId=" & Me.gvSubjectQC.Rows(index).Cells(GVCell_SubjectId).Text.Trim() & _
                                      "&SubjectText=" & Me.gvSubjectQC.Rows(index).Cells(GVCell_Name).Text.Trim() & _
                                      "&schemaid=" & Request.QueryString("schemaid") + """)"
                        Else
                            RedirectStr = "window.open(""" + Me.HFPath.Value + "&workspace=" & Me.HFWorkspaceId.Value.Trim() & _
                                      "&scrhdrno=" & Me.gvSubjectQC.Rows(index).Cells(GVCell_MedexScreeningHdrNo).Text.Trim() & _
                                      "&SearchSubjectId=" & Me.gvSubjectQC.Rows(index).Cells(GVCell_SubjectId).Text.Trim() & _
                                      "&SearchSubjectText=" & Me.gvSubjectQC.Rows(index).Cells(GVCell_Name).Text.Trim() & _
                                      "&schemaid=" & Request.QueryString("schemaid") + """)"
                        End If

                        '===========================
                    Else
                        If (Me.HFPath.Value.Contains("frmSubjectScreening_New.aspx")) Then
                            RedirectStr = "window.open(""" + Me.HFPath.Value + "&workspace=" & Me.HFWorkspaceId.Value.Trim() & _
                             "&scrhdrno=" & Me.gvSubjectQC.Rows(index).Cells(GVCell_MedexScreeningHdrNo).Text.Trim() & _
                             "&SearchSubjectText=" & Me.gvSubjectQC.Rows(index).Cells(GVCell_Name).Text.Trim() & _
                            "&SubjectId=" & Me.gvSubjectQC.Rows(index).Cells(GVCell_SubjectId).Text.Trim() + "&type=rpt" + """)"
                        Else
                            RedirectStr = "window.open(""" + Me.HFPath.Value + "&workspace=" & Me.HFWorkspaceId.Value.Trim() & _
                             "&scrhdrno=" & Me.gvSubjectQC.Rows(index).Cells(GVCell_MedexScreeningHdrNo).Text.Trim() & _
                             "&SearchSubjectText=" & Me.gvSubjectQC.Rows(index).Cells(GVCell_Name).Text.Trim() & _
                            "&SearchSubjectId=" & Me.gvSubjectQC.Rows(index).Cells(GVCell_SubjectId).Text.Trim() + "&type=rpt" + """)"
                        End If

                    End If
                Else
                    If (Me.HFPath.Value.Contains("frmSubjectScreening_New.aspx")) Then
                        RedirectStr = "window.open(""" + Me.HFPath.Value + "&workspace=" & Me.HFWorkspaceId.Value.Trim() & _
                                 "&ScrHdrNo=" & Me.gvSubjectQC.Rows(index).Cells(GVCell_MedexScreeningHdrNo).Text.Trim() & _
                                 "&SearchSubjectText=" & Me.gvSubjectQC.Rows(index).Cells(GVCell_Name).Text.Trim() & _
                                 "&SubjectId=" & Me.gvSubjectQC.Rows(index).Cells(GVCell_SubjectId).Text.Trim() + """)"
                    Else
                        RedirectStr = "window.open(""" + Me.HFPath.Value + "&workspace=" & Me.HFWorkspaceId.Value.Trim() & _
                                 "&ScrHdrNo=" & Me.gvSubjectQC.Rows(index).Cells(GVCell_MedexScreeningHdrNo).Text.Trim() & _
                                 "&SearchSubjectText=" & Me.gvSubjectQC.Rows(index).Cells(GVCell_Name).Text.Trim() & _
                                 "&SearchSubjectId=" & Me.gvSubjectQC.Rows(index).Cells(GVCell_SubjectId).Text.Trim() + """)"
                    End If



                End If
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", RedirectStr, True)
                'If HFActivityId.Value = Str_PIF Then
                '    RedirectStr = "window.open(""" + Me.HFPath.Value + "&workspaceid=" & Me.HFWorkspaceId.Value.Trim() & _
                '                           "&SubjectId=" & Me.gvSubjectQC.Rows(index).Cells(GVCell_SubjectId).Text.Trim() + """)"


                '    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", RedirectStr, True)
                'ElseIf Me.HFActivityId.Value = Str_Screening Then
                '    RedirectStr = "window.open(""" + Me.HFPath.Value + "&workspaceid=" & Me.HFWorkspaceId.Value.Trim() & _
                '                           "&SubjectId=" & Me.gvSubjectQC.Rows(index).Cells(GVCell_SubjectId).Text.Trim() + """)"


                '    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", RedirectStr, True)

                'ElseIf Me.HFActivityId.Value = Str_LabReport Then
                '    RedirectStr = "window.open(""" + Me.HFPath.Value & "&SubjectId=" & Me.gvSubjectQC.Rows(index).Cells(GVCell_SubjectId).Text.Trim() + """)"


                '    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", RedirectStr, True)

                'End If
            End If
        End If

        If e.CommandName.ToUpper = "COMMENT" Then
            ViewState(VS_SubjectId) = Me.gvSubjectQC.Rows(index).Cells(GVCell_SubjectId).Text.Trim()


            If Not fillQCGrid() Then
                Exit Sub
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowHide", "QCDivShowHide('ST');", True)
            Return

        End If

        If e.CommandName.ToUpper = "STATUS" Then

            dv_subdtl = CType(ViewState(VS_dsWorkspaceSubjectMst), DataSet).Tables(0).DefaultView
            dv_subdtl.RowFilter = " vSubjectID = '" + Me.gvSubjectQC.Rows(index).Cells(GVCell_SubjectId).Text.Trim() + "'"
            ds_subdtl.Tables.Add(dv_subdtl.ToTable())
            ViewState(VS_dsSubDtl) = ds_subdtl
            If Me.gvSubjectQC.Rows(index).Cells(GVCell_LOCK).Text.ToUpper.Trim() = "L" Then
                If Not LockUnlockScreening("UNLOCK", False) Then
                    Me.ObjCommon.ShowAlert("Error while saving ScreeningLockDtl", Me)
                End If
            Else
                If Not LockUnlockScreening("LOCK", False) Then
                    Me.ObjCommon.ShowAlert("Error while saving ScreeningLockDtl", Me)
                End If
            End If


        End If

        If e.CommandName.ToUpper = "LABRPT" Then
            'added by megha
            If (Me.Request.QueryString("Type") = "ArchiveRPT") Then
                If (Me.Request.QueryString("LabDataFlag").ToString() = "0") Then
                    ObjCommon.ShowAlert("LabData For This Project Is Not Archived", Me.Page)
                ElseIf Me.Request.QueryString("LabDataStatus") = "Y" Then
                    ObjCommon.ShowAlert("LabData Is Archived to see its Detai unArchive It", Me.Page)
                Else
                    RedirectStr = "window.open(""" & "frmArchiveLabReportReview.aspx?&ScrDateNo=" & Me.gvSubjectQC.Rows(index).Cells(GVCell_MedexScreeningHdrNo).Text.Trim() & "&SchemaId=" & Me.Request.QueryString("SchemaId").ToString() + """)"
                End If
            ElseIf Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                If (Not Me.Request.QueryString("WorkSpaceId") Is Nothing) AndAlso (Not Me.Request.QueryString("Type") Is Nothing) Then
                    RedirectStr = "window.open(""" & "frmReportReview.aspx?mode=4" + "&ScrDateNo=" & Me.gvSubjectQC.Rows(index).Cells(GVCell_MedexScreeningHdrNo).Text.Trim() + "&Type=RPT" + """)"
                Else
                    RedirectStr = "window.open(""" & "frmReportReview.aspx?mode=4" + "&ScrDateNo=" & Me.gvSubjectQC.Rows(index).Cells(GVCell_MedexScreeningHdrNo).Text.Trim() + "&SubjectId=" & Me.gvSubjectQC.Rows(index).Cells(GVCell_SubjectId).Text.Trim() + """)"
                End If
            End If

            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", RedirectStr, True)


        End If

    End Sub

    Protected Sub gvSubjectQC_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSubjectQC.RowCreated
        If e.Row.RowType = DataControlRowType.Header Or _
                           e.Row.RowType = DataControlRowType.DataRow Or _
                           e.Row.RowType = DataControlRowType.Footer Then


            e.Row.Cells(GVCell_LOCK).Visible = False
            e.Row.Cells(GVCell_MedexScreeningHdrNo).Visible = False
            e.Row.Cells(GVCell_cRejectionflag).Visible = False
            e.Row.Cells(GVCell_Name).Visible = False

            If ViewState(str_Activityid) = QAonPIFActID Then
                e.Row.Cells(GVCell_status).Visible = False
            End If


            If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                e.Row.Cells(GVCell_status).Enabled = False
            End If

        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("id", e.Row.RowIndex)
            e.Row.Attributes.Add("onclick", "ShowColor(this);")
        End If

    End Sub

    Protected Sub gvSubjectQC_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSubjectQC.RowDataBound

        Dim ds_maxScreeningLckDtl As New DataSet
        Dim dv_maxScreeningLckDtl As New DataView
        Dim ds_commentcount As New DataSet
        Dim dv_commentcount As New DataView
        Dim CommentFlag As String = String.Empty

        Try
            ''== added by Megha for view mode and type=Report
            If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                If (Not Me.Request.QueryString("WorkSpaceId") Is Nothing) AndAlso (Not Me.Request.QueryString("Type") Is Nothing) Then
                    e.Row.Cells(GVCell_status).Visible = False
                    e.Row.Cells(GVCell_Comments).Visible = False
                End If
            End If
            ''==========================
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(GVCell_SrNo).Text = e.Row.RowIndex + 1 + (Me.gvSubjectQC.PageSize * Me.gvSubjectQC.PageIndex)
                CType(e.Row.FindControl("ImgSourceDoc"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImgSourceDoc"), ImageButton).CommandName = "DOC"
                e.Row.Cells(GVCell_SourceDoc).HorizontalAlign = HorizontalAlign.Center

                CType(e.Row.FindControl("ImgLabRpt"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImgLabRpt"), ImageButton).CommandName = "LABRPT"
                e.Row.Cells(GVCell_LabReport).HorizontalAlign = HorizontalAlign.Center

                CType(e.Row.FindControl("LnkComments"), LinkButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("LnkComments"), LinkButton).CommandName = "COMMENT"
                e.Row.Cells(GVCell_Comments).HorizontalAlign = HorizontalAlign.Center

                CType(e.Row.FindControl("ImgStatus"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("ImgStatus"), ImageButton).CommandName = "STATUS"
                e.Row.Cells(GVCell_status).HorizontalAlign = HorizontalAlign.Center

                CType(e.Row.FindControl("ImgStatus"), ImageButton).ImageUrl = "~/Images/unlock.jpg"
                CType(e.Row.FindControl("ImgStatus"), ImageButton).AlternateText = "UNLOCK"
                e.Row.Cells(GVCell_LOCK).HorizontalAlign = HorizontalAlign.Center
                e.Row.Cells(GVCell_LOCK).Text = "U"


                If Not ViewState(VS_dsMaxScreeningLockDtl) Is Nothing Then
                    ds_maxScreeningLckDtl = CType(ViewState(VS_dsMaxScreeningLockDtl), DataSet)
                    dv_maxScreeningLckDtl = ds_maxScreeningLckDtl.Tables(0).DefaultView
                    dv_maxScreeningLckDtl.RowFilter = "vSubjectID = '" + e.Row.Cells(GVCell_SubjectId).Text + "'"

                    If dv_maxScreeningLckDtl.ToTable.Rows.Count > 0 AndAlso _
                       dv_maxScreeningLckDtl.ToTable.Rows(0)("cLockUnlockFlag") = "L" AndAlso _
                       ViewState(str_Activityid).ToString <> QAonPIFActID Then


                        CType(e.Row.FindControl("ImgStatus"), ImageButton).ImageUrl = "~/Images/Lock.jpg"
                        CType(e.Row.FindControl("ImgStatus"), ImageButton).AlternateText = "LOCK"
                        CType(e.Row.FindControl("LnkComments"), LinkButton).Enabled = False

                        e.Row.Cells(GVCell_LOCK).Text = "L"

                    End If
                End If

                If Not ViewState(VS_dsComment) Is Nothing Then

                    ds_commentcount = CType(ViewState(VS_dsComment), DataSet)
                    dv_commentcount = ds_commentcount.Tables(0).DefaultView

                    If Me.Request.QueryString("Act").ToString.Trim() = "QA on PIF" Then
                        CommentFlag = "P"
                    ElseIf Me.Request.QueryString("Act").ToString.Trim() = "QA on MSR" Then
                        CommentFlag = "M"
                    End If

                    If e.Row.Cells(GVCell_MedexScreeningHdrNo).Text <> "&nbsp;" Then
                        dv_commentcount.RowFilter = "vSubjectId='" & e.Row.Cells(GVCell_SubjectId).Text & "' AND nMedexScreeningHdrNo = " & e.Row.Cells(GVCell_MedexScreeningHdrNo).Text & " And cIsSourceDocComment ='Y' and cStatusIndi <>'D' AND ( cCommentIndi ='" & CommentFlag & "' OR cCommentIndi IS NULL)"
                        If dv_commentcount.ToTable.Rows.Count > 0 Then
                            CType(e.Row.FindControl("LnkComments"), LinkButton).Text = "Comments<span style='Color:blue'>*</span>"
                        End If
                    End If
                End If



                If e.Row.Cells(GVCell_cRejectionflag).Text.ToUpper.Trim() = "Y" Then
                    e.Row.BackColor = Drawing.Color.Red
                End If

            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

#End Region

#Region "Div Section"

#Region "Div Button Events"

    Protected Sub BtnQCSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnQCSave.Click
        Dim estr As String = ""
        Dim ds_QC As New DataSet
        Dim QCMsg As String = ""
        Dim wStr As String = ""
        Try

            'CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Me.lblResponse.Text = "" And CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                Me.ObjCommon.ShowAlert("Please Click Response Button Of Particular QA Comments Against Which You Want To Response", Me.Page)

                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowHide", "QCDivShowHide('ST');", True)

                Exit Sub
            End If

            If Not AssignValues_WorkspaceSubjectComment(ds_QC, "ADD") Then
                Me.ObjCommon.ShowAlert("Error While Assigning Data.", Me.Page)
                Exit Sub
            End If

            'Added for QC Comments

            If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

                If Not Me.objLambda.Save_MedexScreeningHdrQC(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_QC, _
                    Me.Session(S_UserID), estr) Then

                    Me.ObjCommon.ShowAlert(estr, Me.Page)
                    Exit Sub

                End If

                Me.ObjCommon.ShowAlert("Record Saved Successfully", Me.Page)

            Else 'For Response

                If Not Me.objLambda.Save_MedexScreeningHdrQC(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_QC, _
                Me.Session(S_UserID), estr) Then

                    Me.ObjCommon.ShowAlert(estr, Me.Page)
                    Exit Sub

                End If

                Me.ObjCommon.ShowAlert("Response Saved Successfully", Me.Page)

            End If

            If Not fillQCGrid() Then
                Exit Sub
            End If

            If Not FillGridView() Then
                Exit Sub
            End If
            'BindGrid(Me.Request.QueryString("DCId")) 'Fill Grid

            Me.txtQCRemarks.Value = ""
            Me.lblResponse.Text = ""
            Me.RBLQCFlag.SelectedValue = "F"

            Me.txtToEmailId.Text = ""
            Me.txtCCEmailId.Text = ""

            '***********************

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

    Protected Sub BtnQCSaveSend_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim estr As String = ""
        Dim ds_QC As New DataSet
        Dim QCMsg As String = ""
        Dim fromEmailId As String = ""
        Dim toEmailId As String = ""
        Dim password As String = ""
        Dim ccEmailId As String = ""
        Dim SubjectLine As String = ""
        Dim wStr As String = ""
        'Dim ds_EmailAlert As New DataSet
        Try

            'CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Me.lblResponse.Text = "" And CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                Me.ObjCommon.ShowAlert("Please Click Response Button Of Particular QA Comments Against Which You Want To Response", Me.Page)

                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowHide", "QCDivShowHide('ST');", True)

                Exit Sub
            End If

            If Not AssignValues_WorkspaceSubjectComment(ds_QC, "ADD") Then
                Me.ObjCommon.ShowAlert("Error While Assigning Data.", Me.Page)
                Exit Sub
            End If


            'For Sending Mail
            If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

                QCMsg = "Comment on Source data(MSR,Lab Report)  given  for " + Me.ViewState(VS_SubjectName) + " (" + Me.ViewState(VS_SubjectId) + ") of Project " + HFProjectNo.Value + _
                                        "<br/><br/> Of : " + Me.RBLQCFlag.SelectedItem.Text.Trim() + _
                                        "<br/><br/> Comments: " + Me.txtQCRemarks.Value + "<br/><br/>" + _
                                        "Comments Given By : " + Me.Session(S_FirstName).ToString.Trim() + " " + Me.Session(S_LastName).ToString.Trim()

                SubjectLine = "Comments on Source data(MSR,Lab Report) of Project" + HFProjectNo.Value 'ds_EmailAlert.Tables(0).Rows(0)("vSubjectLine").ToString()

            Else

                QCMsg = "Response  on Source data(MSR,Lab Report)  given  for " + Me.ViewState(VS_SubjectName) + " (" + Me.ViewState(VS_SubjectId) + ") of Project " + HFProjectNo.Value + _
                        "<br/><br/>" + Me.lblResponse.Text.Trim() + "<br/><br/>" + _
                        "<br/><br/>Response: " + Me.txtQCRemarks.Value + "<br/><br/>" + _
                        "Response Given By : " + Me.Session(S_FirstName).ToString.Trim() + " " + Me.Session(S_LastName).ToString.Trim()

                SubjectLine = "Response on Source data(MSR,Lab Report) of Project " + HFProjectNo.Value

            End If

            fromEmailId = ConfigurationSettings.AppSettings("Username")
            password = ConfigurationSettings.AppSettings("Password")


            'wStr = "nEmailAlertId =" + Email_QCOFSCREENING.ToString() + " And cStatusIndi <> 'D'"
            'If Not Me.objHelp.GetEmailAlertMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
            '                    ds_EmailAlert, estr) Then

            '    Me.ObjCommon.ShowAlert("Error While Getting Data From EmailAlert : " + estr, Me.Page)
            '    Exit Sub

            'End If

            'If ds_EmailAlert.Tables(0).Rows.Count > 0 Then

            toEmailId = Me.txtToEmailId.Text.Trim() 'ds_EmailAlert.Tables(0).Rows(0)("vToUserEmailId").ToString()
            ccEmailId = Me.txtCCEmailId.Text.Trim() 'ds_EmailAlert.Tables(0).Rows(0)("vCCUserEmailId").ToString()

            Dim sn As New SendMail(Server, Request, Session, SubjectLine, QCMsg, fromEmailId, password, toEmailId, ccEmailId)

            'Changed on 26-Aug-2009
            If Not sn.Send(Server, Response, Session, , fromEmailId, password) Then
                Me.ObjCommon.ShowAlert("Record Saved Successfully But Email Is Not Sent Due To Some Reason", Me.Page)
                Exit Sub
            End If
            '****************************************************

            sn = Nothing


            'Added for QC Comments

            If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

                If Not Me.objLambda.Save_MedexScreeningHdrQC(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_QC, _
                    Me.Session(S_UserID), estr) Then

                    Me.ObjCommon.ShowAlert(estr, Me.Page)
                    Exit Sub

                End If

                Me.ObjCommon.ShowAlert("Record Saved Successfully", Me.Page)

            Else 'For Response

                If Not Me.objLambda.Save_MedexScreeningHdrQC(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_QC, _
                Me.Session(S_UserID), estr) Then

                    Me.ObjCommon.ShowAlert(estr, Me.Page)
                    Exit Sub

                End If

                Me.ObjCommon.ShowAlert("Response Saved Successfully", Me.Page)

            End If

            If Not fillQCGrid() Then
                Exit Sub
            End If

            'BindGrid(Me.Request.QueryString("DCId")) 'Fill Grid

            Me.txtQCRemarks.Value = ""
            Me.lblResponse.Text = ""
            Me.RBLQCFlag.SelectedValue = "F"

            Me.txtToEmailId.Text = ""
            Me.txtCCEmailId.Text = ""

            '***********************

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

#End Region

#Region "AssignValues_WorkspaceSubjectComment"

    Private Function AssignValues_WorkspaceSubjectComment(ByRef DsSave As DataSet, ByVal Type As String) As Boolean

        'Dim DtMedExInfoHdr As New DataTable
        'Dim ObjId As String = ""
        'Dim dr As DataRow
        'Dim TranNo As Integer = 0
        'Dim Wstr As String = ""
        'Dim estr As String = ""
        'Dim ds_MEdexInfroHdrQC As New DataSet
        'Dim dtMEdexInfroHdrQC As New DataTable
        'Try
        '    CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

        '    If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

        '        If Not Me.objHelp.Getme(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_MEdexInfroHdrQC, estr) Then
        '            Me.ObjCommon.ShowAlert(estr, Me.Page)
        '            Return False
        '        End If

        '        dtMEdexInfroHdrQC = ds_MEdexInfroHdrQC.Tables(0)

        '        dr = dtMEdexInfroHdrQC.NewRow
        '        dr("nSubjectMasterQCNo") = 1
        '        dr("vSubjectId") = CType(ViewState(VS_SubjectId), String)
        '        dr("iTranNo") = 1
        '        dr("vQCComment") = Me.txtQCRemarks.Value.Trim()
        '        dr("cQCFlag") = Me.RBLQCFlag.SelectedValue.Trim()
        '        dr("iModifyBy") = Me.Session(S_UserID)
        '        dr("cStatusIndi") = "N"
        '        dr("iQCGivenBy") = Me.Session(S_UserID)
        '        dr("dQCGivenOn") = System.DateTime.Today.Now.ToString("dd-MMM-yyyy hh:mm:ss")
        '        dr("cIsSourceDocComment") = "Y"
        '        dtMEdexInfroHdrQC.Rows.Add(dr)

        '    Else

        '        dtMEdexInfroHdrQC = Me.ViewState(VS_DtQC)

        '        For Each dr In dtMEdexInfroHdrQC.Rows
        '            dr("iModifyBy") = Me.Session(S_UserID)
        '            dr("cStatusIndi") = "E"
        '            dr("vResponse") = Me.txtQCRemarks.Value.Trim()
        '            dr("iResponseGivenBy") = Me.Session(S_UserID)
        '            dr("dResponseGivenOn") = System.DateTime.Today.Now.ToString("dd-MMM-yyyy hh:mm:ss")
        '            dr("cIsSourceDocComment") = "Y"
        '            dr.AcceptChanges()
        '        Next dr

        '    End If

        '    dtMEdexInfroHdrQC.AcceptChanges()

        '    dtMEdexInfroHdrQC.TableName = "SubjectMasterQC"
        '    dtMEdexInfroHdrQC.AcceptChanges()

        '    DsSave.Tables.Add(dtMEdexInfroHdrQC.Copy())
        '    DsSave.AcceptChanges()
        '    Return True



        Dim DtMedExScreeningHdr As New DataTable
        Dim ObjId As String = ""
        Dim dr As DataRow
        Dim TranNo As Integer = 0
        Dim Wstr As String = ""
        Dim estr As String = ""
        Dim ds_MEdexScreeningHdrQC As New DataSet
        Dim dtMEdexScreeningHdrQC As New DataTable
        Dim MedexScreeningHdrNo As String = String.Empty
        Dim CommentFlag As String = String.Empty
        Try


            If Me.ViewState(VS_MedexScreeningHdrNo) Is Nothing AndAlso Convert.ToString(Me.ViewState(VS_MedexScreeningHdrNo)).Trim = "" Then
                Exit Function
            End If

            If Me.Request.QueryString("Act").Trim() Is Nothing Then
                ObjCommon.ShowAlert("No Query String is Attached", Me)
                Return False
            End If

            If Me.Request.QueryString("Act").ToString.Trim() = "QA on PIF" Then
                CommentFlag = "P"
            ElseIf Me.Request.QueryString("Act").ToString.Trim() = "QA on MSR" Then
                CommentFlag = "M"
            End If


            MedexScreeningHdrNo = CType(Me.ViewState(VS_MedexScreeningHdrNo), String)

            If Type.ToString.ToUpper = "ADD" Then
                'DtMedExScreeningHdr = CType(Me.ViewState(VS_dtMedEx_Fill), DataTable)


                If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

                    If Not Me.objHelp.GetMedexScreeningHdrQC(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_MEdexScreeningHdrQC, estr) Then
                        Me.ObjCommon.ShowAlert(estr, Me.Page)
                        Return False
                    End If

                    dtMEdexScreeningHdrQC = ds_MEdexScreeningHdrQC.Tables(0)
                    dr = dtMEdexScreeningHdrQC.NewRow
                    dr("nMedExScreeningHdrQCNo") = 1
                    dr("nMedExScreeningHdrNo") = MedexScreeningHdrNo
                    dr("iTranNo") = 1
                    dr("vQCComment") = Me.txtQCRemarks.Value.Trim()
                    dr("cQCFlag") = Me.RBLQCFlag.SelectedValue.Trim()
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "N"
                    dr("iQCGivenBy") = Me.Session(S_UserID)
                    dr("dQCGivenOn") = System.DateTime.Today.Now.ToString("dd-MMM-yyyy hh:mm tt")
                    dr("cIsSourceDocComment") = "Y"
                    dr("cCommentIndi") = CommentFlag
                    dtMEdexScreeningHdrQC.Rows.Add(dr)

                Else 'For Response

                    dtMEdexScreeningHdrQC = Me.ViewState(VS_DtQC)

                    For Each dr In dtMEdexScreeningHdrQC.Rows
                        dr("iModifyBy") = Me.Session(S_UserID)
                        dr("cStatusIndi") = "E"
                        dr("vResponse") = Me.txtQCRemarks.Value.Trim()
                        dr("iResponseGivenBy") = Me.Session(S_UserID)
                        dr("dResponseGivenOn") = System.DateTime.Today.Now.ToString("dd-MMM-yyyy hh:mm tt")


                        dr.AcceptChanges()
                    Next dr

                End If




            ElseIf Type.ToString.ToUpper = "DELETE" Then

                dtMEdexScreeningHdrQC = Me.ViewState(VS_DtQC)

                For Each dr In dtMEdexScreeningHdrQC.Rows
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "D"
                    dr.AcceptChanges()
                Next dr

            End If


            dtMEdexScreeningHdrQC.AcceptChanges()

            dtMEdexScreeningHdrQC.TableName = "MedExScreeningHdrQC"
            dtMEdexScreeningHdrQC.AcceptChanges()

            DsSave.Tables.Add(dtMEdexScreeningHdrQC.Copy())
            DsSave.AcceptChanges()
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function

#End Region

#Region "fillQCGrid"

    'Private Function fillQCGrid() As Boolean
    '    Dim Ds_QCGrid As New DataSet
    '    Dim Wstr As String = ""
    '    Dim estr As String = ""
    '    Try

    '        ViewState(VS_WSSDDetailId) = "0001"

    '        'CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
    '        Wstr = "vWorkSpaceSubjectDocDetailId='" & Me.ViewState(VS_WSSDDetailId) & "'"

    '        If Not Me.objHelp.GetViewWorkSpaceSubjectComment(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '                                    Ds_QCGrid, estr) Then
    '            Me.ShowErrorMessage(estr, estr)
    '            Return False
    '        End If

    '        Me.GVQCDtl.DataSource = Ds_QCGrid
    '        Me.GVQCDtl.DataBind()

    '        'Me.BtnSave.Visible = True

    '        If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View And _
    '            Ds_QCGrid.Tables(0).Rows.Count <= 0 Then

    '            'Me.BtnSave.Visible = False
    '            Exit Function

    '        End If

    '        Return True

    '    Catch ex As Exception
    '        Me.ShowErrorMessage(ex.Message, "")
    '        Return False

    '    End Try

    'End Function

    Private Function fillQCGrid() As Boolean
        Dim Ds_QCGrid As New DataSet
        Dim Wstr As String = ""
        Dim estr As String = ""
        Dim Str_SubjectId As String = CType(ViewState(VS_SubjectId), String)
        Dim MedexScreeningHdrNo As String = String.Empty
        Dim CommentFlag As String = String.Empty

        Dim ds As New DataSet
        Dim locationcode As String = Session(S_LocationCode)

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Me.ViewState(VS_MedexScreeningHdrNo) Is Nothing Or Convert.ToString(Me.ViewState(VS_MedexScreeningHdrNo)).Trim = "" Then
                ObjCommon.ShowAlert("No Screening Date have been assigned. Please assign Screening Date from Subject Assignment", Me)
                Exit Function
            End If

            MedexScreeningHdrNo = CType(Me.ViewState(VS_MedexScreeningHdrNo), String)

            If Me.Request.QueryString("Act").ToString.Trim() = "QA on PIF" Then
                CommentFlag = "P"
            ElseIf Me.Request.QueryString("Act").ToString.Trim() = "QA on MSR" Then
                CommentFlag = "M"
            End If

            Wstr = "vSubjectId='" & Str_SubjectId & "' AND nMedexScreeningHdrNo = " & MedexScreeningHdrNo & " And cIsSourceDocComment ='Y' And cStatusIndi <>'D' And ( cCommentIndi = '" & CommentFlag & "' OR cCommentIndi IS NULL )"
            If Not Me.objHelp.View_MedexScreeningHdrQc(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        Ds_QCGrid, estr) Then
                Me.ShowErrorMessage(estr, estr)
                Return False
            End If


            If Not Ds_QCGrid Is Nothing Then

                If Ds_QCGrid.Tables(0).Rows.Count > 0 Then
                    Ds_QCGrid.Tables(0).Columns.Add(("ActualTIME"), Type.GetType("System.String"))
                    Ds_QCGrid.Tables(0).Columns.Add(("ActualTIME2"), Type.GetType("System.String"))
                    For Each dr_Qc In Ds_QCGrid.Tables(0).Rows

                        If Not dr_Qc("dQCGivenOn").ToString() = "" Then
                            If Not objHelp.Proc_ActualAuditTrailTime(CDate(dr_Qc("dQCGivenOn")).ToString("dd-MMM-yyyy HH:mm") + "##" + locationcode, ds, estr) Then '' Added by Dipen shah on 3-dec-2014
                                Throw New Exception(estr)
                            End If

                            If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                                dr_Qc("ActualTIME") = CDate(ds.Tables(0).Rows(0).Item("ActualTIME")).ToString("dd-MMM-yyyy HH:mm") + " (" + ds.Tables(0).Rows(0).Item("TimeZoneOffSet").ToString + " GMT)"
                            End If
                        End If

                        If Not dr_Qc("dResponseGivenOn").ToString() = "" Then
                            If Not objHelp.Proc_ActualAuditTrailTime(CDate(dr_Qc("dResponseGivenOn")).ToString("dd-MMM-yyyy HH:mm") + "##" + locationcode, ds, estr) Then '' Added by Dipen shah on 3-dec-2014
                                Throw New Exception(estr)
                            End If
                            If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                                dr_Qc("ActualTIME2") = CDate(ds.Tables(0).Rows(0).Item("ActualTIME")).ToString("dd-MMM-yyyy HH:mm") + " (" + ds.Tables(0).Rows(0).Item("TimeZoneOffSet").ToString + " GMT)"
                            End If
                        End If



                        'If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                        '    dr_Qc("ActualTIME") = CDate(ds.Tables(0).Rows(0).Item("ActualTIME")).ToString("dd-MMM-yyyy HH:mm") + " (" + ds.Tables(0).Rows(0).Item("TimeZoneOffSet").ToString + " GMT)"
                        '    dr_Qc("ActualTIME2") = CDate(ds.Tables(0).Rows(0).Item("ActualTIME")).ToString("dd-MMM-yyyy HH:mm") + " (" + ds.Tables(0).Rows(0).Item("TimeZoneOffSet").ToString + " GMT)"
                        'End If

                    Next
                End If
            End If


            Me.GVQCDtl.DataSource = Ds_QCGrid
            Me.GVQCDtl.DataBind()



            'If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View And _
            '    Ds_QCGrid.Tables(0).Rows.Count <= 0 Then

            'End If

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False

        End Try

    End Function

#End Region

#Region "GVQCDtl Grid Events"

    Protected Sub GVQCDtl_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVQCDtl.RowCreated
        e.Row.Cells(GVCQC_SubjectMasterQCNo).Visible = False
        'e.Row.Cells(GVCQC_vWorkSpaceSubjectDocDetailId).Visible = False
        e.Row.Cells(GVCQC_QCFlag).Visible = False

        e.Row.Cells(GVCQC_LnkResponse).Visible = True
        e.Row.Cells(GVCQC_Delete).Visible = False

        If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

            e.Row.Cells(GVCQC_LnkResponse).Visible = False
            e.Row.Cells(GVCQC_Delete).Visible = True

        End If



    End Sub

    Protected Sub GVQCDtl_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVQCDtl.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            e.Row.Cells(GVCQC_SrNo).Text = e.Row.RowIndex + 1 + (Me.GVQCDtl.PageSize * Me.GVQCDtl.PageIndex)
            CType(e.Row.FindControl("lnkResponse"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkResponse"), LinkButton).CommandName = "Response"
            CType(e.Row.FindControl("lnkResponse"), LinkButton).OnClientClick = "return QCDivShowHide('ST');"

            CType(e.Row.FindControl("ImgDelete"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("ImgDelete"), ImageButton).CommandName = "Delete"
            CType(e.Row.FindControl("ImgDelete"), ImageButton).Visible = False

            If Convert.ToString(e.Row.Cells(GVCQC_ResponseGivenBy).Text) = "&nbsp;" Then
                CType(e.Row.FindControl("ImgDelete"), ImageButton).Visible = True
            End If

            If e.Row.Cells(GVCQC_ResponseGivenBy).Text <> "&nbsp;" AndAlso Not e.Row.Cells(GVCQC_ResponseGivenBy).Text = "" Then
                CType(e.Row.FindControl("lnkResponse"), LinkButton).Enabled = False
            ElseIf e.Row.Cells(GVCQC_ResponseGivenBy).Text = "" Then
                CType(e.Row.FindControl("lnkResponse"), LinkButton).Enabled = True
            End If

        End If



    End Sub

    Protected Sub GVQCDtl_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GVQCDtl.RowCommand
        Dim index As Integer = CType(e.CommandArgument, Integer)
        Dim Wstr As String = ""
        Dim estr As String = ""
        Dim ds_MEdexInfroHdrQC As New DataSet
        Dim ds_QC As New DataSet

        If e.CommandName.ToUpper.Trim() = "RESPONSE" Then


            Me.lblResponse.Text = "Response To: " + Me.GVQCDtl.Rows(index).Cells(GVCQC_QCComment).Text.Trim()

            Wstr = "nMedexScreeningHdrQcNo=" + Me.GVQCDtl.Rows(index).Cells(GVCQC_SubjectMasterQCNo).Text.Trim()

            If Not Me.objHelp.GetMedexScreeningHdrQC(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    ds_MEdexInfroHdrQC, estr) Then

                Me.ObjCommon.ShowAlert(estr, Me.Page)
                Exit Sub

            End If

            Me.ViewState(VS_DtQC) = ds_MEdexInfroHdrQC.Tables(0)

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowHide", "QCDivShowHide('ST');", True)

        ElseIf e.CommandName.ToUpper.Trim() = "DELETE" Then
            Wstr = "nMedexScreeningHdrQcNo=" + Me.GVQCDtl.Rows(index).Cells(GVCQC_SubjectMasterQCNo).Text.Trim()

            If Not Me.objHelp.GetMedexScreeningHdrQC(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    ds_MEdexInfroHdrQC, estr) Then

                Me.ObjCommon.ShowAlert(estr, Me.Page)
                Exit Sub

            End If

            Me.ViewState(VS_DtQC) = ds_MEdexInfroHdrQC.Tables(0)

            If Not AssignValues_WorkspaceSubjectComment(ds_QC, "DELETE") Then
                Me.ObjCommon.ShowAlert("Error While Assigning Data.", Me.Page)
                Exit Sub
            End If

            If Not Me.objLambda.Save_MedexScreeningHdrQC(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_QC, _
                Me.Session(S_UserID), estr) Then

                Me.ObjCommon.ShowAlert(estr, Me.Page)
                Exit Sub

            End If

            ObjCommon.ShowAlert("Comment deleted sucessfully", Me)

            If Not fillQCGrid() Then
                Exit Sub
            End If

            If Not FillGridView() Then
                Exit Sub
            End If
        End If

    End Sub

#End Region

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

    Protected Sub GVQCDtl_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GVQCDtl.RowDeleting

    End Sub
End Class
