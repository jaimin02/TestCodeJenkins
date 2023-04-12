
Partial Class frmRptSubjectPIFAuditTrail
    Inherits System.Web.UI.Page


#Region "VARIABLE DECLARATION "

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
    Private rPage As RepoPage
    Private dsLanguages As DataSet
    Private eStr_Retu As String = ""
#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtproject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(sender, e)
            End If
            GenCall()
        End If
    End Sub

#End Region

#Region "GENCALL"

    Private Function GenCall() As Boolean

        Dim eStr As String = ""
        Dim wStr As String = ""
        Dim dt_WorkspaceSubjectmst As DataTable = Nothing
        Dim ds_WorkspaceSubjectmst As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            
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

#Region "GENCALL_SHOW_UI"

    Private Function GenCall_ShowUI(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, ByVal dt_WorkspaceSubjectmst As DataTable) As Boolean
        Dim WorkspaceId As String = ""
        Dim estr, eStrLang As String
        Dim dsWorkspace As New DataSet
        dsLanguages = New DataSet

        Page.Title = ":: Subject PIF Audit Trail Report   :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
        CType(Master.FindControl("lblHeading"), Label).Text = "Subject PIF Audit Trial Report"

        If Not objHelp.getSubjectLanguageMst(" ", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, dsLanguages, eStrLang) Then
            Me.ShowErrorMessage(eStrLang, "")
        End If
        ViewState("SubjectLanguages") = dsLanguages
        If Not IsNothing(Me.Request.QueryString("workspaceid")) Then
            WorkspaceId = Me.Request.QueryString("workspaceid").Trim()
        End If

        If WorkspaceId.Trim() <> "" Then

            If Not Me.objHelp.getworkspacemst("vWorkspaceId='" & WorkspaceId.Trim() & "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        dsWorkspace, estr) Then
                Me.ShowErrorMessage(estr, "")
                GenCall_ShowUI = False
                Exit Function
            End If

            Me.txtproject.Text = dsWorkspace.Tables(0).Rows(0).Item("vWorkSpaceDesc")
            Me.HProjectId.Value = WorkspaceId.Trim()

        End If

        fillvalues()

        Me.Image1.ImageUrl = "~/Images/demo.jpg"

        GenCall_ShowUI = True
    End Function

#End Region

#Region "Generate Report "

    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        Dim str As String = ""
        Dim FileName As String = ""
        Dim isReportComplete As Boolean = False

        Try
            Dim a As New ArrayList
            a.Add("sad")

            FileName = GetReportName() + ".xls"
            FileName = Server.MapPath("~/ExcelReportFile/" + FileName)

            OpenReport(FileName)

            ReportHeader()

            ReportDetail()

            isReportComplete = True

        Catch ex As Exception
            isReportComplete = False
            Me.ShowErrorMessage(ex.Message, "")
        Finally
            If Not rPage Is Nothing Then
                rPage.CloseReport()
                rPage = Nothing
            End If
        End Try

        If isReportComplete = True Then
            'ReportGeneralFunction.ExportFileAtWeb(ReportGeneralFunction.ExportFileAsEnum.ExportAsXLS, HttpContext.Current.Response, FileName, Me.Server.MapPath(""))
            'ReportGeneralFunction.ExportFileAtWeb(ReportGeneralFunction.ExportFileAsEnum.ExportAsXLS, HttpContext.Current.Response, FileName)
            Dim curContext As System.Web.HttpContext = System.Web.HttpContext.Current

            curContext.Response.Clear()
            curContext.Response.Buffer = True

            curContext.Response.AddHeader("content-disposition", "attachment; filename=" + System.Web.HttpUtility.UrlEncode(Path.GetFileName(FileName), System.Text.Encoding.UTF8))
            curContext.Response.ContentType = "application/octet-stream"
            curContext.Response.WriteFile(FileName)
            curContext.Response.Flush()
            curContext.Response.End()
            HttpContext.Current.ApplicationInstance.CompleteRequest()
        End If

    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

#End Region

#Region "Report Helper Functions"

    Private Function OpenReport(ByVal ReportFileName_1 As String) As Boolean
        '' This Function open file on physical memory(In HardDist)          
        If File.Exists(ReportFileName_1) = True Then
            File.Delete(ReportFileName_1)
        End If
        rPage = New RepoPage(ReportFileName_1)
    End Function

    Private Sub ReportHeader()
        Dim rRow As RepoRow
        Dim rCell As RepoCell

        rRow = New RepoRow
        rCell = rRow.AddCell("CompanyTitle")
        rCell.Alignment = RepoCell.AlignmentEnum.CenterMiddle
        rCell.FontBold = True
        rCell.FontSize = 14
        rCell.Value = System.Configuration.ConfigurationManager.AppSettings("Client")
        rCell.NoofCellContain = 10
        rCell.FontColor = Drawing.Color.Maroon
        rCell.BackgroundColor = Drawing.Color.White
        rPage.Say(rRow)

        rRow = New RepoRow
        rCell = rRow.AddCell("ReportTitle")
        rCell.Value = "Subject PIF AuditTrial Report"
        rCell.Alignment = RepoCell.AlignmentEnum.CenterMiddle
        rCell.FontBold = True
        rCell.FontSize = 12
        rCell.FontColor = Drawing.Color.Maroon
        rCell.NoofCellContain = 10
        rPage.Say(rRow)
        '*****
        'rRow = New RepoRow
        'rCell = rRow.AddCell("ClientName")
        'rCell.Value = "Comments Done on : " & Me.RblCommentsOn.SelectedItem.Text.Trim()
        'rCell.Alignment = RepoCell.AlignmentEnum.CenterMiddle
        'rCell.FontBold = True
        'rCell.FontSize = 12
        'rCell.FontColor = Drawing.Color.Maroon
        'rCell.NoofCellContain = 10
        'rPage.Say(rRow)
        '******

        rRow = New RepoRow
        rCell = rRow.AddCell("ProjectName")
        '***Added d

        If (Me.txtproject.Text.Trim() = "") Then
            rCell.Value = ""

        Else
            rCell.Value = "Project: " & Me.txtproject.Text.Trim()
            rCell.Alignment = RepoCell.AlignmentEnum.CenterMiddle
            rCell.FontBold = True
            rCell.FontSize = 12
            rCell.FontColor = Drawing.Color.Maroon
            rCell.NoofCellContain = 10
            rPage.Say(rRow)
        End If
        rRow = New RepoRow
        rCell = rRow.AddCell("DatePeriod")
        '****added d
        If (Me.chkAll.Checked) Then
            rCell.Value = ""
        Else
            rCell.Value = "Period: " & Me.txtFromDate.Text.Trim() & " To " & Me.txtToDate.Text.Trim()
            rCell.Alignment = RepoCell.AlignmentEnum.CenterMiddle
            rCell.FontBold = True
            rCell.FontSize = 12
            rCell.FontColor = Drawing.Color.Maroon
            rCell.NoofCellContain = 10
            rPage.Say(rRow)
        End If

        rPage.SayBlankRow()

    End Sub

    Private Function masterRow() As RepoRow
        Dim rRow As RepoRow
        Dim rCell As RepoCell
        Dim i As Integer

        rRow = New RepoRow

        rCell = New RepoCell("SubjectId")
        rRow.AddCell(rCell)

        rCell = New RepoCell("ModifiedBy")
        rRow.AddCell(rCell)

        rCell = New RepoCell("ModifiedOn")
        rRow.AddCell(rCell)


        rCell = New RepoCell("Remark")
        rRow.AddCell(rCell)


        rCell = New RepoCell("EnrollmentDate")
        rRow.AddCell(rCell)

        rCell = New RepoCell("SurName")
        rRow.AddCell(rCell)

        rCell = New RepoCell("MiddleName")
        rRow.AddCell(rCell)

      

        rCell = New RepoCell("FirstName")
        rRow.AddCell(rCell)

        rCell = New RepoCell("Initials")
        rRow.AddCell(rCell)


        rCell = New RepoCell("BirthDate")
        rRow.AddCell(rCell)

        rCell = New RepoCell("Sex")
        rRow.AddCell(rCell)


        rCell = New RepoCell("EducationQualification")
        rRow.AddCell(rCell)

        rCell = New RepoCell("ICF_Languages")
        rRow.AddCell(rCell)

        rCell = New RepoCell("Occupation")
        rRow.AddCell(rCell)


        rCell = New RepoCell("BloodGroup")
        rRow.AddCell(rCell)


        rCell = New RepoCell("Rh")
        rRow.AddCell(rCell)


        rCell = New RepoCell("ProofOfAge1")
        rRow.AddCell(rCell)


        rCell = New RepoCell("ProofOfAge2")
        rRow.AddCell(rCell)


        rCell = New RepoCell("ProofOfAge3")
        rRow.AddCell(rCell)


     

    

      

        rCell = New RepoCell("PerAdd1")
        rRow.AddCell(rCell)


        rCell = New RepoCell("PerAdd2")
        rRow.AddCell(rCell)


        rCell = New RepoCell("PerAdd3")
        rRow.AddCell(rCell)


        rCell = New RepoCell("PerTelephoneNo")
        rRow.AddCell(rCell)



        rCell = New RepoCell("PerCity")
        rRow.AddCell(rCell)


        rCell = New RepoCell("PinCity")
        rRow.AddCell(rCell)


        rCell = New RepoCell("PerCountry")
        rRow.AddCell(rCell)
        
        rCell = New RepoCell("LocalAdd11")
        rRow.AddCell(rCell)


        rCell = New RepoCell("LocalAdd12")
        rRow.AddCell(rCell)


        rCell = New RepoCell("LocalAdd13")
        rRow.AddCell(rCell)


        rCell = New RepoCell("LocalTelephoneno1")
        rRow.AddCell(rCell)


        rCell = New RepoCell("LocalCity1")
        rRow.AddCell(rCell)


 


        rCell = New RepoCell("LocalCountry1")
        rRow.AddCell(rCell)


        rCell = New RepoCell("LocalAdd21")
        rRow.AddCell(rCell)


        rCell = New RepoCell("LocalAdd22")
        rRow.AddCell(rCell)


        rCell = New RepoCell("LocalAdd23")
        rRow.AddCell(rCell)


        rCell = New RepoCell("LocalTelephoneno2")
        rRow.AddCell(rCell)


        rCell = New RepoCell("LocalCity2")
        rRow.AddCell(rCell)



        rCell = New RepoCell("LocalCountry2")
        rRow.AddCell(rCell)


        rCell = New RepoCell("ContactName1")
        rRow.AddCell(rCell)


        rCell = New RepoCell("ContactAddress11")
        rRow.AddCell(rCell)



        rCell = New RepoCell("ContactAddress12")
        rRow.AddCell(rCell)


        rCell = New RepoCell("ContactAddress13")
        rRow.AddCell(rCell)



        rCell = New RepoCell("ContactTelephoneNo1")
        rRow.AddCell(rCell)




        rCell = New RepoCell("ContactCity1")
        rRow.AddCell(rCell)






        rCell = New RepoCell("ContactCountry1")
        rRow.AddCell(rCell)




        rCell = New RepoCell("ContactName2")
        rRow.AddCell(rCell)


        rCell = New RepoCell("ContactAddress21")
        rRow.AddCell(rCell)



        rCell = New RepoCell("ContactAddress22")
        rRow.AddCell(rCell)


        rCell = New RepoCell("ContactAddress23")
        rRow.AddCell(rCell)






        rCell = New RepoCell("ContactTelephoneNo2")
        rRow.AddCell(rCell)




        rCell = New RepoCell("ContactCity2")
        rRow.AddCell(rCell)




        rCell = New RepoCell("OfficeAddress")
        rRow.AddCell(rCell)


        rCell = New RepoCell("OfficeTelephoneno")
        rRow.AddCell(rCell)




        rCell = New RepoCell("RefferedBy")
        rRow.AddCell(rCell)



        rCell = New RepoCell("FoodHabit")
        rRow.AddCell(rCell)


        rCell = New RepoCell("Height")
        rRow.AddCell(rCell)


        rCell = New RepoCell("Weight")
        rRow.AddCell(rCell)


        rCell = New RepoCell("BMI")
        rRow.AddCell(rCell)



        rCell = New RepoCell("LastMenstrualDate")
        rRow.AddCell(rCell)


        rCell = New RepoCell("LastMenstrualDays")
        rRow.AddCell(rCell)


        'rCell = New RepoCell("LastMenstrualDate")
        'rRow.AddCell(rCell)


        rCell = New RepoCell("LastDeliveryDate")
        rRow.AddCell(rCell)



        rCell = New RepoCell("NoOfChildren")
        rRow.AddCell(rCell)


        rCell = New RepoCell("NoOfChildrenDied")
        rRow.AddCell(rCell)



        rCell = New RepoCell("RemarksIfDied")
        rRow.AddCell(rCell)

        rCell = New RepoCell("ChildrenHealth")
        rRow.AddCell(rCell)

        rCell = New RepoCell("ChildrenHealthRemark")
        rRow.AddCell(rCell)



        rCell = New RepoCell("Para")
        rRow.AddCell(rCell)




        rCell = New RepoCell("Gravida")
        rRow.AddCell(rCell)

        rCell = New RepoCell("Abortion")
        rRow.AddCell(rCell)

        rCell = New RepoCell("AbortionDate")
        rRow.AddCell(rCell)

        rCell = New RepoCell("Regular")
        rRow.AddCell(rCell)


        rCell = New RepoCell("Loctating")
        rRow.AddCell(rCell)



        rCell = New RepoCell("VoluteerinBearingAge")
        rRow.AddCell(rCell)



        rCell = New RepoCell("FamilyPlannigSelf")
        rRow.AddCell(rCell)


        rCell = New RepoCell("Contraception")
        rRow.AddCell(rCell)


        rCell = New RepoCell("Barrier")
        rRow.AddCell(rCell)


        rCell = New RepoCell("Pills")
        rRow.AddCell(rCell)


        rCell = New RepoCell("Rhythm")
        rRow.AddCell(rCell)


        rCell = New RepoCell("IUCD")
        rRow.AddCell(rCell)

        rCell = New RepoCell("OtherRemark")
        rRow.AddCell(rCell)

        rCell = New RepoCell("FemaleRemark")
        rRow.AddCell(rCell)


        rCell = New RepoCell("CigarettsHabitDetails")
        rRow.AddCell(rCell)

        rCell = New RepoCell("CigarettsConsumptionDetail")
        rRow.AddCell(rCell)


        rCell = New RepoCell("CigarettsStoppedSince")
        rRow.AddCell(rCell)


        rCell = New RepoCell("BidiHabitDetails")
        rRow.AddCell(rCell)


        rCell = New RepoCell("BidiConsumptionDetail")
        rRow.AddCell(rCell)


        rCell = New RepoCell("BidiStoppedSince")
        rRow.AddCell(rCell)


        rCell = New RepoCell("GutkhaHabitDetails")
        rRow.AddCell(rCell)


        rCell = New RepoCell("GutkhaConsumptionDetail")
        rRow.AddCell(rCell)


        rCell = New RepoCell("GutkhaStoppedSince")
        rRow.AddCell(rCell)

        rCell = New RepoCell("SupariTobacoHabitDetails")
        rRow.AddCell(rCell)


        rCell = New RepoCell("SupariTobacoConsumptionDetail")
        rRow.AddCell(rCell)


        rCell = New RepoCell("SupariTobacoStoppedSince")
        rRow.AddCell(rCell)


        rCell = New RepoCell("TobacowithSupariHabitDetails")
        rRow.AddCell(rCell)


        rCell = New RepoCell("TobacowithSupariConsumptionDetail")
        rRow.AddCell(rCell)


        rCell = New RepoCell("TobacowithSupariStoppedSince")
        rRow.AddCell(rCell)


        'rCell = New RepoCell("TobacowithSupariHabitDetails")
        'rRow.AddCell(rCell)


        rCell = New RepoCell("AlchoholHabitDetails")
        rRow.AddCell(rCell)

        rCell = New RepoCell("AlchoholConsumptionDetail")
        rRow.AddCell(rCell)


        rCell = New RepoCell("AlchoholStoppedSince")
        rRow.AddCell(rCell)


        rCell = New RepoCell("OthersHabitDetails")
        rRow.AddCell(rCell)


        rCell = New RepoCell("OthersConsumptionDetail")
        rRow.AddCell(rCell)


        rCell = New RepoCell("OthersStoppedSince")
        rRow.AddCell(rCell)



        For i = 0 To rRow.CellCount - 1
            rRow.Cell(i).FontSize = 8
            rRow.Cell(i).LineBottom.LineType = RepoLine.LineTypeEnum.SingleLine
            rRow.Cell(i).LineTop.LineType = RepoLine.LineTypeEnum.SingleLine
            rRow.Cell(i).LineRight.LineType = RepoLine.LineTypeEnum.SingleLine
            rRow.Cell(i).LineLeft.LineType = RepoLine.LineTypeEnum.SingleLine
        Next i

        Return rRow

    End Function



    Private Sub PrintHeader()
        Dim rRow As RepoRow
        Dim index As Integer
        Try

        
            rRow = New RepoRow

            rRow = masterRow()
            rRow.Cell("SubjectId").Value = "Subject Id" 'Me.RblCommentsOn.SelectedItem.Text.Trim() + 
            rRow.Cell("ModifiedBy").Value = "Modify By"
            rRow.Cell("ModifiedOn").Value = "Modified On"
            rRow.Cell("Remark").Value = "Remark"
            rRow.Cell("EnrollmentDate").Value = "Enrollment Date"
            rRow.Cell("MiddleName").Value = "Middle Name"
            rRow.Cell("SurName").Value = "SurName"
            rRow.Cell("FirstName").Value = "First Name"
            rRow.Cell("Initials").Value = "Initials"
            rRow.Cell("BirthDate").Value = "Birth Date"
            rRow.Cell("Sex").Value = "Sex"
            rRow.Cell("EducationQualification").Value = "Education Qualification"
            rRow.Cell("ICF_Languages").Value = "ICF_Languages"
            rRow.Cell("Occupation").Value = "Occupation"
            rRow.Cell("BloodGroup").Value = "BloodGroup"
            rRow.Cell("Rh").Value = "Rh"
            rRow.Cell("ProofOfAge1").Value = "ProofOfAge1"
            rRow.Cell("ProofOfAge2").Value = "ProofOfAge2"
            rRow.Cell("ProofOfAge3").Value = "ProofOfAge3"
            rRow.Cell("PerAdd1").Value = "PerAdd1"
            rRow.Cell("PerAdd2").Value = "PerAdd2"
            rRow.Cell("PerAdd3").Value = "PerAdd3"
            rRow.Cell("PerTelephoneNo").Value = "PerTelephoneNo"
            rRow.Cell("PerCity").Value = "PerCity"
            rRow.Cell("PinCity").Value = "PinCity"
            rRow.Cell("PerCountry").Value = "PerCountry"
            rRow.Cell("LocalAdd11").Value = "LocalAdd11"
            rRow.Cell("LocalAdd12").Value = "LocalAdd12"
            rRow.Cell("LocalAdd13").Value = "LocalAdd13"
            rRow.Cell("LocalTelephoneno1").Value = "Local Telephoneno1"
            rRow.Cell("LocalCity1").Value = "Local City1"
            rRow.Cell("LocalCountry1").Value = "Local Country1"
            rRow.Cell("LocalAdd21").Value = "LocalAdd21"
            rRow.Cell("LocalAdd22").Value = "LocalAdd22"
            rRow.Cell("LocalAdd23").Value = "Local Add23"
            rRow.Cell("LocalTelephoneno2").Value = "Local Telephoneno2"
            rRow.Cell("LocalCity2").Value = "LocalCity2"
            rRow.Cell("LocalCountry2").Value = "Local Country2"
            rRow.Cell("ContactName1").Value = "Contact Name1"
            rRow.Cell("ContactAddress11").Value = "Contact Address11"
            rRow.Cell("ContactAddress12").Value = "Contact Address12"
            rRow.Cell("ContactAddress13").Value = "Contact Address13"
            rRow.Cell("ContactTelephoneNo1").Value = "Contact TelephoneNo1"
            rRow.Cell("ContactCity1").Value = "Contact City1"
            rRow.Cell("ContactCountry1").Value = "Contact Country1"
            rRow.Cell("ContactName2").Value = "Contact Name2"
            rRow.Cell("ContactAddress21").Value = "Contact Address21"
            rRow.Cell("ContactAddress22").Value = "Contact Address22"
            rRow.Cell("ContactAddress23").Value = "Contact Address23"
            rRow.Cell("ContactTelephoneNo2").Value = "ContactTelephoneNo2"
            rRow.Cell("ContactCity2").Value = "Contact City2"
            rRow.Cell("OfficeAddress").Value = "Office Address"
            rRow.Cell("OfficeTelephoneno").Value = "Office Telephone no"
            rRow.Cell("RefferedBy").Value = "Reffered By"
            rRow.Cell("FoodHabit").Value = "Food Habit"
            rRow.Cell("Height").Value = "Height"
            rRow.Cell("Weight").Value = "Weight"
            rRow.Cell("BMI").Value = "BMI"
            rRow.Cell("LastMenstrualDate").Value = "Last Menstrual Date"
            rRow.Cell("LastMenstrualDays").Value = "Last Menstrual Days"
            'rRow.Cell("LastMenstrualDate").Value = "Last Menstrual Date"
            rRow.Cell("LastDeliveryDate").Value = "Last Delivery Date"
            rRow.Cell("NoOfChildren").Value = "No Of Children"
            rRow.Cell("NoOfChildrenDied").Value = "No Of Children Died"
            rRow.Cell("RemarksIfDied").Value = "Remarks If Died"
            rRow.Cell("ChildrenHealth").Value = "Children Health"
            rRow.Cell("ChildrenHealthRemark").Value = "Children Health Remark"
            rRow.Cell("Para").Value = "Para"
            rRow.Cell("Gravida").Value = "Gravida"
            rRow.Cell("Abortion").Value = "Abortion"
            rRow.Cell("AbortionDate").Value = "Abortion Date"
            rRow.Cell("Regular").Value = "Regular"
            rRow.Cell("Loctating").Value = "Loctating"
            rRow.Cell("VoluteerinBearingAge").Value = "Voluteer in Bearing Age"
            rRow.Cell("FamilyPlannigSelf").Value = "Family Plannig Self"
            rRow.Cell("Contraception").Value = "Contraception"
            rRow.Cell("Barrier").Value = "Barrier"
            rRow.Cell("Pills").Value = "Pills"
            rRow.Cell("Rhythm").Value = "Rhythm"
            rRow.Cell("IUCD").Value = "IUCD"
            rRow.Cell("OtherRemark").Value = "Other Remark"
            rRow.Cell("FemaleRemark").Value = "Female Remark"
            rRow.Cell("CigarettsHabitDetails").Value = "Cigaretts Habit Details"
            rRow.Cell("CigarettsConsumptionDetail").Value = "Cigaretts Consumption Detail"
            rRow.Cell("CigarettsStoppedSince").Value = "Cigaretts Stopped Since"
            rRow.Cell("BidiHabitDetails").Value = "Bidi Habit Details"
            rRow.Cell("BidiConsumptionDetail").Value = "Bidi Consumption Detail"
            rRow.Cell("BidiStoppedSince").Value = "Bidi Stopped Since"
            rRow.Cell("GutkhaHabitDetails").Value = "Gutkha Habit Details"
            rRow.Cell("GutkhaConsumptionDetail").Value = "Gutkha Consumption Detail"
            rRow.Cell("GutkhaStoppedSince").Value = "Gutkha Stopped Since"
            rRow.Cell("SupariTobacoHabitDetails").Value = "SupariTobaco Habit Details"
            rRow.Cell("SupariTobacoConsumptionDetail").Value = "SupariTobaco Consumption Detail"
            rRow.Cell("SupariTobacoStoppedSince").Value = "SupariTobaco Stopped Since"
            rRow.Cell("TobacowithSupariHabitDetails").Value = "TobacowithSupari Habit Details"
            rRow.Cell("TobacowithSupariConsumptionDetail").Value = "TobacowithSupari Consumption Detail"
            rRow.Cell("TobacowithSupariStoppedSince").Value = "TobacowithSupari Stopped Since"
            'rRow.Cell("TobacowithSupariHabitDetails").Value = "TobacowithSupari Habit Details"
            rRow.Cell("AlchoholHabitDetails").Value = "Alchohol Habit Details"
            rRow.Cell("AlchoholConsumptionDetail").Value = "Alchohol Consumption Detail"
            rRow.Cell("AlchoholStoppedSince").Value = "Alchohol Stopped Since"
            rRow.Cell("OthersHabitDetails").Value = "Others Habit Details"
            rRow.Cell("OthersConsumptionDetail").Value = "Others Consumption Detail"
            rRow.Cell("OthersStoppedSince").Value = "Others Stopped Since"

            'rRow.Cell("QCFlag").Value = "QA Flag"

            For index = 0 To rRow.CellCount - 1
                rRow.Cell(index).FontBold = True
                rRow.Cell(index).Alignment = RepoCell.AlignmentEnum.CenterTop
            Next

            rPage.Say(rRow)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Private Sub ReportDetail()
        Dim RowCnt As Integer
        Dim rRow As RepoRow
        Dim dt_Report As New DataTable
        Dim PreviousCompany As String = ""
        Dim PreviousProject As String = ""
        Dim dt_supressReport As New DataTable
        Try

            rRow = masterRow()
            PrintHeader()

            If Not GetData(dt_Report) Then
                Exit Sub
            End If


            

            RowCnt = 0
            ' dr_Row1 = dt_Report.Rows(0)
            Do While RowCnt <= dt_Report.Rows.Count - 1

                rRow.Cell("SubjectId").Value = dt_Report.Rows(RowCnt)("vSubjectId").ToString()
                rRow.Cell("ModifiedBy").Value = dt_Report.Rows(RowCnt)("vModifyBy").ToString
                rRow.Cell("ModifiedOn").Value = dt_Report.Rows(RowCnt)("dModifyOn").ToString
                rRow.Cell("Remark").Value = dt_Report.Rows(RowCnt)("vRemark").ToString()
                rRow.Cell("EnrollmentDate").Value = dt_Report.Rows(RowCnt)("dEnrollmentDate").ToString
                rRow.Cell("MiddleName").Value = dt_Report.Rows(RowCnt)("vMiddleName").ToString()
                rRow.Cell("SurName").Value = dt_Report.Rows(RowCnt)("vSurName").ToString()
                rRow.Cell("FirstName").Value = dt_Report.Rows(RowCnt)("vFirstName").ToString()
                rRow.Cell("Initials").Value = dt_Report.Rows(RowCnt)("vInitials").ToString()
                rRow.Cell("BirthDate").Value = dt_Report.Rows(RowCnt)("dBirthDate").ToString()
                rRow.Cell("Sex").Value = dt_Report.Rows(RowCnt)("cSex").ToString()
                rRow.Cell("EducationQualification").Value = dt_Report.Rows(RowCnt)("vEducationQualification").ToString()
                rRow.Cell("ICF_LAnguages").Value = GetLanguagesByCode(dt_Report.Rows(RowCnt)("vICFLanguageCodeId").ToString())
                rRow.Cell("Occupation").Value = dt_Report.Rows(RowCnt)("vOccupation").ToString()
                rRow.Cell("BloodGroup").Value = dt_Report.Rows(RowCnt)("cBloodGroup").ToString()
                rRow.Cell("Rh").Value = dt_Report.Rows(RowCnt)("cRh").ToString()
                rRow.Cell("ProofOfAge1").Value = dt_Report.Rows(RowCnt)("vProofOfAge1").ToString()
                rRow.Cell("ProofOfAge2").Value = dt_Report.Rows(RowCnt)("vProofOfAge2").ToString()
                rRow.Cell("ProofOfAge3").Value = dt_Report.Rows(RowCnt)("vProofOfAge3").ToString()
                rRow.Cell("PerAdd1").Value = dt_Report.Rows(RowCnt)("vPerAdd1").ToString()
                rRow.Cell("PerAdd2").Value = dt_Report.Rows(RowCnt)("vPerAdd2").ToString()
                rRow.Cell("PerAdd3").Value = dt_Report.Rows(RowCnt)("vPerAdd3").ToString()
                rRow.Cell("PerTelephoneNo").Value = dt_Report.Rows(RowCnt)("vPerTelephoneNo").ToString()
                rRow.Cell("PerCity").Value = dt_Report.Rows(RowCnt)("vPerCity").ToString()
                rRow.Cell("PinCity").Value = dt_Report.Rows(RowCnt)("vPerPinCode").ToString()
                rRow.Cell("PerCountry").Value = dt_Report.Rows(RowCnt)("vPerCountry").ToString()
                rRow.Cell("LocalAdd11").Value = dt_Report.Rows(RowCnt)("vLocalAdd1").ToString()
                rRow.Cell("LocalAdd12").Value = dt_Report.Rows(RowCnt)("vLocalAdd12").ToString()
                rRow.Cell("LocalAdd13").Value = dt_Report.Rows(RowCnt)("vLocalAdd13").ToString()
                rRow.Cell("LocalTelephoneno1").Value = dt_Report.Rows(RowCnt)("vLocalTelephoneno1").ToString()
                rRow.Cell("LocalCity1").Value = dt_Report.Rows(RowCnt)("vLocalCity1").ToString()
                rRow.Cell("LocalCountry1").Value = dt_Report.Rows(RowCnt)("vLocalCountry1").ToString()
                rRow.Cell("LocalAdd21").Value = dt_Report.Rows(RowCnt)("vLocalAdd21").ToString()
                rRow.Cell("LocalAdd22").Value = dt_Report.Rows(RowCnt)("vLocalAdd22").ToString()
                rRow.Cell("LocalAdd23").Value = dt_Report.Rows(RowCnt)("vLocalAdd23").ToString()
                rRow.Cell("LocalTelephoneno2").Value = dt_Report.Rows(RowCnt)("vLocalTelephoneno2").ToString()
                rRow.Cell("LocalCity2").Value = dt_Report.Rows(RowCnt)("vLocalCity2").ToString()
                rRow.Cell("LocalCountry2").Value = dt_Report.Rows(RowCnt)("vLocalCountry2").ToString()
                rRow.Cell("ContactName1").Value = dt_Report.Rows(RowCnt)("vContactName1").ToString()
                rRow.Cell("ContactAddress11").Value = dt_Report.Rows(RowCnt)("vContactAddress11").ToString()
                rRow.Cell("ContactAddress12").Value = dt_Report.Rows(RowCnt)("vContactAddress12").ToString()
                rRow.Cell("ContactAddress13").Value = dt_Report.Rows(RowCnt)("vContactAddress13").ToString()
                rRow.Cell("ContactTelephoneNo1").Value = dt_Report.Rows(RowCnt)("vContactTelephoneNo1").ToString()
                rRow.Cell("ContactCity1").Value = dt_Report.Rows(RowCnt)("vContactCity1").ToString()
                rRow.Cell("ContactCountry1").Value = dt_Report.Rows(RowCnt)("vContactCountry1").ToString()
                rRow.Cell("ContactName2").Value = dt_Report.Rows(RowCnt)("vContactName2").ToString()
                rRow.Cell("ContactAddress21").Value = dt_Report.Rows(RowCnt)("vContactAddress21").ToString()
                rRow.Cell("ContactAddress22").Value = dt_Report.Rows(RowCnt)("vContactAddress22").ToString()
                rRow.Cell("ContactAddress23").Value = dt_Report.Rows(RowCnt)("vContactAddress23").ToString()
                rRow.Cell("ContactTelephoneNo2").Value = dt_Report.Rows(RowCnt)("vContactTelephoneNo2").ToString()
                rRow.Cell("ContactCity2").Value = dt_Report.Rows(RowCnt)("vContactCity2").ToString()
                rRow.Cell("OfficeAddress").Value = dt_Report.Rows(RowCnt)("vOfficeAddress").ToString()
                rRow.Cell("OfficeTelephoneno").Value = dt_Report.Rows(RowCnt)("vOfficeTelephoneno").ToString()
                rRow.Cell("RefferedBy").Value = dt_Report.Rows(RowCnt)("vReferredBy").ToString()
                rRow.Cell("FoodHabit").Value = dt_Report.Rows(RowCnt)("cFoodHabit").ToString()
                rRow.Cell("Height").Value = dt_Report.Rows(RowCnt)("nHeight").ToString()
                rRow.Cell("Weight").Value = dt_Report.Rows(RowCnt)("nWeight").ToString()
                rRow.Cell("BMI").Value = dt_Report.Rows(RowCnt)("nBMI").ToString()
                rRow.Cell("LastMenstrualDate").Value = dt_Report.Rows(RowCnt)("dLastMenstrualDate").ToString()
                rRow.Cell("LastMenstrualDays").Value = dt_Report.Rows(RowCnt)("iLastMenstrualDays").ToString()
                rRow.Cell("LastDeliveryDate").Value = dt_Report.Rows(RowCnt)("dLastDelivaryDate").ToString()
                rRow.Cell("NoOfChildren").Value = dt_Report.Rows(RowCnt)("iNoOfChildren").ToString()
                rRow.Cell("NoOfChildrenDied").Value = dt_Report.Rows(RowCnt)("iNoOfChildrenDied").ToString()
                rRow.Cell("RemarksIfDied").Value = dt_Report.Rows(RowCnt)("vRemarkifDied").ToString()
                rRow.Cell("ChildrenHealth").Value = dt_Report.Rows(RowCnt)("cChildrenHealth").ToString()
                rRow.Cell("ChildrenHealthRemark").Value = dt_Report.Rows(RowCnt)("vChildrenHealthRemark").ToString()
                rRow.Cell("Para").Value = dt_Report.Rows(RowCnt)("vPara").ToString()
                rRow.Cell("Gravida").Value = dt_Report.Rows(RowCnt)("vGravida").ToString()
                rRow.Cell("Abortion").Value = dt_Report.Rows(RowCnt)("cAbortions").ToString()
                rRow.Cell("AbortionDate").Value = dt_Report.Rows(RowCnt)("dAbortionDate").ToString()
                rRow.Cell("Regular").Value = dt_Report.Rows(RowCnt)("cRegular").ToString()
                rRow.Cell("Loctating").Value = dt_Report.Rows(RowCnt)("cLoctating").ToString()
                rRow.Cell("VoluteerinBearingAge").Value = dt_Report.Rows(RowCnt)("cIsVolunteerinBearingAge").ToString()
                rRow.Cell("FamilyPlannigSelf").Value = dt_Report.Rows(RowCnt)("cFamilyPlanningSelf").ToString()
                rRow.Cell("Contraception").Value = dt_Report.Rows(RowCnt)("cContraception").ToString()
                rRow.Cell("Barrier").Value = dt_Report.Rows(RowCnt)("cBarrier").ToString()
                rRow.Cell("Pills").Value = dt_Report.Rows(RowCnt)("cPills").ToString()
                rRow.Cell("Rhythm").Value = dt_Report.Rows(RowCnt)("cRhythm").ToString()
                rRow.Cell("IUCD").Value = dt_Report.Rows(RowCnt)("cIUCD").ToString()
                rRow.Cell("OtherRemark").Value = dt_Report.Rows(RowCnt)("vOtherRemark").ToString()
                rRow.Cell("FemaleRemark").Value = dt_Report.Rows(RowCnt)("vFemaleRemark").ToString()
                rRow.Cell("CigarettsHabitDetails").Value = dt_Report.Rows(RowCnt)("Cigaretts_vHabitDetails").ToString()
                rRow.Cell("CigarettsConsumptionDetail").Value = dt_Report.Rows(RowCnt)("Cigaretts_ConsumptionDetail").ToString()
                rRow.Cell("CigarettsStoppedSince").Value = dt_Report.Rows(RowCnt)("Cigaretts_StoppedSince").ToString()
                rRow.Cell("BidiHabitDetails").Value = dt_Report.Rows(RowCnt)("Bidi_vHabitDetails").ToString()
                rRow.Cell("BidiConsumptionDetail").Value = dt_Report.Rows(RowCnt)("Bidi_ConsumptionDetail").ToString()
                rRow.Cell("BidiStoppedSince").Value = dt_Report.Rows(RowCnt)("Bidi_StoppedSince").ToString()
                rRow.Cell("GutkhaHabitDetails").Value = dt_Report.Rows(RowCnt)("Gutkha_vHabitDetails").ToString()
                rRow.Cell("GutkhaConsumptionDetail").Value = dt_Report.Rows(RowCnt)("Gutkha_ConsumptionDetail").ToString()
                rRow.Cell("GutkhaStoppedSince").Value = dt_Report.Rows(RowCnt)("Gutkha_StoppedSince").ToString()
                rRow.Cell("SupariTobacoHabitDetails").Value = dt_Report.Rows(RowCnt)("Supari_Tobaco_vHabitDetails").ToString()
                rRow.Cell("SupariTobacoConsumptionDetail").Value = dt_Report.Rows(RowCnt)("Supari_Tobaco_ConsumptionDetail").ToString()
                rRow.Cell("SupariTobacoStoppedSince").Value = dt_Report.Rows(RowCnt)("Supari_Tobaco_StoppedSince").ToString()
                rRow.Cell("TobacowithSupariHabitDetails").Value = dt_Report.Rows(RowCnt)("Tobaco_with_Supari_vHabitDetails").ToString()
                rRow.Cell("TobacowithSupariConsumptionDetail").Value = dt_Report.Rows(RowCnt)("Tobaco_with_Supari_ConsumptionDetail").ToString()
                rRow.Cell("TobacowithSupariStoppedSince").Value = dt_Report.Rows(RowCnt)("Tobaco_with_Supari_StoppedSince").ToString()
                rRow.Cell("AlchoholHabitDetails").Value = dt_Report.Rows(RowCnt)("Alchohol_vHabitDetails").ToString()
                rRow.Cell("AlchoholConsumptionDetail").Value = dt_Report.Rows(RowCnt)("Alchohol_ConsumptionDetail").ToString()
                rRow.Cell("AlchoholStoppedSince").Value = dt_Report.Rows(RowCnt)("Alchohol_StoppedSince").ToString()
                rRow.Cell("OthersHabitDetails").Value = dt_Report.Rows(RowCnt)("Others_vHabitDetails").ToString()
                rRow.Cell("OthersConsumptionDetail").Value = dt_Report.Rows(RowCnt)("Others_ConsumptionDetail").ToString()
                rRow.Cell("OthersStoppedSince").Value = dt_Report.Rows(RowCnt)("Others_StoppedSince").ToString()
                rPage.Say(rRow)

                RowCnt = RowCnt + 1

            Loop ''detail loop ending


        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message + "Error While Print Detail", "")
        End Try
    End Sub


