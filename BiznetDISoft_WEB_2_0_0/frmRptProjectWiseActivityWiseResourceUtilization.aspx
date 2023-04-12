<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmRptProjectWiseActivityWiseResourceUtilization.aspx.vb" Inherits="frmRptProjectWiseActivityWiseResourceUtilization" %>

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
        function ValidationForActivity() {
            var chklst = document.getElementById('<%=chklstActivity.clientid%>');
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

            if (!result) {
                msgalert('Please Select Atleast One Site !');
                return false;
            }

            if (document.getElementById('<%=txtFromDate.clientid%>').value == '') {
                msgalert('Please Enter From Date !');
                return false;
            }
            if (document.getElementById('<%=txtToDate.clientid%>').value == '') {
                msgalert('Please Enter To Date !');
                return false;
            }
            if (CompareDate(document.getElementById('<%=txtFromDate.ClientID%>').value, document.getElementById('<%=txtToDate.ClientID%>').value) != true) {
                return false;

            }
            return true;
        }
    </script>

    <table style="width: 100%" cellpadding="3px">
        <tbody>
            <tr>
                <td style="text-align: right; width: 30%;" class="Label">
                    <strong class="Label">Project Name/Project No./Request ID :</strong>
                </td>
                <td style="white-space: nowrap; text-align: left;">
                    <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="70%"></asp:TextBox><asp:Button
                        Style="display: none" ID="btnSetProject" OnClick="btnSetProject_Click" runat="server"
                        Text=" Project"></asp:Button><asp:HiddenField ID="HProjectId" runat="server">
                    </asp:HiddenField>
                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                        TargetControlID="txtProject" ServicePath="AutoComplete.asmx" ServiceMethod="GetProjectCompletionListWithOutSponser"
                        OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1"
                        CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                        CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1"
                        CompletionListElementID="pnlProjectList">
                    </cc1:AutoCompleteExtender>
                    <asp:Button ID="BtnAll" runat="server" Font-Bold="True" Text="All" ToolTip="All"
                        CssClass="btn btnnew" ></asp:Button>
                    <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 300px; overflow: auto;
                        overflow-x: hidden" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <strong class="Label">Activity :</strong>
                </td>
                <td align="left">
                    <asp:CheckBox ID="chkSelectAll" runat="server" Text="Select All" /><br />
                    <asp:Panel ID="pnlActivity" runat="server" ScrollBars="Auto" BorderStyle="Solid"
                        BorderWidth="1px" Style="max-height: 80px; max-width: 70%;">
                        <asp:CheckBoxList ID="chklstActivity" runat="server" CssClass="checkboxlist" RepeatColumns="2"
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
                <td style="text-align: right;">
                    <strong class="Label">Date :</strong>
                </td>
                <td style="text-align: left;">
                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="textBox" Enabled="true"></asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate"  Format="dd-MMM-yyyy"></cc1:CalendarExtender>
                    <strong class="Label">To : </strong>
                    <asp:TextBox ID="txtToDate" runat="server" CssClass="textBox" Enabled="true"></asp:TextBox>
                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"  Format="dd-MMM-yyyy"></cc1:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td style="text-align: center;" colspan="2" class="Label">
                    <asp:Button ID="BtnGo" runat="server" Font-Bold="True" Text="" ToolTip="Go" CssClass="btn btngo"
                        Width="5%" OnClientClick="return ValidationForActivity();"></asp:Button>
                    <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" ToolTip="Cancel" Text="Cancel"
                        Font-Bold="True" />
                    <asp:Button ID="btnExit" runat="server" CssClass="btn btnexit" ToolTip="Exit" Text="Exit"
                        Font-Bold="True" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this); " />
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
