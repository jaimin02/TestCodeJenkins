<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmRptQAComments, App_Web_22suyskz" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" language="javascript" src="Script/popcalendar.js"></script>

    <script type="text/javascript" language="javascript" src="Script/General.js"></script>

    <script type="text/javascript" language="javascript" src="Script/Validation.js"></script>
     
    <script type="text/javascript" language="javascript">
        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%=txtProject.ClientId%>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%=txtProject.clientid%>'),
            $get('<%=HProjectId.clientid%>'), document.getElementById('<%=btnSetProject.ClientId%>'));
        }

        function Validation() {
            var chklst = document.getElementById('<%=chkAll.clientid%>');
            if ((document.getElementById('<%=txtProject.clientid%>').value == '' || document.getElementById('<%=HProjectId.clientid%>').value == '') && (!(chklst.checked))) {
                msgalert('Please Select Project Or Select All !');
                return false;
            }
            else if (document.getElementById('<%=txtFromDate.ClientID%>').value.toString().trim().length <= 0 || document.getElementById('<%=txtToDate.ClientID%>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Date !');
                document.getElementById('<%=txtFromDate.ClientID%>').focus();
                return false;
            }
            else if (CompareDate(document.getElementById('<%=txtFromDate.ClientID%>').value, document.getElementById('<%=txtToDate.ClientID%>').value) != true) {
                return false;

            }
            return true;
        }

    </script>

    <table style="width: 100%">
        <tr>
            <td>
                <table style="width: 100%" cellpadding="3px">
                    <tr>
                        <td class="Label" style="width: 30%; text-align: right">
                            Comments Given on:
                        </td>
                        <td class="Label" align="left">
                            <asp:RadioButtonList ID="RblCommentsOn" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True">PIF</asp:ListItem>
                                <asp:ListItem>MSR</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="Label" style="text-align: right">
                            Project:
                        </td>
                        <td class="Label" style="text-align: left; white-space: nowrap;">
                            <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="70%"></asp:TextBox><asp:CheckBox
                                ID="chkAll" runat="server" Text="All" /><asp:Button ID="btnSetProject" runat="server"
                                    OnClick="btnSetProject_Click" Style="display: none" Text=" Project" /><asp:HiddenField
                                        ID="HProjectId" runat="server" />
                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                OnClientShowing="ClientPopulated" ServiceMethod="GetProjectCompletionListWithOutSponser"
                                ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True"
                                CompletionListElementID="pnlProjectList">
                            </cc1:AutoCompleteExtender>
                            <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto;
                                overflow-x: hidden" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <strong class="Label">QC Date : </strong>
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="textBox" Width="25%"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate"  Format="dd-MMM-yyyy"></cc1:CalendarExtender>
                            <%--<img id="ImgFromDate" alt="Select From Date" onclick="popUpCalendar(this,ctl00_CPHLAMBDA_txtFromDate,'dd-mmm-yy');"
                                src="images/Calendar_scheduleHS.png" />--%>
                            To :
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="textBox" TabIndex="1" Width="25%"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"  Format="dd-MMM-yyyy"></cc1:CalendarExtender>
                            <%--<img id="ImgToDate" alt="Select To Date" onclick="popUpCalendar(this,ctl00_CPHLAMBDA_txtToDate,'dd-mmm-yy');"
                                src="images/Calendar_scheduleHS.png" />--%>
                        </td>
                    </tr>
                    <tr>
                        <%-- <td style="width: 30%; text-align: right; height: 21px;" align="left">
                        </td>--%>
                        <td class="Label" style="text-align: center;" colspan="2">
                            <asp:Button ID="btnGenerate" runat="server" CssClass="btn btnsave" Text="Generate Report"
                                ToolTip="Generate Report" OnClientClick="return Validation();" />
                            <asp:Button ID="btnExit" runat="server" CssClass="btn btnexit" Text="Exit" ToolTip="Exit"
                                OnClientClick="return msgconfirmalert('Are You Sure You Want to Exit?',this); " />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
