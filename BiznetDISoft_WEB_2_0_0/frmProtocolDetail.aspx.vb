Partial Class frmProtocolDetail
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
    Private Const Vs_DsPiUser As String = "Ds_PIUser"

#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Dim scopeno As String = String.Empty
        Dim PMScopeNo As String = Scope_SAll.ToString()
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

                If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                    Me.txtsearch.Text = Session(S_ProjectName)
                    Me.HWorkspaceId.Value = Session(S_ProjectId)
                    BtnEdit_Click(sender, e)
                End If



                '==added on 09-may-2011 by Dharmesh H.Salla to show project according to user
                Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)
                '========
                '' === For give all rights to pmadmin ==
                scopeno = Me.Session(S_ScopeNo).ToString()

                If (scopeno = PMScopeNo) Then
                    Me.AutoCompleteExtender1.ServiceMethod = "GetMyProjectCompletionListwithworkspacedesc"
                Else
                    Me.AutoCompleteExtender1.ServiceMethod = "GetMyProjectCompletionList"
                End If
               '========
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "RegularityRequired", "RegularityRequired();", True)

        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "GenCall"
    Private Function GenCall() As Boolean
        Dim ds As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try
            
            Choice = Me.Request.QueryString("Mode")
            Me.ViewState(VS_Choice) = Choice   'To use it while saving
            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
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
            Me.ShowErrorMessage(ex.Message, "....GenCall")

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

        Try

            Val = Me.ViewState(VS_WorkSpaceId) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Request.QueryString("mode") = 1 AndAlso Not Request.QueryString("Workspace") Is Nothing Then
                wStr = "vWorkSpaceId=" + Request.QueryString("Workspace")
            ElseIf Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vWorkSpaceId=" + Val.ToString
            End If


            If Not objHelp.getPortocolInfo(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If

            If ds Is Nothing Then
                Throw New Exception(eStr_Retu)
            End If

            If ds.Tables(0).Rows.Count <= 0 And _
             Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Throw New Exception("No Records Found For Selected Operation")
            End If

            ds_DWR_Retu = ds
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_Data")

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
        Dim drM As DataRow
        Dim BaseWorkFolder As String = System.Configuration.ConfigurationManager.AppSettings("BaseWorkFolder")
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim Ds_WorkspaceSubject As New DataSet
        Dim sender As New Object
        Dim e As New EventArgs

        Try
            Page.Title = ":: Protocol Detail  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "New Request Form"

            ds_Protocol = Me.ViewState(VS_DsProtocol)

            Choice = Me.ViewState("Choice")
            'txtRetentionPeriod.Attributes.Add("onblur", "return chkNumeric();")

            If Not FillDropDown() Then
                Return False
            End If

            If Not CreateTable() Then 'Create Table for WorkSpaceSubjectMaster
                Return False
            End If

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Or (Request.QueryString("mode") = 1 AndAlso Not Request.QueryString("Workspace") Is Nothing) Then
                dt_Info = Me.ViewState(VS_DtInfo)
                dt_WorkSpace = Me.ViewState(VS_DtWorkSpace)
                dt_WSPD = Me.ViewState(VS_DtWSPD)

                For Each dr In dt_WorkSpace.Rows
                    Me.TxtProject.Value = IIf(dr("vWorkSpaceDesc") Is DBNull.Value, "", dr("vWorkSpaceDesc"))
                    Me.SlcProject.SelectedValue = IIf(dr("vProjectTypeCode") Is DBNull.Value, "", dr("vProjectTypeCode"))
                    Me.SlcSponsor.SelectedValue = IIf(dr("vClientCode") Is DBNull.Value, "", dr("vClientCode"))
                    Me.SlcTemplate.Value = IIf(dr("vTemplateId") Is DBNull.Value, "", dr("vTemplateId"))
                    Me.slcLocation.SelectedValue = IIf(dr("vLocationCode") Is DBNull.Value, "", dr("vLocationCode"))
                    Me.SlcPI.Value = IIf(dr("vPiName") Is DBNull.Value, "", dr("vPiName"))
                    Me.SlcService.Value = IIf(dr("vServiceCode") Is DBNull.Value, "", dr("vServiceCode"))

                    SlcProject_SelectedIndexChanged(sender, e)

                    Me.SlcProjectSubType.SelectedValue = IIf(dr("vProjectSubTypeCode") Is DBNull.Value, "", dr("vProjectSubTypeCode"))
                    hdnRegularityReq.Value = IIf(dr("vRRequirement") Is DBNull.Value, "", dr("vRRequirement"))

                    txtIndication.Text = IIf(dr("vIndication") Is DBNull.Value, "", dr("vIndication"))

                    Me.rBtnEthics.SelectedIndex = Me.rBtnEthics.Items.IndexOf(Me.rBtnEthics.Items.FindByText(IIf(dr("vEARequirement") Is DBNull.Value, "", dr("vEARequirement"))))

                    If slcLocation.SelectedValue <> "Select Location" Then
                        slcLocation_SelectedIndexChanged(sender, e)
                        Me.SlcPI.Value = IIf(dr("VPIName") Is DBNull.Value, "", dr("vPIName"))
                    End If

                    wStr = "vWorkspaceId='" + Me.HWorkspaceId.Value.ToString() + "'"
                    If Not objHelp.GetWorkspaceSubjectMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_WorkspaceSubject, eStr) Then
                        Me.ShowErrorMessage(eStr, "")
                    End If
                    If Not Ds_WorkspaceSubject Is Nothing Then
                        If Ds_WorkspaceSubject.Tables(0).Rows.Count > 0 Then
                            If Ds_WorkspaceSubject.Tables(0).Rows.Count = 1 Then
                                If Ds_WorkspaceSubject.Tables(0).Rows(0)("cRejectionFlag") <> "Y" Then
                                    Me.SlcPI.Disabled = True
                                    Me.SlcSponsor.Enabled = False
                                    Me.SlcManager.Disabled = True
                                    Me.ddlStudytype.Enabled = False
                                    Me.slcLocation.Enabled = False
                                    Me.SlcProject.Enabled = False
                                    Me.SlcCoOrdinate.Disabled = True
                                    Me.TxtSubNo.Enabled = False
                                    Me.SlcDrug.Enabled = False
                                    Me.SlcProjectSubType.Enabled = False
                                    Me.SlcService.Disabled = True
                                    Me.txtIndication.Enabled = False
                                    Me.rBtnEthics.Enabled = False

                                Else
                                    Me.SlcPI.Disabled = False
                                    Me.SlcSponsor.Enabled = False
                                    Me.SlcManager.Disabled = False
                                    Me.ddlStudytype.Enabled = True
                                    Me.slcLocation.Enabled = True
                                    Me.SlcProject.Enabled = True
                                    Me.SlcCoOrdinate.Disabled = False
                                    Me.TxtSubNo.Enabled = True
                                    Me.SlcDrug.Enabled = True
                                    Me.SlcProjectSubType.Enabled = False
                                    Me.SlcService.Disabled = False
                                    Me.txtIndication.Enabled = True
                                    Me.rBtnEthics.Enabled = True

                                End If
                            ElseIf Ds_WorkspaceSubject.Tables(0).Rows.Count > 1 Then
                                Me.SlcPI.Disabled = True
                                Me.SlcSponsor.Enabled = False
                                Me.SlcManager.Disabled = True
                                Me.ddlStudytype.Enabled = False
                                Me.slcLocation.Enabled = False
                                Me.SlcProject.Enabled = False
                                Me.SlcCoOrdinate.Disabled = True
                                Me.TxtSubNo.Enabled = False
                                Me.SlcDrug.Enabled = False
                                Me.SlcProjectSubType.Enabled = False
                                Me.SlcService.Disabled = False
                                Me.txtIndication.Enabled = True
                                Me.rBtnEthics.Enabled = True

                            End If
                        End If
                    End If

                    If Request.QueryString("mode") = "1" AndAlso Not Request.QueryString("Workspace") Is Nothing Then
                    Else

                        Me.TxtProject.Disabled = True
                        If dr("cProjectStatus") <> "I" Then
                            Me.txtProNo.Disabled = True
                            Me.BtnGetProNo.Enabled = False
                        End If
                    End If


                    If Request.QueryString("mode") = 1 AndAlso Not Request.QueryString("Workspace") Is Nothing Then

                    Else
                        Me.SlcTemplate.Disabled = True
                        Me.BtnSave.Text = "Update"
                        Me.BtnSave.ToolTip = "Update"
                    End If



                    Me.ddlStudytype.SelectedValue = IIf(dr("nWorkTypeNo") Is System.DBNull.Value, 0, dr("nWorkTypeNo"))
                    If dr("cFastingFed") Is System.DBNull.Value Then
                        Me.ChkfastfedNo.Checked = True
                        Me.ChkfastfedYes.Checked = False
                    Else
                        If dr("cFastingFed") = "FS" Then
                            Me.ChkfastfedYes.Checked = True
                            Me.ChkfastfedNo.Checked = False
                        ElseIf dr("cFastingFed") = "FD" Then
                            Me.ChkfastfedNo.Checked = True
                            Me.ChkfastfedYes.Checked = False
                        End If
                    End If

                    Me.txtRetentionPeriod.Value = IIf(dr("nRetaintionPeriod") Is System.DBNull.Value, "", dr("nRetaintionPeriod"))

                Next

                For Each drM In dt_WSPD.Rows

                    If Not Request.QueryString("mode") = 1 Then
                        Me.txtProNo.Value = IIf(drM("vProjectNo") Is System.DBNull.Value, "", drM("vProjectNo"))
                    End If

                    Me.TxtSponserNo.Value = IIf(drM("vRemarks") Is System.DBNull.Value, "", drM("vRemarks"))
                    Try
                        'Me.SlcDrug.SelectedItem.Text = IIf(drM("vDrugName") Is System.DBNull.Value, 0, drM("vDrugName"))
                        Me.SlcDrug.SelectedItem.Text = IIf(drM("vDrugName") Is System.DBNull.Value, "", drM("vDrugName"))
                        Try
                            Session("vDrugCode") = drM("vDrugCode")
                        Catch ex As Exception
                            Me.SlcDrug.Items.Insert(drM("vDrugCode"), drM("vDrugName"))
                        End Try
                    Catch ex As Exception
                        Me.SlcDrug.Items.Insert(drM("vDrugCode"), drM("vDrugName"))
                    End Try

                    If Ds_WorkspaceSubject.Tables(0).Rows.Count > 0 Then
                        Me.SlcDrug.Enabled = False
                    Else
                        Me.SlcDrug.Enabled = True
                    End If
                    If (Request.QueryString("mode") = 2) Then
                        SlcDrug_SelectedIndexChanged(Nothing, Nothing)
                    End If


                    Me.SlcSubmission.Value = IIf(drM("vSubRegionCode") Is System.DBNull.Value, 0, drM("vSubRegionCode"))
                    Dim ds_Manager1 As DataSet = CType(ViewState("VS_Manager"), DataSet)
                    Dim dv As DataView = ds_Manager1.Tables(0).DefaultView
                    If drM("iProjectManagerId").ToString() <> "" Then
                        dv.RowFilter = "iUserId  = " + drM("iProjectManagerId").ToString()
                    End If
                    Dim ds_Manager As DataTable
                    ds_Manager = dv.ToTable().DefaultView.ToTable()
                    If ds_Manager.DefaultView.Table().Rows.Count = 0 Then
                        Dim ds_newManager = Me.objHelp.GetResultSet("Select iUserId,ProjectManagerWithProfile From View_ProjectManagerForProjectReport Where iUserId =" + drM("iProjectManagerId").ToString() + " order by ProjectManager", "View_ProjectManagerForProjectReport")
                        If Not ds_newManager Is Nothing AndAlso ds_newManager.Tables(0).Rows.Count > 0 Then
                            Me.SlcManager.Items.Insert(0, New ListItem(ds_newManager.Tables(0).Rows(0)("ProjectManagerWithProfile"), ds_newManager.Tables(0).Rows(0)("iUserId")))
                        End If

                    Else
                        Me.SlcManager.Value = IIf(drM("iProjectManagerId") Is System.DBNull.Value, 0, drM("iProjectManagerId"))
                    End If


                    Me.SlcCoOrdinate.Value = IIf(drM("iProjectCoordinatorId") Is System.DBNull.Value, 0, drM("iProjectCoordinatorId"))
                    Me.TxtStudy.Value = IIf(drM("vStudyDesign") Is System.DBNull.Value, "", drM("vStudyDesign"))
                    Me.TxtSubNo.Text = IIf(drM("iNoOfSubjects") Is System.DBNull.Value, "", drM("iNoOfSubjects"))
                    Me.TxtBrand.Value = IIf(drM("vBrandName") Is System.DBNull.Value, "", drM("vBrandName"))
                    Me.TxtGeneric.Value = IIf(drM("vGenericName") Is System.DBNull.Value, "", drM("vGenericName"))
                    Me.TxtStrength.Value = IIf(drM("vStrength") Is System.DBNull.Value, "", drM("vStrength"))
                    Me.TxtFormulation.Value = IIf(drM("vFormulationType") Is System.DBNull.Value, "", drM("vFormulationType"))
                    Me.TxtMfcName.Value = IIf(drM("vManufactureName") Is System.DBNull.Value, "", drM("vManufactureName"))
                    Me.SlcMfcCountry.Value = IIf(drM("vMfCountry") Is System.DBNull.Value, 0, drM("vMfCountry"))
                    Me.TxtMktName.Value = IIf(drM("vMktPersonName") Is System.DBNull.Value, "", drM("vMktPersonName"))
                    Me.SlcMktCountry.Value = IIf(drM("vMktCountryCode") Is System.DBNull.Value, 0, drM("vMktCountryCode"))
                    Me.TxtLot.Value = IIf(drM("vBatchNo") Is System.DBNull.Value, "", drM("vBatchNo"))
                    Me.TxtGenericT.Value = IIf(drM("vTPGenericName") Is System.DBNull.Value, "", drM("vTPGenericName"))
                    Me.TxtStrengthT.Value = IIf(drM("vTPStrength") Is System.DBNull.Value, "", drM("vTPStrength"))
                    Me.TxtFormulationT.Value = IIf(drM("vTPStrength") Is System.DBNull.Value, "", drM("vTPFormulationType"))
                    Me.TxtMfcNameT.Value = IIf(drM("vTPMfName") Is System.DBNull.Value, "", drM("vTPMfName"))
                    Me.SlcMfcCountryT.Value = IIf(drM("vTPMfCountryCode") Is System.DBNull.Value, 0, drM("vTPMfCountryCode"))
                    Me.TxtLotT.Value = IIf(drM("vTPBatchNo") Is System.DBNull.Value, "", drM("vTPBatchNo"))
                    Me.TxtDose.Value = IIf(drM("vDose") Is System.DBNull.Value, "", drM("vDose"))
                    Me.TxtSafty.Value = IIf(drM("vSafetyIssues") Is System.DBNull.Value, "", drM("vSafetyIssues"))
                    Me.txtEthicalRemarks.Text = IIf(drM("vEthicalRemark") Is System.DBNull.Value, "", drM("vEthicalRemark"))
                    If drM("cNewDrugStatus") Is System.DBNull.Value Then
                        Me.ChkDrugNo.Checked = True
                        Me.ChkDrugYes.Checked = False
                    Else
                        If drM("cNewDrugStatus") = "Y" Then
                            Me.ChkDrugYes.Checked = True
                            Me.ChkDrugNo.Checked = False
                        ElseIf drM("cNewDrugStatus") = "N" Then
                            Me.ChkDrugNo.Checked = True
                            Me.ChkDrugYes.Checked = False
                        End If
                    End If

                    If drM("cPermissionRequired") Is System.DBNull.Value Then
                        Me.ChkPermissionNo.Checked = True
                        Me.ChkPermissionYes.Checked = False
                    Else
                        If drM("cPermissionRequired") = "Y" Then
                            Me.ChkPermissionYes.Checked = True
                            Me.ChkPermissionNo.Checked = False
                        ElseIf drM("cPermissionRequired") = "N" Then
                            Me.ChkPermissionNo.Checked = True
                            Me.ChkPermissionYes.Checked = False
                        End If
                    End If

                    Me.txtAnalytical.Value = IIf(drM("vAnalyticalMethod") Is System.DBNull.Value, "", drM("vAnalyticalMethod"))

                    If drM("cMethodReadyFlag") Is System.DBNull.Value Then
                        Me.ChkMethodYes.Checked = True
                        Me.ChkMethodNo.Checked = False
                    Else
                        If drM("cMethodReadyFlag") = "Y" Then
                            Me.ChkMethodYes.Checked = True
                            Me.ChkMethodNo.Checked = False
                            Me.txtReadyStart.Text = "---"
                            Me.txtReadyEnd.Text = "---"
                            Me.txtReadyStart.Enabled = False
                            Me.txtReadyEnd.Enabled = False
                        ElseIf drM("cMethodReadyFlag") = "N" Then
                            Me.ChkMethodNo.Checked = True
                            Me.ChkMethodYes.Checked = False
                            Me.txtReadyStart.Text = IIf(drM("dMethodReadyStartDate") Is System.DBNull.Value, "", String.Format("{0:dd-MMM-yyyy}", drM("dMethodReadyStartDate")))
                            Me.txtReadyEnd.Text = IIf(drM("dMethodReadyEndDate") Is System.DBNull.Value, "", String.Format("{0:dd-MMM-yyyy}", drM("dMethodReadyEndDate")))
                        End If
                    End If

                    If drM("cTestLicenseRequired") Is System.DBNull.Value Then
                        Me.ChkTestNo.Checked = True
                        Me.ChkTestYes.Checked = False
                    Else
                        If drM("cTestLicenseRequired") = "Y" Then
                            Me.ChkTestYes.Checked = True
                            Me.ChkTestNo.Checked = False
                        ElseIf drM("cTestLicenseRequired") = "N" Then
                            Me.ChkTestNo.Checked = True
                            Me.ChkTestYes.Checked = False
                        End If
                    End If

                Next


                'Contect Grids
                If Not IsNothing(dt_Info) Then
                    dv_Info = dt_Info.DefaultView
                    dv_Info.RowFilter = "vType = 'SA' and cStatusIndi<>'D'"
                    Me.GV_Authorised.DataSource = dv_Info.ToTable
                    Me.GV_Authorised.DataBind()

                    dv_Info = dt_Info.DefaultView
                    dv_Info.RowFilter = "vType = 'SM' and cStatusIndi<>'D'"
                    Me.GV_Medical.DataSource = dv_Info.ToTable
                    Me.GV_Medical.DataBind()

                    dv_Info = dt_Info.DefaultView
                    dv_Info.RowFilter = "vType = 'SC' and cStatusIndi<>'D'"
                    Me.GV_Contact.DataSource = dv_Info.ToTable
                    Me.GV_Contact.DataBind()
                    '***************************
                End If

            End If

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "getData", "getData();", True)

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_ShowUI")
        Finally
        End Try
    End Function

