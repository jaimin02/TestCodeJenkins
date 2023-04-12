Imports System.Net.Http
Imports System.Net.Http.Headers
Imports Newtonsoft.Json

Partial Class frmCreateVisit
    Inherits System.Web.UI.Page

    Private Const VS_DtImport As String = "dtImport"
    Private Const VS_DtSubjectMst As String = "dtSubjectMst"
    Private Const VS_WorkspaceSubjectMst As String = "dtWorkspaceSubjectMst"
    Private Const VS_ImgTransmittalHdr As String = "dtImgTransmittalHdr"
    Private Const VS_View_WorkSpaceNodeDetail As String = "dtView_WorkSpaceNodeDetail"

    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()


#Region "   Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not GenCall_ShowUI(sender, e) Then
                Exit Sub
            End If
        End If
    End Sub

#End Region

#Region "   GenCall_ShowUI"

    Private Function GenCall_ShowUI(ByVal sender As Object, ByVal e As System.EventArgs) As Boolean
        Dim QStr As String = String.Empty
        Dim ds_Workspace As New DataSet

        Try
            Page.Title = ":: Visit Enrollment :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Master.FindControl("lblHeading"), Label).Text = "Visit Enrollment"
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)

            QStr = " Select WorkspaceDefaultWorkflowUserDtl.vWorkspaceId,'[' +vProjectNo+ ']' + vRequestId AS ProjectNo " + vbCrLf +
                   " FROM WorkspaceDefaultWorkflowUserDtl " + vbCrLf +
                   " INNER JOIN view_workspaceprotocoldetail " + vbCrLf +
                   "        ON(WorkspaceDefaultWorkflowUserDtl.vWorkspaceId = view_workspaceprotocoldetail.vWorkspaceId)" + vbCrLf +
                   " WHERE iUserId = " + Me.Session(S_UserID) + " GROUP BY WorkspaceDefaultWorkflowUserDtl.vWorkspaceId,'[' +vProjectNo+ ']' + vRequestId"

            ds_Workspace = Me.objHelp.GetResultSet(QStr, "ImgTransmittalHdr")
            If ds_Workspace.Tables(0).Rows.Count = 1 Then
                HProjectId.Value = ds_Workspace.Tables(0).Rows(0)("vWorkspaceId")
                txtproject.Text = ds_Workspace.Tables(0).Rows(0)("ProjectNo")
                btnSetProject_Click(sender, e)
            ElseIf Not FillGrid() Then
                Throw New Exception("Error While Filling Grid")
            End If

            GenCall_ShowUI = True
        Catch ex As Exception
            objcommon.ShowAlert(ex.Message, Me.Page)

            Return False
        Finally
        End Try
    End Function

#End Region

