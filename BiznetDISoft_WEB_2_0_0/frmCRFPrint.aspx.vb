Imports System.Drawing
Imports System.Collections.Generic
Imports System.Collections
Imports System.Text
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports Winnovative

Partial Class frmCRFPrint
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()


    Private Const VS_dtMedEx_Fill As String = "dt_MedEx_Fill"
    Private Const VS_BlankCRF As String = "BlankCRF"
    Private Const VS_AuthenticatedBy As String = "AuthenticatedBy"
    Private Const VS_AuthenticatedOn As String = "AuthenticatedOn"
    Private Shared MainIdentifier As Integer = 0

#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then

            Me.AutoCompleteExtender1.ContextKey = "iUserId = " + Me.Session(S_UserID)
            Me.ddlActivtyType.Enabled = False
            Me.BtnGeneratePdf.Style.Add("display", "none")
            Me.tblactivity.Style.Add("display", "none")


            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtproject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(sender, e)
            End If

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "CRF Print"
        End If

    End Sub

#End Region

#Region "Fill checkbox list"

    Public Function fillcheckboxlist() As Boolean
        Dim wstr_AllActivities As String = String.Empty
        Dim estr As String = String.Empty
        Dim ds_AllActivities As New DataSet
        Dim dv_AllActivities As New DataView

        Try
            '*************added for subject specific or generic Activities
            If ddlActivtyType.Items.Item(1).Selected Then
                wstr_AllActivities = "vWorkSpaceId='" & Me.HProjectId.Value.Trim() & "'And cSubjectWiseFlag<>'Y' And cstatusIndi <> 'D' And IsNull(vTemplateId,'') <> '0001'   Order by iPeriod ,iNodeid" 'And ccloneFlag='H'" OR  vActivityId in ('1100','1037','1088')"
            ElseIf ddlActivtyType.Items.Item(2).Selected Then
                wstr_AllActivities = "vWorkSpaceId='" & Me.HProjectId.Value.Trim() & "'And cSubjectWiseFlag='Y' And cstatusIndi <> 'D' And IsNull(vTemplateId,'') <> '0001' Order by iPeriod ,iNodeid" 'And ccloneFlag='H'" OR  vActivityId in ('1100','1037','1088')"
            Else
                wstr_AllActivities = "vWorkSpaceId='" & Me.HProjectId.Value.Trim() & "' And cstatusIndi <> 'D' And IsNull(vTemplateId,'') <> '0001'  Order by iPeriod,iNodeid" 'And ccloneFlag='H'" OR  vActivityId in ('1100','1037','1088')"
            End If

            If Not objHelp.GetViewWorkSpaceNodeDetail(wstr_AllActivities, ds_AllActivities, estr) Then
                Me.objCommon.ShowAlert("Error while getting data from workspacenodedetail", Me.Page)
                Exit Function
            End If

            ds_AllActivities.Tables(0).Columns.Add("ChildActivityWithParent&Period")
            ds_AllActivities.AcceptChanges()

            For Each dr As DataRow In ds_AllActivities.Tables(0).Rows
                dr("ChildActivityWithParent&Period") = dr("vNodeDisplayName") + " (Period - " + Convert.ToString(dr("iPeriod")) + ")"
                If dr("iParentNodeId") > 1 Then
                    dr("ChildActivityWithParent&Period") = dr("vNodeDisplayName") + " (" + dr("ParentActivityName") + ") (Period - " + Convert.ToString(dr("iPeriod")) + ")"


                End If
            Next dr
            ds_AllActivities.AcceptChanges()

            dv_AllActivities = ds_AllActivities.Tables(0).DefaultView()
            dv_AllActivities.ToTable().AcceptChanges()
            dv_AllActivities.Sort = "iNodeId"

            Me.ChkListBoxActivity.DataSource = dv_AllActivities
            Me.ChkListBoxActivity.DataTextField = "ChildActivityWithParent&Period"
            Me.ChkListBoxActivity.DataValueField = "iNodeId"
            Me.ChkListBoxActivity.DataBind()
            Me.ChkListBoxActivity.Visible = True

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Filling Activities. ", ex.Message)
            Return False
        Finally
            ds_AllActivities.Dispose()
            dv_AllActivities.Dispose()
        End Try

    End Function

#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean

        If Not GenCall_Data() Then
            Exit Function
        End If

        If Not GenCall_ShowUI() Then
            Exit Function
        End If

        Return True

    End Function

#End Region

#Region "GenCall_Data "

    Private Function GenCall_Data() As Boolean
        Dim ds As New DataSet
        Dim estr_retu As String = String.Empty
        Dim strQuery As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim ds_CRFHdrDtlSubDtl As New DataSet
        Dim AuthenticatedOn As String
        Try

            'Added to remove value 
            Me.Session(VS_AuthenticatedOn) = ""
            Me.Session(VS_AuthenticatedBy) = ""
            If Me.ddlSubject.SelectedIndex > 0 Then

                Wstr = "cActiveFlag <> 'N' and vWorkSpaceId='" & Me.HProjectId.Value.Trim() & "'"
                Wstr += " And iNodeId in('0',"
                For Each item In Me.ChkListBoxActivity.Items
                    If item.Selected Then
                        Wstr += "'" + item.Value.Trim() + "',"
                    End If
                Next item
                If Wstr.Contains(",") Then
                    Wstr = Wstr.Substring(0, Wstr.LastIndexOf(","))
                End If

                Wstr += ") And vSubjectId = '" + Me.ddlSubject.SelectedValue.Trim() + "'"
                Wstr += " Order by iPeriod,iNodeNo,iNodeId,vMedExGroupCode,vMedExSubGroupCode,iSeqNo,View_CRFHdrDtlSubDtl_Print.iRepeatNo"
                Wstr += " OPTION (MAXDOP 1)"

                objHelp.Timeout = -1
                If Not Me.objHelp.View_CRFHdrDtlSubDtl_Print(Wstr, "*,DENSE_RANK() OVER(PARTITION BY nCRFHdrNo,vActivityId,vSubjectId ORDER BY nCRFHdrNo,vActivityId,vSubjectId,iRepeatNo) as [RepetitionNo] ", ds_CRFHdrDtlSubDtl, estr_retu) Then
                    Throw New Exception(estr_retu)
                End If

                If ds_CRFHdrDtlSubDtl.Tables(0).Rows.Count > 1 Then
                    Me.lblSubjectName.Text = ds_CRFHdrDtlSubDtl.Tables(0).Rows(1).Item("vSubjectName").ToString()
                    Me.Session(VS_dtMedEx_Fill) = ds_CRFHdrDtlSubDtl.Tables(0)
                    Me.Session(VS_BlankCRF) = "NO"
                    If ds_CRFHdrDtlSubDtl.Tables(0).Select("", "AuthenticatedOn DESC")(0).Item("AuthenticatedOn") <> "" Then
                        AuthenticatedOn = ds_CRFHdrDtlSubDtl.Tables(0).Select("", "AuthenticatedOn DESC")(0).Item("AuthenticatedOn")
                        Me.Session(VS_AuthenticatedOn) = CType(AuthenticatedOn, DateTime).ToString("dd-MMM-yyyy HH:mm") + strServerOffset 'for format like "16 July, 2011 09:03:00 PM"
                        Me.Session(VS_AuthenticatedBy) = ds_CRFHdrDtlSubDtl.Tables(0).Select("", "AuthenticatedOn DESC")(0).Item("AuthenticatedBy").ToString()
                    End If

                Else
                    Me.HFHeaderLabel.Value = Nothing
                    Me.HFHeaderLabel.Value = ""
                    Me.BtnGeneratePdf.Style.Add("display", "none")

                    PlaceMedEx.Controls.Clear()
                    Me.Session(VS_dtMedEx_Fill) = ds_CRFHdrDtlSubDtl.Tables(0)
                    objCommon.ShowAlert("Selected Subject and Activities Have No Data.", Me.Page)
                End If

            Else

                If ddlSubject.SelectedIndex > 0 Then


                Else
                    Wstr = "vWorkSpaceId = '" & Me.HProjectId.Value.Trim() & "' And vMedExType <> 'Import' "
                    Wstr += "And iNodeId in('0',"
                    For Each item In Me.ChkListBoxActivity.Items
                        If item.Selected Then
                            Wstr += "'" + item.Value.Trim() + "',"
                        End If
                    Next item
                    If Wstr.Contains(",") Then
                        Wstr = Wstr.Substring(0, Wstr.LastIndexOf(","))
                    End If
                    Wstr += ") order by iPeriod,iNodeNo,iNodeId,vMedExGroupCode,vMedExSubGroupCode,iSeqNo,RepetitionNo"   'For Lambda
                    'Wstr += ") order by iSeqNo"  'For Vasmed 

                    If Not Me.objHelp.View_BlankCRF(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr_retu) Then
                        Throw New Exception(estr_retu)
                    End If

                    Me.Session(VS_dtMedEx_Fill) = ds.Tables(0)
                    Me.Session(VS_BlankCRF) = "YES"

                End If

            End If
            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message.ToString, estr_retu)
        Finally
            ds.Dispose()
            ds_CRFHdrDtlSubDtl.Dispose()
        End Try
    End Function

