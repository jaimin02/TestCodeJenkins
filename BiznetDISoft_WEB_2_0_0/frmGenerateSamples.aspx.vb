Imports System.IO.StreamWriter
Imports Aspose.BarCode
Imports System.Drawing.Printing


Partial Class frmGenerateSamples
    Inherits System.Web.UI.Page

#Region " VARIABLE DECLARATION "

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private Const VS_Choice As String = "Choice"

    Private Const VS_DtSubMst As String = "SubjectMst" 'gvwSubject

    Private Const VS_DtMedExMst As String = "MedExMst" 'ddlMedEx
    Private Const VS_DtMedExWorkSpaceDtl As String = "MedExWorkSpaceDtl" 'gvwMedEx

    Private Const VS_DtSampleDetail As String = "SampleDetail"
    Private Const VS_DtSampleMedExDetail As String = "SampleMedExDetail"

    Private Const AddToGrid As String = "AddToGrid"
    Private Const AddToDatabase As String = "AddToDatabase"

    'Private Const GVC_SubCode As Integer = 0
    'Private Const GVC_SubName As Integer = 1

#End Region

#Region "Form Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                GenCall()
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region " GenCall() "
    Private Function GenCall() As Boolean
        Dim ds As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try
            Choice = "1"
            Me.ViewState(VS_Choice) = Choice   'To use it while saving

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            'Me.ViewState(VS_DtSubMst) = ds.Tables("view_SubjectMaster")   ' adding blank DataTable in viewstate

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

