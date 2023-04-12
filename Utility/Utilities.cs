using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Utility
{
    public static class Utilities
    {
        public enum DataRetrievalModeEnum
        {
            DataTable_Empty = 1,
            DataTable_AllRecords = 2,
            DataTable_WithWhereCondition = 3,
            DatatTable_Query = 4
        }

        public enum DataObjOpenSaveModeEnum
        {
            //Enumeration Declaration for Open and Save Mode from Menu Page
            DataObjOpenMode_None = 0,
            DataObjOpenMode_Add = 1,
            DataObjOpenMode_Edit = 2,
            DataObjOpenMode_Delete = 3,
            DataObjOpenMode_View = 4,
            DataObjOpenMode_Rearrange = 5

        }

        public static class clsDB
        {
            //public static string ApiUrl = "http://125.18.133.6:8081/DI_Soft_API/API/";
            //public static string ApiUrl = "http://125.18.133.6:8081/DI_SoftValid_API/API/";
            //public static string ApiUrl = "http://10.1.10.112/DI_Soft_API/API/";
            //public static string ApiUrl = "http://10.1.10.112/DI_SoftValid_API/API/";
            //public static string ApiUrl = "http://10.1.10.39/DI_API/API/";
            //public static string ApiUrl = "http://90.0.0.68/MI_API/API/";
            //public static string ApiUrl = "http://localhost:51606/";   

            //public static string WebUrl = "http://125.18.133.6:8081/DI_Soft/";
            //public static string WebUrl = "http://125.18.133.6:8081/DI_SoftValid/";
            //public static string WebUrl = "http://10.1.10.112/DI_Soft/";
            //public static string WebUrl = "http://10.1.10.112/DI_SoftValid/";
            //public static string WebUrl = "http://10.1.10.39/DI_Soft/";
            //public static string WebUrl = "http://localhost:51577/";

            //public static string DBName = System.Configuration.ConfigurationManager.AppSettings["DB_Name"];
            //public static string DBName = "BizNETDevelopment..";    
            //public static string DBName = "BizNETTest..";               
            //public static string DBName = "[10.1.10.70].[BizNETCTM5Valid].[dbo].";
            //public static string DBName = "[10.1.10.70].[BizNET].[dbo].";
            
        }

        public static class clsMethod
        {
            public static string SubmitMIFinalLesion = "SetData/SubmitMIFinalLesion";
            public static string SkipMIFinalLesion = "SetData/SkipMIFinalLesion";
            public static string SubmitMIFinalLesionData = "SetData/SubmitMIFinalLesionData";
            public static string SkipMIFinalLesionData = "SetData/SkipMIFinalLesionData";
            public static string SaveMIFinalLession = "SetData/SaveMIFinalLession";
            public static string SubjectDetailsForDISOFT = "CommonData/SubjectDetailsForDISOFT";    
        }

        public static class clsTable
        {
            public static string CRFLockDtl = "CRFLockDtl";
        }

        public static class clsSp
        {
            public static string Proc_login = "Proc_LoginForDISoft";
            public static string Proc_LoginFailureCheck = "Proc_LoginFailureCheck";
            public static string Proc_UserLoginDetails = "Proc_UserLoginDetails";
            public static string Insert_ModalityMst = "Insert_ModalityMst";
            public static string Insert_AnatomyMst = "Insert_AnatomyMst";
            public static string Proc_GetModalityAuditTail = "Proc_GetModalityAuditTail";
            public static string Proc_GetAnatomyAuditTail = "Proc_GetAnatomyAuditTail";
            public static string Proc_GetAuditTrail = "Proc_GetAuditTrail";
            public static string Proc_GetCheckListTemplate = "Proc_GetCheckListTemplate";
            public static string Proc_GetProjectVisitDetails = "Proc_GetProjectVisitDetails";
            public static string Insert_CheckListTemplateMst = "Insert_CheckListTemplateMst";
            public static string Insert_CheckListTemplateMstHdr = "Insert_CheckListTemplateMstHdr";
            public static string Insert_CheckListTemplateMstDtl = "Insert_CheckListTemplateMstDtl";
            public static string Insert_CheckListProjectMstHdr = "Insert_CheckListProjectMstHdr";
            public static string Insert_CheckListProjectMstDtl = "Insert_CheckListProjectMstDtl";
            public static string Insert_ImgTransmittalHdr = "Insert_ImgTransmittalHdr";
            public static string Insert_ImgTransmittalDtl = "Insert_ImgTransmittalDtl";
            public static string Insert_ImageTransmittalDetails = "Insert_ImageTransmittalDetails";
            public static string Insert_BizNetImageTransmittalDetails = "Insert_BizNetImageTransmittalDetails";
            public static string Insert_CRFTempDetail = "Insert_CRFTempDetail";
            public static string Pro_GetImgTransmittalHdr = "Pro_GetImgTransmittalHdr";
            public static string Pro_GetImgTransmittalDtl = "Pro_GetImgTransmittalDtl";
            public static string Pro_GetImageTransmittalImgDtl = "Pro_GetImageTransmittalImgDtl";
            public static string Proc_GetCRFDataEntryStatus = "Proc_GetCRFDataEntryStatus";
            public static string Proc_GetCRFDataEntryStatusNew = "Proc_GetCRFDataEntryStatusNew";
            public static string Proc_GetPriviousVisitData = "Proc_GetPriviousVisitData";
            public static string Proc_GetNLDetails = "Proc_GetNLDetails";
            public static string Proc_GetCRFDataEntryStatusDetail = "Proc_GetCRFDataEntryStatusDetail";
            public static string Proc_GetDataSaveStatus = "Proc_GetDataSaveStatus";
            public static string Pro_GetMAXiImageTranNo = "Pro_GetMAXiImageTranNo";
            public static string Proc_GetSubjectStudyDetail = "Proc_GetSubjectStudyDetail";
            public static string Get_StatisticsReportData = "Get_StatisticsReportData";
            public static string Get_StatisticsReportDetail = "Get_StatisticsReportDetail";
            public static string Proc_GetMIWorkSpaceUserNodeDetailWtihDataEntryStatus = "Proc_GetMIWorkSpaceUserNodeDetailWtihDataEntryStatus";
            public static string Proc_CheckVisitIsReviewedGetMAXiImageTranNo = "Proc_CheckVisitIsReviewedGetMAXiImageTranNo";   
         
            public static string Proc_GetSubjectImageStudyDetail = "Proc_GetSubjectImageStudyDetail";
            public static string Proc_AdjudicatorOverAllResponse = "Proc_AdjudicatorOverAllResponse";

            //For Subject Rejection
            public static string Proc_SubjectRejectionDetail = "Proc_SubjectRejectionDetail";
            public static string Proc_GetAuditTrailForDiSoft = "Proc_GetAuditTrailForDiSoft";
            public static string Proc_SubjectForDISOFTwithdataentrystatus = "Proc_SubjectForDISOFTwithdataentrystatus";
            public static string Proc_ViewGetSubjectImageStudyDetail = "Proc_ViewGetSubjectImageStudyDetail";

            public static string Proc_UserAuthentication = "Proc_UserAuthentication";
            public static string Get_ProjectSubjectDetails_New = "Get_ProjectSubjectDetails_New";
            public static string Proc_GetUserWiseProfile = "Proc_GetUserWiseProfile";
            public static string Pro_GetImgTransmittalDtlForAllReview = "Pro_GetImgTransmittalDtlForAllReview";
            public static string GetEmailForTranstrion = "GetEmailForTranstrion";
            public static string Pro_CertGetMAXiImageTranNo = "Pro_CertGetMAXiImageTranNo";
        }

        public static class clsView
        {
            public static string View_UserMst = "View_UserMst";
            public static string View_ModalityMst = "View_GetModalityMst";
            public static string View_AnatomyMst = "View_GetAnatomyMst";
            public static string View_UserWiseProfile = "View_UserWiseProfile";
            public static string View_Menu = "View_Menu";
            public static string View_Project = "View_Project";
            public static string View_Subject = "View_Subject";
            public static string View_GetSubjectStudyDetail = "View_GetSubjectStudyDetail";
            public static string View_GetSubjectImageStudyDetail = "View_GetSubjectImageStudyDetail";
            public static string View_MedExWorkSpaceDtl = "View_MedExWorkSpaceDtl";
            public static string View_WorkSpaceNodeDetail = "View_WorkSpaceNodeDetail";
            public static string View_WorkSpaceUserNodeDetail = "View_WorkSpaceUserNodeDetail";
            public static string View_GetCRFData = "View_GetCRFData";
            public static string View_GetCRFSubDtl = "View_GetCRFSubDtl";
            public static string View_SubjectForDISOFT = "View_SubjectForDISOFT";
            public static string View_CRFVersionForDataEntryControl = "View_CRFVersionForDataEntryControl";
            public static string View_GetRadioLogistData = "View_GetRadioLogistData";
            public static string View_CertificateMaster = "View_CertificateMaster";
            public static string View_CertificateMasterTransaction = "View_CertificateMasterTransaction";

        }

        public static class clsParameters
        {
            public static string vUserName = "@vUserName";
            public static string vLoginPass = "@vLoginPass";
            public static string iUserId = "@iUserId";
            public static string vUserTypeCode = "@vUserTypeCode";
            public static string vLocationCode = "@vLocationCode";
            public static string dModifyOn = "@dModifyOn";
            public static string nModifyBy = "@nModifyBy";
            public static string vModifyByname = "@vModifyByname";
            public static string vRemark = "@vRemark";
            public static string vRemarks = "@vRemarks";
            public static string iModifyBy = "@iModifyBy";
            public static string cStatusIndi = "@cStatusIndi";
            public static string vIPAddress = "@vIPAddress";            
            public static string vUserAgent = "@vUserAgent";
            public static string vUTCHourDiff = "@vUTCHourDiff";
            public static string DataopMode = "@DataopMode";
            public static string vOTPNo = "@vOTPNo";
            public static string dStartTime = "@dStartTime";
            public static string dEndTime = "@dEndTime";
            public static string IsActive = "@IsActive";

            // For ExMsgInfo 05_12_2022
            public static string vNotificationType = "@vNotificationType";
            public static string vSubject = "@vSubject";
            public static string vBody = "@vBody";
            public static string vFromEmailId = "@vFromEmailId";
            public static string vToEmailId = "@vToEmailId";
            public static string vCCEmailId = "@vCCEmailId";
            public static string vBCCEmailId = "@vBCCEmailId";
            public static string vAttachment = "@vAttachment";
            public static string cIsSent = "@cIsSent";
            public static string dSentDate = "@dSentDate";
            public static string dCreatedDate = "@dCreatedDate";
            public static string vPhoneNo = "@vPhoneNo";
            public static string iCreatedBy = "@iCreatedBy";

            //For Modality Master
            public static string nModalityNo = "@nModalityNo";
            public static string vModalityDesc = "@vModalityDesc";

            //For Anatomy Master
            public static string nAnatomyNo = "@nAnatomyNo";
            public static string vAnatomyDesc = "@vAnatomyDesc";
            
            //For Common Audit Trail
            public static string vTableName = "@vTableName";
            public static string vIdName = "@vIdName";
            public static string vIdValue = "@vIdValue";
            public static string vFieldName = "@vFieldName";
            public static string vMasterFieldName = "@vMasterFieldName";
            public static string vMasterTableName = "@vMasterTableName";
    
            //For CheckList Template & Question
            public static string type = "@type";
            public static string nTemplateHdrNo = "@nTemplateHdrNo";
            public static string vQuestion = "@vQuestion";

            //For Project Template & Question
            public static string nTemplateDtlNo = "@nTemplateDtlNo";

            //For Project Visit Detail
            public static string vWorkspaceId = "@vWorkspaceId";
            public static string ParentWorkspaceId = "@vParentWorkSpaceId";
            public static string ActivityId = "@vActivityId";
            public static string PeriodId = "@vPeriodId";
            public static string MySubjectNo = "@iMySubjectNo";
            public static string RepetationNo = "@iRepetationNo";


            //For Image Transmitall

            public static string iImgTransmittalHdrId = "@iImgTransmittalHdrId";
            public static string iImgTransmittalImgDtlId = "@iImgTransmittalImgDtlId"; 
            public static string vProjectNo = "@vProjectNo";
            public static string vSubjectId = "@vSubjectId";
            public static string vRandomizationNo = "@vRandomizationNo";
            public static string vMySubjectNo = "@vMySubjectNo";
            public static string vDOB = "@vDOB";
            public static string iNodeId = "@iNodeId";
            public static string cDeviation = "@cDeviation";
            public static string nvDeviationReason = "@nvDeviationReason";
            public static string nvInstructions = "@nvInstructions";           
            public static string iImgTransmittalDtlId = "@iImgTransmittalDtlId";
            public static string iModalityNo = "@iModalityNo";
            public static string iAnatomyNo = "@iAnatomyNo";
            public static string vAnatomyNo = "@vAnatomyNo"; 
            public static string cIVContrast = "@cIVContrast";
            public static string dExaminationDate = "@dExaminationDate";
            public static string iNoImages = "@iNoImages";
            public static string iImageMode = "@iImageMode";
            public static string TransmittalImgDtl = "@TransmittalImgDtl";

            public static string vFileName = "@vFileName";
            public static string vFileType = "@vFileType";
            public static string vServerPath = "@vServerPath";

            public static string vSize = "@vSize";
            public static string dScheduledDate = "@dScheduledDate";

            public static string iImageStatus = "@iImageStatus";
            public static string FromDate = "@FromDate";
            public static string ToDate = "@ToDate";

            public static string vLesionType = "@vLesionType";
            public static string vLesionSubType = "@vLesionSubType";

            public static string ImageTransmittalImgDtl_iImageTranNo = "@ImageTransmittalImgDtl_iImageTranNo";

            public static string vParentWorkspaceId = "@vParentWorkspaceId";
            public static string cOralContrast = "@cOralContrast";
            public static string vModalityNo = "@vModalityNo";
            //public static string vParentActivityId = "@vParentActivityId";
            //public static string iParentNodeId = "@iParentNodeId";
            //public static string vActivityId = "@vActivityId"; 


            //For Lesion Detail

            public static string vActivityId = "@vActivityId";
            public static string CRFTempDtl = "@CRFTempDtl";
            public static string DATAMODE = "@DATAMODE";            
            public static string iMySubjectNo = "@iMySubjectNo";
            public static string ScreenNo = "@ScreenNo";
            public static string Radiologist = "@Radiologist";
            public static string MedEx_Result = "@MedEx_Result";
            public static string vParentWorkSpaceId = "@vParentWorkSpaceId";
            public static string vPeriodId = "@vPeriodId";
            public static string vActivityName = "@vActivityName";
            public static string vSubActivityName = "@vSubActivityName";
            public static string MODE = "@MODE";
            public static string vSubActivity = "@vSubActivity";
            public static string vActivity = "@vActivity";
            public static string vParentActivityId = "@vParentActivityId";
            public static string cSaveStatus = "@cSaveStatus";
            

            //For Dicom Study Detail

            public static string iPeriod = "@iPeriod";
            public static string iParentNodeId = "@iParentNodeId";
            public static string vSubActivityId = "@vSubActivityId";
            public static string iSubNodeId = "@iSubNodeId";

            // For CRFDataEntryStatus
            public static string cRadiologist = "@cRadiologist";
            public static string Activity = "@Activity";
            public static string returnValue = "@returnValue";
            public static string returnResult = "@returnResult";      

            //MI PHASE 2
            public static string DicomAnnotationDtl = "@DicomAnnotationDtl";
            public static string iImageTranNo = "@iImageTranNo";

            //MI PHASE 2 Report
            public static string Mode = "@Mode";

            //For Set Project
            public static string vOperationcode = "@vOperationcode";

            // for Certificate
            public static string iCertificateMasterId = "@iCertificateMasterID";
            public static string iCertificateMasterImgDtlId = "@iCertificateMasterImgDtlId";
            public static string CertificateMasterImgDtl = "@CertificateMasterImgDtl";
            public static string CertificateMasterImgDtl_iImageTranNo = "@CertificateMasterImgDtl_iImageTranNo";
        }

        public static class clsVariables
        {
            public const string strServerOffset = " IST (+5:30 GMT)";
            public const string strServerOffset_EST = " EST (+5:30 GMT)";
            public const string IndiaStandardTime = "India Standard Time";
            public const string EasternStandardTime = "Eastern Standard Time";
            public const string OperationType = "MI";
        }

        public static string EncryptPassword(string password)
        {
            int len = password.Length;
            //setting len = length of the entered password
            string[] pwd = new string[len + 1];
            //declaring pwd as a string array of length len
            pwd[0] = password.ToString();
            //assigning password in pwd(0) 
            char[] cpwd = new char[len + 1];
            //cpwd as char array of the same length
            Int16 index = default(Int16);
            int[] Lascii = new int[len + 1];
            //Lascii(len) to store corresponding ascii of password
            int[] Sascii = new int[len + 1];
            //Sascii(len) to store value of substituted ascii
            string result = string.Empty;
            //to store the resulting appended string
            string Reverse = string.Empty;
            string retstr = string.Empty;
            //returning the obtained string
            int CountAdd = 0;
            cpwd = pwd[0].ToCharArray();

            for (index = 0; index <= len - 1; index++)
            {
                if (CountAdd == 4)
                {
                    CountAdd = 0;
                }
                //Lascii[index]= AscW[cpwd[index]];

                Lascii[index] = System.Convert.ToInt32(cpwd[index]);
                Sascii[index] = Lascii[index] + (CountAdd + 1);
                cpwd[index] = System.Convert.ToChar(Sascii[index]);
                result += cpwd[index];
                CountAdd += 1;
            }
            Reverse = result;
            retstr = ReversePassword(Reverse);
            return retstr;

        }

        public static string ReversePassword(string Password)
        {
            int len = Password.Length;
            int index;
            String Keytextbox = String.Empty;
            string key;
            char[] Pwd;
            string[] pwd1;
            Pwd = Password.ToCharArray();
            int intLen = (len - 1);
            char[] Reverse = new char[char.MaxValue];
            for (index = 0; (index <= intLen); index++)
            {
                Reverse[index] = Pwd[(intLen - index)];
                Keytextbox = (Keytextbox + Reverse[index]);
            }

            key = Keytextbox;
            return key;
        }

        public static string DecryptPassword(string revpassword)
        {
            string Password = ReversePassword(revpassword);
            int len = revpassword.Length;

            string pws = string.Empty;
            pws = Password.ToString();
            char[] cpwd = new char[len];
            int index;
            int[] Lascii = new int[len];
            int[] Sascii = new int[len];
            string result = string.Empty;
            string retstr = string.Empty;
            int CountAdd = 0;
            cpwd = pws.ToCharArray();
            for (index = 0; (index <= (len - 1)); index++)
            {
                if ((CountAdd == 4))
                {
                    CountAdd = 0;
                }

                Lascii[index] = System.Convert.ToInt16(cpwd[index]);
                Sascii[index] = (Lascii[index] - (CountAdd + 1));
                cpwd[index] = ((char)(Sascii[index]));
                result = (result + cpwd[index]);
                CountAdd++;
            }

            retstr = result;
            return retstr;
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        #region Convert DataTable To List
        public static List<T> ConvertDataTable<T>(DataTable dt, string Flag)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row, Flag);
                data.Add(item);
            }
            return data;
        }

        private static T GetItem<T>(DataRow dr, string Flag)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        if (Flag != "ConvertOnString")
                        {
                            pro.SetValue(obj, dr[column.ColumnName], null);
                        }
                        else if (Flag == "ConvertOnString")
                        {
                            pro.SetValue(obj, Convert.ToString(dr[column.ColumnName]), null);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return obj;
        }
        #endregion 

        #region Convert ByteSize To KB.MB,GB
        public static string GetFileSize(double byteCount)
        {
            string size = "0 Bytes";
            if (byteCount >= 1073741824.0)
                size = String.Format("{0:##.##}", byteCount / 1073741824.0) + " GB";
            else if (byteCount >= 1048576.0)
                size = String.Format("{0:##.##}", byteCount / 1048576.0) + " MB";
            else if (byteCount >= 1024.0)
                size = String.Format("{0:##.##}", byteCount / 1024.0) + " KB";
            else if (byteCount > 0 && byteCount < 1024.0)
                size = byteCount.ToString() + " Bytes";
            return size;
        }
        #endregion 

        #region Convert Datatable to Json Object
        public static string DataTableToJsonObj(DataTable dt)
        {
            DataSet ds = new DataSet();
            ds.Merge(dt);
            StringBuilder JsonString = new StringBuilder();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                JsonString.Append("[");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    JsonString.Append("{");
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        string contentstr = ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString().Replace(@"\", "\\\\").Replace(@"\n", "").Replace("\r\n", "").Replace("\t", "");
                        if (j < ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + contentstr.Trim() + "\",");
                        }
                        else if (j == ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + contentstr.Trim() + "\"");
                        }
                    }
                    if (i == ds.Tables[0].Rows.Count - 1)
                    {
                        JsonString.Append("}");
                    }
                    else
                    {
                        JsonString.Append("},");
                    }
                }
                JsonString.Append("]");

                return JsonString.ToString();


            }
            else
            {
                return null;
            }
        }
        #endregion



    }
}
