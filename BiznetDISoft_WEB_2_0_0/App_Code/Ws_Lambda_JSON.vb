Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Linq
Imports System.Collections.Generic

<System.CodeDom.Compiler.GeneratedCode("System.Xml", "2.0.50727.42")> _
<System.SerializableAttribute()> _
<System.Xml.Serialization.XmlTypeAttribute()> _
Public Enum DataObjOpenSaveModeEnum 'Enumeration Declaration for Open and Save Mode from Menu Page
    DataObjOpenMode_None = 0
    DataObjOpenMode_Add = 1
    DataObjOpenMode_Edit = 2
    DataObjOpenMode_Delete = 3
    DataObjOpenMode_View = 4
    DataObjOpenMode_Rearrange = 5
End Enum

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class Ws_Lambda_JSON
    Inherits System.Web.Services.WebService
#Region "VARIABLE DECLARATION"
    Private Shared objCommon As New clsCommon
    Private Shared objLambda As WS_Lambda.WS_Lambda = objCommon.GetHelpDbLambdaRef()
    Private Shared objDataLogic As New clsDataLogic
    Private Shared objHelp As WS_HelpDbTable.WS_HelpDbTable = objCommon.GetHelpDbTableRef()

#End Region
    <WebMethod()> _
    Public Function HelloWorld() As String
        Return "Hello World"
    End Function