#Region " GenCall_Data "
    Private Function GenCall_Data(ByRef ds_DWR_Retu As DataSet) As Boolean

        Dim eStr_Retu As String = ""
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing

        Try
            Choice = Me.ViewState(VS_Choice)

            If Not objHelp.GetSampleDetail("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                      ds, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If

            Me.ViewState(VS_DtSampleDetail) = ds.Tables(0)
            ds = Nothing

            If Not objHelp.GetSampleMedExDetail("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                                  ds, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If

            Me.ViewState(VS_DtSampleMedExDetail) = ds.Tables(0)

            ds_DWR_Retu = ds
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")

        Finally

        End Try
    End Function
#End Region

#Region " GenCall_showUI() "
    Private Function GenCall_ShowUI() As Boolean
        Dim dt_SubMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try
            Page.Title = " :: Generate Samples ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Generate Samples"
            dt_SubMst = Me.ViewState(VS_DtSubMst)
            Choice = Me.ViewState("Choice")

            If Me.rblSelection.Items(1).Selected Then 'Project Specific
                Me.pnlProjectSpecific.Visible = True
            Else
                Me.rblSelection.Items(0).Selected = True 'Screening
                Me.pnlProjectSpecific.Visible = False
            End If

            If Not FillGridgvwSubject() Then
                Exit Function
            End If

            If Not FillddlMedexGroup() Then
                Exit Function
            End If

            If Not FillddlMedex() Then
                Exit Function
            End If

            If Not FillGridgvwMedEx() Then
                Exit Function
            End If

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                'Me.txtOpName.Text = dt_OpMst.Rows(0).Item("vOperationName")
                'Me.TxtOpPath.Text = dt_OpMst.Rows(0).Item("vOperationPath")
                'Me.DDLParentOp.SelectedValue = dt_OpMst.Rows(0).Item("vParentOperationCode")
                'Me.TxtSeq.Value = dt_OpMst.Rows(0).Item("iSeqNo")
            End If

            Me.btnSave.Attributes.Add("onClientClick", "return ValidationToSave('" & Me.gvwSubject.ClientID & "');")

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function
#End Region

#Region "FillDropDown"
    Private Function FillddlActivity() As Boolean
        Dim ds_MedexWorkspaceDtl As New Data.DataSet
        Dim estr As String = ""
        Dim wstr As String = "1=1"
        Try
            wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "'"
            wstr += " And vMedexType='Import' "
            objHelp.GetViewMedExWorkSpaceDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_MedexWorkspaceDtl, estr)

            Me.ddlActivity.DataSource = ds_MedexWorkspaceDtl.Tables(0).DefaultView.ToTable(True, "vActivityId,vActivityName".Split(","))
            Me.ddlActivity.DataValueField = "vActivityId"
            Me.ddlActivity.DataTextField = "vActivityName"
            Me.ddlActivity.DataBind()
            Me.ddlActivity.Items.Insert(0, New ListItem("--Select Activity--", 0))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function FillddlPeriod() As Boolean
        Dim ds_WorkspaceNodeDetail As New Data.DataSet
        Dim estr As String = ""
        Dim wstr As String = "1=1"
        Try
            wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "' And vActivityId='" & Me.ddlActivity.SelectedValue & "'"

            objHelp.GetViewWorkSpaceNodeDetail(wstr, ds_WorkspaceNodeDetail, estr)

            Me.ddlPeriod.DataSource = ds_WorkspaceNodeDetail.Tables(0).DefaultView.ToTable(True, "iPeriod".ToString())
            Me.ddlPeriod.DataValueField = "iPeriod"
            Me.ddlPeriod.DataTextField = "iPeriod"
            Me.ddlPeriod.DataBind()
            Me.ddlPeriod.Items.Insert(0, New ListItem("--Select Period--", 0))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function FillddlNode() As Boolean
        Dim ds_WorkspaceNodeDetail As New Data.DataSet
        Dim estr As String = ""
        Dim wstr As String = "1=1"
        Try
            wstr = "vWorkspaceId='" & Me.HProjectId.Value.Trim() & "' And vActivityId='" & Me.ddlActivity.SelectedValue & "' And iPeriod='" & Me.ddlPeriod.SelectedValue & "'"

            objHelp.GetViewWorkSpaceNodeDetail(wstr, ds_WorkspaceNodeDetail, estr)

            Me.ddlNode.DataSource = ds_WorkspaceNodeDetail.Tables(0).DefaultView.ToTable(True, "iNodeId,vNodeDisplayName".Split(","))
            Me.ddlNode.DataValueField = "iNodeId"
            Me.ddlNode.DataTextField = "vNodeDisplayName"
            Me.ddlNode.DataBind()
            Me.ddlNode.Items.Insert(0, New ListItem("--Select Node--", 0))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function FillddlMedexGroup() As Boolean
        Dim ds_MedexGroup As New DataSet
        Dim estr As String = ""
        Dim wstr As String = ""
        Try
            Me.ddlMedexGroup.Items.Clear()
            wstr = " vMedexType='Import' "

            If Not objHelp.GetMedExMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, _
                                            ds_MedexGroup, estr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data from MedexGroupMst:" + estr, Me.Page)
                Return False
            End If
            Me.ddlMedexGroup.DataSource = ds_MedexGroup.Tables(0).DefaultView.ToTable(True, "vMedExGroupCode,vMedExGroupDesc".Split(","))
            Me.ddlMedexGroup.DataValueField = "vMedExGroupCode"
            Me.ddlMedexGroup.DataTextField = "vMedExGroupDesc"
            Me.ddlMedexGroup.DataBind()
            Me.ddlMedexGroup.Items.Insert(0, New ListItem("--Select MedExGroup--", 0))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function FillddlMedex() As Boolean
        Dim ds_MedexMst As New Data.DataSet
        Dim wstr As String = ""
        Dim estr As String = ""
        Try
            wstr = " vMedexGroupCode='" + Convert.ToString(Me.ddlMedexGroup.SelectedValue) + "'"
            wstr += " And  vMedexType='Import' "

            If Not objHelp.GetMedExMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_MedexMst, estr) Then
                Me.ObjCommon.ShowAlert("Error While Getting Data from MedexMst:" + estr, Me.Page)
                Return False
            End If

            Me.ddlMedex.DataSource = ds_MedexMst
            Me.ddlMedex.DataValueField = "vMedExCode"
            Me.ddlMedex.DataTextField = "vMedExDesc"
            Me.ddlMedex.DataBind()
            Me.ddlMedex.Items.Insert(0, New ListItem("--Select MedEx--", 0))

            Me.ViewState(VS_DtMedExMst) = ds_MedexMst.Tables(0)
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function
#End Region

