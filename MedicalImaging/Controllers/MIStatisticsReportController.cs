using MedicalImaging.Models;
using MedicalImaging.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using Utility;

namespace MedicalImaging.Controllers
{
    public class MIStatisticsReportController : Controller
    {
        clsCommon _ClsCommon = new clsCommon();
        //
        // GET: /MIStatisticsReport/

        public ActionResult MIStatisticsReport()
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
            ViewBag.hdnUserTypeCode = Convert.ToString(System.Web.HttpContext.Current.Session["UserTypeCode"]);
            //Utilities.clsDB.DBName = System.Configuration.ConfigurationManager.AppSettings["DB_Name"];
            return View();
        }

        [WebMethod]
        public async Task<string> MIStatisticReport(MIStatisticReportDTO _MIStatisticReportDTO)
        {
            string returnString = string.Empty;
            try
            {
                DataTable dt = new DataTable();

                if (_MIStatisticReportDTO.vBtnRadioName.ToUpper().ToString().Trim() == "ALL")
                {
                    _MIStatisticReportDTO.imode = 0;
                }
                else {
                    _MIStatisticReportDTO.imode = 1;
                }

                string URL = "GetData/MIStatisticReport";
                // dt = _ClsCommon.Insert_APITransactionUsingDT(URL, _MIStatisticReportDTO);
                dt = await _ClsCommon.Call_API_GeTMethod(URL, _MIStatisticReportDTO);
                //dt = await _ClsCommon.Insert_APITransactionUsingDT(URL, _MIStatisticReportDTO);
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
        public async Task<string> MIStatisticReport1(MiReports _MiReports)
        {
            string returnString = string.Empty;
            try
            {
                DataTable dt = new DataTable();

                string URL = "GetData/MIStatisticReport1";
                // dt = _ClsCommon.Insert_APITransactionUsingDT(URL, _MIStatisticReportDTO);
                dt = await _ClsCommon.Call_API_GeTMethod(URL, _MiReports);
                //dt = await _ClsCommon.Insert_APITransactionUsingDT(URL, _MIStatisticReportDTO);
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
        public async Task<string> MyProjectCompletionList(GetMyProjectCompletionListDTO _GetMyProjectCompletionListDTO)
        {
            string returnString = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                string URL = "GetData/MyProjectCompletionList";
                //dt = _ClsCommon.Insert_APITransactionUsingDT(URL, _GetMyProjectCompletionListDTO);
                dt = await _ClsCommon.Call_API_GeTMethod(URL, _GetMyProjectCompletionListDTO);
                //dt = await _ClsCommon.Insert_APITransactionUsingDT(URL, _GetMyProjectCompletionListDTO);
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
        public async Task<string> MyStudyCompletionList(GetMyProjectCompletionListDTO _GetMyProjectCompletionListDTO)
        {
            string returnString = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                string URL = "GetData/MyStudyCompletionList";
                //dt = _ClsCommon.Insert_APITransactionUsingDT(URL, _GetMyProjectCompletionListDTO);
                dt = await _ClsCommon.Call_API_GeTMethod(URL, _GetMyProjectCompletionListDTO);
                //dt = await _ClsCommon.Insert_APITransactionUsingDT(URL, _GetMyProjectCompletionListDTO);
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
