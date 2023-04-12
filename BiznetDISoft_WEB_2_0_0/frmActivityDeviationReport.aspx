<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmActivityDeviationReport.aspx.vb" Inherits="frmActivityDeviationReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <asp:UpdatePanel runat="server" ID="UpControls" UpdateMode="Conditional">
        <ContentTemplate>
            <table style="width: 100%;" cellpadding="2">
                <tr>
                    <td class="Label" style="width: 35%; white-space: nowrap; text-align: right;">
                        <asp:Label ID="lblProject" runat="server" Text="Project:" CssClass="Label "></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="70%" TabIndex="1">
                        </asp:TextBox>
                        <asp:Button Style="display: none" ID="btnSetProject" runat="server" Text=" Project">
                        </asp:Button>
                        <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                            TargetControlID="txtProject" ServicePath="AutoComplete.asmx" OnClientShowing="ClientPopulated"
                            OnClientItemSelected="OnSelected" MinimumPrefixLength="1" CompletionListItemCssClass="autocomplete_listitem"
                            CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem" CompletionListCssClass="autocomplete_list"
                            BehaviorID="AutoCompleteExtender1" CompletionListElementID="pnlProjectList">
                        </cc1:AutoCompleteExtender>
                        <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto;
                            overflow-x: hidden" />
                    </td>
                </tr>
                <tr>
                    <td class="Label" style="white-space: nowrap; text-align: right;">
                        Period:
                    </td>
                    <td style="text-align: left;">
                        <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="dropDownList" Width="40%"
                            AutoPostBack="True" TabIndex="2">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button runat="server" ID="BtnSearch" Text="Search" ToolTip="Search" CssClass="btn btnnew"
                            OnClientClick="return verify();" />
                        <asp:Button ID="btnExit" runat="server" CssClass="btn btnexit" ToolTip="Exit" OnClientClick="Redirect();"
                            Text="Exit" />
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pheader" runat="server">
                <div id="divExpandable" runat="server" style="font-weight: bold; font-size: 13px;
                    float: none; color: white; background-color: #1560a1; width: 80%; margin: auto;
                    display: none;">
                    <table width="100%">
                        <tr>
                            <td style="text-align: center; width: 93%;">
                                <asp:Label ID="Label1" runat="server" Text="Advance Search">
                                </asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:Label ID="lblMessage" runat="server" Text="Label">
                                </asp:Label>
                                <asp:Image ID="imgArrows" runat="server" Style="float: right;" />
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
            <asp:Panel ID="pbody" runat="server">
                <table style="width: 80%; margin: auto; padding-top: 1%;" cellpadding="2" class="FieldSetBox">
                    <tr>
                        <td class="Label" style="width: 35%; white-space: nowrap; text-align: right;">
                            Activity:
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList runat="server" ID="ddlActivity" CssClass="dropDownList" Width="40%"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="Label" style="text-align: right;">
                            Subject:
                        </td>
                        <td style="text-align: left;">
                            <asp:DropDownList runat="server" ID="ddlSubject" CssClass="dropDownList" Width="40%"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;" class="Label" colspan="2">
                            <asp:Button ID="Btn_SearchAll" runat="server" CssClass="btn btnnew" ToolTip="Search All"
                                Text="Search All" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="pbody"
                CollapseControlID="pheader" ExpandControlID="pheader" Collapsed="true" CollapsedSize="0"
                CollapsedImage="~/images/expand_blue.jpg" ExpandedImage="~/images/collapse_blue.jpg"
                CollapsedText="Show" ExpandedText="Hide" ImageControlID="imgArrows" TextLabelID="lblMessage">
            </cc1:CollapsiblePanelExtender>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="Btn_SearchAll" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upGridView" runat="server">
        <ContentTemplate>
            <div style="margin: auto; width: 80%; margin-top: 1%; display: none; overflow: auto;
                max-height: 300px;" id="divGrid" runat="server" class="FieldSetBox">
                <asp:GridView ID="GvDeviation" runat="server" SkinID="grdViewSmlAutoSize" AutoGenerateColumns="false"
                    Style="width: 100%;">
                    <Columns>
                        <asp:BoundField HeaderText="Project No" DataField="vProjectNo" />
                        <asp:BoundField HeaderText="Period" DataField="iPeriod" />
                        <asp:BoundField HeaderText="Subject Id" DataField="vSubjectId" />
                        <asp:BoundField HeaderText="Activity Performed" DataField="vNodeDisplayName" />
                        <asp:BoundField HeaderText="PendingNodes" DataField="vPendingNodes" />
                        <asp:BoundField HeaderText="Subject Id" DataField="vMySubjectNo" />
                        <asp:TemplateField HeaderText="Activity Deviated">
                            <ItemTemplate>
                                <img id="ShowPendingActivity" runat="server" alt="img" src="~/images/Plus.gif" />
                                <%--  <asp:ImageButton runat="server" ID="ShowPendingActivity" ImageUrl="~/images/Plus.gif"  />--%>
                                <asp:Panel ID="pnlGrid" runat="server" BorderColor="Black" ScrollBars="Auto" Style="max-height: 100px;">
                                    <asp:GridView runat="server" AutoGenerateColumns="false" ID="GvPendingNodes" ShowFooter="false"
                                        ShowHeader="false" AllowSorting="True" CellPadding="3" BackColor="White" Width="400px"
                                        BorderStyle="Solid" BorderColor="#1560a1" BorderWidth="1px">
                                        <%-- <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Height="20px" ForeColor="White" />--%>
                                        <Columns>
                                            <asp:BoundField DataField="vNodeDisplayName" />
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Remarks" DataField="vRemarks" />
                        <asp:BoundField HeaderText="User" DataField="vDataEntryUser" />
                        <asp:BoundField HeaderText="Date" ItemStyle-Width="11%"  DataField="dModifiedDate" DataFormatString="{0:dd-MMM-yyyy}" />
                        <asp:BoundField HeaderText="iNodeId" DataField="iNodeId" />
                    </Columns>
                </asp:GridView>
                <asp:Button ID="btnExportToExcel" runat="server" ToolTip="Export To Excel"
                    CssClass="btn btnexcel " Style="display: none;" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportToExcel" />
            <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="Btn_SearchAll" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" src="Script/Gridview.js"></script>

    <script type="text/javascript" src="Script/General.js"></script>

    <script type="text/javascript" src="Script/jquery.min.js"></script>

    <script type="text/javascript" language="javascript">

        function Redirect() {
            msgConfirmDeleteAlert(null, "Are you sure want to Exit ?", function (isConfirmed) {
                if (isConfirmed) {
                    var parWin = window.opener;
                    if (parWin != null && typeof (parWin) != 'undefined') {
                        if (parWin && parWin.open && !parWin.closed) {
                            window.parent.document.location.reload();
                        }
                        self.close();
                    }
                    else {
                        window.location.href = "frmMainPage.aspx?mode=1";
                    }
                    return true;
                } else {

                    return false;
                }
            });
            return false;

        }
        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {


            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
        $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));

        }
        function ShowHideGrid(id) {
            msgalert(id);
            return false;
        }

        function Show(ctrl) {
            var id = ctrl.id;
            var i = 0;
            id = id.replace('_ShowPendingActivity', '');


            if (ctrl.checked == true) {
                if (document.getElementById(id + '_GvPendingNodes') == null) {
                    msgalert('No Data Found !');
                    ctrl.checked = false;
                    return;
                }
                i = document.getElementById(id + '_GvPendingNodes').rows.length;
                document.getElementById(id + '_GvPendingNodes').style.display = '';

                if ((i * 25) < 250) {
                    document.getElementById(id + '_pnlGrid').style.height = 25 * i + 'px';
                }
                else {
                    document.getElementById(id + '_pnlGrid').style.height = '250px';
                }
            }
            else {
                document.getElementById(id + '_GvPendingNodes').style.display = 'none';

                document.getElementById(id + '_pnlGrid').style.height = '0px';
            }

        }

        function chk_Status(imgid, gridid) {
            var str = imgid.src;
            var result = str.toString().split("/");
            var i = result.length - 1;
            if (result[i].toString() == "Plus.gif") {
                gridid.style.display = '';
                str = str.replace("Plus", "minus");
                imgid.src = str.toString();

            }
            if (result[i].toString() == "minus.gif") {
                gridid.style.display = 'none';
                str = str.replace("minus", "Plus");
                imgid.src = str.toString();
            }

            return true;
        }
        function verify() {
            if ($('#ctl00_CPHLAMBDA_ddlPeriod').val() > 0) {
                return true;
            }
            else if (document.getElementById('<%= HProjectId.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Project !');
                return false;
            }
            else {
                msgalert("Please Select Period !");
                return false;
            }
        }
       

    </script>

</asp:Content>