#Region "FillGrid"

    Private Function FillGridgvwSubject() As Boolean
        Dim ds_gvwSubject As New Data.DataSet
        Dim dt_gvwSubject As New DataTable
        Dim estr As String = ""
        Dim wstr As String = ""
        Try
            If Not Me.ViewState(VS_DtSubMst) Is Nothing Then

                dt_gvwSubject = CType(Me.ViewState(VS_DtSubMst), DataTable)
                ds_gvwSubject.Tables.Add(dt_gvwSubject)

            Else

                Me.gvwSubject.DataSource = Nothing
                Me.gvwSubject.DataBind()

                If Me.rblSelection.SelectedItem.Value = "00" Then 'If Screening

                    'wstr = "vLocationCode= '" + Me.Session(S_LocationCode).ToString + "'"
                    'If Not objHelp.GetView_SubjectMaster(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_gvwSubject, estr) Then
                    '    Return False
                    'End If

                    wstr = "vWorkspaceId= '0000000000'"
                    If Not objHelp.getViewMedExScreeningHdrDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_gvwSubject, estr) Then
                        Return False
                    End If

                Else 'If Project Specific

                    wstr = "vWorkspaceId = '" & Me.HProjectId.Value.Trim() & "'"
                    If Not objHelp.GetViewWorkspaceSubjectMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_gvwSubject, estr) Then
                        Return False
                    End If

                End If
            End If
            Me.gvwSubject.DataSource = ds_gvwSubject.Tables(0).DefaultView.ToTable(True, "vSubjectId,FullName".Split(","))
            Me.gvwSubject.DataBind()
            Me.ViewState(VS_DtSubMst) = ds_gvwSubject.Tables(0).DefaultView.ToTable(True, "vSubjectId,FullName".Split(","))

            If ds_gvwSubject.Tables(0).Rows.Count < 1 Then
                Me.tblMedEx.Visible = False
            Else
                Me.tblMedEx.Visible = True
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function FillGridgvwMedEx() As Boolean
        Dim ds_gvwMedEx As New Data.DataSet
        Dim dt_gvwMedEx As New DataTable
        Dim estr As String = ""
        Dim wstr As String = ""
        Try
            If Not Me.ViewState(VS_DtMedExWorkSpaceDtl) Is Nothing Then

                dt_gvwMedEx = CType(Me.ViewState(VS_DtMedExWorkSpaceDtl), DataTable)
                ds_gvwMedEx.Tables.Add(dt_gvwMedEx)

            Else

                Me.gvwMedEx.DataSource = Nothing
                Me.gvwMedEx.DataBind()

                If Me.rblSelection.SelectedItem.Value = "00" Then 'If Screening
                    wstr = "vWorkspaceId='0000000000' And vActivityId='0003'"
                    wstr += " And  vMedexType='Import' "
                    If Not objHelp.GetViewMedExWorkSpaceDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_gvwMedEx, estr) Then
                        Return False
                    End If

                Else 'If Project Specific

                    wstr = "vWorkspaceId= '" & Me.HProjectId.Value.Trim() & "' And vActivityId='" & Me.ddlActivity.SelectedValue & "'"
                    wstr += " And  vMedexType='Import'"
                    If Not objHelp.GetViewMedExWorkSpaceDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_gvwMedEx, estr) Then
                        Return False
                    End If

                End If
            End If

            Me.gvwMedEx.DataSource = ds_gvwMedEx
            Me.gvwMedEx.DataBind()
            Me.ViewState(VS_DtMedExWorkSpaceDtl) = ds_gvwMedEx.Tables(0)

            If Me.gvwMedEx.Rows.Count < 1 Then
                Me.btnSave.Visible = False
                Me.chkBarcode.Visible = False
            ElseIf Me.gvwMedEx.Rows.Count > 0 Then
                Me.btnSave.Visible = True
                'Me.chkBarcode.Visible = True
                Me.btnPrintBarcode.Visible = False
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
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

