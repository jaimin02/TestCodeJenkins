
<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="frmUserAuditTrail.aspx.vb" Inherits="frmUserAuditTrail"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
   <script type="text/javascript"> 
// for fix gridview header aded on 22-nov-2011
         function Fixheader()
        {
            FreezeTableHeader($('#<%= gvwUserAuditTrail.ClientID %>'), { height: 250, width: 900 });
        }
        
    </script>
    <script type="text/javascript" src="Script/scrollablegrid.js"></script>
    <asp:UpdatePanel ID="upPnlUserAuditTrail" runat="server" UpdateMode="Conditional"
        RenderMode="Inline">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="0" style="width: 100%;">
                <tr>
                    <td align="center">
                        <table cellpadding="3" cellspacing="0">
                            <tr>
                                <td class="Label">
                                    User:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlUsers" runat="server" CssClass="dropDownList" Width="250px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btnView" runat="server" Text="View" CssClass="btn btnnew" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="height: 10px;">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <div style="height:250px; width:970px; overflow:auto;">
                        <asp:Gridview id="gvwUserAuditTrail" runat="server" AutoGenerateColumns="False"  SkinID="grdViewAutoSizeMax">
                            <Columns>
                                <asp:BoundField DataField="iUserID" HeaderText="UserID">
                                    <ItemStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="vLoginName" HeaderText="User Name">
                                    <ItemStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="vLastName" HeaderText="Last Name">
                                    <ItemStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="vFirstName" HeaderText="First Name">
                                    <ItemStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="vScopeName" HeaderText="Scope">
                                    <ItemStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="dUserCreatedDate" HtmlEncode="false"
                                    DataFormatString="{0:dd-MMM-yyyy HH:mm tt}" HeaderText="Creation Date">
                                    <ItemStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="dUserLastModifyDate" HtmlEncode="false" 
                                    DataFormatString="{0:dd-MMM-yyyy HH:mm tt}" HeaderText="Last Edit Date">
                                    <ItemStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="User History">
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Wrap="false" />
                                    <ItemTemplate>
                                        <asp:Button ID="btnUserHistory" runat="server" CommandName="UserHistory" Text="View"
                                            CommandArgument='<%# Bind("iUserId") %>'
                                             CssClass="btn btnnew"></asp:Button>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Password History">
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Wrap="false" />
                                    <ItemTemplate>
                                        <asp:Button ID="btnPasswordHistory" runat="server" CommandName="PasswordHistory"
                                            CommandArgument='<%# Bind("iUserId") %>'
                                            Text="View"  CssClass="btn btnnew"></asp:Button>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Login Details">
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Wrap="false" />
                                    <ItemTemplate>
                                        <asp:Button ID="btnLoginDetails" runat="server" CommandName="LoginDetails" Text="View"
                                            CssClass="btn btnnew" CommandArgument='<%# Bind("iUserId") %>'></asp:Button>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
