using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utility;

namespace MedicalImaging.Controllers
{
    public class MICheckListProjectController : Controller
    {
        //
        // GET: /MICheckListProject/

        public ActionResult MICheckListProject()
        {
            //Utilities.clsDB.ApiUrl = System.Configuration.ConfigurationManager.AppSettings["Api_Url"];
            ViewBag.hdnuserid = Convert.ToString(System.Web.HttpContext.Current.Session["iuserid"]);
            ViewBag.hdnUserNameWithProfile = Convert.ToString(System.Web.HttpContext.Current.Session["UserNameWithProfile"]);
            ViewBag.hdnlogintime = Convert.ToString(System.Web.HttpContext.Current.Session["LoginTime"]);
            ViewBag.hdnscopevalues = Convert.ToString(System.Web.HttpContext.Current.Session["ScopeValues"]);
            ViewBag.hdnUserName = Convert.ToString(System.Web.HttpContext.Current.Session["vUserName"]);
            ViewBag.hdnUserTypeCode = Convert.ToString(System.Web.HttpContext.Current.Session["UserTypeCode"]);
            ViewBag.hdnUserLocationCode = Convert.ToString(System.Web.HttpContext.Current.Session["vLocationCode"]);
            ViewBag.hdnViewModeID = System.Configuration.ConfigurationManager.AppSettings["ViewMode"];
            ViewBag.hdnIpAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            ViewBag.hdnLocationName = Convert.ToString(System.Web.HttpContext.Current.Session["vLocationName"]);
            ViewBag.hdnDeptName = Convert.ToString(System.Web.HttpContext.Current.Session["vDeptName"]);
            ViewBag.hdnUserFullName = Convert.ToString(System.Web.HttpContext.Current.Session["UserFullName"]);
            //Utilities.clsDB.DBName = System.Configuration.ConfigurationManager.AppSettings["DB_Name"];
            return View();
        }

    }
}
