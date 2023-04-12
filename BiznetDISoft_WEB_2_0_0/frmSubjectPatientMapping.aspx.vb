Imports System.Web.Services

Partial Class frmSubjectPatientMapping
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private Const GVC_SubjectPatient_rbSubject As Integer = 1
    Private Const GVC_SubjectPatient_Subject As Integer = 2
    Private Const GVC_SubjectPatient_rbPatient As Integer = 3
    Private Const GVC_SubjectPatient_Patient As Integer = 4
    Private Const GVC_SubjectPatient_vSubjectID As Integer = 5
    Private Const GVC_SubjectPatient_vPatientID As Integer = 6
    Private Const GVC_SubjectPatient_LabScrNo As Integer = 7

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
    Private Const VS_MappedID As String = "nSubjectPatientMapping"
    Private dsMapped As DataSet
    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

#End Region
    
#Region "Page Load Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            GenCall()
            ViewState("nSubjectPatientMapping") = 1
        End If

    End Sub
    Private Sub GenCall()

        Dim Sender As Object
        Dim e As EventArgs
        Try

            If (Not Session(S_ProjectId) Is Nothing) AndAlso (Not Session(S_ProjectName) Is Nothing) Then
                Me.txtproject.Text = Session(S_ProjectName)
                Me.HProjectId.Value = Session(S_ProjectId)
                btnSetProject_Click(Sender, e)
            End If
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID)
            CType(Master.FindControl("lblHeading"), Label).Text = "Subject Patient Mapping"
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            Page.Title = ":: Subject Patient Mapping :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
        Catch ex As Exception

        End Try


    End Sub
#End Region
    
#Region "Binding Grids"
    Private Sub BindMapResult()

        gvMappedResult.Visible = True
        gvMappedResult.DataSource = CType(ViewState(VS_MappedResult), DataSet).Tables(0)
        gvMappedResult.DataBind()
        If gvMappedResult.Rows.Count > 0 Then
            btnSaveChanges.Visible = True
        Else
            btnSaveChanges.Visible = False
        End If

    End Sub

    Private Sub BindSubjectPatientGrid()

        gvSubjectPatient.DataSource = CType(ViewState(VS_SubjectPatientMapping), DataSet).Tables(0)
        gvSubjectPatient.DataBind()
        If gvSubjectPatient.Rows.Count > 0 Then
            btnMap.Visible = True
        Else
            ObjCommon.ShowAlert("This Project Have No Data Avaialable to Map!", Me.Page)
            btnMap.Visible = False
        End If
    End Sub
#End Region
    
#Region "SubjectPatient Grid Events"
    Protected Sub gvSubjectPatient_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvSubjectPatient.PageIndexChanging
        gvSubjectPatient.PageIndex = e.NewPageIndex
        BindSubjectPatientGrid()
    End Sub

    Protected Sub gvSubjectPatient_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSubjectPatient.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Or _
                  e.Row.RowType = DataControlRowType.Header Or _
                  e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(GVC_SubjectPatient_LabScrNo).Visible = False
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
#End Region

