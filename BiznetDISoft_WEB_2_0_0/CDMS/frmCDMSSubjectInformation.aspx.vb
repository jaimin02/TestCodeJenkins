Imports Newtonsoft.Json

Partial Class CDMS_frmCDMSSubjectInformation
    Inherits System.Web.UI.Page

#Region "Variable Declaration "

    Private objCommon As New clsCommon
    Private objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

    Public Const VS_Choice As String = "Choice"
    Private Const VS_DtSubjectMst As String = "DtSubjectMst"
    Private Const VS_DtSubjectDtl As String = "DtSubjectDtl"
    Private Const VS_DtCDMSDtlHistory As String = "DtCDMSDtlHistory"
    Private Const VS_DtConsumptionList As String = "DtConsumptionList"
    Private Const VS_DtConsumption As String = "DtConsumption"
    Public Const VS_SubjectID As String = "SubjectID"

    Private Const GVCConsu_CDMSConsumptionNo As Integer = 0
    Private Const GVCConsu_Type As Integer = 1
    'Private Const GVCConsu_Code As Integer = 1
    Private Const GVCConsu_Status As Integer = 2
    Private Const GVCConsu_Quantity As Integer = 3
    Private Const GVCConsu_Description As Integer = 4
    Private Const GVCConsu_Frequency As Integer = 5
    Private Const GVCConsu_StartDate As Integer = 6
    Private Const GVCConsu_EndDate As Integer = 7

    Private Const GVCConsuAudit_Code As Integer = 6

#End Region

#Region "Page Load"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            If Not IsPostBack Then
                Me.AutoCompleteExtender1.ContextKey = Session(S_LocationCode)
                If Not GenCall() Then
                    Throw New Exception("Error While calling GenCall()")
                End If
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
        End Try

    End Sub

