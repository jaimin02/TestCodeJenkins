using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace MedicalImaging.Models
{
    public class MiLogin
    {
    }

    public class LoginMst
    {
        public string vUserName { get; set; }
        public string vUserTypeCode { get; set; }
        public string vUserTyepName { get; set; }
        public string vLoginPass { get; set; }

    }

    public class LoginDetails
    {
        public string iUserId { get; set; }
        public string iUserGroupCode { get; set; }
        public string vUserGroupName { get; set; }
        public string vUserName { get; set; }
        public string vLoginName { get; set; }
        public string vLoginPass { get; set; }
        public string vUserTypeCode { get; set; }
        public string vUserTypeName { get; set; }
        public string vDeptCode { get; set; }
        public string vDeptName { get; set; }
        public string vLocationCode { get; set; }
        public string vLocationName { get; set; }
        public string vTimeZoneName { get; set; }
        public string vLocationInitiate { get; set; }
        public string vEmailId { get; set; }
        public string vPhoneNo { get; set; }
        public string vExtNo { get; set; }
        public string vRemark { get; set; }
        public string iModifyBy { get; set; }
        public string dModifyOn { get; set; }
        public string cStatusIndi { get; set; }
        public string vFirstName { get; set; }
        public string vLastName { get; set; }
        public string nScopeNo { get; set; }
        public string vScopeName { get; set; }
        public string vScopeValues { get; set; }
        public string iWorkflowStageId { get; set; }
        public string cIsEDCUser { get; set; }
        public string dFromDate { get; set; }
        public string dToDate { get; set; }
        public string ModifierName { get; set; }
        public string UserNameWithProfile { get; set; }
        public string tmp_dModifyOn { get; set; }
        public string OperationCode { get; set; }
        public string UserWise_CurrDateTime { get; set; }
        public string DataopMode { get; set; }
        public string vUTCHourDiff { get; set; }
        public string vUserAgent { get; set; }


    }

}