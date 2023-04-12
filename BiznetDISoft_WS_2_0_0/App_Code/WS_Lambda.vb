Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports SS.Web.Services

<System.CodeDom.Compiler.GeneratedCode("System.Xml", "2.0.50727.42")> _
    <System.SerializableAttribute()> _
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum DataRetrievalModeEnum
    DataTable_Empty = 1
    DataTable_AllRecords = 2
    DataTable_WithWhereCondition = 3
    DatatTable_Query = 4
End Enum

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

<System.CodeDom.Compiler.GeneratedCode("System.Xml", "2.0.50727.42")> _
<System.SerializableAttribute()> _
<System.Xml.Serialization.XmlTypeAttribute()> _
Public Enum DataObjStageEnum 'Enumeration Declaration for Stages
    DataObjStage_Created = 1
    DataObjStage_Reviewed = 2
    DataObjStage_Approved = 3
    DataObjStage_Rejected = 4
    DataObjStage_All = 5
    DataObjStage_Duplicate = 6
    DataObjStage_Delete = 7
    DataObjStage_View = 8
End Enum

<System.CodeDom.Compiler.GeneratedCode("System.Xml", "2.0.50727.42")> _
<System.SerializableAttribute()> _
<System.Xml.Serialization.XmlTypeAttribute()> _
Public Enum MasterEntriesEnum
    MasterEntriesEnum_TemplateMst = 1
    'Added By Naimesh Dave
    MasterEntriesEnum_OperationMst = 3
    MasterEntriesEnum_UserTypeMst = 4
    MasterEntriesEnum_roleOperationMatrix = 5
    MasterEntriesEnum_DrugMst = 6
    MasterEntriesEnum_regionmst = 7
    MasterEntriesEnum_UserMst = 8
    MasterEntriesEnum_userGroupMst = 9
    MasterEntriesEnum_resourcemst = 10
    MasterEntriesEnum_projectTypeMst = 11
    MasterEntriesEnum_clientmst = 12
    MasterEntriesEnum_activitymst = 13
    MasterEntriesEnum_LocationMst = 14
    MasterEntriesEnum_DoctypeMst = 15
    MasterEntriesEnum_StageMst = 16
    MasterEntriesEnum_TemplateTypeMst = 17
    MasterEntriesEnum_DeptMst = 18
    MasterEntriesEnum_SubjectFemaleDetails = 20
    MasterEntriesEnum_CRFHdr = 21
    MasterEntriesEnum_ProtocolCriterienMst = 22
    MasterEntriesEnum_CRFDrugScanReport = 23
    MasterEntriesEnum_DocTemplateMst = 24
    MasterEntriesEnum_CRFAlcoholtestresult = 25
    MasterEntriesEnum_CRFDosingDtl = 26
    MasterEntriesEnum_CRFICFDtl = 27
    MasterEntriesEnum_WorkspaceProtocolDetail = 28
    MasterEntriesEnum_AdrWorkspaceProtocol = 29
    MasterEntriesEnum_AdrSourceMst = 30
    MasterEntriesEnum_RouteOfAdminMst = 31
    MasterEntriesEnum_AgencyMst = 32
    MasterEntriesEnum_MedDRA_1_hlt_pref_term = 33
    MasterEntriesEnum_CountryMst = 34
    MasterEntriesEnum_MedExMst = 35
    MasterEntriesEnum_MedExGroupMst = 36
    MasterEntriesEnum_HolidayMst = 37
    MasterEntriesEnum_MedExTemplateDtl = 38
    MasterEntriesEnum_MedExWorkspaceDtl = 39
    MasterEntriesEnum_MedExSubGroupMst = 41
    MasterEntriesEnum_WorkspaceSubjectMst = 42
    MasterEntriesEnum_ActivityGroupMst = 43
    MasterEntriesEnum_ItemGroupMst = 44
    MasterEntriesEnum_ItemMst = 45
    MasterEntriesEnum_ScopeMst = 46
    MasterEntriesEnum_UOMMst = 47
    MasterEntriesEnum_ProjectGroupMst = 48
    MasterEntriesEnum_ProjectGroupWorkspaceMatrix = 49
    MasterEntriesEnum_AgeGroupMst = 50
    MasterEntriesEnum_MedExRangeMst = 51
    MasterEntriesEnum_LabTypeMst = 52
    MasterEntriesEnum_MedExCostMst = 53
    MasterEntriesEnum_CurrencyMst = 54
    MasterEntriesEnum_SampleTypeMst = 55
    MasterEntriesEnum_SampleReportPrintingDetail = 56
    MasterEntriesEnum_ReferralMaster = 57
    MasterEntriesEnum_PatientMaster = 58
    MasterEntriesEnum_DocumentUpload = 59
    MasterEntriesEnum_DiscountMedExTemplateMatrix = 60
    MasterEntriesEnum_SMSDetail = 61
    MasterEntriesEnum_SpecialityMst = 62
    MasterEntriesEnum_CityMst = 63
    MasterEntriesEnum_ReasonMst = 64
    MasterEntriesEnum_KnockOffDtl = 65
    MasterEntriesEnum_LabPatientMaster = 66
    MasterEntriesEnum_SampleRemarkDtl = 67
    MasterEntriesEnum_NarrativeMaster = 68
    MasterEntriesEnum_TemplateRemarkDtl = 69
    MasterEntriesEnum_SampleCollecterMaster = 70
    MasterEntriesEnum_CountryMaster = 71
    MasterEntriesEnum_SpecialityMaster = 72
    MasterEntriesEnum_HistoParameterDtl = 73
    MasterEntriesEnum_ChildWorkSpaceMstSite = 74
    MasterEntriesEnum_ParameterHdr = 75
    MasterEntriesEnum_ParameterDtl = 76
    MasterEntriesEnum_SampleParameterDetail = 77
    MasterEntriesEnum_RemarkGroupDtl = 78
    MasterEntriesEnum_MedexRemarkDtl = 79
    MasterEntriesEnum_HistoSampleUserApproval = 80
    MasterEntriesEnum_FranchiseCostMst = 81
    MasterEntriesEnum_MedExWorkspaceTemplateDtl = 82
    MasterEntriesEnum_SampleDispatch = 83
    MasterEntriesEnum_BoneMarrowParameterDtl = 84
    MasterEntriesEnum_EmailAlert = 85
    MastersEntriesEnum_ExpenseTypeMaster = 86
    MastersEntriesEnum_SetProjectMatrix = 87
    MasterEntriesEnum_ServiceMst = 88
    MasterEntriesEnum_PrinterDtl = 89
    MasterEntriesEnum_NoticeMst = 90
    MasterEntriesEnum_CRFBunchDtl = 91
    '****************************
End Enum

<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class WS_Lambda
    Inherits sWebService

#Region "HelloWorld"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function HelloWorld() As String
        Return "Hello World"
    End Function
#End Region

