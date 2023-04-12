using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicalImaging.Models;
using System.Web.Services;

namespace MedicalImaging.Controllers
{
    public class MIBizNETImageReviewController : Controller
    {
        //
        // GET: /MIBizNETImageReview/

        public ActionResult MIBizNETImageReview()
        {
            ViewBag.hdnuserid = Convert.ToString(System.Web.HttpContext.Current.Session["iuserid"]);
            ViewBag.hdnUserNameWithProfile = Convert.ToString(System.Web.HttpContext.Current.Session["UserNameWithProfile"]);
            return View();
        }

        [WebMethod]
        public string DicomDetails(MIDicom _MIDicom)
        {            
            System.Web.HttpContext.Current.Session["ipAddress"] = System.Web.HttpContext.Current.Request.UserHostAddress;
            ViewBag.hdnIpAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            //RedirectToAction("MIDicomViewer", "MIDicomViewer");
            return "Success";
        }

    }
}
