using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DatabaseOperation;
using DatabaseOperation.Contract;
using System.Data;
using System.Web;

namespace BusinessOperation.Repository
{   

    public class MISetRepository
    {
         MISetContract _MISetContract;

        public MISetRepository()
        {
            _MISetContract = new MISetContract();
        }
        public DataTable AssingLoginFailureDetails(LoginDetails _LoginDetails)
        {
            return _MISetContract.AssingLoginFailureDetails(_LoginDetails);
        }

        public DataTable save_UserLoginDetails(LoginDetails _LoginDetails)
        {
            return _MISetContract.save_UserLoginDetails(_LoginDetails);
        }
        public string saveModality(MIModalityDTO _Modality)
        {
            var response = _MISetContract.saveModality(_Modality);
            return response;
        }
        public string saveAnatomy(MIAnatomyDTO _Anatomy)
        {
            var response = _MISetContract.saveAnatomy(_Anatomy);
            return response;
        }
        public string SaveCheckListQuestionTemplateDetails(DataTable dtCheckListQuestionTemplate)
        {
            var response = _MISetContract.SaveCheckListQuestionTemplateDetails(dtCheckListQuestionTemplate);
            return response;
        }
        public string SaveCheckListProjectTemplateDetails(DataTable dtCheckListProjectTemplate)
        {
            var response = _MISetContract.SaveCheckListProjectTemplateDetails(dtCheckListProjectTemplate);
            return response;
        }
        public string SaveImageTransmittalHdr(MIImageTransmittalHdr _MIImageTransmittalHdr)
        {
            var response = _MISetContract.SaveImageTransmittalHdr(_MIImageTransmittalHdr);
            return response;
        }

        public string SaveImageTransmittalDtl(MIImageTransmittalDtl _MIImageTransmittalDtl)
        {
            var response = _MISetContract.SaveImageTransmittalDtl(_MIImageTransmittalDtl);
            return response;
        }

        public string SaveImageTransmittalnew(MIImageTransmittalNew _MIImageTransmittalnew)
        {
            var response = _MISetContract.SaveImageTransmittalnew(_MIImageTransmittalnew);
            return response;
        }

        public DataTable SaveBiznetDicom(MIBizNETSaveImageDTO _MIBizNETSaveImageDTO)
        {
            var response = _MISetContract.SaveBiznetDicom(_MIBizNETSaveImageDTO);
            return response;
        }

        public DataTable saveLessionDetails(LesionDetailDTO _LesionDetailDTO)
        {
            return _MISetContract.saveMILessionDetails(_LesionDetailDTO);           
        }

        public DataTable saveMILessionDetails(LesionDetailDTO _LesionDetailDTO)
        {
            return _MISetContract.saveMILessionDetails(_LesionDetailDTO);
        }

        public DataTable SaveMIFinalLession(MIFinalLesionDetailDTO _MIFinalLesionDetailDTO)
        {
            return _MISetContract.SaveMIFinalLession(_MIFinalLesionDetailDTO);           
        }       

        //public DataTable saveLessionCRFDetails(LesionCRFDTO _LesionCRFDTO)
        //{
        //    return _MISetContract.saveLessionCRFDetails(_LesionCRFDTO);
        //}

        public string Save_CRFHdrDtlSubDtl(Save_CRFHdrDtlSubDtlDTO _Save_CRFHdrDtlSubDtlDTO)
        {
            return _MISetContract.Save_CRFHdrDtlSubDtl(_Save_CRFHdrDtlSubDtlDTO);  
        }

        public string SaveImageTransmittal(MIImageTransmittal _SaveImageTransmittal)
        {
            var response = _MISetContract.SaveImageTransmittal(_SaveImageTransmittal);
            return response;
        }

        public string SubmitMIFinalLesion(MIFinalLesionDataDTO _MIFinalLesionDataDTO)
        {
            var response = _MISetContract.SubmitMIFinalLesion(_MIFinalLesionDataDTO);
            return response;
 
        }

        public string SkipMIFinalLesion(MIFinalSkipLesionDataDTO _MIFinalSkipLesionDataDTO)
        {
            var response = _MISetContract.SkipMIFinalLesion(_MIFinalSkipLesionDataDTO);
            return response;

        }

        public string SubmitMIFinalLesionData(MIFinalLesionDetailsDataDTO _MIFinalLesionDetailsDataDTO)
        {
            var response = _MISetContract.SubmitMIFinalLesionData(_MIFinalLesionDetailsDataDTO);
            return response;

        }

        public string SkipMIFinalLesionData(MIFinalSkipLesionDetailDTO _MIFinalSkipLesionDetailDTO)
        {
            var response = _MISetContract.SkipMIFinalLesionData(_MIFinalSkipLesionDetailDTO);
            return response;

        }

        public DataTable GetTransmittalDtl(string iImgTransmittalDtlId)
        {
            return _MISetContract.GetTransmittalDtl(iImgTransmittalDtlId);
        }

        public string Insert_Otp(OtpDTO _OtpDTO)
        {
            return _MISetContract.Insert_Otp(_OtpDTO);
        }

        public string Insert_Exmsg(ExMsgInfo objmodel)
        {
            return _MISetContract.Insert_ExMsgInfo(objmodel);
        }

        public string InsertUpdate_Certificate(CertificateDTO _SaveCertificate)
        {
            var response = _MISetContract.InsertUpdate_Certificate(_SaveCertificate);
            return response;
        }

    }

}
