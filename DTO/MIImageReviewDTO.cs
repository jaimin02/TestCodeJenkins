using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DTO
{
    public class MIImageReviewDTO
    {
        [DefaultValue("")]
        public string iImgTransmittalHdrId { get; set; }
        [DefaultValue("")]
        public string iImgTransmittalDtlId { get; set; }
        [DefaultValue("")]
        public string iImageStatus { get; set; }
        [DefaultValue("")]
        public string cRadiologist { get; set; }
        [DefaultValue("")]
        public string MODE { get; set; }
        [DefaultValue("")]
        public string ImageTransmittalImgDtl_iImageTranNo { get; set; }
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

    public class MIImageReviewDetailDTO
    {
        [DefaultValue("")]
        public string iImgTransmittalHdrId { get; set; }
        [DefaultValue("")]
        public string iImgTransmittalDtlId { get; set; }
        [DefaultValue("")]
        public string iImageStatus { get; set; }
        [DefaultValue("")]
        public string cRadiologist { get; set; }
        [DefaultValue("")]
        public string MODE { get; set; }
        [DefaultValue("")]
        public string ImageTransmittalImgDtl_iImageTranNo { get; set; }
        [DefaultValue("")]
        public string vParentActivityId { get; set; }
        [DefaultValue("")]
        public string iParentNodeId { get; set; }
    }
}
