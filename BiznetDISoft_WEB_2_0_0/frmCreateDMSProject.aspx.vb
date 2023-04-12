
Partial Class frmCreateDMSProject
    Inherits System.Web.UI.Page
#Region "VARIABLE DECLARATIONS"

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Private Const VS_DsProtocol As String = "Ds_Protocol"
    Private Const VS_DtInfo As String = "Dt_Info"
    Private Const VS_WorkSpaceId As String = "vWorkSpaceId"
    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtWorkSpace As String = "DtWorkSpace"
    Private Const VS_DtWSPD As String = "DtWSPDM"
    Private Const VS_DtWSSub As String = "DtWSSub"
    Private Const VS_SubjectNo As String = "SubjectNo"

#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try

            If Not IsPostBack Then
                CType(Me.Master.FindControl("lblHeading"), Label).Text = "Project Details"
                'FillDropDown()
                Choice = Me.Request.QueryString("Mode")
                Me.ViewState(VS_Choice) = Choice   'To use it while saving

                If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                    Me.tdEdit.Visible = False
                    Me.tdContainer.Visible = True
                    GenCall()
                Else
                    Me.tdEdit.Visible = True
                    Me.tdContainer.Visible = False
                End If

            End If
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "GenCall "
    Private Function GenCall() As Boolean
        Dim ds As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try



            Choice = Me.Request.QueryString("Mode")
            Me.ViewState(VS_Choice) = Choice   'To use it while saving

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                'Me.ViewState(VS_WorkSpaceId) = Me.Request.QueryString("Value").ToString
                Me.ViewState(VS_WorkSpaceId) = Me.HWorkspaceId.Value.Trim()
            End If

            ''''Check for Valid User''''''''''''''

            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DsProtocol) = ds   ' adding blank DataTable in viewstate
            Me.ViewState(VS_DtInfo) = CType(Me.ViewState(VS_DsProtocol), DataSet).Tables("workspaceprotocoldetailmatrix")
            Me.ViewState(VS_DtWorkSpace) = CType(Me.ViewState(VS_DsProtocol), DataSet).Tables("workspacemst")
            Me.ViewState(VS_DtWSPD) = CType(Me.ViewState(VS_DsProtocol), DataSet).Tables("workspaceprotocoldetail")

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If

            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall")

        Finally

        End Try

    End Function
#End Region

#Region "GenCall_Data "
    Private Function GenCall_Data(ByRef ds_DWR_Retu As DataSet) As Boolean

        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim Val As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing
        Dim ds_WSSub As New DataSet
        'Dim objFFR As New FFReporting.FFReporting

        Try

            Val = Me.ViewState(VS_WorkSpaceId) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vWorkSpaceId=" + Val.ToString
            End If


            If Not objHelp.getPortocolInfo(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If

            'Added as suggested by Nikur Sir.
            If Not Me.objHelp.GetWorkspaceSubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                            ds_WSSub, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If
            Me.ViewState(VS_DtWSSub) = ds_WSSub.Tables(0)
            '*************************************

            If ds Is Nothing Then
                Throw New Exception(eStr_Retu)
            End If

            If ds.Tables(0).Rows.Count <= 0 And _
             Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Throw New Exception("No Records Found for Selected Operation")
            End If

            ds_DWR_Retu = ds
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")

        Finally

        End Try
    End Function
#End Region

#Region "GenCall_showUI "

    Private Function GenCall_ShowUI() As Boolean
        Dim ds_Protocol As DataSet = Nothing
        Dim dt_Info As New DataTable
        Dim dt_WorkSpace As New DataTable
        Dim dt_WSPD As New DataTable
        Dim dv_Info As New DataView
        Dim dv_WorkSpace As New DataView
        Dim dv_WSPD As New DataView
        Dim dr As DataRow
        ' Dim drM As DataRow
        Dim BaseWorkFolder As String = System.Configuration.ConfigurationManager.AppSettings("BaseWorkFolder")
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try
            Page.Title = " :: New DMS Project Creation ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "New DMS Project Creation"

            ds_Protocol = Me.ViewState(VS_DsProtocol)

            Choice = Me.ViewState("Choice")

            If Not FillDropDown() Then
                Exit Function
            End If


            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                dt_Info = Me.ViewState(VS_DtInfo)
                dt_WorkSpace = Me.ViewState(VS_DtWorkSpace)
                dt_WSPD = Me.ViewState(VS_DtWSPD)

                For Each dr In dt_WorkSpace.Rows
                    'dr("vWorkSpaceId")
                    Me.TxtProject.Value = IIf(dr("vWorkSpaceDesc") Is DBNull.Value, "", dr("vWorkSpaceDesc"))
                    Me.SlcProject.Value = IIf(dr("vProjectTypeCode") Is DBNull.Value, "", dr("vProjectTypeCode"))
                    Me.SlcTemplate.Value = IIf(dr("vTemplateId") Is DBNull.Value, "", dr("vTemplateId"))
                    Me.TxtSubNo.Text = IIf(dt_WSPD.Rows(0)("iNoOfSubjects") Is DBNull.Value, "", dt_WSPD.Rows(0)("iNoOfSubjects"))
                    Me.SlcTemplate.Disabled = True
                Next
                Me.BtnSave.Text = "Update"
                Me.BtnSave.ToolTip = "Update"
            End If

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..GenCall")
        Finally
        End Try
    End Function

#End Region

#Region "FillDropDown"

    Private Function FillDropDown() As Boolean
        Dim ds_Template As New Data.DataSet
        Dim ds_Project As New Data.DataSet
        
        Dim dv_Template As New DataView
        Dim dt_Template As New DataTable
        Dim dv_Project As New DataView

        Dim estr As String = String.Empty
        Dim Wstr_UserId As String = String.Empty
        Dim Wstr_Scope As String = String.Empty

        Try


            'For ScopeManagement on 21-Jan-2009
            'To Get Where condition of ScopeVales( Project Type )
            If Not ObjCommon.GetScopeValueWithCondition(Wstr_Scope) Then
                Exit Function
            End If

            objHelp.GetviewProjectTypeMst(Wstr_Scope, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Project, estr)

            Wstr_Scope += " And vTemplateTypeCode='0001'"
            objHelp.GetViewTemplateMst(Wstr_Scope, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Template, estr)
            '****************************************

            dv_Template = ds_Template.Tables(0).DefaultView
            dv_Template.Sort = "vTemplateDesc"
            dt_Template = dv_Template.ToTable

            If dt_Template.Rows.Count <= 0 Then
                ObjCommon.ShowAlert("There is No Record in TemplateMst for TypeCode='0001'", Me.Page)
                Return False
            End If

            Me.SlcTemplate.DataSource = dt_Template
            Me.SlcTemplate.DataTextField = "vTemplateDesc"
            Me.SlcTemplate.DataValueField = "vTemplateId"
            Me.SlcTemplate.DataBind()
            Me.SlcTemplate.Items.Insert(0, New ListItem("select Template", ""))

            dv_Project = ds_Project.Tables(0).DefaultView
            dv_Project.Sort = "vProjectTypeName"
            Me.SlcProject.DataSource = dv_Project
            Me.SlcProject.DataValueField = "vProjectTypeCode"
            Me.SlcProject.DataTextField = "vProjectTypeName"
            Me.SlcProject.DataBind()
            Me.SlcProject.Items.Insert(0, New ListItem("select Project type", ""))

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..FillDropDown")
            Return False
        End Try
    End Function

#End Region

#Region "Button Events"

    Protected Sub BtnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnEdit.Click
        Try
            
            Me.tdContainer.Visible = True

            GenCall()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......BtnEdit_Click")
        End Try
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub BtnCancle_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancle.Click
        ResetPage()
    End Sub

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim Ds_Save As New DataSet
        Dim Dt_WS As New DataTable
        Dim Dt_WSPD As New DataTable
        Dim Dt_WSPDM As New DataTable
        Dim Dt_WSSub As New DataTable
        Dim Dv_WSPDM As New DataView
        Dim estr As String = String.Empty
        Dim strRequestId As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Choice = Me.ViewState(VS_Choice)
        Try
            If Not AssignValues() Then
                Exit Sub
            End If

            Ds_Save = New DataSet
            Dt_WS = CType(Me.ViewState(VS_DtWorkSpace), DataTable)
            Dt_WS.TableName = "workspacemst"
            Ds_Save.Tables.Add(Dt_WS)

            Dt_WSPD = CType(Me.ViewState(VS_DtWSPD), DataTable)
            Dt_WSPD.TableName = "workspaceprotocoldetail"
            Ds_Save.Tables.Add(Dt_WSPD)

            Dv_WSPDM = CType(Me.ViewState(VS_DtInfo), DataTable).DefaultView
            Dv_WSPDM.RowFilter = "vWorkSpaceId is Null or vWorkSpaceId=''"
            Dt_WSPDM = Dv_WSPDM.ToTable
            'Dt_WSPDM = CType(Me.ViewState(VS_DtInfo), DataTable)
            Dt_WSPDM.TableName = "workspaceprotocoldetailmatrix"
            Ds_Save.Tables.Add(Dt_WSPDM)

            Dt_WSSub = CType(Me.ViewState(VS_DtWSSub), DataTable)
            Dt_WSSub.TableName = "workspacesubjectmst"
            Ds_Save.Tables.Add(Dt_WSSub)


            If Not objLambda.Save_workspacemst(Me.ViewState(VS_Choice), Ds_Save, Me.Session(S_UserID), strRequestId, estr) Then

                ObjCommon.ShowAlert("Error While Saving WorkSpace", Me.Page)
                Exit Sub

            End If

            'Added by Vishal 05-Sep-2008
            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Redirectthispage", "RedirectPage('Project Created Successfully with Request Id -> " + strRequestId + "');", True)
                'ObjCommon.ShowAlert("Project Created Successfully with Request Id -> " + strRequestId, Me.Page)
            ElseIf Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Redirectthispage", "RedirectPage('Project Updated Successfully');", True)
                'ObjCommon.ShowAlert("Project Updated Successfully", Me.Page)
                Me.tdContainer.Visible = False
            End If

            ResetPage()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...BtnSave_Click")
        End Try
      
    End Sub

#End Region

#Region "Assign Values"

    Private Function AssignValues() As Boolean
        Dim dr As DataRow
        Dim drM As DataRow
        Dim dt_WSSub As New DataTable
        Dim dt_WS As New DataTable
        Dim dt_WSPD As New DataTable

        Dim BaseWorkFolder As String = System.Configuration.ConfigurationManager.AppSettings("BaseWorkFolder")
        Dim estr As String = String.Empty

        dt_WS = CType(Me.ViewState(VS_DtWorkSpace), DataTable)
        dt_WSPD = CType(Me.ViewState(VS_DtWSPD), DataTable)
        Try
            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                If Not dt_WS Is Nothing Then 'If dt_WS is nothing then no need to clear it--Added By-Chandresh Vanker
                    dt_WS.Clear()
                End If

                dr = dt_WS.NewRow()
                'vWorkSpaceDesc,vProjectTypeCode,vClientCode,vTemplateId,vBaseWorkFolder,vBasePublishFolder,vLastPublishedVersion,dCreatedOn,cProjectStatus,vRemark
                dr("vWorkSpaceId") = "01"
                dr("vWorkSpaceDesc") = Me.TxtProject.Value.Trim()
                dr("vProjectTypeCode") = Me.SlcProject.Value.Trim()
                dr("vTemplateId") = Me.SlcTemplate.Value.Trim()
                dr("vBaseWorkFolder") = BaseWorkFolder
                dr("vBasePublishFolder") = BaseWorkFolder
                dr("vLastPublishedVersion") = "0000"
                dr("dCreatedOn") = System.DateTime.Now()
                dr("cProjectStatus") = "I"
                dr("vRemark") = "Remarks"
                dr("iModifyBy") = Me.Session(S_UserID)
                dt_WS.Rows.Add(dr)


                dt_WSPD.Clear()
                drM = dt_WSPD.NewRow()
                drM("vProjectNo") = Me.TxtProNo.Value.Trim()
                drM("iNoOfSubjects") = Me.TxtSubNo.Text.Trim()
                drM("iModifyBy") = Me.Session(S_UserID)
                dt_WSPD.Rows.Add(drM)

                If Not AssignSubject(0) Then
                    Return False
                End If

            Else

                If Me.TxtSubNo.Text < dt_WSPD.Rows(0)("iNoOfSubjects") Then
                    ObjCommon.ShowAlert("No Of Subject Should Be Equal To Or Greater Than " + dt_WSPD.Rows(0)("iNoOfSubjects").ToString.Trim(), Me.Page)
                    Exit Function
                End If

                For Each dr In dt_WS.Rows

                    'vWorkSpaceDesc,vProjectTypeCode,vClientCode,vTemplateId,vBaseWorkFolder,vBasePublishFolder,vLastPublishedVersion,dCreatedOn,cProjectStatus,vRemark
                    dr("vWorkSpaceDesc") = Me.TxtProject.Value.Trim()
                    dr("vProjectTypeCode") = Me.SlcProject.Value.Trim()
                    dr("vTemplateId") = Me.SlcTemplate.Value.Trim()
                    dr("vBaseWorkFolder") = BaseWorkFolder
                    dr("vBasePublishFolder") = BaseWorkFolder
                    dr("vLastPublishedVersion") = "0000"
                    dr("dCreatedOn") = System.DateTime.Now()
                    dr("cProjectStatus") = "I"
                    dr("vRemark") = "Remarks"
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr.AcceptChanges()

                Next dr
                dt_WS.AcceptChanges()

                dt_WSSub = CType(Me.ViewState(VS_DtWSSub), DataTable)
                dt_WSSub.Clear()

                If Me.TxtSubNo.Text > dt_WSPD.Rows(0)("iNoOfSubjects") Then
                    If Not AssignSubject(dt_WSPD.Rows(0)("iNoOfSubjects")) Then
                        Return False
                    End If
                End If

                For Each drM In dt_WSPD.Rows
                    drM("vProjectNo") = Me.TxtProNo.Value.Trim()
                    drM("iNoOfSubjects") = Me.TxtSubNo.Text.Trim()
                    drM("iModifyBy") = Me.Session(S_UserID)
                Next
                dt_WSPD.AcceptChanges()

            End If

            Me.ViewState(VS_DtWSPD) = dt_WSPD
            Me.ViewState(VS_DtWorkSpace) = dt_WS

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....AssignValues")
        End Try

       
    End Function

    'Added as suggested by Nikur Sir on 19-Jun-2009
    Private Function AssignSubject(ByVal LastSubNo As Integer) As Boolean
        Dim drS As DataRow
        Dim dt_WSSub As New DataTable
        Dim ds_WSSub As New DataSet
        Dim estr As String = String.Empty

        dt_WSSub = CType(Me.ViewState(VS_DtWSSub), DataTable)
        Try
            dt_WSSub.Clear()
            For index As Integer = LastSubNo To Me.TxtSubNo.Text - 1
                drS = dt_WSSub.NewRow
                drS("vWorkspaceSubjectId") = index + 1
                drS("vWorkspaceId") = "00"
                drS("iMySubjectNo") = index + 1
                drS("vSubjectId") = "0000"
                drS("vInitials") = ""
                drS("iPeriod") = 1
                drS("dReportingDate") = ObjCommon.GetCurDatetimeWithOffSet(Session(S_TimeZoneName))
                drS("cRejectionFlag") = "N"
                drS("iModifyBy") = Me.Session(S_UserID)
                drS("cStatusIndi") = "N"
                dt_WSSub.Rows.Add(drS)
            Next
            dt_WSSub.AcceptChanges()

            Me.ViewState(VS_DtWSSub) = dt_WSSub
            Return True
            '*************************

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....AssignSubject")
        End Try
       
    End Function

#End Region

#Region "Reset Page"

    Private Sub ResetPage()


        Me.SlcProject.SelectedIndex = 0
        Me.SlcTemplate.SelectedIndex = 0

        Me.txtsearch.Text = ""
        Me.TxtProject.Value = ""
        Me.TxtProNo.Value = ""
        Me.TxtSubNo.Text = ""
        Me.BtnSave.Text = "Save"
        Me.BtnSave.ToolTip = "Save"

        ' Me.tdContainer.Visible = False

        Me.ViewState(VS_DtInfo) = Nothing
        Me.ViewState(VS_DsProtocol) = Nothing
        Me.ViewState(VS_WorkSpaceId) = Nothing
        Me.ViewState(VS_DtWSPD) = Nothing
        Me.ViewState(VS_DtWorkSpace) = Nothing

    End Sub

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

End Class
