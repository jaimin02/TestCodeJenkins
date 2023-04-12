
 <%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="frmPasswordHistoryAuditTrail.aspx.vb" Inherits="frmPasswordHistoryAuditTrail"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" Runat="Server">
    <table cellpadding="0" cellspacing="0" style="width:100%;">
            <tr>
                <td style="height:5px;">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:GridView ID="gvwPasswordHistoryAuditTrail" runat="server" 
                            AutoGenerateColumns="False" 
                            ShowFooter = "false" 
                            SkinID="grdViewSmlSize" CellPadding="2">
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
                                <asp:BoundField DataField="vModifyBy" HeaderText="Modify By">
                                    <ItemStyle Wrap="False" />
                                    <HeaderStyle Wrap="false" />
                                </asp:BoundField>
                                <asp:BoundField DataField="dChangedDate_IST" 
                                    HtmlEncode = "false" 
                                    DataFormatString="{0:dd-MMM-yyyy HH:mm:ss tt}"
                                    HeaderText="Modify Date">
                                    <ItemStyle Wrap="False" />
                                </asp:BoundField>
                            </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td align="center" style="padding-top:15px;">
                    <input type="button" id="btnClose" onclick="self.close();"
                    class="btn btnclose" value="Close" />
                </td>
            </tr>
        </table>
</asp:Content>

