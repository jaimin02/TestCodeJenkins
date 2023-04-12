<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmActivityMst.aspx.vb" Inherits="frmAddActivityMst" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

        #ctl00_CPHLAMBDA_gvactivity_wrapper {
            /*margin: 0px 235px;*/
        }

        .comment {
            word-break: break-all;
            word-wrap: break-word;
        }

         .dataTables_wrapper {
            width:1200px;
        }
           #loadingmessage {
        display: none;
        position: fixed;
        z-index: 1000;
        top: 0;
        left: 0;
        height: 100%;
        width: 100%;
        background: rgba( 255, 255, 255, .5 ) url('images/AjaxLoader.gif') 50% 50% no-repeat;
} 
    </style>

      <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <style type="text/css">
        table.maingrid, .table, th {
            border: #add7ea 1px solid;
        }

        .brkwrd td {
            word-break: break-word;
        }
        /* table tr td
        {
            vertical-align: top;
        }*/
    </style>

  

    <script type="text/javascript" language="javascript">

        function HideSponsorDetails() {
            $('#<%= img2.ClientID%>').click();
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

        function UIgvactivity() {
            $('#<%= gvactivity.ClientID%>').removeAttr('style', 'display:block');
            oTab = $('#<%= gvactivity.ClientID%>').prepend($('<thead>').append($('#<%= gvactivity.ClientID%> tr:first'))).dataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": false,
                "sScrollY": "250px",
                "sScrollX": "100%",
                "bScrollCollapse": true,
                aLengthMenu: [
                    [10, 25, 50, 100, -1],
                    [10, 25, 50, 100, "All"]
                ],
            });
            return false;
        }

        function Validation() {
            if (document.getElementById('<%=ddlActivityGroup.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Activity Group !');
                document.getElementById('<%=ddlActivityGroup.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtactivityname.ClientID%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=txtactivityname.ClientID%>').value = '';
                msgalert('Please Enter Activity Name !');
                document.getElementById('<%=txtactivityname.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=ddldeptcode.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Department !');
                document.getElementById('<%=ddldeptcode.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=txtdays.ClientID%>').value.toString().trim().length <= 0) {
                document.getElementById('<%=txtdays.ClientID%>').value = '';
                msgalert('Please Enter Completion Days !');
                document.getElementById('<%=txtdays.ClientID%>').focus();
                return false;
            }
            else if (document.getElementById('<%=ddlMileStone.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Activity Category !');
                document.getElementById('<%=ddlMileStone.ClientID%>').focus();
                return false;
            }
    return true;
}

function OnSubSelected(sender, e) {
    var strvalue = e.get_value();
    strvalue = strvalue.replace('\'', '');

    var arrstrvalue = strvalue.split('#');

    SubjectOnItemSelected(e.get_value(), $get('<%= txtActivity.ClientId %>'),
                $get('<%= HActivityId.ClientId %>'), $get('<%= btnSearchActivity.ClientId %>'));
}

function SubClientPopulated(sender, e) {
    SubjectClientShowing('AutoCompleteExtender1', $get('<%= txtActivity.ClientId %>'));
}

function Numeric() {
    var ValidChars = "0123456789";
    var Numeric = true;
    var Char;

    sText = document.getElementById('<%=txtdays.ClientID%>').value;
    for (i = 0; i < sText.length && Numeric == true; i++) {
        Char = sText.charAt(i);
        if (ValidChars.indexOf(Char) == -1) {
            msgalert('Please Enter Numeric Value !');
            document.getElementById('<%=txtdays.ClientID%>').value = "";
            Numeric = false;
        }
    }

    return Numeric;
}

function togglePanel(option) {
    var pnl = document.getElementById('<%= pnlCollapse.ClientID %>');
    var spanCollapse = document.getElementById('<%= lblCollaps.ClientID %>');
    var spanExpand = document.getElementById('<%= lblExpand.ClientID %>');
    var hfCollaps = document.getElementById('<%= hfCollaps.ClientID %>');

    pnl.style.display = option; //none/block

    spanCollapse.style.display = 'none';
    spanExpand.style.display = 'none';
    if (option == 'block') {
        spanExpand.style.display = 'block';
        hfCollaps.value = 'block'
    }
    else {
        spanCollapse.style.display = 'block';
        hfCollaps.value = 'none'
    }
}

