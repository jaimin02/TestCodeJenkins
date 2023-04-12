<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmRptSubjectPIFAuditTrail.aspx.vb" Inherits="frmRptSubjectPIFAuditTrail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" language="javascript" src="Script/popcalendar.js"></script>

    <script type="text/javascript" language="javascript" src="Script/General.js"></script>

    <script type="text/javascript" language="javascript">

        ////// For Project    
        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%=txtProject.ClientId%>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%=txtProject.clientid%>'),
            $get('<%=HProjectId.clientid%>'), document.getElementById('<%=btnSetProject.ClientId%>'));
        }

        ///////////For Subject  
        function ClientPopulatedSubject(sender, e) {
            SubjectClientShowing('AutoCompleteExtender2', $get('<%= txtSubject.ClientId %>'));
        }

        function OnSelectedSubject(sender, e) {
            SubjectOnItemSelected(e.get_value(), $get('<%= txtSubject.clientid %>'),
                $get('<%= HSubjectId.clientid %>'), document.getElementById('<%= btnSubject.ClientId %>'));
        }


        function Validation() {
            var chkAl = document.getElementById('<%=chkAll.ClientID%>')
            if ((document.getElementById('<%=txtSubject.clientid%>').value == '' || document.getElementById('<%=HSubjectId.clientid%>').value == '')) {
                msgalert('Please Select Subject !');
                return false;
            }
            if (document.getElementById('<%=txtFromDate.ClientID%>').value.toString().trim().length <= 0 && document.getElementById('<%=txtToDate.ClientID%>').value.toString().trim().length <= 0) {
                if (!chkAl.checked) {
                    msgalert("Please Select Date OR Check All !");
                    return false;
                }

            }
            return true;
        }

    </script>

    <asp:UpdatePanel ID="upPIFAudit" runat="server">
        <ContentTemplate>
            <table style="width: 100%" cellpadding="5px">
                <tbody>
                    <tr>
                        <td style="width: 100%;">
                            <table style="width: 100%">
                                <tbody>
                                    <tr>
                                        <td style="text-align: right; width: 20%" class="Label">
                                            Project:
                                        </td>
                                        <td style="text-align: left" colspan="2">
                                            <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="60%" TabIndex="1"></asp:TextBox>
                                            <asp:Button Style="display: none" ID="btnSetProject" OnClick="btnSetProject_Click"
                                                runat="server" Text=" Project"></asp:Button><asp:HiddenField ID="HProjectId" runat="server">
                                                </asp:HiddenField>
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                                CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                                OnClientShowing="ClientPopulated" ServiceMethod="GetProjectCompletionListWithOutSponser"
                                                ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True"
                                                CompletionListElementID="pnlProjectList">
                                            </cc1:AutoCompleteExtender>
                                            <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto;
                                                overflow-x: hidden" />
                                            <input id="HProjectNo" type="hidden" runat="server" />
                                        </td>
                                        <%-- <td style=" text-align: left" class="Label">
                                        </td>--%>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap; text-align: right;" class="Label">
                                            Period:
                                        </td>
                                        <td style="white-space: nowrap; text-align: left;">
                                            <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="dropDownList" Width="50%"
                                                AutoPostBack="True" TabIndex="2">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="white-space: nowrap; width: 30%;" rowspan="4">
                                            <asp:Image ID="Image1" runat="server" Width="50%" Height="80%"></asp:Image>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right" class="Label">
                                            Subject:
                                        </td>
                                        <td style="white-space: nowrap; text-align: left">
                                            <asp:TextBox ID="txtSubject" TabIndex="3" runat="server" CssClass="textBox" Width="80%"></asp:TextBox><asp:Button
                                                Style="display: none" ID="btnSubject" OnClick="btnSubject_Click" runat="server"
                                                Text="Subject"></asp:Button><asp:HiddenField ID="HSubjectId" runat="server">
                                            </asp:HiddenField>
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" ServiceMethod="GetSubjectCompletionList_Dynamic"
                                                BehaviorID="AutoCompleteExtender2" MinimumPrefixLength="1" OnClientItemSelected="OnSelectedSubject"
                                                OnClientShowing="ClientPopulatedSubject" ServicePath="AutoComplete.asmx" TargetControlID="txtSubject"
                                                UseContextKey="True">
                                            </cc1:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            <strong class="Label">Date: </strong>
                                        </td>
                                        <td style="text-align: left">
                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="textBox" Width="35%" TabIndex="4"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate"  Format="dd-MMM-yyyy"></cc1:CalendarExtender>
                                            To :
                                            <asp:TextBox ID="txtToDate" TabIndex="5" runat="server" CssClass="textBox" Width="35%"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate"  Format="dd-MMM-yyyy"></cc1:CalendarExtender>
                                            &nbsp;<asp:CheckBox ID="chkAll"
                                                    runat="server" Text="All" TabIndex="6"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%-- <td style="width: 10%; height: 21px; text-align: right" align="left">
                                        </td>--%>
                                        <td style="text-align: center" class="Label" colspan="3">
                                            <asp:Button ID="btnGenerate" runat="server" Text="Generate Report" ToolTip="Generate Report"
                                                CssClass="btn btnsave" OnClientClick="return Validation();" TabIndex="7">
                                            </asp:Button>
                                            <asp:Button ID="btnCancel" TabIndex="8" OnClick="btnCancel_Click" runat="server"
                                                Text="Cancel" ToolTip="Cancel" CssClass="btn btncancel" CausesValidation="True"></asp:Button>
                                            <asp:Button ID="btnExit" runat="server" Text="Exit" ToolTip="Exit" CssClass="btn btnexit"
                                                OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this); " TabIndex="9">
                                            </asp:Button>
                                        </td>
                                        <%--<td style="width: 30%; height: 21px; text-align: left" class="Label">
                                        </td>--%>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnGenerate"></asp:PostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
