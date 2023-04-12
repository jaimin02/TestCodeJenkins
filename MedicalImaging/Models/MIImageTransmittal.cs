using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace MedicalImaging.Models
{
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
        [DefaultValue("")]
        public string dScheduledDate { get; set; }
    }

    public class MIImageTransmittal_New
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
}