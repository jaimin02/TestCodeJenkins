using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DTO
{
    public class MILoginDTO
    {

    }

    public class userProfileDTO
    {
        [DefaultValue("")]
        public String vUserTypeCode { get; set; }
        [DefaultValue("")]
        public String vUserTypeName { get; set; }
    }

    public class LoginDetails
    {
        [DefaultValue("")]
        public string iUserId { get; set; }
        [DefaultValue("")]
        public string vUserTypeCode { get; set; }
        [DefaultValue("")]
        public string vUserName { get; set; }
        [DefaultValue("")]
        public string vLoginPass { get; set; }
        [DefaultValue("")]
        public string OperationCode { get; set; }
        [DefaultValue("")]
        public string dLastFailedLogin { get; set; }
        [DefaultValue("")]
        public string nAttemptCount { get; set; }
        [DefaultValue("")]
        public string cBlockedFlag { get; set; }
        [DefaultValue("")]
        public string vIPAddress { get; set; }
        [DefaultValue("")]
        public string iModifyBy { get; set; }
        [DefaultValue("")]
        public string DataopMode { get; set; }
        [DefaultValue("")]
        public string vUTCHourDiff { get; set; }
        [DefaultValue("")]
        public string vUserAgent { get; set; }

    }
}
