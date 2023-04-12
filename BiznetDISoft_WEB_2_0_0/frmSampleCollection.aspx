<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="frmSampleCollection.aspx.vb" Inherits="frmSampleCollection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <script type="text/javascript" src="Script/scrollablegrid.js"></script>
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <script  type="text/javascript" src="Script/jquery.scannerdetection.js"></script>

    <style type="text/css">
        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }
    </style>
    <asp:UpdatePanel runat="server" ID="UpControls" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellpadding="5px" style="width: 100%;">
                <tr>
                    <td>
                        <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                            <legend class="LegendText" style="color: Black; font-size: 12px">
                                <img id="img2" alt="Project Details" src="images/panelcollapse.png"
                                    onclick="Display(this,'divProjectDetail');" runat="server" style="margin-right: 2px;" />Project Details</legend>
                            <div id="divProjectDetail">
                                <asp:Button runat="server" ID="btndefault" Style="display: none" OnClientClick="return PassTheBarcode();" />
                                <table style="width: 100%;" cellpadding="5px">
                                    <tr id="VersionDtl" runat="server" style="display: none;">
                                        <td></td>
                                        <td colspan="4" class="Label" style="text-align: center; padding-left: 15%;">Version :<asp:Label runat="server" ID="VersionNo" Style="padding-right: 10px"></asp:Label>Version
                        Date :<asp:Label ID="VersionDate" Style="padding-right: 10px;" runat="server"></asp:Label>
                                            Status:<img src="images/Freeze.jpg" id="ImageLockUnlock" runat="server" alt="Lock" />
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="Label" style="white-space: nowrap; text-align: right; width: 11%;">Project* :
                                        </td>
                                        <td style="text-align: left; width: 23%;">
                                            <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="99%" TabIndex="1" />
                                            <asp:Button Style="display: none" ID="btnSetProject" OnClientClick="getData(this);" runat="server" Text=" Project" />
                                            <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="HParentvWorkSpaceId" runat="server" />
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                                                TargetControlID="txtProject" ServicePath="AutoComplete.asmx" ServiceMethod="GetMyProjectCompletionList"
                                                OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1"
                                                CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1">
                                            </cc1:AutoCompleteExtender>
                                        </td>
                                        <td class="Label" style="text-align: right; width: 7%;">Period :
                                        </td>
                                        <td style="text-align: left; width: 15%;">
                                            <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="dropDownList" Width="57%"
                                                OnSelectedIndexChanged="ddlPeriod_SelectedIndexChanged" AutoPostBack="True" TabIndex="2">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="Label" style="text-align: right; width: 6%;">Activity :
                                        </td>
                                        <td style="text-align: left; width: 25%;">
                                            <asp:DropDownList ID="ddlActivity" runat="server" AutoPostBack="true" CssClass="dropDownList"
                                                Width="87%" TabIndex="3">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;" class="Label">Collection By :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="ddlUser" runat="server" CssClass="dropDownList" Width="100%"
                                                TabIndex="4">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="Label" style="text-align: right;">Deviation :
                                        </td>
                                        <td align="left" class="Label">
                                            <u>+</u><asp:TextBox ID="txtDeviation" runat="server" Width="35%" CssClass="textBox"
                                                onblur="return CheckDeviation();" TabIndex="5" Font-Bold="True" Font-Size="Large"></asp:TextBox>Minutes
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6" style="white-space: nowrap; text-align: center;" class="Label">
                                            <asp:HiddenField ID="hfTextChnaged" runat="server" />
                                            <asp:Button ID="btnSubjectMgmt" runat="server" CssClass="btn btnnew" Text="Subject Mgmt"
                                                ToolTip="Subject Management" TabIndex="6" OnClientClick="return Validate()" />
                                            <%--OnClientClick="return funValidate();"--%>
                                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btnnew" Text="Search" ToolTip="Search"
                                                TabIndex="7" OnClientClick="return CheckBlankField();" />
                                            <asp:Button ID="btnReplace" runat="server" CssClass="btn btnnew" Text="Replace" ToolTip="Replace"
                                                TabIndex="7" OnClientClick="return CheckBlankField();" Visible="False" />
                                            <asp:Button ID="btnExit" runat="server" CssClass="btn btnexit" Text="Exit" ToolTip="Exit"
                                                TabIndex="9" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this);" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </td>
                </tr>
            </table>

        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel runat="server" ID="UpGridSubjectSample" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellpadding="5px" style="width: 100%;">
                <tr>
                    <td>
                        <fieldset id="fsSampleDetail" runat="server" class="FieldSetBox" style="width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                            <legend class="LegendText" style="color: Black; font-size: 12px">
                                <img id="img1" alt="PK Sample Detail" src="images/panelcollapse.png"
                                    onclick="Display(this,'divSampleDetail');" runat="server" style="margin-right: 2px;" />PK Sample Detail</legend>
                            <div id="divSampleDetail">
                                <table style="width: 100%" cellpadding="5px">
                                    <tr>
                                        <td class="Label" style="white-space: nowrap; text-align: right; width: 10%;">BarCode :
                                        </td>
                                        <%--====VB====--%>
                                        <td align="left" class="Label" width="20%">
                                            <asp:TextBox ID="txtScan" runat="server" onpaste="return Validation();" TabIndex="8" AutoPostBack="true" CssClass="textBox" AutoComplete="off" 
                                                Width="100%"></asp:TextBox>
                                        </td>
                                        <td width="25%">Select For Manual Barcode Collection
                                            <asp:CheckBox ID="chkScan" OnCheckedChanged="chkScan_CheckedChanged" ToolTip="Manual Barcode Collection" AutoPostBack="true" runat="server"></asp:CheckBox></td>
                                        <td>
                                            <asp:DropDownList ID="ddlRemark" runat="server" Visible="false" CssClass="dropDownList" AutoPostBack="True">
                                                <asp:ListItem Text="Select Reason" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Vacutainer barcode not scanned" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="I Card barcode not scanned" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Storage tube barcode not scanned" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="IMP dosing unit barcode not scanned " Value="4"></asp:ListItem>
                                                <asp:ListItem Text="Missing barcode" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="Other" Value="6"></asp:ListItem>
                                            </asp:DropDownList>

                                            <asp:TextBox ID="txtOtherRemarks" runat="server" CssClass="textBox" TextMode="MultiLine" Visible="false"
                                                AutoComplete="off" Width="20%" Style="margin-bottom: -28px" ToolTip="Remarks" Height=""></asp:TextBox>
                                        </td>
                                        <%--====VB====--%>
                                    </tr>
                                    <tr>
                                        <td class="Label" style="text-align: left;" colspan="2">
                                            <asp:Label ID="lblSampleId" runat="server" Text="Sample Id :" Style="width: 75px; margin-left: 4.5%;"></asp:Label>
                                            <asp:Label ID="lblSample" runat="server" Text=""></asp:Label>
                                            <asp:Label ID="lblSubjectId" runat="server" Text="Subject Id :"></asp:Label>
                                            <asp:Label ID="lblSubject" runat="server" Text=""></asp:Label>
                                            <asp:Label ID="lblMySubjectNo" runat="server" Text="MySubject No :"></asp:Label>
                                            <asp:Label ID="lblMySubject" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table width="90%" style="margin: auto;">
                                    <tr>
                                        <td>
                                            <asp:Panel ID="pnlGrid" runat="server" ScrollBars="Auto" Style="margin: auto; width: 100%;">
                                                <asp:GridView ID="gvwSubjectSample" runat="server" AutoGenerateColumns="False" Style="margin: auto; width: 100%;">
                                                    <Columns>
                                                        <asp:BoundField DataField="nPKSampleId" HeaderText="nPKSampleId" />
                                                        <asp:BoundField DataField="vPKSampleId" HeaderText="Sample Id" />
                                                        <asp:BoundField DataField="iMySubjectNo" HeaderText="MySubjectNo" />
                                                        <asp:BoundField DataField="vMySubjectNo" HeaderText="MySubjectNo" />
                                                        <asp:BoundField DataField="vSubjectID" HeaderText="Subject ID">
                                                            <ItemStyle Wrap="false" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vInitials" HeaderText="Subject Initials">
                                                            <ItemStyle Wrap="false" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="iRefNodeId" HeaderText="Ref.NodeId" />
                                                        <asp:BoundField DataField="nRefTime" HeaderText="Scheduled Time">
                                                            <ItemStyle Wrap="false" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="dCollectionDateTime" HeaderText="Collection DateTime">
                                                            <ItemStyle Wrap="false" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="iCollectionBy" HeaderText="Collection By" />
                                                        <asp:BoundField DataField="vCollectionBy" HeaderText="Collection By" />
                                                        <asp:BoundField DataField="nRefTime" HeaderText="Dosing Time" />
                                                        <asp:BoundField DataField="vRemark" HeaderText="Replacement Remark" />
                                                        <asp:BoundField DataField="AttendanceMysubNo" HeaderText="AttendanceMysubNo" />
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="lnkReplace" runat="server" ImageUrl="~/Images/replace.png"
                                                                    ToolTip="Replace" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="vNodeDisplayName" HeaderText="Activity Name">
                                                            <HeaderStyle Width="25%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="GunningTypeRemarks" HeaderText="Collection Mode" />
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSearch" />
            <asp:AsyncPostBackTrigger ControlID="gvwSubjectSample" EventName="RowCommand" />
            <asp:AsyncPostBackTrigger ControlID="txtreplaceCode" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="btnReplaceOK" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="ddlActivity" EventName="SelectedIndexChanged" />
            <asp:PostBackTrigger ControlID="txtScan" />
            <asp:PostBackTrigger ControlID="chkScan" />
            <asp:PostBackTrigger ControlID="btnSaveSubject" />

        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdatePanel runat="server" ID="UpOtherControls" UpdateMode="Conditional">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <button id="btnDeviation" runat="server" style="display: none;" />
                        <cc1:ModalPopupExtender ID="MPEDeviation" runat="server" PopupControlID="divDeviation"
                            PopupDragHandleControlID="LblPopUpDeviation" BackgroundCssClass="modalBackground"
                            TargetControlID="btnDeviation" CancelControlID="ImgPopUpCloseDeviation" BehaviorID="MPEDeviation">
                        </cc1:ModalPopupExtender>
                        <div class="modal-content" id="divDeviation" style="display: none;" runat="server">
                            <div class="modal-header">
                                <h2>Deviation Remarks</h2>
                                <asp:ImageButton runat="server" OnClientClick="return resetPopup();" ID="ImgPopUpCloseDeviation"
                                    alt="Close" src="images/Sqclose.gif" Style="position: relative; float: right; right: 5px; Bottom: 45px;" />
                            </div>
                            <div class="modal-body">
                                <asp:Panel ID="PnlDeviation" runat="server" Visible="true">
                                    <table cellpadding="5" style="width: 600px">
                                        <tr>
                                            <td align="center" class="Label" colspan="2" valign="top" style="text-align: left">
                                                <table style="text-align: left;">
                                                    <tr>
                                                        <td style="text-align: right" nowrap="noWrap">Select Remark :
                                                        </td>
                                                        <td style="text-align: left" nowrap="noWrap">
                                                            <asp:DropDownList ID="ddlRemarks" runat="server" Width="225px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right" nowrap="noWrap">Enter Remarks, If Other :
                                                        </td>
                                                        <td style="text-align: left" nowrap="noWrap">
                                                            <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Width="219px" CssClass="textBox"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap="nowrap" style="text-align: right"></td>
                                                        <td nowrap="nowrap" style="text-align: left"></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                            <div class="modal-header">
                                <asp:Button ID="btnOk" runat="server" OnClientClick="return CheckRemarks(this);" CssClass="btn btnnew" TabIndex="11" Text="Ok" />
                                <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" Text="Cancel" />
                            </div>
                        </div>

                        <button id="btnReplacement" runat="server" style="display: none;" />
                        <cc1:ModalPopupExtender ID="MPEReplacement" runat="server" PopupControlID="divReplacement"
                            PopupDragHandleControlID="LblPopUpReplacement" BackgroundCssClass="modalBackground"
                            TargetControlID="btnReplacement" CancelControlID="ImgPopUpCloseReplacement">
                        </cc1:ModalPopupExtender>
                        <div class="modal-content modal-sm" id="divReplacement" style="display: none;" runat="server">
                            <div class="modal-header">
                                <h2>Sample Replacement</h2>
                                <img id="ImgPopUpCloseReplacement" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px; bottom: 45px;" title="Close" /></h1>
                            </div>
                            <div class="modal-body">
                                <asp:Panel ID="pnlReplace" runat="server" Visible="true">
                                    <table cellpadding="5px" style="width: 100%">
                                        <tr>
                                            <td colspan="2">
                                                <table style="width: 100%; text-align: center;">
                                                    <tr>
                                                        <td nowrap="nowrap" style="text-align: right; width: 30%;" class="Label">BarCode :
                                                        </td>
                                                        <td nowrap="nowrap" style="text-align: left">
                                                            <asp:TextBox ID="txtreplaceCode" runat="server" Width="60%" CssClass="textBox" TabIndex="8"
                                                                AutoPostBack="True"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap="nowrap" style="text-align: right;" class="Label">Replace :
                                                        </td>
                                                        <td nowrap="nowrap" style="text-align: left;">
                                                            <asp:Label ID="lblReplaceCode" runat="server"></asp:Label>
                                                            With
                                                            <asp:Label ID="lbReplaceWith" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right;" nowrap="noWrap" class="Label">Remarks :
                                                        </td>
                                                        <td style="text-align: left;" nowrap="noWrap">
                                                            <asp:TextBox ID="txtReplaceRemark" runat="server" TextMode="MultiLine" Width="60%"
                                                                CssClass="textBox"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap="nowrap" style="text-align: center" class="Label" colspan="2"></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnReplaceOK" runat="server" OnClientClick="return CheckReplaceRemarks();" CssClass="btn btnnew" TabIndex="11" Text="Ok" ToolTip="Ok" />
                                <asp:Button ID="btnReplaceCancel" runat="server" CssClass="btn btncancel" Text="Cancel" ToolTip="Cancel" />
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <button id="btnMangament" runat="server" style="display: none;" />
                        <cc1:ModalPopupExtender ID="MPESubMgmt" runat="server" PopupControlID="divMedEx"
                            PopupDragHandleControlID="LblPopUpSubMgmt" BackgroundCssClass="modalBackground"
                            TargetControlID="btnMangament" CancelControlID="ImgPopUpClose" BehaviorID="MPESubMgmt">
                        </cc1:ModalPopupExtender>

                        <div class="modal-content modal-lg" id="divMedEx" style="display: none;" runat="server">
                            <div class="modal-header">
                                <h2>Subject Management</h2>
                                <img id="ImgPopUpClose" alt="Close" title="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px; bottom: 45px;" /></h1>
                            </div>
                            <div class="modal-body">
                                <table style="width: 100%">
                                    <tr>
                                        <td rowspan="2">
                                            <asp:Panel ID="pnlMedExGrid" runat="server" Height="300px" ScrollBars="Auto" Width="100%"
                                                Visible="true" Style="margin: auto">
                                                <asp:GridView ID="gvwSubjects" runat="server" AutoGenerateColumns="False" PageSize="5"
                                                    TabIndex="10" Visible="true" Width="100%" SkinID="grdViewSmlAutoSize">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <input id="chkSelectAll" onclick="SelectAll(this, 'gvwSubjects')" type="checkbox" />
                                                                <asp:Label ID="Label1" runat="server" Text="All"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="ChkMove" Onclick="CheckUncheckAll('gvwSubjects');" runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="vWorkSpaceId" HeaderText="vWorkSpaceId">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="iMySubjectNoNew" HeaderText="iMySubject No.">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vMySubjectNo" HeaderText="My Subject No.">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vSubjectId" HeaderText="Subject Id">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vInitials" HeaderText=" Subject Initials">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="modal-header">
                                <asp:Button ID="btnSaveSubject" OnClientClick="return CheckAtleastOne('<%= gvwSubjects.ClientId %>');"
                                    runat="server" CssClass="btn btnsave" TabIndex="11" Text="Save" ToolTip="Save" />
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="HsubjectId" runat="server" />
            <asp:HiddenField ID="HPendingNode" runat="server" />
            <asp:HiddenField ID="hremark" runat="server" />
        </ContentTemplate>
        <Triggers>
            <%--   <asp:AsyncPostBackTrigger ControlID="btnSubjectMgmt" />--%>
            <asp:AsyncPostBackTrigger ControlID="gvwSubjectSample" EventName="RowCommand" />
            <asp:AsyncPostBackTrigger ControlID="txtreplaceCode" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="btnReplaceOK" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnSubjectMgmt" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnSaveSubject" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Button ID="btnShow" runat="server" Text="Show Dialog" Style="display: none" />
    <cc1:ModalPopupExtender ID="MPEActivitySequence" runat="server" TargetControlID="btnShow"
        PopupControlID="DivPopSequence" BackgroundCssClass="modalBackground" PopupDragHandleControlID="lblHeading"
        BehaviorID="MPEId" CancelControlID="ImgDeviation">
    </cc1:ModalPopupExtender>

    <div class="modal-content modal-lg" id="DivPopSequence" style="display: none;" runat="server">
        <div class="modal-header">
            <h2>Activity Deviation</h2>
            <img id="ImgDeviation" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px; bottom: 45px;" title="Close" />
        </div>
        <div class="modal-body" style="height: 350px">
            <table width="100%">
                <tbody>
                    <tr>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td style="text-align: left;" colspan="2">
                                        <div style="overflow: auto; max-height: 250px; margin: auto; width: 100%;">
                                            <asp:Panel runat="server" ID="Deviation">
                                                <asp:PlaceHolder ID="PlaceDeviation" runat="server"></asp:PlaceHolder>
                                            </asp:Panel>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left">
                                        <asp:Label ID="lbl" runat="server" Text="Do You Want To Continue? "></asp:Label>
                                        <asp:LinkButton ID="lbtnForSub" runat="server" Text="Sequence" Style="display: none;"
                                            onmouseover="funOnMouseOver(this);"></asp:LinkButton>
                                        <label id="btnPforStruct" onclick="open_ProjStruct();" onmouseover="funOnMouseOver(this);">
                                            <u>Structure Management</u></label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div>
                                            <table width="100%">
                                                <tr>
                                                    <td style="text-align: right; width: 10%;" class="Label">
                                                        <asp:Label ID="lblRemark" runat="server" Text="Remarks:  " Style="color: Navy; font-weight: bold;"></asp:Label>
                                                    </td>
                                                    <td style="text-align: left">
                                                        <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" Width="88%"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td align="left" valign="top">
                                                    <div>
                                                        <asp:Label ID="lblError" runat="server" Style="color: Red;"></asp:Label>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="modal-header">
            <asp:Button ID="btn_ok" Text="Ok" CssClass="btn btnnew" runat="server" ToolTip="Ok" OnClientClick="return chk_Remark(this);" />
            <input type="button" id="btn_cancel" value="Cancel" title="Cancel" class="btn btncancel" onclick="unCheckSelected();" />
        </div>
    </div>


    <asp:HiddenField ID="hndLockStatus" runat="server" />

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" src="Script/Gridview.js"></script>

    <script type="text/javascript" src="Script/General.js"></script>

    <script type="text/javascript" language="javascript">
        //==VB==       

        $(document).ready(function () {
            $("#ctl00_CPHLAMBDA_txtScan").on("keypress", function (key) {
                if ($("#ctl00_CPHLAMBDA_chkScan").prop('checked') == false) {
                    if (key.charCode < 48 || key.charCode > 57) {
                        return false;
                    }
                    else if ((key.charCode < 97 || key.charCode > 122) && (key.charCode < 65 || key.charCode > 90) && (key.charCode != 45)) {
                        return false;
                    }
                }
            });
        });
      
        function fnRemarksOtherTextBox() {
            if ($("#ctl00_CPHLAMBDA_ddlRemark").val() == 6) {
                $("#ctl00_CPHLAMBDA_txtOtherRemarks").attr("style", "display:inline;margin-bottom:-28px;")
            }
            else {
                $("#ctl00_CPHLAMBDA_txtOtherRemarks").attr("style", "display:none;margin-bottom:-28px;")
            }
        }

        function Validation() {
            if ($("#ctl00_CPHLAMBDA_chkScan").prop('checked') == true) {
                if ($("#ctl00_CPHLAMBDA_ddlRemark").val() == 0) {
                    msgalert("Please Select Remark !");
                    return false;
                }
                else if ($("#ctl00_CPHLAMBDA_ddlRemark").val() == 6) {
                    if ($("#ctl00_CPHLAMBDA_txtOtherRemarks").val() == "") {
                        msgalert("Please Enter Remark !");
                        ctl00_CPHLAMBDA_txtOtherRemarks.focus();
                        return false;
                    }
                }

                else {
                    return true;
                }


            }
            else {
                msgalert("Please Check Checkbox And Select Reason Then Paste Barcode !");
                return false;
            }
        }
        //==VB==
        function HideSampleDetails() {
            //$('#<%= img2.ClientID%>').click();
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



        function fsSampleDetail_Show() {

            // $('#<%=fsSampleDetail.ClientID%>').attr('style', $('#<%=fsSampleDetail.ClientID%>').attr('style') + ';display:block');
        }

        function UIgvwSubjectSample() {
            $('#<%= gvwSubjectSample.ClientID%>').removeAttr('style', 'display:block');

            oTab = $('#<%= gvwSubjectSample.ClientID%>').prepend($('<thead>').append($('#<%= gvwSubjectSample.ClientID%> tr:first'))).dataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": -1,
                "bProcessing": true,
                "bSort": false,
                "sScrollY": "250px",
                "sScrollX": "100%",
                "bDestroy": true,
                "bScrollCollapse": true,
                aLengthMenu: [
                    [10, 25, 50, 100, -1],
                    [10, 25, 50, 100, "All"]
                ],
            });
            return false;
        }

        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        function Validate() { //Added bY dipen Shah on 11-March-2015.

            if ($('#<%=HProjectId.ClientID%>').val() == "") {
                msgalert("Select Project !");
                return false;
            }
            if ($('#<%=ddlPeriod.ClientID%>').val() == 0) {
                msgalert("Select Period !");
                return false;
            }
            if ($('#<%=ddlActivity.ClientID%>').val() == 0) {
                msgalert("Select Activity !");
                return false;
            }

        }


        function CheckBlankField() {

            if ($('#<%=HProjectId.ClientID%>').val() == "") {
                msgalert("Select Project !");
                return false;
            }
            if ($('#<%=ddlPeriod.ClientID%>').val() == 0) {
                msgalert("Select Period !");
                return false;
            }
            if ($('#<%=ddlActivity.ClientID%>').val() == 0) {
                msgalert("Select Activity !");
                return false;
            }
            if (document.getElementById('<%=txtDeviation.ClientId %>').value.trim() == '') {
                msgalert('Please Enter Deviation !');
                //document.getElementById('<%=txtDeviation.ClientId %>').focus();
                return false;
            }
            return true;
        }

        function CheckDeviation() {
            var txt = document.getElementById('<%=txtDeviation.ClientId %>');
            var val = document.getElementById('<%=txtDeviation.ClientId %>').value.trim();
            if (txt == null || typeof (txt) == 'undefined') {
                return false;
            }
            else if (val == '') {
                msgalert("Please Enter a Deviation !");
                document.getElementById('<%=txtDeviation.ClientId %>').value = '';
                document.getElementById('<%=txtDeviation.ClientId %>').focus();
                return false;
            }
            else if (isNaN(val) == true) {
                msgalert("Please Enter a Numeric Value !");
                document.getElementById('<%=txtDeviation.ClientId %>').value = '';
                document.getElementById('<%=txtDeviation.ClientId %>').focus();
                return false;
            }
    return true;
}


