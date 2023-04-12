Imports Newtonsoft.Json
Imports Winnovative
Imports System.IO
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Drawing.Imaging



Partial Class frmSubjectPIFMst_New
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
    Private Const VS_SubjectProofNO As String = "nSubjectProofNo"
    Private Const VS_DtSubjectMaster As String = "drSubjectMaster"

    'For inhouse mode
    Private InHouse_Mode As String = "11"
    Private Const BAE_Subject As String = "B"
    Private Const InHouse_Subject As String = "I"
    Private Const Location_Canada As String = "0003"
    Private Subject_Type As String = BAE_Subject
    Private Const VS_SubjectId As String = "SubjectId"
    Private Const VS_GVDtSubjectProofDetails As String = "GVdtSubjectProof"
    Private Const VS_TempSubjectProof As String = "TempSubjectProof"
    Private FileName As String = String.Empty
    Private Language As String = String.Empty



    Private Shared Wstr As String = String.Empty
    Private Shared estr As String = String.Empty


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
    Private Shared popup As Boolean

    'nSubjectProofNo,vSubjectId,iTranNo,vProofType,vProofPath,iModifyBy,dModifyOn,cStatusIndi
    Private Const GVCSubProof_nSubjectProofNo As Integer = 0
    Private Const GVCSubProof_vSubjectId As Integer = 1
    Private Const GVCSubProof_iTranNo As Integer = 2
    Private Const GVCSubProof_vProofType As Integer = 3
    Private Const GVCSubProof_vProofPath As Integer = 4
    Private Const GVCSubProof_iModifyBy As Integer = 5
    Private Const GVCSubProof_cStatusIndi As Integer = 6
    Private Const GVCSubProof_Attachment As Integer = 7
    Private Const GVCSubProof_Delete As Integer = 8

    Private Const VS_TempTransactionNo As String = "TempTransactionNo"
#End Region

#Region "Page Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.HFTimeZone.Value = Session(S_TimeZoneName).ToString()
            If Me.Request.QueryString("mode") = InHouse_Mode Then
                Subject_Type = InHouse_Subject
                Me.btnMSR.Visible = False
            ElseIf Me.Request.QueryString("mode") = 4 Then
                Me.HFMode.Value = "4"
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "disableControl134", "AuditdisableControl();", True)
                Me.ViewState(VS_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View
                Me.BtnNew.Visible = False
                Me.btnAttach.Visible = False
                Me.Image36.Attributes.Add("visibility", "hidden")
                ' Me.btnCancel.Visible = False
                Me.btnsave.Style.Add("display", "none")
            End If

            Me.AutoCompleteExtender1.ServiceMethod = "GetSubjectCompletionList_NotRejected_BlockPeriod"
            If System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge").Contains(Session(S_LocationCode)) = True Then
                Me.AutoCompleteExtender1.ContextKey = Me.Session(S_LocationCode)
                Me.AutoCompleteExtender1.ServiceMethod = "GetSubjectCompletionList_NotRejected_BlockPeriod"
            Else
                Me.AutoCompleteExtender1.ContextKey = System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge")
                Me.AutoCompleteExtender1.ServiceMethod = "GetSubjectCompletionList_NotRejected_BlockPeriodDataMerg"
            End If
            If Me.Request.QueryString("mode") = InHouse_Mode Then
                If System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge").Contains(Session(S_LocationCode)) = True Then
                    Me.AutoCompleteExtender1.ContextKey = "View_subjectMaster#" + " vLocationCode='" & Session(S_LocationCode) & "'"
                    Me.AutoCompleteExtender1.ServiceMethod = "GetSubjectCompletionList_NotRejected_InHouse_BlockPeriod"
                Else
                    Me.AutoCompleteExtender1.ContextKey = "View_subjectMaster#" + " vLocationCode Not In(" + System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge") + ")"
                    Me.AutoCompleteExtender1.ServiceMethod = "GetSubjectCompletionList_NotRejected_InHouse_BlockPeriod"
                End If
                'Me.AutoCompleteExtender1.ContextKey = "View_subjectMaster#" + " vLocationCode Not In(" + System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge") + ")"
                'Me.AutoCompleteExtender1.ServiceMethod = "GetSubjectCompletionList_NotRejected_InHouse_BlockPeriod"
            ElseIf Me.Request.QueryString("mode") = 4 Then
                If System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge").Contains(Session(S_LocationCode)) = True Then
                    AutoCompleteExtender1.ServiceMethod = "GetSubjectCompletionList_NotRejected_OnlyID_BlockPeriod"
                Else
                    AutoCompleteExtender1.ServiceMethod = "GetSubjectCompletionList_NotRejected_OnlyID_BlockPeriodDataMerg"
                End If
            End If

            CType(Me.Master.FindControl("form1"), HtmlForm).DefaultButton = Me.btndefault.UniqueID
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
                If Not GenCall() Then
                    Throw New Exception()
                End If
            End If


        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
        End Try


    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "openFemaleInfo", "openFemaleInfo();", True)
        If (Request.QueryString("mode") = "4") Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "disableControl255", "AuditdisableControl();", True)
        End If

    End Sub


#End Region

#Region "Fill Function"
    Private Function fillchkvICFLanguageCodeId(Optional ByVal All As Boolean = False) As Boolean
        Dim ds_LanguageMst As New DataSet

        Try
            Wstr = "cActiveFlag<>'N' and cStatusIndi<>'D'"
            If Not Me.objHelp.getSubjectLanguageMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_LanguageMst, estr) Then
                Throw New Exception("Error While Getting SubjectLanguageMst " + estr.Trim.ToString())

            End If

            For Each dr As DataRow In ds_LanguageMst.Tables(0).Rows
                Dim lst As New ListItem
                lst.Value = dr("vLanguageId")
                lst.Text = dr("vLanguageName").ToString().ToUpper()
                lst.Attributes.Add("data-lng", dr("vLanguageId"))
                Me.chkvICFLanguageCodeId.Items.Add(lst)
            Next

            'Me.chkvICFLanguageCodeId.DataSource = ds_LanguageMst.Tables(0).DefaultView.ToTable(True, "vLanguageId,vLanguageName".Split(","))
            'Me.chkvICFLanguageCodeId.DataValueField = "vLanguageId"
            'Me.chkvICFLanguageCodeId.DataTextField = "vLanguageName"
            'Me.chkvICFLanguageCodeId.DataBind()

            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
            Return False
        End Try
    End Function

    Private Function fillHabitGrid() As Boolean
        Dim DsHabitDetail As New DataSet
        Dim dsHabitMst As New DataSet
        Dim dr As DataRow
        Dim drMst As DataRow
        Dim estr As String = String.Empty



        Try

            Wstr = "1=2"
            If Me.HSubjectId.Value.ToString.Trim() <> "" Then
                Wstr = " vSubjectId='" + Me.HSubjectId.Value.ToString.Trim() + "' "
            End If
            If Not objHelp.GetSubjectHabitDetails(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                          DsHabitDetail, eStr_Retu) Then
                ShowErrorMessage(eStr_Retu, "")
                Exit Function
            End If


            If Not Me.objHelp.GetSubjectHabitMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, dsHabitMst, estr) Then
                Me.ObjCommon.ShowAlert(estr, Me.Page)
                Return False
            End If

            If DsHabitDetail.Tables(0).Rows.Count < 1 Then

                For Each drMst In dsHabitMst.Tables(0).Rows
                    dr = DsHabitDetail.Tables(0).NewRow()
                    dr("vHabitId") = drMst("vHabitId")
                    dr("vHabitDetails") = drMst("vHabitDetails")
                    dr("cHabitFlag") = "N"
                    DsHabitDetail.Tables(0).Rows.Add(dr)
                    DsHabitDetail.Tables(0).AcceptChanges()
                Next
            End If

            If DsHabitDetail.Tables(0).Rows.Count > 0 Then

                For Each drMst In DsHabitDetail.Tables(0).Rows
                    If Not (drMst("cHabitFlag") = "N") Then
                        ViewState("Habit") = "Y"
                    End If
                Next
            End If

            If (ViewState("Habit") = "Y") Then
                Image38.Style.Add("display", "inline")
                ImgUpdateHabits.Style.Add("display", "inline")
                GVHabits.Enabled = False
            Else
                Image38.Style.Add("display", "none")
                ImgUpdateHabits.Style.Add("display", "none")
            End If

            If Not (ViewState(VS_IsEdit) Is Nothing) Then
                ImgUpdateHabits.Style.Add("display", "inline")
            End If
            Me.GVHabits.DataSource = DsHabitDetail.Tables(0)
            Me.GVHabits.DataBind()
            Me.btnQC.Visible = False
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...fillHabitGrid")
            Return False
        End Try

    End Function

#End Region

#Region "Grid Events"

    Protected Sub GVHabits_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVHabits.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.CssClass = "Habits"
                If Not IsNothing(e.Row.Cells(GVHC_HabitFlag).Text) AndAlso e.Row.Cells(GVHC_HabitFlag).Text.Trim() = "" Then
                    CType(e.Row.FindControl("ddlHebitType"), DropDownList).SelectedValue = e.Row.Cells(GVHC_HabitFlag).Text.Trim()
                End If

                Dim txtEndDate As TextBox = CType(e.Row.FindControl("txtEndDate"), TextBox)
                Dim txtConsumption As TextBox = CType(e.Row.FindControl("txtConsumption"), TextBox)
                Dim ddlHebitType As DropDownList = CType(e.Row.FindControl("ddlHebitType"), DropDownList)

                CType(e.Row.FindControl("ddlHebitType"), DropDownList).Attributes.Add("OnChange", "validate(" + ddlHebitType.ClientID + _
                                                    "," + txtConsumption.ClientID + "," + txtEndDate.ClientID + ")")

                If CType(e.Row.FindControl("ddlHebitType"), DropDownList).SelectedValue = "N" Then
                    CType(e.Row.FindControl("txtConsumption"), TextBox).Enabled = False
                    CType(e.Row.FindControl("txtEndDate"), TextBox).Enabled = False
                End If
                If CType(e.Row.FindControl("ddlHebitType"), DropDownList).SelectedValue = "C" Then
                    CType(e.Row.FindControl("txtConsumption"), TextBox).Enabled = True
                    CType(e.Row.FindControl("txtEndDate"), TextBox).Enabled = False
                End If
                If CType(e.Row.FindControl("ddlHebitType"), DropDownList).SelectedValue = "P" Then
                    CType(e.Row.FindControl("txtConsumption"), TextBox).Enabled = True
                    CType(e.Row.FindControl("txtEndDate"), TextBox).Enabled = True
                End If

            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString(), "...Error while GVHabits.RowDataBound")
        End Try
    End Sub

    Protected Sub GVHabits_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVHabits.RowCreated

        e.Row.Cells(GVHC_SubjectHabitDetailsNo).Style.Add("display", "none")
        e.Row.Cells(GVHC_SubjectId).Visible = False
        e.Row.Cells(GVHC_ScreenId).Style.Add("display", "none")
        e.Row.Cells(GVHC_HabitId).Visible = False
        e.Row.Cells(GVHC_HabitFlag).Visible = False

    End Sub

#End Region

