using MedicalImaging.Models;
using MedicalImaging.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using Utility;
using System.Threading.Tasks;

namespace MedicalImaging.Controllers
{
    public class MIAnatomyController : Controller
    {
        clsCommon _ClsCommon = new clsCommon();
        //
        // GET: /MIAnatomy/

        public ActionResult MIAnatomy()
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

        [WebMethod]
        public async Task<string> PostAddAnatomy(MIAnatomyDTO _Anatomy)
        {
            string returnString = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                if (_Anatomy.cStatusIndi == "D")
                {
                    _Anatomy.vAnatomyDesc = "";
                }
                string URL = "SetData/PostAddAnatomy";
                //returnString = _ClsCommon.Insert_APITransaction(URL, _Anatomy);
                returnString = await _ClsCommon.Insert_APITransaction(URL, _Anatomy);
                return returnString;

            }
            catch (Exception e)
            {
                returnString = "error";
                return returnString;
            }
            finally
            {

            }
        }

        [WebMethod]
        public async Task<string> GetAnatomy()
        {
            string returnString = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                string URL = "GetData/GetAnatomy";
                //dt = _ClsCommon.Call_API_GETMethodDT(URL);
                dt = await _ClsCommon.Call_API_GETMethodDT(URL);
                return _ClsCommon.DataTableToJSONWithStringBuilder(dt);

            }
            catch (Exception e)
            {
                returnString = "error";
                return returnString;
            }
            finally
            {

            }
        }

        [WebMethod]
        public async Task<string> GetAnatomyAuditTrail(MIAnatomyDTO _Anatomy)
        {
            string returnString = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                string URL = "GetData/AnatomyAuditTrail";
                //dt = _ClsCommon.Insert_APITransactionUsingDT(URL, _Anatomy);
                //dt = await _ClsCommon.Insert_APITransactionUsingDT(URL, _Anatomy);
                dt = await _ClsCommon.Call_API_GeTMethod(URL, _Anatomy);
                return _ClsCommon.DataTableToJSONWithStringBuilder(dt);

            }
            catch (Exception e)
            {
                returnString = "error";
                return returnString;
            }
            finally
            {

            }
        }

        [WebMethod]
        public async Task<string> AuditTrail(AuditTrailDTO _AuditTrailDTO)
        {
            string returnString = string.Empty;
            try
            {
                _AuditTrailDTO.iUserId = Session["iuserid"].ToString();
                DataTable dt = new DataTable();
                string URL = "CommonData/AuditTrail";
                // dt = _ClsCommon.Insert_APITransactionUsingDT(URL, _AuditTrailDTO);
                //dt = await _ClsCommon.Insert_APITransactionUsingDT(URL, _AuditTrailDTO);
                dt = await _ClsCommon.Call_API_GeTMethod(URL, _AuditTrailDTO);
                return _ClsCommon.DataTableToJSONWithStringBuilder(dt);

            }
            catch (Exception e)
            {
                returnString = "error";
                return returnString;
            }
            finally
            {

            }
        }
    }
}