function CheckRemarks(e) {
    var ddl = document.getElementById('<%=ddlRemarks.clientid %>');
    var txt = document.getElementById('<%=txtRemarks.clientid %>').value.trim();
    if (ddl.selectedIndex == 0 && txt == '') {
        msgalert('Either Select Or Enter Remarks !');
        return false;
    }
    $find('MPEDeviation').hide();

    return true;
}


function CheckAtleastOne(gv) {
    var gvwSubject = document.getElementById('<%= gvwSubjects.ClientID %>');

    if (CheckOne(gvwSubject.id) == false) {
        msgalert('Please Select Atleast One Subject !');
        return false;
    }
    return true;
}

function CheckReplaceRemarks() {
    var txt = document.getElementById('<%=txtReplaceRemark.clientid %>').value.trim();
    if (txt == '') {
        msgalert('Please enter Remarks !');
        document.getElementById('<%=txtReplaceRemark.clientid %>').focus();
        return false;
    }
}

function PassTheBarcode() {
    if (document.getElementById('<%=txtScan.clientid %>').value.length > 0) {
      <%-- __doPostBack('ctl00$CPHLAMBDA$txtScan', '');--%>
    }
    return false;
}

function CheckSelected() {

    var Gdv = document.getElementById('<%=gvwSubjects.ClientId %>');
    var str = "";
    for (c = 1; c < Gdv.rows.length - 1; c++)// -1 as gdv row count contains both header and footer
    {
        if (Gdv.rows[c].children[0].children[0].type == "checkbox") {
            if (Gdv.rows[c].children[0].children[0].checked == true) {
                if (str.toString() == "") {
                    str = str + Gdv.rows[c].children[2].innerText;
                }
                else {
                    str = str + "," + Gdv.rows[c].children[2].innerText;
                }
            }
        }
    }
    document.getElementById('<%=HsubjectId.ClientID %>').value = str.toString();

}
function CheckUncheckAll(Grid) {
    var Checkall = document.getElementById('chkSelectAll');
    var Gvd = document.getElementById('<%=gvwSubjects.ClientId %>');
    j = 0;
    for (i = 0; i < document.forms[0].elements.length; i++) {
        if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf(Grid) > -1)) {
            j = j + 1;
            if (document.forms[0].elements[i].checked == false) {
                Checkall.checked = false;
                break;
            }
            else if (document.forms[0].elements[i].checked == true) {
                if (j == Gvd.rows.length - 2) {
                    Checkall.checked = true;
                }
            }
        }
    }
}
function SelectAll(CheckBoxControl, Grid) {
    var str = "";
    var Gvd = document.getElementById('<%=gvwSubjects.ClientId %>');
    if (CheckBoxControl.checked == true) {
        var i;
        var Cell = 0;
        for (i = 0; i < document.forms[0].elements.length; i++) {
            if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf(Grid) > -1)) {
                if (document.forms[0].elements[i].disabled == false) {
                    document.forms[0].elements[i].checked = true;
                    Cell += 1;

                    if (str.toString() == "") {
                        str = str + Gvd.lastChild.childNodes[Cell].children[2].innerText;
                    }
                    else {
                        str = str + "," + Gvd.lastChild.childNodes[Cell].children[2].innerText;
                    }

                }
            }
        }
        document.getElementById('<%=HsubjectId.ClientID %>').value = str.toString();

    }
    else {
        var i;
        for (i = 0; i < document.forms[0].elements.length; i++) {
            if ((document.forms[0].elements[i].type == 'checkbox') && (document.forms[0].elements[i].name.indexOf(Grid) > -1)) {
                document.forms[0].elements[i].checked = false;
            }
        }
    }
}