#Region "Button Click Events"

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
        Dim dsSubjectProof As DataSet = Nothing
        Dim dtTemp As DataTable = Nothing

        Try

            Choice = Me.ViewState(VS_Choice)

            Me.HFADD.Value = "Y"
            If (ViewState("Edit") = "Y") Then
                btnEdit_Click(Nothing, Nothing)
            End If

            If Me.ddlProofType.SelectedIndex = 0 Then
                Me.ObjCommon.ShowAlert("Please Select a Proof Type.", Me.Page)
                Exit Sub
            End If

            Me.HFFileName.Value += Me.FlAttachment.FileName.Trim() + ","
            'If Me.FlAttachment.FileName.Trim() = "" Then
            '    Me.ObjCommon.ShowAlert("Please Select A File To Upload", Me.Page)
            '    Exit Sub
            'End If

            If Not Me.objHelp.GetSubjectProofDetails("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                          dsSubjectProof, estr) Then
                Me.ObjCommon.ShowAlert(estr, Me.Page)
            End If

            If Me.ViewState("Edit") = "Y" Then
                Me.ViewState(VS_GVDtSubjectProofDetails) = dsSubjectProof.Tables(0)
                dt_GVSubjectProof = GVSubjectProof.DataSource()
                dtTemp = CType(Me.ViewState(VS_GVDtSubjectProofDetails), DataTable)
                Dim drTemp As DataRow = dtTemp.NewRow

                drTemp("nSubjectProofNo") = 0
                drTemp("vSubjectId") = ""
                drTemp("iTranNo") = 0
                drTemp("vProofType") = Me.ddlProofType.SelectedItem.Text.ToString()

                If Me.ddlProofType.SelectedItem.Text.Trim.ToUpper() = "OTHERS" Then
                    drTemp("vProofType") = "OTHERS:" + Me.txtAttach.Text.Trim()
                End If

                drTemp("iModifyBy") = Session(S_UserID)
                drTemp("cStatusIndi") = "N"
                drTemp("vUpdateRemarks") = Me.HFRemarks.Value

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

                If Not (String.IsNullOrEmpty(Me.FlAttachment.PostedFile.FileName)) Then
                    Me.FlAttachment.SaveAs(strPath + Path.GetFileName(Me.FlAttachment.PostedFile.FileName))
                End If


                'End **********************

                drTemp("vProofPath") = "~\" + System.Configuration.ConfigurationSettings.AppSettings("TempSubjectProofDetails") + dirNo.ToString() + "\" + Me.ViewState(VS_TempTransactionNo).ToString() + "\" + Path.GetFileName(Me.FlAttachment.PostedFile.FileName)

                dtTemp.Rows.Add(drTemp)
                dtTemp.AcceptChanges()

                Me.ViewState(VS_TempSubjectProof) = dtTemp
                If GVSubjectProof.Rows.Count > 0 Then

                    dt_GVSubjectProof = GVSubjectProof.DataSource
                    Dim dr1 As DataRow = dt_GVSubjectProof.NewRow

                    dr1("nSubjectProofNo") = 0
                    dr1("vSubjectId") = ""
                    dr1("iTranNo") = 0
                    dr1("vProofType") = Me.ddlProofType.SelectedItem.Text.ToString()

                    If Me.ddlProofType.SelectedItem.Text.Trim.ToUpper() = "OTHERS" Then
                        dr1("vProofType") = "OTHERS:" + Me.txtAttach.Text.Trim()
                    End If

                    dr1("iModifyBy") = Session(S_UserID)
                    dr1("cStatusIndi") = "N"
                    dr1("vUpdateRemarks") = Me.HFRemarks.Value
                    dr1("vProofPath") = "~\" + System.Configuration.ConfigurationSettings.AppSettings("TempSubjectProofDetails") + dirNo.ToString() + "\" + Me.ViewState(VS_TempTransactionNo).ToString() + "\" + Path.GetFileName(Me.FlAttachment.PostedFile.FileName)
                    dt_GVSubjectProof.Rows.Add(dr1)
                    dt_GVSubjectProof.AcceptChanges()
                    If Not BindGridSubjectProof(dt_GVSubjectProof) Then
                        Exit Sub
                    End If

                Else
                    If Not BindGridSubjectProof(dtTemp) Then
                        Exit Sub
                    End If
                    btnActualSave_Click(Nothing, Nothing)

                End If

            Else
                dt_GVSubjectProof = CType(Me.ViewState(VS_GVDtSubjectProofDetails), DataTable)

                dr = dt_GVSubjectProof.NewRow
                'nSubjectProofNo,vSubjectId,iTranNo,vProofType,vProofPath,iModifyBy,dModifyOn,cStatusIndi
                dr("nSubjectProofNo") = 0
                dr("vSubjectId") = ""
                dr("iTranNo") = 0

                dr("vProofType") = Me.ddlProofType.SelectedItem.Text.ToString()

                If Me.ddlProofType.SelectedItem.Text.Trim.ToUpper() = "OTHERS" Then
                    dr("vProofType") = "OTHERS:" + Me.txtAttach.Text.Trim()
                End If

                dr("iModifyBy") = Session(S_UserID)
                dr("cStatusIndi") = "N"

                dr("vProofPath") = "~\" + System.Configuration.ConfigurationSettings.AppSettings("TempSubjectProofDetails") + dirNo.ToString() + "\" + Me.ViewState(VS_TempTransactionNo).ToString() + "\" + Path.GetFileName(Me.FlAttachment.PostedFile.FileName)

                dt_GVSubjectProof.Rows.Add(dr)
                dt_GVSubjectProof.AcceptChanges()

                If Not BindGridSubjectProof(dt_GVSubjectProof) Then
                    Exit Sub
                End If

            End If











            'dr = dt_GVSubjectProof.NewRow
            ''nSubjectProofNo,vSubjectId,iTranNo,vProofType,vProofPath,iModifyBy,dModifyOn,cStatusIndi
            'dr("nSubjectProofNo") = 0
            'dr("vSubjectId") = ""
            'dr("iTranNo") = 0
            'dr("vProofType") = Me.ddlProofType.SelectedItem.Text.ToString()

            'If Me.ddlProofType.SelectedItem.Text.Trim.ToUpper() = "OTHERS" Then
            '    dr("vProofType") = "OTHERS:" + Me.txtAttach.Text.Trim()
            'End If

            'dr("iModifyBy") = Session(S_UserID)
            'dr("cStatusIndi") = "N"

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

            If Not (String.IsNullOrEmpty(Me.FlAttachment.PostedFile.FileName)) Then
                Me.FlAttachment.SaveAs(strPath + Path.GetFileName(Me.FlAttachment.PostedFile.FileName))
            End If


            'End **********************

            'dr("vProofPath") = "~\" + System.Configuration.ConfigurationSettings.AppSettings("TempSubjectProofDetails") + dirNo.ToString() + "\" + Me.ViewState(VS_TempTransactionNo).ToString() + "\" + Path.GetFileName(Me.FlAttachment.PostedFile.FileName)

            'dt_GVSubjectProof.Rows.Add(dr)
            'dt_GVSubjectProof.AcceptChanges()
            'Me.ViewState(VS_TempSubjectProof) = dt_GVSubjectProof

            'If Not BindGridSubjectProof(dt_GVSubjectProof) Then
            '    Exit Sub
            'End If

            If (ViewState("Edit") = "Y") Then
                ''  dr("vSubjectId") = ""
                ViewState(VS_SubjectId) = Me.HSubjectId.Value
                '' dr("cStatusIndi") = "N"
                Me.ViewState(VS_GVDtSubjectProofDetails) = dsSubjectProof.Tables(0)
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "enableControlAT", "funAttachDoc();", True)
                '' Me.Image36.Style.Remove("Class")
                '' Me.Image36.Style.Add("Class", "UpdateControl")
                Me.HFAttach.Value = "Y"
                'If Not ActualSaveSubjectProof() Then
                '    Throw New Exception("Error while save actaul subject proof")
                'End If


            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....btnAttach_Click")
        Finally
            Me.btnAttach.Visible = True
            If (ViewState("Edit") <> "Y") Then
                txtAge.Text = Me.HFAge.Value
                txtbmi.Text = Me.HfBMI.Value
                txtInitials.Text = Me.HFInitials.Value
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "validate123", "validateHabit();", True)
            End If
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "geta", "getAge();", True)
            '' ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "BMI", "calcBMI();", True)
        End Try

    End Sub

    Protected Sub btnsave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsave.Click

        Dim ds_SubjectMst As New DataSet
        Dim SubjectId As String = String.Empty
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum
        btnsave.Enabled = True
        Dim BDate As Date

        'DateTime date = DateTime.ParseExact(this.Text, "dd/MM/yyyy", null);

        If Not SameSubjectValidation() Then
            Exit Sub
        End If


        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "validation", "Validation12();", True)

        'If (Convert.ToDateTime(txtdBirthDate.Text) > DateTime.Now()) Then
        '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "enableControlmode412", "enableControlmode4();", True)
        '    Me.ObjCommon.ShowAlert("Date should be previous or equal to current date.", Me.Page)
        '    Exit Sub
        'End If


        If txtdBirthDate.Text <> "" Then
            If (Convert.ToDateTime(txtdBirthDate.Text) > DateTime.Now()) Then
                ''ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "enableControlmode412", "enableControlmode4();", True)
                Me.ObjCommon.ShowAlert("Date should be previous or equal to current date.", Me.Page)
                Exit Sub
            End If
        End If



        If txtdEnrollmentDate.Text <> "" Then
            If (Convert.ToDateTime(txtdEnrollmentDate.Text) > DateTime.Now()) Then
                '' ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "enableControlmode412", "enableControlmode4();", True)
                Me.ObjCommon.ShowAlert("Enrolment date should be previous or equal to current date.", Me.Page)
                Exit Sub
            End If

        End If



        If Not (String.IsNullOrEmpty(txtAge.Text)) Then
            If (Convert.ToInt16(txtAge.Text) < 18) Then
                '' ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "enableControlmode412", "enableControlmode4();", True)
                Me.ObjCommon.ShowAlert("Age should be more than 18 Years.", Me.Page)
                Exit Sub
            End If
        Else
            '' ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "enableControlmode412", "enableControlmode4();", True)
            Me.ObjCommon.ShowAlert("Age should be more than 18 Years.", Me.Page)
            Exit Sub

        End If
        If Not (String.IsNullOrEmpty(txtdLastMenstrualDate.Text)) Then
            If (Convert.ToDateTime(txtdLastMenstrualDate.Text) > DateTime.Now().ToString("dd-MMM-yyyy")) Then
                ''ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "enableControlmode412", "enableControlmode4();", True)
                Me.ObjCommon.ShowAlert("Date should be previous or equal to current date.", Me.Page)
                Exit Sub
            End If
        End If

        If Not (String.IsNullOrEmpty(txtdLastDelivaryDate.Text)) Then
            If (Convert.ToDateTime(txtdLastDelivaryDate.Text) > DateTime.Now()) Then
                ''ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "enableControlmode412", "enableControlmode4();", True)
                Me.ObjCommon.ShowAlert("Date of last Delivery  should be previous or equal to current date.", Me.Page)
                Exit Sub
            End If
        End If

        If Not (String.IsNullOrEmpty(txtdAbortionDate.Text)) Then
            If (Convert.ToDateTime(txtdAbortionDate.Text) > DateTime.Now()) Then
                '' ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "enableControlmode412", "enableControlmode4();", True)
                Me.ObjCommon.ShowAlert("Date Of Last Abortion should be less than or equal to Current date.", Me.Page)
                Exit Sub
            End If
        End If


        Try

            If Not AssignValues(ds_SubjectMst) Then
                Throw New Exception("Error While Assignvalues")
            End If
            ds_SubjectMst = New DataSet
            ds_SubjectMst.Tables.Add(CType(Me.ViewState(VS_DtSubjectMst), Data.DataTable).Copy())

            If Not (CType(Me.ViewState(VS_DtSubjectHabitDetails), Data.DataTable) Is Nothing) Then
                ds_SubjectMst.Tables.Add(CType(Me.ViewState(VS_DtSubjectHabitDetails), Data.DataTable).Copy())
            End If

            ds_SubjectMst.AcceptChanges()

            Choice = IIf(ViewState(VS_IsEdit) Is Nothing, WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                                                            WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit)
            If ((ds_SubjectMst.Tables(0).Rows(0)(92).ToString() <> String.Empty Or Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add) AndAlso (ViewState(VS_IsEdit) Is Nothing Or IIf(ViewState(VS_IsEdit) Is Nothing, "", ViewState(VS_IsEdit)) = "Y")) Then
                If Not objLambda.Save_SubjectMst(Choice, ds_SubjectMst, Session(S_UserID).ToString, SubjectId, estr, "") Then
                    Throw New Exception("Error while save subject details")
                End If
            ElseIf (ViewState("IsModifyHabit") = "Y") Then
                ds_SubjectMst.Tables(0).Rows(0)(92) = "--"
                ds_SubjectMst.Tables(0).AcceptChanges()
                If Not objLambda.Save_SubjectMst(Choice, ds_SubjectMst, Session(S_UserID).ToString, SubjectId, estr, "") Then
                    Throw New Exception("Error while save subject details")
                End If
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "hfdskghjksdhgh", " location.reload(true);", True)
                Exit Sub
            End If

            Me.ViewState(VS_SubjectId) = SubjectId

            If (ViewState(VS_IsEdit) <> "Y") Then
                If Not ActualSaveSubjectProof(SubjectId) Then
                    Throw New Exception("Error while save actaul subject proof")
                End If
            End If


            'For inhouse mode
            If Me.Request.QueryString("mode") = InHouse_Mode Then
                Me.Response.Redirect("frmSubjectPIFMst_New.aspx?mode=" & InHouse_Mode & "&LastSubjectId=" & Me.ViewState(VS_SubjectId), False)
            Else
                If Not IsNothing(Me.Request.QueryString("SearchSubjectId")) AndAlso Me.Request.QueryString("SearchSubjectId").Trim() <> "" Then

                    ScriptManager.RegisterClientScriptBlock(Me.Page(), Me.GetType(), "Redir", "AlertAndRedirect('frmSubjectPIFMst_New.aspx?SearchSubjectId=" & Me.ViewState(VS_SubjectId) & "&SearchSubjectText=" & Me.txtSubject.Text.ToString() & "&Saved=True','Subject Saved Successfully with SubjectId = " & Me.Request.QueryString("SearchSubjectId").Trim() & ".')", True)

                    ' Me.Response.Redirect("frmSubjectPIFMst_New.aspx?SearchSubjectId=" & Me.ViewState(VS_SubjectId) & "&SearchSubjectText=" & Me.txtSubject.Text.ToString() & "&Saved=True", False)

                    'ObjCommon.ShowAlertAndRedirect("Subject Saved Successfully with SubjectId = " & Me.Request.QueryString("SearchSubjectId").Trim() & ".", "frmSubjectPIFMst_New.aspx?SearchSubjectId=" & Me.ViewState(VS_SubjectId) & "&SearchSubjectText=" & Me.txtSubject.Text.ToString() & "&Saved=True", Me)
                    'Me.ObjCommon.ShowAlert("Subject Saved Successfully with SubjectId = " & SubjectId & ".", Me.Page)
                    popup = False
                    btnsave.Enabled = True
                Else
                    ScriptManager.RegisterClientScriptBlock(Me.Page(), Me.GetType(), "Redir", "AlertAndRedirect('frmSubjectPIFMst_New.aspx?SearchSubjectId=" & Me.ViewState(VS_SubjectId) & "&SearchSubjectText=" & Me.txtSubject.Text.ToString() & "&Saved=True','Subject Saved Successfully with SubjectId = " & Me.ViewState(VS_SubjectId) & ".')", True)
                    'Me.Response.Redirect("frmSubjectPIFMst_New.aspx?SearchSubjectId=" & Me.ViewState(VS_SubjectId) & "&SearchSubjectText=" & Me.txtSubject.Text.ToString() & "&Saved=True", False)
                    'ObjCommon.ShowAlertAndRedirect("Subject Saved Successfully with SubjectId = " & Me.ViewState(VS_SubjectId) & ".", "frmSubjectPIFMst_New.aspx?SearchSubjectId=" & Me.ViewState(VS_SubjectId) & "&SearchSubjectText=" & Me.txtSubject.Text.ToString() & "&Saved=True", Me)
                    popup = False
                    'Me.ObjCommon.ShowAlert("Subject Saved Successfully with SubjectId = " & SubjectId & ".", Me.Page)
                    ' ObjCommon.ShowAlert("Subject Saved Successfully with SubjectId = " & Me.Request.QueryString("SearchSubjectId").Trim() & ".", Me)
                End If

            End If


            'If Not Me.Request.QueryString("mode") = InHouse_Mode Then

            '    Me.Response.Redirect("frmSubjectPIFMst_New.aspx?SearchSubjectId=" & Me.ViewState(VS_SubjectId) & "&SearchSubjectText=" & Me.txtSubject.Text.ToString() & "&Saved=True", False)

            'Else
            '    Me.Response.Redirect("frmSubjectPIFMst_New.aspx?mode=" & InHouse_Mode & "&SearchSubjectId=" & Me.ViewState(VS_SubjectId) & "&SearchSubjectText=" & Me.txtSubject.Text.ToString() & "&Saved=True", False)
            'End If

            'ScriptManager.RegisterClientScriptBlock(Me.Page(), Me.GetType(), "Redir", "AlertAndRedirect('frmmainpage.aspx','Password Changed Successfully')", True)
            'Me.ObjCommon.ShowAlert("Subject Saved Successfully with SubjectId = " & SubjectId & ".", Me.Page)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString(), "")
        End Try
    End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        Dim dsSubject As DataSet = Nothing
        Dim dsSubjectProof As DataSet = Nothing


        Dim subjectId As String

        Try
            If Not ResetPage() Then
                Throw New Exception("Error while refresh page...")
            End If
            If Me.Request.QueryString("Mode") = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                Me.HFEdit.Value = ""
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "disableControl12", "disableControl();", True)

            Else
                Me.HFEdit.Value = "Y"
            End If

            If Not (Me.Request.QueryString("SearchSubjectId") Is Nothing AndAlso Me.Request.QueryString("SearchSubjectText") Is Nothing) Then
                Try
                    Me.txtSubject.Text = Me.Request.QueryString("SearchSubjectText").ToString
                Catch ex As Exception
                    Me.txtSubject.Text = String.Empty

                End Try

                Me.HSubjectId.Value = Me.Request.QueryString("SearchSubjectId").ToString
            End If
            subjectId = HSubjectId.Value.Trim
            If subjectId = "" Then
                HSubjectId.Value = Request.QueryString("SubjectId")
            End If




            If Not objHelp.GetView_SubjectMaster("vSubjectId='" + HSubjectId.Value.Trim() + "'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                              dsSubject, eStr_Retu) Then
                Throw New Exception(estr)
            End If
            ViewState("Edit") = "Y"
            If dsSubject.Tables(0).Rows.Count > 0 Then
                ViewState(VS_DtSubjectMaster) = dsSubject.Tables(0)
                If Not setPageData(dsSubject) Then
                    Throw New Exception("Error while get data...")
                End If

                If Not AssingAttribute() Then
                    Throw New Exception("Error while Assigning Data...")
                End If
            End If
            dsSubject = Nothing
            If Not objHelp.GetData("SubjectProofDetails", "*", "vSubjectId='" + HSubjectId.Value.Trim() + "' And cStatusIndi<>'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsSubject, estr) Then
                Throw New Exception(estr)
            End If

            dsSubject.Tables(0).DefaultView.RowFilter = "vProofType  <> '' "
            dsSubject.AcceptChanges()

            If dsSubject.Tables(0).Rows.Count > 0 Then
                Me.GVSubjectProof.DataSource = dsSubject.Tables(0)
                Me.GVSubjectProof.DataBind()

                If (ViewState(VS_IsEdit) = "Y") Then
                    Me.Image36.Style.Add("display", "inline")
                    Me.Image37.Style.Add("display", "inline")
                End If
            Else
                If (ViewState(VS_IsEdit) = "Y") Then
                    Me.Image37.Style.Add("display", "inline")
                End If
                Me.Image36.Style.Add("display", "none")

            End If


            dsSubject = Nothing
            If Not fillHabitGrid() Then
                Throw New Exception("Error while fill habit")
            End If
            dsSubject = Nothing
            Wstr = "cRecordType='P' and iTranNo=(select Max(itranNO) from SubjectBlobDetails where cRecordType='P' and vSubjectId='" & HSubjectId.Value.ToString.Trim & "') and vSubjectId='" & HSubjectId.Value.ToString.Trim & "'"

            If Not Me.objHelp.getSubjectBlobDetails(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsSubject, estr) Then
                MsgBox("Error while getting Data." + vbCrLf + estr)
                Exit Sub
            End If

            If dsSubject.Tables(0).Rows.Count > 0 Then
                Me.Image1.ImageUrl = "frmPIFImage.aspx?subjectid=" + Me.HSubjectId.Value.Trim()
                ViewState("Edit") = "Y"
            ElseIf dsSubject.Tables(0).Rows.Count <= 0 Then
                Me.Image1.ImageUrl = "~/images/NotFound.gif"
            End If



            '' Me.btnsave.Style.Add("display", "none")
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "disableControl", "disableControl();", True)


            If Not fillQCGrid() Then
                Exit Sub
            End If

            If (Request.QueryString("mode") = "4") Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "disableControl266", "AuditdisableControl();", True)
            End If

            If (ViewState(VS_IsEdit) = "Y") Then
                If Not Createpdf() Then
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
        End Try
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

            If Me.Request.QueryString("mode") = 4 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "enableControlmode412", "enableControlmode4();", True)
                '' ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "enableControl12", "enableControl();", True)
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "disableControl232", "AuditdisableControl();", True)
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".........btnMSR_Click")
        End Try

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If (Me.Request.QueryString("mode") = "") Then
            Response.Redirect("frmSubjectPIFMst_New.aspx?mode=1")
        End If
        Response.Redirect("frmSubjectPIFMst_New.aspx?mode=" + Me.Request.QueryString("mode"))
    End Sub

    Protected Sub btnHide_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHide.Click
        Page_Load(Nothing, Nothing)
    End Sub

    Protected Sub btnActualSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnActualSave.Click
        Dim dr As DataRow
        Dim dt As New DataTable
        Dim ds As New DataSet
        Dim strPath As String = String.Empty
        Dim di As DirectoryInfo
        Dim Retu_TranNo As String = String.Empty
        ''  Dim fileName As String = String.Empty
        Dim filePath As String = String.Empty
        Dim sourcePath As String = String.Empty

        Try
            dt = CType(Me.ViewState(VS_TempSubjectProof), DataTable)
            Dim dv As DataView = New DataView(dt)
            dv.RowFilter = "nSubjectProofno = '0'"
            dt = dv.ToTable()
            Dim i As Integer = -1
            Dim FileNAMe As String = Me.HFFileName.Value
            Dim fn = FileNAMe.Split(",")
            For Each dr In dt.Rows
                i += 1
                '' fileName = dr("vProofType")
                sourcePath = "~/" + System.Configuration.ConfigurationSettings.AppSettings("FolderForSubjectProof")
                sourcePath += Me.HSubjectId.Value
                sourcePath += "/"
                sourcePath += fn(i)
                dr("vSubjectId") = Me.HSubjectId.Value
                dr("iTranNo") = 1
                dr("vProofPath") = sourcePath
                dr("vUpdateRemarks") = Me.HFRemarks.Value

            Next
            dt.AcceptChanges()
            ds.Tables.Add(dt)
            ds.Tables(0).TableName = "SubjectProofDetails"
            If ds.Tables(0).Rows.Count > 0 Then

                dt.TableName = "SubjectProofDetails"
                If Not Me.objLambda.Save_SubjectProofDetails(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds, Me.Session(S_UserID), Retu_TranNo, estr) Then
                    Throw New Exception(estr)
                End If


                strPath = Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings("FolderForSubjectProof"))
                strPath += Me.HSubjectId.Value.ToString + "\" + Retu_TranNo + "\"
                di = New DirectoryInfo(strPath)
                If Not di.Exists() Then
                    Directory.CreateDirectory(strPath)
                End If

                sourcePath = Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings("TempSubjectProofDetails"))
                sourcePath += CType(Me.Session(S_UserID), String) + "\" + CType(Me.ViewState(VS_TempTransactionNo), String).Trim() + "\" + FileNAMe
                strPath += FileNAMe

                strPath = strPath.Replace(",", "")
                sourcePath = sourcePath.Replace(",", "")
                If Not File.Exists(strPath) Then
                    If File.Exists(sourcePath) Then
                        File.Copy(sourcePath, strPath)
                    End If
                End If
                ' End If
            End If

            '' Return True

            btnEdit_Click(Nothing, Nothing)
            fillSubjectProofGrid()
            btnHide_Click(Nothing, Nothing)

            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "disableControl134456", "ReloadPage1();", True)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".........ActualSaveSubjectProof")
            '' Return False
        Finally
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Redirect123456", "Redirect();", True)
        End Try

    End Sub

    Protected Sub btnBindProof_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBindProof.Click
        fillSubjectProofGrid()
    End Sub

#End Region

#Region "BtnQCSave"

    'Protected Sub BtnQCSaveSend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnQCSaveSend.Click
    '    Dim Ds_QC As New DataSet
    '    Dim estr As String = String.Empty
    '    Dim QCMsg As String = String.Empty
    '    Dim fromEmailId As String = String.Empty
    '    Dim toEmailId As String = String.Empty
    '    Dim password As String = String.Empty
    '    Dim ccEmailId As String = String.Empty
    '    Dim SubjectLine As String = String.Empty
    '    Dim wStr As String = String.Empty
    '    'Dim ds_EmailAlert As New DataSet
    '    Try

    '        If Me.lblResponse.Text = "" And CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
    '            Me.ObjCommon.ShowAlert("Please Click Response Button Of Particular QC Comments Against Which You Want To Response", Me.Page)

    '            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ShowHide", "QCDivShowHide('ST');", True)

    '            Exit Sub
    '        End If

    '        If Not AssignValues_QC(Ds_QC) Then
    '            Exit Sub
    '        End If

    '        If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then

    '            If Not Me.objLambda.Save_SubjectMasterQC(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, Ds_QC, _
    '                Me.Session(S_UserID), estr) Then

    '                Me.ObjCommon.ShowAlert(estr, Me.Page)
    '                Exit Sub

    '            End If

    '            'If Not fillQCGrid() Then
    '            '    Exit Sub
    '            'End If

    '            QCMsg = "QC On PIF of " + Me.txtSubject.Text.Trim() + " <br/><br/>QC : " + Me.RBLQCFlag.SelectedItem.Text.Trim() + _
    '                    "<br/><br/>QC Comments: " + Me.txtQCRemarks.Text + "<br/><br/>" + _
    '                    "Comments Given By : " + Me.Session(S_FirstName).ToString.Trim() + " " + Me.Session(S_LastName).ToString.Trim()

    '            '*****************Sending Mail*************************************

    '            fromEmailId = ConfigurationSettings.AppSettings("Username")
    '            password = ConfigurationSettings.AppSettings("Password")

    '            'wStr = "nEmailAlertId =" + Email_QCOFPIF.ToString() + " And cStatusIndi <> 'D'"
    '            'If Not Me.objHelp.GetEmailAlertMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
    '            '                    ds_EmailAlert, estr) Then

    '            '    Me.ObjCommon.ShowAlert("Error While Getting Data From EmailAlert : " + estr, Me.Page)
    '            '    Exit Sub

    '            'End If

    '            'If ds_EmailAlert.Tables(0).Rows.Count > 0 Then

    '            toEmailId = Me.txtToEmailId.Text.Trim() 'ds_EmailAlert.Tables(0).Rows(0)("vToUserEmailId").ToString()
    '            ccEmailId = Me.txtCCEmailId.Text.Trim() 'ds_EmailAlert.Tables(0).Rows(0)("vCCUserEmailId").ToString()
    '            SubjectLine = "QC On PIF of " + HSubjectId.Value.Trim() 'ds_EmailAlert.Tables(0).Rows(0)("vSubjectLine").ToString()

    '            Dim sn As New SendMail(Server, Request, Session, SubjectLine, QCMsg, fromEmailId, password, toEmailId, ccEmailId)

    '            'Changed on 26-Aug-2009
    '            If Not sn.Send(Server, Response, Session, , fromEmailId, password) Then
    '                Me.ObjCommon.ShowAlert("Record Saved Successfully But Email Is Not Sent Due To Some Reason", Me.Page)
    '                Exit Sub
    '            End If
    '            '****************************************************

    '            sn = Nothing
    '            Me.ObjCommon.ShowAlert("Record Saved Successfully", Me.Page)

    '            'Else
    '            '    Me.ObjCommon.ShowAlert("Record Saved Successfully But Email Is Not Sent Due To Some Reason", Me.Page)
    '            'End If

    '        Else 'For Response

    '            If Not Me.objLambda.Save_SubjectMasterQC(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, Ds_QC, _
    '            Me.Session(S_UserID), estr) Then

    '                Me.ObjCommon.ShowAlert(estr, Me.Page)
    '                Exit Sub

    '            End If

    '            Me.ObjCommon.ShowAlert("Response Saved Successfully", Me.Page)

    '        End If

    '        If Not fillQCGrid() Then
    '            Exit Sub
    '        End If

    '        Me.txtQCRemarks.Text = ""
    '        Me.lblResponse.Text = ""
    '        Me.RBLQCFlag.SelectedValue = "F"

    '        btnEdit_Click(Nothing, Nothing)


    '    Catch ex As Exception
    '        Me.ShowErrorMessage(ex.Message, "....BtnQCSaveSend_Click")
    '    End Try
    'End Sub

    Protected Sub BtnQCSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.BtnQCSave.Visible = True
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

            btnEdit_Click(Nothing, Nothing)


            Me.txtQCRemarks.Text = ""
            Me.lblResponse.Text = ""
            Me.RBLQCFlag.SelectedValue = "F"

            'If Me.Request.QueryString("mode") = 4 Then
            '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "enableControlmode412", "enableControlmode4();", True)
            'End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....BtnQCSave_Click")
        Finally
            Me.BtnQCSave.Visible = True
            If Me.Request.QueryString("mode") = 4 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "enableControlmode412", "enableControlmode4();", True)
            End If
        End Try
    End Sub

#End Region

#Region "GenCall"

    Private Function GenCall() As Boolean
        Dim dt_SubjectMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try
            Me.ViewState(VS_TempTransactionNo) = 0
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
            GenCall = False
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
            GenCall_Data = False
            Me.ShowErrorMessage(ex.Message, "..GenCall_Data")
        Finally
        End Try


    End Function

#End Region

#Region "GenCall_ShowUI"

    Private Function GenCall_ShowUI(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                                      ByVal dt_Dist As DataTable) As Boolean
        Try
            FillPopulation()
            FillLocation()

            Page.Title = ":: Personal Information Form  :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            If popup = False Then

                If Not IsNothing(Me.Request.QueryString("LastSubjectId")) AndAlso Me.Request.QueryString("LastSubjectId").Trim() <> "" Then
                    ObjCommon.ShowAlert("Subject Saved Successfully with SubjectId = " & Me.Request.QueryString("LastSubjectId").Trim() & ".", Me)
                    btnsave.Enabled = True
                    popup = True
                End If
                If Not IsNothing(Me.Request.QueryString("SearchSubjectId")) AndAlso Me.Request.QueryString("SearchSubjectId").Trim() <> "" Then
                    ObjCommon.ShowAlert("Subject Saved Successfully with SubjectId = " & Me.Request.QueryString("SearchSubjectId").Trim() & ".", Me)
                    btnsave.Enabled = True
                    popup = True
                End If
            End If
            '' txtSubject.Text = Me.Request.QueryString("SubjectId")
            '' HSubjectId.Value = Me.Request.QueryString("SubjectId")
            ''rblcRegular.Items(0).Selected = True
            '' rblcChildrenHelath.Items(0).Selected = True
            '' rblcLoctating.Items(0).Selected = True
            'rblcContraception.Items(0).Selected = True
            '' rblcLastMenstrualIndi.SelectedValue = "1"
            '' rblcIsVolunteerinBearingAge.SelectedValue = "Y"
            ''Me.rblcAbortions.SelectedValue = "N"

            If Not fillchkvICFLanguageCodeId() Then
                Throw New Exception("Error while get ICF Language")
            End If
            If Not fillHabitGrid() Then
                Throw New Exception("Error while fill Habits")
            End If
            If Me.Request.QueryString("Mode") = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View Then
                Me.BtnNew.Visible = False
                Me.btnQC.Visible = True
                Me.btnMSR.Visible = True
            ElseIf Me.Request.QueryString("Mode") = "" Then
                Me.btnQC.Visible = False

            Else
                Me.BtnNew.Enabled = True
                'Me.btnQC.Visible = True
                Me.btnMSR.Enabled = True
            End If

            'Contex key and webmethod call add according to mode(inhouse or babe) :start
            'Me.AutoCompleteExtender1.ContextKey = Session(S_LocationCode)
            'Me.AutoCompleteExtender1.ServiceMethod = "GetSubjectCompletionList_NotRejected"
            Me.AutoCompleteExtender1.ServiceMethod = "GetSubjectCompletionList_NotRejected_BlockPeriod"
            If System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge").Contains(Session(S_LocationCode)) = True Then
                Me.AutoCompleteExtender1.ContextKey = Me.Session(S_LocationCode)
                Me.AutoCompleteExtender1.ServiceMethod = "GetSubjectCompletionList_NotRejected_BlockPeriod"
            Else
                Me.AutoCompleteExtender1.ContextKey = System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge")
                Me.AutoCompleteExtender1.ServiceMethod = "GetSubjectCompletionList_NotRejected_BlockPeriodDataMerg"
            End If
            If Me.Request.QueryString("mode") = InHouse_Mode Then
                If System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge").Contains(Session(S_LocationCode)) = True Then
                    Me.AutoCompleteExtender1.ContextKey = "View_subjectMaster#" + " vLocationCode='" & Session(S_LocationCode) & "'"
                    Me.AutoCompleteExtender1.ServiceMethod = "GetSubjectCompletionList_NotRejected_InHouse_BlockPeriod"
                Else
                    Me.AutoCompleteExtender1.ContextKey = "View_subjectMaster#" + " vLocationCode Not In(" + System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge") + ")"
                    Me.AutoCompleteExtender1.ServiceMethod = "GetSubjectCompletionList_NotRejected_InHouse_BlockPeriod"
                End If
                'Me.AutoCompleteExtender1.ContextKey = "View_subjectMaster#" + " vLocationCode Not In(" + System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge") + ")"
                'Me.AutoCompleteExtender1.ServiceMethod = "GetSubjectCompletionList_NotRejected_InHouse_BlockPeriod"
            ElseIf Me.Request.QueryString("mode") = 4 Then
                If System.Configuration.ConfigurationManager.AppSettings("vLocationForDBMerge").Contains(Session(S_LocationCode)) = True Then
                    AutoCompleteExtender1.ServiceMethod = "GetSubjectCompletionList_NotRejected_OnlyID_BlockPeriod"
                Else
                    AutoCompleteExtender1.ServiceMethod = "GetSubjectCompletionList_NotRejected_OnlyID_BlockPeriodDataMerg"
                End If
            End If

            Me.txtnWeight.Attributes.Add("onblur", "calcBMI();")
            Me.txtvMiddleName.Attributes.Add("onblur", "SetInitial('" & Me.txtInitials.ClientID & "');")
            Me.txtvSurName.Attributes.Add("onblur", "SetInitial('" & Me.txtInitials.ClientID & "');")
            Me.txtvFirstName.Attributes.Add("onblur", "SetInitial('" & Me.txtInitials.ClientID & "');")
            Me.txtInitials.Attributes.Add("disabled", "true")
            Me.txtdEnrollmentDate.Text = Format(System.DateTime.Now, "dd-MMM-yyyy")
            'Me.txtdBirthDate.Text = Format(txtdBirthDate.Text, "dd-MMM-yyyy")
            Me.txtQCRemarks.Attributes.Add("onkeydown", "ValidateRemarks(this,'" & Me.lblcnt.ClientID & "',1000);")
            Me.btnsave.OnClientClick = "return Validation('ADD');"
            'Me.btnRemarksUpdate.OnClientClick = "return Validation('Edit');"
            If Not fillSubjectProofGrid() Then
                Exit Function
            End If
            If Not (Me.Request.QueryString("SearchSubjectId") Is Nothing AndAlso Me.Request.QueryString("SearchSubjectText") Is Nothing AndAlso Me.Request.QueryString("Saved") Is Nothing) Then
                Me.HSubjectId.Value = Me.Request.QueryString("SearchSubjectId").ToString.Trim()
                Me.txtSubject.Text = Me.Request.QueryString("SearchSubjectText").ToString.Trim()
                btnEdit_Click(Nothing, Nothing)
            End If

            If Not (Me.Request.QueryString("SubjectId") Is Nothing) Then
                Me.HSubjectId.Value = Me.Request.QueryString("SubjectId").ToString.Trim()
                Me.txtSubject.Text = Me.Request.QueryString("SubjectId").ToString.Trim()
                btnEdit_Click(Nothing, Nothing)
            End If


            GenCall_ShowUI = True

        Catch ex As Exception
            GenCall_ShowUI = False
            ShowErrorMessage(ex.Message, "......GenCall")
        Finally
        End Try

    End Function

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

            If e.Row.Cells(GVCQC_ResponseGivenBy).Text <> "&nbsp;" AndAlso Not e.Row.Cells(GVCQC_ResponseGivenBy).Text = "" Then
                CType(e.Row.FindControl("lnkResponse"), LinkButton).Enabled = False
            ElseIf e.Row.Cells(GVCQC_ResponseGivenBy).Text = "" Then
                CType(e.Row.FindControl("lnkResponse"), LinkButton).Enabled = True
            End If
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

