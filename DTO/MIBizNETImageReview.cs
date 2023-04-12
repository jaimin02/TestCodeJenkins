using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DTO
{
    public class MIBizNETSaveImage
    {
        [DefaultValue("")]
        public string vWorkspaceId { get; set; }
        [DefaultValue("")] 
        public string vProjectNo { get; set; }
        [DefaultValue("")]
        public string vSubjectId { get; set; }
        [DefaultValue("")]
        public string iNodeId { get; set; }
    }


    public class MIBizNETSaveImageDTO
    {
        [DefaultValue("")]
        public int? iImageMode { get; set; } 
        [DefaultValue("")]
        public int? iImgTransmittalHdrId { get; set; }
        [DefaultValue("")]
        public int? iImgTransmittalDtlId { get; set; }
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
        public int? iModifyBy { get; set; }

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
}
