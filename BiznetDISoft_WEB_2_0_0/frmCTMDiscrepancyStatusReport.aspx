
<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmCTMDiscrepancyStatusReport.aspx.vb" Inherits="frmCTMDiscrepancyStatusReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <style type="text/css">
        .ui-multiselect {
            max-height: 35px;
            overflow: auto;
            overflow-x: hidden;
            white-space: nowrap;
        }

        .ui-multiselect-menu {
            width: 430px !important;
            font-size: 0.8em !important;
        }

            .ui-multiselect-menu span {
                vertical-align: top;
            }

        .ui-menu .ui-menu-item a {
            font-size: 11px !important;
            text-align: left !important;
        }

        .ui-autocomplete {
            font-family: Delicious, sans-serif !important;
            max-height: 200px;
            overflow-y: auto; /* prevent horizontal scrollbar */
            overflow-x: hidden;
            background: gainsboro;
            font-size: 0.9em;
            font-weight: bolder;
            color: whitesmoke;
        }

        .tablelabel {
            color: black;
            font-family: Calibri, sans-serif !important;
            font-size: 12px;
        }

        .ui-multiselect-checkboxes li ul li {
            list-style: none !important;
            clear: both;
            font-size: 1.0em;
            padding-right: 3px;
        }

        .columnul {
            float: left !important; /*width: 14.28% !important;*/ /*width: 14.28% !important;*/
            padding-left: 10px !important; /*height: 175px !important;*/
        }

        .columnwidth { /*width: 50% !important;*/
            width: 85% !important;
            font-size: 0.8em !important;
        }

        .ui-multiselect-checkboxes ui-helper-reset {
            height: 200px;
            width: 500px;
            overflow: auto;
        }

        .FieldSetBox {
            border: #aaaaaa 1px solid;
            z-index: 0px;
            border-radius: 4px;
            text-align: left;
        }

        .dataTables_info, .dataTables_length, .dataTables_filter {
            color : white;
        }

        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }
        .ui-state-disabled, .ui-widget-content .ui-state-disabled, .ui-widget-header .ui-state-disabled {
            opacity:1.35 !important;
        }
    </style>
    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" src="Script/General.js"></script>

    <script type="text/javascript" src="Script/Validation.js"></script>

    <script type="text/javascript" src="Script/popcalendar.js"></script>

    <script src="Script/jquery-ui.js" type="text/javascript"></script>

    <link rel="stylesheet" type="text/css" href="App_Themes/jquery.multiselect.css" />
    <link rel="Stylesheet" type="text/css" href="App_Themes/StyleBlue/UI_Theme/jquery-ui.css" />

    <script src="Script/jquery.dataTables.min.js" type="text/javascript"></script>

    <script src="Script/jquery.dataTables.plugins.js" type="text/javascript"></script>

    <script src="Script/jquery.multiselect.js" type="text/javascript"></script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>
            <table>
                <tbody>
                    <tr>
                        <td>
                            <div style="display: none; left: 300px; width: 450px; top: 300px; height: 200px; text-align: left"
                                id="divAuthentication" class="divModalPopup" runat="server">
                                <table style="width: 400px" align="center">
                                    <tbody>
                                        <tr align="center">
                                            <td class="Label" align="center" colspan="2">
                                                <strong style="white-space: nowrap">User Authentication</strong>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="trName">
                                            <td style="white-space: nowrap" class="Label" align="left">Name
                                            </td>
                                            <td class="Label" align="left">
                                                <asp:Label runat="server" ID="lblSignername" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="trDesignation">
                                            <td style="white-space: nowrap" class="Label" align="left">Designation
                                            </td>
                                            <td class="Label" align="left">
                                                <asp:Label runat="server" ID="lblSignerDesignation" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr runat="server" id="trRemarks">
                                            <td style="white-space: nowrap" class="Label" align="left">Remarks
                                            </td>
                                            <td class="Label" align="left">
                                                <asp:Label runat="server" ID="lblSignRemarks" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="white-space: nowrap" class="Label" align="left">Password
                                            </td>
                                            <td class="Label" align="left">
                                                <asp:TextBox ID="txtPassword" runat="Server" Text="" CssClass="textbox password"
                                                    onkeydown="return CheckTheEnterKey(event);" TextMode="Password"> </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="Label" align="left" colspan="2">
                                                <asp:Button ID="btnAuthenticate" runat="server" Text="Authenticate" CssClass="btn btnnew"
                                                    ToolTip="Authenticate" OnClientClick="return ValidationForAuthentication();" />
                                                <asp:Button ID="btnClose" runat="server" Text="Close" ToolTip="Close" CssClass="btn btnclose"
                                                    OnClientClick="return DivAuthenticationHideShow('H');" />
                                                <asp:HiddenField ID="hdnDirectAuthentication" runat="server" Value="false" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

    <table style="width: 100%">
        <tr>
            <td>
                <asp:UpdatePanel ID="upcontrols" runat="server">
                    <ContentTemplate>
                        <fieldset class="FieldSetBox" style="display: inline-block; width: 94%;">
                            <legend class="LegendText" style="color: Black">
                                <img id="imgfldgen" alt="DCF Activity Status Report" src="images/panelcollapse.png"
                                    onclick="displayCRFInfo(this,'tblEntryData');" runat="server" style="margin-right: 2px;" />DCF
                                Activity Detail </legend>
                            <div id="tblEntryData">
                                <table border="0" style="width: 100%">
                                    <tr>
                                        <td colspan="3" style="height: 10px;"></td>
                                    </tr>
                                    <tr>
                                        <td class="Label" nowrap="nowrap" style="text-align: right; width: 20%;">Project Name/Request Id :
                                        </td>

                                        <td class="Label" style="text-align: left; width: 50%;">
                                            <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="100%"></asp:TextBox>
                                            <asp:Button Style="display: none" ID="btnSetProject" runat="server" Text=" Project"></asp:Button>
                                            <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                                CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientItemSelected="OnSelected"
                                                OnClientShowing="ClientPopulated" ServiceMethod="GetMyProjectCompletionList"
                                                ServicePath="AutoComplete.asmx" TargetControlID="txtProject" UseContextKey="True"
                                                CompletionListElementID="pnlProjectList">
                                            </cc1:AutoCompleteExtender>
                                            <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" />
                                        </td>

                                        <td style="text-align: left;padding-left:14px;">
                                            <asp:CheckBox ID="chkRereview" runat="server" Text="Re-review" AutoPostBack="True" onclick="checkValidationforDCF();" />
                                        </td>

                                        <td style="text-align: left">
                                            <asp:CheckBox ID="chkDCF" runat="server" Text="DCF Tracking" AutoPostBack="True" onclick="checkValidationforReReview();" />
                                        </td>

                                    </tr>

                                    <tr runat="server" id="trPeriodnew" visible="false">
                                        <td runat="server" id="trPeriod" class="Label" nowrap="nowrap" style="text-align: right;">Period :
                                        </td>

                                        <td runat="server" id="trddlPeriod" class="Label" nowrap="nowrap" style="text-align: left; float: left; width: 44%;">
                                            <asp:DropDownList runat="server" ID="DdlPeriod" CssClass="dropDownList" AutoPostBack="True" Enabled="false"
                                                Width="200px">
                                            </asp:DropDownList>
                                        </td>
                                        <td id="tdtypeLabel" runat="server" class="LabelText" nowrap="nowrap" style="text-align: right; float: left;">Activity Type*:
                                        </td>
                                        <td id="tdtypeddl" runat="server" class="Label" nowrap="nowrap" style="text-align: left; float: left; width: 38%;">
                                            <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="true" onchange="validationNew()"
                                                CssClass="EntryControl" Width="250px" TabIndex="3">
                                                <asp:ListItem Value="0">All Activity</asp:ListItem>
                                                <asp:ListItem Value="1">Subject Specific Activity</asp:ListItem>
                                                <asp:ListItem Value="2">Generic Activity</asp:ListItem>

                                            </asp:DropDownList>
                                        </td>

                                    </tr>

                                    <tr align="left">
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

                                </table>

                                <table id="tblSecond" border="0" runat="server" style="width: 100%;">

                                    <tr>
                                        <td style="text-align: right; width: 20%">
                                            <strong class="Label">From Date :</strong>
                                        </td>

                                        <td style="width: 20%; text-align: left; white-space: nowrap">
                                            <asp:TextBox ID="txtFromDate" Enabled="true" runat="server" CssClass="textBox"></asp:TextBox>
                                            <img id="ImgFromDate" alt="Select From Date" src="images/Calendar_scheduleHS.png" />
                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate" Format="dd-MMM-yyyy" PopupButtonID="ImgFromDate"></cc1:CalendarExtender>
                                        </td>

                                        <td style="width: 30%; text-align: right">
                                            <strong class="Label">To Date : </strong>
                                            <asp:TextBox ID="txtToDate" Enabled="true" runat="server" CssClass="textBox"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtToDate" Format="dd-MMM-yyyy"></cc1:CalendarExtender>
                                        </td>

                                        <td style="text-align: left;">
                                            <asp:CheckBox ID="chkAllDates" runat="server" Text="All Dates" Checked="true" onclick=" return ClearTextBox();" />
                                        </td>

                                    </tr>

                                    <tr>
                                        <td style="text-align: right; width: 20%">
                                            <asp:Label ID="lblStatus" runat="server" Text="Status :" CssClass="Label"></asp:Label>
                                        </td>

                                        <td style="text-align: left; width: 20%;">
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="dropDownList" AutoPostBack="true">
                                                <asp:ListItem Text="All" Value="All" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Generated" Value="N"></asp:ListItem>
                                                <asp:ListItem Text="Resolved" Value="R"></asp:ListItem>
                                                <asp:ListItem Text="Auto Resolved" Value="A"></asp:ListItem>
                                                <asp:ListItem Text="Answered" Value="O"></asp:ListItem>
                                                <asp:ListItem Text="Internally Resolved" Value="I"></asp:ListItem>
                                                <asp:ListItem Text="Resolute" Value="D"></asp:ListItem>

                                            </asp:DropDownList>
                                        </td>

                                        <td class="Label" style="text-align: right; width: 30%">
                                            <asp:Label ID="lblType" runat="server" Text="Type :"></asp:Label>
                                            <asp:DropDownList ID="ddlDiscrepancyType" runat="server" CssClass="dropDownList"
                                                AutoPostBack="true">
                                                <asp:ListItem Selected="True" Text="All" Value="All"></asp:ListItem>
                                                <asp:ListItem Text="System" Value="System"></asp:ListItem>
                                                <asp:ListItem Text="Manual" Value="Manual"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>

                                    </tr>

                                    <tr>
                                        <td class="Label" style="text-align: right; width: 20%;">Generated By :
                                        </td>

                                        <td style="text-align: left; width: 20%;">
                                            <asp:DropDownList ID="ddlCreatedBy" runat="server" CssClass="dropDownList" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="Label" style="text-align: right; width: 30%;">Data Entry By :
                                    <asp:DropDownList ID="ddlDataEntryBy" runat="server" CssClass="dropDownList" AutoPostBack="true">
                                    </asp:DropDownList>
                                        </td>
                                        <td></td>

                                    </tr>
                                    <tr runat="server" id="trresolved">
                                        <td class="Label" style="text-align: right; width: 20%;">Resolved By :
                                        </td>

                                        <td style="text-align: left; width: 20%;">
                                            <asp:DropDownList ID="ddlResolvedBy" runat="server" CssClass="dropDownList" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                                </table>

                                <table border="0" id="tblRereview" runat="server" style="width: 100%; display: none">
                                    <tr>
                                        <td class="Label" nowrap="nowrap" style="text-align: right; width: 20%;">Activity:
                                        </td>
                                        <td colspan="3" class="Label" nowrap="nowrap" style="text-align: left;">
                                            <asp:DropDownList ID="ddlRereviewActivity" runat="server" CssClass="dropDownList">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Label" nowrap="nowrap" style="text-align: right; width: 20%;">Subject:
                                        </td>
                                        <td class="Label" nowrap="nowrap" style="text-align: left; width: 20%;">
                                            <asp:DropDownList ID="ddlRereviewSubject" runat="server" CssClass="dropDownList">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="Label" nowrap="nowrap" style="text-align: right; width: 20%;">Re-review Level:
                                        </td>
                                        <td class="Label" nowrap="nowrap" style="text-align: left;">
                                            <asp:DropDownList ID="ddlRereviewLevel" runat="server" CssClass="dropDownList" Width="40%">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>

                                <table border="0" id="tblDCF" runat="server" style="width: 100%; display: none">

                                    <tr>
                                        <td class="Label" style="text-align: right; width: 20%">
                                            <asp:Label ID="lblDCfType" runat="server" Text="Type :" CssClass="Label"></asp:Label>

                                        </td>
                                        <td class="Label" style="width: 20%; text-align: left;">
                                            <asp:DropDownList ID="ddlDCFTypes" runat="server" CssClass="dropDownList" onchange="javascript:setTimeout('__doPostBack(\'ctl00$CPHLAMBDA$ddlDCFTypes\',\'\')', 0)">
                                                <asp:ListItem Selected="True" Text="All" Value="All"></asp:ListItem>
                                                <asp:ListItem Text="System" Value="System"></asp:ListItem>
                                                <asp:ListItem Text="Manual" Value="Manual"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>

                                        <td class="Label" style="width: 30%; text-align: right">
                                            <asp:Label ID="lblGrnrratedBy" runat="server" Text="Generated By :"></asp:Label>
                                            <asp:DropDownList ID="ddlGeneratedBy" runat="server" CssClass="dropDownList" onchange="javascript:setTimeout('__doPostBack(\'ctl00$CPHLAMBDA$ddlGeneratedBy\',\'\')', 0)">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="Label" style="text-align: left;"></td>
                                    </tr>


                                </table>

                                <table style="width: 100%">
                                    <tr>
                                        <td colspan="2" style="height: 10px;"></td>
                                    </tr>
                                    <tr>
                                        <td class="Label" colspan="100%" align="center">
                                            <asp:Button runat="server" ID="btnGo" Text="" ToolTip="Go" CssClass="btn btngo"
                                                OnClientClick="return ValidationForGo();" />
                                            <asp:Button runat="server" ID="btnDCFGo" Text="" Visible="false" ToolTip="Go" CssClass="btn btngo" OnClientClick="return ValidateforDCFgo();" />
                                            <asp:Button runat="server" ID="btnRereviewGo" Text="" Visible="false" ToolTip="Go" CssClass="btn btngo"
                                                OnClientClick="return ValidationForRereviewGo();" />
                                            <asp:Button runat="server" ID="btnReReview" Text="Re-review" OnClientClick="return ValidationSelect('chkbxRRCell','Please check at least one activity to be re-reviewed and then click on the “Re-review” button.');"
                                                ToolTip="Re-review the selected activity and proceed further to the next level." CssClass="btn btnnew" Style="width: auto" />
                                            <asp:Button runat="server" ID="btnExportToExcel" ToolTip="Export To Excel"
                                                CssClass="btn btnexcel"/>
                                            <asp:Button runat="server" ID="btnRereviewExportToExcel" Visible="false" ToolTip="Export To Excel"
                                                CssClass="btn btnexcel"/>
                                            <asp:Button runat="server" ID="btnDCFExportToExcel" Visible="false" ToolTip="Export To Excel"
                                                CssClass="btn btnexcel"/>
                                            <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" ToolTip="Cancel" Text="Cancel" />
                                            <asp:Button ID="btnExit" runat="server" CssClass="btn btnexit" Text="Exit" 
                                                    ToolTip="Exit" OnClientClick="return closewindow(this);" />
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </ContentTemplate>

                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSetProject" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                    </Triggers>

                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <fieldset class="FieldSetBox" id="fldgrdParent" runat="server" style="display: none; margin-top: 20px; width: 94%;">
        <legend class="LegendText" style="color: Black">
            <img id="imgfldGrid" alt="DCF Activity Status Report" src="images/panelcollapse.png"
                onclick="displayCRFInfo(this,'tblGrid');" runat="server" style="margin-right: 2px;" />DCF
                                Activity Status Report</legend>
        <div id="tblGrid">
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Button ID="Btn_Resolve" runat="server" Style="display: none" />
                        <asp:HiddenField ID="HFnDCFNo" runat="server" />
                    </td>
                </tr>

                <tr>
                    <td align="left">

                        <asp:UpdatePanel ID="upCount" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblCount" runat="server" Style="margin-left: 5%;"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnDCFGo" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                <asp:PostBackTrigger ControlID="btnExportToExcel" />
                                <asp:AsyncPostBackTrigger ControlID="ddlStatus" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlDataEntryBy" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlCreatedBy" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlGeneratedBy" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlDiscrepancyType" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlResolvedBy" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>

                    </td>
                </tr>

                <tr>
                    <td style="text-align: center" align="center">
                        <table style="width: 90%; margin: auto;">
                            <tr>
                                <td style="width: 100%">
                                    <asp:UpdatePanel ID="upGrid" runat="server">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlGrid" runat="server" ScrollBars="Auto" Style="margin: auto;">

                                                <asp:GridView ID="GVWDSR" runat="server" AutoGenerateColumns="False">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Sr. No">
                                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="nDCFNo" HeaderText="DCF No" />
                                                        <asp:BoundField DataField="Site" HeaderText="Site" />
                                                        <asp:BoundField DataField="Visit" HeaderText="Visit" />
                                                        <asp:BoundField DataField="Activity" HeaderText="Activity" />
                                                        <asp:BoundField DataField="Period" HeaderText="Period" />
                                                        <asp:BoundField DataField="SubjectId" HeaderText="Subject Id" />
                                                        <asp:BoundField DataField="SubjectName" HeaderText="Subject" />
                                                        <asp:BoundField DataField="ScreenNo" HeaderText="SubjectNo." />
                                                        <asp:BoundField DataField="RandomizationNo" HeaderText="Randomization No." />
                                                        <asp:BoundField DataField="RepetitionNo" HeaderText="Repetition No." />
                                                        <asp:BoundField DataField="Attribute" HeaderText="Attribute" />
                                                        <asp:BoundField DataField="DiscrepancyValue" HeaderText="Discrepancy on Value" />
                                                        <asp:BoundField DataField="DCFType" HeaderText="DCF Type" />
                                                        <asp:BoundField DataField="Response" HeaderText="Query" />
                                                        <asp:BoundField DataField="vDefaultValue" HeaderText="Modified Value" />
                                                        <asp:BoundField DataField="vModificationRemark" HeaderText="Modification Remarks" />
                                                        <asp:BoundField DataField="UpdateRemarks" HeaderText="DCF Remarks" />
                                                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" />
                                                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" DataFormatString="{0:dd-MMM-yyyy HH:mm }" ItemStyle-Wrap="false" />
                                                        <asp:BoundField DataField="Status" HeaderText="Status" />
                                                        <asp:BoundField DataField="UpdatedBy" HeaderText="Modified By" />
                                                        <asp:BoundField DataField="UpdatedDate" HeaderText="Modified On" DataFormatString="{0:dd-MMM-yyyy HH:mm }" ItemStyle-Wrap="false" />
                                                        <asp:BoundField DataField="DataEntryBy" HeaderText="Data Entry By" />
                                                        <asp:BoundField DataField="DataEntryOn" HeaderText="Data Entry On" DataFormatString="{0:dd-MMM-yyyy HH:mm }" ItemStyle-Wrap="false" />
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Edit
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="lnkEdit" runat="server" ImageUrl="~/images/Edit2.gif" ToolTip="Edit" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="vActivityId" HeaderText="vActivityId" />
                                                        <asp:BoundField DataField="iNodeId" HeaderText="iNodeId" />
                                                        <asp:BoundField DataField="iMySubjectNo" HeaderText="iMySubjectNo" />
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Resolve
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:LinkButton runat="server" ID="lnkResolve" Text="Resolve" CommandArgument='<%# Eval("nDCFNo") %>'
                                                                    CommandName="Resolve"></asp:LinkButton>

                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblchkhCP" runat="server" Text="Select <br>(for Re-review Only)"></asp:Label><br />
                                                                <asp:CheckBox ID="chkbxRRHead" runat="server" class="chkParent" OnClick="$SelectAll(this,'.chkChild input');" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkbxRRCell" runat="server" class="chkChild" onclick="chkHeaderCheckBox(this,'GVWDSR','chkbxRRHead','chkbxRRCell');" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="vUserTypeCode" HeaderText="vUserTypeCode" />
                                                        <asp:BoundField DataField="cDCFStatus" HeaderText="cDCFStatus" />
                                                        <asp:BoundField DataField="cDCFType" HeaderText="cDCFType" />
                                                        <asp:BoundField DataField="iWorkFlowStageId" HeaderText="iWorkFlowStageId" />
                                                        <asp:BoundField DataField="nCRFDtlNo" HeaderText="CRF Dtl No" />

                                                    </Columns>
                                                    <SelectedRowStyle BackColor="Moccasin"></SelectedRowStyle>
                                                </asp:GridView>

                                            </asp:Panel>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                            <asp:PostBackTrigger ControlID="btnExportToExcel" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlStatus" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlDataEntryBy" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlCreatedBy" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlDiscrepancyType" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlResolvedBy" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>

                            <tr>
                                <td style="width: 100%">
                                    <asp:UpdatePanel ID="upGrid2" runat="server">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlGrid2" runat="server" ScrollBars="Auto" Style="margin: auto;">
                                                <asp:GridView ID="GVWODMSR" runat="server" AutoGenerateColumns="False">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Sr. No">
                                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ParentvNodeDisplayName" HeaderText="Visit" />
                                                        <asp:BoundField DataField="Activity" HeaderText="Activity" />
                                                        <asp:BoundField DataField="ScreenNo" HeaderText="SubjectNo" />
                                                        <asp:BoundField DataField="RandomizationNo" HeaderText="Randomization No." />
                                                        <asp:BoundField DataField="SubjectID" HeaderText="Subject Id" />
                                                        <asp:BoundField DataField="iMySubjectNo" HeaderText="My Subject No." />
                                                        <asp:BoundField DataField="RepetitionNo" HeaderText="Repetition No." />
                                                        <asp:BoundField DataField="Attribute" HeaderText="Attribute" />
                                                        <asp:BoundField DataField="Value" HeaderText="Old Value" />
                                                        <asp:BoundField DataField="LastUpdatedValue" HeaderText="Modified Value" />
                                                        <asp:BoundField DataField="vModificationRemark" HeaderText="Modification Remarks" />
                                                        <asp:BoundField DataField="UpdateRemarks" HeaderText="Update Remarks" />
                                                        <asp:BoundField DataField="UpdatedBy" HeaderText="Modified By" />
                                                        <asp:BoundField DataField="UpdatedDate" HeaderText="Modified Date" DataFormatString="{0:dd-MMM-yyyy HH:mm }" />
                                                        <asp:BoundField DataField="DataEntryBy" HeaderText="Data Entry By" />
                                                        <asp:BoundField DataField="DataEntryDate" HeaderText="DataEntry Date" DataFormatString="{0:dd-MMM-yyyy HH:mm }" />
                                                        <asp:BoundField DataField="Period" HeaderText="Period" />
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Edit
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="lnkEdit2" runat="server" ImageUrl="~/images/Edit2.gif" ToolTip="Edit" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="vActivityId" HeaderText="vActivityId" />
                                                        <asp:BoundField DataField="iNodeId" HeaderText="iNodeId" />
                                                        <asp:BoundField DataField="vProjectTypeCode" HeaderText="vProjectTypeCode" />
                                                    </Columns>
                                                    <SelectedRowStyle BackColor="Moccasin"></SelectedRowStyle>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnGo" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                            <asp:PostBackTrigger ControlID="btnExportToExcel" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlStatus" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlDataEntryBy" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlCreatedBy" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlDiscrepancyType" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlResolvedBy" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:GridView ID="Grdexporttoexcel" runat="server" Style="display: none;" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%">
                                    <asp:UpdatePanel ID="UpRereviewGrid" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                        <ContentTemplate>
                                            <asp:Panel ID="pnlRereview" runat="server" ScrollBars="Auto" Style="margin: auto;">

                                                <asp:GridView ID="gvRereview" runat="server" AutoGenerateColumns="False">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblchkhCP" runat="server" Text="Select <br>(for Re-review Only)"></asp:Label><br />
                                                                <asp:CheckBox ID="chkbxRRHead" runat="server" class="chkParent" OnClick="$SelectAll(this,'.chkChild input');" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkbxRRCell" runat="server" class="chkChild" onclick="chkHeaderCheckBox(this,'gvRereview','chkbxRRHead','chkbxRRCell');" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Site" HeaderText="Project / Site" />
                                                        <asp:BoundField DataField="SubjectID" HeaderText="Subject Id" />
                                                        <asp:BoundField DataField="ScreenNo" HeaderText="Subject No." />
                                                        <asp:BoundField DataField="RandomizationNo" HeaderText="Randomization No." />
                                                        <asp:BoundField DataField="Period" HeaderText="Period" />
                                                        <asp:BoundField DataField="Visit" HeaderText="Visit" />
                                                        <asp:BoundField DataField="RepetitionNo" HeaderText="Repetition No." />
                                                        <asp:BoundField DataField="Activity" HeaderText="Activity" />
                                                        <asp:BoundField DataField="PendingRereviewLevel" HeaderText="Pending Re-review Level" />
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Edit
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="lnkEdit" runat="server" ImageUrl="~/images/Edit2.gif" ToolTip="Edit" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="iWorkFlowstageId" HeaderText="WorkFlowstageId" />
                                                        <asp:BoundField DataField="vActivityId" HeaderText="vActivityId" />
                                                        <asp:BoundField DataField="iNodeId" HeaderText="iNodeId" />
                                                        <asp:BoundField DataField="iMySubjectNo" HeaderText="iMySubjectNo" />
                                                        <asp:BoundField DataField="nCRFDtlNo" HeaderText="nCRFDtlNo" />
                                                        <asp:BoundField DataField="DCFiWorkflowStageId" HeaderText="iDCFBy" />
                                                    </Columns>
                                                    <SelectedRowStyle BackColor="Moccasin"></SelectedRowStyle>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="chkRereview" EventName="CheckedChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="chkDCF" EventName="CheckedChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="btnRereviewGo" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnReReview" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnAuthenticate" EventName="Click" />
                                            <asp:PostBackTrigger ControlID="btnRereviewExportToExcel" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>

                            <tr>
                                <td style="width: 100%">
                                    <asp:UpdatePanel ID="upDCF" runat="server">
                                        <ContentTemplate>

                                            <asp:Panel ID="pnlDCF" runat="server" ScrollBars="Auto" Style="margin: auto;">

                                                <asp:GridView ID="gvwDCF" runat="server" AutoGenerateColumns="False">
                                                    <Columns>
                                                        <asp:BoundField HeaderText="Sr. No">
                                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Visit" HeaderText="Visit" />
                                                        <asp:BoundField DataField="Activity" HeaderText="Activity" />
                                                        <asp:BoundField DataField="Period" HeaderText="Period" />
                                                        <asp:BoundField DataField="SubjectId" HeaderText="Subject Id" />
                                                        <asp:BoundField DataField="iMySubjectNo" HeaderText="Subject No" />
                                                        <asp:BoundField DataField="RepetitionNo" HeaderText="Repetition No." />
                                                        <asp:BoundField DataField="Attribute" HeaderText="Attribute" />
                                                        <asp:BoundField DataField="DCFType" HeaderText="DCF Type" />
                                                        <asp:BoundField DataField="CreatedBy" HeaderText="Generated By" />
                                                        <asp:BoundField DataField="CreatedDate" HeaderText="Generated On" DataFormatString="{0:dd-MMM-yyyy HH:mm }" />
                                                        <asp:BoundField DataField="Status" HeaderText="Status" />
                                                        <asp:BoundField DataField="AnsweredBy" HeaderText="Answered By" />
                                                        <asp:BoundField DataField="AnsweredDate" HeaderText="Answered On" DataFormatString="{0:dd-MMM-yyyy HH:mm }" />

                                                        <asp:BoundField DataField="UpdatedBy" HeaderText="Resolved By" />
                                                        <asp:BoundField DataField="UpdatedDate" HeaderText="Resolved On" DataFormatString="{0:dd-MMM-yyyy HH:mm }" />

                                                        <asp:BoundField DataField="GeneratedtoAnswered" HeaderText="Generated to Answered (Days)" ItemStyle-HorizontalAlign="Center" FooterStyle-VerticalAlign="Middle" />
                                                        <asp:BoundField DataField="AnsweredtoResolved" HeaderText="Answered to Resolved (Days)" ItemStyle-HorizontalAlign="Center" FooterStyle-VerticalAlign="Middle" />
                                                        <asp:BoundField DataField="GeneratedtoResolved" HeaderText="Generated to Resolved (Days)" ItemStyle-HorizontalAlign="Center" FooterStyle-VerticalAlign="Middle" />
                                                        <asp:BoundField DataField="vActivityId" HeaderText="vActivityId" />
                                                        <asp:BoundField DataField="iNodeId" HeaderText="iNodeId" />
                                                        <asp:BoundField DataField="cDCFType" HeaderText="cDCFType" />
                                                    </Columns>
                                                </asp:GridView>

                                            </asp:Panel>

                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnDCFGo" EventName="Click" />
                                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                            <asp:PostBackTrigger ControlID="btnDCFExportToExcel" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlGeneratedBy" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlDCFTypes" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="DdlPeriod" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="DdlPeriod" EventName="SelectedIndexChanged" />

                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

            </table>
        </div>
    </fieldset>
    <asp:HiddenField ID="hdnRereviewActivity" runat="server" />
    <asp:HiddenField ID="hdnRereviewSubject" runat="server" />
    <asp:HiddenField ID="hdnPassword" runat="server" />

    <script language="javascript" type="text/javascript">

        function pageLoad() {
            if ($("#<%= ddlRereviewActivity.ClientID%>")) {
                fnApplySelectActivity();
            }
            if ($("#<%= ddlRereviewSubject.ClientID%>")) {
                fnApplySelectSubject();
            }

            var pnlWidth = screen.width - 100 + 'px';
            $("#ctl00_CPHLAMBDA_pnlGrid").width(pnlWidth);
            $("#ctl00_CPHLAMBDA_pnlDCF").width(pnlWidth);

            $(window).width() > 1180 ? wid = ($(window).width() - 94) + 'px' : wid = $(window).width() - 100 + 'px';
            $('#<%= pnlGrid2.ClientID%>').attr("style", "width:" + wid + ";")
            if ($('#<%= GVWODMSR.ClientID%>')) {
                $('#<%= GVWODMSR.ClientID%>').prepend($('<thead>').append($('#<%= GVWODMSR.ClientID%> tr:first')))
                    .dataTable({
                        "sScrollX": '100%',
                        "bJQueryUI": true,
                        "sPaginationType": "full_numbers",
                        "scrollCollapse": true,
                        "pageLength": 5,
                        "bProcessing": true,
                        "bPaginate": true,
                        "bFooter": false,
                        "bHeader": false,
                        "AutoWidth": true,
                        "bSort": false,
                        "fixedHeader": true,
                        "oLanguage": { "sSearch": "Search" },
                        "aLengthMenu": [[5, 10, 20, 50, -1], [5, 10, 20, 50, "All"]],
                        "iDisplayLength": 5,
                        //"bJQueryUI": true,
                        //"bPaginate": true,
                        //"bFooter": false,
                        //"bHeader": false,
                        //"bAutoWidth": false,
                        //"bSort": false,
                        //"sDom": '<"H"frT>t<"F"i>',
                        ////"oLanguage": { "sSearch": "Search (Day No.)" },
                        "oTableTools": {
                            "aButtons": [
                                "xls"
                            ],
                            "sSwfPath": "Script/swf/copy_cvs_xls_pdf.swf"
                        },
                    });
            }
            $(window).width() > 1180 ? wid = ($(window).width() - 94) + 'px' : wid = $(window).width() - 100 + 'px';
            $('#<%= pnlGrid.ClientID%>').attr("style", "width:" + wid + ";")
            if ($('#<%= GVWDSR.ClientID%>')) {
                $('#<%= GVWDSR.ClientID%>').prepend($('<thead>').append($('#<%= GVWDSR.ClientID%> tr:first')))
                    .dataTable({
                        //"sScrollY": '300px',
                        "sScrollX": '100%',
                        "bJQueryUI": true,
                        "sPaginationType": "full_numbers",
                        "scrollCollapse": true,
                        "pageLength": 5,
                        "bProcessing": true,
                        "bPaginate": true,
                        "bFooter": false,
                        "bHeader": false,
                        "AutoWidth": true,
                        "bSort": false,
                        "fixedHeader": true,
                        "oLanguage": { "sSearch": "Search" },
                        "aLengthMenu": [[5 ,10, 20, 50, -1], [5 ,10, 20, 50, "All"]],
                        "iDisplayLength": 5,
                        //"bJQueryUI": true,
                        //"bPaginate": true,
                        //"bFooter": false,
                        //"bHeader": false,
                        //"bAutoWidth": false,
                        //"bSort": false,
                        //"sDom": '<"H"frT>t<"F"i>',
                        ////"oLanguage": { "sSearch": "Search (Day No.)" },
                        "oTableTools": {
                            "aButtons": [
                                "xls"
                            ],
                            "sSwfPath": "Script/swf/copy_cvs_xls_pdf.swf"
                        },
                    });
            }

            $(window).width() > 1180 ? wid = ($(window).width() - 94) + 'px' : wid = $(window).width() - 100 + 'px';
            $('#<%= pnlRereview.ClientID%>').attr("style", "width:" + wid + ";")
            if ($('#<%= gvRereview.ClientID%>')) {
                $('#<%= gvRereview.ClientID%>').prepend($('<thead>').append($('#<%= gvRereview.ClientID%> tr:first')))
                    .dataTable({
                        "sScrollX": '100%',
                        "bJQueryUI": true,
                        "sPaginationType": "full_numbers",
                        "scrollCollapse": true,
                        "pageLength": 5,
                        "bProcessing": true,
                        "bPaginate": true,
                        "bFooter": false,
                        "bHeader": false,
                        "AutoWidth": true,
                        "bSort": false,
                        "fixedHeader": true,
                        "oLanguage": { "sSearch": "Search" },
                        "aLengthMenu": [[5, 10, 20, 50, -1], [5, 10, 20, 50, "All"]],
                        "iDisplayLength": 5,
                        //"bJQueryUI": true,
                        //"bPaginate": true,
                        //"bFooter": false,
                        //"bHeader": false,
                        //"bAutoWidth": false,
                        //"bSort": false,
                        //"sDom": '<"H"frT>t<"F"i>',
                        ////"oLanguage": { "sSearch": "Search (Day No.)" },
                        "oTableTools": {
                            "aButtons": [
                                "xls"
                            ],
                            "sSwfPath": "Script/swf/copy_cvs_xls_pdf.swf"
                        },
                    });
            }
            $(window).width() > 1180 ? wid = ($(window).width() - 94) + 'px' : wid = $(window).width() - 100 + 'px';
            $('#<%= pnlDCF.ClientID%>').attr("style", "width:" + wid + ";")
            if ($('#<%= gvwDCF.ClientID%>')) {
                $('#<%= gvwDCF.ClientID%>').prepend($('<thead>').append($('#<%= gvwDCF.ClientID%> tr:first')))
                    .dataTable({
                        "sScrollX": '100%',
                        "bJQueryUI": true,
                        "sPaginationType": "full_numbers",
                        "scrollCollapse": true,
                        "pageLength": 5,
                        "bProcessing": true,
                        "bPaginate": true,
                        "bFooter": false,
                        "bHeader": false,
                        "AutoWidth": true,
                        "bSort": false,
                        "fixedHeader": true,
                        "oLanguage": { "sSearch": "Search" },
                        "aLengthMenu": [[5, 10, 20, 50, -1], [5, 10, 20, 50, "All"]],
                        "iDisplayLength": 5,
                        //"bJQueryUI": true,
                        //"bPaginate": true,
                        //"bFooter": false,
                        //"bHeader": false,
                        //"bAutoWidth": false,
                        //"bSort": false,
                        //"sDom": '<"H"frT>t<"F"i>',
                        ////"oLanguage": { "sSearch": "Search (Day No.)" },
                        "oTableTools": {
                            "aButtons": [
                                "xls"
                            ],
                            "sSwfPath": "Script/swf/copy_cvs_xls_pdf.swf"
                        },
                    });
            }

        }

        function displayCRFInfo(control, target) {
            // added by prayag
            if (control.src.toString().toUpperCase().search("EXPAND") != -1) {
                $("#" + target).slideToggle(600);
                control.src = "images/panelcollapse.png";
            }
            else {
                $("#" + target).slideToggle(600);
                control.src = "images/panelexpand.png";
            }
        }

        function validationNew() {

            if (document.getElementById('<%=ddlType.ClientID %>').selectedIndex > 0) {
                if (document.getElementById('<%=HProjectId.ClientId %>').value.toString().trim().length <= 0) {
                    document.getElementById('<%=ddlType.ClientID %>').selectedIndex = 0
                    msgalert('Enter Project First !');
                    return false;
                }

                return true;
            }
        }

        var Activity = [];
        function fnApplySelectActivity() {
            $("#<%= ddlRereviewActivity.ClientID%>").multiselect({
                noneSelectedText: "--Select Activity--",
                click: function (event, ui) {
                    if (ui.checked == true)
                        Activity.push("'" + ui.value + "'");
                    else if (ui.checked == false) {
                        if ($.inArray("'" + ui.value + "'", Activity) >= 0)
                            Activity.splice(Activity.indexOf("'" + ui.value + "'"), 1)
                    }

                    if ($("input[name$='ddlRereviewActivity']").length > 0) {
                        //clearControls();
                    }
                },
                checkAll: function (event, ui) {
                    Activity = [];
                    for (var i = 0; i < event.target.options.length; i++) {
                        Activity.push("'" + $(event.target.options[i]).val() + "'")
                    }
                    if ($("input[name$='ddlRereviewActivity']").length > 0) {
                        //clearControls();
                    }
                    $("#<%= ddlRereviewActivity.ClientID%>").multiselect("refresh");
                    $("#<%= ddlRereviewActivity.ClientID%>").multiselect("widget").find(':checkbox').click();


                },
                uncheckAll: function (event, ui) {
                    Activity = [];
                    $("#<%= ddlRereviewActivity.ClientID%>").multiselect("refresh");
                    if ($("input[name$='ddlRereviewActivity':checked]").length > 0) {
                        //clearControls();
                    }
                }
            });
        }

        var Subject = [];
        function fnApplySelectSubject() {

            $("#<%= ddlRereviewSubject.ClientID%>").multiselect({
                noneSelectedText: "--Select Subject--",
                click: function (event, ui) {
                    if (ui.checked == true)
                        Subject.push("'" + ui.value + "'");
                    else if (ui.checked == false) {
                        if ($.inArray("'" + ui.value + "'", Subject) >= 0)
                            Subject.splice(Subject.indexOf("'" + ui.value + "'"), 1)
                    }
                    if ($("input[name$='ddlRereviewSubject']").length > 0) {
                        //clearControls();
                    }
                },
                checkAll: function (event, ui) {
                    Subject = [];
                    for (var i = 0; i < event.target.options.length; i++) {
                        Subject.push("'" + $(event.target.options[i]).val() + "'")
                    }
                    if ($("input[name$='ddlRereviewSubject']").length > 0) {
                        //clearControls();
                    }
                    $("#<%= ddlRereviewSubject.ClientID%>").multiselect("refresh");
                    $("#<%= ddlRereviewSubject.ClientID%>").multiselect("widget").find(':checkbox').click();


                },
                uncheckAll: function (event, ui) {
                    Subject = [];
                    $("#<%= ddlRereviewSubject.ClientID%>").multiselect("refresh");
                    if ($("input[name$='ddlRereviewSubject']:checked").length > 0) {
                        //clearControls();
                    }
                }
            });
        }
        function checkValidationforDCF() {
            if ($('#ctl00_CPHLAMBDA_chkDCF').is(":checked") == true) {
                msgalert('Please First Deselect DCF Trcking Checkbox !')
                $('#ctl00_CPHLAMBDA_chkRereview').attr('checked', false)
                return false
            }

        }

        function checkValidationforReReview() {
            if ($('#ctl00_CPHLAMBDA_chkRereview').is(":checked") == true) {
                msgalert('Please First Deselect Re-review Checkbox !')
                $('#ctl00_CPHLAMBDA_chkDCF').attr('checked', false)
                return false
            }

        }
        checkValidationforReReview

        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        function Validation() {
            if (document.getElementById('<%= HProjectId.ClientId %>').value.toString().trim().length <= 0) {
                msgalert('Please Enter Project !');
                return false;
            }
            return true;
        }
        function closewindow(e) {
            msgConfirmDeleteAlert(null, "Are you sure want to Exit ?", function (isConfirmed) {
                if (isConfirmed) {
                    var parWin = window.opener;
                    if (parWin != null && typeof (parWin) != 'undefined') {
                        if (parWin && parWin.open && !parWin.closed) {
                            window.parent.document.location.reload();
                        }
                    }
                    self.close();
                    __doPostBack(e.name, '');
                    return true;
                } else {
                    return false;
                }
            });
            return false;
        }
        function $SelectAll(e, range) {
            $(range).each(function () {
                if (!this.disabled) this.checked = e.checked;
            });
        }
        function chkHeaderCheckBox(ele, gvName, chkHeader, chkRow) {

            var gridName = document.getElementById('<%= gvRereview.ClientID%>');
            var cnt = 0;
            for (var i = 0; i < gridName.rows.length - 1; i++) {
                if (document.getElementById(gridName.id + '_ctl' + getCounter(i + 2) + '_' + chkRow)) {
                    if (document.getElementById(gridName.id + '_ctl' + getCounter(i + 2) + '_' + chkRow).disabled == false && document.getElementById(gridName.id + '_ctl' + getCounter(i + 2) + '_' + chkRow).checked == false) {
                        cnt += 1;
                    }
                }
            }
            if (cnt > 0) {
                $('.chkParent input[type="checkbox"]').each(function () {
                    this.checked = false;
                });
            }
            else {
                $('.chkParent input[type="checkbox"]').each(function () {
                    this.checked = true;
                });
            }
            return true;
        }
        function getCounter(i) {
            var cnt = 0;

            if (i > 0 && i < 10)
                cnt = "0" + i;
            else
                cnt = i;
            return cnt;
        }

        function ValidationForAuthentication() {
            if (document.getElementById('<%= txtPassword.ClientId %>').value.trim() == '') {
                document.getElementById('<%= txtPassword.ClientId %>').value = '';
                msgalert('Please Enter Password For Authentication !');
                document.getElementById('<%= txtPassword.ClientId %>').focus();
                return false;
            }

            document.getElementById('<%= hdnPassword.ClientID%>').value = document.getElementById('<%= txtPassword.ClientId %>').value;

        }
        function DivAuthenticationHideShow(Type) {
            if (Type == 'H') {
                $("#<%= divAuthentication.ClientID%>").css({ 'display': 'none' })
            }
            else {
                $("#<%= divAuthentication.ClientID%>").css({ 'display': '' })
                document.getElementById('<%= txtPassword.ClientID%>').value = '';
                document.getElementById('<%= txtPassword.ClientID%>').focus();
            }
            return false;
        }
        function DivfieldShowHide(Type) {
            // added by prayag
            if (Type == 'P') {
                if (document.getElementById('ctl00_CPHLAMBDA_ddlType') !== null) {
                    document.getElementById('<%=ddlType.ClientID %>').selectedIndex = 0
                    document.getElementById('<%=DdlPeriod.ClientID%>').selectedIndex = 0
                }
                document.getElementById('ctl00_CPHLAMBDA_fldgrdParent').style.display = "none";
                document.getElementById('ctl00_CPHLAMBDA_divSubject').style.display = "none";
                document.getElementById('ctl00_CPHLAMBDA_divActivity').style.display = "none";

                UncheckAll()

                return true
            }
            if (Type == 'Q') {
                document.getElementById('ctl00_CPHLAMBDA_fldgrdParent').style.display = "none";

                return true
            }
            if (Type == 'R') {

                document.getElementById('ctl00_CPHLAMBDA_fldgrdParent').style.display = "inline-block";

                return true
            }
        }
        function UncheckAll() {
            inputs = $("#ctl00_CPHLAMBDA_tvActivity").find("table [type=checkbox]");
            inputssub = $("#ctl00_CPHLAMBDA_tvSubject").find("table [type=checkbox]");
            for (i = 0 ; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox") {
                    inputs[i].checked = false;
                }
            }
            for (i = 0 ; i < inputssub.length; i++) {
                if (inputssub[i].type == "checkbox") {
                    inputssub[i].checked = false;
                }
            }
            return true
        }
        function CheckTheEnterKey(e) {
            var unicode = e.charCode ? e.charCode : e.keyCode
            if (unicode == 13) {
                document.getElementById('<%= btnAuthenticate.ClientId %>').click();
                return false;
            }
            return true;
        }

        function ValidationSelect(chk, msg) {
            var frm = document.forms[0], boolcheck = false;
            var WorkflowStageId = '<%= Session(S_WorkFlowStageId)%>';
            var ScopeNo = '<%= Session(S_ScopeNo)%>';
            for (i = 0; i < frm.elements.length; i++) {
                if (frm.elements[i].type == "checkbox" && frm.elements[i].name.lastIndexOf(chk) != -1 && frm.elements[i].disabled == false) {
                    if (frm.elements[i].checked == true) {
                        boolcheck = true;
                        var DirectAuthentication = $('#ctl00_CPHLAMBDA_hdnDirectAuthentication').val();
                        if (DirectAuthentication === "false") {
                            DivAuthenticationHideShow('S');
                            return false;
                        }
                    }
                }
            }
            if (boolcheck == false) {
                msgalert(msg);
                return false;
            }
            return true;
        }

        function ValidationForGo() {

            if (document.getElementById('<%=HProjectId.clientid%>').value == '') {
                msgalert('Please Enter Project !');
                return false;
            }
            if (document.getElementById('ctl00_CPHLAMBDA_ddlType') !== null) {
                if (document.getElementById('<%= ddlType.ClientId %>').selectedIndex == 1) {

                    if ($("#ctl00_CPHLAMBDA_divSubject").find("table [type=checkbox]").length == 0) {
                        msgalert("No Subject Found !");
                        return false;
                    }

                    if ($("#ctl00_CPHLAMBDA_tvSubject [type=checkbox]:checked").length == 0) {
                        msgalert("Please select Subject !");
                        return false;
                    }
                }
            }
            if (document.getElementById('ctl00_CPHLAMBDA_ddlType') !== null) {
                if (document.getElementById('<%= ddlType.ClientId %>').selectedIndex != 0) {

                    if ($("#ctl00_CPHLAMBDA_tvActivity [type=checkbox]:checked").length == 0) {
                        msgalert("Please select Activity !");
                        return false;
                    }

                }
            }
            var chk = document.getElementById('<%=chkAllDates.clientid%>').checked;

            if (chk) {
                if (document.getElementById('<%=txtFromDate.clientid%>').value != '') {
                    document.getElementById('<%=chkAllDates.clientid%>').checked = false;
                    return true;
                }

                if (document.getElementById('<%=txtToDate.clientid%>').value != '') {
                    document.getElementById('<%=chkAllDates.clientid%>').checked = false;
                    return true;
                }

            }
            else {
                if (document.getElementById('<%=txtFromDate.clientid%>').value == '') {
                    msgalert('Please Enter From Date !');
                    return false;
                }
                if (document.getElementById('<%=txtToDate.clientid%>').value == '') {
                    msgalert('Please Enter To Date !');
                    return false;
                }
                if (CompareDate(document.getElementById('<%=txtFromDate.ClientID%>').value, document.getElementById('<%=txtToDate.ClientID%>').value) != true) {
                    return false;

                }

            }

        }
        function ValidationForRereviewGo() {
            if (document.getElementById('<%=HProjectId.clientid%>').value == '') {
                msgalert('Please Enter Project !');
                return false;
            }
            document.getElementById('<%= hdnRereviewActivity.ClientID%>').value = Activity;
            document.getElementById('<%= hdnRereviewSubject.ClientID%>').value = Subject;
            Activity = []; Subject = [];
            return true;
        }

        function OpenRereviewTable() {
            var txtproject = document.getElementById('<%=txtproject.ClientID%>');
        var HProjectId = document.getElementById('<%=HProjectId.ClientID%>');
        if (txtproject.value.trim() == '') {
            msgalert("Please select project first !");
            txtproject.focus();
            return false;
        }
        return true;
    }


    function OpenWindow(Path) {
        window.open(Path);
        return false;
    }

    function RefreshPage() {
        var btn = document.getElementById('<%= btnGo.ClientId %>');
        if (btn) {
            btn.click();
        }
        var btnRereviewGo = document.getElementById('<%= btnRereviewGo.ClientID%>');
            if (btnRereviewGo) {
                btnRereviewGo.click();
            }

        }

        function ClearTextBox() {

            var chk = document.getElementById('<%=chkAllDates.clientid%>').checked;
        if (chk) {

            document.getElementById('<%=txtToDate.clientid%>').value = '';
            document.getElementById('<%=txtFromDate.clientid%>').value = '';
        }

    }

    function ShowConfirmation(nDCFNo) {
        var Istrue = confirm("Are you sure you want to resolve this Discrepancy?");
        if (Istrue == true) {
            document.getElementById('<%=HFnDCFNo.clientid %>').value = nDCFNo;
            document.getElementById('<%=Btn_Resolve.clientid %>').click();
            return true;
        }
        return false;
    }
    function ChangeUrl(page, url) {
        if (typeof (history.pushState) != "undefined") {
            var obj = { Page: page, Url: url };
            history.pushState(obj, obj.Page, obj.Url);
        } else {
            msgalert("Browser does not support HTML5 !");
        }
    }

    function ValidateforDCFgo() {
        if (document.getElementById('<%=HProjectId.clientid%>').value == '') {
            msgalert('Please Enter Project !');
            return false;
        }
        if (document.getElementById('ctl00_CPHLAMBDA_ddlType') !== null) {
            if (document.getElementById('<%= ddlType.ClientId %>').selectedIndex == 1) {

                if ($("#ctl00_CPHLAMBDA_divSubject").find("table [type=checkbox]").length == 0) {
                    msgalert("No Subject Found !");
                    return false;
                }

            }
        }
        if (document.getElementById('ctl00_CPHLAMBDA_ddlType') !== null) {
            if (document.getElementById('<%= ddlType.ClientId %>').selectedIndex != 0) {

                if ($("#ctl00_CPHLAMBDA_tvActivity [type=checkbox]:checked").length == 0) {
                    msgalert("Please select Activity !");
                    return false;
                }

            }
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

    </script>

</asp:Content>
