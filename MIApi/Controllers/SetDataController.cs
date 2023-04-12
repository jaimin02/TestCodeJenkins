using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DTO;
using BusinessOperation;
using BusinessOperation.Repository;
using System.Web.Mvc;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Dicom;

namespace MIApi.Controllers
{
    public class SetDataController : ApiController
    {
        MISetRepository _MISetRepository;
        public SetDataController()
        {
            _MISetRepository = new MISetRepository();
        }

        public DataTable AssingLoginFailureDetails([FromBody]LoginDetails _LoginDetails)
        {
            return _MISetRepository.AssingLoginFailureDetails(_LoginDetails);
        }

        public DataTable save_UserLoginDetails([FromBody]LoginDetails _LoginDetails)
        {
            return _MISetRepository.save_UserLoginDetails(_LoginDetails);

        }

        public string PostAddModality([FromBody]MIModalityDTO _Modality)
        {
            var response = _MISetRepository.saveModality(_Modality);
            if (response == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return response;            
        }

        public string PostAddAnatomy([FromBody]MIAnatomyDTO _Anatomy)
        {
            var response = _MISetRepository.saveAnatomy(_Anatomy);
            if (response == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return response;
        }

        public string SaveCheckListQuestionTemplateDetails([FromBody]MICheckListQuestionTemplateDTO _MICheckListQuestionTemplateDTO)
        {
            DataTable dtCheckListQuestionTemplate = new DataTable();           
            dtCheckListQuestionTemplate.Columns.Add("UserId");
            dtCheckListQuestionTemplate.Columns.Add("Question");
            foreach (var array in _MICheckListQuestionTemplateDTO.vQuestion)
            {
                DataRow dr = dtCheckListQuestionTemplate.NewRow();
                dr[0] = _MICheckListQuestionTemplateDTO.iUserId;
                dr[1] = array[3];
                dtCheckListQuestionTemplate.Rows.Add(dr);                 
            }
            var response = _MISetRepository.SaveCheckListQuestionTemplateDetails(dtCheckListQuestionTemplate);
            if (response == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return response;
        }

        public string SaveCheckListProjectTemplateDetails([FromBody]MICheckListProjectDTO _MICheckListProjectDTO)
        {
            DataTable dtCheckListProjectTemplate = new DataTable();
            dtCheckListProjectTemplate.Columns.Add("vWorkSpaceID");
            dtCheckListProjectTemplate.Columns.Add("nTemplateHdrNo");
            dtCheckListProjectTemplate.Columns.Add("nTemplateDtlNo");
            dtCheckListProjectTemplate.Columns.Add("iUserId");
            foreach (var array in _MICheckListProjectDTO.nTemplateDtlNo)
            {
                DataRow dr = dtCheckListProjectTemplate.NewRow();
                dr[0] = _MICheckListProjectDTO.vWorkSpaceID;
                dr[1] = _MICheckListProjectDTO.nTemplateHdrNo;
                dr[2] = array[1];
                dr[3] = _MICheckListProjectDTO.iUserId;
                dtCheckListProjectTemplate.Rows.Add(dr);
            }
            var response = _MISetRepository.SaveCheckListProjectTemplateDetails(dtCheckListProjectTemplate);
            if (response == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return response;
        }

        public string SaveImageTransmittalHdr(MIImageTransmittalHdr _MIImageTransmittalHdr)
        {
            var response = _MISetRepository.SaveImageTransmittalHdr(_MIImageTransmittalHdr);
            if (response == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return response;
           
        }

        public string SaveImageTransmittalDtl(MIImageTransmittalDtl _MIImageTransmittalDtl)
        {
            var response = _MISetRepository.SaveImageTransmittalDtl(_MIImageTransmittalDtl);
            if (response == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return response;

        }

        public string SaveImageTransmittalnew(MIImageTransmittalNew _MIImageTransmittalnew)
        {
            var response = _MISetRepository.SaveImageTransmittalnew(_MIImageTransmittalnew);
            if (response == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return response;
        }

        public DataTable SaveBiznetDicom(MIBizNETSaveImageDTO _MIBizNETSaveImageDTO)
        {
            var response = _MISetRepository.SaveBiznetDicom(_MIBizNETSaveImageDTO);
            if (response == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return response;
        }       

        public DataTable saveLessionDetails(LesionDetailDTO _LesionDetailDTO)
        {
            var response = _MISetRepository.saveLessionDetails(_LesionDetailDTO);
            if (response == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return response;
        }

        public DataTable saveMILessionDetails(LesionDetailDTO _LesionDetailDTO)
        {
            var response = _MISetRepository.saveLessionDetails(_LesionDetailDTO);
            if (response == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return response;
        }

        public DataTable SaveMIFinalLession(MIFinalLesionDetailDTO _MIFinalLesionDetailDTO)
        {
            var response = _MISetRepository.SaveMIFinalLession(_MIFinalLesionDetailDTO);
            if (response == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return response;
        }       

        //public DataTable saveLessionCRFDetails(LesionCRFDTO _LesionCRFDTO)
        //{
        //    var response = _MISetRepository.saveLessionCRFDetails(_LesionCRFDTO);
        //    if(response == null)
        //    {
        //        throw new HttpResponseException(HttpStatusCode.NotFound);
        //    }
        //    return response;
        //}

        public string Save_CRFHdrDtlSubDtl(Save_CRFHdrDtlSubDtlDTO _Save_CRFHdrDtlSubDtlDTO)
        {
            var response = _MISetRepository.Save_CRFHdrDtlSubDtl(_Save_CRFHdrDtlSubDtlDTO);
            if (response == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return response;
        }

        public string SaveImageTransmittal(MIImageTransmittal _MIImageTransmittal)
        {
            var response = _MISetRepository.SaveImageTransmittal(_MIImageTransmittal);
            if (response == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return response;
        }

        //For Save Image Detail For MI and Biznet
        public string SubmitMIFinalLesion(MIFinalLesionDataDTO _MIFinalLesionDataDTO)
        {
            var response = _MISetRepository.SubmitMIFinalLesion(_MIFinalLesionDataDTO);
            if (response == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return response; 
        }

        //For Skip Dicom Detail For MI and Biznet
        public string SkipMIFinalLesion(MIFinalSkipLesionDataDTO _MIFinalSkipLesionDataDTO)
        {
            var response = _MISetRepository.SkipMIFinalLesion(_MIFinalSkipLesionDataDTO);
            if (response == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return response;
        }


        public string test()
        {
            return "success";
        }

        public string SubmitMIFinalLesionData(MIFinalLesionDetailsDataDTO _MIFinalLesionDetailsDataDTO)
        {
            var response = _MISetRepository.SubmitMIFinalLesionData(_MIFinalLesionDetailsDataDTO);
            if (response == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return response;

        }

        public string SkipMIFinalLesionData(MIFinalSkipLesionDetailDTO _MIFinalSkipLesionDetailDTO)
        {
            var response = _MISetRepository.SkipMIFinalLesionData(_MIFinalSkipLesionDetailDTO);
            if (response == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return response;
        }

        public string InsertUpdate_Certificate(CertificateDTO _Certificate)
        {
            var response = _MISetRepository.InsertUpdate_Certificate(_Certificate);
            if (response == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return response;

        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public string ImageTransmittalFileUpload([FromBody] ImageTransmittalImgDtl _ImageTransmittalImgDtl)
        {
            try
            {
                string path = Path.GetDirectoryName(_ImageTransmittalImgDtl.vServerPath);
                path = HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["DicomName"] + path);
                string filename = _ImageTransmittalImgDtl.vFileName;
                string fileExtension = Path.GetExtension(filename);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var files = Path.Combine(path, filename);
                Byte[] bytes = Convert.FromBase64String(_ImageTransmittalImgDtl.Content);
                if (!File.Exists(files))
                {
                    using (var stream = new FileStream(files, FileMode.CreateNew))
                    {
                        stream.Write(bytes, 0, bytes.Length);
                    }
                }
                else
                {
                    using (var stream = new FileStream(files, FileMode.Append))
                    {
                        stream.Write(bytes, 0, bytes.Length);
                    }
                }
                return "Ceritficated Updated";
            }

            catch (Exception e)
            {
                return "error" + e.Message;
            }
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public async Task<HttpResponseMessage> DownLoadFiles(string iImgTransmittalDtlId)
        {
            string strResponse = string.Empty;
            string subpath = "\\";
            string PathName = "";
            string strDestPathName = string.Empty;
            int IsDestinationUserPermission = 0;
            ResultModel Status = new ResultModel();
            UserImpersonation oDestionalUserImpersonation = new UserImpersonation();
            DataTable Sql_Dt = new DataTable();
            Sql_Dt = _MISetRepository.GetTransmittalDtl(iImgTransmittalDtlId);
            DirectoryInfo dir;
            string ZipFileName = "Download.zip";

            bool IsImpersonate = false;
            try
            {
                string dicomPath = System.Configuration.ConfigurationManager.AppSettings["DicomName"];
                string DownloadPath = Path.GetDirectoryName(Sql_Dt.Rows[0]["vServerPath"].ToString());
                string extractPath = HttpContext.Current.Server.MapPath(dicomPath + DownloadPath + subpath);
                //string DownloadPath =  extractPath + PathName;
                string strDestFilePath = extractPath;
                string ImgTransmittalDtlId = Convert.ToString(iImgTransmittalDtlId);

                if (ImgTransmittalDtlId != "")
                {
                    if (Directory.Exists(strDestFilePath))
                    {
                        //Transfer File                                
                        string[] filess = System.IO.Directory.GetFiles(strDestFilePath, "*.*", SearchOption.AllDirectories);
                        if (filess.Count() > 0)
                        {
                            if (Directory.Exists(strDestFilePath))
                            {
                                try
                                {
                                    using (Ionic.Zip.ZipFile compress = new Ionic.Zip.ZipFile())
                                    {
                                        string temp = "";
                                        string tempPath;
                                        string[] fileDir;
                                        dir = new DirectoryInfo(strDestFilePath);
                                        var ax = Directory.GetFiles(strDestFilePath, "*").Except(Directory.GetFiles(strDestFilePath, "*.zip"));
                                        compress.AddFiles(ax, ZipFileName);
                                        tempPath = extractPath.Replace("\\", "/");
                                        fileDir = tempPath.Split('/');
                                        for(int i=0 ; i< fileDir.Length-2 ; i++)
                                        {
                                            temp = temp + "\\" + fileDir[i];
                                        }
                                        temp = temp + "\\";
                                        string finalPath = temp.Substring(1);
                                        //if (!File.Exists(finalPath + (ZipFileName)))
                                        //{
                                        compress.Save(finalPath + (ZipFileName));
                                        //}
                                    }
                                    string ZipPath = "";
                                    string[] substring;
                                    substring = Path.GetDirectoryName(Sql_Dt.Rows[0]["vServerPath"].ToString()).Replace("\\", "/").Split('/');
                                    for (int j=0;j<substring.Length-1;j++)
                                    {
                                        ZipPath = ZipPath + "/" + substring[j];
                                    }
                                    extractPath = HttpContext.Current.Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.SafeUnescaped) +
                                                    HttpContext.Current.Request.ApplicationPath.Replace("\\", "/") +
                                                    dicomPath.Substring(1) + "/" +
                                                    ZipPath +
                                                    "/" + ZipFileName;
                                    // Other Zip Code
                                    //using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
                                    //{
                                    //    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                                    //    zip.AddDirectoryByName("Files");
                                    //    for (int i = 0; i < filess.Length; i++)
                                    //    {
                                    //        zip.AddFile(filess[i], "Files");
                                    //    }


                                    //    //string[] Filenames = Directory.GetFiles(strDestFilePath);
                                    //    //using (Ionic.Zip.ZipFile zipFile = new Ionic.Zip.ZipFile())
                                    //    //{
                                    //    //    zipFile.AddFiles(Filenames, "Project");//Zip file inside filename  
                                    //    //    zipFile.Save(strDestFilePath);//location and name for creating zip file  

                                    //    //}
                                    //    System.Web.HttpContext.Current.Response.Clear();
                                    //    System.Web.HttpContext.Current.Response.BufferOutput = false;
                                    //    string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                                    //    System.Web.HttpContext.Current.Response.ContentType = "application/zip";
                                    //    System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                                    //    zip.Save(System.Web.HttpContext.Current.Response.OutputStream);
                                    //    System.Web.HttpContext.Current.Response.ContentType = "application/zip";
                                    //    System.Web.HttpContext.Current.Response.Flush();
                                    //    System.Web.HttpContext.Current.Response.End();
                                    //}
                                    // End
                                    Status.Status = "1";
                                    Status.Message = extractPath;

                                }
                                catch (Exception ex)
                                {
                                    Status.Status = "0";
                                    Status.Message = ex.Message;
                                }
                            }
                            else
                            {
                                Status.Status = "0";
                                Status.Message = "Destination Path Directory Not Found";
                            }
                        }
                        else
                        {
                            Status.Status = "0";
                            Status.Message = "File Not Found at the Source Directory";
                        }
                    }
                    else
                    {
                        Status.Status = "0";
                        Status.Message = "Source Path Directory Not Found";
                    }

                }
                else
                {
                    Status.Status = "0";
                    Status.Message = "Please Enter Sourch Path";
                }

            }
            catch (Exception hx)
            {
                Status.Status = "0";
                Status.Message = hx.Message;
            }
            finally
            {
                if (IsDestinationUserPermission == 1 && IsImpersonate == true)
                {
                    oDestionalUserImpersonation.undoimpersonateUser();
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, Status);
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        //public string DicomTagChange(string fileName, string fromPath, string subjectValue)
        public string DicomTagChange(DicomTagChange objDicomTagChange)
        {
            try
            {
                var dirPath = objDicomTagChange.fromPath;
                var filePath = Path.Combine(dirPath, objDicomTagChange.fileName);

                DicomFile openedDicom = DicomFile.Open(filePath, FileReadOption.ReadAll);

                var edit = DicomTag.Parse("0010,0010");
                openedDicom.Dataset.AddOrUpdate(edit, objDicomTagChange.subjectValue);

                openedDicom.Dataset.AddOrUpdate(DicomTag.Parse("0008,0050"), "");
                openedDicom.Dataset.AddOrUpdate(DicomTag.Parse("0008,0081"), "");
                openedDicom.Dataset.AddOrUpdate(DicomTag.Parse("0008,0090"), "");
                openedDicom.Dataset.AddOrUpdate(DicomTag.Parse("0008,1050"), "");
                openedDicom.Dataset.AddOrUpdate(DicomTag.Parse("0010,1001"), "");
                openedDicom.Dataset.AddOrUpdate(DicomTag.Parse("0010,1040"), "");
                openedDicom.Save(filePath);
                return "Success";
            }
            catch (Exception ex)
            {
                return  ex.Message;
                throw ex;
            }
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        //public string DicomTagChange(string fileName, string fromPath, string subjectValue)
        public string CertDicomTagChange(CertDicomTagChange objDicomTagChange)
        {
            try
            {
                var dirPath = objDicomTagChange.fromPath;
                var filePath = Path.Combine(dirPath, objDicomTagChange.fileName);

                DicomFile openedDicom = DicomFile.Open(filePath, FileReadOption.ReadAll);

                var edit = DicomTag.Parse("0010,0010");
                openedDicom.Dataset.AddOrUpdate(edit, objDicomTagChange.fileName);

                openedDicom.Dataset.AddOrUpdate(DicomTag.Parse("0008,0050"), "");
                openedDicom.Dataset.AddOrUpdate(DicomTag.Parse("0008,0081"), "");
                openedDicom.Dataset.AddOrUpdate(DicomTag.Parse("0008,0090"), "");
                openedDicom.Dataset.AddOrUpdate(DicomTag.Parse("0008,1050"), "");
                openedDicom.Dataset.AddOrUpdate(DicomTag.Parse("0010,1001"), "");
                openedDicom.Dataset.AddOrUpdate(DicomTag.Parse("0010,1040"), "");
                openedDicom.Save(filePath);
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw ex;
            }
        }

        public string Insert_Otp(OtpDTO _OtpDTO)
        {
            var response = _MISetRepository.Insert_Otp(_OtpDTO);
            if (response == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return response;
        }

        public string Insert_Exmsg(ExMsgInfo objmodel)
        {
            var response = _MISetRepository.Insert_Exmsg(objmodel);
            if (response == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return response;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public string CertificateFileUpload([FromBody] CertificateMasterImgDtl _CertificateMasterImgDtl)
        {
            try
            {
                string path = Path.GetDirectoryName(_CertificateMasterImgDtl.vServerPath);
                path = HttpContext.Current.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["DicomName"] + path);
                string filename = _CertificateMasterImgDtl.vFileName;
                string fileExtension = Path.GetExtension(filename);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var files = Path.Combine(path, filename);
                Byte[] bytes = Convert.FromBase64String(_CertificateMasterImgDtl.Content);
                if (!File.Exists(files))
                {
                    using (var stream = new FileStream(files, FileMode.CreateNew))
                    {
                        stream.Write(bytes, 0, bytes.Length);
                    }
                }
                else
                {
                    using (var stream = new FileStream(files, FileMode.Append))
                    {
                        stream.Write(bytes, 0, bytes.Length);
                    }
                }
                return "Ceritficated Updated";
            }

            catch (Exception e)
            {
                return "error" + e.Message;
            }
        }

    }
}
