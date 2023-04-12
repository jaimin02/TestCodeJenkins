using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using Utility;
using MedicalImaging.Models;
using System.Data;
using System.Text.RegularExpressions;
using DTO;
using System.Threading.Tasks;
using MedicalImaging.Repository;
using System.Net.Http;
using System.Net;
using System.Net.Mail;
using MIApi.Controllers;
using System.Web.Services.Description;
using SS.Mail;
using Microsoft.Graph;

namespace MedicalImaging.Controllers
{
    public class MILoginController : Controller
    {
        //
        // GET: /MILogin/
        DataTable dt_UserMst = new DataTable();
        clsCommon _ClsCommon = new clsCommon();
        //GetDataController _GetData = new GetDataController();

        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        public ActionResult Login()
        {
            //vivek();
            String userIpAddress, userAgent;

            Response.AddHeader("Cache-Control", "no-cache, no-store,must-revalidate");
            Response.AddHeader("Pragma", "no-cache");
            Response.AddHeader("Expires", "0");

            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();

            userIpAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            userAgent = System.Web.HttpContext.Current.Request.UserAgent;
            System.Web.HttpContext.Current.Session["sIpAddress"] = userIpAddress;

            if (userAgent.IndexOf("MSIE") > -1)
            {
                userAgent = "MSIE";
            }
            else if (userAgent.IndexOf("Firefox/") > -1)
            {
                userAgent = "Firefox";
            }
            else if (userAgent.IndexOf("Chrome/") > -1)
            {
                userAgent = "Chrome";
            }
            else
            {
                userAgent = "Other";
            }

            ViewBag.hdnIpAddress = Convert.ToString(userIpAddress);
            ViewBag.hdnUserAgent = Convert.ToString(userAgent);

            return View();
        }

        [WebMethod]
        public string UserDetails(Models.LoginDetails UserData)
        {
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            string strServerOffset = "";
            if (UserData.vTimeZoneName == Utilities.clsVariables.IndiaStandardTime)
            {
                strServerOffset = Utilities.clsVariables.strServerOffset;
            }
            if (UserData.vTimeZoneName == Utilities.clsVariables.EasternStandardTime)
            {
                strServerOffset = Utilities.clsVariables.strServerOffset_EST;
            }

            string indianTimeformat = indianTime.ToString("dd/MMM/yyyy HH:mm").Replace('/', '-') + " " + strServerOffset;

            LoginMst loginmst = new LoginMst();

            string password = loginmst.vLoginPass;
            System.Web.HttpContext.Current.Session["userdata"] = UserData;
            System.Web.HttpContext.Current.Session["iuserid"] = UserData.iUserId;
            System.Web.HttpContext.Current.Session["UserFullName"] = UserData.vFirstName + " " + UserData.vLastName;
            System.Web.HttpContext.Current.Session["UserNameWithProfile"] = UserData.UserNameWithProfile;
            System.Web.HttpContext.Current.Session["LoginTime"] = UserData.UserWise_CurrDateTime;
            System.Web.HttpContext.Current.Session["ScopeValues"] = UserData.vScopeValues;
            System.Web.HttpContext.Current.Session["vUserName"] = UserData.vUserName;
            System.Web.HttpContext.Current.Session["UserTypeCode"] = UserData.vUserTypeCode;
            System.Web.HttpContext.Current.Session["vUserTypeName"] = UserData.vUserTypeName;
            System.Web.HttpContext.Current.Session["vLocationCode"] = UserData.vLocationCode;
            System.Web.HttpContext.Current.Session["vLocationName"] = UserData.vLocationName;
            System.Web.HttpContext.Current.Session["vDeptName"] = UserData.vDeptName;
            System.Web.HttpContext.Current.Session["vLoginPass"] = UserData.vLoginPass;
            System.Web.HttpContext.Current.Session["vUserTypeName"] = UserData.vUserTypeName;
            System.Web.HttpContext.Current.Session["updatedValue"] = "FALSE";

            return "Success";
        }

