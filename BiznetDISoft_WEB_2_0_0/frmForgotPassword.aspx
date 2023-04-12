<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmForgotPassword.aspx.vb"
    Inherits="frmForgotPassword" ValidateRequest="false" EnableEventValidation="false" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title>Forgot Password</title>
    <link rel="stylesheet" href="App_Themes/font-awesome.min.css" />
    <link rel="stylesheet" href="App_Themes/LoginStyle.css" type="text/css" />
    <script type="text/javascript" src="Script/Login/jquery.min.js"></script>
    <script type="text/javascript" src="Script/sweetalert.js"></script>
    <link href="App_Themes/sweetalert.css" rel="Stylesheet" type="text/css" />
    <link href="App_Themes/StyleCommon/CommonStyle.css" rel="Stylesheet" type="text/css" />

    <script type="text/javascript" src="Script/Validation.js"></script>
    <style type="text/css">
        #txtNewPassword {
            -webkit-text-security: disc !important;
            -mox-text-security: disc !important;
        }

        #txtNewConfPass {
            -webkit-text-security: disc !important;
            -mox-text-security: disc !important;
        }
    </style>
</head>
<body>
    <script type="text/javascript">
        function CheckVal() {
            debugger;
            var minlen = $("#txtpwdlen").val();
            if (!checkVal(document.getElementById('<%=txtNewPassword.ClientID%>').value, '<%=txtNewPassword.ClientID%>', '8')) {
                msgalert("Please Enter New Password");
                document.getElementById('<%=txtNewPassword.ClientID%>').focus();
                 return false;
             }
             else if (!checkVal(document.getElementById('<%=txtNewConfPass.ClientID%>').value, '<%=txtNewConfPass.ClientID%>', '8')) {
                msgalert("Please Enter New Confirm Password");
                document.getElementById('<%=txtNewConfPass.ClientID%>').focus();
                 return false;
             }
             else if (Password.length < minlen) {
                 msgalert("Password Length Should Be Minimum " + minlen + " Characters");
                 document.getElementById('<%=txtNewPassword.ClientID%>').focus();
                  return false;
              }
              else if ((document.getElementById('<%=txtNewConfPass.ClientID%>').value != document.getElementById('<%=txtNewPassword.ClientID%>').value)) {
                  msgalert("New Password And Confirm New Password Does Not Match");
                  document.getElementById('<%=txtNewConfPass.ClientID%>').focus();
                return false;
            }
            else {
                return ValidatePassword();
            }
}