#End Region

#Region "FillDropDown"

    Private Function FillDropDown() As Boolean
        Dim ds_Template As New Data.DataSet
        Dim ds_Project As New Data.DataSet
        Dim ds_PIUser As New Data.DataSet
        Dim ds_Client As New Data.DataSet
        Dim ds_User As New Data.DataSet
        Dim ds_Drug As New Data.DataSet
        Dim ds_region As New Data.DataSet
        Dim ds_Location As New Data.DataSet
        Dim ds_WorktypeMst As New Data.DataSet
        Dim dv_Template As New DataView
        Dim dt_Template As New DataTable
        Dim dt_PiUser As New DataTable
        Dim estr As String = String.Empty
        Dim Wstr_UserId As String = String.Empty
        Dim Wstr_Scope As String = String.Empty
        Dim dv_Project As New DataView
        Dim dv_Client As New DataView
        Dim dv_User As New DataView
        Dim dv_Drug As New DataView
        Dim dv_region As New DataView
        Dim dv_Location As New DataView
        Dim dv_WorktypeMSt As New DataView
        Dim Ds_UserCopy As New DataSet
        Dim ds_Service As New Data.DataSet
        Dim dv_Service As New DataView
        Dim dsProjectMngr As New DataSet
        Try

            objHelp.getWorkTypeMst("cReplicaFlag<>'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                    ds_WorktypeMst, estr)
            objHelp.getclientmst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_Client, estr)
            objHelp.getdrugmst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_Drug, estr)
            objHelp.getregionmst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_region, estr)
            objHelp.GetServiceDetail("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_Service, estr)  ''Added By Ketan

            objHelp.getLocationMst("cLocationType = 'L' And cStatusIndi <> 'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                    ds_Location, estr)

            'For ScopeManagement on 21-Jan-2009
            'To Get Where condition of ScopeVales( Project Type )
            If Not ObjCommon.GetScopeValueWithCondition(Wstr_Scope) Then
                Exit Function
            End If

            objHelp.GetviewProjectTypeMst(Wstr_Scope, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Project, estr)
            objHelp.GetViewTemplateMst(Wstr_Scope, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Template, estr)
            objHelp.GetViewUserMst("nScopeNo=" & Me.Session(S_ScopeNo) & " And cStatusindi<>'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_User, estr)
            dsProjectMngr = Me.objHelp.GetResultSet("Select iUserId,ProjectManagerWithProfile From View_ProjectManager order by ProjectManager", "View_ProjectManager")
            '****************************************
            dv_Template = ds_Template.Tables(0).DefaultView

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                dv_Template.RowFilter = "vTemplateTypeCode='0001'"
            End If

            dv_Template.Sort = "vTemplateDesc"
            dt_Template = dv_Template.ToTable
            If dt_Template.Rows.Count <= 0 Then
                ObjCommon.ShowAlert("There is No Record in TemplateMst for TypeCode='0001'", Me.Page)
            Else
                Me.SlcTemplate.DataSource = dt_Template
                Me.SlcTemplate.DataTextField = "vTemplateDesc"
                Me.SlcTemplate.DataValueField = "vTemplateId"
                Me.SlcTemplate.DataBind()
                Me.SlcTemplate.Items.Insert(0, New ListItem("Select Template", ""))

                'added on 26-feb-10 for tooltip
                For iSlcTemplate As Integer = 0 To SlcTemplate.Items.Count - 1
                    SlcTemplate.Items(iSlcTemplate).Attributes.Add("title", SlcTemplate.Items(iSlcTemplate).Text)
                Next
                '=========
            End If
            dv_Project = ds_Project.Tables(0).DefaultView
            dv_Project.Sort = "vProjectTypeName"
            Me.SlcProject.DataSource = dv_Project
            Me.SlcProject.DataValueField = "vProjectTypeCode"
            Me.SlcProject.DataTextField = "vProjectTypeName"
            Me.SlcProject.DataBind()
            Me.SlcProject.Items.Insert(0, New ListItem("Select Project type", ""))

            'added on 26-feb-10 for tooltip
            For iSlcProject As Integer = 0 To SlcProject.Items.Count - 1
                SlcProject.Items(iSlcProject).Attributes.Add("title", SlcProject.Items(iSlcProject).Text)
            Next
            '=========
            dv_Client = ds_Client.Tables(0).DefaultView
            dv_Client.Sort = "vClientName"
            Me.SlcSponsor.DataSource = dv_Client
            Me.SlcSponsor.DataValueField = "vClientCode"
            Me.SlcSponsor.DataTextField = "vClientName"
            Me.SlcSponsor.DataBind()
            Me.SlcSponsor.Items.Insert(0, New ListItem("Select Sponsor", ""))

            'added on 26-feb-10 for tooltip
            For iSlcSponsor As Integer = 0 To SlcSponsor.Items.Count - 1
                SlcSponsor.Items(iSlcSponsor).Attributes.Add("title", SlcSponsor.Items(iSlcSponsor).Text)
            Next
            '=========
            dv_Service = ds_Service.Tables(0).DefaultView
            dv_Service.Sort = "vServiceName"
            Me.SlcService.DataSource = dv_Service
            Me.SlcService.DataValueField = "vServiceCode"
            Me.SlcService.DataTextField = "vServiceName"
            Me.SlcService.DataBind()
            Me.SlcService.Items.Insert(0, New ListItem("Select Service", ""))

            For iSlcService As Integer = 0 To SlcService.Items.Count - 1
                SlcService.Items(iSlcService).Attributes.Add("title", SlcService.Items(iSlcService).Text)
            Next

            dv_Location = ds_Location.Tables(0).DefaultView
            dv_Location.Sort = "vLocationName"
            Me.slcLocation.DataSource = dv_Location
            Me.slcLocation.DataValueField = "vLocationCode"
            Me.slcLocation.DataTextField = "vLocationName"
            Me.slcLocation.DataBind()
            Me.slcLocation.Items.Insert(0, New ListItem("Select Location", ""))

            For islcLocation As Integer = 0 To slcLocation.Items.Count - 1
                slcLocation.Items(islcLocation).Attributes.Add("title", slcLocation.Items(islcLocation).Text)
            Next

            dv_WorktypeMSt = ds_WorktypeMst.Tables(0).DefaultView
            dv_WorktypeMSt.Sort = "vWorkTypeDesc"
            Me.ddlStudytype.DataSource = dv_WorktypeMSt
            Me.ddlStudytype.DataTextField = "vWorkTypeDesc"
            Me.ddlStudytype.DataValueField = "nWorkTypeNo"
            Me.ddlStudytype.DataBind()
            Me.ddlStudytype.Items.Insert(0, New ListItem("Select StudyType", "0"))

            'For tooltip
            For iddlStudytype As Integer = 0 To ddlStudytype.Items.Count - 1
                ddlStudytype.Items(iddlStudytype).Attributes.Add("title", ddlStudytype.Items(iddlStudytype).Text)
            Next
            '=========
            ''===========Project Manager
            ViewState("VS_Manager") = dsProjectMngr
            Me.SlcManager.DataSource = dsProjectMngr
            Me.SlcManager.DataValueField = "iUserId"
            Me.SlcManager.DataTextField = "ProjectManagerWithProfile"
            Me.SlcManager.DataBind()
            Me.SlcManager.Items.Insert(0, New ListItem("Select Manager", "0"))

            For iSlcManager As Integer = 0 To SlcManager.Items.Count - 1
                SlcManager.Items(iSlcManager).Attributes.Add("title", SlcManager.Items(iSlcManager).Text)
            Next
            ''==========

            If Not ds_User.Tables(0).Rows.Count <= 0 Then

                Ds_UserCopy = ds_User

                For CntOfDs_User As Integer = 0 To Ds_UserCopy.Tables(0).Rows.Count - 1
                    Ds_UserCopy.Tables(0).Rows(CntOfDs_User).Item("vUserName") = Ds_UserCopy.Tables(0).Rows(CntOfDs_User).Item("vUserName").ToString() + "   " + "(" + Ds_UserCopy.Tables(0).Rows(CntOfDs_User).Item("vUserTypeName").ToString() + ")"
                Next CntOfDs_User

                Ds_UserCopy.AcceptChanges()


                dv_User = Ds_UserCopy.Tables(0).DefaultView

                dv_User.Sort = "vUserName"

                Me.SlcCoOrdinate.DataSource = dv_User
                Me.SlcCoOrdinate.DataValueField = "iUserId"
                Me.SlcCoOrdinate.DataTextField = "vUserName"
                Me.SlcCoOrdinate.DataBind()
                Me.SlcCoOrdinate.Items.Insert(0, New ListItem("Select Coordinater", "0"))

                For iSlcCoOrdinate As Integer = 0 To SlcCoOrdinate.Items.Count - 1
                    SlcCoOrdinate.Items(iSlcCoOrdinate).Attributes.Add("title", SlcCoOrdinate.Items(iSlcCoOrdinate).Text)
                Next

            End If

            If Not ds_Drug.Tables(0).Rows.Count <= 0 Then
                dv_Drug = ds_Drug.Tables(0).DefaultView
                dv_Drug.Sort = "vDrugName"
                Me.SlcDrug.DataSource = dv_Drug
                Me.SlcDrug.DataValueField = "vDrugCode"
                Me.SlcDrug.DataTextField = "vDrugName"
                Me.SlcDrug.DataBind()
                Me.SlcDrug.Items.Insert(0, New ListItem("Select Drug", ""))

                'For tooltip
                For iSlcDrug As Integer = 0 To SlcDrug.Items.Count - 1
                    SlcDrug.Items(iSlcDrug).Attributes.Add("title", SlcDrug.Items(iSlcDrug).Text)
                Next
                '=========

            End If

            If Not ds_region.Tables(0).Rows.Count <= 0 Then
                dv_region = ds_region.Tables(0).DefaultView
                dv_region.Sort = "vRegionName"
                Me.SlcSubmission.DataSource = dv_region
                Me.SlcSubmission.DataValueField = "vRegionCode"
                Me.SlcSubmission.DataTextField = "vRegionName"
                Me.SlcSubmission.DataBind()
                Me.SlcSubmission.Items.Insert(0, New ListItem("Select Submission", ""))

                'added on 26-feb-10 for tooltip
                For iSlcSubmission As Integer = 0 To SlcSubmission.Items.Count - 1
                    SlcSubmission.Items(iSlcSubmission).Attributes.Add("title", SlcSubmission.Items(iSlcSubmission).Text)
                Next
                '=========


                Me.SlcMfcCountry.DataSource = dv_region
                Me.SlcMfcCountry.DataValueField = "vRegionCode"
                Me.SlcMfcCountry.DataTextField = "vRegionName"
                Me.SlcMfcCountry.DataBind()
                Me.SlcMfcCountry.Items.Insert(0, New ListItem("Select Country", ""))

                'added on 26-feb-10 for tooltip
                For iSlcMfcCountry As Integer = 0 To SlcMfcCountry.Items.Count - 1
                    SlcMfcCountry.Items(iSlcMfcCountry).Attributes.Add("title", SlcMfcCountry.Items(iSlcMfcCountry).Text)
                Next
                '=========


                Me.SlcMfcCountryT.DataSource = dv_region
                Me.SlcMfcCountryT.DataValueField = "vRegionCode"
                Me.SlcMfcCountryT.DataTextField = "vRegionName"
                Me.SlcMfcCountryT.DataBind()
                Me.SlcMfcCountryT.Items.Insert(0, New ListItem("Select Country", ""))

                'added on 26-feb-10 for tooltip
                For iSlcMfcCountryT As Integer = 0 To SlcMfcCountryT.Items.Count - 1
                    SlcMfcCountryT.Items(iSlcMfcCountryT).Attributes.Add("title", SlcMfcCountryT.Items(iSlcMfcCountryT).Text)
                Next
                '=========


                Me.SlcMktCountry.DataSource = dv_region
                Me.SlcMktCountry.DataValueField = "vRegionCode"
                Me.SlcMktCountry.DataTextField = "vRegionName"
                Me.SlcMktCountry.DataBind()
                Me.SlcMktCountry.Items.Insert(0, New ListItem("Select Country", ""))

                'added on 26-feb-10 for tooltip
                For iSlcMktCountry As Integer = 0 To SlcMktCountry.Items.Count - 1
                    SlcMktCountry.Items(iSlcMktCountry).Attributes.Add("title", SlcMktCountry.Items(iSlcMktCountry).Text)
                Next
                '=========
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillDropDown")
            Return False
        End Try
    End Function

    Private Function CreateTable() As Boolean
        Dim Dt_WSSub As New DataTable
        Dim dc As DataColumn
        Try

            dc = New DataColumn
            dc.ColumnName = "vWorkspaceId"
            dc.DataType = GetType(String)
            Dt_WSSub.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "NoOfSubjects"
            dc.DataType = GetType(Integer)
            Dt_WSSub.Columns.Add(dc)

            dc = New DataColumn
            dc.ColumnName = "iModifyBy"
            dc.DataType = GetType(Integer)
            Dt_WSSub.Columns.Add(dc)

            Me.ViewState(VS_DtWSSub) = Dt_WSSub
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "....CreateTable")
            Return False
        End Try
    End Function

#End Region

#Region "Drop Down Events" '' Added By dipen shah on 10-Dec-2014 to fill pi drop down list location wise.
    Protected Sub slcLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim dt_PiUser As New DataTable
        Dim ds_PiUser As New DataSet
        Dim eStr As String = String.Empty
        Try

            If slcLocation.SelectedIndex <> 0 Then
                If Not objHelp.getuserMst("vUserTypeCode=0024 And vLocationCode =" & Me.slcLocation.SelectedValue.ToString.Trim() & "AND cStatusIndi <> 'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_PiUser, eStr) Then '' Added By Dipen Shah to fetched location wise prinical investigator in drop down list.
                    Me.ShowErrorMessage("", "Error While getUserMst")
                End If

                If Not ds_PiUser Is Nothing AndAlso ds_PiUser.Tables(0).Rows.Count > 0 Then
                    dt_PiUser = ds_PiUser.Tables(0)
                    dt_PiUser.Columns.Add("FullName", GetType(String), "vFirstName + ' ' + vLastName")
                    Me.SlcPI.DataSource = dt_PiUser
                    Me.SlcPI.DataValueField = "iUserid"
                    Me.SlcPI.DataTextField = "FullName"
                    Me.SlcPI.DataBind()
                    Me.SlcPI.Items.Insert(0, New ListItem("Select Principal Investigator", ""))
                Else
                    ObjCommon.ShowAlert("No Records found", Me.Page)
                End If
            Else
                ObjCommon.ShowAlert("Select Location.", Me.Page)
                Me.SlcPI.Items.Clear()
            End If
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "onLoad", "onLoad();", True)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".........Error While slcLocation_SelectedIndexChanged()")
        End Try
    End Sub

    Protected Sub SlcProject_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SlcProject.SelectedIndexChanged

        Dim dt_SubType As New DataTable
        Dim ds_SubType As New DataSet
        Dim eStr As String = String.Empty

        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty

        Try

            If SlcProject.SelectedIndex <> 0 Then

                wStr = "vProjectTypeCode=" + Me.SlcProject.SelectedValue.ToString.Trim() + " And vProjectSubTypeName <> '' "
                If Not objHelp.GetviewProjectSubTypeMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubType, eStr_Retu) Then
                    Response.Write(eStr_Retu)
                    Exit Sub
                End If

                If Not ds_SubType Is Nothing AndAlso ds_SubType.Tables(0).Rows.Count > 0 Then
                    dt_SubType = ds_SubType.Tables(0)
                    Me.SlcPRojectSubType.DataSource = dt_SubType
                    Me.SlcPRojectSubType.DataValueField = "vProjectSubTypeCode"
                    Me.SlcPRojectSubType.DataTextField = "vProjectSubTypeName"
                    Me.SlcPRojectSubType.DataBind()
                    Me.SlcPRojectSubType.Items.Insert(0, New ListItem("Select Project Sub Type", ""))
                Else
                    Me.SlcProjectSubType.Items.Clear()
                    ObjCommon.ShowAlert("Project Sub Type is Not Found", Me.Page)
                End If

            Else
                ObjCommon.ShowAlert("Select Project Type.", Me.Page)
                Me.SlcPRojectSubType.Items.Clear()
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".........Error While slcLocation_SelectedIndexChanged()")
        End Try
    End Sub
