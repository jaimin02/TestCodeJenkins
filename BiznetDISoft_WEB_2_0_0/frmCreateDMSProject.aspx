<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmCreateDMSProject.aspx.vb" Inherits="frmCreateDMSProject" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="CPHLAMBDA" runat="server">

    <script type="text/javascript" language="javascript" src="Script/popcalendar.js">
    </script>

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" src="Script/Validation.js"></script>

    <table style="width: 100%;">
        <tr>
            <td id="tdEdit" runat="server" style="white-space: nowrap">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        Search Project :
                        <asp:TextBox ID="txtsearch" runat="server" CssClass="textBox" Width="45%" />
                        <asp:Button Style="display: none" ID="BtnEdit" runat="server" ForeColor="Black" Font-Bold="True"
                            Text="Edit" BackColor="White" BorderColor="Black" BorderStyle="Solid" />
                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                            CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                            CompletionListElementID="pnlProjectList" CompletionListItemCssClass="autocomplete_listitem"
                            MinimumPrefixLength="1" OnClientShowing="ClientPopulated" ServiceMethod="GetProjectCompletionListWithOutSponser"
                            ServicePath="AutoComplete.asmx" TargetControlID="txtsearch" UseContextKey="True"
                            OnClientItemSelected="OnSelected">
                        </cc1:AutoCompleteExtender>
                        <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 300px; overflow: auto;
                            overflow-x: hidden" />
                        <asp:HiddenField ID="HWorkspaceId" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <table id="tdContainer" runat="server" style="width: 100%; border-spacing: 5px;">
                            <tbody>
                                <tr>
                                    <td style="font-weight: bold; text-align: center;" colspan="2">
                                        <input style="width: 126px" id="TxtProNo" class="textBox " type="text" runat="server"
                                            visible="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Label" style="text-align: right; width: 40%">
                                        Project Name*:
                                    </td>
                                    <td style="font-weight: bold; text-align: left;">
                                        <input style="width: 318px" id="TxtProject" class="textBox " type="text" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Label" style="text-align: right;">
                                        Template Name*:
                                    </td>
                                    <td style="text-align: left;">
                                        <select style="width: 40%" id="SlcTemplate" class="dropDownList" runat="server" />
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Label" style="text-align: right;">
                                        Project Type*:
                                    </td>
                                    <td style="text-align: left;">
                                        <select style="width: 40%" id="SlcProject" class="dropDownList" runat="server" />
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Label" style="text-align: right;">
                                        No. of Subjects*:
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox onblur="return Numeric();" ID="TxtSubNo" runat="server" CssClass="textBox "
                                            Width="5%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center" class="Label " colspan="2">
                                        <asp:Button ID="BtnSave" runat="server" Font-Bold="True" Text="Save" ToolTip="Save"
                                            CssClass="btn btnsave" OnClientClick="return Validation();" />
                                        <asp:Button ID="BtnCancle" runat="server" Font-Bold="True" Text="Cancel" ToolTip="Cancel"
                                            CssClass="btn btncancel" />
                                        <asp:Button ID="btnExit" OnClick="btnExit_Click" runat="server" Font-Bold="True"
                                            Text="Exit" ToolTip="Exit" CssClass="btn btnexit" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);exit" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="BtnCancle" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="BtnEdit" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnExit" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">

        function ClientPopulated(sender, e) {
            var searchText = $get('<%= txtsearch.ClientId %>');
            ProjectClientShowing('AutoCompleteExtender1', searchText);
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtsearch.clientid %>'), $get('<%= hworkspaceid.clientid %>'), document.getElementById('<%=BtnEdit.ClientID%>'));
        }

        function RedirectPage(Msg) {
            if (
                msgalert(Msg)) {
                window.location.href = 'frmProtocolDetail.aspx?mode=2';
            }
        }

        function Numeric() {
            if (!(CheckInteger(document.getElementById('<%=TxtSubNo.ClientID%>').value))) {
                msgalert('Please Enter Numeric Value Only !');
                document.getElementById('<%=TxtSubNo.ClientID%>').value = '0';
                document.getElementById('<%=TxtSubNo.ClientID%>').focus();
                return false;
            }
            return true;
        }

        function Validation() {
            if (document.getElementById('<%=TxtProject.ClientID%>').disabled == false && document.getElementById('<%=TxtProject.ClientID%>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Project Name !');
                document.getElementById('<%=TxtProject.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=SlcTemplate.ClientID%>').disabled == false && document.getElementById('<%=SlcTemplate.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Template !');
                document.getElementById('<%=SlcTemplate.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=SlcProject.ClientID%>').disabled == false && document.getElementById('<%=SlcProject.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Project Type !');
                document.getElementById('<%=SlcProject.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=TxtSubNo.ClientID%>').disabled == false && document.getElementById('<%=TxtSubNo.ClientID%>').value.toString().trim().length <= 0) {
                msgalert('Please Enter No Of Subjects !');
                document.getElementById('<%=TxtSubNo.ClientID%>').focus();
                return false;
            }

            return true;

        }

    </script>

</asp:Content>
