using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DTO;
using DatabaseOperation;
using DatabaseOperation.Contract;

namespace BusinessOperation.Repository
{
    public class MICommonRepositiry
    {
        MICommonContract _MICommonContract;
        public MICommonRepositiry()
        {
            _MICommonContract = new MICommonContract(); 
        }

        public DataTable AuditTrail(AuditTrailDTO _AuditTrailDTO)
        {
            return _MICommonContract.AuditTrail(_AuditTrailDTO);
        }

        public DataTable GetProject(ProjectDTO _ProjectDTO)
        {
            return _MICommonContract.GetProject(_ProjectDTO);
        }

        public DataTable GetSubject(SubjectDTO _SubjectDTO)
        {
            return _MICommonContract.GetSubject(_SubjectDTO);
        }

        public DataTable GetSubjectForDISOFT(SubjectDTO _SubjectDTO)
        {
            return _MICommonContract.GetSubjectForDISOFT(_SubjectDTO);
        }

        public DataSet ProjectVisitDetails(ProjectVisitDTO _ProjectVisitDTO)
        {
            return _MICommonContract.ProjectVisitDetails(_ProjectVisitDTO);
        }

        public DataSet MAXiImageTranNo(MAXiImageTranNoDTO _MAXiImageTranNoDTO)
        {
            return _MICommonContract.MAXiImageTranNo(_MAXiImageTranNoDTO);
        }

        public DataSet CertMAXiImageTranNo(CertMAXiImageTranNoDTO _CertMAXiImageTranNoDTO)
        {
            return _MICommonContract.CertMAXiImageTranNo(_CertMAXiImageTranNoDTO);
        }

        public DataSet CheckVisitIsReviewedGetMAXiImageTranNo(CheckVisitIsReviewedGetMAXiImageTranNoDTO _CheckVisitIsReviewedGetMAXiImageTranNoDTO)
        {
            return _MICommonContract.CheckVisitIsReviewedGetMAXiImageTranNo(_CheckVisitIsReviewedGetMAXiImageTranNoDTO);
        } 
    }
}