#End Region

#Region "Button Events"

    Protected Sub BtnAAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAAdd.ServerClick
        Dim dv_info As New DataView
        AssignValue("SA")
        dv_info = CType(Me.ViewState(VS_DtInfo), DataTable).DefaultView
        dv_info.RowFilter = "vType = 'SA' and cStatusIndi<>'D'"
        Me.GV_Authorised.DataSource = dv_info.ToTable
        Me.GV_Authorised.DataBind()

        Me.TxtAAddr1.Value = ""
        Me.TxtAAddr2.Value = ""
        Me.TxtAAddr3.Value = ""
        Me.TxtADesig.Value = ""
        Me.TxtAExt.Value = ""
        Me.TxtAName.Value = ""
        Me.TxtAQuali.Value = ""
        Me.TxtATel.Value = ""
        hdnPostBack.Value = "false"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SlideToggle for btnaadd", " SlideToggle(ctl00_CPHLAMBDA_TabContainer1_TabPanel1_imgAuthorised, tblAuthorised)", True)

    End Sub

    Protected Sub BtnMAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnMAdd.ServerClick
        Dim dv_info As New DataView
        AssignValue("SM")
        dv_info = CType(Me.ViewState(VS_DtInfo), DataTable).DefaultView
        dv_info.RowFilter = "vType = 'SM' and cStatusIndi<>'D'"
        Me.GV_Medical.DataSource = dv_info.ToTable
        Me.GV_Medical.DataBind()

        Me.TxtMAddr1.Value = ""
        Me.TxtMAddr2.Value = ""
        Me.TxtMAddr3.Value = ""
        Me.TxtMDesig.Value = ""
        Me.TxtMExt.Value = ""
        Me.TxtMName.Value = ""
        Me.TxtMQuali.Value = ""
        Me.TxtMTel.Value = ""
        hdnPostBack.Value = "false"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SlideToggle for btnaadd", " SlideToggle(ctl00_CPHLAMBDA_TabContainer1_TabPanel1_imgMedical1, tblMedical1)", True)

    End Sub

    Protected Sub BtnCAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCAdd.ServerClick
        Dim dv_info As New DataView
        AssignValue("SC")
        dv_info = CType(Me.ViewState(VS_DtInfo), DataTable).DefaultView
        dv_info.RowFilter = "vType = 'SC' and cStatusIndi<>'D'"
        Me.GV_Contact.DataSource = dv_info.ToTable
        Me.GV_Contact.DataBind()

        Me.TxtCAddr1.Value = ""
        Me.TxtCAddr2.Value = ""
        Me.TxtCAddr3.Value = ""
        Me.TxtCExt.Value = ""
        Me.TxtCFax.Value = ""
        Me.TxtCTel.Value = ""
        hdnPostBack.Value = "false"
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SlideToggle for btnaadd", " SlideToggle(ctl00_CPHLAMBDA_TabContainer1_TabPanel1_imgContact, tblContact)", True)

    End Sub

    Protected Sub BtnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnEdit.Click
        Try

            Me.tdContainer.Visible = True
            GenCall()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....BtnEdit_Click")
        End Try
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("frmMainPage.aspx", False)
    End Sub

    Protected Sub BtnCancle_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancle.Click
        ResetPage()
    End Sub

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim Ds_Save As New DataSet
        Dim dsDefaultRights As New DataSet
        Dim dsWorkspace As New DataSet
        Dim Dt_WS As New DataTable
        Dim Dt_WSPD As New DataTable
        Dim Dt_WSPDM As New DataTable
        Dim Dt_WSSub As New DataTable
        Dim Dv_WSPDM As New DataView
        Dim estr As String = String.Empty
        Dim strRequestId As String = String.Empty
        Dim wstr As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim dr As DataRow


        Choice = Me.ViewState(VS_Choice)

        ' Validation : Start
        'If Me.SlcTemplate.SelectedIndex <= 0 Then
        '    ObjCommon.ShowAlert("Please Select Template", Me.Page())
        '    Exit Sub
        'ElseIf Me.SlcProject.SelectedIndex <= 0 Then
        '    ObjCommon.ShowAlert("Please Select Project Type", Me.Page())
        '    Exit Sub
        'ElseIf Me.ddlStudytype.SelectedIndex <= 0 Then
        '    ObjCommon.ShowAlert("Please Select Study Type", Me.Page())
        '    Exit Sub
        'ElseIf Me.SlcSponsor.SelectedIndex <= 0 Then
        '    ObjCommon.ShowAlert("Please Select Sponsor", Me.Page())
        '    Exit Sub
        'ElseIf Me.slcLocation.SelectedIndex <= 0 Then
        '    ObjCommon.ShowAlert("Please Select Location", Me.Page())
        '    Exit Sub
        'ElseIf Me.SlcDrug.SelectedIndex <= 0 Then
        '    ObjCommon.ShowAlert("Please Select Drug", Me.Page())
        '    Exit Sub
        'End If
        ' Validation : End


        'If Not Validation() Then
        'Exit Sub
        'End If
        '********************

        If Not Save_AssignValues() Then
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

        'Dt_WSSub = CType(Me.ViewState(VS_DtWSSub), DataTable)
        'Dt_WSSub.TableName = "workspacesubjectmst"
        'Ds_Save.Tables.Add(Dt_WSSub)

        If Not objLambda.Save_workspacemst(Me.ViewState(VS_Choice), Ds_Save, Me.Session(S_UserID), strRequestId, estr) Then

            ObjCommon.ShowAlert("Error While Saving WorkSpace", Me.Page)
            Exit Sub

        End If

        'Added by Vishal 05-Sep-2008
        If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

            '===========added by deepak singh on 19-09-09=======
            wstr = "vWorkspaceId In (Select vWorkspaceId From WorkspaceMst Where vRequestId='" & strRequestId & _
                    "') And iUserId=" & Me.Session(S_UserID)

            If Not objHelp.GetWorkspaceDefaultWorkFlowUserDtl(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                              dsDefaultRights, estr) Then

            End If

            If dsDefaultRights.Tables(0).Rows.Count <= 0 Then

                wstr = "vWorkspaceId In ( Select vWorkspaceId From WorkspaceMst Where vRequestId='" & strRequestId & "')"

                If Not objHelp.getworkspacemst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                      dsWorkspace, estr) Then

                End If


                dr = dsDefaultRights.Tables(0).NewRow
                'nWorkspaceDefaultWorkflowUserId,vWorkspaceId,iUserId,iStageId,iModifyBy,dModifyOn,cStatusIndi
                dr("nWorkspaceDefaultWorkflowUserId") = 0
                dr("vWorkspaceId") = dsWorkspace.Tables(0).Rows(0).Item("vWorkspaceId")
                dr("iUserId") = Me.Session(S_UserID)
                dr("iStageId") = Stage_Created
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dsDefaultRights.Tables(0).Rows.Add(dr)
                dsDefaultRights.Tables(0).AcceptChanges()
                dsDefaultRights.Tables(0).TableName = "View_WorkspaceDefaultWorkFlowUserDtl"
                'Ds_Save.Tables.Add(dt_Save.Copy())

                If Not objLambda.Save_WorkspaceDefaultWorkFlowUserDtl(Me.ViewState(VS_Choice), dsDefaultRights, Me.Session(S_UserID), estr) Then
                    Me.ObjCommon.ShowAlert("Error While Saving WorkspaceDefaultWorkFlowUserDtl", Me.Page)
                    Exit Sub

                End If

            End If
            '*******************************************************
            ' ResetPage()
            'GenCall()
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Redirectthispage", "RedirectPage('Project Created Successfully With Request Id -> " + strRequestId + "');", True)
            'ObjCommon.ShowAlert("Project Created Successfully with Request Id -> " + strRequestId, Me.Page)
        ElseIf Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then

            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Redirectthispage", "RedirectPage('Project Updated Successfully');", True)
            'ObjCommon.ShowAlert("Project Updated Successfully", Me.Page)
            Me.tdContainer.Visible = False
            'ResetPage()

        End If
        'ResetPage()


    End Sub

    Protected Sub BtnGetProNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGetProNo.Click

        Dim wStr As String = String.Empty
        Dim eStr_Retu As String = String.Empty
        Dim ds As DataSet = Nothing
        Dim ds_MaxNo As New DataSet
        Dim Max As Integer = 0
        Dim ProjNo As String = String.Empty

        If Not Me.objHelp.GetProjectNo(ProjNo, eStr_Retu) Then
            Me.ObjCommon.ShowAlert("Problem While Getting Project No.", Me.Page)
            Exit Sub
        End If
        Me.txtProNo.Value = ProjNo.ToString
        hdnPostBack.Value = "false"
        UpdatePanel3.Update()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SlideToggle for btnaadd", " SlideToggle(ctl00_CPHLAMBDA_TabContainer1_TabPanel1_imgRefProduct, tblRefProduct)", True)

    End Sub