function ValidationOnAdd() {
    if (document.getElementById('<%=ddlRefDocType.ClientID%>').selectedIndex == 0) {
        msgalert('Please Select Reference Document Type !');
        document.getElementById('<%=ddlRefDocType.ClientID%>').focus();
        return false;
    }

    else if (document.getElementById('<%=ddlWorkspace.ClientID%>').selectedIndex == 0) {
        msgalert('Please Select Project !');
        document.getElementById('<%=ddlWorkspace.ClientID%>').focus();
        return false;
    }
    else if (document.getElementById('<%=ddlNode.ClientID%>').selectedIndex == 0) {
        msgalert('Please Select Node !');
        document.getElementById('<%=ddlNode.ClientID%>').focus();
        return false;
    }
    return true;
}
// for fix gridview header aded on 22-nov-2011
function pageLoad() {
    //            FreezeTableHeader($('#<%= gvactivity.ClientID %>'), { height: 250, width: 900 });
    //FreezHeader("<%=gvactivity.ClientID %>");
    fnGetData();
}
    </script>

    <script type="text/javascript" src="Script/scrollablegrid.js"></script>
    <table cellpadding="5px" style="width: 100%;">
        <tr>
            <td>
                <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                    <legend class="LegendText" style="color: Black; font-size: 12px">
                        <img id="img2" alt="Activity/Node Details" src="images/panelcollapse.png"
                            onclick="Display(this,'divActivityDetail');" runat="server" style="margin-right: 2px;" />Activity/Node Details</legend>
                    <div id="divActivityDetail" style="width: 95%; margin: auto;">
                        <table style="width: 95%; padding-top: 10px; margin: auto;">
                            <tr>
                                <td class="Label" style="width: 20%; white-space: nowrap; text-align: right;" nowrap="noWrap">Activity Group* :
                                </td>
                                <td style="width: 30%; text-align: left;">
                                    <asp:DropDownList ID="ddlActivityGroup" runat="server" CssClass="dropDownList" Width="203px" />
                                </td>
                                <td class="Label" style="white-space: nowrap; text-align: right;">Department* :
                                </td>
                                <td style="text-align: left;">
                                    <asp:DropDownList ID="ddldeptcode" runat="server" CssClass="dropDownList" Width="203px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="Label" style="white-space: nowrap; text-align: right;">Activity Name* :
                                </td>
                                <td style="vertical-align: top; text-align: left;">
                                    <asp:TextBox ID="txtactivityname" runat="server" MaxLength="100" Width="203px" CssClass="textBox" />
                                </td>
                                <td class="Label" style="white-space: nowrap; text-align: right;">Completion Days* :
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="txtdays" runat="server" MaxLength="10" CssClass="textBox" onKeydown="return Numeric();" />
                                </td>
                            </tr>
                            <tr>
                                <td class="Label" style="white-space: nowrap; text-align: right;">Doc. Type :
                                </td>
                                <td style="text-align: left;">
                                    <asp:DropDownList ID="ddldoctypecode" runat="server" CssClass="dropDownList" Width="203px" />
                                </td>
                                <td class="Label" style="white-space: nowrap; text-align: right;">Period Specific* :
                                </td>
                                <td style="text-align: left;">
                                    <asp:RadioButtonList ID="RBLPeriod" runat="server" CssClass="RadioButton" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="Y">Yes</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="N">No</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td class="Label" style="white-space: nowrap; text-align: right;">Subject/Multiple Value Flag :
                                </td>
                                <td style="text-align: left;">
                                    <asp:DropDownList ID="ddlsubjectwiseflag" runat="server" CssClass="dropDownList"
                                        Width="203px" />
                                </td>
                                <td class="Label" style="white-space: nowrap; text-align: right;">Activity Category* :
                                </td>
                                <td style="text-align: left;">
                                    <asp:DropDownList ID="ddlMileStone" runat="server" CssClass="dropDownList" Width="203px">
                                        <asp:ListItem Value=" ">Select Activity Category</asp:ListItem>
                                        <asp:ListItem Value="0">None</asp:ListItem>
                                        <asp:ListItem Value="1">Monitoring</asp:ListItem>
                                        <asp:ListItem Value="2">Scheduling</asp:ListItem>
                                        <asp:ListItem Value="3">Monitoring Scheduling</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="Label" style="width: 20%; white-space: nowrap; text-align: right;">Attribute Template :
                                </td>
                                <td style="width: 30%; text-align: left;">
                                    <asp:DropDownList ID="ddlMedExTemplate" runat="server" CssClass="dropDownList" Width="203px" />
                                </td>
                                <td class="Label" style="white-space: nowrap; text-align: right;">Is Repeatable?* :
                                </td>
                                <td style="text-align: left;">
                                    <asp:RadioButtonList ID="rbtnlstIsRepeatable" runat="server" CssClass="RadioButton"
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem Value="Y">Yes</asp:ListItem>
                                        <asp:ListItem Selected="True" Value="N">No</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td style="white-space: nowrap; text-align: right;" class="Label">Ref. Document :
                                </td>
                                <td colspan="3">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 100%; white-space: nowrap; border: navy 0px solid;">
                                                <span id="lblCollaps" runat="server" style="background-image: url('images/expand.jpg'); width: 88%; background-repeat: no-repeat; background-position: right; padding-right: 20px; background-color: #ffcc66;"
                                                    onmouseover="this.style.cursor='hand';" onmouseout="this.style.cursor='default';"
                                                    onclick="togglePanel('block');"><b>Attach Reference Document</b></span><span id="lblExpand"
                                                        runat="server" style="background-image: url('images/collapse.jpg'); width: 88%; background-repeat: no-repeat; background-position: right; padding-right: 20px; background-color: #ffcc66;"
                                                        onmouseover="this.style.cursor='hand';" onmouseout="this.style.cursor='default';"
                                                        onclick="togglePanel('none');"><b>Hide Reference Document</b></span>
                                                <asp:HiddenField ID="hfCollaps" runat="server" Value="none" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%; white-space: nowrap; border-right: navy 0px solid; border-top: navy 0px solid; border-left: navy 0px solid; border-bottom: navy 0px solid;">
                                                <asp:Panel ID="pnlCollapse" runat="server" Height="100px" Width="90%" ScrollBars="Vertical"
                                                    BorderColor="navy" BorderWidth="1px" max-height="250px">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="width: 30%; white-space: nowrap; text-align: right;">Referance Doc Type:
                                                            </td>
                                                            <td style="width: 70%; white-space: nowrap; text-align: left;">
                                                                <asp:DropDownList ID="ddlRefDocType" runat="server" CssClass="dropDownList" Width="203px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 30%; white-space: nowrap; text-align: right;">Project:
                                                            </td>
                                                            <td style="width: 70%; white-space: nowrap; text-align: left;">
                                                                <asp:DropDownList ID="ddlWorkspace" runat="server" CssClass="dropDownList" Width="203px"
                                                                    AutoPostBack="True" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 30%; text-align: right;">Node:
                                                            </td>
                                                            <td style="width: 70%; text-align: left;">
                                                                <asp:DropDownList ID="ddlNode" runat="server" CssClass="dropDownList" Width="203px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" style="margin: auto; text-align:center;">
                                                                <asp:Button ID="BtnAdd" OnClientClick="return ValidationOnAdd();" runat="server"
                                                                    Text="Add" CssClass="btn btnnew" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" style="margin: 0 auto; width: 85%">
                                                                <asp:GridView ID="GVActivityDocLink" runat="server" AutoGenerateColumns="False" Style="width: auto; margin: auto;">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="nActivityDocLinkNo" HeaderText="nActivityDocLinkNo" />
                                                                        <asp:BoundField HeaderText="Sr. No" />
                                                                        <asp:BoundField DataField="vDocTypeCode" HeaderText="DocTypeCode" />
                                                                        <asp:BoundField DataField="vDocTypeName" HeaderText="Doc. Type" />
                                                                        <asp:BoundField DataField="vLinkedWorkSpaceId" HeaderText="ProjectId" />
                                                                        <asp:BoundField DataField="vWorkspaceDesc" HeaderText="Project" />
                                                                        <asp:BoundField DataField="iLinkedNodeId" HeaderText="NodeId" />
                                                                        <asp:BoundField DataField="vLinkedActivityId" HeaderText="ActivityId" />
                                                                        <asp:BoundField DataField="vNodeDisplayName" HeaderText="Activity Name" />
                                                                        <asp:TemplateField HeaderText="Delete">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="ImgDelete" runat="server" ToolTip="Delete" ImageUrl="~/Images/i_delete.gif"
                                                                                    OnClientClick="return confirm('Are You Sure You Want To Exit?')" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="white-space: nowrap; text-align: right;" class="Label">Activity CanStartAfter :
                                </td>
                                <td colspan="3" style="text-align: left;">
                                    <div id="Div_startafter" style="border: gray thin solid; overflow-y: scroll; width: 90%; height: 114px">
                                        <asp:CheckBoxList ID="chklstCanstart" runat="server" RepeatColumns="2" CssClass="checkboxlist"
                                            Font-Size="XX-Small" ForeColor="Black" Height="37px" Width="100%" />
                                    </div>
                                </td>
                            </tr>
                            <tr style="margin: auto; text-align: center;">
                                <td colspan="4">
                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btnsave" Text="Save" OnClientClick="return Validation();" />
                                    <asp:Button ID="btnCancel" runat="server" CausesValidation="False" CssClass="btn btncancel"
                                        OnClick="btnCancel_Click" Text="Cancel" />
                                    <asp:Button ID="btnClose" runat="server" CausesValidation="False" CssClass="btn btnclose"
                                        Text="Exit" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
                                     <asp:Button ID="btnEdit" runat="server" Text="Edit" ToolTip="Edit"   style="display:none"/>
                                </td>
                            </tr>
                        </table>
                    </div>
                </fieldset>
            </td>
        </tr>
    </table>
    <table style="width: 100%;" cellpadding="5px">
        <tbody>
            <tr>
                <td>
                    <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                        <legend class="LegendText" style="color: Black; font-size: 12px">
                            <img id="img1" alt="Activity/Node Data" src="images/panelcollapse.png"
                                onclick="Display(this,'divActivityData');" runat="server" style="margin-right: 2px;" />Activity/Node Data</legend>
                        <div id="divActivityData" style="width: 95%; margin: auto;">
                            <table style="width: 95%; margin: auto;">
                                <tr>
                                    <td class="Label" style="display: none;">Search Activity:
                <asp:TextBox ID="txtActivity" runat="server" Width="250" CssClass="textBox" />
                                        <asp:Button ID="btnViewAllRec" runat="server" Text=" View All " CssClass="btn btnnew" />
                                        <asp:Button ID="btnSearchActivity" runat="server" Text=" Search " Style="display: none;" />
                                        <asp:HiddenField ID="HActivityId" runat="server" />
                                        <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionListCssClass="autocomplete_list"
                                            CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                            UseContextKey="True" ServiceMethod="GetActivityCompletionList" MinimumPrefixLength="1"
                                            BehaviorID="AutoCompleteExtender1" OnClientItemSelected="OnSubSelected" OnClientShowing="SubClientPopulated"
                                            TargetControlID="txtActivity" ServicePath="AutoComplete.asmx"
                                            CompletionListElementID="pnlActivityList">
                                        </cc1:AutoCompleteExtender>
                                        <asp:Panel ID="pnlActivityList" runat="server" Style="max-height: 300px; overflow: auto; overflow-x: hidden;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <%--<div style="width: 100%;">--%>
                                                <asp:GridView ID="gvactivity" runat="server" Style="display: none;" AutoGenerateColumns="False"
                                                    OnRowCommand="gvactivity_RowCommand" OnRowDataBound="gvactivity_RowDataBound"
                                                    OnPageIndexChanging="gvactivity_PageIndexChanging">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="#">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vActivityName" HeaderText="Activity Name">
                                                            <ItemStyle HorizontalAlign="Left" CssClass="comment" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vActivityGroupName" HeaderText="Activity Group Name" />
                                                        <asp:BoundField DataField="vDocTypeName" HeaderText="Doc.Type Name">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="cSubjectWiseFlag" HeaderText="SubjectWise Flag">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vDeptName" HeaderText="Dept. Name">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="cPeriodSpecific" HeaderText="Period Specific" />
                                                        <asp:BoundField DataField="nMilestone" HeaderText="Milestone" />
                                                        <asp:BoundField DataField="vCanStartAfter" HeaderText="Activity CanStart After">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vTemplateName" HeaderText="Attribute Template">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField SortExpression="status" HeaderText="Edit">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImbEdit" runat="server" ImageUrl="~/images/Edit2.gif" ToolTip="Edit" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="status" HeaderText="Delete">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgDelete" runat="server" ToolTip="Delete" ImageUrl="~/Images/i_delete.gif"
                                                                    OnClientClick="return confirm('Are You Sure You Want To Delete?')" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="status" Visible="False" HeaderText="Id">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblId" runat="server" Text='<%# DataBinder.Eval(Container.Dataitem,"vActivityId") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="status" Visible="False" HeaderText="Code">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDocCode" runat="server" Text='<%# DataBinder.Eval(Container.Dataitem,"vDocTypeCode") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                                   <div id ="createtable"> 
                                                       </div>
                                                <%--</div>--%>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSearchActivity" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnViewAllRec" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </fieldset>
                </td>
            </tr>
        </tbody>
    </table>
      <div id='loadingmessage' style='display:none'>
                
                </div>
      <asp:HiddenField ID="hdnEditedId" runat="server" />
    <script type="text/javascript" language="javascript">
        var hfCollaps = document.getElementById('<%= hfCollaps.ClientID %>');
        togglePanel(hfCollaps.value);

        //        function FreezHeader(grid) {
        //            var GridId = grid;
        //            var ScrollHeight = 300;
        //            var grid = document.getElementById(GridId);
        //            var gridWidth = grid.offsetWidth;
        //            var gridHeight = grid.offsetHeight;
        //            var headerCellWidths = new Array();
        //            for (var i = 0; i < grid.getElementsByTagName("TH").length; i++) {
        //                headerCellWidths[i] = grid.getElementsByTagName("TH")[i].offsetWidth;
        //            }
        //            grid.parentNode.appendChild(document.createElement("div"));
        //            var parentDiv = grid.parentNode;

        //            var table = document.createElement("table");
        //            for (i = 0; i < grid.attributes.length; i++) {
        //                if (grid.attributes[i].specified && grid.attributes[i].name != "id") {
        //                    table.setAttribute(grid.attributes[i].name, grid.attributes[i].value);
        //                }
        //            }
        //            table.style.cssText = grid.style.cssText;
        //            table.style.width = gridWidth + "px";
        //            table.appendChild(document.createElement("tbody"));
        //            table.getElementsByTagName("tbody")[0].appendChild(grid.getElementsByTagName("TR")[0]);
        //            var cells = table.getElementsByTagName("TH");

        //            var gridRow = grid.getElementsByTagName("TR")[0];
        //            for (var i = 0; i < cells.length; i++) {
        //                var width;

        //                if (headerCellWidths[i] > gridRow.getElementsByTagName("TD")[i].offsetWidth) {
        //                    width = headerCellWidths[i];
        //                }
        //                else {
        //                    width = gridRow.getElementsByTagName("TD")[i].offsetWidth;
        //                }
        //                cells[i].style.width = parseInt(width - 1) + "px";
        //                gridRow.getElementsByTagName("TD")[i].style.width = parseInt(width - 1) + "px";
        //            }
        //            parentDiv.removeChild(grid);

        //            var dummyHeader = document.createElement("div");
        //            dummyHeader.appendChild(table);
        //            parentDiv.appendChild(dummyHeader);
        //            var scrollableDiv = document.createElement("div");
        //            if (parseInt(gridHeight) > ScrollHeight) {
        //                gridWidth = parseInt(gridWidth) + 26;
        //            }
        //            scrollableDiv.style.cssText = "overflow:auto;height:" + ScrollHeight + "px;width:" + gridWidth + "px";
        //            scrollableDiv.appendChild(grid);
        //            parentDiv.appendChild(scrollableDiv);
        //        }
        
        function ShowAlert(msg) {
            
            alertdooperation(msg, 1, "frmActivityMst.aspx?mode=1");
            //alert(msg);
            //window.location.href = "frmUserMaster.aspx?mode=1";
        }
        function fnGetData() {
            debugger;
            $('#loadingmessage').show();
            $.ajax({
                type: "post",
                serverSide: true,
                url: "frmActivityMst.aspx/View_Activitymst",
                //data: '{"vWorkSpaceId":"' + vWorkSpaceId + '","vType":"' + vType + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: true,
                dataType: "json",
                success: function (data) {
                    var data = data.d;
                    var msgs = JSON.parse(data);
                    summarydata = msgs;
                    if (summarydata == "") return false;
                    // Table = Object(keys(summarydata))[0];
                    CreateSummaryTable(summarydata);
                    $('#loadingmessage').hide();

                },
                failure: function (response) {
                    msgalert("failure");
                    msgalert(data.d);
                },
                error: function (response) {
                    msgalert("error");
                }
            });

        }
        function CreateSummaryTable(summarydata) {

            var ActivityDataset = [];
            var jsondata = summarydata.Table;




            for (var Row = 0; Row < jsondata.length; Row++) {

                if (jsondata[Row]['cPeriodSpecific'] == "Y") {
                    jsondata[Row]['cPeriodSpecific'] = "Yes"
                }
                else if (jsondata[Row]['cPeriodSpecific'] == "N") {
                    jsondata[Row]['cPeriodSpecific'] = "No"
                }
                if (jsondata[Row]['cSubjectWiseFlag'] == "T" || jsondata[Row]['cSubjectWiseFlag'] == "Y") {
                    jsondata[Row]['cSubjectWiseFlag'] = "Yes"
                }
                else if (jsondata[Row]['cSubjectWiseFlag'] == "N" || jsondata[Row]['cSubjectWiseFlag'] == "F") {
                    jsondata[Row]['cSubjectWiseFlag'] = "No"
                }
                else if (jsondata[Row]['cSubjectWiseFlag'] == "V") {
                    jsondata[Row]['cSubjectWiseFlag'] = "View"
                }
                if (jsondata[Row]['nMilestone'] == "" || jsondata[Row]['nMilestone'] == "0" || jsondata[Row]['nMilestone'] == "4") {
                    jsondata[Row]['nMilestone'] = "None"
                }
                else if (jsondata[Row]['nMilestone'] == "1") {
                    jsondata[Row]['nMilestone'] = "Monitoring "
                }
                else if (jsondata[Row]['nMilestone'] == "2") {
                    jsondata[Row]['nMilestone'] = "Scheduling "
                }
                else if (jsondata[Row]['nMilestone'] == "3") {
                    jsondata[Row]['nMilestone'] = "Monitoring & Scheduling "
                }
                var InDataset = [];
                InDataset.push(Row+1, jsondata[Row]['vActivityName'], jsondata[Row]['vActivityGroupName'], jsondata[Row]['vDocTypeName'],
                              jsondata[Row]['cSubjectWiseFlag'], jsondata[Row]['vDeptName'], jsondata[Row]['cPeriodSpecific'],
                              jsondata[Row]['nMilestone'], jsondata[Row]['vCanStartAfter'], jsondata[Row]['vTemplateName'],
                              "", "", jsondata[Row]['vActivityId']);
                ActivityDataset.push(InDataset);

            }

            $ = jQuery;
            var createtable1 = $("<table id='Activityrecord'  border='1'  class='display'  cellspacing='0'  width='1200px'> </table>");
            $("#createtable").empty().append(createtable1);

            $('#Activityrecord').DataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": false,
                "sScrollY": "250px",
                "sScrollX": "100%",
                "bScrollCollapse": true,
                "aaData": ActivityDataset,
                "fnCreatedRow": function (nRow, aData, iDataIndex) {
                    //$('td:eq(15)', nRow).append("<['vActivityName'] = '" + aData[0] + "', ['vUserTypeName'] = '" + aData[1] + "', ['vDeptName'] = '" + aData[2] + "', ['vLocationName'] = '" + aData[3] + "', ['vOperationName'] = '" + aData[4] + "'>");
                    $('td:eq(10)', nRow).append("<input type='image' id='ImbEdit" + iDataIndex + "' name='ImbEdit$" + iDataIndex + "' src='images/Edit2.gif';  tempval='" + aData[12] + "';   OnClick='Editcellrow(this); return false;' style='border-width:0px;'>");
                    $('td:eq(11)', nRow).append("<input type='image' id='ImgDelete" + iDataIndex + "' name='ImgDelete$" + iDataIndex + "' src='images/i_delete.gif';  tempval='" + aData[12] + "';   OnClick='DeleteCellRow(this); return false;'  style='border-width:0px;' >");

                },
                //"fnRowCallback": function (nRow, aData, iDisplayIndex) {
                //    $("td:first", nRow).html(iDisplayIndex + 1);
                //    return nRow;
                //},
                "aoColumns": [

                                              { "sTitle": "#" },
                                              { "sTitle": "Activity Name" },
                                              { "sTitle": "Activity Group Name" },

                                              { "sTitle": "Doc.Type Name" },
                                              { "sTitle": "SubjectWise Flag" },
                                              { "sTitle": "Dept. Name" },
                                              { "sTitle": "Period Specific" },


                                              { "sTitle": "Milestone" },

                                              { "sTitle": "Activity CanStart After" },

                                              { "sTitle": "Attribute Template" },

                                              { "sTitle": "Edit" },
                                              { "sTitle": "Delete" },




                ],

                "columns": [
                    null, null, null, null, null, null, null, null, null, null, null, null, null
                ],
                "oLanguage": {
                    "sEmptyTable": "No Record Found"
                },
            });

            $('#Activityrecord').show();



        }
        function Editcellrow(e) {

            var id = e.attributes.tempval.value;

            $('#ctl00_CPHLAMBDA_hdnEditedId').val(id);


            $('#ctl00_CPHLAMBDA_btnEdit').click();

        }
        var r = '';
        function ConfirmDelete() {

            //r = msgconfirmalert('Are you sure you want to Delete?', this);

            swal({
                title: "",
                text: "Are You Sure You Want To Delete ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: '#EB7140',
                confirmButtonText: '',
                closeOnConfirm: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    r = true;
                    swal.close();
                    return false;
                } else {
                    swal.close();
                    return true;
                }
            });



        }
        function DeleteCellRow(e) {
            debugger;
            var id = e.attributes.tempval.value;

            swal({
                title: "",
                text: "Are You Sure You Want To Delete ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: '#EB7140',
                confirmButtonText: '',
                closeOnConfirm: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    $('#loadingmessage').show();
                    $.ajax({
                        type: "post",
                        url: "frmActivityMst.aspx/Delete_ActivityMst",
                        data: "{'id':'" + id + "'}",
                        contentType: "application/json; charset=utf-8",
                        datatype: JSON,
                        async: true,
                        dataType: "json",
                        success: function (data) {
                            var data = data.d;
                            var msgs = JSON.parse(data);
                            summarydata = msgs;
                            if (summarydata == "") return false;
                            // Table = Object(keys(summarydata))[0];
                            //alertdooperation("Test", 4, "frmUserTypeMst.aspx?mode=1");
                            CreateSummaryTable(summarydata);
                            $('#loadingmessage').hide();
                            msgalert("Record Deleted Sucessfully!!");
                            
                            

                        },
                        failure: function (response) {
                            msgalert("failure");
                            msgalert(data.d);
                        },
                        error: function (response) {
                            msgalert("error");
                        }
                    });
                    swal.close();
                    return false;
                } else {
                    swal.close();
                    return true;
                }
            });

           
        }
    </script>

</asp:Content>
