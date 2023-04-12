
Partial Class frmMTPInfo
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION "
    Private objCommon As New clsCommon
    Private objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()
    Private ObjHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()

    Private VS_Choice As String = "Choice"
    Private Const Vs_dsMTP As String = "ds_MTP"
    Private Const VS_MTPNo As String = "MTPNO"
    Private Const Vs_dsMTPInfoGrid As String = "DtMTPInfoGrid"
    Private Const Vs_dtSiteDtlGrid As String = "dt_SiteDtlGrid"
    Private Const Vs_MTPHdrNO As String = "MTPHdrNO"

    Private mode As String

    Private GVViewMTP_MTPDate As Integer = 0
    Private GVViewMTP_DAY As Integer = 1
    Private GVViewMTP_WorkspaceDesc As Integer = 2
    Private GVViewMTP_ActivityName As Integer = 3
    Private GVViewMTP_InvestigatorName As Integer = 4
    Private GVViewMTP_SiteName As Integer = 5
    Private GVViewMTP_Address As Integer = 6
    Private GVViewMTP_Remark As Integer = 7

    Private GVCityDesc_MTPNo As Integer = 0
    Private GVCityDesc_MTPDate As Integer = 1
    Private GVCityDesc_Day As Integer = 2
    Private GVCityDesc_STPNo As Integer = 3
    Private GVCityDesc_SiteName As Integer = 4
    Private GVCityDesc_ActivityId As Integer = 5
    Private GVCityDesc_ActivityName As Integer = 6
    Private GVCityDesc_HoliDay As Integer = 7
    Private GVCityDesc_Add As Integer = 8
    Private GVCityDesc_View As Integer = 9
    Private GVCityDesc_Delete As Integer = 10
    Private GVCityDesc_MTPHdrNo As Integer = 11

    Private GVSiteDtl_MTPNo As Integer = 0
    Private GVSiteDtl_STPNo As Integer = 1
    Private GVSiteDtl_SiteName As Integer = 2
    Private GVSiteDtl_ActivityName As Integer = 3
    Private GVSiteDtl_Remark As Integer = 4
    Private GVSiteDtl_Delete As Integer = 5


#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            If Not IsPostBack Then
                GenCall()
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "", ex)
        End Try
    End Sub
#End Region