#Region "Mapped-Result Grid Events"
    Protected Sub gvMappedResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvMappedResult.PageIndexChanging
        gvMappedResult.PageIndex = e.NewPageIndex
        BindMapResult()
    End Sub

    Protected Sub gvMappedResult_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMappedResult.RowCommand
        Dim index As Integer = e.CommandArgument
        Dim dsCurrent As New DataSet
        Dim dsMappedResult_AfterDeletion As DataSet
        Dim dsDeletion As New DataSet
        Dim iSubjectPatientMapping As Int64
        If e.CommandName.ToUpper = "DELETE" Then
            iSubjectPatientMapping = Me.gvMappedResult.Rows(index).Cells(GVC_Mapped_nSubjectPatient).Text.Trim
            If Not IsNumeric(iSubjectPatientMapping) Then

            End If
            ''First Delete From Current Mapping If Exist In That
            If Not ViewState(VS_CurrMapped) Is Nothing Then
                dsCurrent = ViewState(VS_CurrMapped)
                For Each dr As DataRow In dsCurrent.Tables(0).Select("nSubjectPatientMapping = '" & iSubjectPatientMapping & "'")
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

            For Each drMaped As DataRow In CType(ViewState(VS_MappedResult), DataSet).Tables(0).Select("nSubjectPatientMapping = '" & iSubjectPatientMapping & "'  And dModifyOn is not Null")

                dsDeletion.Tables(0).Rows.Add(drMaped.ItemArray)
                'dsMappedResult_AfterDeletion.Tables(0).Rows.Remove(drMaped)
            Next
            For Each drMaped As DataRow In CType(ViewState(VS_MappedResult), DataSet).Tables(0).Select("nSubjectPatientMapping = '" & iSubjectPatientMapping & "'")

                'dsDeletion.Tables(0).Rows.Add(drMaped.ItemArray)
                dsMappedResult_AfterDeletion.Tables(0).Rows.Remove(drMaped)
            Next
            dsDeletion.AcceptChanges()
            dsMappedResult_AfterDeletion.AcceptChanges()
            ViewState(VS_MappedResult) = dsMappedResult_AfterDeletion
            ViewState(VS_Deleted) = dsDeletion
            BindMapResult()
            btnSaveChanges.Visible = True

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
            'CType(e.Row.FindControl("btnDelete"), Button).CommandArgument = e.Row.Cells(GVC_Mapped_nSubjectPatient).Text
            'CType(e.Row.FindControl("btnDelete"), Button).CommandArgument = e.Row.RowIndex
            'CType(e.Row.FindControl("btnDelete"), Button).CommandName = "DELETE"

            CType(e.Row.FindControl("btnDelete"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("btnDelete"), ImageButton).CommandName = "DELETE"

        End If
    End Sub

    Protected Sub gvMappedResult_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvMappedResult.RowDeleting

    End Sub
#End Region

#Region "Button Events"

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Dim dsSubjectPatientMapping As New Data.DataSet
        Dim dsCurrentMapping As New DataSet
        Try
            ViewState(VS_MappedResult) = Nothing
            dsSubjectPatientMapping = objHelp.GetResultSet("Select * from View_WorkSpaceSubjectPatientMst  where vWorkSpaceId='" & Me.HProjectId.Value.Trim() & "' And nSubjectPatientMapping is NULL", "View_WorkSpaceSubjectPatientMst")
            dsMapped = objHelp.GetResultSet("Select * from View_SubjectPatientMapping where vWorkSpaceId='" & Me.HProjectId.Value.Trim() & "'   And cStatusIndi<>'D'", "View_SubjectPatientMapping")
            ViewState(VS_MappedResult) = dsMapped
            dsCurrentMapping.Tables.Add(dsMapped.Tables(0).Clone())
            ViewState(VS_SubjectPatientMapping) = dsSubjectPatientMapping
            ViewState(VS_CurrMapped) = dsCurrentMapping
            If dsMapped.Tables(0).Rows.Count > 0 Then
                BindMapResult()
            Else
                gvMappedResult.Visible = False
            End If
            BindSubjectPatientGrid()
            btnSaveChanges.Visible = False
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try

    End Sub

    Protected Sub btnMap_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMap.Click
        Try
            If ViewState(VS_MappedResult) Is Nothing Then
                ViewState(VS_CurrMapped) = objHelp.GetResultSet("Select * from  SubjectPatientMapping where 1=2", "SubjectPatientMapping")
            End If
            Dim SubjectID As String = String.Empty
            Dim PatientID As String = String.Empty
            Dim Subject As String = String.Empty
            Dim Patient As String = String.Empty
            Dim LabScrNo As String = String.Empty
            For Each dr As GridViewRow In gvSubjectPatient.Rows

                If CType(dr.FindControl("rbPatientID"), RadioButton).Checked = True Then
                    PatientID = dr.Cells(GVC_SubjectPatient_vPatientID).Text.ToString()
                    Patient = dr.Cells(GVC_SubjectPatient_Patient).Text.ToString()
                    LabScrNo = dr.Cells(GVC_SubjectPatient_LabScrNo).Text.ToString()
                End If
                If CType(dr.FindControl("rbSubjectId"), RadioButton).Checked = True Then
                    SubjectID = dr.Cells(GVC_SubjectPatient_vSubjectID).Text.ToString()
                    Subject = dr.Cells(GVC_SubjectPatient_Subject).Text.ToString()

                End If
            Next

            '---------------------------------------
            For Each dr As GridViewRow In gvMappedResult.Rows
                If SubjectID = dr.Cells(GVC_Mapped_vSubjectID).Text.ToString() Then
                    ObjCommon.ShowAlert("You have already mapped this subject and patient!", Me.Page)
                    Exit Sub
                End If
            Next
            '---------------------------------------

            If Not Subject.Contains(LabScrNo.Trim) Then
                ObjCommon.ShowAlert("You Can Not Map Subject And Patient With Diffrent Screening No.!", Me.Page)
                Exit Sub
            End If
            If SubjectID.Trim.Length > 5 And PatientID.Trim.Length > 5 Then
                ''Assign Values To CurrentMapping
                Dim dsSubjectIDPatientID As New DataSet
                Dim dr As DataRow
                ViewState(VS_MappedID) = ViewState(VS_MappedID) + 1
                dsSubjectIDPatientID = CType(ViewState(VS_CurrMapped), DataSet)
                dr = dsSubjectIDPatientID.Tables(0).NewRow()
                dr("vWorkSpaceId") = Me.HProjectId.Value.Trim() 'ViewState(VS_WorkSpaceID).ToString.Trim
                dr("vSubjectId") = SubjectID
                dr("vPatientId") = PatientID
                dr("Subject") = Subject
                dr("Patient") = Patient
                dr("cStatusIndi") = "N"
                dr("iUserId") = Session(S_UserID)
                dr("nSubjectPatientMapping") = ViewState(VS_MappedID)
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
                dr("iUserId") = Session(S_UserID)

                drMapped("nSubjectPatientMapping") = ViewState(VS_MappedID)
                dsMapped.Tables(0).Rows.Add(drMapped)
                dsMapped.Tables(0).AcceptChanges()
                ViewState(VS_MappedResult) = dsMapped
                BindMapResult()
            Else
                ObjCommon.ShowAlert("Invalid Subject Or Patient IDs!", Me.Page)
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")

        End Try


    End Sub

    Protected Sub btnSaveChanges_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveChanges.Click
        Dim dsSave As DataSet
        Dim dsDelete As DataSet
        Dim eStr As String = String.Empty
        Dim o As New Object
        Dim e1 As System.EventArgs
        Dim bChanged As Boolean = False
        ' dsSave = ViewState(VS_CurrMapped)
        Try
            If Not ViewState(VS_CurrMapped) Is Nothing AndAlso CType(ViewState(VS_CurrMapped), DataSet).Tables(0).Rows.Count > 0 Then
                dsSave = CType(ViewState(VS_CurrMapped), DataSet).Copy()
                dsSave.Tables(0).TableName = "SubjectPatientMapping"
                dsSave.Tables(0).Columns.Remove("Subject")
                dsSave.Tables(0).Columns.Remove("Patient")

                dsSave.AcceptChanges()
                If Not objLambda.Save_SubjectPatientMapping(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, dsSave, 1, eStr) Then
                    Me.ShowErrorMessage(eStr, "")
                    Exit Sub
                End If
                dsSave.Tables(0).Rows.Clear()
                dsSave.AcceptChanges()
                ' ViewState(VS_CurrMapped) = dsSave

                CType(ViewState(VS_CurrMapped), DataSet).Tables(0).Rows.Clear()
                ViewState(VS_CurrMapped) = CType(ViewState(VS_CurrMapped), DataSet)
                bChanged = True

            End If
            If Not ViewState(VS_Deleted) Is Nothing AndAlso CType(ViewState(VS_Deleted), DataSet).Tables(0).Rows.Count > 0 Then
                dsDelete = CType(ViewState(VS_Deleted), DataSet).Copy()
                For Each dr As DataRow In dsDelete.Tables(0).Rows
                    dr("cStatusIndi") = "D"
                Next
                dsDelete.Tables(0).TableName = "SubjectPatientMapping"
                dsDelete.Tables(0).Columns.Remove("Subject")
                dsDelete.Tables(0).Columns.Remove("Patient")

                dsDelete.AcceptChanges()
                If Not objLambda.Save_SubjectPatientMapping(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, dsDelete, Session(S_UserID), eStr) Then
                    Me.ShowErrorMessage(eStr, "")
                    Exit Sub
                End If
                dsDelete.Tables(0).Rows.Clear()
                dsDelete.AcceptChanges()

                CType(ViewState(VS_Deleted), DataSet).Tables(0).Rows.Clear()
                ViewState(VS_Deleted) = CType(ViewState(VS_Deleted), DataSet)
                bChanged = True
            End If
            ''Clear Current Added table

            ''
            If Not bChanged Then
                ObjCommon.ShowAlert("No Changes Found To Save !", Me.Page)
                Exit Sub
            End If
            btnSetProject_Click(o, e1)
            ObjCommon.ShowAlert("Records Saved Sucessfully !", Me.Page)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            ' Return False
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

End Class
