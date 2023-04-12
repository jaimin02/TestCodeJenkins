<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" 
CodeFile="frmViewExpense.aspx.vb" Inherits="frmViewExpense"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" Runat="Server">
   <script type="text/javascript" src="Script/popcalendar.js"></script>

    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<TABLE style="WIDTH: 100%"><TBODY><TR><TD style="WIDTH: 100%" class="Label" align=center><asp:Label id="lblUser" runat="server" Text="User:" __designer:wfdid="w188"></asp:Label><asp:DropDownList id="ddlUser" runat="server" CssClass="dropDownList" Width="238px" __designer:wfdid="w4"></asp:DropDownList></TD></TR><TR><TD style="WIDTH: 100%; HEIGHT: 36px" class="Label" align=center>From Date: <asp:TextBox id="txtFromDate" tabIndex=1 runat="server" CssClass="textBox" Width="171px" AutoPostBack="True" Enabled="False" EnableViewState="False"></asp:TextBox><IMG id="imgFromDate" onclick="popUpCalendar(this,ctl00_CPHLAMBDA_txtFromDate,'dd-mmm-yyyy');" alt="Select  Date" src="images/Calendar_scheduleHS.png" /> To Date: <asp:TextBox id="txtToDate" tabIndex=2 runat="server" CssClass="textBox" Width="171px" AutoPostBack="True" Enabled="False" EnableViewState="False"></asp:TextBox><IMG id="imgToDate" onclick="popUpCalendar(this,ctl00_CPHLAMBDA_txtToDate,'dd-mmm-yyyy');" alt="Select  Date" src="images/Calendar_scheduleHS.png" /> <asp:Button id="BtnGo" tabIndex=3 runat="server" Text="" CssClass="btn btngo"></asp:Button> <BR /></TD></TR><TR><TD style="WIDTH: 100%" align=center><asp:GridView id="GV_OtherExpDetail" runat="server" Width="100%" OnPageIndexChanging="GV_OtherExpDetail_PageIndexChanging" AllowPaging="True" PageSize="25" OnRowDeleting="GV_OtherExpDetail_RowDeleting" AutoGenerateColumns="False" OnRowCreated="GV_OtherExpDetail_RowCreated"><Columns>
<asp:BoundField DataField="nOtherExpHdrNo" HeaderText="OtherExpHdrNo"></asp:BoundField>
<asp:BoundField DataField="nOtherExpDtlNo" HeaderText="OtherExpDtlNo"></asp:BoundField>
<asp:BoundField DataField="dFromDate" HeaderText="FromDate">
<ItemStyle Wrap="False"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="dToDate" HeaderText="ToDate">
<ItemStyle Wrap="False"></ItemStyle>
</asp:BoundField>
<asp:BoundField DataField="vFromDay" HeaderText="FromDay"></asp:BoundField>
<asp:BoundField DataField="vToDay" HeaderText="ToDay"></asp:BoundField>
<asp:BoundField DataField="vWorkspaceDesc" HeaderText="Project"></asp:BoundField>
<asp:BoundField DataField="vSiteName" HeaderText="Site name"></asp:BoundField>
<asp:BoundField DataField="vOtherExpName" HeaderText="Expense"></asp:BoundField>
<asp:TemplateField HeaderText="Amount"><ItemTemplate>
<asp:TextBox id="txtExpAmt" runat="server" Text='<%# Eval("iExpAmt") %>' CssClass="textBox" Width="70px" __designer:wfdid="w3"></asp:TextBox> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="vRemarks" HeaderText="Remarks"></asp:BoundField>
<asp:BoundField DataField="vRefDetail" HeaderText="Referance Detail"></asp:BoundField>
<asp:TemplateField HeaderText="Ref. Attachment"><ItemTemplate>
<asp:HyperLink id="hlnkFile" runat="server" Text='<%# Eval("vAttachment") %>' Target="_blank" __designer:wfdid="w1"></asp:HyperLink> 
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="iTotalExpAmt" HeaderText="Total Expense"></asp:BoundField>
<asp:TemplateField HeaderText="Delete"><ItemTemplate>
                                <asp:LinkButton ID="lnkDelete" runat="server" onclientclick="return confirm('Are You sure You want to DELETE?')" >Delete</asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Approve"><ItemTemplate>
<asp:LinkButton id="lnkApprove" runat="server" __designer:wfdid="w2">Approve</asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:TemplateField HeaderText="Reject"><ItemTemplate>
<asp:LinkButton id="lnkReject" runat="server" __designer:wfdid="w3">Reject</asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
<asp:BoundField DataField="cApprovalFlag" HeaderText="ApprovalFlag"></asp:BoundField>
<asp:BoundField DataField="vApprovalUserName" HeaderText="ApprovalUserName"></asp:BoundField>
</Columns>

<HeaderStyle BackColor="#FFA24A" ForeColor="White"></HeaderStyle>
</asp:GridView>&nbsp; <asp:Button id="BtnExport" tabIndex=4 onclick="BtnExport_Click" runat="server"  CssClass="btn btnexcel" ></asp:Button></TD></TR></TBODY></TABLE><asp:HiddenField id="HFromDate" runat="server"></asp:HiddenField> <asp:HiddenField id="HToDate" runat="server"></asp:HiddenField> 
</contenttemplate>
        <triggers>
<asp:PostBackTrigger ControlID="BtnExport"></asp:PostBackTrigger>
</triggers>
    </asp:UpdatePanel>
                       

</asp:Content>

