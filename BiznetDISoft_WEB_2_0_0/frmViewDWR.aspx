<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmViewDWR.aspx.vb" Inherits="frmViewDWR"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <script type="text/javascript" src="Script/popcalendar.js"></script>

    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="WIDTH: 100%"><TBODY><TR><TD style="WIDTH: 100%" class="Label" align=center><asp:Label id="lblUser" runat="server" Text="User:" __designer:wfdid="w190"></asp:Label>&nbsp;&nbsp;<asp:DropDownList id="ddlUser" runat="server" CssClass="dropDownList" Width="238px" __designer:wfdid="w4"></asp:DropDownList></TD></TR><TR><TD style="WIDTH: 100%" class="Label" align=center>From Date: <asp:TextBox id="txtFromDate" runat="server" CssClass="textBox" Width="171px" __designer:wfdid="w15" AutoPostBack="True" Enabled="False" EnableViewState="False"></asp:TextBox><IMG id="imgFromDate" onclick="popUpCalendar(this,ctl00_CPHLAMBDA_txtFromDate,'dd-mmm-yyyy');" alt="Select  Date" src="images/Calendar_scheduleHS.png" /> To Date: <asp:TextBox id="txtToDate" runat="server" CssClass="textBox" Width="171px" __designer:wfdid="w16" AutoPostBack="True" Enabled="False" EnableViewState="False"></asp:TextBox><IMG id="imgToDate" onclick="popUpCalendar(this,ctl00_CPHLAMBDA_txtToDate,'dd-mmm-yyyy');" alt="Select  Date" src="images/Calendar_scheduleHS.png" /> <asp:Button id="BtnGo" runat="server" Text="" CssClass="btn btngo" __designer:wfdid="w17"></asp:Button> <BR /></TD></TR><TR><TD style="WIDTH: 100%" align=center><asp:GridView id="GV_DWRDetail" runat="server" Width="100%" __designer:wfdid="w18" OnPageIndexChanging="GV_DWRDetail_PageIndexChanging" AllowPaging="True" PageSize="25" OnRowDeleting="GV_DWRDetail_RowDeleting" AutoGenerateColumns="False" OnRowCreated="GV_DWRDetail_RowCreated"><Columns>
<asp:BoundField DataField="nDWRHdrNo" HeaderText="DWRHdrNo"></asp:BoundField>
<asp:BoundField DataField="nDWRDtlNo" HeaderText="DWRDtlNo"></asp:BoundField>
<asp:BoundField DataField="vActivityId" HeaderText="ActivityId"></asp:BoundField>
<asp:BoundField DataField="dReportDate" HeaderText="Date">
<ItemStyle Wrap="False"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="vDay" HeaderText="Day"></asp:BoundField>
<asp:BoundField DataField="vWorkspaceDesc" HeaderText="Project"></asp:BoundField>
<asp:BoundField DataField="vSiteName" HeaderText="Site name"></asp:BoundField>
<asp:BoundField DataField="vCityName" HeaderText="Visited City"></asp:BoundField>
<asp:BoundField DataField="vWorkTypeDesc" HeaderText="Work Type"></asp:BoundField>
<asp:BoundField DataField="vActivityName" HeaderText="Activity"></asp:BoundField>
<asp:BoundField DataField="dFromTime" HeaderText="From Time"></asp:BoundField>
<asp:BoundField DataField="dToTime" HeaderText="To Time"></asp:BoundField>
<asp:BoundField DataField="vReasonDesc" HeaderText="Reason"></asp:BoundField>
<asp:BoundField DataField="vRemark" HeaderText="Remark"></asp:BoundField>
<asp:TemplateField HeaderText="Delete"><ItemTemplate>
                                <asp:LinkButton ID="lnkDelete" runat="server" onclientclick="return confirm('Are You sure You want to DELETE?')">Delete</asp:LinkButton>
                            
</ItemTemplate>
</asp:TemplateField>
</Columns>

<HeaderStyle BackColor="#FFA24A" ForeColor="White"></HeaderStyle>
</asp:GridView>&nbsp; <asp:Button id="BtnExport" onclick="BtnExport_Click" runat="server" CssClass="btn btnexcel" __designer:wfdid="w19"></asp:Button></TD></TR></TBODY></TABLE><asp:HiddenField id="HFromDate" runat="server"></asp:HiddenField> <asp:HiddenField id="HToDate" runat="server"></asp:HiddenField> 
</contenttemplate>
        <triggers>
<asp:PostBackTrigger ControlID="BtnExport"></asp:PostBackTrigger>

</triggers>
    </asp:UpdatePanel>
                       
</asp:Content>
