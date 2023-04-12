Imports System.Drawing.Imaging

Partial Class frmOVISExport
    Inherits System.Web.UI.Page

#Region "Variable Declaration"

    Private objcommon As New clsCommon
    Private objHelpDbTable As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = objcommon.GetHelpDbTableRef()

    Private Const VS_Choice As String = "Choice"

#End Region

#Region "Form Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not GenCall_ShowUI() Then
                Exit Sub
            End If
        End If
    End Sub

#End Region

#Region "GenCall_ShowUI"

    Private Function GenCall_ShowUI() As Boolean
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None

        Try
            Page.Title = ":: OVIS Export :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "OVIS Export"
            Choice = Me.ViewState("Choice")
            Me.pnlSubject.Visible = False
            Me.chkSelectAll.Visible = False

            chkSelectAll.Attributes.Add("onclick", "CheckBoxListSelection(" + Me.chklstSubject.ClientID + ",this);")

            GenCall_ShowUI = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, ".....GenCall_ShowUI")
        Finally
        End Try
    End Function

#End Region

#Region "FillCheckBoxList"

    Private Sub fillchklstSubject(Optional ByVal All As Boolean = False)
        Dim ds_Subject As New DataSet
        Dim Wstr As String = String.Empty
        Dim estr As String = String.Empty
        Try

            Wstr = "cStatusIndi <> 'D'"
            If Not All Then
                Wstr += " And vWorkspaceId='" & Me.HProjectId.Value.ToString.Trim() & "' And iPeriod='" & Me.ddlPeriod.SelectedValue.Trim() & "' and iMySubjectNo > 0 order by dReportingDate"
            End If

            If Not Me.objHelpDbTable.GetViewWorkspaceSubjectMst(Wstr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                ds_Subject, estr) Then
                Me.objcommon.ShowAlert("Error While Getting View_WorkspaceSubjectMst", Me.Page)
                Exit Sub
            End If

            If ds_Subject.Tables(0).Rows.Count < 1 Then
                Me.pnlSubject.Visible = False
                Me.chkSelectAll.Visible = False
                Exit Sub
            End If

            Me.chklstSubject.DataSource = ds_Subject.Tables(0).DefaultView.ToTable(True, "vSubjectId,FullName".Split(","))
            Me.chklstSubject.DataValueField = "vSubjectId"
            Me.chklstSubject.DataTextField = "FullName"
            Me.chklstSubject.DataBind()
            Me.pnlSubject.Visible = True
            Me.chkSelectAll.Visible = True

            For Each lstItem As ListItem In chklstSubject.Items

                lstItem.Attributes.Add("onclick", "SetCheckUncheckAll(document.getElementById('" + _
                                                Me.chklstSubject.ClientID + "'), document.getElementById('" + _
                                                Me.chkSelectAll.ClientID + "'));")

            Next lstItem

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....fillchklstSubject")
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

#Region "Button Events"

    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim CntSubject As Integer = 0
        Dim strSubject As String = String.Empty
        Dim ds_SubjectBlobDetails As New DataSet
        Dim ds_SubjectMaster As New DataSet
        Try

            Do While CntSubject <= chklstSubject.Items.Count - 1

                If Me.chklstSubject.Items(CntSubject).Selected = True Then
                    strSubject += "'" + Me.chklstSubject.Items(CntSubject).Value.Trim() + "'"
                    strSubject += ","
                End If
                CntSubject += 1

            Loop
            strSubject = strSubject.Substring(0, strSubject.Length - 1)

            wStr = "cRecordType='P' and vSubjectId in (" + strSubject + ") And cStatusIndi <> 'D'"

            If Not objHelpDbTable.View_MaxSubjectBlobdetails_ForOVIS(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                    ds_SubjectBlobDetails, eStr) Then
                objcommon.ShowAlert("Error While Getting Data From View_MaxSubjectBlobdetails_Search.." + eStr, Me.Page)
                Exit Sub
            End If

            wStr = "vSubjectId in (" + strSubject + ") And cStatusIndi <> 'D'"

            If Not objHelpDbTable.GetView_SubjectMaster(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                    ds_SubjectMaster, eStr) Then
                objcommon.ShowAlert("Error While Getting Data From View_SubjectMaster.." + eStr, Me.Page)
                Exit Sub
            End If

            If Not CreatePhoto(ds_SubjectBlobDetails, ds_SubjectMaster) Then
                Exit Sub
            End If

            objcommon.ShowAlert("File Uploaded On Server Successfully... You Can Download It From \\\\" + _
             Server.MachineName.ToString.Trim() + "\\SubjectDetails\\" + Me.HProjectId.Value.Trim() + "\\OVIS\\" + Convert.ToString(Me.ddlPeriod.SelectedValue), Me.Page)

            Me.resetPage()

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...btnUpload_Click")
        End Try
    End Sub

    Protected Sub BtnAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAll.Click
        Me.txtproject.Text = ""
        fillchklstSubject(True)
    End Sub

    Protected Sub btnSetProject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSetProject.Click
        Me.FillPeriodDropDown()
    End Sub

    Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.resetPage()
    End Sub

