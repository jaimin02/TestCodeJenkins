Imports System.Collections.Generic
Imports System.Windows.Forms

Partial Class frmWorkspaceSubjectMedExInfo
    Inherits System.Web.UI.Page

#Region "Variable Declaration "

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()


    Private Const VS_SAVE As String = "Save"
    Private Const VS_DtWorkSpaceSubjectMst As String = "DtWorkSpaceSubjectMst"
    Private Const VS_DtMedExInfoHdr As String = "DtMedExInfoHdr"
    Private Const VS_DtMedExInfoDtl As String = "DtMedExInfoDtl"
    Private Const VS_DtMedexinfoHdrQC As String = "Dt_MedexinfoHdrQC"
    Private Const VS_DtCRFHdrDtlSubDtl As String = "Dt_CRFHdrDtlSubDtl"

    Private Const GVC_ChkSave As Integer = 0
    Private Const GVC_SubjectId As Integer = 1
    Private Const GVC_SubjectNo As Integer = 2
    Private Const GVC_SubjectInitial As Integer = 3
    Private Const GVC_RejectionFlag As Integer = 4
    Private Const GVC_iSubjectId As Integer = 5

#End Region

#Region "Page Load "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                GenCall()
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "getData", "getData();", True)
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "GenCall "

    Private Function GenCall() As Boolean
        Dim ds As New DataSet
        Dim wstr1 As String = String.Empty
        Dim ds_MedexinfoHdrQC As New DataSet
        Dim eStr_Retu As String = String.Empty
        Try

            Me.HFWorkspaceId.Value = Me.Request.QueryString("WorkSpaceId").ToString
            Me.HFActivityId.Value = Me.Request.QueryString("ActivityId").ToString
            Me.HFNodeId.Value = Me.Request.QueryString("NodeId").ToString
            Me.HFPeriodId.Value = Me.Request.QueryString("PeriodId").ToString


            'Added by Chandres Vanker on 17-Sep-2009 For CTM Flow
            If Not IsNothing(Me.Request.QueryString("SubjectId")) AndAlso _
                        Me.Request.QueryString("SubjectId").ToString.Trim() <> "" Then
                Me.HFSubjectId.Value = Me.Request.QueryString("SubjectId").ToString()
            End If
            '*******************

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If

            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall")

        Finally

        End Try

    End Function

#End Region

