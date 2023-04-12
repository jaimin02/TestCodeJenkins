'Imports Microsoft.Win32
'Imports System.Web.Services
'Imports Newtonsoft.Json
'Imports Microsoft
'Imports Microsoft.Office.Interop
'Imports System.IO
'Imports System.Drawing

Partial Class frmNoticeMst
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION "

    Private objcommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private objLambda As WS_Lambda.WS_Lambda = objcommon.GetHelpDbLambdaRef()

    Private Const Vs_Choice As String = "Choice"
    Private Const VS_DtNoticeMst As String = "DtNoticeMst"
    Private Const Vs_NoticeNo As String = "nNoticeNo"
    Private Const VS_NoticeMst As String = "NoticeMst"



    Private rPage As RepoPage
    Private index1 As Integer = 0

    Private Const GVC_SrNo As Integer = 0
    Private Const GVC_nNoticeNo As Integer = 1
    Private Const GVC_UserTypeName As Integer = 2
    Private Const GVC_Subject As Integer = 3
    Private Const GVC_Description As Integer = 4
    Private Const GVC_FromDate As Integer = 5
    Private Const GVC_ToDate As Integer = 6
    Private Const GVC_Edit As Integer = 7
    Private Const GVC_Delete As Integer = 8

    Private WithClientContact As Boolean = True
#End Region

#Region "Page Load"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.Page.Form.Enctype = "multipart/form-data"
            If Not IsPostBack Then
                GenCall()
                CalExtFromDateForNoticeMst.SelectedDate = Date.Today()
                CalExtToDateForNoticeMst.SelectedDate = Date.Today()
            End If
        Catch ex As Exception
            'ShowErrorMessage(Me.Master, ex)
        End Try
    End Sub
#End Region

#Region "GenCall()"
    Private Function GenCall() As Boolean
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim dt_NoticeMst As DataTable = Nothing
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum

        Try
            Choice = Me.Request.QueryString("mode").ToString
            Me.ViewState(Vs_Choice) = Choice

            If Choice <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.ViewState(Vs_NoticeNo) = Me.Request.QueryString("value").ToString
            End If

            If Not GenCall_Data(Choice, dt_NoticeMst) Then
                Exit Function
            End If

            Me.ViewState(VS_DtNoticeMst) = dt_NoticeMst

            If Not GenCall_ShowUI(Choice, dt_NoticeMst) Then
                Exit Function
            End If

            GenCall = True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "..GenCall")
        End Try
    End Function
#End Region

#Region "Gencall Data"
    Private Function GenCall_Data(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                            ByRef dt_Dist_Retu As DataTable) As Boolean

        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim ds_Noticemst As DataSet = Nothing

        Try
            If Choice_1 = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                wStr = "1=2"
            Else
                wStr = "nNoticeNo=" + Me.ViewState(Vs_NoticeNo).ToString()
            End If

            wStr += " And cStatusIndi <> 'D'"

            If Not objHelp.get_ViewNoticeMst(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Noticemst, eStr) Then
                Throw New Exception(eStr)
            End If

            If ds_Noticemst Is Nothing Then
                Throw New Exception(eStr)
            End If

            If ds_Noticemst.Tables(0).Rows.Count <= 0 And _
               Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Throw New Exception("No Records Found for Selected role")
            End If

            dt_Dist_Retu = ds_Noticemst.Tables(0)
            GenCall_Data = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_Data")
        End Try
    End Function
#End Region