#Region "RadioButton List Events"
    Protected Sub rblSelection_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Me.resetpage()
            If Me.rblSelection.Items(0).Selected = False Then

                If Not FillddlActivity() Then
                    Exit Sub
                End If

                If Not FillddlPeriod() Then
                    Exit Sub
                End If

                If Not FillddlNode() Then
                    Exit Sub
                End If

            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub
#End Region

#Region "DropDown List Events"
    Protected Sub ddlMedexGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Not FillddlMedex() Then
                Exit Sub
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub ddlActivity_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Not FillddlPeriod() Then
                Exit Sub
            End If
            If Not FillddlNode() Then
                Exit Sub
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub ddlPeriod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Not FillddlNode() Then
                Exit Sub
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub
#End Region

#Region "Button Events"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim eStr_Retu As String = ""
        Try

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                If Me.rblSelection.Items(0).Selected = False Then
                    If Me.ddlNode.SelectedIndex = 0 Then
                        ObjCommon.ShowAlert("Select Node ", Me.Page)
                        Exit Sub
                    End If
                End If
                '*************Barcode**************
                If File.Exists("E:\temp.doc") Then
                    File.Delete("E:\temp.doc")
                End If
                '**********************************
                AssignValues(AddToDatabase)
                Me.resetpage()
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr_Retu)
        End Try

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            Me.ViewState(VS_DtSubMst) = Nothing
            Me.ViewState(VS_DtMedExWorkSpaceDtl) = Nothing
            Me.GenCall_ShowUI()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub btnAddMedEx_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim ds_gvwMedEx As New DataSet
        Dim dt_DtMedExWorkSpaceDtl As New DataTable
        Dim estr As String = ""
        Dim wstr As String = ""
        Try
            If Not Me.ddlMedexGroup.SelectedIndex = 0 Then
                If Not Me.ddlMedex.SelectedIndex = 0 Then

                    AssignValues(AddToGrid)
                    dt_DtMedExWorkSpaceDtl = CType(Me.ViewState(VS_DtMedExWorkSpaceDtl), DataTable)
                    ds_gvwMedEx = New DataSet
                    dt_DtMedExWorkSpaceDtl.TableName = "MedExWorkSpaceDtl"
                    ds_gvwMedEx.Tables.Add(dt_DtMedExWorkSpaceDtl)
                    Me.gvwMedEx.DataSource = ds_gvwMedEx
                    Me.gvwMedEx.DataBind()

                    If Me.gvwMedEx.Rows.Count < 1 Then
                        Me.btnSave.Visible = False
                        Me.chkBarcode.Visible = False
                    ElseIf Me.gvwMedEx.Rows.Count > 0 Then
                        Me.btnSave.Visible = True
                        'Me.chkBarcode.Visible = True
                        Me.btnPrintBarcode.Visible = False
                    End If
                Else
                    'ObjCommon.ShowAlert("Select MedEx ", Me.Page)
                    Exit Sub
                End If
            Else
                'ObjCommon.ShowAlert("Select MedExGroup ", Me.Page)
                Exit Sub
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Not FillddlActivity() Then
                Exit Sub
            End If
            If Not FillddlPeriod() Then
                Exit Sub
            End If
            If Not FillddlNode() Then
                Exit Sub
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub
#End Region

