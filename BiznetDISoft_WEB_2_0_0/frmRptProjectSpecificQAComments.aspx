<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmRptProjectSpecificQAComments.aspx.vb" Inherits="frmRptProjectSpecificQAComments" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <%--<script type="text/javascript" language ="javascript" src ="Script/popcalendarDiv.js"></script>--%>

    <script type="text/javascript" language="javascript" src="Script/General.js"></script>

    <script type="text/javascript" language="javascript" src="Script/Validation.js"></script>

    <script type="text/javascript" src="Script/popcalendar.js"></script>

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
            if ((document.getElementById('<%=txtProject.clientid%>').value == '' || document.getElementById('<%=HProjectId.clientid%>').value == '')) {
                msgalert('Please Select Project !');
                return false;
            }
            else if ((document.getElementById('<%=DDLActivity.clientid%>').selectedIndex == 0) && (!(chklst.checked))) {
                msgalert('Please Select Activity Or Select All !');
                return false;
            }
            //        else if (document.getElementById('<%=txtFromDate.ClientID%>').value.toString().trim().length <= 0 ||document.getElementById('<%=txtToDate.ClientID%>').value.toString().trim().length <= 0)
            //       {
            //          alert('Please Enter Date');
            //          document.getElementById('<%=txtFromDate.ClientID%>').focus();
            //          return false;
            //       }
            else if (CompareDate(document.getElementById('<%=txtFromDate.ClientID%>').value, document.getElementById('<%=txtToDate.ClientID%>').value) != true) {
                return false;

            }

            return true;

        }

        function popcalender(obj) {
            var txt = document.getElementById('<%=txtFromDate.clientid %>');
            popUpCalendar(obj, txt, 'dd-mmm-yy');
            document.getElementById('<%=chkAllDate.clientid %>').checked = false;
        }

        function Onddlchange() {
            document.getElementById('<%=chkAll.clientid %>').checked = false;
        }
    
    </script>

    <table style="width: 100%">
        <tr>
            <td align="center" style="width: 100%">
                <table align="left" style="width: 100%">
                    <tr>
                        <td class="Label" style="width: 30%; text-align: right">
                            Project:
                        </td>
                        <td class="Label" style="text-align: left; white-space: nowrap;">
                            <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="70%"></asp:TextBox>
                            <asp:Button ID="btnSetProject" runat="server" OnClick="btnSetProject_Click" Style="display: none"
                                Text=" Project" /><asp:HiddenField ID="HProjectId" runat="server" />
                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                OnClientShowing="ClientPopulated" ServiceMethod="GetProjectCompletionListWithOutSponser"
                                ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True"
                                CompletionListElementID="pnlProjectList">
                            </cc1:AutoCompleteExtender>
                            <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto;
                                overflow-x: hidden" />
                            <input id="HProjectNo" runat="server" type="hidden" />
                        </td>
                    </tr>
                    <tr>
                        <%-- <td class="Label" style=" text-align: right" align="left" visible ="false" >
                            
                        </td>--%>
                        <td style="text-align: center" colspan="2">
                            <asp:RadioButtonList ID="RblCommentsOn" runat="server" RepeatDirection="Horizontal"
                                Style="margin: auto;" Visible="true" AutoPostBack="True">
                                <asp:ListItem Selected="True" Value="SOURCE">Source Review</asp:ListItem>
                                <asp:ListItem Value="DOCUMENT">Document</asp:ListItem>
                                <asp:ListItem Value="INPROCESS">InProcess</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr style="display: none" id="trActivity" runat="server">
                        <td class="Label" style="text-align: right">
                            Activity:
                        </td>
                        <td style="text-align: left; white-space: nowrap;">
                            <asp:DropDownList ID="DDLActivity" runat="server" onchange="Onddlchange();" CssClass="dropDownList"
                                Width="560px">
                            </asp:DropDownList>
                            <asp:CheckBox ID="chkAll" runat="server" Checked="true" Text="All" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            <strong class="Label">QA Date : </strong>
                        </td>
                        <td style="text-align: left">
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="textBox" Width="30%"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate"  Format="dd-MMM-yyyy"></cc1:CalendarExtender>
                            To :
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="textBox" TabIndex="1" Width="30%"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"  Format="dd-MMM-yyyy"></cc1:CalendarExtender>
                            <asp:CheckBox ID="ChkAllDate" runat="server"
                                    Text="All" Checked="true" Width="47px" />
                        </td>
                    </tr>
                    <tr>
                        <%-- <td style="width: 30%; text-align: right" align="left">
                        </td>--%>
                        <td class="Label" style="text-align: center" colspan="2">
                            <asp:Button ID="btnGenerate" runat="server" CssClass="btn btnsave" ToolTip="Generate Report"
                                Text="Generate Report" OnClientClick="return Validation();" />
                            <asp:Button ID="btnExit" runat="server" CssClass="btn btnexit" ToolTip="Exit" Text="Exit"
                                OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
