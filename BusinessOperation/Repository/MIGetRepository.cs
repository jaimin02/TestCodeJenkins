using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DatabaseOperation;
using DatabaseOperation.Contract;
using System.Data;


namespace BusinessOperation.Repository
{   
    public class MIGetRepository
    {
        MIGetContract _MIGetContract;

        public MIGetRepository()
        {
            _MIGetContract = new MIGetContract();
        }

        public DataTable GetUserprofile(String userName)
        {
            return _MIGetContract.GetUserProfile(userName);
        }
        
        public DataTable GetUserAuthenticationDetails(LoginDetails _LoginDetails)
        {
            return _MIGetContract.GetUserAuthenticationDetails(_LoginDetails);
        }
        public DataTable GetAdjUserAuthenticationDetails(LoginDetails _LoginDetails)
        {
            return _MIGetContract.GetAdjUserAuthenticationDetails(_LoginDetails);
        }

        

        public DataTable GetModality()
        {
            return _MIGetContract.GetModality();
        }

        public DataTable GetModalityAuditTrail(MIModalityDTO _Modality)
        {
            return _MIGetContract.GetModalityAuditTrail(_Modality);
        }

        public DataTable GetAnatomy()
        {
            return _MIGetContract.GetAnatomy();
        }

        public DataTable GetAnatomyAuditTrail(MIAnatomyDTO _Anatomy)
        {
            return _MIGetContract.GetAnatomyAuditTrail(_Anatomy);
        }

        public DataTable GetUserProfile(LoginDetails _LoginDetails)
        {
            return _MIGetContract.GetUserProfile(_LoginDetails);
        }

        public DataTable GetUserMenu(LoginDetails _LoginDetails)
        {
            return _MIGetContract.GetUserMenu(_LoginDetails);
        }

        public DataTable GetImgTransmittalHdr(MIImageTransmittalHdr _GetImgTransmittalHdrDTO)
        {
            return _MIGetContract.GetImgTransmittalHdr(_GetImgTransmittalHdrDTO);
        }

        public DataTable GetImgTransmittalDtl(MIImageTransmittalDtl _GetImgTransmittalDtlDTO)
        {
            return _MIGetContract.GetImgTransmittalDtl(_GetImgTransmittalDtlDTO);
        }

        public DataTable GetImageTransmittalImgDtl(MIImageTransmittalImgDtl _GetImageTransmittalImgDtlDTO)
        {
            return _MIGetContract.GetImageTransmittalImgDtl(_GetImageTransmittalImgDtlDTO);
        } 

        public DataTable CheckListTemplateDetail(MICheckListTemplateDTO _MICheckListTemplateDTO)
        {
            return _MIGetContract.CheckListTemplateDetail(_MICheckListTemplateDTO);
        }

        public DataTable SubjectStudyDetail(MISubjectStudyDTO _MISubjectStudyDTO)
        {
            return _MIGetContract.SubjectStudyDetail(_MISubjectStudyDTO);
        }
        public DataTable getBizNetSubjectStudyDetail(MIBizNETSaveImage _MIBizNETImageReview)
        {
            return _MIGetContract.getBizNetSubjectStudyDetail(_MIBizNETImageReview);
        }       

        public DataTable getSubjectImageStudyDetail(MIImageReviewDTO _MIImageReviewDTO)
        {
            return _MIGetContract.getSubjectImageStudyDetail(_MIImageReviewDTO);
        }

        public DataTable getSubjectSubSequentImageStudyDetail(MIImageReviewDTO _MIImageReviewDTO)
        {
            return _MIGetContract.getSubjectSubSequentImageStudyDetail(_MIImageReviewDTO);
        }

        public DataTable LesionDetails(LesionDTO _LesionDTO)
        {
            return _MIGetContract.MILesionDetails(_LesionDTO);
        }
        
        public DataTable GetRadioLogistData(string vWorkspaceId)
        {
            return _MIGetContract.GetRadioLogistData(vWorkspaceId);
        }

        public DataTable MILesionDetails(LesionDTO _LesionDTO)
        {
            return _MIGetContract.MILesionDetails(_LesionDTO);
        }

        public DataTable MILesionMARKDetails(LesionMarkDTO _LesionMarkDTO)
        {
            return _MIGetContract.MILesionMARKDetails(_LesionMarkDTO);
        }

        public DataTable getLesionDetails(LesionDetailsDATA_DTO _LesionDetailsDATA_DTO)
        {
            return _MIGetContract.getLesionDetails(_LesionDetailsDATA_DTO); 
        }

        public DataTable getLesionSavedDetails(Save_CRFHdrDtlSubDtlDTO _Save_CRFHdrDtlSubDtlDTO)
        {
            return _MIGetContract.getLesionSavedDetails(_Save_CRFHdrDtlSubDtlDTO);

        }

        public DataTable MyProjectCompletionList(GetMyProjectCompletionListDTO _GetMyProjectCompletionListDTO)
        {
            return _MIGetContract.MyProjectCompletionList(_GetMyProjectCompletionListDTO);
        }

