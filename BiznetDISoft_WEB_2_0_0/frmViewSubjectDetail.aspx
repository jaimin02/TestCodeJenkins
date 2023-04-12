<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmViewSubjectDetail.aspx.vb" Inherits="frmViewSubjectDetail"  %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="ConViewSubjectDetail" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <asp:UpdatePanel id="up_Div" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <contenttemplate>
<DIV style="LEFT: 50px; TOP: 306px" id="divComments" class="DIVSTYLE2" runat="server" visible="false">
<TABLE width="100%"><TBODY><TR><TD vAlign=top align=left></TD></TR><TR><TD>Comments: </TD><TD></TD></TR><TR><TD></TD><TD align=left width="100%"><asp:TextBox id="txtComments" runat="server" TextMode="MultiLine" width="90%" __designer:wfdid="w5" MaxLength="1023"></asp:TextBox> </TD></TR><TR></TR><TR><TD></TD><TD align=left><asp:Button id="btnSave" onclick="btnSave_Click" runat="server" Text=" Save " CssClass="btn btnsave"></asp:Button> <asp:Button id="btnClose" onclick="btnClose_Click" runat="server" Text="Close" __designer:wfdid="w1" CssClass="btn btnclose"></asp:Button></TD></TR><TR><TD vAlign=top align=center colSpan=2><asp:UpdatePanel id="up_inDiv" runat="server" UpdateMode="Conditional" RenderMode="Inline"><ContentTemplate>
<asp:GridView id="GVSubjectComments" runat="server" SkinID="grdView" __designer:wfdid="w1" AutoGenerateColumns="False" OnRowDataBound="GVSubjectComments_RowDataBound"><Columns>
<asp:TemplateField HeaderText="Sr No."><EditItemTemplate>
<asp:TextBox runat="server" id="TextBox1"></asp:TextBox>
</EditItemTemplate>
<ItemTemplate>
<asp:Label id="lblSrNo" runat="server" __designer:wfdid="w2"></asp:Label>
</ItemTemplate>

<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
</asp:TemplateField>
<asp:BoundField DataField="vActivityName" HeaderText="Activity">
<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="vDocTypeName" HeaderText="Doc Type">
<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="vComment" HeaderText="Comment">
<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="dModifyOn" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Given On" HtmlEncode="False">
<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

<ItemStyle HorizontalAlign="Right"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="vUserName" HeaderText="Given By">
<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:BoundField>
</Columns>
</asp:GridView> 
</ContentTemplate>
<Triggers>
<asp:AsyncPostBackTrigger ControlID="gvwViewSubjectDetail" EventName="RowCommand"></asp:AsyncPostBackTrigger>
</Triggers>
</asp:UpdatePanel> </TD></TR></TBODY></TABLE></DIV>
</contenttemplate>
        <triggers>
<asp:AsyncPostBackTrigger ControlID="gvwViewSubjectDetail" EventName="RowCommand"></asp:AsyncPostBackTrigger>
</triggers>
    </asp:UpdatePanel>
    <div>
        <table cellpadding="0" cellspacing="0" width="1004" align="center">
            <tr>
                <td>
                    <asp:UpdatePanel id="upPnlSubDet" runat="server">
                        <contenttemplate>
<TABLE><TBODY><TR><TD align=left></TD></TR></TBODY></TABLE><TABLE cellPadding=3 width="100%"><TBODY><TR><TD><DIV style="PADDING-LEFT: 100px"><TABLE cellPadding=3 align=left><TBODY><TR><TD align=left>Project No: </TD><TD align=left><asp:Label id="lblProjectNo" runat="server" Text="Label" __designer:wfdid="w3"></asp:Label> </TD></TR><TR><TD align=left>Activity: </TD><TD align=left><asp:Label id="lblActivity" runat="server" Text="" __designer:wfdid="w4"></asp:Label> </TD></TR><TR><TD align=left>Filter On DocType: </TD><TD align=left><asp:DropDownList id="ddlDocType" runat="server" CssClass="dropDownList" __designer:wfdid="w5" AutoPostBack="True" OnSelectedIndexChanged="ddlDocType_SelectedIndexChanged"></asp:DropDownList> </TD></TR></TBODY></TABLE><asp:Button id="BtnBack" runat="server" Text="Back" CssClass="btn btnnew" OnClick="BtnBack_Click"></asp:Button></DIV></TD></TR><TR><TD align=left><DIV style="PADDING-LEFT: 100px"><asp:GridView id="gvwViewSubjectDetail" runat="server" SkinID="grdView" __designer:wfdid="w6" AutoGenerateColumns="False" OnRowCommand="gvwViewSubjectDetail_RowCommand" OnRowCreated="gvwViewSubjectDetail_RowCreated" CellPadding="3"><Columns>
<asp:TemplateField HeaderText="Sr No."><ItemTemplate>
<asp:Label id="lblSrNo" runat="server" __designer:wfdid="w1"></asp:Label> 
</ItemTemplate>

<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
</asp:TemplateField>
<asp:BoundField DataField="vWorkspaceSubjectDocDetailId" HeaderText="Workspace Subject Doc. Detail Id">
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>

<ItemStyle HorizontalAlign="Right"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="vWorkspaceSubjectId" HeaderText="Subject Id">
<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

<ItemStyle HorizontalAlign="Right"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="vDocTypeName" HeaderText="Doc. Type">
<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:BoundField>
<asp:TemplateField HeaderText="Doc"><ItemTemplate>
                                            <asp:HyperLink ID="hlnkDoc" runat="server" NavigateUrl='<%# Eval("vDocLink") %>'
                                                Target="_blank" Text='<%# Eval("vDocLink") %>'></asp:HyperLink>
                                        
</ItemTemplate>

<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
</asp:TemplateField>
<asp:BoundField DataField="vUsername" HeaderText="Created By">
<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="dUploadedOn" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="Created On" HtmlEncode="False">
<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

<ItemStyle HorizontalAlign="Right"></ItemStyle>
</asp:BoundField>
<asp:TemplateField HeaderText="Comments"><ItemTemplate>
<asp:LinkButton id="lnkBtnViewComments" runat="server" __designer:wfdid="w2" >View</asp:LinkButton> 
</ItemTemplate>

<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
</asp:TemplateField>
</Columns>
</asp:GridView> </DIV></TD></TR></TBODY></TABLE>
</contenttemplate>
                        <triggers>
<asp:AsyncPostBackTrigger ControlID="gvwViewSubjectDetail" EventName="RowCommand"></asp:AsyncPostBackTrigger>
</triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
