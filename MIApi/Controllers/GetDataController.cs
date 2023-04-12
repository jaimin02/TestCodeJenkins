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
using System.Threading.Tasks;
using System.IO;
using System.Web;
using System.Net.Http.Formatting;

namespace MIApi.Controllers
{
    public class GetDataController : ApiController
    {
        MIGetRepository _MIGetRepository;
        public GetDataController()
        {
            _MIGetRepository = new MIGetRepository();
        }

        public string getCurrentDate()
        {
            DateTime Current = DateTime.Now;
            return Current.ToString("dd-MMM-yyyy HH:mm:ss tt");

        }

        public HttpResponseMessage Userprofile(String userName)
        {
            var userProfile = _MIGetRepository.GetUserprofile(userName);
            return Request.CreateResponse(HttpStatusCode.OK, userProfile);

        }

        public JsonResult PostUserAuthenticationDetails(LoginDetails _LoginDetails)
        {
            var userLoginDetails = _MIGetRepository.GetUserAuthenticationDetails(_LoginDetails);
            JsonResult jsonResult = new JsonResult();
            jsonResult.Data = userLoginDetails;
            return jsonResult;
        }
        public JsonResult UserAuthenticationDetails(LoginDetails _LoginDetails)
        {
            var userLoginDetails = _MIGetRepository.GetAdjUserAuthenticationDetails(_LoginDetails);
            JsonResult jsonResult = new JsonResult();
            jsonResult.Data = userLoginDetails;
            return jsonResult;
        }