#Region "Save CDMS Sheduling"

    <WebMethod()> _
    Public Function Save_WorkSpaceScreeningScheduleHdrDtl(ByVal dataFinal As Object) As String
        Dim ds_hdr As New System.Data.DataSet
        Dim ds_dtl As New System.Data.DataSet
        Dim ds_blankdtl As New System.Data.DataSet
        Dim ds_Extradtl As New System.Data.DataSet
        Dim ds_Save As New System.Data.DataSet
        Dim HdrRow As DataRow
        Dim DtlRow As DataRow
        Dim Estr As String = String.Empty
        Dim SessionUserID As String = String.Empty

        Try
            ds_hdr = objHelp.GetResultSet("SELECT * FROM  WorkSpaceScreeningScheduleHdr WHERE 1=2", "WorkSpaceScreeningScheduleHdr")

            SessionUserID = dataFinal(0)(0)("iModifyBy").ToString
            For indexHdr As Integer = 0 To dataFinal(0).length - 1
                HdrRow = ds_hdr.Tables(0).NewRow
                If dataFinal(2)(0)("mode").ToString = "Edit" Then
                    HdrRow("nWorkSpaceScreeningScheduleHdrId") = dataFinal(0)(indexHdr)("nWorkSpaceScreeningScheduleHdrId").ToString
                ElseIf dataFinal(2)(0)("mode").ToString = "Add" Then
                    HdrRow("nWorkSpaceScreeningScheduleHdrId") = System.DBNull.Value
                End If
                HdrRow("vWorkSpaceId") = dataFinal(0)(indexHdr)("vWorkSpaceId").ToString
                HdrRow("dScheduledate") = dataFinal(0)(indexHdr)("dScheduledate").ToString
                HdrRow("dFromTime") = dataFinal(0)(indexHdr)("dFromTime").ToString
                HdrRow("dToTime") = dataFinal(0)(indexHdr)("dToTime").ToString
                HdrRow("nNoofSubject") = dataFinal(0)(indexHdr)("nNoofSubject").ToString
                HdrRow("vLocationCode") = dataFinal(0)(indexHdr)("vLocationCode").ToString
                HdrRow("vRemarks") = dataFinal(0)(indexHdr)("vRemarks").ToString
                HdrRow("cStatusIndi") = dataFinal(0)(indexHdr)("cStatusIndi").ToString
                HdrRow("iModifyBy") = dataFinal(0)(indexHdr)("iModifyBy").ToString
                HdrRow("dModifyOn") = System.DBNull.Value
                HdrRow("dTimelength") = dataFinal(0)(indexHdr)("dTimelength").ToString
                ds_hdr.Tables(0).Rows.Add(HdrRow)
                ds_hdr.AcceptChanges()
                ds_hdr.Tables(0).TableName = "WorkSpaceScreeningScheduleHdr"
            Next

            ds_Save.Tables.Add(ds_hdr.Tables(0).Copy())

            ds_blankdtl = objHelp.GetResultSet("SELECT * FROM  WorkSpaceScreeningScheduleDtl WHERE 1=2", "WorkSpaceScreeningScheduleDtl")

            ds_dtl = ds_blankdtl.Copy

            For indexDtl As Integer = 0 To dataFinal(1).length - 1
                DtlRow = ds_dtl.Tables(0).NewRow
                If dataFinal(2)(0)("mode").ToString = "Edit" Then
                    DtlRow("nWorkSpaceScreeningScheduleDtlId") = IIf(dataFinal(1)(indexDtl)("nWorkSpaceScreeningScheduleDtlId").ToString = "", System.DBNull.Value, dataFinal(1)(indexDtl)("nWorkSpaceScreeningScheduleDtlId").ToString)
                    DtlRow("nWorkSpaceScreeningScheduleHdrId") = dataFinal(1)(indexDtl)("nWorkSpaceScreeningScheduleHdrId").ToString
                ElseIf dataFinal(2)(0)("mode").ToString = "Add" Then
                    DtlRow("nWorkSpaceScreeningScheduleDtlId") = System.DBNull.Value
                    DtlRow("nWorkSpaceScreeningScheduleHdrId") = System.DBNull.Value
                End If
                DtlRow("nWorkspaceScreeningScheduleNo") = dataFinal(1)(indexDtl)("nWorkspaceScreeningScheduleNo").ToString
                DtlRow("dStartTime") = dataFinal(1)(indexDtl)("dStartTime").ToString
                DtlRow("vSubjectId") = dataFinal(1)(indexDtl)("vSubjectId").ToString
                If dataFinal(2)(0)("mode").ToString = "Edit" Then
                    DtlRow("iTranNo") = IIf(dataFinal(1)(indexDtl)("iTranNo").ToString = "", System.DBNull.Value, dataFinal(1)(indexDtl)("iTranNo").ToString)
                ElseIf dataFinal(2)(0)("mode").ToString = "Add" Then
                    DtlRow("iTranNo") = System.DBNull.Value
                End If
                DtlRow("vRemarks") = dataFinal(1)(indexDtl)("vRemarks").ToString
                DtlRow("cStatusIndi") = dataFinal(1)(indexDtl)("cStatusIndi").ToString
                DtlRow("iModifyBy") = dataFinal(1)(indexDtl)("iModifyBy").ToString
                DtlRow("dModifyOn") = System.DBNull.Value
                DtlRow("cStatus") = dataFinal(1)(indexDtl)("cStatus").ToString
                ds_dtl.Tables(0).Rows.Add(DtlRow)
                ds_dtl.AcceptChanges()
                ds_dtl.Tables(0).TableName = "WorkSpaceScreeningScheduleDtl"
            Next

            ds_Save.Tables.Add(ds_dtl.Tables(0).Copy())
            If dataFinal.length = 4 And dataFinal(2)(0)("mode").ToString = "Edit" Then

                ds_Extradtl = ds_blankdtl.Copy

                For indexDtl As Integer = 0 To dataFinal(3).length - 1
                    DtlRow = ds_Extradtl.Tables(0).NewRow
                    DtlRow("nWorkSpaceScreeningScheduleDtlId") = IIf(dataFinal(3)(indexDtl)("nWorkSpaceScreeningScheduleDtlId").ToString = "", System.DBNull.Value, dataFinal(3)(indexDtl)("nWorkSpaceScreeningScheduleDtlId").ToString)
                    DtlRow("nWorkSpaceScreeningScheduleHdrId") = IIf(dataFinal(3)(indexDtl)("nWorkSpaceScreeningScheduleHdrId").ToString = "", System.DBNull.Value, dataFinal(3)(indexDtl)("nWorkSpaceScreeningScheduleHdrId").ToString)
                    DtlRow("nWorkspaceScreeningScheduleNo") = dataFinal(3)(indexDtl)("nWorkspaceScreeningScheduleNo").ToString
                    DtlRow("dStartTime") = dataFinal(3)(indexDtl)("dStartTime").ToString
                    DtlRow("vSubjectId") = dataFinal(3)(indexDtl)("vSubjectId").ToString
                    DtlRow("iTranNo") = IIf(dataFinal(3)(indexDtl)("iTranNo").ToString = "", System.DBNull.Value, dataFinal(3)(indexDtl)("iTranNo").ToString)
                    DtlRow("vRemarks") = dataFinal(3)(indexDtl)("vRemarks").ToString
                    DtlRow("cStatusIndi") = dataFinal(3)(indexDtl)("cStatusIndi").ToString
                    DtlRow("iModifyBy") = dataFinal(3)(indexDtl)("iModifyBy").ToString
                    DtlRow("dModifyOn") = System.DBNull.Value
                    DtlRow("cStatus") = dataFinal(3)(indexDtl)("cStatus").ToString
                    ds_Extradtl.Tables(0).Rows.Add(DtlRow)
                    ds_Extradtl.AcceptChanges()
                    ds_Extradtl.Tables(0).TableName = "ExtraWorkSpaceScreeningScheduleDtl"
                Next

                ds_Save.Tables.Add(ds_Extradtl.Tables(0).Copy())

            End If
            If dataFinal(2)(0)("mode").ToString = "Add" Then
                If Not objLambda.SaveWorkSpaceScreeningScheduleHdrDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_Save, True, SessionUserID, Estr) Then
                    Return False
                    Exit Function
                End If
            ElseIf dataFinal(2)(0)("mode").ToString = "Edit" Then

                If Not objLambda.SaveWorkSpaceScreeningScheduleHdrDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_Save, False, SessionUserID, Estr) Then
                    Return False
                    Exit Function
                End If
            End If


            Return "success"

        Catch ex As Exception

            Return "error"
        End Try
    End Function

    <WebMethod()> _
    Public Function Save_WorkSpaceScreeningScheduleDtl(ByVal DeletedataDtl As Object) As String

        Dim ds_dtl As New System.Data.DataSet
        Dim DtlRow As DataRow
        Dim Estr As String = String.Empty
        Dim SessionUserID As String = String.Empty
        Dim ds_SubjectDtl As New System.Data.DataSet
        Dim dt_SubjectDtl As New DataTable

        Try
            SessionUserID = DeletedataDtl(0)("iModifyBy").ToString
            ds_dtl = objHelp.GetResultSet("SELECT * FROM  WorkSpaceScreeningScheduleDtl WHERE 1=2", "WorkSpaceScreeningScheduleDtl")


            For indexDtl As Integer = 0 To DeletedataDtl.length - 1
                DtlRow = ds_dtl.Tables(0).NewRow
                DtlRow("nWorkSpaceScreeningScheduleDtlId") = DeletedataDtl(indexDtl)("nWorkSpaceScreeningScheduleDtlId").ToString
                DtlRow("nWorkSpaceScreeningScheduleHdrId") = DeletedataDtl(indexDtl)("nWorkSpaceScreeningScheduleHdrId").ToString
                DtlRow("nWorkspaceScreeningScheduleNo") = DeletedataDtl(indexDtl)("nWorkspaceScreeningScheduleNo").ToString
                DtlRow("dStartTime") = DeletedataDtl(indexDtl)("dStartTime").ToString
                DtlRow("vSubjectId") = DeletedataDtl(indexDtl)("vSubjectId").ToString
                DtlRow("iTranNo") = System.DBNull.Value
                DtlRow("vRemarks") = DeletedataDtl(indexDtl)("vRemarks").ToString
                DtlRow("cStatusIndi") = DeletedataDtl(indexDtl)("cStatusIndi").ToString
                DtlRow("iModifyBy") = DeletedataDtl(indexDtl)("iModifyBy").ToString
                DtlRow("dModifyOn") = System.DBNull.Value
                DtlRow("cStatus") = DeletedataDtl(indexDtl)("cStatus").ToString
                ds_dtl.Tables(0).Rows.Add(DtlRow)
                ds_dtl.AcceptChanges()

            Next


            ' Change by Jeet Patel on 31-March-2015 to restrict user to perform book or delete subject at same time
            If DeletedataDtl(0)("mode").ToString = "DELETE" Then
                If Not objLambda.Save_WorkSpaceScreeningScheduleDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete, ds_dtl, SessionUserID, Estr) Then
                    If Not Estr.ToString() <> "" Then
                        Return "Success"
                    Else
                        Return "NotSave"
                    End If
                    Exit Function
                End If
                Return "Success"

            ElseIf DeletedataDtl(0)("mode").ToString = "EDIT" Then
                ' Added By Jeet Patel on 01-April-2015 to check Status of subject
                ds_SubjectDtl = objHelp.GetResultSet("select * from View_CDMSSubjectDetails where vSubjectID= '" + DeletedataDtl(0)("vSubjectId").ToString() + "'", "View_CDMSSubjectDetails")
                dt_SubjectDtl = ds_SubjectDtl.Tables(0).Copy()

                If (dt_SubjectDtl.Rows.Count > 0) Then
                    If (dt_SubjectDtl.Rows(0)("cStatus").ToString = "BO") Then
                        Return "NotBook"
                    End If
                End If
                '=======================================================================

                If Not objLambda.Save_WorkSpaceScreeningScheduleDtl(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_dtl, SessionUserID, Estr) Then
                    If Not Estr.ToString() <> "" Then
                        Return "Success"
                    Else
                        Return "NotSave"
                    End If
                    Exit Function
                End If
                Return "Success"
            End If
            '============================================================================

        Catch ex As Exception
            Return "error"
        End Try
    End Function


