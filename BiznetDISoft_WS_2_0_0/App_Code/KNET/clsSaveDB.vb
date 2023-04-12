Imports Microsoft.VisualBasic

Public Class clsSaveDB
#Region "Insert_SubmissionInfoUSDtl"
    Friend Function Insert_SubmissionInfoUSDtl(ByRef SubmissionId As String, _
                                               ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                               ByVal Tbl_1 As Data.DataTable, _
                                               ByRef eStr_Retu As String) As Boolean
        Insert_SubmissionInfoUSDtl = False
        Dim ObjData As New ClsDataLogic_New

        Try
            Dim colParam As New ArrayList(0)
            Dim dsTemp As New DataSet
            Dim params As SqlClient.SqlParameter = Nothing
            Dim dr_1 As DataRow = Tbl_1.Rows(0)

            colParam.Add(ObjData.Add_SqlParameter("@vSubmissionInfoUSDtlId", dr_1("SubmissionInfoUSDtlId")))
            colParam.Add(ObjData.Add_SqlParameter("@vWorkspaceId", dr_1("WorkspaceId")))
            colParam.Add(ObjData.Add_SqlParameter("@vCountryCode", dr_1("CountryCode")))
            colParam.Add(ObjData.Add_SqlParameter("@vCurrentSeqNumber", dr_1("CurrentSeqNumber")))
            colParam.Add(ObjData.Add_SqlParameter("@vLastPublishedVersion", dr_1("LastPublishedVersion")))
            colParam.Add(ObjData.Add_SqlParameter("@vSubmissionPath", dr_1("SubmissionPath")))
            colParam.Add(ObjData.Add_SqlParameter("@iSubmitedBy", dr_1("SubmitedBy")))
            colParam.Add(ObjData.Add_SqlParameter("@vSubmissionType", dr_1("SubmissionType")))
            colParam.Add(ObjData.Add_SqlParameter("@dDateOfSubmission", dr_1("DateOfSubmission")))
            colParam.Add(ObjData.Add_SqlParameter("@vRelatedSeqNo", IIf(dr_1("RelatedSeqNo") Is DBNull.Value, "", dr_1("RelatedSeqNo"))))
            colParam.Add(ObjData.Add_SqlParameter("@cConfirm", dr_1("Confirm")))
            colParam.Add(ObjData.Add_SqlParameter("@iModifyBy", dr_1("ModifyBy")))
            colParam.Add(ObjData.Add_SqlParameter("@vLabelId", dr_1("LabelId")))
            colParam.Add(ObjData.Add_SqlParameter("@vSubmissionMode", dr_1("SubmissionMode")))
            colParam.Add(ObjData.Add_SqlParameter("@vApplicationNo", dr_1("ApplicationNo")))
            colParam.Add(ObjData.Add_SqlParameter("@DATAOPMODE", Choice_1))

            dsTemp = ObjData.ExecuteSP_DataSet("Insert_SubmissionInfoUSDtl", colParam)

            SubmissionId = dsTemp.Tables(0).Rows(0)(0)

            Insert_SubmissionInfoUSDtl = True

        Catch ex As Exception
            eStr_Retu = ex.Message
        Finally
            ObjData = Nothing
        End Try
    End Function