#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim dt_SubjectMaster As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try



            Choice = Me.Request.QueryString("Mode").ToString
            Me.ViewState(VS_Choice) = Choice

            If Not Me.Request.QueryString("SubjectID") Is Nothing Then
                Me.ViewState(VS_SubjectID) = Me.Request.QueryString("SubjectID").ToString
                Me.HSubjectId.Value = Me.Request.QueryString("SubjectID").ToString
            End If

            If Not GenCall_Data(Choice, dt_SubjectMaster) Then
                Exit Function
            End If

            Me.ViewState(VS_DtSubjectMst) = dt_SubjectMaster

            If Not GenCall_ShowUI(Choice, dt_SubjectMaster) Then
                Exit Function
            End If

            GenCall = True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "..GenCall")
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_Data"

    Private Function GenCall_Data(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                            ByRef dt_Dist_Retu As DataTable) As Boolean

        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_SubjectMst As DataSet = Nothing
        Dim ds_CDMSSubjectDtl As DataSet = Nothing
        Dim ds_CDMSSubjectDtlHistory As DataSet = Nothing
        Dim ds_CDMSSubjectConsumption As DataSet = Nothing

        Try


            If Choice_1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vSubjectId='" + Me.Request.QueryString("SubjectId").ToString.Trim() + "'"
            End If

            wStr += " And cStatusIndi <> 'D'"

            'Subject Master
            If Not objhelpDb.GetView_CDMSSubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubjectMst, eStr) Then
                Throw New Exception(eStr)
            End If

            'SubjectDtlCDMS
            If Not objhelpDb.getSubjectDtlCDMS(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CDMSSubjectDtl, eStr) Then
                Throw New Exception(eStr)
            End If
            Me.ViewState(VS_DtSubjectDtl) = ds_CDMSSubjectDtl.Tables(0)

            'SubjectDtlCDMSHistory
            If Not objhelpDb.getSubjectDtlCDMSHistory("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CDMSSubjectDtlHistory, eStr) Then
                Throw New Exception(eStr)
            End If
            Me.ViewState(VS_DtCDMSDtlHistory) = ds_CDMSSubjectDtlHistory.Tables(0)

            'SubjectDtlCDMSConsumption
            If Not objhelpDb.View_SubjectDtlCDMSConsumption(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CDMSSubjectConsumption, eStr) Then
                Throw New Exception(eStr)
            End If
            Me.ViewState(VS_DtConsumption) = ds_CDMSSubjectConsumption.Tables(0)


            If ds_SubjectMst Is Nothing Then
                Throw New Exception(eStr)
            End If

            'If ds_SubjectMst.Tables(0).Rows.Count <= 0 And _
            '   Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

            '    Throw New Exception("No Records Found...")

            'End If

            dt_Dist_Retu = ds_SubjectMst.Tables(0)
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_Data")
        Finally
        End Try
    End Function

#End Region

#Region "GenCall_ShowUI"

    Private Function GenCall_ShowUI(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                                      ByVal dt_ClientMst As DataTable) As Boolean

        Dim dsImage As New DataSet
        Dim eStr As String = String.Empty
        Dim wStrImage As String = String.Empty


        Try

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Subject Information"
            Me.Page.Title = " :: CDMS - Subject Information ::" + System.Configuration.ConfigurationManager.AppSettings("Client")

            Me.txtEnrollmentDate.Text = DateTime.Parse(DateTime.Now()).ToString("dd-MMM-yyyy")

            If Choice_1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                Me.btnSaveSubject.Visible = False
                If Not AssingAttribute() Then
                    Return False
                End If

                wStrImage = "cRecordType='P' and iTranNo=(select Max(itranNO) from SubjectBlobDetails where cRecordType='P' and vSubjectId='" & HSubjectId.Value.ToString.Trim & "') and vSubjectId='" & Me.ViewState(VS_SubjectID).ToString.Trim & "'"

                If Not Me.objhelpDb.getSubjectBlobDetails(wStrImage, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsImage, eStr) Then
                    Return False
                End If

                If Not dsImage Is Nothing Then
                    If dsImage.Tables(0).Rows.Count > 0 Then

                        'Me.imgSubjectPhoto.ImageUrl = "~/frmPIFImage.aspx?subjectid=AH11-01131"
                        Me.imgSubjectPhoto.ImageUrl = "~/frmPIFImage.aspx?subjectid=" + Me.ViewState(VS_SubjectID).ToString
                        Me.lblSubjectPhoto.Text = CDate(dsImage.Tables(0).Rows(0).Item("dModifyOn").ToString()).ToString("dd-MMM-yyyy")
                    Else
                        Me.imgSubjectPhoto.ImageUrl = "~/CDMS/images/NotFound.gif"
                        Me.lblSubjectPhoto.Text = ""
                    End If
                Else
                    Me.imgSubjectPhoto.ImageUrl = "~/CDMS/images/NotFound.gif"
                    Me.lblSubjectPhoto.Text = ""
                End If
            End If

            If Not FillLanguage() Then
                Return False
            End If

            If Not FillConsumptionGrid() Then
                Return False
            End If
            If Not FillddlRecruitingSource() Then
                Return False
            End If
            If Not AssignValuesToControl() Then
                Return False
            End If

            If Me.Session(S_WorkFlowStageId) <> 0 Then
                Me.btnNewEntry.Visible = False
                Me.btnSaveSubject.Visible = False
                Me.fieldNew.Visible = False
            End If

            txtEnrollmentDate.Attributes.Add("OnChange", "DateConvertForScreening(this.value,this,0)")
            txtWashoutDate.Attributes.Add("OnChange", "DateConvertForScreening(this.value,this,0)")
            txtStatusStartDate.Attributes.Add("OnChange", "DateConvertForScreening(this.value,this,1)")
            txtStatusEndDate.Attributes.Add("OnChange", "DateConvertForScreening(this.value,this,2)")
            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_ShowUI")
        Finally
        End Try

    End Function

#End Region

#Region "Fill Controls"

    Private Function FillLanguage() As Boolean
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_Language As New DataSet


        Try

            wStr = " cActiveFlag <> 'N' and cStatusIndi <> 'D'"

            If Not objhelpDb.getSubjectLanguageMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_Language, eStr) Then
                Me.objCommon.ShowAlert("Error While Getting SubjectLanguageMst", Me.Page)
                Exit Function
            End If

            If Not ds_Language Is Nothing Then
                If ds_Language.Tables(0).Rows.Count > 0 Then
                    Me.ddlLanguage.DataSource = ds_Language.Tables(0)
                    Me.ddlLanguage.DataTextField = "vLanguageName"
                    Me.ddlLanguage.DataValueField = "vLanguageId"
                    Me.ddlLanguage.DataBind()
                    Me.ddlLanguage.Items.Insert(0, "Select Language")
                End If
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillLanguage")
            Return False
        End Try

    End Function

    Private Function FillConsumptionGrid() As Boolean
        Dim ds_Consumption As New DataSet
        Dim eStr As String = String.Empty

        Try

            If Not objhelpDb.getCDMSConsumption("cStatusIndi <> 'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Consumption, eStr) Then
                Throw New Exception(eStr)
            End If

            Me.ViewState(VS_DtConsumptionList) = ds_Consumption.Tables(0)

            Me.grdGeneralConmp.DataSource = ds_Consumption.Tables(0)
            Me.grdGeneralConmp.DataBind()

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillConsumptionGrid()")
            Return False
        End Try



    End Function

    Private Function FillddlRecruitingSource() As Boolean
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_RecruitingSource As New DataSet


        Try

            wStr = " SELECT vSource,vReferences FROM CodeRecruitingSource WHERE cStatusIndi = 'A'"
            ds_RecruitingSource = objhelpDb.GetResultSet(wStr, "CodeRecruitingSource")


            If Not ds_RecruitingSource Is Nothing Then
                If ds_RecruitingSource.Tables(0).Rows.Count > 0 Then
                    Me.ddlRecruitingSource.DataSource = ds_RecruitingSource.Tables(0)
                    Me.ddlRecruitingSource.DataTextField = "vSource"
                    Me.ddlRecruitingSource.DataValueField = "vSource"
                    Me.ddlRecruitingSource.DataBind()
                    Me.ddlRecruitingSource.Items.Insert(0, New ListItem("Select Recruiting Source", 0))
                End If
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillLanguage")
            Return False
        End Try

    End Function

#End Region

#Region "Assign Values"

    Private Function AssignValues() As Boolean

        Dim dt_SubjectMaster As New DataTable
        Dim dt_SubjectCDMSDtl As New DataTable
        Dim dt_CDMSDtlHistory As New DataTable
        Dim dt_CDMSDtlConsu As New DataTable
        Dim drSubjectMaster As DataRow
        Dim drSubjectCDMSDtl As DataRow
        Dim drCDMSDtlHistory As DataRow
        Dim drCDMSDtlConsu As DataRow

        Try
            'Assign Values for SubjectMaster
            dt_SubjectMaster = Me.ViewState(VS_DtSubjectMst)
            dt_SubjectMaster.Clear()
            drSubjectMaster = dt_SubjectMaster.NewRow()

            drSubjectMaster("vLocationCode") = Me.Session(S_LocationCode)
            drSubjectMaster("vSubjectID") = Pro_Screening
            drSubjectMaster("nSubjectDetailNo") = 1
            drSubjectMaster("dEnrollmentDate") = IIf(String.IsNullOrEmpty(Me.txtEnrollmentDate.Text.Trim), System.DBNull.Value, Me.txtEnrollmentDate.Text.Trim)
            drSubjectMaster("vFirstName") = Me.txtFirstName.Text.Trim
            drSubjectMaster("vSurName") = Me.txtLastName.Text.Trim
            drSubjectMaster("vMiddleName") = Me.txtMiddleName.Text.Trim
            drSubjectMaster("cSex") = Me.ddlSex.SelectedItem.Value.Trim()
            drSubjectMaster("iModifyBy") = Me.Session(S_UserID)

            dt_SubjectMaster.Rows.Add(drSubjectMaster)
            dt_SubjectMaster.TableName = "View_SubjectMaster"
            Me.ViewState(VS_DtSubjectMst) = dt_SubjectMaster

            'Assign Values for SubjecCDMSDtl
            dt_SubjectCDMSDtl = Me.ViewState(VS_DtSubjectDtl)
            dt_SubjectCDMSDtl.Clear()
            drSubjectCDMSDtl = dt_SubjectCDMSDtl.NewRow()

            drSubjectCDMSDtl("vSubjectID") = Pro_Screening
            drSubjectCDMSDtl("vInitials") = Me.hdnInitials.Value.Trim()
            'drSubjectCDMSDtl("vInitials") = Me.txtInitials.Text.Trim()
            drSubjectCDMSDtl("vAddress") = IIf(String.IsNullOrEmpty(Me.txtAddress.Text.Trim()), System.DBNull.Value, Me.txtAddress.Text.Trim())

            drSubjectCDMSDtl("vEmailAddress") = IIf(String.IsNullOrEmpty(Me.txtEmail.Text.Trim()), System.DBNull.Value, Me.txtEmail.Text.Trim())
            drSubjectCDMSDtl("vContactComments") = IIf(String.IsNullOrEmpty(Me.txtContactComments.Text.Trim()), System.DBNull.Value, Me.txtContactComments.Text.Trim())
            drSubjectCDMSDtl("vPlace") = IIf(String.IsNullOrEmpty(Me.txtPlace.Text.Trim()), System.DBNull.Value, Me.txtPlace.Text.Trim())
            drSubjectCDMSDtl("dBirthdate") = IIf(String.IsNullOrEmpty(Me.txtBirthdate.Text.Trim()), System.DBNull.Value, Me.txtBirthdate.Text.Trim())
            drSubjectCDMSDtl("cSex") = Me.ddlSex.SelectedItem.Value.Trim()
            drSubjectCDMSDtl("vLanguage") = Me.ddlLanguage.SelectedItem.Value.Trim()
            If Me.ddlSex.SelectedItem.Value.Trim() = "F" Then
                drSubjectCDMSDtl("iMenstrualCycle") = IIf(String.IsNullOrEmpty(Me.txtMenstrualCycle.Text.Trim()), System.DBNull.Value, Me.txtMenstrualCycle.Text.Trim())
                drSubjectCDMSDtl("cRegular") = Me.ddlRegular.SelectedItem.Value.Trim()
            End If
            drSubjectCDMSDtl("vRace") = Me.ddlRace.SelectedItem.Value.Trim()
            drSubjectCDMSDtl("vAvailiability") = Me.ddlAvailability.SelectedItem.Value.Trim()
            drSubjectCDMSDtl("vTransportation") = Me.ddlTransportation.SelectedItem.Value.Trim()
            drSubjectCDMSDtl("cRegularDiet") = Me.ddlRegularDiet.SelectedItem.Value.Trim()
            drSubjectCDMSDtl("cSwallowPil") = Me.ddlSwallowPill.SelectedItem.Value.Trim()
            drSubjectCDMSDtl("cContactForFutureStudies") = Me.ddlContactFuture.SelectedItem.Value.Trim()
            drSubjectCDMSDtl("nHeight") = IIf(String.IsNullOrEmpty(Me.txtHeight.Text.Trim()), System.DBNull.Value, Me.txtHeight.Text.Trim())
            drSubjectCDMSDtl("nWeight") = IIf(String.IsNullOrEmpty(Me.txtWeight.Text.Trim()), System.DBNull.Value, Me.txtWeight.Text.Trim())
            'drSubjectCDMSDtl("nBMI") = IIf(String.IsNullOrEmpty(Me.txtBMI.Text.Trim()), System.DBNull.Value, Me.txtBMI.Text.Trim())
            drSubjectCDMSDtl("nBMI") = Me.hdnBMI.Value.Trim()
            drSubjectCDMSDtl("vComments") = IIf(String.IsNullOrEmpty(Me.txtComments.Text.Trim()), System.DBNull.Value, Me.txtComments.Text.Trim())
            drSubjectCDMSDtl("nBloodAvailable") = IIf(String.IsNullOrEmpty(Me.txtAvailableBlood.Text.Trim()), System.DBNull.Value, Me.txtAvailableBlood.Text.Trim())
            drSubjectCDMSDtl("nBloodUsed") = IIf(String.IsNullOrEmpty(Me.txtUsedBlood.Text.Trim()), System.DBNull.Value, Me.txtUsedBlood.Text.Trim())
            drSubjectCDMSDtl("dWashOutDate") = IIf(String.IsNullOrEmpty(Me.txtWashoutDate.Text.Trim()), System.DBNull.Value, Me.txtWashoutDate.Text.Trim())
            drSubjectCDMSDtl("vLastStudy") = IIf(String.IsNullOrEmpty(Me.txtLastStudy.Text.Trim()), System.DBNull.Value, Me.txtLastStudy.Text.Trim())
            drSubjectCDMSDtl("cStatus") = "AC"
            drSubjectCDMSDtl("dStartDate") = System.DBNull.Value
            drSubjectCDMSDtl("dEndDate") = System.DBNull.Value
            'drSubjectCDMSDtl("nAge") = IIf(String.IsNullOrEmpty(Me.txtAge.Text.Trim()), System.DBNull.Value, Me.txtAge.Text.Trim())
            drSubjectCDMSDtl("nAge") = IIf(String.IsNullOrEmpty(Me.hdnAge.Value.Trim()), System.DBNull.Value, Me.hdnAge.Value.Trim())
            drSubjectCDMSDtl("vContanct1Prefix") = IIf(String.IsNullOrEmpty(Me.ddlContact2Prefix.SelectedItem.Value.Trim()), System.DBNull.Value, Me.ddlContact2Prefix.SelectedItem.Value.Trim())
            drSubjectCDMSDtl("vContanct2Prefix") = IIf(String.IsNullOrEmpty(Me.ddlContact2Prefix.SelectedItem.Value.Trim()), System.DBNull.Value, Me.ddlContact2Prefix.SelectedItem.Value.Trim())
            drSubjectCDMSDtl("vContactNo1") = IIf(String.IsNullOrEmpty(Me.txtContactNo1.Text.Trim()), System.DBNull.Value, Me.txtContactNo1.Text.Trim())
            drSubjectCDMSDtl("vContactNo2") = IIf(String.IsNullOrEmpty(Me.txtContactNo2.Text.Trim()), System.DBNull.Value, Me.txtContactNo2.Text.Trim())
            drSubjectCDMSDtl("nWeightlb") = IIf(String.IsNullOrEmpty(Me.txtWeightLb.Text.Trim()), System.DBNull.Value, Me.txtWeightLb.Text.Trim())
            drSubjectCDMSDtl("nHeightfeet") = IIf(String.IsNullOrEmpty(Me.txtHeightFeet.Text.Trim()), System.DBNull.Value, Me.txtHeightFeet.Text.Trim())
            drSubjectCDMSDtl("vReferenceSubject") = IIf(String.IsNullOrEmpty(Me.hdnReferenceSubject.Value.Trim()), System.DBNull.Value, Me.hdnReferenceSubject.Value.Trim())
            drSubjectCDMSDtl("vRecruitingSource") = IIf(String.IsNullOrEmpty(Me.ddlRecruitingSource.SelectedItem.Text.Trim()), System.DBNull.Value, Me.ddlRecruitingSource.SelectedItem.Text.Trim())
            drSubjectCDMSDtl("vRsvpId") = IIf(String.IsNullOrEmpty(Me.txtRsvp.Text.Trim()), System.DBNull.Value, Me.txtRsvp.Text.Trim())
            'drSubjectCDMSDtl("vStudyHistory") = IIf(String.IsNullOrEmpty(Me.txtStudyhistory.Text.Trim()), System.DBNull.Value, Me.txtStudyhistory.Text.Trim())
            drSubjectCDMSDtl("iModifyBy") = Me.Session(S_UserID)

            dt_SubjectCDMSDtl.Rows.Add(drSubjectCDMSDtl)
            dt_SubjectCDMSDtl.TableName = "SubjectDtlCDMS"
            Me.ViewState(VS_DtSubjectDtl) = dt_SubjectCDMSDtl


            'Assign Values for SubjectCDMSDtlHistory
            dt_CDMSDtlHistory = Me.ViewState(VS_DtCDMSDtlHistory)
            dt_CDMSDtlHistory.Clear()
            For Each dc As DataColumn In dt_SubjectCDMSDtl.Columns
                If dc.ColumnName <> "nSubjectDtlCDMSNo" And _
                   dc.ColumnName <> "iModifyBy" And _
                   dc.ColumnName <> "dModifyOn" And _
                   dc.ColumnName <> "cStatusIndi" Then
                    drCDMSDtlHistory = dt_CDMSDtlHistory.NewRow()
                    drCDMSDtlHistory("vSubjectID") = Pro_Screening
                    drCDMSDtlHistory("vColumnName") = dc.ColumnName
                    drCDMSDtlHistory("vChangedValue") = dt_SubjectCDMSDtl.Rows(0)(dc.ColumnName).ToString
                    drCDMSDtlHistory("vRemarks") = ""
                    drCDMSDtlHistory("iModifyBy") = Me.Session(S_UserID)
                    dt_CDMSDtlHistory.Rows.Add(drCDMSDtlHistory)
                End If
            Next
            dt_CDMSDtlHistory.TableName = "SubjectDtlCDMSHistory"
            Me.ViewState(VS_DtCDMSDtlHistory) = dt_CDMSDtlHistory

            dt_CDMSDtlConsu = Me.ViewState(VS_DtConsumption)
            dt_CDMSDtlConsu.Clear()
            'dt_CDMSDtlConsu.Columns.Add("vRemarks")
            dt_CDMSDtlConsu.AcceptChanges()
            'Assign Values for SubjectDtlCDMSConsumption
            For Each grRow As GridViewRow In Me.grdGeneralConmp.Rows

                drCDMSDtlConsu = dt_CDMSDtlConsu.NewRow()
                drCDMSDtlConsu("nCDMSConsumptionNo") = grRow.Cells(GVCConsu_CDMSConsumptionNo).Text
                drCDMSDtlConsu("vSubjectID") = Pro_Screening
                drCDMSDtlConsu("vStatus") = CType(grRow.Cells(GVCConsu_Status).FindControl("ddlConmpStatus"), DropDownList).SelectedItem.Text.Trim()
                drCDMSDtlConsu("nQuantity") = IIf(String.IsNullOrEmpty(CType(grRow.Cells(GVCConsu_Quantity).FindControl("txtConmpQuantity"), TextBox).Text.Trim()), _
                                                  System.DBNull.Value, _
                                                  CType(grRow.Cells(GVCConsu_Quantity).FindControl("txtConmpQuantity"), TextBox).Text.Trim())
                drCDMSDtlConsu("vFrequency") = CType(grRow.Cells(GVCConsu_Frequency).FindControl("ddlConmpFrequency"), DropDownList).SelectedItem.Text.Trim()
                drCDMSDtlConsu("dStartDate") = IIf(String.IsNullOrEmpty(CType(grRow.Cells(GVCConsu_StartDate).FindControl("txtStartDate"), TextBox).Text.Trim()), _
                                                   System.DBNull.Value, _
                                                   CType(grRow.Cells(GVCConsu_StartDate).FindControl("txtStartDate"), TextBox).Text.Trim())
                drCDMSDtlConsu("dEndDate") = IIf(String.IsNullOrEmpty(CType(grRow.Cells(GVCConsu_EndDate).FindControl("txtEndDate"), TextBox).Text.Trim()), _
                                                 System.DBNull.Value, _
                                                 CType(grRow.Cells(GVCConsu_EndDate).FindControl("txtEndDate"), TextBox).Text.Trim())
                drCDMSDtlConsu("iModifyBy") = Me.Session(S_UserID)
                'drCDMSDtlConsu("vRemarks") = System.DBNull.Value
                dt_CDMSDtlConsu.Rows.Add(drCDMSDtlConsu)
            Next
            dt_CDMSDtlConsu.TableName = "SubjectDtlCDMSConsumption"
            Me.ViewState(VS_DtConsumption) = dt_CDMSDtlConsu

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...AssignValues()")
            Return False
        End Try

    End Function

    Private Function AssignValuesToControl() As Boolean

        Dim dt_SubjectMaster As New DataTable
        Dim dt_SubjectCDMSDtl As New DataTable
        Dim dt_SubjectConsumption As New DataTable
        Dim ds_refersubjectdtl As New DataSet
        Dim Wstr As String = String.Empty
        Dim Status As String = String.Empty
        Dim CurrentProject As String = String.Empty
        Dim ds_ProjectDtl As New DataSet
        Dim LabelText As String = String.Empty
        Try

            If Not Me.ViewState(VS_DtSubjectMst) Is Nothing Then
                dt_SubjectMaster = Me.ViewState(VS_DtSubjectMst)

                If dt_SubjectMaster.Rows.Count > 0 Then
                    Me.txtFirstName.Text = dt_SubjectMaster.Rows(0)("vFirstName").ToString
                    Me.txtMiddleName.Text = dt_SubjectMaster.Rows(0)("vMiddleName").ToString
                    Me.txtLastName.Text = dt_SubjectMaster.Rows(0)("vSurName").ToString
                    If dt_SubjectMaster.Rows(0)("dEnrollmentDate").ToString <> "" Then
                        Me.txtEnrollmentDate.Text = DateTime.Parse(dt_SubjectMaster.Rows(0)("dEnrollmentDate")).ToString("dd-MMM-yyyy")
                    End If
                End If
            End If

            If Not Me.ViewState(VS_DtSubjectDtl) Is Nothing Then
                dt_SubjectCDMSDtl = Me.ViewState(VS_DtSubjectDtl)
                dt_SubjectCDMSDtl.Columns.Add("vSubjectAge", System.Type.GetType("System.String"))
                For Each dr_Row In dt_SubjectCDMSDtl.Rows
                    dr_Row("vSubjectAge") = (DateDiff(DateInterval.Day, Date.Parse(dr_Row("dBirthDate")), Date.Parse(DateTime.Now)) / 365.25).ToString.Split(".")(0)
                Next
                If dt_SubjectCDMSDtl.Rows.Count > 0 Then
                    Me.txtInitials.Text = dt_SubjectCDMSDtl.Rows(0)("vInitials").ToString
                    Me.txtAddress.Text = dt_SubjectCDMSDtl.Rows(0)("vAddress").ToString
                    Me.txtEmail.Text = dt_SubjectCDMSDtl.Rows(0)("vEmailAddress").ToString
                    Me.txtContactComments.Text = dt_SubjectCDMSDtl.Rows(0)("vContactComments").ToString
                    If dt_SubjectCDMSDtl.Rows(0)("dBirthdate").ToString <> "" Then
                        Me.txtBirthdate.Text = DateTime.Parse(dt_SubjectCDMSDtl.Rows(0)("dBirthdate")).ToString("dd-MMM-yyyy")
                    End If
                    Me.ddlSex.SelectedIndex = Me.ddlSex.Items.IndexOf(Me.ddlSex.Items.FindByValue(dt_SubjectCDMSDtl.Rows(0)("cSex").ToString))
                    Me.tdFemale.Style("display") = "none"
                    If dt_SubjectCDMSDtl.Rows(0)("cSex").ToString = "F" Then
                        Me.tdFemale.Style("display") = "block"
                    End If
                    Me.ddlLanguage.SelectedIndex = Me.ddlLanguage.Items.IndexOf(Me.ddlLanguage.Items.FindByValue(dt_SubjectCDMSDtl.Rows(0)("vLanguage").ToString))
                    Me.txtMenstrualCycle.Text = dt_SubjectCDMSDtl.Rows(0)("iMenstrualCycle").ToString
                    Me.ddlRegular.SelectedIndex = Me.ddlRegular.Items.IndexOf(Me.ddlRegular.Items.FindByValue(dt_SubjectCDMSDtl.Rows(0)("cRegular").ToString))
                    Me.ddlRace.SelectedIndex = Me.ddlRace.Items.IndexOf(Me.ddlRace.Items.FindByValue(dt_SubjectCDMSDtl.Rows(0)("vRace").ToString))
                    Me.ddlAvailability.SelectedIndex = Me.ddlAvailability.Items.IndexOf(Me.ddlAvailability.Items.FindByValue(dt_SubjectCDMSDtl.Rows(0)("vAvailiability").ToString))
                    Me.ddlTransportation.SelectedIndex = Me.ddlTransportation.Items.IndexOf(Me.ddlTransportation.Items.FindByValue(dt_SubjectCDMSDtl.Rows(0)("vTransportation").ToString))
                    Me.ddlRegularDiet.SelectedIndex = Me.ddlRegularDiet.Items.IndexOf(Me.ddlRegularDiet.Items.FindByValue(dt_SubjectCDMSDtl.Rows(0)("cRegularDiet").ToString))
                    Me.ddlSwallowPill.SelectedIndex = Me.ddlSwallowPill.Items.IndexOf(Me.ddlSwallowPill.Items.FindByValue(dt_SubjectCDMSDtl.Rows(0)("cSwallowPil").ToString))
                    Me.ddlContactFuture.SelectedIndex = Me.ddlContactFuture.Items.IndexOf(Me.ddlContactFuture.Items.FindByValue(dt_SubjectCDMSDtl.Rows(0)("cContactForFutureStudies").ToString))
                    Me.txtHeight.Text = dt_SubjectCDMSDtl.Rows(0)("nHeight").ToString
                    Me.txtWeight.Text = dt_SubjectCDMSDtl.Rows(0)("nWeight").ToString
                    Me.txtBMI.Text = dt_SubjectCDMSDtl.Rows(0)("nBMI").ToString
                    Me.txtComments.Text = dt_SubjectCDMSDtl.Rows(0)("vComments").ToString
                    Me.txtAvailableBlood.Text = dt_SubjectCDMSDtl.Rows(0)("nBloodAvailable").ToString
                    Me.txtUsedBlood.Text = dt_SubjectCDMSDtl.Rows(0)("nBloodUsed").ToString
                    If dt_SubjectCDMSDtl.Rows(0)("dWashOutDate").ToString <> "" Then
                        If IsDate(dt_SubjectCDMSDtl.Rows(0)("dWashOutDate").ToString) Then
                            Me.txtWashoutDate.Text = DateTime.Parse(dt_SubjectCDMSDtl.Rows(0)("dWashOutDate")).ToString("dd-MMM-yyyy")
                        Else
                            Me.txtWashoutDate.Text = dt_SubjectCDMSDtl.Rows(0)("dWashOutDate").ToString()
                        End If
                    End If
                    Me.txtLastStudy.Text = dt_SubjectCDMSDtl.Rows(0)("vLastStudy").ToString
                    Me.txtAge.Text = dt_SubjectCDMSDtl.Rows(0)("vSubjectAge").ToString
                    Me.txtContactNo1.Text = dt_SubjectCDMSDtl.Rows(0)("vContactNo1").ToString
                    Me.txtContactNo2.Text = dt_SubjectCDMSDtl.Rows(0)("vContactNo2").ToString
                    Me.ddlContact1Prefix.SelectedIndex = Me.ddlContact1Prefix.Items.IndexOf(Me.ddlContact1Prefix.Items.FindByValue(dt_SubjectCDMSDtl.Rows(0)("vContanct1Prefix").ToString))
                    Me.ddlContact2Prefix.SelectedIndex = Me.ddlContact2Prefix.Items.IndexOf(Me.ddlContact2Prefix.Items.FindByValue(dt_SubjectCDMSDtl.Rows(0)("vContanct2Prefix").ToString))
                    Me.txtWeightLb.Text = dt_SubjectCDMSDtl.Rows(0)("nWeightlb").ToString
                    Me.txtHeightFeet.Text = dt_SubjectCDMSDtl.Rows(0)("nHeightfeet").ToString
                    Me.txtRsvp.Text = dt_SubjectCDMSDtl.Rows(0)("vRsvpId").ToString
                    ' Me.txtStudyhistory.Text = dt_SubjectCDMSDtl.Rows(0)("vStudyHistory").ToString

                    If dt_SubjectCDMSDtl.Rows(0)("cStatus").ToString = "AC" Then
                        Status = "Active"
                        Me.lblStatus.Style.Add("color", "green")
                    ElseIf dt_SubjectCDMSDtl.Rows(0)("cStatus").ToString = "IA" Then
                        Status = "Inactive"
                        Me.lblStatus.Style.Add("color", "maroon")
                    ElseIf dt_SubjectCDMSDtl.Rows(0)("cStatus").ToString = "HO" Then
                        Status = "On-Hold"
                        Me.lblStatus.Style.Add("color", "olive")
                        If Not IsDBNull(dt_SubjectCDMSDtl.Rows(0)("dEndDate")) Then
                            If dt_SubjectCDMSDtl.Rows(0)("dEndDate").ToString() <> "" Then
                                If Date.Now > DateTime.Parse(dt_SubjectCDMSDtl.Rows(0)("dEndDate")).ToString("dd-MMM-yyyy") Then
                                    Status = "Active"
                                    Me.lblStatus.Style.Add("color", "green")
                                End If
                            End If
                        End If
                    ElseIf dt_SubjectCDMSDtl.Rows(0)("cStatus").ToString = "SC" Then
                        If Not AssignProjectNo("SC", dt_SubjectCDMSDtl.Rows(0)("vSubjectID").ToString, LabelText) Then
                            Me.ShowErrorMessage("Error While Assigning Project no To status", "")
                            Exit Function
                        End If
                        Status = LabelText
                        'Me.lblStatus.Style.Add("color", "#3A7DC1")
                    ElseIf dt_SubjectCDMSDtl.Rows(0)("cStatus").ToString = "BO" Then
                        If Not AssignProjectNo("BO", dt_SubjectCDMSDtl.Rows(0)("vSubjectID").ToString, LabelText) Then
                            Me.ShowErrorMessage("Error While Assigning Project no To status", "")
                            Exit Function
                        End If
                        Status = LabelText
                    ElseIf dt_SubjectCDMSDtl.Rows(0)("cStatus").ToString = "OS" Then
                        If Not AssignProjectNo("OS", dt_SubjectCDMSDtl.Rows(0)("vSubjectID").ToString, LabelText) Then
                            Me.ShowErrorMessage("Error While Assigning Project no To status", "")
                            Exit Function
                        End If
                        Status = LabelText
                        'Status = "On Study"
                        'Me.lblStatus.Style.Add("color", "blue")
                    ElseIf dt_SubjectCDMSDtl.Rows(0)("cStatus").ToString = "FI" Then
                        Status = "Forever Ineligible"
                        Me.lblStatus.Style.Add("color", "red")
                    End If
                    Me.lblStatus.InnerText = "Status - " & Status
                    Me.lblStatus.Style.Add("font-size", "13px")
                    Me.lblStatus.Style.Add("font-weight", "bolder")
                    Me.ddlRecruitingSource.SelectedIndex = Me.ddlRecruitingSource.Items.IndexOf(Me.ddlRecruitingSource.Items.FindByText(dt_SubjectCDMSDtl.Rows(0)("vRecruitingSource").ToString))
                    Me.txtreferSubject.Text = ""
                    Me.hdnReferenceSubject.Value = ""
                    Me.trReference.Style("display") = "none"
                    If dt_SubjectCDMSDtl.Rows(0)("vRecruitingSource").ToString = "Word of Mouth" Then
                        Me.trReference.Style("display") = ""
                        ' Me.hdnReferenceSubject.Value = dt_SubjectCDMSDtl.Rows(0)("vReferenceSubject").ToString
                        If dt_SubjectCDMSDtl.Rows(0)("vReferenceSubject").ToString <> "" Then
                            Wstr = "SELECT vFirstName,vMiddleName,vSurName,vContactNo1,vSubjectID FROM View_cdmssubjectdetails WHERE vsubjectid = '" + dt_SubjectCDMSDtl.Rows(0)("vReferenceSubject").ToString + "'"
                            ds_refersubjectdtl = objhelpDb.GetResultSet(Wstr, "View_cdmssubjectdetails")
                            If Not ds_refersubjectdtl Is Nothing Then
                                If ds_refersubjectdtl.Tables(0).Rows.Count > 0 Then
                                    Me.txtreferSubject.Text = ds_refersubjectdtl.Tables(0).Rows(0)("vSurName").ToString + " " + ds_refersubjectdtl.Tables(0).Rows(0)("vFirstName").ToString + " " + ds_refersubjectdtl.Tables(0).Rows(0)("vMiddleName").ToString + " (" + ds_refersubjectdtl.Tables(0).Rows(0)("vSubjectID").ToString + ") (" + ds_refersubjectdtl.Tables(0).Rows(0)("vContactNo1").ToString + ")"
                                End If
                            End If
                        End If
                    End If
                End If
            End If

            If Not Me.ViewState(VS_DtConsumption) Is Nothing Then
                dt_SubjectConsumption = Me.ViewState(VS_DtConsumption)
                If dt_SubjectConsumption.Rows.Count > 0 Then
                    grdGeneralConmp.DataSource = Me.ViewState(VS_DtConsumptionList)
                    grdGeneralConmp.DataBind()
                    For Each grdRow As GridViewRow In grdGeneralConmp.Rows
                        If grdRow.RowIndex < dt_SubjectConsumption.Rows.Count Then
                            For Each dr_Row In dt_SubjectConsumption.Rows
                                If dr_Row("nCdmsConsumptionNo") = grdRow.Cells(GVCConsu_CDMSConsumptionNo).Text Then
                                    If dr_Row("dStartDate").ToString() <> "" Then
                                        If IsDate(dr_Row("dStartDate").ToString()) Then
                                            CType(grdRow.Cells(GVCConsu_StartDate).FindControl("txtStartDate"), TextBox).Text = DateTime.Parse(dr_Row("dStartDate")).ToString("dd-MMM-yyyy")
                                        Else
                                            CType(grdRow.Cells(GVCConsu_StartDate).FindControl("txtStartDate"), TextBox).Text = dr_Row("dStartDate").ToString()
                                        End If
                                    End If
                                    If dr_Row("dEndDate").ToString() <> "" Then
                                        If IsDate(dr_Row("dEndDate").ToString()) Then
                                            CType(grdRow.Cells(GVCConsu_StartDate).FindControl("txtEndDate"), TextBox).Text = DateTime.Parse(dr_Row("dEndDate")).ToString("dd-MMM-yyyy")
                                        Else
                                            CType(grdRow.Cells(GVCConsu_StartDate).FindControl("txtEndDate"), TextBox).Text = dr_Row("dEndDate").ToString()
                                        End If

                                    End If
                                    CType(grdRow.Cells(GVCConsu_Quantity).FindControl("txtConmpQuantity"), TextBox).Text = dr_Row("nQuantity").ToString()
                                    CType(grdRow.Cells(GVCConsu_Status).FindControl("ddlConmpStatus"), DropDownList).SelectedValue = dr_Row("vStatus").ToString()
                                    CType(grdRow.Cells(GVCConsu_Frequency).FindControl("ddlConmpFrequency"), DropDownList).SelectedValue = dr_Row("vFrequency").ToString()
                                End If
                            Next
                        End If
                    Next
                End If
            End If

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...AssingValuesToControl()")
            Return False
        End Try

    End Function

    Private Function AssignProjectNo(ByVal Status As String, ByVal SubjectID As String, ByRef LabelText As String) As Boolean
        Dim Wstr As String = String.Empty
        Dim ds_ProjectDtl As New DataSet
        Dim CurrentProject As String = String.Empty
        Try
            ''Commented and added by Aaditya for show latest project book Information
            ''Wstr = "SELECT * FROM View_CDMSSubjectDetailsStatus WHERE vSubjectid = '" + SubjectID + "'"
            ''ds_ProjectDtl = objhelpDb.GetResultSet(Wstr, "View_CDMSSubjectDetailsStatus")
            Wstr = SubjectID
            ds_ProjectDtl = Me.objhelpDb.ProcedureExecute("dbo.PROC_CDMSSubjectDetailsStatus", Wstr)
            ''Ended by Aaditya

            If Not ds_ProjectDtl Is Nothing Then
                If ds_ProjectDtl.Tables(0).Rows.Count > 0 Then
                    CurrentProject = ds_ProjectDtl.Tables(0).Rows(0)("vProjectNo").ToString()
                    Me.hdnWorkSpaceID.Value = ds_ProjectDtl.Tables(0).Rows(0)("vWorkSpaceId").ToString()
                End If
            End If
            If Status = "BO" Then
                LabelText = "Booked [" + CurrentProject + "]"
                Me.lblStatus.Style.Add("color", "#A46A15")
            ElseIf Status = "SC" Then
                LabelText = "Screened [" + CurrentProject + "]"
                Me.lblStatus.Style.Add("color", "#3A7DC1")
            ElseIf Status = "OS" Then
                LabelText = "On Study [" + CurrentProject + "]"
                Me.lblStatus.Style.Add("color", "blue")
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region

#Region "Button Events"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim ds_SubjectMaster As New DataSet
        Dim eStr As String = String.Empty
        Dim SubjectId As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try
            Choice = Me.ViewState(VS_Choice)

            If Not ValidateSubject() Then
                objCommon.ShowAlert("Subject already exists with the same Name.", Me.Page)
                Exit Sub
            End If

            If Not AssignValues() Then
                Me.objCommon.ShowAlert("Error while Assinging Data", Me.Page)
                Exit Sub
            End If

            ds_SubjectMaster.Tables.Add(CType(Me.ViewState(VS_DtSubjectMst), DataTable).Copy())
            ds_SubjectMaster.Tables.Add(CType(Me.ViewState(VS_DtSubjectDtl), DataTable).Copy())
            ds_SubjectMaster.Tables.Add(CType(Me.ViewState(VS_DtCDMSDtlHistory), DataTable).Copy())
            ds_SubjectMaster.Tables.Add(CType(Me.ViewState(VS_DtConsumption), DataTable).Copy())

            If Not objLambda.Save_CDMSSubjectMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_SubjectMaster, _
                                                 Me.Session(S_UserID), SubjectId, eStr) Then
                Me.ShowErrorMessage(eStr, "Error while saving subject master.")
                Exit Sub
            End If
            Me.ViewState(VS_SubjectID) = SubjectId
            Me.HSubjectId.Value = SubjectId
            Me.mdlSaveRedirect.Show()
            'objCommon.ShowAlert("SubjectId Information Saved Sucessfully", Me.Page)
            '

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnSaveSubject")
        End Try

    End Sub

    Protected Sub btnCancelCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelCancel.Click
        ''ResetPage()
        Me.Response.Redirect("frmCDMSSubjectInformation.aspx?Mode=1")
    End Sub

    Protected Sub imgAuditConsumption_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgAuditConsumption.Click
        Dim ds_Audit As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try

            wStr = "vSubjectID = '" + Me.ViewState(VS_SubjectID).ToString() + "' Order by cStatusIndi Desc"

            If Not objhelpDb.View_SubjectDtlCDMSConsumption(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                               ds_Audit, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds_Audit Is Nothing Then
                If ds_Audit.Tables(0).Rows.Count > 0 Then
                    Me.grdAudit.DataSource = ds_Audit
                    Me.grdAudit.DataBind()
                Else
                    Me.grdAudit.DataSource = Nothing
                    Me.grdAudit.DataBind()
                End If
            End If
            Me.mdlConsAudit.Show()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StrfnApplyDataTable", "fnApplyDataTable();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "fnConDataTablestr", "fnConDataTable();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "fnConDataTablestr1", "fnEditField();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "fnConDataTablestr2", "fnAuditTrail();", True)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillGrid")
        End Try
    End Sub

    Protected Sub btnSearchSubject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchSubject.Click

        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_SubjectMst As DataSet = Nothing
        Dim ds_CDMSSubjectDtl As DataSet = Nothing
        Dim ds_CDMSSubjectConsumption As DataSet = Nothing
        Dim ds_Image As DataSet = Nothing


        Try

            Me.ViewState(VS_SubjectID) = Me.HSubjectId.Value.Trim()
            Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
            Me.hdnWorkSpaceID.Value = ""
            wStr = "vSubjectId='" + Me.ViewState(VS_SubjectID).ToString + "' And cStatusIndi <> 'D'"

            If Not objhelpDb.GetView_CDMSSubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubjectMst, eStr) Then
                Throw New Exception(eStr)
            End If
            Me.ViewState(VS_DtSubjectMst) = ds_SubjectMst.Tables(0)

            'SubjectDtlCDMS
            If Not objhelpDb.getSubjectDtlCDMS(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CDMSSubjectDtl, eStr) Then
                Throw New Exception(eStr)
            End If
            Me.ViewState(VS_DtSubjectDtl) = ds_CDMSSubjectDtl.Tables(0)

            'SubjectDtlCDMSConsumption
            If Not objhelpDb.View_SubjectDtlCDMSConsumption(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_CDMSSubjectConsumption, eStr) Then
                Throw New Exception(eStr)
            End If
            Me.ViewState(VS_DtConsumption) = ds_CDMSSubjectConsumption.Tables(0)

            If Not AssignValuesToControl() Then
                Throw New Exception("AssignValuesToControl")
            End If

            If Not AssingAttribute() Then
                Throw New Exception("AssingAttribute")
            End If
            Me.btnSaveSubject.Visible = False

            wStr = "cRecordType='P' and iTranNo=(select Max(itranNO) from SubjectBlobDetails where cRecordType='P' and vSubjectId='" & HSubjectId.Value.ToString.Trim & "') and vSubjectId='" & Me.ViewState(VS_SubjectID).ToString.Trim & "'"

            If Not Me.objhelpDb.getSubjectBlobDetails(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Image, eStr) Then
                Throw New Exception("Error while getting Subject Photograph...")
            End If

            If Not ds_Image Is Nothing Then
                If ds_Image.Tables(0).Rows.Count > 0 Then
                    'Me.imgSubjectPhoto.ImageUrl = "~/frmPIFImage.aspx?subjectid=AH11-01131"
                    Me.imgSubjectPhoto.ImageUrl = "~/frmPIFImage.aspx?subjectid=" + Me.ViewState(VS_SubjectID).ToString
                    Me.lblSubjectPhoto.Text = CDate(ds_Image.Tables(0).Rows(0).Item("dModifyOn").ToString()).ToString("dd-MMM-yyyy")
                Else
                    Me.imgSubjectPhoto.ImageUrl = "~/CDMS/images/NotFound.gif"
                    Me.lblSubjectPhoto.Text = ""
                End If
            Else
                Me.imgSubjectPhoto.ImageUrl = "~/CDMS/images/NotFound.gif"
                Me.lblSubjectPhoto.Text = ""
            End If

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StrfnFunctionCall", "fnFunctionCall();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StrfnAssignProperties", "fnAssignProperties();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "strfnApplyDataTableKey", "fnApplyDataTable();", True)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...SearchProject()")
        End Try



    End Sub

    Protected Sub btnOkAlert_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOkAlert.Click
        Me.Response.Redirect("frmCDMSMedicalCondition.aspx?Mode=2&SubjectID=" & Me.ViewState(VS_SubjectID))
    End Sub

    Protected Sub btnNewEntry_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewEntry.Click

        Me.Response.Redirect("frmCDMSSubjectInformation.aspx?Mode=1")
    End Sub