#Region "Save_TemplateMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_TemplateMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Masters_1 As MasterEntriesEnum, _
                                ByVal Ds_templateMst As DataSet, _
                                ByVal UserCode_1 As String, _
                                ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster

        Save_TemplateMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_templateMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_InsertTemplateLeafNode"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertTemplateLeafNode(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Ds_templateNodeDetail As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_InsertTemplateLeafNode = objMaster.Save_InsertTemplateLeafNode(Choice_1, Ds_templateNodeDetail, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_InsertTemplateNodeBefore"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertTemplateNodeBefore(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Ds_templateNodeDetail As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_InsertTemplateNodeBefore = objMaster.Save_InsertTemplateNodeBefore(Choice_1, Ds_templateNodeDetail, UserCode_1, eStr_Retu)

    End Function
#End Region

#Region "Save_InsertTemplateNodeAfter"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertTemplateNodeAfter(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds_templateNodeDetail As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_InsertTemplateNodeAfter = objMaster.Save_InsertTemplateNodeAfter(Choice_1, Ds_templateNodeDetail, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_InsertDataForWorkspaceNodeAttrHistory"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertDataForWorkspaceNodeAttrHistory(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                      ByVal Ds_WorkspaceNodeAttrHistroy As DataSet, _
                                      ByVal UserCode_1 As String, _
                                      ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim Tbl_WorkspaceNodeAttrHistroy As New DataTable
        Tbl_WorkspaceNodeAttrHistroy = Ds_WorkspaceNodeAttrHistroy.Tables(0)
        Save_InsertDataForWorkspaceNodeAttrHistory = objMaster.Save_InsertDataForWorkspaceNodeAttrHistory(Choice_1, Tbl_WorkspaceNodeAttrHistroy, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_InsertWorkspaceNodeHistory"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertWorkspaceNodeHistory(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                      ByVal Ds_WorkspaceNodeHistory As DataSet, _
                                      ByVal UserCode_1 As String, _
                                      ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim Tbl_WorkspaceNodeHistory As New DataTable
        Tbl_WorkspaceNodeHistory = Ds_WorkspaceNodeHistory.Tables(0)
        Save_InsertWorkspaceNodeHistory = objMaster.Save_InsertWorkspaceNodeHistory(Choice_1, Tbl_WorkspaceNodeHistory, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_InsertWorkspaceComments"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertWorkspaceComments(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                      ByVal Ds_WorkspaceComment As DataSet, _
                                      ByVal UserCode_1 As String, _
                                      ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_InsertWorkspaceComments = objMaster.Save_InsertWorkspaceComments(Choice_1, Ds_WorkspaceComment, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_ProcChangeProjectStatus"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_ProcChangeProjectStatus(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Ds_WorkspaceMst As DataSet, _
                                     ByVal UserCode_1 As String, _
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_ProcChangeProjectStatus = objMaster.Save_ProcChangeProjectStatus(Choice_1, Ds_WorkspaceMst, UserCode_1, eStr_Retu)
    End Function
#End Region

    '************************************************************************
    'Added By Naimesh Dave
    '************************************************************************

#Region "Save_InsertOperationMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertOperationMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Masters_1 As MasterEntriesEnum, _
                                     ByVal Ds_OperationMst As DataSet, _
                                     ByVal UserCode_1 As String, _
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_InsertOperationMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_OperationMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_InsertUserTypeMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertUserTypeMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Masters_1 As MasterEntriesEnum, _
                                     ByVal Ds_UserTypeMst As DataSet, _
                                     ByVal UserCode_1 As String, _
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_InsertUserTypeMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_UserTypeMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_InsertroleOperationMatrix"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertroleOperationMatrix(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                         ByVal Masters_1 As MasterEntriesEnum, _
                                         ByVal Ds_roleOperationMatrix As DataSet, _
                                         ByVal UserCode_1 As String, _
                                         ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_InsertroleOperationMatrix = objMaster.Save_Masters(Choice_1, Masters_1, Ds_roleOperationMatrix, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_workspacemst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_workspacemst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                         ByVal Ds_roleOperationMatrix As DataSet, _
                                         ByVal UserCode_1 As String, _
                                         ByRef RequestId As String, _
                                         ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        'Save_workspacemst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_roleOperationMatrix, UserCode_1, eStr_Retu)
        Save_workspacemst = objMaster.Save_WorkSpaceMst(Choice_1, Ds_roleOperationMatrix, UserCode_1, _
                                        RequestId, eStr_Retu)
    End Function
#End Region

#Region "Save_InsertDrugMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertDrugMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                        ByVal Masters_1 As MasterEntriesEnum, _
                                        ByVal Ds_OperationMst As DataSet, _
                                        ByVal UserCode_1 As String, _
                                        ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_InsertDrugMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_OperationMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_Insertregionmst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_Insertregionmst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                      ByVal Masters_1 As MasterEntriesEnum, _
                                      ByVal Ds_OperationMst As DataSet, _
                                      ByVal UserCode_1 As String, _
                                      ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_Insertregionmst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_OperationMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_InsertUserMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertUserMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Masters_1 As MasterEntriesEnum, _
                                     ByVal Ds_OperationMst As DataSet, _
                                     ByVal UserCode_1 As String, _
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_InsertUserMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_OperationMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_InsertuserGroupMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertuserGroupMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Masters_1 As MasterEntriesEnum, _
                                    ByVal Ds_OperationMst As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_InsertuserGroupMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_OperationMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_Insertresourcemst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertResourcemst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Masters_1 As MasterEntriesEnum, _
                                     ByVal Ds_OperationMst As DataSet, _
                                     ByVal UserCode_1 As String, _
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_InsertResourcemst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_OperationMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_InsertProjectTypeMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertProjectTypeMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Ds_OperationMst As DataSet, _
                                     ByVal UserCode_1 As String, _
                                     ByRef ProjectTypeCode As String, _
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        'Save_InsertProjectTypeMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_OperationMst, UserCode_1, ProjectTypeCode, eStr_Retu)
        Dim objDtLogic As New ClsDataLogic_New()
        Save_InsertProjectTypeMst = objMaster.Save_ProjectTypeMst(Choice_1, Ds_OperationMst, objDtLogic, UserCode_1, ProjectTypeCode, eStr_Retu)

    End Function
#End Region

#Region "Save_InsertClientmst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertClientmst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Masters_1 As MasterEntriesEnum, _
                                     ByVal Ds_OperationMst As DataSet, _
                                     ByVal UserCode_1 As String, _
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_InsertClientmst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_OperationMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_InsertActivityOperationMatrix"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertClientContactMatrix(ByVal Choice As DataObjOpenSaveModeEnum, _
                                                      ByVal Ds_ClientContactMatrix As Data.DataSet, _
                                                      ByVal UserCode_1 As String, _
                                                      ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_InsertClientContactMatrix = objMaster.Save_InsertClientContactMatrix(Choice, Ds_ClientContactMatrix, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_workspaceSubjectDocDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_workspaceSubjectDocDetail(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_workspaceSubjectDocDetail As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef TranNo_Retu As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_workspaceSubjectDocDetail = objMaster.Save_workspaceSubjectDocDetail(Choice_1, Ds_workspaceSubjectDocDetail, objDtLogic, UserCode_1, TranNo_Retu, eStr_Retu)
    End Function
#End Region

#Region "Save_StageMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_StageMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                      ByVal Masters_1 As MasterEntriesEnum, _
                                    ByVal Ds_StageMst As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster

        Save_StageMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_StageMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_DoctypeMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_DoctypeMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                      ByVal Masters_1 As MasterEntriesEnum, _
                                    ByVal Ds_DoctypeMst As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster

        Save_DoctypeMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_DoctypeMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_LocationMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_LocationMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                      ByVal Masters_1 As MasterEntriesEnum, _
                                    ByVal Ds_LocationMst As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster

        Save_LocationMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_LocationMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_InsertTemplateWorkflowUserDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertTemplateWorkflowUserDtl(ByVal Choice As DataObjOpenSaveModeEnum, _
                                        ByVal ds_TemplateWorkFlowUserDtl As DataSet, _
                                        ByVal DefaultUserRights As String, _
                                        ByVal UserCode_1 As String, _
                                        ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim Tbl_TemplateWorkFlowUserDtl As New DataTable
        Tbl_TemplateWorkFlowUserDtl = ds_TemplateWorkFlowUserDtl.Tables("TemplateWorkFlowUserDtl")
        Save_InsertTemplateWorkflowUserDtl = objMaster.Save_InsertTemplateWorkflowUserDtl(Choice, Tbl_TemplateWorkFlowUserDtl, DefaultUserRights, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Inactive_TemplateNodeDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Inactive_TemplateNodeDetail(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_workspaceSubjectDocDetail As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Inactive_TemplateNodeDetail = objMaster.Inactive_TemplateNodeDetail(Choice_1, Ds_workspaceSubjectDocDetail, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_DeptStageMatrix"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_DeptStageMatrix(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_DeptStageMatrix As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_DeptStageMatrix = objMaster.Save_DeptStageMatrix(Choice_1, Ds_DeptStageMatrix, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_Insert_WorkspaceWorkflowUserDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertWorkspaceWorkflowUserDtl(ByVal Choice As DataObjOpenSaveModeEnum, _
                                        ByVal ds_WorkspaceWorkflowUserDtl As DataSet, _
                                        ByVal DefaultUserRights As String, _
                                        ByVal UserCode_1 As String, _
                                        ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim Tbl_WorkspaceWorkflowUserDtl As New DataTable
        Tbl_WorkspaceWorkflowUserDtl = ds_WorkspaceWorkflowUserDtl.Tables("WorkspaceWorkflowUserDtl")
        Save_InsertWorkspaceWorkflowUserDtl = objMaster.Save_InsertWorkSpaceWorkflowUserDtl(Choice, Tbl_WorkspaceWorkflowUserDtl, DefaultUserRights, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_InsertTemplateNodeDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertTemplateNodeDetail(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                    ByVal Ds_TemplateNodeDetail As DataSet, _
                                                    ByVal UserCode_1 As String, _
                                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_InsertTemplateNodeDetail = objMaster.Save_InsertTemplateNodeDetail(Choice_1, Ds_TemplateNodeDetail, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_Insertcheckedoutfiledetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_Insertcheckedoutfiledetail(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                    ByVal Ds_checkedoutfiledetail As DataSet, _
                                                    ByVal UserCode_1 As String, _
                                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_Insertcheckedoutfiledetail = objMaster.Save_Insertcheckedoutfiledetail(Choice_1, Ds_checkedoutfiledetail, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_InsertActivityOperationMatrix"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertActivityOperationMatrix(ByVal Choice As DataObjOpenSaveModeEnum, _
                                                      ByVal Ds_ActivityOperationMatrix As Data.DataSet, _
                                                      ByVal UserCode_1 As String, _
                                                      ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_InsertActivityOperationMatrix = objMaster.Save_InsertActivityOperationMatrix(Choice, Ds_ActivityOperationMatrix, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_InsertProtocolCriterienMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertProtocolCriterienMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Masters_1 As MasterEntriesEnum, _
                                     ByVal Ds_ProtocolCriterienMst As DataSet, _
                                     ByVal UserCode_1 As String, _
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_InsertProtocolCriterienMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_ProtocolCriterienMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "ProtocolWorkspaceCriterienDtls"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertProtocolWorkspaceCriterienDtls(ByVal Choice As DataObjOpenSaveModeEnum, _
                                        ByVal Ds_ProtocolWorkspaceCriterienDtls As Data.DataSet, _
                                        ByVal UserCode_1 As String, _
                                        ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_InsertProtocolWorkspaceCriterienDtls = objMaster.Save_InsertProtocolWorkspaceCriterienDtls(Choice, Ds_ProtocolWorkspaceCriterienDtls, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "ProtocolWorkSpaceDetails"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertProtocolWorkSpaceDetails(ByVal Choice As DataObjOpenSaveModeEnum, _
                                        ByVal Ds_ProtocolWorkSpaceDetails As Data.DataSet, _
                                        ByVal UserCode_1 As String, _
                                        ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_InsertProtocolWorkSpaceDetails = objMaster.Save_InsertProtocolWorkSpaceDetails(Choice, Ds_ProtocolWorkSpaceDetails, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_DrugRegionMatrix"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_DrugRegionMatrix(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_DrugRegionMatrix As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_DrugRegionMatrix = objMaster.Save_DrugRegionMatrix(Choice_1, Ds_DrugRegionMatrix, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_DrugRegionMatrix"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Insert_WorkspaceActivitySequenceDeviation(ByVal Choice As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_WorkspaceActivitySequenceDeviation As DataSet, _
                                                          ByVal UserCode As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Insert_WorkspaceActivitySequenceDeviation = objMaster.Insert_WorkspaceActivitySequenceDeviation(Choice, Ds_WorkspaceActivitySequenceDeviation, objDtLogic, UserCode, eStr_Retu)
    End Function
#End Region

#Region "Save_DrugRegionPKMatrix"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_DrugRegionPKMatrix(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_DrugRegionPKMatrix As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_DrugRegionPKMatrix = objMaster.Save_DrugRegionPKMatrix(Choice_1, Ds_DrugRegionPKMatrix, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_InsertDocTemplateMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertDocTemplateMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Masters_1 As MasterEntriesEnum, _
                                     ByVal Ds_UserTypeMst As DataSet, _
                                     ByVal UserCode_1 As String, _
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_InsertDocTemplateMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_UserTypeMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Copy Template"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Proc_Copy_Template(ByVal Ds_TemplateMst As DataSet, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Proc_Copy_Template = objMaster.Proc_Copy_Template(Ds_TemplateMst, objDtLogic, eStr_Retu)
    End Function
#End Region

#Region "Save_MedExScreeningDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_MedExScreeningDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_MedExScreeningDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef TranNo_Retu As String, _
                                                          ByRef Is_Transaction As Boolean, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_MedExScreeningDtl = objMaster.Save_MedExScreeningDtl(Choice_1, Ds_MedExScreeningDtl, objDtLogic, UserCode_1, TranNo_Retu, Is_Transaction, eStr_Retu)
    End Function
#End Region

#Region "Save_MedExInfoDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_MedExInfoHdrDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_MedExScreeningDtl As DataSet, _
                                                          ByVal SaveDefaultValue As Boolean, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef TranNo_Retu As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_MedExInfoHdrDtl = objMaster.Save_MedExInfoHdrDtl(Choice_1, Ds_MedExScreeningDtl, SaveDefaultValue, objDtLogic, UserCode_1, TranNo_Retu, eStr_Retu)
    End Function
#End Region

#Region "Save_MedExSubGroupMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_MedExSubGroupMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                         ByVal Masters_1 As MasterEntriesEnum, _
                                         ByVal Ds_MedExSubGroupMst As DataSet, _
                                         ByVal UserCode_1 As String, _
                                         ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_MedExSubGroupMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_MedExSubGroupMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_InsertMedExInfoHdr"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertMedExInfoHdr(ByVal Choice As DataObjOpenSaveModeEnum, _
                                                    ByVal Ds_SubjectMedExHdr As Data.DataSet, _
                                                    ByVal UserCode_1 As String, _
                                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_InsertMedExInfoHdr = objMaster.Save_MedExInfoHdr(Choice, Ds_SubjectMedExHdr, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_InsertActivityGroupMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertActivityGroupMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Masters_1 As MasterEntriesEnum, _
                                     ByVal Ds_UserTypeMst As DataSet, _
                                     ByVal UserCode_1 As String, _
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_InsertActivityGroupMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_UserTypeMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_WorkSpaceNodeDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_WorkSpaceNodeDetail(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_WorkSpaceNodeDetail As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_WorkSpaceNodeDetail = objMaster.Save_WorkSpaceNodeDetail(Choice_1, Ds_WorkSpaceNodeDetail, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_InsertWorkspaceLeafNode"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertWorkspaceLeafNode(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Ds_WorkspaceNodeDetail As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_InsertWorkspaceLeafNode = objMaster.Save_InsertWorkspaceLeafNode(Choice_1, Ds_WorkspaceNodeDetail, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_InsertWorkspaceNodeBefore"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertWorkspaceNodeBefore(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Ds_WorkspaceNodeDetail As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_InsertWorkspaceNodeBefore = objMaster.Save_InsertWorkspaceNodeBefore(Choice_1, Ds_WorkspaceNodeDetail, UserCode_1, eStr_Retu)

    End Function
#End Region

#Region "Save_InsertWorkspaceNodeAfter"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertWorkspaceNodeAfter(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds_WorkspaceNodeDetail As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_InsertWorkspaceNodeAfter = objMaster.Save_InsertWorkspaceNodeAfter(Choice_1, Ds_WorkspaceNodeDetail, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_MedExInfoHdrQC"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_MedExInfoHdrQC(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_MedExScreeningDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_MedExInfoHdrQC = objMaster.Save_MedExInfoHdrQC(Choice_1, Ds_MedExScreeningDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Edit_HousingDetail"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Edit_HousingDetail(ByVal Choice As DataObjOpenSaveModeEnum, _
                                        ByVal Ds_WorkSpaceNodeDetail As Data.DataSet, _
                                        ByVal UserCode_1 As String, _
                                        ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Edit_HousingDetail = objMaster.Edit_HousingDetail(Choice, Ds_WorkSpaceNodeDetail, objDtLogic, UserCode_1, eStr_Retu)
    End Function

#End Region

    '*******For FFR

#Region "Save_InsertSTP"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertSTP(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                          ByVal Ds_STPMst As DataSet, _
                                          ByVal UserCode_1 As String, _
                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_InsertSTP = objMaster.Save_InsertSTP(Choice_1, Ds_STPMst, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_SubmitMTP"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SubmitMTP(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                ByVal Ds_MTP As Data.DataSet, _
                                ByVal UserCode_1 As String, _
                                ByRef eStr_Retu As String) As Boolean

        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_SubmitMTP = objMaster.Save_SubmitMTP(Choice_1, Ds_MTP, objDtLogic, UserCode_1, eStr_Retu)

    End Function
#End Region

#Region "Save_MTPMstDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_MTPMstDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                ByVal Ds_MTP As Data.DataSet, _
                                ByVal UserCode_1 As String, _
                                ByRef eStr_Retu As String) As Boolean

        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_MTPMstDtl = objMaster.Save_MTPMstDtl(Choice_1, Ds_MTP, objDtLogic, UserCode_1, eStr_Retu)

    End Function
#End Region

#Region "Save_DWRHdrDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_DWRHdrDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                ByVal Ds_DWR As Data.DataSet, _
                                ByVal UserCode_1 As String, _
                                ByRef DWRNo As String, _
                                ByRef eStr_Retu As String) As Boolean

        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_DWRHdrDtl = objMaster.Save_DWRHdrDtl(Choice_1, Ds_DWR, objDtLogic, UserCode_1, DWRNo, eStr_Retu)

    End Function
#End Region

#Region "Save_ParameterList"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_ParameterList(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                ByVal Ds_ParameterList As Data.DataSet, _
                                ByVal UserCode_1 As String, _
                                ByRef eStr_Retu As String) As Boolean

        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_ParameterList = objMaster.Save_ParameterList(Choice_1, Ds_ParameterList, objDtLogic, UserCode_1, eStr_Retu)

    End Function
#End Region

    '===Created For:Archive Module====
    '===Created By:Vikas shah=========

#Region "insert_workspace"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function insert_workspace(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                      ByVal Ds_WorkspaceStatusDetail As DataSet, _
                                      ByVal UserCode_1 As String, _
                                      ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim Tbl_WorkspaceStatusDetail As New DataTable
        Tbl_WorkspaceStatusDetail = Ds_WorkspaceStatusDetail.Tables(0)
        insert_workspace = objMaster.insert_workspace(Choice_1, Ds_WorkspaceStatusDetail, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_TransferDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_TransferDetail(ByVal Choice As DataObjOpenSaveModeEnum, _
                               ByVal Dt_TransferDtl As DataTable, _
                               ByVal SaveDefaultValue As Boolean, _
                               ByVal UserCode_1 As String, _
                               ByRef IsSuccess As String, _
                               ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim Tbl_WorkspaceStatusDetail As New DataTable
        Dim objDtLogic As New ClsDataLogic_New
        'Tbl_WorkspaceStatusDetail = Ds_WorkspaceStatusDetail.Tables(0)
        Save_TransferDetail = objMaster.Save_TransferDetail(Choice, Dt_TransferDtl, True, objDtLogic, UserCode_1, IsSuccess, eStr_Retu)
    End Function
#End Region

#Region "Save_ArchiveDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_ArchiveDetail(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                         ByVal Ds_ArchieveDtl As DataSet, _
                                         ByVal UserCode_1 As String, _
                                         ByRef eStr_Retu As String) As Boolean

        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_ArchiveDetail = objMaster.Save_ArchiveDetail(Choice_1, Ds_ArchieveDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "proc_TransferLabRptForArchive"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function proc_TransferLabRptForArchive(ByVal Choice As DataObjOpenSaveModeEnum, _
                               ByVal Dt_TransferDtl As DataTable, _
                               ByVal SaveDefaultValue As Boolean, _
                               ByVal UserCode_1 As String, _
                               ByRef SuccessLabRpt As Integer, _
                               ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim Tbl_WorkspaceStatusDetail As New DataTable
        Dim objDtLogic As New ClsDataLogic_New
        'Tbl_WorkspaceStatusDetail = Ds_WorkspaceStatusDetail.Tables(0)
        proc_TransferLabRptForArchive = objMaster.proc_TransferLabRptForArchive(Choice, Dt_TransferDtl, True, objDtLogic, UserCode_1, SuccessLabRpt, eStr_Retu)
    End Function
#End Region

    '==================================

    '==================================


#Region "Save_SubjectMasterQC"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SubjectMasterQC(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_MedExScreeningDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_SubjectMasterQC = objMaster.Save_SubjectMasterQC(Choice_1, Ds_MedExScreeningDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_MedexScreeningHdrQC"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_MedexScreeningHdrQC(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_MedExScreeningDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_MedexScreeningHdrQC = objMaster.Save_MedexScreeningHdrQC(Choice_1, Ds_MedExScreeningDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_TemplateDefaultWorkFlowUserDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_TemplateDefaultWorkFlowUserDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_MedExScreeningDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_TemplateDefaultWorkFlowUserDtl = objMaster.Save_TemplateDefaultWorkflowUserDtl(Choice_1, Ds_MedExScreeningDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_WorkspaceDefaultWorkFlowUserDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_WorkspaceDefaultWorkFlowUserDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_MedExScreeningDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_WorkspaceDefaultWorkFlowUserDtl = objMaster.Save_WorkspaceDefaultWorkflowUserDtl(Choice_1, Ds_MedExScreeningDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Proc_CopyProjects"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Proc_CopyProjects(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                         ByVal Ds_WrokspaceMst As DataSet, _
                                         ByVal UserCode_1 As String, _
                                         ByRef RequestId As String, _
                                         ByRef Retu_WorkspaceId As String, _
                                         ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Proc_CopyProjects = objMaster.Proc_CopyProjects(Choice_1, Ds_WrokspaceMst, UserCode_1, RequestId, Retu_WorkspaceId, eStr_Retu)
    End Function
#End Region

#Region "Save_ProjectgroupMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_ProjectgroupMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Masters_1 As MasterEntriesEnum, _
                                     ByVal Ds_ProjectgroupMst As DataSet, _
                                     ByVal UserCode_1 As String, _
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_ProjectgroupMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_ProjectgroupMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_ProjectGroupWorkspaceMatrix"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_ProjectGroupWorkspaceMatrix(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Masters_1 As MasterEntriesEnum, _
                                     ByVal Ds_ProjectgroupWorkspaceMatrix As DataSet, _
                                     ByVal UserCode_1 As String, _
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_ProjectGroupWorkspaceMatrix = objMaster.Save_Masters(Choice_1, Masters_1, Ds_ProjectgroupWorkspaceMatrix, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_InsertAgeGroupMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertAgeGroupMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Masters_1 As MasterEntriesEnum, _
                                     ByVal Ds_AgeGroupMst As DataSet, _
                                     ByVal UserCode_1 As String, _
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_InsertAgeGroupMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_AgeGroupMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_SampleTypeDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SampleTypeDetail(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_MedExScreeningDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_SampleTypeDetail = objMaster.Save_SampleTypeDetail(Choice_1, Ds_MedExScreeningDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region
    '-----------------------------------------------
#Region "Save_InsertMedExRangeMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertMedExRangeMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Masters_1 As MasterEntriesEnum, _
                                     ByVal Ds_MedExRangeMst As DataSet, _
                                     ByVal UserCode_1 As String, _
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_InsertMedExRangeMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_MedExRangeMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_InsertSampleTypeMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertSampleTypeMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Masters_1 As MasterEntriesEnum, _
                                     ByVal Ds_MedExRangeMst As DataSet, _
                                     ByVal UserCode_1 As String, _
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_InsertSampleTypeMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_MedExRangeMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_InsertCurrencyMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertCurrencyMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Masters_1 As MasterEntriesEnum, _
                                     ByVal Ds_MedExRangeMst As DataSet, _
                                     ByVal UserCode_1 As String, _
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_InsertCurrencyMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_MedExRangeMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_InsertMedExCostMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertMedExCostMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Masters_1 As MasterEntriesEnum, _
                                     ByVal Ds_MedExRangeMst As DataSet, _
                                     ByVal UserCode_1 As String, _
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_InsertMedExCostMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_MedExRangeMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_InsertLabTypeMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertLabTypeMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Masters_1 As MasterEntriesEnum, _
                                     ByVal Ds_MedExRangeMst As DataSet, _
                                     ByVal UserCode_1 As String, _
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_InsertLabTypeMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_MedExRangeMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_LabMachineMedexMatrix"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_LabMachineMedexMatrix(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_MedExScreeningDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_LabMachineMedexMatrix = objMaster.Save_LabMachineMedexMatrix(Choice_1, Ds_MedExScreeningDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_SubjectRejectionDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SubjectRejectionDetail(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_SubjectRejectionDetail As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_SubjectRejectionDetail = objMaster.Save_SubjectRejectionDetail(Choice_1, Ds_SubjectRejectionDetail, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_SubjectScreeningRecordDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SubjectScreeningRecordDetail(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_SubjectScreeningRecordDetail As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_SubjectScreeningRecordDetail = objMaster.Save_SubjectScreeningRecordDetail(Choice_1, Ds_SubjectScreeningRecordDetail, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Insert_ChangePassword"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Insert_ChangePassword(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_ChangePassword As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Insert_ChangePassword = objMaster.Insert_ChangePassword(Choice_1, Ds_ChangePassword, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_ReasonMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_ReasonMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                      ByVal Masters_1 As MasterEntriesEnum, _
                                    ByVal Ds_StageMst As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster

        Save_ReasonMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_StageMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_DosingDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_DosingDetail(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_DosingDetail As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef BarCodes_Retu As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_DosingDetail = objMaster.Save_DosingDetail(Choice_1, Ds_DosingDetail, objDtLogic, UserCode_1, BarCodes_Retu, eStr_Retu)
    End Function
#End Region

#Region "Save_RandomizationDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_RandomizationDetail(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_DosingDetail As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_RandomizationDetail = objMaster.Save_RandomizationDetail(Choice_1, Ds_DosingDetail, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

    '**********************
    '************************************************************************
    ' Added By Vishal Astik
    '************************************************************************

#Region " InsertActivityMst "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertActivityMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Masters_1 As MasterEntriesEnum, _
                                     ByVal Ds_OperationMst As DataSet, _
                                     ByVal UserCode_1 As String, _
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_InsertActivityMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_OperationMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_InsertworkspaceSubjectComment"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertworkspaceSubjectComment(ByVal Choice As DataObjOpenSaveModeEnum, _
                                        ByVal Ds_WorkspaceSubjectComment As DataSet, _
                                        ByVal vWorkSpaceSubjectCommentId As String, _
                                        ByVal UserCode_1 As String, _
                                        ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_InsertworkspaceSubjectComment = objMaster.Save_InsertworkspaceSubjectComment(Choice, Ds_WorkspaceSubjectComment, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region " InsertTemplateTypeMst "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_TemplateTypeMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Masters_1 As MasterEntriesEnum, _
                                     ByVal Ds_TemplateTypeMst As DataSet, _
                                     ByVal UserCode_1 As String, _
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_TemplateTypeMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_TemplateTypeMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_InsertHolidayMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertHolidayMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal ds_HolidayMst As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objClsDataLogic As New ClsDataLogic_New
        Save_InsertHolidayMst = objMaster.Save_Masters(Choice_1, MasterEntriesEnum.MasterEntriesEnum_HolidayMst, _
                                                 ds_HolidayMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region " SQLBulkCopy "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function BulkCopy(ByVal TableName As String, _
                                                          ByVal dsDataToSave As DataSet, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objClsDataLogic As New ClsDataLogic_New
        BulkCopy = objMaster.BulkCopy(TableName, dsDataToSave, eStr_Retu)
        'Save_RouteOfAdminMst = objMaster.Save_RouteOfAdminMst(Choice_1, ds_RouteOfAdmin, objClsDataLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_SubjectLanguageMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SubjectLanguageMst(ByVal Choice As DataObjOpenSaveModeEnum, _
                                                    ByVal Ds_SubjectLanguageMst As Data.DataSet, _
                                                    ByVal UserCode_1 As String, _
                                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_SubjectLanguageMst = objMaster.Save_SubjectLanguageMst(Choice, Ds_SubjectLanguageMst, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

    '************************************************************************
    'Added By: Mihir Oza
    '************************************************************************

#Region "Save_SubjectMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SubjectMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds_SubjectMst As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef SubjectId As String, _
                                    ByRef eStr_Retu As String, _
                                    ByRef RandomizationNo As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_SubjectMst = objMaster.Save_SubjectMst(Choice_1, Ds_SubjectMst, objDtLogic, UserCode_1, SubjectId, eStr_Retu, RandomizationNo)
    End Function
#End Region

#Region "Save_ImportSubjectMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Save_ImportSubjectMst(ByVal Ds_SubjectMst As DataSet,
                                    ByVal UserCode_1 As String,
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_ImportSubjectMst = objMaster.Save_ImportSubjectMst(Ds_SubjectMst, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "save_subjectFemaleDetails"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function save_subjectFemaleDetails(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                      ByVal Masters_1 As MasterEntriesEnum, _
                                    ByVal Ds_SubjectfemaleMst As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster

        save_subjectFemaleDetails = objMaster.Save_Masters(Choice_1, Masters_1, Ds_SubjectfemaleMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_DrugAnalytesMatrix"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_DrugAnalytesMatrix(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_DrugAnalytesMatrix As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_DrugAnalytesMatrix = objMaster.Save_DrugAnalytesMatrix(Choice_1, Ds_DrugAnalytesMatrix, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region


#Region "SetProjectMatrix"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SetProjectMatrix(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                         ByVal Masters_1 As MasterEntriesEnum, _
                                         ByVal Ds_MedExMst As DataSet, _
                                         ByVal UserCode_1 As String, _
                                         ByRef eStr_Retu As String) As Boolean


        Dim objMaster As New ClsMaster
        Save_SetProjectMatrix = objMaster.Save_Masters(Choice_1, Masters_1, Ds_MedExMst, UserCode_1, eStr_Retu)

    End Function


#End Region


#Region "Save_MedExMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_MedExMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                         ByVal Masters_1 As MasterEntriesEnum, _
                                         ByVal Ds_MedExMst As DataSet, _
                                         ByVal UserCode_1 As String, _
                                         ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_MedExMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_MedExMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_CountryMaster"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_CountryMaster(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                         ByVal Masters_1 As MasterEntriesEnum, _
                                         ByVal Ds_CountryMaster As DataSet, _
                                         ByVal UserCode_1 As String, _
                                         ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_CountryMaster = objMaster.Save_Masters(Choice_1, Masters_1, Ds_CountryMaster, UserCode_1, eStr_Retu)
    End Function
#End Region
#Region "Save_SpecialityMaster"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SpecialityMaster(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                         ByVal Masters_1 As MasterEntriesEnum, _
                                         ByVal Ds_CountryMaster As DataSet, _
                                         ByVal UserCode_1 As String, _
                                         ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_SpecialityMaster = objMaster.Save_Masters(Choice_1, Masters_1, Ds_CountryMaster, UserCode_1, eStr_Retu)
    End Function
#End Region


#Region "Save_ChildWorkSpaceMstSite"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_ChildWorkSpaceMstSite(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                         ByVal Masters_1 As MasterEntriesEnum, _
                                         ByVal Ds_CountryMaster As DataSet, _
                                         ByVal UserCode_1 As String, _
                                         ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_ChildWorkSpaceMstSite = objMaster.Save_Masters(Choice_1, Masters_1, Ds_CountryMaster, UserCode_1, eStr_Retu)
    End Function
#End Region

    '************************************************************************
    'Added By: Bhargav
    '************************************************************************

#Region "Save_CRFBunchData"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Save_CRFBunchData(ByVal Ds_SubjectMst As DataSet,
                                    ByVal UserCode_1 As String,
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_CRFBunchData = objMaster.Save_CRFBunchData(Ds_SubjectMst, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_CRFSubBunchData"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Save_CRFSubBunchData(ByVal Ds_SubjectMst As DataSet,
                                    ByVal UserCode_1 As String,
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_CRFSubBunchData = objMaster.Save_CRFSubBunchData(Ds_SubjectMst, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region


    '************************************************************************
    'Added By: Satyam
    '************************************************************************

#Region "InsertWorkspaceSubjectMst "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertWorkspaceSubjectMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Masters_1 As MasterEntriesEnum, _
                                     ByVal Ds_WorkspaceSubjectMst As DataSet, _
                                     ByVal UserCode_1 As String, _
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_InsertWorkspaceSubjectMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_WorkspaceSubjectMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_MedExGroupMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_MedExGroupMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                         ByVal Masters_1 As MasterEntriesEnum, _
                                         ByVal Ds_MedExGroupMst As DataSet, _
                                         ByVal UserCode_1 As String, _
                                         ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_MedExGroupMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_MedExGroupMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_MedExTemplateDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_MedExTemplateDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                         ByVal Masters_1 As MasterEntriesEnum, _
                                         ByVal Ds_MedExTemplateDtl As DataSet, _
                                         ByVal UserCode_1 As String, _
                                         ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_MedExTemplateDtl = objMaster.Save_Masters(Choice_1, Masters_1, Ds_MedExTemplateDtl, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_MedExWorkspaceDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_MedExWorkspaceDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                      ByVal Masters_1 As MasterEntriesEnum, _
                                    ByVal Ds_MedExWorkspaceMst As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster

        Save_MedExWorkspaceDtl = objMaster.Save_Masters(Choice_1, Masters_1, Ds_MedExWorkspaceMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_DocTypeTemplateMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_DocTypeTemplateMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                         ByVal Ds_DocTypeTemplateMst As DataSet, _
                                         ByVal UserCode_1 As String, _
                                         ByRef eStr_Retu As String, _
                                        ByRef DocId_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_DocTypeTemplateMst = objMaster.Save_DocTypeTemplateMst(Choice_1, Ds_DocTypeTemplateMst, objDtLogic, UserCode_1, eStr_Retu, DocId_Retu)
    End Function
#End Region

#Region "Save_SubjectBlobDetails"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SubjectBlobDetails(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_WorkSpaceNodeDetail As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_SubjectBlobDetails = objMaster.Save_SubjectBlobDetails(Choice_1, Ds_WorkSpaceNodeDetail, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_DocTemplateWorkspaceDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_DocTemplateWorkspaceDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                         ByVal Ds_DocTemplateWorkspaceDtl As DataSet, _
                                         ByVal UserCode_1 As String, _
                                         ByRef eStr_Retu As String) As Boolean

        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_DocTemplateWorkspaceDtl = objMaster.Save_DocTemplateWorkspaceDtl(Choice_1, Ds_DocTemplateWorkspaceDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

    '************************************************************************
    'Added By: Chandresh Vanker on 02-02-2009
    '************************************************************************

#Region "Save_SampleDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SampleDetail(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Ds_SampleDetail As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef nSampleId_Retu As String, _
                                  ByRef vSampleId_Retu As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_SampleDetail = objMaster.Save_SampleDetail(Choice_1, Ds_SampleDetail, objDtLogic, UserCode_1, nSampleId_Retu, vSampleId_Retu, eStr_Retu)
    End Function
#End Region

#Region "Save_SampleMedEXDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SampleMedEXDetail(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                              ByVal Ds_MachineSampleWorkListDtl As DataSet, _
                              ByVal UserCode_1 As String, _
                              ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_SampleMedEXDetail = objMaster.Save_SampleMedExDetail(Choice_1, Ds_MachineSampleWorkListDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_SampleTypeSendReceiveDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SampleTypeSendReceiveDetail(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                              ByVal Ds_SampleSendReceiveDetails As DataSet, _
                              ByVal UserCode_1 As String, _
                              ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_SampleTypeSendReceiveDetail = objMaster.Save_SampleTypeSendReceiveDetail(Choice_1, Ds_SampleSendReceiveDetails, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '========Added on 05-02-2009

#Region "Save_OtherExpHdr"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_OtherExpHdr(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Ds_OtherExpHdr As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_OtherExpHdr = objMaster.Save_OtherExpHdr(Choice_1, Ds_OtherExpHdr, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '=====================Added on 11-02-2009

#Region "Save_OtherExpDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_OtherExpDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                              ByVal Ds_OtherExpDtl As DataSet, _
                              ByVal UserCode_1 As String, _
                              ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_OtherExpDtl = objMaster.Save_OtherExpDtl(Choice_1, Ds_OtherExpDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_ScopeMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_ScopeMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Masters_1 As MasterEntriesEnum, _
                                  ByVal Ds_ScopeMst As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_ScopeMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_ScopeMst, UserCode_1, eStr_Retu)
    End Function
#End Region '========================Added on 25-02-2009

#Region "Save_UserLoginDetails"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_UserLoginDetails(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Ds_UserLoginDetails As DataSet, _
                                     ByVal UserCode_1 As String, _
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_UserLoginDetails = objMaster.Save_UserLoginDetails(Choice_1, Ds_UserLoginDetails, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added on 06-05-2009===================For PassWord Policy

#Region "Save_UOMMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_UOMMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Masters_1 As MasterEntriesEnum, _
                                  ByVal Ds_UOMMst As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_UOMMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_UOMMst, UserCode_1, eStr_Retu)
    End Function
#End Region '========================Added on 03-06-2009

#Region "Save_SubjectProofDetails"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SubjectProofDetails(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Ds_SubjectProofDetails As DataSet, _
                                     ByVal UserCode_1 As String, _
                                     ByRef Retu_TranNo As String, _
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_SubjectProofDetails = objMaster.Save_SubjectProofDetails(Choice_1, Ds_SubjectProofDetails, UserCode_1, Retu_TranNo, eStr_Retu)
    End Function
#End Region '================Added on 02-07-2009===================

#Region "Save_CollectionDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_CollectionDetail(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Ds_CollectionDetail As DataSet, _
                                     ByVal UserCode_1 As String, _
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_CollectionDetail = objMaster.Save_CollectionDetail(Choice_1, Ds_CollectionDetail, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added on 31-07-2009===================

#Region "Save_SubjectWorkspaceAssignment"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SubjectWorkspaceAssignment(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Ds_SubjectWorkspaceAssignment As DataSet, _
                                     ByVal UserCode_1 As String, _
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_SubjectWorkspaceAssignment = objMaster.Save_SubjectWorkspaceAssignment(Choice_1, Ds_SubjectWorkspaceAssignment, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added on 11-08-2009===================

#Region "Save_SubjectLabReportDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SubjectLabReportDetail(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Ds_SubjectWorkspaceAssignment As DataSet, _
                                     ByVal UserCode_1 As String, _
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_SubjectLabReportDetail = objMaster.Save_SubjectLabReportDetail(Choice_1, Ds_SubjectWorkspaceAssignment, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added on 27-08-2009===================

#Region "Save_UserLoginFailureDetails"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_UserLoginFailureDetails(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Ds_UserLoginFailureDetails As DataSet, _
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_UserLoginFailureDetails = objMaster.Save_UserLoginFailureDetails(Choice_1, Ds_UserLoginFailureDetails, eStr_Retu)
    End Function
#End Region '================Added on 16-09-2009===================

#Region "Save_CRFHdrForCTM"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_CRFHdrForCTM(ByVal Choice As DataObjOpenSaveModeEnum, _
                                                    ByVal Ds_CRFHdr As Data.DataSet, _
                                                    ByVal UserCode_1 As String, _
                                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_CRFHdrForCTM = objMaster.Save_CRFHdrForCTM(Choice, Ds_CRFHdr, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added on 19-12-2009===================

#Region "Save_CRFDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_CRFDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_CRFDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_CRFDtl = objMaster.Save_CRFDtl(Choice_1, Ds_CRFDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added on 19-12-2009===================

#Region "Save_CRFSubDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_CRFSubDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_CRFSubDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_CRFSubDtl = objMaster.Save_CRFSubDtl(Choice_1, Ds_CRFSubDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added on 19-12-2009===================

#Region "Save_CRFWorkFlowDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_CRFWorkFlowDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_CRFWorkFlowDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_CRFWorkFlowDtl = objMaster.Save_CRFWorkFlowDtl(Choice_1, Ds_CRFWorkFlowDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added on 19-12-2009===================

#Region "Save_CRFHdrDtlSubDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_CRFHdrDtlSubDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByRef Ds_CRFWorkFlowDtl As DataSet, _
                                                          ByVal SaveDefaultValue As Boolean, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_CRFHdrDtlSubDtl = objMaster.Save_CRFHdrDtlSubDtl(Choice_1, Ds_CRFWorkFlowDtl, SaveDefaultValue, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added on 21-12-2009===================

#Region "Save_DCFMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_DCFMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_DCFMst As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_DCFMst = objMaster.Save_DCFMst(Choice_1, Ds_DCFMst, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added on 25-12-2009===================

#Region "Save_MedExEditChecks"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_MedExEditChecks(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_MedExEditChecks As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_MedExEditChecks = objMaster.Save_MedExEditChecks(Choice_1, Ds_MedExEditChecks, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added on 05-04-2010===================

#Region "Save_EditChecksHdrDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_EditChecksHdrDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_EditChecksHdrDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_EditChecksHdrDtl = objMaster.Save_EditChecksHdrDtl(Choice_1, Ds_EditChecksHdrDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added on 22-04-2010===================

#Region "Save_CRFLockDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_CRFLockDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_CRFLockDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_CRFLockDtl = objMaster.Save_CRFLockDtl(Choice_1, Ds_CRFLockDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added on 25-04-2010===================

#Region "Save_EditChecksDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_EditChecksDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_EditChecksDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_EditChecksDtl = objMaster.Save_EditChecksDtl(Choice_1, Ds_EditChecksDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added on 29-04-2010===================

#Region "Save_OldProjects"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_OldProjects(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_OldProjects As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_OldProjects = objMaster.Save_OldProjects(Choice_1, Ds_OldProjects, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added on 25-08-2010===================

#Region "Save_ExpenseTypeMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_ExpenseTypeMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                      ByVal Masters_1 As MasterEntriesEnum, _
                                    ByVal Ds_ExpenseTypeMst As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster

        Save_ExpenseTypeMst = (objMaster.Save_Masters(Choice_1, Masters_1, Ds_ExpenseTypeMst, UserCode_1, eStr_Retu))
    End Function
#End Region

#Region "Save_ExpenseMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_ExpenseMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                        ByVal Ds_ExpenseMst As DataSet, _
                                                        ByVal UserCode_1 As String, _
                                                        ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_ExpenseMst = objMaster.Save_ExpenseMst(Choice_1, Ds_ExpenseMst, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_ExpenseDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_ExpenseDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                      ByVal Masters_1 As MasterEntriesEnum, _
                                    ByVal Ds_LocationMst As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster

        Save_ExpenseDtl = (objMaster.Save_Masters(Choice_1, Masters_1, Ds_LocationMst, UserCode_1, eStr_Retu))
    End Function
#End Region

#Region "Save_DocumentReleaseDetails"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_DocumentReleaseDetail(ByVal Choice As DataObjOpenSaveModeEnum, _
                                    ByVal Ds_DocumentReleaseDetail As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_DocumentReleaseDetail = (objMaster.Save_DocumentReleaseDetail(Choice, Ds_DocumentReleaseDetail, objDtLogic, UserCode_1, eStr_Retu))
    End Function
#End Region '================Added on 24-03-2011 on request of Vishal===================

#Region "Save_ReferenceTableDefinitions "
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_ReferenceTableDefinitions(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                         ByVal Ds_Dictionary As DataSet, _
                                         ByVal UserCode_1 As String, _
                                         ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_ReferenceTableDefinitions = objMaster.Save_ReferenceTableDefinitions(Choice_1, Ds_Dictionary, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

    '*************************************
    'Added By: Deepak Singh 
    '*************************************

#Region "Save_PKSampleDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_PKSampleDetail(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Ds_PKSampleDetail As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef nPKSampleId_Retn As String, _
                                  ByRef vPKSampleId_Retu As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_PKSampleDetail = objMaster.Save_PKSampleDetail(Choice_1, Ds_PKSampleDetail, objDtLogic, UserCode_1, nPKSampleId_Retn, vPKSampleId_Retu, eStr_Retu)
    End Function
#End Region '================Added on 05-04-2010====

#Region "Save_PDSampleDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_PDSampleDetail(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Ds_PKSampleDetail As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_PDSampleDetail = objMaster.Save_PDSampleDetail(Choice_1, Ds_PKSampleDetail, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added on 27-10-2012 Created  By Megha Shah====

#Region "Save_LabRptLockUnlockDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_LabRptLockUnlockDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Ds_LabRptLockUnlockDtl As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_LabRptLockUnlockDtl = objMaster.Save_LabRptLockUnlockDtl(Choice_1, Ds_LabRptLockUnlockDtl, UserCode_1, eStr_Retu)
    End Function
#End Region '

#Region "Save_ScreeningLockDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_ScreeeningLockDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Ds_ScreeningLockDtl As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_ScreeeningLockDtl = objMaster.Save_ScreeeningLockDtl(Choice_1, Ds_ScreeningLockDtl, UserCode_1, eStr_Retu)
    End Function
#End Region '=====added on 05-July-2010=====

#Region "Save_ReportTypeMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_ReportTypeMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Ds_ReportTypeMst As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_ReportTypeMst = objMaster.Save_ReportTypeMst(Choice_1, Ds_ReportTypeMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_WorkspaceProtocoldetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_WorkspaceProtocoldetail(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                         ByVal Ds_WorkspaceProtocoldetail As DataSet, _
                                         ByVal UserCode_1 As String, _
                                         ByRef RequestId As String, _
                                         ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        'Save_workspacemst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_roleOperationMatrix, UserCode_1, eStr_Retu)
        Save_WorkspaceProtocoldetail = objMaster.Save_WorkspaceProtocoldetail(Choice_1, Ds_WorkspaceProtocoldetail, UserCode_1, _
                                        RequestId, eStr_Retu)
    End Function
#End Region '=====added on 03-Mar-2011=====

    '=====================================
    ' For e-CTD  
    '=====================================

#Region "Save_WorkspaceNodeDtlwithDoc"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_WorkspaceNodeDtlwithDoc(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds_WorkspaceNodeDetail As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String, ByRef NodeId As Integer, _
                                    ByRef iTranNo As Integer, _
                                    ByRef FilePath As String, _
                                    ByRef FolderPath As String, _
                                    ByVal Mode As String, _
                                    ByVal Mode_WorkspaceNodeDetail As String, _
                                    ByVal FormModified As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_WorkspaceNodeDtlwithDoc = objMaster.Save_WorkspaceNodeDtlwithDoc(Choice_1, Ds_WorkspaceNodeDetail, UserCode_1, eStr_Retu, NodeId, iTranNo, FilePath, FolderPath, Mode, Mode_WorkspaceNodeDetail, FormModified)
    End Function
#End Region

#Region "Save_WorkspaceNodeHistory_ECTD"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_WorkspaceNodeHistory_ECTD(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds_WorkspaceNodeDetail As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String, _
                                    ByRef iTranNo As Integer) As Boolean
        Dim objMaster As New ClsMaster
        Save_WorkspaceNodeHistory_ECTD = objMaster.Save_WorkspaceNodeHistory_ECTD(Choice_1, Ds_WorkspaceNodeDetail, UserCode_1, eStr_Retu, iTranNo)
    End Function
#End Region

#Region "Save_WorkspaceCMSMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_WorkspaceCMSMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Ds_ReportTypeMst As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_WorkspaceCMSMst = objMaster.Save_WorkspaceCMSMst(Choice_1, Ds_ReportTypeMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_submissioninfoEU14MSt"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_submissioninfoEU14MSt(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds_StageMst As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster

        Save_submissioninfoEU14MSt = objMaster.Save_submissioninfoEU14MSt(Choice_1, Ds_StageMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_submissioninfoUSMSt"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_submissioninfoUSMSt(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds_StageMst As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster

        Save_submissioninfoUSMSt = objMaster.Save_submissioninfoUSMSt(Choice_1, Ds_StageMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_submissioninfoCAMSt"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_submissioninfoCAMSt(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds_StageMst As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster

        Save_submissioninfoCAMSt = objMaster.Save_submissioninfoCAMSt(Choice_1, Ds_StageMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_ProcCopyPasteWorkspace_ECTD"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_ProcCopyPasteWorkspace_ECTD(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds_WorkspaceNodeDetail As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String, _
                                    ByRef iMaxNodeId As Integer) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_ProcCopyPasteWorkspace_ECTD = objMaster.Save_ProcCopyPasteWorkspace_ECTD(Choice_1, Ds_WorkspaceNodeDetail, objDtLogic, UserCode_1, eStr_Retu, iMaxNodeId)
    End Function
#End Region

#Region "Insert_Workspacenodeattrhistory__ForEctd"    '' Added On 14-11-2011
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Insert_Workspacenodeattrhistory__ForEctd(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds_Workspacenodeattrhistory As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Insert_Workspacenodeattrhistory__ForEctd = objMaster.Insert_Workspacenodeattrhistory__ForEctd(Choice_1, Ds_Workspacenodeattrhistory, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Insert_StfStudyIdentifierMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Insert_StfStudyIdentifierMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal DsStfStudyIdentifierMst As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String, _
                                    ByRef iTagSequenceId As String) As Boolean
        Dim objMaster As New ClsMaster

        Insert_StfStudyIdentifierMst = objMaster.Insert_StfStudyIdentifierMst(Choice_1, DsStfStudyIdentifierMst, UserCode_1, eStr_Retu, iTagSequenceId)
    End Function
#End Region

#Region "Save_workspacemst_eCTD"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_WorkSpaceMst_eCTD(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                         ByVal Ds_roleOperationMatrix As DataSet, _
                                         ByVal UserCode_1 As String, _
                                         ByRef RequestId As String, _
                                         ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        'Save_workspacemst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_roleOperationMatrix, UserCode_1, eStr_Retu)
        Save_WorkSpaceMst_eCTD = objMaster.Save_WorkSpaceMst_eCTD(Choice_1, Ds_roleOperationMatrix, UserCode_1, _
                                        RequestId, eStr_Retu)
    End Function
#End Region
    '=====================================
    'Created By : Bharat Patel
    'Created Date : 28-Nov-2011
    'Reason : Insert the data into WorkspaceNodeDocHistory for the Source Doc
    '======================================
#Region "Save_WorkspaceNodeDocHistory."
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_WorkspaceNodeDocHistory(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal DsStfStudyIdentifierMst As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String, _
                                    ByRef iDocHistId As String) As Boolean
        Dim objMaster As New ClsMaster

        Save_WorkspaceNodeDocHistory = objMaster.Save_WorkspaceNodeDocHistory(Choice_1, DsStfStudyIdentifierMst, UserCode_1, eStr_Retu, iDocHistId)
    End Function
#End Region

#Region "Insert_WorkspaceNodeCommentHistory."
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Insert_WorkspaceNodeCommentHistory(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal DsStfStudyIdentifierMst As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String, _
                                    ByRef iTranNo As String) As Boolean
        Dim objMaster As New ClsMaster

        Insert_WorkspaceNodeCommentHistory = objMaster.Insert_WorkspaceNodeCommentHistory(Choice_1, DsStfStudyIdentifierMst, UserCode_1, eStr_Retu, iTranNo)
    End Function
#End Region

#Region "Insert_DocReleaseTrack"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Insert_DocReleaseTrack(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                         ByVal ds_DocReleaseTrack As DataSet, _
                                         ByVal UserCode_1 As String, _
                                         ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Insert_DocReleaseTrack = objMaster.Insert_DocReleaseTrack(Choice_1, ds_DocReleaseTrack, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Insert_ReleaseDocIdTrack"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Insert_ReleaseDocIdTrack(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                         ByVal ds_DocReleaseTrack As DataSet, _
                                         ByVal UserCode_1 As String, _
                                         ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Insert_ReleaseDocIdTrack = objMaster.Insert_ReleaseDocIdTrack(Choice_1, ds_DocReleaseTrack, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Insert_ReleaseDocMgmt"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Insert_ReleaseDocMgmt(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                         ByVal ds_ReleaseDocMgmt As DataSet, _
                                         ByVal UserCode_1 As String, _
                                         ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Insert_ReleaseDocMgmt = objMaster.Insert_ReleaseDocMgmt(DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds_ReleaseDocMgmt, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Insert_SOPMailMgmt"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Insert_SOPMailMgmt(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                         ByVal ds_SOPMailMgmt As DataSet, _
                                         ByVal UserCode_1 As String, _
                                         ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Insert_SOPMailMgmt = objMaster.Insert_SOPMailMgmt(Choice_1, ds_SOPMailMgmt, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_WorkspaceNodeDtlWithHist_DocCentric"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_WorkspaceNodeDtlWithHist_DocCentric(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                         ByVal ds_DocReleaseTrack As DataSet, _
                                         ByVal UserCode_1 As String, _
                                         ByRef eStr_Retu As String, _
                                         ByVal iDocId As Integer, _
                                         ByVal Repetition As Integer, _
                                         ByVal vDocId As String, _
                                         ByVal BaseFolder As String, _
                                         ByRef vFilePaths As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_WorkspaceNodeDtlWithHist_DocCentric = objMaster.Save_WorkspaceNodeDtlWithHist_DocCentric(Choice_1, ds_DocReleaseTrack, UserCode_1, eStr_Retu, iDocId, Repetition, vDocId, BaseFolder, vFilePaths)
    End Function
#End Region
    '=====================================

    '======================================

#Region "Save_MedExFormulaMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_MedExFormulaMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_MedExFormulaMst As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_MedExFormulaMst = objMaster.Save_MedExFormulaMst(Choice_1, Ds_MedExFormulaMst, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region ' == For LIMS

#Region "Save_UserLoginHistory" '========added on 14-09-09 by Deepak Singh=====
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_UserLoginHistory(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Ds_UserLoginHistory As DataSet, _
                                     ByVal UserCode_1 As String, _
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_UserLoginHistory = objMaster.Save_UserLoginHistory(Choice_1, Ds_UserLoginHistory, UserCode_1, eStr_Retu)
    End Function

#End Region

#Region "Save_MedExWorkspaceTemplateDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_MedExWorkspaceTemplateDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                         ByVal Masters_1 As MasterEntriesEnum, _
                                         ByVal Ds_MedExWorkspaceTemplateDtl As DataSet, _
                                         ByVal UserCode_1 As String, _
                                         ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_MedExWorkspaceTemplateDtl = objMaster.Save_Masters(Choice_1, Masters_1, Ds_MedExWorkspaceTemplateDtl, UserCode_1, eStr_Retu)
    End Function
#End Region


#Region "Save_InvoiceDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InvoiceDetail(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                             ByVal Ds_InvoiceDetail As DataSet, _
                                             ByVal UserCode_1 As String, _
                                             ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_InvoiceDetail = objMaster.Save_InvoiceDetail(Choice_1, Ds_InvoiceDetail, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region ' == For LIMS

#Region "Save_SampleReportPrintingDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SampleReportPrintingDetail(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                             ByVal Ds_SampleReportPrintingDetail As DataSet, _
                                             ByVal UserCode_1 As String, _
                                             ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_SampleReportPrintingDetail = objMaster.Save_SampleReportPrintingDetail(Choice_1, Ds_SampleReportPrintingDetail, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region ' == For LIMS

#Region "Save_ReferralMaster"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_ReferralMaster(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Masters_1 As MasterEntriesEnum, _
                                  ByVal Ds_ReferralMaster As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_ReferralMaster = objMaster.Save_Masters(Choice_1, Masters_1, Ds_ReferralMaster, UserCode_1, eStr_Retu)
    End Function
#End Region ' == For LIMS

#Region "Save_PatientMaster"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_PatientMaster(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Masters_1 As MasterEntriesEnum, _
                                  ByVal Ds_PatientMaster As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_PatientMaster = objMaster.Save_Masters(Choice_1, Masters_1, Ds_PatientMaster, UserCode_1, eStr_Retu)
    End Function
#End Region ' == For LIMS

#Region "Save_SMSDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SMSDetail(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Masters_1 As MasterEntriesEnum, _
                                  ByVal Ds_SMSDetail As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_SMSDetail = objMaster.Save_Masters(Choice_1, Masters_1, Ds_SMSDetail, UserCode_1, eStr_Retu)
    End Function
#End Region ' == For LIMS

#Region "Save_DocumentUpload"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_DocumentUpload(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Masters_1 As MasterEntriesEnum, _
                                  ByVal Ds_PatientMaster As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_DocumentUpload = objMaster.Save_Masters(Choice_1, Masters_1, Ds_PatientMaster, UserCode_1, eStr_Retu)
    End Function
#End Region ' == For LIMS

#Region "Save_DiscountMedExTemplateMatrix"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_DiscountMedExTemplateMatrix(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Masters_1 As MasterEntriesEnum, _
                                  ByVal Ds_DiscountMedExTemplateMatrix As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_DiscountMedExTemplateMatrix = objMaster.Save_Masters(Choice_1, Masters_1, Ds_DiscountMedExTemplateMatrix, UserCode_1, eStr_Retu)
    End Function
#End Region ' == For LIMS

#Region "Save_SpecialityMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SpecialityMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Masters_1 As MasterEntriesEnum, _
                                  ByVal Ds_SpecialityMst As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_SpecialityMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_SpecialityMst, UserCode_1, eStr_Retu)
    End Function
#End Region ' == For LIMS

#Region "Save_CityMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_CityMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Masters_1 As MasterEntriesEnum, _
                                  ByVal Ds_CityMst As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_CityMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_CityMst, UserCode_1, eStr_Retu)
    End Function
#End Region ' == For LIMS 

#Region "Save_KnockOffDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_KnockOffDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Masters_1 As MasterEntriesEnum, _
                                  ByVal Ds_KnockOffDtl As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_KnockOffDtl = objMaster.Save_Masters(Choice_1, Masters_1, Ds_KnockOffDtl, UserCode_1, eStr_Retu)
    End Function
#End Region ' == For LIMS

#Region "Save_LabPatientMaster" 'Added By Ankit Shah
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_LabPatientMaster(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Masters_1 As MasterEntriesEnum, _
                                  ByVal Ds_LabPatientMaster As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_LabPatientMaster = objMaster.Save_Masters(Choice_1, Masters_1, Ds_LabPatientMaster, UserCode_1, eStr_Retu)
    End Function
#End Region ' == For LIMS

#Region "Save_SampleRemarkDtl" 'Added By Ankit Shah
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SampleRemarkDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Masters_1 As MasterEntriesEnum, _
                                  ByVal Ds_SampleRemarkDtl As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_SampleRemarkDtl = objMaster.Save_Masters(Choice_1, Masters_1, Ds_SampleRemarkDtl, UserCode_1, eStr_Retu)
    End Function
#End Region ' == For LIMS

#Region "Save_NarrativeMaster" 'Added By Ankit Shah
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_NarrativeMaster(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Masters_1 As MasterEntriesEnum, _
                                  ByVal Ds_NarrativeMaster As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_NarrativeMaster = objMaster.Save_Masters(Choice_1, Masters_1, Ds_NarrativeMaster, UserCode_1, eStr_Retu)
    End Function
#End Region ' == For LIMS

#Region "Save_EditSampleDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_EditSampleDetail(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Ds_SampleDetail As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef nSampleId_Retu As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_EditSampleDetail = objMaster.Save_EditSampleDetail(Choice_1, Ds_SampleDetail, objDtLogic, UserCode_1, nSampleId_Retu, eStr_Retu)
    End Function
#End Region ' == For LIMS

#Region "Save_SampleTemplateDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SampleTemplateDetail(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                              ByVal Ds_SampleTemplateDetail As DataSet, _
                              ByVal UserCode_1 As String, _
                              ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_SampleTemplateDetail = objMaster.Save_SampleTemplateDetail(Choice_1, Ds_SampleTemplateDetail, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region  ' == For LIMS

#Region "Save_TemplateRemarkDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_TemplateRemarkDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                              ByVal Masters_1 As MasterEntriesEnum, _
                              ByVal Ds_TemplateRemarkDtl As DataSet, _
                              ByVal UserCode_1 As String, _
                              ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_TemplateRemarkDtl = objMaster.Save_Masters(Choice_1, Masters_1, Ds_TemplateRemarkDtl, UserCode_1, eStr_Retu)
    End Function
#End Region  ' == For LIMS

#Region "Save_SampleCollecterMaster"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SampleCollecterMaster(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                              ByVal Masters_1 As MasterEntriesEnum, _
                              ByVal Ds_SampleCollecterMst As DataSet, _
                              ByVal UserCode_1 As String, _
                              ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_SampleCollecterMaster = objMaster.Save_Masters(Choice_1, Masters_1, Ds_SampleCollecterMst, UserCode_1, eStr_Retu)
    End Function
#End Region  ' == For LIMS

#Region "Save_HistoParameterDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_HistoParameterDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                              ByVal Masters_1 As MasterEntriesEnum, _
                              ByVal Ds_HistoParameter As DataSet, _
                              ByVal UserCode_1 As String, _
                              ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_HistoParameterDtl = objMaster.Save_Masters(Choice_1, Masters_1, Ds_HistoParameter, UserCode_1, eStr_Retu)
    End Function
#End Region  ' == For LIMS

#Region "Save_ParameterHdr"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_ParameterHdr(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                              ByVal Masters_1 As MasterEntriesEnum, _
                              ByVal Ds_ParameterHdr As DataSet, _
                              ByVal UserCode_1 As String, _
                              ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_ParameterHdr = objMaster.Save_Masters(Choice_1, Masters_1, Ds_ParameterHdr, UserCode_1, eStr_Retu)
    End Function
#End Region  ' == For LIMS

#Region "Save_ParameterDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_ParameterDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                              ByVal Masters_1 As MasterEntriesEnum, _
                              ByVal Ds_ParameterDtl As DataSet, _
                              ByVal UserCode_1 As String, _
                              ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_ParameterDtl = objMaster.Save_Masters(Choice_1, Masters_1, Ds_ParameterDtl, UserCode_1, eStr_Retu)
    End Function
#End Region  ' == For LIMS

#Region "Save_SampleParameterDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SampleParameterDetail(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                              ByVal Masters_1 As MasterEntriesEnum, _
                              ByVal Ds_SampleParameterDetail As DataSet, _
                              ByVal UserCode_1 As String, _
                              ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_SampleParameterDetail = objMaster.Save_Masters(Choice_1, Masters_1, Ds_SampleParameterDetail, UserCode_1, eStr_Retu)
    End Function
#End Region  ' == For LIMS

#Region "Save_RemarkGroupDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_RemarkGroupDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                              ByVal Masters_1 As MasterEntriesEnum, _
                              ByVal Ds_RemarkGroupDtl As DataSet, _
                              ByVal UserCode_1 As String, _
                              ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_RemarkGroupDtl = objMaster.Save_Masters(Choice_1, Masters_1, Ds_RemarkGroupDtl, UserCode_1, eStr_Retu)
    End Function
#End Region  ' == For LIMS

#Region "Save_MedexRemarkDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_MedexRemarkDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                              ByVal Masters_1 As MasterEntriesEnum, _
                              ByVal Ds_MedexRemarkDtl As DataSet, _
                              ByVal UserCode_1 As String, _
                              ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_MedexRemarkDtl = objMaster.Save_Masters(Choice_1, Masters_1, Ds_MedexRemarkDtl, UserCode_1, eStr_Retu)
    End Function
#End Region  ' == For LIMS

#Region "Save_HistoSampleUserApproval"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_HistoSampleUserApproval(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                              ByVal Masters_1 As MasterEntriesEnum, _
                              ByVal Ds_HistoSampleUserApproval As DataSet, _
                              ByVal UserCode_1 As String, _
                              ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_HistoSampleUserApproval = objMaster.Save_Masters(Choice_1, Masters_1, Ds_HistoSampleUserApproval, UserCode_1, eStr_Retu)
    End Function
#End Region  ' == For LIMS

#Region "Save_FranchiseCosrMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_FranchiseCosrMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Masters_1 As MasterEntriesEnum, _
                                  ByVal Ds_FranchiseCostMst As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_FranchiseCosrMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_FranchiseCostMst, UserCode_1, eStr_Retu)
    End Function
#End Region ' == For LIMS 

#Region "Save_SampleDispatch" ' == For LIMS
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SampleDispatch(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Masters_1 As MasterEntriesEnum, _
                                  ByVal Ds_SampleDispatch As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_SampleDispatch = objMaster.Save_Masters(Choice_1, Masters_1, Ds_SampleDispatch, UserCode_1, eStr_Retu)
    End Function
#End Region ' == For LIMS 

#Region "Save_BoneMarrowParameterDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_BoneMarrowParameterDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                              ByVal Masters_1 As MasterEntriesEnum, _
                              ByVal Ds_BoneParameter As DataSet, _
                              ByVal UserCode_1 As String, _
                              ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_BoneMarrowParameterDtl = objMaster.Save_Masters(Choice_1, Masters_1, Ds_BoneParameter, UserCode_1, eStr_Retu)
    End Function
#End Region  ' == For LIMS

#Region "Save_EmailAlert"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_EmailAlert(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Masters_1 As MasterEntriesEnum, _
                                  ByVal Ds_EmailAlert As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_EmailAlert = objMaster.Save_Masters(Choice_1, Masters_1, Ds_EmailAlert, UserCode_1, eStr_Retu)
    End Function
#End Region ' == For LIMS

#Region "Delete_Collection"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Delete_Collection(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                ByVal Ds_CollDelete As Data.DataSet, _
                                ByVal UserCode_1 As String, _
                                ByRef eStr_Retu As String) As Boolean

        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Delete_Collection = objMaster.Delete_Collection(Choice_1, Ds_CollDelete, objDtLogic, UserCode_1, eStr_Retu)

    End Function
#End Region '=== FOR LIMS

#Region "Save_MedexResultCriticalRemarks"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_MedexResultCriticalRemarks(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_MedexResultCriticalRemarks As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_MedexResultCriticalRemarks = objMaster.Save_MedexResultCriticalRemarks(Choice_1, Ds_MedexResultCriticalRemarks, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '== For LIMS 01 March 2012

#Region "Save_SampleUnlockDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SampleUnlockDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_SampleUnlockDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_SampleUnlockDtl = objMaster.Save_SampleUnlockDtl(Choice_1, Ds_SampleUnlockDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '== For LIMS 01 March 2012

#Region "Save_LabKitSendReceiveDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_LabKitSendReceiveDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_LabKitSampleTypeSendReciveDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_LabKitSendReceiveDtl = objMaster.Save_LabKitSendReceiveDtl(Choice_1, Ds_LabKitSampleTypeSendReciveDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "CancelledLabKit"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function CanCelledLabKit(ByVal SampleId_1 As String, _
                                                  ByVal UserCode_1 As String, _
                                                  ByVal Remarks As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim ObjHelp As clsHelpDbTable = Nothing

        ObjHelp = New clsHelpDbTable
        ObjHelp.ExecuteQuery_Boolean("Update SampleDetail set cStatusIndi ='D' ,dModifyOn = getDate(),iModifyBy= " + UserCode_1 + "  where nSampleId = " + SampleId_1, eStr_Retu)
        CanCelledLabKit = ObjHelp.Proc_CancelledLabKitSample(SampleId_1, UserCode_1, Remarks, eStr_Retu)

    End Function
#End Region

#Region "Edit_SampleDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Edit_SampleDetail(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_LabKitSampleTypeSendReciveDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Edit_SampleDetail = objMaster.Edit_SampleDetail(Choice_1, Ds_LabKitSampleTypeSendReciveDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Receive_SampleDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Receive_SampleDetail(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_LabKitSampleTypeSendReciveDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Receive_SampleDetail = objMaster.Receive_SampleDetail(Choice_1, Ds_LabKitSampleTypeSendReciveDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_LabKitDispatchmst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_LabKitDispatchmst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Masters_1 As MasterEntriesEnum, _
                                  ByVal Ds_LabKitDispatchmst As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_LabKitDispatchmst = objMaster.Save_LabKitDispatchmst(Choice_1, Ds_LabKitDispatchmst, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_ScreenigTmpTable"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_ScreenigTmpTable(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_MedExScreeningDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_ScreenigTmpTable = objMaster.Save_ScreenigTmpTable(Choice_1, Ds_MedExScreeningDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

    '============================================================================================
    'Web Methods For BA Module
    '============================================================================================

#Region "Save_FreezerMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_FreezerMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_FreezerMst As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_FreezerMst = objMaster.Save_FreezerMst(Choice_1, Ds_FreezerMst, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added on 17-01-2012===================

#Region "Save_CentrifugationMachineMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_CentrifugationMachineMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_CentrifugationMachineMst As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_CentrifugationMachineMst = objMaster.Save_CentrifugationMachineMst(Choice_1, Ds_CentrifugationMachineMst, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added on 17-01-2012===================

#Region "Save_SampleGroupMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SampleGroupMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_SampleGroupMst As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_SampleGroupMst = objMaster.Save_SampleGroupMst(Choice_1, Ds_SampleGroupMst, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added on 17-01-2012===================

#Region "Save_SampleStandardMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SampleStandardMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_SampleStandardMst As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_SampleStandardMst = objMaster.Save_SampleStandardMst(Choice_1, Ds_SampleStandardMst, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added on 17-01-2012===================

#Region "Save_BagBatchMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_BagBatchMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_BagBatchMst As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_BagBatchMst = objMaster.Save_BagBatchMst(Choice_1, Ds_BagBatchMst, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added on 17-01-2012===================

#Region "Save_SampleOperationMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SampleOperationMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_SampleOperationMst As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_SampleOperationMst = objMaster.Save_SampleOperationMst(Choice_1, Ds_SampleOperationMst, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added on 17-01-2012===================

#Region "Save_SampleOperationReasonMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SampleOperationReasonMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_SampleOperationReasonMst As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_SampleOperationReasonMst = objMaster.Save_SampleOperationReasonMst(Choice_1, Ds_SampleOperationReasonMst, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added on 17-01-2012===================

#Region "Save_CalibrationMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_CalibrationMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_CalibrationMst As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_CalibrationMst = objMaster.Save_CalibrationMst(Choice_1, Ds_CalibrationMst, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added on 17-01-2012===================

#Region "Save_SampleCentrifugationDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SampleCentrifugationDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_SampleCentrifugationDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_SampleCentrifugationDtl = objMaster.Save_SampleCentrifugationDtl(Choice_1, Ds_SampleCentrifugationDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added on 17-01-2012===================

#Region "Save_SamplePickUpDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SamplePickUpDetail(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_SamplePickUpDetail As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_SamplePickUpDetail = objMaster.Save_SamplePickUpDetail(Choice_1, Ds_SamplePickUpDetail, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added on 18-01-2012===================

#Region "Save_SampleSeparationDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SampleSeparationDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_SampleSeparationDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_SampleSeparationDtl = objMaster.Save_SampleSeparationDtl(Choice_1, Ds_SampleSeparationDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region  '================Added on 22-01-2012===================

#Region "Save_SampleOperationDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SampleOperationDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_SampleOperationDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_SampleOperationDtl = objMaster.Save_SampleOperationDtl(Choice_1, Ds_SampleOperationDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added on 24-01-2012===================

#Region "Save_SampleSendReceiveDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SampleSendReceiveDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_SampleSendReceiveDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_SampleSendReceiveDtl = objMaster.Save_SampleSendReceiveDtl(Choice_1, Ds_SampleSendReceiveDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added on 25-01-2012===================

#Region "Save_BagBatchDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_BagBatchDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_BagBatchDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef SampleOutput As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_BagBatchDtl = objMaster.Save_BagBatchDtl(Choice_1, Ds_BagBatchDtl, objDtLogic, UserCode_1, SampleOutput, eStr_Retu)
    End Function
#End Region  '================Added on 25-01-2012===================

#Region "Save_GeneralRemarksMst" '================Added on 23-03-2012===================
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_GeneralRemarksMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_GeneralRemarksMst As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_GeneralRemarksMst = objMaster.Save_GeneralRemarksMst(Choice_1, Ds_GeneralRemarksMst, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_BA_DocumentAttachment"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_BA_DocumentAttachment(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_BA_DocumentAttachment As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_BA_DocumentAttachment = objMaster.Save_BA_DocumentAttachment(Choice_1, Ds_BA_DocumentAttachment, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region  '================Added on 25-03-2012 By Mrunal===================

#Region "Save_CCSampleRetrievalMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_CCSampleRetrievalMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_CCSampleRetrievalMst As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                           ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_CCSampleRetrievalMst = objMaster.Save_CCSampleRetrievalMst(Choice_1, Ds_CCSampleRetrievalMst, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added on 07-04-2012 By Mrunal===================

#Region "Save_WinNonlin_DocumentAttachMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_WinNonlin_DocumentAttachMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_BA_DocumentAttachment As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_WinNonlin_DocumentAttachMst = objMaster.Save_WinNonlin_DocumentAttachMst(Choice_1, Ds_BA_DocumentAttachment, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region  '================Added on 24-05-2012 By Mrunal===================

#Region "insert_CRFVersionMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function insert_CRFVersionMst(ByVal Choice As DataObjOpenSaveModeEnum, _
                                                         ByVal Ds_CRFVersionMst As DataSet, _
                                                         ByVal UserCode As String, _
                                                         ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        insert_CRFVersionMst = objMaster.insert_CRFVersionMst(Choice, Ds_CRFVersionMst, objDtLogic, UserCode, eStr_Retu)
    End Function
#End Region

#Region "Save_SubjectPatientMapping"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SubjectPatientMapping(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_SubjectPatientMapping As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_SubjectPatientMapping = objMaster.Save_SubjectPatientMapping(Choice_1, Ds_SubjectPatientMapping, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region 'Added bY Akhilesh

#Region "Save_BASampleConcentrationFiles"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_BASampleConcentrationFiles(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_BASampleConcentrationFiles As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_BASampleConcentrationFiles = objMaster.Save_BASampleConcentrationFiles(Choice_1, Ds_BASampleConcentrationFiles, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_MedExScreeningTempDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_MedExScreeningTempDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_MedExScreeningDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef TranNo_Retu As String, _
                                                          ByRef Is_Transaction As Boolean, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_MedExScreeningTempDtl = objMaster.Save_MedExScreeningTempDtl(Choice_1, Ds_MedExScreeningDtl, objDtLogic, UserCode_1, TranNo_Retu, Is_Transaction, eStr_Retu)
    End Function
#End Region '===============Added on 25-09-2012 by Nidhi=============


#Region "Save_BAWorkflowStageMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_BAWorkflowStageMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                   ByVal Ds_BAWorkflowStageMst As DataSet, _
                                                   ByVal UserCode_1 As String, _
                                                   ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_BAWorkflowStageMst = objMaster.Save_BAWorkflowStageMst(Choice_1, Ds_BAWorkflowStageMst, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region  '===============Added on 5-10-2012 by Pundarik=============

#Region "Save_BASampleCollectionWorkFlow"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_BASampleCollectionWorkFlow(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                           ByVal Ds_BASampleCollectionWorkFlow As DataSet, _
                                                           ByVal UserCode_1 As String, _
                                                           ByRef nLatestWorkFlow As String, _
                                                           ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_BASampleCollectionWorkFlow = objMaster.Save_BASampleCollectionWorkFlow(Choice_1, Ds_BASampleCollectionWorkFlow, objDtLogic, UserCode_1, nLatestWorkFlow, eStr_Retu)
    End Function
#End Region  '===============Added on 5-10-2012 by Pundarik=============

#Region "Save_BAResultDCFDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_BAResultDCFDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                           ByVal Ds_BAResultDCFDtl As DataSet, _
                                                           ByVal UserCode_1 As String, _
                                                           ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_BAResultDCFDtl = objMaster.Save_BAResultDCFDtl(Choice_1, Ds_BAResultDCFDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_BASampleResults"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_BASampleResults(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                           ByVal Ds_BASampleResults As DataSet, _
                                                           ByVal UserCode_1 As String, _
                                                           ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_BASampleResults = objMaster.Save_BASampleResults(Choice_1, Ds_BASampleResults, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region  '===============Added on 5-10-2012 by Pundarik=============

#Region "Save_BASampleFinalResults"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_BASampleFinalResults(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                     ByVal Ds_BASampleFinalResults As DataSet, _
                                                     ByVal UserCode_1 As String, _
                                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_BASampleFinalResults = objMaster.Save_BASampleFinalResults(Choice_1, Ds_BASampleFinalResults, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region  '===============Added on 5-10-2012 by Pundarik=============

#Region "Save_BAWorkSpaceDataSetFiles"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_BAWorkSpaceDataSetFiles(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                     ByVal Ds_BAWorkSpaceDataSetFiles As DataSet, _
                                                     ByVal UserCode_1 As String, _
                                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_BAWorkSpaceDataSetFiles = objMaster.Save_BAWorkSpaceDataSetFiles(Choice_1, Ds_BAWorkSpaceDataSetFiles, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region  '===============Added on 5-10-2012 by Pundarik=============

#Region "Insert_WorkspaceSubjectMaster_Rejection"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Insert_WorkspaceSubjectMaster_Rejection(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                     ByVal Ds_WorkspaceSubjectMst As DataSet, _
                                                     ByVal UserCode_1 As String, _
                                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Insert_WorkspaceSubjectMaster_Rejection = objMaster.Insert_WorkspaceSubjectMaster_Rejection(Choice_1, Ds_WorkspaceSubjectMst, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region  '===============Added on 28-12-2012 by Megha=============

#Region "Insert_WorkSpaceScreeningHdrDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Insert_WorkSpaceScreeningHdrDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                     ByVal ds_WorkspaceScreeningHdrDtl As DataSet, _
                                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Insert_WorkSpaceScreeningHdrDtl = objMaster.Insert_WorkSpaceScreeningHdrDtl(Choice_1, ds_WorkspaceScreeningHdrDtl, objDtLogic, eStr_Retu)
    End Function
#End Region  '===============Added on 28-12-2012 by Megha============= 

#Region "Insert_ScreeningTemplateHdrDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Insert_ScreeningTemplateHdrDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                     ByVal ds_WorkspaceScreeningHdrDtl As DataSet, _
                                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Insert_ScreeningTemplateHdrDtl = objMaster.Insert_ScreeningTemplateHdrDtl(Choice_1, ds_WorkspaceScreeningHdrDtl, objDtLogic, eStr_Retu)
    End Function
#End Region  '===============Added on 28-12-2012 by Megha============= 

#Region "TableInsert"
    ''' <summary>
    ''' This Is Generalized function for saving multiple Datatables to DataBase on the basis of their specified Insert Procedure
    ''' </summary>
    ''' <param name="dsSaveDTtoProc">Datatables set where TAble Name is is the Insert Procedure Name we need to save with that procedure</param>
    ''' <param name="UserCode_1"></param>
    ''' <param name="eStr_Retu"></param>
    ''' <returns></returns>
    ''' <remarks>The table name must be named as the target table name to save in database</remarks>
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function TableInsert(ByVal dsProcAsTableName As DataSet, _
                                       ByVal UserCode_1 As String, _
                                       ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        TableInsert = objMaster.TableInsert(objDtLogic, dsProcAsTableName, UserCode_1, eStr_Retu)
    End Function
#End Region 'by akhilesh

#Region "BulkCopyWithColumnMapping"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function BulkCopyWithColumnMapping(ByVal dsBulk As DataSet, _
                                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        BulkCopyWithColumnMapping = objMaster.BulkCopyWithColumnMapping(dsBulk, objDtLogic, eStr_Retu)
    End Function
#End Region ''by akhilesh

    '=============================================================================
    'Added by Vimal Ghoniya For CDMS
    '============================================================================

#Region "SaveWorkSpaceScreeningScheduleHdrDtl" '=============Added on 28-08--2013
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function SaveWorkSpaceScreeningScheduleHdrDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_CRFWorkFlowDtl As DataSet, _
                                                          ByVal SaveDefaultValue As Boolean, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        SaveWorkSpaceScreeningScheduleHdrDtl = objMaster.SaveWorkSpaceScreeningScheduleHdrDtl(Choice_1, Ds_CRFWorkFlowDtl, SaveDefaultValue, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_WorkSpaceScreeningScheduleDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_WorkSpaceScreeningScheduleDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal ds_dtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_WorkSpaceScreeningScheduleDtl = objMaster.Save_WorkSpaceScreeningScheduleDtl(Choice_1, ds_dtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_CDMSSubjectMst"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_CDMSSubjectMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds_CDMSSubjectMst As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef SubjectId As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_CDMSSubjectMst = objMaster.Save_CDMSSubjectMst(Choice_1, Ds_CDMSSubjectMst, objDtLogic, UserCode_1, SubjectId, eStr_Retu)
    End Function

#End Region

#Region "Save_SubjectDtlCDMSMedicalCondition"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SubjectDtlCDMSMedicalCondition(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds_MediCond As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_SubjectDtlCDMSMedicalCondition = objMaster.Save_SubjectDtlCDMSMedicalCondition(Choice_1, Ds_MediCond, objDtLogic, UserCode_1, eStr_Retu)
    End Function

#End Region

#Region "Save_SubjectDtlCDMSConcoMedication"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SubjectDtlCDMSConcoMedication(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds_Medication As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_SubjectDtlCDMSConcoMedication = objMaster.Save_SubjectDtlCDMSConcoMedication(Choice_1, Ds_Medication, objDtLogic, UserCode_1, eStr_Retu)
    End Function

#End Region

#Region "Save_SubjectDtlCDMSConsumption"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SubjectDtlCDMSConsumption(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds_Medication As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_SubjectDtlCDMSConsumption = objMaster.Save_SubjectDtlCDMSConsumption(Choice_1, Ds_Medication, objDtLogic, UserCode_1, eStr_Retu)
    End Function

#End Region

#Region "Insert_UpdateSubjectCDMSDtlField"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Insert_UpdateSubjectCDMSDtlField(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Insert_UpdateSubjectCDMSDtlField = objMaster.Insert_UpdateSubjectCDMSDtlField(Choice_1, Ds, objDtLogic, UserCode_1, eStr_Retu)
    End Function

#End Region

#Region "Save_ProjectDtlCDMSConcoMedication"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_ProjectDtlCDMSConcoMedication(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds_Medication As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_ProjectDtlCDMSConcoMedication = objMaster.Save_ProjectDtlCDMSConcoMedication(Choice_1, Ds_Medication, objDtLogic, UserCode_1, eStr_Retu)
    End Function

#End Region

#Region "Save_ProjectDtlCDMSMedicalCondition"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_ProjectDtlCDMSMedicalCondition(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds_MediCond As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_ProjectDtlCDMSMedicalCondition = objMaster.Save_ProjectDtlCDMSMedicalCondition(Choice_1, Ds_MediCond, objDtLogic, UserCode_1, eStr_Retu)
    End Function

#End Region

#Region "Save_StudyDtlCDMS"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_StudyDtlCDMS(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds_CDMSSubjectMst As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_StudyDtlCDMS = objMaster.Save_StudyDtlCDMS(Choice_1, Ds_CDMSSubjectMst, objDtLogic, UserCode_1, eStr_Retu)
    End Function

#End Region

#Region "Save_StudyDtlCDMSConsumption"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_StudyDtlCDMSConsumption(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds_Consumptiopn As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_StudyDtlCDMSConsumption = objMaster.Save_StudyDtlCDMSConsumption(Choice_1, Ds_Consumptiopn, objDtLogic, UserCode_1, eStr_Retu)
    End Function

#End Region

#Region "Insert_UpdateStudyCDMSDtlField"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Insert_UpdateStudyCDMSDtlField(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Insert_UpdateStudyCDMSDtlField = objMaster.Insert_UpdateStudyCDMSDtlField(Choice_1, Ds, objDtLogic, UserCode_1, eStr_Retu)
    End Function

#End Region


#Region "insert_DataEntryControl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function insert_DataEntryControl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                     ByVal ds_DataEntryControl As DataSet, _
                                                        ByRef eStr_Retu As String, _
                                                        ByRef Return_Val As Char) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        insert_DataEntryControl = objMaster.insert_DataEntryControl(Choice_1, ds_DataEntryControl, objDtLogic, eStr_Retu, Return_Val)
    End Function
#End Region  '===============Added on 06-05-2012 by Megha============= 


#Region "Insert_DataentryControlHistory"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Insert_DataEntryControlHistory(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                     ByVal ds_DataEntryControl As DataSet, _
                                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Insert_DataEntryControlHistory = objMaster.insert_DataEntryControlHistory(Choice_1, ds_DataEntryControl, objDtLogic, eStr_Retu)
    End Function
#End Region


#Region "insert_WorkspaceEditChecksDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function insert_WorkspaceEditChecksDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                     ByVal ds_WorkspaceEditChecksDtl As DataSet, _
                                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        insert_WorkspaceEditChecksDtl = objMaster.insert_WorkspaceEditChecksDtl(Choice_1, ds_WorkspaceEditChecksDtl, objDtLogic, eStr_Retu)
    End Function
#End Region  '===============Added on 01-07-2012 by Megha============= 

#Region "Save_SubjectDtlCDMSStudyHistory"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SubjectDtlCDMSStudyHistory(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds_StudyHistory As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_SubjectDtlCDMSStudyHistory = objMaster.Save_SubjectDtlCDMSStudyHistory(Choice_1, Ds_StudyHistory, objDtLogic, UserCode_1, eStr_Retu)
    End Function

#End Region '===============Added on 16-08-2012 by Debashis============= 

#Region "Save_TimeZoneMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_TimeZoneMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds_TimeZoneMst As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_TimeZoneMst = objMaster.Save_TimeZoneMst(Choice_1, Ds_TimeZoneMst, objDtLogic, UserCode_1, eStr_Retu)
        'Save_TimeZoneMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_TimeZoneMst, UserCode_1, eStr_Retu)
    End Function
#End Region '===============Added on 16-08-2012 by Debashis============= 

#Region "Save_MedExDependency"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_MedExDependency(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds_MedexDependency As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_MedExDependency = objMaster.Save_MedExDependency(Choice_1, Ds_MedexDependency, objDtLogic, UserCode_1, eStr_Retu)
        'Save_TimeZoneMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_TimeZoneMst, UserCode_1, eStr_Retu)
    End Function
#End Region '===============Added on 16-08-2012 by Debashis============= 

#Region "Insert_ScreeningEntryControl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Insert_ScreeningEntryControl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                     ByVal ds_ScreeningEntryControl As DataSet, _
                                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Insert_ScreeningEntryControl = objMaster.Insert_ScreeningEntryControl(Choice_1, ds_ScreeningEntryControl, objDtLogic, eStr_Retu)
    End Function
#End Region  '===============Added on 04-Oct-2013 by Debvashis============= 

#Region "Insert_HL7Data"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Insert_HL7Data(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                     ByVal ds As DataSet, _
                                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Insert_HL7Data = objMaster.Insert_HL7Data(Choice_1, ds, objDtLogic, eStr_Retu)
    End Function
#End Region  '===============Added on 07-OCT-2013 by Megha============= 

#Region "Save_MedExScreeningDtlOnly"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_MedExScreeningDtlOnly(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                     ByVal ds_MedexScreeningDtl As DataSet, _
                                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_MedExScreeningDtlOnly = objMaster.Save_MedExScreeningDtlOnly(Choice_1, ds_MedexScreeningDtl, objDtLogic, eStr_Retu)
    End Function
#End Region  '===============Added on 04-Oct-2013 by Debvashis============= 

#Region "Save_MedexWorkspaceScreeningDtlOnly"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_MedexWorkspaceScreeningDtlOnly(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                     ByVal ds_MedexWorkspaceScreeningDtl As DataSet, _
                                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_MedexWorkspaceScreeningDtlOnly = objMaster.Save_MedexWorkspaceScreeningDtlOnly(Choice_1, ds_MedexWorkspaceScreeningDtl, objDtLogic, eStr_Retu)
    End Function
#End Region  '===============Added on 18-Oct-2013 by Debvashis============= Save_MedExScreeningHdrOnly
#Region "Save_MedexWorkspaceScreeningDtlOnly"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_MedExScreeningHdrOnly(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                     ByVal ds_MedexScreeningHdr As DataSet, _
                                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_MedExScreeningHdrOnly = objMaster.Save_MedExScreeningHdrOnly(Choice_1, ds_MedexScreeningHdr, objDtLogic, eStr_Retu)
    End Function
#End Region  '===============Added on 18-Oct-2013 by Debvashis============= 
#Region "Save_ScreeningWorkflow"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_ScreeningWorkflow(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                     ByVal ds_Workflow As DataSet, _
                                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_ScreeningWorkflow = objMaster.Save_ScreeningWorkflow(Choice_1, ds_Workflow, objDtLogic, eStr_Retu)
    End Function
#End Region  '===============Added on 18-Oct-2013 by Debvashis============= 
#Region "Save_SubjectDtlCDMSStatus"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SubjectDtlCDMSStatus(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                     ByVal ds_Status As DataSet, _
                                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_SubjectDtlCDMSStatus = objMaster.Save_SubjectDtlCDMSStatus(Choice_1, ds_Status, objDtLogic, eStr_Retu)
    End Function
#End Region  '===============Added on 30-Oct-2013 by Debvashis============= 
#Region "Save_WorkspaceSpecificSubject"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_WorkspaceSpecificSubject(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                  ByVal ds_ProjectSpecific As DataSet, _
                                                  ByVal UserCode_1 As String, _
                                                  ByRef eStr_Retu As String) As Boolean

        Dim ObjMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New

        Save_WorkspaceSpecificSubject = ObjMaster.Save_WorkspaceSpecificSubject(Choice_1, ds_ProjectSpecific, objDtLogic, UserCode_1, eStr_Retu)
    End Function

#End Region
    '==============================================
    'Added By Vimal Ghoniya for Sequence Generation
    '==============================================


#Region "Save_BAChildProjects"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_BAChildProjects(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds_BAChildProject As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_BAChildProjects = objMaster.Save_BAChildProjects(Choice_1, Ds_BAChildProject, objDtLogic, UserCode_1, eStr_Retu)
    End Function

#End Region

#Region "Save_BAProjectAnalyteDtl"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_BAProjectAnalyteDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds_BAChildProject As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_BAProjectAnalyteDtl = objMaster.Save_BAProjectAnalyteDtl(Choice_1, Ds_BAChildProject, objDtLogic, UserCode_1, eStr_Retu)
    End Function

#End Region

#Region "Save_BAAnalyteSampleHdr"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_BAAnalyteSampleHdr(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds_BAAnalyteSample As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_BAAnalyteSampleHdr = objMaster.Save_BAAnalyteSampleHdr(Choice_1, Ds_BAAnalyteSample, objDtLogic, UserCode_1, eStr_Retu)
    End Function

#End Region

#Region "SaveBaSampleReqHdrDtl" '=============Added on 28-08--2013
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function SaveBaSampleReqHdrDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_BaSampleReqHdrDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        SaveBaSampleReqHdrDtl = objMaster.SaveBaSampleReqHdrDtl(Choice_1, Ds_BaSampleReqHdrDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Insert_UpdateSubjectPIFFields"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Insert_UpdateSubjectPIFFields(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Insert_UpdateSubjectPIFFields = objMaster.Insert_UpdateSubjectPIFFields(Choice_1, Ds, objDtLogic, UserCode_1, eStr_Retu)
    End Function '=============== Added By Megha Shah=============='

#End Region

#Region "Insert_SubjectHabitDetails"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Insert_SubjectHabitDetails(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds As DataSet, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Insert_SubjectHabitDetails = objMaster.Insert_SubjectHabitDetails(Choice_1, Ds, objDtLogic, eStr_Retu)
    End Function '=============== Added By Megha Shah=============='

#End Region

#Region "Insert_BaSampleSeqError"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Insert_BaSampleSeqError(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds As DataSet, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Insert_BaSampleSeqError = objMaster.Insert_BaSampleSeqError(Choice_1, Ds, objDtLogic, eStr_Retu)
    End Function '=============== Added By Nipun Khant=============='

#End Region

#Region "Save_BaSampleReqHdr"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_BaSampleReqHdr(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal ds_Hdr As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_BaSampleReqHdr = objMaster.Save_BaSampleReqHdr(Choice_1, ds_Hdr, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_BaSampleReqDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_BaSampleReqDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal ds_Dtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_BaSampleReqDtl = objMaster.Save_BaSampleReqDtl(Choice_1, ds_Dtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "SaveBaSequenceScheduleHdrDtl" '=============Added on 07-1--2014
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function SaveBaSequenceScheduleHdrDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_BaSequenceScheduleHdrDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        SaveBaSequenceScheduleHdrDtl = objMaster.SaveBaSequenceScheduleHdrDtl(Choice_1, Ds_BaSequenceScheduleHdrDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "SaveBaTemplateHdrDtl" '=============Added on 07-1--2014
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function SaveBaTemplateHdrDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_BaTemplateHdrDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        SaveBaTemplateHdrDtl = objMaster.SaveBaTemplateHdrDtl(Choice_1, Ds_BaTemplateHdrDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_BASequenceExportDtl"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_BASequenceExportDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds_BAChildProject As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_BASequenceExportDtl = objMaster.Save_BASequenceExportDtl(Choice_1, Ds_BAChildProject, objDtLogic, UserCode_1, eStr_Retu)
    End Function

#End Region

#Region "Save_BASetProjectMatrix"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_BASetProjectMatrix(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds_ProjectMatrix As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_BASetProjectMatrix = objMaster.Save_BASetProjectMatrix(Choice_1, Ds_ProjectMatrix, objDtLogic, UserCode_1, eStr_Retu)
    End Function

#End Region

#Region "Save_UpdateFieldValues"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_UpdateFieldValues(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_UpdateFieldValues = objMaster.Save_UpdateFieldValues(Choice_1, Ds, objDtLogic, UserCode_1, eStr_Retu)
    End Function

#End Region

#Region "Save_PkSampleReviewDtl"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_PkSampleReviewDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds As DataSet, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_PkSampleReviewDtl = objMaster.Save_PkSampleReviewDtl(Choice_1, Ds, objDtLogic, eStr_Retu)
    End Function

#End Region     '===============Added on 07-Mar-2014 by Nipun Khant============= 

#Region "Save_BaSequenceScheduleHdr"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_BaSequenceScheduleHdr(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_BaSequenceScheduleHdr As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_BaSequenceScheduleHdr = objMaster.Save_BaSequenceScheduleHdr(Choice_1, Ds_BaSequenceScheduleHdr, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_BaSequenceScheduleDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_BaSequenceScheduleDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_BaSequenceScheduleDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_BaSequenceScheduleDtl = objMaster.Save_BaSequenceScheduleDtl(Choice_1, Ds_BaSequenceScheduleDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_BASequenceConcentrationMatrix"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_BASequenceConcentrationMatrix(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_ConcentrationSet As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_BASequenceConcentrationMatrix = objMaster.Save_BASequenceConcentrationMatrix(Choice_1, Ds_ConcentrationSet, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

    'Added by Parth Modi on dated 15-March-2014
    'Reason: Insert_BAFileDetail
#Region "Save_BAFileDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_BAFileDetail(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                     ByVal ds_BAFileDetail As DataSet, _
                                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_BAFileDetail = objMaster.Save_BAFileDetail(Choice_1, ds_BAFileDetail, objDtLogic, eStr_Retu)
    End Function
#End Region

#Region "Save_CentrifugateParameterMst"

    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_CentrifugateParameterMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Ds As DataSet, _
                                      ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_CentrifugateParameterMst = objMaster.Save_CentrifugateParameterMst(Choice_1, Ds, objDtLogic, UserCode_1, eStr_Retu)
    End Function

#End Region     '===============Added on 04-Apr-2014 by Nipun Khant============= 

#Region "Save_IndividualSampleOperationDtl"
    'insert individual sample detail (freezer,compartment,operation) in sampleoperationdtl
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_IndividualSampleOperationDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_SampleOperationDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_IndividualSampleOperationDtl = objMaster.Save_IndividualSampleOperationDtl(Choice_1, Ds_SampleOperationDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added on 01-May-2014 ===================

    'Added by Parth Modi on 11-Aug-2014      Reason: 2 screening issue Observe
#Region "Save_ScreeningTimeMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_ScreeningTimeMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_ScreeningTimeMst As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef TranNo_Retu As String, _
                                                          ByRef Is_Transaction As Boolean, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_ScreeningTimeMst = objMaster.Save_ScreeningTimeMst(Choice_1, Ds_ScreeningTimeMst, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "ExecuteQuery_Boolean"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function ExecuteQuery_Boolean(ByVal strQuery As String, ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        ExecuteQuery_Boolean = objMaster.ExecuteQuery_Boolean(strQuery, eStr_Retu)
    End Function
#End Region

#Region "Proc_ReleaseLabReport"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Proc_ReleaseLabReport(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                         ByVal Ds_ReleaseLabReport As DataSet, _
                                         ByVal UserCode_1 As String, _
                                         ByRef ErrorMsg As String, _
                                         ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Proc_ReleaseLabReport = objMaster.Proc_ReleaseLabReport(Choice_1, Ds_ReleaseLabReport, UserCode_1, ErrorMsg, eStr_Retu)
    End Function
#End Region

    'Added by Parth Modi on 10-Dec-2014      Reason: Single PK issue Observe
#Region "Save_CollectionTimeMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_CollectionTimeMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_CollectionTimeMst As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef TranNo_Retu As String, _
                                                          ByRef Is_Transaction As Boolean, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_CollectionTimeMst = objMaster.Save_CollectionTimeMst(Choice_1, Ds_CollectionTimeMst, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

    'Added by Rahul Rupareliya  on 05-Feb-2015

#Region "Save_MachineMedExMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Insert_MachineMedExMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_MachineMedExMst As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Insert_MachineMedExMst = objMaster.Insert_MachineMedExMst(Choice_1, Ds_MachineMedExMst, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Proc_CopyProject"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Proc_CopyProject(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Ds_copyproject As DataSet, _
                                       ByRef ErrorMsg As String, _
                                         ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Proc_CopyProject = objMaster.Proc_CopyProject(Choice_1, Ds_copyproject, ErrorMsg, eStr_Retu)
    End Function
#End Region

    'Add by Shivani Pandya
#Region "Save_CRFUPLOADGUIDELINEDTL"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_CRFUPLOADGUIDELINEDTL(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal DS_CRFUpload As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean

        Dim ObjMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New

        Save_CRFUPLOADGUIDELINEDTL = ObjMaster.Save_CRFUPLOADGUIDELINEDTL(Choice_1, DS_CRFUpload, objDtLogic, UserCode_1, eStr_Retu)

    End Function

#End Region
    'End

    '===============================================
    'Added By Nipun Khant For Reprint-Delete audittrail cr

#Region "insert_ReprintDeleteRemarks"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_ReprintDeleteSampleAudit(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_ReprintDeleteSampleAudit As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_ReprintDeleteSampleAudit = objMaster.Save_ReprintDeleteSampleAudit(Choice_1, Ds_ReprintDeleteSampleAudit, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

    '===============================================

    '===============================================
    'Added By Rahul Rupareliya for Deviation Report For CT 

#Region "insert_WorkSpaceDeviationReport"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_WorkSpaceDeviationReport(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_WorkSpaceDeviationReport As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_WorkSpaceDeviationReport = objMaster.Save_WorkSpaceDeviationReport(Choice_1, Ds_WorkSpaceDeviationReport, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

    '===============================================

#Region "Save_IndividualSampleOperationDtlForRemarks"
    'insert individual sample detail (freezer,compartment,operation) in sampleoperationdtl
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_IndividualSampleOperationDtlForRemarks(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_SampleOperationDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_IndividualSampleOperationDtlForRemarks = objMaster.Save_IndividualSampleOperationDtlForRemarks(Choice_1, Ds_SampleOperationDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added By Nipun Khant 12-Oct-2015 ===================

    'Added By Nipun Khant For dynamic review
#Region "Save_ProjectReviewerMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_ProjectReviewerMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal Ds_ProjectReviewerMst As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_ProjectReviewerMst = objMaster.Save_ProjectReviewerMst(Choice_1, Ds_ProjectReviewerMst, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_SubjectLabReportDetailForRevised"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SubjectLabReportDetailForRevised(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Ds_SubjectWorkspaceAssignment As DataSet, _
                                     ByVal UserCode_1 As String, _
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_SubjectLabReportDetailForRevised = objMaster.Save_SubjectLabReportDetailForRevised(Choice_1, Ds_SubjectWorkspaceAssignment, UserCode_1, eStr_Retu)
    End Function
#End Region

    '' Added by ketan
#Region "Save_InsertServiceMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertServiceMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Masters_1 As MasterEntriesEnum, _
                                     ByVal Ds_OperationMst As DataSet, _
                                     ByVal UserCode_1 As String, _
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_InsertServiceMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_OperationMst, UserCode_1, eStr_Retu)
    End Function
#End Region

    ' added by prayag
#Region "Save_InstrumentInterface"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InstrumentInterface(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Ds_InstruMentHdr As DataSet, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_InstrumentInterface = objMaster.Save_InstrumentInterface(Choice_1, Ds_InstruMentHdr, objDtLogic, eStr_Retu)
    End Function
#End Region

#Region "Save_VisitTracker"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_VisitTracker(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Ds_AddVisit As DataSet, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_VisitTracker = objMaster.Save_VisitTracker(Choice_1, Ds_AddVisit, objDtLogic, eStr_Retu)
    End Function
#End Region

    '' Adede By Rahul Rupareliya
#Region "Save_VisitScheduler"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_Scheduler(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Ds_VisitScheduler As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_Scheduler = objMaster.Save_VisitScheduler(Choice_1, Ds_VisitScheduler, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_VisitScheduler"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_WorkSpaceSubjectMaster(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Ds_WorkSpaceSubejctMaster As DataSet, _
                                  ByVal UserCode_1 As String, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_WorkSpaceSubjectMaster = objMaster.Save_WorkSpaceSubjectMaster(Choice_1, Ds_WorkSpaceSubejctMaster, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_MachineSampleMedexDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_MachineSampleMedexDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal ds_MachineWorkListSampleMedexDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_MachineSampleMedexDtl = objMaster.Save_MachineSampleMedexDtl(Choice_1, ds_MachineWorkListSampleMedexDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added By Vivek Patel ===================

#Region "Save_MachineSampleWorkListDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_MachineSampleWorkListDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                              ByVal Ds_SampleMedEXDetail As DataSet, _
                              ByVal UserCode_1 As String, _
                              ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_MachineSampleWorkListDtl = objMaster.Save_MachineSampleWorkListDtl(Choice_1, Ds_SampleMedEXDetail, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added By Vivek Patel ===================

#Region "Save_CopyScreeningTemplate"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_CopyScreeningTemplate(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                              ByVal Ds_SampleMedEXDetail As DataSet, _
                              ByVal UserCode_1 As String, _
                              ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_CopyScreeningTemplate = objMaster.Save_CopyScreeningTemplate(Choice_1, Ds_SampleMedEXDetail, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region


#Region "Save_ScreeningTemplateVersionMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_ScreeningTemplateVersionMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                              ByVal Ds_SampleMedEXDetail As DataSet, _
                              ByVal UserCode_1 As String, _
                              ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_ScreeningTemplateVersionMst = objMaster.Save_ScreeningTemplateVersionMst(Choice_1, Ds_SampleMedEXDetail, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_SubjectIrisDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SubjectIrisDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal ds_SubjectIrisDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_SubjectIrisDtl = objMaster.Save_SubjectIrisDtl(Choice_1, ds_SubjectIrisDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added By Simki Jayswal ===================

#Region "Save_SubjectIrisVerificationDetail"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SubjectIrisVerificationDetail(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal ds_SubjectIrisVrfctnDtl As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_SubjectIrisVerificationDetail = objMaster.Save_SubjectIrisVerificationDetail(Choice_1, ds_SubjectIrisVrfctnDtl, objDtLogic, UserCode_1, eStr_Retu)
    End Function

#End Region

#Region "Save_SubjectPopulationMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SubjectPopulationMst(ByVal Choice As DataObjOpenSaveModeEnum, _
                                                    ByVal Ds_SubjectPopulationMst As Data.DataSet, _
                                                    ByVal UserCode_1 As String, _
                                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_SubjectPopulationMst = objMaster.Save_SubjectPopulationMst(Choice, Ds_SubjectPopulationMst, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added By Vineet Chourasia =================== 

#Region "Save_PanelDisplay"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_PanelDisplay(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Ds_PanelDisplay As DataSet, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_PanelDisplay = objMaster.Save_PanelDisplay(Choice_1, Ds_PanelDisplay, objDtLogic, eStr_Retu)
    End Function
#End Region

#Region "Save_SCreeningDCFMSt"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_ScreeningDCFMST(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Ds_ScreeningDCFMST As DataSet, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_ScreeningDCFMST = objMaster.Save_ScreeningDCFMST(Choice_1, Ds_ScreeningDCFMST, objDtLogic, eStr_Retu)
    End Function
#End Region

#Region "Save_PrinterDtl"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_PrinterDtl(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                    ByVal Masters_1 As MasterEntriesEnum, _
                                    ByVal Ds_StageMst As DataSet, _
                                    ByVal UserCode_1 As String, _
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster

        Save_PrinterDtl = objMaster.Save_Masters(Choice_1, Masters_1, Ds_StageMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_InsertNoticeMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_InsertNoticeMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                     ByVal Masters_1 As MasterEntriesEnum, _
                                     ByVal Ds_OperationMst As DataSet, _
                                     ByVal UserCode_1 As String, _
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_InsertNoticeMst = objMaster.Save_Masters(Choice_1, Masters_1, Ds_OperationMst, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_ProjectActivityOperationMatrix"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_ProjectActivityOperationMatrix(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal ds_ProjectActivityOperationMatrix As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_ProjectActivityOperationMatrix = objMaster.Save_ProjectActivityOperationMatrix(Choice_1, ds_ProjectActivityOperationMatrix, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added By Vivek Patel ===================

#Region "Save_ScreeningBarcodeMstT"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_ScreeningBarcodeMst(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                  ByVal Ds_ScreeningBarcodeMst As DataSet, _
                                  ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New()
        Save_ScreeningBarcodeMst = objMaster.Save_ScreeningBarcodeMst(Choice_1, Ds_ScreeningBarcodeMst, objDtLogic, eStr_Retu)
    End Function
#End Region

#Region "Save_SubjectECGDetails"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_SubjectECGDetails(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal ds_SubjectECGDetails As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_SubjectECGDetails = objMaster.Save_SubjectECGDetails(Choice_1, ds_SubjectECGDetails, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '================Added By Vivek Patel ===================

#Region "Save_DISoftProjectActivityOperationMatrix"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)> _
    Public Function Save_DISoftProjectActivityOperationMatrix(ByVal Choice_1 As DataObjOpenSaveModeEnum, _
                                                          ByVal ds_ProjectActivityOperationMatrix As DataSet, _
                                                          ByVal UserCode_1 As String, _
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_DISoftProjectActivityOperationMatrix = objMaster.Save_DISoftProjectActivityOperationMatrix(Choice_1, ds_ProjectActivityOperationMatrix, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '

#Region "Save_ImgTransmittalHdr"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Save_ImgTransmittalHdr(ByVal Choice_1 As DataObjOpenSaveModeEnum,
                                                          ByVal ds_ImgTransmittalHdr As DataSet,
                                                          ByVal UserCode_1 As String,
                                                          ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_ImgTransmittalHdr = objMaster.Save_ImgTransmittalHdr(Choice_1, ds_ImgTransmittalHdr, objDtLogic, UserCode_1, eStr_Retu)
    End Function
#End Region '=============== Added by Shyam Kamdar ================

#Region "Save_EmailSetupMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Save_EmailSetupMst(ByVal Choice_1 As DataObjOpenSaveModeEnum,
                                    ByVal Ds_EmailSetupMst As DataSet,
                                    ByVal UserCode_1 As String,
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_EmailSetupMst = objMaster.Save_EmailSetupMst(Choice_1, Ds_EmailSetupMst, objDtLogic, UserCode_1, eStr_Retu)

    End Function
#End Region '=============== Added by Shyam Kamdar on 28-04-2021 ================

#Region "Save_OtpInfoDetails"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Save_OtpInfoDetails(ByVal Choice_1 As DataObjOpenSaveModeEnum,
                                     ByVal dsOtpInfo As DataSet,
                                     ByVal UserCode_1 As String,
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Save_OtpInfoDetails = objMaster.Save_OtpInfoDetails(Choice_1, dsOtpInfo, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save Email details"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function SaveExMsgInfo(ByVal Choice_1 As DataObjOpenSaveModeEnum,
                                  ByVal dsExMsgInfo As DataSet,
                                     ByVal UserCode_1 As String,
                                     ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        SaveExMsgInfo = objMaster.SaveExMsgInfo(Choice_1, dsExMsgInfo, UserCode_1, eStr_Retu)
    End Function
#End Region

#Region "Save_EmailSetupMst"
    <WebMethod(), SoapHeader(WebConstant.AUTHENTICATION_SOAP_HEADER_NAME)>
    Public Function Save_QueryMaster(ByVal Choice_1 As DataObjOpenSaveModeEnum,
                                    ByVal Ds_QueryMaster As DataSet,
                                    ByVal UserCode_1 As String,
                                    ByRef eStr_Retu As String) As Boolean
        Dim objMaster As New ClsMaster
        Dim objDtLogic As New ClsDataLogic_New
        Save_QueryMaster = objMaster.Save_QueryMaster(Choice_1, Ds_QueryMaster, objDtLogic, UserCode_1, eStr_Retu)

    End Function
#End Region '=============== Added by Shyam Kamdar on 28-04-2021 ================
End Class