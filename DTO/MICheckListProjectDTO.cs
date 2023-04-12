using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DTO
{
    public class MICheckListProjectDTO
    {
        [DefaultValue("")]
        public string vWorkSpaceID { get; set; }
        [DefaultValue("")]
        public string nTemplateHdrNo { get; set; }
        [DefaultValue("")]
        public List<string>[] nTemplateDtlNo { get; set; }
        [DefaultValue("")]
        public string iUserId { get; set; }

    }
}
