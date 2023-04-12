
Partial Class frmMasterAuditTrail
    Inherits System.Web.UI.Page
#Region " Variable Declaration"

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

    Dim eStr As String
    Dim eStr_Retu As String

    Private Const VS_Choice As String = "Choice"
    Private Const VS_MasterAuditTrail As String = "MasterAuditTrail"
    Private Const VS_WhereCond As String = "WhereCondition"
    Private Const Vs_TableName As String = "TableName"


    Private Const VS_CreatedBy As String = "LastCreatedBy"
    Private Const VS_ModifyBy As String = "LastModifyBy"
    Private Const VS_CreatedOn As String = "LastCreatedOn"
    Private Const vs_ModifyOn As String = "LastModifiedOn"



#End Region

#Region "Load Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If Not Me.IsPostBack Then

                GenCall()

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)

        End Try

    End Sub

#End Region

#Region " GenCall "

    Private Sub GenCall()

        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Dim ds As New DataSet

        Try

            Choice = Me.Request.QueryString("Mode")

            Me.ViewState(VS_Choice) = Choice
            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                'Me.ViewState(VS_ADRReportHdrNo) = Me.Request.QueryString("Value").ToString
            End If


            If Not GenCall_Data(ds) Then ' For Data Retrieval
                Exit Sub
            End If

            GenCall_ShowUI()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub


#Region " GenCall_Data "
    Private Function GenCall_Data(ByRef ds_DWR_Retu As DataSet) As Boolean

        Dim wStr As String = ""
        Dim eStr_Retu As String = ""
        Dim Val As String = ""
        Dim dsBlankADRReportHdr As New DataSet
        Dim dsBlankADRReportDtl As New DataSet
        Dim dsBlankADRReportDocument As New DataSet
        Dim dsDTDInfoMst As New DataSet
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing


        Try

            Val = Me.ViewState(VS_MasterAuditTrail) 'Value of where condition
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                'wStr = "nADRReportHdrNo=" + Val.ToString
            End If


            If dsBlankADRReportHdr Is Nothing Then
                Throw New Exception(eStr_Retu)
            End If

            ds_DWR_Retu = dsBlankADRReportHdr
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")

        Finally

        End Try
    End Function
#End Region

    Private Sub GenCall_ShowUI()
        Try
            Page.Title = ":: Master Audit Trail :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = " Master Audit Trail "
            Me.FillDropdownList()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)

        End Try

    End Sub
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

#Region "Fill DropDownList"

    Private Sub FillDropdownList()

        Dim wStr_UserId As String = "1=2"
        Dim ds_FillUser As New DataSet

        Try

            If Not objHelp.getuserMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_FillUser, eStr_Retu) Then
                Throw New Exception(eStr)
            End If

            Me.ddlCreatedBy.DataValueField = "iuserid"
            Me.ddlcreatedby.DataTextField = "vLoginname"
            Me.ddlcreatedby.DataSource = ds_FillUser.Tables(0)
            Me.ddlcreatedby.DataBind()
            Me.ddlCreatedBy.Items.Insert(0, " -- Select Created By -- ")
            Me.ddlcreatedby.Items.Insert(1, New ListItem("ALL", "-1"))

            Me.ddlModifiedBy.DataValueField = "iuserid"
            Me.ddlModifiedBy.DataTextField = "vLoginname"
            Me.ddlModifiedBy.DataSource = ds_FillUser.Tables(0)
            Me.ddlModifiedBy.DataBind()
            Me.ddlModifiedBy.Items.Insert(0, " -- Select Modified By -- ")
            Me.ddlModifiedBy.Items.Insert(1, New ListItem("ALL", "-1"))


        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
        End Try

    End Sub

#End Region

#Region " Fill Data "

    Private Function FillData(ByVal wstr_Tablename As String, ByVal wStr_FillData As String) As Boolean
        FillData = False
        Dim dt_FillData As New DataTable
        Dim Qstr_Fill As String = String.Empty

        Try

            Qstr_Fill = "Select *  from " + wstr_Tablename + wStr_FillData
            dt_FillData = Me.objHelp.GetResultSet(Qstr_Fill, wstr_Tablename).Tables(0)
            Me.FillMasterGrid(dt_FillData)
            FillData = True


        Catch ex As Exception
            FillData = False
            Me.ShowErrorMessage(ex.Message, eStr)

        End Try

    End Function

#End Region

#Region " Fill Master Grid"

    Private Function FillMasterGrid(ByVal dt_fillGrid As DataTable) As Boolean
        FillMasterGrid = False

        Try

            If dt_fillGrid.Rows.Count > 0 Then
                Me.gvwMasterAudit.DataSource = dt_fillGrid
                Me.gvwMasterAudit.DataBind()
                FillMasterGrid = True
            Else
                Me.gvwMasterAudit.DataSource = dt_fillGrid
                Me.gvwMasterAudit.DataBind()
                objCommon.ShowAlert("No Records founds", Me)
                FillMasterGrid = False
            End If

        Catch ex As Exception
            FillMasterGrid = False
            Me.ShowErrorMessage(ex.Message, eStr)
        End Try

    End Function
#End Region

