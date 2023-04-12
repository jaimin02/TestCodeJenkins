
Partial Class frmOldProjects
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATIONS"
    Private objCommon As New clsCommon
    Private objHelpDBTable As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()


    'Private Const GRDCell_LnkEdit As Integer = 1
    Private Const GRDCell_LnkDelete As Integer = 1
    Private Const GRDCell_nSerialNo As Integer = 2
    Private Const GRDCell_ProjectNumber As Integer = 3
    Private Const GRDCell_NumberofSubjects As Integer = 4
    Private Const GRDCell_Submission As Integer = 5
    Private Const GRDCell_StudyDescription As Integer = 6
    Private Const GRDCell_ProductName As Integer = 7
    Private Const GRDCell_ProjectStatus As Integer = 8
    Private Const GRDCell_ProjectCoordinator As Integer = 10
    Private Const GRDCell_SponsorName As Integer = 11
    Private Const GRDCell_ReportMonth As Integer = 12
    Private Const GRDCell_ProjectCode As Integer = 13
    Private Const GRDCell_years As Integer = 14
    Private Const GRDCell_Months As Integer = 15
    Private Const GRDCell_Design As Integer = 16
    Private Const GRDCell_TreatmentI As Integer = 17
    Private Const GRDCell_TreatmentII As Integer = 18
    Private Const GRDCell_ReferenceDrugProductA As Integer = 19
    Private Const GRDCell_ReferenceDrugProductB As Integer = 20
    Private Const GRDCell_TestDrugProduct As Integer = 21
    Private Const GRDCell_TestDrugProductC As Integer = 22
    Private Const GRDCell_TestDrugProductB As Integer = 23
    Private Const GRDCell_SponsorAddress As Integer = 24
    Private Const GRDCell_SponsorTelephone As Integer = 25
    Private Const GRDCell_SponsorFax As Integer = 26
    Private Const GRDCell_SponsorContactPerson As Integer = 27
    Private Const GRDCell_Groups As Integer = 28
    Private Const GRDCell_PeriodICheckInDate As Integer = 29
    Private Const GRDCell_PeriodIICheckInDate As Integer = 30
    Private Const GRDCell_PeriodIIICheckInDate As Integer = 31
    Private Const GRDCell_PeriodIVCheckInDate As Integer = 32
    Private Const GRDCell_PeriodIDosingDate As Integer = 33
    Private Const GRDCell_PeriodIIDosingDate As Integer = 34
    Private Const GRDCell_PeriodIIIDosingDate As Integer = 35
    Private Const GRDCell_PeriodIVDosingDate As Integer = 36
    Private Const GRDCell_PeriodICheckOutDate As Integer = 37
    Private Const GRDCell_PeriodIICheckOutDate As Integer = 38
    Private Const GRDCell_PeriodIIICheckOutDate As Integer = 39
    Private Const GRDCell_PeriodIVCheckOutDate As Integer = 40
    Private Const GRDCell_LetterIssued As Integer = 41
    Private Const GRDCell_Dateofletterissue As Integer = 42
    Private Const GRDCell_Remarks As Integer = 43
    Private Const GRDCell_SampleExpectedDate As Integer = 44
    Private Const GRDCell_AnalysisStartDate As Integer = 45
    Private Const GRDCell_AnalysisEndDate As Integer = 46
    Private Const GRDCell_ReportSentDate As Integer = 47
    Private Const GRDCell_ReportYear As Integer = 48
    Private Const GRDCell_ReturntoSponsor As Integer = 49
    Private Const GRDCell_NumberofStandbySubjects As Integer = 50
    Private Const GRDCell_NumberofSamples As Integer = 51
    Private Const GRDCell_ReportStatus As Integer = 52
    Private Const GRDCell_ReportStatus1 As Integer = 53
    Private Const GRDCell_TestDrugProductD As Integer = 54
    Private Const GRDCell_LastAmbulatoryEndStudy As Integer = 55
    Private Const GRDCell_StudyResults As Integer = 56
    Private Const GRDCell_Condition As Integer = 57
    Private Const GRDCell_EArchiving As Integer = 58
    Private Const GRDCell_Location As Integer = 59
    Private Const GRDCell_RetentionPeriod As Integer = 60
    Private Const GRDCell_RpId As Integer = 61
    Private Const GRDCell_StatusName As Integer = 62
    Private Const GRDCell_cStatusIndi As Integer = 63
    Private Const GRDCell_iModifyBy As Integer = 64
    Private Const GRDCell_dModifyOn As Integer = 65
    Private Const GRDCell_TreatmentIII As Integer = 66
    Private Const GRDCell_vDrugCode As Integer = 67
    Private Const GRDCell_vClientCode As Integer = 68
    Private Const GRDCell_vLocationCode As Integer = 69
    Private Const GRDCell_PStatus As Integer = 70

    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtOldProjects As String = "DtOldProjects"
    Private Const VS_OldProjectsDtl As String = "OldProjectsDtl"
    Private Const VS_SerialNo As String = "SerialNo"
    Private ds_OldProjects As DataSet
    Private eStr_Retu As String = ""
    Private SelectedIndex As Integer = 0
#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Me.AutoCompleteExtender1.ContextKey = "cStatusIndi<>'D'"

            If Not GenCall() Then
                Exit Sub
            End If

        End If

    End Sub
#End Region

