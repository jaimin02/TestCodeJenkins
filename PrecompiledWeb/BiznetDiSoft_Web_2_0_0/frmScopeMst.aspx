<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmScopeMst, App_Web_2mzu20n4" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <table style="width: 100%;" cellpadding="5px">
        <tr>
            <td class="Label" style="width: 40%; text-align: right;">
                Scope Name*:
            </td>
            <td style="text-align: left;">
                <asp:TextBox ID="txtScopeName" runat="server" CssClass="textBox" Width="40%" MaxLength="50" />
                <asp:DropDownList ID="ddlScope" runat="server" Visible="false" CssClass="dropDownList"
                    AutoPostBack="true" Width="40%" />
            </td>
        </tr>
        <tr>
            <td style="text-align: right;" class="Label">
                Scope Values :
            </td>
            <td style="text-align: left;">
                <asp:Panel ID="pnlScopeValues" ScrollBars="vertical" runat="server" Height="120px"
                    BorderColor="green" BorderWidth="1px" Width="40%">
                    <asp:CheckBoxList ID="ChkScopeValues" runat="server" CssClass="checkboxlist" />
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td style="text-align: center;" colspan="2">
                <asp:Button ID="BtnSave" runat="server" CssClass="btn btnsave" Text="Save" ToolTip="Save" />
                <asp:Button ID="BtnEdit" runat="server" CssClass="btn btnedit" Text="Edit" ToolTip="Edit" />
                <asp:Button ID="BtnExit" runat="server" CssClass="btn btnexit" Text="Exit" ToolTip="Exit"
                    OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
            </td>
        </tr>
    </table>
</asp:Content>
