Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols

<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class WS_KnowledgeNETPublish
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function HelloWorld() As String
        Return "Hello World"
    End Function

    <WebMethod()> _
    Public Function saveSubmissionDtl(ByVal userId As Integer, _
                                      ByVal subType As String, _
                                      ByVal workspaceId As String, _
                                      ByVal applicationNumber As String, _
                                      ByVal projectPublishType As Char, _
                                      ByVal currentSeqNumber As String, _
                                      ByVal lastPublishedVersion As String, _
                                      ByVal dos As Date, _
                                      ByVal relatedSeqNumber As String, _
                                      ByVal subMode As String, _
                                      ByVal isRMSSelected As Char, _
                                      ByVal subVariationMode As String, _
                                      ByVal addTT As Char, _
                                      ByVal lastConfirmedSubmissionPath As String, _
                                      ByVal baseWorkFolder As String, _
                                      ByVal subDesc As String, _
                                      ByVal selectedCMS() As Integer, _
                                      ByVal trackCMS() As String, _
                                      ByRef eStr_Reru As String) As Boolean

        Dim objPublish As New clsPublishLogic
        saveSubmissionDtl = objPublish.saveSubmissionDtl(userId, subType, workspaceId, applicationNumber, projectPublishType, _
                                                        currentSeqNumber, lastPublishedVersion, dos, relatedSeqNumber, _
                                                        subMode, isRMSSelected, subVariationMode, addTT, _
                                                        lastConfirmedSubmissionPath, baseWorkFolder, subDesc, _
                                                        selectedCMS, trackCMS, eStr_Reru)
    End Function

    <WebMethod()> _
    Public Function getWorkspaceTreeNodesForLeafs(ByVal Param As String, _
                                                  ByRef Sql_DataSet As Data.DataSet, _
                                                  ByRef eStr_Retu As String) As Boolean
        getWorkspaceTreeNodesForLeafs = False
        Try
            Dim objPublish As New clsHelpDB
            getWorkspaceTreeNodesForLeafs = objPublish.getWorkspaceTreeNodesForLeafs(Param, Sql_DataSet, eStr_Retu)
        Catch ex As Exception

        End Try
    End Function

    <WebMethod()> _
    Public Function View_GetCMSCountry(ByVal WhereCondition_1 As String, _
                                       ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                       ByRef Ds_Retrun As DataSet, _
                                       ByRef eStr As String) As Boolean
        Dim objHelpDB As New clsHelpDB

        View_GetCMSCountry = objHelpDB.View_GetCMSCountry(WhereCondition_1, DataRetrieval_1, Ds_Retrun, eStr)
    End Function

    <WebMethod()> _
    Public Function GetSubmissionTypeMst(ByVal WhereCondition_1 As String, _
                                         ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                         ByRef Ds_Retrun As DataSet, _
                                         ByRef eStr As String) As Boolean
        Dim objHelpDB As New clsHelpDB

        GetSubmissionTypeMst = objHelpDB.GetSubmissionTypeMst(WhereCondition_1, DataRetrieval_1, Ds_Retrun, eStr)
    End Function

    <WebMethod(EnableSession:=True)> _
    <System.Web.Script.Services.ScriptMethod()> _
    Public Function GetMyProjectCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim DBHelp As New clsHelpDB
        Dim item() As String
        Dim Objcommon As New clsCommon
        Dim Wstr_ScopeValue As String = ""

        If Not GetScopeValueWithCondition(Wstr_ScopeValue) Then
            Return Nothing
        End If

        'Changed on 01-Sept-2009
        If Not IsNothing(contextKey) AndAlso contextKey.ToString.Trim() <> "" Then
            contextKey += " And " + Wstr_ScopeValue
        Else
            contextKey = Wstr_ScopeValue
        End If
        '*****************************************

        item = DBHelp.GetMyProjectCompletionList(prefixText, count, contextKey)
        Return item

    End Function

    Public Function GetScopeValueWithCondition(ByRef wStr_ScopeValue As String) As Boolean
        GetScopeValueWithCondition = False
        Try
            If Convert.ToString(HttpContext.Current.Session("ScopeValue")).Length > 0 Then
                wStr_ScopeValue = Convert.ToString(HttpContext.Current.Session("ScopeValue"))
                wStr_ScopeValue = " vProjectTypeCode in (" + wStr_ScopeValue + ")"
                GetScopeValueWithCondition = True
            End If
        Catch ex As Exception

        End Try
    End Function

