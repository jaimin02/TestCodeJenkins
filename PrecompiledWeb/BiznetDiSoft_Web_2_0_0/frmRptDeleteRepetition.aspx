<%@ page title="" language="VB" masterpagefile="~/ECTDMasterPage.master" autoeventwireup="false" inherits="frmRptDeleteRepetition, App_Web_22suyskz" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%--<%@ Register Src="~/UserControls/MultiSelectDropDown.ascx" TagName="DropDownControl" TagPrefix="UserControl" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <style type="text/css">
        ul {
            list-style: none;
        }

        #ctl00_CPHLAMBDA_tvSubject ul {
            padding-left: 0Px !important;
        }

        #ctl00_CPHLAMBDA_tvActivity ul {
            padding-left: 0Px !important;
        }

        .FieldSetBox {
            border: #aaaaaa 1px solid;
            z-index: 0px;
            border-radius: 4px;
        }

        html, body {
	        overflow-x: hidden;
        }
    </style>

    <table style="width: 80%">
        <tr>
            <td>
                <asp:UpdatePanel ID="upcontrols" runat="server">
                    <ContentTemplate>

                        <fieldset class="FieldSetBox">
                            <legend class="LegendText" style="color: Black">
                                <img id="imgfldgen" alt="CRF Activity Deletion Report" src="images/panelcollapse.png"
                                    onclick="displayCRFInfo(this,'tblEntryData');" runat="server" style="margin-right: 2px;" />CRF
                                Deleted Activity Detail </legend>
                            <div id="tblEntryData">
                                <table style="width: 90%">
                                    <tr>
                                        <td colspan="3" style="height: 10px; width: 100%;"></td>
                                    </tr>
                                    <tr>

                                        <td class="LabelText" nowrap="nowrap" style="text-align: right; width: 30%;">Project Name/Request Id*:
                                        </td>
                                        <td class="Label" style="text-align: left; width: 50%">
                                            <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="85%" TabIndex="1" Style="margin-left: 5px;"></asp:TextBox>
                                            <asp:Button Style="display: none" ID="btnSetProject" runat="server" Text=" Project"></asp:Button>
                                            <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                                CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                                OnClientShowing="ClientPopulated" ServiceMethod="GetMyProjectCompletionList"
                                                ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True"
                                                CompletionListElementID="pnlProjectList">
                                            </cc1:AutoCompleteExtender>

                                            <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden; width: 95%;" />

                                        </td>
                                        <td style="text-align: left; width: 20%">
                                            <asp:CheckBox ID="chkParent" runat="server" Text="Include Parent" Style="display: none;" onChange="RemoveGrid();" />
                                        </td>
                                    </tr>
                                    <%-- <tr>
                    <td colspan="3" style="height: 3px;"></td>
                </tr>--%>
                                    <tr>
                                        <td class="LabelText" nowrap="nowrap" style="text-align: right; width: 30%;">Period:
                                        </td>
                                        <td class="Label" nowrap="nowrap" style="text-align: left; width: 70%;" colspan="2">
                                            <asp:DropDownList ID="ddlPeriods" CssClass="EntryControl" runat="server" AutoPostBack="true"
                                                Style="width: 230px; margin-left: 5px;" TabIndex="2">
                                            </asp:DropDownList>
                                        </td>

                                    </tr>

                                    <%-- <tr>
                    <td colspan="3" style="height: 5px;"></td>
                </tr>--%>
                                    <tr>
                                        <td class="LabelText" nowrap="nowrap" style="text-align: right; width: 30%;">Report Type:
                                        </td>
                                        <td class="Label" nowrap="nowrap" style="text-align: left; width: 70%;" colspan="2">
                                            <asp:DropDownList ID="ddlRptType" CssClass="EntryControl" runat="server" AutoPostBack="false"
                                                Style="width: 230px; margin-left: 5px;" TabIndex="3" onChange="RemoveGrid();">
                                                <asp:ListItem Text="Activity Wise" Value="Act"></asp:ListItem>
                                                <asp:ListItem Text="Attribute Wise" Value="Att"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <%--  <tr>
                    <td colspan="3" style="height: 5px;"></td>
                </tr>--%>
                                    <tr>
                                        <td class="LabelText" nowrap="nowrap" style="text-align: right; width: 30%;">Activity:
                                        </td>
                                        <td class="Label" nowrap="nowrap" style="text-align: left; width: 70%;" colspan="2">
                                            <asp:DropDownList ID="ddlActivity" CssClass="EntryControl" runat="server" AutoPostBack="true"
                                                Style="width: 350px; margin-left: 5px;" TabIndex="4">
                                            </asp:DropDownList>
                                        </td>

                                    </tr>
                                    <%--<tr>
                    <td colspan="3" style="height: 5px;"></td>
                </tr>--%>
                                    <tr>
                                        <td class="LabelText" nowrap="nowrap" style="text-align: right; width: 30%;">Deleted By:
                                        </td>
                                        <td class="Label" nowrap="nowrap" style="text-align: left; width: 70%;" colspan="2">
                                            <asp:DropDownList ID="ddlDeletedBy" CssClass="EntryControl" runat="server" AutoPostBack="true"
                                                Style="width: 350px; margin-left: 5px;" TabIndex="5">
                                            </asp:DropDownList>
                                        </td>

                                    </tr>
                                    <%--      <tr>
                    <td colspan="3" style="height: 5px;"></td>
                </tr>--%>
                                    <%--  <tr runat="server" id="trselectSubject">
                    <td class="Label" nowrap="nowrap" style="text-align: right; width: 35%;"></td>
                    <td class="Label" nowrap="nowrap" style="text-align: left; width: 65%;" colspan="2">
                       
                    </td>
                </tr>--%>
                                    <tr>
                                        <td class="LabelText" nowrap="nowrap" style="text-align: right; width: 30%;">Subject No.*:
                                        </td>
                                        <td class="Label" nowrap="nowrap" style="text-align: left; width: 50%;">
                                            <%--            <asp:DropDownList ID="" CssClass="EntryControl" runat="server" AutoPostBack="false"
                            Style="width: 350px; margin-left: 5px;" TabIndex="3">
                        </asp:DropDownList>--%>
                                            <%--   <asp:UpdatePanel ID="upSubject" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                            <ContentTemplate>
                                <UserControl:DropDownControl ID="ddlSubject" runat="server" Width="220" ></UserControl:DropDownControl>
                            </ContentTemplate>
                        </asp:UpdatePanel>--%>
                                            <asp:Panel ID="pnlSubjects" runat="server" BackColor="White" BorderColor="Navy" BorderWidth="1px"
                                                ScrollBars="Auto" Style="max-height: 120px; min-height: 15px; margin-left: 5px; width: 95;">
                                                <asp:CheckBoxList ID="chkLstSubjects" Style="width: 100%" runat="server" RepeatColumns="4"
                                                    RepeatDirection="Horizontal" onClick="SelectSubjects();">
                                                </asp:CheckBoxList>
                                            </asp:Panel>

                                        </td>
                                        <td style="text-align: left; width: 20%">
                                            <asp:CheckBox ID="chkSelectAll" runat="server" Text="Select All" onClick="SelectAllSubjects(this)" />
                                        </td>

                                    </tr>

                                    <tr>
                                        <td colspan="3" style="height: 5px; width: 100%;"></td>
                                    </tr>

                                    <tr>


                                        <td align="center" colspan="3" style="width: 100%;">
                                            <asp:Button ID="btnGo" runat="server" OnClientClick="return Validation();" CssClass="btn btngo"
                                                Text="" ToolTip="Go" TabIndex="3" />
                                            <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" Text="Cancel" ToolTip="Cancel"/>
                                        </td>
                                   </tr>
                                </table>
                            </div>
                        </fieldset>
                        <fieldset class="FieldSetBox" id="fldgrdParent" runat="server" style="display: none; margin-top: 20px">
                            <legend class="LegendText" style="color: Black">
                                <img id="imgfldGrid" alt="CRF Activity Deletion Report" src="images/panelcollapse.png"
                                    onclick="displayCRFInfo(this,'tblGrid');" runat="server" style="margin-right: 2px;" />CRF Activity Deletion Report</legend>
                            <div id="tblGrid">
                                <table style="width: 100%">
                                    <tr>
                                        <asp:UpdatePanel ID="upExport" runat="server">
                                            <ContentTemplate>
                                                <td align="center" colspan="3" style="width: 100%;">
                                                    <asp:Button ID="btnExport" runat="server" CssClass="btn btnexcel" ToolTip="Export To Excel"
                                                         TabIndex="5" />
                                                    <asp:Button ID="btnPDF" runat="server" CssClass="btn btnpdf" ToolTip="Export To PDF"
                                                         TabIndex="6" />
                                                </td>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btnExport" />
                                                <asp:PostBackTrigger ControlID="btnPDF" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="3" style="width: 100%;">
                                            <div style="overflow: auto; height: 300px; margin: auto; width: 95%;">
                                                <asp:GridView ID="gvDeletedRecords" runat="server" AutoGenerateColumns="true"
                                                   Font-Size="Small" SkinID="grdViewSmlAutoSize"
                                             ShowFooter="True" ></asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" language="javascript">

        function pageLoad() {
            //    $('tr td', ctl00_CPHLAMBDA_gvDeletedRecords).css('text-align', 'center')
        }

        function ClientPopulated(sender, e) {

            RemoveGrid();
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {

            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        function RemoveGrid() {
            if ($('#<%=gvDeletedRecords.ClientID%>').length > 0) $('#<%=gvDeletedRecords.ClientID%>').remove();
            $get('<%= btnExport.ClientID%>').style.display = "none";
            $get('<%= btnPDF.ClientID%>').style.display = "none";
            $get('<%= fldgrdParent.ClientID%>').style.display = "none";
        }


        function SelectAllSubjects(ele) {
            ele.checked == true ? $('input', $get('<%=pnlSubjects.ClientId%>')).attr("checked", true) : $('input', $get('<%=pnlSubjects.ClientId%>')).attr("checked", false)
        }
        function SelectSubjects() {
            RemoveGrid();
            if ($('input', $get('<%=pnlSubjects.ClientId%>')).length == $('input:checked', $get('<%=pnlSubjects.ClientId%>')).length) $($get('<%= chkSelectAll.ClientId %>')).prop({ checked: true, indeterminate: false });
            else if ($('input:checked', $get('<%=pnlSubjects.ClientId%>')).length > 0) $($get('<%= chkSelectAll.ClientId %>')).prop({ checked: false, indeterminate: true });
            else $($get('<%= chkSelectAll.ClientId %>')).prop({ checked: false, indeterminate: false });
    }
    function Validation() {
        if ($get('<%= HProjectId.ClientId %>').value.toString().trim().length <= 0) {
            msgalert('Please Enter Project !');
            $get('<%= txtproject.ClientID%>').focus();
            return false;
        }
        else if ($('input:checked', $get('<%=pnlSubjects.ClientId%>')).length == 0) {
            msgalert("Please select subjects !");
            return false;
        }
        return true;
    }

    function displayCRFInfo(control, target) {

        if (control.src.toString().toUpperCase().search("EXPAND") != -1) {
            $("#" + target).slideToggle(400);
            control.src = "images/panelcollapse.png";
        }
        else {
            $("#" + target).slideToggle(400);
            control.src = "images/panelexpand.png";
        }
    }
    </script>

</asp:Content>

