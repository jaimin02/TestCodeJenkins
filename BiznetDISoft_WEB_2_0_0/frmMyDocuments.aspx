<%@ Page Language="VB" MasterPageFile="~/ECTDMasterPage.master" AutoEventWireup="false"
    CodeFile="frmMyDocuments.aspx.vb" Inherits="frmMyDocuments" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <style type="text/css">
        #ctl00_CPHLAMBDA_pnlReleasedetail tr:nth-child(even) textarea, #ctl00_CPHLAMBDA_gvParentAuditTrail tr:nth-child(even) textarea, #ctl00_CPHLAMBDA_gvAuditTrail tr:nth-child(even) textarea
        {
            background: white;
            border: 0;
        }
        #ctl00_CPHLAMBDA_pnlReleasedetail tr:nth-child(odd) textarea, #ctl00_CPHLAMBDA_gvParentAuditTrail tr:nth-child(odd) textarea, #ctl00_CPHLAMBDA_gvAuditTrail tr:nth-child(odd) textarea
        {
            background: #CEE3ED;
            border: 0;
        }
        #ctl00_CPHLAMBDA_lblDocLastId
        {
            text-shadow: 0 1px 0 #ccc, 0 2px 0 #c9c9c9, 0 3px 0 #bbb, 0 4px 0 #b9b9b9, 0 5px 0 #aaa, 0 6px 1px rgba(0,0,0,.1), 0 0 5px rgba(0,0,0,.1), 0 1px 3px rgba(0,0,0,.3), 0 3px 5px rgba(0,0,0,.2), 0 5px 10px rgba(0,0,0,.25), 0 10px 10px rgba(0,0,0,.2), 0 20px 20px rgba(0,0,0,.15);
            font-size: 14px !important;
            color: #1560A1 !important;
            font-weight: bold;
            letter-spacing: 1pt;
        }
    </style>
    <asp:HiddenField ID="hdnStartDate" runat="server" />
    <asp:HiddenField ID="hdnEndDate" runat="server" />
    <div style="width: 100%">
        <asp:UpdatePanel ID="upMain" runat="server">
            <ContentTemplate>
                <table style="width: 100%">
                    <tr style="border-top-style: solid; border-top-width: thin">
                        <td id="tdContent" style="top: 5px; vertical-align: top; overflow: auto; display: block;"
                            align="center">
                            <fieldset id="fldReleaseDocTrail" class="detailsbox" runat="server" style="padding: 10px;
                                width: 90%;">
                                <legend class="LabelBold" style="color: Black">&nbsp;&nbsp;Released Document Trail&nbsp;&nbsp;
                                </legend>
                                <table style="width: 100%" cellpadding="2">
                                    <tr>
                                        <td style="width: 35%; text-align: right" class="labelbold">
                                            Select Department* :
                                        </td>
                                        <td style="text-align: left; padding-left: 10px">
                                            <asp:DropDownList ID="ddlProject" runat="server" CssClass="dropDownList" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 35%; text-align: right" class="labelbold">
                                            Select SOP* :
                                        </td>
                                        <td style="text-align: left; padding-left: 10px">
                                            <asp:DropDownList ID="ddlSOPNo" runat="server" CssClass="dropDownList" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 35%; text-align: right" class="labelbold">
                                            Select Form* :
                                        </td>
                                        <td style="text-align: left; padding-left: 10px">
                                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="dropDownList" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 35%; text-align: right" class="labelbold">
                                            Document ID :
                                        </td>
                                        <td style="text-align: left; padding-left: 10px">
                                            <asp:DropDownList ID="ddlProjectPrefix" runat="server" CssClass="dropDownList" Width="95px">
                                            </asp:DropDownList>
                                            -
                                            <asp:DropDownList ID="ddlCategoryPrefix" runat="server" CssClass="dropDownList" Width="95px">
                                            </asp:DropDownList>
                                            <%---
                                            <asp:DropDownList ID="ddlYear" runat="server" CssClass="dropDownList" Width="59px">
                                            </asp:DropDownList>--%>
                                            &nbsp;From* :
                                            <asp:TextBox ID="txtStartId" runat="server" CssClass="textBox" Width="50px"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTEStartId" runat="server" ValidChars="0123456789"
                                                TargetControlID="txtStartId">
                                            </cc1:FilteredTextBoxExtender>
                                            &nbsp;To* :
                                            <asp:TextBox ID="txtEndId" runat="server" CssClass="textBox" Width="50px">
                                            </asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FTEEndId" runat="server" ValidChars="0123456789"
                                                TargetControlID="txtEndId">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 35%; text-align: right" class="labelbold">
                                            Released By :
                                        </td>
                                        <td style="text-align: left; padding-left: 10px">
                                            <asp:DropDownList ID="ddlReleasedBy" runat="server" CssClass="dropDownList">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 35%; text-align: right" class="labelbold">
                                            Released On :
                                        </td>
                                        <td style="text-align: left; padding-left: 10px">
                                            <asp:RadioButtonList ID="rdoDate" runat="server" CssClass="radiobutton" RepeatDirection="Horizontal"
                                                CellSpacing="5" onClick="DisplayDate();">
                                                <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                                                <asp:ListItem Value="1">Specific</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr id="trDateText" runat="server" style="display: none">
                                        <td style="width: 35%; text-align: right" class="labelbold">
                                        </td>
                                        <td style="text-align: left; padding-left: 10px">
                                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="textBox" ReadOnly="true"></asp:TextBox>
                                            <cc1:CalendarExtender ID="calStartDate" runat="server" TargetControlID="txtStartDate"
                                                Format="dd-MMM-yyyy">
                                            </cc1:CalendarExtender>
                                            &nbsp;To&nbsp;
                                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="textBox" ReadOnly="true"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtEndDate"
                                                Format="dd-MMM-yyyy">
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr id="trbtnSearch" runat="server" style="display: none">
                                        <td style="width: 35%; text-align: right; vertical-align: top; padding-top: 11px"
                                            class="labelbold">
                                            Last Released Document ID :
                                        </td>
                                        <td style="text-align: left; padding-left: 10px; padding-top: 10px">
                                            <asp:Label ID="lblDocLastId" runat="server" CssClass="labelbold" Style="font-family: @Verdana;
                                                font-size: medium; color: Black; font-weight: bold"></asp:Label>
                                            <br />
                                            <br />
                                            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btnnew" 
                                                OnClientClick="return Validation();" />
                                            <asp:Button ID="btnParentAuditTrail" runat="server"
                                                CssClass="btn btnaudit"  />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                    <tr id="trReleaseDetail" runat="server">
                        <td style="top: 5px; vertical-align: top; overflow: auto; display: block; padding-top: 10px"
                            align="center">
                            <asp:HiddenField ID="hdnNodeId" runat="server" />
                            <asp:HiddenField ID="hdnFilePath" runat="server" />
                            <fieldset id="fldReleaseDetail" class="detailsbox" runat="server" style="width: 90%;
                                display: none">
                                <legend class="LabelBold" style="color: Black">&nbsp;&nbsp;Released Documents Details&nbsp;&nbsp;</legend>
                                <div>
                                    <table style="width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btnExportgvReleaseDetail" runat="server"  class="btn btnexcel" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <asp:Panel ID="pnlReleasedetail" runat="server" Style="width: 100%; max-height: 500px"
                                    ScrollBars="Auto">
                                    <asp:GridView ID="gvReleaseDetail" runat="server" SkinID="grdViewDocs" AutoGenerateColumns="false"
                                        Style="width: 99%; margin: 20px" AllowPaging="true" PageSize="50">
                                        <Columns>
                                            <asp:BoundField DataFormatString="number" HeaderText="#">
                                                <ItemStyle HorizontalAlign="Center" Width="10"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="nAutoID" HeaderText="Released Track AutoID">
                                                <ItemStyle HorizontalAlign="Left" Width="5"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="vWorkspaceId" HeaderText="WorkspaceID">
                                                <ItemStyle HorizontalAlign="Left" Width="5"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="iParentNodeId" HeaderText="Parent NodeId">
                                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="iNodeId" HeaderText="NodeID">
                                                <ItemStyle HorizontalAlign="Left" Width="50"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Doc Link">
                                                <ItemStyle HorizontalAlign="Left" Width="130" Wrap="false"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                <ItemTemplate>
                                                    <a href='<%# Eval("vFilePath") %>' target="_blank">
                                                        <%#Eval("vNodeDisplayName")%></a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="dModifyOn_IST" DataFormatString="{0:dd'-'MMM'-'yyyy H:mm tt}"
                                                HeaderText="Released On">
                                                <ItemStyle HorizontalAlign="Left" Width="60" Wrap="false"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="iReleasedBy" HeaderText="Released By">
                                                <ItemStyle HorizontalAlign="Left" Width="10" Wrap="false"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="vUserName" HeaderText="Released By">
                                                <ItemStyle HorizontalAlign="Left" Width="100" Wrap="false"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Doc. Action">
                                                <ItemStyle HorizontalAlign="Left" Width="10"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgDocAction" runat="server" ImageUrl="~/Images/docAction.png"
                                                        Height="16" Width="16"></asp:ImageButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:TemplateField>
                                            <%--<asp:BoundField DataField="vComments" HeaderText="Comments">
                                                <ItemStyle HorizontalAlign="Left" Width="250"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            </asp:BoundField>--%>
                                            <asp:TemplateField HeaderText="Comments">
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" Width="400"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtReleaseDetail" runat="server" Enabled="false" TextMode="MultiLine"
                                                        Style="width: 98%;" Text='<%#eval("vComments") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="iStageId" HeaderText="Stage">
                                                <ItemStyle HorizontalAlign="Left" Width="10"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="vFilePath" HeaderText="File Path">
                                                <ItemStyle HorizontalAlign="Left" Width="10"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                            </asp:BoundField>
                                        </Columns>
                                        <PagerSettings Position="TopAndBottom" />
                                    </asp:GridView>
                                </asp:Panel>
                                <%--DOC ACTION MODAL POP-UP--%>
                                <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <cc1:ModalPopupExtender ID="MPEAction" runat="server" PopupControlID="divDocAction"
                                            PopupDragHandleControlID="LblPopUpSubMgmt" BackgroundCssClass="modalBackground"
                                            BehaviorID="MPEAction" CancelControlID="ImgPopUpClose" TargetControlID="btnShow">
                                        </cc1:ModalPopupExtender>
                                        <table>
                                            <tr style="display: none">
                                                <td>
                                                    <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="btn btnnew"/>
                                                </td>
                                            </tr>
                                        </table>
                                        <div id="divDocAction" runat="server" class="centerModalPopup" style="display: none;
                                            overflow: auto; width: 800px; max-height: 600px">
                                            <div style="width: 100%">
                                                <h1 class="header">
                                                    <label id="lblDocAction" class="labelbold">
                                                        Doc Action
                                                    </label>
                                                    <img id="ImgPopUpClose" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
                                                </h1>
                                            </div>
                                            <asp:UpdatePanel runat="server" ID="UPDocAction" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Panel ID="pnlDocAction" runat="server" Visible="true" Style="max-height: 500px">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td align="center" class="Label" colspan="2" valign="top">
                                                                    <table style="width: 100%" cellpadding="3">
                                                                        <tr>
                                                                            <td align="left" style="width: 30%; text-align: right; vertical-align: top" class="labelbold">
                                                                                Comments :
                                                                            </td>
                                                                            <td style="text-align: left; padding-left: 5px">
                                                                                <asp:TextBox ID="txtDocComments" runat="server" Width="250" TextMode="MultiLine"
                                                                                    Height="50">
                                                                                </asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" style="width: 30%; text-align: right" class="labelbold">
                                                                                Action :
                                                                            </td>
                                                                            <td style="text-align: left; padding-left: 5px">
                                                                                <asp:RadioButtonList ID="rdoAction" runat="server" CssClass="radiobutton" RepeatDirection="Horizontal">
                                                                                    <asp:ListItem Value="P">Re-Print</asp:ListItem>
                                                                                    <asp:ListItem Value="R" Selected="True">Returned</asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" width="200">
                                                                            </td>
                                                                            <td align="left" style="width: 3px">
                                                                                <asp:Button ID="btnSaveAction" runat="server" Text="Save Action" CssClass="btn btnsave"
                                                                                    Font-Bold="True" OnClientClick="return ActionValidation();"></asp:Button>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <caption>
                                                                <br />
                                                            </caption>
                                                        </table>
                                                        <fieldset class="detailsbox" id="fldDocTrail" style="display: ; max-height: 300px;">
                                                            <legend class="LabelBold" style="color: black; text-align: left">
                                                                <img alt="Expand" src="images/Collapse.jpg" id="imgEditParent" onclick="ShowDetail(this);" />
                                                                <strong>Audit Trail </strong></legend>
                                                            <div id="divProjectDetail" runat="server" class="Detailpanel" style="display: ; max-height: 300px">
                                                                <asp:Panel ID="pnlEditParent" runat="server" ScrollBars="auto" Style="max-height: 250px;
                                                                    padding: 0">
                                                                    <asp:GridView ID="gvAuditTrail" runat="server" SkinID="grdViewSmlAutoSize" AutoGenerateColumns="false"
                                                                        Width="99%">
                                                                        <Columns>
                                                                            <asp:BoundField DataFormatString="number" HeaderText="#">
                                                                                <ItemStyle HorizontalAlign="Center" Width="20"></ItemStyle>
                                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="vNodeDisplayName" HeaderText="Form Name">
                                                                                <ItemStyle HorizontalAlign="Left" Width="150" Wrap="false"></ItemStyle>
                                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="ModifyByUser" HeaderText="Performed By">
                                                                                <ItemStyle HorizontalAlign="Left" Width="150" Wrap="false"></ItemStyle>
                                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="dModifyOn_IST" HeaderText="Performed On" DataFormatString="{0:dd'-'MMM'-'yyyy H:mm tt}">
                                                                                <ItemStyle HorizontalAlign="Center" Width="130" Wrap="false"></ItemStyle>
                                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="cStatusIndi" HeaderText="Action">
                                                                                <ItemStyle HorizontalAlign="Center" Width="80"></ItemStyle>
                                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                            </asp:BoundField>
                                                                            <%--<asp:BoundField DataField="vComments" HeaderText="Comments">
                                                                                <ItemStyle HorizontalAlign="Left" Width="200"></ItemStyle>
                                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                                                            </asp:BoundField>--%>
                                                                            <asp:TemplateField HeaderText="Comments">
                                                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                <ItemStyle HorizontalAlign="Left" Width="300"></ItemStyle>
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtAuditTrailComment" runat="server" Enabled="false" Style="width: 98%;"
                                                                                        TextMode="MultiLine" Text='<%#eval("vComments") %>'></asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                            </div>
                                                        </fieldset>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                                <Triggers>
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </fieldset>
                            <fieldset id="fldReleaseMsg" class="detailsbox" runat="server" style="width: 90%;
                                display: none">
                                <legend class="LabelBold" style="color: Black"></legend>
                                <asp:Panel ID="pnlReleaseMsg" runat="server" Style="width: 850px; max-height: 30px"
                                    ScrollBars="Auto">
                                    <asp:Label ID="lblMsg" Text="No Data Found" CssClass="labelbold" Style="background-color: Silver;
                                        font-weight: bold; color: Navy">&nbsp;&nbsp;NO DATA FOUND&nbsp;&nbsp;</asp:Label>
                                </asp:Panel>
                            </fieldset>
                            <fieldset id="fldParentAuditTrail" class="detailsbox" runat="server" style="width: 90%;
                                display: none">
                                <legend class="LabelBold" style="color: Black">&nbsp;&nbsp;Form Wise Audit Trail&nbsp;&nbsp;</legend>
                                <asp:Panel ID="pnlParentAuditTrail" runat="server" Style="width: 98%; max-height: 500px"
                                    ScrollBars="Auto">
                                    <asp:GridView ID="gvParentAuditTrail" runat="server" SkinID="grdViewSmlAutoSize"
                                        Width="99%" AutoGenerateColumns="false" AllowPaging="true" PageSize="15">
                                        <Columns>
                                            <asp:BoundField DataFormatString="number" HeaderText="#">
                                                <ItemStyle HorizontalAlign="Center" Width="20"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="vNodeDisplayName" HeaderText="Form Name">
                                                <ItemStyle HorizontalAlign="Left" Width="120" Wrap="false"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ModifyByUser" HeaderText="Performed By">
                                                <ItemStyle HorizontalAlign="Left" Width="120" Wrap="false"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="dModifyOn_IST" HeaderText="Performed On" DataFormatString="{0:dd'-'MMM'-'yyyy H:mm tt}">
                                                <ItemStyle HorizontalAlign="Left" Width="70" Wrap="false"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="cStatusIndi" HeaderText="Action">
                                                <ItemStyle HorizontalAlign="Center" Width="60" Wrap="false"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                            </asp:BoundField>
                                            <%--<asp:BoundField DataField="vComments" HeaderText="Comments">
                                                <ItemStyle HorizontalAlign="Left" Width="300"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                            </asp:BoundField>--%>
                                            <asp:TemplateField HeaderText="Comments">
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" Width="400"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtParentAuditTrailComment" runat="server" Enabled="false" Style="width: 98%;"
                                                        TextMode="MultiLine" Text='<%#eval("vComments") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Position="TopAndBottom" />
                                    </asp:GridView>
                                </asp:Panel>
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center">
                            <input type="button" onclick="onUpdating();document.location.href='frmMainPage.aspx?mode=1';"
                                value="Exit" class="button" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
            <asp:PostBackTrigger ControlID="btnExportgvReleaseDetail" />
            </Triggers>            
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">

        function DisplayDate() {
            if (document.getElementById('ctl00_CPHLAMBDA_rdoDate_1').checked) {
                document.getElementById('ctl00_CPHLAMBDA_trDateText').style.display = '';
            }
            else {
                document.getElementById('ctl00_CPHLAMBDA_trDateText').style.display = 'none';
            }
            return false;
        }

        function Validation() {
            if (document.getElementById('ctl00_CPHLAMBDA_txtStartId').value == '') {
                msgalert("Enter From Value !");
                document.getElementById('ctl00_CPHLAMBDA_txtStartId').focus();
                document.getElementById('ctl00_CPHLAMBDA_txtStartId').style.backgroundColor = "#FFE6F7";
                return false;
            }
            else if (document.getElementById('ctl00_CPHLAMBDA_txtEndId').value == '') {
                msgalert("Enter End Value !");
                document.getElementById('ctl00_CPHLAMBDA_txtEndId').focus();
                document.getElementById('ctl00_CPHLAMBDA_txtEndId').style.backgroundColor = "#FFE6F7";
                return false;
            }

            var Start, End;
            Start = document.getElementById('ctl00_CPHLAMBDA_txtStartId').value;
            End = document.getElementById('ctl00_CPHLAMBDA_txtEndId').value;

            if (parseInt(Start) > parseInt(End)) {
                msgalert("From value must not greater than end value !");
                document.getElementById('ctl00_CPHLAMBDA_txtStartId').focus();
                document.getElementById('ctl00_CPHLAMBDA_txtStartId').style.backgroundColor = "#FFE6F7";
                return false;
            }
            document.getElementById('ctl00_CPHLAMBDA_hdnStartDate').value = document.getElementById('ctl00_CPHLAMBDA_txtStartDate').value;
            document.getElementById('ctl00_CPHLAMBDA_hdnEndDate').value = document.getElementById('ctl00_CPHLAMBDA_txtEndDate').value;
        }

        function ActionValidation() {
            if (document.getElementById('ctl00_CPHLAMBDA_txtDocComments').value == '') {
                msgalert("Please Enter Comments !");
                document.getElementById('ctl00_CPHLAMBDA_txtDocComments').focus();
                document.getElementById('ctl00_CPHLAMBDA_txtDocComments').style.backgroundColor = "#FFE6F7";
                return false;
            }
            return true;
        }

        function ShowDetail(ele) {
            if (ele.src.toUpperCase().search('EXPAND') != -1) {
                $(".Detailpanel").slideToggle(500);
                ele.src = "images/collapse.jpg";
            }
            else {
                $(".Detailpanel").slideToggle(500);
                ele.src = "images/expand.jpg";
            }
        }

        function printDocument(filePath) {
            $("#iFramePdf").each(function() {
                $(this).remove();
            });

            var iframe = document.createElement("iframe");
            iframe.id = "iFramePdf";
            iframe.style.display = "none";
            iframe.src = filePath;
            document.body.appendChild(iframe);

            $("#iFramePdf").contents().find("body").attr("oncontextmenu", "return false;");

            var div = document.createElement('div');
            div.id = "tempDiv";
            div.style.top = '45%';
            div.style.left = '30%';
            div.style.color = "#AAA";
            div.style.zindex = '10000';
            div.style.position = 'fixed';
            div.style.fontWeight = 'bold';
            div.style.fontSize = '50px';
            div.innerHTML = 'Printing in progress...';

            var ele = $('body');
            $(ele).append(div);
            $('#iFramePdf').load(function() {
                setTimeout("$('#tempDiv').remove();$('#iFramePdf').get(0).contentWindow.print();$('#MPEAction_backgroundElement').css('display','none');", 5000);
            });
        }
    </script>

</asp:Content>
