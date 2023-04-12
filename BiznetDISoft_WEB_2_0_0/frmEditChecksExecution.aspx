<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmEditChecksExecution.aspx.vb" Inherits="frmEditChecksExecution" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <asp:UpdatePanel runat="server" ID="UpControls" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlHeader" runat="server" Style="margin-top: 1%; width: 100%;">
                <div id="divExpandable" runat="server" style="font-weight: bold; font-size: 14px;
                    float: none; color: white; background-color: #1560a1; width: 95%; margin: auto;
                    height: 18px;">
                    <div>
                        <asp:Image ID="imgArrows" runat="server" Style="float: right;" />
                    </div>
                    <div style="text-align: left; margin-left: 1%;">
                        <asp:Label ID="Label1" runat="server" Text="Edit Check Criteria">
                        </asp:Label></div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlProject" runat="server" Width="100%">
                <table width="95%" cellpadding="3" style="border: 1px solid; border-color: Black;
                    margin: auto;">
                    <tr>
                        <td width="30%" class="Label" style="text-align: right;">
                            Project Name/Request Id* :
                        </td>
                        <td width="70%" style="text-align: left;">
                            <asp:TextBox ID="txtProject" runat="server" Width="50%"></asp:TextBox>
                            <asp:Button Style="display: none" ID="btnSetProject" runat="server" Text=" Project">
                            </asp:Button><asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                                TargetControlID="txtProject" ServicePath="AutoComplete.asmx" ServiceMethod="GetMyProjectCompletionList"
                                OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1"
                                CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                CompletionListCssClass="autocomplete_list" CompletionListElementID="pnlTemplate"
                                BehaviorID="AutoCompleteExtender1">
                            </cc1:AutoCompleteExtender>
                            <asp:Panel ID="pnlTemplate" runat="server" Style="max-height: 200px; overflow: auto;
                                overflow-x: hidden;" />
                        </td>
                    </tr>
                    <tr>
                        <td width="30%" class="Label" style="text-align: right;">
                            Edit Check Type :
                        </td>
                        <td width="70%" style="text-align: left;">
                            <asp:RadioButtonList ID="rblEditCheckType" runat="server" RepeatDirection="Horizontal" ToolTip="Inclusion Or Exclusion Criteria"
                                            AutoPostBack="true">
                                            <asp:ListItem Value="P" Text="Within Activity" />
                                            <asp:ListItem Value="C" Text="Cross Activity"/>
                                        </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td width="30%" class="Label" style="text-align: right;">
                            Activity* :
                        </td>
                        <td width="70%" style="text-align: left;">
                            <asp:CheckBox ID="chkAll" Text="All" runat="server" class="checkbox"
                                Style="vertical-align: middle;" onClick="return CheckAll(this);"/>
                            <asp:Panel ID="pnlActivity" runat="server" BackColor="White" BorderColor="Navy" BorderWidth="1px"
                                        ScrollBars="Auto" Style="max-height: 120px; max-width: 70%; min-height: 15px;">
                                        <asp:CheckBoxList ID="chkLstActivity" CssClass="CheckBoxActivity" Style="width: 100%" runat="server" RepeatColumns="4"
                                            RepeatDirection="Horizontal" onclick="chkHeaderCheckBox();">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;" colspan="2">
                            <asp:Button ID="btnExecute" class="bn btnsave" Text="Execute" runat="server" OnClientClick="return Validation('E');" />
                            <asp:Button ID="btnView" class="btn btnnew" Text="View" runat="server" OnClientClick="return Validation('V');" />
                            <asp:Button ID="btnCancel" class="btn btncancel" Text="Cancel" runat="server" />
                            <asp:Button ID="btnExport" class="btn btnexcel"  Visible="false"
                                runat="server" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <cc1:CollapsiblePanelExtender ID="Collpase1" runat="server" TargetControlID="pnlProject"
                ExpandControlID="pnlHeader" CollapseControlID="pnlHeader" ExpandedImage="~/Images/minus.png"
                CollapsedImage="~/Images/add.png" ImageControlID="imgArrows" AutoCollapse="false"
                AutoExpand="false">
            </cc1:CollapsiblePanelExtender>
            <asp:Panel ID="PnlDetail" runat="server" Style="margin-top: 2%; display: none; width: 100%;">
                <div id="divDetail" runat="server" style="font-weight: bold; font-size: 14px; float: none;
                    color: white; background-color: #1560a1; width: 95%; margin: auto; height: 18px;">
                    <div>
                        <asp:Image ID="imgArrowsDetail" runat="server" Style="float: right;" />
                    </div>
                    <div style="text-align: left; margin-left: 1%;">
                        <asp:Label ID="Label2" runat="server" Text="Edit Check Details">
                        </asp:Label></div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlDetailGrid" runat="server" Style="display: none; width: 100%;">
                <table width="95%" cellpadding="5px" style="border: 1px solid; border-color: Black;
                    margin: auto;">
                    <tr>
                        <td width="100%" colspan="2">
                            <asp:Panel ID="pnl1" runat="server" Style="width: 99%; margin: auto; margin-top: 2%;">
                                <div style="width: 99%; text-align: right;">
                                    <asp:Button ID="btnResolveAll" CssClass="btn btnnew" runat="server" Text="Resolve"
                                        Visible="false" />
                                </div>
                                <table width="90%" cellpadding="0px" cellspacing="0px">
                                    <tr>
                                        <td style="height: 10px;">
                                        </td>
                                    </tr>
                                </table>
                                <div style="width: 98%; overflow: auto; overflow-y: hidden; margin: auto;">
                                    <asp:GridView ID="Grid" SkinID="grdViewSmlAutoSize" runat="server" AutoGenerateColumns="false"
                                        AllowPaging="true" PageSize="10" Width="130%" PagerSettings-Position="TopAndBottom">
                                        <Columns>
                                            <asp:BoundField DataFormatString="number" HeaderText="#" ItemStyle-Width="2%">
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="vSubjectId" HeaderText="Subject Id" />
                                            <asp:BoundField DataField="vInitials" HeaderText="Initials" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-VerticalAlign="Middle" />
                                            <asp:BoundField DataField="vMySubjectNo" HeaderText="Screen No" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-VerticalAlign="Middle" />
                                            <asp:BoundField DataField="vRandomizationNo" HeaderText="Patient/ Randomization No"
                                                ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" />
                                           
                                             <asp:BoundField DataField="RepetitionNo" HeaderText="Repeat No" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-VerticalAlign="Middle" />

                                            <asp:BoundField DataField="ParentvNodeDisplayName" HeaderText="Visit"  ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-VerticalAlign="Middle" />

                                            <asp:TemplateField HeaderText="Activity" ItemStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnActvityName" runat="server"><%#Eval("vNodeDisplayName")%></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="vWorkspaceId" HeaderText="vWorkspaceId" />
                                            <asp:BoundField DataField="iNodeId" HeaderText="iNodeId" />
                                            
                                            <asp:BoundField DataField="vActivityId" HeaderText="vActivityId" />
                                            <asp:BoundField DataField="iPeriod" HeaderText="Period" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-VerticalAlign="Middle" />
                                            <asp:BoundField DataField="vQueryMessage" HeaderText="Edit Check Formula" />
                                            <asp:BoundField DataField="vErrorMessage" HeaderText="Discrepancy Message" ItemStyle-Width="18%" />
                                            <asp:BoundField DataField="cEditCheckType" HeaderText="Edit Check Type" />
                                            <asp:BoundField DataField="vFiredDate" HeaderText="Executed On" />
                                            <asp:BoundField DataField="cIsQuery" HeaderText="cIsQuery" />
                                            <asp:BoundField DataField="nCRFDtlNo" HeaderText="nCRFdTLNo" />
                                            <asp:BoundField DataField="vResolvedOn" HeaderText="Resolved On" />
                                            <asp:BoundField DataField="vResolvedBy" HeaderText="Resolved By" />
                                            <asp:TemplateField HeaderText="Reason/Remark">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkResolvedRemarks" runat="server" />
                                                    <asp:Label ID="lblResolveRemark" runat="server" Text='<%#Eval("vResolvedRemark")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="nEditChecksDtlNo" HeaderText="nEditChecksDtlNo" />
                                            <asp:BoundField DataField="iMySubjectNo" HeaderText="iMySubjectNo" />
                                            <asp:BoundField DataField="vProjectTypeCode" HeaderText="Project Type Code" />
                                            
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="display: none; width: 35%; max-height: 200px; text-align: left; margin: auto;
                                left: 30% !important;" class="divModalPopup" runat="server" id="divForRemarks">
                                <table style="width: 100%; margin-bottom: 2%">
                                    <tr>
                                        <td colspan="2" style="font-size: 13px;" class="Label">
                                            <img id="imgClose" title="Close Window" alt="Close" src="images/Sqclose.gif" style="position: relative;
                                                float: right; right: 5px;" onclick="funCloseDiv('divForRemarks');" />
                                            Reason/Remark to resolve edit check
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Label" style="text-align: right; width: 20%">
                                            <span class="Label">Reason* :</span>
                                        </td>
                                        <td style="text-align: left; width: 80%">
                                            <asp:DropDownList ID="ddlRemarks" runat="server" CssClass="dropDownList" Width="90%">
                                                <asp:ListItem>Select Reason</asp:ListItem>
                                                <asp:ListItem>No Action Required</asp:ListItem>
                                                <asp:ListItem>Resolved Internally</asp:ListItem>
                                                <asp:ListItem>Obsolete</asp:ListItem>
                                                <asp:ListItem>No source information available</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align: center; width: 100%">
                                            <span class="Label">OR </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Label" style="text-align: right; width: 20%">
                                            <span class="Label" style="vertical-align: top;">Remark* : </span>
                                        </td>
                                        <td style="text-align: left; width: 78%">
                                            <asp:TextBox TextMode="MultiLine" ID="txtRemark" Text="" class="Textbox" Width="88%"
                                                runat="server" Height="60px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="height: 10px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="Label" style="text-align: center; width: 100%">
                                            <asp:Button ID="btnSaveRemark" runat="server" class="btn btnsave" Text="Save" OnClientClick="return funSaveRemark();" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="ModalBackGround" runat="server" class="divModalBackGround" style="display: none;">
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="pnlDetailGrid"
                ExpandControlID="PnlDetail" CollapseControlID="PnlDetail" ExpandedImage="~/Images/minus.png"
                CollapsedImage="~/Images/add.png" ImageControlID="imgArrowsDetail" AutoCollapse="false"
                AutoExpand="false">
            </cc1:CollapsiblePanelExtender>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Grid" EventName="PageIndexChanging" />
            <asp:AsyncPostBackTrigger ControlID="btnResolveAll" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:HiddenField ID="HFWorkpsaceEditChecksDtlNo" runat="server" />
    <script type="text/javascript" language="javascript">
        function pageLoad(sender, e) {
            $find('AutoCompleteExtender1').set_contextKey('iUserId = <%= Session(S_UserID).ToString() %>');
        }

        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }
        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
    $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        function Validation(type) {
            if (document.getElementById('<%= txtProject.ClientID %>').value == '') {
                msgalert('Please Enter Project !');
                return false;
            }
            
            if (document.getElementById('<%=chkLstActivity.ClientID%>')) {
                var i = 0;
                $(".CheckBoxActivity input:checked").each(function () {
                    i = 1;
                });
                if (i == 0) { alert("Please Select activity."); return false;}
            }
            
            if (document.getElementById('<%=rblEditCheckType.ClientID%>').selectedIndex <= 0) {
                msgalert('Please Select Edit Check Type !');
                return false;
            }
            if (type != 'E') {
                if (document.getElementById('<%= rblEditCheckType.ClientId %>').value == 'V') {
                    msgalert('Please Select EditCheck Type !');
                    return false;
                }
            }
            return true;
        }

        function funOPEN(str) {

            window.open(str);
        }
        function SubjectData(e) {
            document.getElementById('divForRemarks').style.display = '';
            displayBackGround();
            $row = $(e).parents('tr:first');
            document.getElementById('HFWorkpsaceEditChecksDtlNo').value = $row[0].cells[8].children[1].value;

        }
        function funCloseDiv(div) {
            document.getElementById('<%=divForRemarks.ClientID %>').style.display = 'none';
            document.getElementById('<%=ModalBackGround.ClientId %>').style.display = 'none';

        }

        function displayBackGround() {

            document.getElementById('<%=divForRemarks.ClientId %>').style.display = '';
            document.getElementById('<%=ModalBackGround.ClientId %>').style.display = '';
            document.getElementById('<%=ModalBackGround.ClientId %>').style.height = ($(document).height()) + "px";
            document.getElementById('<%=ModalBackGround.ClientId %>').style.width = ($(document).width()) + "px";
        }

        function funHide() {
            //            if (document.getElementById('<%=rblEditCheckType.ClientId %>').value != 'V') {
            //                document.getElementById('<%= btnExecute.ClientID %>').style.display = 'none';
            //            }
            //            else {
            //                document.getElementById('<%= btnExecute.ClientID %>').style.display = '';
            //            }
        }

        function funSaveRemark() {
            if (document.getElementById('<%= ddlRemarks.ClientID %>').selectedIndex == 0) {
                if (document.getElementById('<%= txtRemark.ClientID %>').value.toString().trim() == '') {
                    msgalert("Please Enter Remark !"); 
                    return false;
                }
            }
            else if (document.getElementById('<%= ddlRemarks.ClientID %>').selectedIndex != 0 &&
                     document.getElementById('<%= txtRemark.ClientID %>').value.toString().trim() != '') {
                msgalert("Please provide either Reason or Remark !");
                return false;
            }
            return true;
        }

        function CheckAll(e) {
            $(".CheckBoxActivity input").attr('checked', false);
            if (e.checked) {
                $(".CheckBoxActivity input").attr('checked', true);
            }
            return true
        }
        function chkHeaderCheckBox() {
            $(".checkbox input").attr('checked', false);
            if ($(".CheckBoxActivity input").length == $(".CheckBoxActivity input:checked").length) {
                $(".checkbox input").attr('checked', true);
            }
        }

    </script>
</asp:Content>
