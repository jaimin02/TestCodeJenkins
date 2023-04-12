using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class CertificateDTO
    {
        [DefaultValue("")]
        public int? iCertificateMasterId { get; set; }
        [DefaultValue("")]
        public int? iCertificateMasterImgDtlId { get; set; }
        [DefaultValue("")]
        public string vWorkspaceId { get; set; }
        [DefaultValue("")]
        public string vSubjectId { get; set; }
        [DefaultValue("")]
        public string iModalityNo { get; set; }
        [DefaultValue("")]
        public string iAnatomyNo { get; set; }
        [DefaultValue("")]
        public string nvInstructions { get; set; }
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
        public string dModifyon { get; set; }
        [DefaultValue("")]
        public string cStatusIndi { get; set; }
        [DefaultValue("")]
        public string iImageMode { get; set; }
        [DefaultValue("")]
        public string vAnatomyNo { get; set; }

        [DefaultValue("")]
        public string cOralContrast { get; set; }
        [DefaultValue("")]
        public string vModalityNo { get; set; }
        [DefaultValue("")]
        public string vModalityDesc { get; set; }
        [DefaultValue("")]
        public string vAnatomyDesc { get; set; }
        public List<MIUploadCertificateList> _SaveCertificate { get; set; }
        public string iTranNo { get; set; }
        [DefaultValue("")]
        public string vServerPath { get; set; }
        public string iImageTranNo { get; set; }
    }

    public class MIUploadCertificateList
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

    public class CertificateMasterImgDtl
    {
        public string iCertificateMasterImgDtlId { get; set; }
        public string iCertificateMasterID { get; set; }
        public string vWorkspaceId { get; set; }
        public string vSubjectId { get; set; }
        public string iModalityNo { get; set; }
        public string iAnatomyNo { get; set; }
        public string nvInstructions { get; set; }
        public string cIVContrast { get; set; }
        public DateTime dExaminationDate { get; set; }
        public string iNoImages { get; set; }
        public string vRemark { get; set; }
        public string iModifyBy { get; set; }
        public string dModifyon { get; set; }
        public string cStatusIndi { get; set; }
        public string iImageMode { get; set; }
        public string vAnatomyNo { get; set; }
        public string cOralContrast { get; set; }
        public string vModalityNo { get; set; }
        public string vModalityDesc { get; set; }
        public string vAnatomyDesc { get; set; }
        public string iTranNo { get; set; }
        public string vServerPath { get; set; }
        public string vFileName { get; set; }
        public string Content { get; set; }
    }

    public class CertDicomTagChange
    {
        public string fileName { get; set; }
        public string fromPath { get; set; }
    }
}
