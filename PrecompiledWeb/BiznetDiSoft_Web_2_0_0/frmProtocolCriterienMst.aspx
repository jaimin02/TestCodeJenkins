<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmProtocolCriterienMst, App_Web_ybumpksz" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" Runat="Server">

    <script language="javascript" type="text/javascript">
function Validation()
{
if (document.getElementById('<%=txtCriterianDesc.ClientID%>').value =='')
    {
    msgalert('Please Enter Operation Name !');
    return false;
    }
else
    {
    return true;
    }    
}
    </script>

    <asp:UpdatePanel id="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <contenttemplate>

    <table>
        <tr>
            <td align="left">
                Criterien Description:
            </td>
            <td align="left">
                <asp:TextBox ID="txtCriterianDesc" runat="server" CssClass="textBox" Height="42px"
                    TextMode="MultiLine" Width="262px"></asp:TextBox></td>
        </tr>
        <tr>
            <td align="left">
                CriterienType &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;:</td>
            <td align="left">
                <asp:DropDownList ID="DDLCriterienType" runat="server" CssClass="dropDownList">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td align="left">
                Active &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; :</td>
            <td align="left">
                <asp:CheckBox ID="chkActive" runat="server" /></td>
        </tr>
        <tr>
            <td align="left">
            </td>
            <td align="left">
                <asp:Button ID="BtnSave" runat="server" CssClass="btn btnsave" Text="Save" />&nbsp;<asp:Button
                    ID="BtnExit" runat="server" CssClass="btn btnclose" Text="Exit" /></td>
        </tr>
    </table></contenttemplate>
    </asp:UpdatePanel>
    
    <asp:UpdatePanel id="Up_View" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <contenttemplate>
        
        <TABLE align=center>
        <TR><TD align=left colSpan=2><asp:GridView id="GV_Criterien" runat="server" SkinID="grdView" AutoGenerateColumns="False"><Columns>
        <asp:BoundField DataField="vProtocolCriterienID" HeaderText="Protocol Criterien ID"></asp:BoundField>
        <asp:BoundField DataField="vProtocolCriterienDescription" HeaderText="Protocol Criterien Description"></asp:BoundField>
        <asp:BoundField DataField="cProtocolCriterienType" HeaderText="Protocol Criterien Type"></asp:BoundField>
        <asp:BoundField DataField="cActiveFlag" HeaderText="Active Flag"></asp:BoundField>
        </Columns>
        </asp:GridView>
        </TD></TR>
        </TABLE>
        
        </contenttemplate>
        <triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click">
            </asp:AsyncPostBackTrigger>
        </triggers>
    </asp:UpdatePanel>
</asp:Content>

