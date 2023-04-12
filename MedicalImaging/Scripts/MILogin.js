var result = true;
var Flag_ValidateBlocked = "false"
var UserData = "";

function getUserProfile() {
    var userName = $('#txtUserName').val();
    $('#ddlUserProfile').val('');
    $('#ddlUserProfile').empty();
    data = {
        vUserName: userName
    }
    $.ajax({
        url: ApiURL + "GetData/Userprofile",
        type: 'POST',
        data: data,
        dataType: 'json',
        success: SuccessMethod,
        async: false,
        error: function (ex) {
            AlertBox("warning", " MI Login ", "Failed to retrieve User Profile details : " + ex);
        }
    });

    function SuccessMethod(jsonData) {
        $.each(jsonData, function (i, items) {
            $('#ddlUserProfile').append($("<option></option>").val(this['vUserTypeCode']).html(this['vUserTypeName']));
        });

        if ($("#ddlUserProfile").val() != null) {
            $("#btnForgotPwd").css("pointer-events", "fill");
            $("#hdnusername").val(userName);
            var userprofile = $('#ddlUserProfile').find(":selected").val();
            $("#hdnuserprofile").val(userprofile);
        }
    }
    return false;
}

function setUserProfile() {
    var userprofile = $('#ddlUserProfile').find(":selected").val();
    $("#hdnuserprofile").val(userprofile);
}

function CheckUsername() {
    var userName = $('#txtUserName').val();
    if (userName == "") {
        AlertBox("warning", "MI Login", "Please Enter User name !");
        return false;
    }
}