#Region "Button Events"



    Protected Sub btnViewReport_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim wstr_Tablename As String = ""
        Dim wstr_WhereCon As String = ""
        Dim dt_FillData As New DataTable
        Dim Qstr_Fill As String = String.Empty

        Try

            Me.ViewState(VS_CreatedBy) = Me.ddlCreatedBy.SelectedItem.Value().Trim()
            Me.ViewState(VS_ModifyBy) = Me.ddlModifiedBy.SelectedItem.Value().Trim()

            If ddlMasterName.SelectedItem.Value = "1" Then
                wstr_Tablename = "View_AudittrailDrugMst"
            ElseIf ddlMasterName.SelectedItem.Value = "2" Then
                wstr_Tablename = "View_AudittrailUOMMst"
            ElseIf ddlMasterName.SelectedItem.Value = "3" Then
                wstr_Tablename = "View_AudittrailLocationMSt"
            ElseIf ddlMasterName.SelectedItem.Value = "4" Then
                wstr_Tablename = "View_AudittrailDeptMst"
            ElseIf ddlMasterName.SelectedItem.Value = "5" Then
                wstr_Tablename = "VIEW_AudittrailCountryMst"
            ElseIf ddlMasterName.SelectedItem.Value = "6" Then
                wstr_Tablename = "View_AudittrailSpecialityMst"
            End If

            Me.ViewState(Vs_TableName) = wstr_Tablename

            If Me.ddlCreatedBy.SelectedIndex > 1 Then
                wstr_WhereCon = " Where iCreatedBytemp=" + Me.ddlCreatedBy.SelectedItem.Value.Trim()
            End If

            If Me.ddlModifiedBy.SelectedIndex > 1 Then
                If wstr_WhereCon.ToLower.Contains("where") Then
                    wstr_WhereCon += " AND "
                Else
                    wstr_WhereCon = " WHERE "
                End If
                wstr_WhereCon += "iModifyBytemp = " + Me.ddlModifiedBy.SelectedValue.Trim

            End If

            If Me.txtCreatedOn.Text <> "" Then
                If wstr_WhereCon.ToLower.Contains("where") Then
                    wstr_WhereCon += " AND "
                Else
                    wstr_WhereCon = " WHERE "
                End If
                wstr_WhereCon += " CAST(CONVERT(VARCHAR(11),CreatedOn,113) as datetime) > cast( '" + Me.txtCreatedOn.Text + "' as datetime)"

            End If

            If Me.txtModifiedOn.Text <> "" Then
                If wstr_WhereCon.ToLower.Contains("where") Then
                    wstr_WhereCon += " AND "
                Else
                    wstr_WhereCon = " WHERE "
                End If

                wstr_WhereCon += " CAST(CONVERT(VARCHAR(11),LastModifiedOn,113) as datetime) > cast( '" + Me.txtModifiedOn.Text + "' as datetime)"

            End If

            Me.ViewState(VS_WhereCond) = wstr_WhereCon

            Me.FillData(Me.ViewState(Vs_TableName), Me.ViewState(VS_WhereCond))

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
        End Try
    End Sub

    Protected Sub btncancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.ddlCreatedBy.SelectedIndex = -1
        Me.ddlMasterName.SelectedIndex = -1
        Me.ddlModifiedBy.SelectedIndex = -1
        Me.txtCreatedOn.Text = ""
        Me.txtModifiedOn.Text = ""
        Me.gvwMasterAudit.DataSource = Nothing
        Me.gvwMasterAudit.DataBind()
    End Sub


#End Region

#Region "GridView Events"
    Protected Sub gvwMasterAudit_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Try

            gvwMasterAudit.PageIndex = e.NewPageIndex
            Me.FillData(Me.ViewState(Vs_TableName), Me.ViewState(VS_WhereCond))

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
        End Try

    End Sub

    Protected Sub gvwMasterAudit_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Dim index As Integer
        Dim tempDate As DateTime
        Dim DateFormat As String = ""
        Dim FirstDateFormat As String = ""
        Dim LastDateFormat As String = ""
        Dim LastDateFormatFirst As String = ""

        Try
            If e.Row.RowType = DataControlRowType.DataRow Or _
               e.Row.RowType = DataControlRowType.Header Or _
               e.Row.RowType = DataControlRowType.Footer Then

                e.Row.Cells(0).Visible = False
                e.Row.Cells(1).Visible = False

            End If

            If e.Row.RowType = DataControlRowType.DataRow Then
                For index = 0 To gvwMasterAudit.HeaderRow.Cells.Count - 1
                    If gvwMasterAudit.HeaderRow.Cells(index).Text.Contains("On") Then
                        If Not e.Row.Cells(index).Text Is Nothing AndAlso _
                            Not e.Row.Cells(index).Text.Contains("&nbsp;") Then
                            If DateTime.TryParse(e.Row.Cells(index).Text.Trim, tempDate) Then

                                If e.Row.Cells(index).Text.Length = 4 Then

                                    e.Row.Cells(index).Text = tempDate


                                ElseIf e.Row.Cells(index).Text.Length >= 4 And e.Row.Cells(index).Text.Length <= 7 Then
                                    DateFormat = e.Row.Cells(index).Text

                                    DateFormat = e.Row.Cells(index).Text
                                    tempDate = DateFormat.Substring(0, DateFormat.IndexOf(" ") + 1)
                                    LastDateFormat = DateFormat.Substring(DateFormat.IndexOf(" ") + 1)
                                    e.Row.Cells(index).Text = tempDate.ToString("dd-MMM-yyyy") + " " + LastDateFormat

                                ElseIf e.Row.Cells(index).Text.Length >= 9 Then

                                    DateFormat = e.Row.Cells(index).Text
                                    tempDate = DateFormat.Substring(0, DateFormat.IndexOf(" ") + 1)
                                    LastDateFormat = DateFormat.Substring(DateFormat.IndexOf(" ") + 1)
                                    e.Row.Cells(index).Text = tempDate.ToString("dd-MMM-yyyy") + " " + LastDateFormat


                                Else
                                    e.Row.Cells(index).Text = ""

                                End If

                            End If
                        End If
                    End If
                Next
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, eStr)
        End Try
    End Sub
#End Region
End Class
