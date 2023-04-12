using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicalImaging.Models;
using System.IO;
using System.IO.Compression;
using System.Web.Hosting;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.DirectoryServices;
using System.Collections;

namespace MedicalImaging.Controllers
{
    public class FileDownloadController : Controller
    {
        //private DirectroyEntry websiteEntry = null;
        internal const string IIsWebServer = "IIsWebServer";
        DirectoryEntry Services = new DirectoryEntry("IIS://localhost/W3SVC");  
        //
        // GET: /FileDownload/

        public ActionResult FileHome()
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
            return View();
        }

        public ActionResult Download() {
            //FileDownload obj = new FileDownload();
            //////int CurrentFileID = Convert.ToInt32(FileID);  
            var filesCol = GetFile().ToList();
            using (var memoryStream = new MemoryStream())
            {
                using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    if (filesCol.Count == 0)
                    {
                        return new RedirectResult("~/MIHome/Home");   
                    }
                    for (int i = 0; i < filesCol.Count; i++)
                    {
                        ziparchive.CreateEntryFromFile(filesCol[i].FilePath, filesCol[i].FileName);
                    }
                }
                return File(memoryStream.ToArray(), "application/zip", "Attachments.zip");
            }  
        }

       
            public List<MedicalImaging.Models.FileInfo> GetFile()
            {
                List<MedicalImaging.Models.FileInfo> listFiles = new List<MedicalImaging.Models.FileInfo>();

                string fileSavePath = System.Web.HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["Uploader"]);
                
                if (!Directory.Exists(fileSavePath))
                {
                    Directory.CreateDirectory(fileSavePath);
                }

                DirectoryInfo dirInfo = new DirectoryInfo(fileSavePath);
                int i = 0;

                foreach (var item in dirInfo.GetFiles())
                {
                    listFiles.Add(new MedicalImaging.Models.FileInfo()
                    {
                        FileId = i + 1,
                        FileName = item.Name,
                        FilePath = dirInfo.FullName + @"\" + item.Name
                    });
                    i = i + 1;
                }
                return listFiles;
            }

        }

    
}
