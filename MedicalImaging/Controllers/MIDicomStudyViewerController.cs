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

namespace MedicalImaging.Controllers
{
    public class MIDicomStudyViewerController : Controller
    {
        //
        // GET: /MIDicomStudyViewer/


        DataTable dtUploadDicom = new DataTable();
        DataTable dtSaveDicom = new DataTable();
        int DicomCount;
        int iImgTransmittalHdrId, iImgTransmittalDtlId, iModifyBy;
        string iImageStatus, cStatusIndi;
        DateTime dModifyOn;
        string vWorkspaceId, vProjectNo, vSubjectId, iNodeId, iModalityNo, iAnatomyNo, vSize, vFinalSize;

        public ActionResult MIDicomStudyViewer()
        {

            Response.AddHeader("Cache-Control", "no-cache, no-store,must-revalidate");
            Response.AddHeader("Pragma", "no-cache");
            Response.AddHeader("Expires", "0");


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

            System.Web.HttpContext.Current.Session["updatedValue"] = "FALSE";

            //Utilities.clsDB.ApiUrl = System.Configuration.ConfigurationManager.AppSettings["Api_Url"];

            //Utilities.clsDB.DBName = System.Configuration.ConfigurationManager.AppSettings["DB_Name"];

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

                if (!Directory.Exists(path))
                {
                    return "ErrorNoPathFound";
                }
                else
                {
                    path += "\\Updated\\" + cRadiologist;

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string FianlDicomName = vWorkspaceId + "_" + vSubjectId + "_" + iNodeId + "_" + iModalityNo + "_" + cRadiologist + "_" + ImageTransmittalImgDtl_iImageTranNo + "_" + DicomName;
                    //string fileNameWitPath = path + "\\" + FianlDicomName + ".png";
                    string fileNameWitPath = path + "\\" + FianlDicomName + ".jpeg";
                    //string savefileNameWitPath = "/" + vWorkspaceId + "/" + vSubjectId + "/" + iNodeId + "/" + iModalityNo + "/Updated/" + cRadiologist + "/" + FianlDicomName + ".png";
                    string savefileNameWitPath = "/" + vWorkspaceId + "/" + vSubjectId + "/" + iNodeId + "/" + iModalityNo + "/Updated/" + cRadiologist + "/" + FianlDicomName + ".jpeg";

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
                                //dr["vFileType"] = ".png";
                                dr["vFileType"] = ".jpeg";
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
                                //dr["vFileType"] = ".png";
                                dr["vFileType"] = ".jpeg";
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
                    var response = await clsCommon.Call_API_POSTMethod("SetData/SaveImageTransmittalnew", _MIImageTransmittalnew);
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
                    //    string strURL = System.Configuration.ConfigurationManager.AppSettings["Api_Url"] +"SetData/SaveImageTransmittalnew";
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
                if (!Directory.Exists(path))
                {
                    return "Error No Path Found";
                }
                else
                {
                    path += "\\Updated";

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
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
                                //    string strURL = System.Configuration.ConfigurationManager.AppSettings["Api_Url"] +"SetData/SaveBiznetDicom";
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
                if (!Directory.Exists(path))
                {
                    return "Error No Path Found";
                }
                else
                {
                    path += "\\Updated";

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
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
                if (Directory.Exists(path))
                {
                    foreach (string file in Directory.GetFiles(path))
                    {
                        System.IO.File.Delete(file);
                    }
                    Directory.Delete(path);
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

                string path = System.Configuration.ConfigurationManager.AppSettings["DicomURL_1"];
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

                    if (JsonString == "1")
                    {
                        returnString = "success";
                    }
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

    }
}
