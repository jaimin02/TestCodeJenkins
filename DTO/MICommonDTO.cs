using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DTO
{
    public class MICommonDTO
    {
    }

    public class DatatableDTO
    {
        [DefaultValue("")]
        public string vWorkspaceId { get; set; }

        [DefaultValue("")]
        public string SPName { get; set; }
    }

    public class AuditTrailDTO
    {
        [DefaultValue("")]
        public string vTableName { get; set; }
        [DefaultValue("")]
        public string vIdName { get; set; }
        [DefaultValue("")]
        public string vIdValue { get; set; }
        [DefaultValue("")]
        public string vFieldName { get; set; }
        [DefaultValue("")]
        public string iUserId { get; set; }
        [DefaultValue("")]
        public string vMasterFieldName { get; set; }
        [DefaultValue("")]
        public string vMasterTableName { get; set; }
    }

    public class ProjectDTO
    {
        [DefaultValue("")]
        public string iUserId { get; set; }
        [DefaultValue("")]
        public string vProjectNo { get; set; }
        [DefaultValue("")]
        public string vWorkSpaceID { get; set; }
        [DefaultValue("")]
        public string vProjectTypeCode { get; set; }
        [DefaultValue("")]
        public string cProjectFilter { get; set; }
        //N for All and Y for Filter
    }

    public class SubjectDTO
    {
        [DefaultValue("")]
        public string vSubjectNo { get; set; }       
        [DefaultValue("")]
        public string vWorkSpaceID { get; set; }
        [DefaultValue("")]
        public string cSubjectFilter { get; set; }
        [DefaultValue("")]
        public string iUserId { get; set; }
    }

    public class ProjectVisitDTO
    {
        [DefaultValue("")]
        public string vWorkSpaceID { get; set; }
    }

    public class GetMyProjectCompletionListDTO
    {
        [DefaultValue("")]
        public String contextKey { get; set; }
        [DefaultValue("")]
        public String vProjectTypeCode { get; set; }
        [DefaultValue("")]
        public String prefixText { get; set; }
    }

    public class MAXiImageTranNoDTO
    {
        [DefaultValue("")]
        public string vWorkSpaceID { get; set; }
        [DefaultValue("")]
        public string vSubjectId { get; set; }
        [DefaultValue("")]
        public int iNodeId { get; set; }
        public string vModalityNo { get; set; }
    }

    public class CertMAXiImageTranNoDTO
    {
        [DefaultValue("")]
        public string vWorkSpaceID { get; set; }
        public string vModalityNo { get; set; }
    }

    public class ProjectLockDetailDTO
    {
        [DefaultValue("")]
        public string vWorkspaceId { get; set; }
    }

    public class ProjectFreezerDetailDTO
    {
        [DefaultValue("")]
        public string vWorkspaceId { get; set; }
    }

    public class CheckVisitIsReviewedGetMAXiImageTranNoDTO
    {
        [DefaultValue("")]
        public string vWorkSpaceID { get; set; }
        [DefaultValue("")]
        public string vSubjectId { get; set; }
        [DefaultValue("")]
        public string vParentActivityId { get; set; }
        [DefaultValue("")]
        public string iParentNodeId { get; set; }
        [DefaultValue("")]
        public string vActivityId { get; set; }
        [DefaultValue("")]
        public string iNodeId { get; set; }
        [DefaultValue("")]
        public int iModalityNo { get; set; }
    }

    public class DashboardDetailDTO
    {
        [DefaultValue("")]
        public string vWorkSpaceID { get; set; }
        [DefaultValue("")]
        public string iPeriod { get; set; }
        [DefaultValue("")]
        public string vUserTypeCode { get; set; }
        [DefaultValue("")]
        public string iUserId { get; set; }
    }

    public class ChangePasswordDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ID { get; set; }
        public string NewPassword { get; set; }
        public string ConfnewPassword { get; set; }
        public int iModifyBy { get; set; }
        public string sessionUsername { get; set; }
        public string sessionPassword { get; set; }
    }

    public class OtpDTO
    {
        public string Username { get; set; }
        public string UserId { get; set; }
        public string Otp { get; set; }
        public string Password { get; set; }
        public string vUserTypeCode { get; set; }
        public string vLocationCode { get; set; }
    }

    public class MISubmitMIFinalLesionDTO
    {
        [DefaultValue("")]
        public string vWorkspaceId { get; set; }
        [DefaultValue("")]
        public string vSubjectId { get; set; }
        [DefaultValue("")]
        public string vParentWorkSpaceId { get; set; }
        [DefaultValue("")]
        public string vParentActivityId { get; set; }
        [DefaultValue("")]
        public string iParentNodeId { get; set; }
        [DefaultValue("")]
        public string vActivityName { get; set; }
        [DefaultValue("")]
        public string vSubActivityName { get; set; }
        [DefaultValue("")]
        public string vActivityId { get; set; }
        [DefaultValue("")]
        public string iNodeId { get; set; }
        [DefaultValue("")]
        public string vPeriodId { get; set; }
        [DefaultValue("")]
        public string iMySubjectNo { get; set; }
        [DefaultValue("")]
        public string vLesionType { get; set; }
        [DefaultValue("")]
        public string vLesionSubType { get; set; }
        [DefaultValue("")]
        public int? iImgTransmittalHdrId { get; set; }
        [DefaultValue("")]
        public int? iImgTransmittalDtlId { get; set; }
        [DefaultValue("")]
        public string cRadiologist { get; set; }
        [DefaultValue("")]
        public string iImageTranNo { get; set; }
        [DefaultValue("")]
        public string cSaveStatusFlagValidation { get; set; }
        public List<DicomAnnotation> DicomAnnotationDtl { get; set; }
        public string DicomAnnotationDetail { get; set; }
        public List<MIFinalLesionImageList> MIFinalLesionImageList { get; set; }
        public int? iImageMode { get; set; }
        public int? UserID { get; set; }
        public string vLocationCode { get; set; }
        public string Otp { get; set; }
        [DefaultValue("")]
        public string vOperationCode { get; set; }
    }

    public class ExMsgInfo
    {
        public string vNotificationType { get; set; }
        public string vSubject { get; set; }
        public string vBody { get; set; }
        public string vFromEmailId { get; set; }
        public string vToEmailId { get; set; }
        public string vCCEmailId { get; set; }
        public string vBCCEmailId { get; set; }
        public string vAttachment { get; set; }
        public string iCreatedBy { get; set; }
        public string dCreatedDate { get; set; }
        public string vPhoneNo { get; set; }
        public string cIsSent { get; set; }
        public string dSentDate { get; set; }
        public string vRemarks { get; set; }
    }

}