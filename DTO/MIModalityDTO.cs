using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DTO
{
    public class MIModalityDTO
    {
        [DefaultValue(0)]
        public string nModalityNo { get; set; }
        [DefaultValue("")]
        public string vModalityDesc { get; set; }
        [DefaultValue("")]
        public string vRemarks { get; set; }
        [DefaultValue("")]
        public string iModifyBy { get; set; }
        [DefaultValue("")]
        public string dModifyOn { get; set; }
        [DefaultValue("")]
        public string cStatusIndi { get; set; }        
    }
}