#Region "   FillGrid"
    Private Function FillGrid() As Boolean
        Dim eStr As String = String.Empty
        Dim ds_Visit As New DataSet
        Dim wStr As String = String.Empty

        Try

            gvVisitList.DataSource = Nothing

            If Not objcommon.GetScopeValueWithCondition(wStr) Then
                Return Nothing
            End If

            If Me.HProjectId.Value.Trim().Length > 0 Then
                wStr += " And WSMst.vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' "
            End If
            wStr = "      SELECT DISTINCT ImgTHdr.vWorkspaceId, WorkSpaceProtocolDetail.vProjectNo, ImgTHdr.vSubjectId, ImgTHdr.vMySubjectNo,WSSmst.vInitials, ImgTHdr.vRandomizationNo, ImgTHdr.vActivityId, " + vbCrLf +
                   " View_WorkSpaceNodeDetail.vNodeDisplayName AS vVisitNo, ImgTHdr.vDOB," + vbCrLf +
                   " (CASE WHEN ImgTHdr.cReviewStatus = 'QCC' THEN 'QC Review Complete' WHEN ImgTHdr.cReviewStatus = 'RAC' THEN 'Grader Review Complete' WHEN ImgTHdr.cReviewStatus = 'R' THEN 'Rejected'" + vbCrLf +
                   " WHEN ImgTDtl.iImgTransmittalHdrId IS NOT NULL THEN 'Image Uploaded' WHEN ImgTHdr.cReviewStatus IS NULL THEN 'Pending' ELSE 'ReUpload' END) AS Status, " + vbCrLf +
                   " ImgTHdr.iNodeId  " + vbCrLf +
                   " FROM ImgTransmittalHdr AS ImgTHdr" + vbCrLf +
                   " INNER JOIN WorkSpaceMst AS WSMst ON (ImgTHdr.vWorkSpaceId = WSMst.vWorkSpaceId)" + vbCrLf +
                   "INNER JOIN View_WorkSpaceNodeDetail As View_WorkSpaceNodeDetail " + vbCrLf +
                   "                    on (ImgTHdr.vWorkSpaceId = View_WorkSpaceNodeDetail.vWorkSpaceId " + vbCrLf +
                   "                        AND ImgTHdr.iNodeId = View_WorkSpaceNodeDetail.iNodeId)" + vbCrLf +
                   "INNER JOIN WorkSpaceProtocolDetail As WorkSpaceProtocolDetail on (WorkSpaceProtocolDetail.vWorkSpaceId = WSMst.vWorkSpaceId)" + vbCrLf +
                   "  INNER JOIN (SELECT DISTINCT vWorkSpaceID  " + vbCrLf +
                   "                   FROM (SELECT WSWFUserDtl.vWorkspaceId " + vbCrLf +
                   "                        FROM WorkSpaceWorkFlowUserDtl AS WSWFUserDtl " + vbCrLf +
                   "                        WHERE WSWFUserDtl.cStatusIndi <> 'D' AND WSWFUserDtl.iUserId = " + Me.Session(S_UserID) + vbCrLf +
                   "                        UNION " + vbCrLf +
                   "                        SELECT WSDefWFUserDtl.vWorkspaceId " + vbCrLf +
                   "                    FROM WorkspaceDefaultWorkflowUserDtl AS WSDefWFUserDtl " + vbCrLf +
                   "                     WHERE WSDefWFUserDtl.cStatusIndi <> 'D' AND WSDefWFUserDtl.iUserId = " + Me.Session(S_UserID) + ") As Src) AS View_PrjNodeRights " + vbCrLf +
                   "                          On (WSMst.vWorkspaceId = View_PrjNodeRights.vWorkspaceId) " + vbCrLf +
                   "                        LEFT JOIN ImgTransmittalDtl AS ImgTDtl ON (ImgTHdr.iImgTransmittalHdrId = ImgTDtl.iImgTransmittalHdrId) " + vbCrLf +
                   "                        LEFT JOIN	WorkspaceSubjectMst WSSmst ON (WSSmst.vWorkspaceId = ImgTHdr.vWorkspaceId AND WSSmst.vSubjectId = ImgTHdr.vSubjectId)" + vbCrLf +
                   "                        WHERE WSMst.cStatusIndi <> 'D' AND imgTHdr.vMySubjectNo NOT IN ('OOOO','') AND ImgTHdr.cStatusIndi <> 'D' And " + wStr + "" + vbCrLf
            ds_Visit = objHelp.GetResultSet(wStr, "CurrentVisitDetails")
            gvVisitList.DataSource = ds_Visit
            gvVisitList.DataBind()

            Return True
        Catch ex As Exception
            objcommon.ShowAlert(ex.Message, Me.Page)

            Return False
        End Try

    End Function

#End Region

#Region "   Site Click"
    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim wStr As String = String.Empty
        Dim ds_Project As New DataSet
        Dim dt As New DataTable
        Try

            wStr = "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "' order by vWorkspaceId asc"

            ds_Project = Me.objHelp.GetResultSet("Select * From View_ChildProject where " + wStr, "View_ChildProject ")
            dt = ds_Project.Tables(0)
            HProjeName.Value = Convert.ToString(dt.Rows(0)("vProjectNo"))
            HExternalProjectNo.Value = Convert.ToString(dt.Rows(0)("vExternalProjectNo"))

            FillGrid()

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Getting Project Details ", ex.Message)
        End Try

    End Sub

#End Region

