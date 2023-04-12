<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/MasterPage.master"
    CodeFile="frmProtocolDetail.aspx.vb" Inherits="frmProtocolDetail" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="CPHLAMBDA" runat="server">
    <link rel="stylesheet" type="text/css" href="App_Themes/jquery.multiselect.css" />
    <link rel="Stylesheet" href="App_Themes/StyleBlue/UI_Theme/jquery-ui.css" type="text/css" />

    <script language="javascript" src="Script/popcalendar.js"></script>
    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <script type="text/javascript" src="Script/Validation.js"></script>
    <script src="Script/jquery-ui-1.10.4.custom.min.js" type="text/javascript"></script>
    <script src="Script/jquery.multiselect.min.js" type="text/javascript"></script>

    <style>
        .FieldSetBox {
            border: #539FE1 1.5px solid;
            z-index: 0px;
            border-radius: 4px;
            text-align: left;
        }

        .ui-multiselect {
            border: 1px solid navy;
            /*width: 235px !important;*/
            width: 40% !important;
            /*height: 10%;*/
        }

        .ui-multiselect-checkboxes {
            height: 140px !important;
        }
        .ajax__tab_xp .ajax__tab_tab{
         height:initial;
        }
    </style>
    
    <div>
        <table style="width: 100%">
            <tr>
                <td id="tdEdit" runat="server" style="white-space: nowrap">
                    <asp:HiddenField ID="hdnDrug" runat="server" />
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <strong>Search Project : </strong>
                            <asp:TextBox ID="txtsearch" runat="server" CssClass="textboxFileUpload" Width="40%" MaxLength="50"></asp:TextBox>
                            <asp:Button ID="BtnEdit" runat="server" Text="Edit" Font-Bold="True" BackColor="White"
                                BorderColor="White" BorderStyle="None" ForeColor="White" /><cc1:AutoCompleteExtender
                                    ID="AutoCompleteExtender1" runat="server" BehaviorID="AutoCompleteExtender1"
                                    CompletionListCssClass="autocomplete_list" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                    CompletionListElementID="pnlProjectList" CompletionListItemCssClass="autocomplete_listitem" MinimumPrefixLength="1" OnClientShowing="ClientPopulated"
                                    ServiceMethod="GetMyProjectCompletionList" ServicePath="AutoComplete.asmx" TargetControlID="txtsearch"
                                    UseContextKey="True" OnClientItemSelected="OnSelected">
                                </cc1:AutoCompleteExtender>
                            <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 300px; overflow: auto; overflow-x: hidden" />
                            <asp:HiddenField ID="HWorkspaceId" runat="server" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="BtnCancle" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanelDummy" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="tdContainer" runat="server" style="width: 80%; margin: auto;">
                                <tbody>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                <ContentTemplate>
                                                    <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" OnClientActiveTabChanged="ActiveTabChanged">
                                                        <cc1:TabPanel runat="server" HeaderText="TabPanel2" ID="TabPanel2">
                                                            <HeaderTemplate>
                                                                <span style="font-size: 10pt; font-family: Verdana">Feasibility Form</span>
                                                            </HeaderTemplate>
                                                            <ContentTemplate>
                                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                                    <ContentTemplate>
                                                                        <asp:HiddenField ID="hdnPostBack" runat="server" Value="true" />
                                                                        <table style="width: 100%">
                                                                            <tbody>
                                                                                <%-- <tr>
                                                                                    <td style="text-align: left;">
                                                                                        <strong>Project detail </strong>
                                                                                        <br />
                                                                                        <hr style="background-image: none; width: 100%; color: #ffcc66; background-color: #ffcc66"
                                                                                            class="hr " />
                                                                                    </td>
                                                                                </tr> --%>
                                                                                <tr>
                                                                                    <td>
                                                                                        <fieldset class="FieldSetBox" id="fldgrdProjectDetail" runat="server" style="margin-top: 20px; width: 94%;">
                                                                                            <legend class="LegendText" style="color: Black">
                                                                                                <img id="imgprojectdetail" alt="Project detail" src="images/panelcollapse.png" class="imgexpand"
                                                                                                    onclick="displayNewRequest(this,'tblProjectDetail');" runat="server" style="margin-right: 2px;" />
                                                                                                <strong style="color: #1560A1; font-size: small">Project detail</strong>

                                                                                            </legend>
                                                                                            <div id="tblProjectDetail" class="divExpand">
                                                                                                <table width="100%">
                                                                                                    <tbody>
                                                                                                        <tr>
                                                                                                            <%-- <td align="left" width="350">
                                                                                                    </td>--%>
                                                                                                            <td style="font-weight: bold; text-align: center;" colspan="2">
                                                                                                                <input style="display: none; width: 216px" id="TxtProject" class="textBox " type="text"
                                                                                                                    runat="server" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td style="text-align: right; width: 40%;" class="Label ">Template Name*:
                                                                                                            </td>
                                                                                                            <td style="text-align: left;">
                                                                                                                <select style="width: 40%" id="SlcTemplate" class="dropDownList" runat="server">
                                                                                                                </select>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td style="text-align: right;" class="Label">Project Type*:
                                                                                                            </td>
                                                                                                            <td style="text-align: left;">
                                                                                                                <asp:DropDownList ID="SlcProject" runat="server" Style="width: 40%" CssClass="dropDownList" AutoPostBack="true"></asp:DropDownList>

                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td style="text-align: right;" class="Label">Project Sub Type:
                                                                                                            </td>
                                                                                                            <td style="text-align: left;">
                                                                                                                <asp:DropDownList ID="SlcProjectSubType" runat="server" Style="width: 40%" CssClass="dropDownList" AutoPostBack="true"></asp:DropDownList>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td style="text-align: right;" class="Label">Study Type*:
                                                                                                            </td>
                                                                                                            <td style="text-align: left;">
                                                                                                                <asp:DropDownList Style="width: 40%" ID="ddlStudytype" class="dropDownList" runat="server">
                                                                                                                </asp:DropDownList>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td style="text-align: right;" class="Label">Sponsor*:
                                                                                                            </td>
                                                                                                            <td style="text-align: left;">
                                                                                                                <%--<select style="width: 40%" id="SlcSponsor"  class="dropDownList" runat="server">--%>
                                                                                                                <asp:DropDownList ID="SlcSponsor" runat="server" Style="width: 40%" CssClass="dropDownList" OnSelectedIndexChanged="SlcSponsor_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                                                                                </select>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td style="text-align: right;" class="Label">Location*:
                                                                                                            </td>
                                                                                                            <td style="text-align: left;">

                                                                                                                <asp:DropDownList ID="slcLocation" runat="server" Style="width: 40%" CssClass="dropDownList" OnSelectedIndexChanged="slcLocation_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>

                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td style="text-align: right;" class="Label">Project Manager *:
                                                                                                            </td>
                                                                                                            <td style="text-align: left;">
                                                                                                                <select style="width: 40%" id="SlcManager" class="dropDownList" runat="server">
                                                                                                                </select>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td style="text-align: right;" class="Label">Project Co-ordinator:
                                                                                                            </td>
                                                                                                            <td style="text-align: left;">
                                                                                                                <select style="width: 40%" id="SlcCoOrdinate" class="dropDownList" runat="server">
                                                                                                                </select>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td style="text-align: right;" class="Label">Principal Investigator:
                                                                                                            </td>
                                                                                                            <td style="text-align: left;">
                                                                                                                <select style="width: 40%" id="SlcPI" class="dropDownList" runat="server">
                                                                                                                </select>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td style="text-align: right;" class="Label">Scope Of Service:
                                                                                                            </td>
                                                                                                            <td style="text-align: left;">
                                                                                                                <select style="width: 40%" id="SlcService" class="dropDownList" runat="server">
                                                                                                                </select>
                                                                                                            </td>
                                                                                                        </tr>

                                                                                                        <tr>
                                                                                                            <td style="text-align: right;" class="Label">Regulatory Requirement:
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <asp:DropDownList ID="ddlRegularityReq" runat="server" Style="width: 40%;" CssClass="dropDownList" AutoPostBack="false" onchange=" fnRegularityReq();">
                                                                                                                    <asp:ListItem>BE Noc/CTA</asp:ListItem>
                                                                                                                    <asp:ListItem>TL</asp:ListItem>
                                                                                                                    <asp:ListItem>Export Noc</asp:ListItem>
                                                                                                                    <asp:ListItem>CBN Approval</asp:ListItem>
                                                                                                                    <asp:ListItem>None</asp:ListItem>
                                                                                                                    <asp:ListItem>Other</asp:ListItem>
                                                                                                                </asp:DropDownList>

                                                                                                            </td>
                                                                                                        </tr>


                                                                                                        <tr>
                                                                                                            <td style="text-align: right;" class="Label">Indication:
                                                                                                            </td>
                                                                                                            <td style="text-align: left;">
                                                                                                                <asp:TextBox ID="txtIndication" runat="server" CssClass="textBox " />
                                                                                                            </td>
                                                                                                        </tr>

                                                                                                        <tr>
                                                                                                            <td style="text-align: right;" class="Label">No. of Subjects* :
                                                                                                            </td>
                                                                                                            <td style="text-align: left;">
                                                                                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                                                                                    <ContentTemplate>
                                                                                                                        <asp:TextBox ID="TxtSubNo" runat="server" CssClass="textBox " Width="10%" MaxLength="3" />
                                                                                                                        <cc1:FilteredTextBoxExtender ID="FILQty1" runat="server" TargetControlID="TxtSubNo"
                                                                                                                            ValidChars="0123456789">
                                                                                                                        </cc1:FilteredTextBoxExtender>
                                                                                                                    </ContentTemplate>
                                                                                                                </asp:UpdatePanel>
                                                                                                                <br />
                                                                                                            </td>
                                                                                                        </tr>

                                                                                                        <tr>
                                                                                                            <td align="right" class="Label">Ethical Approval Required *:
                                                                                                            </td>
                                                                                                            <td align="left">
                                                                                                                <asp:RadioButtonList runat="server" ID="rBtnEthics"   CssClass="RadioButton" RepeatColumns="3"
                                                                                                                    RepeatDirection="Horizontal">
                                                                                                                    <asp:ListItem Text="IEC"  Value="IEC" />
                                                                                                                    <asp:ListItem Text="IRB" Value="IBR" />
                                                                                                                    <asp:ListItem Text="None" Value="None" />
                                                                                                                </asp:RadioButtonList>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="right" class="Label">Remark :
                                                                                                            </td>
                                                                                                            <td align="left">
                                                                                                                <asp:TextBox runat="server" ID="txtEthicalRemarks"></asp:TextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </tbody>
                                                                                                </table>

                                                                                            </div>
                                                                                        </fieldset>
                                                                                    </td>

                                                                                </tr>
                                                                                <%--tr>
                                                                                    <td style="text-align: left;">
                                                                                        <strong>Protocol Definition</strong><br />
                                                                                        <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66; width: 100%"
                                                                                            class="hr " />
                                                                                    </td>
                                                                                </tr>--%>
                                                                                <tr>
                                                                                    <td>
                                                                                        <fieldset class="FieldSetBox" id="fldProtocolDefinition" runat="server" style="margin-top: 20px; width: 94%;">
                                                                                            <legend class="LegendText" style="color: Black">
                                                                                                <img id="imgprotocoldefinition" alt="Protocol Definition" src="images/panelcollapse.png" class="imgexpand"
                                                                                                    onclick="displayNewRequest(this,'tblprotocoldefinition');" runat="server" style="margin-right: 2px;" />
                                                                                                <strong style="color: #1560A1; font-size: small">Protocol Definition</strong>

                                                                                            </legend>
                                                                                            <div id="tblprotocoldefinition" class="divExpand">
                                                                                                <table width="100%">
                                                                                                    <tbody>
                                                                                                        <tr>
                                                                                                            <td style="text-align: right; width: 40%;" class="Label">Drug Name *:
                                                                                                            </td>
                                                                                                            <td style="text-align: left;">
                                                                                                                <%--<select style="width: 40%" id="SlcDrug" class="dropDownList" runat="server">--%>
                                                                                                                <asp:DropDownList ID="SlcDrug" runat="server" Style="width: 40%" CssClass="dropDownList" OnSelectedIndexChanged="SlcDrug_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                                                                                </select>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td style="text-align: right;" class="Label">Submission * :
                                                                                                            </td>
                                                                                                            <td style="text-align: left;">
                                                                                                                <select style="width: 40%" id="SlcSubmission" class="dropDownList" runat="server">
                                                                                                                </select>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td style="text-align: right;" class="Label">Study Design :
                                                                                                            </td>
                                                                                                            <td style="text-align: left;">
                                                                                                                <textarea style="width: 40%" id="TxtStudy" runat="server" rows="2" cols="0" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td style="text-align: right;" class="Label">Strength :
                                                                                                            </td>
                                                                                                            <td style="text-align: left;">
                                                                                                                <asp:DropDownList ID="ddlStrength" runat="server" Style="width: 40%" CssClass="dropDownList"></asp:DropDownList>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td style="text-align: right;" class="Label">Formulation :
                                                                                                            </td>
                                                                                                            <td style="text-align: left;">
                                                                                                                <asp:DropDownList ID="ddlFromulation" runat="server" Style="width: 40%" CssClass="dropDownList"></asp:DropDownList>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td style="text-align: right;" class="Label">Release: 
                                                                                                            </td>
                                                                                                            <td style="text-align: left;">
                                                                                                                <asp:DropDownList ID="ddlRelease" runat="server" Style="width: 40%" CssClass="dropDownList"></asp:DropDownList>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td style="text-align: right;" class="Label">FastFed :
                                                                                                            </td>
                                                                                                            <td style="text-align: left;">
                                                                                                                <asp:CheckBox ID="ChkfastfedYes" onclick="ChkBox('ChkfedYes')" runat="server" Text="Fasting"></asp:CheckBox>
                                                                                                                <asp:CheckBox ID="ChkfastfedNo" onclick="ChkBox('ChkfedNo')" runat="server" Text="Fed"
                                                                                                                    Checked="True"></asp:CheckBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </tbody>
                                                                                                </table>
                                                                                            </div>
                                                                                        </fieldset>
                                                                                    </td>
                                                                                </tr>
                                                                                <%--<tr>
                                                                                    <td style="text-align: left;">
                                                                                        <strong>Initial Quotes Information</strong><br />
                                                                                        <hr style="background-image: none; width: 100%; color: #ffcc66; background-color: #ffcc66"
                                                                                            class="hr " />
                                                                                    </td>
                                                                                </tr>--%>
                                                                                <tr>
                                                                                    <td>
                                                                                        <fieldset class="FieldSetBox" id="fldIntialInfo" runat="server" style="margin-top: 20px; width: 94%;">
                                                                                            <legend class="LegendText" style="color: Black">
                                                                                                <img id="imgIntialInfo" alt="Initial Quotes Information" src="images/panelcollapse.png" class="imgexpand"
                                                                                                    onclick="displayNewRequest(this,'tblIntialInfo');" runat="server" style="margin-right: 2px;" />
                                                                                                <strong style="color: #1560A1; font-size: small">Initial Quotes Information</strong>

                                                                                            </legend>
                                                                                            <div id="tblIntialInfo" class="divExpand">
                                                                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                                                                    <ContentTemplate>
                                                                                                        <table style="width: 100%">
                                                                                                            <tbody>
                                                                                                                <tr>
                                                                                                                    <td style="text-align: right; width: 40%;" class="Label">New Drug Status :
                                                                                                                    </td>
                                                                                                                    <td style="text-align: left;">
                                                                                                                        <asp:CheckBox ID="ChkDrugYes" onclick="ChkBox('ChkDrugYes')" runat="server" Text="Yes"></asp:CheckBox>
                                                                                                                        <asp:CheckBox ID="ChkDrugNo" onclick="ChkBox('ChkDrugNo')" runat="server" Text="No"
                                                                                                                            Checked="True"></asp:CheckBox>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td style="text-align: right;" class="Label">PermissionRequired :
                                                                                                                    </td>
                                                                                                                    <td style="text-align: left;">
                                                                                                                        <asp:CheckBox ID="ChkPermissionYes" onclick="ChkBox('ChkPermissionYes')" runat="server"
                                                                                                                            Text="Yes"></asp:CheckBox>
                                                                                                                        <asp:CheckBox ID="ChkPermissionNo" onclick="ChkBox('ChkPermissionNo')" runat="server"
                                                                                                                            Text="No" Checked="True"></asp:CheckBox>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td style="text-align: right;" class="Label">Test License Required :
                                                                                                                    </td>
                                                                                                                    <td style="text-align: left;">
                                                                                                                        <asp:CheckBox ID="ChkTestYes" onclick="ChkBox('ChkTestYes')" runat="server" Text="Yes"></asp:CheckBox>
                                                                                                                        <asp:CheckBox ID="ChkTestNo" onclick="ChkBox('ChkTestNo')" runat="server" Text="No"
                                                                                                                            Checked="True"></asp:CheckBox>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td style="text-align: right;" class="Label">Safety Issues :
                                                                                                                    </td>
                                                                                                                    <td style="text-align: left;">
                                                                                                                        <textarea style="width: 40%;" id="TxtSafty" runat="server" rows="2" cols="0"></textarea>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td style="text-align: right;" class="Label">Analytical Method :
                                                                                                                    </td>
                                                                                                                    <td style="text-align: left;">
                                                                                                                        <input style="width: 40%" id="txtAnalytical" class="textBox " maxlength="50" type="text"
                                                                                                                            runat="server" />
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td style="text-align: right;" class="Label">Method Ready :
                                                                                                                    </td>
                                                                                                                    <td style="text-align: left;">
                                                                                                                        <asp:CheckBox ID="ChkMethodYes" onclick="ChkBox('ChkMethodYes')" runat="server" Text="Yes"></asp:CheckBox>
                                                                                                                        <asp:CheckBox ID="ChkMethodNo" onclick="ChkBox('ChkMethodNo')" runat="server" Text="No"
                                                                                                                            Checked="True"></asp:CheckBox>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td style="text-align: right;" class="Label">Method Development Start Date :
                                                                                                                    </td>
                                                                                                                    <td style="text-align: left;">
                                                                                                                        <asp:TextBox ID="txtReadyStart" runat="server" CssClass="textBox" disabled="disabled"
                                                                                                                            Width="40%"> </asp:TextBox>

                                                                                                                        <img id="ImgStart"
                                                                                                                                alt="Select  Date" src="images/Calendar_scheduleHS.png" />

                                                                                                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtReadyStart"
                                                                                                                            Format="dd-MMM-yyyy" PopupButtonID="ImgStart">
                                                                                                                        </cc1:CalendarExtender>

                                                                                                                        
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td style="text-align: right;" class="Label">Method Validation End Date :
                                                                                                                    </td>
                                                                                                                    <td style="text-align: left;">
                                                                                                                        <asp:TextBox disabled="disabled" ID="txtReadyEnd" runat="server" CssClass="textBox"
                                                                                                                            Width="40%">
                                                                                                                        </asp:TextBox>

                                                                                                                        <img style="disabled: false" id="ImgEnd" alt="Select  Date" src="images/Calendar_scheduleHS.png" />

                                                                                                                        <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtReadyEnd"
                                                                                                                            Format="dd-MMM-yyyy" PopupButtonID="ImgEnd">
                                                                                                                        </cc1:CalendarExtender>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td style="text-align: right;" class="Label">Retention Period
                                                                                                                    </td>
                                                                                                                    <td style="text-align: left;">
                                                                                                                        <input style="width: 40%" id="txtRetentionPeriod" class="textBox " maxlength="9"
                                                                                                                            type="text" runat="server" />
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </tbody>
                                                                                                        </table>
                                                                                                    </ContentTemplate>
                                                                                                </asp:UpdatePanel>
                                                                                            </div>
                                                                                        </fieldset>
                                                                                    </td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                    </ContentTemplate>
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="BtnCancle" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="BtnEdit" EventName="Click" />
                                                                    </Triggers>
                                                                </asp:UpdatePanel>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                        <cc1:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1">
                                                            <HeaderTemplate>
                                                                <span style="font-size: 10pt; font-family: Verdana">Project Details Form </span>
                                                            </HeaderTemplate>
                                                            <ContentTemplate>
                                                                <table width="100%">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:UpdatePanel ID="Up_General" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                                                    <ContentTemplate>
                                                                                        <table style="width: 100%">
                                                                                            <tbody>
                                                                                                <%-- <tr>
                                                                                                    <td  align="left">
                                                                                                        <strong></strong>
                                                                                                    </td>
                                                                                                </tr>--%>
                                                                                                <%-- <tr>
                                                                                                    <td style="text-align: left;">
                                                                                                        <strong>Name of Reference Product Detail</strong>
                                                                                                        <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66; width: 100%;"
                                                                                                            class="hr" />
                                                                                                    </td>
                                                                                                </tr>--%>
                                                                                                <tr>
                                                                                                    <td align="center">

                                                                                                        <fieldset class="FieldSetBox" id="fldRefProduct" runat="server" style="margin-top: 20px; margin-right: 45px; width: 94%;">
                                                                                                            <legend class="LegendText" style="color: Black">
                                                                                                                <img id="imgRefProduct" alt="Name of Reference Product Detail" src="images/panelcollapse.png" class="imgexpand"
                                                                                                                    onclick="displayNewRequest(this,'tblRefProduct');" runat="server" style="margin-right: 2px;" />
                                                                                                                <strong style="color: #1560A1; font-size: small">Name of Reference Product Detail</strong>

                                                                                                            </legend>
                                                                                                            <div id="tblRefProduct" class="divExpand">
                                                                                                                <table style="width: 100%">
                                                                                                                    <tbody>
                                                                                                                        <tr>
                                                                                                                            <td colspan="2">
                                                                                                                                <asp:UpdatePanel ID="UpdPro1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                                                                                                    <ContentTemplate>
                                                                                                                                        <table style="width: 100%">
                                                                                                                                            <tbody>
                                                                                                                                                <tr>
                                                                                                                                                    <td style="width: 40%; text-align: right;" class="Label">Project No :
                                                                                                                                                    </td>
                                                                                                                                                    <td style="text-align: left;">
                                                                                                                                                        <input style="width: 35%" id="txtProNo" class="textBox " type="text" runat="server" /><asp:Button
                                                                                                                                                            ID="BtnGetProNo" runat="server" Text="" ToolTip="Go" CssClass="btn btngo" Font-Bold="True"></asp:Button>
                                                                                                                                                    </td>
                                                                                                                                                </tr>
                                                                                                                                                <tr>
                                                                                                                                                    <td style="text-align: right;" class="Label">Sponser Specific No :
                                                                                                                                                    </td>
                                                                                                                                                    <td style="height: 21px" align="left">
                                                                                                                                                        <input style="width: 40%" id="TxtSponserNo" class="textBox " type="text" runat="server" />
                                                                                                                                                    </td>
                                                                                                                                                </tr>
                                                                                                                                            </tbody>
                                                                                                                                        </table>
                                                                                                                                        <asp:HiddenField ID="hdnRegularityReq" runat="server" Value="" />
                                                                                                                                    </ContentTemplate>
                                                                                                                                    <Triggers>
                                                                                                                                        <asp:AsyncPostBackTrigger ControlID="BtnGetProNo" EventName="Click" />
                                                                                                                                    </Triggers>
                                                                                                                                </asp:UpdatePanel>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td style="width: 40%; text-align: right;" class="Label">Brand Name :
                                                                                                                            </td>
                                                                                                                            <td style="text-align: left;">
                                                                                                                                <input style="width: 40%" id="TxtBrand" class="textBox " type="text" runat="server"
                                                                                                                                    maxlength="50" />
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td style="text-align: right;" class="Label">Generic Name :
                                                                                                                            </td>
                                                                                                                            <td style="text-align: left;">
                                                                                                                                <input style="width: 40%" id="TxtGeneric" class="textBox " type="text" runat="server"
                                                                                                                                    maxlength="50" />
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td style="text-align: right;" class="Label">Strength :
                                                                                                                            </td>
                                                                                                                            <td style="text-align: left;">
                                                                                                                                <input style="width: 40%" id="TxtStrength" class="textBox " type="text" maxlength="10"
                                                                                                                                    runat="server" />
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td style="text-align: right;" class="Label">Formulation Type :
                                                                                                                            </td>
                                                                                                                            <td style="text-align: left;">
                                                                                                                                <input style="width: 40%" id="TxtFormulation" class="textBox " type="text" maxlength="20"
                                                                                                                                    runat="server" />
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td style="text-align: right;" class="Label">Manufacturer's Name :
                                                                                                                            </td>
                                                                                                                            <td style="text-align: left;">
                                                                                                                                <input style="width: 40%" id="TxtMfcName" class="textBox " type="text" maxlength="100"
                                                                                                                                    runat="server" />
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td style="text-align: right;" class="Label">Manufacturer's Country :
                                                                                                                            </td>
                                                                                                                            <td style="text-align: left;">
                                                                                                                                <select style="width: 40%" id="SlcMfcCountry" class="dropDownList" runat="server">
                                                                                                                                </select>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td style="text-align: right;" class="Label">Marketing Authorisation Holder's Name :
                                                                                                                            </td>
                                                                                                                            <td style="text-align: left;">
                                                                                                                                <input style="width: 40%" id="TxtMktName" class="textBox " maxlength="100" type="text"
                                                                                                                                    runat="server" />
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td style="text-align: right;" class="Label">Marketing Authorisation Holder's Country :
                                                                                                                            </td>
                                                                                                                            <td style="text-align: left;">
                                                                                                                                <select style="width: 40%" id="SlcMktCountry" class="dropDownList" runat="server">
                                                                                                                                </select>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td style="text-align: right;" class="Label">Lot/Batch No. :
                                                                                                                            </td>
                                                                                                                            <td style="text-align: left;">
                                                                                                                                <input style="width: 40%" id="TxtLot" class="textBox " maxlength="10" type="text"
                                                                                                                                    runat="server" />
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </tbody>
                                                                                                                </table>
                                                                                                            </div>
                                                                                                        </fieldset>


                                                                                                    </td>
                                                                                                </tr>
                                                                                                <%-- <tr>
                                                                                                    <td style="text-align: left;">
                                                                                                        <strong>Name of Test Product Detail </strong>
                                                                                                        <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66; width: 100%;"
                                                                                                            class="hr" />
                                                                                                    </td>
                                                                                                </tr>--%>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <fieldset class="FieldSetBox" id="fldProductDtl" runat="server" style="margin-top: 20px; width: 94%;">
                                                                                                            <legend class="LegendText" style="color: Black">
                                                                                                                <img id="imgProductDtl" alt="Initial Quotes Information" src="images/panelcollapse.png" class="imgexpand"
                                                                                                                    onclick="displayNewRequest(this,'tblProductDtl');" runat="server" style="margin-right: 2px;" />
                                                                                                                <strong style="color: #1560A1; font-size: small">Name of Test Product Detail</strong>

                                                                                                            </legend>
                                                                                                            <div id="tblProductDtl" class="divExpand">
                                                                                                                <table width="100%">
                                                                                                                    <tbody>
                                                                                                                        <tr>
                                                                                                                            <td style="text-align: right; width: 40%;" class="Label">Generic Name :
                                                                                                                            </td>
                                                                                                                            <td style="text-align: left;">
                                                                                                                                <input style="width: 40%" id="TxtGenericT" class="textBox " type="text" runat="server"
                                                                                                                                    maxlength="50" />
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td style="text-align: right;" class="Label">Strength :
                                                                                                                            </td>
                                                                                                                            <td style="text-align: left;">
                                                                                                                                <input style="width: 40%" id="TxtStrengthT" class="textBox " type="text" runat="server"
                                                                                                                                    maxlength="4" />
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td style="text-align: right;" class="Label">Formulation Type :
                                                                                                                            </td>
                                                                                                                            <td style="text-align: left;">
                                                                                                                                <input style="width: 40%" id="TxtFormulationT" class="textBox " type="text" runat="server"
                                                                                                                                    maxlength="20" />
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td style="text-align: right;" class="Label">Manufacturer's Name :
                                                                                                                            </td>
                                                                                                                            <td style="text-align: left;">
                                                                                                                                <input style="width: 40%" id="TxtMfcNameT" class="textBox " type="text" runat="server"
                                                                                                                                    maxlength="100" />
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td style="text-align: right;" class="Label">Manufacturer's Country :
                                                                                                                            </td>
                                                                                                                            <td style="text-align: left;">
                                                                                                                                <select style="width: 40%" id="SlcMfcCountryT" class="dropDownList" runat="server">
                                                                                                                                </select>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td style="text-align: right;" class="Label">Lot/Batch No. :
                                                                                                                            </td>
                                                                                                                            <td style="text-align: left;">
                                                                                                                                <input style="width: 40%" id="TxtLotT" class="textBox " type="text" runat="server"
                                                                                                                                    maxlength="10" />
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td style="text-align: right;" class="Label">Dose of the Test/Reference Product to be administered :
                                                                                                                            </td>
                                                                                                                            <td style="text-align: left;">
                                                                                                                                <input style="width: 40%" id="TxtDose" class="textBox " type="text" runat="server"
                                                                                                                                    maxlength="10" />
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </tbody>
                                                                                                                </table>
                                                                                                            </div>
                                                                                                        </fieldset>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>
                                                                                    </ContentTemplate>
                                                                                    <Triggers>
                                                                                        <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
                                                                                        <asp:AsyncPostBackTrigger ControlID="BtnCancle" EventName="Click" />
                                                                                    </Triggers>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <table width="100%">
                                                                                    <tbody>
                                                                                        <%-- <tr>
                                                                                            <td style="text-align: left;">
                                                                                                <strong>Sponsor's Authorised Signatory to include </strong>
                                                                                                <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66; width: 100%;"
                                                                                                    class="hr" />
                                                                                            </td>
                                                                                        </tr>--%>

                                                                                        <tr>
                                                                                            <td>
                                                                                                <fieldset class="FieldSetBox" id="fldAuthorised" runat="server" style="margin-top: 20px; width: 94%;">
                                                                                                    <legend class="LegendText" style="color: Black">
                                                                                                        <img id="imgAuthorised" alt="Sponsor's Authorised Signatory to include " src="images/panelcollapse.png" class="imgexpand"
                                                                                                            onclick="displayNewRequest(this,'tblAuthorised');" runat="server" style="margin-right: 2px;" />
                                                                                                        <strong style="color: #1560A1; font-size: small">Sponsor's Authorised Signatory to include </strong>

                                                                                                    </legend>
                                                                                                    <div id="tblAuthorised" class="divExpand">
                                                                                                        <table width="100%">
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <asp:UpdatePanel ID="Up_Authorised1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                                                                                        <ContentTemplate>

                                                                                                                            <table style="width: 100%">
                                                                                                                                <tbody>
                                                                                                                                    <tr>
                                                                                                                                        <td style="text-align: right; width: 40%;" class="Label">Name :
                                                                                                                                        </td>
                                                                                                                                        <td style="text-align: left;">
                                                                                                                                            <input onblur="onText('AName')" style="width: 40%" id="TxtAName" class="textBox "
                                                                                                                                                type="text" runat="server" maxlength="50" />
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                    <tr>
                                                                                                                                        <td style="text-align: right;" class="Label">Address 1 :
                                                                                                                                        </td>
                                                                                                                                        <td style="text-align: left;">
                                                                                                                                            <input onblur="onText('AAddr1')" style="width: 40%" id="TxtAAddr1" class="textBox "
                                                                                                                                                type="text" runat="server" maxlength="100" />
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                    <tr>
                                                                                                                                        <td style="text-align: right;" class="Label">Address 2 :
                                                                                                                                        </td>
                                                                                                                                        <td style="text-align: left;">
                                                                                                                                            <input onblur="onText('AAddr2')" style="width: 40%" id="TxtAAddr2" class="textBox "
                                                                                                                                                type="text" runat="server" maxlength="100" />
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                    <tr>
                                                                                                                                        <td style="text-align: right;" class="Label">Address 3 :
                                                                                                                                        </td>
                                                                                                                                        <td style="text-align: left;">
                                                                                                                                            <input onblur="onText('AAddr3')" style="width: 40%" id="TxtAAddr3" class="textBox "
                                                                                                                                                type="text" runat="server" maxlength="100" />
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                    <tr>
                                                                                                                                        <td style="text-align: right;" class="Label">Telephone No. :
                                                                                                                                        </td>
                                                                                                                                        <td style="text-align: left;">
                                                                                                                                            <input onblur="onText('ATel')" style="width: 40%" id="TxtATel" class="textBox " type="text"
                                                                                                                                                runat="server" maxlength="20" />
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                    <tr>
                                                                                                                                        <td style="text-align: right;" class="Label">Extention No. :
                                                                                                                                        </td>
                                                                                                                                        <td style="text-align: left;">
                                                                                                                                            <input onblur="onText('AExt')" style="width: 40%" id="TxtAExt" class="textBox " type="text"
                                                                                                                                                runat="server" maxlength="4" />
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                    <tr>
                                                                                                                                        <td style="text-align: right;" class="Label">Designation :
                                                                                                                                        </td>
                                                                                                                                        <td style="text-align: left;">
                                                                                                                                            <input onblur="onText('ADesig')" style="width: 40%" id="TxtADesig" class="textBox "
                                                                                                                                                type="text" runat="server" maxlength="50" />
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                    <tr>
                                                                                                                                        <td style="text-align: right;" class="Label">Qualification :
                                                                                                                                        </td>
                                                                                                                                        <td style="text-align: left;">
                                                                                                                                            <input onblur="onText('AQuali')" style="width: 40%" id="TxtAQuali" class="textBox "
                                                                                                                                                type="text" runat="server" maxlength="50" />
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                </tbody>
                                                                                                                            </table>
                                                                                                                        </ContentTemplate>
                                                                                                                        <Triggers>
                                                                                                                            <asp:AsyncPostBackTrigger ControlID="BtnAAdd" EventName="ServerClick" />
                                                                                                                            <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
                                                                                                                            <asp:AsyncPostBackTrigger ControlID="BtnCancle" EventName="Click" />
                                                                                                                        </Triggers>
                                                                                                                    </asp:UpdatePanel>
                                                                                                                    <input id="BtnAAdd" class="btn btnnew" type="button" value="Add" title="Add" runat="server" style="margin-left: 50%;" />

                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <asp:UpdatePanel ID="Up_Authorised2" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                                                                                        <ContentTemplate>
                                                                                                                            <asp:GridView ID="GV_Authorised" runat="server" SkinID="grdView" OnRowCommand="GV_Authorised_RowCommand"
                                                                                                                                OnRowDeleting="GV_Authorised_RowDeleting" OnRowDataBound="GV_Authorised_RowDataBound"
                                                                                                                                AutoGenerateColumns="False">
                                                                                                                                <Columns>
                                                                                                                                    <asp:BoundField DataField="vType" HeaderText="Type" />
                                                                                                                                    <asp:BoundField DataField="vName" HeaderText="Name" />
                                                                                                                                    <asp:BoundField DataField="vAddress1" HeaderText="Address1" />
                                                                                                                                    <asp:BoundField DataField="vAddress2" HeaderText="Address2" />
                                                                                                                                    <asp:BoundField DataField="vAddress3" HeaderText="Address3" />
                                                                                                                                    <asp:BoundField DataField="vTeleNo" HeaderText="Telephone No." />
                                                                                                                                    <asp:BoundField DataField="vExtNo" HeaderText="Extention No." />
                                                                                                                                    <asp:BoundField DataField="vDesignation" HeaderText="Designation" />
                                                                                                                                    <asp:BoundField DataField="vQualification" HeaderText="Qualification" />
                                                                                                                                    <asp:TemplateField HeaderText="Delete">
                                                                                                                                        <ItemTemplate>
                                                                                                                                            <asp:ImageButton ID="ImbADelete" runat="server" ToolTip="Delete" ImageUrl="~/Images/i_delete.gif"></asp:ImageButton>
                                                                                                                                        </ItemTemplate>
                                                                                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Bottom" />
                                                                                                                                    </asp:TemplateField>
                                                                                                                                </Columns>
                                                                                                                            </asp:GridView>
                                                                                                                        </ContentTemplate>
                                                                                                                        <Triggers>
                                                                                                                            <asp:AsyncPostBackTrigger ControlID="BtnAAdd" EventName="ServerClick" />
                                                                                                                            <asp:AsyncPostBackTrigger ControlID="BtnCancle" EventName="Click" />
                                                                                                                            <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
                                                                                                                        </Triggers>
                                                                                                                    </asp:UpdatePanel>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </div>
                                                                                                </fieldset>

                                                                                            </td>
                                                                                        </tr>

                                                                                        <%--<tr>
                                                                                            <td style="text-align: left;">
                                                                                                <strong>Sponsor's Medical Expert to include </strong>
                                                                                                <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66; width: 100%;"
                                                                                                    class="hr" />
                                                                                            </td>
                                                                                        </tr>--%>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <fieldset class="FieldSetBox" id="fldMedical1" runat="server" style="margin-top: 20px; width: 94%;">
                                                                                                    <legend class="LegendText" style="color: Black">
                                                                                                        <img id="imgMedical1" alt="Sponsor's Medical Expert to include" src="images/panelcollapse.png" class="imgexpand"
                                                                                                            onclick="displayNewRequest(this,'tblMedical1');" runat="server" style="margin-right: 2px;" />
                                                                                                        <strong style="color: #1560A1; font-size: small">Sponsor's Medical Expert to include</strong>

                                                                                                    </legend>
                                                                                                    <div id="tblMedical1" class="divExpand">
                                                                                                        <table width="100%">
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                            <asp:UpdatePanel ID="Up_Medical1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                                                                                                <ContentTemplate>
                                                                                                                                    <table style="width: 100%">
                                                                                                                                        <tbody>
                                                                                                                                            <tr>
                                                                                                                                                <td style="text-align: right; width: 40%;" class="Label">Name :
                                                                                                                                                </td>
                                                                                                                                                <td style="text-align: left;">
                                                                                                                                                    <input style="width: 40%" id="TxtMName" class="textBox " type="text" runat="server"
                                                                                                                                                        maxlength="50" />
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                            <tr>
                                                                                                                                                <td style="text-align: right;" class="Label">Address 1 :
                                                                                                                                                </td>
                                                                                                                                                <td style="text-align: left;">
                                                                                                                                                    <input style="width: 40%" id="TxtMAddr1" class="textBox " type="text" runat="server"
                                                                                                                                                        maxlength="100" />
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                            <tr>
                                                                                                                                                <td style="text-align: right;" class="Label">Address 2 :
                                                                                                                                                </td>
                                                                                                                                                <td style="text-align: left;">
                                                                                                                                                    <input style="width: 40%" id="TxtMAddr2" class="textBox " type="text" runat="server"
                                                                                                                                                        maxlength="100" />
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                            <tr>
                                                                                                                                                <td style="text-align: right;" class="Label">Address 3 :
                                                                                                                                                </td>
                                                                                                                                                <td style="text-align: left;">
                                                                                                                                                    <input style="width: 40%" id="TxtMAddr3" class="textBox " type="text" runat="server"
                                                                                                                                                        maxlength="100" />
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                            <tr>
                                                                                                                                                <td style="text-align: right;" class="Label">Telephone No. :
                                                                                                                                                </td>
                                                                                                                                                <td style="text-align: left;">
                                                                                                                                                    <input style="width: 40%" id="TxtMTel" class="textBox " type="text" runat="server"
                                                                                                                                                        maxlength="20" />
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                            <tr>
                                                                                                                                                <td style="text-align: right;" class="Label">Extention No. :
                                                                                                                                                </td>
                                                                                                                                                <td style="text-align: left;">
                                                                                                                                                    <input style="width: 40%" id="TxtMExt" class="textBox " type="text" runat="server"
                                                                                                                                                        maxlength="4" />
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                            <tr>
                                                                                                                                                <td style="text-align: right;" class="Label">Designation :
                                                                                                                                                </td>
                                                                                                                                                <td style="text-align: left;">
                                                                                                                                                    <input style="width: 40%" id="TxtMDesig" class="textBox " type="text" runat="server"
                                                                                                                                                        maxlength="50" />
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                            <tr>
                                                                                                                                                <td style="text-align: right;" class="Label">Qualification :
                                                                                                                                                </td>
                                                                                                                                                <td style="text-align: left;">
                                                                                                                                                    <input style="width: 40%" id="TxtMQuali" class="textBox " type="text" runat="server"
                                                                                                                                                        maxlength="50" />
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                        </tbody>
                                                                                                                                    </table>
                                                                                                                                </ContentTemplate>
                                                                                                                                <Triggers>
                                                                                                                                    <asp:AsyncPostBackTrigger ControlID="BtnMAdd" EventName="ServerClick" />
                                                                                                                                    <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
                                                                                                                                    <asp:AsyncPostBackTrigger ControlID="BtnCancle" EventName="Click" />
                                                                                                                                </Triggers>
                                                                                                                            </asp:UpdatePanel>
                                                                                                                            <input id="BtnMAdd" class="btn btnnew" type="button" value="Add" title="Add" runat="server" style="margin-left: 50%;" />

                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                            <asp:UpdatePanel ID="Up_Medical2" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                                                                                                <ContentTemplate>
                                                                                                                                    <asp:GridView ID="GV_Medical" runat="server" SkinID="grdView" OnRowCommand="GV_Medical_RowCommand"
                                                                                                                                        OnRowDeleting="GV_Medical_RowDeleting" OnRowDataBound="GV_Medical_RowDataBound"
                                                                                                                                        AutoGenerateColumns="False">
                                                                                                                                        <Columns>
                                                                                                                                            <asp:BoundField DataField="vType" HeaderText="Type" />
                                                                                                                                            <asp:BoundField DataField="vName" HeaderText="Name" />
                                                                                                                                            <asp:BoundField DataField="vAddress1" HeaderText="Address1" />
                                                                                                                                            <asp:BoundField DataField="vAddress2" HeaderText="Address2" />
                                                                                                                                            <asp:BoundField DataField="vAddress3" HeaderText="Address3" />
                                                                                                                                            <asp:BoundField DataField="vTeleNo" HeaderText="Telephone No." />
                                                                                                                                            <asp:BoundField DataField="vExtNo" HeaderText="Extention No." />
                                                                                                                                            <asp:BoundField DataField="vDesignation" HeaderText="Designation" />
                                                                                                                                            <asp:BoundField DataField="vQualification" HeaderText="Qualification" />
                                                                                                                                            <asp:TemplateField HeaderText="Delete">
                                                                                                                                                <ItemTemplate>
                                                                                                                                                    <asp:ImageButton ID="ImbMDelete" runat="server" ToolTip="Delete" ImageUrl="~/Images/i_delete.gif"></asp:ImageButton>
                                                                                                                                                </ItemTemplate>
                                                                                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                                                                            </asp:TemplateField>
                                                                                                                                        </Columns>
                                                                                                                                    </asp:GridView>
                                                                                                                                </ContentTemplate>
                                                                                                                                <Triggers>
                                                                                                                                    <asp:AsyncPostBackTrigger ControlID="BtnMAdd" EventName="ServerClick" />
                                                                                                                                    <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
                                                                                                                                    <asp:AsyncPostBackTrigger ControlID="BtnCancle" EventName="Click" />
                                                                                                                                </Triggers>
                                                                                                                            </asp:UpdatePanel>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </div>
                                                                                                </fieldset>
                                                                                            </td>
                                                                                        </tr>




                                                                                        <%-- <tr>
                                                                                            <td style="text-align: left;">
                                                                                                <strong>Sponsor's Contact Details to include</strong>
                                                                                                <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66; width: 100%;"
                                                                                                    class="hr" />
                                                                                            </td>
                                                                                        </tr>--%>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <fieldset class="FieldSetBox" id="fldContact" runat="server" style="margin-top: 20px; width: 94%;">
                                                                                                    <legend class="LegendText" style="color: Black">
                                                                                                        <img id="imgContact" alt="Sponsor's Medical Expert to include" src="images/panelcollapse.png" class="imgexpand"
                                                                                                            onclick="displayNewRequest(this,'tblContact');" runat="server" style="margin-right: 2px;" />
                                                                                                        <strong style="color: #1560A1; font-size: small">Sponsor's Contact Details to include</strong>

                                                                                                    </legend>
                                                                                                    <div id="tblContact" class="divExpand">
                                                                                                        <table width="100%">
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                            <asp:UpdatePanel ID="Up_Contact1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                                                                                                <ContentTemplate>
                                                                                                                                    <table style="width: 100%">
                                                                                                                                        <tbody>
                                                                                                                                            <tr>
                                                                                                                                                <td style="text-align: right; width: 40%;" class="Label">Address 1 :
                                                                                                                                                </td>
                                                                                                                                                <td style="text-align: left">
                                                                                                                                                    <input style="width: 40%" id="TxtCAddr1" class="textBox " type="text" runat="server"
                                                                                                                                                        maxlength="100" />
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                            <tr>
                                                                                                                                                <td style="text-align: right;" class="Label">Address 2 :
                                                                                                                                                </td>
                                                                                                                                                <td style="text-align: left">
                                                                                                                                                    <input style="width: 40%" id="TxtCAddr2" class="textBox " type="text" runat="server"
                                                                                                                                                        maxlength="100" />
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                            <tr>
                                                                                                                                                <td style="text-align: right;" class="Label">Address 3 :
                                                                                                                                                </td>
                                                                                                                                                <td style="text-align: left">
                                                                                                                                                    <input style="width: 40%" id="TxtCAddr3" class="textBox " type="text" runat="server"
                                                                                                                                                        maxlength="100" />
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                            <tr>
                                                                                                                                                <td style="text-align: right;" class="Label">Telephone No. :
                                                                                                                                                </td>
                                                                                                                                                <td style="text-align: left">
                                                                                                                                                    <input style="width: 40%" id="TxtCTel" class="textBox " type="text" runat="server"
                                                                                                                                                        maxlength="20" />
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                            <tr>
                                                                                                                                                <td style="text-align: right;" class="Label">Extention No. :
                                                                                                                                                </td>
                                                                                                                                                <td style="text-align: left">
                                                                                                                                                    <input style="width: 40%" id="TxtCExt" class="textBox " type="text" runat="server"
                                                                                                                                                        maxlength="4" />
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                            <tr>
                                                                                                                                                <td style="text-align: right;" class="Label">Fax No :
                                                                                                                                                </td>
                                                                                                                                                <td style="text-align: left">
                                                                                                                                                    <input style="width: 40%" id="TxtCFax" class="textBox " type="text" runat="server"
                                                                                                                                                        maxlength="20" />
                                                                                                                                                </td>
                                                                                                                                            </tr>
                                                                                                                                        </tbody>
                                                                                                                                    </table>
                                                                                                                                </ContentTemplate>
                                                                                                                                <Triggers>
                                                                                                                                    <asp:AsyncPostBackTrigger ControlID="BtnCAdd" EventName="ServerClick" />
                                                                                                                                    <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
                                                                                                                                    <asp:AsyncPostBackTrigger ControlID="BtnCancle" EventName="Click" />
                                                                                                                                </Triggers>
                                                                                                                            </asp:UpdatePanel>
                                                                                                                            <input id="BtnCAdd" class="btn btnnew" type="button" value="Add" title="Add" runat="server" style="margin-left: 50%;" />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td>
                                                                                                                            <asp:UpdatePanel ID="Up_Contact2" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                                                                                                <ContentTemplate>
                                                                                                                                    <asp:GridView ID="GV_Contact" runat="server" SkinID="grdView" OnRowCommand="GV_Contact_RowCommand"
                                                                                                                                        OnRowDeleting="GV_Contact_RowDeleting" OnRowDataBound="GV_Contact_RowDataBound"
                                                                                                                                        AutoGenerateColumns="False">
                                                                                                                                        <Columns>
                                                                                                                                            <asp:BoundField DataField="vType" HeaderText="Type" />
                                                                                                                                            <asp:BoundField DataField="vName" HeaderText="Name" />
                                                                                                                                            <asp:BoundField DataField="vAddress1" HeaderText="Address1" />
                                                                                                                                            <asp:BoundField DataField="vAddress2" HeaderText="Address2" />
                                                                                                                                            <asp:BoundField DataField="vAddress3" HeaderText="Address3" />
                                                                                                                                            <asp:BoundField DataField="vTeleNo" HeaderText="Telephone No." />
                                                                                                                                            <asp:BoundField DataField="vExtNo" HeaderText="Extention No." />
                                                                                                                                            <asp:BoundField DataField="vFaxNo" HeaderText="Fax No." />
                                                                                                                                            <asp:BoundField DataField="vDesignation" HeaderText="Designation" />
                                                                                                                                            <asp:BoundField DataField="vQualification" HeaderText="Qualification" />
                                                                                                                                            <asp:TemplateField HeaderText="Delete">
                                                                                                                                                <ItemTemplate>
                                                                                                                                                    <asp:ImageButton ID="ImbCDelete" runat="server" ToolTip="Delete" ImageUrl="~/Images/i_delete.gif"></asp:ImageButton>
                                                                                                                                                </ItemTemplate>
                                                                                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                                                                            </asp:TemplateField>
                                                                                                                                        </Columns>
                                                                                                                                    </asp:GridView>
                                                                                                                                </ContentTemplate>
                                                                                                                                <Triggers>
                                                                                                                                    <asp:AsyncPostBackTrigger ControlID="BtnCAdd" EventName="ServerClick" />
                                                                                                                                    <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
                                                                                                                                    <asp:AsyncPostBackTrigger ControlID="BtnCancle" EventName="Click" />
                                                                                                                                </Triggers>
                                                                                                                            </asp:UpdatePanel>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td style="text-align: center;">Remarks :
                                                                                                <textarea style="width: 30%; vertical-align: bottom;" wrap="hard" id="TxtRemarks"
                                                                                                    runat="server" rows="2" cols="0" />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </div>
                                                                                                </fieldset>
                                                                                            </td>
                                                                                        </tr>


                                                                                    </tbody>
                                                                                </table>

                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </ContentTemplate>
                                                        </cc1:TabPanel>
                                                    </cc1:TabContainer><asp:Button ID="BtnSave" runat="server" Text="Save" ToolTip="Save"
                                                        CssClass="btn btnsave" Font-Bold="True" OnClientClick="return Validation();"></asp:Button>
                                                    <asp:Button ID="BtnCancle" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="btn btncancel"
                                                        Font-Bold="True"></asp:Button>
                                                    <asp:Button ID="btnExit" OnClick="btnExit_Click" runat="server" Text="Exit" ToolTip="Exit"
                                                        CssClass="btn btnexit" Font-Bold="True" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);"></asp:Button>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="BtnCancle" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="BtnEdit" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="BtnEdit" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>

    </div>

    <script type="text/javascript" language="javascript">

        function ChkBox(type) {
            if (type == 'ChkDrugYes') {
                document.getElementById('<%=ChkDrugYes.ClientID%>').checked = true;
                document.getElementById('<%=ChkDrugNo.ClientID%>').checked = false;
                return true;

            }
            else if (type == 'ChkDrugNo') {
                document.getElementById('<%=ChkDrugNo.ClientID%>').checked = true;
                document.getElementById('<%=ChkDrugYes.ClientID%>').checked = false;
                return true;
            }
            else if (type == 'ChkPermissionYes') {
                document.getElementById('<%=ChkPermissionYes.ClientID%>').checked = true;
                document.getElementById('<%=ChkPermissionNo.ClientID%>').checked = false;
                return true;
            }
            else if (type == 'ChkPermissionNo') {
                document.getElementById('<%=ChkPermissionNo.ClientID%>').checked = true;
                document.getElementById('<%=ChkPermissionYes.ClientID%>').checked = false;
                return true;
            }
            else if (type == 'ChkMethodYes') {
                document.getElementById('<%=ChkMethodYes.ClientID%>').checked = true;
                document.getElementById('<%=ChkMethodNo.ClientID%>').checked = false;

                document.getElementById('<%=txtReadyStart.ClientID%>').value = '---'
                document.getElementById('<%=txtReadyEnd.ClientID%>').value = '---'

                document.getElementById('<%=txtReadyStart.ClientID%>').disabled = true;
                document.getElementById('<%=txtReadyEnd.ClientID%>').disabled = true;

                document.getElementById('ImgStart').style.display = 'none'
                document.getElementById('ImgEnd').style.display = 'none'

                document.getElementById('ImgStart').disabled = true;
                document.getElementById('ImgEnd').disabled = true;
                return true;
            }
            else if (type == 'ChkMethodNo') {
                document.getElementById('<%=ChkMethodNo.ClientID%>').checked = true;
                document.getElementById('<%=ChkMethodYes.ClientID%>').checked = false;

                document.getElementById('<%=txtReadyStart.ClientID%>').value = ''
                document.getElementById('<%=txtReadyEnd.ClientID%>').value = ''

                document.getElementById('<%=txtReadyStart.ClientID%>').disabled = false;
                document.getElementById('<%=txtReadyEnd.ClientID%>').disabled = false;

                document.getElementById('ImgStart').style.display = 'inline'
                document.getElementById('ImgEnd').style.display = 'inline'
                document.getElementById('ImgStart').disabled = false;
                document.getElementById('ImgEnd').disabled = false;
                return true;
            }
            else if (type == 'ChkTestYes') {
                document.getElementById('<%=ChkTestYes.ClientID%>').checked = true;
                document.getElementById('<%=ChkTestNo.ClientID%>').checked = false;
                return true;
            }
            else if (type == 'ChkTestNo') {
                document.getElementById('<%=ChkTestNo.ClientID%>').checked = true;
                document.getElementById('<%=ChkTestYes.ClientID%>').checked = false;
                return true;
            }
            else if (type == 'ChkfedYes') {
                document.getElementById('<%=ChkfastfedYes.ClientID%>').checked = true;
                document.getElementById('<%=ChkfastfedNo.ClientID%>').checked = false;
                return true;
            }
            else if (type == 'ChkfedNo') {
                document.getElementById('<%=ChkfastfedNo.ClientID%>').checked = true;
                document.getElementById('<%=ChkfastfedYes.ClientID%>').checked = false;
                return true;
            }


}

