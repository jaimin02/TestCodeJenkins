using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data;
using DTO;
using Utility;
using DatabaseOperation;
using DatabaseOperation.DBContract;

namespace DatabaseOperation.Contract
{
    public class MISetContract
    {
        SqlHelper _SqlHelper;
        DataSet Sql_Ds;
        DataTable Sql_Dt;
        Hashtable ht;
        DataTable dt_CRFSubDtl;

        public MISetContract()
        {
            _SqlHelper = new SqlHelper();
            Sql_Ds = new DataSet();
            Sql_Dt = new DataTable();
            ht = new Hashtable();
            dt_CRFSubDtl = new DataTable();
        }

        public DataTable AssingLoginFailureDetails(LoginDetails _LoginDetails)
        {
            try
            {
                //ht = new Hashtable();
                ht.Clear();
                ht.Add(Utilities.clsParameters.vUserName, String.IsNullOrEmpty(_LoginDetails.vUserName) ? "" : _LoginDetails.vUserName);
                ht.Add(Utilities.clsParameters.vUserTypeCode, String.IsNullOrEmpty(_LoginDetails.vUserTypeCode) ? "" : _LoginDetails.vUserTypeCode);
                ht.Add(Utilities.clsParameters.vIPAddress, String.IsNullOrEmpty(_LoginDetails.vIPAddress) ? "" : _LoginDetails.vIPAddress);

                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Proc_LoginFailureCheck, ht);
                return Sql_Ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable save_UserLoginDetails(LoginDetails objLogin)
        {
            try
            {
                //ht = new Hashtable();
                ht.Clear();
                ht.Add(Utilities.clsParameters.iUserId, String.IsNullOrEmpty(objLogin.iUserId) ? "0" : objLogin.iUserId);
                ht.Add(Utilities.clsParameters.vIPAddress, String.IsNullOrEmpty(objLogin.vIPAddress) ? "" : objLogin.vIPAddress);
                ht.Add(Utilities.clsParameters.vUTCHourDiff, String.IsNullOrEmpty(objLogin.vUTCHourDiff) ? "" : objLogin.vUTCHourDiff);
                ht.Add(Utilities.clsParameters.vUserAgent, String.IsNullOrEmpty(objLogin.vUserAgent) ? "" : objLogin.vUserAgent);
                ht.Add(Utilities.clsParameters.DataopMode, String.IsNullOrEmpty(objLogin.DataopMode) ? "0" : objLogin.DataopMode);

                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Proc_UserLoginDetails, ht);
                return Sql_Ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string saveModality(MIModalityDTO _Modality)
        {
            //DataSet Sql_Ds = new DataSet();
            //Hashtable ht;
            try
            {
                //ht = new Hashtable();
                ht.Clear();
                ht.Add(Utilities.clsParameters.vModalityDesc, _Modality.vModalityDesc);
                //ht.Add(Utilities.clsParameters.vRemarks, _Modality.vRemarks);
                ht.Add(Utilities.clsParameters.vRemarks, String.IsNullOrEmpty(_Modality.vRemarks) ? "" : _Modality.vRemarks);
                ht.Add(Utilities.clsParameters.iModifyBy, _Modality.iModifyBy);
                ht.Add(Utilities.clsParameters.dModifyOn, DateTime.Now.ToString("h:mm:ss tt"));

                if (_Modality.cStatusIndi == "N")
                {
                    ht.Add(Utilities.clsParameters.cStatusIndi, _Modality.cStatusIndi);
                    ht.Add(Utilities.clsParameters.nModalityNo, 1);
                    ht.Add(Utilities.clsParameters.DataopMode, Utilities.DataObjOpenSaveModeEnum.DataObjOpenMode_Add);
                }
                if (_Modality.cStatusIndi == "E")
                {
                    ht.Add(Utilities.clsParameters.nModalityNo, _Modality.nModalityNo);
                    ht.Add(Utilities.clsParameters.cStatusIndi, _Modality.cStatusIndi);
                    ht.Add(Utilities.clsParameters.DataopMode, Utilities.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit);
                }
                if (_Modality.cStatusIndi == "D")
                {
                    ht.Add(Utilities.clsParameters.nModalityNo, _Modality.nModalityNo);
                    ht.Add(Utilities.clsParameters.cStatusIndi, _Modality.cStatusIndi);
                    ht.Add(Utilities.clsParameters.DataopMode, Utilities.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete);
                }
                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Insert_ModalityMst, ht);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return _Modality.cStatusIndi;
        }

        public string saveAnatomy(MIAnatomyDTO _Anatomy)
        {
            //DataSet Sql_Ds = new DataSet();
            //Hashtable ht;
            try
            {
                //ht = new Hashtable();
                ht.Clear();
                ht.Add(Utilities.clsParameters.vAnatomyDesc, _Anatomy.vAnatomyDesc);
                ht.Add(Utilities.clsParameters.vRemarks, String.IsNullOrEmpty(_Anatomy.vRemarks) ? "" : _Anatomy.vRemarks);
                ht.Add(Utilities.clsParameters.iModifyBy, _Anatomy.iModifyBy);
                ht.Add(Utilities.clsParameters.dModifyOn, DateTime.Now.ToString("h:mm:ss tt"));

                if (_Anatomy.cStatusIndi == "N")
                {
                    ht.Add(Utilities.clsParameters.cStatusIndi, _Anatomy.cStatusIndi);
                    ht.Add(Utilities.clsParameters.nAnatomyNo, 1);
                    ht.Add(Utilities.clsParameters.DataopMode, Utilities.DataObjOpenSaveModeEnum.DataObjOpenMode_Add);
                }
                if (_Anatomy.cStatusIndi == "E")
                {
                    ht.Add(Utilities.clsParameters.nAnatomyNo, _Anatomy.nAnatomyNo);
                    ht.Add(Utilities.clsParameters.cStatusIndi, _Anatomy.cStatusIndi);
                    ht.Add(Utilities.clsParameters.DataopMode, Utilities.DataObjOpenSaveModeEnum.DataObjOpenMode_Edit);
                }
                if (_Anatomy.cStatusIndi == "D")
                {
                    ht.Add(Utilities.clsParameters.nAnatomyNo, _Anatomy.nAnatomyNo);
                    ht.Add(Utilities.clsParameters.cStatusIndi, _Anatomy.cStatusIndi);
                    ht.Add(Utilities.clsParameters.DataopMode, Utilities.DataObjOpenSaveModeEnum.DataObjOpenMode_Delete);
                }
                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Insert_AnatomyMst, ht);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return _Anatomy.cStatusIndi;
        }

        public string SaveCheckListQuestionTemplateDetails(DataTable dtCheckListQuestionTemplate)
        {
            try
            {
                ht.Clear();
                ht.Add(Utilities.clsParameters.iModifyBy, dtCheckListQuestionTemplate.Rows[0][0]);
                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Insert_CheckListTemplateMstHdr, ht);

                for (int i = 0; i < dtCheckListQuestionTemplate.Rows.Count; i++)
                {
                    ht.Clear();
                    ht.Add(Utilities.clsParameters.iModifyBy, dtCheckListQuestionTemplate.Rows[i][0]);
                    ht.Add(Utilities.clsParameters.vQuestion, dtCheckListQuestionTemplate.Rows[i][1]);
                    Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Insert_CheckListTemplateMstDtl, ht);
                }
                return "success";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public string SaveCheckListProjectTemplateDetails(DataTable dtCheckListProjectTemplate)
        {
            try
            {
                ht.Clear();
                ht.Add(Utilities.clsParameters.vWorkspaceId, dtCheckListProjectTemplate.Rows[0][0]);
                ht.Add(Utilities.clsParameters.nTemplateHdrNo, dtCheckListProjectTemplate.Rows[0][1]);
                ht.Add(Utilities.clsParameters.iModifyBy, dtCheckListProjectTemplate.Rows[0][3]);
                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Insert_CheckListProjectMstHdr, ht);

                for (int i = 0; i < dtCheckListProjectTemplate.Rows.Count; i++)
                {
                    ht.Clear();
                    ht.Add(Utilities.clsParameters.nTemplateDtlNo, dtCheckListProjectTemplate.Rows[i][2]);
                    ht.Add(Utilities.clsParameters.iModifyBy, dtCheckListProjectTemplate.Rows[i][3]);
                    Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Insert_CheckListProjectMstDtl, ht);
                }
                return "success";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        public string SaveImageTransmittalHdr(MIImageTransmittalHdr _MIImageTransmittalHdr)
        {
            try
            {
                ht.Clear();
                ht.Add(Utilities.clsParameters.iImgTransmittalHdrId, String.IsNullOrEmpty(_MIImageTransmittalHdr.iImgTransmittalHdrId) ? "" : _MIImageTransmittalHdr.iImgTransmittalHdrId);
                ht.Add(Utilities.clsParameters.vWorkspaceId, String.IsNullOrEmpty(_MIImageTransmittalHdr.vWorkspaceId) ? "" : _MIImageTransmittalHdr.vWorkspaceId);
                ht.Add(Utilities.clsParameters.vProjectNo, String.IsNullOrEmpty(_MIImageTransmittalHdr.vProjectNo) ? "" : _MIImageTransmittalHdr.vProjectNo);
                ht.Add(Utilities.clsParameters.vSubjectId, String.IsNullOrEmpty(_MIImageTransmittalHdr.vSubjectId) ? "" : _MIImageTransmittalHdr.vSubjectId);
                ht.Add(Utilities.clsParameters.vRandomizationNo, String.IsNullOrEmpty(_MIImageTransmittalHdr.vRandomizationNo) ? "" : _MIImageTransmittalHdr.vRandomizationNo);
                ht.Add(Utilities.clsParameters.iNodeId, String.IsNullOrEmpty(_MIImageTransmittalHdr.iNodeId) ? "" : _MIImageTransmittalHdr.iNodeId);
                ht.Add(Utilities.clsParameters.cDeviation, String.IsNullOrEmpty(_MIImageTransmittalHdr.cDeviation) ? "" : _MIImageTransmittalHdr.cDeviation);
                ht.Add(Utilities.clsParameters.nvDeviationReason, String.IsNullOrEmpty(_MIImageTransmittalHdr.nvDeviationReason) ? "" : _MIImageTransmittalHdr.nvDeviationReason);
                ht.Add(Utilities.clsParameters.nvInstructions, String.IsNullOrEmpty(_MIImageTransmittalHdr.nvInstructions) ? "" : _MIImageTransmittalHdr.nvInstructions);
                ht.Add(Utilities.clsParameters.vRemark, String.IsNullOrEmpty(_MIImageTransmittalHdr.vRemark) ? "" : _MIImageTransmittalHdr.vRemark);
                ht.Add(Utilities.clsParameters.iModifyBy, String.IsNullOrEmpty(_MIImageTransmittalHdr.iModifyBy) ? "" : _MIImageTransmittalHdr.iModifyBy);
                ht.Add(Utilities.clsParameters.cStatusIndi, String.IsNullOrEmpty(_MIImageTransmittalHdr.cStatusIndi) ? "" : _MIImageTransmittalHdr.cStatusIndi);
                ht.Add(Utilities.clsParameters.DataopMode, String.IsNullOrEmpty(_MIImageTransmittalHdr.DATAOPMODE) ? "" : _MIImageTransmittalHdr.DATAOPMODE);
                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Insert_ImgTransmittalHdr, ht);
                return "success";
            }
            catch (Exception ex)
            {
                return "error" + ex;
                throw ex;
            }
            finally
            {
            }
        }

        public string SaveImageTransmittalDtl(MIImageTransmittalDtl _MIImageTransmittalDtl)
        {
            try
            {
                ht.Clear();
                ht.Add(Utilities.clsParameters.iImgTransmittalDtlId, String.IsNullOrEmpty(_MIImageTransmittalDtl.iImgTransmittalDtlId) ? "" : _MIImageTransmittalDtl.iImgTransmittalDtlId);
                ht.Add(Utilities.clsParameters.iImgTransmittalHdrId, String.IsNullOrEmpty(_MIImageTransmittalDtl.iImgTransmittalHdrId) ? "" : _MIImageTransmittalDtl.iImgTransmittalHdrId);
                ht.Add(Utilities.clsParameters.vWorkspaceId, String.IsNullOrEmpty(_MIImageTransmittalDtl.vWorkspaceId) ? "" : _MIImageTransmittalDtl.vWorkspaceId);
                ht.Add(Utilities.clsParameters.vSubjectId, String.IsNullOrEmpty(_MIImageTransmittalDtl.vSubjectId) ? "" : _MIImageTransmittalDtl.vSubjectId);
                ht.Add(Utilities.clsParameters.iModalityNo, String.IsNullOrEmpty(_MIImageTransmittalDtl.iModalityNo) ? "" : _MIImageTransmittalDtl.iModalityNo);
                ht.Add(Utilities.clsParameters.iAnatomyNo, String.IsNullOrEmpty(_MIImageTransmittalDtl.iAnatomyNo) ? "" : _MIImageTransmittalDtl.iAnatomyNo);
                ht.Add(Utilities.clsParameters.cIVContrast, String.IsNullOrEmpty(_MIImageTransmittalDtl.cIVContrast) ? "" : _MIImageTransmittalDtl.cIVContrast);
                ht.Add(Utilities.clsParameters.dExaminationDate, String.IsNullOrEmpty(_MIImageTransmittalDtl.dExaminationDate) ? "" : _MIImageTransmittalDtl.dExaminationDate);
                ht.Add(Utilities.clsParameters.iNoImages, String.IsNullOrEmpty(_MIImageTransmittalDtl.iNoImages) ? "" : _MIImageTransmittalDtl.iNoImages);
                ht.Add(Utilities.clsParameters.vRemark, String.IsNullOrEmpty(_MIImageTransmittalDtl.vRemark) ? "" : _MIImageTransmittalDtl.vRemark);
                ht.Add(Utilities.clsParameters.iModifyBy, String.IsNullOrEmpty(_MIImageTransmittalDtl.iModifyBy) ? "" : _MIImageTransmittalDtl.iModifyBy);
                ht.Add(Utilities.clsParameters.cStatusIndi, String.IsNullOrEmpty(_MIImageTransmittalDtl.cStatusIndi) ? "" : _MIImageTransmittalDtl.cStatusIndi);
                ht.Add(Utilities.clsParameters.DataopMode, String.IsNullOrEmpty(_MIImageTransmittalDtl.DATAOPMODE) ? "" : _MIImageTransmittalDtl.DATAOPMODE);
                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Insert_ImgTransmittalDtl, ht);
                return "success";
            }
            catch (Exception ex)
            {
                return "error " + ex;
            }
            finally
            {
            }
        }

        public string SaveImageTransmittalnew(MIImageTransmittalNew _MIImageTransmittalnew)
        {
            string sResult = "";
            try
            {
                DataTable dt = new DataTable();
                //dt.Columns.Add("vFileName", typeof(string));
                //dt.Columns.Add("vServerPath", typeof(string));
                //dt.Columns.Add("vFileType", typeof(string));
                //dt.Columns.Add("vSize", typeof(string));
                //dt.Columns.Add("dScheduledDate", typeof(DateTime));
                //DataRow dr = dt.NewRow();
                //dr["vFileName"] = "Vivek";
                //dr["vServerPath"] = "Vivek";
                //dr["vFileType"] = ".dcm";
                //dr["vSize"] = "1651";
                //dr["dScheduledDate"] = DateTime.Now;
                //dt.Rows.Add(dr);
                dt = Utilities.ToDataTable(_MIImageTransmittalnew._SaveImageTransmittal);
                ht.Clear();
                ht.Add(Utilities.clsParameters.iImgTransmittalHdrId, _MIImageTransmittalnew.iImgTransmittalHdrId);
                ht.Add(Utilities.clsParameters.iImgTransmittalDtlId, _MIImageTransmittalnew.iImgTransmittalDtlId);
                ht.Add(Utilities.clsParameters.iModifyBy, _MIImageTransmittalnew.UserID);
                ht.Add(Utilities.clsParameters.iImageMode, _MIImageTransmittalnew.iImageMode);
                ht.Add(Utilities.clsParameters.TransmittalImgDtl, dt);
                //ht.Add(Utilities.clsParameters.ReturnCode, "");
                sResult = _SqlHelper.ExecuteProcudereWithParamsOutPut("Insert_ImageTransmittalDetails", ht);
                return sResult;
            }
            catch (Exception ex)
            {
                return "error " + ex;
            }
            finally
            {
            }
        }

        public DataTable SaveBiznetDicom(MIBizNETSaveImageDTO _MIBizNETSaveImageDTO)
        {
            try
            {
                ht.Clear();
                ht.Add(Utilities.clsParameters.iImgTransmittalHdrId, _MIBizNETSaveImageDTO.iImgTransmittalHdrId);
                ht.Add(Utilities.clsParameters.iImgTransmittalDtlId, _MIBizNETSaveImageDTO.iImgTransmittalDtlId);
                ht.Add(Utilities.clsParameters.vFileName, _MIBizNETSaveImageDTO.vFileName);
                ht.Add(Utilities.clsParameters.vFileType, _MIBizNETSaveImageDTO.vFileType);
                ht.Add(Utilities.clsParameters.iImageMode, _MIBizNETSaveImageDTO.iImageMode);
                ht.Add(Utilities.clsParameters.vServerPath, _MIBizNETSaveImageDTO.vServerPath);
                ht.Add(Utilities.clsParameters.dScheduledDate, _MIBizNETSaveImageDTO.dScheduledDate);
                ht.Add(Utilities.clsParameters.vSize, _MIBizNETSaveImageDTO.vSize);
                ht.Add(Utilities.clsParameters.iModifyBy, _MIBizNETSaveImageDTO.iModifyBy);

                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Insert_BizNetImageTransmittalDetails, ht);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }

            return Sql_Ds.Tables[0];
        }