#End Region

#Region "Validation"

    'Private Function Validation() As Boolean
    '    If Me.SlcProject.SelectedIndex = 0 Then
    '        Me.ObjCommon.ShowAlert("Please Select Project Type", Me.Page)
    '        Return False
    '    ElseIf SlcTemplate.SelectedIndex = 0 Then
    '        Me.ObjCommon.ShowAlert("Please Select Template", Me.Page)
    '        Return False
    '    ElseIf SlcManager.SelectedIndex = 0 Then
    '        Me.ObjCommon.ShowAlert("Please Select Manager", Me.Page)
    '        Return False
    '    ElseIf SlcSponsor.SelectedIndex = 0 Then
    '        Me.ObjCommon.ShowAlert("Please Select Sponsor", Me.Page)
    '        Return False
    '    ElseIf SlcDrug.SelectedIndex = 0 Then
    '        Me.ObjCommon.ShowAlert("Please Select Drug", Me.Page)
    '        Return False
    '    End If
    '    Return True
    'End Function

#End Region

#Region "AssignValue"
    Protected Sub AssignValue(ByVal Type As String)
        Dim dr As DataRow
        Dim dt_Info As New DataTable
        dt_Info = CType(Me.ViewState(VS_DtInfo), DataTable)
        dr = dt_Info.NewRow()

        If Type = "SA" Then

            'vType,vName,vAddress1,vAddress2,vAddress3,vTeleNo,vExtNo,vFaxNo,vDesignation,vQualification
            dr("vType") = "SA"
            dr("vName") = Me.TxtAName.Value.Trim()
            dr("vAddress1") = Me.TxtAAddr1.Value.Trim()
            dr("vAddress2") = Me.TxtAAddr2.Value.Trim()
            dr("vAddress3") = Me.TxtAAddr3.Value.Trim()
            dr("vTeleNo") = Me.TxtATel.Value.Trim()
            dr("vExtNo") = Me.TxtAExt.Value.Trim()
            dr("vFaxNo") = "NA"
            dr("vDesignation") = Me.TxtADesig.Value.Trim()
            dr("vQualification") = Me.TxtAQuali.Value.Trim()
            dr("iModifyBy") = Me.Session(S_UserID)
            dr("cStatusIndi") = "N"

        ElseIf Type = "SM" Then

            dr("vType") = "SM"
            dr("vName") = Me.TxtMName.Value.Trim()
            dr("vAddress1") = Me.TxtMAddr1.Value.Trim()
            dr("vAddress2") = Me.TxtMAddr2.Value.Trim()
            dr("vAddress3") = Me.TxtMAddr3.Value.Trim()
            dr("vTeleNo") = Me.TxtMTel.Value.Trim()
            dr("vExtNo") = Me.TxtMExt.Value.Trim()
            dr("vFaxNo") = "NA"
            dr("vDesignation") = Me.TxtMDesig.Value.Trim()
            dr("vQualification") = Me.TxtMQuali.Value.Trim()
            dr("iModifyBy") = Me.Session(S_UserID)
            dr("cStatusIndi") = "N"

        ElseIf Type = "SC" Then

            dr("vType") = "SC"
            dr("vName") = "NA"
            dr("vAddress1") = Me.TxtCAddr1.Value.Trim()
            dr("vAddress2") = Me.TxtCAddr2.Value.Trim()
            dr("vAddress3") = Me.TxtCAddr3.Value.Trim()
            dr("vTeleNo") = Me.TxtCTel.Value.Trim()
            dr("vExtNo") = Me.TxtCExt.Value.Trim()
            dr("vFaxNo") = Me.TxtCFax.Value.Trim()
            dr("vDesignation") = "NA"
            dr("vQualification") = "NA"
            dr("iModifyBy") = Me.Session(S_UserID)
            dr("cStatusIndi") = "N"

        End If

        dt_Info.Rows.Add(dr)
        Me.ViewState(VS_DtInfo) = dt_Info
    End Sub

    Private Function Save_AssignValues() As Boolean
        Dim dr As DataRow
        Dim drM As DataRow
        Dim drS As DataRow
        Dim dt_WS As New DataTable
        Dim dt_WSPD As New DataTable
        Dim dt_WSSub As New DataTable
        Dim ds_Location As New DataSet
        Dim estr As String = String.Empty
        Dim BaseWorkFolder As String = System.Configuration.ConfigurationManager.AppSettings("BaseWorkFolder")

        Dim ds_Check As New DataSet
        Dim wStr As String = String.Empty


        dt_WS = CType(Me.ViewState(VS_DtWorkSpace), DataTable)
        dt_WSPD = CType(Me.ViewState(VS_DtWSPD), DataTable)
        dt_WSSub = CType(Me.ViewState(VS_DtWSSub), DataTable)

        'For Validation of Duplication projectno :Start
        If Not Me.txtProNo.Value.Trim() = "" Then

            wStr = "cStatusIndi <> 'D' And vProjectNo='" & Me.txtProNo.Value.Trim() & "'"
            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                wStr += " And vWorkSpaceId <> '" + Me.ViewState(VS_WorkSpaceId).ToString() + "'"
            End If

            If Not objHelp.GetWorkspaceProtocolDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_Check, estr) Then

                Me.ShowErrorMessage("Error While Getting Data From WORKSPACEPROTOCOLDETAIL", estr)
                Exit Function

            End If
            '====

            If ds_Check.Tables(0).Rows.Count > 0 Then

                ObjCommon.ShowAlert("Entered Project No Already Exist.", Me)
                Exit Function

            End If
        End If
        'For Validation of Duplication projectno :Start



        'Added on 13-Jul-2009 as suggested by Nikur Sir
        objHelp.getLocationMst("cStatusIndi<>'D' And vLocationCode='" & Me.slcLocation.SelectedValue.Trim() & "'", _
                               WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Location, estr)

        'As Per ther requirement Combination of Project Type , Drug name and Sponsor. 
        Me.TxtProject.Value = Me.SlcProject.Items(Me.SlcProject.SelectedIndex).Text.Trim() & "-" & _
                               Me.SlcDrug.Items(Me.SlcDrug.SelectedIndex).Text.Trim() & "-" & _
                               Me.SlcSponsor.Items(Me.SlcSponsor.SelectedIndex).Text.Trim() & "-" & _
                               ds_Location.Tables(0).Rows(0).Item("vLocationInitiate").ToString.Trim()
        '***********************************

        If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

            If Not dt_WS Is Nothing Then 'If dt_WS is nothing then no need to clear it--Added By-Chandresh Vanker
                dt_WS.Clear()
            End If


            dr = dt_WS.NewRow()
            'vWorkSpaceDesc,vProjectTypeCode,vClientCode,vTemplateId,vBaseWorkFolder,vBasePublishFolder,vLastPublishedVersion,dCreatedOn,cProjectStatus,vRemark
            dr("vWorkSpaceId") = "01"
            dr("vWorkSpaceDesc") = Me.TxtProject.Value.Trim()
            dr("vProjectTypeCode") = Me.SlcProject.SelectedValue.Trim()
            'dr("vClientCode") = Me.SlcSponsor.Value.Trim()
            dr("vClientCode") = Me.SlcSponsor.SelectedValue.Trim()
            dr("vTemplateId") = Me.SlcTemplate.Value.Trim()
            dr("vBaseWorkFolder") = BaseWorkFolder
            dr("vBasePublishFolder") = BaseWorkFolder
            dr("vLastPublishedVersion") = "0000"
            dr("dCreatedOn") = System.DateTime.Now()
            dr("cProjectStatus") = "I"
            dr("cWorkspaceType") = "P"
            dr("vParentWorkspaceId") = DBNull.Value
            dr("vRemark") = "Remarks"
            dr("iModifyBy") = Me.Session(S_UserID)
            dr("vLocationCode") = Me.slcLocation.SelectedValue.Trim()

            dr("vPiName") = IIf(Me.SlcPI.Value.Trim() <> "", Me.SlcPI.Value.Trim(), 0)
            dr("vProjectSubTypeCode") = Me.SlcProjectSubType.SelectedValue.Trim()
            dr("vServiceCode") = Me.SlcService.Value.Trim()
            Dim selectedText As String = String.Empty
            dr("vRRequirement") = hdnRegularityReq.Value
            dr("vIndication") = Me.txtIndication.Text.Trim()
            dr("vEARequirement") = Me.rBtnEthics.SelectedItem.Text.Trim()

            '===temp
            dr("nWorkTypeNo") = Me.ddlStudytype.SelectedValue.Trim()
            If Me.ChkfastfedYes.Checked = True Then
                dr("cFastingFed") = "FS"
            ElseIf Me.ChkfastfedNo.Checked = True Then
                dr("cFastingFed") = "FD"
            End If
            If Me.txtRetentionPeriod.Value.Trim() = "" Then
                dr("nRetaintionPeriod") = DBNull.Value
            ElseIf Me.txtRetentionPeriod.Value.Trim() <> "" Then
                dr("nRetaintionPeriod") = Me.txtRetentionPeriod.Value.Trim()
            End If

            dt_WS.Rows.Add(dr)

            dt_WSPD.Clear()
            drM = dt_WSPD.NewRow()

            drM("vProjectNo") = Me.txtProNo.Value.Trim()
            drM("vDrugCode") = Me.SlcDrug.SelectedValue.Trim()
            drM("vDrugName") = Me.SlcDrug.SelectedItem.Text.Trim()
            'To add remarks i.e sponser specific project no. Start
            drM("vRemarks") = Me.TxtSponserNo.Value.Trim()
            'To add remarks i.e sponser specific project no. End
            drM("vSubRegionCode") = Me.SlcSubmission.Value.Trim()
            If Me.SlcManager.SelectedIndex = 0 Then
                drM("iProjectManagerId") = DBNull.Value
            Else
                drM("iProjectManagerId") = Me.SlcManager.Value.Trim()
            End If

            If Me.SlcCoOrdinate.SelectedIndex <= 0 Then
                drM("iProjectCoordinatorId") = DBNull.Value
            Else
                drM("iProjectCoordinatorId") = Me.SlcCoOrdinate.Value.Trim()
            End If

            drM("vStudyDesign") = Me.TxtStudy.Value.Trim()
            drM("iNoOfSubjects") = IIf(Me.TxtSubNo.Text.Trim() = "", 0, Me.TxtSubNo.Text.Trim())
            drM("vBrandName") = Me.TxtBrand.Value.Trim()
            drM("vGenericName") = Me.TxtGeneric.Value.Trim()
            drM("vStrength") = Me.TxtStrength.Value.Trim()
            drM("vFormulationType") = Me.TxtFormulation.Value.Trim()
            drM("vManufactureName") = Me.TxtMfcName.Value.Trim()
            drM("vMfCountry") = Me.SlcMfcCountry.Value.Trim()
            drM("vMktPersonName") = Me.TxtMktName.Value.Trim()
            drM("vMktCountryCode") = Me.SlcMktCountry.Value.Trim()
            drM("vBatchNo") = Me.TxtLot.Value.Trim()
            drM("vTPGenericName") = Me.TxtGenericT.Value.Trim()
            drM("vTPStrength") = Me.TxtStrengthT.Value.Trim()
            drM("vTPFormulationType") = Me.TxtFormulationT.Value.Trim()
            drM("vTPMfName") = Me.TxtMfcNameT.Value.Trim()
            drM("vTPMfCountryCode") = Me.SlcMfcCountryT.Value.Trim()
            drM("vTPBatchNo") = Me.TxtLotT.Value.Trim()
            drM("vDose") = Me.TxtDose.Value.Trim()
            drM("vSafetyIssues") = Me.TxtSafty.Value.Trim()
            drM("vDrugStrength") = Convert.ToString(Me.ddlStrength.SelectedValue)
            drM("vDrugFormulation") = Convert.ToString(Me.ddlFromulation.SelectedValue)
            drM("vDrugRelease") = Convert.ToString(Me.ddlRelease.SelectedValue)
            drM("vEthicalRemark") = Convert.ToString(Me.txtEthicalRemarks.Text.Trim())

            If Me.ChkDrugYes.Checked = True Then
                drM("cNewDrugStatus") = "Y"
            ElseIf Me.ChkDrugNo.Checked = True Then
                drM("cNewDrugStatus") = "N"
            End If

            If Me.ChkPermissionYes.Checked = True Then
                drM("cPermissionRequired") = "Y"
            ElseIf Me.ChkPermissionNo.Checked = True Then
                drM("cPermissionRequired") = "N"
            End If
            drM("vAnalyticalMethod") = Me.txtAnalytical.Value.Trim()

            If Me.ChkMethodYes.Checked = True Then
                drM("cMethodReadyFlag") = "Y"
            ElseIf Me.ChkMethodNo.Checked = True Then
                drM("cMethodReadyFlag") = "N"
            End If

            If Me.txtReadyStart.Text.Length > 8 Then
                drM("dMethodReadyStartDate") = Me.txtReadyStart.Text.Trim()
            Else
                drM("dMethodReadyStartDate") = System.DBNull.Value
            End If

            If Me.txtReadyEnd.Text.Length > 8 Then
                drM("dMethodReadyEndDate") = Me.txtReadyEnd.Text.Trim()
            Else
                drM("dMethodReadyEndDate") = System.DBNull.Value
            End If

            If Me.ChkTestYes.Checked = True Then
                drM("cTestLicenseRequired") = "Y"
            ElseIf Me.ChkTestNo.Checked = True Then
                drM("cTestLicenseRequired") = "N"
            End If

            drM("iModifyBy") = Me.Session(S_UserID)
            dt_WSPD.Rows.Add(drM)

            drS = dt_WSSub.NewRow
            'WorkspaceId,@NoOfSubjects,@iModifyBy
            drS("NoOfSubjects") = IIf(Me.TxtSubNo.Text.Trim() = "", 0, Me.TxtSubNo.Text.Trim())
            drS("iModifyBy") = Me.Session(S_UserID)
            dt_WSSub.Rows.Add(drS)

        ElseIf Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

            'If Me.TxtSubNo.Text.Trim() = "" Or Me.TxtSubNo.Text.Trim() = "0" Then
            '    Me.ObjCommon.ShowAlert("Please Enter No. of Subjects.", Me.Page)
            '    Return False
            '    Exit Function
            'End If

            For Each dr In dt_WS.Rows

                'vWorkSpaceDesc,vProjectTypeCode,vClientCode,vTemplateId,vBaseWorkFolder,vBasePublishFolder,vLastPublishedVersion,dCreatedOn,cProjectStatus,vRemark
                dr("vWorkSpaceDesc") = Me.TxtProject.Value.Trim()
                dr("vProjectTypeCode") = Me.SlcProject.SelectedValue.Trim()
                'dr("vClientCode") = Me.SlcSponsor.Value.Trim()
                dr("vClientCode") = Me.SlcSponsor.SelectedValue.Trim()
                dr("vTemplateId") = Me.SlcTemplate.Value.Trim()
                dr("vBaseWorkFolder") = BaseWorkFolder
                dr("vBasePublishFolder") = BaseWorkFolder
                dr("vLastPublishedVersion") = "0000"
                dr("dCreatedOn") = System.DateTime.Now()
                'dr("cProjectStatus") = "I"  'will not be updated  
                dr("cWorkspaceType") = "P"
                dr("vParentWorkspaceId") = DBNull.Value
                dr("vRemark") = "Remarks"
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("vLocationCode") = Me.slcLocation.SelectedValue.Trim()
                dr("nWorkTypeNo") = Me.ddlStudytype.SelectedValue
                dr("vPiName") = IIf(Me.SlcPI.Value.Trim() <> "", Me.SlcPI.Value.Trim(), 0)
                If Me.ChkfastfedYes.Checked = True Then
                    dr("cFastingFed") = "FS"
                ElseIf Me.ChkfastfedNo.Checked = True Then
                    dr("cFastingFed") = "FD"
                End If
                If Me.txtRetentionPeriod.Value.Trim() = "" Then
                    dr("nRetaintionPeriod") = DBNull.Value
                ElseIf Me.txtRetentionPeriod.Value.Trim() <> "" Then
                    dr("nRetaintionPeriod") = Me.txtRetentionPeriod.Value.Trim()
                End If
                dr("vProjectSubTypeCode") = Me.SlcProjectSubType.SelectedValue.Trim()
                dr("vServiceCode") = Me.SlcService.Value.Trim()
                Dim selectedText As String = String.Empty
                dr("vRRequirement") = hdnRegularityReq.Value

                dr("vIndication") = Me.txtIndication.Text.Trim()
                dr("vEARequirement") = Convert.ToString(Me.rBtnEthics.SelectedItem).Trim()

                dr.AcceptChanges()

            Next dr
            dt_WS.AcceptChanges()

            For Each drM In dt_WSPD.Rows
                drM("vProjectNo") = Me.txtProNo.Value.Trim()
                drM("vDrugCode") = IIf(Me.SlcDrug.SelectedValue.Trim() = "", Convert.ToString(Session("vDrugCode")), Me.SlcDrug.SelectedValue.Trim())
                drM("vDrugName") = Me.SlcDrug.SelectedItem.Text.Trim()
                drM("vSubRegionCode") = Me.SlcSubmission.Value.Trim()
                drM("iProjectManagerId") = Me.SlcManager.Value.Trim()
                drM("vRemarks") = Me.TxtSponserNo.Value.Trim()
                If Me.SlcCoOrdinate.SelectedIndex <= 0 Then
                    drM("iProjectCoordinatorId") = DBNull.Value
                Else
                    drM("iProjectCoordinatorId") = Me.SlcCoOrdinate.Value.Trim()
                End If
                drM("vStudyDesign") = Me.TxtStudy.Value.Trim()
                drM("iNoOfSubjects") = IIf(Me.TxtSubNo.Text.Trim() = "", 0, Me.TxtSubNo.Text.Trim())
                drM("vBrandName") = Me.TxtBrand.Value.Trim()
                drM("vGenericName") = Me.TxtGeneric.Value.Trim()
                drM("vStrength") = Me.TxtStrength.Value.Trim()
                drM("vFormulationType") = Me.TxtFormulation.Value.Trim()
                drM("vManufactureName") = Me.TxtMfcName.Value.Trim()
                drM("vMfCountry") = Me.SlcMfcCountry.Value.Trim()
                drM("vMktPersonName") = Me.TxtMktName.Value.Trim()
                drM("vMktCountryCode") = Me.SlcMktCountry.Value.Trim()
                drM("vBatchNo") = Me.TxtLot.Value.Trim()
                drM("vTPGenericName") = Me.TxtGenericT.Value.Trim()
                drM("vTPStrength") = Me.TxtStrengthT.Value.Trim()
                drM("vTPFormulationType") = Me.TxtFormulationT.Value.Trim()
                drM("vTPMfName") = Me.TxtMfcNameT.Value.Trim()
                drM("vTPMfCountryCode") = Me.SlcMfcCountryT.Value.Trim()
                drM("vTPBatchNo") = Me.TxtLotT.Value.Trim()
                drM("vDose") = Me.TxtDose.Value.Trim()
                drM("vSafetyIssues") = Me.TxtSafty.Value.Trim()
                drM("vDrugStrength") = Convert.ToString(Me.ddlStrength.SelectedValue)
                drM("vDrugFormulation") = Convert.ToString(Me.ddlFromulation.SelectedValue)
                drM("vDrugRelease") = Convert.ToString(Me.ddlRelease.SelectedValue)
                drM("vEthicalRemark") = Convert.ToString(Me.txtEthicalRemarks.Text.Trim())

                If Me.ChkDrugYes.Checked = True Then
                    drM("cNewDrugStatus") = "Y"
                ElseIf Me.ChkDrugNo.Checked = True Then
                    drM("cNewDrugStatus") = "N"
                End If

                If Me.ChkPermissionYes.Checked = True Then
                    drM("cPermissionRequired") = "Y"
                ElseIf Me.ChkPermissionNo.Checked = True Then
                    drM("cPermissionRequired") = "N"
                End If

                drM("vAnalyticalMethod") = Me.txtAnalytical.Value.Trim()
                If Me.ChkMethodYes.Checked = True Then
                    drM("cMethodReadyFlag") = "Y"
                ElseIf Me.ChkMethodNo.Checked = True Then
                    drM("cMethodReadyFlag") = "N"
                End If

                If Me.txtReadyStart.Text.Length > 8 Then
                    drM("dMethodReadyStartDate") = Me.txtReadyStart.Text.Trim()
                Else
                    drM("dMethodReadyStartDate") = System.DBNull.Value
                End If

                If Me.txtReadyEnd.Text.Length > 8 Then
                    drM("dMethodReadyEndDate") = Me.txtReadyEnd.Text.Trim()
                Else
                    drM("dMethodReadyEndDate") = System.DBNull.Value
                End If

                If Me.ChkTestYes.Checked = True Then
                    drM("cTestLicenseRequired") = "Y"
                ElseIf Me.ChkTestNo.Checked = True Then
                    drM("cTestLicenseRequired") = "N"
                End If

                drM("iModifyBy") = Me.Session(S_UserID)
                drM.AcceptChanges()

            Next drM

            dt_WSPD.AcceptChanges()
        End If

        Me.ViewState(VS_DtWSPD) = dt_WSPD
        Me.ViewState(VS_DtWorkSpace) = dt_WS
        Me.ViewState(VS_DtWSSub) = dt_WSSub
        Return True
    End Function

