using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DTO
{
    public class MIImageTransmittalDTO
    {
    }

    public class MIImageTransmittalHdr
    {
        [DefaultValue("")]
        public int? iImgTransmittalHdrId_int { get; set; }
        [DefaultValue("")]
        public string iImgTransmittalHdrId { get; set; }
        [DefaultValue("")]
        public string vWorkspaceId { get; set; }
        [DefaultValue("")]
        public string vProjectNo { get; set; }
        [DefaultValue("")]
        public string vSubjectId { get; set; }
        [DefaultValue("")]
        public string vRandomizationNo { get; set; }
        [DefaultValue("")]
        public int? iNodeId_int { get; set; }
        [DefaultValue("")]
        public string iNodeId { get; set; }
        [DefaultValue("")]
        public string cDeviation { get; set; }
        [DefaultValue("")]
        public string nvDeviationReason { get; set; }
        [DefaultValue("")]
        public string nvInstructions { get; set; }
        [DefaultValue("")]
        public string vRemark { get; set; }
        [DefaultValue("")]
        public string iModifyBy { get; set; }
        [DefaultValue("")]
        public string cStatusIndi { get; set; }
        [DefaultValue("")]
        public string DATAOPMODE { get; set; }       
        [DefaultValue("")]
        public int? iImageStatus { get; set; }
        //[DefaultValue("")]
        public DateTime? FromDate { get; set; }
        //[DefaultValue("")]
        public DateTime? ToDate { get; set; } 


        

    }

    public class MIImageTransmittalDtl
    {
        [DefaultValue("")]
        public int? iImgTransmittalDtlId_int { get; set; }
        [DefaultValue("")]
        public string iImgTransmittalDtlId { get; set; }
        [DefaultValue("")]
        public int? iImgTransmittalHdrId_int { get; set; }
        [DefaultValue("")]
        public string iImgTransmittalHdrId { get; set; }
        [DefaultValue("")]
        public string vWorkspaceId { get; set; }
        [DefaultValue("")]
        public string vSubjectId { get; set; }
        [DefaultValue("")]
        public int? iModalityNo_int { get; set; }
        [DefaultValue("")]
        public string iModalityNo { get; set; }
        [DefaultValue("")]
        public int? iAnatomyNo_int { get; set; }
        [DefaultValue("")]
        public string iAnatomyNo { get; set; }
        [DefaultValue("")]
        public string cIVContrast { get; set; }
        [DefaultValue("")]
        public string dExaminationDate { get; set; }
        [DefaultValue("")]
        public string iNoImages { get; set; }
        [DefaultValue("")]
        public string vRemark { get; set; }
        [DefaultValue("")]
        public string iModifyBy { get; set; }
        [DefaultValue("")]
        public string cStatusIndi { get; set; }
        [DefaultValue("")]
        public string DATAOPMODE { get; set; }
        [DefaultValue("")]
        public string iImageStatus { get; set; } 


    }

    public class MIImageTransmittalNew
    {
        [DefaultValue("")]
        public int? iImgTransmittalHdrId { get; set; }
        [DefaultValue("")]
        public int? iImgTransmittalDtlId { get; set; }
        [DefaultValue("")]
        public int? UserID { get; set; }
        [DefaultValue("")]
        public int? iImageMode { get; set; }
        public List<MIImageTransmittalImgList> _SaveImageTransmittal { get; set; }
    }

    //public class MIImageTransmittal
    //{
    //    [DefaultValue("")]
    //    public int? iImgTransmittalHdrId { get; set; }
    //    [DefaultValue("")]
    //    public int? iImgTransmittalDtlId { get; set; }
    //    [DefaultValue("")]
    //    public int? iImgTransmittalImgDtlId { get; set; }
    //    [DefaultValue("")]
    //    public string vWorkspaceId { get; set; }
    //    [DefaultValue("")]
    //    public string vProjectNo { get; set; }
    //    [DefaultValue("")]
    //    public string vSubjectId { get; set; }
    //    [DefaultValue("")]
    //    public string vRandomizationNo { get; set; }
    //    [DefaultValue("")]
    //    public string iNodeId { get; set; }
    //    [DefaultValue("")]
    //    public string cDeviation { get; set; }
    //    [DefaultValue("")]
    //    public string nvDeviationReason { get; set; }
    //    [DefaultValue("")]
    //    public string nvInstructions { get; set; }
    //    [DefaultValue("")]
    //    public string iModalityNo { get; set; }
    //    [DefaultValue("")]
    //    public string iAnatomyNo { get; set; }
    //    [DefaultValue("")]
    //    public string cIVContrast { get; set; }
    //    [DefaultValue("")]
    //    public string dExaminationDate { get; set; }
    //    [DefaultValue("")]
    //    public string iNoImages { get; set; }
    //    [DefaultValue("")]
    //    public string vRemark { get; set; }
    //    [DefaultValue("")]
    //    public string ModifyBy { get; set; }
    //    [DefaultValue("")]
    //    public string iImageMode { get; set; }
    //    public List<MIImageTransmittalImgList> _SaveImageTransmittal { get; set; }
    //}

    public class MIImageTransmittal
    {
        [DefaultValue("")]
        public int? iImgTransmittalHdrId { get; set; }
        [DefaultValue("")]
        public int? iImgTransmittalDtlId { get; set; }
        [DefaultValue("")]
        public int? iImgTransmittalImgDtlId { get; set; }
        [DefaultValue("")]
        public string vWorkspaceId { get; set; }
        [DefaultValue("")]
        public string vProjectNo { get; set; }
        [DefaultValue("")]
        public string vSubjectId { get; set; }
        [DefaultValue("")]
        public string vRandomizationNo { get; set; }
        [DefaultValue("")]
        public string vMySubjectNo { get; set; }
        [DefaultValue("")]
        public string vDOB { get; set; }
        [DefaultValue("")]
        public string iNodeId { get; set; }
        [DefaultValue("")]
        public string cDeviation { get; set; }
        [DefaultValue("")]
        public string nvDeviationReason { get; set; }
        [DefaultValue("")]
        public string nvInstructions { get; set; }
        [DefaultValue("")]
        public string iModalityNo { get; set; }
        [DefaultValue("")]
        public string iAnatomyNo { get; set; }
        [DefaultValue("")]
        public string cIVContrast { get; set; }
        //[DefaultValue("")]
        public DateTime dExaminationDate { get; set; }
        [DefaultValue("")]
        public string iNoImages { get; set; }
        [DefaultValue("")]
        public string vRemark { get; set; }
        [DefaultValue("")]
        public string iModifyBy { get; set; }
        [DefaultValue("")]
        public string iImageMode { get; set; }
        [DefaultValue("")]
        public string vAnatomyNo { get; set; }
        [DefaultValue("")]
        public string vParentWorkspaceId { get; set; }
        [DefaultValue("")]
        public string vParentActivityId { get; set; }        
        public int? iParentNodeId { get; set; }
        [DefaultValue("")]
        public string vActivityId { get; set; }
        [DefaultValue("")]
        public string cOralContrast { get; set; }
        [DefaultValue("")]
        public string vModalityNo { get; set; }
        [DefaultValue("")]
        public string vModalityDesc { get; set; }
        [DefaultValue("")]
        public string vAnatomyDesc { get; set; }
        public List<MIImageTransmittalImgList> _SaveImageTransmittal { get; set; }
    }

    public class MIImageTransmittalImgList
    {
        [DefaultValue("")]
        public string vFileName { get; set; }
        [DefaultValue("")]
        public string vServerPath { get; set; }
        [DefaultValue("")]
        public string vFileType { get; set; }
        [DefaultValue("")]
        public string vSize { get; set; }
        //[DefaultValue("")]
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
        public string  vSeriesInstanceUID { get; set; }
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

    public class MIImageTransmittalImgDtl
    {
        [DefaultValue("")]
        public int? iImgTransmittalImgDtlId_int { get; set; }
        [DefaultValue("")]
        public int? iImgTransmittalDtlId_int { get; set; }
        [DefaultValue("")]
        public int? iImgTransmittalHdrId_int { get; set; }
        [DefaultValue("")]
        public int? iImageStatus_int { get; set; }
    }

    public class Save_CRFHdrDtlSubDtlList
    {
        public int? iCRFTempDetail { get; set; }
        [DefaultValue("")]
        public string vWorkspaceId { get; set; }
        [DefaultValue("")]
        public string vParentWorkSpaceId { get; set; }
        [DefaultValue("")]
        public string vPeriodId { get; set; }
        [DefaultValue("")]
        public string vSubjectId { get; set; }
        [DefaultValue("")]
        public string iMySubjectNo { get; set; }
        [DefaultValue("")]
        public string ScreenNo { get; set; }
        [DefaultValue("")]
        public string vActivityId { get; set; }
        [DefaultValue("")]
        public string iNodeId { get; set; }
        [DefaultValue("")]
        public string iNodeIndex { get; set; }
        //[DefaultValue("")]
        //public string vPeriodId { get; set; }
        public int? iRepeatNo { get; set; }
        [DefaultValue("")]
        public string vMedExCode { get; set; }
        [DefaultValue("")]
        public string vMedExDesc { get; set; }
        [DefaultValue("")]
        public DateTime dMedExDatetime { get; set; }
        [DefaultValue("")]
        public string vMedExResult { get; set; }
        public int? iRepetationNo { get; set; }
        public int? iSeqNo { get; set; }
        [DefaultValue("")]
        public string vRemarks { get; set; }
        public int? iModifyBy { get; set; }
        public DateTime dModifyOn { get; set; }
        [DefaultValue("")]
        public string cStatusIndi { get; set; }
    }

    public class ImageTransmittalImgDtl
    {
        public string vServerPath { get; set; }
        public string vFileName { get; set; }
        public string Content { get; set; }
    }

    public class DicomTagChange
    {
        public string fileName { get; set; }
        public string fromPath { get; set; }
        public string subjectValue { get; set; }
    }

}
