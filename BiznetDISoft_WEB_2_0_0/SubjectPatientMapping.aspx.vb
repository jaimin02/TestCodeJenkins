

Partial Class SubjectPatientMapping
    Inherits System.Web.UI.Page

    Private Const GVC_SubjectPatient_rbSubject As Integer = 1
    Private Const GVC_SubjectPatient_Subject As Integer = 2
    Private Const GVC_SubjectPatient_rbPatient As Integer = 3
    Private Const GVC_SubjectPatient_Patient As Integer = 4
    Private Const GVC_SubjectPatient_vSubjectID As Integer = 5
    Private Const GVC_SubjectPatient_vPatientID As Integer = 6

    Private Const GVC_Mapped_Delete As Integer = 1
    Private Const GVC_Mapped_Subject As Integer = 2
    Private Const GVC_Mapped_Patient As Integer = 3
    Private Const GVC_Mapped_vSubjectID As Integer = 4
    Private Const GVC_Mapped_vPatientID As Integer = 5
    Private Const GVC_Mapped_nSubjectPatient As Integer = 6
    

    Private Const VS_WorkSpaceID As String = "WorkSpaceID"
    Private Const VS_Deleted As String = "DeletedMapping"
    Private Const VS_MappedResult As String = "MappedResult"
    Private Const VS_CurrMapped As String = "Current Mapping"
    Private Const VS_SubjectPatientMapping As String = "SubjectPatientMapping"
    Private dsMapped As DataSet

    Private objCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GenCall()
    End Sub
    Private Sub GenCall()

    End Sub


    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim dsSubjectPatientMapping As New Data.DataSet
        Dim dsCurrentMapping As New DataSet

        dsSubjectPatientMapping = objHelp.GetResultSet("Select * from View_WorkSpaceSubjectPatientMst  where vWorkSpaceId='0000001842' And nSubjectPatientMapping is NULL", "View_WorkSpaceSubjectPatientMst")
        dsMapped = objHelp.GetResultSet("Select * from View_SubjectPatientMapping where vWorkSpaceId='0000001842' And cStatusIndi<>'D'", "View_SubjectPatientMapping")
        ViewState(VS_MappedResult) = dsMapped
        dsCurrentMapping.Tables.Add(dsMapped.Tables(0).Clone())
        ViewState(VS_SubjectPatientMapping) = dsSubjectPatientMapping
        ViewState(VS_CurrMapped) = dsCurrentMapping
        If dsMapped.Tables(0).Rows.Count > 0 Then
            BindMapResult()
        End If
        BindSubjectPatientGrid()

    End Sub



    Protected Sub gvSubjectPatient_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSubjectPatient.PageIndexChanging
        gvSubjectPatient.PageIndex = e.NewPageIndex
        BindSubjectPatientGrid()
    End Sub

    Protected Sub gvSubjectPatient_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSubjectPatient.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or _
                  e.Row.RowType = DataControlRowType.Header Or _
                  e.Row.RowType = DataControlRowType.Footer Then


            e.Row.Cells(GVC_SubjectPatient_vPatientID).Visible = False
            e.Row.Cells(GVC_SubjectPatient_vSubjectID).Visible = False
        End If
    End Sub

    Protected Sub gvSubjectPatient_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSubjectPatient.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("rbSubjectID"), RadioButton).Attributes.Add("OnClick", "UncheckOthers(this);")
            CType(e.Row.FindControl("rbPatientID"), RadioButton).Attributes.Add("OnClick", "UncheckOthers(this);")
            e.Row.Cells(0).Text = e.Row.RowIndex + (Me.gvSubjectPatient.PageSize * Me.gvSubjectPatient.PageIndex) + 1
        End If
        'If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Or DataControlRowType.Header Then
        '    e.Row.Cells(GVC_SubjectPatient_vPatientID).Attributes.Add("style", "display:none")
        '    e.Row.Cells(GVC_SubjectPatient_vSubjectID).Attributes.Add("style", "display:none")
        'End If
       
    End Sub

    Protected Sub btnMap_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMap.Click
        If ViewState(VS_MappedResult) Is Nothing Then
            ViewState(VS_CurrMapped) = objHelp.GetResultSet("Select * from  SubjectPatientMapping where 1=2", "SubjectPatientMapping")
        End If
        Dim SubjectID As String = String.Empty
        Dim PatientID As String = String.Empty
        Dim Subject As String = String.Empty
        Dim Patient As String = String.Empty
        For Each dr As GridViewRow In gvSubjectPatient.Rows

            If CType(dr.FindControl("rbPatientID"), RadioButton).Checked = True Then
                PatientID = dr.Cells(GVC_SubjectPatient_vPatientID).Text.ToString()
                Patient = dr.Cells(GVC_SubjectPatient_Patient).Text.ToString()
            End If
            If CType(dr.FindControl("rbSubjectId"), RadioButton).Checked = True Then
                SubjectID = dr.Cells(GVC_SubjectPatient_vSubjectID).Text.ToString()
                Subject = dr.Cells(GVC_SubjectPatient_Subject).Text.ToString()
            End If
        Next

        If SubjectID.Trim.Length > 5 And PatientID.Trim.Length > 5 Then
            ''Assign Values To CurrentMapping
            Dim dsSubjectIDPatientID As New DataSet
            Dim dr As DataRow
            dsSubjectIDPatientID = CType(ViewState(VS_CurrMapped), DataSet)
            dr = dsSubjectIDPatientID.Tables(0).NewRow()
            dr("vWorkSpaceId") = "0000001842" 'ViewState(VS_WorkSpaceID).ToString.Trim
            dr("vSubjectId") = SubjectID
            dr("vPatientId") = PatientID
            dr("Subject") = Subject
            dr("Patient") = Patient
            dr("cStatusIndi") = "N"
            dsSubjectIDPatientID.Tables(0).Rows.Add(dr)
            dsSubjectIDPatientID.Tables(0).AcceptChanges()
            ViewState(VS_CurrMapped) = dsSubjectIDPatientID

            ''Add rows to existing of Mapped Result table
            Dim dsMapped As New DataSet
            Dim drMapped As DataRow
            dsMapped = CType(ViewState(VS_MappedResult), DataSet)
            drMapped = dsMapped.Tables(0).NewRow()
            drMapped("vSubjectID") = SubjectID
            drMapped("vPatientID") = PatientID
            drMapped("Subject") = Subject
            drMapped("Patient") = Patient
            drMapped("cStatusIndi") = "N"
            dsMapped.Tables(0).Rows.Add(drMapped)
            dsMapped.Tables(0).AcceptChanges()
            ViewState(VS_MappedResult) = dsMapped
            BindMapResult()
        End If



    End Sub

    Private Sub BindMapResult()
        gvMappedResult.Visible = True
        gvMappedResult.DataSource = CType(ViewState(VS_MappedResult), DataSet).Tables(0)
        gvMappedResult.DataBind()

    End Sub
    Private Sub BindSubjectPatientGrid()
        gvSubjectPatient.DataSource = CType(ViewState(VS_SubjectPatientMapping), DataSet).Tables(0)
        gvSubjectPatient.DataBind()
    End Sub

    Protected Sub btnSaveChanges_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveChanges.Click
        Dim dsSave As DataSet
        Dim dsDelete As DataSet
        Dim eStr As String = String.Empty
        Dim o As New Object
        Dim e1 As System.EventArgs
        ' dsSave = ViewState(VS_CurrMapped)
        If Not ViewState(VS_CurrMapped) Is Nothing AndAlso CType(ViewState(VS_CurrMapped), DataSet).Tables(0).Rows.Count > 1 Then
            dsSave = CType(ViewState(VS_CurrMapped), DataSet)
            dsSave.Tables(0).TableName = "SubjectPatientMapping"
            dsSave.Tables(0).Columns.Remove("Subject")
            dsSave.Tables(0).Columns.Remove("Patient")

            dsSave.AcceptChanges()
            If Not objLambda.Save_SubjectPatientMapping(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, dsSave, 1, eStr) Then

            End If
            dsSave.Tables(0).Rows.Clear()
            dsSave.AcceptChanges()
            ViewState(VS_CurrMapped) = dsSave

        ElseIf Not ViewState(VS_Deleted) Is Nothing AndAlso CType(ViewState(VS_Deleted), DataSet).Tables(0).Rows.Count > 0 Then
            dsDelete = CType(ViewState(VS_Deleted), DataSet)
            For Each dr As DataRow In dsDelete.Tables(0).Rows
                dr("cStatusIndi") = "D"
            Next
            dsDelete.Tables(0).TableName = "SubjectPatientMapping"
            dsDelete.Tables(0).Columns.Remove("Subject")
            dsDelete.Tables(0).Columns.Remove("Patient")

            dsDelete.AcceptChanges()
            If Not objLambda.Save_SubjectPatientMapping(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, dsDelete, Session(S_UserID), eStr) Then

            End If
            dsDelete.Tables(0).Rows.Clear()
            dsDelete.AcceptChanges()
            ViewState(VS_Deleted) = dsDelete
        Else
            Exit Sub
        End If
        ''Clear Current Added table
       
        ''
        btnSetProject_Click(o, e1)
    End Sub

    Protected Sub gvMappedResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvMappedResult.PageIndexChanging
        gvMappedResult.PageIndex = e.NewPageIndex
        BindMapResult()
    End Sub

    Protected Sub gvMappedResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMappedResult.RowCommand
        'Dim i As Integer = e.CommandArgument
        Dim dsCurrent As New DataSet
        Dim dsMappedResult_AfterDeletion As DataSet
        Dim dsDeletion As New DataSet
        If e.CommandName.ToUpper = "DELETE" Then
            ''First Delete From Current Mapping If Exist In That
            If Not ViewState(VS_CurrMapped) Is Nothing Then
                dsCurrent = ViewState(VS_CurrMapped)
                For Each dr As DataRow In dsCurrent.Tables(0).Select("nSubjectPatientMapping = '" & e.CommandArgument.ToString & "'")
                    dsCurrent.Tables(0).Rows.Remove(dr)
                Next
                dsCurrent.AcceptChanges()
                ViewState(VS_CurrMapped) = dsCurrent
            End If
            
            ''Then Check in Mapped Result Table And Remove from there  and Also
            ''Prepare a Data Table To send  Delete command To database on Save Changes Button
            dsMappedResult_AfterDeletion = CType(ViewState(VS_MappedResult), DataSet)
            If ViewState(VS_Deleted) Is Nothing Then
                dsDeletion.Tables.Add(dsMappedResult_AfterDeletion.Tables(0).Clone)
            Else
                dsDeletion = CType(ViewState(VS_Deleted), DataSet)
            End If

            For Each drMaped As DataRow In CType(ViewState(VS_MappedResult), DataSet).Tables(0).Select("nSubjectPatientMapping = '" & e.CommandArgument.ToString & "'")

                dsDeletion.Tables(0).Rows.Add(drMaped.ItemArray)
                dsMappedResult_AfterDeletion.Tables(0).Rows.Remove(drMaped)
            Next
            dsDeletion.AcceptChanges()
            dsMappedResult_AfterDeletion.AcceptChanges()
            ViewState(VS_MappedResult) = dsMappedResult_AfterDeletion
            ViewState(VS_Deleted) = dsDeletion
            BindMapResult()


        End If
    End Sub

    Protected Sub gvMappedResult_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMappedResult.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or _
                  e.Row.RowType = DataControlRowType.Header Or _
                  e.Row.RowType = DataControlRowType.Footer Then

            'e.Row.Cells(GVC_SubjectId).Visible = False
            e.Row.Cells(GVC_Mapped_vPatientID).Visible = False
            e.Row.Cells(GVC_Mapped_vSubjectID).Visible = False
            e.Row.Cells(GVC_Mapped_nSubjectPatient).Visible = False
        End If
    End Sub

    Protected Sub gvMappedResult_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMappedResult.RowDataBound
        'If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Footer Or DataControlRowType.Header Then
        '    e.Row.Cells(GVC_Mapped_nSubjectPatient).Attributes.Add("style", "display:none")
        '    e.Row.Cells(GVC_Mapped_vPatientID).Attributes.Add("style", "display:none")
        '    e.Row.Cells(GVC_Mapped_vSubjectID).Attributes.Add("style", "display:none")
        'End If
       
        If e.Row.RowType = DataControlRowType.DataRow Then

            e.Row.Cells(0).Text = e.Row.RowIndex + (Me.gvMappedResult.PageSize * Me.gvMappedResult.PageIndex) + 1
            CType(e.Row.FindControl("btnDelete"), Button).CommandArgument = e.Row.Cells(GVC_Mapped_nSubjectPatient).Text
            CType(e.Row.FindControl("btnDelete"), Button).CommandName = "DELETE"
        End If
    End Sub

    Protected Sub gvMappedResult_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvMappedResult.RowDeleting

    End Sub
End Class
