
Partial Class frmSubjectPIFMst
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION"

    Dim ObjCommon As New clsCommon
    Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
    Dim eStr_Retu As String

    Private Const VS_IsEdit As String = "Edit"
    Private Const VS_Choice As String = "Choice"
    Private Const VS_DtSubjectMst As String = "dtSubjectMst"
    Private Const VS_DtSubjectHabitDetails As String = "DtSubjectHabitDetails"
    Private Const VS_DtQC As String = "dtQC"

    'For inhouse mode
    Private InHouse_Mode As String = "11"
    Private Const BAE_Subject As String = "B"
    Private Const InHouse_Subject As String = "I"
    Private Const Location_Canada As String = "0003"
    Private Subject_Type As String = BAE_Subject

    Private Const VS_GVDtSubjectProofDetails As String = "GVdtSubjectProof"
    Private Const VS_SubjectId As String = "SubjectId"

    Private Const VS_TempTransactionNo As String = "TempTransactionNo"

    Private Const GVHC_SubjectHabitDetailsNo As Integer = 0
    Private Const GVHC_SubjectId As Integer = 1
    Private Const GVHC_ScreenId As Integer = 2
    Private Const GVHC_HabitId As Integer = 3
    Private Const GVHC_Habits As Integer = 4
    Private Const GVHC_HaditFlag As Integer = 5
    Private Const GVHC_History As Integer = 6
    Private Const GVHC_Consumtion As Integer = 7
    Private Const GVHC_HabitFlag As Integer = 8

    Private Const GVCQC_SubjectMasterQCNo As Integer = 0
    Private Const GVCQC_SubjectId As Integer = 1
    Private Const GVCQC_SrNo As Integer = 2
    Private Const GVCQC_Subject As Integer = 3
    Private Const GVCQC_QCComment As Integer = 4
    Private Const GVCQC_QCFlag As Integer = 5
    Private Const GVCQC_QCBy As Integer = 6
    Private Const GVCQC_QCDate As Integer = 7
    Private Const GVCQC_Response As Integer = 8
    Private Const GVCQC_ResponseGivenBy As Integer = 9
    Private Const GVCQC_ResponseGivenOn As Integer = 10
    Private Const GVCQC_LnkResponse As Integer = 11

    'nSubjectProofNo,vSubjectId,iTranNo,vProofType,vProofPath,iModifyBy,dModifyOn,cStatusIndi
    Private Const GVCSubProof_nSubjectProofNo As Integer = 0
    Private Const GVCSubProof_vSubjectId As Integer = 1
    Private Const GVCSubProof_iTranNo As Integer = 2
    Private Const GVCSubProof_vProofType As Integer = 3
    Private Const GVCSubProof_vProofPath As Integer = 4
    Private Const GVCSubProof_iModifyBy As Integer = 5
    Private Const GVCSubProof_dModifyOn As Integer = 6
    Private Const GVCSubProof_cStatusIndi As Integer = 7
    Private Const GVCSubProof_Attachment As Integer = 8
    Private Const GVCSubProof_Delete As Integer = 9

#End Region

#Region "Form Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtotherProf.Enabled = False
        If Me.Request.QueryString("mode") = InHouse_Mode Then
            Subject_Type = InHouse_Subject
            Me.btnMSR.Visible = False
        ElseIf Me.Request.QueryString("mode") = 4 Then
            AutoCompleteExtender1.ServiceMethod = "GetSubjectCompletionList_NotRejected_OnlyID"
            AutoCompleteExtender1.ContextKey = Session(S_LocationCode)
        End If
        
        If Not IsPostBack Then
            If Not (Me.Request.QueryString("SearchSubjectId") Is Nothing AndAlso Me.Request.QueryString("SearchSubjectText") Is Nothing AndAlso Me.Request.QueryString("Saved") Is Nothing) Then
                If Not Me.Request.QueryString("Saved") = "True" Then
                    If Me.Request.QueryString("mode") = 4 Then
                        Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View
                    End If
                    btnEdit_Click(sender, e)
                    Exit Sub
                End If
            End If
        End If

        If Not IsPostBack Then
            GenCall()
        End If
    End Sub

#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean
        Dim dt_SubjectMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try

            Me.ViewState(VS_TempTransactionNo) = 0

            rblsex.Items(0).Selected = True
            rblMartialStatus.Items(0).Selected = True
            rdoLstRegular.Items(0).Selected = True
            rblchildrenhealthy.Items(0).Selected = True
            RBLLactating.Items(0).Selected = True
            'rblcontraception.Items(0).Selected = True
            rblPain.SelectedValue = "1"
            rblVolunteer.SelectedValue = "Y"
            Me.rblAbortion.SelectedValue = "N"

            Choice = CType("1", WS_Lambda.DataObjOpenSaveModeEnum)


            If Not IsNothing(Me.Request.QueryString("mode")) AndAlso _
                    Me.Request.QueryString("mode") <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                Choice = Me.Request.QueryString("mode")   'To be used while QC(View)

            End If

            Me.ViewState(VS_Choice) = Choice   'To use it while saving

            ''''Check for Valid User''''''''''''''
            If Not GenCall_Data(Choice, dt_SubjectMst) Then ' For Data Retrieval
                Exit Function
            End If
            Me.ViewState(VS_DtSubjectMst) = dt_SubjectMst ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI(Choice, dt_SubjectMst) Then 'For Displaying Data 
                Exit Function
            End If

            GenCall = True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "......GenCall")
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
        Dim dsSubjectHabitDetail As DataSet = Nothing

        Dim dsSubjectProof As New DataSet
        Try

            wStr = "1=2"

            If Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add AndAlso _
                Not IsNothing(Me.Request.QueryString("SubjectId")) Then

                wStr = "vSubjectId='" + Me.Request.QueryString("SubjectId").ToString.Trim() & "'" 'Value of where condition
                'wStr += " And cRejectionFlag <> 'Y'"

            End If

            If Not objHelp.GetView_SubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubjectMst, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not objHelp.GetSubjectHabitDetails(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                           dsSubjectHabitDetail, eStr_Retu) Then
                ShowErrorMessage(eStr_Retu, "")
                Exit Function
            End If

            Me.ViewState(VS_DtSubjectHabitDetails) = dsSubjectHabitDetail.Tables(0)

            If ds_SubjectMst Is Nothing Then
                Throw New Exception(eStr)
            End If

            dt_Dist_Retu = ds_SubjectMst.Tables(0)

            If Not Me.objHelp.GetSubjectProofDetails("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                            dsSubjectProof, eStr) Then
                Me.ObjCommon.ShowAlert(eStr, Me.Page)
                Return False
            End If
            Me.ViewState(VS_GVDtSubjectProofDetails) = dsSubjectProof.Tables(0)

            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "..GenCall_Data")
        Finally
        End Try
    End Function

#End Region

#Region "SetSubjectValues"

    Private Sub SetSubjectValues(ByVal dtSubject As DataTable, ByVal dtSubjectHabitDetail As DataTable)
        Dim strLocalAdd1 As String = String.Empty
        Dim strLocalAdd2 As String = String.Empty
        Dim strPerAdd As String = String.Empty
        Dim bldGroup As String = String.Empty
        Dim dr As DataRow
        Dim dt As String = String.Empty
        Dim index As Integer = 0
        Dim Len As Integer = 5
        Dim StrICFlang As String()

        If dtSubject.Rows.Count > 0 Then


            dr = dtSubject.Rows(0)
            If Not dr("vWOrkspaceId") Is DBNull.Value Then
                Me.ddlprojectno.SelectedValue = dr("vWorkspaceId").ToString
            End If

            Me.txtfirstname.Text = dr("vFirstName").ToString
            Me.txtmiddlename.Text = dr("vMiddleName").ToString
            Me.txtlastname.Text = dr("vSurName").ToString

            If Not dr("vInitials") Is DBNull.Value Then
                Me.txtInitials.Text = dr("vInitials")
                Me.hfinitials.Value = dr("vInitials")
                'Me.txtInitials.Enabled = False
            End If

            Me.txtdob.Text = ""

            Me.txtInitials.Attributes.Add("disabled", "true")

            If Not dr("dBirthDate") Is DBNull.Value Then
                Me.txtdob.Text = dr("dBirthDate").ToString.Trim()
                Me.txtAge.Text = dr("Age").ToString.Trim()
                Me.hfAge.Value = dr("Age").ToString.Trim()
                Me.txtAge.Enabled = False
            End If

            Me.txtdoer.Text = ""
            If Not dr("dEnrollmentDate") Is DBNull.Value Then
                Me.txtdoer.Text = dr("dEnrollmentDate").ToString.Trim()
            End If

            'This is for Ref Subject Code given by User
            Me.txtRefSubectCode.Text = dr("vRefSubjectId").ToString.Trim()

            'This is for gender
            tbPnlFemaleDetails.Enabled = False
            If dr("cSex").ToString.ToUpper = "M" Then
                rblsex.Items(0).Selected = True
                rblsex.Items(1).Selected = False

            ElseIf dr("cSex").ToString.ToUpper = "F" Then
                rblsex.Items(1).Selected = True
                rblsex.Items(0).Selected = False
                tbPnlFemaleDetails.Enabled = True
            End If

            'rblsexSelectedIndexChanged()

            'This is for Marital Status
            If dr("cMaritalStatus").ToString.ToUpper = "S" Then
                rblMartialStatus.Items(0).Selected = True
                rblMartialStatus.Items(1).Selected = False
            ElseIf dr("cMaritalStatus").ToString.ToUpper = "M" Then
                rblMartialStatus.Items(1).Selected = True
                rblMartialStatus.Items(0).Selected = False
            End If

            'This is for Food Habit
            If dr("cFoodHabit").ToString = "Vegetarian" Then
                rdoLstFoodHabit.Items(0).Selected = True
            ElseIf dr("cFoodHabit").ToString = "Non-Vegetarian" Then
                rdoLstFoodHabit.Items(1).Selected = True
            ElseIf dr("cFoodHabit").ToString = "Eggetarian" Then
                rdoLstFoodHabit.Items(2).Selected = True
            End If

            'This is for Blood Group
            bldGroup = dr("cbloodgroup").ToString.Trim
            Me.ddlRh.SelectedValue = dr("cRh").ToString.Trim
            ddlbloodgroup.SelectedValue = bldGroup

            'This is for Education Qualification
            txteducationquali.Text = dr("vEducationQualification").ToString

            'This is for ICF Language
            'ddlicflanguage.SelectedValue = dr("vICFLanguageCodeId").ToString.Trim

            'done on 20-Dec-2010

            StrICFlang = dr("vICFLanguageCodeId").ToString.Split(",")

            'Me.chklstIcfLanguage.ClearSelection()

            If chklstIcfLanguage.Items.Count = 0 Then
                fillchklstICFLanguage()
            End If

            For indexStr As Integer = 0 To StrICFlang.Length - 1
                For Each lstItem As ListItem In chklstIcfLanguage.Items
                    If lstItem.Value = StrICFlang(indexStr).ToString.Trim Then
                        lstItem.Selected = True
                    End If
                Next
            Next indexStr
            '=======
            'This for Occupation
            txtoccupation.Text = dr("vOccupation").ToString.Trim

            'This is for Proof of Age
            ''added By Dharmesh Salla
            For CntOfProof As Integer = 0 To cblProofOfAge.Items.Count - 1
                cblProofOfAge.Items(CntOfProof).Selected = False
            Next

            '''' ************* ''


            For Each item As ListItem In cblProofOfAge.Items
                If item.Text.ToUpper.Trim() = Convert.ToString(dr("vProofOfAge1")).ToUpper.Trim() Or item.Text.ToUpper.Trim() = Convert.ToString(dr("vProofOfAge2")).ToUpper.Trim() Or _
                    item.Text.ToUpper.Trim() = Convert.ToString(dr("vProofOfAge3")).ToUpper.Trim() Then

                    item.Selected = True

                ElseIf IIf(dr("vProofOfAge1").ToString.ToUpper.Contains("OTHERS:"), "OTHERS", dr("vProofOfAge1").ToString) = item.Text.ToUpper Or _
                IIf(dr("vProofOfAge2").ToString.ToUpper.Contains("OTHERS:"), "OTHERS", dr("vProofOfAge2").ToString) = item.Text.ToUpper Or _
                IIf(dr("vProofOfAge3").ToString.ToUpper.Contains("OTHERS:"), "OTHERS", dr("vProofOfAge3").ToString) = item.Text.ToUpper Then

                    item.Selected = True
                    'Added on 1-Dec-2008
                    If item.Text.ToUpper = "OTHERS" Then
                        Me.txtotherProf.Enabled = True
                        If dr("vProofOfAge1").ToString.ToUpper.Contains("OTHERS:") Then
                            Me.txtotherProf.Text = dr("vProofOfAge1").ToString.ToUpper.Substring(dr("vProofOfAge1").ToString.ToUpper.LastIndexOf("OTHERS:") + 7)

                        ElseIf dr("vProofOfAge2").ToString.ToUpper.Contains("OTHERS:") Then
                            Me.txtotherProf.Text = dr("vProofOfAge2").ToString.ToUpper.Substring(dr("vProofOfAge2").ToString.ToUpper.LastIndexOf("OTHERS:") + 7)
                        ElseIf dr("vProofOfAge3").ToString.ToUpper.Contains("OTHERS:") Then
                            Me.txtotherProf.Text = dr("vProofOfAge3").ToString.ToUpper.Substring(dr("vProofOfAge3").ToString.ToUpper.LastIndexOf("OTHERS:") + 7)
                        End If
                    End If
                    '***********************************
                End If
            Next

            'This is for Height
            txtheight.Text = dr("nHeight").ToString

            'This is for Weight
            txtweight.Text = dr("nWeight").ToString

            'This is for BMI
            If Not dr("nBMI") Is DBNull.Value Then
                Me.txtbmi.Text = dr("nBMI").ToString
                Me.txtbmi.Enabled = False
                Me.hfbmi.Value = dr("nBMI").ToString

            End If

            'This is for Habit Details on 13-Dec-2008
            Me.GVHabits.DataSource = dtSubjectHabitDetail
            Me.GVHabits.DataBind()
            '********************

            'This is for Local Address 1
            If (dr("vLocalAdd1").ToString <> "") Then
                strLocalAdd1 = dr("vLocalAdd1").ToString
            End If
            If (dr("vLocalAdd12").ToString <> "") Then
                strLocalAdd1 = strLocalAdd1 + "," + dr("vLocalAdd12").ToString
            End If
            If (dr("vLocalAdd13").ToString <> "") Then
                strLocalAdd1 = strLocalAdd1 + "," + dr("vLocalAdd13").ToString
            End If

            'This is for Local Address 2
            If (dr("vLocalAdd21").ToString <> "") Then
                strLocalAdd2 = dr("vLocalAdd21").ToString
            End If
            If (dr("vLocalAdd22").ToString <> "") Then
                strLocalAdd2 = strLocalAdd2 + "," + dr("vLocalAdd22").ToString
            End If
            If (dr("vLocalAdd23").ToString <> "") Then
                strLocalAdd2 = strLocalAdd2 + "," + dr("vLocalAdd23").ToString
            End If

            'This is for Per Add
            If (dr("vPerAdd1").ToString <> "") Then
                strPerAdd = dr("vPerAdd1").ToString
            End If
            If (dr("vPerAdd2").ToString <> "") Then
                strPerAdd = strPerAdd + "," + dr("vPerAdd2").ToString
            End If
            If (dr("vPerAdd3").ToString <> "") Then
                strPerAdd = strPerAdd + "," + dr("vPerAdd3").ToString
            End If


            Me.txtlocaladds1.Text = strLocalAdd1.ToString.Trim
            Me.txtlocaladd2.Text = strLocalAdd2.ToString.Trim
            Me.txtLocaltel1no.Text = dr("vLocalTelephoneno1").ToString
            Me.txtLocaltel2no.Text = dr("vLocalTelephoneno2").ToString
            Me.txtPermanentAdds.Text = strPerAdd.ToString.Trim
            Me.txtPlace.Text = dr("vPerCity").ToString.Trim()
            Me.txtpertelno.Text = dr("vPerTelephoneno").ToString
            Me.txtOfficeAddress.Text = dr("vOfficeAddress").ToString
            Me.txtOfficetelno.Text = dr("vOfficeTelephoneno").ToString

            Me.txtconper.Text = dr("vContactName1").ToString
            Me.txtContactName2.Text = dr("vContactName2").ToString
            Me.txtconperadds1.Text = dr("vContactAddress11").ToString
            Me.txtconperadds2.Text = dr("vContactAddress21").ToString
            Me.txtconpertel1.Text = dr("vContactTelephoneno1").ToString
            Me.txtconpertel2.Text = dr("vContactTelephoneno2").ToString
            Me.txtRefBy.Text = dr("vReferredBy").ToString

            'Added for audittrail on 24-Aug-2009
            Me.lblLastRemark.Text = ""
            Me.lblLastRemark.Text = "Last Remarks:" + dr("vRemark").ToString
            '******************************************

            If Not dr("nSubjectFemaleDetailNo") Is DBNull.Value Then

                Me.tbConPIFMst.Tabs(1).Enabled = True
                Me.txtmendate.Text = IIf(dr("dLastMenstrualDate") Is DBNull.Value, "", dr("dLastMenstrualDate"))
                Me.txtcyclelength.Text = IIf(dr("iLastMenstrualDays") Is DBNull.Value, "", dr("iLastMenstrualDays"))

                If dr("cRegular").ToString = "Y" Then
                    rdoLstRegular.Items(0).Selected = True
                ElseIf dr("cRegular").ToString = "N" Then
                    rdoLstRegular.Items(1).Selected = True
                End If

                Me.txtdold.Text = IIf(dr("dLastDelivaryDate") Is DBNull.Value, "", dr("dLastDelivaryDate"))

                Me.txtgravida.Text = IIf(dr("vGravida") Is DBNull.Value, "", dr("vGravida").ToString)
                Me.txtnoofchildren.Text = IIf(dr("iNoOfChildren") Is DBNull.Value, "", dr("iNoOfChildren").ToString)

                If Not dr("cAbortions") Is DBNull.Value AndAlso dr("cAbortions").ToString.Trim <> "" Then
                    Me.rblAbortion.SelectedValue = dr("cAbortions").ToString
                End If

                txtdolabortion.Text = IIf(dr("dAbortionDate") Is DBNull.Value, "", dr("dAbortionDate"))

                Me.txtpara.Text = IIf(dr("vPara") Is DBNull.Value, "", dr("vPara").ToString)

                Me.RBLLactating.SelectedValue = "N"
                If Not dr("cLoctating") Is DBNull.Value AndAlso dr("cLoctating").ToString.Trim <> "" Then
                    Me.RBLLactating.SelectedValue = dr("cLoctating").ToString
                End If

                If Not dr("cContraception") Is DBNull.Value AndAlso dr("cContraception").ToString.Trim <> "" Then
                    If dr("cContraception").ToString = "P" Then
                        rblcontraception.Items(0).Selected = True
                    ElseIf dr("cContraception").ToString = "T" Then
                        rblcontraception.Items(1).Selected = True
                    End If
                End If

                If Not dr("cLastMenstrualIndi") Is DBNull.Value AndAlso dr("cLastMenstrualIndi").ToString.Trim <> "" Then
                    Me.rblPain.SelectedValue = dr("cLastMenstrualIndi")
                End If

                If Not dr("cIsVolunteerinBearingAge") Is DBNull.Value AndAlso dr("cIsVolunteerinBearingAge").ToString.Trim <> "" Then

                    Me.rblVolunteer.SelectedValue = dr("cIsVolunteerinBearingAge")
                End If

                If Not dr("cChildrenHelath") Is DBNull.Value AndAlso dr("cChildrenHelath").ToString.Trim <> "" Then

                    Me.rblchildrenhealthy.SelectedValue = dr("cChildrenHelath")
                End If

                Me.txtChildrenRemark.Text = IIf(dr("vchildrenHealthRemark") Is DBNull.Value, "", dr("vchildrenHealthRemark").ToString)
                Me.txtNoChildernDied.Text = IIf(dr("iNoOfChildrenDied") Is DBNull.Value, "", dr("iNoOfChildrenDied").ToString)
                Me.txtRemarkChDied.Text = IIf(dr("vRemarkifDied") Is DBNull.Value, "", dr("vRemarkifDied").ToString)

                For index = 0 To chkContraception.Items.Count - 1

                    If chkContraception.Items(index).Value = "B" AndAlso Not dr("cBarrier") Is DBNull.Value AndAlso dr("cBarrier").ToString.ToUpper.Trim = "C" Then
                        chkContraception.Items(index).Selected = True
                    ElseIf chkContraception.Items(index).Value = "P" AndAlso Not dr("cPills") Is DBNull.Value AndAlso dr("cPills").ToString.ToUpper.Trim = "C" Then
                        chkContraception.Items(index).Selected = True
                    ElseIf chkContraception.Items(index).Value = "R" AndAlso Not dr("cRhythm") Is DBNull.Value AndAlso dr("cRhythm").ToString.ToUpper.Trim = "C" Then
                        chkContraception.Items(index).Selected = True
                    ElseIf chkContraception.Items(index).Value = "I" AndAlso Not dr("cIUCD") Is DBNull.Value AndAlso dr("cIUCD").ToString.ToUpper.Trim = "C" Then
                        chkContraception.Items(index).Selected = True
                    End If

                Next index

                Me.txtOtherRemarks.Text = IIf(dr("vOtherRemark") Is DBNull.Value, "", dr("vOtherRemark").ToString)

            End If
        End If

        Me.tbConPIFMst.ActiveTabIndex = 0

    End Sub

