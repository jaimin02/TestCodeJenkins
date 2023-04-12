using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace MedicalImaging.Models
{
    public class MIDicom
    {
        [DefaultValue("")]
        public string iImgTransmittalHdrId { get; set; }
        [DefaultValue("")]
        public string iImgTransmittalDtlId { get; set; }
        [DefaultValue("")]
        public string vWorkspaceId { get; set; }
        [DefaultValue("")]
        public string vProjectNo { get; set; }
        [DefaultValue("")]
        public string vSubjectId { get; set; }
        [DefaultValue("")]
        public string iNodeId { get; set; }
        [DefaultValue("")]
        public string iModalityNo { get; set; }
        [DefaultValue("")]
        public string iAnatomyNo { get; set; }
        [DefaultValue("")]
        public string iImageStatus { get; set; }
    }

    public class MISaveDicom
    {
        [DefaultValue("")]
        public string DicomImage { get; set; }
        [DefaultValue("")]
        public string DicomName { get; set; }
        [DefaultValue("")]
        public int stackLength { get; set; }
        [DefaultValue("")]
        public string vWorkspaceId { get; set; }
        [DefaultValue("")]
        public string vProjectNo { get; set; }
        [DefaultValue("")]
        public string vSubjectId { get; set; }
        [DefaultValue("")]
        public string iNodeId { get; set; }
        [DefaultValue("")]
        public string iModalityNo { get; set; }
        [DefaultValue("")]
        public string iAnatomyNo { get; set; }
        [DefaultValue("")]
        public string iuserid { get; set; }
        [DefaultValue("")]
        public string iImgTransmittalHdrId { get; set; }
        [DefaultValue("")]
        public string iImgTransmittalDtlId { get; set; }   
    }

    public class MIFinalLesionCRFData
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
        public string vLesionType { get; set; }
        [DefaultValue("")]
        public string vLesionSubType { get; set; }
        [DefaultValue("")]
        public int? iImgTransmittalHdrId { get; set; }
        [DefaultValue("")]
        public int? iImgTransmittalDtlId { get; set; }
        [DefaultValue("")]
        public string cRadiologist { get; set;  }
        [DefaultValue("")]
        public string iImageTranNo { get; set; }
        
    }

    public class MIFinalLesionData
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

    public class MIFinalLesionImageData
    {
        [DefaultValue("")]
        public int? iImgTransmittalHdrId { get; set; }
        [DefaultValue("")]
        public int? iImgTransmittalDtlId { get; set; }
        [DefaultValue("")]
        public int? UserID { get; set; }
        [DefaultValue("")]
        public int? iImageMode { get; set; }
        public List<MIFinalLesionImageList> _MIFinalLesionImageList { get; set; }
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
    }

    public class MISessionManagement
    {
        public string ManageSession { get; set; }
    }

    public class MISkipLesionCRFData
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
        public string vLesionType { get; set; }
        [DefaultValue("")]
        public string vLesionSubType { get; set; }
        [DefaultValue("")]
        public int? iImgTransmittalHdrId { get; set; }
        [DefaultValue("")]
        public int? iImgTransmittalDtlId { get; set; }
        [DefaultValue("")]
        public string cRadiologist { get; set; }
    }

    public class MIFinalSkipLesionCRFData
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

    public class MIDicomAnnotation
    {
        [DefaultValue("")]
        public string vFileName { get; set; }
        [DefaultValue("")]
        public string vServerPath { get; set; }
        [DefaultValue("")]
        public string nvDicomAnnotation { get; set; }
        [DefaultValue("")]
        public int? UserID { get; set; }
    }

    public class MISubmitMIFinalLesionData
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

    public class MISkipLesionCRFDetail
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
        public string vLocationCode { get; set; }
        public string Otp { get; set; }
        [DefaultValue("")]
        public string vOperationCode { get; set; }
       
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

}