function onText(Type) {
    return true;
}

function Validation() {

    if (document.getElementById('<%=SlcTemplate.ClientID%>').disabled == false && document.getElementById('<%=SlcTemplate.ClientID%>').selectedIndex == 0) {
        msgalert('Please Select Template !');
        //    document.getElementById('<%=SlcTemplate.ClientID%>').focus();
        return false;
    }
    else if (document.getElementById('<%=SlcProject.ClientID%>').disabled == false && document.getElementById('<%=SlcProject.ClientID%>').selectedIndex == 0) {
        msgalert('Please Select Project Type !');
        return false;
    }
    else if (document.getElementById('<%=ddlStudytype.ClientID%>').disabled == false && document.getElementById('<%=ddlStudytype.ClientID%>').selectedIndex == 0) {
        msgalert('Please Select Study Type !');
        return false;
    }

    else if (document.getElementById('<%=SlcSponsor.ClientID%>').disabled == false && document.getElementById('<%=SlcSponsor.ClientID%>').selectedIndex == 0) {
        msgalert('Please Select Sponsor !');
        return false;
    }
    else if (document.getElementById('<%=slcLocation.ClientID%>').disabled == false && document.getElementById('<%=slcLocation.ClientID%>').selectedIndex == 0) {
        msgalert('Please Select Location !');
        return false;
    }
    else if (document.getElementById('<%=SlcManager.ClientID%>').disabled == false && document.getElementById('<%=SlcManager.ClientID%>').selectedIndex == 0) {
        msgalert('Please Select Project Manager !');
        return false;
    }

    else if (document.getElementById('<%=TxtSubNo.ClientID%>').value == "") {
        msgalert("Please Enter Number Of Subject !");
        return false;
    }

    else if (! $("#<%= rBtnEthics.ClientID %>  tbody tr td input:radio").is(':checked')) {
        msgalert('Please Select atleast one  Ethical Approval Required !');
        return false;
    }

    else if (document.getElementById('<%=SlcDrug.ClientID%>').disabled == false && $("#ctl00_CPHLAMBDA_TabContainer1_TabPanel2_SlcDrug option:selected").text() == "Select Drug") {
        msgalert('Please Select Drug !');
        return false;
    }
    else if (document.getElementById('<%=SlcSubmission.ClientID%>').disabled == false && document.getElementById('<%=SlcSubmission.ClientID%>').selectedIndex == 0) {
        msgalert('Please Select Submission !');
        return false;
    }
    else if (document.getElementById('<%=txtProNo.ClientID%>').value == "") {
        msgalert('Please Enter Project No !');
        return false;
    }

    
    
    return true;

}
function chkNumeric() {
    var val = document.getElementById('<%= txtRetentionPeriod.clientId %>').value.toString().trim();
    isNumeric(val, $get('<%= txtRetentionPeriod.clientId %>'));
}
function RegularityRequired() {
    $('#ctl00_CPHLAMBDA_TabContainer1_TabPanel2_ddlRegularityReq').multiselect({
        includeSelectAllOption: true
    });
}