#End Region '=============Added By Debashis=================== 

#Region "insert_DataEntryControl "
    <WebMethod()> _
    Public Function insert_DataEntryControl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                        ByVal ds_DataentryControl As Object) As String
        Dim Sql_DataSet As New DataSet
        Dim estr As String = String.Empty
        Dim dr As DataRow
        Dim Return_Val As Char
        Try
            If Not objHelp.GetData("DataEntrycontrol", "*", " 1=2", WS_HelpDbTable.DataRetrievalModeEnum.DataTable_WithWhereCondition, Sql_DataSet, estr) Then
                Throw New Exception("Error while getting blank structure")
            End If

            dr = Sql_DataSet.Tables(0).NewRow()
            dr("vWorkspaceId") = ds_DataentryControl(0)("vWorkspaceId").ToString
            dr("iNodeId") = ds_DataentryControl(0)("iNodeId").ToString
            dr("vSubjectId") = ds_DataentryControl(0)("vSubjectId").ToString
            dr("iModifyBy") = ds_DataentryControl(0)("iModifyBy").ToString
            dr("iWorkFlowStageId") = ds_DataentryControl(0)("iWorkflowStageId").ToString
            Sql_DataSet.Tables(0).Rows.Add(dr)
            Sql_DataSet.AcceptChanges()

            If Not objLambda.insert_DataEntryControl(Choice_1, Sql_DataSet, estr, Return_Val) Then
                Throw New Exception("Error While Executing Edit Checks....Proc_ExecuteEditChecks")
            End If
            insert_DataEntryControl = "You have Successfully Exit"

        Catch ex As Exception
            Return ex.ToString = "Error While Exit"
        End Try
    End Function