$(document).ready(function () {
    $("#copyRights").append('Copyright © ' + new Date().getFullYear() + '<a href="http://www.sarjen.com" target="_blank"> Sarjen Systems Pvt. Ltd.</a> All rights reserved. <lable onclick="UnLockInspect();">' + Ver_Env + '</label>')

    $('.btn').hover(function () {
        $(this).stop().animate({ 'padding-left': '20px', 'padding-right': '70px' }, 'fast');
        $(this).find('.num').stop().animate({ 'left': '-50px' }, 'fast');
        $(this).find('.fa').stop().animate({ 'right': '0px' }, 'fast');
    }, function () {
        $(this).stop().animate({ 'padding-left': '70px', 'padding-right': '20px' }, 'fast');
        $(this).find('.num').stop().animate({ 'left': '0px' }, 'fast');
        $(this).find('.fa').stop().animate({ 'right': '-50px' }, 'fast');
    });

    $("#btnUser").click(function () {});

    $('#loginFormModal').on('shown.bs.modal', function () {
        $('#txtUserName').focus();
        $("#btnForgotPwd").css("pointer-events", "none");
        $('#divotp').css("display", "none");
        $("#lnkresendOTP").css("display", "none");
        $("#btnChangePwd").css("display", "none");
        $("#btnSendOtp").css("display", "block");
    })

    $('#loginFormModal').on('hide.bs.modal', function () {
        $('#txtUserName').val("");
        $('#txtUserPassword').val("");
        $('#ddlUserProfile').val("");        
    })

    $('#forgotFormModal').on('shown.bs.modal', function () {
        var userName = $('#hdnusername').val();
        var userProfile = { vUserName: userName };
        $("#OTPInput").val('');

        $.ajax({
            url: ApiURL + "GetData/Userprofile",
            type: 'POST',
            data: userProfile,
            dataType: 'json',
            success: OnSucceeUserData,
            async: false,
            error: onError
        });
    })

    $('#forgotFormModal').on('hide.bs.modal', function () {
        $("#btnForgotPwd").css("pointer-events", "none");
        $("#OTPInput").val('');
        $('#divotp').css("display", "none");
        $("#lnkresendOTP").css("display", "none");
        $("#btnChangePwd").css("display", "none");
        $("#btnSendOtp").css("display", "block");
    })

    function OnSucceeUserData(jsonData) {
        var EmailIds = jsonData[0].vEmailId;
        var MobNo = jsonData[0].vPhoneNo;
        var MaskMobNo = MobNo.replace(MobNo.substring(0, 6), '*'.repeat(6 - 0));
        var MaskEId = EmailIds.replace(EmailIds.substring(0, 7), '*'.repeat(7 - 0));
        var MaskEmId = "********" + EmailIds.slice(-6);

        $("#spnMobNo").html(MaskMobNo);
        $("#spnEmailId").html(MaskEmId);

        if (EmailIds != "") {
            $("#txtEmailId").val(EmailIds);
            $('#lblEmailId').css("display", "block");
        }
        else {
            $("#txtEmailId").val('');
            $('#lblEmailId').css("display", "none");
        }

        if (MobNo != "") {
            $("#txtMobileNumber").val(MobNo);
            $('#lblMobileNumber').css("display", "block");
        }
        else {
            $("#txtMobileNumber").val('');
            $('#lblMobileNumber').css("display", "none");
        }

        if (EmailIds == "" && MobNo == "") {
            $("#btnSendOtp").attr("disabled", "disabled");
            $('#successMsgForSend').css("display", "none");
            AlertBox("warning", " MI Login ", "Mobile No and Email Information not found. Please contact System Administrator.");
            return false;
        }
        else {
            $("#btnSendOtp").removeAttr("disabled");
            $('#successMsgForSend').css("display", "block");

            setTimeout(function () { SendOTP(); }, 2000);
        }
    }

    function SendOTP() {
        var username = $('#hdnusername').val();
        var userProfile = $('#hdnuserprofile').val();

        $.ajax({
            url: WebURL + "MILogin/Sendotp",
            type: 'GET',
            data: {
                Username: username,
                vUserTypeCode: userProfile
            },
            dataType: "text",
            async: false,
            success: OnSuccessOtpSend,
            error: OnErrorCallOtp
        });

        function OnSuccessOtpSend(data) {
            if (data != 0) {
                $('#divotp').css("display", "block");
                $("#lnkresendOTP").css("display", "block");
                $("#btnChangePwd").css("display", "block");
                $("#btnSendOtp").css("display", "none");
                $("#hdnuserid").val(data);
                result = true;
                return true;
            }
        }

        function OnErrorCallOtp(ex) {
            result = false;
            return false;
        }
    }
    
    $(document).on("click", "#btnChangePwd", function (e) {
        var OtpNo = $("#OTPInput").val();
        if (OtpNo != "") {
            var UserId = $("#hdnuserid").val();
            var count = 0;
            var GetOtp = {
                UserId: UserId,
            };

            $.ajax({
                url: ApiURL + "GetData/GetOtp",
                type: 'POST',
                data: GetOtp,
                dataType: 'json',
                success: OnSuccessGetOtp,
                async: false,
                error: onError
            });

            function OnSuccessGetOtp(jsondata) {
                if (jsondata != "") {
                    for (var i = 0 ; i < jsondata.length ; i++) {
                        if (OtpNo == jsondata[i].vOTPNo) {
                            var redirectUrl = $('#redirectToFPwd').val();
                            location.href = redirectUrl;
                            return false;
                        }
                        else {
                            count++;
                        }
                    }

                    if (count == jsondata.length) {
                        alert("Invalid OTP!");
                        $("#OTPInput").val('');
                    }
                }
            }
        }
        else {
            alert("Please Enter OTP!");
            $("#OTPInput").val('');
            return false;
        }
    });



    $(document).on("click", "#btnOtpLogin", function (e) {
        var url = $("#RedirectTo").val();

        if ($("#txtUserName").val() == "") {            
            //runEffect("loginFormModal");
            runEffect("txtUserName");
            $('#txtUserName').focus();
            $('#txtUserName').addClass("element_error");            
            //AlertBox("warning", " MI Login ", "Please Enter UserName !");            
            return false;
        }
        if ($("#txtUserPassword").val() == "") {         
            //runEffect("loginFormModal");
            runEffect("txtUserPassword");
            $('#txtUserPassword').focus();
            $('#txtUserPassword').addClass("element_error");
            //AlertBox("warning", " MI Login ", "Please Enter Password !");    
            return false;
        }        

        if ($('#ddlUserProfile').val() == null) {
            //runEffect("loginFormModal");
            runEffect("ddlUserProfile");                        
            $('#ddlUserProfile').focus();
            $('#ddlUserProfile').addClass("element_error");
            //AlertBox("warning", " MI Login ", "Please Select Profile !");
            return false;
        }

        var userName = $('#txtUserName').val();
        var userPassword = $('#txtUserPassword').val();
        var userProfile = $('#ddlUserProfile').val();
        var ipAddress = $('#hdnIpAddress').val();
        var userAgent = $('#hdnUserAgent').val();

        var userLoginDetails = {
            vUserName: userName,
            vLoginPass: userPassword,
            vUserTypeCode: userProfile,
            vIpAddress: ipAddress,
            vUserAgent: userAgent
        };

        $.ajax({
            url: ApiURL + "GetData/PostUserAuthenticationDetails",
            type: 'POST',
            data: userLoginDetails,
            dataType: 'json',
            success: OnSuccessOtp,
            async: false,
            error: onError

        });
    });

    function OnSuccessOtp(jsonData) {
        var user = $('#txtusername').val();
        var currentUserAgent = $('#hdnUserAgent').val();
        var userProfile = $('#ddlUserProfile').val();
        var resultLogin = true;
        UserData = jsonData;
        if (jsonData.Data.length != 0) {
            if (jsonData.Data[0].IsMFA == 'Y' && (jsonData.Data[0].IsMFAEmail == 'Y' || jsonData.Data[0].IsMFASMS == 'Y')) {
                $('#dvotp').css("display", "block");
                //loadTimer();
                var username = $('#txtUserName').val();
                var password = $('#txtUserPassword').val();

                $.ajax({
                    url: WebURL + "MILogin/GetOTP",
                    type: 'GET',
                    data: {
                        Username: username,
                        vUserTypeCode: userProfile
                    },
                    //dataType: 'json',
                    dataType: "text",
                    async: false,
                    success: OnSuccessOtpSend,
                    error: OnErrorCallOtp
                });

                function OnSuccessOtpSend(data) {
                    if (data == "success") {
                        $("#lblOtpmsg").css("display", "block");
                        $(".login__row").css("display", "block");
                        $("#lblOtpEmailMsg").css("display", "block");
                        $("#lblOtpSmsMsg").css("display", "block");
                        if (jsonData.Data[0].IsMFA == 'Y' && (jsonData.Data[0].IsMFAEmail == 'Y' || jsonData.Data[0].IsMFASMS == 'Y')) {
                            if (jsonData.Data[0].IsMFAEmail == 'Y') {
                                var user_Email = jsonData.Data[0].vEmailId
                                var Email, LastEmail, EmailText;
                                Email = user_Email.substring(0, 2);
                                LastEmail = user_Email.split("@");
                                EmailText = "Email : " + Email + "********" + "@" + LastEmail[1];
                                $("#lblOtpEmailMsg").html(EmailText);
                            }
                            if (jsonData.Data[0].IsMFASMS == 'Y') {
                                var user_Mobile = jsonData.Data[0].vPhoneNo
                                var MObile, LastMobile, MobileText;
                                MObile = user_Mobile.substring(0, 2);
                                LastMobile = user_Mobile.substring(8)
                                MobileText = "Sms : " + MObile + "********" + LastMobile;
                                $("#lblOtpSmsMsg").html(MobileText);
                            }
                        }
                        $("#btnReLogin").css("display", "block");
                        $("#btnOtpLogin").css("display", "none");
                        result = true;
                        return true;
                    }
                }

                function OnErrorCallOtp(ex) {
                    result = false;
                    return false;
                }
            }
            else {
                $("#btnLogin").trigger("click");
            }
        }
        else {

            //alert('Login Failed. Try Again.! ');
            result = false;
        }
        return result;
    }

    $(document).on("click", "#lnkresendOTP", function (e) {
        var url = $("#RedirectTo").val();

        var userName = $('#txtUserName').val();
        var userPassword = $('#txtUserPassword').val();
        var userProfile = $('#ddlUserProfile').val();
        var ipAddress = $('#hdnIpAddress').val();
        var userAgent = $('#hdnUserAgent').val();

        var userLoginDetails = {
            vUserName: userName,
            vLoginPass: userPassword,
            vUserTypeCode: userProfile,
            vIpAddress: ipAddress,
            vUserAgent: userAgent
        };

        $.ajax({
            url: ApiURL + "GetData/PostUserAuthenticationDetails",
            type: 'POST',
            data: userLoginDetails,
            dataType: 'json',
            success: OnSuccessResedOtp,
            async: false,
            error: onError

        });
        document.getElementById("lnkresendOTP").style.display = "hidden";
        document.getElementById('timer').style.visibility = 'block';
        document.getElementById('txttimer').style.visibility = 'block';
    });

    function OnSuccessResedOtp(jsonData) {
        var user = $('#txtusername').val();
        var currentUserAgent = $('#hdnUserAgent').val();
        var userProfile = $('#ddlUserProfile').val();
        var resultLogin = true;

        if (jsonData.Data.length != 0) {
            if (jsonData.Data[0].IsMFA == 'Y'  && (jsonData.Data[0].IsMFAEmail == 'Y' || jsonData.Data[0].IsMFASMS == 'Y')) {
                $('#dvotp').css("display", "block");
                $("#OtpInput").css({ "display": "block" });
                //loadTimer();
                $("#lnkresendOTP").css({ "display": "none" });
                var username = $('#txtUserName').val();
                var password = $('#txtUserPassword').val();
                var usertypecode = $('#ddlProfile').val();
                $.ajax({
                    url: WebURL + "MILogin/GetOTP",
                    type: 'GET',
                    data: {
                        Username: username,
                        vUserTypeCode: userProfile
                    },
                    //dataType: 'json',
                    dataType: "text",
                    async: false,
                    success: OnSuccessOtpReSend,
                    error: OnErrorCallOtp
                });

                function OnSuccessOtpReSend(data) {
                    if (data == "success") {
                        $("#lblOtpmsg").css("display", "block");
                        $(".login__row").css("display", "block");
                        $("#lblOtpEmailMsg").css("display", "block");
                        $("#lblOtpSmsMsg").css("display", "block");
                        if (jsonData.Data[0].IsMFA == 'Y') {
                            if (jsonData.Data[0].IsMFAEmail == 'Y') {
                                var user_Email = jsonData.Data[0].vEmailId
                                var Email, LastEmail, EmailText;
                                Email = user_Email.substring(0, 2);
                                LastEmail = user_Email.split("@");
                                EmailText = "Email : " + Email + "********" + "@" + LastEmail[1];
                                $("#lblOtpEmailMsg").html(EmailText);
                            }
                            if (jsonData.Data[0].IsMFASMS == 'Y') {
                                var user_Mobile = jsonData.Data[0].vPhoneNo
                                var MObile, LastMobile, MobileText;
                                MObile = user_Mobile.substring(0, 2);
                                LastMobile = user_Mobile.substring(8)
                                MobileText = "Sms : " + MObile + "********" + LastMobile;
                                $("#lblOtpSmsMsg").html(MobileText);
                            }
                        }
                        $("#btnReLogin").css("display", "block");
                        $("#btnOtpLogin").css("display", "none");
                        result = true;
                        return true;
                        //timer = true;
                        //return
                    }
                }

                function OnErrorCallOtp(ex) {
                    result = false;
                    return false;
                }
            }
        }
    }

    $("#btnLogin").click(function () {
        debugger;

        if ($("#txtUserName").val() == "") {            
            //runEffect("loginFormModal");
            runEffect("txtUserName");
            $('#txtUserName').focus();
            $('#txtUserName').addClass("element_error");            
            //AlertBox("warning", " MI Login ", "Please Enter UserName !");            
            return false;
        }
        if ($("#txtUserPassword").val() == "") {         
            //runEffect("loginFormModal");
            runEffect("txtUserPassword");
            $('#txtUserPassword').focus();
            $('#txtUserPassword').addClass("element_error");
            //AlertBox("warning", " MI Login ", "Please Enter Password !");    
            return false;
        }        

        if ($('#ddlUserProfile').val() == null) {
            //runEffect("loginFormModal");
            runEffect("ddlUserProfile");                        
            $('#ddlUserProfile').focus();
            $('#ddlUserProfile').addClass("element_error");
            //AlertBox("warning", " MI Login ", "Please Select Profile !");
            return false;
        }
        
        if (userAuthentication() == false) {
            AssingLoginFailureDetails();
            $('#txtUserName').val('');
            $('#txtUserPassword').val('');
            $('#ddlUserProfile').val('');
            $('#ddlUserProfile').empty();
            return false;
        }
        else {
            var redirectUrl;
            if ($('#txtUserName').val() == $("#txtUserPassword").val())
            {
                redirectUrl = $('#redirectToChangePassword').val();
            }
            else {
                redirectUrl = $('#redirectTo').val();
            }
            //window.open(redirectUrl, "_blank");
            location.href = redirectUrl;
            return false;
        }
    });

    $(document).on("click", "#btnReLogin", function (e) {
        var OtpNo = $("#OtpInput").val();
        var UserId = UserData.Data[0].iUserId;
        var count = 0;
        var GetOtp = {
            UserId: UserId,
        };

        $.ajax({
            url: ApiURL + "GetData/GetOtp",
            type: 'POST',
            data: GetOtp,
            dataType: 'json',
            success: OnSuccessGerOtp,
            async: false,
            error: onError
        });

        function OnSuccessGerOtp(jsondata) {
            if (jsondata != "") {
                for (var i = 0 ; i < jsondata.length ; i++) {
                    if (OtpNo == jsondata[i].vOTPNo) {
                        $("#btnLogin").trigger("click");
                    }
                    else {
                        count++;
                    }
                }

                if (count == jsondata.length) {
                    alert("Invalid OTP!");
                    $("#OtpInput").val('');
                }
            }
        }
    });

    $("input").on("focus", function () {
        $(this).addClass("element_focus");
    })
    $("input").on("blur", function () {
        $(this).removeClass("element_focus");
        $(this).removeClass("element_error");
    })

    $("select").on("focus", function () {
        $(this).addClass("element_focus");
    })
    $("select").on("blur", function () {
        $(this).removeClass("element_focus");
        $(this).removeClass("element_error");
    })

    function loadTimer() {
        var timer2 = "02:00";
        var interval = setInterval(function () {
            var timer = timer2.split(':');
            //by parsing integer, I avoid all extra string processing
            var minutes = parseInt(timer[0], 10);
            var seconds = parseInt(timer[1], 10);
            --seconds;
            minutes = (seconds < 0) ? --minutes : minutes;
            if (minutes < 0) {
                clearInterval(interval);
                $("#dvotp").css({ "display": "none" });
                $("#lnkresendOTP").css({ "display": "block" });
            }
            else {

            }
            seconds = (seconds < 0) ? 59 : seconds;
            seconds = (seconds < 10) ? '0' + seconds : seconds;
            //minutes = (minutes < 10) ?  minutes : minutes;
            $('#timer').html(minutes + ':' + seconds);
            timer2 = minutes + ':' + seconds;
        }, 1000);
    }

    function checkSecond(sec) {
        if (sec < 10 && sec >= 0) { sec = "0" + sec }; // add zero in front of numbers < 10
        if (sec < 0) { sec = "59" };
        return sec;
    }
});