function chk_Remark(e) {
    var txtContent = document.getElementById('<%= txtContent.ClientID %>').value.toString().trim();

    if (txtContent == "") {
        //document.getElementById('<%=lblError.ClientID %>').innerHTML = "Please Enter Remark";
        msgalert("Please Enter Remark !");
        return false;
    }
    else {
        document.getElementById('<%=hremark.ClientId %>').value = txtContent.toString();
    }

    getData(e.id);

    return true;
}

function unCheckSelected() {
    var Gdv = document.getElementById('<%=gvwSubjects.ClientId %>');
            $find('MPEId').hide();
            $find('MPESubMgmt').show();
            for (c = 1; c < Gdv.rows.length - 1; c++)// -1 as gdv row count contains both header and footer
            {
                if (Gdv.rows[c].childNodes[0].childNodes[0].type == "checkbox") {
                    if (Gdv.rows[c].childNodes[0].childNodes[0].checked == true) {
                        Gdv.rows[c].childNodes[0].childNodes[0].checked = false;
                    }
                }
            }

            document.getElementById('<%=HsubjectId.ClientID %>').value = "";

        }

        function checkActivity() {
            if (document.getElementById('<%=ddlActivity.ClientID %>').selectedIndex <= 0) {
                msgalert("Please select atleast one activity !");
                return false;
            }
            return true;
        }


        function open_ProjStruct() {
            window.open("frmEditWorkspaceNodeDetail.aspx?WorkSpaceId=" + document.getElementById('<%= HProjectId.ClientId%>').value);
        }
        function funOnMouseOver(id) {

            id.style.cursor = 'pointer';
        }

        function pageLoad() {
            //document.getElementById('<%=txtScan.clientid %>').focus();
            UIgvwSubjectSample();
        }

        function resetPopup() {
            $("#ctl00_CPHLAMBDA_ddlRemarks").val(0);
            $("#ctl00_CPHLAMBDA_txtRemarks").val("")
        }

        //Add by shivani pandya for project lock
        function getData(e) {
            var WorkspaceID = $('input[id$=HProjectId]').val();
            $.ajax({
                type: "post",
                url: "frmSampleCollection.aspx/LockImpact",
                data: '{"WorkspaceID":"' + WorkspaceID + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                async: false,
                dataType: "json",
                success: function (data) {
                    if (data.d == "L") {
                        if (e.id == "ctl00_CPHLAMBDA_btnSetProject") {
                            msgalert("Project is locked !");
                        } else {
                            $("#<%=hndLockStatus.ClientID%>").val("Lock");
                        }
                    }
                    if (data.d == "U") {
                        $("#<%=hndLockStatus.ClientID%>").val("UnLock");
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
        function ThemeSelection() {
            if (document.cookie.split(";")[0] == "Theme=Orange") {
                //$("#ctl00_CPHLAMBDA_gvwSubjectSample tr").last().css({ 'background-color': '#CF8E4C' });
            } else if (document.cookie.split(";")[0] == "Theme=Green") {
                //$("#ctl00_CPHLAMBDA_gvwSubjectSample tr").last().css({ 'background-color': '#33a047' });
            } else if (document.cookie.split(";")[0] == "Theme=Demo") {
                //$("#ctl00_CPHLAMBDA_gvwSubjectSample tr").last().css({ 'background-color': '#999966' });
            } else if (document.cookie.split(";")[0] == "Theme=Blue") {
                //$("#ctl00_CPHLAMBDA_gvwSubjectSample tr").last().css({ 'background-color': '#1560a1' });
            }
        }

        jQuery(window).focus(function () {
            ThemeSelection();
            return false;
        });

        $("#ctl00_CPHLAMBDA_txtScan").scannerDetection();
        $("#ctl00_CPHLAMBDA_txtScan").bind('scannerDetectionComplete', function (e, data) {
            $("#ctl00_CPHLAMBDA_txtScan").val(data.string);
        });


    </script>

</asp:Content>
