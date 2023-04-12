<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmDocTemplateMst.aspx.vb" Inherits="frmDocTemplateMst"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" language="javascript">
function Validation()
{
    if (document.getElementById('<%=txtDocTemplateName.ClientID%>').value.toString().trim().length <= 0)
    {
        document.getElementById('<%=txtDocTemplateName.ClientID%>').value = '';
        msgalert('Please enter Document Template Name');
        document.getElementById('<%=txtDocTemplateName.ClientID%>').focus();
        return false;
    }
    return true;
}
    </script>

    <table>
        <tr>
            <td align="right" style="white-space: nowrap" valign="top" class="Label">
                Document Template Name* :</td>
            <td align="left" valign="top">
                <asp:TextBox ID="txtDocTemplateName" runat="server" CssClass="textBox" Width="193px"></asp:TextBox></td>
        </tr>
        <tr>
            <%--<td align="right" valign="top">
                document template description :</td>
            <td align="left" valign="top">
                <asp:textbox id="txtdescription" runat="server" cssclass="textbox" width="193px"></asp:textbox>
            </td>--%>
        </tr>
        <tr>
            <td align="right" valign="top">
            </td>
            <td align="left" valign="top">
                <asp:Button ID="btnSave" runat="server" CssClass="btn btnsave" Text="Save" />&nbsp;<asp:Button
                    ID="btnCancel" runat="server" CausesValidation="False" CssClass="btn btncancel" OnClick="btnCancel_Click"
                    Text="Cancel" />&nbsp;<asp:Button ID="btnExit" runat="server" CausesValidation="False"
                        CssClass="btn btnexit" Text="Exit" onclientclick="return msgconfirmalert('Are you sure you want to Exit?',this);" /></td>
        </tr>
        <tr>
            <td align="center" colspan="2" valign="top">
                <asp:GridView ID="gvdoctemplatemst" runat="server" AutoGenerateColumns="False" SkinID="grdViewAutoSizeMax" style="width:60%; margin:auto;"
                     AllowPaging="True" PageSize="25" ShowFooter="True">
                    <Columns>
                        <asp:BoundField HeaderText="#">
                            <ItemStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="vDocTemplateId" HeaderText="Id" />
                        <asp:BoundField DataField="vDocTemplateName" HeaderText="Document Template">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="dModifyOn" DataFormatString="{0:dd-MMM-yyyy}" HeaderText="ModifyOn"
                            HtmlEncode="False">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:TemplateField SortExpression="status" HeaderText="Edit">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" Text="Edit" runat="server"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