function pageLoad() {
    if (getQueryStringValue("mode") == 1 || document.getElementById('<%=HWorkspaceId.ClientID%>').value != "") {
        if (document.getElementById('<%=hdnPostBack.ClientID%>').value == "true") {
            onLoad();
        }
        document.getElementById('<%=hdnPostBack.ClientID%>').value="true";
        fnApplyRegularityReq();
    }
}
function getQueryStringValue(key) {
    return unescape(window.location.search.replace(new RegExp("^(?:.*[&\\?]" + escape(key).replace(/[\.\+\*]/g, "\\$&") + "(?:\\=([^&]*))?)?.*$", "i"), "$1"));
}
function onLoad() {

    $(".divExpand").each(function (index) {
        $('#' + this.id).slideUp(0);
    });

    $(".imgexpand").each(function (index) {
        $('#' + this.id)[0].src = "images/panelexpand.png";
    });
    $("#" + 'tblProjectDetail').slideDown();
    $('#ctl00_CPHLAMBDA_TabContainer1_TabPanel2_imgprojectdetail')[0].src = "images/panelcollapse.png";

}

function ClientPopulated(sender, e) {
    var searchText = $get('<%= txtsearch.ClientId %>');
    ProjectClientShowing('AutoCompleteExtender1', searchText);
}

