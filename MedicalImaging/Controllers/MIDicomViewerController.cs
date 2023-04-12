using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using System.Web.Services;
using MedicalImaging.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MedicalImaging.Models;
using Utility;
using MedicalImaging.Repository;
using System.Configuration;
using Microsoft.Graph;

namespace MedicalImaging.Controllers
{
    public class MIDicomViewerController : Controller
    {
        //
        // GET: /MIDicomViewer/
        clsCommon _ClsCommon = new clsCommon();

        DataTable dtUploadDicom = new DataTable();
        DataTable dtSaveDicom = new DataTable();
        int DicomCount;
        int iImgTransmittalHdrId, iImgTransmittalDtlId, iModifyBy;
        string iImageStatus, cStatusIndi;
        DateTime dModifyOn;
        string vWorkspaceId, vProjectNo, vSubjectId, iNodeId, iModalityNo, iAnatomyNo, vSize, vFinalSize;
        private IEnumerable<Recipient> ItemsList;

        public ActionResult MIDicomViewer()
        {
            Response.AddHeader("Cache-Control", "no-cache, no-store,must-revalidate");
            Response.AddHeader("Pragma", "no-cache");
            Response.AddHeader("Expires", "0");

            ViewBag.hdnIsViist = this.Request.Params["hdnIsViist"];
            ViewBag.hdnActivityData = this.Request.Params["activityData"];
            ViewBag.hdnSubActivityData = this.Request.Params["subActivityData"];
            ViewBag.hdnSubActivityNameData = this.Request.Params["subActivityNameData"];
            ViewBag.hdnArrayNo = this.Request.Params["arrayNo"];
            ViewBag.hdnUserNameWithProfile = Convert.ToString(System.Web.HttpContext.Current.Session["UserNameWithProfile"]);
            ViewBag.hdnUserName = Convert.ToString(System.Web.HttpContext.Current.Session["vUserName"]);
            ViewBag.hdnUserTypeCode = Convert.ToString(System.Web.HttpContext.Current.Session["UserTypeCode"]);
            ViewBag.hdnuserid = Convert.ToString(System.Web.HttpContext.Current.Session["iuserid"]);
            ViewBag.hdnIpAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            ViewBag.hdnUserTypeName = System.Web.HttpContext.Current.Session["vUserTypeName"];
            ViewBag.hdniImgTransmittalHdrId = this.Request.Params["iImgTransmittalHdrId"];
            ViewBag.hdniImgTransmittalDtlId = this.Request.Params["iImgTransmittalDtlId"];
            ViewBag.hdniImageStatus = this.Request.Params["iImageStatus"];
            ViewBag.hdnvWorkspaceId = this.Request.Params["vWorkSpaceId"];
            ViewBag.hdnvProjectNo = this.Request.Params["vProjectNo"];
            ViewBag.hdnvSubjectId = this.Request.Params["vSubjectId"];
            ViewBag.hdniModalityNo = this.Request.Params["iModalityNo"];
            ViewBag.hdniAnatomyNo = this.Request.Params["iAnatomyNo"];
            ViewBag.hdnvParentWorkspaceId = this.Request.Params["vParentWorkspaceId"];
            ViewBag.hdnvMySubjectNo = this.Request.Params["vMySubjectNo"];
            ViewBag.hdniMySubjectNo = this.Request.Params["iMySubjectNo"];
            ViewBag.hdniPeriod = this.Request.Params["iPeriod"];
            ViewBag.hdnvActivityId = this.Request.Params["vActivityId"];
            ViewBag.hdniNodeId = this.Request.Params["iNodeId"];
            ViewBag.hdnvSubActivityId = this.Request.Params["vSubActivityId"];
            ViewBag.hdniSubNodeId = this.Request.Params["iSubNodeId"];
            ViewBag.hdnvActivityName = this.Request.Params["vActivityName"];
            ViewBag.hdnvSubActivityName = this.Request.Params["vSubActivityName"];
            ViewBag.hdnvSkipVisit = this.Request.Params["vSkipVisit"];
            ViewBag.hdniImageCount = this.Request.Params["iImageCount"];
            ViewBag.hdnImgTransmittalDtl_iImageTranNo = this.Request.Params["ImgTransmittalDtl_iImageTranNo"];
            ViewBag.hdnImageTransmittalImgDtl_iImageTranNo = this.Request.Params["ImageTransmittalImgDtl_iImageTranNo"];
            ViewBag.hdnEditPreviousVisit = System.Configuration.ConfigurationManager.AppSettings["EditPreviousVisit"];
            ViewBag.subjectRejectionDtl = this.Request.Params["subjectRejectionDtl"];
            System.Web.HttpContext.Current.Session["updatedValue"] = "FALSE";
            ViewBag.activityArray = this.Request.Params["activityArray"];
            ViewBag.vUserType = this.Request.Params["vUserType"];
            ViewBag.vUserTypeR2 = this.Request.Params["vUserTypeR2"];
            ViewBag.iUserIdR1 = this.Request.Params["iUserIdR1"];
            ViewBag.iUserIdR2 = this.Request.Params["iUserIdR2"];
            ViewBag.vUserTypeCodeR1 = this.Request.Params["vUserTypeCodeR1"];
            ViewBag.vUserTypeCodeR2 = this.Request.Params["vUserTypeCodeR2"];
            ViewBag.WorkFlowStageId = this.Request.Params["WorkFlowStageId"];

            //Utilities.clsDB.ApiUrl = System.Configuration.ConfigurationManager.AppSettings["Api_Url"];
            //Utilities.clsDB.DBName = System.Configuration.ConfigurationManager.AppSettings["DB_Name"];

            //Added by Bhargav Thaker
            if (this.Request.Params.AllKeys.Contains("vParentWorkspaceId"))
            {
                string parentWorkspaceid = this.Request.Params["vParentWorkspaceId"].ToString(); //Added by Bhargav Thaker
                var task = Task.Run(async () => await GetStudyDetails(parentWorkspaceid)); //Added by Bhargav Thaker
                var result = task.Result; //Added by Bhargav Thaker
                ViewBag.hdStudyName = result; //Added by Bhargav Thaker
            }

            //Added by Bhargav Thaker
            if (this.Request.Params.AllKeys.Contains("ParentWorkSpaceId"))
            {
                ViewBag.hdnvParentWorkspaceId = this.Request.Params["ParentWorkSpaceId"]; //Added by Bhargav Thaker
                ViewBag.hdnvMySubjectNo = this.Request.Params["ScreenNo"]; //Added by Bhargav Thaker
                string parentWorkspaceid = this.Request.Params["ParentWorkSpaceId"].ToString(); //Added by Bhargav Thaker
                var task = Task.Run(async () => await GetStudyDetails(parentWorkspaceid)); //Added by Bhargav Thaker
                var result = task.Result; //Added by Bhargav Thaker
                ViewBag.hdStudyName = result; //Added by Bhargav Thaker
            }
            return View();
        }

        // For MI
        public void datatableDicom(int stackLength, string vWorkspaceId1, string vProjectNo1, string vSubjectId1, string iNodeId1, string iModalityNo1, string iAnatomyNo1, string iImgTransmittalHdrId1, string iImgTransmittalDtlId1)
        {
            iImageStatus = "2";
            System.Web.HttpContext.Current.Session["S_DicomCount"] = stackLength;
            iModifyBy = Convert.ToInt32(System.Web.HttpContext.Current.Session["iuserid"]);
            cStatusIndi = "N";

            iImgTransmittalHdrId = Convert.ToInt32(iImgTransmittalHdrId1);
            iImgTransmittalDtlId = Convert.ToInt32(iImgTransmittalDtlId1);
            vWorkspaceId = vWorkspaceId1;
            vProjectNo = vProjectNo1;
            vSubjectId = vSubjectId1;
            iNodeId = iNodeId1;
            iModalityNo = iModalityNo1;
            iAnatomyNo = iAnatomyNo1;

            if (System.Web.HttpContext.Current.Session["S_DicomImage"] == null)
            {
                dtUploadDicom.Columns.Add("vFileName", typeof(string));
                dtUploadDicom.Columns.Add("vServerPath", typeof(string));
                dtUploadDicom.Columns.Add("vFileType", typeof(string));
                dtUploadDicom.Columns.Add("vSize", typeof(string));
                dtUploadDicom.Columns.Add("dScheduledDate", typeof(DateTime));
            }
        }

        // For Biznet
        public void biznetDatatableDicom(int stackLength)
        {
            System.Web.HttpContext.Current.Session["S_DicomCount"] = stackLength;
            if (System.Web.HttpContext.Current.Session["S_DicomImageBizNet"] == null)
            {
                dtUploadDicom.Columns.Add("iImgTransmittalHdrId", typeof(int));
                dtUploadDicom.Columns.Add("iImgTransmittalDtlId", typeof(int));
                dtUploadDicom.Columns.Add("vFileName", typeof(string));
                dtUploadDicom.Columns.Add("vServerPath", typeof(string));
                dtUploadDicom.Columns.Add("vFileType", typeof(string));
                dtUploadDicom.Columns.Add("vSize", typeof(string));
                dtUploadDicom.Columns.Add("dScheduledDate", typeof(DateTime));
                dtUploadDicom.Columns.Add("iImageStatus", typeof(string));
                dtUploadDicom.Columns.Add("iModifyBy", typeof(int));
                dtUploadDicom.Columns.Add("dModifyOn", typeof(DateTime));
                dtUploadDicom.Columns.Add("cStatusIndi", typeof(string));
            }
        }

