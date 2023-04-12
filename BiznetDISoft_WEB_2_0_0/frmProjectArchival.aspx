<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmProjectArchival.aspx.vb" Inherits="frmProjectArchival"  %>

<asp:Content ID="conMyProject" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <table cellpadding="4" cellspacing="2" width="100%">
        <tr>
            <td style="width: 100%; text-align: center">
                <asp:UpdatePanel id="upnlProjectsGridView" runat="server">
                    <contenttemplate>
<TABLE width="100%"><TBODY><TR><TD style="WIDTH: 45%; WHITE-SPACE: nowrap" class="Label" align=right>Project NO</TD><TD style="WIDTH: 15%" align="justify"><asp:TextBox id="txtSearchProject" tabIndex=0 runat="server" CssClass="textbox"></asp:TextBox> </TD><TD style="WIDTH: 40%" align=left><asp:Button id="btnSearch" tabIndex=3 runat="server" Text="Search" CssClass="btn btnnew"></asp:Button></TD></TR><TR><TD colSpan=3><asp:GridView id="gvwProjects" runat="server" CssClass="gvwProjects" SkinID="grdView" OnPageIndexChanging="gvwProjects_PageIndexChanging" PageSize="25" AllowPaging="True" AutoGenerateColumns="False" AllowSorting="True" OnRowCreated="gvwProjects_RowCreated" OnRowDataBound="gvwProjects_RowDataBound" CellPadding="3" ShowFooter="True"><Columns>
<asp:BoundField DataField="vWorkspaceID" HeaderText="WorkspaceID" SortExpression="vWorkspaceID">
<HeaderStyle Font-Underline="False"></HeaderStyle>
</asp:BoundField>
<asp:BoundField DataField="vWorkspaceDesc" HeaderText="Project" SortExpression="vWorkspaceDesc">
<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="vRequestId" HeaderText="Requeset ID">
<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

<ItemStyle HorizontalAlign="Right" Wrap="False"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="vProjectNo" HeaderText="Project No.">
<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

<ItemStyle HorizontalAlign="Right"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="vClientName" HeaderText="Sponsor" SortExpression="vClientName">
<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="vDrugName" HeaderText="Drug" SortExpression="vDrugName">
<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="iNoOfSubjects" HeaderText="# Subjects" SortExpression="iNoOfSubjects">
<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

<ItemStyle HorizontalAlign="Right"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="vProjectManager" HeaderText="Project Manager" SortExpression="vBrandName">
<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="vProjectCoordinator" HeaderText="Co-Ordinator" SortExpression="vProjectCoordinator">
<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="vProjectTypeName" HeaderText="Project Type" SortExpression="vProjectTypeName">
<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="vRegionName" HeaderText="Submission" SortExpression="vRegionName">
<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="cProjectStatusDesc" HeaderText="Status" SortExpression="cProjectStatusDesc">
<FooterStyle HorizontalAlign="Center" VerticalAlign="Middle"></FooterStyle>

<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
</asp:BoundField>
<asp:TemplateField HeaderText="Archive"><ItemTemplate>
<%--onClientClick="ShowHideDiv('Y');"--%><asp:LinkButton id="lnkBtn" onclick="lnkBtn_Click" runat="server" Text="Archive" __designer:wfdid="w1"></asp:LinkButton> 
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Details"><ItemTemplate>
<asp:LinkButton id="lnkBtnProDet" onclick="lnkBtnProDet_Click" runat="server" ForeColor="Navy">Project Details</asp:LinkButton> 
</ItemTemplate>

<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
</asp:TemplateField>
<asp:TemplateField HeaderText="Default Document UserRights"><ItemTemplate>
<asp:LinkButton id="lnkBtnRights" runat="server" >User Rights</asp:LinkButton>
</ItemTemplate>

<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
</asp:TemplateField>
</Columns>
</asp:GridView> </TD></TR></TBODY></TABLE>
</contenttemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