#End Region

#Region " GetData "
    Private Function GetData(ByRef dt_Report_Retu As DataTable) As Boolean

        Dim wStr As String = ""
        Dim eStr_Retu As String = ""
        Dim Val As String = ""
        Dim ds As DataSet = Nothing
        Dim Ds_QADetail As New DataSet
        Dim Dv_QADetail As New DataView
        Dim FromDate As String = ""
        Dim ToDate As String = ""
        ' Dim dr As DataRow
        Dim Dt_Subject As New DataTable

        Dim Subjects As String = ""

        Try
            If Me.txtFromDate.Text.Trim() <> "" Then
                FromDate = DateTime.Parse(Me.txtFromDate.Text).ToString("dd-MMM-yyyy")
            End If

            If Me.txtToDate.Text.Trim() <> "" Then
                ToDate = DateTime.Parse(Me.txtToDate.Text).ToString("dd-MMM-yyyy")
            End If

            wStr = "vSubjectId in ('" & Me.HSubjectId.Value.Trim() & "')"

            If Me.chkAll.Checked = False Then

                wStr += " And cast(convert(varchar(11),dModifyOn,113)as smalldatetime)>=cast(convert(varchar(11),cast('" & FromDate & _
                        "' as datetime),113)as smalldatetime) And cast(convert(varchar(11),dModifyOn,113)as smalldatetime)<= cast(convert(varchar(11),cast('" & ToDate & "' as datetime),113)as smalldatetime)"
            End If

            If Not objHelp.View_SubjectPIFAuditTrail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    Ds_QADetail, eStr_Retu) Then

                ShowErrorMessage(eStr_Retu, "")
                Exit Function

            End If

            'End If

            If Ds_QADetail.Tables(0).Rows.Count > 0 Then
                Dv_QADetail = Ds_QADetail.Tables(0).DefaultView
                Dv_QADetail.Sort = "vSubjectID,iTranNo"
                dt_Report_Retu = Dv_QADetail.ToTable()
            Else
                ObjCommon.ShowAlert("No Records Found.", Me)
            End If

            GetData = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")

        Finally

        End Try
    End Function
