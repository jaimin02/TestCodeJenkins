using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MedicalImaging.Controllers
{
    public class MIBizNETAdjudicatorResponseController : Controller
    {
        //
        // GET: /MIBizNETAdjudicatorResponse/

        public ActionResult MIBizNETAdjudicatorResponse()
        {
            ViewBag.hdnuserid = Convert.ToString(System.Web.HttpContext.Current.Session["iuserid"]);
            ViewBag.hdnUserNameWithProfile = Convert.ToString(System.Web.HttpContext.Current.Session["UserNameWithProfile"]);
            ViewBag.hdnUserTypeCode = Convert.ToString(System.Web.HttpContext.Current.Session["UserTypeCode"]);
            ViewBag.hdnUserName = Convert.ToString(System.Web.HttpContext.Current.Session["vUserName"]);

            String userIpAddress, userAgent;

            Response.AddHeader("Cache-Control", "no-cache, no-store,must-revalidate");
            Response.AddHeader("Pragma", "no-cache");
            Response.AddHeader("Expires", "0");

            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();

            userIpAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            userAgent = System.Web.HttpContext.Current.Request.UserAgent;
            System.Web.HttpContext.Current.Session["sIpAddress"] = userIpAddress;

            if (userAgent.IndexOf("MSIE") > -1)
            {
                userAgent = "MSIE";
            }
            else if (userAgent.IndexOf("Firefox/") > -1)
            {
                userAgent = "Firefox";
            }
            else if (userAgent.IndexOf("Chrome/") > -1)
            {
                userAgent = "Chrome";
            }
            else
            {
                userAgent = "Other";
            }

            ViewBag.hdnIpAddress = Convert.ToString(userIpAddress);
            ViewBag.hdnUserAgent = Convert.ToString(userAgent);
            return View();
        }

    }
}
