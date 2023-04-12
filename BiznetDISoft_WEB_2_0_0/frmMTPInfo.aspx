<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="frmMTPInfo.aspx.vb" 
Inherits="frmMTPInfo"  %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" Runat="Server">
<script type="text/javascript" language="javascript" src="Script/Validation.js" />
<script type="text/javascript" language="javascript" src="Script/General.js" />
    <script language="javascript" type="text/javascript">
    function ValidAdd()
        {
        if (document.getElementById('<%=ddlSTP.ClientID%>').selectedIndex==0)
            {
                msgalert("Please Select Site");
                return false;
            }
            else if (document.getElementById('<%=ddlActivityGroup.ClientID%>').selectedIndex==0))
            {
               msgalert("Please Select Activity Group !");
               return false;
            }
            else if (document.getElementById('<%=ddlActivity.ClientID%>').selectedIndex==0))
            {
               msgalert("Please Select Activity !");
               return false;
            }    
        
          return true;
        
        }
       
    </script>

    <div align="left">
        <asp:UpdatePanel ID="UPVMTPDtl" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>
<DIV style="LEFT: 424px; TOP: 488px" id="divMTPDtl" class="DIVSTYLE2" runat="server" visible="false"><asp:Panel id="pnlMTPDtl" runat="server" Visible="false"><TABLE style="WIDTH: 100%"><TBODY><TR class="TR"><TD style="WIDTH: 30%" class="Label" align=left>MTP Date* </TD><TD align=left><INPUT style="WIDTH: 108px" id="txtMtpDt" class="TextBox" type=text name="txtMtpDt" runat="server" /> </TD><TD align=right>&nbsp;</TD><TD style="WIDTH: 190px">&nbsp;</TD></TR><TR class="TR"><TD style="WIDTH: 30%" class="Label" align=left>Select Site : </TD><TD align=left colSpan=3><asp:DropDownList style="WIDTH: 445px" id="ddlSTP" runat="server" CssClass="dropDownList" __designer:wfdid="w3"></asp:DropDownList></TD></TR><TR class="TR"><TD style="WIDTH: 30%" class="Label" align=left>Activity Group :</TD><TD align=left colSpan=3><asp:DropDownList style="WIDTH: 445px" id="ddlActivityGroup" runat="server" CssClass="dropDownList" __designer:wfdid="w102" OnSelectedIndexChanged="ddlActivityGroup_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList></TD></TR><TR class="TR"><TD style="WIDTH: 30%" class="Label" align=left>Activity Name :</TD><TD align=left colSpan=3><asp:DropDownList style="WIDTH: 445px" id="ddlActivity" runat="server" CssClass="dropDownList"></asp:DropDownList> </TD></TR><TR class="TR"><TD style="WIDTH: 30%" class="Label" align=left>Objective :</TD><TD align=left colSpan=3><INPUT style="WIDTH: 440px" id="txtRemark" class="TextBox" type=text name="txtObjective" runat="server" /> </TD></TR><TR class="TR"><TD style="WIDTH: 30%" align=left>&nbsp;</TD><TD align=left colSpan=3><INPUT id="btnAdd" class="btn btnnew" type=button value="Add" runat="server" onclientclick="return ValidAdd();" onserverclick="btnAdd_ServerClick" /> <INPUT id="btncancel" class="btn btncancel" type=button value="Cancel" runat="server" /></TD></TR><TR class="TR"><TD align=left colSpan=4>
<asp:GridView id="GVSiteDtl" runat="server" Font-Size="Small" SkinID="GVPage" __designer:wfdid="w103" OnRowCommand="GVSiteDtl_RowCommand" OnRowDataBound="GVSiteDtl_RowDataBound" OnRowCreated="GVSiteDtl_RowCreated" BorderWidth="1px" AutoGenerateColumns="False" BorderColor="Peru"><Columns>
<asp:BoundField DataField="nMTPNo" HeaderText="nMTPNo"></asp:BoundField>
<asp:BoundField DataField="nSTPNo" HeaderText="nSTPNo"></asp:BoundField>
<asp:BoundField DataField="vSiteName" HeaderText="Site Name"></asp:BoundField>
<asp:BoundField DataField="vActivityName" HeaderText="Activity"></asp:BoundField>
<asp:BoundField DataField="vRemark" HeaderText="Objective"></asp:BoundField>
<asp:TemplateField HeaderText="Delete"><ItemTemplate>
<asp:LinkButton id="lnkSiteDtl_Delete" runat="server" __designer:wfdid="w1">Delete</asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
</Columns>
</asp:GridView> <INPUT id="BtnSave" class="btn btnsave" type=button value="Save" runat="server" OnServerClick="BtnSave_ServerClick" />&nbsp;<INPUT id="btnExitMTP" class="btn btnexit" type=button value="Exit" runat="server" OnServerClick="btnExitMTP_ServerClick1" /></TD></TR></TBODY></TABLE></asp:Panel></DIV>
</ContentTemplate>
            <Triggers>
<asp:AsyncPostBackTrigger ControlID="GVCityDesc" EventName="RowCommand"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="ServerClick"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="btncancel" EventName="ServerClick"></asp:AsyncPostBackTrigger>
</Triggers>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UPViewMTP" runat="server" RenderMode="Inline" UpdateMode="Conditional">
            <ContentTemplate>