#End Region
#Region "Insert_SubmissionInfoEUDtl"
    Friend Function Insert_SubmissionInfoEUDtl(ByRef ParamOut_1 As String, _
                                               ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                               ByVal Tbl_1 As Data.DataTable, _
                                               ByRef eStr_Retu As String) As Boolean
        Insert_SubmissionInfoEUDtl = False
        Dim ObjData As New ClsDataLogic_New

        Try
            Dim colParam As New ArrayList(0)
            Dim dsTemp As New DataSet
            Dim params As SqlClient.SqlParameter = Nothing
            Dim dr_1 As DataRow = Tbl_1.Rows(0)

            colParam.Add(ObjData.Add_SqlParameter("@vSubmissionInfoEUDtlId", dr_1("vSubmissionInfoEUDtlId")))
            colParam.Add(ObjData.Add_SqlParameter("@vWorkspaceId", dr_1("vWorkspaceId")))
            colParam.Add(ObjData.Add_SqlParameter("@vCountryCode", dr_1("vCountryCode")))
            colParam.Add(ObjData.Add_SqlParameter("@vCurrentSeqNumber", dr_1("vCurrentSeqNumber")))
            colParam.Add(ObjData.Add_SqlParameter("@vLastPublishedVersion", dr_1("vLastPublishedVersion")))
            colParam.Add(ObjData.Add_SqlParameter("@vSubmissionPath", dr_1("vSubmissionPath")))
            colParam.Add(ObjData.Add_SqlParameter("@iSubmitedBy", dr_1("iSubmitedBy")))
            colParam.Add(ObjData.Add_SqlParameter("@vSubmissionType", dr_1("vSubmissionType")))
            colParam.Add(ObjData.Add_SqlParameter("@dDateOfSubmission", dr_1("dDateOfSubmission")))
            colParam.Add(ObjData.Add_SqlParameter("@vRelatedSeqNo", dr_1("vRelatedSeqNo")))
            colParam.Add(ObjData.Add_SqlParameter("@cConfirm", dr_1("cConfirm")))
            colParam.Add(ObjData.Add_SqlParameter("@iModifyBy", dr_1("iModifyBy")))
            colParam.Add(ObjData.Add_SqlParameter("@vLabelId", dr_1("vLabelId")))
            colParam.Add(ObjData.Add_SqlParameter("@vSubmissionMode", dr_1("vSubmissionMode")))
            colParam.Add(ObjData.Add_SqlParameter("@cRMSSubmited", dr_1("cRMSSubmited")))
            colParam.Add(ObjData.Add_SqlParameter("@vApplicationNo", dr_1("vApplicationNo")))
            colParam.Add(ObjData.Add_SqlParameter("@DATAOPMODE", Choice_1))


            dsTemp = ObjData.ExecuteSP_DataSet("Insert_SubmissionInfoEUDtl", colParam)

            ParamOut_1 = colParam.Item(0).ToString()

            Insert_SubmissionInfoEUDtl = True

        Catch ex As Exception
            eStr_Retu = ex.Message
        Finally
            ObjData = Nothing
        End Try
    End Function
#End Region
#Region "Insert_SubmissionInfoEUSubDtl"
    Friend Function Insert_SubmissionInfoEUSubDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                  ByVal Tbl_1 As Data.DataTable, _
                                                  ByRef eStr_Retu As String) As Boolean
        Insert_SubmissionInfoEUSubDtl = False
        Dim ObjData As New ClsDataLogic_New

        Try
            Dim colParam As New ArrayList(0)
            Dim dsTemp As New DataSet
            Dim params As SqlClient.SqlParameter = Nothing
            Dim dr_1 As DataRow = Tbl_1.Rows(0)

            colParam.Add(ObjData.Add_SqlParameter("@iSubmissionInfoEUSubDtlId", dr_1("iSubmissionInfoEUSubDtlId")))
            colParam.Add(ObjData.Add_SqlParameter("@iWorkspaceCMSId", dr_1("iWorkspaceCMSId")))
            colParam.Add(ObjData.Add_SqlParameter("@vSubmissionInfoEUDtlId", dr_1("vSubmissionInfoEUDtlId")))
            colParam.Add(ObjData.Add_SqlParameter("@dDateOfSubmission", dr_1("dDateOfSubmission")))
            colParam.Add(ObjData.Add_SqlParameter("@vSubmissionDescription", dr_1("vSubmissionDescription")))
            colParam.Add(ObjData.Add_SqlParameter("@iModifyBy", dr_1("iModifyBy")))
            colParam.Add(ObjData.Add_SqlParameter("@cStatusIndi", dr_1("cStatusIndi")))
            colParam.Add(ObjData.Add_SqlParameter("@dataOpMode", Choice_1))

            dsTemp = ObjData.ExecuteSP_DataSet("Insert_SubmissionInfoEUSubDtl", colParam)

            Insert_SubmissionInfoEUSubDtl = True

        Catch ex As Exception
            eStr_Retu = ex.Message
        Finally
            ObjData = Nothing
        End Try
    End Function
