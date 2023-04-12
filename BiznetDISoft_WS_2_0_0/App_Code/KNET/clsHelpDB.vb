Imports Microsoft.VisualBasic

Public Class clsHelpDB
    Public Function getSubmissionInfo(ByVal WhereCondition_1 As String, _
                                      ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                      ByRef DTOSubmissionMst As DataSet, _
                                      ByRef eStr As String) As Boolean

        getSubmissionInfo = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try
            getSubmissionInfo = getSubmissionInfo(WhereCondition_1, DataRetrieval_1, Tbl_1, eStr)

            DTOSubmissionMst = New DataSet
            DTOSubmissionMst.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr = ex.Message
        End Try

    End Function
    Friend Function getSubmissionInfo(ByVal WhereCondition_1 As String, _
                                      ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                      ByRef DTOSubmissionMst As DataTable, _
                                      ByRef eStr As String) As Boolean
        getSubmissionInfo = False
        Dim ObjData As New ClsDataLogic_New

        Try
            getSubmissionInfo = ObjData.GetDataset("view_AllWorkspaceSubmissionInfo", "", WhereCondition_1, _
                        DataRetrieval_1, DTOSubmissionMst)

            DTOSubmissionMst.Columns.Add(New DataColumn("SubmissionID", GetType(String)))

        Catch ex As Exception
            eStr = ex.Message
        Finally
            ObjData = Nothing
        End Try

    End Function

    Public Function getAllNodesLastHistory(ByVal WhereCondition_1 As String, _
                                           ByRef DS_retu As DataSet, _
                                           ByRef eStr As String) As Boolean

        getAllNodesLastHistory = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try
            getAllNodesLastHistory = getAllNodesLastHistory(WhereCondition_1, Tbl_1, eStr)

            DS_retu = New DataSet
            DS_retu.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr = ex.Message
        End Try

    End Function
    Friend Function getAllNodesLastHistory(ByVal WhereCondition_1 As String, _
                                           ByRef Tbl_Retu As DataTable, _
                                           ByRef eStr As String) As Boolean
        getAllNodesLastHistory = False
        Dim ObjData As New ClsDataLogic_New
        Dim Query As String = String.Empty

        Try

            Query = " Select vWorkspaceId,vWorkSpaceDesc,vBaseWorkFolder,iNodeId,iTranNo,vFileName,vFolderName," + _
                    "        iStageId,vStageDesc,iModifyBy,vUserName,dModifyOn,vNodeName,vNodeFolderName,vUserDefineVersionId" + _
                    " From View_NodesLatestHistory " + _
                    " Where " + WhereCondition_1

            getAllNodesLastHistory = ObjData.GetDataset("View_NodesLatestHistory", Query, WhereCondition_1, _
                       DataRetrievalModeEnum.DatatTable_Query, Tbl_Retu)

        Catch ex As Exception
            eStr = ex.Message
        Finally
            ObjData = Nothing
        End Try

    End Function

    Public Function getMaxWorkspaceLabel(ByVal WhereCondition_1 As String, _
                                         ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                         ByRef Dataset_1 As DataSet, _
                                         ByRef eStr As String) As Boolean

        getMaxWorkspaceLabel = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try
            getMaxWorkspaceLabel = getMaxWorkspaceLabel(WhereCondition_1, DataRetrieval_1, Tbl_1, eStr)

            Dataset_1 = New DataSet
            Dataset_1.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr = ex.Message
        End Try

    End Function
    Friend Function getMaxWorkspaceLabel(ByVal WhereCondition_1 As String, _
                                         ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                         ByRef Tbl_1 As DataTable, _
                                         ByRef eStr As String) As Boolean
        getMaxWorkspaceLabel = False
        Dim ObjData As New ClsDataLogic_New

        Try
            getMaxWorkspaceLabel = ObjData.GetDataset("View_GetMaxWorkspaceLabel", "", WhereCondition_1, _
                                                      DataRetrieval_1, Tbl_1)

        Catch ex As Exception
            eStr = ex.Message
        Finally
            ObjData = Nothing
        End Try

    End Function


    Public Function getWorkspaceSubmissionInfoUSDtlBySubmissionId(ByVal WhereCondition_1 As String, _
                                                                  ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                                  ByRef Dataset_1 As DataSet, _
                                                                  ByRef eStr As String) As Boolean
        getWorkspaceSubmissionInfoUSDtlBySubmissionId = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try
            getWorkspaceSubmissionInfoUSDtlBySubmissionId = getWorkspaceSubmissionInfoUSDtlBySubmissionId(WhereCondition_1, _
                                                                                                          DataRetrieval_1, _
                                                                                                          Tbl_1, eStr)

            Dataset_1 = New DataSet
            Dataset_1.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr = ex.Message
        End Try

    End Function
    Friend Function getWorkspaceSubmissionInfoUSDtlBySubmissionId(ByVal WhereCondition_1 As String, _
                                                                  ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                                  ByRef Tbl_1 As DataTable, _
                                                                  ByRef eStr As String) As Boolean
        getWorkspaceSubmissionInfoUSDtlBySubmissionId = False
        Dim ObjData As New ClsDataLogic_New

        Try
            getWorkspaceSubmissionInfoUSDtlBySubmissionId = ObjData.GetDataset("View_SubmissionInfoUSDtl", "", _
                                                                               WhereCondition_1, DataRetrieval_1, Tbl_1)

        Catch ex As Exception
            eStr = ex.Message
        Finally
            ObjData = Nothing
        End Try
    End Function

    Public Function getWorkspaceSubmissionInfoEUDtlBySubmissionId(ByVal WhereCondition_1 As String, _
                                                                  ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                                  ByRef Dataset_1 As DataSet, _
                                                                  ByRef eStr As String) As Boolean
        getWorkspaceSubmissionInfoEUDtlBySubmissionId = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try
            getWorkspaceSubmissionInfoEUDtlBySubmissionId = getWorkspaceSubmissionInfoEUDtlBySubmissionId(WhereCondition_1, _
                                                                                                          DataRetrieval_1, _
                                                                                                          Tbl_1, eStr)

            Dataset_1 = New DataSet
            Dataset_1.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr = ex.Message
        End Try

    End Function
    Friend Function getWorkspaceSubmissionInfoEUDtlBySubmissionId(ByVal WhereCondition_1 As String, _
                                                                  ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                                  ByRef Tbl_1 As DataTable, _
                                                                  ByRef eStr As String) As Boolean
        getWorkspaceSubmissionInfoEUDtlBySubmissionId = False
        Dim ObjData As New ClsDataLogic_New

        Try
            getWorkspaceSubmissionInfoEUDtlBySubmissionId = ObjData.GetDataset("View_SubmissionInfoEUDtl", "", WhereCondition_1, _
                                                                               DataRetrieval_1, Tbl_1)

        Catch ex As Exception
            eStr = ex.Message
        Finally
            ObjData = Nothing
        End Try

    End Function

    Public Function getWorkspaceSubmissionInfoEU14DtlBySubmissionId(ByVal WhereCondition_1 As String, _
                                                                    ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                                    ByRef Dataset_1 As DataSet, _
                                                                    ByRef eStr As String) As Boolean
        getWorkspaceSubmissionInfoEU14DtlBySubmissionId = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try
            getWorkspaceSubmissionInfoEU14DtlBySubmissionId = getWorkspaceSubmissionInfoEU14DtlBySubmissionId(WhereCondition_1, _
                                                                                                              DataRetrieval_1, _
                                                                                                              Tbl_1, eStr)

            Dataset_1 = New DataSet
            Dataset_1.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr = ex.Message
        End Try

    End Function
    Friend Function getWorkspaceSubmissionInfoEU14DtlBySubmissionId(ByVal WhereCondition_1 As String, _
                                                                    ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                                    ByRef Tbl_1 As DataTable, _
                                                                    ByRef eStr As String) As Boolean
        getWorkspaceSubmissionInfoEU14DtlBySubmissionId = False
        Dim ObjData As New ClsDataLogic_New

        Try
            getWorkspaceSubmissionInfoEU14DtlBySubmissionId = ObjData.GetDataset("View_SubmissionInfoEU14Dtl", "", _
                                                                                 WhereCondition_1, DataRetrieval_1, Tbl_1)

        Catch ex As Exception
            eStr = ex.Message
        Finally
            ObjData = Nothing
        End Try

    End Function

    Public Function getWorkspaceSubmissionInfoCADtlBySubmissionId(ByVal WhereCondition_1 As String, _
                                                                  ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                                  ByRef Dataset_1 As DataSet, _
                                                                  ByRef eStr As String) As Boolean
        getWorkspaceSubmissionInfoCADtlBySubmissionId = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try
            getWorkspaceSubmissionInfoCADtlBySubmissionId = getWorkspaceSubmissionInfoCADtlBySubmissionId(WhereCondition_1, _
                                                                                                          DataRetrieval_1, _
                                                                                                          Tbl_1, eStr)

            Dataset_1 = New DataSet
            Dataset_1.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr = ex.Message
        End Try

    End Function
    Friend Function getWorkspaceSubmissionInfoCADtlBySubmissionId(ByVal WhereCondition_1 As String, _
                                                                  ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                                  ByRef Tbl_1 As DataTable, _
                                                                  ByRef eStr As String) As Boolean
        getWorkspaceSubmissionInfoCADtlBySubmissionId = False
        Dim ObjData As New ClsDataLogic_New

        Try
            getWorkspaceSubmissionInfoCADtlBySubmissionId = ObjData.GetDataset("View_SubmissionInfoCADtl", "", WhereCondition_1, _
                                                                               DataRetrieval_1, Tbl_1)

        Catch ex As Exception
            eStr = ex.Message
        Finally
            ObjData = Nothing
        End Try

    End Function


    Public Function getWorkSpaceDetail(ByVal WhereCondition_1 As String, _
                                       ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                       ByRef DTOWorkSpaceMst As DataSet, _
                                       ByRef eStr As String) As Boolean

        getWorkSpaceDetail = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try
            getWorkSpaceDetail = getMaxWorkspaceLabel(WhereCondition_1, DataRetrieval_1, Tbl_1, eStr)

            DTOWorkSpaceMst = New DataSet
            DTOWorkSpaceMst.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr = ex.Message
        End Try

    End Function
    Friend Function getWorkSpaceDetail(ByVal WhereCondition_1 As String, _
                                       ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                       ByRef DTOWorkSpaceMst As DataTable, _
                                       ByRef eStr As String) As Boolean
        getWorkSpaceDetail = False
        Dim ObjData As New ClsDataLogic_New

        Try
            getWorkSpaceDetail = ObjData.GetDataset("View_CommonWorkspaceDetail", "", WhereCondition_1, _
                        DataRetrieval_1, DTOWorkSpaceMst)

        Catch ex As Exception
            eStr = ex.Message
        Finally
            ObjData = Nothing
        End Try

    End Function


    Public Function getWorkspaceSubmissionInfoEUSubDtl(ByVal WhereCondition_1 As String, _
                                                       ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                       ByRef DTOSubmissionInfoEUDtl As DataSet, _
                                                       ByRef eStr As String) As Boolean

        getWorkspaceSubmissionInfoEUSubDtl = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try
            getWorkspaceSubmissionInfoEUSubDtl = getWorkspaceSubmissionInfoEUSubDtl(WhereCondition_1, DataRetrieval_1, Tbl_1, eStr)

            DTOSubmissionInfoEUDtl = New DataSet
            DTOSubmissionInfoEUDtl.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr = ex.Message
        End Try

    End Function
    Friend Function getWorkspaceSubmissionInfoEUSubDtl(ByVal WhereCondition_1 As String, _
                                                       ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                       ByRef DTOSubmissionInfoEUDtl As DataTable, _
                                                       ByRef eStr As String) As Boolean
        getWorkspaceSubmissionInfoEUSubDtl = False
        Dim ObjData As New ClsDataLogic_New

        Try
            getWorkspaceSubmissionInfoEUSubDtl = ObjData.GetDataset("SubmissionInfoEUSubDtl", "", WhereCondition_1, _
                                                                    DataRetrieval_1, DTOSubmissionInfoEUDtl)

        Catch ex As Exception
            eStr = ex.Message
        Finally
            ObjData = Nothing
        End Try

    End Function

    Public Function getWorkspaceSubmissionInfoEU14SubDtl(ByVal WhereCondition_1 As String, _
                                                         ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                         ByRef DTOSubmissionInfoEUDtl As DataSet, _
                                                         ByRef eStr As String) As Boolean

        getWorkspaceSubmissionInfoEU14SubDtl = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try
            getWorkspaceSubmissionInfoEU14SubDtl = getWorkspaceSubmissionInfoEU14SubDtl(WhereCondition_1, DataRetrieval_1, _
                                                                                        Tbl_1, eStr)

            DTOSubmissionInfoEUDtl = New DataSet
            DTOSubmissionInfoEUDtl.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr = ex.Message
        End Try

    End Function
    Friend Function getWorkspaceSubmissionInfoEU14SubDtl(ByVal WhereCondition_1 As String, _
                                                         ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                         ByRef DTOSubmissionInfoEUDtl As DataTable, _
                                                         ByRef eStr As String) As Boolean
        getWorkspaceSubmissionInfoEU14SubDtl = False
        Dim ObjData As New ClsDataLogic_New

        Try
            getWorkspaceSubmissionInfoEU14SubDtl = ObjData.GetDataset("SubmissionInfoEU14SubDtl", "", WhereCondition_1, _
                                                                      DataRetrieval_1, DTOSubmissionInfoEUDtl)

        Catch ex As Exception
            eStr = ex.Message
        Finally
            ObjData = Nothing
        End Try

    End Function



    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Added by Bharat Patel on 9-Aug-2011 for The GetData
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