function userAuthentication() {
    var userName = $('#txtUserName').val();
    var userPassword = $('#txtUserPassword').val();
    var userProfile = $('#ddlUserProfile').val();
    var ipAddress = $('#hdnIpAddress').val();
    var userAgent = $('#hdnUserAgent').val();

    var userLoginDetails = {
        vUserName: userName,
        vLoginPass: userPassword,
        vUserTypeCode: userProfile,
        vIpAddress: ipAddress,
        vUserAgent: userAgent
    };

    $.ajax({
        url: ApiURL + "GetData/PostUserAuthenticationDetails",
        type: 'POST',
        data: userLoginDetails,
        dataType: 'json',
        success: onSuccess,
        async: false,
        error: onError
    });
    return result;
}

function runEffect(name) {
    $("#" + name).effect("shake", "", 500, callback);
    $("#" + name).effect("shake", "", 200, callback);
}

function callback() {
    setTimeout(function () {        
        $("#loginFormModal").removeAttr("style").hide().show();        
    }, 1000);
}

function onSuccess(jsonData) {
    var user = $('#txtUserName').val();
    var currentUserAgent = $('#hdnUserAgent').val();
    var resultLogin = true;

    if (jsonData.Data.length != 0) {
        if (jsonData.Data[0].cBlockedFlag == 'Y') {
            runEffect("loginFormModal");
            AlertBox("warning", " MI Login ", "Your username is locked due to multiple incorrect login attempts !!!")
            resultLogin = false;
            $("#txtUserName").focus();
            return false;
        }
        else if (jsonData.Data[0].cBlockedFlag == 'E') {
            runEffect("loginFormModal");
            AlertBox("warning", " MI Login ", "Login Failed. Try Again.!")
            resultLogin = false;
            $("#txtUserName").focus();
        }
        else if (jsonData.Data[0].cLockFlag == 'Y' && jsonData.Data[0].cInactive == 'Y') {
            runEffect("loginFormModal");
            AlertBox("warning", " MI Login ", "Your username is locked, as the user is inactive since 90 days, Please contact Administrator.")
            resultLogin = false;
            $("#txtUserName").focus();
        }
        else if (jsonData.Data[0].cLockFlag == 'Y' && jsonData.Data[0].cInactive == 'N') {
            runEffect("loginFormModal");
            AlertBox("warning", " MI Login ", "Your username is locked due to multiple incorrect login attempts, Please contact Administrator.")
            resultLogin = false;
            $("#txtUserName").focus();
        }
        else if (jsonData.Data[0].cLockFlag == 'N' && jsonData.Data[0].cInactive == 'Y') {
            runEffect("loginFormModal");
            AlertBox("warning", " MI Login ", "Your username is locked, as the user is inactive since 90 days !!!")
            resultLogin = false;
            $("#txtUserName").focus();
        }
        else if (jsonData.Data[0].MaxLoginMins >= 0) {
            runEffect("loginFormModal");
            resultLogin = false;
            $.confirm({
                title: 'Confirm!',
                icon: 'fa fa-warning',
                content: 'USER CONFIRMATION',
                onContentReady: function () {
                    var self = this;
                    this.setContentPrepend('<div>MI</div>');
                    setTimeout(function () {
                        self.setContentAppend("<div>\"" + user + "\" is already logged in " + jsonData.Data[0].vUserAgent.trim() + " Browser of " + jsonData.Data[0].vIPAddress + " System. " + "Are you sure want to continue Login with Current " + currentUserAgent + " Browser of " + $('#hdnIpAddress').val() + " System </div>");
                    }, 1000);
                },
                columnClass: 'medium',
                //animation: 'zoom',
                closeIcon: true,
                closeIconClass: 'fa fa-close',
                //columnClass: 'small',
                boxWidth: '30%',
                autoClose: 'danger|30000',
                animation: 'news',
                closeAnimation: 'news',
                closeAnimation: 'scale',
                backgroundDismissAnimation: 'random',
                //backgroundDismissAnimation: 'glow',
                type: 'blue',
                theme: 'blue',
                draggable: true,
                buttons: {
                    info: {
                        btnClass: 'btn-blue',
                        text: 'OK',
                        //keys: ['O'],
                        action: function () {
                            var SaveUserLoginData = {
                                iUserId: jsonData.Data[0].iUserId,
                                vIPAddress: $('#hdnIpAddress').val(),
                                vUTCHourDiff: jsonData.Data[0].vUTCHourDiff,
                                vUserAgent: jsonData.Data[0].vUserAgent,
                                DataopMode: 1
                            }
                            $.ajax({
                                type: 'POST',
                                data: SaveUserLoginData,
                                async: false,
                                url: ApiURL + "SetData/save_UserLoginDetails",
                                success: function () {
                                    resultLogin = true;

                                    window.sessionStorage.setItem("UserNameWithProfile", jsonData.Data[0].UserNameWithProfile);
                                    var UserDetails = {};
                                    UserDetails.iUserId = jsonData.Data[0].iUserId;
                                    UserDetails.iUserGroupCode = jsonData.Data[0].iUserGroupCode;
                                    UserDetails.vUserGroupName = jsonData.Data[0].vUserGroupName;
                                    UserDetails.vUserName = jsonData.Data[0].vUserName;
                                    UserDetails.vLoginName = jsonData.Data[0].vLoginName;
                                    UserDetails.vLoginPass = jsonData.Data[0].vLoginPass;                                    
                                    UserDetails.vUserTypeCode = jsonData.Data[0].vUserTypeCode;
                                    UserDetails.vUserTypeName = jsonData.Data[0].vUserTypeName;
                                    UserDetails.vDeptCode = jsonData.Data[0].vDeptCode;
                                    UserDetails.vDeptName = jsonData.Data[0].vDeptName;
                                    UserDetails.vLocationCode = jsonData.Data[0].vLocationCode;
                                    UserDetails.vLocationName = jsonData.Data[0].vLocationName;
                                    UserDetails.vTimeZoneName = jsonData.Data[0].vTimeZoneName;
                                    UserDetails.vLocationInitiate = jsonData.Data[0].vLocationInitiate;
                                    UserDetails.vEmailId = jsonData.Data[0].vEmailId;
                                    UserDetails.vPhoneNo = jsonData.Data[0].vPhoneNo;
                                    UserDetails.vExtNo = jsonData.Data[0].vExtNo;
                                    UserDetails.vRemark = jsonData.Data[0].vRemark;
                                    UserDetails.iModifyBy = jsonData.Data[0].iModifyBy;
                                    UserDetails.dModifyOn = jsonData.Data[0].dModifyOn;
                                    UserDetails.cStatusIndi = jsonData.Data[0].cStatusIndi;
                                    UserDetails.vFirstName = jsonData.Data[0].vFirstName;
                                    UserDetails.vLastName = jsonData.Data[0].vLastName;
                                    UserDetails.nScopeNo = jsonData.Data[0].nScopeNo;
                                    UserDetails.vScopeName = jsonData.Data[0].vScopeName;
                                    UserDetails.vScopeValues = jsonData.Data[0].vScopeValues;
                                    UserDetails.iWorkflowStageId = jsonData.Data[0].iWorkflowStageId;
                                    UserDetails.cIsEDCUser = jsonData.Data[0].cIsEDCUser;
                                    UserDetails.dFromDate = jsonData.Data[0].dFromDate;
                                    UserDetails.dToDate = jsonData.Data[0].dToDate;
                                    UserDetails.ModifierName = jsonData.Data[0].ModifierName;
                                    UserDetails.UserNameWithProfile = jsonData.Data[0].UserNameWithProfile;
                                    UserDetails.tmp_dModifyOn = jsonData.Data[0].tmp_dModifyOn;
                                    UserDetails.OperationCode = $("#hdnOprationCode").val();
                                    UserDetails.tmp_dModifyOn = jsonData.Data[0].tmp_dModifyOn;
                                    UserDetails.UserWise_CurrDateTime = jsonData.Data[0].UserWise_CurrDateTime;
                                    
                                    $.ajax({
                                        type: "POST",
                                        url: WebURL + "MILogin/UserDetails",
                                        data: JSON.stringify(UserDetails),
                                        contentType: "application/json; charset=utf-8",
                                        //dataType: "json",
                                        dataType: "text",
                                        async: false,
                                        success: OnSuccess1,
                                        error: OnErrorCall
                                    });
                                    function OnSuccess1(jsonData) {

                                    }
                                    function OnErrorCall(ex) {
                                        //alert(ex);
                                    }
                                    result = true;
                                    var redirectUrl = $('#redirectTo').val();                                    
                                    location.href = redirectUrl;
                                    return false;
                                    //return result;

                                },
                                error: function (ex) {
                                    throw ex;
                                    AlertBox("warning", " MI Login ", "Error while saving details of User Login details !")                                    
                                    $("#txtUserName").focus();
                                }
                            });
                        }
                    },
                    danger: {
                        btnClass: 'btn-red any-other-class',
                        text: 'CANCEL',
                        //keys: ['C'],
                        action: function () {
                            resultLogin = false;
                            runEffect("loginFormModal");
                        }
                    },
                }
            });           
        }
        else {
            resultLogin = true;
        }
    }
    if (resultLogin != false) {

        window.sessionStorage.setItem("UserNameWithProfile", jsonData.Data[0].UserNameWithProfile);
        var UserDetails = {};
        UserDetails.iUserId = jsonData.Data[0].iUserId;
        UserDetails.iUserGroupCode = jsonData.Data[0].iUserGroupCode;
        UserDetails.vUserGroupName = jsonData.Data[0].vUserGroupName;
        UserDetails.vUserName = jsonData.Data[0].vUserName;
        UserDetails.vLoginName = jsonData.Data[0].vLoginName;
        UserDetails.vLoginPass = jsonData.Data[0].vLoginPass;
        UserDetails.vUserTypeCode = $('#ddlUserProfile').val();
        UserDetails.vUserTypeName = jsonData.Data[0].vUserTypeName;
        UserDetails.vDeptCode = jsonData.Data[0].vDeptCode;
        UserDetails.vDeptName = jsonData.Data[0].vDeptName;
        UserDetails.vLocationCode = jsonData.Data[0].vLocationCode;
        UserDetails.vLocationName = jsonData.Data[0].vLocationName;
        UserDetails.vTimeZoneName = jsonData.Data[0].vTimeZoneName;
        UserDetails.vLocationInitiate = jsonData.Data[0].vLocationInitiate;
        UserDetails.vEmailId = jsonData.Data[0].vEmailId;
        UserDetails.vPhoneNo = jsonData.Data[0].vPhoneNo;
        UserDetails.vExtNo = jsonData.Data[0].vExtNo;
        UserDetails.vRemark = jsonData.Data[0].vRemark;
        UserDetails.iModifyBy = jsonData.Data[0].iModifyBy;
        UserDetails.dModifyOn = jsonData.Data[0].dModifyOn;
        UserDetails.cStatusIndi = jsonData.Data[0].cStatusIndi;
        UserDetails.vFirstName = jsonData.Data[0].vFirstName;
        UserDetails.vLastName = jsonData.Data[0].vLastName;
        UserDetails.nScopeNo = jsonData.Data[0].nScopeNo;
        UserDetails.vScopeName = jsonData.Data[0].vScopeName;
        UserDetails.vScopeValues = jsonData.Data[0].vScopeValues;
        UserDetails.iWorkflowStageId = jsonData.Data[0].iWorkflowStageId;
        UserDetails.cIsEDCUser = jsonData.Data[0].cIsEDCUser;
        UserDetails.dFromDate = jsonData.Data[0].dFromDate;
        UserDetails.dToDate = jsonData.Data[0].dToDate;
        UserDetails.ModifierName = jsonData.Data[0].ModifierName;
        UserDetails.UserNameWithProfile = jsonData.Data[0].UserNameWithProfile;
        UserDetails.tmp_dModifyOn = jsonData.Data[0].tmp_dModifyOn;
        UserDetails.OperationCode = $("#hdnOprationCode").val();
        UserDetails.tmp_dModifyOn = jsonData.Data[0].tmp_dModifyOn;
        UserDetails.UserWise_CurrDateTime = jsonData.Data[0].UserWise_CurrDateTime;


        $.ajax({
            type: "POST",
            url: WebURL + "MILogin/UserDetails",
            data: JSON.stringify(UserDetails),
            contentType: "application/json; charset=utf-8",
            //dataType: "json",
            dataType: "text",
            async: false,
            success: OnSuccess1,
            error: OnErrorCall
        });
        function OnSuccess1(jsonData) {

        }
        function OnErrorCall(ex) {
            //alert(ex);
        }
        result = true;
    }
    else {
        result = false;
    }
    return result;
}