#End Region

#Region "Reset Page"

    Private Sub ResetPage()

        Me.Response.Redirect("frmProtocolDetail.aspx?mode=" + Me.Request.QueryString("Mode").ToString.Trim, False)

        'Me.SlcCoOrdinate.SelectedIndex = 0
        'Me.SlcDrug.SelectedIndex = 0
        'Me.SlcMfcCountry.SelectedIndex = 0
        'Me.SlcMfcCountryT.SelectedIndex = 0
        'Me.SlcMktCountry.SelectedIndex = 0
        'Me.SlcProject.SelectedIndex = 0
        'Me.SlcSponsor.SelectedIndex = 0
        'Me.SlcSubmission.SelectedIndex = 0
        'Me.SlcTemplate.SelectedIndex = 0
        'Me.SlcManager.SelectedIndex = 0
        'Me.slcLocation.SelectedIndex = 0

        'Me.txtsearch.Text = ""
        'Me.TxtProject.Value = ""
        'Me.TxtFormulation.Value = ""
        'Me.TxtGeneric.Value = ""
        'Me.TxtGenericT.Value = ""
        'Me.TxtLot.Value = ""
        'Me.TxtLotT.Value = ""
        'Me.TxtMfcName.Value = ""
        'Me.TxtMfcNameT.Value = ""
        'Me.TxtMktName.Value = ""
        'Me.TxtSafty.Value = ""
        'Me.TxtStrength.Value = ""
        'Me.TxtStrengthT.Value = ""
        'Me.TxtStudy.Value = ""
        'Me.TxtFormulationT.Value = ""
        'Me.TxtSubNo.Text = ""
        'Me.txtProNo.Value = ""

        'Me.TxtDose.Value = ""
        'Me.TxtBrand.Value = ""

        'Me.TxtAAddr1.Value = ""
        'Me.TxtAAddr2.Value = ""
        'Me.TxtAAddr3.Value = ""
        'Me.TxtADesig.Value = ""
        'Me.TxtAExt.Value = ""
        'Me.TxtAName.Value = ""
        'Me.TxtAQuali.Value = ""
        'Me.TxtATel.Value = ""

        'Me.TxtMAddr1.Value = ""
        'Me.TxtMAddr2.Value = ""
        'Me.TxtMAddr3.Value = ""
        'Me.TxtMDesig.Value = ""
        'Me.TxtMExt.Value = ""
        'Me.TxtMName.Value = ""
        'Me.TxtMQuali.Value = ""
        'Me.TxtMTel.Value = ""

        'Me.TxtCAddr1.Value = ""
        'Me.TxtCAddr2.Value = ""
        'Me.TxtCAddr3.Value = ""
        'Me.TxtCExt.Value = ""
        'Me.TxtCFax.Value = ""
        'Me.TxtCTel.Value = ""

        'Me.txtAnalytical.Value = ""
        'Me.txtReadyEnd.Text = ""
        'Me.txtReadyStart.Text = ""

        'Me.GV_Authorised.DataSource = Nothing
        'Me.GV_Authorised.DataBind()

        'Me.GV_Contact.DataSource = Nothing
        'Me.GV_Contact.DataBind()

        'Me.GV_Medical.DataSource = Nothing
        'Me.GV_Medical.DataBind()

        'Me.ViewState(VS_DtInfo) = Nothing
        'Me.ViewState(VS_DsProtocol) = Nothing
        'Me.ViewState(VS_WorkSpaceId) = Nothing
        'Me.ViewState(VS_DtWSSub) = Nothing
        'Me.ViewState(VS_DtWSPD) = Nothing
        'Me.ViewState(VS_DtWorkSpace) = Nothing
        'Me.BtnSave.ToolTip = "Save"
        'Me.BtnSave.Text = "Save"
        ''GenCall()
    End Sub

