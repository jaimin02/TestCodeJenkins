<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmSetProjectForTrial.aspx.vb" Inherits="frmSetProjectForTrial" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="conSetProject" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/popcalendar.js"></script>

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript">
        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
                    $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnProject.ClientId %>'));

        }
    </script>

    <table cellpadding="5px" style="width: 100%;">
        <%-- <tr style="height: 41px">
            <td>
            </td>
            <td>
            </td>
        </tr>--%>
      
        <tr>
           
            <td class="Label" style="text-align: right; width: 30%;">
                Select Project:
            </td>
            <td style="text-align: left;">
                <asp:TextBox ID="txtProject" runat="server" CssClass="textBox" TabIndex="1" Width="60%" />
                <asp:Button ID="btnProject" runat="server" Text=" Project " Style="display: none;" />
                <asp:HiddenField ID="HProjectId" runat="server" />
                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" MinimumPrefixLength="1"
                    OnClientItemSelected="OnSelected" OnClientShowing="ClientPopulated" TargetControlID="txtProject"
                    BehaviorID="AutoCompleteExtender1" ServiceMethod="GetMyProjectCompletionList"
                    ServicePath="AutoComplete.asmx" UseContextKey="True" CompletionListCssClass="autocomplete_list"
                    CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListItemCssClass="autocomplete_listitem"
                    CompletionListElementID="pnlProjectList">
                </cc1:AutoCompleteExtender>
                <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto;
                    overflow-x: hidden" />
            </td>
        </tr>
        <%--<tr runat="server" id="trPeriod" style="display: none">
            <td class="Label" style="text-align: right;">
                Period:
            </td>
            <td style="text-align: left;">
                <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="dropDownList" TabIndex="2" />
            </td>
        </tr>--%>
        <tr>
            <%--<td>
            </td>--%>
            <td colspan="2" style="text-align: center;">
                <asp:Button ID="btnOk" runat="server" Text="Set" ToolTip="Ok" CssClass="btn btnnew"
                    TabIndex="2" Enabled="false" />
                <asp:Button ID="btnUnSet" runat="server" Text="UnSet" ToolTip="Unset" CssClass="btn btnnew"
                    TabIndex="3" />
                <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" Text="Cancel" ToolTip="Cancel"
                    TabIndex="4" />
                <asp:Button ID="btnExit" runat="server" ToolTip="Exit" Text="Exit" CssClass="btn btnexit"
                    CausesValidation="False" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);"
                    TabIndex="5"></asp:Button>
            </td>
        </tr>
    </table>
   </asp:Content>
