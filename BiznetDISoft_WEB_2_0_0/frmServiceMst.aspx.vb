Imports System.Web.Services
Imports Newtonsoft.Json

Partial Class frmServiceMst
    Inherits System.Web.UI.Page

#Region "Form Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Page.Title = ":: Scope of Services :: " + System.Configuration.ConfigurationManager.AppSettings("Client")
            CType(Me.Master.FindControl("lblHeading"), Label).Text = "Scope of Services"
        End If

    End Sub
#End Region

    Protected Sub BtnExit_Click(sender As Object, e As EventArgs)
        Response.Redirect("frmMainPage.aspx")
    End Sub

#Region "Web Method"

    <WebMethod> _
    Public Shared Function InsertData(ByVal ID As String, ByVal ServiceName As String, ByVal Remark As String, ByVal ServiceCode As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
        Dim ds_Service As DataSet = New DataSet
        Dim dt_Service As DataTable = New DataTable
        Dim dr_Service As DataRow
        Dim wStr As String = String.Empty
        Dim estr As String = String.Empty
        Try

            If ServiceCode = "" Then
                wStr = "cStatusIndi <> 'D' And vServiceName='" + ServiceName.Trim() + "' "
                If Not objHelp.GetServiceDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                               ds_Service, estr) Then
                    Exit Function

                End If
                If ds_Service.Tables.Count > 0 Then
                    If ds_Service.Tables(0).Rows.Count > 0 Then
                        Return "exist"
                    Else
                        dt_Service = ds_Service.Tables(0)
                        dr_Service = dt_Service.NewRow()
                        dr_Service("iModifyBy") = ID
                        dr_Service("vServiceName") = ServiceName
                        dr_Service("vRemark") = Remark
                        dr_Service("vServiceCode") = "0000"
                        dt_Service.Rows.Add(dr_Service)
                        dt_Service.AcceptChanges()
                        'Return "true"
                    End If
                End If
                ds_Service.Tables.Clear()
                If dt_Service.Rows.Count > 0 Then
                    ds_Service.Tables.Add(dt_Service)
                    If Not objLambda.Save_InsertServiceMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_ServiceMst, ds_Service, "1", estr) Then
                        Exit Function
                    End If
                    Return "add"
                End If
            Else

                wStr = "cStatusIndi <> 'D' And vServiceName='" + ServiceName + "' AND vServiceCode <> '" + ServiceCode + "'  "
                If Not objHelp.GetServiceDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                               ds_Service, estr) Then
                    Exit Function

                End If

                If ds_Service.Tables.Count > 0 Then
                    If ds_Service.Tables(0).Rows.Count > 0 Then
                        Return "exist"
                    Else
                        dt_Service = ds_Service.Tables(0)
                        dr_Service = dt_Service.NewRow()
                        dr_Service("iModifyBy") = ID
                        dr_Service("vServiceName") = ServiceName
                        dr_Service("vRemark") = Remark
                        dr_Service("vServiceCode") = ServiceCode
                        dt_Service.Rows.Add(dr_Service)
                        dt_Service.AcceptChanges()
                        'Return "true"
                    End If
                End If
                ds_Service.Tables.Clear()
                If dt_Service.Rows.Count > 0 Then
                    ds_Service.Tables.Add(dt_Service)
                    If Not objLambda.Save_InsertServiceMst(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, WS_Lambda.MasterEntriesEnum.MasterEntriesEnum_ServiceMst, ds_Service, "2", estr) Then
                        Exit Function
                    End If
                    Return "update"
                End If

            End If

            Return "false"
        Catch ex As Exception
            Throw New Exception("Error while Insert data")
            Return "false"
        End Try

    End Function

    <WebMethod> _
    Public Shared Function GetServiceData(ByVal ID As String) As String

        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim objLambda As WS_Lambda.WS_Lambda = ObjCommon.GetHelpDbLambdaRef()
        Dim ds_Service As DataSet = New DataSet
        Dim dt_Service As DataTable = New DataTable
        Dim dr_Service As DataRow
        Dim wStr As String = String.Empty
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty
        Dim dt As New DataTable
        Dim i As Integer = 1
        Try
            wStr = "cStatusIndi <> 'D' "
            If Not objHelp.GetServiceDetail(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                           ds_Service, estr) Then
                Exit Function

            End If
            If Not dt_Service Is Nothing Then
                dt_Service.Columns.Add("SrNo")
                dt_Service.Columns.Add("ServiceCode")
                dt_Service.Columns.Add("ServiceName")
                dt_Service.Columns.Add("Remark")
                dt_Service.Columns.Add("Edit")
                dt_Service.Columns.Add("Audit")

            End If
            dt = ds_Service.Tables(0)
            Dim dv As New DataView(dt)

            For Each dr As DataRow In dv.ToTable.Rows
                dr_Service = dt_Service.NewRow()
                dr_Service("SrNo") = i
                dr_Service("ServiceCode") = dr("vServiceCode").ToString()
                dr_Service("ServiceName") = dr("vServiceName").ToString()
                dr_Service("Remark") = dr("vRemark").ToString()
                dr_Service("Edit") = ""
                dr_Service("Audit") = ""
                dt_Service.Rows.Add(dr_Service)
                dt_Service.AcceptChanges()
                i += 1
            Next

            strReturn = JsonConvert.SerializeObject(dt_Service)
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
    Public Shared Function AuditTrail(ByVal vServiceCode As String) As String
        Dim ObjCommon As New clsCommon
        Dim objHelp As WS_HelpDbTable.WS_HelpDbTable = ObjCommon.GetHelpDbTableRef()
        Dim wStr As String = String.Empty
        Dim ds_Servicemst As New DataSet
        Dim estr As String = String.Empty
        Dim strReturn As String = String.Empty

        Dim dtServiceHistory As New DataTable
        Dim drAuditTrail As DataRow
        Dim i As Integer = 1
        Dim dt As New DataTable
        Try

            wStr = " vServiceCode = '" + vServiceCode + "' Order by nServiceHistoryNo DESC"
            If Not objHelp.GetView_ServiceMstHistory(wStr, WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, ds_Servicemst, estr) Then
                Throw New Exception(estr)
            End If

            If Not dtServiceHistory Is Nothing Then
                dtServiceHistory.Columns.Add("SrNo")
                dtServiceHistory.Columns.Add("ServiceName")
                dtServiceHistory.Columns.Add("Remarks")
                dtServiceHistory.Columns.Add("ModifyBy")
                dtServiceHistory.Columns.Add("ModifyOn")
            End If
            dt = ds_Servicemst.Tables(0)
            Dim dv As New DataView(dt)

            For Each dr As DataRow In dv.ToTable.Rows
                drAuditTrail = dtServiceHistory.NewRow()
                drAuditTrail("SrNo") = i
                drAuditTrail("ServiceName") = dr("vServiceName").ToString()
                drAuditTrail("Remarks") = dr("vRemark").ToString()
                drAuditTrail("ModifyBy") = dr("ModifyBy").ToString()
                drAuditTrail("ModifyOn") = Convert.ToString(dr("dModifyOn"))
                dtServiceHistory.Rows.Add(drAuditTrail)
                dtServiceHistory.AcceptChanges()
                i += 1
            Next
            strReturn = JsonConvert.SerializeObject(dtServiceHistory)
            Return strReturn
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

#End Region

End Class
