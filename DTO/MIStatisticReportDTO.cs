using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DTO
{
    public class MIStatisticReportDTO
    {
        [DefaultValue("")]
        public string vParentWorkSpaceId { get; set; }
        [DefaultValue("")]
        public string vWorkspaceId { get; set; }

        [DefaultValue("")]
        public string vBtnRadioName { get; set; }

        [DefaultValue(0)]
        public int imode { get; set; }


    }

    public class MIStatisticReportOverAllDTO
    {
        [DefaultValue("")]
        public string vParentWorkSpaceId { get; set; }

        [DefaultValue("")]
        public string vWorkspaceId { get; set; }

        [DefaultValue(0)]
        public int imode { get; set; }

    }
}