#Region " Proc_GetAttributeSpecificParentNodes "
    <WebMethod()> _
        Public Function Proc_GetAttributeSpecificParentNodes(ByVal WorkspaceId As String, _
                                        ByVal NodeId As String, _
                                        ByRef Sql_DataSet As Data.DataSet, _
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDB = Nothing

        ObjHelp = New clsHelpDB
        Proc_GetAttributeSpecificParentNodes = ObjHelp.Proc_GetAttributeSpecificParentNodes(WorkspaceId, NodeId, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 25-10-2011=============

#Region "Insert_WorkspaceNodeAttrDetail_ForEctd"
    <WebMethod()> _
    Public Function Insert_WorkspaceNodeAttrDetail_ForEctd(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Ds_WorkspaceNodeAttrDetail_ForEctd As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objSavedb As New clsSaveDB
        Insert_WorkspaceNodeAttrDetail_ForEctd = objSavedb.Insert_WorkspaceNodeAttrDetail_ForEctd(Choice_1, Ds_WorkspaceNodeAttrDetail_ForEctd, UserCode_1, eStr_Retu)

    End Function
#End Region '

    <WebMethod()> _
    Public Function getWorkspaceSubmissionInfoEU14DtlBySubmissionId(ByVal WhereCondition_1 As String, _
                                         ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                         ByRef Ds_Retrun As DataSet, _
                                         ByRef eStr As String) As Boolean
        Dim objHelpDB As New clsHelpDB

        getWorkspaceSubmissionInfoEU14DtlBySubmissionId = objHelpDB.getWorkspaceSubmissionInfoEU14DtlBySubmissionId(WhereCondition_1, DataRetrieval_1, Ds_Retrun, eStr)
    End Function

#Region " Proc_ViewInternalLabel"
    <WebMethod()> _
        Public Function Proc_ViewInternalLabel(ByVal WorkspaceId As String, _
                                        ByRef Sql_DataSet As Data.DataSet, _
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDB = Nothing

        ObjHelp = New clsHelpDB
        Proc_ViewInternalLabel = ObjHelp.Proc_ViewInternalLabel(WorkspaceId, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region " proc_SubmissionNodeDtl"
    <WebMethod()> _
        Public Function proc_SubmissionNodeDtl(ByVal WorkspaceId As String, _
                                               ByVal iLabelNo As Integer, _
                                            ByRef Sql_DataSet As Data.DataSet, _
                                            ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDB = Nothing

        ObjHelp = New clsHelpDB
        proc_SubmissionNodeDtl = ObjHelp.proc_SubmissionNodeDtl(WorkspaceId, iLabelNo, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region " proc_publishVersion"
    <WebMethod()> _
        Public Function proc_publishVersion(ByVal vWorkspaceId As String, _
                                        ByRef Sql_DataSet As Data.DataSet, _
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDB = Nothing

        ObjHelp = New clsHelpDB
        proc_publishVersion = ObjHelp.proc_publishVersion(vWorkspaceId, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Insert_SubmittedWorkspaceNodeDetail"
    <WebMethod()> _
    Public Function Insert_SubmittedWorkspaceNodeDetail(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Ds_SubmittedWorkspaceNodeDetail As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objSavedb As New clsSaveDB
        Insert_SubmittedWorkspaceNodeDetail = objSavedb.Insert_SubmittedWorkspaceNodeDetail(Choice_1, Ds_SubmittedWorkspaceNodeDetail, UserCode_1, eStr_Retu)

    End Function
#End Region

#Region "Insert_SubmissionInfoEU14Dtl"
    <WebMethod()> _
    Public Function Insert_SubmissionInfoEU14Dtl(ByVal ParamOut_1 As String, _
                                        ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                        ByVal Dt_SubmissionInfoEU14Dtl As DataTable, _
                                        ByRef eStr_Retu As String) As Boolean
        Dim objSavedb As New clsSaveDB
        Insert_SubmissionInfoEU14Dtl = objSavedb.Insert_SubmissionInfoEU14Dtl(ParamOut_1, Choice_1, Dt_SubmissionInfoEU14Dtl, eStr_Retu)

    End Function
#End Region

#Region "getSubmissionInfoEU14Dtl"
    <WebMethod()> _
    Public Function getSubmissionInfoEU14Dtl(ByVal wStr As String, _
                                             ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                                  ByRef Sql_DataSet As Data.DataSet, _
                                                  ByRef eStr_Retu As String) As Boolean
        getSubmissionInfoEU14Dtl = False
        Try
            Dim objPublish As New clsHelpDB
            getSubmissionInfoEU14Dtl = objPublish.getSubmissionInfoEU14Dtl(wStr, DataRetrieval_1, Sql_DataSet, eStr_Retu)
        Catch ex As Exception

        End Try
    End Function
#End Region

#Region "getsubmittedWorkspacenodedetail"
    <WebMethod()> _
    Public Function getsubmittedWorkspacenodedetail(ByVal WhereCondition_1 As String, _
                                         ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                         ByRef Ds_Retrun As DataSet, _
                                         ByRef eStr As String) As Boolean
        Dim objHelpDB As New clsHelpDB

        getsubmittedWorkspacenodedetail = objHelpDB.getsubmittedWorkspacenodedetail(WhereCondition_1, DataRetrieval_1, Ds_Retrun, eStr)
    End Function
#End Region

#Region "view_commonworkspaceDetail"
    <WebMethod()> _
    Public Function view_commonworkspaceDetail(ByVal wStr As String, _
                                            ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                            ByRef Sql_DataSet As Data.DataSet, _
                                            ByRef eStr_Retu As String) As Boolean
        view_commonworkspaceDetail = False
        Try
            Dim objPublish As New clsHelpDB
            view_commonworkspaceDetail = objPublish.view_commonworkspaceDetail(wStr, DataRetrieval_1, Sql_DataSet, eStr_Retu)
        Catch ex As Exception

        End Try
    End Function
#End Region

#Region "getcreateWorkspaceLabel"
    <WebMethod()> _
    Public Function getcreateWorkspaceLabel(ByVal workspaceId As String, _
                                                  ByVal userId As String, _
                                                  ByRef dsInternalLabelMst As DataSet, _
                                                  ByRef eStr As String) As Boolean
        getcreateWorkspaceLabel = False
        Try
            Dim objPublishLogic As New clsHelpDB
            getcreateWorkspaceLabel = objPublishLogic.getcreateWorkspaceLabel(workspaceId, userId, dsInternalLabelMst, eStr)
        Catch ex As Exception

        End Try
    End Function
#End Region

#Region "Proc_NewTranNo"
    <WebMethod()> _
    Public Function Proc_NewTranNo(ByVal Choice As DataObjOpenSaveModeEnum, _
                                ByVal DsWsEctdDtl As DataSet, _
                                ByVal UserCode_1 As String, _
                                ByRef eStr_Retu As String, _
                                ByRef iTranNo As Integer) As Boolean
        Proc_NewTranNo = False
        Try
            Dim objPublishLogic As New clsHelpDB
            Proc_NewTranNo = objPublishLogic.Proc_NewTranNo(Choice, DsWsEctdDtl, UserCode_1, eStr_Retu, iTranNo)
        Catch ex As Exception

        End Try
    End Function
#End Region

#Region "GetWorkspaceDtlEctd"
    <WebMethod()> _
        Public Function GetWorkspaceDtlEctd(ByVal WhereCondition_1 As String, _
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                               ByRef Sql_DataSet As Data.DataSet, _
                                               ByRef eStr_Retu As String) As Boolean
        Dim objHelpDB As clsHelpDB = Nothing

        objHelpDB = New clsHelpDB
        GetWorkspaceDtlEctd = objHelpDB.GetWorkspaceDtlEctd(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "publishWorkspaceSubmission"
    <WebMethod()> _
    Public Function publishWorkspaceSubmission(ByRef dtoSubmissionMst As DataTable, _
                                               ByRef dtoInternalLabelMst As DataTable, _
                                               ByVal currentSeqNumber As String, _
                                               ByVal relatedSeqNumber As String, _
                                               ByVal publishDestinationPath As String, _
                                               ByVal sourcePath As String, _
                                               ByVal isRMSSelected As Char, _
                                               ByVal subVariationMode As String, _
                                               ByVal addTT As Char, _
                                               ByVal leafIds() As Integer, _
                                               ByVal dos As Date, _
                                               ByVal userId As Integer, _
                                               ByVal subType As String, _
                                               ByVal baseWorkFolder As String, _
                                               ByRef eStr_Retu As String) As Boolean
        publishWorkspaceSubmission = False
        Try
            Dim objPublishLogic As New clsPublishLogic
            publishWorkspaceSubmission = objPublishLogic.publishWorkspaceSubmission(dtoSubmissionMst, dtoInternalLabelMst, _
                                               currentSeqNumber, relatedSeqNumber, publishDestinationPath, sourcePath, _
                                               isRMSSelected, subVariationMode, addTT, Nothing, dos, userId, subType, baseWorkFolder, eStr_Retu)
        Catch ex As Exception

        End Try
    End Function
#End Region

#Region "getWorkspaceSubmissionInfoUSDtlBySubmissionId"
    <WebMethod()> _
    Public Function getWorkspaceSubmissionInfoUSDtlBySubmissionId(ByVal WhereCondition_1 As String, _
                                         ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                         ByRef Ds_Retrun As DataSet, _
                                         ByRef eStr As String) As Boolean
        Dim objHelpDB As New clsHelpDB

        getWorkspaceSubmissionInfoUSDtlBySubmissionId = objHelpDB.getWorkspaceSubmissionInfoUSDtlBySubmissionId(WhereCondition_1, DataRetrieval_1, Ds_Retrun, eStr)
    End Function
#End Region

#Region "getWorkspaceSubmissionInfoCADtlBySubmissionId"
    <WebMethod()> _
    Public Function getWorkspaceSubmissionInfoCADtlBySubmissionId(ByVal WhereCondition_1 As String, _
                                         ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                         ByRef Ds_Retrun As DataSet, _
                                         ByRef eStr As String) As Boolean
        Dim objHelpDB As New clsHelpDB

        getWorkspaceSubmissionInfoCADtlBySubmissionId = objHelpDB.getWorkspaceSubmissionInfoCADtlBySubmissionId(WhereCondition_1, DataRetrieval_1, Ds_Retrun, eStr)
    End Function
#End Region

#Region "Insert_SubmissionInfoCADtl"
    <WebMethod()> _
    Public Function Insert_SubmissionInfoCADtl(ByVal ParamOut_1 As String, _
                                        ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                        ByVal Dt_SubmissionInfoEU14Dtl As DataTable, _
                                        ByRef eStr_Retu As String) As Boolean
        Dim objSavedb As New clsSaveDB
        Insert_SubmissionInfoCADtl = objSavedb.Insert_SubmissionInfoCADtl(ParamOut_1, Choice_1, Dt_SubmissionInfoEU14Dtl, eStr_Retu)

    End Function
#End Region

#Region "Insert_SubmissionInfoUSDtl"
    <WebMethod()> _
    Public Function Insert_SubmissionInfoUSDtl(ByVal ParamOut_1 As String, _
                                        ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                        ByVal Dt_SubmissionInfoEU14Dtl As DataTable, _
                                        ByRef eStr_Retu As String) As Boolean
        Dim objSavedb As New clsSaveDB
        Insert_SubmissionInfoUSDtl = objSavedb.Insert_SubmissionInfoUSDtl(ParamOut_1, Choice_1, Dt_SubmissionInfoEU14Dtl, eStr_Retu)

    End Function
#End Region

#Region "getWorkspaceRMSSubmissionInfoEU14"
    <WebMethod()> _
    Public Function getWorkspaceRMSSubmissionInfoEU14(ByVal WhereCondition_1 As String, _
                                         ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
                                         ByRef Ds_Retrun As DataSet, _
                                         ByRef eStr As String) As Boolean
        Dim objHelpDB As New clsHelpDB

        getWorkspaceRMSSubmissionInfoEU14 = objHelpDB.getWorkspaceRMSSubmissionInfoEU14(WhereCondition_1, DataRetrieval_1, Ds_Retrun, eStr)
    End Function
#End Region
End Class