#End Region

#Region "HideMenu"

    Private Sub HideMenu()
        Dim tmpMenu As Menu
        Dim tmpddlprofile As DropDownList

        tmpMenu = CType(Me.Master.FindControl("menu1"), Menu)
        tmpddlprofile = CType(Me.Master.FindControl("ddlProfile"), DropDownList)

        If Not tmpMenu Is Nothing Then
            tmpMenu.Visible = False
        End If

        If Not tmpddlprofile Is Nothing Then
            tmpddlprofile.Enabled = False
        End If

    End Sub

#End Region

#Region "GenCall_ShowUI"

    Private Function GenCall_ShowUI(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                                      ByVal dt_Dist As DataTable) As Boolean

        Dim choice As WS_HelpDbTable.DataObjOpenSaveModeEnum
        Dim dt_SubjMst As DataTable = Nothing
        Dim dsUser As New DataSet
        Dim wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim sender As New Object
        Dim e As New EventArgs
        Try

            Page.Title = ":: Personal Information Form  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")

            If Not IsNothing(Me.Request.QueryString("LastSubjectId")) AndAlso Me.Request.QueryString("LastSubjectId").Trim() <> "" Then

                ObjCommon.ShowAlert("Subject Saved Successfully with SubjectId = " & Me.Request.QueryString("LastSubjectId").Trim() & ".", Me)
                btnsave.Enabled = True

            End If

            If Not IsNothing(Me.Request.QueryString("SearchSubjectId")) AndAlso Me.Request.QueryString("SearchSubjectId").Trim() <> "" Then

                ObjCommon.ShowAlert("Subject Saved Successfully with SubjectId = " & Me.Request.QueryString("SearchSubjectId").Trim() & ".", Me)
                btnsave.Enabled = True

            End If

            'Added for compulsory add remark while Edit else not compulsory on 24-Aug-2009
            Me.btnsave.OnClientClick = "return Validation('ADD');"
            '**************************************

            CType(Master.FindControl("lblHeading"), Label).Text = "Subject Master"

            If Not Filldropdown() Then
                Return False
            End If

            'Contex key and webmethod call add according to mode(inhouse or babe) :start
            Me.AutoCompleteExtender1.ContextKey = Session(S_LocationCode)
            Me.AutoCompleteExtender1.ServiceMethod = "GetSubjectCompletionList_NotRejected"
            If Me.Request.QueryString("mode") = InHouse_Mode Then
                Me.AutoCompleteExtender1.ContextKey = "View_subjectMaster#" + " vLocationCode='" & Session(S_LocationCode) & "'"
                Me.AutoCompleteExtender1.ServiceMethod = "GetSubjectCompletionList_NotRejected_InHouse"
            ElseIf Me.Request.QueryString("mode") = 4 Then
                AutoCompleteExtender1.ServiceMethod = "GetSubjectCompletionList_NotRejected_OnlyID"
                'AutoCompleteExtender1.OnClientShowing = "ClientPopulated1"
            End If

            Me.txtweight.Attributes.Add("onblur", "FillBMIValue('" & Me.txtheight.ClientID & "','" & Me.txtweight.ClientID & "','" & Me.txtbmi.ClientID & "');")
            Me.txtmiddlename.Attributes.Add("onblur", "SetInitial('" & Me.txtInitials.ClientID & "');")
            Me.txtlastname.Attributes.Add("onblur", "SetInitial('" & Me.txtInitials.ClientID & "');")
            Me.txtfirstname.Attributes.Add("onblur", "SetInitial('" & Me.txtInitials.ClientID & "');")
            Me.txtInitials.Attributes.Add("disabled", "true")

            Me.txtQCRemarks.Attributes.Add("onkeydown", "ValidateRemarks(this,'" & Me.lblcnt.ClientID & "',1000);")

            'on 3-Jan-2009******************

            Me.txtdoer.Text = Format(System.DateTime.Now, "dd-MMM-yyyy")
            '==added on 12-Jan-2010 by deepak to show btnqc only qc on subject is done==
            Me.btnQC.Visible = False
            '===========
            'Added for QC comments on 22-May-2009
            Me.BtnNew.Visible = True

            Me.BtnQCSaveSend.Visible = False

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                'Me.tbConPIFMst.Enabled = False
                Me.BtnNew.Visible = False
                Me.BtnQCSaveSend.Visible = True

                'added on 04-May-10 by Megha
                txtSubject.Text = Me.Request.QueryString("SubjectId")
                HSubjectId.Value = Me.Request.QueryString("SubjectId")
                btnEdit_Click(sender, e)
            End If
            '************************************
            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                If (Not Me.Request.QueryString("WorkSpaceId") Is Nothing) AndAlso (Not Me.Request.QueryString("Type") Is Nothing) Then
                    Me.tdSubject.Style.Add("display", "none")
                    Me.BtnNew.Style.Add("display", "none")
                    Me.btnEdit.Style.Add("display", "none")
                    Me.txtSubject.Style.Add("display", "none")
                    Me.btnQC.Style.Add("display", "none")
                    Me.btnMSR.Style.Add("display", "none")
                    Me.btnExit.Style.Add("display", "none")
                    HideMenu()
                End If
            End If
            fillHabitGrid()

            If Not fillSubjectProofGrid() Then
                Return False
            End If

            'Added on 07-July-2009 with repect to "frmSubjectEnrollment"
            choice = Me.ViewState(VS_Choice)
            If choice = WS_HelpDbTable.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                If Not IsNothing(Me.Request.QueryString("WorkspaceId")) AndAlso Me.Request.QueryString("WorkspaceId").Trim() <> "" Then
                    Me.ddlprojectno.SelectedValue = Me.Request.QueryString("WorkspaceId").Trim()
                End If
            End If
            '==added on 13-jan-2010
            'Me.Image1.ImageUrl = "~/Images/demo.jpg"
            '***********************************************************
            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_ShowUI")
            GenCall_ShowUI = False
        End Try

    End Function