function OnSelected(sender, e) {
    ProjectOnItemSelected(e.get_value(), $get('<%= txtsearch.clientid %>'), $get('<%= hworkspaceid.clientid %>'), document.getElementById('<%=BtnEdit.ClientID%>'));
}

function RedirectPage(Msg) {
    msgalert(Msg);
    window.location.href = 'frmProtocolDetail.aspx?' + window.location.search.substring(1).split("&")[0];

}
Sys.Browser.WebKit = {};
if (navigator.userAgent.indexOf('WebKit/') > -1) {
    Sys.Browser.agent = Sys.Browser.WebKit;
    Sys.Browser.version = parseFloat(navigator.userAgent.match(/WebKit\/(\d+(\.\d+)?)/)[1]);
    Sys.Browser.name = 'WebKit';
}

//Add by shivani pandya for project lock
function getData() {
    var WorkspaceID = $('input[id$=HWorkspaceId]').val();
    $.ajax({
        type: "post",
        url: "frmProtocolDetail.aspx/LockImpact",
        data: '{"WorkspaceID":"' + WorkspaceID + '"}',
        contentType: "application/json; charset=utf-8",
        datatype: JSON,
        async: false,
        dataType: "json",
        success: function (data) {
            if (data.d == "L") {
                msgalert("Project is locked !");
                $("#ctl00_CPHLAMBDA_TabContainer1_body select").attr("Disabled", "Disabled");
                $("#ctl00_CPHLAMBDA_TabContainer1_body [type=checkbox]").attr("Disabled", "Disabled");
                $("#ctl00_CPHLAMBDA_TabContainer1_body [type=text]").attr("Disabled", "Disabled");
                $("#ctl00_CPHLAMBDA_TabContainer1_body textarea").attr("Disabled", "Disabled");
                $("#ctl00_CPHLAMBDA_BtnSave").attr("style", "Display:none");
                $("#ctl00_CPHLAMBDA_TabContainer1_body img").removeAttr("onClick");
                $("#ctl00_CPHLAMBDA_TabContainer1_body [title=Add]").attr("Disabled", "Disabled");
            }
        },
        failure: function (response) {
            msgalert(response.d);
        },
        error: function (response) {
            msgalert(response.d);
        }
    });
    return true;
}

