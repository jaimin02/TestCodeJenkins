Imports Newtonsoft.Json
Imports Microsoft.Office.Interop
Imports AjaxControlToolkit
Imports System.Web.Services

Partial Class frmImportLabData
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private ObjCommon As New clsCommon
    Private ObjHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private ObjLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()


    Private Const VS_DtDataTobeImported As String = "DtDataTobeImported"
    Private Const VS_MappingDataForCRF As String = "DTMappingDataForCRF"

    Private Const VS_DtGrid As String = "DtGrid"
    Private Const VS_DtDataFromFile As String = "DtDataFromFile"
    Private Const VS_DtBindSourceField As String = "DtBindSourceField"
    Private Const VS_DtDataExistOrNot As String = "DtDataExistOrNot"


    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_SourceField As Integer = 1
    Private Const GVC_TargetField As Integer = 2

#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            GenCall()
        End If
    End Sub

#End Region

#Region "GenCall() "

    Private Function GenCall() As Boolean
        Dim sender As New Object
        Dim e As New EventArgs
        Dim dt As New DataTable

        Try
            Page.Title = ":: Import Lab Data  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Import Lab Data"

            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtproject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(sender, e)
            End If

            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)

            dt.AcceptChanges()
            Me.ViewState(VS_DtDataTobeImported) = dt.Copy()

            Me.chkSourceColumns.ClearSelection()
            Me.chkSourceColumns.Items.Clear()
            For Each Column As DataColumn In dt.Columns
                Me.chkSourceColumns.Items.Add(Column.Caption.Trim())
            Next


            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
        End Try

    End Function

#End Region

