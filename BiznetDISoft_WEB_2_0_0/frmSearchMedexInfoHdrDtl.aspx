<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmSearchMedexInfoHdrDtl.aspx.vb" Inherits="frmSearchMedexInfoHdrDtl" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/General.js"></script>

    <script type="text/javascript" src="Script/Validation.js"></script>

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="width: 100%" cellpadding="5px">
                <tbody>
                    <tr>
                        <td style="width: 40%; text-align: right;">
                            Attribute:
                        </td>
                        <td style="text-align: left;" class="Label">
                            <asp:TextBox ID="txtMedex" runat="server" CssClass="textBox" Width="40%" TabIndex="1" />
                            <asp:HiddenField ID="hMedexCode" runat="server" />
                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" ServiceMethod="GetMedexList"
                                UseContextKey="True" TargetControlID="txtMedex" ServicePath="AutoComplete.asmx"
                                OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1"
                                CompletionListElementID="pnlAttributeList" CompletionListItemCssClass="autocomplete_listitem"
                                CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListCssClass="autocomplete_list"
                                BehaviorID="AutoCompleteExtender1">
                            </cc1:AutoCompleteExtender>
                            <asp:Panel ID="pnlAttributeList" runat="server" Style="max-height: 300px; overflow: auto;overflow-x:hidden"/>
                            <asp:Button Style="display: none" ID="btnSetMedex" runat="server" Text="Medex" ToolTip="Medex" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            Select Option:
                        </td>
                        <td class="Label" style="text-align: left;">
                            <asp:RadioButtonList ID="rblstOption" runat="server" Font-Size="10" TabIndex="2"
                                RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True" Value="Like">Like</asp:ListItem>
                                <asp:ListItem Value="=">=</asp:ListItem>
                                <asp:ListItem Value="&gt;=">&gt;=</asp:ListItem>
                                <asp:ListItem Value="&lt;=">&lt;=</asp:ListItem>
                                <asp:ListItem Value="&lt;&gt;">&lt;&gt;</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            Attribute Value:
                        </td>
                        <td style="text-align: left;" class="Label">
                            <asp:TextBox ID="txtMedexValue" TabIndex="3" runat="server" CssClass="textBox" Width="40%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;" class="Label" colspan="2">
                            <asp:Button ID="BtnAnd" TabIndex="4" runat="server" Text="AND" ToolTip=" And" CssClass="btn btnnew"
                                 OnClientClick="return CheckValidation();" />
                            <asp:Button ID="BtnOr" TabIndex="5" runat="server" Text="OR" ToolTip="Or" CssClass="btn btnnew"
                                OnClientClick="return CheckValidation();" />
                            <asp:Button ID="BtnEnd" TabIndex="6" runat="server" Text="END" ToolTip="End" CssClass="btn btnnew" 
                                 OnClientClick="return CheckValidation();" />
                            <asp:Button ID="BtnCancel" TabIndex="7" runat="server" Text="CANCEL" ToolTip=" Cancel"
                                CssClass="btn btncancel" />
                            <asp:Button ID="BtnExit" TabIndex="8" runat="server" Text="EXIT" ToolTip="Exit" CssClass="btn btnexit"
                                OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);
" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: center; width: 100%">
                            <asp:GridView ID="GVSearch" runat="server" SkinID="GVPage1" AutoGenerateColumns="False"
                                OnRowDeleting="GVSearch_RowDeleting" TabIndex="9">
                                <Columns>
                                    <asp:BoundField DataField="SrNo" HeaderText="Sr. No" />
                                    <asp:BoundField DataField="SearchCondition" HeaderText="Search Condition" />
                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgBtnDelete" runat="server" ImageUrl="~/Images/i_delete.gif"
                                                ToolTip="Delete" OnClientClick="return confirm('Are You Sure You Want To Delete?')" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td class="Label" style="text-align: center;" colspan="2">
                            <asp:Button ID="BtnSearch" TabIndex="10" runat="server" Font-Size="Larger" Font-Bold="True"
                                Text="Search" ToolTip="Search" CssClass="btn btnnew" Width="5%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%" align="center" colspan="2">
                            <asp:GridView ID="GVResult" runat="server" SkinID="grdViewAutoSizeMax" style="width:60%; margin:auto;" AutoGenerateColumns="False"
                                TabIndex="11">
                                <Columns>
                                    <asp:BoundField HeaderText="Sr.No">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vWorkSpaceId" HeaderText="vWorkSpaceId" />
                                    <asp:BoundField DataField="vWorkSpaceDesc" HeaderText="Project" />
                                    <asp:TemplateField HeaderText="View Project Details">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkProjectDetails" runat="server">Project Details</asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        function ClientPopulated(sender, e) {
            MedexShowing('AutoCompleteExtender1', $get('<%= txtMedex.ClientId %>'));
        }

        function OnSelected(sender, e) {
            OnMedexSelected(e.get_value(), $get('<%= txtMedex.clientid %>'),
                    $get('<%= hMedexCode.clientid %>'), document.getElementById('<%= btnSetMedex.ClientId %>'));

        }

        function CheckValidation() {
            if (document.getElementById('<%= hMedexCode.ClientId %>').value == '') {
                msgalert('Please Enter Attribute');
                document.getElementById('<%= txtMedex.ClientId %>').value = '';
                document.getElementById('<%= txtMedex.ClientId %>').focus();
                return false;
            }
            else if (document.getElementById('<%= txtMedexValue.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Attribute Value');
                document.getElementById('<%= txtMedexValue.ClientId %>').value = '';
                document.getElementById('<%= txtMedexValue.ClientId %>').focus();
                return false;
            }
            return true;
        }
    
    </script>

</asp:Content>