/// Addded by ketan
function displayNewRequest(control, target) {

    $(".divExpand").each(function (index) {
        $('#' + this.id).slideUp();
    });
    $(".imgexpand").each(function (index) {
        $('#' + this.id)[0].src = "images/panelexpand.png";
    });
    if (control.src.toString().toUpperCase().search("EXPAND") != -1) {
        $("#" + target).slideToggle(600);
        control.src = "images/panelcollapse.png";
    }
    else {
        $("#" + target).slideToggle(600);
        control.src = "images/panelexpand.png";
    }
}

function SlideToggle(control, target) {

    $(".divExpand").each(function (index) {
        $('#' + this.id).slideUp();
    });
    $(".imgexpand").each(function (index) {
        $('#' + this.id)[0].src = "images/panelexpand.png";
    });

    $("#" + target.id).slideDown();
    $("#" + control.id)[0].src = "images/panelcollapse.png";

}
function ActiveTabChanged(sender, e) {
    $(".divExpand").each(function (index) {
        $('#' + this.id).slideUp(0);
    });

    $(".imgexpand").each(function (index) {
        $('#' + this.id)[0].src = "images/panelexpand.png";
    });

    $("#" + 'tblProjectDetail').slideDown();
    $('#ctl00_CPHLAMBDA_TabContainer1_TabPanel2_imgprojectdetail')[0].src = "images/panelcollapse.png";

    $("#" + 'tblRefProduct').slideDown();
    $('#ctl00_CPHLAMBDA_TabContainer1_TabPanel1_imgRefProduct')[0].src = "images/panelcollapse.png";

}


