Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports SS.Web.Services

<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class LabIntegration
    Inherits sWebService

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function HelloWorld() As String
        Return "Hello World"
    End Function

#Region "Save_MedExMstFromTestMst"

    ' <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    '       Public Function Save_MedExMstFromTestMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
    '                                                ByVal TestCode As String, _
    '                                                ByVal TestName As String, _
    '                                                ByVal UOM As String, _
    '                                                ByVal UserId As String, _
    '                                                ByRef eStr_Retu As String) As Boolean
    '    Dim objMaster As New ClsMaster
    '    Dim Ds_MedexMst As New DataSet
    '    Dim objhelp As New clsHelpDbTable
    '    Dim dr As DataRow

    '    If Not objhelp.GetMedExMst("", DataRetrievalModeEnum.DataTable_Empty, Ds_MedexMst, eStr_Retu) Then
    '        Return False
    '    End If

    '    dr = Ds_MedexMst.Tables(0).NewRow
    '    dr("vMedexCode") = TestCode
    '    dr("vMedExDesc") = TestName
    '    dr("vUOM") = UOM
    '    dr("vMedExType") = "Import"

    '    If Choice_1 = DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
    '        dr("cStatusIndi") = "N"
    '    ElseIf Choice_1 = DataObjOpenSaveModeEnum.DataObjOpenMode_Edit Then
    '        dr("cStatusIndi") = "E"
    '    ElseIf Choice_1 = DataObjOpenSaveModeEnum.DataObjOpenMode_Delete Then
    '        dr("cStatusIndi") = "D"
    '    End If

    '    Ds_MedexMst.Tables(0).Rows.Add(dr)

    '    Ds_MedexMst.Tables(0).AcceptChanges()

    '    Save_MedExMstFromTestMst = objMaster.Save_Masters(Choice_1, MasterEntriesEnum.MasterEntriesEnum_MedExMst, Ds_MedexMst, UserId, eStr_Retu)
    'End Function

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_MedExMst(ByVal Choice As Integer, _
                                         ByVal Dt_MedExMst As DataTable, _
                                         ByVal UserId As String, _
                                         ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim Ds_MedExMst As New DataSet
        Ds_MedExMst.Tables.Add(Dt_MedExMst.Copy())

        Save_MedExMst = objMaster.Save_Masters(Choice, MasterEntriesEnum.MasterEntriesEnum_MedExMst, Ds_MedExMst, UserId, eStr_Retu)
    End Function

#End Region

#Region "Save_SampleMedExDetailFromTestResult"

    ' <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    'Public Function Save_SampleMedExDetailFromTestResult(ByVal SampleId As String, _
    '                                                    ByVal TestCode As String, _
    '                                                    ByVal TestResult As String, _
    '                                                    ByVal TestMachineDetails As String, _
    '                                                    ByVal TestDoneBy As String, _
    '                                                    ByVal TestDoneOn As String, _
    '                                                    ByVal TestApprovedBy As String, _
    '                                                    ByVal TestApprovedOn As String, _
    '                                                    ByVal UserId As String, _
    '                                                    ByRef eStr_Retu As String) As Boolean

    '    Dim objMaster As New ClsMaster
    '    Dim Ds_SampleMedExDetail As New DataSet
    '    Dim objhelp As New clsHelpDbTable
    '    Dim dr As DataRow
    '    Dim Ds_Sample As New DataSet
    '    Dim SampleNo As Integer
    '    Dim objDtLogic As New ClsDataLogic_New()

    '    If SampleId.Trim() = "" Then
    '        eStr_Retu = "Please Give Valid SampleId"
    '        Return False
    '    ElseIf TestCode.Trim() = "" Then
    '        eStr_Retu = "Please Give Valid Test Code"
    '        Return False
    '    ElseIf TestResult.Trim() = "" Then
    '        eStr_Retu = "Please Give Valid Test Result"
    '        Return False
    '    ElseIf TestMachineDetails.Trim() = "" Then
    '        eStr_Retu = "Please Give Valid Test Machine Details"
    '        Return False
    '    ElseIf TestDoneBy.Trim() = "" Then
    '        eStr_Retu = "Please Give Valid Value For Test Done By"
    '        Return False
    '    ElseIf TestDoneOn.Trim() = "" Then
    '        eStr_Retu = "Please Give Valid Value For Test Done On"
    '        Return False
    '    ElseIf TestApprovedBy.Trim() = "" Then
    '        eStr_Retu = "Please Give Valid Value For Test Approved By"
    '        Return False
    '    ElseIf TestApprovedOn.Trim() = "" Then
    '        eStr_Retu = "Please Give Valid Value For Test Approved On"
    '        Return False
    '    End If


    '    If Not objhelp.GetSampleMedExDetail("", DataRetrievalModeEnum.DataTable_Empty, Ds_SampleMedExDetail, eStr_Retu) Then
    '        Return False
    '    End If

    '    If Not objhelp.GetSampleDetail("vSampleId='" & SampleId.Trim() & "'", DataRetrievalModeEnum.DataTable_Empty, Ds_Sample, eStr_Retu) Then
    '        Return False
    '    End If

    '    If Ds_Sample.Tables(0).Rows.Count <= 0 Then
    '        eStr_Retu = "Please Give Valid SampleId"
    '        Return False
    '    End If

    '    SampleNo = Ds_Sample.Tables(0).Rows(0).Item("nSampleId")

    '    Ds_Sample.Tables(0).Copy.DefaultView.RowFilter = "vMedexCode='" & TestCode & "'"

    '    If Ds_Sample.Tables(0).Copy.DefaultView.ToTable.Rows.Count <= 0 Then
    '        eStr_Retu = "This TestCode is Defined for Given Sample"
    '        Return False
    '    End If

    '    dr = Ds_SampleMedExDetail.Tables(0).NewRow
    '    'nSampleId, vMedExCode, vMedExResult, vMachineDetails, iMedExDoneBy, dMedExDoneOn, iMedExApprovedBy, dMedExApprovedOn, iModifyBy, dModifyOn, cStatusIndi

    '    dr("nSampleId") = SampleNo
    '    dr("vMedexCode") = TestCode
    '    dr("vMedExResult") = TestResult
    '    dr("vMachineDetails") = TestMachineDetails
    '    dr("iMedExDoneBy") = TestDoneBy
    '    dr("dMedExDoneOn") = TestDoneOn
    '    dr("iMedExApprovedBy") = TestApprovedBy
    '    dr("dMedExApprovedOn") = TestApprovedOn
    '    dr("cStatusIndi") = "E"

    '    Ds_SampleMedExDetail.Tables(0).Rows.Add(dr)
    '    Ds_SampleMedExDetail.Tables(0).AcceptChanges()

    '    Save_SampleMedExDetailFromTestResult = objMaster.Save_SampleMedExDetail(DataObjOpenSaveModeEnum.DataObjOpenMode_Edit, Ds_SampleMedExDetail, objDtLogic, UserId, eStr_Retu)
    'End Function

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SampleMedEXDetail(ByVal Choice As Integer, _
                                            ByVal Dt_SampleMedEXDetail As DataTable, _
                                            ByVal Company_Parameter As Integer, _
                                            ByVal UserId As String, _
                                            ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objHelp As New clsHelpDbTable
        Dim objDtLogic As New ClsDataLogic_New()
        Dim Ds_SampleMedEXDetail As New DataSet
        Dim eStr_ReturnOfMedex As String = ""


        '' Get Lab and Medex matrix details 
        Dim Dv_LabMachineMedexMatrix As New DataView
        Dim Ds_LabMachineMedexMatrix As New DataSet

        '' Get Sample Details 
        Dim Ds_SampleTypeDetail As New DataSet
        Dim Dt_SampleMedexDetail_Save As New DataTable

        '' Get Sample Medex Detial  
        Dim Ds_SampleMedexDetail_Valid As New DataSet


        Dim nSampleId As Integer
        Dim Dr_DataRow As DataRow
        Dim dr_New As DataRow

        Dim wStr As String = ""
        Dim MedexCount As Integer
        Dim MachineNo As Integer = 0
        Dim vSampleId As String = ""

        Try


            If Not objHelp.getLabMachineMedexMatrix("", DataRetrievalModeEnum.DataTable_AllRecords, Ds_LabMachineMedexMatrix, eStr_Retu) Then
                Return False
            End If

            If Not objHelp.GetSampleMedExDetail("", DataRetrievalModeEnum.DataTable_Empty, _
                    Ds_SampleMedEXDetail, eStr_Retu) Then
                Return False
            End If

            Dt_SampleMedexDetail_Save = Ds_SampleMedEXDetail.Tables(0)
            Dim strBarcode As String = Dt_SampleMedEXDetail.Rows(0).Item("vMachineSampleID").ToString.Trim

            If strBarcode.Length > 8 And strBarcode.Length < 10 Then
                ''strBarcode = strBarcode.Remove(8)

                If Not objHelp.View_SampleTypeDetail("vSampleBarCode = '" & strBarcode & "'", DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_SampleTypeDetail, eStr_Retu) Then
                    Return False
                End If

            End If

            ''If strBarcode.Length < 9 Then
            ''    If Not objHelp.View_SampleTypeDetail("vSampleBarCode = '" & strBarcode & "%'", DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_SampleTypeDetail, eStr_Retu) Then
            ''        Return False
            ''    End If
            ''End If


            If strBarcode.Length > 10 Then
                If Not objHelp.View_SampleTypeDetail("BarcodeId like  '" & strBarcode & "%'", DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_SampleTypeDetail, eStr_Retu) Then
                    Return False
                End If
                Try
                    vSampleId = Ds_SampleTypeDetail.Tables(0).Rows(0)("vSampleBarCode")
                Catch ex As Exception

                End Try

            End If


            If Ds_SampleTypeDetail.Tables(0).Rows.Count > 0 Then
                nSampleId = Ds_SampleTypeDetail.Tables(0).Rows(0).Item("nSampleId")
            Else
                eStr_Retu += "Sample Id Not Found " + Dt_SampleMedEXDetail.Rows(0).Item("vMachineSampleID")
                Return False
            End If

            For Each Dr_DataRow In Dt_SampleMedEXDetail.Rows
                Dv_LabMachineMedexMatrix = Nothing
                Dv_LabMachineMedexMatrix = New DataView
                Dv_LabMachineMedexMatrix = Ds_LabMachineMedexMatrix.Tables(0).Copy.DefaultView()

                If Company_Parameter = 2 Then
                    If Dr_DataRow("vMachineDetails") = "Vitros 950" Then
                        MachineNo = "1"
                    ElseIf Dr_DataRow("vMachineDetails") = "Vitros EciQ" Then
                        MachineNo = "4"
                    ElseIf Dr_DataRow("vMachineDetails") = "Vitros Eci" Then
                        MachineNo = "3"
                    ElseIf Dr_DataRow("vMachineDetails") = "Vitros 1.5 FS" Then
                        MachineNo = "2"
                    ElseIf Dr_DataRow("vMachineDetails") = "Best 2000" Then
                        MachineNo = "6"
                    ElseIf Dr_DataRow("vMachineDetails") = "Cobas U 411" Then
                        MachineNo = "8"
                    ElseIf Dr_DataRow("vMachineDetails") = "Immulite" Then
                        MachineNo = "5"
                    ElseIf Dr_DataRow("vMachineDetails") = "SRS 1000 II" Then
                        MachineNo = "12"
                    ElseIf Dr_DataRow("vMachineDetails") = "Sysmex 2000i" Then
                        MachineNo = "9"
                    ElseIf Dr_DataRow("vMachineDetails") = "Sysmex 2000i2" Then
                        MachineNo = "13"
                    ElseIf Dr_DataRow("vMachineDetails") = "Urisys 2400" Then
                        MachineNo = "7"
                    ElseIf Dr_DataRow("vMachineDetails") = "DavinciQuattro" Then
                        MachineNo = "14"
                    ElseIf Dr_DataRow("vMachineDetails") = "COBAS_E411" Then
                        MachineNo = "15"
                    ElseIf Dr_DataRow("vMachineDetails") = "STA_COMPACT" Then
                        MachineNo = "16"
                    ElseIf Dr_DataRow("vMachineDetails") = "Dsx Elisa" Then
                        MachineNo = "19"
                        ''Added by Vivek Patel for BD FACSCanto II
                    ElseIf Dr_DataRow("vMachineDetails") = "BD FACSCanto II" Then
                        MachineNo = "20"
                    End If
                End If

                If Company_Parameter = 2 Then
                    Dv_LabMachineMedexMatrix.RowFilter = "vLabMachineTestCode = '" & Dr_DataRow("vMachineMedExCode") & "' And" & _
                                                                        " nLabMachineNo=" & MachineNo.ToString
                ElseIf Company_Parameter = 1 Then
                    Dv_LabMachineMedexMatrix.RowFilter = "vLabMachineTestCode = '" & Dr_DataRow("vMachineMedExCode") & "'"
                End If



                If Dv_LabMachineMedexMatrix.ToTable().Rows.Count <= 0 Then
                    eStr_Retu += "Machine Test Mapping Not Found For " + Dr_DataRow("vMachineMedExCode") & vbCrLf
                    'Return False
                End If


                If Dv_LabMachineMedexMatrix.ToTable().Rows.Count > 0 Then

                    For ctr As Integer = 0 To Dv_LabMachineMedexMatrix.ToTable.Rows.Count - 1

                        If Not objHelp.GetSampleMedExDetail("nSampleId = " & nSampleId.ToString.Trim & _
                                " And vMedexCode = '" & Dv_LabMachineMedexMatrix.ToTable.Rows(ctr).Item("vMedExCode") & "'", _
                                DataRetrievalModeEnum.DataTable_WithWhereCondition, Ds_SampleMedexDetail_Valid, eStr_Retu) Then
                            eStr_Retu += "Sample MedEx Detail Not Found"
                            'Return False

                        End If

                        If Ds_SampleMedexDetail_Valid.Tables(0).Rows.Count > 0 Then
                            'Added By Ankit Shah

                            wStr = "select count(nSampleId) from SampleMedexDetail where nSampleId=" + nSampleId.ToString.Trim
                            wStr += " And vMedexCode ='" & Dv_LabMachineMedexMatrix.ToTable.Rows(ctr).Item("vMedExCode") & "'"
                            wStr += " And cApprovedFlag not in('N','X','R')"

                            MedexCount = objHelp.ExecuteQuery_Scalar(wStr)
                            'Ended

                            If MedexCount <= 0 Then

                                If Ds_SampleMedexDetail_Valid.Tables(0).Rows.Count <= 0 Then
                                    eStr_Retu += " For Sample Id " + Dt_SampleMedEXDetail.Rows(0).Item("vMachineSampleID") + _
                                                 " Test " + Dr_DataRow("vMachineMedExCode") + " Does Not Exist "
                                    'Return False

                                End If

                                dr_New = Dt_SampleMedexDetail_Save.NewRow

                                dr_New("nSampleMedexNo") = 0
                                dr_New("nSampleId") = nSampleId
                                dr_New("vMedExCode") = Dv_LabMachineMedexMatrix.ToTable.Rows(ctr).Item("vMedExCode")
                                dr_New("vMachineDetails") = Dr_DataRow("vMachineDetails")
                                dr_New("iMedExDoneBy") = UserId
                                dr_New("dMedExDoneOn") = System.DateTime.Today.ToString("dd-MMM-yyyy hh:mm:ss")
                                dr_New("iModifyBy") = UserId
                                dr_New("cStatusIndi") = "E"
                                dr_New("itranNo") = 0
                                dr_New("iRepeatationNo") = 0
                                Try
                                    dr_New("vSampleId") = vSampleId
                                Catch ex As Exception

                                End Try

                                If (Dr_DataRow("vMachineMedExCode") = "017" Or Dr_DataRow("vMachineMedExCode") = "023" _
                                            Or Dr_DataRow("vMachineMedExCode") = "024") _
                                            And _
                                            (Dr_DataRow("vMachineDetails") = "Vitros EciQ" Or _
                                             Dr_DataRow("vMachineDetails") = "Vitros Eci") Then

                                    If Dr_DataRow("vMachineMedExResult") = "No Result" Then
                                        dr_New("vMedExResult") = "No Result"
                                    ElseIf Val(Dr_DataRow("vMachineMedExResult")) >= 1.0 Then
                                        dr_New("vMedExResult") = "Reactive"
                                    ElseIf Val(Dr_DataRow("vMachineMedExResult")) < 0.9 Then
                                        dr_New("vMedExResult") = "Non-Reactive"
                                    ElseIf Val(Dr_DataRow("vMachineMedExResult")) >= 0.9 And Val(Dr_DataRow("vMachineMedExResult")) < 1.0 Then
                                        dr_New("vMedExResult") = "Indeterminate"
                                    End If
                                    dr_New("vRemarks") = Dr_DataRow("vMachineMedExResult")

                                ElseIf (Dr_DataRow("vMachineMedExCode") = "022" Or Dr_DataRow("vMachineMedExCode") = "020") And _
                                       (Dr_DataRow("vMachineDetails") = "Vitros EciQ" Or _
                                       Dr_DataRow("vMachineDetails") = "Vitros Eci") Then

                                    If Dr_DataRow("vMachineMedExResult") = "No Result" Then
                                        dr_New("vMedExResult") = "No Result"
                                    ElseIf Val(Dr_DataRow("vMachineMedExResult")) >= 1.2 Then
                                        dr_New("vMedExResult") = "Reactive"
                                    ElseIf Val(Dr_DataRow("vMachineMedExResult")) < 0.8 Then
                                        dr_New("vMedExResult") = "Non-Reactive"
                                    ElseIf Val(Dr_DataRow("vMachineMedExResult")) >= 0.8 And Val(Dr_DataRow("vMachineMedExResult")) < 1.2 Then
                                        dr_New("vMedExResult") = "Indeterminate"
                                    End If
                                    dr_New("vRemarks") = Dr_DataRow("vMachineMedExResult")

                                ElseIf Dr_DataRow("vMachineDetails") = "Best 2000" Then
                                    If Dr_DataRow("vMachineMedExResult").ToString.ToUpper = "NEG" Then
                                        dr_New("vMedExResult") = "Non-Reactive"
                                    ElseIf Dr_DataRow("vMachineMedExResult").ToString.ToUpper = "POS" Then
                                        dr_New("vMedExResult") = "Reactive"
                                    ElseIf Dr_DataRow("vMachineMedExResult").ToString.ToUpper = "REACT" Then
                                        dr_New("vMedExResult") = "Reactive"
                                    End If
                                ElseIf Dr_DataRow("vMachineDetails") = "DavinciQuattro" Then 'Added by Akhilesh [June 10 2012]
                                    If Dr_DataRow("vMachineMedExResult").ToString.ToUpper.Contains("NEG") Or Dr_DataRow("vMachineMedExResult").ToString.ToUpper.Contains("NEGATIVE") Then
                                        dr_New("vMedExResult") = "Non-Reactive"
                                    ElseIf Dr_DataRow("vMachineMedExResult").ToString.ToUpper.Contains("POS") Or Dr_DataRow("vMachineMedExResult").ToString.ToUpper.Contains("POSITIVE") Then
                                        dr_New("vMedExResult") = "Reactive"
                                    ElseIf Dr_DataRow("vMachineMedExResult").ToString.ToUpper.Contains("REAC") Or Dr_DataRow("vMachineMedExResult").ToString.ToUpper.Contains("REACTIVE") Then
                                        dr_New("vMedExResult") = "Reactive"
                                    End If
                                ElseIf Dr_DataRow("vMachineDetails") = "COBAS_E411" AndAlso (Dr_DataRow("vMachineMedExCode").ToString = "560" Or Dr_DataRow("vMachineMedExCode").ToString = "900" Or Dr_DataRow("vMachineMedExCode").ToString = "999") Then 'Added by Akhilesh [June 10 2012]
                                    Dim strResult() As String  'Where 560--->[HIV] And 900--->[HBsAg] And 999--->[HCV]
                                    strResult = Dr_DataRow("vMachineMedExResult").ToString.Split("^")
                                    If strResult.Length > 1 Then
                                        If strResult(0) = 1 Then
                                            dr_New("vMedExResult") = "Reactive"
                                            dr_New("vRemarks") = strResult(1)
                                        Else
                                            If strResult(1).ToString.Trim.ToUpper = "NO RESULT" Or strResult(1).ToString.Trim = "" Then
                                                dr_New("vMedExResult") = "No Result"
                                            ElseIf Val(strResult(1)) >= 1.0 Then
                                                dr_New("vMedExResult") = "Reactive"
                                            ElseIf Val(strResult(1)) < 0.9 Then
                                                dr_New("vMedExResult") = "Non-Reactive"
                                            ElseIf Val(strResult(1)) >= 0.9 And Val(strResult(1)) < 1.0 Then
                                                dr_New("vMedExResult") = "Indeterminate"
                                            End If
                                            dr_New("vRemarks") = strResult(1)
                                        End If
                                    Else
                                        If strResult(0).ToString.Trim.ToUpper = "NO RESULT" Or strResult(1).ToString.Trim = "" Then
                                            dr_New("vMedExResult") = "No Result"
                                        ElseIf Val(strResult(0)) >= 1.0 Then
                                            dr_New("vMedExResult") = "Reactive"
                                        ElseIf Val(strResult(0)) < 0.9 Then
                                            dr_New("vMedExResult") = "Non-Reactive"
                                        ElseIf Val(strResult(0)) >= 0.9 And Val(strResult(0)) < 1.0 Then
                                            dr_New("vMedExResult") = "Indeterminate"
                                        End If
                                        dr_New("vRemarks") = strResult(0)
                                    End If
                                ElseIf Dr_DataRow("vMachineDetails") = "STA_COMPACT" Then 'Added by Akhilesh [June 10 2012]
                                    dr_New("vMedExResult") = Dr_DataRow("vMachineMedExResult")
                                ElseIf Dr_DataRow("vMachineDetails") = "Dsx Elisa" Then
                                    If Dr_DataRow("vMachineMedExResult").ToString.ToUpper = "NEG." Then
                                        dr_New("vMedExResult") = "Non-Reactive"
                                    ElseIf Dr_DataRow("vMachineMedExResult").ToString.ToUpper = "POS." Then
                                        dr_New("vMedExResult") = "Reactive"
                                    ElseIf Dr_DataRow("vMachineMedExResult").ToString.ToUpper = "REAC." Then
                                        dr_New("vMedExResult") = "Reactive"
                                    End If
                                ElseIf Dr_DataRow("vMachineDetails") = "BD FACSCanto II" Then 'Added by Vivek Patel
                                    dr_New("vMedExResult") = Dr_DataRow("vMachineMedExResult")
                                Else
                                    dr_New("vMedExResult") = Dr_DataRow("vMachineMedExResult")
                                End If
                                dr_New("nLabMachineNo") = MachineNo
                                Dt_SampleMedexDetail_Save.Rows.Add(dr_New)
                                Dt_SampleMedexDetail_Save.AcceptChanges()
                            End If

                        End If


                    Next

                End If


            Next Dr_DataRow

            Ds_SampleMedEXDetail = Nothing
            Ds_SampleMedEXDetail = New DataSet
            Ds_SampleMedEXDetail.Tables.Add(Dt_SampleMedexDetail_Save.Copy())
            eStr_ReturnOfMedex = eStr_Retu
            Save_SampleMedEXDetail = objMaster.Save_SampleMedExDetail(Choice, Ds_SampleMedEXDetail, objDtLogic, UserId, eStr_Retu)
            eStr_Retu = eStr_ReturnOfMedex
        Catch ex As Exception

        End Try

    End Function

#End Region

End Class