#Region "Gencall showUI"
    Private Function GenCall_ShowUI(ByVal Choice_1 As WS_Lambda.DataObjOpenSaveModeEnum, _
                                      ByVal dt_NoticeMst As DataTable) As Boolean
        Try

            Page.Title = " :: Information Management ::  " + System.Configuration.ConfigurationManager.AppSettings("Client")
            Me.AutoCompleteExtender1.ContextKey = "iUserId = " & Me.Session(S_UserID) & " And cWorkspaceType='P'"
            CType(Master.FindControl("lblHeading"), Label).Text = "Information Management"

            If Not FillGridClient() Then
                Return False
            End If

            If Not FillDropDown() Then
                Return False
            End If


            If Choice_1 <> WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                Me.txtSub.Text = ConvertDbNullToDbTypeDefaultValue(dt_NoticeMst.Rows(0)("vSubject"), dt_NoticeMst.Rows(0)("vsubject").GetType)
                Me.Editor1.Text = ConvertDbNullToDbTypeDefaultValue(dt_NoticeMst.Rows(0)("vDescription"), dt_NoticeMst.Rows(0)("vdescription").GetType)
                Me.txtFromDate.Text = Format(ConvertDbNullToDbTypeDefaultValue(dt_NoticeMst.Rows(0)("dStartDate"), dt_NoticeMst.Rows(0)("dStartDate").GetType))
                Me.txtToDate.Text = Format(ConvertDbNullToDbTypeDefaultValue(dt_NoticeMst.Rows(0)("dEndDate"), dt_NoticeMst.Rows(0)("dEndDate").GetType))
                Me.hdnUserTypeCode.Value = Format(ConvertDbNullToDbTypeDefaultValue(dt_NoticeMst.Rows(0)("vUserTypeCode"), dt_NoticeMst.Rows(0)("vUserTypeCode").GetType))
                Me.HProjectId.Value = Format(ConvertDbNullToDbTypeDefaultValue(dt_NoticeMst.Rows(0)("vParentWorkspaceId"), dt_NoticeMst.Rows(0)("vParentWorkspaceId").GetType))
                Me.txtProject.Text = Format(ConvertDbNullToDbTypeDefaultValue(dt_NoticeMst.Rows(0)("vProjectName"), dt_NoticeMst.Rows(0)("vProjectName").GetType))
                Me.btnSave.Text = "Update"
                Me.btnSave.ToolTip = "Update"
            End If

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....GenCall_ShowUI")
        Finally
        End Try
    End Function
#End Region

