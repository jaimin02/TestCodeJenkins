<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="frmuserRights.aspx.vb" Inherits="frmuserRights"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" Runat="Server">
<script type="text/javascript" language="javascript">
function Validation()
{
if (document.getElementById('<%=DDLDepartment.ClientID%>').selectedIndex==0)
    {
    msgalert('Please Select Department !');
    return false;
    }
else if (document.getElementById('<%=DDLLocation.ClientID%>').selectedIndex==0)
    {
    msgalert('Please Select Location !');
    return false;
    }
else if (document.getElementById('<%=DDLUser.ClientID%>').selectedIndex==0)
    {
    msgalert('Please select User !');
    return false;
    }
else
    {
    return true;
    }    
}
</script>
    <table style="width: 577px">
        <tr>
            <td align="right" class="Label">
                Department :</td>
            <td align="left">
                <asp:DropDownList ID="DDLDepartment" runat="server" CssClass="dropDownList" Width="294px" AppendDataBoundItems="True" AutoPostBack="true">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td align="right" class="Label">
                Location :</td>
            <td align="left">
                <asp:DropDownList ID="DDLLocation" runat="server" CssClass="dropDownList" Width="294px" AppendDataBoundItems="True" AutoPostBack="true">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td align="right" class="Label">
                User :</td>
            <td align="left">
                <asp:UpdatePanel id="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                <contenttemplate>
                <asp:DropDownList ID="DDLUser" runat="server" CssClass="dropDownList" Width="294px" AutoPostBack="true">
                </asp:DropDownList>
                </contenttemplate>
                    <triggers>
<asp:AsyncPostBackTrigger ControlID="DDLLocation" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="DDLDepartment" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
</triggers>
                </asp:UpdatePanel></td>
        </tr>
        <tr>
            <td align="left">
            </td>
            <td align="left">
                <asp:Button ID="BtnSave" runat="server" CssClass="btn btnsave" Text="Save" />&nbsp;<asp:Button
                    ID="BtnExit" runat="server" CssClass="btn btnback" Text="" /></td>
        </tr>
    </table>
</asp:Content>