#Region "FillQCGrid"

    Private Function fillQCGrid() As Boolean
        Dim Ds_QCGrid As New DataSet
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim ds As New DataSet
        Dim locationcode As String = Session(S_LocationCode)
        Try


            Wstr = "vSubjectId='" & Me.HSubjectId.Value.Trim() & "' AND cIsSourceDocComment = 'N'"
            If Not Me.objHelp.View_SubjectMasterQC(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        Ds_QCGrid, estr) Then
                Me.ShowErrorMessage(estr, estr)
                Return False
            End If


            If Not Ds_QCGrid Is Nothing Then

                If Ds_QCGrid.Tables(0).Rows.Count > 0 Then
                    Ds_QCGrid.Tables(0).Columns.Add(("ActualTIME"), Type.GetType("System.String"))
                    Ds_QCGrid.Tables(0).Columns.Add(("ActualTIME2"), Type.GetType("System.String"))
                    For Each dr_Qc In Ds_QCGrid.Tables(0).Rows
                        If Not dr_Qc("dQCGivenOn").ToString() = "" Then
                            If Not objHelp.Proc_ActualAuditTrailTime(CDate(dr_Qc("dQCGivenOn")).ToString("dd-MMM-yyyy HH:mm") + "##" + locationcode, ds, estr) Then '' Added by Dipen shah on 3-dec-2014
                                Throw New Exception(estr)
                            End If

                            If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                                dr_Qc("ActualTIME") = CDate(ds.Tables(0).Rows(0).Item("ActualTIME")).ToString("dd-MMM-yyyy HH:mm") + " (" + ds.Tables(0).Rows(0).Item("TimeZoneOffSet").ToString + " GMT)"
                            End If
                        End If

                        If Not dr_Qc("dResponseGivenOn").ToString() = "" Then
                            If Not objHelp.Proc_ActualAuditTrailTime(CDate(dr_Qc("dResponseGivenOn")).ToString("dd-MMM-yyyy HH:mm") + "##" + locationcode, ds, estr) Then '' Added by Dipen shah on 3-dec-2014
                                Throw New Exception(estr)
                            End If
                            If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                                dr_Qc("ActualTIME2") = CDate(ds.Tables(0).Rows(0).Item("ActualTIME")).ToString("dd-MMM-yyyy HH:mm") + " (" + ds.Tables(0).Rows(0).Item("TimeZoneOffSet").ToString + " GMT)"
                            End If
                        End If



                        'If Not ds Is Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                        '    dr_Qc("ActualTIME") = CDate(ds.Tables(0).Rows(0).Item("ActualTIME")).ToString("dd-MMM-yyyy HH:mm") + " (" + ds.Tables(0).Rows(0).Item("TimeZoneOffSet").ToString + " GMT)"
                        '    dr_Qc("ActualTIME2") = CDate(ds.Tables(0).Rows(0).Item("ActualTIME")).ToString("dd-MMM-yyyy HH:mm") + " (" + ds.Tables(0).Rows(0).Item("TimeZoneOffSet").ToString + " GMT)"
                        'End If

                    Next
                End If
            End If


            Me.GVQCDtl.DataSource = Ds_QCGrid
            Me.GVQCDtl.DataBind()

            Me.btnQC.Visible = True

            If CType(Me.ViewState(VS_Choice), WS_Lambda.DataObjOpenSaveModeEnum) <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_View And _
                Ds_QCGrid.Tables(0).Rows.Count <= 0 Then
                Me.btnQC.Visible = False
            End If

            Return True
            UPPanelQc.Update()
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......fillQCGrid")
            Return False

        End Try

    End Function

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

            wStr += " And cStatusIndi <> 'D' AND vProofType <> '' "

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

#End Region

#Region "Checking For Duplication Of Subjects"

    Protected Sub btnCheckDuplicateSubject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCheckDuplicateSubject.Click

        Dim ds As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim SubjectId As String = String.Empty

        Try

            wStr = "vInitials = '" + Me.HFInitials.Value.Trim() + "' And dBirthDate = '" + Me.txtdBirthDate.Text.Trim() + "'"
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

        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "disableControl1223", "enableControl();", True)

    End Sub

#End Region

#Region "GVSubjectProof Events"

    Protected Sub GVSubjectProof_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVSubjectProof.RowCreated

        e.Row.Cells(GVCSubProof_nSubjectProofNo).Visible = False
        e.Row.Cells(GVCSubProof_vSubjectId).Visible = False
        e.Row.Cells(GVCSubProof_iTranNo).Visible = False
        e.Row.Cells(GVCSubProof_vProofPath).Visible = False
        e.Row.Cells(GVCSubProof_iModifyBy).Visible = False
        e.Row.Cells(GVCSubProof_cStatusIndi).Visible = False
    End Sub

    Protected Sub GVSubjectProof_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVSubjectProof.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            CType(e.Row.FindControl("hlnkFile"), System.Web.UI.WebControls.HyperLink).NavigateUrl = CType(e.Row.FindControl("hlnkFile"), System.Web.UI.WebControls.HyperLink).Text.Trim()
            CType(e.Row.FindControl("hlnkFile"), System.Web.UI.WebControls.HyperLink).Text = Path.GetFileName(CType(e.Row.FindControl("hlnkFile"), System.Web.UI.WebControls.HyperLink).Text.Trim())

            CType(e.Row.FindControl("imgbtnDelete"), ImageButton).CommandArgument = e.Row.RowIndex
            CType(e.Row.FindControl("imgbtnDelete"), ImageButton).CommandName = "DELETE"


        End If
    End Sub

    Protected Sub GVSubjectProof_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GVSubjectProof.RowCommand
        Dim index As Integer = e.CommandArgument
        Dim ds As DataSet = Nothing
        Dim ds_delete As New DataSet
        Dim dr As DataRow
        Dim Retu_TranNo As String = String.Empty
        Dim eStr As String = String.Empty

        Try


            If e.CommandName.ToUpper = "DELETE" Then

                ds = objHelp.GetResultSet("SELECT * FROM SubjectProofDetails WHERE 1=2", "SubjectProofDetails")
                Me.HFDelete.Value = "Y"
                If GVSubjectProof.Rows.Count > 0 Then
                    For i As Integer = 0 To GVSubjectProof.Rows.Count - 1

                        dr = ds.Tables(0).NewRow()
                        dr("nSubjectProofNo") = Me.GVSubjectProof.Rows(i).Cells(GVCSubProof_nSubjectProofNo).Text
                        dr("vSubjectId") = Me.GVSubjectProof.Rows(i).Cells(GVCSubProof_vSubjectId).Text
                        dr("iTranNo") = Me.GVSubjectProof.Rows(i).Cells(GVCSubProof_iTranNo).Text
                        dr("vProofType") = Me.GVSubjectProof.Rows(i).Cells(GVCSubProof_vProofType).Text
                        dr("vProofPath") = Me.GVSubjectProof.Rows(i).Cells(GVCSubProof_vProofPath).Text
                        dr("iModifyBy") = Me.GVSubjectProof.Rows(i).Cells(GVCSubProof_iModifyBy).Text
                        dr("cStatusIndi") = Me.GVSubjectProof.Rows(i).Cells(GVCSubProof_cStatusIndi).Text
                        ds.Tables(0).Rows.Add(dr)
                        ds.Tables(0).AcceptChanges()
                    Next

                End If

                Me.HDSubjectProofDetails.Value = ds.Tables(0).Rows(index)("nSubjectProofNo").ToString()
                Me.GVSubjectProof.DeleteRow(index)
                'Me.GVSubjectProof.DataBind()
                If (ViewState("Edit") = "Y") Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Temp", "fnEditAttachment('ctl00_CPHLAMBDA_Image36');", True)
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "Temp", "enableControl();", True)
                End If

                If (ViewState("Edit") <> "Y") Then
                    Dim dtSubjectProof As DataTable = CType(Me.ViewState(VS_GVDtSubjectProofDetails), DataTable)
                    Dim indexRow As Integer = -1
                    For Each dr In dtSubjectProof.Rows
                        indexRow = indexRow + 1
                        If (indexRow = index) Then
                            dtSubjectProof.Rows.Remove(dr)
                            dtSubjectProof.AcceptChanges()
                            Exit For
                        End If
                    Next
                    dtSubjectProof.AcceptChanges()
                    GVSubjectProof.DataSource = Nothing
                    GVSubjectProof.DataSource = dtSubjectProof
                    GVSubjectProof.DataBind()
                End If
                Me.HFDelete.Value = "Y"
                Me.HFDelete1.Value = "Y"
            End If
            If HFMode.Value = 4 Then
                Me.Image36.Style.Add("display", "none")
                Me.Image38.Style.Add("display", "none")

            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".......GVSubjectProof_RowCommand")
        Finally
            txtAge.Text = Me.HFAge.Value
            txtbmi.Text = Me.HfBMI.Value
            txtInitials.Text = Me.HFInitials.Value
            '' ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "validate", "validateHabit();", True)
            Try
                If HFMode.Value = 4 Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "validateforMode4", "validateHabitForViewMode();", True)
                End If
            Catch ex As Exception

            End Try

        End Try

    End Sub

    Protected Sub GVSubjectProof_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GVSubjectProof.RowDeleting

    End Sub

    Private Function BindGridSubjectProof(ByVal dt As DataTable) As Boolean
        Dim dv As New DataView
        Dim index As Integer = 0

        dv = dt.DefaultView
        dv.RowFilter = "cStatusIndi <> 'D' AND vProofType <> '' "
        dt = CType(dv, Data.DataView).ToTable()
        Me.GVSubjectProof.DataSource = Nothing
        Me.GVSubjectProof.DataBind()
        Me.GVSubjectProof.DataSource = dt.DefaultView
        Me.GVSubjectProof.DataBind()

        If (ViewState(VS_IsEdit) <> "Y") Then
            Me.Image36.Style.Add("display", "none")
            Me.Image37.Style.Add("display", "none")
        End If


        Return True
    End Function

#End Region