        public ActionResult ChangePassword()
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
            ViewBag.hdnLoginPass = Convert.ToString(Utilities.DecryptPassword(System.Web.HttpContext.Current.Session["vLoginPass"].ToString()));
            return View();
        }
        [WebMethod]
        public async Task<string> ChangePwd(ChangePasswordDTO _ChangePasswordDTO)
        {
            string strRes = "";
            int RowIndex;
            string estr = "";
            string wstr;
            string wStr_PasswordHistory = " iUserID=" + _ChangePasswordDTO.ID + " AND cStatusIndi <> 'D' ORDER BY iSrNo DESC ";
            string wStr_PasswordPolicyMst = " cActiveFlag = 'Y' AND cStatusIndi <> 'D'";
            var ds = new DataSet();
            var dsfinal = new DataSet();
            var dt = new DataTable();
            var dtfinal = new DataTable();
            var dtPasswordPolicyMst = new DataTable();
            var dsPasswordPolicyMst = new DataSet();
            var dsPasswordHistory = new DataSet();
            var dtPasswordHistory = new DataTable();
            var dv = new DataView();
            string strPwd = "";
            int iSpecialCount = 0;
            int PwdHistoryCount = 0;
            int MatchChar = 0;
            string eStr_Retu = "";
            string oldPasswordFromHistory = string.Empty;
            string newPasswordFromUserMst = string.Empty;
            string UName = "";
            string LPass = "";

            UName = _ChangePasswordDTO.sessionUsername;
            LPass = _ChangePasswordDTO.sessionPassword;
            try
            {
                if (Regex.IsMatch(_ChangePasswordDTO.NewPassword.Trim(), "[A-Z]"))
                {
                    iSpecialCount = iSpecialCount + 1;
                }

                if (Regex.IsMatch(_ChangePasswordDTO.NewPassword.Trim(), "[a-z]"))
                {
                    iSpecialCount = iSpecialCount + 1;
                }

                if (Regex.IsMatch(_ChangePasswordDTO.NewPassword.Trim(), "[0-9]"))
                {
                    iSpecialCount = iSpecialCount + 1;
                }

                if (Regex.IsMatch(_ChangePasswordDTO.NewPassword.Trim(), "[!@#$%^&*]"))
                {
                    iSpecialCount = iSpecialCount + 1;
                }

                string URL = "GetData/GetPasswordPolicyData";
                dtPasswordPolicyMst = await _ClsCommon.Call_API_GeTMethod(URL, _ChangePasswordDTO);
                dsPasswordPolicyMst.Tables.Add(dtPasswordPolicyMst);

                if (dsPasswordPolicyMst.Tables[0].Rows.Count <= 0)
                {
                    throw new Exception(eStr_Retu);
                }

                foreach (DataRow dr in dsPasswordPolicyMst.Tables[0].Rows)
                {
                    if (Convert.ToString(dr["vPolicyDesc"]).Trim().ToLower() == "MatchHistory".ToLower())
                    {
                        PwdHistoryCount = Convert.ToInt32(dr["vValue"]);
                    }

                    string URL1 = "GetData/GetPasswordHistory";
                    dtPasswordHistory = await _ClsCommon.Call_API_GeTMethod(URL1, _ChangePasswordDTO);
                    dsPasswordHistory.Tables.Add(dtPasswordHistory);

                    if (dsPasswordHistory.Tables[0].Rows.Count <= 0)
                    {
                        throw new Exception(eStr_Retu);
                    }

                    if (dsPasswordHistory.Tables[0].Rows.Count < PwdHistoryCount)
                    {
                        PwdHistoryCount = dsPasswordHistory.Tables[0].Rows.Count;
                    }

                    for (int index = 0, loopTo = PwdHistoryCount - 1; index <= loopTo; index++)
                    {
                        // Get Number of Characters to match
                        dv = dsPasswordPolicyMst.Tables[0].DefaultView;
                        dv.RowFilter = "vPolicyDesc = 'MatchChar'";
                        oldPasswordFromHistory = Convert.ToString(dsPasswordHistory.Tables[0].Rows[index]["vPassword"]).Trim().ToLower();
                        newPasswordFromUserMst = Utilities.EncryptPassword(_ChangePasswordDTO.NewPassword.Trim().ToLower());
                        if (dv.Table.Rows.Count > 0)
                        {
                            if (int.TryParse(Convert.ToString(dv.Table.Rows[0]["vValue"]), out MatchChar))
                            {
                                if (MatchChar > 0)
                                {
                                    if (oldPasswordFromHistory.Trim().Length > MatchChar)
                                    {
                                        oldPasswordFromHistory = oldPasswordFromHistory.Substring(0, MatchChar);
                                    }

                                    if (newPasswordFromUserMst.Trim().Length > MatchChar)
                                    {
                                        newPasswordFromUserMst = newPasswordFromUserMst.Substring(0, MatchChar);
                                    }
                                }
                            }
                        }

                        if ((Convert.ToString(oldPasswordFromHistory).Trim().ToLower() ?? "") == (newPasswordFromUserMst.Trim().ToLower() ?? ""))
                        {
                            if (MatchChar > 0)
                            {
                                strRes = "First " + MatchChar.ToString() + " Characters Match With Password In List Of Previous " + Convert.ToString(dr["vValue"]) + " Passwords !!!";
                                return strRes;
                            }
                            else
                            {
                                strRes = "Password Already Exists In The List Of Previous " + Convert.ToString(dr["vValue"]) + " Passwords !!!";
                                return strRes;
                            }
                        }
                    }
                }

                if (_ChangePasswordDTO.NewPassword.Trim().Length < 8)
                {
                    strRes = "Password length should be minimum eight characters.";
                    return strRes;
                }
                else if (_ChangePasswordDTO.NewPassword.Trim() != _ChangePasswordDTO.ConfnewPassword.Trim())
                {
                    strRes = "New password And Confirm Password Must Be Same.";
                    return strRes;
                }
                else if (iSpecialCount < 4)
                {
                    strRes = "Password must contain at least 1 capital letter." + "\n" + "1 small letter, 1 number and 1 special character." + "\n" + "For special characters you can pick one of these -,(,!,@,#,$,),%,<,>";
                    return strRes;
                }
                else if (_ChangePasswordDTO.Username != UName && _ChangePasswordDTO.Password != LPass)
                {
                    strRes = "Please enter cuurect username & Password";
                }

                else if (_ChangePasswordDTO.Username == UName && _ChangePasswordDTO.Password == LPass)
                {
                    strPwd = Utilities.EncryptPassword(_ChangePasswordDTO.Password.Trim());

                    string URL2 = "GetData/GetUserMst";
                    dt = await _ClsCommon.Call_API_GeTMethod(URL2, _ChangePasswordDTO);
                    ds.Tables.Add(dt);
                    if (ds.Tables[0].Rows.Count <= 0)
                    {
                        strRes = "Error While Retrieving User Record";

                        return strRes;
                    }

                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count <= 0)
                        {
                            strRes = "User ID and Password Does Not Exist. Try Again";
                            return strRes;
                        }
                        else
                        {
                            dt_UserMst = ds.Tables[0];

                            strPwd = _ChangePasswordDTO.NewPassword.Trim();
                            strPwd = Utilities.EncryptPassword(strPwd);

                            var loopTo1 = dt_UserMst.Rows.Count - 1;
                            //for (RowIndex = 0; RowIndex <= loopTo1; RowIndex++)
                            //    dt_UserMst.Rows[RowIndex]["vLoginPass"] = strPwd.Trim();
                            string resultdata;
                            string URL3 = "GetData/Insert_ChangePassword";
                            for (RowIndex = 0; RowIndex <= loopTo1; RowIndex++)
                            {
                                _ChangePasswordDTO.NewPassword = strPwd.Trim();
                                _ChangePasswordDTO.ID = dt_UserMst.Rows[RowIndex]["iUserId"].ToString().Trim();
                                _ChangePasswordDTO.iModifyBy = Convert.ToInt32(_ChangePasswordDTO.ID);

                                resultdata = await _ClsCommon.Insert_APITransaction(URL3, _ChangePasswordDTO);

                                //if (!objLambda.Insert_ChangePassword(WS_Lambda.DataObjOpenSaveModeEnum.DataObjOpenMode_Add, ds, Convert.ToString(_ChangePasswordDTO.ID), ref estr))
                                if (Convert.ToInt32(resultdata) <= 0)
                                {
                                    strRes = "Error While Saving Data " + estr;

                                    return strRes;
                                }
                            }
                            System.Web.HttpContext.Current.Session["vLoginPass"] = strPwd;
                            strRes = "Success";
                            return strRes;

                        }
                    }
                }
                else
                {
                    strRes = "Login Failed. Please Try Again";
                    return strRes;
                }
            }
            catch (Exception ex)
            {
                //objCommonMethod.CreateExceptionLog(ex, "ChangePasswd");
            }
            return strRes;

        }

        [WebMethod]
        public async Task<string> GetOTP(OtpDTO _OtpDTO)
        {
            DataSet ds_Check1 = new DataSet();
            DataSet ds_Check = new DataSet();
            DataSet ds_SMS = new DataSet();
            string wStr = string.Empty;
            string eStr = string.Empty;
            string SMSURL = string.Empty;
            string SMSUser = string.Empty;
            string SMSPassw = string.Empty;
            string SMSSender = string.Empty;
            var dt_ListofUser = new DataTable();            
            bool AuthenticationWithSecretKey;
            SS.Mail.TenantInfo TI = new SS.Mail.TenantInfo();

            string URL1 = "GetData/GetUser";
            dt_ListofUser = await _ClsCommon.Call_API_GeTMethod(URL1, _OtpDTO);
            
            string numbers = "1234567890";
            String characters = numbers;
            characters += numbers;
            int length = 6;
            string otp = string.Empty;
            for (int i = 0; i <= length - 1; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                }
                while (otp.IndexOf(character) != -1);
                otp += character;
            }
            string OTPCode = otp;
            _OtpDTO.Otp = otp;
            string iuserid = Convert.ToString(dt_ListofUser.Rows[0]["iuserId"]);
            string Mobile_No = Convert.ToString(dt_ListofUser.Rows[0]["vPhoneNo"]);
            string vEmailId = Convert.ToString(dt_ListofUser.Rows[0]["vEmailId"]);
            
            _OtpDTO.UserId = iuserid;
            _OtpDTO.vLocationCode = Convert.ToString(dt_ListofUser.Rows[0]["vLocationCode"]);

            string URL2 = "GetData/GetSmsDetails";
            DataTable dt_ListofSms = await _ClsCommon.Call_API_GeTMethod(URL2, _OtpDTO);
            if (dt_ListofUser.Rows[0]["isMFASms"].ToString() == "Y")
            {
                string OTPMessage = OTPCode + " Is your one time password. Kindly use this OTP to access Disoft. Please don't share with anyone. powered by Sarjen Systems.";
                SMSURL = Convert.ToString(dt_ListofSms.Rows[0]["vSMSUrl"]);
                SMSUser = Convert.ToString(dt_ListofSms.Rows[0]["vSMSUser"]); //"SARJENSYSTEMS";
                SMSPassw = Convert.ToString(dt_ListofSms.Rows[0]["vSMSPwd"]); //"S$pl@4321!";
                SMSSender = Convert.ToString(dt_ListofSms.Rows[0]["vSMSSender"]); //"SARJEN";

                string SMSRecipient = Mobile_No;
                string SMSMessageData = OTPMessage;

                string SMSSendingURL = SMSURL + "username=" + SMSUser + "&pass=" + SMSPassw + "&senderid=" + SMSSender + "&dest_mobileno=" + SMSRecipient + "&message=" + OTPMessage + "&response=Y";
                try
                {
                    HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(SMSSendingURL);
                    HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
                    System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
                    string responseString = respStreamReader.ReadToEnd();
                    respStreamReader.Close();
                    myResp.Close();
                    await ExMsgInfoDetails("SMS", "", OTPMessage, "", "", iuserid, "Y", "", Mobile_No);
                }
                catch (Exception ex)
                {
                    await ExMsgInfoDetails("SMS", "", OTPMessage, "", "", iuserid, "N", ex.Message, Mobile_No);
                }
            }
            else { }

            if (dt_ListofUser.Rows[0]["isMFAEmail"].ToString() == "Y")
            {
                string sBody = string.Empty;
                string Emailmessage = string.Empty;
                try
                {
                    TI.TenantId = Convert.ToString(dt_ListofSms.Rows[0]["vTenantId"]);
                    TI.ClientId = Convert.ToString(dt_ListofSms.Rows[0]["vClientId"]);
                    TI.EmailUser = Convert.ToString(dt_ListofSms.Rows[0]["vFromEmail"]);
                    TI.EmailPassword = Convert.ToString(dt_ListofSms.Rows[0]["vPassword"]);
                    TI.Client_secret = Convert.ToString(dt_ListofSms.Rows[0]["vSecretKey"]);
                    AuthenticationWithSecretKey = Convert.ToBoolean(dt_ListofSms.Rows[0]["bAuthSecretKey"]);
                    string EmailSubject = "OTP For Login";

                    DateTime OTPGenDate = DateTime.Now;
                    string FinalOTPDate = OTPGenDate.ToString("dd-MMM-yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);

                    sBody = "Dear User" + "<br>" + "<br>";
                    sBody = sBody + "We have sent you this email in response to reset your Disoft C application password as per below details." + "<br>" + "<br>";
                    sBody = sBody + "<span style=\"width:100%; text-align:center; font-family:Verdana; font-size:12px;margin-left: 270px\">OTP Details</span>" + "<br>";
                    sBody = sBody + "--------------------------------------------------------------------------------------------" + "<br>";
                    sBody = sBody + "Requested By&ensp;&ensp;&ensp; : " + _OtpDTO.Username + "<br>";
                    sBody = sBody + "Requested Date &ensp;: " + FinalOTPDate + "<br>";
                    sBody = sBody + "OTP &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp; : " + "<b>" + OTPCode + "</b>" + "<br>" + "<br>";
                    sBody = sBody + "--------------------------------------------------------------------------------------------" + "<br>";
                    sBody = sBody + "Please contact Disoft C system administrator if you have not done this request." + "<br>" + "<br>" + "<br>";
                    sBody = sBody + "<b>" + "This is an auto generated mail, please do not reply on this email id." + "<b>" + "<br>";
                    sBody = sBody + "Confidential: The content of all the emails sent and/or received by us (including any attachment) is confidential to the intended recipient at the email address to which it has been addressed. It may not be disclosed to or used by anyone other than those addressed, nor may it be copied, disseminated, or distributed in any way. Doing of such acts strictly prohibited. If you have received this email or file in error, please notify the system manager and delete this email from your system. We will ensure such a mistake does not occur in the future. Please note that as a recipient it is your responsibility to check E-mails for malicious software. Finally, the opinions disclosed by sender do not have to reflect those of the company, therefore the company refuses to take any liability for the damage caused by the content of this E-mail." + "<br>";
                    
                    Emailmessage = "And Email Id : " + vEmailId;
                    //Smtp_Server.Send(e_mail);
                    var message = new Microsoft.Graph.Message()
                    {
                        Subject = EmailSubject,
                        Body = new ItemBody() { ContentType = BodyType.Html, Content = sBody.ToString() },
                        ToRecipients = new List<Recipient>(),
                        BccRecipients = new List<Recipient>() { new Recipient() { EmailAddress = new EmailAddress() { Address = Convert.ToString(vEmailId) } } }
                    };
                    //.BccRecipients Added by Bhargav Thaker 24Feb2023

                    if (AuthenticationWithSecretKey == false)
                    {
                        SS.Mail.EmailSender.SendMailUsingPassword(TI, message, true);
                    }
                    else
                    {
                        SS.Mail.EmailSender.sendMailBySecret(TI, message, true);
                    }
                    await ExMsgInfoDetails("Mail", "OTP For Login", sBody.ToString(), TI.EmailUser, vEmailId, iuserid, "Y", "", "");
                }
                catch (Exception ex)
                {
                    await ExMsgInfoDetails("Mail", "OTP For Login", sBody.ToString(), TI.EmailUser, vEmailId, iuserid, "N", ex.Message, "");
                }
            }
            else { }

            //dt_ListofUser = new DataTable();
            string URL3 = "SetData/Insert_Otp";
            var result = _ClsCommon.Call_API_POSTMethodStr(URL3, _OtpDTO);
            //DataSet InsertOTP = _IPmsLoginMstRepo.Insert_Otp(iuserid, OTPCode);
            return "success";
        }

        [WebMethod]
        public async Task<string> Sendotp(OtpDTO _OtpDTO)
        {
            DataSet ds_Check1 = new DataSet();
            DataSet ds_Check = new DataSet();
            DataSet ds_SMS = new DataSet();
            string wStr = string.Empty;
            string eStr = string.Empty;
            string SMSURL = string.Empty;
            string SMSUser = string.Empty;
            string SMSPassw = string.Empty;
            string SMSSender = string.Empty;
            var dt_ListofUser = new DataTable();

            string URL2 = "GetData/GetUser";
            dt_ListofUser = await _ClsCommon.Call_API_GeTMethod(URL2, _OtpDTO);

            #region Generate OTP
            string numbers = "1234567890";
            String characters = numbers;
            characters += numbers;
            int length = 6;
            string otp = string.Empty;
            for (int i = 0; i <= length - 1; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                }
                while (otp.IndexOf(character) != -1);
                otp += character;
            }
            #endregion Generate OTP

            string OTPCode = otp;
            _OtpDTO.Otp = otp;
            string iuserid = Convert.ToString(dt_ListofUser.Rows[0]["iuserId"]);
            string Mobile_No = Convert.ToString(dt_ListofUser.Rows[0]["vPhoneNo"]);
            string vEmailId = Convert.ToString(dt_ListofUser.Rows[0]["vEmailId"]);
            string vLocationCode = Convert.ToString(dt_ListofUser.Rows[0]["vLocationCode"]);
            _OtpDTO.UserId = iuserid;
            _OtpDTO.vLocationCode = vLocationCode;
                        
            #region SMS Send Coding
            string URL4 = "GetData/GetSmsDetails";
            DataTable dt_SMS = await _ClsCommon.Call_API_GeTMethod(URL4, _OtpDTO);
            if (dt_SMS.Rows.Count > 0)
            {
                SMSURL = Convert.ToString(dt_SMS.Rows[0]["vSMSUrl"]);
                SMSUser = Convert.ToString(dt_SMS.Rows[0]["vSMSUser"]);
                SMSPassw = Convert.ToString(dt_SMS.Rows[0]["vSMSPwd"]);
                SMSSender = Convert.ToString(dt_SMS.Rows[0]["vSMSSender"]);
            }
            else { return "SMS Gateway detail not found!"; }

            if (Mobile_No != "")
            {
                string OTPMessage = OTPCode + " Is your one time password. Kindly use this OTP to access Disoft. Please don't share with anyone. powered by Sarjen Systems.";
                string SMSRecipient = Mobile_No;
                string SMSMessageData = OTPMessage;
                string SMSSendingURL = SMSURL + "username=" + SMSUser + "&pass=" + SMSPassw + "&senderid=" + SMSSender + "&dest_mobileno=" + SMSRecipient + "&message=" + OTPMessage + "&response=Y";

                try
                {
                    HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(SMSSendingURL);
                    HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
                    System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
                    string responseString = respStreamReader.ReadToEnd();
                    respStreamReader.Close();
                    myResp.Close();
                    await ExMsgInfoDetails("SMS", "", OTPMessage, "", "", iuserid, "Y", "", Mobile_No);
                }
                catch (Exception ex)
                {
                    await ExMsgInfoDetails("SMS", "", OTPMessage, "", "", iuserid, "N", ex.Message, Mobile_No);
                }
            }
            #endregion SMS Send Coding

            string URL3 = "SetData/Insert_Otp";
            string result = await _ClsCommon.Insert_APITransaction(URL3, _OtpDTO);

            #region Email Send Coding
            string Emailmessage = string.Empty;
            string EmailSubject = "OTP For Login";
            string sBody = string.Empty;
            string FromMailId = dt_SMS.Rows[0]["vFromEmail"].ToString();

            if (vEmailId != "")
            {
                try
                {
                    SS.Mail.TenantInfo TenantInfo = new SS.Mail.TenantInfo();
                    TenantInfo.TenantId = dt_SMS.Rows[0]["vTenantId"].ToString();
                    TenantInfo.ClientId = dt_SMS.Rows[0]["vClientId"].ToString();
                    TenantInfo.Client_secret = dt_SMS.Rows[0]["vSecretKey"].ToString();
                    TenantInfo.EmailUser = dt_SMS.Rows[0]["vFromEmail"].ToString();
                    TenantInfo.EmailPassword = dt_SMS.Rows[0]["vPassword"].ToString();
                    bool AuthenticationKeySecretKey = Convert.ToBoolean(dt_SMS.Rows[0]["bAuthSecretKey"].ToString());

                    DateTime OTPGenDate = DateTime.Now;
                    string FinalOTPDate = OTPGenDate.ToString("dd-MMM-yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);

                    sBody = "Dear User" + "<br>" + "<br>";
                    sBody = sBody + "We have sent you this email in response to reset your Disoft C application password as per below details." + "<br>" + "<br>";
                    sBody = sBody + "<span style=\"width:100%; text-align:center; font-family:Verdana; font-size:12px;margin-left: 270px\">OTP Details</span>" + "<br>";
                    sBody = sBody + "--------------------------------------------------------------------------------------------" + "<br>";
                    sBody = sBody + "Requested By&ensp;&ensp;&ensp; : " + _OtpDTO.Username + "<br>";
                    sBody = sBody + "Requested Date &ensp;: " + FinalOTPDate + "<br>";
                    sBody = sBody + "OTP &ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp;&ensp; : " + "<b>" + OTPCode + "</b>" + "<br>" + "<br>";
                    sBody = sBody + "--------------------------------------------------------------------------------------------" + "<br>";
                    sBody = sBody + "Please contact Disoft C system administrator if you have not done this request." + "<br>" + "<br>" + "<br>";
                    sBody = sBody + "<b>This is an auto generated mail, please do not reply on this email id.</b>" + "<br>" + "<br>";
                    sBody = sBody + "Confidential: The content of all the emails sent and/or received by us (including any attachment) is confidential to the intended recipient at the email address to which it has been addressed. It may not be disclosed to or used by anyone other than those addressed, nor may it be copied, disseminated, or distributed in any way. Doing of such acts strictly prohibited. If you have received this email or file in error, please notify the system manager and delete this email from your system. We will ensure such a mistake does not occur in the future. Please note that as a recipient it is your responsibility to check E-mails for malicious software. Finally, the opinions disclosed by sender do not have to reflect those of the company, therefore the company refuses to take any liability for the damage caused by the content of this E-mail." + "<br>";

                    var message = new Microsoft.Graph.Message()
                    {
                        Subject = "OTP For Login",
                        Body = new ItemBody() { ContentType = BodyType.Html, Content = sBody.ToString() },
                        ToRecipients = new List<Recipient>(),
                        BccRecipients = new List<Recipient>() { new Recipient() { EmailAddress = new EmailAddress() { Address = vEmailId.Trim() } } }
                    };
                    //.BccRecipients Added by Bhargav Thaker 24Feb2023

                    if (AuthenticationKeySecretKey) { SS.Mail.EmailSender.sendMailBySecret(TenantInfo, message, true); }
                    else { SS.Mail.EmailSender.SendMailUsingPassword(TenantInfo, message, true); }
                    await ExMsgInfoDetails("Mail", EmailSubject, sBody.ToString(), FromMailId, vEmailId, iuserid, "Y", "", "");
                }
                catch (Exception ex)
                {
                    await ExMsgInfoDetails("Mail", EmailSubject, sBody.ToString(), FromMailId, vEmailId, iuserid, "N", ex.Message, "");
                }
            }
            #endregion Email Send Coding

            Session["FPUserName"] = _OtpDTO.Username;
            Session["FPUserid"] = _OtpDTO.UserId;
            return iuserid;
        }

        public async Task<string> ExMsgInfoDetails(string vNotificationType, string vSubject, string vBody, string vFromEmailId, string vToEmailId,
            string iCreatedBy, string cIsSent, string vRemarks, string Mobile_No)
        {
            ExMsgInfo obj = new ExMsgInfo();
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

        public ActionResult ForgotPassword()
        {
            string userIpAddress, userAgent;

            userIpAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            userAgent = System.Web.HttpContext.Current.Request.UserAgent;
            System.Web.HttpContext.Current.Session["sIpAddress"] = userIpAddress;

            if (userAgent.IndexOf("MSIE") > -1)
            {
                userAgent = "MSIE";
            }
            else if (userAgent.IndexOf("Firefox/") > -1)
            {
                userAgent = "Firefox";
            }
            else if (userAgent.IndexOf("Chrome/") > -1)
            {
                userAgent = "Chrome";
            }
            else
            {
                userAgent = "Other";
            }

            ViewBag.hdnIpAddress = Convert.ToString(userIpAddress);
            ViewBag.hdnUserAgent = Convert.ToString(userAgent);

            OtpDTO _otpDTO = new OtpDTO();
            _otpDTO.Username = Session["FPUserName"].ToString();
            _otpDTO.UserId = Session["FPUserid"].ToString();
            return View(_otpDTO);
        }

        [WebMethod]
        public async Task<string> CheckPwd(ChangePasswordDTO objmodel)
        {
            try
            {
                var dt_ListofUser = new DataTable();
                string pwd = objmodel.NewPassword;
                string encrpwd = Utilities.EncryptPassword(pwd);
                objmodel.NewPassword = encrpwd;

                string URL2 = "GetData/Userprofile";
                DTO.LoginDetails _tempobj = new DTO.LoginDetails();
                _tempobj.vUserName = objmodel.Username;
                dt_ListofUser = await _ClsCommon.Call_API_GeTMethod(URL2, _tempobj);

                foreach (DataRow row in dt_ListofUser.Rows)
                {
                    string Userid = row["iUserId"].ToString();
                    objmodel.ID = Userid;
                    string URL3 = "GetData/Insert_ChangePassword";
                    string result = await _ClsCommon.Insert_APITransaction(URL3, objmodel);
                }
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public JsonResult GetEncrPwd(string pwd)
        {
            string encrpwd = Utilities.EncryptPassword(pwd);
            return Json(encrpwd, JsonRequestBehavior.AllowGet);
        }

    }

}