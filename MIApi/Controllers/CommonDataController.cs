using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.ComponentModel;
using System.Web.Mvc;
using DTO;
using BusinessOperation;
using BusinessOperation.Repository;

namespace MIApi.Controllers
{
    public class CommonDataController : ApiController
    {
        MICommonRepositiry _MICommonRepositiry;
        public CommonDataController()
        {
            _MICommonRepositiry = new MICommonRepositiry();
        }

        public DataTable AuditTrail(AuditTrailDTO _AuditTrailDTO)
        {
            DataTable dt_AuditTrail = new DataTable();
            dt_AuditTrail = _MICommonRepositiry.AuditTrail(_AuditTrailDTO);
            if (dt_AuditTrail == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return dt_AuditTrail;

        }
        public HttpResponseMessage ProjectDetails([FromBody]ProjectDTO _ProjectDTO)
        {            
            var project = _MICommonRepositiry.GetProject(_ProjectDTO);
            return Request.CreateResponse(HttpStatusCode.OK, project);

        }
        //public HttpResponseMessage SubjectDetails([FromBody]SubjectDTO _SubjectDTO)
        //{
        //    var project = _MICommonRepositiry.GetSubject(_SubjectDTO);
        //    return Request.CreateResponse(HttpStatusCode.OK, project);
        //}
        public HttpResponseMessage ProjectVisitDetails([FromBody]ProjectVisitDTO _ProjectVisitDTO)
        {
            var projectVisitDetails = _MICommonRepositiry.ProjectVisitDetails(_ProjectVisitDTO);
            return Request.CreateResponse(HttpStatusCode.OK, projectVisitDetails); 
        }

        public HttpResponseMessage MAXiImageTranNo([FromBody]MAXiImageTranNoDTO _MAXiImageTranNoDTO)
        {
            var GetMAXiImageTranNo = _MICommonRepositiry.MAXiImageTranNo(_MAXiImageTranNoDTO);
            return Request.CreateResponse(HttpStatusCode.OK, GetMAXiImageTranNo);
        }

        public HttpResponseMessage CertMAXiImageTranNo([FromBody]CertMAXiImageTranNoDTO _CertMAXiImageTranNoDTO)
        {
            var GetCertMAXiImageTranNo = _MICommonRepositiry.CertMAXiImageTranNo(_CertMAXiImageTranNoDTO);
            return Request.CreateResponse(HttpStatusCode.OK, GetCertMAXiImageTranNo);
        }
        public HttpResponseMessage CheckVisitIsReviewedGetMAXiImageTranNo([FromBody]CheckVisitIsReviewedGetMAXiImageTranNoDTO _CheckVisitIsReviewedGetMAXiImageTranNoDTO)
        {
            var ObjCheckVisitIsReviewedGetMAXiImageTranNo = _MICommonRepositiry.CheckVisitIsReviewedGetMAXiImageTranNo(_CheckVisitIsReviewedGetMAXiImageTranNoDTO);
            return Request.CreateResponse(HttpStatusCode.OK, ObjCheckVisitIsReviewedGetMAXiImageTranNo);
        }

        public HttpResponseMessage SubjectDetailsForDISOFT([FromBody]SubjectDTO _SubjectDTO)
        {
            var project = _MICommonRepositiry.GetSubjectForDISOFT(_SubjectDTO);
            return Request.CreateResponse(HttpStatusCode.OK, project);
        }
    }    
}