#End Region
#Region "Insert_SubmissionInfoEU14Dtl"
    Friend Function Insert_SubmissionInfoEU14Dtl(ByRef ParamOut_1 As String, _
                                                 ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                 ByVal Tbl_1 As Data.DataTable, _
                                                 ByRef eStr_Retu As String) As Boolean
        Insert_SubmissionInfoEU14Dtl = False
        Dim ObjData As New ClsDataLogic_New

        Try
            Dim colParam As New ArrayList(0)
            Dim dsTemp As New DataSet
            Dim params As SqlClient.SqlParameter = Nothing
            Dim dr_1 As DataRow = Tbl_1.Rows(0)

            colParam.Add(ObjData.Add_SqlParameter("@vSubmissionInfoEU14DtlId", dr_1("SubmissionInfoEU14DtlId")))
            colParam.Add(ObjData.Add_SqlParameter("@vWorkspaceId", dr_1("WorkspaceId")))
            colParam.Add(ObjData.Add_SqlParameter("@vCountryCode", dr_1("CountryCode")))
            colParam.Add(ObjData.Add_SqlParameter("@vCurrentSeqNumber", dr_1("CurrentSeqNumber")))
            colParam.Add(ObjData.Add_SqlParameter("@vLastPublishedVersion", dr_1("LastPublishedVersion")))
            colParam.Add(ObjData.Add_SqlParameter("@vSubmissionPath", dr_1("SubmissionPath")))
            colParam.Add(ObjData.Add_SqlParameter("@iSubmitedBy", dr_1("SubmitedBy")))
            colParam.Add(ObjData.Add_SqlParameter("@vSubmissionType", dr_1("SubmissionType")))
            colParam.Add(ObjData.Add_SqlParameter("@dDateOfSubmission", dr_1("DateOfSubmission")))
            colParam.Add(ObjData.Add_SqlParameter("@vRelatedSeqNo", dr_1("RelatedSeqNo")))
            colParam.Add(ObjData.Add_SqlParameter("@cConfirm", dr_1("Confirm")))
            colParam.Add(ObjData.Add_SqlParameter("@iModifyBy", dr_1("ModifyBy")))
            colParam.Add(ObjData.Add_SqlParameter("@vLabelId", dr_1("LabelId")))
            colParam.Add(ObjData.Add_SqlParameter("@vSubmissionMode", dr_1("SubmissionMode")))
            colParam.Add(ObjData.Add_SqlParameter("@vSubVariationMode", dr_1("SubVariationMode")))
            colParam.Add(ObjData.Add_SqlParameter("@cRMSSubmited", dr_1("RMSSubmited")))
            colParam.Add(ObjData.Add_SqlParameter("@vTrackingNo", dr_1("TrackingNo")))
            colParam.Add(ObjData.Add_SqlParameter("@DATAOPMODE", Choice_1))

            dsTemp = ObjData.ExecuteSP_DataSet("Insert_SubmissionInfoEU14Dtl", colParam)

            ParamOut_1 = dsTemp.Tables(0).Rows(0)(0).ToString

            Insert_SubmissionInfoEU14Dtl = True

        Catch ex As Exception
            eStr_Retu = ex.Message
        Finally
            ObjData = Nothing
        End Try
    End Function
