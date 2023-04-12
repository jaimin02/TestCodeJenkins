 <%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmLoginAuditTrail, App_Web_5xoe1jh1" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
<TABLE style="WIDTH: 100%" cellSpacing=0 cellPadding=0><TBODY><TR><TD style="PADDING-BOTTOM: 5px"><asp:Label id="lblMessage" runat="server" CssClass="Label" __designer:wfdid="w19"></asp:Label> </TD></TR><TR><TD align=center><TABLE cellPadding=2><TBODY><TR><TD align=left><asp:RadioButton id="rdoMonth" runat="server" Text="Last Month" GroupName="Filter" AutoPostBack="true" __designer:wfdid="w20"></asp:RadioButton> <asp:RadioButton id="rdoQuarter" runat="server" Text="Last Quarter" GroupName="Filter" AutoPostBack="true" __designer:wfdid="w21"></asp:RadioButton> <asp:RadioButton id="rdoYear" runat="server" Text="Last Year" GroupName="Filter" AutoPostBack="true" __designer:wfdid="w22"></asp:RadioButton> </TD></TR><TR><TD style="TEXT-ALIGN: center" align=center><TABLE cellSpacing=0 cellPadding=0><TBODY><TR><TD style="PADDING-BOTTOM: 2px; PADDING-TOP: 1px; TEXT-ALIGN: left" align=left><asp:Label id="lblNote" runat="server" ForeColor="Red" Font-Size="9pt" Font-Names="Verdana" Text="Estimated Logout Time in Case of Abnormal Logout" __designer:wfdid="w23">
                                                </asp:Label> </TD></TR><TR><TD><asp:GridView id="gvwLoginAuditTrail" runat="server" SkinID="grdViewSml" CellPadding="2" PageSize="25" AllowPaging="True" OnPageIndexChanging= "gvwLoginAuditTrail_PageIndexChanging" AutoGenerateColumns="False" showfooter="true" __designer:wfdid="w24">
                                                    <Columns>
                                                        <asp:BoundField DataField="iUserID" HeaderText="UserID">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="#">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vFirstName" HeaderText="First Name">
                                                            <ItemStyle Wrap="False" />
                                                            <HeaderStyle Wrap="false" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vLastName" HeaderText="Last Name">
                                                            <ItemStyle Wrap="False" />
                                                            <HeaderStyle Wrap="false" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vLoginName" HeaderText="Login Name">
                                                            <ItemStyle Wrap="False" />
                                                            <HeaderStyle Wrap="false" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vLOFlag" HeaderText="Login Name">
                                                            <ItemStyle Wrap="False" />
                                                            <HeaderStyle Wrap="false" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="dInOutDateTime_IST" HtmlEncode="false"
                                                            DataFormatString="{0:dd-MMM-yyyy HH:mm tt}" HeaderText="Modify Date">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                </asp:GridView> </TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></TD></TR><TR><TD style="PADDING-TOP: 15px" align=center><INPUT id="btnClose" class="btn btnclose" onclick="self.close();" type=button value="Close" /> </TD></TR></TBODY></TABLE>
</ContentTemplate>
        <triggers>
<asp:AsyncPostBackTrigger ControlID="gvwLoginAuditTrail" EventName="PageIndexChanging"></asp:AsyncPostBackTrigger>
</triggers>
    </asp:UpdatePanel>
</asp:Content>