function onError(jsonData) {
}

function AssingLoginFailureDetails() {
    var username = $('#txtUserName').val();
    var password = $('#txtUserPassword').val();
    var usertypecode = $('#ddlUseProfile').val();
    var Obj_Login = {
        vUserName: username,
        vUserTypeCode: usertypecode
    };

    $.ajax({
        url: ApiURL + "SetData/AssingLoginFailureDetails",
        type: 'POST',
        data: Obj_Login,
        async: false,
        success: success_ValidateBlockData,
        error: function () {
            AlertBox("warning", " MI Login ", "No Insert Data in FailureDetails !")
            $('#txtUserName').focus();            
        }
    });

    function success_ValidateBlockData(jsonData_ValidateBlockData) {
        var str = jsonData_ValidateBlockData;
        if (jsonData_ValidateBlockData == null) {
            $("#loader").attr("style", "display:none");
            return false;
        }
        for (var i = 0; i < jsonData_ValidateBlockData.length; i++) {
            if (jsonData_ValidateBlockData[0].cBlockedFlag == "B") {
                Flag_ValidateBlocked = "true";
                AlertBox("warning", " MI Login ", "Your username is locked due to multiple incorrect login attempts. Please contact your Administrator!")
                $('#txtUserName').focus();                
                $("#loader").attr("style", "display:none");
                return false;
            }
            else {
                Flag_ValidateBlocked = "false";

            }
        }
    }
}

