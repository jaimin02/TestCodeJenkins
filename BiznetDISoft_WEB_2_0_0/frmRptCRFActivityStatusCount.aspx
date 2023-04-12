<%@ Page Title="" Language="VB" MasterPageFile="~/ECTDMasterPage.master" AutoEventWireup="false" 
        CodeFile="frmRptCRFActivityStatusCount.aspx.vb" Inherits="frmRptCRFActivityStatusCount" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <style type="text/css">
        .FooterColor {
            color: white !important;
            font-weight: bold !important;
        }
    </style>
    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <asp:UpdatePanel ID="upcontrols" runat="server">
        <ContentTemplate>
            <table style="width: 90%">
                <tr>
                    <td colspan="3" style="height: 10px;"></td>
                </tr>
                <tr>

                    <td class="LabelText" nowrap="nowrap" style="text-align: right; width: 35%;">Project Name/Request Id*:
                    </td>
                    <td class="Label" style="text-align: left; width: 40%">
                        <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="92%" TabIndex="1" Style="margin-left: 5px;"></asp:TextBox>
                        <asp:Button Style="display: none" ID="btnSetProject" runat="server" Text=" Project" CssClass="btn btnnew"></asp:Button>
                        <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                            CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                            CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                            OnClientShowing="ClientPopulated" ServiceMethod="GetMyProjectCompletionList"
                            ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True"
                            CompletionListElementID="pnlProjectList">
                        </cc1:AutoCompleteExtender>

                        <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" />
                    </td>
                    <td class="Label" style="text-align: left; width: 25%">
                        <asp:CheckBox ID="chkParent" runat="server" Text="Include Parent" Style="display: none;" onChange="RemoveGrid();" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="height: 3px;"></td>
                </tr>
                <tr>
                    <td class="LabelText" nowrap="nowrap" style="text-align: right; width: 35%;">Period:
                    </td>
                    <td class="Label" nowrap="nowrap" style="text-align: left; width: 65%;" colspan="2">
                        <asp:DropDownList ID="ddlPeriods" CssClass="EntryControl" runat="server" AutoPostBack="false"
                            Style="width: 230px; margin-left: 5px;" TabIndex="2" onChange="RemoveGrid();">
                        </asp:DropDownList>
                    </td>

                </tr>
                <tr>
                    <td colspan="3" style="height: 10px;"></td>
                </tr>
                <tr>
                    <td style="width: 35%;"></td>
                    <asp:UpdatePanel ID="upExport" runat="server">
                        <ContentTemplate>
                            <td colspan="2" style="text-align: left; width: 65%;">
                                <asp:Button ID="btnGo" runat="server" OnClientClick="return Validation();" CssClass="btn btngo"
                                     ToolTip="Go" Style="margin-left: 10px; vertical-align: top;" TabIndex="3" />
                                <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" Text="Cancel" ToolTip="Cancel"
                                     Style="margin-left: 5px; vertical-align: top;" TabIndex="4" />
                                <asp:Button ID="btnExport" runat="server" CssClass="btn btnexcel"  ToolTip="Export To Excel"
                                     Style="display: none;" TabIndex="5" />
                                <%--<asp:ImageButton ID="btnExport" ImageUrl="~/images/ExportToExcel.png" runat="server" ToolTip="Export To Excel" Style="display: none;" TabIndex="5" />--%>
                            </td>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnExport" />
                        </Triggers>
                    </asp:UpdatePanel>
                </tr>
                <tr>
                    <td colspan="3" style="height: 10px;"></td>
                </tr>
                <tr>
                    <td colspan="3" align="center">
                        <div style="overflow: auto; height: 450px; margin: auto; width: 100%;">
                            <asp:GridView ID="gvActivityCount" runat="server" AutoGenerateColumns="true" ShowFooter="True"
                                CellPadding="3" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Height="20px" ForeColor="White" />
                                <RowStyle ForeColor="#333333" BackColor="#F7F6F3" Font-Bold="true" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <FooterStyle BackColor="#1560A1" ForeColor="White" Font-Bold="True" HorizontalAlign="Center" />
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            </asp:GridView>
                        </div>
                    </td>

                </tr>
                <tr>
                    <td colspan="3" style="height: 10px;"></td>
                </tr>

            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript" language="javascript">

        jQuery(window).focus(function () {
            ChangeColor();
        });

        function ChangeColor() {
            if ($("#ctl00_CPHLAMBDA_gvActivityCount").length > 0) {
                $("#ctl00_CPHLAMBDA_gvActivityCount tr").last().find("td").css({ 'color': 'white !important' });
            }
        }

        function ClientPopulated(sender, e) {
            $get('<%= chkParent.ClientID%>').parentElement.style.display = "none";
            RemoveGrid();
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {

            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        function Validation() {

            if (document.getElementById('<%= HProjectId.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Project !');
                return false;
            }

            return true;
        }

        function RemoveGrid() {
            if ($('#<%=gvActivityCount.ClientID%>').length > 0) $('#<%=gvActivityCount.ClientID%>').remove();
            $get('<%= btnExport.ClientID%>').style.display = "none";
        }
    </script>
</asp:Content>