#Region "GenCall_Data "

    Private Function GenCall_Data(ByRef ds_Retu As DataSet) As Boolean

        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Val As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing
        Dim WorkspaceId As String = String.Empty
        Dim ActivityId As String = String.Empty
        Dim NodeId As String = String.Empty
        Try



            WorkspaceId = Me.HFWorkspaceId.Value.Trim()
            ActivityId = Me.HFActivityId.Value.Trim()
            NodeId = Me.HFNodeId.Value.Trim()

            wStr = "vWorkSpaceId='" + WorkspaceId + "'" ' and cRegectionFlag<>'Y'"

            If Not objHelp.GetMedExInfoHdr("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If

            If ds Is Nothing Then
                Throw New Exception(eStr_Retu)
            End If

            Me.ViewState(VS_DtMedExInfoHdr) = ds.Tables("MedExInfoHdr")   ' adding blank DataTable in viewstate

            ds = Nothing
            ds = New DataSet
            If Not objHelp.GetMedExInfoDtl("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If

            If ds Is Nothing Then
                Throw New Exception(eStr_Retu)
            End If

            Me.ViewState(VS_DtMedExInfoDtl) = ds.Tables("MedExInfoDtl")   ' adding blank DataTable in viewstate


            ds_Retu = ds
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_Data")

        Finally

        End Try
    End Function

#End Region

#Region "GenCall_showUI "

    Private Function GenCall_ShowUI() As Boolean
        Dim dt_OpMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim dt_heading As New DataTable
        Dim ds_Heading As New DataSet
        Dim estr As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim ds_CRFHdrDtlSubDtl As New DataSet
        Try

            'added by vishal for legends 

            '  Me.imgShow.Attributes.Add("onmouseover", "$('#" + Me.canal.ClientID + "').toggle('medium');")
            ' Me.canal.Attributes.Add("onmouseleave", "$('#" + Me.canal.ClientID + "').toggle('medium');")


            Page.Title = ":: Project Subject Master  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            Wstr = "vWorkspaceId='" & Me.Request.QueryString("WorkSpaceId").ToString.Trim() & "' and vActivityId='" & _
                    Me.HFActivityId.Value.Trim() & "' And iNodeId=" & Me.HFNodeId.Value.Trim()

            If Not Me.objHelp.GetViewWorkSpaceNodeDetail(Wstr, ds_Heading, estr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Header Information.", Page)
                Exit Function
            End If
            dt_heading = ds_Heading.Tables(0)

            CType(Me.Master.FindControl("lblHeading"), System.Web.UI.WebControls.Label).Text = "Project: " + dt_heading.Rows(0).Item("vWorkSpaceDesc").ToString.Trim() + _
                                                                    "</br>Activity: " + dt_heading.Rows(0).Item("vNodeDisplayName").ToString.Trim() + _
                                                                    "</br>Period: " + Me.HFPeriodId.Value.Trim() '"WorkSpaceSubject Master"

            dt_OpMst = Me.ViewState(VS_DtWorkSpaceSubjectMst)

            Choice = Me.ViewState("Choice")

            Wstr = "vWorkSpaceId = '" + Me.HFWorkspaceId.Value.Trim() + "'"
            Wstr += " And iPeriod = " + Me.HFPeriodId.Value.Trim() + ""
            Wstr += " And iNodeId = " + Me.HFNodeId.Value.Trim() + ""
            Wstr += " And vActivityId = '" + Me.HFActivityId.Value.Trim() + "'"
            Wstr += " And cStatusIndi <> 'D' "
            'And cActiveFlag <> 'N'"

            If Not Me.objHelp.GetData("View_CrfHdrDtlDataStatus", "*", Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CRFHdrDtlSubDtl, estr) Then
                ObjCommon.ShowAlert("Error While Getting Data From MedExInfoHdr", Me.Page)
                Exit Function
            End If

            If Not ds_CRFHdrDtlSubDtl.Tables(0) Is Nothing Then
                Me.ViewState(VS_DtCRFHdrDtlSubDtl) = ds_CRFHdrDtlSubDtl.Tables(0)
            End If

            If Not FillGrid() Then
                Exit Function
            End If

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....GenCall_ShowUI")
        Finally
        End Try
    End Function

#End Region

#Region "FillGrid"

    Private Function FillGrid() As Boolean
        Dim ds_View As New Data.DataSet
        Dim estr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim WorkspaceId As String = String.Empty
        Dim ActivityId As String = String.Empty
        Dim NodeId As String = String.Empty
        Try



            WorkspaceId = Me.HFWorkspaceId.Value.Trim()
            ActivityId = Me.HFActivityId.Value.Trim()
            NodeId = Me.HFNodeId.Value.Trim()

            'Commented as per their requirement on 31-Oct-2009 and done on 2-Nov-2009
            'wStr = "vWorkSpaceId='" + WorkspaceId + "' and cRejectionFlag<>'Y' and iPeriod=" + Me.HFPeriodId.Value.Trim()
            wStr = "vWorkSpaceId='" + WorkspaceId + "' and iPeriod=" + Me.HFPeriodId.Value.Trim()
            '************************************

            'Added on 17-Sep-2009 by Chandresh Vanker for CTM Flow
            If Not IsNothing(Me.HFSubjectId.Value) AndAlso Me.HFSubjectId.Value.Trim() <> "" Then
                wStr += " And vSubjectId = '" + Me.HFSubjectId.Value.Trim() + "'"
            End If
            '***************************

            'Added on 23-Sep-2009 
            wStr += " and cStatusindi <>'D' order by iMySubjectNo,dReportingDate"
            '*********************

            If Not objHelp.GetWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                            ds_View, estr) Then

                Return False

            End If

            Me.gvWorkSpaceSubjectMst.DataSource = ds_View
            Me.gvWorkSpaceSubjectMst.DataBind()



            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....FillGrid")
            Return False
        End Try
    End Function

#End Region

#Region "Grid Event"

    Protected Sub gvWorkSpaceSubjectMst_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvWorkSpaceSubjectMst.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or _
        e.Row.RowType = DataControlRowType.Header Or _
        e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVC_RejectionFlag).Visible = False
            e.Row.Cells(GVC_iSubjectId).Visible = False
        End If
    End Sub

    Protected Sub gvWorkSpaceSubjectMst_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvWorkSpaceSubjectMst.RowDataBound
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim CrfDataStatus As String = String.Empty

        Dim SubjectId As String
        Dim RedirectStr As String
        Dim WorkspaceId As String = String.Empty
        Dim ActivityId As String = String.Empty
        Dim NodeId As String = String.Empty
        Dim PeriodId As String = String.Empty
        Dim MySubjectNo As String = String.Empty
        Dim DsSave As New DataSet

        Dim TranNo_Retu As String = String.Empty
        Dim ds As New DataSet
        Dim Param As String = String.Empty
        Dim dt_CRFHdrDtlSubDtl As New DataTable
        Dim dv_CRFHdrDtlSubDtl As DataView
        Dim cell As New TableCell
        Dim stylecell As New DataGridViewCellStyle

        Try

            If e.Row.RowType = DataControlRowType.DataRow Then

                SubjectId = e.Row.Cells(GVC_SubjectId).Text.Trim()
                WorkspaceId = Me.HFWorkspaceId.Value.Trim()
                ActivityId = Me.HFActivityId.Value.Trim()
                NodeId = Me.HFNodeId.Value.Trim()
                PeriodId = Me.HFPeriodId.Value.Trim()
                MySubjectNo = e.Row.Cells(GVC_iSubjectId).Text.Trim()

                CType(e.Row.FindControl("lnkSave"), ImageButton).CommandName = "SAVE"
                CType(e.Row.FindControl("lnkSave"), ImageButton).CommandArgument = e.Row.RowIndex

                'CType(e.Row.FindControl("lnkEdit"), LinkButton).Text = "Edit"
                'CType(e.Row.FindControl("lnkEdit"), LinkButton).CommandName = "EDIT"
                'CType(e.Row.FindControl("lnkEdit"), LinkButton).CommandArgument = e.Row.RowIndex

                dt_CRFHdrDtlSubDtl = CType(Me.ViewState(VS_DtCRFHdrDtlSubDtl), DataTable)
                dv_CRFHdrDtlSubDtl = dt_CRFHdrDtlSubDtl.DefaultView
                dv_CRFHdrDtlSubDtl.RowFilter = "vSubjectId = '" + SubjectId + "'"
                dt_CRFHdrDtlSubDtl = dv_CRFHdrDtlSubDtl.ToTable()

                Param = "E"
                If dt_CRFHdrDtlSubDtl.Rows.Count > 0 Then
                    CrfDataStatus = dt_CRFHdrDtlSubDtl.Rows(0)("cCRFDtlDataStatus").ToString
                    If CrfDataStatus = CRF_DataEntry Or CrfDataStatus = CRF_DataEntryCompleted Then
                        e.Row.BackColor = Drawing.Color.Orange
                        e.Row.Cells(3).Attributes.Add("class", "clrwhite")
                        e.Row.Cells(2).Attributes.Add("class", "clrwhite")
                        e.Row.Cells(1).Attributes.Add("class", "clrwhite")
                    Else
                        e.Row.BackColor = Drawing.Color.Blue
                        'e.Row.ForeColor = Drawing.Color.White
                        e.Row.Cells(3).Attributes.Add("class", "clrwhite")
                        e.Row.Cells(2).Attributes.Add("class", "clrwhite")
                        e.Row.Cells(1).Attributes.Add("class", "clrwhite")
                    End If
                    'CType(e.Row.FindControl("lnkEdit"), LinkButton).OnClientClick = "return confirm('Data Allready Entered For Subject: " + _
                    '                                            e.Row.Cells(GVC_SubjectId).Text.Trim() + "[" + e.Row.Cells(GVC_SubjectNo).Text.Trim() + "]" + _
                    '                                            ". Are You Sure You Want To Edit It Again?');"

                    Param = "E"
                    e.Row.ForeColor = Drawing.Color.White
                    CType(e.Row.FindControl("lnkEdit"), ImageButton).ForeColor = Drawing.Color.White
                End If

                If e.Row.Cells(GVC_RejectionFlag).Text.ToUpper.Trim() = "Y" Then

                    e.Row.BackColor = Drawing.Color.Red
                    'CType(e.Row.FindControl("lnkEdit"), LinkButton).OnClientClick = "return confirm('Subject: " + e.Row.Cells(GVC_SubjectId).Text.Trim() + "[" + _
                    '                        e.Row.Cells(GVC_SubjectNo).Text.Trim() + "] Is Rejected from Project. " + _
                    '                        ". Are You Sure You Want To View Or Edit It Again?');"
                    Param = "R"
                    e.Row.ForeColor = Drawing.Color.White
                    e.Row.Cells(3).Attributes.Add("class", "clrwhite")
                    e.Row.Cells(2).Attributes.Add("class", "clrwhite")
                    e.Row.Cells(1).Attributes.Add("class", "clrwhite")
                    CType(e.Row.FindControl("lnkEdit"), ImageButton).ForeColor = Drawing.Color.White
                End If

                RedirectStr = "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + WorkspaceId.Trim() + _
                             "&ActivityId=" + ActivityId + _
                             "&NodeId=" + NodeId + "&PeriodId=" + PeriodId + "&SubjectId=" + SubjectId + "&Type=BA-BE" + _
                              "&ScreenNo=" + SubjectId + "&MySubjectNo=" + MySubjectNo + "&SubNo="

                CType(e.Row.FindControl("lnkEdit"), ImageButton).OnClientClick = "return OpenWindow('" + RedirectStr + "','" + Param + "','" + SubjectId + "');"

                'As per the requirement of CPMA on 07-Oct-2009
                If e.Row.Cells(GVC_RejectionFlag).Text.ToUpper.Trim() = "Y" And _
                    e.Row.Cells(GVC_SubjectNo).Text.Trim() = "0" And dt_CRFHdrDtlSubDtl.Rows.Count <= 0 Then

                    e.Row.Visible = False

                End If
                '*******************


            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
        End Try
    End Sub

    Protected Sub gvWorkSpaceSubjectMst_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvWorkSpaceSubjectMst.RowCommand
        Dim INDEX As Integer = CType(e.CommandArgument, Integer)
        Dim SubjectId As String
        Dim RedirectStr As String
        Dim WorkspaceId As String = String.Empty
        Dim ActivityId As String = String.Empty
        Dim NodeId As String = String.Empty
        Dim PeriodId As String = String.Empty
        Dim MySubjectNo As String = String.Empty
        Dim vMySubjectNo As String = String.Empty
        Dim DsSave As New DataSet
        Dim estr As String = String.Empty
        Dim TranNo_Retu As String = String.Empty
        Try

            SubjectId = IIf(Me.gvWorkSpaceSubjectMst.Rows(INDEX).Cells(GVC_SubjectId).Text = "", "0", Me.gvWorkSpaceSubjectMst.Rows(INDEX).Cells(GVC_SubjectId).Text)
            WorkspaceId = Me.HFWorkspaceId.Value.Trim()
            ActivityId = Me.HFActivityId.Value.Trim()
            NodeId = Me.HFNodeId.Value.Trim()
            PeriodId = Me.HFPeriodId.Value.Trim()
            MySubjectNo = Me.gvWorkSpaceSubjectMst.Rows(INDEX).Cells(GVC_iSubjectId).Text
            vMySubjectNo = Me.gvWorkSpaceSubjectMst.Rows(INDEX).Cells(GVC_SubjectNo).Text

            If e.CommandName.ToUpper = "SAVE" Then

                AssignValues(SubjectId, WorkspaceId, ActivityId, NodeId, MySubjectNo, DsSave)
                If Not Me.objLambda.Save_MedExInfoHdrDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, DsSave, True, Me.Session(S_UserID), TranNo_Retu, estr) Then
                    Me.ObjCommon.ShowAlert("Error While Saving Default Values.", Me)
                    Exit Sub
                End If

                Me.ObjCommon.ShowAlert("Default Values Are Saved.", Me)

                'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", "RefreshPage();", True)
                Me.FillGrid()

            ElseIf e.CommandName.ToUpper = "EDIT" Then

                'SubjectId = Me.gvWorkSpaceSubjectMst.Rows(INDEX).Cells(GVC_SubjectId).Text

                'RedirectStr = "window.open(""" + "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + WorkspaceId.Trim() + _
                '              "&ActivityId=" + ActivityId + _
                '              "&NodeId=" + NodeId + "&PeriodId=" + PeriodId + "&SubjectId=" + SubjectId + "&Type=BA-BE" + _
                '               "&ScreenNo=" + SubjectId + "&MySubjectNo=" + MySubjectNo + """)"

                'CType(gvWorkSpaceSubjectMst.FindControl("lnkEdit"), LinkButton).OnClientClick = "return OpenWindow('" + RedirectStr + "');"

                'RedirectStr = "window.open(""" + "frmCTMMedExInfoHdrDtl.aspx?WorkSpaceId=" + WorkspaceId.Trim() + _
                '                "&ActivityId=" + ActivityId + _
                '                "&NodeId=" + NodeId + "&PeriodId=" + PeriodId + "&SubjectId=" + SubjectId + "&Type=BA-BE" + _
                '                 "&ScreenNo=" + SubjectId + "&MySubjectNo=" + MySubjectNo + """)"


                'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", RedirectStr, True)


            ElseIf e.CommandName.ToUpper = "VIEW" Then

                SubjectId = Me.gvWorkSpaceSubjectMst.Rows(INDEX).Cells(GVC_SubjectId).Text

                RedirectStr = "window.open(""" + "frmMedExInfoHdrDtl.aspx?Mode=4&WorkSpaceId=" + WorkspaceId.Trim() + _
                                "&ActivityId=" + ActivityId + _
                                "&NodeId=" + NodeId + "&PeriodId=" + PeriodId + "&SubjectId=" + SubjectId + _
                                "&MySubjectNo=" + MySubjectNo + """)"
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", RedirectStr, True)


            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......gvWorkSpaceSubjectMst_RowCommand")
        End Try
    End Sub

    Protected Sub gvWorkSpaceSubjectMst_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvWorkSpaceSubjectMst.RowEditing
        e.Cancel = True
    End Sub

    'Protected Sub gvWorkSpaceSubjectMst_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvWorkSpaceSubjectMst.PageIndexChanging

    '    gvWorkSpaceSubjectMst.PageIndex = e.NewPageIndex

    '    If Not FillGrid() Then
    '        Exit Sub
    '    End If
    'End Sub

#End Region

#Region "AssignValues"

    Private Sub AssignValues(ByVal SubjectId As String, ByVal WorkspaceId As String, _
                                ByVal ActivityId As String, ByVal NodeId As String, _
                                ByVal MySubjectNo As String, ByRef DsSave As DataSet)

        Dim DtMedExInfoHdr As New DataTable
        Dim DtMedExInfoDtl As New DataTable
        'Dim DsSave As New DataSet
        Dim dr As DataRow
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim dsNodeInfo As New DataSet
        Dim DsMedex As New DataSet
        Dim cntDSMedex As Integer
        Try
            Wstr = "vWorkSpaceId='" & WorkspaceId & "' and iNodeId='" & NodeId & "'" & _
                    " and vActivityId='" & ActivityId & "'"
            If Not Me.objHelp.getWorkSpaceNodeDetail(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                            dsNodeInfo, estr) Then
                Me.ObjCommon.ShowAlert("Error while getting NodeIndex", Me)
                Exit Sub
            End If

            DtMedExInfoHdr = CType(Me.ViewState(VS_DtMedExInfoHdr), System.Data.DataTable)
            DtMedExInfoDtl = CType(Me.ViewState(VS_DtMedExInfoDtl), System.Data.DataTable)

            DtMedExInfoHdr.Clear()
            DtMedExInfoDtl.Clear()
            dr = DtMedExInfoHdr.NewRow
            'nMedExInfoHdrNo, vWorkSpaceId, dStartDate, vPeriod, iModifyBy, dModifyOn
            dr("nMedExInfoHdrNo") = 1
            dr("vWorkSpaceId") = WorkspaceId
            dr("dStartDate") = System.DateTime.Now
            dr("iPeriod") = Me.Request.QueryString("PeriodId").Trim()
            dr("iModifyBy") = Me.Session(S_UserID)
            DtMedExInfoHdr.Rows.Add(dr)
            DtMedExInfoHdr.TableName = "MedExInfoHdr"
            DtMedExInfoHdr.AcceptChanges()


            'To Get Default Values
            If Not Me.objHelp.GetViewMedExWorkSpaceDtl("vActivityId = '" & ActivityId & "' AND vWorkSpaceId = '" & _
                                                        WorkspaceId & "' And vMedExType <> 'IMPORT'", _
                                                        WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                        DsMedex, estr) Then

                Me.ObjCommon.ShowAlert("", Me.Page)
                Exit Sub
            End If

            If DsMedex.Tables(0).Rows.Count <= 0 Then
                Me.ObjCommon.ShowAlert("No Medex is attached with this activity.", Me.Page)
                Exit Sub
            End If

            'nMedexInfoDtlNo, nMedexInfoHdrNo, iNodeId, iNodeIndex, vActivityId, iTranNo, vSubjectId, 
            'iMySubjectNo, vMedexCode, vMedexValue, dModifyon, iModifyBy, cStatusIndi
            For cntDSMedex = 0 To DsMedex.Tables(0).Rows.Count - 1
                dr = DtMedExInfoDtl.NewRow

                dr("nMedexInfoDtlNo") = 1
                dr("nMedExInfoHdrNo") = 1
                dr("iNodeId") = NodeId
                dr("iNodeIndex") = dsNodeInfo.Tables(0).Rows(0).Item("iNodeIndex") '1
                dr("vActivityId") = ActivityId
                dr("iTranNo") = 0
                dr("vSubjectId") = SubjectId
                dr("iMySubjectNo") = MySubjectNo
                dr("vMedExCode") = DsMedex.Tables(0).Rows(cntDSMedex)("vMedexCode")

                'Changed as Suggested By Sir on 6-Feb-2009
                If DsMedex.Tables(0).Rows(cntDSMedex)("vMedExType").ToString.ToUpper.Trim() = "TIME" Or DsMedex.Tables(0).Rows(cntDSMedex)("vMedExType").ToString.ToUpper.Trim() = "ASYNCTIME" Then
                    dr("vMedexValue") = DateTime.Now.ToString("HH:mm:ss")
                ElseIf DsMedex.Tables(0).Rows(cntDSMedex)("vMedExType").ToString.ToUpper.Trim() = "DATETIME" Or DsMedex.Tables(0).Rows(cntDSMedex)("vMedExType").ToString.ToUpper.Trim() = "frmsubDATETIME" Then
                    dr("vMedexValue") = DateTime.Now.Date.ToString("dd-MMM-yyyy")
                Else
                    dr("vMedexValue") = DsMedex.Tables(0).Rows(cntDSMedex)("vDefaultValue")
                End If
                '**************************************

                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"

                DtMedExInfoDtl.Rows.Add(dr)
            Next

            DtMedExInfoDtl.TableName = "MedExInfoDtl"
            DtMedExInfoDtl.AcceptChanges()

            DsSave.Tables.Add(DtMedExInfoHdr.Copy())
            DsSave.Tables.Add(DtMedExInfoDtl.Copy())
            DsSave.AcceptChanges()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...AssignValues")
        End Try

    End Sub

#End Region

#Region "Butten Event"

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim INDEX As Integer
        Dim SubjectId As String
        Dim WorkspaceId As String = String.Empty
        Dim ActivityId As String = String.Empty
        Dim NodeId As String = String.Empty
        Dim PeriodId As String = String.Empty
        Dim MySubjectNo As String = String.Empty
        Dim DsSave As New DataSet
        Dim estr As String = String.Empty
        Dim TranNo_Retu As String = String.Empty
        Dim save As Boolean = False
        Try
            WorkspaceId = Me.HFWorkspaceId.Value.Trim()
            ActivityId = Me.HFActivityId.Value.Trim()
            NodeId = Me.HFNodeId.Value.Trim()
            PeriodId = Me.HFPeriodId.Value.Trim()

            For INDEX = 0 To Me.gvWorkSpaceSubjectMst.Rows.Count - 1

                SubjectId = IIf(Me.gvWorkSpaceSubjectMst.Rows(INDEX).Cells(GVC_SubjectId).Text = "", "0", Me.gvWorkSpaceSubjectMst.Rows(INDEX).Cells(GVC_SubjectId).Text)
                MySubjectNo = Me.gvWorkSpaceSubjectMst.Rows(INDEX).Cells(GVC_SubjectNo).Text

                If CType(Me.gvWorkSpaceSubjectMst.Rows(INDEX).FindControl("chkSave"), System.Web.UI.WebControls.CheckBox).Checked Then
                    AssignValues(SubjectId, WorkspaceId, ActivityId, NodeId, MySubjectNo, DsSave)
                    If Not Me.objLambda.Save_MedExInfoHdrDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, DsSave, True, Me.Session(S_UserID), TranNo_Retu, estr) Then
                        Me.ObjCommon.ShowAlert("Error While Saving Default Values:" & estr, Me)
                        Exit Sub
                    End If
                    DsSave = Nothing
                    DsSave = New DataSet
                    save = True
                    CType(Me.gvWorkSpaceSubjectMst.Rows(INDEX).FindControl("chkSave"), System.Web.UI.WebControls.CheckBox).Checked = False
                End If

            Next INDEX

            If save = True Then
                Me.ObjCommon.ShowAlert("Default Values Are Saved.", Me)
            End If

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub BtnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnBack.Click
        Dim WorkspaceId As String = String.Empty

        WorkspaceId = Me.HFWorkspaceId.Value.Trim()
        Me.Response.Redirect(Me.Request.QueryString("Page2") & ".aspx?Type=" & Me.Request.QueryString("Type") & _
                            "&WorkSpaceId=" & WorkspaceId.Trim() & "&page=" & Me.Request.QueryString("Page"))
    End Sub

#End Region

#Region "Error Handler"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), System.Web.UI.WebControls.Label).Text = exMessage + "<BR> " + eStr
        ObjCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), System.Web.UI.WebControls.Label).Text = exMessage + "<BR> " + eStr
        ObjCommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
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