#Region "GenCall"
    Private Function GenCall() As Boolean
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim dt_OldProjects As DataTable = Nothing
        Dim ds_OldProjects As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Old Projects Detail"


            Choice = Me.Request.QueryString("mode").ToString
            Me.ViewState(VS_Choice) = Choice   'To use it while saving

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(VS_SerialNo) = Me.Request.QueryString("Value").ToString
            End If

            ''''Check for Valid User''''''''''''''
            If Not GenCall_Data(Choice, dt_OldProjects) Then ' For Data Retrieval
                Exit Function
            End If

            Me.ViewState(VS_DtOldProjects) = dt_OldProjects ' adding blank DataTable in viewstate
            If Not GenCall_ShowUI(Choice, dt_OldProjects) Then 'For Displaying Data 
                Exit Function
            End If

            'Me.txtlocationname.Attributes.Add("onblur", "setInitial();")
            ' Me.txtcountrycode.Attributes.Add("onblur", "setcountrycode();")

            GenCall = True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....GenCall")
        Finally
        End Try
    End Function
#End Region

#Region "GenCall_Data"
    Private Function GenCall_Data(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                            ByRef dt_Dist_Retu As DataTable) As Boolean

        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_OldProjects As DataSet = Nothing
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Try



            If Choice_1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "nSerialNo= '" + Me.ViewState(VS_SerialNo).ToString() + "'"
            End If

            wStr += " And cStatusIndi <> 'D'"

            If Not objHelp.GetOldProjects(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_OldProjects, eStr) Then
                Me.objCommon.ShowAlert("Error While Getting Data From OldProjects ", Me.Page)
            End If

            If ds_OldProjects Is Nothing Then
                Throw New Exception(eStr)
            End If

            If ds_OldProjects.Tables(0).Rows.Count <= 0 And _
               Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                Throw New Exception("No Records Found For Selected role")

            End If
            ViewState("VS_OldProjectsDtl") = ds_OldProjects.Tables(0)
            dt_Dist_Retu = ds_OldProjects.Tables(0)
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...GenCall_Data")
        Finally
        End Try
    End Function
#End Region

#Region "GenCall_showUI "

    Private Function GenCall_ShowUI(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                                      ByVal dt_LocationMst As DataTable) As Boolean
        Dim dt_OldProjects As DataTable = Nothing
        Try
            Page.Title = ":: Old Projects Detail :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            BindGrid()
            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_ShowUI")
        Finally
        End Try
    End Function

#End Region

#Region "Bind grid"

    Private Sub BindGrid()
        Dim dv_OldProjects As DataView = Nothing

        Dim index As Integer = 0
        Dim datasetcount As Integer = 0
        Dim Wstr As String = String.Empty


        ds_OldProjects = New DataSet

        Try
            If ViewState(VS_SerialNo) = "" Then
                Wstr = "cStatusIndi<>'D' "
            Else
                Wstr = "nSerialNo= '" + Me.ViewState(VS_SerialNo).ToString() + "' And cStatusIndi <> 'D'"
            End If

            If Not Me.objHelpDBTable.GetOldProjects(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_OldProjects, eStr_Retu) Then
                Exit Sub
            End If
            ds_OldProjects.AcceptChanges()
            dv_OldProjects = ds_OldProjects.Tables(0).Copy().DefaultView()

            GVWOldProjects.DataSource = dv_OldProjects.ToTable()
            Me.ViewState("VS_OldProjectsDtl") = dv_OldProjects.ToTable()
            GVWOldProjects.DataBind()


        Catch ex As Exception

            ShowErrorMessage(ex.Message, "...BindGrid")

        End Try
    End Sub

#End Region

#Region "GRID EVENTS "

    Protected Sub GVWOldProjects_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GVWOldProjects.PageIndexChanging
        Try
            GVWOldProjects.PageIndex = e.NewPageIndex
            BindGrid()
        Catch ex As Exception
            Me.objCommon.ShowAlert(ex.ToString(), Me.Page)
        End Try

    End Sub

    Protected Sub GVWOldProjects_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GVWOldProjects.RowCommand

        Try

            Dim dt_OldProjectDtl As New DataTable
            Dim ds_ClientMst As New DataSet
            Dim ds_OldProjects As New DataSet
            Dim Ds_Drug As New DataSet
            Dim Wstr As String = String.Empty
            Dim estr As String = String.Empty
            Dim SelectednSerailNo As Integer
            Dim SearchSubString As String = String.Empty
            Dim SearchSubStringForDrugCode As String = String.Empty
            Dim SelectedIndex As Integer = e.CommandArgument


            If e.CommandName.ToUpper = "EDIT" Then

                'dt_OldProjectDtl = CType(ViewState("VS_OldProjectsDtl"), DataTable)
                SelectednSerailNo = Me.GVWOldProjects.Rows(SelectedIndex).Cells(GRDCell_nSerialNo).Text.ToString().Trim()

                Wstr = "nSerialNo='" & SelectednSerailNo & "'And cStatusIndi<>'D'"
                If Not objHelpDBTable.GetOldProjects(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_OldProjects, estr) Then
                    Me.objCommon.ShowAlert("Error While Getting Data From OldProjects ", Me.Page)
                Else
                    dt_OldProjectDtl = ds_OldProjects.Tables(0)
                    ViewState("VS_OldProjectsDtl") = dt_OldProjectDtl
                End If
                If Me.GVWOldProjects.Rows(SelectedIndex).Cells(GRDCell_SponsorName).Text.Length < 4 Then
                    SearchSubString = Me.GVWOldProjects.Rows(SelectedIndex).Cells(GRDCell_SponsorName).Text.Trim()
                Else
                    SearchSubString = Me.GVWOldProjects.Rows(SelectedIndex).Cells(GRDCell_SponsorName).Text.Substring(0, 4).ToString().Trim()
                End If
                Wstr = "vClientName like '%" & SearchSubString & "%' And cStatusIndi<>'D'"
                If Not objHelpDBTable.getclientmst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_ClientMst, estr) Then
                    Me.objCommon.ShowAlert("Eroor While Getting Data From ClientMst", Me.Page)
                    Exit Sub
                End If
                If Me.GVWOldProjects.Rows(SelectedIndex).Cells(GRDCell_ProductName).Text.Length < 4 Then
                    Me.GVWOldProjects.Rows(SelectedIndex).Cells(GRDCell_ProductName).Text.Trim()

                Else
                    SearchSubStringForDrugCode = Me.GVWOldProjects.Rows(SelectedIndex).Cells(GRDCell_ProductName).Text.Substring(0, 4).ToString().Trim()
                End If

                Wstr = String.Empty
                Wstr = "vDrugName like'%" & SearchSubStringForDrugCode & "%' And cStatusIndi<>'D' "
                If Not objHelpDBTable.getdrugmst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_Drug, estr) Then
                    Me.objCommon.ShowAlert("Eroor While Getting Data From DrugMst", Me.Page)
                End If

                Me.DrpLstSponsorName.DataSource = ds_ClientMst.Tables(0)
                Me.DrpLstSponsorName.DataTextField = "vClientName"
                Me.DrpLstSponsorName.DataValueField = "vClientCode"
                Me.DrpLstSponsorName.DataBind()
                DrpLstSponsorName.Items.Insert(0, "Select....")
                ' To add tooltip
                For SponserCount As Integer = 0 To Me.DrpLstSponsorName.Items.Count - 1 Step 1
                    Me.DrpLstSponsorName.Items(SponserCount).Attributes.Add("Title", Me.DrpLstSponsorName.Items(SponserCount).Text)
                Next SponserCount
                ' to get selected sponser name
                If dt_OldProjectDtl.Rows(0).Item("vClientCode").ToString() <> System.DBNull.Value.ToString() Then
                    Me.DrpLstSponsorName.SelectedValue = dt_OldProjectDtl.Rows(0).Item("vClientCode").ToString()
                End If

                Me.DdllstForDrugName.DataSource = Ds_Drug.Tables(0)
                Me.DdllstForDrugName.DataTextField = "vDrugName"
                Me.DdllstForDrugName.DataValueField = "vDrugCode"
                Me.DdllstForDrugName.DataBind()
                Me.DdllstForDrugName.Items.Insert(0, "Select....")
                ' To add tooltip
                For DrugCount As Integer = 0 To Me.DdllstForDrugName.Items.Count - 1
                    Me.DdllstForDrugName.Items(DrugCount).Attributes.Add("Title", Me.DdllstForDrugName.Items(DrugCount).Text)
                Next DrugCount
                'To get selected product
                If dt_OldProjectDtl.Rows(0).Item("vDrugCode").ToString() <> System.DBNull.Value.ToString() Then
                    Me.DdllstForDrugName.SelectedValue = dt_OldProjectDtl.Rows(0).Item("vDrugCode").ToString()
                End If


                'For IndexForClientName = 0 To ds_ClientMst.Tables(0).Rows.Count - 1 Step 1
                '    DrpLstSponsorName.Items.Add(ds_ClientMst.Tables(0).Rows(IndexForClientName).Item("vClientName").ToString().Trim())
                '    DrpLstSponsorName.Items(IndexForClientName).Value = ds_ClientMst.Tables(0).Rows(IndexForClientName).Item("vClientCode").ToString().Trim()
                'Next


                ViewState("nSerialNo") = Me.GVWOldProjects.Rows(SelectedIndex).Cells(GRDCell_nSerialNo).Text.ToString().Trim()
                TxtProjectNo.Text = dt_OldProjectDtl.Rows(0).Item("ProjectNumber").ToString().Trim()
                TxtNoOfSubjects.Text = dt_OldProjectDtl.Rows(0).Item("NumberofSubjects").ToString().Trim()
                TxtSubmission.Text = dt_OldProjectDtl.Rows(0).Item("Submission").ToString().Trim()
                TxtStudyDescription.Text = dt_OldProjectDtl.Rows(0).Item("StudyDescription").ToString().Trim()
                TxtProductName.Text = dt_OldProjectDtl.Rows(0).Item("ProductName").ToString().Trim()
                TxtStatus.Text = dt_OldProjectDtl.Rows(0).Item("ProjectStatus").ToString().Trim()
                TxtProjectCoordinator.Text = dt_OldProjectDtl.Rows(0).Item("ProjectCoordinator").ToString().Trim()
                TxtSponsorName.Text = dt_OldProjectDtl.Rows(0).Item("SponsorName").ToString().Trim()
                TxtReportMonth.Text = dt_OldProjectDtl.Rows(0).Item("ReportMonth").ToString().Trim()
                TxtProjectCode.Text = dt_OldProjectDtl.Rows(0).Item("ProjectCode").ToString().Trim()
                TxtYear.Text = dt_OldProjectDtl.Rows(0).Item("years").ToString().Trim()
                TxtMonth.Text = dt_OldProjectDtl.Rows(0).Item("Months").ToString().Trim()
                TxtDesign.Text = dt_OldProjectDtl.Rows(0).Item("Design").ToString().Trim()
                TxtTreatment1.Text = dt_OldProjectDtl.Rows(0).Item("TreatmentI").ToString().Trim()
                TxtTreatment2.Text = dt_OldProjectDtl.Rows(0).Item("TreatmentII").ToString().Trim()
                TxtReferenceDrugProductA.Text = dt_OldProjectDtl.Rows(0).Item("ReferenceDrugProductA").ToString().Trim()
                TxtReferenceDrugProductB.Text = dt_OldProjectDtl.Rows(0).Item("ReferenceDrugProductB").ToString().Trim()
                TxtTestDrugProduct.Text = dt_OldProjectDtl.Rows(0).Item("TestDrugProduct").ToString().Trim()
                TxtTestDrugProductC.Text = dt_OldProjectDtl.Rows(0).Item("TestDrugProductC").ToString().Trim()
                TxtTestDrugProductB.Text = dt_OldProjectDtl.Rows(0).Item("TestDrugProductB").ToString().Trim()
                TxtSponsorAddress.Text = dt_OldProjectDtl.Rows(0).Item("SponsorAddress").ToString().Trim()
                TxtSponsorTelephone.Text = dt_OldProjectDtl.Rows(0).Item("SponsorTelephone").ToString().Trim()
                TxtSponsorFax.Text = dt_OldProjectDtl.Rows(0).Item("SponsorFax").ToString().Trim()
                TxtSponsorContactPerson.Text = dt_OldProjectDtl.Rows(0).Item("SponsorContactPerson").ToString().Trim()
                TxtGroup.Text = dt_OldProjectDtl.Rows(0).Item("Groups").ToString().Trim()
                TxtTestDrugProductD.Text = dt_OldProjectDtl.Rows(0).Item("TestDrugProductD").ToString().Trim()
                TxtStudyResults.Text = dt_OldProjectDtl.Rows(0).Item("StudyResults").ToString().Trim()
                TxtEArchiving.Text = dt_OldProjectDtl.Rows(0).Item("EArchiving").ToString().Trim()
                TxtRetentionPeriod.Text = dt_OldProjectDtl.Rows(0).Item("RetentionPeriod").ToString().Trim()
                TxtStatusName.Text = dt_OldProjectDtl.Rows(0).Item("StatusName").ToString().Trim()

                TxtPeriod1CheckInDate.Text = Convert.ToString(dt_OldProjectDtl.Rows(0).Item("PeriodICheckInDate")).Trim()
                TxtPeriod2CheckInDate.Text = Convert.ToString(dt_OldProjectDtl.Rows(0).Item("PeriodIICheckInDate")).ToString().Trim()
                TxtPeriod3CheckInDate.Text = Convert.ToString(dt_OldProjectDtl.Rows(0).Item("PeriodIIICheckInDate")).ToString().Trim()
                TxtPeriod4CheckInDate.Text = Convert.ToString(dt_OldProjectDtl.Rows(0).Item("PeriodIVCheckInDate")).ToString().Trim()
                TxtPeriod1DosingDate.Text = Convert.ToString(dt_OldProjectDtl.Rows(0).Item("PeriodIDosingDate")).ToString().Trim()
                TxtPeriod2DosingDate.Text = Convert.ToString(dt_OldProjectDtl.Rows(0).Item("PeriodIIDosingDate")).ToString().Trim()
                TxtPeriod3DosingDate.Text = Convert.ToString(dt_OldProjectDtl.Rows(0).Item("PeriodIIIDosingDate")).ToString().Trim()
                TxtPeriod4DosingDate.Text = Convert.ToString(dt_OldProjectDtl.Rows(0).Item("PeriodIVDosingDate")).ToString().Trim()
                TxtPeriod1CheckOutDate.Text = Convert.ToString(dt_OldProjectDtl.Rows(0).Item("PeriodICheckOutDate")).ToString().Trim()
                TxtPeriod2CheckOutDate.Text = Convert.ToString(dt_OldProjectDtl.Rows(0).Item("PeriodIICheckOutDate")).ToString().Trim()
                TxtPeriod3CheckOutDate.Text = Convert.ToString(dt_OldProjectDtl.Rows(0).Item("PeriodIIICheckOutDate")).ToString().Trim()
                TxtPeriod4CheckOutDate.Text = Convert.ToString(dt_OldProjectDtl.Rows(0).Item("PeriodIVCheckOutDate")).ToString().Trim()
                TxtLetterIssueds.Text = dt_OldProjectDtl.Rows(0).Item("LetterIssued").ToString().Trim()
                TxtDateOfLetterIssued.Text = dt_OldProjectDtl.Rows(0).Item("Dateofletterissue").ToString().Trim()
                TxtRemarks.Text = dt_OldProjectDtl.Rows(0).Item("Remarks").ToString().Trim()
                TxtSampleExpectedDate.Text = Convert.ToString(dt_OldProjectDtl.Rows(0).Item("SampleExpectedDate")).ToString().Trim()
                TxtAnalysisStartDate.Text = dt_OldProjectDtl.Rows(0).Item("AnalysisStartDate").ToString().Trim()
                TxtAnalysisEndDate.Text = Convert.ToString(dt_OldProjectDtl.Rows(0).Item("AnalysisEndDate").ToString()).Trim()
                TxtReportSentDate.Text = Convert.ToString(dt_OldProjectDtl.Rows(0).Item("ReportSentDate").ToString())
                TxtReportYear.Text = dt_OldProjectDtl.Rows(0).Item("ReportYear").ToString().Trim()
                TxtReturnToSponsor.Text = dt_OldProjectDtl.Rows(0).Item("ReturntoSponsor").ToString().Trim()
                TxtNumberOfStandbySubjects.Text = dt_OldProjectDtl.Rows(0).Item("NumberofStandbySubjects").ToString().Trim()
                TxtNumberOfSamples.Text = dt_OldProjectDtl.Rows(0).Item("NumberofSamples").ToString().Trim()
                TxtReportStatuss.Text = dt_OldProjectDtl.Rows(0).Item("ReportStatus").ToString().Trim()
                TxtReportStatus1.Text = dt_OldProjectDtl.Rows(0).Item("ReportStatus1").ToString().Trim()
                TxtLastAmbulatory.Text = dt_OldProjectDtl.Rows(0).Item("LastAmbulatoryEndStudy").ToString().Trim()
                TxtCondition.Text = dt_OldProjectDtl.Rows(0).Item("Condition").ToString().Trim()
                TxtRpId.Text = dt_OldProjectDtl.Rows(0).Item("RpId").ToString().Trim()
                TxtTreatment3.Text = dt_OldProjectDtl.Rows(0).Item("TreatmentIII").ToString().Trim()

                PnlOldProjects.Visible = True

            ElseIf e.CommandName.ToUpper = "DELETE" Then
                SelectednSerailNo = Me.GVWOldProjects.Rows(SelectedIndex).Cells(GRDCell_nSerialNo).Text.ToString().Trim()
                Wstr = "nSerialNo='" & SelectednSerailNo & "'And cStatusIndi<>'D'"
                If Not objHelpDBTable.GetOldProjects(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_OldProjects, estr) Then
                    Me.objCommon.ShowAlert("Error While Getting Data From OldProjects ", Me.Page)
                Else
                    dt_OldProjectDtl = ds_OldProjects.Tables(0)
                    ViewState("VS_OldProjectsDtl") = dt_OldProjectDtl
                End If
                dt_OldProjectDtl.Rows(0).Item("cStatusIndi") = "D"
                dt_OldProjectDtl.AcceptChanges()

                If Not objLambda.Save_OldProjects(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_OldProjects, Me.Session(S_UserID), estr) Then
                    Throw New Exception("Error While Updating Data into OldProjects")
                End If
                BindGrid()
            End If

        Catch ex As Exception
            Me.objCommon.ShowAlert(ex.ToString(), Me.Page)
        End Try

    End Sub

    Protected Sub GVWOldProjects_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVWOldProjects.RowCreated
        Try
            If e.Row.RowType = DataControlRowType.DataRow Or _
     e.Row.RowType = DataControlRowType.Header Or _
     e.Row.RowType = DataControlRowType.Footer Then
                'e.Row.Cells(GRDCell_LnkDelete).Visible = False
                e.Row.Cells(GRDCell_PStatus).Visible = False
                e.Row.Cells(GRDCell_vLocationCode).Visible = False
                e.Row.Cells(GRDCell_vClientCode).Visible = False
                e.Row.Cells(GRDCell_vDrugCode).Visible = False
                e.Row.Cells(GRDCell_TreatmentIII).Visible = False
                e.Row.Cells(GRDCell_dModifyOn).Visible = False
                e.Row.Cells(GRDCell_iModifyBy).Visible = False
                e.Row.Cells(GRDCell_cStatusIndi).Visible = False
                e.Row.Cells(GRDCell_StatusName).Visible = False
                e.Row.Cells(GRDCell_RpId).Visible = False
                'e.Row.Cells(GRDCell_Design).Width = "80"
                'e.Row.Cells(GRDCell_Design).Wrap = False

            End If
        Catch ex As Exception
            Me.objCommon.ShowAlert(ex.ToString(), Me.Page)
        End Try

    End Sub

    Protected Sub GVWOldProjects_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVWOldProjects.RowDataBound
        '
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.FindControl("LnkBtnEdit"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("LnkBtnEdit"), ImageButton).CommandName = "Edit"
                CType(e.Row.FindControl("LnkBtnDelete"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("LnkBtnDelete"), ImageButton).CommandName = "Delete"

                e.Row.Cells(GRDCell_PeriodICheckInDate).Text = If(e.Row.Cells(GRDCell_PeriodICheckInDate).Text <> "&nbsp;", CDate(e.Row.Cells(GRDCell_PeriodICheckInDate).Text).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset, "")
                e.Row.Cells(GRDCell_PeriodIICheckInDate).Text = If(e.Row.Cells(GRDCell_PeriodIICheckInDate).Text <> "&nbsp;", CDate(e.Row.Cells(GRDCell_PeriodIICheckInDate).Text).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset, "")
                e.Row.Cells(GRDCell_PeriodIIICheckInDate).Text = If(e.Row.Cells(GRDCell_PeriodIIICheckInDate).Text <> "&nbsp;", CDate(e.Row.Cells(GRDCell_PeriodIIICheckInDate).Text).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset, "")
                e.Row.Cells(GRDCell_PeriodIVCheckInDate).Text = If(e.Row.Cells(GRDCell_PeriodIVCheckInDate).Text <> "&nbsp;", CDate(e.Row.Cells(GRDCell_PeriodIVCheckInDate).Text).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset, "")
                e.Row.Cells(GRDCell_PeriodIDosingDate).Text = If(e.Row.Cells(GRDCell_PeriodIDosingDate).Text <> "&nbsp;", CDate(e.Row.Cells(GRDCell_PeriodIDosingDate).Text).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset, "")
                e.Row.Cells(GRDCell_PeriodIIDosingDate).Text = If(e.Row.Cells(GRDCell_PeriodIIDosingDate).Text <> "&nbsp;", CDate(e.Row.Cells(GRDCell_PeriodIIDosingDate).Text).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset, "")
                e.Row.Cells(GRDCell_PeriodIIIDosingDate).Text = If(e.Row.Cells(GRDCell_PeriodIIIDosingDate).Text <> "&nbsp;", CDate(e.Row.Cells(GRDCell_PeriodIIIDosingDate).Text).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset, "")
                e.Row.Cells(GRDCell_PeriodIVDosingDate).Text = If(e.Row.Cells(GRDCell_PeriodIVDosingDate).Text <> "&nbsp;", CDate(e.Row.Cells(GRDCell_PeriodIVDosingDate).Text).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset, "")
                e.Row.Cells(GRDCell_PeriodICheckOutDate).Text = If(e.Row.Cells(GRDCell_PeriodICheckOutDate).Text <> "&nbsp;", CDate(e.Row.Cells(GRDCell_PeriodICheckOutDate).Text).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset, "")
                e.Row.Cells(GRDCell_PeriodIICheckOutDate).Text = If(e.Row.Cells(GRDCell_PeriodIICheckOutDate).Text <> "&nbsp;", CDate(e.Row.Cells(GRDCell_PeriodIICheckOutDate).Text).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset, "")
                e.Row.Cells(GRDCell_PeriodIIICheckOutDate).Text = If(e.Row.Cells(GRDCell_PeriodIIICheckOutDate).Text <> "&nbsp;", CDate(e.Row.Cells(GRDCell_PeriodIIICheckOutDate).Text).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset, "")
                e.Row.Cells(GRDCell_PeriodIVCheckOutDate).Text = If(e.Row.Cells(GRDCell_PeriodIVCheckOutDate).Text <> "&nbsp;", CDate(e.Row.Cells(GRDCell_PeriodIVCheckOutDate).Text).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset, "")
                e.Row.Cells(GRDCell_SampleExpectedDate).Text = If(e.Row.Cells(GRDCell_SampleExpectedDate).Text <> "&nbsp;", CDate(e.Row.Cells(GRDCell_SampleExpectedDate).Text).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset, "")
                e.Row.Cells(GRDCell_AnalysisStartDate).Text = If(e.Row.Cells(GRDCell_AnalysisStartDate).Text <> "&nbsp;", CDate(e.Row.Cells(GRDCell_AnalysisStartDate).Text).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset, "")
                e.Row.Cells(GRDCell_AnalysisEndDate).Text = If(e.Row.Cells(GRDCell_AnalysisEndDate).Text <> "&nbsp;", CDate(e.Row.Cells(GRDCell_AnalysisEndDate).Text).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset, "")
                e.Row.Cells(GRDCell_ReportSentDate).Text = If(e.Row.Cells(GRDCell_ReportSentDate).Text <> "&nbsp;", CDate(e.Row.Cells(GRDCell_ReportSentDate).Text).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset, "")
                e.Row.Cells(GRDCell_LastAmbulatoryEndStudy).Text = If(e.Row.Cells(GRDCell_LastAmbulatoryEndStudy).Text <> "&nbsp;", CDate(e.Row.Cells(GRDCell_LastAmbulatoryEndStudy).Text).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset, "")

                'e.Row.Cells(GRDCell_PeriodICheckInDate).Text = If(dtOldProjectsDtl.Rows(e.Row.RowIndex).Item("PeriodICheckInDate").ToString <> "", CDate(dtOldProjectsDtl.Rows(e.Row.RowIndex).Item("PeriodICheckInDate")).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset, "")
                'e.Row.Cells(GRDCell_PeriodIICheckInDate).Text = If(dtOldProjectsDtl.Rows(e.Row.RowIndex).Item("PeriodIICheckInDate").ToString <> "", CDate(dtOldProjectsDtl.Rows(e.Row.RowIndex).Item("PeriodICheckInDate")).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset, "")
                'e.Row.Cells(GRDCell_PeriodIIICheckInDate).Text = If(dtOldProjectsDtl.Rows(e.Row.RowIndex).Item("PeriodIIICheckInDate").ToString <> "", CDate(dtOldProjectsDtl.Rows(e.Row.RowIndex).Item("PeriodICheckInDate")).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset, "")
                'e.Row.Cells(GRDCell_PeriodIVCheckInDate).Text = If(dtOldProjectsDtl.Rows(e.Row.RowIndex).Item("PeriodIVCheckInDate").ToString <> "", CDate(dtOldProjectsDtl.Rows(e.Row.RowIndex).Item("PeriodICheckInDate")).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset, "")
                'e.Row.Cells(GRDCell_PeriodIDosingDate).Text = If(dtOldProjectsDtl.Rows(e.Row.RowIndex).Item("PeriodIDosingDate").ToString <> "", CDate(dtOldProjectsDtl.Rows(e.Row.RowIndex).Item("PeriodICheckInDate")).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset, "")
                'e.Row.Cells(GRDCell_PeriodIIDosingDate).Text = If(dtOldProjectsDtl.Rows(e.Row.RowIndex).Item("PeriodIIDosingDate").ToString <> "", CDate(dtOldProjectsDtl.Rows(e.Row.RowIndex).Item("PeriodICheckInDate")).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset, "")
                'e.Row.Cells(GRDCell_PeriodIIIDosingDate).Text = If(dtOldProjectsDtl.Rows(e.Row.RowIndex).Item("PeriodIIIDosingDate").ToString <> "", CDate(dtOldProjectsDtl.Rows(e.Row.RowIndex).Item("PeriodICheckInDate")).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset, "")
                'e.Row.Cells(GRDCell_PeriodIVDosingDate).Text = If(dtOldProjectsDtl.Rows(e.Row.RowIndex).Item("PeriodIVDosingDate").ToString <> "", CDate(dtOldProjectsDtl.Rows(e.Row.RowIndex).Item("PeriodICheckInDate")).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset, "")
                'e.Row.Cells(GRDCell_PeriodICheckOutDate).Text = If(dtOldProjectsDtl.Rows(e.Row.RowIndex).Item("PeriodICheckOutDate").ToString <> "", CDate(dtOldProjectsDtl.Rows(e.Row.RowIndex).Item("PeriodICheckInDate")).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset, "")
                'e.Row.Cells(GRDCell_PeriodIICheckOutDate).Text = If(dtOldProjectsDtl.Rows(e.Row.RowIndex).Item("PeriodIICheckOutDate").ToString <> "", CDate(dtOldProjectsDtl.Rows(e.Row.RowIndex).Item("PeriodICheckInDate")).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset, "")
                'e.Row.Cells(GRDCell_PeriodIIICheckOutDate).Text = If(dtOldProjectsDtl.Rows(e.Row.RowIndex).Item("PeriodIIICheckOutDate").ToString <> "", CDate(dtOldProjectsDtl.Rows(e.Row.RowIndex).Item("PeriodICheckInDate")).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset, "")
                'e.Row.Cells(GRDCell_PeriodIVCheckOutDate).Text = If(dtOldProjectsDtl.Rows(e.Row.RowIndex).Item("PeriodIVCheckOutDate").ToString <> "", CDate(dtOldProjectsDtl.Rows(e.Row.RowIndex).Item("PeriodICheckInDate")).ToString("dd-MMM-yyyy HH:mm tt") + strServerOffset, "")

                'e.Row.Cells(GRDCell_Design).Width = "82"
            End If
            If e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(GRDCell_nSerialNo).Text = "Serial No"
                e.Row.Cells(GRDCell_ProjectNumber).Text = "Project No"
                e.Row.Cells(GRDCell_NumberofSubjects).Text = "No Of Subjects"
                e.Row.Cells(GRDCell_Design).Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Design&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                'e.Row.Cells(GRDCell_Design).Width = "82"
            End If



        Catch ex As Exception
            Me.objCommon.ShowAlert(ex.ToString(), Me.Page)
        End Try

    End Sub

    Protected Sub GVWOldProjects_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GVWOldProjects.RowDeleting

    End Sub

    Protected Sub GVWOldProjects_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GVWOldProjects.RowEditing
        Try
            PnlOldProjects.Visible = True
        Catch ex As Exception
            Me.objCommon.ShowAlert(ex.ToString(), Me.Page)
        End Try

    End Sub

#End Region

#Region "Error Handler"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        'Me.lblerrormsg.Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)

    End Sub
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        'Me.lblerrormsg.Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub
#End Region

#Region "Set project"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim ProjectNum As String = String.Empty
        ProjectNum = HProjectId.Value
        Response.Redirect("frmOldProjects.aspx?mode=4&value=" & HProjectId.Value)
    End Sub

#End Region

#Region "Button Events"

    Protected Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click

        Dim estr As String = String.Empty
        Dim dt_OldProjectDtl As New DataTable
        Dim ds_OldProjects As New DataSet


        Try
            dt_OldProjectDtl = CType(ViewState("VS_OldProjectsDtl"), DataTable)

            'If TxtProjectNo.Text <> "" Then
            '    dt_OldProjectDtl.Rows(0).Item("ProjectNumber") = TxtProjectNo.Text
            'End If
            If TxtNoOfSubjects.Text <> "" Then
                dt_OldProjectDtl.Rows(0).Item("NumberofSubjects") = TxtNoOfSubjects.Text
            End If

            dt_OldProjectDtl.Rows(0).Item("Submission") = TxtSubmission.Text
            dt_OldProjectDtl.Rows(0).Item("StudyDescription") = TxtStudyDescription.Text
            dt_OldProjectDtl.Rows(0).Item("ProductName") = TxtProductName.Text
            If DdllstForDrugName.SelectedIndex <> 0 Then
                dt_OldProjectDtl.Rows(0).Item("vDrugCode") = Me.DdllstForDrugName.SelectedValue.ToString().Trim()
            End If
            dt_OldProjectDtl.Rows(0).Item("ProjectStatus") = TxtStatus.Text
            dt_OldProjectDtl.Rows(0).Item("ProjectCoordinator") = TxtProjectCoordinator.Text
            dt_OldProjectDtl.Rows(0).Item("SponsorName") = TxtSponsorName.Text
            If DrpLstSponsorName.SelectedIndex <> 0 Then
                dt_OldProjectDtl.Rows(0).Item("vClientCode") = Me.DrpLstSponsorName.SelectedValue.ToString().Trim()
            End If
            dt_OldProjectDtl.Rows(0).Item("ReportMonth") = TxtReportMonth.Text
            dt_OldProjectDtl.Rows(0).Item("ProjectCode") = TxtProjectCode.Text
            dt_OldProjectDtl.Rows(0).Item("years") = TxtYear.Text
            dt_OldProjectDtl.Rows(0).Item("Months") = TxtMonth.Text
            dt_OldProjectDtl.Rows(0).Item("Design") = TxtDesign.Text
            dt_OldProjectDtl.Rows(0).Item("TreatmentI") = TxtTreatment1.Text
            dt_OldProjectDtl.Rows(0).Item("TreatmentII") = TxtTreatment2.Text
            dt_OldProjectDtl.Rows(0).Item("ReferenceDrugProductA") = TxtReferenceDrugProductA.Text
            dt_OldProjectDtl.Rows(0).Item("ReferenceDrugProductB") = TxtReferenceDrugProductB.Text
            dt_OldProjectDtl.Rows(0).Item("TestDrugProduct") = TxtTestDrugProduct.Text
            dt_OldProjectDtl.Rows(0).Item("TestDrugProductC") = TxtTestDrugProductC.Text
            dt_OldProjectDtl.Rows(0).Item("TestDrugProductB") = TxtTestDrugProductB.Text
            dt_OldProjectDtl.Rows(0).Item("SponsorAddress") = TxtSponsorAddress.Text
            dt_OldProjectDtl.Rows(0).Item("SponsorTelephone") = TxtSponsorTelephone.Text
            dt_OldProjectDtl.Rows(0).Item("SponsorFax") = TxtSponsorFax.Text
            dt_OldProjectDtl.Rows(0).Item("SponsorContactPerson") = TxtSponsorContactPerson.Text
            dt_OldProjectDtl.Rows(0).Item("Groups") = TxtGroup.Text
            dt_OldProjectDtl.Rows(0).Item("TestDrugProductD") = TxtTestDrugProductD.Text
            dt_OldProjectDtl.Rows(0).Item("StudyResults") = TxtStudyResults.Text
            dt_OldProjectDtl.Rows(0).Item("EArchiving") = TxtEArchiving.Text
            dt_OldProjectDtl.Rows(0).Item("RetentionPeriod") = TxtRetentionPeriod.Text
            dt_OldProjectDtl.Rows(0).Item("StatusName") = TxtStatusName.Text
            dt_OldProjectDtl.Rows(0).Item("cStatusIndi") = "E"
            If Me.TxtPeriod1CheckInDate.Text <> "" Then
                dt_OldProjectDtl.Rows(0).Item("PeriodICheckInDate") = TxtPeriod1CheckInDate.Text
            End If
            If Me.TxtPeriod2CheckInDate.Text <> "" Then
                dt_OldProjectDtl.Rows(0).Item("PeriodIICheckInDate") = TxtPeriod2CheckInDate.Text
            End If
            If Me.TxtPeriod3CheckInDate.Text <> "" Then
                dt_OldProjectDtl.Rows(0).Item("PeriodIIICheckInDate") = TxtPeriod3CheckInDate.Text
            End If
            If Me.TxtPeriod4CheckInDate.Text <> "" Then
                dt_OldProjectDtl.Rows(0).Item("PeriodIVCheckInDate") = TxtPeriod4CheckInDate.Text
            End If
            If Me.TxtPeriod1DosingDate.Text <> "" Then
                dt_OldProjectDtl.Rows(0).Item("PeriodIDosingDate") = TxtPeriod1DosingDate.Text
            End If
            If Me.TxtPeriod2DosingDate.Text <> "" Then
                dt_OldProjectDtl.Rows(0).Item("PeriodIIDosingDate") = TxtPeriod2DosingDate.Text
            End If
            If Me.TxtPeriod3DosingDate.Text <> "" Then
                dt_OldProjectDtl.Rows(0).Item("PeriodIIIDosingDate") = TxtPeriod3DosingDate.Text
            End If
            If Me.TxtPeriod4DosingDate.Text <> "" Then
                dt_OldProjectDtl.Rows(0).Item("PeriodIVDosingDate") = TxtPeriod4DosingDate.Text
            End If
            If Me.TxtPeriod1CheckOutDate.Text <> "" Then
                dt_OldProjectDtl.Rows(0).Item("PeriodICheckOutDate") = TxtPeriod1CheckOutDate.Text
            End If
            If Me.TxtPeriod2CheckOutDate.Text <> "" Then
                dt_OldProjectDtl.Rows(0).Item("PeriodIICheckOutDate") = TxtPeriod2CheckOutDate.Text
            End If
            If Me.TxtPeriod3CheckOutDate.Text <> "" Then
                dt_OldProjectDtl.Rows(0).Item("PeriodIIICheckOutDate") = TxtPeriod3CheckOutDate.Text
            End If
            If Me.TxtPeriod4CheckOutDate.Text <> "" Then
                dt_OldProjectDtl.Rows(0).Item("PeriodIVCheckOutDate") = TxtPeriod4CheckOutDate.Text
            End If
            dt_OldProjectDtl.Rows(0).Item("LetterIssued") = TxtLetterIssueds.Text
            If Me.TxtDateOfLetterIssued.Text <> "" Then
                dt_OldProjectDtl.Rows(0).Item("Dateofletterissue") = TxtDateOfLetterIssued.Text
            End If
            dt_OldProjectDtl.Rows(0).Item("Remarks") = TxtRemarks.Text
            If Me.TxtSampleExpectedDate.Text <> "" Then
                dt_OldProjectDtl.Rows(0).Item("SampleExpectedDate") = TxtSampleExpectedDate.Text
            End If
            If Me.TxtAnalysisStartDate.Text <> "" Then
                dt_OldProjectDtl.Rows(0).Item("AnalysisStartDate") = TxtAnalysisStartDate.Text
            End If
            If Me.TxtAnalysisEndDate.Text <> "" Then
                dt_OldProjectDtl.Rows(0).Item("AnalysisEndDate") = TxtAnalysisEndDate.Text
            End If
            If Me.TxtReportSentDate.Text <> "" Then
                dt_OldProjectDtl.Rows(0).Item("ReportSentDate") = TxtReportSentDate.Text
            End If
            If Me.TxtReportYear.Text <> "" Then
                dt_OldProjectDtl.Rows(0).Item("ReportYear") = TxtReportYear.Text
            End If
            If Me.TxtReturnToSponsor.Text <> "" Then
                dt_OldProjectDtl.Rows(0).Item("ReturntoSponsor") = TxtReturnToSponsor.Text
            End If
            If TxtNumberOfStandbySubjects.Text <> "" Then
                dt_OldProjectDtl.Rows(0).Item("NumberofStandbySubjects") = TxtNumberOfStandbySubjects.Text
            End If
            If TxtNumberOfSamples.Text <> "" Then
                dt_OldProjectDtl.Rows(0).Item("NumberofSamples") = TxtNumberOfSamples.Text
            End If
            If TxtReportStatuss.Text <> "" Then
                dt_OldProjectDtl.Rows(0).Item("ReportStatus") = TxtReportStatuss.Text
            End If
            If TxtReportStatus1.Text <> "" Then
                dt_OldProjectDtl.Rows(0).Item("ReportStatus1") = TxtReportStatus1.Text
            End If
            If Me.TxtLastAmbulatory.Text <> "" Then
                dt_OldProjectDtl.Rows(0).Item("LastAmbulatoryEndStudy") = TxtLastAmbulatory.Text
            End If
            dt_OldProjectDtl.Rows(0).Item("Condition") = TxtCondition.Text
            dt_OldProjectDtl.Rows(0).Item("Location") = TxtLocation.Text
            dt_OldProjectDtl.Rows(0).Item("RpId") = TxtRpId.Text
            dt_OldProjectDtl.Rows(0).Item("TreatmentIII") = TxtTreatment3.Text

            ds_OldProjects.Tables.Add(dt_OldProjectDtl)
            ds_OldProjects.AcceptChanges()

            If Not objLambda.Save_OldProjects(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_OldProjects, Me.Session(S_UserID), estr) Then
                Throw New Exception("Error While Updating Data Into OldProjects")
            End If
            Me.objCommon.ShowAlert("Record Updated Successfully", Me.Page)
            'GVWOldProjects.DataSource = Me.ViewState("VS_OldProjectsDtl")
            'GVWOldProjects.DataBind()
            PnlOldProjects.Visible = False
            BindGrid()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...........BtnSave_Click")
        End Try



    End Sub

    Protected Sub BtnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click
        Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        PnlOldProjects.Visible = False
    End Sub

#End Region

#Region "Selected index change"

    Protected Sub DrpLstSponsorName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DrpLstSponsorName.SelectedIndexChanged
        If DrpLstSponsorName.SelectedIndex = 0 Then
            Me.objCommon.ShowAlert("Please Select Any Of The Sponsor Name", Me.Page)
        Else
            TxtSponsorName.Text = DrpLstSponsorName.SelectedItem.ToString.Trim()

        End If
    End Sub

    Protected Sub DdllstForDrugName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdllstForDrugName.SelectedIndexChanged
        If DdllstForDrugName.SelectedIndex = 0 Then
            Me.objCommon.ShowAlert("Please Select Any Of The Product Name", Me.Page)
        Else
            Me.TxtProductName.Text = DdllstForDrugName.SelectedItem.ToString().Trim()
        End If
    End Sub

#End Region

End Class