        public HttpResponseMessage GetModality()
        {
            try
            {
                var modalityData = _MIGetRepository.GetModality();
                return Request.CreateResponse(HttpStatusCode.OK, modalityData);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public HttpResponseMessage ModalityAuditTrail(MIModalityDTO _Modality)
        {
            var ModalityAuditTrail = _MIGetRepository.GetModalityAuditTrail(_Modality);
            return Request.CreateResponse(HttpStatusCode.OK, ModalityAuditTrail);
        }

        public HttpResponseMessage GetAnatomy()
        {
            var anatomyData = _MIGetRepository.GetAnatomy();
            return Request.CreateResponse(HttpStatusCode.OK, anatomyData);
        }

        public HttpResponseMessage AnatomyAuditTrail(MIAnatomyDTO _Anatomy)
        {
            var AnatomyAuditTrail = _MIGetRepository.GetAnatomyAuditTrail(_Anatomy);
            return Request.CreateResponse(HttpStatusCode.OK, AnatomyAuditTrail);
        }

        public HttpResponseMessage UserProfile(LoginDetails _LoginDetails)
        {
            var userProfile = _MIGetRepository.GetUserProfile(_LoginDetails);
            return Request.CreateResponse(HttpStatusCode.OK, userProfile);
        }


        public HttpResponseMessage UserMenu(LoginDetails _LoginDetails)
        {
            var userMenu = _MIGetRepository.GetUserMenu(_LoginDetails);
            return Request.CreateResponse(HttpStatusCode.OK, userMenu);
        }

        public HttpResponseMessage ImgTransmittalHdr(MIImageTransmittalHdr _GetImgTransmittalHdrDTO)
        {
            var ImgTransmittalHdr = _MIGetRepository.GetImgTransmittalHdr(_GetImgTransmittalHdrDTO);
            return Request.CreateResponse(HttpStatusCode.OK, ImgTransmittalHdr);
        }

        public HttpResponseMessage ImgTransmittalDtl(MIImageTransmittalDtl _GetImgTransmittalDtlDTO)
        {
            var ImgTransmittalHdr = _MIGetRepository.GetImgTransmittalDtl(_GetImgTransmittalDtlDTO);
            return Request.CreateResponse(HttpStatusCode.OK, ImgTransmittalHdr);
        }

        public HttpResponseMessage ImageTransmittalImgDtl(MIImageTransmittalImgDtl _GetImageTransmittalImgDtlDTO)
        {
            var ImgTransmittalHdr = _MIGetRepository.GetImageTransmittalImgDtl(_GetImageTransmittalImgDtlDTO);
            return Request.CreateResponse(HttpStatusCode.OK, ImgTransmittalHdr);
        }

        public HttpResponseMessage CheckListTemplateDetail(MICheckListTemplateDTO _MICheckListTemplateDTO)
        {
            var checkListTemplateDetail = _MIGetRepository.CheckListTemplateDetail(_MICheckListTemplateDTO);
            return Request.CreateResponse(HttpStatusCode.OK, checkListTemplateDetail);
        }

        public HttpResponseMessage SubjectStudyDetail(MISubjectStudyDTO _MISubjectStudyDTO)
        {
            var subjectStudyDetail = _MIGetRepository.SubjectStudyDetail(_MISubjectStudyDTO);
            return Request.CreateResponse(HttpStatusCode.OK, subjectStudyDetail);
        }

        public HttpResponseMessage BizNetSubjectStudyDetail(MIBizNETSaveImage _MIBizNETImageReview)
        {
            var subjectStudyDetail = _MIGetRepository.getBizNetSubjectStudyDetail(_MIBizNETImageReview);
            return Request.CreateResponse(HttpStatusCode.OK, subjectStudyDetail);
        }

        public HttpResponseMessage SubjectImageStudyDetail(MIImageReviewDTO _MIImageReviewDTO)
        {
            var subjectImageStudyDetail = _MIGetRepository.getSubjectImageStudyDetail(_MIImageReviewDTO);
            return Request.CreateResponse(HttpStatusCode.OK, subjectImageStudyDetail);
        }

        public HttpResponseMessage SubjectSubSequentImageStudyDetail(MIImageReviewDTO _MIImageReviewDTO)
        {
            var SubjectSubSequentImageStudyDetail = _MIGetRepository.getSubjectSubSequentImageStudyDetail(_MIImageReviewDTO);
            return Request.CreateResponse(HttpStatusCode.OK, SubjectSubSequentImageStudyDetail);
        }

        public HttpResponseMessage LesionDetails(LesionDTO _LesionDTO)
        {
            var LesionDetails = _MIGetRepository.LesionDetails(_LesionDTO);
            return Request.CreateResponse(HttpStatusCode.OK, LesionDetails);
        }

        public HttpResponseMessage GetRadioLogistData(string vWorkspaceId)
        {
            var GetRadiologistData = _MIGetRepository.GetRadioLogistData(vWorkspaceId);
            return Request.CreateResponse(HttpStatusCode.OK, GetRadiologistData);
        }

        public HttpResponseMessage MILesionDetails(LesionDTO _LesionDTO)
        {
            var MILesionDetails = _MIGetRepository.MILesionDetails(_LesionDTO);
            return Request.CreateResponse(HttpStatusCode.OK, MILesionDetails);
        }

        public HttpResponseMessage MILesionMARKDetails(LesionMarkDTO _LesionMarkDTO)
        {
            var MILesionMARKDetails = _MIGetRepository.MILesionMARKDetails(_LesionMarkDTO);
            return Request.CreateResponse(HttpStatusCode.OK, MILesionMARKDetails);
        }

        public HttpResponseMessage LesionDetailsDATA(LesionDetailsDATA_DTO _LesionDetailsDATA_DTO)
        {
            var getLesionDetails = _MIGetRepository.getLesionDetails(_LesionDetailsDATA_DTO);
            if (getLesionDetails.Rows.Count <= 0)
            {
                getLesionDetails = null;
            }
            return Request.CreateResponse(HttpStatusCode.OK, getLesionDetails);

        }

        public HttpResponseMessage LesionSavedDetailsDATA(Save_CRFHdrDtlSubDtlDTO _Save_CRFHdrDtlSubDtlDTO)
        {
            var getLesionDetails = _MIGetRepository.getLesionSavedDetails(_Save_CRFHdrDtlSubDtlDTO);
            if (getLesionDetails.Rows.Count <= 0)
            {
                getLesionDetails = null;
            }
            return Request.CreateResponse(HttpStatusCode.OK, getLesionDetails);

        }

        public HttpResponseMessage MyProjectCompletionList(GetMyProjectCompletionListDTO _GetMyProjectCompletionListDTO)
        {
            var MyProjectCompletionList = _MIGetRepository.MyProjectCompletionList(_GetMyProjectCompletionListDTO);
            return Request.CreateResponse(HttpStatusCode.OK, MyProjectCompletionList);
        }

        public HttpResponseMessage MyStudyCompletionList(GetMyProjectCompletionListDTO _GetMyProjectCompletionListDTO)
        {
            var MyProjectCompletionList = _MIGetRepository.MyStudyCompletionList(_GetMyProjectCompletionListDTO);
            return Request.CreateResponse(HttpStatusCode.OK, MyProjectCompletionList);
        }
        public HttpResponseMessage ProjectActivityDetails(MIDicomProjectDetailDTO _MIDicomProjectDetailDTO)
        {
            var ProjectActivityDetails = _MIGetRepository.ProjectActivityDetails(_MIDicomProjectDetailDTO);
            return Request.CreateResponse(HttpStatusCode.OK, ProjectActivityDetails);

        }

        public HttpResponseMessage CRFDataEntryStatus(MICRFDataEntryStatus _MICRFDataEntryStatus)
        {
            var CRFDataEntryStatus = _MIGetRepository.CRFDataEntryStatus(_MICRFDataEntryStatus);
            return Request.CreateResponse(HttpStatusCode.OK, CRFDataEntryStatus);

        }

        public HttpResponseMessage LesionStatistics(MILesionStatistics _MILesionStatistics)
        {
            var LesionStatistics = _MIGetRepository.LesionStatistics(_MILesionStatistics);
            return Request.CreateResponse(HttpStatusCode.OK, LesionStatistics);
        }

        public HttpResponseMessage LesionStatisticsDetails(MILesionStatisticsDetails _MILesionStatisticsDetails)
        {
            var LesionStatistics = _MIGetRepository.LesionStatisticsDetails(_MILesionStatisticsDetails);
            return Request.CreateResponse(HttpStatusCode.OK, LesionStatistics);
        }

        public HttpResponseMessage NLDetails(MI_NLDetails _MI_NLDetails)
        {
            var NLDetails = _MIGetRepository.NLDetails(_MI_NLDetails);
            return Request.CreateResponse(HttpStatusCode.OK, NLDetails);
        }

        public HttpResponseMessage MI_DataSaveStatus(MI_DataSaveStatus _MI_DataSaveStatus)
        {
            var MI_DataSaveStatus = _MIGetRepository.MI_DataSaveStatus(_MI_DataSaveStatus);
            return Request.CreateResponse(HttpStatusCode.OK, MI_DataSaveStatus);
        }

        public HttpResponseMessage MIStatisticReport(MIStatisticReportDTO _MIStatisticReportDTO)
        {
            var MIStatisticReport = _MIGetRepository.MIStatisticReport(_MIStatisticReportDTO);
            return Request.CreateResponse(HttpStatusCode.OK, MIStatisticReport);
        }

        public HttpResponseMessage MIStatisticReport1(MIStatisticReportOverAllDTO _MIStatisticReportOverAllDTO)
        {
            var MIStatisticReport1 = _MIGetRepository.MIStatisticReport1(_MIStatisticReportOverAllDTO);
            return Request.CreateResponse(HttpStatusCode.OK, MIStatisticReport1);
        }

        public HttpResponseMessage ProjectLockDetail(ProjectLockDetailDTO _ProjectLockDetailDTO)
        {
            var ProjectLockDetail = _MIGetRepository.ProjectLockDetail(_ProjectLockDetailDTO);
            return Request.CreateResponse(HttpStatusCode.OK, ProjectLockDetail);
        }

        public async Task<HttpResponseMessage> RemoveDirectory(DirectoryDTO _DirectoryDTO)
        {
            string strResponse = string.Empty;
            string strDirectoryName = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["DicomName"]);

            string strFilePath = strDirectoryName + "/" + Convert.ToString(_DirectoryDTO.vFilePath);
            string strFileName = Convert.ToString(_DirectoryDTO.vFileName);

            string strServerFilePath = HttpContext.Current.Server.MapPath(strFilePath);
            if (File.Exists(strServerFilePath))
            {
                try
                {
                    await Task.Run(() =>
                    {
                        File.Delete(strServerFilePath);
                    });

                    //Mark
                    string strFilePathMark = Convert.ToString(_DirectoryDTO.vFilePathMark);
                    if (strFilePathMark != "Null")
                    {
                        string strServerFilePathMark = HttpContext.Current.Server.MapPath(strDirectoryName + "/" + strFilePathMark);
                        if (File.Exists(strServerFilePathMark))
                        {
                            await Task.Run(() =>
                            {
                                File.Delete(strServerFilePathMark);
                            });
                        }
                    }
                    //Eligibility
                    string strFilePathEligibility = Convert.ToString(_DirectoryDTO.vFilePathEligibilityReview);
                    if (strFilePathEligibility != "Null")
                    {
                        string strServerFilePathEligibility = HttpContext.Current.Server.MapPath(strDirectoryName + "/" + strFilePathEligibility);
                        if (File.Exists(strServerFilePathEligibility))
                        {
                            await Task.Run(() =>
                            {
                                File.Delete(strServerFilePathEligibility);
                            });
                        }
                    }

                    strResponse = "1";
                }
                catch (Exception hx)
                {
                    hx.ToString();
                    strResponse = "-1";
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, strResponse);
        }

        public HttpResponseMessage SubjectImageStudyDetails(MIImageReviewDetailDTO _MIImageReviewDetailDTO)
        {
            var SubjectImageStudyDetails = _MIGetRepository.getSubjectImageStudyDetails(_MIImageReviewDetailDTO);
            return Request.CreateResponse(HttpStatusCode.OK, SubjectImageStudyDetails);
        }

        public HttpResponseMessage ProjectFreezeDetail(ProjectFreezerDetailDTO _ProjectFreezerDetailDTO)
        {
            var ProjectFreezeDetail = _MIGetRepository.ProjectFreezeDetail(_ProjectFreezerDetailDTO);
            return Request.CreateResponse(HttpStatusCode.OK, ProjectFreezeDetail);
        }

        public HttpResponseMessage SubjectDetailsForDISOFTWithDataEntryStatus(SubjectDTO _SubjectDTO)
        {
            var SubjectDetail = _MIGetRepository.SubjectDetailsForDISOFTWithDataEntryStatus(_SubjectDTO);
            return Request.CreateResponse(HttpStatusCode.OK, SubjectDetail);
        }

        public HttpResponseMessage Datatable(DatatableDTO _SubjectDTO)
        {
            var SubjectDetail = _MIGetRepository.Datatable(_SubjectDTO);
            return Request.CreateResponse(HttpStatusCode.OK, SubjectDetail);
        }
        public HttpResponseMessage ProjectDashboardDetail(DashboardDetailDTO _DashboardDetailDTO)
        {
            var ProjectDashboardDetail = _MIGetRepository.ProjectDashboardDetail(_DashboardDetailDTO);
            return Request.CreateResponse(HttpStatusCode.OK, ProjectDashboardDetail);

        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public HttpResponseMessage GetEmailForTranstrion(MISubmitMIFinalLesionDTO _MISubmitMIFinalLesionData)
        {
            var GetEmailForTranstrion = _MIGetRepository.GetEmailForTranstrion(_MISubmitMIFinalLesionData);
            return Request.CreateResponse(HttpStatusCode.OK, GetEmailForTranstrion);

        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public HttpResponseMessage GetPasswordPolicyData(ChangePasswordDTO _ChangePasswordDTO)
        {
            var GetPasswordPolicyData = _MIGetRepository.GetPasswordPolicyData(_ChangePasswordDTO);
            return Request.CreateResponse(HttpStatusCode.OK, GetPasswordPolicyData);
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public HttpResponseMessage GetPasswordHistory(ChangePasswordDTO _ChangePasswordDTO)
        {
            var GetPasswordHistoryData = _MIGetRepository.GetPasswordHistory(_ChangePasswordDTO);
            return Request.CreateResponse(HttpStatusCode.OK, GetPasswordHistoryData);
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public HttpResponseMessage GetUserMst(ChangePasswordDTO _ChangePasswordDTO)
        {
            var GetUserMstData = _MIGetRepository.GetUserMst(_ChangePasswordDTO);
            return Request.CreateResponse(HttpStatusCode.OK, GetUserMstData);
        }
        public HttpResponseMessage Insert_ChangePassword(ChangePasswordDTO _ChangePasswordDTO)
        {
            var ChangePasswordData = _MIGetRepository.Insert_ChangePassword(_ChangePasswordDTO);
            return Request.CreateResponse(HttpStatusCode.OK, ChangePasswordData);
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public HttpResponseMessage GetUser(OtpDTO _OtpDTO)
        {
            var GetUserMstData = _MIGetRepository.GetUser(_OtpDTO);
            return Request.CreateResponse(HttpStatusCode.OK, GetUserMstData);
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public HttpResponseMessage GetOtp(OtpDTO _OtpDTO)
        {
            var SubjectDetail = _MIGetRepository.GetOtp(_OtpDTO);
            return Request.CreateResponse(HttpStatusCode.OK, SubjectDetail);
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public HttpResponseMessage GetSmsDetails(OtpDTO _OtpDTO)
        {
            var GetUserMstData = _MIGetRepository.GetSmsDetails(_OtpDTO);
            return Request.CreateResponse(HttpStatusCode.OK, GetUserMstData);
        }

        public HttpResponseMessage MIGetImgTransmittalDtlForQCReview(MI_DataSaveStatus _MI_DataSaveStatus)
        {
            var ParameterList = _MIGetRepository.GetImgTransmittalDtlForQCReview(_MI_DataSaveStatus);
            return Request.CreateResponse(HttpStatusCode.OK, ParameterList);
        }

        public HttpResponseMessage GetUserType(string ApplicationId)
        {
            var UserType = _MIGetRepository.GetUserType(ApplicationId);
            return Request.CreateResponse(HttpStatusCode.OK, UserType);
        }

        public IEnumerable<GetFilesApiModel> GetFilesName(string iImgHdrId, string iImgDtlId, string iImageStatus, string cRadiologist, string iTranNo,
            string vParentActivityId, string iParentNodeId)
        {
            List<GetFilesApiModel> files = new List<GetFilesApiModel>();
            DataTable dt = new DataTable();
            var apiurl = System.Configuration.ConfigurationManager.AppSettings["ApiURL"];

            MIImageReviewDetailDTO _MIobj = new MIImageReviewDetailDTO();
            _MIobj.iImgTransmittalHdrId = iImgHdrId;
            _MIobj.iImgTransmittalDtlId = iImgDtlId;
            _MIobj.iImageStatus = iImageStatus;
            _MIobj.cRadiologist = cRadiologist;
            _MIobj.MODE = "1";
            _MIobj.ImageTransmittalImgDtl_iImageTranNo = iTranNo;
            _MIobj.vParentActivityId = vParentActivityId;
            _MIobj.iParentNodeId = iParentNodeId;

            var SubjectImageStudyDetails = _MIGetRepository.getSubjectImageStudyDetails(_MIobj);
            dt = SubjectImageStudyDetails.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                GetFilesApiModel obj = new GetFilesApiModel();
                obj.Name = dr["vFileName"].ToString();
                obj.path = apiurl + dr["vServerPath"].ToString();
                obj.ImgId = "dicomweb:" + apiurl.Replace("http:","").Trim() + dr["vServerPath"].ToString();
                files.Add(obj);
            }
            return files.ToArray();
        }
    }

}
public class ResultModel
{
    public string Status { get; set; }
    public string Message { get; set; }
    public string FileCount { get; set; }
}

public class GetFilesApiModel
{
    public string Name { get; set; }
    public string path { get; set; }
    public string ImgId { get; set; }
}