#End Region

#Region "fillHabitGrid"

    Private Function fillHabitGrid() As Boolean
        Dim DtHabitDetail As New DataTable
        Dim dsHabitMst As New DataSet
        Dim dr As DataRow
        Dim drMst As DataRow
        Dim estr As String = String.Empty
        Try
            DtHabitDetail = CType(Me.ViewState(VS_DtSubjectHabitDetails), DataTable)

            If Not Me.objHelp.GetSubjectHabitMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, dsHabitMst, estr) Then
                Me.ObjCommon.ShowAlert(estr, Me.Page)
                Return False
            End If

            For Each drMst In dsHabitMst.Tables(0).Rows
                dr = DtHabitDetail.NewRow()
                dr("vHabitId") = drMst("vHabitId")
                dr("vHabitDetails") = drMst("vHabitDetails")
                dr("cHabitFlag") = "N"
                DtHabitDetail.Rows.Add(dr)
                DtHabitDetail.AcceptChanges()
            Next
            Me.GVHabits.DataSource = DtHabitDetail
            Me.GVHabits.DataBind()

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...fillHabitGrid")
            Return False
        End Try

    End Function

#End Region

#Region "fill Dropdown"

    Private Function Filldropdown() As Boolean
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim ds_subjtypemst As New DataSet
        Dim ds_workspacemst As New DataSet
        Try

            objHelp.FillDropDown("SubjectLanguageMst", "vLanguageId", "vLanguageName", "cActiveFlag<>'N' and cStatusIndi<>'D'", ds_subjtypemst, eStr)
            'Me.ddlicflanguage.DataSource = ds_subjtypemst.Tables(0)
            'Me.ddlicflanguage.DataValueField = "vLanguageId"
            'Me.ddlicflanguage.DataTextField = "vLanguageName"
            'Me.ddlicflanguage.DataBind()

            fillchklstICFLanguage()

            'To Get Where condition of ScopeVales( Project Type )
            If Not ObjCommon.GetScopeValueWithCondition(wStr) Then
                Return False
            End If

            objHelp.FillDropDown("View_getWorkspaceDetailForHdr", "vWorkSpaceId", "vProjectNo", wStr, ds_workspacemst, eStr)
            Me.ddlprojectno.DataSource = ds_workspacemst.Tables(0)
            Me.ddlprojectno.DataValueField = "vWorkSpaceId"
            Me.ddlprojectno.DataTextField = "vProjectNo"
            Me.ddlprojectno.DataBind()
            Me.ddlprojectno.Items.Insert(0, "  ---SELECT PROJECT---  ")



            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "..fillHabitGrid")
            Return False
        End Try

    End Function

    Private Sub fillchklstICFLanguage(Optional ByVal All As Boolean = False)

        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim ds_LanguageMst As New DataSet
        Try

            Wstr = "cActiveFlag<>'N' and cStatusIndi<>'D'"


            If Not Me.objHelp.getSubjectLanguageMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_LanguageMst, estr) Then
                Me.ObjCommon.ShowAlert("Error While Getting SubjectLanguageMst", Me.Page)
                Exit Sub
            End If

            If ds_LanguageMst.Tables(0).Rows.Count < 1 Then

            End If

            Me.chklstIcfLanguage.DataSource = ds_LanguageMst.Tables(0).DefaultView.ToTable(True, "vLanguageId,vLanguageName".Split(","))
            Me.chklstIcfLanguage.DataValueField = "vLanguageId"
            Me.chklstIcfLanguage.DataTextField = "vLanguageName"
            Me.chklstIcfLanguage.DataBind()

            'For Each lstItem As ListItem In chklstIcfLanguage.Items

            '    lstItem.Attributes.Add("onclick", "SetCheckUncheckAll(document.getElementById('" + _
            '                                    Me.chklstIcfLanguage.ClientID + "'), document.getElementById('" + _
            '                                    Me.chkSelectAl.ClientID + "'));")

            'Next lstItem

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...fillchklstICFLanguage")
        End Try
    End Sub

#End Region