$('#txtMobileNumber').keydown(function (e) {
    if (e.shiftKey || e.ctrlKey || e.altKey) {
        e.preventDefault();
    } else {
        var key = e.keyCode;
        if (!((key == 8) || (key == 46) || (key >= 35 && key <= 40) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105))) {
            e.preventDefault();
        }
    }
});

$('#OTPInput').keydown(function (e) {
    if (e.shiftKey || e.ctrlKey || e.altKey) {
        e.preventDefault();
    } else {
        var key = e.keyCode;
        if (!((key == 8) || (key == 46) || (key >= 35 && key <= 40) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105))) {
            e.preventDefault();
        }
    }
});

document.onkeydown = function (e) {
    if (event.keyCode == 123) {
        return false;
    }
    if (e.ctrlKey && e.shiftKey && e.keyCode == 'I'.charCodeAt(0)) {
        return false;
    }
    if (e.ctrlKey && e.shiftKey && e.keyCode == 'C'.charCodeAt(0)) {
        return false;
    }
    if (e.ctrlKey && e.shiftKey && e.keyCode == 'J'.charCodeAt(0)) {
        return false;
    }
    if (e.ctrlKey && e.keyCode == 'U'.charCodeAt(0)) {
        return false;
    }
}

document.addEventListener('contextmenu', function (e) {
    e.preventDefault();
});

