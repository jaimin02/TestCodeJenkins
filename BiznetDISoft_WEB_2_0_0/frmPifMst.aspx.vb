Partial Class frmPifMst
    Inherits System.Web.UI.Page

    Dim ObjCommon As New clsCommon
    Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
    Private Const VS_Choice As String = "Choice"

#Region "GenCall"

    Private Function GenCall() As Boolean
        Dim dt_SubjectMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try
            Page.Title = ":: PIF Master :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            Choice = CType("1", WS_Lambda.DataObjOpenSaveModeEnum)
            Me.ViewState("Choice") = Choice   'To use it while saving
            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState("vSubjectId") = Me.Request.QueryString("Value").ToString
            End If
            ''''Check for Valid User''''''''''''''
            If Not GenCall_Data(Choice, dt_SubjectMst) Then ' For Data Retrieval
                Exit Function
            End If
            Me.ViewState("dtSubjectMst") = dt_SubjectMst ' adding blank DataTable in viewstate
            If Not GenCall_ShowUI(Choice, dt_SubjectMst) Then 'For Displaying Data 
                Exit Function
            End If
            GenCall = True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function
#Region "GenCall_Data"
    Private Function GenCall_Data(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                            ByRef dt_Dist_Retu As DataTable) As Boolean

        Dim eStr As String = ""
        Dim wStr As String = ""
        Dim ds_SubjectMst As DataSet = Nothing
        'Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable
        Try

            If Choice_1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "vSubjectId=" + Me.ViewState("vSubjectId").ToString() 'Value of where condition
            End If
            If Not objHelp.GetView_SubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubjectMst, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_SubjectMst Is Nothing Then
                Throw New Exception(eStr)
            End If
            If ds_SubjectMst.Tables(0).Rows.Count <= 0 And _
               Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                Throw New Exception("No Records Found for Selected role")

            End If
            dt_Dist_Retu = ds_SubjectMst.Tables(0)
            GenCall_Data = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function
#End Region
#Region "GenCall_ShowUI"
    Private Function GenCall_ShowUI(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                                      ByVal dt_Dist As DataTable) As Boolean
        Dim dt_SubjMst As DataTable = Nothing
        If Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
            Me.txtfirstname.Text = ConvertDbNullToDbTypeDefaultValue(dt_SubjMst.Rows(0).Item("vFirstName"), dt_SubjMst.Rows(0).Item("vFirstName").GetType)
            Me.txtlastname.Text = ConvertDbNullToDbTypeDefaultValue(dt_SubjMst.Rows(0).Item("vSurName"), dt_SubjMst.Rows(0).Item("vSurName").GetType)
            Me.txtmiddlename.Text = ConvertDbNullToDbTypeDefaultValue(dt_SubjMst.Rows(0).Item("vMiddleName"), dt_SubjMst.Rows(0).Item("vMiddleName").GetType)
            Me.txtInitials.Text = ConvertDbNullToDbTypeDefaultValue(dt_SubjMst.Rows(0).Item("vInitials"), dt_SubjMst.Rows(0).Item("vInitials").GetType)
            Me.txtdob.Text = ConvertDbNullToDbTypeDefaultValue(dt_SubjMst.Rows(0).Item("dBirthDate"), dt_SubjMst.Rows(0).Item("dBirthDate").GetType)
            Me.txtdoer.Text = ConvertDbNullToDbTypeDefaultValue(dt_SubjMst.Rows(0).Item("dEnrollmentDate"), dt_SubjMst.Rows(0).Item("dEnrollmentDate").GetType)
            Me.txteducationquali.Text = ConvertDbNullToDbTypeDefaultValue(dt_SubjMst.Rows(0).Item("vEducationQualification"), dt_SubjMst.Rows(0).Item("vEducationQualification").GetType)
            Me.txtoccupation.Text = ConvertDbNullToDbTypeDefaultValue(dt_SubjMst.Rows(0).Item("vOccupation"), dt_SubjMst.Rows(0).Item("vOccupation").GetType)
        End If
        GenCall_ShowUI = True
    End Function
#End Region
#End Region

#Region "fill Dropdown"
    Private Function Filldropdown() As Boolean
        Dim wStr As String = ""
        Dim eStr As String = ""
        Dim ds_subjtypemst As New DataSet
        Dim ds_workspacemst As New DataSet
        Dim ds_subjecthabitmst As New DataSet

        Try
            objHelp.FillDropDown("SubjectLanguageMst", "vLanguageId", "vLanguageName", "", ds_subjtypemst, eStr)
            Me.ddlicflanguage.DataSource = ds_subjtypemst.Tables(0)
            Me.ddlicflanguage.DataValueField = "vLanguageId"
            Me.ddlicflanguage.DataTextField = "vLanguageName"
            Me.ddlicflanguage.DataBind()

            objHelp.FillDropDown("workspacemst", "vWorkSpaceId", "vWorkSpaceDesc", "", ds_workspacemst, eStr)
            Me.ddlprojectno.DataSource = ds_workspacemst.Tables(0)
            Me.ddlprojectno.DataValueField = "vWorkSpaceId"
            Me.ddlprojectno.DataTextField = "vWorkSpaceDesc"
            Me.ddlprojectno.DataBind()

            objHelp.FillDropDown("subjectHabitMst", "vHabitId", "vHabitDetails", "", ds_subjecthabitmst, eStr)
            Me.rblhabitdetail.DataSource = ds_subjecthabitmst.Tables(0)
            Me.rblhabitdetail.DataValueField = "vHabitId"
            Me.rblhabitdetail.DataTextField = "vHabitDetails"
            Me.rblhabitdetail.DataBind()


        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
            Return False
        End Try

    End Function


#End Region

#Region "Reset Page"
    Private Sub resetpage()
        Me.txtdoer.Text = ""
        Me.txtfirstname.Text = ""
        Me.txtlastname.Text = ""
        Me.txtmiddlename.Text = ""
        Me.txtInitials.Text = ""
        Me.txtdob.Text = ""
        Me.ddlicflanguage.SelectedValue = ""



        Me.ddlbloodgroup.SelectedValue = ""
        Me.txteducationquali.Text = ""
        Me.txtoccupation.Text = ""
    End Sub

#End Region

#Region "Save"
    Private Function AssignUpdatedValues() As Boolean
        Dim dtOld As New DataTable
        Dim dr As DataRow
        Dim strarr(3) As String
        Dim i As Integer = 0
        Try
            dtOld = Me.ViewState("dtSubjectMst")
            dr = dtOld.NewRow
            dr("vLocationCode") = "0001"
            dr("vSubjectID") = "0000000000"
            dr("nSubjectDetailNo") = 1 '"0000000000"
            dr("nSubjectHabitDetailsNo") = 1

            dr("dEnrollmentDate") = Me.txtdoer.Text.Trim
            dr("vFirstName") = Me.txtfirstname.Text.Trim
            dr("vSurName") = Me.txtlastname.Text.Trim
            dr("vMiddleName") = Me.txtmiddlename.Text.Trim
            dr("vInitials") = Me.txtInitials.Text.Trim
            dr("dBirthDate") = Me.txtdob.Text.Trim
            dr("vICFLanguageCodeId") = Me.ddlicflanguage.SelectedValue.Trim
            If rblsex.SelectedItem.Value = "M" Then
                dr("cSex") = "Male"
            ElseIf rblsex.SelectedItem.Value = "F" Then
                dr("cSex") = "Female"
            End If
            If rblsex.SelectedItem.Text = "Female" Then
                Me.lnkfemaledt.Visible = True
            End If
            For Each item As ListItem In cblProofOfAge.Items
                If item.Selected Then

                    strarr(i) = item.Text
                    i += 1
                End If
            Next item
            dr("vProofOfAge1") = strarr(0)
            dr("vProofOfAge2") = strarr(1)
            dr("vProofOfAge3") = strarr(2)
            If strarr(i) > 3 Then
                ObjCommon.ShowAlert("select Maximum 3 proof ", Me)
            End If
            dr("cBloodGroup") = Me.ddlbloodgroup.SelectedValue.Trim
            dr("vEducationQualification") = Me.txteducationquali.Text.Trim
            dr("vOccupation") = Me.txtoccupation.Text.Trim
            'dr("vOtherRemark") = Me.txtotherspe.Text
            dr("iModifyBy") = Me.Session(S_UserID)
            'subjectdetailtable
            dr("nHeight") = Me.txtheight.Text.Trim
            dr("nWeight") = Me.txtweight.Text.Trim
            dr("nBMI") = Me.txtbmi.Text.Trim

            If rblMartialStatus.SelectedItem.Value = "S" Then
                dr("cMaritalStatus") = "Single"
            ElseIf rblMartialStatus.SelectedItem.Value = "M" Then
                dr("cMaritalStatus") = "Married"
            End If
            dr("vHabitHistory") = Me.txthabithistory.Text.Trim

            If chfooddetail.SelectedItem.Text = "vegetarian" Then
                dr("cFoodHabit") = "vegetarian"
            ElseIf chfooddetail.SelectedItem.Text = "Non-vegetarian" Then
                dr("cFoodHabit") = "Non-vegetarian"
            ElseIf chfooddetail.SelectedItem.Text = "Eggetarian" Then
                dr("cFoodHabit") = "Eggetarian"
            End If
            'subjectcontactdetail
            dr("nSubjectContactNo") = 1
            dr("vLocalAdd1") = Me.txtlocaladds1.Text.Trim
            dr("vLocalAdd21") = Me.txtlocaladd2.Text.Trim
            dr("vLocalTelephoneno1") = Me.txtLocaltel1no.Text.Trim
            dr("vLocalTelephoneno2") = Me.txtLocaltel2no.Text.Trim
            dr("vPerAdd1") = Me.txtPermanentAdds.Text.Trim
            dr("vPerTelephoneno") = Me.txtpertelno.Text.Trim
            dr("vOfficeAddress") = Me.txtOfficeAddress.Text.Trim
            dr("vOfficeTelephoneno") = Me.txtOfficetelno.Text.Trim
            dr("vContactName1") = Me.txtconper.Text.Trim
            dr("vContactAddress11") = Me.txtconperadds1.Text.Trim
            dr("vContactAddress12") = Me.txtconperadds2.Text.Trim
            dr("vContactTelephoneno1") = Me.txtconpertel1.Text.Trim
            dr("vContactTelephoneno2") = Me.txtconpertel2.Text.Trim
            ''dr("vbPhotograph") = DBNull.Value
            ''dr("vbAutograph") = DBNull.Value
            'subjectallocationtable
            dr("nSubjectWorkSpaceNo") = 1
            dr("vWorkSpaceId") = Me.ddlprojectno.SelectedValue.Trim
            'dr("iScreenId") = "1"
            dr("dAllocationDate") = Me.txtdoer.Text.Trim
            'subjecthabitdetailtable
            'If rblhabitdetail.SelectedItem.Value = "C" Then
            '    dr("vHabitDetails") = "Cigarettes"
            'ElseIf rblhabitdetail.SelectedItem.Value = "T" Then
            '    dr("vHabitDetails") = "Tobacco"
            'ElseIf rblhabitdetail.SelectedItem.Value = "B" Then
            '    dr("vHabitDetails") = "Beedies"
            'ElseIf rblhabitdetail.SelectedItem.Value = "SWT" Then
            '    dr("vHabitDetails") = "Supari With Tobacco"
            'ElseIf rblhabitdetail.SelectedItem.Value = "G" Then
            '    dr("vHabitDetails") = "Gutkha"
            'ElseIf rblhabitdetail.SelectedItem.Value = "A" Then
            '    dr("vHabitDetails") = "Alcohol"
            'ElseIf rblhabitdetail.SelectedItem.Value = "O" Then
            '    dr("vHabitDetails") = "Others"
            'End If
            dr("vHabitId") = Me.rblhabitdetail.SelectedValue.Trim
            dr("vHabitDetails") = Me.rblhabitdetail.SelectedItem.Text.Trim


            dtOld.Rows.Add(dr)
            Me.ViewState("dtSubjectMst") = dtOld
            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsave.Click
        Dim Dt As New DataTable
        Dim Ds_Subjectmst As DataSet
        Dim ds_doctypegrid As New DataSet
        Dim eStr As String = ""
        Dim subjectId As String = ""
        Dim objOPws As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
        Try
            AssignUpdatedValues()
            Ds_Subjectmst = New DataSet
            Ds_Subjectmst.Tables.Add(CType(Me.ViewState("dtSubjectMst"), Data.DataTable).Copy())
            Ds_Subjectmst.Tables(0).TableName = "View_SubjectMaster"   ' New Values on the form to be updated
            'View_SubjectMaster
            If Not objOPws.Save_SubjectMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, Ds_Subjectmst, "1", subjectId, eStr, "") Then
                ShowErrorMessage(eStr, "")
                Exit Sub
            Else
                ObjCommon.ShowAlert("Record Saved Successfully", Me)
                Response.Redirect("frmPifMst.aspx")
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr)
        End Try
    End Sub
#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'CType(Master.FindControl("lblMandatory"), Label).Text = ""
        CType(Master.FindControl("lblHeading"), Label).Text = "Subject Master"
        If Not IsPostBack Then
            GenCall()
            Filldropdown()
            Exit Sub
        End If
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

    Protected Sub lnkfemaledt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkfemaledt.Click
        ' divfemaledetail.Visible = True
        'div2.Visible = False
        'Response.Redirect("frmfemaledetail.aspx")
    End Sub
    '#Region "assign updatevalues1"
    '    Private Function AssignUpdatedValues1() As Boolean
    '        Dim dtOld As New DataTable
    '        Dim dr As DataRow
    '        Dim strarry(4) As String
    '        Dim j As Integer = 0
    '        Try
    '            dtOld = Me.ViewState("dtSubjectFemaleDetails")
    '            dr = dtOld.NewRow

    '            dr("vScreenId") = "0000000000"
    '            dr("dLastMenstrualDate") = Me.txtmendate.Text
    '            dr("dLastDelivaryDate") = Me.txtdold.Text
    '            dr("iLastMenstrualDays") = Me.txtcyclelength.Text
    '            dr("vGravida") = Me.txtgravida.Text
    '            dr("iNoOfChildren") = Me.txtnoofchildren.Text
    '            dr("cAbortions") = Me.txtabortion.Text
    '            dr("dAbortionDate") = Me.txtdolabortion.Text
    '            dr("vPara") = Me.txtpara.Text
    '            dr("cLoctating") = Me.txtLactating.Text
    '            If rblchildrenhealthy.SelectedItem.Value = "Y" Then
    '                dr("vchildrenHealthRemark") = "Yes"
    '            ElseIf rblchildrenhealthy.SelectedItem.Value = "N" Then
    '                dr("vchildrenHealthRemark") = "No"
    '            End If
    '            If rblcontraception.SelectedItem.Value = "P" Then
    '                dr("cContraception") = "Permanent"
    '            ElseIf rblcontraception.SelectedItem.Value = "T" Then
    '                dr("cContraception") = "Temporary"
    '            End If

    '            For Each item As ListItem In rblcurrent.Items
    '                If item.Selected Then
    '                    strarry(j) = item.Text
    '                    j += 1
    '                End If
    '            Next item
    '            dr("cBarrier") = strarry(0)
    '            dr("cPills") = strarry(1)
    '            dr("cRhythm") = strarry(2)
    '            dr("cIUCD") = strarry(3)

    '            'If rblcurrent.SelectedItem.Value = "B" Then
    '            '    dr("cBarrier") = "C"
    '            'ElseIf rblcurrent.SelectedItem.Value = "P" Then
    '            '    dr("cPills") = "C"
    '            'ElseIf rblcurrent.SelectedItem.Value = "R" Then
    '            '    dr("cRhythm") = "C"
    '            'ElseIf rblcurrent.SelectedItem.Value = "I" Then
    '            '    dr("cIUCD") = "C"
    '            'End If

    '            If rblpast.SelectedItem.Value = "B" Then
    '                dr("cBarrier") = "P"
    '            ElseIf rblcurrent.SelectedItem.Value = "P" Then
    '                dr("cPills") = "P"
    '            ElseIf rblcurrent.SelectedItem.Value = "R" Then
    '                dr("cRhythm") = "P"
    '            ElseIf rblcurrent.SelectedItem.Value = "I" Then
    '                dr("cIUCD") = "P"
    '            End If
    '            dtOld.Rows.Add(dr)
    '            Me.ViewState("dtSubjectFemaleDetails") = dtOld
    '            Return True

    '        Catch ex As Exception
    '            ShowErrorMessage(ex.Message, "")
    '            Return False

    '        End Try
    '    End Function
    '#End Region

    '#Region "save femaledetails"
    '    Protected Sub Btnsavenback_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btnsavenback.Click
    '        Dim Dt As New DataTable
    '        Dim Ds_Subjectfemaledtl As DataSet
    '        Dim ds_doctypegrid As New DataSet
    '        Dim eStr As String = ""
    '        Dim objOPws As New WS_Lambda.WS_Lambda
    '        Try

    '            AssignUpdatedValues1()
    '            Ds_Subjectfemaledtl = New DataSet
    '            Ds_Subjectfemaledtl.Tables.Add(CType(Me.ViewState("dtSubjectFemaleDetails"), Data.DataTable).Copy())
    '            Ds_Subjectfemaledtl.Tables(0).TableName = "SubjectFemaleDetails"   ' New Values on the form to be updated

    '            If Not objOPws.save_subjectFemaleDetails(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_SubjectFemaleDetails, Ds_Subjectfemaledtl, "1", eStr) Then
    '                ShowErrorMessage(eStr, "")
    '                Exit Sub
    '            Else
    '                ObjCommon.ShowAlert("Record Saved Successfully", Me)
    '                'Response.Redirect("frmpifMst.aspx")
    '            End If
    '        Catch ex As Exception
    '            ShowErrorMessage(ex.Message, eStr)
    '        End Try

    '    End Sub
    '#End Region
End Class
