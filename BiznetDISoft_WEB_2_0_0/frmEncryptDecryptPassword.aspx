<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmEncryptDecryptPassword.aspx.vb" Inherits="frmEncryptDecryptPassword1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <table width="100%" cellpadding="3px">
        <tr>
            <td style="width: 35%; text-align: right;">
                <asp:Label ID="Label5" runat="server" CssClass="Label" Text="Login Name :"></asp:Label>
            </td>
            <td align="left" style="white-space: nowrap">
                <asp:DropDownList ID="ddlLoginName" runat="server" AutoPostBack="True" CssClass="dropDownList"
                    Width="35%">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="text-align: right;">
                <asp:Label ID="Label1" runat="server" Text="Password :" CssClass="Label"></asp:Label>
            </td>
            <td style="text-align: left;">
                <asp:TextBox ID="TextBox1" runat="server" Width="35%" CssClass="textBox"></asp:TextBox>
                <asp:Button ID="Button1" runat="server" Text="Encrypt" ToolTip="Encrypt" CssClass="btn btnnew" />
            </td>
        </tr>
        <%--<tr>
            <td colspan="2" style="text-align: center;">
                <asp:Button ID="Button1" runat="server" Text="Encrypt" ToolTip="Encrypt" CssClass="button" />
            </td>
        </tr>--%>
        <tr>
            <td style="text-align: right;">
                <asp:Label ID="Label2" runat="server" Text="EncryptedPassword :" CssClass="Label"></asp:Label>
            </td>
            <td style="text-align: left;">
                <asp:TextBox ID="TextBox2" runat="server" Width="35%" CssClass="textBox "></asp:TextBox>
                <asp:Button ID="Button2" runat="server" Text="Decrypt" ToolTip="Decrypt" CssClass="btn btnnew" />
            </td>
        </tr>
        <%-- <tr>
           
            <td style="text-align: center;" colspan="2">
                <asp:Button ID="Button2" runat="server" Text="Decrypt" ToolTip="Decrypt" CssClass="button" />
            </td>
        </tr>--%>
        <tr>
            <td style="text-align: right;">
                <asp:Label ID="Label3" runat="server" Text="DecryptedPassword :" CssClass="Label"></asp:Label>
            </td>
            <td style="text-align: left;">
                <asp:TextBox ID="TextBox3" runat="server" Width="35%" CssClass="textBox "></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align: center;">
                <asp:Button ID="Button3" runat="server" Text="Encrypt All Passwords" CssClass="btn btnnew"
                    OnClientClick="return msgconfirmalert('Are You Sure You want To Encrypt All Password ?',this)"
                    Width="16%" />
                <asp:Button ID="Button4" runat="server" Text="Decrypt All Passwords" ToolTip="Decrypt All Passwords"
                    CssClass="btn btnew" OnClientClick="return msgconfirmalert('Are You Sure You Want To Encrypt All Password ?',this)"
                    Width="16%" />
                <asp:Button ID="Button5" runat="server" CssClass="btn btnexit" Text="Back To Home" ToolTip="Back To Home"
                    Width="13%" />
            </td>
        </tr>
    </table>
</asp:Content>
