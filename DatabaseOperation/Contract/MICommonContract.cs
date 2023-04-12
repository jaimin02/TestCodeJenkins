using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections;
using DTO;
using Utility;
using DatabaseOperation;
using DatabaseOperation.DBContract;

namespace DatabaseOperation.Contract
{
    public class MICommonContract
    {
        SqlHelper _SqlHelper;
        DataSet Sql_Ds;
        DataTable Sql_Dt;
        Hashtable ht;

        public MICommonContract()
        {
            _SqlHelper = new SqlHelper();
            Sql_Ds = new DataSet();
            Sql_Dt = new DataTable();
            ht = new Hashtable();
        }

        public DataTable AuditTrail(AuditTrailDTO _AuditTrailDTO)
        {           
            try
            {
                ht.Add(Utilities.clsParameters.vTableName, string.IsNullOrEmpty(_AuditTrailDTO.vTableName) ? "" : _AuditTrailDTO.vTableName);
                ht.Add(Utilities.clsParameters.vIdName, String.IsNullOrEmpty(_AuditTrailDTO.vIdName) ? "" : _AuditTrailDTO.vIdName);
                ht.Add(Utilities.clsParameters.vIdValue, string.IsNullOrEmpty(_AuditTrailDTO.vIdValue) ? "" : _AuditTrailDTO.vIdValue);
                ht.Add(Utilities.clsParameters.vFieldName, String.IsNullOrEmpty(_AuditTrailDTO.vFieldName) ? "" : _AuditTrailDTO.vFieldName);
                ht.Add(Utilities.clsParameters.iUserId, String.IsNullOrEmpty(_AuditTrailDTO.iUserId) ? "" : _AuditTrailDTO.iUserId);
                ht.Add(Utilities.clsParameters.vMasterFieldName, String.IsNullOrEmpty(_AuditTrailDTO.vMasterFieldName) ? "" : _AuditTrailDTO.vMasterFieldName);
                ht.Add(Utilities.clsParameters.vMasterTableName, String.IsNullOrEmpty(_AuditTrailDTO.vMasterTableName) ? "" : _AuditTrailDTO.vMasterTableName);

                //Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Proc_GetAuditTrail, ht);
                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Proc_GetAuditTrailForDiSoft, ht);
                return Sql_Ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
 
            }

        }

        public DataTable GetProject(ProjectDTO _ProjectDTO)
        {
            try
            {
                if (_ProjectDTO.cProjectFilter == "Y")
                {
                    Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsView.View_Project, "vRequestId + vProjectNo Like '%" + _ProjectDTO.vProjectNo + "%'", Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                }
                else
                {
                    Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsView.View_Project, Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return Sql_Dt;
        }

        public DataTable GetSubject(SubjectDTO _SubjectDTO)
        {
            try
            {
                if (_SubjectDTO.cSubjectFilter == "Y")
                {
                    Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsView.View_Subject, "vWorkspaceId = '" + _SubjectDTO.vWorkSpaceID + "' and DisplayName Like '%" + _SubjectDTO.vSubjectNo + "%'", Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                }
                else
                {
                    Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsView.View_Subject, "vWorkspaceId = '" + _SubjectDTO.vWorkSpaceID + "'",Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return Sql_Dt;
        }

        public DataTable GetSubjectForDISOFT(SubjectDTO _SubjectDTO)
        {
            try
            {
                if (_SubjectDTO.cSubjectFilter == "Y")
                {
                    Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsView.View_SubjectForDISOFT, "vWorkspaceId = '" + _SubjectDTO.vWorkSpaceID + "' and DisplayName Like '%" + _SubjectDTO.vSubjectNo + "%'", Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                }
                else
                {
                    Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsView.View_SubjectForDISOFT, "vWorkspaceId = '" + _SubjectDTO.vWorkSpaceID + "'", Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return Sql_Dt;
        }

        public DataSet ProjectVisitDetails(ProjectVisitDTO _ProjectVisitDTO)
        {
            try
            {
                ht.Add(Utilities.clsParameters.vWorkspaceId, _ProjectVisitDTO.vWorkSpaceID);
                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Proc_GetProjectVisitDetails, ht);
                return Sql_Ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { 
            }
        }

        public DataSet MAXiImageTranNo(MAXiImageTranNoDTO _MAXiImageTranNoDTO)
        {
            try
            {
                ht.Add(Utilities.clsParameters.vWorkspaceId, _MAXiImageTranNoDTO.vWorkSpaceID);
                ht.Add(Utilities.clsParameters.vSubjectId, _MAXiImageTranNoDTO.vSubjectId);
                ht.Add(Utilities.clsParameters.iNodeId, _MAXiImageTranNoDTO.iNodeId);
                ht.Add(Utilities.clsParameters.vModalityNo, _MAXiImageTranNoDTO.vModalityNo);
                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Pro_GetMAXiImageTranNo, ht);
                return Sql_Ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public DataSet CertMAXiImageTranNo(CertMAXiImageTranNoDTO _CertMAXiImageTranNoDTO)
        {
            try
            {
                ht.Add(Utilities.clsParameters.vWorkspaceId, _CertMAXiImageTranNoDTO.vWorkSpaceID);
                ht.Add(Utilities.clsParameters.vModalityNo, _CertMAXiImageTranNoDTO.vModalityNo);
                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Pro_CertGetMAXiImageTranNo, ht);
                return Sql_Ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public DataSet CheckVisitIsReviewedGetMAXiImageTranNo(CheckVisitIsReviewedGetMAXiImageTranNoDTO _CheckVisitIsReviewedGetMAXiImageTranNoDTO)
        {
            try
            {
                ht.Add(Utilities.clsParameters.vWorkspaceId, _CheckVisitIsReviewedGetMAXiImageTranNoDTO.vWorkSpaceID);
                ht.Add(Utilities.clsParameters.vSubjectId, _CheckVisitIsReviewedGetMAXiImageTranNoDTO.vSubjectId);
                ht.Add(Utilities.clsParameters.vParentActivityId, _CheckVisitIsReviewedGetMAXiImageTranNoDTO.vParentActivityId);
                ht.Add(Utilities.clsParameters.iParentNodeId, _CheckVisitIsReviewedGetMAXiImageTranNoDTO.iParentNodeId);
                ht.Add(Utilities.clsParameters.vActivityId, _CheckVisitIsReviewedGetMAXiImageTranNoDTO.vActivityId);
                ht.Add(Utilities.clsParameters.iNodeId, _CheckVisitIsReviewedGetMAXiImageTranNoDTO.iNodeId);
                ht.Add(Utilities.clsParameters.iModalityNo, _CheckVisitIsReviewedGetMAXiImageTranNoDTO.iModalityNo);
                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Proc_CheckVisitIsReviewedGetMAXiImageTranNo, ht);
                return Sql_Ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        } 
    }
}
