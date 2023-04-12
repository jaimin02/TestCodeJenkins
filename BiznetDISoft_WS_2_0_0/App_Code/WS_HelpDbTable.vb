Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports Newtonsoft.Json
Imports SS.Web.Services

<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.None)>
<System.Web.Script.Services.ScriptService()>
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Public Class WS_HelpDbTable
    Inherits sWebService

#Region "HelloWorld"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function HelloWorld() As String
        Return "Hello World"
    End Function
#End Region

#Region "getTemplateMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getTemplateMst(ByVal WhereCondition_1 As String,
                                       ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                       ByRef Sql_DataSet As Data.DataSet,
                                       ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getTemplateMst = ObjHelp.getTemplateMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "getTemplateNodeDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getTemplateNodeDetail(ByVal WhereCondition_1 As String,
                                       ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                       ByRef Sql_DataSet As Data.DataSet,
                                       ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getTemplateNodeDetail = ObjHelp.getTemplateNodeDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetActivityCodeDetails"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetActivityCodeDetails(ByRef Sql_DataSet As Data.DataSet,
                                            ByVal Wstr As String,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetActivityCodeDetails = ObjHelp.GetActivityCodeDetails(Sql_DataSet, Wstr, eStr_Retu)
    End Function
#End Region

#Region "GetActivityStageDetailByActivityId"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetActivityStageDetailByActivityId(ByVal vActivityId As String,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetActivityStageDetailByActivityId = ObjHelp.GetActivityStageDetailByActivityId(vActivityId, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "getProjectStartingInNext7Days"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getProjectStartingInNext7Days(ByVal WhereCondition_1 As String,
                                             ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                             ByRef Sql_DataSet As Data.DataSet,
                                             ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getProjectStartingInNext7Days = ObjHelp.getProjectStartingInNext7Days(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "getWorkspaceSubjectComment"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getWorkspaceSubjectComment(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getWorkspaceSubjectComment = ObjHelp.getWorkspaceSubjectComment(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "TemplateWorkflowUserDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getTemplateWorkflowUserDtl(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getTemplateWorkflowUserDtl = ObjHelp.getTemplateWorkflowUserDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "WorkspaceNodeAttrHistory"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getWorkspaceNodeAttrHistory(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getWorkspaceNodeAttrHistory = ObjHelp.getWorkspaceNodeAttrHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_GetNodeWithSubjectCount"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetNodeWithSubjectCount(ByVal vWorkSpaceId As String,
        ByVal iParentNodeId As Integer,
        ByRef sql_DataSet As Data.DataSet,
        ByRef estr_Retu As String) As Boolean
        Dim objHelp As clsHelpDbTable = Nothing
        objHelp = New clsHelpDbTable
        Proc_GetNodeWithSubjectCount = objHelp.Proc_GetNodeWithSubjectCount(vWorkSpaceId, iParentNodeId, sql_DataSet, estr_Retu)
    End Function
#End Region

#Region "GetMyProjectCompletionListForArchive" 'Added on 30-07-2012 By vikas shah
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMyProjectCompletionListForArchive(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetMyProjectCompletionListForArchive(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#Region "GetView_ProjectDetails"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetView_ProjectDetails(ByVal wstr As String,
        ByRef sql_Dataset As Data.DataSet,
        ByRef estr_Retu As String) As Boolean
        Dim objHelp As clsHelpDbTable = Nothing
        objHelp = New clsHelpDbTable
        GetView_ProjectDetails = objHelp.GetView_ProjectDetails(wstr, sql_Dataset, estr_Retu)
    End Function
#End Region

#Region "GetWorkspaceNodeHistory"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getWorkspaceNodeHistory(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getWorkspaceNodeHistory = ObjHelp.getWorkspaceNodeHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region


    '==============================

    'Added By Naimesh Dave

#Region "OperationMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getOperationMst(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getOperationMst = ObjHelp.getOperationMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "UserTypeMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getUserTypeMst(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getUserTypeMst = ObjHelp.getUserTypeMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "roleOperationMatrix"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getroleOperationMatrix(ByVal WhereCondition_1 As String,
                                         ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                         ByRef Sql_DataSet As Data.DataSet,
                                         ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getroleOperationMatrix = ObjHelp.getroleOperationMatrix(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "workspacemst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getworkspacemst(ByVal WhereCondition_1 As String,
                                        ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getworkspacemst = ObjHelp.getworkspacemst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "drugmst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getdrugmst(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getdrugmst = ObjHelp.getdrugmst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "regionmst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getregionmst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getregionmst = ObjHelp.getregionmst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "userMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getuserMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getuserMst = ObjHelp.getuserMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "userGroupMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getuserGroupMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getuserGroupMst = ObjHelp.getuserGroupMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "resourcemst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getresourcemst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getresourcemst = ObjHelp.getresourcemst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "projectTypeMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getprojectTypeMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getprojectTypeMst = ObjHelp.getprojectTypeMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "clientmst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getclientmst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getclientmst = ObjHelp.getclientmst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "WorkspaceSpecificSubject"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getWorkspaceSpecificSubject(ByVal WhereCondition_1 As String,
                                                ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                                ByRef Sql_Dataset As Data.DataSet,
                                                ByRef estr_retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getWorkspaceSpecificSubject = ObjHelp.getWorkspaceSpecificSubject(WhereCondition_1, DataRetrieval_1, Sql_Dataset, estr_retu)
    End Function
#End Region
#Region "ClientContactMatrix "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getClientContactMatrix(ByVal WhereCondition_1 As String,
                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                               ByRef Sql_DataSet As Data.DataSet,
                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getClientContactMatrix = ObjHelp.getClientContactMatrix(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function
#End Region

#Region "ValidateUser"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function ValidateUser(ByVal UserName As String, ByVal Password As String) As DataSet
        Dim objCommon As New clsCommon
        Return objCommon.ValidateUser(UserName, Password)
    End Function
#End Region

#Region "GetDataSet"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetResultSet(ByVal strQuery As String, ByVal strTableNAme As String) As DataSet
        Dim objDataLogic As New ClsDataLogic_New
        Return objDataLogic.ExecuteQuery_DataSet(strQuery, strTableNAme)
    End Function
#End Region

#Region "GetMenu"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMenu(ByVal UserTypeCode As String) As DataSet
        Dim objcommon As New clsCommon
        Return objcommon.getMenu(UserTypeCode)
    End Function
#End Region

#Region "getworkspaceSubjectDocDetails"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getworkspaceSubjectDocDetails(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getworkspaceSubjectDocDetails = ObjHelp.getWorkspaceSubjectDocDetails(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetDocSubjectDetails"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetDocSubjectDetails(ByVal WorkSpaceId As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetDocSubjectDetails = ObjHelp.GetDocSubjectDetails(WorkSpaceId, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetProjectActivityCurrAttr from View"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetProjectActivityCurrAttr(ByVal WorkSpaceId As String,
                                      ByRef Sql_DataSet As Data.DataSet,
                                      ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetProjectActivityCurrAttr = ObjHelp.GetProjectActivityCurrAttr(WorkSpaceId, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetView_SubjectDocDetails"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetView_SubjectDocDetails(ByVal Wstr As String,
                                       ByRef Sql_DataSet As Data.DataSet,
                                       ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        GetView_SubjectDocDetails = ObjHelp.GetView_SubjectDocDetails(Wstr, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "getLocationMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getLocationMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getLocationMst = ObjHelp.getLocationMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "FillDropDown"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function FillDropDown(ByVal TableName As String,
                                        ByVal DataValeField As String,
                                        ByVal DataTextField As String,
                                        ByVal WhereCondition As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        FillDropDown = ObjHelp.FillDropDown(TableName, DataValeField, DataTextField, WhereCondition, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetStageMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetStageMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetStageMst = ObjHelp.GetStageMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetSetProjectMatrix"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSetProjectMatrix(ByVal WhereCondition_1 As String,
                                        ByVal Dataretrival_1 As DataRetrievalModeEnum,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        GetSetProjectMatrix = ObjHelp.GetSetProjectMatrix(WhereCondition_1, Dataretrival_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetDoctypeMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetDoctypeMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetDoctypeMst = ObjHelp.GetDoctypeMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetDeptStageMatrix"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetDeptStageMatrix(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetDeptStageMatrix = ObjHelp.GetDeptStageMatrix(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetNodeidwiseStage"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetNodeidwiseStage(ByVal NodeId As String,
                                            ByVal TemplateId As String,
                                            ByRef Sql_DataSet As Data.DataSet,
                                            ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetNodeidwiseStage = ObjHelp.GetNodeidwiseStage(NodeId, TemplateId, Sql_DataSet, eStr_Retu)
    End Function
#End Region


#Region "GetViewProjectActivityCurrAttr"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewProjectActivityCurrAttr(ByVal Wstr As String,
                                                       ByRef Sql_DataSet As Data.DataSet,
                                                       ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewProjectActivityCurrAttr = ObjHelp.GetViewProjectActivityCurrAttr(Wstr, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetDataForCommentGrid"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetDataForCommentGrid(ByVal WStr As String,
                                            ByRef Sql_DataSet As Data.DataSet,
                                            ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetDataForCommentGrid = ObjHelp.GetDataForCommentGrid(WStr, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetFieldsOfTable"
    ''' <summary>
    ''' Get Periculare Columns of Table with/without Where 
    ''' </summary>
    ''' <param name="TableName">TableName</param>
    ''' <param name="Columns">Columns to get</param>
    ''' <param name="WhereCondition">Where Condition</param>
    ''' <param name="Sql_DataSet">Return Dataset</param>
    ''' <param name="eStr_Retu">Error Msg</param>
    ''' <returns>DataSet</returns>
    ''' <remarks></remarks>
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetFieldsOfTable(ByVal TableName As String,
                                         ByVal Columns As String,
                                         ByVal WhereCondition As String,
                                         ByRef Sql_DataSet As Data.DataSet,
                                         ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetFieldsOfTable = ObjHelp.GetFieldsOfTable(TableName, Columns, WhereCondition, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetViewTemplateWorkflowUserDtl"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewTemplateWorkflowUserDtl(ByVal Wstr As String,
                                                       ByRef Sql_DataSet As Data.DataSet,
                                                       ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewTemplateWorkflowUserDtl = ObjHelp.GetViewTemplateWorkflowUserDtl(Wstr, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetWorkspaceCommentMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetWorkspaceCommentMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetWorkspaceCommentMst = ObjHelp.GetWorkspaceCommentMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetWorkspaceCommentDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetWorkspaceCommentDetail(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetWorkspaceCommentDetail = ObjHelp.GetWorkspaceCommentDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "getWorkSpaceNodeDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getWorkSpaceNodeDetail(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getWorkSpaceNodeDetail = ObjHelp.getWorkSpaceNodeDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "getworkspaceWorkflowUserDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getworkspaceWorkflowUserDtl(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getworkspaceWorkflowUserDtl = ObjHelp.getworkspaceWorkflowUserDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetViewWorkspaceWorkflowUserDtl"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewWorkspaceWorkflowUserDtl(ByVal Wstr As String,
                                                       ByRef Sql_DataSet As Data.DataSet,
                                                       ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewWorkspaceWorkflowUserDtl = ObjHelp.GetViewWorkspaceWorkflowUserDtl(Wstr, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetCheckedoutfiledetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetCheckedoutfiledetail(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetCheckedoutfiledetail = ObjHelp.GetCheckedoutfiledetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_GetWorkspaceCommentDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetWorkspaceCommentDetail(ByVal vWorkSpaceId As String,
                                                ByVal iNodeId As String,
                                                ByVal iToUserId As String,
                                                ByRef Sql_DataSet As Data.DataSet,
                                                ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_GetWorkspaceCommentDetail = ObjHelp.Proc_GetWorkspaceCommentDetail(vWorkSpaceId, iNodeId, iToUserId, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetActivityOperationMatrix"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetActivityOperationMatrix(ByVal WhereCondition_1 As String,
                                         ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                         ByRef Sql_DataSet As Data.DataSet,
                                         ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetActivityOperationMatrix = ObjHelp.GetActivityOperationMatrix(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region " getProtocolCriterienMst "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getProtocolCriterienMst(ByVal WhereCondition_1 As String,
                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                               ByRef Sql_DataSet As Data.DataSet,
                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        getProtocolCriterienMst = ObjHelp.getProtocolCriterienMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region " getProtocolWorkspaceCriterienDtls "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getProtocolWorkspaceCriterienDtls(ByVal WhereCondition_1 As String,
                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                               ByRef Sql_DataSet As Data.DataSet,
                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        getProtocolWorkspaceCriterienDtls = ObjHelp.getProtocolWorkspaceCriterienDtls(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region " getProtocolWorkSpaceDetails "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getProtocolWorkSpaceDetails(ByVal WhereCondition_1 As String,
                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                               ByRef Sql_DataSet As Data.DataSet,
                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        getProtocolWorkSpaceDetails = ObjHelp.getProtocolWorkSpaceDetails(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region " GetDrugRegionMatrix "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetDrugRegionMatrix(ByVal WhereCondition_1 As String,
                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                               ByRef Sql_DataSet As Data.DataSet,
                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetDrugRegionMatrix = ObjHelp.GetDrugRegionMatrix(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region " GetDrugRegionPKMatrix "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetDrugRegionPKMatrix(ByVal WhereCondition_1 As String,
                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                               ByRef Sql_DataSet As Data.DataSet,
                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetDrugRegionPKMatrix = ObjHelp.GetDrugRegionPKMatrix(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Doctemplatemst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getDoctemplatemst(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getDoctemplatemst = ObjHelp.getDoctemplatemst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "CheckUserRoleOperation"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function CheckUserRoleOperation(ByVal OperationName As String,
                                                  ByVal UserTypeCode As String,
                                                  ByRef ValidRoleOperation As String,
                                                  ByRef Sql_DS As Data.DataSet,
                                                  ByRef eStr_Retu As String) As Boolean


        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        CheckUserRoleOperation = ObjHelp.CheckUserRoleOperation(OperationName, UserTypeCode, ValidRoleOperation, Sql_DS, eStr_Retu)
    End Function
#End Region



#Region "GetMedExScreeningDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMedExScreeningDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetMedExScreeningDtl = ObjHelp.GetMedExScreeningDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetMedExScreeningHdr"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMedExScreeningHdr(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetMedExScreeningHdr = ObjHelp.GetMedExScreeningHdr(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetMedExInfoHdr"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMedExInfoHdr(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetMedExInfoHdr = ObjHelp.GetMedExInfoHdr(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetMedExInfoDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMedExInfoDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetMedExInfoDtl = ObjHelp.GetMedExInfoDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "getSubjectLanguageMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getSubjectLanguageMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getSubjectLanguageMst = ObjHelp.getSubjectLanguageMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetDocTypeTemplateMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetWorkspaceSubjectMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetWorkspaceSubjectMst = ObjHelp.GetWorkspaceSubjectMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

    'Added by Bhargav Thaker Start
#Region "GetCRFBunchMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetCRFBunchMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetCRFBunchMst = ObjHelp.GetCRFBunchMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetCRFBunchSubMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetCRFBunchSubMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetCRFBunchSubMst = ObjHelp.GetCRFBunchSubMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "CRFBunch Validation"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function CRFBunch_Validation(ByVal vWorkspaceId As String, ByVal vSubjectId As String, ByVal iBunchNo As String, ByVal iNodeId As String, ByRef eStr_Retu As String) As Boolean
        Dim objcommon As New clsCommon
        CRFBunch_Validation = objcommon.CRFBunch_Validation(vWorkspaceId, vSubjectId, iBunchNo, iNodeId, eStr_Retu)
    End Function
#End Region

#Region "Get ParentWorkspaceId"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetParentWorkspaceId(ByVal vWorkspaceId As String) As String
        Dim objcommon As New clsCommon
        GetParentWorkspaceId = objcommon.GetParentWorkspaceId(vWorkspaceId)
    End Function
#End Region

#Region "Get CRF Bunch Records"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetCRFBunchRecord(ByVal vWorkspaceId As String, ByVal vSubjectId As String) As DataSet
        Dim objcommon As New clsCommon
        GetCRFBunchRecord = objcommon.GetCRFBunchRecord(vWorkspaceId, vSubjectId)
    End Function
#End Region

#Region "GetVisitList"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetVisitList(ByVal vWorkspaceId As String, ByVal vSubjectId As String) As DataSet
        Dim objcommon As New clsCommon
        GetVisitList = objcommon.GetVisitList(vWorkspaceId, vSubjectId)
    End Function
#End Region

#Region "GetBunchId"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetBunchId(ByVal vWorkspaceId As String, ByVal vSubjectId As String) As String
        Dim objcommon As New clsCommon
        GetBunchId = objcommon.GetBunchId(vWorkspaceId, vSubjectId)
    End Function
#End Region

#Region "GetActivityId"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetActivityId(ByVal vWorkspaceId As String, ByVal iNodeId As String) As String
        Dim objcommon As New clsCommon
        GetActivityId = objcommon.GetActivityId(vWorkspaceId, iNodeId)
    End Function
#End Region

#Region "Get Template Details"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetTemplateDetails(ByVal vWorkspaceId As String) As DataSet
        Dim objcommon As New clsCommon
        GetTemplateDetails = objcommon.GetTemplateDetails(vWorkspaceId)
    End Function
#End Region

#Region "Get Project Name"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetProjectName(ByVal vWorkspaceId As String, ByVal iUserId As String) As String
        Dim objcommon As New clsCommon
        GetProjectName = objcommon.GetProjectName(vWorkspaceId, iUserId)
    End Function
#End Region

#Region "Get SubjectNo"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubjectNo(ByVal vWorkspaceId As String, ByVal vSubjectId As String) As String
        Dim objcommon As New clsCommon
        GetSubjectNo = objcommon.GetSubjectNo(vWorkspaceId, vSubjectId)
    End Function
#End Region
    'Added by Bhargav Thaker End


#Region " getActivityMst "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getActivityGroupMst(ByVal WhereCondition_1 As String,
                                       ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                       ByRef Sql_DataSet As Data.DataSet,
                                       ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getActivityGroupMst = ObjHelp.getActivityGroupMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region " Proc_ProjectNodeCommandButtonRights "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_ProjectNodeCommandButtonRights(ByVal WorkspaceId As String,
                                        ByVal UserId As String,
                                        ByVal MileStone As String,
                                        ByVal Operational As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_ProjectNodeCommandButtonRights = ObjHelp.Proc_ProjectNodeCommandButtonRights(WorkspaceId, UserId, MileStone, Operational, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region " GetProjectDetailsForHdrUsrWise "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetProjectDetailsForHdrUsrWise(ByVal vWorkspaceId As String,
                                                    ByVal iUserId As String,
                                                    ByRef Sql_DataSet As Data.DataSet,
                                                    ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        GetProjectDetailsForHdrUsrWise = ObjHelp.GetProjectDetailsForHdrUsrWise(vWorkspaceId, iUserId, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetViewWorkSpaceNodeDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewWorkSpaceNodeDetail(ByVal WhereCondition As String,
                                                     ByRef Sql_DataSet As Data.DataSet,
                                                     ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewWorkSpaceNodeDetail = ObjHelp.GetViewWorkSpaceNodeDetail(WhereCondition, Sql_DataSet, eStr_Retu)
    End Function
#End Region

    'Add by shivani pandya
#Region "GetViewCRFUploadGuidelineDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetCRFUploadGuidelineDetail(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable

        GetCRFUploadGuidelineDetail = ObjHelp.GetCRFUploadGuidelineDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region
#Region "GetViewCRFUploadGuidelineDetailHistory"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewCRFUploadGuidelineDetailHistory(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable

        GetViewCRFUploadGuidelineDetailHistory = ObjHelp.GetViewCRFUploadGuidelineDetailHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

    'end

#Region "View_MedExInfoHdrDtl_Edit"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_MedExInfoHdrDtl_Edit(ByVal WhereCondition As String,
                                                     ByVal Columns As String,
                                                     ByRef Sql_DataSet As Data.DataSet,
                                                     ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_MedExInfoHdrDtl_Edit = ObjHelp.View_MedExInfoHdrDtl_Edit(WhereCondition, Columns, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetMedexInfoHdrQC"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMedexInfoHdrQC(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetMedexInfoHdrQC = ObjHelp.GetMedexInfoHdrQC(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetViewWorkSpaceSubjectComment"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewWorkSpaceSubjectComment(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewWorkSpaceSubjectComment = ObjHelp.GetViewWorkSpaceSubjectComment(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetViewClientContactMatrix"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewClientContactMatrix(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewClientContactMatrix = ObjHelp.GetViewClientContactMatrix(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetViewMedExWorkSpaceDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewMedExWorkSpaceDtl(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewMedExWorkSpaceDtl = ObjHelp.GetViewMedExWorkSpaceDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetViewUserMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewUserMst(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewUserMst = ObjHelp.GetViewUserMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region


#Region "GetView_UserLoginFailureDetails"

    ' =================== Added By Jeet Patel on 03-Jun-2015 ===================
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetView_UserLoginFailureDetails(ByVal WhereCondition_1 As String,
                                                    ByVal DataRetrival_1 As DataRetrievalModeEnum,
                                                    ByRef Sql_DataSet As Data.DataSet,
                                                    ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable

        GetView_UserLoginFailureDetails = ObjHelp.GetView_UserLoginFailureDetails(WhereCondition_1, DataRetrival_1, Sql_DataSet, eStr_Retu)

    End Function
    ' ==========================================================================
#End Region

#Region "GetVIEWMedExWorkspaceHdr"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetVIEWMedExWorkspaceHdr(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetVIEWMedExWorkspaceHdr = ObjHelp.GetVIEWMedExWorkspaceHdr(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetViewHolidayMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewHolidayMst(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewHolidayMst = ObjHelp.GetViewHolidayMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetViewDrugAnalytesMatrix "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewDrugAnalytesMatrix(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewDrugAnalytesMatrix = ObjHelp.GetViewDrugAnalytesMatrix(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetViewDrugRegionPKMatrix "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewDrugRegionPKMatrix(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewDrugRegionPKMatrix = ObjHelp.GetViewDrugRegionPKMatrix(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetViewDrugRegionAnalytesRpt "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewDrugRegionAnalytesRpt(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewDrugRegionAnalytesRpt = ObjHelp.GetViewDrugRegionAnalytesRpt(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetViewDrugRegionMatrix "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewDrugRegionMatrix(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewDrugRegionMatrix = ObjHelp.GetViewDrugRegionMatrix(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetViewProjectNodeUserRightsDetails "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewProjectNodeUserRightsDetails(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewProjectNodeUserRightsDetails = ObjHelp.GetViewProjectNodeUserRightsDetails(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetViewWorkspaceComments "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewWorkspaceComments(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewWorkspaceComments = ObjHelp.GetViewWorkspaceComments(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetViewResourceMst "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewResourceMst(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewResourceMst = ObjHelp.GetViewResourceMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetViewRoleOperationMatrix "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewRoleOperationMatrix(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewRoleOperationMatrix = ObjHelp.GetViewRoleOperationMatrix(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetSubjectHabitDetails "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubjectHabitDetails(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSubjectHabitDetails = ObjHelp.GetSubjectHabitDetails(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetSubjectHabitMst "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubjectHabitMst(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSubjectHabitMst = ObjHelp.GetSubjectHabitMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "getViewMedExScreeningHdrDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getViewMedExScreeningHdrDtl(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getViewMedExScreeningHdrDtl = ObjHelp.getViewMedExScreeningHdrDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetWorkspaceSubjectMaster"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetWorkspaceSubjectMaster(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetWorkspaceSubjectMaster = ObjHelp.getWorkspaceSubjectMaster(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetViewTemplateMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewTemplateMst(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewTemplateMst = ObjHelp.GetViewTemplateMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetviewProjectTypeMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetviewProjectTypeMst(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetviewProjectTypeMst = ObjHelp.GetviewProjectTypeMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetviewActivityGroupMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetviewActivityGroupMst(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetviewActivityGroupMst = ObjHelp.GetviewActivityGroupMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetViewgetWorkspaceDetailForHdr "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewgetWorkspaceDetailForHdr(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewgetWorkspaceDetailForHdr = ObjHelp.GetViewgetWorkspaceDetailForHdr(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetViewProjectActivityAtrributes "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewProjectActivityAtrributes(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewProjectActivityAtrributes = ObjHelp.GetViewProjectActivityAtrributes(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetViewdeptstagematrix "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewdeptstagematrix(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewdeptstagematrix = ObjHelp.GetViewdeptstagematrix(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetViewDelayedActivityProjects "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewDelayedActivityProjects(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewDelayedActivityProjects = ObjHelp.GetViewDelayedActivityProjects(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region " Proc_GetUserbyScopeValue "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetUserbyScopeValue(ByVal Parameters As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetUserbyScopeValue = ObjHelp.Proc_GetUserbyScopeValue(Parameters, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 29-11-2011 by Mrunal=============

#Region "View_WorkspaceDefaultWorkflowUserDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_WorkspaceDefaultWorkflowUserDtl_New(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_WorkspaceDefaultWorkflowUserDtl_New = ObjHelp.View_WorkspaceDefaultWorkflowUserDtl_New(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 15-12-2011 by Mrunal=============

#Region " Proc_GetProjectChilds "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetProjectChilds(ByVal Parameters As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetProjectChilds = ObjHelp.Proc_GetProjectChilds(Parameters, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 29-05-2012 by Pratiksha =============




    '*******For FFR

#Region "GetMTPHdr "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMTPHdr(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetMTPHdr = ObjHelp.GetMTPHdr(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetMTPDtl "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMTPDtl(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetMTPDtl = ObjHelp.GetMTPDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetDWRHdr "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetDWRHdr(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetDWRHdr = ObjHelp.GetDWRHdr(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetDWRDetail "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetDWRDetail(ByVal WhereCondition_1 As String,
                                ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                ByRef Sql_DataSet As Data.DataSet,
                                ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetDWRDetail = ObjHelp.GetDWRDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetSTP "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSTP(ByVal WhereCondition_1 As String,
                                ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                ByRef Sql_DataSet As Data.DataSet,
                                ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSTP = ObjHelp.GetSTP(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetOtherExpMst "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetOtherExpMst(ByVal WhereCondition_1 As String,
                                ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                ByRef Sql_DataSet As Data.DataSet,
                                ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetOtherExpMst = ObjHelp.GetOtherExpMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetOtherExpDtl "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetOtherExpDtl(ByVal WhereCondition_1 As String,
                                ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                ByRef Sql_DataSet As Data.DataSet,
                                ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetOtherExpDtl = ObjHelp.GetOtherExpDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetPlaceMst "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetPlaceMst(ByVal WhereCondition_1 As String,
                                ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                ByRef Sql_DataSet As Data.DataSet,
                                ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        GetPlaceMst = ObjHelp.GetPlaceMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetStateMSt "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetStateMSt(ByVal WhereCondition_1 As String,
                                ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                ByRef Sql_DataSet As Data.DataSet,
                                ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetStateMSt = ObjHelp.GetStateMSt(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetViewSTPWithScope "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewSTPWithScope(ByVal WhereCondition_1 As String,
                                ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                ByRef Sql_DataSet As Data.DataSet,
                                ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        GetViewSTPWithScope = ObjHelp.GetViewSTPWithScope(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetViewMTPInfo "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewMTPInfo(ByVal WhereCondition_1 As String,
                                ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                ByRef Sql_DataSet As Data.DataSet,
                                ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        GetViewMTPInfo = ObjHelp.GetViewMTPInfo(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Get_ProcMTPMonthWise"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_ProcMTPMonthWise(ByVal dDate As String,
                                                ByVal UserId As String,
                                                ByRef Sql_DS As Data.DataSet,
                                                ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Get_ProcMTPMonthWise = ObjHelp.Get_ProcMTPMonthWise(dDate, UserId, Sql_DS, eStr_Retu)
    End Function
#End Region

#Region "GetViewSTP "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewSTP(ByVal WhereCondition_1 As String,
                                ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                ByRef Sql_DataSet As Data.DataSet,
                                ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        GetViewSTP = ObjHelp.GetViewSTP(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region " GetViewSTPUserWise "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewSTPUserWise(ByVal UserId As String,
                                       ByRef Sql_DataSet As Data.DataSet,
                                       ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        GetViewSTPUserWise = ObjHelp.GetViewSTPUserWise(UserId, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetReasonMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetReasonMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetReasonMst = ObjHelp.GetReasonMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_DWRLeaveHoliPerMonth"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_DWRLeaveHoliPerMonth(ByVal iUserId As Integer,
                                           ByVal vLocationCode As String,
                                           ByVal dDate As String,
                                           ByRef ds_retu As Data.DataSet,
                                           ByRef eStr_retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable

        Proc_DWRLeaveHoliPerMonth = ObjHelp.Proc_DWRLeaveHoliPerMonth(iUserId, vLocationCode, dDate, ds_retu, eStr_retu)
    End Function
#End Region

#Region "IsDcrLock"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function IsDcrLock(ByVal iUserId As Integer,
                              ByVal LocationCode As String,
                              ByVal Choice_1 As DataObjOpenSaveModeEnum,
                              ByVal DCRDate_1 As Date,
                              ByRef IsEditing As Boolean,
                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable

        IsDcrLock = ObjHelp.IsDcrLock(iUserId, LocationCode, Choice_1, DCRDate_1, IsEditing, eStr_Retu)
    End Function
#End Region

#Region " GetOtherExpHdr "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetOtherExpHdr(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetOtherExpHdr = ObjHelp.GetOtherExpHdr(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region " GetViewUserWiseDWR "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewUserWiseDWR(ByVal WhereCondition_1 As String,
                                                   ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                                   ByRef Sql_DataSet As Data.DataSet,
                                                   ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewUserWiseDWR = ObjHelp.GetViewUserWiseDWR(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region " GetViewUserWiseExpense "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewUserWiseExpense(ByVal WhereCondition_1 As String,
                                                   ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                                   ByRef Sql_DataSet As Data.DataSet,
                                                   ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewUserWiseExpense = ObjHelp.GetViewUserWiseExpense(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region " GetParameterList "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetParameterList(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetParameterList = ObjHelp.GetParameterList(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region " GetParameterDeptMatrix "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetParameterDeptMatrix(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetParameterDeptMatrix = ObjHelp.GetParameterDeptMatrix(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

    '*******END FFR

#Region " getActivityDocLinkMatrix "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getActivityDocLinkMatrix(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getActivityDocLinkMatrix = ObjHelp.getActivityDocLinkMatrix(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region " GetColumnNames "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetColumnNames(ByVal TableName As String,
                                     ByRef Sql_DS As Data.DataSet,
                                     ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetColumnNames = ObjHelp.GetColumnNames(TableName, Sql_DS, eStr_Retu)
    End Function
#End Region

#Region " GetTableNames "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetTableNames(ByVal TableType As String,
                                     ByRef Sql_DS As Data.DataSet,
                                     ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetTableNames = ObjHelp.GetTableNames(TableType, Sql_DS, eStr_Retu)
    End Function
#End Region

#Region "Proc_TemplateTreeView"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_TemplateTreeView(ByVal vTemplateId As String,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_TemplateTreeView = ObjHelp.Proc_TemplateTreeView(vTemplateId, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_GetMedExWorkSpaceDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetMedExWorkSpaceDtl(ByVal vWorkSpaceId As String,
                                              ByVal vActivityId As String,
                                              ByVal iPeriod As String,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetMedExWorkSpaceDtl = ObjHelp.Proc_GetMedExWorkSpaceDtl(vWorkSpaceId, vActivityId, iPeriod, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_MedExInfoHdrDtlEdit"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_MedExInfoHdrDtlEdit(ByVal vWorkSpaceId As String,
                                              ByVal vActivityId As String,
                                              ByVal vSubjectId As String,
                                              ByVal iPeriod As String,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_MedExInfoHdrDtlEdit = ObjHelp.Proc_MedExInfoHdrDtlEdit(vWorkSpaceId, vActivityId, vSubjectId, iPeriod, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "WorkSpaceProtocol Info."
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getPortocolInfo(ByVal WhereCondition_1 As String,
                       ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                       ByRef Sql_DataSet As Data.DataSet,
                       ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getPortocolInfo = ObjHelp.getPortocolInfo(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function
#End Region

#Region "GetSubjectMasterQC"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubjectMasterQC(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSubjectMasterQC = ObjHelp.GetSubjectMasterQC(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_SubjectMasterQC"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SubjectMasterQC(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SubjectMasterQC = ObjHelp.View_SubjectMasterQC(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetMedexScreeningHdrQC"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMedexScreeningHdrQC(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetMedexScreeningHdrQC = ObjHelp.GetMedexScreeningHdrQC(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_MedexScreeningHdrQc"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_MedexScreeningHdrQc(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_MedexScreeningHdrQc = ObjHelp.View_MedexScreeningHdrQc(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_LastMedExScreeningHdrDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_LastMedExScreeningHdrDtl(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_LastMedExScreeningHdrDtl = ObjHelp.View_LastMedExScreeningHdrDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetTemplateDefaultWorkFlowUserDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetTemplateDefaultWorkFlowUserDtl(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetTemplateDefaultWorkFlowUserDtl = ObjHelp.GetTemplateDefaultWorkFlowUserDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_TemplateDefaultWorkflowUserDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_TemplateDefaultWorkflowUserDtl(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_TemplateDefaultWorkflowUserDtl = ObjHelp.View_TemplateDefaultWorkflowUserDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_MyProjects"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_MyProjects(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_MyProjects = ObjHelp.View_MyProjects(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_MedexGroupMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_MedexGroupMst(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_MedexGroupMst = ObjHelp.View_MedexGroupMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_ActivityOperationMatrix"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_ActivityOperationMatrix(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_ActivityOperationMatrix = ObjHelp.View_ActivityOperationMatrix(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetWorkspaceDefaultWorkFlowUserDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetWorkspaceDefaultWorkFlowUserDtl(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetWorkspaceDefaultWorkFlowUserDtl = ObjHelp.GetWorkspaceDefaultWorkFlowUserDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_WorkspaceDefaultWorkflowUserDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_WorkspaceDefaultWorkflowUserDtl(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_WorkspaceDefaultWorkflowUserDtl = ObjHelp.View_WorkspaceDefaultWorkflowUserDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_MedexInfoHdrQc"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_MedexInfoHdrQc(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_MedexInfoHdrQc = ObjHelp.View_MedexInfoHdrQc(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_MedExInfoHdrDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_MedExInfoHdrDtl(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_MedExInfoHdrDtl = ObjHelp.View_MedExInfoHdrDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetProjectgroupMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetProjectgroupMst(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetProjectgroupMst = ObjHelp.GetProjectgroupMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_ProjectgroupMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_ProjectgroupMst(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_ProjectgroupMst = ObjHelp.View_ProjectgroupMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetProjectgroupWorkspaceMatrix"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetProjectgroupWorkspaceMatrix(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetProjectgroupWorkspaceMatrix = ObjHelp.GetProjectgroupWorkspaceMatrix(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_ProjectgroupWorkspaceMatrix"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_ProjectgroupWorkspaceMatrix(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_ProjectgroupWorkspaceMatrix = ObjHelp.View_ProjectgroupWorkspaceMatrix(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_SubjectEnrollment"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SubjectEnrollment(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SubjectEnrollment = ObjHelp.View_SubjectEnrollment(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "AgeGroupMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetAgeGroupMst(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetAgeGroupMst = ObjHelp.GetAgeGroupMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "SampleTypeMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSampleTypeMst(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSampleTypeMst = ObjHelp.getSampleTypeMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "SampleTypeDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getSampleTypeDetail(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getSampleTypeDetail = ObjHelp.getSampleTypeDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_SampleTypeDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SampleTypeDetail(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SampleTypeDetail = ObjHelp.View_SampleTypeDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_PendingSampleTypeDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_PendingSampleTypeDetail(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_PendingSampleTypeDetail = ObjHelp.View_PendingSampleTypeDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "CurrencyMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetCurrencyMst(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetCurrencyMst = ObjHelp.getCurrencyMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "MedExCostMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMedExCostMst(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetMedExCostMst = ObjHelp.getMedExCostMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "LabTypeMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetLabTypeMst(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetLabTypeMst = ObjHelp.getLabTypeMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "MedExRangeMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMedExRangeMst(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetMedExRangeMst = ObjHelp.getMedExRangeMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "LabMachineMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetLabMachineMst(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetLabMachineMst = ObjHelp.getLabMachineMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "LabMachineMedexMatrix"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetLabMachineMedexMatrix(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetLabMachineMedexMatrix = ObjHelp.getLabMachineMedexMatrix(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_RptMedexInfo"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_RptMedexInfo(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_RptMedexInfo = ObjHelp.View_RptMedexInfo(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_SubjectSampleCostDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SubjectSampleCostDetail(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SubjectSampleCostDetail = ObjHelp.View_SubjectSampleCostDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetSubjectRejectionDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubjectRejectionDetail(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSubjectRejectionDetail = ObjHelp.GetSubjectRejectionDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetSubjectScreeningRecordDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubjectScreeningRecordDetail(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSubjectScreeningRecordDetail = ObjHelp.GetSubjectScreeningRecordDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_SubjectScreeningRecordDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SubjectScreeningRecordDetail(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SubjectScreeningRecordDetail = ObjHelp.View_SubjectScreeningRecordDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_SubjectRejectionDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SubjectRejectionDetail(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SubjectRejectionDetail = ObjHelp.View_SubjectRejectionDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_MedExInfoHdrDtlEdit_Generic"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_MedExInfoHdrDtlEdit_Generic(ByVal vWorkSpaceId As String,
                                              ByVal vActivityId As String,
                                              ByVal iPeriod As String,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_MedExInfoHdrDtlEdit_Generic = ObjHelp.Proc_MedExInfoHdrDtlEdit_Generic(vWorkSpaceId, vActivityId, iPeriod, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetSpecialityMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSpecialityMst(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSpecialityMst = ObjHelp.GetSpecialityMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_ReasonMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_ReasonMst(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_ReasonMst = ObjHelp.View_ReasonMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_SubjectPIFAuditTrail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SubjectPIFAuditTrail(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SubjectPIFAuditTrail = ObjHelp.View_SubjectPIFAuditTrail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_MedExScreeningHdrDtlAuditTrail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_MedExScreeningHdrDtlAuditTrail(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_MedExScreeningHdrDtlAuditTrail = ObjHelp.View_MedExScreeningHdrDtlAuditTrail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_MedExInfoHdrDtlAuditTrail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_MedExInfoHdrDtlAuditTrail(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_MedExInfoHdrDtlAuditTrail = ObjHelp.View_MedExInfoHdrDtlAuditTrail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetMedExScreeningHdrHistory"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMedExScreeningHdrHistory(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetMedExScreeningHdrHistory = ObjHelp.GetMedExScreeningHdrHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "EncryptPassword"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function EncryptPassword(ByVal revpassword As String) As String
        Dim ObjCommon As clsCommon = Nothing

        ObjCommon = New clsCommon
        EncryptPassword = ObjCommon.EncryptPassword(revpassword)

    End Function

#End Region

#Region "DecryptPassword"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function DecryptPassword(ByVal revpassword As String) As String
        Dim ObjCommon As clsCommon = Nothing

        ObjCommon = New clsCommon
        DecryptPassword = ObjCommon.DecryptPassword(revpassword)

    End Function

#End Region

#Region "GetDosingDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetDosingDetail(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetDosingDetail = ObjHelp.GetDosingDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetRandomizationDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetRandomizationDetail(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetRandomizationDetail = ObjHelp.GetRandomizationDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetView_RandomizationDetail(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetView_RandomizationDetail = ObjHelp.GetView_RandomizationDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_DosingDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_DosingDetail(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_DosingDetail = ObjHelp.View_DosingDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetWorkspaceProtocolDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetWorkspaceProtocolDetail(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetWorkspaceProtocolDetail = ObjHelp.getWorkspaceProtocolDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

    '*********************
    'Added By Vishal Astik
    '*********************

#Region " getActivityMst "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getActivityMst(ByVal WhereCondition_1 As String,
                                       ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                       ByRef Sql_DataSet As Data.DataSet,
                                       ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getActivityMst = ObjHelp.getActivityMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
    '========================
#End Region

#Region "GetSubjectComments "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubjectComments(ByVal WorkSpaceId As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSubjectComments = ObjHelp.GetSubjectComments(WorkSpaceId, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetSlotCalendar from View"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSlotCalendar(ByVal LocationCode As String,
                                       ByVal ResourceCode As String,
                                       ByVal FirstDate As String,
                                       ByVal LastDate As String,
                                       ByVal WorkSpaceId As String,
                                       ByRef Sql_DataSet As Data.DataSet,
                                       ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSlotCalendar = ObjHelp.GetSlotCalendar(LocationCode, ResourceCode, FirstDate, LastDate, WorkSpaceId, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region " getTemplateTypeMst "

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getTemplateTypeMst(ByVal WhereCondition_1 As String,
                                       ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                       ByRef Sql_DataSet As Data.DataSet,
                                       ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getTemplateTypeMst = ObjHelp.GetTemplateTypeMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "GetProcedure_ReturnValue"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetProcedure_ReturnValue(ByVal procedureName As String,
                                    ByVal workspaceId As String,
                                       ByVal ID As String,
                                       ByRef Sql_DataSet As Data.DataSet,
                                       ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable

        GetProcedure_ReturnValue = ObjHelp.GetProcedure_ReturnValue(procedureName, workspaceId, ID, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "GetProc_AutoScheduleDetails"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetProc_AutoScheduleDetails(ByVal workspaceId As String,
                                       ByVal ID As String,
                                       ByVal SchedulingAct As Boolean,
                                       ByRef Sql_DataSet As Data.DataSet,
                                       ByRef eStr_Retu As String) As Boolean
        Try

            Dim ObjHelp As clsHelpDbTable = Nothing

            ObjHelp = New clsHelpDbTable

            Sql_DataSet = ObjHelp.ProcedureExecute("Proc_AutoScheduleDetails", workspaceId + "##" + ID + "##" + IIf(SchedulingAct, "Y", "N").ToString.Trim())

            'GetProcedure_ReturnValue("Proc_AutoScheduleDetails", workspaceId, ID, Sql_DataSet, eStr_Retu)
            GetProc_AutoScheduleDetails = True

        Catch ex As Exception
            GetProc_AutoScheduleDetails = False
        End Try
    End Function

#End Region

#Region "GetProc_TreeViewOfNodes"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetProc_TreeViewOfNodes(ByVal strParamValue As String) As DataSet
        Dim ObjDataLogic_New As ClsDataLogic_New = Nothing

        ObjDataLogic_New = New ClsDataLogic_New
        Return ObjDataLogic_New.ProcedureExecute("Proc_TreeViewOfNodes", strParamValue)
    End Function
#End Region

#Region " GetPanelDisplayDetailByUserType "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetPanelDisplayDetailByUserType(ByVal WhereCondition_1 As String,
                                       ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                       ByRef Sql_DataSet As Data.DataSet,
                                       ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetPanelDisplayDetailByUserType = ObjHelp.GetPanelDisplayDetailByUserType(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region " GetDeptMst "

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetDeptMst(ByVal WhereCondition_1 As String,
                                      ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                      ByRef Sql_DataSet As Data.DataSet,
                                      ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetDeptMst = ObjHelp.GetDeptMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "GetCRFHdr"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetCRFHdr(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetCRFHdr = ObjHelp.GetCRFHdr(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetCRFOperationRemarkDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetCRFOperationRemarkDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetCRFOperationRemarkDtl = ObjHelp.GetCRFOperationRemarkDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "CRFProtocolCriterienDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetCRFProtocolCriterienDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetCRFProtocolCriterienDtl = ObjHelp.GetCRFProtocolCriterienDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetCRFSubSelectionDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetCRFSubSelectionDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetCRFSubSelectionDtl = ObjHelp.GetCRFSubSelectionDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetMedExSubGroupMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMedExSubGroupMst(ByVal WhereCondition_1 As String,
                       ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                       ByRef Sql_DataSet As Data.DataSet,
                       ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetMedExSubGroupMst = ObjHelp.GetMedExSubGroupMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "ProcedureExecute"
    ''' <summary>
    ''' Function Executes Stored Procedure and Returns DataSet
    ''' </summary>
    ''' <param name="strProcedureName">Name of Stored Procedure</param>
    ''' <param name="strParamValue">Parameter Values Concatenated By '##' sign</param>
    ''' <returns>Returns System.Data.DataSet</returns>
    ''' <remarks>Parameter Values must be Concatenated By '##'</remarks>
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function ProcedureExecute(ByVal strProcedureName As String, ByVal strParamValue As String) As DataSet
        Dim ObjDataLogic_New As ClsDataLogic_New = Nothing

        ObjDataLogic_New = New ClsDataLogic_New
        Return ObjDataLogic_New.ProcedureExecute(strProcedureName, strParamValue)
    End Function
#End Region

#Region "AJAX Page Method for AutoComplete Extender Control"

#Region "GetProjectCompletionList"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetProjectCompletionList(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetProjectCompletionList(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#Region "GetParentProjectCompletionList"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetParentProjectCompletionList(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetParentProjectCompletionList(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#Region "GetProjectCompletionListWithOutSponser"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetProjectCompletionListWithOutSponser(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetProjectCompletionListWithOutSponser(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#Region "GetSubjectCompletionList_NotRejected"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubjectCompletionList_NotRejected(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetSubjectCompletionList_NotRejected(prefixText, count, contextKey)
        Return items
    End Function
#End Region

    'Created By Chandresh Vanker on 10-Feb-2010 For getting only assigned subjects
#Region "GetSubjectCompletionList_Assigned_NotRejected"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubjectCompletionList_Assigned_NotRejected(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetSubjectCompletionList_Assigned_NotRejected(prefixText, count, contextKey)
        Return items
    End Function
#End Region
    '******************************************

#Region "GetAllSubjectCompletionList"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetAllSubjectCompletionList(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetAllSubjectCompletionList(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#Region "GetSubjectCompletionList_Dynamic"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubjectCompletionList_Dynamic(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetSubjectCompletionList_Dynamic(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#Region "GetActivityCompletionList"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetActivityCompletionList(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetActivityCompletionList(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#Region "GetDrugCompletionList" 'Added By Naimesh Dave
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetDrugCompletionList(ByVal prefixText As String,
                    ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable
        items = ObjHelp.GetDrugCompletionList(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#Region "GetMedexList" 'Added By Naimesh Dave
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMedexList(ByVal prefixText As String,
                    ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable
        items = ObjHelp.GetMedexList(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#Region "GetMyProjectCompletionList"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMyProjectCompletionList(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetMyProjectCompletionList(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#Region "GetOldProjectsList"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetOldProjectsList(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetOldProjectsList(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#Region "GetMyProjectCompletionListForProjectTrack"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMyProjectCompletionListForProjectTrack(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetMyProjectCompletionListForProjectTrack(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#Region "GetMyProjectCompletionListForDashboard"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMyProjectCompletionListForDashboard(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetMyProjectCompletionListForDashboard(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#Region "GetClientRequestProjectCompletionListForDashboard"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetClientRequestProjectCompletionListForDashboard(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetClientRequestProjectCompletionListForDashboard(prefixText, count, contextKey)
        Return items
    End Function
#End Region

    ' Added by pratiksha
#Region "GetSubjectCompletionList_NotRejected_InHouse"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubjectCompletionList_NotRejected_InHouse(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetSubjectCompletionList_NotRejected_InHouse(prefixText, count, contextKey)
        Return items


    End Function
#End Region

    ' Date : 15-Mar-2011
    ' Reason : For attribute group
#Region "GetAttributeGroup"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetAttributeGroup(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetAttributeGroup(prefixText, count, contextKey)
        Return items
    End Function
#End Region

    ' Date : 16-Mar-2011
    ' Reason : For attribute sub group
#Region "GetAttributesSubGroup"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetAttributeSubGroup(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetAttributeSubGroup(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#Region "GetAllProjectList"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetAllProjectList(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetAllProjectList(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#Region "GetProjectCompletionListForDMS"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetProjectCompletionListForDMS(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetProjectCompletionListForDMS(prefixText, count, contextKey)
        Return items
    End Function
#End Region

    '' added by prayag for subject rejection module changes
#Region "GetSubjectCompletionList_NotRejected_BlockPeriod"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubjectCompletionList_NotRejected_BlockPeriod(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetSubjectCompletionList_NotRejected_BlockPeriod(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#Region "GetSubjectCompletionList_NotRejected_InHouse_BlockPeriod"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubjectCompletionList_NotRejected_InHouse_BlockPeriod(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetSubjectCompletionList_NotRejected_InHouse_BlockPeriod(prefixText, count, contextKey)
        Return items


    End Function
#End Region

#Region "GetSubjectCompletionList_NotRejected_OnlyID_BlockPeriod"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubjectCompletionList_NotRejected_OnlyID_BlockPeriod(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetSubjectCompletionList_NotRejected_OnlyID_BlockPeriod(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#End Region
    '========================
    'Added By Mihir Oza
    '========================

#Region "GetSubjectMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetView_SubjectMaster(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetView_SubjectMaster = ObjHelp.GetView_SubjectMaster(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region


#Region "GetSubjectfemaleDetails"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubjectFemaleDetails(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSubjectFemaleDetails = ObjHelp.GetSubjectFemaleDetails(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetCRFDrugScanReport"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetCRFDrugScanReport(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetCRFDrugScanReport = ObjHelp.GetCRFDrugScanReport(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetDrugAnalytesMatrix"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetDrugAnalytesMatrix(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetDrugAnalytesMatrix = ObjHelp.GetDrugAnalytesMatrix(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetMedExMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMedExMst(ByVal WhereCondition_1 As String,
                       ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                       ByRef Sql_DataSet As Data.DataSet,
                       ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetMedExMst = ObjHelp.GetMedExMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function
#End Region

#Region "GetMedExGroupMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMedExGroupMst(ByVal WhereCondition_1 As String,
                       ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                       ByRef Sql_DataSet As Data.DataSet,
                       ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetMedExGroupMst = ObjHelp.GetMedExGroupMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetMedExTemplateDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMedExTemplateDtl(ByVal WhereCondition_1 As String,
                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                           ByRef Sql_DataSet As Data.DataSet,
                           ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetMedExTemplateDtl = ObjHelp.GetMedExTemplateDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetMedExWorkSpaceDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMedExWorkSpaceDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetMedExWorkSpaceDtl = ObjHelp.GetMedExWorkSpaceDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetTemplateCompletionList"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetTemplateCompletionList(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetTemplateCompletionList(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#Region "GetMedExWorkSpaceHdr"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMedExWorkSpaceHdr(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetMedExWorkSpaceHdr = ObjHelp.GetMedExWorkSpaceHdr(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetMedExWorkspaceDtlDelete"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMedExWorkspaceDtlDelete(ByVal MedExWorkspaceHdr As String,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetMedExWorkspaceDtlDelete = ObjHelp.GetMedExWorkspaceDtlDelete(MedExWorkspaceHdr, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetDocTypeTemplateMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetDocTypeTemplateMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetDocTypeTemplateMst = ObjHelp.GetDocTypeTemplateMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetCountryMaster"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetCountryMaster(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetCountryMaster = ObjHelp.GetCountryMaster(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region


#Region "GetSpecialityMaster"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSpecialityMaster(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSpecialityMaster = ObjHelp.GetSpecialityMaster(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region




#Region " GetConnectionString "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetConnectionString(ByVal Password As String,
                                               ByRef Retu_ConnectionString As String,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetConnectionString = ObjHelp.GetConnectionString(Password, Retu_ConnectionString, eStr_Retu)
    End Function
#End Region


#Region "GetViewCrfhdrDtlSubDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewCrfHdrDtlSubDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewCrfHdrDtlSubDtl = ObjHelp.GetViewCrfHdrDtlSubDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function
#End Region


    '========================
    'Added By Satyam
    '========================

#Region "GetViewWorkspaceSubjectMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewWorkspaceSubjectMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewWorkspaceSubjectMst = ObjHelp.GetViewWorkspaceSubjectMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetSubjectMaster"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubjectMaster(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSubjectMaster = ObjHelp.GetSubjectMaster(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetViewWorkSpaceNodeHistory"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewWorkSpaceNodeHistory(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewWorkSpaceNodeHistory = ObjHelp.GetViewWorkSpaceNodeHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "getSubjectBlobDetails"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getSubjectBlobDetails(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getSubjectBlobDetails = ObjHelp.getSubjectBlobDetails(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetPIFSubjectMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetView_PIFSubjectMaster(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetView_PIFSubjectMaster = ObjHelp.GetView_PIFSubjectMaster(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetDocTypeMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetDocTypeMaster(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetDocTypeMaster = ObjHelp.GetDocTypeMaster(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "DocTypeTemplateDataMatrix"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function DocTypeTemplateDataMatrix(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        DocTypeTemplateDataMatrix = ObjHelp.DocTypeTemplateDataMatrix(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetDocTemplateWorkspaceDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetView_DocTemplateWorkspaceDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetView_DocTemplateWorkspaceDtl = ObjHelp.GetView_DocTemplateWorkspaceDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "PIFSubjectScreeningDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetView_PIFSubjectScreeningDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetView_PIFSubjectScreeningDtl = ObjHelp.GetView_PIFSubjectScreeningDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetViewSubjectBlobDetails"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetView_SubjectBlobDetails(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetView_SubjectBlobDetails = ObjHelp.GetView_SubjectBlobDetails(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "UpLoadFile"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function UpLoadFile(ByVal FileByte As Byte(),
                               ByVal FileName As String,
                               ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        UpLoadFile = ObjHelp.UploadFile(FileByte, FileName, eStr_Retu)
    End Function
#End Region

#Region "GetView_UnAdjustCollectionDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetView_UnAdjustCollectionDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetView_UnAdjustCollectionDtl = ObjHelp.GetView_UnAdjustCollectionDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetView_UnAdjustInvoiceDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetView_UnAdjustInvoiceDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetView_UnAdjustInvoiceDtl = ObjHelp.GetView_UnAdjustInvoiceDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

    '==================================================
    'Added By Chandresh Vanker on 31-01-2009
    '==================================================

#Region "GetSampleDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSampleDetail(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSampleDetail = ObjHelp.GetSampleDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetSampleMedExDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSampleMedExDetail(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSampleMedExDetail = ObjHelp.GetSampleMedExDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Get_ViewSampleDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_ViewSampleDetail(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_ViewSampleDetail = ObjHelp.Get_ViewSampleDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '===============Added on 04-02-2009

#Region "GetSampleTypeSendReceiveDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSampleTypeSendReceiveDetail(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSampleTypeSendReceiveDetail = ObjHelp.GetSampleTypeSendReceiveDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '========Added on 05-02-2009

#Region "View_SampleTypeSendReceiveDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SampleTypeSendReceiveDetail(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SampleTypeSendReceiveDetail = ObjHelp.View_SampleTypeSendReceiveDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '========Added on 09-02-2009

#Region "GetScopeMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetScopeMst(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetScopeMst = ObjHelp.GetScopeMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '========================Added on 25-02-2009

#Region "Get_ViewSampleMedExDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_ViewSampleMedExDetail(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_ViewSampleMedExDetail = ObjHelp.Get_ViewSampleMedExDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '==========Added on 13-04-2009

#Region " GetPasswordPolicyMst "

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetPasswordPolicyMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetPasswordPolicyMst = ObjHelp.GetPasswordPolicyMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region '=============Added on 06-05-2009===================For PassWord Policy

#Region " GetPasswordHistory "

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetPasswordHistory(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetPasswordHistory = ObjHelp.GetPasswordHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region " GetUserLoginDetails "

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetUserLoginDetails(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetUserLoginDetails = ObjHelp.GetUserLoginDetails(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region " GetActivityMedExTemplateDtl "

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetActivityMedExTemplateDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetActivityMedExTemplateDtl = ObjHelp.GetActivityMedExTemplateDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region '=============Added on 30-05-2009=============For Activity MedEx Template

#Region "VIEW_ActivityMedExTemplateDtl "

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function VIEW_ActivityMedExTemplateDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        VIEW_ActivityMedExTemplateDtl = ObjHelp.VIEW_ActivityMedExTemplateDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region '=============Added on 02-06-2009=============For VIEW_ActivityMedExTemplateDtl

#Region "GetUOMMst "

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetUOMMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetUOMMst = ObjHelp.GetUOMMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region '=============Added on 03-06-2009=============

#Region "GetEmailAlertMst "

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetEmailAlertMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetEmailAlertMst = ObjHelp.GetEmailAlertMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region '=============Added on 12-06-2009=============

#Region "GetSubjectProofDetails "

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubjectProofDetails(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSubjectProofDetails = ObjHelp.GetSubjectProofDetails(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region '=============Added on 02-07-2009=============

#Region "View_ProjectgroupWorkspaceSubject "

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_ProjectgroupWorkspaceSubject(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_ProjectgroupWorkspaceSubject = ObjHelp.View_ProjectgroupWorkspaceSubject(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region '=============Added on 13-07-2009=============

#Region "Proc_SchedulingGunningActivities"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_SchedulingGunningActivities(ByVal WorkspaceId As String,
                                        ByVal LocationCode As String,
                                        ByVal DeptCode As String,
                                        ByVal UserTypeCode As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_SchedulingGunningActivities = ObjHelp.Proc_SchedulingGunningActivities(WorkspaceId, LocationCode, DeptCode, UserTypeCode, Sql_DataSet, eStr_Retu)
    End Function ''added by dipen shah on 19-feb-2015
#End Region

#Region " Proc_SubjectEnrollment "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_SubjectEnrollment(ByVal WorkspaceId As String,
                                        ByVal LocationCode As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_SubjectEnrollment = ObjHelp.Proc_SubjectEnrollment(WorkspaceId, LocationCode, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 21-07-2009=============

#Region "View_WorkSpaceVisitDtl "

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_WorkSpaceVisitDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_WorkSpaceVisitDtl = ObjHelp.View_WorkSpaceVisitDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region '=============Added on 24-07-2009=============

#Region "GetCollectionDetail"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetCollectionDetail(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetCollectionDetail = ObjHelp.GetCollectionDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region '=============Added on 31-07-2009=============As per telephonic conversation with Yasheshbhai.

#Region "View_WorkspaceSubjectScreeningDtl "

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_WorkspaceSubjectScreeningDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_WorkspaceSubjectScreeningDtl = ObjHelp.View_WorkspaceSubjectScreeningDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region '=============Added on 10-08-2009=============

#Region "GetSubjectWorkspaceAssignment"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubjectWorkspaceAssignment(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSubjectWorkspaceAssignment = ObjHelp.GetSubjectWorkspaceAssignment(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region '=============Added on 11-08-2009=============

#Region "View_SubjectWorkspaceAssignment"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SubjectWorkspaceAssignment(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SubjectWorkspaceAssignment = ObjHelp.View_SubjectWorkspaceAssignment(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region '=============Added on 11-08-2009=============

#Region "View_MaxSubjectBlobdetails_Search"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_MaxSubjectBlobdetails_Search(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_MaxSubjectBlobdetails_Search = ObjHelp.View_MaxSubjectBlobdetails_Search(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region '=============Added on 20-08-2009=============

#Region "GetSubjectLabReportDetail"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubjectLabReportDetail(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSubjectLabReportDetail = ObjHelp.GetSubjectLabReportDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region '=============Added on 27-08-2009=============

#Region "View_SubjectLabReportDetail"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SubjectLabReportDetail(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SubjectLabReportDetail = ObjHelp.View_SubjectLabReportDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region '=============Added on 27-08-2009=============

#Region "GetUserLoginFailureDetails"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetUserLoginFailureDetails(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetUserLoginFailureDetails = ObjHelp.GetUserLoginFailureDetails(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region '=============Added on 16-09-2009=============

#Region "GetCRFHdrForCTM"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetCRFHdrForCTM(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetCRFHdrForCTM = ObjHelp.GetCRFHdrForCTM(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region '=============Added on 19-12-2009=============

#Region "GetCRFDtl"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetCRFDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetCRFDtl = ObjHelp.GetCRFDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region '=============Added on 19-12-2009=============

#Region "GetCRFSubDtl"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetCRFSubDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetCRFSubDtl = ObjHelp.GetCRFSubDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region '=============Added on 19-12-2009=============

#Region "GetCRFWorkFlowDtl"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetCRFWorkFlowDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetCRFWorkFlowDtl = ObjHelp.GetCRFWorkFlowDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region '=============Added on 19-12-2009=============

#Region "View_CRFHdrDtlSubDtl"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_CRFHdrDtlSubDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_CRFHdrDtlSubDtl = ObjHelp.View_CRFHdrDtlSubDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region '=============Added on 19-12-2009=============

#Region "View_CRFHdrDtlSubDtl_Edit"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_CRFHdrDtlSubDtl_Edit(ByVal WhereCondition As String,
                                                     ByVal Columns As String,
                                                     ByRef Sql_DataSet As Data.DataSet,
                                                     ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_CRFHdrDtlSubDtl_Edit = ObjHelp.View_CRFHdrDtlSubDtl_Edit(WhereCondition, Columns, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 21-12-2009=============

#Region "GetDCFMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetDCFMst(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetDCFMst = ObjHelp.GetDCFMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 25-12-2009=============

#Region "View_DCFMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_DCFMst(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_DCFMst = ObjHelp.View_DCFMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 25-12-2009=============

#Region "GetReferenceTableDefinitions"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetReferenceTableDefinitions(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetReferenceTableDefinitions = ObjHelp.GetReferenceTableDefinitions(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetReferenceTableDefinitionsHistory(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetReferenceTableDefinitionsHistory = ObjHelp.GetReferenceTableDefinitionsHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region '=============Added on 29-12-2009=============

#Region "VIEW_DiscrepancyStatusReport"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function VIEW_DiscrepancyStatusReport(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        VIEW_DiscrepancyStatusReport = ObjHelp.VIEW_DiscrepancyStatusReport(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 31-12-2009=============

#Region "GetProjectNo"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetProjectNo(ByRef ProjNo As String,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetProjectNo = ObjHelp.GetProjectNo(ProjNo, eStr_Retu)
    End Function
#End Region '=============Added on 05-01-2010=============

#Region "View_WorkspaceActivitySubjectMatrix"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_WorkspaceActivitySubjectMatrix(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_WorkspaceActivitySubjectMatrix = ObjHelp.View_WorkspaceActivitySubjectMatrix(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 27-01-2010=============

#Region " Proc_WorkspaceActivitySubjectMatrix "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_WorkspaceActivitySubjectMatrix(ByVal Parameters As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_WorkspaceActivitySubjectMatrix = ObjHelp.Proc_WorkspaceActivitySubjectMatrix(Parameters, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 27-01-2010=============

#Region "GetMedExCrossChecks"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMedExCrossChecks(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetMedExCrossChecks = ObjHelp.GetMedExCrossChecks(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 17-02-2010=============

#Region "View_WorkSpaceSubjectRegistration"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_WorkSpaceSubjectRegistration(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_WorkSpaceSubjectRegistration = ObjHelp.View_WorkSpaceSubjectRegistration(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 26-02-2010=============

#Region " GetColumnNamesWithWhereCondition "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetColumnNamesWithWhereCondition(ByVal TableName As String,
                                     ByVal wStr As String,
                                     ByRef Sql_DS As Data.DataSet,
                                     ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetColumnNamesWithWhereCondition = ObjHelp.GetColumnNamesWithWhereCondition(TableName, wStr, Sql_DS, eStr_Retu)
    End Function
#End Region '=============Added on 11-03-2010=============

#Region "View_CRFDetailedReport"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_CRFDetailedReport(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_CRFDetailedReport = ObjHelp.View_CRFDetailedReport(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 11-03-2010=============

#Region "View_CRFDetailedReport"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_WorkspaceActivitySequenceDeviation(ByVal WhereCondition As String,
                                              ByVal DataRetrieval As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_WorkspaceActivitySequenceDeviation = ObjHelp.View_WorkspaceActivitySequenceDeviation(WhereCondition, DataRetrieval, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 24-Feb-2012=============

#Region "View_CRFActivityStatus"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_CRFActivityStatus(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_CRFActivityStatus = ObjHelp.View_CRFActivityStatus(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 11-03-2010=============

#Region "VIEW_MedExActivityVise"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function VIEW_MedExActivityVise(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        VIEW_MedExActivityVise = ObjHelp.VIEW_MedExActivityVise(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 05-04-2010=============

#Region "GetMedExEditChecks"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMedExEditChecks(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetMedExEditChecks = ObjHelp.GetMedExEditChecks(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 05-04-2010=============

#Region "Proc_GetStructure"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetStructure(ByVal Param As String,
                                            ByRef Sql_DataSet As Data.DataSet,
                                            ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetStructure = ObjHelp.Proc_GetStructure(Param, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_GetNodeInformation"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetNodeInformation(ByVal Param As String,
                                            ByRef Sql_DataSet As Data.DataSet,
                                            ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetNodeInformation = ObjHelp.Proc_GetNodeInformation(Param, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_CRFDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_CRFDetail(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_CRFDetail = ObjHelp.View_CRFDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 07-04-2010=============

#Region "GetEditChecksHdr"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetEditChecksHdr(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetEditChecksHdr = ObjHelp.GetEditChecksHdr(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 07-04-2010=============

#Region "GetEditChecksDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetEditChecksDtl(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetEditChecksDtl = ObjHelp.GetEditChecksDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 07-04-2010=============

#Region "Get_EditChecksHdrDtl"
    '<WebMethod(),SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    '           Public Function view_editcheckshdrdtl(ByVal wherecondition_1 As String, _
    '                                          ByVal dataretrieval_1 As DataRetrievalModeEnum, _
    '                                          ByRef sql_dataset As Data.DataSet, _
    '                                          ByRef estr_retu As String) As Boolean
    '    Dim objhelp As clshelpdbtable = Nothing

    '    objhelp = New clshelpdbtable
    '    view_editcheckshdrdtl = objhelp.view_editcheckshdrdtl(wherecondition_1, dataretrieval_1, sql_dataset, estr_retu)
    'End Function
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_EditChecksHdrDtl(ByVal WorkspaceId As String,
                                                     ByVal SubjectId As String,
                                                     ByVal NodeId As Integer,
                                                     ByRef Sql_DataSet As Data.DataSet,
                                                     ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_EditChecksHdrDtl = ObjHelp.Get_EditChecksHdrDtl(WorkspaceId, SubjectId, NodeId, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 23-04-2010=============

#Region "GetCRFLockDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetCRFLockDtl(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetCRFLockDtl = ObjHelp.GetCRFLockDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 25-04-2010=============

#Region "View_CRFLockDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_CRFLockDtl(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_CRFLockDtl = ObjHelp.View_CRFLockDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 25-04-2010=============

#Region "View_EditChecksHdr"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_EditChecksHdr(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_EditChecksHdr = ObjHelp.View_EditChecksHdr(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 28-04-2010=============

#Region "Get_EditChecksHdr_MaxTran"
    'Removed as view is changed to procedure-Pratiksha
    '<WebMethod(),SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    '           Public Function View_EditChecksHdr_MaxTran(ByVal WhereCondition_1 As String, _
    '                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum, _
    '                                          ByRef Sql_DataSet As Data.DataSet, _
    '                                          ByRef eStr_Retu As String) As Boolean
    '    Dim ObjHelp As clsHelpDbTable = Nothing

    '    ObjHelp = New clsHelpDbTable
    '    View_EditChecksHdr_MaxTran = ObjHelp.View_EditChecksHdr_MaxTran(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    'End Function
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_EditChecksHdr_MaxTran(ByVal WorkspaceId As String,
                                                     ByVal NodeId As Integer,
                                                     ByRef Sql_DataSet As Data.DataSet,
                                                     ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_EditChecksHdr_MaxTran = ObjHelp.Get_EditChecksHdr_MaxTran(WorkspaceId, NodeId, Sql_DataSet, eStr_Retu)
    End Function

#End Region '=============Added on 03-05-2010=============

#Region "View_EditChecksDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_EditChecksDtl(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_EditChecksDtl = ObjHelp.View_EditChecksDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 14-05-2010=============

#Region "GetOldProjects"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetOldProjects(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetOldProjects = ObjHelp.GetOldProjects(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 25-08-2010=============

#Region "view_getanalyticalprojects"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function view_getanalyticalprojects(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        view_getanalyticalprojects = ObjHelp.view_getanalyticalprojects(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_getClinicalPhaseProjects"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_getClinicalPhaseProjects(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_getClinicalPhaseProjects = ObjHelp.View_getClinicalPhaseProjects(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 14-09-2010=============

#Region " Proc_GetProjectStatusCount "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetProjectStatusCount(ByVal ProjectTypeCode As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetProjectStatusCount = ObjHelp.Proc_GetProjectStatusCount(ProjectTypeCode, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 14-09-2010=============

#Region " Proc_GetNotificationEmailId "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetNotificationEmailId(ByVal ActivityId As String,
                                        ByVal ActivityStartedOrEnded As String,
                                        ByVal TaskToDo As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetNotificationEmailId = ObjHelp.Proc_GetNotificationEmailId(ActivityId, ActivityStartedOrEnded, TaskToDo, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 15-10-2010=============

#Region " Proc_GetProjectStudyWorkSummaryDetails "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetProjectStudyWorkSummaryDetails(ByVal ActivityId As String,
                                        ByVal FromDate As String,
                                        ByVal ToDate As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetProjectStudyWorkSummaryDetails = ObjHelp.Proc_GetProjectStudyWorkSummaryDetails(ActivityId, FromDate, ToDate, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 20-10-2010=============

#Region "getExpenseTypeMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getExpenseTypeMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getExpenseTypeMst = ObjHelp.getExpenseTypeMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "getExpenseMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getExpenseMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getExpenseMst = ObjHelp.getExpenseMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_ExpenseMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_ExpenseMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_ExpenseMst = ObjHelp.View_ExpenseMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "getExpenseDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getExpenseDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getExpenseDtl = ObjHelp.getExpenseDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_ActivityStartEndDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_ActivityStartEndDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_ActivityStartEndDtl = ObjHelp.View_ActivityStartEndDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_BlankCRF"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_BlankCRF(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_BlankCRF = ObjHelp.View_BlankCRF(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 17-12-2010=============

#Region "getWorkSpaceSubjectMstHistory"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getWorkSpaceSubjectMstHistory(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getWorkSpaceSubjectMstHistory = ObjHelp.getWorkSpaceSubjectMstHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 27-12-2010=============

#Region "View_WorkSpaceSubjectMstHistory"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_WorkSpaceSubjectMstHistory(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_WorkSpaceSubjectMstHistory = ObjHelp.View_WorkSpaceSubjectMstHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 28-12-2010=============

#Region " Proc_WorkspaceActivitySubjectMatrix_Count "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_WorkspaceActivitySubjectMatrix_Count(ByVal Parameter As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_WorkspaceActivitySubjectMatrix_Count = ObjHelp.Proc_WorkspaceActivitySubjectMatrix_Count(Parameter, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 02-02-2011=============

#Region " Proc_MyProjects "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_MyProjects(ByVal Parameters As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_MyProjects = ObjHelp.Proc_MyProjects(Parameters, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 03-02-2011=============

#Region " Proc_GetMyProjectsCount "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetMyProjectsCount(ByVal Parameters As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetMyProjectsCount = ObjHelp.Proc_GetMyProjectsCount(Parameters, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 03-02-2011=============

#Region "getWorkSpaceStatusDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getWorkSpaceStatusDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getWorkSpaceStatusDtl = ObjHelp.getWorkSpaceStatusDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 04-02-2011=============

#Region " Proc_WorkspaceActivitySubjectMatrix_BABE "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_WorkspaceActivitySubjectMatrix_BABE(ByVal Parameters As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_WorkspaceActivitySubjectMatrix_BABE = ObjHelp.Proc_WorkspaceActivitySubjectMatrix_BABE(Parameters, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 02-03-2011=============

#Region "getDocumentReleaseDetails"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getDocumentReleaseDetails(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getDocumentReleaseDetails = ObjHelp.getDocumentReleaseDetails(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 24-03-2011 on request of Vishal=============

#Region " Proc_GetTabularDataForActivity "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetTabularDataForActivity(ByVal Parameters As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetTabularDataForActivity = ObjHelp.Proc_GetTabularDataForActivity(Parameters, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 14-04-2011=============

#Region "view_ActivityTree BABE"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_ActivityTreeBABE(ByVal vWorkspaceId_1 As String,
                                              ByVal iPeriod As String,
                                            ByVal cSubjectWiseFlag As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_ActivityTreeBABE = ObjHelp.Proc_ActivityTreeBABE(vWorkspaceId_1, iPeriod, cSubjectWiseFlag, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 18-08-2011 on request of Suhani=============

#Region "view_ActivityTree CTM"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_ActivityTreeCTM(ByVal vWorkspaceId_1 As String,
                                              ByVal iPeriod As String,
                                            ByVal cSubjectWiseFlag As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                            ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_ActivityTreeCTM = ObjHelp.Proc_ActivityTreeCTM(vWorkspaceId_1, iPeriod, cSubjectWiseFlag, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 18-08-2011 on request of Suhani=============

#Region "view_ActivityTreeCRFTerm"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetActivityTreeCRFTerm(ByVal vWorkspaceId_1 As String,
                                              ByVal iPeriod As String,
                                            ByVal cSubjectWiseFlag As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetActivityTreeCRFTerm = ObjHelp.Proc_GetActivityTreeCRFTerm(vWorkspaceId_1, iPeriod, cSubjectWiseFlag, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 24-11-2017 for Medical Coding Hiren Rami=============

#Region "view_ActivityTreeCRFTerm_BABE"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetActivityTreeCRFTerm_BABE(ByVal vWorkspaceId_1 As String,
                                              ByVal iPeriod As String,
                                            ByVal cSubjectWiseFlag As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetActivityTreeCRFTerm_BABE = ObjHelp.Proc_GetActivityTreeCRFTerm_BABE(vWorkspaceId_1, iPeriod, cSubjectWiseFlag, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 24-11-2017 for Medical Coding BABE Hiren Rami=============

#Region "proc_GetCRFActivityStatus CTM"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function proc_GetCRFActivityStatusCTM(ByVal WorkSpaceId_1 As String,
                                                  ByVal iPeriod As String,
                                                             ByVal vSubjectId As String,
                                                             ByVal iParentActivityNodeId As String,
                                                             ByVal iActivityNodeId As String,
                                                             ByVal cSubjectWiseFlag As String,
                                                             ByVal cDataStatus As String,
                                                         ByVal iWorkFlowStageId As String,
                                                             ByRef Sql_DataSet As Data.DataSet,
                                                             ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        proc_GetCRFActivityStatusCTM = ObjHelp.proc_GetCRFActivityStatusCTM(WorkSpaceId_1, iPeriod, vSubjectId, iParentActivityNodeId, iActivityNodeId, cSubjectWiseFlag, cDataStatus, iWorkFlowStageId, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 18-08-2011 on request of Suhani=============

#Region "proc_GetCRFActivityStatus BABE"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function proc_GetCRFActivityStatusBABE(ByVal WorkSpaceId_1 As String,
                                                  ByVal iPeriod As String,
                                                             ByVal vSubjectId As String,
                                                             ByVal iParentActivityNodeId As String,
                                                             ByVal iActivityNodeId As String,
                                                             ByVal cSubjectWiseFlag As String,
                                                             ByVal cDataStatus As String,
                                                         ByVal iWorkFlowStageId As String,
                                                             ByRef Sql_DataSet As Data.DataSet,
                                                             ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        proc_GetCRFActivityStatusBABE = ObjHelp.proc_GetCRFActivityStatusBABE(WorkSpaceId_1, iPeriod, vSubjectId, iParentActivityNodeId, iActivityNodeId, cSubjectWiseFlag, cDataStatus, iWorkFlowStageId, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 18-08-2011 on request of Suhani=============

#Region "proc_GetCRFActivityStatusCount"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function proc_GetCRFActivityStatusCount(ByVal WorkSpaceId_1 As String,
                                                        ByVal iPeriod As String,
                                                             ByVal vSubjectId As String,
                                                             ByVal iParentActivityNodeId As String,
                                                             ByVal iActivityNodeId As String,
                                                             ByVal cSubjectWiseFlag As String,
                                                             ByVal ProjectType As Integer,
                                                             ByVal cDataStatus As String,
                                                         ByVal iWorkFlowStageId As String,
                                                             ByRef Sql_DataSet As Data.DataSet,
                                                             ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        proc_GetCRFActivityStatusCount = ObjHelp.proc_GetCRFActivityStatusCount(WorkSpaceId_1, iPeriod, vSubjectId, iParentActivityNodeId, iActivityNodeId, cSubjectWiseFlag, ProjectType, cDataStatus, iWorkFlowStageId, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 18-08-2011 on request of Suhani=============

#Region "Proc_getcrfactivitystatusformedicalcoding"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_getcrfactivitystatusformedicalcoding(ByVal WorkSpaceId_1 As String,
                                                             ByVal iPeriod As String,
                                                             ByVal vSubjectId As String,
                                                             ByVal iParentActivityNodeId As String,
                                                             ByVal iActivityNodeId As String,
                                                             ByVal cSubjectWiseFlag As String,
                                                             ByVal ProjectType As Integer,
                                                             ByVal cDataStatus As String,
                                                             ByVal iWorkFlowStageId As String,
                                                             ByVal cDictionaryType As String,
                                                             ByVal CRFTerm As String,
                                                             ByVal cCodingStatus As String,
                                                             ByVal cActStatus As String,
                                                             ByRef Sql_DataSet As Data.DataSet,
                                                             ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_getcrfactivitystatusformedicalcoding = ObjHelp.Proc_getcrfactivitystatusformedicalcoding(WorkSpaceId_1, iPeriod, vSubjectId, iParentActivityNodeId, iActivityNodeId, cSubjectWiseFlag, ProjectType, cDataStatus, iWorkFlowStageId, cDictionaryType, CRFTerm, cCodingStatus, cActStatus, Sql_DataSet, eStr_Retu)
    End Function
#End Region '==============Added on 15-05-2017 for Medical Coding by Vikram==============

#Region "View_CRFHdrDtlSubDtl_Edit_All"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_CRFHdrDtlSubDtl_Edit_All(ByVal WhereCondition As String,
                                                     ByVal Columns As String,
                                                     ByRef Sql_DataSet As Data.DataSet,
                                                     ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_CRFHdrDtlSubDtl_Edit_All = ObjHelp.View_CRFHdrDtlSubDtl_Edit_All(WhereCondition, Columns, Sql_DataSet, eStr_Retu)
    End Function
#End Region '====added on 15-09-2011 on request of suhani

#Region "View_CRFWorkFlowDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_CRFWorkFlowDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_CRFWorkFlowDtl = ObjHelp.View_CRFWorkFlowDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 31-08-2011=============

#Region "View_Userlogindetails"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_Userlogindetails(ByVal WhereCondition As String,
                                               ByVal DataRetrieval As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_Userlogindetails = ObjHelp.View_Userlogindetails(WhereCondition, DataRetrieval, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 17-10-2011=============

#Region "View_Userlogindetails"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function view_UserLoginHistory(ByVal WhereCondition As String,
                                               ByVal DataRetrieval As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        view_UserLoginHistory = ObjHelp.view_UserLoginHistory(WhereCondition, DataRetrieval, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 17-10-2011=============

#Region " Proc_TreeViewOfNodes_BlankCRF "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_TreeViewOfNodes_BlankCRF(ByVal Parameters As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_TreeViewOfNodes_BlankCRF = ObjHelp.Proc_TreeViewOfNodes_BlankCRF(Parameters, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 17-10-2011=============

#Region " Proc_GetTableFormatDataForCRF "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetTableFormatDataForCRF(ByVal Parameters As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetTableFormatDataForCRF = ObjHelp.Proc_GetTableFormatDataForCRF(Parameters, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 22-11-2011=============

#Region "GetMyProjectCompletionListwithworkspacedesc"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMyProjectCompletionListwithworkspacedesc(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetMyProjectCompletionListwithworkspacedesc(prefixText, count, contextKey)
        Return items
    End Function
#End Region '======Added on 10-10-2011 by Mrunal


    '*************************************************************************************************
    '*************************************************************************************************

    '**************************************************
    'Added By Deepak Singh
    '**************************************************

#Region "getUserMstHistory"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getUserMstHistory(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim objHelp As clsHelpDbTable = Nothing
        objHelp = New clsHelpDbTable
        getUserMstHistory = objHelp.getUserMstHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '====added on 14-09-09===

#Region "getUserLoginHistory"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getUserLoginHistory(ByVal WhereCondition_1 As String,
                                                   ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                                   ByRef Sql_DataSet As Data.DataSet,
                                                   ByRef eStr_Retu As String) As Boolean
        Dim objHelp As clsHelpDbTable = Nothing
        objHelp = New clsHelpDbTable
        getUserLoginHistory = objHelp.getUserLoginHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '====added on 14-09-09====

#Region "View_UserMstHistoryAuditTrail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_UserMstHistoryAuditTrail(ByVal WhereCondition_1 As String,
                                                   ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                                   ByRef Sql_DataSet As Data.DataSet,
                                                   ByRef eStr_Retu As String) As Boolean
        Dim objHelp As clsHelpDbTable = Nothing
        objHelp = New clsHelpDbTable
        View_UserMstHistoryAuditTrail = objHelp.View_UserMstHistoryAuditTrail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '====added on 15-09-09===

#Region "View_UserLoginAuditTrail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_UserLoginAuditTrail(ByVal WhereCondition_1 As String,
                                                   ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                                   ByRef Sql_DataSet As Data.DataSet,
                                                   ByRef eStr_Retu As String) As Boolean
        Dim objHelp As clsHelpDbTable = Nothing
        objHelp = New clsHelpDbTable
        View_UserLoginAuditTrail = objHelp.View_UserLoginAuditTrail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '====added on 15-09-09===

#Region "View_PasswordHistoryAuditTrail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_PasswordHistoryAuditTrail(ByVal WhereCondition_1 As String,
                                                       ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                                       ByRef Sql_DataSet As Data.DataSet,
                                                       ByRef eStr_Retu As String) As Boolean
        Dim objHelp As clsHelpDbTable = Nothing
        objHelp = New clsHelpDbTable
        View_PasswordHistoryAuditTrail = objHelp.View_PasswordHistoryAuditTrail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '====added on 15-09-09===

#Region "View_UserAuditTrail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_UserAuditTrail(ByVal WhereCondition_1 As String,
                                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                                          ByRef Sql_DataSet As Data.DataSet,
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objHelp As clsHelpDbTable = Nothing
        objHelp = New clsHelpDbTable
        View_UserAuditTrail = objHelp.View_UserAuditTrail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region '====added on 15-09-09=====

#Region " GetUsersForDropDown "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetUsersForDropDown(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetUsersForDropDown = ObjHelp.GetUsersForDropDown(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region " GetWorktypeMst "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getWorkTypeMst(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getWorkTypeMst = ObjHelp.getWorkTypeMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region '==added on 08-12-09======

#Region "View_WorkspaceDtlForHdrwithCurrAttr"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_WorkspaceDtlForHdrwithCurrAttr(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_WorkspaceDtlForHdrwithCurrAttr = ObjHelp.View_WorkspaceDtlForHdrwithCurrAttr(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '==Added on 23-Dec-09======


#Region "Common project search for old and new projects"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetOldNewProjects(ByVal WhereCondition As String,
                                      ByVal WhereConditionForNewProjects As String,
                                      ByVal DataRetrival As DataRetrievalModeEnum,
                                      ByRef SqlDataSet As DataSet,
                                      ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetOldNewProjects = ObjHelp.GetOldNewProjects(WhereCondition, WhereConditionForNewProjects, DataRetrival, SqlDataSet, eStr_Retu)
    End Function
#End Region  ' Added by peatiksha for common old and new project search


#Region "GetMedExWorkspaceTemplateDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMedExWorkspaceTemplateDtl(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetMedExWorkspaceTemplateDtl = ObjHelp.GetMedExWorkspaceTemplateDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '==Added on 02-Mar-10====

#Region "View_MedExWorkspaceTemplateDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_MedExWorkspaceTemplateDtl(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_MedExWorkspaceTemplateDtl = ObjHelp.View_MedExWorkspaceTemplateDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '==Added on 09-Mar-10====

#Region "PKSampleDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function PKSampleDetail(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        PKSampleDetail = ObjHelp.PKSampleDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '==added on 31-Mar-2010===

#Region "View_PKSampleDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_PKSampleDetail(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_PKSampleDetail = ObjHelp.View_PKSampleDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '==added on 31-Mar-2010===

#Region "View_SubjectLabRptDtl"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SubjectLabRptDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SubjectLabRptDtl = ObjHelp.View_SubjectLabRptDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region  '==added on 28-Apr-2010===

#Region "View_SubjectLabReportDtl_Audit"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SubjectLabReportDtl_Audit(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SubjectLabReportDtl_Audit = ObjHelp.View_SubjectLabReportDtl_Audit(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region '==Added on 30-Apr-2010===

#Region "GetSourceDocDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSourceDocDtl(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSourceDocDtl = ObjHelp.GetSourceDocDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "VIEW_PreviewMedExTemplateDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function VIEW_PreviewMedExTemplateDtl(ByVal WhereCondition_1 As String,
                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                           ByRef Sql_DataSet As Data.DataSet,
                           ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        VIEW_PreviewMedExTemplateDtl = ObjHelp.VIEW_PreviewMedExTemplateDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetLabRptLockUnlockDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetLabRptLockUnlockDtl(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetLabRptLockUnlockDtl = ObjHelp.GetLabRptLockUnlockDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '==Added on 09-June-2010===

#Region "View_LabRptLockUnlockDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_LabRptLockUnlockDtl(ByVal WhereCondition_1 As String,
                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                           ByRef Sql_DataSet As Data.DataSet,
                           ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_LabRptLockUnlockDtl = ObjHelp.View_LabRptLockUnlockDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '==Added on 10-June-2010===

#Region "View_MaxScreeningLockDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_MaxScreeningLockDtl(ByVal WhereCondition_1 As String,
                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                           ByRef Sql_DataSet As Data.DataSet,
                           ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_MaxScreeningLockDtl = ObjHelp.View_MaxScreeningLockDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '===Added on 05-July-2010 ===

#Region "GetScreeningLockDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetScreeningLockDtl(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetScreeningLockDtl = ObjHelp.GetScreeningLockDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '===Added on 05-July-2010 ===

#Region "ChkLockedScreenDate"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function ChkLockedScreenDate(ByVal ScreeningHdrNo As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        ChkLockedScreenDate = ObjHelp.ChkLockedScreenDate(ScreeningHdrNo)
    End Function
#End Region '===Added on 07-July-2010 ===

#Region "getViewMaxMedExScreeningHdrDtl_Rpt"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getViewMaxMedExScreeningHdrDtl_Rpt(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getViewMaxMedExScreeningHdrDtl_Rpt = ObjHelp.getViewMaxMedExScreeningHdrDtl_Rpt(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '===Added on 07-July-2010 ===

#Region "View_MedExScreeningHdrHistory"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_MedExScreeningHdrHistory(ByVal WhereCondition_1 As String,
                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                           ByRef Sql_DataSet As Data.DataSet,
                           ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_MedExScreeningHdrHistory = ObjHelp.View_MedExScreeningHdrHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '===Added on 23-Aug-2010 ===

#Region "view_rptactivityDetailsReport"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function view_rptactivityDetailsReport(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        view_rptactivityDetailsReport = ObjHelp.view_rptactivityDetailsReport(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_GetOperational_KPIs"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetOperational_KPIs(ByVal StartDAte As String,
                                                ByVal EndDAte As String,
                                                ByRef Sql_DataSet As Data.DataSet,
                                                ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_GetOperational_KPIs = ObjHelp.Proc_GetOperational_KPIs(StartDAte, EndDAte, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_Get_BedNights"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_Get_BedNights(ByVal StartDAte As String,
                                                ByVal EndDAte As String,
                                                ByVal LocationCode As String,
                                                ByRef Sql_DataSet As Data.DataSet,
                                                ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_Get_BedNights = ObjHelp.Proc_Get_BedNights(StartDAte, EndDAte, LocationCode, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_CRFHdrDtlSubDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_CRFHdrDtlSubDtl(ByVal vProjectNo As String,
                                                ByVal vSubjectID As String,
                                                ByRef Sql_DataSet As Data.DataSet,
                                                ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_CRFHdrDtlSubDtl = ObjHelp.Proc_CRFHdrDtlSubDtl(vProjectNo, vSubjectID, Sql_DataSet, eStr_Retu)
    End Function
#End Region '===Added on 08-Dec-2010 for CRFReport

#Region "ReportTypeMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function ReportTypeMst(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        ReportTypeMst = ObjHelp.ReportTypeMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '===Added on 11-Feb-2011 for CRFReport

#Region "View_ReportTypeMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_ReportTypeMst(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_ReportTypeMst = ObjHelp.View_ReportTypeMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region  '===Added on 14-Feb-2011 for CRFReport

#Region "View_GetEligibleScreeningHdrDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_GetEligibleScreeningHdrDtl(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_GetEligibleScreeningHdrDtl = ObjHelp.View_GetEligibleScreeningHdrDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region   '===Added on 02-Mar-2011 for SubjectAssignment

#Region "GetViewSubjectBlobDetails_Housing"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewSubjectBlobDetails_Housing(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewSubjectBlobDetails_Housing = ObjHelp.GetView_SubjectBlobDetails_Housing(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region  '===Added on 01-Apr-2011 for Housing Photo Capturing

#Region "view_RptSubAttendance"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function view_RptSubAttendance(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        view_RptSubAttendance = ObjHelp.view_RptSubAttendance(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=== Added on 12-May-2011 for Attendance Report

#Region "GetProjectTypeDimensionDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetProjectTypeDimensionDtl(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetProjectTypeDimensionDtl = ObjHelp.GetProjectTypeDimensionDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=== Added on 30-August-2011 for geting ProjectTypeDimensionDtl

#Region "Get_EditChecksReport"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_EditChecksReport(ByVal Param As String,
                                                ByRef Sql_DataSet As Data.DataSet,
                                                ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_EditChecksReport = ObjHelp.Get_EditChecksReport(Param, Sql_DataSet, eStr_Retu)
    End Function

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_EditChecksExecutedReport(ByVal Param As String,
                                                ByRef Sql_DataSet As Data.DataSet,
                                                ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_EditChecksExecutedReport = ObjHelp.Get_EditChecksExecutedReport(Param, Sql_DataSet, eStr_Retu)
    End Function

#End Region '=== Added on 09-Nov-2011

#Region "View_MaxSubjectBlobdetails_ForOVIS"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_MaxSubjectBlobdetails_ForOVIS(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_MaxSubjectBlobdetails_ForOVIS = ObjHelp.View_MaxSubjectBlobdetails_ForOVIS(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "Get_TreeView_BlankCRF"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_TreeView_BlankCRF(ByVal WorkspaceId As String,
                                                     ByVal NodeId As Integer,
                                                     ByRef Sql_DataSet As Data.DataSet,
                                                     ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_TreeView_BlankCRF = ObjHelp.Get_TreeView_BlankCRF(WorkspaceId, NodeId, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "GetData"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetData(ByVal TableName As String,
                            ByVal ColumnNames As String,
                            ByVal WhereCondition As String,
                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                              ByRef Sql_DataSet As Data.DataSet,
                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetData = ObjHelp.GetData(TableName, ColumnNames, WhereCondition, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetViewWorkspaceSubjectMstInHouse"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewWorkspaceSubjectMstInHouse(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewWorkspaceSubjectMstInHouse = ObjHelp.GetViewWorkspaceSubjectMstInHouse(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

    '======For e-CTD ===========================================

#Region "GetAgencyMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetAgencyMst(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetAgencyMst = ObjHelp.GetAgencyMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "WorkspaceCMSMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function WorkspaceCMSMst(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        WorkspaceCMSMst = ObjHelp.WorkspaceCMSMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "view_WorkspaceCmsMst"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function view_WorkspaceCmsMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        view_WorkspaceCmsMst = ObjHelp.view_WorkspaceCmsMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "GetSubmissioninfoEU14Mst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubmissioninfoEU14Mst(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSubmissioninfoEU14Mst = ObjHelp.GetSubmissioninfoEU14Mst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetSubmissioninfoUSMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubmissioninfoUSMst(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSubmissioninfoUSMst = ObjHelp.GetSubmissioninfoUSMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetSubmissioninfoCAMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubmissioninfoCAMst(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSubmissioninfoCAMst = ObjHelp.GetSubmissioninfoCAMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "view_AllWorkspaceSubmissionInfo"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function view_AllWorkspaceSubmissionInfo(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        view_AllWorkspaceSubmissionInfo = ObjHelp.view_AllWorkspaceSubmissionInfo(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "GetWorkspacenodeattrdetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetWorkspacenodeattrdetail(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetWorkspacenodeattrdetail = ObjHelp.GetWorkspacenodeattrdetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetAttributeMstForEctd"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetAttributeMstForEctd(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetAttributeMstForEctd = ObjHelp.GetAttributeMstForEctd(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_DocumentHistory_ForECTD"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_DocumentHistory_ForECTD(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_DocumentHistory_ForECTD = ObjHelp.View_DocumentHistory_ForECTD(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_WorkSpaceNodeAttrDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_WorkSpaceNodeAttrDetail(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_WorkSpaceNodeAttrDetail = ObjHelp.View_WorkSpaceNodeAttrDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetAttributeValueMatrix"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetAttributeValueMatrix(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetAttributeValueMatrix = ObjHelp.GetAttributeValueMatrix(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "GetsubmissionInfoEU14Dtl"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetsubmissionInfoEU14Dtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetsubmissionInfoEU14Dtl = ObjHelp.GetsubmissionInfoEU14Dtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "View_WorkspaceNodeHistory_ForEctd"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_WorkspaceNodeHistory_ForEctd(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_WorkspaceNodeHistory_ForEctd = ObjHelp.View_WorkspaceNodeHistory_ForEctd(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Getstfcategorymst"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Getstfcategorymst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Getstfcategorymst = ObjHelp.Getstfcategorymst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "View_STFCategoryAttrValueMatrix"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_STFCategoryAttrValueMatrix(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_STFCategoryAttrValueMatrix = ObjHelp.View_STFCategoryAttrValueMatrix(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Getstfstudyidentifiermst"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Getstfstudyidentifiermst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Getstfstudyidentifiermst = ObjHelp.Getstfstudyidentifiermst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "Getstfcategoryattrvaluematrix"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Getstfcategoryattrvaluematrix(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Getstfcategoryattrvaluematrix = ObjHelp.Getstfcategoryattrvaluematrix(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "Getstfnodemst"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Getstfnodemst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Getstfnodemst = ObjHelp.Getstfnodemst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "View_WorkSpaceNodeDetailS_STF"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_WorkSpaceNodeDetailS_STF(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_WorkSpaceNodeDetailS_STF = ObjHelp.View_WorkSpaceNodeDetailS_STF(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetProc_TreeViewOfNodes_ECTD"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetProc_TreeViewOfNodes_ECTD(ByVal strParamValue As String) As DataSet
        Dim ObjDataLogic_New As ClsDataLogic_New = Nothing

        ObjDataLogic_New = New ClsDataLogic_New
        Return ObjDataLogic_New.ProcedureExecute("Proc_TreeViewOfNodes_ECTD", strParamValue)
    End Function
#End Region

    '=====================================
    'Created By : Bharat Patel
    'Created Date : 28-Nov-2011
    'Reason : Get the data from WorkspaceNodeDocHistory for the Source Doc
    '======================================
#Region "GetWorkspaceNodeDocHistory"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetWorkspaceNodeDocHistory(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetWorkspaceNodeDocHistory = ObjHelp.GetWorkspaceNodeDocHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "GetWorkspaceNodeCommentHistory"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetWorkspaceNodeCommentHistory(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetWorkspaceNodeCommentHistory = ObjHelp.GetWorkspaceNodeCommentHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "View_WorkspaceNodeCommentHistory"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_WorkspaceNodeCommentHistory(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_WorkspaceNodeCommentHistory = ObjHelp.View_WorkspaceNodeCommentHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "View_WorkSpaceNodeDocHistory"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_WorkSpaceNodeDocHistory(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_WorkSpaceNodeDocHistory = ObjHelp.View_WorkSpaceNodeDocHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_DocReleaseTrack"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_DocReleaseTrack(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_DocReleaseTrack = ObjHelp.View_DocReleaseTrack(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "View_DocReleaseTrack_AuditTrail"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_DocReleaseTrack_AuditTrail(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_DocReleaseTrack_AuditTrail = ObjHelp.View_DocReleaseTrack_AuditTrail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "DocReleaseTrack"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function DocReleaseTrack(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        DocReleaseTrack = ObjHelp.DocReleaseTrack(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "GetReleaseDocIdTrack"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetReleaseDocIdTrack(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetReleaseDocIdTrack = ObjHelp.GetReleaseDocIdTrack(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "View_MyReleasedDocuments"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_MyReleasedDocuments(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_MyReleasedDocuments = ObjHelp.View_MyReleasedDocuments(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "View_ReleaseDocMgmt"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_ReleaseDocMgmt(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_ReleaseDocMgmt = ObjHelp.View_ReleaseDocMgmt(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "ReleaseDocMgmt"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function ReleaseDocMgmt(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        ReleaseDocMgmt = ObjHelp.ReleaseDocMgmt(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "View_MedExWorkSpaceNodeDtl_Attr"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_MedExWorkSpaceNodeDtl_Attr(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_MedExWorkSpaceNodeDtl_Attr = ObjHelp.View_MedExWorkSpaceNodeDtl_Attr(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_CRFSubDtlForCategory"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_CRFSubDtlForCategory(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_CRFSubDtlForCategory = ObjHelp.View_CRFSubDtlForCategory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "View_SOPMailMgmt"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SOPMailMgmt(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SOPMailMgmt = ObjHelp.View_SOPMailMgmt(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "GetSOPMailMgmt"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSOPMailMgmt(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSOPMailMgmt = ObjHelp.GetSOPMailMgmt(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

    ''''' FOR GLOBAL DOCUMENT REPOSITORY '''''

#Region "MoleculeMst"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function MoleculeMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        MoleculeMst = ObjHelp.MoleculeMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region         '''''''''''''' Added On 15-Feb-2012 By Jugal Kundal

#Region "Proc_GetDetailsForGlobalDoc"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetDetailsForGlobalDoc(ByVal WorkspaceId As String,
                                                    ByVal Clients As String,
                                                    ByVal Molecules As String,
                                                    ByVal Locations As String,
                                                    ByVal ProjectType As String,
                                                    ByRef Sql_DataSet As Data.DataSet,
                                                    ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetDetailsForGlobalDoc = ObjHelp.Proc_GetDetailsForGlobalDoc(WorkspaceId, Clients, Molecules, Locations, ProjectType, Sql_DataSet, eStr_Retu)
    End Function
#End Region         '''''''''''''' Added On 18-Feb-2012 By Jugal Kundal

#Region "Proc_GetDetailsForProjectSynopsis"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetDetailsForProjectSynopsis(ByVal WorkspaceId As String,
                                                    ByVal Clients As String,
                                                    ByVal Molecules As String,
                                                    ByVal Locations As String,
                                                    ByVal ProjectType As String,
                                                    ByRef Sql_DataSet As Data.DataSet,
                                                    ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetDetailsForProjectSynopsis = ObjHelp.Proc_GetDetailsForProjectSynopsis(WorkspaceId, Clients, Molecules, Locations, ProjectType, Sql_DataSet, eStr_Retu)
    End Function
#End Region         '''''''''''''' Added On 18-Feb-2012 By Jugal Kundal

#Region "Proc_GetDetailsForGlobalDoc_temp"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetDetailsForGlobalDoc_temp(ByVal WorkspaceId As String,
                                                         ByVal FromDate As String,
                                                        ByVal ToDate As String,
                                                        ByRef Sql_DataSet As Data.DataSet,
                                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetDetailsForGlobalDoc_temp = ObjHelp.Proc_GetDetailsForGlobalDoc_temp(WorkspaceId, FromDate, ToDate, Sql_DataSet, eStr_Retu)
    End Function
#End Region


    '*************************************************************************************************
    '*************************************************************************************************

#Region "getInvoiceHdr "

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getInvoiceHdr(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getInvoiceHdr = ObjHelp.getInvoiceHdr(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region ' == For LIMS

#Region "getInvoiceDtl "

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getInvoiceDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getInvoiceDtl = ObjHelp.getInvoiceDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region ' == For LIMS

#Region "getInvoiceSubDtl "

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getInvoiceSubDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getInvoiceSubDtl = ObjHelp.getInvoiceSubDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region ' == For LIMS

#Region "GetSampleReportPrintingDetail"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSampleReportPrintingDetail(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSampleReportPrintingDetail = ObjHelp.GetSampleReportPrintingDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region ' == For LIMS

#Region "View_GetSampleMedExDetailForApproval"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_GetSampleMedExDetailForApproval(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_GetSampleMedExDetailForApproval = ObjHelp.View_GetSampleMedExDetailForApproval(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region ' == For LIMS

#Region "GetReferralMaster"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetReferralMaster(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetReferralMaster = ObjHelp.GetReferralMaster(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region ' == For LIMS

#Region "GetSubjectSampleMedexDetail"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubjectSampleMedexDetail(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSubjectSampleMedexDetail = ObjHelp.GetSubjectSampleMedexDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region ' == For LIMS

#Region "Getview_RptSampleCollection"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Getview_RptSampleCollection(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Getview_RptSampleCollection = ObjHelp.Getview_RptSampleCollection(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region ' == For LIMS

#Region "GetPatientMaster"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_PatientMaster(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_PatientMaster = ObjHelp.Get_PatientMaster(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region ' == For LIMS

#Region "GetSMSDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_SMSDetail(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_SMSDetail = ObjHelp.Get_SMSDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region ' == For LIMS

#Region "Get MedexTemplate Cost Detail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_MedexTemplateCostDetail(ByVal Choice_1 As Integer,
                                               ByVal DiscountCode_1 As Integer,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_MedexTemplateCostDetail = ObjHelp.Get_DiscountMedexTemplateMatrix(Choice_1, DiscountCode_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region ' == For LIMS

#Region "GetSubjectPatientMaster"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubjectPatientMaster(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSubjectPatientMaster = ObjHelp.GetSubjectPatientMaster(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region ' == For LIMS

#Region "GetDocumentUpload LIMS"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_DocumentUpload(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_DocumentUpload = ObjHelp.Get_DocumentUpload(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region ' == For LIMS

#Region "getTestCreationDetails LIMS"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getTestCreationDetails(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getTestCreationDetails = ObjHelp.getTestCreationDetails(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region ' == For LIMS

#Region "UploadDocument"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function UploadDocument(ByVal FileByte As Byte(),
                                   ByVal Path As String,
                                   ByVal FileName As String,
                                   ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        UploadDocument = ObjHelp.UploadDocument(FileByte, Path, FileName, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "GetDiscountMedExTemplateMatrix "

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetDiscountMedExTemplateMatrix(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetDiscountMedExTemplateMatrix = ObjHelp.GetDiscountMedExTemplateMatrix(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "GetTemplateCostMst "

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetTemplateCostMSt(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetTemplateCostMSt = ObjHelp.GetTemplateCostMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "GetView_MedExCostDtl"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetView_MedExCostDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetView_MedExCostDtl = ObjHelp.GetView_MedExCostDtl(WhereCondition_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region ' == For LIMS

#Region "Getview_RptInvoice"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Getview_RptInvoice(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Getview_RptInvoice = ObjHelp.Getview_RptInvoice(WhereCondition_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region ' == For LIMS

#Region "GetServerDateTime"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetServerDateTime() As DateTime
        Dim ObjDataLogic As ClsDataLogic_New = Nothing
        ObjDataLogic = New ClsDataLogic_New
        GetServerDateTime = ObjDataLogic.GetServerDateTime()
    End Function

#End Region ' == For LIMS

#Region "GetView_TemplateCostDtl"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetView_TemplateCostDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetView_TemplateCostDtl = ObjHelp.GetView_TemplateCostDtl(WhereCondition_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region ' == For LIMS

#Region "Get CityMst"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetCityMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetCityMst = ObjHelp.GetCityMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region ' == For LIMS

#Region "GetView_RptCollection"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetView_RptCollection(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetView_RptCollection = ObjHelp.GetView_RptCollection(WhereCondition_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region ' == For LIMS

#Region "GetKnockOffDetail"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetKnockOffDetail(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetKnockOffDetail = ObjHelp.GetKnockOffDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region ' == For LIMS

#Region "Getview_RptRecColection"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Getview_RptRecColection(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Getview_RptRecColection = ObjHelp.Getview_RptRecColection(WhereCondition_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region ' == For LIMS

#Region "Get_LaboratoryDetail"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_LaboratoryDetail(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_LaboratoryDetail = ObjHelp.Get_LaboratoryDetail(WhereCondition_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region ' == For LIMS

#Region "Get_SampleCollectedMaster"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_SampleCollectedMaster(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_SampleCollectedMaster = ObjHelp.Get_SampleCollectedMaster(WhereCondition_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region ' == For LIMS

#Region "Get_LabPatientMaster"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_LabPatientMaster(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_LabPatientMaster = ObjHelp.Get_LabPatientMaster(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region ' == For LIMS

#Region "GetSampleTemplateDetail"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSampleTemplateDetail(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSampleTemplateDetail = ObjHelp.GetSampleTemplateDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region ' == For LIMS

#Region "ExecuteQuery_Boolean"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function ExecuteQuery_Boolean(ByVal strQuery As String,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        ExecuteQuery_Boolean = ObjHelp.ExecuteQuery_Boolean(strQuery, eStr_Retu)
    End Function

#End Region  ' == For LIMS

#Region "ExecuteQuery_Scalar"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function ExecuteQuery_Scalar(ByVal strQuery As String) As Object
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        ExecuteQuery_Scalar = ObjHelp.ExecuteQuery_Scalar(strQuery)
    End Function

#End Region ' == For LIMS

#Region "Getview_InvoiceDetail"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Getview_InvoiceDetail(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Getview_InvoiceDetail = ObjHelp.Getview_InvoiceDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region  ' == For LIMS

#Region "getCashWithDrawal"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getCashWithDrawal(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getCashWithDrawal = ObjHelp.getCashWithDrawal(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region ' == For LIMS

#Region "getTemplateCreationDetails LIMS"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getTemplateCreationDetails(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getTemplateCreationDetails = ObjHelp.getTemplateCreationDetails(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region ' == For LIMS

#Region "Getview_SampleTemplateDetail"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Getview_SampleTemplateDetail(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Getview_SampleTemplateDetail = ObjHelp.Getview_SampleTemplateDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region   ' == For LIMS

#Region "GetTemplateReportMatrix "

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetTemplateReportMatrix(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetTemplateReportMatrix = ObjHelp.GetTemplateReportMatrix(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "Get_SampleRemarkDtl"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_SampleRemarkDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_SampleRemarkDtl = ObjHelp.GetSampleRemarkDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "Get_RemarkGroupMst"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_RemarkGroupMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_RemarkGroupMst = ObjHelp.GetRemarkGroupMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "Get_RemarkGroupDtl"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_RemarkGroupDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_RemarkGroupDtl = ObjHelp.GetRemarkGroupDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "Get_HistoParameterMst"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_HistoParameterMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_HistoParameterMst = ObjHelp.Get_HistoParameterMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "Get_TemplateRemarkDtl"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_TemplateRemarkDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_TemplateRemarkDtl = ObjHelp.Get_TemplateRemarkDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "Get_SampleCollecterMaster"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_SampleCollecterMaster(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_SampleCollecterMaster = ObjHelp.Get_SampleCollecterMaster(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "Get_HistoParameterDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_HistoParameterDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_HistoParameterDtl = ObjHelp.Get_HistoParameterDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "Get_ParameterHdr"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_ParameterHdr(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_ParameterHdr = ObjHelp.Get_ParameterHdr(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "Get_ParameterDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_ParameterDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_ParameterDtl = ObjHelp.Get_ParameterDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "Get_ViewSampleParameterDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_ViewSampleParameterDetail(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_ViewSampleParameterDetail = ObjHelp.Get_ViewSampleParameterDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "View_SampleDetailForLabInvoice"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SampleDetailForLabInvoice(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SampleDetailForLabInvoice = ObjHelp.View_SampleDetailForLabInvoice(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region   ' == For LIMS

#Region "GetView_LabUnAdjustCollectionDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetView_LabUnAdjustCollectionDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetView_LabUnAdjustCollectionDtl = ObjHelp.GetView_LabUnAdjustCollectionDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region ' == For LIMS

#Region "GetView_LabUnAdjustInvoiceDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetView_LabUnAdjustInvoiceDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetView_LabUnAdjustInvoiceDtl = ObjHelp.GetView_LabUnAdjustInvoiceDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region ' == For LIMS

#Region "Get_ViewRptSampleInfo"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_ViewRptSampleInfo(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_ViewRptSampleInfo = ObjHelp.Get_ViewRptSampleInfo(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS
#Region "Get_ViewDailyCashReport"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_ViewDailyCashReport(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_ViewDailyCashReport = ObjHelp.Get_viewRptDailyCash(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS


#Region "Get_ViewRptLabInvoice"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_ViewRptLabInvoice(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_ViewRptLabInvoice = ObjHelp.Get_ViewRptLabInvoice(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "Get_ViewDailyCash_New"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_ViewDailyCash_New(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_ViewDailyCash_New = ObjHelp.Get_RptDailyCash_New(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "Get_ViewRptLabCollection"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_ViewRptLabCollection(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_ViewRptLabCollection = ObjHelp.Get_ViewRptLabCollection(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "View_RptSampleCollector"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_RptSampleCollector(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_RptSampleCollector = ObjHelp.View_RptSampleCollector(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "View_ResultDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_ResultDetail(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_ResultDetail = ObjHelp.View_ResultDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "GetView_SampleWiseLabUnAdjustCollectionDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetView_SampleWiseLabUnAdjustCollectionDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetView_SampleWiseLabUnAdjustCollectionDtl = ObjHelp.GetView_SampleWiseLabUnAdjustCollectionDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region ' == For LIMS

#Region "GetView_SampleWiseLabUnAdjustInvoiceDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetView_SampleWiseLabUnAdjustInvoiceDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetView_SampleWiseLabUnAdjustInvoiceDtl = ObjHelp.GetView_SampleWiseLabUnAdjustInvoiceDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region ' == For LIMS

#Region "Getview_RptLABLedger"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Getview_RptLABLedger(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Getview_RptLABLedger = ObjHelp.Getview_RptLABLedger(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region ' == For LIMS

#Region "view_RptOutStanding"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function view_RptOutStanding(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        view_RptOutStanding = ObjHelp.view_RptOutStanding(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "Get_MedexRemarkDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_MedexRemarkDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_MedexRemarkDtl = ObjHelp.Get_MedexRemarkDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "Getview_RptMedExInfo_HistoDivisions"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Getview_RptMedExInfo_HistoDivisions(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Getview_RptMedExInfo_HistoDivisions = ObjHelp.Getview_RptMedExInfo_HistoDivisions(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "Getview_GroupwisePatientOutStanding"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Getview_GroupwisePatientOutStanding(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Getview_GroupwisePatientOutStanding = ObjHelp.Getview_GroupwisePatientOutStanding(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "Get_viewSampleDetailMedexGroupWise"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_viewSampleDetailMedexGroupWise(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_viewSampleDetailMedexGroupWise = ObjHelp.Get_viewSampleDetailMedexGroupWise(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "Get_viewGetPendingWork"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_viewGetPendingWork(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_viewGetPendingWork = ObjHelp.Get_viewGetPendingWork(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "Get_HistoSampleUserApproval"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_HistoSampleUserApproval(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_HistoSampleUserApproval = ObjHelp.Get_HistoSampleUserApproval(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "Get_view_RptLabKnockOffReport"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_view_RptLabKnockOffReport(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_view_RptLabKnockOffReport = ObjHelp.Get_view_RptLabKnockOffReport(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "Get_MedexFormulaMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_MedexFormulaMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_MedexFormulaMst = ObjHelp.Get_MedexFormulaMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "Get_view_GetLaboratorySample"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_view_GetLaboratorySample(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_view_GetLaboratorySample = ObjHelp.Get_view_GetLaboratorySample(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "Get_view_RptDiscount"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_view_RptDiscount(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_view_RptDiscount = ObjHelp.Get_view_RptDiscount(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "Get_LaboratoryNo"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_LaboratoryNo(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_LaboratoryNo = ObjHelp.Get_LaboratoryNo(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "view_GetDiscountRecord"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function view_GetDiscountRecord(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        view_GetDiscountRecord = ObjHelp.view_GetDiscountRecord(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "GetFranchiseCostMaster"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetFranchiseCostMaster(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetFranchiseCostMaster = ObjHelp.GetFranchiseCostMaster(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region '== For LIMS

#Region "GetSampleAuditorial"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSampleAuditorial(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSampleAuditorial = ObjHelp.GetSampleMedexAudittrial(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 19-03-2010=============

#Region "GetSampleInfo"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSampleInfo(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSampleInfo = ObjHelp.GetSampleInfo(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 19-03-2010=============

#Region "GetBoneMarrowSample"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetBoneMarrowSample(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetBoneMarrowSample = ObjHelp.GetBoneMarrowSample(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 29-03-2010=============

#Region "GetBoneMarrowParaMeter"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetBoneMarrowParaMeter(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetBoneMarrowParaMeter = ObjHelp.GetBoneMarrowParameter(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 29-03-2010=============

#Region "GetDispatchSample"  '==== Added On 16-Mar-2010=========

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetDispatchSample(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetDispatchSample = ObjHelp.GetDispatchSample(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region '=============Added on 03-06-2009=============

#Region "View_RptTRFInfo"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_RptTRFInfo(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_RptTRFInfo = ObjHelp.View_RptTRFInfo(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '================Added on 17-03-2010=============

#Region "Get_viewSendSamplePending"  '==== Added On 29-Mar-2010=========

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_viewSendSamplePending(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_viewSendSamplePending = ObjHelp.Get_viewSendSamplePending(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region '=============Added on 03-06-2009=============

#Region "Get_viewReceiveSamplePending"  '==== Added On 30-Mar-2010=========

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_viewReceiveSamplePending(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_viewReceiveSamplePending = ObjHelp.Get_viewReceiveSamplePending(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region '=============Added on 03-06-2009=============

#Region "GetView_RptBoneMarrowAspiration"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetView_RptBoneMarrowAspiration(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetView_RptBoneMarrowAspiration = ObjHelp.GetView_RptBoneMarrowAspiration(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "GetFranchiseMaster"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetFranchiseMaster(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetFranchiseMaster = ObjHelp.GetFranchiseMaster(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region '== For LIMS

#Region "GetBoneMarrowDiffCountMst"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetBoneMarrowDiffCountMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetBoneMarrowDiffCountMst = ObjHelp.GetBoneMarrowDiffCountMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region ' == For LIMS

#Region "View_BoneMarrowDiffCountDtl"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_BoneMarrowDiffCountDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_BoneMarrowDiffCountDtl = ObjHelp.View_BoneMarrowDiffCountDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region ' == For LIMS

#Region "GetBoneMarrowDiffCountMst"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetBoneMarrowDiffCountDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetBoneMarrowDiffCountDtl = ObjHelp.GetBoneMarrowDiffCountDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region ' == For LIMS

#Region "GetView_CollectionDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetView_CollectionDetail(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetView_CollectionDetail = ObjHelp.GetView_CollectionDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region ' == For LIMS

#Region "Get_View_DataExport_CDM"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_View_DataExport_CDM(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_View_DataExport_CDM = ObjHelp.GetView_DataExport_CDM(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region ' == For LIMS

#Region "View_SampleMedexRangeDtlForRevise"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SampleMedexRangeDtlForRevise(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SampleMedexRangeDtlForRevise = ObjHelp.View_SampleMedexRangeDtlForRevise(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region  ' == For LIMS 

#Region "view_UploadSampleID"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function view_UploadSampleID(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        view_UploadSampleID = ObjHelp.view_UploadSampleID(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "view_WorkspaceSubjectMstDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function view_WorkspaceSubjectMstDetail(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        view_WorkspaceSubjectMstDetail = ObjHelp.view_WorkspaceSubjectMstDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "view_RptGroupwisePatientOutStanding "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function view_RptGroupwisePatientOutStanding(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        view_RptGroupwisePatientOutStanding = ObjHelp.view_RptGroupwisePatientOutStanding(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region " Proc_GetSampleMedExDetailForApproval "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetSampleMedExDetailForApproval(ByVal FromDate_1 As String,
                                                             ByVal ToDate_1 As String,
                                                             ByVal MedexGroupCode_1 As String,
                                                             ByVal MedexCode_1 As String,
                                                             ByVal SubjectId_1 As String,
                                                             ByVal Workspaceid_1 As String,
                                                             ByVal Sampleid_1 As String,
                                                             ByRef Sql_DataSet As Data.DataSet,
                                                             ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_GetSampleMedExDetailForApproval = ObjHelp.Proc_GetSampleMedExDetailForApproval(FromDate_1, ToDate_1, MedexGroupCode_1, MedexCode_1, SubjectId_1, Workspaceid_1, Sampleid_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region  ' == For LIMS

#Region " Proc_ResultDetail "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_ResultDetail(ByVal FromDate_1 As String,
                                                             ByVal ToDate_1 As String,
                                                             ByVal MedexGroupCode_1 As String,
                                                             ByVal MedexCode_1 As String,
                                                             ByVal SubjectId_1 As String,
                                                             ByVal Workspaceid_1 As String,
                                                             ByVal Sampleid_1 As String,
                                                             ByRef Sql_DataSet As Data.DataSet,
                                                             ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_ResultDetail = ObjHelp.Proc_ResultDetail(FromDate_1, ToDate_1, MedexGroupCode_1, MedexCode_1, SubjectId_1, Workspaceid_1, Sampleid_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region  ' == For LIMS

#Region "Get_LabKitSendReceiveDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_LabKitSendReceiveDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_LabKitSendReceiveDtl = ObjHelp.Get_LabKitSendReceiveDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "View_LabKitSampleInfo"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_LabKitSampleInfo(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_LabKitSampleInfo = ObjHelp.View_LabKitSampleInfo(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "get_SampleDetailHistory"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function get_SampleDetailHistory(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        get_SampleDetailHistory = ObjHelp.Get_SampleDetailHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "View_LabKitSendReceiveReport"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_LabKitSendReceiveReport(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_LabKitSendReceiveReport = ObjHelp.View_LabKitSendReceiveReport(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "view_LabKitSendSamples"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function view_LabKitSendSamples(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        view_LabKitSendSamples = ObjHelp.view_LabKitSendSamples(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "view_LabKitBarcode"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function view_LabKitBarcode(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        view_LabKitBarcode = ObjHelp.view_LabKitBarcode(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "view_RptAddressGeneration"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function view_RptAddressGeneration(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        view_RptAddressGeneration = ObjHelp.view_RptAddressGeneration(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "View_SampleMedexWithType"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SampleMedexWithType(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SampleMedexWithType = ObjHelp.View_SampleMedexWithType(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "get_LabKitDispatchmst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function get_LabKitDispatchmst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        get_LabKitDispatchmst = ObjHelp.get_LabKitDispatchmst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "view_LabKitDispatch"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function view_LabKitDispatch(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        view_LabKitDispatch = ObjHelp.view_LabKitDispatch(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' == For LIMS

#Region "view_getCriticalMedexSamples"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function view_getCriticalMedexSamples(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        view_getCriticalMedexSamples = ObjHelp.view_getCriticalMedexSamples(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region   '== For LIMS

#Region " Proc_CheckSampleHIVPositive "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_CheckSampleHIVPositive(ByVal Sampleid_1 As String,
                                                             ByRef Sql_DataSet As Data.DataSet,
                                                             ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_CheckSampleHIVPositive = ObjHelp.Proc_CheckSampleHIVPositive(Sampleid_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region  ' == For LIMS

#Region "Get_ViewSampleDetail_positive"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Get_ViewSampleDetail_Positive(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Get_ViewSampleDetail_Positive = ObjHelp.Get_ViewSampleDetail_Positive(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region  ' == For LIMS

#Region "View_RptMedexInfo_Positive"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_RptMedexInfo_Positive(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_RptMedexInfo_Positive = ObjHelp.View_RptMedexInfo_Positive(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region  ' == For LIMS

#Region "view_SampleRemarkGroup"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function view_SampleRemarkGroup(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        view_SampleRemarkGroup = ObjHelp.view_SampleRemarkGroup(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region   '== For LIMS

#Region " Proc_DataExport_CDM_New "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_DataExport_CDM_New(ByVal FromDate_1 As String,
                                                             ByVal ToDate_1 As String,
                                                             ByVal WorkSpaceId_1 As String,
                                                             ByVal Period_1 As String,
                                                             ByVal ActivityId_1 As String,
                                                             ByVal MedexCode_1 As String,
                                                             ByRef Sql_DataSet As Data.DataSet,
                                                             ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_DataExport_CDM_New = ObjHelp.Proc_DataExport_CDM_New(FromDate_1, ToDate_1, WorkSpaceId_1, Period_1, ActivityId_1, MedexCode_1, Sql_DataSet, eStr_Retu)
    End Function

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_DataExport_CDM_New_Sample(ByVal SampleId_1 As String,
                                                             ByRef Sql_DataSet As Data.DataSet,
                                                             ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_DataExport_CDM_New_Sample = ObjHelp.Proc_DataExport_CDM_New_Sample(SampleId_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region  ' == For LIMS

#Region "View_LabKitDistributionReport"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_LabKitDistributionReport(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_LabKitDistributionReport = ObjHelp.View_LabKitDistributionReport(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region ' ==For LIMS[16-12-2011]

#Region "GetScreenigTmpTable"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetScreenigTmpTable(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetScreenigTmpTable = ObjHelp.getScreenigTmpTable(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetMedexResultCriticalRemarks"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMedexResultCriticalRemarks(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetMedexResultCriticalRemarks = ObjHelp.GetMedexResultCriticalRemarks(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region '== For LIMS 01 March 2012

#Region "GetSampleUnlockDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSampleUnlockDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSampleUnlockDtl = ObjHelp.GetSampleUnlockDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region '== For LIMS 01 March 2012

#Region " Proc_Delete_ScreenigTmpTable "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function DeleteScreenigTmpTable(ByVal nMedExScreenNo As Integer,
                                               ByVal vSubjectID As String,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        DeleteScreenigTmpTable = ObjHelp.DeleteScreenigTmpTable(nMedExScreenNo, vSubjectID, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 23-09-2011=============

#Region "Proc_GetActivityStatusCount"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetActivityStatusCount(ByVal WorkSpaceId_1 As String,
                                                        ByVal iPeriod As String,
                                                             ByRef Sql_DataSet As Data.DataSet,
                                                             ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_GetActivityStatusCount = ObjHelp.Proc_GetActivityStatusCount(WorkSpaceId_1, iPeriod, Sql_DataSet, eStr_Retu)
    End Function
#End Region '=============Added on 27-09-2011 on request of Suhani=============

    '====================================================================================
    'Web Methods Related To BA Module
    '====================================================================================

#Region "GetFreezerMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetFreezerMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetFreezerMst = ObjHelp.GetFreezerMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetSampleGroupMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSampleGroupMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSampleGroupMst = ObjHelp.GetSampleGroupMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetSampleStandardMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSampleStandardMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSampleStandardMst = ObjHelp.GetSampleStandardMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetCentrifugationMachineMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetCentrifugationMachineMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetCentrifugationMachineMst = ObjHelp.GetCentrifugationMachineMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetBagBatchMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetBagBatchMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetBagBatchMst = ObjHelp.GetBagBatchMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetSampleOperationMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSampleOperationMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSampleOperationMst = ObjHelp.GetSampleOperationMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetSampleOperationReasonMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSampleOperationReasonMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSampleOperationReasonMst = ObjHelp.GetSampleOperationReasonMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetSampleCentrifugationDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSampleCentrifugationDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSampleCentrifugationDtl = ObjHelp.GetSampleCentrifugationDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetSampleSeparationDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSampleSeparationDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSampleSeparationDtl = ObjHelp.GetSampleSeparationDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetBagBatchDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetBagBatchDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetBagBatchDtl = ObjHelp.GetBagBatchDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetSampleOperationDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSampleOperationDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSampleOperationDtl = ObjHelp.GetSampleOperationDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetSampleSendReceiveDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSampleSendReceiveDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSampleSendReceiveDtl = ObjHelp.GetSampleSendReceiveDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "GetCalibrationMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetCalibrationMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetCalibrationMst = ObjHelp.GetCalibrationMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "View_FreezerMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_FreezerMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_FreezerMst = ObjHelp.View_FreezerMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_CentrifugationMachineMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_CentrifugationMachineMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_CentrifugationMachineMst = ObjHelp.View_CentrifugationMachineMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_SampleGroupMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SampleGroupMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SampleGroupMst = ObjHelp.View_SampleGroupMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_SampleStandardMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SampleStandardMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SampleStandardMst = ObjHelp.View_SampleStandardMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_SampleOperationMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SampleOperationMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SampleOperationMst = ObjHelp.View_SampleOperationMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_SampleOperationReasonMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SampleOperationReasonMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SampleOperationReasonMst = ObjHelp.View_SampleOperationReasonMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_CalibrationMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_CalibrationMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_CalibrationMst = ObjHelp.View_CalibrationMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_BagBatchMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_BagBatchMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_BagBatchMst = ObjHelp.View_BagBatchMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_SamplePickUpDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SamplePickUpDetail(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SamplePickUpDetail = ObjHelp.View_SamplePickUpDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_SampleCentrifugationDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SampleCentrifugationDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SampleCentrifugationDtl = ObjHelp.View_SampleCentrifugationDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_SampleSeparationDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SampleSeparationDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SampleSeparationDtl = ObjHelp.View_SampleSeparationDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_SampleSendReceiveDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SampleSendReceiveDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SampleSendReceiveDtl = ObjHelp.View_SampleSendReceiveDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_SampleOperationDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SampleOperationDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SampleOperationDtl = ObjHelp.View_SampleOperationDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_BagBatchDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_BagBatchDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_BagBatchDtl = ObjHelp.View_BagBatchDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_SampleSeparationDtl_WithBagDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SampleSeparationDtl_WithBagDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SampleSeparationDtl_WithBagDtl = ObjHelp.View_SampleSeparationDtl_WithBagDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_SampleSeparationDtl_WithBatchDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SampleSeparationDtl_WithBatchDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SampleSeparationDtl_WithBatchDtl = ObjHelp.View_SampleSeparationDtl_WithBatchDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_MaxSampleOperationDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_MaxSampleOperationDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_MaxSampleOperationDtl = ObjHelp.View_MaxSampleOperationDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_SampleCentrifugationDtl_Audit"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SampleCentrifugationDtl_Audit(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SampleCentrifugationDtl_Audit = ObjHelp.View_SampleCentrifugationDtl_Audit(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_Listingreport_BA"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_Listingreport_BA(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_Listingreport_BA = ObjHelp.View_Listingreport_BA(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_SampleSeparationDtl_WithCCDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SampleSeparationDtl_WithCCDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SampleSeparationDtl_WithCCDtl = ObjHelp.View_SampleSeparationDtl_WithCCDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetGeneralRemarksmst" '===============Added on 23-Mar-2012=============
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetGeneralRemarksmst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetGeneralRemarksmst = ObjHelp.GetGeneralRemarksmst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_SampleDiscard"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SampleDiscard(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SampleDiscard = ObjHelp.View_SampleDiscard(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_GeneralRemarks"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_GeneralRemarks(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_GeneralRemarks = ObjHelp.View_GeneralRemarks(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetBA_DocumentAttachmst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetBA_DocumentAttachmst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetBA_DocumentAttachmst = ObjHelp.GetBA_DocumentAttachmst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_BA_DocumentAttachmst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_BA_DocumentAttachmst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_BA_DocumentAttachmst = ObjHelp.View_BA_DocumentAttachmst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetCCSampleRetrievalMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetCCSampleRetrievalMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetCCSampleRetrievalMst = ObjHelp.GetCCSampleRetrievalMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_CCSampleRetrievalMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_CCSampleRetrievalMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_CCSampleRetrievalMst = ObjHelp.View_CCSampleRetrievalMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "view_MaxCCSampleOperationDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function view_MaxCCSampleOperationDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        view_MaxCCSampleOperationDtl = ObjHelp.view_MaxCCSampleOperationDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_CCBagLabel"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_CCBagLabel(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_CCBagLabel = ObjHelp.View_CCBagLabel(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_QCBagLabel"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_QCBagLabel(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_QCBagLabel = ObjHelp.View_QCBagLabel(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetCCSampleRetrievalMstHistory"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetCCSampleRetrievalMstHistory(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetCCSampleRetrievalMstHistory = ObjHelp.GetCCSampleRetrievalMstHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_CCSampleRetrievalMstHistory"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_CCSampleRetrievalMstHistory(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_CCSampleRetrievalMstHistory = ObjHelp.View_CCSampleRetrievalMstHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region " Proc_OTBagLabel "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_OTBagLabel(ByVal Parameters As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_OTBagLabel = ObjHelp.Proc_OTBagLabel(Parameters, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_BagBatchMst_CDCAudit"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_BagBatchMst_CDCAudit(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_BagBatchMst_CDCAudit = ObjHelp.View_BagBatchMst_CDCAudit(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetWinNonlin_DocumentAttachMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetWinNonlin_DocumentAttachMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetWinNonlin_DocumentAttachMst = ObjHelp.GetWinNonlin_DocumentAttachMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_WinNonlin_DocumentAttachMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_WinNonlin_DocumentAttachMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_WinNonlin_DocumentAttachMst = ObjHelp.View_WinNonlin_DocumentAttachMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region " Proc_CRFHdrDtlSubDtl_Edit "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_CRFHdrDtlSubDtl_Edit(ByVal Parameters As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_CRFHdrDtlSubDtl_Edit = ObjHelp.Proc_CRFHdrDtlSubDtl_Edit(Parameters, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region " Proc_CRFHdrDtlSubDtl_Archive "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_CRFHdrDtlSubDtl_Archive(ByVal Parameters As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_CRFHdrDtlSubDtl_Archive = ObjHelp.Proc_CRFHdrDtlSubDtl_Archive(Parameters, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_SubjectLabReportDtl_Audit "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_SubjectLabReportDtl_Audit(ByVal Parameters As String,
                                                    ByRef Sql_DataSet As Data.DataSet,
                                                    ByRef eStr_Retu As String) As Boolean
        Dim objHelp As clsHelpDbTable = Nothing
        objHelp = New clsHelpDbTable
        Proc_SubjectLabReportDtl_Audit = objHelp.Proc_SubjectLabReportDtl_Audit(Parameters, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_SampleDetail "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_SampleDetail(ByVal Parameters As String,
                                                    ByRef Sql_DataSet As Data.DataSet,
                                                    ByRef eStr_Retu As String) As Boolean
        Dim objHelp As clsHelpDbTable = Nothing
        objHelp = New clsHelpDbTable
        Proc_SampleDetail = objHelp.Proc_SampleDetail(Parameters, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_SubjectLabRptDtl "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_SubjectLabRptDtl(ByVal Parameters As String,
                                                    ByRef Sql_DataSet As Data.DataSet,
                                                    ByRef eStr_Retu As String) As Boolean
        Dim objHelp As clsHelpDbTable = Nothing
        objHelp = New clsHelpDbTable
        Proc_SubjectLabRptDtl = objHelp.Proc_SubjectLabRptDtl(Parameters, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "view_ArchiveProjectList" 'added By Megha
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function view_ArchiveProjectList(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.view_ArchiveProjectList(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#Region "proc_medexscreeninghdrdtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function proc_medexscreeninghdrdtl(ByVal vSubjectId As String,
        ByVal dscreendate As String,
        ByVal schema As String,
        ByRef sql_DataSet As Data.DataSet,
        ByRef estr_Retu As String) As Boolean
        Dim objHelp As clsHelpDbTable = Nothing
        objHelp = New clsHelpDbTable
        proc_medexscreeninghdrdtl = objHelp.proc_medexscreeninghdrdtl(vSubjectId, dscreendate, schema, sql_DataSet, estr_Retu)
    End Function
#End Region

#Region "proc_MedexScreeningHdrDtlAuditTrail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function proc_MedexScreeningHdrDtlAuditTrail(ByVal vSubjectId As String,
        ByVal vMedexScreeningHdrNo As String,
        ByVal vMedexCode As String,
        ByVal vSchema As String,
        ByRef sql_DataSet As Data.DataSet,
        ByRef estr_Retu As String) As Boolean
        Dim objHelp As clsHelpDbTable = Nothing
        objHelp = New clsHelpDbTable
        proc_MedexScreeningHdrDtlAuditTrail = objHelp.proc_MedexScreeningHdrDtlAuditTrail(vSubjectId, vMedexScreeningHdrNo, vMedexCode, vSchema, sql_DataSet, estr_Retu)
    End Function
#End Region

#Region "Proc_Schema"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_Schema(ByVal Wstr As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_Schema = ObjHelp.Proc_Schema(Wstr, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetArchieveDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetArchieveDetail(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetArchieveDetail = ObjHelp.GetArchieveDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "getSchema"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getSchema(ByVal Wstr As String,
                                       ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getSchema = ObjHelp.getSchema(Wstr, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "view_ArchiveProjectMst "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function view_ArchiveProjectMst(ByVal WhereCondition_1 As String,
                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                               ByRef Sql_DataSet As Data.DataSet,
                               ByRef eStr_Retu As String) As Boolean
        Dim ObjDtLogic As ClsDataLogic_New = Nothing
        Dim Tbl_1 As Data.DataTable = Nothing

        Try

            ObjDtLogic = New ClsDataLogic_New
            view_ArchiveProjectMst = view_ArchiveProjectMst(WhereCondition_1, DataRetrieval_1, ObjDtLogic, Tbl_1, eStr_Retu)

            Sql_DataSet = New Data.DataSet
            Sql_DataSet.Tables.Add(Tbl_1)

        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try

    End Function

#End Region

#Region "view_ArchiveProjectAuditTrail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function view_ArchiveProjectAuditTrail(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        view_ArchiveProjectAuditTrail = ObjHelp.view_ArchiveProjectAuditTrail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

    Friend Function view_ArchiveProjectMst(ByVal WhereCondition_1 As String,
                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                              ByVal ObjDtLogic As ClsDataLogic_New,
                              ByRef Sql_DtTbl As Data.DataTable,
                              ByRef eStr_Retu As String) As Boolean


        If DataRetrieval_1 = DataRetrievalModeEnum.DatatTable_Query Then
            eStr_Retu = "Invalid Data Retrieval Option"
            Exit Function
        End If

        Try
            view_ArchiveProjectMst = ObjDtLogic.GetDataset("view_ArchiveProjectMst", "", WhereCondition_1, DataRetrieval_1, Sql_DtTbl)
        Catch ex As Exception
            eStr_Retu = ex.Message
        End Try
    End Function
#End Region

#Region "getWorkspaceDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getWorkspaceDetail(ByVal Wstr As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getWorkspaceDetail = ObjHelp.getWorkspaceDetail(Wstr, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_GetTotalSubjectForNode" '===== Added on 4-july-2012 by Megha
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetTotalSubjectForNode(ByVal Parameters As String,
        ByRef sql_DataSet As Data.DataSet,
        ByRef estr_Retu As String) As Boolean
        Try
            Dim ObjHelp As clsHelpDbTable = Nothing
            ObjHelp = New clsHelpDbTable
            Proc_GetTotalSubjectForNode = ObjHelp.Proc_GetTotalSubjectForNode(Parameters, sql_DataSet, estr_Retu)
            Return True
        Catch ex As Exception
            estr_Retu = ex.Message
            Return False
        End Try
    End Function
#End Region

#Region "Proc_GetTotalSubjectForNode_Archive" '===== Added on 12-july-2012 by Vikas
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetTotalSubjectForNode_Archive(ByVal Parameters As String,
        ByRef sql_DataSet As Data.DataSet,
        ByRef estr_Retu As String) As Boolean
        Try
            Dim ObjHelp As clsHelpDbTable = Nothing
            ObjHelp = New clsHelpDbTable
            Proc_GetTotalSubjectForNode_Archive = ObjHelp.Proc_GetTotalSubjectForNode_Archive(Parameters, sql_DataSet, estr_Retu)
            Return True
        Catch ex As Exception
            estr_Retu = ex.Message
            Return False
        End Try
    End Function
#End Region

#Region "Proc_GetNodeWithSubjectCount_Archive" '===== Added on 12-july-2012 by Vikas
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetNodeWithSubjectCount_Archive(ByVal Parameters As String,
        ByRef sql_DataSet As Data.DataSet,
        ByRef estr_Retu As String) As Boolean
        Try
            Dim ObjHelp As clsHelpDbTable = Nothing
            ObjHelp = New clsHelpDbTable
            Proc_GetNodeWithSubjectCount_Archive = ObjHelp.Proc_GetNodeWithSubjectCount_Archive(Parameters, sql_DataSet, estr_Retu)
            Return True
        Catch ex As Exception
            estr_Retu = ex.Message
            Return False
        End Try
    End Function
#End Region

#Region "proc_TotalEnterSubject_Archive "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function proc_TotalEnterSubject_Archive(ByVal Parameters As String,
        ByRef sql_DataSet As Data.DataSet,
        ByRef estr_Retu As String) As Boolean
        Try
            Dim ObjHelp As clsHelpDbTable = Nothing
            ObjHelp = New clsHelpDbTable
            proc_TotalEnterSubject_Archive = ObjHelp.proc_TotalEnterSubject_Archive(Parameters, sql_DataSet, estr_Retu)
            Return True
        Catch ex As Exception
            estr_Retu = ex.Message
            Return False
        End Try
    End Function
#End Region

#Region "CRFVersion "

#Region "Proc_cdc_dbo_CRFVersionMst_CT "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_cdc_dbo_CRFVersionMst_CT(ByVal Parameters As String,
        ByRef sql_DataSet As Data.DataSet,
        ByRef estr_Retu As String) As Boolean
        Try
            Dim ObjHelp As clsHelpDbTable = Nothing
            ObjHelp = New clsHelpDbTable
            Proc_cdc_dbo_CRFVersionMst_CT = ObjHelp.Proc_cdc_dbo_CRFVersionMst_CT(Parameters, sql_DataSet, estr_Retu)
            Return True
        Catch ex As Exception
            estr_Retu = ex.Message
            Return False
        End Try
    End Function
#End Region '===============Added by Megha=============

#Region "GetCrfVersionProjectList"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetCrfVersionProjectList(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetCrfVersionProjectList(prefixText, count, contextKey)
        Return items
    End Function
#End Region   'Added by vikas(25-07-2012)

#Region "GetProjectForTrainingGuideline"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetProjectForTrainingGuideline(ByVal prefixText As String,
                    ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetProjectForTrainingGuideline(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#Region "Proc_ActivityWiseVersionControl "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_ActivityWiseVersionControl(ByVal Parameters As String,
        ByRef sql_DataSet As Data.DataSet,
        ByRef estr_Retu As String) As Boolean
        Try
            Dim ObjHelp As clsHelpDbTable = Nothing
            ObjHelp = New clsHelpDbTable
            Proc_ActivityWiseVersionControl = ObjHelp.Proc_ActivityWiseVersionControl(Parameters, sql_DataSet, estr_Retu)
            Return True
        Catch ex As Exception
            estr_Retu = ex.Message
            Return False
        End Try
    End Function
#End Region '===============Added by Megha=============
#Region "Proc_AttributeWiseVersionControl "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_AttributeWiseVersionControl(ByVal Parameters As String,
        ByRef sql_DataSet As Data.DataSet,
        ByRef estr_Retu As String) As Boolean
        Try
            Dim ObjHelp As clsHelpDbTable = Nothing
            ObjHelp = New clsHelpDbTable
            Proc_AttributeWiseVersionControl = ObjHelp.Proc_AttributeWiseVersionControl(Parameters, sql_DataSet, estr_Retu)
            Return True
        Catch ex As Exception
            estr_Retu = ex.Message
            Return False
        End Try
    End Function
#End Region '===============Added by Megha=============

#Region "Proc_SubjectWiseVersionDtls "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_SubjectWiseVersionDtls(ByVal Parameters As String,
        ByRef sql_DataSet As Data.DataSet,
        ByRef estr_Retu As String) As Boolean
        Try
            Dim ObjHelp As clsHelpDbTable = Nothing
            ObjHelp = New clsHelpDbTable
            Proc_SubjectWiseVersionDtls = ObjHelp.Proc_SubjectWiseVersionDtls(Parameters, sql_DataSet, estr_Retu)
            Return True
        Catch ex As Exception
            estr_Retu = ex.Message
            Return False
        End Try
    End Function
#End Region '===============Added by

#End Region

#Region "Proc__GetStoragedBagDetail1"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc__GetStoragedBagDetail1(ByVal Parameters As String,
                                                ByRef Sql_DataSet As Data.DataSet,
                                                ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc__GetStoragedBagDetail1 = ObjHelp.Proc__GetStoragedBagDetail1(Parameters, Sql_DataSet, eStr_Retu)

    End Function
#End Region

#Region "Proc_GetSamplePickupDetail"


    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetSamplePickupDetail(ByVal Parameters As String,
                                                      ByRef Sql_DataSet As Data.DataSet,
                                                      ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_GetSamplePickupDetail = ObjHelp.Proc_GetSamplePickupDetail(Parameters, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "Proc_GetHemolysedSampleDtl"


    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetHemolysedSampleDtl(ByVal Parameters As String,
                                                      ByRef Sql_DataSet As Data.DataSet,
                                                      ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_GetHemolysedSampleDtl = ObjHelp.Proc_GetHemolysedSampleDtl(Parameters, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "Proc_GetSampleVerificationDtl"


    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetSampleVerificationDtl(ByVal Parameters As String,
                                                      ByRef Sql_DataSet As Data.DataSet,
                                                      ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_GetSampleVerificationDtl = ObjHelp.Proc_GetSampleVerificationDtl(Parameters, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "Proc_GetSampleCentrifugationDtl2"


    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetSampleCentrifugationDtl2(ByVal Parameters As String,
                                                      ByRef Sql_DataSet As Data.DataSet,
                                                      ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_GetSampleCentrifugationDtl2 = ObjHelp.Proc_GetSampleCentrifugationDtl2(Parameters, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "Proc_GetMissingSampleDtl"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetMissingSampleDtl(ByVal Parameters As String,
                                             ByRef Sql_DataSet As Data.DataSet,
                                             ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_GetMissingSampleDtl = ObjHelp.Proc_GetMissingSampleDtl(Parameters, Sql_DataSet, eStr_Retu)

    End Function
#End Region

#Region "Proc_GetFreezerStatusLogBookNew"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetFreezerStatusLogBookNew(ByVal Parameters As String,
                                             ByRef Sql_DataSet As Data.DataSet,
                                             ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_GetFreezerStatusLogBookNew = ObjHelp.Proc_GetFreezerStatusLogBookNew(Parameters, Sql_DataSet, eStr_Retu)

    End Function
#End Region

#Region "Proc_GetSampleReplacedDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetSampleReplacedDtl(ByVal Parameters As String,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_GetSampleReplacedDtl = ObjHelp.Proc_GetSampleReplacedDtl(Parameters, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_GetVerificationDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetVerificationDtl(ByVal Parameters As String,
                                            ByRef Sql_DataSet As Data.DataSet,
                                            ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_GetVerificationDtl = ObjHelp.Proc_GetVerificationDtl(Parameters, Sql_DataSet, eStr_Retu)

    End Function
#End Region

#Region "Proc_GetSampleCentrifugationDtlFromCentrifugeId"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetSampleCentrifugationDtlFromCentrifugeId(ByVal Parameters As String,
                                            ByRef Sql_DataSet As Data.DataSet,
                                            ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_GetSampleCentrifugationDtlFromCentrifugeId = ObjHelp.Proc_GetSampleCentrifugationDtlFromCentrifugeId(Parameters, Sql_DataSet, eStr_Retu)

    End Function
#End Region

#Region "Proc_GetSampleCentrifugationDtlFromProject"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetSampleCentrifugationDtlFromProject(ByVal Parameters As String,
                                            ByRef Sql_DataSet As Data.DataSet,
                                            ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_GetSampleCentrifugationDtlFromProject = ObjHelp.Proc_GetSampleCentrifugationDtlFromProject(Parameters, Sql_DataSet, eStr_Retu)

    End Function
#End Region

#Region "Proc_GetFreezerLogSheet"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetFreezerLogSheet(ByVal Parameters As String,
                                            ByRef Sql_DataSet As Data.DataSet,
                                            ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_GetFreezerLogSheet = ObjHelp.Proc_GetFreezerLogSheet(Parameters, Sql_DataSet, eStr_Retu)

    End Function
#End Region

#Region "Proc_getFreezerstatusLogBook_NEW"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_getFreezerstatusLogBook_NEW(ByVal Parameters As String,
                                            ByRef Sql_DataSet As Data.DataSet,
                                            ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_getFreezerstatusLogBook_NEW = ObjHelp.Proc_getFreezerstatusLogBook_NEW(Parameters, Sql_DataSet, eStr_Retu)

    End Function
#End Region

#Region "Proc_FreezerLogSheet_New"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_FreezerLogSheet_New(ByVal Parameters As String,
                                            ByRef Sql_DataSet As Data.DataSet,
                                            ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_FreezerLogSheet_New = ObjHelp.Proc_FreezerLogSheet_New(Parameters, Sql_DataSet, eStr_Retu)

    End Function
#End Region

#Region "GetScreeningTemplateHdr"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetScreeningTemplateHdr(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Try
            Dim ObjHelp As clsHelpDbTable = Nothing
            ObjHelp = New clsHelpDbTable
            GetScreeningTemplateHdr = ObjHelp.GetScreeningTemplateHdr(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region

#Region "GetScreeningTemplateDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetScreeningTemplateDtl(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetScreeningTemplateDtl = ObjHelp.GetScreeningTemplateDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetSubjectScreeningTemplate"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubjectScreeningTemplate(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSubjectScreeningTemplate = ObjHelp.GetSubjectScreeningTemplate(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetViewScreeningTemplateHdrDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewScreeningTemplateHdrDtl(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewScreeningTemplateHdrDtl = ObjHelp.getViewScreeningTemplateHdrDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetViewScreeningTemplateDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewScreeningTemplateDtl(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewScreeningTemplateDtl = ObjHelp.GetViewScreeningTemplateDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_ScreeningTemplateHdrDtlAuditTrail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_ScreeningTemplateHdrDtlAuditTrail(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_ScreeningTemplateHdrDtlAuditTrail = ObjHelp.View_ScreeningTemplateHdrDtlAuditTrail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetBASampleFinalResults(ByVal WhereCondition_1 As String,
                                         ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                         ByRef Sql_DataSet As Data.DataSet,
                                         ByRef eStr_Retu As String) As Boolean
        Try
            Dim ObjHelp As clsHelpDbTable = Nothing
            ObjHelp = New clsHelpDbTable
            GetBASampleFinalResults = ObjHelp.GetBASampleFinalResults(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
        Catch ex As Exception
            Return False
        End Try

    End Function        '===============Added on 10-Oct-2012 by Pundarik=============

#Region "View_subjectLocation"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_subjectLocation(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_subjectLocation = ObjHelp.View_subjectLocation(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "view_PDsampledetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function view_PDsampledetail(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        view_PDsampledetail = ObjHelp.view_PDsampledetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_SubjectForCRFActivityStatusReport"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SubjectForCRFActivityStatusReport(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SubjectForCRFActivityStatusReport = ObjHelp.View_SubjectForCRFActivityStatusReport(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_ScreeningDetailsInPivotForm"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_ScreeningDetailsInPivotForm(ByVal Parameters As String,
                                            ByRef Sql_DataSet As Data.DataSet,
                                            ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_ScreeningDetailsInPivotForm = ObjHelp.Proc_ScreeningDetailsInPivotForm(Parameters, Sql_DataSet, eStr_Retu)

    End Function
#End Region

#Region "view_workspaceprotocoldetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function view_workspaceprotocoldetail(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        view_workspaceprotocoldetail = ObjHelp.view_workspaceprotocoldetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_ParentActivity" '===== Added on 17-NOV-2012 by Megha
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_ParentActivity(ByVal Parameters As String,
        ByRef sql_DataSet As Data.DataSet,
        ByRef estr_Retu As String) As Boolean
        Try
            Dim ObjHelp As clsHelpDbTable = Nothing
            ObjHelp = New clsHelpDbTable
            Proc_ParentActivity = ObjHelp.Proc_ParentActivity(Parameters, sql_DataSet, estr_Retu)
            Return True
        Catch ex As Exception
            estr_Retu = ex.Message
            Return False
        End Try
    End Function
#End Region

#Region "Proc_GetSampleSeparationDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetSampleSeparationDtl(ByVal Parameters As String,
        ByRef sql_DataSet As Data.DataSet,
        ByRef estr_Retu As String) As Boolean
        Try
            Dim ObjHelp As clsHelpDbTable = Nothing
            ObjHelp = New clsHelpDbTable
            Proc_GetSampleSeparationDtl = ObjHelp.Proc_GetSampleSeparationDtl(Parameters, sql_DataSet, estr_Retu)
            Return True
        Catch ex As Exception
            estr_Retu = ex.Message
            Return False
        End Try
    End Function
#End Region

#Region "Proc_Samplesendreceivedtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_Samplesendreceivedtl(ByVal Parameters As String,
        ByRef sql_DataSet As Data.DataSet,
        ByRef estr_Retu As String) As Boolean
        Try
            Dim ObjHelp As clsHelpDbTable = Nothing
            ObjHelp = New clsHelpDbTable
            Proc_Samplesendreceivedtl = ObjHelp.Proc_Samplesendreceivedtl(Parameters, sql_DataSet, estr_Retu)
            Return True
        Catch ex As Exception
            estr_Retu = ex.Message
            Return False
        End Try
    End Function
#End Region

#Region "Proc_GetSampleVerificationForBioAnalytical"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetSampleVerificationForBioAnalytical(ByVal Parameters As String,
        ByRef sql_DataSet As Data.DataSet,
        ByRef estr_Retu As String) As Boolean
        Try
            Dim ObjHelp As clsHelpDbTable = Nothing
            ObjHelp = New clsHelpDbTable
            Proc_GetSampleVerificationForBioAnalytical = ObjHelp.Proc_GetSampleVerificationForBioAnalytical(Parameters, sql_DataSet, estr_Retu)
            Return True
        Catch ex As Exception
            estr_Retu = ex.Message
            Return False
        End Try
    End Function
#End Region

#Region "GetWorkspaceScreeningHdr"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetWorkspaceScreeningHdr(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Try
            Dim ObjHelp As clsHelpDbTable = Nothing
            ObjHelp = New clsHelpDbTable
            GetWorkspaceScreeningHdr = ObjHelp.GetWorkspaceScreeningHdr(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region

#Region "GetWorkspaceScreeningDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetWorkspaceScreeningDtl(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Try
            Dim ObjHelp As clsHelpDbTable = Nothing
            ObjHelp = New clsHelpDbTable
            GetWorkspaceScreeningDtl = ObjHelp.GetWorkspaceScreeningDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region

#Region "View_WorkspaceScreeningHdrDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_WorkspaceScreeningHdrDtl(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Try
            Dim ObjHelp As clsHelpDbTable = Nothing
            ObjHelp = New clsHelpDbTable
            View_WorkspaceScreeningHdrDtl = ObjHelp.View_WorkspaceScreeningHdrDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region

#Region "View_GeneralScreeningHdrDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_GeneralScreeningHdrDtl(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Try
            Dim ObjHelp As clsHelpDbTable = Nothing
            ObjHelp = New clsHelpDbTable
            View_GeneralScreeningHdrDtl = ObjHelp.View_GeneralScreeningHdrDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region

#Region "GetMedExWorkSpaceScreeningHdr"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMedExWorkSpaceScreeningHdr(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetMedExWorkSpaceScreeningHdr = ObjHelp.GetMedExWorkSpaceScreeningHdr(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetMedExWorkSpaceScreeningDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMedExWorkSpaceScreeningDtl(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetMedExWorkSpaceScreeningDtl = ObjHelp.GetMedExWorkSpaceScreeningDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetMyProjectCompletionListForProjectSpecificScreening"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMyProjectCompletionListForProjectSpScr(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetMyProjectCompletionListForProjectSpScr(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#Region "Proc_GetSampleofCC"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetSampleofCC(ByVal Parameters As String,
        ByRef sql_DataSet As Data.DataSet,
        ByRef estr_Retu As String) As Boolean
        Try
            Dim ObjHelp As clsHelpDbTable = Nothing
            ObjHelp = New clsHelpDbTable
            Proc_GetSampleofCC = ObjHelp.Proc_GetSampleofCC(Parameters, sql_DataSet, estr_Retu)
            Return True
        Catch ex As Exception
            estr_Retu = ex.Message
            Return False
        End Try
    End Function
#End Region

#Region "Proc_GetSampleofQC"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetSampleofQC(ByVal Parameters As String,
        ByRef sql_DataSet As Data.DataSet,
        ByRef estr_Retu As String) As Boolean
        Try
            Dim ObjHelp As clsHelpDbTable = Nothing
            ObjHelp = New clsHelpDbTable
            Proc_GetSampleofQC = ObjHelp.Proc_GetSampleofQC(Parameters, sql_DataSet, estr_Retu)
            Return True
        Catch ex As Exception
            estr_Retu = ex.Message
            Return False
        End Try
    End Function
#End Region

#Region "Proc_GetSampleofOther"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetSampleofOther(ByVal Parameters As String,
        ByRef sql_DataSet As Data.DataSet,
        ByRef estr_Retu As String) As Boolean
        Try
            Dim ObjHelp As clsHelpDbTable = Nothing
            ObjHelp = New clsHelpDbTable
            Proc_GetSampleofOther = ObjHelp.Proc_GetSampleofOther(Parameters, sql_DataSet, estr_Retu)
            Return True
        Catch ex As Exception
            estr_Retu = ex.Message
            Return False
        End Try
    End Function
#End Region

#Region "GetBASubProjects"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetBASubProjects(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetBASubProjects(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#Region "Proc_CRFTermCode"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_CRFTermCode(ByVal Param As String,
                                            ByRef Sql_DataSet As Data.DataSet,
                                            ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_CRFTermCode = ObjHelp.Proc_CRFTermCode(Param, Sql_DataSet, eStr_Retu)
    End Function '========Added By Debashis==========
#End Region

#Region "VIEW_ODMStatusReport"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function VIEW_ODMStatusReport(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        VIEW_ODMStatusReport = ObjHelp.VIEW_ODMStatusReport(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "MedicalConditions"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getMedicalConditionsList(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getMedicalConditionsList = ObjHelp.getMedicalConditionsList(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "SubjectDtlCDMSMedicalCondition"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getSubjectDtlCDMSMedicalCondition(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getSubjectDtlCDMSMedicalCondition = ObjHelp.getSubjectDtlCDMSMedicalCondition(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "GetSubjectDtlCDMS"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getSubjectDtlCDMS(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getSubjectDtlCDMS = ObjHelp.getSubjectDtlCDMS(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function
#End Region

#Region "GetSubjectDtlCDMSHistory"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getSubjectDtlCDMSHistory(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getSubjectDtlCDMSHistory = ObjHelp.getSubjectDtlCDMSHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function
#End Region

#Region "GetSubjectDtlCDMSConsumption"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getSubjectDtlCDMSConsumption(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getSubjectDtlCDMSConsumption = ObjHelp.getSubjectDtlCDMSConsumption(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function
#End Region

#Region "Medication"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getCodeConcoMedicationList(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getCodeConcoMedicationList = ObjHelp.getCodeConcoMedicationList(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "SubjectDtlCDMSConcoMedication"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getSubjectDtlCDMSConcoMedication(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getSubjectDtlCDMSConcoMedication = ObjHelp.getSubjectDtlCDMSConcoMedication(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "View_AuditSubjectDtlCDMS"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_AuditSubjectDtlCDMS(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_AuditSubjectDtlCDMS = ObjHelp.View_AuditSubjectDtlCDMS(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "View_AuditSubjectDtlCDMSMedicalCondition"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_AuditSubjectDtlCDMSMedicalCondition(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_AuditSubjectDtlCDMSMedicalCondition = ObjHelp.View_AuditSubjectDtlCDMSMedicalCondition(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "View_AuditSubjectDtlCDMSConcoMedication"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_AuditSubjectDtlCDMSConcoMedication(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_AuditSubjectDtlCDMSConcoMedication = ObjHelp.View_AuditSubjectDtlCDMSConcoMedication(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "View_AuditProjectDtlCDMSConcoMedication"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_AuditProjectDtlCDMSConcoMedication(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_AuditProjectDtlCDMSConcoMedication = ObjHelp.View_AuditProjectDtlCDMSConcoMedication(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "View_AuditProjectDtlCDMSMedicalCondition"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_AuditProjectDtlCDMSMedicalCondition(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_AuditProjectDtlCDMSMedicalCondition = ObjHelp.View_AuditProjectDtlCDMSMedicalCondition(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "View_AuditSubjectDtlCDMSConsumption"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_AuditSubjectDtlCDMSConsumption(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_AuditSubjectDtlCDMSConsumption = ObjHelp.View_AuditSubjectDtlCDMSConsumption(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "GetCDMSSubjectCompletionList"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetCDMSSubjectCompletionList(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetCDMSSubjectCompletionList(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#Region "GetCDMSSubjectCompletionListActive"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetCDMSSubjectCompletionListActive(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetCDMSSubjectCompletionListActive(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#Region "View_SubjectDtlCDMSConsumption"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SubjectDtlCDMSConsumption(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SubjectDtlCDMSConsumption = ObjHelp.View_SubjectDtlCDMSConsumption(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "getCDMSConsumption"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getCDMSConsumption(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getCDMSConsumption = ObjHelp.getCDMSConsumption(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "GetStudyDtlCDMS"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getStudyDtlCDMS(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getStudyDtlCDMS = ObjHelp.getStudyDtlCDMS(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function
#End Region

#Region "GetStudyDtlCDMSHistory"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getStudyDtlCDMSHistory(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getStudyDtlCDMSHistory = ObjHelp.getStudyDtlCDMSHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function
#End Region

#Region "GetStudyDtlCDMSConsumption"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getStudyDtlCDMSConsumption(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getStudyDtlCDMSConsumption = ObjHelp.getStudyDtlCDMSConsumption(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function
#End Region

#Region "View_StudyInformationDtl"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_StudyInformationDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_StudyInformationDtl = ObjHelp.View_StudyInformationDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "View_StudyDtlCDMSConsumption"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_StudyDtlCDMSConsumption(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_StudyDtlCDMSConsumption = ObjHelp.View_StudyDtlCDMSConsumption(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "View_AuditStudyDtlCDMS"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_AuditStudyDtlCDMS(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_AuditStudyDtlCDMS = ObjHelp.View_AuditStudyDtlCDMS(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "View_AuditStudyDtlCDMSConsumption"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_AuditStudyDtlCDMSConsumption(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_AuditStudyDtlCDMSConsumption = ObjHelp.View_AuditStudyDtlCDMSConsumption(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "Proc_GetMathcedSubjects"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetMathcedSubjects(ByVal Parameters As String,
                                            ByRef Sql_DataSet As Data.DataSet,
                                            ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_GetMathcedSubjects = ObjHelp.Proc_GetMathcedSubjects(Parameters, Sql_DataSet, eStr_Retu)

    End Function
#End Region

#Region "View_GetSubjectDtlCDMSStatusLog"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_GetSubjectDtlCDMSStatusLog(ByVal wStr As String,
                                                 ByRef Sql_DataSet As Data.DataSet,
                                                 ByRef eStr_Retu As String) As Boolean
        Dim objDtLogic_New As ClsDataLogic_New = Nothing
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        View_GetSubjectDtlCDMSStatusLog = ObjHelp.View_GetSubjectDtlCDMSStatusLog(wStr, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetView_CDMSSubjectMaster"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetView_CDMSSubjectMaster(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetView_CDMSSubjectMaster = ObjHelp.GetView_CDMSSubjectMaster(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "ProjectDtlCDMSConcoMedication"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getProjectDtlCDMSConcoMedication(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getProjectDtlCDMSConcoMedication = ObjHelp.getProjectDtlCDMSConcoMedication(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "ProjectDtlCDMSMedicalCondition"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getProjectDtlCDMSMedicalCondition(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getProjectDtlCDMSMedicalCondition = ObjHelp.getProjectDtlCDMSMedicalCondition(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "Proc_ExecuteEditChecks"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_ExecuteEditChecks(ByVal Param As String,
                                            ByRef Sql_DataSet As Data.DataSet,
                                            ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_ExecuteEditChecks = ObjHelp.Proc_ExecuteEditChecks(Param, Sql_DataSet, eStr_Retu)
    End Function '========Added By Debashis==========
#End Region

#Region "Proc_ExecuteEditChecks_WithinPage"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_ExecuteEditChecks_WithinPage(ByVal Param As String,
                                            ByRef Sql_DataSet As Data.DataSet,
                                            ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_ExecuteEditChecks_WithinPage = ObjHelp.Proc_ExecuteEditChecks_WithinPage(Param, Sql_DataSet, eStr_Retu)
    End Function '========Added By Debashis==========
#End Region

#Region " Proc_GetTabularDataForActivity_CTM "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetTabularDataForActivity_CTM(ByVal Parameters As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetTabularDataForActivity_CTM = ObjHelp.Proc_GetTabularDataForActivity_CTM(Parameters, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "SubjectDtlCDMSStudyHistory  "

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getSubjectDtlCDMSStudyHistory(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getSubjectDtlCDMSStudyHistory = ObjHelp.getSubjectDtlCDMSStudyHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "View_SubjectDtlCDMSStudyHistory"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getView_SubjectDtlCDMSStudyHistory(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getView_SubjectDtlCDMSStudyHistory = ObjHelp.getView_SubjectDtlCDMSStudyHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "View_AuditSubjectDtlCDMSStudyHistory"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getView_AuditSubjectDtlCDMSStudyHistory(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getView_AuditSubjectDtlCDMSStudyHistory = ObjHelp.getView_AuditSubjectDtlCDMSStudyHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "GetTimeZoneMaster"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetTimeZoneMaster(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetTimeZoneMaster = ObjHelp.GetTimeZoneMaster(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetMedexDependency"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMedexDependency(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetMedexDependency = ObjHelp.GetMedexDependency(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetVIEW_MedexDependency"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetviewMedexDependency(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetviewMedexDependency = ObjHelp.GetviewMedexDependency(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_CRFHdrDtlSubDtl_ForCRFPrint"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_CRFHdrDtlSubDtl_ForCRFPrint(ByVal WhereCondition As String,
                                                     ByVal Columns As String,
                                                     ByRef Sql_DataSet As Data.DataSet,
                                                     ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_CRFHdrDtlSubDtl_ForCRFPrint = ObjHelp.View_CRFHdrDtlSubDtl_ForCRFPrint(WhereCondition, Columns, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_GetOperational_KPIs"
    <WebMethod(MessageName:="Temp"), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetOperational_KPIs(ByVal Parameters As String,
        ByRef sql_DataSet As Data.DataSet,
        ByRef estr_Retu As String) As Boolean
        Try
            Dim ObjHelp As clsHelpDbTable = Nothing
            ObjHelp = New clsHelpDbTable
            Proc_GetOperational_KPIs = ObjHelp.Proc_GetOperational_KPIs(Parameters, sql_DataSet, estr_Retu)
            Return True
        Catch ex As Exception
            estr_Retu = ex.Message
            Return False
        End Try
    End Function
#End Region

#Region "Proc_ActualAuditTrailTime"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_ActualAuditTrailTime(ByVal Parameters As String,
                                            ByRef Sql_DataSet As Data.DataSet,
                                            ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_ActualAuditTrailTime = ObjHelp.Proc_ActualAuditTrailTime(Parameters, Sql_DataSet, eStr_Retu)

    End Function
#End Region

#Region " PROC_GetLIMSProject "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function PROC_GetLIMSProject(ByVal FromDate_1 As String,
                                        ByVal ToDate_1 As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        PROC_GetLIMSProject = ObjHelp.PROC_GetLIMSProject(FromDate_1, ToDate_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetBAChildProjects"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getBAChildProjects(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getBAChildProjects = ObjHelp.getBAChildProjects(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "GetBAProjectAnalyteDtl"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getBAProjectAnalyteDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getBAProjectAnalyteDtl = ObjHelp.getBAProjectAnalyteDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "GetViewBAProjectAnalyteDtl"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getViewBAProjectAnalyteDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getViewBAProjectAnalyteDtl = ObjHelp.getViewBAProjectAnalyteDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "GetBAAnalyteSampleHdr"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getBAAnalyteSampleHdr(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getBAAnalyteSampleHdr = ObjHelp.getBAAnalyteSampleHdr(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "GetBAAnalyteSampleDtl"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getBAAnalyteSampleDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getBAAnalyteSampleDtl = ObjHelp.getBAAnalyteSampleDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "getBAWorkSPaceNodeDetail"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getBAWorkSPaceNodeDetail(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getBAWorkSPaceNodeDetail = ObjHelp.getBAWorkSPaceNodeDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "GetBaSampleReqDtl"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetBaSampleReqDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetBaSampleReqDtl = ObjHelp.GetBaSampleReqDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "GetBaSampleReqHdr"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetBaSampleReqHdr(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetBaSampleReqHdr = ObjHelp.GetBaSampleReqHdr(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region " Proc_CDMSSubjectDtlAdvanceQuery_COUNT "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_CDMSSubjectDtlAdvanceQuery_COUNT(ByVal Parameter As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_CDMSSubjectDtlAdvanceQuery_COUNT = ObjHelp.Proc_CDMSSubjectDtlAdvanceQuery_COUNT(Parameter, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region " Proc_CDMSSubjectDtlAdvanceQuery "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_CDMSSubjectDtlAdvanceQuery(ByVal Parameter As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_CDMSSubjectDtlAdvanceQuery = ObjHelp.Proc_CDMSSubjectDtlAdvanceQuery(Parameter, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_BAAnalyteSampleHdrDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_BAAnalyteSampleHdrDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_BAAnalyteSampleHdrDtl = ObjHelp.View_BAAnalyteSampleHdrDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_BASampleReqHdrDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_BASampleReqHdrDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_BASampleReqHdrDtl = ObjHelp.View_BASampleReqHdrDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetMyProjectCompletionListOnlyParent" 'Debashis
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMyProjectCompletionListParentOnly(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetMyProjectCompletionListParentOnly(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#Region "Get_SampleSeparationDtl_Lot"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSampleSeperationDtl_Lot(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSampleSeperationDtl_Lot = ObjHelp.GetSampleSeperationDtl_Lot(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetBAInstrumentMst"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getBAInstrumentMst(ByVal WhereCondition_1 As String,
                                       ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                       ByRef Sql_DataSet As Data.DataSet,
                                       ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getBAInstrumentMst = ObjHelp.getBAInstrumentMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "GetBAChildProjectsHistory"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetBAChildProjectsHistory(ByVal WhereCondition_1 As String,
                                      ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                      ByRef Sql_DataSet As Data.DataSet,
                                      ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetBAChildProjectsHistory = ObjHelp.GetBAChildProjectsHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function


#End Region

#Region "GetBAProjectAnalyteDtlHistory"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetBAProjectAnalyteDtlHistory(ByVal WhereCondition_1 As String,
                                      ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                      ByRef Sql_DataSet As Data.DataSet,
                                      ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetBAProjectAnalyteDtlHistory = ObjHelp.GetBAProjectAnalyteDtlHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function


#End Region

#Region "GetBaSequenceScheduleDtl"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetBaSequenceScheduleDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetBaSequenceScheduleDtl = ObjHelp.GetBaSequenceScheduleDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "GetBaSequenceScheduleHdr"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetBaSequenceScheduleHdr(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetBaSequenceScheduleHdr = ObjHelp.GetBaSequenceScheduleHdr(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "GetBaTemplateDtl"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetBaTemplateDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetBaTemplateDtl = ObjHelp.GetBaTemplateDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "GetBaTemplateHdr"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetBaTemplateHdr(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetBaTemplateHdr = ObjHelp.GetBaTemplateHdr(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "GetBAInstrumentDtl"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getBAInstrumentDtl(ByVal WhereCondition_1 As String,
                                       ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                       ByRef Sql_DataSet As Data.DataSet,
                                       ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getBAInstrumentDtl = ObjHelp.getBAInstrumentDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "getViewBABatchExport"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getViewBABatchExport(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getViewBABatchExport = ObjHelp.getViewBABatchExport(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "Proc_SetProjectMatrix"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_SetProjectMatrix(ByVal ParaMeter As String,
                                          ByRef Sql_DataSet As DataSet,
                                          ByRef eStr_Retu As String) As Boolean


        Dim objHelp As clsHelpDbTable = Nothing
        objHelp = New clsHelpDbTable
        Proc_SetProjectMatrix = objHelp.Proc_SetProjectMatrix(ParaMeter, Sql_DataSet, eStr_Retu)


    End Function
#End Region

#Region "Proc_SubjectScreeningInfo"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_SubjectScreeningInfo(ByVal Parameter As String,
                                       ByRef Sql_DataSet As Data.DataSet,
                                       ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_SubjectScreeningInfo = ObjHelp.Proc_SubjectScreeningInfo(Parameter, Sql_DataSet, eStr_Retu)
    End Function


#End Region

#Region " Proc_ScreeningAnalyticRatio "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_ScreeningAnalyticRatio(ByVal Parameter As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_ScreeningAnalyticRatio = ObjHelp.Proc_ScreeningAnalyticRatio(Parameter, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region " Proc_GetTotalDosedSubject "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetTotalDosedSubject(ByVal Parameter As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetTotalDosedSubject = ObjHelp.Proc_GetTotalDosedSubject(Parameter, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetBASequenceExportDtl"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetBASequenceExportDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetBASequenceExportDtl = ObjHelp.GetBASequenceExportDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "Proc_Login"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_Login(ByVal Parameters As String,
                                            ByRef Sql_DataSet As Data.DataSet,
                                            ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_Login = ObjHelp.Proc_Login(Parameters, Sql_DataSet, eStr_Retu)

    End Function
#End Region

#Region "GetViewBAChildProjects"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getViewBAChildProjects(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getViewBAChildProjects = ObjHelp.getViewBAChildProjects(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "GetBAAnalyteSampleDtlHistory"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetBAAnalyteSampleDtlHistory(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetBAAnalyteSampleDtlHistory = ObjHelp.GetBAAnalyteSampleDtlHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "GetBASetProjectMatrix"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetBASetProjectMatrix(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetBASetProjectMatrix = ObjHelp.GetBASetProjectMatrix(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "GetViewBASetProjectMatrix"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewBASetProjectMatrix(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewBASetProjectMatrix = ObjHelp.GetViewBASetProjectMatrix(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "GetBARetriveQCSets"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetBARetriveQCSets(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetBARetriveQCSets = ObjHelp.GetBARetriveQCSets(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "getViewStudySampleTimepoint"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getViewStudySampleTimepoint(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getViewStudySampleTimepoint = ObjHelp.getViewStudySampleTimepoint(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "GetPkSampleReviewDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetPkSampleReviewDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetPkSampleReviewDtl = ObjHelp.GetPkSampleReviewDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetViewgetpksamplereviewdtl "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewgetpksamplereviewdtl(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewgetpksamplereviewdtl = ObjHelp.GetViewgetpksamplereviewdtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "getViewBAExistsingSequenceDetail"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getViewBAExistsingSequenceDetail(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getViewBAExistsingSequenceDetail = ObjHelp.getViewBAExistsingSequenceDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "Proc_BASequenceExportDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_BASequenceExportDtl(ByVal Parameter As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_BASequenceExportDtl = ObjHelp.Proc_BASequenceExportDtl(Parameter, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetLocationWiseTime"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetLocationWiseTime(ByVal userid As Integer) As String
        Dim ObjDataLogic As ClsDataLogic_New = Nothing
        ObjDataLogic = New ClsDataLogic_New
        GetLocationWiseTime = ObjDataLogic.GetLocationWiseTime(userid)
    End Function

#End Region

#Region "GetMyProjectCompletionListDependUser "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMyProjectCompletionListDependUser(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetMyProjectCompletionListDependUser = ObjHelp.GetMyProjectCompletionListDependUser(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetBAFileDetail"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getBAFileDetail(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getBAFileDetail = ObjHelp.getBAFileDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "GetBAFileDetailHistory"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetBAFileDetailHistory(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Try
            Dim ObjHelp As clsHelpDbTable = Nothing
            ObjHelp = New clsHelpDbTable
            GetBAFileDetailHistory = ObjHelp.GetBAFileDetailHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region

#Region "View_BASequenceConcentrationMatrix"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_BASequenceConcentrationMatrix(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_BASequenceConcentrationMatrix = ObjHelp.View_BASequenceConcentrationMatrix(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "GetCentrifugationParameter"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetCentrifugationParameter(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetCentrifugationParameter = ObjHelp.GetCentrifugationParameter(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_BASequenceSamples"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_BASequenceSamples(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_BASequenceSamples = ObjHelp.View_BASequenceSamples(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "GetBASampleConcentrationFiles"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getBASampleConcentrationFiles(ByVal WhereCondition_1 As String,
                                       ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                       ByRef Sql_DataSet As Data.DataSet,
                                       ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getBASampleConcentrationFiles = ObjHelp.getBASampleConcentrationFiles(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "View_CRFSubDtlForCategory"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_CRFSubDtlForCategory_OLD(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_CRFSubDtlForCategory_OLD = ObjHelp.View_CRFSubDtlForCategory_OLD(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "View_BAExportedSequenceDtl"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_BAExportedSequenceDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_BAExportedSequenceDtl = ObjHelp.View_BAExportedSequenceDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "GetBARetriveCCSets"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetBARetriveCCSets(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetBARetriveCCSets = ObjHelp.GetBARetriveCCSets(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "GetSubjectCompletionList_NotRejected_OnlyID"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubjectCompletionList_NotRejected_OnlyId(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetSubjectCompletionList_NotRejected_OnlyID(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#Region "View_MaxIndividualSampleOperationDtl"
    'Get Individual sample detail (freezer,operation,compartment) from sampleoperationdtl
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_MaxIndividualSampleOperationDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_MaxIndividualSampleOperationDtl = ObjHelp.View_MaxIndividualSampleOperationDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "view_IndividualSampleOperationDtl"
    'Get All sample detail (freezer,operation,compartment) using bag,lot,period
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function view_IndividualSampleOperationDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        view_IndividualSampleOperationDtl = ObjHelp.view_IndividualSampleOperationDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "view_ActivityTree BABE For Multiple Projects"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_ActivityForMultipleProjects(ByVal vWorkspaceId_1 As String,
                                                              ByVal iPeriod As String,
                                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                                              ByRef Sql_DataSet As Data.DataSet,
                                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_ActivityForMultipleProjects = ObjHelp.Proc_ActivityForMultipleProjects(vWorkspaceId_1, iPeriod, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_CRFHdrDtlSubDtl_Print"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_CRFHdrDtlSubDtl_Print(ByVal WhereCondition As String,
                                                     ByVal Columns As String,
                                                     ByRef Sql_DataSet As Data.DataSet,
                                                     ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_CRFHdrDtlSubDtl_Print = ObjHelp.View_CRFHdrDtlSubDtl_Print(WhereCondition, Columns, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region " Proc_Test "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_Test(ByVal Parameters As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_Test = ObjHelp.Proc_Test(Parameters, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_CRFSubDtlForCategory_LatestForm"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_CRFSubDtlForCategory_LatestForm(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_CRFSubDtlForCategory_LatestForm = ObjHelp.View_CRFSubDtlForCategory_LatestForm(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "View_CCSampleRetrievalMstForDiscard"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_CCSampleRetrievalMstForDiscard(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_CCSampleRetrievalMstForDiscard = ObjHelp.View_CCSampleRetrievalMstForDiscard(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "GetViewWorkspaceWorkflowUserDtl_ForDMS"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewWorkspaceWorkflowUserDtl_ForDMS(ByVal Wstr As String,
                                                       ByRef Sql_DataSet As Data.DataSet,
                                                       ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewWorkspaceWorkflowUserDtl_ForDMS = ObjHelp.GetViewWorkspaceWorkflowUserDtl_ForDMS(Wstr, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_SampleSeparationDtlForBarcode"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_SampleSeparationDtlForBarcode(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_SampleSeparationDtlForBarcode = ObjHelp.View_SampleSeparationDtlForBarcode(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "View_GetBAFileDetailCount"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_GetBAFileDetailCount(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_GetBAFileDetailCount = ObjHelp.View_GetBAFileDetailCount(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "BAtest"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function BAtest(ByVal wStr As String,
                          ByRef Sql_DataSet As Data.DataSet,
                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        BAtest = ObjHelp.BAtest(wStr, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_GetActivityReport"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetActivityReport(ByVal vWorkSpaceList As String,
                                           ByVal vActivityList As String,
                                           ByVal iPeriod As String,
                                           ByVal iUserId As Integer,
                                           ByRef sql_DataSet As Data.DataSet,
                                           ByRef estr_Retu As String) As Boolean
        Dim objHelp As clsHelpDbTable = Nothing
        objHelp = New clsHelpDbTable
        Proc_GetActivityReport = objHelp.Proc_GetActivityReport(vWorkSpaceList, vActivityList, iPeriod, iUserId, sql_DataSet, estr_Retu)
    End Function
#End Region

#Region "View_CRFHdrDtlSubDtl_Review"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_CRFHdrDtlSubDtl_Review(ByVal WhereCondition As String,
                                                     ByVal Columns As String,
                                                     ByRef Sql_DataSet As Data.DataSet,
                                                     ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_CRFHdrDtlSubDtl_Review = ObjHelp.View_CRFHdrDtlSubDtl_Review(WhereCondition, Columns, Sql_DataSet, eStr_Retu)

    End Function
#End Region

#Region "View_BagBatchMstForBarcode"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_BagBatchMstForBarcode(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_BagBatchMstForBarcode = ObjHelp.View_BagBatchMstForBarcode(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_SampleMedExRangeDtl_Positive"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_SampleMedExRangeDtl_Positive(ByVal nSampleIds As String,
                                           ByRef sql_DataSet As Data.DataSet,
                                           ByRef estr_Retu As String) As Boolean
        Dim objHelp As clsHelpDbTable = Nothing
        objHelp = New clsHelpDbTable
        Proc_SampleMedExRangeDtl_Positive = objHelp.Proc_SampleMedExRangeDtl_Positive(nSampleIds, sql_DataSet, estr_Retu)
    End Function
#End Region

#Region "Proc_SampleMedExRangeDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_SampleMedExRangeDtl(ByVal nSampleIds As String,
                                           ByRef sql_DataSet As Data.DataSet,
                                           ByRef estr_Retu As String) As Boolean
        Dim objHelp As clsHelpDbTable = Nothing
        objHelp = New clsHelpDbTable
        Proc_SampleMedExRangeDtl = objHelp.Proc_SampleMedExRangeDtl(nSampleIds, sql_DataSet, estr_Retu)
    End Function
#End Region

#Region "Proc_GetActivityStatusCountRecords"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetActivityStatusCountRecords(ByVal vWorkSpaceId As String,
                                           ByVal iPeriod As String,
                                           ByVal cIsParentChecked As Char,
                                           ByVal ProjectType As Integer,
                                           ByVal cIsChild As Char,
                                           ByRef sql_DataSet As Data.DataSet,
                                           ByRef estr_Retu As String) As Boolean
        Dim objHelp As clsHelpDbTable = Nothing
        objHelp = New clsHelpDbTable
        Proc_GetActivityStatusCountRecords = objHelp.Proc_GetActivityStatusCountRecords(vWorkSpaceId, iPeriod, cIsParentChecked, ProjectType, cIsChild, sql_DataSet, estr_Retu)
    End Function
#End Region

#Region "view_IndividualSampleOperationDtlForCPMA"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function view_IndividualSampleOperationDtlForCPMA(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        view_IndividualSampleOperationDtlForCPMA = ObjHelp.view_IndividualSampleOperationDtlForCPMA(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Getview_MaxcentrifugationParameterMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Getview_MaxcentrifugationParameterMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Getview_MaxcentrifugationParameterMst = ObjHelp.view_MaxcentrifugationParameterMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Getview_centrifugationParameterMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Getview_centrifugationParameterMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Getview_centrifugationParameterMst = ObjHelp.view_centrifugationParameterMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetView_BagBatchMst_AuditTrail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetView_BagBatchMst_AuditTrail(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetView_BagBatchMst_AuditTrail = ObjHelp.View_BagBatchMst_AuditTrail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetView_GetAuditTrailStudySample"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetView_GetAuditTrailStudySample(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetView_GetAuditTrailStudySample = ObjHelp.View_GetAuditTrailStudySample(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetView_GetAuditTrailBaSample"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetView_GetAuditTrailBaSample(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetView_GetAuditTrailBaSample = ObjHelp.View_GetAuditTrailBaSample(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_GetDeletedCRFRecords"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetDeletedCRFRecords(ByVal vWorkSpaceId As String,
                                           ByVal iPeriod As String,
                                           ByVal vActivityId As String,
                                           ByVal vSubjectId As String,
                                           ByVal vDeletedBy As String,
                                           ByVal cIsParentChecked As Char,
                                           ByVal cIsChild As Char,
                                           ByRef sql_DataSet As Data.DataSet,
                                           ByRef estr_Retu As String) As Boolean
        Dim objHelp As clsHelpDbTable = Nothing
        objHelp = New clsHelpDbTable
        Proc_GetDeletedCRFRecords = objHelp.Proc_GetDeletedCRFRecords(vWorkSpaceId, iPeriod, vActivityId, vSubjectId, vDeletedBy, cIsParentChecked, cIsChild, sql_DataSet, estr_Retu)
    End Function
#End Region

#Region " GetDatesMonthsAndYears "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetDatesMonthsAndYears(ByVal str As String,
                                     ByRef Sql_DS As Data.DataSet,
                                     ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetDatesMonthsAndYears = ObjHelp.GetDatesMonthsAndYears(str, Sql_DS, eStr_Retu)
    End Function
#End Region

#Region "View_BagBatchMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_BagBatchMstForOperation(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_BagBatchMstForOperation = ObjHelp.View_BagBatchMstForOperation(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_GetDataForScheduling"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetDataForScheduling(ByVal vWorkSpaceId As String,
                                              ByVal iPeriod As String,
                                              ByVal vActivityId As String,
                                              ByVal iNodeId As String,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetDataForScheduling = ObjHelp.Proc_GetDataForScheduling(vWorkSpaceId, iPeriod, vActivityId, iNodeId, Sql_DataSet, eStr_Retu)
    End Function
#End Region
#Region "Proc_GetDataForScheduling_Deviation"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetDataForScheduling_Deviation(ByVal vWorkSpaceId As String,
                                              ByVal iPeriod As String,
                                              ByVal iNodeId As String,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef UserId As Integer,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetDataForScheduling_Deviation = ObjHelp.Proc_GetDataForScheduling_Deviation(vWorkSpaceId, iPeriod, iNodeId, Sql_DataSet, UserId, eStr_Retu)
    End Function
#End Region

#Region "Proc_ReleaseLabReport"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_ReleaseLabReport(ByVal nSampleIds As String, ByVal UserId As String,
                                           ByRef sql_DataSet As Data.DataSet,
                                           ByRef estr_Retu As String) As Boolean
        Dim objHelp As clsHelpDbTable = Nothing
        objHelp = New clsHelpDbTable
        Proc_ReleaseLabReport = objHelp.Proc_ReleaseLabReport(nSampleIds, UserId, sql_DataSet, estr_Retu)
    End Function
#End Region

#Region "proc_GetProjectList"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function proc_GetProjectList(ByVal Parameters As String,
                                                ByRef Sql_DataSet As Data.DataSet,
                                                ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        proc_GetProjectList = ObjHelp.proc_GetProjectList(Parameters, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_CRFActivityStatus_New"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_CRFActivityStatus_New(ByVal WhereCondition As String,
                                                     ByVal Columns As String,
                                                     ByRef Sql_DataSet As Data.DataSet,
                                                     ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_CRFActivityStatus_New = ObjHelp.View_CRFActivityStatus_New(WhereCondition, Columns, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_LabMachineTestCode"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_LabMachineTestCode(ByVal WhereCondition As String,
                                                     ByVal Columns As String,
                                                     ByRef Sql_DataSet As Data.DataSet,
                                                     ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_LabMachineTestCode = ObjHelp.View_LabMachineTestCode(WhereCondition, Columns, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_IndividualSampleOperationDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_IndividualSampleOperationDtl(ByVal vWorkSpaceId As String,
                                                      ByVal nBAchildProjectsNo As String,
                                                      ByVal strPeriod As String,
                                                      ByVal cIsBagOrBatch As String,
                                                      ByVal strsamplestandardno As String,
                                                      ByVal strmysubjectno As String,
                                                      ByVal strSampleoperationdtlno As String,
                                                      ByVal strdsenton As String,
                                                      ByVal cstatusindi As String,
                                                      ByVal strpagesize As String,
                                                      ByVal strpagenumber As String,
                                                      ByRef Sql_DataSet As Data.DataSet,
                                                      ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_IndividualSampleOperationDtl = ObjHelp.Proc_IndividualSampleOperationDtl(vWorkSpaceId.ToString(), nBAchildProjectsNo.ToString(), strPeriod.ToString(), cIsBagOrBatch.ToString(), strsamplestandardno.ToString(), strmysubjectno.ToString(), strSampleoperationdtlno.ToString(), strdsenton.ToString(), cstatusindi.ToString(), strpagesize.ToString(), strpagenumber.ToString(), Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_DataEntryControl_Lock"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_DataEntryControl_Lock(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_DataEntryControl_Lock = ObjHelp.View_DataEntryControl_Lock(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "ProcedureExecuteDatatableWorkspaceNodeDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function ProcedureExecute_WorkspaceNodeDetail(ByVal strProcedureName As String, ByVal DataTable_WorkspaceNodeDetail As DataTable,
                                                 ByVal WorkSpaceId As String,
                                                 ByVal ParentNodeId As Integer,
                                                 ByVal DataOpMode As Integer) As Boolean
        Dim ObjDataLogic_New As ClsDataLogic_New = Nothing

        ObjDataLogic_New = New ClsDataLogic_New
        Return ObjDataLogic_New.ProcedureExecute_WorkspaceNodeDetail(strProcedureName, DataTable_WorkspaceNodeDetail, WorkSpaceId, ParentNodeId, DataOpMode)
    End Function
#End Region

#Region "Proc_SendSamples"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_SendSamples(ByVal vWorkSpaceId As String,
                                              ByVal vLocationCode As String,
                                              ByVal vProjectTypeCode As String,
                                                ByVal SelectRecord As String,
                                                ByVal strpagesize As String,
                                                ByVal strpagenumber As String,
                                                 ByVal Fromdate As String,
                                                 ByVal ToDate As String,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_SendSamples = ObjHelp.Proc_SendSamples(vWorkSpaceId.ToString(), vLocationCode.ToString(), vProjectTypeCode.ToString(), SelectRecord.ToString(), strpagesize.ToString(), strpagenumber.ToString(), Fromdate.ToString(), ToDate.ToString(), Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_OperationRollAudit"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_OperationRollAudit(ByVal ddlUsertype As String,
                                              ByVal OperationType As String,
                                              ByVal userid As String,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_OperationRollAudit = ObjHelp.Proc_OperationRollAudit(ddlUsertype.ToString(), OperationType.ToString(), userid.ToString(), Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_DosingDetailForBarCOde Print"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_DosingDetailForBarCodePrint(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_DosingDetailForBarCodePrint = ObjHelp.View_DosingDetailForBarCodePrint(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "ReprintDeleteSampleAudit"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function ReprintDeleteSampleAudit(ByVal WhereCondition_1 As String,
         ByVal DataRetrieval_1 As DataRetrievalModeEnum,
         ByRef Sql_DataSet As Data.DataSet,
         ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        ReprintDeleteSampleAudit = ObjHelp.ReprintDeleteSampleAudit(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "proc_ReprintdeleteSampleAuditFORLabSample"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function proc_ReprintdeleteSampleAuditFORLabSample(ByVal vWorkSpaceId As String,
                                              ByVal cFilter As String,
                                              ByVal iperiod As Integer,
                                                ByVal inodeid As Integer,
                                                ByVal dfromcreationdatetime As String,
                                                ByVal dtocreationdatetime As String,
                                                ByVal cstatus As String,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        proc_ReprintdeleteSampleAuditFORLabSample = ObjHelp.proc_ReprintdeleteSampleAuditFORLabSample(vWorkSpaceId.ToString(), cFilter.ToString(), iperiod.ToString(), inodeid.ToString(), dfromcreationdatetime.ToString(), dtocreationdatetime.ToString(), cstatus.ToString(), Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "proc_ReprintdeleteSampleAuditFORPK"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function proc_ReprintdeleteSampleAuditFORPK(ByVal vWorkSpaceId As String,
                                              ByVal cFilter As String,
                                              ByVal iperiod As Integer,
                                                ByVal inodeid As Integer,
                                                ByVal dfromcreationdatetime As String,
                                                ByVal dtocreationdatetime As String,
                                                ByVal cstatus As String,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        proc_ReprintdeleteSampleAuditFORPK = ObjHelp.proc_ReprintdeleteSampleAuditFORPK(vWorkSpaceId.ToString(), cFilter.ToString(), iperiod.ToString(), inodeid.ToString(), dfromcreationdatetime.ToString(), dtocreationdatetime.ToString(), cstatus.ToString(), Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "proc_PksampleDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function proc_PksampleDetail(ByVal vWorkSpaceId As String,
                                              ByVal cFilter As String,
                                              ByVal iperiod As Integer,
                                                ByVal inodeid As Integer,
                                                ByVal dfromcreationdatetime As String,
                                                ByVal dtocreationdatetime As String,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        proc_PksampleDetail = ObjHelp.proc_PksampleDetail(vWorkSpaceId.ToString(), cFilter.ToString(), iperiod.ToString(), inodeid.ToString(), dfromcreationdatetime.ToString(), dtocreationdatetime.ToString(), Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_AuditTrailofActiveInactiveUser"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_AuditTrailofActiveInactiveUser(ByVal vUserProfileName As String,
                                              ByVal vUserName As String,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_AuditTrailofActiveInactiveUser = ObjHelp.Proc_AuditTrailofActiveInactiveUser(vUserProfileName.ToString(), vUserName.ToString(), Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_GetLegends"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetLegends(ByVal vWorkspaceId As String,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetLegends = ObjHelp.Proc_GetLegends(vWorkspaceId.ToString(), Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_GetProjectReviewerLevel"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetProjectReviewerLevel(ByVal vWorkspaceId As String,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetProjectReviewerLevel = ObjHelp.Proc_GetProjectReviewerLevel(vWorkspaceId.ToString(), Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_GetWorkSpceDeviationReport"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetWorkSpaceDeviationReport(ByVal vWorkspaceId As String,
                                                     ByRef Sql_DataSet As Data.DataSet,
                                                     ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetWorkSpaceDeviationReport = ObjHelp.Proc_GetWorkSpceDeviationReport(vWorkspaceId.ToString(), Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_GetWorkSpceDeviationReport"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetAuditTrailWorkSpaeceDeviationReport(ByVal WorkSpaceDeviationId As String,
                                                                ByVal WorkSpaceId As String,
                                                     ByRef Sql_DataSet As Data.DataSet,
                                                     ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetAuditTrailWorkSpaeceDeviationReport = ObjHelp.Proc_GetAuditTrailWorkSpaeceDeviationReport(WorkSpaceDeviationId.ToString(), WorkSpaceId, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_WorkSpaceDeviationReportForIndependentReport"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_WorkSpaceDeviationReportForIndependentReport(ByVal WorkSpaceDeviationId As String,
                                                                      ByVal SubjectId As String,
                                                                      ByVal WorkSpaceDeviationReportId As Integer,
                                                                      ByVal RefActivityId As Integer,
                                                                      ByVal ChildActivityId As Integer,
                                                     ByRef Sql_DataSet As Data.DataSet,
                                                     ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_WorkSpaceDeviationReportForIndependentReport = ObjHelp.Proc_WorkSpaceDeviationReportForIndependentReport(Convert.ToString(WorkSpaceDeviationId), Convert.ToString(SubjectId), WorkSpaceDeviationReportId, RefActivityId, ChildActivityId, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_WorkSpaceDeviationReportForParentVisit"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_WorkSpaceDeviationReportForParentVisit(ByVal WorkSpaceDeviationId As String,
                                                                 ByVal SubjectId As String,
                                                     ByRef Sql_DataSet As Data.DataSet,
                                                     ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_WorkSpaceDeviationReportForParentVisit = ObjHelp.Proc_WorkSpaceDeviationReportForParentVisit(Convert.ToString(WorkSpaceDeviationId), Convert.ToString(SubjectId), Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_GetLotInformation"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetLotInformation(ByVal vWorkspaceId As String,
                                           ByVal cIsBagOrBatch As String,
                                            ByVal nbachildprojectno As String,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_GetLotInformation = ObjHelp.Proc_GetLotInformation(vWorkspaceId.ToString(), cIsBagOrBatch.ToString(), nbachildprojectno.ToString(), Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_GetDeviationPeriod"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetDeviationPeriod(ByVal WorkSpaceDeviationId As String,
                                                     ByRef Sql_DataSet As Data.DataSet,
                                                     ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetDeviationPeriod = ObjHelp.Proc_GetDeviationPeriod(Convert.ToString(WorkSpaceDeviationId), Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_SampleRetrievalReport"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_SampleRetrievalReport(ByVal FromDate As String,
                                                             ByVal ToDate As String,
                                                             ByRef Sql_DataSet As Data.DataSet,
                                                             ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_SampleRetrievalReport = ObjHelp.Proc_SampleRetrievalReport(FromDate, ToDate, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "Proc_SampleRetrievalReport"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetEditCheckActivity(ByVal WorkSpaceId As String,
                                                             ByVal EditCheckType As String,
                                                             ByRef Sql_DataSet As Data.DataSet,
                                                             ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_GetEditCheckActivity = ObjHelp.Proc_GetEditCheckActivity(WorkSpaceId, EditCheckType, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "PROC_getBagLabel"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function PROC_getBagLabel(ByVal vWorkspaceId As String,
                                           ByVal cIsBagOrBatch As String,
                                            ByVal iLotStandardNO As String,
                                            ByVal nbachildprojectno As String,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        PROC_getBagLabel = ObjHelp.PROC_getBagLabel(vWorkspaceId.ToString(), cIsBagOrBatch.ToString(), iLotStandardNO.ToString(), nbachildprojectno.ToString(), Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_GetLabReportData"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetLabReportData(ByVal nSampleIds As String,
                                           ByRef sql_DataSet As Data.DataSet,
                                           ByRef estr_Retu As String) As Boolean
        Dim objHelp As clsHelpDbTable = Nothing
        objHelp = New clsHelpDbTable
        Proc_GetLabReportData = objHelp.Proc_GetLabReportData(nSampleIds, sql_DataSet, estr_Retu)
    End Function
#End Region

#Region "GetProjectReviewerMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetProjectReviewerMst(ByVal WhereCondition_1 As String,
         ByVal DataRetrieval_1 As DataRetrievalModeEnum,
         ByRef Sql_DataSet As Data.DataSet,
         ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        GetProjectReviewerMst = ObjHelp.GetProjectReviewerMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_GetAuditProjectReviewerLevel"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetAuditProjectReviewerLevel(ByVal vWorkspaceId As String,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetAuditProjectReviewerLevel = ObjHelp.Proc_GetAuditProjectReviewerLevel(vWorkspaceId.ToString(), Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "view_CCSampleRetrievalMstAuditDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function view_CCSampleRetrievalMstAuditDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        view_CCSampleRetrievalMstAuditDtl = ObjHelp.view_CCSampleRetrievalMstAuditDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_InActiveAccount"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_InActiveAccount(ByVal Parameters As String,
                                            ByRef Sql_DataSet As Data.DataSet,
                                            ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_InActiveAccount = ObjHelp.Proc_InActiveAccount(Parameters, Sql_DataSet, eStr_Retu)

    End Function
#End Region

#Region "PROC_GETPROFILELIST"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function PROC_GETPROFILELIST(ByVal nscopeno As String,
                                            ByVal iworkflowstageid As String,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        PROC_GETPROFILELIST = ObjHelp.PROC_GETPROFILELIST(nscopeno.ToString(), iworkflowstageid.ToString, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Viewclientmst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function get_Viewclientmst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        get_Viewclientmst = ObjHelp.get_Viewclientmst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetView_clientmstHistory(ByVal WhereCondition_1 As String,
                                           ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                           ByRef Sql_DataSet As Data.DataSet,
                                           ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetView_clientmstHistory = ObjHelp.GetView_clientmstHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetServiceDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetServiceDetail(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetServiceDetail = ObjHelp.GetServiceDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetView_ServiceMstHistory(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetView_ServiceMstHistory = ObjHelp.GetView_ServiceMstHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region

#Region "GetviewProjectSubTypeMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetviewProjectSubTypeMst(ByVal WhereCondition_1 As String,
                                              ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                              ByRef Sql_DataSet As Data.DataSet,
                                              ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetviewProjectSubTypeMst = ObjHelp.GetviewProjectSubTypeMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region " Proc_GetProjectReport "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetProjectReport(ByVal Parameters As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetProjectReport = ObjHelp.Proc_GetProjectReport(Parameters, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_GetActivityForDasgBoard"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetActivityForDashBoard(ByVal nMilestone As String,
                                                 ByVal vDeptCode As String,
                                                 ByRef Sql_DataSet As Data.DataSet,
                                                 ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetActivityForDashBoard = ObjHelp.Proc_GetActivityForDashBoard(nMilestone, vDeptCode.ToString, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "SMSGateWayDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSMSGateWayDetail(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSMSGateWayDetail = ObjHelp.GetSMSGateWayDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_GetVisitSchedulerreport"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetVisitSchedulerreport(ByVal WorkSpaceId_1 As String,
                                                        ByVal iPeriod As String,
                                                             ByVal vSubjectId As String,
                                                             ByVal iParentActivityNodeId As String,
                                                             ByVal iActivityNodeId As String,
                                                             ByVal cSubjectWiseFlag As String,
                                                             ByVal ProjectType As Integer,
                                                             ByVal cDataStatus As String,
                                                         ByVal iWorkFlowStageId As String,
                                                             ByRef Sql_DataSet As Data.DataSet,
                                                             ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_GetVisitSchedulerreport = ObjHelp.Proc_GetVisitSchedulerreport(WorkSpaceId_1, iPeriod, vSubjectId, iParentActivityNodeId, iActivityNodeId, cSubjectWiseFlag, ProjectType, cDataStatus, iWorkFlowStageId, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_getVisitSchedulerReport_ForCT"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_getVisitSchedulerReport_ForCT(ByVal WorkSpaceId_1 As String,
                                                  ByVal iPeriod As String,
                                                             ByVal vSubjectId As String,
                                                             ByVal iParentActivityNodeId As String,
                                                             ByVal iActivityNodeId As String,
                                                             ByVal cSubjectWiseFlag As String,
                                                             ByVal cDataStatus As String,
                                                         ByVal iWorkFlowStageId As String,
                                                             ByRef Sql_DataSet As Data.DataSet,
                                                             ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_getVisitSchedulerReport_ForCT = ObjHelp.Proc_getVisitSchedulerReport_ForCT(WorkSpaceId_1, iPeriod, vSubjectId, iParentActivityNodeId, iActivityNodeId, cSubjectWiseFlag, cDataStatus, iWorkFlowStageId, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_getVisitSchedulerReport_ForCTDataExport"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_getVisitSchedulerReport_ForCTDataExport(ByVal WorkSpaceId_1 As String,
                                                  ByVal iPeriod As String,
                                                             ByVal vSubjectId As String,
                                                             ByVal iParentActivityNodeId As String,
                                                             ByVal iActivityNodeId As String,
                                                             ByVal cSubjectWiseFlag As String,
                                                             ByVal cDataStatus As String,
                                                         ByVal iWorkFlowStageId As String,
                                                             ByRef Sql_DataSet As Data.DataSet,
                                                             ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_getVisitSchedulerReport_ForCTDataExport = ObjHelp.Proc_getVisitSchedulerReport_ForCTDataExport(WorkSpaceId_1, iPeriod, vSubjectId, iParentActivityNodeId, iActivityNodeId, cSubjectWiseFlag, cDataStatus, iWorkFlowStageId, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "MachineSampleWorkListDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMachineSampleWorkListDtl(ByVal WhereCondition_1 As String,
                       ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                       ByRef Sql_DataSet As Data.DataSet,
                       ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetMachineSampleWorkListDtl = ObjHelp.GetMachineSampleWorkListDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function
#End Region

#Region "SubjectIrisDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubjectIrisDetail(ByVal WhereCondition_1 As String,
                       ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                       ByRef Sql_DataSet As Data.DataSet,
                       ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSubjectIrisDetail = ObjHelp.GetSubjectIrisDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function
#End Region


#Region "Proc_ValidationForCopyEditChecks"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_ValidationForCopyEditChecks(ByVal FromWorkSpaceId As String,
                                                         ByVal ToWorkSpaceId As String,
                                                             ByRef Sql_DataSet As Data.DataSet,
                                                             ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_ValidationForCopyEditChecks = ObjHelp.Proc_ValidationForCopyEditChecks(FromWorkSpaceId, ToWorkSpaceId, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "SubjectIrisVerificationDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubjectIrisVerificationDetail(ByVal WhereCondition_1 As String,
                       ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                       ByRef Sql_DataSet As Data.DataSet,
                       ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSubjectIrisVerificationDetail = ObjHelp.GetSubjectIrisVerificationDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)

    End Function
#End Region

#Region "getSubjectPopulationMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function getSubjectPopulationMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        getSubjectPopulationMst = ObjHelp.getSubjectPopulationMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region " GetView_SubjectPopulationMstHistory "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetView_SubjectPopulationMstHistory(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetView_SubjectPopulationMstHistory = ObjHelp.GetView_SubjectPopulationMstHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetSubjectContactDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubjectContactDetail(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSubjectContactDetail = ObjHelp.GetSubjectContactDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region " Proc_WorkSpaceProjectTotalSubjectCount "

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_WorkSpaceProjectTotalSubjectCount(ByVal vWorkSpaceId As String, ByRef Sql_DataSet As Data.DataSet, ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_WorkSpaceProjectTotalSubjectCount = ObjHelp.Proc_WorkSpaceProjectTotalSubjectCount(vWorkSpaceId, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region " Proc_SiteWiseSubjectInformation "

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_SiteWiseSubjectInformation(ByVal vWorkSpaceId As String, ByRef Sql_DataSet As Data.DataSet, ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_SiteWiseSubjectInformation = ObjHelp.Proc_SiteWiseSubjectInformation(vWorkSpaceId, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region " Proc_GetWorkSpaceAllSubjectDetail "

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_WorkSpaceAllSubjectCount(ByVal vWorkSpaceId As String, ByRef Sql_DataSet As Data.DataSet, ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_WorkSpaceAllSubjectCount = ObjHelp.Proc_WorkSpaceAllSubjectCount(vWorkSpaceId, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region " Proc_WorkSpaceDeActivatedSubjectCount "

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_WorkSpaceDeActivatedSubjectCount(ByVal vWorkSpaceId As String, ByRef Sql_DataSet As Data.DataSet, ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_WorkSpaceDeActivatedSubjectCount = ObjHelp.Proc_WorkSpaceDeActivatedSubjectCount(vWorkSpaceId, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region " Proc_StudyDetail "

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_StudyDetail(ByVal vWorkSpaceId As String, ByRef Sql_DataSet As Data.DataSet, ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_StudyDetail = ObjHelp.Proc_StudyDetail(vWorkSpaceId, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region " Proc_SiteInformation"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_SiteInformation(ByVal vWorkSpaceId As String, ByRef Sql_DataSet As Data.DataSet, ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_SiteInformation = ObjHelp.Proc_SiteInformation(vWorkSpaceId, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region " Proc_SubSiteInformation"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_SubSiteInformation(ByVal vWorkSpaceId As String, ByRef Sql_DataSet As Data.DataSet, ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_SubSiteInformation = ObjHelp.Proc_SubSiteInformation(vWorkSpaceId, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region " Proc_GetParentProjectList"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetParentProjectList(ByRef Sql_DataSet As Data.DataSet, ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetParentProjectList = ObjHelp.Proc_GetParentProjectList(Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region " Proc_ProjectStudyDetail "

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_ProjectStudyDetail(ByVal vWorkSpaceId As String, ByRef Sql_DataSet As Data.DataSet, ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_ProjectStudyDetail = ObjHelp.Proc_ProjectStudyDetail(vWorkSpaceId, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region " Proc_GetWorkSpaceProjectAESAE "

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetWorkSpaceProjectAESAE(ByVal vWorkSpaceId As String, ByRef Sql_DataSet As Data.DataSet, ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetWorkSpaceProjectAESAE = ObjHelp.Proc_GetWorkSpaceProjectAESAE(vWorkSpaceId, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region " Proc_GetWorkSpaceProjectTrainingGuidline "

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetWorkSpaceProjectTrainingGuidline(ByVal vProjectNo As String, ByRef Sql_DataSet As Data.DataSet, ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetWorkSpaceProjectTrainingGuidline = ObjHelp.Proc_GetWorkSpaceProjectTrainingGuidline(vProjectNo, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region " Proc_GetAuditTrail "

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetAuditTrail(ByVal vUserGroupCode As String, ByRef Sql_DataSet As Data.DataSet, ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetAuditTrail = ObjHelp.Proc_GetAuditTrail(vUserGroupCode, Sql_DataSet, eStr_Retu)

    End Function

#End Region

#Region "Get Panel Display Data"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetPanelDisplayData(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetPanelDisplayData = ObjHelp.GetPanelDisplayData(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetPanelDisplay(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetPanelDisplay = ObjHelp.GetPanelDisplay(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetView_PanelDisplayHistory(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetView_PanelDisplayHistory = ObjHelp.GetView_PanelDisplayHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_UserMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_UserMst(ByVal UserName As String,
                                                         ByVal UserTypeCode As String,
                                                         ByVal OperationType As String,
                                                             ByRef Sql_DataSet As Data.DataSet,
                                                             ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_UserMst = ObjHelp.Proc_UserMst(UserName, UserTypeCode, OperationType, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetSubjectCompletionList_NotRejectedDataMerg"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubjectCompletionList_NotRejectedDataMerg(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetSubjectCompletionList_NotRejectedDataMerg(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#Region "Proc_ScreeningSameDayValidation"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_ScreeningSameDayValidation(ByVal vSubjectId As String, ByVal dScreenDate As String, ByRef Sql_DataSet As Data.DataSet, ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_ScreeningSameDayValidation = ObjHelp.Proc_ScreeningSameDayValidation(vSubjectId, dScreenDate, Sql_DataSet, eStr_Retu)

    End Function
#End Region

#Region "Proc_ScreeningVersionStatus"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_ScreeningVersionStatus(ByVal dScreenDate As String, ByVal vSubjectId As String, ByRef Sql_DataSet As Data.DataSet, ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_ScreeningVersionStatus = ObjHelp.Proc_ScreeningVersionStatus(dScreenDate, vSubjectId, Sql_DataSet, eStr_Retu)

    End Function
#End Region

#Region "Proc_SCREENINGTEMPLATEHDRDTL"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_SCREENINGTEMPLATEHDRDTL(ByVal vSubjectId As String, ByVal dScreenDate As String, ByVal vMedExGroupCode As String, ByVal vUserTypeCode As String, ByRef Sql_DataSet As Data.DataSet, ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_SCREENINGTEMPLATEHDRDTL = ObjHelp.Proc_SCREENINGTEMPLATEHDRDTL(vSubjectId, dScreenDate, vMedExGroupCode, vUserTypeCode, Sql_DataSet, eStr_Retu)

    End Function
#End Region

#Region "GetDocumentPrinterDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetDocumentPrinterDtl(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetDocumentPrinterDtl = ObjHelp.GetDocumentPrinterDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_GetDocumentPrinterDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_GetDocumentPrinterDtl(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_GetDocumentPrinterDtl = ObjHelp.View_GetDocumentPrinterDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_GetPrinterAuditTrail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_GetPrinterAuditTrail(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_GetPrinterAuditTrail = ObjHelp.View_GetPrinterAuditTrail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetSubjectCompletionList_NotRejected_BlockPeriodDataMerg"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubjectCompletionList_NotRejected_BlockPeriodDataMerg(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetSubjectCompletionList_NotRejected_BlockPeriodDataMerg(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#Region "GetSubjectCompletionList_NotRejected_OnlyID_BlockPeriodDataMerg"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubjectCompletionList_NotRejected_OnlyID_BlockPeriodDataMerg(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetSubjectCompletionList_NotRejected_OnlyID_BlockPeriodDataMerg(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#Region "GetSubjectCompletionList_NotRejected_OnlyIDDataMerg"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSubjectCompletionList_NotRejected_OnlyIDDataMerg(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetSubjectCompletionList_NotRejected_OnlyIDDataMerg(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#Region "noticemst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetNoticeMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetNoticeMst = ObjHelp.GetNoticeMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "ViewNoticeMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function get_ViewNoticeMst(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        get_ViewNoticeMst = ObjHelp.get_ViewNoticeMst(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_GetNotice "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetNotice(ByVal dFromDate As String,
                                              ByVal dToDate As String,
                                                         ByVal vUserTypeCode As String,
                                                         ByVal iUserId As String,
                                                         ByRef Sql_DataSet As Data.DataSet,
                                                         ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_GetNotice = ObjHelp.Proc_GetNotice(dFromDate, dToDate, vUserTypeCode, iUserId, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_DCFTrackingReport"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_DCFTrackingReport(ByVal vWorkSpaceId As String, ByVal vType As String, ByRef Sql_DataSet As Data.DataSet, ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_DCFTrackingReport = ObjHelp.Proc_DCFTrackingReport(vWorkSpaceId, vType, Sql_DataSet, eStr_Retu)

    End Function
#End Region

#Region "GetViewWorkSpaceUserNodeDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewWorkSpaceUserNodeDetail(ByVal WhereCondition As String,
                                                     ByRef Sql_DataSet As Data.DataSet,
                                                     ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewWorkSpaceUserNodeDetail = ObjHelp.GetViewWorkSpaceUserNodeDetail(WhereCondition, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetSDTMCoreDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSDTMCoreDtl(ByVal WhereCondition_1 As String,
                       ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                       ByRef Sql_DataSet As Data.DataSet,
                       ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSDTMCoreDtl = ObjHelp.GetSDTMCoreDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetSDTMRoleDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetSDTMRoleDtl(ByVal WhereCondition_1 As String,
                       ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                       ByRef Sql_DataSet As Data.DataSet,
                       ByRef eStr_Retu As String) As Boolean

        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetSDTMRoleDtl = ObjHelp.GetSDTMRoleDtl(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "View_GetScreeningDetailsForECG"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function View_GetScreeningDetailsForECG(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        View_GetScreeningDetailsForECG = ObjHelp.View_GetScreeningDetailsForECG(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '================Added By Vivek Patel ===================

#Region "Proc_GetMedExMst "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetMedExMst(ByVal Parameters As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetMedExMst = ObjHelp.Proc_GetMedExMst(Parameters, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_GetActivityOperationMatrix "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_GetActivityOperationMatrix(ByVal Parameters As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_GetActivityOperationMatrix = ObjHelp.Proc_GetActivityOperationMatrix(Parameters, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetOperationName"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetOperationName(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetOperationName(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#Region "GetViewUserWiseProfile"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetViewUserWiseProfile(ByVal WhereCondition_1 As String,
                                          ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetViewUserWiseProfile = ObjHelp.GetViewUserWiseProfile(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region  '================Added By Rinkal Makwana For DI-Soft ===================

#Region "Proc_SiteWiseSubjectInformationForDashboard"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_SiteWiseSubjectInformationForDashboard(ByVal Parameters As String,
                                        ByRef Sql_DataSet As Data.DataSet,
                                        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        Proc_SiteWiseSubjectInformationForDashboard = ObjHelp.Proc_SiteWiseSubjectInformationForDashboard(Parameters, Sql_DataSet, eStr_Retu)
    End Function

#End Region '================Added By Rinkal Makwana For DI-Soft Dashboard ===================

#Region "GetProjectDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetProjectDetail(ByVal WhereCondition_1 As String,
                                                       ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                          ByRef Sql_DataSet As Data.DataSet,
                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetProjectDetail = ObjHelp.GetProjectDetail(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function

#End Region '================Added By Rinkal Makwana For DI-Soft Dashboard ===================

#Region "GetEmailSetupDeatil"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetEmailSetupDeatil(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetEmailSetupDeatil = ObjHelp.GetEmailSetupDeatil(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '================ Added By Shyam Kamdar For EmailSetup ===================

#Region "GetEmailSetupMaster"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetEmailSetupMaster(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetEmailSetupMaster = ObjHelp.GetEmailSetupMaster(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region '================ Added ByShyam Kamdar For EmailSetup ===================


#Region " get_ViewEmailSetupHistory "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetView_EmailSetupHistory(ByVal WhereCondition_1 As String,
                                               ByVal DataRetrieval_1 As DataRetrievalModeEnum,
                                               ByRef Sql_DataSet As Data.DataSet,
                                               ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetView_EmailSetupHistory = ObjHelp.GetView_EmailSetupHistory(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetChildProjectCompletionList" 'Added on 27-04-2021 By Shyam Kamdar
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetChildProjectCompletionList(ByVal prefixText As String,
                ByVal count As Integer, ByVal contextKey As String) As String()
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim items() As String

        ObjHelp = New clsHelpDbTable

        items = ObjHelp.GetChildProjectCompletionList(prefixText, count, contextKey)
        Return items
    End Function
#End Region

#Region "GetMenu-Dashboard"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetMenuDashboard(ByVal UserTypeCode As String, ByVal AdjucatorConstant As String) As DataSet
        Dim objcommon As New clsCommon
        Return objcommon.getMenuDashboard(UserTypeCode, AdjucatorConstant)
    End Function

#End Region
#Region "GetImgTransmittalHdr"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Update_ImgTransmittalHdr(ByVal Parameters As String,
        ByRef sql_DataSet As Data.DataSet,
        ByRef estr_Retu As String) As Boolean
        Try
            Dim ObjHelp As clsHelpDbTable = Nothing
            ObjHelp = New clsHelpDbTable
            Update_ImgTransmittalHdr = ObjHelp.Update_ImgTransmittalHdr(Parameters, sql_DataSet, estr_Retu)
            Return True
        Catch ex As Exception
            estr_Retu = ex.Message
            Return False
        End Try
    End Function
#End Region

#Region " GetOTPInfoDetails "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetOTPInfoDetails(ByVal WhereCondition_1 As String,
        ByVal DataRetrieval_1 As DataRetrievalModeEnum,
        ByRef Sql_DataSet As Data.DataSet,
        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetOTPInfoDetails = ObjHelp.GetOTPInfoDetails(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_OTPInfo"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_OTPInfo(ByVal UserName As String,
                                                            ByRef Sql_DataSet As Data.DataSet,
                                                             ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing
        ObjHelp = New clsHelpDbTable
        Proc_OTPInfo = ObjHelp.Proc_OTPInfo(UserName, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "Proc_EmailSetupMailId"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Proc_EmailSetupMailId(ByVal Parameters As String,
        ByRef sql_DataSet As Data.DataSet,
        ByRef estr_Retu As String) As Boolean
        Dim objHelp As clsHelpDbTable = Nothing
        objHelp = New clsHelpDbTable
        Proc_EmailSetupMailId = objHelp.Proc_EmailSetupMailId(Parameters, sql_DataSet, estr_Retu)
    End Function
#End Region

#Region " GetExMsgInfoDetails "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function GetExMsgInfoDetails(ByVal WhereCondition_1 As String,
        ByVal DataRetrieval_1 As DataRetrievalModeEnum,
        ByRef Sql_DataSet As Data.DataSet,
        ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        GetExMsgInfoDetails = ObjHelp.GetExMsgInfoDetails(WhereCondition_1, DataRetrieval_1, Sql_DataSet, eStr_Retu)
    End Function
#End Region

#Region "GetUserType"
    <WebMethod()>
    Public Function GetUserType() As String
        Dim ObjHelp As clsHelpDbTable = Nothing
        Dim Sql_DataSet As New Data.DataSet
        Dim eStr_Retu As String = String.Empty
        ObjHelp = New clsHelpDbTable
        Try
            ObjHelp.getUserTypeMst(String.Empty, DataRetrievalModeEnum.DataTable_AllRecords, Sql_DataSet, eStr_Retu)
            Return JsonConvert.SerializeObject(Sql_DataSet.Tables(0))
        Catch ex As Exception
            Throw New Exception(ex.Message + vbCrLf + "Error while getting User Type.")
        End Try


    End Function
#End Region

End Class