#Region "Assignvalues"

    Private Function AssignValues(ByRef ds_SubjectMst As DataSet) As Boolean
        Dim ds_SubjectMst_Data As DataSet = Nothing
        Dim dsSubjectHabitDetail As DataSet = Nothing
        Dim dr As DataRow = Nothing
        Dim StrLanguagecode As String = String.Empty
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
        Try
            ' ds_SubjectMst = Nothing


            If Not objHelp.GetView_SubjectMaster("1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubjectMst_Data, estr) Then
                Throw New Exception(estr)
            End If

            If Not objHelp.GetSubjectHabitDetails("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, _
                           dsSubjectHabitDetail, estr) Then
                Throw New Exception(estr)

            End If



            If Not (ViewState("Edit") = "Y") Then

                dr = ds_SubjectMst_Data.Tables(0).NewRow()

                dr("vLocationCode") = Me.Session(S_LocationCode)
                dr("vSubjectID") = Pro_Screening
                dr("nSubjectDetailNo") = 1
                dr("dEnrollmentDate") = IIf(String.IsNullOrEmpty(Me.txtdEnrollmentDate.Text.Trim), System.DBNull.Value, Me.txtdEnrollmentDate.Text.Trim)
                dr("vFirstName") = Me.txtvFirstName.Text.Trim
                dr("vSurName") = Me.txtvSurName.Text.Trim
                dr("vMiddleName") = Me.txtvMiddleName.Text.Trim
                dr("vInitials") = Me.HFInitials.Value.Trim
                dr("dBirthDate") = IIf(String.IsNullOrEmpty(Me.txtdBirthDate.Text.Trim), System.DBNull.Value, Me.txtdBirthDate.Text.Trim)
                ' For inhouse mode :Start
                dr("cSubjectType") = Subject_Type
                ' For inhouse mode :End
                'dr("vICFLanguageCodeId") = Me.ddlicflanguage.SelectedValue.Trim

                'done on 20-Dec-2010
                For Each lstItem As ListItem In chkvICFLanguageCodeId.Items
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
                dr("cSex") = ddlcSex.SelectedItem.Text.ToString()



                dr("vProofOfAge1") = System.DBNull.Value
                dr("vProofOfAge2") = System.DBNull.Value
                dr("vProofOfAge3") = System.DBNull.Value


                dr("cBloodGroup") = Me.ddlcBloodGroup.SelectedValue.Trim
                dr("cRh") = Me.ddlcRh.SelectedValue.Trim
                dr("vEducationQualification") = Me.txtvEducationQualification.Text.Trim
                dr("vOccupation") = Me.txtvOccupation.Text.Trim
                dr("vRefSubjectId") = Me.txtvRefSubjectId.Text.Trim()
                dr("iModifyBy") = Me.Session(S_UserID)
                'subjectdetailtabledr("nHeight") = Me.txtnHeight.Text.Trim
                dr("nWeight") = Me.txtnWeight.Text.Trim
                dr("nHeight") = Me.txtnHeight.Text.Trim
                dr("nBMI") = Me.HfBMI.Value.Trim
                dr("cMaritalStatus") = ddlcMaritalStatus.SelectedItem.Text.ToString()
                dr("cFoodHabit") = ddlcFoodHabit.SelectedItem.Text.ToString()
                dr("vDemographicRemarks") = txtvDemographicRemarks.Text.ToString()
                'subjectcontactdetail

                dr("nSubjectContactNo") = 1
                StrAddress1 = Me.txtvLocalAdd1.Text.Trim
                Address(StrAddress1, StrAdd11, StrAdd12, StrAdd13)
                dr("vLocalAdd1") = Convert.ToString(StrAdd11)
                dr("vLocalAdd12") = Convert.ToString(StrAdd12)
                dr("vLocalAdd13") = Convert.ToString(StrAdd13)

                StrAddress2 = Me.txtvLocalAdd21.Text.Trim
                Address(StrAddress2, StrAdd21, StrAdd22, StrAdd23)
                dr("vLocalAdd21") = Convert.ToString(StrAdd21)
                dr("vLocalAdd22") = Convert.ToString(StrAdd22)
                dr("vLocalAdd23") = Convert.ToString(StrAdd23)

                'This For Local Permanent Address
                StrPerAdd = Me.txtvPerAdd1.Text.Trim
                Address(StrPerAdd, StrAdd1, StrAdd2, StrAdd3)
                dr("vPerAdd1") = Convert.ToString(StrAdd1)
                dr("vPerAdd2") = Convert.ToString(StrAdd2)
                dr("vPerAdd3") = Convert.ToString(StrAdd3)

                'Added on 28-Jul-2009
                'dr("vPerCity") = Me.txtvPerCity.Text.Trim()
                dr("vPerCity") = Convert.ToString(Me.ddlvPerCity.SelectedItem.Text)

                '****************************************

                dr("vLocalTelephoneno1") = Me.txtvLocalTelephoneno1.Text.Trim
                dr("vLocalTelephoneno2") = Me.txtvLocalTelephoneno2.Text.Trim
                dr("vPerTelephoneno") = Me.txtvPerTelephoneno.Text.Trim
                dr("vOfficeTelephoneno") = Me.txtvOfficeTelephoneno.Text.Trim
                dr("vContactName1") = Me.txtvContactName1.Text.Trim
                dr("vContactName2") = Me.txtvContactName2.Text.Trim()
                dr("vContactAddress11") = Me.txtvContactAddress11.Text.Trim
                dr("vContactAddress21") = Me.txtvContactAddress21.Text.Trim
                dr("vContactTelephoneno1") = Me.txtvContactTelephoneno1.Text.Trim
                dr("vContactTelephoneno2") = Me.txtvContactTelephoneno2.Text.Trim
                dr("vReferredBy") = Me.txtvReferredBy.Text.Trim
                dr("vRemarks") = Me.txtvRemarks.Text.Trim
                dr("vOfficeAddress") = Me.txtvOfficeAddress.Text.Trim
                'To save NUll image on 26-Nov-2008 by Naimesh Dave
                dr("vbPhotograph") = System.DBNull.Value
                dr("vPopulation") = Convert.ToString(Me.ddlvPopulation.SelectedItem.Text)
                '************************



                'This is for SubjectFemaleDetails

                If ddlcSex.SelectedValue = "F" Then

                    dr("dLastMenstrualDate") = IIf(String.IsNullOrEmpty(Me.txtdLastMenstrualDate.Text.Trim), System.DBNull.Value, Me.txtdLastMenstrualDate.Text.Trim)
                    dr("dLastDelivaryDate") = IIf(String.IsNullOrEmpty(Me.txtdLastDelivaryDate.Text.Trim), System.DBNull.Value, Me.txtdLastDelivaryDate.Text.Trim)
                    dr("cRegular") = Me.rblcRegular.SelectedValue
                    dr("iLastMenstrualDays") = IIf(String.IsNullOrEmpty(Me.txtiLastMenstrualDays.Text), System.DBNull.Value, Me.txtiLastMenstrualDays.Text)
                    dr("vGravida") = Me.txtvGravida.Text
                    dr("iNoOfChildren") = IIf(String.IsNullOrEmpty(Me.txtiNoOfChildren.Text), System.DBNull.Value, Me.txtiNoOfChildren.Text)
                    'Me.txtiNoOfChildren.Text.ToString()
                    'IIf(String.IsNullOrEmpty(Me.txtiNoOfChildren.Text), System.DBNull.Value, Me.txtiNoOfChildren.Text)

                    If Me.rblcAbortions.SelectedIndex > -1 Then
                        dr("cAbortions") = Me.rblcAbortions.SelectedValue.Trim()
                    End If

                    dr("dAbortionDate") = IIf(String.IsNullOrEmpty(Me.txtdAbortionDate.Text.Trim), System.DBNull.Value, Me.txtdAbortionDate.Text.Trim)
                    dr("vPara") = Me.txtvPara.Text
                    dr("cLoctating") = Me.rblcLoctating.SelectedValue.ToUpper.Trim() 'Me.txtLactating.Text

                    If rblcLastMenstrualIndi.SelectedIndex > -1 Then
                        dr("cLastMenstrualIndi") = Me.rblcLastMenstrualIndi.SelectedValue.Trim()
                    End If

                    If rblcIsVolunteerinBearingAge.SelectedIndex > -1 Then
                        dr("cIsVolunteerinBearingAge") = Me.rblcIsVolunteerinBearingAge.SelectedValue.Trim()
                    End If

                    If rblcChildrenHelath.SelectedIndex > -1 Then
                        dr("cChildrenHelath") = rblcChildrenHelath.SelectedItem.Value.Trim()
                    End If

                    dr("vchildrenHealthRemark") = Me.txtvchildrenHealthRemark.Text.Trim()
                    dr("iNoOfChildrenDied") = IIf(Me.txtiNoOfChildrenDied.Text.Trim() = "", System.DBNull.Value, Me.txtiNoOfChildrenDied.Text.Trim())
                    'txtiNoOfChildrenDied.Text.ToString()
                    'IIf(Me.txtiNoOfChildrenDied.Text.Trim() = "", System.DBNull.Value, Me.txtiNoOfChildrenDied.Text.Trim())
                    dr("vRemarkifDied") = Me.txtvRemarkifDied.Text.Trim()


                    If rblcContraception.SelectedIndex > -1 Then
                        dr("cContraception") = rblcContraception.SelectedValue.Trim()
                    End If

                    For index = 0 To chkcContraception.Items.Count - 1
                        If chkcContraception.Items(index).Selected = True Then
                            If chkcContraception.Items(index).Value.ToUpper.Trim() = "B" Then
                                dr("cBarrier") = "C"
                            ElseIf chkcContraception.Items(index).Value.ToUpper.Trim() = "P" Then
                                dr("cPills") = "C"
                            ElseIf chkcContraception.Items(index).Value.ToUpper.Trim() = "R" Then
                                dr("cRhythm") = "C"
                            ElseIf chkcContraception.Items(index).Value.ToUpper.Trim() = "I" Then
                                dr("cIUCD") = "C"
                            End If
                        End If
                    Next index

                End If
                dr("vOtherRemark") = Me.txtvOtherRemark.Text.Trim()
                ds_SubjectMst_Data.Tables(0).Rows.Add(dr)
                ds_SubjectMst_Data.Tables(0).AcceptChanges()
                ds_SubjectMst_Data.Tables(0).TableName = "View_SubjectMaster"
                ds_SubjectMst.Tables.Add(ds_SubjectMst_Data.Tables(0).Copy())
                Me.ViewState(VS_DtSubjectMst) = ds_SubjectMst_Data.Tables(0)
            End If
            'ds_SubjectMst_Data.Tables(0).Rows.Add(dr)
            'ds_SubjectMst_Data.Tables(0).Rows.Add(dr)
            'ds_SubjectMst_Data.Tables(0).AcceptChanges()
            'ds_SubjectMst_Data.Tables(0).TableName = "View_SubjectMaster"
            'ds_SubjectMst.Tables.Add(ds_SubjectMst_Data.Tables(0).Copy())
            'Me.ViewState(VS_DtSubjectMst) = ds_SubjectMst_Data.Tables(0)

            If (ViewState("Edit") = "Y") Then
                ds_SubjectMst_Data = Nothing
                Wstr = "vSubjectId='" & Me.HSubjectId.Value.Trim() & "'"

                If Not objHelp.GetView_SubjectMaster(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_SubjectMst_Data, estr) Then
                    Throw New Exception(estr)
                End If


            End If


            If (ViewState("Edit") = "Y") Then
                Dim cph As New ContentPlaceHolder
                cph = Me.Master.FindControl("CPHLAMBDA")

                dr = ds_SubjectMst_Data.Tables(0).Rows(0)

                dr("vSubjectId") = Request.QueryString("SearchSubjectId")
                dr("iModifyBy") = Me.Session(S_UserID)
                ' dr = ds_SubjectMst_Data.Tables(0).NewRow()

                Dim ColumnName(ds_SubjectMst_Data.Tables(0).Columns.Count - 1) As String
                Dim ColumnValue(ds_SubjectMst_Data.Tables(0).Columns.Count - 1) As String
                For I As Integer = 0 To ds_SubjectMst_Data.Tables(0).Columns.Count
                    For Each dc As DataColumn In ds_SubjectMst_Data.Tables(0).Columns
                        ColumnName(I) = dc.ColumnName

                        If (txtdLastDelivaryDate.Text = "") Then
                            dr("dLastDelivaryDate") = IIf(String.IsNullOrEmpty(Me.txtdLastDelivaryDate.Text.Trim), System.DBNull.Value, Me.txtdLastDelivaryDate.Text.Trim)
                        End If

                        If (txtdLastMenstrualDate.Text = "") Then
                            dr("dLastMenstrualDate") = IIf(String.IsNullOrEmpty(Me.txtdLastMenstrualDate.Text.Trim), System.DBNull.Value, Me.txtdLastMenstrualDate.Text.Trim)
                        End If

                        If (txtdAbortionDate.Text = "") Then
                            dr("dAbortionDate") = IIf(String.IsNullOrEmpty(Me.txtdAbortionDate.Text.Trim), System.DBNull.Value, Me.txtdAbortionDate.Text.Trim)
                        End If

                        Try
                            Dim img As System.Web.UI.WebControls.Image = CType(cph.FindControl("imgEdit" + (dc.ColumnName)), System.Web.UI.WebControls.Image)

                            If img.Style.Value.Trim().Contains("display:none") Then
                                'If (dc.ColumnName = "vContactAddress11") Then
                                '    Dim txt As TextBox = CType(cph.FindControl("txt" + (dc.ColumnName)), TextArea)
                                'Else
                                Dim txt As TextBox
                                Dim rbl As RadioButtonList
                                Dim chkbl As CheckBoxList
                                Dim ddl As DropDownList
                                If (ColumnName(I) = "vICFLanguageCodeId") Then

                                    For Each lstItem As ListItem In chkvICFLanguageCodeId.Items
                                        If lstItem.Selected = True Then

                                            If StrLanguagecode <> "" Then
                                                StrLanguagecode = StrLanguagecode + "," + lstItem.Value
                                            Else
                                                StrLanguagecode = lstItem.Value
                                            End If
                                        End If
                                    Next
                                    If (StrLanguagecode <> "") Then
                                        dr("vColumnName") += ColumnName(I) + ","
                                        dr("" + ColumnName(I) + "") = StrLanguagecode
                                    End If


                                    'If Not (String.IsNullOrEmpty(dr("vICFLanguageCodeId"))) Then
                                    '    dr("vColumnName") += ColumnName(I) + ","
                                    '    dr("" + ColumnName(I) + "") = ColumnValue(I)
                                    '    Exit Try
                                    'End If

                                    '' chkvICFLanguageCodeId.SelectedValue
                                End If
                                If (ColumnName(I) = "cFoodHabit") Then
                                    ddl = CType(cph.FindControl("ddl" + (dc.ColumnName)), DropDownList)
                                    If (ddl.SelectedValue <> String.Empty) Then
                                        If (ddl.SelectedValue <> "") Then
                                            dr("vColumnName") += ColumnName(I) + ","
                                            dr("" + ColumnName(I) + "") = ddl.SelectedValue
                                            Exit Try
                                        End If
                                    End If
                                End If

                                If (ColumnName(I) = "cBloodGroup") Then
                                    ddl = CType(cph.FindControl("ddl" + (dc.ColumnName)), DropDownList)
                                    If (ddl.SelectedValue <> String.Empty) Then
                                        dr("vColumnName") += ColumnName(I) + "," + "cRh,"
                                        dr("" + ColumnName(I) + "") = ddl.SelectedValue
                                        dr("cRh") = ddlcRh.SelectedValue
                                    End If
                                End If
                                If (ColumnName(I) = "vPopulation") Then
                                    ddl = CType(cph.FindControl("ddl" + (dc.ColumnName)), DropDownList)
                                    If (ddl.SelectedValue <> String.Empty) Then
                                        dr("vColumnName") += ColumnName(I) + ","
                                        dr("" + ColumnName(I) + "") = Convert.ToString(ddl.SelectedItem.Text)
                                    End If
                                End If
                                If (ColumnName(I) = "vPerCity") Then
                                    ddl = CType(cph.FindControl("ddl" + (dc.ColumnName)), DropDownList)
                                    If (ddl.SelectedValue <> String.Empty) Then
                                        dr("vColumnName") += ColumnName(I) + ","
                                        dr("" + ColumnName(I) + "") = Convert.ToString(ddl.SelectedItem.Text)
                                    End If
                                End If


                                If (ColumnName(I) = "cBarrier" Or ColumnName(I) = "cPills" Or ColumnName(I) = "cRhythm" Or ColumnName(I) = "cIUCD") Then
                                    chkbl = chkcContraception
                                    If (chkbl.SelectedValue <> "") Then
                                        dr("vColumnName") += ColumnName(I) + ","
                                        For index = 0 To chkcContraception.Items.Count - 1
                                            If chkcContraception.Items(index).Selected = True Then
                                                If chkcContraception.Items(index).Value.ToUpper.Trim() = "B" Then
                                                    dr("cBarrier") = "C"
                                                ElseIf chkcContraception.Items(index).Value.ToUpper.Trim() = "P" Then
                                                    dr("cPills") = "C"
                                                ElseIf chkcContraception.Items(index).Value.ToUpper.Trim() = "R" Then
                                                    dr("cRhythm") = "C"
                                                ElseIf chkcContraception.Items(index).Value.ToUpper.Trim() = "I" Then
                                                    dr("cIUCD") = "C"
                                                End If
                                            End If
                                        Next index

                                        ColumnValue(I) = chkbl.SelectedValue
                                        ' dr("" + ColumnName(I) + "") = chkbl.SelectedValue
                                    End If
                                    Exit Try
                                End If
                                If (ColumnName(I) = "cRegular" Or ColumnName(I) = "cLastMenstrualIndi" Or ColumnName(I) = "cAbortions" Or ColumnName(I) = "cChildrenHelath" Or ColumnName(I) = "cLoctating" Or ColumnName(I) = "cIsVolunteerinBearingAge" Or ColumnName(I) = "cContraception") Then
                                    rbl = CType(cph.FindControl("rbl" + (dc.ColumnName)), RadioButtonList)
                                    If (rbl.SelectedValue <> "") Then
                                        dr("vColumnName") += ColumnName(I) + ","
                                        ColumnValue(I) = rbl.SelectedValue
                                        dr("" + ColumnName(I) + "") = rbl.SelectedValue
                                    End If
                                    Exit Try
                                Else

                                    txt = CType(cph.FindControl("txt" + (dc.ColumnName)), TextBox)
                                    If (txt.Text <> "") Then
                                        dr("vColumnName") += ColumnName(I) + ","
                                        ColumnValue(I) = txt.Text.Trim()
                                        dr("" + ColumnName(I) + "") = ColumnValue(I)
                                        dr("" + ColumnName(I) + "") = txt.Text
                                        If (ColumnName(I) = "vMiddleName") Then
                                            dr("vInitials") = Me.txtInitials.Text
                                            dr("vInitials") = Me.HFInitials.Value
                                        End If
                                    End If
                                End If
                                'If (ColumnName(I) = "cRegular" Or ColumnName(I) = "cLastMenstrualIndi" Or ColumnName(I) = "cAbortions" Or ColumnName(I) = "cChildrenHelath" Or ColumnName(I) = "cLoctating" Or ColumnName(I) = "cIsVolunteerinBearingAge" Or ColumnName(I) = "cContraception") Then
                                '    If (rbl.SelectedValue <> String.Empty) Then
                                '        dr("vColumnName") += ColumnName(I) + ","
                                '        rbl.SelectedValue = ColumnValue(I)
                                '    End If
                                'End If

                                If (ColumnName(I) = "cBarrier" Or ColumnName(I) = "cPills" Or ColumnName(I) = "cRhythm" Or ColumnName(I) = "cIUCD") Then
                                    If (chkbl.SelectedValue <> String.Empty) Then
                                        dr("vColumnName") += ColumnName(I) + "," + "cPills,cRhythm,cIUCD"
                                        ColumnValue(I) = chkbl.SelectedValue
                                        dr("" + ColumnName(I) + "") = chkbl.SelectedValue
                                        Exit Try
                                    End If
                                End If


                                If (txt.Text.Trim() <> String.Empty) Then
                                    If (ColumnName(I) = "vLocalAdd1") Then
                                        dr("vColumnName") += ColumnName(I) + "," + ColumnName(I) + "2" + "," + ColumnName(I) + "3" + ","
                                        dr("" + ColumnName(I) + "") = ColumnValue(I)
                                        Exit Try
                                    ElseIf (ColumnName(I) = "vLocalAdd21") Then
                                        dr("vColumnName") += ColumnName(I) + "," + ColumnName(I).Substring(0, ColumnName(I).Length - 1) + "2" + "," + ColumnName(I).Substring(0, ColumnName(I).Length - 1) + "3" + ","
                                        ColumnValue(I) = txt.Text.Trim()
                                        dr("" + ColumnName(I) + "") = ColumnValue(I)
                                        Exit Try
                                    ElseIf (ColumnName(I) = "vPerAdd1") Then
                                        dr("vColumnName") += ColumnName(I) + "," + ColumnName(I).Substring(0, ColumnName(I).Length - 1) + "2" + "," + ColumnName(I).Substring(0, ColumnName(I).Length - 1) + "3" + ","
                                        dr("" + ColumnName(I) + "") = ColumnValue(I)
                                        dr("" + ColumnName(I) + "") = ColumnValue(I)
                                        Exit Try
                                    Else
                                        dr("vColumnName") += ColumnName(I) + ","
                                        ColumnValue(I) = txt.Text.Trim()
                                        dr("" + ColumnName(I) + "") = ColumnValue(I)
                                        Exit Try
                                    End If
                                End If
                            End If

                        Catch ex As Exception

                        End Try
                        I = I + 1
                    Next
                Next

                ds_SubjectMst_Data.Tables(0).AcceptChanges()
                ds_SubjectMst_Data.Tables(0).TableName = "View_SubjectMaster"
                ds_SubjectMst.Tables.Add(ds_SubjectMst_Data.Tables(0).Copy())
                Me.ViewState(VS_DtSubjectMst) = ds_SubjectMst_Data.Tables(0)
                '' Return True
            End If
            'ds_SubjectMst_Data.Tables(0).Rows.Add(dr)
            'ds_SubjectMst_Data.Tables(0).AcceptChanges()
            'ds_SubjectMst_Data.Tables(0).TableName = "View_SubjectMaster"
            'ds_SubjectMst.Tables.Add(ds_SubjectMst_Data.Tables(0).Copy())
            'Me.ViewState(VS_DtSubjectMst) = ds_SubjectMst_Data.Tables(0)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "...Error while assign values")
            Return False
        End Try

        ' For Grid view Habits
        If (Image38.Style.Value.Trim().Contains("display:none") AndAlso ViewState("Edit") = "Y") Then
            Dim dr1 As DataRow = Nothing
            Dim IsModifyHabit As String = String.Empty
            For index = 0 To Me.GVHabits.Rows.Count - 1
                dr1 = dsSubjectHabitDetail.Tables(0).NewRow()
                If Not (String.IsNullOrEmpty(CType(Me.GVHabits.Rows(index).FindControl("txtEndDate"), TextBox).Text.Trim()) AndAlso String.IsNullOrEmpty(CType(Me.GVHabits.Rows(index).FindControl("txtConsumption"), TextBox).Text.Trim())) Then
                    IsModifyHabit = "Y"
                    ViewState("IsModifyHabit") = "Y"
                End If
            Next
            If (IsModifyHabit = "Y") Then
                For index = 0 To Me.GVHabits.Rows.Count - 1
                    dr1 = dsSubjectHabitDetail.Tables(0).NewRow()
                    dr1("vSubjectId") = Me.HSubjectId.Value
                    dr1("nSubjectHabitDetailsNo") = IIf(Me.GVHabits.Rows(index).Cells(GVHC_SubjectHabitDetailsNo).Text.Trim = "" Or GVHabits.Rows(index).Cells(GVHC_SubjectHabitDetailsNo).Text.Trim = "&nbsp;", 0, Me.GVHabits.Rows(index).Cells(GVHC_SubjectHabitDetailsNo).Text.Trim)
                    dr1("vHabitDetails") = Me.GVHabits.Rows(index).Cells(GVHC_Habits).Text.Trim
                    dr1("vHabitId") = Me.GVHabits.Rows(index).Cells(GVHC_HabitId).Text.Trim
                    dr1("cHabitFlag") = CType(Me.GVHabits.Rows(index).FindControl("ddlHebitType"), DropDownList).SelectedValue.Trim
                    dr1("vHabitHistory") = IIf(CType(Me.GVHabits.Rows(index).FindControl("txtEndDate"), TextBox).Text.Trim = "", _
                                    System.DBNull.Value, CType(Me.GVHabits.Rows(index).FindControl("txtEndDate"), TextBox).Text.Trim)
                    dr1("vRemarks") = CType(Me.GVHabits.Rows(index).FindControl("txtConsumption"), TextBox).Text.Trim
                    dr1("iModifyBy") = Session(S_UserID)
                    dr1("vColumnName") = "---"
                    dsSubjectHabitDetail.Tables(0).Rows.Add(dr1)
                    dsSubjectHabitDetail.Tables(0).AcceptChanges()
                    ViewState(VS_DtSubjectHabitDetails) = dsSubjectHabitDetail.Tables(0)

                Next
            End If
        Else
            If (ViewState("Edit") <> "Y") Then
                Dim dr1 As DataRow = Nothing
                For index = 0 To Me.GVHabits.Rows.Count - 1
                    dr1 = dsSubjectHabitDetail.Tables(0).NewRow()

                    dr1("nSubjectHabitDetailsNo") = IIf(Me.GVHabits.Rows(index).Cells(GVHC_SubjectHabitDetailsNo).Text.Trim = "" Or GVHabits.Rows(index).Cells(GVHC_SubjectHabitDetailsNo).Text.Trim = "&nbsp;", 0, Me.GVHabits.Rows(index).Cells(GVHC_SubjectHabitDetailsNo).Text.Trim)
                    dr1("vHabitDetails") = Me.GVHabits.Rows(index).Cells(GVHC_Habits).Text.Trim
                    dr1("vHabitId") = Me.GVHabits.Rows(index).Cells(GVHC_HabitId).Text.Trim
                    dr1("cHabitFlag") = CType(Me.GVHabits.Rows(index).FindControl("ddlHebitType"), DropDownList).SelectedValue.Trim
                    dr1("vHabitHistory") = IIf(CType(Me.GVHabits.Rows(index).FindControl("txtEndDate"), TextBox).Text.Trim = "", _
                                    System.DBNull.Value, CType(Me.GVHabits.Rows(index).FindControl("txtEndDate"), TextBox).Text.Trim)
                    dr1("vRemarks") = CType(Me.GVHabits.Rows(index).FindControl("txtConsumption"), TextBox).Text.Trim
                    dr1("iModifyBy") = Session(S_UserID)
                    dsSubjectHabitDetail.Tables(0).Rows.Add(dr1)
                    dsSubjectHabitDetail.Tables(0).AcceptChanges()
                    ViewState(VS_DtSubjectHabitDetails) = dsSubjectHabitDetail.Tables(0)
                Next
            End If
        End If

        '  dsSubjectHabitDetail.Tables(0).Columns.Add("vUpdateRemarks", Type.GetType("System.String"))
        'dsSubjectHabitDetail.AcceptChanges()
        'dsSubjectHabitDetail.Tables(0).TableName = "SubjectHabitDetails"
        'ds_SubjectMst.Tables.Add(dsSubjectHabitDetail.Tables(0).Copy())
        'ds_SubjectMst.AcceptChanges()
        'ViewState(VS_DtSubjectHabitDetails) = dsSubjectHabitDetail.Tables(0)
        '' Return True
        Return True

    End Function

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

    Private Function setPageData(ByVal ds As DataSet) As Boolean
        Try
            Dim strLocalAdd1 As String = String.Empty
            Dim strLocalAdd2 As String = String.Empty
            Dim strPerAdd As String = String.Empty


            Dim cph As New ContentPlaceHolder
            cph = Me.Master.FindControl("CPHLAMBDA")

            Dim Len As Integer = 5
            Dim StrICFlang As String()
            Dim ColumnName(ds.Tables(0).Columns.Count - 1) As String
            Dim ColumnValue(ds.Tables(0).Columns.Count - 1) As String

            For I As Integer = 0 To ds.Tables(0).Columns.Count
                For Each dc As DataColumn In ds.Tables(0).Columns
                    ColumnName(I) = dc.ColumnName
                    ColumnValue(I) = ds.Tables(0).Rows(0)(I).ToString
                    I = I + 1
                Next
            Next

            Try
                Language = ColumnValue(23)
            Catch ex As Exception
                Language = String.Empty
            End Try



            For I As Integer = 0 To ColumnName.Length - 1
                Dim Id As New StringBuilder("txt")
                Try
                    If (ColumnValue(I).ToString.Trim() <> String.Empty) Then
                        If (ColumnName(I) = "cSex") Then
                            CType(cph.FindControl("ddl" + (ColumnName(I))), DropDownList).SelectedValue = ColumnValue(I).ToString.Trim()
                            CType(cph.FindControl("ddl" + (ColumnName(I))), DropDownList).Enabled = False
                        ElseIf (ColumnName(I) = "cBloodGroup") Then
                            CType(cph.FindControl("ddl" + (ColumnName(I))), DropDownList).SelectedValue = ColumnValue(I).ToString.Trim()
                            CType(cph.FindControl("ddl" + (ColumnName(I))), DropDownList).Enabled = False
                            hdncBloodGroup.Value = ColumnValue(I).ToString.Trim()
                            CType(cph.FindControl("ddl" + (ColumnName(I + 1))), DropDownList).SelectedValue = ColumnValue(I + 1).ToString.Trim()
                            CType(cph.FindControl("ddl" + (ColumnName(I + 1))), DropDownList).Enabled = False
                            hdncRh.Value = ColumnValue(I + 1).ToString.Trim()
                        ElseIf (ColumnName(I) = "vPopulation") Then
                            FillPopulation()
                            CType(cph.FindControl("ddl" + (ColumnName(I))), DropDownList).SelectedItem.Text = ColumnValue(I).ToString.Trim()
                            CType(cph.FindControl("ddl" + (ColumnName(I))), DropDownList).Enabled = False
                        ElseIf (ColumnName(I) = "vPerCity") Then
                            FillLocation()
                            Try
                                CType(cph.FindControl("ddl" + (ColumnName(I))), DropDownList).SelectedItem.Text = ColumnValue(I).ToString.Trim()
                                CType(cph.FindControl("ddl" + (ColumnName(I))), DropDownList).Enabled = False
                            Catch ex As Exception

                            End Try


                        ElseIf (ColumnName(I) = "cMaritalStatus" Or ColumnName(I) = "cFoodHabit") Then
                            CType(cph.FindControl("ddl" + (ColumnName(I))), DropDownList).SelectedValue = ColumnValue(I).ToString.Trim()
                            CType(cph.FindControl("ddl" + (ColumnName(I))), DropDownList).Enabled = False
                        ElseIf (ColumnName(I) = "vPerAdd1") Then
                            If (ColumnValue(I) + ColumnValue(I + 1) + ColumnValue(I + 2) <> String.Empty) Then
                                'CType(cph.FindControl("txt" + (ColumnName(I))), TextBox).Text = ColumnValue(I).ToString.Trim()
                                CType(cph.FindControl("txt" + (ColumnName(I))), TextBox).Text = ColumnValue(I).ToString.Trim()
                                If Not (String.IsNullOrEmpty(ColumnValue(I + 1))) Then
                                    CType(cph.FindControl("txt" + (ColumnName(I))), TextBox).Text += "," + ColumnValue(I + 1).ToString.Trim()
                                End If
                                If Not (String.IsNullOrEmpty(ColumnValue(I + 2))) Then
                                    CType(cph.FindControl("txt" + (ColumnName(I))), TextBox).Text += "," + ColumnValue(I + 2).ToString.Trim()
                                End If
                                CType(cph.FindControl("txt" + (ColumnName(I))), TextBox).Enabled = False
                            End If

                        ElseIf (ColumnName(I) = "ICFLanguage") Then
                            'CType(cph.FindControl("chk" + (ColumnName(I))), CheckBoxList) = ColumnValue(I).ToString.Trim() + "," + ColumnValue(I + 1).ToString.Trim() + "," + ColumnValue(I + 2).ToString.Trim()
                            If Not (String.IsNullOrEmpty(ColumnValue(I - 1))) Then
                                StrICFlang = ColumnValue(I - 1).ToString.Split(",")
                                For indexStr As Integer = 0 To StrICFlang.Length - 1
                                    For Each lstItem As ListItem In chkvICFLanguageCodeId.Items
                                        If lstItem.Value = StrICFlang(indexStr).ToString.Trim Then
                                            lstItem.Selected = True
                                        End If
                                        lstItem.Enabled = False
                                    Next
                                Next indexStr
                                If Not (String.IsNullOrEmpty(ColumnValue(I - 1))) Then
                                    If (Request.QueryString("mode") <> 4) Then
                                        CType(cph.FindControl("imgEdit" + (ColumnName(I - 1))), System.Web.UI.WebControls.Image).Style.Add("display", "inline")
                                    End If
                                    CType(cph.FindControl("imgAudit" + (ColumnName(I - 1))), System.Web.UI.WebControls.Image).Style.Add("display", "inline")
                                Else
                                    CType(cph.FindControl("imgEdit" + (ColumnName(I - 1))), System.Web.UI.WebControls.Image).Style.Add("display", "none")
                                    CType(cph.FindControl("imgAudit" + (ColumnName(I - 1))), System.Web.UI.WebControls.Image).Style.Add("display", "none")
                                End If
                            End If

                        ElseIf (ColumnName(I) = "vLocalAdd1") Then
                            If (ColumnValue(I) + ColumnValue(I + 1) + ColumnValue(I + 2) <> String.Empty) Then
                                CType(cph.FindControl("txt" + (ColumnName(I))), TextBox).Text = ColumnValue(I).ToString.Trim()
                                If Not (String.IsNullOrEmpty(ColumnValue(I + 1))) Then
                                    CType(cph.FindControl("txt" + (ColumnName(I))), TextBox).Text += "," + ColumnValue(I + 1).ToString.Trim()
                                End If
                                If Not (String.IsNullOrEmpty(ColumnValue(I + 2))) Then
                                    CType(cph.FindControl("txt" + (ColumnName(I))), TextBox).Text += "," + ColumnValue(I + 2).ToString.Trim()
                                End If
                                CType(cph.FindControl("txt" + (ColumnName(I))), TextBox).Enabled = False
                            End If

                        ElseIf (ColumnName(I) = "vLocalAdd21") Then
                            CType(cph.FindControl("txt" + (ColumnName(I))), TextBox).Text = ColumnValue(I).ToString.Trim()
                            If Not (String.IsNullOrEmpty(ColumnValue(I + 1))) Then
                                CType(cph.FindControl("txt" + (ColumnName(I))), TextBox).Text += "," + ColumnValue(I + 1).ToString.Trim()
                            End If
                            If Not (String.IsNullOrEmpty(ColumnValue(I + 2))) Then
                                CType(cph.FindControl("txt" + (ColumnName(I))), TextBox).Text += "," + ColumnValue(I + 2).ToString.Trim()
                            End If
                            CType(cph.FindControl("txt" + (ColumnName(I))), TextBox).Enabled = False
                        ElseIf (ColumnName(I) = "vContactAddress11") Then
                            CType(cph.FindControl("txt" + (ColumnName(I))), TextBox).Text = ColumnValue(I).ToString.Trim()
                            CType(cph.FindControl("txt" + (ColumnName(I))), TextBox).Enabled = False
                        ElseIf (ColumnName(I) = "nBMI") Then
                            ' CType(cph.FindControl("txt" + (ColumnName(I))), HtmlTextArea).Value = ColumnValue(I).ToString.Trim()
                            Me.txtbmi.Text = ColumnValue(I).ToString.Trim()
                            Me.HfBMI.Value = ColumnValue(I).ToString.Trim()
                        ElseIf (ColumnName(I) = "vInitials") Then
                            Me.txtInitials.Text = ColumnValue(I)
                            Me.HFInitials.Value = ColumnValue(I).ToString.Trim()
                        ElseIf (ColumnName(I) = "cRegular") Then
                            CType(cph.FindControl("rbl" + (ColumnName(I))), RadioButtonList).SelectedValue = ColumnValue(I).ToString.Trim()
                            CType(cph.FindControl("rbl" + (ColumnName(I))), RadioButtonList).Enabled = False
                        ElseIf (ColumnName(I) = "cLastMenstrualIndi") Then
                            CType(cph.FindControl("rbl" + (ColumnName(I))), RadioButtonList).SelectedValue = ColumnValue(I).ToString.Trim()
                            CType(cph.FindControl("rbl" + (ColumnName(I))), RadioButtonList).Enabled = False
                        ElseIf (ColumnName(I) = "cAbortions") Then
                            CType(cph.FindControl("rbl" + (ColumnName(I))), RadioButtonList).SelectedValue = ColumnValue(I).ToString.Trim()
                            CType(cph.FindControl("rbl" + (ColumnName(I))), RadioButtonList).Enabled = False
                        ElseIf (ColumnName(I) = "cLoctating") Then
                            CType(cph.FindControl("rbl" + (ColumnName(I))), RadioButtonList).SelectedValue = ColumnValue(I).ToString.Trim()
                            CType(cph.FindControl("rbl" + (ColumnName(I))), RadioButtonList).Enabled = False
                        ElseIf (ColumnName(I) = "cIsVolunteerinBearingAge") Then
                            CType(cph.FindControl("rbl" + (ColumnName(I))), RadioButtonList).SelectedValue = ColumnValue(I).ToString.Trim()
                            CType(cph.FindControl("rbl" + (ColumnName(I))), RadioButtonList).Enabled = False
                        ElseIf (ColumnName(I) = "cContraception") Then
                            CType(cph.FindControl("rbl" + (ColumnName(I))), RadioButtonList).SelectedValue = ColumnValue(I).ToString.Trim()
                            CType(cph.FindControl("rbl" + (ColumnName(I))), RadioButtonList).Enabled = False
                        ElseIf (ColumnName(I) = "cChildrenHelath") Then
                            CType(cph.FindControl("rbl" + (ColumnName(I))), RadioButtonList).SelectedValue = ColumnValue(I).ToString.Trim()
                            CType(cph.FindControl("rbl" + (ColumnName(I))), RadioButtonList).Enabled = False
                        ElseIf (ColumnName(I) = "cBarrier") Then
                            If (ColumnValue(I).ToUpper = "C") Then
                                chkcContraception.Items(0).Selected = True
                                chkcContraception.Enabled = False
                                imgEditcBarrier.Style.Add("display", "inline")
                                imgAuditcBarrier.Style.Add("display", "inline")
                            End If
                            If (ColumnValue(I).ToUpper = "P") Then
                                chkcContraception.Items(1).Selected = True
                                chkcContraception.Enabled = False
                                imgEditcBarrier.Style.Add("display", "inline")
                                imgAuditcBarrier.Style.Add("display", "inline")
                            End If
                            If (ColumnValue(I).ToUpper = "R") Then
                                chkcContraception.Items(2).Selected = True
                                chkcContraception.Enabled = False
                                imgEditcBarrier.Style.Add("display", "inline")
                                imgAuditcBarrier.Style.Add("display", "inline")
                            End If
                            If (ColumnValue(I).ToUpper = "I") Then
                                chkcContraception.Items(3).Selected = True
                                chkcContraception.Enabled = False
                                imgEditcBarrier.Style.Add("display", "inline")
                                imgAuditcBarrier.Style.Add("display", "inline")
                            End If

                        ElseIf (ColumnName(I) = "cPills") Then
                            If (ColumnValue(I).ToUpper = "C") Then
                                chkcContraception.Items(1).Selected = True
                                chkcContraception.Enabled = False
                                imgEditcBarrier.Style.Add("display", "inline")
                                imgAuditcBarrier.Style.Add("display", "inline")
                            End If
                        ElseIf (ColumnName(I) = "cRhythm") Then
                            If (ColumnValue(I).ToUpper = "C") Then
                                chkcContraception.Items(2).Selected = True
                                chkcContraception.Enabled = False
                                imgEditcBarrier.Style.Add("display", "inline")
                                imgAuditcBarrier.Style.Add("display", "inline")
                            End If
                        ElseIf (ColumnName(I) = "cIUCD") Then
                            If (ColumnValue(I).ToUpper = "C") Then
                                chkcContraception.Items(3).Selected = True
                                chkcContraception.Enabled = False
                                imgEditcBarrier.Style.Add("display", "inline")
                                imgAuditcBarrier.Style.Add("display", "inline")
                            End If
                        ElseIf (ColumnName(I) = "ICFLanguage") Then

                        Else
                            CType(cph.FindControl("txt" + (ColumnName(I))), TextBox).Text = ColumnValue(I).ToString.Trim()
                            CType(cph.FindControl("txt" + (ColumnName(I))), TextBox).Enabled = False
                        End If
                        If (ColumnName(I) <> "vICFLanguageCodeId") Then
                            If (ColumnValue(I) <> String.Empty) Then
                                If (Request.QueryString("mode") <> 4) Then
                                    CType(cph.FindControl("imgEdit" + (ColumnName(I))), System.Web.UI.WebControls.Image).Style.Add("display", "inline")
                                End If
                                CType(cph.FindControl("imgAudit" + (ColumnName(I))), System.Web.UI.WebControls.Image).Style.Add("display", "inline")
                            End If
                        End If
                    Else
                        If (ColumnName(I) = "vPopulation") Then
                            FillPopulation()
                        End If

                        If (ColumnName(I) = "vPerCity") Then
                            FillLocation()
                        End If

                    End If


                Catch ex As Exception
                Finally
                    Try
                        CType(cph.FindControl("imgAudit" + (ColumnName(I))), System.Web.UI.WebControls.Image).Style.Add("display", "inline")
                    Catch ex As Exception

                    End Try
                End Try
            Next

            'For Each dr In ds.Tables(0).Rows

            '    Me.txtvFirstName.Text = dr("vFirstName").ToString
            '    Me.txtvMiddleName.Text = dr("vMiddleName").ToString
            '    Me.txtvSurName.Text = dr("vSurName").ToString
            '    Me.txtInitials.Text = dr("vInitials").ToString()
            '    Me.txtdBirthDate.Text = dr("dBirthDate").ToString()

            '    Me.txtAge.Text = dr("Age").ToString.Trim()

            '    Me.txtdEnrollmentDate.Text = dr("dEnrollmentDate").ToString.Trim()
            '    Me.txtvRefSubjectId.Text = dr("vRefSubjectId").ToString.Trim()

            '    If dr("cSex").ToString.ToUpper = "M" Then
            '        ddlcSex.Items(0).Selected = True
            '        ddlcSex.Items(1).Selected = False
            '    ElseIf dr("cSex").ToString.ToUpper = "F" Then
            '        ddlcSex.Items(1).Selected = True
            '        ddlcSex.Items(0).Selected = False
            '    End If

            '    If dr("cMaritalStatus").ToString.ToUpper = "S" Then
            '        ddlcMaritalStatus.Items(0).Selected = True
            '        ddlcMaritalStatus.Items(1).Selected = False
            '    ElseIf dr("cMaritalStatus").ToString.ToUpper = "M" Then
            '        ddlcMaritalStatus.Items(1).Selected = True
            '        ddlcMaritalStatus.Items(0).Selected = False
            '    End If


            '    If dr("cFoodHabit").ToString = "Vegetarian" Then
            '        ddlcFoodHabit.Items(0).Selected = True
            '        ddlcFoodHabit.Items(1).Selected = False
            '        ddlcFoodHabit.Items(2).Selected = False
            '    ElseIf dr("cFoodHabit").ToString = "Non-Vegetarian" Then
            '        ddlcFoodHabit.Items(1).Selected = True
            '        ddlcFoodHabit.Items(2).Selected = False
            '        ddlcFoodHabit.Items(0).Selected = False
            '    ElseIf dr("cFoodHabit").ToString = "Eggetarian" Then
            '        ddlcFoodHabit.Items(2).Selected = True
            '        ddlcFoodHabit.Items(0).Selected = False
            '        ddlcFoodHabit.Items(1).Selected = False
            '    End If


            '    Me.ddlcRh.SelectedValue = dr("cRh").ToString.Trim
            '    ddlcBloodGroup.SelectedValue = dr("cbloodgroup").ToString.Trim


            '    txtvEducationQualification.Text = dr("vEducationQualification").ToString

            '    StrICFlang = dr("vICFLanguageCodeId").ToString.Split(",")


            '    If chkvICFLanguageCodeId.Items.Count = 0 Then
            '        fillchkvICFLanguageCodeId()
            '    End If

            '    For indexStr As Integer = 0 To StrICFlang.Length - 1
            '        For Each lstItem As ListItem In chkvICFLanguageCodeId.Items
            '            If lstItem.Value = StrICFlang(indexStr).ToString.Trim Then
            '                lstItem.Selected = True
            '            End If
            '        Next
            '    Next indexStr

            '    txtvOccupation.Text = dr("vOccupation").ToString.Trim

            '    'This is for Height
            '    txtnHeight.Text = dr("nHeight").ToString

            '    'This is for Weight
            '    txtnWeight.Text = dr("nWeight").ToString


            '    Me.txtbmi.Text = dr("nBMI").ToString

            '    'Me.GVHabits.DataSource = dtSubjectHabitDetail
            '    'Me.GVHabits.DataBind()


            '    'This is for Local Address 1
            '    If (dr("vLocalAdd1").ToString <> "") Then
            '        strLocalAdd1 = dr("vLocalAdd1").ToString
            '    End If
            '    If (dr("vLocalAdd12").ToString <> "") Then
            '        strLocalAdd1 = strLocalAdd1 + "," + dr("vLocalAdd12").ToString
            '    End If
            '    If (dr("vLocalAdd13").ToString <> "") Then
            '        strLocalAdd1 = strLocalAdd1 + "," + dr("vLocalAdd13").ToString
            '    End If

            '    'This is for Local Address 2
            '    If (dr("vLocalAdd21").ToString <> "") Then
            '        strLocalAdd2 = dr("vLocalAdd21").ToString
            '    End If
            '    If (dr("vLocalAdd22").ToString <> "") Then
            '        strLocalAdd2 = strLocalAdd2 + "," + dr("vLocalAdd22").ToString
            '    End If
            '    If (dr("vLocalAdd23").ToString <> "") Then
            '        strLocalAdd2 = strLocalAdd2 + "," + dr("vLocalAdd23").ToString
            '    End If

            '    'This is for Per Add
            '    If (dr("vPerAdd1").ToString <> "") Then
            '        strPerAdd = dr("vPerAdd1").ToString
            '    End If
            '    If (dr("vPerAdd2").ToString <> "") Then
            '        strPerAdd = strPerAdd + "," + dr("vPerAdd2").ToString
            '    End If
            '    If (dr("vPerAdd3").ToString <> "") Then
            '        strPerAdd = strPerAdd + "," + dr("vPerAdd3").ToString
            '    End If


            '    Me.txtvLocalAdd1.Value = strLocalAdd1.ToString.Trim
            '    Me.txtvLocalAdd21.Value = strLocalAdd2.ToString.Trim
            '    Me.txtvLocalTelephoneno1.Text = dr("vLocalTelephoneno1").ToString
            '    Me.txtvLocalTelephoneno2.Text = dr("vLocalTelephoneno2").ToString
            '    Me.txtvPerAdd1.Value = strPerAdd.ToString.Trim
            '    Me.txtvPerCity.Text = dr("vPerCity").ToString.Trim()
            '    Me.txtvPerTelephoneno.Text = dr("vPerTelephoneno").ToString
            '    Me.txtvOfficeAddress.Value = dr("vOfficeAddress").ToString
            '    Me.txtvOfficeTelephoneno.Text = dr("vOfficeTelephoneno").ToString

            '    Me.txtvContactName1.Text = dr("vContactName1").ToString
            '    Me.txtvContactName2.Text = dr("vContactName2").ToString
            '    Me.txtvContactAddress11.Value = dr("vContactAddress11").ToString
            '    Me.txtvContactAddress21.Value = dr("vContactAddress21").ToString
            '    Me.txtvContactTelephoneno1.Text = dr("vContactTelephoneno1").ToString
            '    Me.txtvContactTelephoneno2.Text = dr("vContactTelephoneno2").ToString
            '    Me.txtvReferredBy.Text = dr("vReferredBy").ToString
            '    Me.txtvRemarks.Text = dr("vRemarks").ToString


            '    If Not dr("nSubjectFemaleDetailNo") Is DBNull.Value Then


            '        Me.txtdLastMenstrualDate.Text = IIf(dr("dLastMenstrualDate") Is DBNull.Value, "", dr("dLastMenstrualDate"))
            '        Me.txtiLastMenstrualDays.Text = IIf(dr("iLastMenstrualDays") Is DBNull.Value, "", dr("iLastMenstrualDays"))

            '        If dr("cRegular").ToString = "Y" Then
            '            rblcRegular.Items(0).Selected = True
            '        ElseIf dr("cRegular").ToString = "N" Then
            '            rblcRegular.Items(1).Selected = True
            '        End If

            '        Me.txtdLastDelivaryDate.Text = IIf(dr("dLastDelivaryDate") Is DBNull.Value, "", dr("dLastDelivaryDate"))

            '        Me.txtvGravida.Text = IIf(dr("vGravida") Is DBNull.Value, "", dr("vGravida").ToString)
            '        Me.txtiNoOfChildren.Text = IIf(dr("iNoOfChildren") Is DBNull.Value, "", dr("iNoOfChildren").ToString)

            '        If Not dr("cAbortions") Is DBNull.Value AndAlso dr("cAbortions").ToString.Trim <> "" Then
            '            Me.rblcAbortions.SelectedValue = dr("cAbortions").ToString
            '        End If

            '        txtdAbortionDate.Text = IIf(dr("dAbortionDate") Is DBNull.Value, "", dr("dAbortionDate"))
            '        txtvOtherRemark.InnerText = IIf(dr("vOtherRemark") Is DBNull.Value, "", dr("vOtherRemark"))


            '        Me.txtvPara.Text = IIf(dr("vPara") Is DBNull.Value, "", dr("vPara").ToString)

            '        '' Me.rblcLoctating.SelectedValue = "N"
            '        If Not dr("cLoctating") Is DBNull.Value AndAlso dr("cLoctating").ToString.Trim <> "" Then
            '            Me.rblcLoctating.SelectedValue = dr("cLoctating").ToString
            '        End If

            '        If Not dr("cContraception") Is DBNull.Value AndAlso dr("cContraception").ToString.Trim <> "" Then
            '            If dr("cContraception").ToString = "P" Then
            '                rblcContraception.Items(0).Selected = True
            '            ElseIf dr("cContraception").ToString = "T" Then
            '                rblcContraception.Items(1).Selected = True
            '            End If
            '        End If

            '        If Not dr("cLastMenstrualIndi") Is DBNull.Value AndAlso dr("cLastMenstrualIndi").ToString.Trim <> "" Then
            '            Me.rblcLastMenstrualIndi.SelectedValue = dr("cLastMenstrualIndi")
            '        End If

            '        If Not dr("cIsVolunteerinBearingAge") Is DBNull.Value AndAlso dr("cIsVolunteerinBearingAge").ToString.Trim <> "" Then

            '            Me.rblcIsVolunteerinBearingAge.SelectedValue = dr("cIsVolunteerinBearingAge")
            '        End If

            '        If Not dr("cChildrenHelath") Is DBNull.Value AndAlso dr("cChildrenHelath").ToString.Trim <> "" Then

            '            Me.rblcChildrenHelath.SelectedValue = dr("cChildrenHelath")
            '        End If

            '        Me.txtvchildrenHealthRemark.Text = IIf(dr("vchildrenHealthRemark") Is DBNull.Value, "", dr("vchildrenHealthRemark").ToString)
            '        Me.txtiNoOfChildrenDied.Text = IIf(dr("iNoOfChildrenDied") Is DBNull.Value, "", dr("iNoOfChildrenDied").ToString)
            '        Me.txtvRemarkifDied.Text = IIf(dr("vRemarkifDied") Is DBNull.Value, "", dr("vRemarkifDied").ToString)

            '        For index = 0 To chkContraception.Items.Count - 1

            '            If chkContraception.Items(index).Value = "B" AndAlso Not dr("cBarrier") Is DBNull.Value AndAlso dr("cBarrier").ToString.ToUpper.Trim = "C" Then
            '                chkContraception.Items(index).Selected = True
            '            ElseIf chkContraception.Items(index).Value = "P" AndAlso Not dr("cPills") Is DBNull.Value AndAlso dr("cPills").ToString.ToUpper.Trim = "C" Then
            '                chkContraception.Items(index).Selected = True
            '            ElseIf chkContraception.Items(index).Value = "R" AndAlso Not dr("cRhythm") Is DBNull.Value AndAlso dr("cRhythm").ToString.ToUpper.Trim = "C" Then
            '                chkContraception.Items(index).Selected = True
            '            ElseIf chkContraception.Items(index).Value = "I" AndAlso Not dr("cIUCD") Is DBNull.Value AndAlso dr("cIUCD").ToString.ToUpper.Trim = "C" Then
            '                chkContraception.Items(index).Selected = True
            '            End If

            '        Next index
            '    End If
            'Next



            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    Private Function ResetPage() As Boolean
        Try
            Me.txtvFirstName.Text = String.Empty
            Me.txtvMiddleName.Text = String.Empty
            Me.txtvSurName.Text = String.Empty
            Me.txtInitials.Text = String.Empty
            Me.txtdBirthDate.Text = String.Empty
            Me.txtAge.Text = String.Empty
            Me.txtdEnrollmentDate.Text = String.Empty
            Me.txtvRefSubjectId.Text = String.Empty
            ddlcSex.Items(0).Selected = True
            ddlcSex.Items(1).Selected = False
            ddlcMaritalStatus.Items(0).Selected = True
            ddlcMaritalStatus.Items(1).Selected = False
            ddlcFoodHabit.Items(0).Selected = True
            ddlcFoodHabit.Items(1).Selected = False
            ddlcFoodHabit.Items(2).Selected = False
            Me.ddlcRh.SelectedIndex = 0
            ddlcBloodGroup.SelectedIndex = 0
            txtvEducationQualification.Text = String.Empty
            chkvICFLanguageCodeId.Items.Clear()
            If chkvICFLanguageCodeId.Items.Count = 0 Then
                fillchkvICFLanguageCodeId()
            End If
            txtvOccupation.Text = String.Empty
            txtnHeight.Text = String.Empty
            txtnWeight.Text = String.Empty
            Me.txtbmi.Text = String.Empty

            Me.txtvLocalAdd1.Text = String.Empty
            Me.txtvLocalAdd21.Text = String.Empty
            Me.txtvLocalTelephoneno1.Text = String.Empty
            Me.txtvLocalTelephoneno2.Text = String.Empty
            Me.txtvPerAdd1.Text = String.Empty
            'Me.txtvPerCity.Text = String.Empty
            Me.txtvPerTelephoneno.Text = String.Empty
            Me.txtvOfficeAddress.Text = String.Empty
            Me.txtvOfficeTelephoneno.Text = String.Empty

            Me.txtvContactName1.Text = String.Empty
            Me.txtvContactName2.Text = String.Empty
            Me.txtvContactAddress11.Text = String.Empty
            Me.txtvContactAddress21.Text = String.Empty
            Me.txtvContactTelephoneno1.Text = String.Empty
            Me.txtvContactTelephoneno2.Text = String.Empty
            Me.txtvReferredBy.Text = String.Empty
            Me.txtvRemarks.Text = String.Empty

            Me.txtdLastMenstrualDate.Text = String.Empty
            Me.txtiLastMenstrualDays.Text = String.Empty



            Me.txtdLastDelivaryDate.Text = String.Empty
            Me.txtvGravida.Text = String.Empty
            Me.txtiNoOfChildren.Text = String.Empty
            txtdAbortionDate.Text = String.Empty
            Me.txtvPara.Text = String.Empty

            Me.rblcAbortions.Items(0).Selected = False
            Me.rblcAbortions.Items(1).Selected = False

            rblcRegular.Items(0).Selected = False
            rblcRegular.Items(1).Selected = False

            Me.rblcLoctating.Items(0).Selected = False
            Me.rblcLoctating.Items(1).Selected = False

            rblcContraception.Items(0).Selected = False
            rblcContraception.Items(1).Selected = False

            Me.rblcLastMenstrualIndi.Items(0).Selected = False
            Me.rblcLastMenstrualIndi.Items(1).Selected = False

            Me.rblcIsVolunteerinBearingAge.Items(0).Selected = False
            Me.rblcIsVolunteerinBearingAge.Items(1).Selected = False

            Me.rblcChildrenHelath.Items(0).Selected = False
            Me.rblcChildrenHelath.Items(1).Selected = False
            Me.rblcChildrenHelath.Items(2).Selected = False

            Me.txtvchildrenHealthRemark.Text = String.Empty
            Me.txtiNoOfChildrenDied.Text = String.Empty
            Me.txtvRemarkifDied.Text = String.Empty


            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString, "")
            Return False
        End Try
    End Function

    Private Function AssingAttribute() As Boolean

        Try

            Me.txtvFirstName.Attributes.Add("tName", "SubjectMaster")
            Me.txtvFirstName.Attributes.Add("cName", "vFirstName")

            Me.txtvMiddleName.Attributes.Add("tName", "SubjectMaster")
            Me.txtvMiddleName.Attributes.Add("cName", "vMiddleName")

            Me.txtvSurName.Attributes.Add("tName", "SubjectMaster")
            Me.txtvSurName.Attributes.Add("cName", "vSurName")

            Me.txtdEnrollmentDate.Attributes.Add("tName", "SubjectMaster")
            Me.txtdEnrollmentDate.Attributes.Add("cName", "dEnrollmentDate")

            Me.txtdBirthDate.Attributes.Add("tName", "SubjectMaster")
            Me.txtdBirthDate.Attributes.Add("cName", "dBirthDate")

            'Me.txtvFirstName.Attributes.Add("tName", "SubjectMaster")
            'Me.txtvFirstName.Attributes.Add("cName", "vFirstName")

            'Me.txtvMiddleName.Attributes.Add("tName", "SubjectMaster")
            'Me.txtvMiddleName.Attributes.Add("cName", "vMiddleName")

            'Me.txtvSurName.Attributes.Add("tName", "SubjectMaster")
            'Me.txtvSurName.Attributes.Add("cName", "vSurName")

            'Me.txtInitials.Attributes.Add("tName", "SubjectMaster")
            'Me.txtInitials.Attributes.Add("cName", "vInitials")

            'Me.txtdBirthDate.Attributes.Add("tName", "SubjectMaster")
            'Me.txtdBirthDate.Attributes.Add("cName", "dBirthDate")

            Me.txtdEnrollmentDate.Attributes.Add("tName", "SubjectMaster")
            Me.txtdEnrollmentDate.Attributes.Add("cName", "dEnrollmentDate")

            Me.txtvRefSubjectId.Attributes.Add("tName", "SubjectMaster")
            Me.txtvRefSubjectId.Attributes.Add("cName", "vRefSubjectId")

            Me.ddlcSex.Attributes.Add("tName", "SubjectMaster")
            Me.ddlcSex.Attributes.Add("cName", "cSex")

            Me.ddlcRh.Attributes.Add("tName", "SubjectMaster")
            Me.ddlcRh.Attributes.Add("cName", "cRh")

            Me.txtvOccupation.Attributes.Add("tName", "SubjectMaster")
            Me.txtvOccupation.Attributes.Add("cName", "vOccupation")


            Me.ddlcBloodGroup.Attributes.Add("tName", "SubjectMaster")
            Me.ddlcBloodGroup.Attributes.Add("cName", "cbloodgroup")

            Me.txtvEducationQualification.Attributes.Add("tName", "SubjectMaster")
            Me.txtvEducationQualification.Attributes.Add("cName", "vEducationQualification")

            Me.ddlcMaritalStatus.Attributes.Add("tName", "SubjectDetails")
            Me.ddlcMaritalStatus.Attributes.Add("cName", "cMaritalStatus")

            Me.ddlcFoodHabit.Attributes.Add("tName", "SubjectDetails")
            Me.ddlcFoodHabit.Attributes.Add("cName", "cFoodHabit")

            Me.chkvICFLanguageCodeId.Attributes.Add("tName", "SubjectMaster")
            Me.chkvICFLanguageCodeId.Attributes.Add("cName", "vICFLanguageCodeId")

            Me.txtnHeight.Attributes.Add("tName", "SubjectDetails")
            Me.txtnHeight.Attributes.Add("cName", "nHeight")

            Me.txtnWeight.Attributes.Add("tName", "SubjectDetails")
            Me.txtnWeight.Attributes.Add("cName", "nWeight")

            Me.txtbmi.Attributes.Add("tName", "SubjectDetails")
            Me.txtbmi.Attributes.Add("cName", "nBMI")

            Me.txtvLocalAdd1.Attributes.Add("tName", "SubjectContactDetails")
            Me.txtvLocalAdd1.Attributes.Add("cName", "vLocalAdd1,vLocalAdd12,vLocalAdd13")
            'Me.txtvLocalAdd1.Attributes.Add("cName", "vLocalAdd1")

            Me.txtvLocalAdd21.Attributes.Add("tName", "SubjectContactDetails")
            Me.txtvLocalAdd21.Attributes.Add("cName", "vLocalAdd21,vLocalAdd22,vLocalAdd23")
            'Me.txtvLocalAdd21.Attributes.Add("cName", "vLocalAdd21")

            Me.txtvPerAdd1.Attributes.Add("tName", "SubjectContactDetails")
            Me.txtvPerAdd1.Attributes.Add("cName", "vPerAdd1,vPerAdd2,vPerAdd3")

            Me.txtvLocalTelephoneno1.Attributes.Add("tName", "SubjectContactDetails")
            Me.txtvLocalTelephoneno1.Attributes.Add("cName", "vLocalTelephoneno1")

            Me.txtvLocalTelephoneno2.Attributes.Add("tName", "SubjectContactDetails")
            Me.txtvLocalTelephoneno2.Attributes.Add("cName", "vLocalTelephoneno2")

            Me.ddlvPerCity.Attributes.Add("tName", "SubjectContactDetails")
            Me.ddlvPerCity.Attributes.Add("cName", "vPerCity")

            Me.txtvPerTelephoneno.Attributes.Add("tName", "SubjectContactDetails")
            Me.txtvPerTelephoneno.Attributes.Add("cName", "vPerTelephoneno")

            Me.txtvOfficeAddress.Attributes.Add("tName", "SubjectContactDetails")
            Me.txtvOfficeAddress.Attributes.Add("cName", "vOfficeAddress")

            Me.txtvOfficeTelephoneno.Attributes.Add("tName", "SubjectContactDetails")
            Me.txtvOfficeTelephoneno.Attributes.Add("cName", "vOfficeTelephoneno")

            Me.txtvContactName1.Attributes.Add("tName", "SubjectContactDetails")
            Me.txtvContactName1.Attributes.Add("cName", "vContactName1")

            Me.txtvContactName2.Attributes.Add("tName", "SubjectContactDetails")
            Me.txtvContactName2.Attributes.Add("cName", "vContactName2")

            Me.txtvContactAddress11.Attributes.Add("tName", "SubjectContactDetails")
            Me.txtvContactAddress11.Attributes.Add("cName", "vContactAddress11")

            Me.txtvContactAddress21.Attributes.Add("tName", "SubjectContactDetails")
            Me.txtvContactAddress21.Attributes.Add("cName", "vContactAddress21")

            Me.txtvContactTelephoneno1.Attributes.Add("tName", "SubjectContactDetails")
            Me.txtvContactTelephoneno1.Attributes.Add("cName", "vContactTelephoneno1")

            Me.txtvContactTelephoneno2.Attributes.Add("tName", "SubjectContactDetails")
            Me.txtvContactTelephoneno2.Attributes.Add("cName", "vContactTelephoneno2")

            Me.txtvReferredBy.Attributes.Add("tName", "SubjectContactDetails")
            Me.txtvReferredBy.Attributes.Add("cName", "vReferredBy")

            Me.txtdLastMenstrualDate.Attributes.Add("tName", "SubjectFemaleDetails")
            Me.txtdLastMenstrualDate.Attributes.Add("cName", "dLastMenstrualDate")

            Me.txtiLastMenstrualDays.Attributes.Add("tName", "SubjectFemaleDetails")
            Me.txtiLastMenstrualDays.Attributes.Add("cName", "iLastMenstrualDays")

            Me.rblcRegular.Attributes.Add("tName", "SubjectFemaleDetails")
            Me.rblcRegular.Attributes.Add("cName", "cRegular")

            Me.txtdLastDelivaryDate.Attributes.Add("tName", "SubjectFemaleDetails")
            Me.txtdLastDelivaryDate.Attributes.Add("cName", "dLastDelivaryDate")

            Me.txtvGravida.Attributes.Add("tName", "SubjectFemaleDetails")
            Me.txtvGravida.Attributes.Add("cName", "vGravida")

            Me.txtiNoOfChildren.Attributes.Add("tName", "SubjectFemaleDetails")
            Me.txtiNoOfChildren.Attributes.Add("cName", "iNoOfChildren")

            Me.rblcAbortions.Attributes.Add("tName", "SubjectFemaleDetails")
            Me.rblcAbortions.Attributes.Add("cName", "cAbortions")

            Me.txtdAbortionDate.Attributes.Add("tName", "SubjectFemaleDetails")
            Me.txtdAbortionDate.Attributes.Add("cName", "dAbortionDate")

            Me.txtvPara.Attributes.Add("tName", "SubjectFemaleDetails")
            Me.txtvPara.Attributes.Add("cName", "vPara")

            Me.rblcLoctating.Attributes.Add("tName", "SubjectFemaleDetails")
            Me.rblcLoctating.Attributes.Add("cName", "cLoctating")

            Me.rblcContraception.Attributes.Add("tName", "SubjectFemaleDetails")
            Me.rblcContraception.Attributes.Add("cName", "cContraception")

            Me.rblcLastMenstrualIndi.Attributes.Add("tName", "SubjectFemaleDetails")
            Me.rblcLastMenstrualIndi.Attributes.Add("cName", "cLastMenstrualIndi")

            Me.rblcIsVolunteerinBearingAge.Attributes.Add("tName", "SubjectFemaleDetails")
            Me.rblcIsVolunteerinBearingAge.Attributes.Add("cName", "cIsVolunteerinBearingAge")

            Me.rblcChildrenHelath.Attributes.Add("tName", "SubjectFemaleDetails")
            Me.rblcChildrenHelath.Attributes.Add("cName", "cChildrenHelath")

            Me.txtvchildrenHealthRemark.Attributes.Add("tName", "SubjectFemaleDetails")
            Me.txtvchildrenHealthRemark.Attributes.Add("cName", "vchildrenHealthRemark")

            Me.txtiNoOfChildrenDied.Attributes.Add("tName", "SubjectFemaleDetails")
            Me.txtiNoOfChildrenDied.Attributes.Add("cName", "iNoOfChildrenDied")

            Me.txtvRemarkifDied.Attributes.Add("tName", "SubjectFemaleDetails")
            Me.txtvRemarkifDied.Attributes.Add("cName", "vRemarkifDied")

            Me.txtvOtherRemark.Attributes.Add("tName", "SubjectFemaleDetails")
            Me.txtvOtherRemark.Attributes.Add("cName", "vOtherRemark")

            Me.chkcContraception.Attributes.Add("tName", "SubjectFemaleDetails")
            Me.chkcContraception.Attributes.Add("cName", "cBarrier,cPills,cRhythm,cIUCD")

            Me.GVSubjectProof.Attributes.Add("tName", "SubjectProofDetails")
            Me.GVSubjectProof.Attributes.Add("cName", "cStatusIndi")

            Me.txtvRemarks.Attributes.Add("tName", "SubjectContactDetails")
            Me.txtvRemarks.Attributes.Add("cName", "vRemarks")

            Me.txtvDemographicRemarks.Attributes.Add("tName", "SubjectDetails")
            Me.txtvDemographicRemarks.Attributes.Add("cName", "vDemographicRemarks")

            Me.ddlvPopulation.Attributes.Add("tName", "SubjectMaster")
            Me.ddlvPopulation.Attributes.Add("cName", "vPopulation")
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...AssignAttribute()")
            Return False
        End Try




    End Function