#End Region
#Region "Insert_SubmissionInfoEU14SubDtl"
    Friend Function Insert_SubmissionInfoEU14SubDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                    ByVal Tbl_1 As Data.DataTable, _
                                                    ByRef eStr_Retu As String) As Boolean
        Insert_SubmissionInfoEU14SubDtl = False
        Dim ObjData As New ClsDataLogic_New

        Try
            Dim colParam As New ArrayList(0)
            Dim dsTemp As New DataSet
            Dim params As SqlClient.SqlParameter = Nothing
            Dim dr_1 As DataRow = Tbl_1.Rows(0)

            colParam.Add(ObjData.Add_SqlParameter("@iSubmissionInfoEU14SubDtlId", IIf(IsDBNull(dr_1("iSubmissionInfoEU14SubDtlId")), "", dr_1("iSubmissionInfoEU14SubDtlId"))))
            colParam.Add(ObjData.Add_SqlParameter("@iWorkspaceCMSId", dr_1("iWorkspaceCMSId")))
            colParam.Add(ObjData.Add_SqlParameter("@vSubmissionInfoEU14DtlId", dr_1("vSubmissionInfoEU14DtlId")))
            colParam.Add(ObjData.Add_SqlParameter("@dDateOfSubmission", dr_1("dDateOfSubmission")))
            colParam.Add(ObjData.Add_SqlParameter("@vSubmissionDescription", dr_1("vSubmissionDescription")))
            colParam.Add(ObjData.Add_SqlParameter("@iModifyBy", dr_1("iModifyBy")))
            colParam.Add(ObjData.Add_SqlParameter("@cStatusIndi", IIf(IsDBNull(dr_1("cStatusIndi")), "", dr_1("cStatusIndi"))))
            colParam.Add(ObjData.Add_SqlParameter("@vPublishCMSTrackingNo", dr_1("vPublishCMSTrackingNo")))
            colParam.Add(ObjData.Add_SqlParameter("@DATAOPMODE", Choice_1))



            dsTemp = ObjData.ExecuteSP_DataSet("Insert_SubmissionInfoEU14SubDtl", colParam)

            Insert_SubmissionInfoEU14SubDtl = True

        Catch ex As Exception
            eStr_Retu = ex.Message
        Finally
            ObjData = Nothing
        End Try
    End Function
#End Region
#Region "Insert_SubmissionInfoForManualMode"
    Friend Function Insert_SubmissionInfoForManualMode(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                       ByVal Tbl_1 As Data.DataTable, _
                                                       ByRef eStr_Retu As String) As Boolean
        Insert_SubmissionInfoForManualMode = False
        Dim ObjData As ClsDataLogic_New = Nothing

        Try
            Dim colParam As New ArrayList(0)
            Dim dsTemp As New DataSet
            Dim params As SqlClient.SqlParameter = Nothing


            For Each dr_1 As DataRow In Tbl_1.Rows
                colParam.Add(ObjData.Add_SqlParameter("@iSubInfoManualModeId", dr_1("iSubInfoManualModeId")))
                colParam.Add(ObjData.Add_SqlParameter("@vWorkspaceId", dr_1("vWorkspaceId")))
                colParam.Add(ObjData.Add_SqlParameter("@vRegion", dr_1("vRegion")))
                colParam.Add(ObjData.Add_SqlParameter("@vSubmissionId", dr_1("vSubmissionId")))
                colParam.Add(ObjData.Add_SqlParameter("@iNodeId", dr_1("iNodeId")))
                colParam.Add(ObjData.Add_SqlParameter("@iTranNo", dr_1("iTranNo")))
                colParam.Add(ObjData.Add_SqlParameter("@vRefID", dr_1("vRefID")))
                colParam.Add(ObjData.Add_SqlParameter("@vOperation", dr_1("vOperation")))
                colParam.Add(ObjData.Add_SqlParameter("@vRelSeqNo", dr_1("vRelSeqNo")))
                colParam.Add(ObjData.Add_SqlParameter("@iModifyBy", dr_1("iModifyBy")))
                colParam.Add(ObjData.Add_SqlParameter("@DATAOPMODE", Choice_1))

                dsTemp = ObjData.ExecuteSP_DataSet("Insert_SubmissionInfoEU14SubDtl", colParam)
            Next dr_1

            Insert_SubmissionInfoForManualMode = True

        Catch ex As Exception
            eStr_Retu = ex.Message
        Finally
            ObjData = Nothing
        End Try
    End Function
