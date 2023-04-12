<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/MasterPage.master"
    CodeFile="frmProjectSubmissionDtl.aspx.vb" Inherits="frmProjectSubmissionDtl" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="content1" ContentPlaceHolderID="CPHLAMBDA" runat="server">
    <div>
        <br />
        <table style="width: 90%">
            <tr id="trSelection" runat="server" style="white-space: nowrap">
                <td>
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <center>
                                <table width="90%">
                                    <tr align="center">
                                        <td class="Label" align="center">
                                            <strong>Project Name : </strong>
                                        </td>
                                        <td align="center">
                                            <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="622px" TabIndex="0"></asp:TextBox>
                                            <asp:HiddenField ID="HProjectId" runat="server" />
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                                                TargetControlID="txtProject" ServicePath="AutoComplete.asmx" ServiceMethod="GetMyProjectCompletionList"
                                                OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1"
                                                CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1">
                                            </cc1:AutoCompleteExtender>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click"></asp:AsyncPostBackTrigger>
                        </Triggers>
                    </asp:UpdatePanel>
                    &nbsp; &nbsp;&nbsp;
                </td>
            </tr>
        </table>
        <br />
        <table style="width: 90%">
            <tr id="trMainContainer" runat="server" style="display: none" align="center">
                <td>
                    <table style="width: 98%">
                        <tr align="center">
                            <td>
                                <strong>:: Project Details ::</strong><br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="panel1" runat="server" Style="border-style: solid; border-width: thin">
                                    <asp:Button ID="btnSetProject" runat="server" Style="display: none" Text=" Project" CssClass="btn btnnew" />
                                    <table cellpadding="3" width="98%">
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
                                        <tr id="trRegionName" runat="server">
                                            <td align="left" style="width: 150px">
                                                Region Name :
                                            </td>
                                            <td align="left" style="width: 500px; font-weight: bold; color: Black">
                                                <asp:Label ID="lblRegionName" runat="server" Text="" />
                                            </td>
                                        </tr>
                                        <tr id="trTemplateName" runat="server">
                                            <td align="left" style="width: 150px">
                                                Template Name :
                                            </td>
                                            <td align="left" style="width: 500px; font-weight: bold; color: Black">
                                                <asp:Label ID="lblTemplateName" runat="server" Text="" />
                                            </td>
                                        </tr>
                                        <tr id="trDetailsEditedBy" runat="server">
                                            <td align="left" style="width: 150px">
                                                Details Edited By :
                                            </td>
                                            <td align="left" style="width: 500px; font-weight: bold; color: Black">
                                                <asp:Label ID="lblDetailsEditedBy" runat="server" Text="" />
                                            </td>
                                        </tr>
                                        <tr id="trDetailsEditedOn" runat="server">
                                            <td align="left" style="width: 150px">
                                                Details Edited On :
                                            </td>
                                            <td align="left" style="width: 500px; font-weight: bold; color: Black">
                                                <asp:Label ID="lblDetailsEditedOn" runat="server" Text="" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <br />
                            </td>
                        </tr>
                        <tr id="trPanel2" runat="server" style="display: none">
                            <td>
                                <asp:Panel ID="panel2" runat="server" Style="border-style: solid; border-width: thin">
                                    <asp:UpdatePanel runat="server" ID="UpPanel2" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellpadding="3" width="100%">
                                                <tr id="trRegion" runat="server">
                                                    <td align="left" width="75">
                                                        Region* :
                                                    </td>
                                                    <td align="left" style="width: 3px">
                                                        <asp:DropDownList ID="ddlRegion" runat="server" CssClass="dropDownList" Width="220px"
                                                            AutoPostBack="true" TabIndex="1">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="trCountry" runat="server">
                                                    <td align="left" width="75">
                                                        Country* :
                                                    </td>
                                                    <td align="left" style="width: 3px">
                                                        <asp:DropDownList ID="ddlCountry" runat="server" CssClass="dropDownList" Width="220px"
                                                            AutoPostBack="true" TabIndex="2">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="trAgencyName" runat="server">
                                                    <td align="left" width="75">
                                                        Agency Name* :
                                                    </td>
                                                    <td align="left" style="width: 3px">
                                                        <asp:DropDownList ID="ddlAgencyName" runat="server" CssClass="dropDownList" Width="220px">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="trRegionalVersion" runat="server" style="display: none">
                                                    <td align="left" width="75">
                                                        Regional Version* :
                                                    </td>
                                                    <td align="left" style="width: 3px">
                                                        <asp:DropDownList ID="ddlRegionalVersion" runat="server" CssClass="dropDownList"
                                                            Width="220px">
                                                            <asp:ListItem Selected="True">Select Regional Version</asp:ListItem>
                                                            <asp:ListItem>1.4</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="trApplicationNo" runat="server" style="display: none">
                                                    <td align="left" width="75">
                                                        Application Number* :
                                                    </td>
                                                    <td align="left" style="width: 3px">
                                                        <asp:TextBox ID="txtApplicationNumber" runat="server" MaxLength="7">                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr id="trTrackingNo" runat="server" style="display: none">
                                                    <td align="left" width="75">
                                                        Tracking Number* :
                                                    </td>
                                                    <td align="left" style="width: 3px">
                                                        <asp:TextBox ID="txtTrackingNumber" runat="server" MaxLength="250">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="BtnSave" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </asp:Panel>
                                <br />
                            </td>
                        </tr>
                        <tr id="trPanel3" runat="server" style="display: none">
                            <td align="left">
                                <asp:Panel ID="panel3" runat="server" Style="border-style: solid; border-width: thin">
                                    <asp:UpdatePanel runat="server" ID="UpPanel3" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table cellpadding="3" width="100%">
                                                <tr style="width: 100%">
                                                    <td>
                                                        &nbsp;<asp:Label ID="lblDetailTag" Font-Names="" runat="server" Font-Bold="true"
                                                            Text="" />
                                                        <hr style="border-style: solid; border-width: thin" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <table cellpadding="3">
                                                <tr id="trCompanyName" runat="server" style="display: none">
                                                    <td align="left" width="230">
                                                        Company Name* :
                                                    </td>
                                                    <td align="left" style="width: 3px">
                                                        <asp:TextBox ID="txtCompanyName" runat="server" MaxLength="100">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr id="trProductName" runat="server" style="display: none">
                                                    <td align="left" width="230">
                                                        Product Name* :
                                                    </td>
                                                    <td align="left" style="width: 3px">
                                                        <asp:TextBox ID="txtProductName" runat="server" MaxLength="100">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr id="trProductType" runat="server" style="display: none">
                                                    <td align="left" width="230">
                                                        Product Type* :
                                                    </td>
                                                    <td align="left" style="width: 3px">
                                                        <asp:DropDownList ID="ddlProductType" runat="server" CssClass="dropDownList" Width="220px">
                                                            <asp:ListItem Value="0" Selected="True">Select Product Type</asp:ListItem>
                                                            <asp:ListItem>established</asp:ListItem>
                                                            <asp:ListItem>proprietary</asp:ListItem>
                                                            <asp:ListItem>chemical</asp:ListItem>
                                                            <asp:ListItem>code</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="trApplicationType" runat="server" style="display: none">
                                                    <td align="left" width="230">
                                                        Application Type* :
                                                    </td>
                                                    <td align="left" style="width: 3px">
                                                        <asp:DropDownList ID="ddlApplicationType" runat="server" CssClass="dropDownList"
                                                            Width="220px">
                                                            <asp:ListItem Value="0" Selected="True">Select Application Type</asp:ListItem>
                                                            <asp:ListItem>nda</asp:ListItem>
                                                            <asp:ListItem>anda</asp:ListItem>
                                                            <asp:ListItem>bla</asp:ListItem>
                                                            <asp:ListItem>dmf</asp:ListItem>
                                                            <asp:ListItem>ind</asp:ListItem>
                                                            <asp:ListItem>master-file</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr id="trHighLevelNo" runat="server" style="display: none">
                                                    <td align="left" width="230">
                                                        High Level Number :
                                                    </td>
                                                    <td align="left" style="width: 3px">
                                                        <asp:TextBox ID="txtHighLevelNumber" runat="server" MaxLength="150">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr id="trApplicant" runat="server" style="display: none">
                                                    <td align="left" width="230">
                                                        Applicant* :
                                                    </td>
                                                    <td align="left" style="width: 3px">
                                                        <asp:TextBox ID="txtApplicant" runat="server" MaxLength="50">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr id="trProcedureType" runat="server" style="display: none">
                                                    <td align="left" width="230">
                                                        Procedure Type* :
                                                    </td>
                                                    <td align="left" style="width: 230px">
                                                        <asp:DropDownList ID="ddlProcedureType" runat="server" CssClass="dropDownList" Width="220px"
                                                            AutoPostBack="true">
                                                            <asp:ListItem Value="0" Selected="True">Select Procedure Type</asp:ListItem>
                                                            <asp:ListItem>centralised</asp:ListItem>
                                                            <asp:ListItem>national</asp:ListItem>
                                                            <asp:ListItem>mutual-recognition</asp:ListItem>
                                                            <asp:ListItem>decentralised</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <div id="divbtn" runat="server" style="display: none">
                                                        </div>
                                                    </td>
                                                    <td id="tdCMSDetails" runat="server" style="display: none; width: 108" align="left">
                                                        <asp:Button ID="btnCMSDetails" runat="server" CssClass="btn btnnew" Text="CMS Details"/>
                                                        <asp:Button ID="btncall" runat="server" Style="display: none" Text="Open CMS" />
                                                        <cc1:ModalPopupExtender ID="MPECMS" runat="server" PopupControlID="divCMS" PopupDragHandleControlID="LblPopUpSubMgmt"
                                                            BackgroundCssClass="modalBackground" BehaviorID="MPECMS" TargetControlID="btncall"
                                                            CancelControlID="ImgPopUpClose">
                                                        </cc1:ModalPopupExtender>
                                                        <div id="divCMS" runat="server" class="centerModalPopup" style="display: none; overflow: auto;
                                                            width: 700px">
                                                            <div style="width: 100%">
                                                                <h1 class="header">
                                                                    <label id="lblAuditTrail" class="labelbold">
                                                                        CMS DETAILS
                                                                    </label>
                                                                    <img id="ImgPopUpClose" alt="Close" src="images/Sqclose.gif" onmouseover="this.style.cursor='pointer';" />
                                                                </h1>
                                                            </div>
                                                            <asp:UpdatePanel runat="server" ID="UpPanelCMS" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Panel ID="pnlCMS" runat="server" Visible="true">
                                                                        <table cellpadding="2" style="width: 100%">
                                                                            <tr>
                                                                                <td align="center" class="Label" colspan="2" valign="top">
                                                                                    <table style="width: 75%" cellpadding="3">
                                                                                        <tr>
                                                                                            <td align="left" width="200">
                                                                                                Wave* :
                                                                                            </td>
                                                                                            <td align="left" style="width: 3px">
                                                                                                <asp:TextBox ID="txtWave" runat="server" Width="50" Text="1" MaxLength="4">
                                                                                                </asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" width="200">
                                                                                                Country Name* :
                                                                                            </td>
                                                                                            <td align="left" style="width: 3px">
                                                                                                <asp:DropDownList ID="ddlCMSCountryName" runat="server" CssClass="dropDownList" Width="220px"
                                                                                                    AutoPostBack="true">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" width="200">
                                                                                                Agency Name* :
                                                                                            </td>
                                                                                            <td align="left" style="width: 3px">
                                                                                                <asp:DropDownList ID="ddlCMSAgencyName" runat="server" CssClass="dropDownList" Width="220px">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" width="200">
                                                                                                Tracking Number :
                                                                                            </td>
                                                                                            <td align="left" style="width: 3px">
                                                                                                <asp:TextBox ID="txtCMSTrackingNumber" runat="server" Width="75" MaxLength="50">
                                                                                                </asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" width="200">
                                                                                            </td>
                                                                                            <td align="left" style="width: 3px">
                                                                                                <asp:Button ID="btnAddCMS" runat="server" Text="ADD CMS" CssClass="btn btnnew"
                                                                                                    Font-Bold="True" OnClientClick="return CMSValidation();"></asp:Button>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <caption>
                                                                                <br />
                                                                            </caption>
                                                                        </table>
                                                                        <table style="width: 100%">
                                                                            <tr align="center">
                                                                                <td>
                                                                                    <asp:Panel ID="pnlGrid" runat="server" ScrollBars="Auto" Width="98%" Height="230">
                                                                                        <asp:GridView ID="gvCMS" runat="server" AutoGenerateColumns="False" PageSize="5"
                                                                                            SkinID="grdViewSmlAutoSize" Visible="true" Width="100%">
                                                                                            <Columns>
                                                                                                <asp:BoundField DataFormatString="number" HeaderText="#">
                                                                                                    <ItemStyle HorizontalAlign="Center" Width="30"></ItemStyle>
                                                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="iWorkspaceCMSId" HeaderText="Id">
                                                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="vWorkspaceId" HeaderText="Id">
                                                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="iWaveNo" HeaderText="Wave">
                                                                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="vCountryName" HeaderText="CMS Name">
                                                                                                    <ItemStyle HorizontalAlign="Left" Width="150"></ItemStyle>
                                                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                                </asp:BoundField>
                                                                                                <asp:BoundField DataField="vAgencyName" HeaderText="Agency">
                                                                                                    <ItemStyle HorizontalAlign="Left" Width="150"></ItemStyle>
                                                                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                                                                </asp:BoundField>
                                                                                                <asp:TemplateField HeaderText="Tracking Number">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:TextBox ID="txtTrackingNumber" runat="server" Enabled="false" MaxLength="50"
                                                                                                            Width="150" Text='<%#eval("vCMSTrackingNo") %>'></asp:TextBox>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Update">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:ImageButton ID="ImgSave" runat="server" ImageUrl="~/images/save.gif"></asp:ImageButton>
                                                                                                        <asp:ImageButton ID="ImgCancel" runat="server" ImageUrl="~/images/Cancel.gif"></asp:ImageButton>
                                                                                                        <asp:ImageButton ID="ImgEdit" runat="server" ImageUrl="~/images/Edit2.gif"></asp:ImageButton>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Delete">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:ImageButton ID="ImgDelete" runat="server" ImageUrl="~/images/cancel.gif"></asp:ImageButton>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                                </asp:TemplateField>
                                                                                            </Columns>
                                                                                        </asp:GridView>
                                                                                    </asp:Panel>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="ddlCMSCountryName" EventName="SelectedIndexChanged" />
                                                                    <asp:AsyncPostBackTrigger ControlID="GVCMS" EventName="RowCommand" />
                                                                    <asp:AsyncPostBackTrigger ControlID="btnAddCMS" EventName="Click" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr id="trInventedName" runat="server" style="display: none">
                                                    <td align="left" width="230">
                                                        Invented Name* :
                                                    </td>
                                                    <td align="left" style="width: 3px">
                                                        <asp:TextBox ID="txtInventedName" runat="server" MaxLength="100">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr id="trINN" runat="server" style="display: none">
                                                    <td align="left" width="230">
                                                        INN* :
                                                    </td>
                                                    <td align="left" style="width: 3px">
                                                        <asp:TextBox ID="txtINN" runat="server" MaxLength="250">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr id="trSubmissionDesc" runat="server" style="display: none">
                                                    <td align="left" width="230">
                                                        Submission Description :
                                                    </td>
                                                    <td align="left" style="width: 3px">
                                                        <asp:TextBox ID="txtSubmissionDesc" runat="server" MaxLength="100">
                                                        </asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="ddlRegion" />
                                            <asp:AsyncPostBackTrigger ControlID="BtnSave" />
                                            <asp:AsyncPostBackTrigger ControlID="ddlCMSCountryName" EventName="SelectedIndexChanged" />
                                            <asp:AsyncPostBackTrigger ControlID="GVCMS" EventName="RowCommand" />
                                            <asp:AsyncPostBackTrigger ControlID="btnAddCMS" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </asp:Panel>
                                <br />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <!-- SAVE CLEAR EXIT BUTTONS -->
            <tr id="trButtons" runat="server" align="center" style="display: none">
                <td>
                    <br />
                    <asp:UpdatePanel ID="UpdatePanelDummy" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table id="tdContainer" runat="server">
                                <tbody>
                                    <tr runat="server" style="display: block">
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                <ContentTemplate>
                                                    <asp:Button ID="BtnSave" runat="server" Text="Save Submission Details" 
                                                        CssClass="btn btnsave" Font-Bold="True" OnClientClick="return Validation();"></asp:Button>                                                    
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click"></asp:AsyncPostBackTrigger>
                                                </Triggers>
                                            </asp:UpdatePanel>
                                            <br />
                                            <br />
                                            &nbsp;
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript" language="javascript" src="Script/popcalendar.js"></script>

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" src="Script/Validation.js"></script>

    <script type="text/javascript" language="javascript">
        function ClientPopulated(sender, e)
        {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e)
        {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }
        
        function CloseDiv()
        {
            var e = $find("MPECMS");
            e.hide();
            return true;
        }
        
        function ShowDiv()
        {
            var e = $find("MPECMS");
            e.show();
        }
        
        function Validation() {
            
            if (document.getElementById('<%=ddlRegion.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Region !');
                document.getElementById('<%=ddlRegion.ClientID%>').focus();
                document.getElementById('<%=ddlRegion.ClientID%>').style.backgroundColor="#FFE6F7"; 
                return false;
            }
            else if (document.getElementById('<%=ddlCountry.ClientID%>').selectedIndex == 0) {
                msgalert('Please Select Country !');
                document.getElementById('<%=ddlCountry.ClientID%>').focus();
                document.getElementById('<%=ddlCountry.ClientID%>').style.backgroundColor="#FFE6F7"; 
                return false;
            }
            
            var ddl = document.getElementById("<%=ddlRegion.ClientID%>");
            var Text = ddl.options[ddl.selectedIndex].text; 
            var Value = ddl.options[ddl.selectedIndex].value;
            //alert(Text);
            //alert(Value);
            
            if(Text == 'US')
            {
                if (document.getElementById('<%=ddlAgencyName.ClientID%>').selectedIndex == 0) {
                    msgalert('Please Select Agency !');
                    document.getElementById('<%=ddlAgencyName.ClientID%>').focus();
                    document.getElementById('<%=ddlAgencyName.ClientID%>').style.backgroundColor="#FFE6F7"; 
                    return false;
                }
                else if (document.getElementById('<%=txtApplicationNumber.ClientID%>').value == '') {
                    msgalert('Please specify Application Number !');
                    document.getElementById('<%=txtApplicationNumber.ClientID%>').focus();
                    document.getElementById('<%=txtApplicationNumber.ClientID%>').style.backgroundColor="#FFE6F7"; 
                    return false;
                }
                else if (document.getElementById('<%=txtCompanyName.ClientID%>').value == '') {
                    msgalert('Please specify Company Name !');
                    document.getElementById('<%=txtCompanyName.ClientID%>').focus();
                    document.getElementById('<%=txtCompanyName.ClientID%>').style.backgroundColor="#FFE6F7"; 
                    return false;
                }
                else if (document.getElementById('<%=txtProductName.ClientID%>').value == '') {
                    msgalert('Please specify Product Name !');
                    document.getElementById('<%=txtProductName.ClientID%>').focus();
                    document.getElementById('<%=txtProductName.ClientID%>').style.backgroundColor="#FFE6F7"; 
                    return false;
                }
                else if (document.getElementById('<%=ddlProductType.ClientID%>').selectedIndex == 0) {
                    msgalert('Please Select Product Type !');
                    document.getElementById('<%=ddlProductType.ClientID%>').focus();
                    document.getElementById('<%=ddlProductType.ClientID%>').style.backgroundColor="#FFE6F7"; 
                    return false;
                }
                else if (document.getElementById('<%=ddlApplicationType.ClientID%>').selectedIndex == 0) {
                    msgalert('Please Select Application Type !');
                    document.getElementById('<%=ddlApplicationType.ClientID%>').focus();
                    document.getElementById('<%=ddlApplicationType.ClientID%>').style.backgroundColor="#FFE6F7"; 
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
            else if(Text == 'EU')
            {
                if (document.getElementById('<%=ddlRegionalVersion.ClientID%>').selectedIndex == 0) {
                    msgalert('Please Select Regional Version !');
                    document.getElementById('<%=ddlRegionalVersion.ClientID%>').focus();
                    document.getElementById('<%=ddlRegionalVersion.ClientID%>').style.backgroundColor="#FFE6F7"; 
                    return false;
                }
                else if (document.getElementById('<%=ddlAgencyName.ClientID%>').selectedIndex == 0) {
                    msgalert('Please Select Agency !');
                    document.getElementById('<%=ddlAgencyName.ClientID%>').focus();
                    document.getElementById('<%=ddlAgencyName.ClientID%>').style.backgroundColor="#FFE6F7"; 
                    return false;
                }
                else if (document.getElementById('<%=ddlAgencyName.ClientID%>').selectedIndex == 0) {
                    msgalert('Please Select Agency !');
                    document.getElementById('<%=ddlAgencyName.ClientID%>').focus();
                    document.getElementById('<%=ddlAgencyName.ClientID%>').style.backgroundColor="#FFE6F7"; 
                    return false;
                }
                else if (document.getElementById('<%=txtTrackingNumber.ClientID%>').value == '') {
                    msgalert('Please specify Tracking Number !');
                    document.getElementById('<%=txtTrackingNumber.ClientID%>').focus();
                    document.getElementById('<%=txtTrackingNumber.ClientID%>').style.backgroundColor="#FFE6F7"; 
                    return false;
                }
//                else if (document.getElementById('<%=txtHighLevelNumber.ClientID%>').value == '') {
//                   alert('Please specify High Level Number');
//                    document.getElementById('<%=txtHighLevelNumber.ClientID%>').focus();
//                    document.getElementById('<%=txtHighLevelNumber.ClientID%>').style.backgroundColor="#FFE6F7"; 
//                    return false;
//                }
                else if (document.getElementById('<%=txtApplicant.ClientID%>').value == '') {
                    msgalert('Please specify Applicant !');
                    document.getElementById('<%=txtApplicant.ClientID%>').focus();
                    document.getElementById('<%=txtApplicant.ClientID%>').style.backgroundColor="#FFE6F7"; 
                    return false;
                }
                else if (document.getElementById('<%=ddlProcedureType.ClientID%>').selectedIndex == 0) {
                    msgalert('Please Select Procedure Type !');
                    document.getElementById('<%=ddlProcedureType.ClientID%>').focus();
                    document.getElementById('<%=ddlProcedureType.ClientID%>').style.backgroundColor="#FFE6F7"; 
                    return false;
                }
                else if (document.getElementById('<%=txtInventedName.ClientID%>').value == '') {
                    msgalert('Please specify Invented Name !');
                    document.getElementById('<%=txtInventedName.ClientID%>').focus();
                    document.getElementById('<%=txtInventedName.ClientID%>').style.backgroundColor="#FFE6F7"; 
                    return false;
                }
                else if (document.getElementById('<%=txtINN.ClientID%>').value == '') {
                    msgalert('Please specify INN !');
                    document.getElementById('<%=txtINN.ClientID%>').focus();
                    document.getElementById('<%=txtINN.ClientID%>').style.backgroundColor="#FFE6F7"; 
                    return false;
                }
//                else if (document.getElementById('<%=txtSubmissionDesc.ClientID%>').value == '') {
//                   alert('Please specify Description');
//                    document.getElementById('<%=txtSubmissionDesc.ClientID%>').focus();
//                    document.getElementById('<%=txtSubmissionDesc.ClientID%>').style.backgroundColor="#FFE6F7"; 
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
            }
            else if(Text == 'CA')
            {
                if (document.getElementById('<%=ddlAgencyName.ClientID%>').selectedIndex == 0) {
                    msgalert('Please Select Agency !');
                    document.getElementById('<%=ddlAgencyName.ClientID%>').focus();
                    document.getElementById('<%=ddlAgencyName.ClientID%>').style.backgroundColor="#FFE6F7"; 
                    return false;
                }
                else if (document.getElementById('<%=txtApplicationNumber.ClientID%>').value == '') {
                    msgalert('Please specify Application Number !');
                    document.getElementById('<%=txtApplicationNumber.ClientID%>').focus();
                    document.getElementById('<%=txtApplicationNumber.ClientID%>').style.backgroundColor="#FFE6F7"; 
                    return false;
                }
                else if (document.getElementById('<%=txtProductName.ClientID%>').value == '') {
                    msgalert('Please specufy Product Name !');
                    document.getElementById('<%=txtProductName.ClientID%>').focus();
                    document.getElementById('<%=txtProductName.ClientID%>').style.backgroundColor="#FFE6F7"; 
                    return false;
                }
                else if (document.getElementById('<%=txtApplicant.ClientID%>').value == '') {
                    msgalert('Please specify Applicant !');
                    document.getElementById('<%=txtApplicant.ClientID%>').focus();
                    document.getElementById('<%=txtApplicant.ClientID%>').style.backgroundColor="#FFE6F7"; 
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
			    
			    if(appno.length != 6)
			    {
			        msgalert("Application Number must be of 7 characters !");
		    		document.getElementById('<%=txtApplicationNumber.ClientID%>').focus();
                    document.getElementById('<%=txtApplicationNumber.ClientID%>').style.backgroundColor="#FFE6F7"; 
				   	return false;
			    }
			    
            }
                 
            return true;
        }
        

        //------------------------ CMS VALIDATION --------------------------
        function CMSValidation() {
                      
            if (document.getElementById('<%=txtWave.ClientID%>').value == '') {
                msgalert('Please Enter Wave !');
                document.getElementById('<%=txtWave.ClientID%>').focus();
                document.getElementById('<%=txtWave.ClientID%>').style.backgroundColor="#FFE6F7"; 
                return false;
            }
            if (document.getElementById('<%=ddlCMSCountryName.ClientID%>').selectedIndex <= 0) {
                msgalert('Please Select Country !');
                document.getElementById('<%=ddlCMSCountryName.ClientID%>').focus();
                document.getElementById('<%=ddlCMSCountryName.ClientID%>').style.backgroundColor="#FFE6F7"; 
                return false;
            }
            
            if (document.getElementById('<%=ddlCMSAgencyName.ClientID%>').selectedIndex <= 0) {
                msgalert('Please Select Agency !');
                document.getElementById('<%=ddlCMSAgencyName.ClientID%>').focus();
                document.getElementById('<%=ddlCMSAgencyName.ClientID%>').style.backgroundColor="#FFE6F7"; 
                return false;
            }
            
            var cmstrackingnum = document.getElementById('<%=txtCMSTrackingNumber.ClientID%>').value;
            //alert(trackingnum);
            if(!cmstrackingnum.match(/^([a-zA-Z0-9\/\u002D\u002C])*$/))
			{
                msgalert("Only digits, Alphabets, '-' , '/' and ',' are allowed !");
				document.getElementById('<%=txtCMSTrackingNumber.ClientID%>').focus();
                document.getElementById('<%=txtCMSTrackingNumber.ClientID%>').style.backgroundColor="#FFE6F7"; 
			  	return false;
			}
                        
            return true;
        }
        
        
    </script>

</asp:Content>