#End Region

#Region "ActualSaveSubjectProof"

    Private Function ActualSaveSubjectProof(ByVal SubjectId As String) As Boolean
        Dim dr As DataRow
        Dim ds As New DataSet
        Dim strPath As String = String.Empty
        Dim di As DirectoryInfo
        Dim Retu_TranNo As String = String.Empty
        Dim fileName As String = String.Empty
        Dim filePath As String = String.Empty
        Dim sourcePath As String = String.Empty

        Try



            If Not objHelp.GetData("SubjectProofDetails", "*", "1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds, estr) Then
                Throw New Exception(estr)
            End If

            If GVSubjectProof.Rows.Count > 0 Then
                For i As Integer = 0 To GVSubjectProof.Rows.Count - 1
                    If Me.GVSubjectProof.Rows(i).Cells(GVCSubProof_nSubjectProofNo).Text <= 0 Then
                        dr = ds.Tables(0).NewRow()
                        dr("nSubjectProofNo") = Me.GVSubjectProof.Rows(i).Cells(GVCSubProof_nSubjectProofNo).Text
                        dr("vSubjectId") = SubjectId.ToString()
                        dr("iTranNo") = Me.GVSubjectProof.Rows(i).Cells(GVCSubProof_iTranNo).Text
                        dr("vProofType") = Me.GVSubjectProof.Rows(i).Cells(GVCSubProof_vProofType).Text
                        strPath = "~/" + System.Configuration.ConfigurationSettings.AppSettings("FolderForSubjectProof")
                        filePath = Me.GVSubjectProof.Rows(i).Cells(GVCSubProof_vProofPath).Text.Trim()
                        fileName = filePath.Substring(filePath.LastIndexOf("\") + 1) ', filePath.Length - 1)
                        'fileName = filePath.Substring(filePath.LastIndexOf("/") + 1)

                        dr("vProofPath") = strPath + SubjectId.ToString.Trim() + "/" + fileName
                        dr("iModifyBy") = Me.GVSubjectProof.Rows(i).Cells(GVCSubProof_iModifyBy).Text
                        dr("cStatusIndi") = Me.GVSubjectProof.Rows(i).Cells(GVCSubProof_cStatusIndi).Text
                        If (Me.HFRemarks.Value = "") Then
                            dr("vUpdateRemarks") = ""
                        Else
                            dr("vUpdateRemarks") = "" = Me.HFRemarks.Value

                        End If

                        ds.Tables(0).Rows.Add(dr)
                        ds.Tables(0).AcceptChanges()
                    End If
                Next

            End If

            If GVSubjectProof.Rows.Count = 0 Then
                dr = ds.Tables(0).NewRow()
                dr("nSubjectProofNo") = "0"
                dr("vSubjectId") = SubjectId.ToString()
                dr("iTranNo") = "1"
                dr("vProofType") = ""
                '' strPath = "~/" + System.Configuration.ConfigurationSettings.AppSettings("FolderForSubjectProof")
                ''filePath = Me.GVSubjectProof.Rows(i).Cells(GVCSubProof_vProofPath).Text.Trim()
                ''fileName = filePath.Substring(filePath.LastIndexOf("\") + 1) ', filePath.Length - 1)
                'fileName = filePath.Substring(filePath.LastIndexOf("/") + 1)

                dr("vProofPath") = ""
                dr("iModifyBy") = Session(S_UserID)
                dr("cStatusIndi") = "N"
                If (Me.HFRemarks.Value = "") Then
                    dr("vUpdateRemarks") = ""
                Else
                    dr("vUpdateRemarks") = "" = Me.HFRemarks.Value

                End If

                ds.Tables(0).Rows.Add(dr)
                ds.Tables(0).AcceptChanges()
            End If

            If ds.Tables(0).Rows.Count > 0 Then

                ds.Tables(0).TableName = "SubjectProofDetails"
                If Not Me.objLambda.Save_SubjectProofDetails(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds, Me.Session(S_UserID), Retu_TranNo, estr) Then
                    Throw New Exception(estr)
                End If

                'If Choice = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                strPath = Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings("FolderForSubjectProof"))
                strPath += SubjectId.ToString + "\" + Retu_TranNo + "\"
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
                ' End If
            End If





            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".........ActualSaveSubjectProof")
            Return False
        End Try
    End Function

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
        Dim dt_Save1 As DataTable = Nothing
        Dim dsSubject As DataSet = Nothing


        Try
            Choice = Me.ViewState(VS_Choice)

            ds_SubjectProof.Tables.Add(CType(Me.ViewState(VS_GVDtSubjectProofDetails), DataTable).Copy())
            If Not objHelp.GetData("SubjectProofDetails", "*", "vSubjectId='" + HSubjectId.Value.Trim() + "' And cStatusIndi<>'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsSubject, estr) Then
                Throw New Exception(estr)
            End If


            If ds_SubjectProof.Tables(0).Rows.Count < 1 Then
                Return True
            End If

            dt_Save = dsSubject.Tables(0)

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
                dt_Save1 = dt_Save.Copy()
                ds_Save.Tables.Clear()
                ds_Save.Tables.Add(dt_Save1)

                ds_Save.Tables(0).TableName = "SubjectProofDetails"

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

            If (ViewState("Edit") = "Y") Then
                btnEdit_Click(Nothing, Nothing)

            End If

            Me.ViewState(VS_TempTransactionNo) = 0
            Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)
            Return True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".........ActualSaveSubjectProof")
            Return False
        End Try
    End Function

#End Region


#Region "ValidationForSameSubject"
    Public Function SameSubjectValidation() As Boolean
        Try

            Dim ds As New DataSet
            Dim wStr As String = String.Empty
            Dim eStr As String = String.Empty
            Dim SubjectId As String = String.Empty

            Try

                wStr = "vMiddleName = '" + Me.txtvMiddleName.Text.Trim() + "' AND vSurName = '" + txtvSurName.Text.Trim() + "'  AND vFirstName = '" + txtvFirstName.Text + "'  And dBirthDate = '" + Me.txtdBirthDate.Text.Trim() + "'"
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
                        If (ViewState("Edit") <> "Y") Then
                            Me.ObjCommon.ShowAlert("  Volunteer with same name and D.O.B.already exists having subject Id : " + SubjectId + ".", Me.Page)
                            Return False
                        End If
                    End If

                End If
            Catch ex As Exception
                Me.ShowErrorMessage("Error While Checking For Subject Duplication. ", ex.Message)
            End Try
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
#End Region

#Region "Fill Population"
    Private Function FillPopulation() As Boolean
        Dim Ds_SubjectPopulation As New DataSet
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim ds As New DataSet
        Try

            Wstr = "cStatusIndi <> 'D'"
            If Not Me.objHelp.getSubjectPopulationMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        Ds_SubjectPopulation, estr) Then
                Me.ShowErrorMessage(estr, estr)
                Return False
            End If

            ddlvPopulation.DataSource = Ds_SubjectPopulation
            ddlvPopulation.DataTextField = "vPopulationName"
            ddlvPopulation.DataValueField = "nPopulationId"
            ddlvPopulation.DataBind()
            ddlvPopulation.Items.Insert(0, "--Select Population --")

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......Subject Population")
            Return False
        End Try
    End Function
#End Region

#Region "Fill Location Place"
    Private Function FillLocation() As Boolean
        Dim Ds_SubjectLocation As New DataSet
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Dim ds As New DataSet
        Try

            Wstr = "cStatusIndi <> 'D'"
            If Not Me.objHelp.getLocationMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                        Ds_SubjectLocation, estr) Then
                Me.ShowErrorMessage(estr, estr)
                Return False
            End If

            ddlvPerCity.DataSource = Ds_SubjectLocation
            ddlvPerCity.DataTextField = "vLocationName"
            ddlvPerCity.DataValueField = "vLocationCode"
            ddlvPerCity.DataBind()

            ddlvPerCity.Items.Insert(0, "--Select Place --")

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "......Fill Location ")
            Return False
        End Try
    End Function
