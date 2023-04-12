using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DTO
{
    public class MICheckListTemplateDTO
    {
        [DefaultValue("")]
        public string type { get; set; }
        [DefaultValue("")]
        public string nTemplateHdrNo { get; set; }
        [DefaultValue("")]
        public string cTemplateFilter { get; set; }
        [DefaultValue("")]
        public string vTemplateDesc { get; set; }
    }

    public class MICheckListQuestionTemplateDTO
    {
        [DefaultValue("")]
        public List<string>[] vQuestion { get; set; }
        [DefaultValue("")]
        public string iUserId { get; set; }
    }
}
