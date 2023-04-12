<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmPublishAndSubmit, App_Web_2mzu20n4" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <style type="text/css">
        #ctl00_CPHLAMBDA_tdRelatedSeqNo
        {
            text-align: left !important;
        }
        #ctl00_CPHLAMBDA_tblSub tr td:first-child
        {
            text-align: right;
        }
        #ctl00_CPHLAMBDA_tblSub tr td
        {
            padding-top: 10px !important;
        }
        #ctl00_CPHLAMBDA_tblSub tr td:last-child
        {
            text-align: left;
        }
        #ctl00_CPHLAMBDA_tdSubmissionDesc_1, #ctl00_CPHLAMBDA_tdIncludeRMS_1
        {
            padding-left: 50px;
            text-align: right;
        }
        #ctl00_CPHLAMBDA_tdSelectCMS
        {
            padding-left: 60px;
        }
        #ctl00_CPHLAMBDA_gvPublishedProject
        {
            background-color: Gray;
        }
    </style>
    <div style="width: 880px; text-align: center">
        <asp:UpdatePanel ID="upPublsihAndSubmit" runat="server">
            <ContentTemplate>
                <table style="width: 100%; text-align: center">
                    <tr id="trSelection" align="center" runat="server" style="white-space: nowrap">
                        <td style="text-align: left">
                            <fieldset id="fsProject" class="detailsbox" title="Select Project" style="width: 850px;
                                padding: 10px">
                                <legend class="LabelBold" style="color: Black">Select Project </legend>
                                <%--<asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>--%>
                                <table style="width: 100%">
                                    <tr>
                                        <%--<td align="center" style="display: none">
                                            <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="650px" TabIndex="0"></asp:TextBox>
                                            <asp:HiddenField ID="HProjectId" runat="server" />
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                                                TargetControlID="txtProject" ServicePath="AutoComplete.asmx" ServiceMethod="GetMyProjectCompletionList"
                                                OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1"
                                                CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1">
                                            </cc1:AutoCompleteExtender>
                                        </td>--%>
                                        <td style="width: 100%">
                                            <asp:DropDownList ID="ddlProject" CssClass="dropDownList" runat="server" Width="400"
                                                AutoPostBack="true">
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="HProjectRegion" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <%--  </ContentTemplate>--%>
                                <%-- <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click"></asp:AsyncPostBackTrigger>
                                <asp:AsyncPostBackTrigger ControlID="ddlProject" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>--%>
                            </fieldset>
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" style="width: 100%; text-align: center">
                    <!-- PROJECT DETAILS -->
                    <tr id="trProjectDetail" runat="server" style="display: none">
                        <td>
                            <fieldset id="fsProjectDetails" class="detailsbox" runat="server" title="Project Details"
                                style="width: 850px; padding: 10px">
                                <legend class="LabelBold" style="color: Black">&nbsp;<img id="ImgDetail" class="flip"
                                    alt="Image Not Found" src="images/expand.jpg" runat="server" onmouseover="this.style.cursor='pointer';"
                                    onclick="ShowDetail(this);" />&nbsp;&nbsp;Project Details </legend>
                                <div id="divProjectDetail" class="Detailpanel" style="max-height: 150px; display: none">
                                    <asp:Panel ID="panel1" runat="server">
                                        <asp:Button ID="btnSetProject" runat="server" Style="display: none" Text=" Project" CssClass="btn btnnew" />
                                        <table cellpadding="7" width="100%">
                                            <tr id="trProjectName" runat="server">
                                                <td align="left" style="width: 150px">
                                                    Project Name :
                                                </td>
                                                <td align="left" style="width: 500px; font-weight: bold; color: Black">
                                                    <asp:Label ID="lblProjectName" runat="server" Text="" />
                                                </td>
                                            </tr>
                                            <tr id="trProjectType" runat="server">
                                                <td align="left" style="width: 150px">
                                                    Project Type :
                                                </td>
                                                <td align="left" style="width: 500px; font-weight: bold; color: Black">
                                                    <asp:Label ID="lblProjectType" runat="server" Text="" />
                                                </td>
                                            </tr>
                                            <tr id="trClientName" runat="server">
                                                <td align="left" style="width: 150px">
                                                    Client Name :
                                                </td>
                                                <td align="left" style="width: 500px; font-weight: bold; color: Black">
                                                    <asp:Label ID="lblClientName" runat="server" Text="" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                    <!-- ATTRIBUTE GRID -->
                    <tr id="trAttribute" runat="server" style="display: none">
                        <td>
                            <fieldset id="fsAttributes" class="detailsbox" title="Attribute" style="width: 850px;
                                padding: 10px">
                                <legend class="LabelBold" style="color: Black">&nbsp;<img id="ImgexpColAttr" class="flip"
                                    alt="Image Not Found" src="images/collapse.jpg" runat="server" onmouseover="this.style.cursor='pointer';"
                                    onclick="ShowAttrGrid(this);" />&nbsp;&nbsp;Attribute Validation &nbsp;&nbsp;</legend>
                                <table cellpadding="0" style="width: 100%">
                                    <tr id="trAttrGrid" runat="server" style="display: none">
                                        <td>
                                            <asp:UpdatePanel ID="UpSubmissionDetail" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div id="divAttrGrid" runat="server" class="panel" style="width: 99%; max-height: 240px">
                                                        <table width="100%">
                                                            <tr align="center">
                                                                <td>
                                                                    <asp:Panel ID="pnlAttrGrid" runat="server" ScrollBars="Auto" Width="98%" Style="max-height: 200px">
                                                                        <asp:GridView ID="GvAttributes" runat="server" AutoGenerateColumns="False" SkinID="grdViewSmlAutoSize"
                                                                            Visible="true" Width="100%">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="">
                                                                                    <ItemStyle HorizontalAlign="Center" Width="25"></ItemStyle>
                                                                                    <ItemTemplate>
                                                                                        <asp:Image ID="ImgAttrStatus" AlternateText="Image Not Found" runat="server" ImageUrl=""
                                                                                            Height="23px" Width="23px" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField DataField="vWorkspaceId" HeaderText="Id">
                                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="iNodeId" HeaderText="NodeId">
                                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="iAttrId" HeaderText="AttrId">
                                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="vNodeName" HeaderText="Node Name">
                                                                                    <ItemStyle HorizontalAlign="Left" Width="300"></ItemStyle>
                                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="vNodeDisplayName" HeaderText="Node Display Name">
                                                                                    <ItemStyle HorizontalAlign="Left" Width="400"></ItemStyle>
                                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="vAttrName" HeaderText="Attribute Name">
                                                                                    <ItemStyle HorizontalAlign="Left" Width="200"></ItemStyle>
                                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                                                </asp:BoundField>
                                                                                <asp:TemplateField HeaderText="Attribute Value">
                                                                                    <ItemStyle HorizontalAlign="Left" Width="100"></ItemStyle>
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtAttrValue" runat="server" Width="100" Text='<%#eval("vAttrValue") %>'></asp:TextBox>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:BoundField DataField="cAttrForIndi" HeaderText="Attribute Indi">
                                                                                    <ItemStyle HorizontalAlign="Left" Width="150"></ItemStyle>
                                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="vAttrType" HeaderText="Attribute Type">
                                                                                    <ItemStyle HorizontalAlign="Left" Width="150"></ItemStyle>
                                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                                                </asp:BoundField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <br />
                                                                    <asp:Button ID="btnSaveAttributes" runat="server" Text="Save" CssClass="btn btnsave" Font-Bold="True" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnSaveAttributes" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr id="trAttrMsg" runat="server" style="display: none; font-weight: bold; color: black">
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <!-- SUBMISSION ENTRY -->
                    <tr id="trSubmissionEntry" runat="server" style="display: none">
                        <td align="left">
                            <fieldset id="fsSubmissionEntry" class="detailsbox" title="Submission Entry" style="width: 850px;
                                padding: 10px">
                                <legend class="LabelBold" style="color: Black">Submission Detail </legend>
                                <%--<asp:UpdatePanel ID="UpSubmissionDetail" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>--%>
                                <table id="tblSub" style="width: 920px" runat="server">
                                    <tr align="left">
                                        <td id="tdsubmissionType_1" runat="server" style="width: 200px">
                                            Submission Type :
                                        </td>
                                        <td id="tdsubmissionType_2" runat="server" style="width: 200px">
                                            <asp:DropDownList ID="ddlSubmissionType" CssClass="dropDownList" runat="server" Width="200">
                                            </asp:DropDownList>
                                        </td>
                                        <td id="tdSubmissionDesc_1" runat="server" style="width: 175px">
                                            Submission Description :
                                        </td>
                                        <td id="tdSubmissionDesc_2" runat="server" style="width: 200px">
                                            <asp:TextBox ID="txtSubmissionDesc" runat="server" Width="150px" MaxLength="250">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td id="tdsubmissionMode_1" runat="server" style="width: 200px">
                                            Submission Mode :
                                        </td>
                                        <td id="tdsubmissionMode_2" runat="server" style="width: 200px">
                                            <asp:DropDownList ID="ddlSubmissionMode" CssClass="dropDownList" runat="server" Width="200px">
                                            </asp:DropDownList>
                                        </td>
                                        <td id="tdIncludeRMS_1" runat="server" style="width: 175px">
                                            Include RMS
                                            <asp:Label ID="lblRMSCountryName" runat="server" Text="Country" />
                                            :
                                        </td>
                                        <td id="tdIncludeRMS_2" runat="server" style="width: 200px">
                                            <asp:RadioButtonList ID="rbRMSCountry" runat="server" RepeatDirection="Horizontal"
                                                CellPadding="6">
                                                <asp:ListItem Value="Y" Selected="True">Yes</asp:ListItem>
                                                <asp:ListItem Value="N">No</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td id="tdApplicationNumber_1" runat="server" style="width: 200px">
                                            Application Number :
                                        </td>
                                        <td id="tdApplicationNumber_2" runat="server" style="width: 200px">
                                            <asp:TextBox ID="txtApplicationNumber" runat="server" Width="150px" MaxLength="7">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td id="tdTrackingNumber_1" runat="server" style="width: 200px">
                                            Tracking Number :
                                        </td>
                                        <td id="tdTrackingNumber_2" runat="server" style="width: 200px">
                                            <asp:TextBox ID="txtTrackingNumber" runat="server" Width="150px" MaxLength="250">
                                            </asp:TextBox>
                                        </td>
                                        <td id="tdSelectCMS" runat="server" style="width: 190px" colspan="2" rowspan="3">
                                            <table cellpadding="2" width="400px">
                                                <tr>
                                                    <td valign="top" style="width: 300px">
                                                        Select CMS :
                                                    </td>
                                                    <td id="tdCMSGrid" style="width: 300px; display: none" runat="server">
                                                        <asp:Panel ID="pnlgvCMS" runat="server" Width="300px" ScrollBars="Auto">
                                                            <asp:GridView ID="gvCMS" runat="server" AutoGenerateColumns="False" SkinID="grdViewSmlSize"
                                                                Visible="true" Width="300px" Style="table-layout: auto">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5px" />
                                                                        <HeaderTemplate>
                                                                            <input id="chkSelectAll" onclick="SelectAll(this,'gvCMS')" type="checkbox" style="padding-top: 10px" />
                                                                            <%--<asp:Label ID="Label1" runat="server" Text="All"></asp:Label>--%>
                                                                        </HeaderTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" Width="10px" />
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="ChkMove" runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="iWorkspaceCMSId" HeaderText="vWorkspaceCMSId">
                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vWorkspaceId" HeaderText="vWorkspaceId">
                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vCountryCode" HeaderText="vCountryCode">
                                                                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="vCountryName" HeaderText="CMS Country">
                                                                        <ItemStyle HorizontalAlign="Left" Width="150px"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Tracking Number">
                                                                        <ItemStyle Width="125px"></ItemStyle>
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtCMSTrackingNumber" runat="server" Width="100" Enabled="false"
                                                                                MaxLength="250" Text='<%#eval("vCMSTrackingNo") %>'></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </td>
                                                    <td id="tdCMSNotFound" style="width: 300px; display: none; color: CaptionText" runat="server">
                                                        <strong>No CMS Countries Found for this project.</strong>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td id="tdCurrentSeqNumber_1" runat="server" style="width: 200px" nowrap="nowrap">
                                            Current Sequence Number :
                                        </td>
                                        <td id="tdCurrentSeqNumber_2" runat="server" style="width: 200px">
                                            <asp:TextBox ID="txtCurrentSeqNumber" runat="server" Text="0000" Width="50px" ReadOnly="true">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td id="tdRelatedSeqNumber_1" runat="server" style="width: 200px">
                                            Related Sequence Number :
                                        </td>
                                        <td id="tdRelatedSeqNumber_2" runat="server" style="width: 200px">
                                            <asp:Panel ID="pnlRelatedSeqNo" runat="server" ScrollBars="Auto" Width="200" Style="border-style: solid;
                                                border-width: thin">
                                                <table>
                                                    <tr align="left">
                                                        <td id="tdRelatedSeqNo" runat="server" style="display: none">
                                                            <asp:CheckBoxList ID="chkRelatedSeqNo" runat="server" RepeatDirection="Horizontal"
                                                                RepeatLayout="flow" CssClass="checkboxlist">
                                                            </asp:CheckBoxList>
                                                        </td>
                                                        <td id="tdRelatedSeqNoMsg" runat="server" style="display: block; color: Black;">
                                                            <asp:Label ID="lblRelatedSeqMsg" runat="server" Text="No Related Sequence Found."></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td id="tdDOS_1" runat="server" style="width: 200px">
                                            Date Of Submission :
                                        </td>
                                        <td id="tdDOS_2" runat="server" style="width: 200px">
                                            <asp:TextBox ID="txtdos" ReadOnly="true" runat="server" Width="150px">
                                            </asp:TextBox>
                                            <cc1:CalendarExtender ID="calDOS" TargetControlID="txtdos" Animated="true" BehaviorID="calDOS"
                                                runat="server" Format="dd-MMM-yyyy">
                                            </cc1:CalendarExtender>
                                            <asp:Label ID="lblDateFormate" runat="server" Text="DD-MMM-YYYY" />
                                        </td>
                                        <td id="tdAddTrackingTable_1" runat="server" style="width: 200px; padding-left: 160px"
                                            colspan="2">
                                            <asp:CheckBox ID="chkAddTrackingTable" runat="server" Text=" Add Tracking Table in XML"
                                                Checked="true" />
                                        </td>
                                    </tr>
                                    <tr id="trTransamissionMedia" align="left" runat="server">
                                        <td id="tdTransamissionMedia_1" runat="server" style="width: 175px">
                                            Transmission Media :
                                        </td>
                                        <td id="tdTransamissionMedia_2" runat="server" style="width: 200px">
                                            <asp:DropDownList ID="ddlTransamissionMedia" CssClass="dropDownList" runat="server"
                                                Width="75">
                                                <asp:ListItem Value="ESG">ESG</asp:ListItem>
                                                <asp:ListItem Value="Mail">Mail</asp:ListItem>
                                                <asp:ListItem Value="Post">Post</asp:ListItem>
                                                <asp:ListItem Value="Email">Email</asp:ListItem>
                                                <asp:ListItem Value="Courier">Courier</asp:ListItem>
                                                <asp:ListItem Value="Web">Web</asp:ListItem>
                                                <asp:ListItem Value="Other">Other</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <%--</ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                                <asp:PostBackTrigger ControlID="gvCMS" />
                                <asp:AsyncPostBackTrigger ControlID="ddlProject" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>--%>
                            </fieldset>
                        </td>
                    </tr>
                    <!-- SAVE CLEAR EXIT BUTTONS -->
                    <tr id="trButtons" runat="server" style="display: none">
                        <td style="width: 100%; text-align: center">
                            <br />
                            <%-- <asp:UpdatePanel ID="UpPublishButton" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                        <ContentTemplate>--%>
                            <table id="tdContainer" runat="server" style="width: 100%; text-align: center">
                                <tbody>
                                    <tr id="Tr1" runat="server" style="display: block">
                                        <td>
                                            <%--<asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                <ContentTemplate>--%>
                                            <asp:Button ID="btnSubmit" runat="server" Text="Publish & Submit" CssClass="btn btnsave"
                                                Font-Bold="True"  OnClientClick="return Validation();"></asp:Button>
                                            <%-- </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click"></asp:AsyncPostBackTrigger>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlProject" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>--%>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <%-- </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlProject" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>--%>
                        </td>
                    </tr>
                    <!-- SUBMITED PROJECT DETAILS -->
                    <tr id="trProjectSubmissionDetail" runat="server" style="display: none" align="center">
                        <td>
                            <fieldset id="fsProjectSubmissionDetail" class="detailsbox" title="Project Submission Detail"
                                style="width: 850px; padding: 15px">
                                <legend class="LabelBold" style="color: Black">Project Submission Detail </legend>
                                <%-- <asp:UpdatePanel ID="UpProjectSubmissionDetail" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>--%>
                                <table cellpadding="7" style="width: 100%" border="0">
                                    <tr align="left">
                                        <td id="tdProjectName_Detail_1" runat="server" style="width: 200px">
                                            Project Name :
                                        </td>
                                        <td id="tdProjectName_Detail_2" runat="server" style="width: 200px" colspan="3">
                                            <asp:Label ID="lblProjectNameInDetail" runat="server" Text="-" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td id="tdTrackingNumber_Detail_1" runat="server" style="width: 200px">
                                            Tracking Number :
                                        </td>
                                        <td id="tdTrackingNumber_Detail_2" runat="server" style="width: 200px">
                                            <asp:Label ID="lblTrackingNumberInDetail" runat="server" Text="-" />
                                        </td>
                                        <td id="tdApplicationNumber_Detail_1" runat="server" style="width: 200px">
                                            Application Number :
                                        </td>
                                        <td id="tdApplicationNumber_Detail_2" runat="server" style="width: 200px">
                                            <asp:Label ID="lblApplicationNumberInDetail" runat="server" Text="-" />
                                        </td>
                                    </tr>
                                    <tr align="left">
                                        <td id="tdSubmissionSountry_Detail_1" runat="server" style="width: 200px">
                                            Submission Country :
                                        </td>
                                        <td id="tdSubmissionSountry_Detail_2" runat="server" style="width: 200px">
                                            <asp:Label ID="lblSubmissionCountryInDetail" runat="server" Text="-" />
                                        </td>
                                        <td id="tdAgency_Detail_1" runat="server" style="width: 200px">
                                            Agency :
                                        </td>
                                        <td id="tdAgency_Detail_2" runat="server" style="width: 200px">
                                            <asp:Label ID="lblAgencyInDetail" runat="server" Text="-" />
                                        </td>
                                    </tr>
                                </table>
                                <%-- </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlProject" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>--%>
                            </fieldset>
                        </td>
                    </tr>
                    <!-- PUBLISHED PROJECT GRID -->
                    <tr id="trpublishedProject" runat="server" style="display: none">
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <fieldset id="fsPublishedProject" class="detailsbox" style="width: 850px; padding: 10px">
                                            <legend class="LabelBold" style="color: Black">Published Project Details</legend>
                                            <br />
                                            <asp:UpdatePanel ID="UPPublishedPorject" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Panel ID="pnlPublishedProject" runat="server" ScrollBars="Auto" Width="100%"
                                                        Style="max-height: 500px">
                                                        <asp:GridView ID="gvPublishedProject" runat="server" AutoGenerateColumns="False"
                                                            SkinID="grdViewSmlAutoSize" Visible="true" Width="100%">
                                                            <Columns>
                                                                <asp:BoundField DataFormatString="number" HeaderText="#">
                                                                    <ItemStyle HorizontalAlign="Center" Width="10"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SubmissionInfoEU14DtlId" HeaderText="Id">
                                                                    <ItemStyle HorizontalAlign="Left" Width="12"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SubmissionInfoUSDtlId" HeaderText="Id">
                                                                    <ItemStyle HorizontalAlign="Left" Width="12"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SubmissionInfoCADtlId" HeaderText="Id">
                                                                    <ItemStyle HorizontalAlign="Left" Width="12"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="WorkspaceId" HeaderText="WorkspaceId">
                                                                    <ItemStyle HorizontalAlign="Left" Width="15"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CurrentSeqNumber" HeaderText="Sequence">
                                                                    <ItemStyle HorizontalAlign="Left" Width="12"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="RelatedSeqNo" HeaderText="Related Sequences">
                                                                    <ItemStyle HorizontalAlign="Left" Width="20"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="SubmissionType" HeaderText="Submission Type">
                                                                    <ItemStyle HorizontalAlign="Left" Width="20"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Details">
                                                                    <ItemStyle HorizontalAlign="Left" Width="25"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ImgDetails" runat="server" ImageUrl="~/images/information.png">
                                                                        </asp:ImageButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="View XML">
                                                                    <ItemStyle HorizontalAlign="Left" Width="25"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ImgViewXML" runat="server" ImageUrl="~/Images/view.gif"></asp:ImageButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Broken Links">
                                                                    <ItemStyle HorizontalAlign="Left" Width="25"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ImgBrokenLinks" runat="server" ImageUrl="~/images/broken links.png">
                                                                        </asp:ImageButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Validate">
                                                                    <ItemStyle HorizontalAlign="Left" Width="25"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ImgValidate" runat="server" ImageUrl="~/images/Correct.png">
                                                                        </asp:ImageButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Recompile">
                                                                    <ItemStyle HorizontalAlign="Left" Width="25"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ImgRecompile" runat="server" ImageUrl="~/images/Recompile.png"
                                                                            Width="15" Height="15"></asp:ImageButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Confirm">
                                                                    <ItemStyle HorizontalAlign="Left" Width="25"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkConfirm" Text="Confirm" runat="server" OnClientClick="return Confirm(this);">
                                                                        </asp:LinkButton>
                                                                        <asp:Label ID="lblConfirmed" runat="server" Text="Confirmed" Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Publish Path">
                                                                    <ItemStyle HorizontalAlign="Left" Width="25"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ImgPublishPath" runat="server" ImageUrl="~/images/Publish Path.png"
                                                                            Width="15" Height="15"></asp:ImageButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Size (MB)">
                                                                    <ItemStyle HorizontalAlign="Left" Width="75"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkSize" Text="Get Size" runat="server">
                                                                        </asp:LinkButton>
                                                                        <asp:Label ID="lblSize" runat="server" Text="Size" Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Delete">
                                                                    <ItemStyle HorizontalAlign="Left" Width="25"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ImgDelete" runat="server" ImageUrl="~/images/cancel.gif" OnClientClick="return delete(this);">
                                                                        </asp:ImageButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="Confirm" HeaderText="Confirm Value">
                                                                    <ItemStyle HorizontalAlign="Left" Width="20"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="StatusIndi" HeaderText="Status Indicator">
                                                                    <ItemStyle HorizontalAlign="Left" Width="20"></ItemStyle>
                                                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                                </asp:BoundField>
                                                            </Columns>
                                                        </asp:GridView>
                                                        <input type="hidden" id="HFRecompile" runat="server" style="display: none" />
                                                        <cc1:ModalPopupExtender ID="mpeRecompile" runat="server" PopupControlID="divRecompile"
                                                            BackgroundCssClass="modalBackground" TargetControlID="HFRecompile" CancelControlID="ImgPopUpClose">
                                                        </cc1:ModalPopupExtender>
                                                        <div id="divRecompile" runat="server" class="centerModalPopup" style="display: block;
                                                            left: 521px; width: 370px; position: absolute; top: 550px; height: 160px">
                                                            <div style="width: 100%">
                                                                <h1 class="header">
                                                                    <label id="lblAuditTrail" class="labelbold">
                                                                        RECOMPILE
                                                                    </label>
                                                                    <img id="ImgPopUpClose" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
                                                                </h1>
                                                            </div>
                                                            <div id="divContent" runat="server" style="width: 100%">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td align="Center" style="width: 150px">
                                                                            <br />
                                                                            New Date of Submission :
                                                                            <asp:TextBox ID="txtNewDos" ReadOnly="true" runat="server" Width="150px">
                                                                            </asp:TextBox>
                                                                            <cc1:CalendarExtender ID="CalNewDos" TargetControlID="txtNewDos" Animated="true"
                                                                                BehaviorID="calNewDOS" runat="server" Format="dd-MMM-yyyy">
                                                                            </cc1:CalendarExtender>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblNewDateFormate" runat="server" Text="DD-MMM-YYYY" Style="padding-left: 197px" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center">
                                                                            <br />
                                                                            <asp:Button ID="btnRecompile" runat="server" Text="Recompile" CssClass="btn btnnew" />
                                                                            <asp:HiddenField ID="HSubmissionPath" runat="server" />
                                                                            <asp:HiddenField ID="HSubmissionId" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                        <%--Display Details of Project Submission--%>
                                                        <input type="hidden" id="HDetails" runat="server" style="display: none" />
                                                        <cc1:ModalPopupExtender ID="mpeDetails" runat="server" PopupControlID="divDetails"
                                                            BackgroundCssClass="modalBackground" TargetControlID="HDetails" CancelControlID="imgCloseDetails">
                                                        </cc1:ModalPopupExtender>
                                                        <div id="divDetails" runat="server" class="centerModalPopup" style="display: block;
                                                            left: 521px; max-width: 550px; position: absolute; top: 550px; height: 450px">
                                                            <div style="width: 100%">
                                                                <h1 class="header">
                                                                    <label id="lblDetails" class="labelbold">
                                                                        SUBMISSION DETAILS
                                                                    </label>
                                                                    <img id="imgCloseDetails" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
                                                                </h1>
                                                            </div>
                                                            <asp:Panel ID="pnlDetails" runat="server" ScrollBars="Auto" Width="100%" Height="405">
                                                                <%--<div style="width: 100%; overflow: scroll; height: 400px">--%>
                                                                <table border="1" cellpadding="6" style="width: 100%; border-style: solid; border-width: thin;
                                                                    border-color: Black; color: Black">
                                                                    <tr align="left" id="trProjectName_dtl" runat="server" style="display: none">
                                                                        <td style="background-color: #E5E5E5; width: 180px; padding-left: 5px; font-weight: bold">
                                                                            Project Name
                                                                        </td>
                                                                        <td style="padding-left: 10px">
                                                                            <asp:Label ID="lblProjectName_dtl" runat="server" Text="" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left" id="trApplicationNo_dtl" runat="server" style="display: none">
                                                                        <td style="background-color: #E5E5E5; width: 180px; padding-left: 5px; font-weight: bold">
                                                                            Application No.
                                                                        </td>
                                                                        <td style="padding-left: 10px">
                                                                            <asp:Label ID="lblApplicationNo_dtl" runat="server" Text="" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left" id="trTrackingNo_Dtl" runat="server" style="display: none">
                                                                        <td style="background-color: #E5E5E5; width: 180px; padding-left: 5px; font-weight: bold">
                                                                            Tracking No.
                                                                        </td>
                                                                        <td style="padding-left: 10px">
                                                                            <asp:Label ID="lblTrackingNo_dtl" runat="server" Text="" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left" id="trCompanyName_dtl" runat="server" style="display: none">
                                                                        <td style="background-color: #E5E5E5; width: 180px; padding-left: 5px; font-weight: bold">
                                                                            Company Name
                                                                        </td>
                                                                        <td style="padding-left: 10px">
                                                                            <asp:Label ID="lblCompanyName_dtl" runat="server" Text="" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left" id="trProductName_dtl" runat="server" style="display: none">
                                                                        <td style="background-color: #E5E5E5; width: 180px; padding-left: 5px; font-weight: bold">
                                                                            Product Name
                                                                        </td>
                                                                        <td style="padding-left: 10px">
                                                                            <asp:Label ID="lblProductName_dtl" runat="server" Text="" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left" id="trProductType_dtl" runat="server" style="display: none">
                                                                        <td style="background-color: #E5E5E5; width: 180px; padding-left: 5px; font-weight: bold">
                                                                            Product Type
                                                                        </td>
                                                                        <td style="padding-left: 10px">
                                                                            <asp:Label ID="lblProductType_dtl" runat="server" Text="" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left" id="trSubCountry_dtl" runat="server" style="display: none">
                                                                        <td style="background-color: #E5E5E5; width: 180px; padding-left: 5px; font-weight: bold">
                                                                            Submission Country
                                                                        </td>
                                                                        <td style="padding-left: 10px">
                                                                            <asp:Label ID="lblSubCountry_dtl" runat="server" Text="" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left" id="trAgency_dtl" runat="server" style="display: none">
                                                                        <td style="background-color: #E5E5E5; width: 180px; padding-left: 5px; font-weight: bold">
                                                                            Agency
                                                                        </td>
                                                                        <td style="padding-left: 10px">
                                                                            <asp:Label ID="lblAgency_dtl" runat="server" Text="" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left" id="trApplicationType_dtl" runat="server" style="display: none">
                                                                        <td style="background-color: #E5E5E5; width: 180px; padding-left: 5px; font-weight: bold">
                                                                            Application Type
                                                                        </td>
                                                                        <td style="padding-left: 10px">
                                                                            <asp:Label ID="lblApplicationType_dtl" runat="server" Text="" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left" id="trApplicant_dtl" runat="server" style="display: none">
                                                                        <td style="background-color: #E5E5E5; width: 180px; padding-left: 5px; font-weight: bold">
                                                                            Applicant
                                                                        </td>
                                                                        <td style="padding-left: 10px">
                                                                            <asp:Label ID="lblApplicant_dtl" runat="server" Text="" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left" id="trProcedureType_dtl" runat="server" style="display: none">
                                                                        <td style="background-color: #E5E5E5; width: 180px; padding-left: 5px; font-weight: bold">
                                                                            Procedure Type
                                                                        </td>
                                                                        <td style="padding-left: 10px">
                                                                            <asp:Label ID="lblProcedureType_dtl" runat="server" Text="" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left" id="trInventedName_dtl" runat="server" style="display: none">
                                                                        <td style="background-color: #E5E5E5; width: 180px; padding-left: 5px; font-weight: bold">
                                                                            Invented Name
                                                                        </td>
                                                                        <td style="padding-left: 10px">
                                                                            <asp:Label ID="lblInventedName_dtl" runat="server" Text="" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left" id="trInn_dtl" runat="server" style="display: none">
                                                                        <td style="background-color: #E5E5E5; width: 180px; padding-left: 5px; font-weight: bold">
                                                                            Inn
                                                                        </td>
                                                                        <td style="padding-left: 10px">
                                                                            <asp:Label ID="lblInn_dtl" runat="server" Text="" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left" id="trSubType_dtl" runat="server" style="display: none">
                                                                        <td style="background-color: #E5E5E5; width: 180px; padding-left: 5px; font-weight: bold">
                                                                            Submission Type
                                                                        </td>
                                                                        <td style="padding-left: 10px">
                                                                            <asp:Label ID="lblSubType_dtl" runat="server" Text="" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left" id="trSubSeq_dtl" runat="server" style="display: none">
                                                                        <td style="background-color: #E5E5E5; width: 180px; padding-left: 5px; font-weight: bold">
                                                                            Submission Sequence
                                                                        </td>
                                                                        <td style="padding-left: 10px">
                                                                            <asp:Label ID="lblSubSeq_dtl" runat="server" Text="" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left" id="trRelatedSeq_dtl" runat="server" style="display: none">
                                                                        <td style="background-color: #E5E5E5; width: 180px; padding-left: 5px; font-weight: bold">
                                                                            Related Sequences
                                                                        </td>
                                                                        <td style="padding-left: 10px">
                                                                            <asp:Label ID="lblRelatedSeq_dtl" runat="server" Text="" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left" id="trDos_dtl" runat="server" style="display: none">
                                                                        <td style="background-color: #E5E5E5; width: 180px; padding-left: 5px; font-weight: bold">
                                                                            Date of Submission
                                                                        </td>
                                                                        <td style="padding-left: 10px">
                                                                            <asp:Label ID="lblDos_dtl" runat="server" Text="" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left" id="trSubDesc_dtl" runat="server" style="display: none">
                                                                        <td style="background-color: #E5E5E5; width: 180px; padding-left: 5px; font-weight: bold">
                                                                            Submission Description
                                                                        </td>
                                                                        <td style="padding-left: 10px">
                                                                            <asp:Label ID="lblSubDesc_dtl" runat="server" Text="" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left" id="trSubmittedOn_dtl" runat="server" style="display: none">
                                                                        <td style="background-color: #E5E5E5; width: 180px; padding-left: 5px; font-weight: bold">
                                                                            Submitted On
                                                                        </td>
                                                                        <td style="padding-left: 10px">
                                                                            <asp:Label ID="lblSubmittedOn_dtl" runat="server" Text="" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left" id="trSubStatus_dtl" runat="server" style="display: none">
                                                                        <td style="background-color: #E5E5E5; width: 180px; padding-left: 5px; font-weight: bold">
                                                                            Submission Status
                                                                        </td>
                                                                        <td style="padding-left: 10px">
                                                                            <asp:Label ID="lblSubStatus_dtl" runat="server" Text="" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="left" id="trSubMode_dtl" runat="server" style="display: none">
                                                                        <td style="background-color: #E5E5E5; width: 180px; padding-left: 5px; font-weight: bold">
                                                                            Submission Mode
                                                                        </td>
                                                                        <td style="padding-left: 10px">
                                                                            <asp:Label ID="lblSubMode_dtl" runat="server" Text="" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <%--</div>--%></asp:Panel>
                                                        </div>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnRecompile" EventName="Click"></asp:AsyncPostBackTrigger>
                                                    <%--<asp:PostBackTrigger ControlID="gvPublishedProject"></asp:PostBackTrigger>--%>
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <br />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript" language="javascript" src="Script/popcalendar.js"></script>

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" src="Script/Validation.js"></script>

    <script type="text/javascript" src="Script/jquery-1.4.3.min.js"></script>

    <script type="text/javascript" src="Script/jquery.min.js"></script>

    <script type="text/javascript"> 

        ////////////////////////////////////////////////////
        
        $(document).ready(function(){
        $(".flip").mouseover(function(){
        $(".flip").css("background-color","#99CCFF");
        });
        $(".flip").mouseout(function(){
        $(".flip").css("background-color","White");
        });
        });
        ///////////////////////////////////////////////////
        
        function ShowAttrGrid(ele)
        {
            if(ele.src.toUpperCase().search('COLLAPSE')!=-1)
            {
                $(".panel").slideToggle(500);
                ele.src="images/expand.jpg";
            }
            else            
            {
                $(".panel").slideToggle(500);
                ele.src="images/collapse.jpg";
            }
        }
        
        function ShowDetail(ele)
        {
            
            if(ele.src.toUpperCase().search('EXPAND')!=-1)
            {
                $(".Detailpanel").slideToggle(500);
                ele.src="images/collapse.jpg";
            }
            else            
            {
                $(".Detailpanel").slideToggle(500);
                ele.src="images/expand.jpg";
            }
        }
        
        ///////////////////////////////////////////////////////
        
        function validateCountry()
        {
            var rdorms;
            var chkcms;
            var chkTracking;
            rdorms = document.getElementById('ctl00_CPHLAMBDA_rbRMSCountry_1').checked;
            chkcms = $('#ctl00_CPHLAMBDA_gvCMS input[type=checkbox]:checked').length;
            chkTracking = document.getElementById('ctl00_CPHLAMBDA_chkAddTrackingTable').checked;
            //alert(chkcms);
            //alert(chkrms);
            
            if(chkTracking == true)
            {
                if(rdorms == true && chkcms == 0)
                {
                    msgalert("Select RMS or atleast one CMS Country to Generate Tracking Table !");
                    return false;
                }
                else 
                {
                    return true;
                }
            }
        }
        
        ///////////////////////////////////////////////////////
                
        function Validation()
        {
            var region;
            region = document.getElementById('<%=HProjectRegion.ClientID%>').value;
            //alert(region);
            
            if(region.toUpperCase() == 'EU')
            {
                //alert(region);
                if (document.getElementById('<%=txtTrackingNumber.ClientID%>').value == '') {
                    msgalert('Please specify Tracking Number !');
                    document.getElementById('<%=txtTrackingNumber.ClientID%>').focus();
                    document.getElementById('<%=txtTrackingNumber.ClientID%>').style.backgroundColor="#FFE6F7"; 
                    return false;
                }
                else if (document.getElementById('<%=txtdos.ClientID%>').value == '') {
                    msgalert('Please specify Date Of Submission !');
                    document.getElementById('<%=txtdos.ClientID%>').focus();
                    document.getElementById('<%=txtdos.ClientID%>').style.backgroundColor="#FFE6F7"; 
                    return false;
                }
                else if (document.getElementById('<%=txtSubmissionDesc.ClientID%>').value == '') {
                    msgalert('Please specify Submission Description !');
                    document.getElementById('<%=txtSubmissionDesc.ClientID%>').focus();
                    document.getElementById('<%=txtSubmissionDesc.ClientID%>').style.backgroundColor="#FFE6F7"; 
                    return false;
                }
//                else if (document.getElementById('<%=ddlSubmissionMode.ClientID%>').selectedIndex <= 0) {
//                    alert('Please select Submission Mode');
//                    document.getElementById('<%=ddlSubmissionMode.ClientID%>').focus();
//                    document.getElementById('<%=ddlSubmissionMode.ClientID%>').style.backgroundColor="#FFE6F7"; 
//                    return false;
//                }
                
                var trackingnum = document.getElementById('<%=txtTrackingNumber.ClientID%>').value;
                //alert(trackingnum);
                if(!trackingnum.match(/^([a-zA-Z0-9\/\u002D\u002C])*$/))
				{
                    msgalert("Only digits, Alphabets, '-' , '/' and ',' are allowed !");
					document.getElementById('<%=txtTrackingNumber.ClientID%>').focus();
                    document.getElementById('<%=txtTrackingNumber.ClientID%>').style.backgroundColor="#FFE6F7"; 
				   	return false;
			    }
			    
			    
			    if($('#ctl00_CPHLAMBDA_gvCMS input:text[value=""][disabled=false]').length > 0)
			    {
			        msgalert("Tracking Number of CMS Country should not be blank !");
			        return false; 
			    }
                
                
                var abc;
                abc = validateCountry();
                return abc;
                                
            }
            else if(region.toUpperCase() == 'US')
            {
                
                if (document.getElementById('<%=txtApplicationNumber.ClientID%>').value == '') {
                    msgalert('Please specify Application Number !');
                    document.getElementById('<%=txtApplicationNumber.ClientID%>').focus();
                    document.getElementById('<%=txtApplicationNumber.ClientID%>').style.backgroundColor="#FFE6F7"; 
                    return false;
                }
                else if (document.getElementById('<%=txtdos.ClientID%>').value == '') {
                    msgalert('Please Specify Date Of Submission !');
                    document.getElementById('<%=txtdos.ClientID%>').focus();
                    document.getElementById('<%=txtdos.ClientID%>').style.backgroundColor="#FFE6F7"; 
                    return false;
                }
                
                var applicationNum = document.getElementById('<%=txtApplicationNumber.ClientID%>').value;
                //alert(applicationNum.length);
                if(applicationNum.length != 6 )
		    		{
                    msgalert("Please enter six digit Application Number for US submission !");
			    		document.getElementById('<%=txtApplicationNumber.ClientID%>').focus();
                        document.getElementById('<%=txtApplicationNumber.ClientID%>').style.backgroundColor="#FFE6F7"; 
					   	return false;
		    		}
		        if(!applicationNum.match(/^[0-9]*$/))
		    	{
		            msgalert("Only digits are allowed in Application Number for US Submission !");
		    		document.getElementById('<%=txtApplicationNumber.ClientID%>').focus();
                    document.getElementById('<%=txtApplicationNumber.ClientID%>').style.backgroundColor="#FFE6F7"; 
				   	return false;
		    	}
            }
            else if(region.toUpperCase() == 'CA')
            {
                
                if (document.getElementById('<%=txtApplicationNumber.ClientID%>').value == '') {
                    msgalert('Please specify Application Number !');
                    document.getElementById('<%=txtApplicationNumber.ClientID%>').focus();
                    document.getElementById('<%=txtApplicationNumber.ClientID%>').style.backgroundColor="#FFE6F7"; 
                    return false;
                }
                else if (document.getElementById('<%=txtdos.ClientID%>').value == '') {
                    msgalert('Please Specify Date Of Submission !');
                    document.getElementById('<%=txtdos.ClientID%>').focus();
                    document.getElementById('<%=txtdos.ClientID%>').style.backgroundColor="#FFE6F7"; 
                    return false;
                }
                
                var applicationNum = document.getElementById('<%=txtApplicationNumber.ClientID%>').value;
                if(applicationNum.charAt(0) != 'e' && applicationNum.charAt(0) != 's')
		        {
                    msgalert("Application number must starts with character \'e\'or \'s\' ");
		    		document.getElementById('<%=txtApplicationNumber.ClientID%>').focus();
                    document.getElementById('<%=txtApplicationNumber.ClientID%>').style.backgroundColor="#FFE6F7"; 
				   	return false;
		    	}
		    	
		    	var appno = applicationNum.substring(1); //match last 6 characters
		    	if(!appno.match(/^[0-9]*$/))
		    	{	
		    	    msgalert("Last Six characters must be digits for CA submission !");
		    		document.getElementById('<%=txtApplicationNumber.ClientID%>').focus();
                    document.getElementById('<%=txtApplicationNumber.ClientID%>').style.backgroundColor="#FFE6F7"; 
				   	return false;
			    }
            }
            
            return true;
        }
        
    </script>

    <style type="text/css">
        div.panel, p.flip
        {
            margin: 0px;
            padding: 5px;
            text-align: center;
        }
        div.panel
        {
            display: block;
        }
    </style>

    <script type="text/javascript" language="javascript">
