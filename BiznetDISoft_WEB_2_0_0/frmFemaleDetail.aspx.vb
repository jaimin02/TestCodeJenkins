
Partial Class frmFemaleDetail
    Inherits System.Web.UI.Page

    Dim ObjCommon As New clsCommon
    Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

  
#Region "GenCall"

    Private Function GenCall() As Boolean
        Dim dt_SubjectFemaleDetails As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Try
            Choice = CType("1", WS_Lambda.DataObjOpenSaveModeEnum)
            Me.ViewState("Choice") = Choice   'To use it while saving
            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState("pScreenId") = Me.Request.QueryString("Value").ToString
            End If
            ''''Check for Valid User''''''''''''''
            If Not GenCall_Data(Choice, dt_SubjectFemaleDetails) Then ' For Data Retrieval
                Exit Function
            End If
            Me.ViewState("dtSubjectFemaleDetails") = dt_SubjectFemaleDetails 'adding blank DataTable in viewstate
            If Not GenCall_ShowUI(Choice, dt_SubjectFemaleDetails) Then 'For Displaying Data 
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
        Dim ds_SubjectFemaleDetails As DataSet = Nothing
        'Dim objHelp As New WS_HelpDbTable.WS_HelpDbTable
        Try

            If Choice_1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "pScreenId=" + Me.ViewState("pScreenId").ToString() 'Value of where condition
            End If
            If Not objHelp.GetSubjectFemaleDetails(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubjectFemaleDetails, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_SubjectFemaleDetails Is Nothing Then
                Throw New Exception(eStr)
            End If
            If ds_SubjectFemaleDetails.Tables(0).Rows.Count <= 0 And _
               Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                Throw New Exception("No Records Found for Selected role")

            End If
            dt_Dist_Retu = ds_SubjectFemaleDetails.Tables(0)
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
        Dim dt_Subjdetmst As DataTable = Nothing

        Page.Title = " :: Female Detail ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
        If Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
            'Me.txtdob.Text = ConvertDbNullToDbTypeDefaultValue(dt_Subjdetmst.Rows(0).Item("vFirstName"), dt_Subjdetmst.Rows(0).Item("vFirstName").GetType)
            Me.txtmendate.Text = ConvertDbNullToDbTypeDefaultValue(dt_Subjdetmst.Rows(0).Item("dLastMenstrualDate"), dt_Subjdetmst.Rows(0).Item("dLastMenstrualDate").GetType)
            'Me.txtcyclelength.Text = ConvertDbNullToDbTypeDefaultValue(dt_Subjdetmst.Rows(0).Item("vMiddleName"), dt_Subjdetmst.Rows(0).Item("vMiddleName").GetType)
            Me.txtdold.Text = ConvertDbNullToDbTypeDefaultValue(dt_Subjdetmst.Rows(0).Item("dLastDelivaryDate"), dt_Subjdetmst.Rows(0).Item("dLastDelivaryDate").GetType)
            'Me.txtdob.Text = ConvertDbNullToDbTypeDefaultValue(dt_Subjdetmst.Rows(0).Item("dBirthDate"), dt_Subjdetmst.Rows(0).Item("dBirthDate").GetType)
            Me.txtgravida.Text = ConvertDbNullToDbTypeDefaultValue(dt_Subjdetmst.Rows(0).Item("vGravida"), dt_Subjdetmst.Rows(0).Item("vGravida").GetType)
            Me.txtnoofchildren.Text = ConvertDbNullToDbTypeDefaultValue(dt_Subjdetmst.Rows(0).Item("iNoOfChildren"), dt_Subjdetmst.Rows(0).Item("iNoOfChildren").GetType)
            Me.txtabortion.Text = ConvertDbNullToDbTypeDefaultValue(dt_Subjdetmst.Rows(0).Item("cAbortions"), dt_Subjdetmst.Rows(0).Item("cAbortions").GetType)
            Me.txtdolabortion.Text = ConvertDbNullToDbTypeDefaultValue(dt_Subjdetmst.Rows(0).Item("dAbortionDate"), dt_Subjdetmst.Rows(0).Item("dAbortionDate").GetType)
            Me.txtpara.Text = ConvertDbNullToDbTypeDefaultValue(dt_Subjdetmst.Rows(0).Item("vPara"), dt_Subjdetmst.Rows(0).Item("vPara").GetType)
            Me.txtLactating.Text = ConvertDbNullToDbTypeDefaultValue(dt_Subjdetmst.Rows(0).Item("cLoctating"), dt_Subjdetmst.Rows(0).Item("cLoctating").GetType)

        End If
        GenCall_ShowUI = True
    End Function
#End Region
#End Region

#Region "Reset Page"
    Private Sub resetpage()
        Me.txtmendate.Text = ""
        Me.txtdold.Text = ""
        Me.txtgravida.Text = ""
        Me.txtnoofchildren.Text = ""
        Me.txtabortion.Text = ""
        Me.txtdolabortion.Text = ""
        Me.txtpara.Text = ""
        Me.txtLactating.Text = ""
    End Sub

#End Region

#Region "Save"
    Private Function AssignUpdatedValues() As Boolean
        Dim dtOld As New DataTable
        Dim dr As DataRow
        Dim strarry(4) As String
        Dim k As Integer = 0

        Try
            dtOld = Me.ViewState("dtSubjectFemaleDetails")
            dr = dtOld.NewRow

            dr("pScreenId") = "0000000000"
            dr("dLastMenstrualDate") = Me.txtmendate.Text
            dr("dLastDelivaryDate") = Me.txtdold.Text
            dr("iLastMenstrualDays") = Me.txtcyclelength.Text
            dr("vGravida") = Me.txtgravida.Text
            dr("iNoOfChildren") = Me.txtnoofchildren.Text
            dr("cAbortions") = Me.txtabortion.Text
            dr("dAbortionDate") = Me.txtdolabortion.Text
            dr("vPara") = Me.txtpara.Text
            dr("cLoctating") = Me.txtLactating.Text
            If rblchildrenhealthy.SelectedItem.Value = "Y" Then
                dr("vchildrenHealthRemark") = "Yes"
            ElseIf rblchildrenhealthy.SelectedItem.Value = "N" Then
                dr("vchildrenHealthRemark") = "No"
            End If
            If rblcontraception.SelectedItem.Value = "P" Then
                dr("cContraception") = "Permanent"
            ElseIf rblcontraception.SelectedItem.Value = "T" Then
                dr("cContraception") = "Temporary"
            End If

            'For Each item As ListItem In rblcurrent.Items
            '    If item.Selected Then
            '        strarry(k) = item.Text
            '        k += 1
            '    End If
            'Next item
            'dr("cBarrier") = strarry(0)
            'dr("cPills") = strarry(1)
            'dr("cRhythm") = strarry(2)
            'dr("cIUCD") = strarry(3)

            If rblcurrent.SelectedItem.Value = "B" Then
                dr("cBarrier") = "Y"
            ElseIf rblcurrent.SelectedItem.Value = "P" Then
                dr("cPills") = "Y"
            ElseIf rblcurrent.SelectedItem.Value = "R" Then
                dr("cRhythm") = "Y"
            ElseIf rblcurrent.SelectedItem.Value = "I" Then
                dr("cIUCD") = "Y"
            End If

            If rblpast.SelectedItem.Value = "B" Then
                dr("cBarrier") = "Y"
            ElseIf rblcurrent.SelectedItem.Value = "P" Then
                dr("cPills") = "Y"
            ElseIf rblcurrent.SelectedItem.Value = "R" Then
                dr("cRhythm") = "Y"
            ElseIf rblcurrent.SelectedItem.Value = "I" Then
                dr("cIUCD") = "Y"
            End If
            dtOld.Rows.Add(dr)
            Me.ViewState("dtSubjectFemaleDetails") = dtOld
            Return True

        Catch ex As Exception
            ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function
#End Region
#Region "save"
    Protected Sub Btnsavenback_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btnsavenback.Click
        Dim Dt As New DataTable
        Dim Ds_Subjectfemaledtl As DataSet
        Dim ds_doctypegrid As New DataSet
        Dim eStr As String = ""
        Dim objOPws As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
        Try

            AssignUpdatedValues()
            Ds_Subjectfemaledtl = New DataSet
            Ds_Subjectfemaledtl.Tables.Add(CType(Me.ViewState("dtSubjectFemaleDetails"), Data.DataTable).Copy())
            Ds_Subjectfemaledtl.Tables(0).TableName = "SubjectFemaleDetails"   ' New Values on the form to be updated

            If Not objOPws.save_subjectFemaleDetails(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_SubjectFemaleDetails, Ds_Subjectfemaledtl, "1", eStr) Then
                ShowErrorMessage(eStr, "")
                Exit Sub
            Else
                ObjCommon.ShowAlert("Record Saved Successfully", Me)
                Response.Redirect("frmpifMst.aspx")
            End If
        Catch ex As Exception
            ShowErrorMessage(ex.Message, eStr)
        End Try
    End Sub
#End Region
#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'CType(Master.FindControl("lblMandatory"), Label).Text = ""
        CType(Master.FindControl("lblHeading"), Label).Text = "Subject Female Detail"
        If Not IsPostBack Then
            GenCall()
            'Filldropdown()
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



End Class
