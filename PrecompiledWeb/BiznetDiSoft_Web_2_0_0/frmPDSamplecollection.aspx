<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmPDSamplecollection, App_Web_l40sj1d0" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">
    <script type="text/javascript" src="Script/scrollablegrid.js"></script>
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
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
                                <asp:Button runat="server" ID="btndefault" Style="display: none" OnClientClick="return PassTheBarcode();" CssClass="btn btnnew" />
                                <table style="width: 100%;" cellpadding="5px">
                                    <tr id="VersionDtl" runat="server" style="display: none;">
                                        <td></td>
                                        <td style="text-align: left;" class="Label">Version :<asp:Label runat="server" ID="VersionNo" Style="padding-right: 10px"></asp:Label>Version
                        Date :<asp:Label ID="VersionDate" Style="padding-right: 10px;" runat="server"></asp:Label>
                                            Status :<img src="~/images/Freeze.jpg" id="ImageLockUnlock" runat="server" alt="Lock" />
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="Label" style="width: 11%; white-space: nowrap; text-align: right;">Project* :
                                        </td>
                                        <td align="left" style="width: 23%;">
                                            <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="99%" TabIndex="1"></asp:TextBox><asp:Button
                                                Style="display: none" ID="btnSetProject" OnClientClick="getData(this);" runat="server" Text=" Project"></asp:Button><asp:HiddenField
                                                    ID="HProjectId" runat="server"></asp:HiddenField>
                                            <asp:HiddenField ID="HFLocalMachineTime" runat="server" />
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                                                TargetControlID="txtProject" ServicePath="AutoComplete.asmx" ServiceMethod="GetMyProjectCompletionList"
                                                OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1"
                                                CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1">
                                            </cc1:AutoCompleteExtender>
                                        </td>
                                        <td class="Label" style="width: 7%; white-space: nowrap; text-align: right;">Period :
                                        </td>
                                        <td style="text-align: left; width: 15%;">
                                            <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="dropDownList" Width="42%"
                                                OnSelectedIndexChanged="ddlPeriod_SelectedIndexChanged" AutoPostBack="True" TabIndex="2">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="Label" style="white-space: nowrap; text-align: right; width: 6%;">Activity :
                                        </td>
                                        <td style="text-align: left; width: 25%;">
                                            <asp:DropDownList ID="ddlActivity" runat="server" AutoPostBack="true" CssClass="dropDownList"
                                                Width="87%" TabIndex="3">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Label" style="text-align: right; white-space: nowrap;">Collection By :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="ddlUser" runat="server" CssClass="dropDownList" Width="100%"
                                                TabIndex="4">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="Label" style="white-space: nowrap; text-align: right;">Deviation :
                                        </td>
                                        <td style="text-align: left;" class="Label">
                                            <u>+</u><asp:TextBox ID="txtDeviation" runat="server" Width="35%" CssClass="textBox"
                                                onblur="return CheckDeviation();" TabIndex="5" Font-Bold="True" Font-Size="Large"></asp:TextBox>Minutes
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6" style="white-space: nowrap; text-align: center;" class="Label">
                                            <asp:HiddenField ID="hfTextChnaged" runat="server" />
                                            <asp:Button ID="btnSubjectMgmt" runat="server" CssClass="btn btnnew" Text="Subject Mgmt"
                                                ToolTip="Subject Management" TabIndex="6" OnClientClick="return Validate()"/>
                                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btnnew" ToolTip="Search" Text="Search"
                                                TabIndex="7" OnClientClick="return CheckBlankField();" />
                                            <asp:Button ID="btnReplace" runat="server" CssClass="btn btnnew" Text="Replace" ToolTip="Replace"
                                                TabIndex="7" OnClientClick="return CheckBlankField();" Visible="False" />
                                            <asp:Button ID="btnExit" runat="server" CssClass="btn btnexit" Text="Exit" ToolTip="Exit"
                                                TabIndex="9" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this); " />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnFasting" runat="server" Text="Show Dialog" Style="display: none" />
                                            <cc1:ModalPopupExtender ID="MPEFasting" runat="server" TargetControlID="btnFasting"
                                                PopupControlID="divFasting" BackgroundCssClass="modalBackground" BehaviorID="MPEFasting">
                                            </cc1:ModalPopupExtender>
                                            <div id="divFasting" style="display: none; width: 40%; max-height: 50%; background-color: White; top: 20%;">
                                                <h1 class="header">Blood Sample Collection Status
                                                </h1>
                                                <table style="text-align: left;">
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButtonList AutoPostBack="true" ID="rblCollectionStatus" runat="server"
                                                                RepeatDirection="Vertical">
                                                                <asp:ListItem Text="Fasting" Value="0">
                                                                </asp:ListItem>
                                                                <asp:ListItem Text="Post Prandial" Value="1">
                                                                </asp:ListItem>
                                                                <asp:ListItem Text="Random" Value="2">
                                                                </asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </td>
                </tr>
            </table>
            <%--<asp:HiddenField ID="HFnSampleId" runat="server" />--%>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel runat="server" ID="UpGridSubjectSample" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellpadding="5px" style="width: 100%;">
                <tr>
                    <td>
                        <fieldset id="fsetPDSample" runat="server" class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                            <legend class="LegendText" style="color: Black; font-size: 12px">
                                <img id="img1" alt="PD Sample Detail" src="images/panelcollapse.png"
                                    onclick="Display(this,'divSampleDetail');" runat="server" style="margin-right: 2px;" />PD Sample Detail</legend>
                            <div id="divSampleDetail">
                                <table style="width: 100%" cellpadding="5px">
                                    <tr>
                                        <td class="Label" style="white-space: nowrap; text-align: right; width: 10%;">BarCode :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox Width="35%" ID="txtScan" runat="server" TabIndex="8" AutoPostBack="true" CssClass="textBox"
                                                Enabled="False"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Label" style="text-align: left;" colspan="2">
                                            <asp:Label ID="lblSampleId" runat="server" Text="Sample Id :" Style="width: 75px; margin-left: 4.5%;"></asp:Label><asp:Label
                                                ID="lblSample" runat="server" Text=""></asp:Label>
                                            <asp:Label ID="lblSubjectId" runat="server" Text="Subject Id :"></asp:Label><asp:Label
                                                ID="lblSubject" runat="server" Text=""></asp:Label>
                                            <asp:Label ID="lblMySubjectNo" runat="server" Text="MySubject No :"></asp:Label><asp:Label
                                                ID="lblMySubject" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table width="90%" style="margin: auto;">
                                    <tr>
                                        <td>
                                            <asp:Panel ID="pnlGrid" runat="server" ScrollBars="Auto" Style="margin: auto; width: 100%;">
                                                <asp:GridView ID="gvwSubjectSample" runat="server" AutoGenerateColumns="False" Width="100%"
                                                    PageSize="25" >
                                                    <%--SkinID="grdViewSmlAutoSize"--%>
                                                    <Columns>
                                                        <asp:BoundField DataField="vSampleId" HeaderText="vSampleId" />
                                                        <asp:BoundField DataField="vSampleBarCode" HeaderText="PDSample Id">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="iMySubjectNo" HeaderText="MySubjectNo" />
                                                        <asp:BoundField DataField="vMySubjectNo" HeaderText="MySubjectNo">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vSubjectID" HeaderText="Subject ID">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vInitials" HeaderText="Subject Initials">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="iRefNodeId" HeaderText="Ref.NodeId" />
                                                        <asp:BoundField DataField="nRefTime" HeaderText="Scheduled Time">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Collection DateTime" DataField="dCollectionDateTime">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="iCollectionBy" HeaderText="Collection By" />
                                                        <asp:BoundField DataField="vCollectionBy" HeaderText="Collection By">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="nRefTime" HeaderText="Dosing Time" />
                                                        <asp:BoundField DataField="nSampleID" HeaderText="SampleID" />
                                                        <asp:BoundField DataField="iMySubjectNoNew" HeaderText="iMySubjectNoNew" />
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
            <asp:HiddenField ID="HFnSampleId" runat="server" />
        </ContentTemplate>
        <Triggers>
            <%--<asp:AsyncPostBackTrigger ControlID="btnSearch" />--%>
            <asp:PostBackTrigger ControlID="btnSearch" />
            <asp:AsyncPostBackTrigger ControlID="gvwSubjectSample" EventName="RowCommand" />
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
                        <div id="divDeviation" runat="server" class="popUpDivNoTop" style="left: 25px; width: 600px; display: none; position: absolute; top: 901px; height: 150px">
                            <asp:Panel ID="PnlDeviation" runat="server" Visible="true">
                                <table cellpadding="5" style="width: 600px">
                                    <tr>
                                        <td align="left" class="Label" style="text-align: center; height: 22px;" valign="top">
                                            <asp:Label ID="lblDeviation" runat="server" class="Label" Visible="true" Text="Deviation Remarks"></asp:Label>
                                        </td>
                                        <td align="right">
                                            <img id="ImgPopUpCloseDeviation" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" class="Label" colspan="2" valign="top" style="text-align: left">
                                            <table style="width: 100%; text-align: left;">
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
                                                    <td nowrap="nowrap" style="text-align: left">
                                                        <asp:Button ID="btnOk" runat="server" OnClientClick="return CheckRemarks();" CssClass="btn btnnew"
                                                            TabIndex="11" Text="Ok" />
                                                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btncancel" Text="Cancel" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
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
                        
                        <div class="modal-content modal-lg" id="divMedEx" style="display:none;" runat="server">
                            <div class="modal-header">
                                <h2>Subject Management</h2>
                                <img id="ImgPopUpClose" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px;bottom:45px;" title="Close" /></h1>
                            </div>
                            <div class="modal-body">
                                <table style="width: 100%">
                                    <tr>
                                        <td rowspan="2">
                                            <asp:Panel ID="pnlMedExGrid" runat="server" Height="300px" ScrollBars="Auto" Width="100%"
                                                Visible="true" Style="margin: auto;">
                                                <asp:GridView ID="gvwSubjects" runat="server" AutoGenerateColumns="False" PageSize="5"
                                                    SkinID="grdViewSmlAutoSize" TabIndex="10" Width="100%" Visible="true">
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
                                <asp:Button ID="btnSaveSubject" OnClientClick="return CheckAtleastOne('<%= gvwSubjects.ClientId %>');" runat="server" CssClass="btn btnsave" TabIndex="11" Text="Save" ToolTip="Save" />
                            </div>
                        </div>

                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="HsubjectId" runat="server" />
            <asp:HiddenField ID="HPendingNode" runat="server" />
            <asp:HiddenField ID="hremark" runat="server" />
            <asp:HiddenField ID="hndLockStatus" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvwSubjectSample" EventName="RowCommand" />
            <asp:AsyncPostBackTrigger ControlID="btnSubjectMgmt" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnOk" />
            <asp:PostBackTrigger ControlID="btnSaveSubject" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Button ID="btnShow" runat="server" Text="Show Dialog" Style="display: none" />
    <cc1:ModalPopupExtender ID="MPEActivitySequence" runat="server" TargetControlID="btnShow"
        PopupControlID="DivPopSequence" BackgroundCssClass="modalBackground" BehaviorID="MPEId">
    </cc1:ModalPopupExtender>
    <div id="DivPopSequence" runat="server" class="centerModalPopup" style="display: none; left: 521px; width: 50%; position: absolute; top: 525px; max-height: 404px;">
        <table width="100%">
            <tr>
                <td class="LabelBold" style="text-align: center; height: 22px;">
                    <h1 class="header">
                        <asp:Label runat="server" ID="lblHeading" Text="Activity Deviation" class="LabelBold" /></h1>
                </td>
            </tr>
        </table>
        <hr />
        <div style="width: 100%; left: 204px; top: 77px" id="divTV" runat="server">
            <table width="100%">
                <tbody>
                    <tr>
                        <td style="width: 452px" valign="top" align="left">
                            <table width="100%">
                                <tr>
                                    <td style="text-align: left;" colspan="2">
                                        <div style="overflow: auto; max-height: 100px; margin: auto; width: 100%;">
                                            <asp:Panel runat="server" ID="Deviation">
                                                <asp:PlaceHolder ID="PlaceDeviation" runat="server"></asp:PlaceHolder>
                                            </asp:Panel>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: left;">
                                        <asp:Label ID="lbl" runat="server" Text="Do You Want To Continue? "></asp:Label>
                                        <asp:LinkButton ID="lbtnForSub" runat="server" Text="Sequence" Style="display: none;"
                                            onmouseover="funOnMouseOver(this);"></asp:LinkButton>
                                        <label id="btnPforStruct" onclick="open_ProjStruct();" onmouseover="funOnMouseOver(this);">
                                            <u>Structure Management</u></label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
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
                                                <td style="text-align: left;" valign="top">
                                                    <div>
                                                        <asp:Label ID="lblError" runat="server" Style="color: Red;"></asp:Label>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: center;" class="Label ">
                                        <asp:Button ID="btn_ok" Text="Ok" ToolTip="Ok" CssClass="btn btnnew" runat="server" OnClientClick="return chk_Remark(this);" />
                                        <input type="button" id="btn_cancel" value="Cancel" title="Cancel" class="btn btncancel"
                                            onclick="unCheckSelected();" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>

   <%-- <script src="Script/jquery-1.7.min.js" type="text/javascript"></script>--%>

    <script type="text/javascript" src="Script/AutoComplete.js"></script>

    <script type="text/javascript" src="Script/Gridview.js"></script>

    <script type="text/javascript" src="Script/General.js"></script>

    <script type="text/javascript" language="javascript">

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



        function fsetPDSample_Show() {
            $('#<%=fsetPDSample.ClientID%>').attr('style', $('#<%=fsetPDSample.ClientID%>').attr('style') + ';display:block');
        }

        function UIgvwSubjectSample() {
            $('#<%= gvwSubjectSample.ClientID%>').removeAttr('style', 'display:block');
            oTab = $('#<%= gvwSubjectSample.ClientID%>').prepend($('<thead>').append($('#<%= gvwSubjectSample.ClientID%> tr:first'))).dataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": 10,
                "bProcessing": true,
                "bSort": false,
                "sScrollY": "250px",
                "sScrollX": "100%",
                "bScrollCollapse": true,
                "bDestroy": true,
                aLengthMenu: [
                    [10, 25, 50, 100, -1],
                    [10, 25, 50, 100, "All"]
                ],
            });
            return false;
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

        $(document).ready(function () {

            $('#<%= txtscan.clientid %>').bind('keypress', function (e) {

                if (e.keyCode == 13) {

                    $('#<%= txtscan.clientid %>').blur();
                    //$find('mpefasting').show();
                    return false;
                }
            });
        });
        function HandleEnter(source) {

        }
        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {
            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        function CheckBlankField() {
            if (document.getElementById('<%=txtDeviation.ClientId %>').value.trim() == '') {
                msgalert('Please Enter Deviation !');
                document.getElementById('<%=txtDeviation.ClientId %>').focus();
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


function CheckRemarks() {
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



function PassTheBarcode() {
    if (document.getElementById('<%=txtScan.clientid %>').value.length > 0) {
        __doPostBack('ctl00$CPHLAMBDA$txtScan', '');
    }
    return false;
}

function CheckSelected() {

    var Gdv = document.getElementById('<%=gvwSubjects.ClientId %>');
    var str = "";
    for (c = 1; c < Gdv.rows.length - 1; c++)// -1 as gdv row count contains both header and footer
    {
        if (Gdv.rows[c].childNodes[0].childNodes[0].type == "checkbox") {
            if (Gdv.rows[c].childNodes[0].childNodes[0].checked == true) {
                if (str.toString() == "") {
                    str = str + Gdv.rows[c].childNodes[2].innerText;
                }
                else {
                    str = str + "," + Gdv.rows[c].childNodes[2].innerText;
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
        document.getElementById('<%=lblError.ClientID %>').innerHTML = "Please Enter Remark";
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
function fun_closeMPEFasting() {
    $.find('MPEFasting').hide();
}
function pageLoad() {
    document.getElementById('<%=txtScan.ClientID%>').focus();
    UIgvwSubjectSample();
}
//Add by shivani pandya for project lock
function getData(e) {
    var WorkspaceID = $('input[id$=HProjectId]').val();
    $.ajax({
        type: "post",
        url: "frmIPAdministration.aspx/LockImpact",
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
    </script>

</asp:Content>
