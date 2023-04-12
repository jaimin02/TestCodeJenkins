<%@ page language="VB" masterpagefile="~/ECTDMasterPage.master" autoeventwireup="false" inherits="frmSubjectWiseEntry, App_Web_l40sj1d0" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <style>
        .ExcellButtonShoHide {
            display: none;
        }
    </style>

    <asp:UpdatePanel ID="pnlSubjectWiseEntry" runat="server">
        <ContentTemplate>
            <table cellpadding="5px" style="width: 100%;">
                <tr>
                    <td>
                        <fieldset class="FieldSetBox" style="display: block; width: 96%; text-align: left; border: #aaaaaa 1px solid;">
                            <legend class="LegendText" style="color: Black; font-size: 12px">
                                <img id="img1" alt="Project Details" src="images/panelcollapse.png"
                                    onclick="Display(this,'divProjectFilterDetail');" runat="server" style="margin-right: 2px;" />Project Activity Details</legend>
                            <div id="divProjectFilterDetail">
                                <table style="width: 95%;" cellpadding="5px">
                                    <tbody>
                                        <tr>
                                            <td class="Label" style="white-space: nowrap; text-align: right; width: 30%;">Project Name* :
                                            </td>
                                            <td style="text-align: left; width: 70%">
                                                <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="65%" />
                                                <asp:Button ID="btnSetProject" runat="server" OnClick="btnSetProject_Click" Style="display: none"
                                                    Text=" Project" />
                                                <asp:HiddenField ID="HProjectId" runat="server" />
                                                <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                                    CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                    CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                                    OnClientShowing="ClientPopulated" ServiceMethod="GetMyProjectCompletionList"
                                                    ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True"
                                                    CompletionListElementID="pnlProjectList">
                                                </cc1:AutoCompleteExtender>
                                                <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" />
                                            </td>
                                        </tr>
                                        <tr id="trPeriod" runat="server" visible="false">
                                            <td style="text-align: right; width: 30%" class="Label">Period* :
                                            </td>
                                            <td style="text-align: left; width: 70%">
                                                <asp:DropDownList ID="ddlPeriod" runat="server" AutoPostBack="True" CssClass="dropDownList"
                                                    OnSelectedIndexChanged="ddlPeriod_SelectedIndexChanged" TabIndex="1" Width="30%">
                                                    <asp:ListItem Value="0" Text="Select Period"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Label" style="width: 30%; text-align: right;"></td>
                                            <td style="white-space: nowrap; width: 70%; text-align: left;" class="rblSubject">
                                                <asp:RadioButtonList ID="rblSubjectSpecific" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblSubjectSpecific_SelectedIndexChanged"
                                                    RepeatDirection="Horizontal">
                                                    <asp:ListItem Selected="True" Value="Y">Subject Specific</asp:ListItem>
                                                    <asp:ListItem Value="N">Generic</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 30%;" class="Label">Parent Activity* :
                                            </td>
                                            <td style="text-align: left; width: 70%">
                                                <asp:DropDownList ID="ddlVisit" runat="server" CssClass="dropDownList" TabIndex="2"
                                                    Width="65%" AutoPostBack="true">
                                                    <asp:ListItem Value="0" Text="Select Visit/Parent Activity"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 30%;" class="Label">Activity* :
                                            </td>
                                            <td style="text-align: left; width: 70%">
                                                <asp:DropDownList ID="ddlActivity" runat="server" CssClass="dropDownList" TabIndex="2"
                                                    Width="65%" AutoPostBack="true">
                                                    <asp:ListItem Value="0" Text="Select Activity"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:CheckBox ID="chkAllActivity" runat="server" Text="Attach To All Same Activities ?" AutoPostBack="true" TabIndex="11" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 30%;" class="Label">Column Name Section* :</td>
                                            <td style="text-align: left; width: 70%">
                                                <asp:DropDownList ID="ddlColumnHeader" runat="server" CssClass="dropDownList" TabIndex="2"
                                                    Width="65%" AutoPostBack="false">
                                                    <asp:ListItem Value="A" Text="Attribute Wise"></asp:ListItem>
                                                    <asp:ListItem Value="V" Text="Variable Wise"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <asp:HiddenField ID="hdnColumnHeader" runat="server" Value="" />
                                        </tr>
                                        <tr id="trChkActivity" runat="server" style="display: none;">
                                            <td colspan="2">
                                                <fieldset id="fldsData" runat="server" class="FieldSetBox" style="width: 85%; margin: auto; margin-top: 10px;">
                                                    <legend id="lblData" runat="server" class="LegendText" style="color: black; font-weight: bolder; font-size: 12px; text-align: left;">
                                                        <img id="img3" alt="Activities" src="images/panelcollapse.png"
                                                            onclick="Display(this,'divData');" runat="server" style="margin-right: 2px;" />Activities</legend>
                                                    <asp:HiddenField ID="hdnActivityID" runat="server" Value="" />
                                                    <asp:HiddenField ID="hdnNodeId" runat="server" Value="" />
                                                    <div id="divData">
                                                        <asp:Panel ID="Panel1" runat="server" Style="max-height: 100px" ScrollBars="Auto"
                                                            BorderColor="#184E8A" BorderWidth="0px">
                                                            <table style="width: 100%; margin: auto;" id="tblAllActivity" runat="server">
                                                                <tr>
                                                                    <td runat="server" id="tdAllActivity"></td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </div>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr id="trChkSubject" runat="server" style="display: none;">
                                            <td colspan="2">
                                                <fieldset id="fldsSubject" runat="server" class="FieldSetBox" style="width: 85%; margin: auto; margin-top: 10px;">
                                                    <legend id="lblSubject" runat="server" class="LegendText" style="color: black; font-weight: bolder; font-size: 12px; text-align: left;">
                                                        <img id="img4" alt="Activities" src="images/panelcollapse.png"
                                                            onclick="Display(this,'divSubject');" runat="server" style="margin-right: 2px;" />Subject</legend>
                                                    <div id="divSubject">
                                                        <asp:HiddenField ID="hdnSubjectID" runat="server" Value="" />
                                                        <asp:Panel ID="pnlFields" runat="server" Style="max-height: 100px" ScrollBars="Auto"
                                                            BorderColor="#184E8A" BorderWidth="0px">
                                                            <table style="width: 100%; margin: auto;" id="tblSubject" runat="server">
                                                                <tr>
                                                                    <td runat="server" id="tdSubject"></td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </div>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: center;" colspan="2" class="Label">
                                                <asp:Button ID="btnSearch" runat="server" CssClass="btn btnnew" OnClick="btnSearch_Click"
                                                    OnClientClick="return Validation();" TabIndex="4" Text="Search" ToolTip="Search" />
                                                <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" OnClick="btnCancel_Click"
                                                    Text="Cancel" ToolTip="Cancel" />
                                                <asp:Button ID="btnClose" runat="server" CssClass="btn btnexit" OnClick="btnClose_Click"
                                                    OnClientClick="return msgconfirmalert('Are You Sure You Want To Exit?',this);" TabIndex="5"
                                                    Text="Exit" ToolTip="Exit" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </fieldset>
                    </td>
                </tr>
            </table>
            <table cellpadding="5px" style="width: 100%;">
                <tr>
                    <td>
                        <fieldset class="FieldSetBox" style="display: block; width: 96%; text-align: left; border: #aaaaaa 1px solid;">
                            <legend class="LegendText" style="color: Black; font-size: 12px">
                                <img id="img2" alt="Project Details" src="images/panelcollapse.png"
                                    onclick="Display(this,'divProjectDetail');" runat="server" style="margin-right: 2px;" />Project Activity Data</legend>
                            <div id="divProjectDetail">
                                <table width="100%">
                                    <tbody>
                                        <tr style="width: 90%; margin: 0 auto;">
                                            <td style="width: 100%; text-align: center; text-align: center;" colspan="2">
                                                <asp:Panel ID="pnlMedExInfoHdr" runat="server" ScrollBars="Auto" Style="min-width: 60%; text-align: center; margin: auto; max-height: 400px; margin-top: 1%;">
                                                    <asp:GridView ID="gvwMedExInfoHdr" runat="server" AutoGenerateColumns="True" EmptyDataRowStyle-HorizontalAlign="Center"
                                                        Style="font-size: large; margin: auto;" TabIndex="6" SkinID="grdViewSmlAutoSize">
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </fieldset>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlActivity" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Button ID="btnExport" runat="server" CssClass="btn ExcellButtonShoHide"
        ToolTip="Export To Excel" Text="Export"/>
    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <script type="text/javascript" language="javascript">
        var IsChecked = false;
        var oTab = "";
        var oTab = "";
        function pageLoad()
        {
            $(<%= pnlMedExInfoHdr.ClientID%>).css("max-width", $(window).width() - 73);
            IsChecked = false;
            if(typeof <%= gvwMedExInfoHdr.ClientID%> != "undefined"){
                if ($(<%= gvwMedExInfoHdr.ClientID%>).find("tr").length > 0) {
                    $(<%= btnExport.ClientID%>).removeClass("ExcellButtonShoHide")
                } else {
                    $(<%= btnExport.ClientID%>).addClass("ExcellButtonShoHide")
                }
            }
            else{
                $(<%= btnExport.ClientID%>).addClass("ExcellButtonShoHide")
            }
            
        }
        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.ClientID%>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }
        function checkValidation(ele) {
            if (document.getElementById('<%=HProjectId.ClientID%>').value == "") {
                msgalert("Please Select Project.");
                document.getElementById('<%=txtProject.ClientID%>').focus();
                return false;
            }
            else if ((document.getElementById('<%=ddlPeriod.ClientID%>') != null)?document.getElementById('<%=ddlPeriod.ClientID%>').selectedIndex == 0 && document.getElementById('<%=trPeriod.ClientID%>').style.display != "none":false) {
                msgalert("Please Select Period.");
                document.getElementById('<%=ddlPeriod.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=ddlVisit.ClientID%>').selectedIndex == 0) {
                msgalert("Please Select Visit/Parent Activity.");
                document.getElementById('<%=ddlVisit.ClientID%>').focus();
                return false;
            }
            if (document.getElementById('<%=ddlActivity.ClientID%>').selectedIndex == 0) {
                    msgalert("Please Select Activity.");
                    document.getElementById('<%=ddlActivity.ClientID%>').focus();
                    return false;
                }
                __doPostBack('ctl00$CPHLAMBDA$chkAllActivity','')
            }
        function Validation() {
            var iSubjectNo = "";
            var delimiter = "";
            if($(".rblSubject input[type='radio']:checked").val() == "Y"){
                $(".chkSubject input:checked").each(function (ele) {
                    iSubjectNo += delimiter + $(this).parent().attr("SubjectId");
                    delimiter = ",";
                });
            }
            else{
                iSubjectNo = "0000";
            }
            if (document.getElementById('<%=HProjectId.ClientID%>').value == "") {
                msgalert("Please Select Project.");
                document.getElementById('<%=txtProject.ClientID%>').focus();
                return false;
            }
            else if ((document.getElementById('<%=ddlPeriod.ClientID%>') != null)?document.getElementById('<%=ddlPeriod.ClientID%>').selectedIndex == 0 &&  document.getElementById('<%=trPeriod.ClientID%>').style.display != "none":false) {
                msgalert("Please Select Period.");
                document.getElementById('<%=ddlPeriod.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=ddlVisit.ClientID%>').selectedIndex == 0) {
                msgalert("Please Select Visit/Parent Activity.");
                document.getElementById('<%=ddlVisit.ClientID%>').focus();
                return false;
            }

            if (document.getElementById('<%=ddlActivity.ClientID%>').selectedIndex == 0) {
                msgalert("Please Select Activity.");
                document.getElementById('<%=ddlActivity.ClientID%>').focus();
                return false;
            }
            else if ($(<%=chkAllActivity.ClientID%>).attr("checked") == "checked") {
                var iNodeNo = "";
                var vActivityId = "";
                var delimiter = "";
                var checkID = "";
                $(".chkActivity input:checked").each(function (ele) {
                    iNodeNo += delimiter + $(this).parent().attr("nodeid").split("#")[0];
                    checkID = $(this).parent().attr("nodeid").split("#")[1];
                    if(vActivityId.split(",").indexOf(checkID) == -1){
                        vActivityId += delimiter + checkID;
                    }
                    delimiter = ",";
                });
                if (iNodeNo == "") {
                    msgalert("Please Select Activity.");
                    return false;
                }
                $(<%=hdnNodeId.ClientID%>).val(iNodeNo);
                $(<%=hdnActivityID.ClientID%>).val(vActivityId);
            }
            else{
                $(<%=hdnNodeId.ClientID%>).val($(<%=ddlActivity.ClientID%>).val().split("#")[0]);
                $(<%=hdnActivityID.ClientID%>).val($(<%=ddlActivity.ClientID%>).val().split("#")[1]);
            }
            
            if (iSubjectNo == "") {
                msgalert("Please Select Subject.");
                return false;
            }
            else {
                $(<%=hdnSubjectID.ClientID%>).val(iSubjectNo);
            }
            $(<%=hdnColumnHeader.ClientID%>).val($(<%=ddlColumnHeader.ClientID%>).val());
            return true;
        }
        function IsSubjectSelect(ele) {
            var element = $(ele).closest("table").find("input");
            var IsCheckedTemp = false;
            IsChecked = false;
            $.each(element, function (index, ele) {
                if (ele.checked) {
                    IsCheckedTemp = true;
                }
            });
            if (IsCheckedTemp) {
                IsChecked = true;
            }
        }
        function fnSelectAll(Ctrl) {
            if (Ctrl.checked)
                $($(Ctrl).closest('table')[0]).find(':checkbox').prop("checked", true);
            else
                $($(Ctrl).closest('table')[0]).find(':checkbox:checked').prop("checked", false);
        }
        function fnCheckSelect(Ctrl) {
            if (Ctrl.checked) {
                if ($($(Ctrl).closest('table')[0]).find(".chkSelectUnSelect input:checkbox").length ==
                    $($(Ctrl).closest('table')[0]).find(".chkSelectUnSelect input:checkbox:checked").length) {
                    $($($(Ctrl).closest('table')[0]).find(":checkbox")[0]).prop("checked", true);
                }
            } 
            else{
                if ($($(Ctrl).closest('table')[0]).find(".chkSelectUnSelect input:checkbox").length !=
                    $($(Ctrl).closest('table')[0]).find(".chkSelectUnSelect input:checkbox:checked").length) {
                    $($($(Ctrl).closest('table')[0]).find(":checkbox")[0]).prop("checked", false);
                }
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
    </script>
</asp:Content>