#End Region '===============Added by Megha=============

    <WebMethod()> _
    Public Function Save_MedexScreeningDtl(ByVal Editdata As Object) As String

        Dim ds_dtl As New System.Data.DataSet
        Dim DtlRow As DataRow
        Dim Estr As String = String.Empty
        Dim SessionUserID As String = String.Empty
        Dim ds_ScreeningHdr As New DataSet


        Try
            SessionUserID = Editdata(0)("iModifyBy").ToString

            If Editdata(0)("vWorkSpaceID").ToString = "0000000000" Then
                ds_dtl = objHelp.GetResultSet("SELECT * FROM  MedexScreeningdtl WHERE 1=2", "MedexScreeningdtl")
                For indexDtl As Integer = 0 To Editdata.length - 1
                    DtlRow = ds_dtl.Tables(0).NewRow
                    DtlRow("nMedExScreeningHdrNo") = Editdata(indexDtl)("nMedExScreeningHdrNo").ToString
                    DtlRow("vMedExCode") = Editdata(indexDtl)("vMedExCode").ToString
                    DtlRow("vMedExResult") = Editdata(indexDtl)("vMedExResult").ToString
                    DtlRow("iModifyBy") = SessionUserID
                    DtlRow("cStatusIndi") = Editdata(indexDtl)("cStatusIndi").ToString
                    DtlRow("vRemarks") = Editdata(indexDtl)("vRemarks").ToString
                    ds_dtl.Tables(0).Rows.Add(DtlRow)
                    ds_dtl.AcceptChanges()
                Next
                If Not objLambda.Save_MedExScreeningDtlOnly(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_dtl, Estr) Then
                    Return "error"
                    Exit Function
                End If
            Else
                ds_dtl = objHelp.GetResultSet("SELECT * FROM  MedexWorkspaceScreeningDtl WHERE 1=2", "MedexWorkspaceScreeningDtl")
                ds_ScreeningHdr = objHelp.GetResultSet("SELECT * FROM  MedexWorkspaceScreeningHdr WHERE vWorKspaceId = '" + Editdata(0)("vWorkSpaceID").ToString() + "' AND nMedExScreeningHdrNo = '" + Editdata(0)("nMedExScreeningHdrNo").ToString() + "'", "MedexWorkspaceScreeningDtl")
                For indexDtl As Integer = 0 To Editdata.length - 1
                    DtlRow = ds_dtl.Tables(0).NewRow
                    DtlRow("nMedExWorkSpaceScreeningHdrNo") = ds_ScreeningHdr.Tables(0).Rows(0)("nMedWorkSpaceExScreeningHdrNo")
                    DtlRow("vMedExCode") = Editdata(indexDtl)("vMedExCode").ToString
                    DtlRow("vMedExResult") = Editdata(indexDtl)("vMedExResult").ToString
                    DtlRow("iModifyBy") = SessionUserID
                    DtlRow("cStatusIndi") = Editdata(indexDtl)("cStatusIndi").ToString
                    DtlRow("vRemarks") = Editdata(indexDtl)("vRemarks").ToString
                    ds_dtl.Tables(0).Rows.Add(DtlRow)
                    ds_dtl.AcceptChanges()
                Next
                If Not objLambda.Save_MedexWorkspaceScreeningDtlOnly(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, ds_dtl, Estr) Then
                    Return "error"
                    Exit Function
                End If
            End If

            Return "Sucess"
        Catch ex As Exception
            Return "error"
        End Try
    End Function