#Region "GenCall "
    Private Function GenCall() As Boolean
        Dim ds_MTP As DataSet = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add
            If Not IsNothing(Me.Request.QueryString("Mode")) Then
                Choice = CType(Me.Request.QueryString("Mode").ToString, WS_Lambda.DataObjOpenSaveModeEnum)
            End If
            Me.ViewState(VS_Choice) = Choice   'To use it while saving

            ''''Check for Valid User''''''''''''''

            If Not GenCall_Data(ds_MTP) Then ' For Data Retrieval
                Exit Function
            End If


            Me.ViewState(Vs_dsMTP) = ds_MTP  ' adding blank DataTable in viewstate

            If Not GenCall_ShowUI() Then ' For Showing Information
                Exit Function
            End If

            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "", ex)
        Finally
            ds_MTP = Nothing
        End Try

    End Function
#End Region

#Region "GenCall_Data "
    Private Function GenCall_Data(ByRef ds_MTP_Retu As DataSet) As Boolean

        Dim wStr As String = ""
        Dim eStr_Retu As String = ""
        Dim Val As String = ""
        Dim Val2 As String = ""
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        Dim ds As DataSet = Nothing
        Dim dsDtl As DataSet = Nothing
        'Dim objLambda As New WS_Lambda.WS_Lambda

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            'Val = Me.ViewState("nMTPNo") 'Value of where condition
            'Val2 = Me.Request.QueryString("StpNo")
            Choice = Me.ViewState(VS_Choice)

            If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "nMtpNo=" + Val.ToString '+ " and " + "nStpNo=" + Val2.ToString
            End If

            If Not Me.ObjHelp.GetMTPHdr(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If
            ds_MTP_Retu = New DataSet

            If Not Me.ObjHelp.GetMTPDtl(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsDtl, eStr_Retu) Then
                Response.Write(eStr_Retu)
                Exit Function
            End If

            ds_MTP_Retu.Tables.Add(ds.Tables(0).Copy())
            ds_MTP_Retu.AcceptChanges()
            ds_MTP_Retu.Tables.Add(dsDtl.Tables(0).Copy())
            ds_MTP_Retu.AcceptChanges()

            If ds Is Nothing Then
                Throw New Exception(eStr_Retu)
            End If

            If ds.Tables(0).Rows.Count <= 0 And _
               Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

                Throw New Exception("No Records Found for Selected Operation")

            End If

            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "", ex)

        Finally
            ds = Nothing
            objLambda = Nothing
        End Try
    End Function
#End Region

#Region "GenCall_showUI "
    Private Function GenCall_ShowUI() As Boolean
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Try
            Page.Title = ":: MTP Info :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Monthly Tour Plan"

            Choice = Me.ViewState(VS_Choice)

            Me.ddlMonth.Items.Clear()
            Me.ddlMonth.Items.Insert(0, New ListItem("Select Month"))
            Me.ddlMonth.Items.Insert(1, New ListItem(Format(CDate(System.DateTime.Now.AddMonths(-1)), "MMM-yyyy"), Format(CDate(System.DateTime.Now.AddMonths(-1)), "MMM-yyyy")))
            Me.ddlMonth.Items.Insert(2, New ListItem(Format(CDate(System.DateTime.Now), "MMM-yyyy"), Format(CDate(System.DateTime.Now), "MMM-yyyy")))
            Me.ddlMonth.Items.Insert(3, New ListItem(Format(CDate(System.DateTime.Now.AddMonths(1)), "MMM-yyyy"), Format(CDate(System.DateTime.Now.AddMonths(1)), "MMM-yyyy")))
            Me.ddlMonth.Items.Insert(4, New ListItem(Format(CDate(System.DateTime.Now.AddMonths(2)), "MMM-yyyy"), Format(CDate(System.DateTime.Now.AddMonths(2)), "MMM-yyyy")))

            Me.btnSubmit.Enabled = False
            Me.BtnApprove.Enabled = False
            Me.btnReject.Enabled = False

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then

            End If

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "", ex)

        Finally
        End Try
    End Function

#End Region

#Region "Fill DropDown"

    Private Function FillActivityGroup() As Boolean

        Dim dsActivityGroup As New DataSet
        Dim eStr As String = ""
        Dim Wstr_Scope As String = ""

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            'To Get Where condition of ScopeVales( Project Type )
            If Not objCommon.GetScopeValueWithCondition(Wstr_Scope) Then
                Exit Function
            End If

            If Not ObjHelp.GetviewActivityGroupMst(Wstr_Scope, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                    dsActivityGroup, eStr) Then
                Me.objCommon.ShowAlert(eStr + vbCrLf + "Error occured while retrieving Activity Group", Me)
                Exit Function
            End If

            dsActivityGroup.Tables(0).DefaultView.Sort = "vActivityGroupName"
            Me.ddlActivityGroup.DataSource = dsActivityGroup.Tables(0).DefaultView
            Me.ddlActivityGroup.DataTextField = "vActivityGroupName"
            Me.ddlActivityGroup.DataValueField = "vActivityGroupId"
            Me.ddlActivityGroup.DataBind()
            Me.ddlActivityGroup.Items.Insert(0, New ListItem("Please select ActivityGroup", "0"))

            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try

    End Function

#Region "Fill STP"
    Private Function FillSTP() As Boolean
        Dim ds_STP As New DataSet
        Dim Wstr As String = ""
        Dim estr As String = ""
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Not Me.ObjHelp.GetViewSTPUserWise(Me.Session(S_UserID), ds_STP, estr) Then
                Me.objCommon.ShowAlert("Error While Gwtting STP UserWise", Me.Page)
                Return False
            End If

            Me.ddlSTP.DataSource = ds_STP
            Me.ddlSTP.DataTextField = "vSiteName"
            Me.ddlSTP.DataValueField = "nStpNo"
            Me.ddlSTP.DataBind()
            Me.ddlSTP.Items.Insert(0, New ListItem("Please Select Site", "0"))

        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
        Finally
            If Not IsNothing(ds_STP) Then
                ds_STP = Nothing
            End If
        End Try

        Return True
    End Function
#End Region

#End Region

#Region "Drop Down Selection Change"

    Protected Sub ddlActivityGroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim estr As String = ""
        Dim wstr As String = ""
        Dim ds As New Data.DataSet
        Dim ds_type As New Data.DataSet
        Dim dv_Activity As New DataView
        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            wstr = "vActivityGroupId='" & Me.ddlActivityGroup.SelectedValue.Trim() & "' And cStatusIndi <> 'D'"

            If Not ObjHelp.getActivityMst(wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_type, estr) Then
                ShowErrorMessage("", estr)
            End If

            dv_Activity = ds_type.Tables(0).DefaultView
            dv_Activity.Sort = "vActivityName"

            Me.ddlActivity.DataSource = dv_Activity.ToTable()
            Me.ddlActivity.DataValueField = "vActivityId"
            Me.ddlActivity.DataTextField = "vActivityName"
            Me.ddlActivity.DataBind()
            Me.ddlActivity.Items.Insert(0, New ListItem("Please select Activity", "0"))

            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + _
                                                      Me.divMTPDtl.ClientID.ToString.Trim() + "');", True)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

#End Region

#Region "Button Go"
    Private Sub ProcessForViewMTP(ByVal CheckApprovalProcess_1 As Boolean)
        'Dim objLambda As New Ws_Lambda.Ws_Lambda
        Dim MTPDate As String = ""
        Dim dsMTPInfo As New DataSet
        Dim dvMTPinfo As New DataView
        Dim dsSubmitMTP As New DataSet

        Dim eStr As String = ""
        Dim Wstr As String = ""

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            Me.GVCityDesc.Enabled = True

            Me.btnSubmit.Enabled = True
            Me.BtnApprove.Enabled = True
            Me.btnReject.Enabled = True

            If Me.ddlMonth.SelectedIndex = 0 Then
                objCommon.ShowAlert("Please Select Month", Me)
                Exit Sub
            End If

            'To clear Grid
            Me.GVCityDesc.DataSource = Nothing
            Me.GVCityDesc.DataBind()

            MTPDate = "1-" & Me.ddlMonth.SelectedItem.Text.Trim()

            Me.ObjHelp.Get_ProcMTPMonthWise(MTPDate, Me.Session(S_UserID), dsMTPInfo, eStr)
            Me.ViewState(Vs_dsMTPInfoGrid) = dsMTPInfo

            Me.GVCityDesc.DataSource = dsMTPInfo
            Me.GVCityDesc.DataBind()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "", ex)
        End Try

    End Sub

    Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo.Click
        ProcessForViewMTP(True)
        FillSTP()
        FillActivityGroup()
    End Sub

#End Region

#Region "GvCity Grid view Events"
    Protected Sub GVCityDesc_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        e.Row.Cells(GVCityDesc_ActivityId).Visible = False
        e.Row.Cells(GVCityDesc_MTPNo).Visible = False
        e.Row.Cells(GVCityDesc_STPNo).Visible = False
        e.Row.Cells(GVCityDesc_HoliDay).Visible = False
        e.Row.Cells(GVCityDesc_MTPHdrNo).Visible = False
    End Sub

    Protected Sub GVCityDesc_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVCityDesc.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then

                CType(e.Row.FindControl("lnkAdd"), LinkButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("lnkAdd"), LinkButton).CommandName = "ADD"
                CType(e.Row.FindControl("lnkDelete"), LinkButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("lnkDelete"), LinkButton).CommandName = "DELETE"
                CType(e.Row.FindControl("lnkView"), LinkButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("lnkView"), LinkButton).CommandName = "VIEW"

                CType(e.Row.FindControl("lnkDelete"), LinkButton).OnClientClick = "return confirm('Are you sure you want to delete information of " + CType(e.Row.FindControl("lblGvDate"), Label).Text.Trim() + "?');"

                If e.Row.Cells(GVCityDesc_Day).Text.ToUpper = "SUNDAY" Then
                    e.Row.BackColor = Drawing.Color.OrangeRed
                End If

                If e.Row.Cells(GVCityDesc_MTPNo).Text.Replace("&nbsp;", "") = "" Or e.Row.Cells(GVCityDesc_MTPNo).Text.Replace("&nbsp;", "") = 0 Then
                    CType(e.Row.FindControl("lnkAdd"), LinkButton).Visible = True ' = "Add"
                    CType(e.Row.FindControl("lnkDelete"), LinkButton).Visible = False
                    CType(e.Row.FindControl("lnkView"), LinkButton).Visible = False
                Else
                    CType(e.Row.FindControl("lnkAdd"), LinkButton).Visible = False
                    CType(e.Row.FindControl("lnkDelete"), LinkButton).Visible = True ' = "Delete"
                    CType(e.Row.FindControl("lnkView"), LinkButton).Visible = True
                End If

                If e.Row.Cells(GVCityDesc_HoliDay).Text.Replace("&nbsp;", "") <> "" Then
                    e.Row.Cells(GVCityDesc_SiteName).Text = e.Row.Cells(GVCityDesc_HoliDay).Text.Trim()
                    CType(e.Row.FindControl("lnkAdd"), LinkButton).Visible = False
                    CType(e.Row.FindControl("lnkDelete"), LinkButton).Visible = False
                    CType(e.Row.FindControl("lnkView"), LinkButton).Visible = False
                    e.Row.BackColor = Drawing.Color.OrangeRed
                End If

                If e.Row.RowIndex > 0 Then
                    If CType(e.Row.FindControl("lblGvDate"), Label).Text.Trim() = CType(Me.GVCityDesc.Rows(e.Row.RowIndex - 1).FindControl("lblGvDate"), Label).Text.Trim() Then
                        CType(e.Row.FindControl("lblGvDate"), Label).Visible = False
                        e.Row.Cells(GVCityDesc_Day).Text = ""
                        CType(e.Row.FindControl("lnkDelete"), LinkButton).Visible = False
                    End If
                End If


            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "", ex)
        End Try
    End Sub

    Protected Sub GVCityDesc_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GVCityDesc.RowCommand
        Dim MTPDate As String
        Dim MTPNO As Integer
        Dim MTPHdrNO As Integer
        Dim dsMtpInfo As New DataSet
        Dim ds_MTP As New DataSet
        Dim dtMTPHdr As New DataTable
        Dim dtMTPDtl As New DataTable
        Dim eStr As String = ""
        Dim dr As DataRow
        Try
            Dim i As Int32 = CInt(e.CommandArgument)

            MTPDate = CType(Me.GVCityDesc.Rows(i).FindControl("lblGvDate"), Label).Text.Trim()
            MTPNO = Me.GVCityDesc.Rows(i).Cells(GVCityDesc_MTPNo).Text.Trim()
            MTPHdrNO = CInt(Me.GVCityDesc.Rows(i).Cells(GVCityDesc_MTPHdrNo).Text.Trim())
            Me.ViewState(Vs_MTPHdrNO) = MTPHdrNO
            If e.CommandName.ToUpper = "ADD" Then

                'For GVSiteDtl Grid of Div
                CreateSiteDtl()
                '*******************

                If IsNothing(Me.ViewState(VS_MTPNo)) Then
                    Me.ViewState(VS_MTPNo) = 0
                End If
                Me.ViewState(VS_MTPNo) = IIf(MTPNO = 0, CInt(Me.ViewState(VS_MTPNo)) + 1, MTPNO)

                Me.divMTPDtl.Visible = True
                Me.pnlMTPDtl.Visible = True
                Me.btnAdd.Visible = True
                Me.btncancel.Visible = True

                Me.txtMtpDt.Value = MTPDate
                Me.txtMtpDt.Disabled = True
                Me.txtRemark.Value = ""

                Me.GVCityDesc.Enabled = False

                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + _
                                                        Me.divMTPDtl.ClientID.ToString.Trim() + "');", True)

            ElseIf e.CommandName.ToUpper = "DELETE" Then

                'Me.ObjHelp.ProcedureExecute("Proc_UpdateMTP", MTPNO.ToString + "##" + Me.Session(S_UserID) + "##")
                dsMtpInfo = CType(Me.ViewState(Vs_dsMTP), DataSet)
                dtMTPHdr = dsMtpInfo.Tables("MTPHdr")
                dtMTPDtl = dsMtpInfo.Tables("MTPDtl")

                dr = dtMTPHdr.NewRow
                'iUserId,iDay,cMTPType,dMTPDate,dApprovalDate,cApprovalFlag,iApprovalUserId,dSubmitDate, cActiveFlag, iModifyBY
                If MTPNO <> 0 Then
                    dr("nMTPNo") = MTPNO
                End If
                dr("iApprovalUserId") = System.DBNull.Value
                dr("cApprovalFlag") = "N" '"A" ''H for Hold
                dr("dApprovalDate") = System.DBNull.Value
                dr("cActiveFlag") = "N"
                dr("iModifyBY") = Me.Session(GeneralModule.S_UserID)

                dtMTPHdr.Rows.Add(dr)
                dtMTPHdr.AcceptChanges()

                'nMTPHdrNo,nSTPNo,nCityNo,nWorkWithNo,nWorkTypeNo,vActivityId, nReasonNo, vRemark, cActiveFlag, iModifyBY
                dr = dtMTPDtl.NewRow
                If MTPNO <> 0 Then
                    dr("nMTPHdrNo") = MTPNO
                End If
                dr("cActiveFlag") = "N"
                dr("iModifyBY") = Me.Session(S_UserID)
                dtMTPDtl.Rows.Add(dr)

                dtMTPDtl.AcceptChanges()

                dtMTPHdr.TableName = "MTPHdr"
                dtMTPHdr.AcceptChanges()
                dtMTPDtl.TableName = "MTPDtl"
                dtMTPDtl.AcceptChanges()

                ds_MTP = New DataSet
                ds_MTP.Tables.Add(dtMTPHdr.Copy())
                ds_MTP.Tables.Add(dtMTPDtl.Copy())

                If Not Me.objLambda.Save_MTPMstDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_MTP, Me.Session(S_UserID), eStr) Then
                    Me.objCommon.ShowAlert("Error While Deleteing Detail", Me.Page)
                    Exit Sub
                End If

                ProcessForViewMTP(False)

            ElseIf e.CommandName.ToUpper = "VIEW" Then

                dsMtpInfo = CType(Me.ViewState(Vs_dsMTPInfoGrid), DataSet)

                dsMtpInfo.Tables(0).DefaultView.RowFilter = "nMTPNo= " & MTPNO & _
                                                "and nStpNo=" & Me.GVCityDesc.Rows(i).Cells(GVCityDesc_STPNo).Text.Trim() ' & "and vWorkTypeDesc='" & WorkType & "'"

                Me.GVViewMTP.DataSource = dsMtpInfo.Tables(0).DefaultView
                Me.GVViewMTP.DataBind()

                Me.pnlViewMTP.Visible = True
                Me.divMTP.Visible = True

                Me.GVCityDesc.Enabled = False

                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + _
                                                        Me.divMTP.ClientID.ToString.Trim() + "');", True)



            End If

        Catch ex As System.Threading.ThreadAbortException

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "", ex)
        End Try
    End Sub

    Protected Sub GVCityDesc_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GVCityDesc.RowDeleting

    End Sub
#End Region

#Region "Show Error"
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objCommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub
#End Region

#Region "CreateTable For Showing GVSiteDtl Grid"

    Private Sub CreateSiteDtl()
        Dim dtSiteDtl As New DataTable
        Dim dc As DataColumn

        dc = New DataColumn
        dc.ColumnName = "nMTPNo"
        dc.DataType = GetType(Integer)
        dtSiteDtl.Columns.Add(dc)

        dc = New DataColumn
        dc.ColumnName = "nSTPNo"
        dc.DataType = GetType(Integer)
        dtSiteDtl.Columns.Add(dc)

        dc = New DataColumn
        dc.ColumnName = "vActivityName"
        dc.DataType = GetType(String)
        dtSiteDtl.Columns.Add(dc)

        dc = New DataColumn
        dc.ColumnName = "vSiteName"
        dc.DataType = GetType(String)
        dtSiteDtl.Columns.Add(dc)

        dc = New DataColumn
        dc.ColumnName = "vRemark"
        dc.DataType = GetType(String)
        dtSiteDtl.Columns.Add(dc)

        Me.ViewState(Vs_dtSiteDtlGrid) = dtSiteDtl

    End Sub

#End Region

#Region "Button Add, Cancel, Save And Exit Of MTP Details Div"
    Private Function ValidAdd() As Boolean

        If Me.ddlSTP.SelectedIndex <= 0 Then
            objCommon.ShowAlert("Please Select STP", Me)
            Exit Function
        ElseIf Me.ddlActivityGroup.SelectedIndex <= 0 Then
            objCommon.ShowAlert("Please Select Activity Group", Me)
            Exit Function
        ElseIf Me.ddlActivity.SelectedIndex <= 0 Then
            objCommon.ShowAlert("Please Select Activity", Me)
            Exit Function
        End If

        For index As Integer = 0 To Me.GVSiteDtl.Rows.Count - 1

            If Me.ddlSTP.SelectedValue.Trim() = Me.GVSiteDtl.Rows(index).Cells(GVSiteDtl_STPNo).Text.Trim() Then

                objCommon.ShowAlert("This STP is Allready Added for the Day.", Me)
                Exit Function

            End If

        Next index

        Return True

    End Function

    Private Function AssignValue() As Boolean
        Dim ds_StpPlace As New DataSet
        Dim ds_MTP As DataSet
        Dim dsMTPInfo As New DataSet
        Dim dtMTPHdr As New DataTable
        Dim dtMTPDtl As New DataTable
        Dim dtMTPInfo As New DataTable
        Dim dr As DataRow
        Dim eStr As String = ""
        Dim MtpNo As Integer = 0
        Dim dtSiteDtl As New DataTable

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            MtpNo = Me.ViewState(VS_MTPNo)

            dtSiteDtl = CType(Me.ViewState(Vs_dtSiteDtlGrid), DataTable)

            dr = dtSiteDtl.NewRow()
            If MtpNo <> 0 Then
                dr("nMTPNo") = MtpNo
            End If
            dr("nSTPNo") = Me.ddlSTP.SelectedValue.Trim()
            dr("vActivityName") = Me.ddlActivity.SelectedItem.Text.Trim()
            dr("vSiteName") = Me.ddlSTP.SelectedItem.Text.Trim()
            dr("vRemark") = Me.txtRemark.Value.Trim()
            dtSiteDtl.Rows.Add(dr)

            Me.ViewState(Vs_dtSiteDtlGrid) = dtSiteDtl

            dsMTPInfo = CType(Me.ViewState(Vs_dsMTP), DataSet)
            dtMTPHdr = dsMTPInfo.Tables("MTPHdr")
            dtMTPDtl = dsMTPInfo.Tables("MTPDtl")

            If dtMTPHdr.Rows.Count <= 0 Then
                dr = dtMTPHdr.NewRow
            Else
                dr = dtMTPHdr.Rows(0)
            End If
            'iUserId,iDay,cMTPType,dMTPDate,dApprovalDate,cApprovalFlag,iApprovalUserId,dSubmitDate, cActiveFlag, iModifyBY
            If MtpNo <> 0 Then
                dr("nMTPNo") = MtpNo
            End If
            dr("iUserId") = Me.Session(S_UserID)
            dr("iDay") = 0 'System.DBNull.Value
            dr("cMTPType") = "M"
            dr("dMTPDate") = Format(CDate(Me.txtMtpDt.Value), "dd-MMM-yyyy")
            dr("iApprovalUserId") = System.DBNull.Value
            dr("cApprovalFlag") = "N" '"A" ''H for Hold
            dr("dApprovalDate") = System.DBNull.Value
            dr("cActiveFlag") = "Y"
            dr("iModifyBY") = Me.Session(GeneralModule.S_UserID)
            dr("nMTPHdrNo") = Me.ViewState(Vs_MTPHdrNO) '0

            If dtMTPHdr.Rows.Count <= 0 Then
                dtMTPHdr.Rows.Add(dr)
            End If

            dtMTPHdr.AcceptChanges()

            If Not ObjHelp.GetSTP("nStpNo=" + Me.ddlSTP.SelectedValue.ToString(), _
                                WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                 ds_StpPlace, eStr) Then
                objCommon.ShowAlert(eStr, Me)
                Exit Function
            End If

            For Each dr_StpPlace As DataRow In ds_StpPlace.Tables(0).Rows
                'nMTPHdrNo,nSTPNo,nCityNo,nWorkWithNo,nWorkTypeNo,vActivityId, nReasonNo, vRemark, cActiveFlag, iModifyBY
                dr = dtMTPDtl.NewRow
                If MtpNo <> 0 Then
                    dr("nMTPHdrNo") = MtpNo
                End If
                dr("nSTPNo") = Me.ddlSTP.SelectedValue.Trim()
                dr("nCityNo") = dr_StpPlace("nCityNo")
                dr("nWorkWithNo") = -1
                dr("nWorkTypeNO") = 1
                dr("vActivityId") = Me.ddlActivity.SelectedValue.Trim()
                dr("nReasonNo") = 0
                dr("vRemark") = Me.txtRemark.Value.Trim()
                dr("cActiveFlag") = "Y"
                dr("iModifyBY") = Me.Session(S_UserID)
                dtMTPDtl.Rows.Add(dr)

            Next dr_StpPlace
            dtMTPDtl.AcceptChanges()

            dtMTPHdr.TableName = "MTPHdr"
            dtMTPHdr.AcceptChanges()
            dtMTPDtl.TableName = "MTPDtl"
            dtMTPDtl.AcceptChanges()

            ds_MTP = New DataSet
            ds_MTP.Tables.Add(dtMTPHdr.Copy())
            ds_MTP.Tables.Add(dtMTPDtl.Copy())

            Me.ViewState(Vs_dsMTP) = ds_MTP
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "", ex)
            Return False
        Finally
            dsMTPInfo = Nothing
            dtMTPHdr = Nothing
            dtMTPDtl = Nothing
            dtMTPInfo = Nothing
        End Try
    End Function

    Protected Sub btnAdd_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs)

        If Not ValidAdd() Then
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + _
                                                        Me.divMTPDtl.ClientID.ToString.Trim() + "');", True)
            Exit Sub
        End If

        If Not AssignValue() Then
            Me.objCommon.ShowAlert("Error while Assign Values", Me.Page)
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + _
                                                        Me.divMTPDtl.ClientID.ToString.Trim() + "');", True)
            Exit Sub
        End If

        Me.GVSiteDtl.DataSource = CType(Me.ViewState(Vs_dtSiteDtlGrid), DataTable)
        Me.GVSiteDtl.DataBind()

        btncancel_ServerClick(sender, e) 'To Clear All the values

        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + _
                                                        Me.divMTPDtl.ClientID.ToString.Trim() + "');", True)

    End Sub

    Protected Sub BtnSave_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ds_MTP As DataSet
        Dim eStr As String = ""
        Try
            ds_MTP = Me.ViewState(Vs_dsMTP)

            If Not objLambda.Save_MTPMstDtl(Me.ViewState(VS_Choice), ds_MTP, Me.Session(GeneralModule.S_UserID), eStr) Then

                objCommon.ShowAlert(eStr, Me)
                Exit Sub

            End If

            ResetDtl()
            GenCall()
            Me.ddlMonth.SelectedValue = Format(CDate(Me.txtMtpDt.Value.Trim()), "MMM-yyyy")
            ProcessForViewMTP(False)

            Me.btnExit.Visible = True
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btncancel_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncancel.ServerClick

        If Me.ddlSTP.SelectedIndex > 0 Then
            Me.ddlSTP.SelectedIndex = 0
        End If

        If Me.ddlActivityGroup.SelectedIndex > 0 Then
            Me.ddlActivityGroup.SelectedIndex = 0
        End If

        If Me.ddlActivity.SelectedIndex > 0 Then
            Me.ddlActivity.SelectedIndex = 0
        End If
        Me.txtRemark.Value = ""

        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "SetCenter", "SetCenter('" + _
                                                       Me.divMTPDtl.ClientID.ToString.Trim() + "');", True)
    End Sub

    Protected Sub btnExitMTP_ServerClick1(ByVal sender As Object, ByVal e As System.EventArgs)
        ResetDtl()
        Me.divMTPDtl.Visible = False
        Me.pnlMTPDtl.Visible = False

        Me.GVCityDesc.Enabled = True
    End Sub

#End Region

#Region "CreateTable MTPInfo Final Submit"

    Private Sub CreateMTPInfo(ByRef dtMTPInfo As DataTable)

        Dim dc As DataColumn

        dc = New DataColumn
        dc.ColumnName = "dMtpDate"
        dc.DataType = GetType(String)
        dtMTPInfo.Columns.Add(dc)

        dc = New DataColumn
        dc.ColumnName = "iApprovalUserId"
        dc.DataType = GetType(Integer)
        dtMTPInfo.Columns.Add(dc)

        dc = New DataColumn
        dc.ColumnName = "cApprovalFlag"
        dc.DataType = GetType(String)
        dtMTPInfo.Columns.Add(dc)

        dc = New DataColumn
        dc.ColumnName = "vRejectionRemark"
        dc.DataType = GetType(String)
        dtMTPInfo.Columns.Add(dc)

        dc = New DataColumn
        dc.ColumnName = "iModifyBY"
        dc.DataType = GetType(String)
        dtMTPInfo.Columns.Add(dc)

    End Sub

#End Region

#Region "Button Exit Click Of DivMTP (View)"
    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.pnlViewMTP.Visible = False
        Me.divMTP.Visible = False

        Me.GVCityDesc.Enabled = True
    End Sub
#End Region

#Region "Button Submit And Exit of Page"

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Dim dsMTPInfo As New DataSet
        Dim dtMTPInfo As New DataTable
        'Dim objLambda As New WS_Lambda.WS_Lambda
        Dim cnt As Integer = 0
        Dim rowcnt As Integer = 0
        Dim stp_rowcnt As Integer = 0
        Dim eStr As String = ""
        Dim dr As DataRow
        Dim bln As Boolean

        Try

            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            bln = False
            ''new code starting....
            For index As Integer = 0 To Me.GVCityDesc.Rows.Count - 1

                If Me.GVCityDesc.Rows(index).Cells(GVCityDesc_Day).Text.ToUpper <> "SUNDAY" And _
                    Me.GVCityDesc.Rows(index).Cells(GVCityDesc_HoliDay).Text.Replace("&nbsp;", "") = "" Then

                    If Me.GVCityDesc.Rows(index).Cells(GVCityDesc_SiteName).Text.Replace("&nbsp;", "") <> "" Then
                        stp_rowcnt = stp_rowcnt + 1
                    End If

                    rowcnt = rowcnt + 1
                End If

            Next index

            If stp_rowcnt = 0 Then
                Me.objCommon.ShowAlert("Please Add Atleast One MTP", Me)
                Exit Sub
            End If

            If rowcnt > stp_rowcnt Then
                objCommon.ShowAlert("Please Enter STP for All Days", Me)
                Exit Sub
            End If

            '' new code ending

            CreateMTPInfo(dtMTPInfo)

            dr = dtMTPInfo.NewRow

            dr("dMtpDate") = CType(Me.GVCityDesc.Rows(1).FindControl("lblGvDate"), Label).Text.Trim()
            dr("iApprovalUserId") = Me.Session(S_UserID)
            dr("cApprovalFlag") = "N"
            dr("vRejectionRemark") = "Null"
            dr("iModifyBY") = Me.Session(S_UserID)

            dtMTPInfo.Rows.Add(dr)
            dtMTPInfo.AcceptChanges()

            dsMTPInfo.Tables.Add(dtMTPInfo.Copy())
            dsMTPInfo.AcceptChanges()

            If Not objLambda.Save_SubmitMTP(Me.ViewState(VS_Choice), dsMTPInfo, Me.Session("UserCode"), eStr) Then

                Me.ShowErrorMessage("", eStr)
                Exit Sub

            End If

            objCommon.ShowAlert("Records Successfully Submitted", Me)
            ResetPage()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "", ex)
        Finally
            dsMTPInfo = Nothing
            objLambda = Nothing

        End Try
    End Sub

    Protected Sub BtnExitPage_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Response.Redirect("frmMainpage.aspx")
    End Sub

#End Region

#Region "Reset"

    Private Sub ResetPage()
        Me.ddlMonth.SelectedIndex = 0
        Me.GVCityDesc.DataSource = Nothing
        Me.GVCityDesc.DataBind()
        Me.btnSubmit.Enabled = False
    End Sub

    Private Sub ResetDtl()
        Me.divMTPDtl.Visible = False
        Me.pnlMTPDtl.Visible = False
        Me.btnAdd.Visible = False
        Me.btncancel.Visible = False

        Me.ddlSTP.SelectedIndex = 0
        Me.ddlActivityGroup.SelectedIndex = 0
        Me.ddlActivity.Items.Clear()
        Me.txtRemark.Value = ""
        'Me.txtMtpDt.Value = ""
        Me.GVSiteDtl.DataSource = Nothing
        Me.GVSiteDtl.DataBind()

        Me.ViewState(Vs_dsMTP) = Nothing
        Me.ViewState(Vs_dtSiteDtlGrid) = Nothing


    End Sub
#End Region

#Region "GVSiteDtl Grid Event"
    Protected Sub GVSiteDtl_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        e.Row.Cells(GVSiteDtl_MTPNo).Visible = False
        e.Row.Cells(GVSiteDtl_STPNo).Visible = False

    End Sub

    Protected Sub GVSiteDtl_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lnkSiteDtl_Delete"), LinkButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("lnkSiteDtl_Delete"), LinkButton).CommandName = "DELETE"
        End If

    End Sub

    Protected Sub GVSiteDtl_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim Index As Integer = CInt(e.CommandArgument)
        Dim ds_MTP As DataSet
        Dim dsMTPInfo As New DataSet
        Dim dtMTPHdr As New DataTable
        Dim dtMTPDtl As New DataTable
        Dim dtMTPInfo As New DataTable
        Dim eStr As String = ""
        Dim MtpHeaderNo_1 As Integer = 0
        Dim dtSiteDtl As New DataTable
        Dim cntDr As Integer = 0

        If e.CommandName.ToUpper.Trim() = "DELETE" Then
            dtSiteDtl = CType(Me.ViewState(Vs_dtSiteDtlGrid), DataTable)
            dsMTPInfo = CType(Me.ViewState(Vs_dsMTP), DataSet)
            dtMTPHdr = dsMTPInfo.Tables("MTPHdr")
            dtMTPDtl = dsMTPInfo.Tables("MTPDtl")

            For cntDr = 0 To dtSiteDtl.Rows.Count - 1

                If Me.GVSiteDtl.Rows(Index).Cells(GVSiteDtl_MTPNo).Text = dtSiteDtl.Rows(cntDr).Item("nMTPNo") And _
                    Me.GVSiteDtl.Rows(Index).Cells(GVSiteDtl_STPNo).Text = dtSiteDtl.Rows(cntDr).Item("nSTPNo") Then

                    dtSiteDtl.Rows(cntDr).Delete()

                End If

            Next cntDr

            For cntDr = 0 To dtMTPDtl.Rows.Count - 1

                If Me.GVSiteDtl.Rows(Index).Cells(GVSiteDtl_MTPNo).Text = dtMTPDtl.Rows(cntDr).Item("nMTPNo") And _
                    Me.GVSiteDtl.Rows(Index).Cells(GVSiteDtl_STPNo).Text = dtMTPDtl.Rows(cntDr).Item("nSTPNo") Then

                    dtMTPDtl.Rows(cntDr).Delete()

                End If

            Next cntDr

            If dtMTPDtl.Rows.Count <= 0 Then

                dtMTPHdr.Clear()

            End If

            ds_MTP = New DataSet
            ds_MTP.Tables.Add(dtMTPHdr)
            ds_MTP.Tables.Add(dtMTPDtl)

            Me.ViewState(Vs_dsMTP) = ds_MTP
            Me.ViewState(Vs_dtSiteDtlGrid) = dtSiteDtl

        End If

    End Sub

#End Region

#Region "UnUsed Approve And Reject Button"
    'Protected Sub BtnApprove_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim dsMTPInfo As New DataSet
    '    Dim objLambda As New WS_Lambda.WS_Lambda
    '    Try
    '        Dim eStr As String = ""

    '        Dim dr As DataRow
    '        Dim ds As New DataSet
    '        Dim bln As Boolean = False
    '        Dim nMtpNo As String = ""
    '        Dim drno As DataRow
    '        Dim dsMtpNo As New DataSet
    '        Dim MtpNo As String

    '        Dim rowcnt As Integer = 0
    '        Dim stp_rowcnt As Integer = 0

    '        CreateMTPInfo()
    '        CreateMTPNo()

    '        ''
    '        For i As Integer = 0 To Me.GVCityDesc.Rows.Count - 1
    '            If (CType(Me.GVCityDesc.Rows(i).Cells(2).Controls(1), Label).Text <> "Sunday" And Val(Me.GVCityDesc.Rows(i).Cells(15).Text) = 0) And Val(Me.GVCityDesc.Rows(i).Cells(17).Text) = 0 Then
    '                'If CType(Me.GVCityDesc.Rows(i).Cells(4).Controls(1), Label).Text <> "" Then
    '                If CType(Me.GVCityDesc.Rows(i).Cells(1).Controls(1), Label).Text <> "" Then
    '                    stp_rowcnt = stp_rowcnt + 1
    '                End If
    '                rowcnt = rowcnt + 1
    '            End If
    '        Next

    '        If stp_rowcnt = 0 Then
    '            Me.objCommon.ShowAlert("Please Add Atleast One MTP", Me)
    '            Exit Sub
    '        End If
    '        If rowcnt > stp_rowcnt Then
    '            objCommon.ShowAlert("Please Enter STP for All Days", Me)
    '            Exit Sub
    '        End If

    '        ''

    '        dsMTPInfo.Tables.Add(CType(Me.ViewState("dtMTPInfo"), DataTable))

    '        dsMtpNo.Tables.Add(CType(Me.ViewState("dtMTPNo"), DataTable))
    '        For i As Integer = 0 To GVCityDesc.Rows.Count - 1


    '            nMtpNo = CType(Me.GVCityDesc.Rows(i).Cells(7).Controls(1), Label).Text.Trim()
    '            If nMtpNo.ToString = "" Then
    '            Else
    '                Exit For
    '            End If
    '        Next
    '        Dim dMtpDate As String
    '        dMtpDate = CType(Me.GVCityDesc.Rows(1).Cells(3).Controls(1), Label).Text.Trim()

    '        If nMtpNo.ToString = "" Then
    '            drno = dsMtpNo.Tables(0).NewRow
    '            drno("nMtpHdrNo") = nMtpNo.ToString
    '            drno("dMtpDate") = dMtpDate
    '            drno("nPositionNo") = Me.ddlEmp.SelectedValue
    '            dsMtpNo.Tables(0).Rows.Add(drno)

    '            'excute proce which return nmtphdrno 
    '            If Not objLambda.GetMaxHdrNo(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, dsMtpNo, MtpNo, Me.Session("UserCode"), eStr) Then
    '                Me.ShowErrorMessage("", eStr)
    '                Exit Sub

    '            End If
    '            nMtpNo = MtpNo

    '        End If
    '        dr = dsMTPInfo.Tables(0).NewRow


    '        dr("dMtpDate") = dMtpDate
    '        dr("nApprovalPositionNo") = Me.Session("PositionNo")
    '        dr("cApprovalFlag") = Me.ViewState(Vs_Flag)
    '        dr("vRejectionRemark") = "Null"
    '        dr("nUserNo") = Session("UserNo")
    '        dr("nMtpHdrNo") = nMtpNo

    '        dsMTPInfo.Tables(0).Rows.Add(dr)

    '        If Not objLambda.MTPApproval(Me.ViewState(VS_Choice), dsMTPInfo, Me.Session("UserCode"), eStr) Then
    '            Me.ShowErrorMessage("", eStr)
    '            Exit Sub
    '        Else
    '            objCommon.ShowAlert("Records Successfully Approved", Me)
    '            Me.Reset()
    '        End If


    '    Catch ex As Exception
    '        Me.ShowErrorMessage(ex.Message, "", ex)
    '    Finally
    '        dsMTPInfo = Nothing
    '        objLambda = Nothing
    '    End Try
    'End Sub

    'Protected Sub btnReject_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim dsMTPInfo As New DataSet
    '    Dim objLambda As New WS_Lambda.WS_Lambda
    '    Try
    '        Dim eStr As String = ""

    '        Dim dr As DataRow
    '        Dim bln As Boolean = False
    '        Dim MtpNo As String
    '        Dim nMtpNo As String = ""

    '        CreateMTPInfo()

    '        dsMTPInfo.Tables.Add(CType(Me.ViewState("dtMTPInfo"), DataTable))

    '        For i As Integer = 0 To GVCityDesc.Rows.Count - 1


    '            nMtpNo = CType(Me.GVCityDesc.Rows(i).Cells(7).Controls(1), Label).Text.Trim()
    '            If nMtpNo.ToString = "" Then
    '            Else
    '                Exit For
    '            End If
    '        Next

    '        dr = dsMTPInfo.Tables(0).NewRow

    '        dr("nApprovalPositionNo") = Me.Session("PositionNo")
    '        dr("cApprovalFlag") = "X"
    '        dr("vRejectionRemark") = "Null"
    '        dr("dMtpDate") = CType(Me.GVCityDesc.Rows(1).Cells(3).Controls(1), Label).Text.Trim()
    '        dr("nUserNo") = Session("UserNo")
    '        dr("nMtpHdrNo") = nMtpNo.ToString
    '        dr("nPositionNo") = Me.ddlEmp.SelectedValue

    '        dsMTPInfo.Tables(0).Rows.Add(dr)

    '        If Not objLambda.MTPApproval(Me.ViewState(VS_Choice), dsMTPInfo, Me.Session("UserCode"), eStr) Then
    '            Me.ShowErrorMessage("", eStr)
    '            Exit Sub
    '        Else
    '            objCommon.ShowAlert("Records Successfully Rejected", Me)
    '            Me.Reset()
    '        End If


    '    Catch ex As Exception
    '        Me.ShowErrorMessage(ex.Message, "", ex)
    '    Finally
    '        dsMTPInfo = Nothing
    '        objLambda = Nothing
    '    End Try
    'End Sub
    '#Region "CreateTable"
    '    Private Sub CreateMTPInfo()

    '        Dim dtMTPInfo As New DataTable
    '        Dim dtMTPNo As New DataTable
    '        Dim dc As DataColumn

    '        dc = New DataColumn
    '        dc.ColumnName = "nApprovalPositionNo"
    '        dc.DataType = GetType(Integer)
    '        dtMTPInfo.Columns.Add(dc)

    '        dc = New DataColumn
    '        dc.ColumnName = "cApprovalFlag"
    '        dc.DataType = GetType(String)
    '        dtMTPInfo.Columns.Add(dc)

    '        dc = New DataColumn
    '        dc.ColumnName = "vRejectionRemark"
    '        dc.DataType = GetType(String)
    '        dtMTPInfo.Columns.Add(dc)

    '        dc = New DataColumn
    '        dc.ColumnName = "dMtpDate"
    '        dc.DataType = GetType(String)
    '        dtMTPInfo.Columns.Add(dc)

    '        dc = New DataColumn
    '        dc.ColumnName = "nUserNo"
    '        dc.DataType = GetType(Integer)
    '        dtMTPInfo.Columns.Add(dc)

    '        dc = New DataColumn
    '        dc.ColumnName = "nMtpHdrNo"
    '        dc.DataType = GetType(String)
    '        dtMTPInfo.Columns.Add(dc)

    '        dc = New DataColumn
    '        dc.ColumnName = "nPositionNo"
    '        dc.DataType = GetType(String)
    '        dtMTPInfo.Columns.Add(dc)


    '        Me.ViewState("dtMTPInfo") = dtMTPInfo


    '    End Sub
    '    Private Sub CreateMTPNo()

    '        Dim dtMTPNo As New DataTable
    '        Dim dc As DataColumn


    '        dc = New DataColumn
    '        dc.ColumnName = "dMtpDate"
    '        dc.DataType = GetType(String)
    '        dtMTPNo.Columns.Add(dc)


    '        dc = New DataColumn
    '        dc.ColumnName = "nMtpHdrNo"
    '        dc.DataType = GetType(String)
    '        dtMTPNo.Columns.Add(dc)

    '        dc = New DataColumn
    '        dc.ColumnName = "nPositionNo"
    '        dc.DataType = GetType(String)
    '        dtMTPNo.Columns.Add(dc)

    '        Me.ViewState("dtMTPNo") = dtMTPNo

    '    End Sub
    '#End Region
#End Region

End Class