#Region "Grid Events"

    Protected Sub gvwsubject_pageindexchanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Try
            gvwSubject.PageIndex = e.NewPageIndex
            If Not FillGridgvwSubject() Then
                Exit Sub
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub gvwmedex_pageindexchanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Try
            gvwMedEx.PageIndex = e.NewPageIndex
            If Not FillGridgvwMedEx() Then
                Exit Sub
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub gvwMedEx_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs)

    End Sub

    Protected Sub gvwMedEx_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then

            CType(e.Row.FindControl("imgDelete"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("imgDelete"), ImageButton).CommandName = "Delete"

        End If
    End Sub

    Protected Sub gvwMedEx_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim i As Integer = e.CommandArgument

        If e.CommandName.ToUpper = "DELETE" Then

            Dim dt_DtMedExWorkSpaceDtl As New DataTable

            Try

                dt_DtMedExWorkSpaceDtl = CType(Me.ViewState(VS_DtMedExWorkSpaceDtl), DataTable)

                'Deleteing from Grid Data table dt_DtMedExWorkSpaceDtl

                dt_DtMedExWorkSpaceDtl.Rows(i).Delete()
                dt_DtMedExWorkSpaceDtl.AcceptChanges()

                Me.gvwMedEx.DataSource = dt_DtMedExWorkSpaceDtl
                Me.gvwMedEx.DataBind()

                Me.ViewState(VS_DtMedExWorkSpaceDtl) = dt_DtMedExWorkSpaceDtl

            Catch ex As Exception
                Me.ShowErrorMessage(ex.Message, "")
            Finally
                dt_DtMedExWorkSpaceDtl = Nothing
            End Try

        End If

    End Sub

#End Region

#Region "Assign Values"
    Private Sub AssignValues(ByVal mode As String)
        Dim dr As DataRow = Nothing
        Dim drMedEx As DataRow = Nothing
        Dim dt_MedEx As New DataTable
        Dim dt_DtMedExWorkSpaceDtl As New DataTable
        Dim ds As New DataSet
        Dim ds_save As New DataSet
        Dim dt_SampleDetail As New DataTable
        Dim dt_SampleMedExDetail As New DataTable
        Dim estr_Retu As String = ""
        Dim wstr As String = ""
        Dim IndexSubject As Integer = 0
        Dim IndexMedEx As Integer = 0
        Dim chkSubject As CheckBox

        Dim nSampleId_Retu As String = "" 'Return variable from Save_SAmpleDEtail
        Dim vSampleId_Retu As String = "" 'Return variable from Save_SAmpleDEtail
        Dim objStreamWriter As StreamWriter
        Try
            If mode = AddToGrid Then

                dt_MedEx = CType(Me.ViewState(VS_DtMedExMst), DataTable)
                dt_DtMedExWorkSpaceDtl = CType(Me.ViewState(VS_DtMedExWorkSpaceDtl), DataTable)

                If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                    For Each dr In dt_MedEx.Rows
                        If (dr("vMedExCode").ToString = Me.ddlMedex.SelectedValue.ToString) Then
                            drMedEx = dr
                            Exit For
                        End If
                    Next

                    '--------Checking for duplicate MedEx Entry
                    For Each dr In dt_DtMedExWorkSpaceDtl.Rows
                        If (dr("vMedExCode") = Me.ddlMedex.SelectedValue.ToString) Then
                            ObjCommon.ShowAlert("Selected MedEx is already added ", Me.Page)
                            Exit Sub
                        End If
                    Next
                    '----------------

                    dr = dt_DtMedExWorkSpaceDtl.NewRow()
                    dr("vMedExCode") = drMedEx("vMedExCode")
                    dr("vMedExDesc") = drMedEx("vMedExDesc")
                    dr("vDefaultValue") = drMedEx("vDefaultValue")
                    dt_DtMedExWorkSpaceDtl.Rows.Add(dr)
                    dt_DtMedExWorkSpaceDtl.AcceptChanges()

                End If
                Me.ViewState(VS_DtMedExWorkSpaceDtl) = dt_DtMedExWorkSpaceDtl

            ElseIf mode = AddToDatabase Then

                For IndexSubject = 0 To Me.gvwSubject.Rows.Count - 1

                    chkSubject = Me.gvwSubject.Rows(IndexSubject).FindControl("chkSelect_Sub")
                    If Not chkSubject Is Nothing AndAlso chkSubject.Checked Then

                        Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add

                        dt_SampleDetail = Me.ViewState(VS_DtSampleDetail)
                        dt_SampleMedExDetail = Me.ViewState(VS_DtSampleMedExDetail)
                        dt_SampleDetail.Clear()
                        dt_SampleMedExDetail.Clear()

                        'Adding datarow to SampleDetail data table

                        dr = dt_SampleDetail.NewRow()
                        dr("vSampleId") = 0
                        dr("vLocationCode") = Me.Session(S_LocationCode).ToString
                        dr("vSubjectID") = Me.gvwSubject.Rows(IndexSubject).Cells(1).Text.ToString
                        dr("dCollectionDateTime") = Today.Date.Now.ToString("dd-MMM-yy")
                        'For Screening
                        dr("vWorkspaceId") = "0000000000"
                        dr("vActivityId") = GeneralModule.Act_Screening
                        dr("iNodeId") = 0
                        dr("iPeriod") = 0
                        dr("iModifyBy") = Me.Session(S_UserID)
                        'dr("dModifyOn") = 
                        dr("cStatusIndi") = "N"
                        'For Project specific
                        If Me.rblSelection.SelectedValue = "01" Then
                            dr("vWorkspaceId") = Me.HProjectId.Value
                            dr("vActivityId") = Me.ddlActivity.SelectedItem.Value
                            dr("iNodeId") = Me.ddlNode.SelectedItem.Value
                            dr("iPeriod") = Me.ddlPeriod.SelectedItem.Value
                        End If

                        dt_SampleDetail.Rows.Add(dr)

                        For IndexMedEx = 0 To Me.gvwMedEx.Rows.Count - 1

                            Dim chkMedEx As CheckBox = Me.gvwMedEx.Rows(IndexMedEx).FindControl("chkSelect_Med")
                            If Not chkMedEx Is Nothing AndAlso chkMedEx.Checked Then

                                'Adding datarow to SampleMedExDetail data table
                                drMedEx = dt_SampleMedExDetail.NewRow()
                                drMedEx("vMedExCode") = Me.gvwMedEx.Rows(IndexMedEx).Cells(1).Text.ToString
                                drMedEx("vMedExResult") = ""
                                'drMedEx("vMachineDetails") = 
                                'drMedEx("iMedExDoneBy") = 
                                'drMedEx("dMedExDoneOn") = Today.Date.Now
                                'drMedEx("iMedExApprovedBy") = 
                                'drMedEx("dMedExApprovedOn") = 
                                drMedEx("iModifyBy") = Me.Session(S_UserID)
                                'drMedEx("dModifyOn") = 
                                drMedEx("cStatusIndi") = "N"
                                dt_SampleMedExDetail.Rows.Add(drMedEx)

                            End If
                        Next IndexMedEx

                        'Checking for Duplicate Entry

                        '  wstr = "vSubjectID = '" + Me.gvwSubject.Rows(IndexSubject).Cells(1).Text.ToString + "'"
                        '  wstr += " And vWorkspaceId = '0000000000'"
                        '  wstr += " And vActivityId = 0003"
                        '  wstr += " And iNodeId = 0"
                        '  If Me.rblSelection.SelectedValue = "01" Then 'Project Specific
                        '      wstr = "vSubjectID = '" + Me.gvwSubject.Rows(IndexSubject).Cells(1).Text.ToString + "'"
                        '      wstr += " And vWorkspaceId = '" + Me.HProjectId.Value + "'"
                        '      wstr += " And vActivityId = " + Me.ddlActivity.SelectedItem.Value
                        '      wstr += " And iNodeId = " + Me.ddlNode.SelectedItem.Value
                        '  End If

                        '  If Not objHelp.Get_ViewSubjectSampleDetail(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                        'ds, estr_Retu) Then
                        '      Response.Write(estr_Retu)
                        '      Exit Sub
                        '  End If
                        '  If Not ds.Tables(0).Rows.Count < 1 Then
                        '      'ObjCommon.ShowAlert("Subject already exists : " + Me.gvwSubject.Rows(IndexSubject).Cells(2).Text.ToString, Me.Page)
                        '      Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                        '      For Each dr In dt_SampleDetail.Rows
                        '          dr("nSampleId") = ds.Tables(0).Rows(0).Item(0)
                        '          dt_SampleDetail.AcceptChanges()
                        '      Next
                        '  End If

                        '----------End Checking Duplicate Entry---------------

                        ds_save = New DataSet
                        ds_save.Tables.Add(dt_SampleDetail.Copy())
                        ds_save.Tables(0).TableName = "SampleDetail"

                        ds_save.Tables.Add(dt_SampleMedExDetail.Copy())
                        ds_save.Tables(1).TableName = "SampleMedExDetail"

                        If Not objLambda.Save_SampleDetail(Me.ViewState(VS_Choice), ds_save, Me.Session(S_UserID), nSampleId_Retu, vSampleId_Retu, estr_Retu) Then
                            ObjCommon.ShowAlert("Error While Saving SampleDetail", Me.Page)
                            Exit Sub
                        End If

                        '*************Barcode Generating********************

                        objStreamWriter = New StreamWriter("E:\temp.doc", True)
                        objStreamWriter.WriteLine(vSampleId_Retu)
                        objStreamWriter.Close()

                        '***************************************************
                    End If

                Next IndexSubject
                ObjCommon.ShowAlert("SampleDetail Saved SuccessFully", Me.Page)
                'Me.btnPrintBarcode.Visible = True
                'If chkBarcode.Checked Then
                '    Response.Redirect("http://localhost:1618/Barcode-WebAspose/Default.aspx", False)
                'End If

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

