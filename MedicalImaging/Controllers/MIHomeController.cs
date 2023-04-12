using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using Utility;
using MedicalImaging.Models;
using System.Data;
using MedicalImaging.Repository;
using DTO;

namespace MedicalImaging.Controllers
{
    public class MIHomeController : Controller
    {
        // GET: /MIHome/
        clsCommon _ClsCommon = new clsCommon();

        public ActionResult Home()
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

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Convert.ToString(System.Web.HttpContext.Current.Session["UserNameWithProfile"]) != "")
            {
                base.OnActionExecuting(filterContext);
            }
            else
            {
                filterContext.Result = new RedirectResult("~/MILogin/Login");
            }

        }

        [WebMethod]
        public string ProjectSubjectDetail(DatatableDTO _DatatableDTO)
        {
            string returnString = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                string URL = "GetData/Datatable";
                dt = _ClsCommon.Insert_APITransactionUsingDataTable(URL, _DatatableDTO);
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
        public string DashBoardDetail(DashboardDetailDTO _DashboardDetailDTO)
        {
            string returnString = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                string URL = "GetData/ProjectDashboardDetail";
                dt = _ClsCommon.Insert_APITransactionUsingDataTable(URL, _DashboardDetailDTO);

                // if condition Added By Bhargav Thaker And modify on 07Mar2023
                if (dt.Rows.Count > 0)
                {
                    //dt = dt.AsEnumerable().GroupBy(r => new { SiteNo = r["SiteNo"], Screening_No = r["Screening No"] }).Select(g => g.OrderByDescending(r => r["dModifyOn"]).FirstOrDefault()).CopyToDataTable();

                    string UserType = System.Web.HttpContext.Current.Session["UserTypeCode"].ToString();
                    string currentprofile = System.Web.HttpContext.Current.Session["UserNameWithProfile"].ToString();
                    var splitval = currentprofile.Split('-');
                    string lastChar = splitval[1].Substring(splitval[1].Length - 1);

                    DataSet ds = new DataSet();
                    ds.Tables.Add(dt);
                    int cnt = 0;
                    string status = "CA1 Review Complete";
                    string subjectNo = string.Empty;
                    var rowsToDelete = new List<DataRow>();

                    foreach (DataRow dr in dt.Rows)
                    {
                        if (subjectNo.Trim() != dr["vSubjectId"].ToString().Trim())
                        {
                            cnt = 0;
                            subjectNo = dr["vSubjectId"].ToString();
                        }

                        if (dr["Status"].ToString().ToLower().Trim() == status.ToLower().Trim())
                        {
                            cnt++;
                            if (cnt > 1)
                            {
                                cnt = 0;
                                rowsToDelete.Add(dr);
                            }
                        }
                                                
                        if (lastChar == "1")
                        {
                            if (dr["Status"].ToString().ToLower().Trim() == ("R1 Review Complete").ToLower().Trim()) { rowsToDelete.Add(dr); }
                        }
                        else if (lastChar == "2")
                        {
                            if (dr["Status"].ToString().ToLower().Trim() == ("R2 Review Complete").ToLower().Trim()) { rowsToDelete.Add(dr); }
                        }
                    }
                    rowsToDelete.ForEach(x => dt.Rows.Remove(x));
                }
                return _ClsCommon.DataTableToJSONWithStringBuilder(dt);
            }
            catch (Exception e)
            {
                returnString = "error";
                return returnString;
            }
            finally { }
        }

    }
}