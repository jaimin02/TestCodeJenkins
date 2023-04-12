using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utility;
using MedicalImaging.Models;
using System.Web.Services;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using MedicalImaging.Repository;

namespace MedicalImaging.Controllers
{
    public class MIDicomStudyController : Controller
    {

        clsCommon _ClsCommon = new clsCommon();
        //
        // GET: /MIDicomStudy/
        public ActionResult MIDicomStudy()
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

        public ActionResult MIDicomStudyDetail(String vParentWorkspaceId, String vWorkSpaceId, String vSubjectId, String vMySubjectNo, String iMySubjectNo, String iPeriod, String vActivityId, String iNodeId, String vSubActivityId, String iSubNodeId, String vActivityName, String vSubActivityName)
        {

            System.Web.HttpContext.Current.Session["dicomStudyvParentWorkspaceId"] = vParentWorkspaceId;
            System.Web.HttpContext.Current.Session["dicomStudyvWorkSpaceId"] = vWorkSpaceId;
            System.Web.HttpContext.Current.Session["dicomStudyvSubjectId"] = vSubjectId;
            System.Web.HttpContext.Current.Session["dicomStudyvMySubjectNo"] = vMySubjectNo;
            System.Web.HttpContext.Current.Session["dicomStudyiMySubjectNo"] = iMySubjectNo;
            System.Web.HttpContext.Current.Session["dicomStudyiPeriod"] = iPeriod;
            System.Web.HttpContext.Current.Session["dicomStudyvActivityId"] = vActivityId;
            System.Web.HttpContext.Current.Session["dicomStudyiNodeId"] = iNodeId;
            System.Web.HttpContext.Current.Session["dicomStudyvSubActivityId"] = vSubActivityId;
            System.Web.HttpContext.Current.Session["dicomStudyiSubNodeId"] = iSubNodeId;
            System.Web.HttpContext.Current.Session["dicomStudyvActivityName"] = vActivityName;
            System.Web.HttpContext.Current.Session["dicomStudyvSubActivityName"] = vSubActivityName;

            return View("MIDicomStudy");

        }

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

        [WebMethod]
        public String updatedSessionValue()
        {
            string updatedVal = Convert.ToString(System.Web.HttpContext.Current.Session["updatedValue"]);
            System.Web.HttpContext.Current.Session["updatedValue"] = "FALSE";
            return updatedVal;
        }

        [WebMethod]
        public async Task<string> SubjectDetailsForDISOFT(SubjectDTO _SubjectDTO)
        {
            string returnString = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                string URL = "CommonData/SubjectDetailsForDISOFT";
                //dt = _ClsCommon.Insert_APITransactionUsingDT(URL, _SubjectDTO);
                //dt = await _ClsCommon.Insert_APITransactionUsingDT(URL, _SubjectDTO);
                dt = await _ClsCommon.Call_API_GeTMethod(URL, _SubjectDTO);
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
        public async Task<string> ProjectActivityDetails(MIDicomProjectDetailDTO _MIDicomProjectDetailDTO)
        {
            string returnString = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                string URL = "GetData/ProjectActivityDetails";
                //dt = _ClsCommon.Insert_APITransactionUsingDT(URL, _MIDicomProjectDetailDTO);
                //dt = await _ClsCommon.Insert_APITransactionUsingDT(URL, _MIDicomProjectDetailDTO);
                dt = await _ClsCommon.Call_API_GeTMethod(URL, _MIDicomProjectDetailDTO);
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
        public async Task<string> MI_DataSaveStatus(MI_DataSaveStatus _MI_DataSaveStatus)
        {
            string returnString = string.Empty;
            try
            {
                
                string URL = "GetData/MI_DataSaveStatus";
                //returnString = _ClsCommon.Insert_APITransaction(URL, _MI_DataSaveStatus);
                returnString = await _ClsCommon.Insert_APITransaction(URL, _MI_DataSaveStatus);
                return returnString;
            }
            catch (Exception e)
            {
                returnString = "ERROR";
                return returnString;
            }
            finally
            {

            }
        }

        [WebMethod]
        public async Task<string> CRFDataEntryStatus(MICRFDataEntryStatus _MICRFDataEntryStatus)
        {
            string returnString = string.Empty;
            try
            {

                string URL = "GetData/CRFDataEntryStatus";
                //returnString = _ClsCommon.Insert_APITransaction(URL, _MICRFDataEntryStatus);
                returnString = await _ClsCommon.Insert_APITransaction(URL, _MICRFDataEntryStatus);
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
        public async Task<string> SubjectStudyDetail(MISubjectStudyDTO _MISubjectStudyDTO)
        {
            string returnString = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                string URL = "GetData/SubjectStudyDetail";
                //dt = _ClsCommon.Insert_APITransactionUsingDT(URL, _MISubjectStudyDTO);
                dt = await _ClsCommon.Call_API_GeTMethod(URL, _MISubjectStudyDTO);
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
        public async Task<string> ProjectLockDetail(ProjectLockDetailDTO _ProjectLockDetailDTO)
        {
            string returnString = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                string URL = "GetData/ProjectLockDetail";
                //dt = _ClsCommon.Insert_APITransactionUsingDT(URL, _ProjectLockDetailDTO);
                dt = await _ClsCommon.Call_API_GeTMethod(URL, _ProjectLockDetailDTO);
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
               // dt = await _ClsCommon.Insert_APITransactionUsingDT(URL, _GetMyProjectCompletionListDTO);
                dt = await _ClsCommon.Call_API_GeTMethod(URL, _GetMyProjectCompletionListDTO);
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
        public async Task<string> ProjectFreezerDetail(ProjectFreezerDetailDTO _ProjectFreezerDetailDTO)
        {
            string returnString = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                string URL = "GetData/ProjectFreezeDetail";
                // dt = _ClsCommon.Insert_APITransactionUsingDT(URL, _ProjectFreezerDetailDTO);
                //dt = await _ClsCommon.Insert_APITransactionUsingDT(URL, _ProjectFreezerDetailDTO);
                dt = await _ClsCommon.Call_API_GeTMethod(URL, _ProjectFreezerDetailDTO);
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
        public async Task<string> SubjectDetailsForDISOFTWithDataEntryStatus(SubjectDTO _SubjectDTO)
        {
            string returnString = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                string URL = "GetData/SubjectDetailsForDISOFTWithDataEntryStatus";
                //dt = _ClsCommon.Insert_APITransactionUsingDT(URL, _SubjectDTO);
                //dt = await _ClsCommon.Insert_APITransactionUsingDT(URL, _SubjectDTO);
                dt = await _ClsCommon.Call_API_GeTMethod(URL, _SubjectDTO);
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
        public async Task<string> GetRadioLogistData(ProjectLockDetailDTO _ProjectLockDetailDTO)
        {
            string returnString = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                string URL = "GetData/GetRadioLogistData";
                //dt = _ClsCommon.Insert_APITransactionUsingDT(URL, _SubjectDTO);
                //dt = await _ClsCommon.Insert_APITransactionUsingDT(URL, _SubjectDTO);
                dt = await _ClsCommon.Call_API_GeTMethod(URL, _ProjectLockDetailDTO);
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
