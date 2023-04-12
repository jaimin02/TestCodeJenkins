<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmRptDataDefinationReport, App_Web_ybumpksz" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <style type="text/css">
        .ui-state-default a, .ui-state-default a:link, .ui-state-default a:visited {
            color: white !important;
        }
         .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }
    </style>
    <%--  <script type="text/javascript" src="DataTable/css/demo_table_jui.css"></script>

    <script type="text/javascript" src="DataTable/css/demo_table.css"></script>

    <script type="text/javascript" src="DataTable/css/demo_page.css"></script>--%>

    <%-- <script type="text/javascript" src="Script/jquery-1.4.3.min.js"></script>--%>

    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>

    <%--  <script type="text/javascript" src="DataTable/js/jquery.dataTables.js"></script>--%>

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" language="javascript">




        $(document).ready(function () {
            $('#test').dataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers"

            });
        });

        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%=txtProject.ClientId%>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%=txtProject.clientid%>'),
            $get('<%=HProjectId.clientid%>'), document.getElementById('<%=btnSetProject.ClientId%>'));
        }

        function Validation() {

            if (document.getElementById('<%= HProjectId.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Project !');
                return false;
            }

            var chklst = document.getElementById('<%=chklstColumns.clientid%>');
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
                msgalert('Please Select Atleast One Field !');
                return false;
            }



            return true;
        }

        function SelectAllFields() {
            var chkSelectAll = document.getElementById('<%=chkSelectAllFields.clientid%>').checked;
            var chklst = document.getElementById('<%=chklstColumns.clientid%>');
            var chks;
            var result = false;
            var i;
            if (chklst != null && typeof (chklst) != 'undefined') {
                chks = chklst.getElementsByTagName('input');
                if (chkSelectAll == true) {
                    for (i = 0; i < chks.length; i++) {
                        chks[i].checked = true;
                    }
                }
                else {
                    for (i = 0; i < chks.length; i++) {
                        chks[i].checked = false;
                    }
                }
            }
            return false;
        }
        function ReportUncheck() {
            $('#ctl00_CPHLAMBDA_chklstColumns input[type=checkbox]').each(function () {
                this.checked = false;
            });

            $('#ctl00_CPHLAMBDA_chkSelectAllFields')[0].checked = false;
        }
        function pageLoad() {
            //  CreateGridHeader();
        }


        function UIGV_RptDefination() {
            $('#<%= gvwActivityGrid.ClientID%>').removeAttr('style', 'display:block');
            oTab = $('#<%= gvwActivityGrid.ClientID%>').prepend($('<thead>').append($('#<%= gvwActivityGrid.ClientID%> tr:first'))).dataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": false,
                "sScrollY": "250px",
                "sScrollX": "100%",
                "bScrollCollapse": true,
                aLengthMenu: [
                    [10, 25, 50, 100, -1],
                    [10, 25, 50, 100, "All"]
                ],
            });



            return false;
        }

    </script>

    <div style="width: 100%">
        <table style="width: 100%;">
            <tr>
                <td>
                    <table style="width: 100%" cellpadding="3px">
                        <tr>
                            <td class="Label" style="width: 30%; text-align: right">Project* :
                                <br />
                            </td>
                            <td class="Label" style="text-align: left; white-space: nowrap;">
                                <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="70%"></asp:TextBox><asp:Button
                                    ID="btnSetProject" runat="server" OnClientClick="ReportUncheck();" OnClick="btnSetProject_Click"
                                    Style="display: none" Text=" Project" /><asp:HiddenField ID="HProjectId" runat="server" />
                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                    CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                    CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                    OnClientShowing="ClientPopulated" ServiceMethod="GetProjectCompletionListWithOutSponser"
                                    ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True"
                                    CompletionListElementID="pnlProjectList">
                                </cc1:AutoCompleteExtender>
                                <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" />
                            </td>
                        </tr>
                        <tr>
                            <td class="Label" style="text-align: right; vertical-align: bottom;">Select Fields* :
                            </td>
                            <td style="text-align: left">
                                <asp:CheckBox ID="chkSelectAllFields" runat="server" Text="Select All" onClick="SelectAllFields()" /><asp:Panel
                                    ID="pnlFields" runat="server" ScrollBars="Auto" BorderColor="#184E8A" BorderWidth="2px"
                                    Style="max-height: 120px; max-width: 70%; min-height: 15px">
                                    <asp:CheckBoxList ID="chklstColumns" runat="server" RepeatColumns="3" RepeatDirection="Horizontal">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="Label" style="text-align: center">
                                <asp:Button ID="btnGenerate" runat="server" CssClass="btn btngo" Text="" ToolTip="Go"
                                    OnClientClick="return Validation();" />
                                <asp:Button ID="btnExportGrid" OnClick="btnExportGrid_Click" Visible="false" runat="server"
                                    ToolTip="Export To Excel" CssClass="btn btnexcel"></asp:Button>
                                <asp:Label ID="LblgridActivity" Visible="false" runat="server" Text="No Records Found "></asp:Label>
                                <asp:Button ID="btnSearch" Visible="false" runat="server" CssClass="btn btnnew" OnClick="btnSearch_Click"
                                    Text="Search" OnClientClick="return Validation();" />
                                <asp:Button ID="btnExit" runat="server" Text="Exit" ToolTip="Exit" CssClass="btn btnexit"
                                    CausesValidation="False" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this); "></asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td class="Label" colspan="2">
                                <asp:Panel ID="pnlPlaceSearchOptions" runat="server" Width="100%" ScrollBars="Auto"
                                    CssClass="Label">
                                    <asp:PlaceHolder ID="PlaceSearchOptions" runat="server" EnableViewState="true"></asp:PlaceHolder>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table style="width: 100%">
            <tbody>
                <tr>
                    <td>
                      
                        <div style="max-width: 1000px; margin-left: 17%; height: 315px;"
                            id="div_Data" runat="server">

                         
                            <asp:GridView ID="gvwActivityGrid" runat="server" Style="width: 100%; margin: auto; display: none;"
                                OnPageIndexChanging="gvwActivityGrid_PageIndexChanging"
                                AutoGenerateColumns="False" AllowSorting="True" EmptyDataText="No records Found">
                                <Columns>
                                     <asp:BoundField HeaderText=" # " />
                                    <asp:BoundField DataField="vProjectNo" HeaderText="Project No" SortExpression="vProjectNo" />
                                    <asp:BoundField DataField="ActivityGroupName" HeaderText="Activity Group Name" SortExpression="ActivityGroupName" />
                                    <asp:BoundField DataField="Visit/ParentActivity" HeaderText="Visit/Parent Activity" SortExpression="Visit/ParentActivity" />
                                    <asp:BoundField DataField="AttributeGroupName" HeaderText="Attribute Group Name" SortExpression="AttributeGroupName" />
                                    <asp:BoundField DataField="AttributeSubGroupName" HeaderText="Attribute SubGroup Name" SortExpression="AttributeSubGroupName" />
                                    <asp:BoundField DataField="AttributeDesc" HeaderText="Attribute Description" SortExpression="AttributeDesc" />
                                    <asp:BoundField DataField="AttributeType" HeaderText="Attribute Type" SortExpression="AttributeType" />
                                    <asp:BoundField DataField="AttributeValue" HeaderText="Attribute Value" SortExpression="AttributeValue" />
                                    <asp:BoundField DataField="CDISCValues" HeaderText="CDISC Values" SortExpression="CDISCValues" />
                                    <asp:BoundField DataField="UOM" HeaderText="UOM" SortExpression="UOM" />
                                    <asp:BoundField DataField="AttributeLength" HeaderText="Attribute length" SortExpression="AttributeLength" />
                                    <asp:BoundField DataField="ValidationType" HeaderText="Validation Type" SortExpression="ValidationType" />
                                    <asp:BoundField DataField="AlertMessage" HeaderText="Alert Message" SortExpression="AlertMessage" />
                                    <asp:BoundField DataField="DefaultValue" HeaderText="Default Value" SortExpression="DefaultValue" />
                                    <asp:BoundField DataField="AlertValue" HeaderText="Alert Value" SortExpression="AlertValue" />
                                    <asp:BoundField DataField="LowRange" HeaderText="Low Range" SortExpression="LowRange" />
                                    <asp:BoundField DataField="HighRange" HeaderText="High Range" SortExpression="HighRange" />
                                    <asp:BoundField DataField="NOTNULL" HeaderText="Not Null" SortExpression="NOTNULL" />
                                </Columns>
                            </asp:GridView>

                        </div>
                       
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
