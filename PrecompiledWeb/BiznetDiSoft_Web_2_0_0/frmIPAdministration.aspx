<%@ page language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmIPAdministration, App_Web_mlepfeoz" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" runat="Server">

    <style type="text/css">
        #txtScan {
            text-transform: uppercase;
        }

        .paging_full_numbers .ui-button {
            padding: 2px 6px;
            margin: 0;
            cursor: pointer;
            * cursor: hand;
        }

        [valign~=top] {
            background-color: #5999c3!important;
            color: #FFF;
        }
    </style>
    <asp:UpdatePanel runat="server" ID="UpControls" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellpadding="5px" style="width: 100%;">
                <tr>
                    <td>
                        <fieldset class="FieldSetBox" style="display: block; width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                            <legend class="LegendText" style="color: Black; font-size: 12px">
                                <img id="img2" alt="IMP Details" src="images/panelcollapse.png"
                                    onclick="Display(this,'divIMPDetail');" runat="server" style="margin-right: 2px;" />IMP Details</legend>
                            <div id="divIMPDetail">
                                <asp:Button runat="server" ID="btndefault" Style="display: none" OnClientClick="return PassTheBarcode();" CssClass="btn btnnew" />
                                <table style="width: 100%" cellpadding="5px">
                                    <tr id="VersionDtl" class="Label" runat="server" style="display: none; text-align: left;">
                                        <td colspan="6" style="text-align: left; width: 30%; padding-left: 15.5%;">version :<asp:Label runat="server" ID="VersionNo" Style="padding-right: 10px"></asp:Label>Version
                        Date :<asp:Label ID="VersionDate" Style="padding-right: 10px;" runat="server"></asp:Label>
                                            Status:<img src="images/Freeze.jpg" id="ImageLockUnlock" runat="server" alt="Lock" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="Label" style="white-space: nowrap; text-align: right; width: 15%;">Project* :
                                        </td>
                                        <td style="text-align: left; width: 23%;">
                                            <asp:TextBox ID="txtproject" runat="server" CssClass="textBox" Width="97%" TabIndex="1"></asp:TextBox>
                                            <asp:Button Style="display: none" ID="btnSetProject" OnClientClick="getData(this);" runat="server" Text=" Project"></asp:Button>
                                            <asp:HiddenField ID="HProjectId" runat="server"></asp:HiddenField>
                                            <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" UseContextKey="True"
                                                TargetControlID="txtProject" ServicePath="AutoComplete.asmx" ServiceMethod="GetMyProjectCompletionList"
                                                OnClientShowing="ClientPopulated" OnClientItemSelected="OnSelected" MinimumPrefixLength="1"
                                                CompletionListItemCssClass="autocomplete_listitem" CompletionListHighlightedItemCssClass="autocomplete_highlighted_listitem"
                                                CompletionListCssClass="autocomplete_list" BehaviorID="AutoCompleteExtender1"
                                                CompletionListElementID="pnlProjectList">
                                            </cc1:AutoCompleteExtender>
                                            <asp:Panel ID="pnlProjectList" runat="server" Style="max-height: 200px; overflow: auto; overflow-x: hidden" />
                                        </td>
                                        <td class="Label" style="white-space: nowrap; text-align: right; width: 19%;">Period :
                                        </td>
                                        <td style="text-align: left; width: 12%;">
                                            <asp:DropDownList ID="ddlPeriod" runat="server" CssClass="dropDownList" Width="97%"
                                                OnSelectedIndexChanged="ddlPeriod_SelectedIndexChanged" AutoPostBack="True" TabIndex="2">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="Label" style="white-space: nowrap; text-align: right; width: 9%;">Activity :
                                        </td>
                                        <td style="text-align: left; width: 22%;">
                                            <asp:DropDownList ID="ddlActivity" runat="server" CssClass="dropDownList" Width="70%"
                                                TabIndex="3" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Label" style="white-space: nowrap; text-align: right;">Dosing Supervisor :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="ddlDosingSupervisor" runat="server" CssClass="dropDownList"
                                                Width="98%" TabIndex="4">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="Label" style="text-align: right; white-space: nowrap;">ML Of Water Administered With IP :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtWaterQuantity" runat="server" Width="94%" CssClass="textBox"
                                                TabIndex="5"></asp:TextBox>
                                        </td>
                                        <td class="Label" style="white-space: nowrap; text-align: right;">Doser Name :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="ddlDoserName" runat="server" CssClass="dropDownList"
                                                Width="70%" TabIndex="4">
                                                <asp:ListItem>--Select--</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Label" style="white-space: nowrap; text-align: right;">Dosing Day :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="ddlDosingDay" runat="server" CssClass="dropDownList" Width="98%">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="Label" style="white-space: nowrap; text-align: right;">Dosing No :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="ddlDosingNo" runat="server" CssClass="dropDownList" Width="97%">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center" class="Label" colspan="6">
                                            <asp:HiddenField ID="hfTextChnaged" runat="server" />
                                            <asp:Button ID="btnSubjectMgmt" runat="server" CssClass="btn btnnew" Text="Subject Mgmt"
                                                ToolTip="Subject Management" TabIndex="6" Enabled="false" />
                                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btnnew" Text="Search" ToolTip="Search"
                                                TabIndex="7" OnClientClick="return CheckBlankField();" />
                                            <asp:Button ID="btnReplace" runat="server" CssClass="btn btnnew" Text="Replace" TabIndex="8"
                                                OnClientClick="return CheckBlankField();" Visible="False" ToolTip="Replace" />
                                            <asp:Button ID="btnExit" runat="server" CssClass="btn btnexit" Text="Exit" ToolTip="Exit"
                                                TabIndex="9" OnClientClick="return msgconfirmalert('Are you sure you want to Exit?',this); " />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </td>
                </tr>
            </table>

            <table width="100%">
                <tr>
                    <td>
                        <button id="btnSubjectManagement" runat="server" style="display: none;" cssclass="btn btnnew" />
                        <cc1:ModalPopupExtender ID="MPESubjectManagement" runat="server" PopupControlID="divSubjectManagement"
                            BackgroundCssClass="modalBackground" TargetControlID="btnSubjectManagement" CancelControlID="imgSubjectManagement"
                            BehaviorID="MPESubjectManagement1">
                        </cc1:ModalPopupExtender>

                        <div class="modal-content modal-lg" id="divSubjectManagement" style="display: none;" runat="server">
                            <div class="modal-header">
                                <h2>Subject Management</h2>
                                <img id="imgSubjectManagement" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px; bottom: 45px;" title="Close" /></h1>
                            </div>
                            <div class="modal-body" style="overflow: auto; width: 96%;">
                                <table style="width: 100%">
                                    <tr>
                                        <td>
                                            <asp:Panel ID="pnlMedExGrid" runat="server" Height="300px" ScrollBars="Auto" Width="100%"
                                                Style="margin: auto;">
                                                <asp:GridView ID="gvwSubjects" runat="server" AutoGenerateColumns="False" PageSize="5"
                                                    SkinID="grdViewDocs" TabIndex="10" Width="100%" Visible="true">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <input id="chkSelectAll" onclick="SelectAll(this, 'gvwSubjects')" type="checkbox" />
                                                                <asp:Label ID="Label1" runat="server" Text="All"></asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="ChkMove" runat="server" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="vWorkSpaceId" HeaderText="vWorkSpaceId">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="iMySubjectNoNew" HeaderText="iMySubjectNo.">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vMySubjectNo" HeaderText="MySubject No.">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vSubjectId" HeaderText="Subject Id">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20%" />
                                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="vInitials" HeaderText="Subject Initials">
                                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />
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
                                <asp:Button ID="btnClose" runat="server" CssClass="btn btnclose" Text="Close" ToolTip="Close" />
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <button id="btnSubjectCompliant" runat="server" style="display: none;" cssclass="btn btnnew" />
                        <cc1:ModalPopupExtender ID="MPESubjectCompliant" runat="server" PopupControlID="t123"
                            PopupDragHandleControlID="lblSubjectCompliant" BackgroundCssClass="modalBackground"
                            TargetControlID="btnSubjectCompliant" CancelControlID="imgSubjectCompliant">
                        </cc1:ModalPopupExtender>

                        <div class="modal-content modal-sm" id="t123" style="display: none;" runat="server">
                            <div class="modal-body">
                                <table width="100%">
                                    <tr style="width: 100%">
                                        <td align="left" class="Label" style="width: 35%; white-space: nowrap;">
                                            <asp:Label runat="server" ID="lblSubjectCompliant" class="LabelBold" Text="Is subject compliant to all predose requirements as per protocol?"
                                                CssClass="Label" />
                                        </td>
                                        <td align="right">
                                            <img id="imgSubjectCompliant" alt="Close" runat="server" onclick=" return ResetIPAndSubjectID()" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:RadioButtonList ID="rbllstCompiance" runat="server" RepeatDirection="Horizontal"
                                                OnClick=" return SetOrOpenPopUp();">
                                                <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                <asp:ListItem Value="N">No</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="modal-header">
                                <asp:Button ID="btnSaveRemarks" runat="server" Text="Confirm" CssClass="btn btnsave" 
                                    OnClientClick="return ValidationForRemark();" ToolTip="Confirm"></asp:Button>  
                            </div>
                        </div>

                    </td>
                </tr>
                <tr>
                    <td>
                        <button id="btnReplacement" runat="server" style="display: none;" cssclass="btn btnnew" />
                        <cc1:ModalPopupExtender ID="MPEReplacement" runat="server" PopupControlID="divReplacement"
                            BackgroundCssClass="modalBackground" TargetControlID="btnReplacement" CancelControlID="imgReplacement">
                        </cc1:ModalPopupExtender>

                        <div class="modal-content modal-sm" id="divReplacement" style="display: none;" runat="server">
                            <div class="modal-header">
                                <h2>IPLabel Replacement</h2>
                                <img id="imgReplacement" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px; bottom: 45px;" title="Close" />
                            </div>
                            <div class="modal-body">
                                <table style="width: 100%; text-align: left;">
                                    <tr>
                                        <td nowrap="nowrap" style="text-align: right; width: 35%">BarCode :
                                        </td>
                                        <td nowrap="nowrap" style="text-align: left;">
                                            <asp:TextBox ID="txtreplaceCode" runat="server" CssClass="textBox" TabIndex="8" AutoPostBack="True"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap="nowrap" style="text-align: right;">Replace :
                                        </td>
                                        <td nowrap="nowrap" style="text-align: left;">
                                            <asp:Label ID="lblReplaceCode" runat="server"></asp:Label>
                                            With
                                            <asp:Label ID="lbReplaceWith" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;" nowrap="noWrap">Remarks :
                                        </td>
                                        <td style="text-align: left;" nowrap="noWrap">
                                            <asp:TextBox ID="txtReplaceRemark" runat="server" TextMode="MultiLine" Width="219px"
                                                CssClass="textBox"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="modal-header">
                                <asp:Button ID="btnReplaceOK" runat="server" OnClientClick="return CheckReplaceRemarks();" CssClass="btn btnnew" TabIndex="11" Text="Ok" ToolTip="Ok" />
                                <asp:Button ID="btnReplaceCancel" runat="server" CssClass="btn btncancel" Text="Cancel" ToolTip="Cancel" />
                            </div>
                        </div>

                    </td>
                </tr>
                <tr>
                    <td>
                        <button id="Btn3" runat="server" style="display: none;" cssclass="btn btnnew" />
                        <cc1:ModalPopupExtender ID="MpeDialogNoWhenComplaince" runat="server" PopupControlID="DivPopUp"
                            PopupDragHandleControlID="LblPopUpTitleWorkSummary" BackgroundCssClass="modalBackground"
                            TargetControlID="btn3" CancelControlID="ImgPopUp">
                        </cc1:ModalPopupExtender>

                        <div class="modal-content modal-sm" id="DivPopUp" style="display: none;" runat="server">
                            <div class="modal-header">
                                <h2>Give Comment</h2>
                                <img id="ImgPopUp" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px; bottom: 45px" />
                            </div>
                            <div class="modal-body">
                                <table width="100%">
                                    <tr>
                                        <td align="right">Comment :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtCommnetWhenComplianceNo" runat="server" Text="" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="modal-header">
                                <asp:Button ID="BtnSaveCommentsWhenComplianceNo" Text="Save" runat="server" CssClass="btn btnsave" OnClientClick="return SaveValueInGrid();" />
                                <asp:Button ID="BtnSaveWhenYesInCompliance" Text="Cancel" runat="server" CssClass="btn btncancel" OnClientClick="return AssignRemarks();" />
                                <asp:Button ID="BtnSaveAfterGridFromJavaScript" Text="Save" runat="server" CssClass="btn btnsave" Style="display: none;" />
                                <asp:HiddenField ID="HdFieldFoundRow" runat="server" Value="" />
                                <asp:HiddenField ID="HdFieldRemarks" runat="server" Value="" />
                                <asp:HiddenField ID="HdFieldSubjectID" runat="server" Value="" />
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <button id="BtnMouthCheck" runat="server" style="display: none;" cssclass="btn btnnew" />
                        <cc1:ModalPopupExtender ID="MpeMouthCheckDone" runat="server" PopupControlID="DivPopUpMouthCheckDone"
                            PopupDragHandleControlID="LblHeaderPopUpMouthCheckDone" BackgroundCssClass="modalBackground"
                            TargetControlID="BtnMouthCheck" CancelControlID="ImgCloseMouthCheckDone">
                        </cc1:ModalPopupExtender>
                        <div class="modal-content modal-sm" id="DivPopUpMouthCheckDone" style="display: none;" runat="server">
                            <div class="modal-header" style="height: 40px;">
                                <h2></h2>
                                <img id="ImgCloseMouthCheckDone" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px; bottom: 10px;" onclick="return CloseModalPopup();" />
                                <asp:Button ID="btnCloseMouthCheckDone" runat="server" Style="display: none" CssClass="btn btnclose" />
                            </div>
                            <div class="modal-body">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="LblHeaderPopUpMouthCheckDone" runat="server" class="LabelBold" Visible="true"
                                                Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">Mouth Check Done?
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="RblMouthCheckDone" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="Y" Selected="True">Yes</asp:ListItem>
                                                <asp:ListItem Value="N">No</asp:ListItem>
                                                <asp:ListItem Value="NA">NA</asp:ListItem>

                                            </asp:RadioButtonList>
                                            <asp:Button ID="BtnAfterMouthCheckDoneRadioSelected" Style="display: none;" runat="server" CssClass="btn btnnew" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="BtnOkWhileMouthCheckDone" runat="server" Text="OK" OnClientClick=" return SetPopUpIfNo();" CssClass="btn btnnew" ToolTip="OK" />
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnCancelDosing" />
            <asp:AsyncPostBackTrigger ControlID="gvwSubjectSample" EventName="RowCommand" />
            <asp:AsyncPostBackTrigger ControlID="txtreplaceCode" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="btnReplaceOK" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdatePanel runat="server" ID="UpGridSubjectSample" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellpadding="5px" style="width: 100%;">
                <tr>
                    <td>
                        <fieldset id="fsBarcode" runat="server" class="FieldSetBox" style="width: 96%; margin: auto; text-align: left; border: #aaaaaa 1px solid;">
                            <legend class="LegendText" style="color: Black; font-size: 12px">
                                <img id="img1" alt="Barcode Details" src="images/panelcollapse.png"
                                    onclick="Display(this,'divBarcodeDetail');" runat="server" style="margin-right: 2px;" />Barcode Details</legend>
                            <div id="divBarcodeDetail">
                                <table style="width: 100%" cellpadding="5px">
                                    <tr>
                                        <td class="Label" style="white-space: nowrap; text-align: right; width: 10%;">Barcode :
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtScan" runat="server" CssClass="textBox" Enabled="False" AutoPostBack="true" OnChange="return CheckValidation();"
                                                onblur="" BorderStyle="Solid" Width="30%" onkeyup="this.value=this.value.toUpperCase()"></asp:TextBox>
                                            <asp:Button ID="BtnCancelDosing" CssClass="btn btncancel" runat="server" Text="Cancel" ToolTip="Cancel"
                                                Visible="true" />
                                            <asp:HiddenField ID="HdFieldValidationTrueOrFalse" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Label" style="text-align: left;" colspan="2">
                                            <asp:Label ID="lblIPLabel" runat="server" Text="IPLabel Id :" Style="width: 75px; margin-left: 4.5%;"></asp:Label>
                                            <asp:Label ID="lblIPLabelID" runat="server"></asp:Label>
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
                                            <asp:GridView ID="gvwSubjectSample" runat="server" AutoGenerateColumns="False" Style="margin: auto; width: 100%;">
                                                <Columns>
                                                    <asp:BoundField HeaderText="#">
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vDosingBarCode" HeaderText="IPLabelId">
                                                        <ItemStyle Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="iMySubjectNo" HeaderText="iMySubjectNo" />
                                                    <asp:BoundField DataField="vMySubjectNo" HeaderText="MySubject No">
                                                        <ItemStyle Width="5%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vSubjectID" HeaderText="Subject ID">
                                                        <ItemStyle Width="20%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="FullName" HeaderText="Subject Name">
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="dDosedOnDatetime" HeaderText="Dosing Time">
                                                        <ItemStyle Width="20%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="iDosedBy" HeaderText="Dosing By">
                                                        <ItemStyle Width="5%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vDoserName" HeaderText="Doser Name">
                                                        <ItemStyle Width="5%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="iDosingSupervisor" HeaderText="iSupervisor">
                                                        <ItemStyle Width="5%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vSupervisorName" HeaderText="Supervisor" />
                                                    <asp:BoundField DataField="vWaterAdministered" HeaderText="Water Administered">
                                                        <ItemStyle Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="vRemarks" HeaderText="Remark">
                                                        <ItemStyle Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="nDosingDetailNo" HeaderText="DosingDetailNo" />
                                                    <asp:TemplateField HeaderText="Mouth check Done?">
                                                        <ItemTemplate>
                                                            <asp:RadioButtonList ID="rblMouthchk" runat="server" AutoPostBack="False" RepeatDirection="Horizontal">
                                                                <asp:ListItem Selected="True" Value="Y">Yes</asp:ListItem>
                                                                <asp:ListItem Value="N">NO</asp:ListItem>
                                                                <asp:ListItem Value="NA">NA</asp:ListItem>

                                                            </asp:RadioButtonList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Remarks">
                                                        <ItemTemplate>
                                                            <asp:TextBox runat="server" Enabled="false" ID="txtRemark" Text="" CssClass="textbox"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Edit" SortExpression="status">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgEdit" runat="server" OnClientClick="GetValidationForBarcode(this)" ToolTip="Edit" ImageUrl="~/images/Edit2.gif" />
                                                            <asp:ImageButton ID="ImgSave" runat="server" ToolTip="Save" ImageUrl="~/images/save.gif" />
                                                            <asp:ImageButton ID="ImgCancel" runat="server" ToolTip="Cancel" ImageUrl="~/images/Cancel.gif" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="lnkReplace" runat="server" ToolTip="Replace" ImageUrl="~/Images/replace.png" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                        <asp:HiddenField ID="HdFieldForSelectedGridIndex" runat="server" />
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
            <asp:AsyncPostBackTrigger ControlID="btnCloseMouthCheckDone" EventName="click" />
            <asp:AsyncPostBackTrigger ControlID="BtnSaveAfterGridFromJavaScript" />
            <asp:AsyncPostBackTrigger ControlID="BtnAfterMouthCheckDoneRadioSelected" />
            <asp:AsyncPostBackTrigger ControlID="gvwSubjectSample" EventName="RowCommand" />
            <asp:AsyncPostBackTrigger ControlID="txtreplaceCode" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="btnReplaceOK" EventName="Click" />
            <asp:PostBackTrigger ControlID="btnSaveSubject" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Button ID="btnShow" runat="server" Text="Show Dialog" Style="display: none" CssClass="btn btnnew" />
    <cc1:ModalPopupExtender ID="MPEActivitySequence" runat="server" TargetControlID="btnShow"
        PopupControlID="DivPopSequence" BackgroundCssClass="modalBackground" BehaviorID="MPEId" CancelControlID="ImgDeviation">
    </cc1:ModalPopupExtender>

    <div class="modal-content modal-lg" id="DivPopSequence" style="display: none;" runat="server">
        <div class="modal-header">
            <h2>Activity Deviation</h2>
            <img id="ImgDeviation" alt="Close" src="images/Sqclose.gif" style="position: relative; float: right; right: 5px; bottom: 45px;" title="Close" />
        </div>
        <div class="modal-body" style="height: 350px;">
            <table width="100%">
                <tbody>
                    <tr>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <div style="overflow: auto; max-height: 280px; margin: auto; width: 100%;">
                                            <asp:Panel runat="server" ID="Deviation">
                                                <asp:PlaceHolder ID="PlaceDeviation" runat="server"></asp:PlaceHolder>
                                            </asp:Panel>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: left;">
                                        <asp:Label ID="lbl" runat="server" Text="Do You Want To Continue? "></asp:Label>
                                        <asp:LinkButton ID="lbtnForSub" runat="server" Text="Sequence" Style="display: none;"></asp:LinkButton>
                                        <label id="btnPforStruct" onclick="open_ProjStruct();" onmouseover="funOnMouseOver(this);">
                                            <u>Structure Management</u></label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div>
                                            <table width="100%">
                                                <tr>
                                                    <td style="text-align: right; width: 10%;">
                                                        <asp:Label ID="lblRemark" runat="server" Text="Remarks:  " Style="color: Navy; font-weight: bold;"></asp:Label>
                                                    </td>
                                                    <td style="text-align: left;">
                                                        <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" Width="88%"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <%--<tr>
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
                                </tr>--%>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="modal-header">
            <asp:Button ID="btnOk" Text="Ok" ToolTip="Ok" CssClass="btn btnnew" runat="server" OnClientClick="return chk_Remark(this);" />
            <input type="button" id="btnCancel" title="Cancel" value="Cancel" class="btn btncancel" onclick="unCheckSelected();" />
        </div>
    </div>

    <asp:HiddenField ID="HsubjectId" runat="server" />
    <asp:HiddenField ID="HPendingNode" runat="server" />
    <asp:HiddenField ID="hremark" runat="server" />
    <asp:HiddenField ID="hndLockStatus" runat="server" />
    <asp:Button runat="server" Style="display: none" ID="btnDisplayNone" OnClick="btnDisplayNone_Click" CssClass="btn btnnew" />
    <script type="text/javascript" src="Script/AutoComplete.js"></script>
    <script type="text/javascript" src="Script/Gridview.js"></script>
    <script type="text/javascript" src="Script/General.js"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/scrollablegrid.js"></script>

    <script type="text/javascript" language="javascript">

        function HideIMPDetails() {
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

        function fsBarcode() {
            $('#<%=fsBarcode.ClientID%>').attr('style', $('#<%=fsBarcode.ClientID%>').attr('style') + ';display:block');
        }

        function UIgvwSubjectSample() {
            $('#<%= gvwSubjectSample.ClientID%>').removeAttr('style', 'display:block');
            oTab = $('#<%= gvwSubjectSample.ClientID%>').prepend($('<thead>').append($('#<%= gvwSubjectSample.ClientID%> tr:first'))).dataTable({
                "bJQueryUI": true,
                "sPaginationType": "full_numbers",
                "bLengthChange": true,
                "iDisplayLength": -1,//==VB==
                "bProcessing": true,
                "bSort": false,
                "bDestroy": true,
                // "sScrollY": "250px",
                // "sScrollX": "100%",
                "bScrollCollapse": true,
                aLengthMenu: [
                    [10, 25, 50, 100, -1],
                    [10, 25, 50, 100, "All"]
                ],
            });
            return false;
        }

        jQuery(window).focus(function () {
            ThemeSelection();
            return false;
        });

        function open_ProjStruct() {
            window.open("frmEditWorkspaceNodeDetail.aspx?WorkSpaceId=" + document.getElementById('<%= HProjectId.ClientId%>').value);
        }

        function ClientPopulated(sender, e) {
            ProjectClientShowing('AutoCompleteExtender1', $get('<%= txtProject.ClientId %>'));
        }

        function OnSelected(sender, e) {

            ProjectOnItemSelected(e.get_value(), $get('<%= txtProject.clientid %>'),
            $get('<%= HProjectId.clientid %>'), document.getElementById('<%= btnSetProject.ClientId %>'));
        }

        function CheckBlankField() {
            if ($('#' + '<%=txtproject.ClientID%>').val() == "") {
                msgalert("Enter Project !")
                $('#' + '<%=txtproject.ClientID%>').focus();
                return false;
            }
            if ($('#' + '<%=ddlPeriod.ClientID%>').val() == 0) {
                msgalert("Select Period !");
                $('#' + '<%=ddlPeriod.ClientID%>').focus();
                return false;
            }
            if ($('#' + '<%=ddlActivity.ClientID%>').val() == 0) {
                msgalert("Select Activity !");
                $('#' + '<%=ddlActivity.ClientID%>').focus();
                return false;
            }
            return true;
        }

        function ShowHideDivcompliance(type) {
            if (type == 'H') {
                document.getElementById('<%=t123.clientid %>').style.display = 'none';
            }
            else if (type == 'S') {
                document.getElementById('<%=t123.clientid %>').style.display = 'block';
                SetCenter('<%=t123.clientid %>');
            }
    }

    function CheckAtleastOne(gv) {
        var gvwSubject = document.getElementById('<%= gvwSubjects.ClientID %>');

        if (CheckOne(gvwSubject.id) == false) {
            msgalert('Please Select Atleast One Subject !');
            return false;
        }
        return true;
    }

    function ResetIPAndSubjectID() {
        document.getElementById('<%=lblSubject.clientid %>').innerHTML = '';
        document.getElementById('<%=lblMySubject.ClientID%>').innerHTML = '';

        document.getElementById('<%=lblSubject.clientid %>').value = '';
        // document.getElementById('<%=lblSubjectId.ClientID%>').innerHTML = '';
        document.getElementById('<%=lblMySubject.ClientID%>').value = '';
        //document.getElementById('<%=txtScan.ClientID()%>').value = '';

        // alert('Please Select any one Compliant');
        var btn = document.getElementById('<%=btnDisplayNone.clientid %>');
        btn.click()
        return false;
    }

    function SetOrOpenPopUp() {

        var popupclose = $find('ctl00_CPHLAMBDA_MPESubjectCompliant');
        var UpDivNoTop1;
        var radio = document.getElementById('<%=rbllstCompiance.clientid %>').getElementsByTagName('input');

        for (var i = 0; i < radio.length; i++) {
            if (radio[i].checked) {

                if (radio[i].value == 'Y') {
                    popupclose.hide();
                    document.getElementById('<%=BtnSaveWhenYesInCompliance.clientid %>').click();
                    $('#<%=txtScan.ClientID%>').val("");
                    return true;
                }
                else {
                    popupclose.hide();
                    var popupcontrol = $find('ctl00_CPHLAMBDA_MpeDialogNoWhenComplaince');
                    UpDivNoTop1 = document.getElementById('<%=t123.clientid %>');
                    UpDivNoTop1.style.display = 'none';
                    popupcontrol.show();
                    return true;
                }
            }
        }
        return false;
    }

    function OpenPopUpMouthCheckDone() {
        var popupcontrol = document.getElementById('<%=MpeMouthCheckDone.clientid %>')
        UpDivNoTop1 = document.getElementById('<%=t123.clientid %>');
        UpDivNoTop1.style.display = 'none';
        popupcontrol.show();
    }

    function AssignRemarks() {

        var gridname = document.getElementById('<%=gvwSubjectSample.clientid %>');
        var txtname = document.getElementById('<%=txtscan.clientid %>');

        for (var i = 0; i <= gridname.rows.length - 1; i++) {
            if (gridname.rows[i].cells[3].innerText.trim() == txtname.value.trim()) {

                document.getElementById('<%=HdFieldRemarks.clientid %>').value = gridname.rows[i].cells[8].children[0].value.trim();
            }
        }
        //$('#<%=txtScan.ClientID%>').val("");
        return true;

    }


    function SaveValueInGrid() {
        var gridname = document.getElementById('<%=gvwSubjectSample.clientid %>');
        var txtname = document.getElementById('<%=txtscan.clientid %>');
        var txtremark = document.getElementById('<%=TxtCommnetWhenComplianceNo.clientid %>');

        for (var i = 0; i <= gridname.rows.length - 1; i++) {
            if (gridname.rows[i].cells[3].innerText.trim() == txtname.value.trim()) {
                var k;
                k = i + 1;
                if (k <= 9) {
                    var txtMakeName = 'ctl00_CPHLAMBDA_gvwSubjectSample_ctl0' + k + '_txtRemark';
                }
                else {
                    var txtMakeName = 'ctl00_CPHLAMBDA_gvwSubjectSample_ctl' + k + '_txtRemark';
                }

                $get(txtMakeName).value = txtremark.value;
                var popupcontrol = $find('ctl00_CPHLAMBDA_MpeDialogNoWhenComplaince');
                popupcontrol.hide();
                document.getElementById('<%=HdFieldFoundRow.clientid %>').value = i;
                document.getElementById('<%=HdFieldSubjectID.clientid %>').value = txtname.value;
                document.getElementById('<%=HdFieldRemarks.clientid %>').value = txtremark.value;
                txtremark.value = '';
                document.getElementById('<%=BtnSaveAfterGridFromJavaScript.clientid %>').click();
                $('#<%=txtScan.ClientID%>').val("");               
                return false;
            }
        }
        return false;
    }

    function getGridSelectedRowIndexThroughSubjectId() {
        var gridname = document.getElementById('<%=gvwSubjectSample.clientid %>');
        var txtname = document.getElementById('<%=txtscan.clientid %>');
        for (var i = 0; i <= gridname.rows.length - 1; i++) {
            if (gridname.rows[i].cells[3].innerText.trim() == txtname.value) {
                var k;
                document.getElementById('<%=HdFieldForSelectedGridIndex.clientid %>').value = k;
            }
        }
    }

    function PassTheBarcode() {
        if (document.getElementById('<%=txtscan.clientid %>').value.length > 0) {
            CheckValidation();
        }
        return false;
    }

    function ValidationForRemark() {
        $("#ctl00_CPHLAMBDA_txtScan").val("");
        $("#ctl00_CPHLAMBDA_HdFieldValidationTrueOrFalse").val("false");
    }

    function CheckValidation() {
        var gridname = document.getElementById('<%=gvwSubjectSample.clientid %>');
        var txtname = document.getElementById('<%=txtscan.clientid %>');
        var hdFieldTrueOrFalse = document.getElementById('<%=HdFieldValidationTrueOrFalse.clientid %>');
        var doser = document.getElementById('<%=ddlDoserName.ClientID%>').selectedIndex;

        if (doser == 0) {
            msgalert('Please Select Doser Name !')
            txtname.value = '';
            txtname.focus();
            return false;
        }

        var IsValidSubject = false;
        var IsValidLabel = false;
        if (txtname.value.indexOf('-') >= 0) {
            for (var i = 0; i <= gridname.rows.length - 1; i++) {
                if (gridname.rows[i].cells[3].innerText.trim() == txtname.value.trim()) {
                    IsValidSubject = true;
                    if (gridname.rows[i].cells[4].innerText.trim() != '') {
                        hdFieldTrueOrFalse.value = 'false';
                        msgalert('Subject Is Already Dosed !');
                        txtname.value = '';
                        txtname.focus();
                        return false;
                    }
                    else {
                        hdFieldTrueOrFalse.value = 'true';
                        __doPostBack('ctl00$CPHLAMBDA$txtScan', '');   //  Commented by Rahul Rupareliya due to twise POst back  While gunning 
                    }
                }
            }
            if (IsValidSubject == false) {
                msgalert('Subject Is Not Valid For You !');
                txtname.value = '';
                txtname.focus();
                return false;
            }
        }
        else {
            for (var i = 0; i <= gridname.rows.length - 1; i++) {
                if (gridname.rows[i].cells[1].innerText == txtname.value) {
                    IsValidLabel = true;
                    if (gridname.rows[i].cells[4].innerText.trim() != '') {

                        msgalert('Dosing Label Is Already Collected !');
                        //hdFieldTrueOrFalse.value = 'false';
                        txtname.value = '';
                        txtname.focus();
                        return false;
                    }
                    else {
                        hdFieldTrueOrFalse.value = 'true';
                        __doPostBack('ctl00$CPHLAMBDA$txtScan', '');  //  Commented by Rahul Rupareliya due to twise POst back  While gunning 
                    }
                }
            }
            if (IsValidLabel == false) {
                msgalert('Dosing Label Is Not Valid For You !');
                txtname.value = '';
                txtname.focus();
                return false;
            }
        }
    }

    function SetPopUpIfNo() {
        var UpDivNoTop1;
        var radio = document.getElementById('<%=rblMouthCheckDone.clientid %>').getElementsByTagName('input');
        var DivCompliance;
        var btn = document.getElementById('<%=BtnAfterMouthCheckDoneRadioSelected.clientid %>');
        for (var i = 0; i < radio.length; i++) {
            if (radio[i].checked) {
                btn.click();
                return false;
            }
        }
    }


    function CloseModalPopup() {
        document.getElementById('<%=btnCloseMouthCheckDone.ClientID%>').click();
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
        $find('MPESubjectManagement1').show();
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
    function funOnMouseOver(id) {

        id.style.cursor = 'pointer';
    }
    function CheckReplaceRemarks() {
        var txt = document.getElementById('<%=txtReplaceRemark.clientid %>').value.trim();
        if (txt == '') {
            msgalert('Please Enter Remarks !');
            document.getElementById('<%=txtReplaceRemark.clientid %>').focus();
                return false;
            }
            return true;
        }

        function pageLoad() {
            // document.getElementById('<%=txtScan.clientid %>').focus();
        UIgvwSubjectSample();
    }

    function GetValidationForBarcode(e) {
        var rowId = $(e).attr("editrow")
        if ($("#ctl00_CPHLAMBDA_gvwSubjectSample [DosingTime" + rowId + "=no]").html() == "&nbsp;" || $("#ctl00_CPHLAMBDA_gvwSubjectSample [DosingName" + rowId + "=no]").html() == "&nbsp;") {
            msgalert("Subject Is Not Dosed.You Can Not Enter Remarks !");
            return false;
        }
        return false;
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

    function RefreshScan() {
        $('#<%=txtScan.ClientID%>').val("");

        }

    </script>

</asp:Content>
