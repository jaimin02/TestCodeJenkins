<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmCrossActivityReport.aspx.vb" Inherits="frmCrossActivityReport" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <script src="Script/jquery.dataTables.min.js" type="text/javascript"></script>

    <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }
    </style>


    <table style="width: 100%" cellpadding="5px">
        <tr>
            <td>
                <asp:UpdatePanel ID="upcontrols" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%" cellpadding="5px">
                            <tr>
                                <td class="Label" nowrap="nowrap" style="text-align: right; width: 18%">
                                    Project Name/Request Id :
                                </td>
                                <td class="Label" style="text-align: left">
                                    <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="80%"></asp:TextBox><asp:Button
                                        Style="display: none" ID="btnSetProject" runat="server" Text=" Project"></asp:Button>
                                    <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                        CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                        CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                        OnClientShowing="ClientPopulated" ServiceMethod="GetMyProjectCompletionList"
                                        ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True"
                                        CompletionListElementID="pnlProjectList">
                                    </cc1:AutoCompleteExtender>
                                    <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto;
                                        overflow-x: hidden" />
                                </td>
                            </tr>
                            <tr runat="server" id="trsubGen">
                                <td class="Label" style="text-align: right">
                                    Activity Type :
                                </td>
                                <td class="Label" style="text-align: left">
                                    <asp:DropDownList ID="DdlActivityType1" CssClass="dropDownList" runat="server" Width="38%"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                    &lt;&gt;
                                    <asp:DropDownList ID="DdlActivityType2" CssClass="dropDownList" runat="server" Width="38%"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr runat="server" id="trPeriod" style="display: none">
                                <td class="Label" style="text-align: right">
                                    Period :
                                </td>
                                <td class="Label" style="text-align: left">
                                    <asp:DropDownList ID="ddlperiod1" CssClass="dropDownList" runat="server" Width="38%"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                    &lt;&gt;
                                    <asp:DropDownList ID="Ddlperiod2" CssClass="dropDownList" runat="server" Width="38%"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="Label" nowrap="nowrap" style="text-align: right;">
                                </td>
                                <td class="Label" style="text-align: left;">
                                    <table width="100%;">
                                        <tr>
                                            <td style="width: 40%;">
                                                <asp:CheckBox ID="chkSelectAll" runat="server" Text="Select All" Enabled="false"
                                                    onClick="SelectAllSubjects()" />
                                            </td>
                                            <%-- <td style="width: 15px;">
                                            </td>--%>
                                            <td>
                                                <asp:CheckBox ID="chkSelectAll1" runat="server" Text="Select All" Enabled="false"
                                                    onClick="SelectAllSubjects1()" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trsubject" runat="server">
                                <td id="lblsubject" class="Label" style="text-align: right; white-space: nowrap;">
                                    Subject :
                                </td>
                                <td class="Label" style="text-align: left;">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 40%;">
                                                <asp:Panel ID="pnlSubjects" runat="server" BackColor="White" BorderColor="Navy" BorderWidth="1px"
                                                    Height="75px" ScrollBars="Auto" Width="95%">
                                                    <asp:CheckBoxList ID="chkLstSubjects" runat="server" RepeatColumns="2" RepeatDirection="Horizontal">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                            </td>
                                            <%-- <td style="width: 5px;">
                                              
                                            </td>--%>
                                            <td>
                                                <asp:Panel ID="pnlSubjects1" runat="server" BackColor="White" BorderColor="Navy"
                                                    BorderWidth="1px" Height="75px" ScrollBars="Auto" Width="65%">
                                                    <asp:CheckBoxList ID="ChklstSubjects1" runat="server" RepeatColumns="2" RepeatDirection="Horizontal">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trvisit" runat="server">
                                <td class="Label" style="text-align: right">
                                    Visit/Parent activity :
                                </td>
                                <td class="Label" style="text-align: left">
                                    <asp:DropDownList ID="ddlVisit" CssClass="dropDownList" runat="server" Width="38%"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                    &lt;&gt;
                                    <asp:DropDownList ID="ddlCrossVisit" CssClass="dropDownList" runat="server" Width="38%"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="Label" style="text-align: right">
                                    Activity :
                                </td>
                                <td class="Label" style="text-align: left">
                                    <asp:DropDownList ID="ddlActivity" CssClass="dropDownList" runat="server" Width="38%">
                                    </asp:DropDownList>
                                    &lt;&gt;
                                    <asp:DropDownList ID="ddlCrossActivity" CssClass="dropDownList" runat="server" Width="38%">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <%-- <td class="Label">
                                </td>--%>
                                <td style="text-align: center;" colspan="2">
                                    <asp:Button ID="btnGo" runat="server" CssClass="btn btngo" Text="" ToolTip="Go" OnClientClick="return Validation();" />
                                    <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" Text="Cancel" ToolTip="Cancel" />
                                    <asp:Button ID="btnExit" runat="server" Text="Exit" ToolTip="Exit" CssClass="btn btnexit"
                                        CausesValidation="False" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);">
                                    </asp:Button>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="upGrid" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tbody>
                    <tr>
                        <td style="width: 50%;">
                            <asp:Panel ID="pnlActivityGrid" runat="server" Width="100%" Height="250px" ScrollBars="Auto">
                                <asp:GridView ID="gvwActivityGrid" runat="server" SkinID="grdViewSmlAutoSize" AutoGenerateColumns="False"
                                    ShowFooter="True">
                                    <Columns>
                                        <asp:BoundField DataField="vMySubjectNo" HeaderText="Screen No" />
                                        <asp:BoundField DataField="vMedExDesc" HeaderText="Attribute" />
                                        <asp:BoundField DataField="vMedExResult" HeaderText="Value" />
                                        <asp:BoundField DataField="iTranNo" HeaderText="Trans No" />
                                        <asp:BoundField DataField="dModifyOnSubDtl" HeaderText="Modify on" DataFormatString="{0:dd-MMM-yyyy HH:mm tt}" />
                                        <asp:BoundField DataField="CRFSubDtlChangedBy" HeaderText="Modify by" />
                                        <asp:BoundField DataField="vModificationRemark" HeaderText="Modify Remarks" />
                                        <asp:BoundField DataField="iRepeatNo" HeaderText="repetition" />
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                        <td>
                            <asp:Panel ID="pnlCrossActivityGrid" runat="server" Width="100%" Height="250px" ScrollBars="Auto">
                                <asp:GridView ID="gvwCrossActivityGrid" runat="server" SkinID="grdViewSmlAutoSize"
                                    AutoGenerateColumns="False" ShowFooter="True">
                                    <Columns>
                                        <asp:BoundField DataField="vMySubjectNo" HeaderText="Screen No" />
                                        <asp:BoundField DataField="vMedExDesc" HeaderText="Attribute" />
                                        <asp:BoundField DataField="vMedExResult" HeaderText="Value" />
                                        <asp:BoundField DataField="iTranNo" HeaderText="Trans No" />
                                        <asp:BoundField DataField="dModifyOnSubDtl" HeaderText="Modify on" DataFormatString="{0:dd-MMM-yyyy HH:mm tt}" />
                                        <asp:BoundField DataField="CRFSubDtlChangedBy" HeaderText="Modify by" />
                                        <asp:BoundField DataField="vModificationRemark" HeaderText="Modify Remarks" />
                                        <asp:BoundField DataField="iRepeatNo" HeaderText="repetition" />
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; margin: auto;">
                            <asp:Button ID="btnExportGrid" OnClick="btnExportGrid_Click" runat="server" 
                                CssClass="btn btnexcel" Visible="false" ToolTip="Export To Excel"></asp:Button>
                            <asp:Label ID="LblgridActivity" Visible="false" runat="server" Text="No Records Found "></asp:Label>
                        </td>
                        <td style="text-align: center; margin: auto;">
                            <asp:Button ID="btnExportCrossGrid" Visible="false" OnClick="btnExportCrossGrid_Click"
                                runat="server"  ToolTip="Export To Excel" CssClass="btn btnexcel"></asp:Button>
                            <asp:Label ID="lblgridCrossActivity" Visible="false" runat="server" Text="No Records Found"></asp:Label>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnExportGrid" />
            <asp:PostBackTrigger ControlID="btnExportCrossGrid" />
        </Triggers>
    </asp:UpdatePanel>

    <script type="text/javascript" language="javascript">

        function fngvwCrossActivityGrid() {

            if ($get('<%= gvwCrossActivityGrid.ClientID%>') != null && $get('<%= gvwCrossActivityGrid.ClientID%>_wrapper') == null) {
                var oTab = $('#<%= gvwCrossActivityGrid.ClientID%>').prepend($('<thead>').append($('#<%= gvwCrossActivityGrid.ClientID %> tr:first'))).dataTable({
                    "bJQueryUI": true,
                    "bPaginate": true,
                    "sPaginationType": "full_numbers",
                    "iDisplayLength": 5,
                    "bProcessing": true,
                    "bSort": false,
                    aLengthMenu: [
                        [5, 10, 15, 20, 50, -1],
                        [5, 10, 15, 20, 50, "All"]
                    ],
                });
            }
        }

        function fngvwActivityGrid() {

            if ($get('<%= gvwActivityGrid.ClientID%>') != null && $get('<%= gvwActivityGrid.ClientID%>_wrapper') == null) {
                var oTab = $('#<%= gvwActivityGrid.ClientID%>').prepend($('<thead>').append($('#<%= gvwActivityGrid.ClientID %> tr:first'))).dataTable({
                    "bJQueryUI": true,
                    "bPaginate": true,
                    "sPaginationType": "full_numbers",
                    "iDisplayLength": 5,
                    "bProcessing": true,
                    "bSort": false,
                    aLengthMenu: [
                        [5, 10, 15, 20, 50, -1],
                        [5, 10, 15, 20, 50, "All"]
                    ],
                });
            }
        }



        function ClientPopulated(sender, e) {
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
            var Typevalue1 = document.getElementById('<%=DdlActivityType1.ClientID%>').selectedIndex;
            if (Typevalue1 == 0) {
                msgalert('Please Select Activity Type !');
                return false;
            }
            if(Typevalue1 ==1)
            {
             var visitvalue1 = document.getElementById('<%=ddlvisit.ClientID%>').selectedIndex;
                if (visitvalue1 == 0 && Typevalue1 == 1) {
                    msgalert('Please Select Visit/Parent Activity !');
                return false;
                }
            }
            var Typevalue2 = document.getElementById('<%=DdlActivityType2.ClientID%>').selectedIndex;
            if (Typevalue2 == 0) {
                msgalert('Please Select Cross Activity Type !');
                return false;
            }
            if(Typevalue2 ==1)
            {
                var visitvalue2 = document.getElementById('<%=ddlcrossvisit.ClientID%>').selectedIndex;
                if (visitvalue2 == 0 && Typevalue2 == 1) {
                    msgalert('Please Select Cross Visit/Cross Parent Activity !');
                return false;
                }
            }
            var Periodvalue1 = document.getElementById('<%=ddlperiod1.ClientID%>').selectedIndex;
            if (Periodvalue1 == 0) {
                msgalert('Please Select Period !');
                return false;
            }

            var Periodvalue2 = document.getElementById('<%=ddlperiod2.ClientID%>').selectedIndex;
            if (Periodvalue2 == 0) {
                msgalert('Please Select Cross Period !');
                return false;
            }
            var Activityvalue1 = document.getElementById('<%=ddlActivity.ClientID%>').selectedIndex;
            if (Activityvalue1 == 0) {
                msgalert('Please Select Activity !');
                return false;
            }

            var Activityvalue2 = document.getElementById('<%=ddlCrossActivity.ClientID%>').selectedIndex;
            if (Activityvalue2 == 0) {
                msgalert('Please Select Cross Activity !');
                return false;
            }


            if (Typevalue1 == 1) {
                var chklst = document.getElementById('<%=chklstSubjects.clientid%>');
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
                    msgalert('Please Select Atleast One Subject For Activity Report !');
                    return false;
                }


            }
            //cross activities

            if (Typevalue2 == 1) {

                var chklstsubject = document.getElementById('<%=chklstSubjects1.clientid%>');
                var chkssubject;
                var resultsubject = false;
                var index;
                if (chklstsubject != null && typeof (chklstsubject) != 'undefined') {
                    chkssubject = chklstsubject.getElementsByTagName('input');
                    for (index = 0; index < chkssubject.length; index++) {
                        if (chkssubject[index].type.toUpperCase() == 'CHECKBOX' && chkssubject[index].checked) {
                            resultsubject = true;
                            break;
                        }
                    }
                }
                if (!resultsubject) {
                    msgalert('Please Select Atleast One Subject For Cross Activity Report !');
                    return false;
                }

            }


            return true;
        }



        function SelectAllSubjects() {
            var chkSelectAll = document.getElementById('<%=chkSelectAll.clientid%>').checked;
            var chklst = document.getElementById('<%=chkLstSubjects.clientid%>');
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
        function SelectAllSubjects1() {
            var chkSelectAll = document.getElementById('<%=chkSelectAll1.clientid%>').checked;
            var chklst = document.getElementById('<%=chklstSubjects1.clientid%>');
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
    </script>

</asp:Content>
