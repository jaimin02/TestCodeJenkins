using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.IO;

namespace MedicalImaging.Models
{
    public class MICommon
    {
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

    public class MIDicomProjectDetailDTO
    {
        [DefaultValue("")]
        public string vWorkSpaceID { get; set; }
        [DefaultValue("")]
        public string iPeriod { get; set; }
        [DefaultValue("")]
        public string iParentNodeId { get; set; }
        [DefaultValue("")]
        public string vUserTypeCode { get; set; }
        [DefaultValue("")]
        public string vSubjectId { get; set; }
        [DefaultValue("")]
        public string iUserId { get; set; }
        [DefaultValue("")]
        public string MODE { get; set; }
        [DefaultValue("")]
        public string vActivityName { get; set; }
    }

    public class MI_DataSaveStatus
    {
        [DefaultValue("")]
        public string vParentWorkSpaceId { get; set; }
        [DefaultValue("")]
        public string vWorkspaceId { get; set; }
        [DefaultValue("")]
        public string vSubjectId { get; set; }
        [DefaultValue("")]
        public string cRadiologist { get; set; }
        [DefaultValue("")]
        public string vActivityId { get; set; }
        [DefaultValue("")]
        public string iNodeId { get; set; }
        [DefaultValue("")]
        public string vSubActivityId { get; set; }
        [DefaultValue("")]
        public string iSubNodeId { get; set; }
        [DefaultValue("")]
        public string vMySubjectNo { get; set; }
        [DefaultValue("")]
        public string vOperationCode { get; set; }
        [DefaultValue(0)]
        public int iImgTransmittalHdrId { get; set; }
        [DefaultValue(0)]
        public int iImgTransmittalDtlId { get; set; }
        [DefaultValue("")]
        public string iUserId { get; set; }
        [DefaultValue("")]
        public string iParentNodeId { get; set; }
    }

    public class MICRFDataEntryStatus
    {
        [DefaultValue("")]
        public string MODE { get; set; }
        [DefaultValue("")]
        public string vParentWorkSpaceId { get; set; }
        [DefaultValue("")]
        public string iMySubjectNo { get; set; }
        [DefaultValue("")]
        public string ScreenNo { get; set; }
        [DefaultValue("")]
        public string Radiologist { get; set; }
        [DefaultValue("")]
        public string Activity { get; set; }
        [DefaultValue("")]
        public string vWorkspaceId { get; set; }
        [DefaultValue("")]
        public string vSubjectId { get; set; }
        [DefaultValue("")]
        public string cRadiologist { get; set; }
        [DefaultValue("")]
        public string vActivityName { get; set; }
        //[DefaultValue("")]
        //public string returnValue { get; set; }
        //[DefaultValue("")]
        //public string returnResult { get; set; }

    }

    public class MISubjectStudyDTO
    {
        [DefaultValue("")]
        public string vWorkspaceId { get; set; }
        [DefaultValue("")]
        public string vSubjectId { get; set; }
        [DefaultValue("")]
        public string iNodeId { get; set; }
        [DefaultValue("")]
        public string cRadiologist { get; set; }
        [DefaultValue("")]
        public string MODE { get; set; }
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

    public class GetMyProjectCompletionListDTO
    {
        [DefaultValue("")]
        public String contextKey { get; set; }
        [DefaultValue("")]
        public String vProjectTypeCode { get; set; }
        [DefaultValue("")]
        public String prefixText { get; set; }
    }

    public class MIStatisticReportDTO
    {
        [DefaultValue("")]
        public string vWorkspaceId { get; set; }

        [DefaultValue("")]
        public string vBtnRadioName { get; set; }

        [DefaultValue(0)]
        public int imode { get; set; }
        

    }

    public class MIModalityDTO
    {
        [DefaultValue(0)]
        public string nModalityNo { get; set; }
        [DefaultValue("")]
        public string vModalityDesc { get; set; }
        [DefaultValue("")]
        public string vRemarks { get; set; }
        [DefaultValue("")]
        public string iModifyBy { get; set; }
        [DefaultValue("")]
        public string dModifyOn { get; set; }
        [DefaultValue("")]
        public string cStatusIndi { get; set; }
    }

    public class MIAnatomyDTO
    {
        [DefaultValue(0)]
        public string nAnatomyNo { get; set; }
        [DefaultValue("")]
        public string vAnatomyDesc { get; set; }
        [DefaultValue("")]
        public string vRemarks { get; set; }
        [DefaultValue("")]
        public string iModifyBy { get; set; }
        [DefaultValue("")]
        public string dModifyOn { get; set; }
        [DefaultValue("")]
        public string cStatusIndi { get; set; }
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

    public class MiReports
    {
        
            [DefaultValue("")]
        public string vParentWorkSpaceId { get; set; }
        [DefaultValue("")]
        public string vWorkspaceId { get; set; }

        [DefaultValue(0)]
        public int imode { get; set; }
    }


    public class FileInfo
    {
        public int FileId
        {
            get;
            set;
        }
        public string FileName
        {
            get;
            set;
        }
        public string FilePath
        {
            get;
            set;
        }
    }



}