#End Region
#Region "Insert_SubmissionInfoCADtl"
    Friend Function Insert_SubmissionInfoCADtl(ByRef ParamOut_1 As String, _
                                               ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                               ByVal Tbl_1 As Data.DataTable, _
                                               ByRef eStr_Retu As String) As Boolean
        Insert_SubmissionInfoCADtl = False
        Dim ObjData As New ClsDataLogic_New

        Try
            Dim colParam As New ArrayList(0)
            Dim dsTemp As New DataSet
            Dim params As SqlClient.SqlParameter = Nothing
            Dim dr_1 As DataRow = Tbl_1.Rows(0)

            colParam.Add(ObjData.Add_SqlParameter("@vSubmissionInfoCADtlId", dr_1("SubmissionInfoCADtlId")))
            colParam.Add(ObjData.Add_SqlParameter("@vWorkspaceId", dr_1("WorkspaceId")))
            colParam.Add(ObjData.Add_SqlParameter("@vCountryCode", dr_1("CountryCode")))
            colParam.Add(ObjData.Add_SqlParameter("@vCurrentSeqNumber", dr_1("CurrentSeqNumber")))
            colParam.Add(ObjData.Add_SqlParameter("@vLastPublishedVersion", dr_1("LastPublishedVersion")))
            colParam.Add(ObjData.Add_SqlParameter("@vSubmissionPath", dr_1("SubmissionPath")))
            colParam.Add(ObjData.Add_SqlParameter("@iSubmitedBy", dr_1("SubmitedBy")))
            colParam.Add(ObjData.Add_SqlParameter("@vSubmissionType", dr_1("SubmissionType")))
            colParam.Add(ObjData.Add_SqlParameter("@dDateOfSubmission", dr_1("DateOfSubmission")))
            colParam.Add(ObjData.Add_SqlParameter("@vRelatedSeqNo", dr_1("RelatedSeqNo")))
            colParam.Add(ObjData.Add_SqlParameter("@cConfirm", dr_1("Confirm")))
            colParam.Add(ObjData.Add_SqlParameter("@iModifyBy", dr_1("ModifyBy")))
            colParam.Add(ObjData.Add_SqlParameter("@vLabelId", dr_1("LabelId")))
            colParam.Add(ObjData.Add_SqlParameter("@vSubmissionMode", dr_1("SubmissionMode")))
            colParam.Add(ObjData.Add_SqlParameter("@vApplicationNo", dr_1("ApplicationNo")))
            colParam.Add(ObjData.Add_SqlParameter("@DATAOPMODE", Choice_1))

            dsTemp = ObjData.ExecuteSP_DataSet("Insert_SubmissionInfoCADtl", colParam)

            ParamOut_1 = dsTemp.Tables(0).Rows(0)(0)

            Insert_SubmissionInfoCADtl = True

        Catch ex As Exception
            eStr_Retu = ex.Message
        Finally
            ObjData = Nothing
        End Try
    End Function
#End Region
#Region "createInternalLabel"
    Friend Function createInternalLabel(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                        ByVal Tbl_1 As Data.DataTable, _
                                        ByRef eStr_Retu As String) As Boolean
        createInternalLabel = False
        Dim ObjData As New ClsDataLogic_New

        Try
            Dim colParam As New ArrayList(0)
            Dim dsTemp As New DataSet
            Dim params As SqlClient.SqlParameter = Nothing
            Dim dr_1 As DataRow = Tbl_1.Rows(0)

            colParam.Add(ObjData.Add_SqlParameter("@vWorkspaceId", dr_1("WorkspaceId")))
            colParam.Add(ObjData.Add_SqlParameter("@vLabelId", dr_1("LabelId")))
            colParam.Add(ObjData.Add_SqlParameter("@vLabelRemark", dr_1("Remark")))
            colParam.Add(ObjData.Add_SqlParameter("@iCreatedBy", dr_1("ModifyBy")))

            dsTemp = ObjData.ExecuteSP_DataSet("Insert_InternalLabel", colParam)

            createInternalLabel = True

        Catch ex As Exception
            eStr_Retu = ex.Message
        Finally
            ObjData = Nothing
        End Try
    End Function
#End Region

