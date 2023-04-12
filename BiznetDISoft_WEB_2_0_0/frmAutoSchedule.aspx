<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmAutoSchedule.aspx.vb" Inherits="frmAutoSchedule" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <%--<script type="text/javascript" src="Script/popcalendar.js"></script>--%>

    <script type="text/javascript" src="Script/Validation.js"></script>

    <script type="text/javascript">

        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        function CheckLocation() {
            var ddl = $get('<%= ddlLocation.ClientId %>');

            if (ddl.selectedIndex <= 0) {
                msgalert('Please Select Location !');
                return false
            }
            else if (ddl.selectedIndex > 0) {
                return true;
            }
            return false;
        }

        function CheckLocationAndResource() {
            var ddlLocation = $get('<%= ddlLocation.ClientId %>');
            var ddlResource = $get('<%= ddlResource.ClientId %>');

            if (ddlLocation.selectedIndex <= 0) {
                msgalert('Please Select Location !');
                return false
            }
            else if (ddlResource.selectedIndex <= 0) {
                msgalert('Please Select Resource !');
                return false
            }
            return true;
        }

        function CheckDays(obj) {
            if (!checkVal(obj.value, obj.id, '2')) {
                msgalert("Please Enter Days In Numeric And Greater Than 0 !");
                obj.value = 1;
            }
            if (obj.value == "") {
                msgalert("Please Enter Days In Numeric And Greater Than 0 !");
                obj.value = 1;
            }
        }
      

      
   
    </script>

    <table cellpadding="0" width="98%">
        <tr>
            <td class="Label" style="text-align: right; width: 30; text-align: right">
                Project No. / Request ID* :
            </td>
            <td style="text-align: left; text-align: left; width: 70%">
                <span style="font-weight: normal">
                    <asp:TextBox ID="txtProject" TabIndex="1" runat="server" CssClass="textBox" Width="70%" /></span>
                <asp:Button Style="display: none" ID="btnSetProject" OnClick="btnSetProject_Click"
                    runat="server" Text=" Project" />
                <asp:HiddenField ID="HProjectId" runat="server" />
                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionListCssClass="autocomplete_list"
                    CompletionListElementID="pnlProjectRequestList" CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                    UseContextKey="True" MinimumPrefixLength="1" BehaviorID="AutoCompleteExtender1"
                    OnClientItemSelected="OnSelected" OnClientShowing="ClientPopulated" TargetControlID="txtProject"
                    ServicePath="AutoComplete.asmx">
                </cc1:AutoCompleteExtender>
                <asp:Panel ID="pnlProjectRequestList" runat="server" Style="max-height: 300px; overflow: auto;
                    overflow-x: hidden" />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="width: 100%">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td style="text-align: right; width: 30%;" class="Label">
                                    Select Location:
                                </td>
                                <td style="text-align: left; width: 70%;" class="Label">
                                    <asp:DropDownList ID="ddlLocation" CssClass="dropDownList" runat="server" AutoPostBack="True"
                                        TabIndex="2" />
                                    Select Resource:
                                    <asp:DropDownList ID="ddlResource" CssClass="dropDownList" runat="server" TabIndex="3" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%; text-align: right;">
                                </td>
                                <td class="Label" style="text-align: left; width: 70%">
                                    <asp:Button ID="btnApply" runat="server" Text=" Set Location " ToolTip="Set Location"
                                        CssClass="btn btnnew" TabIndex="4" />
                                    <asp:Button ID="btnView" runat="server" Text=" View " ToolTip="View" CssClass="btn btnnew"
                                        OnClientClick="return CheckLocation();" TabIndex="5" />
                                    <asp:Button ID="btnSchedule" runat="server" Text=" Schedule " ToolTip="Schedule"
                                        CssClass="btn btnnew" OnClientClick="return CheckLocationAndResource();" TabIndex="6" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="width: 99%; margin: auto; text-align: center;">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvwAutoSchedule" runat="server" SkinID="grdViewSmlAutoSize" AutoGenerateColumns="False">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelectRow" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="vActivityId" HeaderText="Activity Id">
                                    <ItemStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="NodeDisName" HeaderText="Activity">
                                    <ItemStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Can Start After">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("vCanStartAfter") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCanStartAfter" runat="server" Text='<%# Bind("vCanStartAfter") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Start Date">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("vAttr1Value") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlStartDate" runat="server" Wrap="False">
                                            <asp:TextBox ID="txtStartDate" runat="server" Style="width: 100px" CssClass="textBox"
                                                Text='<%# Bind("vAttr1Value") %>'></asp:TextBox>
                                            <img runat="server" id="imgStartDate" alt="Select  Date" src="images/calendar.gif" /></asp:Panel>
                                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtStartDate"  Format="dd-MMM-yyyy" PopupButtonID="imgStartDate">
                                            </cc1:CalendarExtender>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="End Date">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("vAttr1Value") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlEndDate" runat="server" Wrap="False">
                                            <asp:TextBox ID="txtEndDate" Style="width: 100px" runat="server" CssClass="textBox"
                                                Text='<%# Bind("vAttr2Value") %>'></asp:TextBox>
                                            <img runat="server" id="imgEndDate" alt="Select  Date" src="images/calendar.gif" /></asp:Panel>
                                            <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtEndDate"  Format="dd-MMM-yyyy" PopupButtonID="imgEndDate">
                                            </cc1:CalendarExtender>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Days">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtCompletionDays" runat="server" Text='<%# Bind("nCompletionDays") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtCompletionDays" runat="server" CssClass="textBox" Height="16px"
                                            Text='<%# Eval("nCompletionDays") %>' Width="46px" onblur="CheckDays(this);"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Update">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnReCalculate" runat="server" ImageUrl="~/Images/save1.png"
                                            CommandName="ReCalculate" ToolTip="Update" />
                                        <%--<asp:Button ID="btnReCalculate" runat="server" CommandName="ReCalculate" CssClass="button"
                                            Text=" Update " ToolTip="Update" />--%>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Location">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLocation" runat="server" Text='<%# Eval("vLocationResourceName") %>'></asp:Label>
                                        <asp:HiddenField ID="HLocationId" runat="server" Value='<%# Bind("vLocationCode") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Resource">
                                    <ItemTemplate>
                                        <asp:Label ID="lblResource" runat="server" Text='<%# Eval("vResourceName") %>'></asp:Label>
                                        <asp:HiddenField ID="HResourceId" runat="server" Value='<%# Bind("vAttr5Value") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="iNodeId" HeaderText="Node Id" />
                                <asp:BoundField DataField="cPeriodSpecific" HeaderText="cPeriodSpecific" />
                                <asp:BoundField DataField="nMilestone" HeaderText="nMilestone" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSchedule" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnApply" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <%--<td style="width: 30%;">
            </td>--%>
            <td style="width: 100%; text-align: center;" colspan="2">
                <asp:Button ID="btnSave" runat="server" Text=" Save " ToolTip="Save" CssClass="btn btnsave"
                    TabIndex="7" />
                <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" Text="Cancel" ToolTip="Cancel"
                    TabIndex="8" />
                <asp:Button ID="btnExit" runat="server" CssClass="btn btnexit" Text="Exit" ToolTip="Exit"
                    OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" TabIndex="9" />
            </td>
        </tr>
    </table>
</asp:Content>