        public DataTable MyStudyCompletionList(GetMyProjectCompletionListDTO _GetMyProjectCompletionListDTO)
        {
            return _MIGetContract.MyStudyCompletionList(_GetMyProjectCompletionListDTO);
        }
        public DataTable ProjectActivityDetails(MIDicomProjectDetailDTO _MIDicomProjectDetailDTO)
        {
            return _MIGetContract.ProjectActivityDetails(_MIDicomProjectDetailDTO); 
        }

        public String CRFDataEntryStatus(MICRFDataEntryStatus _MICRFDataEntryStatus)
        {
            return _MIGetContract.CRFDataEntryStatus(_MICRFDataEntryStatus);
        }

        public DataTable LesionStatistics(MILesionStatistics _MILesionStatistics)
        {
            return _MIGetContract.LesionStatistics(_MILesionStatistics);
        }

        public DataTable LesionStatisticsDetails(MILesionStatisticsDetails _MILesionStatisticsDetails)
        {
            return _MIGetContract.LesionStatisticsDetails(_MILesionStatisticsDetails);
        }

        public DataTable NLDetails(MI_NLDetails _MI_NLDetails)
        {
            return _MIGetContract.NLDetails(_MI_NLDetails);
        }

        public string MI_DataSaveStatus(MI_DataSaveStatus _MI_DataSaveStatus) 
        {
            return _MIGetContract.MI_DataSaveStatus(_MI_DataSaveStatus); 
        }

        public DataTable MIStatisticReport(MIStatisticReportDTO _MIStatisticReportDTO)
        {
            return _MIGetContract.MIStatisticReport(_MIStatisticReportDTO);
        }

        public DataTable MIStatisticReport1(MIStatisticReportOverAllDTO _MIStatisticReportOverAllDTO)
        {
            return _MIGetContract.MIStatisticReport1(_MIStatisticReportOverAllDTO);
        }

        public DataTable ProjectLockDetail(ProjectLockDetailDTO _ProjectLockDetailDTO)
        {
            return _MIGetContract.ProjectLockDetail(_ProjectLockDetailDTO);
        }

        public DataSet getSubjectImageStudyDetails(MIImageReviewDetailDTO _MIImageReviewDetailDTO)
        {
            return _MIGetContract.getSubjectImageStudyDetails(_MIImageReviewDetailDTO);
        }

        public DataTable ProjectFreezeDetail(ProjectFreezerDetailDTO _ProjectFreezerDetailDTO)
        {
            return _MIGetContract.ProjectFreezeDetail(_ProjectFreezerDetailDTO);
        }

        public DataTable SubjectDetailsForDISOFTWithDataEntryStatus(SubjectDTO _SubjectDTO)
        {
            return _MIGetContract.SubjectDetailsForDISOFTWithDataEntryStatus(_SubjectDTO);
        }

        public DataTable Datatable(DatatableDTO _DatatableDTO)
        {
            return _MIGetContract.Datatable(_DatatableDTO);
        }
        public DataTable ProjectDashboardDetail(DashboardDetailDTO _DashboardDetailDTO)
        {
            return _MIGetContract.ProjectDashboardDetail(_DashboardDetailDTO);
        }
        public DataTable GetPasswordPolicyData(ChangePasswordDTO _ChangePasswordDTO)
        {
            return _MIGetContract.GetPasswordPolicyData(_ChangePasswordDTO);
        }
        public DataTable GetPasswordHistory(ChangePasswordDTO _ChangePasswordDTO)
        {
            return _MIGetContract.GetPasswordHistory(_ChangePasswordDTO);
        }
        public DataTable GetUserMst(ChangePasswordDTO _ChangePasswordDTO)
        {
            return _MIGetContract.GetUserMst(_ChangePasswordDTO);
        }

        public int Insert_ChangePassword(ChangePasswordDTO _ChangePasswordDTO)
        {
            return _MIGetContract.Insert_ChangePassword(_ChangePasswordDTO);
        }

        public DataTable GetUser(OtpDTO _OtpDTO)
        {
            return _MIGetContract.GetUser(_OtpDTO);
        }

        public DataTable GetOtp(OtpDTO _OtpDTO)
        {
            return _MIGetContract.GetOtp(_OtpDTO);
        }
        public DataTable GetSmsDetails(OtpDTO _OtpDTO)
        {
            return _MIGetContract.GetSmsDetails(_OtpDTO);
        }

        public DataSet GetImgTransmittalDtlForQCReview(MI_DataSaveStatus _MI_DataSaveStatus)
        {
            return _MIGetContract.GetImgTransmittalDtlForQCReview(_MI_DataSaveStatus);
        }
        
        public DataTable GetEmailForTranstrion(MISubmitMIFinalLesionDTO _MISubmitMIFinalLesionData)
        {
            return _MIGetContract.GetEmailForTranstrion(_MISubmitMIFinalLesionData);
        }

        public DataTable GetUserType(string ApplicationId)
        {
            return _MIGetContract.GetUserType(ApplicationId);
        }
    }
}