#Region "Insert_WorkspaceNodeAttrDetail_ForEctd"
    Public Function Insert_WorkspaceNodeAttrDetail_ForEctd(ByVal Choice As DataObjOpenSaveModeEnum, _
                                ByVal Ds_WorkspaceNodeAttrDetail_ForEctd As DataSet, _
                                ByVal UserCode_1 As String, _
                                ByRef eStr_Retu As String) As Boolean
        Dim objDtLogic As New ClsDataLogic_New

        Dim dataRet_1 As DataRetrievalModeEnum
        Dim objHelpDbTbl As clsHelpDbTable = Nothing

        'Object of friend class
        Dim objSave As SaveDataInDbObj = Nothing

        Dim Tbl_Audit As DataTable = Nothing
        Dim Sql_DtSet As Data.DataSet = Nothing
        Dim serverDateTime_1 As DateTime

        Dim Instr_WorkspaceId As String = ""

        Dim PkColArr(2) As String
        Dim DataReturnList As New ArrayList
        Dim RetuVal As ParameterReturnValue = Nothing

        Try
            objDtLogic.Open_Connection()
            objDtLogic.Begin_Transaction()

            'Logic Of save Start

            eStr_Retu = ""

            If objDtLogic Is Nothing Then
                eStr_Retu = "Data Object Is Invalid or Nothing"
                Exit Function
            End If

            If objDtLogic.Transaction Is Nothing Then
                eStr_Retu = "Transaction object is not started"
                Exit Function
            End If

            If Ds_WorkspaceNodeAttrDetail_ForEctd Is Nothing Then
                eStr_Retu = "Dataset Is Invalid or Nothing"
                Exit Function
            End If

            If Choice = DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                dataRet_1 = DataRetrievalModeEnum.DataTable_Empty
            Else
                dataRet_1 = DataRetrievalModeEnum.DataTable_WithWhereCondition
            End If

            objHelpDbTbl = New clsHelpDbTable()

            serverDateTime_1 = objDtLogic.GetServerDateTime
            objSave = New SaveDataInDbObj(objDtLogic)

            PkColArr(0) = "vWorkSpaceId"
            PkColArr(1) = "iNodeId"
            PkColArr(2) = "iAttrId"

            If Not objSave.AssignValues(Ds_WorkspaceNodeAttrDetail_ForEctd.Tables(0).TableName, PkColArr, Choice, _
                                        Ds_WorkspaceNodeAttrDetail_ForEctd.Tables(Ds_WorkspaceNodeAttrDetail_ForEctd.Tables(0).TableName), _
                                        Ds_WorkspaceNodeAttrDetail_ForEctd.Tables(0), Tbl_Audit, False, UserCode_1, serverDateTime_1, eStr_Retu) Then
                eStr_Retu += vbCrLf + "Error occured while assigning values"
                Exit Function
            End If

            ' Save in WorkspaceNodeAttrDetail
            If Ds_WorkspaceNodeAttrDetail_ForEctd.Tables(0).Rows.Count > 0 Then
                If Not objSave.SaveInDb("Insert_WorkspaceNodeAttrDetail_ForEctd", Ds_WorkspaceNodeAttrDetail_ForEctd.Tables(0), Choice, DataReturnList, eStr_Retu) Then
                    eStr_Retu += vbCrLf + "Error occured while Save Information of WorkspaceNodeAttrDetail In Database"
                    Exit Function
                End If
            End If

            Insert_WorkspaceNodeAttrDetail_ForEctd = True
            'Logic of Save End

            If Insert_WorkspaceNodeAttrDetail_ForEctd Then
                objDtLogic.Commit_Transaction()
            Else
                objDtLogic.RollBack_Transaction()
            End If

        Catch ex As Exception
            eStr_Retu = ex.Message + vbCrLf + "Error occured while saving WorkspaceNodeAttrDetail"
            Insert_WorkspaceNodeAttrDetail_ForEctd = False
            objDtLogic.RollBack_Transaction()
        Finally
            If Not objDtLogic Is Nothing Then
                objDtLogic.Close_Connection()
                objDtLogic = Nothing
            End If
        End Try
    End Function
#End Region