// Forgot Password
$(document).ready(function () {

    $(document).on("click", "#btnSubmit", function (e) {
        var Newpwd = $("#txtNewPassword").val();
        var ConfNewpwd = $("#txtNewConfPass").val();
        var Isblank = 0;
        var InValid = 0;

        if (Newpwd == "") {
            $("#NpwdErrmsg").html('');
            $("#NpwdErrmsg").html('Please Enter New Password');
            Isblank = 1;
        }
        else if (ConfNewpwd == "") {
            $("#CofNpwdErrmsg").html('');
            $("#CofNpwdErrmsg").html('Please Enter New Confirm Password');
            Isblank = 1;
        }
        else if (Newpwd == "" && ConfNewpwd == "") {
            $("#NpwdErrmsg").html('');
            $("#NpwdErrmsg").html('Please Enter New Password');
            $("#CofNpwdErrmsg").html('');
            $("#CofNpwdErrmsg").html('Please Enter New Confirm Password');
            Isblank = 1;
        }
        else {
            Isblank = 0;
            if (Newpwd != ConfNewpwd) {
                $("#CofNpwdErrmsg").html('');
                $("#CofNpwdErrmsg").html('New Password And Confirm New Password Does Not Match');
            }
        }

        var NpwdErrmsg = $("#NpwdErrmsg").text().length;
        var ConfNpwdErrmsg = $("#CofNpwdErrmsg").text().length;
        if (NpwdErrmsg > 1) {
            InValid = 1;
        }
        else if (ConfNpwdErrmsg > 1) {
            InValid = 1;
        }
        else if (NpwdErrmsg > 1 && ConfNpwdErrmsg > 1) {
            InValid = 1;
        }
        else {
            InValid = 0;
        }

        if (Isblank == 0 && InValid == 0) {
            var Uname = $("#hdnUName").val();
            var UId = $("#hdnUId").val();
            $.ajax({
                url: WebURL + "MILogin/CheckPwd",
                type: 'GET',
                data: {
                    Username: Uname,
                    iModifyBy: UId,
                    ID: UId,
                    NewPassword: Newpwd,
                    ConfnewPassword: ConfNewpwd
                },
                dataType: "text",
                async: false,
                success: OnSuccessChnage,
                error: onError
            });

            function OnSuccessChnage(data) {
                if (data == "success") {
                    $("#SuccessModal").modal('show');
                }
            }
        }
    })

    $(document).on("click", "#btnBacktoLogin", function (e) {
        var redirectUrl = $('#redirectToLogin').val();
        location.href = redirectUrl;
        return false;
    })

    $(document).on("click", "#btnSuccessToLogin", function (e) {
        var redirectUrl = $('#redirectToLogin').val();
        location.href = redirectUrl;
        return false;
    })
})

