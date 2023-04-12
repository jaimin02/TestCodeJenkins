<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmRptProjectWiseSiteResourceUtilization.aspx.vb" Inherits="frmRptProjectWiseSiteResourceUtilization" %>

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
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%=btnSetProject.ClientId%>'));
        }
        function ValidationForSTP() {
            var chklst = document.getElementById('<%=chklstSTP.clientid%>');
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
        <tr>
            <td style="text-align: right;">
                <strong class="Label">Project Name/Project No./Request ID :</strong>
            </td>
            <td style="white-space: nowrap; text-align: left;">
                <asp:TextBox ID="txtProject" runat="server" CssClass="textBox" Width="70%"></asp:TextBox><asp:Button
                    ID="btnSetProject" runat="server" Style="display: none" Text=" Project" /><asp:HiddenField
                        ID="HProjectId" runat="server" />
                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                    CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                    CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                    OnClientShowing="ClientPopulated" ServiceMethod="GetProjectCompletionListWithOutSponser"
                    ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True"
                    CompletionListElementID="pnlProjectList">
                </cc1:AutoCompleteExtender>
                <asp:Button ID="BtnAll" runat="server" CssClass="btn btnnew" Font-Bold="True" ToolTip="All"
                    Text="All" />
                <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" />
            </td>
        </tr>
        <tr>
            <td style="text-align: right;">
                <strong class="Label">Site :</strong>
            </td>
            <td align="left">
                <asp:CheckBox ID="chkSelectAll" runat="server" Text="Select All" /><br />
                <asp:Panel ID="pnlSTP" runat="server" ScrollBars="Auto" BorderStyle="Solid" BorderWidth="1px"
                    Style="max-height: 80px; max-width: 70%;">
                    <asp:CheckBoxList ID="chklstSTP" runat="server" CssClass="checkboxlist" RepeatColumns="2"
                        RepeatDirection="Horizontal">
                    </asp:CheckBoxList>
                </asp:Panel>
            </td>
        </tr>
        <%--<tr>
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
                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate" Format="dd-MMM-yyyy">
                </cc1:CalendarExtender>


                &nbsp;<strong class="Label">To : </strong>
                <asp:TextBox ID="txtToDate" runat="server" CssClass="textBox" Enabled="true"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></cc1:CalendarExtender>
            </td>
        </tr>

        <tr>
            <td class="Label" colspan="2" style="text-align: center;">
                <asp:Button ID="BtnGo" OnClientClick="return ValidationForSTP();" runat="server"
                    CssClass="btn btngo" Font-Bold="True" Text="" ToolTip="Go" />&nbsp;<asp:Button
                        ID="btnCancel" runat="server" CssClass="btn btncancel" Text="Cancel" ToolTip="Cancel"
                        Font-Bold="True" />
                <asp:Button ID="btnExit" runat="server" CssClass="btn btnexit" Text="Exit" ToolTip="Exit"
                    Font-Bold="True" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this); " />
            </td>
        </tr>
    </table>
</asp:Content>