#Region "Fill Functions"
    Private Function FillDropDown() As Boolean
        Dim dsProfile As New DataSet
        Dim eStr_Retu As String = String.Empty
        Dim dvProfile As New DataView
        Dim wStr As String = String.Empty

        Try
            objHelp.getUserTypeMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, dsProfile, eStr_Retu)

            dvProfile = dsProfile.Tables(0).DefaultView
            dvProfile.Sort = "vUserTypeName"
            ddlProfile.Items.Clear()
            Me.ddlProfile.DataSource = dvProfile
            Me.ddlProfile.DataValueField = "vUserTypeCode"
            Me.ddlProfile.DataTextField = "vUserTypeName"
            Me.ddlProfile.DataBind()
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillDropDown")
            Return False
        End Try
    End Function

    Private Function FillGridClient() As Boolean
        Dim eStr As String = String.Empty
        Dim ds_NoticeMst As New DataSet
        Dim dt_notice As New DataTable
        Try

            If Not objHelp.get_ViewNoticeMst("cStatusIndi <> 'D' and iModifyBy = " + Session(S_UserID) + "", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_NoticeMst, eStr) Then
                Throw New Exception(eStr)
            End If

            dt_notice = ds_NoticeMst.Tables(0)
            ViewState(VS_NoticeMst) = dt_notice
            '==============================
            Me.gvnotice.DataSource = ds_NoticeMst
            Me.gvnotice.DataBind()


            If gvnotice.Rows.Count > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "UIgvNotice", "UIgvNotice(); ", True)
            End If
            Return True
        Catch ex As Exception
            ShowErrorMessage(ex.Message, "....FillGridNotice")
            Return False
        End Try
    End Function

    Private Function AssignUpdatedValues(ByVal AttachFileName As String) As Boolean

        Dim dtOld As New DataTable
        Dim dr As DataRow
        Dim ds_Check As New DataSet
        Dim eStr As String = String.Empty
        Dim wStr As String = String.Empty
        Dim StrDescription As String = String.Empty
        Dim userType As String = String.Empty


        Try
            dtOld = Me.ViewState(VS_DtNoticeMst)

            StrDescription = Trim(Me.Editor1.Text)
            StrDescription = Trim(Me.Editor1.Text)
            RemoveTableFromString(StrDescription)
            RemoveRowFromString(StrDescription)
            ReplaceTDFromString(StrDescription)
            RemoveColTBodyFromString(StrDescription)

            If Not Me.objHelp.GetNoticeMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_Empty, ds_Check, eStr) Then
                Throw New Exception(eStr)
            End If

            If Me.ViewState(Vs_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                ds_Check.Clear()
                dr = ds_Check.Tables(0).NewRow
                dr("vUserTypeCode") = Me.ddlProfile.SelectedValue.Trim()
                dr("vSubject") = Me.txtSub.Text
                dr("vDescription") = StrDescription
                dr("dFromDate") = Me.txtFromDate.Text.Trim()
                dr("dToDate") = Me.txtToDate.Text.Trim()
                dr("iModifyBy") = Session(S_UserID)
                dr("vParentWorkspaceId") = Me.HProjectId.Value.Trim()
                If Me.FileUpload1.HasFile Then
                    dr("vAttachment") = AttachFileName
                End If

                userType += Me.hdnUserTypeCode.Value()
                dr("vUserTypeCode") = userType.Trim()
                ds_Check.Tables(0).Rows.Add(dr)

                'End If
            ElseIf Me.ViewState(Vs_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
                ds_Check.Clear()
                dr = ds_Check.Tables(0).NewRow
                dr("nNoticeNo") = Me.ViewState(Vs_NoticeNo).ToString()
                dr("vUserTypeCode") = Me.ddlProfile.SelectedValue.Trim()
                dr("vSubject") = Me.txtSub.Text
                dr("vDescription") = StrDescription
                dr("dFromDate") = Me.txtFromDate.Text.Trim()
                dr("dToDate") = Me.txtToDate.Text.Trim()
                dr("iModifyBy") = Session(S_UserID)
                dr("vParentWorkspaceId") = Me.HProjectId.Value.Trim()

                If Me.FileUpload1.HasFile Then
                    dr("vAttachment") = AttachFileName
                End If

                userType += Me.hdnUserTypeCode.Value()
                dr("vUserTypeCode") = userType.Trim()

                ds_Check.Tables(0).Rows.Add(dr)
                ds_Check.AcceptChanges()
            Else
                ds_Check.Clear()
                dr = ds_Check.Tables(0).NewRow
                dr("nNoticeNo") = Me.gvnotice.Rows(0).Cells(GVC_nNoticeNo).Text.Trim()
                dr("iModifyBy") = Session(S_UserID)
                ds_Check.Tables(0).Rows.Add(dr)
                ds_Check.AcceptChanges()


            End If
            'Next Items

            ds_Check.AcceptChanges()
            Me.ViewState(VS_DtNoticeMst) = ds_Check.Tables(0)

            Return True
        Catch ex As Exception
            'ShowErrorMessage(Me.Master, ex)
        End Try
    End Function

    Private Function UploadAttachment(ByRef fileName As String) As Boolean

        Dim DocValidFile As String = String.Empty
        Dim strValidFile() As String
        Dim DocPath As String = String.Empty
        Dim str As String
        Dim iFileSize As Integer = 0
        Dim iCounter As Integer = 0

        Try

            DocValidFile = System.Configuration.ConfigurationManager.AppSettings("Validity")
            'DocPath = System.Configuration.ConfigurationManager.AppSettings("DocPath")
            DocPath = "~/InformationManagement/"


            If Not IsNothing(FileUpload1) Then

                iFileSize = FileUpload1.PostedFile.ContentLength
                iFileSize = iFileSize / 1024 / 1024
                Dim ConvertGBsize As Double

                ConvertGBsize = (iFileSize / 1024)
                If ConvertGBsize <= 1 Then 'Validity of Size 

                    strValidFile = DocValidFile.Split("#")
                    Me.Session("FileSize") = iFileSize.ToString + "KB"
                    Me.Session("FileName") = FileUpload1.PostedFile.FileName
                    str = Replace(Session("FileName"), "\", "\\")

                    If Not Directory.Exists(Server.MapPath(DocPath.Replace("\", ""))) Then
                        Directory.CreateDirectory(Server.MapPath(DocPath))
                    End If

                    FileUpload1.PostedFile.SaveAs(Server.MapPath(DocPath) & Path.GetFileName(FileUpload1.PostedFile.FileName))

                    fileName = "/InformationManagement/" + Path.GetFileName(FileUpload1.PostedFile.FileName)

                Else
                    objcommon.ShowAlert("Error Occured, File size should be less than 1 GB...", Me)
                    Exit Function
                End If
            End If

            Return True
        Catch ex As Exception
            'ShowErrorMessage(Me.Master, ex)
        End Try
    End Function

    Public Function ConvertDbNullToDbTypeDefaultValue(ByVal Val As Object, ByVal DbType_1 As System.Type) As Object
        Dim DbTypeName_1 As String = ""
        Dim defDateTime As DateTime
        Dim DefChar As Char

        DbTypeName_1 = DbType_1.Name.ToUpper

        If DbTypeName_1 = "STRING" Then

            Try
                ConvertDbNullToDbTypeDefaultValue = Val.ToString
            Catch ex As Exception
                ConvertDbNullToDbTypeDefaultValue = ""
            End Try

        ElseIf DbTypeName_1 = "CHAR" Then

            Try
                ConvertDbNullToDbTypeDefaultValue = Convert.ToChar(Val)
            Catch ex As Exception
                ConvertDbNullToDbTypeDefaultValue = DefChar
            End Try

        ElseIf DbTypeName_1 = "DATETIME" Then

            Try
                ConvertDbNullToDbTypeDefaultValue = Convert.ToDateTime(Val)
            Catch ex As Exception
                ConvertDbNullToDbTypeDefaultValue = defDateTime
            End Try

        ElseIf DbTypeName_1 = "BOOLEAN" Then

            Try
                ConvertDbNullToDbTypeDefaultValue = Convert.ToBoolean(Val)
            Catch ex As Exception
                ConvertDbNullToDbTypeDefaultValue = False
            End Try

        ElseIf DbTypeName_1 = "DECIMAL" Or DbTypeName_1 = "INT16" Or DbTypeName_1 = "INT32" Or _
               DbTypeName_1 = "INT64" Or DbTypeName_1 = "DOUBLE" Or DbTypeName_1 = "BYTE" Then

            If IsDBNull(Val) Then
                ConvertDbNullToDbTypeDefaultValue = 0
            Else
                ConvertDbNullToDbTypeDefaultValue = Val
            End If

        End If
    End Function
#End Region

#Region "Reset Page"
    Private Sub ResetPage()
        Dim dtNotice As DataTable = Nothing
        Try
            Me.Editor1.Text = String.Empty
            Me.txtSub.Text = String.Empty
            Me.txtFromDate.Text = String.Empty
            Me.txtToDate.Text = String.Empty
            Me.ddlProfile.SelectedIndex = 0
            Me.txtRemarks.Text = String.Empty
            Me.hdnUserTypeCode.Value = 0
            Me.txtProject.Text = String.Empty
            FillGridClient()

        Catch ex As Exception
            'ShowErrorMessage(Me.Master, ex)
        End Try
    End Sub
#End Region

#Region "Button Events"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim Dt As New DataTable
        Dim Ds_stagemat As DataSet
        Dim ds_LocGrid As New DataSet
        Dim eStr As String = String.Empty
        Dim message As String = String.Empty
        Dim fileName As String = ""

        Try

            If Me.FileUpload1.HasFile Then
                If Not UploadAttachment(fileName) Then
                    Exit Sub
                End If
            End If

            If Not AssignUpdatedValues(fileName) Then
                Exit Sub
            End If

            Ds_stagemat = New DataSet
            Ds_stagemat.Tables.Add(CType(Me.ViewState(VS_DtNoticeMst), Data.DataTable).Copy())

            If Not objLambda.Save_InsertNoticeMst(Me.ViewState(Vs_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_NoticeMst, Ds_stagemat, "1", eStr) Then
                objcommon.ShowAlert("Error While Saving Notice Details!", Me.Page)
            End If

            ResetPage()
            Me.Response.Redirect("frmNoticeMst.aspx?mode=1")
            message = IIf(Me.ViewState(Vs_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, "Notice Details Saved Successfully", "Notice Details Updated Successfully")
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "Alert", "ShowAlert('" + message + "')", True)
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".......btnSave_Click")
        Finally
            Ds_stagemat = Nothing
        End Try
    End Sub

    Protected Sub btnRemarkSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemarkSave.Click

        Dim eStr As String = String.Empty
        Dim Ds_stagemat As DataSet
        Dim index As Integer

        Try

            Me.ViewState(Vs_Choice) = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete

            If Not AssignUpdatedValues("") Then
                Exit Sub
            End If




            Ds_stagemat = New DataSet
            Ds_stagemat.Tables.Add(CType(Me.ViewState(VS_DtNoticeMst), Data.DataTable).Copy())

            If Not objLambda.Save_InsertNoticeMst(Me.ViewState(Vs_Choice), WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_NoticeMst, Ds_stagemat, "1", eStr) Then
                objcommon.ShowAlert("Error While Saving ClientMst", Me.Page)
            End If

            objcommon.ShowAlert("Record Deleted Successfully", Me)
            ResetPage()
            Me.txtRemarks.Text = ""

            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "ModalPopupClose", "ModalPopupClose(); ", True)
        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".......btnSave_Click")
        Finally
            Ds_stagemat = Nothing
        End Try
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        ResetPage()
        Me.Response.Redirect("frmNoticeMst.aspx?mode=1")
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub
#End Region

#Region "Remove & Replace Functions"

    Private Sub RemoveTableFromString(ByRef StrTable As String)
        Dim StartIndex As Integer = 0
        Dim EndIndex As Integer = 0
        Dim StartString As String
        Dim ReplaceString As String
        If StrTable.Contains("<TABLE") Then
            EndIndex = StrTable.IndexOf("<TABLE")
            ReplaceString = StrTable.Substring(EndIndex + 1)
            StartString = StrTable.Substring(StartIndex, EndIndex - 1)
            ReplaceString = ReplaceString.Substring(ReplaceString.IndexOf(">") + 1)
            StartString += ReplaceString
            StartString = StartString.Replace("</TABLE>", "")
            StrTable = StartString
        End If
    End Sub

    Private Sub RemoveRowFromString(ByRef StrTable As String)
        Dim StartIndex As Integer = 0
        Dim EndIndex As Integer = 0
        Dim StartString As String
        Dim ReplaceString As String
        If StrTable.Contains("<TR") Then
            EndIndex = StrTable.IndexOf("<TR")
            ReplaceString = StrTable.Substring(EndIndex + 1)
            StartString = StrTable.Substring(StartIndex, EndIndex - 1)
            ReplaceString = ReplaceString.Substring(ReplaceString.IndexOf(">") + 1)
            StartString += "</BR>" + ReplaceString
            StartString = StartString.Replace("</TR>", "")
            StrTable = StartString
        End If
    End Sub

    Private Sub ReplaceTDFromString(ByRef StrTable As String)
        Dim ReplaceString As String
        If StrTable.Contains("<TD") Then
            StrTable = StrTable.Replace("<TD", "<SPAN")
            ReplaceString = StrTable.Replace("</TD>", "</SPAN>")
            StrTable = ReplaceString
        End If
    End Sub

    Private Sub RemoveColTBodyFromString(ByRef StrTable As String)
        Dim StartIndex As Integer = 0
        Dim EndIndex As Integer = 0
        Dim StartString As String
        Dim ReplaceString As String
        If StrTable.Contains("<COLGROUP>") Then
            StrTable = StrTable.Replace("<COLGROUP>", "")
            EndIndex = StrTable.IndexOf("<COL")
            ReplaceString = StrTable.Substring(EndIndex + 1)
            StartString = StrTable.Substring(StartIndex, EndIndex - 1)
            ReplaceString = ReplaceString.Substring(ReplaceString.IndexOf(">") + 1)
            StartString += ReplaceString
            StartString = StartString.Replace("<TBODY>", "")
            StartString = StartString.Replace("</TBODY>", "")
            StrTable = StartString
        End If
    End Sub

#End Region

#Region "Error Handler"

    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objcommon.WriteError(Server, Request, Session, exMessage + "<BR> " + eStr)
    End Sub
    Private Sub ShowErrorMessage(ByVal exMessage As String, ByVal eStr As String, ByVal ex As Exception)
        CType(Me.Master.FindControl("lblerrormsg"), Label).Text = exMessage + "<BR> " + eStr
        objcommon.WriteError(Server, Request, Session, ex, exMessage + "<BR> " + eStr)
    End Sub
#End Region

#Region "Grid Event"

    Protected Sub gvnotice_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim index As Integer = e.CommandArgument

        If e.CommandName.ToUpper = "EDIT" Then
            Me.Response.Redirect("frmNoticeMst.aspx?mode=2&value=" & Me.gvnotice.Rows(index).Cells(GVC_nNoticeNo).Text.Trim())
        End If

        If e.CommandName.ToUpper = "Delete" Then
            'Me.Response.Redirect("frmNoticeMst.aspx?mode=2&value=" & Me.gvnotice.Rows(index).Cells(GVC_nNoticeNo).Text.Trim())
        End If

    End Sub

    Protected Sub gvnotice_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Dim strCellValue As String = ""
        If Not e.Row.RowType = DataControlRowType.Pager Then
            e.Row.Cells(GVC_nNoticeNo).Visible = False

            If e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Cells(GVC_SrNo).Text = e.Row.RowIndex + (gvnotice.PageSize * gvnotice.PageIndex) + 1
                CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("lnkEdit"), ImageButton).CommandName = "Edit"
                CType(e.Row.FindControl("lnkDelete"), ImageButton).CommandArgument = e.Row.RowIndex
                CType(e.Row.FindControl("lnkDelete"), ImageButton).CommandName = "Delete"

            End If
        End If
    End Sub

#End Region

    

End Class
