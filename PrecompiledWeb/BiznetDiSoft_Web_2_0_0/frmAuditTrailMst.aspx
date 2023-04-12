<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmAuditTrailMst, App_Web_vq2225em" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" Runat="Server">
<TABLE style="WIDTH: 778px" align=center><TBODY><TR><TD style="WIDTH: 122px" align=left class="Label">Project No : &nbsp;</TD><TD align=left>
    <asp:Label ID="lblProjectNo" runat="server" Text="Label"></asp:Label></TD></TR><TR><TD style="WIDTH: 122px" align=left class="Label">
    Project Name: :</TD><TD align=left>
    <asp:Label ID="lblProjectName" runat="server" Text="Label"></asp:Label></TD></TR>
    <tr>
        <td align="left" style="width: 122px" class="Label">
            Activity Name :</td>
        <td align="left">
            <asp:Label ID="lblActivityName" runat="server" Text="Label"></asp:Label></td>
    </tr>
</TBODY>
</TABLE>
    <table style="width: 148px">
        <tr>
            <td>
                <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="btn btnnew"
                    OnClick="btnBack_Click" Text="Back" /></td>
        </tr>
    </table>
    <br />
    <br />
 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
<TABLE style="WIDTH: 231px"><TBODY><TR><TD><asp:GridView id="gvdeptstage" runat="server" SkinID="grdViewsml" OnRowDataBound="gvdeptstage_RowDataBound" AutoGenerateColumns="False" AllowPaging="True" ShowFooter="True" OnPageIndexChanging="gvdeptstage_PageIndexChanging"><Columns>
<asp:TemplateField HeaderText="Document Name"><ItemTemplate>
<asp:HyperLink id="hlnkDocument" runat="server" Text='<%# Eval("DocPath") %>' NavigateUrl='<%# Eval("DocPath") %>' Target="_blank"></asp:HyperLink> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="iTranNo" HeaderText="Document Vers."></asp:BoundField>
<asp:BoundField DataField="vstageDesc" HeaderText="Document Status"></asp:BoundField>
<asp:BoundField DataField="vUserName" HeaderText="ModifyBy"></asp:BoundField>
<asp:BoundField HtmlEncode="False" DataFormatString="{0:dd-MMM-yyyy}" DataField="dModifyOn_IST" HeaderText="ModifyOn"></asp:BoundField>
</Columns>
</asp:GridView> </TD></TR></TBODY></TABLE>
</ContentTemplate>
</asp:UpdatePanel>




</asp:Content>