var Password_Minlen = 0;
function CheckValidation() {
    var Newpwd = $("#txtNewPassword").val();
    if (Newpwd == "") {
        $("#NpwdErrmsg").html('');
        $("#NpwdErrmsg").html('Please Enter New Password');
    }
    else {
        $("#NpwdErrmsg").html('');

        var PrevCnt = 0;
        $.ajax({
            url: ApiURL + "GetData/GetPasswordPolicyData",
            type: 'POST',
            data: null,
            dataType: 'json',
            success: SuccessPolicyData,
            async: false,
            error: function (ex) {
                AlertBox("warning", " MI Forgot Password ", "Failed to retrieve Password Policy Data");
            }
        });

        function SuccessPolicyData(jsonData) {
            if (jsonData != "") {
                for (var i = 0; i < jsonData.length ; i++) {
                    if (jsonData[i].nPolicyNo == "5") {
                        Password_Minlen = jsonData[i].vValue;
                    }
                    else if (jsonData[i].nPolicyNo == "4") {
                        PrevCnt = jsonData[i].vValue;
                    }
                }
            }
        }

        if (Newpwd.length < Password_Minlen) {
            $("#NpwdErrmsg").html('');
            $("#NpwdErrmsg").html('New Password Length Should Be Minimum ' + Password_Minlen + ' Characters');
        }
        else {
            $("#NpwdErrmsg").html('');

            var EncrPwd = "";
            $.ajax({
                url: WebURL + "MILogin/GetEncrPwd",
                type: 'GET',
                data: { pwd: Newpwd },
                dataType: 'json',
                success: SuccessEncrPwd,
                async: false,
                error: function (ex) {
                    AlertBox("warning", " MI Forgot Password ", "Failed to Encrypt Password");
                }
            });

            function SuccessEncrPwd(jsonData) {
                EncrPwd = jsonData;
            }


            var UId = $("#hdnUId").val();
            $.ajax({
                url: ApiURL + "GetData/GetPasswordHistory",
                type: 'POST',
                data: { ID: UId },
                dataType: 'json',
                success: SuccessHistory,
                async: false,
                error: function (ex) {
                    AlertBox("warning", " MI Forgot Password ", "Failed to retrieve Password History Data");
                }
            });

            function SuccessHistory(jsonData) {
                if (jsonData != "") {
                    for (var i = 0; i < jsonData.length; i++) {
                        if (i == PrevCnt) {
                            break;
                        }
                        if (jsonData[i].vPassword == EncrPwd) {
                            $("#NpwdErrmsg").html('');
                            $("#NpwdErrmsg").html('Password Already Exists In The List Of Previous ' + PrevCnt + ' Passwords !!');
                            break;
                        }
                    }
                }
            }

            var resAlhpa = "true";
            var resChar = "true";
            var alphaExp = /^[0-9]+$/;
            var charExp = /^[a-zA-Z]+$/;
            //var pwdRegEx = /^(?=.*\d)(?=.*[a-z])\w{6,}$/;
            var pwdRegEx = /^(?=.*\d)(?=.*[a-zA-Z])(?=.*[!@#$%])(?!.*\s)/;

            if (Newpwd.length >= Password_Minlen) {
                if (!Newpwd.match(pwdRegEx)) {
                    $("#NpwdErrmsg").html('');
                    $("#NpwdErrmsg").html('Password Must Contain Atleast One Alphabet [A-Z] And One Digit [0-9] And One Special Character [!@#$%^&*] !!!');
                    return false;
                }
            }
        }
    }
}

function ConfCheckValidation() {
    var ConfNewpwd = $("#txtNewConfPass").val();
    if (ConfNewpwd == "") {
        $("#CofNpwdErrmsg").html('');
        $("#CofNpwdErrmsg").html('Please Enter Confirm New Password');
    }
    else {
        $("#CofNpwdErrmsg").html('');
        if (ConfNewpwd.length < Password_Minlen) {
            $("#CofNpwdErrmsg").html('');
            $("#CofNpwdErrmsg").html('Confirm New Password Length Should Be Minimum ' + Password_Minlen + ' Characters');
        }
        else {
            $("#CofNpwdErrmsg").html('');
            var Newpwd = $("#txtNewPassword").val();
            if (Newpwd != "") {
                if (Newpwd != ConfNewpwd) {
                    $("#CofNpwdErrmsg").html('');
                    $("#CofNpwdErrmsg").html('New Password And Confirm New Password Does Not Match');
                }
                else {
                    var resAlhpa = "true";
                    var resChar = "true";
                    var alphaExp = /^[0-9]+$/;
                    var charExp = /^[a-zA-Z]+$/;
                    var pwdRegEx = /^(?=.*\d)(?=.*[a-zA-Z])(?=.*[!@#$%])(?!.*\s)/;

                    //if (ConfNewpwd.length > 5) {
                    //    if (ConfNewpwd.match(alphaExp)) {
                    //        resAlhpa = "true";
                    //    }
                    //    else {
                    //        resAlhpa = "false";
                    //    }

                    //    if (ConfNewpwd.match(charExp)) {
                    //        resChar = "true";
                    //    } else {
                    //        resChar = "false";
                    //    }
                    //}

                    if (!ConfNewpwd.match(pwdRegEx)) {
                        $("#CofNpwdErrmsg").html('');
                        $("#CofNpwdErrmsg").html('Password Must Contain Atleast One Alphabet [A-Z] And One Digit [0-9] And One Special Character [!@#$%^&*] !!!');
                        return false;
                    }
                }
            }
        }
    }
}