//    function ClientPopulated(sender, e)
//        {
//            ProjectClientShowing('AutoCompleteExtender1', $get('txtProject'));
//        }

//        function OnSelected(sender, e)
//        {
//            ProjectOnItemSelected(e.get_value(), $get('txtProject'),
//            $get('HProjectId'), document.getElementById('btnSetProject'));
//        }
//        
        //FOR ALL CHECKBOX
        function SelectAll(CheckBoxControl, Grid)
        {
            var chk, txt;
            var chkid,txtid;
            if (CheckBoxControl.checked == true)
            {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++)
                {
                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf(Grid) > -1))
                    {
                        if (document.forms[0].elements[i].disabled == false)
                        {
                            document.forms[0].elements[i].checked = true;
                            
                            chk = document.getElementById(document.forms[0].elements[i].id)
                            chkid = chk.id;
                            txt = document.getElementById(chkid.replace('ChkMove','txtCMSTrackingNumber'));
                            txt.disabled = false;
                        }
                    }
                }
            }
            else
            {
                var i;
                for (i = 0; i < document.forms[0].elements.length; i++)
                {
                    if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf(Grid) > -1))
                    {
                        document.forms[0].elements[i].checked = false;
                        
                        chk = document.getElementById(document.forms[0].elements[i].id)
                        chkid = chk.id;
                        txt = document.getElementById(chkid.replace('ChkMove','txtCMSTrackingNumber'));
                        txt.disabled = true;
                    }
                }
            }
        }
        
        //FOR SINGLE CHECKBOX
        function ChkEnableTextbox(chk, TrackingNoTextbox)
        {
            var Checkbox = document.getElementById(chk);
            var Textbox = document.getElementById(TrackingNoTextbox);
            
            Textbox.disabled = true;
            if (Checkbox.checked == true) 
            {
                Textbox.disabled = false;
            }
        }
        
        function Confirm(e)
        {
            msgConfirmDeleteAlert(null, "Are you sure you want to confirm this sequence ?", function (isConfirmed) {
                if (isConfirmed) {
                    __doPostBack(e.name, '');
                    return true;
                } else {

                    return false;
                }
            });
            return false;
        }
        
        function Delete(e)
        {
           msgConfirmDeleteAlert(null, "Are you sure you want to delete this sequence ?", function (isConfirmed) {
                if (isConfirmed) {
                    __doPostBack(e.name, '');
                    return true;
                } else {

                    return false;
                }
            });
            return false;
        }

         
    </script>

</asp:Content>