#Region "   Import/Create Click"
    Protected Sub btnImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImport.Click
        Try

            Me.MpeSubjectMst.Show()
            Import()

        Catch ex As Exception
            Me.objcommon.ShowAlert(ex.Message + vbCrLf + "...........btnImport_Click", Me.Page)
        End Try
    End Sub

    'need to change(change by shyam Kamdar)

    Private Sub Import()
        Dim client As New HttpClient
        Dim response As HttpResponseMessage
        Dim content As FormUrlEncodedContent
        Dim values As Dictionary(Of String, String)
        Dim dsConfig, dsSubject, dsDisplayVisit, dsClounSubject As New DataSet
        Dim dtToken, dtCreateVisit, dtClounSubject As DataTable
        Dim dv As DataView
        Dim APIURL As String
        Dim Token As String

        Try

            dsConfig = Me.objHelp.GetResultSet("Select * from APIConfig", "APIConfig")
            dv = dsConfig.Tables(0).DefaultView
            dv.RowFilter = "vType='TOKEN'"
            If dv.Count <= 0 Then
                Throw New Exception("Config not define for Token.")
            End If

            'Token API
            APIURL = dv(0)("vAPIURL") + dv(0)("vMethodURL")
            client = New HttpClient
            client.BaseAddress = New Uri(APIURL)
            client.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"))
            values = New Dictionary(Of String, String) From {
                                                            {"grant_type", dv(0)("vGrantType")},
                                                            {"username", dv(0)("vUserName")},
                                                            {"password", dv(0)("vPassword")}}

            content = New FormUrlEncodedContent(values)

            response = client.PostAsync(APIURL, content).Result

            If response.StatusCode <> Net.HttpStatusCode.OK Then
                Throw New Exception("No getting proper Response")
            End If

            dtToken = JsonConvert.DeserializeObject("[" + response.Content.ReadAsStringAsync().Result + "]", GetType(DataTable))

            If dtToken.Rows.Count < 0 OrElse Not dtToken.Columns.Contains("access_token") Then
                Throw New Exception("No Response found in proper structure.")
            End If
            Token = dtToken.Rows(0)("access_token")
            'End Token API

            dv = dsConfig.Tables(0).DefaultView
            dv.RowFilter = "vType='VISIT'"
            If dv.Count <= 0 Then
                Throw New Exception("Config not define for create Visit.")
            End If

            'Visit API
            APIURL = dv(0)("vAPIURL") + dv(0)("vMethodURL")
            client = New HttpClient
            'Dim httpMethod = New HttpMethod("GET")
            'Dim requestMessage = New HttpRequestMessage(httpMethod, APIURL)
            client.BaseAddress = New Uri(APIURL)
            client.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"))
            'requestMessage.Content = New ByteArrayContent(Encoding.UTF8.GetBytes("application/x-www-form-urlencoded"))
            'requestMessage.Headers.Add("Authorization", "Bearer " + Token)

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token)
            'client.DefaultRequestHeaders.Authorization = New System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token)
            'client.DefaultRequestHeaders.Add("ProjectNo", Me.HProjeName.Value)
            'client.DefaultRequestHeaders.Add("ChildProjectNo", Me.HExternalProjectNo.Value.Trim())
            'client.DefaultRequestHeaders.Add("TableType", "R")
            'client.DefaultRequestHeaders.Add("FromDate", "01-Oct-2020")
            'client.DefaultRequestHeaders.Add("ToDate", Date.Now.ToString("dd-MMM-yyyy"))

            Dim queryParameters = New Dictionary(Of String, String)
            'queryParameters.Add("ProjectNo", Me.HProjeName.Value)
            'queryParameters.Add("ChildProjectNo", Me.HExternalProjectNo.Value.Trim())
            'queryParameters.Add("TableType", "R")
            'queryParameters.Add("FromDate", "01-Oct-2020")
            'queryParameters.Add("ToDate", Date.Now.ToString("dd-MMM-yyyy"))
            'Dim dictFormUrlEncoded = New FormUrlEncodedContent(queryParameters)
            'Dim queryString = dictFormUrlEncoded.ReadAsStringAsync()

            values = New Dictionary(Of String, String) From {
                                                            {"ProjectNo", Me.HExternalProjectNo.Value.Trim()},
                                                            {"ChildProjectNo", ""},
                                                            {"FromDate", "01-Oct-2020"},
                                                            {"ToDate", Date.Now.ToString("dd-MMM-yyyy")}}

            content = New FormUrlEncodedContent(values)
            'Dim response1 = client.GetAsync(APIURL & "?ProjectNo=" + Me.HProjeName.Value + "&ChildProjectNo=" + Me.HExternalProjectNo.Value.Trim() + "&TableType=R&FromDate=01-Oct-2020&ToDate=" + Date.Now.ToString("dd-MMM-yyyy") + "").Result
            response = client.PostAsync(APIURL, content).Result

            If response.StatusCode <> Net.HttpStatusCode.OK Then
                Throw New Exception("No getting proper Response")
            End If

            dsClounSubject = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result, GetType(DataSet))
            If dsClounSubject.Tables(0).Rows.Count <= 0 Then
                Me.objcommon.ShowAlert("select site does not have data in SmartR...", Me.Page)
                Me.MpeSubjectMst.Hide()
                Exit Sub
            End If
            dtClounSubject = dsClounSubject.Tables(0).DefaultView.ToTable(True, "workSpaceId,ProjectNo,vBizNetProjectNo,vSubjectId,vMySubjectNo,vInitials,dReportingDate,vRandomizationNo,vRandomizationDate,vVisitNo,dVisitDate,cDisContinue,vFirstName,vMiddleName,vSurName".Split(","))
            dsSubject.Tables.Add(dtClounSubject)
            GetVisit(dsDisplayVisit)
            dsDisplayVisit.Tables.Add(dsSubject.Tables(0).Copy)

            dtCreateVisit = dsSubject.Tables(0).Clone()
            For Each dr As DataRow In dsSubject.Tables(0).Rows
                dv = dsDisplayVisit.Tables(0).DefaultView
                dv.RowFilter = "vExternalProjectNo = '" + dr("vBizNetProjectNo") + "' AND " +
                               "vExternalSubjectId = '" + dr("vSubjectId") + "' AND " +
                               "vVisitNo = '" + If(dr("vVisitNo") = "", System.Configuration.ConfigurationManager.AppSettings("ELIGIBILITY_REVIEW"), dr("vVisitNo")) + "'"
                If dv.Count = 0 Then
                    dtCreateVisit.ImportRow(dr)
                End If
            Next dr

            If dtCreateVisit.Rows.Count <= 0 Then
                Me.objcommon.ShowAlert("select site does not have data in SmartR...", Me.Page)
                BtnSaveVisit.Enabled = False
                Exit Sub
            End If
            BtnSaveVisit.Enabled = True
            If dtCreateVisit.Columns.Contains("dtCreateVisit") = False Then
                dtCreateVisit.Columns.Add("cStatusIndi")
            End If

            gvCreateVisit.DataSource = dtCreateVisit
            gvCreateVisit.DataBind()
            'End Visit API

            Me.ViewState(VS_DtImport) = dtCreateVisit
            Me.ViewState(VS_ImgTransmittalHdr) = dsDisplayVisit.Tables(0).Clone()

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "pageLoad", "pageLoad(); ", True)


        Catch ex As Exception
            objcommon.ShowAlert(ex.Message + vbCrLf + "Error while getting Visit Details.", Me.Page)
        Finally
            client.Dispose()
            response = Nothing
            content = Nothing
            values = Nothing
            dsConfig = Nothing
            dsSubject = Nothing
            dsDisplayVisit = Nothing
            dtToken = Nothing
            dtCreateVisit = Nothing
            dv = Nothing
            APIURL = Nothing
            Token = Nothing
        End Try
    End Sub

    'Private Sub Import()
    '    Dim client As New HttpClient
    '    Dim response As HttpResponseMessage
    '    Dim content As FormUrlEncodedContent
    '    Dim values As Dictionary(Of String, String)
    '    Dim dsConfig, dsSubject, dsDisplayVisit, dsClounSubject As New DataSet
    '    Dim dtToken, dtCreateVisit, dtClounSubject As DataTable
    '    Dim dv As DataView
    '    Dim APIURL As String
    '    Dim Token As String

    '    Try

    '        dsConfig = Me.objHelp.GetResultSet("Select * from APIConfig", "APIConfig")
    '        dv = dsConfig.Tables(0).DefaultView
    '        dv.RowFilter = "vType='TOKEN'"
    '        If dv.Count <= 0 Then
    '            Throw New Exception("Config not define for Token.")
    '        End If

    '        'Token API
    '        APIURL = dv(0)("vAPIURL") + dv(0)("vMethodURL")
    '        client = New HttpClient
    '        client.BaseAddress = New Uri(APIURL)
    '        client.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"))
    '        values = New Dictionary(Of String, String) From {
    '                                                        {"grant_type", dv(0)("vGrantType")},
    '                                                        {"username", dv(0)("vUserName")},
    '                                                        {"password", dv(0)("vPassword")}}

    '        content = New FormUrlEncodedContent(values)

    '        response = client.PostAsync(APIURL, content).Result

    '        If response.StatusCode <> Net.HttpStatusCode.OK Then
    '            Throw New Exception("No getting proper Response")
    '        End If

    '        dtToken = JsonConvert.DeserializeObject("[" + response.Content.ReadAsStringAsync().Result + "]", GetType(DataTable))

    '        If dtToken.Rows.Count < 0 OrElse Not dtToken.Columns.Contains("access_token") Then
    '            Throw New Exception("No Response found in proper structure.")
    '        End If
    '        Token = dtToken.Rows(0)("access_token")
    '        'End Token API

    '        dv = dsConfig.Tables(0).DefaultView
    '        dv.RowFilter = "vType='VISIT'"
    '        If dv.Count <= 0 Then
    '            Throw New Exception("Config not define for create Visit.")
    '        End If

    '        'Visit API
    '        APIURL = dv(0)("vAPIURL") + dv(0)("vMethodURL")
    '        client = New HttpClient
    '        client.BaseAddress = New Uri(APIURL)
    '        client.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"))
    '        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token)
    '        values = New Dictionary(Of String, String) From {
    '                                                        {"ProjectNo", ""},
    '                                                        {"ChildProjectNo", Me.HExternalProjectNo.Value.Trim()},
    '                                                        {"TableType", "R"},
    '                                                        {"FromDate", "01-Oct-2020"},
    '                                                        {"ToDate", Date.Now.ToString("dd-MMM-yyyy")}}

    '        content = New FormUrlEncodedContent(values)

    '        response = client.PostAsync(APIURL, content).Result

    '        If response.StatusCode <> Net.HttpStatusCode.OK Then
    '            Throw New Exception("No getting proper Response")
    '        End If

    '        dsClounSubject = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result, GetType(DataSet))
    '        If dsClounSubject.Tables(0).Rows.Count <= 0 Then
    '            Me.objcommon.ShowAlert("select site does not have data in SmartR...", Me.Page)
    '            Me.MpeSubjectMst.Hide()
    '            Exit Sub
    '        End If
    '        dtClounSubject = dsClounSubject.Tables(0).DefaultView.ToTable(True, "workSpaceId,ProjectNo,vBizNetProjectNo,vSubjectId,vMySubjectNo,vInitials,dReportingDate,vRandomizationNo,vRandomizationDate,vVisitNo,dVisitDate,cDisContinue,vFirstName,vMiddleName,vSurName".Split(","))
    '        dsSubject.Tables.Add(dtClounSubject)
    '        GetVisit(dsDisplayVisit)
    '        dsDisplayVisit.Tables.Add(dsSubject.Tables(0).Copy)

    '        dtCreateVisit = dsSubject.Tables(0).Clone()
    '        For Each dr As DataRow In dsSubject.Tables(0).Rows
    '            dv = dsDisplayVisit.Tables(0).DefaultView
    '            dv.RowFilter = "vExternalProjectNo = '" + dr("vBizNetProjectNo") + "' AND " +
    '                           "vExternalSubjectId = '" + dr("vSubjectId") + "' AND " +
    '                           "vVisitNo = '" + If(dr("vVisitNo") = "", System.Configuration.ConfigurationManager.AppSettings("ELIGIBILITY_REVIEW"), dr("vVisitNo")) + "'"
    '            If dv.Count = 0 Then
    '                dtCreateVisit.ImportRow(dr)
    '            End If
    '        Next dr

    '        If dtCreateVisit.Rows.Count <= 0 Then
    '            Me.objcommon.ShowAlert("select site does not have data in SmartR...", Me.Page)
    '            BtnSaveVisit.Enabled = False
    '            Exit Sub
    '        End If
    '        BtnSaveVisit.Enabled = True
    '        If dtCreateVisit.Columns.Contains("dtCreateVisit") = False Then
    '            dtCreateVisit.Columns.Add("cStatusIndi")
    '        End If

    '        gvCreateVisit.DataSource = dtCreateVisit
    '        gvCreateVisit.DataBind()
    '        'End Visit API

    '        Me.ViewState(VS_DtImport) = dtCreateVisit
    '        Me.ViewState(VS_ImgTransmittalHdr) = dsDisplayVisit.Tables(0).Clone()

    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "pageLoad", "pageLoad(); ", True)


    '    Catch ex As Exception
    '        objcommon.ShowAlert(ex.Message + vbCrLf + "Error while getting Visit Details.", Me.Page)
    '    Finally
    '        client.Dispose()
    '        response = Nothing
    '        content = Nothing
    '        values = Nothing
    '        dsConfig = Nothing
    '        dsSubject = Nothing
    '        dsDisplayVisit = Nothing
    '        dtToken = Nothing
    '        dtCreateVisit = Nothing
    '        dv = Nothing
    '        APIURL = Nothing
    '        Token = Nothing
    '    End Try
    'End Sub

    Private Sub GetVisit(ByRef ds_Visit As DataSet)
        Dim wStr As String

        Try
            wStr = "vExternalProjectNo = '" + Me.HExternalProjectNo.Value.Trim() + "'"

            ds_Visit = Me.objHelp.GetResultSet("Select * From View_GetVisit where " + wStr, "ImgTransmittalHdr")


        Catch ex As Exception
            objcommon.ShowAlert(ex.Message + vbCrLf + "Error while getting Visit Details.", Me.Page)
        End Try
    End Sub