#End Region

#Region "DisplayHeader"

    Private Function DisplayHeader() As Boolean
        Dim Wstr As String = ""
        Dim ds_Heading As New DataSet
        Dim estr As String = ""

        Try

            Wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "'"

            If Not Me.objHelp.View_MyProjects(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition _
                                              , ds_Heading, estr) Then
                Throw New Exception("Error while getting Header information. " + estr)
            End If

            If ds_Heading.Tables(0).Rows.Count > 0 Then
                Me.spnhdr.InnerHtml = System.Configuration.ConfigurationManager.AppSettings("Client")
                Me.spnBaBe.InnerHtml = "Sponsor Name: " + ds_Heading.Tables(0).Rows(0).Item("vClientName").ToString.Trim()
                Me.lblProjectNo.Text = ds_Heading.Tables(0).Rows(0).Item("vProjectNo").ToString.Trim()
                Me.lblSubjectNo.Text = "_"
                Me.lblSubjectName.Text = "_"
                Me.tdSiteName.Style.Add("display", "none")
                Me.tdSiteId.Style.Add("display", "none")
                Me.trProtocol.Style.Add("display", "none")

                'check for profile edited by vishal
                If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then

                    Dim strLen As Int32 = ds_Heading.Tables(0).Rows(0).Item("vProjectNo").ToString.Trim().LastIndexOf("-")
                    Me.lblProjectNo.Text = ds_Heading.Tables(0).Rows(0).Item("vProjectNo").ToString.Trim()

                    If strLen > 5 Then
                        Me.tdSiteName.Style.Add("display", "")
                        Me.tdSiteId.Style.Add("display", "")
                        Me.lblProjectNo.Text = ds_Heading.Tables(0).Rows(0).Item("vProjectNo").ToString.Trim().Substring(0, strLen)
                        strLen = strLen + 1
                        Me.lblSiteNo.Text = ds_Heading.Tables(0).Rows(0).Item("vProjectNo").ToString.Trim().Substring(strLen)
                    End If


                    Me.lblSubjectNo.Text = "_"
                    Me.lblSubjectName.Text = "_"

                End If
                If ds_Heading.Tables(0).Rows(0).Item("ProtocolNo").ToString.Length > 0 Then

                    Me.trProtocol.Style.Add("display", "")
                    Me.LblProtocol.Text = ds_Heading.Tables(0).Rows(0).Item("ProtocolNo").ToString.Trim()
                End If

                '' Added By dipen Shah on 16/07/2014 To get the CRF Print with filled data with the subjectNo '0000'.
                If ddlActivtyType.SelectedIndex = 3 Then
                    If Me.ddlSubject.SelectedIndex = 1 Then
                        Me.lblSubjectNo.Text = "0000"
                        Me.lblSubjectName.Text = "-"
                    End If
                ElseIf ddlActivtyType.SelectedIndex = 1 Then

                    If Me.ddlSubject.SelectedIndex > 0 Then
                        Me.lblSubjectNo.Text = "0000"
                        Me.lblSubjectName.Text = "-"
                        Me.SpnSubjectInit.Style.Add("display", "none")
                        Me.SpnSubject.Style.Add("display", "none")
                        Me.lblSubjectNo.Visible = False
                    End If
                Else
                    'to add Project no. to the Header of PDF
                    If Me.ddlSubject.SelectedIndex > 0 Then
                        Me.lblSubjectNo.Text = ddlSubject.SelectedItem.Text.Substring(0, ddlSubject.SelectedItem.Text.IndexOf(" "))
                        Me.lblSubjectName.Text = ddlSubject.SelectedItem.Text.Substring(ddlSubject.SelectedItem.Text.IndexOf(" "), CInt(ddlSubject.SelectedItem.Text.Length - ddlSubject.SelectedItem.Text.IndexOf(" "))) + "  "
                    End If
                End If
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        Finally
            ds_Heading.Dispose()
        End Try
    End Function

#End Region