<DIV style="LEFT: 99px; WIDTH: 700px; TOP: 340px" id="divMTP" class="DIVSTYLE2" runat="server" visible="false"><asp:Panel id="pnlViewMTP" runat="server" Visible="false" __designer:wfdid="w1"><asp:GridView id="GVViewMTP" runat="server" Font-Size="Small" SkinID="GVPage" __designer:wfdid="w2" BorderColor="Peru" AutoGenerateColumns="False" BorderWidth="1px"><Columns>
<asp:BoundField DataField="dMTPDate" HeaderText="Date"></asp:BoundField>
<asp:BoundField DataField="vDay" HeaderText="Day"></asp:BoundField>
<asp:BoundField DataField="vWorkspaceDesc" HeaderText="Project"></asp:BoundField>
<asp:BoundField DataField="vActivityName" HeaderText="Activity"></asp:BoundField>
<asp:BoundField DataField="vInvestigatorName" HeaderText="Investigator Name"></asp:BoundField>
<asp:BoundField DataField="vSiteName" HeaderText="Site Name"></asp:BoundField>
<asp:BoundField DataField="vAddress" HeaderText="Address"></asp:BoundField>
<asp:BoundField DataField="vRemark" HeaderText="Objective"></asp:BoundField>
</Columns>
</asp:GridView> <asp:Button id="btnExit" onclick="btnExit_Click" runat="server" Font-Bold="True" Text="Exit" CssClass="btn btnexit"  __designer:wfdid="w3"></asp:Button> </asp:Panel> </DIV>
</ContentTemplate>
            <triggers>
<asp:AsyncPostBackTrigger ControlID="GVCityDesc" EventName="RowCommand"></asp:AsyncPostBackTrigger>
</triggers>
            
        </asp:UpdatePanel>
        <table align="left">
            <tr>
                <td align="left" valign="top">
                    <asp:UpdatePanel ID="UPMonth" runat="server">
                        <ContentTemplate>
<TABLE><TBODY><TR><TD class="Label" vAlign=top align=left>Select Month* :</TD><TD vAlign=top align=left><asp:DropDownList id="ddlMonth" runat="server" CssClass="dropDownList" Width="120px" __designer:wfdid="w6"></asp:DropDownList> </TD><TD vAlign=top align=left><asp:Button id="btnGo" runat="server" Text="" CssClass="btn btngo"  __designer:wfdid="w7"></asp:Button></TD><TD vAlign=top align=left><asp:Button id="btnBulkCopy" runat="server" Text="Bulk Copy" CssClass="btn btnnew"  Visible="False" __designer:wfdid="w8"></asp:Button>&nbsp;<asp:Button id="btnLeaveRequest" runat="server" Text="Leave Request" CssClass="btn btnnew"  Visible="False" __designer:wfdid="w9"></asp:Button></TD></TR></TBODY></TABLE>
</ContentTemplate>
                        <Triggers>
<asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="BtnApprove" EventName="Click"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="btnReject" EventName="Click"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="ServerClick"></asp:AsyncPostBackTrigger>
</Triggers>
                    </asp:UpdatePanel>
<asp:Button id="BtnApprove" runat="server" Text="Approve" CssClass="btn btnnew"  Visible="False" Font-Bold="True"></asp:Button><asp:Button id="btnReject" runat="server" Text="Reject" CssClass="btn btncancel"  Visible="False" Font-Bold="True"></asp:Button> 
                </td>
                <td align="left" valign="top">
                </td>
            </tr>
            <tr>
                <td colspan="5" align="left" valign="top">
                    <table>
                        <tr>
                            <td align="left" valign="top">
                                <asp:UpdatePanel ID="UPMTP" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                    <ContentTemplate>
<asp:GridView id="GVCityDesc" runat="server" Font-Size="Small" SkinID="grdView" __designer:wfdid="w1" OnRowCreated="GVCityDesc_RowCreated" BorderWidth="1px" AutoGenerateColumns="False" BorderColor="Green"><Columns>
<asp:BoundField DataField="nMTPNo" HeaderText="MTPNo"></asp:BoundField>
<asp:TemplateField HeaderText="Date"><ItemTemplate>
<asp:Label id="lblGvDate" style="WHITE-SPACE:nowrap;" runat="server" Text='<% #databinder.eval(container.dataitem,"dDate") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="vDay" HeaderText="Day"></asp:BoundField>
<asp:BoundField DataField="nSTPNo" HeaderText="nSTPNo"></asp:BoundField>
<asp:BoundField DataField="vSiteName" HeaderText="Site Name"></asp:BoundField>
<asp:BoundField DataField="vActivityId" HeaderText="vActivityId"></asp:BoundField>
<asp:BoundField DataField="vActivityName" HeaderText="Activity Name"></asp:BoundField>
<asp:BoundField DataField="vHolidayDescription" HeaderText="Holiday"></asp:BoundField>
<asp:TemplateField HeaderText="Add"><ItemTemplate>
<asp:LinkButton id="lnkAdd" runat="server" __designer:wfdid="w101">Add</asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Delete"><ItemTemplate>
<asp:LinkButton id="lnkDelete" runat="server" >Delete</asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="View"><ItemTemplate>
<asp:LinkButton id="lnkView" runat="server" __designer:wfdid="w100">View</asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="nMTPHdrNo" HeaderText="nMTPHdrNo"></asp:BoundField>
</Columns>
</asp:GridView> 
</ContentTemplate>
                                    <Triggers>
<asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="BtnApprove" EventName="Click"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="btnReject" EventName="Click"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="ServerClick"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="btnExit" EventName="Click"></asp:AsyncPostBackTrigger>
<asp:AsyncPostBackTrigger ControlID="btnExitMTP" EventName="ServerClick"></asp:AsyncPostBackTrigger>
</Triggers>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
<asp:Button id="btnSubmit" runat="server" Text="Save" CssClass="btn btnsave"></asp:Button>&nbsp;<asp:Button id="BtnExitPage" runat="server" Text="Exit" CssClass="btn btnexit"  __designer:wfdid="w2" OnClick="BtnExitPage_Click"></asp:Button>
</ContentTemplate>
                                    <Triggers>
</Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

