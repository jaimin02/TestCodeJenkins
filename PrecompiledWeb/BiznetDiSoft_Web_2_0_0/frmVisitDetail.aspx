<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmVisitDetail, App_Web_pna05jsx" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" Runat="Server">
    <table width="100%">
        <tr>
            <td style="width: 100%">
                <asp:Label ID="lblSubject" runat="server" CssClass="Label"></asp:Label></td>
        </tr>
        <tr>
            <td class="Label" style="width: 100%; text-align: left">
                <asp:Button ID="btnBack" runat="server" CssClass="btn btnback" Text="" /></td>
        </tr>
        <tr>
            <td style="width: 100%">
                <asp:GridView ID="GVVisits" runat="server" AutoGenerateColumns="False" SkinID="grdVForProjectDetail">
                    <Columns>
                        <asp:BoundField DataField="iPeriod" HeaderText="iPeriod" />
                        <asp:TemplateField HeaderText="Visit">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPresent" runat="server" Text='<%# eval("iPeriod") %>' OnClientClick="return confirm('Are You Sure Subject Is Present?');"></asp:LinkButton>
                                <asp:Label ID="lblVisit" Text='<%# eval("iPeriod") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="cPresent" HeaderText="cPresent" />
                        <asp:TemplateField HeaderText="Visited on">
                            <ItemTemplate>
                                <asp:Label ID="lblPresent" Text='<%# eval("vPresentOn") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Visit Detail">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkView" runat="server">Visit Detail</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td style="width: 100%">
                <asp:LinkButton ID="lnkUnScheculed" runat="server">Add Unscheduled Visit</asp:LinkButton></td>
        </tr>
    </table>
    <br />
</asp:Content>

