Imports System.Web.Services
Imports Newtonsoft.Json

Partial Class frmPanelRightsMst
    Inherits System.Web.UI.Page

#Region "VARIABLE DECLARATION "

    Private ObjCommon As New clsCommon
    Private objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
    Private Const VS_DtClientMst As String = "dtPanelDisplayData"
#End Region

#Region "Form Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Page.Title = ":: Panel Rights :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Panel Rights"
            GenCall()
        End If

    End Sub

#End Region

#Region "GenCall "
    Private Function GenCall() As Boolean
        Dim ds As New DataSet

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""

            If Not GenCall_Data() Then ' For Data Retrieval
                Exit Function
            End If

            If Not GenCall_ShowUI() Then 'For Displaying Data
                Exit Function
            End If

            GenCall = True

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function
#End Region

#Region "GenCall_Data "
    Private Function GenCall_Data() As Boolean

        Dim eStr_Retu As String = ""
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Dim ds As DataSet = Nothing

        Try
            CType(Me.Master.FindControl("lblerrormsg"), Label).Text = ""
            GenCall_Data = True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")

        Finally

        End Try
    End Function
#End Region

#Region "GenCall_showUI "

    Private Function GenCall_ShowUI() As Boolean
        Dim Choice As WS_Lambda.DataObjOpenSaveModeEnum = WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_None
        Try
            Page.Title = "::Panel Display Rights :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Panel Display Rights"
            Choice = Me.ViewState("Choice")

            If Not FillDropDown() Then
                Return False
            End If
          
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        Finally
        End Try
    End Function

#End Region

#Region "Control Event "
    Protected Sub BtnExit_Click(sender As Object, e As EventArgs)
        Response.Redirect("frmMainPage.aspx")
    End Sub

    Protected Sub btnExportToExcel_Click(sender As Object, e As EventArgs)
        Dim ds_Field As New DataSet
        Dim fileName As String = String.Empty
        
        Try

            ds_Field = Me.Session("dtPanelDisplayData")
            ViewState("IdOfBtn") = "BtnExportToExcelForGrid"
            fileName = "Dash Board Panel Rights-" + Date.Today.ToString("dd-MMM-yyyy") + ".xls"
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)

            Context.Response.Write(ConvertDsuserTO(ds_Field))

            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

    Protected Sub BtnExportToExcel_AuditTrail_Click(sender As Object, e As EventArgs)
        Dim ds_Field As New DataSet
        Dim fileName As String = String.Empty
        Dim dt_Final As New DataTable
        Dim wStr As String, estr As String = String.Empty
        Dim npaneldisplyno As String = String.Empty

        Try
            npaneldisplyno = hdn_npaneldisplyno.Value

            wStr = " npaneldisplyno = '" + npaneldisplyno + "' Order by npaneldisplyHistoryno DESC"
            If Not objHelp.GetView_PanelDisplayHistory(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Field, estr) Then
                Throw New Exception(estr)
            End If
            If ds_Field.Tables(0).Rows.Count = 0 Then
                Me.ObjCommon.ShowAlert("Data Not Found", Me.Page)
                Exit Sub
            End If

            ViewState("IdOfBtn") = "BtnExportToExcelForAuditTrail"
            fileName = "Dash Board Panel Rights Audit Trail-" + Date.Today.ToString("dd-MMM-yyyy") + ".xls"
            dt_Final = ds_Field.Tables(0)
            'ds_Field.Tables.Add(dt_Final)
            Context.Response.Buffer = True
            Context.Response.ClearContent()
            Context.Response.ClearHeaders()

            Context.Response.AddHeader("Content-type", "application/vnd.ms-excel")
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName)

            Context.Response.Write(ConvertDsuserTO(ds_Field))

            Context.Response.Flush()
            Context.Response.End()

            System.IO.File.Delete(fileName)

        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "")
        End Try
    End Sub

#End Region