#End Region

#Region "Grid Event"

    Protected Sub GV_Authorised_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("ImbADelete"), ImageButton).CommandName = "ADelete"
            CType(e.Row.FindControl("ImbADelete"), ImageButton).CommandArgument = e.Row.RowIndex

        End If
    End Sub

    Protected Sub GV_Medical_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("ImbMDelete"), ImageButton).CommandName = "MDelete"
            CType(e.Row.FindControl("ImbMDelete"), ImageButton).CommandArgument = e.Row.RowIndex

        End If
    End Sub

    Protected Sub GV_Contact_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("ImbCDelete"), ImageButton).CommandName = "CDelete"
            CType(e.Row.FindControl("ImbCDelete"), ImageButton).CommandArgument = e.Row.RowIndex

        End If
    End Sub

    Protected Sub GV_Authorised_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim index As Int32
        Dim IndexDelete As Int32
        Dim DTDELETE As New DataTable
        Dim DVDELETE As New DataView
        index = CInt(e.CommandArgument)

        If CType(GV_Authorised.Rows(index).FindControl("ImbADelete"), ImageButton).CommandName.ToUpper = "ADELETE" Then
            DTDELETE = CType(Me.ViewState(VS_DtInfo), DataTable)

            For IndexDelete = 0 To DTDELETE.Rows.Count - 1

                If DTDELETE.Rows(IndexDelete).Item("vType") = "SA" And _
                   DTDELETE.Rows(IndexDelete).Item("vName") = Replace(GV_Authorised.Rows(index).Cells(1).Text, "&nbsp;", " ").Trim And _
                   DTDELETE.Rows(IndexDelete).Item("vAddress1") = Replace(GV_Authorised.Rows(index).Cells(2).Text, "&nbsp;", " ").Trim And _
                   DTDELETE.Rows(IndexDelete).Item("vAddress2") = Replace(GV_Authorised.Rows(index).Cells(3).Text, "&nbsp;", " ").Trim And _
                   DTDELETE.Rows(IndexDelete).Item("vAddress3") = Replace(GV_Authorised.Rows(index).Cells(4).Text, "&nbsp;", " ").Trim Then

                    If DTDELETE.Rows(IndexDelete).Item("vWorkSpaceId") Is System.DBNull.Value Then
                        DTDELETE.Rows(IndexDelete).Delete()
                        DTDELETE.AcceptChanges()
                        Exit For
                    Else

                        If DTDELETE.Rows(IndexDelete).Item("vWorkSpaceId") = "" Then
                            DTDELETE.Rows(IndexDelete).Delete()
                            DTDELETE.AcceptChanges()
                            Exit For
                        Else
                            DTDELETE.Rows(IndexDelete).Item("vWorkSpaceId") = System.DBNull.Value
                            DTDELETE.Rows(IndexDelete).Item("cStatusIndi") = "D"
                            Exit For
                        End If

                    End If

                End If

            Next IndexDelete

            Me.ViewState(VS_DtInfo) = DTDELETE
            DVDELETE = CType(Me.ViewState(VS_DtInfo), DataTable).DefaultView
            DVDELETE.RowFilter = "vType = 'SA' and cStatusIndi<>'D'"
            Me.GV_Authorised.DataSource = DVDELETE.ToTable
            Me.GV_Authorised.DataBind()
        End If

    End Sub

    Protected Sub GV_Medical_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim Index As Int32
        Dim IndexDelete As Int32
        Dim DTDELETE As New DataTable
        Dim DVDELETE As New DataView
        Index = CInt(e.CommandArgument)

        If CType(GV_Medical.Rows(Index).FindControl("ImbMDelete"), ImageButton).CommandName.ToUpper = "MDELETE" Then
            DTDELETE = CType(Me.ViewState(VS_DtInfo), DataTable)
            For IndexDelete = 0 To DTDELETE.Rows.Count - 1
                If DTDELETE.Rows(IndexDelete).Item("vType") = "SM" And _
                    DTDELETE.Rows(IndexDelete).Item("vName") = Replace(GV_Medical.Rows(Index).Cells(1).Text, "&nbsp;", " ").Trim And _
                    DTDELETE.Rows(IndexDelete).Item("vAddress1") = Replace(GV_Medical.Rows(Index).Cells(2).Text, "&nbsp;", " ").Trim And _
                    DTDELETE.Rows(IndexDelete).Item("vAddress2") = Replace(GV_Medical.Rows(Index).Cells(3).Text, "&nbsp;", " ").Trim And _
                    DTDELETE.Rows(IndexDelete).Item("vAddress3") = Replace(GV_Medical.Rows(Index).Cells(4).Text, "&nbsp;", " ").Trim Then

                    If DTDELETE.Rows(IndexDelete).Item("vWorkSpaceId") Is System.DBNull.Value Then
                        DTDELETE.Rows(IndexDelete).Delete()
                        DTDELETE.AcceptChanges()
                        Exit For
                    Else

                        If DTDELETE.Rows(IndexDelete).Item("vWorkSpaceId") = "" Then
                            DTDELETE.Rows(IndexDelete).Delete()
                            DTDELETE.AcceptChanges()
                            Exit For
                        Else
                            DTDELETE.Rows(IndexDelete).Item("vWorkSpaceId") = System.DBNull.Value
                            DTDELETE.Rows(IndexDelete).Item("cStatusIndi") = "D"
                            Exit For
                        End If

                    End If

                End If

            Next IndexDelete

            Me.ViewState(VS_DtInfo) = DTDELETE
            DVDELETE = CType(Me.ViewState(VS_DtInfo), DataTable).DefaultView
            DVDELETE.RowFilter = "vType = 'SM' and cStatusIndi<>'D'"
            Me.GV_Medical.DataSource = DVDELETE.ToTable
            Me.GV_Medical.DataBind()
        End If
    End Sub

    Protected Sub GV_Contact_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim Index As Int32
        Dim IndexDelete As Int32
        Dim DTDELETE As New DataTable
        Dim DVDELETE As New DataView
        Index = CInt(e.CommandArgument)

        If CType(GV_Contact.Rows(Index).FindControl("ImbCDelete"), ImageButton).CommandName.ToUpper = "CDELETE" Then
            DTDELETE = CType(Me.ViewState(VS_DtInfo), DataTable)
            'DVDELETE.RowFilter = ""
            For IndexDelete = 0 To DTDELETE.Rows.Count - 1
                If DTDELETE.Rows(IndexDelete).Item("vType") = "SC" And _
                    DTDELETE.Rows(IndexDelete).Item("vName") = Replace(GV_Contact.Rows(Index).Cells(1).Text, "&nbsp;", " ").Trim And _
                    DTDELETE.Rows(IndexDelete).Item("vAddress1") = Replace(GV_Contact.Rows(Index).Cells(2).Text, "&nbsp;", " ").Trim And _
                    DTDELETE.Rows(IndexDelete).Item("vAddress2") = Replace(GV_Contact.Rows(Index).Cells(3).Text, "&nbsp;", " ").Trim And _
                    DTDELETE.Rows(IndexDelete).Item("vAddress3") = Replace(GV_Contact.Rows(Index).Cells(4).Text, "&nbsp;", " ").Trim Then

                    If DTDELETE.Rows(IndexDelete).Item("vWorkSpaceId") Is System.DBNull.Value Then
                        DTDELETE.Rows(IndexDelete).Delete()
                        DTDELETE.AcceptChanges()
                        Exit For
                    Else

                        If DTDELETE.Rows(IndexDelete).Item("vWorkSpaceId") = "" Then
                            DTDELETE.Rows(IndexDelete).Delete()
                            DTDELETE.AcceptChanges()
                            Exit For
                        Else
                            DTDELETE.Rows(IndexDelete).Item("vWorkSpaceId") = System.DBNull.Value
                            DTDELETE.Rows(IndexDelete).Item("cStatusIndi") = "D"
                            Exit For
                        End If

                        Exit For
                    End If

                End If

            Next IndexDelete
            Me.ViewState(VS_DtInfo) = DTDELETE
            DVDELETE = CType(Me.ViewState(VS_DtInfo), DataTable).DefaultView
            DVDELETE.RowFilter = "vType = 'SC' and cStatusIndi<>'D'"
            Me.GV_Contact.DataSource = DVDELETE.ToTable
            Me.GV_Contact.DataBind()
        End If
    End Sub

    Protected Sub GV_Authorised_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs)

    End Sub

    Protected Sub GV_Medical_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs)

    End Sub

    Protected Sub GV_Contact_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs)

    End Sub