#End Region

#Region "Grid Events"

    Protected Sub grdAudit_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdAudit.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("imgAudit"), ImageButton).CommandName = "Audit"
            CType(e.Row.FindControl("imgAudit"), ImageButton).CommandArgument = e.Row.RowIndex
        End If

    End Sub

    Protected Sub grdAudit_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdAudit.RowCommand

        Dim ds_Audit As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty

        Try
            If e.CommandName = "Audit" Then

                wStr = "vSubjectID = '" + Me.ViewState(VS_SubjectID).ToString() + "' And vConsumptionCode = '" + _
                        CType(grdAudit.Rows(e.CommandArgument).Cells(GVCConsuAudit_Code).FindControl("hdnCode"), HiddenField).Value + "'"

                If Not objhelpDb.View_AuditSubjectDtlCDMSConsumption(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                   ds_Audit, eStr) Then
                    Throw New Exception(eStr)
                End If

                If Not ds_Audit Is Nothing Then
                    If ds_Audit.Tables(0).Rows.Count > 0 Then
                        ds_Audit.Tables(0).Columns.Add("dModifyOffSet", Type.GetType("System.String"))
                        For Each dr_Audit In ds_Audit.Tables(0).Rows
                            dr_Audit("dModifyOffSet") = Convert.ToString(CDate(dr_Audit("dModifyOn")).ToString("dd-MMM-yyyy HH:mm") + strServerOffset)
                        Next 'CType(Replace(e.Row.Cells(GVCSub_dModifyOn).Text.Trim(), "&nbsp;", ""), Date).ToString("dd-MMM-yyyy HH:mm").Trim() + strServerOffset.ToString.Replace("IST ", "")
                        Me.grdRowAudit.DataSource = ds_Audit
                        Me.grdRowAudit.DataBind()
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StrfnApplyDataTableAudit", "fnApplyDataTable();", True)
                        Me.mdlRowAudit.Show()
                    Else
                        Me.grdRowAudit.DataSource = Nothing
                        Me.grdRowAudit.DataBind()
                    End If
                End If

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...grdAudit_RowCommand()")
        End Try

    End Sub

    Protected Sub grdGeneralConmp_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdGeneralConmp.RowDataBound

        If e.Row.RowType = DataControlRowType.Header Or _
           e.Row.RowType = DataControlRowType.DataRow Or _
           e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(GVCConsu_CDMSConsumptionNo).Visible = False
        End If
    End Sub