#Region "Save_SubjectDtlCDMSStatus"
    <WebMethod()> _
    Public Function Save_SubjectDtlCDMSStatus(ByVal changeStatusDtl As Object) As String

        Dim ds_dtl As New System.Data.DataSet
        Dim DtlRow As DataRow
        Dim Estr As String = String.Empty
        Dim SessionUserID As String = String.Empty
        Dim eStr_retu As String = String.Empty

        Try
            SessionUserID = changeStatusDtl(0)("iModifyBy").ToString
            ds_dtl = objHelp.GetResultSet("SELECT * FROM  SubjectDtlCDMSStatus WHERE 1=2", "SubjectDtlCDMSStatus")


            For indexDtl As Integer = 0 To changeStatusDtl.length - 1
                DtlRow = ds_dtl.Tables(0).NewRow

                DtlRow("vSubjectId") = changeStatusDtl(indexDtl)("vSubjectId").ToString
                DtlRow("vWorkSpaceId") = changeStatusDtl(indexDtl)("vWorkSpaceId").ToString
                DtlRow("iTranNo") = 0 'Convert.ToInt32((changeStatusDtl(indexDtl)("iTranNo")))
                DtlRow("cStatus") = changeStatusDtl(indexDtl)("cStatus").ToString
                DtlRow("iModifyBy") = changeStatusDtl(indexDtl)("iModifyBy").ToString
                DtlRow("dModifyOn") = System.DBNull.Value
                DtlRow("cStatusIndi") = changeStatusDtl(indexDtl)("cStatusIndi").ToString
                '    DtlRow("mode") = changeStatusDtl(indexDtl)("mode").ToString

                ds_dtl.Tables(0).Rows.Add(DtlRow)
                ds_dtl.AcceptChanges()

            Next
            'If changeStatusDtl(0)("mode").ToString = "DELETE" Then

            If Not objLambda.Save_SubjectDtlCDMSStatus(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_dtl, eStr_retu) Then
                Return "error"
                Exit Function
            End If
            Return "success"
        Catch ex As Exception
            Return "error"
        End Try

    End Function
#End Region '===== Added By Pratik Soni =====

End Class