#Region "Export to excel"

    Private Function ConvertDsuserTO(ByVal ds As DataSet) As String
        Dim strMessage As New StringBuilder
        Dim i As Integer
        Dim iCol As Integer
        Dim j As Integer
        Dim dsConvert As New DataSet

        Try
            strMessage.Append("<table width=""100%"" border=""1"" cellpadding=""0"" cellspacing=""0"">")

            strMessage.Append("<tr>")
            Select Case ViewState("IdOfBtn")
                Case "BtnExportToExcelForAuditTrail"
                    strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                    strMessage.Append("</font></strong><center></b></td>")
                    strMessage.Append("</tr>")

                    strMessage.Append("<tr>")
                    strMessage.Append("</tr>")

                    strMessage.Append("<tr>")
                    strMessage.Append("<td colspan=""6""><center><strong><b><font color=""#000099"" size=""2.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append("Audit Trail :-> " + Convert.ToString(ds.Tables(0).Rows(0).Item(2)).Trim())
                    strMessage.Append("</font></strong><center></b></td>")
                    strMessage.Append("</tr>")

                    strMessage.Append("<tr><td><font color=""#000099"" size=""2.5"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""3"" face=""Verdana""><b>" + Date.Today.ToString("dd-MMM-yyyy") + "</b></font></td><td></td><td></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")

                Case "BtnExportToExcelForGrid"
                    strMessage.Append("<td colspan=""4""><center><strong><b><font color=""#000099"" size=""4"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append(System.Configuration.ConfigurationManager.AppSettings("Client"))
                    strMessage.Append("</font></strong><center></b></td>")
                    strMessage.Append("</tr>")

                    strMessage.Append("<tr>")
                    strMessage.Append("</tr>")

                    strMessage.Append("<tr>")
                    strMessage.Append("<td colspan=""4""><center><strong><b><font color=""#000099"" size=""2.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                    strMessage.Append("Dash Board Panel Rights")
                    strMessage.Append("</font></strong><center></b></td>")
                    strMessage.Append("</tr>")

                    strMessage.Append("<tr><td><font color=""#000099"" size=""2.5"" face=""Verdana""><b>Print Date:-</b></font></td> <td align = ""left""><font color=""#000099"" size=""3"" face=""Verdana""><b>" + Date.Today.ToString("dd-MMM-yyyy") + "</b></font></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>Printed By:-</b></font></td><td><font color=""#000099"" size=""3"" face=""Verdana""><b>" + Session(S_LoginName) + "</b></font></td></tr>")


            End Select

             strMessage.Append("<tr>")
            strMessage.Append("</tr>")
            strMessage.Append("<tr>")

            If ViewState("IdOfBtn") = "BtnExportToExcelForAuditTrail" Then
                dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vPanelName,vUserTypeName,cActiveFlag,vRemark,vModifyBy,dModifyOn".Split(",")).DefaultView.ToTable())
                dsConvert.AcceptChanges()
               dsConvert.Tables(0).Columns(0).ColumnName = "Panel Name"
                dsConvert.Tables(0).Columns(1).ColumnName = "Profile"
                dsConvert.Tables(0).Columns(2).ColumnName = "Current Status"
                dsConvert.Tables(0).Columns(3).ColumnName = "Remark"
                dsConvert.Tables(0).Columns(4).ColumnName = "Modify By"
                dsConvert.Tables(0).Columns(5).ColumnName = "Modify On"
                dsConvert.AcceptChanges()
            ElseIf ViewState("IdOfBtn") = "BtnExportToExcelForGrid" Then
                dsConvert.Tables.Add(ds.Tables(0).DefaultView.ToTable(True, "vPanelName,vUserTypeName,cActiveFlag,vRemark".Split(",")).DefaultView.ToTable())
                dsConvert.AcceptChanges()
                dsConvert.Tables(0).Columns(0).ColumnName = "Panel Name"
                dsConvert.Tables(0).Columns(1).ColumnName = "Profile"
                dsConvert.Tables(0).Columns(2).ColumnName = "Current Status"
                dsConvert.Tables(0).Columns(3).ColumnName = "Remark"
                dsConvert.AcceptChanges()

            End If

             For iCol = 0 To dsConvert.Tables(0).Columns.Count - 1
                strMessage.Append("<td bgcolor=""#1560a1""><strong><font color=""#FFFFFF"" size=""2.5"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                strMessage.Append(Convert.ToString(dsConvert.Tables(0).Columns(iCol)).Trim())
                strMessage.Append("</font></strong></td>")

            Next

            strMessage.Append("</tr>")
            strMessage.Append("<tr>")
            strMessage.Append("</tr>")

            For j = 0 To dsConvert.Tables(0).Rows.Count - 1
                strMessage.Append("<tr>")
                For i = 0 To dsConvert.Tables(0).Columns.Count - 1

                    If j Mod 2 = 0 Then
                        strMessage.Append("<td align=""left"" bgcolor=""84c8e6""><font color=""#191970"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                        If Convert.ToString(dsConvert.Tables(0).Columns(i)).Trim() = "Current Status" And ViewState("IdOfBtn") = "BtnExportToExcelForGrid" Then
                            If Convert.ToString(dsConvert.Tables(0).Rows(j).Item(i)).Trim() = "Y" Then
                                strMessage.Append("Active")
                            Else
                                strMessage.Append("InActive")
                            End If

                        Else
                            strMessage.Append(Convert.ToString(dsConvert.Tables(0).Rows(j).Item(i)).Trim())
                        End If
                        strMessage.Append("</font></td>")
                    Else
                        strMessage.Append("<td align=""left"" bgcolor=""#""><font color=""#191970"" size=""2"" face=""Verdana, Arial, Helvetica, sans-serif"">")
                        If Convert.ToString(dsConvert.Tables(0).Columns(i)).Trim() = "Current Status" And ViewState("IdOfBtn") = "BtnExportToExcelForGrid" Then
                            If Convert.ToString(dsConvert.Tables(0).Rows(j).Item(i)).Trim() = "Y" Then
                                strMessage.Append("Active")
                            Else
                                strMessage.Append("InActive")
                            End If
                        Else
                            strMessage.Append(Convert.ToString(dsConvert.Tables(0).Rows(j).Item(i)).Trim())
                        End If
                        strMessage.Append("</font></td>")
                    End If


                Next
                strMessage.Append("</tr>")
            Next
            strMessage.Append("</table>")

            Return strMessage.ToString

        Catch ex As Exception
            ShowErrorMessage(ex.Message, ".....ConvertDsuserTO")
            Return ""
        End Try
    End Function

#End Region


#Region "FillDropDown"
    Private Function FillDropDown() As Boolean
        Dim ds_UserTypemst As New Data.DataSet
        Dim estr As String = String.Empty
        Dim dv_UserTypemst As New DataView

        Try

            objHelp.getUserTypeMst("", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_AllRecords, ds_UserTypemst, estr)

            dv_UserTypemst = ds_UserTypemst.Tables(0).DefaultView
            dv_UserTypemst.Sort = "vUserTypeName"
            ddlProfile.Items.Clear()
            Me.ddlProfile.DataSource = dv_UserTypemst
            Me.ddlProfile.DataValueField = "vUserTypeCode"
            Me.ddlProfile.DataTextField = "vUserTypeName"
            Me.ddlProfile.DataBind()
            'Me.ddlProfile.Items.Insert(0, "--Select Profile--")
            '========
            Return True
        Catch ex As Exception
            Me.ShowErrorMessage(ex.Message, "...FillDropDown")
            Return False
        End Try
    End Function

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

#Region "Web Method"

    <WebMethod> _
    Public Shared Function GetPanelRightsData(ByVal ID As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
        Dim ds_Service As DataSet = New DataSet
        Dim dt_PanelDisplay As DataTable = New DataTable
        Dim dr_Service As DataRow
        Dim wStr As String = String.Empty
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim dt As New DataTable

        Dim ds_PanelDisplay As DataSet = New DataSet

        Dim i As Integer = 1
        Try
            wStr = "cStatusIndi <> 'D' "
            If Not objHelp.GetPanelDisplayData(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                           ds_Service, estr) Then
                Return "false"
                Exit Function

            End If
            HttpContext.Current.Session("dtPanelDisplayData") = ds_Service
            Dim distinctDT As DataTable = ds_Service.Tables(0).DefaultView.ToTable(True, "vPanelId", "vPanelName")
            If Not dt_PanelDisplay Is Nothing Then
                dt_PanelDisplay.Columns.Add("SrNo")
                dt_PanelDisplay.Columns.Add("vPanelId")
                dt_PanelDisplay.Columns.Add("vPanelName")
                dt_PanelDisplay.Columns.Add("vUserTypeCode")
                dt_PanelDisplay.Columns.Add("vDeptCode")
                dt_PanelDisplay.Columns.Add("vLocationCode")
                dt_PanelDisplay.Columns.Add("cStatusIndi")
                dt_PanelDisplay.Columns.Add("nPanelDisplyNo")
                dt_PanelDisplay.Columns.Add("cActiveFlag")
                dt_PanelDisplay.Columns.Add("vRemark")

                dt_PanelDisplay.Columns.Add("vUserTypeName")
                dt_PanelDisplay.Columns.Add("vDeptName")
                dt_PanelDisplay.Columns.Add("vLocationName")

                dt_PanelDisplay.Columns.Add("Edit")
                dt_PanelDisplay.Columns.Add("Audit")

            End If
            dt = ds_Service.Tables(0)
            Dim dv As New DataView(dt)

            For Each dr As DataRow In dv.ToTable.Rows
                dr_Service = dt_PanelDisplay.NewRow()
                dr_Service("SrNo") = i
                dr_Service("vPanelId") = dr("vPanelId").ToString()
                dr_Service("vPanelName") = dr("vPanelName").ToString()
                dr_Service("vUserTypeCode") = dr("vUserTypeCode").ToString()
                dr_Service("vDeptCode") = dr("vDeptCode").ToString()
                dr_Service("vLocationCode") = dr("vLocationCode").ToString()
                dr_Service("cStatusIndi") = dr("cStatusIndi").ToString()
                dr_Service("nPanelDisplyNo") = dr("nPanelDisplyNo").ToString()
                dr_Service("cActiveFlag") = dr("cActiveFlag").ToString()
                dr_Service("vRemark") = dr("vRemark").ToString()

                dr_Service("vUserTypeName") = dr("vUserTypeName").ToString()
                dr_Service("vDeptName") = dr("vDeptName").ToString()
                dr_Service("vLocationName") = dr("vLocationName").ToString()

                dr_Service("Edit") = ""
                dr_Service("Audit") = ""
                dt_PanelDisplay.Rows.Add(dr_Service)
                dt_PanelDisplay.AcceptChanges()
                i += 1
            Next

            dt_PanelDisplay.TableName = "dt_PanelDisplay"
            distinctDT.TableName = "distinctDT"
            ds_PanelDisplay.Tables.Add(dt_PanelDisplay)
            ds_PanelDisplay.Tables.Add(distinctDT)
            strReturn = JsonConvert.SerializeObject(ds_PanelDisplay)
            Return strReturn


            If ds_Service.Tables.Count > 0 Then
                If ds_Service.Tables(0).Rows.Count > 0 Then
                    strReturn = JsonConvert.SerializeObject(ds_Service.Tables(0))
                    Return strReturn
                End If
            End If
            Return "false"

        Catch ex As Exception
            Throw New Exception("Error while Insert data")
            Return "false"
        End Try

    End Function

    <WebMethod> _
    Public Shared Function InsertData(ByVal userid As String, ByVal vPanelId As String, ByVal vPanelName As String, ByVal vUserTypeCode As String, ByVal vRemark As String, ByVal cActiveFlag As String, ByVal OpMode As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
        Dim ds_PanelDisplay As DataSet = New DataSet
        Dim dt_PanelDisplay As DataTable = New DataTable
        Dim dr_PanelDisplay As DataRow
        Dim wStr As String = String.Empty
        Dim estr As String = String.Empty
        Try


            If OpMode = "1" Then
                wStr = " 1 = 2 "
            Else
                wStr = " vPanelId = '" + vPanelId + "' AND  vUserTypeCode = '" + vUserTypeCode + "' AND  cStatusIndi <> 'D' "
            End If
            If Not objHelp.GetPanelDisplay(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                            ds_PanelDisplay, estr) Then

                Exit Function

            End If

            If ds_PanelDisplay.Tables.Count > 0 Then    ''For EDIT
                If ds_PanelDisplay.Tables(0).Rows.Count > 0 Then
                    dt_PanelDisplay = ds_PanelDisplay.Tables(0)
                    dr_PanelDisplay = dt_PanelDisplay.Rows(0)
                    dr_PanelDisplay("iModifyBy") = userid
                    dr_PanelDisplay("vPanelId") = vPanelId
                    dr_PanelDisplay("vUserTypeCode") = vUserTypeCode
                    dr_PanelDisplay("vRemark") = vRemark
                    dr_PanelDisplay("cActiveFlag") = cActiveFlag
                    dr_PanelDisplay("cStatusIndi") = "N"
                    dt_PanelDisplay.AcceptChanges()

                    ds_PanelDisplay.Tables.Clear()
                    If dt_PanelDisplay.Rows.Count > 0 Then
                        ds_PanelDisplay.Tables.Add(dt_PanelDisplay)
                        If Not objLambda.Save_PanelDisplay(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_PanelDisplay, estr) Then
                            Exit Function
                        End If
                    End If

                Else          '' For ADD
                    dt_PanelDisplay = ds_PanelDisplay.Tables(0)
                    dr_PanelDisplay = dt_PanelDisplay.NewRow()
                    dr_PanelDisplay("iModifyBy") = userid
                    dr_PanelDisplay("vPanelId") = vPanelId
                    dr_PanelDisplay("vPanelName") = vPanelName
                    dr_PanelDisplay("vUserTypeCode") = vUserTypeCode
                    dr_PanelDisplay("vRemark") = vRemark
                    dr_PanelDisplay("cActiveFlag") = cActiveFlag
                    dr_PanelDisplay("cStatusIndi") = "N"

                    dt_PanelDisplay.Rows.Add(dr_PanelDisplay)
                    dt_PanelDisplay.AcceptChanges()

                    ds_PanelDisplay.Tables.Clear()
                    If dt_PanelDisplay.Rows.Count > 0 Then
                        ds_PanelDisplay.Tables.Add(dt_PanelDisplay)
                        If Not objLambda.Save_PanelDisplay(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_PanelDisplay, estr) Then
                            Exit Function
                        End If
                    End If

                End If

            End If

            Return "false"
        Catch ex As Exception
            Throw New Exception("Error while Insert data")
            Return "false"
        End Try

    End Function

    <WebMethod> _
    Public Shared Function AuditTrail(ByVal npaneldisplyno As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_PanelDisplay As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty

        Dim dt_PanelDisplayHistory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable
        Try

            wStr = " npaneldisplyno = '" + npaneldisplyno + "' Order by npaneldisplyHistoryno DESC"
            If Not objHelp.GetView_PanelDisplayHistory(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_PanelDisplay, estr) Then
                Throw New Exception(estr)
            End If

            If Not dt_PanelDisplayHistory Is Nothing Then
                dt_PanelDisplayHistory.Columns.Add("SrNo")
                dt_PanelDisplayHistory.Columns.Add("vPanelName")
                dt_PanelDisplayHistory.Columns.Add("vUserTypeName")
                dt_PanelDisplayHistory.Columns.Add("cActiveFlag")
                dt_PanelDisplayHistory.Columns.Add("vRemark")
                dt_PanelDisplayHistory.Columns.Add("vModifyBy")
                dt_PanelDisplayHistory.Columns.Add("dModifyOn")
            End If

            dt = ds_PanelDisplay.Tables(0)
            Dim dv As New DataView(dt)

            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dt_PanelDisplayHistory.NewRow()
                drAuditTrail("SrNo") = i
                drAuditTrail("vPanelName") = dr("vPanelName").ToString()
                drAuditTrail("vUserTypeName") = dr("vUserTypeName").ToString()
                drAuditTrail("cActiveFlag") = dr("cActiveFlag").ToString()
                drAuditTrail("vRemark") = dr("vRemark").ToString()
                drAuditTrail("vModifyBy") = dr("vModifyBy").ToString()
                drAuditTrail("dModifyOn") = Convert.ToString(dr("dModifyOn"))
                dt_PanelDisplayHistory.Rows.Add(drAuditTrail)
                dt_PanelDisplayHistory.AcceptChanges()
                i += 1
            Next
            strReturn = JsonConvert.SerializeObject(dt_PanelDisplayHistory)
            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

#End Region

    
End Class

