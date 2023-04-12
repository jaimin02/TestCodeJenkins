<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmCRFLockDtl, App_Web_2mzu20n4" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    
    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>
            <table cellpadding="5px" width="100%">
                <tbody>
                    <tr>
                        <td style="text-align: right; width: 40%" class="Label">
                            Project No/Site Id*:
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="40%" />
                            <asp:Button Style="display: none" ID="btnSetProject" runat="server" Text=" Project" />
                            <asp:HiddenField ID="HProjectId" runat="server" />
                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                                TargetControlID="txtProject" ServicePath="AutoComplete.asmx" ServiceMethod="GetMyProjectCompletionList"
                                OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1"
                                CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1"
                                CompletionListElementID="pnlProject">
                            </cc1:AutoCompleteExtender>
                            <asp:Panel ID="pnlProject" runat="server" Style="max-height: 100px; overflow: auto;
                                overflow-x: hidden;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right" class="Label">
                            Select Status:
                        </td>
                        <td style="text-align: left" class="Label">
                            <asp:RadioButtonList ID="rbtnlstLockUnlock" runat="server" RepeatDirection="Horizontal"
                                Enabled="false">
                                <asp:ListItem Value="L">Lock</asp:ListItem>
                                <asp:ListItem Value="U">Unlock</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right" class="Label">
                            Remarks:
                        </td>
                        <td style="text-align: left" class="Label" nowrap="nowrap">
                            <asp:TextBox ID="txtRemarks" runat="server" Width="40%" TextMode="MultiLine" MaxLength="200" />
                        </td>
                    </tr>
                    <%-- <tr height="10">
                        <td style="text-align: right" class="Label">
                        </td>
                        <td style="text-align: left" class="Label">
                        </td>
                    </tr>--%>
                    <tr>
                        <%-- <td style="text-align: right" class="Label">
                        </td>--%>
                        <td style="text-align: center" class="Label" colspan="2">
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btnsave" ToolTip="Save"
                                OnClientClick="return ValidationForSave();" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="btn btncancel" />
                            <asp:Button ID="btnExit" runat="server" Text="Exit" CssClass="btn btnexit" ToolTip="Exit"
                                OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
                        </td>
                    </tr>
                    <%-- <tr height="10">
                        <td style="text-align: right" class="Label">
                        </td>
                        <td style="text-align: left" class="Label">
                        </td>
                    </tr>--%>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upGrid" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tbody>
                    <tr>
                        <td style="text-align: Center" colspan="2">
                            <asp:GridView ID="GVCRFLockDtl" runat="server" SkinID="grdViewAutoSizeMax" AutoGenerateColumns="false" style="width:60%; margin:auto;">
                            <Columns>
                                    <asp:BoundField HeaderText="#">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="nCRFLockDtlNo" HeaderText="nCRFLockDtlNo" />
                                    <asp:BoundField DataField="vWorkspaceId" HeaderText="vWorkspaceId" />
                                    <asp:BoundField DataField="vProjectNo" HeaderText="Site Id" Visible="false" />
                                    <asp:BoundField DataField="cLockFlag" HeaderText="Status" />
                                    <asp:BoundField DataField="iTranNo" HeaderText="Tran No" />
                                    <asp:BoundField DataField="vRemarks" HeaderText="Remarks" />
                                    <asp:BoundField DataField="vLockChangedBy" HeaderText="Status Changed By" />
                                    <asp:BoundField DataField="dLockChangedOn" HeaderText="Status Changed On" DataFormatString="{0:dd-MMM-yyyy, HH:mm}">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </tbody>
            </table>
            <asp:HiddenField id="hdnSubjectlock" runat="server"/>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

    <script type="text/javascript" language="javascript">

        jQuery(window).focus(function () {
            ThemeSelection();
            return false;
        });

        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        function ValidationForSave() {
            if (document.getElementById('<%= txtProject.ClientId %>').value.trim() == '') {
                msgalert('Please Enter Project !');
                document.getElementById('<%= txtProject.ClientId %>').focus();
                return false;
            }
            else if (document.getElementById('<%= txtRemarks.ClientId %>').value.trim() == '') {
                msgalert('Please Enter Remarks !');
                document.getElementById('<%= txtRemarks.ClientId %>').focus();
                return false;
            }


            var RB1 = document.getElementById("<%=rbtnlstLockUnlock.ClientID%>");
            var radio = RB1.getElementsByTagName("input");
            var isChecked = false;
            for (var i = 0; i < radio.length; i++) {
                if (radio[0].checked) {
                    confirmbox();
                    break;
                }
            }
                                    
            return true;
        }

        function ValidationForView() {
            if (document.getElementById('<%= txtProject.ClientId %>').value.trim() == '') {
                msgalert('Please Enter project !');
                document.getElementById('<%= txtProject.ClientId %>').focus();
                return false;
            }
            return true;
        }

        function confirmbox() {
            msgConfirmDeleteAlert(null, "MSR of this project are in unlock state. This action will lock both MSR & Project status.", function (isConfirmed) {
                if (isConfirmed) {
                    document.getElementById("<%= hdnSubjectlock.ClientID %>").value = "True";
                    return true;
                } else
                {
                    document.getElementById("<%= hdnSubjectlock.ClientId %>").value = "False";
                    return false;
                }
            });
            return false;
        }
        function ShowAlert(msg) {
            alertdooperation(msg, 1, "frmCRFLockDtl.aspx?mode=1");
        }
        function savedata() {
            msgalert("Project Status and MSR Status Saved Sucessfully");
            var url = "frmCRFLockDtl.aspx";
            $(location).attr('href', url);
        }
        function ThemeSelection() {
            if (document.cookie.split(";")[0] == "Theme=Orange") {
                $("#ctl00_CPHLAMBDA_GVCRFLockDtl tr").last().css({ 'background-color': '#CF8E4C' });
            } else if (document.cookie.split(";")[0] == "Theme=Green") {
                $("#ctl00_CPHLAMBDA_GVCRFLockDtl tr").last().css({ 'background-color': '#33a047' });
            } else if (document.cookie.split(";")[0] == "Theme=Demo") {
                $("#ctl00_CPHLAMBDA_GVCRFLockDtl tr").last().css({ 'background-color': '#999966' });
            } else if (document.cookie.split(";")[0] == "Theme=Blue") {
                $("#ctl00_CPHLAMBDA_GVCRFLockDtl tr").last().css({ 'background-color': '#1560a1' });
            }
        }
    </script>

</asp:Content>