function ValidatePassword() {
    //(?=.*[A-Z])
    var resAlhpa = "true";
    var resUpperChar = "true";
    var resLowerChar = "true";
    var alphaExp = /^[0-9]+$/;
    var UppercharExp = /^[A-Z]+$/;
    var LowercharExp = /^[a-z]+$/;
    var SpecialcharExp = /[!#*$@%.&]/g;
    var pwdRegEx = /^(?=.*\d)(?=.*[a-z])\w{8,}$/;
    var txtPWD = document.getElementById('<%=txtNewPassword.ClientID%>');

    if (txtPWD.value.length > 7) {
        if (txtPWD.value.match(alphaExp)) {
            resAlhpa = "true";
        }
        else {
            resAlhpa = "false";
        }

        if (txtPWD.value.match(UppercharExp)) {
            resUpperChar = "true";
        } else {
            resUpperChar = "false";
        }

        if (txtPWD.value.match(LowercharExp)) {
            resLowerChar = "true";
        } else {
            resLowerChar = "false";
        }
    }

    if (resAlhpa == "false" && resUpperChar == "false" && resLowerChar == "false") {
        return true;
    } else {
        msgalert('Password Must Be More Than 7 Characters.\nPassword Must Contain Atleast One Upper Alphabet [A-Z].\nPassword Must Contain Atleast One Lower Alphabet [a-z].\nPassword Must Contain Atleast One Digit [0-9].\nPassword Must Contain Atleast One Special Character.');
        txtPWD.focus();
        return false;
    }
    return true;
}
function AlertAndRedirect(URL, msg) {
    //msgalert(msg);
    window.location.href = URL;
}
function AlertAndClose(msg) {
    msgalert(msg);
    window.close();
}
function pageToRedirect() {
    if (confirm('Are You Sure You Want To Exit?')) {
        if (window.opener) {
            window.close();
        }
        else {
            return true;
        }
        return false;
    }
    else {
        return false;
    }
    return true;
}
function pageToExit() {
    try {
        var confirm_value = document.createElement("INPUT");
        confirm_value.type = "hidden";
        confirm_value.name = "confirm_value";
        confirm_value.value = null;
        if (confirm('Are You Sure You Want To Exit?')) {
            confirm_value.value = "Yes";

        } else {
            confirm_value.value = "No";
        }
        document.forms[0].appendChild(confirm_value);
    }
    catch (err) {
        msgalert(er.message);
    }
}
function pwd() {
    var newpassword = null;
    newpassword = document.createElement("INPUT");
    document.createElement("INPUT").value = null;
    newpassword.type = "hidden";
    newpassword.name = "newpassword";
    newpassword.id = "newpassword";
    newpassword.value = null;
    newpassword.value = jQuery("#txtNewPassword").val();
    document.forms[0].appendChild(newpassword);
    return true;
}
    </script>
    <form id="form1" runat="server">

        <table cellspacing="0" style="border-collapse: collapse; border: 0 solid #111111; width: 100%"
            id="AutoNumber1" cellpadding="0">
            <tr style="height: 65">
                <%--<td style="width: 95%; background-image:url(images/bg.jpg); background-repeat:repeat-x;" align="left" >
                            <img border="0" src="images/topheader.jpg" width="1004"></td>--%>
                <td style="vertical-align: bottom; width: 100%; height: 65px; text-align: left;">
                    <div style="background-image: url('images/left1.jpg'); background-repeat: repeat-x; min-width: 1000px; height: 65px;">
                        <div style="float: left;">
                            <img src="images/LeftLogo.jpg" alt="biznet logo left" />
                        </div>
                        <div style="float: left">
                            <img src="images/GoGreen.png" alt="goGreen left" />
                        </div>
                        <div style="float: right;">
                            <img src="images/topheader3.jpg" alt="biznet logo right" />
                        </div>
                    </div>
                    <div style="clear: both; position: relative; margin-top: -65px; float: right; width: 50%;">
                        <table style="width: 45%; border: 0 solid #111111; text-align: right; float: right;">
                            <tr style="height: 35px">
                                <td style="white-space: nowrap; vertical-align: top; height: 35px;">&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="white-space: nowrap;">
                                    <span id="lblWelcome" class="Label" style="color: #000; font-size: 15px;">Welcome :</span>
                                    <asp:Label ID="lblUserName" runat="server" CssClass="Label" Style="color: blanchedalmond; text-shadow: 0px 0px 20px #000; font-size: 14px;" />
                                    </span></b>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td id="menuTd" class="MenuTD">&nbsp;</td>
            </tr>
            <tr>
                <td style="height: 8px">
                    <asp:Button ID="btnDefault" runat="server" OnClientClick="return false;" Style="display: none;" CssClass="btn btnnew" />
                </td>
            </tr>
            <tr>
                <td style="width: 100%; text-align: left">
                    <div style="text-align: center;">
                        <table style="border: 1px solid #1560a1 inherit; width: 100%;" cellspacing="1" cellpadding="0">
                            <tr>
                                <td style="width: 98%; text-align: center">
                                    <asp:Panel ID="Pan_Hdr" runat="server" Width="100%" CssClass="InnerTable" BackColor="#F9FFFC">
                                        <asp:Panel Style="min-height: 440px; height: auto !important; height: 440px" ID="Pan_Child"
                                            runat="server" Width="100%" BackColor="Window">
                                            <div id="Header Label" style="text-align: center; text-align: center" class="Div">
                                                <table style="width: 98%">
                                                    <tr>
                                                        <td style="text-align: left">&nbsp;</td>
                                                        <td style="font-weight: bold; font-size: x-small; width: 30%; color: Navy; font-family: Verdana, Sans-Serif; font-variant: normal; text-align: right;">
                                                            <asp:Label ID="lblMandatory" runat="server" Text="Fields with * are Mandatory " />
                                                        </td>
                                                    </tr>
                                                </table>

                                                <div style="width: 100%; text-align: center;">
                                                    <asp:Label ID="lblHeading" runat="server" SkinID="lblHeading" />
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:ScriptManager ID="ScriptManager2" runat="server">
                                                            </asp:ScriptManager>
                                                            <asp:Label ID="lblerrormsg" runat="server" Width="545px" SkinID="lblError" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>

                                            <table cellpadding="5px" cellspacing="0" style="margin: auto;" width="50%">
                                                <tr>
                                                    <td class="Label" style="white-space: nowrap; text-align: right;" valign="top">New Password* : </td>
                                                    <td style="text-align: left;" valign="top">
                                                        <input runat="server" id="txtpwdlen" type="hidden" />
                                                        <asp:UpdatePanel ID="UPNewPwd" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtNewPassword" runat="server" CssClass="textBox" MaxLength="50" Type="Text" Width="60%" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="BtnSubmit" EventName="Click" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNewPassword" ErrorMessage="Enter new password"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right" class="Label" style="white-space: nowrap" valign="top">Confirm New Password* : </td>
                                                    <td align="left" valign="top">
                                                        <asp:UpdatePanel ID="UPConNewPwd" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtNewConfPass" runat="server" CssClass="textBox" MaxLength="50" Type="Text" Width="60%" autocomplete="off" AutoCompleteType="Disabled"></asp:TextBox>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="BtnSubmit" EventName="Click" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtNewPassword" ControlToValidate="txtNewConfPass" ErrorMessage="New And Confirm New Password Not Match" Width="272px"></asp:CompareValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="text-align: center;" valign="top">
                                                        <asp:Button ID="BtnSubmit" runat="server" CssClass="btn btnsave" Text="Submit" ToolTip="Submit" />
                                                        <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="btn btncancel" OnClientClick="pageToExit();" Text="Exit" ToolTip="Exit" OnClick="pageExit" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2" valign="top">
                                                        <asp:UpdatePanel ID="UPMessage" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Width="350px"></asp:Label>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="BtnSubmit" EventName="Click" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td></td>
            </tr>
            <tr>
                <td>
                    <br />
                    <br />
                    <br />
                    <br />
                </td>
            </tr>
            <tr>
                <td class="footer_Master">
                    <p style="text-align: center">
                        <script type="text/javascript">
                            var copyright;
                            var update;
                            copyright = new Date();
                            update = copyright.getFullYear();
                            document.write("<font face=\"verdana\" size=\"1\" color=\"black\">© Copyright " + update + ", Sarjen Systems Pvt LTD. </font>");
                        </script>
                    </p>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