#Region "GenCall_ShowUI "

    Private Function GenCall_ShowUI() As Boolean

        Dim dt As New DataTable
        Dim dv As New DataView
        Dim dt_MedExMst As New DataTable
        Dim dr As DataRow
        Dim drGroup As DataRow
        Dim objelement As Object
        Dim dt_MedexGroup As New DataTable
        Dim CntSubGroup As Integer = 0
        Dim PrevSubGroupCodes As String = ""
        Dim Nodeid As Integer = 0
        Dim PreviousNodeID As Integer = 1
        Dim NodeDisplayName As String = String.Empty
        Dim dv_MedexGrp As New DataView
        Dim drNode As DataRow
        Dim dtNodeID As New DataTable
        Dim Index As Integer = 0
        Dim SubjectIndex As Integer = 0
        Dim dt_Subject As New DataTable
        Dim dr_Subject As DataRow
        Dim SubjectId As String = ""
        Dim Prev_SubjectId As String = ""
        Dim SubjectName As String = ""
        Dim MedIndex As Integer = 0
        Dim Identifier As Integer = 0
        Dim ParentNodeId As Integer = 0
        Dim PreviousParentNodeId As Integer = 0
        Dim dtPeriod As New DataTable
        Dim dtParentActivity As New DataTable
        Dim Period As Integer = 0
        Dim PreviousPeriod As Integer = 0
        Dim dtMain As New DataTable
        Dim dvMain As New DataView
        Dim dvSort As New DataView
        Dim NodeIds As String = ""
        Dim dtData As New DataTable


        Try

            Page.Title = " ::  CRF Print ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            PlaceMedEx.Controls.Clear()

            If CType(Me.Session(VS_dtMedEx_Fill), DataTable).Rows.Count < 1 Then

                If Not IsPostBack Then
                    Me.objCommon.ShowAlert("No Attribute is Attached with This Activity. So, please Attach Attribute to this Activity.", Me.Page)
                    Exit Function
                End If

            End If
            'check for profile edited by vishal

            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                ' Me.spnBaBe.InnerHtml = "Department: Clinical Trial Management"
                Me.SpnSubject.InnerHtml = "Patient No:"
                Me.SpnSubjectInit.InnerHtml = "Patient Initials:"
            End If

            PlaceMedEx.Controls.Add(New LiteralControl("<Table Width=""100%"">"))

            dt_Subject = CType(Me.Session(VS_dtMedEx_Fill), DataTable).Copy.DefaultView.ToTable(True, "vSubjectId,vSubjectName".Split(","))

            For Each dr_Subject In dt_Subject.Rows

                SubjectIndex += 1
                SubjectId = Convert.ToString(dr_Subject("vSubjectId")).Trim()
                SubjectName = Convert.ToString(dr_Subject("vSubjectName")).Trim()

                dtPeriod = CType(Me.Session(VS_dtMedEx_Fill), DataTable).Copy.DefaultView.ToTable(True, "iPeriod".Split(","))
                Period = 0
                PreviousPeriod = 0
                For Each drPeriod As DataRow In dtPeriod.Rows

                    Period = drPeriod("iPeriod")
                    PreviousPeriod = drPeriod("iPeriod")

                    dvMain = CType(Me.Session(VS_dtMedEx_Fill), DataTable).Copy.DefaultView
                    dvMain.RowFilter = "iPeriod = " + Period.ToString()
                    dvSort = dvMain.ToTable.Copy.DefaultView.ToTable(True, "iParentNodeId,ParentActivityName,ParentNodeNo".Split(",")).DefaultView
                    dvSort.RowFilter = "isnull(iParentNodeId,0) <> 0"
                    dvSort.Sort = "ParentNodeNo asc"
                    dtParentActivity = dvSort.ToTable()
                    ParentNodeId = 0
                    PreviousParentNodeId = 0

                    For Each drParentActivity As DataRow In dtParentActivity.Rows

                        'If drParentActivity("parentActivityName") = "New Project Feasibility" Then
                        '    Continue For
                        'End If
                        ParentNodeId = drParentActivity("iParentNodeID")

                        If ParentNodeId = 1 Then
                            Continue For
                        End If

                        If ParentNodeId <> PreviousParentNodeId AndAlso ParentNodeId <> 0 Then
                            If ParentNodeId <> 1 Then
                                PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""100%"" ALIGN=LEFT style=""BACKGROUND-COLOR: #FFB895; page-break-inside:avoid; "">")) '008ecd''redish orange for parent activity
                                If PreviousParentNodeId = 0 Then
                                    PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""vertical-align:middle; text-align: center; font-size:small; font-weight:bold;"">")) 'white-space: nowrap; 
                                Else
                                    PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""vertical-align:middle; text-align: center; font-size:small; font-weight:bold;"">")) 'white-space: nowrap; 
                                End If
                                PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
                                PlaceMedEx.Controls.Add(New LiteralControl(Convert.ToString(drParentActivity("ParentActivityName")).Trim()))
                                PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                                PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
                            End If
                        End If
                        PreviousParentNodeId = drParentActivity("iParentNodeID")

                        dvMain.RowFilter = "(iPeriod = " + Period.ToString() + " And iNodeId = " + ParentNodeId.ToString() + ") Or (iPeriod = " + Period.ToString() + " And iParentNodeId = " + ParentNodeId.ToString() + ")"
                        dtMain = dvMain.ToTable().Copy()
                        dvSort = Nothing
                        dvSort = dtMain.Copy.DefaultView.ToTable(True, "iNodeID,vNodeDisplayName,iNodeNo,ParentNodeNo".Split(",")).DefaultView
                        dvSort.Sort = "ParentNodeNo,iNodeNo asc"
                        dtNodeID = dvSort.ToTable()
                        Nodeid = 0
                        PreviousNodeID = 0

                        For Each drNode In dtNodeID.Rows

                            NodeIds += Convert.ToString(drNode("iNodeID")).Trim() + ","

                            Nodeid = drNode("iNodeID")
                            NodeDisplayName = Convert.ToString(drNode("vNodeDisplayName")).Trim()

                            dv_MedexGrp = dtMain.Copy.DefaultView
                            dv_MedexGrp.RowFilter = "iNodeID = " + Nodeid.ToString() ' + " And vSubjectId = '" + SubjectId + "'"

                            Dim dt_Repeatition As DataTable
                            dt_Repeatition = dv_MedexGrp.ToTable(True, "RepetitionNo".Split(","))

                            Dim PreviousRepeatition As Integer = 0

                            For Each drRepeatition As DataRow In dt_Repeatition.Rows

                                dv_MedexGrp.RowFilter = "RepetitionNo = " + Convert.ToString(drRepeatition("RepetitionNo")).ToString()

                                dt_MedexGroup = dv_MedexGrp.ToTable(True, "vMedExGroupCode".Split(","))

                                PlaceMedEx.Controls.Add(New LiteralControl("<Tr ALIGN=LEFT >"))
                                PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""vertical-align:top"">")) 'white-space: nowrap; 

                                If PreviousRepeatition <> drRepeatition("RepetitionNo") Then

                                    If (PreviousNodeID <> Nodeid) Or (PreviousNodeID = Nodeid AndAlso PreviousRepeatition <> drRepeatition("RepetitionNo")) Then

                                        PlaceMedEx.Controls.Add(New LiteralControl("<Table width=""100%"" cellspacing='0' >")) ''blue line of activity
                                        PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""100%"" color:#FFFFFF; ALIGN=LEFT style=""BACKGROUND-COLOR: #008ecd; page-break-inside:avoid;"">")) '008ecd

                                        If ParentNodeId = 1 Then
                                            'PlaceMedEx.Controls.Add(New LiteralControl("<Td style="" style=""vertical-align:middle;  "">")) 'white-space: nowrap; 
                                            PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""vertical-align:middle;color:#FFFFFF;  width: 80%"">")) 'white-space: nowrap; 
                                        Else
                                            PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""vertical-align:middle;color:#FFFFFF;  width: 80%"">")) 'white-space: nowrap; 
                                        End If

                                        dvMain.RowFilter = "iNodeId = " + Nodeid.ToString()
                                        dtData = dvMain.ToTable()

                                        PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
                                        If drRepeatition("RepetitionNo") > 1 Then
                                            PlaceMedEx.Controls.Add(New LiteralControl(NodeDisplayName + ", Repeatition: " + Convert.ToString(drRepeatition("RepetitionNo"))))
                                        Else
                                            PlaceMedEx.Controls.Add(New LiteralControl(NodeDisplayName))
                                        End If
                                        If RBLProjecttype.SelectedIndex = 1 Then
                                            If dtData.Rows.Count > 0 Then
                                                If dtData.Rows(0)("vMedExGroupCDISCValue").ToString() <> "" Then
                                                    PlaceMedEx.Controls.Add(GetMedExGroupCDISC(IIf(dtData.Rows(0)("vMedExGroupCDISCValue") Is System.DBNull.Value, " ", dtData.Rows(0)("vMedExGroupCDISCValue")), dtData.Rows(0)("vMedExGroupCode"), dtData.Rows(0)("vMedExSubGroupCode"), dtData.Rows(0)("vMedExGroupCode") + dtData.Rows(0)("vMedExSubGroupCode") + dtData.Rows(0)("vMedExGroupCode") + Nodeid.ToString() + "R" + Index.ToString() + SubjectIndex.ToString() + MedIndex.ToString() + Identifier.ToString()))
                                                End If
                                            End If
                                        End If
                                        PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))


                                        '******Added For Showing Period*************
                                        If Not Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                                            PlaceMedEx.Controls.Add(New LiteralControl("<Td align=""right""  style=""color:#FFFFFF ;width: 20%"">")) ''style=""vertical-align:right;""
                                            PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;Period: "))
                                            PlaceMedEx.Controls.Add(New LiteralControl(Period.ToString()))
                                            PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                                        End If

                                        '********************************************

                                        PlaceMedEx.Controls.Add(New LiteralControl("</Tr>")) ''activty row done
                                        PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""100%"" height=""1px"" ALIGN=LEFT style=""BACKGROUND-COLOR: #FFFFFF; "">"))
                                        PlaceMedEx.Controls.Add(New LiteralControl("<Td style=""vertical-align:middle"" >")) 'white-space: nowrap; 
                                        PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                                        PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
                                        PlaceMedEx.Controls.Add(New LiteralControl("</Table>"))

                                    End If

                                End If

                                PreviousRepeatition = drRepeatition("RepetitionNo")

                                PreviousNodeID = drNode("iNodeID")

                                Index = 0

                                PlaceMedEx.Controls.Add(New LiteralControl("<Table width=""100%"" cellspacing='0' style=""border: solid 1px gray"">")) 'Added on 30-01-2010 to fix the size of display

                                For Each drGroup In dt_MedexGroup.Rows

                                    Index += 1

                                    dt = New DataTable
                                    dt_MedExMst = New DataTable
                                    dt = dtMain.Copy()
                                    dv = New DataView
                                    dv = dt.DefaultView
                                    dv.RowFilter = "iNodeID = " + Nodeid.ToString() + " And vMedExGroupCode='" & drGroup("vMedExGroupCode").ToString.Trim() & "'" + " And vSubjectId = '" + SubjectId + "' And RepetitionNo = " + Convert.ToString(drRepeatition("RepetitionNo"))
                                    dv.Sort = "iSeqNo,nmedexworkspacedtlno asc"
                                    dt_MedExMst = dv.ToTable()

                                    'PlaceMedEx.Controls.Add(New LiteralControl("<Table width=""100%"" cellspacing='0' style=""border: solid 1px gray"">")) 'Added on 30-01-2010 to fix the size of display

                                    MedIndex = 0
                                    For Each dr In dt_MedExMst.Rows

                                        MedIndex += 1
                                        If PrevSubGroupCodes = "" Or PrevSubGroupCodes <> dr("vMedExSubGroupCode") Then

                                            If Convert.ToString(dr("vMedExSubGroupCode")).Trim() <> "0000" Then 'Added for default SubGroup

                                                PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""100%"" ALIGN=LEFT style=""BACKGROUND-COLOR: #ffcc66; page-break-inside:avoid;"">")) 'ffcc66
                                                PlaceMedEx.Controls.Add(New LiteralControl("<Td colspan=""2"" style=""vertical-align:middle"">"))
                                                PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
                                                PlaceMedEx.Controls.Add(Getlable(dr("vMedExSubGroupDesc").ToString.Trim(), dr("vMedExSubGroupCode") + CntSubGroup.ToString.Trim() + Nodeid.ToString() + "R" + Index.ToString() + SubjectIndex.ToString() + MedIndex.ToString()))


                                                CntSubGroup += 1
                                                PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                                                PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                                            End If

                                        End If

                                        PlaceMedEx.Controls.Add(New LiteralControl("<Tr ALIGN=LEFT  style=""page-break-inside:avoid;"">"))

                                        PrevSubGroupCodes = dr("vMedExSubGroupCode")
                                        ''''''''''''2nd td of white row
                                        If Convert.ToString(dr("vMedExType")).Trim.ToUpper() <> "LABEL" Then

                                            Identifier += 1 'for content TD of activity
                                            PlaceMedEx.Controls.Add(New LiteralControl("<Td width=""60%"" style=""  !important; vertical-align:middle; "" ALIGN=LEFT>")) 'border:1px solid gray;
                                            PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
                                            PlaceMedEx.Controls.Add(GetlableWithHistory(dr("vMedExDesc") & ": ", dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExGroupCode") + dr("vMedExSubGroupCode") + dr("vMedExCode") + Nodeid.ToString() + "R" + Index.ToString() + SubjectIndex.ToString() + MedIndex.ToString() + Identifier.ToString()))
                                            If RBLProjecttype.SelectedIndex = 1 Then
                                                PlaceMedEx.Controls.Add(GetCDISC(IIf(dr("vCDISCValues") Is System.DBNull.Value, " ", dr("vCDISCValues")), dr("vMedExGroupCode"), dr("vMedExSubGroupCode"), dr("vMedExGroupCode") + dr("vMedExSubGroupCode") + dr("vMedExCode") + Nodeid.ToString() + "R" + Index.ToString() + SubjectIndex.ToString() + MedIndex.ToString() + Identifier.ToString()))
                                            End If

                                            PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                                            PlaceMedEx.Controls.Add(New LiteralControl("<Td width=""40%"" style=""vertical-align:middle; border-left:1px solid gray;"" ALIGN=LEFT>")) 'border:1px solid gray;'white-space: nowrap; 

                                        Else

                                            PlaceMedEx.Controls.Add(New LiteralControl("<Td colspan=""2"" style="" !important; vertical-align:middle"" ALIGN=LEFT>"))

                                        End If

                                        If Convert.ToString(dr("vMedExType")).Trim.ToUpper() <> "LABEL" And _
                                                    Convert.ToString(dr("vMedExType")).Trim.ToUpper() <> "FILE" And _
                                                    Convert.ToString(dr("vMedExType")).Trim.ToUpper() <> "RADIO" And _
                                                    Convert.ToString(dr("vMedExType")).Trim.ToUpper() <> "CHECKBOX" Then
                                            PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))
                                        End If

                                        MainIdentifier += 1

                                        objelement = GetObject(dr("vMedExType"), dr("vMedExCode") + Nodeid.ToString() + "R" + Index.ToString() + SubjectIndex.ToString() + MedIndex.ToString() + MainIdentifier.ToString(), dr("vMedExValues"), IIf(dr("vDefaultValue") Is System.DBNull.Value, "", dr("vDefaultValue")), _
                                                                    IIf(dr("vRefTable") Is System.DBNull.Value, "", dr("vRefTable")), _
                                                                    IIf(dr("vRefColumn") Is System.DBNull.Value, "", dr("vRefColumn")))


                                        PlaceMedEx.Controls.Add(objelement)

                                        'For Print UOM 
                                        If Not dr("vUOM") Is System.DBNull.Value AndAlso Convert.ToString(dr("vUOM")).Trim() <> "" Then
                                            PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;&nbsp;&nbsp;"))
                                            PlaceMedEx.Controls.Add(Getlable(dr("vUOM"), "UOM" + dr("vMedExCode") + Nodeid.ToString() + "R" + Index.ToString() + SubjectIndex.ToString() + MedIndex.ToString() + MainIdentifier.ToString()))
                                        End If
                                        '******************

                                        PlaceMedEx.Controls.Add(New LiteralControl("</Td>")) ''''''''''END of 2nd td of white row
                                        PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
                                        '**********for intial black line
                                        'PlaceMedEx.Controls.Add(New LiteralControl("<thead style=""display:table-header-group"">"))
                                        'PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""100%"" height=""1px"" ALIGN=LEFT style=""BACKGROUND-COLOR: #000000;"">"))
                                        PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""100%"" height=""1px"" ALIGN=LEFT>"))
                                        PlaceMedEx.Controls.Add(New LiteralControl("<Td colspan=""2"" style=""vertical-align:middle"">")) 'white-space: nowrap; 
                                        PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                                        PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
                                        'PlaceMedEx.Controls.Add(New LiteralControl("</thead>"))
                                        '**********For User's readibility**************line after 1st attribute of activity
                                        PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""100%"" height=""1px"" ALIGN=LEFT style=""BACKGROUND-COLOR: #000000;"">"))
                                        PlaceMedEx.Controls.Add(New LiteralControl("<Td colspan=""2"" style=""vertical-align:middle"">")) 'white-space: nowrap; 
                                        PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                                        PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
                                        '****************************************

                                    Next dr

                                Next drGroup

                                PlaceMedEx.Controls.Add(New LiteralControl("</Table>"))
                                PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                                PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                            Next drRepeatition
                        Next drNode

                    Next drParentActivity


                Next drPeriod

                Prev_SubjectId = Convert.ToString(dr_Subject("vSubjectId")).Trim()

            Next dr_Subject

            PlaceMedEx.Controls.Add(New LiteralControl("</Table>"))

            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message.ToString, "")
        Finally
            dt.Dispose()
            dv.Dispose()
            dt_MedExMst.Dispose()
            dt_MedexGroup.Dispose()
            dv_MedexGrp.Dispose()
            dtNodeID.Dispose()
            dt_Subject.Dispose()
            dtPeriod.Dispose()
            dtParentActivity.Dispose()
            dtMain.Dispose()
            dvMain.Dispose()
            dvSort.Dispose()
        End Try

    End Function

#End Region

#Region "Getlable"

    Private Function Getlable(ByVal vlabelName As String, ByVal Id As String, Optional ByVal vFieldType As String = "") As Label
        Dim lab As Label

        Try
            lab = New Label
            lab.ID = "Lab" & Id
            lab.Text = vlabelName.Trim()
            'lab.SkinID = "lblDisplay"
            lab.ForeColor = System.Drawing.Color.FromName("Navy")
            lab.Font.Size = 9
            lab.Font.Bold = True
            lab.Font.Name = "Georgia"
            If vFieldType.ToUpper.Trim() = "IMPORT" Then
                lab.Visible = False
            End If
            lab.EnableViewState = False
            Getlable = lab
            lab.Dispose()
        Catch ex As Exception
            Me.ShowErrorMessage("Error while calling Getlable..", ex.Message, ex)
        End Try
    End Function

    Private Function GetlableForSubGroup(ByVal vlabelName As String, ByVal Id As String, Optional ByVal vFieldType As String = "") As Label
        Dim lab As Label
        lab = New Label
        lab.ID = "Lab" & Id
        lab.Text = vlabelName.Trim()
        lab.SkinID = "lblDisplay"
        lab.ForeColor = System.Drawing.Color.FromName("Navy")
        If vFieldType.ToUpper.Trim() = "IMPORT" Then
            lab.Visible = False
        End If

        'lab.CssClass = "Label"
        lab.Width = "800px"
        lab.Style.Add("word-wrap", "break-word")
        lab.Style.Add("white-space", "")
        lab.EnableViewState = False
        GetlableForSubGroup = lab
        lab.Dispose()
    End Function

    Private Function GetlableWithHistory(ByVal vlabelName As String, ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal MedExCode As String) As Label
        Dim lnk As Label
        lnk = New Label
        lnk.ID = "Lnk" + MedExCode
        lnk.Text = vlabelName.Trim()
        lnk.SkinID = "LabelDisplay"
        'lnk.Width = 800
        lnk.Style.Add("word-wrap", "break-word")
        lnk.Style.Add("white-space", "")
        lnk.EnableViewState = False
        GetlableWithHistory = lnk
        lnk.Dispose()
    End Function

    Private Function GetCDISC(ByVal vCDISCValues As String, ByVal MedExGroupCode As String, ByVal MedExSubGroupCode As String, ByVal MedExCode As String) As Label
        Dim lnk As Label
        lnk = New Label
        lnk.ID = "Lnkcdisc" + MedExCode

        lnk.Text = " [" + vCDISCValues.Trim() + "]"
        If lnk.Text <> " []" Then
            lnk.SkinID = "LabelDisplay"
            lnk.BackColor = Color.Yellow
            lnk.BorderColor = Color.Black
            lnk.BorderWidth = 2
            lnk.BorderStyle = BorderStyle.Solid
            lnk.ForeColor = Color.Black
        End If
        lnk.Style.Add("word-wrap", "break-word")
        lnk.Style.Add("white-space", "")
        lnk.EnableViewState = False
        GetCDISC = lnk
        lnk.Dispose()
    End Function

    Private Function GetMedExGroupCDISC(ByVal vCDISCValues As String, _
                                        ByVal MedExGroupCode As String, _
                                        ByVal MedExSubGroupCode As String, _
                                        ByVal MedExCode As String) As Label
        Dim lnk As Label
        lnk = New Label
        lnk.ID = "Lnkcdisc" + MedExGroupCode + MedExSubGroupCode + MedExCode

        lnk.Text = "  [DOMAIN:" + vCDISCValues.Trim() + "]"
        If lnk.Text <> " []" Then
            lnk.SkinID = "LabelDisplay"
            lnk.BackColor = Color.Yellow
            lnk.BorderColor = Color.Black
            lnk.BorderWidth = 2
            lnk.BorderStyle = BorderStyle.Solid
            lnk.ForeColor = Color.Black
        End If
        lnk.Style.Add("word-wrap", "break-word")
        lnk.Style.Add("white-space", "")
        lnk.EnableViewState = False
        GetMedExGroupCDISC = lnk
        lnk.Dispose()
    End Function

#End Region

#Region "GetButtons"

    Private Function GetButtons(ByVal BtnName As String, ByVal Id As String) As Boolean
        Dim Btn As Button
        Dim index As Integer
        Dim StrGroupCode_arry() As String
        Dim StrGroupDesc_arry() As String
        Try
            StrGroupCode_arry = Id.Split(",")
            StrGroupDesc_arry = BtnName.Split(",")
            For index = 0 To StrGroupCode_arry.Length - 1
                Btn = New Button
                Btn.ID = "Btn" & StrGroupCode_arry(index)
                Btn.Text = " " & StrGroupDesc_arry(index).Trim() & " "

                If StrGroupDesc_arry(index).Trim().Length > 30 Then
                    Btn.Text = " " & StrGroupDesc_arry(index).Substring(0, 30).Trim() & "... "
                End If

                Btn.ToolTip = StrGroupDesc_arry(index).Trim()
                Btn.CssClass = "buttonForTabActive"

                If index = 0 Then
                    Btn.CssClass = "buttonForTabInActive"
                End If

                Btn.OnClientClick = "return DisplayDiv('" & StrGroupCode_arry(index) & "','" & Id & "');"
                PlaceMedEx.Controls.Add(Btn)
                PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
                PlaceMedEx.Controls.Add(New LiteralControl("&nbsp;"))
            Next
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Function

#End Region

#Region "GetObject"

    Private Function GetObject(ByVal vFieldType As String, ByVal Id As String, ByVal vMedExValues As String, _
                                ByVal dtValues As String, Optional ByVal RefTable As String = "", _
                                Optional ByVal RefColumn As String = "") As Object
        Dim txt As TextBox
        Dim ddl As DropDownList
        Dim FileBro As FileUpload
        Dim RBL As RadioButtonList
        Dim CBL As CheckBoxList
        Dim lbl As Label

        Select Case vFieldType.ToUpper.Trim()
            Case "TEXTBOX"

                If Me.Session(VS_BlankCRF) = "YES" Then

                    lbl = New Label
                    lbl.ID = "txt" + Id
                    lbl.Style.Add("word-wrap", "break-word")
                    lbl.Style.Add("white-space", "none")
                    lbl.Text = dtValues.Trim()
                Else
                    lbl = New Label
                    lbl.ID = "txt" + Id
                    lbl.Style.Add("word-wrap", "break-word")
                    lbl.Style.Add("white-space", "none")
                    lbl.Text = dtValues.Trim()
                End If
                lbl.EnableViewState = False
                GetObject = lbl
            Case "COMBOBOX"

                If Me.Session(VS_BlankCRF) = "YES" Then
                    Dim Arrvalue() As String = Nothing
                    Dim i As Integer
                    Dim ds_ddl As New DataSet
                    Dim estr As String = ""
                    ddl = New DropDownList
                    ddl.ID = Id
                    ddl.CssClass = "dropDownList"
                    ddl.EnableViewState = False
                    If RefTable.Trim() <> "" Then
                        If Not Me.objHelp.GetFieldsOfTable(RefTable.Trim(), RefColumn.Trim(), "", ds_ddl, estr) Then
                            Me.objCommon.ShowAlert(estr, Me.Page())
                            Return Nothing
                        End If
                        ds_ddl.Tables(0).DefaultView.Sort = RefColumn
                        ddl.DataSource = ds_ddl.Tables(0).DefaultView.ToTable()
                        ddl.DataTextField = RefColumn
                        ddl.DataValueField = RefColumn
                        ddl.DataBind()
                    Else
                        Arrvalue = Split(vMedExValues, ",")
                        For i = 0 To Arrvalue.Length - 1
                            ddl.Items.Add(New System.Web.UI.WebControls.ListItem(Arrvalue(i).Trim(), Arrvalue(i).Trim()))
                            ddl.Items(i).Attributes.Add("title", Arrvalue(i).Trim())
                        Next
                        If Not dtValues = "" Then
                            ddl.SelectedValue = dtValues.Trim()
                            ddl.Attributes.Add("title", dtValues)
                        End If
                    End If
                    GetObject = ddl
                    lbl = New Label
                    lbl.ID = "ddl" + Id
                    lbl.Style.Add("word-wrap", "break-word")
                    lbl.Style.Add("white-space", "none")
                    lbl.Text = dtValues.Trim()
                    lbl.EnableViewState = False
                    GetObject = lbl
                Else
                    lbl = New Label
                    lbl.ID = "ddl" + Id
                    lbl.Style.Add("word-wrap", "break-word")
                    lbl.Style.Add("white-space", "none")
                    lbl.Text = dtValues.Trim()
                    lbl.EnableViewState = False
                    GetObject = lbl
                End If

            Case "RADIO"
                Dim Arrvalue() As String = Nothing
                Dim i As Integer
                Dim ds_Radio As New DataSet
                Dim estr As String = ""
                RBL = New RadioButtonList
                RBL.ID = "rdo" + Id
                RBL.EnableViewState = False
                If RefTable.Trim() <> "" Then
                    If Not Me.objHelp.GetFieldsOfTable(RefTable.Trim(), RefColumn.Trim(), "", ds_Radio, estr) Then
                        Me.objCommon.ShowAlert(estr, Me.Page())
                        Return Nothing
                    End If
                    ds_Radio.Tables(0).DefaultView.Sort = RefColumn
                    RBL.DataSource = ds_Radio.Tables(0).DefaultView.ToTable()
                    RBL.DataTextField = RefColumn
                    RBL.DataValueField = RefColumn
                    RBL.DataBind()
                Else
                    Arrvalue = Split(vMedExValues, ",")
                    For i = 0 To Arrvalue.Length - 1
                        RBL.Items.Add(New System.Web.UI.WebControls.ListItem(Arrvalue(i).Trim(), Arrvalue(i).Trim().ToUpper()))
                    Next
                    If Not dtValues = "" Then
                        RBL.SelectedValue = dtValues.Trim().ToUpper()
                    End If
                End If
                RBL.RepeatDirection = RepeatDirection.Horizontal
                RBL.RepeatColumns = 3
                GetObject = RBL

            Case "CHECKBOX"
                Dim Arrvalue() As String = Nothing
                Dim Defvalue() As String = Nothing
                Dim i As Integer
                Dim ds_Check As New DataSet
                Dim estr As String = ""
                CBL = New CheckBoxList
                CBL.ID = "chk" + Id
                CBL.EnableViewState = False
                If RefTable.Trim() <> "" Then
                    If Not Me.objHelp.GetFieldsOfTable(RefTable.Trim(), RefColumn.Trim(), "", ds_Check, estr) Then
                        Me.objCommon.ShowAlert(estr, Me.Page())
                        Return Nothing
                    End If
                    ds_Check.Tables(0).DefaultView.Sort = RefColumn
                    CBL.DataSource = ds_Check.Tables(0).DefaultView.ToTable()
                    CBL.DataTextField = RefColumn
                    CBL.DataValueField = RefColumn
                    CBL.DataBind()
                Else
                    Arrvalue = Split(vMedExValues, ",")
                    For i = 0 To Arrvalue.Length - 1
                        CBL.Items.Add(New System.Web.UI.WebControls.ListItem(Arrvalue(i).Trim(), Arrvalue(i).Trim()))
                    Next
                End If
                CBL.ClearSelection()
                If Not dtValues = "" Then
                    Defvalue = Split(dtValues, ",")
                    For i = 0 To Defvalue.Length - 1
                        For Each item As System.Web.UI.WebControls.ListItem In CBL.Items
                            If item.Value.Trim().ToUpper() = Defvalue(i).Trim().ToUpper() Then
                                item.Selected = True
                                Exit For
                            End If
                        Next item
                    Next i
                End If
                CBL.RepeatDirection = RepeatDirection.Horizontal
                CBL.RepeatColumns = 3
                GetObject = CBL

            Case "FILE"
                FileBro = New FileUpload
                FileBro.ID = "FU" + Id
                FileBro.CssClass = "textBox"
                FileBro.EnableViewState = False
                GetObject = FileBro

            Case "TEXTAREA"

                If Me.Session(VS_BlankCRF) = "YES" Then
                    lbl = New Label
                    lbl.ID = "txta" + Id
                    lbl.Style.Add("word-wrap", "break-word")
                    lbl.Style.Add("white-space", "none")
                    lbl.Text = dtValues.Trim()
                    lbl.EnableViewState = False
                    GetObject = lbl
                Else
                    lbl = New Label
                    lbl.ID = "txta" + Id
                    lbl.Style.Add("word-wrap", "break-word")
                    lbl.Style.Add("white-space", "none")
                    lbl.Text = dtValues.Trim()
                    lbl.EnableViewState = False
                    GetObject = lbl
                End If

            Case "DATETIME", "STANDARDDATE"

                If Me.Session(VS_BlankCRF) = "YES" Then
                    lbl = New Label
                    lbl.ID = "txtd" + Id
                    lbl.Style.Add("word-wrap", "break-word")
                    lbl.Style.Add("white-space", "none")
                    lbl.Text = dtValues.Trim()
                    lbl.EnableViewState = False
                    GetObject = lbl
                Else
                    lbl = New Label
                    lbl.ID = "txtd" + Id
                    lbl.Style.Add("word-wrap", "break-word")
                    lbl.Style.Add("white-space", "none")
                    lbl.Text = dtValues.Trim()
                    lbl.EnableViewState = False
                    GetObject = lbl
                End If

            Case "TIME"

                If Me.Session(VS_BlankCRF) = "YES" Then
                    lbl = New Label
                    lbl.ID = "txtt" + Id
                    lbl.Style.Add("word-wrap", "break-word")
                    lbl.Style.Add("white-space", "none")
                    lbl.Text = dtValues.Trim()
                    lbl.EnableViewState = False
                    GetObject = lbl
                Else
                    lbl = New Label
                    lbl.ID = "txtt" + Id
                    lbl.Style.Add("word-wrap", "break-word")
                    lbl.Style.Add("white-space", "none")
                    lbl.Text = dtValues.Trim()
                    lbl.EnableViewState = False
                    GetObject = lbl
                End If


            Case "ASYNCDATETIME"

                If Me.Session(VS_BlankCRF) = "YES" Then
                    lbl = New Label
                    lbl.ID = "txtd" + Id
                    lbl.Style.Add("word-wrap", "break-word")
                    lbl.Style.Add("white-space", "none")
                    lbl.Text = dtValues.Trim()
                    lbl.EnableViewState = False
                    GetObject = lbl
                Else
                    lbl = New Label
                    lbl.ID = "txtd" + Id
                    lbl.Style.Add("word-wrap", "break-word")
                    lbl.Style.Add("white-space", "none")
                    lbl.Text = dtValues.Trim()
                    lbl.EnableViewState = False
                    GetObject = lbl
                End If

            Case "ASYNCTIME"

                If Me.Session(VS_BlankCRF) = "YES" Then
                    lbl = New Label
                    lbl.ID = "txtt" + Id
                    lbl.Style.Add("word-wrap", "break-word")
                    lbl.Style.Add("white-space", "none")
                    lbl.Text = dtValues.Trim()
                    lbl.EnableViewState = False
                    GetObject = lbl
                Else
                    lbl = New Label
                    lbl.ID = "txtt" + Id
                    lbl.Style.Add("word-wrap", "break-word")
                    lbl.Style.Add("white-space", "none")
                    lbl.Text = dtValues.Trim()
                    lbl.EnableViewState = False
                    GetObject = lbl
                End If

            Case "IMPORT"

                If Me.Session(VS_BlankCRF) = "YES" Then
                    lbl = New Label
                    lbl.ID = "txti" + Id
                    lbl.Style.Add("word-wrap", "break-word")
                    lbl.Style.Add("white-space", "none")
                    lbl.Text = dtValues.Trim()
                    lbl.EnableViewState = False
                    GetObject = lbl
                Else
                    lbl = New Label
                    lbl.ID = "txti" + Id
                    lbl.Style.Add("word-wrap", "break-word")
                    lbl.Style.Add("white-space", "none")
                    lbl.Text = dtValues.Trim()
                    lbl.EnableViewState = False
                    GetObject = lbl
                End If

            Case "COMBOGLOBALDICTIONARY"

                If Me.Session(VS_BlankCRF) = "YES" Then
                    txt = New TextBox
                    txt.ID = Id
                    txt.CssClass = "textBox"
                    txt.Text = dtValues
                    txt.Attributes.Add("title", dtValues)
                    txt.EnableViewState = False
                    GetObject = txt
                Else
                    lbl = New Label
                    lbl.ID = "txtc" + Id
                    lbl.Style.Add("word-wrap", "break-word")
                    lbl.Style.Add("white-space", "none")

                    Dim str As String = String.Empty
                    Dim Count As Integer = dtValues.Trim().Length
                    Dim strsplit As Array = dtValues.Trim().Split("##")
                    If strsplit.Length = 0 Then
                        str = dtValues.Trim()
                    Else
                        For i As Integer = 0 To strsplit.Length - 1
                            If strsplit(i) <> "" Then
                                If i = strsplit.Length - 1 Then
                                    str += strsplit(i) + " "
                                Else
                                    str += strsplit(i) + "## "
                                End If
                            End If
                        Next
                    End If

                    lbl.Text = str
                    lbl.EnableViewState = False
                    GetObject = lbl
                End If

            Case "FORMULA"

                If Me.Session(VS_BlankCRF) = "YES" Then
                    txt = New TextBox
                    txt.ID = Id
                    txt.CssClass = "textBox"
                    txt.Text = dtValues
                    txt.Attributes.Add("title", dtValues)
                    txt.EnableViewState = False
                    GetObject = txt
                Else
                    lbl = New Label
                    lbl.ID = "txtf" + Id
                    lbl.Style.Add("word-wrap", "break-word")
                    lbl.Style.Add("white-space", "none")
                    lbl.Text = dtValues.Trim()
                    lbl.EnableViewState = False
                    GetObject = lbl
                End If

            Case "LABEL"
                lbl = New Label
                lbl.ID = "lbl" + Id
                lbl.CssClass = "Label"
                lbl.Style.Add("word-wrap", "break-word")
                lbl.Style.Add("white-space", "none")
                lbl.Text = vMedExValues.Trim()
                lbl.EnableViewState = False
                GetObject = lbl
            Case "CRFTERM"
                txt = New TextBox
                txt.ID = Id
                txt.CssClass = "textBox"
                txt.Text = dtValues
                txt.EnableViewState = False
                GetObject = txt

            Case Else
                Return Nothing
        End Select
    End Function

#End Region

#Region "Button Events"

    Protected Sub BtnGeneratePdf_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGeneratePdf.Click
        Dim htmlcontent As String = String.Empty
        Dim headercontent As String = String.Empty
        Dim downloadbytes As Byte()
        Dim d1 As Document
        Dim data As String = String.Empty
        Dim stylesheetarraylist As New ArrayList
        Dim watermarkTextFont As System.Drawing.Font
        Dim watermarkTextElement As TextElement
        Dim strProfileStatus As String = String.Empty
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_Check As New DataSet
        Dim pdfFont As System.Drawing.Font

        Try
            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D'"

            If Not Me.objHelp.GetCRFLockDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_Check, eStr) Then
                Throw New Exception(eStr)
            End If

            Dim pdfconverter As HtmlToPdfConverter = New HtmlToPdfConverter()
            pdfconverter.PdfDocumentOptions.AvoidImageBreak = True
            pdfconverter.PdfDocumentOptions.AvoidTextBreak = True
            'pdfconverter.TruncateOutOfBoundsText = True
            'pdfconverter.PdfDocumentOptions.StretchToFit = True
            pdfconverter.PdfDocumentOptions.FitWidth = True
            pdfconverter.LicenseKey = "dfvo+uv66OPj6vrr6PTq+unr9Ovo9OPj4+P66g=="
            pdfconverter.PdfDocumentOptions.TopMargin = 20
            'pdfconverter.PdfDocumentOptions.BottomMargin = 20
            pdfconverter.PdfDocumentOptions.LeftMargin = 20
            pdfconverter.PdfHeaderOptions.HeaderHeight = 120
            'pdfconverter.PdfHeaderOptions.HeaderTextFontName = "verdana"
            'pdfconverter.PdfHeaderOptions.DrawHeaderLine = False
            'pdfconverter.PdfFooterOptions.DrawFooterLine = True
            'pdfconverter.PdfFooterOptions.FooterTextFontName = "verdana"
            'pdfconverter.PdfFooterOptions.PageNumberTextFontName = "verdana"
            'pdfconverter.PdfFooterOptions.PageNumberTextFontSize = 7

            pdfconverter.PdfDocumentOptions.JpegCompressionEnabled = False
            pdfconverter.PdfDocumentOptions.JpegCompressionLevel = 0
            pdfconverter.PdfDocumentOptions.ShowHeader = True
            pdfconverter.PdfDocumentOptions.ShowFooter = True

            'pdfconverter.PdfFooterOptions.PageNumberYLocation = 30
            'pdfconverter.PdfFooterOptions.PageNumberTextFontStyle = FontStyle.Bold

            '========================Header=====================================================
            Dim Path As String = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.ToString.LastIndexOf("/"))
            ImgLogo.Src = Path + "/" + ImgLogo.Src
            pdfconverter.PdfDocumentOptions.ShowHeader = True
            headercontent = Regex.Replace(Me.HFHeaderPDF.Value.ToString(), "<IMG[^>]+", "<IMG id=ctl00_CPHLAMBDA_ImgLogo alt=" + ImgLogo.Src.ToString + " align=right src=" + ImgLogo.Src.ToString + " /", RegexOptions.IgnoreCase)
            'headercontent = Me.HFHeaderPDF.Value.ToString() '.Replace("images/lambda_logo.jpg", Server.MapPath("~/images/lambda_logo.jpg"))
            htmlcontent = Me.HFHeaderLabel.Value.ToString()

            Dim htmlHeader As New HtmlToPdfElement(headercontent, Nothing) '21.0, 0.0,
            pdfconverter.PdfHeaderOptions.AddElement(htmlHeader)
            '===================================================================================
            '========================Footer=====================================================
            'pdfconverter.PdfFooterOptions.FooterTextFontName = "Tahoma"
            pdfFont = New System.Drawing.Font("Tahoma", 8, FontStyle.Bold, GraphicsUnit.Point)
            pdfconverter.PdfFooterOptions.AddElement(New TextElement(50, 18, "[Authenticated By:" + CType(Session(VS_AuthenticatedBy), String) + "]  [Authenticated On:" + CType(Session(VS_AuthenticatedOn), String) + "]", PdfFont))
            'pdfconverter.PdfFooterOptions.FooterText = "       *This is an Electronically authenticated report.                                                                                                         Page &p; of &P; pages                   "
            'pdfconverter.PdfFooterOptions.PageNumberingFormatString = "[Authenticated By: " + CType(Session(VS_AuthenticatedBy), String) + "]  [Authenticated On: " + CType(Session(VS_AuthenticatedOn), String) + "]                  "
            pdfconverter.PdfFooterOptions.AddElement(New LineElement(0, 0, pdfconverter.PdfDocumentOptions.PdfPageSize.Width - pdfconverter.PdfDocumentOptions.LeftMargin - pdfconverter.PdfDocumentOptions.RightMargin, 0))
            Dim footerText As New TextElement(0, 5, "*This is an Electronically authenticated report.                                                                                                         Page &p; of &P; pages                   ", New Font(New FontFamily("Tahoma"), 8.5, GraphicsUnit.Point))
            footerText.TextAlign = HorizontalTextAlign.Right
            footerText.ForeColor = Color.Black
            footerText.EmbedSysFont = True
            pdfconverter.PdfFooterOptions.AddElement(footerText)
            pdfconverter.PdfFooterOptions.PageNumberingStartIndex = 0
            '===================================================================================
            BtnGeneratePdf.Enabled = False

            d1 = pdfconverter.ConvertHtmlToPdfDocumentObject(htmlcontent, "")

            'draft copy only when unlock and subject wise edited by vishal

            If Me.ddlSubject.SelectedIndex <> 0 Then

                If ds_Check.Tables(0).Rows.Count = 0 Then
                    'watermarkTextFont = d1.AddFont(Winnovative.WnvHtmlConvert.PdfDocument.StdFontBaseFamily.HelveticaBold)
                    'watermarkTextFont.Size = 75
                    watermarkTextFont = New System.Drawing.Font("HelveticaBold", 75, FontStyle.Bold, GraphicsUnit.Point)
                    watermarkTextElement = New TextElement(100, 400, strProfileStatus + " Draft Copy", watermarkTextFont)
                    watermarkTextElement.ForeColor = System.Drawing.Color.Blue
                    watermarkTextElement.Opacity = 20
                    watermarkTextElement.TextAngle = 45
                    For Each pdfPage In d1.Pages
                        pdfPage.AddElement(watermarkTextElement)
                    Next

                ElseIf ds_Check.Tables(0).Rows.Count > 0 Then

                    'watermarkTextFont = d1.AddFont(Winnovative.WnvHtmlConvert.PdfDocument.StdFontBaseFamily.HelveticaBold)
                    'watermarkTextFont.Size = 75
                    watermarkTextFont = New System.Drawing.Font("HelveticaBold", 75, FontStyle.Bold, GraphicsUnit.Point)
                    watermarkTextElement = New TextElement(100, 400, strProfileStatus + " Draft Copy", watermarkTextFont)
                    watermarkTextElement.ForeColor = System.Drawing.Color.Blue
                    watermarkTextElement.Opacity = 20
                    watermarkTextElement.TextAngle = 45
                    For Each pdfPage In d1.Pages
                        pdfPage.AddElement(watermarkTextElement)
                    Next
                End If
            End If


            downloadbytes = d1.Save()

            Dim response As System.Web.HttpResponse = System.Web.HttpContext.Current.Response
            response.Clear()
            response.ContentType = "application/pdf"
            response.AddHeader("content-disposition", "attachment; filename=pdf1.pdf; size=" & downloadbytes.Length.ToString())
            response.Flush()
            response.BinaryWrite(downloadbytes)
            response.Flush()
            response.End()

        Catch ex As Exception
            Me.objCommon.ShowAlert(ex.ToString(), Me.Page)
        Finally
            ds_Check.Dispose()
        End Try
    End Sub

    Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click
        If Not DisplayHeader() Then
            Exit Sub
        End If
        BtnGeneratePdf.Enabled = True

        If Not Me.GenCall() Then
            Exit Sub
        End If
        Me.HeaderLabel.Style.Add("display", "block")

        Me.BtnGeneratePdf.Style.Add("display", "block")
    End Sub

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.ddlActivtyType.Enabled = True
        Me.BtnGeneratePdf.Enabled = False
        Me.ddlActivtyType.SelectedIndex = 0
        Me.ChkBoxSelectAll.Checked = False
        Me.ddlSubject.Items.Clear()
        Me.ChkListBoxActivity.Items.Clear()
        Me.BtnGeneratePdf.Style.Add("display", "none")
        Me.RBLProjecttype.Items(0).Selected = False
        Me.RBLProjecttype.Items(1).Selected = False
        'If Not Me.fillcheckboxlist() Then
        '    Exit Sub
        'End If

        'If Not Me.FillSubjects() Then
        '    Exit Sub
        'End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.ResetPage()
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub



#End Region

#Region "ResetPage"

    Private Sub ResetPage()
        Me.lblSubjectName.Text = ""
        Me.lblSubjectNo.Text = ""
        Me.txtproject.Text = ""
        Me.HFHeaderLabel.Value = ""
        Me.HFHeaderPDF.Value = "."
        Me.HProjectId.Value = "" '
        Me.ddlSubject.Items.Clear()
        Me.ChkListBoxActivity.Items.Clear()
        Me.ChkBoxSelectAll.Checked = False
        Me.ddlActivtyType.Enabled = False
    End Sub

#End Region

#Region "Fill Functions"


    'Private Function FillSubjectsforGenericActivity() As Boolean
    '    Dim eStr As String = String.Empty
    '    Dim ds_Subjects As New DataSet
    '    Dim wStr As String = String.Empty
    '    Dim dsGenericSubject As DataSet

    '    Try

    '        If ddlActivtyType.SelectedIndex = 1 Then

    '            wStr = "iPeriod=1 and vWorkSpaceID='" + Me.HProjectId.Value.Trim() + "' And cSubjectWiseflag <> 'Y'"
    '            If Not Me.objHelp.GetViewCrfHdrDtlSubDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsGenericSubject, eStr) Then

    '                Throw New Exception(eStr)
    '            End If
    '            Dim dt As New DataTable
    '            dt.Columns.Add("vSubjectid")
    '            dt.AcceptChanges()
    '            Dim dr As DataRow
    '            dr = dt.NewRow()
    '            dr("vSubjectid") = "Select Subject"
    '            dt.Rows.Add(dr)
    '            dt.AcceptChanges()
    '            dr = dt.NewRow()
    '            dr("vSubjectid") = dsGenericSubject.Tables(0).Rows(0)("vSubjectid")
    '            dt.Rows.Add(dr)
    '            dt.AcceptChanges()

    '            Me.ddlSubject.DataSource = dt
    '            Me.ddlSubject.DataTextField = "vSubjectid"
    '            Me.ddlSubject.DataBind()

    '            Return True
    '        End If

    '    Catch ex As Exception
    '        ShowErrorMessage("Error While Filling Subjects. ", ex.Message)
    '        Return False
    '    Finally
    '        dsGenericSubject.Dispose()
    '    End Try


    'End Function

    Private Function FillSubjects() As Boolean
        Dim eStr As String = String.Empty
        Dim ds_Subjects As New DataSet
        Dim wStr As String = String.Empty
        Dim dvData As DataView

        Try
            'added where condition for only 1st Period's subject

            If ddlActivtyType.SelectedIndex = 1 Then
                Me.ddlSubject.Items.Insert(0, "Select Subject")
                Me.ddlSubject.Items.Insert(1, "0000")

            Else


                wStr = "iPeriod=1 and vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D' order by iMysubjectNo"

                If Not Me.objHelp.GetViewWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        ds_Subjects, eStr) Then
                    Throw New Exception(eStr)
                End If

                If Not ds_Subjects.Tables(0) Is Nothing Then

                    'ds_Subjects.Tables(0).Columns.Add("FieldToDisplay", GetType(String))

                    ' ds_Subjects.AcceptChanges()

                    For Each dr As DataRow In ds_Subjects.Tables(0).Rows
                        'dr("FieldToDisplay") = Convert.ToString(dr("vMySubjectNo")).Trim() + " (" + Convert.ToString(dr("FullName")).Trim() + ")"
                        dr("FieldToDisplay") = Convert.ToString(dr("vMySubjectNo")).Trim() + " (" + Convert.ToString(dr("vInitials")).Trim() + ")"
                    Next dr
                    ds_Subjects.AcceptChanges()

                    dvData = ds_Subjects.Tables(0).DefaultView
                    dvData.Sort = "iMysubjectNo"

                    Me.ddlSubject.DataSource = dvData
                    Me.ddlSubject.DataTextField = "FieldToDisplay"
                    Me.ddlSubject.DataValueField = "vSubjectId"
                    Me.ddlSubject.DataBind()

                End If
                Me.ddlSubject.Items.Insert(0, "Select Subject")
                If ddlActivtyType.SelectedIndex = 3 Then
                    Me.ddlSubject.Items.Insert(1, "0000")
                End If
                For count As Integer = 0 To ddlSubject.Items.Count - 1
                    ddlSubject.Items(count).Attributes.Add("title", ddlSubject.Items(count).Text)
                Next

            End If
            Return True

        Catch ex As Exception
            ShowErrorMessage("Error While Filling Subjects. ", ex.Message)
            Return False
        Finally
            ds_Subjects.Dispose()
        End Try

    End Function

#End Region

#Region "DropDown Activity Selected Index Changed"

    Protected Sub ddlActivtyType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlActivtyType.SelectedIndexChanged

        ddlSubject.Enabled = True
        Me.tblactivity.Style.Add("display", "")

        ddlSubject.Items.Clear()
        If Not Me.fillcheckboxlist() Then
            Exit Sub
        End If

        'If ddlActivtyType.SelectedIndex = 1 Then
        '    ddlSubject.Enabled = True
        '    If Not Me.FillSubjectsforGenericActivity() Then
        '        Exit Sub
        '    End If
        'End If

        If ddlActivtyType.SelectedIndex = 2 Or ddlActivtyType.SelectedIndex = 3 Or ddlActivtyType.SelectedIndex = 1 Then
            ddlSubject.Enabled = True
            If Not Me.FillSubjects() Then
                Exit Sub
            End If
        End If

        'If ddlActivtyType.SelectedIndex = 2 Then
        '    ddlSubject.Enabled = True
        '    If Not Me.FillSubjects() Then
        '        Exit Sub
        '    End If
        'End If

        Me.BtnGeneratePdf.Style.Add("display", "none")
        ChkBoxSelectAll.Checked = False

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

End Class