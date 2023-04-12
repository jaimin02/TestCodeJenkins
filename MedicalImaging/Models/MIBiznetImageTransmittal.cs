using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace MedicalImaging.Models
{
    public class MIBiznetImageTransmittal
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
}