<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmChangePwd, App_Web_5xoe1jh1" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <style type="text/css">
        /*Password*/
        #ctl00_CPHLAMBDA_txtOldPassWord {
            -webkit-text-security: disc !important;
            -mox-text-security: disc !important;
        }
        #ctl00_CPHLAMBDA_txtNewPassword {
            -webkit-text-security: disc !important;
            -mox-text-security: disc !important;
        }
        #ctl00_CPHLAMBDA_txtNewConfPass {
            -webkit-text-security: disc !important;
            -mox-text-security: disc !important;
        }
    </style>
    <script type="text/javascript" src="Script/Validation.js"></script>

    <script type="text/javascript">
        function CheckVal() {
            var minlen = $("#ctl00_CPHLAMBDA_txtpwdlen").val();
            var Password = document.getElementById('<%=txtNewPassword.ClientID%>').value;
            if (!checkVal(document.getElementById('<%=txtOldPassWord.ClientID%>').value, '<%=txtOldPassWord.ClientID%>', '8')) {
                msgalert("Please Enter Old Password !");
                document.getElementById('<%=txtOldPassWord.ClientID%>').focus();
                return false;
            }
            else if (!checkVal(document.getElementById('<%=txtNewPassword.ClientID%>').value, '<%=txtNewPassword.ClientID%>', '8')) {
                msgalert("Please Enter New Password !");
                document.getElementById('<%=txtNewPassword.ClientID%>').focus();
                return false;
            }

            else if (!checkVal(document.getElementById('<%=txtNewConfPass.ClientID%>').value, '<%=txtNewConfPass.ClientID%>', '8')) {
                msgalert("Please Enter New Confirm Password !");
                document.getElementById('<%=txtNewConfPass.ClientID%>').focus();
                return false;
            }

            else if (Password.length < minlen) {
                msgalert("Password Length Should Be Minimum " + minlen + " Characters !");
                document.getElementById('<%=txtNewPassword.ClientID%>').focus();
                return false;
            }
            else if ((document.getElementById('<%=txtNewConfPass.ClientID%>').value != document.getElementById('<%=txtNewPassword.ClientID%>').value)) {
                msgalert("New Password And Confirm New Password Does Not Match !");
                document.getElementById('<%=txtNewConfPass.ClientID%>').focus();
                return false;
            }
            else {
                return ValidatePassword();
            }
        }

        function ValidatePassword() {
            var pwdRegEx = /^(?=.*\d)(?=.*[a-zA-Z])(?=.*[!@#$%])(?!.*\s)/;
            var txtPWD = document.getElementById('<%=txtNewPassword.ClientID%>');
            var minlen = $("#ctl00_CPHLAMBDA_txtpwdlen").val();
            var PwdMethod = 1;
            if (PwdMethod == 1) {
                var res = "true";
                var regex = /^(?=.*\d)(?=.*[a-zA-Z])/;
                if (txtPWD.value.length > minlen - 1) {
                    if (txtPWD.value.match(regex)) {
                        res = "false";
                    }
                }

                if (res == "false") {
                    return true;
                } else {
                    msgalert('Password Length Should Be Minimum ' + minlen + ' Characters.\nPassword Must Contain One Alphabet [A-Z] And One Digit [0-9] !!!');
                    txtPWD.focus();
                    return false;
                }
            }
            else {
                var res = "true";
                if (txtPWD.value.length > minlen - 1) {
                    if (txtPWD.value.match(pwdRegEx)) {
                        res = "false";
                    }
                }

                if (res == "false") {
                    return true;
                } else {
                    msgalert('Password Length Should Be Minimum ' + minlen + ' Characters.\nPassword Must Contain Atleast One Alphabet [A-Z] And One Digit [0-9] And One Special Character [!@#$%^&*] !!!');
                    txtPWD.focus();
                    return false;
                }
            }
            return true;
        }

        function AlertAndRedirect(URL, msg) {
            alertdooperation(msg, 1, URL);
        }

        function AlertAndClose(msg) {
            msgalert(msg);
            window.close();
        }

        function ShowAlert(msg,status) {
            alertdooperation(msg, status, "frmMainPage.aspx");
        }

        function pageToRedirect() {
            if (msgconfirmalert('Are You Sure You Want To Exit ?',this)) {
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
    </script>

    <div style ="width :100%;">
        <table width="50%" cellspacing="0" cellpadding="5px" style ="margin :auto;">
            <tr>
                <td colspan="2" valign="top" align="left">
                    <asp:UpdatePanel ID="UPMessage" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" Width="328px" ForeColor="Red"></asp:Label>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="BtnSubmit" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td valign="top"  class="Label" style="white-space: nowrap; width :35%; text-align :right ;">
                    Old Password* :
                </td>
                <td valign="top" style ="text-align :left ;">
                    <asp:UpdatePanel ID="UPOldPwd" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtOldPassWord" runat="server" CssClass="textBox" autocomplete="off" MaxLength="50" Width="60%"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="BtnSubmit" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Enter old password"
                        ControlToValidate="txtOldPassWord"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td valign="top"  class="Label" style="white-space: nowrap; text-align :right ;">
                    New Password* :
                </td>
                <td valign="top" style ="text-align :left ;">
                    <input runat="server" id="txtpwdlen" type="hidden" />
                    <input runat="server" id="txtpwdstyle" type="hidden" />
                    <asp:UpdatePanel ID="UPNewPwd" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtNewPassword" runat="server" CssClass="textBox" autocomplete="off" MaxLength="50" Width="60%"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="BtnSubmit" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Enter new password"
                        ControlToValidate="txtNewPassword"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td valign="top" align="right" class="Label" style="white-space: nowrap">
                    Confirm New Password* :
                </td>
                <td valign="top" align="left">
                    <asp:UpdatePanel ID="UPConNewPwd" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtNewConfPass" runat="server" CssClass="textBox" autocomplete="off" MaxLength="50" Width="60%"></asp:TextBox>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="BtnSubmit" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="New And Confirm New Password Not Match"
                        ControlToCompare="txtNewPassword" Width="272px" ControlToValidate="txtNewConfPass"></asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td valign="top" style="text-align: center;" colspan ="2">
                    <asp:Button ID="BtnSubmit" runat="server" Text="Submit" ToolTip ="Submit" CssClass="btn btnsave"  />
                    <asp:Button ID="btnCancel" runat="server" Text="Exit" ToolTip ="Exit" CssClass="btn btncancel"
                        CausesValidation="False" OnClientClick="return msgconfirmalert('Are You Sure You Want To Exit ?',this);" />
                <%--    <asp:Button ID="BtnExit" runat="server" ToolTip="Exit" CausesValidation="False" CssClass="btn btnclose"
                        OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this); " Text="Exit" />--%>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
