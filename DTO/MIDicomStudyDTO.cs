using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DTO
{
    public class MIDicomStudyDTO
    {      
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

    public class MI_DataSaveStatus
    {
        [DefaultValue("")]
        public string vParentWorkSpaceId { get; set; }
        [DefaultValue("")]
        public string vWorkspaceId { get; set; }        
        [DefaultValue("")]
        public string vSubjectId { get; set;}
        [DefaultValue("")]
        public string cRadiologist { get; set;}
        [DefaultValue("")]
        public string vActivityId { get; set;}
        [DefaultValue("")]
        public string iNodeId { get; set; }
        [DefaultValue("")]
        public string vSubActivityId { get; set; }
        [DefaultValue("")]
        public string iSubNodeId { get; set; }
        [DefaultValue("")]
        public string vMySubjectNo { get; set; }
        [DefaultValue("")]
        public string vOperationcode { get; set; }
        [DefaultValue(0)]
        public int iImgTransmittalHdrId { get; set; }
        [DefaultValue(0)]
        public int iImgTransmittalDtlId { get; set; }
        [DefaultValue("")]
        public string iUserId { get; set; }
        [DefaultValue("")]
        public string iParentNodeId { get; set; }
    }

}