#End Region

#Region "DropDown Change Event"
    Protected Sub SlcDrug_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            Dim ds_DrugMst As DataSet
            Dim wStr As String
            If (Request.QueryString("mode") = 2) Then
                wStr = Session("vDrugCode")
            Else
                wStr = Me.SlcDrug.SelectedValue
            End If

            ds_DrugMst = objHelp.ProcedureExecute("dbo.Proc_DrugMst", wStr)

            ddlFromulation.DataSource = ds_DrugMst
            Me.ddlFromulation.DataTextField = "vformulation"
            Me.ddlFromulation.DataValueField = "vformulation"
            ddlFromulation.DataBind()

            ddlStrength.DataSource = ds_DrugMst
            Me.ddlStrength.DataTextField = "vstrength"
            Me.ddlStrength.DataValueField = "vstrength"
            ddlStrength.DataBind()

            ddlRelease.DataSource = ds_DrugMst
            Me.ddlRelease.DataTextField = "vrelease"
            Me.ddlRelease.DataValueField = "vrelease"
            ddlRelease.DataBind()
            hdnPostBack.Value = "false"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SlideToggle for btnaadd", " SlideToggle(ctl00_CPHLAMBDA_TabContainer1_TabPanel2_imgprotocoldefinition, tblprotocoldefinition)", True)

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub SlcSponsor_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim eStr As String = String.Empty
        Dim ds_ClientMst As New DataSet
        Dim dt_client As New DataTable
        Dim wstr As String
        Try
            wstr = "vClientCode = " + Me.SlcSponsor.SelectedValue

            If Not objHelp.get_Viewclientmst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ClientMst, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds_ClientMst Is Nothing And ds_ClientMst.Tables(0).Rows.Count > 0 Then
                SlcManager.Value = ds_ClientMst.Tables(0).Rows(0)(7)
            End If

        Catch ex As Exception

        End Try

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

#Region "WebMethods"
    '' Added by Dipen Shah for auto time calculation of application serevr.
    <Web.Services.WebMethod()> _
    Public Shared Function GetServerTime(ByVal timeZone As String) As String

        Dim objCommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

        Try
            Dim da As Date = CDate(objCommon.GetCurDatetimeWithOffSet(timeZone).DateTime).GetDateTimeFormats()(23)
            Return da
        Catch ex As Exception
            Return ex.Message
            Return False
        End Try
        Return True
    End Function

    ''Add by shivani pandya for Project lock
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
#End Region






End Class

