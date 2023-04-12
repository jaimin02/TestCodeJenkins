using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DTO
{
    public class LesionDTO
    {
        [DefaultValue("")]
        public string vWorkspaceId { get; set; }
        [DefaultValue("")]
        public string vActivityId { get; set; }
        [DefaultValue("")]
        public string iNodeId { get; set; }

    }

    public class LesionMarkDTO
    {
        [DefaultValue("")]
        public string vWorkspaceId { get; set; }
        [DefaultValue("")]
        public string vActivityId { get; set; }
        [DefaultValue("")]
        public string iNodeId { get; set; }
        [DefaultValue("")]
        public string vSubjectId { get; set; }  
        [DefaultValue("")]
        public string iMySubjectNo { get; set; }
        [DefaultValue("")]
        public string ScreenNo { get; set; }
        [DefaultValue("")]
        public string vActivityName { get; set; }
        [DefaultValue("")]
        public string vSubActivityName { get; set; }
    }

    public class LesionDetailDTO
    {        
        [DefaultValue("")]
        public string vWorkspaceId { get; set; }
        [DefaultValue("")]
        public string vSubjectId { get; set; }
        [DefaultValue("")]
        public string vActivityId { get; set; }
        [DefaultValue("")]
        public string iNodeId { get; set; }
        [DefaultValue("")]
        public string iModifyBy { get; set; }
        [DefaultValue("")]
        public string DATAMODE { get; set; } 
        [DefaultValue("")]
        public string iMySubjectNo { get; set; } 
        [DefaultValue("")]
        public string ScreenNo { get; set; }
        [DefaultValue("")]
        public string MedEx_Result { get; set; }
        [DefaultValue("")]
        public string ParentWorkSpaceId { get; set; }
        [DefaultValue("")]
        public string PeriodId { get; set; }

        
        public List<LesionDataDTO> _LesionDataDTO { get; set; }
    }

    public class LesionDataDTO
    {
        [DefaultValue("")]
        public string vMedExCode { get; set; }
        [DefaultValue("")]
        public string vMedExDesc { get; set; }
        [DefaultValue("")]
        public string vMedExResult { get; set; }            
    }

    public class LesionCRFDTO
    {
        [DefaultValue("")]
        public string ParentWorkSpaceId { get; set; }
        [DefaultValue("")]
        public string vWorkspaceId { get; set; }        
        [DefaultValue("")]
        public string vSubjectId { get; set; }          
        [DefaultValue("")]
        public string iMySubjectNo { get; set; }
        [DefaultValue("")]
        public string ScreenNo { get; set; }
        [DefaultValue("")]
        public string PeriodId { get; set; }
        [DefaultValue("")]
        public string ActivityID { get; set; }
        [DefaultValue("")]
        public string NodeID { get; set; }
    }

    public class Save_CRFHdrDtlSubDtlDTO
    {
        [DefaultValue("")]
        public string vWorkspaceId { get; set; }
        [DefaultValue("")]
        public string vSubjectId { get; set; }
        [DefaultValue("")]
        public string vParentWorkSpaceId { get; set; }
        [DefaultValue("")]
        public string vActivityId { get; set; }
        [DefaultValue("")]
        public string iNodeId { get; set; }
        [DefaultValue("")]
        public string vPeriodId { get; set; }
        [DefaultValue("")]
        public string iMySubjectNo { get; set; }       
    }

    public class LesionDetailsDATA_DTO
    {
        [DefaultValue("")]
        public string vWorkspaceId { get; set; }
        [DefaultValue("")]
        public string vSubjectId { get; set; }
        [DefaultValue("")]
        public string vParentWorkSpaceId { get; set; }
        [DefaultValue("")]
        public string vActivityId { get; set; }
        [DefaultValue("")]
        public string iNodeId { get; set; }
        [DefaultValue("")]
        public string vPeriodId { get; set; }
        [DefaultValue("")]
        public string iMySubjectNo { get; set; }
        [DefaultValue("")]
        public string cSaveStatusFlagValidation { get; set; }
    }

}