#End Region

#Region "Web Methods"

    <Web.Services.WebMethod()> _
    Public Shared Function UpdateFieldValues(ByVal SubjectId As String, ByVal workspaceid As String, ByVal ColumnName As String, _
                                             ByVal TableName As String, ByVal ChangedValue As String, _
                                             ByVal Remarks As String, ByVal Refcolumn As String, ByVal JSONString As String) As Boolean
        Dim objCommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

        Dim eStr As String = String.Empty

        Dim ds_Consumption As New DataSet
        Dim ds_Field As New DataSet
        Dim dt_Field As New DataTable
        Dim ds_Save As New DataSet
        Dim dr_Field As DataRow
        Dim ds_Status As New DataSet
        Dim dt_Status As New DataTable

        Try

            If JSONString = "" Then
                dt_Field.Columns.Add("vSubjectID", Type.GetType("System.String"))
                dt_Field.Columns.Add("vTableName", Type.GetType("System.String"))
                dt_Field.Columns.Add("vColumnName", Type.GetType("System.String"))
                dt_Field.Columns.Add("vChangedValue", Type.GetType("System.String"))
                dt_Field.Columns.Add("vRemarks", Type.GetType("System.String"))
                dt_Field.Columns.Add("iModifyBy", Type.GetType("System.Int32"))


                dr_Field = dt_Field.NewRow
                dr_Field("vSubjectID") = SubjectId
                dr_Field("vTableName") = TableName
                dr_Field("vColumnName") = ColumnName
                dr_Field("vChangedValue") = ChangedValue
                dr_Field("vRemarks") = Remarks
                dr_Field("iModifyBy") = HttpContext.Current.Session(S_UserID)
                dt_Field.Rows.Add(dr_Field)

                ds_Field.Tables.Add(dt_Field)

                If Not objLambda.Insert_UpdateSubjectCDMSDtlField(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Field, _
                                                     HttpContext.Current.Session(S_UserID), eStr) Then
                    Return False
                    Exit Function
                End If
                If ColumnName = "cStatus" AndAlso workspaceid <> "" Then
                    dt_Status.Columns.Add("nSubjectDtlCDMSStatusNo", Type.GetType("System.String"))
                    dt_Status.Columns.Add("vSubjectId", Type.GetType("System.String"))
                    dt_Status.Columns.Add("vWorkSpaceId", Type.GetType("System.String"))
                    dt_Status.Columns.Add("cStatus", Type.GetType("System.String"))
                    dt_Status.Columns.Add("iTranNo", Type.GetType("System.String"))
                    dt_Status.Columns.Add("iModifyBy", Type.GetType("System.Int32"))
                    dt_Status.Columns.Add("dModifyOn", Type.GetType("System.String"))
                    dt_Status.Columns.Add("cStatusIndi", Type.GetType("System.String"))

                    dr_Field = dt_Status.NewRow
                    dr_Field("vSubjectID") = SubjectId
                    dr_Field("vWorkSpaceId") = workspaceid
                    dr_Field("cStatus") = ChangedValue
                    dr_Field("iModifyBy") = HttpContext.Current.Session(S_UserID)
                    dt_Status.Rows.Add(dr_Field)


                    ds_Status.Tables.Add(dt_Status)

                    If Not objLambda.Save_SubjectDtlCDMSStatus(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Status, _
                                                         eStr) Then
                        Return False
                        Exit Function
                    End If
                End If
                If ColumnName = "cSex" Then
                    For Each dr_Field In ds_Field.Tables(0).Rows
                        dr_Field("vTableName") = "SubjectMaster"
                        ds_Field.AcceptChanges()
                    Next
                    If Not objLambda.Insert_UpdateSubjectCDMSDtlField(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Field, _
                                                     HttpContext.Current.Session(S_UserID), eStr) Then
                        Return False
                        Exit Function
                    End If
                End If

            Else

                If Refcolumn = "" Then
                    If Not objhelpDb.getSubjectDtlCDMSConsumption("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Consumption, eStr) Then
                        Throw New Exception(eStr)
                    End If

                    ds_Field.Tables.Add(JsonConvert.DeserializeObject(JSONString, GetType(System.Data.DataTable)))

                    ds_Consumption.Tables(0).Columns.Add("vRemarks", System.Type.GetType("System.String"))

                    For Each dr As DataRow In ds_Field.Tables(0).Rows
                        dr_Field = ds_Consumption.Tables(0).NewRow()
                        dr_Field("nSubjectDtlCDMSConsumptionNo") = 1
                        dr_Field("vSubjectID") = SubjectId
                        dr_Field("nCDMSConsumptionNo") = dr("nCDMSConsumptionNo")
                        dr_Field("vStatus") = dr("vStatus")
                        dr_Field("nQuantity") = IIf(String.IsNullOrEmpty(dr("nQuantity")), System.DBNull.Value, dr("nQuantity"))
                        dr_Field("vFrequency") = dr("vFrequency")
                        dr_Field("dStartDate") = IIf(String.IsNullOrEmpty(dr("dStartDate")), System.DBNull.Value, dr("dStartDate"))
                        dr_Field("dEndDate") = IIf(String.IsNullOrEmpty(dr("dEndDate")), System.DBNull.Value, dr("dEndDate"))
                        dr_Field("iModifyBy") = HttpContext.Current.Session(S_UserID)
                        dr_Field("dModifyOn") = DateTime.Now
                        dr_Field("vRemarks") = Remarks
                        ds_Consumption.Tables(0).Rows.Add(dr_Field)
                    Next

                    If Not objLambda.Save_SubjectDtlCDMSConsumption(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Consumption, _
                                                         HttpContext.Current.Session(S_UserID), eStr) Then
                        Return False
                        Exit Function
                    End If
                ElseIf Refcolumn = "TRUE" Then
                    ds_Field.Tables.Add(JsonConvert.DeserializeObject(JSONString, GetType(System.Data.DataTable)))


                    dt_Field.Columns.Add("vSubjectID", Type.GetType("System.String"))
                    dt_Field.Columns.Add("vTableName", Type.GetType("System.String"))
                    dt_Field.Columns.Add("vColumnName", Type.GetType("System.String"))
                    dt_Field.Columns.Add("vChangedValue", Type.GetType("System.String"))
                    dt_Field.Columns.Add("vRemarks", Type.GetType("System.String"))
                    dt_Field.Columns.Add("iModifyBy", Type.GetType("System.Int32"))


                    dr_Field = dt_Field.NewRow
                    dr_Field("vSubjectID") = SubjectId
                    dr_Field("vTableName") = TableName
                    dr_Field("vColumnName") = ColumnName
                    dr_Field("vChangedValue") = ChangedValue
                    dr_Field("vRemarks") = Remarks
                    dr_Field("iModifyBy") = HttpContext.Current.Session(S_UserID)
                    dt_Field.Rows.Add(dr_Field)

                    For Each dr As DataRow In ds_Field.Tables(0).Rows
                        dr_Field = dt_Field.NewRow
                        dr_Field("vSubjectID") = SubjectId
                        dr_Field("vTableName") = dr("TableName")
                        dr_Field("vColumnName") = dr("ColumnName")
                        dr_Field("vChangedValue") = dr("ChangedValue")
                        dr_Field("vRemarks") = Remarks
                        dr_Field("iModifyBy") = HttpContext.Current.Session(S_UserID)
                        dt_Field.Rows.Add(dr_Field)
                    Next
                    'ds_Save = Nothing
                    ds_Save.Tables.Add(dt_Field)

                    If Not objLambda.Insert_UpdateSubjectCDMSDtlField(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Save, _
                                                                        HttpContext.Current.Session(S_UserID), eStr) Then
                        Return False
                        Exit Function
                    End If


                End If


            End If

            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    <Web.Services.WebMethod()> _
     Public Shared Function GetAuditTrailField(ByVal SubjectId As String, ByVal ColumnName As String) As String
        Dim objCommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_Audit As New DataSet
        Dim RowIndex As Integer = 1

        Try

            wStr = "vSubjectId = '" + SubjectId + "' And vColumnName = '" + ColumnName + "' Order by dModifyOn "

            If Not objhelpDb.View_AuditSubjectDtlCDMS(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Audit, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds_Audit Is Nothing Then
                If ds_Audit.Tables(0).Rows.Count > 0 Then
                    ds_Audit.Tables(0).TableName = "tblAudit"
                    ds_Audit.Tables(0).Columns.Add("dModifyOffSet", Type.GetType("System.String"))
                    For Each dr_Audit In ds_Audit.Tables(0).Rows
                        '' Added by Rahul 
                        If dr_Audit("vColumnName").ToString().ToUpper = "DBIRTHDATE" Then
                            dr_Audit("vChangedValue") = Convert.ToString(CDate(dr_Audit("vChangedValue")).ToString("dd-MMM-yyyy"))
                        End If
                        '' Ended 
                        dr_Audit("dModifyOffSet") = Convert.ToString(dr_Audit("dModifyOn"))
                    Next
                    Return JsonConvert.SerializeObject(ds_Audit.Tables(0))
                End If
            End If

        Catch ex As Exception
            Return ex.Message
        End Try

    End Function

    <Web.Services.WebMethod()> _
Public Shared Function UpdateStatus(ByVal JSONString As String) As Boolean
        Dim objCommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

        Dim eStr As String = String.Empty
        Dim ds_Field As New DataSet
        Dim workspaceid As String = String.Empty
        Dim dt_Status As New DataTable
        Dim dr_Field As DataRow
        Dim ds_Status As New DataSet
        Dim SubjectId As String = String.Empty

        Try


            ds_Field.Tables.Add(JsonConvert.DeserializeObject(JSONString, GetType(System.Data.DataTable)))
            ds_Field.Tables(0).Columns.Add("iModifyBy", Type.GetType("System.Int32"))

            For Each dr As DataRow In ds_Field.Tables(0).Rows
                dr("iModifyBy") = HttpContext.Current.Session(S_UserID)
                workspaceid = dr("workspaceid").ToString()
                If dr("vSubjectID").ToString() <> "" Then
                    SubjectId = dr("vSubjectID").ToString()
                End If
            Next


            If Not objLambda.Insert_UpdateSubjectCDMSDtlField(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Field, _
                                                 HttpContext.Current.Session(S_UserID), eStr) Then
                Return False
                Exit Function
            End If
            If workspaceid <> "" Then
                dt_Status.Columns.Add("nSubjectDtlCDMSStatusNo", Type.GetType("System.String"))
                dt_Status.Columns.Add("vSubjectId", Type.GetType("System.String"))
                dt_Status.Columns.Add("vWorkSpaceId", Type.GetType("System.String"))
                dt_Status.Columns.Add("cStatus", Type.GetType("System.String"))
                dt_Status.Columns.Add("iTranNo", Type.GetType("System.String"))
                dt_Status.Columns.Add("iModifyBy", Type.GetType("System.Int32"))
                dt_Status.Columns.Add("dModifyOn", Type.GetType("System.String"))
                dt_Status.Columns.Add("cStatusIndi", Type.GetType("System.String"))

                dr_Field = dt_Status.NewRow
                dr_Field("vSubjectID") = SubjectId
                dr_Field("vWorkSpaceId") = workspaceid
                dr_Field("cStatus") = "HO"
                dr_Field("iModifyBy") = HttpContext.Current.Session(S_UserID)
                dt_Status.Rows.Add(dr_Field)


                ds_Status.Tables.Add(dt_Status)

                If Not objLambda.Save_SubjectDtlCDMSStatus(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Status, _
                                                     eStr) Then
                    Return False
                    Exit Function
                End If
            End If

            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

#End Region

#Region "Helper Function"

    Private Sub ResetPage()
        Me.btnSaveSubject.Visible = True
        Me.txtFirstName.Text = ""
        Me.txtMiddleName.Text = ""
        Me.txtLastName.Text = ""
        Me.txtInitials.Text = ""
        Me.txtEnrollmentDate.Text = ""
        Me.txtBirthdate.Text = ""
        Me.txtContactNo1.Text = ""
        Me.txtContactNo2.Text = ""
        Me.txtHeightFeet.Text = ""
        Me.txtWeightLb.Text = ""
        Me.ddlContact1Prefix.SelectedIndex = -1
        Me.ddlContact2Prefix.SelectedIndex = -1
        Me.txtAge.Text = ""
        Me.txtEmail.Text = ""
        Me.txtAddress.Text = ""
        Me.txtContactComments.Text = ""
        Me.ddlContactFuture.SelectedIndex = -1
        Me.txtPlace.Text = ""
        Me.ddlSex.SelectedIndex = -1
        Me.txtMenstrualCycle.Text = ""
        Me.ddlRegular.SelectedIndex = -1
        Me.ddlLanguage.SelectedIndex = -1
        Me.ddlRace.SelectedIndex = -1
        Me.ddlTransportation.SelectedIndex = -1
        Me.ddlAvailability.SelectedIndex = -1
        Me.ddlRegularDiet.SelectedIndex = -1
        Me.ddlSwallowPill.SelectedIndex = -1
        Me.txtWeight.Text = ""
        Me.txtHeight.Text = ""
        Me.txtBMI.Text = ""
        Me.txtComments.Text = ""
        Me.txtAvailableBlood.Text = ""
        Me.txtUsedBlood.Text = ""
        Me.txtWashoutDate.Text = ""
        Me.txtLastStudy.Text = ""
        Me.ddlSubjectStatus.SelectedIndex = -1
        Me.ddlRecruitingSource.SelectedIndex = -1
        Me.hdnReferenceSubject.Value = ""
        Me.txtreferSubject.Text = ""
    End Sub

    Private Function AssingAttribute() As Boolean

        Try

            Me.txtFirstName.Attributes.Add("tName", "SubjectMaster")
            Me.txtFirstName.Attributes.Add("cName", "vFirstName")

            Me.txtMiddleName.Attributes.Add("tName", "SubjectMaster")
            Me.txtMiddleName.Attributes.Add("cName", "vMiddleName")

            Me.txtLastName.Attributes.Add("tName", "SubjectMaster")
            Me.txtLastName.Attributes.Add("cName", "vSurName")

            Me.txtEnrollmentDate.Attributes.Add("tName", "SubjectMaster")
            Me.txtEnrollmentDate.Attributes.Add("cName", "dEnrollmentDate")


            Me.txtBirthdate.Attributes.Add("cName", "dBirthdate")
            Me.txtContactNo1.Attributes.Add("cName", "vContactNo1")
            Me.txtContactNo2.Attributes.Add("cName", "vContactNo2")
            Me.ddlContact1Prefix.Attributes.Add("cName", "vContanct1Prefix")
            Me.ddlContact2Prefix.Attributes.Add("cName", "vContanct2Prefix")
            Me.txtAge.Attributes.Add("cName", "nAge")
            Me.txtWeightLb.Attributes.Add("cName", "nWeightlb")
            Me.txtHeightFeet.Attributes.Add("cName", "nHeightfeet")
            Me.txtEmail.Attributes.Add("cName", "vEmailAddress")
            Me.txtAddress.Attributes.Add("cName", "vAddress")
            Me.txtContactComments.Attributes.Add("cName", "vContactComments")
            Me.ddlContactFuture.Attributes.Add("cName", "cContactForFutureStudies")
            Me.txtPlace.Attributes.Add("cName", "vPlace")
            Me.ddlSex.Attributes.Add("cName", "cSex")
            Me.txtMenstrualCycle.Attributes.Add("cName", "iMenstrualCycle")
            Me.ddlRegular.Attributes.Add("cName", "cRegular")
            Me.ddlLanguage.Attributes.Add("cName", "vLanguage")
            Me.ddlRace.Attributes.Add("cName", "vRace")
            Me.ddlTransportation.Attributes.Add("cName", "vTransportation")
            Me.ddlAvailability.Attributes.Add("cName", "vAvailiability")
            Me.ddlRegularDiet.Attributes.Add("cName", "cRegularDiet")
            Me.ddlSwallowPill.Attributes.Add("cName", "cSwallowPil")
            Me.txtHeight.Attributes.Add("cName", "nHeight")
            Me.txtWeight.Attributes.Add("cName", "nWeight")
            Me.txtComments.Attributes.Add("cName", "vComments")
            Me.txtAvailableBlood.Attributes.Add("cName", "nBloodAvailable")
            Me.txtWashoutDate.Attributes.Add("cName", "dWashOutDate")
            Me.txtUsedBlood.Attributes.Add("cName", "nBloodUsed")
            Me.txtLastStudy.Attributes.Add("cName", "vLastStudy")
            Me.ddlSubjectStatus.Attributes.Add("cName", "cStatus")
            Me.ddlRecruitingSource.Attributes.Add("cName", "vRecruitingSource")
            Me.txtreferSubject.Attributes.Add("cName", "vReferenceSubject")
            Me.txtRsvp.Attributes.Add("cName", "vRsvpId")
            ' Me.txtStudyhistory.Attributes.Add("cName", "vStudyHistory")
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...AssignAttribute()")
            Return False
        End Try




    End Function

    Private Function ValidateSubject() As Boolean
        Dim Wstr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_valid As DataSet = Nothing
        Try
            Wstr = "vFirstName = '" + Me.txtFirstName.Text.Trim().Replace("'", "''") + "' AND vMiddleName = '" + Me.txtMiddleName.Text.Trim().Replace("'", "''") + "' AND vSurName= '" + Me.txtLastName.Text.Trim().Replace("'", "''") + "' AND cStatusIndi <> 'D'"


            If Not objhelpDb.GetSubjectMaster(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_valid, eStr) Then
                Me.ShowErrorMessage("Error While getting Subject Details", eStr)
                Return False
            End If


            If Not ds_valid Is Nothing Then
                If ds_valid.Tables(0).Rows.Count > 0 Then
                    Return False
                    Exit Try
                End If
            End If
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...ValidateSubject()")
            Return False
        End Try


    End Function

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

