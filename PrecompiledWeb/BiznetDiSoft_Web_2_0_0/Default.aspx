<%@ page language="VB" autoeventwireup="false" inherits="_Default, App_Web_eoahe1pj" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<!DOCTYPE html>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <meta charset="UTF-8" />
    <title>Lambda Therapeutic Research .</title>
    <link rel="stylesheet" href="App_Themes/font-awesome.min.css" />
    <link rel="stylesheet" href="App_Themes/LoginStyle.css" type="text/css" />

    <link rel="shortcut icon" type="image/x-icon" href="images/biznet.ico" />
    <%--<script type="text/javascript" src="Script/Login/custom.min.js"></script>--%>
    <script src="Script/Login/jquery.min.js"></script>
    <script type="text/javascript" src="Script/sweetalert.js"></script>
    <link href="App_Themes/sweetalert.css" rel="Stylesheet" type="text/css" />
    <style>
        .hrclass {
            margin-top: 87px;
        }

        @font-face {
            font-family: 'password';
            font-style: normal;
            font-weight: 400;
        }

        #hdnpassword {
            font-family: 'password';
        }

        .spnLoginValidationMsg {
            text-align: left;
        }
    </style>

</head>
<body>

    <div <%--class="loginbg" style="float: left;"--%>>
        <ul class="cb-slideshow">
            <li><span>Image 01</span><div></div>
            </li>
            <li><span>Image 02</span><div></div>
            </li>
            <li><span>Image 03</span><div></div>
            </li>
            <li><span>Image 04</span><div></div>
            </li>
            <li><span>Image 05</span><div></div>
            </li>
            <li><span>Image 06</span><div></div>
            </li>
        </ul>
    </div>
    <form id="Form1" runat="server" autocomplete="off" class="form">
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="1000" EnablePageMethods="True"></asp:ScriptManager>
        <div id="qodef-particles" class="fixed" style="background-color: #f9f9f9;" data-particles-density="high" data-particles-color="#9fd3ff"
            data-particles-opacity="0.8" data-particles-size="3" data-speed="3" data-show-lines="yes"
            data-line-length="100" data-hover="yes" data-click="yes">

            <div id="qodef-p-particles-container" style="position: absolute; width: 100%; height: 100%;">
                <canvas class="particles-js-canvas-el" width="1349" height="635"></canvas>
            </div>
                        
            <div class="loginbox">
                <div class="login">
                    <header class="login-header">
                        <span class="loader"></span>
                        <span class="text">
                            <img src="images/biznet-logo.png" style="" alt="Biznet" /></span>
                    </header>

                    <a href="http://www.biznet-ctm.in/" class="loginbox_link_center" target="_blank">
                        <img src="images/biznet-logo.png" alt="Biznet" class="LoginImageAlign" />
                    </a>
                    <%--<div style="position: fixed; width: 17%; margin-top: 9%">
                        <span id="spanloginfailedMessage" class="spnLoginValidationMsg"></span>
                        <span id="spnMsgLogin" class="spnLoginValidationMsg"></span>
                        <span id="spnMsgPassword" class="spnLoginValidationMsg"></span>
                    </div>--%>

                    <%--<p class="title">&nbsp;&nbsp;</p>--%>
                    <hr class="hrclass" />

                    <div>
                        <input id="txtUserName" class="login-input inputmargin" name="uname" runat="server" type="text"
                            placeholder="Username" onblur="SetProfiles();" tabindex="1" onkeyup="CheckUserName()" />
                        <i class="fa fa-user"></i>
                    </div>

                    <%--<asp:TextBox ID="hdnpassword" runat="server" onchange="pwdchk();" name="password" oncopy="return false" onpaste="return false" TabIndex="2"
                        oncut="return false" placeholder="Password" class="td-ie8 login-input" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>--%>
                    <input id="hdnpassword" type="password" name="password" oncopy="return false" onpaste="return false" tabindex="2" data-encrypted="password"
                        oncut="return false" placeholder="Password" class="td-ie8 login-input" autocomplete="off" value="<%=NamePass %>" />
                    <i class="fa fa-key"></i>


                    <asp:UpdatePanel ID="UPProfiles" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                        <ContentTemplate>
                            <i style="margin-bottom: -43px;" id="textinput" class="icon-list-alt "></i>
                            <asp:DropDownList runat="server" ID="ddlProfile" CssClass="td-ie8" TabIndex="3"></asp:DropDownList>
                            <i class="fa fa-list-alt" aria-hidden="true"></i>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnMediator" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>

                    <div style="margin-bottom: 10px">
                        <asp:Label ID="lblOtpmsg" runat="server" Text="OTP has been Sent on below Address." Style="color: red; font-size: 12px;" Visible="false"></asp:Label>
                        <asp:Label ID="lblOtpEmailMsg" runat="server" Style="color: red; font-size: 12px;" Visible="false"></asp:Label><br />
                        <asp:Label ID="lblOtpSmsMsg" runat="server" Style="color: red; font-size: 12px;" Visible="false"></asp:Label>
                    </div>

                    <div id="dvotp" runat="server" visible="false" style="margin-bottom: -25px;">
                        <asp:TextBox ID="txtotp" runat="server" name="Otp" TabIndex="3" MaxLength="6"
                            placeholder="Enter OTP"></asp:TextBox>
                        <%--<p style="font-size: 11px; color: black; text-align: left;">
                            <span id="txttimer">Time left = </span><span id="timer"></span>
                        </p>--%>

                        <asp:LinkButton ID="lnkresendOTP" runat="server" ToolTip="Resend Otp" Enabled="true" Text="Resend Otp" TabIndex="5"
                        Style="text-decoration: none; visibility: hidden" OnClick="lnkresendOTP_Click">
                            <p style="font-size: 11px; color: red; text-align:right; margin-bottom:-25px;"> ReSend OTP</p>
                    </asp:LinkButton>
                    </div>
                    
                    <div style="float:left;">
                        <span id="spanloginfailedMessage" class="spnLoginValidationMsg"></span>
                        <span id="spnMsgLogin" class="spnLoginValidationMsg"></span>
                        <span id="spnMsgPassword" class="spnLoginValidationMsg"></span>
                        <span id="spnRemainAttemp" style="margin-top: 10px; color: #203054" class="spnLoginValidationMsg"></span>
                    </div>
                    
                    <button runat="server" id="ImgBtnLogin" onclick="return ValidateLogin();" tabindex="4" title="Login">
                        <span class="state">Login</span>
                    </button>
                    <button runat="server" id="ImgBtnReLogin" visible="false" onclick="return ValidateReLogin();" tabindex="4" title="Login" style="margin-top: 50px !important">
                        <span class="state">Login</span>
                    </button>
                    <asp:LinkButton ID="lnkForgotPassword" runat="server" ToolTip="Forgot Password" Enabled="true" Text="Forgot Password" TabIndex="5"
                        OnClientClick=" return ValidateForgotPassword()" OnClick="ForgotPassword" Style="text-decoration: none; color: red;" class="forgot">
                            <p style="padding: 0;margin: 0;font-size: 14px;color: #0026ff; text-decoration-line:underline;"> Forgot Password ?</p>
                    </asp:LinkButton>

                    <asp:Button runat="server" ID="btnMediator" Style="display: none;" Text="Mediator" />
                    <asp:Button ID="btnLogin" runat="server" Style="display: none;" />
                    <asp:Button ID="BtnReLogin" runat="server" Style="display: none;" />
                    <asp:Button ID="btnLoginAgain" runat="server" Style="display: none;" OnClick="btnLoginAgain_Click" OnClientClick="clickButton()" />
                    <asp:Button ID="btnSessionRemove" runat="server" Style="display: none;" />
                    <asp:HiddenField ID="hdntemp" runat="server" />
                    <asp:HiddenField ID="OTPGenerated" runat="server" />
                    <asp:HiddenField ID="OTPEntered" runat="server" />
                    <asp:Label ID="client" runat="server" />

                    <div id="updateProgress" class="updateProgress" style="display: none; vertical-align: middle">
                        <div align="center">
                            <table>
                                <tr>
                                    <td style="height: 130px">
                                        <font class="updateText">Please Wait...</font>
                                    </td>
                                    <td style="height: 130px">
                                        <div title="Wait" class="updateImage">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <footer style="bottom: 0; position: absolute;">
                    <div class="h5footer">
                        <a href="http://www.sarjen.com/" target="_blank" style="text-decoration: none;" data-tooltip="07 July 2021">
                            <h5 class="h5alignfooter">©
                                <script>document.write(new Date().getFullYear())</script>
                                , Sarjen Systems Pvt. Ltd.</h5>
                            <h5 class="versionalign" id="h5version"></h5>
                        </a>
                    </div>
                </footer>
            </div>
        </div>
        <script type="text/javascript" src="Script/General.js"></script>
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:HiddenField ID="HiddenField2" runat="server" />
        <asp:HiddenField ID="hdnRelogin" runat="server" Value="False" />

        <asp:UpdatePanel ID="upHidden" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="hdnIsAllowADUser" runat="server" />
                <asp:HiddenField ID="hdnADUserName" runat="server" />
                <asp:HiddenField ID="hdntempPassWord" runat="server" />
                <asp:HiddenField ID="HiddenField3" runat="server" Value="False"/>
            </ContentTemplate>
        </asp:UpdatePanel>

        <script type="text/javascript">
            $("#hdnpassword").on("blur", function () {
                $("#spnMsgPassword").html("");
            });

            document.getElementById("h5version").innerText = "<%= System.Configuration.ConfigurationManager.AppSettings("Version_Env") %>";

            function CheckUserName() {
                var name = document.getElementById('txtUserName').value;
                //same change in vb page on click btnMediator_Click
                document.getElementById('txtUserName').value = name.replaceAll('\'', '').replaceAll('\"', '').replaceAll('<', '').replaceAll('>', '').
                replaceAll('=', '').replaceAll('/', '').trim()
            }

            function SpanLoginFailedMessage() {
                document.getElementById('spanloginfailedMessage').style.display = "block";
                document.getElementById('spanloginfailedMessage').innerHTML = "Login Failed. Try Again !"
                var txtUserName = document.getElementById('<%= txtUserName.ClientId%>');
                txtUserName.focus();
                $('.login').addClass('focused');
            }

            function spnRemainAttemp(remainattemp) {
                document.getElementById('spnRemainAttemp').style.display = "block";
                document.getElementById('spnRemainAttemp').innerHTML = "Remaining Attempts Are : " + remainattemp;
            }

            if (!((typeof window.chrome === "object") || (navigator.userAgent.indexOf("iPad") >= 0 && navigator.userAgent.indexOf("CriOS") >= 0)) || (navigator.userAgent.indexOf('Edge') >= 0)) {
                msgalert("Please use Google Chrome!");
                $(".form").hide();
            }

            //function pwdchk() {
            //    var password = null;
            //    password = document.createElement("INPUT");
            //    document.createElement("INPUT").value = null;
            //    password.type = "hidden";
            //    password.name = "password";
            //    password.id = "password";
            //    password.value = null;
            //    password.value = jQuery("#hdnpassword").val();
            //    document.forms[0].appendChild(password);
            //    return true;
            //}

            var check = false;
            var returnValue;
            function SetProfiles() {
                $("#spnMsgLogin").html("");
                if (document.getElementById('<%= txtUserName.ClientId%>').value.trim() != '') {
                    var btn = document.getElementById('<%= btnMediator.ClientId%>');
                    btn.click();
                    var txtPassword = $("#hdnpassword").val();
                    $("#hdnpassword").focus()
                }
            }

            function Changeurl() {
                var currenturl = location.href
                location.href = currenturl.split("?")[0];
                document.getElementById('<%= btnSessionRemove.ClientID%>').click();
            }

            function AlertAndRedirect(URL, msg) {
                msgalert(msg);
                window.location.href = URL;
            }

            function is_mobile() {
                if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
                    return true;
                }
                return false;
            }

            function ValidateLogin() {
                $(".spnLoginValidationMsg").html("");
                try {
                    var txtUserName = document.getElementById('<%= txtUserName.ClientId%>');
                    var txtPassword = $("#hdnpassword").val();
                    var profile = document.getElementById('<%= ddlProfile.ClientId%>');
                    var userAgent = navigator.userAgent.toLowerCase();

                    if (txtUserName.value.toString().trim().length <= 0) {
                        txtUserName.focus();
                        $("#spnMsgLogin").html("Please enter user name !");
                        document.getElementById('spnMsgLogin').style.display = 'block';
                        return false;
                    }
                    else if (txtPassword.trim().length <= 0) {
                        txtPassword.focus();
                        $("#spnMsgPassword").html("Please enter password !");
                        document.getElementById('spnMsgPassword').style.display = 'block';
                        return false;
                    }
                    else if (profile.value.toString().trim().length <= 0) {
                        $("#spnMsgLogin").html("Please Select Profile !");
                        document.getElementById('spnMsgLogin').style.display = 'block';
                        profile.focus();
                        return false;
                    }
                    if (window.ss && ss.enc && ss.enc.process) {
                        ss.enc.process();
                        $("#hdnpassword").val(ss.enc.getFieldValue('hdnpassword'));
                    }
                    PageMethods.CheckUserScope(txtUserName.value, profile.value, //Added By Dipen Shah on 17-Feb-2015 to check usertype.
                        function (resopnse) {
                            returnValue = resopnse;
                            resopnse = resopnse.split("-")[0];
                            if (resopnse == "Success") {
                                if (is_mobile()) {
                                    if (((typeof window.chrome === "object") || (navigator.userAgent.indexOf("iPad") >= 0 && navigator.userAgent.indexOf("CriOS") >= 0))) {
                                        check = true;
                                        document.getElementById('<%= btnLogin.ClientID%>').click();
                                    }
                                    else {
                                        msgalert("Please use Google Chrome!");
                                        $(".form").hide();
                                        check = false;
                                        return false;
                                    }
                                }
                                else {
                                    var UserAgent = navigator.userAgent.toLowerCase;
                                    if (navigator.userAgent.search("Chrome") > 0) {
                                        userAgent = userAgent.substring(userAgent.indexOf('chrome/') + 7);
                                        if ((navigator.userAgent.search("Chrome") > 0) && userAgent.substring(0, userAgent.indexOf('.')) < 22) {
                                            msgalert("Please use Browser version greater than 21!");
                                            check = false;
                                            return false;
                                        }
                                        else {
                                            check = true;
                                            document.getElementById('<%= btnLogin.ClientID%>').click();
                                        }
                                    }
                                    else {
                                        msgalert("Please use Google Chrome!");
                                        document.getElementById('<%= ImgBtnLogin.ClientID %>').style.display = "none";
                                        document.getElementById('<%=btnLogin.ClientID%>').style.display = "none";
                                        check = false;
                                        return false;
                                    }
                                }
                            }
                            else {
                                alert("error");
                            }
                        });
                        return false;

                    }
                    catch (err) {
                        alert(err.message);
                    }
                }

                function ValidateReLogin() {
                    var Result = "";
                    $(".spnLoginValidationMsg").html("");
                    try {
                        var txtUserName = document.getElementById('<%= txtUserName.ClientId%>');
                        var txtPassword = document.getElementById("hdnpassword"); //$("#hdnpassword").val();
                        var profile = document.getElementById('<%= ddlProfile.ClientId%>');
                        var userAgent = navigator.userAgent.toLowerCase();

                        if (txtUserName.value.toString().trim().length <= 0) {
                            txtUserName.focus();
                            $("#spnMsgLogin").html("Please enter user name !");
                            document.getElementById('spnMsgLogin').style.display = 'block';
                            return false;
                        }
                        else if (txtPassword.value.toString().trim().length <= 0) {
                            txtPassword.focus();
                            $("#spnMsgPassword").html("Please enter password !");
                            document.getElementById('spnMsgPassword').style.display = 'block';
                            return false;
                        }
                        else if (profile.value.toString().trim().length <= 0) {
                            $("#spnMsgLogin").html("Please select Profile !");
                            document.getElementById('spnMsgLogin').style.display = 'block';
                            profile.focus();
                            return false;
                        }
                        else if (txtotp.value.toString().trim().length <= 0) {
                            $("#spnMsgOTP").html("Please enter OTP !");
                            document.getElementById('spnMsgOTP').style.display = 'block';
                            txtotp.focus();
                            return false;
                        }
                        else if (txtotp.value.toString().trim().length != 0) {
                            debugger;
                            var OTP = txtotp.value.toString();
                            var result = $.ajax({
                                type: "POST",
                                url: "Default.aspx/GetOTPInfo",
                                data: JSON.stringify({ 'txtOtpNo': OTP }),
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (msg) {
                                    if (msg.d == "Success") {
                                        Result = msg.d;
                                        window.onUpdating();
                                    }
                                    else {
                                        $("#txtotp").val("");
                                        alert("Invalid OTP!");
                                        return false;
                                    }
                                },
                                failure: function (msg) {
                                    return false;
                                },
                                error: function (xhr, err) {
                                    alert(err);
                                    return false;
                                }
                            });
                        }
                        PageMethods.CheckUserScope(txtUserName.value, profile.value, //Added By Dipen Shah on 17-Feb-2015 to check usertype.
                            function (resopnse) {
                                returnValue = resopnse;
                                resopnse = resopnse.split("-")[0];
                                if (resopnse == "Success") {
                                    if (is_mobile()) {
                                        if (((typeof window.chrome === "object") || (navigator.userAgent.indexOf("iPad") >= 0 && navigator.userAgent.indexOf("CriOS") >= 0))) {
                                            check = true;
                                            document.getElementById('<%= BtnReLogin.ClientID%>').click();
                                        }
                                        else {
                                            msgalert("Please use Google Chrome!");
                                            $(".form").hide();
                                            check = false;
                                            return false;
                                        }
                                    }
                                    else {
                                        var UserAgent = navigator.userAgent.toLowerCase;
                                        if (navigator.userAgent.search("Chrome") > 0) {
                                            userAgent = userAgent.substring(userAgent.indexOf('chrome/') + 7);
                                            if ((navigator.userAgent.search("Chrome") > 0) && userAgent.substring(0, userAgent.indexOf('.')) < 22) {
                                                msgalert("Please use browser version greater than 21!");
                                                check = false;
                                                return false;
                                            }
                                            else {
                                                check = true;
                                                if (Result == "Success") {
                                                    document.getElementById('<%= BtnReLogin.ClientID%>').click();
                                    }
                                }
                            }
                            else {
                                msgalert("Please use Google Chrome!");

                                document.getElementById('<%= ImgBtnReLogin.ClientID %>').style.display = "none";
                                document.getElementById('<%=BtnReLogin.ClientID%>').style.display = "none";

                                        check = false;
                                        return false;
                                    }
                                }

                            }
                            else {
                                alert("error");
                            }
                            });
                        return false;

                    }
                    catch (err) {
                        alert(err.message);
                    }
                }

                function pageLoad() {
                    try {
                        $.ajax({
                            type: "post",
                            url: "Default.aspx/SetPageLoad",
                            data: [],
                            contentType: "application/json; charset=utf-8",
                            datatype: JSON,
                            async: false,
                            dataType: "json",
                            success: function (data) {
                                if (data.d == "Y") {
                                    document.getElementById('<%=lnkForgotPassword.ClientID%>').style.display = "";
                                }
                                else {
                                    document.getElementById('<%=lnkForgotPassword.ClientID%>').style.display = "none";
                                }
                            }
                        });
                        //document.cookie = "Theme=Blue";
                    }
                    catch (error) {
                        msgalert(err.message);
                    }
                }

                function confirmLoginAgain(strIpAddress, strUserAgent, currentIPAddtess, currentUserAgent, user) {  //added by Rahul Shah on 08-Apr-2015
                    $('.login').addClass('focused');
                    swal({
                        title: "",
                        text: "\"" + user + "\" is already logged in " + strUserAgent + " Browser of " + strIpAddress + " System. " + "Are you sure want to continue Login with Current " + currentUserAgent + " Browser of " + currentIPAddtess + " System",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: '#EB7140',
                        confirmButtonText: '',
                        closeOnConfirm: false
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                document.getElementById('<%= btnLoginAgain.ClientID%>').click();
                                swal.close();
                                return false;
                            }
                            else {
                                var str = location.href;
                                var abc = str.split('?');
                                var url = location.href;
                                if (abc.length == 1) {
                                    url = abc[0] + '?SessionExpire=True';
                                    window.open(url, "_self");
                                    document.getElementById('<%= hdntemp.ClientID%>').val("1");
                            }
                            else {
                                if (abc[1].indexOf("SessionExpire") >= 0) {
                                    window.open(url, "_self");
                                    document.getElementById('<%= hdntemp.ClientID%>').val("1");
                                }
                                else if (abc[1].indexOf("username") >= 0) {
                                    document.getElementById('<%= hdntemp.ClientID%>').val("1");
                                }
                        }
                        swal.close();
                        return true;
                    }
                        });
            }

            function clickButton() {
                //alert("Click Button");
            }
            function getForm() {
                $("#frmMain").removeAttr("style");
                $("#imgLogo").attr("style", "display:none");
            }

            function ValidateForgotPassword() {
                try {
                    var txtUserName = document.getElementById('<%= txtUserName.ClientId%>');
                    if (txtUserName.value.toString().trim().length <= 0) {
                        $("#spnMsgLogin").html("Please enter user name !");
                        document.getElementById('spnMsgLogin').style.display = 'block';
                        txtUserName.focus();
                        return false;
                    }
                    var profile = document.getElementById('<%= ddlProfile.ClientID%>').selectedIndex;
                    if (profile < 0) {
                        $("#spnMsgLogin").html("Please Enter Valid UserName !");
                        document.getElementById('spnMsgLogin').style.display = 'block';
                        return false;
                    }
                    var confirm_value = document.createElement("INPUT");
                    confirm_value.type = "hidden";
                    confirm_value.name = "confirm_value";
                    if (confirm("Do you want to Reset Password?")) {
                        confirm_value.value = "Yes";
                    }
                    else {
                        confirm_value.value = "No";
                    }
                    document.forms[0].appendChild(confirm_value);
                    return true;
                }
                catch (err) {
                    msgalert(er.message);
                }
            }

            function OTPPrompt() {
                var OTPEntered = prompt("Enter OTP : ", "Please Enter OTP Here!");
                var OTPGenerated = document.getElementById('<%=OTPGenerated.ClientID%>').value

                if (OTPEntered != null) {
                    if (OTPEntered == OTPGenerated) {
                        window.open("frmForgotPassword.aspx", "_self");
                    }
                    else {
                        msgalert("Your Entered OTP is Not Matched \nPlease Try Again Later")
                    }
                }
            }

            let timerOn = true;
            function timer(remaining) {

                var m = Math.floor(remaining / 60);
                var s = remaining % 60;

                m = m < 10 ? '0' + m : m;
                s = s < 10 ? '0' + s : s;
                document.getElementById('timer').innerHTML = m + ':' + s;
                remaining -= 1;

                if (remaining >= 0 && timerOn) {
                    setTimeout(function () {
                        timer(remaining);
                    }, 1000);
                    return;
                }

                if (remaining < 1) {
                    // Do validate stuff here
                    document.getElementById("lnkresendOTP").style.visibility = 'visible';
                    document.getElementById('timer').style.visibility = 'hidden';
                    document.getElementById('txttimer').style.visibility = 'hidden';

                    timer = false;
                    return;
                }
                else {
                    //document.getElementById("lnkresendOTP").innerHTML = "Nilay!";

                    return;
                }

                // Do timeout stuff here
                alert('Timeout for otp');
            }

            //timer(120);

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


        </script>

        <%--<div class="sticky-container">
		<ul class="sticky">
			<li>
				<img title="" alt="" src="images/link.png" />
				<a href="https://www.cdisc.org/standards" target="_blank" class="SidePanelGuidelines">CDISC-SDTM Standard</a>
				<a href="http://www.ema.europa.eu/ema/" target="_blank" class="SidePanelGuidelines">EMA</a>
				<a href="https://www.fda.gov/ScienceResearch/SpecialTopics/RunningClinicalTrials/GuidancesInformationSheetsandNotices/default.htm" target="_blank" class="SidePanelGuidelines">USFDA</a>
				<a href="http://www.cdsco.nic.in/forms/Default.aspx" target="_blank" class="SidePanelGuidelines">CDSCO</a>
				<a href="https://www.ncbi.nlm.nih.gov/" target="_blank" class="SidePanelGuidelines">NCBI</a>
			</li>
		</ul>
	</div> --%>
    </form>
</body>
<script type="text/javascript" src="Script/Login/third-party.min.js"></script>
<script type="text/javascript" src="Script/Login/modules.min.js"></script>
<script type="text/javascript" src="Script/Login/index.js"></script>
<script type="text/javascript" src="Script/Login/jquery-ui.js"></script>
</html>