        // For MI
        [WebMethod]
        public string UploadDicom(string DicomImage, string DicomName, int stackLength, string vWorkspaceId, string vProjectNo, string vSubjectId, string iNodeId, string iModalityNo, string iAnatomyNo, string iImgTransmittalHdrId, string iImgTransmittalDtlId, string cRadiologist, string ImageTransmittalImgDtl_iImageTranNo)
        {
            try
            {
                datatableDicom(stackLength, vWorkspaceId, vProjectNo, vSubjectId, iNodeId, iModalityNo, iAnatomyNo, iImgTransmittalHdrId, iImgTransmittalDtlId);

                if (vWorkspaceId == "" || vWorkspaceId == null || vWorkspaceId == string.Empty)
                {
                    return "SessionExpired";
                }
                DataRow dr;
                string path = System.Configuration.ConfigurationManager.AppSettings["DicomURL_1"] + "\\" + vWorkspaceId + "\\" + vSubjectId + "\\" + iNodeId + "\\" + iModalityNo;
                System.Web.HttpContext.Current.Session["MIDicomPath"] = path;
                System.Web.HttpContext.Current.Session["MIDicomPathRadiologist"] = cRadiologist;

                if (!System.IO.Directory.Exists(path))
                {
                    return "ErrorNoPathFound";
                }
                else
                {
                    path += "\\Updated\\" + cRadiologist;

                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    string FianlDicomName = vWorkspaceId + "_" + vSubjectId + "_" + iNodeId + "_" + iModalityNo + "_" + cRadiologist + "_" + ImageTransmittalImgDtl_iImageTranNo + "_" + DicomName;
                    string fileNameWitPath = path + "\\" + FianlDicomName + ".png";
                    //string fileNameWitPath = path + "\\" + FianlDicomName + ".jpeg";
                    string savefileNameWitPath = "/" + vWorkspaceId + "/" + vSubjectId + "/" + iNodeId + "/" + iModalityNo + "/Updated/" + cRadiologist + "/" + FianlDicomName + ".png";
                    //string savefileNameWitPath = "/" + vWorkspaceId + "/" + vSubjectId + "/" + iNodeId + "/" + iModalityNo + "/Updated/" + cRadiologist + "/" + FianlDicomName + ".jpeg";

                    using (FileStream fs = new FileStream(fileNameWitPath, FileMode.Create))
                    {
                        using (BinaryWriter bw = new BinaryWriter(fs))
                        {
                            vSize = Convert.ToString(((DicomImage.Length) * 3) / 4);
                            vFinalSize = Utility.Utilities.GetFileSize(Convert.ToDouble(vSize));
                            byte[] data = Convert.FromBase64String(DicomImage);
                            bw.Write(data);
                            bw.Close();
                            if (System.Web.HttpContext.Current.Session["S_DicomImage"] != null)
                            {
                                dtUploadDicom = (DataTable)System.Web.HttpContext.Current.Session["S_DicomImage"];
                                dr = dtUploadDicom.NewRow();
                                dr["vFileName"] = Convert.ToString(FianlDicomName);
                                dr["vServerPath"] = savefileNameWitPath;
                                dr["vFileType"] = ".png";
                                //dr["vFileType"] = ".jpeg";
                                dr["vSize"] = vFinalSize;
                                dr["dScheduledDate"] = DateTime.Now;

                                dtUploadDicom.Rows.Add(dr);
                                System.Web.HttpContext.Current.Session["S_DicomImage"] = dtUploadDicom;
                            }
                            else
                            {
                                dr = dtUploadDicom.NewRow();
                                dr["vFileName"] = Convert.ToString(FianlDicomName);
                                dr["vServerPath"] = savefileNameWitPath;
                                dr["vFileType"] = ".png";
                                //dr["vFileType"] = ".jpeg";
                                dr["vSize"] = vFinalSize;
                                dr["dScheduledDate"] = DateTime.Now;

                                dtUploadDicom.Rows.Add(dr);
                                System.Web.HttpContext.Current.Session["S_DicomImage"] = dtUploadDicom;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "error";
                throw ex;
            }
            finally
            {
            }
            return "success";
        }

        // For MI
        [WebMethod]
        public async Task<string> SaveDicom()
        {
            try
            {
                string path = System.Configuration.ConfigurationManager.AppSettings["DicomURL_1"];
                MIImageTransmittal_New _MIImageTransmittalnew = new MIImageTransmittal_New();
                dtSaveDicom = (DataTable)System.Web.HttpContext.Current.Session["S_DicomImage"];
                DicomCount = (int)System.Web.HttpContext.Current.Session["S_DicomCount"];
                _MIImageTransmittalnew.iImgTransmittalHdrId = Convert.ToInt32(System.Web.HttpContext.Current.Session["iImgTransmittalHdrId"]);
                _MIImageTransmittalnew.iImgTransmittalDtlId = Convert.ToInt32(System.Web.HttpContext.Current.Session["iImgTransmittalDtlId"]);
                _MIImageTransmittalnew.iImageMode = 2;
                int uID = Convert.ToInt32(System.Web.HttpContext.Current.Session["iuserid"]);
                _MIImageTransmittalnew.UserID = uID;
                _MIImageTransmittalnew._SaveImageTransmittal = Utility.Utilities.ConvertDataTable<MIImageTransmittalImgList>(dtSaveDicom, "ConvertOnString");
                if (dtSaveDicom.Rows.Count == DicomCount)
                {
                    string URL = "SetData/SaveImageTransmittalnew";
                    var response = await clsCommon.Call_API_POSTMethod(URL, _MIImageTransmittalnew);

                    if (response.IsSuccessStatusCode)
                    {
                        var JsonStringModality = await response.Content.ReadAsStringAsync();

                        if (JsonStringModality.Contains("1"))
                        {
                        }
                        else
                        {
                            foreach (DataRow dr in dtSaveDicom.Rows)
                            {
                                string destination = path + Convert.ToString(dr["vServerPath"]);
                                if (System.IO.File.Exists(destination))
                                {
                                    System.IO.File.Delete(destination);
                                }
                            }
                        }
                    }

                    //using (var client = new HttpClient())
                    //{
                    //    //string URL = "SetData/SaveImageTransmittalnew";
                    //    string strURL = System.Configuration.ConfigurationManager.AppSettings["Api_Url"] + "SetData/SaveImageTransmittalnew";
                    //    client.DefaultRequestHeaders.Accept.Clear();
                    //    client.BaseAddress = new Uri(strURL);
                    //    var serializedProduct_new = JsonConvert.SerializeObject(_MIImageTransmittalnew);
                    //    var content = new StringContent(serializedProduct_new, Encoding.UTF8, "application/json");
                    //    var response = await client.PostAsync(String.Format("{0}", strURL), content);
                    //    if (response.IsSuccessStatusCode)
                    //    {
                    //        var JsonStringModality = await response.Content.ReadAsStringAsync();
                    //        if (JsonStringModality.Contains("1"))
                    //        {
                    //        }
                    //        else
                    //        {
                    //            foreach (DataRow dr in dtSaveDicom.Rows)
                    //            {
                    //                string destination = path + Convert.ToString(dr["vServerPath"]);
                    //                if (System.IO.File.Exists(destination))
                    //                {
                    //                    System.IO.File.Delete(destination);
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                }
                else
                {
                    foreach (DataRow dr in dtSaveDicom.Rows)
                    {
                        string destination = path + Convert.ToString(dr["vServerPath"]);
                        if (System.IO.File.Exists(destination))
                        {
                            System.IO.File.Delete(destination);
                        }
                    }
                    //delete image and change flag
                    return "Error While Saving Dicom Image";
                }
            }
            catch (Exception ex)
            {
                return "Error" + ex;
            }
            finally
            {
            }
            return "Succes";
        }

        // For Biznet
        public async Task<string> BizNETUploadSaveDicom(string DicomImage, string DicomName, int stackLength, string vWorkspaceId, string vProjectNo, string vSubjectId, string iNodeId, string iModalityNo, string iAnatomyNo, string iuserid, string iImgTransmittalHdrId, string iImgTransmittalDtlId)
        {
            try
            {
                string WorkspaceId = vWorkspaceId;
                string ProjectNo = vProjectNo;
                string SubjectId = vSubjectId;
                string NodeId = iNodeId;
                string ModalityNo = iModalityNo;
                string AnatomyNo = iAnatomyNo;
                string userid = iuserid;
                string ImgTransmittalHdrId = iImgTransmittalHdrId;
                string ImgTransmittalDtlId = iImgTransmittalDtlId;

                biznetDatatableDicom(stackLength);
                DataRow dr;
                string path = System.Configuration.ConfigurationManager.AppSettings["DicomURL_1"] + "\\" + WorkspaceId + "\\" + SubjectId + "\\" + NodeId + "\\" + ModalityNo;
                if (!System.IO.Directory.Exists(path))
                {
                    return "Error No Path Found";
                }
                else
                {
                    path += "\\Updated";

                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    string FianlDicomName = vWorkspaceId + "_" + vSubjectId + "_" + iNodeId + "_" + iModalityNo + "_" + iAnatomyNo + "_" + DicomName;
                    string fileNameWitPath = path + "\\" + FianlDicomName + ".png";
                    string savefileNameWitPath = "/" + vWorkspaceId + "/" + vSubjectId + "/" + iNodeId + "/" + iModalityNo + "/" + iAnatomyNo + "/Updated/" + FianlDicomName + ".png";
                    using (FileStream fs = new FileStream(fileNameWitPath, FileMode.Create))
                    {
                        using (BinaryWriter bw = new BinaryWriter(fs))
                        {
                            vSize = Convert.ToString(((DicomImage.Length) * 3) / 4);
                            vFinalSize = Utility.Utilities.GetFileSize(Convert.ToDouble(vSize));
                            byte[] data = Convert.FromBase64String(DicomImage);
                            bw.Write(data);
                            bw.Close();

                            try
                            {
                                string pathtosave = System.Configuration.ConfigurationManager.AppSettings["DicomURL_1"];

                                MIBiznetImageTransmittal _MIBiznetImageTransmittal = new MIBiznetImageTransmittal();
                                _MIBiznetImageTransmittal.iImageMode = 2;
                                _MIBiznetImageTransmittal.iImgTransmittalHdrId = Convert.ToInt32(ImgTransmittalHdrId);
                                _MIBiznetImageTransmittal.iImgTransmittalDtlId = Convert.ToInt32(ImgTransmittalDtlId);
                                _MIBiznetImageTransmittal.vFileName = FianlDicomName;
                                _MIBiznetImageTransmittal.vServerPath = savefileNameWitPath;
                                _MIBiznetImageTransmittal.vFileType = ".png";
                                _MIBiznetImageTransmittal.vSize = vFinalSize;
                                _MIBiznetImageTransmittal.dScheduledDate = DateTime.Now;
                                _MIBiznetImageTransmittal.iModifyBy = Convert.ToInt32(userid);

                                var response = await clsCommon.Call_API_POSTMethod("SetData/SaveBiznetDicom", _MIBiznetImageTransmittal);
                                if (response.IsSuccessStatusCode)
                                {
                                    var JsonStringModality = await response.Content.ReadAsStringAsync();
                                    if (JsonStringModality.Contains("1"))
                                    {
                                        return "1";
                                    }
                                    else if (JsonStringModality.Contains("2"))
                                    {
                                        return "2";
                                    }
                                    else
                                    {
                                        foreach (DataRow dr_dtSaveDicom in dtSaveDicom.Rows)
                                        {
                                            string destination = pathtosave + Convert.ToString(dr_dtSaveDicom["vServerPath"]);
                                            if (System.IO.File.Exists(destination))
                                            {
                                                System.IO.File.Delete(destination);

                                            }
                                        }
                                        return "0";
                                    }
                                }

                                //using (var client = new HttpClient())
                                //{
                                //    string strURL = System.Configuration.ConfigurationManager.AppSettings["Api_Url"] + "SetData/SaveBiznetDicom";
                                //    client.DefaultRequestHeaders.Accept.Clear();
                                //    client.BaseAddress = new Uri(strURL);
                                //    var serializedProduct_new = JsonConvert.SerializeObject(_MIBiznetImageTransmittal);
                                //    var content = new StringContent(serializedProduct_new, Encoding.UTF8, "application/json");
                                //    var response = await client.PostAsync(String.Format("{0}", strURL), content);
                                //    if (response.IsSuccessStatusCode)
                                //    {
                                //        var JsonStringModality = await response.Content.ReadAsStringAsync();
                                //        if (JsonStringModality.Contains("1"))
                                //        {
                                //            return "1";
                                //        }
                                //        else if (JsonStringModality.Contains("2"))
                                //        {
                                //            return "2";
                                //        }
                                //        else
                                //        {
                                //            foreach (DataRow dr_dtSaveDicom in dtSaveDicom.Rows)
                                //            {
                                //                string destination = pathtosave + Convert.ToString(dr_dtSaveDicom["vServerPath"]);
                                //                if (System.IO.File.Exists(destination))
                                //                {
                                //                    System.IO.File.Delete(destination);

                                //                }
                                //            }
                                //            return "0";
                                //        }
                                //    }
                                //}
                            }
                            catch (Exception ex)
                            {
                                return "Error" + ex;
                            }
                            finally
                            {
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return "success";
        }
        // For Biznet
        public async Task<string> BizNETUploadSaveDicom2(string DicomImage, string DicomName, int stackLength, string vWorkspaceId, string vProjectNo, string vSubjectId, string iNodeId, string iModalityNo, string iAnatomyNo, string iuserid, string iImgTransmittalHdrId, string iImgTransmittalDtlId)
        {
            try
            {
                string WorkspaceId = vWorkspaceId;
                string ProjectNo = vProjectNo;
                string SubjectId = vSubjectId;
                string NodeId = iNodeId;
                string ModalityNo = iModalityNo;
                string AnatomyNo = iAnatomyNo;
                string userid = iuserid;
                string ImgTransmittalHdrId = iImgTransmittalHdrId;
                string ImgTransmittalDtlId = iImgTransmittalDtlId;

                biznetDatatableDicom(stackLength);
                DataRow dr;
                string path = System.Configuration.ConfigurationManager.AppSettings["DicomURL_1"] + "\\" + WorkspaceId + "\\" + SubjectId + "\\" + NodeId + "\\" + ModalityNo;
                if (!System.IO.Directory.Exists(path))
                {
                    return "Error No Path Found";
                }
                else
                {
                    path += "\\Updated";

                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    string FianlDicomName = vWorkspaceId + "_" + vSubjectId + "_" + iNodeId + "_" + iModalityNo + "_" + iAnatomyNo + "_" + DicomName;
                    string fileNameWitPath = path + "\\" + FianlDicomName + ".png";
                    string savefileNameWitPath = "/" + vWorkspaceId + "/" + vSubjectId + "/" + iNodeId + "/" + iModalityNo + "/" + iAnatomyNo + "/Updated/" + FianlDicomName + ".png";
                    using (FileStream fs = new FileStream(fileNameWitPath, FileMode.Create))
                    {
                        using (BinaryWriter bw = new BinaryWriter(fs))
                        {
                            vSize = Convert.ToString(((DicomImage.Length) * 3) / 4);
                            vFinalSize = Utility.Utilities.GetFileSize(Convert.ToDouble(vSize));
                            byte[] data = Convert.FromBase64String(DicomImage);
                            bw.Write(data);
                            bw.Close();

                            try
                            {
                                string pathtosave = System.Configuration.ConfigurationManager.AppSettings["DicomURL_1"];

                                MIBiznetImageTransmittal _MIBiznetImageTransmittal = new MIBiznetImageTransmittal();
                                _MIBiznetImageTransmittal.iImageMode = 2;
                                _MIBiznetImageTransmittal.iImgTransmittalHdrId = Convert.ToInt32(ImgTransmittalHdrId);
                                _MIBiznetImageTransmittal.iImgTransmittalDtlId = Convert.ToInt32(ImgTransmittalDtlId);
                                _MIBiznetImageTransmittal.vFileName = FianlDicomName;
                                _MIBiznetImageTransmittal.vServerPath = savefileNameWitPath;
                                _MIBiznetImageTransmittal.vFileType = ".png";
                                _MIBiznetImageTransmittal.vSize = vFinalSize;
                                _MIBiznetImageTransmittal.dScheduledDate = DateTime.Now;
                                _MIBiznetImageTransmittal.iModifyBy = Convert.ToInt32(userid);

                                var response = await clsCommon.Call_API_POSTMethod("SetData/SaveBiznetDicom", _MIBiznetImageTransmittal);
                                if (response.IsSuccessStatusCode)
                                {
                                    var JsonStringModality = await response.Content.ReadAsStringAsync();
                                    if (JsonStringModality.Contains("1"))
                                    {
                                        return "1";
                                    }
                                    else if (JsonStringModality.Contains("2"))
                                    {
                                        return "2";
                                    }
                                    else
                                    {
                                        foreach (DataRow dr_dtSaveDicom in dtSaveDicom.Rows)
                                        {
                                            string destination = pathtosave + Convert.ToString(dr_dtSaveDicom["vServerPath"]);
                                            if (System.IO.File.Exists(destination))
                                            {
                                                System.IO.File.Delete(destination);

                                            }
                                        }
                                        return "0";
                                    }
                                }

                                //using (var client = new HttpClient())
                                //{
                                //    string strURL = System.Configuration.ConfigurationManager.AppSettings["Api_Url"] + "SetData/SaveBiznetDicom";
                                //    client.DefaultRequestHeaders.Accept.Clear();
                                //    client.BaseAddress = new Uri(strURL);
                                //    var serializedProduct_new = JsonConvert.SerializeObject(_MIBiznetImageTransmittal);
                                //    var content = new StringContent(serializedProduct_new, Encoding.UTF8, "application/json");
                                //    var response = await client.PostAsync(String.Format("{0}", strURL), content);
                                //    if (response.IsSuccessStatusCode)
                                //    {
                                //        var JsonStringModality = await response.Content.ReadAsStringAsync();
                                //        if (JsonStringModality.Contains("1"))
                                //        {
                                //            return "1";
                                //        }
                                //        else if (JsonStringModality.Contains("2"))
                                //        {
                                //            return "2";
                                //        }
                                //        else
                                //        {
                                //            foreach (DataRow dr_dtSaveDicom in dtSaveDicom.Rows)
                                //            {
                                //                string destination = pathtosave + Convert.ToString(dr_dtSaveDicom["vServerPath"]);
                                //                if (System.IO.File.Exists(destination))
                                //                {
                                //                    System.IO.File.Delete(destination);

                                //                }
                                //            }
                                //            return "0";
                                //        }
                                //    }
                                //}
                            }
                            catch (Exception ex)
                            {
                                return "Error" + ex;
                            }
                            finally
                            {
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return "success";
        }

        [WebMethod]
        public string ClearSession()
        {
            System.Web.HttpContext.Current.Session["S_DicomImage"] = null;
            System.Web.HttpContext.Current.Session["S_DicomCount"] = null;
            System.Web.HttpContext.Current.Session["MIDicomPath"] = null;
            System.Web.HttpContext.Current.Session["MIDicomPathRadiologist"] = null;
            return "success";
        }

        //For Biznet To Delete Image if data not stored or get error while saving CRF detail
        [WebMethod]
        public string BizNetImage(string WorkspaceId, string SubjectId, string NodeId, string ModalityNo)
        {
            try
            {
                string path = System.Configuration.ConfigurationManager.AppSettings["DicomURL_1"] + "\\" + WorkspaceId + "\\" + SubjectId + "\\" + NodeId + "\\" + ModalityNo + "\\" + "Updated";
                path += "\\Updated";
                if (System.IO.Directory.Exists(path))
                {
                    foreach (string file in System.IO.Directory.GetFiles(path))
                    {
                        System.IO.File.Delete(file);
                    }
                    System.IO.Directory.Delete(path);
                    return "success";
                }
                return "error";
            }
            catch (Exception e)
            {
                return "error";
            }
        }

        //For MI To Delete Image if image is  not stored
        [WebMethod]
        public string MIManageImage()
        {
            try
            {
                #region
                //*****************************Do Not Delete For Reference and Future Use.*****************************//
                //**********Prevent Image From Delete because when got error image get replaced in next try.**********//

                //string path = Convert.ToString(System.Web.HttpContext.Current.Session["MIDicomPath"]);
                //string radiologist = Convert.ToString(System.Web.HttpContext.Current.Session["MIDicomPathRadiologist"]);
                //if (path == "" || path == null || path == string.Empty)
                //{
                //    return "success";
                //}
                ////path += "\\Updated";
                //path += "\\Updated\\" + radiologist;
                //if (Directory.Exists(path))
                //{
                //    foreach (string file in Directory.GetFiles(path))
                //    {
                //        System.IO.File.Delete(file);
                //    }
                //    Directory.Delete(path);
                //    return "success";
                //}
                //return "error";

                //*****************************Do Not Delete For Reference and Future Use.*****************************//
                //**********Prevent Image From Delete because when got error image get replaced in next try.**********//
                #endregion
                return "success";
            }
            catch (Exception e)
            {
                return "error";
            }

        }

        [WebMethod]
        [AsyncTimeout(100000)]
        public async Task<string> SubmitMIFinalLesion(MIFinalLesionCRFData _MIFinalLesionCRFData)
        {
            string returnString = string.Empty;

            try
            {
                if (System.Web.HttpContext.Current.Session["S_DicomImage"] == null || System.Web.HttpContext.Current.Session["S_DicomImage"] == "")
                {
                    returnString = "SessionExpired";
                    return returnString;
                }
                else if (System.Web.HttpContext.Current.Session["S_DicomCount"] == null || System.Web.HttpContext.Current.Session["S_DicomCount"] == "")
                {
                    returnString = "SessionExpired";
                    return returnString;
                }
                else if (_MIFinalLesionCRFData.iImgTransmittalHdrId == null || _MIFinalLesionCRFData.iImgTransmittalHdrId == 0)
                {
                    returnString = "SessionExpired";
                    return returnString;
                }

                //string path = System.Configuration.ConfigurationManager.AppSettings["DicomURL_1"];
                MIFinalLesionData _MIFinalLesionData = new MIFinalLesionData();
                _MIFinalLesionData.vParentWorkSpaceId = _MIFinalLesionCRFData.vParentWorkSpaceId;
                _MIFinalLesionData.vWorkspaceId = _MIFinalLesionCRFData.vWorkspaceId;
                _MIFinalLesionData.vSubjectId = _MIFinalLesionCRFData.vSubjectId;
                _MIFinalLesionData.iMySubjectNo = _MIFinalLesionCRFData.iMySubjectNo;
                _MIFinalLesionData.vActivityId = _MIFinalLesionCRFData.vActivityId;
                _MIFinalLesionData.iNodeId = _MIFinalLesionCRFData.iNodeId;
                _MIFinalLesionData.vPeriodId = _MIFinalLesionCRFData.vPeriodId;
                _MIFinalLesionData.vLesionType = _MIFinalLesionCRFData.vLesionType;
                _MIFinalLesionData.vLesionSubType = _MIFinalLesionCRFData.vLesionSubType;
                _MIFinalLesionData.cRadiologist = _MIFinalLesionCRFData.cRadiologist;
                _MIFinalLesionData.iImageTranNo = _MIFinalLesionCRFData.iImageTranNo;

                dtSaveDicom = (DataTable)System.Web.HttpContext.Current.Session["S_DicomImage"];
                DicomCount = (int)System.Web.HttpContext.Current.Session["S_DicomCount"];
                _MIFinalLesionData.iImgTransmittalHdrId = Convert.ToInt32(_MIFinalLesionCRFData.iImgTransmittalHdrId);
                _MIFinalLesionData.iImgTransmittalDtlId = Convert.ToInt32(_MIFinalLesionCRFData.iImgTransmittalDtlId);
                _MIFinalLesionData.iImageMode = 2;
                int uID = Convert.ToInt32(System.Web.HttpContext.Current.Session["iuserid"]);
                _MIFinalLesionData.UserID = uID;
                _MIFinalLesionData.MIFinalLesionImageList = Utility.Utilities.ConvertDataTable<MIFinalLesionImageList>(dtSaveDicom, "");
                if (dtSaveDicom.Rows.Count == DicomCount)
                {
                    var response = await clsCommon.Call_API_POSTMethod(Utility.Utilities.clsMethod.SubmitMIFinalLesion, _MIFinalLesionData);
                    if (response.IsSuccessStatusCode)
                    {
                        var JsonString = await response.Content.ReadAsStringAsync();

                        //if (JsonString.Contains("1"))
                        if (JsonString == "1")
                        {
                            returnString = "success";
                            System.Web.HttpContext.Current.Session["updatedValue"] = "TRUE";
                        }
                        //else if (JsonString.Contains("success"))
                        else if (JsonString == "\"success\"")
                        {
                            returnString = "success";
                            System.Web.HttpContext.Current.Session["updatedValue"] = "TRUE";
                        }
                        else if (JsonString.Contains("RecordsNotFound"))
                        {
                            returnString = "RecordsNotFound";
                        }
                        else
                        {
                            returnString = "error";
                        }
                    }
                    //using (var client = new HttpClient())
                    //{
                    //    string strURL = System.Configuration.ConfigurationManager.AppSettings["Api_Url"] + Utility.Utilities.clsMethod.SubmitMIFinalLesion;
                    //    //string strURL = "http://90.0.0.68/MI_API/API/SetData/SubmitMIFinalLesion";
                    //    client.DefaultRequestHeaders.Accept.Clear();
                    //    client.BaseAddress = new Uri(strURL);
                    //    client.Timeout = TimeSpan.FromMinutes(10);
                    //    var serializedProduct_new = JsonConvert.SerializeObject(_MIFinalLesionData);
                    //    var content = new StringContent(serializedProduct_new, Encoding.UTF8, "application/json");
                    //    var response = await client.PostAsync(String.Format("{0}", strURL), content);
                    //    if (response.IsSuccessStatusCode)
                    //    {
                    //        var JsonString = await response.Content.ReadAsStringAsync();

                    //        //if (JsonString.Contains("1"))
                    //        if (JsonString == "1")
                    //        {
                    //            returnString = "success";
                    //            System.Web.HttpContext.Current.Session["updatedValue"] = "TRUE";
                    //        }
                    //        //else if (JsonString.Contains("success"))
                    //        else if (JsonString == "\"success\"")
                    //        {
                    //            returnString = "success";
                    //            System.Web.HttpContext.Current.Session["updatedValue"] = "TRUE";
                    //        }
                    //        else if (JsonString.Contains("RecordsNotFound"))
                    //        {
                    //            returnString = "RecordsNotFound";
                    //        }
                    //        else
                    //        {
                    //            #region
                    //            //***************************Do Not Delete For Reference and Future Use.**********************************//
                    //            //************Prevent Image From Delete because when got error image get replaced in next try.************//

                    //            //foreach (DataRow dr in dtSaveDicom.Rows)
                    //            //{
                    //            //    string destination = path + Convert.ToString(dr["vServerPath"]);
                    //            //    if (System.IO.File.Exists(destination))
                    //            //    {
                    //            //        System.IO.File.Delete(destination);
                    //            //    }
                    //            //}

                    //            //***************************Do Not Delete For Reference and Future Use.**********************************//
                    //            //************Prevent Image From Delete because when got error image get replaced in next try.************//
                    //            #endregion
                    //            returnString = "error";

                    //        }
                    //    }
                    //}
                }
                else
                {
                    #region
                    //***************************Do Not Delete For Reference and Future Use.**********************************//
                    //************Prevent Image From Delete because when got error image get replaced in next try.************//

                    //foreach (DataRow dr in dtSaveDicom.Rows)
                    //{
                    //    string destination = path + Convert.ToString(dr["vServerPath"]);
                    //    if (System.IO.File.Exists(destination))
                    //    {
                    //        System.IO.File.Delete(destination);
                    //    }
                    //}
                    //delete image and change flag
                    //return "Error While Saving Dicom Image";

                    //***************************Do Not Delete For Reference and Future Use.**********************************//
                    //************Prevent Image From Delete because when got error image get replaced in next try.************//
                    #endregion
                    returnString = "ImageNotSavedProperlyToPhysicalPath";
                }

            }
            catch (Exception ex)
            {
                returnString = "error";
                return returnString;
                //return "Error" + ex;
            }
            finally
            {
            }
            //return "success";
            return returnString;
        }

        [WebMethod]
        public string MIeSignatureVerification(string password)
        {
            if (Convert.ToString(System.Web.HttpContext.Current.Session["vLoginPass"]) == "" || Convert.ToString(System.Web.HttpContext.Current.Session["vLoginPass"]) == string.Empty || Convert.ToString(System.Web.HttpContext.Current.Session["vLoginPass"]) == null)
            {
                return "sessionexpired";
            }
            else if (Convert.ToString(System.Web.HttpContext.Current.Session["vLoginPass"]) == Utilities.EncryptPassword(password))
            {
                return "success";
            }
            else
            {
                return "error";
            }
        }

        [WebMethod]
        public async Task<string> SkipMIFinalLesion(MISkipLesionCRFData _MISkipLesionCRFData)
        {
            string returnString = string.Empty;

            try
            {
                MIFinalSkipLesionCRFData _MIFinalSkipLesionCRFData = new MIFinalSkipLesionCRFData();
                _MIFinalSkipLesionCRFData.vParentWorkSpaceId = _MISkipLesionCRFData.vParentWorkSpaceId;
                _MIFinalSkipLesionCRFData.vWorkspaceId = _MISkipLesionCRFData.vWorkspaceId;
                _MIFinalSkipLesionCRFData.vSubjectId = _MISkipLesionCRFData.vSubjectId;
                _MIFinalSkipLesionCRFData.iMySubjectNo = _MISkipLesionCRFData.iMySubjectNo;
                _MIFinalSkipLesionCRFData.vActivityId = _MISkipLesionCRFData.vActivityId;
                _MIFinalSkipLesionCRFData.iNodeId = _MISkipLesionCRFData.iNodeId;
                _MIFinalSkipLesionCRFData.vPeriodId = _MISkipLesionCRFData.vPeriodId;
                _MIFinalSkipLesionCRFData.vLesionType = _MISkipLesionCRFData.vLesionType;
                _MIFinalSkipLesionCRFData.vLesionSubType = _MISkipLesionCRFData.vLesionSubType;
                _MIFinalSkipLesionCRFData.cRadiologist = _MISkipLesionCRFData.cRadiologist;
                int uID = Convert.ToInt32(System.Web.HttpContext.Current.Session["iuserid"]);
                _MIFinalSkipLesionCRFData.UserID = uID;

                var response = await clsCommon.Call_API_POSTMethod(Utility.Utilities.clsMethod.SkipMIFinalLesion, _MIFinalSkipLesionCRFData);
                if (response.IsSuccessStatusCode)
                {
                    var JsonString = await response.Content.ReadAsStringAsync();

                    //if (JsonString.Contains("1"))
                    if (JsonString == "1")
                    {
                        returnString = "success";
                    }
                    //else if (JsonString.Contains("success"))
                    else if (JsonString == "\"success\"")
                    {
                        returnString = "success";
                    }
                    else if (JsonString.Contains("RecordsNotFound"))
                    {
                        returnString = "RecordsNotFound";
                    }
                    else
                    {
                        returnString = "error";
                    }
                }
                //using (var client = new HttpClient())
                //{
                //    string strURL = System.Configuration.ConfigurationManager.AppSettings["Api_Url"] + Utility.Utilities.clsMethod.SkipMIFinalLesion;
                //    client.DefaultRequestHeaders.Accept.Clear();
                //    client.BaseAddress = new Uri(strURL);
                //    var serializedProduct_new = JsonConvert.SerializeObject(_MIFinalSkipLesionCRFData);
                //    var content = new StringContent(serializedProduct_new, Encoding.UTF8, "application/json");
                //    var response = await client.PostAsync(String.Format("{0}", strURL), content);
                //    if (response.IsSuccessStatusCode)
                //    {
                //        var JsonString = await response.Content.ReadAsStringAsync();
                //        //if (JsonString.Contains("1"))
                //        if (JsonString == "1")
                //        {
                //            returnString = "success";
                //        }
                //        //else if (JsonString.Contains("success"))
                //        else if (JsonString == "\"success\"")
                //        {
                //            returnString = "success";
                //        }
                //        else if (JsonString.Contains("RecordsNotFound"))
                //        {
                //            returnString = "RecordsNotFound";
                //        }
                //        else
                //        {
                //            returnString = "error";
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                returnString = "error";
                return returnString;
                //return "Error" + ex;
            }
            finally
            {
            }
            //return "success";
            return returnString;
        }

        [WebMethod]
        public string SUCCESS()
        {
            return "SUCCESS";
        }

        //PHASE 2 DEVELOPMENT

        public async Task<string> SubmitMIFinalLesionData(MISubmitMIFinalLesionData _MISubmitMIFinalLesionData)
        {
            _MISubmitMIFinalLesionData.DicomAnnotationDtl = Utilities.ConvertDataTable<DicomAnnotation>(JsonConvert.DeserializeObject<DataTable>(_MISubmitMIFinalLesionData.DicomAnnotationDetail), "");
            HashSet<DicomAnnotation> uniquedata = new HashSet<DicomAnnotation>(_MISubmitMIFinalLesionData.DicomAnnotationDtl); //Added by Bhargav Thaker 14Mar2023
            _MISubmitMIFinalLesionData.DicomAnnotationDtl = uniquedata.ToList(); //Added by Bhargav Thaker 14Mar2023

            DataTable dt_UploadDicom = new DataTable();
            dt_UploadDicom.Columns.Add("vFileName", typeof(string));
            dt_UploadDicom.Columns.Add("vServerPath", typeof(string));
            dt_UploadDicom.Columns.Add("vFileType", typeof(string));
            dt_UploadDicom.Columns.Add("vSize", typeof(string));
            dt_UploadDicom.Columns.Add("dScheduledDate", typeof(DateTime));

            _MISubmitMIFinalLesionData.MIFinalLesionImageList = Utility.Utilities.ConvertDataTable<MIFinalLesionImageList>(dt_UploadDicom, "");

            string returnString = string.Empty;
            try
            {
                if (_MISubmitMIFinalLesionData.iImgTransmittalHdrId == null || _MISubmitMIFinalLesionData.iImgTransmittalHdrId == 0)
                {
                    returnString = "SessionExpired";
                    return returnString;
                }

                if (Convert.ToInt32(System.Web.HttpContext.Current.Session["iuserid"]) == 0)
                {
                    returnString = "SessionExpired";
                    return returnString;
                }

                _MISubmitMIFinalLesionData.iImageMode = 2;
                int uID = Convert.ToInt32(System.Web.HttpContext.Current.Session["iuserid"]);
                _MISubmitMIFinalLesionData.UserID = uID;

                string URL = "SetData/SubmitMIFinalLesionData";
                var response = await clsCommon.Call_API_POSTMethod(URL, _MISubmitMIFinalLesionData);

                if (response.IsSuccessStatusCode)
                {
                    var JsonStringModality = await response.Content.ReadAsStringAsync();

                    var JsonString = await response.Content.ReadAsStringAsync();

                    if (JsonString == "\"success\"")
                    {
                        returnString = "success";
                        var respons = await SendEmail(_MISubmitMIFinalLesionData); // change at 09/12/2022 vishal Mameriya
                    }
                    else if (JsonString == "\"NO_LESION_DETAIL_FOUND\"")
                    {
                        returnString = "NO_LESION_DETAIL_FOUND";
                    }
                    else if (JsonString.Contains("RecordsNotFound"))
                    {
                        returnString = "RecordsNotFound";
                    }
                    else
                    {
                        returnString = "error";
                    }
                }

                //using (var client = new HttpClient())
                //{
                //    //string strURL = Utility.Utilities.clsDB.ApiUrl + Utility.Utilities.clsMethod.SubmitMIFinalLesionData;
                //    string strURL = System.Configuration.ConfigurationManager.AppSettings["Api_Url"] + Utility.Utilities.clsMethod.SubmitMIFinalLesionData;
                //    client.DefaultRequestHeaders.Accept.Clear();
                //    client.BaseAddress = new Uri(strURL);
                //    client.Timeout = TimeSpan.FromMinutes(10);
                //    var serializedProduct_new = JsonConvert.SerializeObject(_MISubmitMIFinalLesionData);
                //    var content = new StringContent(serializedProduct_new, Encoding.UTF8, "application/json");
                //    var response = await client.PostAsync(String.Format("{0}", strURL), content);
                //    if (response.IsSuccessStatusCode)
                //    {
                //        var JsonString = await response.Content.ReadAsStringAsync();

                //        if (JsonString == "\"success\"")
                //        {
                //            returnString = "success";
                //        }
                //        else if (JsonString == "\"NO_LESION_DETAIL_FOUND\"")
                //        {
                //            returnString = "NO_LESION_DETAIL_FOUND";
                //        }
                //        else if (JsonString.Contains("RecordsNotFound"))
                //        {
                //            returnString = "RecordsNotFound";
                //        }
                //        else
                //        {
                //            #region
                //            //***************************Do Not Delete For Reference and Future Use.**********************************//
                //            //************Prevent Image From Delete because when got error image get replaced in next try.************//

                //            //foreach (DataRow dr in dtSaveDicom.Rows)
                //            //{
                //            //    string destination = path + Convert.ToString(dr["vServerPath"]);
                //            //    if (System.IO.File.Exists(destination))
                //            //    {
                //            //        System.IO.File.Delete(destination);
                //            //    }
                //            //}

                //            //***************************Do Not Delete For Reference and Future Use.**********************************//
                //            //************Prevent Image From Delete because when got error image get replaced in next try.************//
                //            #endregion
                //            returnString = "error";

                //        }
                //    }
                //}
                return returnString;
            }
            catch (Exception e)
            {
                return "Error : " + e.InnerException;
            }
        }

        [WebMethod]
        public async Task<string> SkipMIFinalLesionData(MISkipLesionCRFDetail _MISkipLesionCRFDetail)
        {
            string returnString = string.Empty;

            try
            {
                int uID = Convert.ToInt32(System.Web.HttpContext.Current.Session["iuserid"]);
                _MISkipLesionCRFDetail.UserID = uID;

                if (Convert.ToInt32(System.Web.HttpContext.Current.Session["iuserid"]) == 0)
                {
                    returnString = "SessionExpired";
                    return returnString;
                }

                var response = await clsCommon.Call_API_POSTMethod(Utility.Utilities.clsMethod.SkipMIFinalLesionData, _MISkipLesionCRFDetail);
                if (response.IsSuccessStatusCode)
                {
                    var JsonString = await response.Content.ReadAsStringAsync();

                    if (JsonString == "\"success\"")
                    {
                        returnString = "success";
                        var respons = await SendEmail(_MISkipLesionCRFDetail);
                    }
                    else if (JsonString == "\"NO_LESION_DETAIL_FOUND\"")
                    {
                        returnString = "NO_LESION_DETAIL_FOUND";
                    }
                    else if (JsonString.Contains("RecordsNotFound"))
                    {
                        returnString = "RecordsNotFound";
                    }
                    else
                    {
                        returnString = "error";
                    }
                }

                //using (var client = new HttpClient())
                //{
                //    string strURL = System.Configuration.ConfigurationManager.AppSettings["Api_Url"] + Utility.Utilities.clsMethod.SkipMIFinalLesionData;
                //    client.DefaultRequestHeaders.Accept.Clear();
                //    client.BaseAddress = new Uri(strURL);
                //    var serializedProduct_new = JsonConvert.SerializeObject(_MISkipLesionCRFDetail);
                //    var content = new StringContent(serializedProduct_new, Encoding.UTF8, "application/json");
                //    var response = await client.PostAsync(String.Format("{0}", strURL), content);
                //    if (response.IsSuccessStatusCode)
                //    {
                //        var JsonString = await response.Content.ReadAsStringAsync();
                //        if (JsonString == "\"success\"")
                //        {
                //            returnString = "success";
                //        }
                //        else if (JsonString == "\"NO_LESION_DETAIL_FOUND\"")
                //        {
                //            returnString = "NO_LESION_DETAIL_FOUND";
                //        }
                //        else if (JsonString.Contains("RecordsNotFound"))
                //        {
                //            returnString = "RecordsNotFound";
                //        }
                //        else
                //        {
                //            returnString = "error";
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                returnString = "error";
                return returnString;
                //return "Error" + ex;
            }
            finally
            {
            }
            //return "success";
            return returnString;
        }

        public async Task<string> SaveMIFinalLession(MIFinalLesionDetailDTO _MIFinalLesionDetailDTO)
        {
            string returnString = string.Empty;

            if (Convert.ToInt32(System.Web.HttpContext.Current.Session["iuserid"]) == 0)
            {
                returnString = "SessionExpired";
                return returnString;
            }

            try
            {
                var response = await clsCommon.Call_API_POSTMethod(Utility.Utilities.clsMethod.SaveMIFinalLession, _MIFinalLesionDetailDTO);
                if (response.IsSuccessStatusCode)
                {
                    var JsonString = await response.Content.ReadAsStringAsync();

                    //DataTable DT = JsonConvert.DeserializeObject<DataTable>(JsonString);
                    if (JsonString == "[{\"Status\":\"1\"}]")
                    {
                        returnString = "1";
                    }
                    else
                    {
                        returnString = "error";
                    }
                }
                //using (var client = new HttpClient())
                //{
                //    string strURL = System.Configuration.ConfigurationManager.AppSettings["Api_Url"] + Utility.Utilities.clsMethod.SaveMIFinalLession;
                //    client.DefaultRequestHeaders.Accept.Clear();
                //    client.BaseAddress = new Uri(strURL);
                //    var serializedProduct_new = JsonConvert.SerializeObject(_MIFinalLesionDetailDTO);
                //    var content = new StringContent(serializedProduct_new, Encoding.UTF8, "application/json");
                //    var response = await client.PostAsync(String.Format("{0}", strURL), content);
                //    if (response.IsSuccessStatusCode)
                //    {
                //        var JsonString = await response.Content.ReadAsStringAsync();
                //        //DataTable DT = JsonConvert.DeserializeObject<DataTable>(JsonString);
                //        if (JsonString == "[{\"Status\":\"1\"}]")
                //        {
                //            returnString = "1";
                //        }                        
                //        else
                //        {
                //            returnString = "error";
                //        }                        
                //    }
                //}
            }
            catch (Exception ex)
            {
                returnString = "error";
                return returnString;
            }
            finally
            {
            }
            return returnString;
        }

        public async Task<string> SendEmail(MISubmitMIFinalLesionData _MISubmitMIFinalLesionData)
        {
            string returnString = string.Empty;
            System.Net.Mail.MailMessage objMailMessage = new System.Net.Mail.MailMessage();
            System.Net.Mail.SmtpClient objSmtpClient = new System.Net.Mail.SmtpClient();
            string ToEmail = string.Empty;
            int i, j;
            StringBuilder strMessage = new StringBuilder();
            StringBuilder strCertificate = new StringBuilder();

            DataTable dt = new DataTable();
            DataTable DtEmail = new DataTable();
            DataTable DT_Email = new DataTable();

            string strFromMail = string.Empty;

            //WS_HelpDbTable objHelp = GetHelpDbTableRef();

            string FromMail = string.Empty;
            string Link = string.Empty;
            string RoleNo = string.Empty;
            string LiveLink = string.Empty;
            string locallink = string.Empty;
            string wStr = string.Empty;
            string ToUser = string.Empty;
            string EmailSubject = string.Empty;
            string SetNo = string.Empty;
            string Site_No = string.Empty;
            string StudyProtocol = string.Empty; //Convert.ToString(ds_GetselectedChild.Tables(0).Rows(0).Item("ProtocolNo"))
            string Site_PI = string.Empty; //Convert.ToString(ds_GetselectedChild.Tables(0).Rows(0).Item("SitePI"))
            string UploadedBy = string.Empty;
            SS.Mail.TenantInfo TI = new SS.Mail.TenantInfo();
            DataSet dsEmail = new DataSet();

            var dt_Listofreview = new DataTable();
            var dt_ListofUser = new DataTable();
            //DataSet ds = new DataSet();

            try
            {
                string UserType = Convert.ToString(System.Web.HttpContext.Current.Session["UserTypeCode"]);
                string vOperationCode = "";
                if (UserType == "0118")
                    vOperationCode = "5";
                else if (UserType == "0119")
                    vOperationCode = "6";
                else
                    vOperationCode = "";

                MI_DataSaveStatus _MI_DataSaveStatus = new MI_DataSaveStatus();
                _MI_DataSaveStatus.vWorkspaceId = _MISubmitMIFinalLesionData.vWorkspaceId;
                _MI_DataSaveStatus.vSubjectId = _MISubmitMIFinalLesionData.vSubjectId;
                _MI_DataSaveStatus.vActivityId = _MISubmitMIFinalLesionData.vActivityId;
                _MI_DataSaveStatus.iNodeId = _MISubmitMIFinalLesionData.iNodeId;
                _MI_DataSaveStatus.vOperationCode = vOperationCode;
                _MI_DataSaveStatus.iImgTransmittalHdrId = Convert.ToInt32(_MISubmitMIFinalLesionData.iImgTransmittalHdrId);
                _MI_DataSaveStatus.iImgTransmittalDtlId = Convert.ToInt32(_MISubmitMIFinalLesionData.iImgTransmittalDtlId);
                _MI_DataSaveStatus.cRadiologist = _MISubmitMIFinalLesionData.cRadiologist; //Added by Bhargav Thaker 15Mar2023
                _MI_DataSaveStatus.iUserId = _MISubmitMIFinalLesionData.UserID.ToString(); //Added by Bhargav Thaker 15Mar2023
                _MI_DataSaveStatus.iParentNodeId = _MISubmitMIFinalLesionData.iParentNodeId; //Added by Bhargav Thaker 15Mar2023

                using (var client = new HttpClient())
                {
                    string strURL = System.Configuration.ConfigurationManager.AppSettings["Api_Url"] + "GetData/MIGetImgTransmittalDtlForQCReview";
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.BaseAddress = new Uri(strURL);
                    client.Timeout = TimeSpan.FromMinutes(10);
                    var serializedProduct_new = JsonConvert.SerializeObject(_MI_DataSaveStatus);
                    var content = new StringContent(serializedProduct_new, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(String.Format("{0}", strURL), content);
                    string otp = string.Empty;
                    string OTPCode = otp;
                    _MISubmitMIFinalLesionData.Otp = otp;

                    string iuserid = Convert.ToString(System.Web.HttpContext.Current.Session["iuserId"]);
                    string vLocationCode = Convert.ToString(System.Web.HttpContext.Current.Session["vLocationCode"]);

                    //_MISubmitMIFinalLesionData.UserID = iuserid.ToString();
                    _MISubmitMIFinalLesionData.vLocationCode = vLocationCode.ToString();
                    _MISubmitMIFinalLesionData.vOperationCode = vOperationCode.ToString();
                    //_MISubmitMIFinalLesionData.UserID = iuserid.ToString();
                    string URL4 = "GetData/GetSmsDetails";
                    DataTable dt_SMS = await _ClsCommon.Call_API_GeTMethod(URL4, _MISubmitMIFinalLesionData);

                    if (response.IsSuccessStatusCode)
                    {
                        var JsonStringModality = response.Content.ReadAsStringAsync().Result;
                        DataSet ds = JsonConvert.DeserializeObject<DataSet>(JsonStringModality);

                        //If-Else condition Added by Bhargav Thaker 15Mar2023
                        var Splitsubactivity = _MISubmitMIFinalLesionData.vSubActivityName.Split('-');
                        string SubActivity = Splitsubactivity[1].Trim();

                        int res = Convert.ToInt32(ds.Tables[2].Rows[0]["Result"]);
                        if (res != 0)
                        {
                            returnString = "True";
                            return returnString;
                        }
                        else
                        {
                            if (_MISubmitMIFinalLesionData.vActivityName.ToUpper().Trim() == "BL")
                            {
                                if (SubActivity.ToUpper() != "NTL")
                                {
                                    returnString = "True";
                                    return returnString;
                                }
                            }
                            else
                            {
                                if (SubActivity.ToUpper() != "NL")
                                {
                                    returnString = "True";
                                    return returnString;
                                }
                            }

                            TI.TenantId = dt_SMS.Rows[0]["vTenantId"].ToString();
                            TI.ClientId = dt_SMS.Rows[0]["vClientId"].ToString();
                            TI.Client_secret = dt_SMS.Rows[0]["vSecretKey"].ToString();
                            TI.EmailUser = dt_SMS.Rows[0]["vFromEmail"].ToString();
                            TI.EmailPassword = dt_SMS.Rows[0]["vPassword"].ToString();
                            bool AuthenticationWithSecretKey = Convert.ToBoolean(dt_SMS.Rows[0]["bAuthSecretKey"].ToString());
                            string FromMailId = dt_SMS.Rows[0]["vFromEmail"].ToString();

                            if (vOperationCode == "5")
                                //EmailSubject = "DiSoftC - Radiologist1 Review for Screening -  " + ds.Tables[0].Rows[0]["ScreeningNo"].ToString(); //Commented by Bhargav Thaker 14March2023
                                EmailSubject = "DiSoftC - Radiologist1 Review for " + _MISubmitMIFinalLesionData.vActivityName.ToString().Trim() + " - " + ds.Tables[0].Rows[0]["ScreeningNo"].ToString(); //Added by Bhargav Thaker 14March2023
                            else if (vOperationCode == "6")
                                //EmailSubject = "DiSoftC - Radiologist2 Review for ScreeningNo - " + ds.Tables[0].Rows[0]["ScreeningNo"].ToString(); //Commented by Bhargav Thaker 14March2023
                                EmailSubject = "DiSoftC - Radiologist2 Review for " + _MISubmitMIFinalLesionData.vActivityName.ToString().Trim() + " - " + ds.Tables[0].Rows[0]["ScreeningNo"].ToString(); //Added by Bhargav Thaker 14March2023

                            string myHtmlFile = string.Empty;
                            string EmailBody = string.Empty;
                            StringBuilder myBuilder = new System.Text.StringBuilder();

                            myBuilder.Append("<!DOCTYPE html>");
                            myBuilder.Append("<html><head>");
                            myBuilder.Append("<title>");
                            myBuilder.Append("Page-");
                            myBuilder.Append("</title>");
                            myBuilder.Append("<style>");
                            myBuilder.Append("body{ font-family:'Times New Roman','Times Roman','Verdana';}");
                            myBuilder.Append("table{border-collapse: collapse;border-width: 1px;border-color:rgb(0,0,0);}");
                            myBuilder.Append("thead{}");
                            myBuilder.Append("th{padding:8px 30px 8px 8px;border-bottom-color:rgb(0,0,0);border-width: 0px 1px 2px 1px;}");
                            myBuilder.Append("td{padding:5px 30px 5px 5px;}");
                            myBuilder.Append("</style>");
                            myBuilder.Append("</head>");
                            myBuilder.Append("<body>");
                            EmailBody = "";
                            EmailBody += "<table align='left' cellspacing='2' border='0' width='100%' style='table-layout: fixed;'>";
                            EmailBody += "<tr>";
                            EmailBody += "<td> Dear All,</td>";
                            EmailBody += "</tr>";
                            EmailBody += "<tr>";
                            //EmailBody += "<td> The image(s) has been reviewed and following is the status:</td>"; //Commented by Bhargav Thaker 14March2023
                            EmailBody += "<td> The image(s) has been reviewed and following is the details:</td>"; //Added by Bhargav Thaker 14March2023
                            EmailBody += "</tr>";
                            EmailBody += "<tr>";
                            EmailBody += "<td> Study Protocol: " + Convert.ToString(ds.Tables[0].Rows[0]["ProtocolNo"]).ToString() + " </td>";
                            EmailBody += "</tr>";
                            EmailBody += "<tr>";
                            EmailBody += "<td> Site Number: " + Convert.ToString(ds.Tables[0].Rows[0]["ProjectNo"]).ToString() + " </td>";
                            EmailBody += "</tr>";
                            EmailBody += "<tr>"; //Added by Bhargav Thaker 14March2023
                            EmailBody += "<td> Screening Number: " + ds.Tables[0].Rows[0]["ScreeningNo"].ToString().Trim() + " </td>"; //Added by Bhargav Thaker 14March2023
                            EmailBody += "</tr>"; //Added by Bhargav Thaker 14March2023
                            EmailBody += "<tr>";
                            EmailBody += "<td> ActivityName: " + _MISubmitMIFinalLesionData.vActivityName.ToString() + " </td>";
                            EmailBody += "</tr>";
                            if (_MISubmitMIFinalLesionData.vActivityName.ToString().Trim().ToUpper() == "BL")
                            {
                                EmailBody += "<tr>";
                                EmailBody += "<td> Overall Response: " + string.Empty + " </td>";
                                EmailBody += "</tr>";
                                EmailBody += "<tr>";
                                EmailBody += "<td> Remark: " + string.Empty + " </td>";
                                EmailBody += "</tr>";
                            }
                            else
                            {
                                EmailBody += "<tr>";
                                EmailBody += "<td> Overall Response: " + ds.Tables[0].Rows[0]["Global_Response"].ToString().Trim() + " </td>";
                                EmailBody += "</tr>";
                                EmailBody += "<tr>";
                                EmailBody += "<td> Remark: " + ds.Tables[0].Rows[0]["Remarks"].ToString().Trim() + " </td>";
                                EmailBody += "</tr>";
                            }
                            //EmailBody += "<tr>"; //Commented by Bhargav Thaker 17March2023
                            //EmailBody += "<td>  Examination Date: " + Convert.ToString(ds.Tables[0].Rows[0]["DatePerformed"]).ToString() + " </td>"; //Commented by Bhargav Thaker 17March2023
                            //EmailBody += "</tr>"; //Commented by Bhargav Thaker 17March2023
                            EmailBody += "<tr>"; //Modify by Bhargav Thaker 17March2023
                            EmailBody += "<td>  Examination Date: " + Convert.ToDateTime(ds.Tables[0].Rows[0]["DatePerformed"]).ToString("dd-MMM-yyyy") + " </td>"; //Modify by Bhargav Thaker 17March2023
                            EmailBody += "</tr>"; //Modify by Bhargav Thaker 17March2023
                            EmailBody += "<tr>";
                            EmailBody += "<td> Reviewed Date: " + DateTime.Now.ToString("dd-MMM-yyyy") + "</ td>";
                            EmailBody += "</tr>";
                            //EmailBody += "<tr>"; //Commented by Bhargav Thaker 14March2023
                            //EmailBody += "<td> Reviewed By: " + Convert.ToString(System.Web.HttpContext.Current.Session["UserNameWithProfile"]).ToString() + "</ td>"; //Commented by Bhargav Thaker 14March2023
                            //EmailBody += "</tr>"; //Commented by Bhargav Thaker 14March2023
                            EmailBody += "</table>";
                            EmailBody += "	<br><br><br>";
                            myBuilder.Append(EmailBody);
                            EmailBody = String.Empty;
                            EmailBody += "	<br>";
                            EmailBody += "	<table align='left' cellspacing='2' border='0' width='100%' style='table-layout: fixed;'>";
                            EmailBody += "		<tr>";
                            EmailBody += "				<td>This is a system generated email. Please do not reply directly to this mail. In case you have any questions please contact DI-Soft-C team and we will get back to you as soon as possible.</td>";
                            EmailBody += "		</tr>		";
                            EmailBody += "		<tr>";
                            EmailBody += "				<td>&nbsp;</td>";
                            EmailBody += "		</tr>		";
                            EmailBody += "		<tr>";
                            EmailBody += "				<td>Thanks,</td>";
                            EmailBody += "		</tr>		";
                            EmailBody += "		<tr>";
                            EmailBody += "				<td>DI-Soft-C Team</ td>";
                            EmailBody += "		</tr>";
                            EmailBody += "	</table>";

                            myBuilder.Append(EmailBody);
                            myBuilder.Append("</body>");
                            myBuilder.Append("</html>");
                            myHtmlFile = myBuilder.ToString();

                            string URL5 = "GetData/GetEmailForTranstrion";
                            DataTable dt_Email = await _ClsCommon.Call_API_GeTMethod(URL5, _MISubmitMIFinalLesionData);

                            if (dt_Email.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt_Email.Rows)
                                {
                                    ToUser += dr["vEmailId"].ToString() + ",";
                                    //ToUser += dt_dr["vEmailId"].ToString() + ",";
                                }
                                ToUser = ToUser.Substring(0, ToUser.LastIndexOf(","));
                                //var emailIds = ToUser.Trim().Split(',');
                            }

                            var emailIds = ToUser.Trim().Split(',');
                            List<Recipient> ItemsList = new List<Recipient>();
                            foreach (var drToEmail in emailIds)
                                ItemsList.Add(new Recipient() { EmailAddress = new EmailAddress() { Address = Convert.ToString(drToEmail.Trim()) } });
                            var message = new Microsoft.Graph.Message()
                            {
                                Subject = EmailSubject.ToString(),
                                Body = new ItemBody() { ContentType = BodyType.Html, Content = myHtmlFile.ToString() },
                                ToRecipients = new List<Recipient>(), //new List<Recipient>() { new Recipient() { EmailAddress = new EmailAddress() { Address = emailIds.Trim() } } }
                                BccRecipients = ItemsList
                            };
                            //.BccRecipients Added by Bhargav Thaker 24Feb2023

                            try
                            {
                                if (AuthenticationWithSecretKey == false) { SS.Mail.EmailSender.SendMailUsingPassword(TI, message, true); }
                                else { SS.Mail.EmailSender.sendMailBySecret(TI, message, true); }

                                await ExMsgInfoDetails("Mail", EmailSubject, myHtmlFile.ToString(), FromMailId, ToUser, iuserid, "Y", "", "");
                            }
                            catch (Exception ex)
                            {
                                await ExMsgInfoDetails("Mail", EmailSubject, myHtmlFile.ToString(), TI.EmailUser, ToUser, iuserid, "N", ex.Message, "");
                            }
                            //await ExMsgInfoDetails("Mail", EmailSubject, sBody.ToString(), FromMailId, vEmailId, iuserid, "Y", "", "");
                        }
                    }
                    returnString = "True";
                    return returnString;
                }
            }
            catch (Exception e)
            {
                return "Error : " + e.InnerException;
            }
        }

        public async Task<string> SendEmail(MISkipLesionCRFDetail _MISkipLesionCRFDetail)
        {
            string returnString = string.Empty;
            System.Net.Mail.MailMessage objMailMessage = new System.Net.Mail.MailMessage();
            System.Net.Mail.SmtpClient objSmtpClient = new System.Net.Mail.SmtpClient();
            string ToEmail = string.Empty;
            int i, j;
            StringBuilder strMessage = new StringBuilder();
            StringBuilder strCertificate = new StringBuilder();
            DataTable dt = new DataTable();
            DataTable DtEmail = new DataTable();
            DataTable DT_Email = new DataTable();
            string strFromMail = string.Empty;
            //WS_HelpDbTable objHelp = GetHelpDbTableRef();
            string FromMail = string.Empty;
            string Link = string.Empty;
            string RoleNo = string.Empty;
            string LiveLink = string.Empty;
            string locallink = string.Empty;
            string wStr = string.Empty;
            string ToUser = string.Empty;
            string EmailSubject = string.Empty;
            string SetNo = string.Empty;
            string Site_No = string.Empty;
            string StudyProtocol = string.Empty; //Convert.ToString(ds_GetselectedChild.Tables(0).Rows(0).Item("ProtocolNo"))
            string Site_PI = string.Empty; //Convert.ToString(ds_GetselectedChild.Tables(0).Rows(0).Item("SitePI"))
            string UploadedBy = string.Empty;
            SS.Mail.TenantInfo TI = new SS.Mail.TenantInfo();
            DataSet dsEmail = new DataSet();
            //string emailIds = string.Empty;
            var dt_Listofreview = new DataTable();
            var dt_ListofUser = new DataTable();
            //DataSet ds = new DataSet();

            try
            {
                string UserType = Convert.ToString(System.Web.HttpContext.Current.Session["UserTypeCode"]);
                string vOperationCode = "";
                if (UserType == "0123")
                    vOperationCode = "7";
                else
                    vOperationCode = "";

                MI_DataSaveStatus _MI_DataSaveStatus = new MI_DataSaveStatus();
                _MI_DataSaveStatus.vWorkspaceId = _MISkipLesionCRFDetail.vWorkspaceId;
                _MI_DataSaveStatus.vSubjectId = _MISkipLesionCRFDetail.vSubjectId;
                _MI_DataSaveStatus.vActivityId = _MISkipLesionCRFDetail.vActivityId;
                _MI_DataSaveStatus.iNodeId = _MISkipLesionCRFDetail.iNodeId;
                _MI_DataSaveStatus.vOperationCode = vOperationCode;
                _MI_DataSaveStatus.iImgTransmittalHdrId = Convert.ToInt32(_MISkipLesionCRFDetail.iImgTransmittalHdrId);
                _MI_DataSaveStatus.iImgTransmittalDtlId = Convert.ToInt32(_MISkipLesionCRFDetail.iImgTransmittalDtlId);
                _MI_DataSaveStatus.cRadiologist = _MISkipLesionCRFDetail.cRadiologist; //Added by Bhargav Thaker 17Mar2023
                _MI_DataSaveStatus.iUserId = _MISkipLesionCRFDetail.UserID.ToString(); //Added by Bhargav Thaker 17Mar2023
                _MI_DataSaveStatus.iParentNodeId = "0"; //Added by Bhargav Thaker 17Mar2023

                using (var client = new HttpClient())
                {
                    string strURL = System.Configuration.ConfigurationManager.AppSettings["Api_Url"] + "GetData/MIGetImgTransmittalDtlForQCReview";
                    //dt_ListofUser = await _ClsCommon.Call_API_GETMethodDT(strURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.BaseAddress = new Uri(strURL);
                    client.Timeout = TimeSpan.FromMinutes(10);
                    var serializedProduct_new = JsonConvert.SerializeObject(_MI_DataSaveStatus);
                    var content = new StringContent(serializedProduct_new, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(String.Format("{0}", strURL), content);

                    //string URL = System.Configuration.ConfigurationManager.AppSettings["Api_Url"] + "GetData/MIGetImgTransmittalDtlForQCReview";
                    //dt_ListofUser = await _ClsCommon.Call_API_GETMethodDT(strURL);
                    string otp = string.Empty;
                    string OTPCode = otp;
                    _MISkipLesionCRFDetail.Otp = otp;

                    string iuserid = Convert.ToString(System.Web.HttpContext.Current.Session["iuserId"]);
                    string vLocationCode = Convert.ToString(System.Web.HttpContext.Current.Session["vLocationCode"]);

                    //_MISubmitMIFinalLesionData.UserID = iuserid.ToString();
                    _MISkipLesionCRFDetail.vLocationCode = vLocationCode.ToString();
                    _MISkipLesionCRFDetail.vOperationCode = vOperationCode.ToString();
                    //_MISubmitMIFinalLesionData.UserID = iuserid.ToString();
                    string URL4 = "GetData/GetSmsDetails";
                    DataTable dt_SMS = await _ClsCommon.Call_API_GeTMethod(URL4, _MISkipLesionCRFDetail);

                    if (response.IsSuccessStatusCode)
                    {
                        var JsonStringModality = response.Content.ReadAsStringAsync().Result;
                        DataSet ds = JsonConvert.DeserializeObject<DataSet>(JsonStringModality);

                        TI.TenantId = dt_SMS.Rows[0]["vTenantId"].ToString();
                        TI.ClientId = dt_SMS.Rows[0]["vClientId"].ToString();
                        TI.Client_secret = dt_SMS.Rows[0]["vSecretKey"].ToString();
                        TI.EmailUser = dt_SMS.Rows[0]["vFromEmail"].ToString();
                        TI.EmailPassword = dt_SMS.Rows[0]["vPassword"].ToString();
                        bool AuthenticationWithSecretKey = Convert.ToBoolean(dt_SMS.Rows[0]["bAuthSecretKey"].ToString());
                        string FromMailId = dt_SMS.Rows[0]["vFromEmail"].ToString();

                        //EmailSubject = "DiSoftC - Adjudicator Review for Screening -  " + ds.Tables[0].Rows[0]["ScreeningNo"].ToString(); //Commented by Bhargav Thaker 17March2023
                        EmailSubject = "DiSoftC - Adjudicator Review for "+ ds.Tables[0].Rows[0]["Adj_Activity"].ToString().Trim() + " - " + ds.Tables[0].Rows[0]["ScreeningNo"].ToString(); //Modify by Bhargav Thaker 17March2023

                        string myHtmlFile = string.Empty;
                        string EmailBody = string.Empty;
                        StringBuilder myBuilder = new System.Text.StringBuilder();

                        myBuilder.Append("<!DOCTYPE html>");
                        myBuilder.Append("<html><head>");
                        myBuilder.Append("<title>");
                        myBuilder.Append("Page-");
                        myBuilder.Append("</title>");
                        myBuilder.Append("<style>");
                        myBuilder.Append("body{ font-family:'Times New Roman','Times Roman','Verdana';}");
                        myBuilder.Append("table{border-collapse: collapse;border-width: 1px;border-color:rgb(0,0,0);}");
                        myBuilder.Append("thead{}");
                        myBuilder.Append("th{padding:8px 30px 8px 8px;border-bottom-color:rgb(0,0,0);border-width: 0px 1px 2px 1px;}");
                        myBuilder.Append("td{padding:5px 30px 5px 5px;}");
                        myBuilder.Append("</style>");
                        myBuilder.Append("</head>");
                        myBuilder.Append("<body>");
                        EmailBody = "";
                        EmailBody += "<table align='left' cellspacing='2' border='0' width='100%' style='table-layout: fixed;'>";
                        EmailBody += "<tr>";
                        EmailBody += "<td> Dear All,</td>";
                        EmailBody += "</tr>";
                        EmailBody += "<tr>";
                        //EmailBody += "<td> The image(s) has been reviewed and following is the status:</td>"; //Commented by Bhargav Thaker 17March2023
                        EmailBody += "<td> The image(s) has been reviewed and following is the details:</td>";
                        EmailBody += "</tr>";
                        EmailBody += "<tr>";
                        EmailBody += "<td> Study Protocol: " + Convert.ToString(ds.Tables[0].Rows[0]["ProtocolNo"]).ToString() + " </td>";
                        EmailBody += "</tr>";
                        EmailBody += "<tr>";
                        EmailBody += "<td> Site Number: " + Convert.ToString(ds.Tables[0].Rows[0]["ProjectNo"]).ToString() + " </td>";
                        EmailBody += "</tr>";
                        EmailBody += "<tr>"; //Added by Bhargav Thaker 17March2023
                        EmailBody += "<td> Screening Number: " + ds.Tables[0].Rows[0]["ScreeningNo"].ToString().Trim() + " </td>"; //Added by Bhargav Thaker 17March2023
                        EmailBody += "</tr>"; //Added by Bhargav Thaker 17March2023
                        EmailBody += "<tr>";
                        //EmailBody += "<td> ActivityName: " + this.Request.Params["vActivityName"] + " </td>";
                        EmailBody += "<td> ActivityName: " + ds.Tables[0].Rows[0]["Adj_Activity"].ToString().Trim() + " </td>";
                        EmailBody += "</tr>";
                        //EmailBody += "<tr>"; //Added by Bhargav Thaker 17March2023
                        //EmailBody += "<td> Overall Response: " + ds.Tables[0].Rows[0]["Adj_Response"].ToString().Trim() + " </td>"; //Added by Bhargav Thaker 17March2023
                        //EmailBody += "</tr>"; //Added by Bhargav Thaker 17March2023
                        //EmailBody += "<tr>"; //Added by Bhargav Thaker 17March2023
                        //EmailBody += "<td> Remark: " + ds.Tables[0].Rows[0]["Adj_Remarks"].ToString().Trim() + " </td>"; //Added by Bhargav Thaker 17March2023
                        //EmailBody += "</tr>"; //Added by Bhargav Thaker 17March2023
                        //EmailBody += "<tr>";
                        //EmailBody += "<td> Remark: " + _MISkipLesionCRFDetail.vRemark.ToString() + " </td>";
                        //EmailBody += "</tr>";
                        EmailBody += "<tr>";
                        //EmailBody += "<td>  Examination Date: " + Convert.ToString(ds.Tables[0].Rows[0]["DatePerformed"]).ToString() + " </td>"; //Commented by Bhargav Thaker 17March2023
                        EmailBody += "<td> Examination Date: " + Convert.ToDateTime(ds.Tables[0].Rows[0]["DatePerformed"]).ToString("dd-MMM-yyyy") + " </td>"; //Modify by Bhargav Thaker 17March2023
                        EmailBody += "</tr>";
                        EmailBody += "<tr>";
                        EmailBody += "<td> Reviewed Date: " + DateTime.Now.ToString("dd-MMM-yyyy") + "</ td>";
                        EmailBody += "</tr>";
                        //EmailBody += "<tr>"; //Commented by Bhargav Thaker 17March2023
                        //EmailBody += "<td> Reviewed By: " + Convert.ToString(System.Web.HttpContext.Current.Session["UserNameWithProfile"]).ToString() + "</ td>"; //Commented by Bhargav Thaker 17March2023
                        //EmailBody += "</tr>"; //Commented by Bhargav Thaker 17March2023
                        EmailBody += "</table>";
                        EmailBody += "	<br><br><br>";
                        myBuilder.Append(EmailBody);
                        EmailBody = String.Empty;
                        EmailBody += "	<br>";
                        EmailBody += "	<table align='left' cellspacing='2' border='0' width='100%' style='table-layout: fixed;'>";
                        EmailBody += "		<tr>";
                        EmailBody += "				<td>This is a system generated email. Please do not reply directly to this mail. In case you have any questions please contact DI-Soft-C team and we will get back to you as soon as possible.</td>";
                        EmailBody += "		</tr>		";
                        EmailBody += "		<tr>";
                        EmailBody += "				<td>&nbsp;</td>";
                        EmailBody += "		</tr>		";
                        EmailBody += "		<tr>";
                        EmailBody += "				<td>Thanks,</td>";
                        EmailBody += "		</tr>		";
                        EmailBody += "		<tr>";
                        EmailBody += "				<td>DI-Soft-C Team</ td>";
                        EmailBody += "		</tr>";
                        EmailBody += "	</table>";

                        myBuilder.Append(EmailBody);
                        myBuilder.Append("</body>");
                        myBuilder.Append("</html>");
                        myHtmlFile = myBuilder.ToString();

                        string URL5 = "GetData/GetEmailForTranstrion";
                        DataTable dt_Email = await _ClsCommon.Call_API_GeTMethod(URL5, _MISkipLesionCRFDetail);


                        if (dt_Email.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt_Email.Rows)
                            {
                                ToUser += dr["vEmailId"].ToString() + ",";
                            }
                            ToUser = ToUser.Substring(0, ToUser.LastIndexOf(","));
                        }

                        var emailIds = ToUser.Trim().Split(',');
                        List<Recipient> ItemsList = new List<Recipient>();
                        foreach (var drToEmail in emailIds)
                            ItemsList.Add(new Recipient() { EmailAddress = new EmailAddress() { Address = Convert.ToString(drToEmail.Trim()) } });
                        var message = new Microsoft.Graph.Message()
                        {
                            Subject = EmailSubject.ToString(),
                            Body = new ItemBody() { ContentType = BodyType.Html, Content = myHtmlFile.ToString() },
                            ToRecipients = new List<Recipient>(), //new List<Recipient>() { new Recipient() { EmailAddress = new EmailAddress() { Address = emailIds.Trim() } } }
                            BccRecipients = ItemsList
                        };
                        //.BccRecipients Added by Bhargav Thaker 24Feb2023

                        try
                        {
                            if (AuthenticationWithSecretKey == false) { SS.Mail.EmailSender.SendMailUsingPassword(TI, message, true); }
                            else { SS.Mail.EmailSender.sendMailBySecret(TI, message, true); }

                            await ExMsgInfoDetails("Mail", EmailSubject, myHtmlFile.ToString(), FromMailId, ToUser, iuserid, "Y", "", "");
                        }
                        catch (Exception ex)
                        {
                            await ExMsgInfoDetails("Mail", EmailSubject, myHtmlFile.ToString(), TI.EmailUser, ToUser, iuserid, "N", ex.Message, "");
                        }
                        //await ExMsgInfoDetails("Mail", EmailSubject, sBody.ToString(), FromMailId, vEmailId, iuserid, "Y", "", "");
                    }
                    returnString = "True";
                    return returnString;
                }
            }
            catch (Exception e)
            {
                return "Error : " + e.InnerException;
            }
        }

        public async Task<string> ExMsgInfoDetails(string vNotificationType, string vSubject, string vBody, string vFromEmailId, string vToEmailId,
           string iCreatedBy, string cIsSent, string vRemarks, string Mobile_No)
        {
            DTO.ExMsgInfo obj = new DTO.ExMsgInfo();
            obj.vNotificationType = vNotificationType;
            obj.vSubject = vSubject;
            obj.vBody = vBody;
            obj.vFromEmailId = vFromEmailId;
            obj.vToEmailId = string.Empty; //Modify by Bhargav Thaker 23Feb2023
            obj.vBCCEmailId = vToEmailId; //Added by Bhargav Thaker 24Feb2023
            obj.iCreatedBy = iCreatedBy;
            obj.dCreatedDate = DateTime.Now.ToString();
            obj.vPhoneNo = Mobile_No;
            obj.cIsSent = cIsSent;
            obj.dSentDate = cIsSent == "Y" ? DateTime.Now.ToString() : "1900-01-01";
            obj.vRemarks = vRemarks;

            string URL3 = "SetData/Insert_Exmsg";
            string result = await _ClsCommon.Insert_APITransaction(URL3, obj);

            return "success";
        }

        //Added by Bhargav Thaker
        [WebMethod]
        public async Task<string> GetStudyDetails(string ParentWorkSpaceId)
        {
            string result = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                GetMyProjectCompletionListDTO _GetMyProjectCompletionListDTO = new GetMyProjectCompletionListDTO();
                _GetMyProjectCompletionListDTO.contextKey = "vWorkSpaceId='" + ParentWorkSpaceId + "'";
                _GetMyProjectCompletionListDTO.vProjectTypeCode = "-1"; //-1 for identification
                string URL = "GetData/MyStudyCompletionList";
                dt = await _ClsCommon.Call_API_GeTMethod(URL, _GetMyProjectCompletionListDTO);
                result = dt.Rows[0]["StudyName"].ToString();
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }
            return result;
        }

    }
}