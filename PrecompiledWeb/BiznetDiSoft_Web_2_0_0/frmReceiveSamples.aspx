<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmReceiveSamples, App_Web_mlepfeoz" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/popcalendar.js">
    </script>

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" src="Script/Gridview.js"></script>

    <script type="text/javascript" language="javascript">

        function fsetReceiveSample_Show() {
            $('#<%=fsetReceiveSample.ClientID%>').attr('style', $('#<%=fsetReceiveSample.ClientID%>').attr('style') + ';display:block');
        }

        function Display(control, target) {
            if (control.src.toString().toUpperCase().search("EXPAND") != -1) {
                $("#" + target).slideToggle(600);
                control.src = "images/panelcollapse.png";
            }
            else {
                $("#" + target).slideToggle(600);
                control.src = "images/panelexpand.png";
            }
        }
        function Display(control, target) {
            if (control.src.toString().toUpperCase().search("EXPAND") != -1) {
                $("#" + target).slideToggle(600);
                control.src = "images/panelcollapse.png";
            }
            else {
                $("#" + target).slideToggle(600);
                control.src = "images/panelexpand.png";
            }
        }

        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        function SelectAll(CheckBoxControl, Grid) {
            if (CheckBoxControl.checked == true) {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf(Grid) > -1)) {
                        if (document.forms[0].elements[i].disabled == false) {
                            document.forms[0].elements[i].checked = true;
                        }
                    }
                }
            }
            else {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++) {
                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf(Grid) > -1)) {
                        document.forms[0].elements[i].checked = false;
                    }
                }
            }
        }

        function ValidationToReceive(gv) {
            var gvwSample = document.getElementById('<%= gvwSample.ClientID %>');

            if (gvwSample == null || typeof (gvwSample) == 'undefined') {
                return false;
            }
            else if (CheckOne(gvwSample.id) == false) {
                msgalert('Select Atleast One Sample !');
                return false;
            }
            return true;
        }
        function Validation() {

            if (document.getElementById('<%=chkAllProjects.ClientID%>').checked == false && document.getElementById('<%=txtproject.ClientID%>').value == '') {
                msgalert('Please Enter Project Name Or Select All Project !');
                return false;
            }
            return true;
        }

        function CheckUncheckAll(Grid) {
            var Checkall = document.getElementById('chkSelectAll');
            var Gvd = document.getElementById('<%=gvwSample.ClientId %>');
            j = 0;
            for (i = 0; i < document.forms[0].elements.length; i++) {
                if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf(Grid) > -1)) {
                    j = j + 1;
                    if (document.forms[0].elements[i].checked == false) {
                        Checkall.checked = false;
                        break;
                    }
                    else if (document.forms[0].elements[i].checked == true) {
                        if (j == Gvd.rows.length - 2) {
                            Checkall.checked = true;
                        }


                    }

                }

            }
        }
    </script>
    <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
        <legend class="LegendText" style="color: Black; font-size: 12px">
            <img id="img2" alt="Receive Samples Details" src="images/panelcollapse.png"
                onclick="Display(this,'divReceiveDetail');" runat="server" style="margin-right: 2px;" />Receive Samples Details</legend>
        <div id="divReceiveDetail">
            <table style="width: 100%" cellpadding="5px">
                <tbody>
                    <tr>
                        <td style="text-align: center" class="Label" colspan="2">
                            <asp:RadioButtonList ID="rblSelection" runat="server" AutoPostBack="True" RepeatDirection="Horizontal"
                                OnSelectedIndexChanged="rblSelection_SelectedIndexChanged" Width="30%" Style="margin: auto;">
                                <asp:ListItem Value="00">Screening</asp:ListItem>
                                <asp:ListItem Value="01">Project Specific</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center" align="left" colspan="2">
                            <asp:Panel ID="pnlProjectSpecific" runat="Server">
                                <table width="100%">
                                    <tbody>
                                        <tr>
                                            <td style="text-align: right; width: 70%" class="Label">Project Name/Request Id :
                                        <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="60%" TabIndex="1"></asp:TextBox>
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:CheckBox ID="chkAllProjects" TabIndex="1" runat="server" Text="All" AutoPostBack="True"
                                                    OnCheckedChanged="chkAllProjects_CheckedChanged"></asp:CheckBox><asp:Button Style="display: none"
                                                        ID="btnSetProject" OnClick="btnSetProject_Click" runat="server" Text=" Project"></asp:Button><asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                                    CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                    CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                                    OnClientShowing="ClientPopulated" ServiceMethod="GetProjectCompletionList" ServicePath="AutoComplete.asmx"
                                                    TargetControlID="txtProject" UseContextKey="True">
                                                </cc1:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center" class="Label" colspan="2">Sent Date :&nbsp;&nbsp;
                    <asp:TextBox ID="txtFromDate" TabIndex="2" runat="server" Width="10%">
                    </asp:TextBox>&nbsp;&nbsp;
                            <cc1:CalendarExtender ID="CalExtFromDate" runat="server" Format="dd-MMM-yyyy" TargetControlID="txtFromDate">
                            </cc1:CalendarExtender>
                            To :&nbsp;&nbsp;<asp:TextBox ID="txtToDate" TabIndex="3" runat="server" Width="10%">
                            </asp:TextBox>
                            <cc1:CalendarExtender ID="CalExtToDate" runat="server" Format="dd-MMM-yyyy" TargetControlID="txtToDate">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center" colspan="2">
                            <asp:Button ID="btnSearch" TabIndex="4" OnClick="btnSearch_Click" runat="server"
                                Text="Search" ToolTip="Search" CssClass="btn btnnew"></asp:Button>
                            <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Text="Cancel"
                                CssClass="btn btncancel" TabIndex="5" ToolTip="Cancel"></asp:Button>
                            <asp:Button ID="btnExit" OnClick="btnExit_Click" runat="server" Text="Exit" ToolTip="Exit"
                                CssClass="btn btnexit" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this); "
                                TabIndex="6"></asp:Button>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </fieldset>
    <br />
    <br />
    <fieldset id="fsetReceiveSample" runat="server" class="FieldSetBox" style="display: none; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
        <legend class="LegendText" style="color: Black; font-size: 12px">
            <img id="img1" alt="Receive Samples Data" src="images/panelcollapse.png"
                onclick="Display(this,'divReceiveData');" runat="server" style="margin-right: 2px;" />Receive Samples Data</legend>
        <div id="divReceiveData">
            <table style="width: 100%" cellpadding="5px">
                <tbody>
                    <tr>
                        <td colspan="2">
                            <table width="100%">
                                <tr>
                                    <td style="width: 53%; text-align: right;">
                                        <asp:Label ID="lblSampleId" runat="server" Text="Sample Id :" Visible="False">
                                        </asp:Label>
                                        <asp:TextBox ID="txtSampleId" runat="server" OnTextChanged="txtSampleId_TextChanged">
                                        </asp:TextBox>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:Button ID="btnReceiveAll" TabIndex="6" OnClick="btnReceiveAll_Click" runat="server"
                                            Text="Receive All" ToolTip="Recieve All" CssClass="btn btnsave"  Visible="false"
                                            OnClientClick="return ValidationToReceive('<%= gvwSample.ClientID %>');"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblSampleType" runat="server" Text="Filter By Sample Type :" Visible="False"></asp:Label>
                                        <asp:DropDownList ID="ddlSampleType" runat="server" class="dropDownList" AutoPostBack="True"
                                            Visible="False">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left;">
                            <asp:Label ID="lblSelect" runat="server" Text="Select Sample Source:" Visible="False"></asp:Label>
                            <asp:DropDownList ID="ddlSelect" runat="server" class="dropDownList" AutoPostBack="True"
                                Visible="False">
                                <asp:ListItem><-Select-></asp:ListItem>
                                <asp:ListItem Text="Location" Value="L" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Site" Value="S"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="text-align: right;">
                            <asp:Label ID="lblLocationSite" runat="server" Text="Select Location/Site:" Visible="False"></asp:Label>
                            <asp:DropDownList ID="ddlLocationSite" runat="server" class="dropDownList" AutoPostBack="True"
                                Visible="False">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:UpdatePanel ID="upReceiveSamples" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlgvwSample" runat="server" BorderWidth="1px" BorderStyle="None"
                                        ScrollBars="Auto" Width="100%" Style="margin: auto;">
                                        <asp:GridView ID="gvwSample" TabIndex="5" runat="server" OnPageIndexChanging="gvwSample_PageIndexChanging"
                                            AllowPaging="False" PageSize="25" AutoGenerateColumns="False" OnRowDataBound="gvwSample_RowDataBound"
                                            OnRowCreated="gvwSample_RowCreated" OnRowCommand="gvwSample_RowCommand" Width="100%">
                                            <RowStyle BackColor="#cee3ed" Font-Names="Verdana" VerticalAlign="Middle" HorizontalAlign="left"
                                                Font-Size="9pt" ForeColor="navy" />
                                            <EditRowStyle BackColor="#cee3ed" Font-Names="Verdana" Font-Size="9pt" VerticalAlign="Middle" />
                                            <HeaderStyle Font-Underline="False" BackColor="#1560a1" Font-Names="Verdana" VerticalAlign="Top"
                                                Font-Size="10pt" HorizontalAlign="Center" ForeColor="white" Font-Bold="True" />
                                            <FooterStyle BackColor="#1560a1" Font-Names="Verdana" Font-Size="X-Small" HorizontalAlign="left"
                                                ForeColor="Navy" Font-Bold="True" />
                                            <AlternatingRowStyle BackColor="white" Font-Names="Verdana" HorizontalAlign="left"
                                                Font-Size="9pt" ForeColor="navy" />
                                            <PagerStyle ForeColor="#ffa24a" Font-Underline="False" BackColor="white" Font-Bold="True"
                                                Font-Names="Verdana" HorizontalAlign="Center" Font-Size="X-Small" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="All">
                                                    <HeaderTemplate>
                                                        <input id="chkSelectAll" onclick="SelectAll(this, 'gvwSample')" type="checkbox" />
                                                        <asp:Label ID="lblSelectAll" runat="server" Text="All"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelectSample" onclick="CheckUncheckAll('gvwSample')" runat="server"></asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="nSampleTypeDetailNo" HeaderText="nSampleTypeDetailNo" />
                                                <asp:BoundField DataField="vSampleBarCode" HeaderText="Sample" />
                                                <asp:BoundField DataField="vSampleTypeDesc" HeaderText="Sample Type" />
                                                <asp:BoundField DataField="cSampleStatusflag" HeaderText="cSampleStatusflag" />
                                                <asp:BoundField DataField="vWorkspacedesc" HeaderText="Project" />
                                                <asp:BoundField DataField="vSubjectID" HeaderText="SubjectId">
                                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="FullName" HeaderText="Subject" />
                                                <asp:BoundField DataField="vNodeDisplayName" HeaderText="Activity" />
                                                <asp:BoundField DataField="dSendOnDate" HeaderText="Sent Date" HtmlEncode="False" />
                                                <asp:BoundField DataField="vSendBYUser" HeaderText="Sent By" />
                                                <asp:BoundField DataField="vSendBYDeptName" HeaderText="Sent By Dept." />
                                                <asp:TemplateField HeaderText="Receive">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnReceive" runat="server" Text="Receive" ToolTip="Recieve" CssClass="btn btnsave" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Reject Detail">
                                                    <ItemTemplate>
                                                        <table style="width: 130px">
                                                            <tbody>
                                                                <tr>
                                                                    <td rowspan="2">
                                                                        <asp:TextBox ID="txtRejectionRemark" runat="server" Text='<%# DataBinder.Eval(Container.Dataitem,"vRemark") %>'
                                                                            TextMode="MultiLine"></asp:TextBox>
                                                                    </td>
                                                                    <td style="width: 160px">
                                                                        <asp:Button ID="btnNotReceived" runat="server" Text="Not Received" ToolTip="Not Recieved"
                                                                            CssClass="btn btncancel"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 160px">
                                                                        <asp:Button ID="btnReject" runat="server" Text="Discard" ToolTip="Discard" CssClass="btn btncancel" ></asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="23%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    
                </tbody>
            </table>
        </div>
    </fieldset>
</asp:Content>