        public DataTable saveLessionDetails(LesionDetailDTO _LesionDetailDTO)
        {
            try
            {
                Sql_Dt = new DataTable();
                ht.Clear();
                DataTable dtLessionDetails = Utilities.ToDataTable(_LesionDetailDTO._LesionDataDTO);
                ht.Add(Utilities.clsParameters.vWorkspaceId, _LesionDetailDTO.vWorkspaceId);
                ht.Add(Utilities.clsParameters.vSubjectId, _LesionDetailDTO.vSubjectId);
                ht.Add(Utilities.clsParameters.vActivityId, _LesionDetailDTO.vActivityId);
                ht.Add(Utilities.clsParameters.iNodeId, _LesionDetailDTO.iNodeId);
                ht.Add(Utilities.clsParameters.iModifyBy, _LesionDetailDTO.iModifyBy);
                ht.Add(Utilities.clsParameters.DATAMODE, _LesionDetailDTO.DATAMODE);
                ht.Add(Utilities.clsParameters.iMySubjectNo, _LesionDetailDTO.iMySubjectNo);
                ht.Add(Utilities.clsParameters.ScreenNo, _LesionDetailDTO.ScreenNo);
                ht.Add(Utilities.clsParameters.CRFTempDtl, dtLessionDetails);
                ht.Add(Utilities.clsParameters.vParentWorkSpaceId, _LesionDetailDTO.ParentWorkSpaceId);
                ht.Add(Utilities.clsParameters.vPeriodId, _LesionDetailDTO.PeriodId);
                ht.Add(Utilities.clsParameters.MedEx_Result, Convert.ToString(dtLessionDetails.Rows[0]["vMedExResult"]));

                Sql_Dt = _SqlHelper.ExecuteProcudereWithParamsOutPut2(Utilities.clsSp.Insert_CRFTempDetail, ht);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            { }
            return Sql_Dt;
        }

        public DataTable saveMILessionDetails(LesionDetailDTO _LesionDetailDTO)
        {
            try
            {
                Sql_Dt = new DataTable();
                ht.Clear();
                DataTable dtLessionDetails = Utilities.ToDataTable(_LesionDetailDTO._LesionDataDTO);
                ht.Add(Utilities.clsParameters.vWorkspaceId, _LesionDetailDTO.vWorkspaceId);
                ht.Add(Utilities.clsParameters.vSubjectId, _LesionDetailDTO.vSubjectId);
                ht.Add(Utilities.clsParameters.vActivityId, _LesionDetailDTO.vActivityId);
                ht.Add(Utilities.clsParameters.iNodeId, _LesionDetailDTO.iNodeId);
                ht.Add(Utilities.clsParameters.iModifyBy, _LesionDetailDTO.iModifyBy);
                ht.Add(Utilities.clsParameters.DATAMODE, _LesionDetailDTO.DATAMODE);
                ht.Add(Utilities.clsParameters.iMySubjectNo, _LesionDetailDTO.iMySubjectNo);
                ht.Add(Utilities.clsParameters.ScreenNo, _LesionDetailDTO.ScreenNo);
                ht.Add(Utilities.clsParameters.CRFTempDtl, dtLessionDetails);
                ht.Add(Utilities.clsParameters.vParentWorkSpaceId, _LesionDetailDTO.ParentWorkSpaceId);
                ht.Add(Utilities.clsParameters.vPeriodId, _LesionDetailDTO.PeriodId);
                ht.Add(Utilities.clsParameters.MedEx_Result, Convert.ToString(dtLessionDetails.Rows[0]["vMedExResult"]));

                Sql_Dt = _SqlHelper.ExecuteProcudereWithParamsOutPut2(Utilities.clsSp.Insert_CRFTempDetail, ht);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { }
            return Sql_Dt;
        }

        public DataTable SaveMIFinalLession(MIFinalLesionDetailDTO _MIFinalLesionDetailDTO)
        {
            try
            {
                Sql_Dt = new DataTable();
                ht.Clear();
                DataTable dtLessionDetails = Utilities.ToDataTable(_MIFinalLesionDetailDTO.MIFinalLesionDetailDataDTO);
                ht.Add(Utilities.clsParameters.vWorkspaceId, _MIFinalLesionDetailDTO.vWorkspaceId);
                ht.Add(Utilities.clsParameters.vSubjectId, _MIFinalLesionDetailDTO.vSubjectId);
                ht.Add(Utilities.clsParameters.vParentActivityId, _MIFinalLesionDetailDTO.vParentActivityId);
                ht.Add(Utilities.clsParameters.iParentNodeId, _MIFinalLesionDetailDTO.iParentNodeId);
                ht.Add(Utilities.clsParameters.vActivityId, _MIFinalLesionDetailDTO.vActivityId);
                ht.Add(Utilities.clsParameters.iNodeId, _MIFinalLesionDetailDTO.iNodeId);
                ht.Add(Utilities.clsParameters.iModifyBy, _MIFinalLesionDetailDTO.iModifyBy);
                ht.Add(Utilities.clsParameters.DATAMODE, _MIFinalLesionDetailDTO.DATAMODE);
                ht.Add(Utilities.clsParameters.iMySubjectNo, _MIFinalLesionDetailDTO.iMySubjectNo);
                ht.Add(Utilities.clsParameters.ScreenNo, _MIFinalLesionDetailDTO.ScreenNo);
                ht.Add(Utilities.clsParameters.CRFTempDtl, dtLessionDetails);
                ht.Add(Utilities.clsParameters.vParentWorkSpaceId, _MIFinalLesionDetailDTO.ParentWorkSpaceId);
                ht.Add(Utilities.clsParameters.vPeriodId, _MIFinalLesionDetailDTO.PeriodId);
                ht.Add(Utilities.clsParameters.vActivityName, _MIFinalLesionDetailDTO.vActivityName);
                ht.Add(Utilities.clsParameters.vSubActivityName, _MIFinalLesionDetailDTO.vSubActivityName);
                ht.Add(Utilities.clsParameters.MedEx_Result, Convert.ToString(dtLessionDetails.Rows[0]["vMedExResult"]));

                Sql_Dt = _SqlHelper.ExecuteProcudereWithParamsOutPut2(Utilities.clsSp.Insert_CRFTempDetail, ht);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally { }
            return Sql_Dt;
        }

        //public DataTable saveLessionCRFDetails(LesionCRFDTO _LesionCRFDTO)
        //{
        //    try
        //    {
        //        Sql_Dt = new DataTable();
        //        return Sql_Dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //    }
        //}

        //public string SaveImageTransmittal(MIImageTransmittal _MIImageTransmittal)
        //{
        //    string sResult = "";
        //    try
        //    {
        //        string eStr_Retu123 = "";
        //        DataTable dt_Return = new DataTable();
        //        ArrayList DataReturnList = new ArrayList();
        //        //ParameterReturnValue RetuVal = null;
        //        DataTable dtImagesDetails = Utilities.ToDataTable(_MIImageTransmittal._SaveImageTransmittal);
        //        ht.Clear();
        //        ht.Add(Utilities.clsParameters.iImgTransmittalHdrId, _MIImageTransmittal.iImgTransmittalHdrId);
        //        ht.Add(Utilities.clsParameters.iImgTransmittalDtlId, _MIImageTransmittal.iImgTransmittalDtlId);
        //        ht.Add(Utilities.clsParameters.iImgTransmittalImgDtlId, _MIImageTransmittal.iImgTransmittalImgDtlId);
        //        ht.Add(Utilities.clsParameters.vWorkspaceId, _MIImageTransmittal.vWorkspaceId);
        //        ht.Add(Utilities.clsParameters.vProjectNo, _MIImageTransmittal.vProjectNo);
        //        ht.Add(Utilities.clsParameters.vSubjectId, _MIImageTransmittal.vSubjectId);
        //        ht.Add(Utilities.clsParameters.vRandomizationNo, _MIImageTransmittal.vRandomizationNo);
        //        ht.Add(Utilities.clsParameters.iNodeId, _MIImageTransmittal.iNodeId);
        //        ht.Add(Utilities.clsParameters.cDeviation, _MIImageTransmittal.cDeviation);
        //        ht.Add(Utilities.clsParameters.nvDeviationReason, _MIImageTransmittal.nvDeviationReason);
        //        ht.Add(Utilities.clsParameters.nvInstructions, _MIImageTransmittal.nvInstructions);
        //        ht.Add(Utilities.clsParameters.iModalityNo, _MIImageTransmittal.iModalityNo);
        //        ht.Add(Utilities.clsParameters.iAnatomyNo, _MIImageTransmittal.iAnatomyNo);
        //        ht.Add(Utilities.clsParameters.cIVContrast, _MIImageTransmittal.cIVContrast);
        //        ht.Add(Utilities.clsParameters.dExaminationDate, _MIImageTransmittal.dExaminationDate);
        //        ht.Add(Utilities.clsParameters.iNoImages, _MIImageTransmittal.iNoImages);
        //        ht.Add(Utilities.clsParameters.vRemark, String.IsNullOrEmpty(_MIImageTransmittal.vRemark) ? "" : _MIImageTransmittal.vRemark);
        //        ht.Add(Utilities.clsParameters.iModifyBy, _MIImageTransmittal.iModifyBy);
        //        ht.Add(Utilities.clsParameters.iImageMode, _MIImageTransmittal.iImageMode);
        //        ht.Add(Utilities.clsParameters.TransmittalImgDtl, dtImagesDetails);
        //        //sResult = _SqlHelper.ExecuteProcudereWithParamsOutPut("Insert_ImageTransmittalDetails", ht);
        //        if (!_SqlHelper.SaveInDb("Insert_ImageTransmittalDetails", ht, 1, ref DataReturnList, ref eStr_Retu123))
        //        {
        //            sResult = "Error Occured While Retrieving Procedure Info.";
        //            //return false;
        //        }
        //        //DataReturnList = (ArrayList)DataReturnList[0];
        //        //RetuVal = (ParameterReturnValue)DataReturnList[0];
        //        //sResult = RetuVal.ParameterValue.ToString();
        //        sResult = (string)DataReturnList[0];
        //        //foreach (parameterreturnvalue retun_val in datareturnlist)
        //        //{
        //        //    string str = convert.tostring(retun_val.parametervalue);
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        return "error " + ex;
        //    }
        //    finally
        //    {
        //    }
        //    return sResult;
        //}

        public string SaveImageTransmittal(MIImageTransmittal _MIImageTransmittal)
        {
            string sResult = "";
            try
            {
                string eStr_Retu123 = "";
                DataTable dt_Return = new DataTable();
                ArrayList DataReturnList = new ArrayList();
                //ParameterReturnValue RetuVal = null;
                DataTable dtImagesDetails = Utilities.ToDataTable(_MIImageTransmittal._SaveImageTransmittal);
                ht.Clear();
                ht.Add(Utilities.clsParameters.iImgTransmittalHdrId, _MIImageTransmittal.iImgTransmittalHdrId);
                ht.Add(Utilities.clsParameters.iImgTransmittalDtlId, _MIImageTransmittal.iImgTransmittalDtlId);
                ht.Add(Utilities.clsParameters.iImgTransmittalImgDtlId, _MIImageTransmittal.iImgTransmittalImgDtlId);
                ht.Add(Utilities.clsParameters.vWorkspaceId, String.IsNullOrEmpty(_MIImageTransmittal.vWorkspaceId) ? "" : _MIImageTransmittal.vWorkspaceId);
                ht.Add(Utilities.clsParameters.vProjectNo, _MIImageTransmittal.vProjectNo);
                ht.Add(Utilities.clsParameters.vSubjectId, String.IsNullOrEmpty(_MIImageTransmittal.vSubjectId) ? "" : _MIImageTransmittal.vSubjectId);
                ht.Add(Utilities.clsParameters.vRandomizationNo, _MIImageTransmittal.vRandomizationNo);
                ht.Add(Utilities.clsParameters.vMySubjectNo, _MIImageTransmittal.vMySubjectNo);
                ht.Add(Utilities.clsParameters.vDOB, _MIImageTransmittal.vDOB);
                ht.Add(Utilities.clsParameters.iNodeId, String.IsNullOrEmpty(_MIImageTransmittal.iNodeId) ? "" : _MIImageTransmittal.iNodeId);
                ht.Add(Utilities.clsParameters.cDeviation, _MIImageTransmittal.cDeviation);
                ht.Add(Utilities.clsParameters.nvDeviationReason, _MIImageTransmittal.nvDeviationReason);
                ht.Add(Utilities.clsParameters.nvInstructions, _MIImageTransmittal.nvInstructions);
                ht.Add(Utilities.clsParameters.iModalityNo, String.IsNullOrEmpty(_MIImageTransmittal.iModalityNo) ? "" : _MIImageTransmittal.iModalityNo);
                ht.Add(Utilities.clsParameters.iAnatomyNo, _MIImageTransmittal.iAnatomyNo);
                ht.Add(Utilities.clsParameters.cIVContrast, _MIImageTransmittal.cIVContrast);
                ht.Add(Utilities.clsParameters.dExaminationDate, _MIImageTransmittal.dExaminationDate);
                ht.Add(Utilities.clsParameters.iNoImages, String.IsNullOrEmpty(_MIImageTransmittal.iNoImages) ? "" : _MIImageTransmittal.iNoImages);
                ht.Add(Utilities.clsParameters.vRemark, String.IsNullOrEmpty(_MIImageTransmittal.vRemark) ? "" : _MIImageTransmittal.vRemark);
                ht.Add(Utilities.clsParameters.iModifyBy, _MIImageTransmittal.iModifyBy);
                ht.Add(Utilities.clsParameters.iImageMode, _MIImageTransmittal.iImageMode);
                ht.Add(Utilities.clsParameters.vParentWorkspaceId, _MIImageTransmittal.vParentWorkspaceId);
                ht.Add(Utilities.clsParameters.vParentActivityId, _MIImageTransmittal.vParentActivityId);
                ht.Add(Utilities.clsParameters.iParentNodeId, _MIImageTransmittal.iParentNodeId);
                ht.Add(Utilities.clsParameters.vActivityId, _MIImageTransmittal.vActivityId);
                ht.Add(Utilities.clsParameters.TransmittalImgDtl, dtImagesDetails);
                ht.Add(Utilities.clsParameters.cOralContrast, _MIImageTransmittal.cOralContrast);
                ht.Add(Utilities.clsParameters.vModalityNo, _MIImageTransmittal.vModalityNo);
                ht.Add(Utilities.clsParameters.vModalityDesc, _MIImageTransmittal.vModalityDesc);
                ht.Add(Utilities.clsParameters.vAnatomyDesc, _MIImageTransmittal.vAnatomyDesc);
                string strImageMode = Convert.ToString(_MIImageTransmittal.iImageMode);
                if (strImageMode == "1")
                {
                    DataTable dtAnatomyNo = new DataTable();
                    if (!_SqlHelper.GetFromDB("Pro_GetAnatomyNoWithSplit", ht, ref DataReturnList, ref dtAnatomyNo, ref eStr_Retu123))
                    {
                        sResult = "Error Occured While Retrieving Data from AnatomyNo.";
                        return sResult;
                    }
                    if (dtAnatomyNo != null && dtAnatomyNo.Rows.Count != 0)
                    {
                        string strAnatomyNo = Convert.ToString(_MIImageTransmittal.vAnatomyNo);
                        int[] ArrayAnatomyNo = strAnatomyNo.Split(',').Select(int.Parse).ToArray();
                        ArrayList NewArrayAnatomyNo = new ArrayList();
                        int j = 0;
                        foreach (DataRow dr in dtAnatomyNo.Rows)
                        {
                            int strWord = Convert.ToInt32(dr["word"]);

                            int index2 = Array.IndexOf(ArrayAnatomyNo, strWord);
                            NewArrayAnatomyNo.Add(strWord);
                            if (index2 != -1)
                            {
                                ArrayAnatomyNo = ArrayAnatomyNo.Where(val => val != strWord).ToArray();
                            }
                            j++;
                        }
                        NewArrayAnatomyNo.AddRange(ArrayAnatomyNo);
                        NewArrayAnatomyNo.Sort();
                        string newAnatomyNo = String.Join(",", NewArrayAnatomyNo.ToArray());
                        ht.Add(Utilities.clsParameters.vAnatomyNo, newAnatomyNo);
                    }
                    else
                    {
                        ht.Add(Utilities.clsParameters.vAnatomyNo, _MIImageTransmittal.vAnatomyNo);
                    }
                }
                else
                {
                    ht.Add(Utilities.clsParameters.vAnatomyNo, _MIImageTransmittal.vAnatomyNo);
                }

                if (!_SqlHelper.SaveInDb("Insert_ImageTransmittalDetails", ht, 1, ref DataReturnList, ref eStr_Retu123))
                {
                    sResult = "Error Occured While Retrieving Data.";
                    return sResult;
                }
                //DataReturnList = (ArrayList)DataReturnList[0];
                //RetuVal = (ParameterReturnValue)DataReturnList[0];
                //sResult = RetuVal.ParameterValue.ToString();
                //sResult = (string)DataReturnList[1];
                sResult = DataReturnList[0].ToString();
                //foreach (parameterreturnvalue retun_val in datareturnlist)
                //{
                //    string str = convert.tostring(retun_val.parametervalue);
                //}
            }
            catch (Exception ex)
            {
                return "error " + ex;
            }
            finally
            {
            }
            return sResult;
        }

        public string Save_CRFHdrDtlSubDtl(Save_CRFHdrDtlSubDtlDTO _Save_CRFHdrDtlSubDtlDTO)
        {
            string sResult = "";
            try
            {
                string str_ReturnValue1 = "";
                string str_ReturnValue2 = "";
                ArrayList DataReturnList = new ArrayList();
                DataSet dsResult = new DataSet();

                //DataTable dtCRFHdrDtlSubDtl = Utilities.ToDataTable(_Save_CRFHdrDtlSubDtlDTO._Save_CRFHdrDtlSubDtlList);
                string SubjectId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vSubjectId);
                string WorkspaceId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vWorkspaceId);
                string ParentWorkspaceId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vParentWorkSpaceId);
                string ActivityId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vActivityId);
                string NodeId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.iNodeId);
                string PeriodId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vPeriodId);
                string MySubjectNo = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.iMySubjectNo);

                ht.Clear();
                ht.Add(Utilities.clsParameters.vSubjectId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vSubjectId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vSubjectId);
                ht.Add(Utilities.clsParameters.vWorkspaceId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vWorkspaceId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vWorkspaceId);
                ht.Add(Utilities.clsParameters.ParentWorkspaceId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vParentWorkSpaceId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vParentWorkSpaceId);
                ht.Add(Utilities.clsParameters.ActivityId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vActivityId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vActivityId);
                ht.Add(Utilities.clsParameters.iNodeId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.iNodeId) ? "" : _Save_CRFHdrDtlSubDtlDTO.iNodeId);
                ht.Add(Utilities.clsParameters.PeriodId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vPeriodId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vPeriodId);
                ht.Add(Utilities.clsParameters.MySubjectNo, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.iMySubjectNo) ? "" : _Save_CRFHdrDtlSubDtlDTO.iMySubjectNo);
                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams("Proc_GetCRFTempDetail", ht);

                if (Sql_Ds != null && Sql_Ds.Tables.Count > 0)
                {
                    DataTable dt_TOP_CRFHdrDtlSubDtl = Sql_Ds.Tables[0];
                    DataTable dtNoOfRepetation = Sql_Ds.Tables[1];

                    if (dt_TOP_CRFHdrDtlSubDtl != null && dt_TOP_CRFHdrDtlSubDtl.Rows.Count != 0)
                    {
                        foreach (DataRow dr_NoOfRepetation in dtNoOfRepetation.Rows)
                        {
                            int? iRepetationNo = Convert.ToInt32(dr_NoOfRepetation["iRepetationNo"]);

                            DataView dv = new DataView(dt_TOP_CRFHdrDtlSubDtl);
                            dv.RowFilter = "iRepetationNo=" + iRepetationNo + "";
                            DataTable dtCRFHdrDtlSubDtl = dv.ToTable();
                            string ModifyBy = Convert.ToString(dtCRFHdrDtlSubDtl.Rows[0]["iModifyBy"]);

                            if (!AssignValues(SubjectId, WorkspaceId, ParentWorkspaceId, ActivityId, NodeId, PeriodId, MySubjectNo, ModifyBy, dtCRFHdrDtlSubDtl, ref dsResult))
                            {
                                sResult = "Error Occured While Retrieving Procedure Info.";
                                return sResult;
                            }

                            try
                            {
                                _SqlHelper.Open_Connection();
                                _SqlHelper.Begin_Transaction();
                                bool bol_CRFHdrDtlSubDtl = true;

                                if (_SqlHelper == null)
                                {
                                    sResult = "Data Object Is Invalid or Nothing";
                                    return sResult;
                                }

                                if (_SqlHelper.Transaction == null)
                                {
                                    sResult = "Transaction object is not started";
                                    return sResult;
                                }

                                str_ReturnValue1 = "";
                                str_ReturnValue2 = "";
                                DataReturnList = null;
                                DataReturnList = new ArrayList();
                                DataTable Tbl_DtCRFHdr = dsResult.Tables["CRFHdr"];
                                DataTable Tbl_DtCRFDtl = dsResult.Tables["CRFDtl"];
                                DataTable Tbl_DtCRFSubDtl = dsResult.Tables["CRFSubDtl"];
                                DataTable Tbl_DtDCFMst = dsResult.Tables["DCFMst"];
                                if (!_SqlHelper.SaveInDbDataTable("Insert_CRFHdr", Tbl_DtCRFHdr, 1, ref DataReturnList, ref sResult))
                                {
                                    sResult = sResult + " Error occured while Save Information of CRFHdr In Database";
                                    return sResult;
                                }

                                str_ReturnValue1 = (string)DataReturnList[0];
                                foreach (DataRow dr in Tbl_DtCRFDtl.Rows)
                                {
                                    dr["nCRFHdrNo"] = Convert.ToInt32(str_ReturnValue1);
                                    dr.AcceptChanges();
                                }
                                Tbl_DtCRFDtl.AcceptChanges();

                                DataTable Tbl_Top_CRFDtl = Tbl_DtCRFDtl.Copy();
                                foreach (DataRow dr in Tbl_Top_CRFDtl.Rows)
                                {
                                    dr["iRepeatNo"] = 0;
                                    dr.AcceptChanges();
                                }
                                Tbl_Top_CRFDtl.AcceptChanges();

                                str_ReturnValue1 = "";
                                str_ReturnValue2 = "";
                                DataReturnList = null;
                                DataReturnList = new ArrayList();
                                if (!_SqlHelper.SaveInDbDataTable("Insert_CRFDtl", Tbl_Top_CRFDtl, 1, ref DataReturnList, ref sResult))
                                {
                                    sResult = sResult + " Error occured while Save Information of CRFDtl In Database";
                                    return sResult;
                                }
                                Tbl_DtCRFDtl.Rows[0].Delete();
                                Tbl_DtCRFDtl.AcceptChanges();

                                str_ReturnValue1 = (string)DataReturnList[0];
                                str_ReturnValue2 = (string)DataReturnList[1];

                                foreach (DataRow dr in Tbl_DtCRFDtl.Rows)
                                {
                                    dr["nCRFDtlNo"] = Convert.ToInt32(str_ReturnValue1);
                                    dr["iRepeatNo"] = Convert.ToInt32(str_ReturnValue2);
                                    dr.AcceptChanges();
                                }
                                Tbl_DtCRFDtl.AcceptChanges();

                                foreach (DataRow dr in Tbl_Top_CRFDtl.Rows)
                                {
                                    dr["nCRFDtlNo"] = Convert.ToInt32(str_ReturnValue1);
                                    dr["iRepeatNo"] = Convert.ToInt32(str_ReturnValue2);
                                    dr.AcceptChanges();
                                }
                                Tbl_Top_CRFDtl.AcceptChanges();

                                foreach (DataRow dr in Tbl_DtCRFSubDtl.Rows)
                                {
                                    dr["nCRFDtlNo"] = Convert.ToInt32(str_ReturnValue1);
                                    dr.AcceptChanges();
                                }
                                Tbl_DtCRFSubDtl.AcceptChanges();

                                DataTable Tbl_Top_CRFSubDtl = Tbl_DtCRFSubDtl.Copy();
                                for (int i = Tbl_Top_CRFSubDtl.Rows.Count - 1; i >= 1; i += -1)
                                {
                                    Tbl_Top_CRFSubDtl.Rows[i].Delete();
                                    Tbl_Top_CRFSubDtl.AcceptChanges();
                                }
                                foreach (DataRow dr in Tbl_Top_CRFSubDtl.Rows)
                                {
                                    dr["iTranNo"] = 0;
                                    dr.AcceptChanges();
                                }
                                Tbl_Top_CRFSubDtl.AcceptChanges();

                                str_ReturnValue1 = "";
                                str_ReturnValue2 = "";
                                DataReturnList = null;
                                DataReturnList = new ArrayList();
                                if (!_SqlHelper.SaveInDbDataTable("Insert_CRFSubDtl", Tbl_Top_CRFSubDtl, 1, ref DataReturnList, ref sResult))
                                {
                                    sResult = sResult + " Error occured while Save Information of CRFSubDtl In Database";
                                    return sResult;
                                }
                                Tbl_DtCRFSubDtl.Rows[0].Delete();
                                Tbl_DtCRFSubDtl.AcceptChanges();

                                str_ReturnValue1 = (string)DataReturnList[0];
                                foreach (DataRow dr in Tbl_DtCRFSubDtl.Rows)
                                {
                                    dr["iTranNo"] = Convert.ToInt32(str_ReturnValue1);
                                    dr.AcceptChanges();
                                }
                                Tbl_DtCRFSubDtl.AcceptChanges();

                                str_ReturnValue1 = "";
                                str_ReturnValue2 = "";
                                DataReturnList = null;
                                DataReturnList = new ArrayList();
                                //Now saving Remaining Rows with same iTranNo
                                if (!_SqlHelper.SaveInDbDataTable("Insert_CRFSubDtl", Tbl_DtCRFSubDtl, 1, ref DataReturnList, ref sResult))
                                {
                                    sResult = sResult + " Error occured while Save Information of CRFSubDtl In Database";
                                    return sResult;
                                }

                                foreach (DataRow dr in Tbl_Top_CRFDtl.Rows)
                                {
                                    string strcDataStatus = Convert.ToString(dr["cDataStatus"]);
                                    if (strcDataStatus == "C")
                                    {
                                        dr["cDataStatus"] = "D";
                                    }
                                }
                                Tbl_Top_CRFDtl.AcceptChanges();

                                str_ReturnValue1 = "";
                                str_ReturnValue2 = "";
                                DataReturnList = null;
                                DataReturnList = new ArrayList();
                                if (!_SqlHelper.SaveInDbDataTable("Insert_CRFDtl", Tbl_Top_CRFDtl, 1, ref DataReturnList, ref sResult))
                                {
                                    sResult = sResult + " Error occured while Save Information of CRFDtl In Database";
                                    return sResult;
                                }

                                if (bol_CRFHdrDtlSubDtl == true)
                                {
                                    _SqlHelper.Commit_Transaction();
                                }
                                else
                                {
                                    _SqlHelper.RollBack_Transaction();
                                }
                            }
                            catch (Exception cx)
                            {
                                sResult = cx.Message + " Error occured while saving CRFHdrDtlSubDtl";
                                _SqlHelper.RollBack_Transaction();
                                throw;
                            }
                            finally
                            {
                                _SqlHelper.Close_Connection();
                            }

                            if (iRepetationNo != null)
                            {
                                ht.Clear();
                                ht.Add(Utilities.clsParameters.vSubjectId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vSubjectId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vSubjectId);
                                ht.Add(Utilities.clsParameters.vWorkspaceId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vWorkspaceId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vWorkspaceId);
                                ht.Add(Utilities.clsParameters.ParentWorkspaceId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vParentWorkSpaceId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vParentWorkSpaceId);
                                ht.Add(Utilities.clsParameters.ActivityId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vActivityId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vActivityId);
                                ht.Add(Utilities.clsParameters.iNodeId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.iNodeId) ? "" : _Save_CRFHdrDtlSubDtlDTO.iNodeId);
                                ht.Add(Utilities.clsParameters.PeriodId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vPeriodId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vPeriodId);
                                ht.Add(Utilities.clsParameters.MySubjectNo, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.iMySubjectNo) ? "" : _Save_CRFHdrDtlSubDtlDTO.iMySubjectNo);
                                ht.Add(Utilities.clsParameters.RepetationNo, iRepetationNo);
                                if (!_SqlHelper.SaveInDb("Proc_RemoveCRFTempDetail", ht, 1, ref DataReturnList, ref sResult))
                                {
                                    sResult = sResult + " Error occured while Remove Information of CRFTempDetail";
                                    return sResult;
                                }
                            }
                        }
                        sResult = "Success";
                    }
                    else
                    {
                        sResult = "Records not found.";
                    }
                }
                else
                {
                    sResult = "Records not found.";
                }
            }
            catch (Exception ex)
            {
                sResult = "Error " + ex.Message;
                return sResult;
            }
            finally
            {
                //_SqlHelper.Close_Connection();
                //_SqlHelper.Close_ConnectionBiz();
            }
            return sResult;
        }

        public string Save_CRFHdrDtlSubDtl(Save_CRFHdrDtlSubDtlDTO _Save_CRFHdrDtlSubDtlDTO, MIFinalLesionDataDTO _MIFinalLesionDataDTO)
        {
            string sResult = "";
            try
            {
                string str_ReturnValue1 = "";
                string str_ReturnValue2 = "";
                ArrayList DataReturnList = new ArrayList();
                DataSet dsResult = new DataSet();

                //DataTable dtCRFHdrDtlSubDtl = Utilities.ToDataTable(_Save_CRFHdrDtlSubDtlDTO._Save_CRFHdrDtlSubDtlList);
                string SubjectId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vSubjectId);
                string WorkspaceId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vWorkspaceId);
                string ParentWorkspaceId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vParentWorkSpaceId);
                string ActivityId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vActivityId);
                string NodeId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.iNodeId);
                string PeriodId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vPeriodId);
                string MySubjectNo = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.iMySubjectNo);

                ht.Clear();
                ht.Add(Utilities.clsParameters.vSubjectId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vSubjectId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vSubjectId);
                ht.Add(Utilities.clsParameters.vWorkspaceId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vWorkspaceId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vWorkspaceId);
                ht.Add(Utilities.clsParameters.ParentWorkspaceId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vParentWorkSpaceId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vParentWorkSpaceId);
                ht.Add(Utilities.clsParameters.ActivityId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vActivityId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vActivityId);
                ht.Add(Utilities.clsParameters.iNodeId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.iNodeId) ? "" : _Save_CRFHdrDtlSubDtlDTO.iNodeId);
                ht.Add(Utilities.clsParameters.PeriodId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vPeriodId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vPeriodId);
                ht.Add(Utilities.clsParameters.MySubjectNo, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.iMySubjectNo) ? "" : _Save_CRFHdrDtlSubDtlDTO.iMySubjectNo);
                try
                {
                    Sql_Ds = _SqlHelper.ExecuteProcudereWithParams("Proc_GetCRFTempDetail", ht);
                }
                catch (Exception ex)
                {
                    sResult = ex.Message + " Error occured while Retriving Value from Proc_GetCRFTempDetail";
                    _SqlHelper.RollBack_Transaction();
                    //_SqlHelper.RollBack_TransactionBiz();
                }

                if (Sql_Ds != null && Sql_Ds.Tables.Count > 0)
                {
                    DataTable dt_TOP_CRFHdrDtlSubDtl = Sql_Ds.Tables[0];
                    DataTable dtNoOfRepetation = Sql_Ds.Tables[1];


                    if (dt_TOP_CRFHdrDtlSubDtl != null && dt_TOP_CRFHdrDtlSubDtl.Rows.Count != 0)
                    {
                        bool save = false;
                        string result = string.Empty;

                        //_SqlHelper.Open_ConnectionBiz();
                        //_SqlHelper.Begin_TransactionBiz();
                        _SqlHelper.Open_Connection();
                        _SqlHelper.Begin_Transaction();

                        try
                        {
                            foreach (DataRow dr_NoOfRepetation in dtNoOfRepetation.Rows)
                            {

                                int? iRepetationNo = Convert.ToInt32(dr_NoOfRepetation["iRepetationNo"]);

                                DataView dv = new DataView(dt_TOP_CRFHdrDtlSubDtl);
                                dv.RowFilter = "iRepetationNo=" + iRepetationNo + "";
                                DataTable dtCRFHdrDtlSubDtl = dv.ToTable();
                                string ModifyBy = Convert.ToString(dtCRFHdrDtlSubDtl.Rows[0]["iModifyBy"]);
                                try
                                {
                                    if (!AssignValues(SubjectId, WorkspaceId, ParentWorkspaceId, ActivityId, NodeId, PeriodId, MySubjectNo, ModifyBy, dtCRFHdrDtlSubDtl, ref dsResult))
                                    {
                                        result = "Error Occured While Retrieving Procedure Info.";
                                        save = false;
                                        break;
                                        //sResult = "Error Occured While Retrieving Procedure Info.";                                
                                        //return sResult;
                                    }

                                }
                                catch (Exception ex)
                                {
                                    sResult = ex.Message + " Error occured while Assigning value";
                                    _SqlHelper.RollBack_Transaction();
                                    //_SqlHelper.RollBack_TransactionBiz();
                                }



                                //bool bol_CRFHdrDtlSubDtl = true;
                                if (_SqlHelper == null)
                                {
                                    result = "Data Object Is Invalid or Nothing";
                                    save = false;
                                    break;
                                    //sResult = "Data Object Is Invalid or Nothing";
                                    //return sResult;
                                }

                                if (_SqlHelper.Transaction == null)
                                {
                                    result = "Transaction object is not started";
                                    save = false;
                                    break;
                                    //sResult = "Transaction object is not started";
                                    //return sResult;
                                }

                                string wstr = "vWorkspaceId='" + _Save_CRFHdrDtlSubDtlDTO.vWorkspaceId + "' And vSubjectId='" + _Save_CRFHdrDtlSubDtlDTO.vSubjectId + "' And iNodeId='" + _Save_CRFHdrDtlSubDtlDTO.iNodeId + "' And vactivityid = '" + _Save_CRFHdrDtlSubDtlDTO.vActivityId + "' and cStatusIndi <> 'D' ";
                                try
                                {
                                    dt_CRFSubDtl = _SqlHelper.GetDataTable(Utilities.clsView.View_GetCRFSubDtl, wstr, Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                                }
                                catch (Exception ex)
                                {
                                    sResult = ex.Message + " Error occured while Retriving dt_CRFSubDtl Detail";
                                    _SqlHelper.RollBack_Transaction();
                                    //_SqlHelper.RollBack_TransactionBiz();
                                }


                                if (dt_CRFSubDtl.Rows.Count != 0)
                                {
                                    str_ReturnValue1 = "";
                                    str_ReturnValue2 = "";
                                    DataReturnList = null;
                                    DataReturnList = new ArrayList();
                                    DataTable Tbl_DtCRFHdr = dsResult.Tables["CRFHdr"];
                                    DataTable Tbl_DtCRFDtl = dsResult.Tables["CRFDtl"];
                                    DataTable Tbl_DtCRFSubDtl = dsResult.Tables["CRFSubDtl"];
                                    DataTable Tbl_DtDCFMst = dsResult.Tables["DCFMst"];



                                    //foreach (DataRow dr in Tbl_DtCRFSubDtl.Rows)
                                    //{
                                    //    dr["nCRFDtlNo"] = Convert.ToInt32(dt_CRFSubDtl.Rows[0]["nCRFDtlNo"]);
                                    //    dr.AcceptChanges();
                                    //}
                                    //Tbl_DtCRFSubDtl.AcceptChanges();

                                    //DataTable Tbl_Top_CRFSubDtl = Tbl_DtCRFSubDtl.Copy();
                                    //for (int i = Tbl_Top_CRFSubDtl.Rows.Count - 1; i >= 1; i += -1)
                                    //{
                                    //    Tbl_Top_CRFSubDtl.Rows[i].Delete();
                                    //    Tbl_Top_CRFSubDtl.AcceptChanges();
                                    //}
                                    //foreach (DataRow dr in Tbl_Top_CRFSubDtl.Rows)
                                    //{
                                    //    dr["iTranNo"] = 0;
                                    //    dr.AcceptChanges();
                                    //}
                                    //Tbl_Top_CRFSubDtl.AcceptChanges();

                                    //str_ReturnValue1 = "";
                                    //str_ReturnValue2 = "";
                                    //DataReturnList = null;
                                    //DataReturnList = new ArrayList();
                                    //if (!_SqlHelper.SaveInDbDataTable("Insert_CRFSubDtl", Tbl_Top_CRFSubDtl, 1, ref DataReturnList, ref sResult))
                                    //{
                                    //    result = "Error occured while Save Information of CRFSubDtl In Database";
                                    //    save = false;
                                    //    break;                                        
                                    //}
                                    //Tbl_DtCRFSubDtl.Rows[0].Delete();
                                    //Tbl_DtCRFSubDtl.AcceptChanges();

                                    //str_ReturnValue1 = (string)DataReturnList[0];
                                    foreach (DataRow dr in Tbl_DtCRFSubDtl.Rows)
                                    {
                                        dr["nCRFDtlNo"] = Convert.ToInt32(dt_CRFSubDtl.Rows[0]["nCRFDtlNo"]);
                                        dr["iTranNo"] = "1";
                                        dr.AcceptChanges();
                                    }
                                    Tbl_DtCRFSubDtl.AcceptChanges();

                                    str_ReturnValue1 = "";
                                    str_ReturnValue2 = "";
                                    DataReturnList = null;
                                    DataReturnList = new ArrayList();
                                    //Now saving Remaining Rows with same iTranNo
                                    try
                                    {
                                        if (!_SqlHelper.SaveInDbDataTable("Insert_CRFSubDtl", Tbl_DtCRFSubDtl, 1, ref DataReturnList, ref sResult))
                                        {
                                            result = "Error occured while Save Information of CRFSubDtl In Database";
                                            save = false;
                                            break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        sResult = ex.Message + " Error occured while saving Insert_CRFSubDtl For Remaining Rows With Same ITranNo for Updation";
                                        _SqlHelper.RollBack_Transaction();
                                        //_SqlHelper.RollBack_TransactionBiz();
                                    }



                                }
                                else
                                {
                                    str_ReturnValue1 = "";
                                    str_ReturnValue2 = "";
                                    DataReturnList = null;
                                    DataReturnList = new ArrayList();
                                    DataTable Tbl_DtCRFHdr = dsResult.Tables["CRFHdr"];
                                    DataTable Tbl_DtCRFDtl = dsResult.Tables["CRFDtl"];
                                    DataTable Tbl_DtCRFSubDtl = dsResult.Tables["CRFSubDtl"];
                                    DataTable Tbl_DtDCFMst = dsResult.Tables["DCFMst"];
                                    try
                                    {
                                        if (!_SqlHelper.SaveInDbDataTable("Insert_CRFHdr", Tbl_DtCRFHdr, 1, ref DataReturnList, ref sResult))
                                        {
                                            result = "Error occured while Save Information of CRFHdr In Database";
                                            save = false;
                                            break;
                                            //sResult = sResult + " Error occured while Save Information of CRFHdr In Database";
                                            //save = false;
                                            //return sResult;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        sResult = ex.Message + " Error occured while saving Insert_CRFHdr";
                                        _SqlHelper.RollBack_Transaction();
                                        //_SqlHelper.RollBack_TransactionBiz();
                                    }

                                    str_ReturnValue1 = (string)DataReturnList[0];
                                    foreach (DataRow dr in Tbl_DtCRFDtl.Rows)
                                    {
                                        dr["nCRFHdrNo"] = Convert.ToInt32(str_ReturnValue1);
                                        dr.AcceptChanges();
                                    }
                                    Tbl_DtCRFDtl.AcceptChanges();

                                    DataTable Tbl_Top_CRFDtl = Tbl_DtCRFDtl.Copy();
                                    foreach (DataRow dr in Tbl_Top_CRFDtl.Rows)
                                    {
                                        dr["iRepeatNo"] = 0;
                                        dr.AcceptChanges();
                                    }
                                    Tbl_Top_CRFDtl.AcceptChanges();

                                    str_ReturnValue1 = "";
                                    str_ReturnValue2 = "";
                                    DataReturnList = null;
                                    DataReturnList = new ArrayList();
                                    try
                                    {
                                        if (!_SqlHelper.SaveInDbDataTable("Insert_CRFDtl", Tbl_Top_CRFDtl, 1, ref DataReturnList, ref sResult))
                                        {
                                            result = "Error occured while Save Information of CRFDtl In Database";
                                            save = false;
                                            break;
                                            //sResult = sResult + " Error occured while Save Information of CRFDtl In Database";
                                            //return sResult;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        sResult = ex.Message + " Error occured while saving Insert_CRFDtl";
                                        _SqlHelper.RollBack_Transaction();
                                        //_SqlHelper.RollBack_TransactionBiz();
                                    }

                                    Tbl_DtCRFDtl.Rows[0].Delete();
                                    Tbl_DtCRFDtl.AcceptChanges();

                                    str_ReturnValue1 = (string)DataReturnList[0];
                                    str_ReturnValue2 = (string)DataReturnList[1];

                                    foreach (DataRow dr in Tbl_DtCRFDtl.Rows)
                                    {
                                        dr["nCRFDtlNo"] = Convert.ToInt32(str_ReturnValue1);
                                        dr["iRepeatNo"] = Convert.ToInt32(str_ReturnValue2);
                                        dr.AcceptChanges();
                                    }
                                    Tbl_DtCRFDtl.AcceptChanges();

                                    foreach (DataRow dr in Tbl_Top_CRFDtl.Rows)
                                    {
                                        dr["nCRFDtlNo"] = Convert.ToInt32(str_ReturnValue1);
                                        dr["iRepeatNo"] = Convert.ToInt32(str_ReturnValue2);
                                        dr.AcceptChanges();
                                    }
                                    Tbl_Top_CRFDtl.AcceptChanges();

                                    foreach (DataRow dr in Tbl_DtCRFSubDtl.Rows)
                                    {
                                        dr["nCRFDtlNo"] = Convert.ToInt32(str_ReturnValue1);
                                        dr.AcceptChanges();
                                    }
                                    Tbl_DtCRFSubDtl.AcceptChanges();

                                    DataTable Tbl_Top_CRFSubDtl = Tbl_DtCRFSubDtl.Copy();
                                    for (int i = Tbl_Top_CRFSubDtl.Rows.Count - 1; i >= 1; i += -1)
                                    {
                                        Tbl_Top_CRFSubDtl.Rows[i].Delete();
                                        Tbl_Top_CRFSubDtl.AcceptChanges();
                                    }
                                    foreach (DataRow dr in Tbl_Top_CRFSubDtl.Rows)
                                    {
                                        dr["iTranNo"] = 0;
                                        dr.AcceptChanges();
                                    }
                                    Tbl_Top_CRFSubDtl.AcceptChanges();

                                    str_ReturnValue1 = "";
                                    str_ReturnValue2 = "";
                                    DataReturnList = null;
                                    DataReturnList = new ArrayList();
                                    try
                                    {
                                        if (!_SqlHelper.SaveInDbDataTable("Insert_CRFSubDtl", Tbl_Top_CRFSubDtl, 1, ref DataReturnList, ref sResult))
                                        {
                                            result = "Error occured while Save Information of CRFSubDtl In Database";
                                            save = false;
                                            break;
                                            //sResult = sResult + " Error occured while Save Information of CRFSubDtl In Database";
                                            //return sResult;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        sResult = ex.Message + " Error occured while saving Insert_CRFSubDtl";
                                        _SqlHelper.RollBack_Transaction();
                                        //_SqlHelper.RollBack_TransactionBiz();
                                    }

                                    Tbl_DtCRFSubDtl.Rows[0].Delete();
                                    Tbl_DtCRFSubDtl.AcceptChanges();

                                    str_ReturnValue1 = (string)DataReturnList[0];
                                    foreach (DataRow dr in Tbl_DtCRFSubDtl.Rows)
                                    {
                                        dr["iTranNo"] = Convert.ToInt32(str_ReturnValue1);
                                        dr.AcceptChanges();
                                    }
                                    Tbl_DtCRFSubDtl.AcceptChanges();

                                    str_ReturnValue1 = "";
                                    str_ReturnValue2 = "";
                                    DataReturnList = null;
                                    DataReturnList = new ArrayList();
                                    //Now saving Remaining Rows with same iTranNo
                                    try
                                    {
                                        if (!_SqlHelper.SaveInDbDataTable("Insert_CRFSubDtl", Tbl_DtCRFSubDtl, 1, ref DataReturnList, ref sResult))
                                        {
                                            result = "Error occured while Save Information of CRFSubDtl In Database";
                                            save = false;
                                            break;
                                            //sResult = sResult + " Error occured while Save Information of CRFSubDtl In Database";
                                            //return sResult;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        sResult = ex.Message + " Error occured while saving Insert_CRFSubDtl for Remaining Rows with same iTranNo";
                                        _SqlHelper.RollBack_Transaction();
                                        //_SqlHelper.RollBack_TransactionBiz();
                                    }


                                    foreach (DataRow dr in Tbl_Top_CRFDtl.Rows)
                                    {
                                        string strcDataStatus = Convert.ToString(dr["cDataStatus"]);
                                        if (strcDataStatus == "C")
                                        {
                                            dr["cDataStatus"] = "D";
                                        }
                                    }
                                    Tbl_Top_CRFDtl.AcceptChanges();

                                    str_ReturnValue1 = "";
                                    str_ReturnValue2 = "";
                                    DataReturnList = null;
                                    DataReturnList = new ArrayList();
                                    try
                                    {
                                        if (!_SqlHelper.SaveInDbDataTable("Insert_CRFDtl", Tbl_Top_CRFDtl, 1, ref DataReturnList, ref sResult))
                                        {
                                            result = "Error occured while Save Information of CRFDtl In Database";
                                            save = false;
                                            break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        sResult = ex.Message + " Error occured while saving Insert_CRFDtl for Remaining Rows with same iTranNo";
                                        _SqlHelper.RollBack_Transaction();
                                        //_SqlHelper.RollBack_TransactionBiz();
                                    }

                                }

                                if (iRepetationNo != null)
                                {
                                    ht.Clear();
                                    ht.Add(Utilities.clsParameters.vSubjectId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vSubjectId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vSubjectId);
                                    ht.Add(Utilities.clsParameters.vWorkspaceId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vWorkspaceId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vWorkspaceId);
                                    ht.Add(Utilities.clsParameters.ParentWorkspaceId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vParentWorkSpaceId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vParentWorkSpaceId);
                                    ht.Add(Utilities.clsParameters.ActivityId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vActivityId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vActivityId);
                                    ht.Add(Utilities.clsParameters.iNodeId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.iNodeId) ? "" : _Save_CRFHdrDtlSubDtlDTO.iNodeId);
                                    ht.Add(Utilities.clsParameters.PeriodId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vPeriodId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vPeriodId);
                                    ht.Add(Utilities.clsParameters.MySubjectNo, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.iMySubjectNo) ? "" : _Save_CRFHdrDtlSubDtlDTO.iMySubjectNo);
                                    ht.Add(Utilities.clsParameters.RepetationNo, iRepetationNo);
                                    try
                                    {
                                        int resultdata = _SqlHelper.ExecuteSP("Proc_RemoveCRFTempDetail", ht);
                                        if (resultdata != -1)
                                        {
                                            result = "Error occured while Save Information of CRFDtl In Database";
                                            save = false;
                                            break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        sResult = ex.Message + " Error occured while saving Proc_RemoveCRFTempDetail";
                                        _SqlHelper.RollBack_Transaction();
                                        //_SqlHelper.RollBack_TransactionBiz();
                                    }


                                    //if (!_SqlHelper.SaveInDb("Proc_RemoveCRFTempDetail", ht, 1, ref DataReturnList, ref sResult))
                                    //{
                                    //    sResult = sResult + " Error occured while Remove Information of CRFTempDetail";
                                    //    return sResult;
                                    //}                                    
                                }
                                result = "success";
                                save = true;
                            }

                            //for mi image save

                            try
                            {
                                if (save == true)
                                {
                                    ht.Clear();
                                    DataTable dt = new DataTable();
                                    dt = Utilities.ToDataTable(_MIFinalLesionDataDTO.MIFinalLesionImageList);
                                    ht.Clear();
                                    ht.Add(Utilities.clsParameters.iImgTransmittalHdrId, _MIFinalLesionDataDTO.iImgTransmittalHdrId);
                                    ht.Add(Utilities.clsParameters.iImgTransmittalDtlId, _MIFinalLesionDataDTO.iImgTransmittalDtlId);
                                    ht.Add(Utilities.clsParameters.iModifyBy, _MIFinalLesionDataDTO.UserID);
                                    ht.Add(Utilities.clsParameters.iImageMode, _MIFinalLesionDataDTO.iImageMode);
                                    ht.Add(Utilities.clsParameters.TransmittalImgDtl, dt);
                                    ht.Add(Utilities.clsParameters.vLesionType, _MIFinalLesionDataDTO.vLesionType);
                                    ht.Add(Utilities.clsParameters.vLesionSubType, _MIFinalLesionDataDTO.vLesionSubType);
                                    ht.Add(Utilities.clsParameters.cRadiologist, _MIFinalLesionDataDTO.cRadiologist);
                                    ht.Add(Utilities.clsParameters.ImageTransmittalImgDtl_iImageTranNo, _MIFinalLesionDataDTO.iImageTranNo);
                                    //ht.Add(Utilities.clsParameters.ReturnCode, "");
                                    try
                                    {
                                        sResult = _SqlHelper.ExecuteProcedure("Insert_ImageTransmittalDetails", ht);
                                    }
                                    catch (Exception ex)
                                    {
                                        sResult = ex.Message + " Error occured while saving Insert_ImageTransmittalDetails";
                                        _SqlHelper.RollBack_Transaction();
                                        //_SqlHelper.RollBack_TransactionBiz();
                                    }


                                    if (sResult == "1")
                                    {
                                        sResult = "success";
                                        //_SqlHelper.Commit_TransactionBiz();
                                        _SqlHelper.Commit_Transaction();
                                    }
                                    else
                                    {
                                        sResult = "error";
                                        //_SqlHelper.RollBack_TransactionBiz();
                                        _SqlHelper.RollBack_Transaction();
                                    }

                                }
                                else
                                {
                                    sResult = result;
                                    //_SqlHelper.RollBack_TransactionBiz();
                                    _SqlHelper.RollBack_Transaction();
                                }
                            }
                            catch (Exception e)
                            {
                                sResult = e.Message + " Error occured while saving Insert_ImageTransmittalDetails";
                                _SqlHelper.RollBack_Transaction();
                                //_SqlHelper.RollBack_TransactionBiz();
                            }

                        }

                        catch (Exception cx)
                        {
                            //sResult = cx.Message + " Error occured while saving CRFHdrDtlSubDtl";
                            _SqlHelper.RollBack_Transaction();
                            //_SqlHelper.RollBack_TransactionBiz();
                            //throw;
                        }
                        finally
                        {
                            _SqlHelper.Close_Connection();
                            //_SqlHelper.Close_ConnectionBiz();                            
                        }

                    }
                    else
                    {
                        sResult = "RecordsNotFound";
                    }
                }
                else
                {
                    sResult = "RecordsNotFound";
                }
            }
            catch (Exception ex)
            {
                //sResult = "Error " + ex.Message;
                return sResult;
            }
            finally
            {
                //_SqlHelper.Close_Connection();
                //_SqlHelper.Close_ConnectionBiz();
            }
            return sResult;
        }

        public string Skip_CRFHdrDtlSubDtl(Save_CRFHdrDtlSubDtlDTO _Save_CRFHdrDtlSubDtlDTO)
        {
            string sResult = "";
            try
            {
                string str_ReturnValue1 = "";
                string str_ReturnValue2 = "";
                ArrayList DataReturnList = new ArrayList();
                DataSet dsResult = new DataSet();

                //DataTable dtCRFHdrDtlSubDtl = Utilities.ToDataTable(_Save_CRFHdrDtlSubDtlDTO._Save_CRFHdrDtlSubDtlList);
                string SubjectId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vSubjectId);
                string WorkspaceId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vWorkspaceId);
                string ParentWorkspaceId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vParentWorkSpaceId);
                string ActivityId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vActivityId);
                string NodeId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.iNodeId);
                string PeriodId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vPeriodId);
                string MySubjectNo = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.iMySubjectNo);

                ht.Clear();
                ht.Add(Utilities.clsParameters.vSubjectId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vSubjectId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vSubjectId);
                ht.Add(Utilities.clsParameters.vWorkspaceId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vWorkspaceId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vWorkspaceId);
                ht.Add(Utilities.clsParameters.ParentWorkspaceId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vParentWorkSpaceId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vParentWorkSpaceId);
                ht.Add(Utilities.clsParameters.ActivityId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vActivityId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vActivityId);
                ht.Add(Utilities.clsParameters.iNodeId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.iNodeId) ? "" : _Save_CRFHdrDtlSubDtlDTO.iNodeId);
                ht.Add(Utilities.clsParameters.PeriodId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vPeriodId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vPeriodId);
                ht.Add(Utilities.clsParameters.MySubjectNo, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.iMySubjectNo) ? "" : _Save_CRFHdrDtlSubDtlDTO.iMySubjectNo);
                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams("Proc_GetCRFTempDetail", ht);

                if (Sql_Ds != null && Sql_Ds.Tables.Count > 0)
                {
                    DataTable dt_TOP_CRFHdrDtlSubDtl = Sql_Ds.Tables[0];
                    DataTable dtNoOfRepetation = Sql_Ds.Tables[1];

                    if (dt_TOP_CRFHdrDtlSubDtl != null && dt_TOP_CRFHdrDtlSubDtl.Rows.Count != 0)
                    {
                        bool save = false;
                        string result = string.Empty;

                        _SqlHelper.Open_Connection();
                        _SqlHelper.Begin_Transaction();

                        try
                        {
                            foreach (DataRow dr_NoOfRepetation in dtNoOfRepetation.Rows)
                            {
                                int? iRepetationNo = Convert.ToInt32(dr_NoOfRepetation["iRepetationNo"]);

                                DataView dv = new DataView(dt_TOP_CRFHdrDtlSubDtl);
                                dv.RowFilter = "iRepetationNo=" + iRepetationNo + "";
                                DataTable dtCRFHdrDtlSubDtl = dv.ToTable();
                                string ModifyBy = Convert.ToString(dtCRFHdrDtlSubDtl.Rows[0]["iModifyBy"]);

                                if (!AssignValues(SubjectId, WorkspaceId, ParentWorkspaceId, ActivityId, NodeId, PeriodId, MySubjectNo, ModifyBy, dtCRFHdrDtlSubDtl, ref dsResult))
                                {
                                    result = "Error Occured While Retrieving Procedure Info.";
                                    save = false;
                                    break;
                                }

                                //bool bol_CRFHdrDtlSubDtl = true;
                                if (_SqlHelper == null)
                                {
                                    result = "Data Object Is Invalid or Nothing";
                                    save = false;
                                    break;
                                }

                                if (_SqlHelper.Transaction == null)
                                {
                                    result = "Transaction object is not started";
                                    save = false;
                                    break;
                                }

                                string wstr = "vWorkspaceId='" + _Save_CRFHdrDtlSubDtlDTO.vWorkspaceId + "' And vSubjectId='" + _Save_CRFHdrDtlSubDtlDTO.vSubjectId + "' And iNodeId='" + _Save_CRFHdrDtlSubDtlDTO.iNodeId + "' And vactivityid = '" + _Save_CRFHdrDtlSubDtlDTO.vActivityId + "' and cStatusIndi <> 'D' ";

                                dt_CRFSubDtl = _SqlHelper.GetDataTable(Utilities.clsView.View_GetCRFSubDtl, wstr, Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);

                                if (dt_CRFSubDtl.Rows.Count != 0)
                                {
                                    str_ReturnValue1 = "";
                                    str_ReturnValue2 = "";
                                    DataReturnList = null;
                                    DataReturnList = new ArrayList();
                                    DataTable Tbl_DtCRFHdr = dsResult.Tables["CRFHdr"];
                                    DataTable Tbl_DtCRFDtl = dsResult.Tables["CRFDtl"];
                                    DataTable Tbl_DtCRFSubDtl = dsResult.Tables["CRFSubDtl"];
                                    DataTable Tbl_DtDCFMst = dsResult.Tables["DCFMst"];

                                    //str_ReturnValue1 = (string)DataReturnList[0];
                                    foreach (DataRow dr in Tbl_DtCRFSubDtl.Rows)
                                    {
                                        dr["nCRFDtlNo"] = Convert.ToInt32(dt_CRFSubDtl.Rows[0]["nCRFDtlNo"]);
                                        dr["iTranNo"] = "1";
                                        dr.AcceptChanges();
                                    }
                                    Tbl_DtCRFSubDtl.AcceptChanges();

                                    str_ReturnValue1 = "";
                                    str_ReturnValue2 = "";
                                    DataReturnList = null;
                                    DataReturnList = new ArrayList();
                                    //Now saving Remaining Rows with same iTranNo
                                    if (!_SqlHelper.SaveInDbDataTable("Insert_CRFSubDtl", Tbl_DtCRFSubDtl, 1, ref DataReturnList, ref sResult))
                                    {
                                        result = "Error occured while Save Information of CRFSubDtl In Database";
                                        save = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    str_ReturnValue1 = "";
                                    str_ReturnValue2 = "";
                                    DataReturnList = null;
                                    DataReturnList = new ArrayList();
                                    DataTable Tbl_DtCRFHdr = dsResult.Tables["CRFHdr"];
                                    DataTable Tbl_DtCRFDtl = dsResult.Tables["CRFDtl"];
                                    DataTable Tbl_DtCRFSubDtl = dsResult.Tables["CRFSubDtl"];
                                    DataTable Tbl_DtDCFMst = dsResult.Tables["DCFMst"];
                                    if (!_SqlHelper.SaveInDbDataTable("Insert_CRFHdr", Tbl_DtCRFHdr, 1, ref DataReturnList, ref sResult))
                                    {
                                        result = "Error occured while Save Information of CRFHdr In Database";
                                        save = false;
                                        break;
                                    }

                                    str_ReturnValue1 = (string)DataReturnList[0];
                                    foreach (DataRow dr in Tbl_DtCRFDtl.Rows)
                                    {
                                        dr["nCRFHdrNo"] = Convert.ToInt32(str_ReturnValue1);
                                        dr.AcceptChanges();
                                    }
                                    Tbl_DtCRFDtl.AcceptChanges();

                                    DataTable Tbl_Top_CRFDtl = Tbl_DtCRFDtl.Copy();
                                    foreach (DataRow dr in Tbl_Top_CRFDtl.Rows)
                                    {
                                        dr["iRepeatNo"] = 0;
                                        dr.AcceptChanges();
                                    }
                                    Tbl_Top_CRFDtl.AcceptChanges();

                                    str_ReturnValue1 = "";
                                    str_ReturnValue2 = "";
                                    DataReturnList = null;
                                    DataReturnList = new ArrayList();
                                    if (!_SqlHelper.SaveInDbDataTable("Insert_CRFDtl", Tbl_Top_CRFDtl, 1, ref DataReturnList, ref sResult))
                                    {
                                        result = "Error occured while Save Information of CRFDtl In Database";
                                        save = false;
                                        break;
                                    }
                                    Tbl_DtCRFDtl.Rows[0].Delete();
                                    Tbl_DtCRFDtl.AcceptChanges();

                                    str_ReturnValue1 = (string)DataReturnList[0];
                                    str_ReturnValue2 = (string)DataReturnList[1];

                                    foreach (DataRow dr in Tbl_DtCRFDtl.Rows)
                                    {
                                        dr["nCRFDtlNo"] = Convert.ToInt32(str_ReturnValue1);
                                        dr["iRepeatNo"] = Convert.ToInt32(str_ReturnValue2);
                                        dr.AcceptChanges();
                                    }
                                    Tbl_DtCRFDtl.AcceptChanges();

                                    foreach (DataRow dr in Tbl_Top_CRFDtl.Rows)
                                    {
                                        dr["nCRFDtlNo"] = Convert.ToInt32(str_ReturnValue1);
                                        dr["iRepeatNo"] = Convert.ToInt32(str_ReturnValue2);
                                        dr.AcceptChanges();
                                    }
                                    Tbl_Top_CRFDtl.AcceptChanges();

                                    foreach (DataRow dr in Tbl_DtCRFSubDtl.Rows)
                                    {
                                        dr["nCRFDtlNo"] = Convert.ToInt32(str_ReturnValue1);
                                        dr.AcceptChanges();
                                    }
                                    Tbl_DtCRFSubDtl.AcceptChanges();

                                    DataTable Tbl_Top_CRFSubDtl = Tbl_DtCRFSubDtl.Copy();
                                    for (int i = Tbl_Top_CRFSubDtl.Rows.Count - 1; i >= 1; i += -1)
                                    {
                                        Tbl_Top_CRFSubDtl.Rows[i].Delete();
                                        Tbl_Top_CRFSubDtl.AcceptChanges();
                                    }
                                    foreach (DataRow dr in Tbl_Top_CRFSubDtl.Rows)
                                    {
                                        dr["iTranNo"] = 0;
                                        dr.AcceptChanges();
                                    }
                                    Tbl_Top_CRFSubDtl.AcceptChanges();

                                    str_ReturnValue1 = "";
                                    str_ReturnValue2 = "";
                                    DataReturnList = null;
                                    DataReturnList = new ArrayList();
                                    if (!_SqlHelper.SaveInDbDataTable("Insert_CRFSubDtl", Tbl_Top_CRFSubDtl, 1, ref DataReturnList, ref sResult))
                                    {
                                        result = "Error occured while Save Information of CRFSubDtl In Database";
                                        save = false;
                                        break;
                                    }
                                    Tbl_DtCRFSubDtl.Rows[0].Delete();
                                    Tbl_DtCRFSubDtl.AcceptChanges();

                                    str_ReturnValue1 = (string)DataReturnList[0];
                                    foreach (DataRow dr in Tbl_DtCRFSubDtl.Rows)
                                    {
                                        dr["iTranNo"] = Convert.ToInt32(str_ReturnValue1);
                                        dr.AcceptChanges();
                                    }
                                    Tbl_DtCRFSubDtl.AcceptChanges();

                                    str_ReturnValue1 = "";
                                    str_ReturnValue2 = "";
                                    DataReturnList = null;
                                    DataReturnList = new ArrayList();
                                    //Now saving Remaining Rows with same iTranNo
                                    if (!_SqlHelper.SaveInDbDataTable("Insert_CRFSubDtl", Tbl_DtCRFSubDtl, 1, ref DataReturnList, ref sResult))
                                    {
                                        result = "Error occured while Save Information of CRFSubDtl In Database";
                                        save = false;
                                        break;
                                    }

                                    foreach (DataRow dr in Tbl_Top_CRFDtl.Rows)
                                    {
                                        string strcDataStatus = Convert.ToString(dr["cDataStatus"]);
                                        if (strcDataStatus == "C")
                                        {
                                            dr["cDataStatus"] = "D";
                                        }
                                    }
                                    Tbl_Top_CRFDtl.AcceptChanges();

                                    str_ReturnValue1 = "";
                                    str_ReturnValue2 = "";
                                    DataReturnList = null;
                                    DataReturnList = new ArrayList();
                                    if (!_SqlHelper.SaveInDbDataTable("Insert_CRFDtl", Tbl_Top_CRFDtl, 1, ref DataReturnList, ref sResult))
                                    {
                                        result = "Error occured while Save Information of CRFDtl In Database";
                                        save = false;
                                        break;
                                    }
                                }

                                if (iRepetationNo != null)
                                {
                                    ht.Clear();
                                    ht.Add(Utilities.clsParameters.vSubjectId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vSubjectId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vSubjectId);
                                    ht.Add(Utilities.clsParameters.vWorkspaceId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vWorkspaceId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vWorkspaceId);
                                    ht.Add(Utilities.clsParameters.ParentWorkspaceId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vParentWorkSpaceId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vParentWorkSpaceId);
                                    ht.Add(Utilities.clsParameters.ActivityId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vActivityId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vActivityId);
                                    ht.Add(Utilities.clsParameters.iNodeId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.iNodeId) ? "" : _Save_CRFHdrDtlSubDtlDTO.iNodeId);
                                    ht.Add(Utilities.clsParameters.PeriodId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vPeriodId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vPeriodId);
                                    ht.Add(Utilities.clsParameters.MySubjectNo, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.iMySubjectNo) ? "" : _Save_CRFHdrDtlSubDtlDTO.iMySubjectNo);
                                    ht.Add(Utilities.clsParameters.RepetationNo, iRepetationNo);

                                    int resultdata = _SqlHelper.ExecuteSP("Proc_RemoveCRFTempDetail", ht);
                                    if (resultdata != -1)
                                    {
                                        result = "Error occured while Save Information of CRFDtl In Database";
                                        save = false;
                                        break;
                                    }
                                }
                                result = "success";
                                save = true;
                            }

                            //for mi image save
                            if (save == true)
                            {
                                sResult = "success";
                                _SqlHelper.Commit_Transaction();
                            }
                            else
                            {
                                sResult = result;
                                _SqlHelper.RollBack_Transaction();
                            }
                        }
                        catch (Exception cx)
                        {
                            sResult = cx.Message + " Error occured while saving CRFHdrDtlSubDtl";
                            _SqlHelper.RollBack_Transaction();
                        }
                        finally
                        {
                            _SqlHelper.Close_Connection();
                        }
                    }
                    else
                    {
                        sResult = "RecordsNotFound";
                    }
                }
                else
                {
                    sResult = "RecordsNotFound";
                }
            }
            catch (Exception ex)
            {
                sResult = "Error " + ex.Message;
                return sResult;
            }
            finally
            {
                //_SqlHelper.Close_Connection();
                //_SqlHelper.Close_ConnectionBiz();
            }
            return sResult;
        }

        private bool AssignValues(string SubjectId, string WorkspaceId, string ParentWorkspaceId, string ActivityId, string NodeId, string PeriodId, string MySubjectNo, string ModifyBy, DataTable dtCRFHdrDtlSubDtl, ref DataSet DsSave)
        {
            try
            {
                DataSet DsCRFHdr = new DataSet();
                DataSet DsCRFDtl = new DataSet();
                DataTable DtCRFHdr = new DataTable();
                DataTable DtCRFDtl = new DataTable();
                DataTable DtCRFSubDtl = new DataTable();
                DataTable DtDCFMst = new DataTable();

                string Wstr = string.Empty;
                string estr = string.Empty;
                string NodeIndex = string.Empty;
                DataSet dsNodeInfo = new DataSet();

                Wstr = "Where vWorkSpaceId='" + ParentWorkspaceId + "' and iNodeId='" + NodeId + "'" + " and vActivityId='" + ActivityId + "'";
                DataTable dsNode = _SqlHelper.GetDataTableWorkSpacenodedetail("WorkSpacenodedetail", Wstr);
                NodeIndex = Convert.ToString(dsNode.Rows[0]["iNodeIndex"]);

                Wstr = "Where 1=2";
                DtDCFMst = _SqlHelper.GetDataTableWorkSpacenodedetail("DCFMst", Wstr);
                DtCRFHdr.Columns.Add("nCRFHdrNo", typeof(int));
                DtCRFHdr.Columns.Add("vWorkSpaceId", typeof(string));
                DtCRFHdr.Columns.Add("dStartDate", typeof(DateTime));
                DtCRFHdr.Columns.Add("iPeriod", typeof(int));
                DtCRFHdr.Columns.Add("iNodeId", typeof(int));
                DtCRFHdr.Columns.Add("iNodeIndex", typeof(int));
                DtCRFHdr.Columns.Add("vActivityId", typeof(string));
                DtCRFHdr.Columns.Add("cLockStatus", typeof(string));
                DtCRFHdr.Columns.Add("iModifyBy", typeof(int));
                DtCRFHdr.Columns.Add("cStatusIndi", typeof(string));
                DtCRFHdr.Columns.Add("dModifyOn", typeof(DateTime));

                DataRow drCRFHdr = DtCRFHdr.NewRow();
                drCRFHdr["nCRFHdrNo"] = 1;
                drCRFHdr["vWorkSpaceId"] = WorkspaceId;
                drCRFHdr["dStartDate"] = DateTime.Now;
                drCRFHdr["iPeriod"] = PeriodId;
                drCRFHdr["iNodeId"] = NodeId;
                drCRFHdr["iNodeIndex"] = NodeIndex;
                drCRFHdr["vActivityId"] = ActivityId;
                drCRFHdr["cLockStatus"] = "U";//'cLockStatus
                drCRFHdr["iModifyBy"] = Convert.ToInt32(ModifyBy);
                drCRFHdr["cStatusIndi"] = "N";
                drCRFHdr["dModifyOn"] = DateTime.Now;
                DtCRFHdr.Rows.Add(drCRFHdr);
                DtCRFHdr.TableName = "CRFHdr";
                DtCRFHdr.AcceptChanges();

                DtCRFDtl.Columns.Add("nCRFDtlNo", typeof(int));
                DtCRFDtl.Columns.Add("nCRFHdrNo", typeof(int));
                DtCRFDtl.Columns.Add("iRepeatNo", typeof(int));
                DtCRFDtl.Columns.Add("dRepeatationDate", typeof(DateTime));
                DtCRFDtl.Columns.Add("vSubjectId", typeof(string));
                DtCRFDtl.Columns.Add("iMySubjectNo", typeof(int));
                DtCRFDtl.Columns.Add("cLockStatus", typeof(string));
                DtCRFDtl.Columns.Add("iWorkFlowstageId", typeof(int));
                DtCRFDtl.Columns.Add("iModifyBy", typeof(int));
                DtCRFDtl.Columns.Add("cStatusIndi", typeof(string));
                DtCRFDtl.Columns.Add("dModifyOn", typeof(DateTime));
                DtCRFDtl.Columns.Add("cDataStatus", typeof(string));

                DataRow drCRFDtl = DtCRFDtl.NewRow();
                drCRFDtl["nCRFDtlNo"] = 1;
                drCRFDtl["nCRFHdrNo"] = 1;
                drCRFDtl["iRepeatNo"] = 1;
                drCRFDtl["dRepeatationDate"] = DateTime.Now;
                drCRFDtl["vSubjectId"] = SubjectId;
                drCRFDtl["iMySubjectNo"] = Convert.ToInt32(MySubjectNo);
                drCRFDtl["cLockStatus"] = "U";
                drCRFDtl["iWorkFlowstageId"] = 0;
                drCRFDtl["iModifyBy"] = Convert.ToInt32(ModifyBy);
                drCRFDtl["cStatusIndi"] = "N";
                drCRFDtl["dModifyOn"] = DateTime.Now;
                drCRFDtl["cDataStatus"] = "C";
                DtCRFDtl.Rows.Add(drCRFDtl);
                DtCRFDtl.TableName = "CRFDtl";
                DtCRFDtl.AcceptChanges();

                DtCRFSubDtl.Columns.Add("nCRFSubDtlNo", typeof(int));
                DtCRFSubDtl.Columns.Add("nCRFDtlNo", typeof(int));
                DtCRFSubDtl.Columns.Add("iTranNo", typeof(int));
                DtCRFSubDtl.Columns.Add("vMedExCode", typeof(string));
                DtCRFSubDtl.Columns.Add("vMedExDesc", typeof(string));
                DtCRFSubDtl.Columns.Add("dMedExDateTime", typeof(DateTime));
                DtCRFSubDtl.Columns.Add("vMedexResult", typeof(string));
                DtCRFSubDtl.Columns.Add("vModificationRemark", typeof(string));
                DtCRFSubDtl.Columns.Add("iModifyBy", typeof(int));
                DtCRFSubDtl.Columns.Add("cStatusIndi", typeof(string));
                DtCRFSubDtl.Columns.Add("dModifyOn", typeof(DateTime));
                DtCRFSubDtl.Columns.Add("cHL", typeof(string));
                DtCRFSubDtl.Columns.Add("vHighRange", typeof(string));
                DtCRFSubDtl.Columns.Add("vLowRange", typeof(string));
                DataRow drDtCRFSubDtl;
                int TranNo = 0;
                foreach (DataRow dr in dtCRFHdrDtlSubDtl.Rows)
                {
                    TranNo++;
                    drDtCRFSubDtl = DtCRFSubDtl.NewRow();

                    drDtCRFSubDtl["nCRFSubDtlNo"] = TranNo;
                    drDtCRFSubDtl["nCRFDtlNo"] = 1;
                    drDtCRFSubDtl["iTranNo"] = TranNo;
                    drDtCRFSubDtl["vMedExDesc"] = Convert.ToString(dr["vMedExDesc"]);
                    if (string.IsNullOrEmpty(Convert.ToString(dr["vModificationRemark"])))
                    {
                        drDtCRFSubDtl["vModificationRemark"] = "";
                    }
                    else
                    {
                        drDtCRFSubDtl["vModificationRemark"] = Convert.ToString(dr["vModificationRemark"]);
                    }
                    drDtCRFSubDtl["iModifyBy"] = Convert.ToInt32(ModifyBy);
                    drDtCRFSubDtl["cStatusIndi"] = "N";
                    drDtCRFSubDtl["dMedExDateTime"] = Convert.ToDateTime(dr["dMedExDateTime"]);
                    drDtCRFSubDtl["vMedExCode"] = Convert.ToString(dr["vMedExCode"]);
                    drDtCRFSubDtl["vMedexResult"] = Convert.ToString(dr["vMedexResult"]);
                    drDtCRFSubDtl["dModifyOn"] = DateTime.Now;

                    DtCRFSubDtl.Rows.Add(drDtCRFSubDtl);
                    DtCRFSubDtl.AcceptChanges();
                }
                DtCRFSubDtl.TableName = "CRFSubDtl";
                DtCRFSubDtl.AcceptChanges();

                DsSave = null;
                DsSave = new DataSet();
                DsSave.Tables.Add(DtCRFHdr.Copy());
                DsSave.Tables.Add(DtCRFDtl.Copy());
                DsSave.Tables.Add(DtCRFSubDtl.Copy());
                DsSave.AcceptChanges();

                DtDCFMst.TableName = "DCFMst";
                DtDCFMst.AcceptChanges();
                DsSave.Tables.Add(DtDCFMst.Copy());
                DsSave.AcceptChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        public string SubmitMIFinalLesion(MIFinalLesionDataDTO _MIFinalLesionDataDTO)
        {
            string result = string.Empty;
            try
            {   //DTO that is used to save data in Save_CRFHdrDtlSubDtl
                Save_CRFHdrDtlSubDtlDTO _Save_CRFHdrDtlSubDtlDTO = new Save_CRFHdrDtlSubDtlDTO();
                _Save_CRFHdrDtlSubDtlDTO.vParentWorkSpaceId = _MIFinalLesionDataDTO.vParentWorkSpaceId;
                _Save_CRFHdrDtlSubDtlDTO.vWorkspaceId = _MIFinalLesionDataDTO.vWorkspaceId;
                _Save_CRFHdrDtlSubDtlDTO.vSubjectId = _MIFinalLesionDataDTO.vSubjectId;
                _Save_CRFHdrDtlSubDtlDTO.iMySubjectNo = _MIFinalLesionDataDTO.iMySubjectNo;
                _Save_CRFHdrDtlSubDtlDTO.vActivityId = _MIFinalLesionDataDTO.vActivityId;
                _Save_CRFHdrDtlSubDtlDTO.vPeriodId = _MIFinalLesionDataDTO.vPeriodId;
                _Save_CRFHdrDtlSubDtlDTO.iNodeId = _MIFinalLesionDataDTO.iNodeId;

                result = Save_CRFHdrDtlSubDtl(_Save_CRFHdrDtlSubDtlDTO, _MIFinalLesionDataDTO);
                //result = Save_CRFHdrDtlSubDtl(_Save_CRFHdrDtlSubDtlDTO);
            }
            catch (Exception e) { }
            finally { }
            return result;
        }

        public string SkipMIFinalLesion(MIFinalSkipLesionDataDTO _MIFinalSkipLesionDataDTO)
        {
            string result = string.Empty;
            try
            {   //DTO that is used to save data in Save_CRFHdrDtlSubDtl
                Save_CRFHdrDtlSubDtlDTO _Save_CRFHdrDtlSubDtlDTO = new Save_CRFHdrDtlSubDtlDTO();
                _Save_CRFHdrDtlSubDtlDTO.vParentWorkSpaceId = _MIFinalSkipLesionDataDTO.vParentWorkSpaceId;
                _Save_CRFHdrDtlSubDtlDTO.vWorkspaceId = _MIFinalSkipLesionDataDTO.vWorkspaceId;
                _Save_CRFHdrDtlSubDtlDTO.vSubjectId = _MIFinalSkipLesionDataDTO.vSubjectId;
                _Save_CRFHdrDtlSubDtlDTO.iMySubjectNo = _MIFinalSkipLesionDataDTO.iMySubjectNo;
                _Save_CRFHdrDtlSubDtlDTO.vActivityId = _MIFinalSkipLesionDataDTO.vActivityId;
                _Save_CRFHdrDtlSubDtlDTO.vPeriodId = _MIFinalSkipLesionDataDTO.vPeriodId;
                _Save_CRFHdrDtlSubDtlDTO.iNodeId = _MIFinalSkipLesionDataDTO.iNodeId;

                result = Skip_CRFHdrDtlSubDtl(_Save_CRFHdrDtlSubDtlDTO);
                //result = Save_CRFHdrDtlSubDtl(_Save_CRFHdrDtlSubDtlDTO);
            }
            catch (Exception e) { }
            finally { }
            return result;
        }

        //MI PHASE 2 DEVELOPEMENT

        public string SubmitMIFinalLesionData(MIFinalLesionDetailsDataDTO _MIFinalLesionDetailsDataDTO)
        {
            string result = string.Empty;
            try
            {
                string Wstr = string.Empty;
                string vWorkSpaceId = Convert.ToString(_MIFinalLesionDetailsDataDTO.vWorkspaceId);
                string vActivityId = Convert.ToString(_MIFinalLesionDetailsDataDTO.vActivityId);
                string iNodeId = Convert.ToString(_MIFinalLesionDetailsDataDTO.iNodeId);
                string vSubjectId = Convert.ToString(_MIFinalLesionDetailsDataDTO.vSubjectId);
                string vPeriodId = Convert.ToString(_MIFinalLesionDetailsDataDTO.vPeriodId);
                //Wstr = "vWorkSpaceId='" + vWorkSpaceId + "' and vActivityId='" + vActivityId + "'" + " and iNodeId='" + iNodeId + "' and vSubjectId = '" + vSubjectId + "' and vPeriodId = '" + vPeriodId + "'";
                //Wstr = "vWorkSpaceId='" + vWorkSpaceId + "' and vActivityId='" + vActivityId + "'" + " and iNodeId='" + iNodeId + "' and vSubjectId = '" + vSubjectId + "' and vPeriodId = '" + vPeriodId + "' and cSaveStatus = 'N' and cStatusIndi <> 'D'";
                if (_MIFinalLesionDetailsDataDTO.cSaveStatusFlagValidation == "N")
                {
                    Wstr = "vWorkSpaceId='" + vWorkSpaceId + "' and vActivityId='" + vActivityId + "'" + " and iNodeId='" + iNodeId + "' and vSubjectId = '" + vSubjectId + "' and vPeriodId = '" + vPeriodId + "' and cStatusIndi <> 'D'";
                }
                else
                {
                    Wstr = "vWorkSpaceId='" + vWorkSpaceId + "' and vActivityId='" + vActivityId + "'" + " and iNodeId='" + iNodeId + "' and vSubjectId = '" + vSubjectId + "' and vPeriodId = '" + vPeriodId + "' and cSaveStatus = 'N' and cStatusIndi <> 'D'";
                }

                //Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsView.View_GetSubjectImageStudyDetail, "iImgTransmittalHdrId = '" + _MIImageReviewDTO.iImgTransmittalHdrId + "' AND iImgTransmittalDtlId='" + _MIImageReviewDTO.iImgTransmittalDtlId + "'AND iImageStatus = '" + _MIImageReviewDTO.iImageStatus + "' AND cStatusIndi <> 'D' order by iImgTransmittalHdrId", Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                Sql_Dt = _SqlHelper.GetDataTable("CRFTempDetail", Wstr, Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);

                if (Sql_Dt.Rows.Count == 0)
                {
                    result = "NO_LESION_DETAIL_FOUND";
                    return result;
                }

                //DTO that is used to save data in Save_CRFHdrDtlSubDtl
                Save_CRFHdrDtlSubDtlDTO _Save_CRFHdrDtlSubDtlDTO = new Save_CRFHdrDtlSubDtlDTO();
                _Save_CRFHdrDtlSubDtlDTO.vParentWorkSpaceId = _MIFinalLesionDetailsDataDTO.vParentWorkSpaceId;
                _Save_CRFHdrDtlSubDtlDTO.vWorkspaceId = _MIFinalLesionDetailsDataDTO.vWorkspaceId;
                _Save_CRFHdrDtlSubDtlDTO.vSubjectId = _MIFinalLesionDetailsDataDTO.vSubjectId;
                _Save_CRFHdrDtlSubDtlDTO.iMySubjectNo = _MIFinalLesionDetailsDataDTO.iMySubjectNo;
                _Save_CRFHdrDtlSubDtlDTO.vActivityId = _MIFinalLesionDetailsDataDTO.vActivityId;
                _Save_CRFHdrDtlSubDtlDTO.vPeriodId = _MIFinalLesionDetailsDataDTO.vPeriodId;
                _Save_CRFHdrDtlSubDtlDTO.iNodeId = _MIFinalLesionDetailsDataDTO.iNodeId;

                result = Save_CRFHdrDtlSubDtl(_Save_CRFHdrDtlSubDtlDTO, _MIFinalLesionDetailsDataDTO, _MIFinalLesionDetailsDataDTO.cSaveStatusFlagValidation);
                //result = Save_CRFHdrDtlSubDtl(_Save_CRFHdrDtlSubDtlDTO);
                return result;
            }
            catch (Exception e)
            {
                result = "Error : " + e.InnerException;
                return result;
            }
            finally { }
        }

        public string Save_CRFHdrDtlSubDtl(Save_CRFHdrDtlSubDtlDTO _Save_CRFHdrDtlSubDtlDTO, MIFinalLesionDetailsDataDTO _MIFinalLesionDetailsDataDTO, string cSaveStatusFlagValidation)
        {
            string sResult = "";
            try
            {
                string str_ReturnValue1 = "";
                string str_ReturnValue2 = "";
                ArrayList DataReturnList = new ArrayList();
                DataSet dsResult = new DataSet();
                DataTable dtDicomAnnotation = new DataTable();

                //DataTable dtCRFHdrDtlSubDtl = Utilities.ToDataTable(_Save_CRFHdrDtlSubDtlDTO._Save_CRFHdrDtlSubDtlList);
                string SubjectId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vSubjectId);
                string WorkspaceId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vWorkspaceId);
                string ParentWorkspaceId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vParentWorkSpaceId);
                string ActivityId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vActivityId);
                string NodeId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.iNodeId);
                string PeriodId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vPeriodId);
                string MySubjectNo = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.iMySubjectNo);

                ht.Clear();
                ht.Add(Utilities.clsParameters.vSubjectId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vSubjectId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vSubjectId);
                ht.Add(Utilities.clsParameters.vWorkspaceId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vWorkspaceId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vWorkspaceId);
                ht.Add(Utilities.clsParameters.ParentWorkspaceId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vParentWorkSpaceId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vParentWorkSpaceId);
                ht.Add(Utilities.clsParameters.ActivityId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vActivityId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vActivityId);
                ht.Add(Utilities.clsParameters.iNodeId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.iNodeId) ? "" : _Save_CRFHdrDtlSubDtlDTO.iNodeId);
                ht.Add(Utilities.clsParameters.PeriodId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vPeriodId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vPeriodId);
                ht.Add(Utilities.clsParameters.MySubjectNo, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.iMySubjectNo) ? "" : _Save_CRFHdrDtlSubDtlDTO.iMySubjectNo);

                dtDicomAnnotation = Utilities.ToDataTable(_MIFinalLesionDetailsDataDTO.DicomAnnotationDtl);

                try
                {
                    Sql_Ds = _SqlHelper.ExecuteProcudereWithParams("Proc_GetCRFTempDetail", ht);
                }
                catch (Exception ex)
                {
                    sResult = ex.Message + " Error occured while Retriving Value from Proc_GetCRFTempDetail";
                    _SqlHelper.RollBack_Transaction();
                }

                if (Sql_Ds != null && Sql_Ds.Tables.Count > 0)
                {
                    DataTable dt_TOP_CRFHdrDtlSubDtl = Sql_Ds.Tables[0];
                    DataTable dtNoOfRepetation = Sql_Ds.Tables[1];

                    if (dt_TOP_CRFHdrDtlSubDtl != null && dt_TOP_CRFHdrDtlSubDtl.Rows.Count != 0)
                    {
                        bool save = false;
                        string result = string.Empty;

                        _SqlHelper.Open_Connection();
                        _SqlHelper.Begin_Transaction();

                        try
                        {
                            foreach (DataRow dr_NoOfRepetation in dtNoOfRepetation.Rows)
                            {
                                int? iRepetationNo = Convert.ToInt32(dr_NoOfRepetation["iRepetationNo"]);

                                DataView dv = new DataView(dt_TOP_CRFHdrDtlSubDtl);
                                dv.RowFilter = "iRepetationNo=" + iRepetationNo + "";
                                DataTable dtCRFHdrDtlSubDtl = dv.ToTable();
                                string ModifyBy = Convert.ToString(dtCRFHdrDtlSubDtl.Rows[0]["iModifyBy"]);
                                try
                                {
                                    if (!AssignValues(SubjectId, WorkspaceId, ParentWorkspaceId, ActivityId, NodeId, PeriodId, MySubjectNo, ModifyBy, dtCRFHdrDtlSubDtl, ref dsResult))
                                    {
                                        result = "Error Occured While Retrieving Procedure Info.";
                                        save = false;
                                        break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    sResult = ex.Message + " Error occured while Assigning value";
                                    _SqlHelper.RollBack_Transaction();
                                }

                                if (_SqlHelper == null)
                                {
                                    result = "Data Object Is Invalid or Nothing";
                                    save = false;
                                    break;
                                }

                                if (_SqlHelper.Transaction == null)
                                {
                                    result = "Transaction object is not started";
                                    save = false;
                                    break;
                                }

                                string wstr = "vWorkspaceId='" + _Save_CRFHdrDtlSubDtlDTO.vWorkspaceId + "' And vSubjectId='" + _Save_CRFHdrDtlSubDtlDTO.vSubjectId + "' And iNodeId='" + _Save_CRFHdrDtlSubDtlDTO.iNodeId + "' And vactivityid = '" + _Save_CRFHdrDtlSubDtlDTO.vActivityId + "' and cStatusIndi <> 'D' ";
                                try
                                {
                                    dt_CRFSubDtl = _SqlHelper.GetDataTable(Utilities.clsView.View_GetCRFSubDtl, wstr, Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                                }
                                catch (Exception ex)
                                {
                                    sResult = ex.Message + " Error occured while Retriving dt_CRFSubDtl Detail";
                                    _SqlHelper.RollBack_Transaction();
                                }

                                if (dt_CRFSubDtl.Rows.Count != 0)
                                {
                                    str_ReturnValue1 = "";
                                    str_ReturnValue2 = "";
                                    DataReturnList = null;
                                    DataReturnList = new ArrayList();
                                    DataTable Tbl_DtCRFHdr = dsResult.Tables["CRFHdr"];
                                    DataTable Tbl_DtCRFDtl = dsResult.Tables["CRFDtl"];
                                    DataTable Tbl_DtCRFSubDtl = dsResult.Tables["CRFSubDtl"];
                                    DataTable Tbl_DtDCFMst = dsResult.Tables["DCFMst"];

                                    foreach (DataRow dr in Tbl_DtCRFSubDtl.Rows)
                                    {
                                        dr["nCRFDtlNo"] = Convert.ToInt32(dt_CRFSubDtl.Rows[0]["nCRFDtlNo"]);
                                        dr["iTranNo"] = "1";
                                        dr.AcceptChanges();
                                    }
                                    Tbl_DtCRFSubDtl.AcceptChanges();

                                    str_ReturnValue1 = "";
                                    str_ReturnValue2 = "";
                                    DataReturnList = null;
                                    DataReturnList = new ArrayList();
                                    //Now saving Remaining Rows with same iTranNo
                                    try
                                    {
                                        if (!_SqlHelper.SaveInDbDataTable("Insert_CRFSubDtl", Tbl_DtCRFSubDtl, 1, ref DataReturnList, ref sResult))
                                        {
                                            result = "Error occured while Save Information of CRFSubDtl In Database";
                                            save = false;
                                            break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        sResult = ex.Message + " Error occured while saving Insert_CRFSubDtl For Remaining Rows With Same ITranNo for Updation";
                                        _SqlHelper.RollBack_Transaction();
                                    }
                                }
                                else
                                {
                                    str_ReturnValue1 = "";
                                    str_ReturnValue2 = "";
                                    DataReturnList = null;
                                    DataReturnList = new ArrayList();
                                    DataTable Tbl_DtCRFHdr = dsResult.Tables["CRFHdr"];
                                    DataTable Tbl_DtCRFDtl = dsResult.Tables["CRFDtl"];
                                    DataTable Tbl_DtCRFSubDtl = dsResult.Tables["CRFSubDtl"];
                                    DataTable Tbl_DtDCFMst = dsResult.Tables["DCFMst"];
                                    try
                                    {
                                        if (!_SqlHelper.SaveInDbDataTable("Insert_CRFHdr", Tbl_DtCRFHdr, 1, ref DataReturnList, ref sResult))
                                        {
                                            result = "Error occured while Save Information of CRFHdr In Database";
                                            save = false;
                                            break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        sResult = ex.Message + " Error occured while saving Insert_CRFHdr";
                                        _SqlHelper.RollBack_Transaction();
                                    }

                                    str_ReturnValue1 = (string)DataReturnList[0];
                                    foreach (DataRow dr in Tbl_DtCRFDtl.Rows)
                                    {
                                        dr["nCRFHdrNo"] = Convert.ToInt32(str_ReturnValue1);
                                        dr.AcceptChanges();
                                    }
                                    Tbl_DtCRFDtl.AcceptChanges();

                                    DataTable Tbl_Top_CRFDtl = Tbl_DtCRFDtl.Copy();
                                    foreach (DataRow dr in Tbl_Top_CRFDtl.Rows)
                                    {
                                        dr["iRepeatNo"] = 0;
                                        dr.AcceptChanges();
                                    }
                                    Tbl_Top_CRFDtl.AcceptChanges();

                                    str_ReturnValue1 = "";
                                    str_ReturnValue2 = "";
                                    DataReturnList = null;
                                    DataReturnList = new ArrayList();
                                    try
                                    {
                                        if (!_SqlHelper.SaveInDbDataTable("Insert_CRFDtl", Tbl_Top_CRFDtl, 1, ref DataReturnList, ref sResult))
                                        {
                                            result = "Error occured while Save Information of CRFDtl In Database";
                                            save = false;
                                            break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        sResult = ex.Message + " Error occured while saving Insert_CRFDtl";
                                        _SqlHelper.RollBack_Transaction();
                                    }

                                    Tbl_DtCRFDtl.Rows[0].Delete();
                                    Tbl_DtCRFDtl.AcceptChanges();

                                    str_ReturnValue1 = (string)DataReturnList[0];
                                    str_ReturnValue2 = (string)DataReturnList[1];

                                    foreach (DataRow dr in Tbl_DtCRFDtl.Rows)
                                    {
                                        dr["nCRFDtlNo"] = Convert.ToInt32(str_ReturnValue1);
                                        dr["iRepeatNo"] = Convert.ToInt32(str_ReturnValue2);
                                        dr.AcceptChanges();
                                    }
                                    Tbl_DtCRFDtl.AcceptChanges();

                                    foreach (DataRow dr in Tbl_Top_CRFDtl.Rows)
                                    {
                                        dr["nCRFDtlNo"] = Convert.ToInt32(str_ReturnValue1);
                                        dr["iRepeatNo"] = Convert.ToInt32(str_ReturnValue2);
                                        dr.AcceptChanges();
                                    }
                                    Tbl_Top_CRFDtl.AcceptChanges();

                                    foreach (DataRow dr in Tbl_DtCRFSubDtl.Rows)
                                    {
                                        dr["nCRFDtlNo"] = Convert.ToInt32(str_ReturnValue1);
                                        dr.AcceptChanges();
                                    }
                                    Tbl_DtCRFSubDtl.AcceptChanges();

                                    DataTable Tbl_Top_CRFSubDtl = Tbl_DtCRFSubDtl.Copy();
                                    for (int i = Tbl_Top_CRFSubDtl.Rows.Count - 1; i >= 1; i += -1)
                                    {
                                        Tbl_Top_CRFSubDtl.Rows[i].Delete();
                                        Tbl_Top_CRFSubDtl.AcceptChanges();
                                    }
                                    foreach (DataRow dr in Tbl_Top_CRFSubDtl.Rows)
                                    {
                                        dr["iTranNo"] = 0;
                                        dr.AcceptChanges();
                                    }
                                    Tbl_Top_CRFSubDtl.AcceptChanges();

                                    str_ReturnValue1 = "";
                                    str_ReturnValue2 = "";
                                    DataReturnList = null;
                                    DataReturnList = new ArrayList();
                                    try
                                    {
                                        if (!_SqlHelper.SaveInDbDataTable("Insert_CRFSubDtl", Tbl_Top_CRFSubDtl, 1, ref DataReturnList, ref sResult))
                                        {
                                            result = "Error occured while Save Information of CRFSubDtl In Database";
                                            save = false;
                                            break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        sResult = ex.Message + " Error occured while saving Insert_CRFSubDtl";
                                        _SqlHelper.RollBack_Transaction();
                                    }

                                    Tbl_DtCRFSubDtl.Rows[0].Delete();
                                    Tbl_DtCRFSubDtl.AcceptChanges();

                                    str_ReturnValue1 = (string)DataReturnList[0];
                                    foreach (DataRow dr in Tbl_DtCRFSubDtl.Rows)
                                    {
                                        dr["iTranNo"] = Convert.ToInt32(str_ReturnValue1);
                                        dr.AcceptChanges();
                                    }
                                    Tbl_DtCRFSubDtl.AcceptChanges();

                                    str_ReturnValue1 = "";
                                    str_ReturnValue2 = "";
                                    DataReturnList = null;
                                    DataReturnList = new ArrayList();
                                    //Now saving Remaining Rows with same iTranNo
                                    try
                                    {
                                        if (!_SqlHelper.SaveInDbDataTable("Insert_CRFSubDtl", Tbl_DtCRFSubDtl, 1, ref DataReturnList, ref sResult))
                                        {
                                            result = "Error occured while Save Information of CRFSubDtl In Database";
                                            save = false;
                                            break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        sResult = ex.Message + " Error occured while saving Insert_CRFSubDtl for Remaining Rows with same iTranNo";
                                        _SqlHelper.RollBack_Transaction();
                                    }

                                    foreach (DataRow dr in Tbl_Top_CRFDtl.Rows)
                                    {
                                        string strcDataStatus = Convert.ToString(dr["cDataStatus"]);
                                        if (strcDataStatus == "C")
                                        {
                                            dr["cDataStatus"] = "D";
                                        }
                                    }
                                    Tbl_Top_CRFDtl.AcceptChanges();

                                    str_ReturnValue1 = "";
                                    str_ReturnValue2 = "";
                                    DataReturnList = null;
                                    DataReturnList = new ArrayList();
                                    try
                                    {
                                        if (!_SqlHelper.SaveInDbDataTable("Insert_CRFDtl", Tbl_Top_CRFDtl, 1, ref DataReturnList, ref sResult))
                                        {
                                            result = "Error occured while Save Information of CRFDtl In Database";
                                            save = false;
                                            break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        sResult = ex.Message + " Error occured while saving Insert_CRFDtl for Remaining Rows with same iTranNo";
                                        _SqlHelper.RollBack_Transaction();
                                    }
                                }

                                if (cSaveStatusFlagValidation != "Y")
                                {
                                    if (iRepetationNo != null)
                                    {
                                        ht.Clear();
                                        ht.Add(Utilities.clsParameters.vSubjectId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vSubjectId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vSubjectId);
                                        ht.Add(Utilities.clsParameters.vWorkspaceId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vWorkspaceId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vWorkspaceId);
                                        ht.Add(Utilities.clsParameters.ParentWorkspaceId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vParentWorkSpaceId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vParentWorkSpaceId);
                                        ht.Add(Utilities.clsParameters.ActivityId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vActivityId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vActivityId);
                                        ht.Add(Utilities.clsParameters.iNodeId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.iNodeId) ? "" : _Save_CRFHdrDtlSubDtlDTO.iNodeId);
                                        ht.Add(Utilities.clsParameters.PeriodId, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.vPeriodId) ? "" : _Save_CRFHdrDtlSubDtlDTO.vPeriodId);
                                        ht.Add(Utilities.clsParameters.MySubjectNo, String.IsNullOrEmpty(_Save_CRFHdrDtlSubDtlDTO.iMySubjectNo) ? "" : _Save_CRFHdrDtlSubDtlDTO.iMySubjectNo);
                                        ht.Add(Utilities.clsParameters.RepetationNo, iRepetationNo);
                                        try
                                        {
                                            int resultdata = _SqlHelper.ExecuteSP("Proc_RemoveCRFTempDetail", ht);
                                            if (resultdata != -1)
                                            {
                                                result = "Error occured while Save Information of CRFDtl In Database";
                                                save = false;
                                                break;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            sResult = ex.Message + " Error occured while saving Proc_RemoveCRFTempDetail";
                                            _SqlHelper.RollBack_Transaction();
                                        }
                                    }
                                }
                                result = "success";
                                save = true;
                            }

                            //for mi image save
                            try
                            {
                                if (save == true)
                                {
                                    ht.Clear();
                                    DataTable dt = new DataTable();
                                    DataTable DicomAnnotationDtl = new DataTable();
                                    dt = Utilities.ToDataTable(_MIFinalLesionDetailsDataDTO.MIFinalLesionImageList);
                                    DicomAnnotationDtl = Utilities.ToDataTable(_MIFinalLesionDetailsDataDTO.DicomAnnotationDtl);

                                    ht.Clear();
                                    ht.Add(Utilities.clsParameters.iImgTransmittalHdrId, _MIFinalLesionDetailsDataDTO.iImgTransmittalHdrId);
                                    ht.Add(Utilities.clsParameters.iImgTransmittalDtlId, _MIFinalLesionDetailsDataDTO.iImgTransmittalDtlId);
                                    ht.Add(Utilities.clsParameters.iModifyBy, _MIFinalLesionDetailsDataDTO.UserID);
                                    ht.Add(Utilities.clsParameters.iImageMode, _MIFinalLesionDetailsDataDTO.iImageMode);
                                    ht.Add(Utilities.clsParameters.TransmittalImgDtl, dt);
                                    ht.Add(Utilities.clsParameters.DicomAnnotationDtl, DicomAnnotationDtl);
                                    ht.Add(Utilities.clsParameters.vLesionType, _MIFinalLesionDetailsDataDTO.vLesionType);
                                    ht.Add(Utilities.clsParameters.vLesionSubType, _MIFinalLesionDetailsDataDTO.vLesionSubType);
                                    ht.Add(Utilities.clsParameters.cRadiologist, _MIFinalLesionDetailsDataDTO.cRadiologist);
                                    ht.Add(Utilities.clsParameters.ImageTransmittalImgDtl_iImageTranNo, _MIFinalLesionDetailsDataDTO.iImageTranNo);
                                    ht.Add(Utilities.clsParameters.vParentActivityId, _MIFinalLesionDetailsDataDTO.vParentActivityId);
                                    ht.Add(Utilities.clsParameters.iParentNodeId, _MIFinalLesionDetailsDataDTO.iParentNodeId);
                                    ht.Add(Utilities.clsParameters.vActivityId, _MIFinalLesionDetailsDataDTO.vActivityId);
                                    ht.Add(Utilities.clsParameters.iNodeId, _MIFinalLesionDetailsDataDTO.iNodeId);
                                    ht.Add(Utilities.clsParameters.vActivityName, _MIFinalLesionDetailsDataDTO.vActivityName);
                                    ht.Add(Utilities.clsParameters.vSubActivityName, _MIFinalLesionDetailsDataDTO.vSubActivityName);
                                    ht.Add(Utilities.clsParameters.vPeriodId, _MIFinalLesionDetailsDataDTO.vPeriodId);
                                    ht.Add(Utilities.clsParameters.vModalityDesc, _MIFinalLesionDetailsDataDTO.vModalityDesc);
                                    ht.Add(Utilities.clsParameters.vAnatomyDesc, _MIFinalLesionDetailsDataDTO.vAnatomyDesc);

                                    try
                                    {
                                        sResult = _SqlHelper.ExecuteProcedure("Insert_ImageTransmittalDetails", ht);
                                    }
                                    catch (Exception ex)
                                    {
                                        sResult = ex.Message + " Error occured while saving Insert_ImageTransmittalDetails";
                                        _SqlHelper.RollBack_Transaction();
                                    }

                                    //if (sResult == "1") //Commented By Bhargav Thaker //22Feb2023
                                    if (sResult == _MIFinalLesionDetailsDataDTO.iImgTransmittalDtlId.ToString()) //Added By Bhargav Thaker //22Feb2023
                                    {
                                        sResult = "success";
                                        _SqlHelper.Commit_Transaction();
                                    }
                                    else
                                    {
                                        sResult = "error";
                                        _SqlHelper.RollBack_Transaction();
                                    }
                                }
                                else
                                {
                                    sResult = result;
                                    _SqlHelper.RollBack_Transaction();
                                }
                            }
                            catch (Exception e)
                            {
                                sResult = e.Message + " Error occured while saving Insert_ImageTransmittalDetails";
                                _SqlHelper.RollBack_Transaction();
                            }
                        }
                        catch (Exception cx)
                        {
                            _SqlHelper.RollBack_Transaction();
                        }
                        finally
                        {
                            _SqlHelper.Close_Connection();
                        }
                    }
                    else
                    {
                        sResult = "RecordsNotFound";
                    }
                }
                else
                {
                    sResult = "RecordsNotFound";
                }
            }
            catch (Exception ex)
            {
                return sResult;
            }
            finally { }
            return sResult;
        }

        public string SkipMIFinalLesionData(MIFinalSkipLesionDetailDTO _MIFinalSkipLesionDetailDTO)
        {
            string result = string.Empty;
            try
            {
                string Wstr = string.Empty;
                string vWorkSpaceId = Convert.ToString(_MIFinalSkipLesionDetailDTO.vWorkspaceId);
                string vActivityId = Convert.ToString(_MIFinalSkipLesionDetailDTO.vActivityId);
                string iNodeId = Convert.ToString(_MIFinalSkipLesionDetailDTO.iNodeId);
                string vSubjectId = Convert.ToString(_MIFinalSkipLesionDetailDTO.vSubjectId);
                string vPeriodId = Convert.ToString(_MIFinalSkipLesionDetailDTO.vPeriodId);
                if (_MIFinalSkipLesionDetailDTO.cSaveStatusFlagValidation == "N")
                {
                    Wstr = "vWorkSpaceId='" + vWorkSpaceId + "' and vActivityId='" + vActivityId + "'" + " and iNodeId='" + iNodeId + "' and vSubjectId = '" + vSubjectId + "' and vPeriodId = '" + vPeriodId + "' and cStatusIndi <> 'D'";
                }
                else
                {
                    Wstr = "vWorkSpaceId='" + vWorkSpaceId + "' and vActivityId='" + vActivityId + "'" + " and iNodeId='" + iNodeId + "' and vSubjectId = '" + vSubjectId + "' and vPeriodId = '" + vPeriodId + "' and cSaveStatus = 'N' and cStatusIndi <> 'D'";
                }
                Sql_Dt = _SqlHelper.GetDataTable("CRFTempDetail", Wstr, Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);

                if (Sql_Dt.Rows.Count == 0)
                {
                    result = "NO_LESION_DETAIL_FOUND";
                    return result;
                }
                //DTO that is used to save data in Save_CRFHdrDtlSubDtl                
                //result = Save_CRFHdrDtlSubDtl(_Save_CRFHdrDtlSubDtlDTO);
                result = Skip_CRFHdrDtlSubDtl(_MIFinalSkipLesionDetailDTO);
                return result;

            }
            catch (Exception e)
            {
                result = "Error : " + e.InnerException;
                return result;
            }
            finally
            {
            }
        }

        public string Skip_CRFHdrDtlSubDtl(MIFinalSkipLesionDetailDTO _MIFinalSkipLesionDetailDTO)
        {
            string sResult = "";
            try
            {
                string str_ReturnValue1 = "";
                string str_ReturnValue2 = "";
                ArrayList DataReturnList = new ArrayList();
                DataSet dsResult = new DataSet();

                //DataTable dtCRFHdrDtlSubDtl = Utilities.ToDataTable(_Save_CRFHdrDtlSubDtlDTO._Save_CRFHdrDtlSubDtlList);
                string SubjectId = Convert.ToString(_MIFinalSkipLesionDetailDTO.vSubjectId);
                string WorkspaceId = Convert.ToString(_MIFinalSkipLesionDetailDTO.vWorkspaceId);
                string ParentWorkspaceId = Convert.ToString(_MIFinalSkipLesionDetailDTO.vParentWorkSpaceId);
                string ActivityId = Convert.ToString(_MIFinalSkipLesionDetailDTO.vActivityId);
                string NodeId = Convert.ToString(_MIFinalSkipLesionDetailDTO.iNodeId);
                string PeriodId = Convert.ToString(_MIFinalSkipLesionDetailDTO.vPeriodId);
                string MySubjectNo = Convert.ToString(_MIFinalSkipLesionDetailDTO.iMySubjectNo);

                ht.Clear();
                ht.Add(Utilities.clsParameters.vSubjectId, String.IsNullOrEmpty(_MIFinalSkipLesionDetailDTO.vSubjectId) ? "" : _MIFinalSkipLesionDetailDTO.vSubjectId);
                ht.Add(Utilities.clsParameters.vWorkspaceId, String.IsNullOrEmpty(_MIFinalSkipLesionDetailDTO.vWorkspaceId) ? "" : _MIFinalSkipLesionDetailDTO.vWorkspaceId);
                ht.Add(Utilities.clsParameters.ParentWorkspaceId, String.IsNullOrEmpty(_MIFinalSkipLesionDetailDTO.vParentWorkSpaceId) ? "" : _MIFinalSkipLesionDetailDTO.vParentWorkSpaceId);
                ht.Add(Utilities.clsParameters.ActivityId, String.IsNullOrEmpty(_MIFinalSkipLesionDetailDTO.vActivityId) ? "" : _MIFinalSkipLesionDetailDTO.vActivityId);
                ht.Add(Utilities.clsParameters.iNodeId, String.IsNullOrEmpty(_MIFinalSkipLesionDetailDTO.iNodeId) ? "" : _MIFinalSkipLesionDetailDTO.iNodeId);
                ht.Add(Utilities.clsParameters.PeriodId, String.IsNullOrEmpty(_MIFinalSkipLesionDetailDTO.vPeriodId) ? "" : _MIFinalSkipLesionDetailDTO.vPeriodId);
                ht.Add(Utilities.clsParameters.MySubjectNo, String.IsNullOrEmpty(_MIFinalSkipLesionDetailDTO.iMySubjectNo) ? "" : _MIFinalSkipLesionDetailDTO.iMySubjectNo);
                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams("Proc_GetCRFTempDetail", ht);

                if (Sql_Ds != null && Sql_Ds.Tables.Count > 0)
                {
                    DataTable dt_TOP_CRFHdrDtlSubDtl = Sql_Ds.Tables[0];
                    DataTable dtNoOfRepetation = Sql_Ds.Tables[1];

                    if (dt_TOP_CRFHdrDtlSubDtl != null && dt_TOP_CRFHdrDtlSubDtl.Rows.Count != 0)
                    {
                        bool save = false;
                        string result = string.Empty;

                        _SqlHelper.Open_Connection();
                        _SqlHelper.Begin_Transaction();

                        try
                        {
                            foreach (DataRow dr_NoOfRepetation in dtNoOfRepetation.Rows)
                            {
                                int? iRepetationNo = Convert.ToInt32(dr_NoOfRepetation["iRepetationNo"]);

                                DataView dv = new DataView(dt_TOP_CRFHdrDtlSubDtl);
                                dv.RowFilter = "iRepetationNo=" + iRepetationNo + "";
                                DataTable dtCRFHdrDtlSubDtl = dv.ToTable();
                                string ModifyBy = Convert.ToString(dtCRFHdrDtlSubDtl.Rows[0]["iModifyBy"]);

                                if (!AssignValues(SubjectId, WorkspaceId, ParentWorkspaceId, ActivityId, NodeId, PeriodId, MySubjectNo, ModifyBy, dtCRFHdrDtlSubDtl, ref dsResult))
                                {
                                    result = "Error Occured While Retrieving Procedure Info.";
                                    save = false;
                                    break;
                                }

                                if (_SqlHelper == null)
                                {
                                    result = "Data Object Is Invalid or Nothing";
                                    save = false;
                                    break;
                                }

                                if (_SqlHelper.Transaction == null)
                                {
                                    result = "Transaction object is not started";
                                    save = false;
                                    break;
                                }

                                string wstr = "vWorkspaceId='" + _MIFinalSkipLesionDetailDTO.vWorkspaceId + "' And vSubjectId='" + _MIFinalSkipLesionDetailDTO.vSubjectId + "' And iNodeId='" + _MIFinalSkipLesionDetailDTO.iNodeId + "' And vactivityid = '" + _MIFinalSkipLesionDetailDTO.vActivityId + "' and cStatusIndi <> 'D' ";

                                dt_CRFSubDtl = _SqlHelper.GetDataTable(Utilities.clsView.View_GetCRFSubDtl, wstr, Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);

                                if (dt_CRFSubDtl.Rows.Count != 0)
                                {
                                    str_ReturnValue1 = "";
                                    str_ReturnValue2 = "";
                                    DataReturnList = null;
                                    DataReturnList = new ArrayList();
                                    DataTable Tbl_DtCRFHdr = dsResult.Tables["CRFHdr"];
                                    DataTable Tbl_DtCRFDtl = dsResult.Tables["CRFDtl"];
                                    DataTable Tbl_DtCRFSubDtl = dsResult.Tables["CRFSubDtl"];
                                    DataTable Tbl_DtDCFMst = dsResult.Tables["DCFMst"];

                                    //str_ReturnValue1 = (string)DataReturnList[0];
                                    foreach (DataRow dr in Tbl_DtCRFSubDtl.Rows)
                                    {
                                        dr["nCRFDtlNo"] = Convert.ToInt32(dt_CRFSubDtl.Rows[0]["nCRFDtlNo"]);
                                        dr["iTranNo"] = "1";
                                        dr.AcceptChanges();
                                    }
                                    Tbl_DtCRFSubDtl.AcceptChanges();

                                    str_ReturnValue1 = "";
                                    str_ReturnValue2 = "";
                                    DataReturnList = null;
                                    DataReturnList = new ArrayList();
                                    //Now saving Remaining Rows with same iTranNo
                                    if (!_SqlHelper.SaveInDbDataTable("Insert_CRFSubDtl", Tbl_DtCRFSubDtl, 1, ref DataReturnList, ref sResult))
                                    {
                                        result = "Error occured while Save Information of CRFSubDtl In Database";
                                        save = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    str_ReturnValue1 = "";
                                    str_ReturnValue2 = "";
                                    DataReturnList = null;
                                    DataReturnList = new ArrayList();
                                    DataTable Tbl_DtCRFHdr = dsResult.Tables["CRFHdr"];
                                    DataTable Tbl_DtCRFDtl = dsResult.Tables["CRFDtl"];
                                    DataTable Tbl_DtCRFSubDtl = dsResult.Tables["CRFSubDtl"];
                                    DataTable Tbl_DtDCFMst = dsResult.Tables["DCFMst"];
                                    if (!_SqlHelper.SaveInDbDataTable("Insert_CRFHdr", Tbl_DtCRFHdr, 1, ref DataReturnList, ref sResult))
                                    {
                                        result = "Error occured while Save Information of CRFHdr In Database";
                                        save = false;
                                        break;
                                    }

                                    str_ReturnValue1 = (string)DataReturnList[0];
                                    foreach (DataRow dr in Tbl_DtCRFDtl.Rows)
                                    {
                                        dr["nCRFHdrNo"] = Convert.ToInt32(str_ReturnValue1);
                                        dr.AcceptChanges();
                                    }
                                    Tbl_DtCRFDtl.AcceptChanges();

                                    DataTable Tbl_Top_CRFDtl = Tbl_DtCRFDtl.Copy();
                                    foreach (DataRow dr in Tbl_Top_CRFDtl.Rows)
                                    {
                                        dr["iRepeatNo"] = 0;
                                        dr.AcceptChanges();
                                    }
                                    Tbl_Top_CRFDtl.AcceptChanges();

                                    str_ReturnValue1 = "";
                                    str_ReturnValue2 = "";
                                    DataReturnList = null;
                                    DataReturnList = new ArrayList();
                                    if (!_SqlHelper.SaveInDbDataTable("Insert_CRFDtl", Tbl_Top_CRFDtl, 1, ref DataReturnList, ref sResult))
                                    {
                                        result = "Error occured while Save Information of CRFDtl In Database";
                                        save = false;
                                        break;
                                    }
                                    Tbl_DtCRFDtl.Rows[0].Delete();
                                    Tbl_DtCRFDtl.AcceptChanges();

                                    str_ReturnValue1 = (string)DataReturnList[0];
                                    str_ReturnValue2 = (string)DataReturnList[1];

                                    foreach (DataRow dr in Tbl_DtCRFDtl.Rows)
                                    {
                                        dr["nCRFDtlNo"] = Convert.ToInt32(str_ReturnValue1);
                                        dr["iRepeatNo"] = Convert.ToInt32(str_ReturnValue2);
                                        dr.AcceptChanges();
                                    }
                                    Tbl_DtCRFDtl.AcceptChanges();

                                    foreach (DataRow dr in Tbl_Top_CRFDtl.Rows)
                                    {
                                        dr["nCRFDtlNo"] = Convert.ToInt32(str_ReturnValue1);
                                        dr["iRepeatNo"] = Convert.ToInt32(str_ReturnValue2);
                                        dr.AcceptChanges();
                                    }
                                    Tbl_Top_CRFDtl.AcceptChanges();

                                    foreach (DataRow dr in Tbl_DtCRFSubDtl.Rows)
                                    {
                                        dr["nCRFDtlNo"] = Convert.ToInt32(str_ReturnValue1);
                                        dr.AcceptChanges();
                                    }
                                    Tbl_DtCRFSubDtl.AcceptChanges();

                                    DataTable Tbl_Top_CRFSubDtl = Tbl_DtCRFSubDtl.Copy();
                                    for (int i = Tbl_Top_CRFSubDtl.Rows.Count - 1; i >= 1; i += -1)
                                    {
                                        Tbl_Top_CRFSubDtl.Rows[i].Delete();
                                        Tbl_Top_CRFSubDtl.AcceptChanges();
                                    }
                                    foreach (DataRow dr in Tbl_Top_CRFSubDtl.Rows)
                                    {
                                        dr["iTranNo"] = 0;
                                        dr.AcceptChanges();
                                    }
                                    Tbl_Top_CRFSubDtl.AcceptChanges();

                                    str_ReturnValue1 = "";
                                    str_ReturnValue2 = "";
                                    DataReturnList = null;
                                    DataReturnList = new ArrayList();
                                    if (!_SqlHelper.SaveInDbDataTable("Insert_CRFSubDtl", Tbl_Top_CRFSubDtl, 1, ref DataReturnList, ref sResult))
                                    {
                                        result = "Error occured while Save Information of CRFSubDtl In Database";
                                        save = false;
                                        break;
                                    }
                                    Tbl_DtCRFSubDtl.Rows[0].Delete();
                                    Tbl_DtCRFSubDtl.AcceptChanges();

                                    str_ReturnValue1 = (string)DataReturnList[0];
                                    foreach (DataRow dr in Tbl_DtCRFSubDtl.Rows)
                                    {
                                        dr["iTranNo"] = Convert.ToInt32(str_ReturnValue1);
                                        dr.AcceptChanges();
                                    }
                                    Tbl_DtCRFSubDtl.AcceptChanges();

                                    str_ReturnValue1 = "";
                                    str_ReturnValue2 = "";
                                    DataReturnList = null;
                                    DataReturnList = new ArrayList();
                                    //Now saving Remaining Rows with same iTranNo
                                    if (!_SqlHelper.SaveInDbDataTable("Insert_CRFSubDtl", Tbl_DtCRFSubDtl, 1, ref DataReturnList, ref sResult))
                                    {
                                        result = "Error occured while Save Information of CRFSubDtl In Database";
                                        save = false;
                                        break;
                                    }

                                    foreach (DataRow dr in Tbl_Top_CRFDtl.Rows)
                                    {
                                        string strcDataStatus = Convert.ToString(dr["cDataStatus"]);
                                        if (strcDataStatus == "C")
                                        {
                                            dr["cDataStatus"] = "D";
                                        }
                                    }
                                    Tbl_Top_CRFDtl.AcceptChanges();

                                    str_ReturnValue1 = "";
                                    str_ReturnValue2 = "";
                                    DataReturnList = null;
                                    DataReturnList = new ArrayList();
                                    if (!_SqlHelper.SaveInDbDataTable("Insert_CRFDtl", Tbl_Top_CRFDtl, 1, ref DataReturnList, ref sResult))
                                    {
                                        result = "Error occured while Save Information of CRFDtl In Database";
                                        save = false;
                                        break;
                                    }
                                }

                                if (iRepetationNo != null)
                                {
                                    ht.Clear();
                                    ht.Add(Utilities.clsParameters.vSubjectId, String.IsNullOrEmpty(_MIFinalSkipLesionDetailDTO.vSubjectId) ? "" : _MIFinalSkipLesionDetailDTO.vSubjectId);
                                    ht.Add(Utilities.clsParameters.vWorkspaceId, String.IsNullOrEmpty(_MIFinalSkipLesionDetailDTO.vWorkspaceId) ? "" : _MIFinalSkipLesionDetailDTO.vWorkspaceId);
                                    ht.Add(Utilities.clsParameters.ParentWorkspaceId, String.IsNullOrEmpty(_MIFinalSkipLesionDetailDTO.vParentWorkSpaceId) ? "" : _MIFinalSkipLesionDetailDTO.vParentWorkSpaceId);
                                    ht.Add(Utilities.clsParameters.ActivityId, String.IsNullOrEmpty(_MIFinalSkipLesionDetailDTO.vActivityId) ? "" : _MIFinalSkipLesionDetailDTO.vActivityId);
                                    ht.Add(Utilities.clsParameters.iNodeId, String.IsNullOrEmpty(_MIFinalSkipLesionDetailDTO.iNodeId) ? "" : _MIFinalSkipLesionDetailDTO.iNodeId);
                                    ht.Add(Utilities.clsParameters.PeriodId, String.IsNullOrEmpty(_MIFinalSkipLesionDetailDTO.vPeriodId) ? "" : _MIFinalSkipLesionDetailDTO.vPeriodId);
                                    ht.Add(Utilities.clsParameters.MySubjectNo, String.IsNullOrEmpty(_MIFinalSkipLesionDetailDTO.iMySubjectNo) ? "" : _MIFinalSkipLesionDetailDTO.iMySubjectNo);
                                    ht.Add(Utilities.clsParameters.RepetationNo, iRepetationNo);

                                    int resultdata = _SqlHelper.ExecuteSP("Proc_RemoveCRFTempDetail", ht);
                                    if (resultdata != -1)
                                    {
                                        result = "Error occured while Save Information of CRFDtl In Database";
                                        save = false;
                                        break;
                                    }
                                }
                                result = "success";
                                save = true;
                            }

                            //for mi image save
                            if (save == true)
                            {
                                sResult = "success";
                                _SqlHelper.Commit_Transaction();
                            }
                            else
                            {
                                sResult = result;
                                _SqlHelper.RollBack_Transaction();
                            }
                        }
                        catch (Exception cx)
                        {
                            sResult = cx.Message + " Error occured while saving CRFHdrDtlSubDtl";
                            _SqlHelper.RollBack_Transaction();
                        }
                        finally
                        {
                            _SqlHelper.Close_Connection();
                        }
                    }
                    else
                    {
                        sResult = "RecordsNotFound";
                    }
                }
                else
                {
                    sResult = "RecordsNotFound";
                }
            }
            catch (Exception ex)
            {
                sResult = "Error " + ex.Message;
                return sResult;
            }
            finally
            {
                //_SqlHelper.Close_Connection();
                //_SqlHelper.Close_ConnectionBiz();
            }
            return sResult;
        }

        public DataTable GetTransmittalDtl(string iImgTransmittalDtlId)
        {
            try
            {
                Sql_Dt = new DataTable();
                string Wstr = "iImgTransmittalDtlId = " + iImgTransmittalDtlId;
                Sql_Dt = _SqlHelper.GetDataTable("ImageTransmittalImgDtl", Wstr, Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            { }
            return Sql_Dt;
        }

        public string Insert_Otp(OtpDTO _OtpDTO)
        {
            DataSet Sql_DsTbl = new DataSet();
            Hashtable ht;
            string wstr = string.Empty;

            try
            {
                ht = new Hashtable();
                ht.Add(Utilities.clsParameters.iUserId, String.IsNullOrEmpty(_OtpDTO.UserId) ? "0" : _OtpDTO.UserId);
                ht.Add(Utilities.clsParameters.vOTPNo, String.IsNullOrEmpty(_OtpDTO.Otp) ? "" : _OtpDTO.Otp);
                ht.Add(Utilities.clsParameters.dStartTime, DateTime.Now);
                ht.Add(Utilities.clsParameters.dEndTime, DateTime.Now.AddMinutes(10));
                ht.Add(Utilities.clsParameters.IsActive, "Y");

                var InsertOTP = _SqlHelper.ExecuteSP("Insert_OTPInfo", ht);
                return InsertOTP.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Insert_ExMsgInfo(ExMsgInfo objmodel)
        {
            DataSet Sql_DsTbl = new DataSet();
            Hashtable ht;

            string vNotificationType = objmodel.vNotificationType;
            string vSubject = objmodel.vSubject;
            string vBody = objmodel.vBody;
            string vFromEmailId = objmodel.vFromEmailId;
            string vToEmailId = objmodel.vToEmailId;
            string vBCCEmailId = objmodel.vBCCEmailId; // Added by Bhargav Thaker //24Feb2023
            string cIsSent = objmodel.cIsSent;
            string iCreatedBy = objmodel.iCreatedBy;
            string vRemarks = objmodel.vRemarks;
            string Mobile_No = objmodel.vPhoneNo;
            DateTime defaultval = new DateTime(1900, 1, 1); // Added by Bhargav Thaker //13Mar2023
            DateTime dSentDate = cIsSent == "Y" ? DateTime.Now : defaultval; // Added by Bhargav Thaker //13Mar2023

            try
            {
                ht = new Hashtable();
                ht.Add(Utilities.clsParameters.vNotificationType, vNotificationType);
                ht.Add(Utilities.clsParameters.vSubject, String.IsNullOrEmpty(vSubject) ? "" : vSubject);
                ht.Add(Utilities.clsParameters.vBody, vBody);
                ht.Add(Utilities.clsParameters.vFromEmailId, string.IsNullOrEmpty(vFromEmailId) ? "" : vFromEmailId);
                ht.Add(Utilities.clsParameters.vToEmailId, string.IsNullOrEmpty(vToEmailId) ? "" : vToEmailId);
                ht.Add(Utilities.clsParameters.vCCEmailId, null);
                ht.Add(Utilities.clsParameters.vBCCEmailId, string.IsNullOrEmpty(vBCCEmailId) ? "" : vBCCEmailId); //Modify by Bhargav Thaker //24Feb2023
                ht.Add(Utilities.clsParameters.vAttachment, null);
                ht.Add(Utilities.clsParameters.cIsSent, cIsSent);
                ht.Add(Utilities.clsParameters.dSentDate, dSentDate);
                ht.Add(Utilities.clsParameters.iCreatedBy, iCreatedBy);
                ht.Add(Utilities.clsParameters.dCreatedDate, DateTime.Now);
                ht.Add(Utilities.clsParameters.vRemarks, string.IsNullOrEmpty(vRemarks) ? "" : vRemarks);
                ht.Add(Utilities.clsParameters.vPhoneNo, string.IsNullOrEmpty(Mobile_No) ? "" : Mobile_No);

                var InsertOTP = _SqlHelper.ExecuteSP("Insert_ExMsgInfo", ht);
                return InsertOTP.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string InsertUpdate_Certificate(CertificateDTO _Certificate)
        {
            string sResult = "";
            int sResult1;
            try
            {
                string eStr_Retu123 = "";
                DataTable dt_Return = new DataTable();
                ArrayList DataReturnList = new ArrayList();
                //ParameterReturnValue RetuVal = null;
                DataTable dtCertificateDetails = Utilities.ToDataTable(_Certificate._SaveCertificate);
                ht.Clear();
                ht.Add(Utilities.clsParameters.iCertificateMasterId, _Certificate.iCertificateMasterId);
                ht.Add(Utilities.clsParameters.iCertificateMasterImgDtlId, _Certificate.iCertificateMasterImgDtlId);
                ht.Add(Utilities.clsParameters.vWorkspaceId, String.IsNullOrEmpty(_Certificate.vWorkspaceId) ? "" : _Certificate.vWorkspaceId);
                //ht.Add(Utilities.clsParameters.vSubjectId, String.IsNullOrEmpty(_Certificate.vSubjectId) ? "" : _Certificate.vSubjectId);
                ht.Add(Utilities.clsParameters.nvInstructions, _Certificate.nvInstructions);
                ht.Add(Utilities.clsParameters.iModalityNo, String.IsNullOrEmpty(_Certificate.iModalityNo) ? "" : _Certificate.iModalityNo);
                ht.Add(Utilities.clsParameters.iAnatomyNo, _Certificate.iAnatomyNo);
                ht.Add(Utilities.clsParameters.cIVContrast, _Certificate.cIVContrast);
                ht.Add(Utilities.clsParameters.dExaminationDate, _Certificate.dExaminationDate);
                ht.Add(Utilities.clsParameters.iNoImages, String.IsNullOrEmpty(_Certificate.iNoImages) ? "" : _Certificate.iNoImages);
                ht.Add(Utilities.clsParameters.vServerPath, _Certificate.vServerPath);
                ht.Add(Utilities.clsParameters.vRemark, String.IsNullOrEmpty(_Certificate.vRemark) ? "" : _Certificate.vRemark);
                ht.Add(Utilities.clsParameters.iModifyBy, _Certificate.iModifyBy);
                ht.Add(Utilities.clsParameters.iImageMode, _Certificate.iImageMode);
                ht.Add(Utilities.clsParameters.CertificateMasterImgDtl, dtCertificateDetails);
                ht.Add(Utilities.clsParameters.cOralContrast, _Certificate.cOralContrast);
                ht.Add(Utilities.clsParameters.vModalityNo, _Certificate.vModalityNo);
                ht.Add(Utilities.clsParameters.vModalityDesc, _Certificate.vModalityDesc);
                ht.Add(Utilities.clsParameters.vAnatomyDesc, _Certificate.vAnatomyDesc);
                ht.Add(Utilities.clsParameters.iImageTranNo, _Certificate.iImageTranNo);
                string strImageMode = Convert.ToString(_Certificate.iImageMode);
                if (strImageMode == "1")
                {
                    DataTable dtAnatomyNo = new DataTable();
                    if (!_SqlHelper.GetFromDB("Pro_GetCertAnatomyNoWithSplit", ht, ref DataReturnList, ref dtAnatomyNo, ref eStr_Retu123))
                    {
                        sResult = "Error Occured While Retrieving Data from AnatomyNo.";
                        return sResult;
                    }
                    if (dtAnatomyNo != null && dtAnatomyNo.Rows.Count != 0)
                    {
                        string strAnatomyNo = Convert.ToString(_Certificate.vAnatomyNo);
                        int[] ArrayAnatomyNo = strAnatomyNo.Split(',').Select(int.Parse).ToArray();
                        ArrayList NewArrayAnatomyNo = new ArrayList();
                        int j = 0;
                        foreach (DataRow dr in dtAnatomyNo.Rows)
                        {
                            int strWord = Convert.ToInt32(dr["word"]);

                            int index2 = Array.IndexOf(ArrayAnatomyNo, strWord);
                            NewArrayAnatomyNo.Add(strWord);
                            if (index2 != -1)
                            {
                                ArrayAnatomyNo = ArrayAnatomyNo.Where(val => val != strWord).ToArray();
                            }
                            j++;
                        }
                        NewArrayAnatomyNo.AddRange(ArrayAnatomyNo);
                        NewArrayAnatomyNo.Sort();
                        string newAnatomyNo = String.Join(",", NewArrayAnatomyNo.ToArray());
                        ht.Add(Utilities.clsParameters.vAnatomyNo, newAnatomyNo);
                    }
                    else
                    {
                        ht.Add(Utilities.clsParameters.vAnatomyNo, _Certificate.vAnatomyNo);
                    }
                }
                else
                {
                    ht.Add(Utilities.clsParameters.vAnatomyNo, _Certificate.vAnatomyNo);
                }

                if (!_SqlHelper.SaveInDb("InsertUpdate_CertificateMaster", ht, 1, ref DataReturnList, ref eStr_Retu123))
                {
                    sResult = "Error Occured While Retrieving Data.";
                    return sResult;
                }
                //DataReturnList = (ArrayList)DataReturnList[0];
                //RetuVal = (ParameterReturnValue)DataReturnList[0];
                //sResult = RetuVal.ParameterValue.ToString();
                sResult1 = Convert.ToInt32(DataReturnList[0]);
                if (sResult1 == -1)
                {
                    sResult = "-1";
                    //return sResult;
                }
                else
                {
                    sResult = (string)DataReturnList[1];
                }
                //foreach (parameterreturnvalue retun_val in datareturnlist)
                //{
                //    string str = convert.tostring(retun_val.parametervalue);
                //}
            }
            catch (Exception ex)
            {
                return "error " + ex;
            }
            finally
            {
            }
            return sResult;
        }
    }
}