/// ended by ketan

function fnRegularityReq() {
    var ProjectManager = [];
    //Subject = [];
    document.getElementById('<%= hdnRegularityReq.ClientID%>').value = "";
            for (i = 0; i < $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_TabContainer1_TabPanel2_ddlRegularityReq']:checked").length ; i++) {
                ProjectManager.push("'" + $(".ui-multiselect-checkboxes.ui-helper-reset input[name='multiselect_ctl00_CPHLAMBDA_TabContainer1_TabPanel2_ddlRegularityReq']:checked").eq(i).attr("value") + "'");
            }
            document.getElementById('<%= hdnRegularityReq.ClientID%>').value = ProjectManager;
            return true;
        }

        var RegularityReq = [];
        function fnApplyRegularityReq() {
            // fnDeletePreviousMultiselect();
            debugger;
            $("#<%= ddlRegularityReq.ClientID%>").multiselect({
                noneSelectedText: "Select Regulatory Requirement",
                click: function (event, ui) {
                    if (ui.checked == true)
                        RegularityReq.push("'" + ui.value + "'");
                    else if (ui.checked == false) {
                        if ($.inArray("'" + ui.value + "'", RegularityReq) >= 0)
                            RegularityReq.splice(RegularityReq.indexOf("'" + ui.value + "'"), 1)
                    }
                    if ($("input[name$='ddlRegularityReq']").length > 0) {
                        //clearControls();
                    }
                },
                checkAll: function (event, ui) {
                    RegularityReq = [];
                    for (var i = 0; i < event.target.options.length; i++) {
                        RegularityReq.push("'" + $(event.target.options[i]).val() + "'")
                    }

                },
                uncheckAll: function (event, ui) {
                    RegularityReq = [];

                }
            });

            $("#<%= ddlRegularityReq.ClientID%>").multiselect("refresh");
            var CheckedCheckBox = document.getElementById('<%= hdnRegularityReq.ClientID%>').value
            if (CheckedCheckBox != "") {

                CheckedCheckBox = CheckedCheckBox.split(',');
                for (i = 0; i <= CheckedCheckBox.length - 1; i++) {
                    $("#<%= ddlRegularityReq.ClientID%>").multiselect("widget").find(".ui-multiselect-checkboxes.ui-helper-reset").find("input[value=" + CheckedCheckBox[i] + "]").attr("checked", "checked");

                }
                $('#<%= ddlRegularityReq.ClientID%>').multiselect("update");
            }
        }

    </script>
</asp:Content>