#Region "Fill And Helper Functions"

    Private Function FillddlActivity() As Boolean
        Dim ds_Activity As New Data.DataSet
        Dim dt_Activity As New Data.DataTable
        Dim dv_Activity As New Data.DataView
        Dim estr As String = ""
        Dim dr As DataRow
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            ds_Activity = Nothing
            If Not ObjHelp.Proc_ProjectNodeCommandButtonRights(Me.HProjectId.Value.Trim(), Me.Session(S_UserID), _
                                    "", "NO", ds_Activity, estr) Then

                Me.ObjCommon.ShowAlert("Error While Getting Data from Proc_ProjectNodeCommandButtonRights:" + estr, Me.Page)
                Return False
            End If

            ds_Activity.Tables(0).Columns.Add("DataValueField")
            ds_Activity.AcceptChanges()

            For Each dr In ds_Activity.Tables(0).Rows
                dr("DataValueField") = Convert.ToString(dr("iNodeId")).Trim() + "#" + Convert.ToString(dr("iNodeIndex")).Trim() + "#" + Convert.ToString(dr("vActivityId")).Trim()
            Next dr
            ds_Activity.AcceptChanges()

            dv_Activity = ds_Activity.Tables(0).DefaultView
            dt_Activity = dv_Activity.ToTable()

            Dim dv_SubGroup As DataView = New DataView(dt_Activity)
            dv_SubGroup.RowFilter = "iParentNodeId <> 1 AND iParentNodeId <> 0 "
            dt_Activity = dv_SubGroup.ToTable


            Me.ddlActivity.DataSource = dt_Activity.DefaultView.ToTable(True, "DataValueField,vNodeDisplayName".Split(","))
            Me.ddlActivity.DataValueField = "DataValueField"
            Me.ddlActivity.DataTextField = "vNodeDisplayName"
            Me.ddlActivity.DataBind()
            Me.ddlActivity.Items.Insert(0, New ListItem("--Select Activity--", 0))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function

    Private Function FillMappingGrid() As Boolean

        Dim dt_Grid As DataTable
        Dim dr As DataRow
        Dim item As ListItem
        Dim index As Integer
        Try

            If Me.ViewState(VS_DtGrid) Is Nothing Then
                Me.ObjCommon.ShowAlert("Please import csv file !", Me.Page)
                Exit Function
            End If

            dt_Grid = CType(Me.ViewState(VS_DtGrid), DataTable).Copy()
            dr = dt_Grid.NewRow()
            dr(GVC_SrNo) = dt_Grid.Rows.Count + 1

            For Each item In Me.chkSourceColumns.Items
                If item.Selected Then
                    dr(GVC_SourceField) = item.Text.Trim()
                    Me.chkSourceColumns.ClearSelection()
                    Exit For
                End If
            Next

            If Convert.ToString(dr(GVC_SourceField)).Trim() = "" Then
                Me.ObjCommon.ShowAlert("Please select Source field", Me.Page)
                Exit Function
            End If
            'Checking for Duplication

            For index = 0 To dt_Grid.Rows.Count - 1
                If Convert.ToString(dr(GVC_SourceField)).Trim.ToUpper() = Convert.ToString(dt_Grid.Rows(index)(GVC_SourceField)).Trim.ToUpper() Then
                    Me.ObjCommon.ShowAlert("Source field is already mapped, Please select other field", Me.Page)
                    Exit Function
                End If
            Next index

            '*************************

            For Each item In Me.chkTargetColumns.Items
                If item.Selected Then
                    If MappingIsProper(Convert.ToString(dr(GVC_SourceField)).Trim.ToUpper(), item.Text.Trim().ToUpper()) Then
                        dr(GVC_TargetField) = item.Text.Trim()
                        Me.chkTargetColumns.ClearSelection()
                    Else
                        Me.ObjCommon.ShowAlert("Please Select Proper Target Field !", Me.Page)
                        Me.chkTargetColumns.ClearSelection()
                        Exit Function
                    End If
                    Exit For
                End If
            Next

            If Convert.ToString(dr(GVC_TargetField)).Trim() = "" Then
                Me.ObjCommon.ShowAlert("Please select Target field", Me.Page)
                Exit Function
            End If

            dt_Grid.Rows.Add(dr)
            dt_Grid.AcceptChanges()

            Me.ViewState(VS_DtGrid) = dt_Grid

            Me.GVWMapping.DataSource = Nothing
            Me.GVWMapping.DataSource = dt_Grid
            Me.GVWMapping.DataBind()

            FillMappingGrid = True

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Filling Mapping Grid.", ex.Message)
        End Try

    End Function

    Private Function MappingIsProper(ByVal SourceField As String, ByRef TargetFied As String) As Boolean

        If SourceField <> TargetFied Then
            If SourceField = "STUDYID" Then
                Return False
            End If
            If SourceField = "SCRNNUM" Then
                Return False
            End If
            If SourceField = "USUBJID" And TargetFied <> "RANDOMIZATIONNO" Then
                Return False
            End If
            If SourceField = "VISIT" Then
                Return False
            End If
        End If

        Return True

    End Function

    Private Function GetColumnsFromFile(ByVal Path As String, ByRef FileUploader As FileUpload) As Boolean

        Dim sRead As System.IO.StreamReader
        Dim sRow As String
        Dim sColArr() As String
        Dim NewPath As String = String.Empty
        Dim IndCol As Integer
        Dim dr As Data.DataRow
        Dim dt_DataFromFile As New DataTable

        Try

            NewPath = ConfigurationManager.AppSettings.Item("TempSubjectProofDetails").Trim()
            NewPath += "LabData.csv" 'Path.Substring(Path.LastIndexOf("\") + 1)

            FileUploader.SaveAs(Server.MapPath(NewPath))

            'If File.Exists(Server.MapPath(NewPath)) Then
            '    File.Delete(Server.MapPath(NewPath))
            'End If
            'File.Copy(Path, Server.MapPath(NewPath))

            sRead = New System.IO.StreamReader(Server.MapPath(NewPath))
            sRow = sRead.ReadLine()

            'sColArr = sRow.Split(vbTab)
            'changed by vishal for Target Fields Fill
            sColArr = sRow.Split(",")
            Me.chkTargetColumns.Items.Clear()
            For index As Integer = 0 To sColArr.Length - 1
                Me.chkTargetColumns.Items.Add(sColArr(index))
                dt_DataFromFile.Columns.Add(sColArr(index))
                dt_DataFromFile.AcceptChanges()
            Next index

            Do While sRead.Peek() >= 0

                sRow = sRead.ReadLine()
                'sColArr = sRow.Split(vbTab)
                sColArr = sRow.Split(",")

                dr = dt_DataFromFile.NewRow

                For IndCol = 0 To sColArr.Length - 1

                    dr(IndCol) = sColArr(IndCol)

                Next IndCol

                dt_DataFromFile.Rows.Add(dr)

            Loop

            Me.ViewState(VS_DtDataFromFile) = dt_DataFromFile.Copy()

            GetColumnsFromFile = True

        Catch ex As Exception
            Me.ShowErrorMessage("Error occured while retrieving information from tab delimated file.", ex.Message)
        Finally

            If Not sRead Is Nothing Then
                sRead.Close()
                sRead.Dispose()
            End If
            sRead = Nothing
            'File.Delete(Server.MapPath(NewPath))
        End Try

    End Function

    Private Function GetDataFromFile(ByVal Path As String, _
                                         ByRef ds_TargetUpload As Data.DataSet) As Boolean

        Dim sRead As System.IO.StreamReader
        Dim sRow As String
        Dim sColArr() As String
        Dim IndCol As Integer
        Dim dr As Data.DataRow
        Dim NewPath As String = String.Empty

        Try



            NewPath = ConfigurationManager.AppSettings.Item("TempSubjectProofDetails").Trim()
            NewPath += Path.Substring(Path.LastIndexOf("\") + 1)

            If File.Exists(Server.MapPath(NewPath)) Then
                File.Delete(Server.MapPath(NewPath))
            End If
            File.Copy(Path, Server.MapPath(NewPath))

            sRead = New System.IO.StreamReader(Server.MapPath(NewPath))
            sRow = sRead.ReadLine()

            'If sRow.Split(vbTab).Length <> ValidColumnLength_1 Then
            '    Throw New Exception("Invalid column or mismatch column information in " + TableName1)
            'End If

            Do While sRead.Peek() >= 0

                sRow = sRead.ReadLine()
                'sColArr = sRow.Split(vbTab)
                sColArr = sRow.Split(",")

                dr = ds_TargetUpload.Tables(0).NewRow

                For IndCol = 0 To sColArr.Length - 1

                    dr(IndCol) = sColArr(IndCol)

                Next IndCol

                ds_TargetUpload.Tables(0).Rows.Add(dr)

            Loop

            GetDataFromFile = True

        Catch ex As Exception
            Me.ShowErrorMessage("Error occured while retrieving information from tab delimated file.", ex.Message)
        Finally

            File.Delete(Server.MapPath(NewPath))
            If Not sRead Is Nothing Then
                sRead.Close()
                sRead.Dispose()
            End If

            sRead = Nothing
        End Try

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

#Region "Button Events"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_Check As New DataSet
        Dim dv_Check As New DataView
        Dim ds_Subjects As New DataSet
        Dim ds_CreatedBy As New DataSet
        Dim dv_CreatedBy As New DataView

        Try

            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D'"

            If Not Me.ObjHelp.GetCRFLockDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_Check, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds_Check Is Nothing Then

                dv_Check = ds_Check.Tables(0).DefaultView
                dv_Check.Sort = "iTranNo desc"
                ' edited by vishal for lock/unlock site
                If dv_Check.ToTable().Rows.Count > 0 Then

                    If Convert.ToString(dv_Check.ToTable().Rows(0)("cLockFlag")).Trim.ToUpper() = "L" Then
                        Me.ObjCommon.ShowAlert("Project/Site is Locked.", Me.Page)
                        Me.txtproject.Text = ""
                        Me.HProjectId.Value = ""
                        Exit Sub

                    End If

                End If

            End If

            If Not Me.FillddlActivity() Then
                Exit Sub
            End If
        Catch ex As Exception
            Me.ShowErrorMessage("Error While Getting CRF Lock Details ", ex.Message)
        End Try
    End Sub

    Protected Sub btnImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImport.Click
        Dim ds_Save As New DataSet
        Dim ds_CRFHdr As New DataSet
        Dim ds_CRFDtl As New DataSet
        Dim ds_CRFSubDtl As New DataSet
        Dim dr_CRFSubDtl As DataRow
        Dim dt_DataFromFile As New DataTable
        Dim eStr As String = String.Empty
        Dim dr As DataRow
        Dim dt_Grid As New DataTable
        Dim dt_DataTobeImported As New DataTable
        Dim dv_DataTobeImported As New DataView
        Dim drNew As DataRow
        Dim Previous_ScreenNo As String = ""
        Dim dr_CRFDtl As DataRow

        Dim ds_Subjects As New DataSet
        Dim dv_Subjects As New DataView
        Dim wStr As String = String.Empty
        Dim ds_Template As New DataSet
        Dim dv_Template As New DataView

        Dim Success_Entries As String = ""
        Dim dt_AttributeName As DataTable = New DataTable()
        Dim dt_MergeHDR_DTL_SUBDTL As DataTable = New DataTable()

        Dim strColName As String = String.Empty

        Dim strSubject As String = String.Empty

        Dim ds_WorkSpaceUserNodeDetail As DataSet = New DataSet()
        Try

           

            If Me.ddlActivity.SelectedIndex = 0 Then
                Me.ObjCommon.ShowAlert("Please select an Activity", Me.Page)
                Exit Sub
            End If

            wStr = " vWorkSpaceId = '" + Me.HProjectId.Value.ToString() + "' AND iNodeId = '" + Me.ddlActivity.SelectedItem.Value.Substring(0, Me.ddlActivity.SelectedItem.Value.IndexOf("#")) + "' AND vActivityId = " + Me.ddlActivity.SelectedItem.Value.Substring(Me.ddlActivity.SelectedItem.Value.LastIndexOf("#") + 1) + " AND iNodeIndex=" + Me.ddlActivity.SelectedItem.Value.Substring(Me.ddlActivity.SelectedItem.Value.IndexOf("#") + 1, Me.ddlActivity.SelectedItem.Value.LastIndexOf("#") - (Me.ddlActivity.SelectedItem.Value.IndexOf("#") + 1)) + " "
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                If Not Me.ObjHelp.GetViewWorkSpaceUserNodeDetail(wStr, ds_WorkSpaceUserNodeDetail, eStr) Then
                    Throw New Exception(eStr)
                End If
            Else
                If Not Me.ObjHelp.GetViewWorkSpaceNodeDetail(wStr, ds_WorkSpaceUserNodeDetail, eStr) Then
                    Throw New Exception(eStr)
                End If
            End If

            If (ds_WorkSpaceUserNodeDetail.Tables(0).Rows(0)("cIsRepeatable").ToString() = "Y") Then
                ImportRepeatableData()
                Exit Sub
            End If

            If Me.ViewState(VS_DtDataFromFile) Is Nothing Then
                Me.ObjCommon.ShowAlert("Please mapped field.! " + eStr, Me.Page)
                Exit Sub
            End If

            dt_DataFromFile = CType(Me.ViewState(VS_DtDataFromFile), DataTable).Copy()
            dt_Grid = CType(Me.ViewState(VS_DtGrid), DataTable).Copy()
            dt_DataTobeImported = CType(Me.ViewState(VS_DtDataTobeImported), DataTable).Copy()
            dt_AttributeName = CType(Me.ViewState(VS_MappingDataForCRF), DataTable).Copy()

            dt_MergeHDR_DTL_SUBDTL.Columns.Add("STUDYID")
            dt_MergeHDR_DTL_SUBDTL.Columns.Add("SCRNNUM")
            dt_MergeHDR_DTL_SUBDTL.Columns.Add("USUBJID")
            dt_MergeHDR_DTL_SUBDTL.Columns.Add("VISIT")

            dt_MergeHDR_DTL_SUBDTL.Columns.Add("vCDISCValues")
            dt_MergeHDR_DTL_SUBDTL.Columns.Add("vMedExSubGroupdesc")
            dt_MergeHDR_DTL_SUBDTL.Columns.Add("vMEdExCode")
            dt_MergeHDR_DTL_SUBDTL.Columns.Add("vMedExDesc")
            dt_MergeHDR_DTL_SUBDTL.Columns.Add("vMedExResult")

            dt_MergeHDR_DTL_SUBDTL.Columns.Add("vHighRange")
            dt_MergeHDR_DTL_SUBDTL.Columns.Add("vLowRange")
            dt_MergeHDR_DTL_SUBDTL.Columns.Add("cHL")
            dt_MergeHDR_DTL_SUBDTL.Columns.Add("vMappedCol")


            dt_MergeHDR_DTL_SUBDTL.Columns.Add("iNodeId")
            dt_MergeHDR_DTL_SUBDTL.Columns.Add("iNodeIndex")


            For Each dr In dt_DataFromFile.Rows
                drNew = dt_DataTobeImported.NewRow()
                For Each dr_Grid As DataRow In dt_Grid.Rows
                    drNew(Convert.ToString(dr_Grid(GVC_SourceField))) = dr(dr_Grid(GVC_TargetField).ToString())
                Next dr_Grid
                dt_DataTobeImported.Rows.Add(drNew)
                dt_DataTobeImported.AcceptChanges()

            Next dr

            strColName = IIf(dt_Grid.Select("[Source field]='STUDYID'").Length = 0, "STUDYID,", "")
            strColName += IIf(dt_Grid.Select("[Source field]='SCRNNUM'").Length = 0, "SCRNNUM,", "")
            strColName += IIf(dt_Grid.Select("[Source field]='USUBJID'").Length = 0, "USUBJID,", "")
            strColName += IIf(dt_Grid.Select("[Source field]='VISIT'").Length = 0, "VISIT,", "")

            If strColName <> "" Then
                Me.ObjCommon.ShowAlert(strColName.Substring(0, strColName.Length - 1) + " Field Must be Mapped With CSV Import Fields . ", Me.Page)
                Exit Sub
            End If

            dv_DataTobeImported = dt_DataTobeImported.DefaultView
            For Each dr_Grid1 As DataRow In dt_Grid.Rows
                If (Convert.ToString(dr_Grid1(GVC_SourceField)).ToUpper = "STUDYID" Or Convert.ToString(dr_Grid1(GVC_SourceField)).ToUpper = "SCRNNUM" Or Convert.ToString(dr_Grid1(GVC_SourceField)).ToUpper = "USUBJID" Or Convert.ToString(dr_Grid1(GVC_SourceField)).ToUpper() = "VISIT") Then
                    dr_Grid1.Delete()
                End If
            Next
            dt_Grid.AcceptChanges()

            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D'"
            If Not Me.ObjHelp.View_WorkSpaceSubjectRegistration(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition _
                                , ds_Subjects, eStr) Then
                Throw New Exception(eStr)
            End If

            wStr += " And vActivityId = '" + Me.ddlActivity.SelectedItem.Value.Substring(Me.ddlActivity.SelectedItem.Value.LastIndexOf("#") + 1) + "' And cActiveFlag = 'Y' "
            If Not Me.ObjHelp.GetVIEWMedExWorkspaceHdr(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition _
                                            , ds_Template, eStr) Then
                Throw New Exception(eStr)
            End If

            Dim dv_SubJect As DataView = New DataView(dt_DataFromFile)
            Dim dt_Subject As DataTable = dv_SubJect.ToTable(True, "SCRNNUM")

            Dim ds_WorkSpaceProtocolDetail As DataSet = New DataSet()
            ds_WorkSpaceProtocolDetail = ObjHelp.GetResultSet("Select * from WorkSpaceProtocolDetail  where vWorkspaceId='" + Me.HProjectId.Value.ToString() + "' AND cStatusIndi <> 'D'  ", "WorkSpaceProtocolDetail")

            For index_Subject As Integer = 0 To dt_Subject.Rows.Count - 1
                Dim dv_SubGroup As DataView = New DataView(dt_DataFromFile)
                dv_SubJect.RowFilter = "SCRNNUM = '" & dt_Subject.Rows(index_Subject)("SCRNNUM").ToString() & "' AND STUDYID = '" & ds_WorkSpaceProtocolDetail.Tables(0).Rows(0)("vProjectNo").ToString() & "' "
                Dim dt_SubGroup As DataTable = dv_SubJect.ToTable
                Dim dv_SubGroupWiseMapping As DataView = New DataView(dt_DataTobeImported)
                dv_SubGroupWiseMapping.RowFilter = "SCRNNUM = '" & dt_Subject.Rows(index_Subject)("SCRNNUM").ToString() & "' "
                Dim dt_SubGroupWiseMapping As DataTable = dv_SubGroupWiseMapping.ToTable

                Dim dv_SubjectId As DataView = New DataView(ds_Subjects.Tables(0))
                dv_SubjectId.RowFilter = "vMySubjectNo = '" & dt_Subject.Rows(index_Subject)("SCRNNUM").ToString() & "' "

                If (dv_SubjectId.ToTable().Rows.Count = 0) Then
                    strSubject += dt_Subject.Rows(index_Subject)("SCRNNUM").ToString() + ","
                    Continue For
                End If

                For index_SubGroup As Integer = 0 To dt_SubGroup.Rows.Count - 1
                    'For index1 As Integer = 0 To dt_Grid.Rows.Count - 1

                    For Each dr_Template In ds_Template.Tables(0).Rows
                        Dim row_Mapped As DataRow = dt_Grid.Select("[Source field]='" + Convert.ToString(dr_Template("vCDISCValues")).Trim().ToUpper() + "-" + Convert.ToString(dr_Template("vMedExDesc")).Trim().ToUpper() + "'").FirstOrDefault()
                        If Not row_Mapped Is Nothing Then
                            Dim searchedValue As String = row_Mapped.Item("target field")
                        End If
                        If ((Not row_Mapped Is Nothing) And _
                            Convert.ToString(dr_Template("vmedexsubGroupDesc")).Trim().ToUpper() = Convert.ToString(dt_SubGroup.Rows(index_SubGroup)("LBTEST")).Trim().ToUpper() _
                            And Convert.ToString(dr_Template("ParentVisit")).Trim().ToUpper() = Convert.ToString(dt_SubGroup.Rows(index_SubGroup)("VISIT")).Trim().ToUpper()) Then

                            drNew = dt_MergeHDR_DTL_SUBDTL.NewRow()

                            drNew("vMedExDesc") = Convert.ToString(dr_Template("vMedExDesc"))
                            drNew("vMEdExCode") = Convert.ToString(dr_Template("vMEdExCode"))
                            drNew("iNodeId") = Convert.ToString(dr_Template("iNodeId"))
                            drNew("iNodeIndex") = Convert.ToString(dr_Template("iNodeIndex"))

                            drNew("STUDYID") = dt_SubGroupWiseMapping.Rows(index_SubGroup)("STUDYID")
                            drNew("SCRNNUM") = Convert.ToString(dv_SubjectId.ToTable().Rows(0)("vSubjectId")).Trim()
                            drNew("USUBJID") = dt_SubGroupWiseMapping.Rows(index_SubGroup)("USUBJID")
                            drNew("VISIT") = dt_SubGroupWiseMapping.Rows(index_SubGroup)("VISIT")

                            drNew("vCDISCValues") = Convert.ToString(dr_Template("vCDISCValues")).Trim().ToUpper()
                            drNew("vMedExResult") = dt_SubGroupWiseMapping.Rows(index_SubGroup)(Convert.ToString(dr_Template("vCDISCValues")).Trim().ToUpper() + "-" + Convert.ToString(dr_Template("vMedExDesc")).Trim().ToUpper())

                            ''drNew("vHighRange") = dt_SubGroup.Rows(index_SubGroup)("RPTNRHI")
                            ''drNew("vLowRange") = dt_SubGroup.Rows(index_SubGroup)("RPTNRLO")
                            ''drNew("cHL") = dt_SubGroup.Rows(index_SubGroup)("ALRTFL")
                            drNew("vMedExSubGroupdesc") = dt_SubGroup.Rows(index_SubGroup)("LBTEST")
                            drNew("vMappedCol") = Convert.ToString(row_Mapped.Item("Target field")).Trim().ToUpper()

                            dt_MergeHDR_DTL_SUBDTL.Rows.Add(drNew)
                            dt_MergeHDR_DTL_SUBDTL.AcceptChanges()
                            dt_MergeHDR_DTL_SUBDTL.TableName = "dt_MergeHDR_DTL_SUBDTL"

                        ElseIf (Convert.ToString(dr_Template("vmedexsubGroupDesc")).Trim().ToUpper() = Convert.ToString(dt_SubGroup.Rows(index_SubGroup)("LBTEST")).Trim().ToUpper() _
                            And Convert.ToString(dr_Template("ParentVisit")).Trim().ToUpper() = Convert.ToString(dt_SubGroup.Rows(index_SubGroup)("VISIT")).Trim().ToUpper()) Then

                            drNew = dt_MergeHDR_DTL_SUBDTL.NewRow()
                            drNew("vMedExDesc") = Convert.ToString(dr_Template("vMedExDesc"))
                            drNew("vMEdExCode") = Convert.ToString(dr_Template("vMEdExCode"))

                            drNew("iNodeId") = Convert.ToString(dr_Template("iNodeId"))
                            drNew("iNodeIndex") = Convert.ToString(dr_Template("iNodeIndex"))

                            drNew("STUDYID") = Convert.ToString(dt_SubGroupWiseMapping.Rows(index_SubGroup)("STUDYID"))

                            drNew("SCRNNUM") = Convert.ToString(dv_SubjectId.ToTable().Rows(0)("vSubjectId"))
                            drNew("USUBJID") = Convert.ToString(dv_SubjectId.ToTable().Rows(0)("vRandomizationNo"))
                            drNew("VISIT") = Convert.ToString(dt_SubGroupWiseMapping.Rows(index_SubGroup)("VISIT"))

                            drNew("vCDISCValues") = Convert.ToString(dr_Template("vCDISCValues"))
                            ''drNew("vHighRange") = Convert.ToString(dr_Template("vHighRange"))
                            ''drNew("vLowRange") = Convert.ToString(dr_Template("vLowRange"))
                            ''drNew("cHL") = ""
                            drNew("vMedExSubGroupdesc") = Convert.ToString(dr_Template("vMedExSubGroupdesc"))

                            dt_MergeHDR_DTL_SUBDTL.Rows.Add(drNew)
                            dt_MergeHDR_DTL_SUBDTL.AcceptChanges()
                            dt_MergeHDR_DTL_SUBDTL.TableName = "dt_MergeHDR_DTL_SUBDTL"

                        End If

                    Next
                Next
            Next
            'Next

            If Not String.IsNullOrEmpty(strSubject).ToString().Length <> 0 AndAlso String.IsNullOrEmpty(strSubject.Substring(0, strSubject.Length - 1)) Then
                Me.ObjCommon.ShowAlert("No Subject Found In The Specified File.", Me.Page)
            End If
            If dt_MergeHDR_DTL_SUBDTL.Rows.Count <= 0 Then
                Me.ObjCommon.ShowAlert("No Appropriate Data Found In CSV File !", Me.Page)
                Exit Sub
            End If

            wStr = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "'  And "
            wStr += "iNodeId in (" + Me.ddlActivity.SelectedItem.Value.Substring(0, Me.ddlActivity.SelectedItem.Value.IndexOf("#")) + ") AND "
            wStr += "vActivityId in ('" + Me.ddlActivity.SelectedItem.Value.Substring(Me.ddlActivity.SelectedItem.Value.LastIndexOf("#") + 1) + "')"

            If Not Me.ObjHelp.GetCRFHdr(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                               ds_CRFHdr, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_CRFHdr Is Nothing Then
                Throw New Exception("CRFHdr No. Not found.")
            End If

            If ds_CRFHdr.Tables(0).Rows.Count = 0 Then
                dr = ds_CRFHdr.Tables(0).NewRow()
                dr("nCRFHdrNo") = 0
                dr("vWorkSpaceId") = Me.HProjectId.Value.Trim()
                dr("dStartDate") = Date.Now.ToString()
                dr("iPeriod") = 1
                dr("iNodeId") = Me.ddlActivity.SelectedItem.Value.Substring(0, Me.ddlActivity.SelectedItem.Value.IndexOf("#"))
                dr("iNodeIndex") = Me.ddlActivity.SelectedItem.Value.Substring(Me.ddlActivity.SelectedItem.Value.IndexOf("#") + 1, Me.ddlActivity.SelectedItem.Value.LastIndexOf("#") - (Me.ddlActivity.SelectedItem.Value.IndexOf("#") + 1))
                dr("vActivityId") = Me.ddlActivity.SelectedItem.Value.Substring(Me.ddlActivity.SelectedItem.Value.LastIndexOf("#") + 1)
                dr("cLockStatus") = "U"
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dr("dModifyon") = DateTime.Now()
                ds_CRFHdr.Tables(0).Rows.Add(dr)
                ds_CRFHdr.AcceptChanges()
            Else
                If (Me.txtRemarks.Text.Trim() = "") Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ModalPopupOpen", "ModalPopupOpen(' Import CSV data already been uploaded !! Whould you like to upload again ?? '); ", True)
                    Me.lblAlertStr.Text = "Import CSV data already been uploaded !! Whould you like to upload again ??"
                    Exit Sub
                End If
            End If

            If Not Me.ObjHelp.GetCRFDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                             ds_CRFDtl, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not Me.ObjHelp.GetCRFSubDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                                ds_CRFSubDtl, eStr) Then
                Throw New Exception(eStr)
            End If

            dv_DataTobeImported = dt_MergeHDR_DTL_SUBDTL.DefaultView
            dv_DataTobeImported.Sort = "SCRNNUM"
            dt_DataTobeImported = Nothing
            dt_DataTobeImported = New DataTable
            dt_DataTobeImported = dv_DataTobeImported.ToTable()

            For index As Integer = 0 To dt_DataTobeImported.Rows.Count - 1

                If Previous_ScreenNo.Trim() <> "" AndAlso _
                        Previous_ScreenNo.ToUpper() <> Convert.ToString(dt_DataTobeImported.Rows(index)("SCRNNUM")).Trim.ToUpper() + Convert.ToString(dt_DataTobeImported.Rows(index)("iNodeId")).Trim.ToUpper() Then

                    If ds_CRFDtl.Tables(0).Rows.Count > 0 Then

                        Success_Entries += Previous_ScreenNo.Trim() + ","
                        ds_Save.Tables.Add(ds_CRFHdr.Tables(0).Copy())
                        ds_Save.Tables(0).TableName = "CRFHdr"
                        ds_Save.AcceptChanges()

                        ds_Save.Tables.Add(ds_CRFDtl.Tables(0).Copy())
                        ds_Save.Tables(1).TableName = "CRFDtl"
                        ds_Save.AcceptChanges()

                        ds_Save.Tables.Add(ds_CRFSubDtl.Tables(0).Copy())
                        ds_Save.Tables(2).TableName = "CRFSubDtl"
                        ds_Save.AcceptChanges()

                        If ds_CRFDtl.Tables(0).Select("[nCRFDTLNO]='1'").Length = 1 Then
                            If Not Me.ObjLambda.Save_CRFHdrDtlSubDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                                            ds_Save, False, Me.Session(S_UserID), eStr) Then
                                Throw New Exception(eStr)
                            End If
                        Else
                            If Not Me.ObjLambda.Save_CRFSubDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                                     ds_CRFSubDtl, Me.Session(S_UserID), eStr) Then
                                Throw New Exception(eStr)
                            End If
                        End If
                    End If
                    ds_CRFDtl.Tables(0).Rows.Clear()
                    ds_CRFDtl.AcceptChanges()
                    ds_CRFSubDtl.Tables(0).Rows.Clear()
                    ds_CRFSubDtl.AcceptChanges()
                    ds_Save = Nothing
                    ds_Save = New DataSet

                End If
                dv_Template = dt_DataTobeImported.DefaultView
                dv_Template.RowFilter = "SCRNNUM = '" + dt_DataTobeImported.Rows(index)("SCRNNUM") + "'"
                If dv_Template.ToTable().Rows.Count < 1 Then
                    Previous_ScreenNo = Convert.ToString(dt_DataTobeImported.Rows(index)("SCRNNUM")).Trim()
                    Continue For
                End If
                dv_Subjects = dt_DataTobeImported.DefaultView
                dv_Subjects.RowFilter = "USUBJID = '" + Convert.ToString(dt_DataTobeImported.Rows(index)("USUBJID")).Trim() + "'"

                If Previous_ScreenNo.ToUpper() <> Convert.ToString(dt_DataTobeImported.Rows(index)("SCRNNUM")).Trim.ToUpper() + Convert.ToString(dt_DataTobeImported.Rows(index)("iNodeId")).Trim.ToUpper() Then
                    wStr = "nCRFHdrNo = " + Convert.ToString(ds_CRFHdr.Tables(0).Rows(0)("nCRFHdrNo")) + "  And vSubjectId = '" + Convert.ToString(dv_Subjects.ToTable().Rows(0)("SCRNNUM")).Trim() + _
                   "'  And cStatusIndi <> 'D'"

                    If Not Me.ObjHelp.GetCRFDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                ds_CRFDtl, eStr) Then
                        Throw New Exception("Error While Getting Data From CRFDtl. " + eStr)
                    End If
                    If ds_CRFDtl Is Nothing Then
                        Throw New Exception("CRFDtl No. Not found.")
                    End If
                End If

                dr_CRFSubDtl = ds_CRFSubDtl.Tables(0).NewRow()
                dr_CRFSubDtl("nCRFSubDtlNo") = index + 1

                If ds_CRFDtl.Tables(0).Rows.Count = 0 Then
                    dr_CRFSubDtl("nCRFDtlNo") = 1
                Else
                    dr_CRFSubDtl("nCRFDtlNo") = ds_CRFDtl.Tables(0).Rows(0)("nCRFDtlNo")
                End If
                dr_CRFSubDtl("iTranNo") = 1
                dr_CRFSubDtl("vMedExCode") = dt_DataTobeImported.Rows(index)("vMedExCode")
                dr_CRFSubDtl("vMedExDesc") = dt_DataTobeImported.Rows(index)("vMedExDesc")
                dr_CRFSubDtl("dMedExDatetime") = CDate(ObjCommon.GetCurDatetimeWithOffSet(Me.Session(S_TimeZoneName)).DateTime)
                dr_CRFSubDtl("vMedExResult") = dt_DataTobeImported.Rows(index)("vMedExResult")
                dr_CRFSubDtl("iModifyBy") = Me.Session(S_UserID)
                dr_CRFSubDtl("cHL") = dt_DataTobeImported.Rows(index)("cHL")
                dr_CRFSubDtl("vHighRange") = dt_DataTobeImported.Rows(index)("vHighRange")
                dr_CRFSubDtl("vLowRange") = dt_DataTobeImported.Rows(index)("vLowRange")
                dr_CRFSubDtl("vMappedCol") = dt_DataTobeImported.Rows(index)("vMappedCol")
                dr_CRFSubDtl("dModifyOn") = DateTime.Now()
                dr_CRFSubDtl("vModificationRemark") = txtRemarks.Text().Trim()
                ds_CRFSubDtl.Tables(0).Rows.Add(dr_CRFSubDtl)
                ds_CRFSubDtl.AcceptChanges()

                If Previous_ScreenNo.ToUpper() = Convert.ToString(dt_DataTobeImported.Rows(index)("SCRNNUM")).Trim.ToUpper() + Convert.ToString(dt_DataTobeImported.Rows(index)("iNodeId")).Trim.ToUpper() Then
                    Continue For
                End If

                If ds_CRFDtl.Tables(0).Rows.Count = 0 Then
                    If dv_Subjects.ToTable().Rows.Count > 0 Then
                        dr_CRFDtl = ds_CRFDtl.Tables(0).NewRow()
                        dr_CRFDtl("nCRFDtlNo") = 1
                        dr_CRFDtl("nCRFHdrNo") = 1
                        dr_CRFDtl("iRepeatNo") = 1
                        dr_CRFDtl("dRepeatationDate") = CDate(ObjCommon.GetCurDatetimeWithOffSet(Me.Session(S_TimeZoneName)).DateTime)
                        dr_CRFDtl("vSubjectId") = Convert.ToString(dv_Subjects.ToTable().Rows(0)("SCRNNUM")).Trim()
                        dr_CRFDtl("iMySubjectNo") = Convert.ToString(dv_Subjects.ToTable().Rows(0)("USUBJID")).Trim()
                        dr_CRFDtl("cLockStatus") = "U"
                        dr_CRFDtl("iWorkFlowstageId") = Me.Session(S_WorkFlowStageId)
                        dr_CRFDtl("iModifyBy") = Me.Session(S_UserID)
                        dr_CRFDtl("cStatusIndi") = "N"
                        dr_CRFDtl("cDataStatus") = CRF_DataEntryCompleted
                        dr_CRFDtl("dModifyon") = DateTime.Now()

                        ds_CRFDtl.Tables(0).Rows.Add(dr_CRFDtl)
                        ds_CRFDtl.AcceptChanges()

                    End If
                End If
                Previous_ScreenNo = Convert.ToString(dt_DataTobeImported.Rows(index)("SCRNNUM")).Trim().ToUpper() + Convert.ToString(dt_DataTobeImported.Rows(index)("iNodeId")).Trim().ToUpper()

                ds_CRFHdr.Tables(0).Rows(0)("iNodeId") = Convert.ToString(dt_DataTobeImported.Rows(index)("iNodeId"))
                ds_CRFHdr.Tables(0).Rows(0)("iNodeIndex") = Convert.ToString(dt_DataTobeImported.Rows(index)("iNodeIndex"))
            Next index

            If ds_CRFDtl.Tables(0).Rows.Count > 0 Then
                Success_Entries += Previous_ScreenNo.Trim() + ","

                ds_Save.Tables.Add(ds_CRFHdr.Tables(0).Copy())
                ds_Save.Tables(0).TableName = "CRFHdr"
                ds_Save.AcceptChanges()

                ds_Save.Tables.Add(ds_CRFDtl.Tables(0).Copy())
                ds_Save.Tables(1).TableName = "CRFDtl"
                ds_Save.AcceptChanges()

                ds_Save.Tables.Add(ds_CRFSubDtl.Tables(0).Copy())
                ds_Save.Tables(2).TableName = "CRFSubDtl"
                ds_Save.AcceptChanges()

                If ds_CRFDtl.Tables(0).Select("[nCRFDTLNO]='1'").Length = 1 Then
                    If Not Me.ObjLambda.Save_CRFHdrDtlSubDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                                ds_Save, False, Me.Session(S_UserID), eStr) Then
                        Me.ObjCommon.ShowAlert("There Are Some Fields, Which Are Not Matching In File. " + eStr, Me.Page)
                        Exit Sub
                    End If
                Else
                    If Not Me.ObjLambda.Save_CRFSubDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                                    ds_CRFSubDtl, Me.Session(S_UserID), eStr) Then
                        Throw New Exception(eStr)
                    End If
                End If
            End If

            If Success_Entries.Trim() = "" Then
                Me.ObjCommon.ShowAlert("No Subject Found In The Specified File.", Me.Page)
                Exit Sub
            End If

            Success_Entries = Success_Entries.Substring(0, Success_Entries.LastIndexOf(","))
            Me.ObjCommon.ShowAlert("Data Imported Successfully For Subjects : " + Success_Entries, Me.Page)
            Me.resetPage()
            Me.txtRemarks.Text = ""

            wStr = " vWorkSpaceId = '" + Me.HProjectId.Value.ToString() + "' AND iNodeId = '" + Me.ddlActivity.SelectedItem.Value.Substring(0, Me.ddlActivity.SelectedItem.Value.IndexOf("#")) + "' AND vActivityId = " + Me.ddlActivity.SelectedItem.Value.Substring(Me.ddlActivity.SelectedItem.Value.LastIndexOf("#") + 1) + " AND iNodeIndex=" + Me.ddlActivity.SelectedItem.Value.Substring(Me.ddlActivity.SelectedItem.Value.IndexOf("#") + 1, Me.ddlActivity.SelectedItem.Value.LastIndexOf("#") - (Me.ddlActivity.SelectedItem.Value.IndexOf("#") + 1)) + " "
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                If Not Me.ObjHelp.GetViewWorkSpaceUserNodeDetail(wStr, ds_WorkSpaceUserNodeDetail, eStr) Then
                    Throw New Exception(eStr)
                End If
            Else
                If Not Me.ObjHelp.GetViewWorkSpaceNodeDetail(wStr, ds_WorkSpaceUserNodeDetail, eStr) Then
                    Throw New Exception(eStr)
                End If
            End If

            If (ds_WorkSpaceUserNodeDetail.Tables(0).Rows(0)("cIsRepeatable").ToString() = "Y") Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "TablulerFormate", "TablulerFormate(); ", True)
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "GetDataAlreadyExist", "GetDataAlreadyExist(); ", True)
            End If

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ModalPopupclose", "ModalPopupClose(); ", True)
            Exit Sub
            ''================================================================ Ended by ketan

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Importing Data.", ex.Message)
        End Try

    End Sub


    Public Function ImportRepeatableData() As Boolean
        Dim ds_Save As New DataSet
        Dim ds_CRFHdr As New DataSet
        Dim ds_CRFDtl As New DataSet
        Dim ds_CRFSubDtl As New DataSet
        Dim dr_CRFSubDtl As DataRow
        Dim dt_DataFromFile As New DataTable
        Dim eStr As String = String.Empty
        Dim dr As DataRow
        Dim dt_Grid As New DataTable
        Dim dt_DataTobeImported As New DataTable
        Dim dv_DataTobeImported As New DataView
        Dim drNew As DataRow
        Dim Previous_ScreenNo As String = ""
        Dim dr_CRFDtl As DataRow
        Dim ds_Subjects As New DataSet
        Dim dv_Subjects As New DataView
        Dim wStr As String = String.Empty
        Dim ds_Template As New DataSet
        Dim dv_Template As New DataView
        Dim dv_PrevSubGroup As New DataView
        Dim Success_Entries As String = ""
        Dim dt_AttributeName As DataTable = New DataTable()
        Dim dt_MergeHDR_DTL_SUBDTL As DataTable = New DataTable()

        Dim strColName As String = String.Empty

        Dim strSubject As String = String.Empty

        Dim Previous_SubGroup As String = String.Empty
        Try

            If Me.ddlActivity.SelectedIndex = 0 Then
                Me.ObjCommon.ShowAlert("Please select an Activity", Me.Page)
                Exit Function
            End If

            dt_DataFromFile = CType(Me.ViewState(VS_DtDataFromFile), DataTable).Copy()
            dt_Grid = CType(Me.ViewState(VS_DtGrid), DataTable).Copy()
            dt_DataTobeImported = CType(Me.ViewState(VS_DtDataTobeImported), DataTable).Copy()
            dt_AttributeName = CType(Me.ViewState(VS_MappingDataForCRF), DataTable).Copy()

            dt_MergeHDR_DTL_SUBDTL.Columns.Add("STUDYID")
            dt_MergeHDR_DTL_SUBDTL.Columns.Add("SCRNNUM")
            dt_MergeHDR_DTL_SUBDTL.Columns.Add("USUBJID")
            dt_MergeHDR_DTL_SUBDTL.Columns.Add("VISIT")

            dt_MergeHDR_DTL_SUBDTL.Columns.Add("vCDISCValues")
            dt_MergeHDR_DTL_SUBDTL.Columns.Add("vMedExSubGroupdesc")
            dt_MergeHDR_DTL_SUBDTL.Columns.Add("vMEdExCode")
            dt_MergeHDR_DTL_SUBDTL.Columns.Add("vMedExDesc")
            dt_MergeHDR_DTL_SUBDTL.Columns.Add("vMedExResult")

            dt_MergeHDR_DTL_SUBDTL.Columns.Add("vHighRange")
            dt_MergeHDR_DTL_SUBDTL.Columns.Add("vLowRange")
            dt_MergeHDR_DTL_SUBDTL.Columns.Add("cHL")
            dt_MergeHDR_DTL_SUBDTL.Columns.Add("vMappedCol")

            dt_MergeHDR_DTL_SUBDTL.Columns.Add("iNodeId")
            dt_MergeHDR_DTL_SUBDTL.Columns.Add("iNodeIndex")

            For Each dr In dt_DataFromFile.Rows
                drNew = dt_DataTobeImported.NewRow()
                For Each dr_Grid As DataRow In dt_Grid.Rows
                    drNew(Convert.ToString(dr_Grid(GVC_SourceField))) = dr(dr_Grid(GVC_TargetField).ToString())
                Next dr_Grid
                dt_DataTobeImported.Rows.Add(drNew)
                dt_DataTobeImported.AcceptChanges()
            Next dr

            strColName = IIf(dt_Grid.Select("[Source field]='STUDYID'").Length = 0, "STUDYID,", "")
            strColName += IIf(dt_Grid.Select("[Source field]='SCRNNUM'").Length = 0, "SCRNNUM,", "")
            strColName += IIf(dt_Grid.Select("[Source field]='USUBJID'").Length = 0, "USUBJID,", "")
            strColName += IIf(dt_Grid.Select("[Source field]='VISIT'").Length = 0, "VISIT,", "")

            If strColName <> "" Then
                Me.ObjCommon.ShowAlert(strColName.Substring(0, strColName.Length - 1) + " Field Must be Mapped With CSV Import Fields . ", Me.Page)
                Exit Function
            End If

            dv_DataTobeImported = dt_DataTobeImported.DefaultView

            For Each dr_Grid1 As DataRow In dt_Grid.Rows
                If (Convert.ToString(dr_Grid1(GVC_SourceField)).ToUpper = "STUDYID" Or Convert.ToString(dr_Grid1(GVC_SourceField)).ToUpper = "SCRNNUM" Or Convert.ToString(dr_Grid1(GVC_SourceField)).ToUpper = "USUBJID" Or Convert.ToString(dr_Grid1(GVC_SourceField)).ToUpper() = "VISIT") Then
                    dr_Grid1.Delete()
                End If
            Next
            dt_Grid.AcceptChanges()

            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' And cStatusIndi <> 'D'"
            If Not Me.ObjHelp.View_WorkSpaceSubjectRegistration(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition _
                                , ds_Subjects, eStr) Then
                Throw New Exception(eStr)
            End If

            wStr += " And vActivityId = '" + Me.ddlActivity.SelectedItem.Value.Substring(Me.ddlActivity.SelectedItem.Value.LastIndexOf("#") + 1) + "' And cActiveFlag = 'Y' AND cIsVisibleFront = 'Y' "
            If Not Me.ObjHelp.GetVIEWMedExWorkspaceHdr(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition _
                                            , ds_Template, eStr) Then
                Throw New Exception(eStr)
            End If

            Dim dv_SubJect As DataView = New DataView(dt_DataFromFile)
            Dim dt_Subject As DataTable = dv_SubJect.ToTable(True, "SCRNNUM")

            Dim ds_WorkSpaceProtocolDetail As DataSet = New DataSet()
            ds_WorkSpaceProtocolDetail = ObjHelp.GetResultSet("Select * from WorkSpaceProtocolDetail  where vWorkspaceId='" + Me.HProjectId.Value.ToString() + "' AND cStatusIndi <> 'D'  ", "WorkSpaceProtocolDetail")

            For index_Subject As Integer = 0 To dt_Subject.Rows.Count - 1
                Dim dv_SubGroup As DataView = New DataView(dt_DataFromFile)
                dv_SubJect.RowFilter = "SCRNNUM = '" & dt_Subject.Rows(index_Subject)("SCRNNUM").ToString() & "' AND STUDYID = '" & ds_WorkSpaceProtocolDetail.Tables(0).Rows(0)("vProjectNo").ToString() & "' "
                Dim dt_SubGroup As DataTable = dv_SubJect.ToTable

                Dim dv_SubGroupWiseMapping As DataView = New DataView(dt_DataTobeImported)
                dv_SubGroupWiseMapping.RowFilter = "SCRNNUM = '" & dt_Subject.Rows(index_Subject)("SCRNNUM").ToString() & "' "
                Dim dt_SubGroupWiseMapping As DataTable = dv_SubGroupWiseMapping.ToTable

                Dim dv_SubjectId As DataView = New DataView(ds_Subjects.Tables(0))
                dv_SubjectId.RowFilter = "vMySubjectNo = '" & dt_Subject.Rows(index_Subject)("SCRNNUM").ToString() & "' "

                If (dv_SubjectId.ToTable().Rows.Count = 0) Then
                    strSubject += dt_Subject.Rows(index_Subject)("SCRNNUM").ToString() + ","
                    Continue For
                End If

                For index_SubGroup As Integer = 0 To dt_SubGroup.Rows.Count - 1
                    'For index1 As Integer = 0 To dt_Grid.Rows.Count - 1
                    For Each dr_Template In ds_Template.Tables(0).Rows
                        Dim row_Mapped As DataRow = dt_Grid.Select("[Source field]='" + Convert.ToString(dr_Template("vCDISCValues")).Trim().ToUpper() + "-" + Convert.ToString(dr_Template("vMedExDesc")).Trim().ToUpper() + "'").FirstOrDefault()
                        If Not row_Mapped Is Nothing Then
                            Dim searchedValue As String = row_Mapped.Item("target field")
                        End If
                        If ((Not row_Mapped Is Nothing) _
                            And Convert.ToString(dr_Template("ParentVisit")).Trim().ToUpper() = Convert.ToString(dt_SubGroup.Rows(index_SubGroup)("VISIT")).Trim().ToUpper()) Then
                           
                            drNew = dt_MergeHDR_DTL_SUBDTL.NewRow()

                            drNew("vMedExDesc") = Convert.ToString(dr_Template("vMedExDesc"))
                            drNew("vMEdExCode") = Convert.ToString(dr_Template("vMEdExCode"))

                            drNew("iNodeId") = Convert.ToString(dr_Template("iNodeId"))
                            drNew("iNodeIndex") = Convert.ToString(dr_Template("iNodeIndex"))

                            drNew("STUDYID") = dt_SubGroupWiseMapping.Rows(index_SubGroup)("STUDYID")
                            drNew("SCRNNUM") = Convert.ToString(dv_SubjectId.ToTable().Rows(0)("vSubjectId")).Trim()
                            drNew("USUBJID") = dt_SubGroupWiseMapping.Rows(index_SubGroup)("USUBJID")
                            drNew("VISIT") = dt_SubGroupWiseMapping.Rows(index_SubGroup)("VISIT")

                            'drNew("vCDISCValues") = dt_Grid.Rows(index1)(GVC_SourceField)
                            drNew("vCDISCValues") = Convert.ToString(dr_Template("vCDISCValues")).Trim().ToUpper()
                            drNew("vMedExResult") = dt_SubGroupWiseMapping.Rows(index_SubGroup)(Convert.ToString(dr_Template("vCDISCValues")).Trim().ToUpper() + "-" + Convert.ToString(dr_Template("vMedExDesc")).Trim().ToUpper())

                            'drNew("vHighRange") = dt_SubGroup.Rows(index_SubGroup)("RPTNRHI")
                            'drNew("vLowRange") = dt_SubGroup.Rows(index_SubGroup)("RPTNRLO")
                            'drNew("cHL") = dt_SubGroup.Rows(index_SubGroup)("ALRTFL")
                            drNew("vMedExSubGroupdesc") = dt_SubGroup.Rows(index_SubGroup)("LBTEST")
                            drNew("vMappedCol") = Convert.ToString(row_Mapped.Item("Target field")).Trim().ToUpper()

                            dt_MergeHDR_DTL_SUBDTL.Rows.Add(drNew)
                            dt_MergeHDR_DTL_SUBDTL.AcceptChanges()
                            dt_MergeHDR_DTL_SUBDTL.TableName = "dt_MergeHDR_DTL_SUBDTL"

                        ElseIf (Convert.ToString(dr_Template("ParentVisit")).Trim().ToUpper() = Convert.ToString(dt_SubGroup.Rows(index_SubGroup)("VISIT")).Trim().ToUpper()) Then

                            drNew = dt_MergeHDR_DTL_SUBDTL.NewRow()

                            drNew("vMedExDesc") = Convert.ToString(dr_Template("vMedExDesc"))
                            drNew("vMEdExCode") = Convert.ToString(dr_Template("vMEdExCode"))
                            drNew("iNodeId") = Convert.ToString(dr_Template("iNodeId"))
                            drNew("iNodeIndex") = Convert.ToString(dr_Template("iNodeIndex"))

                            drNew("STUDYID") = Convert.ToString(dt_SubGroupWiseMapping.Rows(index_SubGroup)("STUDYID"))
                            drNew("VISIT") = Convert.ToString(dt_SubGroupWiseMapping.Rows(index_SubGroup)("VISIT"))

                            drNew("SCRNNUM") = Convert.ToString(dv_SubjectId.ToTable().Rows(0)("vSubjectId"))
                            drNew("USUBJID") = Convert.ToString(dv_SubjectId.ToTable().Rows(0)("vRandomizationNo"))


                            drNew("vCDISCValues") = Convert.ToString(dr_Template("vCDISCValues"))
                            'drNew("vHighRange") = Convert.ToString(dr_Template("vHighRange"))
                            'drNew("vLowRange") = Convert.ToString(dr_Template("vLowRange"))
                            'drNew("cHL") = ""
                            'drNew("vMedExSubGroupdesc") = Convert.ToString(dr_Template("vMedExSubGroupdesc"))
                            drNew("vMedExSubGroupdesc") = dt_SubGroup.Rows(index_SubGroup)("LBTEST")

                            dt_MergeHDR_DTL_SUBDTL.Rows.Add(drNew)
                            dt_MergeHDR_DTL_SUBDTL.AcceptChanges()
                            dt_MergeHDR_DTL_SUBDTL.TableName = "dt_MergeHDR_DTL_SUBDTL"

                        End If

                    Next
                Next
            Next
            'Next

            If Not String.IsNullOrEmpty(strSubject).ToString().Length <> 0 AndAlso String.IsNullOrEmpty(strSubject.Substring(0, strSubject.Length - 1)) Then
                Me.ObjCommon.ShowAlert("No Subject Found In The Specified File.", Me.Page)
            End If
            If dt_MergeHDR_DTL_SUBDTL.Rows.Count <= 0 Then
                Exit Function
            End If

            wStr = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "'  And "
            wStr += "iNodeId in (" + Me.ddlActivity.SelectedItem.Value.Substring(0, Me.ddlActivity.SelectedItem.Value.IndexOf("#")) + ") AND "
            wStr += "vActivityId in ('" + Me.ddlActivity.SelectedItem.Value.Substring(Me.ddlActivity.SelectedItem.Value.LastIndexOf("#") + 1) + "')"

            If Not Me.ObjHelp.GetCRFHdr(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                               ds_CRFHdr, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_CRFHdr Is Nothing Then
                Throw New Exception("CRFHdr No. Not found.")
            End If

            If ds_CRFHdr.Tables(0).Rows.Count = 0 Then
                dr = ds_CRFHdr.Tables(0).NewRow()
                dr("nCRFHdrNo") = 0
                dr("vWorkSpaceId") = Me.HProjectId.Value.Trim()
                dr("dStartDate") = Date.Now.ToString()
                dr("iPeriod") = 1
                dr("iNodeId") = Me.ddlActivity.SelectedItem.Value.Substring(0, Me.ddlActivity.SelectedItem.Value.IndexOf("#"))
                dr("iNodeIndex") = Me.ddlActivity.SelectedItem.Value.Substring(Me.ddlActivity.SelectedItem.Value.IndexOf("#") + 1, Me.ddlActivity.SelectedItem.Value.LastIndexOf("#") - (Me.ddlActivity.SelectedItem.Value.IndexOf("#") + 1))
                dr("vActivityId") = Me.ddlActivity.SelectedItem.Value.Substring(Me.ddlActivity.SelectedItem.Value.LastIndexOf("#") + 1)
                dr("cLockStatus") = "U"
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dr("dModifyon") = DateTime.Now()
                ds_CRFHdr.Tables(0).Rows.Add(dr)
                ds_CRFHdr.AcceptChanges()
            Else
                txtRemarks.Text = ""
                If (Me.txtRemarks.Text.Trim() = "") Then
                    Me.ObjCommon.ShowAlert("Selected CSV data already been uploaded !", Me.Page)
                    Exit Function
                End If
            End If

            If Not Me.ObjHelp.GetCRFDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                             ds_CRFDtl, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not Me.ObjHelp.GetCRFSubDtl("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                                ds_CRFSubDtl, eStr) Then
                Throw New Exception(eStr)
            End If

            dv_DataTobeImported = dt_MergeHDR_DTL_SUBDTL.DefaultView
            dv_DataTobeImported.Sort = "SCRNNUM,vMedExSubGroupDesc"
            dt_DataTobeImported = Nothing
            dt_DataTobeImported = New DataTable
            dt_DataTobeImported = dv_DataTobeImported.ToTable()

            For index As Integer = 0 To dt_DataTobeImported.Rows.Count - 1

                If (Previous_ScreenNo.Trim() <> "" AndAlso _
                        Previous_ScreenNo.ToUpper() <> Convert.ToString(dt_DataTobeImported.Rows(index)("SCRNNUM")).Trim.ToUpper() + Convert.ToString(dt_DataTobeImported.Rows(index)("vMedexsubGroupdesc")).Trim.ToUpper() _
                        + Convert.ToString(dt_DataTobeImported.Rows(index)("iNodeId")).Trim.ToUpper()) Then
                    If ds_CRFDtl.Tables(0).Rows.Count > 0 Then

                        Success_Entries += Previous_ScreenNo.Trim() + ","
                        ds_Save.Tables.Add(ds_CRFHdr.Tables(0).Copy())
                        ds_Save.Tables(0).TableName = "CRFHdr"
                        ds_Save.AcceptChanges()

                        ds_Save.Tables.Add(ds_CRFDtl.Tables(0).Copy())
                        ds_Save.Tables(1).TableName = "CRFDtl"
                        ds_Save.AcceptChanges()

                        ds_Save.Tables.Add(ds_CRFSubDtl.Tables(0).Copy())
                        ds_Save.Tables(2).TableName = "CRFSubDtl"
                        ds_Save.AcceptChanges()


                        If ds_CRFDtl.Tables(0).Select("[nCRFDTLNO]='1'").Length = 1 Then
                            If Not Me.ObjLambda.Save_CRFHdrDtlSubDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                                            ds_Save, False, Me.Session(S_UserID), eStr) Then
                                Throw New Exception(eStr)
                            End If
                        Else
                            If Not Me.ObjLambda.Save_CRFSubDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                                     ds_CRFSubDtl, Me.Session(S_UserID), eStr) Then
                                Throw New Exception(eStr)
                            End If
                        End If

                    End If

                    ds_CRFDtl.Tables(0).Rows.Clear()
                    ds_CRFDtl.AcceptChanges()
                    ds_CRFSubDtl.Tables(0).Rows.Clear()
                    ds_CRFSubDtl.AcceptChanges()
                    ds_Save = Nothing
                    ds_Save = New DataSet

                End If


                dv_Template = dt_DataTobeImported.DefaultView
                dv_Template.RowFilter = " SCRNNUM = '" + dt_DataTobeImported.Rows(index)("SCRNNUM") + "' AND VMedExSubGroupDesc = '" + dt_DataTobeImported.Rows(index)("VMedExSubGroupDesc") + "' "
                If dv_Template.ToTable().Rows.Count < 1 Then
                    Previous_ScreenNo = Convert.ToString(dt_DataTobeImported.Rows(index)("SCRNNUM")).Trim() + Convert.ToString(dt_DataTobeImported.Rows(index)("VMedExSubGroupDesc")).Trim()
                    Continue For
                End If

                dv_Subjects = dt_DataTobeImported.DefaultView
                dv_Subjects.RowFilter = "USUBJID = '" + Convert.ToString(dt_DataTobeImported.Rows(index)("USUBJID")).Trim() + "'"

                dr_CRFSubDtl = ds_CRFSubDtl.Tables(0).NewRow()
                dr_CRFSubDtl("nCRFSubDtlNo") = index + 1

                If ds_CRFDtl.Tables(0).Rows.Count = 0 Then
                    dr_CRFSubDtl("nCRFDtlNo") = 1
                Else
                    dr_CRFSubDtl("nCRFDtlNo") = ds_CRFDtl.Tables(0).Rows(0)("nCRFDtlNo")
                End If
                dr_CRFSubDtl("iTranNo") = 1
                dr_CRFSubDtl("vMedExCode") = dt_DataTobeImported.Rows(index)("vMedExCode")
                dr_CRFSubDtl("vMedExDesc") = dt_DataTobeImported.Rows(index)("vMedExDesc")
                dr_CRFSubDtl("dMedExDatetime") = CDate(ObjCommon.GetCurDatetimeWithOffSet(Me.Session(S_TimeZoneName)).DateTime)
                dr_CRFSubDtl("vMedExResult") = dt_DataTobeImported.Rows(index)("vMedExResult")
                dr_CRFSubDtl("iModifyBy") = Me.Session(S_UserID)
                dr_CRFSubDtl("cHL") = dt_DataTobeImported.Rows(index)("cHL")
                dr_CRFSubDtl("vHighRange") = dt_DataTobeImported.Rows(index)("vHighRange")
                dr_CRFSubDtl("vLowRange") = dt_DataTobeImported.Rows(index)("vLowRange")
                dr_CRFSubDtl("vMappedCol") = dt_DataTobeImported.Rows(index)("vMappedCol")
                dr_CRFSubDtl("dModifyOn") = DateTime.Now()
                dr_CRFSubDtl("vModificationRemark") = txtRemarks.Text.Trim()
                ds_CRFSubDtl.Tables(0).Rows.Add(dr_CRFSubDtl)
                ds_CRFSubDtl.AcceptChanges()

                If Previous_ScreenNo.ToUpper() = Convert.ToString(dt_DataTobeImported.Rows(index)("SCRNNUM")).Trim.ToUpper() + Convert.ToString(dt_DataTobeImported.Rows(index)("vMedExSubGroupDesc")).Trim().ToUpper() + Convert.ToString(dt_DataTobeImported.Rows(index)("iNodeId")).Trim.ToUpper() Then
                    Continue For
                End If

                If ds_CRFDtl.Tables(0).Rows.Count = 0 Then
                    If dv_Subjects.ToTable().Rows.Count > 0 Then

                        dr_CRFDtl = ds_CRFDtl.Tables(0).NewRow()
                        dr_CRFDtl("nCRFDtlNo") = 1
                        dr_CRFDtl("nCRFHdrNo") = 1
                        dr_CRFDtl("iRepeatNo") = 1
                        dr_CRFDtl("dRepeatationDate") = CDate(ObjCommon.GetCurDatetimeWithOffSet(Me.Session(S_TimeZoneName)).DateTime)
                        dr_CRFDtl("vSubjectId") = Convert.ToString(dv_Subjects.ToTable().Rows(0)("SCRNNUM")).Trim()
                        dr_CRFDtl("iMySubjectNo") = Convert.ToString(dv_Subjects.ToTable().Rows(0)("USUBJID")).Trim()
                        dr_CRFDtl("cLockStatus") = "U"
                        dr_CRFDtl("iWorkFlowstageId") = Me.Session(S_WorkFlowStageId)
                        dr_CRFDtl("iModifyBy") = Me.Session(S_UserID)
                        dr_CRFDtl("cStatusIndi") = "N"
                        dr_CRFDtl("cDataStatus") = CRF_DataEntryCompleted
                        dr_CRFDtl("dModifyon") = DateTime.Now()

                        ds_CRFDtl.Tables(0).Rows.Add(dr_CRFDtl)
                        ds_CRFDtl.AcceptChanges()

                    End If
                End If
                Previous_ScreenNo = Convert.ToString(dt_DataTobeImported.Rows(index)("SCRNNUM")).Trim() + Convert.ToString(dt_DataTobeImported.Rows(index)("vMedExSubGroupDesc")).Trim() + Convert.ToString(dt_DataTobeImported.Rows(index)("iNodeId")).Trim().ToUpper()

                ds_CRFHdr.Tables(0).Rows(0)("iNodeId") = Convert.ToString(dt_DataTobeImported.Rows(index)("iNodeId"))
                ds_CRFHdr.Tables(0).Rows(0)("iNodeIndex") = Convert.ToString(dt_DataTobeImported.Rows(index)("iNodeIndex"))

            Next index

            If ds_CRFDtl.Tables(0).Rows.Count > 0 Then

                Success_Entries += Previous_ScreenNo.Trim() + ","
                ds_Save.Tables.Add(ds_CRFHdr.Tables(0).Copy())
                ds_Save.Tables(0).TableName = "CRFHdr"
                ds_Save.AcceptChanges()

                ds_Save.Tables.Add(ds_CRFDtl.Tables(0).Copy())
                ds_Save.Tables(1).TableName = "CRFDtl"
                ds_Save.AcceptChanges()

                ds_Save.Tables.Add(ds_CRFSubDtl.Tables(0).Copy())
                ds_Save.Tables(2).TableName = "CRFSubDtl"
                ds_Save.AcceptChanges()

                If ds_CRFDtl.Tables(0).Select("[nCRFDTLNO]='1'").Length = 1 Then
                    If Not Me.ObjLambda.Save_CRFHdrDtlSubDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                                ds_Save, False, Me.Session(S_UserID), eStr) Then
                        Me.ObjCommon.ShowAlert("There Are Some Fields, Which Are Not Matching In File. " + eStr, Me.Page)
                        Exit Function
                    End If
                Else
                    If Not Me.ObjLambda.Save_CRFSubDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                                    ds_CRFSubDtl, Me.Session(S_UserID), eStr) Then
                        Throw New Exception(eStr)
                    End If
                End If
            End If

            If Success_Entries.Trim() = "" Then
                Me.ObjCommon.ShowAlert("No Subject Found In The Specified File.", Me.Page)
                Exit Function
            End If

            Success_Entries = Success_Entries.Substring(0, Success_Entries.LastIndexOf(","))

            Me.ObjCommon.ShowAlert("Data Imported Successfully For Subjects : " + Success_Entries, Me.Page)
            Me.resetPage()

            Me.txtRemarks.Text = ""

            Dim ds_WorkSpaceUserNodeDetail As DataSet = New DataSet()
            wStr = " vWorkSpaceId = '" + Me.HProjectId.Value.ToString() + "' AND iNodeId = '" + Me.ddlActivity.SelectedItem.Value.Substring(0, Me.ddlActivity.SelectedItem.Value.IndexOf("#")) + "' AND vActivityId = " + Me.ddlActivity.SelectedItem.Value.Substring(Me.ddlActivity.SelectedItem.Value.LastIndexOf("#") + 1) + " AND iNodeIndex=" + Me.ddlActivity.SelectedItem.Value.Substring(Me.ddlActivity.SelectedItem.Value.IndexOf("#") + 1, Me.ddlActivity.SelectedItem.Value.LastIndexOf("#") - (Me.ddlActivity.SelectedItem.Value.IndexOf("#") + 1)) + " "
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                If Not Me.ObjHelp.GetViewWorkSpaceUserNodeDetail(wStr, ds_WorkSpaceUserNodeDetail, eStr) Then
                    Throw New Exception(eStr)
                End If
            Else
                If Not Me.ObjHelp.GetViewWorkSpaceNodeDetail(wStr, ds_WorkSpaceUserNodeDetail, eStr) Then
                    Throw New Exception(eStr)
                End If
            End If

            If (ds_WorkSpaceUserNodeDetail.Tables(0).Rows(0)("cIsRepeatable").ToString() = "Y") Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "TablulerFormate", "TablulerFormate(); ", True)
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "GetDataAlreadyExist", "GetDataAlreadyExist(); ", True)
            End If

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ModalPopupClose", "ModalPopupClose(); ", True)

            Exit Function

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Importing Data.", ex.Message)
        End Try



        Return True
    End Function

    Private Function CheckValueInDataTable(myTable As DataTable, columnName As String, searchValue As String) As Boolean
        For Each row As DataRow In myTable.Rows
            If row(columnName) = searchValue Then
                Return True
            End If
        Next
        Return False
    End Function

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.resetPage()
        If Not Me.GenCall() Then
            Exit Sub
        End If
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btnMap_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMap.Click
        
        If Not Me.FillMappingGrid() Then
            Exit Sub
        End If
    End Sub

    Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click
        Dim Path As String = String.Empty
        Dim dt_Grid As New DataTable

        Try

         
            If (CType(Me.ViewState(VS_DtDataExistOrNot), DataTable).Copy().Rows.Count <> 0 AndAlso CType(Me.ViewState(VS_DtDataExistOrNot), DataTable).Copy().Rows(0)("cIsRepeatable").ToString() = "Y") Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "TablulerFormate", "TablulerFormate(); ", True)
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "GetDataAlreadyExist", "GetDataAlreadyExist(); ", True)
            End If

            If Me.flupLabData.PostedFile Is Nothing Then
                Me.ObjCommon.ShowAlert("Please Select a File", Me.Page)
                Exit Sub
            End If

            dt_Grid.Columns.Add("Sr.No")
            dt_Grid.Columns.Add("Source Field")
            dt_Grid.Columns.Add("Target Field")
            dt_Grid.AcceptChanges()

            Me.ViewState(VS_DtGrid) = dt_Grid.Copy()

            Path = Me.flupLabData.PostedFile.FileName

            If Not Me.GetColumnsFromFile(Path, Me.flupLabData) Then
                Throw New Exception()
            End If


        Catch ex As Exception
            Me.ShowErrorMessage("Error While Importing Data.", ex.Message)
        End Try
    End Sub

    Protected Sub ddlActivity_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlActivity.SelectedIndexChanged
        Dim eStr As String = String.Empty
        Dim Param As String = String.Empty
        Dim ds_MedExWorkspaceHdrDTL As DataSet = New DataSet()
        Dim dt As New DataTable

        Dim wStr As String = String.Empty
        Dim ds_WorkSpaceUserNodeDetail As DataSet = New DataSet()

        Try
            Param = Me.HProjectId.Value.Trim() + "##" + Convert.ToString(Me.ddlActivity.SelectedValue).Split("#")(2)
            ds_MedExWorkspaceHdrDTL = ObjHelp.ProcedureExecute("Proc_GetMappingDataForCRF", Param)

            Me.ViewState(VS_MappingDataForCRF) = ds_MedExWorkspaceHdrDTL.Tables(0)

            Me.chkSourceColumns.ClearSelection()
            Me.chkSourceColumns.Items.Clear()

            dt.Columns.Add("STUDYID")
            dt.Columns.Add("SCRNNUM")
            dt.Columns.Add("USUBJID")
            dt.Columns.Add("VISIT")

            For Each row_val As DataRow In ds_MedExWorkspaceHdrDTL.Tables(0).Rows
                If Not (row_val("vCDISCValues").ToString().Contains("STUDYID") And row_val("vCDISCValues").ToString().Contains("SCRNNUM") And _
                        row_val("vCDISCValues").ToString().Contains("USUBJID") And row_val("vCDISCValues").ToString().Contains("VISIT")) Then


                    If Not dt.Columns.Contains(Convert.ToString(row_val("vCDISCValues")) + "-" + Convert.ToString(row_val("vMedExDesc"))) Then
                        dt.Columns.Add(Convert.ToString(row_val("vCDISCValues")) + "-" + Convert.ToString(row_val("vMedExDesc")))
                    End If

                End If
            Next
            Me.ViewState(VS_DtDataTobeImported) = dt
            dt.AcceptChanges()
            Me.ViewState(VS_DtDataTobeImported) = dt.Copy()

            For Each Col_Val As DataColumn In dt.Columns
                Me.chkSourceColumns.Items.Add(Col_Val.Caption.ToString().Trim())
            Next

            Me.ViewState(VS_DtBindSourceField) = ds_MedExWorkspaceHdrDTL.Tables(0)

            For i As Integer = 0 To chkSourceColumns.Items.Count - 1
                Dim row_Mapped As DataRow = ds_MedExWorkspaceHdrDTL.Tables(0).Select("[vCDISCValues]='" + Convert.ToString(chkSourceColumns.Items(i)).Split("-")(0) + "'").FirstOrDefault()
                If Not row_Mapped Is Nothing Then
                    If (row_Mapped.Item("cIsVisibleFront").ToString() = "N") Then
                        chkSourceColumns.Items(i).Attributes.Add("style", "color: red; font-size:12px;")
                    End If
                End If
            Next

            wStr = " vWorkSpaceId = '" + Me.HProjectId.Value.ToString() + "' AND iNodeId = '" + Me.ddlActivity.SelectedItem.Value.Substring(0, Me.ddlActivity.SelectedItem.Value.IndexOf("#")) + "' AND vActivityId = " + Me.ddlActivity.SelectedItem.Value.Substring(Me.ddlActivity.SelectedItem.Value.LastIndexOf("#") + 1) + " AND iNodeIndex=" + Me.ddlActivity.SelectedItem.Value.Substring(Me.ddlActivity.SelectedItem.Value.IndexOf("#") + 1, Me.ddlActivity.SelectedItem.Value.LastIndexOf("#") - (Me.ddlActivity.SelectedItem.Value.IndexOf("#") + 1)) + " "
            If Me.Session(S_ScopeNo) = Scope_ClinicalTrial Then
                If Not Me.ObjHelp.GetViewWorkSpaceUserNodeDetail(wStr, ds_WorkSpaceUserNodeDetail, eStr) Then
                    Throw New Exception(eStr)
                End If
            Else
                If Not Me.ObjHelp.GetViewWorkSpaceNodeDetail(wStr, ds_WorkSpaceUserNodeDetail, eStr) Then
                    Throw New Exception(eStr)
                End If
            End If

            Me.ViewState(VS_DtDataExistOrNot) = ds_WorkSpaceUserNodeDetail.Tables(0)

            If (CType(Me.ViewState(VS_DtDataExistOrNot), DataTable).Copy().Rows.Count <> 0 AndAlso CType(Me.ViewState(VS_DtDataExistOrNot), DataTable).Copy().Rows(0)("cIsRepeatable").ToString() = "Y") Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "TablulerFormate", "TablulerFormate(); ", True)
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "GetDataAlreadyExist", "GetDataAlreadyExist(); ", True)
            End If

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Importing Data.", ex.Message)
        End Try


    End Sub