#Region "Assign Values"

    Private Function AssignValues() As Boolean
        Dim dsEdit As New DataSet
        Dim dsSubjectHabitDetail_Edit As New DataSet
        Dim dtSubjectHabitDetail As New DataTable
        Dim dtEdit As New DataTable
        Dim dtOld As New DataTable
        Dim dr As DataRow
        Dim drHabit As DataRow
        Dim strarr(2) As String
        Dim StrAddress1 As String = String.Empty
        Dim StrAdd11 As String = String.Empty
        Dim StrAdd12 As String = String.Empty
        Dim StrAdd13 As String = String.Empty
        Dim StrAddress2 As String = String.Empty
        Dim StrAdd21 As String = String.Empty
        Dim StrAdd22 As String = String.Empty
        Dim StrAdd23 As String = String.Empty
        Dim StrPerAdd As String = String.Empty
        Dim StrAdd1 As String = String.Empty
        Dim StrAdd2 As String = String.Empty
        Dim StrAdd3 As String = String.Empty
        Dim index As Integer = 0
        Dim StrLanguagecode As String = String.Empty

        Try


            dtSubjectHabitDetail = CType(Me.ViewState(VS_DtSubjectHabitDetails), DataTable)

            If ViewState(VS_IsEdit) = True Then 'This is for Editing selected subject
                dtEdit = CType(Me.ViewState(VS_DtSubjectMst), DataTable)

                If dtEdit.Rows.Count < 1 Then
                    Return False
                End If

                dr = dtEdit.Rows(0)

                If ddlprojectno.SelectedIndex > 0 Then
                    dr("vWorkspaceId") = ddlprojectno.SelectedValue
                End If

                dr("dEnrollmentDate") = IIf(String.IsNullOrEmpty(Me.txtdoer.Text.Trim), System.DBNull.Value, Me.txtdoer.Text.Trim)
                dr("vFirstName") = Me.txtfirstname.Text.Trim
                dr("vSurName") = Me.txtlastname.Text.Trim
                dr("vMiddleName") = Me.txtmiddlename.Text.Trim
                dr("vInitials") = Me.hfinitials.Value.Trim
                dr("dBirthDate") = IIf(String.IsNullOrEmpty(Me.txtdob.Text.Trim), System.DBNull.Value, Me.txtdob.Text.Trim)
                ' For inhouse mode :Start
                dr("cSubjectType") = Subject_Type
                ' For inhouse mode :End
                ' dr("vICFLanguageCodeId") = Me.ddlicflanguage.SelectedValue.Trim

                'done on 20-Dec-2010
                For Each lstItem As ListItem In chklstIcfLanguage.Items
                    If lstItem.Selected = True Then

                        If StrLanguagecode <> "" Then
                            StrLanguagecode = StrLanguagecode + "," + lstItem.Value
                        Else
                            StrLanguagecode = lstItem.Value
                        End If
                    End If
                Next
                'StrLanguagecode = StrLanguagecode.Substring(0, StrLanguagecode.LastIndexOf(","))
                '===================
                dr("vICFLanguageCodeId") = StrLanguagecode.Trim

                If rblsex.SelectedItem.Value = "M" Then
                    dr("cSex") = "Male"
                ElseIf rblsex.SelectedItem.Value = "F" Then
                    dr("cSex") = "Female"
                End If

                'On 1-Jan-2008
                index = 0
                For Each item As ListItem In cblProofOfAge.Items
                    If item.Selected AndAlso index < 3 Then
                        strarr(index) = item.Text
                        If item.Text.ToUpper = "OTHERS" Then
                            strarr(index) = item.Text + ":" + Me.txtotherProf.Text.Trim()
                        End If
                        index += 1
                    End If
                Next item
                '**************************

                dr("vProofOfAge1") = strarr(0)
                dr("vProofOfAge2") = strarr(1)
                dr("vProofOfAge3") = strarr(2)
                If strarr.Length >= 4 Then
                    ObjCommon.ShowAlert("Select Maximum 3 Proof ", Me)
                    Return False
                End If
                dr("cBloodGroup") = Me.ddlbloodgroup.SelectedValue.Trim
                dr("cRh") = Me.ddlRh.SelectedValue.Trim
                dr("vEducationQualification") = Me.txteducationquali.Text.Trim
                dr("vOccupation") = Me.txtoccupation.Text.Trim
                dr("vRefSubjectId") = Me.txtRefSubectCode.Text.Trim()
                dr("iModifyBy") = Me.Session(S_UserID)

                'subjectdetailtable
                dr("nHeight") = Me.txtheight.Text.Trim
                dr("nWeight") = Me.txtweight.Text.Trim
                dr("nBMI") = Me.hfbmi.Value.Trim

                If rblMartialStatus.SelectedItem.Value = "S" Then
                    dr("cMaritalStatus") = "Single"
                ElseIf rblMartialStatus.SelectedItem.Value = "M" Then
                    dr("cMaritalStatus") = "Married"
                End If

                If rdoLstFoodHabit.SelectedItem.Text = "Vegetarian" Then
                    dr("cFoodHabit") = "Vegetarian"
                ElseIf rdoLstFoodHabit.SelectedItem.Text = "Non-Vegetarian" Then
                    dr("cFoodHabit") = "Non-Vegetarian"
                ElseIf rdoLstFoodHabit.SelectedItem.Text = "Eggetarian" Then
                    dr("cFoodHabit") = "Eggetarian"
                End If

                'subjectcontactdetail

                'This For Local Address1
                StrAddress1 = Me.txtlocaladds1.Text.Trim
                Address(StrAddress1, StrAdd11, StrAdd12, StrAdd13)
                dr("vLocalAdd1") = Convert.ToString(StrAdd11)
                dr("vLocalAdd12") = Convert.ToString(StrAdd12)
                dr("vLocalAdd13") = Convert.ToString(StrAdd13)

                'This For Local Address2
                StrAddress2 = Me.txtlocaladd2.Text.Trim
                Address(StrAddress2, StrAdd21, StrAdd22, StrAdd23)
                dr("vLocalAdd21") = Convert.ToString(StrAdd21)
                dr("vLocalAdd22") = Convert.ToString(StrAdd22)
                dr("vLocalAdd23") = Convert.ToString(StrAdd23)

                'This For Local Permanent Address
                StrPerAdd = Me.txtPermanentAdds.Text.Trim
                Address(StrPerAdd, StrAdd1, StrAdd2, StrAdd3)
                dr("vPerAdd1") = Convert.ToString(StrAdd1)
                dr("vPerAdd2") = Convert.ToString(StrAdd2)
                dr("vPerAdd3") = Convert.ToString(StrAdd3)

                'Added on 28-Jul-2009
                dr("vPerCity") = Me.txtPlace.Text.Trim()
                '****************************************

                dr("vLocalTelephoneno1") = Me.txtLocaltel1no.Text.Trim
                dr("vLocalTelephoneno2") = Me.txtLocaltel2no.Text.Trim
                dr("vPerTelephoneno") = Me.txtpertelno.Text.Trim
                dr("vOfficeAddress") = Me.txtOfficeAddress.Text.Trim
                dr("vOfficeTelephoneno") = Me.txtOfficetelno.Text.Trim
                dr("vContactName1") = Me.txtconper.Text.Trim
                dr("vContactName2") = Me.txtContactName2.Text.Trim()
                dr("vContactAddress11") = Me.txtconperadds1.Text.Trim
                dr("vContactAddress21") = Me.txtconperadds2.Text.Trim
                dr("vContactTelephoneno1") = Me.txtconpertel1.Text.Trim
                dr("vContactTelephoneno2") = Me.txtconpertel2.Text.Trim
                dr("vReferredBy") = Me.txtRefBy.Text.Trim

                'subjectallocationtable
                dr("vWorkSpaceId") = IIf(ddlprojectno.SelectedIndex = 0, System.DBNull.Value, Me.ddlprojectno.SelectedValue.Trim)
                dr("dAllocationDate") = IIf(String.IsNullOrEmpty(Me.txtdoer.Text.Trim), System.DBNull.Value, Me.txtdoer.Text.Trim)

                'Added for audittrail on 24-Aug-2009
                dr("vRemark") = Me.txtRemark.Text.Trim()
                '***************************************************

                'This is for SubjectFemaleDetails

                If tbConPIFMst.Tabs(1).Enabled Then
                    dr("dLastMenstrualDate") = IIf(String.IsNullOrEmpty(Me.txtmendate.Text.Trim), System.DBNull.Value, Me.txtmendate.Text.Trim)
                    dr("dLastDelivaryDate") = IIf(String.IsNullOrEmpty(Me.txtdold.Text.Trim), System.DBNull.Value, Me.txtdold.Text.Trim)
                    dr("cRegular") = Me.rdoLstRegular.SelectedValue
                    dr("iLastMenstrualDays") = IIf(Me.txtcyclelength.Text.Trim() = "", 0, Me.txtcyclelength.Text.Trim())
                    dr("vGravida") = Me.txtgravida.Text
                    dr("iNoOfChildren") = IIf(Me.txtnoofchildren.Text.Trim() = "", 0, Me.txtnoofchildren.Text.Trim())

                    If Me.rblAbortion.SelectedIndex > -1 Then
                        dr("cAbortions") = Me.rblAbortion.SelectedValue.Trim()
                    End If

                    dr("dAbortionDate") = IIf(String.IsNullOrEmpty(Me.txtdolabortion.Text.Trim), System.DBNull.Value, Me.txtdolabortion.Text.Trim)
                    dr("vPara") = Me.txtpara.Text
                    dr("cLoctating") = Me.RBLLactating.SelectedValue.ToUpper.Trim() 'Me.txtLactating.Text

                    If rblPain.SelectedIndex > -1 Then
                        dr("cLastMenstrualIndi") = Me.rblPain.SelectedValue.Trim()
                    End If

                    If rblVolunteer.SelectedIndex > -1 Then
                        dr("cIsVolunteerinBearingAge") = Me.rblVolunteer.SelectedValue.Trim()
                    End If

                    If rblchildrenhealthy.SelectedIndex > -1 Then
                        dr("cChildrenHelath") = rblchildrenhealthy.SelectedItem.Value.Trim()
                    End If

                    dr("vchildrenHealthRemark") = Me.txtChildrenRemark.Text.Trim()
                    dr("iNoOfChildrenDied") = IIf(Me.txtNoChildernDied.Text.Trim() = "", 0, Me.txtNoChildernDied.Text.Trim())
                    dr("vRemarkifDied") = Me.txtRemarkChDied.Text.Trim()

                    If rblcontraception.SelectedIndex > -1 Then
                        dr("cContraception") = rblcontraception.SelectedValue.Trim()
                    End If

                    dr("cBarrier") = "N"
                    dr("cPills") = "N"
                    dr("cRhythm") = "N"
                    dr("cIUCD") = "N"

                    For index = 0 To chkContraception.Items.Count - 1

                        If chkContraception.Items(index).Selected = True Then

                            If chkContraception.Items(index).Value.ToUpper.Trim() = "B" Then
                                dr("cBarrier") = "C"
                            ElseIf chkContraception.Items(index).Value.ToUpper.Trim() = "P" Then
                                dr("cPills") = "C"
                            ElseIf chkContraception.Items(index).Value.ToUpper.Trim() = "R" Then
                                dr("cRhythm") = "C"
                            ElseIf chkContraception.Items(index).Value.ToUpper.Trim() = "I" Then
                                dr("cIUCD") = "C"
                            End If

                        End If

                    Next index

                    dr("vOtherRemark") = Me.txtOtherRemarks.Text.Trim()

                End If

                dtEdit.AcceptChanges()
                dtEdit.TableName = "View_SubjectMaster"
                Me.ViewState(VS_DtSubjectMst) = dtEdit

                'For Subject Habit Detail on 13-Dec-2008
                For index = 0 To Me.GVHabits.Rows.Count - 1

                    For Each drHabit In dtSubjectHabitDetail.Rows

                        If drHabit("nSubjectHabitDetailsNo") = Me.GVHabits.Rows(index).Cells(GVHC_SubjectHabitDetailsNo).Text.Trim Then
                            drHabit("vHabitDetails") = Me.GVHabits.Rows(index).Cells(GVHC_Habits).Text.Trim
                            drHabit("cHabitFlag") = CType(Me.GVHabits.Rows(index).FindControl("ddlHebitType"), DropDownList).SelectedValue.Trim
                            drHabit("vHabitHistory") = IIf(CType(Me.GVHabits.Rows(index).FindControl("txtEndDate"), TextBox).Text.Trim = "", _
                                            System.DBNull.Value, CType(Me.GVHabits.Rows(index).FindControl("txtEndDate"), TextBox).Text.Trim)
                            drHabit("vRemarks") = CType(Me.GVHabits.Rows(index).FindControl("txtConsumption"), TextBox).Text.Trim
                            drHabit.AcceptChanges()
                            Exit For
                        End If
                    Next

                Next index

                dtSubjectHabitDetail.TableName = "SubjectHabitDetails"
                Me.ViewState(VS_DtSubjectHabitDetails) = dtSubjectHabitDetail
                ''''''''''''''''''''''''''''''''''''''

            Else 'This is for inserting new Subject

                dtOld = Me.ViewState(VS_DtSubjectMst)
                dtOld.Clear()
                dr = dtOld.NewRow
                dr("vLocationCode") = Me.Session(S_LocationCode)
                dr("vSubjectID") = Pro_Screening
                dr("nSubjectDetailNo") = 1

                dr("dEnrollmentDate") = IIf(String.IsNullOrEmpty(Me.txtdoer.Text.Trim), System.DBNull.Value, Me.txtdoer.Text.Trim)
                dr("vFirstName") = Me.txtfirstname.Text.Trim
                dr("vSurName") = Me.txtlastname.Text.Trim
                dr("vMiddleName") = Me.txtmiddlename.Text.Trim
                dr("vInitials") = Me.hfinitials.Value.Trim
                dr("dBirthDate") = IIf(String.IsNullOrEmpty(Me.txtdob.Text.Trim), System.DBNull.Value, Me.txtdob.Text.Trim)
                ' For inhouse mode :Start
                dr("cSubjectType") = Subject_Type
                ' For inhouse mode :End
                'dr("vICFLanguageCodeId") = Me.ddlicflanguage.SelectedValue.Trim

                'done on 20-Dec-2010
                For Each lstItem As ListItem In chklstIcfLanguage.Items
                    If lstItem.Selected = True Then

                        If StrLanguagecode <> "" Then
                            StrLanguagecode = StrLanguagecode + "," + lstItem.Value
                        Else
                            StrLanguagecode = lstItem.Value
                        End If
                    End If
                Next
                'StrLanguagecode = StrLanguagecode.Substring(0, StrLanguagecode.LastIndexOf(","))
                '===================
                dr("vICFLanguageCodeId") = StrLanguagecode.Trim

                If rblsex.SelectedItem.Value = "M" Then
                    dr("cSex") = "Male"
                ElseIf rblsex.SelectedItem.Value = "F" Then
                    dr("cSex") = "Female"
                End If

                index = 0

                'On 1-Jan-2008
                For Each item As ListItem In cblProofOfAge.Items

                    If item.Selected AndAlso index < 3 Then
                        strarr(index) = item.Text
                        If item.Text.ToUpper = "OTHERS" Then
                            strarr(index) = item.Text + ":" + Me.txtotherProf.Text.Trim()
                        End If
                        index += 1
                    End If

                Next item
                '**************************

                dr("vProofOfAge1") = strarr(0)
                dr("vProofOfAge2") = strarr(1)
                dr("vProofOfAge3") = strarr(2)
                If strarr.Length > 3 Then
                    ObjCommon.ShowAlert("Select Maximum 3 Proof ", Me)
                End If
                dr("cBloodGroup") = Me.ddlbloodgroup.SelectedValue.Trim
                dr("cRh") = Me.ddlRh.SelectedValue.Trim
                dr("vEducationQualification") = Me.txteducationquali.Text.Trim
                dr("vOccupation") = Me.txtoccupation.Text.Trim
                dr("vRefSubjectId") = Me.txtRefSubectCode.Text.Trim()
                dr("iModifyBy") = Me.Session(S_UserID)

                'subjectdetailtable
                dr("nHeight") = Me.txtheight.Text.Trim
                dr("nWeight") = Me.txtweight.Text.Trim
                dr("nBMI") = Me.hfbmi.Value.Trim

                If rblMartialStatus.SelectedItem.Value = "S" Then
                    dr("cMaritalStatus") = "Single"
                ElseIf rblMartialStatus.SelectedItem.Value = "M" Then
                    dr("cMaritalStatus") = "Married"
                End If

                If rdoLstFoodHabit.SelectedItem.Text = "Vegetarian" Then
                    dr("cFoodHabit") = "Vegetarian"
                ElseIf rdoLstFoodHabit.SelectedItem.Text = "Non-Vegetarian" Then
                    dr("cFoodHabit") = "Non-Vegetarian"
                ElseIf rdoLstFoodHabit.SelectedItem.Text = "Eggetarian" Then
                    dr("cFoodHabit") = "Eggetarian"
                End If

                'subjectcontactdetail

                dr("nSubjectContactNo") = 1
                StrAddress1 = Me.txtlocaladds1.Text.Trim
                Address(StrAddress1, StrAdd11, StrAdd12, StrAdd13)
                dr("vLocalAdd1") = Convert.ToString(StrAdd11)
                dr("vLocalAdd12") = Convert.ToString(StrAdd12)
                dr("vLocalAdd13") = Convert.ToString(StrAdd13)

                StrAddress2 = Me.txtlocaladd2.Text.Trim
                Address(StrAddress2, StrAdd21, StrAdd22, StrAdd23)
                dr("vLocalAdd21") = Convert.ToString(StrAdd21)
                dr("vLocalAdd22") = Convert.ToString(StrAdd22)
                dr("vLocalAdd23") = Convert.ToString(StrAdd23)

                'This For Local Permanent Address
                StrPerAdd = Me.txtPermanentAdds.Text.Trim
                Address(StrPerAdd, StrAdd1, StrAdd2, StrAdd3)
                dr("vPerAdd1") = Convert.ToString(StrAdd1)
                dr("vPerAdd2") = Convert.ToString(StrAdd2)
                dr("vPerAdd3") = Convert.ToString(StrAdd3)

                'Added on 28-Jul-2009
                dr("vPerCity") = Me.txtPlace.Text.Trim()
                '****************************************

                dr("vLocalTelephoneno1") = Me.txtLocaltel1no.Text.Trim
                dr("vLocalTelephoneno2") = Me.txtLocaltel2no.Text.Trim
                dr("vPerTelephoneno") = Me.txtpertelno.Text.Trim
                dr("vOfficeAddress") = Me.txtOfficeAddress.Text.Trim
                dr("vOfficeTelephoneno") = Me.txtOfficetelno.Text.Trim
                dr("vContactName1") = Me.txtconper.Text.Trim
                dr("vContactName2") = Me.txtContactName2.Text.Trim()
                dr("vContactAddress11") = Me.txtconperadds1.Text.Trim
                dr("vContactAddress21") = Me.txtconperadds2.Text.Trim
                dr("vContactTelephoneno1") = Me.txtconpertel1.Text.Trim
                dr("vContactTelephoneno2") = Me.txtconpertel2.Text.Trim
                dr("vReferredBy") = Me.txtRefBy.Text.Trim

                'To save NUll image on 26-Nov-2008 by Naimesh Dave
                dr("vbPhotograph") = System.DBNull.Value
                '************************
                'subjectallocationtable
                dr("nSubjectWorkSpaceNo") = 1
                dr("vWorkSpaceId") = IIf(ddlprojectno.SelectedIndex = 0, System.DBNull.Value, Me.ddlprojectno.SelectedValue.Trim)
                dr("dAllocationDate") = IIf(String.IsNullOrEmpty(Me.txtdoer.Text.Trim), System.DBNull.Value, Me.txtdoer.Text.Trim)

                'Added for audittrail on 24-Aug-2009
                dr("vRemark") = Me.txtRemark.Text.Trim()
                '***************************************************

                'This is for SubjectFemaleDetails

                If tbConPIFMst.Tabs(1).Enabled Then
                    dr("dLastMenstrualDate") = IIf(String.IsNullOrEmpty(Me.txtmendate.Text.Trim), System.DBNull.Value, Me.txtmendate.Text.Trim)
                    dr("dLastDelivaryDate") = IIf(String.IsNullOrEmpty(Me.txtdold.Text.Trim), System.DBNull.Value, Me.txtdold.Text.Trim)
                    dr("cRegular") = Me.rdoLstRegular.SelectedValue
                    dr("iLastMenstrualDays") = IIf(String.IsNullOrEmpty(Me.txtcyclelength.Text), 0, Me.txtcyclelength.Text)
                    dr("vGravida") = Me.txtgravida.Text
                    dr("iNoOfChildren") = IIf(String.IsNullOrEmpty(Me.txtnoofchildren.Text), 0, Me.txtnoofchildren.Text)

                    If Me.rblAbortion.SelectedIndex > -1 Then
                        dr("cAbortions") = Me.rblAbortion.SelectedValue.Trim()
                    End If

                    dr("dAbortionDate") = IIf(String.IsNullOrEmpty(Me.txtdolabortion.Text.Trim), System.DBNull.Value, Me.txtdolabortion.Text.Trim)
                    dr("vPara") = Me.txtpara.Text
                    dr("cLoctating") = Me.RBLLactating.SelectedValue.ToUpper.Trim() 'Me.txtLactating.Text

                    If rblPain.SelectedIndex > -1 Then
                        dr("cLastMenstrualIndi") = Me.rblPain.SelectedValue.Trim()
                    End If

                    If rblVolunteer.SelectedIndex > -1 Then
                        dr("cIsVolunteerinBearingAge") = Me.rblVolunteer.SelectedValue.Trim()
                    End If

                    If rblchildrenhealthy.SelectedIndex > -1 Then
                        dr("cChildrenHelath") = rblchildrenhealthy.SelectedItem.Value.Trim()
                    End If

                    dr("vchildrenHealthRemark") = Me.txtChildrenRemark.Text.Trim()
                    dr("iNoOfChildrenDied") = IIf(Me.txtNoChildernDied.Text.Trim() = "", 0, Me.txtNoChildernDied.Text.Trim())
                    dr("vRemarkifDied") = Me.txtRemarkChDied.Text.Trim()

                    If rblcontraception.SelectedIndex > -1 Then
                        dr("cContraception") = rblcontraception.SelectedValue.Trim()
                    End If

                    For index = 0 To chkContraception.Items.Count - 1

                        If chkContraception.Items(index).Selected = True Then

                            If chkContraception.Items(index).Value.ToUpper.Trim() = "B" Then
                                dr("cBarrier") = "C"
                            ElseIf chkContraception.Items(index).Value.ToUpper.Trim() = "P" Then
                                dr("cPills") = "C"
                            ElseIf chkContraception.Items(index).Value.ToUpper.Trim() = "R" Then
                                dr("cRhythm") = "C"
                            ElseIf chkContraception.Items(index).Value.ToUpper.Trim() = "I" Then
                                dr("cIUCD") = "C"
                            End If

                        End If

                    Next index

                    dr("vOtherRemark") = Me.txtOtherRemarks.Text.Trim()

                End If

                dtOld.Rows.Add(dr)
                dtOld.TableName = "View_SubjectMaster"
                Me.ViewState(VS_DtSubjectMst) = dtOld

                'For Subject Habit Detail on 13-Dec-2008
                dtSubjectHabitDetail.Clear()

                For index = 0 To Me.GVHabits.Rows.Count - 1

                    drHabit = dtSubjectHabitDetail.NewRow
                    drHabit("vSubjectId") = Pro_Screening
                    drHabit("vHabitId") = Me.GVHabits.Rows(index).Cells(GVHC_HabitId).Text.Trim
                    drHabit("vHabitDetails") = Me.GVHabits.Rows(index).Cells(GVHC_Habits).Text.Trim
                    drHabit("cHabitFlag") = CType(Me.GVHabits.Rows(index).FindControl("ddlHebitType"), DropDownList).SelectedValue.Trim
                    drHabit("vHabitHistory") = IIf(CType(Me.GVHabits.Rows(index).FindControl("txtEndDate"), TextBox).Text.Trim = "", _
                                            System.DBNull.Value, CType(Me.GVHabits.Rows(index).FindControl("txtEndDate"), TextBox).Text.Trim)
                    drHabit("vRemarks") = CType(Me.GVHabits.Rows(index).FindControl("txtConsumption"), TextBox).Text.Trim
                    dtSubjectHabitDetail.Rows.Add(drHabit)

                Next index

                dtSubjectHabitDetail.TableName = "SubjectHabitDetails"
                Me.ViewState(VS_DtSubjectHabitDetails) = dtSubjectHabitDetail
                ''''''''''''''''''''''''''''''''''''''

            End If

            Return True
        Catch ex As Threading.ThreadAbortException
            Return False
            Exit Function
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "......AssignValues")
            Return False
        End Try
    End Function

