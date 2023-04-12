<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmOVISExport, App_Web_ybumpksz" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script language="javascript" src="Script/popcalendar.js"></script>
     
    <script language="javascript" src="Script/General.js"></script>

    <script language="javascript" src="Script/Validation.js"></script>

    <script type="text/javascript" language="javascript">
        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%=txtProject.ClientId%>'));
        }
        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%=txtProject.clientid%>'),
            $get('<%=HProjectId.clientid%>'), document.getElementById('<%=btnSetProject.ClientId%>'));
        }
        function ValidationForSubject() {
            var chklst = document.getElementById('<%=chklstSubject.clientid%>');
            var chks;
            var result = false;
            var i;

            if (chklst != null && typeof (chklst) != 'undefined') {
                chks = chklst.getElementsByTagName('input');
                for (i = 0; i < chks.length; i++) {
                    if (chks[i].type.toUpperCase() == 'CHECKBOX' && chks[i].checked) {
                        result = true;
                        break;
                    }
                }
            }
            if (document.getElementById('<%= ddlPeriod.ClientId %>').selectedIndex == 0) {
                msgalert('Please select Period !');
                document.getElementById('<%= ddlPeriod.ClientId %>').focus();
                return false;
            }
            else if (!result) {
                msgalert('Please Select Atleast One Subject !');
                return false;
            }
            return true;
        }
    </script>

    <table style="width: 100%" cellpadding ="5px">
        <tbody>
            <tr>
                <td style ="text-align :right ; width :35%;">
                    <strong class="Label">Project Name/Project No./Request ID :</strong>
                </td>
                <td style="white-space: nowrap; text-align :left ;" >
                    <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="75%"></asp:TextBox><asp:Button
                        Style="display: none" ID="btnSetProject" OnClick="btnSetProject_Click" runat="server"
                        Text=" Project"></asp:Button><asp:HiddenField ID="HProjectId" runat="server">
                    </asp:HiddenField>
                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                        TargetControlID="txtProject" ServicePath="AutoComplete.asmx" ServiceMethod="GetProjectCompletionListWithOutSponser"
                        OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1"
                        CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                        CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1">
                    </cc1:AutoCompleteExtender>
                    <asp:Button ID="BtnAll" runat="server" Font-Bold="True" Text="All" CssClass="btn btnnew"
                        Visible="False"></asp:Button>
                </td>
            </tr>
            <tr>
                <td  style ="text-align :right ;">
                    <strong class="Label">Period :</strong>
                </td>
                <td style ="text-align :left ;">
                          <asp:DropDownList ID="ddlPeriod" runat="server" AutoPostBack="True" CssClass="dropDownList"
                           OnSelectedIndexChanged="ddlPeriod_SelectedIndexChanged" Width="203px" TabIndex="4" />
                </td>
            </tr>
            <tr>
                <td  style ="text-align :right ;">
                    <strong class="Label">Subject :</strong>
                </td>
                <td style ="text-align :left ;">
                    <asp:CheckBox ID="chkSelectAll" runat="server" Text="Select All" /><br />
                    <asp:Panel ID="pnlSubject" runat="server" Height="100px" ScrollBars="Auto" BorderStyle="Solid"
                        BorderWidth="1px" style="max-width :65%;">
                        <asp:CheckBoxList ID="chklstSubject" runat="server" CssClass="checkboxlist" RepeatColumns="2"
                            RepeatDirection="Horizontal">
                        </asp:CheckBoxList>
                    </asp:Panel>
                </td>
            </tr>
           <%-- <tr>
                <td align="left">
                </td>
                <td align="left">
                </td>
            </tr>--%>
            <tr>
               <%-- <td align="left">
                </td>--%>
                <td style ="text-align :center ;" colspan ="2" class ="Label">
                    <asp:Button ID="btnUpload" runat="server" Font-Bold="True" Text="Download" CssClass="btn btnnew" ToolTip ="Download OVIS File"
                         OnClientClick="return ValidationForSubject();"></asp:Button>
                    <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" Text="Cancel" Font-Bold="True" ToolTip ="Cancel" />
                    <asp:Button ID="btnExit" runat="server" CssClass="btn btnexit" Text="Exit" ToolTip ="Exit" Font-Bold="True"
                        OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