#End Region

#Region "CreatePhoto()"
    'vSubjectID,vLocationcode,nSubjectContactNo,vLocalAdd1,vLocalAdd12,vLocalAdd13,vLocalAdd21,vLocalAdd22,vLocalAdd23,vPerAdd1,vPerAdd2,vPerAdd3,dEnrollmentDate,vFirstName,vLocalTelephoneno1,vLocalTelephoneno2,vSurName,vMiddleName,FullName,vInitials,dBirthDate,cSex,vICFLanguageCodeId,ICFLanguage,vEducationQualification,vOccupation,cBloodGroup,cRh,vProofOfAge1,vProofOfAge2,vProofOfAge3,vbPhotograph,vbAutograph,vbThumb,vRefSubjectId,iModifyBy,dModifyOn,cStatusIndi,vPerTelephoneno,vOfficeAddress,vOfficeTelephoneno,vContactName1,vContactName2,vContactAddress11,vContactAddress21,VCONTACTADDRESS12,vContactTelephoneno1,vContactTelephoneno2,vReferredBy,nSubjectDetailNo,cMaritalStatus,cFoodHabit,nHeight,nWeight,nBMI,nSubjectFemaleDetailNo,dLastMenstrualDate,cLastMenstrualIndi,iLastMenstrualDays,dLastDelivaryDate,iNoOfChildren,cChildrenHelath,vchildrenHealthRemark,vGravida,cAbortions,dAbortionDate,vPara,cRegular,cLoctating,cFamilyPlanningSelf,cContraception,cBarrier,cPills,cRhythm,cIUCD,cOthers,vOtherRemark,cIsVolunteerinBearingAge,iNoOfChildrenDied,vRemarkifDied,nSubjectWorkSpaceNo,vWorkSpaceId,iScreenId,Age,dAllocationDate,vPerCity,cRejectionFlag,vRemark

    Private Function CreatePhoto(ByVal ds_Photo As DataSet, ByVal ds_SubMst As DataSet) As Boolean
        Dim imgConverted As Drawing.Bitmap
        Dim bytes As Byte()
        Dim Path As String = System.Configuration.ConfigurationSettings.AppSettings.Item("FolderForSubjectDetail").Trim()
        Dim SubjectPath As String = String.Empty
        Dim dr As DataRow
        Dim FullName As String = String.Empty
        Dim dirinfo As DirectoryInfo
        Dim dv As New DataView
        Dim dt As New DataTable
        Dim strLine As String
        Dim fi As StreamWriter
        Dim filePath As String = String.Empty
        Try

            Path += Me.HProjectId.Value.Trim() + "/OVIS/" + Convert.ToString(Me.ddlPeriod.SelectedValue) + "/"
            dirinfo = New DirectoryInfo(Server.MapPath(Path))
            If Not dirinfo.Exists Then
                dirinfo.Create()
            End If

            filePath = Server.MapPath(Path) + "OVISExport.txt"
            fi = New StreamWriter(filePath, False)

            strLine = "----- ONLINE VOLUNTEERS INFORMATION SYSTEM -----"
            fi.WriteLine(strLine)
            fi.WriteLine("")
            strLine = "      Volunteers' Basic Information Upload File.     "
            fi.WriteLine(strLine)
            fi.WriteLine("")
            strLine = "   Participating CRO: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            fi.WriteLine(strLine)
            
            For Each dr In ds_Photo.Tables(0).Rows

                bytes = dr("vtranValue")
                imgConverted = BytesToBmp_Serialized(bytes)

                SubjectPath = Path + dr("vSubjectId") + ".JPG"
                imgConverted.Save(Server.MapPath(SubjectPath), ImageFormat.Jpeg)

                dv = ds_SubMst.Tables(0).DefaultView
                dv.RowFilter = "vSubjectId = '" + dr("vSubjectId") + "'"
                dt = dv.ToTable()
                '======added on 1-dec-2009 By Deepak Singh to change the format of Fullname
                FullName = dt.Rows(0).Item("vFirstName") + " " + dt.Rows(0).Item("vMiddleName") + " " + dt.Rows(0).Item("vSurName")

                '==========================================================================
                strLine = FullName.ToString.Trim() + "," + dr("vSubjectId") + ".JPG," + _
                            dt.Rows(0).Item("vProofOfAge1").ToString.Trim()
                fi.WriteLine(strLine.ToUpper())
            Next

            fi.Close()
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "....CreatePhoto")
            Return False
        End Try

    End Function

    Private Function BytesToBmp_Serialized(ByVal bmpBytes As Byte()) As Drawing.Bitmap
        Dim bf As New Runtime.Serialization.Formatters.Binary.BinaryFormatter()
        ' copy the bytes to the memory
        Dim ms As New MemoryStream(bmpBytes)

        Try
            Return DirectCast(bf.Deserialize(ms), Drawing.Bitmap)
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...BytesToBmp_Serialized")
        End Try

    End Function