#Region "Insert_SubmittedWorkspaceNodeDetail"
    Public Function Insert_SubmittedWorkspaceNodeDetail(ByVal Choice As DataObjOpenSaveModeEnum, _
                                ByVal Ds_SubmittedWorkspaceNodeDetail As DataSet, _
                                ByVal UserCode_1 As String, _
                                ByRef eStr_Retu As String) As Boolean
        Dim objDtLogic As New ClsDataLogic_New

        Dim dataRet_1 As DataRetrievalModeEnum
        Dim objHelpDbTbl As clsHelpDbTable = Nothing

        'Object of friend class
        Dim objSave As SaveDataInDbObj = Nothing

        Dim Tbl_SubmittedWorkSpaceNodeDetail As New DataTable
        Dim Tbl_Audit As DataTable = Nothing
        Dim Sql_DtSet As Data.DataSet = Nothing
        Dim serverDateTime_1 As DateTime

        Dim Instr_WorkspaceId As String = ""

        Dim PkColArr(2) As String
        Dim DataReturnList As New ArrayList
        Dim RetuVal As ParameterReturnValue = Nothing

        Try
            objDtLogic.Open_Connection()
            objDtLogic.Begin_Transaction()

            'Logic Of save Start

            eStr_Retu = ""

            If objDtLogic Is Nothing Then
                eStr_Retu = "Data Object Is Invalid or Nothing"
                Exit Function
            End If

            If objDtLogic.Transaction Is Nothing Then
                eStr_Retu = "Transaction object is not started"
                Exit Function
            End If

            If Ds_SubmittedWorkspaceNodeDetail Is Nothing Then
                eStr_Retu = "Dataset Is Invalid or Nothing"
                Exit Function
            End If

            Tbl_SubmittedWorkSpaceNodeDetail = Ds_SubmittedWorkspaceNodeDetail.Tables(0).Copy()
            Tbl_SubmittedWorkSpaceNodeDetail.Rows.Clear()
            Tbl_SubmittedWorkSpaceNodeDetail.AcceptChanges()

            If Choice = DataObjOpenSaveModeEnum.DataObjOpenMode_Add Then
                dataRet_1 = DataRetrievalModeEnum.DataTable_Empty
            Else
                dataRet_1 = DataRetrievalModeEnum.DataTable_WithWhereCondition
            End If

            objHelpDbTbl = New clsHelpDbTable()

            serverDateTime_1 = objDtLogic.GetServerDateTime
            objSave = New SaveDataInDbObj(objDtLogic)

            PkColArr(0) = "vworkspaceId"
            PkColArr(1) = "submissionId"
            PkColArr(2) = "iNodeId"
            
            If Not objSave.AssignValues(Ds_SubmittedWorkspaceNodeDetail.Tables(0).TableName, PkColArr, Choice, _
                                        Ds_SubmittedWorkspaceNodeDetail.Tables(Ds_SubmittedWorkspaceNodeDetail.Tables(0).TableName), _
                                        Tbl_SubmittedWorkSpaceNodeDetail, Tbl_Audit, False, UserCode_1, serverDateTime_1, eStr_Retu) Then
                eStr_Retu += vbCrLf + "Error occured while assigning values"
                Exit Function
            End If

            ' Save in WorkspaceNodeAttrDetail
            If Tbl_SubmittedWorkSpaceNodeDetail.Rows.Count > 0 Then
                If Not objSave.SaveInDb("Insert_SubmittedWorkspaceNodeDetail", Tbl_SubmittedWorkSpaceNodeDetail, Choice, DataReturnList, eStr_Retu) Then
                    eStr_Retu += vbCrLf + "Error occured while Save Information of SubmittedWorkspaceNodeDetail In Database"
                    Exit Function
                End If
            End If

            Insert_SubmittedWorkspaceNodeDetail = True
            'Logic of Save End

            If Insert_SubmittedWorkspaceNodeDetail Then
                objDtLogic.Commit_Transaction()
            Else
                objDtLogic.RollBack_Transaction()
            End If

        Catch ex As Exception
            eStr_Retu = ex.Message + vbCrLf + "Error occured while saving SubmittedWorkspaceNodeDetail"
            Insert_SubmittedWorkspaceNodeDetail = False
            objDtLogic.RollBack_Transaction()
        Finally
            If Not objDtLogic Is Nothing Then
                objDtLogic.Close_Connection()
                objDtLogic = Nothing
            End If
        End Try
    End Function
#End Region

End Class