#End Region

#Region "   Save Visit"

    Private Sub BtnSaveVisit_Click(sender As Object, e As EventArgs) Handles BtnSaveVisit.Click
        Dim dtVisit, dtSaveVisit As DataTable
        Dim chk As CheckBox
        Dim strSubjectId As String = String.Empty

        Try
            dtVisit = CType(Me.ViewState(VS_DtImport), DataTable)
            dtSaveVisit = dtVisit.Clone()

            For Each gridrow As GridViewRow In gvCreateVisit.Rows
                chk = CType(gridrow.FindControl("chkVisit"), CheckBox)
                If chk.Checked Then
                    For Each dr As DataRow In CType(Me.ViewState(VS_DtImport), DataTable).Select("vSubjectId='" + gvCreateVisit.DataKeys(gridrow.RowIndex).Value.ToString() + "'")
                        dtSaveVisit.ImportRow(dr)
                        strSubjectId += "'" + dr("vSubjectId") + "',"
                    Next dr
                End If
            Next gridrow

            If dtSaveVisit.Rows.Count <= 0 Then
                Me.objcommon.ShowAlert("Please select Visit.", Me.Page)
                Exit Sub
            End If

            If Not GetStructure(strSubjectId.Substring(0, strSubjectId.Length - 1)) Then
                Throw New Exception("Error while GetStructure.")
            End If

            SaveSubjectMst(dtSaveVisit)

        Catch ex As Exception
            Me.objcommon.ShowAlert(ex.Message, Me.Page)
        End Try
    End Sub

    Private Function GetStructure(ByVal strSubjectId As String) As Boolean
        Dim ds_Data As DataSet = Nothing
        Dim eStr = "", wStr As String

        Try
            wStr = "vExternalSubjectId IN(" + strSubjectId + ")"
            If Not objHelp.GetView_SubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                                    ds_Data, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_Data Is Nothing Then
                Throw New Exception(eStr)
            End If

            Me.ViewState(VS_DtSubjectMst) = ds_Data.Tables(0).Copy()
            ds_Data = Nothing

            wStr += " AND vExternalProjectNo = '" + Me.HExternalProjectNo.Value.Trim() + "'"
            If Not objHelp.GetWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition,
                                                                ds_Data, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_Data Is Nothing Then
                Throw New Exception(eStr)
            End If
            Me.ViewState(VS_WorkspaceSubjectMst) = ds_Data.Tables(0).Copy()
            ds_Data = Nothing

            wStr = "select * from View_WorkSpaceNodeDetail where vWorkSpaceId='" + Me.HProjectId.Value.Trim() + "' and iParentNodeId = 1 and iPeriod = 1 and cStatusIndi<>'D'"
            ds_Data = objHelp.GetResultSet(wStr, "View_WorkSpaceNodeDetail")

            If ds_Data Is Nothing Then
                Throw New Exception(eStr)
            End If
            Me.ViewState(VS_View_WorkSpaceNodeDetail) = ds_Data.Tables(0).Copy()

        Catch ex As Exception
            Throw New Exception(ex.Message + vbCrLf + "Error in GetStructure.")
            Return False
        Finally
            ds_Data = Nothing
        End Try

        Return True
    End Function

    Protected Sub SaveSubjectMst(ByVal dtSaveVisit As DataTable)

        Dim Ds_Subjectmst As New DataSet
        Dim eStr As String = String.Empty
        Dim SubjectId As String = String.Empty
        Dim RandomizationNo As String = String.Empty
        Dim mode = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
        Dim Msg As String = String.Empty

        Try

            For Each drVisit In dtSaveVisit.Rows
                If drVisit("cDisContinue") = "N" Then
                    drVisit("cStatusIndi") = "N"
                Else
                    drVisit("cStatusIndi") = "D"
                End If
                ' dtSaveVisit.Rows.Add(drVisit)
            Next drVisit

            If Not AssignValues(dtSaveVisit) Then
                Me.objcommon.ShowAlert("Error While Assigning Data of Subject Master.", Me.Page)
                Exit Sub
            End If

            Ds_Subjectmst.Tables.Add(CType(Me.ViewState(VS_DtSubjectMst), Data.DataTable).Copy())
            Ds_Subjectmst.AcceptChanges()

            If Not Enrollment(dtSaveVisit) Then
                Me.objcommon.ShowAlert("Error while Assigning Data of Workspace Subject Master.", Me.Page)
                Exit Sub
            End If

            Ds_Subjectmst.Tables.Add(CType(Me.ViewState(VS_WorkspaceSubjectMst), DataTable).Copy())
            Ds_Subjectmst.AcceptChanges()

            If Not AssignVisit(dtSaveVisit) Then
                Me.objcommon.ShowAlert("Error while Assigning Data of Workspace Subject Master.", Me.Page)
                Exit Sub
            End If

            Ds_Subjectmst.Tables.Add(CType(Me.ViewState(VS_ImgTransmittalHdr), DataTable).Copy())
            Ds_Subjectmst.AcceptChanges()

            If Not objLambda.Save_ImportSubjectMst(Ds_Subjectmst, Me.Session(S_UserID), eStr) Then
                Throw New Exception(eStr)
            End If

            If Not Me.FillGrid() Then
                Exit Sub
            End If

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "..........BtnSaveSubjectMst_Click")
        End Try

    End Sub

    Private Function AssignValues(ByRef dtSaveVisit As DataTable) As Boolean
        Dim dtOld As New DataTable
        Dim dr As DataRow

        Try

            dtOld = Me.ViewState(VS_DtSubjectMst)

            For Each drVisit In dtSaveVisit.DefaultView.ToTable(True, "vSubjectId,vInitials,vFirstName,vSurName,vMiddleName,vMySubjectNo,dReportingDate,cStatusIndi".Split(",")).Rows

                dtOld.DefaultView.RowFilter = "vExternalSubjectId='" + drVisit("vSubjectId") + "'"
                If dtOld.DefaultView.Count <= 0 Then
                    dr = dtOld.NewRow
                    dtOld.Rows.Add(dr)
                    dr("vSubjectID") = Pro_Screening
                    dr("vExternalSubjectId") = drVisit("vSubjectId")
                Else
                    dr = dtOld.DefaultView.ToTable().Rows(0)
                End If

                dr("vLocationCode") = Me.Session(S_LocationCode)
                dr("dEnrollmentDate") = drVisit("dReportingDate")
                dr("vFirstName") = drVisit("vFirstName").ToString().Trim.ToUpper()
                dr("vSurName") = drVisit("vSurName").ToString().Trim.ToUpper()
                dr("vMiddleName") = drVisit("vMiddleName").ToString().Trim.ToUpper()
                dr("vInitials") = drVisit("vInitials").ToString().ToUpper()
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cSubjectType") = "C"
                dr("nSubjectWorkSpaceNo") = 1
                dr("vWorkSpaceId") = Me.HProjectId.Value.Trim()
                dr("dAllocationDate") = ""
                dr("cStatusIndi") = drVisit("cStatusIndi").ToString().ToUpper()

            Next drVisit

            dtOld.AcceptChanges()
            Me.ViewState(VS_DtSubjectMst) = dtOld

            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....AssignValues")
            Return False
        End Try
    End Function

    Private Function Enrollment(ByVal dtSaveVisit As DataTable) As Boolean
        Dim estr As String = String.Empty
        Dim dr As DataRow
        Dim Dt_WorkspaceSubjectMst As New DataTable
        Dim dsSubjectNo As New DataSet
        Dim SubjectNo As Integer = 0
        Dim wStr As String = String.Empty
        Dim Ds_Check As New DataSet

        Try

            Dt_WorkspaceSubjectMst = CType(Me.ViewState(VS_WorkspaceSubjectMst), DataTable)

            For Each drVisit In dtSaveVisit.DefaultView.ToTable(True, "vBiznetProjectNo,vSubjectId,vInitials,vMySubjectNo,vRandomizationNo,vRandomizationDate".Split(",")).Rows

                Dt_WorkspaceSubjectMst.DefaultView.RowFilter = "vExternalProjectNo='" + drVisit("vBiznetProjectNo") + "' AND vExternalSubjectId='" + drVisit("vSubjectId") + "'"

                If Dt_WorkspaceSubjectMst.DefaultView.Count <= 0 Then

                    AssignELIGIBILITYREVIEWVisit(drVisit)
                    If SubjectNo < 1 Then
                        If Not Me.objHelp.GetFieldsOfTable("WorkspaceSubjectMst", "ISNULL(MAX(iMySubjectNo),0) as MaxSubNo",
                                       "vWorkspaceId = '" + Me.HProjectId.Value.Trim() + "'", dsSubjectNo, estr) Then
                            Throw New Exception(estr)
                        End If

                        If dsSubjectNo.Tables(0).Rows(0)("MaxSubNo") = 0 Then
                            SubjectNo = 1
                        Else
                            SubjectNo = dsSubjectNo.Tables(0).Rows(0)("MaxSubNo") + 1
                        End If
                    Else
                        SubjectNo += 1
                    End If

                    dr = Dt_WorkspaceSubjectMst.NewRow()
                    Dt_WorkspaceSubjectMst.Rows.Add(dr)
                    dr("vWorkspaceSubjectId") = 0
                    dr("vWorkspaceid") = Me.HProjectId.Value.Trim()
                    dr("iMySubjectNo") = SubjectNo
                    dr("vSubjectId") = ""
                    dr("iPeriod") = 1 ' Fixed value 
                    dr("dReportingDate") = objcommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName))  'Its getting saved from Insert Procedure
                    dr("cRejectionFlag") = "N"
                    dr("vExternalProjectNo") = drVisit("vBiznetProjectNo")
                    dr("vExternalSubjectId") = drVisit("vSubjectId")

                Else

                    dr = Dt_WorkspaceSubjectMst.DefaultView(0).Row
                    dr("cScreenFailure") = "NULL"
                    dr("cDisContinue") = "NULL"
                    dr("vScreenFailureRemaks") = DBNull.Value
                    dr("dScreenFailureDate") = DBNull.Value

                End If

                dr("vRandomizationNo") = drVisit("vRandomizationNo").ToString().ToUpper()
                dr("vInitials") = drVisit("vInitials").ToString().ToUpper()
                dr("vMySubjectNo") = drVisit("vMySubjectNo").ToString().ToUpper()
                dr("iTranNo") = 0
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("dICFDate") = System.DateTime.Now
                dr("nWorkspaceSubjectHistoryId") = 0

                Dt_WorkspaceSubjectMst.AcceptChanges()

            Next drVisit


            Me.ViewState(VS_WorkspaceSubjectMst) = Dt_WorkspaceSubjectMst

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...Enrollment")
            Return False
        End Try
    End Function

    Private Function AssignELIGIBILITYREVIEWVisit(ByVal drVisit As DataRow) As Boolean
        Dim estr As String = String.Empty
        Dim dr As DataRow
        Dim Dt_ImgTransmittalHdr, dt_View_WorkSpaceNodeDetail As New DataTable
        Dim dsSubjectNo As New DataSet
        Dim SubjectNo As Integer = 0
        Dim wStr As String = String.Empty
        Dim Ds_Check As New DataSet

        Try

            Dt_ImgTransmittalHdr = CType(Me.ViewState(VS_ImgTransmittalHdr), DataTable)
            Dt_ImgTransmittalHdr.TableName = "ImgTransmittalHdr"
            dt_View_WorkSpaceNodeDetail = CType(Me.ViewState(VS_View_WorkSpaceNodeDetail), DataTable).Copy()

            dt_View_WorkSpaceNodeDetail.DefaultView.RowFilter = "vNodeDisplayName='" + System.Configuration.ConfigurationManager.AppSettings("ELIGIBILITY_REVIEW") + "'"

            dr = Dt_ImgTransmittalHdr.NewRow()
            dr("iImgTransmittalHdrId") = 0
            dr("vParentWorkspaceId") = 1
            dr("vWorkspaceId") = Me.HProjectId.Value.Trim()
            dr("vProjectNo") = HProjeName.Value.Trim()
            dr("vSubjectId") = Pro_Screening
            dr("vMySubjectNo") = drVisit("vMySubjectNo").ToString().ToUpper()
            dr("vDOB") = objcommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName))  'Its getting saved from Insert Procedure
            dr("vParentActivityId") = dt_View_WorkSpaceNodeDetail.Rows(0)("vParentActivityId")
            dr("iParentNodeId") = dt_View_WorkSpaceNodeDetail.Rows(0)("iParentNodeId")
            dr("vActivityId") = dt_View_WorkSpaceNodeDetail.Rows(0)("vActivityId")
            dr("iNodeId") = dt_View_WorkSpaceNodeDetail.Rows(0)("iNodeId")
            dr("nvInstructions") = dt_View_WorkSpaceNodeDetail.Rows(0)("vNodeDisplayName")

            dr("vExternalProjectNo") = drVisit("vBiznetProjectNo")
            dr("vExternalSubjectId") = drVisit("vSubjectId")
            dr("iModifyBy") = Me.Session(S_UserID)
            dr("cReviewStatus") = ""

            Dt_ImgTransmittalHdr.Rows.Add(dr)

            Me.ViewState(VS_ImgTransmittalHdr) = Dt_ImgTransmittalHdr

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...Enrollment")
            Return False
        End Try
    End Function

    Private Function AssignVisit(ByVal dtSaveVisit As DataTable) As Boolean
        Dim estr As String = String.Empty
        Dim dr As DataRow
        Dim Dt_ImgTransmittalHdr, dt_View_WorkSpaceNodeDetail As New DataTable
        Dim dsSubjectNo As New DataSet
        Dim SubjectNo As Integer = 0
        Dim wStr As String = String.Empty
        Dim Ds_Check As New DataSet

        Try

            Dt_ImgTransmittalHdr = CType(Me.ViewState(VS_ImgTransmittalHdr), DataTable)
            Dt_ImgTransmittalHdr.TableName = "ImgTransmittalHdr"
            dt_View_WorkSpaceNodeDetail = CType(Me.ViewState(VS_View_WorkSpaceNodeDetail), DataTable)

            For Each drVisit In dtSaveVisit.DefaultView.ToTable(True, "vBiznetProjectNo,vSubjectId,vMySubjectNo,vRandomizationNo,vVisitNo".Split(",")).Rows

                dt_View_WorkSpaceNodeDetail.DefaultView.RowFilter = "vNodeDisplayName='" + drVisit("vVisitNo") + "'"

                If dt_View_WorkSpaceNodeDetail.DefaultView.Count <= 0 Then
                    Continue For
                End If

                dr = Dt_ImgTransmittalHdr.NewRow()
                dr("iImgTransmittalHdrId") = 0
                dr("vParentWorkspaceId") = 1
                dr("vWorkspaceId") = Me.HProjectId.Value.Trim()
                dr("vProjectNo") = HProjeName.Value.Trim()
                dr("vSubjectId") = ""
                dr("vMySubjectNo") = drVisit("vMySubjectNo").ToString().ToUpper()
                dr("vRandomizationNo") = drVisit("vRandomizationNo").ToString().ToUpper()
                dr("vDOB") = DateTime.Now.ToString("dd-MMM-yyyy")  'Its getting saved from Insert Procedure
                dr("vParentActivityId") = dt_View_WorkSpaceNodeDetail.DefaultView.ToTable().Rows(0)("vParentActivityId")
                dr("iParentNodeId") = dt_View_WorkSpaceNodeDetail.DefaultView.ToTable().Rows(0)("iParentNodeId")
                dr("vActivityId") = dt_View_WorkSpaceNodeDetail.DefaultView.ToTable().Rows(0)("vActivityId")
                dr("iNodeId") = dt_View_WorkSpaceNodeDetail.DefaultView.ToTable().Rows(0)("iNodeId")
                dr("nvInstructions") = drVisit("vVisitNo")

                dr("vExternalProjectNo") = drVisit("vBiznetProjectNo")
                dr("vExternalSubjectId") = drVisit("vSubjectId")
                dr("iModifyBy") = Me.Session(S_UserID)

                Dt_ImgTransmittalHdr.Rows.Add(dr)
            Next drVisit

            Me.ViewState(VS_ImgTransmittalHdr) = Dt_ImgTransmittalHdr

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...AssignVisit")
            Return False
        End Try
    End Function

#End Region

#Region "   Error Handler"
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objcommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objcommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub
#End Region

    Protected Sub btnUpload_Click(sender As Object, e As EventArgs)
        Dim Button As Button = CType(sender, Button)
        Dim index As Integer = Button.CommandArgument
        Dim row As GridViewRow = gvVisitList.Rows(index)
        Dim ProjeName As String = row.Cells(0).Text
        Dim ProjeId As String = gvVisitList.DataKeys(index).Values(0).ToString()
        Dim visitId As String = gvVisitList.DataKeys(index).Values(1).ToString()
        Dim SubjectId As String = gvVisitList.DataKeys(index).Values(2).ToString()
        'string country = GridView1.SelectedRow.Cells[1].Text;
        Response.Redirect(String.Format("frmImageTransmittal.aspx?ProjeName={0}&ProjeId={1}&SubjectId={2}&VisitId={3}", ProjeName, ProjeId, SubjectId, visitId))

    End Sub


End Class