#End Region

#Region "Fill Function"

    Private Function fillvalues() As Boolean
        Try

            FillPeriodDropDown()
            FillSubjectDropDown()

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub FillPeriodDropDown()
        Dim dsPeriod As New DataSet
        Dim WorkspaceId As String = ""
        WorkspaceId = Me.HProjectId.Value.Trim()
        If WorkspaceId.Trim() <> "" Then

            If Not objHelp.GetViewWorkSpaceNodeDetail("vWorkSpaceId='" & WorkspaceId & "'", _
                                dsPeriod, eStr_Retu) Then

                ShowErrorMessage(eStr_Retu, "")
                Exit Sub
            End If

            If dsPeriod.Tables(0).Rows.Count > 0 Then

                Me.ddlPeriod.DataSource = dsPeriod.Tables(0).DefaultView.ToTable("WorkSpaceNodeDetail", True, "iPeriod")
                Me.ddlPeriod.DataTextField = "iPeriod"
                Me.ddlPeriod.DataValueField = "iPeriod"
                Me.ddlPeriod.DataBind()
                Me.ddlPeriod.Items.Insert(0, New ListItem("Select Period", "0"))

                If Not IsNothing(Me.Request.QueryString("PeriodId")) Then
                    Me.ddlPeriod.SelectedValue = Me.Request.QueryString("PeriodId").Trim()
                    Me.ddlPeriod.Enabled = False
                End If
            End If

        End If
    End Sub

    Private Sub FillSubjectDropDown()
        Dim dsSubject As New DataSet
        Dim ds_WSSubject As New DataSet
        Dim dt_Period As New DataTable
        Dim dv_Period As New DataView
        Dim estr As String = ""
        Dim Wstr As String = ""
        Dim WorkspaceId As String = ""
        Dim iperiod As Integer

        WorkspaceId = Me.HProjectId.Value.Trim()
        If Me.ddlPeriod.Items.Count > 0 Then
            iperiod = Me.ddlPeriod.SelectedValue.Trim()
        End If
        ''Wstr = "vLocationcode='" & Me.Session(S_LocationCode).ToString.Trim() & "'"
        ''Wstr += " AND cRejectionFlag <> 'Y'"
        'Wstr = "vLocationcode Not In(" + System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge") + ") And cRejectionFlag <> 'Y'"
        'Me.AutoCompleteExtender2.ContextKey = "View_SubjectMaster" + "#" + Wstr.Trim()

        'Vineet'
        If System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge").Contains(Session(S_LocationCode)) = True Then
            Wstr = "vLocationcode='" & Me.Session(S_LocationCode).ToString.Trim() & "'"
            Wstr += " AND cRejectionFlag <> 'Y'"
            Me.AutoCompleteExtender2.ContextKey = "View_SubjectMaster" + "#" + Wstr.Trim()
        Else
            Wstr = "vLocationcode Not In(" + System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge") + ") And cRejectionFlag <> 'Y'"
            Me.AutoCompleteExtender2.ContextKey = "View_SubjectMaster" + "#" + Wstr.Trim()
        End If

        If WorkspaceId.Trim() <> "" Then

            Wstr = "vWorkspaceId='" & WorkspaceId.Trim() & "' and cRejectionFlag <> 'Y' order by vWorkspaceSubjectId Desc"

            If Not Me.objHelp.GetWorkspaceSubjectMaster(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                    ds_WSSubject, estr) Then
                Exit Sub
            End If

            If Me.ddlPeriod.SelectedIndex = 0 Then
                If ds_WSSubject.Tables(0).Rows.Count > 0 Then
                    dt_Period = ds_WSSubject.Tables(0).DefaultView.ToTable(True, "iPeriod")
                    dv_Period = dt_Period.DefaultView
                    dv_Period.Sort = "iPeriod"
                    iperiod = dv_Period.ToTable.Rows(dt_Period.Rows.Count - 1).Item("iPeriod")
                    dv_Period = Nothing

                    Me.ddlPeriod.SelectedValue = iperiod
                End If

            End If

            If ds_WSSubject.Tables(0).Rows.Count > 0 AndAlso iperiod > 0 Then 'ds_WSSubject.Tables(0).Rows(0).Item("iPeriod") 

                Wstr = ""
                Me.AutoCompleteExtender2.ContextKey = ""

                Wstr = "vWorkspaceId='" & WorkspaceId.Trim() & "' and cRejectionFlag <> 'Y' and iPeriod=" & iperiod

                Me.AutoCompleteExtender2.ContextKey = "view_WorkspaceSubjectMst" + "#" + Wstr.Trim()

            End If

        End If

    End Sub