#Region "getFolderByWorkSpaceId "
    Public Function getFolderByWorkSpaceId(ByVal WhereCondition_1 As String, _
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                           ByRef Sql_DataSet As Data.DataSet, _
                                           ByRef eStr_Retu As String) As Boolean
        getFolderByWorkSpaceId = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try

            getFolderByWorkSpaceId = getFolderByWorkSpaceId(WhereCondition_1, DataRetrieval_1, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getFolderByWorkSpaceId(ByVal WhereCondition_1 As String, _
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                           ByRef Sql_DtTbl As Data.DataTable, _
                                           ByRef eStr_Retu As String) As Boolean
        getFolderByWorkSpaceId = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            getFolderByWorkSpaceId = ObjDtLogic.GetDataset("View_CommonWorkspaceDetail", WhereCondition_1, "", _
                                                           DataRetrieval_1, Sql_DtTbl)
            Return True
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getAllNodesFromHistoryForRevisedSubmission"
    Public Function getAllNodesFromHistoryForRevisedSubmission(ByVal WhereCondition_1 As String, _
                                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                               ByRef Sql_DataSet As Data.DataSet, _
                                                               ByRef eStr_Retu As String) As Boolean
        getAllNodesFromHistoryForRevisedSubmission = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try

            getAllNodesFromHistoryForRevisedSubmission = getAllNodesFromHistoryForRevisedSubmission(WhereCondition_1, _
                                                                                                    DataRetrieval_1, Tbl_1, _
                                                                                                    eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getAllNodesFromHistoryForRevisedSubmission(ByVal WhereCondition_1 As String, _
                                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                               ByRef Sql_DtTbl As Data.DataTable, _
                                                               ByRef eStr_Retu As String) As Boolean
        getAllNodesFromHistoryForRevisedSubmission = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            getAllNodesFromHistoryForRevisedSubmission = ObjDtLogic.GetDataset("Proc_WorkSpaceNodeForRevisedSubmission_Doc", "", _
                                                                               WhereCondition_1, DataRetrieval_1, Sql_DtTbl)
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getWorkspaceTreeNodesForLeafs"
    Public Function getWorkspaceTreeNodesForLeafs(ByVal WhereCondition_1 As String, _
                                                  ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                  ByRef Sql_DataSet As Data.DataSet, _
                                                  ByRef eStr_Retu As String) As Boolean
        getWorkspaceTreeNodesForLeafs = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try

            getWorkspaceTreeNodesForLeafs = getWorkspaceTreeNodesForLeafs(WhereCondition_1, DataRetrieval_1, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getWorkspaceTreeNodesForLeafs(ByVal WhereCondition_1 As String, _
                                                  ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                  ByRef Sql_DtTbl As Data.DataTable, _
                                                  ByRef eStr_Retu As String) As Boolean
        getWorkspaceTreeNodesForLeafs = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            getWorkspaceTreeNodesForLeafs = ObjDtLogic.GetDataset("Proc_WorkSpaceNodeForRevisedSubmission_Doc", "", WhereCondition_1, DataRetrieval_1, Sql_DtTbl)
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getWorkspaceCMSSubmissionInfo"
    Public Function getWorkspaceCMSSubmissionInfo(ByVal WhereCondition_1 As String, _
                                                  ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                  ByRef Sql_DataSet As Data.DataSet, _
                                                  ByRef eStr_Retu As String) As Boolean
        getWorkspaceCMSSubmissionInfo = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try

            getWorkspaceCMSSubmissionInfo = getWorkspaceCMSSubmissionInfo(WhereCondition_1, DataRetrieval_1, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getWorkspaceCMSSubmissionInfo(ByVal WhereCondition_1 As String, _
                                                  ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                  ByRef Sql_DtTbl As Data.DataTable, _
                                                  ByRef eStr_Retu As String) As Boolean
        getWorkspaceCMSSubmissionInfo = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            getWorkspaceCMSSubmissionInfo = ObjDtLogic.GetDataset("View_WorkspaceCMSSubmissionDtl", "", WhereCondition_1, _
                                                                  DataRetrieval_1, Sql_DtTbl)
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getWorkspaceRMSSubmissionInfo"
    Public Function getWorkspaceRMSSubmissionInfo(ByVal WhereCondition_1 As String, _
                                                  ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                  ByRef Sql_DataSet As Data.DataSet, _
                                                  ByRef eStr_Retu As String) As Boolean
        getWorkspaceRMSSubmissionInfo = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try

            getWorkspaceRMSSubmissionInfo = getWorkspaceRMSSubmissionInfo(WhereCondition_1, DataRetrieval_1, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getWorkspaceRMSSubmissionInfo(ByVal WhereCondition_1 As String, _
                                                  ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                  ByRef Sql_DtTbl As Data.DataTable, _
                                                  ByRef eStr_Retu As String) As Boolean
        getWorkspaceRMSSubmissionInfo = False
        Dim ObjDtLogic As New ClsDataLogic_New

        Try
            getWorkspaceRMSSubmissionInfo = ObjDtLogic.GetDataset("SubmissionInfoEUSubDtl", "", WhereCondition_1, _
                                                                  DataRetrieval_1, Sql_DtTbl)
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getChildNodeByParentForPublishForM1"
    Public Function getChildNodeByParentForPublishForM1(ByVal Query As String, _
                                                        ByRef Sql_DataSet As Data.DataSet, _
                                                        ByRef eStr_Retu As String) As Boolean
        getChildNodeByParentForPublishForM1 = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try

            getChildNodeByParentForPublishForM1 = getChildNodeByParentForPublishForM1(Query, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getChildNodeByParentForPublishForM1(ByVal Query As String, _
                                                        ByRef Sql_DtTbl As Data.DataTable, _
                                                        ByRef eStr_Retu As String) As Boolean
        getChildNodeByParentForPublishForM1 = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            getChildNodeByParentForPublishForM1 = ObjDtLogic.GetDataset("workspaceNodeDetail", Query, "", _
                                                                        DataRetrievalModeEnum.DatatTable_Query, Sql_DtTbl)
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getLatestNodeAttrHistory"
    Public Function getLatestNodeAttrHistory(ByVal WhereCondition_1 As String, _
                                             ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                             ByRef Sql_DataSet As Data.DataSet, _
                                             ByRef eStr_Retu As String) As Boolean
        getLatestNodeAttrHistory = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try

            getLatestNodeAttrHistory = getLatestNodeAttrHistory(WhereCondition_1, DataRetrieval_1, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getLatestNodeAttrHistory(ByVal WhereCondition_1 As String, _
                                             ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                             ByRef Sql_DtTbl As Data.DataTable, _
                                             ByRef eStr_Retu As String) As Boolean
        getLatestNodeAttrHistory = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            getLatestNodeAttrHistory = ObjDtLogic.GetDataset("view_NodeVersionHistoryDetail", "", WhereCondition_1, _
                                                             DataRetrieval_1, Sql_DtTbl)
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getAttributesForNodeForPublis"
    Public Function getAttributesForNodeForPublish(ByVal WhereCondition_1 As String, _
                                                   ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                   ByRef Sql_DataSet As Data.DataSet, _
                                                   ByRef eStr_Retu As String) As Boolean
        getAttributesForNodeForPublish = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try

            getAttributesForNodeForPublish = getAttributesForNodeForPublish(WhereCondition_1, DataRetrieval_1, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getAttributesForNodeForPublish(ByVal WhereCondition_1 As String, _
                                                   ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                   ByRef Sql_DtTbl As Data.DataTable, _
                                                   ByRef eStr_Retu As String) As Boolean
        getAttributesForNodeForPublish = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            Dim colParam As New ArrayList(0)
            Dim dsTemp As New DataSet
            Dim params As SqlClient.SqlParameter = Nothing

            colParam.Add(ObjDtLogic.Add_SqlParameter("@vWorkspaceid", WhereCondition_1.Split("#")(1)))
            colParam.Add(ObjDtLogic.Add_SqlParameter("@inodeid", WhereCondition_1.Split("#")(0)))
            colParam.Add(ObjDtLogic.Add_SqlParameter("@iLabelNo", WhereCondition_1.Split("#")(2)))
            colParam.Add(ObjDtLogic.Add_SqlParameter("@AttrForIndi", WhereCondition_1.Split("#")(3)))

            dsTemp = ObjDtLogic.ExecuteSP_DataSet("Proc_AttributesForNodeForPublish", colParam)

            Sql_DtTbl = dsTemp.Tables(0).Copy
            Return True
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getAttributeValueOfModifiedFile"
    Public Function getAttributeValueOfModifiedFile(ByVal WhereCondition_1 As String, _
                                                    ByRef Sql_DataSet As Data.DataSet, _
                                                    ByRef eStr_Retu As String) As Boolean
        getAttributeValueOfModifiedFile = False
        Dim Tbl_1 As Data.DataTable = Nothing
        Try


            getAttributeValueOfModifiedFile = getAttributeValueOfModifiedFile(WhereCondition_1, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getAttributeValueOfModifiedFile(ByVal WhereCondition_1 As String, _
                                                    ByRef Sql_DtTbl As Data.DataTable, _
                                                    ByRef eStr_Retu As String) As Boolean
        getAttributeValueOfModifiedFile = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Dim Query As String = String.Empty

        Try
            Query = "select max(vlastpublishversion) as vLastPublishVersion,indexid " + _
                    " from submittedworkspacenodedetail where " + WhereCondition_1 + _
                    " group by indexid"
            getAttributeValueOfModifiedFile = ObjDtLogic.GetDataset("submittedworkspacenodedetail", Query, "", _
                                                                    DataRetrievalModeEnum.DatatTable_Query, Sql_DtTbl)
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region " getAllNodesLastHistory"
    Public Function getAllNodesLastHistory(ByVal WhereCondition_1 As String, _
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                           ByRef Sql_DataSet As Data.DataSet, _
                                           ByRef eStr_Retu As String) As Boolean
        getAllNodesLastHistory = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try

            getAllNodesLastHistory = getAllNodesLastHistory(WhereCondition_1, DataRetrieval_1, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getAllNodesLastHistory(ByVal WhereCondition_1 As String, _
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                           ByRef Sql_DtTbl As Data.DataTable, _
                                           ByRef eStr_Retu As String) As Boolean
        getAllNodesLastHistory = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            getAllNodesLastHistory = ObjDtLogic.GetDataset("View_NodesLatestHistory", "", WhereCondition_1, DataRetrieval_1, _
                                                           Sql_DtTbl)
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getFileNameForNodeForPublish"
    Public Function getFileNameForNodeForPublish(ByVal WhereCondition_1 As String, _
                                                 ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                 ByRef Sql_DataSet As Data.DataSet, _
                                                 ByRef eStr_Retu As String) As Boolean
        getFileNameForNodeForPublish = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try

            getFileNameForNodeForPublish = getFileNameForNodeForPublish(WhereCondition_1, DataRetrieval_1, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getFileNameForNodeForPublish(ByVal WhereCondition_1 As String, _
                                                 ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                 ByRef Sql_DtTbl As Data.DataTable, _
                                                 ByRef eStr_Retu As String) As Boolean
        getFileNameForNodeForPublish = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            Dim colParam As New ArrayList(0)
            Dim dsTemp As New DataSet
            Dim params As SqlClient.SqlParameter = Nothing

            colParam.Add(ObjDtLogic.Add_SqlParameter("@vWorkspaceId", WhereCondition_1.Split("#")(0)))
            colParam.Add(ObjDtLogic.Add_SqlParameter("@iNodeId", WhereCondition_1.Split("#")(1)))
            colParam.Add(ObjDtLogic.Add_SqlParameter("@iLabelNo", WhereCondition_1.Split("#")(2)))


            dsTemp = ObjDtLogic.ExecuteSP_DataSet("Proc_FileNameForPublish", colParam)

            Sql_DtTbl = dsTemp.Tables(0).Copy

            getFileNameForNodeForPublish = True

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "isLeafNodes"
    Friend Function isLeafNodes(ByVal WhereCondition_1 As String, _
                                ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                ByRef isLeaf As Integer, _
                                ByRef eStr_Retu As String) As Boolean
        isLeafNodes = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Dim Sql_DtTbl As New DataTable
        Try
            isLeafNodes = ObjDtLogic.GetDataset("workspacenodedetail", "", WhereCondition_1, _
                                                DataRetrieval_1, Sql_DtTbl)


            If Sql_DtTbl.Rows.Count > 0 Then
                isLeaf = 0 'parent
            Else
                isLeaf = 1 'Leaf
            End If
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getAllSTFFirstNodes"
    Public Function getAllSTFFirstNodes(ByVal WhereCondition_1 As String, _
                                        ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                        ByRef Sql_DataSet As Data.DataSet, _
                                        ByRef eStr_Retu As String) As Boolean
        getAllSTFFirstNodes = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try

            getAllSTFFirstNodes = getAllSTFFirstNodes(WhereCondition_1, DataRetrieval_1, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getAllSTFFirstNodes(ByVal WhereCondition_1 As String, _
                                        ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                        ByRef Sql_DtTbl As Data.DataTable, _
                                        ByRef eStr_Retu As String) As Boolean
        getAllSTFFirstNodes = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            getAllSTFFirstNodes = ObjDtLogic.GetDataset("View_CommonWorkspaceDetail", "", WhereCondition_1, DataRetrieval_1, _
                                                        Sql_DtTbl)
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getSTFIdentifierByNodeId"
    Public Function getSTFIdentifierByNodeId(ByVal WhereCondition_1 As String, _
                                             ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                             ByRef Sql_DataSet As Data.DataSet, _
                                             ByRef eStr_Retu As String) As Boolean
        getSTFIdentifierByNodeId = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try

            getSTFIdentifierByNodeId = getSTFIdentifierByNodeId(WhereCondition_1, DataRetrieval_1, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getSTFIdentifierByNodeId(ByVal WhereCondition_1 As String, _
                                             ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                             ByRef Sql_DtTbl As Data.DataTable, _
                                             ByRef eStr_Retu As String) As Boolean
        getSTFIdentifierByNodeId = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            getSTFIdentifierByNodeId = ObjDtLogic.GetDataset("stfstudyIdentifiermst", "", WhereCondition_1, DataRetrieval_1, _
                                                             Sql_DtTbl)
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getLastNodeHistory"
    Public Function getLastNodeHistory(ByVal WhereCondition_1 As String, _
                                       ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                       ByRef Sql_DataSet As Data.DataSet, _
                                       ByRef eStr_Retu As String) As Boolean
        getLastNodeHistory = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try

            getLastNodeHistory = getLastNodeHistory(WhereCondition_1, DataRetrieval_1, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getLastNodeHistory(ByVal WhereCondition_1 As String, _
                                       ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                       ByRef Sql_DtTbl As Data.DataTable, _
                                       ByRef eStr_Retu As String) As Boolean
        getLastNodeHistory = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            getLastNodeHistory = ObjDtLogic.GetDataset("View_CommonWorkspaceDetail", "", WhereCondition_1, DataRetrieval_1, _
                                                       Sql_DtTbl)
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getNodeAttributes"
    Public Function getNodeAttributes(ByVal WhereCondition_1 As String, _
                                      ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                      ByRef Sql_DataSet As Data.DataSet, _
                                      ByRef eStr_Retu As String) As Boolean
        getNodeAttributes = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try

            getNodeAttributes = getNodeAttributes(WhereCondition_1, DataRetrieval_1, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getNodeAttributes(ByVal WhereCondition_1 As String, _
                                      ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                      ByRef Sql_DtTbl As Data.DataTable, _
                                      ByRef eStr_Retu As String) As Boolean
        getNodeAttributes = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            getNodeAttributes = ObjDtLogic.GetDataset("workspaceNodeAttrDetail", "", WhereCondition_1, DataRetrieval_1, Sql_DtTbl)
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getChildNodeByParent"
    Public Function getChildNodeByParent(ByVal WhereCondition_1 As String, _
                                         ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                         ByRef Sql_DataSet As Data.DataSet, _
                                         ByRef eStr_Retu As String) As Boolean
        getChildNodeByParent = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try

            getChildNodeByParent = getChildNodeByParent(WhereCondition_1, DataRetrieval_1, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getChildNodeByParent(ByVal WhereCondition_1 As String, _
                                         ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                         ByRef Sql_DtTbl As Data.DataTable, _
                                         ByRef eStr_Retu As String) As Boolean
        getChildNodeByParent = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try

            WhereCondition_1 = "Select * From workspacenodedetail Where " + WhereCondition_1 + " Order By iNodeNo"

            getChildNodeByParent = ObjDtLogic.GetDataset("workspacenodedetail", WhereCondition_1, "", _
                                                         DataRetrievalModeEnum.DatatTable_Query, Sql_DtTbl)


        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getXmlWorkspaceDtl"
    Public Function getXmlWorkspaceDtl(ByVal WhereCondition_1 As String, _
                                       ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                       ByRef Sql_DataSet As Data.DataSet, _
                                       ByRef eStr_Retu As String) As Boolean
        getXmlWorkspaceDtl = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try

            getXmlWorkspaceDtl = getXmlWorkspaceDtl(WhereCondition_1, DataRetrieval_1, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getXmlWorkspaceDtl(ByVal WhereCondition_1 As String, _
                                       ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                       ByRef Sql_DtTbl As Data.DataTable, _
                                       ByRef eStr_Retu As String) As Boolean
        getXmlWorkspaceDtl = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            getXmlWorkspaceDtl = ObjDtLogic.GetDataset("XmlWorkspaceMst", "", WhereCondition_1, DataRetrieval_1, Sql_DtTbl)
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getXmlNodeDtl"
    Public Function getXmlNodeDtl(ByVal WhereCondition_1 As String, _
                                  ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                  ByRef Sql_DataSet As Data.DataSet, _
                                  ByRef eStr_Retu As String) As Boolean
        getXmlNodeDtl = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try

            getXmlNodeDtl = getXmlNodeDtl(WhereCondition_1, DataRetrieval_1, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getXmlNodeDtl(ByVal WhereCondition_1 As String, _
                                  ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                  ByRef Sql_DtTbl As Data.DataTable, _
                                  ByRef eStr_Retu As String) As Boolean
        getXmlNodeDtl = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            getXmlNodeDtl = ObjDtLogic.GetDataset("View_XmlNodeDtl", "", WhereCondition_1, DataRetrieval_1, Sql_DtTbl)
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getXmlChildNodeDtl"
    Public Function getXmlChildNodeDtl(ByVal WhereCondition_1 As String, _
                                       ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                       ByRef Sql_DataSet As Data.DataSet, _
                                       ByRef eStr_Retu As String) As Boolean
        getXmlChildNodeDtl = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try

            getXmlChildNodeDtl = getXmlChildNodeDtl(WhereCondition_1, DataRetrieval_1, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getXmlChildNodeDtl(ByVal WhereCondition_1 As String, _
                                       ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                       ByRef Sql_DtTbl As Data.DataTable, _
                                       ByRef eStr_Retu As String) As Boolean
        getXmlChildNodeDtl = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            getXmlChildNodeDtl = ObjDtLogic.GetDataset("View_XmlNodeDtl", "", WhereCondition_1, DataRetrieval_1, Sql_DtTbl)
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getXmlNodeAttrDtl"
    Public Function getXmlNodeAttrDtl(ByVal WhereCondition_1 As String, _
                                      ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                      ByRef Sql_DataSet As Data.DataSet, _
                                      ByRef eStr_Retu As String) As Boolean
        getXmlNodeAttrDtl = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try

            getXmlNodeAttrDtl = getXmlNodeAttrDtl(WhereCondition_1, DataRetrieval_1, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getXmlNodeAttrDtl(ByVal WhereCondition_1 As String, _
                                      ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                      ByRef Sql_DtTbl As Data.DataTable, _
                                      ByRef eStr_Retu As String) As Boolean
        getXmlNodeAttrDtl = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            getXmlNodeAttrDtl = ObjDtLogic.GetDataset("XmlNodeAttrDtl", "", WhereCondition_1, DataRetrieval_1, Sql_DtTbl)
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getXmlAttrDtl"
    Public Function getXmlAttrDtl(ByVal WhereCondition_1 As String, _
                                  ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                  ByRef Sql_DataSet As Data.DataSet, _
                                  ByRef eStr_Retu As String) As Boolean
        getXmlAttrDtl = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try

            getXmlAttrDtl = getXmlAttrDtl(WhereCondition_1, DataRetrieval_1, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getXmlAttrDtl(ByVal WhereCondition_1 As String, _
                                  ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                  ByRef Sql_DtTbl As Data.DataTable, _
                                  ByRef eStr_Retu As String) As Boolean
        getXmlAttrDtl = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            getXmlAttrDtl = ObjDtLogic.GetDataset("XmlNodeAttrDtl", "", WhereCondition_1, DataRetrieval_1, Sql_DtTbl)
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getWorkspaceCMSSubmissionInfoEU14"
    Public Function getWorkspaceCMSSubmissionInfoEU14(ByVal WhereCondition_1 As String, _
                                                      ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                      ByRef Sql_DataSet As Data.DataSet, _
                                                      ByRef eStr_Retu As String) As Boolean
        getWorkspaceCMSSubmissionInfoEU14 = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try

            getWorkspaceCMSSubmissionInfoEU14 = getWorkspaceCMSSubmissionInfoEU14(WhereCondition_1, DataRetrieval_1, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getWorkspaceCMSSubmissionInfoEU14(ByVal WhereCondition_1 As String, _
                                                      ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                      ByRef Sql_DtTbl As Data.DataTable, _
                                                      ByRef eStr_Retu As String) As Boolean
        getWorkspaceCMSSubmissionInfoEU14 = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            getWorkspaceCMSSubmissionInfoEU14 = ObjDtLogic.GetDataset("View_WorkspaceCMSSubmissionDtlForEU14", "", _
                                                                      WhereCondition_1, DataRetrieval_1, Sql_DtTbl)
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getWorkspaceRMSSubmissionInfoEU14"
    Public Function getWorkspaceRMSSubmissionInfoEU14(ByVal WhereCondition_1 As String, _
                                                      ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                      ByRef Sql_DataSet As Data.DataSet, _
                                                      ByRef eStr_Retu As String) As Boolean
        getWorkspaceRMSSubmissionInfoEU14 = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try

            getWorkspaceRMSSubmissionInfoEU14 = getWorkspaceRMSSubmissionInfoEU14(WhereCondition_1, DataRetrieval_1, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getWorkspaceRMSSubmissionInfoEU14(ByVal WhereCondition_1 As String, _
                                                      ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                      ByRef Sql_DtTbl As Data.DataTable, _
                                                      ByRef eStr_Retu As String) As Boolean
        getWorkspaceRMSSubmissionInfoEU14 = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            getWorkspaceRMSSubmissionInfoEU14 = ObjDtLogic.GetDataset("SubmissionInfoEU14SubDtl", "", WhereCondition_1, _
                                                                      DataRetrieval_1, Sql_DtTbl)
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getCountry"
    Public Function getCountry(ByVal WhereCondition_1 As String, _
                               ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                               ByRef Sql_DataSet As Data.DataSet, _
                               ByRef eStr_Retu As String) As Boolean
        getCountry = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try

            getCountry = getCountry(WhereCondition_1, DataRetrieval_1, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getCountry(ByVal WhereCondition_1 As String, _
                               ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                               ByRef Sql_DtTbl As Data.DataTable, _
                               ByRef eStr_Retu As String) As Boolean
        getCountry = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            getCountry = ObjDtLogic.GetDataset("CountryMst", "", WhereCondition_1, DataRetrieval_1, Sql_DtTbl)
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getChildNodeByParentForPublishFromM2toM5"
    Public Function getChildNodeByParentForPublishFromM2toM5(ByVal WhereCondition_1 As String, _
                                                             ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                             ByRef Sql_DataSet As Data.DataSet, _
                                                             ByRef eStr_Retu As String) As Boolean
        getChildNodeByParentForPublishFromM2toM5 = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try

            getChildNodeByParentForPublishFromM2toM5 = getChildNodeByParentForPublishFromM2toM5(WhereCondition_1, DataRetrieval_1, _
                                                                                                Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getChildNodeByParentForPublishFromM2toM5(ByVal WhereCondition_1 As String, _
                                                             ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                             ByRef Sql_DtTbl As Data.DataTable, _
                                                             ByRef eStr_Retu As String) As Boolean
        getChildNodeByParentForPublishFromM2toM5 = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            getChildNodeByParentForPublishFromM2toM5 = ObjDtLogic.GetDataset("workspaceNodeDetail", "", WhereCondition_1, _
                                                                             DataRetrieval_1, Sql_DtTbl)
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getXmlNodeValue"
    Public Function getXmlNodeValue(ByVal Query As String, _
                                    ByRef Sql_DataSet As Data.DataSet, _
                                    ByRef eStr_Retu As String) As Boolean
        getXmlNodeValue = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try

            getXmlNodeValue = getXmlNodeValue(Query, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getXmlNodeValue(ByVal Query As String, _
                                    ByRef Sql_DtTbl As Data.DataTable, _
                                    ByRef eStr_Retu As String) As Boolean
        getXmlNodeValue = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            Return getRequiredData(Query, Sql_DtTbl, eStr_Retu)
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getXmlAttrValuesForRepeatableNode"
    Public Function getXmlAttrValuesForRepeatableNode(ByVal Query As String, _
                                                      ByRef Sql_DataSet As Data.DataSet, _
                                                      ByRef eStr_Retu As String) As Boolean
        Dim Tbl_1 As Data.DataTable = Nothing
        getXmlAttrValuesForRepeatableNode = False
        Try

            getXmlAttrValuesForRepeatableNode = getXmlAttrValuesForRepeatableNode(Query, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getXmlAttrValuesForRepeatableNode(ByVal Query As String, _
                                                      ByRef Sql_DtTbl As Data.DataTable, _
                                                      ByRef eStr_Retu As String) As Boolean
        getXmlAttrValuesForRepeatableNode = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            Return getRequiredData(Query, Sql_DtTbl, eStr_Retu)
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getRequiredData"
    Public Function getRequiredData(ByVal Query As String, _
                                    ByRef Sql_DataSet As Data.DataSet, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim Tbl_1 As Data.DataTable = Nothing
        getRequiredData = False
        Try

            getRequiredData = getRequiredData(Query, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getRequiredData(ByVal Query As String, _
                                    ByRef Sql_DtTbl As Data.DataTable, _
                                    ByRef eStr_Retu As String) As Boolean
        getRequiredData = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            getRequiredData = ObjDtLogic.GetDataset("workspaceNodeDetail", Query, "", DataRetrievalModeEnum.DatatTable_Query, Sql_DtTbl)
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getXmlAttrValue"
    Public Function getXmlAttrValue(ByVal Query As String, _
                                    ByRef Sql_DataSet As Data.DataSet, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim Tbl_1 As Data.DataTable = Nothing
        getXmlAttrValue = False
        Try

            getXmlAttrValue = getXmlAttrValue(Query, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getXmlAttrValue(ByVal Query As String, _
                                    ByRef Sql_DtTbl As Data.DataTable, _
                                    ByRef eStr_Retu As String) As Boolean
        getXmlAttrValue = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            getXmlAttrValue = ObjDtLogic.GetDataset("workspaceNodeDetail", Query, "", DataRetrievalModeEnum.DatatTable_Query, _
                                                    Sql_DtTbl)
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getWorkspaceTreeNodesForLeafs"
    Public Function getWorkspaceTreeNodesForLeafs(ByVal Param As String, _
                                                  ByRef Sql_DataSet As Data.DataSet, _
                                                  ByRef eStr_Retu As String) As Boolean
        getWorkspaceTreeNodesForLeafs = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try

            getWorkspaceTreeNodesForLeafs = getWorkspaceTreeNodesForLeafs(Param, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getWorkspaceTreeNodesForLeafs(ByVal Param As String, _
                                                  ByRef Sql_DtTbl As Data.DataTable, _
                                                  ByRef eStr_Retu As String) As Boolean
        getWorkspaceTreeNodesForLeafs = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            Dim colParam As New ArrayList(0)
            Dim dsTemp As New DataSet
            Dim params As SqlClient.SqlParameter = Nothing

            colParam.Add(ObjDtLogic.Add_SqlParameter("@WorkSpaceId", Param.Split("#")(0)))
            colParam.Add(ObjDtLogic.Add_SqlParameter("@AllNodeIdsCSV", Param.Split("#")(1)))

            dsTemp = ObjDtLogic.ExecuteSP_DataSet("Proc_WorkspaceTreeNodesForLeafs", colParam)

            Sql_DtTbl = dsTemp.Tables(0).Copy

            getWorkspaceTreeNodesForLeafs = True
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getAllNodesFromHistoryForRevisedSubmission"
    Public Function getAllNodesFromHistoryForRevisedSubmission(ByVal Param As String, _
                                                               ByRef Sql_DataSet As Data.DataSet, _
                                                               ByRef eStr_Retu As String) As Boolean
        getAllNodesFromHistoryForRevisedSubmission = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try

            getAllNodesFromHistoryForRevisedSubmission = getAllNodesFromHistoryForRevisedSubmission(Param, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getAllNodesFromHistoryForRevisedSubmission(ByVal Param As String, _
                                                               ByRef Sql_DtTbl As Data.DataTable, _
                                                               ByRef eStr_Retu As String) As Boolean
        getAllNodesFromHistoryForRevisedSubmission = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            Dim colParam As New ArrayList(0)
            Dim dsTemp As New DataSet
            Dim params As SqlClient.SqlParameter = Nothing

            colParam.Add(ObjDtLogic.Add_SqlParameter("@WorkSpaceId", Param.Split("#")(0)))
            colParam.Add(ObjDtLogic.Add_SqlParameter("@iLabelNo", Param.Split("#")(1)))

            dsTemp = ObjDtLogic.ExecuteSP_DataSet("Proc_WorkSpaceNodeForRevisedSubmission_Doc", colParam)

            Sql_DtTbl = dsTemp.Tables(0).Copy

            getAllNodesFromHistoryForRevisedSubmission = True
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getChildNodeByParentForPublishFromM2toM5"
    Public Function getChildNodeByParentForPublishFromM2toM5(ByVal Query As String, _
                                                             ByRef Sql_DataSet As Data.DataSet, _
                                                             ByRef eStr_Retu As String) As Boolean

        getChildNodeByParentForPublishFromM2toM5 = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try
            getChildNodeByParentForPublishFromM2toM5 = getChildNodeByParentForPublishFromM2toM5(Query, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getChildNodeByParentForPublishFromM2toM5(ByVal Query As String, _
                                                             ByRef Sql_DtTbl As Data.DataTable, _
                                                             ByRef eStr_Retu As String) As Boolean
        getChildNodeByParentForPublishFromM2toM5 = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            getChildNodeByParentForPublishFromM2toM5 = ObjDtLogic.GetDataset("workspaceNodeDetail", Query, "", DataRetrievalModeEnum.DatatTable_Query, Sql_DtTbl)
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getAllChildSTFNodes"
    Public Function getAllChildSTFNodes(ByVal Query As String, _
                                        ByRef Sql_DataSet As Data.DataSet, _
                                        ByRef eStr_Retu As String) As Boolean
        Dim Tbl_1 As Data.DataTable = Nothing
        getAllChildSTFNodes = False

        Try

            getAllChildSTFNodes = getAllChildSTFNodes(Query, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getAllChildSTFNodes(ByVal Query As String, _
                                        ByRef Sql_DtTbl As Data.DataTable, _
                                        ByRef eStr_Retu As String) As Boolean
        getAllChildSTFNodes = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            getAllChildSTFNodes = ObjDtLogic.GetDataset("view_commonworkspacedetail", Query, "", _
                                                        DataRetrievalModeEnum.DatatTable_Query, Sql_DtTbl)
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getChildNodeByParent"
    Public Function getChildNodeByParent(ByVal Query As String, _
                                         ByRef Sql_DataSet As Data.DataSet, _
                                         ByRef eStr_Retu As String) As Boolean
        getChildNodeByParent = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try

            getChildNodeByParent = getChildNodeByParent(Query, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getChildNodeByParent(ByVal Query As String, _
                                         ByRef Sql_DtTbl As Data.DataTable, _
                                         ByRef eStr_Retu As String) As Boolean
        getChildNodeByParent = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            getChildNodeByParent = ObjDtLogic.GetDataset("workspaceNodeDetail", Query, "", _
                                                         DataRetrievalModeEnum.DatatTable_Query, Sql_DtTbl)
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region
#Region "getNodeDetail"
    Public Function getNodeDetail(ByVal WhereCondition_1 As String, _
                                  ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                  ByRef DTOSubmissionMst As DataSet, _
                                  ByRef eStr As String) As Boolean

        getNodeDetail = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try
            getNodeDetail = getNodeDetail(WhereCondition_1, DataRetrieval_1, Tbl_1, eStr)

            DTOSubmissionMst = New DataSet
            DTOSubmissionMst.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr = ex.Message
        End Try

    End Function
    Friend Function getNodeDetail(ByVal WhereCondition_1 As String, _
                                  ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                  ByRef DTOSubmissionMst As DataTable, _
                                  ByRef eStr As String) As Boolean
        getNodeDetail = False
        Dim ObjData As New ClsDataLogic_New

        Try
            getNodeDetail = ObjData.GetDataset("workspaceNodeDetail", "", WhereCondition_1, DataRetrieval_1, DTOSubmissionMst)

            DTOSubmissionMst.Columns.Add(New DataColumn("vApplicationNo", GetType(String)))
            DTOSubmissionMst.Columns.Add(New DataColumn("vTrackingNo", GetType(String)))

        Catch ex As Exception
            eStr = ex.Message
        Finally
            ObjData = Nothing
        End Try

    End Function
#End Region
#Region "getParentNodeID"
    Public Function getParentNodeId(ByVal WhereCondition_1 As String, _
                                    ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                    ByRef iParentNodeId As Integer, _
                                    ByRef eStr As String) As Boolean

        getParentNodeId = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try
            getParentNodeId = getParentNodeId(WhereCondition_1, iParentNodeId, DataRetrieval_1, eStr)

        Catch ex As Exception
            eStr = ex.Message
        End Try

    End Function
    Friend Function getParentNodeId(ByVal WhereCondition_1 As String, _
                                    ByRef iParentNodeId As Integer, _
                                    ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                    ByRef eStr As String) As Boolean
        getParentNodeId = False
        Dim ObjData As New ClsDataLogic_New
        Dim Tbl_1 As New DataTable

        Try
            getParentNodeId = ObjData.GetDataset("workspacenodedetail", "", WhereCondition_1, DataRetrieval_1, Tbl_1)

            If Tbl_1 IsNot Nothing AndAlso Tbl_1.Rows.Count > 0 Then
                iParentNodeId = Tbl_1.Rows(0)("iParentNodeId")
            End If

        Catch ex As Exception
            eStr = ex.Message
        Finally
            ObjData = Nothing
        End Try

    End Function
#End Region

#Region "View_GetCMSCountry"
    Public Function View_GetCMSCountry(ByVal WhereCondition_1 As String, _
                                       ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                       ByRef Ds_Retrun As DataSet, _
                                       ByRef eStr As String) As Boolean

        View_GetCMSCountry = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try
            View_GetCMSCountry = View_GetCMSCountry(WhereCondition_1, DataRetrieval_1, Tbl_1, eStr)

            Ds_Retrun.Tables.Add(Tbl_1.Copy)
        Catch ex As Exception
            eStr = ex.Message
        End Try

    End Function
    Friend Function View_GetCMSCountry(ByVal WhereCondition_1 As String, _
                                       ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                       ByRef Dt_Retrun As DataTable, _
                                       ByRef eStr As String) As Boolean
        View_GetCMSCountry = False
        Dim ObjData As New ClsDataLogic_New

        Try
            View_GetCMSCountry = ObjData.GetDataset("View_GetCMSCountry", "", WhereCondition_1, DataRetrieval_1, Dt_Retrun)



        Catch ex As Exception
            eStr = ex.Message
        Finally
            ObjData = Nothing
        End Try

    End Function
#End Region

#Region "GetSubmissionTypeMst"
    Public Function GetSubmissionTypeMst(ByVal WhereCondition_1 As String, _
                                         ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                         ByRef Ds_Retrun As DataSet, _
                                         ByRef eStr As String) As Boolean

        GetSubmissionTypeMst = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try
            GetSubmissionTypeMst = GetSubmissionTypeMst(WhereCondition_1, DataRetrieval_1, Tbl_1, eStr)

            Ds_Retrun.Tables.Add(Tbl_1.Copy)
        Catch ex As Exception
            eStr = ex.Message
        End Try

    End Function
    Friend Function GetSubmissionTypeMst(ByVal WhereCondition_1 As String, _
                                         ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                         ByRef Dt_Retrun As DataTable, _
                                         ByRef eStr As String) As Boolean
        GetSubmissionTypeMst = False
        Dim ObjData As New ClsDataLogic_New

        Try
            GetSubmissionTypeMst = ObjData.GetDataset("SubmissionTypeMst", "", WhereCondition_1, DataRetrieval_1, Dt_Retrun)



        Catch ex As Exception
            eStr = ex.Message
        Finally
            ObjData = Nothing
        End Try

    End Function


    Public Function GetMyProjectCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim items As System.Collections.Generic.List(Of String) = New System.Collections.Generic.List(Of String)()

        If count = 0 Then
            count = 10
        End If

        Dim ds As New DataSet()
        Dim estr As String = ""
        Dim result As Boolean = False
        Dim dr As DataRow = Nothing
        Dim whereCondition As String = "vWorkspaceDesc + vRequestId  + vProjectNo + vClientName   " + " Like '%"
        whereCondition += prefixText + "%'" + IIf(contextKey.Trim() <> "", " AND " & contextKey.Trim(), "")



        result = GetFieldsOfTable("View_Myprojects", " * ", whereCondition, ds, estr)

        For index As Integer = 0 To ds.Tables(0).Rows.Count - 1
            dr = ds.Tables(0).Rows(index)
            items.Add("'" + dr.Item("vWorkspaceid").ToString + "#" + dr.Item("vWorkspaceDesc").ToString + _
                "#" + dr.Item("vProjectNo").ToString + "#" + _
                dr.Item("vClientName").ToString + "#" + dr.Item("vRequestId").ToString())



        Next

        Return items.ToArray()
    End Function

    Public Function GetFieldsOfTable(ByVal TableName As String, _
                                          ByVal Columns As String, _
                                          ByVal WhereCondition As String, _
                                          ByRef Sql_DataSet As Data.DataSet, _
                                          ByRef eStr_Retu As String) As Boolean
        Dim qStr As String = ""
        Dim objDbLogic As New ClsDataLogic_New

        If String.IsNullOrEmpty(WhereCondition) Then
            qStr = "Select " & Columns & " from " & TableName
        Else
            qStr = "Select " & Columns & " from " & TableName & " where " & WhereCondition
        End If

        Sql_DataSet = objDbLogic.ExecuteQuery_DataSet(qStr)
        Return True

    End Function
#End Region

#Region " Proc_GetAttributeSpecificParentNodes "
    Public Function Proc_GetAttributeSpecificParentNodes(ByVal WorkspaceId As String, _
                                        ByVal NodeId As String, _
                                        ByRef Sql_DataSet As Data.DataSet, _
                                        ByRef eStr_Retu As String) As Boolean
        Try

            Dim ObjDtLogic As New ClsDataLogic_New
            Dim Param As String = ""
            Param = WorkspaceId + "##" + NodeId + "##"
            Sql_DataSet = ObjDtLogic.ProcedureExecute("Proc_GetAttributeSpecificParentNodes", Param)
            Return True

        Catch ex As Exception
            eStr_Retu = ex.Message
            Return False
        End Try
    End Function
#End Region  '==============Added on 25-10-2011===================

#Region " Proc_ViewInternalLabel "
    Public Function Proc_ViewInternalLabel(ByVal WorkspaceId As String, _
                                        ByRef Sql_DataSet As Data.DataSet, _
                                        ByRef eStr_Retu As String) As Boolean
        Try

            Dim ObjDtLogic As New ClsDataLogic_New
            Dim Param As String = ""
            Param = WorkspaceId
            Sql_DataSet = ObjDtLogic.ProcedureExecute("Proc_ViewInternalLabel", Param)
            Return True

        Catch ex As Exception
            eStr_Retu = ex.Message
            Return False
        End Try
    End Function
#End Region                '============== Added On 07-11-2011 =================

#Region " proc_SubmissionNodeDtl "
    Public Function proc_SubmissionNodeDtl(ByVal WorkspaceId As String, _
                                        ByVal iLabelNo As Integer, _
                                        ByRef Sql_DataSet As Data.DataSet, _
                                        ByRef eStr_Retu As String) As Boolean
        Try

            Dim ObjDtLogic As New ClsDataLogic_New
            Dim Param As String = ""
            Param = WorkspaceId + "##" + iLabelNo.ToString + "##"
            Sql_DataSet = ObjDtLogic.ProcedureExecute("proc_SubmissionNodeDtl", Param)
            Return True

        Catch ex As Exception
            eStr_Retu = ex.Message
            Return False
        End Try
    End Function
#End Region                '============== Added On 07-11-2011 =================

#Region " proc_publishVersion "
    Public Function proc_publishVersion(ByVal vWorkspaceId As String, _
                                        ByRef Sql_DataSet As Data.DataSet, _
                                        ByRef eStr_Retu As String) As Boolean
        Try

            Dim ObjDtLogic As New ClsDataLogic_New
            Dim Param As String = ""
            Param = vWorkspaceId
            Sql_DataSet = ObjDtLogic.ProcedureExecute("proc_publishVersion", Param)
            Return True

        Catch ex As Exception
            eStr_Retu = ex.Message
            Return False
        End Try
    End Function
#End Region                   '============== Added On 07-11-2011 =================

#Region "getSubmissionInfoEU14Dtl"
    Public Function getSubmissionInfoEU14Dtl(ByVal WhereCondition_1 As String, _
                                                  ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                  ByRef Sql_DataSet As Data.DataSet, _
                                                  ByRef eStr_Retu As String) As Boolean
        getSubmissionInfoEU14Dtl = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try

            getSubmissionInfoEU14Dtl = getSubmissionInfoEU14Dtl(WhereCondition_1, DataRetrieval_1, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
    Friend Function getSubmissionInfoEU14Dtl(ByVal WhereCondition_1 As String, _
                                                  ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                  ByRef Sql_DtTbl As Data.DataTable, _
                                                  ByRef eStr_Retu As String) As Boolean
        getSubmissionInfoEU14Dtl = False
        Dim ObjDtLogic As New ClsDataLogic_New
        Try
            getSubmissionInfoEU14Dtl = ObjDtLogic.GetDataset("SubmissionInfoEU14Dtl", "", WhereCondition_1, DataRetrieval_1, Sql_DtTbl)
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region                '============== Added On 07-11-2011 =================

#Region "getsubmittedWorkspacenodedetail"
    Public Function getsubmittedWorkspacenodedetail(ByVal WhereCondition_1 As String, _
                                         ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                         ByRef Ds_Retrun As DataSet, _
                                         ByRef eStr As String) As Boolean

        getsubmittedWorkspacenodedetail = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try
            getsubmittedWorkspacenodedetail = getsubmittedWorkspacenodedetail(WhereCondition_1, DataRetrieval_1, Tbl_1, eStr)

            Ds_Retrun.Tables.Add(Tbl_1.Copy)
        Catch ex As Exception
            eStr = ex.Message
        End Try

    End Function
    Friend Function getsubmittedWorkspacenodedetail(ByVal WhereCondition_1 As String, _
                                         ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                         ByRef Dt_Retrun As DataTable, _
                                         ByRef eStr As String) As Boolean
        getsubmittedWorkspacenodedetail = False
        Dim ObjData As New ClsDataLogic_New

        Try
            getsubmittedWorkspacenodedetail = ObjData.GetDataset("submittedWorkspacenodedetail", "", WhereCondition_1, DataRetrieval_1, Dt_Retrun)

        Catch ex As Exception
            eStr = ex.Message
        Finally
            ObjData = Nothing
        End Try

    End Function
#End Region            '============== Added On 08-11-2011 =================

#Region "view_commonworkspaceDetail"
    Public Function view_commonworkspaceDetail(ByVal WhereCondition_1 As String, _
                                       ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                       ByRef Ds_Retrun As DataSet, _
                                       ByRef eStr As String) As Boolean

        view_commonworkspaceDetail = False
        Dim Tbl_1 As Data.DataTable = Nothing

        Try
            view_commonworkspaceDetail = view_commonworkspaceDetail(WhereCondition_1, DataRetrieval_1, Tbl_1, eStr)

            Ds_Retrun.Tables.Add(Tbl_1.Copy)
        Catch ex As Exception
            eStr = ex.Message
        End Try

    End Function
    Friend Function view_commonworkspaceDetail(ByVal WhereCondition_1 As String, _
                                       ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                       ByRef Dt_Retrun As DataTable, _
                                       ByRef eStr As String) As Boolean
        view_commonworkspaceDetail = False
        Dim ObjData As New ClsDataLogic_New

        Try
            view_commonworkspaceDetail = ObjData.GetDataset("view_commonworkspaceDetail", "", WhereCondition_1, DataRetrieval_1, Dt_Retrun)



        Catch ex As Exception
            eStr = ex.Message
        Finally
            ObjData = Nothing
        End Try

    End Function
#End Region              ' ================ Added On 09-11-2011 ====================

#Region "getcreateWorkspaceLabel"
    Public Function getcreateWorkspaceLabel(ByVal WorkspaceId As String, _
                                            ByVal UserId As String, _
                                            ByRef ds_InternalLabelMst As DataSet, _
                                            ByRef eStr As String) As Boolean

        getcreateWorkspaceLabel = False
        Dim Tbl As New DataTable

        Try
            getcreateWorkspaceLabel = getcreateWorkspaceLabel(WorkspaceId, UserId, Tbl, eStr)
            ds_InternalLabelMst.Tables.Add(Tbl)
        Catch ex As Exception
            eStr = ex.Message
        End Try

    End Function
    Friend Function getcreateWorkspaceLabel(ByVal WorkspaceId As String, _
                                            ByVal UserId As String, _
                                            ByRef dt_InternalLabelMst As DataTable, _
                                            ByRef eStr As String) As Boolean
        getcreateWorkspaceLabel = False
        Dim objSaveDb As clsSaveDB = Nothing

        Dim maxLabelId As String = String.Empty
        Dim tempLabelIdString As String = String.Empty

        Dim tempLabelIdInteger As Integer


        Try

            If Not getMaxWorkspaceLabel("WorkspaceId='" + WorkspaceId + "'", _
                                                  DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                  dt_InternalLabelMst, eStr) Then
                Throw New Exception(eStr)
            End If

            maxLabelId = dt_InternalLabelMst.Rows(0)("LabelId").ToString()

            'Increment labelId by 1, e.g. L0001 to L0002
            tempLabelIdInteger = Convert.ToInt32(maxLabelId.Substring(maxLabelId.Length - 4)) 'Integer 0001
            tempLabelIdInteger = tempLabelIdInteger + 1 'integer 0002
            tempLabelIdString = "000" + tempLabelIdInteger.ToString() 'string 0002
            tempLabelIdString = maxLabelId.Substring(0, maxLabelId.Length - 4) + _
                                tempLabelIdString.Substring(tempLabelIdString.Length - 4) 'string L0001

            dt_InternalLabelMst.Rows(0)("LabelId") = tempLabelIdString
            dt_InternalLabelMst.Rows(0)("ModifyBy") = UserId

            objSaveDb = New clsSaveDB
            If Not objSaveDb.createInternalLabel(DataObjOpenSaveModeEnum.DataObjOpenMode_Add, _
                                                 dt_InternalLabelMst, estr) Then
                Throw New Exception(eStr)
            End If

            If Not getMaxWorkspaceLabel("WorkspaceId='" + WorkspaceId + "'", _
                                                  DataRetrievalModeEnum.DataTable_WithWhereCondition, _
                                                  dt_InternalLabelMst, estr) Then
                Throw New Exception(eStr)
            End If

            Return True
        Catch ex As Exception
            estr = ex.Message
        Finally

        End Try

    End Function
#End Region

#Region "Proc_NewTranNo"
    Public Function Proc_NewTranNo(ByVal Choice As DataObjOpenSaveModeEnum, _
                                ByVal DsWSDtlECTD As DataSet, _
                                ByVal UserCode_1 As String, _
                                ByRef eStr_Retu As String, _
                                ByRef iTranNo As Integer) As Boolean
        Dim objDtLogic As New ClsDataLogic_New

        Dim dataRet_1 As DataRetrievalModeEnum
        Dim objHelpDbTbl As clsHelpDbTable = Nothing

        'Object of friend class
        Dim objSave As SaveDataInDbObj = Nothing

        Dim Sql_DtSet As Data.DataSet = Nothing
        Dim Tbl_Audit As Data.DataTable = Nothing
        Dim serverDateTime_1 As DateTime

        Dim Instr_WorkspaceId As String = ""

        Dim PkColArr(0) As String
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

            If DsWSDtlECTD Is Nothing Then
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

            'Retrieval of Online_AuditInfo
            If Not objHelpDbTbl.getAuditorial("", DataRetrievalModeEnum.DataTable_Empty, objDtLogic, _
                                              Tbl_Audit, eStr_Retu) Then
                eStr_Retu += vbCrLf + "Error occured while retrieving Audit Information "
                Exit Function
            End If

            'Assignment of Values
            PkColArr(0) = "vWorkspaceId"

            If Not objSave.AssignValues(DsWSDtlECTD.Tables(0).TableName, PkColArr, Choice, _
                                        DsWSDtlECTD.Tables(DsWSDtlECTD.Tables(0).TableName), _
                                        DsWSDtlECTD.Tables(0), Tbl_Audit, False, UserCode_1, serverDateTime_1, eStr_Retu) Then
                eStr_Retu += vbCrLf + "Error occured while assigning values"
                Exit Function
            End If


            ' Save in WorkspaceNodeDetail

            If DsWSDtlECTD.Tables(0).Rows.Count > 0 Then
                If Not objSave.SaveInDb("Proc_NewTranNo", DsWSDtlECTD.Tables(0), Choice, DataReturnList, eStr_Retu) Then
                    eStr_Retu += vbCrLf + "Error occured while Save Information of WorkspaceDtlECTD In Database"
                    Exit Function
                End If

            End If


            DataReturnList = DataReturnList.Item(0)
            If DataReturnList.Count > 0 Then
                RetuVal = DataReturnList.Item(0)
                iTranNo = RetuVal.ParameterValue
            End If

            Proc_NewTranNo = True
            'Logic of Save End

            If Proc_NewTranNo Then
                objDtLogic.Commit_Transaction()
            Else
                objDtLogic.RollBack_Transaction()
            End If

        Catch ex As Exception
            eStr_Retu = ex.Message + vbCrLf + "Error occured while saving WorkspaceDtlECTD"
            Proc_NewTranNo = False
            objDtLogic.RollBack_Transaction()
        Finally
            If Not objDtLogic Is Nothing Then
                objDtLogic.Close_Connection()
                objDtLogic = Nothing
            End If
        End Try
    End Function
#End Region


#Region "GetWorkspaceDtlEctd "
    Public Function GetWorkspaceDtlEctd(ByVal WhereCondition_1 As String, _
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                               ByRef Sql_DataSet As Data.DataSet, _
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjDtLogic As ClsDataLogic_New = Nothing
        Dim Tbl_1 As Data.DataTable = Nothing

        Try

            ObjDtLogic = New ClsDataLogic_New
            GetWorkspaceDtlEctd = GetWorkspaceDtlEctd(WhereCondition_1, DataRetrieval_1, ObjDtLogic, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try

    End Function
    Friend Function GetWorkspaceDtlEctd(ByVal WhereCondition_1 As String, _
                               ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                               ByVal ObjDtLogic As ClsDataLogic_New, _
                               ByRef Sql_DtTbl As Data.DataTable, _
                               ByRef eStr_Retu As String) As Boolean


        If DataRetrieval_1 = DataRetrievalModeEnum.DatatTable_Query Then
            eStr_Retu = "Invalid Data Retrieval Option"
            Exit Function
        End If

        Try
            GetWorkspaceDtlEctd = ObjDtLogic.GetDataset("WorkspaceDtlEctd", "", WhereCondition_1, DataRetrieval_1, Sql_DtTbl)
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try


    End Function
#End Region

End Class