#End Region

#Region "Reset Page"

    Protected Sub resetPage()

        Me.HProjectId.Value = ""
        Me.txtproject.Text = ""
        Me.chkSelectAll.Checked = False
        Me.chklstSubject.ClearSelection()
        Me.chklstSubject.DataSource = Nothing
        Me.ddlPeriod.Items.Clear()
        If Not GenCall_ShowUI() Then
            Exit Sub
        End If

    End Sub

#End Region

    Private Sub FillPeriodDropDown()
        Dim ds_Periods As New DataSet
        Dim wStr As String = String.Empty
        Dim eStr As String = String.Empty
        Dim Periods As Integer = 1

        Try

            Me.ddlPeriod.Items.Clear()
            wStr = "vWorkSpaceId = '" + Me.HProjectId.Value.Trim() + "'"

            If Not objHelp.GetViewWorkSpaceNodeDetail(wStr, ds_Periods, eStr) Then
                Throw New Exception(eStr)
            End If
            If ds_Periods.Tables(0).Rows.Count > 0 Then

                Periods = ds_Periods.Tables(0).DefaultView.ToTable(True, "iPeriod").Rows.Count
                For count As Integer = 0 To Periods - 1
                    Me.ddlPeriod.Items.Add((count + 1).ToString)
                Next count
                Me.ddlPeriod.Items.Insert(0, New ListItem("Select Period", "0"))

                If Not IsNothing(Me.Request.QueryString("PeriodId")) Then
                    Me.ddlPeriod.SelectedValue = Me.Request.QueryString("PeriodId").Trim()
                    Me.ddlPeriod.Enabled = False
                End If
            End If

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillPeriodDropDown")
        End Try

    End Sub


#Region "DropDown Events"

    Protected Sub ddlPeriod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            If Me.ddlPeriod.SelectedIndex > 0 Then
                fillchklstSubject()
            Else
                Me.pnlSubject.Visible = False
                Me.chkSelectAll.Visible = False
            End If
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...ddlPeriod_SelectedIndexChanged")
        End Try
    End Sub

#End Region

End Class
