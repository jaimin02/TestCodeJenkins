<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="frmEditChecksExecution_New.aspx.vb" Inherits="frmEditChecksExecution_New" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <style type="text/css">
        table.dataTable td {
            padding: 3px 11px;
        }

        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

        table.dataTable thead th {
            padding: 0px !important;
        }
    </style>

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <%--<script type="text/javascript" src="http://cdn.datatables.net/1.10.0/js/jquery.dataTables.js"></script>--%>

    <asp:UpdatePanel runat="server" ID="UpControls" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlHeader" runat="server" Style="margin-top: 1%; width: 100%;">

                <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                    <legend class="LegendText" style="color: Black; font-size: 12px">
                        <img id="img2" alt="Edit Check Criteria" src="images/panelcollapse.png"
                            onclick="Display(this,'divEditCheckCriteria');" runat="server" style="margin-right: 2px;" />Edit Check Criteria</legend>

                    <%-- <div id="divExpandable1" runat="server" style="font-weight: bold; font-size: 14px; float: none; color: white; background-color: #1560a1; width: 95%; margin: auto; height: 18px;">
                    <div>
                        <asp:Image ID="imgArrows" runat="server" Style="float: left;" />
                    </div>
                    <div style="text-align: left; margin-left: 1%;">
                        <asp:Label ID="Label1" runat="server" Text="Edit Check Criteria">
                        </asp:Label>
                    </div>
                </div>--%>

                    <div id="divEditCheckCriteria">
                        <asp:Panel ID="pnlProject" runat="server" Width="100%">
                            <table width="95%" cellpadding="3" style="margin: auto;">
                                <tr>
                                    <td width="30%" class="Label" style="text-align: right;">Project Name/Request Id* :
                                    </td>
                                    <td width="70%" style="text-align: left;">
                                        <asp:TextBox ID="txtProject" runat="server" Width="50%"></asp:TextBox>
                                        <asp:Button Style="display: none" ID="btnSetProject" runat="server" Text=" Project"></asp:Button><asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                                            TargetControlID="txtProject" ServicePath="AutoComplete.asmx" ServiceMethod="GetMyProjectCompletionList"
                                            OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1"
                                            CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                            CompletionListCssClass="autocomplete_list" CompletionListElementID="pnlTemplate"
                                            BehaviorID="AutoCompleteExtender1">
                                        </cc1:AutoCompleteExtender>
                                        <asp:Panel ID="pnlTemplate" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="30%" class="Label" style="text-align: right;">Edit Check Operation :
                                    </td>
                                    <td width="70%" style="text-align: left;">
                                        <asp:RadioButtonList ID="rblEditCheckOperation" runat="server" RepeatDirection="Horizontal" ToolTip="Edit Check Operation"
                                            AutoPostBack="true">
                                            <asp:ListItem Value="V" Text="Report" />
                                            <asp:ListItem Value="E" Text="Execute" />

                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="30%" class="Label" style="text-align: right;">Edit Check Type :
                                    </td>
                                    <td width="70%" style="text-align: left;">
                                        <asp:RadioButtonList ID="rblEditCheckType" runat="server" RepeatDirection="Horizontal" ToolTip="Inclusion Or Exclusion Criteria"
                                            AutoPostBack="true">
                                            <asp:ListItem Value="P" Text="Within Activity" />
                                            <asp:ListItem Value="C" Text="Cross Activity" />
                                            <asp:ListItem Value="P,C" Text="Both" />
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td width="30%" class="Label" style="text-align: right; display: none">Activity* :
                                    </td>
                                    <td width="70%" style="text-align: left; display: none">
                                        <asp:CheckBox ID="chkAll" Text="All" runat="server" class="checkbox"
                                            Style="vertical-align: middle;" onClick="return CheckAll(this);" />
                                        <asp:Panel ID="pnlActivity" runat="server" BackColor="White" BorderColor="Navy" BorderWidth="1px"
                                            ScrollBars="Auto" Style="max-height: 120px; max-width: 70%; min-height: 15px;">
                                            <asp:CheckBoxList ID="chkLstActivity" CssClass="CheckBoxActivity" Style="width: 100%" runat="server" RepeatColumns="4"
                                                RepeatDirection="Horizontal" onclick="chkHeaderCheckBox();">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                    </td>

                                    <td valign="top" style="align: right">
                                        <fieldset id="divSubject" runat="server" class="FieldSetBox" style="overflow: auto; display: none; max-height: 230px;"
                                            tabindex="5">
                                            <asp:TreeView ID="tvSubject" Width="10px" Height="15px" ShowCheckBoxes="All" BorderColor="DarkGreen"
                                                Font-Bold="True" Font-Size="X-Small" runat="server">
                                            </asp:TreeView>
                                            <asp:HiddenField ID="SubjectCount" runat="server" />
                                        </fieldset>
                                    </td>
                                    <td colspan="3" valign="top">
                                        <fieldset id="divActivity" runat="server" class="FieldSetBox" style="overflow: auto; display: none; max-height: 230px;"
                                            tabindex="6">
                                            <asp:TreeView ID="tvActivity" runat="server" BorderColor="DarkGreen" Font-Bold="True"
                                                Font-Size="X-Small" Height="250px" ShowCheckBoxes="All" Width="100px">
                                            </asp:TreeView>
                                            <asp:HiddenField ID="ActivityCount" runat="server" />
                                        </fieldset>
                                    </td>


                                </tr>
                                <tr>
                                    <td style="text-align: center;" colspan="2">
                                        <asp:Button ID="btnExecute" class="btn btnsave" Text="Execute" runat="server" OnClientClick="return Validation('E');" />
                                        <asp:Button ID="btnView" class="btn btnnew" Text="View" runat="server" OnClientClick="return Validation('V');" />
                                        <asp:Button ID="btnCancel" class="btn btncancel" Text="Cancel" runat="server" OnClientClick="return ClearData();" />
                                        <asp:Button ID="btnExport" class="btn btnexcel" Visible="false"
                                            runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                </fieldset>
            </asp:Panel>
            <%--<cc1:CollapsiblePanelExtender ID="Collpase1" runat="server" TargetControlID="pnlProject"
                ExpandControlID="pnlHeader" CollapseControlID="pnlHeader" ExpandedImage="~/Images/minus.png"
                CollapsedImage="~/Images/add.png" ImageControlID="imgArrows" AutoCollapse="false"
                AutoExpand="false">
            </cc1:CollapsiblePanelExtender>--%>
            <asp:Panel ID="PnlDetail" runat="server" Style="margin-top: 3% ! important; display: none; width: 97%; margin: 0px auto;">

                <fieldset class="FieldSetBox" style="display: block; width: 100%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                    <legend class="LegendText" style="color: Black; font-size: 12px">
                        <img id="img1" alt="Edit Check Details" src="images/panelcollapse.png"
                            onclick="Display(this,'divDetail');" runat="server" style="margin-right: 2px;" />Edit Check Details</legend>

                    <%-- <div id="divDetail" runat="server" style="font-weight: bold; font-size: 14px; float: none; color: white; background-color: #1560a1; width: 100%; margin: auto; height: 18px;">
                    <div>
                        <asp:Image ID="imgArrowsDetail" runat="server" Style="float: left;" />
                    </div>
                    <div style="text-align: left; margin-left: 1%;">
                        <asp:Label ID="Label2" runat="server" Text="Edit Check Details">
                        </asp:Label>
                    </div>
                </div>--%>

                    <div id="divDetail">
                    <asp:Panel ID="pnlDetailGrid" runat="server" Style="display: none; width: 100%; margin: 2px auto; overflow-y: hidden">

                        <table id="tblGrid " width="95%" cellpadding="5px" style="margin: auto;">
                            <tr>
                                <td width="95%" colspan="2">
                                    <asp:Panel ID="pnl1" runat="server" Style="width: 100%; margin: auto; margin-top: 2%;">
                                        <div style="width: 95%; text-align: right;">
                                            <asp:Button ID="btnResolveAll" CssClass="btn btnnew" runat="server" Text="Resolve"
                                                Visible="false" />
                                        </div>
                                        <table width="100%" cellpadding="0px" cellspacing="0px">
                                            <tr>
                                                <td style="height: 10px;"></td>
                                            </tr>
                                        </table>
                                        <div id="divGrid" style="width: 100%; overflow: auto; overflow-y: hidden; margin: auto;">
                                            <%--<asp:GridView ID="Grid" SkinID="grdViewSmlAutoSize" runat="server" AutoGenerateColumns="false"
                                        AllowPaging="true" PageSize="10" Width="130%" PagerSettings-Position="TopAndBottom">--%>

                                            <asp:GridView ID="Grid"  runat="server" AutoGenerateColumns="false" AllowPaging="false" Width="100%" Style="font-size: 12px;">

                                                <Columns>
                                                    <asp:BoundField DataFormatString="number" HeaderText="Sr. No" ItemStyle-Width="2%">
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vSubjectId" HeaderText="Subject Id" />
                                                    <asp:BoundField DataField="vProjectNo" HeaderText="Project No" ItemStyle-Width="7%" />
                                                    <asp:BoundField DataField="vMySubjectNo" HeaderText="Subject No" ItemStyle-Width="7%" ItemStyle-HorizontalAlign="Center"
                                                        ItemStyle-VerticalAlign="Middle" />

                                                    <asp:BoundField DataField="vRandomizationNo" HeaderText="Randomization No"
                                                        ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" />

                                                    <asp:BoundField DataField="RepetitionNo" HeaderText="Repeat No" ItemStyle-HorizontalAlign="Center"
                                                        ItemStyle-VerticalAlign="Middle" />

                                                    <asp:BoundField DataField="ParentvNodeDisplayName" HeaderText="Parent Activity" ItemStyle-Width="7%" />

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
                                                    <asp:BoundField DataField="vQueryMessage" HeaderText="Edit Check Formula" ItemStyle-Width="3%" />
                                                    <asp:BoundField DataField="vErrorMessage" HeaderText="Discrepancy Message" ItemStyle-Width="7%" />
                                                    <asp:BoundField DataField="cEditCheckTypeDisplay" HeaderText="Edit Check Type" />

                                                    <asp:BoundField DataField="vFiredBy" HeaderText="Executed By" />
                                                    <asp:BoundField DataField="vFiredDate" HeaderText="Executed On" />
                                                    <asp:BoundField DataField="cIsQuery" HeaderText="cIsQuery" />
                                                    <asp:BoundField DataField="nCRFDtlNo" HeaderText="nCRFdTLNo" />
                                                    <asp:BoundField DataField="vResolvedBy" HeaderText="Resolved By" />
                                                    <asp:BoundField DataField="vResolvedOn" HeaderText="Resolved On" ItemStyle-Width="7%" />
                                                    <asp:TemplateField HeaderText="Reason/Remark" ItemStyle-Width="5%">
                                                        <HeaderTemplate>
                                                            All Pages 
                                                    <asp:CheckBox ID="chkAllPages" runat="server" OnCheckedChanged="chkAllResolvedRemarks_CheckedChanged" AutoPostBack="true" />
                                                            </br>
                                                     Page Wise 
                                                    <asp:CheckBox ID="chkAllResolvedRemarks" runat="server" OnCheckedChanged="chkAllResolvedRemarks_CheckedChanged" AutoPostBack="true" />
                                                        </HeaderTemplate>
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
                                    <div class="modal-content" id="divForRemarks" style="display:none;z-index:999 !important" runat="server" >
                                        <div class="modal-header">
                                            <h4>Reason/Remark To Resolve Edit Check</h4>
                                            <img id="imgClose" title="Close Window" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px;bottom:37px;" onclick="funCloseDiv('divForRemarks');" />
                                        </div>
                                        <div class="modal-body">
                                            <table style="width: 100%; margin-bottom: 2%">
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
                                            </table>
                                        </div>

                                        <div class="modal-header">
                                            <asp:Button ID="btnSaveRemark" runat="server" class="btn btnsave" Text="Save" OnClientClick="return funSaveRemark();" 
                                                     Style="margin-left:45%;font-size:medium;" ToolTip="Save"/>
                                        </div>
                                    </div>

                                    <div id="ModalBackGround" runat="server" class="divModalBackGround" style="display: none;">
                                    </div>
                                </td>
                            </tr>
                        </table>

                    </asp:Panel>
                        </div>
                </fieldset>
            </asp:Panel>

            <%--<cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="pnlDetailGrid"
                ExpandControlID="PnlDetail" CollapseControlID="PnlDetail" ExpandedImage="~/Images/minus.png"
                CollapsedImage="~/Images/add.png" ImageControlID="imgArrowsDetail" AutoCollapse="false"
                AutoExpand="false">
            </cc1:CollapsiblePanelExtender>--%>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Grid" EventName="PageIndexChanging" />
            <asp:AsyncPostBackTrigger ControlID="btnResolveAll" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:HiddenField ID="HFWorkpsaceEditChecksDtlNo" runat="server" />

    <%--<script type="text/javascript" src="Script/jquery.min.js"></script>--%>
    <%--<script src="Script/jquery-2.1.0.min.js" type="text/javascript"></script>--%>
    <%--<script src="Script/jquery.dataTables.fixedColumns.js" type="text/javascript"></script>--%>
    <%--<script src="Script/jquery-1.11.dataTables.min.js" type="text/javascript"></script>--%>

    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>

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

            //if (document.getElementById('<%=chkLstActivity.ClientID%>')) {
            //    var i = 0;
            //    $(".CheckBoxActivity input:checked").each(function () {
            //        i = 1;
            //    });
            //    if (i == 0) { alert("Please Select activity."); return false; }
            //}
            //
            //if (document.getElementById('<%=rblEditCheckType.ClientID%>').selectedIndex <= 0) {
            //    alert('Please Select Edit Check Type');
            //    return false;
            //}
            //if (type != 'E') {
            //    if (document.getElementById('<%= rblEditCheckType.ClientId %>').value == 'V') {
            //        alert('Please Select EditCheck Type');
            //        return false;
            //    }
            //}
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
            BindDatatable()
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

    //Add by shivani for check all checkbox
    $("[id*=tvSubject] input[type=checkbox]").live("click", function () {
        var table = $(this).closest("table");
        var Flag = false;
        var Index = 0;
        var IndexNot = 0;
        if (table.next().length > 0 && table.next()[0].tagName == "DIV") {
            //Is Parent CheckBox
            var childDiv = table.next();
            var isChecked = $(this).is(":checked");
            $("input[type=checkbox]", childDiv).each(function () {
                if (isChecked) {
                    $(this).attr("checked", "checked");
                } else {
                    $(this).removeAttr("checked");
                }
            });
        } else {
            //Is Child CheckBox
            var parentDIV = $(this).closest("DIV");
            if ($("input[type=checkbox]", parentDIV).length == $("input[type=checkbox]:checked", parentDIV).length) {
                $("input[type=checkbox]", parentDIV.prev()).attr("checked", "checked");
            }
            else {
                $("input[type=checkbox]", parentDIV.prev()).removeAttr("checked");
            }
        }
    });

    //Add by shivani for check all checkbox
    $("[id*=tvActivity] input[type=checkbox]").live("click", function () {
        var table = $(this).closest("table");
        var Flag = false;
        var Index = 0;
        var IndexNot = 0;
        var IndexAll = 0;
        if (table.next().length > 0 && table.next()[0].tagName == "DIV") {
            //Is Parent CheckBox
            var childDiv = table.next();
            var isChecked = $(this).is(":checked");
            $("input[type=checkbox]", childDiv).each(function () {
                if (isChecked) {
                    $(this).attr("checked", "checked");
                } else {
                    $(this).removeAttr("checked");
                }
            });
        } else {
            //Is Child CheckBox
            var parentDIV = $(this).closest("DIV");
            $(this).closest("DIV").find("table [type=checkbox]").each(function () {
                if ($(this).attr("checked") == true) {
                    Flag = true;
                    Index = Index + 1;
                }
                if ($(this).attr("checked") == false) {
                    IndexNot = IndexNot + 1;
                }
            });
            if (Flag == true) {
                parentDIV.prev().find("[type=checkbox]").attr("checked", "checked");
            }
            if ($(this).closest("DIV").find("table [type=checkbox]").length == Index) {
                parentDIV.prev().find("[type=checkbox]").removeAttr("checked");
                parentDIV.prev().find("[type=checkbox]").attr("checked", "checked");

            }
            if ($(this).closest("DIV").find("table [type=checkbox]").length == IndexNot) {
                parentDIV.prev().find("[type=checkbox]").removeAttr("checked");
                parentDIV.prev().find("[type=checkbox]").removeAttr("checked");
            }
            $("#ctl00_CPHLAMBDA_tvActivity").find("table [type=checkbox]").each(function () {
                if ($(this).find("table [type=checkbox]").attr("checked") == true) {
                    IndexAll = IndexAll + 1;
                }
            });
            if ($("#ctl00_CPHLAMBDA_tvActivity").find("table [type=checkbox]").length == IndexAll) {
                $("#ctl00_CPHLAMBDA_tvActivity").find("table [type=checkbox]").first().attr("checked", "checked");
            } else {
                $("#ctl00_CPHLAMBDA_tvActivity").find("table [type=checkbox]").first().removeAttr("checked")
            }
        }
    });

    function BindDatatable() {
        oTab = $('#<%= Grid.ClientID%>').prepend($('<thead>').append($('#<%= Grid.ClientID%> tr:first'))).dataTable({
            "bJQueryUI": true,
            "sPaginationType": "full_numbers",
            "bLengthChange": true,
            "iDisplayLength": 10,
            "bProcessing": true,
            "bSort": false,
            "bDestroy": true,
            "oLanguage": {
                "sEmptyTable": "No Record Found",
            },

        });
        return false;
    }

    function ClearData() {

        $('#ctl00_CPHLAMBDA_txtProject').val('');
        $("table[id$=rblEditCheckOperation] input:radio").each(function (i, x) {
            if ($(x).is(":checked")) {
                $(x).removeAttr("checked");
            }
        });
        $("table[id$=rblEditCheckType] input:radio").each(function (i, x) {
            if ($(x).is(":checked")) {
                $(x).removeAttr("checked");
            }
        });
        var tvSubject = "[id*=tvSubject] input[type=checkbox]";
        $(tvSubject).removeAttr("checked");

        var tvActivity = "[id*=tvActivity] input[type=checkbox]";
        $(tvActivity).removeAttr("checked");

        document.getElementById("ctl00_CPHLAMBDA_divSubject").style.display = "none";
        document.getElementById("ctl00_CPHLAMBDA_divActivity").style.display = "none";
        //document.getElementById("ctl00_CPHLAMBDA_btnExport").style.display = "none";
        //document.getElementById("ctl00_CPHLAMBDA_pnlDetailGrid").style.display = "none";

        return true;
    }

    function clear_Subject_ACtivity() {
        var tvSubject = "[id*=tvSubject] input[type=checkbox]";
        $(tvSubject).removeAttr("checked");

        var tvActivity = "[id*=tvActivity] input[type=checkbox]";
        $(tvActivity).removeAttr("checked");
        if ($("#ctl00_CPHLAMBDA_btnExport").is(":visible") == true) {
            document.getElementById("ctl00_CPHLAMBDA_btnExport").style.display = "none";
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

    function DisplayFromCodeBehind(control, target) {
        control = document.getElementById(control)
        if (control.src.toString().toUpperCase().search("EXPAND") != -1) {
            $("#" + target).slideToggle(600);
            control.src = "images/panelcollapse.png";
        }
        else {
            $("#" + target).slideToggle(600);
            control.src = "images/panelexpand.png";
        }
    }

    //var table = $('#Grid').DataTable();

    //$('#Grid').bind('ctl00_CPHLAMBDA_Grid_paginate a', function () {
    //    var info = table.page.info();
    //    var page = info.page + 1;
    //    alert('changed - page ' + page + ' out of ' + info.pages + ' is clicked');
    //});


    </script>
</asp:Content>