#End Region

#Region "Reset Page"

    Private Sub resetpage()
        Me.hfAge.Value = ""
        Me.hfbmi.Value = ""
        Me.hfinitials.Value = ""
        Me.txtRefSubectCode.Text = ""
        Me.txtAge.Text = ""
        Me.txtbmi.Text = ""
        Me.txtconper.Text = ""
        Me.txtconperadds1.Text = ""
        Me.txtconperadds2.Text = ""
        Me.txtconpertel1.Text = ""
        Me.txtconpertel2.Text = ""
        Me.txtContactName2.Text = ""
        Me.txtRefBy.Text = ""
        Me.txtcyclelength.Text = ""
        Me.txtdob.Text = ""
        Me.txtdoer.Text = ""
        Me.txtdolabortion.Text = ""
        Me.txtdold.Text = ""
        Me.txteducationquali.Text = ""
        Me.txtfirstname.Text = ""
        Me.txtgravida.Text = ""
        Me.txtheight.Text = ""
        Me.txtInitials.Text = ""
        Me.txtlastname.Text = ""
        Me.txtlocaladd2.Text = ""
        Me.txtlocaladds1.Text = ""
        Me.txtLocaltel1no.Text = ""
        Me.txtLocaltel2no.Text = ""
        Me.txtmendate.Text = ""
        Me.txtmendate.Text = ""
        Me.txtmiddlename.Text = ""
        Me.txtnoofchildren.Text = ""
        Me.txtoccupation.Text = ""
        Me.txtOfficeAddress.Text = ""
        Me.txtOfficetelno.Text = ""
        Me.txtpara.Text = ""
        Me.txtPermanentAdds.Text = ""
        Me.txtpertelno.Text = ""
        Me.txtSubject.Text = ""
        Me.txtweight.Text = ""
        Me.txtotherProf.Text = ""
        Me.txtToEmailId.Text = ""
        Me.txtCCEmailId.Text = ""
        Me.lblLastRemark.Text = ""
        Me.txtRemark.Text = ""

        Me.ddlbloodgroup.SelectedIndex = 0
        Me.chklstIcfLanguage.ClearSelection()
        ' Me.ddlicflanguage.SelectedIndex = 0
        Me.ddlprojectno.SelectedIndex = 0
        Me.ddlRh.SelectedIndex = 0
        Me.HSubjectId.Value = ""
        Me.rblsex.SelectedIndex = 0
        Me.ViewState(VS_Choice) = Nothing
        Me.ViewState(VS_DtSubjectHabitDetails) = Nothing
        Me.ViewState(VS_DtSubjectMst) = Nothing
        Me.tbConPIFMst.ActiveTabIndex = 0

        If Not tbConPIFMst.Tabs(1).Enabled Then
            tbConPIFMst.Tabs(1).Enabled = False
        End If

        Me.ViewState(VS_GVDtSubjectProofDetails) = Nothing
        Me.ViewState(VS_SubjectId) = Nothing
        Me.ViewState(VS_TempTransactionNo) = 0

    End Sub

#End Region

#Region "tbConPIFMst_ActiveTabChanged"

    Protected Sub tbConPIFMst_ActiveTabChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbConPIFMst.ActiveTabChanged

        If tbConPIFMst.ActiveTabIndex = 0 Then 'Personal Information
            CType(Me.Master.FindControl("ScriptManager1"), ScriptManager).SetFocus(Me.txtlastname)
        ElseIf tbConPIFMst.ActiveTabIndex = 1 Then 'For Female
            CType(Me.Master.FindControl("ScriptManager1"), ScriptManager).SetFocus(Me.txtmendate)
        ElseIf tbConPIFMst.ActiveTabIndex = 2 Then ' Contact Information
            CType(Me.Master.FindControl("ScriptManager1"), ScriptManager).SetFocus(Me.txtlocaladds1)
        End If

    End Sub

#End Region