#End Region

#Region "Reset Page"

    Private Sub resetPage()
        Me.chkSourceColumns.Items.Clear()
        Me.chkTargetColumns.Items.Clear()
        Me.GVWMapping.DataSource = Nothing
        Me.GVWMapping.DataBind()
        Me.ddlActivity.SelectedIndex = 0
        Me.txtproject.Text = ""
        Me.HProjectId.Value = ""
        Me.ViewState(VS_DtDataFromFile) = Nothing
        Me.ViewState(VS_DtDataTobeImported) = Nothing
        Me.ViewState(VS_DtGrid) = Nothing

        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "GetDataAlreadyExistDisplayNone", "document.getElementById('dvTabluer').style.display = 'none'; ", True)
        If Not Me.GenCall() Then
            Exit Sub
        End If
    End Sub

#End Region

#Region "GridView Events"

    Protected Sub GVWMapping_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GVWMapping.RowCommand

        Dim index As Integer = e.CommandArgument
        Dim dt_Grid As DataTable

        If e.CommandName = "DELETE" Then

            dt_Grid = CType(Me.ViewState(VS_DtGrid), DataTable).Copy()
            dt_Grid.Rows(index).Delete()
            dt_Grid.AcceptChanges()

            For index = 0 To dt_Grid.Rows.Count - 1
                dt_Grid.Rows(index)(GVC_SrNo) = index + 1
            Next
            dt_Grid.AcceptChanges()

            Me.ViewState(VS_DtGrid) = dt_Grid.Copy()
            Me.GVWMapping.DataSource = Nothing
            Me.GVWMapping.DataSource = dt_Grid
            Me.GVWMapping.DataBind()

        End If

    End Sub

    Protected Sub GVWMapping_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVWMapping.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            CType(e.Row.FindControl("imgBtnDelete"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("imgBtnDelete"), ImageButton).CommandName = "DELETE"

        End If

    End Sub

    Protected Sub GVWMapping_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GVWMapping.RowDeleting

    End Sub

#End Region

    <WebMethod> _
    Public Shared Function GetDataAlreadyImportCSVFile(ByVal vWorkspaceId As String, ByVal iNodeId As String, ByVal iNodeIndex As String, ByVal vActivityId As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim dt As New DataTable
        Dim Param As String = String.Empty
        Dim ds_ImportCSVdata As DataSet = New DataSet()
        Dim dt_ImportCSVdata As DataTable = New DataTable()
        Try
            Param = vWorkspaceId + "##" + iNodeId + "##" + iNodeIndex + "##" + vActivityId
            ds_ImportCSVdata = objHelp.ProcedureExecute("Proc_GetDataAlreadyImportCSVFile", Param)

            If ds_ImportCSVdata Is Nothing Then
                Exit Function
            End If

            dt_ImportCSVdata = ds_ImportCSVdata.Tables(0).Copy()

            Dim name(ds_ImportCSVdata.Tables(0).Columns.Count) As String
            Dim i As Integer = 0
            For Each column As DataColumn In ds_ImportCSVdata.Tables(0).Columns
                If (column.ColumnName.ToString().ToUpper() = ("RandomizationNo").ToUpper() Or column.ColumnName.ToString().ToUpper() = "LBSEQ") Then
                    Continue For
                End If

                Dim dv_SubGroup As DataView = New DataView(ds_ImportCSVdata.Tables(0))
                dv_SubGroup.RowFilter = "" + column.ColumnName + " IS NOT NULL AND " + column.ColumnName + " <> ''"
                Dim dt_SubGroup As DataTable = dv_SubGroup.ToTable
                If dv_SubGroup.ToTable.Rows.Count = 0 Then
                    dt_ImportCSVdata.Columns.Remove((column.ColumnName).ToString())
                    dt_ImportCSVdata.AcceptChanges()
                End If
            Next

            strReturn = JsonConvert.SerializeObject(dt_ImportCSVdata)
            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    <WebMethod> _
    Public Shared Function TablulerRepetationGrid(ByVal WorkspaceID As String, ByVal ActivityId As String, ByVal NodeId As String) As String
        Dim Wstr As String = String.Empty
        Dim estr_retu As String = String.Empty
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim ds_CRFSubjectHdrDtl As New DataSet
        Dim dtTablulerRepetation As New DataTable
        Dim AttributeValue As String = String.Empty
        Dim drTable As DataRow
        Dim Repetation As String = String.Empty
        Dim ds_Basic As New DataSet
        Dim strReturn As String = String.Empty
        Dim CodeNo As Integer = 0
        Dim StrGroup(1) As String
        Dim dt_MedexGroup As New DataTable
        Dim dv_MedexGroup As New DataView
        Dim StrGroupCodes As String = String.Empty
        Dim StrGroupDesc As String = String.Empty
        Try

            Wstr = "cActiveFlag<>'N' and vWorkSpaceId='" + WorkspaceID + "'" + _
                      " and vActivityId='" + ActivityId + "' And iNodeId=" + _
                      NodeId


            Wstr += "  Order by RepetitionNo,iSeqNo OPTION (MAXDOP 1)"

            If Not objHelp.View_CRFHdrDtlSubDtl_Edit(Wstr, "*,DENSE_RANK() OVER(PARTITION BY View_CRFHdrDtlSubDtl_Edit.nCRFHdrNo,View_CRFHdrDtlSubDtl_Edit.vActivityId,View_CRFHdrDtlSubDtl_Edit.vSubjectId ORDER BY View_CRFHdrDtlSubDtl_Edit.nCRFHdrNo,View_CRFHdrDtlSubDtl_Edit.vActivityId,View_CRFHdrDtlSubDtl_Edit.vSubjectId,View_CRFHdrDtlSubDtl_Edit.iRepeatNo) as [RepetitionNo] ", ds_CRFSubjectHdrDtl, estr_retu) Then
                Throw New Exception(estr_retu)
            End If

            dv_MedexGroup = ds_CRFSubjectHdrDtl.Tables(0).DefaultView

            StrGroup(0) = "vMedExGroupCode"
            StrGroup(1) = "vMedExGroupDesc"
            dt_MedexGroup = dv_MedexGroup.ToTable(True, StrGroup)

            For Each drGroup In dt_MedexGroup.Rows

                If StrGroupCodes <> "" Then
                    StrGroupCodes += ",Div" + drGroup("vMedExGroupCode")
                    StrGroupDesc += "," + drGroup("vMedExGroupDesc")
                Else
                    StrGroupCodes = "Div" + drGroup("vMedExGroupCode")
                    StrGroupDesc = "" + drGroup("vMedExGroupDesc")
                End If

            Next

            Wstr = "cActiveFlag<>'N' and vWorkSpaceId='" + WorkspaceID + "'" & _
                        " and vActivityId='" + ActivityId + "'" + _
                        " And iNodeId=" + _
                        NodeId + " Order by iSeqNo"

            If Not objHelp.GetViewMedExWorkSpaceDtl(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Basic, estr_retu) Then
                Throw New Exception(estr_retu)
            End If

            ds_CRFSubjectHdrDtl.Tables(0).DefaultView.RowFilter = "vMedExDesc='' and RepetitionNo='1' "
            If ds_CRFSubjectHdrDtl.Tables(0).DefaultView.ToTable.Rows.Count > 0 Then
                CodeNo = Convert.ToInt32(ds_CRFSubjectHdrDtl.Tables(0).DefaultView.ToTable.Rows(0)("vMedExCode").ToString)
            End If

            dtTablulerRepetation.Columns.Add("Repetition")

            For Each dr As DataRow In ds_Basic.Tables(0).Rows
                If dr("vMedExCode") <> CodeNo Then
                    AttributeValue = dr("vMedExDesc").ToString()
                    dtTablulerRepetation.Columns.Add(AttributeValue)
                    AttributeValue = ""
                End If
            Next

            For Each drCRF As DataRow In ds_CRFSubjectHdrDtl.Tables(0).Rows

                ds_CRFSubjectHdrDtl.Tables(0).DefaultView.RowFilter = "RepetitionNo = " + drCRF("RepetitionNo").ToString()
                drTable = dtTablulerRepetation.NewRow()

                If drCRF("RepetitionNo").ToString() <> Repetation.ToString() Then

                    Repetation = drCRF("RepetitionNo")
                    drTable("Repetition") = StrGroupDesc + "_" + Repetation

                    For Each dr As DataRow In ds_CRFSubjectHdrDtl.Tables(0).DefaultView.ToTable.Rows
                        If dr("vMedExDesc").ToString() <> "" Then
                            AttributeValue = dr("vMedExDesc").ToString()
                            drTable(AttributeValue) = dr("vDefaultValue").ToString()
                            AttributeValue = ""
                        End If
                    Next

                    dtTablulerRepetation.Rows.Add(drTable)
                    dtTablulerRepetation.AcceptChanges()
                End If

            Next
            strReturn = JsonConvert.SerializeObject(dtTablulerRepetation)
            Return strReturn
        Catch ex As Exception
            Throw New Exception("Error while TablulerRepetation()")
        End Try
    End Function

End Class
