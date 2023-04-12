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
    public class MIGetContract
    {
        SqlHelper _SqlHelper;
        DataSet Sql_Ds;
        DataTable Sql_Dt;
        Hashtable ht;

        public MIGetContract()
        {
            _SqlHelper = new SqlHelper();
            Sql_Ds = new DataSet();
            Sql_Dt = new DataTable();
            ht = new Hashtable();
        }

        public DataTable GetUserProfile(String userName)
        {
            // DataTable Sql_Dt;
            try
            {
                //Sql_Dt = new DataTable();
                //Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsDB.DBName + Utilities.clsView.View_UserMst, "vUserName='" + userName + "' order by vUserTypeName", Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                //Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsView.View_UserWiseProfile, "vUserName='" + userName + "' order by vUserTypeName", Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                ht.Clear();
                ht.Add(Utilities.clsParameters.vUserName, userName);
                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Proc_GetUserWiseProfile, ht);
                return Sql_Ds.Tables[0];
            }
            catch (Exception e)
            {
                throw e;
            }

            finally
            {
            }
            return Sql_Dt;
        }

        public DataTable GetUserAuthenticationDetails(LoginDetails _LoginDetails)
        {
            //DataSet Sql_Ds = new DataSet();
            //Hashtable ht;
            try
            {
                ht = new Hashtable();
                ht.Add(Utilities.clsParameters.vUserName, _LoginDetails.vUserName);
                ht.Add(Utilities.clsParameters.vLoginPass, Utilities.EncryptPassword(_LoginDetails.vLoginPass));
                ht.Add(Utilities.clsParameters.vUserTypeCode, _LoginDetails.vUserTypeCode);
                ht.Add(Utilities.clsParameters.vIPAddress, String.IsNullOrEmpty(_LoginDetails.vIPAddress) ? "" : _LoginDetails.vIPAddress);
                ht.Add(Utilities.clsParameters.vUserAgent, String.IsNullOrEmpty(_LoginDetails.vUserAgent) ? "" : _LoginDetails.vUserAgent);

                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Proc_login, ht);
            }
            catch (Exception e)
            {
            }
            finally
            {
            }
            return Sql_Ds.Tables[0];
        }

        public DataTable GetAdjUserAuthenticationDetails(LoginDetails _LoginDetails)
        {
            //DataSet Sql_Ds = new DataSet();
            //Hashtable ht;
            try
            {
                ht = new Hashtable();
                ht.Add(Utilities.clsParameters.vUserName, _LoginDetails.vUserName);
                ht.Add(Utilities.clsParameters.vUserTypeCode, _LoginDetails.vUserTypeCode);

                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Proc_UserAuthentication, ht);
            }
            catch (Exception e)
            {
            }
            finally
            {
            }

            return Sql_Ds.Tables[0];

        }

        public DataTable GetModality()
        {
            //DataTable sql_dt;
            try
            {
                //sql_dt = new DataTable();
                Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsView.View_ModalityMst, Utilities.DataRetrievalModeEnum.DataTable_AllRecords);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return Sql_Dt;
        }

        public DataTable GetModalityAuditTrail(MIModalityDTO _Modality)
        {
            //DataSet Sql_Ds = new DataSet();
            //Hashtable ht;
            try
            {
                //ht = new Hashtable();
                ht.Add(Utilities.clsParameters.nModalityNo, _Modality.nModalityNo);
                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Proc_GetModalityAuditTail, ht);
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

        public DataTable GetAnatomyAuditTrail(MIAnatomyDTO _Anatomy)
        {
            //DataSet Sql_Ds = new DataSet();
            //Hashtable ht;
            try
            {
                //ht = new Hashtable();
                ht.Add(Utilities.clsParameters.nAnatomyNo, _Anatomy.nAnatomyNo);
                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Proc_GetAnatomyAuditTail, ht);
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

        public DataTable GetAnatomy()
        {
            //DataTable sql_dt;           
            try
            {
                //sql_dt = new DataTable();
                Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsView.View_AnatomyMst, Utilities.DataRetrievalModeEnum.DataTable_AllRecords);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }

            return Sql_Dt;
        }

        public DataTable GetUserProfile(LoginDetails _LoginDetails)
        {
            try
            {
                //Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsView.View_UserWiseProfile, "vUserName='" + _LoginDetails.vUserName + "' order by vUserTypeName", Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                ht.Clear();
                ht.Add(Utilities.clsParameters.vUserName, _LoginDetails.vUserName);
                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Proc_GetUserWiseProfile, ht);
                return Sql_Ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return Sql_Dt;
        }

        public DataTable GetUserMenu(LoginDetails _LoginDetails)
        {
            try
            {
                Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsView.View_Menu, "cActiveFlag<>'D' AND cOperationType='" + Utilities.clsVariables.OperationType + "' AND vUserTypeCode=" + _LoginDetails.vUserTypeCode + "order by ParentID,iSeqNo", Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return Sql_Dt;
        }

        public DataTable GetImgTransmittalHdr(MIImageTransmittalHdr _GetImgTransmittalHdrDTO)
        {
            //DataSet Sql_Ds = new DataSet();
            //Hashtable ht;
            try
            {
                //ht = new Hashtable();
                ht.Add(Utilities.clsParameters.iImgTransmittalHdrId, _GetImgTransmittalHdrDTO.iImgTransmittalHdrId_int);
                ht.Add(Utilities.clsParameters.vWorkspaceId, _GetImgTransmittalHdrDTO.vWorkspaceId);
                ht.Add(Utilities.clsParameters.vProjectNo, _GetImgTransmittalHdrDTO.vProjectNo);
                ht.Add(Utilities.clsParameters.vSubjectId, _GetImgTransmittalHdrDTO.vSubjectId);
                ht.Add(Utilities.clsParameters.vRandomizationNo, _GetImgTransmittalHdrDTO.vRandomizationNo);
                ht.Add(Utilities.clsParameters.iNodeId, _GetImgTransmittalHdrDTO.iNodeId_int);
                ht.Add(Utilities.clsParameters.iImageStatus, _GetImgTransmittalHdrDTO.iImageStatus);
                ht.Add(Utilities.clsParameters.FromDate, _GetImgTransmittalHdrDTO.FromDate);
                ht.Add(Utilities.clsParameters.ToDate, _GetImgTransmittalHdrDTO.ToDate);
                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Pro_GetImgTransmittalHdr, ht);
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

        public DataTable GetImgTransmittalDtl(MIImageTransmittalDtl _GetImgTransmittalDtlDTO)
        {
            //DataSet Sql_Ds = new DataSet();
            //Hashtable ht;
            try
            {
                //ht = new Hashtable();
                ht.Add(Utilities.clsParameters.iImgTransmittalDtlId, _GetImgTransmittalDtlDTO.iImgTransmittalDtlId_int);
                ht.Add(Utilities.clsParameters.iImgTransmittalHdrId, _GetImgTransmittalDtlDTO.iImgTransmittalHdrId_int);
                ht.Add(Utilities.clsParameters.iModalityNo, _GetImgTransmittalDtlDTO.iModalityNo_int);
                ht.Add(Utilities.clsParameters.iAnatomyNo, _GetImgTransmittalDtlDTO.iAnatomyNo_int);
                ht.Add(Utilities.clsParameters.iImageStatus, _GetImgTransmittalDtlDTO.iImageStatus);
                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Pro_GetImgTransmittalDtl, ht);
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

        public DataTable GetImageTransmittalImgDtl(MIImageTransmittalImgDtl _GetImageTransmittalImgDtlDTO)
        {
            //DataSet Sql_Ds = new DataSet();
            //Hashtable ht;
            try
            {
                //ht = new Hashtable();
                ht.Add(Utilities.clsParameters.iImgTransmittalImgDtlId, _GetImageTransmittalImgDtlDTO.iImgTransmittalImgDtlId_int);
                ht.Add(Utilities.clsParameters.iImgTransmittalDtlId, _GetImageTransmittalImgDtlDTO.iImgTransmittalDtlId_int);
                ht.Add(Utilities.clsParameters.iImgTransmittalHdrId, _GetImageTransmittalImgDtlDTO.iImgTransmittalHdrId_int);
                ht.Add(Utilities.clsParameters.iImageStatus, _GetImageTransmittalImgDtlDTO.iImageStatus_int);
                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Pro_GetImageTransmittalImgDtl, ht);
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

        public DataTable CheckListTemplateDetail(MICheckListTemplateDTO _MICheckListTemplateDTO)
        {
            try
            {
                ht.Add(Utilities.clsParameters.type, _MICheckListTemplateDTO.type);
                ht.Add(Utilities.clsParameters.nTemplateHdrNo, _MICheckListTemplateDTO.nTemplateHdrNo);
                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Proc_GetCheckListTemplate, ht);
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

        public DataTable SubjectStudyDetail(MISubjectStudyDTO _MISubjectStudyDTO)
        {
            try
            {
                if (_MISubjectStudyDTO.MODE != null || _MISubjectStudyDTO.MODE != string.Empty)
                {
                    ht.Clear();
                    ht.Add(Utilities.clsParameters.vWorkspaceId, _MISubjectStudyDTO.vWorkspaceId);
                    ht.Add(Utilities.clsParameters.vSubjectId, _MISubjectStudyDTO.vSubjectId);
                    ht.Add(Utilities.clsParameters.iNodeId, _MISubjectStudyDTO.iNodeId);
                    ht.Add(Utilities.clsParameters.cRadiologist, _MISubjectStudyDTO.cRadiologist);
                    Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Proc_GetSubjectStudyDetail, ht);
                    return Sql_Ds.Tables[0];
                }
                else
                {
                    string wstr = "vWorkspaceId='" + _MISubjectStudyDTO.vWorkspaceId + "' And vSubjectId='" + _MISubjectStudyDTO.vSubjectId + "' And iNodeId='" + _MISubjectStudyDTO.iNodeId + "' And ( cRadiologist is null  OR  cRadiologist = '" + _MISubjectStudyDTO.cRadiologist + "') And cStatusIndi <> 'D' order by iImgTransmittalHdrId";
                    //Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsView.View_GetSubjectStudyDetail, " vWorkspaceId = '" + _MISubjectStudyDTO.vWorkspaceId + "' AND vSubjectId = '" + _MISubjectStudyDTO.vSubjectId + "' AND iNodeId='" + _MISubjectStudyDTO.iNodeId + "' AND cStatusIndi <> 'D' order by iImgTransmittalHdrId", Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                    Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsView.View_GetSubjectStudyDetail, wstr, Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                    return Sql_Dt;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public DataTable getBizNetSubjectStudyDetail(MIBizNETSaveImage _MIBizNETImageReview)
        {
            try
            {
                Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsView.View_GetSubjectStudyDetail, " vWorkspaceId = '" + _MIBizNETImageReview.vWorkspaceId + "' AND vSubjectId = '" + _MIBizNETImageReview.vSubjectId + "' AND iNodeId='" + _MIBizNETImageReview.iNodeId + "'AND iImageStatus = 1 AND cStatusIndi <> 'D' order by dModify desc", Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                return Sql_Dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public DataTable getSubjectImageStudyDetail(MIImageReviewDTO _MIImageReviewDTO)
        {
            try
            {
                if (_MIImageReviewDTO.MODE != null && _MIImageReviewDTO.MODE != string.Empty)
                {

                    ht.Clear();
                    ht.Add(Utilities.clsParameters.iImgTransmittalHdrId, _MIImageReviewDTO.iImgTransmittalHdrId);
                    ht.Add(Utilities.clsParameters.iImgTransmittalDtlId, _MIImageReviewDTO.iImgTransmittalDtlId);
                    ht.Add(Utilities.clsParameters.iImageStatus, _MIImageReviewDTO.iImageStatus);
                    ht.Add(Utilities.clsParameters.cRadiologist, _MIImageReviewDTO.cRadiologist);
                    ht.Add(Utilities.clsParameters.iImageTranNo, _MIImageReviewDTO.ImageTransmittalImgDtl_iImageTranNo);
                    Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Proc_ViewGetSubjectImageStudyDetail, ht);
                    return Sql_Ds.Tables[0];

                    //string wstr = "iImgTransmittalHdrId = '" + _MIImageReviewDTO.iImgTransmittalHdrId + "' And iImgTransmittalDtlId = '" + _MIImageReviewDTO.iImgTransmittalDtlId + "' And iImageStatus = '" + _MIImageReviewDTO.iImageStatus + "' And ( cRadiologist is null  OR  cRadiologist = '" + _MIImageReviewDTO.cRadiologist + "') AND iImageTranNo = '" + _MIImageReviewDTO.ImageTransmittalImgDtl_iImageTranNo + "' AND cStatusIndi <> 'D' order by vSeriesNumber,vImgSliceLocation";
                    ////Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsView.View_GetSubjectImageStudyDetail, "iImgTransmittalHdrId = '" + _MIImageReviewDTO.iImgTransmittalHdrId + "' AND iImgTransmittalDtlId='" + _MIImageReviewDTO.iImgTransmittalDtlId + "'AND iImageStatus = '"+_MIImageReviewDTO.iImageStatus+"' AND cStatusIndi <> 'D' order by iImgTransmittalHdrId", Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                    //Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsView.View_GetSubjectImageStudyDetail, wstr, Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                    //return Sql_Dt;
                }
                else
                {
                    ht.Clear();
                    ht.Add(Utilities.clsParameters.iImgTransmittalHdrId, _MIImageReviewDTO.iImgTransmittalHdrId);
                    ht.Add(Utilities.clsParameters.iImgTransmittalDtlId, _MIImageReviewDTO.iImgTransmittalDtlId);
                    ht.Add(Utilities.clsParameters.iImageStatus, _MIImageReviewDTO.iImageStatus);
                    ht.Add(Utilities.clsParameters.cRadiologist, _MIImageReviewDTO.cRadiologist);
                    ht.Add(Utilities.clsParameters.iImageTranNo, _MIImageReviewDTO.ImageTransmittalImgDtl_iImageTranNo);
                    Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Proc_ViewGetSubjectImageStudyDetail, ht);
                    return Sql_Ds.Tables[0];

                    //string wstr = "iImgTransmittalHdrId = '" + _MIImageReviewDTO.iImgTransmittalHdrId + "' And iImgTransmittalDtlId = '" + _MIImageReviewDTO.iImgTransmittalDtlId + "' And iImageStatus = '" + _MIImageReviewDTO.iImageStatus + "' And ( cRadiologist is null  OR  cRadiologist = '" + _MIImageReviewDTO.cRadiologist + "') AND cStatusIndi <> 'D' order by vSeriesNumber,vImgSliceLocation";
                    ////Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsView.View_GetSubjectImageStudyDetail, "iImgTransmittalHdrId = '" + _MIImageReviewDTO.iImgTransmittalHdrId + "' AND iImgTransmittalDtlId='" + _MIImageReviewDTO.iImgTransmittalDtlId + "'AND iImageStatus = '"+_MIImageReviewDTO.iImageStatus+"' AND cStatusIndi <> 'D' order by iImgTransmittalHdrId", Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                    //Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsView.View_GetSubjectImageStudyDetail, wstr, Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                    //return Sql_Dt;
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public DataTable getSubjectSubSequentImageStudyDetail(MIImageReviewDTO _MIImageReviewDTO)
        {
            try
            {
                string wstr = "iImgTransmittalHdrId = '" + _MIImageReviewDTO.iImgTransmittalHdrId + "' And iImgTransmittalDtlId = '" + _MIImageReviewDTO.iImgTransmittalDtlId + "' And iImageStatus = '" + _MIImageReviewDTO.iImageStatus + "' And ( cRadiologist is null  OR  cRadiologist = '" + _MIImageReviewDTO.cRadiologist + "') AND iImageTranNo = '" + _MIImageReviewDTO.ImageTransmittalImgDtl_iImageTranNo + "' AND cStatusIndi <> 'D' order by iImgTransmittalHdrId";
                Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsView.View_GetSubjectImageStudyDetail, wstr, Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                return Sql_Dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }

        }


        public DataTable LesionDetails(LesionDTO _LesionDTO)
        {
            try
            {
                Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsView.View_MedExWorkSpaceDtl, "vWorkSpaceId = '" + _LesionDTO.vWorkspaceId + "' AND vActivityId='" + _LesionDTO.vActivityId + "'AND iNodeId = '" + _LesionDTO.iNodeId + "' AND cActiveFlag<>'N' order by iSeqNo", Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                return Sql_Dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //FOR LESION DETAIL FROM DYNAMIC PAGE THAT IS DESIGNED FOR MI
        public DataTable MILesionDetails(LesionDTO _LesionDTO)
        {
            try
            {
                Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsView.View_MedExWorkSpaceDtl, "vWorkSpaceId = '" + _LesionDTO.vWorkspaceId + "' AND vActivityId='" + _LesionDTO.vActivityId + "'AND iNodeId = '" + _LesionDTO.iNodeId + "' AND cActiveFlag<>'N' order by iSeqNo", Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                return Sql_Dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //FOR Radiologist UserId and UserType DETAIL
        public DataTable GetRadioLogistData(string vWorkspaceId)
        {
            try
            {
                Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsView.View_GetRadioLogistData, "vWorkSpaceId = '" + vWorkspaceId + "' AND vUserTypeName like '%Radiologist%'", Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                return Sql_Dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //FOR MARK DETAIL FROM MI THAT IS SAVED IN BIZNET ALSO TO VIEW MARK DETAIL IN ALL SUB SEQUENT VISIT
        public DataTable MILesionMARKDetails(LesionMarkDTO _LesionMarkDTO)
        {
            try
            {
                string Wstr = string.Empty;
                //Wstr = "vWorkSpaceId = '" + _LesionMarkDTO.vWorkspaceId + "' And vActivityId = '" + _LesionMarkDTO.vActivityId + "' And iNodeId = '" + _LesionMarkDTO.iNodeId + "' And vSubjectId = '"+_LesionMarkDTO.vSubjectId+"' And iMySubjectNo = '"+_LesionMarkDTO.iMySubjectNo+"' And ScreenNo = '"+_LesionMarkDTO.ScreenNo+"' And cSaveStatus = 'Y' And vActivityName like '%MARK%' And cStatusIndi <> 'D'";
                Wstr = "vWorkSpaceId = '" + _LesionMarkDTO.vWorkspaceId + "' And vSubjectId = '" + _LesionMarkDTO.vSubjectId + "' And iMySubjectNo = '" + _LesionMarkDTO.iMySubjectNo + "' And ScreenNo = '" + _LesionMarkDTO.ScreenNo + "' And cSaveStatus = 'Y' And vActivityName = '" + _LesionMarkDTO.vActivityName + "' And vSubActivityName = '" + _LesionMarkDTO.vSubActivityName + "' And cStatusIndi <> 'D' Order by iSeqNo";
                Sql_Dt = _SqlHelper.GetDataTable("CRFTempDetail", Wstr, Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                return Sql_Dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        //From MI Temp Table Those Data that are not still Saved in BizNet
        public DataTable getLesionDetailsold(Save_CRFHdrDtlSubDtlDTO _Save_CRFHdrDtlSubDtlDTO)
        {
            string StrGroupDesc = "";
            string StrGroupCodes = "";
            string AttributeValue = "";
            string Repetation = "";
            string strReturn = "";
            int CodeNo = 0;
            DataTable dt = new DataTable();
            DataTable dtTablulerRepetation = new DataTable();
            DataRow drTable;
            try
            {
                string Wstr = string.Empty;
                string vWorkSpaceId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vWorkspaceId);
                string vActivityId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vActivityId);
                string iNodeId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.iNodeId);
                string vSubjectId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vSubjectId);
                string vPeriodId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vPeriodId);
                //Wstr = "vWorkSpaceId='" + vWorkSpaceId + "' and vActivityId='" + vActivityId + "'" + " and iNodeId='" + iNodeId + "' and vSubjectId = '" + vSubjectId + "' and vPeriodId = '" + vPeriodId + "'";
                Wstr = "vWorkSpaceId='" + vWorkSpaceId + "' and vActivityId='" + vActivityId + "'" + " and iNodeId='" + iNodeId + "' and vSubjectId = '" + vSubjectId + "' and vPeriodId = '" + vPeriodId + "' and cSaveStatus = 'N' and cStatusIndi <> 'D'";
                //Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsView.View_GetSubjectImageStudyDetail, "iImgTransmittalHdrId = '" + _MIImageReviewDTO.iImgTransmittalHdrId + "' AND iImgTransmittalDtlId='" + _MIImageReviewDTO.iImgTransmittalDtlId + "'AND iImageStatus = '" + _MIImageReviewDTO.iImageStatus + "' AND cStatusIndi <> 'D' order by iImgTransmittalHdrId", Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                Sql_Dt = _SqlHelper.GetDataTable("CRFTempDetail", Wstr, Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);

                Sql_Dt.DefaultView.RowFilter = "iRepetationNo='1'";
                dt = Sql_Dt.DefaultView.ToTable();

                foreach (DataRow dr in dt.Rows)
                {
                    //if (Convert.ToInt32(dr["vMedExCode"]) != CodeNo)
                    //{
                    AttributeValue = dr["vMedExDesc"].ToString();
                    dtTablulerRepetation.Columns.Add(AttributeValue);
                    AttributeValue = "";
                    //}
                }

                foreach (DataRow drCRF in Sql_Dt.Rows)
                {

                    Sql_Dt.DefaultView.RowFilter = "iRepetationNo = " + drCRF["iRepetationNo"].ToString();
                    drTable = dtTablulerRepetation.NewRow();

                    if (drCRF["iRepetationNo"].ToString() != Repetation.ToString())
                    {
                        Repetation = drCRF["iRepetationNo"].ToString();
                        foreach (DataRow dr in Sql_Dt.DefaultView.ToTable().Rows)
                        {
                            if (!string.IsNullOrEmpty(dr["vMedExDesc"].ToString()))
                            {
                                AttributeValue = dr["vMedExDesc"].ToString();
                                drTable[AttributeValue] = dr["vMedExResult"].ToString();
                                AttributeValue = "";
                            }
                        }
                        dtTablulerRepetation.Rows.Add(drTable);
                        dtTablulerRepetation.AcceptChanges();
                    }
                }
                Sql_Dt = dtTablulerRepetation;
                return Sql_Dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable getLesionDetails(LesionDetailsDATA_DTO _LesionDetailsDATA_DTO)
        {
            string StrGroupDesc = "";
            string StrGroupCodes = "";
            string AttributeValue = "";
            string Repetation = "";
            string strReturn = "";
            int CodeNo = 0;
            DataTable dt = new DataTable();
            DataTable dtTablulerRepetation = new DataTable();
            DataRow drTable;
            try
            {
                string Wstr = string.Empty;
                string vWorkSpaceId = Convert.ToString(_LesionDetailsDATA_DTO.vWorkspaceId);
                string vActivityId = Convert.ToString(_LesionDetailsDATA_DTO.vActivityId);
                string iNodeId = Convert.ToString(_LesionDetailsDATA_DTO.iNodeId);
                string vSubjectId = Convert.ToString(_LesionDetailsDATA_DTO.vSubjectId);
                string vPeriodId = Convert.ToString(_LesionDetailsDATA_DTO.vPeriodId);
                //Wstr = "vWorkSpaceId='" + vWorkSpaceId + "' and vActivityId='" + vActivityId + "'" + " and iNodeId='" + iNodeId + "' and vSubjectId = '" + vSubjectId + "' and vPeriodId = '" + vPeriodId + "'";
                //Wstr = "vWorkSpaceId='" + vWorkSpaceId + "' and vActivityId='" + vActivityId + "'" + " and iNodeId='" + iNodeId + "' and vSubjectId = '" + vSubjectId + "' and vPeriodId = '" + vPeriodId + "' and cSaveStatus = 'N' and cStatusIndi <> 'D'";
                if (_LesionDetailsDATA_DTO.cSaveStatusFlagValidation == "N")
                {
                    Wstr = "vWorkSpaceId='" + vWorkSpaceId + "' and vActivityId='" + vActivityId + "'" + " and iNodeId='" + iNodeId + "' and vSubjectId = '" + vSubjectId + "' and vPeriodId = '" + vPeriodId + "' and cStatusIndi <> 'D'";
                }
                else
                {
                    Wstr = "vWorkSpaceId='" + vWorkSpaceId + "' and vActivityId='" + vActivityId + "'" + " and iNodeId='" + iNodeId + "' and vSubjectId = '" + vSubjectId + "' and vPeriodId = '" + vPeriodId + "' and cSaveStatus = 'N' and cStatusIndi <> 'D'";
                }

                //Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsView.View_GetSubjectImageStudyDetail, "iImgTransmittalHdrId = '" + _MIImageReviewDTO.iImgTransmittalHdrId + "' AND iImgTransmittalDtlId='" + _MIImageReviewDTO.iImgTransmittalDtlId + "'AND iImageStatus = '" + _MIImageReviewDTO.iImageStatus + "' AND cStatusIndi <> 'D' order by iImgTransmittalHdrId", Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                Sql_Dt = _SqlHelper.GetDataTable("CRFTempDetail", Wstr, Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);

                return Sql_Dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //From mi CRF Temp Tables Those Data that are Saved in BizNet
        public DataTable getLesionSavedDetailsold(Save_CRFHdrDtlSubDtlDTO _Save_CRFHdrDtlSubDtlDTO)
        {
            string StrGroupDesc = "";
            string StrGroupCodes = "";
            string AttributeValue = "";
            string Repetation = "";
            string strReturn = "";
            int CodeNo = 0;
            DataTable dt = new DataTable();
            DataTable dtTablulerRepetation = new DataTable();
            DataRow drTable;
            try
            {
                string Wstr = string.Empty;
                string vWorkSpaceId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vWorkspaceId);
                string vParentWorkspaceId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vParentWorkSpaceId);
                string vActivityId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vActivityId);
                string iNodeId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.iNodeId);
                string vSubjectId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vSubjectId);
                string iMySubjectNo = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.iMySubjectNo);
                string vPeriodId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vPeriodId);
                //Wstr = "vParentWorkspaceId='" + vParentWorkspaceId + "' and vWorkSpaceId='" + vWorkSpaceId + "' and vActivityId='" + vActivityId + "'" + " and iNodeId='" + iNodeId + "' and vsubjectid = '" + vSubjectId + "' and iperiod = '" + vPeriodId + "' and iMySubjectNo = '" + iMySubjectNo + "'";
                //Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsView.View_GetCRFData, Wstr, Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);

                Wstr = "vParentWorkspaceId='" + vParentWorkspaceId + "' and vWorkSpaceId='" + vWorkSpaceId + "' and vActivityId='" + vActivityId + "'" + " and iNodeId='" + iNodeId + "' and vSubjectId = '" + vSubjectId + "' and vPeriodId = '" + vPeriodId + "' and cSaveStatus = 'Y' and cStatusIndi <> 'D'";
                Sql_Dt = _SqlHelper.GetDataTable("CRFTempDetail", Wstr, Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);

                Sql_Dt.DefaultView.RowFilter = "iRepetationNo='1'";
                dt = Sql_Dt.DefaultView.ToTable();

                foreach (DataRow dr in dt.Rows)
                {
                    //if (Convert.ToInt32(dr["vMedExCode"]) != CodeNo)
                    //{
                    AttributeValue = dr["vMedExDesc"].ToString();
                    dtTablulerRepetation.Columns.Add(AttributeValue);
                    AttributeValue = "";
                    //}
                }

                foreach (DataRow drCRF in Sql_Dt.Rows)
                {
                    Sql_Dt.DefaultView.RowFilter = "iRepetationNo = " + drCRF["iRepetationNo"].ToString();
                    drTable = dtTablulerRepetation.NewRow();

                    if (drCRF["iRepetationNo"].ToString() != Repetation.ToString())
                    {
                        Repetation = drCRF["iRepetationNo"].ToString();
                        foreach (DataRow dr in Sql_Dt.DefaultView.ToTable().Rows)
                        {
                            if (!string.IsNullOrEmpty(dr["vMedExDesc"].ToString()))
                            {
                                AttributeValue = dr["vMedExDesc"].ToString();
                                drTable[AttributeValue] = dr["vMedExResult"].ToString();
                                AttributeValue = "";
                            }
                        }
                        dtTablulerRepetation.Rows.Add(drTable);
                        dtTablulerRepetation.AcceptChanges();
                    }
                }
                Sql_Dt = dtTablulerRepetation;
                return Sql_Dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable getLesionSavedDetails(Save_CRFHdrDtlSubDtlDTO _Save_CRFHdrDtlSubDtlDTO)
        {
            string StrGroupDesc = "";
            string StrGroupCodes = "";
            string AttributeValue = "";
            string Repetation = "";
            string strReturn = "";
            int CodeNo = 0;
            DataTable dt = new DataTable();
            DataTable dtTablulerRepetation = new DataTable();
            DataRow drTable;
            try
            {
                string Wstr = string.Empty;
                string vWorkSpaceId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vWorkspaceId);
                string vParentWorkspaceId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vParentWorkSpaceId);
                string vActivityId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vActivityId);
                string iNodeId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.iNodeId);
                string vSubjectId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vSubjectId);
                string iMySubjectNo = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.iMySubjectNo);
                string vPeriodId = Convert.ToString(_Save_CRFHdrDtlSubDtlDTO.vPeriodId);
                //Wstr = "vParentWorkspaceId='" + vParentWorkspaceId + "' and vWorkSpaceId='" + vWorkSpaceId + "' and vActivityId='" + vActivityId + "'" + " and iNodeId='" + iNodeId + "' and vsubjectid = '" + vSubjectId + "' and iperiod = '" + vPeriodId + "' and iMySubjectNo = '" + iMySubjectNo + "'";
                //Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsView.View_GetCRFData, Wstr, Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);

                Wstr = "vParentWorkspaceId='" + vParentWorkspaceId + "' and vWorkSpaceId='" + vWorkSpaceId + "' and vActivityId='" + vActivityId + "'" + " and iNodeId='" + iNodeId + "' and vSubjectId = '" + vSubjectId + "' and vPeriodId = '" + vPeriodId + "' and cStatusIndi <> 'D'";
                Sql_Dt = _SqlHelper.GetDataTable("CRFTempDetail", Wstr, Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);

                Sql_Dt.DefaultView.RowFilter = "iRepetationNo='1'";

                return Sql_Dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable MyProjectCompletionList(GetMyProjectCompletionListDTO _GetMyProjectCompletionListDTO)
        {
            DataTable dtReturn = new DataTable();
            try
            {
                string contextKey = Convert.ToString(_GetMyProjectCompletionListDTO.contextKey);
                string vProjectTypeCode = Convert.ToString(_GetMyProjectCompletionListDTO.vProjectTypeCode);
                string prefixText = Convert.ToString(_GetMyProjectCompletionListDTO.prefixText);

                if (!string.IsNullOrEmpty(vProjectTypeCode))
                {
                    vProjectTypeCode = " vProjectTypeCode in (" + vProjectTypeCode + ")";
                }
                if (!string.IsNullOrEmpty(contextKey))
                {
                    contextKey += " And " + vProjectTypeCode;
                }
                else
                {
                    contextKey = vProjectTypeCode;
                }

                string whereCondition = "vRequestId  + vProjectNo " + " Like '%";
                whereCondition += prefixText + "%'" + (!string.IsNullOrEmpty(contextKey.Trim()) ? " AND " + contextKey.Trim() : "");

                DataSet dsList = new DataSet();
                string strReturn = "";
                if (!_SqlHelper.GetFieldsOfTable("View_Myprojects", " *,'['+vProjectNo+'] '+vRequestId As vStudyNo ", whereCondition, ref dsList, ref strReturn))
                {
                    strReturn = strReturn + " Error while getting View_Myprojects";
                    throw new Exception(strReturn);
                }
                dtReturn = dsList.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtReturn;
        }

        public DataSet GetImgTransmittalDtlForQCReview(MI_DataSaveStatus _MI_DataSaveStatus)
        {
            string wstr = string.Empty;
            Sql_Ds = new DataSet();
            try
            {
                ht.Clear();
                ht.Add(Utilities.clsParameters.vWorkspaceId, _MI_DataSaveStatus.vWorkspaceId);
                ht.Add(Utilities.clsParameters.vSubjectId, _MI_DataSaveStatus.vSubjectId);
                ht.Add(Utilities.clsParameters.vActivityId, _MI_DataSaveStatus.vActivityId);
                ht.Add(Utilities.clsParameters.iNodeId, _MI_DataSaveStatus.iNodeId);
                ht.Add(Utilities.clsParameters.vOperationcode, _MI_DataSaveStatus.vOperationcode);
                ht.Add(Utilities.clsParameters.iImgTransmittalHdrId, _MI_DataSaveStatus.iImgTransmittalHdrId);
                ht.Add(Utilities.clsParameters.iImgTransmittalDtlId, _MI_DataSaveStatus.iImgTransmittalDtlId);
                ht.Add(Utilities.clsParameters.cRadiologist, _MI_DataSaveStatus.cRadiologist); //Added By Bhargav Thaker 15Mar2023
                ht.Add(Utilities.clsParameters.iParentNodeId, _MI_DataSaveStatus.iParentNodeId); //Added By Bhargav Thaker 15Mar2023
                ht.Add(Utilities.clsParameters.iUserId, _MI_DataSaveStatus.iUserId); //Added By Bhargav Thaker 15Mar2023

                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Pro_GetImgTransmittalDtlForAllReview, ht);
                return Sql_Ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return Sql_Ds;
        }

        public DataTable MyStudyCompletionList(GetMyProjectCompletionListDTO _GetMyProjectCompletionListDTO)
        {
            DataTable dtReturn = new DataTable();
            DataSet dsList = new DataSet();
            try
            {
                //Condition Added by Bhargav Thaker
                if (!string.IsNullOrEmpty(_GetMyProjectCompletionListDTO.vProjectTypeCode))
                {
                    if (_GetMyProjectCompletionListDTO.vProjectTypeCode == "-1")
                    {
                        string contextKey = Convert.ToString(_GetMyProjectCompletionListDTO.contextKey);
                        string whereCondition = contextKey;

                        string strReturn = "";
                        if (!_SqlHelper.GetFieldsOfTable("view_workspaceprotocoldetail", " '['+vprojectno+'] '+ vRequestId AS StudyName ", whereCondition, ref dsList, ref strReturn))
                        {
                            strReturn = strReturn + " Error while getting view_workspaceprotocoldetail";
                            throw new Exception(strReturn);
                        }
                    }
                    else
                    {
                        string contextKey = Convert.ToString(_GetMyProjectCompletionListDTO.contextKey);
                        string vProjectTypeCode = Convert.ToString(_GetMyProjectCompletionListDTO.vProjectTypeCode);
                        string prefixText = Convert.ToString(_GetMyProjectCompletionListDTO.prefixText);

                        if (!string.IsNullOrEmpty(vProjectTypeCode))
                        {
                            vProjectTypeCode = " vProjectTypeCode in (" + vProjectTypeCode + ")";
                        }
                        if (!string.IsNullOrEmpty(contextKey))
                        {
                            contextKey += " And " + vProjectTypeCode;
                        }
                        else
                        {
                            contextKey = vProjectTypeCode;
                        }

                        string whereCondition = "cWorkspaceType = 'P' AND vRequestId  + vProjectNo " + " Like '%";
                        whereCondition += prefixText + "%'" + (!string.IsNullOrEmpty(contextKey.Trim()) ? " AND " + contextKey.Trim() : "");

                        string strReturn = "";
                        if (!_SqlHelper.GetFieldsOfTable("View_Myprojects", " * ", whereCondition, ref dsList, ref strReturn))
                        {
                            strReturn = strReturn + " Error while getting View_Myprojects";
                            throw new Exception(strReturn);
                        }
                    }
                }
                else
                {
                    string contextKey = Convert.ToString(_GetMyProjectCompletionListDTO.contextKey);
                    string vProjectTypeCode = Convert.ToString(_GetMyProjectCompletionListDTO.vProjectTypeCode);
                    string prefixText = Convert.ToString(_GetMyProjectCompletionListDTO.prefixText);

                    if (!string.IsNullOrEmpty(vProjectTypeCode))
                    {
                        vProjectTypeCode = " vProjectTypeCode in (" + vProjectTypeCode + ")";
                    }
                    if (!string.IsNullOrEmpty(contextKey))
                    {
                        contextKey += " And " + vProjectTypeCode;
                    }
                    else
                    {
                        contextKey = vProjectTypeCode;
                    }

                    string whereCondition = "cWorkspaceType = 'P' AND vRequestId  + vProjectNo " + " Like '%";
                    whereCondition += prefixText + "%'" + (!string.IsNullOrEmpty(contextKey.Trim()) ? " AND " + contextKey.Trim() : "");

                    string strReturn = "";
                    if (!_SqlHelper.GetFieldsOfTable("View_Myprojects", " * ", whereCondition, ref dsList, ref strReturn))
                    {
                        strReturn = strReturn + " Error while getting View_Myprojects";
                        throw new Exception(strReturn);
                    }
                }
                dtReturn = dsList.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtReturn;
        }

        public DataTable GetStudyName(GetMyProjectCompletionListDTO _GetMyProjectCompletionListDTO)
        {
            DataTable dtresult = new DataTable();
            try
            {
                string contextKey = Convert.ToString(_GetMyProjectCompletionListDTO.contextKey);
                string whereCondition = contextKey;
                DataSet dsList = new DataSet();
                string strReturn = "";
                if (!_SqlHelper.GetFieldsOfTable("view_workspaceprotocoldetail", " '['+vprojectno+'] '+ vRequestId AS StudyName ", whereCondition, ref dsList, ref strReturn))
                {
                    strReturn = strReturn + " Error while getting View_Myprojects";
                    throw new Exception(strReturn);
                }
                dtresult = dsList.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtresult;
        }

        public DataTable ProjectActivityDetails(MIDicomProjectDetailDTO _MIDicomProjectDetailDTO)
        {
            string wstr = string.Empty;
            try
            {
                //ht.Add(Utilities.clsParameters.type, _MICheckListTemplateDTO.type);
                //ht.Add(Utilities.clsParameters.nTemplateHdrNo, _MICheckListTemplateDTO.nTemplateHdrNo);
                //Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Proc_GetCheckListTemplate, ht);


                if (_MIDicomProjectDetailDTO.iParentNodeId == null)
                {
                    wstr = "vWorkspaceId = '" + _MIDicomProjectDetailDTO.vWorkSpaceID + "' And iPeriod = '" + _MIDicomProjectDetailDTO.iPeriod + "' And cStatusIndi <> 'D' order by iNodeNo, iNodeId OPTION (MAXDOP 1)";
                    //Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsDB.DBName + Utilities.clsView.View_WorkSpaceNodeDetail, wstr , Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                    Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsView.View_WorkSpaceUserNodeDetail, wstr, Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                }
                else
                {
                    if (_MIDicomProjectDetailDTO.MODE != null)
                    {
                        ht.Clear();
                        ht.Add(Utilities.clsParameters.vWorkspaceId, _MIDicomProjectDetailDTO.vWorkSpaceID);
                        ht.Add(Utilities.clsParameters.iPeriod, _MIDicomProjectDetailDTO.iPeriod);
                        ht.Add(Utilities.clsParameters.iParentNodeId, _MIDicomProjectDetailDTO.iParentNodeId);
                        ht.Add(Utilities.clsParameters.vUserTypeCode, _MIDicomProjectDetailDTO.vUserTypeCode);
                        ht.Add(Utilities.clsParameters.vSubjectId, _MIDicomProjectDetailDTO.vSubjectId);
                        ht.Add(Utilities.clsParameters.iUserId, _MIDicomProjectDetailDTO.iUserId);
                        ht.Add(Utilities.clsParameters.MODE, _MIDicomProjectDetailDTO.MODE);
                        ht.Add(Utilities.clsParameters.vActivityName, _MIDicomProjectDetailDTO.vActivityName);

                        Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Proc_GetMIWorkSpaceUserNodeDetailWtihDataEntryStatus, ht);
                    }
                    else
                    {
                        wstr = "vWorkspaceId = '" + _MIDicomProjectDetailDTO.vWorkSpaceID + "' And iPeriod = '" + _MIDicomProjectDetailDTO.iPeriod + "' And iParentNodeId = '" + _MIDicomProjectDetailDTO.iParentNodeId + "' And vUserTypeCode = '" + _MIDicomProjectDetailDTO.vUserTypeCode + "' And cStatusIndi <> 'D' order by iNodeNo, iNodeId OPTION (MAXDOP 1)";
                        Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsView.View_WorkSpaceUserNodeDetail, wstr, Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                    }
                }

            }
            catch (Exception e)
            {
            }
            finally
            {
            }

            if (_MIDicomProjectDetailDTO.MODE != null)
            {
                return Sql_Ds.Tables[0];
            }
            else
            {
                return Sql_Dt;
            }

        }

        public String CRFDataEntryStatus(MICRFDataEntryStatus _MICRFDataEntryStatus)
        {
            string data = String.Empty;
            try
            {
                ht.Clear();
                ArrayList DataReturnList = new ArrayList();
                string eStr = "";
                bool result;

                ht.Add(Utilities.clsParameters.MODE, _MICRFDataEntryStatus.MODE);
                ht.Add(Utilities.clsParameters.vParentWorkSpaceId, _MICRFDataEntryStatus.vParentWorkSpaceId);
                ht.Add(Utilities.clsParameters.vWorkspaceId, _MICRFDataEntryStatus.vWorkspaceId);
                ht.Add(Utilities.clsParameters.vSubjectId, _MICRFDataEntryStatus.vSubjectId);
                ht.Add(Utilities.clsParameters.iMySubjectNo, _MICRFDataEntryStatus.iMySubjectNo);
                ht.Add(Utilities.clsParameters.ScreenNo, _MICRFDataEntryStatus.ScreenNo);
                ht.Add(Utilities.clsParameters.Radiologist, _MICRFDataEntryStatus.Radiologist);
                ht.Add(Utilities.clsParameters.Activity, _MICRFDataEntryStatus.Activity);
                //ht.Add(Utilities.clsParameters.vActivityName, _MICRFDataEntryStatus.vActivityName);
                //ht.Add(Utilities.clsParameters.cRadiologist, _MICRFDataEntryStatus.cRadiologist);
                //ht.Add(Utilities.clsParameters.returnValue, _MICRFDataEntryStatus.returnValue);
                //ht.Add(Utilities.clsParameters.returnResult, _MICRFDataEntryStatus.returnResult);
                //result = _SqlHelper.SaveInDb(Utilities.clsSp.Proc_GetCRFDataEntryStatus, ht, 1, ref DataReturnList, ref eStr);
                //result = _SqlHelper.SaveInDb(Utilities.clsSp.Proc_GetCRFDataEntryStatusNew, ht, 1, ref DataReturnList, ref eStr);
                result = _SqlHelper.SaveInDb(Utilities.clsSp.Proc_GetCRFDataEntryStatusDetail, ht, 1, ref DataReturnList, ref eStr);
                if (result)
                {
                    if ((Convert.ToString(DataReturnList[0])) != "")
                    {
                        if (Convert.ToInt32(DataReturnList[0]) == 1)
                        {
                            //data = "success" + "#" + Convert.ToString(DataReturnList[2]);
                            data = "success" + "#" + Convert.ToString(DataReturnList[2]) + "@" + Convert.ToString(DataReturnList[3]);

                        }
                        //else if (Convert.ToInt32(DataReturnList[0]) == 2)
                        //{
                        //    data = Convert.ToString(DataReturnList[1]);

                        //}
                        else
                        {
                            //data = Convert.ToString(DataReturnList[1]) + "#" + Convert.ToString(DataReturnList[2]);
                            data = Convert.ToString(DataReturnList[1]) + "#" + Convert.ToString(DataReturnList[2]) + "@" + Convert.ToString(DataReturnList[3]);

                        }
                    }
                    else
                    {
                        data = "NO-DATA";
                    }
                }
                else
                {
                    data = "error";
                }

            }
            catch (Exception e)
            {
            }
            finally
            {
            }
            return data;
        }

        public DataTable LesionStatistics(MILesionStatistics _MILesionStatistics)
        {
            DataTable dtReturn = new DataTable();
            try
            {
                string Wstr = string.Empty;
                if (_MILesionStatistics.Type == "BL-TL")
                {
                    Wstr = "vParentWorkSpaceId = '" + _MILesionStatistics.vParentWorkSpaceId + "' And vWorkspaceId = '" + _MILesionStatistics.vWorkspaceId + "' And vSubjectId = '" + _MILesionStatistics.vSubjectId + "' And iMySubjectNo = '" + _MILesionStatistics.iMySubjectNo + "' And ScreenNo = '" + _MILesionStatistics.ScreenNo + "' And cSaveStatus = '" + _MILesionStatistics.cSaveStatus + "' And vMedExDesc LIKE '%" + _MILesionStatistics.vMedExDesc + "%' And vActivityName = '" + _MILesionStatistics.vActivityName + "' And vSubActivityName = '" + _MILesionStatistics.vSubActivityName + "' And cStatusIndi <> 'D'";
                }
                else if (_MILesionStatistics.Type == "TP2-TL")
                {
                    String[] MedExDesc;
                    MedExDesc = _MILesionStatistics.vMedExDesc.Split('_');
                    string MedExDescString = "(vMEdexdesc like '%" + MedExDesc[0] + "%' OR vMEdexdesc like '%" + MedExDesc[1] + "%' OR vMEdexdesc like '%" + MedExDesc[2] + "%')";
                    Wstr = "vParentWorkSpaceId = '" + _MILesionStatistics.vParentWorkSpaceId + "' And vWorkspaceId = '" + _MILesionStatistics.vWorkspaceId + "' And vSubjectId = '" + _MILesionStatistics.vSubjectId + "' And iMySubjectNo = '" + _MILesionStatistics.iMySubjectNo + "' And ScreenNo = '" + _MILesionStatistics.ScreenNo + "' And cSaveStatus = '" + _MILesionStatistics.cSaveStatus + "' And " + MedExDescString + " And vActivityName = '" + _MILesionStatistics.vActivityName + "' And vSubActivityName = '" + _MILesionStatistics.vSubActivityName + "' And cStatusIndi <> 'D'";
                }
                else if (_MILesionStatistics.Type == "TP3-TL")
                {
                    String[] MedExDesc;
                    MedExDesc = _MILesionStatistics.vMedExDesc.Split('_');
                    string MedExDescString = "(vMEdexdesc like '%" + MedExDesc[0] + "%' OR vMEdexdesc like '%" + MedExDesc[1] + "%' OR vMEdexdesc like '%" + MedExDesc[2] + "%' OR vMEdexdesc like '%" + MedExDesc[3] + "%' OR vMEdexdesc like '%" + MedExDesc[4] + "%')";
                    Wstr = "vParentWorkSpaceId = '" + _MILesionStatistics.vParentWorkSpaceId + "' And vWorkspaceId = '" + _MILesionStatistics.vWorkspaceId + "' And vSubjectId = '" + _MILesionStatistics.vSubjectId + "' And iMySubjectNo = '" + _MILesionStatistics.iMySubjectNo + "' And ScreenNo = '" + _MILesionStatistics.ScreenNo + "' And cSaveStatus = '" + _MILesionStatistics.cSaveStatus + "' And " + MedExDescString + " And vActivityName = '" + _MILesionStatistics.vActivityName + "' And vSubActivityName = '" + _MILesionStatistics.vSubActivityName + "' And cStatusIndi <> 'D'";
                }
                else if (_MILesionStatistics.Type == "TP4-TL")
                {
                    String[] MedExDesc;
                    MedExDesc = _MILesionStatistics.vMedExDesc.Split('_');
                    string MedExDescString = "(vMEdexdesc like '%" + MedExDesc[0] + "%' OR vMEdexdesc like '%" + MedExDesc[1] + "%' OR vMEdexdesc like '%" + MedExDesc[2] + "%' OR vMEdexdesc like '%" + MedExDesc[3] + "%' OR vMEdexdesc like '%" + MedExDesc[4] + "%' OR vMEdexdesc like '%" + MedExDesc[5] + "%' OR vMEdexdesc like '%" + MedExDesc[6] + "%')";
                    Wstr = "vParentWorkSpaceId = '" + _MILesionStatistics.vParentWorkSpaceId + "' And vWorkspaceId = '" + _MILesionStatistics.vWorkspaceId + "' And vSubjectId = '" + _MILesionStatistics.vSubjectId + "' And iMySubjectNo = '" + _MILesionStatistics.iMySubjectNo + "' And ScreenNo = '" + _MILesionStatistics.ScreenNo + "' And cSaveStatus = '" + _MILesionStatistics.cSaveStatus + "' And " + MedExDescString + " And vActivityName = '" + _MILesionStatistics.vActivityName + "' And vSubActivityName = '" + _MILesionStatistics.vSubActivityName + "' And cStatusIndi <> 'D'";
                }
                else if (_MILesionStatistics.Type == "TP5-TL")
                {
                    String[] MedExDesc;
                    MedExDesc = _MILesionStatistics.vMedExDesc.Split('_');
                    string MedExDescString = "(vMEdexdesc like '%" + MedExDesc[0] + "%' OR vMEdexdesc like '%" + MedExDesc[1] + "%' OR vMEdexdesc like '%" + MedExDesc[2] + "%' OR vMEdexdesc like '%" + MedExDesc[3] + "%' OR vMEdexdesc like '%" + MedExDesc[4] + "%' OR vMEdexdesc like '%" + MedExDesc[5] + "%' OR vMEdexdesc like '%" + MedExDesc[6] + "%' OR vMEdexdesc like '%" + MedExDesc[7] + "%' OR vMEdexdesc like '%" + MedExDesc[8] + "%')";
                    Wstr = "vParentWorkSpaceId = '" + _MILesionStatistics.vParentWorkSpaceId + "' And vWorkspaceId = '" + _MILesionStatistics.vWorkspaceId + "' And vSubjectId = '" + _MILesionStatistics.vSubjectId + "' And iMySubjectNo = '" + _MILesionStatistics.iMySubjectNo + "' And ScreenNo = '" + _MILesionStatistics.ScreenNo + "' And cSaveStatus = '" + _MILesionStatistics.cSaveStatus + "' And " + MedExDescString + " And vActivityName = '" + _MILesionStatistics.vActivityName + "' And vSubActivityName = '" + _MILesionStatistics.vSubActivityName + "' And cStatusIndi <> 'D'";
                }
                else if (_MILesionStatistics.Type == "TP6-TL")
                {
                    String[] MedExDesc;
                    MedExDesc = _MILesionStatistics.vMedExDesc.Split('_');
                    string MedExDescString = "(vMEdexdesc like '%" + MedExDesc[0] + "%' OR vMEdexdesc like '%" + MedExDesc[1] + "%' OR vMEdexdesc like '%" + MedExDesc[2] + "%' OR vMEdexdesc like '%" + MedExDesc[3] + "%' OR vMEdexdesc like '%" + MedExDesc[4] + "%' OR vMEdexdesc like '%" + MedExDesc[5] + "%' OR vMEdexdesc like '%" + MedExDesc[6] + "%' OR vMEdexdesc like '%" + MedExDesc[7] + "%' OR vMEdexdesc like '%" + MedExDesc[8] + "%' OR vMEdexdesc like '%" + MedExDesc[9] + "%' OR vMEdexdesc like '%" + MedExDesc[10] + "%')";
                    Wstr = "vParentWorkSpaceId = '" + _MILesionStatistics.vParentWorkSpaceId + "' And vWorkspaceId = '" + _MILesionStatistics.vWorkspaceId + "' And vSubjectId = '" + _MILesionStatistics.vSubjectId + "' And iMySubjectNo = '" + _MILesionStatistics.iMySubjectNo + "' And ScreenNo = '" + _MILesionStatistics.ScreenNo + "' And cSaveStatus = '" + _MILesionStatistics.cSaveStatus + "' And " + MedExDescString + " And vActivityName = '" + _MILesionStatistics.vActivityName + "' And vSubActivityName = '" + _MILesionStatistics.vSubActivityName + "' And cStatusIndi <> 'D'";
                }
                else if (_MILesionStatistics.Type == "TP7-TL")
                {
                    String[] MedExDesc;
                    MedExDesc = _MILesionStatistics.vMedExDesc.Split('_');
                    string MedExDescString = "(vMEdexdesc like '%" + MedExDesc[0] + "%' OR vMEdexdesc like '%" + MedExDesc[1] + "%' OR vMEdexdesc like '%" + MedExDesc[2] + "%' OR vMEdexdesc like '%" + MedExDesc[3] + "%' OR vMEdexdesc like '%" + MedExDesc[4] + "%' OR vMEdexdesc like '%" + MedExDesc[5] + "%' OR vMEdexdesc like '%" + MedExDesc[6] + "%' OR vMEdexdesc like '%" + MedExDesc[7] + "%' OR vMEdexdesc like '%" + MedExDesc[8] + "%' OR vMEdexdesc like '%" + MedExDesc[9] + "%' OR vMEdexdesc like '%" + MedExDesc[10] + "%' OR vMEdexdesc like '%" + MedExDesc[11] + "%' OR vMEdexdesc like '%" + MedExDesc[12] + "%')";
                    Wstr = "vParentWorkSpaceId = '" + _MILesionStatistics.vParentWorkSpaceId + "' And vWorkspaceId = '" + _MILesionStatistics.vWorkspaceId + "' And vSubjectId = '" + _MILesionStatistics.vSubjectId + "' And iMySubjectNo = '" + _MILesionStatistics.iMySubjectNo + "' And ScreenNo = '" + _MILesionStatistics.ScreenNo + "' And cSaveStatus = '" + _MILesionStatistics.cSaveStatus + "' And " + MedExDescString + " And vActivityName = '" + _MILesionStatistics.vActivityName + "' And vSubActivityName = '" + _MILesionStatistics.vSubActivityName + "' And cStatusIndi <> 'D'";
                }
                else if (_MILesionStatistics.Type == "TP8-TL")
                {
                    String[] MedExDesc;
                    MedExDesc = _MILesionStatistics.vMedExDesc.Split('_');
                    string MedExDescString = "(vMEdexdesc like '%" + MedExDesc[0] + "%' OR vMEdexdesc like '%" + MedExDesc[1] + "%' OR vMEdexdesc like '%" + MedExDesc[2] + "%' OR vMEdexdesc like '%" + MedExDesc[3] + "%' OR vMEdexdesc like '%" + MedExDesc[4] + "%' OR vMEdexdesc like '%" + MedExDesc[5] + "%' OR vMEdexdesc like '%" + MedExDesc[6] + "%' OR vMEdexdesc like '%" + MedExDesc[7] + "%' OR vMEdexdesc like '%" + MedExDesc[8] + "%' OR vMEdexdesc like '%" + MedExDesc[9] + "%' OR vMEdexdesc like '%" + MedExDesc[10] + "%' OR vMEdexdesc like '%" + MedExDesc[11] + "%' OR vMEdexdesc like '%" + MedExDesc[12] + "%' OR vMEdexdesc like '%" + MedExDesc[13] + "%' OR vMEdexdesc like '%" + MedExDesc[14] + "%')";
                    Wstr = "vParentWorkSpaceId = '" + _MILesionStatistics.vParentWorkSpaceId + "' And vWorkspaceId = '" + _MILesionStatistics.vWorkspaceId + "' And vSubjectId = '" + _MILesionStatistics.vSubjectId + "' And iMySubjectNo = '" + _MILesionStatistics.iMySubjectNo + "' And ScreenNo = '" + _MILesionStatistics.ScreenNo + "' And cSaveStatus = '" + _MILesionStatistics.cSaveStatus + "' And " + MedExDescString + " And vActivityName = '" + _MILesionStatistics.vActivityName + "' And vSubActivityName = '" + _MILesionStatistics.vSubActivityName + "' And cStatusIndi <> 'D'";
                }
                else if (_MILesionStatistics.Type == "TP9-TL")
                {
                    String[] MedExDesc;
                    MedExDesc = _MILesionStatistics.vMedExDesc.Split('_');
                    string MedExDescString = "(vMEdexdesc like '%" + MedExDesc[0] + "%' OR vMEdexdesc like '%" + MedExDesc[1] + "%' OR vMEdexdesc like '%" + MedExDesc[2] + "%' OR vMEdexdesc like '%" + MedExDesc[3] + "%' OR vMEdexdesc like '%" + MedExDesc[4] + "%' OR vMEdexdesc like '%" + MedExDesc[5] + "%' OR vMEdexdesc like '%" + MedExDesc[6] + "%' OR vMEdexdesc like '%" + MedExDesc[7] + "%' OR vMEdexdesc like '%" + MedExDesc[8] + "%' OR vMEdexdesc like '%" + MedExDesc[9] + "%' OR vMEdexdesc like '%" + MedExDesc[10] + "%' OR vMEdexdesc like '%" + MedExDesc[11] + "%' OR vMEdexdesc like '%" + MedExDesc[12] + "%' OR vMEdexdesc like '%" + MedExDesc[13] + "%' OR vMEdexdesc like '%" + MedExDesc[14] + "%' OR vMEdexdesc like '%" + MedExDesc[15] + "%' OR vMEdexdesc like '%" + MedExDesc[16] + "%')";
                    Wstr = "vParentWorkSpaceId = '" + _MILesionStatistics.vParentWorkSpaceId + "' And vWorkspaceId = '" + _MILesionStatistics.vWorkspaceId + "' And vSubjectId = '" + _MILesionStatistics.vSubjectId + "' And iMySubjectNo = '" + _MILesionStatistics.iMySubjectNo + "' And ScreenNo = '" + _MILesionStatistics.ScreenNo + "' And cSaveStatus = '" + _MILesionStatistics.cSaveStatus + "' And " + MedExDescString + " And vActivityName = '" + _MILesionStatistics.vActivityName + "' And vSubActivityName = '" + _MILesionStatistics.vSubActivityName + "' And cStatusIndi <> 'D'";
                }
                else if (_MILesionStatistics.Type == "TP10-TL")
                {
                    String[] MedExDesc;
                    MedExDesc = _MILesionStatistics.vMedExDesc.Split('_');
                    string MedExDescString = "(vMEdexdesc like '%" + MedExDesc[0] + "%' OR vMEdexdesc like '%" + MedExDesc[1] + "%' OR vMEdexdesc like '%" + MedExDesc[2] + "%' OR vMEdexdesc like '%" + MedExDesc[3] + "%' OR vMEdexdesc like '%" + MedExDesc[4] + "%' OR vMEdexdesc like '%" + MedExDesc[5] + "%' OR vMEdexdesc like '%" + MedExDesc[6] + "%' OR vMEdexdesc like '%" + MedExDesc[7] + "%' OR vMEdexdesc like '%" + MedExDesc[8] + "%' OR vMEdexdesc like '%" + MedExDesc[9] + "%' OR vMEdexdesc like '%" + MedExDesc[10] + "%' OR vMEdexdesc like '%" + MedExDesc[11] + "%' OR vMEdexdesc like '%" + MedExDesc[12] + "%' OR vMEdexdesc like '%" + MedExDesc[13] + "%' OR vMEdexdesc like '%" + MedExDesc[14] + "%' OR vMEdexdesc like '%" + MedExDesc[15] + "%' OR vMEdexdesc like '%" + MedExDesc[16] + "%' OR vMEdexdesc like '%" + MedExDesc[17] + "%' OR vMEdexdesc like '%" + MedExDesc[18] + "%')";
                    Wstr = "vParentWorkSpaceId = '" + _MILesionStatistics.vParentWorkSpaceId + "' And vWorkspaceId = '" + _MILesionStatistics.vWorkspaceId + "' And vSubjectId = '" + _MILesionStatistics.vSubjectId + "' And iMySubjectNo = '" + _MILesionStatistics.iMySubjectNo + "' And ScreenNo = '" + _MILesionStatistics.ScreenNo + "' And cSaveStatus = '" + _MILesionStatistics.cSaveStatus + "' And " + MedExDescString + " And vActivityName = '" + _MILesionStatistics.vActivityName + "' And vSubActivityName = '" + _MILesionStatistics.vSubActivityName + "' And cStatusIndi <> 'D'";
                }
                else if (_MILesionStatistics.Type == "TP11-TL")
                {
                    String[] MedExDesc;
                    MedExDesc = _MILesionStatistics.vMedExDesc.Split('_');
                    string MedExDescString = "(vMEdexdesc like '%" + MedExDesc[0] + "%' OR vMEdexdesc like '%" + MedExDesc[1] + "%' OR vMEdexdesc like '%" + MedExDesc[2] + "%' OR vMEdexdesc like '%" + MedExDesc[3] + "%' OR vMEdexdesc like '%" + MedExDesc[4] + "%' OR vMEdexdesc like '%" + MedExDesc[5] + "%' OR vMEdexdesc like '%" + MedExDesc[6] + "%' OR vMEdexdesc like '%" + MedExDesc[7] + "%' OR vMEdexdesc like '%" + MedExDesc[8] + "%' OR vMEdexdesc like '%" + MedExDesc[9] + "%' OR vMEdexdesc like '%" + MedExDesc[10] + "%' OR vMEdexdesc like '%" + MedExDesc[11] + "%' OR vMEdexdesc like '%" + MedExDesc[12] + "%' OR vMEdexdesc like '%" + MedExDesc[13] + "%' OR vMEdexdesc like '%" + MedExDesc[14] + "%' OR vMEdexdesc like '%" + MedExDesc[15] + "%' OR vMEdexdesc like '%" + MedExDesc[16] + "%' OR vMEdexdesc like '%" + MedExDesc[17] + "%' OR vMEdexdesc like '%" + MedExDesc[18] + "%' OR vMEdexdesc like '%" + MedExDesc[19] + "%' OR vMEdexdesc like '%" + MedExDesc[20] + "%')";
                    Wstr = "vParentWorkSpaceId = '" + _MILesionStatistics.vParentWorkSpaceId + "' And vWorkspaceId = '" + _MILesionStatistics.vWorkspaceId + "' And vSubjectId = '" + _MILesionStatistics.vSubjectId + "' And iMySubjectNo = '" + _MILesionStatistics.iMySubjectNo + "' And ScreenNo = '" + _MILesionStatistics.ScreenNo + "' And cSaveStatus = '" + _MILesionStatistics.cSaveStatus + "' And " + MedExDescString + " And vActivityName = '" + _MILesionStatistics.vActivityName + "' And vSubActivityName = '" + _MILesionStatistics.vSubActivityName + "' And cStatusIndi <> 'D'";
                }
                else if (_MILesionStatistics.Type == "TP12-TL")
                {
                    String[] MedExDesc;
                    MedExDesc = _MILesionStatistics.vMedExDesc.Split('_');
                    string MedExDescString = "(vMEdexdesc like '%" + MedExDesc[0] + "%' OR vMEdexdesc like '%" + MedExDesc[1] + "%' OR vMEdexdesc like '%" + MedExDesc[2] + "%' OR vMEdexdesc like '%" + MedExDesc[3] + "%' OR vMEdexdesc like '%" + MedExDesc[4] + "%' OR vMEdexdesc like '%" + MedExDesc[5] + "%' OR vMEdexdesc like '%" + MedExDesc[6] + "%' OR vMEdexdesc like '%" + MedExDesc[7] + "%' OR vMEdexdesc like '%" + MedExDesc[8] + "%' OR vMEdexdesc like '%" + MedExDesc[9] + "%' OR vMEdexdesc like '%" + MedExDesc[10] + "%' OR vMEdexdesc like '%" + MedExDesc[11] + "%' OR vMEdexdesc like '%" + MedExDesc[12] + "%' OR vMEdexdesc like '%" + MedExDesc[13] + "%' OR vMEdexdesc like '%" + MedExDesc[14] + "%' OR vMEdexdesc like '%" + MedExDesc[15] + "%' OR vMEdexdesc like '%" + MedExDesc[16] + "%' OR vMEdexdesc like '%" + MedExDesc[17] + "%' OR vMEdexdesc like '%" + MedExDesc[18] + "%' OR vMEdexdesc like '%" + MedExDesc[19] + "%' OR vMEdexdesc like '%" + MedExDesc[20] + "%' OR vMEdexdesc like '%" + MedExDesc[21] + "%' OR vMEdexdesc like '%" + MedExDesc[22] + "%')";
                    Wstr = "vParentWorkSpaceId = '" + _MILesionStatistics.vParentWorkSpaceId + "' And vWorkspaceId = '" + _MILesionStatistics.vWorkspaceId + "' And vSubjectId = '" + _MILesionStatistics.vSubjectId + "' And iMySubjectNo = '" + _MILesionStatistics.iMySubjectNo + "' And ScreenNo = '" + _MILesionStatistics.ScreenNo + "' And cSaveStatus = '" + _MILesionStatistics.cSaveStatus + "' And " + MedExDescString + " And vActivityName = '" + _MILesionStatistics.vActivityName + "' And vSubActivityName = '" + _MILesionStatistics.vSubActivityName + "' And cStatusIndi <> 'D'";
                }
                else if (_MILesionStatistics.Type == "TP13-TL")
                {
                    String[] MedExDesc;
                    MedExDesc = _MILesionStatistics.vMedExDesc.Split('_');
                    string MedExDescString = "(vMEdexdesc like '%" + MedExDesc[0] + "%' OR vMEdexdesc like '%" + MedExDesc[1] + "%' OR vMEdexdesc like '%" + MedExDesc[2] + "%' OR vMEdexdesc like '%" + MedExDesc[3] + "%' OR vMEdexdesc like '%" + MedExDesc[4] + "%' OR vMEdexdesc like '%" + MedExDesc[5] + "%' OR vMEdexdesc like '%" + MedExDesc[6] + "%' OR vMEdexdesc like '%" + MedExDesc[7] + "%' OR vMEdexdesc like '%" + MedExDesc[8] + "%' OR vMEdexdesc like '%" + MedExDesc[9] + "%' OR vMEdexdesc like '%" + MedExDesc[10] + "%' OR vMEdexdesc like '%" + MedExDesc[11] + "%' OR vMEdexdesc like '%" + MedExDesc[12] + "%' OR vMEdexdesc like '%" + MedExDesc[13] + "%' OR vMEdexdesc like '%" + MedExDesc[14] + "%' OR vMEdexdesc like '%" + MedExDesc[15] + "%' OR vMEdexdesc like '%" + MedExDesc[16] + "%' OR vMEdexdesc like '%" + MedExDesc[17] + "%' OR vMEdexdesc like '%" + MedExDesc[18] + "%' OR vMEdexdesc like '%" + MedExDesc[19] + "%' OR vMEdexdesc like '%" + MedExDesc[20] + "%' OR vMEdexdesc like '%" + MedExDesc[21] + "%' OR vMEdexdesc like '%" + MedExDesc[22] + "%' OR vMEdexdesc like '%" + MedExDesc[23] + "%' OR vMEdexdesc like '%" + MedExDesc[24] + "%')";
                    Wstr = "vParentWorkSpaceId = '" + _MILesionStatistics.vParentWorkSpaceId + "' And vWorkspaceId = '" + _MILesionStatistics.vWorkspaceId + "' And vSubjectId = '" + _MILesionStatistics.vSubjectId + "' And iMySubjectNo = '" + _MILesionStatistics.iMySubjectNo + "' And ScreenNo = '" + _MILesionStatistics.ScreenNo + "' And cSaveStatus = '" + _MILesionStatistics.cSaveStatus + "' And " + MedExDescString + " And vActivityName = '" + _MILesionStatistics.vActivityName + "' And vSubActivityName = '" + _MILesionStatistics.vSubActivityName + "' And cStatusIndi <> 'D'";
                }
                else if (_MILesionStatistics.Type == "TP14-TL")
                {
                    String[] MedExDesc;
                    MedExDesc = _MILesionStatistics.vMedExDesc.Split('_');
                    string MedExDescString = "(vMEdexdesc like '%" + MedExDesc[0] + "%' OR vMEdexdesc like '%" + MedExDesc[1] + "%' OR vMEdexdesc like '%" + MedExDesc[2] + "%' OR vMEdexdesc like '%" + MedExDesc[3] + "%' OR vMEdexdesc like '%" + MedExDesc[4] + "%' OR vMEdexdesc like '%" + MedExDesc[5] + "%' OR vMEdexdesc like '%" + MedExDesc[6] + "%' OR vMEdexdesc like '%" + MedExDesc[7] + "%' OR vMEdexdesc like '%" + MedExDesc[8] + "%' OR vMEdexdesc like '%" + MedExDesc[9] + "%' OR vMEdexdesc like '%" + MedExDesc[10] + "%' OR vMEdexdesc like '%" + MedExDesc[11] + "%' OR vMEdexdesc like '%" + MedExDesc[12] + "%' OR vMEdexdesc like '%" + MedExDesc[13] + "%' OR vMEdexdesc like '%" + MedExDesc[14] + "%' OR vMEdexdesc like '%" + MedExDesc[15] + "%' OR vMEdexdesc like '%" + MedExDesc[16] + "%' OR vMEdexdesc like '%" + MedExDesc[17] + "%' OR vMEdexdesc like '%" + MedExDesc[18] + "%' OR vMEdexdesc like '%" + MedExDesc[19] + "%' OR vMEdexdesc like '%" + MedExDesc[20] + "%' OR vMEdexdesc like '%" + MedExDesc[21] + "%' OR vMEdexdesc like '%" + MedExDesc[22] + "%' OR vMEdexdesc like '%" + MedExDesc[23] + "%' OR vMEdexdesc like '%" + MedExDesc[24] + "%' OR vMEdexdesc like '%" + MedExDesc[25] + "%' OR vMEdexdesc like '%" + MedExDesc[26] + "%')";
                    Wstr = "vParentWorkSpaceId = '" + _MILesionStatistics.vParentWorkSpaceId + "' And vWorkspaceId = '" + _MILesionStatistics.vWorkspaceId + "' And vSubjectId = '" + _MILesionStatistics.vSubjectId + "' And iMySubjectNo = '" + _MILesionStatistics.iMySubjectNo + "' And ScreenNo = '" + _MILesionStatistics.ScreenNo + "' And cSaveStatus = '" + _MILesionStatistics.cSaveStatus + "' And " + MedExDescString + " And vActivityName = '" + _MILesionStatistics.vActivityName + "' And vSubActivityName = '" + _MILesionStatistics.vSubActivityName + "' And cStatusIndi <> 'D'";
                }

                dtReturn = _SqlHelper.GetDataTable("CRFTempDetail", Wstr, Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return dtReturn;
        }

        public DataTable LesionStatisticsDetails(MILesionStatisticsDetails _MILesionStatisticsDetails)
        {
            ArrayList dataReturnList = new ArrayList();
            string Wstr = string.Empty;
            string estr = string.Empty;
            string result = string.Empty;
            try
            {
                Wstr = "MODE = '" + _MILesionStatisticsDetails.MODE + "' AND vParentWorkspaceId = '" + _MILesionStatisticsDetails.vParentWorkSpaceId + "' AND vWorkspaceId = '" + _MILesionStatisticsDetails.vWorkspaceId + "' AND vSubjectId = '" + _MILesionStatisticsDetails.vSubjectId + "' AND iMySubjectNo = '" + _MILesionStatisticsDetails.iMySubjectNo + "' AND ScreenNo = '" + _MILesionStatisticsDetails.ScreenNo + "' AND vSubActivityName = '" + _MILesionStatisticsDetails.vSubActivity + "' AND vActivityName = '" + _MILesionStatisticsDetails.vActivity + "' AND vParentActivityId = '" + _MILesionStatisticsDetails.vParentActivityId + "' AND iParentNodeId = '" + _MILesionStatisticsDetails.iParentNodeId + "' AND vActivityId = '" + _MILesionStatisticsDetails.vActivityId + "' AND iNodeId = '" + _MILesionStatisticsDetails.iNodeId + "' AND cSaveStatus = '" + _MILesionStatisticsDetails.cSaveStatus + "' ";
                ht.Clear();

                ht.Add(Utilities.clsParameters.MODE, _MILesionStatisticsDetails.MODE);
                ht.Add(Utilities.clsParameters.vParentWorkSpaceId, _MILesionStatisticsDetails.vParentWorkSpaceId);
                ht.Add(Utilities.clsParameters.vWorkspaceId, _MILesionStatisticsDetails.vWorkspaceId);
                ht.Add(Utilities.clsParameters.vSubjectId, _MILesionStatisticsDetails.vSubjectId);
                ht.Add(Utilities.clsParameters.iMySubjectNo, _MILesionStatisticsDetails.iMySubjectNo);
                ht.Add(Utilities.clsParameters.ScreenNo, _MILesionStatisticsDetails.ScreenNo);
                ht.Add(Utilities.clsParameters.Radiologist, _MILesionStatisticsDetails.Radiologist);
                ht.Add(Utilities.clsParameters.vSubActivity, _MILesionStatisticsDetails.vSubActivity);
                ht.Add(Utilities.clsParameters.vActivity, _MILesionStatisticsDetails.vActivity);
                ht.Add(Utilities.clsParameters.vParentActivityId, _MILesionStatisticsDetails.vParentActivityId);
                ht.Add(Utilities.clsParameters.iParentNodeId, _MILesionStatisticsDetails.iParentNodeId);
                ht.Add(Utilities.clsParameters.vActivityId, _MILesionStatisticsDetails.vActivityId);
                ht.Add(Utilities.clsParameters.iNodeId, _MILesionStatisticsDetails.iNodeId);
                ht.Add(Utilities.clsParameters.cSaveStatus, _MILesionStatisticsDetails.cSaveStatus);

                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Proc_GetPriviousVisitData, ht);

                //if (!_SqlHelper.SaveInDb(Utilities.clsSp.Proc_GetPriviousVisitData, ht, 1, ref dataReturnList, ref estr))
                //{
                //    result = (string)dataReturnList[1];
                //}
                //else
                //{
                //    result = "success";
                //}
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

        public DataTable NLDetails(MI_NLDetails _MI_NLDetails)
        {
            ArrayList dataReturnList = new ArrayList();
            string Wstr = string.Empty;
            string estr = string.Empty;
            string result = string.Empty;
            try
            {
                ht.Clear();
                ht.Add(Utilities.clsParameters.MODE, _MI_NLDetails.MODE);
                ht.Add(Utilities.clsParameters.vParentWorkSpaceId, _MI_NLDetails.vParentWorkSpaceId);
                ht.Add(Utilities.clsParameters.vWorkspaceId, _MI_NLDetails.vWorkspaceId);
                ht.Add(Utilities.clsParameters.vSubjectId, _MI_NLDetails.vSubjectId);
                ht.Add(Utilities.clsParameters.iMySubjectNo, _MI_NLDetails.iMySubjectNo);
                ht.Add(Utilities.clsParameters.ScreenNo, _MI_NLDetails.ScreenNo);
                ht.Add(Utilities.clsParameters.Radiologist, _MI_NLDetails.Radiologist);
                ht.Add(Utilities.clsParameters.vSubActivity, _MI_NLDetails.vSubActivity);
                ht.Add(Utilities.clsParameters.vActivity, _MI_NLDetails.vActivity);
                ht.Add(Utilities.clsParameters.vParentActivityId, _MI_NLDetails.vParentActivityId);
                ht.Add(Utilities.clsParameters.iParentNodeId, _MI_NLDetails.iParentNodeId);
                ht.Add(Utilities.clsParameters.vActivityId, _MI_NLDetails.vActivityId);
                ht.Add(Utilities.clsParameters.iNodeId, _MI_NLDetails.iNodeId);
                ht.Add(Utilities.clsParameters.cSaveStatus, _MI_NLDetails.cSaveStatus);

                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Proc_GetNLDetails, ht);

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

        public string MI_DataSaveStatus(MI_DataSaveStatus _MI_DataSaveStatus)
        {
            string returnData = string.Empty;
            ArrayList returnArrayList = new ArrayList();
            DataTable dtReturn = new DataTable();
            string eStr_Retu = string.Empty;
            Boolean result;
            try
            {
                ht.Clear();
                ht.Add(Utilities.clsParameters.vParentWorkSpaceId, _MI_DataSaveStatus.vParentWorkSpaceId);
                ht.Add(Utilities.clsParameters.vWorkspaceId, _MI_DataSaveStatus.vWorkspaceId);
                ht.Add(Utilities.clsParameters.vSubjectId, _MI_DataSaveStatus.vSubjectId);
                ht.Add(Utilities.clsParameters.cRadiologist, _MI_DataSaveStatus.cRadiologist);
                ht.Add(Utilities.clsParameters.vActivityId, _MI_DataSaveStatus.vActivityId);
                ht.Add(Utilities.clsParameters.iNodeId, _MI_DataSaveStatus.iNodeId);
                ht.Add(Utilities.clsParameters.vSubActivityId, _MI_DataSaveStatus.vSubActivityId);
                ht.Add(Utilities.clsParameters.iSubNodeId, _MI_DataSaveStatus.iSubNodeId);
                ht.Add(Utilities.clsParameters.vMySubjectNo, _MI_DataSaveStatus.vMySubjectNo);

                result = _SqlHelper.GetFromDB(Utilities.clsSp.Proc_GetDataSaveStatus, ht, ref returnArrayList, ref dtReturn, ref eStr_Retu, false);

                if (result)
                {
                    //returnData = (string)returnArrayList[1];
                    //returnData = (string)returnArrayList[1] + "#" + (string)returnArrayList[2] + "@" + (string)returnArrayList[3];
                    returnData = (string)returnArrayList[1] + "#" + (string)returnArrayList[2] + "@" + (string)returnArrayList[3] + "#" + (string)returnArrayList[4];
                }
                else
                {
                    returnData = "ERROR";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return returnData;
        }

        public DataTable MIStatisticReport(MIStatisticReportDTO _MIStatisticReportDTO)
        {
            try
            {
                ht.Clear();
                ht.Add(Utilities.clsParameters.vParentWorkspaceId, _MIStatisticReportDTO.vParentWorkSpaceId);
                ht.Add(Utilities.clsParameters.vWorkspaceId, _MIStatisticReportDTO.vWorkspaceId);
                ht.Add(Utilities.clsParameters.Mode, Convert.ToInt32(_MIStatisticReportDTO.imode));
                //Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Get_StatisticsReportData, ht);
                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Get_StatisticsReportDetail, ht);

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

        public DataTable MIStatisticReport1(MIStatisticReportOverAllDTO _MIStatisticReportOverAllDTO)
        {
            try
            {
                ht.Clear();
                ht.Add(Utilities.clsParameters.vWorkspaceId, _MIStatisticReportOverAllDTO.vWorkspaceId);
                ht.Add(Utilities.clsParameters.Mode, Convert.ToInt32(_MIStatisticReportOverAllDTO.imode));
                //Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Get_StatisticsReportData, ht);
                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Proc_AdjudicatorOverAllResponse, ht);

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

        public DataTable ProjectLockDetail(ProjectLockDetailDTO _ProjectLockDetailDTO)
        {
            string wstr = string.Empty;
            try
            {
                ht.Clear();
                wstr = "vWorkSpaceId = '" + _ProjectLockDetailDTO.vWorkspaceId + "' And cStatusIndi <> 'D' order by iTranno desc";
                Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsTable.CRFLockDtl, wstr, Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return Sql_Dt;
        }

        public DataSet getSubjectImageStudyDetails(MIImageReviewDetailDTO _MIImageReviewDetailDTO)
        {
            try
            {
                ht.Clear();
                ht.Add(Utilities.clsParameters.iImgTransmittalHdrId, _MIImageReviewDetailDTO.iImgTransmittalHdrId);
                ht.Add(Utilities.clsParameters.iImgTransmittalDtlId, _MIImageReviewDetailDTO.iImgTransmittalDtlId);
                ht.Add(Utilities.clsParameters.iImageStatus, _MIImageReviewDetailDTO.iImageStatus);
                ht.Add(Utilities.clsParameters.cRadiologist, _MIImageReviewDetailDTO.cRadiologist);
                ht.Add(Utilities.clsParameters.iImageTranNo, _MIImageReviewDetailDTO.ImageTransmittalImgDtl_iImageTranNo);
                ht.Add(Utilities.clsParameters.vParentActivityId, _MIImageReviewDetailDTO.vParentActivityId);
                ht.Add(Utilities.clsParameters.iParentNodeId, _MIImageReviewDetailDTO.iParentNodeId);
                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Proc_GetSubjectImageStudyDetail, ht);
                return Sql_Ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public DataTable ProjectFreezeDetail(ProjectFreezerDetailDTO _ProjectFreezerDetailDTO)
        {
            string wstr = string.Empty;
            try
            {
                ht.Clear();
                wstr = "vWorkSpaceId = '" + _ProjectFreezerDetailDTO.vWorkspaceId + "'";
                Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsView.View_CRFVersionForDataEntryControl, wstr, Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return Sql_Dt;
        }

        public DataTable SubjectDetailsForDISOFTWithDataEntryStatus(SubjectDTO _SubjectDTO)
        {
            try
            {
                ht.Clear();
                ht.Add(Utilities.clsParameters.vWorkspaceId, _SubjectDTO.vWorkSpaceID);
                ht.Add(Utilities.clsParameters.iUserId, _SubjectDTO.iUserId);
                Sql_Dt = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Proc_SubjectForDISOFTwithdataentrystatus, ht).Tables[0];
                return Sql_Dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public DataTable Datatable(DatatableDTO _DatatableDTO)
        {
            string wstr = string.Empty;
            try
            {
                ht.Clear();
                ht.Add(Utilities.clsParameters.vWorkspaceId, _DatatableDTO.vWorkspaceId);

                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(_DatatableDTO.SPName, ht);
                return Sql_Ds.Tables[0];

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return Sql_Dt;
        }

        public DataTable ProjectDashboardDetail(DashboardDetailDTO _DashboardDetailDTO)
        {

            try
            {
                ht.Clear();
                ht.Add(Utilities.clsParameters.vWorkspaceId, _DashboardDetailDTO.vWorkSpaceID);
                ht.Add(Utilities.clsParameters.iPeriod, _DashboardDetailDTO.iPeriod);
                ht.Add(Utilities.clsParameters.vUserTypeCode, _DashboardDetailDTO.vUserTypeCode);
                ht.Add(Utilities.clsParameters.iUserId, _DashboardDetailDTO.iUserId);

                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.Get_ProjectSubjectDetails_New, ht);
            }
            catch (Exception e)
            {
            }
            finally
            {
            }
            return Sql_Ds.Tables[0];
        }

        public DataTable GetPasswordPolicyData(ChangePasswordDTO _ChangePasswordDTO)
        {
            try
            {
                Sql_Dt = _SqlHelper.GetDataTable("PasswordPolicyMst", " cActiveFlag = 'Y' AND cStatusIndi <> 'D' ", Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                return Sql_Dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetPasswordHistory(ChangePasswordDTO _ChangePasswordDTO)
        {
            try
            {
                Sql_Dt = _SqlHelper.GetDataTable("PasswordHistory", " iUserID = " + _ChangePasswordDTO.ID + " AND cStatusIndi <> 'D' ORDER BY iSrNo DESC ", Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                return Sql_Dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetUserMst(ChangePasswordDTO _ChangePasswordDTO)
        {
            try
            {
                Sql_Dt = _SqlHelper.GetDataTable("UserMst", "cStatusIndi <> 'D' And vUserName ='" + _ChangePasswordDTO.Username + "'", Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                return Sql_Dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Insert_ChangePassword(ChangePasswordDTO _ChangePasswordDTO)
        {
            try
            {
                ht.Clear();
                ht.Add(Utilities.clsParameters.iUserId, _ChangePasswordDTO.ID);
                ht.Add(Utilities.clsParameters.vLoginPass, _ChangePasswordDTO.NewPassword);
                ht.Add(Utilities.clsParameters.iModifyBy, _ChangePasswordDTO.iModifyBy);
                ht.Add(Utilities.clsParameters.cStatusIndi, "N");
                ht.Add(Utilities.clsParameters.DataopMode, 1);

                int resultdata = _SqlHelper.ExecuteSP("Insert_ChangePassword", ht);
                return resultdata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetUser(OtpDTO _OtpDTO)
        {
            try
            {
                Sql_Dt = _SqlHelper.GetDataTable("UserMst", "cStatusIndi <> 'D' And vUserName ='" + _OtpDTO.Username + "' And vUserTypeCode ='" + _OtpDTO.vUserTypeCode + "'", Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                return Sql_Dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetOtp(OtpDTO _OtpDTO)
        {
            try
            {
                ht.Clear();
                ht.Add(Utilities.clsParameters.iUserId, _OtpDTO.UserId);

                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams("Proc_GetOTPInfo", ht);
                return Sql_Ds.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetSmsDetails(OtpDTO _OtpDTO)
        {
            try
            {
                Sql_Dt = _SqlHelper.GetDataTable("SMSGateWayDetail", "cStatusIndi <> 'D' And vSMSLocationCode ='" + _OtpDTO.vLocationCode + "'", Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);
                return Sql_Dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetEmailForTranstrion(MISubmitMIFinalLesionDTO _MISubmitMIFinalLesionData)
        {
            try
            {
                //ht.Add(Utilities.clsParameters.DataopMode, 1);                ht.Clear();
                ht.Add(Utilities.clsParameters.vWorkspaceId, _MISubmitMIFinalLesionData.vWorkspaceId);
                ht.Add(Utilities.clsParameters.vOperationcode, _MISubmitMIFinalLesionData.vOperationCode);

                Sql_Ds = _SqlHelper.ExecuteProcudereWithParams(Utilities.clsSp.GetEmailForTranstrion, ht);
            }
            catch (Exception e)
            {
            }
            finally
            {
            }
            return Sql_Ds.Tables[0];
        }

        public DataTable GetUserType(string ApplicationId)
        {
            string wstr = string.Empty;
            DataTable dtReturn = new DataTable();
            DataSet dsList = new DataSet();
            try
            {
                ht.Clear();
                wstr = "cStatusIndi <> 'D'";
                //Sql_Dt = _SqlHelper.GetDataTable(Utilities.clsTable.USERTYPEMST, wstr, Utilities.DataRetrievalModeEnum.DataTable_WithWhereCondition);

                string strReturn = "";
                if (!_SqlHelper.GetFieldsOfTable("USERTYPEMST", "vUserTypeCode AS Role, vUserTypeName AS RoleName,'' as ApplicationId", wstr, ref dsList, ref strReturn))
                {
                    strReturn = strReturn + " Error while getting USERTYPEMST";
                    throw new Exception(strReturn);
                }
                dtReturn = dsList.Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return dtReturn;
        }

    }
}
