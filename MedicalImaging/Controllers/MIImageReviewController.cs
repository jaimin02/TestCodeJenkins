using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicalImaging.Models;
using System.Web.Services;
using Utility;


namespace MedicalImaging.Controllers
{
    public class MIImageReviewController : Controller
    {
        //
        // GET: /MIImageReview/

        public ActionResult MIImageReview()
        {
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

            ViewBag.dicomStudyvParentWorkspaceId = this.Request.Params["vParentWorkspaceId"];
            ViewBag.dicomStudyvWorkSpaceId = this.Request.Params["vWorkSpaceId"];
            ViewBag.dicomStudyvSubjectId = this.Request.Params["vSubjectId"];
            ViewBag.dicomStudyvMySubjectNo = this.Request.Params["vMySubjectNo"];
            ViewBag.dicomStudyiMySubjectNo = this.Request.Params["iMySubjectNo"];
            ViewBag.dicomStudyiPeriod = this.Request.Params["iPeriod"];
            ViewBag.dicomStudyvActivityId = this.Request.Params["vActivityId"];
            ViewBag.dicomStudyiNodeId = this.Request.Params["iNodeId"];
            ViewBag.dicomStudyvSubActivityId = this.Request.Params["vSubActivityId"];
            ViewBag.dicomStudyiSubNodeId = this.Request.Params["iSubNodeId"];
            ViewBag.dicomStudyvActivityName = this.Request.Params["vActivityName"];
            ViewBag.dicomStudyvSubActivityName = this.Request.Params["vSubActivityName"];

            //Utilities.clsDB.DBName = System.Configuration.ConfigurationManager.AppSettings["DB_Name"];

            return View("MIImageReview");
        }
        
        //[WebMethod]
        //public string DicomDetails(MIDicom _MIDicom)

        public ActionResult DicomDetails(MIDicom _MIDicom)
        {

            System.Web.HttpContext.Current.Session["iImgTransmittalHdrId"] = _MIDicom.iImgTransmittalHdrId;
            System.Web.HttpContext.Current.Session["iImgTransmittalDtlId"] = _MIDicom.iImgTransmittalDtlId;
            System.Web.HttpContext.Current.Session["iImageStatus"] = _MIDicom.iImageStatus;
            System.Web.HttpContext.Current.Session["vWorkspaceId"] = _MIDicom.vWorkspaceId;
            System.Web.HttpContext.Current.Session["vProjectNo"] = _MIDicom.vProjectNo;
            System.Web.HttpContext.Current.Session["vSubjectId"] = _MIDicom.vSubjectId;
            System.Web.HttpContext.Current.Session["iNodeId"] = _MIDicom.iNodeId;
            System.Web.HttpContext.Current.Session["iModalityNo"] = _MIDicom.iModalityNo;
            System.Web.HttpContext.Current.Session["iAnatomyNo"] = _MIDicom.iAnatomyNo;
            System.Web.HttpContext.Current.Session["ipAddress"] = System.Web.HttpContext.Current.Request.UserHostAddress;
            ViewBag.hdnIpAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            //RedirectToAction("MIDicomViewer", "MIDicomViewer");
            //return "Success";
            return View("MIImageReview");
        }

        public ActionResult MIBiznetImageReview()
        {
            return View();
        }

    }
}
