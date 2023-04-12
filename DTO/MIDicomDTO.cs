using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DTO
{
    public class MIDicomDTO
    {
    }


    public class MIFinalLesionDataDTO
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
        public int? iImgTransmittalHdrId { get; set; }
        [DefaultValue("")]
        public int? iImgTransmittalDtlId { get; set; }
        [DefaultValue("")]
        public int? UserID { get; set; }
        [DefaultValue("")]
        public int? iImageMode { get; set; }

        [DefaultValue("")]
        public string vLesionType { get; set; }
        [DefaultValue("")]
        public string vLesionSubType { get; set; }
        [DefaultValue("")]
        public string cRadiologist { get; set; }
        [DefaultValue("")]
        public string iImageTranNo { get; set; }



        public List<MIFinalLesionImageList> MIFinalLesionImageList { get; set; } 
    }

    public class MIFinalSkipLesionDataDTO
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
        public int? UserID { get; set; }    
        [DefaultValue("")]
        public string vLesionType { get; set; }
        [DefaultValue("")]
        public string vLesionSubType { get; set; }
        [DefaultValue("")]
        public string cRadiologist { get; set; }
    }

    
    public class MIFinalLesionImageDataDTO
    {
        [DefaultValue("")]
        public int? iImgTransmittalHdrId { get; set; }
        [DefaultValue("")]
        public int? iImgTransmittalDtlId { get; set; }
        [DefaultValue("")]
        public int? UserID { get; set; }
        [DefaultValue("")]
        public int? iImageMode { get; set; }
        public List<MIFinalLesionImageList> MIFinalLesionImageList { get; set; }
    }

    public class MIFinalLesionImageList
    {
        [DefaultValue("")]
        public string vFileName { get; set; }
        [DefaultValue("")]
        public string vServerPath { get; set; }
        [DefaultValue("")]
        public string vFileType { get; set; }
        [DefaultValue("")]
        public string vSize { get; set; }
        [DefaultValue("")]
        public DateTime dScheduledDate { get; set; }
        [DefaultValue("")]
        public string vImgSliceLocation { get; set; }
        [DefaultValue("")]
        public string vImgModality { get; set; }
        [DefaultValue("")]
        public string vSeriesNumber { get; set; }
        [DefaultValue("")]
        public string vAcquisitionNumber { get; set; }     
        [DefaultValue("")]
        public string vSeriesInstanceUID { get; set; }
        [DefaultValue("")]
        public string vSeriesDescription { get; set; }
        [DefaultValue("")]
        public string iStudyID { get; set; }
        [DefaultValue("")]
        public string iInstanceNumber { get; set; }
        [DefaultValue("")]
        public string vModalityNo { get; set; }
        [DefaultValue("")]
        public string vAnatomyNo { get; set; }
        public int? iImgSeqNo { get; set; }
    }
    

    //for Lesion save in temp table
    public class MIFinalLesionDetailDTO
    {

        [DefaultValue("")]
        public string vWorkspaceId { get; set; }
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
        public string iModifyBy { get; set; }
        [DefaultValue("")]
        public string DATAMODE { get; set; }
        [DefaultValue("")]
        public string iMySubjectNo { get; set; }
        [DefaultValue("")]
        public string ScreenNo { get; set; }
        //Table Type Parameter For SQL
        [DefaultValue("")]
        public string MedEx_Result { get; set; }
        [DefaultValue("")]
        public string ParentWorkSpaceId { get; set; }
        [DefaultValue("")]
        public string PeriodId { get; set; }
        [DefaultValue("")]
        public string vActivityName { get; set; }
        [DefaultValue("")]
        public string vSubActivityName { get; set; }


        public List<MIFinalLesionDetailDataDTO> MIFinalLesionDetailDataDTO { get; set; }
    }

    public class MIFinalLesionDetailDataDTO
    {
        [DefaultValue("")]
        public string vMedExCode { get; set; }
        [DefaultValue("")]
        public string vMedExDesc { get; set; }
        [DefaultValue("")]
        public string vMedExResult { get; set; }
        [DefaultValue("")]
        public string vMedExType { get; set; }
        [DefaultValue("")]
        public string vModificationRemark { get; set; }
    }

    public class MILesionStatistics
    {
        [DefaultValue("")]
        public string vParentWorkSpaceId { get; set; }
        [DefaultValue("")]
        public string vWorkspaceId { get; set; }
        [DefaultValue("")]
        public string vSubjectId { get; set; }
        [DefaultValue("")]
        public string iMySubjectNo { get; set; }
        [DefaultValue("")]
        public string ScreenNo { get; set; }
        [DefaultValue("")]
        public string vMedExDesc { get; set; }
        [DefaultValue("")]
        public string cSaveStatus { get; set; }
        [DefaultValue("")]
        public string vActivityName { get; set; }
        [DefaultValue("")]
        public string vSubActivityName { get; set; }
        [DefaultValue("")]
        public string Type { get; set; }
       
 
    }

    public class MILesionStatisticsDetails
    {
        [DefaultValue("")]
        public string MODE { get; set; }
        [DefaultValue("")]
        public string vParentWorkSpaceId { get; set; }
        [DefaultValue("")]
        public string vWorkspaceId { get; set; }
        [DefaultValue("")]
        public string vSubjectId { get; set; }
        [DefaultValue("")]
        public string iMySubjectNo { get; set; }
        [DefaultValue("")]
        public string Radiologist { get; set; }
        [DefaultValue("")]
        public string ScreenNo { get; set; }
        [DefaultValue("")]
        public string vActivity { get; set; }
        [DefaultValue("")]
        public string vSubActivity { get; set; }
        [DefaultValue("")]
        public string vParentActivityId { get; set; }
        [DefaultValue("")]
        public string iParentNodeId { get; set; }
        [DefaultValue("")]
        public string vActivityId { get; set; }
        [DefaultValue("")]
        public string iNodeId { get; set; }
        [DefaultValue("")]
        public string cSaveStatus { get; set; }
    }

    public class MI_NLDetails
    {
        [DefaultValue("")]
        public string MODE { get; set; }
        [DefaultValue("")]
        public string vParentWorkSpaceId { get; set; }
        [DefaultValue("")]
        public string vWorkspaceId { get; set; }
        [DefaultValue("")]
        public string vSubjectId { get; set; }
        [DefaultValue("")]
        public string iMySubjectNo { get; set; }
        [DefaultValue("")]
        public string Radiologist { get; set; }
        [DefaultValue("")]
        public string ScreenNo { get; set; }
        [DefaultValue("")]
        public string vActivity { get; set; }
        [DefaultValue("")]
        public string vSubActivity { get; set; }
        [DefaultValue("")]
        public string vParentActivityId { get; set; }
        [DefaultValue("")]
        public string iParentNodeId { get; set; }
        [DefaultValue("")]
        public string vActivityId { get; set; }
        [DefaultValue("")]
        public string iNodeId { get; set; }
        [DefaultValue("")]
        public string cSaveStatus { get; set; }
    }

    public class DirectoryDTO
    {
        [DefaultValue("")]
        public string vFilePath { get; set; }
        [DefaultValue("")]
        public string vFileName { get; set; }
        [DefaultValue("")]
        public string vFilePathMark { get; set; }
        [DefaultValue("")]
        public string vFilePathEligibilityReview { get; set; }
    }

    public class MIFinalLesionDetailsDataDTO
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
        public int? iImgTransmittalHdrId { get; set; }
        [DefaultValue("")]
        public int? iImgTransmittalDtlId { get; set; }
        [DefaultValue("")]
        public int? UserID { get; set; }
        [DefaultValue("")]
        public int? iImageMode { get; set; }

        [DefaultValue("")]
        public string vLesionType { get; set; }
        [DefaultValue("")]
        public string vLesionSubType { get; set; }
        [DefaultValue("")]
        public string cRadiologist { get; set; }
        [DefaultValue("")]
        public string iImageTranNo { get; set; }

        public List<MIFinalLesionImageList> MIFinalLesionImageList { get; set; }

        [DefaultValue("")]
        public string cSaveStatusFlagValidation { get; set; }

        public List<DicomAnnotation> DicomAnnotationDtl { get; set; }

        [DefaultValue("")]
        public string vModificationRemark { get; set; }
        [DefaultValue("")]
        public string vModalityDesc { get; set; }
        [DefaultValue("")]
        public string vAnatomyDesc { get; set; }
    }

    public class DicomAnnotation
    {        
        [DefaultValue("")]
        public string vFileName { get; set; }
        [DefaultValue("")]
        public string vServerPath { get; set; }
        [DefaultValue("")]
        public string vAnnotationType { get; set; }
        [DefaultValue("")]
        public string nvDicomAnnotation { get; set; }
    }

    public class MIFinalSkipLesionDetailDTO
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
        public int? UserID { get; set; }
        
        
    }

}