#End Region

#Region "Buttons Event"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        fillvalues()
    End Sub

    Protected Sub btnSubject_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim wstr As String = ""
        Dim ds_SubjectBlob As New DataSet
        Dim estr As String = ""

        Me.Image1.Visible = True
        '==ADDED on 13-jan-2010 by deepak singh 
        wstr = "cRecordType='P' and iTranNo=(select Max(itranNO) from SubjectBlobDetails where cRecordType='P' and vSubjectId='" & HSubjectId.Value.ToString.Trim & "') and vSubjectId='" & HSubjectId.Value.ToString.Trim & "'"

        If Not Me.objHelp.getSubjectBlobDetails(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubjectBlob, estr) Then
            MsgBox("Error while getting Data." + vbCrLf + estr)
            Exit Sub
        End If

        If ds_SubjectBlob.Tables(0).Rows.Count > 0 Then
            Me.Image1.ImageUrl = "frmPIFImage.aspx?subjectid=" + Me.HSubjectId.Value.Trim()
        ElseIf ds_SubjectBlob.Tables(0).Rows.Count <= 0 Then
            Me.Image1.ImageUrl = "~/Images/demo.jpg"
        End If
        '==========







    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("frmRptSubjectPIFAuditTrail.aspx")
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

    Private Function GetLanguagesByCode(ByVal ICFLangCodes As String) As String
        Dim StrLanguages As String = ""
        dsLanguages = CType(ViewState("SubjectLanguages"), DataSet)
        'Dim dvLanguage As New DataView
       
        'For Each dr As DataRow In dsLanguages.Tables(0).Rows
        For Each Str As String In ICFLangCodes.Split(",")
            If Str.Trim <> "" Then
                StrLanguages += dsLanguages.Tables(0).Select("vLanguageId='" & Str & "'")(0)("vLanguageName") + ","
            End If
        Next
        ' Next

        If StrLanguages.Contains(",") Then
            StrLanguages = StrLanguages.Remove(StrLanguages.Length - 1)
        End If

        Return StrLanguages
    End Function
  
End Class