#End Region


#Region "Web Methods"
    <Web.Services.WebMethod()> _
    Public Shared Function UpdateFieldValues(ByVal SubjectId As String, ByVal ColumnName As String, _
                                             ByVal TableName As String, ByVal ChangedValue As String, _
                                             ByVal Remarks As String, ByVal JSONString As String) As Boolean


        ChangedValue = ChangedValue.Replace("'", "''")
        If TableName = "SubjectContactDetails" Then
            If Not ChangedValue.Contains("#") Then
                If ColumnName.Contains(",") Then
                    ChangedValue += "##"
                End If
            End If

            'If ChangedValue = "" Then
            '    ChangedValue = "###"
            'End If
        End If
        Dim ObjCommon As New clsCommon
        Dim objhelpDb As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()

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

                ' ds_Field.Tables.Add(JsonConvert.DeserializeObject(JSONString, GetType(System.Data.DataTable)))

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
                ds_Save.Tables.Add(dt_Field)




                If Not objLambda.Insert_UpdateSubjectPIFFields(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Save, _
                                                     HttpContext.Current.Session(S_UserID), eStr) Then
                    Return False
                    Exit Function
                End If
            Else
                ds_Field.Tables.Add(JsonConvert.DeserializeObject(JSONString, GetType(System.Data.DataTable)))
                For Each dr As DataRow In ds_Field.Tables(0).Rows
                    dr("iModifyBy") = HttpContext.Current.Session(S_UserID)
                    dr("dModifyon") = Date.Now()
                Next
                ds_Field.AcceptChanges()
                If Not objLambda.Insert_SubjectHabitDetails(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_Field, eStr) Then
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

    Protected Sub btnSaveProof_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveProof.Click
        Dim dsSubject As New DataSet
        Try
            If Not ActualSaveSubjectProof(Me.HSubjectId.Value.Trim()) Then
                Throw New Exception("Error while save actaul subject proof")
            End If
            dsSubject = Nothing
            If Not objHelp.GetData("SubjectProofDetails", "*", "vSubjectId='" + HSubjectId.Value.Trim() + "' And cStatusIndi<>'D'", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, dsSubject, estr) Then
                Throw New Exception(estr)
            End If
            Me.upAttach.Update()
            Me.GVSubjectProof.DataSource = Nothing
            Me.GVSubjectProof.DataBind()

            If dsSubject.Tables(0).Rows.Count > 0 Then
                Me.GVSubjectProof.DataSource = dsSubject.Tables(0)
                Me.GVSubjectProof.DataBind()
                If (ViewState("Edit") <> "Y") Then
                    Me.Image36.Style.Add("Display", "none")
                    Me.Image37.Style.Add("Display", "none")
                End If
            Else
                Me.Image36.Style.Add("Display", "none")
                Me.Image37.Style.Add("Display", "none")
            End If
            Me.HFAttachstatus.Value = ""
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "disableControl12", "disableControl();", True)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.ToString(), "")
        End Try
    End Sub

    Protected Sub btngenerate_Click(sender As Object, e As EventArgs) Handles btngenerate.Click

        Try

            Dim URL As String
            URL = Request.Url.ToString()
            Dim htmlStringWriter As New System.IO.StringWriter()
            Dim sourceString As String = Me.HFHeaderLabel.Value

            Dim d1 As Document

            Dim pdfconverter As HtmlToPdfConverter = New HtmlToPdfConverter()
            Dim downloadbytes As Byte()

            pdfconverter.PdfDocumentOptions.AvoidImageBreak = True
            pdfconverter.PdfDocumentOptions.AvoidTextBreak = True
            'pdfconverter.TruncateOutOfBoundsText = True
            pdfconverter.PdfDocumentOptions.StretchToFit = True
            pdfconverter.PdfDocumentOptions.FitWidth = True
            pdfconverter.LicenseKey = "dfvo+uv66OPj6vrr6PTq+unr9Ovo9OPj4+P66g=="
            pdfconverter.PdfDocumentOptions.TopMargin = 20
            pdfconverter.PdfDocumentOptions.BottomMargin = 20
            pdfconverter.PdfDocumentOptions.LeftMargin = 20
            pdfconverter.PdfHeaderOptions.HeaderHeight = 100
            'pdfconverter.PdfHeaderOptions.HeaderTextFontName = "verdana"
            'pdfconverter.PdfHeaderOptions.DrawHeaderLine = False
            'pdfconverter.PdfFooterOptions.DrawFooterLine = True
            'pdfconverter.PdfFooterOptions.FooterTextFontName = "verdana"
            'pdfconverter.PdfFooterOptions.PageNumberTextFontName = "verdana"
            'pdfconverter.PdfFooterOptions.PageNumberTextFontSize = 7

            pdfconverter.PdfDocumentOptions.JpegCompressionEnabled = False
            pdfconverter.PdfDocumentOptions.JpegCompressionLevel = 0
            pdfconverter.PdfDocumentOptions.ShowHeader = True
            pdfconverter.PdfDocumentOptions.ShowFooter = True
            ' pdfconverter.PdfFooterOptions.ShowPageNumber = True
            'pdfconverter.PdfFooterOptions.PageNumberYLocation = 30
            '   pdfconverter.PdfFooterOptions.PageNumberingStartIndex = 0
            sourceString = Me.HFHeaderLabel.Value

            Dim headercontent As String = String.Empty
            Dim htmlcontent As String

            ' headercontent = Regex.Replace(Me.HFHeaderPDF.Value.ToString(), "<IMG[^>]+", "<IMG id=ctl00_CPHLAMBDA_ImgLogo alt=" + ImgLogo.Src.ToString + " align=right src=" + ImgLogo.Src.ToString + " /", RegexOptions.IgnoreCase)

            '=====================================Header===========================================
            Dim Path As String = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.ToString.LastIndexOf("/"))
            ImgLogo.Src = Path + "/" + ImgLogo.Src
            headercontent = Regex.Replace(Me.HFHeaderPDF.Value.ToString(), "<IMG[^>]+", "<IMG id=ctl00_CPHLAMBDA_ImgLogo alt=" + ImgLogo.Src.ToString + " align=right src=" + ImgLogo.Src.ToString + " /", RegexOptions.IgnoreCase)
            htmlcontent = Me.HFHeaderLabel.Value.ToString()
            Dim htmlHeader As New HtmlToPdfElement(headercontent, Nothing) '21.0, 0.0,
            pdfconverter.PdfHeaderOptions.AddElement(htmlHeader)
            '======================================================================================
            '=====================================Footer===========================================
            'pdfconverter.PdfFooterOptions.FooterTextFontName = "Tahoma"

            'pdfconverter.PdfFooterOptions.FooterText = "       *This is an Electronically generated report.                                                                                                         Page &p; of &P;                     "
            pdfconverter.PdfFooterOptions.PageNumberingStartIndex = 0
            pdfconverter.PdfFooterOptions.AddElement(New LineElement(0, 0, pdfconverter.PdfDocumentOptions.PdfPageSize.Width - pdfconverter.PdfDocumentOptions.LeftMargin - pdfconverter.PdfDocumentOptions.RightMargin, 0))
            Dim footerText As New TextElement(0, 15, "*This is an Electronically Generated Report.                                                                                                         Page &p; of &P;                    ", New System.Drawing.Font(New System.Drawing.FontFamily("Tahoma"), 8.5, Drawing.GraphicsUnit.Point))
            footerText.TextAlign = HorizontalTextAlign.Right
            footerText.ForeColor = System.Drawing.Color.Black
            footerText.EmbedSysFont = True
            pdfconverter.PdfFooterOptions.AddElement(footerText)
            '======================================================================================

            d1 = pdfconverter.ConvertHtmlToPdfDocumentObject(htmlcontent, "")

            'Dim Path1 = "c:\Downloads\"
            'Path1 += Me.HSubjectId.Value
            'Path1 += ".pdf"
            'd1.Save(Path1)

            ' d1.Save("c:\Downloads\" +Me.HSubjectId  +."pdf"")
            downloadbytes = d1.Save()

            Dim response As System.Web.HttpResponse = System.Web.HttpContext.Current.Response
            response.Clear()
            response.ContentType = "application/pdf"
            response.AddHeader("content-disposition", "attachment; filename=" + Me.HSubjectId.Value + ".pdf; size=" & downloadbytes.Length.ToString())
            response.Flush()
            response.BinaryWrite(downloadbytes)
            response.Flush()
            response.End()
        Catch ex As Exception
            Me.ObjCommon.ShowAlert(ex.ToString(), Me.Page)
        End Try

    End Sub

    Protected Function Createpdf() As Boolean
        Dim dsSuebjctMaster As Data.DataTable
        Dim Path2 As String = ""
        Try
            Path2 = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.ToString.LastIndexOf("/"))
            dsSuebjctMaster = (CType(ViewState(VS_DtSubjectMaster), Data.DataTable).Copy())
            Dim ColumnName(dsSuebjctMaster.Columns.Count - 1) As String
            Dim ColumnValue(dsSuebjctMaster.Columns.Count - 1) As String
            lblSubjectNo.Text = Me.HSubjectId.Value

            Try
                PlaceMedEx.Controls.Add(New LiteralControl("<fieldset id=""onalo"" class=""FieldSetBox"" style=""text-align: left;  width: 90%;  font-family: ""Times New Roman"""">"))
                PlaceMedEx.Controls.Add(New LiteralControl("<legend class=""Legext"" style=""font-size: 16px !important; font-family: ""Times New Roman""""><B>Personal Information</B></legend>"))



                PlaceMedEx.Controls.Add(New LiteralControl("<Table style=""width: 99%; font-family: ""Times New Roman"""">"))

                PlaceMedEx.Controls.Add(New LiteralControl("<Tr  collapse=""2"">"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td width=""70%""  >"))

                PlaceMedEx.Controls.Add(New LiteralControl("<fieldset id=""ffo"" class=""Fieox"" style=""text-align: left; margin:0px 20px 20px 20px; width: 90%;"">"))
                PlaceMedEx.Controls.Add(New LiteralControl("<legend class=""Legendt"" style=""font-size: 16px !important;""><B>Personal Information</B></legend>"))

                PlaceMedEx.Controls.Add(New LiteralControl("<Table style=""width: 99%; font-family: ""Times New Roman"""" >"))

                PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""50%"" collapse=""2"">"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > LastName: </td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtvSurName.Text + """></input></td>"))


                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > First Name: </td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtvFirstName.Text + """></input></td>"))

                PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))


                PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""40%"" collapse=""2"" >"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Middle Name: </td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtvMiddleName.Text + """></input></td>"))


                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Initials: </td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtInitials.Text + """></input></td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))


                PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""40%"" collapse=""2"" >"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Date of Birth: </td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtdBirthDate.Text + """></input></td>"))


                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Date of Enrolment: </td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtdEnrollmentDate.Text + """></input></td>"))

                PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))


                PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""20%"" collapse=""2"">"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Ref. Subject Code: </td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtvRefSubjectId.Text + """></input></td>"))


                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Age: </td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtAge.Text + """>(Years)</input></td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))


                PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""20%"" >"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Sex: </td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + ddlcSex.SelectedItem.Text.ToString() + """></input></td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                PlaceMedEx.Controls.Add(New LiteralControl("</Table>"))


                PlaceMedEx.Controls.Add(New LiteralControl("</fieldset>"))

                PlaceMedEx.Controls.Add(New LiteralControl("</td>"))

                PlaceMedEx.Controls.Add(New LiteralControl("<td>"))


                Dim Path As String = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.ToString.LastIndexOf("/"))

                PlaceMedEx.Controls.Add(New LiteralControl("<fieldset id=""info"" class=""Fox"" style=""text-align: left; width; 100%;  Height:200px;"">"))
                PlaceMedEx.Controls.Add(New LiteralControl("<legend class=""Let"" ><B>Photograph</B></legend>"))

                PlaceMedEx.Controls.Add(New LiteralControl("<Table style=""width: 99%;"">"))
                PlaceMedEx.Controls.Add(New LiteralControl("<Tr  collapse=""2"">"))

                If ViewState("Edit") = "Y" Then
                    Dim ds As DataSet
                    Dim img
                    Wstr = "cRecordType='P' and iTranNo=(select Max(itranNO) from SubjectBlobDetails where cRecordType='P' and vSubjectId='" & HSubjectId.Value.ToString.Trim & "') and vSubjectId='" & HSubjectId.Value.ToString.Trim & "'"

                    If Not Me.objHelp.getSubjectBlobDetails(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds, estr) Then
                        MsgBox("Error while getting Data." + vbCrLf + estr)
                        Exit Function
                    End If
                    Try

                        If (ds.Tables(0).Rows.Count > 0) Then
                            Dim bytes As Byte() = ds.Tables(0).Rows(0)(3)

                            bytes = DirectCast(bytes, Byte())

                            Dim btmp As System.Drawing.Bitmap = BytesToBmp_Serialized(bytes)

                            Dim SubjectID As String = Me.HSubjectId.Value
                            SubjectID += CType(DateTime.Now.Ticks, String)
                            SubjectID += ".JPG"

                            Dim path1 = Server.MapPath("Images/") + "" + SubjectID + ""
                            btmp.Save(path1)
                            PlaceMedEx.Controls.Add(New LiteralControl("<td><img id=""imgphoto"" alt=" + Path2 + "/Images/" + SubjectID + " src=" + Path2 + "/Images/" + SubjectID + "></img></td>"))
                        Else
                            PlaceMedEx.Controls.Add(New LiteralControl("<td><img id=""imgphoto""  ></img></td>"))
                        End If

                    Catch ex As Exception

                    End Try
                Else

                End If

                PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</Table>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</fieldset>"))

                PlaceMedEx.Controls.Add(New LiteralControl("</td>"))

                PlaceMedEx.Controls.Add(New LiteralControl("</tr>"))




                PlaceMedEx.Controls.Add(New LiteralControl("<tr collapse=""2"">"))

                PlaceMedEx.Controls.Add(New LiteralControl("<td>"))

                PlaceMedEx.Controls.Add(New LiteralControl("<fieldset id=""fMt"" class=""Fix"" style=""text-align: left; margin:0px 20px 20px 20px; width: 90%;"">"))
                PlaceMedEx.Controls.Add(New LiteralControl("<legend class=""Legxt"" style=""font-size: 16px !important;""><B>Measurements / Demographics</B></legend>"))

                PlaceMedEx.Controls.Add(New LiteralControl("<Table style=""width: 90%;"">"))

                PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""40%"" collapse=""2"">"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Height(cm): </td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtnHeight.Text + """></input></td>"))

                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > BMI: </td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtbmi.Text + """>(kg/m2)</input></td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))



                PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""40%"" collapse=""2"">"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Weight(kg): </td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtnWeight.Text + """></input></td>"))

                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Marital Status: </td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + ddlcMaritalStatus.SelectedItem.Text.ToString() + """></input></td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))


                PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""40%"" collapse=""2"">"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Food Habit: </td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + ddlcFoodHabit.SelectedItem.Text.ToString() + """></input></td>"))

                Dim bolldgroup As String = ""
                If (ddlcRh.SelectedItem.Text.ToString() = "+Ve") Then
                    bolldgroup = "Positive"
                ElseIf (ddlcRh.SelectedItem.Text.ToString() = "-Ve") Then
                    bolldgroup = "Negative"
                Else
                    bolldgroup = ""

                End If

                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Blood Group: </td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + ddlcBloodGroup.SelectedItem.Text.ToString() + "   " + bolldgroup.ToString() + """></input></td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))


                PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""40%"" collapse=""2"">"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Occupation: </td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtvOccupation.Text + """></input></td>"))

                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Educational Qualification: </td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtvEducationQualification.Text + """></input></td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""40%"" collapse=""2"">"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Remarks: </td>"))
                ' PlaceMedEx.Controls.Add(New LiteralControl("<td><textarea rows=""10"" cols=""30"" style=""text-align: left; overflow:auto"" > " + txtvDemographicRemarks.Text + " </textarea ></td>"))

                If (String.IsNullOrEmpty(txtvDemographicRemarks.Text)) Then
                    'PlaceMedEx.Controls.Add(New LiteralControl("<td><textarea rows=""10"" cols=""30"" style=""text-align: left; border:1px dotted  gray; overflow:auto"" > " + Language.ToString() + "</textarea ></td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtvDemographicRemarks.Text + """></input></td>"))
                Else
                    PlaceMedEx.Controls.Add(New LiteralControl("<td style=""Width:150px; word-break: break-all; border:1px dotted  gray; ""> " + txtvDemographicRemarks.Text + "</td>"))
                End If


                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Population :	 </td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + ddlvPopulation.SelectedItem.Text.ToString() + """></input></td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                'PlaceMedEx.Controls.Add(New LiteralControl("<td style=""Width:150px; word-break: break-all; border:1px dotted  gray; ""> " + txtvDemographicRemarks.Text + "</td>"))


                PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                PlaceMedEx.Controls.Add(New LiteralControl("</Table>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</fieldset>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</td>"))


                PlaceMedEx.Controls.Add(New LiteralControl("<td>"))


                PlaceMedEx.Controls.Add(New LiteralControl("<fieldset id=""fICf"" class=""FieldSetBox"" style=""text-align: left; width: 90%; Height:auto;"">"))
                PlaceMedEx.Controls.Add(New LiteralControl("<legend class=""Leg"" style=""font-size: 16px !important;""><B>ICF Required In Language</B></legend>"))



                PlaceMedEx.Controls.Add(New LiteralControl("<Table style= ""  font-family: ""Times New Roman"""">"))
                PlaceMedEx.Controls.Add(New LiteralControl("<Tr>"))

                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Language </td>"))
                ' PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left;"" Value=""" + Language.ToString() + """></input></td>"))
                If (String.IsNullOrEmpty(Language.ToString())) Then
                    'PlaceMedEx.Controls.Add(New LiteralControl("<td><textarea rows=""10"" cols=""30"" style=""text-align: left; border:1px dotted  gray; overflow:auto"" > " + Language.ToString() + "</textarea ></td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + Language.ToString() + """></input></td>"))
                Else
                    PlaceMedEx.Controls.Add(New LiteralControl("<td style=""Width:150px; word-break: break-all; height:auto; border:1px dotted  gray; ""> " + Language.ToString() + "</td>"))
                End If


                PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</Table>"))

                PlaceMedEx.Controls.Add(New LiteralControl("</ BR>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</BR>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</BR>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</BR>"))

                PlaceMedEx.Controls.Add(New LiteralControl("</fieldset>"))

                PlaceMedEx.Controls.Add(New LiteralControl("</fieldset>"))

                PlaceMedEx.Controls.Add(New LiteralControl("</td>"))

                PlaceMedEx.Controls.Add(New LiteralControl("</tr>"))





                PlaceMedEx.Controls.Add(New LiteralControl("</Table>"))

                PlaceMedEx.Controls.Add(New LiteralControl("</fieldset>"))



                PlaceMedEx.Controls.Add(New LiteralControl("<fieldset id=""fMt"" class=""FieldSetBox"" style=""text-align: left;  width: 90%;"">"))
                PlaceMedEx.Controls.Add(New LiteralControl("<legend class=""Let"" style=""font-size: 16px !important;""><B>Attachment Details/Smoking/chewing/alcohol History</B></legend>"))



                PlaceMedEx.Controls.Add(New LiteralControl("<Table width: 100%; Style=""font-family: ""Times New Roman"""">"))

                PlaceMedEx.Controls.Add(New LiteralControl("<Tr collapse=""2"" >"))
                PlaceMedEx.Controls.Add(New LiteralControl("<Td>"))

                PlaceMedEx.Controls.Add(New LiteralControl("<fieldset id=""fPerIfo"" class=""FietBox"" style=""text-align: left; Height: 300px; width: 90%;"">"))
                PlaceMedEx.Controls.Add(New LiteralControl("<legend class=""LegendText"" style=""font-size: 16px !important; ""><B>Attachment Details</B></legend>"))

                PlaceMedEx.Controls.Add(New LiteralControl("<Table style=""width: 100%; border:1px solid Black; "">"))
                Dim dtSUbjectProof As DataTable
                If (GVSubjectProof.Rows.Count > 0) Then
                    dtSUbjectProof = GVSubjectProof.DataSource()
                End If

                Dim dtHabit As DataTable = GVHabits.DataSource()

                Try
                    If (dtSUbjectProof.Rows.Count > 0) Then
                        dtSUbjectProof.DefaultView.RowFilter = "vProofType <> ''"
                    End If

                    dtSUbjectProof.DefaultView.ToTable().AcceptChanges()
                    If dtSUbjectProof.DefaultView.ToTable().Rows.Count > 0 Then

                        For Each dr As DataRow In dtSUbjectProof.DefaultView.ToTable().Rows

                            Dim Attachment = dr(4).ToString.Split("/")
                            PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""40%"" style=""border:1px solid Black;"" cellspacing=2 cellpadding = 0  collapse=""2"">"))
                            Try
                                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left; border: 1px solid black; "" > " + dr(3) + "</td>"))
                            Catch ex As Exception
                            End Try
                            Try
                                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left; border: 1px solid black; "" >  " + Attachment(5) + "</td>"))
                            Catch ex As Exception
                            End Try
                            PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                        Next
                    End If
                Catch ex As Exception

                End Try




                PlaceMedEx.Controls.Add(New LiteralControl("</Table>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</fieldset>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))




                PlaceMedEx.Controls.Add(New LiteralControl("</br>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</br>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</br>"))

                PlaceMedEx.Controls.Add(New LiteralControl("<Td>"))


                PlaceMedEx.Controls.Add(New LiteralControl("<fieldset id=""fPeo"" class=""FieldSetBox"" style=""text-align: left; margin-left: 20px;  Height: 300px; width: 80%;"">"))
                PlaceMedEx.Controls.Add(New LiteralControl("<legend class=""Let"" style=""font-size: 16px !important;""><B>Smoking/chewing/alcohol History</B></legend>"))


                PlaceMedEx.Controls.Add(New LiteralControl("<Table style=""width: 90%; border: 1px solid black; ""cellspacing=0 cellpadding = 0 >"))
                PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""100%""  collapse=""4"">"))

                PlaceMedEx.Controls.Add(New LiteralControl("<td width=""30%"" style=""text-align: center; border: 1px solid black; "" > Habit Details</td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td width=""20%"" style=""text-align: center; border: 1px solid black;"" > Habit  </td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td width=""30%"" style=""text-align: center; border: 1px solid black;"" >Consumption Detail </td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td width=""20%"" style=""text-align: center; border: 1px solid black;"" >If Previous, stopped since</td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</BR>"))
                For Each dr As DataRow In dtHabit.Rows
                    PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""100%"" collapse=""4"">"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<td width=""30%"" style=""text-align: left;border: 1px solid black; "" > " + dr(6).ToString() + "</td>"))
                    Dim Flag As String
                    If (dr(5).ToString().ToUpper() = "C") Then
                        Flag = "Current"
                    ElseIf (dr(5).ToString().ToUpper() = "P") Then
                        Flag = "Previous"
                    Else
                        Flag = "Never"
                    End If
                    PlaceMedEx.Controls.Add(New LiteralControl("<td width=""20%"" style=""text-align: left; border: 1px solid black;"" > " + Flag + "</td>"))
                    If Not (dr(8).ToString() = "") Then
                        PlaceMedEx.Controls.Add(New LiteralControl("<td width=""30%"" style=""text-align: left; border: 1px solid black;"" > " + dr(8).ToString() + "</td>"))
                    Else
                        PlaceMedEx.Controls.Add(New LiteralControl("<td width=""30%"" style=""text-align: left; border: 1px solid black;"" > " + "--" + "</td>"))
                    End If

                    If Not (dr(7).ToString() = "") Then
                        PlaceMedEx.Controls.Add(New LiteralControl("<td width=""30%"" style=""text-align: left; border: 1px solid black;"" > " + dr(7).ToString() + "</td>"))
                    Else
                        PlaceMedEx.Controls.Add(New LiteralControl("<td width=""30%"" style=""text-align: left; border: 1px solid black;"" > " + "  -- " + "</td>"))
                    End If


                    ' PlaceMedEx.Controls.Add(New LiteralControl("<td width=""20%"" style=""text-align: left; border: 1px solid black;"" > " + dr(7).ToString() + "</td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
                Next

                PlaceMedEx.Controls.Add(New LiteralControl("</Table>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</fieldset>"))

                PlaceMedEx.Controls.Add(New LiteralControl("</Td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
                'PlaceMedEx.Controls.Add(New LiteralControl("</BR>"))
                'PlaceMedEx.Controls.Add(New LiteralControl("</BR>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</Table>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</fieldset>"))




                '''''''''''''''''''''''''''''''Complete '''''''''''''''''''''''''''''''''''''''''''


                If (ddlcSex.SelectedValue = "F") Then
                    PlaceMedEx.Controls.Add(New LiteralControl("</BR>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</BR>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</BR>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</BR>"))

                    PlaceMedEx.Controls.Add(New LiteralControl("<fieldset id=""Ifo"" class=""FieldSetBox"" style=""text-align: left; page-break-before: always; width: 90%;"">"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<legend class=""Lext"" style=""font-size: 16px !important; ""><B>Female Information</B></legend>"))

                    PlaceMedEx.Controls.Add(New LiteralControl("</BR>"))

                    PlaceMedEx.Controls.Add(New LiteralControl("<fieldset id=""fo"" class=""FieldSetBox"" style=""text-align: left; margin:20px 20px 20px 20px; width: 90%;"">"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<legend class=""Lt"" style=""font-size: 16px !important; ""><B>Last Menstrual Period\Family Planning Measures</B></legend>"))

                    PlaceMedEx.Controls.Add(New LiteralControl("<Table style=""width: 90%; font-family: ""Times New Roman"" "">"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""40%"" collapse=""2"">"))

                    Try
                        PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Date:	 </td>"))
                        PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left;"" Value=""" + txtdLastMenstrualDate.Text + """></input></td>"))

                        PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Family Planning Measures: </td>"))
                        If (String.IsNullOrEmpty(rblcContraception.SelectedValue.ToString)) Then
                            PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left;"" Value=""" + rblcContraception.SelectedValue.ToString + """></input></td>"))
                        Else
                            PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left;"" Value=""" + rblcContraception.SelectedItem.ToString + """></input></td>"))
                        End If


                    Catch ex As Exception
                        PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left;"" Value=""></input></td>"))
                    End Try


                    PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                    PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""40%"" collapse=""2"">"))


                    PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Cycle Length:	 </td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left;"" Value=""" + txtiLastMenstrualDays.Text + """></input></td>"))

                    Try
                        PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Details of Contraception: </td>"))
                        Dim chk As String = ""
                        For index = 0 To chkcContraception.Items.Count - 1
                            If chkcContraception.Items(index).Selected = True Then
                                If chkcContraception.Items(index).Value.ToUpper.Trim() = "B" Then
                                    chk = "Double Barrier"
                                ElseIf chkcContraception.Items(index).Value.ToUpper.Trim() = "P" Then
                                    chk += "   Pills"
                                ElseIf chkcContraception.Items(index).Value.ToUpper.Trim() = "R" Then
                                    chk += "    Rhythm"
                                ElseIf chkcContraception.Items(index).Value.ToUpper.Trim() = "I" Then
                                    chk += "    IUCD"
                                End If
                            End If
                        Next index

                        If (String.IsNullOrEmpty(chk.ToString)) Then
                            PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" TextMode=""MultiLine"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + chk.ToString + """></input></td>"))
                        Else
                            PlaceMedEx.Controls.Add(New LiteralControl("<td style=""Width:150px; word-break: break-all; border:1px dotted  gray; ""> " + chk.ToString + "</td>"))
                        End If

                        'If (String.IsNullOrEmpty() Then
                        '    PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + chk.ToString + """></input></td>"))
                        'Else
                        '    PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + chk.ToString + """></input></td>"))
                        'End If

                        'PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left;"" Value=""" + chk.ToString + """></input></td>"))
                    Catch ex As Exception
                        PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left;"" Value=""></input></td>"))
                    End Try

                    PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""40%"" collapse=""2"">"))

                    Try
                        PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Regular:	 </td>"))
                        If (String.IsNullOrEmpty(rblcRegular.SelectedValue.ToString)) Then
                            PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + rblcRegular.SelectedValue.ToString + """></input></td>"))
                        Else
                            PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + rblcRegular.SelectedItem.ToString + """></input></td>"))
                        End If

                    Catch ex As Exception
                        PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left;"" Value=""   ""></input></td>"))
                    End Try

                    PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Remarks : </td>"))
                    ' PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left;"" Value=""" + txtvRemarks.Text + """></input></td>"))

                    If (txtvOtherRemark.Text = "") Then
                        'PlaceMedEx.Controls.Add(New LiteralControl("<td><textarea rows=""10"" cols=""30"" style=""text-align: left; overflow:auto"" > " + txtvOtherRemark.Text + " </textarea ></td>"))
                        PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtvOtherRemark.Text + """></input></td>"))
                    Else
                        PlaceMedEx.Controls.Add(New LiteralControl("<td style=""Width:150px; word-break: break-all; border:1px dotted  gray; ""> " + txtvOtherRemark.Text + "</td>"))
                    End If





                    PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                    PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""40%"">"))

                    Try

                        PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Association with Pain:	 </td>"))
                        If (String.IsNullOrEmpty(rblcLastMenstrualIndi.SelectedValue.ToString)) Then
                            PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + rblcLastMenstrualIndi.SelectedValue.ToString + """></input></td>"))
                        Else
                            PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + rblcLastMenstrualIndi.SelectedItem.ToString + """></input></td>"))
                        End If

                    Catch ex As Exception
                        PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left;"" Value=""   ""></input></td>"))
                    End Try
                    PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))


                    PlaceMedEx.Controls.Add(New LiteralControl("</Table>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</fieldset>"))


                    PlaceMedEx.Controls.Add(New LiteralControl("</BR>"))

                    PlaceMedEx.Controls.Add(New LiteralControl("<fieldset id=""fo"" class=""Fieox"" style=""text-align: left;margin:20px 20px 20px 20px; width: 90%;"">"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<legend class=""Legxt"" style=""font-size: 16px !important; margin:20px 20px 20px 20px; ""><B>Last Obstetric History</B></legend>"))

                    PlaceMedEx.Controls.Add(New LiteralControl("<Table style=""width: 90%; font-family: ""Times New Roman"" "">"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""40%"" collapse=""2"">"))


                    PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Date of Last Delivery: </td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtdLastDelivaryDate.Text + """></input></td>"))

                    PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Gravida: </td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtvGravida.Text + """></input></td>"))


                    PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                    PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""40%"" collapse=""2"">"))


                    PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > No.of Live Children: </td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtiNoOfChildren.Text + """></input></td>"))

                    PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Para: </td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left;"" Value=""" + txtvPara.Text + """></input></td>"))


                    PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                    PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""40%"" collapse=""2"">"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > No. of children died </td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtiNoOfChildrenDied.Text + """></input></td>"))

                    PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Remarks if children died: </td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtvRemarkifDied.Text + """></input></td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))


                    PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""40%"" collapse=""2"">"))
                    Try
                        PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > All Children Healthy:	 </td>"))
                        If (String.IsNullOrEmpty(rblcChildrenHelath.SelectedValue.ToString)) Then
                            PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + rblcChildrenHelath.SelectedValue.ToString + """></input></td>"))
                        Else
                            PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + rblcChildrenHelath.SelectedItem.ToString + """></input></td>"))
                        End If

                    Catch ex As Exception
                        PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left;"" Value="" ""></input></td>"))
                    End Try

                    PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Remarks: </td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtvchildrenHealthRemark.Text + """></input></td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))


                    PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""40%"" collapse=""2"">"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Any Spontaneous Abortion/MTP : </td>"))
                    Try
                        If (String.IsNullOrEmpty(rblcAbortions.SelectedValue.ToString)) Then
                            PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + rblcAbortions.SelectedValue.ToString + """></input></td>"))
                        Else
                            PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + rblcAbortions.SelectedItem.ToString + """></input></td>"))
                        End If

                    Catch ex As Exception
                        PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""  ""></input></td>"))
                    End Try

                    PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Date Of Last Abortion: </td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtdAbortionDate.Text + """></input></td>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))


                    PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""40%"" collapse=""2"">"))
                    PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Lactating: </td>"))

                    Try
                        If (String.IsNullOrEmpty(rblcLoctating.SelectedValue.ToString)) Then
                            PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + rblcLoctating.SelectedValue.ToString + """></input></td>"))
                        Else
                            PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + rblcLoctating.SelectedItem.ToString + """></input></td>"))
                        End If

                    Catch ex As Exception
                        PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""  ""></input></td>"))
                    End Try
                    PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Volunteer is in the child bearing age: </td>"))
                    Try
                        If (String.IsNullOrEmpty(rblcIsVolunteerinBearingAge.SelectedValue.ToString)) Then
                            PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + rblcIsVolunteerinBearingAge.SelectedValue.ToString + """></input></td>"))
                        Else
                            PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + rblcIsVolunteerinBearingAge.SelectedItem.ToString + """></input></td>"))
                        End If

                        PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
                    Catch ex As Exception
                        PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""  ""></input></td>"))
                    End Try

                    PlaceMedEx.Controls.Add(New LiteralControl("</Table>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</fieldset>"))

                    PlaceMedEx.Controls.Add(New LiteralControl("</BR>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</BR>"))

                    PlaceMedEx.Controls.Add(New LiteralControl("</fieldset>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</BR>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</BR>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</BR>"))
                    PlaceMedEx.Controls.Add(New LiteralControl("</BR>"))
                End If


                ''''''''''''''''''''''''''Complete''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


                PlaceMedEx.Controls.Add(New LiteralControl("<fieldset id=""fPeIfo"" class=""FieldSetBox"" style=""text-align: left; page-break-before: always; width: 90%;"">"))
                PlaceMedEx.Controls.Add(New LiteralControl("<legend class=""Legxt"" style=""font-size: 16px !important;  ""><B>Contact Information</B></legend>"))


                PlaceMedEx.Controls.Add(New LiteralControl("</BR>"))

                PlaceMedEx.Controls.Add(New LiteralControl("<fieldset id=""fPIfo"" class=""FieldSetBox"" style=""text-align: left; margin:20px 20px 20px 20px; width: 90%;"">"))
                PlaceMedEx.Controls.Add(New LiteralControl("<legend class=""LeText"" style=""font-size: 16px !important; margin:20px 20px 20px 20px; ""><B>Subject Contact Detail</B></legend>"))


                PlaceMedEx.Controls.Add(New LiteralControl("<Table style=""width: 90%; font-family: ""Times New Roman"" "">"))
                PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""40%"" collapse=""2"">"))


                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Local Address(1):	 </td>"))
                ' PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; Height=100px; Width=250px;"" Value=""" + txtvLocalAdd1.Text + """></input></td>"))
                ' PlaceMedEx.Controls.Add(New LiteralControl("<td><textarea rows=""10"" cols=""30"" style=""text-align: left; overflow:auto "" > " + txtvLocalAdd1.Text + " </textarea ></td>"))
                If (txtvLocalAdd1.Text = "") Then
                    PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtvLocalAdd1.Text + """></input></td>"))
                Else
                    PlaceMedEx.Controls.Add(New LiteralControl("<td style=""Width:150px; word-break: break-all; border:1px dotted  gray; ""> " + txtvLocalAdd1.Text + "</td>"))
                End If

                'PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                'PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""20%"" >"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Local Address(2): </td>"))
                'PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" TextMode=""MultiLine"" style=""text-align: left;"" Value=""" + txtvLocalAdd21.Text + """></input></td>"))
                'PlaceMedEx.Controls.Add(New LiteralControl("<td><textarea rows=""10"" cols=""30"" style=""text-align: left; overflow:auto"" > " + txtvLocalAdd21.Text + "</textarea ></td>"))
                If (String.IsNullOrEmpty(txtvLocalAdd21.Text)) Then
                    PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" TextMode=""MultiLine"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtvLocalAdd21.Text + """></input></td>"))
                Else
                    PlaceMedEx.Controls.Add(New LiteralControl("<td style=""Width:150px; word-break: break-all; border:1px dotted  gray; ""> " + txtvLocalAdd21.Text + "</td>"))
                End If


                PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))


                PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""40%"" collapse=""2"">"))


                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Local Tel1 No: </td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtvLocalTelephoneno1.Text + """></input></td>"))
                'PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))

                'PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""20%"" >"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Local Tel2 No:	 </td>"))
                If (String.IsNullOrEmpty(txtvLocalAdd21.Text)) Then
                    PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtvLocalTelephoneno2.Text + """></input></td>"))
                Else
                    PlaceMedEx.Controls.Add(New LiteralControl("<td style=""Width:150px; word-break: break-all; border:1px dotted  gray; ""> " + txtvLocalTelephoneno2.Text + "</td>"))
                End If



                PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))


                PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""40%"" collapse=""2"">"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Permanent Address: </td>"))
                ' PlaceMedEx.Controls.Add(New LiteralControl("<td><textarea rows=""10"" cols=""30"" style=""text-align: left; overflow:auto"" > " + txtvPerAdd1.Text + "</textarea ></td>"))

                If (String.IsNullOrEmpty(txtvPerAdd1.Text)) Then
                    PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtvPerAdd1.Text + """></input></td>"))
                Else
                    PlaceMedEx.Controls.Add(New LiteralControl("<td style=""Width:150px; word-break: break-all; border:1px dotted  gray; ""> " + txtvPerAdd1.Text + "</td>"))
                    ' PlaceMedEx.Controls.Add(New LiteralControl("<td><textarea rows=""10"" cols=""30"" style=""text-align: left; overflow:auto"" > " + txtvPerAdd1.Text + "</textarea ></td>"))
                End If


                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Permanent Tel No:</td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtvPerTelephoneno.Text + """></input></td>"))

                PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))



                PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))


                PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""40%"" collapse=""2"">"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Office Address:</td>"))


                If (txtvOfficeAddress.Text = "") Then
                    PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtvOfficeAddress.Text + """></input></td>"))
                Else
                    PlaceMedEx.Controls.Add(New LiteralControl("<td style=""Width:150px; word-break: break-all; border:1px dotted  gray; ""> " + txtvOfficeAddress.Text + "</td>"))
                End If

                ' PlaceMedEx.Controls.Add(New LiteralControl("<td><textarea rows=""10"" cols=""30"" style=""text-align: left; overflow:auto"" > " + txtvOfficeAddress.Text + "</textarea ></td>"))
                'If (String.IsNullOrEmpty(txtvOfficeAddress.Text)) Then
                '    PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; font-family: ""Times New Roman"" border:1px dotted  gray;"" Value=""" + txtvOfficeAddress.Text + """></input></td>"))
                'Else
                '    PlaceMedEx.Controls.Add(New LiteralControl("<td style=""Width:150px; word-break: break-all; font-family: ""Times New Roman"" border:1px dotted  gray; ""> " + txtvOfficeAddress.Text + "</td>"))
                'End If



                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Office Tel No:</td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtvOfficeTelephoneno.Text + """></input></td>"))

                PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))



                PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""40%"">"))

                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Place: </td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + Convert.ToString(Me.ddlvPerCity.SelectedItem.Text) + """></input></td>"))

                PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))



                PlaceMedEx.Controls.Add(New LiteralControl("</Table>"))

                PlaceMedEx.Controls.Add(New LiteralControl("</fieldset>"))

                PlaceMedEx.Controls.Add(New LiteralControl("</BR>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</BR>"))


                PlaceMedEx.Controls.Add(New LiteralControl("<fieldset id=""fPefo"" class=""FieldSetBox"" style=""text-align: left; margin:20px 20px 20px 20px; width: 90%;"">"))
                PlaceMedEx.Controls.Add(New LiteralControl("<legend class=""LeTet"" style=""font-size: 16px !important; margin:20px 20px 2Enroll0px 20px;""><B>Contact Person Contact Detail</B></legend>"))

                PlaceMedEx.Controls.Add(New LiteralControl("<Table style=""width: 90%; font-family: ""Times New Roman"""">"))


                PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""40%"" collapse=""2"">"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Person Name(1):</td>"))

                ' PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtvContactName1.Text + """></input></td>"))
                If (String.IsNullOrEmpty(txtvContactName1.Text)) Then
                    PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtvContactName1.Text + """></input></td>"))
                Else
                    PlaceMedEx.Controls.Add(New LiteralControl("<td style=""Width:150px; word-break: break-all; border:1px dotted  gray; ""> " + txtvContactName1.Text + "</td>"))
                End If



                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Person Name(2):</td>"))

                If (String.IsNullOrEmpty(txtvContactName2.Text)) Then
                    PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtvContactName2.Text + """></input></td>"))
                Else
                    PlaceMedEx.Controls.Add(New LiteralControl("<td style=""Width:150px; word-break: break-all; border:1px dotted  gray; ""> " + txtvContactName2.Text + "</td>"))
                End If

                'PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left;border:1px dotted  gray;"" Value=""" + txtvContactName2.Text + """></input></td>"))

                PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))


                PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""40%"" collapse=""2"">"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Person Adds.(1): </td>"))

                ' PlaceMedEx.Controls.Add(New LiteralControl("<td><textarea rows=""10"" cols=""30"" style=""text-align: left; overflow:auto"" > " + txtvContactAddress11.Text + "</textarea ></td>"))

                If (String.IsNullOrEmpty(txtvContactAddress11.Text)) Then
                    PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtvContactAddress11.Text + """></input></td>"))
                Else
                    PlaceMedEx.Controls.Add(New LiteralControl("<td style=""Width:150px; word-break: break-all; border:1px dotted  gray; ""> " + txtvContactAddress11.Text + "</td>"))
                End If


                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Person Add(2): </td>"))

                ' PlaceMedEx.Controls.Add(New LiteralControl("<td><textarea rows=""10"" cols=""30"" style=""text-align: left; overflow:auto"" > " + txtvContactAddress21.Text + "</textarea ></td>"))
                If (String.IsNullOrEmpty(txtvContactAddress21.Text)) Then

                    PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtvContactAddress21.Text + """></input></td>"))
                    'PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; 33"" Value=""" + txtvContactAddress21.Text + """></input></td>"))
                Else
                    PlaceMedEx.Controls.Add(New LiteralControl("<td style=""Width:150px; word-break: break-all; border:1px dotted  gray; ""> " + txtvContactAddress21.Text + "</td>"))
                    'PlaceMedEx.Controls.Add(New LiteralControl("<td style=""Width:150px; word-break: break-all; border:1px dotted  gray; ""> " + txtvContactAddress21.Text + "</td>"))
                End If


                PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))


                PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""40%"" collapse=""2"">"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Person Tel No(1): </td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtvContactTelephoneno1.Text + """></input></td>"))

                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Person Tel No(2):	 </td>"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtvContactTelephoneno2.Text + """></input></td>"))

                PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))


                PlaceMedEx.Controls.Add(New LiteralControl("<Tr width=""40%"" collapse=""2"">"))
                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Referred By: </td>"))

                ' PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtvReferredBy.Text + """></input></td>"))

                If (String.IsNullOrEmpty(txtvReferredBy.Text)) Then
                    PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; display:none; border:1px dotted  gray;"" Value=""" + txtvReferredBy.Text + """></input></td>"))
                Else
                    PlaceMedEx.Controls.Add(New LiteralControl("<td style=""Width:150px; word-break: break-all; display:none;  border:1px dotted  gray; ""> " + txtvReferredBy.Text + "</td>"))
                End If



                PlaceMedEx.Controls.Add(New LiteralControl("<td style=""text-align: left;"" > Remarks: </td>"))

                ' PlaceMedEx.Controls.Add(New LiteralControl("<td><textarea rows=""10"" cols=""30"" style=""text-align: left; overflow:auto; "" > " + txtvRemarks.Text + "</textarea ></td>"))
                If (String.IsNullOrEmpty(txtvRemarks.Text)) Then
                    PlaceMedEx.Controls.Add(New LiteralControl("<td><input type=""text"" style=""text-align: left; border:1px dotted  gray;"" Value=""" + txtvRemarks.Text + """></input></td>"))
                Else
                    PlaceMedEx.Controls.Add(New LiteralControl("<td style=""Width:150px; word-break: break-all; border:1px dotted  gray; ""> " + txtvRemarks.Text + "</td>"))
                End If



                PlaceMedEx.Controls.Add(New LiteralControl("</Tr>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</BR>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</BR>"))
                PlaceMedEx.Controls.Add(New LiteralControl("</Table>"))

                PlaceMedEx.Controls.Add(New LiteralControl("</fieldset>"))





            Catch ex As Exception

            End Try

            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function


    Public Function ConvertBytesToImageFile(ByVal ImageData As Byte(), ByVal FilePath As String) As Boolean
        If IsNothing(ImageData) = True Then
            Return False
            'Throw New ArgumentNullException("Image Binary Data Cannot be Null or Empty", "ImageData")
        End If
        Try
            Dim fs As IO.FileStream = New IO.FileStream(FilePath, IO.FileMode.OpenOrCreate, IO.FileAccess.Write)
            Dim bw As IO.BinaryWriter = New IO.BinaryWriter(fs)

            bw.Write(ImageData)
            bw.Flush()
            bw.Close()
            fs.Close()
            bw = Nothing
            fs.Dispose()

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function


#Region "Conversion Code"

    Private Function BytesToBmp_Serialized(ByVal bmpBytes As Byte()) As System.Drawing.Bitmap
        Dim bf As New BinaryFormatter()
        ' copy the bytes to the memory
        Dim ms As New MemoryStream(bmpBytes)

        Try
            Return DirectCast(bf.Deserialize(ms), System.Drawing.Bitmap)

        Catch ex As Exception
            'MessageBox.Show(ex.Message)
        End Try

    End Function

    Private Function BmpToBytes_Serialization(ByVal bmp As System.Drawing.Bitmap) As Byte()
        ' stream to save the bitmap to
        Dim ms As New MemoryStream()
        Dim bf As New BinaryFormatter()

        Try
            bf.Serialize(ms, bmp)
            ' read to end
            Dim bmpBytes As Byte() = ms.GetBuffer()
            bmp.Dispose()
            ms.Close()
            Return bmpBytes

        Catch ex As Exception
            'MessageBox.Show(ex.Message)
        End Try

    End Function

#End Region

End Class