#End Region

#Region "ResetPage"
    Protected Sub resetpage()
        Me.HProjectId.Value = ""
        Me.ddlActivity.DataSource = Nothing
        Me.ddlActivity.DataBind()
        Me.ddlPeriod.DataSource = Nothing
        Me.ddlPeriod.DataBind()
        Me.ddlNode.DataSource = Nothing
        Me.ddlNode.DataBind()
        Me.ddlMedexGroup.DataSource = Nothing
        Me.ddlMedexGroup.DataBind()
        Me.ddlMedex.DataSource = Nothing
        Me.ddlMedex.DataBind()

        Me.ViewState(VS_Choice) = Nothing
        Me.ViewState(VS_DtSampleDetail) = Nothing
        Me.ViewState(VS_DtSampleMedExDetail) = Nothing
        Me.ViewState(VS_DtMedExMst) = Nothing
        Me.ViewState(VS_DtSubMst) = Nothing
        Me.ViewState(VS_DtMedExWorkSpaceDtl) = Nothing

        Me.gvwSubject.DataSource = Nothing
        Me.gvwSubject.DataBind()
        Me.gvwMedEx.DataSource = Nothing
        Me.gvwMedEx.DataBind()

        GenCall()
    End Sub
#End Region

#Region "Barcode"

    Protected Sub btnPrintBarcode_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        'Dim q As String = ""
        'Try

        '    'q = "window.open(""" + "http://localhost/Barcode-WebAspose/Barcode.aspx" + """)"
        '    q = "window.open(""" + "http://localhost/BarCodeCaption/default.aspx" + """)"
        '    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", q, True)

        '    'Response.Redirect("http://localhost/Barcode-WebAspose/Barcode.aspx", False)
        'Catch ex As Exception
        '    Me.ShowErrorMessage(ex.Message, "")
        'End Try





        ' set the properties of the barcode control
        'BarCodeWebControl1.CodeText = "00000115"
        'BarCodeWebControl1.SymbologyType = Symbology.Code128

        '' caption above and below
        'BarCodeWebControl1.CaptionAbove = New Caption("ProjectID")
        'BarCodeWebControl1.CaptionBelow = New Caption("SubjectID")

        'BarCodeWebControl1.CodeLocation = CodeLocation.Below



        'Me.BarCodeWebControl1.xDimension = 0.2
        'Me.BarCodeWebControl1.yDimension = 2
        'Me.BarCodeWebControl1.BarHeight = 6

        'Dim font1 As New Drawing.Font(Drawing.FontFamily.GenericSerif, 8.0!)

        'Me.BarCodeWebControl1.CodeTextFont = font1
        'Me.BarCodeWebControl1.BackColor = Drawing.Color.White
        'Me.BarCodeWebControl1.ForeColor = Drawing.Color.Black
        'Me.BarCodeWebControl1.CodeColor = Drawing.Color.Black
        'Me.BarCodeWebControl1.BorderColor = Drawing.Color.Black
        'Me.BarCodeWebControl1.RotationAngleF = RotationAngle.ZeroDegree
        'Me.BarCodeWebControl1.ImageQuality = ImageQualityMode.Default


        'Dim objStreamReader As StreamReader
        'Dim str As String = ""

        'Dim document1 As New PrintDocument
        'AddHandler document1.PrintPage, New PrintPageEventHandler(AddressOf Me.printDoc_PrintPage)
        'Try

        '    If File.Exists("E:\temp.doc") Then
        '        objStreamReader = New StreamReader("E:\temp.doc")
        '        While Not objStreamReader.EndOfStream

        '            str = objStreamReader.ReadLine()
        '            str = "0000000" + str
        '            str = Right(str, 8)
        '            Me.SetDefaultBarCode(str)

        '            document1.Print()

        '        End While

        '        objStreamReader.Close()
        '        'File.Delete("E:\temp.doc")
        '    End If

        '    'Response.Redirect("http://localhost/Web_Lambda/frmgeneratesamples.aspx", True)
        'Catch exception1 As InvalidPrinterException
        '    'MsgBox("Invalid printer or printer not found!", MsgBoxStyle.OkOnly)
        'End Try

    End Sub

    'Private Sub SetDefaultBarCode(ByVal strBarcode As String)
    '    BarCodeWebControl1.CodeText = strBarcode
    '    BarCodeWebControl1.SymbologyType = Symbology.Code128

    '    ' caption above and below
    '    BarCodeWebControl1.CaptionAbove = New Caption("ProjectID")
    '    BarCodeWebControl1.CaptionBelow = New Caption("SubjectID")

    '    BarCodeWebControl1.CodeLocation = CodeLocation.Below

    '    Me.BarCodeWebControl1.xDimension = 0.2
    '    Me.BarCodeWebControl1.yDimension = 2
    '    Me.BarCodeWebControl1.BarHeight = 6

    '    Dim font1 As New Drawing.Font(Drawing.FontFamily.GenericSerif, 8.0!)

    '    Me.BarCodeWebControl1.CodeTextFont = font1
    '    Me.BarCodeWebControl1.BackColor = Drawing.Color.White
    '    Me.BarCodeWebControl1.ForeColor = Drawing.Color.Black
    '    Me.BarCodeWebControl1.CodeColor = Drawing.Color.Black
    '    Me.BarCodeWebControl1.BorderColor = Drawing.Color.Black
    '    Me.BarCodeWebControl1.RotationAngleF = RotationAngle.ZeroDegree
    '    Me.BarCodeWebControl1.ImageQuality = ImageQualityMode.Default

    'End Sub

    'Private Sub printDoc_PrintPage(ByVal sender As Object, ByVal e As PrintPageEventArgs)
    '    e.Graphics.DrawImage(Me.BarCodeWebControl1.BarCodeImage, 0, 0)
    'End Sub

#End Region

End Class