#Region "BUTTON EVENTS"

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsave.Click
        Dim Dt As New DataTable
        Dim Ds_Subjectmst As DataSet
        Dim ds_doctypegrid As New DataSet
        Dim eStr As String = String.Empty
        Dim objOPws As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
        Dim SubjectId As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try

            btnsave.Enabled = False


            If Not AssignValues() Then
                Me.ObjCommon.ShowAlert("Error while Assigning Data", Me.Page)
                Exit Sub
            End If


            Ds_Subjectmst = New DataSet
            Ds_Subjectmst.Tables.Add(CType(Me.ViewState(VS_DtSubjectMst), Data.DataTable).Copy())
            Ds_Subjectmst.Tables.Add(CType(Me.ViewState(VS_DtSubjectHabitDetails), Data.DataTable).Copy())
            Ds_Subjectmst.AcceptChanges()

            Choice = IIf(ViewState(VS_IsEdit), WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, _
                                                            WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add)

            If Not objOPws.Save_SubjectMst(Choice, Ds_Subjectmst, Me.Session(S_UserID), SubjectId, eStr, "") Then
                ViewState(VS_IsEdit) = False
                ShowErrorMessage(eStr, "")
                Exit Sub
            End If

            Me.ViewState(VS_SubjectId) = SubjectId

            If Not Me.ActualSaveSubjectProof() Then
                Exit Sub
            End If

            If Not IsNothing(Me.Request.QueryString("Page2")) AndAlso Me.Request.QueryString("Page2").Trim() <> "" Then
                Me.Response.Redirect(Me.Request.QueryString("page2") + ".aspx?WorkspaceId=" + Me.Request.QueryString("WorkspaceId"), False)
            End If
            'For inhouse mode
            If Me.Request.QueryString("mode") = InHouse_Mode Then
                Me.Response.Redirect("frmSubjectPIFMst.aspx?mode=" & InHouse_Mode & "&LastSubjectId=" & Me.ViewState(VS_SubjectId), False)
            Else
                If Not IsNothing(Me.Request.QueryString("SearchSubjectId")) AndAlso Me.Request.QueryString("SearchSubjectId").Trim() <> "" Then

                    'ObjCommon.ShowAlert("Subject Saved Successfully with SubjectId = " & Me.Request.QueryString("SearchSubjectId").Trim() & ".", Me)
                    Me.Response.Redirect("frmSubjectPIFMst.aspx?SearchSubjectId=" & Me.ViewState(VS_SubjectId) & "&SearchSubjectText=" & Me.txtSubject.Text.ToString() & "&Saved=True", False)
                    btnsave.Enabled = True
                Else
                    Me.Response.Redirect("frmSubjectPIFMst.aspx?SearchSubjectId=" & Me.ViewState(VS_SubjectId) & "&SearchSubjectText=" & Me.txtSubject.Text.ToString() & "&Saved=True", False)

                End If
            End If
            'For inhouse mode
            'Me.Response.Redirect("frmSubjectPIFMst.aspx?LastSubjectId=" & Me.ViewState(VS_SubjectId), False)
            



        Catch threadex As Threading.ThreadAbortException

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "...............btnSave_Click")
        End Try

    End Sub

    Protected Sub btnNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNext.Click, btnPnl1Next.Click

        Dim totalTabs As Integer = tbConPIFMst.Tabs.Count
        Dim newTabIndex As Integer = tbConPIFMst.ActiveTabIndex + 1

        If newTabIndex = 1 Then
            If Not tbConPIFMst.Tabs(1).Enabled Then
                newTabIndex = 2
            End If
        End If

        tbConPIFMst.ActiveTabIndex = newTabIndex
        tbConPIFMst_ActiveTabChanged(sender, e)

    End Sub

    Protected Sub btnPnl2Prev_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPnl3Prev.Click, btnPnl1Prev.Click

        Dim newTabIndex As Integer = tbConPIFMst.ActiveTabIndex - 1

        If newTabIndex = 1 Then
            If Not tbConPIFMst.Tabs(1).Enabled Then
                newTabIndex = 0
            End If
        End If

        tbConPIFMst.ActiveTabIndex = newTabIndex
        tbConPIFMst_ActiveTabChanged(sender, e)

    End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click

        Dim subjectId As String
        Dim dsSubject As New DataSet
        Dim ds_SubjectBlob As New DataSet
        Dim dsSubjectHabitDetail As New DataSet
        Dim wStr As String = String.Empty
        Dim wStr1 As String = String.Empty

        Dim ds_EmailAlert As New DataSet
        Dim eStr As String = String.Empty
        Try


            'resetpage()
            'Me.tbPnlFemaleDetails.Controls.Clear()
            If Not (Me.Request.QueryString("SearchSubjectId") Is Nothing AndAlso Me.Request.QueryString("SearchSubjectText") Is Nothing) Then
                Me.txtSubject.Text = Me.Request.QueryString("SearchSubjectText").ToString
                Me.HSubjectId.Value = Me.Request.QueryString("SearchSubjectId").ToString
            End If
            subjectId = HSubjectId.Value.Trim

            Me.ViewState(VS_TempTransactionNo) = 0
            'And cRejectionFlag <> 'Y'" edited by vishal for view of Rejected Subject
            If objHelp.GetView_SubjectMaster("vSubjectId='" + subjectId + "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            dsSubject, eStr_Retu) Then

                If Not objHelp.GetSubjectHabitDetails("vSubjectId='" + subjectId + "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        dsSubjectHabitDetail, eStr_Retu) Then
                    ShowErrorMessage(eStr_Retu, "")
                    Exit Sub
                End If

                Me.ViewState(VS_DtSubjectMst) = dsSubject.Tables(0)
                Me.ViewState(VS_DtSubjectHabitDetails) = dsSubjectHabitDetail.Tables(0)

                If Not fillSubjectProofGrid() Then
                    Exit Sub
                End If

                tbConPIFMst.Tabs(1).Enabled = False
                SetSubjectValues(dsSubject.Tables(0), dsSubjectHabitDetail.Tables(0))
                ViewState(VS_IsEdit) = True

                'Added for compulsory add remark while Edit else not compulsory on 24-Aug-2009
                Me.btnsave.OnClientClick = "return Validation('EDIT');"
                '*************************************************

                'Added for QC comments on 22-May-2009
                Me.BtnNew.Visible = True

                If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                    Me.BtnNew.Visible = False
                    GetChildControls(tbConPIFMst)
                End If




                'Me.cblProofOfAge.Enabled = False

                If Not fillQCGrid() Then
                    Exit Sub
                End If


                'Added on 06-July
                wStr = "nEmailAlertId =" + Email_QCOFPIF.ToString() + " And cStatusIndi <> 'D'"
                If Not Me.objHelp.GetEmailAlertMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                    ds_EmailAlert, eStr) Then

                    Me.ObjCommon.ShowAlert("Error While Getting Data From EmailAlert : " + eStr, Me.Page)
                    Exit Sub

                End If

                If ds_EmailAlert.Tables(0).Rows.Count > 0 Then

                    Me.txtToEmailId.Text = ds_EmailAlert.Tables(0).Rows(0)("vToUserEmailId").ToString()
                    Me.txtCCEmailId.Text = ds_EmailAlert.Tables(0).Rows(0)("vCCUserEmailId").ToString()

                End If
                '****added on 13-jan-2010 by deepak singh to show default 
                'image if there is no image of subject


                wStr1 = "cRecordType='P' and iTranNo=(select Max(itranNO) from SubjectBlobDetails where cRecordType='P' and vSubjectId='" & HSubjectId.Value.ToString.Trim & "') and vSubjectId='" & HSubjectId.Value.ToString.Trim & "'"

                If Not Me.objHelp.getSubjectBlobDetails(wStr1, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubjectBlob, eStr) Then
                    MsgBox("Error while getting Data." + vbCrLf + eStr)
                    Exit Sub
                End If


                If ds_SubjectBlob.Tables(0).Rows.Count > 0 Then
                    Me.Image1.ImageUrl = "frmPIFImage.aspx?subjectid=" + Me.HSubjectId.Value.Trim()
                ElseIf ds_SubjectBlob.Tables(0).Rows.Count <= 0 Then
                    Me.Image1.ImageUrl = "~/Images/demo.jpg"
                End If

                Me.txtweight.Attributes.Add("onblur", "FillBMIValue('" & Me.txtheight.ClientID & "','" & Me.txtweight.ClientID & "','" & Me.txtbmi.ClientID & "');")
                Me.txtmiddlename.Attributes.Add("onblur", "SetInitial('" & Me.txtInitials.ClientID & "');")
                Me.txtlastname.Attributes.Add("onblur", "SetInitial('" & Me.txtInitials.ClientID & "');")
                Me.txtfirstname.Attributes.Add("onblur", "SetInitial('" & Me.txtInitials.ClientID & "');")
                Me.txtInitials.Attributes.Add("disabled", "true")

                ' Me.Image1.ImageUrl = "frmPIFImage.aspx?subjectid=" + subjectId.ToString()
                '************************************
            End If

        Catch ex As Exception
            ShowErrorMessage(eStr_Retu, "........btnEdit_Click")
        End Try

    End Sub

    Protected Sub BtnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnNew.Click
        'For inhouse mode
        If Me.Request.QueryString("mode") = InHouse_Mode Then
            Me.Response.Redirect("frmSubjectPIFMst.aspx?mode=" & InHouse_Mode, False)
        Else
            Me.Response.Redirect("frmSubjectPIFMst.aspx", False)
        End If
        'For inhouse mode
        'Me.Response.Redirect("frmSubjectPIFMst.aspx", False)
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click

        If Not IsNothing(Me.Request.QueryString("Page2")) AndAlso Me.Request.QueryString("Page2").Trim() <> "" Then
            Me.Response.Redirect(Me.Request.QueryString("page2") + ".aspx?WorkspaceId=" + Me.Request.QueryString("WorkspaceId"), False)
        End If

        Me.Response.Redirect("frmMainPage.aspx", False)
    End Sub

    Protected Sub btnMSR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMSR.Click
        Dim RedirectStr As String = String.Empty
        Dim Wstr As String = String.Empty
        Dim ds_SubDetail As New DataSet
        'Dim location As String = String.Empty

        Try
            '------------ Nidhi -------------------
            Wstr = "vSubjectId='" & Me.HSubjectId.Value.Trim() & "'"
            If Not Me.objHelp.GetView_SubjectMaster(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                                ds_SubDetail, eStr_Retu) Then
                Exit Sub
            End If
            '-------------------------------------------

            If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                If Not ds_SubDetail.Tables(0).Rows(0)("vLocationcode").ToString = Location_Canada Then
                    RedirectStr = "window.open(""" & "frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&SubId=" & _
                                Me.HSubjectId.Value.Trim() & """)"
                Else
                    RedirectStr = "window.open(""" & "frmSubjectScreening_New.aspx?mode=4&Workspace=0000000000&SubId=" & _
                                Me.HSubjectId.Value.Trim() & """)"
                End If
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", RedirectStr, True)

            ElseIf Me.ViewState(VS_Choice) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                If Not ds_SubDetail.Tables(0).Rows(0)("vLocationcode").ToString = Location_Canada Then
                    RedirectStr = "window.open(""" & "frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000&SubId=" & _
                                    Me.HSubjectId.Value.Trim() & """)"
                Else
                    RedirectStr = "window.open(""" & "frmSubjectScreening_New.aspx?mode=1&Workspace=0000000000&SubId=" & _
                                    Me.HSubjectId.Value.Trim() & """)"
                End If
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "WinOpen", RedirectStr, True)
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".........btnMSR_Click")
        End Try

    End Sub

#End Region

#Region "FillQCGrid"

    Private Function fillQCGrid() As Boolean
        Dim Ds_QCGrid As New DataSet
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Try


            Wstr = "vSubjectId='" & Me.HSubjectId.Value.Trim() & "' AND cIsSourceDocComment = 'N'"
            If Not Me.objHelp.View_SubjectMasterQC(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        Ds_QCGrid, estr) Then
                Me.ShowErrorMessage(estr, estr)
                Return False
            End If

            Me.GVQCDtl.DataSource = Ds_QCGrid
            Me.GVQCDtl.DataBind()

            Me.btnQC.Visible = True

            If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View And _
                Ds_QCGrid.Tables(0).Rows.Count <= 0 Then
                Me.btnQC.Visible = False
            End If

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......fillQCGrid")
            Return False

        End Try

    End Function

#End Region

#Region "Error Handler"
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        ObjCommon.WriteError(Server, Request, Session, exMessage + " " + eStr)
    End Sub
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        ObjCommon.WriteError(Server, Request, Session, ex, exMessage + " " + eStr)
    End Sub
#End Region

#Region "RadioButton sex SelectedIndexChanged"

    Protected Sub rblsex_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblsex.SelectedIndexChanged
        rblsexSelectedIndexChanged()
    End Sub

    Private Sub rblsexSelectedIndexChanged()

        tbPnlFemaleDetails.Enabled = False
        If rblsex.SelectedIndex = 1 Then
            tbPnlFemaleDetails.Enabled = True
            Exit Sub
        End If

    End Sub

#End Region

#Region "Helper Functions"

    Public Sub Address(ByVal StrAddress As String, ByRef StrAdd11 As String, ByRef StrAdd12 As String, ByRef StrAdd13 As String)

        Try
            If Not InStr(StrAddress, ",", CompareMethod.Text) > 0 Then
                StrAdd11 = StrAddress.ToString
                Exit Sub
            End If

            StrAdd11 = StrAddress.Substring(0, StrAddress.IndexOf(","))
            StrAddress = StrAddress.Remove(0, StrAdd11.Length + 1)

            If Not InStr(StrAddress, ",", CompareMethod.Text) > 0 Then
                StrAdd12 = StrAddress.ToString
                Exit Sub
            End If

            StrAdd12 = StrAddress.Substring(0, StrAddress.IndexOf(","))
            StrAddress = StrAddress.Remove(0, StrAdd12.Length + 1)

            If StrAddress.ToString <> "" Then
                StrAdd13 = StrAddress.ToString
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......Address")
        End Try

    End Sub

#End Region

#Region "Grid Events"

    Protected Sub GVHabits_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.DataRow Then
            If Not IsNothing(e.Row.Cells(GVHC_HabitFlag).Text) AndAlso e.Row.Cells(GVHC_HabitFlag).Text.Trim() = "" Then
                CType(e.Row.FindControl("ddlHebitType"), DropDownList).SelectedValue = e.Row.Cells(GVHC_HabitFlag).Text.Trim()
            End If

            Dim txtEndDate As TextBox = CType(e.Row.FindControl("txtEndDate"), TextBox)
            Dim txtConsumption As TextBox = CType(e.Row.FindControl("txtConsumption"), TextBox)
            Dim ddlHebitType As DropDownList = CType(e.Row.FindControl("ddlHebitType"), DropDownList)

            CType(e.Row.FindControl("ddlHebitType"), DropDownList).Attributes.Add("OnChange", "validate(" + ddlHebitType.ClientID + _
                                                "," + txtConsumption.ClientID + "," + txtEndDate.ClientID + ")")

            If CType(e.Row.FindControl("ddlHebitType"), DropDownList).SelectedValue = "N" Then
                'CType(e.Row.FindControl("txtConsumption"), TextBox).Style.Add("disabled", "true")
                'CType(e.Row.FindControl("txtEndDate"), TextBox).Style.Add("disabled", "true")
                CType(e.Row.FindControl("txtConsumption"), TextBox).Enabled = False
                CType(e.Row.FindControl("txtEndDate"), TextBox).Enabled = False
            End If

        End If

    End Sub

    Protected Sub GVHabits_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        e.Row.Cells(GVHC_SubjectHabitDetailsNo).Visible = False
        e.Row.Cells(GVHC_SubjectId).Visible = False
        e.Row.Cells(GVHC_ScreenId).Visible = False
        e.Row.Cells(GVHC_HabitId).Visible = False
        e.Row.Cells(GVHC_HabitFlag).Visible = False

    End Sub

#End Region

#Region "BtnQCSave"

    Protected Sub BtnQCSaveSend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnQCSaveSend.Click
        Dim Ds_QC As New DataSet
        Dim estr As String = String.Empty
        Dim QCMsg As String = String.Empty
        Dim fromEmailId As String = String.Empty
        Dim toEmailId As String = String.Empty
        Dim password As String = String.Empty
        Dim ccEmailId As String = String.Empty
        Dim SubjectLine As String = String.Empty
        Dim wStr As String = String.Empty
        'Dim ds_EmailAlert As New DataSet
        Try

            If Me.lblResponse.Text = "" And CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                Me.ObjCommon.ShowAlert("Please Click Response Button Of Particular QC Comments Against Which You Want To Response", Me.Page)

                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowHide", "QCDivShowHide('ST');", True)

                Exit Sub
            End If

            If Not AssignValues_QC(Ds_QC) Then
                Exit Sub
            End If

            If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

                If Not Me.objLambda.Save_SubjectMasterQC(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, Ds_QC, _
                    Me.Session(S_UserID), estr) Then

                    Me.ObjCommon.ShowAlert(estr, Me.Page)
                    Exit Sub

                End If

                'If Not fillQCGrid() Then
                '    Exit Sub
                'End If

                QCMsg = "QC On PIF of " + Me.txtSubject.Text.Trim() + " <br/><br/>QC : " + Me.RBLQCFlag.SelectedItem.Text.Trim() + _
                        "<br/><br/>QC Comments: " + Me.txtQCRemarks.Text + "<br/><br/>" + _
                        "Comments Given By : " + Me.Session(S_FirstName).ToString.Trim() + " " + Me.Session(S_LastName).ToString.Trim()

                '*****************Sending Mail*************************************

                fromEmailId = ConfigurationSettings.AppSettings("Username")
                password = ConfigurationSettings.AppSettings("Password")

                'wStr = "nEmailAlertId =" + Email_QCOFPIF.ToString() + " And cStatusIndi <> 'D'"
                'If Not Me.objHelp.GetEmailAlertMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                '                    ds_EmailAlert, estr) Then

                '    Me.ObjCommon.ShowAlert("Error While Getting Data From EmailAlert : " + estr, Me.Page)
                '    Exit Sub

                'End If

                'If ds_EmailAlert.Tables(0).Rows.Count > 0 Then

                toEmailId = Me.txtToEmailId.Text.Trim() 'ds_EmailAlert.Tables(0).Rows(0)("vToUserEmailId").ToString()
                ccEmailId = Me.txtCCEmailId.Text.Trim() 'ds_EmailAlert.Tables(0).Rows(0)("vCCUserEmailId").ToString()
                SubjectLine = "QC On PIF of " + HSubjectId.Value.Trim() 'ds_EmailAlert.Tables(0).Rows(0)("vSubjectLine").ToString()

                Dim sn As New SendMail(Server, Request, Session, SubjectLine, QCMsg, fromEmailId, password, toEmailId, ccEmailId)

                'Changed on 26-Aug-2009
                If Not sn.Send(Server, Response, Session, , fromEmailId, password) Then
                    Me.ObjCommon.ShowAlert("Record Saved Successfully But Email Is Not Sent Due To Some Reason", Me.Page)
                    Exit Sub
                End If
                '****************************************************

                sn = Nothing
                Me.ObjCommon.ShowAlert("Record Saved Successfully", Me.Page)

                'Else
                '    Me.ObjCommon.ShowAlert("Record Saved Successfully But Email Is Not Sent Due To Some Reason", Me.Page)
                'End If

            Else 'For Response

                If Not Me.objLambda.Save_SubjectMasterQC(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, Ds_QC, _
                Me.Session(S_UserID), estr) Then

                    Me.ObjCommon.ShowAlert(estr, Me.Page)
                    Exit Sub

                End If

                Me.ObjCommon.ShowAlert("Response Saved Successfully", Me.Page)

            End If

            If Not fillQCGrid() Then
                Exit Sub
            End If

            Me.txtQCRemarks.Text = ""
            Me.lblResponse.Text = ""
            Me.RBLQCFlag.SelectedValue = "F"

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....BtnQCSaveSend_Click")
        End Try
    End Sub

    Protected Sub BtnQCSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnQCSave.Click
        Dim Ds_QC As New DataSet
        Dim estr As String = String.Empty
        Dim QCMsg As String = String.Empty
        Dim fromEmailId As String = String.Empty
        Dim toEmailId As String = String.Empty
        Dim password As String = String.Empty
        Dim ccEmailId As String = String.Empty
        Dim SubjectLine As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_EmailAlert As New DataSet
        Try

            If Me.lblResponse.Text = "" And CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                Me.ObjCommon.ShowAlert("Please Click Response Button Of Particular QC Comments Against Which You Want To Response", Me.Page)

                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowHide", "QCDivShowHide('ST');", True)

                Exit Sub
            End If

            If Not AssignValues_QC(Ds_QC) Then
                Exit Sub
            End If

            If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

                If Not Me.objLambda.Save_SubjectMasterQC(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, Ds_QC, _
                    Me.Session(S_UserID), estr) Then

                    Me.ObjCommon.ShowAlert(estr, Me.Page)
                    Exit Sub

                End If

                Me.ObjCommon.ShowAlert("Record Saved Successfully", Me.Page)

            Else 'For Response

                If Not Me.objLambda.Save_SubjectMasterQC(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, Ds_QC, _
                Me.Session(S_UserID), estr) Then

                    Me.ObjCommon.ShowAlert(estr, Me.Page)
                    Exit Sub

                End If

                Me.ObjCommon.ShowAlert("Response Saved Successfully", Me.Page)

            End If

            If Not fillQCGrid() Then
                Exit Sub
            End If

            Me.txtQCRemarks.Text = ""
            Me.lblResponse.Text = ""
            Me.RBLQCFlag.SelectedValue = "F"

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....BtnQCSave_Click")
        End Try
    End Sub

#End Region

#Region "AssignValues_QC"

    Private Function AssignValues_QC(ByRef DsSave As DataSet) As Boolean
        Dim DtMedExInfoHdr As New DataTable
        Dim ObjId As String = String.Empty
        Dim dr As DataRow
        Dim TranNo As Integer = 0
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim ds_MEdexInfroHdrQC As New DataSet
        Dim dtMEdexInfroHdrQC As New DataTable
        Try

            If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

                If Not Me.objHelp.GetSubjectMasterQC(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_MEdexInfroHdrQC, estr) Then
                    Me.ObjCommon.ShowAlert(estr, Me.Page)
                    Return False
                End If

                dtMEdexInfroHdrQC = ds_MEdexInfroHdrQC.Tables(0)

                dr = dtMEdexInfroHdrQC.NewRow
                dr("nSubjectMasterQCNo") = 1
                dr("vSubjectId") = Me.HSubjectId.Value.Trim()
                dr("iTranNo") = 1
                dr("vQCComment") = Me.txtQCRemarks.Text.Trim()
                dr("cQCFlag") = Me.RBLQCFlag.SelectedValue.Trim()
                dr("iModifyBy") = Me.Session(S_UserID)
                dr("cStatusIndi") = "N"
                dr("iQCGivenBy") = Me.Session(S_UserID)
                dr("dQCGivenOn") = System.DateTime.Today.Now.ToString("dd-MMM-yyyy hh:mm tt")

                dtMEdexInfroHdrQC.Rows.Add(dr)

            Else

                dtMEdexInfroHdrQC = Me.ViewState(VS_DtQC)

                For Each dr In dtMEdexInfroHdrQC.Rows
                    dr("iModifyBy") = Me.Session(S_UserID)
                    dr("cStatusIndi") = "E"
                    dr("vResponse") = Me.txtQCRemarks.Text.Trim()
                    dr("iResponseGivenBy") = Me.Session(S_UserID)
                    dr("dResponseGivenOn") = System.DateTime.Today.Now.ToString("dd-MMM-yyyy hh:mm tt")

                    dr.AcceptChanges()
                Next dr

            End If

            dtMEdexInfroHdrQC.AcceptChanges()

            dtMEdexInfroHdrQC.TableName = "SubjectMasterQC"
            dtMEdexInfroHdrQC.AcceptChanges()

            DsSave.Tables.Add(dtMEdexInfroHdrQC.Copy())
            DsSave.AcceptChanges()
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "........AssignValues_QC")
            Return False
        End Try

    End Function

#End Region

#Region "GVQCDtl Grid Events"

    Protected Sub GVQCDtl_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVQCDtl.RowCreated
        e.Row.Cells(GVCQC_SubjectMasterQCNo).Visible = False
        e.Row.Cells(GVCQC_SubjectId).Visible = False
        e.Row.Cells(GVCQC_QCFlag).Visible = False


        e.Row.Cells(GVCQC_LnkResponse).Visible = True

        If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

            e.Row.Cells(GVCQC_LnkResponse).Visible = False

        End If


    End Sub

    Protected Sub GVQCDtl_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVQCDtl.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lnkResponse"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkResponse"), LinkButton).CommandName = "Response"
            CType(e.Row.FindControl("lnkResponse"), LinkButton).OnClientClick = "return QCDivShowHide('ST');"
        End If

    End Sub

    Protected Sub GVQCDtl_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GVQCDtl.RowCommand
        Dim index As Integer = CType(e.CommandArgument, Integer)
        Dim Wstr As String = ""
        Dim estr As String = ""
        Dim ds_MEdexInfroHdrQC As New DataSet

        If e.CommandName.ToUpper.Trim() = "RESPONSE" Then

            Me.lblResponse.Text = "Response To: " + Me.GVQCDtl.Rows(index).Cells(GVCQC_QCComment).Text.Trim()

            Wstr = "nSubjectMasterQCNo=" + Me.GVQCDtl.Rows(index).Cells(GVCQC_SubjectMasterQCNo).Text.Trim()

            If Not Me.objHelp.GetSubjectMasterQC(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    ds_MEdexInfroHdrQC, estr) Then

                Me.ObjCommon.ShowAlert(estr, Me.Page)
                Exit Sub

            End If

            Me.ViewState(VS_DtQC) = ds_MEdexInfroHdrQC.Tables(0)

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowHide", "QCDivShowHide('ST');", True)

        End If

    End Sub

#End Region

#Region "Subject Proof"

    Private Function fillSubjectProofGrid() As Boolean
        Dim dsSubjectProof As New DataSet
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim choice As WS_HelpDbTable.DataRetrievalModeEnum

        Try

            choice = WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition
            If Not IsNothing(Me.Request.QueryString("Subject Id")) Then
                wStr = "vSubjectId='" + Me.Request.QueryString("SubjectId").ToString.Trim() + "'"
            ElseIf Not (Me.HSubjectId.Value.ToString() Is Nothing Or Me.HSubjectId.Value.ToString() = "") Then
                wStr = "vSubjectId='" + Me.HSubjectId.Value.ToString() + "'"
            ElseIf Not (Me.ViewState(VS_SubjectId) Is Nothing Or Me.ViewState(VS_SubjectId) = "") Then
                wStr = "vSubjectId='" + Me.ViewState(VS_SubjectId) + "'"
            Else
                choice = WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty
            End If

            wStr += " And cStatusIndi <> 'D'"

            If Not Me.objHelp.GetSubjectProofDetails(wStr, choice, dsSubjectProof, eStr) Then
                Me.ObjCommon.ShowAlert(eStr, Me.Page)
                Return False
            End If

            If Not BindGridSubjectProof(dsSubjectProof.Tables(0)) Then
                Exit Function
            End If
            Me.ViewState(VS_GVDtSubjectProofDetails) = dsSubjectProof.Tables(0)

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...fillSubjectProofGrid")
            Return False
        End Try

    End Function

    Protected Sub btnAttach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAttach.Click
        Dim dr As DataRow
        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Dim dt_GVSubjectProof As New DataTable
        Dim strPath As String = String.Empty
        Dim di As DirectoryInfo
        Dim dirNo As Integer = 0
        Dim TranNo As Integer = 0
        Try

            Choice = Me.ViewState(VS_Choice)

            If Me.FlAttachment.FileName.Trim() = "" Then
                Me.ObjCommon.ShowAlert("Please Select A File To Upload", Me.Page)
                Exit Sub
            End If

            dt_GVSubjectProof = Me.ViewState(VS_GVDtSubjectProofDetails)

            dr = dt_GVSubjectProof.NewRow
            'nSubjectProofNo,vSubjectId,iTranNo,vProofType,vProofPath,iModifyBy,dModifyOn,cStatusIndi
            dr("nSubjectProofNo") = 0
            dr("vSubjectId") = ""
            dr("iTranNo") = 0
            dr("vProofType") = Me.ddlProofType.SelectedItem.Text.ToString()

            If Me.ddlProofType.SelectedItem.Text.Trim.ToUpper() = "OTHERS" Then
                dr("vProofType") = "OTHERS:" + Me.txtAttach.Text.Trim()
            End If

            dr("iModifyBy") = Me.Session(S_UserID)
            dr("cStatusIndi") = "N"

            strPath = Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings("TempSubjectProofDetails"))
            di = New DirectoryInfo(strPath)
            dirNo = Me.Session(S_UserID)
            strPath += dirNo.ToString() + "\"

            'File Creation**************

            di = New DirectoryInfo(strPath)

            If CType(Me.ViewState(VS_TempTransactionNo), Integer) = 0 Then

                If di.Exists() Then
                    TranNo = di.GetDirectories.Length
                End If

                TranNo += 1
                Me.ViewState(VS_TempTransactionNo) = TranNo

            End If

            strPath += CType(Me.ViewState(VS_TempTransactionNo), String).Trim() + "\"
            di = Nothing
            di = New DirectoryInfo(strPath)
            If Not di.Exists() Then
                Directory.CreateDirectory(strPath)
            End If

            Me.FlAttachment.SaveAs(strPath + Path.GetFileName(Me.FlAttachment.PostedFile.FileName))

            'End **********************

            dr("vProofPath") = "~\" + System.Configuration.ConfigurationSettings.AppSettings("TempSubjectProofDetails") + dirNo.ToString() + "\" + Me.ViewState(VS_TempTransactionNo).ToString() + "\" + Path.GetFileName(Me.FlAttachment.PostedFile.FileName)

            dt_GVSubjectProof.Rows.Add(dr)
            dt_GVSubjectProof.AcceptChanges()
            Me.ViewState(VS_GVDtSubjectProofDetails) = dt_GVSubjectProof

            If Not BindGridSubjectProof(dt_GVSubjectProof) Then
                Exit Sub
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....btnAttach_Click")
        End Try
    End Sub

#End Region

#Region "ActualSaveSubjectProof"

    Private Function ActualSaveSubjectProof() As Boolean
        Dim dr As DataRow
        Dim drSave As DataRow
        Dim ds_SubjectProof As New DataSet

        Dim ds_Save As New DataSet
        Dim dt_Save As New DataTable

        Dim estr As String = String.Empty
        Dim wstr As String = String.Empty
        Dim strPath As String = String.Empty
        Dim di As DirectoryInfo
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Dim Retu_TranNo As String = String.Empty
        Dim fileName As String = String.Empty
        Dim filePath As String = String.Empty
        Dim sourcePath As String = String.Empty

        Try
            Choice = Me.ViewState(VS_Choice)

            ds_SubjectProof.Tables.Add(CType(Me.ViewState(VS_GVDtSubjectProofDetails), DataTable).Copy())

            If ds_SubjectProof.Tables(0).Rows.Count < 1 Then
                Return True
            End If

            dt_Save = Me.ViewState(VS_GVDtSubjectProofDetails)

            For Each dr In ds_SubjectProof.Tables(0).Rows

                If dr("vSubjectId") = "" Then
                    Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
                Else
                    Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit
                End If

                dr("vSubjectId") = Me.ViewState(VS_SubjectId)

                strPath = "~/" + System.Configuration.ConfigurationSettings.AppSettings("FolderForSubjectProof")
                filePath = dr("vProofPath").ToString.Trim()
                fileName = filePath.Substring(filePath.LastIndexOf("\") + 1) ', filePath.Length - 1)

                dr("vProofPath") = strPath + Me.ViewState(VS_SubjectId) + "/" + fileName
                ds_SubjectProof.AcceptChanges()


                dt_Save.Rows.Clear()
                drSave = dt_Save.NewRow()
                drSave("nSubjectProofNo") = dr("nSubjectProofNo")
                drSave("vSubjectId") = dr("vSubjectId")
                drSave("iTranNo") = dr("iTranNo")
                drSave("vProofType") = dr("vProofType")
                drSave("vProofPath") = dr("vProofPath")
                drSave("iModifyBy") = dr("iModifyBy")
                drSave("cStatusIndi") = dr("cStatusIndi")

                dt_Save.Rows.Add(drSave)
                ds_Save.Tables.Clear()
                ds_Save.Tables.Add(dt_Save)

                If Not Me.objLambda.Save_SubjectProofDetails(Choice, ds_Save, Me.Session(S_UserID), Retu_TranNo, estr) Then
                    Me.ObjCommon.ShowAlert(estr, Me.Page)
                    Return False
                End If

                If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                    'File Creation**************
                    strPath = Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings("FolderForSubjectProof"))
                    strPath += Me.ViewState(VS_SubjectId) + "\" + Retu_TranNo + "\"
                    di = New DirectoryInfo(strPath)
                    If Not di.Exists() Then
                        Directory.CreateDirectory(strPath)
                    End If

                    sourcePath = Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings("TempSubjectProofDetails"))
                    sourcePath += CType(Me.Session(S_UserID), String) + "\" + CType(Me.ViewState(VS_TempTransactionNo), String).Trim() + "\" + fileName
                    strPath += fileName

                    If Not File.Exists(strPath) Then
                        If File.Exists(sourcePath) Then
                            File.Copy(sourcePath, strPath)
                        End If
                    End If
                    'File.Delete(sourcePath)
                    'End **********************

                End If

            Next dr

            'sourcePath = sourcePath.Substring(0, sourcePath.LastIndexOf("\") + 1)
            'di = New DirectoryInfo(sourcePath)
            'di.Delete()

            Me.ViewState(VS_TempTransactionNo) = 0
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".........ActualSaveSubjectProof")
            Return False
        End Try
    End Function

#End Region

#Region "GVSubjectProof Events"

    Protected Sub GVSubjectProof_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVSubjectProof.RowCreated
        e.Row.Cells(GVCSubProof_nSubjectProofNo).Visible = False
        e.Row.Cells(GVCSubProof_vSubjectId).Visible = False
        e.Row.Cells(GVCSubProof_iTranNo).Visible = False
        'e.Row.Cells(GVCSubProof_vProofType).Visible = False
        e.Row.Cells(GVCSubProof_vProofPath).Visible = False
        e.Row.Cells(GVCSubProof_dModifyOn).Visible = False
        e.Row.Cells(GVCSubProof_iModifyBy).Visible = False
        e.Row.Cells(GVCSubProof_cStatusIndi).Visible = False
    End Sub

    Protected Sub GVSubjectProof_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVSubjectProof.RowDataBound
        If Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
            If (Not Me.Request.QueryString("WorkSpaceId") Is Nothing) AndAlso (Not Me.Request.QueryString("Type") Is Nothing) Then
                e.Row.Cells(GVCSubProof_Attachment).Enabled = False
                e.Row.Cells(GVCSubProof_Delete).Enabled = False
            End If
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then

            CType(e.Row.FindControl("hlnkFile"), HyperLink).NavigateUrl = CType(e.Row.FindControl("hlnkFile"), HyperLink).Text.Trim()
            CType(e.Row.FindControl("hlnkFile"), HyperLink).Text = Path.GetFileName(CType(e.Row.FindControl("hlnkFile"), HyperLink).Text.Trim())

            CType(e.Row.FindControl("imgbtnDelete"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("imgbtnDelete"), ImageButton).CommandName = "DELETE"

        End If
    End Sub

    Protected Sub GVSubjectProof_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GVSubjectProof.RowCommand
        Dim index As Integer = e.CommandArgument
        Dim dtGVSubjectProof As New DataTable
        Dim SubjectProofNo As Integer = 0


        Try

            If e.CommandName.ToUpper = "DELETE" Then

                SubjectProofNo = Me.GVSubjectProof.Rows(index).Cells(GVCSubProof_nSubjectProofNo).Text.Trim()

                dtGVSubjectProof = CType(Me.ViewState(VS_GVDtSubjectProofDetails), DataTable)

                If Me.GVSubjectProof.Rows(index).Cells(GVCSubProof_vSubjectId).Text.Trim() = "" Or _
                            Me.GVSubjectProof.Rows(index).Cells(GVCSubProof_vSubjectId).Text.Trim() = "&nbsp;" Then

                    dtGVSubjectProof.Rows(index).Delete()
                    dtGVSubjectProof.AcceptChanges()

                Else
                    For IndexSubProof As Integer = 0 To dtGVSubjectProof.Rows.Count - 1
                        If SubjectProofNo = dtGVSubjectProof.Rows(IndexSubProof)("nSubjectProofNo") Then
                            dtGVSubjectProof.Rows(IndexSubProof)(GVCSubProof_cStatusIndi) = "D"
                            dtGVSubjectProof.AcceptChanges()
                        End If
                    Next
                End If

                If Not BindGridSubjectProof(dtGVSubjectProof) Then
                    Exit Sub
                End If

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".......GVSubjectProof_RowCommand")
        End Try

    End Sub

    Protected Sub GVSubjectProof_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GVSubjectProof.RowDeleting

    End Sub

    Private Function BindGridSubjectProof(ByVal dt As DataTable) As Boolean
        Dim dv As New DataView
        Dim index As Integer = 0

        dv = dt.DefaultView
        dv.RowFilter = "cStatusIndi <> 'D'"
        dt = CType(dv, Data.DataView).ToTable()

        Me.GVSubjectProof.DataSource = dt
        Me.GVSubjectProof.DataBind()

        Me.cblProofOfAge.Enabled = True
        If (Not Me.Request.QueryString("mode") Is Nothing) AndAlso (Not Me.Request.QueryString("Type") Is Nothing) Then
            If Me.Request.QueryString("mode").ToString() = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                Me.cblProofOfAge.Enabled = False
            End If

        End If


        'For Each item As ListItem In Me.cblProofOfAge.Items
        '    item.Selected = False
        'Next

        'For Each dr As DataRow In dt.Rows

        '    For Each item As ListItem In Me.cblProofOfAge.Items

        '        If dr("vProofType").ToString.Trim.ToUpper().Contains(item.Text.Trim.ToUpper()) Then

        '            item.Selected = True

        '        End If

        '    Next

        'Next
        'Me.cblProofOfAge.Enabled = False

        Return True
    End Function

#End Region

    'Protected Sub ddlHebitType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim gridViewRow As GridViewRow = CType(CType(sender, DropDownList).Parent.Parent, GridViewRow)

    '    If CType(gridViewRow.FindControl("ddlHebitType"), DropDownList).SelectedValue = "N" Then
    '        CType(gridViewRow.FindControl("txtConsumption"), TextBox).Enabled = False
    '        CType(gridViewRow.FindControl("txtEndDate"), TextBox).Enabled = False
    '    End If

    'End Sub

#Region "Checking For Duplication Of Subjects"

    Protected Sub btnCheckDuplicateSubject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCheckDuplicateSubject.Click

        Dim ds As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim SubjectId As String = String.Empty

        Try

            wStr = "vInitials = '" + Me.hfinitials.Value.Trim() + "' And dBirthDate = '" + Me.txtdob.Text.Trim() + "'"
            If Not Me.objHelp.GetSubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds, eStr) Then
                Throw New Exception(eStr)
            End If

            If Not ds Is Nothing Then

                For Each dr As DataRow In ds.Tables(0).Rows

                    If SubjectId <> String.Empty Then
                        SubjectId += ","
                    End If
                    SubjectId += ds.Tables(0).Rows(0)("vSubjectId").ToString()

                Next dr

                If SubjectId <> String.Empty Then
                    Me.ObjCommon.ShowAlert("Subject(s) With Id(s) " + SubjectId + " Have Same Initials And Birth Date, Please Verify.", Me.Page)
                End If

            End If

        Catch ex As Exception
            Me.ShowErrorMessage("Error While Checking For Subject Duplication. ", ex.Message)
        End Try

    End Sub

#End Region

#Region "enable controlls"
    Private Sub GetChildControls(ByVal parent As Control)

        If parent.HasControls() Then

            Dim CtrlClctn As ControlCollection
            Dim ctrl As Control

            CtrlClctn = parent.Controls

            For Each ctrl In CtrlClctn

                If Not (ctrl.ID = "GVSubjectProof" Or ctrl.ID = "btnNext" Or ctrl.ID = "btnPnl3Prev") Then
                    EnableControlls(ctrl)
                End If


                If ctrl.HasControls() Then
                    GetChildControls(ctrl)
                End If
            Next ctrl

        End If
    End Sub


    Private Sub EnableControlls(ByVal ctrl As Control)
        If ctrl.GetType.ToString.Trim() = "System.Web.UI.WebControls.Label" Then
            CType(ctrl, Label).Enabled = False
        ElseIf ctrl.GetType.ToString.Trim() = "System.Web.UI.WebControls.TextBox" Then
            CType(ctrl, TextBox).Enabled = False
        ElseIf ctrl.GetType.ToString.Trim() = "System.Web.UI.WebControls.DropDownList" Then
            CType(ctrl, DropDownList).Enabled = False
        ElseIf ctrl.GetType.ToString.Trim() = "System.Web.UI.WebControls.FileUpload" Then
            CType(ctrl, FileUpload).Enabled = False
        ElseIf ctrl.GetType.ToString.Trim() = "System.Web.UI.WebControls.CheckBoxList" Then
            CType(ctrl, CheckBoxList).Enabled = False
        ElseIf ctrl.GetType.ToString.Trim() = "System.Web.UI.WebControls.Image" Then
            CType(ctrl, Image).Enabled = False
        ElseIf ctrl.GetType.ToString.Trim() = "System.Web.UI.WebControls.Button" Then
            CType(ctrl, Button).Enabled = False
        ElseIf ctrl.GetType.ToString.Trim() = "System.Web.UI.WebControls.RadioButtonList" Then
            CType(ctrl, RadioButtonList).Enabled = False
        ElseIf ctrl.GetType.ToString.Trim() = "System.Web.UI.WebControls.Panel" Then
            CType(ctrl, Panel).Enabled = False
        ElseIf ctrl.GetType.ToString.Trim() = "System.Web.UI.WebControls.GridView" Then
            CType(ctrl, GridView).Enabled = False
        ElseIf ctrl.GetType.ToString.Trim() = "System.Web.UI.WebControls.ImageButton" Then
            CType(ctrl, ImageButton).Enabled = False
        End If


    End Sub
#End Region

End Class